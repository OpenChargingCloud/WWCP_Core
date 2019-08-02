/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

#endregion

namespace org.GraphDefined.WWCP.ChargingStations
{

    /// <summary>
    /// A remote charging station operator.
    /// </summary>
    public class RemoteChargingStationOperator : IRemoteChargingStationOperator
    {

        #region Properties

        public ChargingStationOperator Operator { get; }

        private readonly Dictionary<ChargingReservation_Id, ChargingReservation> _ChargingReservations;

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
            => _ChargingReservations.Select(_ => _.Value);


        private readonly Dictionary<ChargingSession_Id, ChargingSession> _ChargingSessions;

        /// <summary>
        /// All current charging sessions.
        /// </summary>
        public IEnumerable<ChargingSession> ChargingSessions
            => _ChargingSessions.Select(_ => _.Value);

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever an EVSE is being reserved.
        /// </summary>
        public event OnReserveEVSEDelegate        OnReserveEVSE;

        /// <summary>
        /// An event fired whenever an EVSE reservation is canceled.
        /// </summary>
        public event OnCancelReservationDelegate  OnCancelEVSEReservation;

        /// <summary>
        /// An event sent whenever an EVSE should start charging.
        /// </summary>
        public event OnRemoteStartEVSEDelegate    OnRemoteStartEVSE;

        /// <summary>
        /// An event sent whenever a charging session should stop.
        /// </summary>
        public event OnRemoteStopEVSEDelegate     OnRemoteStopEVSE;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new remote charging station operator.
        /// </summary>
        /// <param name="Operator">The charging station operator.</param>
        /// <param name="OnReserveEVSE">A delegate fired whenever an EVSE is being reserved.</param>
        /// <param name="OnCancelEVSEReservation">A delegate fired whenever an EVSE reservation is canceled.</param>
        /// <param name="OnRemoteStartEVSE">A delegate sent whenever an EVSE should start charging.</param>
        /// <param name="OnRemoteStopEVSE">A delegate sent whenever a charging session should stop.</param>
        public RemoteChargingStationOperator(ChargingStationOperator      Operator,
                                             OnReserveEVSEDelegate        OnReserveEVSE,
                                             OnCancelReservationDelegate  OnCancelEVSEReservation,
                                             OnRemoteStartEVSEDelegate    OnRemoteStartEVSE,
                                             OnRemoteStopEVSEDelegate     OnRemoteStopEVSE)
        {

            this.Operator                 = Operator;

            this.OnReserveEVSE           += OnReserveEVSE;
            this.OnCancelEVSEReservation += OnCancelEVSEReservation;
            this.OnRemoteStartEVSE       += OnRemoteStartEVSE;
            this.OnRemoteStopEVSE        += OnRemoteStopEVSE;

            this._ChargingReservations  = new Dictionary<ChargingReservation_Id, ChargingReservation>();
            this._ChargingSessions      = new Dictionary<ChargingSession_Id,     ChargingSession>();

        }

        #endregion


        #region Reserve(EVSEId, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(EVSE_Id                           EVSEId,
                    DateTime?                         StartTime,
                    TimeSpan?                         Duration,
                    ChargingReservation_Id?           ReservationId          = null,
                    eMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)

        {


            var OnReserveEVSELocal = OnReserveEVSE;
            if (OnReserveEVSELocal == null)
                return ReservationResult.OutOfService;

            var results = await Task.WhenAll(OnReserveEVSELocal.
                                                 GetInvocationList().
                                                 Select(subscriber => (subscriber as OnReserveEVSEDelegate)
                                                     (ReservationId,
                                                      EVSEId,
                                                      StartTime,
                                                      Duration,
                                                      ProviderId,
                                                      RemoteAuthentication,
                                                      ChargingProduct,
                                                      AuthTokens,
                                                      eMAIds,
                                                      PINs,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout)));

            var result = results.FirstOrDefault(_result => _result.Result != ReservationResultType.Unspecified);

            if (result        != null &&
                result.Result == ReservationResultType.Success)
            {

                //_ChargingSessions.Add(result.Session.Id, result.Session);

            }

            return result;

        }

        #endregion

        #region Reserve(ChargingStationId, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<ReservationResult>

            Reserve(ChargingStation_Id                ChargingStationId,
                    DateTime?                         StartTime,
                    TimeSpan?                         Duration,
                    ChargingReservation_Id?           ReservationId          = null,
                    eMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)

            => Task.FromResult(ReservationResult.OutOfService);

        #endregion

        #region Reserve(ChargingPoolId, ...)

        /// <summary>
        /// Reserve the possibility to charge within the given charging pool.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the charging pool to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<ReservationResult>

            Reserve(ChargingPool_Id                   ChargingPoolId,
                    DateTime?                         StartTime,
                    TimeSpan?                         Duration,
                    ChargingReservation_Id?           ReservationId          = null,
                    eMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)

            => Task.FromResult(ReservationResult.OutOfService);

        #endregion


        #region CancelReservation(...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              eMobilityProvider_Id?                  ProviderId          = null,

