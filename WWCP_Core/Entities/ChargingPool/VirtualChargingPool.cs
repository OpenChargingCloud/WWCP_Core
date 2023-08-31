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

using System.Collections;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP.Virtual
{

    /// <summary>
    /// Extension methods for virtual charging pools.
    /// </summary>
    public static class VirtualChargingPoolExtensions
    {

        #region AddVirtualChargingPool(this ChargingStationOperator, ChargingPoolId = null, ChargingPoolConfigurator = null, VirtualChargingPoolConfigurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create a new virtual charging pool.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="ChargingPoolId">The charging station identification for the charging station to be created.</param>
        /// <param name="ChargingPoolConfigurator">An optional delegate to configure the new (local) charging station.</param>
        /// <param name="VirtualChargingPoolConfigurator">An optional delegate to configure the new virtual charging station.</param>
        /// <param name="OnSuccess">An optional delegate for reporting success.</param>
        /// <param name="OnError">An optional delegate for reporting an error.</param>
        public static Task<AddChargingPoolResult> AddVirtualChargingPool(this IChargingStationOperator                                       ChargingStationOperator,

                                                                         ChargingPool_Id                                                     ChargingPoolId,
                                                                         I18NString?                                                         Name                              = null,
                                                                         I18NString?                                                         Description                       = null,

                                                                         Address?                                                            Address                           = null,
                                                                         GeoCoordinate?                                                      GeoLocation                       = null,
                                                                         Time_Zone?                                                          TimeZone                          = null,
                                                                         OpeningTimes?                                                       OpeningTimes                      = null,
                                                                         Boolean?                                                            ChargingWhenClosed                = null,
                                                                         AccessibilityTypes?                                                 Accessibility                     = null,
                                                                         Languages?                                                          LocationLanguage                  = null,
                                                                         PhoneNumber?                                                        HotlinePhoneNumber                = null,

                                                                         IEnumerable<Brand>?                                                 Brands                            = null,
                                                                         IEnumerable<RootCAInfo>?                                            MobilityRootCAs                   = null,

                                                                         Timestamped<ChargingPoolAdminStatusTypes>?                          InitialAdminStatus                = null,
                                                                         Timestamped<ChargingPoolStatusTypes>?                               InitialStatus                     = null,
                                                                         String                                                              EllipticCurve                     = "P-256",
                                                                         ECPrivateKeyParameters?                                             PrivateKey                        = null,
                                                                         PublicKeyCertificates?                                              PublicKeyCertificates             = null,
                                                                         TimeSpan?                                                           SelfCheckTimeSpan                 = null,
                                                                         UInt16?                                                             MaxAdminStatusScheduleSize        = null,
                                                                         UInt16?                                                             MaxStatusScheduleSize             = null,

                                                                         String?                                                             DataSource                        = null,
                                                                         DateTime?                                                           LastChange                        = null,

                                                                         JObject?                                                            CustomData                        = null,
                                                                         UserDefinedDictionary?                                              InternalData                      = null,

                                                                         Action<IChargingPool>?                                              ChargingPoolConfigurator          = null,
                                                                         Action<VirtualChargingPool>?                                        VirtualChargingPoolConfigurator   = null,

                                                                         Action<IChargingPool, EventTracking_Id>?                            OnSuccess                         = null,
                                                                         Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                           = null,

                                                                         Boolean                                                             SkipAddedNotifications            = false,
                                                                         Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds      = null,
                                                                         EventTracking_Id?                                                   EventTrackingId                   = null,
                                                                         User_Id?                                                            CurrentUserId                     = null)

            => ChargingStationOperator.AddChargingPool(

                   ChargingPoolId,
                   Name,
                   Description,

                   Address,
                   GeoLocation,
                   TimeZone,
                   OpeningTimes,
                   ChargingWhenClosed,
                   Accessibility,
                   LocationLanguage,
                   HotlinePhoneNumber,

                   Brands,
                   MobilityRootCAs,

                   InitialAdminStatus ?? ChargingPoolAdminStatusTypes.Operational,
                   InitialStatus      ?? ChargingPoolStatusTypes.Available,
                   MaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize,

                   DataSource,
                   LastChange,

                   CustomData,
                   InternalData,

                   ChargingPoolConfigurator,
                   newChargingPool => {

                       var virtualpool = new VirtualChargingPool(
                                             newChargingPool.Id,
                                             ChargingStationOperator.RoamingNetwork,
                                             newChargingPool.Name,
                                             newChargingPool.Description,
                                             InitialAdminStatus ?? ChargingPoolAdminStatusTypes.Operational,
                                             InitialStatus      ?? ChargingPoolStatusTypes.Available,

                                             Address,
                                             GeoLocation,
                                             TimeZone,
                                             OpeningTimes,
                                             ChargingWhenClosed,

                                             EllipticCurve,
                                             PrivateKey,
                                             PublicKeyCertificates,
                                             SelfCheckTimeSpan,
                                             MaxAdminStatusScheduleSize,
                                             MaxStatusScheduleSize
                                         );

                       VirtualChargingPoolConfigurator?.Invoke(virtualpool);

                       return virtualpool;

                   },

                   OnSuccess,
                   OnError,

                   SkipAddedNotifications,
                   AllowInconsistentOperatorIds,
                   EventTrackingId,
                   CurrentUserId
               );

        #endregion

    }


    /// <summary>
    /// A virtual charging pool for (internal) tests.
    /// </summary>
    public class VirtualChargingPool : ACryptoEMobilityEntity<ChargingPool_Id,
                                                              ChargingPoolAdminStatusTypes,
                                                              ChargingPoolStatusTypes>,
                                       IEquatable<VirtualChargingPool>, IComparable<VirtualChargingPool>, IComparable,
                                       IRemoteChargingPool
    {

        #region Data

        private readonly       Decimal   EPSILON                         = 0.01m;

        /// <summary>
        /// The default max size of the status history.
        /// </summary>
        public const           UInt16    DefaultMaxStatusScheduleSize        = 50;

        /// <summary>
        /// The default max size of the admin status history.
        /// </summary>
        public const           UInt16    DefaultMaxAdminStatusScheduleSize   = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly TimeSpan  MaxReservationDuration          = TimeSpan.FromMinutes(15);

        #endregion

        #region Properties

        /// <summary>
        /// The identification of the operator of this virtual EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator_Id    OperatorId
            => Id.OperatorId;

        /// <summary>
        /// The charging station operator of this charging pool.
        /// </summary>
        [Optional]
        public IChargingStationOperator?     Operator               { get; }

        /// <summary>
        /// The charging station sub operator of this charging pool.
        /// </summary>
        [Optional]
        public IChargingStationOperator?     SubOperator            { get; }


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

        #region TimeZone

        private Time_Zone? timeZone;

        /// <summary>
        /// The time zone of this charging pool.
        /// </summary>
        [Mandatory]
        public Time_Zone? TimeZone
        {

            get
            {
                return timeZone;
            }

            set
            {
                if (value != timeZone)
                {
                    SetProperty(ref timeZone, value);
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
        public ReactiveSet<ChargingPoolFeature>  Features                { get; }

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

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new virtual charging pool.
        /// </summary>
        public VirtualChargingPool(ChargingPool_Id                             Id,
                                   IRoamingNetwork                             RoamingNetwork,
                                   I18NString?                                 Name                         = null,
                                   I18NString?                                 Description                  = null,
                                   Timestamped<ChargingPoolAdminStatusTypes>?  InitialAdminStatus           = null,
                                   Timestamped<ChargingPoolStatusTypes>?       InitialStatus                = null,

                                   Address?                                    Address                      = null,
                                   GeoCoordinate?                              GeoLocation                  = null,
                                   Time_Zone?                                  TimeZone                     = null,
                                   OpeningTimes?                               OpeningTimes                 = null,
                                   Boolean?                                    ChargingWhenClosed           = null,

                                   String                                      EllipticCurve                = "P-256",
                                   ECPrivateKeyParameters?                     PrivateKey                   = null,
                                   PublicKeyCertificates?                      PublicKeyCertificates        = null,
                                   TimeSpan?                                   SelfCheckTimeSpan            = null,
                                   UInt16?                                     MaxAdminStatusScheduleSize   = null,
                                   UInt16?                                     MaxStatusScheduleSize        = null,

                                   String?                                     DataSource                   = null,
                                   DateTime?                                   LastChange                   = null,

                                   JObject?                                    CustomData                   = null,
                                   UserDefinedDictionary?                      InternalData                 = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,
                   InitialAdminStatus         ?? ChargingPoolAdminStatusTypes.Operational,
                   InitialStatus              ?? ChargingPoolStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Init data and properties

            this.chargingStations     = new HashSet<IChargingStation>();

            this.Address              = Address;
            this.geoLocation          = GeoLocation;
            this.timeZone             = TimeZone;
            this.openingTimes         = OpeningTimes ?? OpeningTimes.Open24Hours;
            this.ChargingWhenClosed   = ChargingWhenClosed;

            #endregion

            #region Setup crypto

            if (PrivateKey            is null &&
                PublicKeyCertificates is null)
            {

                var generator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
                generator.Init(new ECKeyGenerationParameters(ECSpec, new SecureRandom()));

                var  keyPair                = generator.GenerateKeyPair();
                this.PrivateKey             = keyPair.Private as ECPrivateKeyParameters;
                this.PublicKeyCertificates  = new PublicKeyCertificate(
                                                  PublicKey:           new PublicKeyLifetime(
                                                                           PublicKey:  keyPair.Public as ECPublicKeyParameters,
                                                                           NotBefore:  Timestamp.Now,
                                                                           NotAfter:   Timestamp.Now + TimeSpan.FromDays(365),
                                                                           Algorithm:  "P-256",
                                                                           Comment:    I18NString.Empty
                                                                       ),
                                                  Description:         I18NString.Create(Languages.en, "Auto-generated test keys for a virtual charging pool!"),
                                                  Operations:          JSONObject.Create(
                                                                           new JProperty("signMeterValues",  true),
                                                                           new JProperty("signCertificates",
                                                                               JSONObject.Create(
                                                                                   new JProperty("maxChilds", 2)
                                                                               ))
                                                                       ),
                                                  ChargingPoolId:      Id);

            }

            #endregion

            #region Link events

            this.adminStatusSchedule.OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                          => UpdateAdminStatus(timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            this.statusSchedule.     OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                          => UpdateStatus     (timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            #endregion

        }

        event OnChargingPoolDataChangedDelegate IChargingPool.OnDataChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnChargingPoolAdminStatusChangedDelegate IChargingPool.OnAdminStatusChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnChargingPoolStatusChangedDelegate IChargingPool.OnStatusChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        #endregion


        #region EVSEs...

        #region TryGetEVSEById(EVSEId, out RemoteEVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE? RemoteEVSE)
        {

            foreach (var station in chargingStations)
            {
                if (station is not null &&
                    station.TryGetEVSEById(EVSEId, out RemoteEVSE))
                {
                    return true;
                }
            }

            RemoteEVSE = null;
            return false;

        }

        #endregion

        #endregion

        #region Charging stations...

        #region Stations

        private readonly HashSet<IChargingStation> chargingStations;

        /// <summary>
        /// All registered charging stations.
        /// </summary>
        public IEnumerable<IChargingStation> ChargingStations
            => chargingStations;

        #endregion

        #region AddVirtualStation(ChargingStationId, ..., Configurator = null, OnSuccess = null, OnError = null, ...)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the charging station.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        public async Task<AddChargingStationResult> AddVirtualStation(ChargingStation_Id                                   ChargingStationId,
                                                                      I18NString?                                          Name                         = null,
                                                                      I18NString?                                          Description                  = null,
                                                                      ChargingStationAdminStatusTypes?                     InitialAdminStatus           = null,
                                                                      ChargingStationStatusTypes?                          InitialStatus                = null,
                                                                      String?                                              EllipticCurve                = null,
                                                                      ECPrivateKeyParameters?                              PrivateKey                   = null,
                                                                      PublicKeyCertificates?                               PublicKeyCertificates        = null,
                                                                      TimeSpan?                                            SelfCheckTimeSpan            = null,
                                                                      Action<VirtualChargingStation>?                      Configurator                 = null,
                                                                      Action<VirtualChargingStation>?                      OnSuccess                    = null,
                                                                      Action<VirtualChargingStation, ChargingStation_Id>?  OnError                      = null,
                                                                      UInt16?                                              MaxAdminStatusScheduleSize   = null,
                                                                      UInt16?                                              MaxStatusScheduleSize        = null)
        {

            #region Initial checks

            if (chargingStations.Any(station => station.Id == ChargingStationId))
            {
                throw new Exception("StationAlreadyExistsInPool");
                //if (OnError == null)
                //    throw new ChargingStationAlreadyExistsInStation(this.ChargingStation, ChargingStation.Id);
                //else
                //    OnError?.Invoke(this, ChargingStation.Id);
            }

            #endregion

            var Now             = Timestamp.Now;
            var virtualStation  = new VirtualChargingStation(
                                      ChargingStationId,
                                      RoamingNetwork,
                                      Name,
                                      Description,
                                      InitialAdminStatus ?? ChargingStationAdminStatusTypes.Operational,
                                      InitialStatus      ?? ChargingStationStatusTypes.     Available,
                                      EllipticCurve,
                                      PrivateKey,
                                      PublicKeyCertificates,
                                      SelfCheckTimeSpan,
                                      MaxAdminStatusScheduleSize,
                                      MaxStatusScheduleSize
                                  );

            Configurator?.Invoke(virtualStation);

            if (chargingStations.Add(virtualStation))
            {

                //_VirtualEVSE.OnPropertyChanged        += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                //                                           => UpdateEVSEData(Timestamp, Sender as VirtualEVSE, PropertyName, OldValue, NewValue);
                //
                //_VirtualEVSE.OnStatusChanged          += UpdateEVSEStatus;
                //_VirtualEVSE.OnAdminStatusChanged     += UpdateEVSEAdminStatus;
                //_VirtualEVSE.OnNewReservation         += SendNewReservation;
                //_VirtualEVSE.OnNewChargingSession     += SendNewChargingSession;
                //_VirtualEVSE.OnNewChargeDetailRecord  += SendNewChargeDetailRecord;

                OnSuccess?.Invoke(virtualStation);

                return AddChargingStationResult.Success(
                           virtualStation,
                           EventTracking_Id.New,
                           Id,
                           this,
                           this
                       );

            }

            return AddChargingStationResult.Error(
                       virtualStation,
                       "error".ToI18NString(),
                       EventTracking_Id.New,
                       Id,
                       this,
                       this
                   );

        }

        #endregion

        public Task<AddChargingStationResult> AddChargingStation(IChargingStation                                                ChargingStation,

                                                                 Action<IChargingStation>?                                       OnSuccess                      = null,
                                                                 Action<IChargingPool, IChargingStation>?                        OnError                        = null,

                                                                 Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                 EventTracking_Id?                                               EventTrackingId                = null,
                                                                 User_Id?                                                        CurrentUserId                  = null)

        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationResult> AddChargingStationIfNotExists(IChargingStation                                                ChargingStation,

                                                                            Action<IChargingStation>?                                       OnSuccess                      = null,
                                                                            Action<IChargingPool, IChargingStation>?                        OnError                        = null,

                                                                            Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                            EventTracking_Id?                                               EventTrackingId                = null,
                                                                            User_Id?                                                        CurrentUserId                  = null)

        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingStationResult> AddOrUpdateChargingStation(IChargingStation                                                ChargingStation,

                                                                                 Action<IChargingStation>?                                       OnAdditionSuccess              = null,
                                                                                 Action<IChargingStation, IChargingStation>?                     OnUpdateSuccess                = null,
                                                                                 Action<IChargingPool,    IChargingStation>?                     OnError                        = null,

                                                                                 Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                                 EventTracking_Id?                                               EventTrackingId                = null,
                                                                                 User_Id?                                                        CurrentUserId                  = null)
        {
            throw new NotImplementedException();
        }



        #region ContainsChargingStationId(ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of an ChargingStation.</param>
        public Boolean ContainsChargingStationId(ChargingStation_Id ChargingStationId)
            => chargingStations.Any(evse => evse.Id == ChargingStationId);

        #endregion

        #region GetChargingStationById(ChargingStationId)

        public IChargingStation? GetChargingStationById(ChargingStation_Id ChargingStationId)
            => chargingStations.FirstOrDefault(evse => evse.Id == ChargingStationId);

        #endregion

        #region TryGetChargingStationById(ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out IChargingStation? ChargingStation)
        {

            ChargingStation = GetChargingStationById(ChargingStationId);

            return ChargingStation is not null;

        }

        #endregion


        #region TryGetChargingStationByEVSEId(EVSEId, out RemoteChargingStation)

        public Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out IChargingStation? RemoteChargingStation)
        {

            foreach (var station in chargingStations)
            {

                if (station.TryGetEVSEById(EVSEId, out var RemoteEVSE))
                {
                    RemoteChargingStation = station;
                    return true;
                }

            }

            RemoteChargingStation = null;
            return false;

        }

        #endregion

        #endregion

        #region Energy Meters

        /// <summary>
        /// Return all charging stations registered within this charing pool.
        /// </summary>
        public IEnumerable<IEnergyMeter> EnergyMeters { get; }

        #endregion


        #region (Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of the charging station changed.
        /// </summary>
        public event OnRemoteChargingPoolDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of the charging station changed.
        /// </summary>
        public event OnRemoteChargingPoolAdminStatusChangedDelegate?  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of the charging station changed.
        /// </summary>
        public event OnRemoteChargingPoolStatusChangedDelegate?       OnStatusChanged;

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                                    Timestamp,
                                              EventTracking_Id                            EventTrackingId,
                                              Timestamped<ChargingPoolAdminStatusTypes>   NewStatus,
                                              Timestamped<ChargingPoolAdminStatusTypes>?  OldStatus    = null,
                                              Context?                                    DataSource   = null)
        {

            OnAdminStatusChanged?.Invoke(Timestamp,
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
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                               Timestamp,
                                         EventTracking_Id                       EventTrackingId,
                                         Timestamped<ChargingPoolStatusTypes>   NewStatus,
                                         Timestamped<ChargingPoolStatusTypes>?  OldStatus    = null,
                                         Context?                               DataSource   = null)
        {

            OnStatusChanged?.Invoke(Timestamp,
                                    EventTrackingId,
                                    this,
                                    NewStatus,
                                    OldStatus,
                                    DataSource);

        }

        #endregion

        #endregion


        #region Reservations...

        #region Data

        private readonly Dictionary<ChargingReservation_Id, ChargingReservationCollection> chargingReservations;

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
            => chargingReservations.Select(_ => _.Value).LastOrDefault();

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
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<EMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null,
                    CancellationToken                  CancellationToken      = default)


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

            ChargingReservation? newReservation  = null;
            ReservationResult?   result          = null;

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
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnReserveRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.ChargingPoolId.HasValue && ChargingLocation.ChargingPoolId.Value != Id)
                    result = ReservationResult.UnknownLocation;

                else if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                         AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (ReservationLevel == ChargingReservationLevel.EVSE                                   &&
                        ChargingLocation.EVSEId.HasValue                                                    &&
                        TryGetChargingStationByEVSEId(ChargingLocation.EVSEId.Value, out var remoteStation) &&
                        remoteStation is not null)
                    {

                        result = await remoteStation.
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
                                                   EventTrackingId,
                                                   RequestTimeout,
                                                   CancellationToken);

                        newReservation = result.Reservation;

                    }

                    else if (ReservationLevel == ChargingReservationLevel.ChargingPool &&
                             ChargingLocation.ChargingPoolId.HasValue)
                    {

                        var results = new List<ReservationResult>();

                        foreach (var remoteStation2 in chargingStations)
                        {

                            results.Add(await remoteStation2.
                                                  Reserve(ChargingLocation,
                                                          ReservationLevel,
                                                          ReservationStartTime,
                                                          Duration,
                                                          ChargingReservation_Id.NewRandom(OperatorId),
                                                          LinkedReservationId,
                                                          ProviderId,
                                                          RemoteAuthentication,
                                                          ChargingProduct,
                                                          AuthTokens,
                                                          eMAIds,
                                                          PINs,

                                                          Timestamp,
                                                          EventTrackingId,
                                                          RequestTimeout,
                                                          CancellationToken));

                        }

                        var newReservations = results.Where (_result => _result.Result == ReservationResultType.Success).
                                                      Select(_result => _result.Reservation).
                                                      ToArray();

                        if (newReservations.Length > 0)
                        {

                            newReservation = new ChargingReservation(Id:                      ReservationId ?? ChargingReservation_Id.NewRandom(OperatorId),
                                                                     Timestamp:               Timestamp.Value,
                                                                     StartTime:               ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                     Duration:                Duration  ?? MaxReservationDuration,
                                                                     EndTime:                 (ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now) + (Duration ?? MaxReservationDuration),
                                                                     ConsumedReservationTime: TimeSpan.FromSeconds(0),
                                                                     ReservationLevel:        ReservationLevel,
                                                                     ProviderId:              ProviderId,
                                                                     StartAuthentication:     RemoteAuthentication,
                                                                     RoamingNetworkId:        RoamingNetwork.Id,
                                                                     ChargingPoolId:          Id,
                                                                     ChargingStationId:       null,
                                                                     EVSEId:                  null,
                                                                     ChargingProduct:         ChargingProduct,
                                                                     SubReservations:         newReservations);

                            foreach (var subReservation in newReservation.SubReservations)
                            {
                                subReservation.ParentReservation = newReservation;
                                subReservation.ChargingPoolId    = Id;
                            }

                            result = ReservationResult.Success(newReservation);

                        }

                        else
                            result = ReservationResult.AlreadyReserved;

                    }

                    else
                        result = ReservationResult.UnknownLocation;

                }
                else
                {
                    result = AdminStatus.Value switch {
                        _ => ReservationResult.OutOfService,
                    };
                }


                if (result.Result == ReservationResultType.Success &&
                    newReservation != null)
                {

                    chargingReservations.Add(newReservation.Id, new ChargingReservationCollection(newReservation));

                    foreach (var subReservation in newReservation.SubReservations)
                        chargingReservations.Add(subReservation.Id, new ChargingReservationCollection(subReservation));

                    OnNewReservation?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                             this,
                                             newReservation);

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
                                          startTime,
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
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnReserveResponse));
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
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
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
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    var _Reservation = ChargingReservations.FirstOrDefault(reservation => reservation.Id == ReservationId);

                    if (_Reservation != null &&
                        _Reservation.ChargingStationId.HasValue)
                    {

                        var chargingStation = GetChargingStationById(_Reservation.ChargingStationId.Value);

                        if (chargingStation is not null)
                            result = await chargingStation.CancelReservation(ReservationId,
                                                                             Reason,

                                                                             Timestamp,
                                                                             EventTrackingId,
                                                                             RequestTimeout,
                                                                             CancellationToken);

                        if (result is not null && result.Result != CancelReservationResultTypes.UnknownReservationId)
                            return result;

                    }

                    foreach (var chargingStation in chargingStations)
                    {

                        result = await chargingStation.CancelReservation(ReservationId,
                                                                         Reason,

                                                                         Timestamp,
                                                                         EventTrackingId,
                                                                         RequestTimeout,
                                                                         CancellationToken);

                        if (result.Result != CancelReservationResultTypes.UnknownReservationId)
                            break;

                    }

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

            result ??= CancelReservationResult.Error(ReservationId, Reason);


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
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnCancelReservationResponse));
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

            if (chargingReservations.TryGetValue(ReservationId, out var reservationCollection))
                return reservationCollection?.LastOrDefault();

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

            if (chargingReservations.TryGetValue(ReservationId, out var reservationCollection))
                return reservationCollection;

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

            if (chargingReservations.TryGetValue(ReservationId, out var reservationCollection))
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

            => chargingReservations.TryGetValue(ReservationId, out ChargingReservations);

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

        private readonly Dictionary<ChargingSession_Id, ChargingSession> chargingSessions;

        public IEnumerable<ChargingSession> ChargingSessions
            => chargingSessions.Select(_ => _.Value);

        #region Contains(ChargingSessionId)

        /// <summary>
        /// Whether the given charging session identification is known within the EVSE.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        public Boolean Contains(ChargingSession_Id ChargingSessionId)

            => chargingSessions.ContainsKey(ChargingSessionId);

        #endregion

        #region TryGetChargingSessionById(SessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession? ChargingSession)
            => chargingSessions.TryGetValue(SessionId, out ChargingSession);

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
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate     OnRemoteStopResponse;


        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate     OnNewChargingSession;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate  OnNewChargeDetailRecord;
        public event OnChargingStationDataChangedDelegate OnChargingStationDataChanged;
        public event OnChargingStationStatusChangedDelegate OnChargingStationStatusChanged;
        public event OnChargingStationAdminStatusChangedDelegate OnChargingStationAdminStatusChanged;
        public event OnEVSEDataChangedDelegate OnEVSEDataChanged;
        public event OnEVSEStatusChangedDelegate OnEVSEStatusChanged;
        public event OnEVSEAdminStatusChangedDelegate OnEVSEAdminStatusChanged;

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
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingProduct?         ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication?    RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null,
                        CancellationToken        CancellationToken      = default)


                => RemoteStart(ChargingLocation.FromChargingPoolId(Id),
                               ChargingProduct,
                               ReservationId,
                               SessionId,
                               ProviderId,
                               RemoteAuthentication,

                               Timestamp,
                               EventTrackingId,
                               RequestTimeout,
                               CancellationToken);

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
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct?         ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication?    RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null,
                        CancellationToken        CancellationToken      = default)
        {

            #region Initial checks

            SessionId       ??= ChargingSession_Id.NewRandom(OperatorId);
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
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.ChargingPoolId.HasValue &&
                    ChargingLocation.ChargingPoolId.Value != Id)
                {
                    result = RemoteStartResult.UnknownLocation();
                }

                else if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                         AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (!ChargingLocation.EVSEId.HasValue)
                        result = RemoteStartResult.UnknownLocation();

                    else if (!TryGetChargingStationByEVSEId(ChargingLocation.EVSEId.Value, out var remoteStation))
                        result = RemoteStartResult.UnknownLocation();

                    else if (remoteStation is not null)
                        result = await remoteStation.
                                           RemoteStart(ChargingProduct,
                                                       ReservationId,
                                                       SessionId,
                                                       ProviderId,
                                                       RemoteAuthentication,

                                                       Timestamp,
                                                       EventTrackingId,
                                                       RequestTimeout,
                                                       CancellationToken);

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

            result ??= RemoteStartResult.Error("unknown");


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
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnRemoteStartResponse));
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
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null,
                       CancellationToken      CancellationToken      = default)

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
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (!TryGetChargingSessionById(SessionId, out var chargingSession))
                    {

                        foreach (var remoteStation in chargingStations)
                        {

                            result = await remoteStation.
                                               RemoteStop(SessionId,
                                                          ReservationHandling,
                                                          ProviderId,
                                                          RemoteAuthentication,

                                                          Timestamp,
                                                          EventTrackingId,
                                                          RequestTimeout,
                                                          CancellationToken);

                            if (result?.Result == RemoteStopResultTypes.Success)
                                break;

                        }

                        if (result?.Result != RemoteStopResultTypes.Success)
                            result = RemoteStopResult.InvalidSessionId(SessionId);

                    }

                    else if (chargingSession.ChargingStation                       is not null &&
                             chargingSession.ChargingStation.RemoteChargingStation is not null)
                    {

                        result = await chargingSession.ChargingStation.RemoteChargingStation.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

                    }

                    else if (chargingSession.ChargingStationId.HasValue &&
                             TryGetChargingStationById(chargingSession.ChargingStationId.Value, out var remoteStation))
                    {

                        result = await remoteStation.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

                    }

                    result = RemoteStopResult.UnknownLocation(SessionId);

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
                                             EndTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnRemoteStopResponse));
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
                    Session.ChargingPoolId  = Id;

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


        #region Operator overloading

        #region Operator == (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(VirtualChargingPool1, VirtualChargingPool2))
                return true;

            // If one is null, but not both, return false.
            if ((VirtualChargingPool1 is null) || (VirtualChargingPool2 is null))
                return false;

            return VirtualChargingPool1.Equals(VirtualChargingPool2);

        }

        #endregion

        #region Operator != (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
            => !(VirtualChargingPool1 == VirtualChargingPool2);

        #endregion

        #region Operator <  (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
        {

            if (VirtualChargingPool1 is null)
                throw new ArgumentNullException(nameof(VirtualChargingPool1), "The given VirtualChargingPool1 must not be null!");

            return VirtualChargingPool1.CompareTo(VirtualChargingPool2) < 0;

        }

        #endregion

        #region Operator <= (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
            => !(VirtualChargingPool1 > VirtualChargingPool2);

        #endregion

        #region Operator >  (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
        {

            if (VirtualChargingPool1 is null)
                throw new ArgumentNullException(nameof(VirtualChargingPool1), "The given VirtualChargingPool1 must not be null!");

            return VirtualChargingPool1.CompareTo(VirtualChargingPool2) > 0;

        }

        #endregion

        #region Operator >= (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
            => !(VirtualChargingPool1 < VirtualChargingPool2);

        #endregion

        #endregion

        #region IComparable<VirtualChargingPool> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is VirtualChargingPool VirtualChargingPool))
                throw new ArgumentException("The given object is not a virtual charging station!");

            return CompareTo(VirtualChargingPool);

        }

        #endregion

        #region CompareTo(VirtualChargingPool)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool">An virtual charging station to compare with.</param>
        public Int32 CompareTo(VirtualChargingPool VirtualChargingPool)
        {

            if (VirtualChargingPool is null)
                throw new ArgumentNullException(nameof(VirtualChargingPool),  "The given virtual charging station must not be null!");

            return Id.CompareTo(VirtualChargingPool.Id);

        }

        #endregion

        #endregion

        #region IEquatable<VirtualChargingPool> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            if (!(Object is VirtualChargingPool VirtualChargingPool))
                return false;

            return Equals(VirtualChargingPool);

        }

        #endregion

        #region Equals(VirtualChargingPool)

        /// <summary>
        /// Compares two virtual charging stations for equality.
        /// </summary>
        /// <param name="VirtualChargingPool">A virtual charging station to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(VirtualChargingPool VirtualChargingPool)
        {

            if (VirtualChargingPool is null)
                return false;

            return Id.Equals(VirtualChargingPool.Id);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

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





        public IRemoteChargingPool RemoteChargingPool => throw new NotImplementedException();

        public IEnumerable<IEVSE> EVSEs => throw new NotImplementedException();

        public org.GraphDefined.Vanaheimr.Styx.Arrows.IVotingSender<DateTime, EventTracking_Id, User_Id,  IChargingPool, IChargingStation, Boolean> OnChargingStationAddition => throw new NotImplementedException();

        public org.GraphDefined.Vanaheimr.Styx.Arrows.IVotingSender<DateTime, IChargingPool, IChargingStation, Boolean> OnChargingStationRemoval => throw new NotImplementedException();

        public org.GraphDefined.Vanaheimr.Styx.Arrows.IVotingSender<DateTime, IChargingStation, IEVSE, Boolean> OnEVSERemoval => throw new NotImplementedException();

        public Partly IsHubjectCompatible => throw new NotImplementedException();

        public Partly DynamicInfoAvailable => throw new NotImplementedException();

        public Func<ChargingStationStatusReport, ChargingPoolStatusTypes> StatusAggregationDelegate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        TimeSpan IChargingReservations.MaxReservationDuration { get; set; }

        public org.GraphDefined.Vanaheimr.Styx.Arrows.IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSEAddition => throw new NotImplementedException();

        public org.GraphDefined.Vanaheimr.Styx.Arrows.IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, IChargingStation, Boolean> OnChargingStationUpdate => throw new NotImplementedException();

        org.GraphDefined.Vanaheimr.Styx.Arrows.IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> IChargingPool.OnChargingStationRemoval => throw new NotImplementedException();

        public org.GraphDefined.Vanaheimr.Styx.Arrows.IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, IEVSE, Boolean> OnEVSEUpdate => throw new NotImplementedException();

        org.GraphDefined.Vanaheimr.Styx.Arrows.IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> IChargingPool.OnEVSERemoval => throw new NotImplementedException();

        public Boolean Equals(IChargingPool? other)
        {
            throw new NotImplementedException();
        }

        public Int32 CompareTo(IChargingPool? other)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ChargingStation> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Boolean ContainsEVSE(EVSE_Id? EVSEId)
        {
            throw new NotImplementedException();
        }

        public Boolean ContainsChargingStation(IChargingStation ChargingStation)
        {
            throw new NotImplementedException();
        }

        public ChargingPool UpdateWith(ChargingPool OtherChargingPool)
        {
            throw new NotImplementedException();
        }

        public Boolean ContainsEVSE(EVSE_Id EVSEId)
        {
            throw new NotImplementedException();
        }

        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate IncludeStations = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChargingStationAdminStatus> ChargingStationAdminStatus(IncludeChargingStationDelegate IncludeStations = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChargingStationStatus> ChargingStationStatus(IncludeChargingStationDelegate IncludeStations = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate? IncludeEVSEs = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate IncludeEVSEs = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EVSEAdminStatus> EVSEAdminStatusSchedule(IncludeEVSEDelegate? IncludeEVSEs = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate IncludeEVSEs = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EVSEStatus> EVSEStatusSchedule(IncludeEVSEDelegate IncludeEVSEs = null)
        {
            throw new NotImplementedException();
        }

        public Boolean ContainsEVSE(EVSE EVSE)
        {
            throw new NotImplementedException();
        }

        public IEVSE GetEVSEById(EVSE_Id EVSEId)
        {
            throw new NotImplementedException();
        }

        IEnumerator<IChargingStation> IEnumerable<IChargingStation>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>> EVSEAdminStatusSchedule(IncludeEVSEDelegate? IncludeEVSEs = null, Func<DateTime, Boolean>? TimestampFilter = null, Func<EVSEAdminStatusTypes, Boolean>? StatusFilter = null, UInt64? Skip = null, UInt64? Take = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>> EVSEStatusSchedule(IncludeEVSEDelegate? IncludeEVSEs = null, Func<DateTime, Boolean>? TimestampFilter = null, Func<EVSEStatusTypes, Boolean>? StatusFilter = null, UInt64? Skip = null, UInt64? Take = null)
        {
            throw new NotImplementedException();
        }

        public JObject ToJSON(Boolean                                              Embedded                            = false,
                              InfoStatus                                           ExpandRoamingNetworkId              = InfoStatus.ShowIdOnly,
                              InfoStatus                                           ExpandChargingStationOperatorId     = InfoStatus.ShowIdOnly,
                              InfoStatus                                           ExpandChargingStationIds            = InfoStatus.Expanded,
                              InfoStatus                                           ExpandEVSEIds                       = InfoStatus.Hidden,
                              InfoStatus                                           ExpandBrandIds                      = InfoStatus.ShowIdOnly,
                              InfoStatus                                           ExpandDataLicenses                  = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<IChargingPool>?      CustomChargingPoolSerializer        = null,
                              CustomJObjectSerializerDelegate<IChargingStation>?   CustomChargingStationSerializer     = null,
                              CustomJObjectSerializerDelegate<IEVSE>?              CustomEVSESerializer                = null,
                              CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingStationResult> RemoveChargingStation(ChargingStation_Id ChargingStationId, EventTracking_Id EventTrackingId, User_Id? CurrentUserId)
        {
            throw new NotImplementedException();
        }

        public Boolean TryRemoveChargingStation(ChargingStation_Id ChargingStationId, out IChargingStation? ChargingStation, EventTracking_Id EventTrackingId, User_Id? CurrentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationResult> UpdateChargingStation(IChargingStation ChargingStation, Action<IChargingStation>? OnAdditionSuccess = null, Action<IChargingStation, IChargingStation>? OnUpdateSuccess = null, Action<IChargingPool, IChargingStation>? OnError = null, Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationResult> UpdateChargingStation(IChargingStation ChargingStation, Action<IChargingStation> UpdateDelegate, Action<IChargingStation>? OnAdditionSuccess = null, Action<IChargingStation, IChargingStation>? OnUpdateSuccess = null, Action<IChargingPool, IChargingStation>? OnError = null, Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public ChargingPool Clone()
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationResult> AddChargingStation(IChargingStation ChargingStation, Action<IChargingStation, EventTracking_Id>? OnSuccess = null, Action<IChargingPool, IChargingStation, EventTracking_Id>? OnError = null, Boolean SkipAddedNotifications = false, Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationResult> AddChargingStationIfNotExists(IChargingStation ChargingStation, Action<IChargingStation, EventTracking_Id>? OnSuccess = null, Boolean SkipAddedNotifications = false, Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingStationResult> AddOrUpdateChargingStation(IChargingStation ChargingStation, Action<IChargingStation, EventTracking_Id>? OnAdditionSuccess = null, Action<IChargingStation, IChargingStation, EventTracking_Id>? OnUpdateSuccess = null, Action<IChargingPool, IChargingStation, EventTracking_Id>? OnError = null, Boolean SkipAddOrUpdatedUpdatedNotifications = false, Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationResult> UpdateChargingStation(IChargingStation ChargingStation, Action<IChargingStation, IChargingStation, EventTracking_Id>? OnUpdateSuccess = null, Action<IChargingPool, IChargingStation, EventTracking_Id>? OnError = null, Boolean SkipUpdatedNotifications = false, Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationResult> UpdateChargingStation(IChargingStation ChargingStation, Action<IChargingStation> UpdateDelegate, Action<IChargingStation, IChargingStation, EventTracking_Id>? OnUpdateSuccess = null, Action<IChargingPool, IChargingStation, EventTracking_Id>? OnError = null, Boolean SkipUpdatedNotifications = false, Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingStationResult> RemoveChargingStation(ChargingStation_Id Id, Action<IChargingStation, EventTracking_Id>? OnSuccess = null, Action<IChargingPool, IChargingStation, EventTracking_Id>? OnError = null, Boolean SkipRemovedNotifications = false, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

    }

}
