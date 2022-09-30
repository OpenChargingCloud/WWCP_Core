﻿/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using cloud.charging.open.protocols.WWCP.Net.IO.JSON;

#endregion

namespace cloud.charging.open.protocols.WWCP
{




    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// This is meant to be one electrical circuit which can charge a electric vehicle
    /// independently. Thus there could be multiple interdependent power sockets.
    /// </summary>
    public class EVSE : AEMobilityEntity<EVSE_Id,
                                         EVSEAdminStatusTypes,
                                         EVSEStatusTypes>,
                        IEquatable<EVSE>, IComparable<EVSE>,
                        IEVSE
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const            String    JSONLDContext                            = "https://open.charging.cloud/contexts/wwcp+json/EVSE";


        private readonly        Decimal   EPSILON                                  = 0.01m;

        /// <summary>
        /// The default max size of the EVSE admin status schedule/history.
        /// </summary>
        public const            UInt16    DefaultMaxEVSEAdminStatusScheduleSize    = 50;

        /// <summary>
        /// The default max size of the EVSE status schedule/history.
        /// </summary>
        public const            UInt16    DefaultMaxEVSEStatusScheduleSize         = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly  TimeSpan  DefaultMaxReservationDuration            = TimeSpan.FromMinutes(15);

        #endregion

        #region Properties

        /// <summary>
        /// All brands registered for this EVSE.
        /// </summary>
        [Optional, SlowData]
        public EntityHashSet<EVSE, Brand_Id, Brand>     Brands                  { get; }

        /// <summary>
        /// The license of the EVSE data.
        /// </summary>
        [Mandatory, SlowData]
        public ReactiveSet<DataLicense>                 DataLicenses            { get; }

        /// <summary>
        /// The charging modes.
        /// </summary>
        [Mandatory, SlowData]
        public ReactiveSet<ChargingModes>               ChargingModes           { get; }

        /// <summary>
        /// The power socket outlets.
        /// </summary>
        [Mandatory, SlowData]
        public ReactiveSet<SocketOutlet>                SocketOutlets           { get; }


        #region CurrentType

        private CurrentTypes? currentType;

        /// <summary>
        /// The type of the current.
        /// </summary>
        [Mandatory, SlowData]
        public CurrentTypes? CurrentType
        {

            get
            {
                return currentType;
            }

            set
            {

                if (currentType != value)
                    SetProperty(ref currentType,
                                value,
                                EventTracking_Id.New);

            }

        }

        #endregion

        #region AverageVoltage

        private Decimal? averageVoltage;

        /// <summary>
        /// The average voltage.
        /// </summary>
        [Optional, SlowData]
        public Decimal? AverageVoltage
        {

            get
            {
                return averageVoltage;
            }

            set
            {

                if (value is not null)
                {

                    if (!averageVoltage.HasValue)
                        averageVoltage = value;

                    else if (Math.Abs(averageVoltage.Value - value.Value) > EPSILON)
                        SetProperty(ref averageVoltage,
                                    value,
                                    EventTracking_Id.New);

                }
                else
                    DeleteProperty(ref averageVoltage);

            }

        }

        #endregion


        #region MaxCurrent

