﻿/*
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
using System.Collections;
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
    public static partial class ChargingStationOperatorExtentions
    {

        #region ToJSON(this ChargingStationOperators,           Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="Skip">The optional number of charging station operators to skip.</param>
        /// <param name="Take">The optional number of charging station operators to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        public static JArray ToJSON(this IEnumerable<ChargingStationOperator>                 ChargingStationOperators,
                                    UInt64?                                                   Skip                                      = null,
                                    UInt64?                                                   Take                                      = null,
                                    Boolean                                                   Embedded                                  = false,
                                    InfoStatus                                                ExpandRoamingNetworkId                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<ChargingStationOperator>  CustomChargingStationOperatorSerializer   = null,
                                    CustomJObjectSerializerDelegate<ChargingPool>             CustomChargingPoolSerializer              = null,
                                    CustomJObjectSerializerDelegate<ChargingStation>          CustomChargingStationSerializer           = null,
                                    CustomJObjectSerializerDelegate<EVSE>                     CustomEVSESerializer                      = null)


            => ChargingStationOperators?.SafeAny() == true

                   ? new JArray(ChargingStationOperators.
                                    Where         (cso => cso != null).
                                    OrderBy       (cso => cso.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (cso => cso.ToJSON(Embedded,
                                                                     ExpandRoamingNetworkId,
                                                                     ExpandChargingPoolIds,
                                                                     ExpandChargingStationIds,
                                                                     ExpandEVSEIds,
                                                                     ExpandBrandIds,
                                                                     ExpandDataLicenses,
                                                                     CustomChargingStationOperatorSerializer,
                                                                     CustomChargingPoolSerializer,
                                                                     CustomChargingStationSerializer,
                                                                     CustomEVSESerializer)).
                                    Where         (cso => cso != null))

                   : new JArray();

        #endregion


        #region ToJSON(this ChargingStationOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorAdminStatusTypes>>>>  ChargingStationOperatorAdminStatus,
                                     UInt64?                                                                                                                        Skip         = null,
                                     UInt64?                                                                                                                        Take         = null,
                                     UInt64?                                                                                                                        HistorySize  = 1)

        {

            #region Initial checks

            if (ChargingStationOperatorAdminStatus == null || !ChargingStationOperatorAdminStatus.Any())
                return new JObject();

            var _ChargingStationOperatorAdminStatus = new Dictionary<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorAdminStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate ChargingStationOperator identifications in the enumeration... take the newest one!

            foreach (var csostatus in Take.HasValue ? ChargingStationOperatorAdminStatus.Skip(Skip).Take(Take)
                                                    : ChargingStationOperatorAdminStatus.Skip(Skip))
            {

                if (!_ChargingStationOperatorAdminStatus.ContainsKey(csostatus.Key))
                    _ChargingStationOperatorAdminStatus.Add(csostatus.Key, csostatus.Value);

                else if (csostatus.Value.FirstOrDefault().Timestamp > _ChargingStationOperatorAdminStatus[csostatus.Key].FirstOrDefault().Timestamp)
                    _ChargingStationOperatorAdminStatus[csostatus.Key] = csostatus.Value;

            }

            #endregion

            return _ChargingStationOperatorAdminStatus.Count == 0

                   ? new JObject()

                   : new JObject(_ChargingStationOperatorAdminStatus.
                                     SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                  new JObject(statuslist.Value.

                                                                              // Will filter multiple cso status having the exact same ISO 8601 timestamp!
                                                                              GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                              Select           (group => group.First()).

                                                                              OrderByDescending(tsv   => tsv.Timestamp).
                                                                              Take             (HistorySize).
                                                                              Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                       tsv.Value.    ToString())))

                                                              )));

        }

        #endregion


        #region ToJSON(this ChargingStationOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorStatusTypes>>>>  ChargingStationOperatorStatus,
                                     UInt64?                                                                                                                   Skip         = null,
                                     UInt64?                                                                                                                   Take         = null,
                                     UInt64?                                                                                                                   HistorySize  = 1)

        {

            #region Initial checks

            if (ChargingStationOperatorStatus == null || !ChargingStationOperatorStatus.Any())
                return new JObject();

            var _ChargingStationOperatorStatus = new Dictionary<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate ChargingStationOperator identifications in the enumeration... take the newest one!

            foreach (var csostatus in Take.HasValue ? ChargingStationOperatorStatus.Skip(Skip).Take(Take)
                                                    : ChargingStationOperatorStatus.Skip(Skip))
            {

                if (!_ChargingStationOperatorStatus.ContainsKey(csostatus.Key))
                    _ChargingStationOperatorStatus.Add(csostatus.Key, csostatus.Value);

                else if (csostatus.Value.FirstOrDefault().Timestamp > _ChargingStationOperatorStatus[csostatus.Key].FirstOrDefault().Timestamp)
                    _ChargingStationOperatorStatus[csostatus.Key] = csostatus.Value;

            }

            #endregion

            return _ChargingStationOperatorStatus.Count == 0

                   ? new JObject()

                   : new JObject(_ChargingStationOperatorStatus.
                                     SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                  new JObject(statuslist.Value.

                                                                              // Will filter multiple cso status having the exact same ISO 8601 timestamp!
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


    public interface IChargingStationOperator : IEquatable<ChargingStationOperator>,
                                                IComparable<ChargingStationOperator>,
                                                IComparable,
                                                IEnumerable<ChargingPool>,
                                                IReserveRemoteStartStop,
                                                IStatus<ChargingStationOperatorStatusTypes>
    {

        IRoamingNetwork                 RoamingNetwork                   { get; }

        IRemoteChargingStationOperator  RemoteChargingStationOperator    { get; }

    }


    /// <summary>
    /// The Charging Station Operator (CSO) is responsible for operating charging pools,
    /// charging stations and EVSEs (power connectors), but is not neccessarily also the
    /// owner of all these devices.
    /// The Charging Station Operator delivers the locations, characteristics and real-time
    /// status information of its charging pools/-stations and EVSEs as Linked
    /// Open Data (LOD) to e-mobility service providers, navigation service
    /// providers and the public. For these delivered services (energy, parking, etc.) the
    /// operator will either be payed directly by the ev driver or by a contracted
    /// e-mobility service provider. The required pricing information can either be public
    /// information or part of B2B contracts.
    /// </summary>
    public class ChargingStationOperator : ACryptoEMobilityEntity<ChargingStationOperator_Id>,
                                           IChargingStationOperator
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext                   = "https://open.charging.cloud/contexts/wwcp+json/chargingStationOperator";

        /// <summary>
        /// The default max size of the admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 15;

        /// <summary>
        /// The default max size of the status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 15;

        #endregion

        #region Properties

        //  public IEnumerable<ChargingStationOperator_Id> OperatedIds { get; }

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of the EVSE Operator.
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
                    SetProperty(ref _Description, value);

            }

        }

        public I18NString SetDescription(Languages Language, String Text)
            => _Description = I18NString.Create(Language, Text);

        public I18NString SetDescription(I18NString I18NText)
            => _Description = I18NText;

        public I18NString AddDescription(Languages Language, String Text)
            => _Description.Add(Language, Text);

        #endregion

        #region Logo

        private String _Logo;

        /// <summary>
        /// The logo of this evse operator.
        /// </summary>
        [Optional]
        public String Logo
        {

            get
            {
                return _Logo;
            }

            set
            {
                if (_Logo != value)
                    SetProperty(ref _Logo, value);
            }

        }

        #endregion

        #region DataLicense

        private ReactiveSet<DataLicense> _DataLicenses;

        /// <summary>
        /// The license of the charging station operator data.
        /// </summary>
        [Mandatory]
        public ReactiveSet<DataLicense> DataLicenses
        {

            get
            {

                return _DataLicenses != null && _DataLicenses.Any()
                           ? _DataLicenses
                           : RoamingNetwork?.DataLicenses;

            }

            set
            {

                if (value != _DataLicenses && value != RoamingNetwork?.DataLicenses)
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
        /// The address of the operators headquarter.
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

                if (value == null)
                    _Address = value;

                if (_Address != value)
                    SetProperty(ref _Address, value);

            }

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate _GeoLocation;

        /// <summary>
        /// The geographical location of this operator.
        /// </summary>
        [Optional]
        public GeoCoordinate GeoLocation
        {

            get
            {
                return _GeoLocation;
            }

            set
            {

                if (value == null)
                    value = new GeoCoordinate(Latitude.Parse(0), Longitude.Parse(0));

                if (_GeoLocation != value)
                    SetProperty(ref _GeoLocation, value);

            }

        }

        #endregion

        #region Telephone

        private String _Telephone;

        /// <summary>
        /// The telephone number of the operator's (sales) office.
        /// </summary>
        [Optional]
        public String Telephone
        {

            get
            {
                return _Telephone;
            }

            set
            {
                if (_Telephone != value)
                    SetProperty(ref _Telephone, value);
            }

        }

        #endregion

        #region EMailAddress

        private String _EMailAddress;

        /// <summary>
        /// The e-mail address of the operator's (sales) office.
        /// </summary>
        [Optional]
        public String EMailAddress
        {

            get
            {
                return _EMailAddress;
            }

            set
            {
                if (_EMailAddress != value)
                    SetProperty(ref _EMailAddress, value);
            }

        }

        #endregion

        #region Homepage

        private String _Homepage;

        /// <summary>
        /// The homepage of this evse operator.
        /// </summary>
        [Optional]
        public String Homepage
        {

            get
            {
                return _Homepage;
            }

            set
            {
                if (_Homepage != value)
                    SetProperty(ref _Homepage, value);
            }

        }

        #endregion

        #region HotlinePhoneNumber

        private String _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the Charging Station Operator hotline.
        /// </summary>
        [Optional]
        public String HotlinePhoneNumber
        {

            get
            {
                return _HotlinePhoneNumber;
            }

            set
            {
                if (_HotlinePhoneNumber != value)
                    SetProperty(ref _HotlinePhoneNumber, value);
            }

        }

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current admin status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingStationOperatorAdminStatusTypes> AdminStatus

            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<ChargingStationOperatorAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingStationOperatorAdminStatusTypes>> AdminStatusSchedule(UInt64? HistorySize = null)
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
        public Timestamped<ChargingStationOperatorStatusTypes> Status

            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<ChargingStationOperatorStatusTypes> _StatusSchedule;

        /// <summary>
        /// The status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingStationOperatorStatusTypes>> StatusSchedule(UInt64? HistorySize = null)
        {

            if (HistorySize.HasValue)
                return _StatusSchedule.Take(HistorySize);

            return _StatusSchedule;

        }

        #endregion


        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject  CustomData    { get; }

        #endregion

        #region Links

        /// <summary>
        /// The remote charging station operator.
        /// </summary>
        [Optional]
        public IRemoteChargingStationOperator  RemoteChargingStationOperator    { get; }

        /// <summary>
        /// The roaming provider of this charging station operator.
        /// </summary>
        [Optional]
        public ICSORoamingProvider             EMPRoamingProvider               { get; }

        #endregion

        #region Events

        #region OnInvalidEVSEIdAdded

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public delegate void OnInvalidEVSEIdAddedDelegate(DateTime Timestamp, ChargingStationOperator ChargingStationOperator, EVSE_Id EVSEId);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        public event OnInvalidEVSEIdAddedDelegate OnInvalidEVSEIdAdded;

        #endregion

        #region OnInvalidEVSEIdRemoved

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public delegate void OnInvalidEVSEIdRemovedDelegate(DateTime Timestamp, ChargingStationOperator ChargingStationOperator, EVSE_Id EVSEId);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        public event OnInvalidEVSEIdRemovedDelegate OnInvalidEVSEIdRemoved;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator (CSO) having the given
        /// charging station operator identification (CSO Id).
        /// </summary>
        /// <param name="Id">The unique identification of the Charging Station Operator.</param>
        /// <param name="Name">The offical (multi-language) name of the EVSE Operator.</param>
        /// <param name="Description">An optional (multi-language) description of the EVSE Operator.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        public ChargingStationOperator(IEnumerable<ChargingStationOperator_Id>                Ids,
                                       IRoamingNetwork                                        RoamingNetwork,
                                       Action<ChargingStationOperator>                        Configurator                           = null,
                                       RemoteChargingStationOperatorCreatorDelegate           RemoteChargingStationOperatorCreator   = null,
                                       I18NString                                             Name                                   = null,
                                       I18NString                                             Description                            = null,
                                       Timestamped<ChargingStationOperatorAdminStatusTypes>?  InitialAdminStatus                     = null,
                                       Timestamped<ChargingStationOperatorStatusTypes>?       InitialStatus                          = null,
                                       UInt16                                                 MaxAdminStatusListSize                 = DefaultMaxAdminStatusListSize,
                                       UInt16                                                 MaxStatusListSize                      = DefaultMaxStatusListSize,

                                       JObject                                                CustomData                             = null,
                                       IReadOnlyDictionary<String, Object>                    InternalData                           = null)

            : base(Ids,
                   Name,
                   RoamingNetwork,
                   InternalData: InternalData)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The roaming network must not be null!");

            InitialAdminStatus = InitialAdminStatus ?? new Timestamped<ChargingStationOperatorAdminStatusTypes>(ChargingStationOperatorAdminStatusTypes.Operational);
            InitialStatus      = InitialStatus      ?? new Timestamped<ChargingStationOperatorStatusTypes>     (ChargingStationOperatorStatusTypes.Available);

            this.CustomData    = CustomData         ?? new JObject();

            #endregion

            #region Init data and properties

            this._Description                 = Description ?? new I18NString();
            this._DataLicenses                = new ReactiveSet<DataLicense>();

            #region InvalidEVSEIds

            this.InvalidEVSEIds               = new ReactiveSet<EVSE_Id>();

            InvalidEVSEIds.OnItemAdded += (Timestamp, Set, EVSEId) =>
                OnInvalidEVSEIdAdded?.Invoke(Timestamp, this, EVSEId);

            InvalidEVSEIds.OnItemRemoved += (Timestamp, Set, EVSEId) =>
                OnInvalidEVSEIdRemoved?.Invoke(Timestamp, this, EVSEId);

            #endregion

            this._Brands                      = new SpecialHashSet<ChargingStationOperator, Brand_Id,                Brand>               (this);

            this._ChargingPools               = new EntityHashSet <ChargingStationOperator, ChargingPool_Id,         ChargingPool>        (this);
            this._ChargingStationGroups       = new EntityHashSet <ChargingStationOperator, ChargingStationGroup_Id, ChargingStationGroup>(this);
            this._EVSEGroups                  = new EntityHashSet <ChargingStationOperator, EVSEGroup_Id,            EVSEGroup>           (this);

            this._ChargingTariffs             = new EntityHashSet <ChargingStationOperator, ChargingTariff_Id,       ChargingTariff>      (this);
            this._ChargingTariffGroups        = new EntityHashSet <ChargingStationOperator, ChargingTariffGroup_Id,  ChargingTariffGroup> (this);

            this._AdminStatusSchedule         = new StatusSchedule<ChargingStationOperatorAdminStatusTypes>(MaxAdminStatusListSize);
            this._AdminStatusSchedule.Insert(InitialAdminStatus.Value);

            this._StatusSchedule              = new StatusSchedule<ChargingStationOperatorStatusTypes>(MaxStatusListSize);
            this._StatusSchedule.Insert(InitialStatus.Value);

            //this._ChargingReservations        = new ConcurrentDictionary<ChargingReservation_Id, ChargingPool>();
            //this._ChargingSessions            = new ConcurrentDictionary<ChargingSession_Id,     ChargingPool>();

            #endregion

            #region Init events

            this.ChargingPoolAddition          = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingPool,         Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval           = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingPool,         Boolean>(() => new VetoVote(), true);
            //this.ChargingPoolGroupAddition     = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingPoolGroup,    Boolean>(() => new VetoVote(), true);
            //this.ChargingPoolGroupRemoval      = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingPoolGroup,    Boolean>(() => new VetoVote(), true);

            this.ChargingStationAddition       = new VotingNotificator<DateTime, ChargingPool,               ChargingStation,      Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval        = new VotingNotificator<DateTime, ChargingPool,               ChargingStation,      Boolean>(() => new VetoVote(), true);
            this.ChargingStationGroupAddition  = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingStationGroup, Boolean>(() => new VetoVote(), true);
            this.ChargingStationGroupRemoval   = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingStationGroup, Boolean>(() => new VetoVote(), true);

            this.EVSEAddition                  = new VotingNotificator<DateTime, ChargingStation,            EVSE,                 Boolean>(() => new VetoVote(), true);
            this.EVSERemoval                   = new VotingNotificator<DateTime, ChargingStation,            EVSE,                 Boolean>(() => new VetoVote(), true);
            this.EVSEGroupAddition             = new VotingNotificator<DateTime, ChargingStationOperator,    EVSEGroup,            Boolean>(() => new VetoVote(), true);
            this.EVSEGroupRemoval              = new VotingNotificator<DateTime, ChargingStationOperator,    EVSEGroup,            Boolean>(() => new VetoVote(), true);

            this.BrandAddition                 = new VotingNotificator<DateTime, ChargingStationOperator,    Brand,                Boolean>(() => new VetoVote(), true);
            this.BrandRemoval                  = new VotingNotificator<DateTime, ChargingStationOperator,    Brand,                Boolean>(() => new VetoVote(), true);

            this.ChargingTariffAddition        = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingTariff,       Boolean>(() => new VetoVote(), true);
            this.ChargingTariffRemoval         = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingTariff,       Boolean>(() => new VetoVote(), true);
            this.ChargingTariffGroupAddition   = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingTariffGroup,  Boolean>(() => new VetoVote(), true);
            this.ChargingTariffGroupRemoval    = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingTariffGroup,  Boolean>(() => new VetoVote(), true);

            #endregion

            #region Link events

            //this.OnPropertyChanged += UpdateData;

            //this._StatusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          //=> UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            //this._AdminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          //=> UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

            this.RemoteChargingStationOperator = RemoteChargingStationOperatorCreator?.Invoke(this);

            Configurator?.Invoke(this);

        }

        #endregion


        #region  Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnChargingStationOperatorDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnChargingStationOperatorAdminStatusChangedDelegate  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnChargingStationOperatorStatusChangedDelegate       OnStatusChanged;

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new admin status.</param>
        public void SetAdminStatus(ChargingStationOperatorAdminStatusTypes  NewAdminStatus)
        {

            _AdminStatusSchedule.Insert(NewAdminStatus);

        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<ChargingStationOperatorAdminStatusTypes> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(ChargingStationOperatorAdminStatusTypes  NewAdminStatus,
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
        public void SetAdminStatus(IEnumerable<Timestamped<ChargingStationOperatorAdminStatusTypes>>  NewAdminStatusList,
                                   ChangeMethods                                          ChangeMethod = ChangeMethods.Replace)
        {

            _AdminStatusSchedule.Set(NewAdminStatusList, ChangeMethod);

        }

        #endregion


        #region (internal) UpdateData(Timestamp, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="Sender">The changed Charging Station Operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateData(DateTime  Timestamp,
                                       Object    Sender,
                                       String    PropertyName,
                                       Object    OldValue,
                                       Object    NewValue)
        {

            var OnDataChangedLocal = OnDataChanged;
            if (OnDataChangedLocal != null)
                await OnDataChangedLocal(Timestamp, Sender as ChargingStationOperator, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                             Timestamp,
                                         Timestamped<ChargingStationOperatorStatusTypes>  OldStatus,
                                         Timestamped<ChargingStationOperatorStatusTypes>  NewStatus)
        {

            var OnStatusChangedLocal = OnStatusChanged;
            if (OnStatusChangedLocal != null)
                await OnStatusChangedLocal(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                                  Timestamp,
                                              Timestamped<ChargingStationOperatorAdminStatusTypes>  OldStatus,
                                              Timestamped<ChargingStationOperatorAdminStatusTypes>  NewStatus)
        {

            var OnAdminStatusChangedLocal = OnAdminStatusChanged;
            if (OnAdminStatusChangedLocal != null)
                await OnAdminStatusChangedLocal(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region AddDataLicense(params DataLicense)

        public ChargingStationOperator AddDataLicense(params DataLicense[] DataLicenses)
        {

            lock (_DataLicenses)
            {

                if (DataLicenses.Length > 0)
                {
                    foreach (var license in DataLicenses.Where(license => license != null))
                        _DataLicenses.Add(license);
                }

                return this;

            }

        }

        #endregion

        #region Brands

        #region BrandAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, Brand, Boolean> BrandAddition;

        /// <summary>
        /// Called whenever a brand will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, Brand, Boolean> OnBrandAddition

            => BrandAddition;

        #endregion

        #region CreateBrand     (Id, Name, Logo = null, Homepage = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new brand having the given
        /// unique brand identification.
        /// </summary>
        /// <param name="Id">The unique identification of this brand.</param>
        /// <param name="Name">The multi-language brand name.</param>
        /// <param name="Logo">An optional logo of this brand.</param>
        /// <param name="Homepage">An optional homepage of this brand.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new brand after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the brand failed.</param>
        public Brand CreateBrand(Brand_Id                                   Id,
                                 I18NString                                 Name,
                                 String                                     Logo        = null,
                                 String                                     Homepage    = null,

                                 Action<ChargingStationOperator, Brand>     OnSuccess   = null,
                                 Action<ChargingStationOperator, Brand_Id>  OnError     = null)

        {

            lock (_Brands)
            {

                #region Initial checks

                if (_Brands.ContainsId(Id))
                {

                    if (OnError != null)
                        OnError?.Invoke(this, Id);

                    else
                        throw new BrandAlreadyExists(this, Id);

                }

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the brand must not be null or empty!");

                #endregion

                var _Brand = new Brand(Id,
                                       Name,
                                       Logo,
                                       Homepage);


                if (BrandAddition.SendVoting(DateTime.UtcNow,
                                             this,
                                             _Brand) &&
                    _Brands.TryAdd(_Brand))
                {

                    OnSuccess?.Invoke(this, _Brand);

                    BrandAddition.SendNotification(DateTime.UtcNow,
                                                   this,
                                                   _Brand);

                    return _Brand;

                }

                return null;

            }

        }

        #endregion

        #region GetOrCreateBrand(Id, Name, Logo = null, Homepage = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new brand having the given
        /// unique brand identification.
        /// </summary>
        /// <param name="Id">The unique identification of this brand.</param>
        /// <param name="Name">The multi-language brand name.</param>
        /// <param name="Logo">An optional logo of this brand.</param>
        /// <param name="Homepage">An optional homepage of this brand.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new brand after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the brand failed.</param>
        public Brand GetOrCreateBrand(Brand_Id                                   Id,
                                      I18NString                                 Name,
                                      String                                     Logo        = null,
                                      String                                     Homepage    = null,

                                      Action<ChargingStationOperator, Brand>     OnSuccess   = null,
                                      Action<ChargingStationOperator, Brand_Id>  OnError     = null)

        {

            lock (_Brands)
            {

                #region Initial checks

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the brand must not be null or empty!");

                #endregion

                if (_Brands.TryGet(Id, out Brand _Brand))
                    return _Brand;

                return CreateBrand(Id,
                                   Name,
                                   Logo,
                                   Homepage,

                                   OnSuccess,
                                   OnError);


            }

        }

        #endregion


        #region Brands

        private readonly SpecialHashSet<ChargingStationOperator, Brand_Id, Brand> _Brands;

        /// <summary>
        /// All brands registered within this charging station operator.
        /// </summary>
        public IEnumerable<Brand> Brands
            => _Brands;

        #endregion

        #region BrandIds

        /// <summary>
        /// All brand identifications registered within this charging station operator.
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
        public Boolean TryGetBrand(Brand_Id   Id,
                                   out Brand  Brand)

            => _Brands.TryGet(Id, out Brand);

        #endregion


        #region BrandRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, Brand, Boolean> BrandRemoval;

        /// <summary>
        /// Called whenever a brand will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, Brand, Boolean> OnBrandRemoval

            => BrandRemoval;

        #endregion

        #region RemoveBrand(BrandId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All brands registered within this charging station operator.
        /// </summary>
        /// <param name="BrandId">The unique identification of the brand to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new brand after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the brand failed.</param>
        public Brand RemoveBrand(Brand_Id                                   BrandId,
                                 Action<ChargingStationOperator, Brand>     OnSuccess   = null,
                                 Action<ChargingStationOperator, Brand_Id>  OnError     = null)
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
        /// All brands registered within this charging station operator.
        /// </summary>
        /// <param name="Brand">The brand to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new brand after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the brand failed.</param>
        public Brand RemoveBrand(Brand                                   Brand,
                                 Action<ChargingStationOperator, Brand>  OnSuccess   = null,
                                 Action<ChargingStationOperator, Brand>  OnError     = null)
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

        #region Charging pools

        #region ChargingPoolAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingPool, Boolean> ChargingPoolAddition;

        /// <summary>
        /// Called whenever an charging pool will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingPool, Boolean> OnChargingPoolAddition

            => ChargingPoolAddition;

        #endregion

        #region CreateChargingPool        (ChargingPoolId, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging pool having the given
        /// unique charging pool identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the new charging pool.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging pool before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging pool after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging pool failed.</param>
        public ChargingPool CreateChargingPool(ChargingPool_Id?                                  ChargingPoolId              = null,
                                               Action<ChargingPool>                              Configurator                = null,
                                               RemoteChargingPoolCreatorDelegate                 RemoteChargingPoolCreator   = null,
                                               Timestamped<ChargingPoolAdminStatusTypes>?        InitialAdminStatus          = null,
                                               Timestamped<ChargingPoolStatusTypes>?             InitialStatus               = null,
                                               UInt16                                            MaxAdminStatusListSize      = ChargingPool.DefaultMaxAdminStatusListSize,
                                               UInt16                                            MaxStatusListSize           = ChargingPool.DefaultMaxStatusListSize,
                                               Action<ChargingPool>                              OnSuccess                   = null,
                                               Action<ChargingStationOperator, ChargingPool_Id>  OnError                     = null)

        {

            #region Initial checks

            if (!ChargingPoolId.HasValue)
                ChargingPoolId = ChargingPool_Id.Random(Id);

            // Do not throw an exception when an OnError delegate was given!
            if (_ChargingPools.ContainsId(ChargingPoolId.Value))
            {

                if (OnError == null)
                    throw new ChargingPoolAlreadyExists(this, ChargingPoolId.Value);

                    OnError?.Invoke(this, ChargingPoolId.Value);

            }

            if (!Ids.Contains(ChargingPoolId.Value.OperatorId))
                throw new InvalidChargingPoolOperatorId(this,
                                                        ChargingPoolId.Value.OperatorId,
                                                        Ids);

            #endregion

            var _ChargingPool = new ChargingPool(ChargingPoolId.Value,
                                                 this,
                                                 Configurator,
                                                 RemoteChargingPoolCreator,
                                                 InitialAdminStatus,
                                                 InitialStatus,
                                                 MaxAdminStatusListSize,
                                                 MaxStatusListSize);


            if (ChargingPoolAddition.SendVoting(DateTime.UtcNow, this, _ChargingPool) &&
                _ChargingPools.TryAdd(_ChargingPool))
            {

                _ChargingPool.OnDataChanged                             += UpdateChargingPoolData;
                _ChargingPool.OnAdminStatusChanged                      += UpdateChargingPoolAdminStatus;
                _ChargingPool.OnStatusChanged                           += UpdateChargingPoolStatus;

                _ChargingPool.OnChargingStationAddition.OnVoting        += (timestamp, evseoperator, pool, vote)    => ChargingStationAddition.SendVoting      (timestamp, evseoperator, pool, vote);
                _ChargingPool.OnChargingStationAddition.OnNotification  += (timestamp, evseoperator, pool)          => ChargingStationAddition.SendNotification(timestamp, evseoperator, pool);
                _ChargingPool.OnChargingStationDataChanged              += UpdateChargingStationData;
                _ChargingPool.OnChargingStationAdminStatusChanged       += UpdateChargingStationAdminStatus;
                _ChargingPool.OnChargingStationStatusChanged            += UpdateChargingStationStatus;
                _ChargingPool.OnChargingStationRemoval. OnVoting        += (timestamp, evseoperator, pool, vote)    => ChargingStationRemoval. SendVoting      (timestamp, evseoperator, pool, vote);
                _ChargingPool.OnChargingStationRemoval. OnNotification  += (timestamp, evseoperator, pool)          => ChargingStationRemoval. SendNotification(timestamp, evseoperator, pool);

                _ChargingPool.OnEVSEAddition.           OnVoting        += (timestamp, station, evse, vote)         => EVSEAddition.           SendVoting      (timestamp, station, evse, vote);
                _ChargingPool.OnEVSEAddition.           OnNotification  += (timestamp, station, evse)               => EVSEAddition.           SendNotification(timestamp, station, evse);
                _ChargingPool.OnEVSEDataChanged                         += UpdateEVSEData;
                _ChargingPool.OnEVSEAdminStatusChanged                  += UpdateEVSEAdminStatus;
                _ChargingPool.OnEVSEStatusChanged                       += UpdateEVSEStatus;
                _ChargingPool.OnEVSERemoval.            OnVoting        += (timestamp, station, evse, vote)         => EVSERemoval .           SendVoting      (timestamp, station, evse, vote);
                _ChargingPool.OnEVSERemoval.            OnNotification  += (timestamp, station, evse)               => EVSERemoval .           SendNotification(timestamp, station, evse);


                _ChargingPool.OnNewReservation                          += SendNewReservation;
                _ChargingPool.OnReservationCanceled                     += SendReservationCanceled;
                _ChargingPool.OnNewChargingSession                      += SendNewChargingSession;
                _ChargingPool.OnNewChargeDetailRecord                   += SendNewChargeDetailRecord;

                OnSuccess?.Invoke(_ChargingPool);
                ChargingPoolAddition.SendNotification(DateTime.UtcNow, this, _ChargingPool);

                return _ChargingPool;

            }

            return null;

        }

        #endregion

        #region CreateOrUpdateChargingPool(ChargingPoolId, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register or udpate a new charging pool having the given
        /// unique charging pool identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the new charging pool.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging pool before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging pool after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging pool failed.</param>
        public ChargingPool CreateOrUpdateChargingPool(ChargingPool_Id                                   ChargingPoolId,
                                                       Action<ChargingPool>                              Configurator                = null,
                                                       RemoteChargingPoolCreatorDelegate                 RemoteChargingPoolCreator   = null,
                                                       Timestamped<ChargingPoolAdminStatusTypes>?        InitialAdminStatus          = null,
                                                       Timestamped<ChargingPoolStatusTypes>?             InitialStatus               = null,
                                                       UInt16                                            MaxAdminStatusListSize      = ChargingPool.DefaultMaxAdminStatusListSize,
                                                       UInt16                                            MaxStatusListSize           = ChargingPool.DefaultMaxStatusListSize,
                                                       Action<ChargingPool>                              OnSuccess                   = null,
                                                       Action<ChargingStationOperator, ChargingPool_Id>  OnError                     = null)

        {

            lock (_ChargingPools)
            {

                #region Initial checks

                if (!Ids.Contains(ChargingPoolId.OperatorId))
                    throw new InvalidChargingPoolOperatorId(this,
                                                            ChargingPoolId.OperatorId,
                                                            Ids);

                #endregion

                #region If the charging pool identification is new/unknown: Call CreateChargingPool(...)

                if (!_ChargingPools.ContainsId(ChargingPoolId))
                    return CreateChargingPool(ChargingPoolId,
                                              Configurator,
                                              RemoteChargingPoolCreator,
                                              InitialAdminStatus,
                                              InitialStatus,
                                              MaxAdminStatusListSize,
                                              MaxStatusListSize,
                                              OnSuccess,
                                              OnError);

                #endregion


                // Merge existing charging pool with new pool data...

                try
                {

                    return _ChargingPools.
                               GetById(ChargingPoolId).
                               UpdateWith(new ChargingPool(ChargingPoolId,
                                                           this,
                                                           Configurator,
                                                           null,
                                                           new Timestamped<ChargingPoolAdminStatusTypes>(DateTime.MinValue, ChargingPoolAdminStatusTypes.Operational),
                                                           new Timestamped<ChargingPoolStatusTypes>(DateTime.MinValue, ChargingPoolStatusTypes.Available)));

                } catch (Exception e)
                {
                    DebugX.Log("CSO.CreateOrUpdateChargingPool(...) failed: " + e.Message);
                }

                return null;

            }

        }

        #endregion


        #region ChargingPools

        private EntityHashSet<ChargingStationOperator, ChargingPool_Id, ChargingPool> _ChargingPools;

        public IEnumerable<ChargingPool> ChargingPools

            => _ChargingPools;

        #endregion

        #region ChargingPoolIds        (IncludePools = null)

        /// <summary>
        /// Return an enumeration of all charging pool identifications.
        /// </summary>
        /// <param name="IncludePools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPool_Id> ChargingPoolIds(IncludeChargingPoolDelegate IncludePools = null)

            => IncludePools == null

                   ? _ChargingPools.
                         Select    (pool => pool.Id)

                   : _ChargingPools.
                         Where     (pool => IncludePools(pool)).
                         Select    (pool => pool.Id);

        #endregion

        #region ChargingPoolAdminStatus(IncludePools = null)

        /// <summary>
        /// Return an enumeration of all charging pool admin status.
        /// </summary>
        /// <param name="IncludePools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPoolAdminStatus> ChargingPoolAdminStatus(IncludeChargingPoolDelegate IncludePools = null)

            => IncludePools == null

                   ? _ChargingPools.
                         Select(pool => new ChargingPoolAdminStatus(pool.Id, pool.AdminStatus))

                   : _ChargingPools.
                         Where (pool => IncludePools(pool)).
                         Select(pool => new ChargingPoolAdminStatus(pool.Id, pool.AdminStatus));

        #endregion

        #region ChargingPoolStatus     (IncludePools = null)

        /// <summary>
        /// Return an enumeration of all charging pool status.
        /// </summary>
        /// <param name="IncludePools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPoolStatus> ChargingPoolStatus(IncludeChargingPoolDelegate IncludePools = null)

            => IncludePools == null

                   ? _ChargingPools.
                         Select(pool => new ChargingPoolStatus(pool.Id, pool.Status))

                   : _ChargingPools.
                         Where (pool => IncludePools(pool)).
                         Select(pool => new ChargingPoolStatus(pool.Id, pool.Status));

        #endregion


        #region ContainsChargingPool(ChargingPool)

        /// <summary>
        /// Check if the given ChargingPool is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public Boolean ContainsChargingPool(ChargingPool ChargingPool)

            => _ChargingPools.Contains(ChargingPool);

        #endregion

        #region ContainsChargingPool(ChargingPoolId)

        /// <summary>
        /// Check if the given ChargingPool identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the charging pool.</param>
        public Boolean ContainsChargingPool(ChargingPool_Id ChargingPoolId)

            => _ChargingPools.ContainsId(ChargingPoolId);

        #endregion

        #region GetChargingPoolById(ChargingPoolId)

        public ChargingPool GetChargingPoolById(ChargingPool_Id ChargingPoolId)

            => _ChargingPools.GetById(ChargingPoolId);

        #endregion

        #region TryGetChargingPoolById(ChargingPoolId, out ChargingPool)

        public Boolean TryGetChargingPoolById(ChargingPool_Id ChargingPoolId, out ChargingPool ChargingPool)

            => _ChargingPools.TryGet(ChargingPoolId, out ChargingPool);

        #endregion

        #region RemoveChargingPool(ChargingPoolId)

        public ChargingPool RemoveChargingPool(ChargingPool_Id ChargingPoolId)
        {

            ChargingPool _ChargingPool = null;

            if (TryGetChargingPoolById(ChargingPoolId, out _ChargingPool))
            {

                if (ChargingPoolRemoval.SendVoting(DateTime.UtcNow, this, _ChargingPool))
                {

                    if (_ChargingPools.TryRemove(ChargingPoolId, out _ChargingPool))
                    {

                        ChargingPoolRemoval.SendNotification(DateTime.UtcNow, this, _ChargingPool);

                        return _ChargingPool;

                    }

                }

            }

            return null;

        }

        #endregion

        #region TryRemoveChargingPool(ChargingPoolId, out ChargingPool)

        public Boolean TryRemoveChargingPool(ChargingPool_Id ChargingPoolId, out ChargingPool ChargingPool)
        {

            if (TryGetChargingPoolById(ChargingPoolId, out ChargingPool))
            {

                if (ChargingPoolRemoval.SendVoting(DateTime.UtcNow, this, ChargingPool))
                {

                    if (_ChargingPools.TryRemove(ChargingPoolId, out ChargingPool))
                    {

                        ChargingPoolRemoval.SendNotification(DateTime.UtcNow, this, ChargingPool);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, NewStatus)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                           ChargingPoolId,
                                               Timestamped<ChargingPoolAdminStatusTypes>  NewStatus,
                                               Boolean                                   SendUpstream = false)
        {

            ChargingPool _ChargingPool = null;
            if (TryGetChargingPoolById(ChargingPoolId, out _ChargingPool))
                _ChargingPool.SetAdminStatus(NewStatus);

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, NewStatus, Timestamp)

        public void SetChargingPoolAdminStatus(ChargingPool_Id              ChargingPoolId,
                                               ChargingPoolAdminStatusTypes  NewStatus,
                                               DateTime                     Timestamp)
        {

            ChargingPool _ChargingPool  = null;
            if (TryGetChargingPoolById(ChargingPoolId, out _ChargingPool))
                _ChargingPool.SetAdminStatus(NewStatus, Timestamp);

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                                        ChargingPoolId,
                                               IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>  StatusList,
                                               ChangeMethods                                          ChangeMethod  = ChangeMethods.Replace)
        {

            ChargingPool _ChargingPool  = null;
            if (TryGetChargingPoolById(ChargingPoolId, out _ChargingPool))
                _ChargingPool.SetAdminStatus(StatusList, ChangeMethod);

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendChargingPoolAdminStatusDiff(new ChargingPoolAdminStatusDiff(DateTime.UtcNow,
            //                                               ChargingStationOperatorId:    Id,
            //                                               ChargingStationOperatorName:  Name,
            //                                               NewStatus:         new List<KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>>(),
            //                                               ChangedStatus:     new List<KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>>() {
            //                                                                          new KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>(ChargingPoolId, NewStatus.Value)
            //                                                                      },
            //                                               RemovedIds:        new List<ChargingPool_Id>()));
            //
            //}

        }

        #endregion


        #region OnChargingPoolData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolDataChangedDelegate         OnChargingPoolDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolStatusChangedDelegate       OnChargingPoolStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate  OnChargingPoolAdminStatusChanged;

        #endregion


        #region (internal) UpdateChargingPoolData       (Timestamp, EventTrackingId, ChargingPool, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the data of an charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateChargingPoolData(DateTime          Timestamp,
                                                   EventTracking_Id  EventTrackingId,
                                                   ChargingPool      ChargingPool,
                                                   String            PropertyName,
                                                   Object            OldValue,
                                                   Object            NewValue)
        {

            var OnChargingPoolDataChangedLocal = OnChargingPoolDataChanged;
            if (OnChargingPoolDataChangedLocal != null)
                await OnChargingPoolDataChangedLocal(Timestamp,
                                                     EventTrackingId,
                                                     ChargingPool,
                                                     PropertyName,
                                                     OldValue,
                                                     NewValue);

        }

        #endregion

        #region (internal) UpdateChargingPoolAdminStatus(Timestamp, EventTrackingId, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
        internal async Task UpdateChargingPoolAdminStatus(DateTime                                  Timestamp,
                                                          EventTracking_Id                          EventTrackingId,
                                                          ChargingPool                              ChargingPool,
                                                          Timestamped<ChargingPoolAdminStatusTypes>  OldStatus,
                                                          Timestamped<ChargingPoolAdminStatusTypes>  NewStatus)
        {

            var OnChargingPoolAdminStatusChangedLocal = OnChargingPoolAdminStatusChanged;
            if (OnChargingPoolAdminStatusChangedLocal != null)
                await OnChargingPoolAdminStatusChangedLocal(Timestamp,
                                                            EventTrackingId,
                                                            ChargingPool,
                                                            OldStatus,
                                                            NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingPoolStatus     (Timestamp, EventTrackingId, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
        internal async Task UpdateChargingPoolStatus(DateTime                             Timestamp,
                                                     EventTracking_Id                     EventTrackingId,
                                                     ChargingPool                         ChargingPool,
                                                     Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                                     Timestamped<ChargingPoolStatusTypes>  NewStatus)
        {

            var OnChargingPoolStatusChangedLocal = OnChargingPoolStatusChanged;
            if (OnChargingPoolStatusChangedLocal != null)
                await OnChargingPoolStatusChangedLocal(Timestamp,
                                                       EventTrackingId,
                                                       ChargingPool,
                                                       OldStatus,
                                                       NewStatus);

        }

        #endregion


        #region IEnumerable<ChargingPool> Members

        IEnumerator IEnumerable.GetEnumerator()

            => _ChargingPools.GetEnumerator();

        public IEnumerator<ChargingPool> GetEnumerator()

            => _ChargingPools.GetEnumerator();

        #endregion


        #region ChargingPoolRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingPool, Boolean> ChargingPoolRemoval;

        /// <summary>
        /// Called whenever an charging pool will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingPool, Boolean> OnChargingPoolRemoval

            => ChargingPoolRemoval;

        #endregion

        #endregion

        #region Charging stations

        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationAddition

            => ChargingStationAddition;

        #endregion


        #region ChargingStations

        /// <summary>
        /// Return an enumeration of all charging stations.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations

            => _ChargingPools.
                   SelectMany(pool => pool.ChargingStations);

        #endregion

        #region ChargingStationIds        (IncludeStations = null)

        /// <summary>
        /// Return an enumeration of all charging station identifications.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate IncludeStations = null)

            => IncludeStations == null

                   ? _ChargingPools.
                         SelectMany(pool    => pool.ChargingStations).
                         Select    (station => station.Id)

                   : _ChargingPools.
                         SelectMany(pool    => pool.ChargingStations).
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

                   ? _ChargingPools.
                         SelectMany(pool    => pool.ChargingStations).
                         Select    (station => new ChargingStationAdminStatus(station.Id, station.AdminStatus))

                   : _ChargingPools.
                         SelectMany(pool    => pool.ChargingStations).
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

                   ? _ChargingPools.
                         SelectMany(pool    => pool.ChargingStations).
                         Select    (station => new ChargingStationStatus(station.Id, station.Status))

                   : _ChargingPools.
                         SelectMany(pool    => pool.ChargingStations).
                         Where     (station => IncludeStations(station)).
                         Select    (station => new ChargingStationStatus(station.Id, station.Status));

        #endregion


        #region ContainsChargingStation(ChargingStation)

        /// <summary>
        /// Check if the given ChargingStation is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation ChargingStation)

            => _ChargingPools.Any(pool => pool.ContainsChargingStation(ChargingStation.Id));

        #endregion

        #region ContainsChargingStation(ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)

            => _ChargingPools.Any(pool => pool.ContainsChargingStation(ChargingStationId));

        #endregion

        #region GetChargingStationById(ChargingStationId)

        public ChargingStation GetChargingStationById(ChargingStation_Id ChargingStationId)

            => _ChargingPools.
                   SelectMany    (pool    => pool.ChargingStations).
                   FirstOrDefault(station => station.Id == ChargingStationId);

        #endregion

        #region TryGetChargingStationById(ChargingStationId, out ChargingStation ChargingStation)

        public Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {

            ChargingStation = _ChargingPools.
                                  SelectMany    (pool    => pool.ChargingStations).
                                  FirstOrDefault(station => station.Id == ChargingStationId);

            return ChargingStation != null;

        }

        #endregion

        #region TryGetChargingPoolByStationId(ChargingStationId, out Pool)

        public Boolean TryGetChargingPoolByStationId(ChargingStation_Id ChargingStationId, out ChargingPool Pool)
        {

            foreach (var pool in _ChargingPools)
            {
                if (pool.TryGetChargingStationById(ChargingStationId, out ChargingStation station))
                {
                    Pool = pool;
                    return true;
                }
            }

            Pool = null;
            return false;

        }

        #endregion


        #region SetChargingStationStatus(ChargingStationId, NewStatus)

        public void SetChargingStationStatus(ChargingStation_Id         ChargingStationId,
                                             ChargingStationStatusTypes  NewStatus)
        {

            ChargingStation _ChargingStation  = null;
            if (TryGetChargingStationById(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetStatus(NewStatus);

        }

        #endregion

        #region SetChargingStationStatus(ChargingStationId, NewTimestampedStatus)

        public void SetChargingStationStatus(ChargingStation_Id                      ChargingStationId,
                                             Timestamped<ChargingStationStatusTypes>  NewTimestampedStatus)
        {

            ChargingStation _ChargingStation = null;
            if (TryGetChargingStationById(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetStatus(NewTimestampedStatus);

        }

        #endregion


        #region SetChargingStationAdminStatus(ChargingStationId, NewStatus)

        public void SetChargingStationAdminStatus(ChargingStation_Id              ChargingStationId,
                                                  ChargingStationAdminStatusTypes  NewStatus)
        {

            ChargingStation _ChargingStation  = null;
            if (TryGetChargingStationById(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetAdminStatus(NewStatus);

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, NewTimestampedStatus)

        public void SetChargingStationAdminStatus(ChargingStation_Id                           ChargingStationId,
                                                  Timestamped<ChargingStationAdminStatusTypes>  NewTimestampedStatus)
        {

            ChargingStation _ChargingStation = null;
            if (TryGetChargingStationById(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetAdminStatus(NewTimestampedStatus);

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, NewStatus, Timestamp)

        public void SetChargingStationAdminStatus(ChargingStation_Id              ChargingStationId,
                                                  ChargingStationAdminStatusTypes  NewStatus,
                                                  DateTime                        Timestamp)
        {

            ChargingStation _ChargingStation  = null;
            if (TryGetChargingStationById(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetAdminStatus(NewStatus, Timestamp);

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetChargingStationAdminStatus(ChargingStation_Id                                        ChargingStationId,
                                                  IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>  StatusList,
                                                  ChangeMethods                                             ChangeMethod  = ChangeMethods.Replace)
        {

            ChargingStation _ChargingStation  = null;
            if (TryGetChargingStationById(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetAdminStatus(StatusList, ChangeMethod);

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendChargingStationAdminStatusDiff(new ChargingStationAdminStatusDiff(DateTime.UtcNow,
            //                                               ChargingStationOperatorId:    Id,
            //                                               ChargingStationOperatorName:  Name,
            //                                               NewStatus:         new List<KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>>(),
            //                                               ChangedStatus:     new List<KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>>() {
            //                                                                          new KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>(ChargingStationId, NewStatus.Value)
            //                                                                      },
            //                                               RemovedIds:        new List<ChargingStation_Id>()));
            //
            //}

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


        #region (internal) UpdateChargingStationData       (Timestamp, EventTrackingId, ChargingStation, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the data of a charging station.
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

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
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

        #region (internal) UpdateChargingStationStatus     (Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
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

        }

        #endregion


        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationRemoval

            => ChargingStationRemoval;

        #endregion

        #endregion

        #region Charging station groups

        #region ChargingStationGroupAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> ChargingStationGroupAddition;

        /// <summary>
        /// Called whenever a charging station group will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> OnChargingStationGroupAddition

            => ChargingStationGroupAddition;

        #endregion


        #region ChargingStationGroups

        private readonly EntityHashSet<ChargingStationOperator, ChargingStationGroup_Id, ChargingStationGroup> _ChargingStationGroups;

        /// <summary>
        /// All charging station groups registered within this charging station operator.
        /// </summary>
        public IEnumerable<ChargingStationGroup> ChargingStationGroups

            => _ChargingStationGroups;

        #endregion


        #region CreateChargingStationGroup     (Id,       Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Name">The offical (multi-language) name of this charging station group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging station group.</param>
        /// 
        /// <param name="Members">An enumeration of charging stations member building this charging station group.</param>
        /// <param name="MemberIds">An enumeration of charging station identifications which are building this charging station group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new charging stations automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated charging stations.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the charging station group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the charging station group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        public ChargingStationGroup CreateChargingStationGroup(ChargingStationGroup_Id                                             Id,
                                                               I18NString                                                          Name,
                                                               I18NString                                                          Description                   = null,

                                                               Brand                                                               Brand                         = null,
                                                               Priority?                                                           Priority                      = null,
                                                               ChargingTariff                                                      Tariff                        = null,
                                                               IEnumerable<DataLicense>                                            DataLicenses                  = null,

                                                               IEnumerable<ChargingStation>                                        Members                       = null,
                                                               IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                               Func<ChargingStation, Boolean>                                      AutoIncludeStations           = null,

                                                               Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                               UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                               UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                               Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                               Action<ChargingStationOperator, ChargingStationGroup_Id>            OnError                       = null)

        {

            lock (_ChargingStationGroups)
            {

                #region Initial checks

                if (_ChargingStationGroups.ContainsId(Id))
                {

                    if (OnError != null)
                        OnError?.Invoke(this, Id);

                    throw new ChargingStationGroupAlreadyExists(this, Id);

                }

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the charging station group must not be null or empty!");

                #endregion

                var _ChargingStationGroup = new ChargingStationGroup(Id,
                                                                     this,
                                                                     Name,
                                                                     Description,

                                                                     Brand,
                                                                     Priority,
                                                                     Tariff,
                                                                     DataLicenses,

                                                                     Members,
                                                                     MemberIds,
                                                                     AutoIncludeStations,
                                                                     StatusAggregationDelegate,
                                                                     MaxGroupAdminStatusListSize,
                                                                     MaxGroupStatusListSize);


                if (ChargingStationGroupAddition.SendVoting(DateTime.UtcNow, this, _ChargingStationGroup) &&
                    _ChargingStationGroups.TryAdd(_ChargingStationGroup))
                {

                    _ChargingStationGroup.OnEVSEDataChanged                             += UpdateEVSEData;
                    _ChargingStationGroup.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                    _ChargingStationGroup.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;

                    _ChargingStationGroup.OnChargingStationDataChanged                  += UpdateChargingStationData;
                    _ChargingStationGroup.OnChargingStationStatusChanged                += UpdateChargingStationStatus;
                    _ChargingStationGroup.OnChargingStationAdminStatusChanged           += UpdateChargingStationAdminStatus;

                    //_ChargingStationGroup.OnDataChanged                                 += UpdateChargingStationGroupData;
                    //_ChargingStationGroup.OnAdminStatusChanged                          += UpdateChargingStationGroupAdminStatus;

                    OnSuccess?.Invoke(_ChargingStationGroup);

                    ChargingStationGroupAddition.SendNotification(DateTime.UtcNow,
                                                                  this,
                                                                  _ChargingStationGroup);

                    return _ChargingStationGroup;

                }

                return null;

            }

        }

        #endregion

        #region CreateChargingStationGroup     (IdSuffix, Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging group.</param>
        /// <param name="Name">The offical (multi-language) name of this charging station group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging station group.</param>
        /// 
        /// <param name="Members">An enumeration of charging stations member building this charging station group.</param>
        /// <param name="MemberIds">An enumeration of charging station identifications which are building this charging station group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new charging stations automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated charging stations.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the charging station group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the charging station group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        public ChargingStationGroup CreateChargingStationGroup(String                                                              IdSuffix,
                                                               I18NString                                                          Name,
                                                               I18NString                                                          Description                   = null,

                                                               IEnumerable<ChargingStation>                                        Members                       = null,
                                                               IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                               Func<ChargingStation, Boolean>                                      AutoIncludeStations           = null,

                                                               Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                               UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                               UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                               Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                               Action<ChargingStationOperator, ChargingStationGroup_Id>            OnError                       = null)

        {

            #region Initial checks

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix), "The given suffix of the unique identification of the new charging group must not be null or empty!");

            #endregion

            return CreateChargingStationGroup(ChargingStationGroup_Id.Parse(Id,
                                                                            IdSuffix.Trim().ToUpper()),
                                              Name,
                                              Description,

                                              null,
                                              new Priority(0),
                                              null,
                                              null,

                                              Members,
                                              MemberIds,
                                              AutoIncludeStations,
                                              StatusAggregationDelegate,
                                              MaxGroupAdminStatusListSize,
                                              MaxGroupStatusListSize,
                                              OnSuccess,
                                              OnError);

        }

        #endregion

        #region GetOrCreateChargingStationGroup(Id,       Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Name">The offical (multi-language) name of this charging station group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging station group.</param>
        /// 
        /// <param name="Members">An enumeration of charging stations member building this charging station group.</param>
        /// <param name="MemberIds">An enumeration of charging station identifications which are building this charging station group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new charging stations automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated charging stations.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the charging station group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the charging station group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        public ChargingStationGroup GetOrCreateChargingStationGroup(ChargingStationGroup_Id                                             Id,
                                                                    I18NString                                                          Name,
                                                                    I18NString                                                          Description                   = null,

                                                                    IEnumerable<ChargingStation>                                        Members                       = null,
                                                                    IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                                    Func<ChargingStation, Boolean>                                      AutoIncludeStations           = null,

                                                                    Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                                    UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                                    UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                                    Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                                    Action<ChargingStationOperator, ChargingStationGroup_Id>            OnError                       = null)

        {

            lock (_ChargingStationGroups)
            {

                #region Initial checks

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the charging station group must not be null or empty!");

                #endregion

                if (_ChargingStationGroups.TryGet(Id, out ChargingStationGroup _ChargingStationGroup))
                    return _ChargingStationGroup;

                return CreateChargingStationGroup(Id,
                                                  Name,
                                                  Description,

                                                  null,
                                                  new Priority(0),
                                                  null,
                                                  null,

                                                  Members,
                                                  MemberIds,
                                                  AutoIncludeStations,
                                                  StatusAggregationDelegate,
                                                  MaxGroupAdminStatusListSize,
                                                  MaxGroupStatusListSize,
                                                  OnSuccess,
                                                  OnError);

            }

        }

        #endregion

        #region GetOrCreateChargingStationGroup(IdSuffix, Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging group.</param>
        /// <param name="Name">The offical (multi-language) name of this charging station group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging station group.</param>
        /// 
        /// <param name="Members">An enumeration of charging stations member building this charging station group.</param>
        /// <param name="MemberIds">An enumeration of charging station identifications which are building this charging station group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new charging stations automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated charging stations.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the charging station group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the charging station group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        public ChargingStationGroup GetOrCreateChargingStationGroup(String                                                             IdSuffix,
                                                                    I18NString                                                         Name,
                                                                    I18NString                                                         Description                   = null,

                                                                    IEnumerable<ChargingStation>                                       Members                       = null,
                                                                    IEnumerable<ChargingStation_Id>                                    MemberIds                     = null,
                                                                    Func<ChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                                                    Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                                    UInt16                                                             MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                                    UInt16                                                             MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                                    Action<ChargingStationGroup>                                       OnSuccess                     = null,
                                                                    Action<ChargingStationOperator, ChargingStationGroup_Id>           OnError                       = null)


        {

            #region Initial checks

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix), "The given suffix of the unique identification of the new charging group must not be null or empty!");

            #endregion

            return GetOrCreateChargingStationGroup(ChargingStationGroup_Id.Parse(Id,
                                                                                 IdSuffix.Trim().ToUpper()),
                                                   Name,
                                                   Description,
                                                   Members,
                                                   MemberIds,
                                                   AutoIncludeStations,
                                                   StatusAggregationDelegate,
                                                   MaxGroupAdminStatusListSize,
                                                   MaxGroupStatusListSize,
                                                   OnSuccess,
                                                   OnError);

        }

        #endregion


        #region TryGetChargingStationGroup(Id, out ChargingStationGroup)

        /// <summary>
        /// Try to return to charging station group for the given charging station group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="ChargingStationGroup">The charing station group.</param>
        public Boolean TryGetChargingStationGroup(ChargingStationGroup_Id   Id,
                                                  out ChargingStationGroup  ChargingStationGroup)

            => _ChargingStationGroups.TryGet(Id, out ChargingStationGroup);

        #endregion


        #region ChargingStationGroupRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> ChargingStationGroupRemoval;

        /// <summary>
        /// Called whenever a charging station group will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> OnChargingStationGroupRemoval

            => ChargingStationGroupRemoval;

        #endregion

        #region RemoveChargingStationGroup(ChargingStationGroupId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All charging station groups registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingStationGroupId">The unique identification of the charging station group to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station group after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging station group failed.</param>
        public ChargingStationGroup RemoveChargingStationGroup(ChargingStationGroup_Id                                   ChargingStationGroupId,
                                                               Action<ChargingStationOperator, ChargingStationGroup>     OnSuccess   = null,
                                                               Action<ChargingStationOperator, ChargingStationGroup_Id>  OnError     = null)
        {

            lock (_ChargingStationGroups)
            {

                if (_ChargingStationGroups.TryGet(ChargingStationGroupId, out ChargingStationGroup ChargingStationGroup) &&
                    ChargingStationGroupRemoval.SendVoting(DateTime.UtcNow,
                                                           this,
                                                           ChargingStationGroup) &&
                    _ChargingStationGroups.TryRemove(ChargingStationGroupId, out ChargingStationGroup _ChargingStationGroup))
                {

                    OnSuccess?.Invoke(this, ChargingStationGroup);

                    ChargingStationGroupRemoval.SendNotification(DateTime.UtcNow,
                                                                 this,
                                                                 _ChargingStationGroup);

                    return _ChargingStationGroup;

                }

                OnError?.Invoke(this, ChargingStationGroupId);

                return null;

            }

        }

        #endregion

        #region RemoveChargingStationGroup(ChargingStationGroup,   OnSuccess = null, OnError = null)

        /// <summary>
        /// All charging station groups registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingStationGroup">The charging station group to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station group after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging station group failed.</param>
        public ChargingStationGroup RemoveChargingStationGroup(ChargingStationGroup                                   ChargingStationGroup,
                                                               Action<ChargingStationOperator, ChargingStationGroup>  OnSuccess   = null,
                                                               Action<ChargingStationOperator, ChargingStationGroup>  OnError     = null)
        {

            lock (_ChargingStationGroups)
            {

                if (ChargingStationGroupRemoval.SendVoting(DateTime.UtcNow,
                                                           this,
                                                           ChargingStationGroup) &&
                    _ChargingStationGroups.TryRemove(ChargingStationGroup.Id, out ChargingStationGroup _ChargingStationGroup))
                {

                    OnSuccess?.Invoke(this, _ChargingStationGroup);

                    ChargingStationGroupRemoval.SendNotification(DateTime.UtcNow,
                                                                 this,
                                                                 _ChargingStationGroup);

                    return _ChargingStationGroup;

                }

                OnError?.Invoke(this, ChargingStationGroup);

                return ChargingStationGroup;

            }

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

        /// <summary>
        /// All EVSEs.
        /// </summary>
        public IEnumerable<EVSE> EVSEs

            => _ChargingPools.
                   SelectMany(v => v.ChargingStations).
                   SelectMany(v => v.EVSEs);

        #endregion

        #region EVSEIds                (IncludeEVSEs = null)

        /// <summary>
        /// Return an enumeration of all EVSE identifications.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate IncludeEVSEs = null)

            => IncludeEVSEs == null

                   ? _ChargingPools.
                         SelectMany(pool => pool.EVSEs).
                         Select    (evse => evse.Id)

                   : _ChargingPools.
                         SelectMany(pool => pool.EVSEs).
                         Where     (evse => IncludeEVSEs(evse)).
                         Select    (evse => evse.Id);

        #endregion

        #region EVSEAdminStatus        (IncludeEVSEs = null)

        /// <summary>
        /// Return an enumeration of all EVSE admin status.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate IncludeEVSEs = null)

            => IncludeEVSEs == null

                   ? _ChargingPools.
                         SelectMany(pool => pool.EVSEs).
                         Select    (evse => new EVSEAdminStatus(evse.Id,
                                                                evse.AdminStatus))

                   : _ChargingPools.
                         SelectMany(pool    => pool.EVSEs).
                         Where     (evse => IncludeEVSEs(evse)).
                         Select    (evse => new EVSEAdminStatus(evse.Id,
                                                                evse.AdminStatus));

        #endregion

        #region EVSEAdminStatusSchedule(IncludeEVSEs = null, TimestampFilter  = null, StatusFilter = null, HistorySize = null)

        /// <summary>
        /// Return an enumeration of all EVSE admin status.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        /// <param name="TimestampFilter">An optional admin status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<EVSEAdminStatusSchedule> EVSEAdminStatusSchedule(IncludeEVSEDelegate                  IncludeEVSEs      = null,
                                                                            Func<DateTime,             Boolean>  TimestampFilter   = null,
                                                                            Func<EVSEAdminStatusTypes, Boolean>  StatusFilter      = null,
                                                                            UInt64?                              HistorySize       = null)
{

            if (IncludeEVSEs == null)
                IncludeEVSEs = evse => true;

            return _ChargingPools.
                         SelectMany(pool           => pool.EVSEs).
                         Where     (evse           => IncludeEVSEs(evse)).
                         Select    (evse           => new EVSEAdminStatusSchedule(evse.Id,
                                                                                  evse.AdminStatusSchedule(TimestampFilter,
                                                                                                           StatusFilter,
                                                                                                           HistorySize))).
                         Where     (statusSchedule => statusSchedule.StatusSchedule.Count() > 0);

        }

        #endregion

        #region EVSEStatus             (IncludeEVSEs = null)

        /// <summary>
        /// Return an enumeration of all EVSE status.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate IncludeEVSEs = null)

            => IncludeEVSEs == null

                   ? _ChargingPools.
                         SelectMany(pool => pool.EVSEs).
                         Select    (evse => new EVSEStatus(evse.Id,
                                                           evse.Status))

                   : _ChargingPools.
                         SelectMany(pool => pool.EVSEs).
                         Where     (evse => IncludeEVSEs(evse)).
                         Select    (evse => new EVSEStatus(evse.Id,
                                                           evse.Status));

        #endregion

        #region EVSEStatusSchedule     (IncludeEVSEs = null, TimestampFilter  = null, StatusFilter = null, HistorySize = null)

        /// <summary>
        /// Return an enumeration of all EVSE status.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<EVSEStatusSchedule> EVSEStatusSchedule(IncludeEVSEDelegate             IncludeEVSEs      = null,
                                                                  Func<DateTime,        Boolean>  TimestampFilter   = null,
                                                                  Func<EVSEStatusTypes, Boolean>  StatusFilter      = null,
                                                                  UInt64?                         HistorySize       = null)
        {

            if (IncludeEVSEs == null)
                IncludeEVSEs = evse => true;

            return _ChargingPools.
                         SelectMany(pool           => pool.EVSEs).
                         Where     (evse           => IncludeEVSEs(evse)).
                         Select    (evse           => new EVSEStatusSchedule(evse.Id,
                                                                             evse.StatusSchedule(TimestampFilter,
                                                                                                 StatusFilter,
                                                                                                 HistorySize))).
                         Where     (statusSchedule => statusSchedule.StatusSchedule.Count() > 0);

        }

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)

            => _ChargingPools.Any(pool => pool.ContainsEVSE(EVSE.Id));

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)

            => _ChargingPools.Any(pool => pool.ContainsEVSE(EVSEId));

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id? EVSEId)
        {

            if (!EVSEId.HasValue)
                return false;

            return _ChargingPools.Any(pool => pool.ContainsEVSE(EVSEId.Value));

        }

        #endregion

        #region GetEVSEById(EVSEId)

        public EVSE GetEVSEById(EVSE_Id EVSEId)

            => _ChargingPools.
                   SelectMany    (pool    => pool.   ChargingStations).
                   SelectMany    (station => station.EVSEs).
                   FirstOrDefault(evse    => evse.Id == EVSEId);

        #endregion

        #region TryGetEVSEById(EVSEId, out EVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = _ChargingPools.
                       SelectMany    (pool    => pool.   ChargingStations).
                       SelectMany    (station => station.EVSEs).
                       FirstOrDefault(evse    => evse.Id == EVSEId);

            return EVSE != null;

        }

        public Boolean TryGetEVSEbyId(EVSE_Id? EVSEId, out EVSE EVSE)
        {

            if (!EVSEId.HasValue)
            {
                EVSE = null;
                return false;
            }

            EVSE = _ChargingPools.
                       SelectMany    (pool    => pool.   ChargingStations).
                       SelectMany    (station => station.EVSEs).
                       FirstOrDefault(evse    => evse.Id == EVSEId);

            return EVSE != null;

        }

        #endregion

        #region TryGetChargingPoolByEVSEId(EVSEId, out Pool)

        public Boolean TryGetChargingPoolByEVSEId(EVSE_Id EVSEId, out ChargingPool Pool)
        {

            foreach (var pool in _ChargingPools)
            {
                foreach (var station in pool.ChargingStations)
                {
                    if (station.TryGetEVSEById(EVSEId, out EVSE evse))
                    {
                        Pool = pool;
                        return true;
                    }
                }
            }

            Pool = null;
            return false;

        }

        #endregion



        #region ValidEVSEIds

        //private readonly ReactiveSet<EVSE_Id> _ValidEVSEIds;

        ///// <summary>
        ///// A list of valid EVSE Ids. All others will be filtered.
        ///// </summary>
        //public ReactiveSet<EVSE_Id> ValidEVSEIds
        //{
        //    get
        //    {
        //        return _ValidEVSEIds;
        //    }
        //}

        #endregion

        #region InvalidEVSEIds

        /// <summary>
        /// A list of invalid EVSE Ids.
        /// </summary>
        public ReactiveSet<EVSE_Id> InvalidEVSEIds { get; }

        #endregion


        #region SetEVSEStatus(NewStatus)

        public void SetEVSEStatus(EVSEStatus  NewStatus)
        {

            if (TryGetEVSEById(NewStatus.Id, out EVSE _EVSE))
                _EVSE.SetStatus(NewStatus.Status);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewStatus)

        public void SetEVSEStatus(EVSE_Id          EVSEId,
                                  EVSEStatusTypes  NewStatus)
        {

            if (TryGetEVSEById(EVSEId, out EVSE _EVSE))
                _EVSE.SetStatus(NewStatus);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewTimestampedStatus)

        public void SetEVSEStatus(EVSE_Id                      EVSEId,
                                  Timestamped<EVSEStatusTypes>  NewTimestampedStatus)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEById(EVSEId, out _EVSE))
                _EVSE.SetStatus(NewTimestampedStatus);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewStatus, Timestamp)

        public void SetEVSEStatus(EVSE_Id         EVSEId,
                                  EVSEStatusTypes  NewStatus,
                                  DateTime        Timestamp)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEById(EVSEId, out _EVSE))
                _EVSE.SetStatus(NewStatus, Timestamp);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEVSEStatus(EVSE_Id                                   EVSEId,
                                  IEnumerable<Timestamped<EVSEStatusTypes>>  StatusList,
                                  ChangeMethods                             ChangeMethod  = ChangeMethods.Replace)
        {

            if (InvalidEVSEIds.Contains(EVSEId))
                return;

            EVSE _EVSE  = null;
            if (TryGetEVSEById(EVSEId, out _EVSE))
                _EVSE.SetStatus(StatusList, ChangeMethod);

        }

        #endregion

        #region CalcEVSEStatusDiff(EVSEStatus, IncludeEVSE = null)

        //public EVSEStatusDiff CalcEVSEStatusDiff(Dictionary<EVSE_Id, EVSEStatusTypes>  EVSEStatus,
        //                                         Func<EVSE, Boolean>                  IncludeEVSE  = null)
        //{

        //    if (EVSEStatus == null || EVSEStatus.Count == 0)
        //        return new EVSEStatusDiff(DateTime.UtcNow, Id, Name);

        //    #region Get data...

        //    var EVSEStatusDiff     = new EVSEStatusDiff(DateTime.UtcNow, Id, Name);

        //    // Only ValidEVSEIds!
        //    // Do nothing with manual EVSE Ids!
        //    var CurrentEVSEStates  = AllEVSEStatus(IncludeEVSE).
        //                                 //Where(KVP => ValidEVSEIds. Contains(KVP.Key) &&
        //                                 //            !ManualEVSEIds.Contains(KVP.Key)).
        //                                 ToDictionary(v => v.Key, v => v.Value);

        //    var OldEVSEIds         = new List<EVSE_Id>(CurrentEVSEStates.Keys);

        //    #endregion

        //    try
        //    {

        //        #region Find new and changed EVSE states

        //        // Only for ValidEVSEIds!
        //        // Do nothing with manual EVSE Ids!
        //        foreach (var NewEVSEStatus in EVSEStatus)
        //                                          //Where(KVP => ValidEVSEIds. Contains(KVP.Key) &&
        //                                          //            !ManualEVSEIds.Contains(KVP.Key)))
        //        {

        //            // Add to NewEVSEStates, if new EVSE was found!
        //            if (!CurrentEVSEStates.ContainsKey(NewEVSEStatus.Key))
        //                EVSEStatusDiff.AddNewStatus(NewEVSEStatus);

        //            else
        //            {

        //                // Add to CHANGED, if state of known EVSE changed!
        //                if (CurrentEVSEStates[NewEVSEStatus.Key] != NewEVSEStatus.Value)
        //                    EVSEStatusDiff.AddChangedStatus(NewEVSEStatus);

        //                // Remove EVSEId, as it was processed...
        //                OldEVSEIds.Remove(NewEVSEStatus.Key);

        //            }

        //        }

        //        #endregion

        //        #region Delete what is left in OldEVSEIds!

        //        EVSEStatusDiff.AddRemovedId(OldEVSEIds);

        //        #endregion

        //        return EVSEStatusDiff;

        //    }

        //    catch (Exception e)
        //    {

        //        while (e.InnerException != null)
        //            e = e.InnerException;

        //        DebugX.Log("GetEVSEStatusDiff led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

        //    }

        //    // empty!
        //    return new EVSEStatusDiff(DateTime.UtcNow, Id, Name);

        //}

        #endregion

        #region ApplyEVSEStatusDiff(EVSEStatusDiff)

        public EVSEStatusDiff ApplyEVSEStatusDiff(EVSEStatusDiff EVSEStatusDiff)
        {

            #region Initial checks

            if (EVSEStatusDiff == null)
                throw new ArgumentNullException(nameof(EVSEStatusDiff),  "The given EVSE status diff must not be null!");

            #endregion

            foreach (var status in EVSEStatusDiff.NewStatus)
                SetEVSEStatus(status.Key, status.Value);

            foreach (var status in EVSEStatusDiff.ChangedStatus)
                SetEVSEStatus(status.Key, status.Value);

            return EVSEStatusDiff;

        }

        #endregion


        #region SetEVSEAdminStatus(EVSEId, NewAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id              EVSEId,
                                       EVSEAdminStatusTypes  NewAdminStatus)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEById(EVSEId, out _EVSE))
                _EVSE.SetAdminStatus(NewAdminStatus);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, NewTimestampedAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id                           EVSEId,
                                       Timestamped<EVSEAdminStatusTypes>  NewTimestampedAdminStatus)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEById(EVSEId, out _EVSE))
                _EVSE.SetAdminStatus(NewTimestampedAdminStatus);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, NewAdminStatus, Timestamp)

        public void SetEVSEAdminStatus(EVSE_Id              EVSEId,
                                       EVSEAdminStatusTypes  NewAdminStatus,
                                       DateTime             Timestamp)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEById(EVSEId, out _EVSE))
                _EVSE.SetAdminStatus(NewAdminStatus, Timestamp);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, AdminStatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEVSEAdminStatus(EVSE_Id                                         EVSEId,
                                       IEnumerable<Timestamped<EVSEAdminStatusTypes>>  AdminStatusList,
                                       ChangeMethods                                   ChangeMethod  = ChangeMethods.Replace)
        {

            if (InvalidEVSEIds.Contains(EVSEId))
                return;

            if (TryGetEVSEById(EVSEId, out EVSE _EVSE))
                _EVSE.SetAdminStatus(AdminStatusList, ChangeMethod);

        }

        #endregion

        #region ApplyEVSEAdminStatusDiff(EVSEAdminStatusDiff)

        public EVSEAdminStatusDiff ApplyEVSEAdminStatusDiff(EVSEAdminStatusDiff EVSEAdminStatusDiff)
        {

            #region Initial checks

            if (EVSEAdminStatusDiff == null)
                throw new ArgumentNullException(nameof(EVSEAdminStatusDiff),  "The given EVSE admin status diff must not be null!");

            #endregion

            foreach (var status in EVSEAdminStatusDiff.NewStatus)
                SetEVSEAdminStatus(status.Key, status.Value);

            foreach (var status in EVSEAdminStatusDiff.ChangedStatus)
                SetEVSEAdminStatus(status.Key, status.Value);

            return EVSEAdminStatusDiff;

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


        #region (internal) UpdateEVSEData       (Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
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
        /// Update an EVSE admin status.
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
        /// Update an EVSE status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                     Timestamp,
                                             EventTracking_Id             EventTrackingId,
                                             EVSE                         EVSE,
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


        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSERemoval

            => EVSERemoval;

        #endregion

        #endregion

        #region EVSE groups

        #region EVSEGroupAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, EVSEGroup, Boolean> EVSEGroupAddition;

        /// <summary>
        /// Called whenever a EVSE group will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, EVSEGroup, Boolean> OnEVSEGroupAddition

            => EVSEGroupAddition;

        #endregion


        #region EVSEGroups

        private readonly EntityHashSet<ChargingStationOperator, EVSEGroup_Id, EVSEGroup> _EVSEGroups;

        /// <summary>
        /// All EVSE groups registered within this charging station operator.
        /// </summary>
        public IEnumerable<EVSEGroup> EVSEGroups

            => _EVSEGroups;

        #endregion


        #region CreateEVSEGroup     (Id,       Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Name">The offical (multi-language) name of this EVSE group.</param>
        /// <param name="Description">An optional (multi-language) description of this EVSE group.</param>
        /// 
        /// <param name="Members">An enumeration of EVSEs member building this EVSE group.</param>
        /// <param name="MemberIds">An enumeration of EVSE identifications which are building this EVSE group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new EVSEs automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated EVSEs.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the EVSE group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the EVSE group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        public EVSEGroup CreateEVSEGroup(EVSEGroup_Id                                   Id,
                                         I18NString                                     Name,
                                         I18NString                                     Description                   = null,

                                         Brand                                          Brand                         = null,
                                         Priority?                                      Priority                      = null,
                                         ChargingTariff                                 Tariff                        = null,
                                         IEnumerable<DataLicense>                       DataLicenses                  = null,

                                         IEnumerable<EVSE>                              Members                       = null,
                                         IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                         Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                         Func<EVSE,    Boolean>                         AutoIncludeEVSEs              = null,

                                         Func<EVSEStatusReport, EVSEGroupStatusTypes>   StatusAggregationDelegate     = null,
                                         UInt16                                         MaxGroupStatusListSize        = EVSEGroup.DefaultMaxGroupStatusListSize,
                                         UInt16                                         MaxGroupAdminStatusListSize   = EVSEGroup.DefaultMaxGroupAdminStatusListSize,

                                         Action<EVSEGroup>                              OnSuccess                     = null,
                                         Action<ChargingStationOperator, EVSEGroup_Id>  OnError                       = null)

        {

            lock (_EVSEGroups)
            {

                #region Initial checks

                if (_EVSEGroups.ContainsId(Id))
                {

                    if (OnError != null)
                        OnError?.Invoke(this, Id);

                    throw new EVSEGroupAlreadyExists(this, Id);

                }

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the EVSE group must not be null or empty!");

                #endregion

                var _EVSEGroup = new EVSEGroup(Id,
                                               this,
                                               Name,
                                               Description,

                                               Brand,
                                               Priority,
                                               Tariff,
                                               DataLicenses,

                                               Members,
                                               MemberIds,
                                               AutoIncludeEVSEIds,
                                               AutoIncludeEVSEs,
                                               StatusAggregationDelegate,
                                               MaxGroupAdminStatusListSize,
                                               MaxGroupStatusListSize);


                if (EVSEGroupAddition.SendVoting(DateTime.UtcNow, this, _EVSEGroup) &&
                    _EVSEGroups.TryAdd(_EVSEGroup))
                {

                    _EVSEGroup.OnEVSEDataChanged                             += UpdateEVSEData;
                    _EVSEGroup.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                    _EVSEGroup.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;

                    _EVSEGroup.OnEVSEDataChanged                  += UpdateEVSEData;
                    _EVSEGroup.OnEVSEStatusChanged                += UpdateEVSEStatus;
                    _EVSEGroup.OnEVSEAdminStatusChanged           += UpdateEVSEAdminStatus;

                    //_EVSEGroup.OnDataChanged                                 += UpdateEVSEGroupData;
                    //_EVSEGroup.OnAdminStatusChanged                          += UpdateEVSEGroupAdminStatus;

                    OnSuccess?.Invoke(_EVSEGroup);

                    EVSEGroupAddition.SendNotification(DateTime.UtcNow,
                                                                  this,
                                                                  _EVSEGroup);

                    return _EVSEGroup;

                }

                return null;

            }

        }

        #endregion

        #region CreateEVSEGroup     (IdSuffix, Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging group.</param>
        /// <param name="Name">The offical (multi-language) name of this EVSE group.</param>
        /// <param name="Description">An optional (multi-language) description of this EVSE group.</param>
        /// 
        /// <param name="Members">An enumeration of EVSEs member building this EVSE group.</param>
        /// <param name="MemberIds">An enumeration of EVSE identifications which are building this EVSE group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new EVSEs automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated EVSEs.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the EVSE group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the EVSE group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        public EVSEGroup CreateEVSEGroup(String                                         IdSuffix,
                                         I18NString                                     Name,
                                         I18NString                                     Description                   = null,

                                         Brand                                          Brand                         = null,
                                         Priority?                                      Priority                      = null,
                                         ChargingTariff                                 Tariff                        = null,
                                         IEnumerable<DataLicense>                       DataLicenses                  = null,

                                         IEnumerable<EVSE>                              Members                       = null,
                                         IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                         Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                         Func<EVSE,    Boolean>                         AutoIncludeEVSEs              = null,

                                         Func<EVSEStatusReport, EVSEGroupStatusTypes>   StatusAggregationDelegate     = null,
                                         UInt16                                         MaxGroupStatusListSize        = EVSEGroup.DefaultMaxGroupStatusListSize,
                                         UInt16                                         MaxGroupAdminStatusListSize   = EVSEGroup.DefaultMaxGroupAdminStatusListSize,

                                         Action<EVSEGroup>                              OnSuccess                     = null,
                                         Action<ChargingStationOperator, EVSEGroup_Id>  OnError                       = null)

        {

            #region Initial checks

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix), "The given suffix of the unique identification of the new charging group must not be null or empty!");

            #endregion

            return CreateEVSEGroup(EVSEGroup_Id.Parse(Id,
                                                      IdSuffix.Trim().ToUpper()),
                                   Name,
                                   Description,

                                   Brand,
                                   Priority,
                                   Tariff,
                                   DataLicenses,

                                   Members,
                                   MemberIds,
                                   AutoIncludeEVSEIds,
                                   AutoIncludeEVSEs,
                                   StatusAggregationDelegate,
                                   MaxGroupAdminStatusListSize,
                                   MaxGroupStatusListSize,
                                   OnSuccess,
                                   OnError);

        }

        #endregion

        #region GetOrCreateEVSEGroup(Id,       Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Name">The offical (multi-language) name of this EVSE group.</param>
        /// <param name="Description">An optional (multi-language) description of this EVSE group.</param>
        /// 
        /// <param name="Members">An enumeration of EVSEs member building this EVSE group.</param>
        /// <param name="MemberIds">An enumeration of EVSE identifications which are building this EVSE group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new EVSEs automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated EVSEs.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the EVSE group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the EVSE group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        public EVSEGroup GetOrCreateEVSEGroup(EVSEGroup_Id                                   Id,
                                              I18NString                                     Name,
                                              I18NString                                     Description                   = null,

                                              Brand                                          Brand                         = null,
                                              Priority?                                      Priority                      = null,
                                              ChargingTariff                                 Tariff                        = null,
                                              IEnumerable<DataLicense>                       DataLicenses                  = null,

                                              IEnumerable<EVSE>                              Members                       = null,
                                              IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                              Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                              Func<EVSE,    Boolean>                         AutoIncludeEVSEs              = null,

                                              Func<EVSEStatusReport, EVSEGroupStatusTypes>   StatusAggregationDelegate     = null,
                                              UInt16                                         MaxGroupStatusListSize        = EVSEGroup.DefaultMaxGroupStatusListSize,
                                              UInt16                                         MaxGroupAdminStatusListSize   = EVSEGroup.DefaultMaxGroupAdminStatusListSize,

                                              Action<EVSEGroup>                              OnSuccess                     = null,
                                              Action<ChargingStationOperator, EVSEGroup_Id>  OnError                       = null)

        {

            lock (_EVSEGroups)
            {

                #region Initial checks

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the EVSE group must not be null or empty!");

                #endregion

                if (_EVSEGroups.TryGet(Id, out EVSEGroup _EVSEGroup))
                    return _EVSEGroup;

                return CreateEVSEGroup(Id,
                                       Name,
                                       Description,

                                       Brand,
                                       Priority,
                                       Tariff,
                                       DataLicenses,

                                       Members,
                                       MemberIds,
                                       AutoIncludeEVSEIds,
                                       AutoIncludeEVSEs,
                                       StatusAggregationDelegate,
                                       MaxGroupAdminStatusListSize,
                                       MaxGroupStatusListSize,
                                       OnSuccess,
                                       OnError);

            }

        }

        #endregion

        #region GetOrCreateEVSEGroup(IdSuffix, Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging group.</param>
        /// <param name="Name">The offical (multi-language) name of this EVSE group.</param>
        /// <param name="Description">An optional (multi-language) description of this EVSE group.</param>
        /// 
        /// <param name="Members">An enumeration of EVSEs member building this EVSE group.</param>
        /// <param name="MemberIds">An enumeration of EVSE identifications which are building this EVSE group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new EVSEs automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated EVSEs.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the EVSE group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the EVSE group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        public EVSEGroup GetOrCreateEVSEGroup(String                                         IdSuffix,
                                              I18NString                                     Name,
                                              I18NString                                     Description                   = null,

                                              Brand                                          Brand                         = null,
                                              Priority?                                      Priority                      = null,
                                              ChargingTariff                                 Tariff                        = null,
                                              IEnumerable<DataLicense>                       DataLicenses                  = null,

                                              IEnumerable<EVSE>                              Members                       = null,
                                              IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                              Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                              Func<EVSE,    Boolean>                         AutoIncludeEVSEs              = null,

                                              Func<EVSEStatusReport, EVSEGroupStatusTypes>   StatusAggregationDelegate     = null,
                                              UInt16                                         MaxGroupStatusListSize        = EVSEGroup.DefaultMaxGroupStatusListSize,
                                              UInt16                                         MaxGroupAdminStatusListSize   = EVSEGroup.DefaultMaxGroupAdminStatusListSize,

                                              Action<EVSEGroup>                              OnSuccess                     = null,
                                              Action<ChargingStationOperator, EVSEGroup_Id>  OnError                       = null)


        {

            #region Initial checks

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix), "The given suffix of the unique identification of the new charging group must not be null or empty!");

            #endregion

            return GetOrCreateEVSEGroup(EVSEGroup_Id.Parse(Id, IdSuffix.Trim().ToUpper()),
                                        Name,
                                        Description,

                                        Brand,
                                        Priority,
                                        Tariff,
                                        DataLicenses,

                                        Members,
                                        MemberIds,
                                        AutoIncludeEVSEIds,
                                        AutoIncludeEVSEs,
                                        StatusAggregationDelegate,
                                        MaxGroupAdminStatusListSize,
                                        MaxGroupStatusListSize,
                                        OnSuccess,
                                        OnError);

        }

        #endregion


        #region TryGetEVSEGroup(Id, out EVSEGroup)

        /// <summary>
        /// Try to return to EVSE group for the given EVSE group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="EVSEGroup">The charing station group.</param>
        public Boolean TryGetEVSEGroup(EVSEGroup_Id   Id,
                                                  out EVSEGroup  EVSEGroup)

            => _EVSEGroups.TryGet(Id, out EVSEGroup);

        #endregion


        #region EVSEGroupRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, EVSEGroup, Boolean> EVSEGroupRemoval;

        /// <summary>
        /// Called whenever a EVSE group will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, EVSEGroup, Boolean> OnEVSEGroupRemoval

            => EVSEGroupRemoval;

        #endregion

        #region RemoveEVSEGroup(EVSEGroupId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All EVSE groups registered within this charging station operator.
        /// </summary>
        /// <param name="EVSEGroupId">The unique identification of the EVSE group to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new EVSE group after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the EVSE group failed.</param>
        public EVSEGroup RemoveEVSEGroup(EVSEGroup_Id                                   EVSEGroupId,
                                                               Action<ChargingStationOperator, EVSEGroup>     OnSuccess   = null,
                                                               Action<ChargingStationOperator, EVSEGroup_Id>  OnError     = null)
        {

            lock (_EVSEGroups)
            {

                if (_EVSEGroups.TryGet(EVSEGroupId, out EVSEGroup EVSEGroup) &&
                    EVSEGroupRemoval.SendVoting(DateTime.UtcNow,
                                                           this,
                                                           EVSEGroup) &&
                    _EVSEGroups.TryRemove(EVSEGroupId, out EVSEGroup _EVSEGroup))
                {

                    OnSuccess?.Invoke(this, EVSEGroup);

                    EVSEGroupRemoval.SendNotification(DateTime.UtcNow,
                                                                 this,
                                                                 _EVSEGroup);

                    return _EVSEGroup;

                }

                OnError?.Invoke(this, EVSEGroupId);

                return null;

            }

        }

        #endregion

        #region RemoveEVSEGroup(EVSEGroup,   OnSuccess = null, OnError = null)

        /// <summary>
        /// All EVSE groups registered within this charging station operator.
        /// </summary>
        /// <param name="EVSEGroup">The EVSE group to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new EVSE group after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the EVSE group failed.</param>
        public EVSEGroup RemoveEVSEGroup(EVSEGroup                                   EVSEGroup,
                                                               Action<ChargingStationOperator, EVSEGroup>  OnSuccess   = null,
                                                               Action<ChargingStationOperator, EVSEGroup>  OnError     = null)
        {

            lock (_EVSEGroups)
            {

                if (EVSEGroupRemoval.SendVoting(DateTime.UtcNow,
                                                           this,
                                                           EVSEGroup) &&
                    _EVSEGroups.TryRemove(EVSEGroup.Id, out EVSEGroup _EVSEGroup))
                {

                    OnSuccess?.Invoke(this, _EVSEGroup);

                    EVSEGroupRemoval.SendNotification(DateTime.UtcNow,
                                                                 this,
                                                                 _EVSEGroup);

                    return _EVSEGroup;

                }

                OnError?.Invoke(this, EVSEGroup);

                return EVSEGroup;

            }

        }

        #endregion

        #endregion


        #region ChargingTariffs

        #region ChargingTariffAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariff, Boolean> ChargingTariffAddition;

        /// <summary>
        /// Called whenever a charging tariff will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingTariff, Boolean> OnChargingTariffAddition

            => ChargingTariffAddition;

        #endregion


        #region ChargingTariffs

        private readonly EntityHashSet<ChargingStationOperator, ChargingTariff_Id, ChargingTariff> _ChargingTariffs;

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        public IEnumerable<ChargingTariff> ChargingTariffs

            => _ChargingTariffs;

        #endregion


        #region CreateChargingTariff     (Id,       Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        public ChargingTariff CreateChargingTariff(ChargingTariff_Id                                   Id,
                                                   I18NString                                          Name,
                                                   I18NString                                          Description,
                                                   Brand                                               Brand,
                                                   Uri                                                 TariffUrl,
                                                   Currency                                            Currency,
                                                   EnergyMix                                           EnergyMix,
                                                   IEnumerable<ChargingTariffElement>                  TariffElements,

                                                   Action<ChargingTariff>                              OnSuccess  = null,
                                                   Action<ChargingStationOperator, ChargingTariff_Id>  OnError    = null)

        {

            lock (_ChargingTariffs)
            {

                #region Initial checks

                if (_ChargingTariffs.ContainsId(Id))
                {

                    if (OnError != null)
                        OnError?.Invoke(this, Id);

                    throw new ChargingTariffAlreadyExists(this, Id);

                }

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the charging station group must not be null or empty!");

                #endregion

                var _ChargingTariff = new ChargingTariff(Id,
                                                         this,
                                                         Name,
                                                         Description,
                                                         Brand,
                                                         TariffUrl,
                                                         Currency,
                                                         EnergyMix,
                                                         TariffElements);


                if (ChargingTariffAddition.SendVoting(DateTime.UtcNow, this, _ChargingTariff) &&
                    _ChargingTariffs.TryAdd(_ChargingTariff))
                {

                    //_ChargingTariff.OnEVSEDataChanged                             += UpdateEVSEData;
                    //_ChargingTariff.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                    //_ChargingTariff.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;

                    //_ChargingTariff.OnChargingStationDataChanged                  += UpdateChargingStationData;
                    //_ChargingTariff.OnChargingStationStatusChanged                += UpdateChargingStationStatus;
                    //_ChargingTariff.OnChargingStationAdminStatusChanged           += UpdateChargingStationAdminStatus;

                    ////_ChargingTariff.OnDataChanged                                 += UpdateChargingTariffData;
                    ////_ChargingTariff.OnAdminStatusChanged                          += UpdateChargingTariffAdminStatus;

                    OnSuccess?.Invoke(_ChargingTariff);

                    ChargingTariffAddition.SendNotification(DateTime.UtcNow,
                                                            this,
                                                            _ChargingTariff);

                    return _ChargingTariff;

                }

                return null;

            }

        }

        #endregion

        #region CreateChargingTariff     (IdSuffix, Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        public ChargingTariff CreateChargingTariff(String                                                        IdSuffix,
                                                   I18NString                                                    Name,
                                                   I18NString                                                    Description,
                                                   Brand                                                         Brand,
                                                   Uri                                                           TariffUrl,
                                                   Currency                                                      Currency,
                                                   EnergyMix                                                     EnergyMix,
                                                   IEnumerable<ChargingTariffElement>                            TariffElements,

                                                   Action<ChargingTariff>                                        OnSuccess                     = null,
                                                   Action<ChargingStationOperator, ChargingTariff_Id>            OnError                       = null)

        {

            #region Initial checks

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix), "The given suffix of the unique identification of the new charging tariff must not be null or empty!");

            #endregion

            return CreateChargingTariff(ChargingTariff_Id.Parse(Id,
                                                                IdSuffix.Trim().ToUpper()),
                                        Name,
                                        Description,
                                        Brand,
                                        TariffUrl,
                                        Currency,
                                        EnergyMix,
                                        TariffElements,

                                        OnSuccess,
                                        OnError);

        }

        #endregion

        #region GetOrCreateChargingTariff(Id,       Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        public ChargingTariff GetOrCreateChargingTariff(ChargingTariff_Id                                   Id,
                                                        I18NString                                          Name,
                                                        I18NString                                          Description,
                                                        Brand                                               Brand,
                                                        Uri                                                 TariffUrl,
                                                        Currency                                            Currency,
                                                        EnergyMix                                           EnergyMix,
                                                        IEnumerable<ChargingTariffElement>                  TariffElements,

                                                        Action<ChargingTariff>                              OnSuccess  = null,
                                                        Action<ChargingStationOperator, ChargingTariff_Id>  OnError    = null)

        {

            lock (_ChargingTariffs)
            {

                #region Initial checks

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the charging tariff must not be null or empty!");

                #endregion

                if (_ChargingTariffs.TryGet(Id, out ChargingTariff _ChargingTariff))
                    return _ChargingTariff;

                return CreateChargingTariff(Id,
                                            Name,
                                            Description,
                                            Brand,
                                            TariffUrl,
                                            Currency,
                                            EnergyMix,
                                            TariffElements,

                                            OnSuccess,
                                            OnError);

            }

        }

        #endregion

        #region GetOrCreateChargingTariff(IdSuffix, Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        public ChargingTariff GetOrCreateChargingTariff(String                                              IdSuffix,
                                                        I18NString                                          Name,
                                                        I18NString                                          Description,
                                                        Brand                                               Brand,
                                                        Uri                                                 TariffUrl,
                                                        Currency                                            Currency,
                                                        EnergyMix                                           EnergyMix,
                                                        IEnumerable<ChargingTariffElement>                  TariffElements,

                                                        Action<ChargingTariff>                              OnSuccess  = null,
                                                        Action<ChargingStationOperator, ChargingTariff_Id>  OnError    = null)


        {

            #region Initial checks

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix), "The given suffix of the unique identification of the new charging tariff must not be null or empty!");

            #endregion

            return GetOrCreateChargingTariff(ChargingTariff_Id.Parse(Id,
                                                                     IdSuffix.Trim().ToUpper()),
                                             Name,
                                             Description,
                                             Brand,
                                             TariffUrl,
                                             Currency,
                                             EnergyMix,
                                             TariffElements,

                                             OnSuccess,
                                             OnError);

        }

        #endregion


        #region GetChargingTariff(Id)

        /// <summary>
        /// Return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        public ChargingTariff GetChargingTariff(ChargingTariff_Id Id)
        {

            if (_ChargingTariffs.TryGet(Id, out ChargingTariff Tariff))
                return Tariff;

            return null;

        }

        #endregion

        #region TryGetChargingTariff(Id, out ChargingTariff)

        /// <summary>
        /// Try to return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="ChargingTariff">The charing tariff.</param>
        public Boolean TryGetChargingTariff(ChargingTariff_Id   Id,
                                            out ChargingTariff  ChargingTariff)

            => _ChargingTariffs.TryGet(Id, out ChargingTariff);

        #endregion


        #region ChargingTariffRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariff, Boolean> ChargingTariffRemoval;

        /// <summary>
        /// Called whenever a charging tariff will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingTariff, Boolean> OnChargingTariffRemoval

            => ChargingTariffRemoval;

        #endregion

        #region RemoveChargingTariff(ChargingTariffId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariffId">The unique identification of the charging tariff to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging tariff failed.</param>
        public ChargingTariff RemoveChargingTariff(ChargingTariff_Id                                   ChargingTariffId,
                                                               Action<ChargingStationOperator, ChargingTariff>     OnSuccess   = null,
                                                               Action<ChargingStationOperator, ChargingTariff_Id>  OnError     = null)
        {

            lock (_ChargingTariffs)
            {

                if (_ChargingTariffs.TryGet(ChargingTariffId, out ChargingTariff ChargingTariff) &&
                    ChargingTariffRemoval.SendVoting(DateTime.UtcNow,
                                                           this,
                                                           ChargingTariff) &&
                    _ChargingTariffs.TryRemove(ChargingTariffId, out ChargingTariff _ChargingTariff))
                {

                    OnSuccess?.Invoke(this, ChargingTariff);

                    ChargingTariffRemoval.SendNotification(DateTime.UtcNow,
                                                                 this,
                                                                 _ChargingTariff);

                    return _ChargingTariff;

                }

                OnError?.Invoke(this, ChargingTariffId);

                return null;

            }

        }

        #endregion

        #region RemoveChargingTariff(ChargingTariff,   OnSuccess = null, OnError = null)

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariff">The charging tariff to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging tariff failed.</param>
        public ChargingTariff RemoveChargingTariff(ChargingTariff                                   ChargingTariff,
                                                               Action<ChargingStationOperator, ChargingTariff>  OnSuccess   = null,
                                                               Action<ChargingStationOperator, ChargingTariff>  OnError     = null)
        {

            lock (_ChargingTariffs)
            {

                if (ChargingTariffRemoval.SendVoting(DateTime.UtcNow,
                                                           this,
                                                           ChargingTariff) &&
                    _ChargingTariffs.TryRemove(ChargingTariff.Id, out ChargingTariff _ChargingTariff))
                {

                    OnSuccess?.Invoke(this, _ChargingTariff);

                    ChargingTariffRemoval.SendNotification(DateTime.UtcNow,
                                                                 this,
                                                                 _ChargingTariff);

                    return _ChargingTariff;

                }

                OnError?.Invoke(this, ChargingTariff);

                return ChargingTariff;

            }

        }

        #endregion

        #endregion

        #region ChargingTariffGroups

        #region ChargingTariffGroupAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> ChargingTariffGroupAddition;

        /// <summary>
        /// Called whenever a charging tariff will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> OnChargingTariffGroupAddition

            => ChargingTariffGroupAddition;

        #endregion

        #region ChargingTariffGroups

        private readonly EntityHashSet<ChargingStationOperator, ChargingTariffGroup_Id, ChargingTariffGroup> _ChargingTariffGroups;

        /// <summary>
        /// All charging tariff groups registered within this charging station operator.
        /// </summary>
        public IEnumerable<ChargingTariffGroup> ChargingTariffGroups

            => _ChargingTariffGroups;

        #endregion


        #region CreateChargingTariffGroup     (IdSuffix, Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging tariff group having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the charing tariff group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff group.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff group failed.</param>
        public ChargingTariffGroup CreateChargingTariffGroup(String                                                   IdSuffix,
                                                             I18NString                                               Description,
                                                             Action<ChargingTariffGroup>                              OnSuccess  = null,
                                                             Action<ChargingStationOperator, ChargingTariffGroup_Id>  OnError    = null)

        {

            lock (_ChargingTariffGroups)
            {

                #region Initial checks

                var NewGroupId = ChargingTariffGroup_Id.Parse(Id, IdSuffix);

                if (_ChargingTariffGroups.ContainsId(NewGroupId))
                {

                    if (OnError != null)
                        OnError?.Invoke(this, NewGroupId);

                    throw new ChargingTariffGroupAlreadyExists(this, NewGroupId);

                }

                #endregion

                var _ChargingTariffGroup = new ChargingTariffGroup(NewGroupId,
                                                                   this,
                                                                   Description);


                if (ChargingTariffGroupAddition.SendVoting(DateTime.UtcNow, this, _ChargingTariffGroup) &&
                    _ChargingTariffGroups.TryAdd(_ChargingTariffGroup))
                {

                    //_ChargingTariffGroup.OnEVSEDataChanged                             += UpdateEVSEData;
                    //_ChargingTariffGroup.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                    //_ChargingTariffGroup.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;

                    //_ChargingTariffGroup.OnChargingStationDataChanged                  += UpdateChargingStationData;
                    //_ChargingTariffGroup.OnChargingStationStatusChanged                += UpdateChargingStationStatus;
                    //_ChargingTariffGroup.OnChargingStationAdminStatusChanged           += UpdateChargingStationAdminStatus;

                    ////_ChargingTariffGroup.OnDataChanged                                 += UpdateChargingTariffGroupData;
                    ////_ChargingTariffGroup.OnAdminStatusChanged                          += UpdateChargingTariffGroupAdminStatus;

                    OnSuccess?.Invoke(_ChargingTariffGroup);

                    ChargingTariffGroupAddition.SendNotification(DateTime.UtcNow,
                                                                 this,
                                                                 _ChargingTariffGroup);

                    return _ChargingTariffGroup;

                }

                return null;

            }

        }

        #endregion

        #region GetOrCreateChargingTariffGroup(Id,       Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the charing tariff group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        public ChargingTariffGroup GetOrCreateChargingTariffGroup(String                                                   IdSuffix,
                                                                  I18NString                                               Description,
                                                                  Action<ChargingTariffGroup>                              OnSuccess  = null,
                                                                  Action<ChargingStationOperator, ChargingTariffGroup_Id>  OnError    = null)

        {

            lock (_ChargingTariffGroups)
            {

                if (_ChargingTariffGroups.TryGet(ChargingTariffGroup_Id.Parse(Id, IdSuffix), out ChargingTariffGroup _ChargingTariffGroup))
                    return _ChargingTariffGroup;

                return CreateChargingTariffGroup(IdSuffix,
                                                 Description,
                                                 OnSuccess,
                                                 OnError);

            }

        }

        #endregion


        #region GetChargingTariffGroup(Id)

        /// <summary>
        /// Return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        public ChargingTariffGroup GetChargingTariffGroup(ChargingTariffGroup_Id Id)
        {

            if (_ChargingTariffGroups.TryGet(Id, out ChargingTariffGroup TariffGroup))
                return TariffGroup;

            return null;

        }

        #endregion

        #region TryGetChargingTariffGroup(Id, out ChargingTariffGroup)

        /// <summary>
        /// Try to return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="ChargingTariffGroup">The charing tariff.</param>
        public Boolean TryGetChargingTariffGroup(ChargingTariffGroup_Id Id,
                                                 out ChargingTariffGroup ChargingTariffGroup)

            => _ChargingTariffGroups.TryGet(Id, out ChargingTariffGroup);

        #endregion


        #region ChargingTariffGroupRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> ChargingTariffGroupRemoval;

        /// <summary>
        /// Called whenever a charging tariff group will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> OnChargingTariffGroupRemoval

            => ChargingTariffGroupRemoval;

        #endregion

        #region RemoveChargingTariffGroup(ChargingTariffGroupId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariffGroupId">The unique identification of the charging tariff to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging tariff failed.</param>
        public ChargingTariffGroup RemoveChargingTariffGroup(ChargingTariffGroup_Id                                   ChargingTariffGroupId,
                                                             Action<ChargingStationOperator, ChargingTariffGroup>     OnSuccess   = null,
                                                             Action<ChargingStationOperator, ChargingTariffGroup_Id>  OnError     = null)
        {

            lock (_ChargingTariffGroups)
            {

                if (_ChargingTariffGroups.TryGet(ChargingTariffGroupId, out ChargingTariffGroup ChargingTariffGroup) &&
                    ChargingTariffGroupRemoval.SendVoting(DateTime.UtcNow,
                                                          this,
                                                          ChargingTariffGroup) &&
                    _ChargingTariffGroups.TryRemove(ChargingTariffGroupId, out ChargingTariffGroup _ChargingTariffGroup))
                {

                    OnSuccess?.Invoke(this, ChargingTariffGroup);

                    ChargingTariffGroupRemoval.SendNotification(DateTime.UtcNow,
                                                                this,
                                                                _ChargingTariffGroup);

                    return _ChargingTariffGroup;

                }

                OnError?.Invoke(this, ChargingTariffGroupId);

                return null;

            }

        }

        #endregion

        #region RemoveChargingTariffGroup(ChargingTariffGroup,   OnSuccess = null, OnError = null)

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariffGroup">The charging tariff to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging tariff failed.</param>
        public ChargingTariffGroup RemoveChargingTariffGroup(ChargingTariffGroup                                   ChargingTariffGroup,
                                                             Action<ChargingStationOperator, ChargingTariffGroup>  OnSuccess   = null,
                                                             Action<ChargingStationOperator, ChargingTariffGroup>  OnError     = null)
        {

            lock (_ChargingTariffGroups)
            {

                if (ChargingTariffGroupRemoval.SendVoting(DateTime.UtcNow,
                                                          this,
                                                          ChargingTariffGroup) &&
                    _ChargingTariffGroups.TryRemove(ChargingTariffGroup.Id, out ChargingTariffGroup _ChargingTariffGroup))
                {

                    OnSuccess?.Invoke(this, _ChargingTariffGroup);

                    ChargingTariffGroupRemoval.SendNotification(DateTime.UtcNow,
                                                                this,
                                                                _ChargingTariffGroup);

                    return _ChargingTariffGroup;

                }

                OnError?.Invoke(this, ChargingTariffGroup);

                return ChargingTariffGroup;

            }

        }

        #endregion

        #endregion


        #region Reservations...

        #region Data

        public IEnumerable<ChargingReservation> ChargingReservations
            => RoamingNetwork.ReservationsStore.
                   Where (reservation => reservation.Last().ChargingStationOperatorId == Id).
                   Select(reservation => reservation.Last());

        public Boolean TryGetChargingReservationById(ChargingReservation_Id Id, out ChargingReservation ChargingReservation)
        {

            if (RoamingNetwork.ReservationsStore.TryGet(Id, out ChargingReservationCollection ReservationCollection))
            {
                ChargingReservation = ReservationCollection.Last();
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
        /// Reserve the possibility to charge at this charging station operator.
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


                => Reserve(ChargingLocation.FromChargingStationOperatorId(Id),
                           ChargingReservationLevel.ChargingStationOperator,
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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnReserveRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.ChargingStationOperatorId.HasValue && ChargingLocation.ChargingStationOperatorId.Value != Id)
                    result = ReservationResult.UnknownLocation;

                else if (AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.Operational ||
                         AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.InternalUse)
                {

                    if (RemoteChargingStationOperator != null)
                    {

                        result = await RemoteChargingStationOperator.
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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnReserveResponse));
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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.InternalUse)
                {

                    if (RemoteChargingStationOperator != null)
                    {

                        result = await RemoteChargingStationOperator.
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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnCancelReservationResponse));
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
            => RoamingNetwork.SessionsStore.Where(session => session.ChargingStationOperatorId == Id);

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
            => RoamingNetwork.SessionsStore.TryGet(SessionId, out ChargingSession);

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate              OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate             OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate              OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate               OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate              OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate           OnNewChargeDetailRecord;

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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.InternalUse)
                {

                    if ((ChargingLocation.EVSEId.           HasValue && TryGetChargingPoolByEVSEId   (ChargingLocation.EVSEId.           Value, out ChargingPool chargingPool)) ||
                        (ChargingLocation.ChargingStationId.HasValue && TryGetChargingPoolByStationId(ChargingLocation.ChargingStationId.Value, out              chargingPool)) ||
                        (ChargingLocation.ChargingPoolId.   HasValue && TryGetChargingPoolById       (ChargingLocation.ChargingPoolId.   Value, out              chargingPool)))
                    {

                        result = await chargingPool.
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
                                result.Session.ChargingStationOperator = this;

                        }

                        #endregion

                    }
                    else
                        result = RemoteStartResult.UnknownLocation();

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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnRemoteStartResponse));
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


            RemoteStopResult result        = null;
            ChargingPool    _ChargingPool  = null;

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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.InternalUse)
                {

                    if (TryGetChargingSessionById(SessionId, out ChargingSession chargingSession) &&
                       ((chargingSession.EVSEId.           HasValue && TryGetChargingPoolByEVSEId   (chargingSession.EVSEId.           Value, out ChargingPool chargingPool)) ||
                        (chargingSession.ChargingStationId.HasValue && TryGetChargingPoolByStationId(chargingSession.ChargingStationId.Value, out              chargingPool)) ||
                        (chargingSession.ChargingPoolId.   HasValue && TryGetChargingPoolById       (chargingSession.ChargingPoolId.   Value, out              chargingPool))))
                    {

                        result = await chargingPool.
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
                        DebugX.Log("Invalid charging session at charging station operator '" + Id + "': " + SessionId);
                        result = RemoteStopResult.InvalidSessionId(SessionId);
                    }

                    if (result.Result == RemoteStopResultTypes.Success)
                    {

                        //// The CDR could also be sent separately!
                        //if (result.ChargeDetailRecord != null)
                        //{
                        //    OnNewChargeDetailRecord?.Invoke(DateTime.UtcNow,
                        //                                    this,
                        //                                    result.ChargeDetailRecord);
                        //}

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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnRemoteStopResponse));
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

                if (Session.ChargingStationOperator == null)
                {
                    Session.ChargingStationOperator    = this;
                    Session.ChargingStationOperatorId  = Id;
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


        #region ToJSON(this ChargingStationOperator,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given charging station operator.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        public JObject ToJSON(Boolean                                                Embedded                                  = false,
                              InfoStatus                                             ExpandRoamingNetworkId                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                             ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                              InfoStatus                                             ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                              InfoStatus                                             ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                              InfoStatus                                             ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                              InfoStatus                                             ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<ChargingStationOperator>  CustomChargingStationOperatorSerializer   = null,
                              CustomJObjectSerializerDelegate<ChargingPool>             CustomChargingPoolSerializer              = null,
                              CustomJObjectSerializerDelegate<ChargingStation>          CustomChargingStationSerializer           = null,
                              CustomJObjectSerializerDelegate<EVSE>                     CustomEVSESerializer                      = null)
        {

            var JSON = JSONObject.Create(

                     new JProperty("@id",  Id.ToString()),

                         !Embedded
                             ? new JProperty("@context", JSONLDContext)
                             : null,

                         new JProperty("name",  Name.ToJSON()),

                         Description.IsNeitherNullNorEmpty()
                             ? Description.ToJSON("description")
                             : null,

                         DataSource.ToJSON("dataSource"),

                         ExpandDataLicenses != InfoStatus.Hidden
                             ? ExpandDataLicenses.Switch(
                                   () => new JProperty("dataLicenseIds",  new JArray(DataLicenses.SafeSelect(license => license.Id.ToString()))),
                                   () => new JProperty("dataLicenses",    DataLicenses.ToJSON()))
                             : null,

                         ExpandRoamingNetworkId != InfoStatus.Hidden
                             ? ExpandRoamingNetworkId.Switch(
                                   () => new JProperty("roamingNetworkId",   RoamingNetwork.Id. ToString()),
                                   () => new JProperty("roamingNetwork",     RoamingNetwork.    ToJSON(Embedded:                          true,
                                                                                                       ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
                                                                                                       ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                       ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                       ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                       ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                       ExpandDataLicenses:                InfoStatus.Hidden)))
                             : null,

                         Address?.ToJSON("address"),

                         // API
                         // MainKeys
                         // RobotKeys
                         // Endpoints
                         // DNS SRV

                         Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         Homepage.IsNotNullOrEmpty()
                             ? new JProperty("homepage",            Homepage)
                             : null,

                         HotlinePhoneNumber.IsNotNullOrEmpty()
                             ? new JProperty("hotline",             HotlinePhoneNumber)
                             : null,


                         ExpandChargingPoolIds != InfoStatus.Hidden && ChargingPools.Any()
                             ? ExpandChargingPoolIds.Switch(

                                   () => new JProperty("chargingPoolIds",
                                                       new JArray(ChargingPoolIds().
                                                                                          OrderBy(poolId => poolId).
                                                                                          Select (poolId => poolId.ToString()))),

                                   () => new JProperty("chargingPools",
                                                       new JArray(ChargingPools.
                                                                                          OrderBy(poolId => poolId).
                                                                                          ToJSON (Embedded:                         true,
                                                                                                  ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                  ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                  ExpandChargingStationIds:         InfoStatus.Expanded,
                                                                                                  ExpandEVSEIds:                    InfoStatus.Expanded,
                                                                                                  ExpandBrandIds:                   InfoStatus.ShowIdOnly,
                                                                                                  ExpandDataLicenses:               InfoStatus.Hidden,
                                                                                                  CustomChargingPoolSerializer:     CustomChargingPoolSerializer,
                                                                                                  CustomChargingStationSerializer:  CustomChargingStationSerializer,
                                                                                                  CustomEVSESerializer:             CustomEVSESerializer))))
                             : null,


                         ExpandChargingStationIds != InfoStatus.Hidden && ChargingStations.Any()
                             ? ExpandChargingStationIds.Switch(

                                   () => new JProperty("chargingStationIds",
                                                       new JArray(ChargingStationIds().
                                                                                          OrderBy(stationid => stationid).
                                                                                          Select (stationid => stationid.ToString()))),

                                   () => new JProperty("chargingStations",
                                                       new JArray(ChargingStations.
                                                                                          OrderBy(station   => station).
                                                                                          ToJSON (Embedded:                         true,
                                                                                                  ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                  ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                  ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                  ExpandEVSEIds:                    InfoStatus.Expanded,
                                                                                                  ExpandBrandIds:                   InfoStatus.ShowIdOnly,
                                                                                                  ExpandDataLicenses:               InfoStatus.Hidden,
                                                                                                  CustomChargingStationSerializer:  CustomChargingStationSerializer,
                                                                                                  CustomEVSESerializer:             CustomEVSESerializer))))
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
                                                                                          ToJSON (Embedded:                         true,
                                                                                                  ExpandChargingPoolIds:            InfoStatus.Hidden,
                                                                                                  ExpandChargingStationIds:         InfoStatus.Hidden,
                                                                                                  ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                  ExpandDataLicenses:               InfoStatus.ShowIdOnly))))
                             : null

                         );

            return CustomChargingStationOperatorSerializer != null
                       ? CustomChargingStationOperatorSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region IComparable<ChargingStationOperator> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSE_Operator.
            var EVSE_Operator = Object as ChargingStationOperator;
            if ((Object) EVSE_Operator == null)
                throw new ArgumentException("The given object is not an EVSE_Operator!");

            return CompareTo(EVSE_Operator);

        }

        #endregion

        #region CompareTo(Operator)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Operator">An Charging Station Operator object to compare with.</param>
        public Int32 CompareTo(ChargingStationOperator Operator)
        {

            if ((Object) Operator == null)
                throw new ArgumentNullException("The given Charging Station Operator must not be null!");

            return Id.CompareTo(Operator.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperator> Members

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

            // Check if the given object is an ChargingStationOperator.
            var EVSE_Operator = Object as ChargingStationOperator;
            if ((Object) EVSE_Operator == null)
                return false;

            return this.Equals(EVSE_Operator);

        }

        #endregion

        #region Equals(Operator)

        /// <summary>
        /// Compares two Charging Station Operators for equality.
        /// </summary>
        /// <param name="Operator">An Charging Station Operator to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationOperator Operator)
        {

            if ((Object) Operator == null)
                return false;

            return Id.Equals(Operator.Id);

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

            => String.Concat("'",
                             Name.FirstText(),
                             "' (",
                             Id.ToString(),
                             ") in ",
                             RoamingNetwork.Id.ToString());

        #endregion


    }

}
