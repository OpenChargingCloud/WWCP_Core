/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Diagnostics;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.WWCP.Net.IO.JSON;
using System.Linq.Expressions;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A charging station to charge an electric vehicle.
    /// </summary>
    public class ChargingStation : AEMobilityEntity<ChargingStation_Id,
                                                    ChargingStationAdminStatusTypes,
                                                    ChargingStationStatusTypes>,
                                   IEquatable <ChargingStation>,
                                   IComparable<ChargingStation>,
                                   IChargingStation
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const            String    JSONLDContext                                       = "https://open.charging.cloud/contexts/wwcp+json/chargingStation";


        private readonly        Decimal   EPSILON                                             = 0.01m;

        /// <summary>
        /// The default max size of the charging station (aggregated EVSE) status list.
        /// </summary>
        public const            UInt16    DefaultMaxChargingStationStatusScheduleSize         = 15;

        /// <summary>
        /// The default max size of the charging station admin status list.
        /// </summary>
        public const            UInt16    DefaultMaxChargingStationAdminStatusScheduleSize    = 15;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly  TimeSpan  DefaultMaxChargingStationReservationDuration        = TimeSpan.FromMinutes(30);

        #endregion

        #region Properties

        /// <summary>
        /// The roaming network of this charging station.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork?              RoamingNetwork
            => Operator?.RoamingNetwork;

        /// <summary>
        /// The charging station operator of this charging station.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator?      Operator
            => ChargingPool?.Operator;

        /// <summary>
        /// The charging station sub operator of this charging station.
        /// </summary>
        [Optional]
        public ChargingStationOperator?      SubOperator                 { get; }

        /// <summary>
        /// The charging pool.
        /// </summary>
        [InternalUseOnly]
        public ChargingPool?                 ChargingPool                { get; }

        /// <summary>
        /// An optional remote charging station.
        /// </summary>
        public IRemoteChargingStation?       RemoteChargingStation       { get; }


        /// <summary>
        /// All brands registered for this charging station.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<Brand>            Brands          { get; }

        /// <summary>
        /// The license of the charging station data.
        /// </summary>
        [Mandatory, SlowData]
        public ReactiveSet<OpenDataLicense>  DataLicenses    { get; }


        #region Address

        private Address? address;

        /// <summary>
        /// The address of this charging station.
        /// </summary>
        [Optional]
        public Address? Address
        {

            get
            {
                return address;
            }

            set
            {

                if (value is null)
                    DeleteProperty(ref address);

                else if (value != address &&
                         value != ChargingPool?.Address)
                {
                    SetProperty(ref address, value);
                }

            }

        }

        #endregion

        #region OpenStreetMap NodeId

        private String? openStreetMapNodeId;

        /// <summary>
        /// OpenStreetMap Node Id.
        /// </summary>
        [Optional]
        public String? OpenStreetMapNodeId
        {

            get
            {
                return openStreetMapNodeId;
            }

            set
            {

                if (value is null)
                    DeleteProperty(ref openStreetMapNodeId);

                else if (value != openStreetMapNodeId)
                {
                    SetProperty(ref openStreetMapNodeId, value);
                }

            }

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate? geoLocation;

        /// <summary>
        /// The geographical location of this charging station.
        /// </summary>
        [Optional]
        public GeoCoordinate? GeoLocation
        {

            get
            {
                return geoLocation;
            }

            set
            {

                if (value is null)
                    DeleteProperty(ref geoLocation);

                else if (value != geoLocation &&
                         value != ChargingPool?.GeoLocation)
                {
                    SetProperty(ref geoLocation, value);
                }

            }

        }

        #endregion

        #region EntranceAddress

        private Address? entranceAddress;

        /// <summary>
        /// The address of the entrance to this charging station.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address? EntranceAddress
        {

            get
            {
                return entranceAddress;
            }

            set
            {

                if (value is null)
                    DeleteProperty(ref entranceAddress);

                else if (value != entranceAddress &&
                         value != ChargingPool?.EntranceAddress)
                {
                    SetProperty(ref entranceAddress, value);
                }

            }

        }

        #endregion

        #region EntranceLocation

        internal GeoCoordinate? entranceLocation;

        /// <summary>
        /// The geographical location of the entrance to this charging station.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate? EntranceLocation
        {

            get
            {
                return entranceLocation;
            }

            set
            {

                if (value is null)
                    DeleteProperty(ref entranceLocation);

                else if (value != entranceLocation &&
                         value != ChargingPool?.EntranceLocation)
                {
                    SetProperty(ref entranceLocation, value);
                }

            }

        }

        #endregion

        #region ArrivalInstructions

        /// <summary>
        /// An optional (multi-language) description of how to find the charging station.
        /// </summary>
        [Optional]
        public I18NString ArrivalInstructions { get; }

        #endregion

        #region OpeningTimes

        private OpeningTimes openingTimes;

        /// <summary>
        /// The opening times of this charging station (non recursive).
        /// </summary>
        public OpeningTimes OpeningTimes
        {

            get
            {
                return openingTimes;
            }

            set
            {
                if (value != openingTimes)
                {
                    SetProperty(ref openingTimes, value);
                }
            }

        }

        #endregion

        #region ParkingSpaces

        /// <summary>
        /// Parking spaces located at the charging station.
        /// </summary>
        [Optional]
        public ReactiveSet<ParkingSpace>                ParkingSpaces           { get; }

        #endregion

        #region UIFeatures

        /// <summary>
        /// User interface features of the charging station.
        /// </summary>
        [Optional]
        public ReactiveSet<UIFeatures>                  UIFeatures              { get; }

        #endregion

        #region AuthenticationModes

        /// <summary>
        /// The authentication options an EV driver can use.
        /// </summary>
        [Mandatory]
        public ReactiveSet<AuthenticationModes>         AuthenticationModes     { get; }

        #endregion

        #region PaymentOptions

        /// <summary>
        /// The payment options an EV driver can use.
        /// </summary>
        [Mandatory]
        public ReactiveSet<PaymentOptions>              PaymentOptions          { get; }

        #endregion

        #region Accessibility

        private AccessibilityTypes? _Accessibility;

        /// <summary>
        /// The accessibility of the charging station.
        /// </summary>
        [Optional]
        public AccessibilityTypes? Accessibility
        {

            get
            {

                return _Accessibility != null
                           ? _Accessibility
                           : ChargingPool?.Accessibility;

            }

            set
            {

                if (value != _Accessibility && value != ChargingPool?.Accessibility)
                {

                    if (value == null)
                        DeleteProperty(ref _Accessibility);

                    else
                        SetProperty(ref _Accessibility, value);

                }

            }

        }

        #endregion

        #region Features

        /// <summary>
        /// Charging features of the charging station.
        /// </summary>
        [Optional]
        public ReactiveSet<Features>                    Features                { get; }

        #endregion


        /// <summary>
        /// An optional number/string printed on the outside of the charging station for visual identification.
        /// </summary>
        public String?                                  PhysicalReference       { get; }

        /// <summary>
        /// URIs of photos of this charging station.
        /// </summary>
        [Optional]
        public ReactiveSet<URL>                         PhotoURLs               { get; }


        #region HotlinePhoneNumber

        private PhoneNumber? _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the charging station operator hotline.
        /// </summary>
        [Optional]
        public PhoneNumber? HotlinePhoneNumber
        {

            get
            {
                return _HotlinePhoneNumber;
            }

            set
            {

                if (_HotlinePhoneNumber != value)
                {

                    if (value == null)
                        DeleteProperty(ref _HotlinePhoneNumber);

                    else
                        SetProperty(ref _HotlinePhoneNumber, value);

                }

            }

        }

        #endregion


        #region ExitAddress

        private Address? exitAddress;

        /// <summary>
        /// The address of the exit of this charging station.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address ExitAddress
        {

            get
            {
                return exitAddress ?? ChargingPool?.ExitAddress;
            }

            set
            {

                if (value != exitAddress && value != ChargingPool?.ExitAddress)
                {

                    if (value == null)
                        DeleteProperty(ref exitAddress);

                    else
                        SetProperty(ref exitAddress, value);

                }

            }

        }

        #endregion

        #region ExitLocation

        private GeoCoordinate? exitLocation;

        /// <summary>
        /// The geographical location of the exit of this charging station.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate? ExitLocation
        {

            get
            {

                return exitLocation.HasValue
                           ? exitLocation
                           : ChargingPool?.ExitLocation;

            }

            set
            {

                if (value != exitLocation && value != ChargingPool?.ExitLocation)
                {

                    if (value == null)
                        DeleteProperty(ref exitLocation);

                    else
                        SetProperty(ref exitLocation, value);

                }

            }

        }

        #endregion


        #region GridConnection

        private GridConnectionTypes? gridConnection;

        /// <summary>
        /// The grid connection of the charging station.
        /// </summary>
        [Optional]
        public GridConnectionTypes? GridConnection
        {

            get
            {
                return gridConnection;
            }

            set
            {

                if (value != gridConnection)
                {

                    if (value is null)
                        DeleteProperty(ref gridConnection);

                    else
                        SetProperty(ref gridConnection, value);

                }

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
                                    value);

                    else if (Math.Abs(maxCurrent.Value - value.Value) > EPSILON)
                        SetProperty(ref maxCurrent,
                                    value);

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
                                value);

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
                                    value);

                    else if (Math.Abs(maxPower.Value - value.Value) > EPSILON)
                        SetProperty(ref maxPower,
                                    value);

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
                                value);

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
                                    value);

                    else if (Math.Abs(maxCapacity.Value - value.Value) > EPSILON)
                        SetProperty(ref maxCapacity,
                                    value);

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
                                value);

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
                return energyMix ?? ChargingPool?.EnergyMix;
            }

            set
            {

                if (value != energyMix && value != ChargingPool?.EnergyMix)
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
                return energyMixPrognoses ?? ChargingPool?.EnergyMixPrognoses;
            }

            set
            {

                if (value != energyMixPrognoses && value != ChargingPool?.EnergyMixPrognoses)
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
                SetProperty(ref isFreeOfCharge,
                            value);
            }

        }

        #endregion



        #region IsHubjectCompatible

        private Boolean _IsHubjectCompatible;

        [Optional]
        public Boolean IsHubjectCompatible
        {

            get
            {
                return _IsHubjectCompatible;
            }

            set
            {

                if (_IsHubjectCompatible != value)
                    SetProperty(ref _IsHubjectCompatible, value);

            }

        }

        #endregion

        #region DynamicInfoAvailable

        private Boolean _DynamicInfoAvailable;

        [Optional]
        public Boolean DynamicInfoAvailable
        {

            get
            {
                return _DynamicInfoAvailable;
            }

            set
            {

                if (_DynamicInfoAvailable != value)
                    SetProperty(ref _DynamicInfoAvailable, value);

            }

        }

        #endregion


        #region ServiceIdentification

        private String? serviceIdentification;

        /// <summary>
        /// The internal service identification of the charging station maintained by the Charging Station Operator.
        /// </summary>
        [Optional]
        public String? ServiceIdentification
        {

            get
            {
                return serviceIdentification;
            }

            set
            {

                if (ServiceIdentification != value)
                    SetProperty(ref serviceIdentification, value);

            }

        }

        #endregion

        #region ModelCode

        private String? modelCode;

        /// <summary>
        /// The internal model code of the charging station maintained by the Charging Station Operator.
        /// </summary>
        [Optional]
        public String? ModelCode
        {

            get
            {
                return modelCode;
            }

            set
            {

                if (ModelCode != value)
                    SetProperty(ref modelCode, value);

            }

        }

        #endregion

        #region HubjectStationId

        private String? hubjectStationId;

        [Optional]
        public String? HubjectStationId
        {

            get
            {
                return hubjectStationId;
            }

            set
            {

                if (HubjectStationId != value)
                    SetProperty<String>(ref hubjectStationId, value);

            }

        }

        #endregion


        #region StatusAggregationDelegate

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated EVSEs.
        /// </summary>
        public Func<EVSEStatusReport, ChargingStationStatusTypes>? StatusAggregationDelegate { get; set; }

        #endregion

        #endregion

        #region Events

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnChargingStationDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnChargingStationStatusChangedDelegate?       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate?  OnAdminStatusChanged;

        #endregion

        #endregion

        #region Constructor(s)

        #region ChargingStation(Id, ...)

        /// <summary>
        /// Create a new charging station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station pool.</param>
        /// <param name="Configurator">A delegate to configure the newly created charging station.</param>
        /// <param name="RemoteChargingStationCreator">A delegate to attach a remote charging station.</param>
        /// <param name="InitialAdminStatus">An optional initial admin status of the EVSE.</param>
        /// <param name="InitialStatus">An optional initial status of the EVSE.</param>
        /// <param name="MaxAdminStatusListSize">An optional max length of the admin staus list.</param>
        /// <param name="MaxStatusListSize">An optional max length of the staus list.</param>
        public ChargingStation(ChargingStation_Id                             Id,
                               I18NString?                                    Name                           = null,
                               I18NString?                                    Description                    = null,

                               Address?                                       Address                        = null,
                               GeoCoordinate?                                 GeoLocation                    = null,

                               Action<ChargingStation>?                       Configurator                   = null,
                               RemoteChargingStationCreatorDelegate?          RemoteChargingStationCreator   = null,
                               Timestamped<ChargingStationAdminStatusTypes>?  InitialAdminStatus             = null,
                               Timestamped<ChargingStationStatusTypes>?       InitialStatus                  = null,
                               UInt16?                                        MaxAdminStatusListSize         = null,
                               UInt16?                                        MaxStatusListSize              = null)

            : this(Id,
                   null,
                   Name,
                   Description,

                   Address,
                   GeoLocation,

                   Configurator,
                   RemoteChargingStationCreator,
                   InitialAdminStatus,
                   InitialStatus,
                   MaxAdminStatusListSize,
                   MaxStatusListSize)

        { }

        #endregion

        #region ChargingStation(Id, ChargingPool, ...)

        /// <summary>
        /// Create a new charging station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station pool.</param>
        /// <param name="ChargingPool">The parent charging pool.</param>
        /// <param name="Configurator">A delegate to configure the newly created charging station.</param>
        /// <param name="RemoteChargingStationCreator">A delegate to attach a remote charging station.</param>
        /// <param name="InitialAdminStatus">An optional initial admin status of the EVSE.</param>
        /// <param name="InitialStatus">An optional initial status of the EVSE.</param>
        /// <param name="MaxAdminStatusScheduleSize">An optional max length of the admin staus list.</param>
        /// <param name="MaxStatusScheduleSize">An optional max length of the staus list.</param>
        public ChargingStation(ChargingStation_Id                             Id,
                               ChargingPool?                                  ChargingPool                   = null,

                               I18NString?                                    Name                           = null,
                               I18NString?                                    Description                    = null,
                               Address?                                       Address                        = null,
                               GeoCoordinate?                                 GeoLocation                    = null,

                               Action<ChargingStation>?                       Configurator                   = null,
                               RemoteChargingStationCreatorDelegate?          RemoteChargingStationCreator   = null,
                               Timestamped<ChargingStationAdminStatusTypes>?  InitialAdminStatus             = null,
                               Timestamped<ChargingStationStatusTypes>?       InitialStatus                  = null,
                               UInt16?                                        MaxAdminStatusScheduleSize     = null,
                               UInt16?                                        MaxStatusScheduleSize          = null,

                               String?                                        DataSource                     = null,
                               DateTime?                                      LastChange                     = null,

                               JObject?                                       CustomData                     = null,
                               UserDefinedDictionary?                         InternalData                   = null)

            : base(Id,
                   Name,
                   Description,
                   InitialAdminStatus         ?? ChargingStationAdminStatusTypes.Operational,
                   InitialStatus              ?? ChargingStationStatusTypes.     Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxChargingStationAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxChargingStationStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Init data and properties

            this.ChargingPool                        = ChargingPool;

            this.Address                             = Address;
            this.GeoLocation                         = GeoLocation;

            this.Brands                              = new ReactiveSet<Brand>();
            this.Brands.OnSetChanged                += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("Brands",
                                oldItems,
                                newItems);

            };

            this.DataLicenses                        = new ReactiveSet<OpenDataLicense>();
            this.DataLicenses.OnSetChanged          += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("DataLicenses",
                                oldItems,
                                newItems);

            };


            this.UIFeatures                          = new ReactiveSet<UIFeatures>();
            this.UIFeatures.OnSetChanged            += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("UIFeatures",
                                oldItems,
                                newItems);

            };

            this.AuthenticationModes                 = new ReactiveSet<AuthenticationModes>();
            this.AuthenticationModes.OnSetChanged   += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("AuthenticationModes",
                                oldItems,
                                newItems);

            };

            this.PaymentOptions                      = new ReactiveSet<PaymentOptions>();
            this.PaymentOptions.OnSetChanged        += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("PaymentOptions",
                                oldItems,
                                newItems);

            };

            this.Features                            = new ReactiveSet<Features>();
            this.Features.OnSetChanged              += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("Features",
                                oldItems,
                                newItems);

            };

            this.ParkingSpaces                       = new ReactiveSet<ParkingSpace>();
            this.ParkingSpaces.OnSetChanged         += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("ParkingSpaces",
                                oldItems,
                                newItems);

            };

            this.ParkingSpaces                       = new ReactiveSet<ParkingSpace>();
            this.ParkingSpaces.OnSetChanged         += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("ParkingSpaces",
                                oldItems,
                                newItems);

            };

            this.PhotoURLs                           = new ReactiveSet<URL>();
            this.PhotoURLs.OnSetChanged             += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("PhotoURLs",
                                oldItems,
                                newItems);

            };

            this.ArrivalInstructions = ArrivalInstructions ?? I18NString.Empty;

            this.ArrivalInstructions.OnPropertyChanged += (timestamp,
                                                           eventTrackingId,
                                                           sender,
                                                           propertyName,
                                                           newValue,
                                                           oldValue,
                                                           dataSource) =>
            {

                PropertyChanged("ArrivalInstructions",
                                newValue,
                                oldValue,
                                dataSource,
                                eventTrackingId);

                return Task.CompletedTask;

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

            this.HotlinePhoneNumber                = HotlinePhoneNumber;

            this.evses                             = new EntityHashSet<ChargingStation, EVSE_Id, IEVSE>(this);
            //this.evses.OnSetChanged               += (timestamp, reactiveSet, newItems, oldItems) =>
            //{

            //    PropertyChanged("EVSEs",
            //                    oldItems,
            //                    newItems);

            //};

            this.openingTimes                = OpeningTimes.Open24Hours;
            this.evses                       = new EntityHashSet<ChargingStation, EVSE_Id,  IEVSE> (this);

            #endregion

            #region Init events

            // ChargingStation events
            this.EVSEAddition             = new VotingNotificator<DateTime, IChargingStation, IEVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval              = new VotingNotificator<DateTime, IChargingStation, IEVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events

            #endregion

            Configurator?.Invoke(this);

            #region Link events

            this.OnPropertyChanged += UpdateData;

            this.adminStatusSchedule.OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                          => UpdateAdminStatus(timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            this.statusSchedule.     OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                          => UpdateStatus     (timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            #endregion

            this.RemoteChargingStation = RemoteChargingStationCreator?.Invoke(this);

        }

        #endregion

        #endregion


        #region Data/(Admin-)Status

        #region (internal) UpdateData       (Timestamp, EventTrackingId, Sender, PropertyName, NewValue, OldValue = null, DataSource = null)

        /// <summary>
        /// Update the static data.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="Sender">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The optional old value of the changed property.</param>
        /// <param name="DataSource">An optional data source or context for the charging station data update.</param>
        internal async Task UpdateData(DateTime          Timestamp,
                                       EventTracking_Id  EventTrackingId,
                                       Object            Sender,
                                       String            PropertyName,
                                       Object?           NewValue,
                                       Object?           OldValue     = null,
                                       Context?          DataSource   = null)
        {

            var onDataChanged = OnDataChanged;
            if (onDataChanged is not null)
                await onDataChanged(Timestamp,
                                    EventTrackingId,
                                    Sender as ChargingStation,
                                    PropertyName,
                                    NewValue,
                                    OldValue,
                                    DataSource);

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus, DataSource = null)

        /// <summary>
        /// Update the current admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        /// <param name="OldStatus">The optional old charging station admin status.</param>
        /// <param name="DataSource">An optional data source or context for the charging station admin status update.</param>
        internal async Task UpdateAdminStatus(DateTime                                       Timestamp,
                                              EventTracking_Id                               EventTrackingId,
                                              Timestamped<ChargingStationAdminStatusTypes>   NewStatus,
                                              Timestamped<ChargingStationAdminStatusTypes>?  OldStatus    = null,
                                              Context?                                       DataSource   = null)
        {

            var onAdminStatusChanged = OnAdminStatusChanged;
            if (onAdminStatusChanged is not null)
                await onAdminStatusChanged(Timestamp,
                                           EventTrackingId,
                                           this,
                                           NewStatus,
                                           OldStatus,
                                           DataSource);

        }

        #endregion

        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus, DataSource = null)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="NewStatus">The new charging station status.</param>
        /// <param name="OldStatus">The optional old charging station status.</param>
        /// <param name="DataSource">An optional data source or context for the charging station status update.</param>
        internal async Task UpdateStatus(DateTime                                  Timestamp,
                                         EventTracking_Id                          EventTrackingId,
                                         Timestamped<ChargingStationStatusTypes>   NewStatus,
                                         Timestamped<ChargingStationStatusTypes>?  OldStatus    = null,
                                         Context?                                  DataSource   = null)
        {

            var onAggregatedStatusChanged = OnStatusChanged;
            if (onAggregatedStatusChanged is not null)
                await onAggregatedStatusChanged(Timestamp,
                                                EventTrackingId,
                                                this,
                                                NewStatus,
                                                OldStatus,
                                                DataSource);

        }

        #endregion

        #endregion

        #region EVSEs

        #region EVSEAddition

        internal readonly IVotingNotificator<DateTime, IChargingStation, IEVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, IChargingStation, IEVSE, Boolean> OnEVSEAddition

            => EVSEAddition;

        #endregion

        #region EVSEs

        private readonly EntityHashSet<ChargingStation, EVSE_Id, IEVSE> evses;

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging station.
        /// </summary>
        public IEnumerable<IEVSE> EVSEs
        {
            get
            {
                lock (evses)
                {
                    return evses.ToArray();
                }
            }
        }

        #endregion

        #region EVSEIds                (IncludeEVSEs = null)

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment (EVSEs)
        /// present within this charging station.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate?  IncludeEVSEs   = null)
        {

            IncludeEVSEs ??= (evse => true);

            return evses.Where (evse => IncludeEVSEs(evse)).
                         Select(evse => evse.Id);

        }

        #endregion

        #region EVSEAdminStatus        (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate? IncludeEVSEs = null)
        {

            IncludeEVSEs ??= (evse => true);

            return evses.Where (evse => IncludeEVSEs?.Invoke(evse) ?? true).
                         Select(evse => new EVSEAdminStatus(evse.Id,
                                                            evse.AdminStatus));

        }

        #endregion

        #region EVSEAdminStatusSchedule(IncludeEVSEs = null, TimestampFilter  = null, StatusFilter = null, Skip = null, Take = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>>

            EVSEAdminStatusSchedule(IncludeEVSEDelegate?                  IncludeEVSEs      = null,
                                    Func<DateTime,             Boolean>?  TimestampFilter   = null,
                                    Func<EVSEAdminStatusTypes, Boolean>?  StatusFilter      = null,
                                    UInt64?                               Skip              = null,
                                    UInt64?                               Take              = null)

        {

            IncludeEVSEs ??= (evse => true);

            return EVSEs.Where (evse => IncludeEVSEs(evse)).
                         Select(evse => new Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>(
                                            evse.Id,
                                            evse.AdminStatusSchedule(TimestampFilter,
                                                                     StatusFilter,
                                                                     Skip,
                                                                     Take)));

        }

        #endregion

        #region EVSEStatus             (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate? IncludeEVSEs = null)
        {

            IncludeEVSEs ??= (evse => true);

            return evses.Where (evse => IncludeEVSEs?.Invoke(evse) ?? true).
                         Select(evse => new EVSEStatus(evse.Id,
                                                       evse.Status));

        }

        #endregion

        #region EVSEStatusSchedule     (IncludeEVSEs = null, TimestampFilter  = null, StatusFilter = null, Skip = null, Take = null)

        /// <summary>
        /// Return the status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>>

            EVSEStatusSchedule(IncludeEVSEDelegate?             IncludeEVSEs      = null,
                               Func<DateTime,        Boolean>?  TimestampFilter   = null,
                               Func<EVSEStatusTypes, Boolean>?  StatusFilter      = null,
                               UInt64?                          Skip              = null,
                               UInt64?                          Take              = null)

        {

            IncludeEVSEs ??= (evse => true);

            return EVSEs.Where (evse => IncludeEVSEs(evse)).
                         Select(evse => new Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>(
                                            evse.Id,
                                            evse.StatusSchedule(TimestampFilter,
                                                                StatusFilter,
                                                                Skip,
                                                                Take)));

        }

        #endregion


        #region CreateEVSE(Id, Configurator = null, RemoteEVSECreator = null, ...)

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new EVSE.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE after its creation.</param>
        /// <param name="RemoteEVSECreator">An optional delegate to configure a new remote EVSE after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the EVSE.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        public async Task<AddEVSEResult> CreateEVSE(EVSE_Id                             Id,
                                                    I18NString?                         Name                         = null,
                                                    I18NString?                         Description                  = null,

                                                    Timestamped<EVSEAdminStatusTypes>?  InitialAdminStatus           = null,
                                                    Timestamped<EVSEStatusTypes>?       InitialStatus                = null,
                                                    UInt16?                             MaxAdminStatusScheduleSize   = null,
                                                    UInt16?                             MaxStatusScheduleSize        = null,

                                                    IEnumerable<URL>?                   PhotoURLs                    = null,
                                                    IEnumerable<Brand>?                 Brands                       = null,
                                                    IEnumerable<OpenDataLicense>?       OpenDataLicenses             = null,
                                                    IEnumerable<ChargingModes>?         ChargingModes                = null,
                                                    IEnumerable<ChargingTariff>?        ChargingTariffs              = null,
                                                    CurrentTypes?                       CurrentType                  = null,
                                                    Decimal?                            AverageVoltage               = null,
                                                    Timestamped<Decimal>?               AverageVoltageRealTime       = null,
                                                    IEnumerable<Timestamped<Decimal>>?  AverageVoltagePrognoses      = null,
                                                    Decimal?                            MaxCurrent                   = null,
                                                    Timestamped<Decimal>?               MaxCurrentRealTime           = null,
                                                    IEnumerable<Timestamped<Decimal>>?  MaxCurrentPrognoses          = null,
                                                    Decimal?                            MaxPower                     = null,
                                                    Timestamped<Decimal>?               MaxPowerRealTime             = null,
                                                    IEnumerable<Timestamped<Decimal>>?  MaxPowerPrognoses            = null,
                                                    Decimal?                            MaxCapacity                  = null,
                                                    Timestamped<Decimal>?               MaxCapacityRealTime          = null,
                                                    IEnumerable<Timestamped<Decimal>>?  MaxCapacityPrognoses         = null,
                                                    EnergyMix?                          EnergyMix                    = null,
                                                    Timestamped<EnergyMix>?             EnergyMixRealTime            = null,
                                                    EnergyMixPrognosis?                 EnergyMixPrognoses           = null,
                                                    EnergyMeter?                        EnergyMeter                  = null,
                                                    Boolean?                            IsFreeOfCharge               = null,
                                                    IEnumerable<IChargingConnector>?    SocketOutlets                = null,

                                                    ChargingSession?                    ChargingSession              = null,
                                                    DateTime?                           LastStatusUpdate             = null,
                                                    String?                             DataSource                   = null,
                                                    DateTime?                           LastChange                   = null,
                                                    JObject?                            CustomData                   = null,
                                                    UserDefinedDictionary?              InternalData                 = null,

                                                    Action<IEVSE>?                      Configurator                 = null,
                                                    RemoteEVSECreatorDelegate?          RemoteEVSECreator            = null,
                                                    Action<IEVSE>?                      OnSuccess                    = null,
                                                    Action<IChargingStation, EVSE_Id>?  OnError                      = null)
        {

            //lock (evses)
            //{

                #region Initial checks

                if (evses.Any(evse => evse.Id == Id))
                {
                    if (OnError is null)
                        throw new EVSEAlreadyExistsInStation(this, Id);
                    else
                        OnError?.Invoke(this, Id);
                }

                if (Operator.Id != Id.OperatorId)
                    return null;
                    //throw new InvalidChargingStationOperatorId(Id.OperatorId);

                #endregion

                var now   = Timestamp.Now;

                var evse  = new EVSE(
                                Id,
                                this,   // ChargingStation
                                Name,
                                Description,

                                InitialAdminStatus,
                                InitialStatus,
                                MaxAdminStatusScheduleSize,
                                MaxStatusScheduleSize,

                                PhotoURLs,
                                Brands,
                                OpenDataLicenses,
                                ChargingModes,
                                ChargingTariffs,
                                CurrentType,
                                AverageVoltage,
                                AverageVoltageRealTime,
                                AverageVoltagePrognoses,
                                MaxCurrent,
                                MaxCurrentRealTime,
                                MaxCurrentPrognoses,
                                MaxPower,
                                MaxPowerRealTime,
                                MaxPowerPrognoses,
                                MaxCapacity,
                                MaxCapacityRealTime,
                                MaxCapacityPrognoses,
                                EnergyMix,
                                EnergyMixRealTime,
                                EnergyMixPrognoses,
                                EnergyMeter,
                                IsFreeOfCharge,
                                SocketOutlets,

                                ChargingSession,
                                LastStatusUpdate,
                                DataSource,
                                LastChange,

                                Configurator,
                                RemoteEVSECreator,

                                CustomData,
                                InternalData

                            );

                if (EVSEAddition.SendVoting(now, this, evse) &&
                    evses.TryAdd(evse))
                {

                    evse.OnDataChanged           += UpdateEVSEData;
                    evse.OnStatusChanged         += UpdateEVSEStatus;
                    evse.OnAdminStatusChanged    += UpdateEVSEAdminStatus;

                    evse.OnNewReservation        += SendNewReservation;
                    evse.OnReservationCanceled   += SendReservationCanceled;
                    evse.OnNewChargingSession    += SendNewChargingSession;
                    evse.OnNewChargeDetailRecord += SendNewChargeDetailRecord;

                    //UpdateEVSEStatus(Now,
                    //                 EventTracking_Id.New,
                    //                 _EVSE,
                    //                 new Timestamped<EVSEStatusTypes>(Now, EVSEStatusTypes.Unspecified),
                    //                 _EVSE.Status).Wait();

                    if (RemoteChargingStation is not null)
                    {

                        if (evse.RemoteEVSE is not null)
                            RemoteChargingStation.AddEVSE(evse.RemoteEVSE);

                        OnAdminStatusChanged               += (Timestamp, EventTrackingId, station, newstatus, oldstatus, dataSource) => { adminStatusSchedule.Insert(newstatus, dataSource); return Task.CompletedTask; };
                        OnStatusChanged                    += (Timestamp, EventTrackingId, station, newstatus, oldstatus, dataSource) => { statusSchedule.     Insert(newstatus, dataSource); return Task.CompletedTask; };

                        this.RemoteChargingStation.OnAdminStatusChanged    += (timestamp, eventTrackingId, chargingStation, newstatus, oldstatus, dataSource) => { adminStatusSchedule.Insert(newstatus, dataSource); return Task.CompletedTask; };
                        this.RemoteChargingStation.OnStatusChanged         += (timestamp, eventTrackingId, chargingStation, newstatus, oldstatus, dataSource) => { statusSchedule.     Insert(newstatus, dataSource); return Task.CompletedTask; };

                    //RemoteConfigurator?.Invoke(_EVSE.RemoteEVSE);

                }

                    OnSuccess?.Invoke(evse);
                    EVSEAddition.SendNotification(now, this, evse);

                    return AddEVSEResult.Success(evse, EventTracking_Id.New);

                }

                Debug.WriteLine("EVSE '" + Id + "' was not created!");
                return null;

            //}

        }

        #endregion

        #region CreateOrUpdateEVSE(Id, Configurator = null, RemoteEVSECreator = null, ...)

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new EVSE.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new EVSE after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the EVSE failed.</param>
        public async Task<AddOrUpdateEVSEResult> CreateOrUpdateEVSE(EVSE_Id                             Id,
                                                                    I18NString?                         Name                         = null,
                                                                    I18NString?                         Description                  = null,

                                                                    Timestamped<EVSEAdminStatusTypes>?  InitialAdminStatus           = null,
                                                                    Timestamped<EVSEStatusTypes>?       InitialStatus                = null,
                                                                    UInt16?                             MaxAdminStatusScheduleSize   = null,
                                                                    UInt16?                             MaxStatusScheduleSize        = null,

                                                                    IEnumerable<URL>?                   PhotoURLs                    = null,
                                                                    IEnumerable<Brand>?                 Brands                       = null,
                                                                    IEnumerable<OpenDataLicense>?       OpenDataLicenses             = null,
                                                                    IEnumerable<ChargingModes>?         ChargingModes                = null,
                                                                    IEnumerable<ChargingTariff>?        ChargingTariffs              = null,
                                                                    CurrentTypes?                       CurrentType                  = null,
                                                                    Decimal?                            AverageVoltage               = null,
                                                                    Timestamped<Decimal>?               AverageVoltageRealTime       = null,
                                                                    IEnumerable<Timestamped<Decimal>>?  AverageVoltagePrognoses      = null,
                                                                    Decimal?                            MaxCurrent                   = null,
                                                                    Timestamped<Decimal>?               MaxCurrentRealTime           = null,
                                                                    IEnumerable<Timestamped<Decimal>>?  MaxCurrentPrognoses          = null,
                                                                    Decimal?                            MaxPower                     = null,
                                                                    Timestamped<Decimal>?               MaxPowerRealTime             = null,
                                                                    IEnumerable<Timestamped<Decimal>>?  MaxPowerPrognoses            = null,
                                                                    Decimal?                            MaxCapacity                  = null,
                                                                    Timestamped<Decimal>?               MaxCapacityRealTime          = null,
                                                                    IEnumerable<Timestamped<Decimal>>?  MaxCapacityPrognoses         = null,
                                                                    EnergyMix?                          EnergyMix                    = null,
                                                                    Timestamped<EnergyMix>?             EnergyMixRealTime            = null,
                                                                    EnergyMixPrognosis?                 EnergyMixPrognoses           = null,
                                                                    EnergyMeter?                        EnergyMeter                  = null,
                                                                    Boolean?                            IsFreeOfCharge               = null,
                                                                    IEnumerable<IChargingConnector>?    SocketOutlets                = null,

                                                                    ChargingSession?                    ChargingSession              = null,
                                                                    DateTime?                           LastStatusUpdate             = null,
                                                                    String?                             DataSource                   = null,
                                                                    DateTime?                           LastChange                   = null,
                                                                    JObject?                            CustomData                   = null,
                                                                    UserDefinedDictionary?              InternalData                 = null,

                                                                    Action<IEVSE>?                      Configurator                 = null,
                                                                    RemoteEVSECreatorDelegate?          RemoteEVSECreator            = null,
                                                                    Action<IEVSE>?                      OnSuccess                    = null,
                                                                    Action<IChargingStation, EVSE_Id>?  OnError                      = null)
        {

            #region Initial checks

            if (Operator.Id != Id.OperatorId)
                return null;
                //throw new InvalidChargingStationOperatorId(this,
                //                                Id.OperatorId);

            InitialAdminStatus ??= new Timestamped<EVSEAdminStatusTypes>(EVSEAdminStatusTypes.Operational);
            InitialStatus      ??= new Timestamped<EVSEStatusTypes>     (EVSEStatusTypes.Available);

            #endregion

            //lock (evses)
            //{

                #region If the EVSE identification is new/unknown: Call CreateEVSE(...)

                if (!evses.ContainsId(Id))
                {

                    var result = await CreateEVSE(Id,
                                                  Name,
                                                  Description,

                                                  InitialAdminStatus,
                                                  InitialStatus,
                                                  MaxAdminStatusScheduleSize,
                                                  MaxStatusScheduleSize,

                                                  PhotoURLs,
                                                  Brands,
                                                  OpenDataLicenses,
                                                  ChargingModes,
                                                  ChargingTariffs,
                                                  CurrentType,
                                                  AverageVoltage,
                                                  AverageVoltageRealTime,
                                                  AverageVoltagePrognoses,
                                                  MaxCurrent,
                                                  MaxCurrentRealTime,
                                                  MaxCurrentPrognoses,
                                                  MaxPower,
                                                  MaxPowerRealTime,
                                                  MaxPowerPrognoses,
                                                  MaxCapacity,
                                                  MaxCapacityRealTime,
                                                  MaxCapacityPrognoses,
                                                  EnergyMix,
                                                  EnergyMixRealTime,
                                                  EnergyMixPrognoses,
                                                  EnergyMeter,
                                                  IsFreeOfCharge,
                                                  SocketOutlets,

                                                  ChargingSession,
                                                  LastStatusUpdate,
                                                  DataSource,
                                                  LastChange,

                                                  CustomData,
                                                  InternalData,

                                                  Configurator,
                                                  RemoteEVSECreator,

                                                  OnSuccess,
                                                  OnError);

                    return AddOrUpdateEVSEResult.Success(
                               result.EVSE,
                               social.OpenData.UsersAPI.AddedOrUpdated.Add,
                               EventTracking_Id.New
                           );

                }

                #endregion


                var existingEVSE = evses.GetById(Id);

                // Merge existing EVSE with new EVSE data...
                if (existingEVSE is not null)
                {

                    var updatedEVSE  = existingEVSE.UpdateWith(
                                           new EVSE(Id,
                                                    this,
                                                    Name,
                                                    Description,

                                                    InitialAdminStatus,
                                                    InitialStatus,
                                                    MaxAdminStatusScheduleSize,
                                                    MaxStatusScheduleSize,

                                                    PhotoURLs,
                                                    Brands,
                                                    OpenDataLicenses,
                                                    ChargingModes,
                                                    ChargingTariffs,
                                                    CurrentType,
                                                    AverageVoltage,
                                                    AverageVoltageRealTime,
                                                    AverageVoltagePrognoses,
                                                    MaxCurrent,
                                                    MaxCurrentRealTime,
                                                    MaxCurrentPrognoses,
                                                    MaxPower,
                                                    MaxPowerRealTime,
                                                    MaxPowerPrognoses,
                                                    MaxCapacity,
                                                    MaxCapacityRealTime,
                                                    MaxCapacityPrognoses,
                                                    EnergyMix,
                                                    EnergyMixRealTime,
                                                    EnergyMixPrognoses,
                                                    EnergyMeter,
                                                    IsFreeOfCharge,
                                                    SocketOutlets,

                                                    ChargingSession,
                                                    LastStatusUpdate,
                                                    DataSource,
                                                    LastChange,

                                                    Configurator,
                                                    RemoteEVSECreator,

                                                    CustomData,
                                                    InternalData)
                                           );

                    return AddOrUpdateEVSEResult.Success(
                               updatedEVSE,
                               social.OpenData.UsersAPI.AddedOrUpdated.Update,
                               EventTracking_Id.New
                           );

                }

            //}

            return AddOrUpdateEVSEResult.Failed(
                       Id,
                       EventTracking_Id.New,
                       "",
                       this
                   );

        }

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the charging station.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(IEVSE EVSE)

            => evses.Contains(EVSE);

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the charging station.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)

            => evses.ContainsId(EVSEId);

        #endregion

        #region GetEVSEById(EVSEId)

        public IEVSE? GetEVSEById(EVSE_Id EVSEId)

            => evses.GetById(EVSEId);

        #endregion

        #region TryGetEVSEById(EVSEId, out EVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE? EVSE)

            => evses.TryGet(EVSEId, out EVSE);

        #endregion

        #region RemoveEVSE(EVSEId)

        public IEVSE? RemoveEVSE(EVSE_Id EVSEId)

            => evses.Remove(EVSEId);

        #endregion

        #region TryRemoveEVSE(EVSEId, out EVSE)

        public Boolean TryRemoveEVSE(EVSE_Id EVSEId, out IEVSE? EVSE)

            => evses.TryRemove(EVSEId, out EVSE);

        #endregion


        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate?         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate?       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate?  OnEVSEAdminStatusChanged;

        #endregion


        #region (internal) UpdateEVSEData       (Timestamp, EventTrackingId, EVSE, NewValue,       OldValue       = null, DataSource = null)

        /// <summary>
        /// Update the static data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The optional old value of the changed property.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE data update.</param>
        internal async Task UpdateEVSEData(DateTime          Timestamp,
                                           EventTracking_Id  EventTrackingId,
                                           IEVSE             EVSE,
                                           String            PropertyName,
                                           Object?           NewValue,
                                           Object?           OldValue     = null,
                                           Context?          DataSource   = null)
        {

            try
            {

                var onEVSEDataChanged = OnEVSEDataChanged;
                if (onEVSEDataChanged is not null)
                    await onEVSEDataChanged(Timestamp,
                                            EventTrackingId,
                                            EVSE,
                                            PropertyName,
                                            NewValue,
                                            OldValue,
                                            DataSource);

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"ChargingStation '{Id}'.UpdateEVSEData of EVSE '{EVSE.Id}' property '{PropertyName}' from '{OldValue?.ToString() ?? "-"}' to '{NewValue?.ToString() ?? "-"}'");
            }

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EventTrackingId, EVSE, NewAdminStatus, OldAdminStatus = null, DataSource = null)

        /// <summary>
        /// Update the current admin status of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="NewAdminStatus">The new EVSE admin status.</param>
        /// <param name="OldAdminStatus">The optional old EVSE admin status.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE admin status update.</param>
        internal async Task UpdateEVSEAdminStatus(DateTime                            Timestamp,
                                                  EventTracking_Id                    EventTrackingId,
                                                  IEVSE                               EVSE,
                                                  Timestamped<EVSEAdminStatusTypes>   NewAdminStatus,
                                                  Timestamped<EVSEAdminStatusTypes>?  OldAdminStatus   = null,
                                                  Context?                            DataSource       = null)
        {

            try
            {

                var onEVSEAdminStatusChanged = OnEVSEAdminStatusChanged;
                if (onEVSEAdminStatusChanged is not null)
                    await onEVSEAdminStatusChanged(Timestamp,
                                                   EventTrackingId,
                                                   EVSE,
                                                   NewAdminStatus,
                                                   OldAdminStatus,
                                                   DataSource);

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"ChargingStation '{Id}'.UpdateEVSEAdminStatus of EVSE '{EVSE.Id}' from '{OldAdminStatus?.ToString() ?? "-"}' to '{NewAdminStatus}'");
            }

        }

        #endregion

        #region (internal) UpdateEVSEStatus     (Timestamp, EventTrackingId, EVSE, NewStatus,      OldStatus      = null, DataSource = null)

        /// <summary>
        /// Update the current status of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        /// <param name="OldStatus">The optional old EVSE status.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE status update.</param>
        internal async Task UpdateEVSEStatus(DateTime                       Timestamp,
                                             EventTracking_Id               EventTrackingId,
                                             IEVSE                          EVSE,
                                             Timestamped<EVSEStatusTypes>   NewStatus,
                                             Timestamped<EVSEStatusTypes>?  OldStatus    = null,
                                             Context?                       DataSource   = null)
        {

            try
            {

                var onEVSEStatusChanged = OnEVSEStatusChanged;
                if (onEVSEStatusChanged is not null)
                    await onEVSEStatusChanged(Timestamp,
                                              EventTrackingId,
                                              EVSE,
                                              NewStatus,
                                              OldStatus,
                                              DataSource);

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"ChargingStation '{Id}'.UpdateEVSEStatus of EVSE '{EVSE.Id}' from '{OldStatus}' to '{NewStatus}'");
            }

        }

        #endregion


        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, IChargingStation, IEVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, IChargingStation, IEVSE, Boolean> OnEVSERemoval

            => EVSERemoval;

        #endregion

        #region IEnumerable<IEVSE> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => evses.GetEnumerator();

        public IEnumerator<IEVSE> GetEnumerator()
            => evses.GetEnumerator();

        #endregion

        #endregion


        #region Reservations

        #region Data

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
            => RoamingNetwork is not null
                   ? RoamingNetwork.ReservationsStore.
                         Where     (reservations => reservations.First().ChargingStationId == Id).
                         SelectMany(reservations => reservations)
                   : Array.Empty<ChargingReservation>();

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservationCollection ChargingReservations)
            => RoamingNetwork.ReservationsStore.TryGet(ReservationId, out ChargingReservations);


        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation? ChargingReservation)
        {

            if (RoamingNetwork.ReservationsStore.TryGet(ReservationId, out var chargingReservations))
            {
                ChargingReservation = chargingReservations.FirstOrDefault();
                return true;
            }

            ChargingReservation = null;
            return false;

        }

        #endregion

        #region Events

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

        #region Reserve(                                           StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at this charging station.
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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<ReservationResult>

            Reserve(DateTime?                          StartTime              = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    CancellationToken?                 CancellationToken      = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null)


                => Reserve(ChargingLocation.FromChargingStationId(Id),
                           ChargingReservationLevel.ChargingStation,
                           StartTime,
                           Duration,
                           ReservationId,
                           LinkedReservationId,
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
        /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
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

            Reserve(ChargingLocation                   ChargingLocation,
                    ChargingReservationLevel           ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                          ReservationStartTime   = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    CancellationToken?                 CancellationToken      = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null)

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
                DebugX.LogException(e, nameof(ChargingStation) + "." + nameof(OnReserveRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.ChargingStationId.HasValue && ChargingLocation.ChargingStationId.Value != Id)
                    result = ReservationResult.UnknownLocation;

                else if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                         AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
                {

                    var EVSEId = ChargingLocation.EVSEId;

                    //if ()

                    if (RemoteChargingStation != null)
                    {

                        result = await RemoteChargingStation.
                                           Reserve(ChargingLocation,
                                                   ReservationLevel,
                                                   ReservationStartTime,
                                                   Duration,
                                                   ReservationId,
                                                   LinkedReservationId,
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
                    result = AdminStatus.Value switch {
                        _ => ReservationResult.OutOfService,
                    };
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
                DebugX.LogException(e, nameof(ChargingStation) + "." + nameof(OnReserveResponse));
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
                              EventTracking_Id?                      EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;


            ChargingReservation?     canceledReservation   = null;
            CancelReservationResult? result                = null;

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
                DebugX.LogException(e, nameof(ChargingStation) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
                {

                    if (RemoteChargingStation is not null)
                    {

                        result = await RemoteChargingStation.
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
                DebugX.LogException(e, nameof(ChargingStation) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (internal) SendNewReservation     (Timestamp, Sender, Reservation)

        internal void SendNewReservation(DateTime             Timestamp,
                                         Object               Sender,
                                         ChargingReservation  Reservation)
        {

            OnNewReservation?.Invoke(Timestamp, Sender, Reservation);

        }

        #endregion

        #region (internal) SendReservationCanceled(Timestamp, Sender, Reservation, Reason)

        internal void SendReservationCanceled(DateTime                               Timestamp,
                                              Object                                 Sender,
                                              ChargingReservation                    Reservation,
                                              ChargingReservationCancellationReason  Reason)
        {

            OnReservationCanceled?.Invoke(Timestamp, Sender, Reservation, Reason);

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions

        #region Data

        public IEnumerable<ChargingSession> ChargingSessions
            => RoamingNetwork.SessionsStore.Where(session => session.ChargingStationId == Id);

        #region TryGetChargingSessionById(SessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
            => RoamingNetwork.SessionsStore.TryGet(SessionId, out ChargingSession);

        #endregion

        #endregion

        #region Events

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

            RemoteStart(ChargingProduct?         ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication?    RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)


                => RemoteStart(ChargingLocation.FromChargingStationId(Id),
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
        /// Start a charging session at the given EVSE.
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
                        ChargingProduct?         ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication?    RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;


            RemoteStartResult? result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartRequest?.Invoke(startTime,
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
                DebugX.LogException(e, nameof(ChargingStation) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
                {

                    if (ChargingLocation.EVSEId.HasValue                            &&
                        TryGetEVSEById(ChargingLocation.EVSEId.Value, out var evse) &&
                        evse is not null)
                    {

                        result = await evse.RemoteStart(ChargingLocation,
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

                        if (result?.Result == RemoteStartResultTypes.Success ||
                            result?.Result == RemoteStartResultTypes.AsyncOperation)
                        {

                            // The session can be delivered within the response
                            // or via an explicit message afterwards!
                            if (result.Session is not null)
                                result.Session.ChargingStation = this;

                        }

                        #endregion

                    }
                    else
                        result = RemoteStartResult.UnknownLocation();

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

            result ??= RemoteStartResult.Error("unkown");


            #region Send OnRemoteStartResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartResponse?.Invoke(endTime,
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
                                              endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(ChargingStation) + "." + nameof(OnRemoteStartResponse));
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
                       EMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;


            RemoteStopResult? result = null;

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
                DebugX.LogException(e, nameof(ChargingStation) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
                {

                    if (TryGetChargingSessionById(SessionId, out var chargingSession) &&
                        chargingSession.EVSEId.HasValue                               &&
                        TryGetEVSEById(chargingSession.EVSEId.Value, out var evse)    &&
                        evse is not null)
                    {

                        result = await evse.RemoteStop(SessionId,
                                                       ReservationHandling,
                                                       ProviderId,
                                                       RemoteAuthentication,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout);

                    }

                    if (result is null)
                    {
                        DebugX.Log("Invalid charging session at charging station '" + Id + "': " + SessionId);
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

            result ??= RemoteStopResult.Error(SessionId, "unknown");


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
                DebugX.LogException(e, nameof(ChargingStation) + "." + nameof(OnRemoteStopResponse));
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

                if (Session.ChargingStation == null)
                {
                    Session.ChargingStation    = this;
                    Session.ChargingStationId  = Id;
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


        #region ToJSON(this ChargingStation,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging station.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public JObject ToJSON(Boolean                                             Embedded                          = false,
                              InfoStatus                                          ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                                          ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                                          ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                              InfoStatus                                          ExpandEVSEIds                     = InfoStatus.Expanded,
                              InfoStatus                                          ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                          ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<IChargingStation>?  CustomChargingStationSerializer   = null,
                              CustomJObjectSerializerDelegate<IEVSE>?             CustomEVSESerializer              = null)
        {

            try
            {

                var json = JSONObject.Create(

                               new JProperty("@id", Id.ToString()),

                               !Embedded
                                   ? new JProperty("@context",    JSONLDContext)
                                   : null,

                               Name.       IsNeitherNullNorEmpty()
                                   ? new JProperty("name",        Name.ToJSON())
                                   : null,

                               Description.IsNeitherNullNorEmpty()
                                   ? new JProperty("description", Description.ToJSON())
                                   : null,

                               (!Embedded || DataSource   != ChargingPool.DataSource)
                                   ? new JProperty("dataSource", DataSource)
                                   : null,

                               (!Embedded || DataLicenses != ChargingPool.DataLicenses)
                                   ? ExpandDataLicenses.Switch(
                                       () => new JProperty("dataLicenseIds",  new JArray(DataLicenses.SafeSelect(license => license.Id.ToString()))),
                                       () => new JProperty("dataLicenses",    DataLicenses.ToJSON()))
                                   : null,

                               ExpandRoamingNetworkId != InfoStatus.Hidden && RoamingNetwork is not null
                                   ? ExpandRoamingNetworkId.Switch(
                                         () => new JProperty("roamingNetworkId",                  RoamingNetwork.Id. ToString()),
                                         () => new JProperty("roamingNetwork",                    RoamingNetwork.    ToJSON(Embedded:                          true,
                                                                                                                                         ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
                                                                                                                                         ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                                         ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                                         ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                                         ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                                         ExpandDataLicenses:                InfoStatus.Hidden)))
                                   : null,

                               ExpandChargingStationOperatorId != InfoStatus.Hidden && Operator is not null
                                   ? ExpandChargingStationOperatorId.Switch(
                                         () => new JProperty("chargingStationOperatorperatorId",  Operator.Id.       ToString()),
                                         () => new JProperty("chargingStationOperatorperator",    Operator.          ToJSON(Embedded:                          true,
                                                                                                                                         ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                                         ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                                         ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                                         ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                                         ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                                         ExpandDataLicenses:                InfoStatus.Hidden)))
                                   : null,

                               ExpandChargingPoolId != InfoStatus.Hidden && ChargingPool is not null
                                   ? ExpandChargingPoolId.Switch(
                                         () => new JProperty("chargingPoolId",             ChargingPool.Id.   ToString()),
                                         () => new JProperty("chargingPool",               ChargingPool.      ToJSON(Embedded:                          true,
                                                                                                                                     ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                                     ExpandChargingStationOperatorId:   InfoStatus.Hidden,
                                                                                                                                     ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                                     ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                                     ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                                     ExpandDataLicenses:                InfoStatus.Hidden)))
                                   : null,

                               (!Embedded || GeoLocation         != ChargingPool.GeoLocation)         ? new JProperty("geoLocation",          (GeoLocation ?? ChargingPool.GeoLocation).Value.  ToJSON()) : null,
                               (!Embedded || Address             != ChargingPool.Address)             ? new JProperty("address",              (Address     ?? ChargingPool.Address).            ToJSON()) : null,
                               (!Embedded || AuthenticationModes != ChargingPool.AuthenticationModes) ? new JProperty("authenticationModes",  AuthenticationModes.ToJSON())   : null,
                               (!Embedded || HotlinePhoneNumber  != ChargingPool.HotlinePhoneNumber)  ? new JProperty("hotlinePhoneNumber",   HotlinePhoneNumber. ToString()) : null,
                               (!Embedded || OpeningTimes        != ChargingPool.OpeningTimes)        ? new JProperty("openingTimes",         OpeningTimes.       ToJSON())   : null,

                               ExpandEVSEIds != InfoStatus.Hidden && EVSEs.Any()
                                   ? ExpandEVSEIds.Switch(

                                         () => new JProperty("EVSEIds",
                                                             new JArray(EVSEs.Select (evse   => evse.Id).
                                                                              OrderBy(evseId => evseId).
                                                                              Select (evseId => evseId.ToString()))),

                                         () => new JProperty("EVSEs",
                                                             new JArray(EVSEs.OrderBy(evse   => evse).
                                                                              ToJSON (Embedded:                         true,
                                                                                      ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                      ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                      ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                      ExpandChargingStationId:          InfoStatus.Hidden,
                                                                                      ExpandBrandIds:                   InfoStatus.ShowIdOnly,
                                                                                      ExpandDataLicenses:               InfoStatus.Hidden,
                                                                                      CustomEVSESerializer:             CustomEVSESerializer).
                                                                              Where  (evse => evse != null))))

                                   : null,


                               ExpandBrandIds != InfoStatus.Hidden && Brands.Any()
                                   ? ExpandBrandIds.Switch(

                                         () => new JProperty("brandIds",
                                                             new JArray(Brands.Select (brand   => brand.Id).
                                                                               OrderBy(brandId => brandId).
                                                                               Select (brandId => brandId.ToString()))),

                                         () => new JProperty("brands",
                                                             new JArray(Brands.OrderBy(brand => brand.Id).
                                                                               ToJSON (Embedded:            true,
                                                                                       ExpandDataLicenses:  InfoStatus.ShowIdOnly))))

                                   : null

                         );

                return CustomChargingStationSerializer is not null
                           ? CustomChargingStationSerializer(this, json)
                           : json;

            }
            catch (Exception e)
            {
                return new JObject(
                           new JProperty("@id",         Id.ToString()),
                           new JProperty("@context",    JSONLDContext),
                           new JProperty("exception",   e.Message),
                           new JProperty("stackTrace",  e.StackTrace)
                       );
            }

        }

        #endregion



        #region UpdateWith(OtherChargingStation)

        /// <summary>
        /// Update this charging station with the data of the other charging station.
        /// </summary>
        /// <param name="OtherChargingStation">Another charging station.</param>
        public IChargingStation UpdateWith(IChargingStation OtherChargingStation)
        {

            Name.               Set    (OtherChargingStation.Name);
            Description.        Set    (OtherChargingStation.Description);

            Brands.             Replace(OtherChargingStation.Brands);
            UIFeatures.         Replace(OtherChargingStation.UIFeatures);
            AuthenticationModes.Replace(OtherChargingStation.AuthenticationModes);
            PaymentOptions.     Replace(OtherChargingStation.PaymentOptions);
            PhotoURLs.          Replace(OtherChargingStation.PhotoURLs);
            Features.           Replace(OtherChargingStation.Features);

            ArrivalInstructions.Set    (OtherChargingStation.ArrivalInstructions);
            HotlinePhoneNumber        = OtherChargingStation.HotlinePhoneNumber;

            Address                   = OtherChargingStation.Address;
            OpenStreetMapNodeId       = OtherChargingStation.OpenStreetMapNodeId;
            GeoLocation               = OtherChargingStation.GeoLocation;
            EntranceAddress           = OtherChargingStation.EntranceAddress;
            EntranceLocation          = OtherChargingStation.EntranceLocation;
            OpeningTimes              = OtherChargingStation.OpeningTimes;
            //ParkingSpaces             = OtherChargingStation.ParkingSpaces;
            Accessibility             = OtherChargingStation.Accessibility;
            GridConnection            = OtherChargingStation.GridConnection;
            ExitAddress               = OtherChargingStation.ExitAddress;
            ExitLocation              = OtherChargingStation.ExitLocation;

            if (OtherChargingStation.AdminStatus.Timestamp > AdminStatus.Timestamp)
                AdminStatus           = OtherChargingStation.AdminStatus;

            if (OtherChargingStation.Status.     Timestamp > Status.     Timestamp)
                Status                = OtherChargingStation.Status;

            return this;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStation ChargingStation1,
                                           ChargingStation ChargingStation2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStation1, ChargingStation2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingStation1 is null || ChargingStation2 is null)
                return false;

            return ChargingStation1.Equals(ChargingStation2);

        }

        #endregion

        #region Operator != (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStation ChargingStation1,
                                           ChargingStation ChargingStation2)

            => !(ChargingStation1 == ChargingStation2);

        #endregion

        #region Operator <  (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStation ChargingStation1,
                                          ChargingStation ChargingStation2)
        {

            if (ChargingStation1 is null)
                throw new ArgumentNullException(nameof(ChargingStation1), "The given ChargingStation1 must not be null!");

            return ChargingStation1.CompareTo(ChargingStation2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStation ChargingStation1,
                                           ChargingStation ChargingStation2)

            => !(ChargingStation1 > ChargingStation2);

        #endregion

        #region Operator >  (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStation ChargingStation1,
                                          ChargingStation ChargingStation2)
        {

            if (ChargingStation1 is null)
                throw new ArgumentNullException(nameof(ChargingStation1), "The given ChargingStation1 must not be null!");

            return ChargingStation1.CompareTo(ChargingStation2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStation ChargingStation1,
                                           ChargingStation ChargingStation2)

            => !(ChargingStation1 < ChargingStation2);

        #endregion

        #endregion

        #region IComparable<ChargingStation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is ChargingStation chargingStation
                   ? CompareTo(chargingStation)
                   : throw new ArgumentException("The given object is not a charging station!", nameof(Object));

        #endregion

        #region CompareTo(ChargingStation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation">An ChargingStation to compare with.</param>
        public Int32 CompareTo(ChargingStation? ChargingStation)

            => ChargingStation is not null
                   ? Id.CompareTo(ChargingStation.Id)
                   : throw new ArgumentException("The given object is not a ChargingStation!", nameof(ChargingStation));

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IChargingStation">An IChargingStation to compare with.</param>
        public Int32 CompareTo(IChargingStation? IChargingStation)

            => IChargingStation is not null
                   ? Id.CompareTo(IChargingStation.Id)
                   : throw new ArgumentException("The given object is not a IChargingStation!", nameof(IChargingStation));

        #endregion

        #endregion

        #region IEquatable<ChargingStation> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStation chargingStation &&
                   Equals(chargingStation);

        #endregion

        #region Equals(ChargingStation)

        /// <summary>
        /// Compares two charging stations for equality.
        /// </summary>
        /// <param name="ChargingStation">A charging station to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStation? ChargingStation)

            => ChargingStation is not null &&
               Id.Equals(ChargingStation.Id);

        /// <summary>
        /// Compares two charging stations for equality.
        /// </summary>
        /// <param name="IChargingStation">A charging station to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IChargingStation? IChargingStation)

            => IChargingStation is not null &&
               Id.Equals(IChargingStation.Id);

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
