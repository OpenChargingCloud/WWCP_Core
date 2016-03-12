/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The interface of a remote charging station.
    /// </summary>
    public interface IRemoteChargingStation
    {

        IEnumerable<EVSE>                           EVSEs       { get; }
        ChargingStation_Id                          Id          { get; }
        Timestamped<ChargingStationStatusType>      Status      { get; }

        Timestamped<ChargingStationAdminStatusType> AdminStatus { get; set; }

        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEDataChangedDelegate         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEStatusChangedDelegate       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEAdminStatusChangedDelegate  OnEVSEAdminStatusChanged;

        #endregion

        #region OnChargingStationData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteChargingStationDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteChargingStationStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteChargingStationAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        ChargingStation_Id RemoteChargingStationId { get; set; }
        String                 RemoteEVSEIdPrefix  { get; set; }
        void AddMapping(EVSE_Id LocalEVSEId, EVSE_Id RemoteEVSEId);


        IRemoteEVSE CreateNewEVSE(EVSE_Id                           EVSEId,
                                  Action<EVSE>                      Configurator  = null,
                                  Action<EVSE>                      OnSuccess     = null,
                                  Action<ChargingStation, EVSE_Id>  OnError       = null);


        Task<IEnumerable<EVSEStatus>> GetEVSEStatus(DateTime                 Timestamp,
                                                    CancellationToken        CancellationToken,
                                                    EventTracking_Id         EventTrackingId,
                                                    TimeSpan?                QueryTimeout = null);


        #region Reserve(...)

        /// <summary>
        /// Reserve the possibility to charge.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<ReservationResult> Reserve(DateTime                 Timestamp,
                                        CancellationToken        CancellationToken,
                                        EventTracking_Id         EventTrackingId,
                                        DateTime?                StartTime,
                                        TimeSpan?                Duration,
                                        ChargingReservation_Id   ReservationId      = null,
                                        EVSP_Id                  ProviderId         = null,
                                        eMA_Id                   eMAId              = null,
                                        ChargingProduct_Id       ChargingProductId  = null,
                                        IEnumerable<Auth_Token>  AuthTokens         = null,
                                        IEnumerable<eMA_Id>      eMAIds             = null,
                                        IEnumerable<UInt32>      PINs               = null,
                                        TimeSpan?                QueryTimeout       = null);


        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<ReservationResult> Reserve(DateTime                 Timestamp,
                                        CancellationToken        CancellationToken,
                                        EventTracking_Id         EventTrackingId,
                                        EVSE_Id                  EVSEId,
                                        DateTime?                StartTime,
                                        TimeSpan?                Duration,
                                        ChargingReservation_Id   ReservationId      = null,
                                        EVSP_Id                  ProviderId         = null,
                                        eMA_Id                   eMAId              = null,
                                        ChargingProduct_Id       ChargingProductId  = null,
                                        IEnumerable<Auth_Token>  AuthTokens         = null,
                                        IEnumerable<eMA_Id>      eMAIds             = null,
                                        IEnumerable<UInt32>      PINs               = null,
                                        TimeSpan?                QueryTimeout       = null);


        event OnNewReservationDelegate OnNewReservation;


        /// <summary>
        /// Remote charging reservations.
        /// </summary>
        IEnumerable<ChargingReservation> ChargingReservations { get; }


        Boolean TryGetReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation);


        event OnReservationCancelledInternalDelegate OnReservationCancelled;

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<CancelReservationResult> CancelReservation(DateTime                               Timestamp,
                                                        CancellationToken                      CancellationToken,
                                                        EventTracking_Id                       EventTrackingId,
                                                        ChargingReservation_Id                 ReservationId,
                                                        ChargingReservationCancellationReason  Reason,
                                                        TimeSpan?                              QueryTimeout  = null);

        #endregion

        #region RemoteStart/-Stop

        event OnNewChargingSessionDelegate OnNewChargingSession;

        /// <summary>
        /// Initiate a remote start of the given charging session at the given charging station
        /// and for the given provider/eMAId.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<RemoteStartChargingStationResult> RemoteStart(DateTime                Timestamp,
                                                           CancellationToken       CancellationToken,
                                                           EventTracking_Id        EventTrackingId,
                                                           ChargingProduct_Id      ChargingProductId,
                                                           ChargingReservation_Id  ReservationId,
                                                           ChargingSession_Id      SessionId,
                                                           EVSP_Id                 ProviderId,
                                                           eMA_Id                  eMAId,
                                                           TimeSpan?               QueryTimeout  = null);

        /// <summary>
        /// Initiate a remote start of the given charging session at the given EVSE
        /// and for the given Provider/eMAId.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<RemoteStartEVSEResult> RemoteStart(DateTime                Timestamp,
                                                CancellationToken       CancellationToken,
                                                EventTracking_Id        EventTrackingId,
                                                EVSE_Id                 EVSEId,
                                                ChargingProduct_Id      ChargingProductId,
                                                ChargingReservation_Id  ReservationId,
                                                ChargingSession_Id      SessionId,
                                                EVSP_Id                 ProviderId,
                                                eMA_Id                  eMAId,
                                                TimeSpan?               QueryTimeout  = null);



        event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;



        /// <summary>
        /// Stop the given charging session at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<RemoteStopResult> RemoteStop(DateTime             Timestamp,
                                          CancellationToken    CancellationToken,
                                          EventTracking_Id     EventTrackingId,
                                          ChargingSession_Id   SessionId,
                                          ReservationHandling  ReservationHandling,
                                          EVSP_Id              ProviderId,
                                          eMA_Id               eMAId,
                                          TimeSpan?            QueryTimeout  = null);


        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<RemoteStopEVSEResult> RemoteStop(DateTime             Timestamp,
                                              CancellationToken    CancellationToken,
                                              EventTracking_Id     EventTrackingId,
                                              EVSE_Id              EVSEId,
                                              ChargingSession_Id   SessionId,
                                              ReservationHandling  ReservationHandling,
                                              EVSP_Id              ProviderId,
                                              eMA_Id               eMAId,
                                              TimeSpan?            QueryTimeout  = null);


        /// <summary>
        /// Stop the given charging session at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStationId">The unique identification of the charging station to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<RemoteStopChargingStationResult> RemoteStop(DateTime             Timestamp,
                                                         CancellationToken    CancellationToken,
                                                         EventTracking_Id     EventTrackingId,
                                                         ChargingStation_Id   ChargingStationId,
                                                         ChargingSession_Id   SessionId,
                                                         ReservationHandling  ReservationHandling,
                                                         EVSP_Id              ProviderId,
                                                         eMA_Id               eMAId,
                                                         TimeSpan?            QueryTimeout  = null);

        #endregion

    }

}