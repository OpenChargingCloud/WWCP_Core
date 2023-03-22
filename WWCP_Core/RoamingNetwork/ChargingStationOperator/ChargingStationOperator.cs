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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.WWCP.Net.IO.JSON;

using social.OpenData.UsersAPI;
using org.GraphDefined.Vanaheimr.Hermod.Mail;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Charging station extentions.
    /// </summary>
    public static partial class ChargingStationOperatorExtensions
    {

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
    public class ChargingStationOperator : ACryptoEMobilityEntity<ChargingStationOperator_Id,
                                                                  ChargingStationOperatorAdminStatusTypes,
                                                                  ChargingStationOperatorStatusTypes>,
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

        protected static readonly  SemaphoreSlim  ChargingPoolsSemaphore   = new (1, 1);

        protected static readonly  TimeSpan       SemaphoreSlimTimeout     = TimeSpan.FromSeconds(5);

        #endregion

        #region Properties

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


        /// <summary>
        /// All brands registered for this charging station operator.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<Brand>              Brands                           { get; }

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

        private ReactiveSet<OpenDataLicense> _DataLicenses;

        /// <summary>
        /// The license of the charging station operator data.
        /// </summary>
        [Mandatory]
        public ReactiveSet<OpenDataLicense> DataLicenses
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

        private PhoneNumber? _Telephone;

        /// <summary>
        /// The telephone number of the operator's (sales) office.
        /// </summary>
        [Optional]
        public PhoneNumber? Telephone
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

        private SimpleEMailAddress? _EMailAddress;

        /// <summary>
        /// The e-mail address of the operator's (sales) office.
        /// </summary>
        [Optional]
        public SimpleEMailAddress? EMailAddress
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

        private URL? _Homepage;

        /// <summary>
        /// The homepage of this charging station operator.
        /// </summary>
        [Optional]
        public URL? Homepage
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

        private PhoneNumber? _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the Charging Station Operator hotline.
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
                    SetProperty(ref _HotlinePhoneNumber, value);
            }

        }

        #endregion

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
        public ChargingStationOperator(ChargingStationOperator_Id                             Id,
                                       IRoamingNetwork                                        RoamingNetwork,
                                       I18NString?                                            Name                                   = null,
                                       I18NString?                                            Description                            = null,
                                       Action<ChargingStationOperator>?                       Configurator                           = null,
                                       RemoteChargingStationOperatorCreatorDelegate?          RemoteChargingStationOperatorCreator   = null,
                                       Timestamped<ChargingStationOperatorAdminStatusTypes>?  InitialAdminStatus                     = null,
                                       Timestamped<ChargingStationOperatorStatusTypes>?       InitialStatus                          = null,
                                       UInt16                                                 MaxAdminStatusListSize                 = DefaultMaxAdminStatusListSize,
                                       UInt16                                                 MaxStatusListSize                      = DefaultMaxStatusListSize,

                                       JObject?                                               CustomData                             = null,
                                       UserDefinedDictionary?                                 InternalData                           = null)

            : base(Id,
                   RoamingNetwork,
                   Name        ?? I18NString.Create(Languages.en, "Charging Station Operator " + Id.ToString()),
                   Description,
                   null,
                   null,
                   null,
                   InitialAdminStatus,
                   InitialStatus,
                   MaxAdminStatusListSize,
                   MaxStatusListSize,
                   null,
                   null,
                   CustomData,
                   InternalData)

        {

            #region Initial checks

            if (RoamingNetwork is null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The roaming network must not be null!");

            #endregion

            #region Init data and properties

            this.Brands                       = new ReactiveSet<Brand>();
            this._DataLicenses                = new ReactiveSet<OpenDataLicense>();

            #region InvalidEVSEIds

            this.InvalidEVSEIds               = new ReactiveSet<EVSE_Id>();

            InvalidEVSEIds.OnItemAdded += (Timestamp, Set, EVSEId) =>
                OnInvalidEVSEIdAdded?.Invoke(Timestamp, this, EVSEId);

            InvalidEVSEIds.OnItemRemoved += (Timestamp, Set, EVSEId) =>
                OnInvalidEVSEIdRemoved?.Invoke(Timestamp, this, EVSEId);

            #endregion

            this.chargingPools                = new EntityHashSet <IChargingStationOperator, ChargingPool_Id,         IChargingPool>        (this);
            this._ChargingStationGroups       = new EntityHashSet <ChargingStationOperator, ChargingStationGroup_Id, ChargingStationGroup>(this);
            this._EVSEGroups                  = new EntityHashSet <ChargingStationOperator, EVSEGroup_Id,            EVSEGroup>           (this);

            this.chargingTariffs             = new EntityHashSet <ChargingStationOperator, ChargingTariff_Id,       ChargingTariff>      (this);
            this.chargingTariffGroups        = new EntityHashSet <ChargingStationOperator, ChargingTariffGroup_Id,  ChargingTariffGroup> (this);

            //this._ChargingReservations        = new ConcurrentDictionary<ChargingReservation_Id, ChargingPool>();
            //this._ChargingSessions            = new ConcurrentDictionary<ChargingSession_Id,     ChargingPool>();

            #endregion

            #region Init events

            this.ChargingPoolAddition          = new VotingNotificator<DateTime, IChargingStationOperator,    IChargingPool,         Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval           = new VotingNotificator<DateTime, IChargingStationOperator,    IChargingPool,         Boolean>(() => new VetoVote(), true);
            //this.ChargingPoolGroupAddition     = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingPoolGroup,    Boolean>(() => new VetoVote(), true);
            //this.ChargingPoolGroupRemoval      = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingPoolGroup,    Boolean>(() => new VetoVote(), true);

            this.ChargingStationAddition       = new VotingNotificator<DateTime, IChargingPool,               IChargingStation,      Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval        = new VotingNotificator<DateTime, IChargingPool,               IChargingStation,      Boolean>(() => new VetoVote(), true);
            this.ChargingStationGroupAddition  = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingStationGroup, Boolean>(() => new VetoVote(), true);
            this.ChargingStationGroupRemoval   = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingStationGroup, Boolean>(() => new VetoVote(), true);

            this.evseAddition                  = new VotingNotificator<DateTime, IChargingStation,           IEVSE,                Boolean>(() => new VetoVote(), true);
            this.evseRemoval                   = new VotingNotificator<DateTime, IChargingStation,           IEVSE,                Boolean>(() => new VetoVote(), true);
            this.EVSEGroupAddition             = new VotingNotificator<DateTime, ChargingStationOperator,    EVSEGroup,            Boolean>(() => new VetoVote(), true);
            this.EVSEGroupRemoval              = new VotingNotificator<DateTime, ChargingStationOperator,    EVSEGroup,            Boolean>(() => new VetoVote(), true);

            this.chargingTariffAddition        = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingTariff,       Boolean>(() => new VetoVote(), true);
            this.chargingTariffRemoval         = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingTariff,       Boolean>(() => new VetoVote(), true);
            this.chargingTariffGroupAddition   = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingTariffGroup,  Boolean>(() => new VetoVote(), true);
            this.chargingTariffGroupRemoval    = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingTariffGroup,  Boolean>(() => new VetoVote(), true);

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


        #region (internal) UpdateData       (Timestamp, EventTrackingId, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="Sender">The changed Charging Station Operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateData(DateTime          Timestamp,
                                       EventTracking_Id  EventTrackingId,
                                       Object            Sender,
                                       String            PropertyName,
                                       Object?           OldValue,
                                       Object?           NewValue)
        {

            var OnDataChangedLocal = OnDataChanged;
            if (OnDataChangedLocal is not null)
                await OnDataChangedLocal(Timestamp,
                                         EventTrackingId,
                                         Sender as ChargingStationOperator,
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
        internal async Task UpdateAdminStatus(DateTime                                              Timestamp,
                                              EventTracking_Id                                      EventTrackingId,
                                              Timestamped<ChargingStationOperatorAdminStatusTypes>  OldStatus,
                                              Timestamped<ChargingStationOperatorAdminStatusTypes>  NewStatus)
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
        internal async Task UpdateStatus(DateTime                                         Timestamp,
                                         EventTracking_Id                                 EventTrackingId,
                                         Timestamped<ChargingStationOperatorStatusTypes>  OldStatus,
                                         Timestamped<ChargingStationOperatorStatusTypes>  NewStatus)
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

        #region Charging pools

        #region ChargingPoolAddition

        internal readonly IVotingNotificator<DateTime, IChargingStationOperator, IChargingPool, Boolean> ChargingPoolAddition;

        /// <summary>
        /// Called whenever an charging pool will be or was added.
        /// </summary>
        public IVotingSender<DateTime, IChargingStationOperator, IChargingPool, Boolean> OnChargingPoolAddition

            => ChargingPoolAddition;

        #endregion


        #region CreateChargingPool        (Id, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging pool having the given
        /// unique charging pool identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new charging pool.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging pool before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging pool after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging pool failed.</param>
        public async Task<AddChargingPoolResult> CreateChargingPool(ChargingPool_Id?                                             Id                             = null,
                                                                    I18NString?                                                  Name                           = null,
                                                                    I18NString?                                                  Description                    = null,

                                                                    Address?                                                     Address                        = null,
                                                                    GeoCoordinate?                                               GeoLocation                    = null,

                                                                    Action<IChargingPool>?                                       Configurator                   = null,
                                                                    RemoteChargingPoolCreatorDelegate?                           RemoteChargingPoolCreator      = null,
                                                                    Timestamped<ChargingPoolAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                    Timestamped<ChargingPoolStatusTypes>?                        InitialStatus                  = null,
                                                                    UInt16                                                       MaxAdminStatusListSize         = ChargingPool.DefaultMaxAdminStatusScheduleSize,
                                                                    UInt16                                                       MaxStatusListSize              = ChargingPool.DefaultMaxStatusScheduleSize,
                                                                    Action<IChargingPool>?                                       OnSuccess                      = null,
                                                                    Action<IChargingStationOperator, ChargingPool_Id>?           OnError                        = null,
                                                                    Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                    EventTracking_Id?                                            EventTrackingId                = null,
                                                                    User_Id?                                                     CurrentUserId                  = null)
        {

            #region Initial checks

            Id ??= ChargingPool_Id.NewRandom(this.Id);

            // Do not throw an exception when an OnError delegate was given!
            if (chargingPools.ContainsId(Id.Value))
            {

                OnError?.Invoke(this, Id.Value);

                return AddChargingPoolResult.Failed(
                           null, //chargingPool,
                           EventTracking_Id.New,
                           ""
                       );

            }

            AllowInconsistentOperatorIds ??= ((chargingStationOperatorId, chargingPoolId) => false);

            if (this.Id != Id.Value.OperatorId && !AllowInconsistentOperatorIds(this.Id, Id.Value))
                return null;
                //throw new InvalidChargingPoolOperatorId(this,
                //                                        Id.Value.OperatorId);

            #endregion

            var chargingPool = new ChargingPool(Id.Value,
                                                this,
                                                Name,
                                                Description,

                                                Address,
                                                GeoLocation,

                                                Configurator,
                                                RemoteChargingPoolCreator,
                                                InitialAdminStatus,
                                                InitialStatus,
                                                MaxAdminStatusListSize,
                                                MaxStatusListSize);


            if (ChargingPoolAddition.SendVoting(Timestamp.Now, this, chargingPool) &&
                chargingPools.TryAdd(chargingPool))
            {

                chargingPool.OnDataChanged                             += UpdateChargingPoolData;
                chargingPool.OnAdminStatusChanged                      += UpdateChargingPoolAdminStatus;
                chargingPool.OnStatusChanged                           += UpdateChargingPoolStatus;

                chargingPool.OnChargingStationAddition.OnVoting        += (timestamp, evseoperator, pool, vote)    => ChargingStationAddition.SendVoting      (timestamp, evseoperator, pool, vote);
                chargingPool.OnChargingStationAddition.OnNotification  += (timestamp, evseoperator, pool)          => ChargingStationAddition.SendNotification(timestamp, evseoperator, pool);
                chargingPool.OnChargingStationDataChanged              += UpdateChargingStationData;
                chargingPool.OnChargingStationAdminStatusChanged       += UpdateChargingStationAdminStatus;
                chargingPool.OnChargingStationStatusChanged            += UpdateChargingStationStatus;
                chargingPool.OnChargingStationRemoval. OnVoting        += (timestamp, evseoperator, pool, vote)    => ChargingStationRemoval. SendVoting      (timestamp, evseoperator, pool, vote);
                chargingPool.OnChargingStationRemoval. OnNotification  += (timestamp, evseoperator, pool)          => ChargingStationRemoval. SendNotification(timestamp, evseoperator, pool);

                chargingPool.OnEVSEAddition.           OnVoting        += (timestamp, station, evse, vote)         => evseAddition.           SendVoting      (timestamp, station, evse, vote);
                chargingPool.OnEVSEAddition.           OnNotification  += (timestamp, station, evse)               => evseAddition.           SendNotification(timestamp, station, evse);
                chargingPool.OnEVSEDataChanged                         += UpdateEVSEData;
                chargingPool.OnEVSEAdminStatusChanged                  += UpdateEVSEAdminStatus;
                chargingPool.OnEVSEStatusChanged                       += UpdateEVSEStatus;
                chargingPool.OnEVSERemoval.            OnVoting        += (timestamp, station, evse, vote)         => evseRemoval .           SendVoting      (timestamp, station, evse, vote);
                chargingPool.OnEVSERemoval.            OnNotification  += (timestamp, station, evse)               => evseRemoval .           SendNotification(timestamp, station, evse);


                chargingPool.OnNewReservation                          += SendNewReservation;
                chargingPool.OnReservationCanceled                     += SendReservationCanceled;
                chargingPool.OnNewChargingSession                      += SendNewChargingSession;
                chargingPool.OnNewChargeDetailRecord                   += SendNewChargeDetailRecord;

                OnSuccess?.Invoke(chargingPool);
                ChargingPoolAddition.SendNotification(Timestamp.Now, this, chargingPool);

                return AddChargingPoolResult.Success(
                           chargingPool,
                           EventTracking_Id.New,
                           this
                       );

            }

            return AddChargingPoolResult.Failed(
                       chargingPool,
                       EventTracking_Id.New,
                       ""
                   );

        }

        #endregion

        #region CreateOrUpdateChargingPool(Id, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register or udpate a new charging pool having the given
        /// unique charging pool identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new charging pool.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging pool before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging pool after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging pool failed.</param>
        public async Task<AddOrUpdateChargingPoolResult> CreateOrUpdateChargingPool(ChargingPool_Id                                              Id,
                                                                                    I18NString?                                                  Name                           = null,
                                                                                    I18NString?                                                  Description                    = null,

                                                                                    Address?                                                     Address                        = null,
                                                                                    GeoCoordinate?                                               GeoLocation                    = null,

                                                                                    Action<IChargingPool>?                                       Configurator                   = null,
                                                                                    RemoteChargingPoolCreatorDelegate?                           RemoteChargingPoolCreator      = null,
                                                                                    Timestamped<ChargingPoolAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                                    Timestamped<ChargingPoolStatusTypes>?                        InitialStatus                  = null,
                                                                                    UInt16                                                       MaxAdminStatusListSize         = ChargingPool.DefaultMaxAdminStatusScheduleSize,
                                                                                    UInt16                                                       MaxStatusListSize              = ChargingPool.DefaultMaxStatusScheduleSize,
                                                                                    Action<IChargingPool>?                                       OnSuccess                      = null,
                                                                                    Action<IChargingStationOperator, ChargingPool_Id>?           OnError                        = null,
                                                                                    Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                                    EventTracking_Id?                                            EventTrackingId                = null,
                                                                                    User_Id?                                                     CurrentUserId                  = null)
        {

            //lock (chargingPools)
            //{

                #region Initial checks

                AllowInconsistentOperatorIds ??= ((chargingStationOperatorId, chargingPoolId) => false);

                if (this.Id != Id.OperatorId && !AllowInconsistentOperatorIds(this.Id, Id))
                    return null;
                //throw new InvalidChargingPoolOperatorId(this,
                //                                        Id.OperatorId);

                #endregion

                #region If the charging pool identification is new/unknown: Call CreateChargingPool(...)

                if (!chargingPools.ContainsId(Id))
                {

                    var result = await CreateChargingPool(Id,
                                                          Name,
                                                          Description,

                                                          Address,
                                                          GeoLocation,

                                                          Configurator,
                                                          RemoteChargingPoolCreator,
                                                          InitialAdminStatus,
                                                          InitialStatus,
                                                          MaxAdminStatusListSize,
                                                          MaxStatusListSize,
                                                          OnSuccess,
                                                          OnError,
                                                          AllowInconsistentOperatorIds);

                    return AddOrUpdateChargingPoolResult.Success(
                               result.ChargingPool,
                               AddedOrUpdated.Add,
                               EventTracking_Id.New
                           );

                }

                #endregion


                // Merge existing charging pool with new pool data...

                try
                {

                    var existingChargingPool = chargingPools.GetById(Id);

                    if (existingChargingPool is not null)
                    {

                        var result = existingChargingPool.UpdateWith(new ChargingPool(Id,
                                                                                      this,
                                                                                      Name,
                                                                                      Description,

                                                                                      Address,
                                                                                      GeoLocation,

                                                                                      Configurator,
                                                                                      null,
                                                                                      new Timestamped<ChargingPoolAdminStatusTypes>(DateTime.MinValue, ChargingPoolAdminStatusTypes.Operational),
                                                                                      new Timestamped<ChargingPoolStatusTypes>     (DateTime.MinValue, ChargingPoolStatusTypes.     Available)));

                        return AddOrUpdateChargingPoolResult.Success(
                                   result,
                                   AddedOrUpdated.Update,
                                   EventTracking_Id.New
                               );

                    }

                } catch (Exception e)
                {
                    DebugX.Log("CSO.CreateOrUpdateChargingPool(...) failed: " + e.Message);
                }

                return AddOrUpdateChargingPoolResult.Failed(
                           Id,
                           EventTracking_Id.New,
                           ""
                       );

            //}

        }

        #endregion

        #region UpdateChargingPool(ChargingPool, SkipUserUpdatedNotifications = false, OnUpdated = null, EventTrackingId = null, CurrentUserId = null)

        /// <summary>
        /// A delegate called whenever a charging pool was updated.
        /// </summary>
        /// <param name="Timestamp">The timestamp when the charging pool was updated.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldChargingPool">The old charging pool.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentChargingPoolId">An optional charging pool identification initiating this command/request.</param>
        public delegate Task OnChargingPoolUpdatedDelegate(DateTime           Timestamp,
                                                           ChargingPool       ChargingPool,
                                                           ChargingPool       OldChargingPool,
                                                           EventTracking_Id?  EventTrackingId         = null,
                                                           ChargingPool_Id?   CurrentChargingPoolId   = null);

        /// <summary>
        /// An event fired whenever a charging pool was updated.
        /// </summary>
        public event OnChargingPoolUpdatedDelegate OnChargingPoolUpdated;


        #region (protected internal) _UpdateChargingPool(ChargingPool,                 SkipUserUpdatedNotifications = false, OnUpdated = null, ...)

        /// <summary>
        /// Update the given charging pool to/within the API.
        /// </summary>
        /// <param name="User">A charging pool.</param>
        /// <param name="OnUpdated">A delegate run whenever the charging pool has been updated successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional charging pool identification initiating this command/request.</param>
        protected internal async Task<UpdateChargingPoolResult> _UpdateChargingPool(ChargingPool                             ChargingPool,
                                                                                    Boolean                                  SkipUserUpdatedNotifications   = false,
                                                                                    Action<ChargingPool, EventTracking_Id>?  OnUpdated                      = null,
                                                                                    EventTracking_Id?                        EventTrackingId                = null,
                                                                                    User_Id?                                 CurrentUserId                  = null)
        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (!_TryGetChargingPoolById(ChargingPool.Id, out var OldChargingPool))
                return UpdateChargingPoolResult.ArgumentError(ChargingPool,
                                                              eventTrackingId,
                                                              nameof(ChargingPool),
                                                              "The given charging pool '" + ChargingPool.Id + "' does not exists in this API!");

            //if (ChargingPool.API is not null && ChargingPool.API != this)
            //    return UpdateChargingPoolResult.ArgumentError(ChargingPool,
            //                                                  eventTrackingId,
            //                                                  nameof(ChargingPool.API),
            //                                                  "The given charging pool is not attached to this API!");

            //ChargingPool.API = this;


            //await WriteToDatabaseFile(updateChargingPool_MessageType,
            //                          ChargingPool.ToJSON(),
            //                          eventTrackingId,
            //                          CurrentChargingPoolId);

            chargingPools.Remove(OldChargingPool.Id);
            //ChargingPool.CopyAllLinkedDataFrom(OldChargingPool);
            chargingPools.TryAdd(ChargingPool);

            OnUpdated?.Invoke(ChargingPool,
                              eventTrackingId);

            //var OnChargingPoolUpdatedLocal = OnChargingPoolUpdated;
            //if (OnChargingPoolUpdatedLocal is not null)
            //    await OnChargingPoolUpdatedLocal.Invoke(Timestamp.Now,
            //                                            ChargingPool,
            //                                            OldChargingPool,
            //                                            eventTrackingId, 
            //                                            CurrentChargingPoolId);

            //if (!SkipChargingPoolUpdatedNotifications)
            //    await SendNotifications(ChargingPool,
            //                            updateChargingPool_MessageType,
            //                            OldChargingPool,
            //                            eventTrackingId,
            //                            CurrentChargingPoolId);

            return UpdateChargingPoolResult.Success(ChargingPool,
                                                    eventTrackingId);

        }

        #endregion

        #region UpdateChargingPool(ChargingPool, SkipChargingPoolUpdatedNotifications = false, OnUpdated = null, EventTrackingId = null, CurrentUserId = null)

        /// <summary>
        /// Update the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="OnUpdated">A delegate run whenever the charging pool has been updated successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional charging pool identification initiating this command/request.</param>
        public async Task<UpdateChargingPoolResult> UpdateChargingPool(ChargingPool                             ChargingPool,
                                                                       Boolean                                  SkipChargingPoolUpdatedNotifications   = false,
                                                                       Action<ChargingPool, EventTracking_Id>?  OnUpdated                              = null,
                                                                       EventTracking_Id?                        EventTrackingId                        = null,
                                                                       User_Id?                                 CurrentUserId                          = null)
        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (await ChargingPoolsSemaphore.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    return await _UpdateChargingPool(ChargingPool,
                                                     SkipChargingPoolUpdatedNotifications,
                                                     OnUpdated,
                                                     EventTrackingId,
                                                     CurrentUserId);

                }
                catch (Exception e)
                {

                    DebugX.LogException(e);

                    return UpdateChargingPoolResult.Failed(ChargingPool,
                                                           eventTrackingId,
                                                           e);

                }
                finally
                {
                    try
                    {
                        ChargingPoolsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return UpdateChargingPoolResult.Failed(ChargingPool,
                                                   eventTrackingId,
                                                   "Internal locking failed!");

        }

        #endregion


        #region (protected internal) _UpdateChargingPool(ChargingPool, UpdateDelegate, SkipChargingPoolUpdatedNotifications = false, OnUpdated = null, ...)

        /// <summary>
        /// Update the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="UpdateDelegate">A delegate to update the given charging pool.</param>
        /// <param name="OnUpdated">A delegate run whenever the charging pool has been updated successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentChargingPoolId">An optional charging pool identification initiating this command/request.</param>
        protected internal async Task<UpdateChargingPoolResult> _UpdateChargingPool(ChargingPool                             ChargingPool,
                                                                                    Action<ChargingPool.Builder>             UpdateDelegate,
                                                                                    Boolean                                  SkipChargingPoolUpdatedNotifications   = false,
                                                                                    Action<ChargingPool, EventTracking_Id>?  OnUpdated                              = null,
                                                                                    EventTracking_Id?                        EventTrackingId                        = null,
                                                                                    ChargingPool_Id?                         CurrentChargingPoolId                  = null)
        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (!_ChargingPoolExists(ChargingPool.Id))
                return UpdateChargingPoolResult.ArgumentError(ChargingPool,
                                                              eventTrackingId,
                                                              nameof(ChargingPool),
                                                              "The given charging pool '" + ChargingPool.Id + "' does not exists in this API!");

            //if (ChargingPool.API != this)
            //    return UpdateChargingPoolResult.ArgumentError(ChargingPool,
            //                                                  eventTrackingId,
            //                                                  nameof(ChargingPool.API),
            //                                                  "The given charging pool is not attached to this API!");

            if (UpdateDelegate is null)
                return UpdateChargingPoolResult.ArgumentError(ChargingPool,
                                                              eventTrackingId,
                                                              nameof(UpdateDelegate),
                                                              "The given update delegate must not be null!");


            return UpdateChargingPoolResult.Failed(ChargingPool,
                                                   eventTrackingId,
                                                   "Not yet implemented!");



            //var builder = ChargingPool.ToBuilder();
            //UpdateDelegate(builder);
            //var updatedChargingPool = builder.ToImmutable;

            ////await WriteToDatabaseFile(updateChargingPool_MessageType,
            ////                          updatedChargingPool.ToJSON(),
            ////                          eventTrackingId,
            ////                          CurrentChargingPoolId);

            //chargingPools.Remove(ChargingPool.Id);
            ////updatedChargingPool.CopyAllLinkedDataFrom(ChargingPool);
            //chargingPools.TryAdd(updatedChargingPool);

            //OnUpdated?.Invoke(updatedChargingPool,
            //                  eventTrackingId);

            //var OnChargingPoolUpdatedLocal = OnChargingPoolUpdated;
            //if (OnChargingPoolUpdatedLocal is not null)
            //    await OnChargingPoolUpdatedLocal.Invoke(Timestamp.Now,
            //                                            updatedChargingPool,
            //                                            ChargingPool,
            //                                            eventTrackingId,
            //                                            CurrentChargingPoolId);

            ////if (!SkipChargingPoolUpdatedNotifications)
            ////    await SendNotifications(updatedChargingPool,
            ////                            updateChargingPool_MessageType,
            ////                            ChargingPool,
            ////                            eventTrackingId,
            ////                            CurrentChargingPoolId);

            //return UpdateChargingPoolResult.Success(ChargingPool,
            //                                        eventTrackingId);

        }

        #endregion

        #region UpdateChargingPool                      (ChargingPool, UpdateDelegate, SkipChargingPoolUpdatedNotifications = false, OnUpdated = null, ...)

        /// <summary>
        /// Update the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="UpdateDelegate">A delegate to update the given charging pool.</param>
        /// <param name="OnUpdated">A delegate run whenever the charging pool has been updated successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentChargingPoolId">An optional charging pool identification initiating this command/request.</param>
        public async Task<UpdateChargingPoolResult> UpdateChargingPool(ChargingPool                             ChargingPool,
                                                                       Action<ChargingPool.Builder>             UpdateDelegate,
                                                                       Boolean                                  SkipChargingPoolUpdatedNotifications   = false,
                                                                       Action<ChargingPool, EventTracking_Id>?  OnUpdated                              = null,
                                                                       EventTracking_Id?                        EventTrackingId                        = null,
                                                                       ChargingPool_Id?                         CurrentChargingPoolId                  = null)
        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (await ChargingPoolsSemaphore.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    return await _UpdateChargingPool(ChargingPool,
                                                     UpdateDelegate,
                                                     SkipChargingPoolUpdatedNotifications,
                                                     OnUpdated,
                                                     eventTrackingId,
                                                     CurrentChargingPoolId);

                }
                catch (Exception e)
                {

                    DebugX.LogException(e);

                    return UpdateChargingPoolResult.Failed(ChargingPool,
                                                           eventTrackingId,
                                                           e);

                }
                finally
                {
                    try
                    {
                        ChargingPoolsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return UpdateChargingPoolResult.Failed(ChargingPool,
                                                   eventTrackingId,
                                                   "Internal locking failed!");

        }

        #endregion

        #endregion


        #region ChargingPools

        private readonly EntityHashSet<IChargingStationOperator, ChargingPool_Id, IChargingPool> chargingPools;

        /// <summary>
        /// Return an enumeration of all charging pools.
        /// </summary>
        public IEnumerable<IChargingPool> ChargingPools

            => chargingPools;

        #endregion

        #region ChargingPoolIds                (IncludeChargingPools = null)

        /// <summary>
        /// Return an enumeration of all charging pool identifications.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPool_Id> ChargingPoolIds(IncludeChargingPoolDelegate? IncludeChargingPools = null)
        {

            IncludeChargingPools ??= (chargingPool => true);

            return chargingPools.
                       Where (chargingPool => IncludeChargingPools(chargingPool)).
                       Select(chargingPool => chargingPool.Id);

        }

        #endregion

        #region ChargingPoolAdminStatus        (IncludeChargingPools = null)

        /// <summary>
        /// Return an enumeration of all charging pool admin status.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPoolAdminStatus> ChargingPoolAdminStatus(IncludeChargingPoolDelegate? IncludeChargingPools = null)
        {

            IncludeChargingPools ??= (chargingPool => true);

            return chargingPools.
                       Where (chargingPool => IncludeChargingPools(chargingPool)).
                       Select(chargingPool => new ChargingPoolAdminStatus(chargingPool.Id,
                                                                          chargingPool.AdminStatus));
        }

        #endregion

        #region ChargingPoolAdminStatusSchedule(IncludeChargingPools = null, TimestampFilter  = null, StatusFilter = null, HistorySize = null)

        /// <summary>
        /// Return the admin status of all charging pools registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="AdminStatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of admin status entries per pool to skip.</param>
        /// <param name="Take">The number of admin status entries per pool to return.</param>
        public IEnumerable<Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>>>

            ChargingPoolAdminStatusSchedule(IncludeChargingPoolDelegate?                  IncludeChargingPools   = null,
                                            Func<DateTime,                     Boolean>?  TimestampFilter        = null,
                                            Func<ChargingPoolAdminStatusTypes, Boolean>?  AdminStatusFilter      = null,
                                            UInt64?                                       Skip                   = null,
                                            UInt64?                                       Take                   = null)

        {

            IncludeChargingPools ??= (chargingPool => true);

            return chargingPools.
                         Where (chargingPool => IncludeChargingPools(chargingPool)).
                         Select(chargingPool => new Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>>(
                                                    chargingPool.Id,
                                                    chargingPool.AdminStatusSchedule(TimestampFilter,
                                                                                     AdminStatusFilter,
                                                                                     Skip,
                                                                                     Take)));

        }

        #endregion

        #region ChargingPoolStatus             (IncludeChargingPools = null)

        /// <summary>
        /// Return an enumeration of all charging pool status.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPoolStatus> ChargingPoolStatus(IncludeChargingPoolDelegate? IncludeChargingPools = null)
        {

            IncludeChargingPools ??= (chargingPool => true);

            return chargingPools.
                       Where (chargingPool => IncludeChargingPools  (chargingPool)).
                       Select(chargingPool => new ChargingPoolStatus(chargingPool.Id,
                                                                     chargingPool.Status));

        }

        #endregion

        #region ChargingPoolStatusSchedule     (IncludeChargingPools = null, TimestampFilter  = null, StatusFilter = null, HistorySize = null)

        /// <summary>
        /// Return the admin status of all charging pools registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of status entries per pool to skip.</param>
        /// <param name="Take">The number of status entries per pool to return.</param>
        public IEnumerable<Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusTypes>>>>

            ChargingPoolStatusSchedule(IncludeChargingPoolDelegate?             IncludeChargingPools   = null,
                                       Func<DateTime,                Boolean>?  TimestampFilter        = null,
                                       Func<ChargingPoolStatusTypes, Boolean>?  StatusFilter           = null,
                                       UInt64?                                  Skip                   = null,
                                       UInt64?                                  Take                   = null)

        {

            IncludeChargingPools ??= (chargingPool => true);

            return chargingPools.
                         Where (chargingPool => IncludeChargingPools(chargingPool)).
                         Select(chargingPool => new Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusTypes>>>(
                                                    chargingPool.Id,
                                                    chargingPool.StatusSchedule(TimestampFilter,
                                                                                StatusFilter,
                                                                                Skip,
                                                                                Take)));

        }

        #endregion


        #region ChargingPoolExists(ChargingPool)

        /// <summary>
        /// Check if the given ChargingPool is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public Boolean ChargingPoolExists(IChargingPool ChargingPool)

            => chargingPools.Contains(ChargingPool);

        #endregion

        #region ChargingPoolExists(ChargingPoolId)

        /// <summary>
        /// Determines whether the given user identification exists within this API.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of an user.</param>
        protected internal Boolean _ChargingPoolExists(ChargingPool_Id ChargingPoolId)

            => ChargingPoolId.IsNotNullOrEmpty &&
               chargingPools.ContainsId(ChargingPoolId);

        /// <summary>
        /// Determines whether the given user identification exists within this API.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of an user.</param>
        protected internal Boolean _ChargingPoolExists(ChargingPool_Id? ChargingPoolId)

            => ChargingPoolId.HasValue &&
               ChargingPoolId.Value.IsNotNullOrEmpty &&
               chargingPools.ContainsId(ChargingPoolId.Value);


        /// <summary>
        /// Determines whether the given user identification exists within this API.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of an user.</param>
        public Boolean ChargingPoolExists(ChargingPool_Id ChargingPoolId)
        {

            if (ChargingPoolsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _ChargingPoolExists(ChargingPoolId);

                }
                catch
                { }
                finally
                {
                    try
                    {
                        ChargingPoolsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return false;

        }

        /// <summary>
        /// Determines whether the given user identification exists within this API.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of an user.</param>
        public Boolean ChargingPoolExists(ChargingPool_Id? ChargingPoolId)
        {

            if (ChargingPoolsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _ChargingPoolExists(ChargingPoolId);

                }
                catch
                { }
                finally
                {
                    try
                    {
                        ChargingPoolsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return false;

        }

        #endregion

        #region GetChargingPoolById(ChargingPoolId)

        public IChargingPool? GetChargingPoolById(ChargingPool_Id ChargingPoolId)

            => chargingPools.GetById(ChargingPoolId);

        #endregion

        #region TryGetChargingPoolById(ChargingPoolId, out ChargingPool)

        //public Boolean TryGetChargingPoolById(ChargingPool_Id ChargingPoolId, out IChargingPool? ChargingPool)

        //    => chargingPools.TryGet(ChargingPoolId, out ChargingPool);

        /// <summary>
        /// Try to get the charging pool having the given unique identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        protected internal Boolean _TryGetChargingPoolById(ChargingPool_Id ChargingPoolId, out IChargingPool? ChargingPool)
        {

            if (!ChargingPoolId.IsNullOrEmpty &&
                chargingPools.TryGet(ChargingPoolId, out var chargingPool))
            {
                ChargingPool = chargingPool;
                return true;
            }

            ChargingPool = null;
            return false;

        }

        /// <summary>
        /// Try to get the charging pool having the given unique identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        protected internal Boolean _TryGetChargingPoolById(ChargingPool_Id? ChargingPoolId, out IChargingPool? ChargingPool)
        {

            if (ChargingPoolId.IsNotNullOrEmpty() &&
               chargingPools.TryGet(ChargingPoolId!.Value, out var chargingPool))
            {
                ChargingPool = chargingPool;
                return true;
            }

            ChargingPool = null;
            return false;

        }


        /// <summary>
        /// Try to get the charging pool having the given unique identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        public Boolean TryGetChargingPoolById(ChargingPool_Id ChargingPoolId, out IChargingPool? ChargingPool)
        {

            if (ChargingPoolsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _TryGetChargingPoolById(ChargingPoolId, out ChargingPool);

                }
                catch
                { }
                finally
                {
                    try
                    {
                        ChargingPoolsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            ChargingPool = null;
            return false;

        }

        /// <summary>
        /// Try to get the charging pool having the given unique identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        public Boolean TryGetChargingPoolById(ChargingPool_Id? ChargingPoolId, out IChargingPool? ChargingPool)
        {

            if (ChargingPoolsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _TryGetChargingPoolById(ChargingPoolId, out ChargingPool);

                }
                catch
                { }
                finally
                {
                    try
                    {
                        ChargingPoolsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            ChargingPool = null;
            return false;

        }

        #endregion

        #region TryGetChargingPoolByStationId(ChargingStationId, out ChargingPool)

        public Boolean TryGetChargingPoolByStationId(ChargingStation_Id ChargingStationId, out IChargingPool? ChargingPool)
        {

            foreach (var chargingPool in chargingPools)
            {
                if (chargingPool.TryGetChargingStationById(ChargingStationId, out var chargingStation))
                {
                    ChargingPool = chargingPool;
                    return true;
                }
            }

            ChargingPool = null;
            return false;

        }

        #endregion

        #region RemoveChargingPool(ChargingPoolId)

        public async Task<RemoveChargingPoolResult> RemoveChargingPool(ChargingPool_Id ChargingPoolId)
        {

            if (TryGetChargingPoolById(ChargingPoolId, out var chargingPool))
            {

                if (chargingPool is not null &&
                    ChargingPoolRemoval.SendVoting(Timestamp.Now,
                                                   this,
                                                   chargingPool))
                {

                    if (chargingPools.TryRemove(ChargingPoolId, out _))
                    {

                        ChargingPoolRemoval.SendNotification(Timestamp.Now,
                                                             this,
                                                             chargingPool);

                        return RemoveChargingPoolResult.Success(
                                   chargingPool,
                                   EventTracking_Id.New,
                                   this
                               );

                    }

                }

            }

            return RemoveChargingPoolResult.Failed(
                       ChargingPoolId,
                       EventTracking_Id.New,
                       ""
                   );

        }

        #endregion

        #region TryRemoveChargingPool(ChargingPoolId, out ChargingPool)

        public Boolean TryRemoveChargingPool(ChargingPool_Id ChargingPoolId, out IChargingPool? ChargingPool)
        {

            if (TryGetChargingPoolById(ChargingPoolId, out ChargingPool))
            {

                if (ChargingPool is not null &&
                    ChargingPoolRemoval.SendVoting(Timestamp.Now,
                                                   this,
                                                   ChargingPool))
                {

                    if (chargingPools.TryRemove(ChargingPoolId, out _))
                    {

                        ChargingPoolRemoval.SendNotification(Timestamp.Now,
                                                             this,
                                                             ChargingPool);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, NewStatus)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                            ChargingPoolId,
                                               Timestamped<ChargingPoolAdminStatusTypes>  NewStatus,
                                               Boolean                                    SendUpstream = false)
        {

            if (TryGetChargingPoolById(ChargingPoolId, out var chargingPool))
                chargingPool.AdminStatus = NewStatus;

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, NewStatus, Timestamp)

        public void SetChargingPoolAdminStatus(ChargingPool_Id               ChargingPoolId,
                                               ChargingPoolAdminStatusTypes  NewStatus,
                                               DateTime                      Timestamp)
        {

            if (TryGetChargingPoolById(ChargingPoolId, out var chargingPool) &&
                chargingPool is not null)
            {
                chargingPool.AdminStatus = new Timestamped<ChargingPoolAdminStatusTypes>(Timestamp, NewStatus);
            }

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                                        ChargingPoolId,
                                               IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>  StatusList,
                                               ChangeMethods                                          ChangeMethod  = ChangeMethods.Replace)
        {

            if (TryGetChargingPoolById(ChargingPoolId, out var chargingPool) &&
                chargingPool is not null)
            {
                chargingPool.SetAdminStatus(StatusList, ChangeMethod);
            }

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendChargingPoolAdminStatusDiff(new ChargingPoolAdminStatusDiff(Timestamp.Now,
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
        public event OnChargingPoolDataChangedDelegate?         OnChargingPoolDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolStatusChangedDelegate?       OnChargingPoolStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate?  OnChargingPoolAdminStatusChanged;

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
                                                   IChargingPool     ChargingPool,
                                                   String            PropertyName,
                                                   Object?           OldValue,
                                                   Object?           NewValue)
        {

            var OnChargingPoolDataChangedLocal = OnChargingPoolDataChanged;
            if (OnChargingPoolDataChangedLocal is not null)
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
        internal async Task UpdateChargingPoolAdminStatus(DateTime                                   Timestamp,
                                                          EventTracking_Id                           EventTrackingId,
                                                          IChargingPool                              ChargingPool,
                                                          Timestamped<ChargingPoolAdminStatusTypes>  OldStatus,
                                                          Timestamped<ChargingPoolAdminStatusTypes>  NewStatus)
        {

            var OnChargingPoolAdminStatusChangedLocal = OnChargingPoolAdminStatusChanged;
            if (OnChargingPoolAdminStatusChangedLocal is not null)
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
        internal async Task UpdateChargingPoolStatus(DateTime                              Timestamp,
                                                     EventTracking_Id                      EventTrackingId,
                                                     IChargingPool                         ChargingPool,
                                                     Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                                     Timestamped<ChargingPoolStatusTypes>  NewStatus)
        {

            var OnChargingPoolStatusChangedLocal = OnChargingPoolStatusChanged;
            if (OnChargingPoolStatusChangedLocal is not null)
                await OnChargingPoolStatusChangedLocal(Timestamp,
                                                       EventTrackingId,
                                                       ChargingPool,
                                                       OldStatus,
                                                       NewStatus);

        }

        #endregion


        #region IEnumerable<ChargingPool> Members

        IEnumerator IEnumerable.GetEnumerator()

            => chargingPools.GetEnumerator();

        public IEnumerator<IChargingPool> GetEnumerator()

            => chargingPools.GetEnumerator();

        #endregion


        #region ChargingPoolRemoval

        internal readonly IVotingNotificator<DateTime, IChargingStationOperator, IChargingPool, Boolean> ChargingPoolRemoval;

        /// <summary>
        /// Called whenever an charging pool will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, IChargingStationOperator, IChargingPool, Boolean> OnChargingPoolRemoval

            => ChargingPoolRemoval;

        #endregion

        #endregion

        #region Charging stations

        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, IChargingPool, IChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, IChargingPool, IChargingStation, Boolean> OnChargingStationAddition

            => ChargingStationAddition;

        #endregion


        #region ChargingStations

        /// <summary>
        /// Return an enumeration of all charging stations.
        /// </summary>
        public IEnumerable<IChargingStation> ChargingStations

            => chargingPools.SelectMany(chargingPool => chargingPool.ChargingStations);

        #endregion

        #region ChargingStationIds                (IncludeChargingStations = null)

        /// <summary>
        /// Return an enumeration of all charging station identifications.
        /// </summary>
        /// <param name="IncludeChargingStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate? IncludeChargingStations = null)
        {

            IncludeChargingStations ??= (chargingStation => true);

            return ChargingStations.
                       Where (chargingStation => IncludeChargingStations(chargingStation)).
                       Select(chargingStation => chargingStation.Id);

        }

        #endregion

        #region ChargingStationAdminStatus        (IncludeChargingStations = null)

        /// <summary>
        /// Return an enumeration of all charging station admin status.
        /// </summary>
        /// <param name="IncludeChargingStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStationAdminStatus> ChargingStationAdminStatus(IncludeChargingStationDelegate? IncludeChargingStations = null)
        {

            IncludeChargingStations ??= (chargingStation => true);

            return ChargingStations.
                       Where (chargingStation => IncludeChargingStations(chargingStation)).
                       Select(chargingStation => new ChargingStationAdminStatus(chargingStation.Id,
                                                                                chargingStation.AdminStatus));

        }

        #endregion

        #region ChargingStationAdminStatusSchedule(IncludeChargingStations = null, TimestampFilter  = null, StatusFilter = null, HistorySize = null)

        /// <summary>
        /// Return the admin status of all charging stations registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingStations">An optional delegate for filtering charging stations.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="AdminStatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of admin status entries per station to skip.</param>
        /// <param name="Take">The number of admin status entries per station to return.</param>
        public IEnumerable<Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>>>

            ChargingStationAdminStatusSchedule(IncludeChargingStationDelegate?                  IncludeChargingStations   = null,
                                               Func<DateTime,                        Boolean>?  TimestampFilter           = null,
                                               Func<ChargingStationAdminStatusTypes, Boolean>?  AdminStatusFilter         = null,
                                               UInt64?                                          Skip                      = null,
                                               UInt64?                                          Take                      = null)

        {

            IncludeChargingStations ??= (chargingStation => true);

            return ChargingStations.
                         Where (chargingStation => IncludeChargingStations(chargingStation)).
                         Select(chargingStation => new Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>>(
                                                       chargingStation.Id,
                                                       chargingStation.AdminStatusSchedule(TimestampFilter,
                                                                                           AdminStatusFilter,
                                                                                           Skip,
                                                                                           Take)));

        }

        #endregion

        #region ChargingStationStatus             (IncludeChargingStations = null)

        /// <summary>
        /// Return an enumeration of all charging station status.
        /// </summary>
        /// <param name="IncludeChargingStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStationStatus> ChargingStationStatus(IncludeChargingStationDelegate? IncludeChargingStations = null)
        {

            IncludeChargingStations ??= (chargingStation => true);

            return ChargingStations.
                       Where (chargingStation => IncludeChargingStations  (chargingStation)).
                       Select(chargingStation => new ChargingStationStatus(chargingStation.Id,
                                                                           chargingStation.Status));

        }

        #endregion

        #region ChargingStationStatusSchedule     (IncludeChargingStations = null, TimestampFilter  = null, StatusFilter = null, HistorySize = null)

        /// <summary>
        /// Return the admin status of all charging stations registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingStations">An optional delegate for filtering charging stations.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of status entries per station to skip.</param>
        /// <param name="Take">The number of status entries per station to return.</param>
        public IEnumerable<Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusTypes>>>>

            ChargingStationStatusSchedule(IncludeChargingStationDelegate?                  IncludeChargingStations   = null,
                                               Func<DateTime,                   Boolean>?  TimestampFilter           = null,
                                               Func<ChargingStationStatusTypes, Boolean>?  StatusFilter              = null,
                                               UInt64?                                     Skip                      = null,
                                               UInt64?                                     Take                      = null)

        {

            IncludeChargingStations ??= (chargingStation => true);

            return ChargingStations.
                         Where (chargingStation => IncludeChargingStations(chargingStation)).
                         Select(chargingStation => new Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusTypes>>>(
                                                       chargingStation.Id,
                                                       chargingStation.StatusSchedule(TimestampFilter,
                                                                                      StatusFilter,
                                                                                      Skip,
                                                                                      Take)));

        }

        #endregion


        #region ContainsChargingStation      (ChargingStation)

        /// <summary>
        /// Check if the given ChargingStation is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStation(IChargingStation ChargingStation)

            => chargingPools.Any(chargingPool => chargingPool.ContainsChargingStation(ChargingStation.Id));

        #endregion

        #region ContainsChargingStation      (ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)

            => chargingPools.Any(chargingPool => chargingPool.ContainsChargingStation(ChargingStationId));

        #endregion

        #region GetChargingStationById       (ChargingStationId)

        public IChargingStation? GetChargingStationById(ChargingStation_Id ChargingStationId)

            => ChargingStations.FirstOrDefault(chargingStation => chargingStation.Id == ChargingStationId);

        #endregion

        #region TryGetChargingStationById    (ChargingStationId, out ChargingStation ChargingStation)

        public Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out IChargingStation? ChargingStation)
        {

            ChargingStation = ChargingStations.FirstOrDefault(chargingStation => chargingStation.Id == ChargingStationId);

            return ChargingStation is not null;

        }

        #endregion



        #region SetChargingStationAdminStatus(ChargingStationId, NewAdminStatus)

        public void SetChargingStationAdminStatus(ChargingStation_Id               ChargingStationId,
                                                  ChargingStationAdminStatusTypes  NewAdminStatus)
        {

            if (TryGetChargingStationById(ChargingStationId, out var chargingStation) &&
                chargingStation is not null)
            {
                chargingStation.AdminStatus = NewAdminStatus;
            }

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, NewTimestampedAdminStatus)

        public void SetChargingStationAdminStatus(ChargingStation_Id                            ChargingStationId,
                                                  Timestamped<ChargingStationAdminStatusTypes>  NewTimestampedAdminStatus)
        {

            if (TryGetChargingStationById(ChargingStationId, out var chargingStation) &&
                chargingStation is not null)
            {
                chargingStation.AdminStatus = NewTimestampedAdminStatus;
            }

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, NewAdminStatus, Timestamp)

        public void SetChargingStationAdminStatus(ChargingStation_Id               ChargingStationId,
                                                  ChargingStationAdminStatusTypes  NewAdminStatus,
                                                  DateTime                         Timestamp)
        {

            if (TryGetChargingStationById(ChargingStationId, out var chargingStation) &&
                chargingStation is not null)
            {
                chargingStation.AdminStatus = new Timestamped<ChargingStationAdminStatusTypes>(Timestamp, NewAdminStatus);
            }

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetChargingStationAdminStatus(ChargingStation_Id                                         ChargingStationId,
                                                  IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>  AdminStatusList,
                                                  ChangeMethods                                              ChangeMethod  = ChangeMethods.Replace)
        {

            if (TryGetChargingStationById(ChargingStationId, out var chargingStation) &&
                chargingStation is not null)
            {
                chargingStation.SetAdminStatus(AdminStatusList, ChangeMethod);
            }

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendChargingStationAdminStatusDiff(new ChargingStationAdminStatusDiff(Timestamp.Now,
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


        #region SetChargingStationStatus(ChargingStationId, NewStatus)

        public void SetChargingStationStatus(ChargingStation_Id          ChargingStationId,
                                             ChargingStationStatusTypes  NewStatus)
        {

            if (TryGetChargingStationById(ChargingStationId, out var chargingStation) &&
                chargingStation is not null)
            {
                chargingStation.Status = NewStatus;
            }

        }

        #endregion

        #region SetChargingStationStatus(ChargingStationId, NewTimestampedStatus)

        public void SetChargingStationStatus(ChargingStation_Id                       ChargingStationId,
                                             Timestamped<ChargingStationStatusTypes>  NewTimestampedStatus)
        {

            if (TryGetChargingStationById(ChargingStationId, out var chargingStation) &&
                chargingStation is not null)
            {
                chargingStation.Status = NewTimestampedStatus;
            }

        }

        #endregion

        #region SetChargingStationStatus(ChargingStationId, NewStatus, Timestamp)

        public void SetChargingStationStatus(ChargingStation_Id          ChargingStationId,
                                             ChargingStationStatusTypes  NewStatus,
                                             DateTime                    Timestamp)
        {

            if (TryGetChargingStationById(ChargingStationId, out var chargingStation) &&
                chargingStation is not null)
            {
                chargingStation.Status = new Timestamped<ChargingStationStatusTypes>(Timestamp, NewStatus);
            }

        }

        #endregion

        #region SetChargingStationStatus(ChargingStationId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetChargingStationStatus(ChargingStation_Id                                    ChargingStationId,
                                             IEnumerable<Timestamped<ChargingStationStatusTypes>>  StatusList,
                                             ChangeMethods                                         ChangeMethod  = ChangeMethods.Replace)
        {

            if (TryGetChargingStationById(ChargingStationId, out var chargingStation) &&
                chargingStation is not null)
            {
                chargingStation.SetStatus(StatusList, ChangeMethod);
            }

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendChargingStationStatusDiff(new ChargingStationStatusDiff(Timestamp.Now,
            //                                               ChargingStationOperatorId:    Id,
            //                                               ChargingStationOperatorName:  Name,
            //                                               NewStatus:         new List<KeyValuePair<ChargingStation_Id, ChargingStationStatusType>>(),
            //                                               ChangedStatus:     new List<KeyValuePair<ChargingStation_Id, ChargingStationStatusType>>() {
            //                                                                          new KeyValuePair<ChargingStation_Id, ChargingStationStatusType>(ChargingStationId, NewStatus.Value)
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
                                                      IChargingStation  ChargingStation,
                                                      String            PropertyName,
                                                      Object?           OldValue,
                                                      Object?           NewValue)
        {

            var OnChargingStationDataChangedLocal = OnChargingStationDataChanged;
            if (OnChargingStationDataChangedLocal is not null)
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
                                                             IChargingStation                              ChargingStation,
                                                             Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                                             Timestamped<ChargingStationAdminStatusTypes>  NewStatus)
        {

            var OnChargingStationAdminStatusChangedLocal = OnChargingStationAdminStatusChanged;
            if (OnChargingStationAdminStatusChangedLocal is not null)
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
                                                        IChargingStation                         ChargingStation,
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

        }

        #endregion


        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, IChargingPool, IChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, IChargingPool, IChargingStation, Boolean> OnChargingStationRemoval

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
                                                               IEnumerable<OpenDataLicense>                                            DataLicenses                  = null,

                                                               IEnumerable<IChargingStation>                                       Members                       = null,
                                                               IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                               Func<IChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                                               Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                               UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                               UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                               Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                               Action<IChargingStationOperator, ChargingStationGroup_Id>           OnError                       = null)

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


                if (ChargingStationGroupAddition.SendVoting(Timestamp.Now, this, _ChargingStationGroup) &&
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

                    ChargingStationGroupAddition.SendNotification(Timestamp.Now,
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

                                                               IEnumerable<IChargingStation>                                       Members                       = null,
                                                               IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                               Func<IChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                                               Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                               UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                               UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                               Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                               Action<IChargingStationOperator, ChargingStationGroup_Id>           OnError                       = null)

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

                                                                    IEnumerable<IChargingStation>                                       Members                       = null,
                                                                    IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                                    Func<IChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                                                    Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                                    UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                                    UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                                    Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                                    Action<IChargingStationOperator, ChargingStationGroup_Id>           OnError                       = null)

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
        public ChargingStationGroup GetOrCreateChargingStationGroup(String                                                              IdSuffix,
                                                                    I18NString                                                          Name,
                                                                    I18NString                                                          Description                   = null,

                                                                    IEnumerable<IChargingStation>                                       Members                       = null,
                                                                    IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                                    Func<IChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                                                    Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                                    UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                                    UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                                    Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                                    Action<IChargingStationOperator, ChargingStationGroup_Id>           OnError                       = null)


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
                    ChargingStationGroupRemoval.SendVoting(Timestamp.Now,
                                                           this,
                                                           ChargingStationGroup) &&
                    _ChargingStationGroups.TryRemove(ChargingStationGroupId, out ChargingStationGroup _ChargingStationGroup))
                {

                    OnSuccess?.Invoke(this, ChargingStationGroup);

                    ChargingStationGroupRemoval.SendNotification(Timestamp.Now,
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

                if (ChargingStationGroupRemoval.SendVoting(Timestamp.Now,
                                                           this,
                                                           ChargingStationGroup) &&
                    _ChargingStationGroups.TryRemove(ChargingStationGroup.Id, out ChargingStationGroup _ChargingStationGroup))
                {

                    OnSuccess?.Invoke(this, _ChargingStationGroup);

                    ChargingStationGroupRemoval.SendNotification(Timestamp.Now,
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

        internal readonly IVotingNotificator<DateTime, IChargingStation, IEVSE, Boolean> evseAddition;

        public IVotingNotificator<DateTime, IChargingStation, IEVSE, Boolean> EVSEAddition
            => evseAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, IChargingStation, IEVSE, Boolean> OnEVSEAddition

            => evseAddition;

        #endregion

        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, IChargingStation, IEVSE, Boolean> evseRemoval;

        public IVotingNotificator<DateTime, IChargingStation, IEVSE, Boolean> EVSERemoval
            => evseRemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, IChargingStation, IEVSE, Boolean> OnEVSERemoval

            => evseRemoval;

        #endregion


        #region EVSEs

        /// <summary>
        /// Return an enumeration of all EVSEs.
        /// </summary>
        public IEnumerable<IEVSE> EVSEs

            => chargingPools.SelectMany(chargingPool => chargingPool.EVSEs);

        #endregion

        #region EVSEIds                (IncludeEVSEs = null)

        /// <summary>
        /// Return an enumeration of all EVSE identifications.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate? IncludeEVSEs = null)
        {

            IncludeEVSEs ??= (evse => true);

            return EVSEs.
                       Where (evse => IncludeEVSEs(evse)).
                       Select(evse => evse.Id);

        }

        #endregion

        #region EVSEAdminStatus        (IncludeEVSEs = null)

        /// <summary>
        /// Return an enumeration of all EVSE admin status.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate? IncludeEVSEs = null)
        {

            IncludeEVSEs ??= (evse => true);

            return EVSEs.
                       Where (evse => IncludeEVSEs(evse)).
                       Select(evse => new EVSEAdminStatus(evse.Id,
                                                          evse.AdminStatus));
        }

        #endregion

        #region EVSEAdminStatusSchedule(IncludeEVSEs = null, TimestampFilter  = null, StatusFilter = null, HistorySize = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="AdminStatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of admin status entries per pool to skip.</param>
        /// <param name="Take">The number of admin status entries per pool to return.</param>
        public IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>>

            EVSEAdminStatusSchedule(IncludeEVSEDelegate?                  IncludeEVSEs        = null,
                                    Func<DateTime,             Boolean>?  TimestampFilter     = null,
                                    Func<EVSEAdminStatusTypes, Boolean>?  AdminStatusFilter   = null,
                                    UInt64?                               Skip                = null,
                                    UInt64?                               Take                = null)

        {

            IncludeEVSEs ??= (evse => true);

            return EVSEs.
                       Where (evse => IncludeEVSEs(evse)).
                       Select(evse => new Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>(
                                          evse.Id,
                                          evse.AdminStatusSchedule(TimestampFilter,
                                                                   AdminStatusFilter,
                                                                   Skip,
                                                                   Take)));

        }

        #endregion

        #region EVSEStatus             (IncludeEVSEs = null)

        /// <summary>
        /// Return an enumeration of all EVSE status.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate? IncludeEVSEs = null)
        {

            IncludeEVSEs ??= (evse => true);

            return EVSEs.
                       Where (evse => IncludeEVSEs  (evse)).
                       Select(evse => new EVSEStatus(evse.Id,
                                                     evse.Status));

        }

        #endregion

        #region EVSEStatusSchedule     (IncludeEVSEs = null, TimestampFilter  = null, StatusFilter = null, HistorySize = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of status entries per pool to skip.</param>
        /// <param name="Take">The number of status entries per pool to return.</param>
        public IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>>

            EVSEStatusSchedule(IncludeEVSEDelegate?             IncludeEVSEs      = null,
                               Func<DateTime,        Boolean>?  TimestampFilter   = null,
                               Func<EVSEStatusTypes, Boolean>?  StatusFilter      = null,
                               UInt64?                          Skip              = null,
                               UInt64?                          Take              = null)

        {

            IncludeEVSEs ??= (evse => true);

            return EVSEs.
                       Where (evse => IncludeEVSEs(evse)).
                       Select(evse => new Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>(
                                          evse.Id,
                                          evse.StatusSchedule(TimestampFilter,
                                                              StatusFilter,
                                                              Skip,
                                                              Take)));

        }

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)

            => chargingPools.Any(chargingPool => chargingPool.ContainsEVSE(EVSE.Id));

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)

            => chargingPools.Any(chargingPool => chargingPool.ContainsEVSE(EVSEId));

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id? EVSEId)
        {

            if (!EVSEId.HasValue)
                return false;

            return chargingPools.Any(chargingPool => chargingPool.ContainsEVSE(EVSEId.Value));

        }

        #endregion

        #region GetEVSEById(EVSEId)

        public IEVSE? GetEVSEById(EVSE_Id EVSEId)

            => EVSEs.FirstOrDefault(evse => evse.Id == EVSEId);

        #endregion

        #region TryGetEVSEById(EVSEId, out EVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE? EVSE)
        {

            EVSE = EVSEs.FirstOrDefault(evse => evse.Id == EVSEId);

            return EVSE is not null;

        }

        public Boolean TryGetEVSEById(EVSE_Id? EVSEId, out IEVSE? EVSE)
        {

            if (!EVSEId.HasValue)
            {
                EVSE = null;
                return false;
            }

            EVSE = EVSEs.FirstOrDefault(evse => evse.Id == EVSEId);

            return EVSE is not null;

        }

        #endregion

        #region TryGetChargingStationByEVSEId(EVSEId, out Station)

        public Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out IChargingStation? Station)
        {

            foreach (var station in ChargingStations)
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

        #region TryGetChargingPoolByEVSEId(EVSEId, out ChargingPool)

        public Boolean TryGetChargingPoolByEVSEId(EVSE_Id EVSEId, out IChargingPool? ChargingPool)
        {

            foreach (var chargingPool in chargingPools)
            {
                foreach (var chargingStation in chargingPool.ChargingStations)
                {
                    if (chargingStation.TryGetEVSEById(EVSEId, out _))
                    {
                        ChargingPool = chargingPool;
                        return true;
                    }
                }
            }

            ChargingPool = null;
            return false;

        }

        #endregion

        #region InvalidEVSEIds

        /// <summary>
        /// A list of invalid EVSE Ids.
        /// </summary>
        public ReactiveSet<EVSE_Id> InvalidEVSEIds { get; }

        #endregion


        #region SetEVSEAdminStatus(NewAdminStatus)

        public void SetEVSEAdminStatus(EVSEAdminStatus NewAdminStatus)
        {

            if (TryGetEVSEById(NewAdminStatus.Id, out var evse) &&
                evse is not null)
            {
                evse.AdminStatus = NewAdminStatus.TimestampedAdminStatus;
            }

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, NewAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id EVSEId,
                                       EVSEAdminStatusTypes NewAdminStatus)
        {

            if (TryGetEVSEById(EVSEId, out var evse) &&
                evse is not null)
            {
                evse.AdminStatus = NewAdminStatus;
            }

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, NewTimestampedAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id                            EVSEId,
                                       Timestamped<EVSEAdminStatusTypes>  NewTimestampedAdminStatus)
        {

            if (TryGetEVSEById(EVSEId, out var evse) &&
                evse is not null)
            {
                evse.AdminStatus = NewTimestampedAdminStatus;
            }

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, NewAdminStatus, Timestamp)

        public void SetEVSEAdminStatus(EVSE_Id               EVSEId,
                                       EVSEAdminStatusTypes  NewAdminStatus,
                                       DateTime              Timestamp)
        {

            if (TryGetEVSEById(EVSEId, out var evse) &&
                evse is not null)
            {
                evse.AdminStatus = new Timestamped<EVSEAdminStatusTypes>(Timestamp, NewAdminStatus);
            }

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, AdminStatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEVSEAdminStatus(EVSE_Id                                         EVSEId,
                                       IEnumerable<Timestamped<EVSEAdminStatusTypes>>  AdminStatusList,
                                       ChangeMethods                                   ChangeMethod  = ChangeMethods.Replace)
        {

            if (InvalidEVSEIds.Contains(EVSEId))
                return;

            if (TryGetEVSEById(EVSEId, out var evse) &&
                evse is not null)
            {
                evse.SetAdminStatus(AdminStatusList, ChangeMethod);
            }

        }

        #endregion

        #region ApplyEVSEAdminStatusDiff(EVSEAdminStatusDiff)

        public EVSEAdminStatusDiff ApplyEVSEAdminStatusDiff(EVSEAdminStatusDiff EVSEAdminStatusDiff)
        {

            #region Initial checks

            if (EVSEAdminStatusDiff is null)
                throw new ArgumentNullException(nameof(EVSEAdminStatusDiff),  "The given EVSE admin status diff must not be null!");

            #endregion

            foreach (var status in EVSEAdminStatusDiff.NewStatus)
                SetEVSEAdminStatus(status.Key, status.Value);

            foreach (var status in EVSEAdminStatusDiff.ChangedStatus)
                SetEVSEAdminStatus(status.Key, status.Value);

            return EVSEAdminStatusDiff;

        }

        #endregion


        #region SetEVSEStatus(NewStatus)

        public void SetEVSEStatus(EVSEStatus  NewStatus)
        {

            if (TryGetEVSEById(NewStatus.Id, out var evse) &&
                evse is not null)
            {
                evse.Status = NewStatus.Status;
            }

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewStatus)

        public void SetEVSEStatus(EVSE_Id          EVSEId,
                                  EVSEStatusTypes  NewStatus)
        {

            if (TryGetEVSEById(EVSEId, out var evse) &&
                evse is not null)
            {
                evse.Status = NewStatus;
            }

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewTimestampedStatus)

        public void SetEVSEStatus(EVSE_Id                       EVSEId,
                                  Timestamped<EVSEStatusTypes>  NewTimestampedStatus)
        {

            if (TryGetEVSEById(EVSEId, out var evse) &&
                evse is not null)
            {
                evse.Status = NewTimestampedStatus;
            }

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewStatus, Timestamp)

        public void SetEVSEStatus(EVSE_Id          EVSEId,
                                  EVSEStatusTypes  NewStatus,
                                  DateTime         Timestamp)
        {

            if (TryGetEVSEById(EVSEId, out var evse) &&
                evse is not null)
            {
                evse.Status = new Timestamped<EVSEStatusTypes>(Timestamp, NewStatus);
            }

        }

        #endregion

        #region SetEVSEStatus(EVSEId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEVSEStatus(EVSE_Id                                    EVSEId,
                                  IEnumerable<Timestamped<EVSEStatusTypes>>  StatusList,
                                  ChangeMethods                              ChangeMethod  = ChangeMethods.Replace)
        {

            if (InvalidEVSEIds.Contains(EVSEId))
                return;

            if (TryGetEVSEById(EVSEId, out var evse) &&
                evse is not null)
            {
                evse.SetStatus(StatusList, ChangeMethod);
            }

        }

        #endregion

        #region CalcEVSEStatusDiff(EVSEStatus, IncludeEVSE = null)

        //public EVSEStatusDiff CalcEVSEStatusDiff(Dictionary<EVSE_Id, EVSEStatusTypes>  EVSEStatus,
        //                                         Func<EVSE, Boolean>                  IncludeEVSE  = null)
        //{

        //    if (EVSEStatus == null || EVSEStatus.Count == 0)
        //        return new EVSEStatusDiff(Timestamp.Now, Id, Name);

        //    #region Get data...

        //    var EVSEStatusDiff     = new EVSEStatusDiff(Timestamp.Now, Id, Name);

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
        //    return new EVSEStatusDiff(Timestamp.Now, Id, Name);

        //}

        #endregion

        #region ApplyEVSEStatusDiff(EVSEStatusDiff)

        public EVSEStatusDiff ApplyEVSEStatusDiff(EVSEStatusDiff EVSEStatusDiff)
        {

            #region Initial checks

            if (EVSEStatusDiff is null)
                throw new ArgumentNullException(nameof(EVSEStatusDiff),  "The given EVSE status diff must not be null!");

            #endregion

            foreach (var status in EVSEStatusDiff.NewStatus)
                SetEVSEStatus(status.Key, status.Value);

            foreach (var status in EVSEStatusDiff.ChangedStatus)
                SetEVSEStatus(status.Key, status.Value);

            return EVSEStatusDiff;

        }

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


        #region (internal) UpdateEVSEData       (Timestamp, EventTrackingId, EVSE, NewValue,  OldValue  = null)

        /// <summary>
        /// Update the static data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The optional old value of the changed property.</param>
        internal async Task UpdateEVSEData(DateTime          Timestamp,
                                           EventTracking_Id  EventTrackingId,
                                           IEVSE             EVSE,
                                           String            PropertyName,
                                           Object?           NewValue,
                                           Object?           OldValue   = null)
        {

            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal is not null)
                await OnEVSEDataChangedLocal(Timestamp,
                                             EventTrackingId ?? EventTracking_Id.New,
                                             EVSE,
                                             PropertyName,
                                             NewValue,
                                             OldValue);

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EventTrackingId, EVSE, NewStatus, OldStatus = null)

        /// <summary>
        /// Update the current admin status of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        /// <param name="OldStatus">The optional old EVSE status.</param>
        internal async Task UpdateEVSEAdminStatus(DateTime                            Timestamp,
                                                  EventTracking_Id                    EventTrackingId,
                                                  IEVSE                               EVSE,
                                                  Timestamped<EVSEAdminStatusTypes>   NewStatus,
                                                  Timestamped<EVSEAdminStatusTypes>?  OldStatus   = null)
        {

            var OnEVSEAdminStatusChangedLocal = OnEVSEAdminStatusChanged;
            if (OnEVSEAdminStatusChangedLocal is not null)
                await OnEVSEAdminStatusChangedLocal(Timestamp,
                                                    EventTrackingId ?? EventTracking_Id.New,
                                                    EVSE,
                                                    NewStatus,
                                                    OldStatus);

        }

        #endregion

        #region (internal) UpdateEVSEStatus     (Timestamp, EventTrackingId, EVSE, NewStatus, OldStatus = null)

        /// <summary>
        /// Update the current status of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        /// <param name="OldStatus">The optional old EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                       Timestamp,
                                             EventTracking_Id               EventTrackingId,
                                             IEVSE                          EVSE,
                                             Timestamped<EVSEStatusTypes>   NewStatus,
                                             Timestamped<EVSEStatusTypes>?  OldStatus   = null)
        {

            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal is not null)
                await OnEVSEStatusChangedLocal(Timestamp,
                                               EventTrackingId ?? EventTracking_Id.New,
                                               EVSE,
                                               NewStatus,
                                               OldStatus);

        }

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
                                         IEnumerable<OpenDataLicense>                       DataLicenses                  = null,

                                         IEnumerable<EVSE>                              Members                       = null,
                                         IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                         Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                         Func<IEVSE,   Boolean>                         AutoIncludeEVSEs              = null,

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


                if (EVSEGroupAddition.SendVoting(Timestamp.Now, this, _EVSEGroup) &&
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

                    EVSEGroupAddition.SendNotification(Timestamp.Now,
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
                                         IEnumerable<OpenDataLicense>                       DataLicenses                  = null,

                                         IEnumerable<EVSE>                              Members                       = null,
                                         IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                         Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                         Func<IEVSE,   Boolean>                         AutoIncludeEVSEs              = null,

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
                                              IEnumerable<OpenDataLicense>                       DataLicenses                  = null,

                                              IEnumerable<EVSE>                              Members                       = null,
                                              IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                              Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                              Func<IEVSE,   Boolean>                         AutoIncludeEVSEs              = null,

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
                                              IEnumerable<OpenDataLicense>                       DataLicenses                  = null,

                                              IEnumerable<EVSE>                              Members                       = null,
                                              IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                              Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                              Func<IEVSE,   Boolean>                         AutoIncludeEVSEs              = null,

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
        public Boolean TryGetEVSEGroup(EVSEGroup_Id    Id,
                                       out EVSEGroup?  EVSEGroup)

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
                    EVSEGroupRemoval.SendVoting(Timestamp.Now,
                                                           this,
                                                           EVSEGroup) &&
                    _EVSEGroups.TryRemove(EVSEGroupId, out EVSEGroup _EVSEGroup))
                {

                    OnSuccess?.Invoke(this, EVSEGroup);

                    EVSEGroupRemoval.SendNotification(Timestamp.Now,
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

                if (EVSEGroupRemoval.SendVoting(Timestamp.Now,
                                                           this,
                                                           EVSEGroup) &&
                    _EVSEGroups.TryRemove(EVSEGroup.Id, out EVSEGroup _EVSEGroup))
                {

                    OnSuccess?.Invoke(this, _EVSEGroup);

                    EVSEGroupRemoval.SendNotification(Timestamp.Now,
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

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariff, Boolean> chargingTariffAddition;

        public IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariff, Boolean> ChargingTariffAddition
            => chargingTariffAddition;

        /// <summary>
        /// Called whenever a charging tariff will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingTariff, Boolean> OnChargingTariffAddition
            => chargingTariffAddition;

        #endregion


        #region ChargingTariffs

        private readonly EntityHashSet<ChargingStationOperator, ChargingTariff_Id, ChargingTariff> chargingTariffs;

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        public IEnumerable<ChargingTariff> ChargingTariffs
            => chargingTariffs;

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
        public ChargingTariff? CreateChargingTariff(ChargingTariff_Id                                    Id,
                                                    I18NString                                           Name,
                                                    I18NString                                           Description,
                                                    IEnumerable<ChargingTariffElement>                   TariffElements,
                                                    Currency                                             Currency,
                                                    Brand                                                Brand,
                                                    URL                                                  TariffURL,
                                                    EnergyMix                                            EnergyMix,

                                                    Action<ChargingTariff>?                              OnSuccess   = null,
                                                    Action<ChargingStationOperator, ChargingTariff_Id>?  OnError     = null)

        {

            lock (chargingTariffs)
            {

                #region Initial checks

                if (chargingTariffs.ContainsId(Id))
                {

                    if (OnError != null)
                        OnError?.Invoke(this, Id);

                    throw new ChargingTariffAlreadyExists(this, Id);

                }

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the charging station group must not be null or empty!");

                #endregion

                var _ChargingTariff = new ChargingTariff(Id,
                                                         Name,
                                                         Description,
                                                         TariffElements,
                                                         Currency,
                                                         Brand,
                                                         TariffURL,
                                                         EnergyMix);


                if (chargingTariffAddition.SendVoting(Timestamp.Now, this, _ChargingTariff) &&
                    chargingTariffs.TryAdd(_ChargingTariff))
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

                    chargingTariffAddition.SendNotification(Timestamp.Now,
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
        public ChargingTariff? CreateChargingTariff(String                                                         Id,
                                                    I18NString                                                     Name,
                                                    I18NString                                                     Description,
                                                    IEnumerable<ChargingTariffElement>                             TariffElements,
                                                    Currency                                                       Currency,
                                                    Brand                                                          Brand,
                                                    URL                                                            TariffURL,
                                                    EnergyMix                                                      EnergyMix,

                                                    Action<ChargingTariff>?                                        OnSuccess   = null,
                                                    Action<ChargingStationOperator, ChargingTariff_Id>?            OnError     = null)

        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given suffix of the unique identification of the new charging tariff must not be null or empty!");

            #endregion

            return CreateChargingTariff(ChargingTariff_Id.Parse(Id),
                                        Name,
                                        Description,
                                        TariffElements,
                                        Currency,
                                        Brand,
                                        TariffURL,
                                        EnergyMix,

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
        public ChargingTariff? GetOrCreateChargingTariff(ChargingTariff_Id                                    Id,
                                                         I18NString                                           Name,
                                                         I18NString                                           Description,
                                                         IEnumerable<ChargingTariffElement>                   TariffElements,
                                                         Currency                                             Currency,
                                                         Brand                                                Brand,
                                                         URL                                                  TariffURL,
                                                         EnergyMix                                            EnergyMix,

                                                         Action<ChargingTariff>?                              OnSuccess   = null,
                                                         Action<ChargingStationOperator, ChargingTariff_Id>?  OnError     = null)

        {

            lock (chargingTariffs)
            {

                #region Initial checks

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the charging tariff must not be null or empty!");

                #endregion

                if (chargingTariffs.TryGet(Id, out var chargingTariff))
                    return chargingTariff;

                return CreateChargingTariff(Id,
                                            Name,
                                            Description,
                                            TariffElements,
                                            Currency,
                                            Brand,
                                            TariffURL,
                                            EnergyMix,

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
        public ChargingTariff? GetOrCreateChargingTariff(String                                               IdSuffix,
                                                         I18NString                                           Name,
                                                         I18NString                                           Description,
                                                         IEnumerable<ChargingTariffElement>                   TariffElements,
                                                         Currency                                             Currency,
                                                         Brand                                                Brand,
                                                         URL                                                  TariffURL,
                                                         EnergyMix                                            EnergyMix,

                                                         Action<ChargingTariff>?                              OnSuccess   = null,
                                                         Action<ChargingStationOperator, ChargingTariff_Id>?  OnError     = null)


        {

            #region Initial checks

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix), "The given suffix of the unique identification of the new charging tariff must not be null or empty!");

            #endregion

            return GetOrCreateChargingTariff(ChargingTariff_Id.Parse(Id, IdSuffix.Trim()),
                                             Name,
                                             Description,
                                             TariffElements,
                                             Currency,
                                             Brand,
                                             TariffURL,
                                             EnergyMix,

                                             OnSuccess,
                                             OnError);

        }

        #endregion


        #region GetChargingTariff(Id)

        /// <summary>
        /// Return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        public ChargingTariff? GetChargingTariff(ChargingTariff_Id Id)
        {

            if (chargingTariffs.TryGet(Id, out var tariff))
                return tariff;

            return null;

        }

        #endregion

        #region TryGetChargingTariff(Id, out ChargingTariff)

        /// <summary>
        /// Try to return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="ChargingTariff">The charing tariff.</param>
        public Boolean TryGetChargingTariff(ChargingTariff_Id    Id,
                                            out ChargingTariff?  ChargingTariff)

            => chargingTariffs.TryGet(Id, out ChargingTariff);

        #endregion


        #region ChargingTariffRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariff, Boolean> chargingTariffRemoval;

        public IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariff, Boolean> ChargingTariffRemoval
            => chargingTariffRemoval;

        /// <summary>
        /// Called whenever a charging tariff will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingTariff, Boolean> OnChargingTariffRemoval
            => chargingTariffRemoval;

        #endregion

        #region RemoveChargingTariff(ChargingTariffId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariffId">The unique identification of the charging tariff to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging tariff failed.</param>
        public ChargingTariff? RemoveChargingTariff(ChargingTariff_Id                                     ChargingTariffId,
                                                    Action<IChargingStationOperator, ChargingTariff>?     OnSuccess   = null,
                                                    Action<IChargingStationOperator, ChargingTariff_Id>?  OnError     = null)
        {

            lock (chargingTariffs)
            {

                if (chargingTariffs.TryGet(ChargingTariffId, out var chargingTariff)      &&
                    chargingTariff is not null                                            &&
                    chargingTariffRemoval.SendVoting(Timestamp.Now, this, chargingTariff) &&
                    chargingTariffs.TryRemove(ChargingTariffId, out _))
                {

                    OnSuccess?.Invoke(this, chargingTariff);

                    chargingTariffRemoval.SendNotification(Timestamp.Now,
                                                           this,
                                                           chargingTariff);

                    return chargingTariff;

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
        public ChargingTariff? RemoveChargingTariff(ChargingTariff                                     ChargingTariff,
                                                    Action<IChargingStationOperator, ChargingTariff>?  OnSuccess   = null,
                                                    Action<IChargingStationOperator, ChargingTariff>?  OnError     = null)
        {

            lock (chargingTariffs)
            {

                if (chargingTariffRemoval.SendVoting(Timestamp.Now, this, ChargingTariff) &&
                    chargingTariffs.TryRemove(ChargingTariff.Id, out var chargingTariff)  &&
                    chargingTariff is not null)
                {

                    OnSuccess?.Invoke(this, chargingTariff);

                    chargingTariffRemoval.SendNotification(Timestamp.Now,
                                                           this,
                                                           chargingTariff);

                    return chargingTariff;

                }

                OnError?.Invoke(this, ChargingTariff);

                return ChargingTariff;

            }

        }

        #endregion

        #endregion

        #region ChargingTariffGroups

        #region ChargingTariffGroupAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> chargingTariffGroupAddition;

        public IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> ChargingTariffGroupAddition
            => chargingTariffGroupAddition;

        /// <summary>
        /// Called whenever a charging tariff will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> OnChargingTariffGroupAddition

            => chargingTariffGroupAddition;

        #endregion

        #region ChargingTariffGroups

        private readonly EntityHashSet<ChargingStationOperator, ChargingTariffGroup_Id, ChargingTariffGroup> chargingTariffGroups;

        /// <summary>
        /// All charging tariff groups registered within this charging station operator.
        /// </summary>
        public IEnumerable<ChargingTariffGroup> ChargingTariffGroups
            => chargingTariffGroups;

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
        public ChargingTariffGroup? CreateChargingTariffGroup(String                                                     IdSuffix,
                                                              I18NString                                                 Description,
                                                              Action<ChargingTariffGroup>?                               OnSuccess   = null,
                                                              Action<IChargingStationOperator, ChargingTariffGroup_Id>?  OnError     = null)

        {

            lock (chargingTariffGroups)
            {

                #region Initial checks

                var newGroupId = ChargingTariffGroup_Id.Parse(Id, IdSuffix);

                if (chargingTariffGroups.ContainsId(newGroupId))
                {

                    if (OnError != null)
                        OnError?.Invoke(this, newGroupId);

                    throw new ChargingTariffGroupAlreadyExists(this, newGroupId);

                }

                #endregion

                var chargingTariffGroup = new ChargingTariffGroup(newGroupId,
                                                                  this,
                                                                  Description);


                if (chargingTariffGroupAddition.SendVoting(Timestamp.Now, this, chargingTariffGroup) &&
                    chargingTariffGroups.TryAdd(chargingTariffGroup))
                {

                    //_ChargingTariffGroup.OnEVSEDataChanged                             += UpdateEVSEData;
                    //_ChargingTariffGroup.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                    //_ChargingTariffGroup.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;

                    //_ChargingTariffGroup.OnChargingStationDataChanged                  += UpdateChargingStationData;
                    //_ChargingTariffGroup.OnChargingStationStatusChanged                += UpdateChargingStationStatus;
                    //_ChargingTariffGroup.OnChargingStationAdminStatusChanged           += UpdateChargingStationAdminStatus;

                    ////_ChargingTariffGroup.OnDataChanged                                 += UpdateChargingTariffGroupData;
                    ////_ChargingTariffGroup.OnAdminStatusChanged                          += UpdateChargingTariffGroupAdminStatus;

                    OnSuccess?.Invoke(chargingTariffGroup);

                    chargingTariffGroupAddition.SendNotification(Timestamp.Now,
                                                                 this,
                                                                 chargingTariffGroup);

                    return chargingTariffGroup;

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
        public ChargingTariffGroup? GetOrCreateChargingTariffGroup(String                                                     IdSuffix,
                                                                   I18NString                                                 Description,
                                                                   Action<ChargingTariffGroup>?                               OnSuccess   = null,
                                                                   Action<IChargingStationOperator, ChargingTariffGroup_Id>?  OnError     = null)

        {

            lock (chargingTariffGroups)
            {

                if (chargingTariffGroups.TryGet(ChargingTariffGroup_Id.Parse(Id, IdSuffix), out var chargingTariffGroup))
                    return chargingTariffGroup;

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
        public ChargingTariffGroup? GetChargingTariffGroup(ChargingTariffGroup_Id Id)
        {

            if (chargingTariffGroups.TryGet(Id, out var chargingTariffGroup))
                return chargingTariffGroup;

            return null;

        }

        #endregion

        #region TryGetChargingTariffGroup(Id, out ChargingTariffGroup)

        /// <summary>
        /// Try to return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="ChargingTariffGroup">The charing tariff.</param>
        public Boolean TryGetChargingTariffGroup(ChargingTariffGroup_Id   Id,
                                                 out ChargingTariffGroup? ChargingTariffGroup)

            => chargingTariffGroups.TryGet(Id, out ChargingTariffGroup);

        #endregion


        #region ChargingTariffGroupRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> chargingTariffGroupRemoval;

        public IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> ChargingTariffGroupRemoval
            => chargingTariffGroupRemoval;

        /// <summary>
        /// Called whenever a charging tariff group will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> OnChargingTariffGroupRemoval

            => chargingTariffGroupRemoval;

        #endregion

        #region RemoveChargingTariffGroup(ChargingTariffGroupId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariffGroupId">The unique identification of the charging tariff to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging tariff failed.</param>
        public ChargingTariffGroup? RemoveChargingTariffGroup(ChargingTariffGroup_Id                                     ChargingTariffGroupId,
                                                              Action<IChargingStationOperator, ChargingTariffGroup>?     OnSuccess   = null,
                                                              Action<IChargingStationOperator, ChargingTariffGroup_Id>?  OnError     = null)
        {

            lock (chargingTariffGroups)
            {

                if (chargingTariffGroups.TryGet(ChargingTariffGroupId, out var chargingTariffGroup) &&
                    chargingTariffGroup is not null                                                 &&
                    chargingTariffGroupRemoval.SendVoting(Timestamp.Now, this, chargingTariffGroup) &&
                    chargingTariffGroups.TryRemove(ChargingTariffGroupId, out _))
                {

                    OnSuccess?.Invoke(this, chargingTariffGroup);

                    chargingTariffGroupRemoval.SendNotification(Timestamp.Now,
                                                                this,
                                                                chargingTariffGroup);

                    return chargingTariffGroup;

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
        public ChargingTariffGroup? RemoveChargingTariffGroup(ChargingTariffGroup                                     ChargingTariffGroup,
                                                              Action<IChargingStationOperator, ChargingTariffGroup>?  OnSuccess   = null,
                                                              Action<IChargingStationOperator, ChargingTariffGroup>?  OnError     = null)
        {

            lock (chargingTariffGroups)
            {

                if (chargingTariffGroupRemoval.SendVoting(Timestamp.Now, this, ChargingTariffGroup) &&
                    chargingTariffGroups.TryRemove(ChargingTariffGroup.Id, out var chargingTariffGroup) &&
                    chargingTariffGroup is not null)
                {

                    OnSuccess?.Invoke(this, chargingTariffGroup);

                    chargingTariffGroupRemoval.SendNotification(Timestamp.Now,
                                                                this,
                                                                chargingTariffGroup);

                    return chargingTariffGroup;

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

        public Boolean TryGetChargingReservationById(ChargingReservation_Id Id, out ChargingReservation? ChargingReservation)
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
        /// Reserve the possibility to charge at this charging station operator.
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


                => Reserve(ChargingLocation.FromChargingStationOperatorId(Id),
                           ChargingReservationLevel.ChargingStationOperator,
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

                              DateTime?                              Timestamp           = null,
                              CancellationToken?                     CancellationToken   = null,
                              EventTracking_Id?                      EventTrackingId     = null,
                              TimeSpan?                              RequestTimeout      = null)

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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.InternalUse)
                {

                    if (RemoteChargingStationOperator is not null)
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

        public TimeSpan MaxReservationDuration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.InternalUse)
                {

                    if ((ChargingLocation.EVSEId.           HasValue && TryGetChargingPoolByEVSEId   (ChargingLocation.EVSEId.           Value, out var chargingPool) ||
                         ChargingLocation.ChargingStationId.HasValue && TryGetChargingPoolByStationId(ChargingLocation.ChargingStationId.Value, out     chargingPool) ||
                         ChargingLocation.ChargingPoolId.   HasValue && TryGetChargingPoolById       (ChargingLocation.ChargingPoolId.   Value, out     chargingPool)) &&
                         chargingPool is not null)
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
                            if (result.Session is not null)
                                result.Session.ChargingStationOperator = this;

                        }

                        #endregion

                    }
                    else
                        result = RemoteStartResult.UnknownLocation();

                }
                else
                    result = RemoteStartResult.OutOfService();

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


            RemoteStopResult result        = null;
            ChargingPool    _ChargingPool  = null;

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
                DebugX.LogException(e, nameof(ChargingStationOperator) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.InternalUse)
                {

                    if (TryGetChargingSessionById(SessionId, out ChargingSession chargingSession) &&
                       ((chargingSession.EVSEId.           HasValue && TryGetChargingPoolByEVSEId   (chargingSession.EVSEId.           Value, out var chargingPool)) ||
                        (chargingSession.ChargingStationId.HasValue && TryGetChargingPoolByStationId(chargingSession.ChargingStationId.Value, out     chargingPool)) ||
                        (chargingSession.ChargingPoolId.   HasValue && TryGetChargingPoolById       (chargingSession.ChargingPoolId.   Value, out     chargingPool))))
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
                        //    OnNewChargeDetailRecord?.Invoke(Timestamp.Now,
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
        public JObject ToJSON(Boolean                                                    Embedded                                  = false,
                              InfoStatus                                                 ExpandRoamingNetworkId                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                                 ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                              InfoStatus                                                 ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                              InfoStatus                                                 ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                              InfoStatus                                                 ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                              InfoStatus                                                 ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<ChargingStationOperator>?  CustomChargingStationOperatorSerializer   = null,
                              CustomJObjectSerializerDelegate<ChargingPool>?             CustomChargingPoolSerializer              = null,
                              CustomJObjectSerializerDelegate<ChargingStation>?          CustomChargingStationSerializer           = null,
                              CustomJObjectSerializerDelegate<EVSE>?                     CustomEVSESerializer                      = null)
        {

            try
            {

                var json = JSONObject.Create(

                                     new JProperty("@id",                 Id.ToString()),

                               !Embedded
                                   ? new JProperty("@context",            JSONLDContext)
                                   : null,

                                     new JProperty("name",                Name.       ToJSON()),

                               Description.IsNeitherNullNorEmpty()
                                   ? new JProperty("description",         Description.ToJSON())
                                   : null,

                               DataSource.IsNotNullOrEmpty()
                                   ? new JProperty("dataSource",          DataSource)
                                   : null,

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

                               Address is not null
                                   ? new JProperty("address",             Address.ToJSON(Embedded: true))
                                   : null,

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

                               Homepage.HasValue
                                   ? new JProperty("homepage",            Homepage.ToString())
                                   : null,

                               HotlinePhoneNumber.HasValue
                                   ? new JProperty("hotline",             HotlinePhoneNumber.ToString())
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
                                                             new JArray(Brands.                 Select (brand   => brand.Id).
                                                                                                OrderBy(brandId => brandId).
                                                                                                Select (brandId => brandId.ToString()))),

                                         () => new JProperty("brands",
                                                             new JArray(Brands.
                                                                                                OrderBy(brand => brand).
                                                                                                ToJSON (Embedded:                         true,
                                                                                                        ExpandDataLicenses:               InfoStatus.ShowIdOnly))))
                                   : null

                               );

                return CustomChargingStationOperatorSerializer != null
                           ? CustomChargingStationOperatorSerializer(this, json)
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


        #region IComparable<ChargingStationOperator> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is ChargingStationOperator chargingStationOperator
                   ? CompareTo(chargingStationOperator)
                   : throw new ArgumentException("The given object is not a charging station operator!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationOperator)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator object to compare with.</param>
        public Int32 CompareTo(ChargingStationOperator? ChargingStationOperator)
        {

            if (ChargingStationOperator is null)
                throw new ArgumentNullException("The given Charging Station Operator must not be null!");

            return Id.CompareTo(ChargingStationOperator.Id);

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
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationOperator chargingStationOperator &&
                   Equals(chargingStationOperator);

        #endregion

        #region Equals(Operator)

        /// <summary>
        /// Compares two Charging Station Operators for equality.
        /// </summary>
        /// <param name="Operator">An Charging Station Operator to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationOperator? ChargingStationOperator)

            => ChargingStationOperator is not null &&
               Id.Equals(ChargingStationOperator.Id);

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
