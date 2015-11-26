/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.LocalService
{

    /// <summary>
    /// A simple router to dispatch incoming requests to different service
    /// implementations. The SessionId is used as a minimal state and routing
    /// key to avoid flooding.
    /// </summary>
    public class RequestRouter : IAuthServices,
                                 IRemoteStartStop
    {

        #region Data

        private readonly Dictionary<UInt32,             IAuthServices>     AuthenticationServices;
        private readonly Dictionary<ChargingSession_Id, IAuthServices>     SessionIdAuthenticatorCache;
        private readonly Dictionary<EVSEOperator_Id,    IRemoteStartStop>  EVSEOperatorLookup;

        #endregion

        #region Properties

        #region RoamingNetwork

        private readonly RoamingNetwork_Id _RoamingNetwork;

        public RoamingNetwork_Id RoamingNetwork
        {
            get
            {
                return _RoamingNetwork;
            }
        }

        #endregion

        #region AuthorizatorId

        private readonly Authorizator_Id _AuthorizatorId;

        public Authorizator_Id AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion


        #region ChargingReservations

        private readonly Dictionary<ChargingReservation_Id, ChargingReservation> _ChargingReservations;

        public IEnumerable<ChargingReservation> ChargingReservations
        {
            get
            {
                return _ChargingReservations.Select(kvp => kvp.Value);
            }
        }

        #endregion



        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AllTokens
        {
            get
            {
                return AuthenticationServices.SelectMany(vv => vv.Value.AllTokens);
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AuthorizedTokens
        {
            get
            {
                return AuthenticationServices.SelectMany(vv => vv.Value.AuthorizedTokens);
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> NotAuthorizedTokens
        {
            get
            {
                return AuthenticationServices.SelectMany(vv => vv.Value.NotAuthorizedTokens);
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> BlockedTokens
        {
            get
            {
                return AuthenticationServices.SelectMany(vv => vv.Value.BlockedTokens);
            }
        }

        #endregion

        #region Events

        #region OnRemoteStart

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartDelegate OnRemoteStart;

        #endregion

        #region OnRemoteStop

        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopDelegate OnRemoteStop;

        #endregion

        #region OnFilterCDRRecords

        public delegate SENDCDRResult OnFilterCDRRecordsDelegate(Authorizator_Id AuthorizatorId, AuthInfo AuthInfo, ChargingSession_Id PartnerSessionId);

        /// <summary>
        /// An event fired whenever a CDR needs to be filtered.
        /// </summary>
        public event OnFilterCDRRecordsDelegate OnFilterCDRRecords;

        #endregion


        #region OnChargingPoolAdminDiff

        public delegate void OnChargingPoolAdminDiffDelegate(ChargingPoolAdminStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a charging station admin status diff was received.
        /// </summary>
        public event OnChargingPoolAdminDiffDelegate OnChargingPoolAdminDiff;

        #endregion

        #region OnChargingStationAdminDiff

        public delegate void OnChargingStationAdminDiffDelegate(ChargingStationAdminStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a charging station admin status diff was received.
        /// </summary>
        public event OnChargingStationAdminDiffDelegate OnChargingStationAdminDiff;

        #endregion

        #region OnEVSEStatusDiff

        public delegate void OnEVSEStatusDiffDelegate(EVSEStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a EVSE status diff was received.
        /// </summary>
        public event OnEVSEStatusDiffDelegate OnEVSEStatusDiff;

        #endregion

        #endregion

        #region Constructor(s)

        public RequestRouter(RoamingNetwork_Id  RoamingNetwork,
                             Authorizator_Id    AuthorizatorId  = null)
        {

            this._RoamingNetwork              = RoamingNetwork;
            this._AuthorizatorId              = (AuthorizatorId == null) ? Authorizator_Id.Parse("GraphDefined E-Mobility Gateway") : AuthorizatorId;
            this.AuthenticationServices       = new Dictionary<UInt32,                 IAuthServices>();
            this.SessionIdAuthenticatorCache  = new Dictionary<ChargingSession_Id,     IAuthServices>();
            this.EVSEOperatorLookup           = new Dictionary<EVSEOperator_Id,        IRemoteStartStop>();

            this._ChargingReservations        = new Dictionary<ChargingReservation_Id, ChargingReservation>();

        }

        #endregion


        #region RegisterAuthService(Priority, AuthenticationService)

        public Boolean RegisterAuthService(UInt32         Priority,
                                           IAuthServices  AuthenticationService)
        {

            lock (AuthenticationServices)
            {

                if (!AuthenticationServices.ContainsKey(Priority))
                {
                    AuthenticationServices.Add(Priority, AuthenticationService);
                    return true;
                }

                return false;

            }

        }

        #endregion


        #region AuthorizeStart(OperatorId, AuthToken, EVSEId = null, SessionId = null, PartnerProductId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<AUTHSTARTResult>>

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId            = null,
                           ChargingSession_Id  SessionId         = null,
                           ChargingProduct_Id  PartnerProductId  = null,
                           ChargingSession_Id  PartnerSessionId  = null,
                           TimeSpan?           QueryTimeout      = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            // Will store the SessionId in order to contact the right authenticator at later requests!

            lock (AuthenticationServices)
            {

                AUTHSTARTResult AuthStartResult;

                foreach (var AuthenticationService in AuthenticationServices.
                                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
                {

                    var _Task = AuthenticationService.AuthorizeStart(OperatorId,
                                                                     AuthToken,
                                                                     EVSEId,
                                                                     SessionId,
                                                                     PartnerProductId,
                                                                     PartnerSessionId,
                                                                     QueryTimeout);

                    _Task.Wait();
                    AuthStartResult = _Task.Result.Content;

                    #region Authorized

                    if (AuthStartResult.AuthorizationResult == AuthorizationResult.Authorized)
                    {

                        // Store the upstream SessionId and its AuthenticationService!
                        // Will be deleted when the CDRecord was sent!
                        SessionIdAuthenticatorCache.Add(AuthStartResult.SessionId, AuthenticationService);

                        return new HTTPResponse<AUTHSTARTResult>(_Task.Result.HttpResponse,
                                                                 AuthStartResult);

                    }

                    #endregion

                    #region Blocked

                    else if (AuthStartResult.AuthorizationResult == AuthorizationResult.Blocked)
                        return new HTTPResponse<AUTHSTARTResult>(_Task.Result.HttpResponse,
                                                                 AuthStartResult);

                    #endregion

                }

                #region ...else fail!

                return new HTTPResponse<AUTHSTARTResult>(new HTTPResponse(),
                                                         new AUTHSTARTResult(AuthorizatorId) {
                                                             AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                                             PartnerSessionId     = PartnerSessionId,
                                                             Description          = "No authorization service returned a positiv result!"
                                                         });

                #endregion

            }

        }

        #endregion

        #region AuthorizeStop(OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<AUTHSTOPResult>>

            AuthorizeStop(EVSEOperator_Id      OperatorId,
                          ChargingSession_Id   SessionId,
                          Auth_Token           AuthToken,
                          EVSE_Id              EVSEId            = null,   // OICP v2.0: Optional
                          ChargingSession_Id   PartnerSessionId  = null,   // OICP v2.0: Optional [50]
                          TimeSpan?            QueryTimeout      = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException("SessionId",  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            lock (AuthenticationServices)
            {

                #region An authenticator was found for the upstream SessionId!

                IAuthServices AuthenticationService;

                if (SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
                {

                    var _Task = AuthenticationService.AuthorizeStop(OperatorId, SessionId, AuthToken, EVSEId, PartnerSessionId);
                    _Task.Wait();

                    if (_Task.Result.Content.AuthorizationResult == AuthorizationResult.Authorized ||
                        _Task.Result.Content.AuthorizationResult == AuthorizationResult.Blocked)
                        return _Task.Result;

                }

                #endregion

                #region Try to find anyone who might kown anything about the given SessionId!

                foreach (var OtherAuthenticationService in AuthenticationServices.
                                                               OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                               Select (AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                               ToArray())
                {

                    var _Task = OtherAuthenticationService.AuthorizeStop(OperatorId,
                                                                         SessionId,
                                                                         AuthToken,
                                                                         EVSEId,
                                                                         PartnerSessionId,
                                                                         QueryTimeout);
                    _Task.Wait();

                    if (_Task.Result.Content.AuthorizationResult == AuthorizationResult.Authorized ||
                        _Task.Result.Content.AuthorizationResult == AuthorizationResult.Blocked)
                        return _Task.Result;

                }

                #endregion

                #region ...else fail!

                return new HTTPResponse<AUTHSTOPResult>(new HTTPResponse(),
                                                        new AUTHSTOPResult(AuthorizatorId) {
                                                            AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                                            PartnerSessionId     = PartnerSessionId,
                                                            Description          = "No authorization service returned a positiv result!"
                                                        });

                #endregion

            }

        }

        #endregion

        #region SendChargeDetailRecord(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, AuthToken = null, eMAId = null, PartnerSessionId = null, ..., QueryTimeout = null)

        /// <summary>
        /// Create a SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId"></param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthToken">An optional (RFID) user identification.</param>
        /// <param name="eMAId">An optional e-Mobility account identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<SENDCDRResult>>

            SendChargeDetailRecord(EVSE_Id              EVSEId,
                                   ChargingSession_Id   SessionId,
                                   ChargingProduct_Id   PartnerProductId,
                                   DateTime             SessionStart,
                                   DateTime             SessionEnd,
                                   AuthInfo             AuthInfo,
                                   ChargingSession_Id   PartnerSessionId      = null,
                                   DateTime?            ChargingStart         = null,
                                   DateTime?            ChargingEnd           = null,
                                   Double?              MeterValueStart       = null,
                                   Double?              MeterValueEnd         = null,
                                   IEnumerable<Double>  MeterValuesInBetween  = null,
                                   Double?              ConsumedEnergy        = null,
                                   String               MeteringSignature     = null,
                                   HubOperator_Id       HubOperatorId         = null,
                                   EVSP_Id              HubProviderId         = null,
                                   TimeSpan?            QueryTimeout          = null)

        {

            #region Initial checks

            if (EVSEId           == null)
                throw new ArgumentNullException("EVSEId",            "The given parameter must not be null!");

            if (SessionId        == null)
                throw new ArgumentNullException("SessionId",         "The given parameter must not be null!");

            if (PartnerProductId == null)
                throw new ArgumentNullException("PartnerProductId",  "The given parameter must not be null!");

            if (SessionStart     == null)
                throw new ArgumentNullException("SessionStart",      "The given parameter must not be null!");

            if (SessionEnd       == null)
                throw new ArgumentNullException("SessionEnd",        "The given parameter must not be null!");

            if (AuthInfo         == null)
                throw new ArgumentNullException("AuthInfo",          "The given parameter must not be null!");

            #endregion

            lock (AuthenticationServices)
            {

                #region Some CDR should perhaps be filtered...

                var OnFilterCDRRecordsLocal = OnFilterCDRRecords;
                if (OnFilterCDRRecordsLocal != null)
                {

                    var _SENDCDRResult = OnFilterCDRRecordsLocal(AuthorizatorId, AuthInfo, PartnerSessionId);

                    if (_SENDCDRResult != null)
                        return new HTTPResponse<SENDCDRResult>(new HTTPResponse(),
                                                               _SENDCDRResult);

                }

                #endregion

                IAuthServices  AuthenticationService;

                #region An authenticator was found for the upstream SessionId!

                if (SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
                {

                    var _Task = AuthenticationService.SendChargeDetailRecord(EVSEId,
                                                                             SessionId,
                                                                             PartnerProductId,
                                                                             SessionStart,
                                                                             SessionEnd,
                                                                             AuthInfo,
                                                                             PartnerSessionId,
                                                                             ChargingStart,
                                                                             ChargingEnd,
                                                                             MeterValueStart,
                                                                             MeterValueEnd,
                                                                             MeterValuesInBetween,
                                                                             ConsumedEnergy,
                                                                             MeteringSignature,
                                                                             HubOperatorId,
                                                                             HubProviderId,
                                                                             QueryTimeout);

                    _Task.Wait();

                    if (_Task.Result.Content.State == SENDCDRState.Forwarded)
                    {
                        SessionIdAuthenticatorCache.Remove(SessionId);
                        return _Task.Result;
                    }

                }

                #endregion

                #region Try to find anyone who might kown anything about the given SessionId!

                foreach (var OtherAuthenticationService in AuthenticationServices.
                                                               OrderBy(v => v.Key).
                                                               Select(v => v.Value).
                                                               ToArray())
                {

                    var _Task = OtherAuthenticationService.SendChargeDetailRecord(EVSEId,
                                                                                  SessionId,
                                                                                  PartnerProductId,
                                                                                  SessionStart,
                                                                                  SessionEnd,
                                                                                  AuthInfo,
                                                                                  PartnerSessionId,
                                                                                  ChargingStart,
                                                                                  ChargingEnd,
                                                                                  MeterValueStart,
                                                                                  MeterValueEnd,
                                                                                  MeterValuesInBetween,
                                                                                  ConsumedEnergy,
                                                                                  MeteringSignature,
                                                                                  HubOperatorId,
                                                                                  HubProviderId,
                                                                                  QueryTimeout);

                    _Task.Wait();

                    if (_Task.Result.Content.State == SENDCDRState.Forwarded)
                    {
                        SessionIdAuthenticatorCache.Remove(SessionId);
                        return _Task.Result;
                    }

                }

                #endregion

                #region ...else fail!

                return new HTTPResponse<SENDCDRResult>(new HTTPResponse(),
                                                       new SENDCDRResult(AuthorizatorId) {
                                                           State             = SENDCDRState.False,
                                                           PartnerSessionId  = PartnerSessionId,
                                                           Description       = "No authorization service returned a positiv result!"
                                                       });

                #endregion

            }

        }

        #endregion


        #region RemoteStart(Timestamp, RoamingNetworkId, SessionId, PartnerSessionId, ProviderId, eMAId, EVSEId, ChargingProductId)

        ///// <summary>
        ///// Initiate a remote start of the given charging session at the given EVSE
        ///// and for the given Provider/eMAId.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp of the request.</param>
        ///// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
        ///// <param name="SessionId">The unique identification for this charging session.</param>
        ///// <param name="PartnerSessionId">The unique identification for this charging session on the partner side.</param>
        ///// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        ///// <param name="eMAId">The unique identification of the e-mobility account.</param>
        ///// <param name="EVSEId">The unique identification of an EVSE.</param>
        ///// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
        ///// <returns>A remote start result object.</returns>
        //public RemoteStartResult RemoteStart(DateTime            Timestamp,
        //                                     RoamingNetwork_Id   RoamingNetworkId,
        //                                     ChargingSession_Id  SessionId,
        //                                     ChargingSession_Id  PartnerSessionId,
        //                                     EVSP_Id             ProviderId,
        //                                     eMA_Id              eMAId,
        //                                     EVSE_Id             EVSEId,
        //                                     ChargingProduct_Id  ChargingProductId)
        //{

        //    lock (AuthenticationServices)
        //    {

        //        var OnRemoteStartLocal = OnRemoteStart;
        //        if (OnRemoteStartLocal != null)
        //        {

        //            return OnRemoteStartLocal(Timestamp,
        //                                      RoamingNetworkId,
        //                                      SessionId,
        //                                      PartnerSessionId,
        //                                      ProviderId,
        //                                      eMAId,
        //                                      EVSEId,
        //                                      ChargingProductId);

        //        }

        //        return RemoteStartResult.Error;

        //    }

        //}

        #endregion

        #region RemoteStop(Timestamp, RoamingNetworkId, SessionId, PartnerSessionId, ProviderId, EVSEId)

        ///// <summary>
        ///// Initiate a remote stop of the given charging session at the given EVSE.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp of the request.</param>
        ///// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
        ///// <param name="SessionId">The unique identification for this charging session.</param>
        ///// <param name="PartnerSessionId">The unique identification for this charging session on the partner side.</param>
        ///// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        ///// <param name="EVSEId">The unique identification of an EVSE.</param>
        ///// <returns>A remote stop result object.</returns>
        //public RemoteStopResult RemoteStop(DateTime            Timestamp,
        //                                   RoamingNetwork_Id   RoamingNetworkId,
        //                                   ChargingSession_Id  SessionId,
        //                                   ChargingSession_Id  PartnerSessionId,
        //                                   EVSP_Id             ProviderId,
        //                                   EVSE_Id             EVSEId)
        //{

        //    lock (AuthenticationServices)
        //    {

        //        var OnRemoteStopLocal = OnRemoteStop;
        //        if (OnRemoteStopLocal != null)
        //            return OnRemoteStopLocal(Timestamp,
        //                                     RoamingNetworkId,
        //                                     SessionId,
        //                                     PartnerSessionId,
        //                                     ProviderId,
        //                                     EVSEId);

        //        return RemoteStopResult.Error;

        //    }

        //}

        #endregion


        #region SendChargingPoolAdminStatusDiff(StatusDiff)

        public void SendChargingPoolAdminStatusDiff(ChargingPoolAdminStatusDiff StatusDiff)
        {

            lock (AuthenticationServices)
            {

                var OnChargingPoolAdminDiffLocal = OnChargingPoolAdminDiff;
                if (OnChargingPoolAdminDiffLocal != null)
                    OnChargingPoolAdminDiffLocal(StatusDiff);

                //return RemoteStartResult.Error;

            }

            //return StatusDiff;

        }

        #endregion

        #region SendChargingStationAdminStatusDiff(StatusDiff)

        public void SendChargingStationAdminStatusDiff(ChargingStationAdminStatusDiff StatusDiff)
        {

            lock (AuthenticationServices)
            {

                var OnChargingStationAdminDiffLocal = OnChargingStationAdminDiff;
                if (OnChargingStationAdminDiffLocal != null)
                    OnChargingStationAdminDiffLocal(StatusDiff);

                //return RemoteStartResult.Error;

            }

            //return StatusDiff;

        }

        #endregion

        #region SendEVSEStatusDiff(StatusDiff)

        public void SendEVSEStatusDiff(EVSEStatusDiff StatusDiff)
        {

            lock (AuthenticationServices)
            {

                var OnEVSEStatusDiffLocal = OnEVSEStatusDiff;
                if (OnEVSEStatusDiffLocal != null)
                    OnEVSEStatusDiffLocal(StatusDiff);

                //return RemoteStartResult.Error;

            }

            //return StatusDiff;

        }

        #endregion




        #region SendReserveEVSE(...)

        public async Task<ReservationResult> SendReserveEVSE(CancellationToken       CancellationToken,
                                                             DateTime                Timestamp,
                                                             ChargingReservation_Id  ReservationId,
                                                             DateTime?               StartTime,
                                                             TimeSpan?               Duration,
                                                             EVSP_Id                 ProviderId,
                                                             ChargingPool_Id         ChargingPoolId,
                                                             ChargingStation_Id      ChargingStationId,
                                                             EVSE_Id                 EVSEId,
                                                             ChargingProduct_Id      ChargingProductId)
        {

            if (_ChargingReservations.ContainsKey(ReservationId))
                return new ReservationResult(ReservationResultType.ReservationId_AlreadyInUse);

            var MaxDuration = TimeSpan.FromMinutes(15);

            var Reservation = _ChargingReservations.
                                  AddAndReturnValue(ReservationId,
                                                    new ChargingReservation(Timestamp,
                                                                            ReservationId,
                                                                            ProviderId,
                                                                            StartTime.HasValue                           ? StartTime.Value : DateTime.Now,
                                                                            Duration. HasValue && Duration < MaxDuration ? Duration. Value : MaxDuration,
                                                                            ChargingPoolId,
                                                                            ChargingStationId,
                                                                            EVSEId,
                                                                            ChargingProductId));

            return new ReservationResult(ReservationResultType.Success,
                                         Reservation);

        }

        #endregion


    }

}
