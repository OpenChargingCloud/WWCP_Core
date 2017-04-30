/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;

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
                        IStatus<EVSEStatusTypes>

    {

        #region Data

        private Double EPSILON = 0.01;

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

        /// <summary>
        /// An description of this EVSE.
        /// </summary>
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
                    SetProperty(ref _Description, value);

            }

        }

        #endregion


        #region ChargingModes

        private ReactiveSet<ChargingModes> _ChargingModes;

        /// <summary>
        /// Charging modes.
        /// </summary>
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

        #region AverageVoltage

        private Single _AverageVoltage;

        /// <summary>
        /// The average voltage.
        /// </summary>
        [Mandatory]
        public Single AverageVoltage
        {

            get
            {
                return _AverageVoltage;
            }

            set
            {

                if (Math.Abs(_AverageVoltage - value) > EPSILON)
                    SetProperty(ref _AverageVoltage, value);

            }

        }

        #endregion

        #region CurrentType

        private CurrentTypes _CurrentType;

        /// <summary>
        /// The type of the current.
        /// </summary>
        [Mandatory]
        public CurrentTypes CurrentType
        {

            get
            {
                return _CurrentType;
            }

            set
            {

                if (_CurrentType != value)
                    SetProperty(ref _CurrentType, value);

            }

        }

        #endregion

        #region MaxCurrent

        private Single _MaxCurrent;

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public Single MaxCurrent
        {

            get
            {
                return _MaxCurrent;
            }

            set
            {

                if (Math.Abs(_MaxCurrent - value) > EPSILON)
                    SetProperty(ref _MaxCurrent, value);

            }

        }

        #endregion

        #region MaxPower

        private Single? _MaxPower;

        /// <summary>
        /// The maximum power [kWatt].
        /// </summary>
        [Optional]
        public Single? MaxPower
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

        private Single? _RealTimePower;

        /// <summary>
        /// The current real-time power delivery [Watt].
        /// </summary>
        [Optional]
        public Single? RealTimePower
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

        #region MaxCapacity

        private Single? _MaxCapacity;

        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public Single? MaxCapacity
        {

            get
            {
                return _MaxCapacity;
            }

            set
            {

                if (_MaxCapacity != value)
                    SetProperty(ref _MaxCapacity, value);

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


        #region Status

        /// <summary>
        /// The current EVSE status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<EVSEStatusTypes> Status
        {

            get
            {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    return _StatusSchedule.CurrentStatus;

                }

                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            return new Timestamped<EVSEStatusTypes>(AdminStatus.Timestamp, EVSEStatusTypes.OutOfService);

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

        private readonly StatusSchedule<EVSEStatusTypes> _StatusSchedule;

        /// <summary>
        /// The EVSE status schedule.
        /// </summary>
        public IEnumerable<Timestamped<EVSEStatusTypes>> StatusSchedule(UInt64? HistorySize = null)
        {

             if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                 AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
             {

                if (HistorySize.HasValue)
                    return _StatusSchedule.Take(HistorySize);

                return _StatusSchedule;

            }

             else
             {

                 switch (AdminStatus.Value)
                 {

                     default:
                         return new Timestamped<EVSEStatusTypes>[] {
                                    new Timestamped<EVSEStatusTypes>(AdminStatus.Timestamp, EVSEStatusTypes.OutOfService)
                                };

                 }

             }

        }

        #endregion

        #region AdminStatus

        /// <summary>
        /// The current EVSE admin status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<EVSEAdminStatusTypes> AdminStatus
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

        private readonly StatusSchedule<EVSEAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The EVSE admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<EVSEAdminStatusTypes>> AdminStatusSchedule(UInt64? HistorySize = null)
        {

            if (HistorySize.HasValue)
                return _AdminStatusSchedule.Take(HistorySize);

            return _AdminStatusSchedule;

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

        /// <summary>
        /// The charging station of this EVSE.
        /// </summary>
        public ChargingStation ChargingStation   { get; }

        #endregion

        #region Operator

        /// <summary>
        /// The operator of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator Operator
        {
            get
            {
                return ChargingStation.ChargingPool.Operator;
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

        #region EVSE(Id, ...)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE) having the given EVSE identification.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="Configurator">A delegate to configure the newly created EVSE.</param>
        /// <param name="RemoteEVSECreator">A delegate to attach a remote EVSE.</param>
        /// <param name="InitialAdminStatus">An optional initial admin status of the EVSE.</param>
        /// <param name="InitialStatus">An optional initial status of the EVSE.</param>
        /// <param name="MaxAdminStatusListSize">An optional max length of the admin staus list.</param>
        /// <param name="MaxStatusListSize">An optional max length of the staus list.</param>
        public EVSE(EVSE_Id                    Id,
                    Action<EVSE>               Configurator             = null,
                    RemoteEVSECreatorDelegate  RemoteEVSECreator        = null,
                    EVSEAdminStatusTypes       InitialAdminStatus       = EVSEAdminStatusTypes.OutOfService,
                    EVSEStatusTypes            InitialStatus            = EVSEStatusTypes.     OutOfService,
                    UInt16                     MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                    UInt16                     MaxStatusListSize        = DefaultMaxEVSEStatusListSize)

            : base(Id)

        {

            #region Init data and properties

            this._Description           = new I18NString();
            this._ChargingModes         = new ReactiveSet<ChargingModes>();
            this._SocketOutlets         = new ReactiveSet<SocketOutlet>();

            this._StatusSchedule        = new StatusSchedule<EVSEStatusTypes>(MaxStatusListSize);
            this._StatusSchedule.     Insert(InitialStatus);

            this._AdminStatusSchedule   = new StatusSchedule<EVSEAdminStatusTypes>(MaxStatusListSize);
            this._AdminStatusSchedule.Insert(InitialAdminStatus);

            #endregion

            Configurator?.Invoke(this);

            #region Init events

            this.SocketOutletAddition   = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval    = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            #endregion

            #region Link events

            _AdminStatusSchedule.OnStatusChanged += (Timestamp,
                                                      EventTrackingId,
                                                      StatusSchedule,
                                                      OldStatus,
                                                      NewStatus)
                => UpdateAdminStatus(Timestamp,
                                     EventTrackingId,
                                     OldStatus,
                                     NewStatus);

            _StatusSchedule.     OnStatusChanged += (Timestamp,
                                                     EventTrackingId,
                                                     StatusSchedule,
                                                     OldStatus,
                                                     NewStatus)

                => UpdateStatus(Timestamp,
                                EventTrackingId,
                                OldStatus,
                                NewStatus);


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

            this.RemoteEVSE = RemoteEVSECreator?.Invoke(this);

        }

        #endregion

        #region EVSE(Id, ChargingStation, ...)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE) having the given EVSE identification.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="ChargingStation">The charging station hosting this EVSE.</param>
        /// <param name="Configurator">A delegate to configure the newly created EVSE.</param>
        /// <param name="RemoteEVSECreator">A delegate to attach a remote EVSE.</param>
        /// <param name="InitialAdminStatus">An optional initial admin status of the EVSE.</param>
        /// <param name="InitialStatus">An optional initial status of the EVSE.</param>
        /// <param name="MaxAdminStatusListSize">An optional max length of the admin staus list.</param>
        /// <param name="MaxStatusListSize">An optional max length of the staus list.</param>
        public EVSE(EVSE_Id                    Id,
                    ChargingStation            ChargingStation,
                    Action<EVSE>               Configurator             = null,
                    RemoteEVSECreatorDelegate  RemoteEVSECreator        = null,
                    EVSEAdminStatusTypes       InitialAdminStatus       = EVSEAdminStatusTypes.OutOfService,
                    EVSEStatusTypes            InitialStatus            = EVSEStatusTypes.     OutOfService,
                    UInt16                     MaxStatusListSize        = DefaultMaxEVSEStatusListSize,
                    UInt16                     MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize)

            : this(Id,
                   Configurator,
                   RemoteEVSECreator,
                   InitialAdminStatus,
                   InitialStatus,
                   MaxAdminStatusListSize,
                   MaxStatusListSize)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation),  "The charging station must not be null!");

            #endregion

            #region Init data and properties

            this.ChargingStation  = ChargingStation;

            #endregion

        }

        #endregion

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
        public void SetStatus(EVSEStatusTypes NewStatus)
        {
            _StatusSchedule.Insert(NewStatus);
        }

        #endregion

        #region SetStatus(NewTimestampedStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewTimestampedStatus">A new timestamped status.</param>
        public void SetStatus(Timestamped<EVSEStatusTypes> NewTimestampedStatus)
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
        public void SetStatus(EVSEStatusTypes  NewStatus,
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
        public void SetStatus(IEnumerable<Timestamped<EVSEStatusTypes>>  NewStatusList,
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
        public void SetAdminStatus(EVSEAdminStatusTypes NewAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<EVSEAdminStatusTypes> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(EVSEAdminStatusTypes  NewAdminStatus,
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
        public void SetAdminStatus(IEnumerable<Timestamped<EVSEAdminStatusTypes>>  NewAdminStatusList,
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

        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                          Timestamp,
                                              EventTracking_Id                  EventTrackingId,
                                              Timestamped<EVSEAdminStatusTypes>  OldStatus,
                                              Timestamped<EVSEAdminStatusTypes>  NewStatus)
        {

            var OnAdminStatusChangedLocal = OnAdminStatusChanged;
            if (OnAdminStatusChangedLocal != null)
                await OnAdminStatusChangedLocal(Timestamp,
                                                EventTrackingId,
                                                this,
                                                OldStatus,
                                                NewStatus);

        }

        #endregion

        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                     Timestamp,
                                         EventTracking_Id             EventTrackingId,
                                         Timestamped<EVSEStatusTypes>  OldStatus,
                                         Timestamped<EVSEStatusTypes>  NewStatus)
        {

            var OnStatusChangedLocal = OnStatusChanged;
            if (OnStatusChangedLocal != null)
                await OnStatusChangedLocal(Timestamp,
                                           EventTrackingId,
                                           this,
                                           OldStatus,
                                           NewStatus);

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

                        OnNewReservation?.Invoke(DateTime.Now, this, _Reservation);

                    }

                    //else
                    //    SetStatus(EVSEStatusType.Available);

                }

            }

        }

        #endregion

        #region OnReserveRequest/-Response / OnNewReservation

        /// <summary>
        /// An event fired whenever a reserve command was received.
        /// </summary>
        public event OnReserveEVSERequestDelegate   OnReserveRequest;

        /// <summary>
        /// An event fired whenever a reserve command completed.
        /// </summary>
        public event OnReserveEVSEResponseDelegate  OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate       OnNewReservation;

        #endregion


        #region Reserve(...StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge.
        /// </summary>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
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

            Reserve(DateTime?                         StartTime           = null,
                    TimeSpan?                         Duration            = null,
                    ChargingReservation_Id?           ReservationId       = null,
                    eMobilityProvider_Id?             ProviderId          = null,
                    eMobilityAccount_Id?              eMAId               = null,
                    ChargingProduct                   ChargingProduct     = null,
                    IEnumerable<Auth_Token>           AuthTokens          = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds              = null,
                    IEnumerable<UInt32>               PINs                = null,

                    DateTime?                         Timestamp           = null,
                    CancellationToken?                CancellationToken   = null,
                    EventTracking_Id                  EventTrackingId     = null,
                    TimeSpan?                         RequestTimeout      = null)

        {

            #region Initial checks

            ReservationResult result = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnReserveRequest?.Invoke(DateTime.Now,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                         ReservationId,
                                         Id,
                                         StartTime,
                                         Duration,
                                         ProviderId,
                                         eMAId,
                                         ChargingProduct,
                                         AuthTokens,
                                         eMAIds,
                                         PINs,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnReserveRequest));
            }

            #endregion


            if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
            {

                if (_RemoteEVSE != null)
                {

                    result = await _RemoteEVSE.
                                       ChargingStation.
                                           Reserve(Id,
                                                   StartTime,
                                                   Duration,
                                                   ReservationId,
                                                   ProviderId,
                                                   eMAId,
                                                   ChargingProduct,
                                                   AuthTokens,
                                                   eMAIds,
                                                   PINs,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);

                    if (result.Result == ReservationResultType.Success)
                    {

                        OnNewReservation?.Invoke(DateTime.Now,
                                                 this,
                                                 result.Reservation);

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


            #region Send OnReserveResponse event

            Runtime.Stop();

            try
            {

                OnReserveResponse?.Invoke(DateTime.Now,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                          ReservationId,
                                          Id,
                                          StartTime,
                                          Duration,
                                          ProviderId,
                                          eMAId,
                                          ChargingProduct,
                                          AuthTokens,
                                          eMAIds,
                                          PINs,
                                          result,
                                          Runtime.Elapsed,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnReserveResponse));
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
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              eMobilityProvider_Id?                  ProviderId         = null,

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)
        {

            #region Initial checks

            if (ReservationId == null)
                throw new ArgumentNullException(nameof(ReservationId),  "The given charging reservation identification must not be null!");

            CancelReservationResult result = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion


            if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
            {

                if (_Reservation == null)
                    result = CancelReservationResult.Success(ReservationId,
                                                             Reason);

                if (_Reservation.Id != ReservationId)
                    result = CancelReservationResult.UnknownReservationId(ReservationId,
                                                                          Reason);


                var SavedReservation = _Reservation;

                _Reservation = null;

                result = CancelReservationResult.Success(ReservationId,
                                                         Reason,
                                                         SavedReservation,
                                                         TimeSpan.FromMilliseconds(5));

                SendOnReservationCancelled(DateTime.Now,
                                           Timestamp.Value,
                                           this,
                                           EventTrackingId,
                                           new RoamingNetwork_Id?(),
                                           ProviderId,
                                           SavedReservation.Id,
                                           SavedReservation,
                                           Reason,
                                           result,
                                           TimeSpan.FromMilliseconds(5),
                                           RequestTimeout);


            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        result = CancelReservationResult.OutOfService(ReservationId,
                                                                      Reason,
                                                                      Runtime: TimeSpan.FromMilliseconds(5));
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
        public event OnCancelReservationResponseDelegate OnReservationCancelled;


        internal Task SendOnReservationCancelled(DateTime                               LogTimestamp,
                                                 DateTime                               RequestTimestamp,
                                                 Object                                 Sender,
                                                 EventTracking_Id                       EventTrackingId,

                                                 RoamingNetwork_Id?                     RoamingNetworkId,
                                                 eMobilityProvider_Id?                  ProviderId,
                                                 ChargingReservation_Id                 ReservationId,
                                                 ChargingReservation                    Reservation,
                                                 ChargingReservationCancellationReason  Reason,
                                                 CancelReservationResult                Result,
                                                 TimeSpan                               Runtime,
                                                 TimeSpan?                              RequestTimeout)
        {

            // Yes, this is really needed!
            _Reservation = null;

            return OnReservationCancelled?.Invoke(LogTimestamp,
                                                  RequestTimestamp,
                                                  Sender,
                                                  EventTrackingId,
                                                  RoamingNetworkId,
                                                  ProviderId,
                                                  ReservationId,
                                                  Reservation,
                                                  Reason,
                                                  Result,
                                                  Runtime,
                                                  RequestTimeout);

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions

        #region OnRemoteStart / OnRemoteStarted

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartEVSERequestDelegate    OnRemoteStart;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartEVSEResponseDelegate  OnRemoteStarted;

        #endregion

        #region RemoteStart(...ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartEVSEResult>

            RemoteStart(ChargingProduct          ChargingProduct     = null,
                        ChargingReservation_Id?  ReservationId       = null,
                        ChargingSession_Id?      SessionId           = null,
                        eMobilityProvider_Id?    ProviderId          = null,
                        eMobilityAccount_Id?     eMAId               = null,

                        DateTime?                Timestamp           = null,
                        CancellationToken?       CancellationToken   = null,
                        EventTracking_Id         EventTrackingId     = null,
                        TimeSpan?                RequestTimeout      = null)
        {

            #region Initial checks

            RemoteStartEVSEResult result = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteStart event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteStart?.Invoke(DateTime.Now,
                                      Timestamp.Value,
                                      this,
                                      EventTrackingId,
                                      ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                      Id,
                                      ChargingProduct,
                                      ReservationId,
                                      SessionId,
                                      ProviderId,
                                      eMAId,
                                      RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnRemoteStart));
            }

            #endregion


            if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
            {

                if (_RemoteEVSE != null)
                {

                    result = await _RemoteEVSE.
                                       ChargingStation.
                                       RemoteStart(Id,
                                                   ChargingProduct,
                                                   ReservationId,
                                                   SessionId,
                                                   ProviderId,
                                                   eMAId,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);

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

                OnRemoteStarted?.Invoke(DateTime.Now,
                                        Timestamp.Value,
                                        this,
                                        EventTrackingId,
                                        ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                        Id,
                                        ChargingProduct,
                                        ReservationId,
                                        SessionId,
                                        ProviderId,
                                        eMAId,
                                        RequestTimeout,
                                        result,
                                        Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnRemoteStart));
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

                        OnNewChargingSession?.Invoke(DateTime.Now, this, _ChargingSession);

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

            OnNewChargingSession?.Invoke(Timestamp, Sender, ChargingSession);

        }

        #endregion


        #region OnRemoteStop / OnRemoteStopped / 

        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopEVSERequestDelegate     OnRemoteStop;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopEVSEResponseDelegate  OnRemoteStopped;

        #endregion

        #region RemoteStop(...SessionId, ReservationHandling = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopEVSEResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling   = null,
                       eMobilityProvider_Id?  ProviderId            = null,
                       eMobilityAccount_Id?   eMAId                 = null,

                       DateTime?              Timestamp             = null,
                       CancellationToken?     CancellationToken     = null,
                       EventTracking_Id       EventTrackingId       = null,
                       TimeSpan?              RequestTimeout        = null)
        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given charging session identification must not be null!");

            RemoteStopEVSEResult result = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteStop?.Invoke(DateTime.Now,
                                     Timestamp.Value,
                                     this,
                                     EventTrackingId,
                                     ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                     Id,
                                     SessionId,
                                     ReservationHandling,
                                     ProviderId,
                                     eMAId,
                                     RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnRemoteStop));
            }

            #endregion


            if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
            {

                if (_RemoteEVSE != null)
                {

                    result = await _RemoteEVSE.
                                       ChargingStation.
                                       RemoteStop(Id,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout);

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

                OnRemoteStopped?.Invoke(DateTime.Now,
                                        Timestamp.Value,
                                        this,
                                        EventTrackingId,
                                        ChargingStation.ChargingPool.Operator.RoamingNetwork.Id,
                                        Id,
                                        SessionId,
                                        ReservationHandling,
                                        ProviderId,
                                        eMAId,
                                        RequestTimeout,
                                        result,
                                        Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnRemoteStopped));
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

            OnNewChargeDetailRecord?.Invoke(Timestamp, Sender, ChargeDetailRecord);

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
                throw new ArgumentNullException(nameof(EVSE),  "The given EVSE must not be null!");

            return Id.CompareTo(EVSE.Id);

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

            return Id.Equals(EVSE.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => Id.ToString();

        #endregion


        public class Builder //: ChargingStation.Builder
        {

            #region Properties

            public I18NString ChargingStationOperatorName { get; set; }

            public I18NString ChargingStationName { get; set; }

            /// <summary>
            /// The internal service identification of the charging station maintained by the Charging Station Operator.
            /// </summary>
            [Optional]
            public String ServiceIdentification { get; set; }

            [Optional]
            public String HubjectStationId { get; set; }

            /// <summary>
            /// The offical (multi-language) name of this charging station.
            /// </summary>
            [Mandatory]
            public I18NString Name { get; set; }

            /// <summary>
            /// An optional (multi-language) description of this charging station.
            /// </summary>
            [Optional]
            public I18NString Description { get; set; }

            /// <summary>
            /// A brand for this charging station
            /// is this is different from the Charging Station Operator.
            /// </summary>
            [Optional]
            public Brand Brand { get; set; }

            /// <summary>
            /// The address of this charging station.
            /// </summary>
            [Optional]
            public Address Address { get; set; }

            /// <summary>
            /// OSM Node Id.
            /// </summary>
            [Optional]
            public String OSM_NodeId { get; set; }

            /// <summary>
            /// The geographical location of this charging station.
            /// </summary>
            [Optional]
            public GeoCoordinate GeoLocation { get; set; }

            /// <summary>
            /// The address of the entrance to this charging station.
            /// (If different from 'Address').
            /// </summary>
            [Optional]
            public Address EntranceAddress { get; set; }

            /// <summary>
            /// The geographical location of the entrance to this charging station.
            /// (If different from 'GeoLocation').
            /// </summary>
            [Optional]
            public GeoCoordinate EntranceLocation { get; set; }

            /// <summary>
            /// The address of the exit of this charging station.
            /// (If different from 'Address').
            /// </summary>
            [Optional]
            public Address ExitAddress { get; set; }

            /// <summary>
            /// The geographical location of the exit of this charging station.
            /// (If different from 'GeoLocation').
            /// </summary>
            [Optional]
            public GeoCoordinate ExitLocation { get; set; }

            /// <summary>
            /// parking spaces reachable from this charging station.
            /// </summary>
            [Optional]
            public HashSet<ParkingSpace> ParkingSpaces { get; set; }

            /// <summary>
            /// The opening times of this charging station.
            /// </summary>
            public OpeningTimes OpeningTimes { get; set; }

            public HashSet<AuthenticationModes> AuthenticationModes { get; set; }

            [Mandatory]
            public HashSet<PaymentOptions> PaymentOptions { get; set; }

            [Optional]
            public AccessibilityTypes Accessibility { get; set; }

            /// <summary>
            /// The telephone number of the Charging Station Operator hotline.
            /// </summary>
            [Optional]
            public String HotlinePhoneNumber { get; set; }

            [Optional]
            public Boolean IsHubjectCompatible { get; set; }

            [Optional]
            public Boolean DynamicInfoAvailable { get; set; }

            /// <summary>
            /// A comment from the users.
            /// </summary>
            [Optional]
            public I18NString UserComment { get; set; }

            /// <summary>
            /// A comment from the service provider.
            /// </summary>
            [Optional]
            public I18NString ServiceProviderComment { get; set; }

            /// <summary>
            /// The grid connection of the charging station.
            /// </summary>
            [Optional]
            public GridConnection GridConnection { get; set; }

            /// <summary>
            /// The features of the charging station.
            /// </summary>
            [Optional]
            public ChargingStationUIFeatures UIFeatures { get; set; }

            /// <summary>
            /// URIs of photos of this charging station.
            /// </summary>
            [Optional]
            public HashSet<String> PhotoURI { get; set; }

            /// <summary>
            /// A delegate called to aggregate the dynamic status of all subordinated EVSEs.
            /// </summary>
            public Func<EVSEStatusReport, EVSEStatusTypes> StatusAggregationDelegate { get; set; }

            /// <summary>
            /// The charging station admin status schedule.
            /// </summary>
            public IEnumerable<Timestamped<EVSEAdminStatusTypes>> AdminStatusSchedule { get; set; }

            #endregion

            public Builder(EVSE_Id Id)
            { }

        }


    }

}
