///*
// * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
// *
// * Licensed under the Affero GPL license, Version 3.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.gnu.org/licenses/agpl.html
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//using org.GraphDefined.Vanaheimr.Illias;

//#endregion

//namespace org.GraphDefined.WWCP.ChargingStations
//{

//    /// <summary>
//    /// A remote charging station operator.
//    /// </summary>
//    public class RemoteChargingStationOperator : IRemoteChargingStationOperator
//    {

//        #region Properties

//        public ChargingStationOperator Operator { get; }

//        private readonly Dictionary<ChargingReservation_Id, ChargingReservation> _ChargingReservations;

//        /// <summary>
//        /// All current charging reservations.
//        /// </summary>
//        public IEnumerable<ChargingReservation> Reservations
//            => _ChargingReservations.Select(_ => _.Value);


//        private readonly Dictionary<ChargingSession_Id, ChargingSession> _ChargingSessions;

//        /// <summary>
//        /// All current charging sessions.
//        /// </summary>
//        public IEnumerable<ChargingSession> ChargingSessions
//            => _ChargingSessions.Select(_ => _.Value);

//        #endregion

//        #region Events

//        /// <summary>
//        /// An event fired whenever an EVSE is being reserved.
//        /// </summary>
//        public event OnReserveDelegate        OnReserve;

//        /// <summary>
//        /// An event fired whenever an EVSE reservation is canceled.
//        /// </summary>
//        public event OnCancelReservationDelegate  OnCancelReservation;

//        /// <summary>
//        /// An event sent whenever an EVSE should start charging.
//        /// </summary>
//        public event OnRemoteStartDelegate        OnRemoteStart;

//        /// <summary>
//        /// An event sent whenever a charging session should stop.
//        /// </summary>
//        public event OnRemoteStopDelegate         OnRemoteStop;

//        #endregion

//        #region Constructor(s)

//        /// <summary>
//        /// Create a new remote charging station operator.
//        /// </summary>
//        /// <param name="Operator">The charging station operator.</param>
//        /// <param name="OnReserve">A delegate fired whenever an EVSE is being reserved.</param>
//        /// <param name="OnCancelReservation">A delegate fired whenever an EVSE reservation is canceled.</param>
//        /// <param name="OnRemoteStart">A delegate sent whenever an EVSE should start charging.</param>
//        /// <param name="OnRemoteStop">A delegate sent whenever a charging session should stop.</param>
//        public RemoteChargingStationOperator(ChargingStationOperator      Operator,
//                                             OnReserveDelegate            OnReserve,
//                                             OnCancelReservationDelegate  OnCancelReservation,
//                                             OnRemoteStartDelegate        OnRemoteStart,
//                                             OnRemoteStopDelegate         OnRemoteStop)
//        {

//            this.Operator                 = Operator;

//            this.OnReserve               += OnReserve;
//            this.OnCancelReservation     += OnCancelReservation;
//            this.OnRemoteStart           += OnRemoteStart;
//            this.OnRemoteStop            += OnRemoteStop;

//            this._ChargingReservations  = new Dictionary<ChargingReservation_Id, ChargingReservation>();
//            this._ChargingSessions      = new Dictionary<ChargingSession_Id,     ChargingSession>();

//        }

//        #endregion


//        #region Reserve(EVSEId, ...)

//        /// <summary>
//        /// Reserve the possibility to charge at the given EVSE.
//        /// </summary>
//        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
//        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
//        /// <param name="Duration">The duration of the reservation.</param>
//        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
//        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
//        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
//        /// <param name="ChargingProduct">The charging product to be reserved.</param>
//        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
//        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
//        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public async Task<ReservationResult>

//            Reserve(EVSE_Id                           EVSEId,
//                    DateTime?                         StartTime,
//                    TimeSpan?                         Duration,
//                    ChargingReservation_Id?           ReservationId          = null,
//                    eMobilityProvider_Id?             ProviderId             = null,
//                    RemoteAuthentication              RemoteAuthentication   = null,
//                    ChargingProduct                   ChargingProduct        = null,
//                    IEnumerable<Auth_Token>           AuthTokens             = null,
//                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
//                    IEnumerable<UInt32>               PINs                   = null,

//                    DateTime?                         Timestamp              = null,
//                    CancellationToken?                CancellationToken      = null,
//                    EventTracking_Id                  EventTrackingId        = null,
//                    TimeSpan?                         RequestTimeout         = null)

//        {


//            var OnReserveEVSELocal = OnReserve;
//            if (OnReserveEVSELocal == null)
//                return ReservationResult.OutOfService;

