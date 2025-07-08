///*
// * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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
//using System.Threading;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//using org.GraphDefined.Vanaheimr.Illias;
//using org.GraphDefined.Vanaheimr.Illias.Votes;
//using org.GraphDefined.Vanaheimr.Styx.Arrows;
//using org.GraphDefined.Vanaheimr.Hermod;
//using System.Linq;

//#endregion

//namespace cloud.charging.open.protocols.WWCP.Networking
//{

//    /// <summary>
//    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
//    /// This is meant to be one electrical circuit which can charge a electric vehicle
//    /// independently. Thus there could be multiple interdependent power sockets.
//    /// </summary>
//    public class NetworkEVSEStub : AEMobilityEntity<EVSE_Id>,
//                                   IEquatable<NetworkEVSEStub>, IComparable<NetworkEVSEStub>, IComparable,
//                                   IEnumerable<SocketOutlet>,
//                                   IStatus<EVSEStatusType>,
//                                   IRemoteEVSE
//    {

//        #region Data

//        /// <summary>
//        /// The default max size of the EVSE status history.
//        /// </summary>
//        public const UInt16 DefaultMaxEVSEStatusListSize = 50;

//        /// <summary>
//        /// The default max size of the EVSE admin status history.
//        /// </summary>
//        public const UInt16 DefaultMaxAdminStatusScheduleSize = 50;

//        /// <summary>
//        /// The maximum time span for a reservation.
//        /// </summary>
//        public static readonly TimeSpan MaxReservationDuration      = TimeSpan.FromMinutes(15);


//        public static readonly TimeSpan ReservationSelfCancelAfter  = TimeSpan.FromSeconds(10);

//        private static readonly Random _random = new Random();

//        #endregion

//        #region Properties

//        #region Description

//        private I18NString _Description;

//        /// <summary>
//        /// An description of this EVSE.
//        /// </summary>
//        [Mandatory]
//        public I18NString Description
//        {

//            get
//            {
//                return _Description ?? _ANetworkChargingStation.Description;
//            }

//            set
//            {

//                if (value == _ANetworkChargingStation.Description)
//                    value = null;

//                if (_Description != value)
//                    SetProperty<I18NString>(ref _Description, value);

//            }

//        }

//        #endregion


//        #region ChargingModes

//        private ReactiveSet<ChargingModes> _ChargingModes;

//        /// <summary>
//        /// Charging modes.
//        /// </summary>
//        [Mandatory]
//        public ReactiveSet<ChargingModes> ChargingModes
//        {

//            get
//            {
//                return _ChargingModes;
//            }

//            set
//            {

//                if (_ChargingModes != value)
//                    SetProperty(ref _ChargingModes, value);

//            }

//        }

//        #endregion

//        #region RMSVoltage

//        private Double _RMSVoltage;

//        /// <summary>
//        /// The average voltage.
//        /// </summary>
//        [Mandatory]
//        public Double RMSVoltage
//        {

//            get
//            {
//                return _RMSVoltage;
//            }

//            set
//            {

//                if (_RMSVoltage != value)
//                    SetProperty(ref _RMSVoltage, value);

//            }

//        }

//        #endregion

//        #region CurrentType

//        private CurrentTypes _CurrentType;

//        /// <summary>
//        /// The type of the current.
//        /// </summary>
//        [Mandatory]
//        public CurrentTypes CurrentType
//        {

//            get
//            {
//                return _CurrentType;
//            }

//            set
//            {

//                if (_CurrentType != value)
//                    SetProperty(ref _CurrentType, value);

//            }

//        }

//        #endregion

//        #region MaxCurrent

//        private Double _MaxCurrent;

//        /// <summary>
//        /// The maximum current [Ampere].
//        /// </summary>
//        [Mandatory]
//        public Double MaxCurrent
//        {

//            get
//            {
//                return _MaxCurrent;
//            }

//            set
//            {

//                if (_MaxCurrent != value)
//                    SetProperty(ref _MaxCurrent, value);

//            }

//        }

//        #endregion

//        #region MaxPower

//        private Double _MaxPower;

//        /// <summary>
//        /// The maximum power [kWatt].
//        /// </summary>
//        [Mandatory]
//        public Double MaxPower
//        {

//            get
//            {
//                return _MaxPower;
//            }

//            set
//            {

