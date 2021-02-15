/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod.JSON;
using org.GraphDefined.WWCP.Net.IO.JSON;

#endregion

namespace org.GraphDefined.WWCP
{


    /// <summary>
    /// WWCP JSON I/O EVSE extentions.
    /// </summary>
    public static partial class EVSEExtentions
    {

        #region ToJSON(this EVSEs, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="Skip">The optional number of EVSEs to skip.</param>
        /// <param name="Take">The optional number of EVSEs to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        public static JArray ToJSON(this IEnumerable<EVSE>              EVSEs,
                                    UInt64?                             Skip                              = null,
                                    UInt64?                             Take                              = null,
                                    Boolean                             Embedded                          = false,
                                    InfoStatus                          ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                          ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                          ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                          ExpandChargingStationId           = InfoStatus.ShowIdOnly,
                                    InfoStatus                          ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                          ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<EVSE>  CustomEVSESerializer              = null)


            => EVSEs == null || !EVSEs.Any()

                   ? null

                   : new JArray(EVSEs.
                                    Where         (evse => evse != null).
                                    OrderBy       (evse => evse.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (evse => evse.ToJSON(Embedded,
                                                                       ExpandRoamingNetworkId,
                                                                       ExpandChargingStationOperatorId,
                                                                       ExpandChargingPoolId,
                                                                       ExpandChargingStationId,
                                                                       ExpandBrandIds,
                                                                       ExpandDataLicenses,
                                                                       CustomEVSESerializer)));

        #endregion

        #region ToJSON(this EVSEs, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<EVSE> EVSEs, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return EVSEs?.Any() == true
                       ? new JProperty(JPropertyKey, EVSEs.ToJSON())
                       : null;

        }

        #endregion


