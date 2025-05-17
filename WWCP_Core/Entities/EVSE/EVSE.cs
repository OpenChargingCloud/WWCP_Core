/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Runtime.CompilerServices;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// This is meant to be one electrical circuit which can charge a electric vehicle
    /// independently. Thus there could be multiple interdependent power sockets.
    /// </summary>
    public class EVSE : AEMobilityEntity<EVSE_Id,
                                         EVSEAdminStatusType,
                                         EVSEStatusType>,
                        IEquatable<EVSE>, IComparable<EVSE>,
                        IEVSE
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const            String    JSONLDContext                           = "https://open.charging.cloud/contexts/wwcp+json/EVSE";


        private readonly        Decimal   EPSILON                                 = 0.01m;

        /// <summary>
        /// The default max size of the EVSE admin status schedule/history.
        /// </summary>
        public const            UInt16    DefaultMaxEVSEAdminStatusScheduleSize   = 50;

        /// <summary>
        /// The default max size of the EVSE status schedule/history.
        /// </summary>
        public const            UInt16    DefaultMaxEVSEStatusScheduleSize        = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly  TimeSpan  DefaultMaxReservationDuration           = TimeSpan.FromMinutes(15);

        #endregion

        #region Properties

        /// <summary>
        /// The roaming network of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork?                         RoamingNetwork
            => ChargingStation?.RoamingNetwork;

        /// <summary>
        /// The charging station operator of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public IChargingStationOperator?                Operator
            => ChargingStation?.Operator;

        /// <summary>
        /// The charging pool of this EVSE.
        /// </summary>
        public IChargingPool?                           ChargingPool
            => ChargingStation?.ChargingPool;

        /// <summary>
        /// The charging station of this EVSE.
        /// </summary>
        public IChargingStation?                        ChargingStation             { get; }

        /// <summary>
        /// An optional remote EVSE.
        /// </summary>
        public IRemoteEVSE?                             RemoteEVSE                  { get; }

        #region PhysicalReference

        private String? physicalReference;

        /// <summary>
        /// An optional number/string printed on the outside of the EVSE for visual identification.
        /// </summary>
        [Optional, SlowData]
        public String? PhysicalReference
        {

            get
            {
                return physicalReference;
            }

            set
            {

                if (physicalReference != value)
                    SetProperty(ref physicalReference,
                                value);

            }

        }

        #endregion

        /// <summary>
        /// The geographical location of this EVSE, e.g. when this EVSE is part of a satellite system.
        /// </summary>
        public GeoCoordinate?                           GeoLocation                 { get; }

        /// <summary>
        /// An optional enumeration of links to photos related to the EVSE.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<URL>                         PhotoURLs                   { get; }

        /// <summary>
        /// An enumeration of all brands registered for this EVSE.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<Brand>                       Brands                      { get; }

        /// <summary>
        /// All e-mobility related Root-CAs, e.g. ISO 15118-2/-20, available at this EVSE.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<RootCAInfo>                  MobilityRootCAs             { get; }

        /// <summary>
        /// An enumeration of all data license(s) of this EVSE.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<DataLicense>                 DataLicenses                { get; }

        /// <summary>
        /// The optional URL where certificates, identifiers and public keys related to the calibration
        /// of meters in this EVSE can be found.
        /// </summary>
        public URL?                                     CalibrationInfo             { get; }

        /// <summary>
        /// An enumeration of all supported charging modes of this EVSE.
        /// </summary>
        [Mandatory, SlowData]
        public ReactiveSet<ChargingModes>               ChargingModes               { get; }

        ///// <summary>
        ///// An enumeration of all available charging tariffs at this EVSE.
        ///// </summary>
        //[Optional, SlowData]
        //public ReactiveSet<ChargingTariff>              ChargingTariffs             { get; }


        //public Dictionary<EMPRoamingProvider_Id, List<ChargingTariff_Id>>  ChargingTariffIds    { get; }


        #region CurrentType

        private CurrentTypes currentType;

        /// <summary>
        /// The type of the current.
        /// </summary>
        [Mandatory, SlowData]
        public CurrentTypes CurrentType
        {

            get
            {
                return currentType;
            }

            set
            {

                if (currentType != value)
                    SetProperty(ref currentType,
                                value);

            }

        }

        #endregion


        #region AverageVoltage

        private Volt? averageVoltage;

        /// <summary>
        /// The average voltage.
        /// </summary>
        [Optional, SlowData]
        public Volt? AverageVoltage
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

                    else if (Math.Abs(averageVoltage.Value.Value - value.Value.Value) > EPSILON)
                        SetProperty(ref averageVoltage,
                                    value);

                }
                else
                    DeleteProperty(ref averageVoltage);

            }

        }

        #endregion

        #region AverageVoltageRealTime

        private Timestamped<Volt>? averageVoltageRealTime;

        /// <summary>
        /// The real-time average voltage.
        /// </summary>
        [Optional, FastData]
        public Timestamped<Volt>? AverageVoltageRealTime
        {

            get
            {
                return averageVoltageRealTime;
            }

            set
            {

                if (value is not null)
                {

                    if (!averageVoltageRealTime.HasValue || Math.Abs(averageVoltageRealTime.Value.Value.Value - value.Value.Value.Value) > EPSILON)
                        SetProperty(ref averageVoltageRealTime,
                                    value);

                }
                else
                    DeleteProperty(ref averageVoltage);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the average voltage.
        /// </summary>
        [Optional, FastData]
        public ReactiveSet<Timestamped<Volt>>           AverageVoltagePrognoses     { get; }


        #region MaxCurrent

        private Ampere? maxCurrent;

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        [Mandatory, SlowData]
        public Ampere? MaxCurrent
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
                                    value);

                    else if (Math.Abs(maxCurrent.Value.Value - value.Value.Value) > EPSILON)
                        SetProperty(ref maxCurrent,
                                    value);

                }
                else
                    DeleteProperty(ref maxCurrent);

            }

        }

        #endregion

        #region MaxCurrentRealTime

        private Timestamped<Ampere>? maxCurrentRealTime;

        /// <summary>
        /// The real-time maximum current [Ampere].
        /// </summary>
        [Optional, FastData]
        public Timestamped<Ampere>? MaxCurrentRealTime
        {

            get
            {
                return maxCurrentRealTime;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxCurrentRealTime.HasValue || Math.Abs(maxCurrentRealTime.Value.Value.Value - value.Value.Value.Value) > EPSILON)
                        SetProperty(ref maxCurrentRealTime,
                                    value);

                }

                else
                    DeleteProperty(ref maxCurrentRealTime);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the maximum current].
        /// </summary>
        [Optional, FastData]
        public ReactiveSet<Timestamped<Ampere>>         MaxCurrentPrognoses         { get; }


        #region MaxPower

        private Watt? maxPower;

        /// <summary>
        /// The maximum power.
        /// </summary>
        [Optional, SlowData]
        public Watt? MaxPower
        {

            get
            {
                return maxPower;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxPower.HasValue || Math.Abs(maxPower.Value.Value - value.Value.Value) > EPSILON)
                        SetProperty(ref maxPower,
                                    value);

                }
                else
                    DeleteProperty(ref maxPower);

            }

        }

        #endregion

        #region MaxPowerRealTime

        private Timestamped<Watt>? maxPowerRealTime;

        /// <summary>
        /// The real-time maximum power.
        /// </summary>
        [Optional, FastData]
        public Timestamped<Watt>? MaxPowerRealTime
        {

            get
            {
                return maxPowerRealTime;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxPowerRealTime.HasValue || Math.Abs(maxPowerRealTime.Value.Value.Value - value.Value.Value.Value) > EPSILON)
                        SetProperty(ref maxPowerRealTime,
                                    value);

                }
                else
                    DeleteProperty(ref maxPowerRealTime);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the maximum power.
        /// </summary>
        [Optional, FastData]
        public ReactiveSet<Timestamped<Watt>>           MaxPowerPrognoses           { get; }


        #region MaxCapacity

        private WattHour? maxCapacity;

        /// <summary>
        /// The maximum capacity.
        /// </summary>
        [Mandatory]
        public WattHour? MaxCapacity
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
                                    value);

                    else if (Math.Abs(maxCapacity.Value.Value - value.Value.Value) > EPSILON)
                        SetProperty(ref maxCapacity,
                                    value);

                }
                else
                    DeleteProperty(ref maxCapacity);

            }

        }

        #endregion

        #region MaxCapacityRealTime

        private Timestamped<WattHour>? maxCapacityRealTime;

        /// <summary>
        /// The real-time maximum capacity.
        /// </summary>
        [Optional]
        public Timestamped<WattHour>? MaxCapacityRealTime
        {

            get
            {
                return maxCapacityRealTime;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxCapacityRealTime.HasValue || Math.Abs(maxCapacityRealTime.Value.Value.Value - value.Value.Value.Value) > EPSILON)
                        SetProperty(ref maxCapacityRealTime,
                                    value);

                }
                else
                    DeleteProperty(ref maxCapacityRealTime);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the maximum capacity.
        /// </summary>
        [Optional]
        public ReactiveSet<Timestamped<WattHour>>       MaxCapacityPrognoses        { get; }


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
        [Optional, FastData]
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
                                value);

                else
                    DeleteProperty(ref energyMixRealTime);

            }

        }

        #endregion

        #region EnergyMixPrognoses

        private EnergyMixPrognosis? energyMixPrognoses;

        /// <summary>
        /// Prognoses on future values of the energy mix.
        /// </summary>
        [Optional, FastData]
        public EnergyMixPrognosis? EnergyMixPrognoses
        {

            get
            {
                return energyMixPrognoses ?? ChargingStation?.EnergyMixPrognoses;
            }

            set
            {

                if (value != energyMixPrognoses && value != ChargingStation?.EnergyMixPrognoses)
                {

                    if (value == null)
                        DeleteProperty(ref energyMixPrognoses);

                    else
                        SetProperty(ref energyMixPrognoses, value);

                }

            }

        }

        #endregion


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
                if (maxReservationDuration.TotalSeconds != value.TotalSeconds)
                    SetProperty(ref maxReservationDuration,
                                value);
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
                if (isFreeOfCharge != value)
                    SetProperty(ref isFreeOfCharge,
                                value);
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
        /// The power socket outlets.
        /// </summary>
        [Mandatory, SlowData]
        public ReactiveSet<IChargingConnector>          ChargingConnectors          { get; }

        /// <summary>
        /// The timestamp of the last status update.
        /// This might be different from the timestamp of the last status change,
        /// when the status was imported from a third party.
        /// </summary>
        [Optional]
        public DateTime?                                LastStatusUpdate            { get; set; }

        #endregion

        #region Events

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate?  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate?       OnStatusChanged;

        #endregion

        #region Reservations

        /// <summary>
        /// An event fired whenever a charging location is being reserved.
        /// </summary>
        public event OnReserveRequestDelegate?             OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate?            OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate?             OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate?   OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate?  OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate?        OnReservationCanceled;

        #endregion

        #region RemoteStart/-Stop

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate?     OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate?    OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate?     OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate?      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate?     OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate?  OnNewChargeDetailRecord;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE) having the given EVSE identification.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="ChargingStation">The charging station hosting this EVSE.</param>
        /// 
        /// <param name="RemoteEVSECreator">A delegate to attach a remote EVSE.</param>
        /// <param name="InitialAdminStatus">An optional initial admin status of the EVSE.</param>
        /// <param name="InitialStatus">An optional initial status of the EVSE.</param>
        /// <param name="MaxAdminStatusScheduleSize">An optional max length of the admin staus schedule.</param>
        /// <param name="MaxStatusScheduleSize">An optional max length of the staus schedule.</param>
        /// 
        /// <param name="Name">An optional multi-language text e.g. printed on the outside of the EVSE for visual identification.</param>
        /// <param name="Description">An optional multi-language description of this EVSE.</param>
        /// 
        /// <param name="PhotoURLs">An optional enumeration of links to photos related to the EVSE.</param>
        /// <param name="Brands">An optional enumeration of brands registered for this EVSE.</param>
        /// <param name="DataLicenses">An optional enumeration of data license(s) of this EVSE.</param>
        /// <param name="ChargingModes">An optional enumeration of the supported charging modes of this EVSE.</param>
        /// 
        /// <param name="DataSource"></param>
        /// <param name="LastChange"></param>
        /// 
        /// <param name="Configurator">A delegate to configure the newly created EVSE.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="InternalData">Optional internal data.</param>
        public EVSE(EVSE_Id                              Id,
                    IChargingStation                     ChargingStation,
                    I18NString?                          Name                         = null,
                    I18NString?                          Description                  = null,

                    String?                              PhysicalReference            = null,
                    GeoCoordinate?                       GeoLocation                  = null,
                    IEnumerable<URL>?                    PhotoURLs                    = null,
                    IEnumerable<Brand>?                  Brands                       = null,
                    IEnumerable<RootCAInfo>?             MobilityRootCAs              = null,
                    IEnumerable<DataLicense>?            DataLicenses                 = null,
                    IEnumerable<ChargingModes>?          ChargingModes                = null,
                    IEnumerable<ChargingTariff>?         ChargingTariffs              = null,
                    CurrentTypes?                        CurrentType                  = null,
                    Volt?                                AverageVoltage               = null,
                    Timestamped<Volt>?                   AverageVoltageRealTime       = null,
                    IEnumerable<Timestamped<Volt>>?      AverageVoltagePrognoses      = null,
                    Ampere?                              MaxCurrent                   = null,
                    Timestamped<Ampere>?                 MaxCurrentRealTime           = null,
                    IEnumerable<Timestamped<Ampere>>?    MaxCurrentPrognoses          = null,
                    Watt?                                MaxPower                     = null,
                    Timestamped<Watt>?                   MaxPowerRealTime             = null,
                    IEnumerable<Timestamped<Watt>>?      MaxPowerPrognoses            = null,
                    WattHour?                            MaxCapacity                  = null,
                    Timestamped<WattHour>?               MaxCapacityRealTime          = null,
                    IEnumerable<Timestamped<WattHour>>?  MaxCapacityPrognoses         = null,
                    EnergyMix?                           EnergyMix                    = null,
                    Timestamped<EnergyMix>?              EnergyMixRealTime            = null,
                    EnergyMixPrognosis?                  EnergyMixPrognoses           = null,
                    EnergyMeter?                         EnergyMeter                  = null,
                    Boolean?                             IsFreeOfCharge               = null,
                    URL?                                 CalibrationInfo              = null,
                    IEnumerable<IChargingConnector>?     ChargingConnectors           = null,

                    ChargingSession?                     ChargingSession              = null,

                    Timestamped<EVSEAdminStatusType>?    InitialAdminStatus           = null,
                    Timestamped<EVSEStatusType>?         InitialStatus                = null,
                    UInt16?                              MaxAdminStatusScheduleSize   = null,
                    UInt16?                              MaxStatusScheduleSize        = null,
                    DateTime?                            LastStatusUpdate             = null,

                    String?                              DataSource                   = null,
                    DateTime?                            Created                      = null,
                    DateTime?                            LastChange                   = null,

                    JObject?                             CustomData                   = null,
                    UserDefinedDictionary?               InternalData                 = null,

                    Action<IEVSE>?                       Configurator                 = null,
                    RemoteEVSECreatorDelegate?           RemoteEVSECreator            = null)

            : base(Id,
                   Name,
                   Description,
                   InitialAdminStatus         ?? EVSEAdminStatusType.Operational,
                   InitialStatus              ?? EVSEStatusType.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxEVSEAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxEVSEStatusScheduleSize,
                   DataSource,
                   Created,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Init data and properties

            this.ChargingStation                    = ChargingStation;

            this.physicalReference                  = PhysicalReference;
            this.GeoLocation                        = GeoLocation;
            this.PhysicalReference                  = PhysicalReference;
            this.GeoLocation                        = GeoLocation;

            this.PhotoURLs                          = PhotoURLs is null
                                                          ? []
                                                          : [.. PhotoURLs];
            this.PhotoURLs.OnSetChanged            += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("PhotoURLs",
                                oldItems,
                                newItems);

            };

            this.Brands                             = Brands is null
                                                          ? []
                                                          : [.. Brands];
            this.Brands.OnSetChanged               += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("DataLicenses",
                                oldItems,
                                newItems);

            };


            this.MobilityRootCAs = new ReactiveSet<RootCAInfo>();

            if (MobilityRootCAs is not null)
                foreach (var mobilityRootCA in MobilityRootCAs)
                    this.MobilityRootCAs.Add(mobilityRootCA);

            this.MobilityRootCAs.OnSetChanged       += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("MobilityRootCAs",
                                oldItems,
                                newItems);

            };

            this.DataLicenses                   = DataLicenses is null
                                                      ? []
                                                      : [.. DataLicenses];
            this.DataLicenses.OnSetChanged     += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("DataLicenses",
                                oldItems,
                                newItems);

            };

            this.ChargingModes                      = ChargingModes is null
                                                          ? []
                                                          : [.. ChargingModes];
            this.ChargingModes.OnSetChanged        += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("ChargingModes",
                                oldItems,
                                newItems);

            };

            //this.ChargingTariffs                    = ChargingTariffs is null
            //                                              ? new ReactiveSet<ChargingTariff>()
            //                                              : new ReactiveSet<ChargingTariff>(ChargingTariffs);
            //this.ChargingTariffs.OnSetChanged      += (timestamp, reactiveSet, newItems, oldItems) =>
            //{

            //    PropertyChanged("ChargingTariffs",
            //                    oldItems,
            //                    newItems);

            //};

            this.CalibrationInfo                    = CalibrationInfo;

            this.currentType                        = CurrentType ?? CurrentTypes.AC_ThreePhases;

            this.averageVoltage                     = AverageVoltage;
            this.averageVoltageRealTime             = AverageVoltageRealTime;

            this.AverageVoltagePrognoses            = AverageVoltagePrognoses is null
                                                          ? []
                                                          : [.. AverageVoltagePrognoses];
            this.AverageVoltagePrognoses.OnSetChanged  += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("AverageVoltagePrognoses",
                                oldItems,
                                newItems);

            };


            this.maxCurrent                         = MaxCurrent;
            this.maxCurrentRealTime                 = MaxCurrentRealTime;

            this.MaxCurrentPrognoses                = MaxCurrentPrognoses is null
                                                          ? []
                                                          : [.. MaxCurrentPrognoses];
            this.MaxCurrentPrognoses.OnSetChanged  += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("MaxCurrentPrognoses",
                                oldItems,
                                newItems);

            };

            this.maxPower                           = MaxPower;
            this.maxPowerRealTime                   = MaxPowerRealTime;

            this.MaxPowerPrognoses                  = MaxPowerPrognoses is null
                                                          ? []
                                                          : [.. MaxPowerPrognoses];
            this.MaxPowerPrognoses.OnSetChanged    += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("MaxPowerPrognoses",
                                oldItems,
                                newItems);

            };

            this.maxCapacity                        = MaxCapacity;
            this.maxCapacityRealTime                = MaxCapacityRealTime;

            this.MaxCapacityPrognoses               = MaxCapacityPrognoses is null
                                                          ? []
                                                          : [.. MaxCapacityPrognoses];
            this.MaxCapacityPrognoses.OnSetChanged += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("MaxCapacityPrognoses",
                                oldItems,
                                newItems);

            };

            this.energyMix                          = EnergyMix;
            this.energyMixRealTime                  = EnergyMixRealTime;
            this.energyMixPrognoses                 = EnergyMixPrognoses;

            this.energyMeter                        = EnergyMeter;

            this.IsFreeOfCharge                     = IsFreeOfCharge ?? false;

            this.ChargingConnectors                 = ChargingConnectors is null
                                                          ? []
                                                          : [.. ChargingConnectors];
            this.ChargingConnectors.OnSetChanged   += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("ChargingConnectors",
                                oldItems,
                                newItems);

            };

            foreach (var chargingConnector in this.ChargingConnectors)
                chargingConnector.EVSE = this;

            this.ChargingSession                    = ChargingSession;
            this.LastStatusUpdate                   = LastStatusUpdate;

            #endregion

            Configurator?.Invoke(this);

            #region Link events

            this.OnPropertyChanged                   += (timestamp, eventTrackingId, sender, propertyName, newValue, oldValue, dataSource)
                                                         => UpdateData       (timestamp, eventTrackingId, propertyName, newValue, oldValue, dataSource);

            this.adminStatusSchedule.OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                         => UpdateAdminStatus(timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            this.statusSchedule.     OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                         => UpdateStatus     (timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            #endregion

            #region RemoteEVSE

            this.RemoteEVSE = RemoteEVSECreator?.Invoke(this);

            if (this.RemoteEVSE is not null)
            {

                this.RemoteEVSE.OnAdminStatusChanged    += (timestamp, eventTrackingId, remoteEVSE, newStatus, oldStatus, dataSource) => { adminStatusSchedule.Insert(newStatus, dataSource); return Task.CompletedTask; };
                this.RemoteEVSE.OnStatusChanged         += (timestamp, eventTrackingId, remoteEVSE, newStatus, oldStatus, dataSource) => { statusSchedule.     Insert(newStatus, dataSource); return Task.CompletedTask; };

                this.RemoteEVSE.OnNewReservation        += (Timestamp, RemoteEVSE, Reservation)         => OnNewReservation.     Invoke(Timestamp, RemoteEVSE, Reservation);
                this.RemoteEVSE.OnReservationCanceled   += (Timestamp, RemoteEVSE, Reservation, Reason) => OnReservationCanceled.Invoke(Timestamp, RemoteEVSE, Reservation, Reason);

                this.RemoteEVSE.OnNewChargingSession    += (Timestamp, RemoteEVSE, ChargingSession) => {

                                                               RoamingNetwork?.SessionsStore.AddOrUpdateSession(
                                                                   ChargingSession,
                                                                   (session) => {
                                                                       session.EVSEId = Id;
                                                                       session.EVSE   = this;
                                                                       return session;
                                                                   }
                                                               );

                                                               //_ChargingSession       = ChargingSession;
                                                               //_ChargingSession.EVSE  = this;

                                                               OnNewChargingSession?.Invoke(Timestamp, this, ChargingSession);

                                                           };

                this.RemoteEVSE.OnNewChargeDetailRecord += (Timestamp, RemoteEVSE, ChargeDetailRecord) => OnNewChargeDetailRecord?.Invoke(Timestamp, RemoteEVSE, ChargeDetailRecord);

            }

            #endregion

        }

        #endregion


        #region UpdateWith(OtherEVSE)

        /// <summary>
        /// Update this EVSE with the data of the other EVSE.
        /// </summary>
        /// <param name="OtherEVSE">Another EVSE.</param>
        public IEVSE UpdateWith(IEVSE OtherEVSE)
        {

            Name.                   Set    (OtherEVSE.Name);
            Description.            Set    (OtherEVSE.Description);

            Brands.                 Replace(OtherEVSE.Brands);
            ChargingModes.          Replace(OtherEVSE.ChargingModes);
            ChargingConnectors.     Replace(OtherEVSE.ChargingConnectors);
            DataLicenses.           Replace(OtherEVSE.DataLicenses);
            AverageVoltagePrognoses.Replace(OtherEVSE.AverageVoltagePrognoses);
            MaxCurrentPrognoses.    Replace(OtherEVSE.MaxCurrentPrognoses);
            MaxPowerPrognoses.      Replace(OtherEVSE.MaxPowerPrognoses);
            MaxCapacityPrognoses.   Replace(OtherEVSE.MaxCapacityPrognoses);

            CurrentType                = OtherEVSE.CurrentType;
            AverageVoltage             = OtherEVSE.AverageVoltage;
            AverageVoltageRealTime     = OtherEVSE.AverageVoltageRealTime;
            MaxCurrent                 = OtherEVSE.MaxCurrent;
            MaxCurrentRealTime         = OtherEVSE.MaxCurrentRealTime;
            MaxPower                   = OtherEVSE.MaxPower;
            MaxPowerRealTime           = OtherEVSE.MaxPowerRealTime;
            MaxCapacity                = OtherEVSE.MaxCapacity;
            MaxCapacityRealTime        = OtherEVSE.MaxCapacityRealTime;
            EnergyMix                  = OtherEVSE.EnergyMix;               //ToDo: Implement Equality!
            EnergyMixRealTime          = OtherEVSE.EnergyMixRealTime;       //ToDo: Implement Equality!
            EnergyMixPrognoses         = OtherEVSE.EnergyMixPrognoses;      //ToDo: Implement Equality!
            EnergyMeter                = OtherEVSE.EnergyMeter;             //ToDo: Implement Equality!
            IsFreeOfCharge             = OtherEVSE.IsFreeOfCharge;
            MaxReservationDuration     = OtherEVSE.MaxReservationDuration;

            if (OtherEVSE.AdminStatus.Timestamp > AdminStatus.Timestamp)
                AdminStatus            = OtherEVSE.AdminStatus;

            if (OtherEVSE.Status.     Timestamp > Status.     Timestamp)
                Status                 = OtherEVSE.Status;

            return this;

        }

        #endregion


        #region Data/(Admin-)Status


        public void SetAdminStatus(EVSEAdminStatus EVSEAdminStatus)
        {

            //adminStatusSchedule.Insert(EVSEAdminStatus.Status,
            //                           EVSEAdminStatus.Timestamp,
            //                           EVSEAdminStatus.DataSource);

        }

        public void SetAdminStatus(EVSEAdminStatusUpdate EVSEAdminStatusUpdate)
        {

        }

        public void SetStatus(EVSEStatus EVSEStatus)
        {

            statusSchedule.Insert(EVSEStatus.Status,
                                  EVSEStatus.Timestamp,
                                  EVSEStatus.Context);

        }

        public void SetStatus(EVSEStatusUpdate EVSEStatusUpdate)
        {

            statusSchedule.Insert(EVSEStatusUpdate.NewStatus,
                                  EVSEStatusUpdate.Context);

        }

        public void SetEnergyStatus(EVSEEnergyStatus EVSEEnergyStatus)
        {

        }

        public void SetEnergyStatus(EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate)
        {

        }


        #region (internal) UpdateData       (Timestamp, EventTrackingId, PropertyName, NewValue, OldValue = null, DataSource = null)

        /// <summary>
        /// Update the static data of the EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The optional old value of the changed property.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        internal async Task UpdateData(DateTime          Timestamp,
                                       EventTracking_Id  EventTrackingId,
                                       String            PropertyName,
                                       Object?           NewValue,
                                       Object?           OldValue     = null,
                                       Context?          DataSource   = null)
        {

            try
            {

                var onEVSEDataChanged = OnDataChanged;
                if (onEVSEDataChanged is not null)
                    await onEVSEDataChanged(Timestamp,
                                            EventTrackingId,
                                            this,
                                            PropertyName,
                                            NewValue,
                                            OldValue,
                                            DataSource);

            }
            catch (Exception e)
            {

                DebugX.Log(e,
                           $"EVSE '{Id}'.UpdateEVSEData of property '{PropertyName}' from '{OldValue?.ToString() ?? "-"}' to '{NewValue?.ToString() ?? "-"}'" +
                           DataSource is not null ? $" ({DataSource})" : "");

            }

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, NewAdminStatus, OldAdminStatus = null, DataSource = null)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="NewAdminStatus">The new EVSE admin status.</param>
        /// <param name="OldAdminStatus">The optional old EVSE admin status.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        internal async Task UpdateAdminStatus(DateTime                            Timestamp,
                                              EventTracking_Id                    EventTrackingId,
                                              Timestamped<EVSEAdminStatusType>   NewAdminStatus,
                                              Timestamped<EVSEAdminStatusType>?  OldAdminStatus   = null,
                                              Context?                            DataSource       = null)
        {

            try
            {

                var onEVSEAdminStatusChanged = OnAdminStatusChanged;
                if (onEVSEAdminStatusChanged is not null)
                    await onEVSEAdminStatusChanged(Timestamp,
                                                   EventTrackingId,
                                                   this,
                                                   NewAdminStatus,
                                                   OldAdminStatus,
                                                   DataSource);

            }
            catch (Exception e)
            {

                DebugX.Log(e,
                           $"EVSE '{Id}'.UpdateEVSEAdminStatus from '{OldAdminStatus?.ToString() ?? "-"}' to '{NewAdminStatus}'" +
                           DataSource is not null ? $" ({DataSource})" : "");

            }

        }

        #endregion

        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        /// <param name="OldStatus">The optional old EVSE status.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        internal async Task UpdateStatus(DateTime                       Timestamp,
                                         EventTracking_Id               EventTrackingId,
                                         Timestamped<EVSEStatusType>   NewStatus,
                                         Timestamped<EVSEStatusType>?  OldStatus    = null,
                                         Context?                       DataSource   = null)
        {

            try
            {

                var onEVSEStatusChanged = OnStatusChanged;
                if (onEVSEStatusChanged is not null)
                    await onEVSEStatusChanged(Timestamp,
                                              EventTrackingId,
                                              this,
                                              NewStatus,
                                              OldStatus,
                                              DataSource);

            }
            catch (Exception e)
            {

                DebugX.Log(e,
                           $"EVSE '{Id}'.UpdateEVSEStatus from '{OldStatus}' to '{NewStatus}'" +
                           DataSource is not null ? $" ({DataSource})" : "");

            }

        }

        #endregion

        #endregion

        #region ChargingReservation(s)

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations

            => RoamingNetwork?.ReservationsStore.Where (reservationCollection => reservationCollection.EVSEId == Id).
                                                 Select(reservationCollection => reservationCollection.LastOrDefault()).
                                                 Where (result                => result is not null).
                                                 Cast<ChargingReservation>()

                   ?? Array.Empty<ChargingReservation>();

        #endregion

        #region ChargingSession(s)

        /// <summary>
        /// The current charging session, if available.
        /// </summary>
        [InternalUseOnly]
        public ChargingSession?  ChargingSession    { get; set; }


        /// <summary>
        /// All charging sessions.
        /// </summary>
        public IEnumerable<ChargingSession> ChargingSessions

            => ChargingSession is not null
                   ? [ ChargingSession ]
                   : [];


        #region ContainsChargingSessionId (ChargingSessionId)

        /// <summary>
        /// Whether the given charging session identification is known within the EVSE.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        public Boolean ContainsChargingSessionId(ChargingSession_Id ChargingSessionId)

            => ChargingSessionId == ChargingSession?.Id;

        #endregion

        #region GetChargingSessionById    (ChargingSessionId)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        public ChargingSession? GetChargingSessionById(ChargingSession_Id ChargingSessionId)

            => RoamingNetwork?.GetChargingSessionById(ChargingSessionId);

        #endregion

        #region TryGetChargingSessionById (ChargingSessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given charging session identification.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id    ChargingSessionId,
                                                 out ChargingSession?  ChargingSession)
        {

            if (ChargingSessionId == this.ChargingSession?.Id)
            {
                ChargingSession = this.ChargingSession;
                return true;
            }

            ChargingSession = null;
            return false;

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

        #endregion


        #region Reserve/CancelReservation

        #region Reserve(                                           StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at this EVSE.
        /// </summary>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public Task<ReservationResult>

            Reserve(DateTime?                          StartTime              = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    Auth_Path?                         AuthenticationPath     = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<EMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null,
                    CancellationToken                  CancellationToken      = default)

                => Reserve(ChargingLocation.FromEVSEId(Id),
                           ChargingReservationLevel.EVSE,
                           StartTime,
                           Duration,
                           ReservationId,
                           LinkedReservationId,
                           ProviderId,
                           RemoteAuthentication,
                           AuthenticationPath,
                           ChargingProduct,
                           AuthTokens,
                           eMAIds,
                           PINs,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout,
                           CancellationToken);

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
        /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<ReservationResult>

            Reserve(ChargingLocation                   ChargingLocation,
                    ChargingReservationLevel           ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                          ReservationStartTime   = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    Auth_Path?                         AuthenticationPath     = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<EMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null,
                    CancellationToken                  CancellationToken      = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
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
                                         LinkedReservationId,
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

                else if (AdminStatus.Value == EVSEAdminStatusType.Operational ||
                         AdminStatus.Value == EVSEAdminStatusType.InternalUse)
                {

                    if (RemoteEVSE != null)
                    {

                        result = await RemoteEVSE.
                                           Reserve(ChargingLocation,
                                                   ReservationLevel,
                                                   ReservationStartTime,
                                                   Duration,
                                                   ReservationId,
                                                   LinkedReservationId,
                                                   ProviderId,
                                                   RemoteAuthentication,
                                                   AuthenticationPath,
                                                   ChargingProduct,
                                                   AuthTokens,
                                                   eMAIds,
                                                   PINs,

                                                   Timestamp,
                                                   EventTrackingId,
                                                   RequestTimeout,
                                                   CancellationToken);

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
                                          LinkedReservationId,
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

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,

                              DateTime?                              Timestamp           = null,
                              EventTracking_Id?                      EventTrackingId     = null,
                              TimeSpan?                              RequestTimeout      = null,
                              CancellationToken                      CancellationToken   = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            ChargingReservation?     canceledReservation  = null;
            CancelReservationResult? result               = null;

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

                if (AdminStatus.Value == EVSEAdminStatusType.Operational ||
                    AdminStatus.Value == EVSEAdminStatusType.InternalUse)
                {

                    if (RemoteEVSE != null)
                    {

                        result = await RemoteEVSE.
                                           CancelReservation(ReservationId,
                                                             Reason,

                                                             Timestamp,
                                                             EventTrackingId,
                                                             RequestTimeout,
                                                             CancellationToken);

                    }

                    else
                        result = CancelReservationResult.Offline(ReservationId,
                                                                 Reason);

                }

                else
                {

                    result = AdminStatus.Value switch {
                        _ => CancelReservationResult.OutOfService(ReservationId,
                                                                  Reason),
                    };
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


        #region GetChargingReservationById    (ReservationId)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        public ChargingReservation? GetChargingReservationById(ChargingReservation_Id ReservationId)
        {

            if (RoamingNetwork is not null &&
                RoamingNetwork.ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                return reservationCollection?.LastOrDefault();
            }

            return null;

        }

        #endregion

        #region GetChargingReservationsById   (ReservationId)

        /// <summary>
        /// Return the charging reservations specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        public ChargingReservationCollection? GetChargingReservationsById(ChargingReservation_Id ReservationId)
        {

            if (RoamingNetwork is not null &&
                RoamingNetwork.ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                return reservationCollection;
            }

            return null;

        }

        #endregion

        #region TryGetChargingReservationById (ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation? Reservation)
        {

            if (RoamingNetwork is not null &&
                RoamingNetwork.ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                Reservation = reservationCollection?.LastOrDefault();
                return true;
            }

            Reservation = null;
            return false;

        }

        #endregion

        #region TryGetChargingReservationsById(ReservationId, out ChargingReservations)

        /// <summary>
        /// Return the charging reservation collection specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="ChargingReservations">The charging reservations.</param>
        public Boolean TryGetChargingReservationsById(ChargingReservation_Id ReservationId, out ChargingReservationCollection? ChargingReservations)
        {

            if (RoamingNetwork is not null &&
                RoamingNetwork.ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                ChargingReservations = reservationCollection;
                return true;
            }

            ChargingReservations = null;
            return false;

        }

        #endregion

        #endregion

        #region AuthorizeStart/-Stop

        #region Properties

        public IId      AuthId
            => RoamingNetwork.AuthId;

        /// <summary>
        /// Disable the local authorization of charging processes.
        /// </summary>
        public Boolean  DisableAuthorization    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever an AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate?   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate?  OnAuthorizeStartResponse;


        /// <summary>
        /// An event fired whenever an AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate?    OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate?   OnAuthorizeStopResponse;

        #endregion

        #region AuthorizeStart           (LocalAuthentication, ChargingLocation = null, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingLocation?            ChargingLocation      = null,
                           ChargingProduct?             ChargingProduct       = null,
                           ChargingSession_Id?          SessionId             = null,
                           ChargingSession_Id?          CPOPartnerSessionId   = null,
                           ChargingStationOperator_Id?  OperatorId            = null,

                           DateTime?                    RequestTimestamp      = null,
                           EventTracking_Id?            EventTrackingId       = null,
                           TimeSpan?                    RequestTimeout        = null,
                           CancellationToken            CancellationToken     = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;
            RequestTimeout   ??= TimeSpan.FromSeconds(10);

            AuthStartResult? result = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStartRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          LocalAuthentication,
                          ChargingLocation,
                          ChargingProduct,
                          SessionId,
                          CPOPartnerSessionId,
                          [],
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                result = ChargingStation is not null

                             ? await ChargingStation.AuthorizeStart(
                                         LocalAuthentication,
                                         ChargingLocation,
                                         ChargingProduct,
                                         SessionId,
                                         CPOPartnerSessionId,
                                         OperatorId,

                                         RequestTimestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     )

                             : AuthStartResult.OutOfService(
                                   Id,
                                   this,
                                   SessionId:  SessionId,
                                   Runtime:    Timestamp.Now - startTime
                               );

            }
            catch (Exception e)
            {

                result = AuthStartResult.Error(
                             Id,
                             this,
                             SessionId:    SessionId,
                             Description:  I18NString.Create(e.Message),
                             Runtime:      Timestamp.Now - startTime
                         );

            }


            #region Send OnAuthorizeStartResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStartResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          LocalAuthentication,
                          ChargingLocation,
                          ChargingProduct,
                          SessionId,
                          CPOPartnerSessionId,
                          [],
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop (SessionId, LocalAuthentication, ChargingLocation = null,                                           OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given location.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingLocation?            ChargingLocation      = null,
                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                          ChargingStationOperator_Id?  OperatorId            = null,

                          DateTime?                    RequestTimestamp      = null,
                          EventTracking_Id?            EventTrackingId       = null,
                          TimeSpan?                    RequestTimeout        = null,
                          CancellationToken            CancellationToken     = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;
            RequestTimeout   ??= TimeSpan.FromSeconds(10);

            AuthStopResult? result = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStopRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          ChargingLocation,
                          SessionId,
                          CPOPartnerSessionId,
                          LocalAuthentication,
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                result = ChargingStation is not null

                             ? await ChargingStation.AuthorizeStop(
                                         SessionId,
                                         LocalAuthentication,
                                         ChargingLocation,
                                         CPOPartnerSessionId,
                                         OperatorId,

                                         RequestTimestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     )

                             : AuthStopResult.OutOfService(
                                   Id,
                                   this,
                                   SessionId:  SessionId,
                                   Runtime:    Timestamp.Now - startTime
                               );

            }
            catch (Exception e)
            {

                result = AuthStopResult.Error(
                             SessionId,
                             this,
                             SessionId,
                             I18NString.Create(e.Message),
                             Timestamp.Now - startTime
                         );

            }


            #region Send OnAuthorizeStopResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStopResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          ChargingLocation,
                          SessionId,
                          CPOPartnerSessionId,
                          LocalAuthentication,
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop

        #region RemoteStart(                  ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingProduct">The chosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingProduct?         ChargingProduct          = null,
                        ChargingReservation_Id?  ReservationId            = null,
                        ChargingSession_Id?      SessionId                = null,
                        EMobilityProvider_Id?    ProviderId               = null,
                        RemoteAuthentication?    RemoteAuthentication     = null,
                        JObject?                 AdditionalSessionInfos   = null,
                        Auth_Path?               AuthenticationPath       = null,

                        DateTime?                RequestTimestamp         = null,
                        EventTracking_Id?        EventTrackingId          = null,
                        TimeSpan?                RequestTimeout           = null,
                        CancellationToken        CancellationToken        = default)


                => RemoteStart(
                       ChargingLocation.FromEVSEId(Id),
                       ChargingProduct,
                       ReservationId,
                       SessionId,
                       ProviderId,
                       RemoteAuthentication,
                       AdditionalSessionInfos,
                       AuthenticationPath,

                       RequestTimestamp,
                       EventTrackingId,
                       RequestTimeout,
                       CancellationToken
                   );

        #endregion

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The chosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct?         ChargingProduct          = null,
                        ChargingReservation_Id?  ReservationId            = null,
                        ChargingSession_Id?      SessionId                = null,
                        EMobilityProvider_Id?    ProviderId               = null,
                        RemoteAuthentication?    RemoteAuthentication     = null,
                        JObject?                 AdditionalSessionInfos   = null,
                        Auth_Path?               AuthenticationPath       = null,

                        DateTime?                RequestTimestamp         = null,
                        EventTracking_Id?        EventTrackingId          = null,
                        TimeSpan?                RequestTimeout           = null,
                        CancellationToken        CancellationToken        = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;

            RemoteStartResult? result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStartRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          ChargingLocation,
                          RemoteAuthentication,
                          SessionId,
                          ReservationId,
                          ChargingProduct,
                          null,
                          null,
                          ProviderId,
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                if (AdminStatus.Value == EVSEAdminStatusType.Operational ||
                    AdminStatus.Value == EVSEAdminStatusType.InternalUse)
                {

                    #region Try remote EVSE

                    if (RemoteEVSE is not null)

                        result = await RemoteEVSE.RemoteStart(
                                           ChargingLocation,
                                           ChargingProduct,
                                           ReservationId,
                                           SessionId,
                                           ProviderId,
                                           RemoteAuthentication,
                                           AdditionalSessionInfos,
                                           AuthenticationPath,

                                           RequestTimestamp,
                                           EventTrackingId,
                                           RequestTimeout,
                                           CancellationToken
                                       );

                    #endregion

                    #region Try remote Charging Station

                    else if (ChargingStation?.RemoteChargingStation is not null)

                        result = await ChargingStation.RemoteChargingStation.RemoteStart(
                                           ChargingLocation,
                                           ChargingProduct,
                                           ReservationId,
                                           SessionId,
                                           ProviderId,
                                           RemoteAuthentication,
                                           AdditionalSessionInfos,
                                           AuthenticationPath,

                                           RequestTimestamp,
                                           EventTrackingId,
                                           RequestTimeout,
                                           CancellationToken
                                       );

                    #endregion

                    #region Try remote Charging Pool

                    else if (ChargingStation?.ChargingPool?.RemoteChargingPool is not null)

                        result = await ChargingStation.ChargingPool.RemoteChargingPool.RemoteStart(
                                           ChargingLocation,
                                           ChargingProduct,
                                           ReservationId,
                                           SessionId,
                                           ProviderId,
                                           RemoteAuthentication,
                                           AdditionalSessionInfos,
                                           AuthenticationPath,

                                           RequestTimestamp,
                                           EventTrackingId,
                                           RequestTimeout,
                                           CancellationToken
                                       );

                    #endregion

                    #region Try remote Charging Station Operator

                    else if (ChargingStation?.ChargingPool?.Operator?.RemoteChargingStationOperator is not null)

                        result = await ChargingStation.ChargingPool.Operator.RemoteChargingStationOperator.RemoteStart(
                                           ChargingLocation,
                                           ChargingProduct,
                                           ReservationId,
                                           SessionId,
                                           ProviderId,
                                           RemoteAuthentication,
                                           AdditionalSessionInfos,
                                           AuthenticationPath,

                                           RequestTimestamp,
                                           EventTrackingId,
                                           RequestTimeout,
                                           CancellationToken
                                       );

                    #endregion

                    #region Try EMP Roaming Provider

                    else if (ChargingStation?.ChargingPool?.Operator?.EMPRoamingProvider is not null)

                        result = await ChargingStation.ChargingPool.Operator.EMPRoamingProvider.RemoteStart(
                                           ChargingLocation,
                                           ChargingProduct,
                                           ReservationId,
                                           SessionId,
                                           ProviderId,
                                           RemoteAuthentication,
                                           AdditionalSessionInfos,
                                           AuthenticationPath,

                                           RequestTimestamp,
                                           EventTrackingId,
                                           RequestTimeout,
                                           CancellationToken
                                       );

                    #endregion


                    #region ...or send 'OFFLINE'...

                    else
                        result = RemoteStartResult.Offline(
                                     System_Id.Local,
                                     Runtime: Timestamp.Now - startTime
                                 );

                    #endregion


                    if ((result.Result == RemoteStartResultTypes.Success ||
                         result.Result == RemoteStartResultTypes.AsyncOperation) &&
                         result.Session is not null)
                    {
                        ChargingSession = result.Session;
                        result.Session.EVSE = this;
                    }

                }

                else
                    result = AdminStatus.Value switch {

                        _ => RemoteStartResult.OutOfService(
                                 System_Id.Local,
                                 Runtime: Timestamp.Now - startTime
                             )

                    };

            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(
                             System_Id.Local,
                             e.Message,
                             Runtime: Timestamp.Now - startTime
                         );
            }


            #region Send OnRemoteStartResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStartResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          ChargingLocation,
                          RemoteAuthentication,
                          SessionId,
                          ReservationId,
                          ChargingProduct,
                          null,
                          null,
                          ProviderId,
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

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
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       EMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,
                       Auth_Path?             AuthenticationPath     = null,

                       DateTime?              RequestTimestamp       = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null,
                       CancellationToken      CancellationToken      = default)
        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;

            RemoteStopResult? result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStopRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          SessionId,
                          ReservationHandling,
                          null,
                          null,
                          ProviderId,
                          RemoteAuthentication,
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                if (AdminStatus.Value == EVSEAdminStatusType.Operational ||
                    AdminStatus.Value == EVSEAdminStatusType.InternalUse)
                {

                    if (SessionId == ChargingSession?.Id)
                    {

                        #region Try remote EVSE

                        if (RemoteEVSE is not null)

                            result = await RemoteEVSE.RemoteStop(
                                               SessionId,
                                               ReservationHandling,
                                               ProviderId,
                                               RemoteAuthentication,
                                               AuthenticationPath,

                                               RequestTimestamp,
                                               EventTrackingId,
                                               RequestTimeout,
                                               CancellationToken
                                           );

                        #endregion

                        #region Try remote Charging Station

                        else if (ChargingStation?.RemoteChargingStation is not null)

                            result = await ChargingStation.RemoteChargingStation.RemoteStop(
                                               SessionId,
                                               ReservationHandling,
                                               ProviderId,
                                               RemoteAuthentication,
                                               AuthenticationPath,

                                               RequestTimestamp,
                                               EventTrackingId,
                                               RequestTimeout,
                                               CancellationToken
                                           );

                        #endregion

                        #region Try remote Charging Pool

                        else if (ChargingStation?.ChargingPool?.RemoteChargingPool is not null)

                            result = await ChargingStation.ChargingPool.RemoteChargingPool.RemoteStop(
                                               SessionId,
                                               ReservationHandling,
                                               ProviderId,
                                               RemoteAuthentication,
                                               AuthenticationPath,

                                               RequestTimestamp,
                                               EventTrackingId,
                                               RequestTimeout,
                                               CancellationToken
                                           );

                        #endregion

                        #region Try remote Charging Station Operator

                        else if (ChargingStation?.ChargingPool?.Operator?.RemoteChargingStationOperator is not null)

                            result = await ChargingStation.ChargingPool.Operator.RemoteChargingStationOperator.RemoteStop(
                                               SessionId,
                                               ReservationHandling,
                                               ProviderId,
                                               RemoteAuthentication,
                                               AuthenticationPath,

                                               RequestTimestamp,
                                               EventTrackingId,
                                               RequestTimeout,
                                               CancellationToken
                                           );

                        #endregion

                        #region Try EMP Roaming Provider

                        else if (ChargingStation?.ChargingPool?.Operator?.EMPRoamingProvider is not null)

                            result = await ChargingStation.ChargingPool.Operator.EMPRoamingProvider.RemoteStop(
                                               SessionId,
                                               ReservationHandling,
                                               ProviderId,
                                               RemoteAuthentication,
                                               AuthenticationPath,

                                               RequestTimestamp,
                                               EventTrackingId,
                                               RequestTimeout,
                                               CancellationToken
                                           );

                        #endregion


                        #region ...or send 'OFFLINE'...

                        else
                            result = RemoteStopResult.Offline(
                                         SessionId,
                                         System_Id.Local,
                                         Runtime: Timestamp.Now - startTime
                                     );

                        #endregion


                        #region In case of success...

                        if (result.Result == RemoteStopResultTypes.Success)
                        {
                            ChargingSession = null;
                        }

                        #endregion

                    }

                    result ??= RemoteStopResult.InvalidSessionId(
                                   SessionId,
                                   System_Id.Local,
                                   Runtime: Timestamp.Now - startTime
                               );

                }

                else
                    result = AdminStatus.Value switch {

                        _ => RemoteStopResult.OutOfService(
                                 SessionId,
                                 System_Id.Local,
                                 Runtime: Timestamp.Now - startTime
                             ),

                    };

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(
                             SessionId,
                             System_Id.Local,
                             e.Message,
                             Runtime: Timestamp.Now - startTime
                         );
            }


            #region Send OnRemoteStopResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStopResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
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
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region SendNewChargeDetailRecord

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
        public JObject? ToJSON(Boolean                                              Embedded                            = false,
                               InfoStatus                                           ExpandRoamingNetworkId              = InfoStatus.ShowIdOnly,
                               InfoStatus                                           ExpandChargingStationOperatorId     = InfoStatus.ShowIdOnly,
                               InfoStatus                                           ExpandChargingPoolId                = InfoStatus.ShowIdOnly,
                               InfoStatus                                           ExpandChargingStationId             = InfoStatus.ShowIdOnly,
                               InfoStatus                                           ExpandBrandIds                      = InfoStatus.ShowIdOnly,
                               InfoStatus                                           ExpandDataLicenses                  = InfoStatus.ShowIdOnly,
                               CustomJObjectSerializerDelegate<IEVSE>?              CustomEVSESerializer                = null,
                               CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null)
        {

            try
            {

                var geoLocation  = GeoLocation;

                if (geoLocation.HasValue && ChargingStation?.GeoLocation.HasValue == true && geoLocation.Value == ChargingStation.GeoLocation.Value)
                    geoLocation  = null;

                if (geoLocation.HasValue && ChargingPool?.   GeoLocation.HasValue == true && geoLocation.Value == ChargingPool.   GeoLocation.Value)
                    geoLocation  = null;

                var json         = JSONObject.Create(

                                             new JProperty("@id",               Id.ToString()),

                                       !Embedded
                                           ? new JProperty("@context",          JSONLDContext)
                                           : null,

                                       Description.IsNotNullOrEmpty()
                                           ? new JProperty("description",       Description.ToJSON())
                                           : null,

                                       Brands.SafeAny()
                                           ? ExpandBrandIds.Switch(
                                                 () => new JProperty("brandId", Brands.Select(brand => brand.Id.ToString())),
                                                 () => new JProperty("brand",   Brands.ToJSON()))
                                           : null,

                                       !Embedded && DataSource != ChargingStation?.DataSource
                                           ? new JProperty("dataSource", DataSource)
                                           : null,

                                       !Embedded && DataLicenses != ChargingStation?.DataLicenses && DataLicenses.Any()
                                           ? ExpandDataLicenses.Switch(
                                                 () => new JProperty("dataLicenseIds",             new JArray(DataLicenses.SafeSelect(dataLicense => dataLicense.Id.ToString()))),
                                                 () => new JProperty("dataLicenses",               DataLicenses.ToJSON()))
                                           : null,

                                       ExpandRoamingNetworkId != InfoStatus.Hidden && RoamingNetwork is not null
                                           ? ExpandRoamingNetworkId.Switch(
                                                 () => new JProperty("roamingNetworkId",           RoamingNetwork.Id.ToString()),
                                                 () => new JProperty("roamingNetwork",             RoamingNetwork.   ToJSON(Embedded:                          true,
                                                                                                                            ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
                                                                                                                            ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                            ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                            ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                            ExpandBrandIds:                    ExpandBrandIds,
                                                                                                                            ExpandDataLicenses:                ExpandDataLicenses)))
                                           : null,

                                       ExpandChargingStationOperatorId != InfoStatus.Hidden && Operator is not null
                                           ? ExpandChargingStationOperatorId.Switch(
                                                 () => new JProperty("chargingStationOperatorId",  Operator.Id.      ToString()),
                                                 () => new JProperty("chargingStationOperator",    Operator.         ToJSON(Embedded:                          true,
                                                                                                                            ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                            ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                            ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                            ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                            ExpandBrandIds:                    ExpandBrandIds,
                                                                                                                            ExpandDataLicenses:                ExpandDataLicenses)))
                                           : null,

                                       ExpandChargingPoolId != InfoStatus.Hidden && ChargingPool is not null
                                           ? ExpandChargingPoolId.Switch(
                                                 () => new JProperty("chargingPoolId",            ChargingPool.Id.   ToString()),
                                                 () => new JProperty("chargingPool",              ChargingPool.      ToJSON(Embedded:                          true,
                                                                                                                            ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                            ExpandChargingStationOperatorId:   InfoStatus.Hidden,
                                                                                                                            ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                            ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                            ExpandBrandIds:                    ExpandBrandIds,
                                                                                                                            ExpandDataLicenses:                ExpandDataLicenses)))
                                           : null,

                                       ExpandChargingStationId != InfoStatus.Hidden && ChargingStation is not null
                                           ? ExpandChargingStationId.Switch(
                                                 () => new JProperty("chargingStationId",         ChargingStation.Id.ToString()),
                                                 () => new JProperty("chargingStation",           ChargingStation.   ToJSON(Embedded:                          true,
                                                                                                                            ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                            ExpandChargingStationOperatorId:   InfoStatus.Hidden,
                                                                                                                            ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                            ExpandBrandIds:                    ExpandBrandIds,
                                                                                                                            ExpandDataLicenses:                ExpandDataLicenses)))
                                           : null,

                                       !Embedded && geoLocation.HasValue
                                           ? new JProperty("geoLocation",          geoLocation.Value.ToJSON(Embedded: true))
                                           : null,

                                       !Embedded && ChargingStation is not null && ChargingPool is not null && (ChargingStation.Address is not null  || ChargingPool.Address is not null)
                                           ? new JProperty("address",              (ChargingStation.Address ?? ChargingPool.Address)?.ToJSON())
                                           : null,

                                       !Embedded && ChargingStation is not null && ChargingStation.AuthenticationModes.Any()
                                           ? new JProperty("authenticationModes",   ChargingStation.AuthenticationModes.ToJSON())
                                           : null,

                                       IsFreeOfCharge
                                           ? new JProperty("isFreeOfCharge",        IsFreeOfCharge)
                                           : null,

                                       ChargingModes.SafeAny()
                                           ? new JProperty("chargingModes",         new JArray(ChargingModes.SafeSelect(chargingMode => chargingMode.ToText())))
                                           : null,

                                       new JProperty("currentType", CurrentType.ToText()),

                                       AverageVoltage.HasValue && AverageVoltage.Value.Value > 0
                                           ? new JProperty("averageVoltage",        Math.Round(AverageVoltage.Value.Value, 2))
                                           : null,

                                       MaxCurrent.    HasValue && MaxCurrent.    Value.Value > 0
                                           ? new JProperty("maxCurrent",            Math.Round(MaxCurrent.    Value.Value, 2))
                                           : null,

                                       MaxPower.      HasValue && MaxPower.      Value.Value > 0
                                           ? new JProperty("maxPower",              Math.Round(MaxPower.      Value.Value, 2))
                                           : null,

                                       MaxCapacity.   HasValue && MaxCapacity.   Value.Value > 0
                                           ? new JProperty("maxCapacity",           Math.Round(MaxCapacity.   Value.Value, 2))
                                           : null,

                                       ChargingConnectors.Count > 0
                                           ? new JProperty("socketOutlets",         new JArray(ChargingConnectors.ToJSON(Embedded: true,
                                                                                                                         CustomChargingConnectorSerializer)))
                                           : null,

                                       EnergyMeter is not null
                                           ? new JProperty("energyMeter",           EnergyMeter.ToJSON())
                                           : null,

                                       !Embedded && ChargingStation?.OpeningTimes is not null
                                           ? new JProperty("openingTimes",          ChargingStation.OpeningTimes.ToJSON())
                                           : null,

                                       CustomData.HasValues
                                           ? new JProperty("customData",            CustomData)
                                           : null

                                 );

                return CustomEVSESerializer is not null
                           ? CustomEVSESerializer(this, json)
                           : json;

            }
            catch (Exception e)
            {
                DebugX.LogException(e, "EVSE.ToJSON(...)");
            }

            return null;

        }

        #endregion


        #region IEnumerable<ChargingConnector> Members

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => ChargingConnectors.GetEnumerator();

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        public IEnumerator<IChargingConnector> GetEnumerator()
            => ChargingConnectors.GetEnumerator();

        #endregion


        #region (private) LogEvent(Logger, LogHandler, ...)

        private Task LogEvent<TDelegate>(TDelegate?                                         Logger,
                                         Func<TDelegate, Task>                              LogHandler,
                                         [CallerArgumentExpression(nameof(Logger))] String  EventName   = "",
                                         [CallerMemberName()]                       String  Command     = "")

            where TDelegate : Delegate

                => LogEvent(
                       nameof(EVSE),
                       Logger,
                       LogHandler,
                       EventName,
                       Command
                   );

        #endregion


        #region Operator overloading

        #region Operator == (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator !=(EVSE EVSE1, EVSE EVSE2)
            => !(EVSE1 == EVSE2);

        #endregion

        #region Operator <  (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <=(EVSE EVSE1, EVSE EVSE2)
            => !(EVSE1 > EVSE2);

        #endregion

        #region Operator >  (EVSE1, EVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE1">An EVSE.</param>
        /// <param name="EVSE2">Another EVSE.</param>
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >=(EVSE EVSE1, EVSE EVSE2)
            => !(EVSE1 < EVSE2);

        #endregion

        #endregion

        #region IComparable<EVSE> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSEs.
        /// </summary>
        /// <param name="Object">An EVSE to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is EVSE evse
                   ? CompareTo(evse)
                   : throw new ArgumentException("The given object is not an EVSE!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSE)

        /// <summary>
        /// Compares two EVSEs.
        /// </summary>
        /// <param name="EVSE">An EVSE to compare with.</param>
        public Int32 CompareTo(EVSE? EVSE)

            => CompareTo(EVSE as IEVSE);


        /// <summary>
        /// Compares two EVSEs.
        /// </summary>
        /// <param name="IEVSE">An EVSE to compare with.</param>
        public Int32 CompareTo(IEVSE? EVSE)
        {

            if (EVSE is null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            return Id.CompareTo(EVSE.Id);

            //ToDo: Compare more properties!

        }

        #endregion

        #endregion

        #region IEquatable<EVSE> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSEs for equality.
        /// </summary>
        /// <param name="Object">An EVSE to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSE evse &&
                   Equals(evse);

        #endregion

        #region Equals(EVSE)

        /// <summary>
        /// Compares two EVSEs for equality.
        /// </summary>
        /// <param name="EVSE">An EVSE to compare with.</param>
        public Boolean Equals(EVSE? EVSE)

            => Equals(EVSE as IEVSE);


        /// <summary>
        /// Compares two EVSEs for equality.
        /// </summary>
        /// <param name="IEVSE">An EVSE to compare with.</param>
        public Boolean Equals(IEVSE? IEVSE)

            => IEVSE is not null &&
                   Id.Equals(IEVSE.Id);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hash code of this object.
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
