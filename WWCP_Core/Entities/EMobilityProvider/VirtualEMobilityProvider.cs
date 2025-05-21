/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using cloud.charging.open.protocols.WWCP.Networking;
using cloud.charging.open.protocols.WWCP.MobilityProvider;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A virtual E-Mobility Provider for (internal) tests.
    /// </summary>
    public class VirtualEMobilityProvider : //ToDo: IEMobilityProvider,
                                            IRemoteEMobilityProvider,
                                            IRemoteEMobilityProviderUI
    {

        //ToDo: A virtual e-mobility provider might expose an HTTP API for smart phone/e-vehicle access!

        #region Data

        private readonly ConcurrentDictionary<AuthenticationToken,   AuthStartResult>     authCache             = new();
        private readonly ConcurrentDictionary<ChargeDetailRecord_Id, ChargeDetailRecord>  chargeDetailRecords   = new();

        public TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(120);

        #endregion

        #region Properties

        public EMobilityProvider_Id   Id                { get; }

        public IId                    AuthId            { get; }

        public IRoamingNetwork?       RoamingNetwork    { get; set; }

        public TimeSpan?              RequestTimeout    { get; set; }


        public Dictionary<ChargingSession_Id, ChargingSession>  ChargingSessions     { get; }



        public EMobilityProviderAPI?  HTTPAPI { get; private set; }



        public IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> AllTokens            => throw new NotImplementedException();

        public IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> AuthorizedTokens     => throw new NotImplementedException();

        public IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> NotAuthorizedTokens  => throw new NotImplementedException();

        public IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> BlockedTokens        => throw new NotImplementedException();

        #endregion

        #region Events

        public event OnAuthorizeStartRequestDelegate?   OnAuthorizeStartRequest;
        public event OnAuthorizeStartResponseDelegate?  OnAuthorizeStartResponse;

        public event OnAuthorizeStopRequestDelegate?    OnAuthorizeStopRequest;
        public event OnAuthorizeStopResponseDelegate?   OnAuthorizeStopResponse;

        public event OnRemoteStartRequestDelegate?      OnRemoteStartRequest;
        public event OnRemoteStartResponseDelegate?     OnRemoteStartResponse;

        public event OnRemoteStopRequestDelegate?       OnRemoteStopRequest;
        public event OnRemoteStopResponseDelegate?      OnRemoteStopResponse;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new virtual e-mobility provider.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="RoamingNetwork"></param>
        /// 
        /// <param name="DisableLogfiles"></param>
        /// <param name="ReloadDataOnStart"></param>
        /// 
        /// <param name="RoamingNetworkInfos"></param>
        /// <param name="DisableNetworkSync"></param>
        /// <param name="LoggingPath"></param>
        /// <param name="DNSClient"></param>
        public VirtualEMobilityProvider(EMobilityProvider_Id              Id,
                                        IRoamingNetwork?                  RoamingNetwork        = null,

                                        Boolean                           DisableLogfiles       = false,
                                        Boolean                           ReloadDataOnStart     = true,

                                        IEnumerable<RoamingNetworkInfo>?  RoamingNetworkInfos   = null,
                                        Boolean                           DisableNetworkSync    = false,
                                        String?                           LoggingPath           = null,
                                        DNSClient?                        DNSClient             = null)
        {

            this.Id                = Id;
            this.AuthId            = Id;
            this.RoamingNetwork    = RoamingNetwork;
            this.RequestTimeout    = DefaultRequestTimeout;

            this.ChargingSessions  = new ();

        }

        #endregion


        // From the roaming network


        #region UpdateAdminStatus

        public Task<PushEVSEAdminStatusResult> UpdateEVSEAdminStatus(IEnumerable<EVSEAdminStatusUpdate> AdminStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationAdminStatusResult> UpdateChargingStationAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate> AdminStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingPoolAdminStatusResult> UpdateChargingPoolAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate> AdminStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationOperatorAdminStatusResult> UpdateChargingStationOperatorAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate> AdminStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushRoamingNetworkAdminStatusResult> UpdateRoamingNetworkAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate> AdminStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UpdateStatus

        public Task<PushEVSEStatusResult> UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate> StatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationStatusResult> UpdateChargingStationStatus(IEnumerable<ChargingStationStatusUpdate> StatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingPoolStatusResult> UpdateChargingPoolStatus(IEnumerable<ChargingPoolStatusUpdate> StatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationOperatorStatusResult> UpdateChargingStationOperatorStatus(IEnumerable<ChargingStationOperatorStatusUpdate> StatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushRoamingNetworkStatusResult> UpdateRoamingNetworkStatus(IEnumerable<RoamingNetworkStatusUpdate> StatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UpdateEnergyStatus

        public Task<PushEVSEEnergyStatusResult> UpdateEVSEEnergyStatus(IEnumerable<EVSEEnergyStatusUpdate> EnergyStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationEnergyStatusResult> UpdateChargingStationEnergyStatus(IEnumerable<ChargingStationEnergyStatusUpdate> EnergyStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingPoolEnergyStatusResult> UpdateChargingPoolEnergyStatus(IEnumerable<ChargingPoolEnergyStatusUpdate> EnergyStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion



        #region AuthorizeStart(LocalAuthentication, ...)

        public async Task<AuthStartResult> AuthorizeStart(LocalAuthentication          LocalAuthentication,
                                                          ChargingLocation?            ChargingLocation      = null,
                                                          ChargingProduct?             ChargingProduct       = null,
                                                          ChargingSession_Id?          SessionId             = null,
                                                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                                                          ChargingStationOperator_Id?  OperatorId            = null,

                                                          DateTime?                    Timestamp             = null,
                                                          EventTracking_Id?            EventTrackingId       = null,
                                                          TimeSpan?                    RequestTimeout        = null,
                                                          CancellationToken            CancellationToken     = default)
        {

            if (LocalAuthentication.AuthToken.HasValue &&
                authCache.TryGetValue(LocalAuthentication.AuthToken.Value, out var authStartResult))
            {
                return authStartResult;
            }

            return AuthStartResult.NotAuthorized(AuthorizatorId:            AuthId,
                                                 ISendAuthorizeStartStop:   null,
                                                 SessionId:                 null,

                                                 ProviderId:                null,
                                                 Description:               null,
                                                 AdditionalInfo:            null,
                                                 NumberOfRetries:           0,
                                                 Runtime:                   null);

        }

        #endregion

        #region AuthorizeStop (SessionId, LocalAuthentication, ...)

        public async Task<AuthStopResult> AuthorizeStop(ChargingSession_Id           SessionId,
                                                        LocalAuthentication          LocalAuthentication,
                                                        ChargingLocation?            ChargingLocation      = null,
                                                        ChargingSession_Id?          CPOPartnerSessionId   = null,
                                                        ChargingStationOperator_Id?  OperatorId            = null,

                                                        DateTime?                    Timestamp             = null,
                                                        EventTracking_Id?            EventTrackingId       = null,
                                                        TimeSpan?                    RequestTimeout        = null,
                                                        CancellationToken            CancellationToken     = default)
        {

            if (LocalAuthentication.AuthToken.HasValue &&
                authCache.TryGetValue(LocalAuthentication.AuthToken.Value, out var authStartResult))
            {

                if (authStartResult.Result == AuthStartResultTypes.Authorized)
                    return AuthStopResult.Authorized(authStartResult.AuthorizatorId,
                                                     authStartResult.ISendAuthorizeStartStop,
                                                     authStartResult.CachedResultEndOfLifeTime,
                                                     authStartResult.SessionId,
                                                     authStartResult.ProviderId,
                                                     authStartResult.Description,
                                                     authStartResult.AdditionalInfo,
                                                     authStartResult.NumberOfRetries,
                                                     authStartResult.Runtime);

                if (authStartResult.Result == AuthStartResultTypes.NotAuthorized)
                    return AuthStopResult.NotAuthorized(authStartResult.AuthorizatorId,
                                                        authStartResult.ISendAuthorizeStartStop,
                                                        authStartResult.CachedResultEndOfLifeTime,
                                                        authStartResult.SessionId,
                                                        authStartResult.ProviderId,
                                                        authStartResult.Description,
                                                        authStartResult.AdditionalInfo,
                                                        authStartResult.NumberOfRetries,
                                                        authStartResult.Runtime);

            }

            return AuthStopResult.NotAuthorized(AuthorizatorId:            AuthId,
                                                ISendAuthorizeStartStop:   null,
                                                SessionId:                 null,

                                                ProviderId:                null,
                                                Description:               null,
                                                AdditionalInfo:            null,
                                                NumberOfRetries:           0,
                                                Runtime:                   null);

        }

        #endregion

        #region ReceiveChargeDetailRecords(ChargeDetailRecords)

        public async Task<SendCDRsResult> ReceiveChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,

                                                                     DateTime?                        Timestamp           = null,
                                                                     EventTracking_Id?                EventTrackingId     = null,
                                                                     TimeSpan?                        RequestTimeout      = null,
                                                                     CancellationToken                CancellationToken   = default)
        {

            foreach (var chargeDetailRecord in ChargeDetailRecords)
                chargeDetailRecords.TryAdd(chargeDetailRecord.Id,
                                           chargeDetailRecord);

            return SendCDRsResult.Success(
                       org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                       AuthId,
                       this,
                       ChargeDetailRecords,
                       I18NString.Create("All fine. Thank you!"),
                       Array.Empty<Warning>(),
                       TimeSpan.Zero
                   );

        }

        #endregion


        //ToDo: SessionStarted
        //ToDo: SessionUpdated
        //ToDo: SessionStopped


        // Management methods

        #region AddAuth(Token, Result)

        public void AddAuth(AuthenticationToken  Token,
                            AuthStartResult      Result)
        {

            this.authCache.TryAdd(Token, Result);

        }

        #endregion

        #region RemoveAuth(Token)

        public void RemoveAuth(AuthenticationToken Token)
        {

            this.authCache.Remove(Token, out _);

        }

        #endregion


        // UI methods

        #region RemoteStart(ChargingLocation, RemoteAuthentication, ChargingProduct = null, ReservationId = null, SessionId = null, AdditionalSessionInfos = null, AuthenticationPath = null, ...)

        /// <summary>
        /// Start a charging session at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The chosen charging product.</param>
        /// <param name="ReservationId">Use the given optional unique charging reservation identification for charging.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        RemoteAuthentication     RemoteAuthentication,
                        ChargingProduct?         ChargingProduct          = null,
                        ChargingReservation_Id?  ReservationId            = null,
                        ChargingSession_Id?      SessionId                = null,
                        JObject?                 AdditionalSessionInfos   = null,
                        Auth_Path?               AuthenticationPath       = null,

                        DateTime?                RequestTimestamp         = null,
                        EventTracking_Id?        EventTrackingId          = null,
                        TimeSpan?                RequestTimeout           = null,
                        CancellationToken        CancellationToken        = default)

        {

            #region Init

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;
            RequestTimeout   ??= this.RequestTimeout;

            RemoteStartResult? result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStartRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          ChargingLocation,
                          RemoteAuthentication,
                          SessionId,
                          ReservationId,
                          ChargingProduct,
                          null,
                          null,
                          Id,
                          RequestTimeout
                      )
                  );

            #endregion


            //ToDo: Should we check the ChargingLocation here?

            //ToDo: Should we check the ChargingProduct here?


            if (RoamingNetwork is not null)
            {

                var chargingSessionId = SessionId ?? ChargingSession_Id.NewRandom(Id);

                while (ChargingSessions.ContainsKey(chargingSessionId))
                    chargingSessionId = ChargingSession_Id.NewRandom(Id);

                ChargingSessions.Add(chargingSessionId,
                                     new ChargingSession(
                                         chargingSessionId,
                                         EventTrackingId,
                                         RoamingNetwork
                                     ));


                result = await RoamingNetwork.RemoteStart(
                                   ChargingLocation,
                                   ChargingProduct,
                                   ReservationId,
                                   SessionId,
                                   Id,
                                   RemoteAuthentication,
                                   AdditionalSessionInfos,
                                   AuthenticationPath,

                                   RequestTimestamp,
                                   EventTrackingId,
                                   RequestTimeout,
                                   CancellationToken
                               );


                switch (result.Result)
                {

                    case RemoteStartResultTypes.Success:
                        //ChargingSessions[chargingSessionId]
                        break;

                    case RemoteStartResultTypes.AsyncOperation:
                        //ChargingSessions[chargingSessionId]
                        break;

                    default:
                        break;

                }

            }

            result ??= RemoteStartResult.Error(
                           System_Id.Local,
                           I18NString.Create("No roaming network available!"),
                           Runtime: TimeSpan.Zero
                       );


            #region Send OnRemoteStartResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStartResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          ChargingLocation,
                          RemoteAuthentication,
                          SessionId,
                          ReservationId,
                          ChargingProduct,
                          null,
                          null,
                          Id,
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop (SessionId, ReservationHandling, RemoteAuthentication = null, AuthenticationPath = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,
                       Auth_Path?             AuthenticationPath     = null,

                       DateTime?              RequestTimestamp       = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null,
                       CancellationToken      CancellationToken      = default)

        {

            #region Init

            ReservationHandling ??= WWCP.ReservationHandling.Close;
            RequestTimestamp    ??= Timestamp.Now;
            EventTrackingId     ??= EventTracking_Id.New;
            RequestTimeout      ??= this.RequestTimeout;

            RemoteStopResult? result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStopRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          SessionId,
                          ReservationHandling,
                          null,
                          null,
                          Id,
                          RemoteAuthentication,
                          RequestTimeout
                      )
                  );

            #endregion


            //ToDo: Should we check the SessionId here?

            if (RoamingNetwork is not null)
            {

                result = await RoamingNetwork.RemoteStop(
                                   SessionId,
                                   ReservationHandling,
                                   Id,
                                   RemoteAuthentication,
                                   AuthenticationPath,

                                   RequestTimestamp,
                                   EventTrackingId,
                                   RequestTimeout,
                                   CancellationToken
                               );

                switch (result.Result)
                {

                    case RemoteStopResultTypes.Success:
                        //ChargingSessions[chargingSessionId]
                        break;

                    case RemoteStopResultTypes.AsyncOperation:
                        //ChargingSessions[chargingSessionId]
                        break;

                    default:
                        break;

                }

            }

            result ??= RemoteStopResult.Error(SessionId,
                                              System_Id.Local,
                                              I18NString.Create("No roaming network available!"),
                                              Runtime: TimeSpan.Zero);


            #region Send OnRemoteStopResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStopResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          SessionId,
                          ReservationHandling,
                          null,
                          null,
                          Id,
                          RemoteAuthentication,
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion




        public EMobilityProviderAPI StartAPI(IPPort? HTTPServerPort = null)
        {

            HTTPAPI = new EMobilityProviderAPI(
                          this,
                          HTTPServerPort:  HTTPServerPort,
                          AutoStart:       true
                      );

            return HTTPAPI;

        }

        public void ShutdownAPI()
        {

            HTTPAPI?.Shutdown(Wait: true);

        }

        public Task<AddRoamingNetworkResult> AddRoamingNetwork(IRoamingNetwork RoamingNetwork, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddRoamingNetworkResult> AddRoamingNetworkIfNotExists(IRoamingNetwork RoamingNetwork, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateRoamingNetworkResult> AddOrUpdateRoamingNetwork(IRoamingNetwork RoamingNetwork, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateRoamingNetworkResult> UpdateRoamingNetwork(IRoamingNetwork RoamingNetwork, String PropertyName, Object? NewValue, Object? OldValue = null, Context? DataSource = null, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteRoamingNetworkResult> DeleteRoamingNetwork(IRoamingNetwork RoamingNetwork, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddRoamingNetworksResult> AddRoamingNetworks(IEnumerable<IRoamingNetwork> RoamingNetworks, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddRoamingNetworksResult> AddRoamingNetworksIfNotExist(IEnumerable<IRoamingNetwork> RoamingNetworks, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateRoamingNetworksResult> AddOrUpdateRoamingNetworks(IEnumerable<IRoamingNetwork> RoamingNetworks, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateRoamingNetworksResult> UpdateRoamingNetworks(IEnumerable<IRoamingNetwork> RoamingNetworks, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteRoamingNetworksResult> DeleteRoamingNetworks(IEnumerable<IRoamingNetwork> RoamingNetworks, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationOperatorResult> AddChargingStationOperator(IChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationOperatorResult> AddChargingStationOperatorIfNotExists(IChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingStationOperatorResult> AddOrUpdateChargingStationOperator(IChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationOperatorResult> UpdateChargingStationOperator(IChargingStationOperator ChargingStationOperator, String PropertyName, Object? NewValue, Object? OldValue = null, Context? DataSource = null, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingStationOperatorResult> DeleteChargingStationOperator(IChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationOperatorsResult> AddChargingStationOperators(IEnumerable<IChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationOperatorsResult> AddChargingStationOperatorsIfNotExist(IEnumerable<IChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingStationOperatorsResult> AddOrUpdateChargingStationOperators(IEnumerable<IChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationOperatorsResult> UpdateChargingStationOperators(IEnumerable<IChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingStationOperatorsResult> DeleteChargingStationOperators(IEnumerable<IChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingPoolResult> AddChargingPool(IChargingPool ChargingPool, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingPoolResult> AddChargingPoolIfNotExists(IChargingPool ChargingPool, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingPoolResult> AddOrUpdateChargingPool(IChargingPool ChargingPool, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingPoolResult> UpdateChargingPool(IChargingPool ChargingPool, String PropertyName, Object? NewValue, Object? OldValue = null, Context? DataSource = null, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingPoolResult> DeleteChargingPool(IChargingPool ChargingPool, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingPoolsResult> AddChargingPools(IEnumerable<IChargingPool> ChargingPools, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingPoolsResult> AddChargingPoolsIfNotExist(IEnumerable<IChargingPool> ChargingPools, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingPoolsResult> AddOrUpdateChargingPools(IEnumerable<IChargingPool> ChargingPools, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingPoolsResult> UpdateChargingPools(IEnumerable<IChargingPool> ChargingPools, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingPoolsResult> DeleteChargingPools(IEnumerable<IChargingPool> ChargingPools, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationResult> AddChargingStation(IChargingStation ChargingStation, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationResult> AddChargingStationIfNotExists(IChargingStation ChargingStation, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingStationResult> AddOrUpdateChargingStation(IChargingStation ChargingStation, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationResult> UpdateChargingStation(IChargingStation ChargingStation, String PropertyName, Object? NewValue, Object? OldValue = null, Context? DataSource = null, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingStationResult> DeleteChargingStation(IChargingStation ChargingStation, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationsResult> AddChargingStations(IEnumerable<IChargingStation> ChargingStations, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationsResult> AddChargingStationsIfNotExist(IEnumerable<IChargingStation> ChargingStations, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingStationsResult> AddOrUpdateChargingStations(IEnumerable<IChargingStation> ChargingStations, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationsResult> UpdateChargingStations(IEnumerable<IChargingStation> ChargingStations, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingStationsResult> DeleteChargingStations(IEnumerable<IChargingStation> ChargingStations, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEResult> AddEVSE(IEVSE EVSE, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEResult> AddEVSEIfNotExists(IEVSE EVSE, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateEVSEResult> AddOrUpdateEVSE(IEVSE EVSE, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateEVSEResult> UpdateEVSE(IEVSE EVSE, String PropertyName, Object? NewValue, Object? OldValue = null, Context? DataSource = null, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteEVSEResult> DeleteEVSE(IEVSE EVSE, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEsResult> AddEVSEs(IEnumerable<IEVSE> EVSEs, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEsResult> AddEVSEsIfNotExist(IEnumerable<IEVSE> EVSEs, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateEVSEsResult> AddOrUpdateEVSEs(IEnumerable<IEVSE> EVSEs, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateEVSEsResult> UpdateEVSEs(IEnumerable<IEVSE> EVSEs, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteEVSEsResult> DeleteEVSEs(IEnumerable<IEVSE> EVSEs, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingPoolEnergyStatusResult> UpdateChargingPoolEnergyStatus(IEnumerable<ChargingPoolEnergyStatusUpdate> ChargingPoolEnergyStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken? CancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationEnergyStatusResult> UpdateChargingStationEnergyStatus(IEnumerable<ChargingStationEnergyStatusUpdate> ChargingStationEnergyStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken? CancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEEnergyStatusResult> UpdateEVSEEnergyStatus(IEnumerable<EVSEEnergyStatusUpdate> EVSEEnergyStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken? CancellationToken = null)
        {
            throw new NotImplementedException();
        }



        #region (private) LogEvent(Logger, LogHandler, ...)

        private Task LogEvent<TDelegate>(TDelegate?                                         Logger,
                                         Func<TDelegate, Task>                              LogHandler,
                                         [CallerArgumentExpression(nameof(Logger))] String  EventName   = "",
                                         [CallerMemberName()]                       String  Command     = "")

            where TDelegate : Delegate

                => LogEvent(
                       nameof(EVSE),
                       Logger,
                       LogHandler,
                       EventName,
                       Command
                   );

        #endregion

        #region (protected) LogEvent(Logger, LogHandler, ...)

        protected async Task LogEvent<TDelegate>(String                                             WWCPIO,
                                                 TDelegate?                                         Logger,
                                                 Func<TDelegate, Task>                              LogHandler,
                                                 [CallerArgumentExpression(nameof(Logger))] String  EventName   = "",
                                                 [CallerMemberName()]                       String  Command     = "")

            where TDelegate : Delegate

        {
            if (Logger is not null)
            {
                try
                {

                    await Task.WhenAll(
                              Logger.GetInvocationList().
                                     OfType<TDelegate>().
                                     Select(LogHandler)
                          );

                }
                catch (Exception e)
                {
                    await HandleErrors(WWCPIO, $"{Command}.{EventName}", e);
                }
            }
        }

        #endregion

        #region (virtual) HandleErrors(Module, Caller, ErrorResponse)

        public virtual Task HandleErrors(String  Module,
                                         String  Caller,
                                         String  ErrorResponse)
        {

            return Task.CompletedTask;

        }

        #endregion

        #region (virtual) HandleErrors(Module, Caller, ExceptionOccurred)

        public virtual Task HandleErrors(String     Module,
                                         String     Caller,
                                         Exception  ExceptionOccurred)
        {

            return Task.CompletedTask;

        }

        #endregion


    }

}