//                if (_MaxPower != value)
//                    SetProperty(ref _MaxPower, value);

//            }

//        }

//        #endregion

//        #region RealTimePower

//        private Double _RealTimePower;

//        /// <summary>
//        /// The current real-time power delivery [Watt].
//        /// </summary>
//        [Mandatory]
//        public Double RealTimePower
//        {

//            get
//            {
//                return _RealTimePower;
//            }

//            set
//            {

//                if (_RealTimePower != value)
//                    SetProperty(ref _RealTimePower, value);

//            }

//        }

//        #endregion

//        #region MaxCapacity

//        private Double? _MaxCapacity;

//        /// <summary>
//        /// The maximum capacity [kWh].
//        /// </summary>
//        [Mandatory]
//        public Double? MaxCapacity
//        {

//            get
//            {
//                return _MaxCapacity;
//            }

//            set
//            {

//                if (_MaxCapacity != value)
//                    SetProperty(ref _MaxCapacity, value);

//            }

//        }

//        #endregion

//        #region EnergyMeterId

//        private EnergyMeter_Id? _EnergyMeterId;

//        /// <summary>
//        /// The energy meter identification.
//        /// </summary>
//        [Optional]
//        public EnergyMeter_Id? EnergyMeterId
//        {

//            get
//            {
//                return _EnergyMeterId;
//            }

//            set
//            {

//                if (_EnergyMeterId != value)
//                    SetProperty<EnergyMeter_Id?>(ref _EnergyMeterId, value);

//            }

//        }

//        #endregion


//        #region SocketOutlets

//        private ReactiveSet<SocketOutlet> _SocketOutlets;

//        public ReactiveSet<SocketOutlet> SocketOutlets
//        {

//            get
//            {
//                return _SocketOutlets;
//            }

//            set
//            {

//                if (_SocketOutlets != value)
//                    SetProperty(ref _SocketOutlets, value);

//            }

//        }

//        #endregion


//        #region Status

//        /// <summary>
//        /// The current EVSE status.
//        /// </summary>
//        [InternalUseOnly]
//        public Timestamped<EVSEStatusType> Status
//        {

//            get
//            {
//                return _StatusSchedule.CurrentStatus;
//            }

//            set
//            {
//                SetStatus(value);
//            }

//        }

//        #endregion

//        #region StatusSchedule

//        private StatusSchedule<EVSEStatusType> _StatusSchedule;

//        /// <summary>
//        /// The EVSE status schedule.
//        /// </summary>
//        public IEnumerable<Timestamped<EVSEStatusType>> StatusSchedule
//        {
//            get
//            {
//                return _StatusSchedule;
//            }
//        }

//        #endregion

//        #region AdminStatus

//        /// <summary>
//        /// The current EVSE admin status.
//        /// </summary>
//        [InternalUseOnly]
//        public Timestamped<EVSEAdminStatusTypes> AdminStatus
//        {

//            get
//            {
//                return _AdminStatusSchedule.CurrentStatus;
//            }

//            set
//            {
//                SetAdminStatus(value);
//            }

//        }

//        #endregion

//        #region AdminStatusSchedule

//        private StatusSchedule<EVSEAdminStatusTypes> _AdminStatusSchedule;

//        /// <summary>
//        /// The EVSE admin status schedule.
//        /// </summary>
//        public IEnumerable<Timestamped<EVSEAdminStatusTypes>> AdminStatusSchedule
//        {
//            get
//            {
//                return _AdminStatusSchedule;
//            }
//        }

//        #endregion

//        #endregion

//        #region Links

//        #region ChargingStation

//        private readonly ANetworkChargingStation _ANetworkChargingStation;

//        /// <summary>
//        /// The charging station of this EVSE.
//        /// </summary>
//        [InternalUseOnly]
//        public IRemoteChargingStation ChargingStation
//            => _ANetworkChargingStation;

//        #endregion

//        #region OperatorId

//        /// <summary>
//        /// The identification of the operator of this EVSE.
//        /// </summary>
//        [InternalUseOnly]
//        public ChargingStationOperator_Id OperatorId
//            => _ANetworkChargingStation.Id.OperatorId;

//        #endregion

//        #endregion

//        #region Events

//        #region SocketOutletAddition

//        internal readonly IVotingNotificator<DateTime, IRemoteEVSE, SocketOutlet, Boolean> SocketOutletAddition;

