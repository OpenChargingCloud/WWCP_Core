/*
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

using System.Diagnostics;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using cloud.charging.open.protocols.WWCP.Net.IO.JSON;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering charging pools.
    /// </summary>
    /// <param name="ChargingPool">A charging pool to include.</param>
    public delegate Boolean IncludeChargingPoolDelegate(ChargingPool ChargingPool);


    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging pool
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingPool : AEMobilityEntity<ChargingPool_Id,
                                                 ChargingPoolAdminStatusTypes,
                                                 ChargingPoolStatusTypes>,
                                IEquatable<ChargingPool>, IComparable<ChargingPool>,
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
        /// All brands registered for this charging pool.
        /// </summary>
        [Optional, SlowData]
        public EntityHashSet<ChargingPool, Brand_Id, Brand>  Brands          { get; }

        /// <summary>
        /// The license of the charging pool data.
        /// </summary>
        [Mandatory, SlowData]
        public ReactiveSet<DataLicense>                      DataLicenses    { get; }


        #region LocationLanguage

        private Languages? _LocationLanguage;

        /// <summary>
        /// The official language at this charging pool.
        /// </summary>
        [Optional]
        public Languages? LocationLanguage
        {

            get
            {
                return _LocationLanguage;
            }

            set
            {

                if (_LocationLanguage != value)
                {

                    if (value == null)
                        DeleteProperty(ref _LocationLanguage);

                    else
                        SetProperty(ref _LocationLanguage, value);

                }

            }

        }

        #endregion

        #region Address

        private Address _Address;

        /// <summary>
        /// The address of this charging pool.
        /// </summary>
        [Optional]
        public Address Address
        {

            get
            {
                return _Address;
            }

            set
            {

                if (_Address != value)
                {

                    if (value == null)
                        DeleteProperty(ref _Address);

                    else
                        SetProperty(ref _Address, value);

                    // Delete inherited addresses
                    chargingStations.ForEach(station => station.Address = null);

                }

            }

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate? _GeoLocation;

        /// <summary>
        /// The geographical location of this charging pool.
        /// </summary>
        [Optional]
        public GeoCoordinate? GeoLocation
        {

            get
            {
                return _GeoLocation;
            }

            set
            {

                if (_GeoLocation != value)
                {

                    if (value == null)
                        DeleteProperty(ref _GeoLocation);

                    else
                        SetProperty(ref _GeoLocation, value);

                    // Delete inherited geo locations
                    chargingStations.ForEach(station => station.GeoLocation = null);

                }

            }

        }

        #endregion

        #region EntranceAddress

        private Address _EntranceAddress;

        /// <summary>
        /// The address of the entrance to this charging pool.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address EntranceAddress
        {

            get
            {
                return _EntranceAddress;
            }

            set
            {

                if (_EntranceAddress != value)
                {

                    if (value == null)
                        DeleteProperty(ref _EntranceAddress);

                    else
                        SetProperty(ref _EntranceAddress, value);

                    // Delete inherited entrance addresses
                    chargingStations.ForEach(station => station.EntranceAddress = null);

                }

            }

        }

        #endregion

        #region EntranceLocation

        private GeoCoordinate? _EntranceLocation;

        /// <summary>
        /// The geographical location of the entrance to this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate? EntranceLocation
        {

            get
            {
                return _EntranceLocation;
            }

            set
            {

                if (_EntranceLocation != value)
                {

                    if (value == null)
                        DeleteProperty(ref _EntranceLocation);

                    else
                        SetProperty(ref _EntranceLocation, value);

                    // Delete inherited entrance locations
                    chargingStations.ForEach(station => station.EntranceLocation = null);

                }

            }

        }

        #endregion

        #region ArrivalInstructions

        private I18NString arrivalInstructions;

        /// <summary>
        /// An optional (multi-language) description of how to find the charging pool.
        /// </summary>
        [Optional]
        public I18NString ArrivalInstructions
        {

            get
            {
                return arrivalInstructions;
            }

            set
            {

                if (value is null)
                    DeleteProperty(ref arrivalInstructions);

                else if (value != arrivalInstructions)
                {
                    SetProperty(ref arrivalInstructions, value);
                }

            }

        }

        #endregion

        #region OpeningTimes

        private OpeningTimes openingTimes;

        /// <summary>
        /// The opening times of this charging pool.
        /// </summary>
        [Optional]
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

        #region UIFeatures

        private UIFeatures? _UIFeatures;

        /// <summary>
        /// The user interface features of the charging station.
        /// </summary>
        [Optional]
        public UIFeatures? UIFeatures
        {

            get
            {
                return _UIFeatures;
            }

            set
            {

                if (_UIFeatures != value)
                {

                    if (value == null)
                        DeleteProperty(ref _UIFeatures);

                    else
                        SetProperty(ref _UIFeatures, value);

                    // Delete inherited user interface features
                    chargingStations.ForEach(station => station.UIFeatures = null);

                }

            }

        }

        #endregion

        #region AuthenticationModes

        private ReactiveSet<AuthenticationModes> _AuthenticationModes;

        public ReactiveSet<AuthenticationModes> AuthenticationModes
        {

            get
            {
                return _AuthenticationModes;
            }

            set
            {

                if (value == null)
                    value = new ReactiveSet<AuthenticationModes>();

                if (_AuthenticationModes != value)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _AuthenticationModes);

                    else
                    {

                        if (_AuthenticationModes == null)
                            _AuthenticationModes = new ReactiveSet<AuthenticationModes>();

                        SetProperty(ref _AuthenticationModes, _AuthenticationModes.Set(value));

                    }

                    // Delete inherited authentication modes
                    chargingStations.ForEach(station => station.AuthenticationModes = null);

                }

            }

        }

        #endregion

        #region PaymentOptions

        private ReactiveSet<PaymentOptions> _PaymentOptions;

        [Mandatory]
        public ReactiveSet<PaymentOptions> PaymentOptions
        {

            get
            {
                return _PaymentOptions;
            }

            set
            {

                if (value == null)
                    value = new ReactiveSet<PaymentOptions>();

                if (_PaymentOptions != value)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _PaymentOptions);

                    else
                    {

                        if (_PaymentOptions == null)
                            _PaymentOptions = new ReactiveSet<PaymentOptions>();

                        SetProperty(ref _PaymentOptions, _PaymentOptions.Set(value));

                    }

                    // Delete inherited payment options
                    chargingStations.ForEach(station => station.PaymentOptions = null);

                }

            }

        }

        #endregion

        #region Accessibility

        private AccessibilityTypes? _Accessibility;

        [Optional]
        public AccessibilityTypes? Accessibility
        {

            get
            {
                return _Accessibility;
            }

            set
            {

                if (_Accessibility != value)
                {

                    if (value == null)
                        DeleteProperty(ref _Accessibility);

                    else
                        SetProperty(ref _Accessibility, value);

                    // Delete inherited accessibilities
                    chargingStations.ForEach(station => station.Accessibility = null);

                }

            }

        }

        #endregion

        #region PhotoURIs

        private ReactiveSet<String> _PhotoURIs;

        /// <summary>
        /// URIs of photos of this charging pool.
        /// </summary>
        [Optional]
        public ReactiveSet<String> PhotoURIs
        {

            get
            {
                return _PhotoURIs;
            }

            set
            {

                if (value == null)
                    value = new ReactiveSet<String>();

                if (_PhotoURIs != value)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _PhotoURIs);

                    else
                        SetProperty(ref _PhotoURIs, _PhotoURIs.Set(value));

                    // Delete inherited photo uris
                    chargingStations.ForEach(station => station.PhotoURIs = null);

                }

            }

        }

        #endregion

        #region HotlinePhoneNumber

        private I18NString _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the charging station operator hotline.
        /// </summary>
        [Optional]
        public I18NString HotlinePhoneNumber
        {

            get
            {
                return _HotlinePhoneNumber;
            }

            set
            {

                if (value == null)
                    value = new I18NString();

                if (_HotlinePhoneNumber != value)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _HotlinePhoneNumber);

                    else
                        SetProperty(ref _HotlinePhoneNumber, value);

                    // Delete inherited HotlinePhoneNumbers
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


        #region ExitAddress

        private Address _ExitAddress;

        /// <summary>
        /// The address of the exit of this charging pool.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address ExitAddress
        {

            get
            {
                return _ExitAddress;
            }

            set
            {

                if (_ExitAddress != value)
                {

                    if (value == null)
                        DeleteProperty(ref _ExitAddress);

                    else
                        SetProperty(ref _ExitAddress, value);

                    // Delete inherited exit addresses
                    chargingStations.ForEach(station => station.ExitAddress = null);

                }

            }

        }

        #endregion

        #region ExitLocation

        private GeoCoordinate? _ExitLocation;

        /// <summary>
        /// The geographical location of the exit of this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate? ExitLocation
        {

            get
            {
                return _ExitLocation;
            }

            set
            {

                if (_ExitLocation != value)
                {

                    if (value == null)
                        DeleteProperty(ref _ExitLocation);

                    else
                        SetProperty(ref _ExitLocation, value);

                    // Delete inherited exit locations
                    chargingStations.ForEach(station => station.ExitLocation = null);

                }

            }

        }

        #endregion


        #region IsHubjectCompatible

        [Optional]
        public Partly IsHubjectCompatible

            => PartlyHelper.Generate(chargingStations.Select(station => station.IsHubjectCompatible));

        #endregion

        #region DynamicInfoAvailable

        [Optional]
        public Partly DynamicInfoAvailable

            => PartlyHelper.Generate(chargingStations.Select(station => station.DynamicInfoAvailable));

        #endregion


        #region StatusAggregationDelegate

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated charging stations.
        /// </summary>
        public Func<ChargingStationStatusReport, ChargingPoolStatusTypes> StatusAggregationDelegate { get; set; }

        #endregion


        /// <summary>
        /// The charging station operator of this charging pool.
        /// </summary>
        [Optional]
        public ChargingStationOperator  Operator       { get; }

        /// <summary>
        /// The charging station sub operator of this charging pool.
        /// </summary>
        [Optional]
        public ChargingStationOperator  SubOperator    { get; }

        #endregion

        #region Links

        /// <summary>
        /// The roaming network of this charging pool.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork RoamingNetwork
            => Operator?.RoamingNetwork;

        /// <summary>
        /// The remote charging pool.
        /// </summary>
        [Optional]
        public IRemoteChargingPool  RemoteChargingPool    { get; }

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

            this.Operator                          = Operator;

            this.Brands                            = new EntityHashSet<ChargingPool, Brand_Id, Brand>(this);
            //this.Brands.OnSetChanged              += (timestamp, sender, newItems, oldItems) => {

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


            this.chargingStations            = new EntityHashSet<ChargingPool, ChargingStation_Id, ChargingStation>(this);
            //this.evses.OnSetChanged               += (timestamp, reactiveSet, newItems, oldItems) =>
            //{

            //    PropertyChanged("ChargingStations",
            //                    oldItems,
            //                    newItems);

            //};

            #endregion

            #region Init events

            // ChargingPool events
            this.ChargingStationAddition  = new VotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval   = new VotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition             = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval              = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events

            #endregion

            Configurator?.Invoke(this);

            #region Link events

            this.OnPropertyChanged += UpdateData;

            this.adminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this.statusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

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
            if (OnDataChangedLocal is not null)
                await OnDataChangedLocal(Timestamp,
                                         EventTrackingId,
                                         Sender as ChargingPool,
                                         PropertyName,
                                         OldValue,
                                         NewValue);

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                                  Timestamp,
                                              EventTracking_Id                          EventTrackingId,
                                              Timestamped<ChargingPoolAdminStatusTypes>  OldStatus,
                                              Timestamped<ChargingPoolAdminStatusTypes>  NewStatus)
        {

            var OnAdminStatusChangedLocal = OnAdminStatusChanged;
            if (OnAdminStatusChangedLocal is not null)
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
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                             Timestamp,
                                         EventTracking_Id                     EventTrackingId,
                                         Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                         Timestamped<ChargingPoolStatusTypes>  NewStatus)
        {

            var OnStatusChangedLocal = OnStatusChanged;
            if (OnStatusChangedLocal is not null)
                await OnStatusChangedLocal(Timestamp,
                                           EventTrackingId,
                                           this,
                                           OldStatus,
                                           NewStatus);

        }

        #endregion

        #endregion

        #region Charging stations

        #region ChargingStations

        private readonly EntityHashSet<ChargingPool, ChargingStation_Id, ChargingStation> chargingStations;

        /// <summary>
        /// Return all charging stations registered within this charing pool.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations

            => chargingStations;

        #endregion

        #region ChargingStationIds        (IncludeStations = null)

        /// <summary>
        /// Return an enumeration of all charging station identifications.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate IncludeStations = null)

            => IncludeStations == null

                   ? chargingStations.
                         Select    (station => station.Id)

                   : chargingStations.
                         Where     (station => IncludeStations(station)).
                         Select    (station => station.Id);

        #endregion

        #region ChargingStationAdminStatus(IncludeStations = null)

        /// <summary>
        /// Return an enumeration of all charging station admin status.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStationAdminStatus> ChargingStationAdminStatus(IncludeChargingStationDelegate IncludeStations = null)

            => IncludeStations == null

                   ? chargingStations.
                         Select    (station => new ChargingStationAdminStatus(station.Id, station.AdminStatus))

                   : chargingStations.
                         Where     (station => IncludeStations(station)).
                         Select    (station => new ChargingStationAdminStatus(station.Id, station.AdminStatus));

        #endregion

        #region ChargingStationStatus     (IncludeStations = null)

        /// <summary>
        /// Return an enumeration of all charging station status.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStationStatus> ChargingStationStatus(IncludeChargingStationDelegate IncludeStations = null)

            => IncludeStations == null

                   ? chargingStations.
                         Select    (station => new ChargingStationStatus(station.Id, station.Status))

                   : chargingStations.
                         Where     (station => IncludeStations(station)).
                         Select    (station => new ChargingStationStatus(station.Id, station.Status));

        #endregion


        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationAddition

            => ChargingStationAddition;

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationRemoval

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
        public ChargingStation CreateChargingStation(ChargingStation_Id                             Id,
                                                     I18NString?                                    Name                           = null,
                                                     I18NString?                                    Description                    = null,
                                                     Action<ChargingStation>?                       Configurator                   = null,
                                                     RemoteChargingStationCreatorDelegate?          RemoteChargingStationCreator   = null,
                                                     Timestamped<ChargingStationAdminStatusTypes>?  InitialAdminStatus             = null,
                                                     Timestamped<ChargingStationStatusTypes>?       InitialStatus                  = null,
                                                     UInt16?                                        MaxAdminStatusListSize         = null,
                                                     UInt16?                                        MaxStatusListSize              = null,
                                                     Action<ChargingStation>?                       OnSuccess                      = null,
                                                     Action<ChargingPool, ChargingStation_Id>?      OnError                        = null)
        {

            #region Initial checks

            if (chargingStations.ContainsId(Id))
            {
                if (OnError == null)
                    throw new ChargingStationAlreadyExistsInPool(this, Id);
                else
                    OnError?.Invoke(this, Id);
            }

            if (Operator.Id != Id.OperatorId)
                throw new InvalidChargingStationOperatorId(this,
                                                           Id.OperatorId);

            #endregion

            var _ChargingStation = new ChargingStation(Id,
                                                       this,
                                                       Name,
                                                       Description,
                                                       Configurator,
                                                       RemoteChargingStationCreator,
                                                       InitialAdminStatus,
                                                       InitialStatus,
                                                       MaxAdminStatusListSize,
                                                       MaxStatusListSize);


            if (ChargingStationAddition.SendVoting(Timestamp.Now, this, _ChargingStation) &&
                chargingStations.TryAdd(_ChargingStation))
            {

                _ChargingStation.OnDataChanged                  += UpdateChargingStationData;
                _ChargingStation.OnStatusChanged                += UpdateChargingStationStatus;
                _ChargingStation.OnAdminStatusChanged           += UpdateChargingStationAdminStatus;

                _ChargingStation.OnEVSEAddition.OnVoting        += (timestamp, station, evse, vote)  => EVSEAddition.SendVoting      (timestamp, station, evse, vote);
                _ChargingStation.OnEVSEAddition.OnNotification  += (timestamp, station, evse)        => EVSEAddition.SendNotification(timestamp, station, evse);
                _ChargingStation.OnEVSEDataChanged              += UpdateEVSEData;
                _ChargingStation.OnEVSEStatusChanged            += UpdateEVSEStatus;
                _ChargingStation.OnEVSEAdminStatusChanged       += UpdateEVSEAdminStatus;
                _ChargingStation.OnEVSERemoval. OnVoting        += (timestamp, station, evse, vote)  => EVSERemoval.SendVoting       (timestamp, station, evse, vote);
                _ChargingStation.OnEVSERemoval. OnNotification  += (timestamp, station, evse)        => EVSERemoval.SendNotification (timestamp, station, evse);


                _ChargingStation.OnNewReservation               += SendNewReservation;
                _ChargingStation.OnReservationCanceled          += SendReservationCanceled;
                _ChargingStation.OnNewChargingSession           += SendNewChargingSession;
                _ChargingStation.OnNewChargeDetailRecord        += SendNewChargeDetailRecord;


                if (RemoteChargingStationCreator != null)
                {

                    _ChargingStation.RemoteChargingStation.OnNewReservation += SendNewReservation;

                    _ChargingStation.RemoteChargingStation.OnNewReservation += (a, b, reservation) => {

                        var __EVSE = GetEVSEById(reservation.EVSEId.Value);

                        //__EVSE.Reservation = reservation;

                    };

                    _ChargingStation.RemoteChargingStation.OnNewChargingSession += (a, b, session) => {

                        var __EVSE = GetEVSEById(session.EVSEId.Value);

                        //__EVSE.ChargingSession = session;

                    };

                    _ChargingStation.RemoteChargingStation.OnNewChargeDetailRecord += (a, b, cdr) => {

                        var __EVSE = GetEVSEById(cdr.EVSEId.Value);

                        __EVSE.SendNewChargeDetailRecord(Timestamp.Now, this, cdr);

                    };


                    _ChargingStation.RemoteChargingStation.OnReservationCanceled += _ChargingStation.SendReservationCanceled;

                    _ChargingStation.RemoteChargingStation.OnEVSEStatusChanged += (Timestamp,
                                                                                   EventTrackingId,
                                                                                   EVSE,
                                                                                   OldStatus,
                                                                                   NewStatus)

                        => _ChargingStation.UpdateEVSEStatus(Timestamp,
                                                             EventTrackingId,
                                                             GetEVSEById(EVSE.Id),
                                                             OldStatus,
                                                             NewStatus);

                }

                OnSuccess?.Invoke(_ChargingStation);
                ChargingStationAddition.SendNotification(Timestamp.Now, this, _ChargingStation);

                return _ChargingStation;

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
        public ChargingStation? CreateOrUpdateChargingStation(ChargingStation_Id                             Id,
                                                              I18NString?                                    Name                           = null,
                                                              I18NString?                                    Description                    = null,
                                                              Action<ChargingStation>?                       Configurator                   = null,
                                                              RemoteChargingStationCreatorDelegate?          RemoteChargingStationCreator   = null,
                                                              Timestamped<ChargingStationAdminStatusTypes>?  InitialAdminStatus             = null,
                                                              Timestamped<ChargingStationStatusTypes>?       InitialStatus                  = null,
                                                              UInt16?                                        MaxAdminStatusListSize         = null,
                                                              UInt16?                                        MaxStatusListSize              = null,
                                                              Action<ChargingStation>?                       OnSuccess                      = null,
                                                              Action<ChargingPool, ChargingStation_Id>?      OnError                        = null)
        {

            lock (chargingStations)
            {

                #region Initial checks

                if (Operator.Id != Id.OperatorId)
                    throw new InvalidChargingStationOperatorId(this,
                                                               Id.OperatorId);

                #endregion

                #region If the charging pool identification is new/unknown: Call CreateChargingPool(...)

                if (!chargingStations.ContainsId(Id))
                    return CreateChargingStation(Id,
                                                 Name,
                                                 Description,
                                                 Configurator,
                                                 RemoteChargingStationCreator,
                                                 InitialAdminStatus,
                                                 InitialStatus,
                                                 MaxAdminStatusListSize ?? ChargingStation.DefaultMaxAdminStatusScheduleSize,
                                                 MaxStatusListSize      ?? ChargingStation.DefaultMaxStatusScheduleSize,
                                                 OnSuccess,
                                                 OnError);

                #endregion


                var existingChargingStation = chargingStations.GetById(Id);

                // Merge existing charging station with new station data...

                if (existingChargingStation is not null)
                    return existingChargingStation.
                               UpdateWith(new ChargingStation(Id,
                                                              this,
                                                              Name,
                                                              Description,
                                                              Configurator,
                                                              null,
                                                              new Timestamped<ChargingStationAdminStatusTypes>(DateTime.MinValue, ChargingStationAdminStatusTypes.Operational),
                                                              new Timestamped<ChargingStationStatusTypes>     (DateTime.MinValue, ChargingStationStatusTypes.     Available)));

                return null;

            }

        }

        #endregion


        #region ContainsChargingStation(ChargingStation)

        /// <summary>
        /// Check if the given ChargingStation is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStationId(ChargingStation ChargingStation)

            => chargingStations.ContainsId(ChargingStation.Id);

        #endregion

        #region ContainsChargingStation(ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)

            => chargingStations.ContainsId(ChargingStationId);

        #endregion

        #region GetChargingStationById(ChargingStationId)

        public ChargingStation GetChargingStationById(ChargingStation_Id ChargingStationId)

            => chargingStations.GetById(ChargingStationId);

        #endregion

        #region TryGetChargingStationById(ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)

            => chargingStations.TryGet(ChargingStationId, out ChargingStation);

        #endregion

        #region RemoveChargingStation(ChargingStationId)

        public ChargingStation RemoveChargingStation(ChargingStation_Id ChargingStationId)
        {

            ChargingStation _ChargingStation = null;

            if (TryGetChargingStationById(ChargingStationId, out _ChargingStation))
            {

                if (ChargingStationRemoval.SendVoting(Timestamp.Now, this, _ChargingStation))
                {

                    if (chargingStations.TryRemove(ChargingStationId, out _ChargingStation))
                    {

                        ChargingStationRemoval.SendNotification(Timestamp.Now, this, _ChargingStation);

                        return _ChargingStation;

                    }

                }

            }

            return null;

        }

        #endregion

        #region TryRemoveChargingStation(ChargingStationId, out ChargingStation)

        public Boolean TryRemoveChargingStation(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {

            if (TryGetChargingStationById(ChargingStationId, out ChargingStation))
            {

                if (ChargingStationRemoval.SendVoting(Timestamp.Now, this, ChargingStation))
                {

                    if (chargingStations.TryRemove(ChargingStationId, out ChargingStation))
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
        public event OnChargingStationDataChangedDelegate         OnChargingStationDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationStatusChangedDelegate       OnChargingStationStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate  OnChargingStationAdminStatusChanged;

        #endregion


        #region (internal) UpdateChargingStationData        (Timestamp, EventTrackingId, ChargingStation, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateChargingStationData(DateTime          Timestamp,
                                                      EventTracking_Id  EventTrackingId,
                                                      ChargingStation   ChargingStation,
                                                      String            PropertyName,
                                                      Object            OldValue,
                                                      Object            NewValue)
        {

            var OnChargingStationDataChangedLocal = OnChargingStationDataChanged;
            if (OnChargingStationDataChangedLocal != null)
                await OnChargingStationDataChangedLocal(Timestamp,
                                                        EventTrackingId,
                                                        ChargingStation,
                                                        PropertyName,
                                                        OldValue,
                                                        NewValue);

        }

        #endregion

        #region (internal) UpdateChargingStationStatus(Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the curent status of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old charging station status.</param>
        /// <param name="NewStatus">The new charging station status.</param>
        internal async Task UpdateChargingStationStatus(DateTime                                 Timestamp,
                                                        EventTracking_Id                         EventTrackingId,
                                                        ChargingStation                          ChargingStation,
                                                        Timestamped<ChargingStationStatusTypes>  OldStatus,
                                                        Timestamped<ChargingStationStatusTypes>  NewStatus)
        {

            var OnChargingStationStatusChangedLocal = OnChargingStationStatusChanged;
            if (OnChargingStationStatusChangedLocal is not null)
                await OnChargingStationStatusChangedLocal(Timestamp,
                                                          EventTrackingId,
                                                          ChargingStation,
                                                          OldStatus,
                                                          NewStatus);

            if (StatusAggregationDelegate is not null)
            {
                statusSchedule.Insert(StatusAggregationDelegate(new ChargingStationStatusReport(chargingStations)),
                                      Timestamp);
            }

        }

        #endregion

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the curent admin status of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal async Task UpdateChargingStationAdminStatus(DateTime                                      Timestamp,
                                                             EventTracking_Id                              EventTrackingId,
                                                             ChargingStation                               ChargingStation,
                                                             Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                                             Timestamped<ChargingStationAdminStatusTypes>  NewStatus)
        {

            var OnChargingStationAdminStatusChangedLocal = OnChargingStationAdminStatusChanged;
            if (OnChargingStationAdminStatusChangedLocal != null)
                await OnChargingStationAdminStatusChangedLocal(Timestamp,
                                                               EventTrackingId,
                                                               ChargingStation,
                                                               OldStatus,
                                                               NewStatus);

        }

        #endregion


        #region IEnumerable<ChargingStation> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => chargingStations.GetEnumerator();

        public IEnumerator<ChargingStation> GetEnumerator()
            => chargingStations.GetEnumerator();

        #endregion


        #region TryGetChargingStationByEVSEId(EVSEId, out Station)

        public Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out ChargingStation Station)
        {

            foreach (var station in chargingStations)
            {

                if (station.TryGetEVSEById(EVSEId, out EVSE evse))
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

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSEAddition

            => EVSEAddition;

        #endregion

        #region EVSEs

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging pool.
        /// </summary>
        public IEnumerable<EVSE> EVSEs

            => chargingStations.
                   SelectMany(station => station.EVSEs);

        #endregion

        #region EVSEIds                (IncludeEVSEs = null)

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment
        /// (EVSEs) present within this charging pool.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate IncludeEVSEs = null)

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
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatusSchedule(IncludeEVSEDelegate IncludeEVSEs = null)

            => chargingStations.
                   SelectMany(station => station.EVSEAdminStatus(IncludeEVSEs));

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

        #region EVSEStatusSchedule     (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatusSchedule(IncludeEVSEDelegate IncludeEVSEs = null)

            => chargingStations.
                   SelectMany(station => station.EVSEStatus(IncludeEVSEs));

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

        public EVSE GetEVSEById(EVSE_Id EVSEId)

            => chargingStations.
                   SelectMany    (station => station.EVSEs).
                   FirstOrDefault(EVSE    => EVSE.Id == EVSEId);

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = chargingStations.
                       SelectMany    (station => station.EVSEs).
                       FirstOrDefault(_EVSE   => _EVSE.Id == EVSEId);

            return EVSE != null;

        }

        #endregion


        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate  OnEVSEAdminStatusChanged;

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

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSERemoval

            => EVSERemoval;

        #endregion


        #region (internal) UpdateEVSEData       (Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the static data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateEVSEData(DateTime          Timestamp,
                                           EventTracking_Id  EventTrackingId,
                                           EVSE              EVSE,
                                           String            PropertyName,
                                           Object            OldValue,
                                           Object            NewValue)
        {

            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal != null)
                await OnEVSEDataChangedLocal(Timestamp,
                                             EventTrackingId ?? EventTracking_Id.New,
                                             EVSE,
                                             PropertyName,
                                             OldValue,
                                             NewValue);

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the current admin status of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEAdminStatus(DateTime                           Timestamp,
                                                  EventTracking_Id                   EventTrackingId,
                                                  EVSE                               EVSE,
                                                  Timestamped<EVSEAdminStatusTypes>  OldStatus,
                                                  Timestamped<EVSEAdminStatusTypes>  NewStatus)
        {

            var OnEVSEAdminStatusChangedLocal = OnEVSEAdminStatusChanged;
            if (OnEVSEAdminStatusChangedLocal != null)
                await OnEVSEAdminStatusChangedLocal(Timestamp,
                                                    EventTrackingId ?? EventTracking_Id.New,
                                                    EVSE,
                                                    OldStatus,
                                                    NewStatus);

        }

        #endregion

        #region (internal) UpdateEVSEStatus     (Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                      Timestamp,
                                             EventTracking_Id              EventTrackingId,
                                             EVSE                          EVSE,
                                             Timestamped<EVSEStatusTypes>  OldStatus,
                                             Timestamped<EVSEStatusTypes>  NewStatus)
        {

            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal != null)
                await OnEVSEStatusChangedLocal(Timestamp,
                                               EventTrackingId ?? EventTracking_Id.New,
                                               EVSE,
                                               OldStatus,
                                               NewStatus);

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
        public IEnumerable<ChargingReservationCollection> Reservations
            => RoamingNetwork.ReservationsStore.
                   Where(reservation => reservation.First().ChargingPoolId == Id);

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservationCollection Reservation)
            => RoamingNetwork.ReservationsStore.TryGet(ReservationId, out Reservation);

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
        /// Reserve the possibility to charge at this charging pool.
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
                    EMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)


                => Reserve(ChargingLocation.FromChargingPoolId(Id),
                           ChargingReservationLevel.ChargingPool,
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
                    EMobilityProvider_Id?             ProviderId             = null,
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
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ReservationResult result = null;

            #endregion

            #region Send OnReserveRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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

                    if (RemoteChargingPool != null)
                    {

                        result = await RemoteChargingPool.
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

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                DebugX.LogException(e, nameof(ChargingPool) + "." + nameof(OnReserveResponse));
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
                              EMobilityProvider_Id?                  ProviderId         = null,

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ChargingReservation     canceledReservation  = null;
            CancelReservationResult result               = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                DebugX.LogException(e, nameof(ChargingPool) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (RemoteChargingPool != null)
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

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
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
                        ChargingProduct          ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)

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
                DebugX.LogException(e, nameof(ChargingStation) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if ((ChargingLocation.EVSEId.           HasValue && TryGetChargingStationByEVSEId(ChargingLocation.EVSEId.           Value, out ChargingStation chargingStation)) ||
                        (ChargingLocation.ChargingStationId.HasValue && TryGetChargingStationById    (ChargingLocation.ChargingStationId.Value, out                 chargingStation)))
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
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            #region Initial checks

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
                DebugX.LogException(e, nameof(ChargingPool) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (TryGetChargingSessionById(SessionId, out ChargingSession chargingSession) &&
                       ((chargingSession.EVSEId.           HasValue && TryGetChargingStationByEVSEId(chargingSession.EVSEId.           Value, out ChargingStation chargingStation)) ||
                        (chargingSession.ChargingStationId.HasValue && TryGetChargingStationById    (chargingSession.ChargingStationId.Value, out                 chargingStation))))
                    {

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
        public JObject ToJSON(Boolean                                            Embedded                          = false,
                              InfoStatus                                         ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandChargingStationIds          = InfoStatus.Expanded,
                              InfoStatus                                         ExpandEVSEIds                     = InfoStatus.Hidden,
                              InfoStatus                                         ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<ChargingPool>?     CustomChargingPoolSerializer      = null,
                              CustomJObjectSerializerDelegate<ChargingStation>?  CustomChargingStationSerializer   = null,
                              CustomJObjectSerializerDelegate<EVSE>?             CustomEVSESerializer              = null)
        {

            var JSON = JSONObject.Create(

                         new JProperty("@id", Id.ToString()),

                         !Embedded
                             ? new JProperty("@context", JSONLDContext)
                             : null,

                         Name.       IsNeitherNullNorEmpty()
                             ? new JProperty("name",        Name.ToJSON())
                             : null,

                         Description.IsNeitherNullNorEmpty()
                             ? new JProperty("description", Description.ToJSON())
                             : null,

                         ((!Embedded || DataSource != Operator.DataSource) && DataSource is not null)
                             ? new JProperty("dataSource", DataSource)
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

                         new JProperty("geoLocation",          GeoLocation.        ToJSON()),
                         new JProperty("address",              Address.            ToJSON()),

                         AuthenticationModes is not null && AuthenticationModes.Any()
                             ? new JProperty("authenticationModes",  AuthenticationModes.ToJSON())
                             : null,

                         HotlinePhoneNumber is not null && HotlinePhoneNumber.Any()
                             ? new JProperty("hotlinePhoneNumber",   HotlinePhoneNumber. ToJSON())
                             : null,

                         OpeningTimes is not null
                             ? new JProperty("openingTimes",         OpeningTimes.       ToJSON())
                             : null,


                         ExpandChargingStationIds != InfoStatus.Hidden && ChargingStations.Any()
                             ? ExpandChargingStationIds.Switch(

                                   () => new JProperty("chargingStationIds", ChargingStationIds().
                                                                                          OrderBy(stationId => stationId).
                                                                                          Select (stationId => stationId.ToString())),

                                   () => new JProperty("chargingStations",   ChargingStations.
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
                                                       new JArray(Brands.OrderBy(brand => brand).
                                                                         ToJSON (Embedded:                         true,
                                                                                 ExpandDataLicenses:               InfoStatus.ShowIdOnly))))

                             : null

                     );

            return CustomChargingPoolSerializer is not null
                       ? CustomChargingPoolSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region UpdateWith(OtherChargingPool)

        /// <summary>
        /// Update this charging pool with the data of the other charging pool.
        /// </summary>
        /// <param name="OtherChargingPool">Another charging pool.</param>
        public ChargingPool UpdateWith(ChargingPool OtherChargingPool)
        {

            Name.       Add(OtherChargingPool.Name);
            Description.Add(OtherChargingPool.Description);

            Brands.Clear();
            Brands.TryAdd(OtherChargingPool.Brands);

            LocationLanguage     = OtherChargingPool.LocationLanguage;
            Address              = OtherChargingPool.Address;
            GeoLocation          = OtherChargingPool.GeoLocation;
            EntranceAddress      = OtherChargingPool.EntranceAddress;
            EntranceLocation     = OtherChargingPool.EntranceLocation;
            ArrivalInstructions  = OtherChargingPool.ArrivalInstructions;
            OpeningTimes         = OtherChargingPool.OpeningTimes;
            UIFeatures           = OtherChargingPool.UIFeatures;

            if (AuthenticationModes == null && OtherChargingPool.AuthenticationModes != null)
                AuthenticationModes = new ReactiveSet<AuthenticationModes>(OtherChargingPool.AuthenticationModes);
            else if (AuthenticationModes != null)
                AuthenticationModes.Set(OtherChargingPool.AuthenticationModes);

            if (PaymentOptions == null && OtherChargingPool.PaymentOptions != null)
                PaymentOptions = new ReactiveSet<PaymentOptions>(OtherChargingPool.PaymentOptions);
            else if (PaymentOptions != null)
                PaymentOptions.Set(OtherChargingPool.PaymentOptions);

            Accessibility        = OtherChargingPool.Accessibility;

            if (PhotoURIs == null && OtherChargingPool.PhotoURIs != null)
                PhotoURIs = new ReactiveSet<String>(OtherChargingPool.PhotoURIs);
            else if (PhotoURIs != null)
                PhotoURIs.Set(OtherChargingPool.PhotoURIs);

            HotlinePhoneNumber   = OtherChargingPool.HotlinePhoneNumber;
            GridConnection       = OtherChargingPool.GridConnection;
            ExitAddress          = OtherChargingPool.ExitAddress;
            ExitLocation         = OtherChargingPool.ExitLocation;


            if (OtherChargingPool.AdminStatus.Timestamp > AdminStatus.Timestamp)
                AdminStatus = OtherChargingPool.AdminStatus;

            if (OtherChargingPool.Status.Timestamp > Status.Timestamp)
                Status      = OtherChargingPool.Status;

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