        private Decimal? maxCurrent;

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        [Mandatory, SlowData]
        public Decimal? MaxCurrent
        {

            get
            {
                return maxCurrent;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxCurrent.HasValue)
                        SetProperty(ref maxCurrent,
                                    value,
                                    EventTracking_Id.New);

                    else if (Math.Abs(maxCurrent.Value - value.Value) > EPSILON)
                        SetProperty(ref maxCurrent,
                                    value,
                                    EventTracking_Id.New);

                }
                else
                    DeleteProperty(ref maxCurrent);

            }

        }

        #endregion

        #region MaxCurrentRealTime

        private Timestamped<Decimal>? maxCurrentRealTime;

        /// <summary>
        /// The real-time maximum current [Ampere].
        /// </summary>
        [Optional, FastData]
        public Timestamped<Decimal>? MaxCurrentRealTime
        {

            get
            {
                return maxCurrentRealTime;
            }

            set
            {

                if (value is not null)
                    SetProperty(ref maxCurrentRealTime,
                                value,
                                EventTracking_Id.New);

                else
                    DeleteProperty(ref maxCurrentRealTime);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the maximum current [Ampere].
        /// </summary>
        [Optional, FastData]
        public ReactiveSet<Timestamped<Decimal>>        MaxCurrentPrognoses     { get; }


        #region MaxPower

        private Decimal? maxPower;

        /// <summary>
        /// The maximum power [kWatt].
        /// </summary>
        [Optional, SlowData]
        public Decimal? MaxPower
        {

            get
            {
                return maxPower;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxPower.HasValue)
                        SetProperty(ref maxPower,
                                    value,
                                    EventTracking_Id.New);

                    else if (Math.Abs(maxPower.Value - value.Value) > EPSILON)
                        SetProperty(ref maxPower,
                                    value,
                                    EventTracking_Id.New);

                }
                else
                    DeleteProperty(ref maxPower);

            }

        }

        #endregion

        #region MaxPowerRealTime

        private Timestamped<Decimal>? maxPowerRealTime;

        /// <summary>
        /// The real-time maximum power [kWatt].
        /// </summary>
        [Optional, FastData]
        public Timestamped<Decimal>? MaxPowerRealTime
        {

            get
            {
                return maxPowerRealTime;
            }

            set
            {

                if (value is not null)
                    SetProperty(ref maxPowerRealTime,
                                value,
                                EventTracking_Id.New);

                else
                    DeleteProperty(ref maxPowerRealTime);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the maximum power [kWatt].
        /// </summary>
        [Optional, FastData]
        public ReactiveSet<Timestamped<Decimal>>        MaxPowerPrognoses       { get; }


        #region MaxCapacity

        private Decimal? maxCapacity;

        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public Decimal? MaxCapacity
        {

            get
            {
                return maxCapacity;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxCapacity.HasValue)
                        SetProperty(ref maxCapacity,
                                    value,
                                    EventTracking_Id.New);

                    else if (Math.Abs(maxCapacity.Value - value.Value) > EPSILON)
                        SetProperty(ref maxCapacity,
                                    value,
                                    EventTracking_Id.New);

                }
                else
                    DeleteProperty(ref maxCapacity);

            }

        }

        #endregion

        #region MaxCapacityRealTime

        private Timestamped<Decimal>? maxCapacityRealTime;

        /// <summary>
        /// The real-time maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public Timestamped<Decimal>? MaxCapacityRealTime
        {

            get
            {
                return maxCapacityRealTime;
            }

            set
            {

                if (value is not null)
                    SetProperty(ref maxCapacityRealTime,
                                value,
                                EventTracking_Id.New);

                else
                    DeleteProperty(ref maxCapacityRealTime);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public ReactiveSet<Timestamped<Decimal>>        MaxCapacityPrognoses    { get; }


        #region EnergyMix

        private EnergyMix? energyMix;

        /// <summary>
        /// The energy mix.
        /// </summary>
        [Optional, SlowData]
        public EnergyMix? EnergyMix
        {

            get
            {
                return energyMix ?? ChargingStation?.EnergyMix;
            }

            set
            {

                if (value != energyMix && value != ChargingStation?.EnergyMix)
                {

                    if (value == null)
                        DeleteProperty(ref energyMix);

                    else
                        SetProperty(ref energyMix, value);

                }

            }

        }

        #endregion

        #region EnergyMixRealTime

        private Timestamped<EnergyMix>? energyMixRealTime;

        /// <summary>
        /// The current energy mix.
        /// </summary>
        [Mandatory, FastData]
        public Timestamped<EnergyMix>? EnergyMixRealTime
        {

            get
            {
                return energyMixRealTime;
            }

            set
            {

                if (value is not null)
                    SetProperty(ref energyMixRealTime,
                                value,
                                EventTracking_Id.New);

                else
                    DeleteProperty(ref energyMixRealTime);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the energy mix.
        /// </summary>
        [Mandatory, FastData]
        public ReactiveSet<Timestamped<EnergyMix>>      EnergyMixPrognoses      { get; }


        #region MaxReservationDuration

        private TimeSpan maxReservationDuration;

        /// <summary>
        /// The maximum reservation time at this EVSE.
        /// </summary>
        [Optional, SlowData]
        public TimeSpan MaxReservationDuration
        {

            get
            {
                return maxReservationDuration;
            }

            set
            {
                SetProperty(ref maxReservationDuration,
                            value,
                            EventTracking_Id.New);
            }

        }

        #endregion

        #region IsFreeOfCharge

        private Boolean isFreeOfCharge;

        /// <summary>
        /// Charging at this EVSE is ALWAYS free of charge.
        /// </summary>
        [Optional, SlowData]
        public Boolean IsFreeOfCharge
        {

            get
            {
                return isFreeOfCharge;
            }

            set
            {
                SetProperty(ref isFreeOfCharge,
                            value,
                            EventTracking_Id.New);
            }

        }

        #endregion


        #region EnergyMeter

        private EnergyMeter? energyMeter;

        /// <summary>
        /// The smart energy meter attached to this EVSE.
        /// </summary>
        [Optional, SlowData]
        public EnergyMeter? EnergyMeter
        {

            get
            {
                return energyMeter;
            }

            set
            {

                if (value is not null)
                    SetProperty(ref energyMeter, value);

                else
                    DeleteProperty(ref energyMeter);

            }

        }

        #endregion




        /// <summary>
        /// The current charging session, if available.
        /// </summary>
        [InternalUseOnly]
        public ChargingSession? ChargingSession { get; internal set; }



        public DateTime? LastStatusUpdate { get; set; }

        #endregion

        #region Events

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate? OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate? OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate? OnStatusChanged;

        #endregion

        #region Reservations

        /// <summary>
        /// An event fired whenever a charging location is being reserved.
        /// </summary>
        public event OnReserveRequestDelegate? OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate? OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate? OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate? OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate? OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate? OnReservationCanceled;

        #endregion

        #region RemoteStart/-Stop

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate? OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate? OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate? OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate? OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate? OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate? OnNewChargeDetailRecord;

        #endregion

        #endregion

        #region Links

        /// <summary>
        /// An optional remote EVSE.
        /// </summary>
        public IRemoteEVSE? RemoteEVSE { get; }

        /// <summary>
        /// The charging station of this EVSE.
        /// </summary>
        public ChargingStation? ChargingStation { get; }

        /// <summary>
        /// The charging pool of this EVSE.
        /// </summary>
        public ChargingPool? ChargingPool
            => ChargingStation?.ChargingPool;

        /// <summary>
        /// The operator of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator? Operator
            => ChargingStation?.ChargingPool?.Operator;

        /// <summary>
        /// The roaming network of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork? RoamingNetwork
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
        /// <param name="MaxAdminStatusScheduleSize">An optional max length of the admin staus schedule.</param>
        /// <param name="MaxStatusScheduleSize">An optional max length of the staus schedule.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="InternalData">An optional dictionary of internal data.</param>
        public EVSE(EVSE_Id                             Id,
                    ChargingStation                     ChargingStation,
                    I18NString?                         Name                         = null,
                    I18NString?                         Description                  = null,
                    Action<EVSE>?                       Configurator                 = null,
                    RemoteEVSECreatorDelegate?          RemoteEVSECreator            = null,
                    Timestamped<EVSEAdminStatusTypes>?  InitialAdminStatus           = null,
                    Timestamped<EVSEStatusTypes>?       InitialStatus                = null,
                    UInt16?                             MaxAdminStatusScheduleSize   = null,
                    UInt16?                             MaxStatusScheduleSize        = null,

                    String?                             DataSource                   = null,
                    DateTime?                           LastChange                   = null,

                    JObject?                            CustomData                   = null,
                    UserDefinedDictionary?              InternalData                 = null)

            : base(Id,
                   Name,
                   Description,
                   InitialAdminStatus         ?? EVSEAdminStatusTypes.Operational,
                   InitialStatus              ?? EVSEStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxEVSEAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxEVSEStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Init data and properties

            this.ChargingStation = ChargingStation;

            this.Brands                             = new EntityHashSet<EVSE, Brand_Id, Brand>(this);
            //this.Brands.OnSetChanged               += (timestamp, sender, newItems, oldItems) => {

            //    PropertyChanged("DataLicenses",
            //                    oldItems,
            //                    newItems);

            //};

            this.DataLicenses                       = new ReactiveSet<DataLicense>();
            this.DataLicenses.OnSetChanged         += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("DataLicenses",
                                oldItems,
                                newItems);

            };

            this.ChargingModes                      = new ReactiveSet<ChargingModes>();
            this.ChargingModes.OnSetChanged        += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("ChargingModes",
                                oldItems,
                                newItems);

            };

            this.MaxCurrentPrognoses                = new ReactiveSet<Timestamped<Decimal>>();
            this.MaxCurrentPrognoses.OnSetChanged  += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("MaxCurrentPrognoses",
                                oldItems,
                                newItems);

            };

            this.MaxPowerPrognoses                  = new ReactiveSet<Timestamped<Decimal>>();
            this.MaxPowerPrognoses.OnSetChanged    += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("MaxPowerPrognoses",
                                oldItems,
                                newItems);

            };

            this.MaxCapacityPrognoses               = new ReactiveSet<Timestamped<Decimal>>();
            this.MaxCapacityPrognoses.OnSetChanged += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("MaxCapacityPrognoses",
                                oldItems,
                                newItems);

            };

            this.EnergyMixPrognoses                 = new ReactiveSet<Timestamped<EnergyMix>>();
            this.EnergyMixPrognoses.OnSetChanged   += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("EnergyMixPrognoses",
                                oldItems,
                                newItems);

            };

            this.SocketOutlets                      = new ReactiveSet<SocketOutlet>();
            this.SocketOutlets.OnSetChanged        += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("SocketOutlets",
                                oldItems,
                                newItems);

            };

            #endregion

            Configurator?.Invoke(this);

            #region Link events

            this.OnPropertyChanged += UpdateData;

            this.adminStatusSchedule.OnStatusChanged    += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                            => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this.statusSchedule.OnStatusChanged         += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                            => UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

            this.RemoteEVSE = RemoteEVSECreator?.Invoke(this);

            if (this.RemoteEVSE is not null)
            {

                this.RemoteEVSE.OnAdminStatusChanged    += (Timestamp, EventTrackingId, RemoteEVSE, OldStatus, NewStatus) => { AdminStatus = NewStatus; return Task.CompletedTask; };
                this.RemoteEVSE.OnStatusChanged         += (Timestamp, EventTrackingId, RemoteEVSE, OldStatus, NewStatus) => { Status = NewStatus; return Task.CompletedTask; };

                this.RemoteEVSE.OnNewReservation        += (Timestamp, RemoteEVSE, Reservation) => OnNewReservation.Invoke(Timestamp, RemoteEVSE, Reservation);
                this.RemoteEVSE.OnReservationCanceled   += (Timestamp, RemoteEVSE, Reservation, Reason) => OnReservationCanceled.Invoke(Timestamp, RemoteEVSE, Reservation, Reason);

                this.RemoteEVSE.OnNewChargingSession    += (Timestamp, RemoteEVSE, ChargingSession) =>
                {
                    RoamingNetwork.SessionsStore.NewOrUpdate(ChargingSession, session => { session.EVSEId = Id; session.EVSE = this; });
                    //_ChargingSession       = ChargingSession;
                    //_ChargingSession.EVSE  = this;
                    OnNewChargingSession.Invoke(Timestamp, this, ChargingSession);
                };

                this.RemoteEVSE.OnNewChargeDetailRecord += (Timestamp, RemoteEVSE, ChargeDetailRecord) => OnNewChargeDetailRecord?.Invoke(Timestamp, RemoteEVSE, ChargeDetailRecord);

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

            Name.Add(OtherEVSE.Name);
            Description.Add(OtherEVSE.Description);

            Brands.Clear();
            Brands.TryAdd(OtherEVSE.Brands);

            ChargingModes.Clear();
            ChargingModes.Add(OtherEVSE.ChargingModes);

            SocketOutlets.Clear();
            SocketOutlets.Add(OtherEVSE.SocketOutlets);

            AverageVoltage = OtherEVSE.AverageVoltage;
            CurrentType = OtherEVSE.CurrentType;
            MaxCurrent = OtherEVSE.MaxCurrent;
            MaxPower = OtherEVSE.MaxPower;
            MaxCapacity = OtherEVSE.MaxCapacity;
            EnergyMeter = OtherEVSE.EnergyMeter;

            if (OtherEVSE.AdminStatus.Timestamp > AdminStatus.Timestamp)
                SetAdminStatus(OtherEVSE.AdminStatus);

            if (OtherEVSE.Status.Timestamp > Status.Timestamp)
                SetStatus(OtherEVSE.Status);

            return this;

        }

        #endregion


        //public void AddChargingMode(ChargingModes ChargingMode)
        //{

        //    if (!_ChargingModes.HasValue)
        //        _ChargingModes = ChargingMode;

        //    else
        //        _ChargingModes |= ChargingMode;

        //}

        public void AddCurrentType(CurrentTypes CurrentType)
        {

            if (!currentType.HasValue)
                currentType = CurrentType;

            else
                currentType |= CurrentType;

        }


        #region Data/(Admin-)Status

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
        internal async Task UpdateData(DateTime Timestamp,
                                       EventTracking_Id EventTrackingId,
                                       Object Sender,
                                       String PropertyName,
                                       Object OldValue,
                                       Object NewValue)
        {

            var OnDataChangedLocal = OnDataChanged;
            if (OnDataChangedLocal is not null)
                await OnDataChangedLocal(Timestamp,
                                         EventTrackingId ?? EventTracking_Id.New,
                                         this,
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
        internal async Task UpdateAdminStatus(DateTime Timestamp,
                                              EventTracking_Id EventTrackingId,
                                              Timestamped<EVSEAdminStatusTypes> OldStatus,
                                              Timestamped<EVSEAdminStatusTypes> NewStatus)
        {

            var OnAdminStatusChangedLocal = OnAdminStatusChanged;
            if (OnAdminStatusChangedLocal is not null)
                await OnAdminStatusChangedLocal(Timestamp,
                                                EventTrackingId ?? EventTracking_Id.New,
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
        internal async Task UpdateStatus(DateTime Timestamp,
                                         EventTracking_Id EventTrackingId,
                                         Timestamped<EVSEStatusTypes> OldStatus,
                                         Timestamped<EVSEStatusTypes> NewStatus)
        {

            var OnStatusChangedLocal = OnStatusChanged;
            if (OnStatusChangedLocal is not null)
                await OnStatusChangedLocal(Timestamp,
                                           EventTrackingId ?? EventTracking_Id.New,
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

            => RoamingNetwork?.ReservationsStore.Where(reservationCollection => reservationCollection.EVSEId == Id).
                                                 Select(reservationCollection => reservationCollection.LastOrDefault()).
                                                 Where(result => result is not null).
                                                 Cast<ChargingReservation>()

                   ?? Array.Empty<ChargingReservation>();

        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation? ChargingReservation)
        {

            if (RoamingNetwork is not null &&
                RoamingNetwork.ReservationsStore.TryGet(ReservationId, out ChargingReservationCollection ReservationCollection))
            {
                ChargingReservation = ReservationCollection.LastOrDefault();
                return true;
            }

            ChargingReservation = null;
            return false;

        }

        #endregion

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

            Reserve(DateTime? StartTime = null,
                    TimeSpan? Duration = null,
                    ChargingReservation_Id? ReservationId = null,
                    eMobilityProvider_Id? ProviderId = null,
                    RemoteAuthentication? RemoteAuthentication = null,
                    ChargingProduct? ChargingProduct = null,
                    IEnumerable<Auth_Token>? AuthTokens = null,
                    IEnumerable<eMobilityAccount_Id>? eMAIds = null,
                    IEnumerable<UInt32>? PINs = null,

                    DateTime? Timestamp = null,
                    CancellationToken? CancellationToken = null,
                    EventTracking_Id? EventTrackingId = null,
                    TimeSpan? RequestTimeout = null)


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

            Reserve(ChargingLocation ChargingLocation,
                    ChargingReservationLevel ReservationLevel = ChargingReservationLevel.EVSE,
                    DateTime? ReservationStartTime = null,
                    TimeSpan? Duration = null,
                    ChargingReservation_Id? ReservationId = null,
                    eMobilityProvider_Id? ProviderId = null,
                    RemoteAuthentication? RemoteAuthentication = null,
                    ChargingProduct? ChargingProduct = null,
                    IEnumerable<Auth_Token>? AuthTokens = null,
                    IEnumerable<eMobilityAccount_Id>? eMAIds = null,
                    IEnumerable<UInt32>? PINs = null,

                    DateTime? Timestamp = null,
                    CancellationToken? CancellationToken = null,
                    EventTracking_Id? EventTrackingId = null,
                    TimeSpan? RequestTimeout = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;


            ReservationResult? result = null;

            #endregion

            #region Send OnReserveRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveRequest?.Invoke(startTime,
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
                DebugX.LogException(e, nameof(EVSE) + "." + nameof(OnReserveRequest));
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

                            OnNewReservation?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
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

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveResponse?.Invoke(endTime,
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
                                          endTime - startTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EVSE) + "." + nameof(OnReserveResponse));
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

            CancelReservation(ChargingReservation_Id ReservationId,
                              ChargingReservationCancellationReason Reason,

                              DateTime? Timestamp = null,
                              CancellationToken? CancellationToken = null,
                              EventTracking_Id? EventTrackingId = null,
                              TimeSpan? RequestTimeout = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;


            ChargingReservation? canceledReservation = null;
            CancelReservationResult? result = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationRequest?.Invoke(startTime,
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
                DebugX.LogException(e, nameof(EVSE) + "." + nameof(OnCancelReservationRequest));
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

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationResponse?.Invoke(endTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ReservationId,
                                                    canceledReservation,
                                                    Reason,
                                                    result,
                                                    endTime - startTime,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EVSE) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and SendSession/-CDR

        #region Data

        public IEnumerable<ChargingSession> ChargingSessions
            => ChargingSession is not null
                   ? new ChargingSession[] { ChargingSession }
                   : Array.Empty<ChargingSession>();

        #region TryGetChargingSessionById(SessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession? chargingSession)
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

            RemoteStart(ChargingProduct? ChargingProduct = null,
                        ChargingReservation_Id? ReservationId = null,
                        ChargingSession_Id? SessionId = null,
                        eMobilityProvider_Id? ProviderId = null,
                        RemoteAuthentication? RemoteAuthentication = null,

                        DateTime? Timestamp = null,
                        CancellationToken? CancellationToken = null,
                        EventTracking_Id? EventTrackingId = null,
                        TimeSpan? RequestTimeout = null)


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

            RemoteStart(ChargingLocation ChargingLocation,
                        ChargingProduct? ChargingProduct = null,
                        ChargingReservation_Id? ReservationId = null,
                        ChargingSession_Id? SessionId = null,
                        eMobilityProvider_Id? ProviderId = null,
                        RemoteAuthentication? RemoteAuthentication = null,

                        DateTime? Timestamp = null,
                        CancellationToken? CancellationToken = null,
                        EventTracking_Id? EventTrackingId = null,
                        TimeSpan? RequestTimeout = null)
        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStartResult result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                                             null,
                                             null,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EVSE) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    #region Try Remote EVSE

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

                    }

                    #endregion

                    #region Try Remote Charging Station

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

                    #region Try Remote Charging Pool

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

                    #region Try Remote Charging Station Operator

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

                    #region Try EMP Roaming Provider

                    else if (ChargingStation.ChargingPool.Operator.EMPRoamingProvider != null)
                    {

                        result = await ChargingStation.ChargingPool.Operator.EMPRoamingProvider.
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
                        result = RemoteStartResult.Offline();

                    #endregion


                    if (result?.Session != null &&
                       (result?.Result == RemoteStartResultTypes.Success ||
                        result?.Result == RemoteStartResultTypes.AsyncOperation))
                    {
                        ChargingSession = result.Session;
                        result.Session.EVSE = this;
                    }

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStartResult.OutOfService();
                            break;

                    }

                }


            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }


            #region Send OnRemoteStartResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                                              null,
                                              null,
                                              ProviderId,
                                              RemoteAuthentication,
                                              RequestTimeout,
                                              result,
                                              EndTime - StartTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EVSE) + "." + nameof(OnRemoteStartResponse));
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

            RemoteStop(ChargingSession_Id SessionId,
                       ReservationHandling? ReservationHandling = null,
                       eMobilityProvider_Id? ProviderId = null,
                       RemoteAuthentication? RemoteAuthentication = null,

                       DateTime? Timestamp = null,
                       CancellationToken? CancellationToken = null,
                       EventTracking_Id? EventTrackingId = null,
                       TimeSpan? RequestTimeout = null)
        {

            #region Initial checks

            if (SessionId == null)
                SessionId = ChargingSession_Id.NewRandom;

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStopResult result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopRequest?.Invoke(StartTime,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            SessionId,
                                            ReservationHandling,
                                            null,
                                            null,
                                            ProviderId,
                                            RemoteAuthentication,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EVSE) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    if (SessionId == ChargingSession?.Id)
                    {

                        #region Try Remote EVSE

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

                        }

                        #endregion

                        #region Try Remote Charging Station

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

                        #region Try Remote Charging Pool

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

                        #region Try Remote Charging Station Operator

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

                        #region Try EMP Roaming Provider

                        else if (ChargingStation.ChargingPool.Operator.EMPRoamingProvider != null)
                        {

                            result = await ChargingStation.ChargingPool.Operator.EMPRoamingProvider.
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


                        #region In case of success...

                        if (result?.Result == RemoteStopResultTypes.Success)
                        {
                            ChargingSession = null;
                        }

                        #endregion

                    }
                    else
                    {
                        DebugX.Log("Invalid charging session at EVSE '" + Id + "': " + SessionId + " != " + ChargingSession?.Id);
                        result = RemoteStopResult.InvalidSessionId(SessionId);
                    }

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

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopResponse?.Invoke(EndTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             SessionId,
                                             ReservationHandling,
                                             null,
                                             null,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout,
                                             result,
                                             EndTime - StartTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EVSE) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (internal) SendNewChargingSession   (Timestamp, Sender, Session)

        internal void SendNewChargingSession(DateTime Timestamp,
                                             Object Sender,
                                             ChargingSession Session)
        {

            if (Session is not null)
            {

                if (Session.EVSE is null)
                {
                    Session.EVSE = this;
                    Session.EVSEId = Id;
                }

                OnNewChargingSession?.Invoke(Timestamp, Sender, Session);

            }

        }

        #endregion

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime Timestamp,
                                                Object Sender,
                                                ChargeDetailRecord ChargeDetailRecord)
        {

            if (ChargeDetailRecord is not null)
                OnNewChargeDetailRecord?.Invoke(Timestamp, Sender, ChargeDetailRecord);

        }

        #endregion

        #endregion


        #region ToJSON(this EVSE, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given EVSE.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        public JObject ToJSON(Boolean Embedded = false,
                              InfoStatus ExpandRoamingNetworkId = InfoStatus.ShowIdOnly,
                              InfoStatus ExpandChargingStationOperatorId = InfoStatus.ShowIdOnly,
                              InfoStatus ExpandChargingPoolId = InfoStatus.ShowIdOnly,
                              InfoStatus ExpandChargingStationId = InfoStatus.ShowIdOnly,
                              InfoStatus ExpandBrandIds = InfoStatus.ShowIdOnly,
                              InfoStatus ExpandDataLicenses = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<EVSE>? CustomEVSESerializer = null)

        {

            var JSON = JSONObject.Create(

                           new JProperty("@id", Id.ToString()),

                           !Embedded
                               ? new JProperty("@context", JSONLDContext)
                               : null,

                           Description.IsNeitherNullNorEmpty()
                               ? new JProperty("description", Description.ToJSON())
                               : null,

                           Brands.SafeAny()
                                 ? ExpandBrandIds.Switch(
                                       () => new JProperty("brandId", Brands.Select(brand => brand.Id.ToString())),
                                       () => new JProperty("brand", Brands.ToJSON()))
                                 : null,

                           (!Embedded || DataSource != ChargingStation.DataSource)
                               ? new JProperty("dataSource", DataSource)
                               : null,

                           (!Embedded || DataLicenses != ChargingStation.DataLicenses)
                               ? ExpandDataLicenses.Switch(
                                     () => new JProperty("dataLicenseIds", new JArray(DataLicenses.SafeSelect(license => license.Id.ToString()))),
                                     () => new JProperty("dataLicenses", DataLicenses.ToJSON()))
                               : null,

                           ExpandRoamingNetworkId != InfoStatus.Hidden && RoamingNetwork != null
                               ? ExpandRoamingNetworkId.Switch(
                                     () => new JProperty("roamingNetworkId", RoamingNetwork.Id.ToString()),
                                     () => new JProperty("roamingNetwork", RoamingNetwork.ToJSON(Embedded: true,
                                                                                                                        ExpandChargingStationOperatorIds: InfoStatus.Hidden,
                                                                                                                        ExpandChargingPoolIds: InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationIds: InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds: InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds: InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses: InfoStatus.Hidden)))
                               : null,

                           ExpandChargingStationOperatorId != InfoStatus.Hidden && Operator != null
                               ? ExpandChargingStationOperatorId.Switch(
                                     () => new JProperty("chargingStationOperatorperatorId", Operator.Id.ToString()),
                                     () => new JProperty("chargingStationOperatorperator", Operator.ToJSON(Embedded: true,
                                                                                                                        ExpandRoamingNetworkId: InfoStatus.Hidden,
                                                                                                                        ExpandChargingPoolIds: InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationIds: InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds: InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds: InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses: InfoStatus.Hidden)))
                               : null,

                           ExpandChargingPoolId != InfoStatus.Hidden && ChargingPool != null
                               ? ExpandChargingPoolId.Switch(
                                     () => new JProperty("chargingPoolId", ChargingPool.Id.ToString()),
                                     () => new JProperty("chargingPool", ChargingPool.ToJSON(Embedded: true,
                                                                                                                        ExpandRoamingNetworkId: InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationOperatorId: InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationIds: InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds: InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds: InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses: InfoStatus.Hidden)))
                               : null,

                           ExpandChargingStationId != InfoStatus.Hidden && ChargingStation != null
                               ? ExpandChargingStationId.Switch(
                                     () => new JProperty("chargingStationId", ChargingStation.Id.ToString()),
                                     () => new JProperty("chargingStation", ChargingStation.ToJSON(Embedded: true,
                                                                                                                        ExpandRoamingNetworkId: InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationOperatorId: InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds: InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds: InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses: InfoStatus.Hidden)))
                               : null,

                           !Embedded ? new JProperty("geoLocation", ChargingStation.GeoLocation.Value.ToJSON()) : null,
                           !Embedded ? new JProperty("address", ChargingStation.Address.ToJSON()) : null,
                           !Embedded ? new JProperty("authenticationModes", ChargingStation.AuthenticationModes.ToJSON()) : null,

                           ChargingModes.SafeAny()
                               ? new JProperty("chargingModes", new JArray(ChargingModes.SafeSelect(mode => mode.ToText())))
                               : null,

                           CurrentType.HasValue && CurrentType.Value != CurrentTypes.Unspecified
                               ? new JProperty("currentTypes", new JArray(CurrentType.Value.ToText()))
                               : null,

                           AverageVoltage.HasValue && AverageVoltage > 0 ? new JProperty("averageVoltage", Math.Round(AverageVoltage.Value, 2)) : null,
                           MaxCurrent.HasValue && MaxCurrent > 0 ? new JProperty("maxCurrent", Math.Round(MaxCurrent.Value, 2)) : null,
                           MaxPower.HasValue && MaxPower.HasValue ? new JProperty("maxPower", Math.Round(MaxPower.Value, 2)) : null,
                           MaxCapacity.HasValue && MaxCapacity.HasValue ? new JProperty("maxCapacity", Math.Round(MaxCapacity.Value, 2)) : null,

                           SocketOutlets.Count > 0
                               ? new JProperty("socketOutlets", new JArray(SocketOutlets.ToJSON()))
                               : null,

                           EnergyMeter is not null
                               ? new JProperty("energyMeter", EnergyMeter.ToJSON())
                               : null,

                           !Embedded && ChargingStation?.OpeningTimes is not null
                               ? new JProperty("openingTimes", ChargingStation.OpeningTimes.ToJSON())
                               : null

                     );

            return CustomEVSESerializer is not null
                       ? CustomEVSESerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region IEnumerable<SocketOutlet> Members

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => SocketOutlets.GetEnumerator();

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        public IEnumerator<SocketOutlet> GetEnumerator()
            => SocketOutlets.GetEnumerator();

        #endregion

        #region Operator overloading

        #region Operator == (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator ==(EVSE EVSE1, EVSE EVSE2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSE1, EVSE2))
                return true;

            // If one is null, but not both, return false.
            if (EVSE1 is null || EVSE2 is null)
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
        public static Boolean operator !=(EVSE EVSE1, EVSE EVSE2)
            => !(EVSE1 == EVSE2);

        #endregion

        #region Operator <  (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <(EVSE EVSE1, EVSE EVSE2)
        {

            if (EVSE1 is null)
                throw new ArgumentNullException(nameof(EVSE1), "The given EVSE1 must not be null!");

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
        public static Boolean operator <=(EVSE EVSE1, EVSE EVSE2)
            => !(EVSE1 > EVSE2);

        #endregion

        #region Operator >  (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >(EVSE EVSE1, EVSE EVSE2)
        {

            if (EVSE1 is null)
                throw new ArgumentNullException(nameof(EVSE1), "The given EVSE1 must not be null!");

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
        public static Boolean operator >=(EVSE EVSE1, EVSE EVSE2)
            => !(EVSE1 < EVSE2);

        #endregion

        #endregion

        #region IComparable<EVSE> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is EVSE evse
                   ? CompareTo(evse)
                   : throw new ArgumentException("The given object is not an evse!", nameof(Object));

        #endregion

        #region CompareTo(EVSE)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE">An EVSE to compare with.</param>
        public Int32 CompareTo(EVSE? EVSE)

            => EVSE is not null
                   ? Id.CompareTo(EVSE.Id)
                   : throw new ArgumentException("The given object is not an EVSE!", nameof(EVSE));

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IEVSE">An IEVSE to compare with.</param>
        public Int32 CompareTo(IEVSE? IEVSE)

            => IEVSE is not null
                   ? Id.CompareTo(IEVSE.Id)
                   : throw new ArgumentException("The given object is not an IEVSE!", nameof(IEVSE));

        #endregion

        #endregion

        #region IEquatable<EVSE> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is EVSE evse &&
                  Equals(evse);

        #endregion

        #region Equals(EVSE)

        /// <summary>
        /// Compares two EVSEs for equality.
        /// </summary>
        /// <param name="EVSE">An EVSE to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE? EVSE)

            => EVSE is not null &&
               Id.Equals(EVSE.Id);

        /// <summary>
        /// Compares two EVSEs for equality.
        /// </summary>
        /// <param name="IEVSE">An EVSE to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IEVSE? IEVSE)

            => IEVSE is not null &&
               Id.Equals(IEVSE.Id);

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