//        /// <summary>
//        /// Called whenever a socket outlet will be or was added.
//        /// </summary>
//        public IVotingSender<DateTime, IRemoteEVSE, SocketOutlet, Boolean> OnSocketOutletAddition
//        {
//            get
//            {
//                return SocketOutletAddition;
//            }
//        }

//        #endregion

//        #region SocketOutletRemoval

//        internal readonly IVotingNotificator<DateTime, IRemoteEVSE, SocketOutlet, Boolean> SocketOutletRemoval;

//        /// <summary>
//        /// Called whenever a socket outlet will be or was removed.
//        /// </summary>
//        public IVotingSender<DateTime, IRemoteEVSE, SocketOutlet, Boolean> OnSocketOutletRemoval
//        {
//            get
//            {
//                return SocketOutletRemoval;
//            }
//        }

//        #endregion

//        #endregion

//        #region Constructor(s)

//        /// <summary>
//        /// Create a new Electric Vehicle Supply Equipment (EVSE) having the given EVSE identification.
//        /// </summary>
//        /// <param name="Id">The unique identification of this EVSE.</param>
//        /// <param name="ChargingStation">The parent charging station.</param>
//        /// <param name="MaxStatusScheduleSize">The maximum size of the EVSE status list.</param>
//        /// <param name="MaxAdminStatusScheduleSize">The maximum size of the EVSE admin status list.</param>
//        internal NetworkEVSEStub(EVSE_Id                  Id,
//                                 ANetworkChargingStation  ChargingStation,
//                                 UInt16                   MaxStatusScheduleSize       = DefaultMaxEVSEStatusListSize,
//                                 UInt16                   MaxAdminStatusScheduleSize  = DefaultMaxAdminStatusScheduleSize)

//            : base(Id)

//        {

//            #region Initial checks

//            if (ChargingStation == null)
//                throw new ArgumentNullException("ChargingStation", "The charging station must not be null!");

//            #endregion

//            #region Init data and properties

//            this._ANetworkChargingStation       = ChargingStation;

//            this._Description           = new I18NString();
//            this._ChargingModes         = new ReactiveSet<ChargingModes>();
//            this._SocketOutlets         = new ReactiveSet<SocketOutlet>();

//            this._StatusSchedule        = new StatusSchedule<EVSEStatusType>(MaxStatusScheduleSize);
//            this._StatusSchedule.Insert(EVSEStatusType.OutOfService);

//            this._AdminStatusSchedule   = new StatusSchedule<EVSEAdminStatusTypes>(MaxStatusScheduleSize);
//            this._AdminStatusSchedule.Insert(EVSEAdminStatusTypes.OutOfService);

//            #endregion

//            #region Init events

//            this.SocketOutletAddition   = new VotingNotificator<DateTime, IRemoteEVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);
//            this.SocketOutletRemoval    = new VotingNotificator<DateTime, IRemoteEVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

//            #endregion

//            #region Link events

//            this._StatusSchedule.     OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus)
//                                                          => UpdateStatus     (timestamp, eventTrackingId, newStatus, oldStatus);

//            this._AdminStatusSchedule.OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus)
//                                                          => UpdateAdminStatus(timestamp, eventTrackingId, newStatus, oldStatus);


//            //this.SocketOutletAddition.OnVoting        += (timestamp, evse, outlet, vote)
//            //                                              => ChargingStation.SocketOutletAddition.SendVoting      (timestamp, evse, outlet, vote);
//            //
//            //this.SocketOutletAddition.OnNotification  += (timestamp, evse, outlet)
//            //                                              => ChargingStation.SocketOutletAddition.SendNotification(timestamp, evse, outlet);
//            //
//            //this.SocketOutletRemoval. OnVoting        += (timestamp, evse, outlet, vote)
//            //                                              => ChargingStation.SocketOutletRemoval. SendVoting      (timestamp, evse, outlet, vote);
//            //
//            //this.SocketOutletRemoval. OnNotification  += (timestamp, evse, outlet)
//            //                                              => ChargingStation.SocketOutletRemoval. SendNotification(timestamp, evse, outlet);

//            #endregion


//            //this.OnStatusChanged += async (Timestamp,
//            //                               EventTrackingId,
//            //                               IRemoteEVSE,
//            //                               OldStatus,
//            //                               NewStatus) => {

