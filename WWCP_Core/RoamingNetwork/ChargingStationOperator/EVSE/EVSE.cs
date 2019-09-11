/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading.Tasks;
using System.Collections.Generic;

using Org.BouncyCastle.Bcpg.OpenPgp;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

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
        public const           UInt16    DefaultMaxEVSEStatusListSize    = 50;

        /// <summary>
        /// The default max size of the EVSE admin status history.
        /// </summary>
        public const           UInt16    DefaultMaxAdminStatusListSize   = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly TimeSpan  DefaultMaxReservationDuration   = TimeSpan.FromMinutes(15);

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

                return _Description.IsNeitherNullNorEmpty()
                           ? _Description
                           : ChargingStation?.Description;

            }

            set
            {

                if (value != _Description && value != ChargingStation?.Description)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _Description);

                    else
                        SetProperty(ref _Description, value);

                }

            }

        }

        public I18NString SetDescription(Languages Language, String Text)
            => _Description = I18NString.Create(Language, Text);

        public I18NString SetDescription(I18NString I18NText)
            => _Description = I18NText;

        public I18NString AddDescription(Languages Language, String Text)
            => _Description.Add(Language, Text);

        #endregion

        #region Brand

        private Brand _Brand;

        /// <summary>
        /// A (multi-language) brand name for this EVSE.
        /// </summary>
        [Optional]
        public Brand Brand
        {

            get
            {
                return _Brand ?? ChargingStation?.Brand;
            }

            set
            {

                if (value != _Brand && value != ChargingStation?.Brand)
                {

                    if (value == null)
                        DeleteProperty(ref _Brand);

                    else
                        SetProperty(ref _Brand, value);

                }

            }

        }

        #endregion

        #region DataLicense

        private ReactiveSet<DataLicense> _DataLicenses;

        /// <summary>
        /// The license of the EVSE data.
        /// </summary>
        [Mandatory]
        public ReactiveSet<DataLicense> DataLicenses
        {

            get
            {

                return _DataLicenses != null && _DataLicenses.Any()
                           ? _DataLicenses
                           : ChargingPool?.DataLicenses;

            }

            set
            {

                if (value != _DataLicenses && value != ChargingPool?.DataLicenses)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _DataLicenses);

                    else
                    {

                        if (_DataLicenses == null)
                            SetProperty(ref _DataLicenses, value);

                        else
                            SetProperty(ref _DataLicenses, _DataLicenses.Set(value));

                    }

                }

            }

        }

        #endregion


        #region ChargingModes

        private ChargingModes? _ChargingModes;

        /// <summary>
        /// Charging modes.
        /// </summary>
        [Mandatory]
        public ChargingModes? ChargingModes
        {

            get
            {
                return _ChargingModes;
            }

            set
            {

                if (_ChargingModes != value)
                {

                    if (_ChargingModes == null)
                        _ChargingModes = value;

                    SetProperty(ref _ChargingModes, value);

                }

            }

        }

        #endregion

        #region CurrentTypes

        private CurrentTypes? _CurrentTypes;

        /// <summary>
        /// The type of the current.
        /// </summary>
        [Mandatory]
        public CurrentTypes? CurrentTypes
        {

            get
            {
                return _CurrentTypes;
            }

            set
            {

                if (_CurrentTypes != value)
                    SetProperty(ref _CurrentTypes, value);

            }

        }

        #endregion

        #region AverageVoltage

        private Single? _AverageVoltage;

        /// <summary>
        /// The average voltage.
        /// </summary>
        [Mandatory]
        public Single? AverageVoltage
        {

            get
            {
                return _AverageVoltage;
            }

            set
            {

                if (value != null)
                {

                    if (!_AverageVoltage.HasValue)
                        _AverageVoltage = value;

                    else if (Math.Abs(_AverageVoltage.Value - value.Value) > EPSILON)
                        SetProperty(ref _AverageVoltage, value);

                }

                else
                    DeleteProperty(ref _AverageVoltage);

            }

        }

        #endregion


        #region MaxCurrent

        private Single? _MaxCurrent;

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public Single? MaxCurrent
        {

            get
            {
                return _MaxCurrent;
            }

            set
            {

                if (value != null)
                {

                    if (!_MaxCurrent.HasValue)
                        SetProperty(ref _MaxCurrent, value);

                    else if (Math.Abs(_MaxCurrent.Value - value.Value) > EPSILON)
                        SetProperty(ref _MaxCurrent, value);

                }

                else
                    DeleteProperty(ref _MaxCurrent);

            }

        }

        #endregion

        #region MaxCurrentRealTime

        private Timestamped<Single>? _MaxCurrentRealTime;

        /// <summary>
        /// The real-time maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public Timestamped<Single>? MaxCurrentRealTime
        {

            get
            {
                return _MaxCurrentRealTime;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxCurrentRealTime, value);

                else
                    DeleteProperty(ref _MaxCurrentRealTime);

            }

        }

        #endregion

        #region MaxCurrentPrognoses

        private IEnumerable<Timestamped<Single>> _MaxCurrentPrognoses;

        /// <summary>
        /// Prognoses on future values of the maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public IEnumerable<Timestamped<Single>> MaxCurrentPrognoses
        {

            get
            {
                return _MaxCurrentPrognoses;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxCurrentPrognoses, value);

                else
                    DeleteProperty(ref _MaxCurrentPrognoses);

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

                if (value != null)
                {

                    if (!_MaxPower.HasValue)
                        _MaxPower = value;

                    else if (Math.Abs(_MaxPower.Value - value.Value) > EPSILON)
                        SetProperty(ref _MaxPower, value);

                }

                else
                    DeleteProperty(ref _MaxPower);

            }

        }

        #endregion

        #region MaxPowerRealTime

        private Timestamped<Single>? _MaxPowerRealTime;

        /// <summary>
        /// The real-time maximum power [kWatt].
        /// </summary>
        [Mandatory]
        public Timestamped<Single>? MaxPowerRealTime
        {

            get
            {
                return _MaxPowerRealTime;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxPowerRealTime, value);

                else
                    DeleteProperty(ref _MaxPowerRealTime);

            }

        }

        #endregion

        #region MaxPowerPrognoses

        private IEnumerable<Timestamped<Single>> _MaxPowerPrognoses;

        /// <summary>
        /// Prognoses on future values of the maximum power [kWatt].
        /// </summary>
        [Mandatory]
        public IEnumerable<Timestamped<Single>> MaxPowerPrognoses
        {

            get
            {
                return _MaxPowerPrognoses;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxPowerPrognoses, value);

                else
                    DeleteProperty(ref _MaxPowerPrognoses);

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

                if (value != null)
                {

                    if (!_MaxCapacity.HasValue)
                        _MaxCapacity = value;

                    else if (Math.Abs(_MaxCapacity.Value - value.Value) > EPSILON)
                        SetProperty(ref _MaxCapacity, value);

                }

                else
                    DeleteProperty(ref _MaxCapacity);

            }

        }

        #endregion

        #region MaxCapacityRealTime

        private Timestamped<Single>? _MaxCapacityRealTime;

        /// <summary>
        /// The real-time maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public Timestamped<Single>? MaxCapacityRealTime
        {

            get
            {
                return _MaxCapacityRealTime;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxCapacityRealTime, value);

                else
                    DeleteProperty(ref _MaxCapacityRealTime);

            }

        }

        #endregion

        #region MaxCapacityPrognoses

        private IEnumerable<Timestamped<Single>> _MaxCapacityPrognoses;

        /// <summary>
        /// Prognoses on future values of the maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public IEnumerable<Timestamped<Single>> MaxCapacityPrognoses
        {

            get
            {
                return _MaxCapacityPrognoses;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxCapacityPrognoses, value);

                else
                    DeleteProperty(ref _MaxCapacityPrognoses);

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

        #region EnergyMix

        private EnergyMix _EnergyMix;

        /// <summary>
        /// The energy mix at the EVSE.
        /// </summary>
        [Optional]
        public EnergyMix EnergyMix
        {

            get
            {
                return _EnergyMix ?? ChargingStation?.EnergyMix;
            }

            set
            {

                if (value != _EnergyMix && value != ChargingStation?.EnergyMix)
                {

                    if (value == null)
                        DeleteProperty(ref _EnergyMix);

                    else
                        SetProperty(ref _EnergyMix, value);

                }

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


        #region MaxReservationDuration

        private TimeSpan _MaxReservationDuration;

        /// <summary>
        /// The maximum reservation time.
        /// </summary>
        [Optional]
        public TimeSpan MaxReservationDuration
        {

            get
            {
                return _MaxReservationDuration;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxReservationDuration, value);

                else
                    DeleteProperty(ref _MaxReservationDuration);

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

        #region AdminStatusSchedule(HistorySize = null)

        private readonly StatusSchedule<EVSEAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The EVSE admin status schedule.
        /// </summary>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<Timestamped<EVSEAdminStatusTypes>> AdminStatusSchedule(UInt64? HistorySize = null)
        {

            if (HistorySize.HasValue)
                return _AdminStatusSchedule.Take(HistorySize);

            return _AdminStatusSchedule;

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

        #region StatusSchedule(HistorySize = null)

        private readonly StatusSchedule<EVSEStatusTypes> _StatusSchedule;

        /// <summary>
        /// The EVSE status schedule.
        /// </summary>
        /// <param name="HistorySize">The size of the history.</param>
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

        #endregion

        #region Events

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate       OnStatusChanged;

        #endregion

        #endregion

        #region Links

        /// <summary>
        /// An optional remote EVSE.
        /// </summary>
        public IRemoteEVSE              RemoteEVSE        { get; }

        /// <summary>
        /// The charging station of this EVSE.
        /// </summary>
        public ChargingStation          ChargingStation   { get; }

        /// <summary>
        /// The charging pool of this EVSE.
        /// </summary>
        public ChargingPool             ChargingPool
            => ChargingStation?.ChargingPool;

        /// <summary>
        /// The operator of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator  Operator
            => ChargingStation?.ChargingPool?.Operator;

        /// <summary>
        /// The roaming network of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork           RoamingNetwork
            => ChargingStation?.ChargingPool?.Operator?.RoamingNetwork;

        #endregion

        #region Constructor(s)

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
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EVSE(EVSE_Id                              Id,
                    ChargingStation                      ChargingStation,
                    Action<EVSE>                         Configurator             = null,
                    RemoteEVSECreatorDelegate            RemoteEVSECreator        = null,
                    Timestamped<EVSEAdminStatusTypes>?   InitialAdminStatus       = null,
                    Timestamped<EVSEStatusTypes>?        InitialStatus            = null,
                    UInt16                               MaxStatusListSize        = DefaultMaxEVSEStatusListSize,
                    UInt16                               MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                    IReadOnlyDictionary<String, Object>  CustomData               = null)

            : base(Id,
                   CustomData)

        {

            #region Init data and properties

            this.ChargingStation        = ChargingStation;

            InitialAdminStatus          = InitialAdminStatus ?? new Timestamped<EVSEAdminStatusTypes>(EVSEAdminStatusTypes.Operational);
            InitialStatus               = InitialStatus      ?? new Timestamped<EVSEStatusTypes>     (EVSEStatusTypes.Available);

            this._Description           = new I18NString();
            this._SocketOutlets         = new ReactiveSet<SocketOutlet>();

            this._AdminStatusSchedule   = new StatusSchedule<EVSEAdminStatusTypes>(MaxAdminStatusListSize);
            this._AdminStatusSchedule.Insert(InitialAdminStatus.Value);

            this._StatusSchedule        = new StatusSchedule<EVSEStatusTypes>(MaxStatusListSize);
            this._StatusSchedule.     Insert(InitialStatus.Value);

            #endregion

            Configurator?.Invoke(this);

            #region Link events

            this.OnPropertyChanged += UpdateData;

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this._StatusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

            this.RemoteEVSE = RemoteEVSECreator?.Invoke(this);

            if (this.RemoteEVSE != null)
            {

                this.RemoteEVSE.OnAdminStatusChanged    += async (Timestamp, EventTrackingId, RemoteEVSE, OldStatus, NewStatus)  => AdminStatus                 = NewStatus;
                this.RemoteEVSE.OnStatusChanged         += async (Timestamp, EventTrackingId, RemoteEVSE, OldStatus, NewStatus)  => Status                      = NewStatus;

                this.RemoteEVSE.OnNewReservation        += (Timestamp, RemoteEVSE, Reservation)                                  => OnNewReservation.        Invoke(Timestamp, RemoteEVSE, Reservation);
                this.RemoteEVSE.OnReservationCanceled   += (Timestamp, RemoteEVSE, Reservation, Reason)                          => OnReservationCanceled.   Invoke(Timestamp, RemoteEVSE, Reservation, Reason);

                this.RemoteEVSE.OnNewChargingSession    += (Timestamp, RemoteEVSE, ChargingSession)                              => {
                    RoamingNetwork.SessionsStore.NewOrUpdate(ChargingSession, session => { session.EVSEId = Id; session.EVSE = this; });
                    //_ChargingSession       = ChargingSession;
                    //_ChargingSession.EVSE  = this;
                    OnNewChargingSession.Invoke(Timestamp, this, ChargingSession);
                };

                this.RemoteEVSE.OnNewChargeDetailRecord += (Timestamp, RemoteEVSE, ChargeDetailRecord)                           => OnNewChargeDetailRecord?.Invoke(Timestamp, RemoteEVSE, ChargeDetailRecord);

            }

        }

        #endregion


        #region UpdateWith(OtherEVSE)

        /// <summary>
        /// Update this EVSE with the data of the other EVSE.
        /// </summary>
        /// <param name="OtherEVSE">Another EVSE.</param>
        public EVSE UpdateWith(EVSE OtherEVSE)
        {

            Description          = OtherEVSE.Description;

            ChargingModes        = OtherEVSE.ChargingModes;
            AverageVoltage       = OtherEVSE.AverageVoltage;
            CurrentTypes         = OtherEVSE.CurrentTypes;
            MaxCurrent           = OtherEVSE.MaxCurrent;
            MaxPower             = OtherEVSE.MaxPower;
            MaxCapacity          = OtherEVSE.MaxCapacity;

            if (SocketOutlets == null && OtherEVSE.SocketOutlets != null)
                SocketOutlets = new ReactiveSet<SocketOutlet>(OtherEVSE.SocketOutlets);
            else if (SocketOutlets != null)
                SocketOutlets = OtherEVSE.SocketOutlets;

            EnergyMeterId        = OtherEVSE.EnergyMeterId;


            if (OtherEVSE.AdminStatus.Timestamp > AdminStatus.Timestamp)
                SetAdminStatus(OtherEVSE.AdminStatus);

            if (OtherEVSE.Status.Timestamp > Status.Timestamp)
                SetStatus(OtherEVSE.Status);

            return this;

        }

        #endregion


        public void AddChargingMode(ChargingModes ChargingMode)
        {

            if (!_ChargingModes.HasValue)
                _ChargingModes = ChargingMode;

            else
                _ChargingModes |= ChargingMode;

        }

        public void AddCurrentType(CurrentTypes CurrentType)
        {

            if (!_CurrentTypes.HasValue)
                _CurrentTypes = CurrentType;

            else
                _CurrentTypes |= CurrentType;

        }


        #region Data/(Admin-)Status

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
                                   ChangeMethods                                   ChangeMethod = ChangeMethods.Replace)
        {
            _AdminStatusSchedule.Insert(NewAdminStatusList, ChangeMethod);
        }

        #endregion


        #region (internal) UpdateData       (Timestamp, EventTrackingId, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data of the EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="Sender">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateData(DateTime          Timestamp,
                                       EventTracking_Id  EventTrackingId,
                                       Object            Sender,
                                       String            PropertyName,
                                       Object            OldValue,
                                       Object            NewValue)
        {

            var OnDataChangedLocal = OnDataChanged;
            if (OnDataChangedLocal != null)
                await OnDataChangedLocal(Timestamp,
                                         EventTrackingId,
                                         Sender as EVSE,
                                         PropertyName,
                                         OldValue,
                                         NewValue);

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

        #region Reservations

        #region Data

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> Reservations
            => RoamingNetwork.ReservationsStore.Where (reservationCollection => reservationCollection.EVSEId == Id).
                                                Select(reservationCollection => reservationCollection.LastOrDefault());

        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
        {

            if (RoamingNetwork.ReservationsStore.TryGet(ReservationId, out ChargingReservationCollection ReservationCollection))
            {
                Reservation = ReservationCollection.LastOrDefault();
                return true;
            }

            Reservation = null;
            return false;

        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a charging location is being reserved.
        /// </summary>
        public event OnReserveRequestDelegate             OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate            OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate             OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate   OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate  OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate        OnReservationCanceled;

        #endregion

        #region Reserve(                                           StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at this EVSE.
        /// </summary>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
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

            Reserve(DateTime?                         StartTime              = null,
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


                => Reserve(ChargingLocation.FromEVSEId(Id),
                           ChargingReservationLevel.EVSE,
                           StartTime,
                           Duration,
                           ReservationId,
                           ProviderId,
                           RemoteAuthentication,
                           ChargingProduct,
                           AuthTokens,
                           eMAIds,
                           PINs,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout);

        #endregion

        #region Reserve(ChargingLocation, ReservationLevel = EVSE, StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">A charging location.</param>
        /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
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

            Reserve(ChargingLocation                  ChargingLocation,
                    ChargingReservationLevel          ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                         ReservationStartTime   = null,
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

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ReservationResult result = null;

            #endregion

            #region Send OnReserveRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnReserveRequest?.Invoke(StartTime,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         ReservationId,
                                         ChargingLocation,
                                         ReservationStartTime,
                                         Duration,
                                         ProviderId,
                                         RemoteAuthentication,
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


            try
            {

                if (ChargingLocation.EVSEId.HasValue && ChargingLocation.EVSEId.Value != Id)
                    result = ReservationResult.UnknownLocation;

                else if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                         AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    if (RemoteEVSE != null)
                    {

                        result = await RemoteEVSE.
                                           Reserve(ChargingLocation,
                                                   ReservationLevel,
                                                   ReservationStartTime,
                                                   Duration,
                                                   ReservationId,
                                                   ProviderId,
                                                   RemoteAuthentication,
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

                            OnNewReservation?.Invoke(DateTime.UtcNow,
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

            }
            catch (Exception e)
            {
                result = ReservationResult.Error(e.Message);
            }


            #region Send OnReserveResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnReserveResponse?.Invoke(EndTime,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          ReservationId,
                                          ChargingLocation,
                                          ReservationStartTime,
                                          Duration,
                                          ProviderId,
                                          RemoteAuthentication,
                                          ChargingProduct,
                                          AuthTokens,
                                          eMAIds,
                                          PINs,
                                          result,
                                          EndTime - StartTime,
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

        #region CancelReservation(ReservationId, Reason, ...)

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

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ChargingReservation     canceledReservation  = null;
            CancelReservationResult result                = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnCancelReservationRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   ReservationId,
                                                   Reason,
                                                   RequestTimeout);


            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    if (RemoteEVSE != null)
                    {

                        result = await RemoteEVSE.
                                           CancelReservation(ReservationId,
                                                             Reason,

                                                             Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             RequestTimeout);

                    }

                    else
                        result = CancelReservationResult.Offline(ReservationId,
                                                                 Reason);

                }

                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = CancelReservationResult.OutOfService(ReservationId,
                                                                          Reason);
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = CancelReservationResult.Error(ReservationId,
                                                       Reason,
                                                       e.Message);
            }


            #region Send OnCancelReservationResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnCancelReservationResponse?.Invoke(EndTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ReservationId,
                                                    canceledReservation,
                                                    Reason,
                                                    result,
                                                    EndTime - StartTime,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and SendSession/-CDR

        #region Data

        /// <summary>
        /// The current charging session, if available.
        /// </summary>
        [InternalUseOnly]
        public ChargingSession ChargingSession { get; private set; }


        public IEnumerable<ChargingSession> ChargingSessions
            => ChargingSession != null
                   ? new ChargingSession[] { ChargingSession }
                   : new ChargingSession[0];

        #region TryGetChargingSessionById(SessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession chargingSession)
        {

            if (SessionId == ChargingSession.Id)
            {
                chargingSession = ChargingSession;
                return true;
            }

            chargingSession = null;
            return false;

        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate     OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate    OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate     OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate     OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate  OnNewChargeDetailRecord;

        #endregion

        #region RemoteStart(                  ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

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
        public Task<RemoteStartResult>

            RemoteStart(ChargingProduct          ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        eMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)


                => RemoteStart(ChargingLocation.FromEVSEId(Id),
                               ChargingProduct,
                               ReservationId,
                               SessionId,
                               ProviderId,
                               RemoteAuthentication,

                               Timestamp,
                               CancellationToken,
                               EventTrackingId,
                               RequestTimeout);

        #endregion

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
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
        public async Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct          ChargingProduct        = null,
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

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStartResult result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnRemoteStartRequest?.Invoke(StartTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             ChargingLocation,
                                             ChargingProduct,
                                             ReservationId,
                                             SessionId,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    #region Try RemoteEVSE...

                    if (RemoteEVSE != null)
                    {

                        result = await RemoteEVSE.
                                           RemoteStart(ChargingLocation,
                                                       ChargingProduct,
                                                       ReservationId,
                                                       SessionId,
                                                       ProviderId,
                                                       RemoteAuthentication,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout);

                        #region In case of success...

                        if (result?.Result == RemoteStartResultType.Success)
                        {

                            // The session can be delivered within the response
                            // or via an explicit message afterwards!
                            if (result.Session != null)
                            {
                                ChargingSession = result.Session;
                                result.Session.EVSE = this;
                            }

                        }

                        #endregion

                    }

                    #endregion

                    #region Try RemoteChargingStation

                    else if (ChargingStation.RemoteChargingStation != null)
                    {

                        result = await ChargingStation.RemoteChargingStation.
                                           RemoteStart(ChargingLocation,
                                                       ChargingProduct,
                                                       ReservationId,
                                                       SessionId,
                                                       ProviderId,
                                                       RemoteAuthentication,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout);

                    }

                    #endregion

                    #region Try RemoteChargingPool

                    else if (ChargingStation.ChargingPool.RemoteChargingPool != null)
                    {

                        result = await ChargingStation.ChargingPool.RemoteChargingPool.
                                           RemoteStart(ChargingLocation,
                                                       ChargingProduct,
                                                       ReservationId,
                                                       SessionId,
                                                       ProviderId,
                                                       RemoteAuthentication,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout);

                    }

                    #endregion

                    #region Try RemoteChargingStationOperator

                    else if (ChargingStation.ChargingPool.Operator.RemoteChargingStationOperator != null)
                    {

                        result = await ChargingStation.ChargingPool.Operator.RemoteChargingStationOperator.
                                           RemoteStart(ChargingLocation,
                                                       ChargingProduct,
                                                       ReservationId,
                                                       SessionId,
                                                       ProviderId,
                                                       RemoteAuthentication,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout);

                    }

                    #endregion

                    #region ...or send 'OFFLINE'...

                    else
                        result = RemoteStartResult.Offline;

                    #endregion


                    if (result.Result == RemoteStartResultType.Success &&
                        result.Session != null)
                    {
                        result.Session.EVSE = this;
                    }

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStartResult.OutOfService;
                            break;

                    }

                }


            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }


            #region Send OnRemoteStartResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnRemoteStartResponse?.Invoke(EndTime,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              RoamingNetwork.Id,
                                              ChargingLocation,
                                              ChargingProduct,
                                              ReservationId,
                                              SessionId,
                                              ProviderId,
                                              RemoteAuthentication,
                                              RequestTimeout,
                                              result,
                                              EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop (SessionId, ReservationHandling = null, ProviderId = null, RemoteAuthentication = null, ...)

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
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       eMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)
        {

            #region Initial checks

            if (SessionId == null)
                SessionId = ChargingSession_Id.New;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStopResult result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnRemoteStopRequest?.Invoke(StartTime,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            SessionId,
                                            ReservationHandling,
                                            ProviderId,
                                            RemoteAuthentication,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    if (SessionId == ChargingSession?.Id)
                    {

                        #region Try RemoteEVSE...

                        if (RemoteEVSE != null)
                        {

                            result = await RemoteEVSE.
                                               RemoteStop(SessionId,
                                                          ReservationHandling,
                                                          ProviderId,
                                                          RemoteAuthentication,

                                                          Timestamp,
                                                          CancellationToken,
                                                          EventTrackingId,
                                                          RequestTimeout);

                            #region In case of success...

                            if (result?.Result == RemoteStopResultType.Success)
                            {
                                ChargingSession = null;
                            }

                            #endregion

                        }

                        #endregion

                        #region Try RemoteChargingStation

                        else if (ChargingStation.RemoteChargingStation != null)
                        {

                            result = await ChargingStation.RemoteChargingStation.
                                               RemoteStop(SessionId,
                                                          ReservationHandling,
                                                          ProviderId,
                                                          RemoteAuthentication,

                                                          Timestamp,
                                                          CancellationToken,
                                                          EventTrackingId,
                                                          RequestTimeout);

                        }

                        #endregion

                        #region Try RemoteChargingPool

                        else if (ChargingStation.ChargingPool.RemoteChargingPool != null)
                        {

                            result = await ChargingStation.ChargingPool.RemoteChargingPool.
                                               RemoteStop(SessionId,
                                                          ReservationHandling,
                                                          ProviderId,
                                                          RemoteAuthentication,

                                                          Timestamp,
                                                          CancellationToken,
                                                          EventTrackingId,
                                                          RequestTimeout);

                        }

                        #endregion

                        #region Try RemoteChargingStationOperator

                        else if (ChargingStation.ChargingPool.Operator.RemoteChargingStationOperator != null)
                        {

                            result = await ChargingStation.ChargingPool.Operator.RemoteChargingStationOperator.
                                               RemoteStop(SessionId,
                                                          ReservationHandling,
                                                          ProviderId,
                                                          RemoteAuthentication,

                                                          Timestamp,
                                                          CancellationToken,
                                                          EventTrackingId,
                                                          RequestTimeout);

                        }

                        #endregion

                        #region ...or send 'OFFLINE'...

                        else
                            result = RemoteStopResult.Offline(SessionId);

                        #endregion

                    }
                    else
                        result = RemoteStopResult.InvalidSessionId(SessionId);

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStopResult.OutOfService(SessionId);
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(SessionId,
                                                e.Message);
            }


            #region Send OnRemoteStopResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnRemoteStopResponse?.Invoke(EndTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             SessionId,
                                             ReservationHandling,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout,
                                             result,
                                             EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (internal) SendNewChargingSession   (Timestamp, Sender, Session)

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  Session)
        {

            if (Session != null)
            {

                if (Session.EVSE == null)
                {
                    Session.EVSE    = this;
                    Session.EVSEId  = Id;
                }

                OnNewChargingSession?.Invoke(Timestamp, Sender, Session);

            }

        }

        #endregion

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            if (ChargeDetailRecord != null)
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


        #region Operator overloading

        #region Operator == (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSE EVSE1, EVSE EVSE2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSE1, EVSE2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSE1 == null) || ((Object) EVSE2 == null))
                return false;

            return EVSE1.Equals(EVSE2);

        }

        #endregion

        #region Operator != (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSE EVSE1, EVSE EVSE2)
            => !(EVSE1 == EVSE2);

        #endregion

        #region Operator <  (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSE EVSE1, EVSE EVSE2)
        {

            if ((Object) EVSE1 == null)
                throw new ArgumentNullException("The given EVSE1 must not be null!");

            return EVSE1.CompareTo(EVSE2) < 0;

        }

        #endregion

        #region Operator <= (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSE EVSE1, EVSE EVSE2)
            => !(EVSE1 > EVSE2);

        #endregion

        #region Operator >  (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSE EVSE1, EVSE EVSE2)
        {

            if ((Object) EVSE1 == null)
                throw new ArgumentNullException("The given EVSE1 must not be null!");

            return EVSE1.CompareTo(EVSE2) > 0;

        }

        #endregion

        #region Operator >= (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSE EVSE1, EVSE EVSE2)
            => !(EVSE1 < EVSE2);

        #endregion

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
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

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

            var EVSE = Object as EVSE;
            if ((Object) EVSE == null)
                return false;

            return Equals(EVSE);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Id.ToString();

        #endregion


    }

}
