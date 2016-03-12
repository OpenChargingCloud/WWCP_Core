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
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// This is meant to be one electrical circuit which can charge a electric vehicle
    /// independently. Thus there could be multiple interdependent power sockets.
    /// </summary>
    public class EVSE : AEMobilityEntity<EVSE_Id>,
                        IEquatable<EVSE>, IComparable<EVSE>, IComparable,
                        IEnumerable<SocketOutlet>,
                        IStatus<EVSEStatusType>

    {

        #region Data

        /// <summary>
        /// The default max size of the EVSE status history.
        /// </summary>
        public const UInt16 DefaultMaxEVSEStatusListSize    = 50;

        /// <summary>
        /// The default max size of the EVSE admin status history.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(15);

        #endregion

        #region Properties

        #region Description

        private I18NString _Description;

        [Mandatory]
        public I18NString Description
        {

            get
            {

                return _Description != null
                    ? _Description
                    : ChargingStation.Description;

            }

            set
            {

                if (value == ChargingStation.Description)
                    value = null;

                if (_Description != value)
                    SetProperty<I18NString>(ref _Description, value);

            }

        }

        #endregion

        #region AverageVoltage

        private Double _AverageVoltage;

        /// <summary>
        /// Average voltage at the connector [Volt].
        /// </summary>
        [Mandatory]
        public Double AverageVoltage
        {

            get
            {
                return _AverageVoltage;
            }

            set
            {

                if (_AverageVoltage != value)
                    SetProperty(ref _AverageVoltage, value);

            }

        }

        #endregion

        #region MaxPower

        private Double _MaxPower;

        /// <summary>
        /// Max power at connector [Watt].
        /// </summary>
        [Mandatory]
        public Double MaxPower
        {

            get
            {
                return _MaxPower;
            }

            set
            {

                if (_MaxPower != value)
                    SetProperty(ref _MaxPower, value);

            }

        }

        #endregion

        #region RealTimePower

        private Double _RealTimePower;

        /// <summary>
        /// Real-time power at connector [Watt].
        /// </summary>
        [Mandatory]
        public Double RealTimePower
        {

            get
            {
                return _RealTimePower;
            }

            set
            {

                if (_RealTimePower != value)
                    SetProperty(ref _RealTimePower, value);

            }

        }

        #endregion

        #region GuranteedMinPower

        private Double _GuranteedMinPower;

        /// <summary>
        /// Guranteed min power at connector [Watt].
        /// </summary>
        [Mandatory]
        public Double GuranteedMinPower
        {

            get
            {
                return _GuranteedMinPower;
            }

            set
            {

                if (_MaxPower != value)
                    SetProperty(ref _GuranteedMinPower, value);

            }

        }

        #endregion

        #region MaxCapacity_kWh

        private Double? _MaxCapacity_kWh;

        /// <summary>
        /// Max power capacity at the connector [kWh].
        /// </summary>
        [Mandatory]
        public Double? MaxCapacity_kWh
        {

            get
            {
                return _MaxCapacity_kWh;
            }

            set
            {

                if (_MaxCapacity_kWh != value)
                    SetProperty(ref _MaxCapacity_kWh, value);

            }

        }

        #endregion

        #region ChargingModes

        private ReactiveSet<ChargingModes> _ChargingModes;

        [Mandatory]
        public ReactiveSet<ChargingModes> ChargingModes
        {

            get
            {
                return _ChargingModes;
            }

            set
            {

                if (_ChargingModes != value)
                    SetProperty(ref _ChargingModes, value);

            }

        }

        #endregion

        #region ChargingFacilities

        private ReactiveSet<ChargingFacilities> _ChargingFacilities;

        [Mandatory]
        public ReactiveSet<ChargingFacilities> ChargingFacilities
        {

            get
            {
                return _ChargingFacilities;
            }

            set
            {

                if (_ChargingFacilities != value)
                    SetProperty(ref _ChargingFacilities, value);

            }

        }

        #endregion

        #region SocketOutlets

        private ReactiveSet<SocketOutlet> _SocketOutlets;

        public ReactiveSet<SocketOutlet> SocketOutlets
        {

            get
            {
                return _SocketOutlets;
            }

            set
            {

                if (_SocketOutlets != value)
                    SetProperty(ref _SocketOutlets, value);

            }

        }

        #endregion

        #region PointOfDelivery // MeterId

        private String _PointOfDelivery;

        /// <summary>
        /// Point of delivery or meter identification.
        /// </summary>
        [Optional]
        public String PointOfDelivery
        {

            get
            {
                return _PointOfDelivery;
            }

            set
            {

                if (_PointOfDelivery != value)
                    SetProperty<String>(ref _PointOfDelivery, value);

            }

        }

        #endregion


        #region Status

        /// <summary>
        /// The current EVSE status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<EVSEStatusType> Status
        {

            get
            {

                if (AdminStatus.Value == EVSEAdminStatusType.Operational ||
                    AdminStatus.Value == EVSEAdminStatusType.InternalUse)
                {

                    return _StatusSchedule.CurrentStatus;

                }

                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            return new Timestamped<EVSEStatusType>(AdminStatus.Timestamp, EVSEStatusType.OutOfService);

                    }

                }

            }

            set
            {

                if (value == null)
                    return;

                if (_StatusSchedule.CurrentValue != value.Value)
                    SetStatus(value);

            }

        }

        #endregion

        #region StatusSchedule

        private StatusSchedule<EVSEStatusType> _StatusSchedule;

        /// <summary>
        /// The EVSE status schedule.
        /// </summary>
        public IEnumerable<Timestamped<EVSEStatusType>> StatusSchedule
        {
            get
            {

                if (AdminStatus.Value == EVSEAdminStatusType.Operational ||
                    AdminStatus.Value == EVSEAdminStatusType.InternalUse)
                {

                    return _StatusSchedule;

                }

                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            return new Timestamped<EVSEStatusType>[] {
                                       new Timestamped<EVSEStatusType>(AdminStatus.Timestamp, EVSEStatusType.OutOfService)
                                   };

                    }

                }

            }
        }

        #endregion

        #region AdminStatus

        /// <summary>
        /// The current EVSE admin status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<EVSEAdminStatusType> AdminStatus
        {

            get
            {
                return _AdminStatusSchedule.CurrentStatus;
            }

            set
            {

                if (value == null)
                    return;

                if (_AdminStatusSchedule.CurrentValue != value.Value)
                    SetAdminStatus(value);

            }

        }

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<EVSEAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The EVSE admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<EVSEAdminStatusType>> AdminStatusSchedule
        {
            get
            {
                return _AdminStatusSchedule;
            }
        }

        #endregion

        #endregion

        #region Links

        #region RemoteEVSE

        private IRemoteEVSE _RemoteEVSE;

        /// <summary>
        /// An optional remote EVSE.
        /// </summary>
        public IRemoteEVSE RemoteEVSE
        {

            get
            {
                return _RemoteEVSE;
            }

            internal set
            {
                _RemoteEVSE = value;
            }

        }

        #endregion

        #region ChargingStation

        private readonly ChargingStation _ChargingStation;

        /// <summary>
        /// The charging station of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStation ChargingStation
        {
            get
            {
                return _ChargingStation;
            }
        }

        #endregion

        #region Operator

        /// <summary>
        /// The operator of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public EVSEOperator Operator
        {
            get
            {
                return _ChargingStation.ChargingPool.Operator;
            }
        }

        #endregion

        #endregion

        #region Events

        #region SocketOutletAddition

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletAddition
        {
            get
            {
                return SocketOutletAddition;
            }
        }

        #endregion

        #region SocketOutletRemoval

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletRemoval;

        /// <summary>
        /// Called whenever a socket outlet will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletRemoval
        {
            get
            {
                return SocketOutletRemoval;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE) having the given EVSE identification.
        /// </summary>
        /// <param name="Id">The unique identification of this EVSE.</param>
        /// <param name="ChargingStation">The parent charging station.</param>
        /// <param name="MaxStatusListSize">The maximum size of the EVSE status list.</param>
        /// <param name="MaxAdminStatusListSize">The maximum size of the EVSE admin status list.</param>
        internal EVSE(EVSE_Id          Id,
                      ChargingStation  ChargingStation,
                      UInt16           MaxStatusListSize       = DefaultMaxEVSEStatusListSize,
                      UInt16           MaxAdminStatusListSize  = DefaultMaxAdminStatusListSize)

            : base(Id)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The charging station must not be null!");

            #endregion

            #region Init data and properties

            this._ChargingStation       = ChargingStation;

            this._Description           = new I18NString();
            this._ChargingModes         = new ReactiveSet<ChargingModes>();
            this._ChargingFacilities    = new ReactiveSet<ChargingFacilities>();
            this._SocketOutlets         = new ReactiveSet<SocketOutlet>();

            this._StatusSchedule        = new StatusSchedule<EVSEStatusType>(MaxStatusListSize);
            this._StatusSchedule.     Insert(EVSEStatusType.     OutOfService);

            this._AdminStatusSchedule   = new StatusSchedule<EVSEAdminStatusType>(MaxStatusListSize);
            this._AdminStatusSchedule.Insert(EVSEAdminStatusType.OutOfService);

            #endregion

            #region Init events

            this.SocketOutletAddition   = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval    = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            #endregion

            #region Link events

            this._StatusSchedule.     OnStatusChanged += (Timestamp, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, OldStatus, NewStatus);

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, OldStatus, NewStatus);


            this.SocketOutletAddition.OnVoting        += (timestamp, evse, outlet, vote)
                                                          => ChargingStation.SocketOutletAddition.SendVoting      (timestamp, evse, outlet, vote);

            this.SocketOutletAddition.OnNotification  += (timestamp, evse, outlet)
                                                          => ChargingStation.SocketOutletAddition.SendNotification(timestamp, evse, outlet);

            this.SocketOutletRemoval. OnVoting        += (timestamp, evse, outlet, vote)
                                                          => ChargingStation.SocketOutletRemoval. SendVoting      (timestamp, evse, outlet, vote);

            this.SocketOutletRemoval. OnNotification  += (timestamp, evse, outlet)
                                                          => ChargingStation.SocketOutletRemoval. SendNotification(timestamp, evse, outlet);

            #endregion

            this.OnPropertyChanged += UpdateData;

        }

        #endregion


        #region Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        #region SetStatus(NewStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        public void SetStatus(EVSEStatusType NewStatus)
        {
            _StatusSchedule.Insert(NewStatus);
        }

        #endregion

        #region SetStatus(NewTimestampedStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewTimestampedStatus">A new timestamped status.</param>
        public void SetStatus(Timestamped<EVSEStatusType> NewTimestampedStatus)
        {
            _StatusSchedule.Insert(NewTimestampedStatus);
        }

        #endregion

        #region SetStatus(NewStatus, Timestamp)

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public void SetStatus(EVSEStatusType  NewStatus,
                              DateTime        Timestamp)
        {
            _StatusSchedule.Insert(NewStatus, Timestamp);
        }

        #endregion

        #region SetStatus(NewStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped status.
        /// </summary>
        /// <param name="NewStatusList">A list of new timestamped status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetStatus(IEnumerable<Timestamped<EVSEStatusType>>  NewStatusList,
                              ChangeMethods                             ChangeMethod = ChangeMethods.Replace)
        {
            _StatusSchedule.Insert(NewStatusList, ChangeMethod);
        }

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(EVSEAdminStatusType NewAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<EVSEAdminStatusType> NewTimestampedAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewTimestampedAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewAdminStatus, Timestamp)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="NewAdminStatus">A new admin status.</param>
        public void SetAdminStatus(EVSEAdminStatusType  NewAdminStatus,
                                   DateTime             Timestamp)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus, Timestamp);
        }

        #endregion

        #region SetAdminStatus(NewAdminStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped admin status.
        /// </summary>
        /// <param name="NewAdminStatusList">A list of new timestamped admin status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetAdminStatus(IEnumerable<Timestamped<EVSEAdminStatusType>>  NewAdminStatusList,
                                   ChangeMethods                                  ChangeMethod = ChangeMethods.Replace)
        {
            _AdminStatusSchedule.Insert(NewAdminStatusList, ChangeMethod);
        }

        #endregion


        #region (internal) UpdateData(Timestamp, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data of the EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="Sender">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateData(DateTime  Timestamp,
                                       Object    Sender,
                                       String    PropertyName,
                                       Object    OldValue,
                                       Object    NewValue)
        {

            var OnDataChangedLocal = OnDataChanged;
            if (OnDataChangedLocal != null)
                await OnDataChangedLocal(Timestamp, Sender as EVSE, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                     Timestamp,
                                         Timestamped<EVSEStatusType>  OldStatus,
                                         Timestamped<EVSEStatusType>  NewStatus)
        {

            var OnStatusChangedLocal = OnStatusChanged;
            if (OnStatusChangedLocal != null)
                await OnStatusChangedLocal(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                          Timestamp,
                                              Timestamped<EVSEAdminStatusType>  OldStatus,
                                              Timestamped<EVSEAdminStatusType>  NewStatus)
        {

            var OnAdminStatusChangedLocal = OnAdminStatusChanged;
            if (OnAdminStatusChangedLocal != null)
                await OnAdminStatusChangedLocal(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region Reservations

        #region Reservation

        private ChargingReservation _Reservation;

        /// <summary>
        /// The current charging reservation, if available.
        /// </summary>
        [InternalUseOnly]
        public ChargingReservation Reservation
        {

            get
            {
                return _Reservation;
            }

            set
            {

                // Skip, if the reservation is already known... 
                if (_Reservation != value)
                {

                    _Reservation = value;

                    if (_Reservation != null)
                    {

                        //SetStatus(EVSEStatusType.Reserved);

                        var OnNewReservationLocal = OnNewReservation;
                        if (OnNewReservationLocal != null)
                            OnNewReservationLocal(DateTime.Now, this, _Reservation);

                    }

                    //else
                    //    SetStatus(EVSEStatusType.Available);

                }

            }

        }

        #endregion

        #region OnReserve / OnReserved / OnNewReservation

        /// <summary>
        /// An event fired whenever a reserve command was received.
        /// </summary>
        public event OnEVSEReserveDelegate     OnReserve;

        /// <summary>
        /// An event fired whenever a reserve command completed.
        /// </summary>
        public event OnEVSEReservedDelegate    OnReserved;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate  OnNewReservation;

        #endregion


        #region Reserve(...StartTime, Duration, ReservationId = null, ProviderId = null, ...)

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
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(DateTime                 Timestamp,
                    CancellationToken        CancellationToken,
                    EventTracking_Id         EventTrackingId,
                    DateTime?                StartTime          = null,
                    TimeSpan?                Duration           = null,
                    ChargingReservation_Id   ReservationId      = null,
                    EVSP_Id                  ProviderId         = null,
                    eMA_Id                   eMAId              = null,
                    ChargingProduct_Id       ChargingProductId  = null,
                    IEnumerable<Auth_Token>  AuthTokens         = null,
                    IEnumerable<eMA_Id>      eMAIds             = null,
                    IEnumerable<UInt32>      PINs               = null,
                    TimeSpan?                QueryTimeout       = null)

        {

            #region Initial checks

            ReservationResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserve event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnReserveLocal = OnReserve;
                if (OnReserveLocal != null)
                    OnReserveLocal(this,
                                   Timestamp,
                                   EventTrackingId,
                                   _ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                   ReservationId,
                                   Id,
                                   StartTime,
                                   Duration,
                                   ProviderId,
                                   eMAId,
                                   ChargingProductId,
                                   AuthTokens,
                                   eMAIds,
                                   PINs,
                                   QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSE." + nameof(OnReserve));
            }

            #endregion


            if (AdminStatus.Value == EVSEAdminStatusType.Operational ||
                AdminStatus.Value == EVSEAdminStatusType.InternalUse)
            {

                if (_RemoteEVSE != null)
                {

                    result = await _RemoteEVSE.
                                       ChargingStation.
                                       Reserve(Timestamp,
                                               CancellationToken,
                                               EventTrackingId,
                                               Id,
                                               StartTime,
                                               Duration,
                                               ReservationId,
                                               ProviderId,
                                               eMAId,
                                               ChargingProductId,
                                               AuthTokens,
                                               eMAIds,
                                               PINs,
                                               QueryTimeout);

                    if (result.Result == ReservationResultType.Success)
                    {

                        var OnNewReservationLocal = OnNewReservation;
                        if (OnNewReservationLocal != null)
                            OnNewReservationLocal(DateTime.Now, this, result.Reservation);

                    }

                }

                else
                    result = ReservationResult.Offline;

            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        result = ReservationResult.OutOfService;
                        break;

                }

            }


            #region Send OnReserved event

            Runtime.Stop();

            try
            {

                var OnReservedLocal = OnReserved;
                if (OnReservedLocal != null)
                    OnReservedLocal(this,
                                    Timestamp,
                                    EventTrackingId,
                                    _ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                    ReservationId,
                                    Id,
                                    StartTime,
                                    Duration,
                                    ProviderId,
                                    eMAId,
                                    ChargingProductId,
                                    AuthTokens,
                                    eMAIds,
                                    PINs,
                                    result,
                                    Runtime.Elapsed,
                                    QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSE." + nameof(OnReserved));
            }

            #endregion

            return result;

        }

        #endregion

        #region CancelReservation(ReservationId)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <returns>True when successful, false otherwise</returns>
        public async Task<CancelReservationResult> CancelReservation(DateTime                               Timestamp,
                                                                     CancellationToken                      CancellationToken,
                                                                     EventTracking_Id                       EventTrackingId,
                                                                     ChargingReservation_Id                 ReservationId,
                                                                     ChargingReservationCancellationReason  Reason,
                                                                     TimeSpan?                              QueryTimeout  = null)
        {

            #region Initial checks

            if (ReservationId == null)
                throw new ArgumentNullException(nameof(ReservationId),  "The given charging reservation identification must not be null!");

            CancelReservationResult result = null;

            #endregion


            if (AdminStatus.Value == EVSEAdminStatusType.Operational ||
                AdminStatus.Value == EVSEAdminStatusType.InternalUse)
            {

                if (_Reservation == null)
                    result = CancelReservationResult.Success(ReservationId);

                if (_Reservation.Id != ReservationId)
                    result = CancelReservationResult.UnknownReservationId(ReservationId);


                var OldReservationId = _Reservation.Id;

                _Reservation = null;

                SendOnReservationCancelled(Timestamp,
                                           this,
                                           EventTrackingId,
                                           OldReservationId,
                                           Reason);

                result = CancelReservationResult.Success(ReservationId);

            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        result = CancelReservationResult.OutOfService;
                        break;

                }

            }

            return result;

        }

        #endregion

        #region OnReservationCancelled

        /// <summary>
        /// An event fired whenever a charging reservation was deleted.
        /// </summary>
        public event OnReservationCancelledInternalDelegate OnReservationCancelled;


        internal void SendOnReservationCancelled(DateTime                               Timestamp,
                                                 Object                                 Sender,
                                                 EventTracking_Id                       EventTrackingId,
                                                 ChargingReservation_Id                 ReservationId,
                                                 ChargingReservationCancellationReason  Reason)
        {

            // Yes, this is really needed!
            _Reservation = null;

            var OnReservationCancelledLocal = OnReservationCancelled;
            if (OnReservationCancelledLocal != null)
                OnReservationCancelledLocal(Timestamp,
                                            Sender,
                                            EventTrackingId,
                                            ReservationId,
                                            Reason);

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions

        #region OnRemoteStart / OnRemoteStarted

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteEVSEStartDelegate    OnRemoteStart;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteEVSEStartedDelegate  OnRemoteStarted;

        #endregion

        #region RemoteStart(...ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

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
        public async Task<RemoteStartEVSEResult>

            RemoteStart(DateTime                Timestamp,
                        CancellationToken       CancellationToken,
                        EventTracking_Id        EventTrackingId,
                        ChargingProduct_Id      ChargingProductId  = null,
                        ChargingReservation_Id  ReservationId      = null,
                        ChargingSession_Id      SessionId          = null,
                        EVSP_Id                 ProviderId         = null,
                        eMA_Id                  eMAId              = null,
                        TimeSpan?               QueryTimeout       = null)
        {

            #region Initial checks

            RemoteStartEVSEResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteStart event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnRemoteStartLocal = OnRemoteStart;
                if (OnRemoteStartLocal != null)
                    OnRemoteStartLocal(Timestamp,
                                       this,
                                       EventTrackingId,
                                       _ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                       Id,
                                       ChargingProductId,
                                       ReservationId,
                                       SessionId,
                                       ProviderId,
                                       eMAId,
                                       QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSE." + nameof(OnRemoteStart));
            }

            #endregion


            if (AdminStatus.Value == EVSEAdminStatusType.Operational ||
                AdminStatus.Value == EVSEAdminStatusType.InternalUse)
            {

                if (_RemoteEVSE != null)
                {

                    result = await _RemoteEVSE.
                                       ChargingStation.
                                       RemoteStart(Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   Id,
                                                   ChargingProductId,
                                                   ReservationId,
                                                   SessionId,
                                                   ProviderId,
                                                   eMAId,
                                                   QueryTimeout);

                    if (result.Result == RemoteStartEVSEResultType.Success)
                        result.Session.EVSE = this;

                }

                else
                    result = RemoteStartEVSEResult.Offline;

            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        result = RemoteStartEVSEResult.OutOfService;
                        break;

                }

            }


            #region Send OnRemoteStarted event

            Runtime.Stop();

            try
            {

                var OnRemoteStartedLocal = OnRemoteStarted;
                if (OnRemoteStartedLocal != null)
                    OnRemoteStartedLocal(Timestamp,
                                         this,
                                         EventTrackingId,
                                         _ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                         Id,
                                         ChargingProductId,
                                         ReservationId,
                                         SessionId,
                                         ProviderId,
                                         eMAId,
                                         QueryTimeout,
                                         result,
                                         Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log("EVSE." + nameof(OnRemoteStart));
            }

            #endregion

            return result;

        }

        #endregion

        #region ChargingSession

        private ChargingSession _ChargingSession;

        /// <summary>
        /// The current charging session, if available.
        /// </summary>
        [InternalUseOnly]
        public ChargingSession ChargingSession
        {

            get
            {
                return _ChargingSession;
            }

            set
            {

                // Skip, if the charging session is already known... 
                if (_ChargingSession != value)
                {

                    _ChargingSession = value;

                    if (_ChargingSession != null)
                    {

                        if (_ChargingSession.EVSE == null)
                            _ChargingSession.EVSE = this;

                        //SetStatus(EVSEStatusType.Charging);

                        var OnNewChargingSessionLocal = OnNewChargingSession;
                        if (OnNewChargingSessionLocal != null)
                            OnNewChargingSessionLocal(DateTime.Now, this, _ChargingSession);

                    }

                    //else
                    //    SetStatus(EVSEStatusType.Available);

                }

            }

        }

        #endregion

        #region OnNewChargingSession

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate OnNewChargingSession;


        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  ChargingSession)
        {

            var OnNewChargingSessionLocal = OnNewChargingSession;
            if (OnNewChargingSessionLocal != null)
                OnNewChargingSessionLocal(Timestamp, Sender, ChargingSession);

        }

        #endregion


        #region OnRemoteStop / OnRemoteStopped / 

        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteEVSEStopDelegate     OnRemoteStop;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteEVSEStoppedDelegate  OnRemoteStopped;

        #endregion

        #region RemoteStop(...SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

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
        public async Task<RemoteStopEVSEResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EventTracking_Id     EventTrackingId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId    = null,
                       eMA_Id               eMAId         = null,
                       TimeSpan?            QueryTimeout  = null)
        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given charging session identification must not be null!");

            RemoteStopEVSEResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnRemoteStopLocal = OnRemoteStop;
                if (OnRemoteStopLocal != null)
                    OnRemoteStopLocal(this,
                                      Timestamp,
                                      EventTrackingId,
                                      _ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                      Id,
                                      SessionId,
                                      ReservationHandling,
                                      ProviderId,
                                      eMAId,
                                      QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSE." + nameof(OnRemoteStop));
            }

            #endregion


            if (AdminStatus.Value == EVSEAdminStatusType.Operational ||
                AdminStatus.Value == EVSEAdminStatusType.InternalUse)
            {

                if (_RemoteEVSE != null)
                {

                    result = await _RemoteEVSE.
                                       ChargingStation.
                                       RemoteStop(Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  Id,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,
                                                  QueryTimeout);

                }

                else
                    result = RemoteStopEVSEResult.Offline(SessionId);

            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        result = RemoteStopEVSEResult.OutOfService(SessionId);
                        break;

                }

            }


            #region Send OnRemoteStopped event

            Runtime.Stop();

            try
            {

                var OnRemoteStoppedLocal = OnRemoteStopped;
                if (OnRemoteStoppedLocal != null)
                    OnRemoteStoppedLocal(this,
                                         Timestamp,
                                         EventTrackingId,
                                         _ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                         Id,
                                         SessionId,
                                         ReservationHandling,
                                         ProviderId,
                                         eMAId,
                                         QueryTimeout,
                                         result,
                                         Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log("EVSE." + nameof(OnRemoteStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region OnNewChargeDetailRecord

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;


        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            if (_ChargingSession != null &&
                _ChargingSession.Id == ChargeDetailRecord.SessionId)
            {

                _ChargingSession = null;

            }

            var OnNewChargeDetailRecordLocal = OnNewChargeDetailRecord;
            if (OnNewChargeDetailRecordLocal != null)
                OnNewChargeDetailRecordLocal(Timestamp, Sender, ChargeDetailRecord);

        }

        #endregion

        #endregion


        #region IEnumerable<SocketOutlet> Members

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _SocketOutlets.GetEnumerator();
        }

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        public IEnumerator<SocketOutlet> GetEnumerator()
        {
            return _SocketOutlets.GetEnumerator();
        }

        #endregion

        #region IComparable<EVSE> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSE.
            var EVSE = Object as EVSE;
            if ((Object) EVSE == null)
                throw new ArgumentException("The given object is not an EVSE!");

            return CompareTo(EVSE);

        }

        #endregion

        #region CompareTo(EVSE)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE">An EVSE to compare with.</param>
        public Int32 CompareTo(EVSE EVSE)
        {

            if ((Object) EVSE == null)
                throw new ArgumentNullException("The given EVSE must not be null!");

            return _Id.CompareTo(EVSE._Id);

        }

        #endregion

        #endregion

        #region IEquatable<EVSE> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an EVSE.
            var EVSE = Object as EVSE;
            if ((Object) EVSE == null)
                return false;

            return this.Equals(EVSE);

        }

        #endregion

        #region Equals(EVSE)

        /// <summary>
        /// Compares two EVSEs for equality.
        /// </summary>
        /// <param name="EVSE">An EVSE to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE EVSE)
        {

            if ((Object) EVSE == null)
                return false;

            return _Id.Equals(EVSE._Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return _Id.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _Id.ToString();
        }

        #endregion

    }

}