//            //    if (OldStatus.Value == EVSEStatusType.Reserved &&
//            //        NewStatus.Value != EVSEStatusType.Reserved &&
//            //        _Reservation != null)
//            //    {

//            //        CancelReservation(_Reservation.Id,
//            //                          ChargingReservationCancellationReason.Aborted).Wait();

//            //    }

//            //};

//        }

//        #endregion


//        #region Data/(Admin-)Status management

//        #region OnData/(Admin)StatusChanged

//        /// <summary>
//        /// An event fired whenever the static data of the EVSE changed.
//        /// </summary>
//        public event OnRemoteEVSEDataChangedDelegate          OnDataChanged;

//        /// <summary>
//        /// An event fired whenever the dynamic status of the EVSE changed.
//        /// </summary>
//        public event OnRemoteEVSEStatusChangedDelegate        OnStatusChanged;

//        /// <summary>
//        /// An event fired whenever the admin status of the EVSE changed.
//        /// </summary>
//        public event OnRemoteEVSEAdminStatusChangedDelegate   OnAdminStatusChanged;

//        #endregion


//        #region SetStatus(NewStatus)

//        /// <summary>
//        /// Set the current status.
//        /// </summary>
//        /// <param name="NewStatus">A new status.</param>
//        public void SetStatus(EVSEStatusType  NewStatus)
//        {
//            _StatusSchedule.Insert(NewStatus);
//        }

//        #endregion

//        #region SetStatus(NewTimestampedStatus)

//        /// <summary>
//        /// Set the current status.
//        /// </summary>
//        /// <param name="NewTimestampedStatus">A new timestamped status.</param>
//        public void SetStatus(Timestamped<EVSEStatusType> NewTimestampedStatus)
//        {
//            _StatusSchedule.Insert(NewTimestampedStatus);
//        }

//        #endregion

//        #region SetStatus(NewStatus, Timestamp)

//        /// <summary>
//        /// Set the status.
//        /// </summary>
//        /// <param name="NewStatus">A new status.</param>
//        /// <param name="Timestamp">The timestamp when this change was detected.</param>
//        public void SetStatus(EVSEStatusType  NewStatus,
//                              DateTime        Timestamp)
//        {
//            _StatusSchedule.Insert(NewStatus, Timestamp);
//        }

//        #endregion

//        #region SetStatus(NewStatusList, ChangeMethod = ChangeMethods.Replace)

//        /// <summary>
//        /// Set the timestamped status.
//        /// </summary>
//        /// <param name="NewStatusList">A list of new timestamped status.</param>
//        /// <param name="ChangeMethod">The change mode.</param>
//        public void SetStatus(IEnumerable<Timestamped<EVSEStatusType>>  NewStatusList,
//                              ChangeMethods                             ChangeMethod = ChangeMethods.Replace)
//        {
//            _StatusSchedule.Set(NewStatusList, ChangeMethod);
//        }

//        #endregion


//        #region SetAdminStatus(NewAdminStatus)

//        /// <summary>
//        /// Set the admin status.
//        /// </summary>
//        /// <param name="NewAdminStatus">A new admin status.</param>
//        public void SetAdminStatus(EVSEAdminStatusTypes NewAdminStatus)
//        {
//            _AdminStatusSchedule.Insert(NewAdminStatus);
//        }

//        #endregion

//        #region SetAdminStatus(NewTimestampedAdminStatus)

//        /// <summary>
//        /// Set the admin status.
//        /// </summary>
//        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
//        public void SetAdminStatus(Timestamped<EVSEAdminStatusTypes> NewTimestampedAdminStatus)
//        {
//            _AdminStatusSchedule.Insert(NewTimestampedAdminStatus);
//        }

//        #endregion

//        #region SetAdminStatus(NewAdminStatus, Timestamp)

//        /// <summary>
//        /// Set the admin status.
//        /// </summary>
//        /// <param name="Timestamp">The timestamp when this change was detected.</param>
//        /// <param name="NewAdminStatus">A new admin status.</param>
//        public void SetAdminStatus(EVSEAdminStatusTypes  NewAdminStatus,
//                                   DateTime             Timestamp)
//        {
//            _AdminStatusSchedule.Insert(NewAdminStatus, Timestamp);
//        }