//            var results = await Task.WhenAll(OnReserveEVSELocal.
//                                                 GetInvocationList().
//                                                 Select(subscriber => (subscriber as OnReserveDelegate)
//                                                     (ReservationId,
//                                                      EVSEId,
//                                                      StartTime,
//                                                      Duration,
//                                                      ProviderId,
//                                                      RemoteAuthentication,
//                                                      ChargingProduct,
//                                                      AuthTokens,
//                                                      eMAIds,
//                                                      PINs,

//                                                      Timestamp,
//                                                      CancellationToken,
//                                                      EventTrackingId,
//                                                      RequestTimeout)));

//            var result = results.FirstOrDefault(_result => _result.Result != ReservationResultType.Unspecified);

//            if (result        != null &&
//                result.Result == ReservationResultType.Success)
//            {

//                //_ChargingSessions.Add(result.Session.Id, result.Session);

//            }

//            return result;

//        }

//        #endregion

//        #region Reserve(ChargingStationId, ...)

//        /// <summary>
//        /// Reserve the possibility to charge at the given charging station.
//        /// </summary>
//        /// <param name="ChargingStationId">The unique identification of the charging station to be reserved.</param>
//        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
//        /// <param name="Duration">The duration of the reservation.</param>
//        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
//        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
//        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
//        /// <param name="ChargingProduct">The charging product to be reserved.</param>
//        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
//        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
//        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<ReservationResult>

//            Reserve(ChargingStation_Id                ChargingStationId,
//                    DateTime?                         StartTime,
//                    TimeSpan?                         Duration,
//                    ChargingReservation_Id?           ReservationId          = null,
//                    eMobilityProvider_Id?             ProviderId             = null,
//                    RemoteAuthentication              RemoteAuthentication   = null,
//                    ChargingProduct                   ChargingProduct        = null,
//                    IEnumerable<Auth_Token>           AuthTokens             = null,
//                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
//                    IEnumerable<UInt32>               PINs                   = null,

//                    DateTime?                         Timestamp              = null,
//                    CancellationToken?                CancellationToken      = null,
//                    EventTracking_Id                  EventTrackingId        = null,
//                    TimeSpan?                         RequestTimeout         = null)

//            => Task.FromResult(ReservationResult.OutOfService);

//        #endregion

//        #region Reserve(ChargingPoolId, ...)

//        /// <summary>
//        /// Reserve the possibility to charge within the given charging pool.
//        /// </summary>
//        /// <param name="ChargingPoolId">The unique identification of the charging pool to be reserved.</param>
//        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
//        /// <param name="Duration">The duration of the reservation.</param>
//        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
//        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
//        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
//        /// <param name="ChargingProduct">The charging product to be reserved.</param>
//        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
//        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
//        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<ReservationResult>

//            Reserve(ChargingPool_Id                   ChargingPoolId,
//                    DateTime?                         StartTime,
//                    TimeSpan?                         Duration,
//                    ChargingReservation_Id?           ReservationId          = null,
//                    eMobilityProvider_Id?             ProviderId             = null,
//                    RemoteAuthentication              RemoteAuthentication   = null,
//                    ChargingProduct                   ChargingProduct        = null,
//                    IEnumerable<Auth_Token>           AuthTokens             = null,
//                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
//                    IEnumerable<UInt32>               PINs                   = null,

//                    DateTime?                         Timestamp              = null,
//                    CancellationToken?                CancellationToken      = null,
//                    EventTracking_Id                  EventTrackingId        = null,
//                    TimeSpan?                         RequestTimeout         = null)

//            => Task.FromResult(ReservationResult.OutOfService);

//        #endregion


//        #region CancelReservation(...)

//        /// <summary>
//        /// Try to remove the given charging reservation.
//        /// </summary>
//        /// <param name="ReservationId">The unique charging reservation identification.</param>
//        /// <param name="Reason">A reason for this cancellation.</param>
//        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public async Task<CancelReservationResult>

//            CancelReservation(ChargingReservation_Id                 ReservationId,
//                              ChargingReservationCancellationReason  Reason,
//                              eMobilityProvider_Id?                  ProviderId          = null,

//                              DateTime?                              Timestamp           = null,
//                              CancellationToken?                     CancellationToken   = null,
//                              EventTracking_Id                       EventTrackingId     = null,
//                              TimeSpan?                              RequestTimeout      = null)

//        {


//            var OnCancelEVSEReservationLocal = OnCancelReservation;
//            if (OnCancelEVSEReservationLocal == null)
//                return CancelReservationResult.OutOfService(ReservationId, Reason);

