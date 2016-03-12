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
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The interface of a remote EVSE.
    /// </summary>
    public interface IRemoteEVSE
    {

        #region Properties

        EVSE_Id Id { get; }

        double AverageVoltage { get; set; }
        ReactiveSet<ChargingFacilities> ChargingFacilities { get; set; }
        ReactiveSet<ChargingModes> ChargingModes { get; set; }
        I18NString Description { get; set; }
        double GuranteedMinPower { get; set; }
        double? MaxCapacity_kWh { get; set; }
        double MaxPower { get; set; }
        string PointOfDelivery { get; set; }
        double RealTimePower { get; set; }
        ReactiveSet<SocketOutlet> SocketOutlets { get; set; }

        #endregion

        #region Events

        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        event OnNewReservationDelegate      OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation was cancelled.
        /// </summary>
        event OnReservationCancelledInternalDelegate  OnReservationCancelled;


        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        event OnNewChargingSessionDelegate  OnNewChargingSession;



        IVotingSender<DateTime, IRemoteEVSE, SocketOutlet, bool> OnSocketOutletAddition { get; }
        IVotingSender<DateTime, IRemoteEVSE, SocketOutlet, bool> OnSocketOutletRemoval  { get; }

        #endregion


        #region (Admin-)Status

        void SetAdminStatus(EVSEAdminStatusType NewAdminStatus);
        void SetAdminStatus(Timestamped<EVSEAdminStatusType> NewTimestampedAdminStatus);
        void SetAdminStatus(IEnumerable<Timestamped<EVSEAdminStatusType>> NewAdminStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetAdminStatus(EVSEAdminStatusType NewAdminStatus, DateTime Timestamp);

        Timestamped<EVSEAdminStatusType>              AdminStatus         { get; set; }
        IEnumerable<Timestamped<EVSEAdminStatusType>> AdminStatusSchedule { get; }


        void SetStatus(EVSEStatusType NewStatus);
        void SetStatus(Timestamped<EVSEStatusType> NewTimestampedStatus);
        void SetStatus(IEnumerable<Timestamped<EVSEStatusType>> NewStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetStatus(EVSEStatusType NewStatus, DateTime Timestamp);

        Timestamped<EVSEStatusType>              Status         { get; set; }
        IEnumerable<Timestamped<EVSEStatusType>> StatusSchedule { get; }

        #endregion

        #region Reservations

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

        ChargingReservation Reservation { get; set; }

        #endregion

        #region RemoteStart/-Stop

        /// <summary>
        /// Start a charging session.
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
        Task<RemoteStartEVSEResult> RemoteStart(DateTime                Timestamp,
                                                CancellationToken       CancellationToken,
                                                EventTracking_Id        EventTrackingId,
                                                ChargingProduct_Id      ChargingProductId,
                                                ChargingReservation_Id  ReservationId,
                                                ChargingSession_Id      SessionId,
                                                EVSP_Id                 ProviderId    = null,
                                                eMA_Id                  eMAId         = null,
                                                TimeSpan?               QueryTimeout  = null);


        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<RemoteStopEVSEResult> RemoteStop(DateTime             Timestamp,
                                              CancellationToken    CancellationToken,
                                              EventTracking_Id     EventTrackingId,
                                              ChargingSession_Id   SessionId,
                                              ReservationHandling  ReservationHandling,
                                              EVSP_Id              ProviderId    = null,
                                              eMA_Id               eMAId         = null,
                                              TimeSpan?            QueryTimeout  = null);

        ChargingSession ChargingSession { get; set; }

        #endregion



        IRemoteChargingStation ChargingStation { get; }
        EVSEOperator Operator { get; }

        IEnumerator<SocketOutlet> GetEnumerator();

        Int32   CompareTo(Object Object);
        Boolean Equals(Object Object);

        Int32   GetHashCode();
        String  ToString();

    }

}