//        #endregion

//        #region SetAdminStatus(NewAdminStatusList, ChangeMethod = ChangeMethods.Replace)

//        /// <summary>
//        /// Set the timestamped admin status.
//        /// </summary>
//        /// <param name="NewAdminStatusList">A list of new timestamped admin status.</param>
//        /// <param name="ChangeMethod">The change mode.</param>
//        public void SetAdminStatus(IEnumerable<Timestamped<EVSEAdminStatusTypes>>  NewAdminStatusList,
//                                   ChangeMethods                                  ChangeMethod = ChangeMethods.Replace)
//        {
//            _AdminStatusSchedule.Set(NewAdminStatusList, ChangeMethod);
//        }

//        #endregion


//        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

//        /// <summary>
//        /// Update the current status.
//        /// </summary>
//        /// <param name="Timestamp">The timestamp when this change was detected.</param>
//        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
//        /// <param name="OldStatus">The old EVSE admin status.</param>
//        /// <param name="NewStatus">The new EVSE admin status.</param>
//        internal async Task UpdateAdminStatus(DateTime                          Timestamp,
//                                              EventTracking_Id                  EventTrackingId,
//                                              Timestamped<EVSEAdminStatusTypes>  OldStatus,
//                                              Timestamped<EVSEAdminStatusTypes>  NewStatus)
//        {

//            var onAdminStatusChanged = OnAdminStatusChanged;
//            if (onAdminStatusChanged != null)
//                await onAdminStatusChanged(Timestamp,
//                                                EventTrackingId,
//                                                this,
//                                                OldStatus,
//                                                NewStatus);

//        }

//        #endregion

//        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus)

//        /// <summary>
//        /// Update the current status.
//        /// </summary>
//        /// <param name="Timestamp">The timestamp when this change was detected.</param>
//        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
//        /// <param name="OldStatus">The old EVSE status.</param>
//        /// <param name="NewStatus">The new EVSE status.</param>
//        internal async Task UpdateStatus(DateTime                     Timestamp,
//                                         EventTracking_Id             EventTrackingId,
//                                         Timestamped<EVSEStatusType>  OldStatus,
//                                         Timestamped<EVSEStatusType>  NewStatus)
//        {

//            var onStatusChanged = OnStatusChanged;
//            if (onStatusChanged != null)
//                await onStatusChanged(Timestamp,
//                                           EventTrackingId,
//                                           this,
//                                           OldStatus,
//                                           NewStatus);

//        }

//        #endregion

//        #endregion

//        #region Reservations...

//        #region Data

//        private readonly Dictionary<ChargingReservation_Id, ChargingReservation> _Reservations;

//        /// <summary>
//        /// All current charging reservations.
//        /// </summary>
//        public IEnumerable<ChargingReservation> ChargingReservations
//            => _Reservations.Select(_ => _.Value);

//        #region TryGetReservationById(ReservationId, out Reservation)

//        /// <summary>
//        /// Return the charging reservation specified by the given identification.
//        /// </summary>
//        /// <param name="ReservationId">The charging reservation identification.</param>
//        /// <param name="Reservation">The charging reservation.</param>
//        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
//            => _Reservations.TryGetValue(ReservationId, out Reservation);

//        #endregion

//        #endregion

//        #region Events

//        /// <summary>
//        /// An event fired whenever a charging location is being reserved.
//        /// </summary>
//        public event OnReserveRequestDelegate             OnReserveRequest;

//        /// <summary>
//        /// An event fired whenever a charging location was reserved.
//        /// </summary>
//        public event OnReserveResponseDelegate            OnReserveResponse;

//        /// <summary>
//        /// An event fired whenever a new charging reservation was created.
//        /// </summary>
//        public event OnNewReservationDelegate             OnNewReservation;


//        /// <summary>
//        /// An event fired whenever a charging reservation is being canceled.
//        /// </summary>
//        public event OnCancelReservationRequestDelegate   OnCancelReservationRequest;

//        /// <summary>
//        /// An event fired whenever a charging reservation was canceled.
//        /// </summary>
//        public event OnCancelReservationResponseDelegate  OnCancelReservationResponse;

//        /// <summary>
//        /// An event fired whenever a charging reservation was canceled.
//        /// </summary>
//        public event OnReservationCanceledDelegate        OnReservationCanceled;

