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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    public interface IRemoteEVSE
    {

        #region Events

        event OnStatusChangedDelegate       OnStatusChanged;
        event OnAdminStatusChangedDelegate  OnAdminStatusChanged;


        /// <summary>
        /// An event fired whenever a reserve command was received.
        /// </summary>
        //event OnReserveEVSEDelegate         OnReserve;

        /// <summary>
        /// An event fired whenever a reserve command completed.
        /// </summary>
        //event OnEVSEReservedDelegate        OnReserved;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        event OnNewReservationDelegate      OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation was cancelled.
        /// </summary>
        event OnReservationCancelledDelegate  OnReservationCancelled;


        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        //event OnRemoteEVSEStartDelegate     OnRemoteStart;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        //event OnRemoteEVSEStartedDelegate   OnRemoteStarted;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        event OnNewChargingSessionDelegate  OnNewChargingSession;


        #endregion


        #region Reserve/RemoteStart/RemoteStop

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
                                        ChargingProduct_Id       ChargingProductId  = null,
                                        IEnumerable<Auth_Token>  AuthTokens         = null,
                                        IEnumerable<eMA_Id>      eMAIds             = null,
                                        IEnumerable<UInt32>      PINs               = null,
                                        TimeSpan?                QueryTimeout       = null);


        Task<RemoteStartEVSEResult> RemoteStart(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, ChargingProduct_Id ChargingProductId, ChargingReservation_Id ReservationId, ChargingSession_Id SessionId, EVSP_Id ProviderId, eMA_Id eMAId, TimeSpan? QueryTimeout = null);
        Task<RemoteStopEVSEResult> RemoteStop(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, ChargingSession_Id SessionId, ReservationHandling ReservationHandling, EVSP_Id ProviderId, TimeSpan? QueryTimeout = null);

        #endregion




        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <returns>True when successful, false otherwise</returns>
        Task<Boolean> CancelReservation(ChargingReservation_Id           ReservationId,
                                        ChargingReservationCancellation  ReservationCancellation);




        Timestamped<EVSEAdminStatusType> AdminStatus { get; set; }
        IEnumerable<Timestamped<EVSEAdminStatusType>> AdminStatusSchedule { get; }
        double AverageVoltage { get; set; }
        ReactiveSet<ChargingFacilities> ChargingFacilities { get; set; }
        ReactiveSet<ChargingModes> ChargingModes { get; set; }
        IRemoteChargingStation ChargingStation { get; }
        ChargingSession ChargingSession { get; set; }
        I18NString Description { get; set; }
        double GuranteedMinPower { get; set; }
        double? MaxCapacity_kWh { get; set; }
        double MaxPower { get; set; }
        IVotingSender<DateTime, IRemoteEVSE, SocketOutlet, bool> OnSocketOutletAddition { get; }
        IVotingSender<DateTime, IRemoteEVSE, SocketOutlet, bool> OnSocketOutletRemoval { get; }
        EVSEOperator Operator { get; }
        string PointOfDelivery { get; set; }
        double RealTimePower { get; set; }
        ChargingReservation Reservation { get; set; }
        ReactiveSet<SocketOutlet> SocketOutlets { get; set; }
        Timestamped<EVSEStatusType> Status { get; set; }
        IEnumerable<Timestamped<EVSEStatusType>> StatusSchedule { get; }

     //   int CompareTo(VirtualEVSE VirtualEVSE);
        int CompareTo(object Object);
    //    bool Equals(VirtualEVSE VirtualEVSE);
        bool Equals(object Object);
        IEnumerator<SocketOutlet> GetEnumerator();
        int GetHashCode();
        void SetAdminStatus(Timestamped<EVSEAdminStatusType> NewAdminStatus);
        void SetAdminStatus(IEnumerable<Timestamped<EVSEAdminStatusType>> NewAdminStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetAdminStatus(DateTime Timestamp, EVSEAdminStatusType NewAdminStatus);
        void SetStatus(Timestamped<EVSEStatusType> NewStatus);
        void SetStatus(IEnumerable<Timestamped<EVSEStatusType>> NewStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetStatus(DateTime Timestamp, EVSEStatusType NewStatus);
        string ToString();

    }

}