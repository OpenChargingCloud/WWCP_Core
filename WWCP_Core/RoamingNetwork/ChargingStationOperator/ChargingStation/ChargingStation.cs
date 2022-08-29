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

using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.JSON;
using org.GraphDefined.WWCP.Net.IO.JSON;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// WWCP JSON I/O charging station extentions.
    /// </summary>
    public static partial class ChargingStationExtensions
    {

        #region ToJSON(this ChargingStations, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="Skip">The optional number of charging stations to skip.</param>
        /// <param name="Take">The optional number of charging stations to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static JArray ToJSON(this IEnumerable<ChargingStation>                 ChargingStations,
                                    UInt64?                                           Skip                              = null,
                                    UInt64?                                           Take                              = null,
                                    Boolean                                           Embedded                          = false,
                                    InfoStatus                                        ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                        ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                                        ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                                        ExpandEVSEIds                     = InfoStatus.Expanded,
                                    InfoStatus                                        ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                                        ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<ChargingStation>  CustomChargingStationSerializer   = null,
                                    CustomJObjectSerializerDelegate<EVSE>             CustomEVSESerializer              = null)


            => ChargingStations?.SafeAny() == true

                   ? new JArray(ChargingStations.
                                    Where         (station => station != null).
                                    OrderBy       (station => station.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (station => station.ToJSON(Embedded,
                                                                             ExpandRoamingNetworkId,
                                                                             ExpandChargingStationOperatorId,
                                                                             ExpandChargingPoolId,
                                                                             ExpandEVSEIds,
                                                                             ExpandBrandIds,
                                                                             ExpandDataLicenses,
                                                                             CustomChargingStationSerializer,
                                                                             CustomEVSESerializer)).
                                    Where         (station => station != null))

                   : new JArray();

        #endregion

        #region ToJSON(this ChargingStations, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingStation> ChargingStations, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingStations?.Any() == true
                       ? new JProperty(JPropertyKey, ChargingStations.ToJSON())
                       : null;

        }

        #endregion


        #region ToJSON(this ChargingStationAdminStatus,          Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<ChargingStationAdminStatus>  ChargingStationAdminStatus,
                                     UInt64?                                       Skip  = null,
                                     UInt64?                                       Take  = null)
        {

            #region Initial checks

            if (ChargingStationAdminStatus == null || !ChargingStationAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingStation_Id, ChargingStationAdminStatus>();

            foreach (var status in ChargingStationAdminStatus)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take.HasValue ? _FilteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)
                                              : _FilteredStatus.OrderBy(status => status.Key).Skip(Skip)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region ToJSON(this ChargingStationAdminStatusSchedules, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<ChargingStationAdminStatusSchedule>  ChargingStationAdminStatusSchedules,
                                     UInt64?                                               Skip         = null,
                                     UInt64?                                               Take         = null,
                                     UInt64                                                HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingStationAdminStatusSchedules == null || !ChargingStationAdminStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingStation_Id, ChargingStationAdminStatusSchedule>();

            foreach (var status in ChargingStationAdminStatusSchedules)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].StatusSchedule.Any() &&
                         _FilteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take.HasValue ? _FilteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)
                                              : _FilteredStatus.OrderBy(status => status.Key).Skip(Skip)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JObject(
                                                                   kvp.Value.StatusSchedule.

                                                                             // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                             GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                             Select           (group => group.First()).

                                                                             OrderByDescending(tsv   => tsv.Timestamp).
                                                                             Take             (HistorySize).
                                                                             Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                      tsv.Value.    ToString())))

                                                              )));

        }

        #endregion


        #region ToJSON(this ChargingStationStatus,               Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<ChargingStationStatus>  ChargingStationStatus,
                                     UInt64?                                  Skip  = null,
                                     UInt64?                                  Take  = null)
        {

            #region Initial checks

            if (ChargingStationStatus == null || !ChargingStationStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingStation_Id, ChargingStationStatus>();

            foreach (var status in ChargingStationStatus)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take.HasValue ? _FilteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)
                                              : _FilteredStatus.OrderBy(status => status.Key).Skip(Skip)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region ToJSON(this ChargingStationAdminStatusSchedules, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<ChargingStationStatusSchedule>  ChargingStationStatusSchedules,
                                     UInt64?                                          Skip         = null,
                                     UInt64?                                          Take         = null,
                                     UInt64                                           HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingStationStatusSchedules == null || !ChargingStationStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingStation_Id, ChargingStationStatusSchedule>();

            foreach (var status in ChargingStationStatusSchedules)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].StatusSchedule.Any() &&
                         _FilteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take.HasValue ? _FilteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)
                                              : _FilteredStatus.OrderBy(status => status.Key).Skip(Skip)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JObject(
                                                                   kvp.Value.StatusSchedule.

                                                                             // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                             GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                             Select           (group => group.First()).

                                                                             OrderByDescending(tsv   => tsv.Timestamp).
                                                                             Take             (HistorySize).
                                                                             Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                      tsv.Value.    ToString())))

                                                              )));

        }

        #endregion

    }


    public interface IChargingStation : IEquatable<ChargingStation>, IComparable<ChargingStation>, IComparable,
                                        IEnumerable<EVSE>,
                                        IStatus<ChargingStationStatusTypes>,
                                        IEntity<ChargingStation_Id>,
                                        IHasIds<ChargingStation_Id>
    {

        /// <summary>
        /// The unique identification of this charging Station.
        /// </summary>
        ChargingStation_Id         Id                    { get; }

        /// <summary>
        /// The roaming network of this charging Station.
        /// </summary>
        IRoamingNetwork          RoamingNetwork        { get; }

        /// <summary>
        /// The charging station operator of this charging Station.
        /// </summary>
        [Optional]
        ChargingStationOperator  Operator              { get; }

        /// <summary>
        /// The remote charging Station.
        /// </summary>
        [Optional]
        IRemoteChargingStation     RemoteChargingStation    { get; }



        I18NString Name         { get; }
        I18NString Description  { get; }

    }


    /// <summary>
    /// A charging station to charge an electric vehicle.
    /// </summary>
    public class ChargingStation : AEMobilityEntity<ChargingStation_Id>,
                                   IChargingStation
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext = "https://open.charging.cloud/contexts/wwcp+json/chargingStation";


        private readonly Double EPSILON = 0.01;

        /// <summary>
        /// The default max size of the charging station (aggregated EVSE) status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize         = 15;

        /// <summary>
        /// The default max size of the charging station admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize    = 15;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly TimeSpan  DefaultMaxReservationDuration   = TimeSpan.FromMinutes(30);

        #endregion

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of this charging station.
        /// </summary>
        [Mandatory]
        public I18NString Name
        {

            get
            {

                return _Name.IsNeitherNullNorEmpty()
                           ? _Name
                           : ChargingPool?.Name;

            }

            set
            {

                if (value != _Name && value != ChargingPool?.Name)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _Name);

                    else
                        SetProperty(ref _Name, value);

                }

            }

        }

        public I18NString SetName(Languages Language, String Text)
            => _Name = I18NString.Create(Language, Text);

        public I18NString SetName(I18NString I18NText)
            => _Name = I18NText;

        public I18NString AddName(Languages Language, String Text)
            => _Name.Add(Language, Text);

        #endregion

        #region Description

        internal I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of this charging station.
        /// </summary>
        [Optional]
        public I18NString Description
        {

            get
            {

                return _Description.IsNeitherNullNorEmpty()
                           ? _Description
                           : ChargingPool?.Description;

            }

            set
            {

                if (value != _Description && value != ChargingPool?.Description)
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

        #region Brands

        #region BrandAddition

        internal readonly IVotingNotificator<DateTime, ChargingStation, Brand, Boolean> BrandAddition;

        /// <summary>
        /// Called whenever a brand will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, Brand, Boolean> OnBrandAddition

            => BrandAddition;

        #endregion

        #region Brands

        private readonly SpecialHashSet<ChargingStation, Brand_Id, Brand> _Brands;

        /// <summary>
        /// All brands registered within this charging station.
        /// </summary>
        public IEnumerable<Brand> Brands
            => _Brands;

        #endregion

        public void Add(Brand Brand)
        {
            _Brands.TryAdd(Brand);
        }

        #region BrandIds

        /// <summary>
        /// All brand identifications registered within this charging station.
        /// </summary>
        public IEnumerable<Brand_Id> BrandIds
            => _Brands.Select(brand => brand.Id);

        #endregion

        #region TryGetBrand(Id, out Brand)

        /// <summary>
        /// Try to return the brand of the given brand identification.
        /// </summary>
        /// <param name="Id">The unique identification of the brand.</param>
        /// <param name="Brand">The brand.</param>
        public Boolean TryGetBrand(Brand_Id Id, out Brand Brand)
            => _Brands.TryGet(Id, out Brand);

        #endregion


        #region BrandRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStation, Brand, Boolean> BrandRemoval;

        /// <summary>
        /// Called whenever a brand will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, Brand, Boolean> OnBrandRemoval

            => BrandRemoval;

        #endregion

        #region RemoveBrand(BrandId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All brands registered within this charging station.
        /// </summary>
        /// <param name="BrandId">The unique identification of the brand to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new brand after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the brand failed.</param>
        public Brand RemoveBrand(Brand_Id BrandId,
                                 Action<ChargingStation, Brand> OnSuccess = null,
                                 Action<ChargingStation, Brand_Id> OnError = null)
        {

            lock (_Brands)
            {

                if (_Brands.TryGet(BrandId, out Brand Brand) &&
                    BrandRemoval.SendVoting(DateTime.UtcNow,
                                            this,
                                            Brand) &&
                    _Brands.TryRemove(BrandId, out Brand _Brand))
                {

                    OnSuccess?.Invoke(this, Brand);

                    BrandRemoval.SendNotification(DateTime.UtcNow,
                                                  this,
                                                  _Brand);

                    return _Brand;

                }

                OnError?.Invoke(this, BrandId);

                return null;

            }

        }

        #endregion

        #region RemoveBrand(Brand,   OnSuccess = null, OnError = null)

        /// <summary>
        /// All brands registered within this charging station.
        /// </summary>
        /// <param name="Brand">The brand to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new brand after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the brand failed.</param>
        public Brand RemoveBrand(Brand Brand,
                                 Action<ChargingStation, Brand> OnSuccess = null,
                                 Action<ChargingStation, Brand> OnError = null)
        {

            lock (_Brands)
            {

                if (BrandRemoval.SendVoting(DateTime.UtcNow,
                                            this,
                                            Brand) &&
                    _Brands.TryRemove(Brand.Id, out Brand _Brand))
                {

                    OnSuccess?.Invoke(this, _Brand);

                    BrandRemoval.SendNotification(DateTime.UtcNow,
                                                  this,
                                                  _Brand);

                    return _Brand;

                }

                OnError?.Invoke(this, Brand);

                return Brand;

            }

        }

        #endregion

        #endregion

        #region DataLicense

        private ReactiveSet<DataLicense> _DataLicenses;

        /// <summary>
        /// The license of the charging station data.
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

        #region Address

        private Address _Address;

        /// <summary>
        /// The address of this charging station.
        /// </summary>
        [Optional]
        public Address Address
        {

            get
            {
                return _Address ?? ChargingPool?.Address;
            }

            set
            {

                if (value != _Address && value != ChargingPool?.Address)
                {

                    if (value == null)
                        DeleteProperty(ref _Address);

                    else
                        SetProperty(ref _Address, value);

                }

            }

        }

        #endregion

        #region OSM_NodeId

        private String _OSM_NodeId;

        /// <summary>
        /// OSM Node Id.
        /// </summary>
        [Optional]
        public String OSM_NodeId
        {

            get
            {
                return _OSM_NodeId;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _OSM_NodeId, value);

                else
                    DeleteProperty(ref _OSM_NodeId);

            }

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate? _GeoLocation;

        /// <summary>
        /// The geographical location of this charging station.
        /// </summary>
        [Optional]
        public GeoCoordinate? GeoLocation
        {

            get
            {

                return _GeoLocation.HasValue
                           ? _GeoLocation
                           : ChargingPool?.GeoLocation;

            }

            set
            {

                if (value != _GeoLocation && value != ChargingPool?.GeoLocation)
                {

                    if (value == null)
                        DeleteProperty(ref _GeoLocation);

                    else
                        SetProperty(ref _GeoLocation, value);

                }

            }

        }

        #endregion

        #region EntranceAddress

        internal Address _EntranceAddress;

        /// <summary>
        /// The address of the entrance to this charging station.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address EntranceAddress
        {

            get
            {
                return _EntranceAddress ?? ChargingPool?.EntranceAddress;
            }

            set
            {

                if (value != _EntranceAddress && value != ChargingPool?.EntranceAddress)
                {

                    if (value == null)
                        DeleteProperty(ref _EntranceAddress);

                    else
                        SetProperty(ref _EntranceAddress, value);

                }

            }

        }

        #endregion

        #region EntranceLocation

        internal GeoCoordinate? _EntranceLocation;

        /// <summary>
        /// The geographical location of the entrance to this charging station.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate? EntranceLocation
        {

            get
            {

                return _EntranceLocation.HasValue
                           ? _EntranceLocation
                           : ChargingPool?.EntranceLocation;

            }

            set
            {

                if (value != _EntranceLocation && value != ChargingPool?.EntranceLocation)
                {

                    if (value == null)
                        DeleteProperty(ref _EntranceLocation);

                    else
                        SetProperty(ref _EntranceLocation, value);

                }

            }

        }

        #endregion

        #region ArrivalInstructions

        private I18NString _ArrivalInstructions;

        /// <summary>
        /// An optional (multi-language) description of how to find the charging pool.
        /// </summary>
        [Optional]
        public I18NString ArrivalInstructions
        {

            get
            {

                return _ArrivalInstructions.IsNeitherNullNorEmpty()
                           ? _ArrivalInstructions
                           : ChargingPool?.ArrivalInstructions;

            }

            set
            {

                if (value != _ArrivalInstructions && value != ChargingPool?.ArrivalInstructions)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _ArrivalInstructions);

                    else
                        SetProperty(ref _ArrivalInstructions, value);

                }

            }

        }

        #endregion

        #region OpeningTimes

        private OpeningTimes? openingTimes;

        /// <summary>
        /// The opening times of this charging station (non recursive).
        /// </summary>
        public OpeningTimes? _OpeningTimes
        {

            get
            {
                return openingTimes;
            }

            set
            {

                if (value != openingTimes)
                {

                    if (value is null)
                        DeleteProperty(ref openingTimes);

                    else
                        SetProperty(ref openingTimes, value);

                }

            }

        }

        /// <summary>
        /// The opening times of this charging station (or the charging pool).
        /// </summary>
        public OpeningTimes? OpeningTimes
        {

            get
            {
                return openingTimes ?? ChargingPool?.OpeningTimes;
            }

            set
            {

                if (value != openingTimes && value != ChargingPool?.OpeningTimes)
                {

                    if (value is null)
                        DeleteProperty(ref openingTimes);

                    else
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

        private GridConnectionTypes? _GridConnection;

        /// <summary>
        /// The grid connection of the charging station.
        /// </summary>
        [Optional]
        public GridConnectionTypes? GridConnection
        {

            get
            {
                return _GridConnection ?? ChargingPool?.GridConnection;
            }

            set
            {

                if (value != _GridConnection && value != ChargingPool?.GridConnection)
                {

                    if (value == null)
                        DeleteProperty(ref _GridConnection);

                    else
                        SetProperty(ref _GridConnection, value);

                }

            }

        }

        #endregion

        #region EnergyMix

        private EnergyMix _EnergyMix;

        /// <summary>
        /// The energy mix at the charging station.
        /// </summary>
        [Optional]
        public EnergyMix EnergyMix
        {

            get
            {
                return _EnergyMix ?? ChargingPool?.EnergyMix;
            }

            set
            {

                if (value != _EnergyMix && value != ChargingPool?.EnergyMix)
                {

                    if (value == null)
                        DeleteProperty(ref _EnergyMix);

                    else
                        SetProperty(ref _EnergyMix, value);

                }

            }

        }

        #endregion

        #region MaxCurrent

        private Single? _MaxCurrent;

        /// <summary>
        /// The maximum current of the grid connector of the charging station [Ampere].
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
                        _MaxCurrent = value;

                    else if (Math.Abs(_MaxCurrent.Value - value.Value) > EPSILON)
                        SetProperty(ref _MaxCurrent, value);

                }

                else
                    DeleteProperty(ref _MaxCurrent);

            }

        }

        #endregion

        #region MaxPower

        private Single? _MaxPower;

        /// <summary>
        /// The maximum power of the grid connector of the charging station [kWatt].
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

        #region MaxCapacity

        private Single? _MaxCapacity;

        /// <summary>
        /// The maximum capacity of the grid connector of the charging station [kWh].
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



        #region AdminStatus

        /// <summary>
        /// The current charging station admin status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingStationAdminStatusTypes> AdminStatus
        {

            get
            {
                return _AdminStatusSchedule.CurrentStatus;
            }

            set
            {
                SetAdminStatus(value);
            }

        }

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<ChargingStationAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The charging station admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingStationAdminStatusTypes>> AdminStatusSchedule(UInt64? HistorySize = null)
        {

            if (HistorySize.HasValue)
                return _AdminStatusSchedule.Take(HistorySize);

            return _AdminStatusSchedule;

        }

        #endregion


        #region Status

        /// <summary>
        /// The current charging station status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingStationStatusTypes> Status
        {

            get
            {

                if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
                {

                    return _StatusSchedule.CurrentStatus;

                }

                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            return new Timestamped<ChargingStationStatusTypes>(AdminStatus.Timestamp, ChargingStationStatusTypes.OutOfService);

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

        private StatusSchedule<ChargingStationStatusTypes> _StatusSchedule;

        /// <summary>
        /// The charging station status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingStationStatusTypes>> StatusSchedule(UInt64? HistorySize = null)
        {

            if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
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
                        return new Timestamped<ChargingStationStatusTypes>[] {
                                   new Timestamped<ChargingStationStatusTypes>(AdminStatus.Timestamp, ChargingStationStatusTypes.OutOfService)
                               };

                }

            }

        }

        #endregion

        #region StatusAggregationDelegate

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated EVSEs.
        /// </summary>
        public Func<EVSEStatusReport, ChargingStationStatusTypes> StatusAggregationDelegate { get; set; }

        #endregion


        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject                   CustomData               { get; }

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
                               Action<ChargingStation>?                       Configurator                   = null,
                               RemoteChargingStationCreatorDelegate?          RemoteChargingStationCreator   = null,
                               Timestamped<ChargingStationAdminStatusTypes>?  InitialAdminStatus             = null,
                               Timestamped<ChargingStationStatusTypes>?       InitialStatus                  = null,
                               UInt16                                         MaxAdminStatusListSize         = DefaultMaxAdminStatusListSize,
                               UInt16                                         MaxStatusListSize              = DefaultMaxStatusListSize)

            : this(Id,
                   null,
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
        /// <param name="MaxAdminStatusListSize">An optional max length of the admin staus list.</param>
        /// <param name="MaxStatusListSize">An optional max length of the staus list.</param>
        public ChargingStation(ChargingStation_Id                             Id,
                               ChargingPool                                   ChargingPool,
                               Action<ChargingStation>?                       Configurator                   = null,
                               RemoteChargingStationCreatorDelegate?          RemoteChargingStationCreator   = null,
                               Timestamped<ChargingStationAdminStatusTypes>?  InitialAdminStatus             = null,
                               Timestamped<ChargingStationStatusTypes>?       InitialStatus                  = null,
                               UInt16                                         MaxAdminStatusListSize         = DefaultMaxAdminStatusListSize,
                               UInt16                                         MaxStatusListSize              = DefaultMaxStatusListSize,

                               JObject?                                       CustomData                     = null,
                               IReadOnlyDictionary<String, Object>?           InternalData                   = null)

            : base(Id,
                   InternalData)

        {

            #region Init data and properties

            this.ChargingPool                = ChargingPool;

            InitialAdminStatus               = InitialAdminStatus != null ? InitialAdminStatus : new Timestamped<ChargingStationAdminStatusTypes>(ChargingStationAdminStatusTypes.Operational);
            InitialStatus                    = InitialStatus      != null ? InitialStatus      : new Timestamped<ChargingStationStatusTypes>     (ChargingStationStatusTypes.     Available);

            this._Name                       = new I18NString();
            this._Description                = new I18NString();
            this.openingTimes               = OpeningTimes.Open24Hours;
            this._Brands                     = new SpecialHashSet<ChargingStation, Brand_Id, Brand>(this);

            this._AdminStatusSchedule        = new StatusSchedule<ChargingStationAdminStatusTypes>(MaxAdminStatusListSize);
            this._AdminStatusSchedule.Insert(InitialAdminStatus.Value);

            this._StatusSchedule             = new StatusSchedule<ChargingStationStatusTypes>(MaxStatusListSize);
            this._StatusSchedule.Insert(InitialStatus.Value);

            this._EVSEs                      = new EntityHashSet<ChargingStation, EVSE_Id, EVSE>(this);

            this.CustomData                  = CustomData ?? new JObject();

            #endregion

            #region Init events

            // ChargingStation events
            this.EVSEAddition             = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval              = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events

            #endregion

            Configurator?.Invoke(this);

            #region Link events

            this.OnPropertyChanged += UpdateData;

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this._StatusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

            this.RemoteChargingStation = RemoteChargingStationCreator?.Invoke(this);

        }

        #endregion

        #endregion


        #region Data/(Admin-)Status

        #region SetStatus(NewStatus)

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="NewStatus">A new timestamped status.</param>
        public void SetStatus(Timestamped<ChargingStationStatusTypes>  NewStatus)
        {

            _StatusSchedule.Insert(NewStatus);

        }

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(ChargingStationAdminStatusTypes  NewAdminStatus)
        {

            _AdminStatusSchedule.Insert(NewAdminStatus);

        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<ChargingStationAdminStatusTypes> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(ChargingStationAdminStatusTypes  NewAdminStatus,
                                   DateTime                        Timestamp)
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
        public void SetAdminStatus(IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>  NewAdminStatusList,
                                   ChangeMethods                                             ChangeMethod = ChangeMethods.Replace)
        {

            _AdminStatusSchedule.Set(NewAdminStatusList, ChangeMethod);

        }

        #endregion


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

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSEAddition

            => EVSEAddition;

        #endregion

        #region EVSEs

        private readonly EntityHashSet<ChargingStation, EVSE_Id, EVSE> _EVSEs;

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging station.
        /// </summary>
        public IEnumerable<EVSE> EVSEs

            => _EVSEs;

        #endregion

        #region EVSEIds        (IncludeEVSEs = null)

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment (EVSEs)
        /// present within this charging station.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate IncludeEVSEs = null)

            => IncludeEVSEs == null

                   ? _EVSEs.
                         Select(evse => evse.Id)

                   : _EVSEs.
                         Where (evse => IncludeEVSEs(evse)).
                         Select(evse => evse.Id);

        #endregion

        #region EVSEAdminStatus(IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate IncludeEVSEs = null)

            => _EVSEs.
                   Where (evse => IncludeEVSEs(evse)).
                   Select(evse => new EVSEAdminStatus(evse.Id,
                                                      evse.AdminStatus));

        #endregion

        #region EVSEStatus     (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate IncludeEVSEs = null)

            => _EVSEs.
                   Where (evse => IncludeEVSEs(evse)).
                   Select(evse => new EVSEStatus(evse.Id,
                                                 evse.Status));

        #endregion


        #region CreateEVSE(EVSEId, Configurator = null, RemoteEVSECreator = null, ...)

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the new EVSE.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE after its creation.</param>
        /// <param name="RemoteEVSECreator">An optional delegate to configure a new remote EVSE after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the EVSE.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        public EVSE CreateEVSE(EVSE_Id                             EVSEId,
                               Action<EVSE>                        Configurator             = null,
                               RemoteEVSECreatorDelegate           RemoteEVSECreator        = null,
                               Timestamped<EVSEAdminStatusTypes>?  InitialAdminStatus       = null,
                               Timestamped<EVSEStatusTypes>?       InitialStatus            = null,
                               UInt16                              MaxAdminStatusListSize   = EVSE.DefaultMaxAdminStatusListSize,
                               UInt16                              MaxStatusListSize        = EVSE.DefaultMaxEVSEStatusListSize,
                               Action<EVSE>                        OnSuccess                = null,
                               Action<ChargingStation, EVSE_Id>    OnError                  = null)
        {

            lock (_EVSEs)
            {

                #region Initial checks

                if (EVSEId == null)
                    throw new ArgumentNullException(nameof(EVSEId),  "The given EVSE identification must not be null!");

                InitialAdminStatus = InitialAdminStatus ?? new Timestamped<EVSEAdminStatusTypes>(EVSEAdminStatusTypes.Operational);
                InitialStatus      = InitialStatus      ?? new Timestamped<EVSEStatusTypes>     (EVSEStatusTypes.Available);

                if (_EVSEs.Any(evse => evse.Id == EVSEId))
                {
                    if (OnError == null)
                        throw new EVSEAlreadyExistsInStation(this, EVSEId);
                    else
                        OnError?.Invoke(this, EVSEId);
                }

                if (!Operator.Ids.Contains(EVSEId.OperatorId))
                    throw new InvalidEVSEOperatorId(this,
                                                    EVSEId.OperatorId,
                                                    Operator.Ids);

                #endregion

                var Now   = DateTime.UtcNow;
                var _EVSE = new EVSE(EVSEId,
                                     this,
                                     Configurator,
                                     RemoteEVSECreator,
                                     InitialAdminStatus,
                                     InitialStatus,
                                     MaxAdminStatusListSize,
                                     MaxStatusListSize);

                if (EVSEAddition.SendVoting(Now, this, _EVSE) &&
                    _EVSEs.TryAdd(_EVSE))
                {

                    _EVSE.OnDataChanged           += UpdateEVSEData;
                    _EVSE.OnStatusChanged         += UpdateEVSEStatus;
                    _EVSE.OnAdminStatusChanged    += UpdateEVSEAdminStatus;

                    _EVSE.OnNewReservation        += SendNewReservation;
                    _EVSE.OnReservationCanceled   += SendReservationCanceled;
                    _EVSE.OnNewChargingSession    += SendNewChargingSession;
                    _EVSE.OnNewChargeDetailRecord += SendNewChargeDetailRecord;

                    //UpdateEVSEStatus(Now,
                    //                 EventTracking_Id.New,
                    //                 _EVSE,
                    //                 new Timestamped<EVSEStatusTypes>(Now, EVSEStatusTypes.Unspecified),
                    //                 _EVSE.Status).Wait();

                    if (RemoteChargingStation != null)
                    {

                        if (_EVSE.RemoteEVSE != null)
                            RemoteChargingStation.AddEVSE(_EVSE.RemoteEVSE);

                        OnAdminStatusChanged               += async (Timestamp, EventTrackingId, station, oldstatus, newstatus) => this.AdminStatus      = newstatus;
                        OnStatusChanged                    += async (Timestamp, EventTrackingId, station, oldstatus, newstatus) => this.Status           = newstatus;

                        this.RemoteChargingStation.OnAdminStatusChanged    += async (Timestamp, EventTrackingId, RemoteEVSE, OldStatus, NewStatus)  => AdminStatus                 = NewStatus;
                        this.RemoteChargingStation.OnStatusChanged         += async (Timestamp, EventTrackingId, RemoteEVSE, OldStatus, NewStatus)  => Status                      = NewStatus;

                        //RemoteConfigurator?.Invoke(_EVSE.RemoteEVSE);

                    }

                    OnSuccess?.Invoke(_EVSE);
                    EVSEAddition.SendNotification(Now, this, _EVSE);

                    return _EVSE;

                }

                Debug.WriteLine("EVSE '" + EVSEId + "' was not created!");
                return null;

            }

        }

        #endregion

        #region CreateOrUpdateEVSE(EVSEId, Configurator = null, RemoteEVSECreator = null, ...)

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the new EVSE.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new EVSE after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the EVSE failed.</param>
        public EVSE CreateOrUpdateEVSE(EVSE_Id                             EVSEId,
                                       Action<EVSE>                        Configurator             = null,
                                       RemoteEVSECreatorDelegate           RemoteEVSECreator        = null,
                                       Timestamped<EVSEAdminStatusTypes>?  InitialAdminStatus       = null,
                                       Timestamped<EVSEStatusTypes>?       InitialStatus            = null,
                                       UInt16                              MaxAdminStatusListSize   = EVSE.DefaultMaxAdminStatusListSize,
                                       UInt16                              MaxStatusListSize        = EVSE.DefaultMaxEVSEStatusListSize,
                                       Action<EVSE>                        OnSuccess                = null,
                                       Action<ChargingStation, EVSE_Id>    OnError                  = null)
        {

            #region Initial checks

            if (!Operator.Ids.Contains(EVSEId.OperatorId))
                throw new InvalidEVSEOperatorId(this,
                                                EVSEId.OperatorId,
                                                Operator.Ids);

            InitialAdminStatus = InitialAdminStatus ?? new Timestamped<EVSEAdminStatusTypes>(EVSEAdminStatusTypes.Operational);
            InitialStatus      = InitialStatus      ?? new Timestamped<EVSEStatusTypes>     (EVSEStatusTypes.Available);

            #endregion

            lock (_EVSEs)
            {

                #region If the EVSE identification is new/unknown: Call CreateEVSE(...)

                if (!_EVSEs.ContainsId(EVSEId))
                    return CreateEVSE(EVSEId,
                                      Configurator,
                                      RemoteEVSECreator,
                                      InitialAdminStatus,
                                      InitialStatus,
                                      MaxAdminStatusListSize,
                                      MaxStatusListSize,
                                      OnSuccess,
                                      OnError);

                #endregion


                // Merge existing EVSE with new EVSE data...
                return _EVSEs.
                           GetById(EVSEId).
                           UpdateWith(new EVSE(EVSEId,
                                               this,
                                               Configurator,
                                               null,
                                               new Timestamped<EVSEAdminStatusTypes>(DateTime.MinValue, EVSEAdminStatusTypes.Operational),
                                               new Timestamped<EVSEStatusTypes>     (DateTime.MinValue, EVSEStatusTypes.Available)));

            }

        }

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the charging station.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)

            => _EVSEs.Contains(EVSE);

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the charging station.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)

            => _EVSEs.ContainsId(EVSEId);

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)

            => _EVSEs.GetById(EVSEId);

        #endregion

        #region TryGetEVSEById(EVSEId, out EVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out EVSE EVSE)

            => _EVSEs.TryGet(EVSEId, out EVSE);

        #endregion

        #region RemoveEVSE(EVSEId)

        public EVSE RemoveEVSE(EVSE_Id EVSEId)

            => _EVSEs.Remove(EVSEId);

        #endregion

        #region TryRemoveEVSE(EVSEId, out EVSE)

        public Boolean TryRemoveEVSE(EVSE_Id EVSEId, out EVSE EVSE)

            => _EVSEs.TryRemove(EVSEId, out EVSE);

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
        /// Update the current charging station status.
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
        /// Update the current charging station status.
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

            if (StatusAggregationDelegate != null)
            {
                _StatusSchedule.Insert(StatusAggregationDelegate(new EVSEStatusReport(_EVSEs)),
                                       Timestamp);
            }

        }

        #endregion


        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSERemoval

            => EVSERemoval;

        #endregion

        #region IEnumerable<EVSE> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => _EVSEs.GetEnumerator();

        public IEnumerator<EVSE> GetEnumerator()
            => _EVSEs.GetEnumerator();

        #endregion

        #endregion


        #region Reservations

        #region Data

        private readonly Dictionary<ChargingReservation_Id, ChargingReservation> _Reservations;

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> Reservations
            => _Reservations.Select(_ => _.Value);

        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
            => _Reservations.TryGetValue(ReservationId, out Reservation);

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
                        eMobilityProvider_Id?    ProviderId             = null,
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

                    if (TryGetEVSEById(ChargingLocation.EVSEId.Value, out EVSE evse))
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
                            if (result.Session != null)
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
                       eMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            #region Initial checks

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

                    if (TryGetChargingSessionById(SessionId, out ChargingSession chargingSession) &&
                        chargingSession.EVSEId.HasValue &&
                        TryGetEVSEById(chargingSession.EVSEId.Value, out EVSE evse))
                    {

                        result = await evse.
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

            if (ParkingSpaces != null)
            {

                if (_ParkingSpaces == null)
                    _ParkingSpaces = new ReactiveSet<ParkingSpace>();

                _ParkingSpaces.Add(ParkingSpaces);

            }

        }



        #region ToJSON(this ChargingStation,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging station.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public JObject ToJSON(Boolean                                        Embedded                          = false,
                              InfoStatus                                     ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandEVSEIds                     = InfoStatus.Expanded,
                              InfoStatus                                     ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<ChargingStation>  CustomChargingStationSerializer   = null,
                              CustomJObjectSerializerDelegate<EVSE>             CustomEVSESerializer              = null)
        {

            var JSON = JSONObject.Create(

                         Id.ToJSON("@id"),

                         !Embedded
                             ? new JProperty("@context", JSONLDContext)
                             : null,

                         Name.       IsNeitherNullNorEmpty()
                             ? Name.       ToJSON("name")
                             : null,

                         Description.IsNeitherNullNorEmpty()
                             ? Description.ToJSON("description")
                             : null,

                         (!Embedded || DataSource != ChargingPool.DataSource)
                             ? DataSource.ToJSON("dataSource")
                             : null,

                         (!Embedded || DataLicenses != ChargingPool.DataLicenses)
                             ? ExpandDataLicenses.Switch(
                                 () => new JProperty("dataLicenseIds",  new JArray(DataLicenses.SafeSelect(license => license.Id.ToString()))),
                                 () => new JProperty("dataLicenses",    DataLicenses.ToJSON()))
                             : null,

                         ExpandRoamingNetworkId != InfoStatus.Hidden && RoamingNetwork != null
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

                         ExpandChargingStationOperatorId != InfoStatus.Hidden && Operator != null
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

                         ExpandChargingPoolId != InfoStatus.Hidden && ChargingPool != null
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

                         (!Embedded || GeoLocation         != ChargingPool.GeoLocation)         ? new JProperty("geoLocation",          GeoLocation.Value.  ToJSON())    : null,
                         (!Embedded || Address             != ChargingPool.Address)             ? new JProperty("address",              Address.            ToJSON())    : null,
                         (!Embedded || AuthenticationModes != ChargingPool.AuthenticationModes) ? new JProperty("authenticationModes",  new JArray(AuthenticationModes)) : null,
                         (!Embedded || HotlinePhoneNumber  != ChargingPool.HotlinePhoneNumber)  ? new JProperty("hotlinePhoneNumber",   HotlinePhoneNumber. ToJSON())    : null,
                         (!Embedded || OpeningTimes        != ChargingPool.OpeningTimes)        ? new JProperty("openingTimes",         OpeningTimes.       ToJSON())    : null,

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
                                                                                          CustomEVSESerializer:             CustomEVSESerializer).
                                                                                  Where  (evse => evse != null))))

                             : null,


                         ExpandBrandIds != InfoStatus.Hidden && Brands.Any()
                             ? ExpandBrandIds.Switch(

                                   () => new JProperty("brandIds",
                                                       new JArray(BrandIds.
                                                                                  OrderBy(brandId => brandId).
                                                                                  Select (brandId => brandId.ToString()))),

                                   () => new JProperty("brands",
                                                       new JArray(Brands.
                                                                                  OrderBy(brand => brand).
                                                                                  ToJSON (Embedded:                         true,
                                                                                          ExpandChargingPoolIds:            InfoStatus.Hidden,
                                                                                          ExpandChargingStationIds:         InfoStatus.Hidden,
                                                                                          ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                          ExpandDataLicenses:               InfoStatus.ShowIdOnly))))

                             : null

                     );

            return CustomChargingStationSerializer != null
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

            Name                 = OtherChargingStation.Name;
            Description          = OtherChargingStation.Description;
            _Brands.Clear();
            _Brands.TryAdd(OtherChargingStation.Brands);
            Address              = OtherChargingStation.Address;
            OSM_NodeId           = OtherChargingStation.OSM_NodeId;
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
                SetAdminStatus(OtherChargingStation.AdminStatus);

            if (OtherChargingStation.Status.Timestamp > Status.Timestamp)
                SetStatus(OtherChargingStation.Status);

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
        public static Boolean operator == (ChargingStation ChargingStation1, ChargingStation ChargingStation2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStation1, ChargingStation2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStation1 == null) || ((Object) ChargingStation2 == null))
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
        public static Boolean operator != (ChargingStation ChargingStation1, ChargingStation ChargingStation2)
            => !(ChargingStation1 == ChargingStation2);

        #endregion

        #region Operator <  (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStation ChargingStation1, ChargingStation ChargingStation2)
        {

            if ((Object) ChargingStation1 == null)
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
        public static Boolean operator <= (ChargingStation ChargingStation1, ChargingStation ChargingStation2)
            => !(ChargingStation1 > ChargingStation2);

        #endregion

        #region Operator >  (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStation ChargingStation1, ChargingStation ChargingStation2)
        {

            if ((Object) ChargingStation1 == null)
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
        public static Boolean operator >= (ChargingStation ChargingStation1, ChargingStation ChargingStation2)
            => !(ChargingStation1 < ChargingStation2);

        #endregion

        #endregion

        #region IComparable<ChargingStation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charging station.
            var ChargingStation = Object as ChargingStation;
            if ((Object) ChargingStation == null)
                throw new ArgumentException("The given object is not a charging station!");

            return CompareTo(ChargingStation);

        }

        #endregion

        #region CompareTo(ChargingStation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation">A charging station object to compare with.</param>
        public Int32 CompareTo(ChargingStation ChargingStation)
        {

            if ((Object) ChargingStation == null)
                throw new ArgumentNullException("The given charging station must not be null!");

            return Id.CompareTo(ChargingStation.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStation> Members

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

            // Check if the given object is a charging station.
            var ChargingStation = Object as ChargingStation;
            if ((Object) ChargingStation == null)
                return false;

            return this.Equals(ChargingStation);

        }

        #endregion

        #region Equals(ChargingStation)

        /// <summary>
        /// Compares two charging stations for equality.
        /// </summary>
        /// <param name="ChargingStation">A charging station to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStation ChargingStation)
        {

            if ((Object) ChargingStation == null)
                return false;

            return Id.Equals(ChargingStation.Id);

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