                              DateTime?                              Timestamp           = null,
                              CancellationToken?                     CancellationToken   = null,
                              EventTracking_Id                       EventTrackingId     = null,
                              TimeSpan?                              RequestTimeout      = null)

        {


            var OnCancelEVSEReservationLocal = OnCancelEVSEReservation;
            if (OnCancelEVSEReservationLocal == null)
                return CancelReservationResult.OutOfService(ReservationId, Reason);

            var results = await Task.WhenAll(OnCancelEVSEReservationLocal.
                                                 GetInvocationList().
                                                 Select(subscriber => (subscriber as OnCancelReservationDelegate)
                                                     (Timestamp.Value,
                                                      CancellationToken.Value,
                                                      EventTrackingId,

                                                      ReservationId,
                                                      Reason,
                                                      ProviderId,

                                                      RequestTimeout)));

            var result = results.FirstOrDefault(_result => _result.Result != CancelReservationResults.Unspecified);

            if (result        != null &&
                result.Result == CancelReservationResults.Success)
            {

                //_ChargingSessions.Add(result.Session.Id, result.Session);

            }

            return result;

        }

        #endregion


        #region RemoteStart(EVSEId, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartEVSEResult>

            RemoteStart(EVSE_Id                  EVSEId,
                        ChargingProduct          ChargingProduct,
                        ChargingReservation_Id?  ReservationId,
                        ChargingSession_Id?      SessionId,
                        eMobilityProvider_Id?    ProviderId,
                        RemoteAuthentication     RemoteAuthentication,

                        DateTime?                Timestamp          = null,
                        CancellationToken?       CancellationToken  = null,
                        EventTracking_Id         EventTrackingId    = null,
                        TimeSpan?                RequestTimeout     = null)

        {


            var OnRemoteStartEVSELocal = OnRemoteStartEVSE;
            if (OnRemoteStartEVSELocal == null)
                return RemoteStartEVSEResult.OutOfService;

            var results = await Task.WhenAll(OnRemoteStartEVSELocal.
                                                 GetInvocationList().
                                                 Select(subscriber => (subscriber as OnRemoteStartEVSEDelegate)
                                                     (Timestamp.Value,
                                                      CancellationToken.Value,
                                                      EventTrackingId,
                                                      EVSEId,
                                                      ChargingProduct,
                                                      ReservationId,
                                                      SessionId,
                                                      ProviderId,
                                                      RemoteAuthentication,
                                                      RequestTimeout)));

            var result = results.FirstOrDefault(_result => _result.Result != RemoteStartEVSEResultType.Unspecified);

            if (result        != null &&
                result.Result == RemoteStartEVSEResultType.Success)
            {

                _ChargingSessions.Add(result.Session.Id, result.Session);

            }

            return result;

        }

        #endregion

        #region RemoteStart(ChargingStationId, ...)

        /// <summary>
        /// Start a charging session at the given charging stations.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station to be started.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartChargingStationResult>

            RemoteStart(ChargingStation_Id       ChargingStationId,
                        ChargingProduct          ChargingProduct,
                        ChargingReservation_Id?  ReservationId,
                        ChargingSession_Id?      SessionId,
                        eMobilityProvider_Id?    ProviderId,
                        RemoteAuthentication     RemoteAuthentication,

                        DateTime?                Timestamp          = null,
                        CancellationToken?       CancellationToken  = null,
                        EventTracking_Id         EventTrackingId    = null,
                        TimeSpan?                RequestTimeout     = null)

            => RemoteStartChargingStationResult.OutOfService;

        #endregion


        #region RemoteStop(SessionId, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       eMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

            => Task.FromResult(RemoteStopResult.OutOfService(SessionId));

        #endregion

        #region RemoteStop(EVSEId, SessionId, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopEVSEResult>

            RemoteStop(EVSE_Id                EVSEId,
                       ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       eMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            var OnRemoteStopEVSELocal = OnRemoteStopEVSE;
            if (OnRemoteStopEVSELocal == null)
                return RemoteStopEVSEResult.Error(SessionId);

            var results = await Task.WhenAll(OnRemoteStopEVSELocal.
                                                 GetInvocationList().
                                                 Select(subscriber => (subscriber as OnRemoteStopEVSEDelegate)
                                                     (Timestamp.Value,
                                                      CancellationToken.Value,
                                                      EventTrackingId,
                                                      ReservationHandling,
                                                      SessionId,
                                                      ProviderId,
                                                      RemoteAuthentication,
                                                      EVSEId,
                                                      RequestTimeout)));

            var result = results.
                             FirstOrDefault(_result => _result.Result != RemoteStopEVSEResultType.Unspecified);

            if (result        != null &&
                result.Result == RemoteStopEVSEResultType.Success)
            {

                if (_ChargingSessions.ContainsKey(result.SessionId))
                    _ChargingSessions.Remove(result.SessionId);

            }

            return result;

        }

        #endregion

        #region RemoteStop(ChargingStationId, SessionId, ...)

        public Task<RemoteStopChargingStationResult>

            RemoteStop(ChargingStation_Id     ChargingStationId,
                       ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       eMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

            => Task.FromResult(RemoteStopChargingStationResult.OutOfService(SessionId));

        #endregion


    }

}