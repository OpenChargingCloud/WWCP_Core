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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging pool
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingPool : AEMobilityEntity<ChargingPool_Id,
                                                 ChargingPoolAdminStatusTypes,
                                                 ChargingPoolStatusTypes>,
                                IEquatable<ChargingPool>,
                                IComparable<ChargingPool>,
                                IChargingPool
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const            String    JSONLDContext                                    = "https://open.charging.cloud/contexts/wwcp+json/chargingPool";


        private readonly        Decimal   EPSILON                                          = 0.01m;

        /// <summary>
        /// The default max size of the charging pool (aggregated charging station) status list.
        /// </summary>
        public const            UInt16    DefaultMaxChargingPoolStatusScheduleSize         = 15;

        /// <summary>
        /// The default max size of the charging pool admin status list.
        /// </summary>
        public const            UInt16    DefaultMaxChargingPoolAdminStatusScheduleSize    = 15;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly  TimeSpan  DefaultMaxChargingPoolReservationDuration        = TimeSpan.FromMinutes(30);

        #endregion

        #region Properties

        /// <summary>
        /// The roaming network of this charging pool.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork?              RoamingNetwork
            => Operator?.RoamingNetwork;

        /// <summary>
        /// The charging station operator of this charging pool.
        /// </summary>
        [Optional]
        public ChargingStationOperator?      Operator               { get; }

        /// <summary>
        /// The charging station sub operator of this charging pool.
        /// </summary>
        [Optional]
        public ChargingStationOperator?      SubOperator            { get; }


        /// <summary>
        /// The remote charging pool.
        /// </summary>
        [Optional]
        public IRemoteChargingPool?          RemoteChargingPool     { get; }


        /// <summary>
        /// All brands registered for this charging pool.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<Brand>            Brands                 { get; }

        /// <summary>
        /// The license of the charging pool data.
        /// </summary>
        [Mandatory, SlowData]
        public ReactiveSet<OpenDataLicense>  DataLicenses           { get; }


        #region LocationLanguage

        private Languages? locationLanguage;

        /// <summary>
        /// The official language at this charging pool.
        /// </summary>
        [Optional]
        public Languages? LocationLanguage
        {

            get
            {
                return locationLanguage;
            }

            set
            {

                if (locationLanguage != value)
                {

                    if (value == null)
                        DeleteProperty(ref locationLanguage);

                    else
                        SetProperty(ref locationLanguage, value);

                }

            }

        }

        #endregion

        #region Address

        private Address? address;

        /// <summary>
        /// The address of this charging pool.
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

                if (address != value)
                {

                    if (value == null)
                        DeleteProperty(ref address);

                    else
                        SetProperty(ref address, value);

                    // Delete inherited addresses
                    chargingStations.ForEach(station => station.Address = null);

                }

            }

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate? geoLocation;

        /// <summary>
        /// The geographical location of this charging pool.
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

                if (geoLocation != value)
                {

                    if (value == null)
                        DeleteProperty(ref geoLocation);

                    else
                        SetProperty(ref geoLocation, value);

                    // Delete inherited geo locations
                    chargingStations.ForEach(station => station.GeoLocation = null);

                }

            }

        }

        #endregion

        #region EntranceAddress

        private Address? entranceAddress;

        /// <summary>
        /// The address of the entrance to this charging pool.
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

                if (entranceAddress != value)
                {

                    if (value == null)
                        DeleteProperty(ref entranceAddress);

                    else
                        SetProperty(ref entranceAddress, value);

                    // Delete inherited entrance addresses
                    chargingStations.ForEach(station => station.EntranceAddress = null);

                }

            }

        }

        #endregion

        #region EntranceLocation

        private GeoCoordinate? entranceLocation;

        /// <summary>
        /// The geographical location of the entrance to this charging pool.
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

                if (entranceLocation != value)
                {

                    if (value == null)
                        DeleteProperty(ref entranceLocation);

                    else
                        SetProperty(ref entranceLocation, value);

                    // Delete inherited entrance locations
                    chargingStations.ForEach(station => station.EntranceLocation = null);

                }

            }

        }

        #endregion

        #region ArrivalInstructions

        /// <summary>
        /// An optional (multi-language) description of how to find the charging pool.
        /// </summary>
        [Optional]
        public I18NString                               ArrivalInstructions     { get; }

        #endregion

        #region OpeningTimes

        private OpeningTimes openingTimes;

        /// <summary>
        /// The opening times of this charging pool.
        /// </summary>
        [Mandatory]
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

        #region ChargingWhenClosed

        /// <summary>
        /// Indicates if the charging stations are still charging outside the opening hours of the charging pool.
        /// </summary>
        public Boolean?  ChargingWhenClosed    { get; set; }

        #endregion

        #region UIFeatures

        /// <summary>
        /// User interface features of the charging pool, when those features
        /// are not features of the charging stations, e.g. an external payment terminal.
        /// </summary>
        [Optional]
        public ReactiveSet<UIFeatures>                  UIFeatures              { get; }

        #endregion

        #region AuthenticationModes

        /// <summary>
        /// The authentication options an EV driver can use.
        /// </summary>
        [Optional]
        public ReactiveSet<AuthenticationModes>         AuthenticationModes     { get; }

        #endregion

        #region PaymentOptions

        /// <summary>
        /// The payment options an EV driver can use.
        /// </summary>
        [Optional]
        public ReactiveSet<PaymentOptions>              PaymentOptions          { get; }

        #endregion

        #region Accessibility

        private AccessibilityTypes? accessibility;

        /// <summary>
        /// The accessibility of the charging station.
        /// </summary>
        [Optional]
        public AccessibilityTypes? Accessibility
        {

            get
            {
                return accessibility;
            }

            set
            {

                if (accessibility != value)
                {

                    if (value == null)
                        DeleteProperty(ref accessibility);

                    else
                        SetProperty(ref accessibility, value);

                    // Delete inherited accessibilities
                    chargingStations.ForEach(station => station.Accessibility = null);

                }

            }

        }

        #endregion

        #region Features

        /// <summary>
        /// Charging features of the charging pool, when those features
        /// are not features of the charging stations, e.g. hasARoof.
        /// </summary>
        [Optional]
        public ReactiveSet<Features>                    Features                { get; }

        #endregion

        #region Facilities

        /// <summary>
        /// Charging facilities of the charging pool, e.g. a supermarket.
        /// </summary>
        [Optional]
        public ReactiveSet<Facilities>                  Facilities              { get; }

        #endregion


        #region PhotoURLs

        /// <summary>
        /// URIs of photos of this charging pool.
        /// </summary>
        [Optional]
        public ReactiveSet<URL>                         PhotoURLs               { get; }

        #endregion

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

                    // Delete inherited accessibilities
                    chargingStations.ForEach(station => station.HotlinePhoneNumber = null);

                }

            }

        }

        #endregion


        #region GridConnection

        private GridConnectionTypes? gridConnection;

        /// <summary>
        /// The grid connection of the charging pool.
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

                if (gridConnection != value)
                {

                    if (value is null)
                        DeleteProperty(ref gridConnection);

                    else
                        SetProperty(ref gridConnection, value);

                    // Delete inherited grid connections
                    chargingStations.ForEach(station => station.GridConnection = null);

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
                return energyMix;
            }

            set
            {

                if (value != energyMix)
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
                return energyMixPrognoses;
            }

            set
            {

                if (value != energyMixPrognoses)
                {

                    if (value == null)
                        DeleteProperty(ref energyMixPrognoses);

                    else
                        SetProperty(ref energyMixPrognoses, value);

                }

            }

        }

        #endregion


        #region ExitAddress

        private Address? exitAddress;

        /// <summary>
        /// The address of the exit of this charging pool.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address? ExitAddress
        {

            get
            {
                return exitAddress;
            }

            set
            {

                if (exitAddress != value)
                {

                    if (value == null)
                        DeleteProperty(ref exitAddress);

                    else
                        SetProperty(ref exitAddress, value);

                    // Delete inherited exit addresses
                    chargingStations.ForEach(station => station.ExitAddress = null);

                }

            }

        }

        #endregion

        #region ExitLocation

        private GeoCoordinate? exitLocation;

        /// <summary>
        /// The geographical location of the exit of this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate? ExitLocation
        {

            get
            {
                return exitLocation;
            }

            set
            {

                if (exitLocation != value)
                {

                    if (value == null)
                        DeleteProperty(ref exitLocation);

                    else
                        SetProperty(ref exitLocation, value);

                    // Delete inherited exit locations
                    chargingStations.ForEach(station => station.ExitLocation = null);

                }

            }

        }

        #endregion


        #region StatusAggregationDelegate

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated charging stations.
        /// </summary>
        public Func<ChargingStationStatusReport, ChargingPoolStatusTypes>? StatusAggregationDelegate { get; set; }

        #endregion

        #endregion

        #region Constructor(s)

        #region ChargingPool(Id, ...)

        /// <summary>
        /// Create a new group/pool of charging stations having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing pool.</param>
        /// <param name="MaxPoolStatusListSize">The default size of the charging pool (aggregated charging station) status list.</param>
        /// <param name="MaxPoolAdminStatusListSize">The default size of the charging pool admin status list.</param>
        public ChargingPool(ChargingPool_Id                             Id,
                            I18NString?                                 Name                         = null,
                            I18NString?                                 Description                  = null,

                            Address?                                    Address                      = null,
                            GeoCoordinate?                              GeoLocation                  = null,

                            Action<ChargingPool>?                       Configurator                 = null,
                            RemoteChargingPoolCreatorDelegate?          RemoteChargingPoolCreator    = null,
                            Timestamped<ChargingPoolAdminStatusTypes>?  InitialAdminStatus           = null,
                            Timestamped<ChargingPoolStatusTypes>?       InitialStatus                = null,
                            UInt16                                      MaxPoolAdminStatusListSize   = DefaultMaxAdminStatusScheduleSize,
                            UInt16                                      MaxPoolStatusListSize        = DefaultMaxStatusScheduleSize)

            : this(Id,
                   null,
                   Name,
                   Description,

                   Address,
                   GeoLocation,

                   Configurator,
                   RemoteChargingPoolCreator,
                   InitialAdminStatus,
                   InitialStatus,
                   MaxPoolAdminStatusListSize,
                   MaxPoolStatusListSize)

        { }

        #endregion

        #region ChargingPool(Id, Operator, ...)

        /// <summary>
        /// Create a new group/pool of charging stations having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing pool.</param>
        /// <param name="Operator">The parent charging station operator.</param>
        /// <param name="Configurator">A delegate to configure the newly created charging station.</param>
        /// <param name="RemoteChargingPoolCreator">A delegate to attach a remote charging pool.</param>
        /// <param name="InitialAdminStatus">An optional initial admin status of the EVSE.</param>
        /// <param name="InitialStatus">An optional initial status of the EVSE.</param>
        /// <param name="MaxPoolStatusScheduleSize">The default size of the charging pool (aggregated charging station) status list.</param>
        /// <param name="MaxPoolAdminStatusScheduleSize">The default size of the charging pool admin status list.</param>
        public ChargingPool(ChargingPool_Id                             Id,
                            ChargingStationOperator                     Operator,
                            I18NString?                                 Name                             = null,
                            I18NString?                                 Description                      = null,

                            Address?                                    Address                          = null,
                            GeoCoordinate?                              GeoLocation                      = null,

                            Action<ChargingPool>?                       Configurator                     = null,
                            RemoteChargingPoolCreatorDelegate?          RemoteChargingPoolCreator        = null,
                            Timestamped<ChargingPoolAdminStatusTypes>?  InitialAdminStatus               = null,
                            Timestamped<ChargingPoolStatusTypes>?       InitialStatus                    = null,
                            UInt16?                                     MaxPoolAdminStatusScheduleSize   = null,
                            UInt16?                                     MaxPoolStatusScheduleSize        = null,

                            String?                                     DataSource                       = null,
                            DateTime?                                   LastChange                       = null,

                            JObject?                                    CustomData                       = null,
                            UserDefinedDictionary?                      InternalData                     = null)

            : base(Id,
                   Name,
                   Description,
                   InitialAdminStatus             ?? ChargingPoolAdminStatusTypes.Operational,
                   InitialStatus                  ?? ChargingPoolStatusTypes.Available,
                   MaxPoolAdminStatusScheduleSize ?? DefaultMaxAdminStatusScheduleSize,
                   MaxPoolStatusScheduleSize      ?? DefaultMaxStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Init data and properties

            this.Address                           = Address;
            this.GeoLocation                       = GeoLocation;

            this.Operator                          = Operator;
            this.openingTimes                      = OpeningTimes.Open24Hours;

            this.Brands                            = new ReactiveSet<Brand>();
            this.Brands.OnSetChanged              += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("Brands",
                                oldItems,
                                newItems);

            };

            this.DataLicenses                       = new ReactiveSet<OpenDataLicense>();
            this.DataLicenses.OnSetChanged         += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("DataLicenses",
                                oldItems,
                                newItems);

            };

            this.UIFeatures                         = new ReactiveSet<UIFeatures>();
            this.UIFeatures.OnSetChanged           += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("UIFeatures",
                                oldItems,
                                newItems);

            };

            this.AuthenticationModes                = new ReactiveSet<AuthenticationModes>();
            this.AuthenticationModes.OnSetChanged  += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("AuthenticationModes",
                                oldItems,
                                newItems);

            };

            this.PaymentOptions                     = new ReactiveSet<PaymentOptions>();
            this.PaymentOptions.OnSetChanged       += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("PaymentOptions",
                                oldItems,
                                newItems);

            };

            this.Features                           = new ReactiveSet<Features>();
            this.Features.OnSetChanged             += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("Features",
                                oldItems,
                                newItems);

            };

            this.Facilities                         = new ReactiveSet<Facilities>();
            this.Facilities.OnSetChanged           += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("Facilities",
                                oldItems,
                                newItems);

            };


            this.PhotoURLs                          = new ReactiveSet<URL>();
            this.PhotoURLs.OnSetChanged            += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("PhotoURLs",
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

            this.HotlinePhoneNumber  = HotlinePhoneNumber;

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


            this.chargingStations            = new EntityHashSet<IChargingPool, ChargingStation_Id, IChargingStation>(this);
            //this.evses.OnSetChanged               += (timestamp, reactiveSet, newItems, oldItems) =>
            //{

            //    PropertyChanged("ChargingStations",
            //                    oldItems,
            //                    newItems);

            //};

            #endregion

            #region Init events

            // ChargingPool events
            this.ChargingStationAddition  = new VotingNotificator<DateTime, IChargingPool, IChargingStation, Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval   = new VotingNotificator<DateTime, IChargingPool, IChargingStation, Boolean>(() => new VetoVote(), true);

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

            this.RemoteChargingPool = RemoteChargingPoolCreator?.Invoke(this);

        }

        #endregion

        #endregion


        #region Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnChargingPoolDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status changed.
        /// </summary>
        public event OnChargingPoolStatusChangedDelegate?       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate?  OnAdminStatusChanged;

        #endregion


        #region (internal) UpdateData       (Timestamp, EventTrackingId, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="Sender">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The optional old value of the changed property.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool data update.</param>
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
                                    Sender as ChargingPool,
                                    PropertyName,
                                    NewValue,
                                    OldValue,
                                    DataSource);

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        /// <param name="OldStatus">The optional old charging station admin status.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool admin status update.</param>
        internal async Task UpdateAdminStatus(DateTime                                    Timestamp,
                                              EventTracking_Id                            EventTrackingId,
                                              Timestamped<ChargingPoolAdminStatusTypes>   NewStatus,
                                              Timestamped<ChargingPoolAdminStatusTypes>?  OldStatus    = null,
                                              Context?                                    DataSource   = null)
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

        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        /// <param name="OldStatus">The optional old EVSE status.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool admin status update.</param>
        internal async Task UpdateStatus(DateTime                               Timestamp,
                                         EventTracking_Id                       EventTrackingId,
                                         Timestamped<ChargingPoolStatusTypes>   NewStatus,
                                         Timestamped<ChargingPoolStatusTypes>?  OldStatus    = null,
                                         Context?                               DataSource   = null)
        {

            var onStatusChanged = OnStatusChanged;
            if (onStatusChanged is not null)
                await onStatusChanged(Timestamp,
                                      EventTrackingId,
                                      this,
                                      NewStatus,
                                      OldStatus,
                                      DataSource);

        }

        #endregion

        #endregion

        #region Charging stations

        #region ChargingStations

        private readonly EntityHashSet<IChargingPool, ChargingStation_Id, IChargingStation> chargingStations;

        /// <summary>
        /// Return all charging stations registered within this charing pool.
        /// </summary>
        public IEnumerable<IChargingStation> ChargingStations
        {
            get
            {
                lock (chargingStations)
                {
                    return chargingStations.ToArray();
                }
            }
        }

        #endregion

        #region ChargingStationIds        (IncludeStations = null)

        /// <summary>
        /// Return an enumeration of all charging station identifications.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate? IncludeStations = null)

            => IncludeStations is null

                   ? ChargingStations.
                         Select    (station => station.Id)

                   : ChargingStations.
                         Where     (station => IncludeStations(station)).
                         Select    (station => station.Id);

        #endregion

        #region ChargingStationAdminStatus(IncludeStations = null)

        /// <summary>
        /// Return an enumeration of all charging station admin status.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStationAdminStatus> ChargingStationAdminStatus(IncludeChargingStationDelegate? IncludeStations = null)

            => IncludeStations is null

                   ? ChargingStations.
                         Select    (station => new ChargingStationAdminStatus(station.Id, station.AdminStatus))

                   : ChargingStations.
                         Where     (station => IncludeStations(station)).
                         Select    (station => new ChargingStationAdminStatus(station.Id, station.AdminStatus));

        #endregion

        #region ChargingStationStatus     (IncludeStations = null)

        /// <summary>
        /// Return an enumeration of all charging station status.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStationStatus> ChargingStationStatus(IncludeChargingStationDelegate? IncludeStations = null)

            => IncludeStations is null

                   ? ChargingStations.
                         Select    (station => new ChargingStationStatus(station.Id, station.Status))

                   : ChargingStations.
                         Where     (station => IncludeStations(station)).
                         Select    (station => new ChargingStationStatus(station.Id, station.Status));

        #endregion


        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, IChargingPool, IChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, IChargingPool, IChargingStation, Boolean> OnChargingStationAddition

            => ChargingStationAddition;

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, IChargingPool, IChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, IChargingPool, IChargingStation, Boolean> OnChargingStationRemoval

            => ChargingStationRemoval;

        #endregion


        #region CreateChargingStation        (Id, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station failed.</param>
        public async Task<AddChargingStationResult> CreateChargingStation(ChargingStation_Id                                              Id,
                                                                          I18NString?                                                     Name                           = null,
                                                                          I18NString?                                                     Description                    = null,

                                                                          Address?                                                        Address                        = null,
                                                                          GeoCoordinate?                                                  GeoLocation                    = null,

                                                                          Action<IChargingStation>?                                       Configurator                   = null,
                                                                          RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator   = null,
                                                                          Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                          Timestamped<ChargingStationStatusTypes>?                        InitialStatus                  = null,
                                                                          UInt16?                                                         MaxAdminStatusListSize         = null,
                                                                          UInt16?                                                         MaxStatusListSize              = null,
                                                                          Action<IChargingStation>?                                       OnSuccess                      = null,
                                                                          Action<IChargingPool, ChargingStation_Id>?                      OnError                        = null,
                                                                          Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null)
        {

            //ToDo: Add async lock!

            #region Initial checks

            if (chargingStations.ContainsId(Id))
            {
                if (OnError is null)
                    throw new ChargingStationAlreadyExistsInPool(this, Id);
                else
                    OnError?.Invoke(this, Id);
            }

            AllowInconsistentOperatorIds ??= ((chargingStationOperatorId, chargingStationId) => false);

            if (Operator.Id != Id.OperatorId && !AllowInconsistentOperatorIds(Operator.Id, Id))
                return null;
                //throw new InvalidChargingStationOperatorId(this,
                //                                           Id.OperatorId);

            #endregion

            var chargingStation = new ChargingStation(Id,
                                                      this,
                                                      Name,
                                                      Description,

                                                      Address,
                                                      GeoLocation,

                                                      Configurator,
                                                      RemoteChargingStationCreator,
                                                      InitialAdminStatus,
                                                      InitialStatus,
                                                      MaxAdminStatusListSize,
                                                      MaxStatusListSize);


            if (ChargingStationAddition.SendVoting(Timestamp.Now, this, chargingStation) &&
                chargingStations.TryAdd(chargingStation))
            {

                chargingStation.OnDataChanged                  += UpdateChargingStationData;
                chargingStation.OnStatusChanged                += UpdateChargingStationStatus;
                chargingStation.OnAdminStatusChanged           += UpdateChargingStationAdminStatus;

                chargingStation.OnEVSEAddition.OnVoting        += (timestamp, station, evse, vote)  => EVSEAddition.SendVoting      (timestamp, station, evse, vote);
                chargingStation.OnEVSEAddition.OnNotification  += (timestamp, station, evse)        => EVSEAddition.SendNotification(timestamp, station, evse);
                chargingStation.OnEVSEDataChanged              += UpdateEVSEData;
                chargingStation.OnEVSEStatusChanged            += UpdateEVSEStatus;
                chargingStation.OnEVSEAdminStatusChanged       += UpdateEVSEAdminStatus;
                chargingStation.OnEVSERemoval. OnVoting        += (timestamp, station, evse, vote)  => EVSERemoval.SendVoting       (timestamp, station, evse, vote);
                chargingStation.OnEVSERemoval. OnNotification  += (timestamp, station, evse)        => EVSERemoval.SendNotification (timestamp, station, evse);


                chargingStation.OnNewReservation               += SendNewReservation;
                chargingStation.OnReservationCanceled          += SendReservationCanceled;
                chargingStation.OnNewChargingSession           += SendNewChargingSession;
                chargingStation.OnNewChargeDetailRecord        += SendNewChargeDetailRecord;


                if (RemoteChargingStationCreator != null)
                {

                    chargingStation.RemoteChargingStation.OnNewReservation += SendNewReservation;

                    chargingStation.RemoteChargingStation.OnNewReservation += (a, b, reservation) => {

                        var __EVSE = GetEVSEById(reservation.EVSEId.Value);

                        //__EVSE.Reservation = reservation;

                    };

                    chargingStation.RemoteChargingStation.OnNewChargingSession += (a, b, session) => {

                        var __EVSE = GetEVSEById(session.EVSEId.Value);

                        //__EVSE.ChargingSession = session;

                    };

                    chargingStation.RemoteChargingStation.OnNewChargeDetailRecord += (a, b, cdr) => {

                        var __EVSE = GetEVSEById(cdr.EVSEId.Value);

                        if (__EVSE is EVSE evse)
                            evse.SendNewChargeDetailRecord(Timestamp.Now,
                                                           this,
                                                           cdr);

                    };


                    chargingStation.RemoteChargingStation.OnReservationCanceled += chargingStation.SendReservationCanceled;

                    chargingStation.RemoteChargingStation.OnEVSEStatusChanged += (timestamp,
                                                                                  eventTrackingId,
                                                                                  evse,
                                                                                  newStatus,
                                                                                  oldStatus,
                                                                                  dataSource)

                        => chargingStation.UpdateEVSEStatus(timestamp,
                                                            eventTrackingId,
                                                            GetEVSEById(evse.Id),
                                                            newStatus,
                                                            oldStatus,
                                                            dataSource);

                }

                OnSuccess?.Invoke(chargingStation);
                ChargingStationAddition.SendNotification(Timestamp.Now, this, chargingStation);

                return AddChargingStationResult.Success(chargingStation, EventTracking_Id.New);

            }

            Debug.WriteLine("ChargingStation '" + Id + "' could not be created!");

            if (OnError == null)
                throw new ChargingStationCouldNotBeCreated(this, Id);

            OnError?.Invoke(this, Id);
            return null;

        }

        #endregion

        #region CreateOrUpdateChargingStation(Id, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station before its successful creation.</param>
        /// <param name="RemoteChargingStationCreator">A delegate to attach a remote charging station.</param>
        /// <param name="InitialAdminStatus">An optional initial admin status of the EVSE.</param>
        /// <param name="InitialStatus">An optional initial status of the EVSE.</param>
        /// <param name="MaxAdminStatusListSize">An optional max length of the admin staus list.</param>
        /// <param name="MaxStatusListSize">An optional max length of the staus list.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station failed.</param>
        public async Task<AddChargingStationResult> CreateOrUpdateChargingStation(ChargingStation_Id                                              Id,
                                                                                  I18NString?                                                     Name                           = null,
                                                                                  I18NString?                                                     Description                    = null,

                                                                                  Address?                                                        Address                        = null,
                                                                                  GeoCoordinate?                                                  GeoLocation                    = null,

                                                                                  Action<IChargingStation>?                                       Configurator                   = null,
                                                                                  RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator   = null,
                                                                                  Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                                  Timestamped<ChargingStationStatusTypes>?                        InitialStatus                  = null,
                                                                                  UInt16?                                                         MaxAdminStatusListSize         = null,
                                                                                  UInt16?                                                         MaxStatusListSize              = null,
                                                                                  Action<IChargingStation>?                                       OnSuccess                      = null,
                                                                                  Action<IChargingPool, ChargingStation_Id>?                      OnError                        = null,
                                                                                  Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null)
        {

            //lock (chargingStations)
            //{

                #region Initial checks

                AllowInconsistentOperatorIds ??= ((chargingStationOperatorId, chargingStationId) => false);

                if (Operator.Id != Id.OperatorId && !AllowInconsistentOperatorIds(Operator.Id, Id))
                    return null;
                    //throw new InvalidChargingStationOperatorId(this,
                    //                                           Id.OperatorId);

                #endregion

                #region If the charging pool identification is new/unknown: Call CreateChargingPool(...)

                if (!chargingStations.ContainsId(Id))
                    return await CreateChargingStation(Id,
                                                       Name,
                                                       Description,

                                                       Address,
                                                       GeoLocation,

                                                       Configurator,
                                                       RemoteChargingStationCreator,
                                                       InitialAdminStatus,
                                                       InitialStatus,
                                                       MaxAdminStatusListSize ?? ChargingStation.DefaultMaxAdminStatusScheduleSize,
                                                       MaxStatusListSize      ?? ChargingStation.DefaultMaxStatusScheduleSize,
                                                       OnSuccess,
                                                       OnError,
                                                       AllowInconsistentOperatorIds);

                #endregion


                var existingChargingStation = chargingStations.GetById(Id);

                // Merge existing charging station with new station data...

                if (existingChargingStation is not null)
                {

                    var updatedChargingStation = existingChargingStation.
                                                     UpdateWith(new ChargingStation(Id,
                                                                                    this,
                                                                                    Name,
                                                                                    Description,

                                                                                    Address,
                                                                                    GeoLocation,

                                                                                    Configurator,
                                                                                    null,
                                                                                    new Timestamped<ChargingStationAdminStatusTypes>(DateTime.MinValue, ChargingStationAdminStatusTypes.Operational),
                                                                                    new Timestamped<ChargingStationStatusTypes>(DateTime.MinValue, ChargingStationStatusTypes.Available)));

                    return AddChargingStationResult.Success(updatedChargingStation, EventTracking_Id.New);

                }

                return null;

            //}

        }

        #endregion


        #region ContainsChargingStation  (ChargingStation)

        /// <summary>
        /// Check if the given ChargingStation is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStation(IChargingStation ChargingStation)

            => chargingStations.ContainsId(ChargingStation.Id);

        #endregion

        #region ContainsChargingStation  (ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)

            => chargingStations.ContainsId(ChargingStationId);

        #endregion

        #region GetChargingStationById   (ChargingStationId)

        public IChargingStation? GetChargingStationById(ChargingStation_Id ChargingStationId)

            => chargingStations.GetById(ChargingStationId);

        #endregion

        #region TryGetChargingStationById(ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out IChargingStation? ChargingStation)

            => chargingStations.TryGet(ChargingStationId, out ChargingStation);

        #endregion


        #region RemoveChargingStation    (ChargingStationId)

        public async Task<RemoveChargingStationResult> RemoveChargingStation(ChargingStation_Id ChargingStationId)
        {

            if (TryGetChargingStationById(ChargingStationId, out var chargingStation))
            {

                if (ChargingStationRemoval.SendVoting(Timestamp.Now, this, chargingStation))
                {

                    if (chargingStations.TryRemove(ChargingStationId, out chargingStation))
                    {

                        ChargingStationRemoval.SendNotification(Timestamp.Now, this, chargingStation);

                        return RemoveChargingStationResult.Success(chargingStation, EventTracking_Id.New);

                    }

                }

            }

            return RemoveChargingStationResult.Failed(ChargingStationId, EventTracking_Id.New, "");

        }

        #endregion

        #region TryRemoveChargingStation (ChargingStationId, out ChargingStation)

        public Boolean TryRemoveChargingStation(ChargingStation_Id ChargingStationId, out IChargingStation? ChargingStation)
        {

            if (TryGetChargingStationById(ChargingStationId, out ChargingStation) &&
                ChargingStation is not null)
            {

                if (ChargingStationRemoval.SendVoting(Timestamp.Now, this, ChargingStation))
                {

                    if (chargingStations.TryRemove(ChargingStationId, out _))
                    {

                        ChargingStationRemoval.SendNotification(Timestamp.Now, this, ChargingStation);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion


        #region OnChargingStationData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationDataChangedDelegate?         OnChargingStationDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationStatusChangedDelegate?       OnChargingStationStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate?  OnChargingStationAdminStatusChanged;

        #endregion


        #region (internal) UpdateChargingStationData       (Timestamp, EventTrackingId, ChargingStation, PropertyName, NewValue, OldValue = null, DataSource = null)

        /// <summary>
        /// Update the static data of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The optional old value of the changed property.</param>
        /// <param name="DataSource">An optional data source or context for the charging station data update.</param>
        internal async Task UpdateChargingStationData(DateTime          Timestamp,
                                                      EventTracking_Id  EventTrackingId,
                                                      IChargingStation  ChargingStation,
                                                      String            PropertyName,
                                                      Object?           NewValue,
                                                      Object?           OldValue     = null,
                                                      Context?          DataSource   = null)
        {

            var onChargingStationDataChanged = OnChargingStationDataChanged;
            if (onChargingStationDataChanged is not null)
                await onChargingStationDataChanged(Timestamp,
                                                   EventTrackingId,
                                                   ChargingStation,
                                                   PropertyName,
                                                   NewValue,
                                                   OldValue,
                                                   DataSource);

        }

        #endregion

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, EventTrackingId, ChargingStation, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update the curent admin status of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        /// <param name="OldStatus">The optional old charging station admin status.</param>
        /// <param name="DataSource">An optional data source or context for the charging station admin status update.</param>
        internal async Task UpdateChargingStationAdminStatus(DateTime                                       Timestamp,
                                                             EventTracking_Id                               EventTrackingId,
                                                             IChargingStation                               ChargingStation,
                                                             Timestamped<ChargingStationAdminStatusTypes>   NewStatus,
                                                             Timestamped<ChargingStationAdminStatusTypes>?  OldStatus    = null,
                                                             Context?                                       DataSource   = null)
        {

            var onChargingStationAdminStatusChanged = OnChargingStationAdminStatusChanged;
            if (onChargingStationAdminStatusChanged is not null)
                await onChargingStationAdminStatusChanged(Timestamp,
                                                          EventTrackingId,
                                                          ChargingStation,
                                                          NewStatus,
                                                          OldStatus,
                                                          DataSource);

        }

        #endregion

        #region (internal) UpdateChargingStationStatus     (Timestamp, EventTrackingId, ChargingStation, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update the curent status of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="NewStatus">The new charging station status.</param>
        /// <param name="OldStatus">The optional old charging station status.</param>
        /// <param name="DataSource">An optional data source or context for the charging station status update.</param>
        internal async Task UpdateChargingStationStatus(DateTime                                  Timestamp,
                                                        EventTracking_Id                          EventTrackingId,
                                                        IChargingStation                          ChargingStation,
                                                        Timestamped<ChargingStationStatusTypes>   NewStatus,
                                                        Timestamped<ChargingStationStatusTypes>?  OldStatus    = null,
                                                        Context?                                  DataSource   = null)
        {

            var onChargingStationStatusChanged = OnChargingStationStatusChanged;
            if (onChargingStationStatusChanged is not null)
                await onChargingStationStatusChanged(Timestamp,
                                                     EventTrackingId,
                                                     ChargingStation,
                                                     NewStatus,
                                                     OldStatus,
                                                     DataSource);

            if (StatusAggregationDelegate is not null)
                statusSchedule.Insert(StatusAggregationDelegate(new ChargingStationStatusReport(chargingStations)),
                                      Timestamp,
                                      DataSource);

        }

        #endregion


        #region IEnumerable<ChargingStation> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => chargingStations.GetEnumerator();

        public IEnumerator<IChargingStation> GetEnumerator()
            => chargingStations.GetEnumerator();

        #endregion


        #region TryGetChargingStationByEVSEId(EVSEId, out Station)

        public Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out IChargingStation? Station)
        {

            foreach (var station in chargingStations)
            {

                if (station.TryGetEVSEById(EVSEId, out var evse))
                {
                    Station = station;
                    return true;
                }

            }

            Station = null;
            return false;

        }

        #endregion

        #endregion

        #region Charging station groups

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

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging pool.
        /// </summary>
        public IEnumerable<IEVSE> EVSEs
        {
            get
            {
                lock (chargingStations)
                {

                    return chargingStations.SelectMany(station => station.EVSEs).
                                            ToArray();

                }
            }
        }

        #endregion

        #region EVSEIds                (IncludeEVSEs = null)

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment
        /// (EVSEs) present within this charging pool.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate? IncludeEVSEs = null)

            => IncludeEVSEs == null

                   ? chargingStations.
                         SelectMany(station => station.EVSEs).
                         Select    (evse    => evse.Id)

                   : chargingStations.
                         SelectMany(station => station.EVSEs).
                         Where     (evse    => IncludeEVSEs(evse)).
                         Select    (evse    => evse.Id);

        #endregion

        #region EVSEAdminStatus        (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate IncludeEVSEs = null)

            => chargingStations.
                   SelectMany(station => station.EVSEAdminStatus(IncludeEVSEs));

        #endregion

        #region EVSEAdminStatusSchedule(IncludeEVSEs = null)

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

            => chargingStations.
                   SelectMany(station => station.EVSEAdminStatusSchedule(IncludeEVSEs,
                                                                         TimestampFilter,
                                                                         StatusFilter,
                                                                         Skip,
                                                                         Take));

        #endregion

        #region EVSEStatus             (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate IncludeEVSEs = null)

            => chargingStations.
                   SelectMany(station => station.EVSEStatus(IncludeEVSEs));

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

            => chargingStations.
                   SelectMany(station => station.EVSEStatusSchedule(IncludeEVSEs,
                                                                    TimestampFilter,
                                                                    StatusFilter,
                                                                    Skip,
                                                                    Take));

        #endregion



        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)

            => chargingStations.Any(ChargingStation => ChargingStation.EVSEIds().Contains(EVSE.Id));

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)

            => chargingStations.Any(ChargingStation => ChargingStation.EVSEIds().Contains(EVSEId));

        #endregion

        #region GetEVSEById(EVSEId)

        public IEVSE GetEVSEById(EVSE_Id EVSEId)

            => chargingStations.
                   SelectMany    (station => station.EVSEs).
                   FirstOrDefault(EVSE    => EVSE.Id == EVSEId);

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

        //#region SocketOutletAddition

        //internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        ///// <summary>
        ///// Called whenever a socket outlet will be or was added.
        ///// </summary>
        //public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletAddition

        //    => SocketOutletAddition;

        //#endregion

        //#region SocketOutletRemoval

        //internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletRemoval;

        ///// <summary>
        ///// Called whenever a socket outlet will be or was removed.
        ///// </summary>
        //public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletRemoval

        //    => SocketOutletRemoval;

        //#endregion


        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, IChargingStation, IEVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, IChargingStation, IEVSE, Boolean> OnEVSERemoval

            => EVSERemoval;

        #endregion


        #region TryGetEVSEById(EVSEId, out EVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE? EVSE)
        {

            EVSE = ChargingStations.
                       SelectMany    (station => station.EVSEs).
                       FirstOrDefault(evse    => evse.Id == EVSEId);

            return EVSE is not null;

        }

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
                DebugX.Log(e, $"ChargingPool '{Id}'.UpdateEVSEData of EVSE '{EVSE.Id}' property '{PropertyName}' from '{OldValue?.ToString() ?? "-"}' to '{NewValue?.ToString() ?? "-"}'");
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
                DebugX.Log(e, $"ChargingPool '{Id}'.UpdateEVSEAdminStatus of EVSE '{EVSE.Id}' from '{OldAdminStatus?.ToString() ?? "-"}' to '{NewAdminStatus}'");
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
                DebugX.Log(e, $"ChargingPool '{Id}'.UpdateEVSEStatus of EVSE '{EVSE.Id}' from '{OldStatus}' to '{NewStatus}'");
            }

        }

        #endregion

        #endregion


        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(30);


        #region Reservations...

        #region Data

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
            => RoamingNetwork.ReservationsStore.
                   Where     (reservations => reservations.First().ChargingPoolId == Id).
                   SelectMany(reservations => reservations);

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
        /// Reserve the possibility to charge at this charging pool.
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
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    CancellationToken                  CancellationToken      = default,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null)

                => Reserve(ChargingLocation.FromChargingPoolId(Id),
                           ChargingReservationLevel.ChargingPool,
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
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    CancellationToken                  CancellationToken      = default,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null)

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
                DebugX.LogException(e, nameof(ChargingPool) + "." + nameof(OnReserveRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.ChargingPoolId.HasValue && ChargingLocation.ChargingPoolId.Value != Id)
                    result = ReservationResult.UnknownLocation;

                else if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                         AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (RemoteChargingPool is not null)
                    {

                        result = await RemoteChargingPool.
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

                            RoamingNetwork.ReservationsStore.UpdateAll(result.Reservation.Id,
                                                                    reservation => reservation.ChargingPoolId = Id);

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

            var wndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveResponse?.Invoke(wndTime,
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
                                          wndTime - startTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(ChargingPool) + "." + nameof(OnReserveResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region CancelReservation(ReservationId, Reason, ...)

        public Task<CancelReservationResult> CancelReservation(ChargingReservation_Id                 ReservationId,
                                                               ChargingReservationCancellationReason  Reason,
                                                               DateTime?                              Timestamp           = null,
                                                               CancellationToken                      CancellationToken   = default,
                                                               EventTracking_Id?                      EventTrackingId     = null,
                                                               TimeSpan?                              RequestTimeout      = null)
        {
            throw new NotImplementedException();
        }

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
                              EMobilityProvider_Id?                  ProviderId          = null,

                              DateTime?                              Timestamp           = null,
                              CancellationToken                      CancellationToken   = default,
                              EventTracking_Id?                      EventTrackingId     = null,
                              TimeSpan?                              RequestTimeout      = null)

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
                DebugX.LogException(e, nameof(ChargingPool) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (RemoteChargingPool is not null)
                    {

                        result = await RemoteChargingPool.
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
                DebugX.LogException(e, nameof(ChargingPool) + "." + nameof(OnCancelReservationResponse));
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
            => RoamingNetwork.SessionsStore.Where(session => session.ChargingPoolId == Id);

        TimeSpan IChargingReservations.MaxReservationDuration { get; set; }

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
                        CancellationToken        CancellationToken      = default,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)


                => RemoteStart(ChargingLocation.FromChargingPoolId(Id),
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
                        CancellationToken        CancellationToken      = default,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
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

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if ((ChargingLocation.EVSEId.           HasValue && TryGetChargingStationByEVSEId(ChargingLocation.EVSEId.           Value, out var chargingStation) ||
                         ChargingLocation.ChargingStationId.HasValue && TryGetChargingStationById    (ChargingLocation.ChargingStationId.Value, out     chargingStation)) &&
                         chargingStation is not null)
                    {

                        result = await chargingStation.
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

                        if (result?.Result == RemoteStartResultTypes.Success ||
                            result?.Result == RemoteStartResultTypes.AsyncOperation)
                        {

                            // The session can be delivered within the response
                            // or via an explicit message afterwards!
                            if (result.Session != null)
                                result.Session.ChargingPool = this;

                        }

                        #endregion

                    }
                    else
                        result = RemoteStartResult.UnknownLocation();

                }
                else
                {
                    result = AdminStatus.Value switch {
                        _ => RemoteStartResult.OutOfService(),
                    };
                }

            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }

            result ??= RemoteStartResult.Error();


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
                DebugX.LogException(e, nameof(ChargingPool) + "." + nameof(OnRemoteStartResponse));
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
                       CancellationToken      CancellationToken      = default,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            RemoteStopResult? result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopRequest?.Invoke(startTime,
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
                DebugX.LogException(e, nameof(ChargingPool) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (TryGetChargingSessionById(SessionId, out var chargingSession) &&
                       ((chargingSession.EVSEId.           HasValue && TryGetChargingStationByEVSEId(chargingSession.EVSEId.           Value, out var chargingStation)) ||
                        (chargingSession.ChargingStationId.HasValue && TryGetChargingStationById    (chargingSession.ChargingStationId.Value, out     chargingStation))))
                    {

                        if (chargingStation is not null)
                            result = await chargingStation.
                                              RemoteStop(SessionId,
                                                         ReservationHandling,
                                                         ProviderId,
                                                         RemoteAuthentication,

                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout);

                    }

                    if (result == null)
                    {
                        DebugX.Log("Invalid charging session at charging pool '" + Id + "': " + SessionId);
                        result = RemoteStopResult.InvalidSessionId(SessionId);
                    }

                }
                else
                {
                    result = AdminStatus.Value switch {
                        _ => RemoteStopResult.OutOfService(SessionId),
                    };
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
                                             EndTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(ChargingPool) + "." + nameof(OnRemoteStopResponse));
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

                if (Session.ChargingPool == null)
                {
                    Session.ChargingPool    = this;
                    Session.ChargingPoolId  = Id;
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


        #region ToJSON(Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging pool.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public JObject ToJSON(Boolean                                             Embedded                          = false,
                              InfoStatus                                          ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                                          ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                                          ExpandChargingStationIds          = InfoStatus.Expanded,
                              InfoStatus                                          ExpandEVSEIds                     = InfoStatus.Hidden,
                              InfoStatus                                          ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                          ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<IChargingPool>?     CustomChargingPoolSerializer      = null,
                              CustomJObjectSerializerDelegate<IChargingStation>?  CustomChargingStationSerializer   = null,
                              CustomJObjectSerializerDelegate<IEVSE>?             CustomEVSESerializer              = null)
        {

            try
            {

                var json = JSONObject.Create(

                               new JProperty("@id", Id.ToString()),

                               !Embedded
                                   ? new JProperty("@context",     JSONLDContext)
                                   : null,

                               Name.       IsNeitherNullNorEmpty()
                                   ? new JProperty("name",         Name.ToJSON())
                                   : null,

                               Description.IsNeitherNullNorEmpty()
                                   ? new JProperty("description",  Description.ToJSON())
                                   : null,

                               ((!Embedded || DataSource != Operator.DataSource) && DataSource is not null)
                                   ? new JProperty("dataSource",   DataSource)
                                   : null,

                               ExpandDataLicenses.Switch(
                                   () => new JProperty("dataLicenseIds",  new JArray(DataLicenses.SafeSelect(license => license.Id.ToString()))),
                                   () => new JProperty("dataLicenses",    DataLicenses.ToJSON())),


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

                               GeoLocation.HasValue
                                   ? new JProperty("geoLocation",          GeoLocation.Value.  ToJSON())
                                   : null,

                               Address is not null
                                   ? new JProperty("address",              Address.            ToJSON())
                                   : null,

                               AuthenticationModes.Any()
                                   ? new JProperty("authenticationModes",  AuthenticationModes.ToJSON())
                                   : null,

                               HotlinePhoneNumber.HasValue
                                   ? new JProperty("hotlinePhoneNumber",   HotlinePhoneNumber. ToString())
                                   : null,

                               OpeningTimes is not null
                                   ? new JProperty("openingTimes",         OpeningTimes.       ToJSON())
                                   : null,


                               ExpandChargingStationIds != InfoStatus.Hidden && ChargingStations.Any()
                                   ? ExpandChargingStationIds.Switch(

                                         () => new JProperty("chargingStationIds",  ChargingStationIds().
                                                                                                 OrderBy(stationId => stationId).
                                                                                                 Select (stationId => stationId.ToString())),

                                         () => new JProperty("chargingStations",    ChargingStations.
                                                                                                 OrderBy(station   => station.Id).
                                                                                                 ToJSON (Embedded:                         true,
                                                                                                         ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                         ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                         ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                         ExpandEVSEIds:                    InfoStatus.Expanded,
                                                                                                         ExpandBrandIds:                   InfoStatus.ShowIdOnly,
                                                                                                         ExpandDataLicenses:               InfoStatus.Hidden,
                                                                                                         CustomChargingStationSerializer:  CustomChargingStationSerializer,
                                                                                                         CustomEVSESerializer:             CustomEVSESerializer)))

                                   : null,


                               ExpandEVSEIds != InfoStatus.Hidden && EVSEs.Any()
                                   ? ExpandEVSEIds.Switch(

                                         () => new JProperty("EVSEIds",
                                                             new JArray(EVSEIds().
                                                                                                 OrderBy(evseId => evseId).
                                                                                                 Select (evseId => evseId.ToString()))),

                                         () => new JProperty("EVSEs",
                                                             new JArray(EVSEs.
                                                                                                 OrderBy(evse   => evse).
                                                                                                 ToJSON (Embedded:                         true,
                                                                                                         ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                         ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                         ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                         ExpandChargingStationId:          InfoStatus.Hidden,
                                                                                                         ExpandBrandIds:                   InfoStatus.ShowIdOnly,
                                                                                                         ExpandDataLicenses:               InfoStatus.Hidden,
                                                                                                         CustomEVSESerializer:             CustomEVSESerializer))))

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

                return CustomChargingPoolSerializer is not null
                           ? CustomChargingPoolSerializer(this, json)
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


        #region UpdateWith(OtherChargingPool)

        /// <summary>
        /// Update this charging pool with the data of the other charging pool.
        /// </summary>
        /// <param name="OtherChargingPool">Another charging pool.</param>
        public ChargingPool UpdateWith(ChargingPool OtherChargingPool)
        {

            Name.                   Set(OtherChargingPool.Name);
            Description.            Set(OtherChargingPool.Description);

            Brands.             Replace(OtherChargingPool.Brands);
            UIFeatures.         Replace(OtherChargingPool.UIFeatures);
            AuthenticationModes.Replace(OtherChargingPool.AuthenticationModes);
            PaymentOptions.     Replace(OtherChargingPool.PaymentOptions);
            PhotoURLs.          Replace(OtherChargingPool.PhotoURLs);
            Features.           Replace(OtherChargingPool.Features);
            Facilities.         Replace(OtherChargingPool.Facilities);

            LocationLanguage          = OtherChargingPool.LocationLanguage;
            Address                   = OtherChargingPool.Address;
            GeoLocation               = OtherChargingPool.GeoLocation;
            EntranceAddress           = OtherChargingPool.EntranceAddress;
            EntranceLocation          = OtherChargingPool.EntranceLocation;
            ArrivalInstructions.    Set(OtherChargingPool.ArrivalInstructions);
            OpeningTimes              = OtherChargingPool.OpeningTimes;
            Accessibility             = OtherChargingPool.Accessibility;

            HotlinePhoneNumber        = OtherChargingPool.HotlinePhoneNumber;
            GridConnection            = OtherChargingPool.GridConnection;
            ExitAddress               = OtherChargingPool.ExitAddress;
            ExitLocation              = OtherChargingPool.ExitLocation;

            if (OtherChargingPool.AdminStatus.Timestamp > AdminStatus.Timestamp)
                AdminStatus           = OtherChargingPool.AdminStatus;

            if (OtherChargingPool.Status.Timestamp > Status.Timestamp)
                Status                = OtherChargingPool.Status;

            return this;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPool1, ChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPool1">A charging pool.</param>
        /// <param name="ChargingPool2">Another charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPool ChargingPool1,
                                           ChargingPool ChargingPool2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingPool1, ChargingPool2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingPool1 is null || ChargingPool2 is null)
                return false;

            return ChargingPool1.Equals(ChargingPool2);

        }

        #endregion

        #region Operator != (ChargingPool1, ChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPool1">A charging pool.</param>
        /// <param name="ChargingPool2">Another charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPool ChargingPool1,
                                           ChargingPool ChargingPool2)

            => !(ChargingPool1 == ChargingPool2);

        #endregion

        #region Operator <  (ChargingPool1, ChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPool1">A charging pool.</param>
        /// <param name="ChargingPool2">Another charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPool ChargingPool1,
                                          ChargingPool ChargingPool2)
        {

            if (ChargingPool1 is null)
                throw new ArgumentNullException(nameof(ChargingPool1), "The given ChargingPool1 must not be null!");

            return ChargingPool1.CompareTo(ChargingPool2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPool1, ChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPool1">A charging pool.</param>
        /// <param name="ChargingPool2">Another charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPool ChargingPool1,
                                           ChargingPool ChargingPool2)

            => !(ChargingPool1 > ChargingPool2);

        #endregion

        #region Operator >  (ChargingPool1, ChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPool1">A charging pool.</param>
        /// <param name="ChargingPool2">Another charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPool ChargingPool1,
                                          ChargingPool ChargingPool2)
        {

            if (ChargingPool1 is null)
                throw new ArgumentNullException(nameof(ChargingPool1), "The given ChargingPool1 must not be null!");

            return ChargingPool1.CompareTo(ChargingPool2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPool1, ChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPool1">A charging pool.</param>
        /// <param name="ChargingPool2">Another charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPool ChargingPool1,
                                           ChargingPool ChargingPool2)

            => !(ChargingPool1 < ChargingPool2);

        #endregion

        #endregion

        #region IComparable<ChargingPool> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is ChargingPool chargingPool
                   ? CompareTo(chargingPool)
                   : throw new ArgumentException("The given object is not a charging pool!", nameof(Object));

        #endregion

        #region CompareTo(ChargingPool)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPool">An ChargingPool to compare with.</param>
        public Int32 CompareTo(ChargingPool? ChargingPool)

            => ChargingPool is not null
                   ? Id.CompareTo(ChargingPool.Id)
                   : throw new ArgumentException("The given object is not a ChargingPool!", nameof(ChargingPool));

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IChargingPool">An IChargingPool to compare with.</param>
        public Int32 CompareTo(IChargingPool? IChargingPool)

            => IChargingPool is not null
                   ? Id.CompareTo(IChargingPool.Id)
                   : throw new ArgumentException("The given object is not a IChargingPool!", nameof(IChargingPool));

        #endregion

        #endregion

        #region IEquatable<ChargingPool> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPool chargingPool &&
                   Equals(chargingPool);

        #endregion

        #region Equals(ChargingPool)

        /// <summary>
        /// Compares two charging pools for equality.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPool? ChargingPool)

            => ChargingPool is not null &&
               Id.Equals(ChargingPool.Id);

        /// <summary>
        /// Compares two charging pools for equality.
        /// </summary>
        /// <param name="IChargingPool">A charging pool to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IChargingPool? IChargingPool)

            => IChargingPool is not null &&
               Id.Equals(IChargingPool.Id);

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