//        #endregion

//        #region Reserve(                                           StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

//        /// <summary>
//        /// Reserve the possibility to charge at this EVSE.
//        /// </summary>
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

//            Reserve(DateTime?                         StartTime              = null,
//                    TimeSpan?                         Duration               = null,
//                    ChargingReservation_Id?           ReservationId          = null,
//                    eMobilityProvider_Id?             ProviderId             = null,
//                    RemoteAuthentication              RemoteAuthentication   = null,
//                    ChargingProduct                   ChargingProduct        = null,
//                    IEnumerable<AuthenticationToken>           AuthTokens             = null,
//                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
//                    IEnumerable<UInt32>               PINs                   = null,

//                    DateTime?                         Timestamp              = null,
//                    CancellationToken?                CancellationToken      = null,
//                    EventTracking_Id                  EventTrackingId        = null,
//                    TimeSpan?                         RequestTimeout         = null)


//                => Reserve(ChargingLocation.FromEVSEId(Id),
//                           ChargingReservationLevel.EVSE,
//                           StartTime,
//                           Duration,
//                           ReservationId,
//                           ProviderId,
//                           RemoteAuthentication,
//                           ChargingProduct,
//                           AuthTokens,
//                           eMAIds,
//                           PINs,

//                           Timestamp,
//                           CancellationToken,
//                           EventTrackingId,
//                           RequestTimeout);

//        #endregion

//        #region Reserve(ChargingLocation, ReservationLevel = EVSE, StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

//        /// <summary>
//        /// Reserve the possibility to charge at the given charging location.
//        /// </summary>
//        /// <param name="ChargingLocation">A charging location.</param>
//        /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
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

//            Reserve(ChargingLocation                  ChargingLocation,
//                    ChargingReservationLevel          ReservationLevel       = ChargingReservationLevel.EVSE,
//                    DateTime?                         StartTime              = null,
//                    TimeSpan?                         Duration               = null,
//                    ChargingReservation_Id?           ReservationId          = null,
//                    eMobilityProvider_Id?             ProviderId             = null,
//                    RemoteAuthentication              RemoteAuthentication   = null,
//                    ChargingProduct                   ChargingProduct        = null,
//                    IEnumerable<AuthenticationToken>           AuthTokens             = null,
//                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
//                    IEnumerable<UInt32>               PINs                   = null,

//                    DateTime?                         Timestamp              = null,
//                    CancellationToken?                CancellationToken      = null,
//                    EventTracking_Id                  EventTrackingId        = null,
//                    TimeSpan?                         RequestTimeout         = null)

//        {

//            if (_ANetworkChargingStation == null)
//                return ReservationResult.OutOfService;

//            return await _ANetworkChargingStation.
//                             Reserve(ChargingLocation,
//                                     ReservationLevel,
//                                     StartTime,
//                                     Duration,
//                                     ReservationId,
//                                     ProviderId,
//                                     RemoteAuthentication,
//                                     ChargingProduct,
//                                     AuthTokens,
//                                     eMAIds,
//                                     PINs,

//                                     Timestamp,
//                                     CancellationToken,
//                                     EventTrackingId,
//                                     RequestTimeout);

//        }

//        #endregion


//        #region CancelReservation(ReservationId, Reason, ...)

//        /// <summary>
//        /// Try to remove the given charging reservation.
//        /// </summary>
//        /// <param name="ReservationId">The unique charging reservation identification.</param>
//        /// <param name="Reason">A reason for this cancellation.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public async Task<CancelReservationResult>

//            CancelReservation(ChargingReservation_Id                 ReservationId,
//                              ChargingReservationCancellationReason  Reason,

//                              DateTime?                              Timestamp          = null,
//                              CancellationToken?                     CancellationToken  = null,
//                              EventTracking_Id                       EventTrackingId    = null,
//                              TimeSpan?                              RequestTimeout     = null)

//        {

//            if (_ANetworkChargingStation == null)
//                return CancelReservationResult.OutOfService(ReservationId,
//                                                            Reason);

//            return await _ANetworkChargingStation.
//                             CancelReservation(ReservationId,
//                                               Reason,

//                                               Timestamp,
//                                               CancellationToken,
//                                               EventTrackingId,
//                                               RequestTimeout);

//        }

//        #endregion

//        #endregion

//        #region RemoteStart/-Stop and Sessions...

