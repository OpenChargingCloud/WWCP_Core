/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 HTTP <http://www.github.com/eMI3/HTTP>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.ConsoleLog;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;

#endregion

namespace org.GraphDefined.WWCP.LocalService
{

    /// <summary>
    /// A simple router to dispatch incoming requests to different service
    /// implementations. The SessionId is used as a minimal state and routing
    /// key to avoid flooding.
    /// </summary>
    public class RequestRouter : IRoamingProviderProvided_EVSEOperatorServices,
                                 IEVSEOperatorProvidedServices
    {

        #region Data

        private readonly Dictionary<UInt32,             IRoamingProviderProvided_EVSEOperatorServices>   AuthenticationServices;
        private readonly Dictionary<ChargingSession_Id, IRoamingProviderProvided_EVSEOperatorServices>   SessionIdAuthenticatorCache;
        private readonly Dictionary<EVSEOperator_Id,    IEVSEOperatorProvidedServices>                   EVSEOperatorLookup;

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

        #region DNSClient

        private readonly DNSClient _DNSClient;

        /// <summary>
        /// The default server name.
        /// </summary>
        public DNSClient DNSClient
        {
            get
            {
                return _DNSClient;
            }
        }

        #endregion

        #endregion

        #region Events

        #region OnRemoteStart

        public delegate RemoteStartResult OnRemoteStartDelegate(EVSE_Id EVSEId, String SessionId, EVSP_Id ProviderId, eMA_Id eMAId, EventTracking_Id EventTrackingId = null);

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartDelegate OnRemoteStart;

        #endregion

        #region OnRemoteStop

        public delegate RemoteStopResult OnRemoteStopDelegate(EVSE_Id EVSEId, String SessionId, EVSP_Id ProviderId, EventTracking_Id EventTrackingId = null);

        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopDelegate OnRemoteStop;

        #endregion

        #endregion

        #region Constructor(s)

        public RequestRouter(RoamingNetwork_Id  RoamingNetwork,
                             Authorizator_Id    AuthorizatorId  = null,
                             DNSClient          DNSClient       = null)
        {

            this._RoamingNetwork              = RoamingNetwork;
            this._AuthorizatorId              = (AuthorizatorId == null) ? Authorizator_Id.Parse("eMI3 Local E-Mobility Gateway") : AuthorizatorId;
            this.AuthenticationServices       = new Dictionary<UInt32,             IRoamingProviderProvided_EVSEOperatorServices>();
            this.SessionIdAuthenticatorCache  = new Dictionary<ChargingSession_Id, IRoamingProviderProvided_EVSEOperatorServices>();
            this.EVSEOperatorLookup           = new Dictionary<EVSEOperator_Id,    IEVSEOperatorProvidedServices>();
            this._DNSClient                   = DNSClient != null ? DNSClient : new DNSClient();

        }

        #endregion


        #region RegisterService(Priority, AuthenticationService)

        public Boolean RegisterService(UInt32                                         Priority,
                                       IRoamingProviderProvided_EVSEOperatorServices  AuthenticationService)
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


        #region AuthorizeStart(OperatorId, EVSEId, PartnerSessionId, UID)

        public AUTHSTARTResult AuthorizeStart(EVSEOperator_Id    OperatorId,
                                              EVSE_Id            EVSEId,
                                              ChargingSession_Id  PartnerSessionId,
                                              Auth_Token              UID)
        {

            // Will store the SessionId in order to contact the right authenticator at later requests!

            lock (AuthenticationServices)
            {

                AUTHSTARTResult AuthStartResult;

                foreach (var AuthenticationService in AuthenticationServices.
                                                          OrderBy(v => v.Key).
                                                          Select(v => v.Value))
                {

                    AuthStartResult = AuthenticationService.AuthorizeStart(OperatorId, EVSEId, PartnerSessionId, UID);

                    #region Authorized

                    if (AuthStartResult.AuthorizationResult == AuthorizationResult.Authorized)
                    {

                        // Store the upstream SessionId and its AuthenticationService!
                        // Will be deleted when the CDRecord was sent!
                        SessionIdAuthenticatorCache.Add(AuthStartResult.SessionId, AuthenticationService);

                        return AuthStartResult;

                    }

                    #endregion

                    #region Blocked

                    else if (AuthStartResult.AuthorizationResult == AuthorizationResult.Blocked)
                        return AuthStartResult;

                    #endregion

                }

                #region ...else fail!

                return new AUTHSTARTResult(AuthorizatorId) {
                    AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    PartnerSessionId     = PartnerSessionId,
                    Description          = "No authorization service returned a positiv result!"
                };

                #endregion

            }

        }

        #endregion

        #region AuthorizeStop(OperatorId, EVSEId, SessionId, PartnerSessionId, UID)

        public AUTHSTOPResult AuthorizeStop(EVSEOperator_Id    OperatorId,
                                            EVSE_Id            EVSEId,
                                            ChargingSession_Id  SessionId,
                                            ChargingSession_Id  PartnerSessionId,
                                            Auth_Token              UID)
        {

            lock (AuthenticationServices)
            {

                AUTHSTOPResult         AuthStopResult;
                IRoamingProviderProvided_EVSEOperatorServices  AuthenticationService;

                #region An authenticator was found for the upstream SessionId!

                if (SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
                {

                    AuthStopResult = AuthenticationService.AuthorizeStop(OperatorId, EVSEId, SessionId, PartnerSessionId, UID);

                    if (AuthStopResult.AuthorizationResult == AuthorizationResult.Authorized ||
                        AuthStopResult.AuthorizationResult == AuthorizationResult.Blocked)
                        return AuthStopResult;

                }

                #endregion

                #region Try to find anyone who might kown anything about the given SessionId!

                foreach (var OtherAuthenticationService in AuthenticationServices.
                                                               OrderBy(v => v.Key).
                                                               Select(v => v.Value).
                                                               ToArray())
                {

                    AuthStopResult = OtherAuthenticationService.AuthorizeStop(OperatorId, EVSEId, SessionId, PartnerSessionId, UID);

                    if (AuthStopResult.AuthorizationResult == AuthorizationResult.Authorized ||
                        AuthStopResult.AuthorizationResult == AuthorizationResult.Blocked)
                        return AuthStopResult;

                }

                #endregion

                #region ...else fail!

                return new AUTHSTOPResult(AuthorizatorId) {
                    AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    PartnerSessionId     = PartnerSessionId,
                    Description          = "No authorization service returned a positiv result!"
                };

                #endregion

            }

        }

        #endregion

        #region SendCDR(EVSEId, SessionId, PartnerSessionId, PartnerProductId, ChargeStart, ChargeEnd, UID = null, eMAId = null, SessionStart = null, SessionEnd = null, MeterValueStart = null, MeterValueEnd = null)

        public SENDCDRResult SendCDR(EVSE_Id             EVSEId,
                                     ChargingSession_Id  SessionId,
                                     ChargingSession_Id  PartnerSessionId,
                                     String              PartnerProductId,
                                     DateTime            ChargeStart,
                                     DateTime            ChargeEnd,
                                     Auth_Token          UID             = null,
                                     eMA_Id              eMAId           = null,
                                     DateTime?           SessionStart    = null,
                                     DateTime?           SessionEnd      = null,
                                     Double?             MeterValueStart = null,
                                     Double?             MeterValueEnd   = null)

        {

            lock (AuthenticationServices)
            {

                #region Filter ChargeNow/BMW CDRecords

                if ((UID   != null && (UID.ToString() == "5C037451" ||
                                       UID.ToString() == "5ABCC451" ||
                                       UID.ToString() == "5AC18451" ||
                                       UID.ToString() == "54266451" ||
                                       UID.ToString() == "5C8AC451")) ||

                    (eMAId != null && (eMAId.ToString() == "DE*BMW*0010LT*7" ||
                                       eMAId.ToString() == "DE*BMW*0010LX*7" ||
                                       eMAId.ToString() == "DE*BMW*0010LY*3" ||
                                       eMAId.ToString() == "DE*BMW*0010LZ*X" ||
                                       eMAId.ToString() == "DE*BMW*0010M0*2")))

                    return new SENDCDRResult(AuthorizatorId) {
                        State             = SENDCDRState.NotForwared,
                        PartnerSessionId  = PartnerSessionId,
                        Description       = "This ChargeDetailRecord will not be forwarded because of administrative filter rules!"
                    };

                #endregion

                SENDCDRResult                                  SENDCDRResult;
                IRoamingProviderProvided_EVSEOperatorServices  AuthenticationService;

                #region An authenticator was found for the upstream SessionId!

                if (SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
                {

                    SENDCDRResult = AuthenticationService.SendCDR(EVSEId,
                                                                  SessionId,
                                                                  PartnerSessionId,
                                                                  PartnerProductId,
                                                                  ChargeStart,
                                                                  ChargeEnd,
                                                                  UID,
                                                                  eMAId,
                                                                  SessionStart,
                                                                  SessionEnd,
                                                                  MeterValueStart,
                                                                  MeterValueEnd);

                    if (SENDCDRResult.State == SENDCDRState.Forwarded)
                    {

                        SessionIdAuthenticatorCache.Remove(SessionId);

                        return SENDCDRResult;

                    }

                }

                #endregion

                #region Try to find anyone who might kown anything about the given SessionId!

                foreach (var OtherAuthenticationService in AuthenticationServices.
                                                               OrderBy(v => v.Key).
                                                               Select(v => v.Value).
                                                               ToArray())
                {

                    SENDCDRResult = OtherAuthenticationService.SendCDR(EVSEId,
                                                                       SessionId,
                                                                       PartnerSessionId,
                                                                       PartnerProductId,
                                                                       ChargeStart,
                                                                       ChargeEnd,
                                                                       UID,
                                                                       eMAId,
                                                                       SessionStart,
                                                                       SessionEnd,
                                                                       MeterValueStart,
                                                                       MeterValueEnd);

                    if (SENDCDRResult.State == SENDCDRState.Forwarded)
                    {

                        SessionIdAuthenticatorCache.Remove(SessionId);

                        return SENDCDRResult;

                    }

                }

                #endregion

                #region ...else fail!

                return new SENDCDRResult(AuthorizatorId) {
                    State             = SENDCDRState.False,
                    PartnerSessionId  = PartnerSessionId,
                    Description       = "No authorization service returned a positiv result!"
                };

                #endregion

            }

        }

        #endregion


        #region RemoteStart(EVSEId, SessionId, ProviderId, eMAId, EventTrackingId = null)

        /// <summary>
        /// Initiate a remote start of a charging station socket outlet.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="EventTrackingId">An optional unique identification for tracking related events.</param>
        public RemoteStartResult RemoteStart(EVSE_Id           EVSEId,
                                             String            SessionId,
                                             EVSP_Id           ProviderId,
                                             eMA_Id            eMAId,
                                             EventTracking_Id  EventTrackingId = null)
        {

            lock (AuthenticationServices)
            {

                var OnRemoteStartLocal = OnRemoteStart;
                if (OnRemoteStartLocal != null)
                    return OnRemoteStartLocal(EVSEId, SessionId, ProviderId, eMAId, EventTrackingId);

                return RemoteStartResult.EVSE_NotReachable;

            }

        }

        #endregion

        #region RemoteStop(EVSEId, SessionId, ProviderId, EventTrackingId = null)

        /// <summary>
        /// Initiate a remote stop of a charging station socket outlet.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="EventTrackingId">An optional unique identification for tracking related events.</param>
        public RemoteStopResult RemoteStop(EVSE_Id           EVSEId,
                                           String            SessionId,
                                           EVSP_Id           ProviderId,
                                           EventTracking_Id  EventTrackingId  = null)
        {

            lock (AuthenticationServices)
            {

                var OnRemoteStopLocal = OnRemoteStop;
                if (OnRemoteStopLocal != null)
                    return OnRemoteStopLocal(EVSEId, SessionId, ProviderId, EventTrackingId);

                return RemoteStopResult.Error;

            }

        }

        #endregion


    }

}
