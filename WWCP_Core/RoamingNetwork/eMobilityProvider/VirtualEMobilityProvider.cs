/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using cloud.charging.open.protocols.WWCP.Networking;
using cloud.charging.open.protocols.WWCP.MobilityProvider;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    #region Delegates

    /// <summary>
    /// Indicate a remote start of the given charging session at the given EVSE
    /// and for the given provider/eMAId.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="ChargingLocation">The charging location.</param>
    /// <param name="ChargingProduct">The choosen charging product.</param>
    /// <param name="ReservationId">The unique identification for a charging reservation.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task OnRemoteStartRequestDelegate2(DateTime                  LogTimestamp,
                                                       DateTime                  RequestTimestamp,
                                                       Object                    Sender,
                                                       EventTracking_Id          EventTrackingId,
                                                       ChargingLocation          ChargingLocation,
                                                       ChargingProduct?          ChargingProduct,
                                                       ChargingReservation_Id?   ReservationId,
                                                       ChargingSession_Id?       SessionId,
                                                       RemoteAuthentication?     RemoteAuthentication,
                                                       TimeSpan?                 RequestTimeout);


    /// <summary>
    /// Indicate a remote start of the given charging session at the given EVSE
    /// and for the given provider/eMAId.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="ChargingLocation">The charging location.</param>
    /// <param name="ChargingProduct">The choosen charging product.</param>
    /// <param name="ReservationId">The unique identification for a charging reservation.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The remote start result.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnRemoteStartResponseDelegate2(DateTime                  LogTimestamp,
                                                        DateTime                  RequestTimestamp,
                                                        Object                    Sender,
                                                        EventTracking_Id          EventTrackingId,
                                                        ChargingLocation          ChargingLocation,
                                                        ChargingProduct?          ChargingProduct,
                                                        ChargingReservation_Id?   ReservationId,
                                                        ChargingSession_Id?       SessionId,
                                                        RemoteAuthentication?     RemoteAuthentication,
                                                        TimeSpan?                 RequestTimeout,
                                                        RemoteStartResult         Result,
                                                        TimeSpan                  Runtime);


    /// <summary>
    /// Indicate a remote stop of the given charging session.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task OnRemoteStopRequestDelegate2(DateTime                LogTimestamp,
                                                      DateTime                RequestTimestamp,
                                                      Object                  Sender,
                                                      EventTracking_Id        EventTrackingId,
                                                      ChargingSession_Id      SessionId,
                                                      ReservationHandling?    ReservationHandling,
                                                      RemoteAuthentication?   RemoteAuthentication,
                                                      TimeSpan?               RequestTimeout);


    /// <summary>
    /// Indicate a remote stop of the given charging session.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The remote stop result.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnRemoteStopResponseDelegate2(DateTime                LogTimestamp,
                                                       DateTime                RequestTimestamp,
                                                       Object                  Sender,
                                                       EventTracking_Id        EventTrackingId,
                                                       ChargingSession_Id      SessionId,
                                                       ReservationHandling?    ReservationHandling,
                                                       RemoteAuthentication?   RemoteAuthentication,
                                                       TimeSpan?               RequestTimeout,
                                                       RemoteStopResult        Result,
                                                       TimeSpan                Runtime);

    #endregion


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

        public TimeSpan               RequestTimeout    { get; }


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

        public event OnRemoteStartRequestDelegate2?     OnRemoteStartRequest;
        public event OnRemoteStartResponseDelegate2?    OnRemoteStartResponse;

        public event OnRemoteStopRequestDelegate2?      OnRemoteStopRequest;
        public event OnRemoteStopResponseDelegate2?     OnRemoteStopResponse;

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
                       I18NString.Create(Languages.en, "All fine. Thank you!"),
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

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, RemoteAuthentication = null, SessionId = null, ...)

        /// <summary>
        /// Start a charging session at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional cancellation token to cancel this request.</param>
        public async Task<RemoteStartResult> RemoteStart(ChargingLocation         ChargingLocation,
                                                         ChargingProduct?         ChargingProduct        = null,
                                                         ChargingReservation_Id?  ReservationId          = null,
                                                         RemoteAuthentication?    RemoteAuthentication   = null,
                                                         ChargingSession_Id?      SessionId              = null,

                                                         DateTime?                Timestamp              = null,
                                                         EventTracking_Id?        EventTrackingId        = null,
                                                         TimeSpan?                RequestTimeout         = null,
                                                         CancellationToken        CancellationToken      = default)
        {

            #region Init

            var startTime        = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            var timestamp        = Timestamp       ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            var eventTrackingId  = EventTrackingId ?? EventTracking_Id.New;
            var requestTimeout   = RequestTimeout  ?? this.RequestTimeout;

            RemoteStartResult? result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            try
            {

                if (OnRemoteStartRequest is not null)
                    await Task.WhenAll(OnRemoteStartRequest.GetInvocationList().
                                       Cast<OnRemoteStartRequestDelegate2>().
                                       Select(e => e(startTime,
                                                     timestamp,
                                                     this,
                                                     eventTrackingId,
                                                     ChargingLocation,
                                                     ChargingProduct,
                                                     ReservationId,
                                                     SessionId,
                                                     RemoteAuthentication,
                                                     requestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualEMobilityProvider) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            //ToDo: Should we check the ChargingLocation here?

            //ToDo: Should we check the ChargingProduct here?


            if (RoamingNetwork is not null)
            {

                var chargingSessionId = SessionId ?? ChargingSession_Id.NewRandom;

                while (ChargingSessions.ContainsKey(chargingSessionId))
                    chargingSessionId = ChargingSession_Id.NewRandom;

                ChargingSessions.Add(chargingSessionId,
                                     new ChargingSession(
                                         chargingSessionId,
                                         eventTrackingId
                                     ));


                result = await RoamingNetwork.RemoteStart(ChargingLocation,
                                                          ChargingProduct,
                                                          ReservationId,
                                                          SessionId,
                                                          Id,
                                                          RemoteAuthentication,

                                                          timestamp,
                                                          eventTrackingId,
                                                          requestTimeout,
                                                          CancellationToken);


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

            result ??= RemoteStartResult.Error(I18NString.Create(Languages.en, "No roaming network available!"),
                                               Runtime: TimeSpan.Zero);


            #region Send OnRemoteStartResponse event

            var endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                if (OnRemoteStartResponse is not null)
                    await Task.WhenAll(OnRemoteStartResponse.GetInvocationList().
                                       Cast<OnRemoteStartResponseDelegate2>().
                                       Select(e => e(endtime,
                                                     timestamp,
                                                     this,
                                                     eventTrackingId,
                                                     ChargingLocation,
                                                     ChargingProduct,
                                                     ReservationId,
                                                     SessionId,
                                                     RemoteAuthentication,
                                                     RequestTimeout,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualEMobilityProvider) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop (SessionId, ReservationHandling = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time (default: Close).</param>
        /// <param name="RemoteAuthentication">An optional remote authentication, e.g. an e-mobility account identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStopResult> RemoteStop(ChargingSession_Id     SessionId,
                                                       ReservationHandling?   ReservationHandling    = null,
                                                       RemoteAuthentication?  RemoteAuthentication   = null,

                                                       DateTime?              Timestamp              = null,
                                                       EventTracking_Id?      EventTrackingId        = null,
                                                       TimeSpan?              RequestTimeout         = null,
                                                       CancellationToken      CancellationToken      = default)
        {

            #region Init

            var reservationHandling  = ReservationHandling ?? WWCP.ReservationHandling.Close;
            var startTime            = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            var timestamp            = Timestamp           ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            var eventTrackingId      = EventTrackingId     ?? EventTracking_Id.New;
            var requestTimeout       = RequestTimeout      ?? this.RequestTimeout;

            RemoteStopResult? result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            try
            {

                if (OnRemoteStopRequest is not null)
                    await Task.WhenAll(OnRemoteStopRequest.GetInvocationList().
                                       Cast<OnRemoteStopRequestDelegate2>().
                                       Select(e => e(startTime,
                                                     timestamp,
                                                     this,
                                                     eventTrackingId,
                                                     SessionId,
                                                     ReservationHandling,
                                                     RemoteAuthentication,
                                                     requestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualEMobilityProvider) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            //ToDo: Should we check the SessionId here?

            if (RoamingNetwork is not null)
            {

                result = await RoamingNetwork.RemoteStop(SessionId,
                                                         ReservationHandling,
                                                         Id,
                                                         RemoteAuthentication,

                                                         Timestamp,
                                                         EventTrackingId,
                                                         RequestTimeout,
                                                         CancellationToken);

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
                                              I18NString.Create(Languages.en, "No roaming network available!"),
                                              Runtime: TimeSpan.Zero);


            #region Send OnRemoteStopResponse event

            var endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                if (OnRemoteStopResponse is not null)
                    await Task.WhenAll(OnRemoteStopResponse.GetInvocationList().
                                       Cast<OnRemoteStopResponseDelegate2>().
                                       Select(e => e(endtime,
                                                     timestamp,
                                                     this,
                                                     eventTrackingId,
                                                     SessionId,
                                                     ReservationHandling,
                                                     RemoteAuthentication,
                                                     requestTimeout,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualEMobilityProvider) + "." + nameof(OnRemoteStopResponse));
            }

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
    }

}
