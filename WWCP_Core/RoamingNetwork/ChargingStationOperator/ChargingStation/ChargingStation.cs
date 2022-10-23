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
using System.Collections.Concurrent;

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
        /// All brands registered for this charging station.
        /// </summary>
        [Optional, SlowData]
        public ConcurrentDictionary<Brand_Id, Brand>            Brands          { get; }

        /// <summary>
        /// The license of the charging station data.
        /// </summary>
        [Mandatory, SlowData]
        public ReactiveSet<DataLicense>                         DataLicenses    { get; }


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

        private I18NString arrivalInstructions;

        /// <summary>
        /// An optional (multi-language) description of how to find the charging station.
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

                else if (value != arrivalInstructions &&
                         value != ChargingPool?.ArrivalInstructions)
                {
                    SetProperty(ref arrivalInstructions, value);
                }

            }

        }

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

        private ReactiveSet<ParkingSpace> _ParkingSpaces;

        /// <summary>
        /// Parking spaces located at the charging station.
        /// </summary>
        [Optional]
        public ReactiveSet<ParkingSpace> ParkingSpaces
        {

            get
            {
                return _ParkingSpaces;
            }

            set
            {

                if (value != _ParkingSpaces)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _ParkingSpaces);

                    else
                    {

                        if (_ParkingSpaces == null)
                            SetProperty(ref _ParkingSpaces, value);

                        else
                            SetProperty(ref _ParkingSpaces, _ParkingSpaces.Set(value));

                    }

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

                return _UIFeatures != null
                           ? _UIFeatures
                           : ChargingPool?.UIFeatures;

            }

            set
            {

                if (value != _UIFeatures && value != ChargingPool?.UIFeatures)
                {

                    if (value != null)
                        DeleteProperty(ref _UIFeatures);

                    else
                        SetProperty(ref _UIFeatures, value);

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

                return _AuthenticationModes != null && _AuthenticationModes.Any()
                           ? _AuthenticationModes
                           : ChargingPool?.AuthenticationModes;

            }

            set
            {

                if (value != _AuthenticationModes && value != ChargingPool?.AuthenticationModes)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _AuthenticationModes);

                    else
                    {

                        if (_AuthenticationModes == null)
                            SetProperty(ref _AuthenticationModes, value);

                        else
                            SetProperty(ref _AuthenticationModes, _AuthenticationModes.Set(value));

                    }

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

                return _PaymentOptions != null && _PaymentOptions.Any()
                           ? _PaymentOptions
                           : ChargingPool?.PaymentOptions;

            }

            set
            {

                if (value != _PaymentOptions && value != ChargingPool?.PaymentOptions)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _PaymentOptions);

                    else
                    {

                        if (_PaymentOptions == null)
                            SetProperty(ref _PaymentOptions, value);

                        else
                            SetProperty(ref _PaymentOptions, _PaymentOptions.Set(value));

                    }

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

        #region PhotoURIs

        private ReactiveSet<String> _PhotoURIs;

        /// <summary>
        /// URIs of photos of this charging station.
        /// </summary>
        [Optional]
        public ReactiveSet<String> PhotoURIs
        {

            get
            {

                return _PhotoURIs != null && _PhotoURIs.Any()
                           ? _PhotoURIs
                           : ChargingPool?.PhotoURIs;

            }

            set
            {

                if (value != _PhotoURIs && value != ChargingPool?.PhotoURIs)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _PhotoURIs);

                    else
                    {

                        if (_PhotoURIs == null)
                            SetProperty(ref _PhotoURIs, value);

                        else
                            SetProperty(ref _PhotoURIs, _PhotoURIs.Set(value));

                    }

                }

            }

        }

        #endregion

        #region HotlinePhoneNumber

        private I18NString _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the Charging Station Operator hotline.
        /// </summary>
        [Optional]
        public I18NString HotlinePhoneNumber
        {

            get
            {

                return _HotlinePhoneNumber.IsNeitherNullNorEmpty()
                           ? _HotlinePhoneNumber
                           : ChargingPool?.HotlinePhoneNumber;

            }

            set
            {

                if (value != _HotlinePhoneNumber && value != ChargingPool?.HotlinePhoneNumber)
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

        private Address _ExitAddress;

        /// <summary>
        /// The address of the exit of this charging station.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address ExitAddress
        {

            get
            {
                return _ExitAddress ?? ChargingPool?.ExitAddress;
            }

            set
            {

                if (value != _ExitAddress && value != ChargingPool?.ExitAddress)
                {

                    if (value == null)
                        DeleteProperty(ref _ExitAddress);

                    else
                        SetProperty(ref _ExitAddress, value);

                }

            }

        }

        #endregion

        #region ExitLocation

        private GeoCoordinate? _ExitLocation;

        /// <summary>
        /// The geographical location of the exit of this charging station.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate? ExitLocation
        {

            get
            {

                return _ExitLocation.HasValue
                           ? _ExitLocation
                           : ChargingPool?.ExitLocation;

            }

            set
            {

                if (value != _ExitLocation && value != ChargingPool?.ExitLocation)
                {

                    if (value == null)
                        DeleteProperty(ref _ExitLocation);

                    else
                        SetProperty(ref _ExitLocation, value);

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
                return gridConnection ?? ChargingPool?.GridConnection;
            }

            set
            {

                if (value != gridConnection && value != ChargingPool?.GridConnection)
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

        private String _ServiceIdentification;

        /// <summary>
        /// The internal service identification of the charging station maintained by the Charging Station Operator.
        /// </summary>
        [Optional]
        public String ServiceIdentification
        {

            get
            {
                return _ServiceIdentification;
            }

            set
            {

                if (ServiceIdentification != value)
                    SetProperty(ref _ServiceIdentification, value);

            }

        }

        #endregion

        #region ModelCode

        private String _ModelCode;

        /// <summary>
        /// The internal model code of the charging station maintained by the Charging Station Operator.
        /// </summary>
        [Optional]
        public String ModelCode
        {

            get
            {
                return _ModelCode;
            }

            set
            {

                if (ModelCode != value)
                    SetProperty(ref _ModelCode, value);

            }

        }

        #endregion

        #region HubjectStationId

        private String _HubjectStationId;

        [Optional]
        public String HubjectStationId
        {

            get
            {
                return _HubjectStationId;
            }

            set
            {

                if (HubjectStationId != value)
                    SetProperty<String>(ref _HubjectStationId, value);

            }

        }

        #endregion


        #region StatusAggregationDelegate

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated EVSEs.
        /// </summary>
        public Func<EVSEStatusReport, ChargingStationStatusTypes> StatusAggregationDelegate { get; set; }

        #endregion

        #endregion

        #region Links

        /// <summary>
        /// An optional remote charging station.
        /// </summary>
        public IRemoteChargingStation?   RemoteChargingStation    { get; }


        /// <summary>
        /// The charging pool.
        /// </summary>
        [InternalUseOnly]
        public ChargingPool?             ChargingPool             { get; }


        /// <summary>
        /// The Charging Station Operator of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator?  Operator
            => ChargingPool?.Operator;

        /// <summary>
        /// The roaming network of this charging station.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork?          RoamingNetwork
            => Operator?.RoamingNetwork;

        #endregion

        #region Events

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnChargingStationDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnChargingStationStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate  OnAdminStatusChanged;

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

            this.ChargingPool                = ChargingPool;

            this.Brands                      = new ConcurrentDictionary<Brand_Id, Brand>();
            //this.Brands.OnSetChanged += (timestamp, sender, newItems, oldItems) => {

            //    PropertyChanged("DataLicenses",
            //                    oldItems,
            //                    newItems);

            //};

            this.DataLicenses                = new ReactiveSet<DataLicense>();
            this.DataLicenses.OnSetChanged  += (timestamp, reactiveSet, newItems, oldItems) =>
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

            this.adminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this.statusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

            this.RemoteChargingStation = RemoteChargingStationCreator?.Invoke(this);

        }

        #endregion

        #endregion


        #region Data/(Admin-)Status

        #region (internal) UpdateData       (Timestamp, EventTrackingId, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="Sender">The changed charging station.</param>
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
                                         Sender as ChargingStation,
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
        internal async Task UpdateAdminStatus(DateTime                                      Timestamp,
                                              EventTracking_Id                              EventTrackingId,
                                              Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                              Timestamped<ChargingStationAdminStatusTypes>  NewStatus)
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
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                                 Timestamp,
                                         EventTracking_Id                         EventTrackingId,
                                         Timestamped<ChargingStationStatusTypes>  OldStatus,
                                         Timestamped<ChargingStationStatusTypes>  NewStatus)
        {

            var OnAggregatedStatusChangedLocal = OnStatusChanged;
            if (OnAggregatedStatusChangedLocal != null)
                await OnAggregatedStatusChangedLocal(Timestamp,
                                                     EventTrackingId,
                                                     this,
                                                     OldStatus,
                                                     NewStatus);

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

            => evses;

        #endregion

        #region EVSEIds        (IncludeEVSEs = null)

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment (EVSEs)
        /// present within this charging station.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate?  IncludeEVSEs   = null)

            => IncludeEVSEs is null

                   ? evses.
                         Select(evse => evse.Id)

                   : evses.
                         Where (evse => IncludeEVSEs(evse)).
                         Select(evse => evse.Id);

        #endregion

        #region EVSEAdminStatus(IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate? IncludeEVSEs = null)

            => evses.
                   Where (evse => IncludeEVSEs?.Invoke(evse) ?? true).
                   Select(evse => new EVSEAdminStatus(evse.Id,
                                                      evse.AdminStatus));

        #endregion

        #region EVSEStatus     (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate? IncludeEVSEs = null)

            => evses.
                   Where (evse => IncludeEVSEs?.Invoke(evse) ?? true).
                   Select(evse => new EVSEStatus(evse.Id,
                                                 evse.Status));

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
        public EVSE? CreateEVSE(EVSE_Id                             Id,
                                I18NString?                         Name                         = null,
                                I18NString?                         Description                  = null,
                                Action<EVSE>?                       Configurator                 = null,
                                RemoteEVSECreatorDelegate?          RemoteEVSECreator            = null,
                                Timestamped<EVSEAdminStatusTypes>?  InitialAdminStatus           = null,
                                Timestamped<EVSEStatusTypes>?       InitialStatus                = null,
                                UInt16?                             MaxAdminStatusScheduleSize   = null,
                                UInt16?                             MaxStatusScheduleSize        = null,
                                Action<EVSE>?                       OnSuccess                    = null,
                                Action<ChargingStation, EVSE_Id>?   OnError                      = null)
        {

            lock (evses)
            {

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
                var evse  = new EVSE(Id,
                                     this,
                                     Name,
                                     Description,
                                     Configurator,
                                     RemoteEVSECreator,
                                     InitialAdminStatus,
                                     InitialStatus,
                                     MaxAdminStatusScheduleSize,
                                     MaxStatusScheduleSize);

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

                        OnAdminStatusChanged               += (Timestamp, EventTrackingId, station, oldstatus, newstatus) => { this.AdminStatus = newstatus; return Task.CompletedTask; };
                        OnStatusChanged                    += (Timestamp, EventTrackingId, station, oldstatus, newstatus) => { this.Status      = newstatus; return Task.CompletedTask; };

                        this.RemoteChargingStation.OnAdminStatusChanged    += async (Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus) => AdminStatus = NewStatus;
                        this.RemoteChargingStation.OnStatusChanged         += async (Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus) => Status      = NewStatus;

                        //RemoteConfigurator?.Invoke(_EVSE.RemoteEVSE);

                    }

                    OnSuccess?.Invoke(evse);
                    EVSEAddition.SendNotification(now, this, evse);

                    return evse;

                }

                Debug.WriteLine("EVSE '" + Id + "' was not created!");
                return null;

            }

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
        public EVSE? CreateOrUpdateEVSE(EVSE_Id                             Id,
                                        I18NString?                         Name                         = null,
                                        I18NString?                         Description                  = null,
                                        Action<EVSE>?                       Configurator                 = null,
                                        RemoteEVSECreatorDelegate?          RemoteEVSECreator            = null,
                                        Timestamped<EVSEAdminStatusTypes>?  InitialAdminStatus           = null,
                                        Timestamped<EVSEStatusTypes>?       InitialStatus                = null,
                                        UInt16                              MaxAdminStatusScheduleSize   = EVSE.DefaultMaxAdminStatusScheduleSize,
                                        UInt16                              MaxStatusScheduleSize        = EVSE.DefaultMaxEVSEStatusScheduleSize,
                                        Action<EVSE>?                       OnSuccess                    = null,
                                        Action<ChargingStation, EVSE_Id>?   OnError                      = null)
        {

            #region Initial checks

            if (Operator.Id != Id.OperatorId)
                return null;
                //throw new InvalidChargingStationOperatorId(this,
                //                                Id.OperatorId);

            InitialAdminStatus = InitialAdminStatus ?? new Timestamped<EVSEAdminStatusTypes>(EVSEAdminStatusTypes.Operational);
            InitialStatus      = InitialStatus      ?? new Timestamped<EVSEStatusTypes>     (EVSEStatusTypes.Available);

            #endregion

            lock (evses)
            {

                #region If the EVSE identification is new/unknown: Call CreateEVSE(...)

                if (!evses.ContainsId(Id))
                    return CreateEVSE(Id,
                                      Name,
                                      Description,
                                      Configurator,
                                      RemoteEVSECreator,
                                      InitialAdminStatus,
                                      InitialStatus,
                                      MaxAdminStatusScheduleSize,
                                      MaxStatusScheduleSize,
                                      OnSuccess,
                                      OnError);

                #endregion


                var existingEVSE = evses.GetById(Id);

                // Merge existing EVSE with new EVSE data...
                if (existingEVSE is not null)
                    return existingEVSE.
                               UpdateWith(new EVSE(Id,
                                                   this,
                                                   Name,
                                                   Description,
                                                   Configurator,
                                                   null,
                                                   new Timestamped<EVSEAdminStatusTypes>(DateTime.MinValue, EVSEAdminStatusTypes.Operational),
                                                   new Timestamped<EVSEStatusTypes>     (DateTime.MinValue, EVSEStatusTypes.     Available)));

            }

            return null;

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


        #region (internal) UpdateEVSEData       (Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateEVSEData(DateTime          Timestamp,
                                           EventTracking_Id  EventTrackingId,
                                           IEVSE             EVSE,
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
        /// Update the current charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEAdminStatus(DateTime                           Timestamp,
                                                  EventTracking_Id                   EventTrackingId,
                                                  IEVSE                              EVSE,
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
        /// Update the current charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                      Timestamp,
                                             EventTracking_Id              EventTrackingId,
                                             IEVSE                         EVSE,
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

            if (StatusAggregationDelegate != null)
            {
                statusSchedule.Insert(StatusAggregationDelegate(new EVSEStatusReport(evses)),
                                      Timestamp);
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
        /// Reserve the possibility to charge at this charging station.
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


                => Reserve(ChargingLocation.FromChargingStationId(Id),
                           ChargingReservationLevel.ChargingStation,
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
            CancelReservationResult result                = null;

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
                DebugX.LogException(e, nameof(ChargingStation) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
                {

                    if (RemoteChargingStation != null)
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


        public void AddParkingSpaces(params ParkingSpace[] ParkingSpaces)
        {

            if (ParkingSpaces is not null)
            {

                _ParkingSpaces ??= new ReactiveSet<ParkingSpace>();

                _ParkingSpaces.Add(ParkingSpaces);

            }

        }



        #region ToJSON(this ChargingStation,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging station.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public JObject ToJSON(Boolean                                            Embedded                          = false,
                              InfoStatus                                         ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandEVSEIds                     = InfoStatus.Expanded,
                              InfoStatus                                         ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandDataLicenses                = InfoStatus.ShowIdOnly,
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

                         (!Embedded || DataSource != ChargingPool.DataSource)
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
                         (!Embedded || AuthenticationModes != ChargingPool.AuthenticationModes) ? new JProperty("authenticationModes",  AuthenticationModes.ToJSON()) : null,
                         (!Embedded || HotlinePhoneNumber  != ChargingPool.HotlinePhoneNumber)  ? new JProperty("hotlinePhoneNumber",   HotlinePhoneNumber. ToJSON()) : null,
                         (!Embedded || OpeningTimes        != ChargingPool.OpeningTimes)        ? new JProperty("openingTimes",         OpeningTimes.       ToJSON()) : null,

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
                                                       new JArray(Brands.Select (brand   => brand.Key).
                                                                         OrderBy(brandId => brandId).
                                                                         Select (brandId => brandId.ToString()))),

                                   () => new JProperty("brands",
                                                       new JArray(Brands.OrderBy(brand => brand.Key).
                                                                         Select (brand => brand.Value).
                                                                         ToJSON (Embedded:                         true,
                                                                                 ExpandDataLicenses:               InfoStatus.ShowIdOnly))))

                             : null

                     );

            return CustomChargingStationSerializer is not null
                       ? CustomChargingStationSerializer(this, JSON)
                       : JSON;

        }

        #endregion



        #region UpdateWith(OtherChargingStation)

        /// <summary>
        /// Update this charging station with the data of the other charging station.
        /// </summary>
        /// <param name="OtherChargingStation">Another charging station.</param>
        public ChargingStation UpdateWith(ChargingStation OtherChargingStation)
        {

            Name.       Add(OtherChargingStation.Name);
            Description.Add(OtherChargingStation.Description);

            Brands.Clear();
            foreach (var brand in OtherChargingStation.Brands)
                Brands.TryAdd(brand.Key, brand.Value);

            Address              = OtherChargingStation.Address;
            OpenStreetMapNodeId           = OtherChargingStation.OpenStreetMapNodeId;
            GeoLocation          = OtherChargingStation.GeoLocation;
            EntranceAddress      = OtherChargingStation.EntranceAddress;
            EntranceLocation     = OtherChargingStation.EntranceLocation;
            ArrivalInstructions  = OtherChargingStation.ArrivalInstructions;
            OpeningTimes         = OtherChargingStation.OpeningTimes;
            //ParkingSpaces        = OtherChargingStation.ParkingSpaces;
            UIFeatures           = OtherChargingStation.UIFeatures;

            if (AuthenticationModes == null && OtherChargingStation.AuthenticationModes != null)
                AuthenticationModes = new ReactiveSet<AuthenticationModes>(OtherChargingStation.AuthenticationModes);
            else if (AuthenticationModes != null)
                AuthenticationModes.Set(OtherChargingStation.AuthenticationModes);

            if (PaymentOptions == null && OtherChargingStation.PaymentOptions != null)
                PaymentOptions = new ReactiveSet<PaymentOptions>(OtherChargingStation.PaymentOptions);
            else if (PaymentOptions != null)
                PaymentOptions.Set(OtherChargingStation.PaymentOptions);

            Accessibility        = OtherChargingStation.Accessibility;

            if (PhotoURIs == null && OtherChargingStation.PhotoURIs != null)
                PhotoURIs = new ReactiveSet<String>(OtherChargingStation.PhotoURIs);
            else if (PhotoURIs != null)
                PhotoURIs.Set(OtherChargingStation.PhotoURIs);

            HotlinePhoneNumber   = OtherChargingStation.HotlinePhoneNumber;
            GridConnection       = OtherChargingStation.GridConnection;
            ExitAddress          = OtherChargingStation.ExitAddress;
            ExitLocation         = OtherChargingStation.ExitLocation;


            if (OtherChargingStation.AdminStatus.Timestamp > AdminStatus.Timestamp)
                AdminStatus = OtherChargingStation.AdminStatus;

            if (OtherChargingStation.Status.Timestamp > Status.Timestamp)
                Status      = OtherChargingStation.Status;

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