//        #region Data

//        private ChargingSession _ChargingSession;


//        public IEnumerable<ChargingSession> ChargingSessions
//            => _ChargingSession != null
//                   ? new ChargingSession[] { _ChargingSession }
//                   : new ChargingSession[0];

//        #region TryGetChargingSessionById(SessionId, out ChargingSession)

//        /// <summary>
//        /// Return the charging session specified by the given identification.
//        /// </summary>
//        /// <param name="SessionId">The charging session identification.</param>
//        /// <param name="ChargingSession">The charging session.</param>
//        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
//        {

//            if (SessionId == _ChargingSession.Id)
//            {
//                ChargingSession = _ChargingSession;
//                return true;
//            }

//            ChargingSession = null;
//            return false;

//        }

//        #endregion

//        /// <summary>
//        /// The current charging session, if available.
//        /// </summary>
//        [InternalUseOnly]
//        public ChargingSession ChargingSession
//        {

//            get
//            {
//                return _ChargingSession;
//            }

//            set
//            {

//                // Skip, if the charging session is already known... 
//                if (_ChargingSession != value)
//                {

//                    _ChargingSession = value;

//                    if (_ChargingSession != null)
//                    {

//                        SetStatus(EVSEStatusType.Charging);

//                        OnNewChargingSession?.Invoke(Timestamp.Now, this, _ChargingSession);

//                    }

//                    else
//                        SetStatus(EVSEStatusType.Available);

//                }

//            }

//        }

//        #endregion

//        #region Events

//        /// <summary>
//        /// An event fired whenever a remote start command was received.
//        /// </summary>
//        public event OnRemoteStartRequestDelegate     OnRemoteStartRequest;

//        /// <summary>
//        /// An event fired whenever a remote start command completed.
//        /// </summary>
//        public event OnRemoteStartResponseDelegate    OnRemoteStartResponse;

//        /// <summary>
//        /// An event fired whenever a new charging session was created.
//        /// </summary>
//        public event OnNewChargingSessionDelegate     OnNewChargingSession;


//        /// <summary>
//        /// An event fired whenever a remote stop command was received.
//        /// </summary>
//        public event OnRemoteStopRequestDelegate      OnRemoteStopRequest;

//        /// <summary>
//        /// An event fired whenever a remote stop command completed.
//        /// </summary>
//        public event OnRemoteStopResponseDelegate     OnRemoteStopResponse;

//        /// <summary>
//        /// An event fired whenever a new charge detail record was created.
//        /// </summary>
//        public event OnNewChargeDetailRecordDelegate  OnNewChargeDetailRecord;

//        #endregion

//        #region RemoteStart(                  ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

//        /// <summary>
//        /// Start a charging session.
//        /// </summary>
//        /// <param name="ChargingProduct">The chosen charging product.</param>
//        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
//        /// <param name="SessionId">The unique identification for this charging session.</param>
//        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
//        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<RemoteStartResult>

//            RemoteStart(ChargingProduct          ChargingProduct        = null,
//                        ChargingReservation_Id?  ReservationId          = null,
//                        ChargingSession_Id?      SessionId              = null,
//                        eMobilityProvider_Id?    ProviderId             = null,
//                        RemoteAuthentication     RemoteAuthentication   = null,

//                        DateTime?                Timestamp              = null,
//                        CancellationToken?       CancellationToken      = null,
//                        EventTracking_Id         EventTrackingId        = null,
//                        TimeSpan?                RequestTimeout         = null)


//                => RemoteStart(ChargingLocation.FromEVSEId(Id),
//                               ChargingProduct,
//                               ReservationId,
//                               SessionId,
//                               ProviderId,
//                               RemoteAuthentication,

//                               Timestamp,
//                               CancellationToken,
//                               EventTrackingId,
//                               RequestTimeout);

//        #endregion

//        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

//        /// <summary>
//        /// Start a charging session.
//        /// </summary>
//        /// <param name="ChargingLocation">The charging location.</param>
//        /// <param name="ChargingProduct">The chosen charging product.</param>
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
//                        ChargingProduct          ChargingProduct        = null,
//                        ChargingReservation_Id?  ReservationId          = null,
//                        ChargingSession_Id?      SessionId              = null,
//                        eMobilityProvider_Id?    ProviderId             = null,
//                        RemoteAuthentication     RemoteAuthentication   = null,

