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

        Timestamped<EVSEAdminStatusType> AdminStatus { get; set; }
        IEnumerable<Timestamped<EVSEAdminStatusType>> AdminStatusSchedule { get; }
        double AverageVoltage { get; set; }
        ReactiveSet<ChargingFacilities> ChargingFacilities { get; set; }
        ReactiveSet<ChargingModes> ChargingModes { get; set; }
        IRemoteChargingStation ChargingStation { get; }
        ChargingSession_Id CurrentChargingSession { get; set; }
        I18NString Description { get; set; }
        double GuranteedMinPower { get; set; }
        double? MaxCapacity_kWh { get; set; }
        double MaxPower { get; set; }
        IVotingSender<DateTime, EVSE, SocketOutlet, bool> OnSocketOutletAddition { get; }
        IVotingSender<DateTime, EVSE, SocketOutlet, bool> OnSocketOutletRemoval { get; }
        EVSEOperator Operator { get; }
        string PointOfDelivery { get; set; }
        double RealTimePower { get; set; }
        ChargingReservation Reservation { get; set; }
        ChargingReservation_Id ReservationId { get; }
        ReactiveSet<SocketOutlet> SocketOutlets { get; set; }
        Timestamped<EVSEStatusType> Status { get; set; }
        IEnumerable<Timestamped<EVSEStatusType>> StatusSchedule { get; }

        //event VirtualEVSE.OnAdminStatusChangedDelegate OnAdminStatusChanged;
        //event VirtualEVSE.OnNewReservationDelegate OnNewReservation;
        //event VirtualEVSE.OnReservationDeletedDelegate OnReservationDeleted;
        //event VirtualEVSE.OnStatusChangedDelegate OnStatusChanged;

     //   int CompareTo(VirtualEVSE VirtualEVSE);
        int CompareTo(object Object);
    //    bool Equals(VirtualEVSE VirtualEVSE);
        bool Equals(object Object);
        IEnumerator<SocketOutlet> GetEnumerator();
        int GetHashCode();
        Task<RemoteStartEVSEResult> RemoteStart(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, ChargingProduct_Id ChargingProductId, ChargingReservation_Id ReservationId, ChargingSession_Id SessionId, EVSP_Id ProviderId, eMA_Id eMAId, TimeSpan? QueryTimeout = null);
        Task<RemoteStopEVSEResult> RemoteStop(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, ChargingSession_Id SessionId, ReservationHandling ReservationHandling, EVSP_Id ProviderId, TimeSpan? QueryTimeout = null);
        Task<ReservationResult> Reserve(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, EVSP_Id ProviderId, ChargingReservation_Id ReservationId, DateTime? StartTime, TimeSpan? Duration, ChargingProduct_Id ChargingProductId = null, IEnumerable<Auth_Token> RFIDIds = null, IEnumerable<eMA_Id> eMAIds = null, IEnumerable<uint> PINs = null, TimeSpan? QueryTimeout = null);
        void SetAdminStatus(Timestamped<EVSEAdminStatusType> NewAdminStatus);
        void SetAdminStatus(IEnumerable<Timestamped<EVSEAdminStatusType>> NewAdminStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetAdminStatus(DateTime Timestamp, EVSEAdminStatusType NewAdminStatus);
        void SetStatus(Timestamped<EVSEStatusType> NewStatus);
        void SetStatus(IEnumerable<Timestamped<EVSEStatusType>> NewStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetStatus(DateTime Timestamp, EVSEStatusType NewStatus);
        string ToString();

    }

}