        #region ToJSON(this EVSEAdminStatus,          Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EVSEAdminStatus>  EVSEAdminStatus,
                                     UInt64?                            Skip  = null,
                                     UInt64?                            Take  = null)
        {

            #region Initial checks

            if (EVSEAdminStatus == null || !EVSEAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<EVSE_Id, EVSEAdminStatus>();

            foreach (var status in EVSEAdminStatus)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region ToJSON(this EVSEAdminStatusSchedules, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EVSEAdminStatusSchedule>  EVSEAdminStatusSchedules,
                                     UInt64?                                    Skip  = null,
                                     UInt64?                                    Take  = null)
        {

            #region Initial checks

            if (EVSEAdminStatusSchedules == null || !EVSEAdminStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<EVSE_Id, EVSEAdminStatusSchedule>();

            foreach (var status in EVSEAdminStatusSchedules)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].StatusSchedule.Any() &&
                         _FilteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JObject(
                                                                   kvp.Value.StatusSchedule.
                                                                                 // Filter multiple status having the exact same ISO 8601 timestamp!
                                                                                 GroupBy(status => status.Timestamp.ToIso8601()).
                                                                                 Select (group => new JProperty(group.First().Timestamp.ToIso8601(),
                                                                                                                group.Select(status => status.Value.ToString()).AggregateWith(",")))
                                                              ))));

        }

        #endregion


        #region ToJSON(this EVSEStatus,               Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EVSEStatus>  EVSEStatus,
                                     UInt64?                       Skip  = null,
                                     UInt64?                       Take  = null)
        {

            #region Initial checks

            if (EVSEStatus == null || !EVSEStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<EVSE_Id, EVSEStatus>();

            foreach (var status in EVSEStatus)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region ToJSON(this EVSEStatusSchedules,      Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EVSEStatusSchedule>  EVSEStatusSchedules,
                                     UInt64?                               Skip  = null,
                                     UInt64?                               Take  = null)
        {

            #region Initial checks

            if (EVSEStatusSchedules == null || !EVSEStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<EVSE_Id, EVSEStatusSchedule>();

            foreach (var status in EVSEStatusSchedules)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].StatusSchedule.Any() &&
                         _FilteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select (kvp    => new JProperty(kvp.Key.ToString(),
                                                                   new JObject(
                                                                       kvp.Value.StatusSchedule.
                                                                                     // Filter multiple status having the exact same ISO 8601 timestamp!
                                                                                     GroupBy(status => status.Timestamp.ToIso8601()).
                                                                                     Select (group => new JProperty(group.First().Timestamp.ToIso8601(),
                                                                                                                    group.Select(status => status.Value.ToString()).AggregateWith(",")))
                                                                  ))));

        }

        #endregion

    }


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

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext = "https://open.charging.cloud/contexts/wwcp+json/EVSE";


        private readonly Decimal EPSILON = 0.01m;

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

        #region Brands

        #region BrandAddition

        internal readonly IVotingNotificator<DateTime, EVSE, Brand, Boolean> BrandAddition;

        /// <summary>
        /// Called whenever a brand will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EVSE, Brand, Boolean> OnBrandAddition

            => BrandAddition;

        #endregion

        #region Brands

        private readonly SpecialHashSet<EVSE, Brand_Id, Brand> _Brands;

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

        internal readonly IVotingNotificator<DateTime, EVSE, Brand, Boolean> BrandRemoval;

        /// <summary>
        /// Called whenever a brand will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EVSE, Brand, Boolean> OnBrandRemoval

            => BrandRemoval;

        #endregion

        #region RemoveBrand(BrandId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All brands registered within this charging station.
        /// </summary>
        /// <param name="BrandId">The unique identification of the brand to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new brand after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the brand failed.</param>
        public Brand RemoveBrand(Brand_Id                BrandId,
                                 Action<EVSE, Brand>     OnSuccess  = null,
                                 Action<EVSE, Brand_Id>  OnError    = null)
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
        public Brand RemoveBrand(Brand                Brand,
                                 Action<EVSE, Brand>  OnSuccess  = null,
                                 Action<EVSE, Brand>  OnError    = null)
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
                            SetProperty(ref _DataLicenses,
                                        value,
                                        EventTracking_Id.New);

                        else
                            SetProperty(ref _DataLicenses,
                                        _DataLicenses.Set(value),
                                        EventTracking_Id.New);

                    }

                }

            }

        }

        #endregion


        #region ChargingModes

        private ReactiveSet<ChargingModes> _ChargingModes;

        /// <summary>
        /// Charging modes.
        /// </summary>
        [Mandatory]
        public ReactiveSet<ChargingModes> ChargingModes
        {

            get
            {
                return _ChargingModes;
            }

            set
            {

                if (value.IsNullOrEmpty())
                    DeleteProperty(ref _ChargingModes);

                else
                {

                    if (_ChargingModes == null)
                        SetProperty(ref _ChargingModes,
                                    value,
                                    EventTracking_Id.New);

                    else
                        SetProperty(ref _ChargingModes,
                                    _ChargingModes.Set(value),
                                    EventTracking_Id.New);

                }

            }

        }

        #endregion

        #region CurrentType

        private CurrentTypes? _CurrentType;

        /// <summary>
        /// The type of the current.
        /// </summary>
        [Mandatory]
        public CurrentTypes? CurrentType
        {

            get
            {
                return _CurrentType;
            }

            set
            {

                if (_CurrentType != value)
                    SetProperty(ref _CurrentType,
                                value,
                                EventTracking_Id.New);

            }

        }

        #endregion

        #region AverageVoltage

        private Decimal? _AverageVoltage;

        /// <summary>
        /// The average voltage.
        /// </summary>
        [Mandatory]
        public Decimal? AverageVoltage
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
                        SetProperty(ref _AverageVoltage,
                                    value,
                                    EventTracking_Id.New);

                }

                else
                    DeleteProperty(ref _AverageVoltage);

            }

        }

        #endregion


        #region MaxCurrent

        private Decimal? _MaxCurrent;

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public Decimal? MaxCurrent
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
                        SetProperty(ref _MaxCurrent,
                                    value,
                                    EventTracking_Id.New);

                    else if (Math.Abs(_MaxCurrent.Value - value.Value) > EPSILON)
                        SetProperty(ref _MaxCurrent,
                                    value,
                                    EventTracking_Id.New);

                }

                else
                    DeleteProperty(ref _MaxCurrent);

            }

        }

        #endregion

        #region MaxCurrentRealTime

        private Timestamped<Decimal>? _MaxCurrentRealTime;

        /// <summary>
        /// The real-time maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public Timestamped<Decimal>? MaxCurrentRealTime
        {

            get
            {
                return _MaxCurrentRealTime;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxCurrentRealTime,
                                value,
                                EventTracking_Id.New);

                else
                    DeleteProperty(ref _MaxCurrentRealTime);

            }

        }

        #endregion

        #region MaxCurrentPrognoses

        private IEnumerable<Timestamped<Decimal>> _MaxCurrentPrognoses;

        /// <summary>
        /// Prognoses on future values of the maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public IEnumerable<Timestamped<Decimal>> MaxCurrentPrognoses
        {

            get
            {
                return _MaxCurrentPrognoses;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxCurrentPrognoses,
                                value,
                                EventTracking_Id.New);

                else
                    DeleteProperty(ref _MaxCurrentPrognoses);

            }

        }

        #endregion


        #region MaxPower

        private Decimal? _MaxPower;

        /// <summary>
        /// The maximum power [kWatt].
        /// </summary>
        [Optional]
        public Decimal? MaxPower
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
                        SetProperty(ref _MaxPower,
                                    value,
                                    EventTracking_Id.New);

                }

                else
                    DeleteProperty(ref _MaxPower);

            }

        }

        #endregion

        #region MaxPowerRealTime

        private Timestamped<Decimal>? _MaxPowerRealTime;

        /// <summary>
        /// The real-time maximum power [kWatt].
        /// </summary>
        [Mandatory]
        public Timestamped<Decimal>? MaxPowerRealTime
        {

            get
            {
                return _MaxPowerRealTime;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxPowerRealTime,
                                value,
                                EventTracking_Id.New);

                else
                    DeleteProperty(ref _MaxPowerRealTime);

            }

        }

        #endregion

        #region MaxPowerPrognoses

        private IEnumerable<Timestamped<Decimal>> _MaxPowerPrognoses;

        /// <summary>
        /// Prognoses on future values of the maximum power [kWatt].
        /// </summary>
        [Mandatory]
        public IEnumerable<Timestamped<Decimal>> MaxPowerPrognoses
        {

            get
            {
                return _MaxPowerPrognoses;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxPowerPrognoses,
                                value,
                                EventTracking_Id.New);

                else
                    DeleteProperty(ref _MaxPowerPrognoses);

            }

        }

        #endregion


        #region MaxCapacity

        private Decimal? _MaxCapacity;

        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public Decimal? MaxCapacity
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
                        SetProperty(ref _MaxCapacity,
                                    value,
                                    EventTracking_Id.New);

                }

                else
                    DeleteProperty(ref _MaxCapacity);

            }

        }

        #endregion

        #region MaxCapacityRealTime

        private Timestamped<Decimal>? _MaxCapacityRealTime;

        /// <summary>
        /// The real-time maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public Timestamped<Decimal>? MaxCapacityRealTime
        {

            get
            {
                return _MaxCapacityRealTime;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxCapacityRealTime,
                                value,
                                EventTracking_Id.New);

                else
                    DeleteProperty(ref _MaxCapacityRealTime);

            }

        }

        #endregion

        #region MaxCapacityPrognoses

        private IEnumerable<Timestamped<Decimal>> _MaxCapacityPrognoses;

        /// <summary>
        /// Prognoses on future values of the maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public IEnumerable<Timestamped<Decimal>> MaxCapacityPrognoses
        {

            get
            {
                return _MaxCapacityPrognoses;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _MaxCapacityPrognoses,
                                value,
                                EventTracking_Id.New);

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
                    SetProperty(ref _MaxReservationDuration,
                                value,
                                EventTracking_Id.New);

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
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the status history.</param>
        public IEnumerable<Timestamped<EVSEAdminStatusTypes>> AdminStatusSchedule(Func<DateTime,             Boolean> TimestampFilter  = null,
                                                                                  Func<EVSEAdminStatusTypes, Boolean> StatusFilter     = null,
                                                                                  UInt64?                             HistorySize      = null)
        {

            if (TimestampFilter == null)
                TimestampFilter = timestamp => true;

            if (StatusFilter    == null)
                StatusFilter    = status    => true;

            var filteredStatusSchedule = _AdminStatusSchedule.
                                             Where(status => TimestampFilter(status.Timestamp)).
                                             Where(status => StatusFilter   (status.Value));

            return HistorySize.HasValue
                       ? filteredStatusSchedule.Take(HistorySize)
                       : filteredStatusSchedule;

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
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the status history.</param>
        public IEnumerable<Timestamped<EVSEStatusTypes>> StatusSchedule(Func<DateTime,        Boolean> TimestampFilter  = null,
                                                                        Func<EVSEStatusTypes, Boolean> StatusFilter     = null,
                                                                        UInt64?                        HistorySize      = null)
        {

             if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                 AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
             {

                if (TimestampFilter == null)
                    TimestampFilter = timestamp => true;

                if (StatusFilter    == null)
                    StatusFilter    = status    => true;

                var filteredStatusSchedule = _StatusSchedule.
                                                 Where(status => TimestampFilter(status.Timestamp)).
                                                 Where(status => StatusFilter   (status.Value));

                return HistorySize.HasValue
                           ? filteredStatusSchedule.Take(HistorySize)
                           : filteredStatusSchedule;

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

            InitialAdminStatus          ??= new Timestamped<EVSEAdminStatusTypes>(EVSEAdminStatusTypes.Operational);
            InitialStatus               ??= new Timestamped<EVSEStatusTypes>     (EVSEStatusTypes.Available);

            this._Description           = new I18NString();
            this._ChargingModes         = new ReactiveSet<ChargingModes>();
            this._SocketOutlets         = new ReactiveSet<SocketOutlet>();
            this._Brands                = new SpecialHashSet<EVSE, Brand_Id, Brand>(this);

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
            CurrentType          = OtherEVSE.CurrentType;
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


        //public void AddChargingMode(ChargingModes ChargingMode)
        //{

        //    if (!_ChargingModes.HasValue)
        //        _ChargingModes = ChargingMode;

        //    else
        //        _ChargingModes |= ChargingMode;

        //}

        public void AddCurrentType(CurrentTypes CurrentType)
        {

            if (!_CurrentType.HasValue)
                _CurrentType = CurrentType;

            else
                _CurrentType |= CurrentType;

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
            _StatusSchedule.Set(NewStatusList, ChangeMethod);
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
            _AdminStatusSchedule.Set(NewAdminStatusList, ChangeMethod);
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
                                         EventTrackingId ?? EventTracking_Id.New,
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
        internal async Task UpdateStatus(DateTime                      Timestamp,
                                         EventTracking_Id              EventTrackingId,
                                         Timestamped<EVSEStatusTypes>  OldStatus,
                                         Timestamped<EVSEStatusTypes>  NewStatus)
        {

            var OnStatusChangedLocal = OnStatusChanged;
            if (OnStatusChangedLocal != null)
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
        public ChargingSession ChargingSession { get; internal set; }


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
                                             null,
                                             null,
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
                        ChargingSession      = result.Session;
                        result.Session.EVSE  = this;
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
                                            null,
                                            null,
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


        #region ToJSON(this EVSE, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given EVSE.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        public JObject ToJSON(Boolean                                Embedded                          = false,
                              InfoStatus                             ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                             ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                             ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                              InfoStatus                             ExpandChargingStationId           = InfoStatus.ShowIdOnly,
                              InfoStatus                             ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                             ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<EVSE>  CustomEVSESerializer              = null)

        {


            try
            {

                var JSON = JSONObject.Create(

                             Id.ToJSON("@id"),

                             !Embedded
                                 ? new JProperty("@context",  JSONLDContext)
                                 : null,

                             Description.IsNeitherNullNorEmpty()
                                 ? Description.ToJSON("description")
                                 : null,

                             Brands.SafeAny()
                                   ? ExpandBrandIds.Switch(
                                         () => new JProperty("brandId",      Brands.Select(brand => brand.Id.ToString())),
                                         () => new JProperty("brand",        Brands.ToJSON()))
                                   : null,

                             (!Embedded || DataSource != ChargingStation.DataSource)
                                 ? DataSource.ToJSON("dataSource")
                                 : null,

                             (!Embedded || DataLicenses != ChargingStation.DataLicenses)
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
                                       () => new JProperty("chargingPoolId",                    ChargingPool.Id.   ToString()),
                                       () => new JProperty("chargingPool",                      ChargingPool.      ToJSON(Embedded:                          true,
                                                                                                                               ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                               ExpandChargingStationOperatorId:   InfoStatus.Hidden,
                                                                                                                               ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                               ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                               ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                               ExpandDataLicenses:                InfoStatus.Hidden)))
                                 : null,

                             ExpandChargingStationId != InfoStatus.Hidden && ChargingStation != null
                                 ? ExpandChargingStationId.Switch(
                                       () => new JProperty("chargingStationId",                 ChargingStation.Id.ToString()),
                                       () => new JProperty("chargingStation",                   ChargingStation.   ToJSON(Embedded:                          true,
                                                                                                                               ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                               ExpandChargingStationOperatorId:   InfoStatus.Hidden,
                                                                                                                               ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                               ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                               ExpandDataLicenses:                InfoStatus.Hidden)))
                                 : null,

                             !Embedded ? ChargingStation.GeoLocation.Value.  ToJSON("geoLocation")         : null,
                             !Embedded ? ChargingStation.Address.            ToJSON("address")             : null,
                             !Embedded ? ChargingStation.AuthenticationModes.ToJSON("authenticationModes") : null,

                             ChargingModes.SafeAny()
                                 ? new JProperty("chargingModes",  new JArray(ChargingModes.SafeSelect(mode => mode.ToText())))
                                 : null,

                             CurrentType.HasValue  && CurrentType.Value  != WWCP.CurrentTypes.Unspecified
                                 ? new JProperty("currentTypes",   new JArray(CurrentType. Value.ToText()))
                                 : null,

                             AverageVoltage.HasValue && AverageVoltage > 0     ? new JProperty("averageVoltage",  Math.Round(AverageVoltage.Value, 2)) : null,
                             MaxCurrent.    HasValue && MaxCurrent     > 0     ? new JProperty("maxCurrent",      Math.Round(MaxCurrent.    Value, 2)) : null,
                             MaxPower.      HasValue && MaxPower.     HasValue ? new JProperty("maxPower",        Math.Round(MaxPower.      Value, 2)) : null,
                             MaxCapacity.   HasValue && MaxCapacity.  HasValue ? new JProperty("maxCapacity",     Math.Round(MaxCapacity.   Value, 2)) : null,

                             SocketOutlets.Count > 0
                                ? new JProperty("socketOutlets",  new JArray(SocketOutlets.ToJSON()))
                                : null,

                             EnergyMeterId.HasValue ? new JProperty("energyMeterId", EnergyMeterId) : null,

                             !Embedded ? ChargingStation.OpeningTimes.ToJSON("openingTimes") : null

                         );

                return CustomEVSESerializer != null
                           ? CustomEVSESerializer(this, JSON)
                           : JSON;

            }
            catch (Exception e)
            {
                return null;
            }

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
            if (ReferenceEquals(EVSE1, EVSE2))
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
        public override Int32 CompareTo(Object Object)
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
