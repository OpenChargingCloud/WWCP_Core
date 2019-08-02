/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Cloud <https://github.com/GraphDefined/WWCP_Cloud>
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

using Org.BouncyCastle.Bcpg.OpenPgp;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP.ChargingStations
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// This is meant to be one electrical circuit which can charge a electric vehicle
    /// independently. Thus there could be multiple interdependent power sockets.
    /// </summary>
    public class VirtualEVSE : AEMobilityEntity<EVSE_Id>,
                               IEquatable<VirtualEVSE>, IComparable<VirtualEVSE>, IComparable,
                               IEnumerable<SocketOutlet>,
                               IStatus<EVSEStatusTypes>,
                               IRemoteEVSE
    {

        #region Data

        /// <summary>
        /// The default max size of the status history.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 50;

        /// <summary>
        /// The default max size of the admin status history.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public  static readonly TimeSpan  MaxReservationDuration  = TimeSpan.FromMinutes(15);

        private static readonly Random    _random                 = new Random(DateTime.UtcNow.Millisecond);

        private                 Object    EnergyMeterLock;
        private                 Timer     EnergyMeterTimer;

        #endregion

        #region Properties

        public PgpSecretKeyRing  SecretKeyRing          { get; }

        public PgpPublicKeyRing  PublicKeyRing          { get; }

        public TimeSpan          EnergyMeterInterval    { get; }


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
                    : _ChargingStation.Description;

            }

            set
            {

                if (value == _ChargingStation.Description)
                    value = null;

                if (_Description != value)
                    SetProperty<I18NString>(ref _Description, value);

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

        private Double _AverageVoltage;

        /// <summary>
        /// The average voltage.
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

        private Double _MaxCurrent;

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public Double MaxCurrent
        {

            get
            {
                return _MaxCurrent;
            }

            set
            {

                if (_MaxCurrent != value)
                    SetProperty(ref _MaxCurrent, value);

            }

        }

        #endregion

        #region MaxPower

        private Double _MaxPower;

        /// <summary>
        /// The maximum power [kWatt].
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
        /// The current real-time power delivery [Watt].
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

        #region MaxCapacity

        private Double? _MaxCapacity;

        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public Double? MaxCapacity
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

        #region EnergyMeterId

        private EnergyMeter_Id? _EnergyMeterId;

        /// <summary>
        /// The energy meter identification.
        /// </summary>
        [Optional]
        public EnergyMeter_Id? EnergyMeterId
        {

            get
            {
                return _EnergyMeterId;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _EnergyMeterId, value);

                else
                    DeleteProperty(ref _EnergyMeterId);

            }

        }

        #endregion


        //public PgpSecretKeyRingBundle SecretKeys   { get; set; }
        //public PgpPublicKeyRingBundle PublicKeys   { get; set; }
        public String                 Passphrase   { get; set; }


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

        private StatusSchedule<EVSEStatusTypes> _StatusSchedule;

        /// <summary>
        /// The EVSE status schedule.
        /// </summary>
        public IEnumerable<Timestamped<EVSEStatusTypes>> StatusSchedule
        {
            get
            {
                return _StatusSchedule;
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

        private StatusSchedule<EVSEAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The EVSE admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<EVSEAdminStatusTypes>> AdminStatusSchedule
        {
            get
            {
                return _AdminStatusSchedule;
            }
        }

        #endregion

        #endregion

        #region Links

        #region ChargingStation

        private readonly VirtualChargingStation _ChargingStation;

        /// <summary>
        /// The charging station of this virtual EVSE.
        /// </summary>
        [InternalUseOnly]
        public IRemoteChargingStation ChargingStation
            => _ChargingStation;

        #endregion

        #region OperatorId

        /// <summary>
        /// The identification of the operator of this virtual EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator_Id OperatorId
            => _ChargingStation.Id.OperatorId;

        #endregion

        #endregion

        #region Events

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE) having the given EVSE identification.
        /// </summary>
        /// <param name="Id">The unique identification of this EVSE.</param>
        /// <param name="ChargingStation">The parent virtual charging station.</param>
        /// <param name="MaxAdminStatusListSize">The maximum size of the EVSE admin status list.</param>
        /// <param name="MaxStatusListSize">The maximum size of the EVSE status list.</param>
        internal VirtualEVSE(EVSE_Id                 Id,
                             VirtualChargingStation  ChargingStation,
                             EnergyMeter_Id          EnergyMeterId,
                             PgpSecretKeyRing        SecretKeyRing            = null,
                             PgpPublicKeyRing        PublicKeyRing            = null,
                             EVSEAdminStatusTypes    InitialAdminStatus       = EVSEAdminStatusTypes.Operational,
                             EVSEStatusTypes         InitialStatus            = EVSEStatusTypes.Available,
                             UInt16                  MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                             UInt16                  MaxStatusListSize        = DefaultMaxStatusListSize)

            : base(Id)

        {

            #region Init data and properties

            this._ChargingStation       = ChargingStation ?? throw new ArgumentNullException(nameof(ChargingStation), "The charging station must not be null!");

            this._Description           = new I18NString();
            this._ChargingModes         = new ReactiveSet<ChargingModes>();
            this._SocketOutlets         = new ReactiveSet<SocketOutlet>();

            this.EnergyMeterId          = EnergyMeterId;

            this._AdminStatusSchedule   = new StatusSchedule<EVSEAdminStatusTypes>(MaxAdminStatusListSize);
            this._AdminStatusSchedule.Insert(InitialAdminStatus);

            this._StatusSchedule        = new StatusSchedule<EVSEStatusTypes>(MaxStatusListSize);
            this._StatusSchedule.     Insert(InitialStatus);

            #endregion

            #region Link events

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this._StatusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

            EnergyMeterLock      = new Object();
            EnergyMeterTimer     = new Timer(ReadEnergyMeter, null, Timeout.Infinite, Timeout.Infinite);
            EnergyMeterInterval  = TimeSpan.FromSeconds(30);

        }

        #endregion


        #region (private, Timer) ReadEnergyMeter(Status)

        private void ReadEnergyMeter(Object Status)
        {

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            if (Monitor.TryEnter(EnergyMeterLock))
            {

                try
                {

                    ChargingSession.AddEnergyMeterValue(new Timestamped<Single>(DateTime.UtcNow, 1));

                }
                catch (Exception e)
                {
                    DebugX.LogT("'ReadEnergyMeter' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(EnergyMeterLock);
                }

            }

            else
                DebugX.LogT("'ReadEnergyMeter' skipped!");

        }

        #endregion



        #region Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of the EVSE changed.
        /// </summary>
        public event OnRemoteEVSEDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of the EVSE changed.
        /// </summary>
        public event OnRemoteEVSEStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of the EVSE changed.
        /// </summary>
        public event OnRemoteEVSEAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        #region SetStatus(NewStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        public void SetStatus(EVSEStatusTypes  NewStatus)
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
        /// <param name="NewAdminStatus">A new admin status.</param>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
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


        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                           Timestamp,
                                              EventTracking_Id                   EventTrackingId,
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
        internal async Task UpdateStatus(DateTime                      Timestamp,
                                         EventTracking_Id              EventTrackingId,
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

        #region Reservations...

        #region Reserve(...StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
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

            Reserve(ChargingReservationLevel          ReservationLevel,
                    DateTime?                         StartTime              = null,
                    TimeSpan?                         Duration               = null,
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

            if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
            {


                #region Check if this is a reservation update...

                if (_Reservation != null)
                {

                    // Same ids => it's an update!
                    if (_Reservation.Id == ReservationId)
                    {

                        var OldReservation = _Reservation; // Store already consumed reservation time!

                        this._Reservation = new ChargingReservation(OldReservation.Id,
                                                                    Timestamp.Value,
                                                                    OldReservation.StartTime,
                                                                    Duration. HasValue  ? Duration. Value : MaxReservationDuration,
                                                                    (StartTime.HasValue ? StartTime.Value : DateTime.UtcNow) + (Duration.HasValue ? Duration.Value : MaxReservationDuration),
                                                                    OldReservation.ConsumedReservationTime + OldReservation.Duration - OldReservation.TimeLeft,
                                                                    ReservationLevel,
                                                                    ProviderId,
                                                                    RemoteAuthentication,
                                                                    null, //ChargingStation.ChargingPool.EVSEOperator.RoamingNetwork,
                                                                    null, //ChargingStation.ChargingPool.Id,
                                                                    ChargingStation.Id,
                                                                    Id,
                                                                    ChargingProduct,
                                                                    AuthTokens,
                                                                    eMAIds,
                                                                    PINs);

                        OnNewReservation?.Invoke(DateTime.UtcNow, this, _Reservation);

                        return ReservationResult.Success(_Reservation);

                    }

                    return ReservationResult.AlreadyReserved;

                }

                #endregion


                switch (Status.Value)
                {

                    case EVSEStatusTypes.OutOfService:
                        return ReservationResult.OutOfService;

                    case EVSEStatusTypes.Charging:
                        return ReservationResult.AlreadyInUse;

                    case EVSEStatusTypes.Reserved:
                        return ReservationResult.AlreadyReserved;

                    case EVSEStatusTypes.Available:

                        // Will do: Status = EVSEStatusType.Reserved
                        // Will do: Send OnNewReservation event!
                        this.Reservation = new ChargingReservation(ReservationId:           ReservationId ?? ChargingReservation_Id.Parse(OperatorId, _random.RandomString(25)),
                                                                   Timestamp:               Timestamp.Value,
                                                                   StartTime:               StartTime. HasValue ? StartTime.Value : DateTime.UtcNow,
                                                                   Duration:                Duration.  HasValue ? Duration. Value : MaxReservationDuration,
                                                                   EndTime:                 (StartTime.HasValue ? StartTime.Value : DateTime.UtcNow) + (Duration.HasValue ? Duration.Value : MaxReservationDuration),
                                                                   ConsumedReservationTime: TimeSpan.FromSeconds(0),
                                                                   ReservationLevel:        ReservationLevel,
                                                                   ProviderId:              ProviderId,
                                                                   Identification:          RemoteAuthentication,
                                                                   RoamingNetwork:          null,
                                                                   ChargingPoolId:          null,
                                                                   ChargingStationId:       ChargingStation.Id,
                                                                   EVSEId:                  Id,
                                                                   ChargingProduct:         ChargingProduct,
                                                                   AuthTokens:              AuthTokens,
                                                                   eMAIds:                  eMAIds,
                                                                   PINs:                    PINs != null ? PINs : new UInt32[] { (UInt32)(_random.Next(1000000) + 100000) });

                        return ReservationResult.Success(_Reservation);

                    default:
                        return ReservationResult.Error();

                }

            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        return ReservationResult.OutOfService;

                }

            }

        }

        #endregion

        #region Reservation

        private ChargingReservation _Reservation;

        /// <summary>
        /// The charging reservation, if available.
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

                    if (value != null)
                    {

                        _Reservation = value;

                        SetStatus(EVSEStatusTypes.Reserved);

                        OnNewReservation?.Invoke(DateTime.UtcNow,
                                                 this,
                                                 _Reservation);

                    }

                    else
                    {

                        _Reservation = null;

                        SetStatus(EVSEStatusTypes.Available);

                        //OnReservationCancelled?.Invoke(DateTime.UtcNow,
                        //                               DateTime.UtcNow,
                        //                               this,
                        //                               EventTracking_Id.New,
                        //                               RoamingNetworkId,
                        //                               _Reservation.Id,
                        //                               _Reservation,
                        //                               ChargingReservationCancellationReason.EndOfProcess);

                    }

                }

            }

        }

        #endregion

        #region OnNewReservation

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate OnNewReservation;

        #endregion


        #region CheckIfReservationIsExpired()

        /// <summary>
        /// Check if the reservation is expired.
        /// </summary>
        public async Task CheckIfReservationIsExpired()
        {

            if (_Reservation != null &&
                Status.Value == EVSEStatusTypes.Reserved &&
                _Reservation.IsExpired())
            {

                await __CancelReservation(_Reservation.Id,
                                          ChargingReservationCancellationReason.Expired);

            }

        }

        #endregion

        #region (private) __CancelReservation(...ReservationId, Reason, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        private async Task<CancelReservationResult>

            __CancelReservation(ChargingReservation_Id                 ReservationId,
                                ChargingReservationCancellationReason  Reason,
                                eMobilityProvider_Id?                  ProviderId         = null,

                                DateTime?                              Timestamp          = null,
                                CancellationToken?                     CancellationToken  = null,
                                EventTracking_Id                       EventTrackingId    = null,
                                TimeSpan?                              RequestTimeout     = null)

        {

            if (_Reservation == null || _Reservation.Id != ReservationId)
                return CancelReservationResult.UnknownReservationId(ReservationId,
                                                                    Reason);


            var SavedReservation = _Reservation;

            _Reservation = null;

            var result = CancelReservationResult.Success(ReservationId,
                                                         Reason,
                                                         SavedReservation);

            OnReservationCancelled?.Invoke(DateTime.UtcNow,
                                           Timestamp.HasValue
                                               ? Timestamp.Value
                                               : DateTime.UtcNow,
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

            // Will send events!
            SetStatus(EVSEStatusTypes.Available);

            return result;

        }

        #endregion

        #region CancelReservation(...ReservationId, Reason, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              eMobilityProvider_Id?                  ProviderId         = null,

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)

                return await __CancelReservation(ReservationId,
                                                 Reason,
                                                 ProviderId,

                                                 Timestamp,
                                                 CancellationToken,
                                                 EventTrackingId,
                                                 RequestTimeout);


            switch (AdminStatus.Value)
            {

                default:
                    return CancelReservationResult.OutOfService(ReservationId,
                                                                Reason);

            }

        }

        #endregion

        #region OnReservationCancelled

        /// <summary>
        /// An event fired whenever a charging reservation was deleted.
        /// </summary>
        public event OnCancelReservationResponseDelegate OnReservationCancelled;

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions...

        #region RemoteStart(...ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
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

            RemoteStart(ChargingProduct          ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        eMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)

        {

            #region Initial checks

            if (SessionId == null)
                SessionId = ChargingSession_Id.New;

            #endregion


            if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
            {

                switch (Status.Value)
                {

                    #region Available

                    case EVSEStatusTypes.Available:
                    case EVSEStatusTypes.DoorNotClosed:

                        // Will also set the status -> EVSEStatusType.Charging!
                        ChargingSession = new ChargingSession(SessionId.Value) {
                                              EventTrackingId      = EventTrackingId,
                                              Reservation          = Reservation != null && Reservation.Id == ReservationId ? Reservation : null,
                                              ReservationId        = ReservationId,
                                              EVSEId               = Id,
                                              ChargingProduct      = ChargingProduct,
                                              ProviderIdStart      = ProviderId,
                                              IdentificationStart  = RemoteAuthentication,
                                          };

                        ChargingSession.AddEnergyMeterValue(new Timestamped<Single>(DateTime.UtcNow, 0));
                        EnergyMeterTimer.Change(EnergyMeterInterval, EnergyMeterInterval);

                        return RemoteStartEVSEResult.Success(_ChargingSession);

                    #endregion

                    #region Reserved

                    case EVSEStatusTypes.Reserved:

                        #region Not matching reservation identifications...

                        if (Reservation != null && ReservationId == null)
                            return RemoteStartEVSEResult.Reserved("Missing reservation identification!");

                        if (Reservation != null && ReservationId != Reservation.Id)
                            return RemoteStartEVSEResult.Reserved("Invalid reservation identification!");

                        #endregion

                        #region ...or a matching reservation identification!

                        // Check if this remote start is allowed!
                        if (RemoteAuthentication?.RemoteIdentification.HasValue == true &&
                            !Reservation.eMAIds.Contains(RemoteAuthentication.RemoteIdentification.Value))
                        {
                            return RemoteStartEVSEResult.InvalidCredentials;
                        }


                        Reservation.AddToConsumedReservationTime(Reservation.Duration - Reservation.TimeLeft);

                        // Will also set the status -> EVSEStatusType.Charging;
                        ChargingSession  = new ChargingSession(SessionId.Value) {
                                                               EventTrackingId      = EventTrackingId,
                                                               Reservation          = Reservation,
                                                               ProviderIdStart      = ProviderId,
                                                               IdentificationStart  = RemoteAuthentication,
                                                               EVSEId               = Id,
                                                               ChargingProduct      = ChargingProduct
                        };


                        Reservation.ChargingSession = ChargingSession;

                        ChargingSession.AddEnergyMeterValue(new Timestamped<Single>(DateTime.UtcNow, 0));
                        EnergyMeterTimer.Change(EnergyMeterInterval, EnergyMeterInterval);

                        return RemoteStartEVSEResult.Success(_ChargingSession);

                        #endregion

                    #endregion

                    #region Charging

                    case EVSEStatusTypes.Charging:
                        return RemoteStartEVSEResult.AlreadyInUse;

                    #endregion

                    #region OutOfService

                    case EVSEStatusTypes.OutOfService:
                        return RemoteStartEVSEResult.OutOfService;

                    #endregion

                    #region Offline

                    case EVSEStatusTypes.Offline:
                        return RemoteStartEVSEResult.Offline;

                    #endregion

                }

                return RemoteStartEVSEResult.Error("Could not start charging!");

            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        return RemoteStartEVSEResult.OutOfService;

                }

            }

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

                        SetStatus(EVSEStatusTypes.Charging);

                        OnNewChargingSession?.Invoke(DateTime.UtcNow, this, _ChargingSession);

                    }

                    else
                        SetStatus(EVSEStatusTypes.Available);

                }

            }

        }

        #endregion

        #region OnNewChargingSession

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate OnNewChargingSession;

        #endregion

        #region RemoteStop(...SessionId, ReservationHandling = null, ProviderId = null, eMAId = null, ...)

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
        public async Task<RemoteStopEVSEResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       eMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
            {

                switch (Status.Value)
                {

                    #region Available

                    case EVSEStatusTypes.Available:
                        return RemoteStopEVSEResult.InvalidSessionId(SessionId);

                    #endregion

                    #region Reserved

                    case EVSEStatusTypes.Reserved:
                        return RemoteStopEVSEResult.InvalidSessionId(SessionId);

                    #endregion

                    #region Charging

                    case EVSEStatusTypes.Charging:

                        #region Matching session identification...

                        if (ChargingSession.Id == SessionId)
                        {

                            EnergyMeterTimer.Change(Timeout.Infinite, Timeout.Infinite);

                            var Now                  = DateTime.UtcNow;
                            var Duration             = Now - _ChargingSession.SessionTime.StartTime;
                            var Consumption          = (Single) Math.Round(Duration.TotalHours * MaxPower, 2);

                            ChargingSession.AddEnergyMeterValue(new Timestamped<Single>(Now, 0));

                            var _ChargeDetailRecord  = new ChargeDetailRecord(SessionId:                 _ChargingSession.Id,
                                                                              Reservation:               _ChargingSession.Reservation,
                                                                              EVSEId:                    _ChargingSession.EVSEId,
                                                                              EVSE:                      _ChargingSession.EVSE,
                                                                              ChargingStation:           _ChargingSession.EVSE?.ChargingStation,
                                                                              ChargingPool:              _ChargingSession.EVSE?.ChargingStation?.ChargingPool,
                                                                              ChargingStationOperator:   _ChargingSession.EVSE?.Operator,
                                                                              ChargingProduct:           _ChargingSession.ChargingProduct,
                                                                                ProviderIdStart:           _ChargingSession.ProviderIdStart,
                                                                                ProviderIdStop:            _ChargingSession.ProviderIdStop,
                                                                              SessionTime:               _ChargingSession.SessionTime,

                                                                                IdentificationStart:       _ChargingSession.IdentificationStart,
                                                                                IdentificationStop:        _ChargingSession.IdentificationStop,

                                                                              EnergyMeterId:             EnergyMeterId,
                                                                              EnergyMeteringValues:      ChargingSession.EnergyMeteringValues
                                                                              //SignedMeteringValues:      ChargingSession.EnergyMeteringValues.Select(metervalue =>
                                                                              //                               new SignedMeteringValue(metervalue.Timestamp,
                                                                              //                                                       metervalue.Value,
                                                                              //                                                       EnergyMeterId.Value,
                                                                              //                                                       Id,
                                                                              //                                                       _ChargingSession.IdentificationStart,
                                                                              //                                                       PublicKeyRing.First()).
                                                                              //                                   Sign(SecretKeyRing.First(),
                                                                              //                                        Passphrase))
                                                                                                               //                      PublicKeys.First().First()).
                                                                                                               //  Sign(SecretKeys.First().First(),
                                                                                                               //       Passphrase),


                                                                             );

                            // Will do: Status = EVSEStatusType.Available
                            ChargingSession = null;

                            if (!ReservationHandling.HasValue ||
                                (ReservationHandling.HasValue &&
                                !ReservationHandling.Value.IsKeepAlive))
                            {

                                // Will do: Status = EVSEStatusType.Available
                                Reservation = null;

                            }

                            else
                            {
                                //ToDo: Reservation will live on!
                            }


                            OnNewChargeDetailRecord?.Invoke(DateTime.UtcNow,
                                                            this,
                                                            _ChargeDetailRecord);

                            return RemoteStopEVSEResult.Success(_ChargeDetailRecord,
                                                                _ChargingSession.Reservation?.Id,
                                                                ReservationHandling);

                        }

                        #endregion

                        #region ...or unknown session identification!

                        else
                            return RemoteStopEVSEResult.InvalidSessionId(SessionId);

                        #endregion

                    #endregion

                    #region OutOfService

                    case EVSEStatusTypes.OutOfService:
                        return RemoteStopEVSEResult.OutOfService(SessionId);

                    #endregion

                    #region Offline

                    case EVSEStatusTypes.Offline:
                        return RemoteStopEVSEResult.Offline(SessionId);

                    #endregion

                }

                return RemoteStopEVSEResult.Error(SessionId);

            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        return RemoteStopEVSEResult.OutOfService(SessionId);

                }

            }

        }

        #endregion

        #region OnNewChargeDetailRecord

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;

        #endregion

        #endregion


        #region IEnumerable<SocketOutlet> Members

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => _SocketOutlets.GetEnumerator();

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        public IEnumerator<SocketOutlet> GetEnumerator()
            => _SocketOutlets.GetEnumerator();

        #endregion

        #region IComparable<VirtualEVSE> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var VirtualEVSE = Object as VirtualEVSE;
            if ((Object) VirtualEVSE == null)
                throw new ArgumentException("The given object is not a virtual EVSE!");

            return CompareTo(VirtualEVSE);

        }

        #endregion

        #region CompareTo(VirtualEVSE)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE">An virtual EVSE to compare with.</param>
        public Int32 CompareTo(VirtualEVSE VirtualEVSE)
        {

            if ((Object) VirtualEVSE == null)
                throw new ArgumentNullException(nameof(VirtualEVSE),  "The given virtual EVSE must not be null!");

            return Id.CompareTo(VirtualEVSE.Id);

        }

        #endregion

        #endregion

        #region IEquatable<VirtualEVSE> Members

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

            var VirtualEVSE = Object as VirtualEVSE;
            if ((Object) VirtualEVSE == null)
                return false;

            return Equals(VirtualEVSE);

        }

        #endregion

        #region Equals(VirtualEVSE)

        /// <summary>
        /// Compares two virtual EVSEs for equality.
        /// </summary>
        /// <param name="VirtualEVSE">A virtual EVSE to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(VirtualEVSE VirtualEVSE)
        {

            if ((Object) VirtualEVSE == null)
                return false;

            return Id.Equals(VirtualEVSE.Id);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Id.ToString();

        #endregion

    }

}