//                        DateTime?                Timestamp              = null,
//                        CancellationToken?       CancellationToken      = null,
//                        EventTracking_Id         EventTrackingId        = null,
//                        TimeSpan?                RequestTimeout         = null)
//        {

//            if (_ANetworkChargingStation == null)
//                return RemoteStartResult.OutOfService();

//            return await _ANetworkChargingStation.
//                             RemoteStart(ChargingLocation.FromEVSEId(Id),
//                                         ChargingProduct,
//                                         ReservationId,
//                                         SessionId,
//                                         ProviderId,
//                                         RemoteAuthentication,

//                                         Timestamp,
//                                         CancellationToken,
//                                         EventTrackingId,
//                                         RequestTimeout);

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

//            if (_ANetworkChargingStation == null)
//                return RemoteStopResult.OutOfService(SessionId);

//            return await _ANetworkChargingStation.
//                             RemoteStop(SessionId,
//                                        ReservationHandling,
//                                        ProviderId,
//                                        RemoteAuthentication,

//                                        Timestamp,
//                                        CancellationToken,
//                                        EventTrackingId,
//                                        RequestTimeout);

//        }

//        #endregion

//        #endregion


//        #region CheckIfReservationIsExpired()

//        /// <summary>
//        /// Check if the reservation is expired.
//        /// </summary>
//        public async Task CheckIfReservationIsExpired()
//        {
//            // ToDo: What to do here?
//        }

//        #endregion


//        #region IEnumerable<SocketOutlet> Members

//        /// <summary>
//        /// Return a socket outlet enumerator.
//        /// </summary>
//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            return _SocketOutlets.GetEnumerator();
//        }

//        /// <summary>
//        /// Return a socket outlet enumerator.
//        /// </summary>
//        public IEnumerator<SocketOutlet> GetEnumerator()
//        {
//            return _SocketOutlets.GetEnumerator();
//        }

//        #endregion

//        #region IComparable<RemoteEVSE> Members

//        #region CompareTo(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        public override Int32 CompareTo(Object Object)
//        {

//            if (Object == null)
//                throw new ArgumentNullException("The given object must not be null!");

//            // Check if the given object is a virtual EVSE.
//            var RemoteEVSE = Object as NetworkEVSEStub;
//            if ((Object) RemoteEVSE == null)
//                throw new ArgumentException("The given object is not a virtual EVSE!");

//            return CompareTo(RemoteEVSE);

//        }

//        #endregion

//        #region CompareTo(RemoteEVSE)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="RemoteEVSE">An virtual EVSE to compare with.</param>
//        public Int32 CompareTo(NetworkEVSEStub RemoteEVSE)
//        {

//            if ((Object) RemoteEVSE == null)
//                throw new ArgumentNullException(nameof(RemoteEVSE),  "The given virtual EVSE must not be null!");

//            return Id.CompareTo(RemoteEVSE.Id);

//        }

//        #endregion

//        #endregion

//        #region IEquatable<RemoteEVSE> Members

//        #region Equals(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public override Boolean Equals(Object Object)
//        {

//            if (Object == null)
//                return false;

//            // Check if the given object is a virtual EVSE.
//            var RemoteEVSE = Object as NetworkEVSEStub;
//            if ((Object) RemoteEVSE == null)
//                return false;

//            return this.Equals(RemoteEVSE);

//        }

//        #endregion

//        #region Equals(RemoteEVSE)

//        /// <summary>
//        /// Compares two virtual EVSEs for equality.
//        /// </summary>
//        /// <param name="RemoteEVSE">A virtual EVSE to compare with.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public Boolean Equals(NetworkEVSEStub RemoteEVSE)
//        {

//            if ((Object) RemoteEVSE == null)
//                return false;

//            return Id.Equals(RemoteEVSE.Id);

//        }

//        #endregion

//        #endregion

//        #region (override) GetHashCode()

//        /// <summary>
//        /// Get the hash code of this object.
//        /// </summary>
//        public override Int32 GetHashCode()

//            => Id.GetHashCode();

//        #endregion

//        #region (override) ToString()

//        /// <summary>
//        /// Return a text representation of this object.
//        /// </summary>
//        public override String ToString()

//            => Id.ToString();

//        #endregion

//    }

//}