//            var results = await Task.WhenAll(OnCancelEVSEReservationLocal.
//                                                 GetInvocationList().
//                                                 Select(subscriber => (subscriber as OnCancelReservationDelegate)
//                                                     (Timestamp.Value,
//                                                      CancellationToken.Value,
//                                                      EventTrackingId,

//                                                      ReservationId,
//                                                      Reason,
//                                                      ProviderId,

//                                                      RequestTimeout)));

//            var result = results.FirstOrDefault(_result => _result.Result != CancelReservationResults.Unspecified);

//            if (result        != null &&
//                result.Result == CancelReservationResults.Success)
//            {

//                //_ChargingSessions.Add(result.Session.Id, result.Session);

//            }

//            return result;

//        }

//        #endregion


//        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

//        /// <summary>
//        /// Start a charging session at the given EVSE.
//        /// </summary>
//        /// <param name="ChargingLocation">The charging location.</param>
//        /// <param name="ChargingProduct">The choosen charging product.</param>
//        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
//        /// <param name="SessionId">The unique identification for this charging session.</param>
//        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
//        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public async Task<RemoteStartResult>

//            RemoteStart(ChargingLocation         ChargingLocation,
//                        ChargingProduct          ChargingProduct,
//                        ChargingReservation_Id?  ReservationId,
//                        ChargingSession_Id?      SessionId,
//                        eMobilityProvider_Id?    ProviderId,
//                        RemoteAuthentication     RemoteAuthentication,

//                        DateTime?                Timestamp          = null,
//                        CancellationToken?       CancellationToken  = null,
//                        EventTracking_Id         EventTrackingId    = null,
//                        TimeSpan?                RequestTimeout     = null)

//        {


//            var OnRemoteStartLocal = OnRemoteStart;
//            if (OnRemoteStartLocal == null)
//                return RemoteStartResult.OutOfService;

//            var results = await Task.WhenAll(OnRemoteStartLocal.
//                                                 GetInvocationList().
//                                                 Select(subscriber => (subscriber as OnRemoteStartDelegate)
//                                                     (Timestamp.Value,
//                                                      CancellationToken.Value,
//                                                      EventTrackingId,
//                                                      ChargingProduct,
//                                                      ReservationId,
//                                                      SessionId,
//                                                      ProviderId,
//                                                      RemoteAuthentication,
//                                                      RequestTimeout)));

//            var result = results.FirstOrDefault(_result => _result.Result != RemoteStartResultType.Unspecified);

//            if (result        != null &&
//                result.Result == RemoteStartResultType.Success)
//            {

//                _ChargingSessions.Add(result.Session.Id, result.Session);

//            }

//            return result;

//        }

//        #endregion

//        #region RemoteStop (SessionId, ReservationHandling = null, ProviderId = null, RemoteAuthentication = null, ...)

//        /// <summary>
//        /// Stop the given charging session.
//        /// </summary>
//        /// <param name="SessionId">The unique identification for this charging session.</param>
//        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
//        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
//        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public async Task<RemoteStopResult>

//            RemoteStop(ChargingSession_Id     SessionId,
//                       ReservationHandling?   ReservationHandling    = null,
//                       eMobilityProvider_Id?  ProviderId             = null,
//                       RemoteAuthentication   RemoteAuthentication   = null,

//                       DateTime?              Timestamp              = null,
//                       CancellationToken?     CancellationToken      = null,
//                       EventTracking_Id       EventTrackingId        = null,
//                       TimeSpan?              RequestTimeout         = null)

//        {

//            var OnRemoteStopLocal = OnRemoteStop;
//            if (OnRemoteStopLocal == null)
//                return RemoteStopResult.Error(SessionId);

//            var results = await Task.WhenAll(OnRemoteStopLocal.
//                                                 GetInvocationList().
//                                                 Select(subscriber => (subscriber as OnRemoteStopDelegate)
//                                                     (Timestamp.Value,
//                                                      CancellationToken.Value,
//                                                      EventTrackingId,
//                                                      ReservationHandling,
//                                                      SessionId,
//                                                      ProviderId,
//                                                      RemoteAuthentication,
//                                                      RequestTimeout)));

//            var result = results.
//                             FirstOrDefault(_result => _result.Result != RemoteStopResultType.Unspecified);

//            if (result        != null &&
//                result.Result == RemoteStopResultType.Success)
//            {

//                if (_ChargingSessions.ContainsKey(result.SessionId))
//                    _ChargingSessions.Remove(result.SessionId);

//            }

//            return result;

//        }

//        #endregion


//    }

//}