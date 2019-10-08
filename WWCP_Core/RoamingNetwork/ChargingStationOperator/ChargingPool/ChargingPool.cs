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
    public static partial class ChargingPoolExtentions
    {

        #region ToJSON(this ChargingPools, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="Skip">The optional number of charging pools to skip.</param>
        /// <param name="Take">The optional number of charging pools to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JArray ToJSON(this IEnumerable<ChargingPool>                 ChargingPools,
                                    UInt64?                                        Skip                              = null,
                                    UInt64?                                        Take                              = null,
                                    Boolean                                        Embedded                          = false,
                                    InfoStatus                                     ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                     ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                                     ExpandChargingStationIds          = InfoStatus.Expand,
                                    InfoStatus                                     ExpandEVSEIds                     = InfoStatus.Hidden,
                                    InfoStatus                                     ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                                     ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                                    CustomJSONSerializerDelegate<ChargingPool>     CustomChargingPoolSerializer      = null,
                                    CustomJSONSerializerDelegate<ChargingStation>  CustomChargingStationSerializer   = null,
                                    CustomJSONSerializerDelegate<EVSE>             CustomEVSESerializer              = null)


            => ChargingPools == null || !ChargingPools.Any()

                   ? new JArray()

                   : new JArray(ChargingPools.
                                    Where         (pool => pool != null).
                                    OrderBy       (pool => pool.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (pool => pool.ToJSON(Embedded,
                                                                       ExpandRoamingNetworkId,
                                                                       ExpandChargingStationOperatorId,
                                                                       ExpandChargingStationIds,
                                                                       ExpandEVSEIds,
                                                                       ExpandBrandIds,
                                                                       ExpandDataLicenses,
                                                                       CustomChargingPoolSerializer,
                                                                       CustomChargingStationSerializer,
                                                                       CustomEVSESerializer)));


        #endregion

        #region ToJSON(this ChargingPools, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingPool> ChargingPools, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingPools?.Any() == true
                       ? new JProperty(JPropertyKey, ChargingPools.ToJSON())
                       : null;

        }

        #endregion


        #region ToJSON(this ChargingPoolAdminStatus,          Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<ChargingPoolAdminStatus>  ChargingPoolAdminStatus,
                                     UInt64?                                    Skip  = null,
                                     UInt64?                                    Take  = null)
        {

            #region Initial checks

            if (ChargingPoolAdminStatus == null || !ChargingPoolAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingPool_Id, ChargingPoolAdminStatus>();

            foreach (var status in ChargingPoolAdminStatus)
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

        #region ToJSON(this ChargingPoolAdminStatusSchedules, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<ChargingPoolAdminStatusSchedule>  ChargingPoolAdminStatusSchedules,
                                     UInt64?                                            Skip         = null,
                                     UInt64?                                            Take         = null,
                                     UInt64                                             HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingPoolAdminStatusSchedules == null || !ChargingPoolAdminStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingPool_Id, ChargingPoolAdminStatusSchedule>();

            foreach (var status in ChargingPoolAdminStatusSchedules)
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


        #region ToJSON(this ChargingPoolStatus,               Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<ChargingPoolStatus>  ChargingPoolStatus,
                                     UInt64?                               Skip  = null,
                                     UInt64?                               Take  = null)
        {

            #region Initial checks

            if (ChargingPoolStatus == null || !ChargingPoolStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingPool_Id, ChargingPoolStatus>();

            foreach (var status in ChargingPoolStatus)
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

        #region ToJSON(this ChargingPoolStatusSchedules,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<ChargingPoolStatusSchedule>  ChargingPoolStatusSchedules,
                                     UInt64?                                       Skip         = null,
                                     UInt64?                                       Take         = null,
                                     UInt64                                        HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingPoolStatusSchedules == null || !ChargingPoolStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingPool_Id, ChargingPoolStatusSchedule>();

            foreach (var status in ChargingPoolStatusSchedules)
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


    public interface IChargingPool : IEquatable<ChargingPool>, IComparable<ChargingPool>, IComparable,
                                     IEnumerable<ChargingStation>,
                                     IStatus<ChargingPoolStatusTypes>
    {

        /// <summary>
        /// The unique identification of this charging pool.
        /// </summary>
        ChargingPool_Id         Id                    { get; }

        /// <summary>
        /// The roaming network of this charging pool.
        /// </summary>
        IRoamingNetwork         RoamingNetwork        { get; }

        /// <summary>
        /// The charging station operator of this charging pool.
        /// </summary>
        [Optional]
        ChargingStationOperator Operator              { get; }

        /// <summary>
        /// The remote charging pool.
        /// </summary>
        [Optional]
        IRemoteChargingPool     RemoteChargingPool    { get; }



        I18NString Name         { get; }
        I18NString Description  { get; }

    }

    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging pool
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingPool : AEMobilityEntity<ChargingPool_Id>,
                                IChargingPool
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext                   = "https://open.charging.cloud/contexts/wwcp+json/chargingPool";


        /// <summary>
        /// The default max size of the charging pool (aggregated charging station) status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 15;

        /// <summary>
        /// The default max size of the charging pool admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 15;

        #endregion

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of this charging pool.
        /// </summary>
        [Mandatory]
        public I18NString Name
        {

            get
            {
                return _Name;
            }

            set
            {

                if (value == null)
                    value = new I18NString();

                if (_Name != value)
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

        private I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of this charging pool.
        /// </summary>
        [Optional]
        public I18NString Description
        {

            get
            {
                return _Description;
            }

            set
            {

                if (value == null)
                    value = new I18NString();

                if (_Description != value)
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

        internal readonly IVotingNotificator<DateTime, ChargingPool, Brand, Boolean> BrandAddition;

        /// <summary>
        /// Called whenever a brand will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, Brand, Boolean> OnBrandAddition

            => BrandAddition;

        #endregion

        #region Brands

        private readonly SpecialHashSet<ChargingPool, Brand_Id, Brand> _Brands;

        /// <summary>
        /// All brands registered within this charging station pool.
        /// </summary>
        public IEnumerable<Brand> Brands
            => _Brands;

        #endregion

        #region BrandIds

        /// <summary>
        /// All brand identifications registered within this charging station pool.
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

        internal readonly IVotingNotificator<DateTime, ChargingPool, Brand, Boolean> BrandRemoval;

        /// <summary>
        /// Called whenever a brand will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, Brand, Boolean> OnBrandRemoval

            => BrandRemoval;

        #endregion

        #region RemoveBrand(BrandId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All brands registered within this charging station pool.
        /// </summary>
        /// <param name="BrandId">The unique identification of the brand to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new brand after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the brand failed.</param>
        public Brand RemoveBrand(Brand_Id BrandId,
                                 Action<ChargingPool, Brand> OnSuccess = null,
                                 Action<ChargingPool, Brand_Id> OnError = null)
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
        /// All brands registered within this charging station pool.
        /// </summary>
        /// <param name="Brand">The brand to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new brand after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the brand failed.</param>
        public Brand RemoveBrand(Brand Brand,
                                 Action<ChargingPool, Brand> OnSuccess = null,
                                 Action<ChargingPool, Brand> OnError = null)
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
        /// The license of the charging pool data.
        /// </summary>
        [Mandatory]
        public ReactiveSet<DataLicense> DataLicenses
        {

            get
            {

                return _DataLicenses != null && _DataLicenses.Any()
                           ? _DataLicenses
                           : Operator?.DataLicenses;

            }

            set
            {

                if (value != _DataLicenses && value != Operator?.DataLicenses)
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
                    _ChargingStations.ForEach(station => station.Address = null);

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
                    _ChargingStations.ForEach(station => station.GeoLocation = null);

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
                    _ChargingStations.ForEach(station => station.EntranceAddress = null);

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
                    _ChargingStations.ForEach(station => station.EntranceLocation = null);

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
                return _ArrivalInstructions;
            }

            set
            {

                if (value == null)
                    value = new I18NString();

                if (_ArrivalInstructions != value)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _ArrivalInstructions);

                    else
                        SetProperty(ref _ArrivalInstructions, value);

                    // Delete inherited arrival instructions
                    _ChargingStations.ForEach(station => station.ArrivalInstructions = null);

                }

            }

        }

        #endregion

        #region OpeningTimes

        private OpeningTimes _OpeningTimes;

        /// <summary>
        /// The opening times of this charging pool.
        /// </summary>
        [Optional]
        public OpeningTimes OpeningTimes
        {

            get
            {
                return _OpeningTimes;
            }

            set
            {

                //if (value == null)
                //    value = OpeningTimes.Open24Hours;

                if (_OpeningTimes != value)
                {

                    // Can not be deleted!
                    // Will always be at least 'Open24Hours'!

                    SetProperty(ref _OpeningTimes, value);

                    // Delete inherited opening times
                    _ChargingStations.ForEach(station => station.OpeningTimes = null);

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
                    _ChargingStations.ForEach(station => station.UIFeatures = null);

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
                    _ChargingStations.ForEach(station => station.AuthenticationModes = null);

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
                    _ChargingStations.ForEach(station => station.PaymentOptions = null);

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
                    _ChargingStations.ForEach(station => station.Accessibility = null);

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
                    _ChargingStations.ForEach(station => station.PhotoURIs = null);

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
                    _ChargingStations.ForEach(station => station.HotlinePhoneNumber = null);

                }

            }

        }

        #endregion

        #region GridConnection

        private GridConnectionTypes? _GridConnection;

        /// <summary>
        /// The grid connection of the charging pool.
        /// </summary>
        [Optional]
        public GridConnectionTypes? GridConnection
        {

            get
            {
                return _GridConnection;
            }

            set
            {

                if (_GridConnection != value)
                {

                    if (value == null)
                        DeleteProperty(ref _GridConnection);

                    else
                        SetProperty(ref _GridConnection, value);

                    // Delete inherited grid connections
                    _ChargingStations.ForEach(station => station.GridConnection = null);

                }

            }

        }

        #endregion

        #region EnergyMix

        private EnergyMix _EnergyMix;

        /// <summary>
        /// The energy mix at the charging pool.
        /// </summary>
        [Optional]
        public EnergyMix EnergyMix
        {

            get
            {
                return _EnergyMix;
            }

            set
            {

                if (_EnergyMix != value)
                {

                    if (value == null)
                        DeleteProperty(ref _EnergyMix);

                    else
                        SetProperty(ref _EnergyMix, value);

                }

            }

        }

        #endregion

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
                    _ChargingStations.ForEach(station => station.ExitAddress = null);

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
                    _ChargingStations.ForEach(station => station.ExitLocation = null);

                }

            }

        }

        #endregion


        #region IsHubjectCompatible

        [Optional]
        public Partly IsHubjectCompatible

            => PartlyHelper.Generate(_ChargingStations.Select(station => station.IsHubjectCompatible));

        #endregion

        #region DynamicInfoAvailable

        [Optional]
        public Partly DynamicInfoAvailable

            => PartlyHelper.Generate(_ChargingStations.Select(station => station.DynamicInfoAvailable));

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current admin status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingPoolAdminStatusTypes> AdminStatus

            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<ChargingPoolAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>> AdminStatusSchedule(UInt64? HistorySize = null)
        {

            if (HistorySize.HasValue)
                return _AdminStatusSchedule.Take(HistorySize);

            return _AdminStatusSchedule;

        }

        #endregion


        #region Status

        /// <summary>
        /// The current status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingPoolStatusTypes> Status

            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<ChargingPoolStatusTypes> _StatusSchedule;

        /// <summary>
        /// The status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingPoolStatusTypes>> StatusSchedule(UInt64? HistorySize = null)
        {

            if (HistorySize.HasValue)
                return _StatusSchedule.Take(HistorySize);

            return _StatusSchedule;

        }

        #endregion

        #region StatusAggregationDelegate

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated charging stations.
        /// </summary>
        public Func<ChargingStationStatusReport, ChargingPoolStatusTypes> StatusAggregationDelegate { get; set; }

        #endregion

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
        public IRemoteChargingPool      RemoteChargingPool  { get; }


        /// <summary>
        /// The charging station operator of this charging pool.
        /// </summary>
        [Optional]
        public ChargingStationOperator  Operator            { get; }

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
                            Action<ChargingPool>                        Configurator                 = null,
                            RemoteChargingPoolCreatorDelegate           RemoteChargingPoolCreator    = null,
                            Timestamped<ChargingPoolAdminStatusTypes>?  InitialAdminStatus           = null,
                            Timestamped<ChargingPoolStatusTypes>?       InitialStatus                = null,
                            UInt16                                      MaxPoolAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                            UInt16                                      MaxPoolStatusListSize        = DefaultMaxStatusListSize)

            : this(Id,
                   null,
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
        /// <param name="MaxPoolStatusListSize">The default size of the charging pool (aggregated charging station) status list.</param>
        /// <param name="MaxPoolAdminStatusListSize">The default size of the charging pool admin status list.</param>
        public ChargingPool(ChargingPool_Id                             Id,
                            ChargingStationOperator                     Operator,
                            Action<ChargingPool>                        Configurator                 = null,
                            RemoteChargingPoolCreatorDelegate           RemoteChargingPoolCreator    = null,
                            Timestamped<ChargingPoolAdminStatusTypes>?  InitialAdminStatus           = null,
                            Timestamped<ChargingPoolStatusTypes>?       InitialStatus                = null,
                            UInt16                                      MaxPoolAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                            UInt16                                      MaxPoolStatusListSize        = DefaultMaxStatusListSize)

            : base(Id)

        {

            #region Init data and properties

            this.Operator                    = Operator;

            InitialAdminStatus               ??= new Timestamped<ChargingPoolAdminStatusTypes>(ChargingPoolAdminStatusTypes.Operational);
            InitialStatus                    ??= new Timestamped<ChargingPoolStatusTypes>     (ChargingPoolStatusTypes.     Available);

            this._Name                       = new I18NString();
            this._Description                = new I18NString();
            this._Brands                     = new SpecialHashSet<ChargingPool, Brand_Id, Brand>(this);

            this._AdminStatusSchedule        = new StatusSchedule<ChargingPoolAdminStatusTypes>(MaxPoolAdminStatusListSize);
            this._AdminStatusSchedule.Insert(InitialAdminStatus.Value);

            this._StatusSchedule             = new StatusSchedule<ChargingPoolStatusTypes>(MaxPoolStatusListSize);
            this._StatusSchedule.Insert(InitialStatus.Value);

            this._ChargingStations           = new EntityHashSet<ChargingPool, ChargingStation_Id, ChargingStation>(this);

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

            this._StatusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

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
        public event OnChargingPoolDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status changed.
        /// </summary>
        public event OnChargingPoolStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        #region SetStatus(NewStatus)

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="NewStatus">A new timestamped status.</param>
        public void SetStatus(Timestamped<ChargingPoolStatusTypes>  NewStatus)
        {

            _StatusSchedule.Insert(NewStatus);

        }

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(ChargingPoolAdminStatusTypes  NewAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<ChargingPoolAdminStatusTypes> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(ChargingPoolAdminStatusTypes  NewAdminStatus,
                                   DateTime                     Timestamp)
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
        public void SetAdminStatus(IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>  NewAdminStatusList,
                                   ChangeMethods                                          ChangeMethod = ChangeMethods.Replace)
        {

            _AdminStatusSchedule.Insert(NewAdminStatusList, ChangeMethod);

        }

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
            if (OnDataChangedLocal != null)
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
        internal async Task UpdateStatus(DateTime                             Timestamp,
                                         EventTracking_Id                     EventTrackingId,
                                         Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                         Timestamped<ChargingPoolStatusTypes>  NewStatus)
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

        #region Charging stations

        #region ChargingStations

        private readonly EntityHashSet<ChargingPool, ChargingStation_Id, ChargingStation> _ChargingStations;

        /// <summary>
        /// Return all charging stations registered within this charing pool.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations

            => _ChargingStations;

        #endregion

        #region ChargingStationIds        (IncludeStations = null)

        /// <summary>
        /// Return an enumeration of all charging station identifications.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate IncludeStations = null)

            => IncludeStations == null

                   ? _ChargingStations.
                         Select    (station => station.Id)

                   : _ChargingStations.
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

                   ? _ChargingStations.
                         Select    (station => new ChargingStationAdminStatus(station.Id, station.AdminStatus))

                   : _ChargingStations.
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

                   ? _ChargingStations.
                         Select    (station => new ChargingStationStatus(station.Id, station.Status))

                   : _ChargingStations.
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


        #region CreateChargingStation        (ChargingStationId, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station failed.</param>
        public ChargingStation CreateChargingStation(ChargingStation_Id                             ChargingStationId,
                                                     Action<ChargingStation>                        Configurator                   = null,
                                                     RemoteChargingStationCreatorDelegate           RemoteChargingStationCreator   = null,
                                                     Timestamped<ChargingStationAdminStatusTypes>?  InitialAdminStatus             = null,
                                                     Timestamped<ChargingStationStatusTypes>?       InitialStatus                  = null,
                                                     UInt16                                         MaxAdminStatusListSize         = ChargingStation.DefaultMaxAdminStatusListSize,
                                                     UInt16                                         MaxStatusListSize              = ChargingStation.DefaultMaxStatusListSize,
                                                     Action<ChargingStation>                        OnSuccess                      = null,
                                                     Action<ChargingPool, ChargingStation_Id>       OnError                        = null)
        {

            #region Initial checks

            if (_ChargingStations.ContainsId(ChargingStationId))
            {
                if (OnError == null)
                    throw new ChargingStationAlreadyExistsInPool(this, ChargingStationId);
                else
                    OnError?.Invoke(this, ChargingStationId);
            }

            if (!Operator.Ids.Contains(ChargingStationId.OperatorId))
                throw new InvalidChargingStationOperatorId(this,
                                                           ChargingStationId.OperatorId,
                                                           this.Operator.Ids);

            #endregion

            var _ChargingStation = new ChargingStation(ChargingStationId,
                                                       this,
                                                       Configurator,
                                                       RemoteChargingStationCreator,
                                                       InitialAdminStatus,
                                                       InitialStatus,
                                                       MaxAdminStatusListSize,
                                                       MaxStatusListSize);


            if (ChargingStationAddition.SendVoting(DateTime.UtcNow, this, _ChargingStation) &&
                _ChargingStations.TryAdd(_ChargingStation))
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

                        var __EVSE = GetEVSEbyId(reservation.EVSEId.Value);

                        //__EVSE.Reservation = reservation;

                    };

                    _ChargingStation.RemoteChargingStation.OnNewChargingSession += (a, b, session) => {

                        var __EVSE = GetEVSEbyId(session.EVSEId.Value);

                        //__EVSE.ChargingSession = session;

                    };

                    _ChargingStation.RemoteChargingStation.OnNewChargeDetailRecord += (a, b, cdr) => {

                        var __EVSE = GetEVSEbyId(cdr.EVSEId.Value);

                        __EVSE.SendNewChargeDetailRecord(DateTime.UtcNow, this, cdr);

                    };


                    _ChargingStation.RemoteChargingStation.OnReservationCanceled += _ChargingStation.SendReservationCanceled;

                    _ChargingStation.RemoteChargingStation.OnEVSEStatusChanged += (Timestamp,
                                                                                   EventTrackingId,
                                                                                   EVSE,
                                                                                   OldStatus,
                                                                                   NewStatus)

                        => _ChargingStation.UpdateEVSEStatus(Timestamp,
                                                             EventTrackingId,
                                                             GetEVSEbyId(EVSE.Id),
                                                             OldStatus,
                                                             NewStatus);

                }

                OnSuccess?.Invoke(_ChargingStation);
                ChargingStationAddition.SendNotification(DateTime.UtcNow, this, _ChargingStation);

                return _ChargingStation;

            }

            Debug.WriteLine("ChargingStation '" + ChargingStationId + "' could not be created!");

            if (OnError == null)
                throw new ChargingStationCouldNotBeCreated(this, ChargingStationId);

            OnError?.Invoke(this, ChargingStationId);
            return null;

        }

        #endregion

        #region CreateOrUpdateChargingStation(ChargingStationId, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station before its successful creation.</param>
        /// <param name="RemoteChargingStationCreator">A delegate to attach a remote charging station.</param>
        /// <param name="InitialAdminStatus">An optional initial admin status of the EVSE.</param>
        /// <param name="InitialStatus">An optional initial status of the EVSE.</param>
        /// <param name="MaxAdminStatusListSize">An optional max length of the admin staus list.</param>
        /// <param name="MaxStatusListSize">An optional max length of the staus list.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station failed.</param>
        public ChargingStation CreateOrUpdateChargingStation(ChargingStation_Id                             ChargingStationId,
                                                             Action<ChargingStation>                        Configurator                   = null,
                                                             RemoteChargingStationCreatorDelegate           RemoteChargingStationCreator   = null,
                                                             Timestamped<ChargingStationAdminStatusTypes>?  InitialAdminStatus             = null,
                                                             Timestamped<ChargingStationStatusTypes>?       InitialStatus                  = null,
                                                             UInt16                                         MaxAdminStatusListSize         = ChargingStation.DefaultMaxAdminStatusListSize,
                                                             UInt16                                         MaxStatusListSize              = ChargingStation.DefaultMaxStatusListSize,
                                                             Action<ChargingStation>                        OnSuccess                      = null,
                                                             Action<ChargingPool, ChargingStation_Id>       OnError                        = null)
        {

            lock (_ChargingStations)
            {

                #region Initial checks

                if (!Operator.Ids.Contains(ChargingStationId.OperatorId))
                    throw new InvalidChargingStationOperatorId(this,
                                                               ChargingStationId.OperatorId,
                                                               Operator.Ids);

                #endregion

                #region If the charging pool identification is new/unknown: Call CreateChargingPool(...)

                if (!_ChargingStations.ContainsId(ChargingStationId))
                    return CreateChargingStation(ChargingStationId,
                                                 Configurator,
                                                 RemoteChargingStationCreator,
                                                 InitialAdminStatus,
                                                 InitialStatus,
                                                 MaxAdminStatusListSize,
                                                 MaxStatusListSize,
                                                 OnSuccess,
                                                 OnError);

                #endregion


                // Merge existing charging station with new station data...

                return _ChargingStations.
                           GetById(ChargingStationId).
                           UpdateWith(new ChargingStation(ChargingStationId,
                                                          this,
                                                          Configurator,
                                                          null,
                                                          new Timestamped<ChargingStationAdminStatusTypes>(DateTime.MinValue, ChargingStationAdminStatusTypes.Operational),
                                                          new Timestamped<ChargingStationStatusTypes>(DateTime.MinValue, ChargingStationStatusTypes.Available)));

            }

        }

        #endregion


        #region ContainsChargingStation(ChargingStation)

        /// <summary>
        /// Check if the given ChargingStation is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStationId(ChargingStation ChargingStation)

            => _ChargingStations.ContainsId(ChargingStation.Id);

        #endregion

        #region ContainsChargingStation(ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)

            => _ChargingStations.ContainsId(ChargingStationId);

        #endregion

        #region GetChargingStationById(ChargingStationId)

        public ChargingStation GetChargingStationById(ChargingStation_Id ChargingStationId)

            => _ChargingStations.GetById(ChargingStationId);

        #endregion

        #region TryGetChargingStationById(ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)

            => _ChargingStations.TryGet(ChargingStationId, out ChargingStation);

        #endregion

        #region RemoveChargingStation(ChargingStationId)

        public ChargingStation RemoveChargingStation(ChargingStation_Id ChargingStationId)
        {

            ChargingStation _ChargingStation = null;

            if (TryGetChargingStationById(ChargingStationId, out _ChargingStation))
            {

                if (ChargingStationRemoval.SendVoting(DateTime.UtcNow, this, _ChargingStation))
                {

                    if (_ChargingStations.TryRemove(ChargingStationId, out _ChargingStation))
                    {

                        ChargingStationRemoval.SendNotification(DateTime.UtcNow, this, _ChargingStation);

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

                if (ChargingStationRemoval.SendVoting(DateTime.UtcNow, this, ChargingStation))
                {

                    if (_ChargingStations.TryRemove(ChargingStationId, out ChargingStation))
                    {

                        ChargingStationRemoval.SendNotification(DateTime.UtcNow, this, ChargingStation);

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
            if (OnChargingStationStatusChangedLocal != null)
                await OnChargingStationStatusChangedLocal(Timestamp,
                                                          EventTrackingId,
                                                          ChargingStation,
                                                          OldStatus,
                                                          NewStatus);

            if (StatusAggregationDelegate != null)
            {
                _StatusSchedule.Insert(StatusAggregationDelegate(new ChargingStationStatusReport(_ChargingStations)),
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
            => _ChargingStations.GetEnumerator();

        public IEnumerator<ChargingStation> GetEnumerator()
            => _ChargingStations.GetEnumerator();

        #endregion


        #region TryGetChargingStationByEVSEId(EVSEId, out Station)

        public Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out ChargingStation Station)
        {

            foreach (var station in _ChargingStations)
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

            => _ChargingStations.
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

                   ? _ChargingStations.
                         SelectMany(station => station.EVSEs).
                         Select    (evse    => evse.Id)

                   : _ChargingStations.
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

            => _ChargingStations.
                   SelectMany(station => station.EVSEAdminStatus(IncludeEVSEs));

        #endregion

        #region EVSEAdminStatusSchedule(IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatusSchedule(IncludeEVSEDelegate IncludeEVSEs = null)

            => _ChargingStations.
                   SelectMany(station => station.EVSEAdminStatus(IncludeEVSEs));

        #endregion

        #region EVSEStatus             (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate IncludeEVSEs = null)

            => _ChargingStations.
                   SelectMany(station => station.EVSEStatus(IncludeEVSEs));

        #endregion

        #region EVSEStatusSchedule     (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatusSchedule(IncludeEVSEDelegate IncludeEVSEs = null)

            => _ChargingStations.
                   SelectMany(station => station.EVSEStatus(IncludeEVSEs));

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)

            => _ChargingStations.Any(ChargingStation => ChargingStation.EVSEIds().Contains(EVSE.Id));

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)

            => _ChargingStations.Any(ChargingStation => ChargingStation.EVSEIds().Contains(EVSEId));

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)

            => _ChargingStations.
                   SelectMany    (station => station.EVSEs).
                   FirstOrDefault(EVSE    => EVSE.Id == EVSEId);

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = _ChargingStations.
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
                                             EventTrackingId,
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
                                                    EventTrackingId,
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
                                               EventTrackingId,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnReserveRequest));
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnReserveResponse));
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
                              eMobilityProvider_Id?                  ProviderId         = null,

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
            CancelReservationResult result               = null;

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
                e.Log(nameof(ChargingPool) + "." + nameof(OnCancelReservationRequest));
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnCancelReservationResponse));
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
                        eMobilityProvider_Id?    ProviderId             = null,
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
                e.Log(nameof(ChargingStation) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if ((ChargingLocation.EVSEId.           HasValue && TryGetChargingStationByEVSEId(ChargingLocation.EVSEId.           Value, out ChargingStation Station)) ||
                        (ChargingLocation.ChargingStationId.HasValue && TryGetChargingStationById    (ChargingLocation.ChargingStationId.Value, out                 Station)))
                    {

                        result = await Station.RemoteStart(ChargingLocation,
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

                        if (result != null &&
                            result.Result == RemoteStartResultType.Success)
                        {

                            // The session can be delivered within the response
                            // or via an explicit message afterwards!
                            if (result.Session != null)
                                result.Session.ChargingPool = this;

                        }

                        #endregion

                    }
                    else
                        result = RemoteStartResult.UnknownLocation;

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
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteStartResponse));
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
                                            ProviderId,
                                            RemoteAuthentication,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteStopRequest));
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

                        result = await chargingStation.
                                          RemoteStop(SessionId,
                                                     ReservationHandling,
                                                     ProviderId,
                                                     RemoteAuthentication,

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout);

                    if (result == null)
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteStopResponse));
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



        #region ToJSON(this ChargingPool,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging pool.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public JObject ToJSON(Boolean                                        Embedded                          = false,
                              InfoStatus                                     ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandChargingStationIds          = InfoStatus.Expand,
                              InfoStatus                                     ExpandEVSEIds                     = InfoStatus.Hidden,
                              InfoStatus                                     ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                              CustomJSONSerializerDelegate<ChargingPool>     CustomChargingPoolSerializer      = null,
                              CustomJSONSerializerDelegate<ChargingStation>  CustomChargingStationSerializer   = null,
                              CustomJSONSerializerDelegate<EVSE>             CustomEVSESerializer              = null)
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

                         (!Embedded || DataSource != Operator.DataSource)
                             ? DataSource.ToJSON("dataSource")
                             : null,

                         ExpandDataLicenses.Switch(
                             () => new JProperty("dataLicenseIds",  new JArray(DataLicenses.SafeSelect(license => license.Id.ToString()))),
                             () => new JProperty("dataLicenses",    DataLicenses.ToJSON())),


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

                         GeoLocation.        ToJSON("geoLocation"),
                         Address.            ToJSON("address"),
                         AuthenticationModes.ToJSON("authenticationModes"),
                         HotlinePhoneNumber. ToJSON("hotlinePhoneNumber"),
                         OpeningTimes.       ToJSON("openingTimes"),


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
                                                                                                  ExpandEVSEIds:                    InfoStatus.Expand,
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
                                                       new JArray(BrandIds.
                                                                               OrderBy(brandId => brandId).
                                                                               Select (brandId => brandId.ToString()))),

                                   () => new JProperty("brands",
                                                       new JArray(Brands.
                                                                               OrderBy(brand => brand).
                                                                               ToJSON (Embedded:                  true,
                                                                                       ExpandChargingPoolIds:     InfoStatus.Hidden,
                                                                                       ExpandChargingStationIds:  InfoStatus.Hidden,
                                                                                       ExpandEVSEIds:             InfoStatus.Hidden,
                                                                                       ExpandDataLicenses:        InfoStatus.ShowIdOnly))))

                             : null

                     );

            return CustomChargingPoolSerializer != null
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

            Name                 = OtherChargingPool.Name;
            Description          = OtherChargingPool.Description;
            _Brands.Clear();
            _Brands.TryAdd(OtherChargingPool.Brands);
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
                SetAdminStatus(OtherChargingPool.AdminStatus);

            if (OtherChargingPool.Status.Timestamp > Status.Timestamp)
                SetStatus(OtherChargingPool.Status);

            return this;

        }

        #endregion


        #region IComparable<ChargingPool> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var ChargingPool = Object as ChargingPool;
            if ((Object) ChargingPool == null)
                throw new ArgumentException("The given object is not a charging pool!", nameof(Object));

            return CompareTo(ChargingPool);

        }

        #endregion

        #region CompareTo(ChargingPool)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPool">A charging pool object to compare with.</param>
        public Int32 CompareTo(ChargingPool ChargingPool)
        {

            if ((Object) ChargingPool == null)
                throw new ArgumentNullException(nameof(Object), "The given charging pool must not be null!");

            return Id.CompareTo(ChargingPool.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPool> Members

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

            var ChargingPool = Object as ChargingPool;
            if ((Object) ChargingPool == null)
                return false;

            return Equals(ChargingPool);

        }

        #endregion

        #region Equals(ChargingPool)

        /// <summary>
        /// Compares two charging pools for equality.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPool ChargingPool)
        {

            if ((Object) ChargingPool == null)
                return false;

            return Id.Equals(ChargingPool.Id);

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
