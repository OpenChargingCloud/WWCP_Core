/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Mail;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Charging station extensions.
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
        public const String  JSONLDContext                                               = "https://open.charging.cloud/contexts/wwcp+json/chargingStationOperator";

        /// <summary>
        /// The default max size of the charging station operator admin status list.
        /// </summary>
        public const UInt16  DefaultMaxChargingStationOperatorAdminStatusScheduleSize    = 15;

        /// <summary>
        /// The default max size of the charging station operator (aggregated charging station) status list.
        /// </summary>
        public const UInt16  DefaultMaxChargingStationOperatorStatusScheduleSize         = 15;

        #endregion

        #region Properties

        /// <summary>
        /// The remote charging station operator.
        /// </summary>
        [InternalUseOnly]
        public IRemoteChargingStationOperator?  RemoteChargingStationOperator    { get; }

        /// <summary>
        /// The roaming provider of this charging station operator.
        /// </summary>
        [InternalUseOnly]
        public IEMPRoamingProvider?             EMPRoamingProvider               { get; }


        #region Logo

        private URL? logo;

        /// <summary>
        /// The logo of this evse operator.
        /// </summary>
        [Optional]
        public URL? Logo
        {

            get
            {
                return logo;
            }

            set
            {
                if (logo != value)
                    SetProperty(ref logo, value);
            }

        }

        #endregion

        #region Brands

        /// <summary>
        /// All brands registered for this charging station operator.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<Brand>               Brands                           { get; } = new();

        #endregion

        #region Address

        private Address? address;

        /// <summary>
        /// The address of the operators headquarter.
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
                SetProperty(ref address, value);
            }

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate? geoLocation;

        /// <summary>
        /// The geographical location of this operator.
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
                SetProperty(ref geoLocation, value);
            }

        }

        #endregion

        #region Telephone

        private PhoneNumber? telephone;

        /// <summary>
        /// The telephone number of the operator's (sales) office.
        /// </summary>
        [Optional]
        public PhoneNumber? Telephone
        {

            get
            {
                return telephone;
            }

            set
            {
                SetProperty(ref telephone, value);
            }

        }

        #endregion

        #region EMailAddress

        private SimpleEMailAddress? eMailAddress;

        /// <summary>
        /// The e-mail address of the operator's (sales) office.
        /// </summary>
        [Optional]
        public SimpleEMailAddress? EMailAddress
        {

            get
            {
                return eMailAddress;
            }

            set
            {
                SetProperty(ref eMailAddress, value);
            }

        }

        #endregion

        #region Homepage

        private URL? homepage;

        /// <summary>
        /// The homepage of this charging station operator.
        /// </summary>
        [Optional]
        public URL? Homepage
        {

            get
            {
                return homepage;
            }

            set
            {
                SetProperty(ref homepage, value);
            }

        }

        #endregion

        #region HotlinePhoneNumber

        private PhoneNumber? hotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the Charging Station Operator hotline.
        /// </summary>
        [Optional]
        public PhoneNumber? HotlinePhoneNumber
        {

            get
            {
                return hotlinePhoneNumber;
            }

            set
            {
                SetProperty(ref hotlinePhoneNumber, value);
            }

        }

        #endregion

        #region TermsAndConditionsURL

        private URL? termsAndConditionsURL;

        /// <summary>
        /// The optional URL to terms and conditions for charging.
        /// </summary>
        [Optional]
        public URL? TermsAndConditionsURL
        {

            get
            {
                return termsAndConditionsURL;
            }

            set
            {
                SetProperty(ref termsAndConditionsURL, value);
            }

        }

        #endregion

        #region DataLicenses

        private readonly ReactiveSet<OpenDataLicense> dataLicenses = new();

        /// <summary>
        /// The license(s) of the charging station operator data.
        /// </summary>
        [Optional]
        public ReactiveSet<OpenDataLicense> DataLicenses

            => dataLicenses.Any()
                   ? dataLicenses
                   : RoamingNetwork?.DataLicenses ?? dataLicenses;

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
                                       UInt16?                                                MaxAdminStatusScheduleSize             = DefaultMaxAdminStatusScheduleSize,
                                       UInt16?                                                MaxStatusScheduleSize                  = DefaultMaxStatusScheduleSize,

                                       JObject?                                               CustomData                             = null,
                                       UserDefinedDictionary?                                 InternalData                           = null)

            : base(Id,
                   RoamingNetwork,
                   Name ?? I18NString.Create("Charging Station Operator " + Id.ToString()),
                   Description,
                   null,
                   null,
                   null,
                   InitialAdminStatus         ?? ChargingStationOperatorAdminStatusTypes.Operational,
                   InitialStatus              ?? ChargingStationOperatorStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxChargingStationOperatorAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxChargingStationOperatorStatusScheduleSize,
                   null,
                   null,
                   CustomData,
                   InternalData)

        {

            #region Init data and properties

            this.chargingPools               = new EntityHashSet <IChargingStationOperator, ChargingPool_Id,         IChargingPool>        (this,
                                                                                                                                            new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool,                    Boolean>(() => new VetoVote(), true),
                                                                                                                                            new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool,                    Boolean>(() => new VetoVote(), true),
                                                                                                                                            new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool,   IChargingPool,   Boolean>(() => new VetoVote(), true),
                                                                                                                                            new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool,   IChargingPool,   Boolean>(() => new VetoVote(), true),
                                                                                                                                            new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool,                    Boolean>(() => new VetoVote(), true));

            this.chargingTariffs             = new EntityHashSet <IChargingStationOperator, ChargingTariff_Id,      IChargingTariff>       (this,
                                                                                                                                            new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff,                  Boolean>(() => new VetoVote(), true),
                                                                                                                                            new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff,                  Boolean>(() => new VetoVote(), true),
                                                                                                                                            new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, IChargingTariff, Boolean>(() => new VetoVote(), true),
                                                                                                                                            new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, IChargingTariff, Boolean>(() => new VetoVote(), true),
                                                                                                                                            new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff,                  Boolean>(() => new VetoVote(), true));

            this.evseGroups                  = new EntityHashSet <ChargingStationOperator,  EVSEGroup_Id,            EVSEGroup>           (this);
            this.chargingStationGroups       = new EntityHashSet <ChargingStationOperator,  ChargingStationGroup_Id, ChargingStationGroup>(this);
            this.chargingTariffGroups        = new EntityHashSet <ChargingStationOperator,  ChargingTariffGroup_Id,  ChargingTariffGroup> (this);

            #endregion

            #region Init events

            this.ChargingStationAddition       = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingPool,               IChargingStation,                   Boolean>(() => new VetoVote(), true);
            this.ChargingStationUpdate         = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingPool,               IChargingStation, IChargingStation, Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval        = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingPool,               IChargingStation,                   Boolean>(() => new VetoVote(), true);
            this.ChargingStationGroupAddition  = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingStationGroup, Boolean>(() => new VetoVote(), true);
            this.ChargingStationGroupRemoval   = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingStationGroup, Boolean>(() => new VetoVote(), true);

            this.evseAddition                  = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation,            IEVSE,                              Boolean>(() => new VetoVote(), true);
            this.evseUpdate                    = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation,            IEVSE,            IEVSE,            Boolean>(() => new VetoVote(), true);
            this.evseRemoval                   = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation,            IEVSE,                              Boolean>(() => new VetoVote(), true);
            this.evseGroupAddition             = new VotingNotificator<DateTime, ChargingStationOperator,    EVSEGroup,            Boolean>(() => new VetoVote(), true);
            this.evseGroupRemoval              = new VotingNotificator<DateTime, ChargingStationOperator,    EVSEGroup,            Boolean>(() => new VetoVote(), true);

            this.chargingTariffAddition        = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator,    IChargingTariff,                    Boolean>(() => new VetoVote(), true);
            this.chargingTariffUpdate          = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator,    IChargingTariff,  IChargingTariff,  Boolean>(() => new VetoVote(), true);
            this.chargingTariffRemoval         = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator,    IChargingTariff,                    Boolean>(() => new VetoVote(), true);
            this.chargingTariffGroupAddition   = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingTariffGroup,  Boolean>(() => new VetoVote(), true);
            this.chargingTariffGroupRemoval    = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingTariffGroup,  Boolean>(() => new VetoVote(), true);

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
        public event OnChargingStationOperatorDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnChargingStationOperatorAdminStatusChangedDelegate?  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnChargingStationOperatorStatusChangedDelegate?       OnStatusChanged;

        #endregion


        #region (internal) UpdateData       (Timestamp, EventTrackingId, Sender, PropertyName, NewValue, OldValue, DataSource)

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

            var onDataChanged = OnDataChanged;
            if (onDataChanged is not null)
                await onDataChanged(Timestamp,
                                         EventTrackingId,
                                         Sender as ChargingStationOperator,
                                         PropertyName,
                                         OldValue,
                                         NewValue);

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, NewStatus, OldStatus, DataSource)

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

            var onAdminStatusChanged = OnAdminStatusChanged;
            if (onAdminStatusChanged is not null)
                await onAdminStatusChanged(Timestamp,
                                                EventTrackingId,
                                                this,
                                                OldStatus,
                                                NewStatus);

        }

        #endregion

        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, NewStatus, OldStatus, DataSource)

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

            var onStatusChanged = OnStatusChanged;
            if (onStatusChanged is not null)
                await onStatusChanged(Timestamp,
                                           EventTrackingId,
                                           this,
                                           OldStatus,
                                           NewStatus);

        }

        #endregion

        #endregion

        #region Charging Pools

        #region Data

        private readonly EntityHashSet<IChargingStationOperator, ChargingPool_Id, IChargingPool> chargingPools;

        /// <summary>
        /// Return an enumeration of all charging pools.
        /// </summary>
        public IEnumerable<IChargingPool> ChargingPools

            => chargingPools;

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


        #region ChargingPoolAddition

        /// <summary>
        /// Called whenever an charging pool will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, Boolean> OnChargingPoolAddition

            => chargingPools.OnAddition;

        #endregion

        #region ChargingPoolUpdate

        /// <summary>
        /// Called whenever an charging pool will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, IChargingPool, Boolean> OnChargingPoolUpdate

            => chargingPools.OnUpdate;


        ///// <summary>
        ///// A delegate called whenever a charging pool was updated.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp when the charging pool was updated.</param>
        ///// <param name="ChargingPool">The updated charging pool.</param>
        ///// <param name="OldChargingPool">The old charging pool.</param>
        ///// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        ///// <param name="CurrentChargingPoolId">An optional charging pool identification initiating this command/request.</param>
        //public delegate Task OnChargingPoolUpdatedDelegate(DateTime           Timestamp,
        //                                                   ChargingPool       ChargingPool,
        //                                                   ChargingPool       OldChargingPool,
        //                                                   EventTracking_Id?  EventTrackingId         = null,
        //                                                   ChargingPool_Id?   CurrentChargingPoolId   = null);

        ///// <summary>
        ///// An event fired whenever a charging pool was updated.
        ///// </summary>
        //public event OnChargingPoolUpdatedDelegate OnChargingPoolUpdated;

        #endregion

        #region ChargingPoolRemoval

        /// <summary>
        /// Called whenever an charging pool will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, Boolean> OnChargingPoolRemoval

            => chargingPools.OnRemoval;

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


        #region Connect(ChargingPool)

        private void Connect(IChargingPool ChargingPool)
        {

            ChargingPool.OnDataChanged                             += UpdateChargingPoolData;
            ChargingPool.OnAdminStatusChanged                      += UpdateChargingPoolAdminStatus;
            ChargingPool.OnStatusChanged                           += UpdateChargingPoolStatus;

            ChargingPool.OnChargingStationAddition.OnVoting        += (timestamp, eventTrackingId, userId, chargingPool, chargingStation, vote) => ChargingStationAddition.SendVoting(timestamp, eventTrackingId, userId, chargingPool, chargingStation, vote);
            ChargingPool.OnChargingStationAddition.OnNotification  += (timestamp, eventTrackingId, userId, chargingPool, chargingStation)       => {
                chargingStationLookup.TryAdd(chargingStation.Id, chargingStation);
                ChargingStationAddition.SendNotification(timestamp, eventTrackingId, userId, chargingPool, chargingStation);
            };
            ChargingPool.OnChargingStationDataChanged              += UpdateChargingStationData;
            ChargingPool.OnChargingStationAdminStatusChanged       += UpdateChargingStationAdminStatus;
            ChargingPool.OnChargingStationStatusChanged            += UpdateChargingStationStatus;
            ChargingPool.OnChargingStationRemoval. OnVoting        += (timestamp, eventTrackingId, userId, chargingPool, chargingStation, vote) => ChargingStationRemoval. SendVoting(timestamp, eventTrackingId, userId, chargingPool, chargingStation, vote);
            ChargingPool.OnChargingStationRemoval. OnNotification  += (timestamp, eventTrackingId, userId, chargingPool, chargingStation)       => {
                chargingStationLookup.TryRemove(chargingStation.Id, out _);
                ChargingStationRemoval.SendNotification(timestamp, eventTrackingId, userId, chargingPool, chargingStation);
            };

            ChargingPool.OnEVSEAddition.           OnVoting        += (timestamp, eventTrackingId, userId, station, evse, vote) => evseAddition.SendVoting(timestamp, eventTrackingId, userId, station, evse, vote);
            ChargingPool.OnEVSEAddition.           OnNotification  += (timestamp, eventTrackingId, userId, station, evse)       => {
                evseLookup.TryAdd(evse.Id, evse);
                evseAddition.SendNotification(timestamp, eventTrackingId, userId, station, evse);
            };
            ChargingPool.OnEVSEDataChanged                         += UpdateEVSEData;
            ChargingPool.OnEVSEAdminStatusChanged                  += UpdateEVSEAdminStatus;
            ChargingPool.OnEVSEStatusChanged                       += UpdateEVSEStatus;
            ChargingPool.OnEVSERemoval.            OnVoting        += (timestamp, eventTrackingId, userId, station, evse, vote) => evseRemoval .SendVoting(timestamp, eventTrackingId, userId, station, evse, vote);
            ChargingPool.OnEVSERemoval.            OnNotification  += (timestamp, eventTrackingId, userId, station, evse)       => {
                evseLookup.TryRemove(evse.Id, out _);
                evseRemoval.SendNotification(timestamp, eventTrackingId, userId, station, evse);
            };


            ChargingPool.OnNewReservation                          += SendNewReservation;
            ChargingPool.OnReservationCanceled                     += SendReservationCanceled;
            ChargingPool.OnNewChargingSession                      += SendNewChargingSession;
            ChargingPool.OnNewChargeDetailRecord                   += SendNewChargeDetailRecord;

        }

        #endregion

        #region AddChargingPool           (ChargingPool,                             OnSuccess       = null, OnError = null, ...)

        /// <summary>
        /// Add a new charging pool.
        /// </summary>
        /// <param name="ChargingPool">A new charging pool.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging pool failed.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddChargingPoolResult> AddChargingPool(IChargingPool                                                       ChargingPool,

                                                                 Action<IChargingPool,                           EventTracking_Id>?  OnSuccess                      = null,
                                                                 Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                        = null,

                                                                 Boolean                                                             SkipAddedNotifications         = false,
                                                                 Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                                 EventTracking_Id?                                                   EventTrackingId                = null,
                                                                 User_Id?                                                            CurrentUserId                  = null)
        {

            #region Initial checks

            EventTrackingId              ??= EventTracking_Id.New;
            AllowInconsistentOperatorIds ??= ((chargingStationOperatorId, chargingPoolId) => false);

            if (ChargingPool.Id.OperatorId != this.Id && !AllowInconsistentOperatorIds(this.Id, ChargingPool.Id))
                return AddChargingPoolResult.ArgumentError(
                           ChargingPool,
                           $"The operator identification of the given charging pool '{ChargingPool.Id.OperatorId}' is invalid!".ToI18NString(),
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            #endregion


            if (chargingPools.TryAdd(ChargingPool,
                                     Connect,
                                     EventTrackingId,
                                     CurrentUserId).Result == CommandResult.Success)
            {

                //ToDo: Persistency
                await Task.Delay(1);

                OnSuccess?.Invoke(ChargingPool,
                                  EventTrackingId);

                return AddChargingPoolResult.Success(
                           ChargingPool,
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            }

            OnError?.Invoke(this,
                            ChargingPool,
                            EventTrackingId);

            return AddChargingPoolResult.Error(
                       ChargingPool,
                       "Could not add the given charging pool!".ToI18NString(),
                       EventTrackingId,
                       Id,
                       this,
                       this
                   );

        }

        #endregion

        #region AddChargingPoolIfNotExists(ChargingPool,                             OnSuccess       = null,                 ...)

        /// <summary>
        /// Add a new charging pool, but do not fail when this charging pool already exists.
        /// </summary>
        /// <param name="ChargingPool">A new charging pool.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging pool.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddChargingPoolResult> AddChargingPoolIfNotExists(IChargingPool                                                ChargingPool,

                                                                            Action<IChargingPool, EventTracking_Id>?                     OnSuccess                      = null,

                                                                            Boolean                                                      SkipAddedNotifications         = false,
                                                                            Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                            EventTracking_Id?                                            EventTrackingId                = null,
                                                                            User_Id?                                                     CurrentUserId                  = null)
        {

            #region Initial checks

            EventTrackingId              ??= EventTracking_Id.New;
            AllowInconsistentOperatorIds ??= ((chargingStationOperatorId, chargingPoolId) => false);

            if (ChargingPool.Id.OperatorId != Id && !AllowInconsistentOperatorIds(Id, ChargingPool.Id))
                return AddChargingPoolResult.ArgumentError(
                           ChargingPool,
                           $"The operator identification of the given charging pool '{ChargingPool.Id.OperatorId}' is invalid!".ToI18NString(),
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            #endregion

            if (chargingPools.TryAdd(ChargingPool,
                                     Connect,
                                     EventTrackingId,
                                     CurrentUserId).Result == CommandResult.Success)
            {

                //ToDo: Persistency
                await Task.Delay(1);

                OnSuccess?.Invoke(ChargingPool,
                                  EventTrackingId);

                return AddChargingPoolResult.Success(
                           ChargingPool,
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            }

            return AddChargingPoolResult.NoOperation(
                       ChargingPool,
                       EventTrackingId,
                       Id,
                       this,
                       this
                   );

        }

        #endregion

        #region AddOrUpdateChargingPool   (ChargingPool,   OnAdditionSuccess = null, OnUpdateSuccess = null, OnError = null, ...)

        /// <summary>
        /// Add a new or update an existing charging pool.
        /// </summary>
        /// <param name="ChargingPool">A new or updated charging pool.</param>
        /// 
        /// <param name="OnAdditionSuccess">An optional delegate to be called after the successful addition of the charging pool.</param>
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging pool failed.</param>
        /// 
        /// <param name="SkipAddOrUpdatedUpdatedNotifications">Whether to skip sending the 'OnAddedOrUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddOrUpdateChargingPoolResult> AddOrUpdateChargingPool(IChargingPool                                                       ChargingPool,

                                                                                 Action<IChargingPool,                           EventTracking_Id>?  OnAdditionSuccess                      = null,
                                                                                 Action<IChargingPool,            IChargingPool, EventTracking_Id>?  OnUpdateSuccess                        = null,
                                                                                 Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                                = null,

                                                                                 Boolean                                                             SkipAddOrUpdatedUpdatedNotifications   = false,
                                                                                 Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds           = null,
                                                                                 EventTracking_Id?                                                   EventTrackingId                        = null,
                                                                                 User_Id?                                                            CurrentUserId                          = null)
        {

            #region Initial checks

            EventTrackingId              ??= EventTracking_Id.New;
            AllowInconsistentOperatorIds ??= ((chargingStationOperatorId, chargingPoolId) => false);

            if (ChargingPool.Id.OperatorId != this.Id && !AllowInconsistentOperatorIds(this.Id, ChargingPool.Id))
                return AddOrUpdateChargingPoolResult.ArgumentError(
                           ChargingPool,
                           $"The operator identification of the given charging pool '{ChargingPool.Id.OperatorId}' is invalid!".ToI18NString(),
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            #endregion


            if (chargingPools.TryGet(ChargingPool.Id, out var existingChargingPool) &&
                existingChargingPool is not null)
            {

                var xx1 = existingChargingPool.Equals(ChargingPool);

                var xx2 = existingChargingPool == ChargingPool; //FalseFriend!!!

                if (chargingPools.TryUpdate(ChargingPool.Id,
                                            ChargingPool,
                                            existingChargingPool,
                                            EventTrackingId,
                                            CurrentUserId))
                {

                    //ToDo: Persistency
                    await Task.Delay(1);

                    Connect(ChargingPool);

                    OnUpdateSuccess?.Invoke(ChargingPool,
                                            existingChargingPool,
                                            EventTrackingId);

                    return AddOrUpdateChargingPoolResult.Updated(
                               ChargingPool,
                               EventTrackingId,
                               Id,
                               this,
                               this
                           );

                }
                else
                {

                    OnError?.Invoke(this,
                                    ChargingPool,
                                    EventTrackingId);

                    return AddOrUpdateChargingPoolResult.Error(
                               ChargingPool,
                               "Error!".ToI18NString(),
                               EventTrackingId,
                               Id,
                               this,
                               this
                           );

                }

            }

            else
            {

                if (chargingPools.TryAdd(ChargingPool,
                                         Connect,
                                         EventTrackingId,
                                         CurrentUserId).Result == CommandResult.Success)
                {

                    //ToDo: Persistency
                    await Task.Delay(1);

                    OnAdditionSuccess?.Invoke(ChargingPool,
                                              EventTrackingId);

                    return AddOrUpdateChargingPoolResult.Added(
                               ChargingPool,
                               EventTrackingId,
                               Id,
                               this,
                               this
                           );

                }
                else
                {

                    OnError?.Invoke(this,
                                    ChargingPool,
                                    EventTrackingId);

                    return AddOrUpdateChargingPoolResult.Error(
                               ChargingPool,
                               "Error!".ToI18NString(),
                               EventTrackingId,
                               Id,
                               this,
                               this
                           );

                }

            }

        }

        #endregion

        #region UpdateChargingPool        (ChargingPool,                             OnUpdateSuccess = null, OnError = null, ...)

        /// <summary>
        /// Update the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the update of the new charging pool failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<UpdateChargingPoolResult> UpdateChargingPool(IChargingPool                                                       ChargingPool,

                                                                       Action<IChargingPool,            IChargingPool, EventTracking_Id>?  OnUpdateSuccess                = null,
                                                                       Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                        = null,

                                                                       Boolean                                                             SkipUpdatedNotifications       = false,
                                                                       Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                                       EventTracking_Id?                                                   EventTrackingId                = null,
                                                                       User_Id?                                                            CurrentUserId                  = null)
        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (!TryGetChargingPoolById(ChargingPool.Id, out var OldChargingPool))
                return UpdateChargingPoolResult.ArgumentError(
                           ChargingPool,
                           $"The given charging pool '{ChargingPool.Id}' does not exists in this API!".ToI18NString(),
                           eventTrackingId,
                           Id,
                           this,
                           this
                       );

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

            chargingPools.TryRemove(OldChargingPool.Id,
                                    out _,
                                    EventTrackingId,
                                    CurrentUserId);

            //ChargingPool.CopyAllLinkedDataFrom(OldChargingPool);
            chargingPools.TryAdd(ChargingPool,
                                 EventTrackingId,
                                 CurrentUserId);

            OnUpdateSuccess?.Invoke(ChargingPool,
                                    ChargingPool,
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

        #region UpdateChargingPool        (ChargingPoolId, UpdateDelegate,           OnUpdateSuccess = null, OnError = null, ...)

        /// <summary>
        /// Update the given charging pool.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification.</param>
        /// <param name="UpdateDelegate">A delegate for updating the given charging pool.</param>
        /// 
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the update of the new charging pool failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<UpdateChargingPoolResult> UpdateChargingPool(ChargingPool_Id                                                     ChargingPoolId,
                                                                       Action<IChargingPool>                                               UpdateDelegate,

                                                                       Action<IChargingPool,            IChargingPool, EventTracking_Id>?  OnUpdateSuccess                = null,
                                                                       Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                        = null,

                                                                       Boolean                                                             SkipUpdatedNotifications       = false,
                                                                       Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                                       EventTracking_Id?                                                   EventTrackingId                = null,
                                                                       User_Id?                                                            CurrentUserId                  = null)
        {

            EventTrackingId ??= EventTracking_Id.New;

            if (!chargingPools.TryRemove(ChargingPoolId,
                                         out var oldChargingPool,
                                         EventTrackingId,
                                         CurrentUserId) ||
                oldChargingPool is null)
            {

                return UpdateChargingPoolResult.ArgumentError(
                           ChargingPoolId,
                           $"The given charging pool '{ChargingPoolId}' does not exists!".ToI18NString(),
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            }

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

            var newChargingPool = oldChargingPool.Clone();
            UpdateDelegate(oldChargingPool);

            //ChargingPool.CopyAllLinkedDataFrom(OldChargingPool);
            chargingPools.TryAdd(newChargingPool,
                                 EventTrackingId,
                                 CurrentUserId);

            OnUpdateSuccess?.Invoke(newChargingPool,
                                    oldChargingPool,
                                    EventTrackingId);

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

            return UpdateChargingPoolResult.Success(newChargingPool,
                                                    EventTrackingId);

        }

        #endregion

        #region RemoveChargingPool        (ChargingPoolId,                           OnRemoveSuccess = null, OnError = null, ...)

        /// <summary>
        /// Remove the given charging pool.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the charging pool.</param>
        /// 
        /// <param name="OnRemoveSuccess">An optional delegate to be called after the successful removal of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the removal of the new charging pool failed.</param>
        /// 
        /// <param name="SkipRemovedNotifications">Whether to skip sending the 'OnRemoved' event.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<DeleteChargingPoolResult> RemoveChargingPool(ChargingPool_Id                                      ChargingPoolId,

                                                                       Action<IChargingPool,            EventTracking_Id>?  OnRemoveSuccess                = null,
                                                                       Action<IChargingStationOperator, EventTracking_Id>?  OnError                        = null,

                                                                       Boolean                                              SkipRemovedNotifications       = false,
                                                                       EventTracking_Id?                                    EventTrackingId                = null,
                                                                       User_Id?                                             CurrentUserId                  = null)
        {

            EventTrackingId ??= EventTracking_Id.New;

            if (chargingPools.TryRemove(ChargingPoolId,
                                        out var chargingPool,
                                        EventTrackingId,
                                        null) &&
                chargingPool is not null)
            {

                return DeleteChargingPoolResult.Success(
                           chargingPool,
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            }

            return DeleteChargingPoolResult.ArgumentError(
                       ChargingPoolId,
                       "error".ToI18NString(),
                       EventTrackingId,
                       Id,
                       this,
                       this
                   );

        }

        #endregion


        #region ChargingPoolExists(ChargingPool)

        /// <summary>
        /// Check if the given ChargingPool is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public Boolean ChargingPoolExists(IChargingPool ChargingPool)

            => ChargingPools.Contains(ChargingPool);

        #endregion

        #region ChargingPoolExists(ChargingPoolId)

        /// <summary>
        /// Determines whether the given user identification exists within this API.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of an user.</param>
        public Boolean ChargingPoolExists(ChargingPool_Id ChargingPoolId)

            => ChargingPoolId.IsNotNullOrEmpty &&
               chargingPools.ContainsId(ChargingPoolId);

        /// <summary>
        /// Determines whether the given user identification exists within this API.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of an user.</param>
        public Boolean ChargingPoolExists(ChargingPool_Id? ChargingPoolId)

            => ChargingPoolId.HasValue &&
               ChargingPoolId.Value.IsNotNullOrEmpty &&
               chargingPools.ContainsId(ChargingPoolId.Value);

        #endregion

        #region GetChargingPoolById(ChargingPoolId)

        public IChargingPool? GetChargingPoolById(ChargingPool_Id ChargingPoolId)

            => chargingPools.GetById(ChargingPoolId);

        #endregion

        #region TryGetChargingPoolById(ChargingPoolId, out ChargingPool)

        /// <summary>
        /// Try to get the charging pool having the given unique identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        public Boolean TryGetChargingPoolById(ChargingPool_Id                         ChargingPoolId,
                                              [NotNullWhen(true)] out IChargingPool?  ChargingPool)
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
        public Boolean TryGetChargingPoolById(ChargingPool_Id?                        ChargingPoolId,
                                              [NotNullWhen(true)] out IChargingPool?  ChargingPool)
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

        #endregion

        #region TryGetChargingPoolByStationId(ChargingStationId, out ChargingPool)

        public Boolean TryGetChargingPoolByStationId(ChargingStation_Id                      ChargingStationId,
                                                     [NotNullWhen(true)] out IChargingPool?  ChargingPool)
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

        #region IEnumerable<ChargingPool> Members

        IEnumerator IEnumerable.GetEnumerator()

            => chargingPools.GetEnumerator();

        public IEnumerator<IChargingPool> GetEnumerator()

            => chargingPools.GetEnumerator();

        #endregion


        #region SetChargingPoolAdminStatus(ChargingPoolId, NewStatus)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                            ChargingPoolId,
                                               Timestamped<ChargingPoolAdminStatusTypes>  NewStatus,
                                               Boolean                                    SendUpstream = false)
        {

            if (TryGetChargingPoolById(ChargingPoolId, out var chargingPool) &&
                chargingPool is not null)
            {
                chargingPool.AdminStatus = NewStatus;
            }

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

        public void SetChargingPoolAdminStatus(ChargingPool_Id                                         ChargingPoolId,
                                               IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>  StatusList,
                                               ChangeMethods                                           ChangeMethod  = ChangeMethods.Replace)
        {

            if (TryGetChargingPoolById(ChargingPoolId, out var chargingPool) &&
                chargingPool is not null)
            {
                chargingPool.SetAdminStatus(StatusList, ChangeMethod);
            }

        }

        #endregion


        #region (internal) UpdateChargingPoolData       (Timestamp, EventTrackingId, ChargingPool, PropertyName, NewValue, OldValue = null, DataSource = null)

        /// <summary>
        /// Update the data of an charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The optional old value of the changed property.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        internal async Task UpdateChargingPoolData(DateTime          Timestamp,
                                                   EventTracking_Id  EventTrackingId,
                                                   IChargingPool     ChargingPool,
                                                   String            PropertyName,
                                                   Object?           NewValue,
                                                   Object?           OldValue     = null,
                                                   Context?          DataSource   = null)
        {

            var onChargingPoolDataChanged = OnChargingPoolDataChanged;
            if (onChargingPoolDataChanged is not null)
                await onChargingPoolDataChanged(Timestamp,
                                                     EventTrackingId,
                                                     ChargingPool,
                                                     PropertyName,
                                                     NewValue,
                                                     OldValue,
                                                     DataSource);

        }

        #endregion

        #region (internal) UpdateChargingPoolAdminStatus(Timestamp, EventTrackingId, ChargingPool, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update a charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="NewStatus">The new charging pool status.</param>
        /// <param name="OldStatus">The old charging pool status.</param>
        /// <param name="DataSource">An optional data source or context for the admin status update.</param>
        internal async Task UpdateChargingPoolAdminStatus(DateTime                                    Timestamp,
                                                          EventTracking_Id                            EventTrackingId,
                                                          IChargingPool                               ChargingPool,
                                                          Timestamped<ChargingPoolAdminStatusTypes>   NewStatus,
                                                          Timestamped<ChargingPoolAdminStatusTypes>?  OldStatus    = null,
                                                          Context?                                    DataSource   = null)
        {

            var onChargingPoolAdminStatusChanged = OnChargingPoolAdminStatusChanged;
            if (onChargingPoolAdminStatusChanged is not null)
                await onChargingPoolAdminStatusChanged(Timestamp,
                                                       EventTrackingId,
                                                       ChargingPool,
                                                       NewStatus,
                                                       OldStatus,
                                                       DataSource);

        }

        #endregion

        #region (internal) UpdateChargingPoolStatus     (Timestamp, EventTrackingId, ChargingPool, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update a charging pool status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="NewStatus">The new charging pool status.</param>
        /// <param name="OldStatus">The optional old charging pool status.</param>
        /// <param name="DataSource">An optional data source or context for the admin status update.</param>
        internal async Task UpdateChargingPoolStatus(DateTime                               Timestamp,
                                                     EventTracking_Id                       EventTrackingId,
                                                     IChargingPool                          ChargingPool,
                                                     Timestamped<ChargingPoolStatusTypes>   NewStatus,
                                                     Timestamped<ChargingPoolStatusTypes>?  OldStatus    = null,
                                                     Context?                               DataSource   = null)
        {

            var onChargingPoolStatusChanged = OnChargingPoolStatusChanged;
            if (onChargingPoolStatusChanged is not null)
                await onChargingPoolStatusChanged(Timestamp,
                                                  EventTrackingId,
                                                  ChargingPool,
                                                  NewStatus,
                                                  OldStatus,
                                                  DataSource);

        }

        #endregion

        #endregion

        //ToDo: Charging Pool Groups

        #region Charging Stations

        #region Data

        private readonly ConcurrentDictionary<ChargingStation_Id, IChargingStation> chargingStationLookup = new ();

        /// <summary>
        /// Return an enumeration of all charging stations.
        /// </summary>
        public IEnumerable<IChargingStation> ChargingStations

            => chargingStationLookup.Values;

        #endregion

        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> OnChargingStationAddition

            => ChargingStationAddition;

        #endregion

        #region ChargingStationUpdate

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, IChargingStation, Boolean> ChargingStationUpdate;

        /// <summary>
        /// Called whenever a charging station will be or was updated.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, IChargingStation, Boolean> OnChargingStationUpdate

            => ChargingStationUpdate;

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> OnChargingStationRemoval

            => ChargingStationRemoval;

        #endregion


        #region ChargingStationIds                (IncludeChargingStations = null)

        /// <summary>
        /// Return an enumeration of all charging station identifications.
        /// </summary>
        /// <param name="IncludeChargingStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate? IncludeChargingStations = null)
        {

            IncludeChargingStations ??= (chargingStation => true);

            return chargingStationLookup.Values.
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

            return chargingStationLookup.Values.
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

            return chargingStationLookup.Values.
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

            return chargingStationLookup.Values.
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

            return chargingStationLookup.Values.
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

            => chargingStationLookup.ContainsKey(ChargingStation.Id);

        #endregion

        #region ContainsChargingStation      (ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)

            => chargingStationLookup.ContainsKey(ChargingStationId);


        /// <summary>
        /// Check if the given ChargingStation identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id? ChargingStationId)

            => ChargingStationId.HasValue &&
                   chargingStationLookup.ContainsKey(ChargingStationId.Value);

        #endregion

        #region GetChargingStationById       (ChargingStationId)

        public IChargingStation? GetChargingStationById(ChargingStation_Id ChargingStationId)
        {

            if (chargingStationLookup.TryGetValue(ChargingStationId, out var chargingStation))
                return chargingStation;

            return null;

        }

        public IChargingStation? GetChargingStationById(ChargingStation_Id? ChargingStationId)
        {

            if (ChargingStationId.HasValue &&
                chargingStationLookup.TryGetValue(ChargingStationId.Value, out var chargingStation))
            {
                return chargingStation;
            }

            return null;

        }

        #endregion

        #region TryGetChargingStationById    (ChargingStationId, out ChargingStation ChargingStation)

        public Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out IChargingStation? ChargingStation)

            => chargingStationLookup.TryGetValue(ChargingStationId, out ChargingStation);


        public Boolean TryGetChargingStationById(ChargingStation_Id? ChargingStationId, out IChargingStation? ChargingStation)
        {

            if (!ChargingStationId.HasValue)
            {
                ChargingStation = null;
                return false;
            }

            return chargingStationLookup.TryGetValue(ChargingStationId.Value, out ChargingStation);

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


        #region (internal) UpdateChargingStationData       (Timestamp, EventTrackingId, ChargingStation, PropertyName, NewValue, OldValue = null, DataSource = null)

        /// <summary>
        /// Update the data of a charging station.
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
        /// Update the current charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        /// <param name="DataSource">An optional data source or context for the charging station admin update.</param>
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
        /// Update a charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggregated charging station status.</param>
        /// <param name="NewStatus">The new aggregated charging station status.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool admin status update.</param>
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

        }

        #endregion

        #endregion

        #region Charging Station Groups

        #region Data

        private readonly EntityHashSet<ChargingStationOperator, ChargingStationGroup_Id, ChargingStationGroup> chargingStationGroups;

        /// <summary>
        /// All charging station groups registered within this charging station operator.
        /// </summary>
        public IEnumerable<ChargingStationGroup> ChargingStationGroups

            => chargingStationGroups;

        #endregion

        #region ChargingStationGroupAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> ChargingStationGroupAddition;

        /// <summary>
        /// Called whenever a charging station group will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> OnChargingStationGroupAddition

            => ChargingStationGroupAddition;

        #endregion

        #region ChargingStationGroupRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> ChargingStationGroupRemoval;

        /// <summary>
        /// Called whenever a charging station group will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> OnChargingStationGroupRemoval

            => ChargingStationGroupRemoval;

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
                                                               IEnumerable<OpenDataLicense>                                        DataLicenses                  = null,

                                                               IEnumerable<IChargingStation>                                       Members                       = null,
                                                               IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                               Func<IChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                                               Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                               UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                               UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                               Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                               Action<IChargingStationOperator, ChargingStationGroup_Id>           OnError                       = null)

        {

            lock (chargingStationGroups)
            {

                #region Initial checks

                if (chargingStationGroups.ContainsId(Id))
                {

                    if (OnError != null)
                        OnError?.Invoke(this, Id);

                    throw new ChargingStationGroupAlreadyExists(this, Id);

                }

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the charging station group must not be null or empty!");

                #endregion

                var chargingStationGroup = new ChargingStationGroup(Id,
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


                if (chargingStationGroups.TryAdd(chargingStationGroup,
                                                 EventTracking_Id.New,
                                                 null).Result == CommandResult.Success)
                {

                    chargingStationGroup.OnEVSEDataChanged                             += UpdateEVSEData;
                    chargingStationGroup.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                    chargingStationGroup.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;

                    chargingStationGroup.OnChargingStationDataChanged                  += UpdateChargingStationData;
                    chargingStationGroup.OnChargingStationStatusChanged                += UpdateChargingStationStatus;
                    chargingStationGroup.OnChargingStationAdminStatusChanged           += UpdateChargingStationAdminStatus;

                    //_ChargingStationGroup.OnDataChanged                                 += UpdateChargingStationGroupData;
                    //_ChargingStationGroup.OnAdminStatusChanged                          += UpdateChargingStationGroupAdminStatus;

                    OnSuccess?.Invoke(chargingStationGroup);

                    return chargingStationGroup;

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

            lock (chargingStationGroups)
            {

                #region Initial checks

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the charging station group must not be null or empty!");

                #endregion

                if (chargingStationGroups.TryGet(Id, out ChargingStationGroup _ChargingStationGroup))
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

            => chargingStationGroups.TryGet(Id, out ChargingStationGroup);

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

            lock (chargingStationGroups)
            {

                if (chargingStationGroups.TryGet(ChargingStationGroupId, out var ChargingStationGroup) &&
                    ChargingStationGroupRemoval.SendVoting(Timestamp.Now,
                                                           this,
                                                           ChargingStationGroup) &&
                    chargingStationGroups.TryRemove(ChargingStationGroupId,
                                                    out var _ChargingStationGroup,
                                                    EventTracking_Id.New,
                                                    null))
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

            lock (chargingStationGroups)
            {

                if (ChargingStationGroupRemoval.SendVoting(Timestamp.Now,
                                                           this,
                                                           ChargingStationGroup) &&
                    chargingStationGroups.TryRemove(ChargingStationGroup.Id,
                                                    out var _ChargingStationGroup,
                                                    EventTracking_Id.New,
                                                    null))
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

        #region Data

        private readonly ConcurrentDictionary<EVSE_Id, IEVSE> evseLookup = new ();

        /// <summary>
        /// Return an enumeration of all EVSEs.
        /// </summary>
        public IEnumerable<IEVSE> EVSEs

            => evseLookup.Values;

        #endregion


        #region EVSEAddition

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> evseAddition;

        public IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> EVSEAddition
            => evseAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSEAddition

            => evseAddition;

        #endregion

        #region EVSEUpdate

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, IEVSE, Boolean> evseUpdate;

        public IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, IEVSE, Boolean> EVSEUpdate
            => evseUpdate;

        /// <summary>
        /// Called whenever an EVSE will be or was update.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, IEVSE, Boolean> OnEVSEUpdate

            => evseUpdate;

        #endregion

        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> evseRemoval;

        public IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> EVSERemoval
            => evseRemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSERemoval

            => evseRemoval;

        #endregion


        #region EVSEIds                (IncludeEVSEs = null)

        /// <summary>
        /// Return an enumeration of all EVSE identifications.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate? IncludeEVSEs = null)
        {

            IncludeEVSEs ??= (evse => true);

            return evseLookup.Values.
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

            return evseLookup.Values.
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

            return evseLookup.Values.
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

            return evseLookup.Values.
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

            return evseLookup.Values.
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

            => evseLookup.ContainsKey(EVSE.Id);

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)

            => evseLookup.ContainsKey(EVSEId);

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id? EVSEId)

            => EVSEId.HasValue &&
                   evseLookup.ContainsKey(EVSEId.Value);

        #endregion

        #region GetEVSEById(EVSEId)

        public IEVSE? GetEVSEById(EVSE_Id EVSEId)
        {

            if (evseLookup.TryGetValue(EVSEId, out var evse))
                return evse;

            return null;

        }

        public IEVSE? GetEVSEById(EVSE_Id? EVSEId)
        {

            if (EVSEId.HasValue &&
                evseLookup.TryGetValue(EVSEId.Value, out var evse))
            {
                return evse;
            }

            return null;

        }

        #endregion

        #region TryGetEVSEById(EVSEId, out EVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE? EVSE)

            => evseLookup.TryGetValue(EVSEId, out EVSE);


        public Boolean TryGetEVSEById(EVSE_Id? EVSEId, out IEVSE? EVSE)
        {

            if (!EVSEId.HasValue)
            {
                EVSE = null;
                return false;
            }

            return evseLookup.TryGetValue(EVSEId.Value, out EVSE);

        }

        #endregion

        #region TryGetChargingStationByEVSEId(EVSEId, out ChargingStation)

        public Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out IChargingStation? ChargingStation)
        {

            if (evseLookup.TryGetValue(EVSEId, out var evse))
            {
                ChargingStation = evse.ChargingStation;
                return ChargingStation is not null;
            }

            ChargingStation = null;
            return false;

        }

        public Boolean TryGetChargingStationByEVSEId(EVSE_Id? EVSEId, out IChargingStation? ChargingStation)
        {

            if (EVSEId.HasValue &&
                evseLookup.TryGetValue(EVSEId.Value, out var evse))
            {
                ChargingStation = evse.ChargingStation;
                return ChargingStation is not null;
            }

            ChargingStation = null;
            return false;

        }

        #endregion

        #region TryGetChargingPoolByEVSEId   (EVSEId, out ChargingPool)

        public Boolean TryGetChargingPoolByEVSEId(EVSE_Id EVSEId, out IChargingPool? ChargingPool)
        {

            if (evseLookup.TryGetValue(EVSEId, out var evse))
            {
                ChargingPool = evse.ChargingStation?.ChargingPool;
                return ChargingPool is not null;
            }

            ChargingPool = null;
            return false;

        }

        public Boolean TryGetChargingPoolByEVSEId(EVSE_Id? EVSEId, out IChargingPool? ChargingPool)
        {

            if (EVSEId.HasValue &&
                evseLookup.TryGetValue(EVSEId.Value, out var evse))
            {
                ChargingPool = evse.ChargingStation?.ChargingPool;
                return ChargingPool is not null;
            }

            ChargingPool = null;
            return false;

        }

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
                evse.SetStatus(NewStatus);
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
                DebugX.Log(e, $"ChargingStationOperator '{Id}'.UpdateEVSEData of EVSE '{EVSE.Id}' property '{PropertyName}' from '{OldValue?.ToString() ?? "-"}' to '{NewValue?.ToString() ?? "-"}'");
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
                DebugX.Log(e, $"ChargingStationOperator '{Id}'.UpdateEVSEAdminStatus of EVSE '{EVSE.Id}' from '{OldAdminStatus?.ToString() ?? "-"}' to '{NewAdminStatus}'");
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
                DebugX.Log(e, $"ChargingStationOperator '{Id}'.UpdateEVSEStatus of EVSE '{EVSE.Id}' from '{OldStatus}' to '{NewStatus}'");
            }

        }

        #endregion

        #endregion

        #region EVSE Groups

        #region Data

        private readonly EntityHashSet<ChargingStationOperator, EVSEGroup_Id, EVSEGroup> evseGroups;

        /// <summary>
        /// All EVSE groups registered within this charging station operator.
        /// </summary>
        public IEnumerable<EVSEGroup> EVSEGroups

            => evseGroups;

        #endregion

        #region EVSEGroupAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, EVSEGroup, Boolean> evseGroupAddition;

        /// <summary>
        /// Called whenever an EVSE group will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, EVSEGroup, Boolean> OnEVSEGroupAddition

            => evseGroupAddition;

        #endregion

        #region EVSEGroupRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, EVSEGroup, Boolean> evseGroupRemoval;

        /// <summary>
        /// Called whenever an EVSE group will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, EVSEGroup, Boolean> OnEVSEGroupRemoval

            => evseGroupRemoval;

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
                                         IEnumerable<OpenDataLicense>                   DataLicenses                  = null,

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

            lock (evseGroups)
            {

                #region Initial checks

                if (evseGroups.ContainsId(Id))
                {

                    if (OnError != null)
                        OnError?.Invoke(this, Id);

                    throw new EVSEGroupAlreadyExists(this, Id);

                }

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the EVSE group must not be null or empty!");

                #endregion

                var evseGroup = new EVSEGroup(Id,
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


                if (evseGroups.TryAdd(evseGroup,
                                      EventTracking_Id.New,
                                      null).Result == CommandResult.Success)
                {

                    evseGroup.OnEVSEDataChanged                  += UpdateEVSEData;
                    evseGroup.OnEVSEStatusChanged                += UpdateEVSEStatus;
                    evseGroup.OnEVSEAdminStatusChanged           += UpdateEVSEAdminStatus;

                    evseGroup.OnEVSEDataChanged                  += UpdateEVSEData;
                    evseGroup.OnEVSEStatusChanged                += UpdateEVSEStatus;
                    evseGroup.OnEVSEAdminStatusChanged           += UpdateEVSEAdminStatus;

                    //_EVSEGroup.OnDataChanged                                 += UpdateEVSEGroupData;
                    //_EVSEGroup.OnAdminStatusChanged                          += UpdateEVSEGroupAdminStatus;

                    OnSuccess?.Invoke(evseGroup);

                    return evseGroup;

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

            lock (evseGroups)
            {

                #region Initial checks

                if (Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(Name), "The name of the EVSE group must not be null or empty!");

                #endregion

                if (evseGroups.TryGet(Id, out EVSEGroup _EVSEGroup))
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

            => evseGroups.TryGet(Id, out EVSEGroup);

        #endregion


        #region RemoveEVSEGroup(EVSEGroupId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All EVSE groups registered within this charging station operator.
        /// </summary>
        /// <param name="EVSEGroupId">The unique identification of the EVSE group to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new EVSE group after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the EVSE group failed.</param>
        public EVSEGroup RemoveEVSEGroup(EVSEGroup_Id                                    EVSEGroupId,
                                         Action<ChargingStationOperator, EVSEGroup>?     OnSuccess   = null,
                                         Action<ChargingStationOperator, EVSEGroup_Id>?  OnError     = null)
        {

            lock (evseGroups)
            {

                if (evseGroups.TryGet(EVSEGroupId, out var EVSEGroup) &&
                    evseGroupRemoval.SendVoting(Timestamp.Now,
                                                this,
                                                EVSEGroup) &&
                    evseGroups.TryRemove(EVSEGroupId,
                                         out var _EVSEGroup,
                                         EventTracking_Id.New,
                                         null))
                {

                    OnSuccess?.Invoke(this, EVSEGroup);

                    evseGroupRemoval.SendNotification(Timestamp.Now,
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

            lock (evseGroups)
            {

                if (evseGroupRemoval.SendVoting(Timestamp.Now,
                                                this,
                                                EVSEGroup) &&
                    evseGroups.TryRemove(EVSEGroup.Id,
                                         out var _EVSEGroup,
                                         EventTracking_Id.New,
                                         null))
                {

                    OnSuccess?.Invoke(this, _EVSEGroup);

                    evseGroupRemoval.SendNotification(Timestamp.Now,
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

        #region Data

        private readonly EntityHashSet<IChargingStationOperator, ChargingTariff_Id, IChargingTariff> chargingTariffs;

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        public IEnumerable<IChargingTariff> ChargingTariffs
            => chargingTariffs;

        #endregion


        #region ChargingTariffAddition

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, Boolean> chargingTariffAddition;

        public IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, Boolean> ChargingTariffAddition
            => chargingTariffAddition;

        /// <summary>
        /// Called whenever a charging tariff will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, Boolean> OnChargingTariffAddition
            => chargingTariffAddition;

        #endregion

        #region ChargingTariffUpdate

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, IChargingTariff, Boolean> chargingTariffUpdate;

        public IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, IChargingTariff, Boolean> ChargingTariffUpdate
            => chargingTariffUpdate;

        /// <summary>
        /// Called whenever a charging Tariff will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, IChargingTariff, Boolean> OnChargingTariffUpdate
            => chargingTariffUpdate;

        #endregion

        #region ChargingTariffRemoval

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, Boolean> chargingTariffRemoval;

        public IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, Boolean> ChargingTariffRemoval
            => chargingTariffRemoval;

        /// <summary>
        /// Called whenever a charging Tariff will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingTariff, Boolean> OnChargingTariffRemoval
            => chargingTariffRemoval;

        #endregion


        #region GetOrCreateChargingTariff(Id,       Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new charging Tariff having the given
        /// unique charging Tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing Tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging Tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging Tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging Tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging Tariff failed.</param>
        public Task<AddChargingTariffResult> GetOrCreateChargingTariff(ChargingTariff_Id                                                     Id,
                                                                       I18NString                                                            Name,
                                                                       I18NString                                                            Description,
                                                                       IEnumerable<ChargingTariffElement>                                    TariffElements,
                                                                       Currency                                                              Currency,
                                                                       Brand                                                                 Brand,
                                                                       URL                                                                   TariffURL,
                                                                       EnergyMix                                                             EnergyMix,

                                                                       String?                                                               DataSource                     = null,
                                                                       DateTime?                                                             LastChange                     = null,

                                                                       JObject?                                                              CustomData                     = null,
                                                                       UserDefinedDictionary?                                                InternalData                   = null,

                                                                       Action<IChargingTariff,                           EventTracking_Id>?  OnSuccess                      = null,
                                                                       Action<IChargingStationOperator, IChargingTariff, EventTracking_Id>?  OnError                        = null,

                                                                       Boolean                                                               SkipAddedNotifications         = false,
                                                                       Func<ChargingStationOperator_Id, ChargingTariff_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                                       EventTracking_Id?                                                     EventTrackingId                = null,
                                                                       User_Id?                                                              CurrentUserId                  = null)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name), "The name of the charging Tariff must not be null or empty!");

            #endregion

            if (chargingTariffs.TryGet(Id, out var chargingTariff) &&
                chargingTariff is not null)
            {

                return Task.FromResult(
                           AddChargingTariffResult.Success(
                               chargingTariff,
                               EventTracking_Id.New,
                               Id,
                               this,
                               this
                           )
                       );
            }

            return this.CreateChargingTariff(Id,
                                             Name,
                                             Description,
                                             TariffElements,
                                             Currency,
                                             Brand,
                                             TariffURL,
                                             EnergyMix,

                                             DataSource,
                                             LastChange,

                                             CustomData,
                                             InternalData,

                                             OnSuccess,
                                             OnError,

                                             SkipAddedNotifications,
                                             AllowInconsistentOperatorIds,
                                             EventTrackingId,
                                             CurrentUserId);

        }

        #endregion

        #region GetOrCreateChargingTariff(IdSuffix, Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new charging Tariff having the given
        /// unique charging Tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging Tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging Tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging Tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging Tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging Tariff failed.</param>
        public Task<AddChargingTariffResult> GetOrCreateChargingTariff(String                                                                IdSuffix,
                                                                       I18NString                                                            Name,
                                                                       I18NString                                                            Description,
                                                                       IEnumerable<ChargingTariffElement>                                    TariffElements,
                                                                       Currency                                                              Currency,
                                                                       Brand                                                                 Brand,
                                                                       URL                                                                   TariffURL,
                                                                       EnergyMix                                                             EnergyMix,

                                                                       String?                                                               DataSource                     = null,
                                                                       DateTime?                                                             LastChange                     = null,

                                                                       JObject?                                                              CustomData                     = null,
                                                                       UserDefinedDictionary?                                                InternalData                   = null,

                                                                       Action<IChargingTariff,                           EventTracking_Id>?  OnSuccess                      = null,
                                                                       Action<IChargingStationOperator, IChargingTariff, EventTracking_Id>?  OnError                        = null,

                                                                       Boolean                                                               SkipAddedNotifications         = false,
                                                                       Func<ChargingStationOperator_Id, ChargingTariff_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                                       EventTracking_Id?                                                     EventTrackingId                = null,
                                                                       User_Id?                                                              CurrentUserId                  = null)


        {

            #region Initial checks

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix), "The given suffix of the unique identification of the new charging Tariff must not be null or empty!");

            #endregion

            return GetOrCreateChargingTariff(ChargingTariff_Id.Parse(Id, IdSuffix.Trim()),
                                             Name,
                                             Description,
                                             TariffElements,
                                             Currency,
                                             Brand,
                                             TariffURL,
                                             EnergyMix,

                                             DataSource,
                                             LastChange,

                                             CustomData,
                                             InternalData,

                                             OnSuccess,
                                             OnError,

                                             SkipAddedNotifications,
                                             AllowInconsistentOperatorIds,
                                             EventTrackingId,
                                             CurrentUserId);

        }

        #endregion


        #region AddChargingTariff           (ChargingTariff,                             OnSuccess       = null, OnError = null, ...)

        /// <summary>
        /// Add a new charging tariff.
        /// </summary>
        /// <param name="ChargingTariff">A new charging tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging tariff.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging tariff failed.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddChargingTariffResult> AddChargingTariff(IChargingTariff                                                       ChargingTariff,

                                                                     Action<IChargingTariff,                           EventTracking_Id>?  OnSuccess                      = null,
                                                                     Action<IChargingStationOperator, IChargingTariff, EventTracking_Id>?  OnError                        = null,

                                                                     Boolean                                                               SkipAddedNotifications         = false,
                                                                     Func<ChargingStationOperator_Id, ChargingTariff_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                                     EventTracking_Id?                                                     EventTrackingId                = null,
                                                                     User_Id?                                                              CurrentUserId                  = null)
        {

            #region Initial checks

            EventTrackingId              ??= EventTracking_Id.New;
            AllowInconsistentOperatorIds ??= ((chargingStationOperatorId, chargingTariffId) => false);

            if (ChargingTariff.Id.OperatorId != this.Id && !AllowInconsistentOperatorIds(this.Id, ChargingTariff.Id))
                return AddChargingTariffResult.Error(
                           ChargingTariff,
                           $"The operator identification of the given charging tariff '{ChargingTariff.Id.OperatorId}' is invalid!".ToI18NString(),
                           EventTrackingId,
                           this.Id,
                           this
                       );

            #endregion


            if (chargingTariffs.TryAdd(ChargingTariff,
                                       EventTrackingId,
                                       CurrentUserId).Result == CommandResult.Success)
            {

                //ToDo: Persistency
                await Task.Delay(1);

                OnSuccess?.Invoke(ChargingTariff,
                                  EventTrackingId);

                return AddChargingTariffResult.Success(
                           ChargingTariff,
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            }

            OnError?.Invoke(this,
                            ChargingTariff,
                            EventTrackingId);

            return AddChargingTariffResult.Error(
                       ChargingTariff,
                       "Could not add the given charging tariff!".ToI18NString(),
                       EventTrackingId,
                       Id,
                       this,
                       this
                   );

        }

        #endregion

        #region AddChargingTariffIfNotExists(ChargingTariff,                             OnSuccess       = null,                 ...)

        /// <summary>
        /// Add a new charging tariff, but do not fail when this charging tariff already exists.
        /// </summary>
        /// <param name="ChargingTariff">A new charging tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging tariff.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddChargingTariffResult> AddChargingTariffIfNotExists(IChargingTariff                                                ChargingTariff,

                                                                                Action<IChargingTariff, EventTracking_Id>?                     OnSuccess                      = null,

                                                                                Boolean                                                        SkipAddedNotifications         = false,
                                                                                Func<ChargingStationOperator_Id, ChargingTariff_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                                EventTracking_Id?                                              EventTrackingId                = null,
                                                                                User_Id?                                                       CurrentUserId                  = null)
        {

            #region Initial checks

            EventTrackingId              ??= EventTracking_Id.New;
            AllowInconsistentOperatorIds ??= ((chargingStationOperatorId, chargingTariffId) => false);

            if (ChargingTariff.Id.OperatorId != Id && !AllowInconsistentOperatorIds(Id, ChargingTariff.Id))
                return AddChargingTariffResult.ArgumentError(
                           ChargingTariff,
                           $"The operator identification of the given charging tariff '{ChargingTariff.Id.OperatorId}' is invalid!".ToI18NString(),
                           EventTrackingId,
                           Id,
                           this
                       );

            #endregion

            if (chargingTariffs.TryAdd(ChargingTariff,
                                       EventTrackingId,
                                       CurrentUserId).Result == CommandResult.Success)
            {

                //ToDo: Persistency
                await Task.Delay(1);

                OnSuccess?.Invoke(ChargingTariff,
                                  EventTrackingId);

                return AddChargingTariffResult.Success(
                           ChargingTariff,
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            }

            return AddChargingTariffResult.NoOperation(
                       ChargingTariff,
                       EventTrackingId,
                       Id,
                       this,
                       this
                   );

        }

        #endregion

        #region AddOrUpdateChargingTariff   (ChargingTariff,   OnAdditionSuccess = null, OnUpdateSuccess = null, OnError = null, ...)

        /// <summary>
        /// Add a new or update an existing charging tariff.
        /// </summary>
        /// <param name="ChargingTariff">A new or updated charging tariff.</param>
        /// 
        /// <param name="OnAdditionSuccess">An optional delegate to be called after the successful addition of the charging tariff.</param>
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging tariff.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging tariff failed.</param>
        /// 
        /// <param name="SkipAddOrUpdatedUpdatedNotifications">Whether to skip sending the 'OnAddedOrUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddOrUpdateChargingTariffResult> AddOrUpdateChargingTariff(IChargingTariff                                                       ChargingTariff,

                                                                                     Action<IChargingTariff,                           EventTracking_Id>?  OnAdditionSuccess                      = null,
                                                                                     Action<IChargingTariff,          IChargingTariff, EventTracking_Id>?  OnUpdateSuccess                        = null,
                                                                                     Action<IChargingStationOperator, IChargingTariff, EventTracking_Id>?  OnError                                = null,

                                                                                     Boolean                                                               SkipAddOrUpdatedUpdatedNotifications   = false,
                                                                                     Func<ChargingStationOperator_Id, ChargingTariff_Id, Boolean>?         AllowInconsistentOperatorIds           = null,
                                                                                     EventTracking_Id?                                                     EventTrackingId                        = null,
                                                                                     User_Id?                                                              CurrentUserId                          = null)
        {

            #region Initial checks

            EventTrackingId              ??= EventTracking_Id.New;
            AllowInconsistentOperatorIds ??= ((chargingStationOperatorId, chargingTariffId) => false);

            if (ChargingTariff.Id.OperatorId != this.Id && !AllowInconsistentOperatorIds(this.Id, ChargingTariff.Id))
                return AddOrUpdateChargingTariffResult.ArgumentError(
                           ChargingTariff,
                           $"The operator identification of the given charging tariff '{ChargingTariff.Id.OperatorId}' is invalid!".ToI18NString(),
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            #endregion


            if (chargingTariffs.TryGet(ChargingTariff.Id, out var existingChargingTariff) &&
                existingChargingTariff is not null)
            {

                var xx1 = existingChargingTariff.Equals(ChargingTariff);

                var xx2 = existingChargingTariff == ChargingTariff; //FalseFriend!!!

                if (chargingTariffs.TryUpdate(ChargingTariff.Id,
                                              ChargingTariff,
                                              existingChargingTariff,
                                              EventTrackingId,
                                              CurrentUserId))
                {

                    //ToDo: Persistency
                    await Task.Delay(1);

                    OnUpdateSuccess?.Invoke(ChargingTariff,
                                            existingChargingTariff,
                                            EventTrackingId);

                    return AddOrUpdateChargingTariffResult.Updated(
                               ChargingTariff,
                               EventTrackingId,
                               Id,
                               this,
                               this
                           );

                }
                else
                {

                    OnError?.Invoke(this,
                                    ChargingTariff,
                                    EventTrackingId);

                    return AddOrUpdateChargingTariffResult.Error(
                               ChargingTariff,
                               "Error!".ToI18NString(),
                               EventTrackingId,
                               Id,
                               this,
                               this
                           );

                }

            }

            else
            {

                if (chargingTariffs.TryAdd(ChargingTariff,
                                           EventTrackingId,
                                           CurrentUserId).Result == CommandResult.Success)
                {

                    //ToDo: Persistency
                    await Task.Delay(1);

                    OnAdditionSuccess?.Invoke(ChargingTariff,
                                              EventTrackingId);

                    return AddOrUpdateChargingTariffResult.Added(
                               ChargingTariff,
                               EventTrackingId,
                               Id,
                               this,
                               this
                           );

                }
                else
                {

                    OnError?.Invoke(this,
                                    ChargingTariff,
                                    EventTrackingId);

                    return AddOrUpdateChargingTariffResult.Error(
                               ChargingTariff,
                               "Error!".ToI18NString(),
                               EventTrackingId,
                               Id,
                               this,
                               this
                           );

                }

            }

        }

        #endregion

        #region UpdateChargingTariff        (ChargingTariff,                             OnUpdateSuccess = null, OnError = null, ...)

        /// <summary>
        /// Update the given charging tariff.
        /// </summary>
        /// <param name="ChargingTariff">A charging tariff.</param>
        /// 
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging tariff.</param>
        /// <param name="OnError">An optional delegate to be called whenever the update of the new charging tariff failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<UpdateChargingTariffResult> UpdateChargingTariff(IChargingTariff                                                       ChargingTariff,

                                                                           Action<IChargingTariff,          IChargingTariff, EventTracking_Id>?  OnUpdateSuccess                = null,
                                                                           Action<IChargingStationOperator, IChargingTariff, EventTracking_Id>?  OnError                        = null,

                                                                           Boolean                                                               SkipUpdatedNotifications       = false,
                                                                           Func<ChargingStationOperator_Id, ChargingTariff_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                                           EventTracking_Id?                                                     EventTrackingId                = null,
                                                                           User_Id?                                                              CurrentUserId                  = null)
        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (!TryGetChargingTariffById(ChargingTariff.Id, out var OldChargingTariff))
                return UpdateChargingTariffResult.ArgumentError(
                           ChargingTariff,
                           $"The given charging tariff '{ChargingTariff.Id}' does not exists in this API!".ToI18NString(),
                           eventTrackingId,
                           Id,
                           this,
                           this
                       );

            //if (ChargingTariff.API is not null && ChargingTariff.API != this)
            //    return UpdateChargingTariffResult.ArgumentError(ChargingTariff,
            //                                                  eventTrackingId,
            //                                                  nameof(ChargingTariff.API),
            //                                                  "The given charging tariff is not attached to this API!");

            //ChargingTariff.API = this;


            //await WriteToDatabaseFile(updateChargingTariff_MessageType,
            //                          ChargingTariff.ToJSON(),
            //                          eventTrackingId,
            //                          CurrentChargingTariffId);

            chargingTariffs.TryRemove(OldChargingTariff.Id,
                                      out _,
                                      EventTrackingId,
                                      CurrentUserId);

            //ChargingTariff.CopyAllLinkedDataFrom(OldChargingTariff);
            chargingTariffs.TryAdd(ChargingTariff,
                                   EventTrackingId,
                                   CurrentUserId);

            OnUpdateSuccess?.Invoke(ChargingTariff,
                                    ChargingTariff,
                                    eventTrackingId);

            //var OnChargingTariffUpdatedLocal = OnChargingTariffUpdated;
            //if (OnChargingTariffUpdatedLocal is not null)
            //    await OnChargingTariffUpdatedLocal.Invoke(Timestamp.Now,
            //                                            ChargingTariff,
            //                                            OldChargingTariff,
            //                                            eventTrackingId, 
            //                                            CurrentChargingTariffId);

            //if (!SkipChargingTariffUpdatedNotifications)
            //    await SendNotifications(ChargingTariff,
            //                            updateChargingTariff_MessageType,
            //                            OldChargingTariff,
            //                            eventTrackingId,
            //                            CurrentChargingTariffId);

            return UpdateChargingTariffResult.Success(
                       ChargingTariff,
                       eventTrackingId,
                       Id,
                       this,
                       this
                   );

        }

        #endregion

        #region UpdateChargingTariff        (ChargingTariffId, UpdateDelegate,           OnUpdateSuccess = null, OnError = null, ...)

        /// <summary>
        /// Update the given charging tariff.
        /// </summary>
        /// <param name="ChargingTariffId">A charging tariff identification.</param>
        /// <param name="UpdateDelegate">A delegate for updating the given charging tariff.</param>
        /// 
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging tariff.</param>
        /// <param name="OnError">An optional delegate to be called whenever the update of the new charging tariff failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<UpdateChargingTariffResult> UpdateChargingTariff(ChargingTariff_Id                                                     ChargingTariffId,
                                                                           Action<IChargingTariff>                                               UpdateDelegate,

                                                                           Action<IChargingTariff,          IChargingTariff, EventTracking_Id>?  OnUpdateSuccess                = null,
                                                                           Action<IChargingStationOperator, IChargingTariff, EventTracking_Id>?  OnError                        = null,

                                                                           Boolean                                                               SkipUpdatedNotifications       = false,
                                                                           Func<ChargingStationOperator_Id, ChargingTariff_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                                           EventTracking_Id?                                                     EventTrackingId                = null,
                                                                           User_Id?                                                              CurrentUserId                  = null)
        {

            EventTrackingId ??= EventTracking_Id.New;

            if (!chargingTariffs.TryRemove(ChargingTariffId,
                                           out var oldChargingTariff,
                                           EventTrackingId,
                                           CurrentUserId) ||
                oldChargingTariff is null)
            {

                return UpdateChargingTariffResult.ArgumentError(
                           oldChargingTariff,
                           $"The given charging tariff '{ChargingTariffId}' does not exists!".ToI18NString(),
                           EventTrackingId,
                           Id,
                           this,
                           this
                       );

            }

            //if (ChargingTariff.API is not null && ChargingTariff.API != this)
            //    return UpdateChargingTariffResult.ArgumentError(ChargingTariff,
            //                                                  eventTrackingId,
            //                                                  nameof(ChargingTariff.API),
            //                                                  "The given charging tariff is not attached to this API!");

            //ChargingTariff.API = this;


            //await WriteToDatabaseFile(updateChargingTariff_MessageType,
            //                          ChargingTariff.ToJSON(),
            //                          eventTrackingId,
            //                          CurrentChargingTariffId);

            var newChargingTariff = oldChargingTariff.Clone();
            UpdateDelegate(oldChargingTariff);

            //ChargingTariff.CopyAllLinkedDataFrom(OldChargingTariff);
            chargingTariffs.TryAdd(newChargingTariff,
                                   EventTrackingId,
                                   CurrentUserId);

            OnUpdateSuccess?.Invoke(newChargingTariff,
                                    oldChargingTariff,
                                    EventTrackingId);

            //var OnChargingTariffUpdatedLocal = OnChargingTariffUpdated;
            //if (OnChargingTariffUpdatedLocal is not null)
            //    await OnChargingTariffUpdatedLocal.Invoke(Timestamp.Now,
            //                                            ChargingTariff,
            //                                            OldChargingTariff,
            //                                            eventTrackingId, 
            //                                            CurrentChargingTariffId);

            //if (!SkipChargingTariffUpdatedNotifications)
            //    await SendNotifications(ChargingTariff,
            //                            updateChargingTariff_MessageType,
            //                            OldChargingTariff,
            //                            eventTrackingId,
            //                            CurrentChargingTariffId);

            return UpdateChargingTariffResult.Success(
                       newChargingTariff,
                       EventTrackingId,
                       Id,
                       this,
                       this
                   );

        }

        #endregion

        #region RemoveChargingTariff(ChargingTariffId, OnSuccess = null, OnError = null)

        /// <summary>
        /// Remove the given charging Tariff.
        /// </summary>
        /// <param name="Id">The unique identification of the charging Tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful removal of the charging Tariff.</param>
        /// <param name="OnError">An optional delegate to be called whenever the removal of the new charging Tariff failed.</param>
        /// 
        /// <param name="SkipRemovedNotifications">Whether to skip sending the 'OnRemoved' event.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<DeleteChargingTariffResult>

            RemoveChargingTariff(ChargingTariff_Id                                    Id,

                                 Action<IChargingTariff,          EventTracking_Id>?  OnSuccess                  = null,
                                 Action<IChargingStationOperator, EventTracking_Id>?  OnError                    = null,

                                 Boolean                                              SkipRemovedNotifications   = false,
                                 EventTracking_Id?                                    EventTrackingId            = null,
                                 User_Id?                                             CurrentUserId              = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            if (chargingTariffs.TryRemove(Id,
                                          out var chargingTariff,
                                          EventTrackingId,
                                          null) &&
                chargingTariff is not null)
            {

                return DeleteChargingTariffResult.Success(
                           chargingTariff,
                           EventTrackingId,
                           this.Id,
                           this
                       );

            }

            return DeleteChargingTariffResult.ArgumentError(
                       Id,
                       "error".ToI18NString(),
                       EventTrackingId,
                       this.Id,
                       this
                   );

        }

        #endregion


        #region ChargingTariffExists(ChargingTariff)

        /// <summary>
        /// Check if the given ChargingTariff is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingTariff">A charging pool.</param>
        public Boolean ChargingTariffExists(IChargingTariff ChargingTariff)

            => ChargingTariffs.Contains(ChargingTariff);

        #endregion

        #region ChargingTariffExists(ChargingTariffId)

        /// <summary>
        /// Determines whether the given user identification exists within this API.
        /// </summary>
        /// <param name="ChargingTariffId">The unique identification of an user.</param>
        public Boolean ChargingTariffExists(ChargingTariff_Id ChargingTariffId)

            => ChargingTariffId.IsNotNullOrEmpty &&
               chargingTariffs.ContainsId(ChargingTariffId);

        /// <summary>
        /// Determines whether the given user identification exists within this API.
        /// </summary>
        /// <param name="ChargingTariffId">The unique identification of an user.</param>
        public Boolean ChargingTariffExists(ChargingTariff_Id? ChargingTariffId)

            => ChargingTariffId.HasValue &&
               ChargingTariffId.Value.IsNotNullOrEmpty &&
               chargingTariffs.ContainsId(ChargingTariffId.Value);

        #endregion

        #region GetChargingTariff   (Id)

        /// <summary>
        /// Return to charging Tariff for the given charging Tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing Tariff.</param>
        public IChargingTariff? GetChargingTariff(ChargingTariff_Id Id)
        {

            if (chargingTariffs.TryGet(Id, out var chargingTariff))
                return chargingTariff;

            return null;

        }

        #endregion

        #region TryGetChargingTariffById(Id, out ChargingTariff)

        /// <summary>
        /// Try to return to charging Tariff for the given charging Tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing Tariff.</param>
        /// <param name="ChargingTariff">The charing Tariff.</param>
        public Boolean TryGetChargingTariffById(ChargingTariff_Id     Id,
                                                out IChargingTariff?  ChargingTariff)

            => chargingTariffs.TryGet(Id, out ChargingTariff);

        #endregion

        public IEnumerable<IChargingTariff>   GetChargingTariffs  (ChargingPool_Id?       ChargingPoolId        = null,
                                                                   ChargingStation_Id?    ChargingStationId     = null,
                                                                   EVSE_Id?               EVSEId                = null,
                                                                   ChargingConnector_Id?  ChargingConnectorId   = null,
                                                                   EMobilityProvider_Id?  EMobilityProviderId   = null)
        {

            return Array.Empty<ChargingTariff>();

        }

        public IEnumerable<ChargingTariff_Id> GetChargingTariffIds(ChargingPool_Id?       ChargingPoolId        = null,
                                                                   ChargingStation_Id?    ChargingStationId     = null,
                                                                   EVSE_Id?               EVSEId                = null,
                                                                   ChargingConnector_Id?  ChargingConnectorId   = null,
                                                                   EMobilityProvider_Id?  EMobilityProviderId   = null)
        {

            return Array.Empty<ChargingTariff_Id>();

        }

        #endregion

        #region ChargingTariffGroups

        #region ChargingTariffGroupAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> chargingTariffGroupAddition;

        public IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> ChargingTariffGroupAddition
            => chargingTariffGroupAddition;

        /// <summary>
        /// Called whenever a charging Tariff will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> OnChargingTariffGroupAddition

            => chargingTariffGroupAddition;

        #endregion

        #region ChargingTariffGroups

        private readonly EntityHashSet<ChargingStationOperator, ChargingTariffGroup_Id, ChargingTariffGroup> chargingTariffGroups;

        /// <summary>
        /// All charging Tariff groups registered within this charging station operator.
        /// </summary>
        public IEnumerable<ChargingTariffGroup> ChargingTariffGroups
            => chargingTariffGroups;

        #endregion


        #region CreateChargingTariffGroup     (IdSuffix, Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging Tariff group having the given
        /// unique charging Tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the charing Tariff group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging Tariff group.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging Tariff group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging Tariff group failed.</param>
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


                if (chargingTariffGroups.TryAdd(chargingTariffGroup,
                                                EventTracking_Id.New,
                                                null).Result == CommandResult.Success)
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

                    return chargingTariffGroup;

                }

                return null;

            }

        }

        #endregion

        #region GetOrCreateChargingTariffGroup(Id,       Name, Description = null, ..., OnSuccess = null, OnError = null)

        /// <summary>
        /// Get or create and register a new charging Tariff having the given
        /// unique charging Tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the charing Tariff group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging Tariff.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging Tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging Tariff failed.</param>
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
        /// Return to charging Tariff for the given charging Tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing Tariff.</param>
        public ChargingTariffGroup? GetChargingTariffGroup(ChargingTariffGroup_Id Id)
        {

            if (chargingTariffGroups.TryGet(Id, out var chargingTariffGroup))
                return chargingTariffGroup;

            return null;

        }

        #endregion

        #region TryGetChargingTariffGroup(Id, out ChargingTariffGroup)

        /// <summary>
        /// Try to return to charging Tariff for the given charging Tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing Tariff.</param>
        /// <param name="ChargingTariffGroup">The charing Tariff.</param>
        public Boolean TryGetChargingTariffGroup(ChargingTariffGroup_Id   Id,
                                                 out ChargingTariffGroup? ChargingTariffGroup)

            => chargingTariffGroups.TryGet(Id, out ChargingTariffGroup);

        #endregion


        #region ChargingTariffGroupRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> chargingTariffGroupRemoval;

        public IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> ChargingTariffGroupRemoval
            => chargingTariffGroupRemoval;

        /// <summary>
        /// Called whenever a charging Tariff group will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> OnChargingTariffGroupRemoval

            => chargingTariffGroupRemoval;

        #endregion

        #region RemoveChargingTariffGroup(ChargingTariffGroupId, OnSuccess = null, OnError = null)

        /// <summary>
        /// All charging Tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariffGroupId">The unique identification of the charging Tariff to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging Tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging Tariff failed.</param>
        public ChargingTariffGroup? RemoveChargingTariffGroup(ChargingTariffGroup_Id                                     ChargingTariffGroupId,
                                                              Action<IChargingStationOperator, ChargingTariffGroup>?     OnSuccess   = null,
                                                              Action<IChargingStationOperator, ChargingTariffGroup_Id>?  OnError     = null)
        {

            lock (chargingTariffGroups)
            {

                if (chargingTariffGroups.TryGet(ChargingTariffGroupId, out var chargingTariffGroup) &&
                    chargingTariffGroup is not null                                                 &&
                    chargingTariffGroupRemoval.SendVoting(Timestamp.Now, this, chargingTariffGroup) &&
                    chargingTariffGroups.TryRemove(ChargingTariffGroupId,
                                                   out _,
                                                   EventTracking_Id.New,
                                                   null))
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
        /// All charging Tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariffGroup">The charging Tariff to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging Tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging Tariff failed.</param>
        public ChargingTariffGroup? RemoveChargingTariffGroup(ChargingTariffGroup                                     ChargingTariffGroup,
                                                              Action<IChargingStationOperator, ChargingTariffGroup>?  OnSuccess   = null,
                                                              Action<IChargingStationOperator, ChargingTariffGroup>?  OnError     = null)
        {

            lock (chargingTariffGroups)
            {

                if (chargingTariffGroupRemoval.SendVoting(Timestamp.Now, this, ChargingTariffGroup) &&
                    chargingTariffGroups.TryRemove(ChargingTariffGroup.Id,
                                                   out var chargingTariffGroup,
                                                   EventTracking_Id.New,
                                                   null) &&
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


        #region ChargingSession(s)

        /// <summary>
        /// All charging sessions.
        /// </summary>
        public IEnumerable<ChargingSession> ChargingSessions

            => RoamingNetwork?.SessionsStore.Where(session => session.ChargingStationOperatorId == Id)
                   ?? [];

        #region ContainsChargingSessionId (ChargingSessionId)

        /// <summary>
        /// Whether the given charging session identification is known within the EVSE.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        public Boolean ContainsChargingSessionId(ChargingSession_Id ChargingSessionId)
        {

            if (RoamingNetwork is not null &&
                RoamingNetwork.TryGetChargingSessionById(ChargingSessionId, out var chargingSession))
            {
                return chargingSession.ChargingStationOperatorId == Id;
            }

            return false;

        }

        #endregion

        #region GetChargingSessionById    (ChargingSessionId)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        public ChargingSession? GetChargingSessionById(ChargingSession_Id ChargingSessionId)

            => RoamingNetwork?.GetChargingSessionById(ChargingSessionId);

        #endregion

        #region TryGetChargingSessionById (ChargingSessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id                        ChargingSessionId,
                                                 [NotNullWhen(true)] out ChargingSession?  ChargingSession)
        {

            if (RoamingNetwork is not null &&
                RoamingNetwork.TryGetChargingSessionById(ChargingSessionId, out var chargingSession) &&
                chargingSession.ChargingStationOperatorId == Id)
            {
                ChargingSession = chargingSession;
                return true;
            }

            ChargingSession = null;
            return false;

        }

        #endregion


        #region (internal) SendNewChargingSession   (Timestamp, Sender, Session)

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  Session)
        {

            if (Session is not null)
            {

                if (Session.ChargingStationOperator is null)
                {
                    Session.ChargingStationOperator    = this;
                    Session.ChargingStationOperatorId  = Id;
                }

                OnNewChargingSession?.Invoke(Timestamp, Sender, Session);

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

        public TimeSpan MaxReservationDuration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
                    Auth_Path?                         AuthenticationPath     = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<EMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null,
                    CancellationToken                  CancellationToken      = default)


                => Reserve(ChargingLocation.FromChargingStationOperatorId(Id),
                           ChargingReservationLevel.ChargingStationOperator,
                           StartTime,
                           Duration,
                           ReservationId,
                           LinkedReservationId,
                           ProviderId,
                           RemoteAuthentication,
                           AuthenticationPath,
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
                    Auth_Path?                         AuthenticationPath     = null,
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
                                                   AuthenticationPath,
                                                   ChargingProduct,
                                                   AuthTokens,
                                                   eMAIds,
                                                   PINs,

                                                   Timestamp,
                                                   EventTrackingId,
                                                   RequestTimeout,
                                                   CancellationToken);

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
                                                             EventTrackingId,
                                                             RequestTimeout,
                                                             CancellationToken);

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


        #region GetChargingReservationById    (ReservationId)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        public ChargingReservation? GetChargingReservationById(ChargingReservation_Id ReservationId)
        {

            if (RoamingNetwork is not null &&
                RoamingNetwork.ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                return reservationCollection?.LastOrDefault();
            }

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

            if (RoamingNetwork is not null &&
                RoamingNetwork.ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                return reservationCollection;
            }

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

            if (RoamingNetwork is not null &&
                RoamingNetwork.ReservationsStore.TryGet(ReservationId, out var reservationCollection))
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
        {

            if (RoamingNetwork is not null &&
                RoamingNetwork.ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                ChargingReservations = reservationCollection;
                return true;
            }

            ChargingReservations = null;
            return false;

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

        #region AuthorizeStart/-Stop

        #region Properties

        public IId      AuthId
            => RoamingNetwork.AuthId;

        /// <summary>
        /// Disable the local authorization of charging processes.
        /// </summary>
        public Boolean  DisableAuthorization    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever an AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate?   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate?  OnAuthorizeStartResponse;


        /// <summary>
        /// An event fired whenever an AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate?    OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate?   OnAuthorizeStopResponse;

        #endregion

        #region AuthorizeStart           (LocalAuthentication, ChargingLocation = null, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingLocation?            ChargingLocation      = null,
                           ChargingProduct?             ChargingProduct       = null,
                           ChargingSession_Id?          SessionId             = null,
                           ChargingSession_Id?          CPOPartnerSessionId   = null,
                           ChargingStationOperator_Id?  OperatorId            = null,

                           DateTime?                    RequestTimestamp      = null,
                           EventTracking_Id?            EventTrackingId       = null,
                           TimeSpan?                    RequestTimeout        = null,
                           CancellationToken            CancellationToken     = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;
            RequestTimeout   ??= TimeSpan.FromSeconds(10);

            AuthStartResult? result = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStartRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          LocalAuthentication,
                          ChargingLocation,
                          ChargingProduct,
                          SessionId,
                          CPOPartnerSessionId,
                          [],
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                result = RoamingNetwork is not null

                             ? await RoamingNetwork.AuthorizeStart(
                                         LocalAuthentication,
                                         ChargingLocation,
                                         ChargingProduct,
                                         SessionId,
                                         CPOPartnerSessionId,
                                         OperatorId,

                                         RequestTimestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     )

                             : AuthStartResult.OutOfService(
                                   Id,
                                   this,
                                   SessionId:  SessionId,
                                   Runtime:    Timestamp.Now - startTime
                               );

            }
            catch (Exception e)
            {

                result = AuthStartResult.Error(
                             Id,
                             this,
                             SessionId:    SessionId,
                             Description:  I18NString.Create(e.Message),
                             Runtime:      Timestamp.Now - startTime
                         );

            }


            #region Send OnAuthorizeStartResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStartResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          LocalAuthentication,
                          ChargingLocation,
                          ChargingProduct,
                          SessionId,
                          CPOPartnerSessionId,
                          [],
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop (SessionId, LocalAuthentication, ChargingLocation = null,                                           OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given location.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingLocation?            ChargingLocation      = null,
                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                          ChargingStationOperator_Id?  OperatorId            = null,

                          DateTime?                    RequestTimestamp      = null,
                          EventTracking_Id?            EventTrackingId       = null,
                          TimeSpan?                    RequestTimeout        = null,
                          CancellationToken            CancellationToken     = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;
            RequestTimeout   ??= TimeSpan.FromSeconds(10);

            AuthStopResult? result = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStopRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          ChargingLocation,
                          SessionId,
                          CPOPartnerSessionId,
                          LocalAuthentication,
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                result = RoamingNetwork is not null

                             ? await RoamingNetwork.AuthorizeStop(
                                         SessionId,
                                         LocalAuthentication,
                                         ChargingLocation,
                                         CPOPartnerSessionId,
                                         OperatorId,

                                         RequestTimestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     )

                             : AuthStopResult.OutOfService(
                                   Id,
                                   this,
                                   SessionId:  SessionId,
                                   Runtime:    Timestamp.Now - startTime
                               );

            }
            catch (Exception e)
            {

                result = AuthStopResult.Error(
                             SessionId,
                             this,
                             SessionId,
                             I18NString.Create(e.Message),
                             Timestamp.Now - startTime
                         );

            }


            #region Send OnAuthorizeStopResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStopResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          ChargingLocation,
                          SessionId,
                          CPOPartnerSessionId,
                          LocalAuthentication,
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions

        #region Events

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate?              OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate?             OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate?              OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate?               OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate?              OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate?           OnNewChargeDetailRecord;

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
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct?         ChargingProduct          = null,
                        ChargingReservation_Id?  ReservationId            = null,
                        ChargingSession_Id?      SessionId                = null,
                        EMobilityProvider_Id?    ProviderId               = null,
                        RemoteAuthentication?    RemoteAuthentication     = null,
                        JObject?                 AdditionalSessionInfos   = null,
                        Auth_Path?               AuthenticationPath       = null,

                        DateTime?                RequestTimestamp         = null,
                        EventTracking_Id?        EventTrackingId          = null,
                        TimeSpan?                RequestTimeout           = null,
                        CancellationToken        CancellationToken        = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;

            RemoteStartResult? result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStartRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          ChargingLocation,
                          RemoteAuthentication,
                          SessionId,
                          ReservationId,
                          ChargingProduct,
                          null,
                          null,
                          ProviderId,
                          RequestTimeout
                      )
                  );

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

                        result = await chargingPool.RemoteStart(
                                           ChargingLocation,
                                           ChargingProduct,
                                           ReservationId,
                                           SessionId,
                                           ProviderId,
                                           RemoteAuthentication,
                                           AdditionalSessionInfos,
                                           AuthenticationPath,

                                           RequestTimestamp,
                                           EventTrackingId,
                                           RequestTimeout,
                                           CancellationToken
                                       );

                        #region In case of success...

                        if (result.Result == RemoteStartResultTypes.Success ||
                            result.Result == RemoteStartResultTypes.AsyncOperation)
                        {

                            // The session can be delivered within the response
                            // or via an explicit message afterwards!
                            if (result.Session is not null)
                                result.Session.ChargingStationOperator = this;

                        }

                        #endregion

                    }

                    else
                        result = RemoteStartResult.UnknownLocation(
                                     System_Id.Local,
                                     Runtime: Timestamp.Now - startTime
                                 );

                }

                else
                    result = RemoteStartResult.OutOfService(
                                 System_Id.Local,
                                 Runtime: Timestamp.Now - startTime
                             );

            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(
                             System_Id.Local,
                             e.Message,
                             Runtime: Timestamp.Now - startTime
                         );
            }


            #region Send OnRemoteStartResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStartResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          ChargingLocation,
                          RemoteAuthentication,
                          SessionId,
                          ReservationId,
                          ChargingProduct,
                          null,
                          null,
                          ProviderId,
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

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
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       EMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,
                       Auth_Path?             AuthenticationPath     = null,

                       DateTime?              RequestTimestamp       = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null,
                       CancellationToken      CancellationToken      = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;

            RemoteStopResult? result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStopRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          SessionId,
                          ReservationHandling,
                          null,
                          null,
                          ProviderId,
                          RemoteAuthentication,
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationOperatorAdminStatusTypes.InternalUse)
                {

                    if (TryGetChargingSessionById(SessionId, out var chargingSession) &&
                       ((chargingSession.EVSEId.           HasValue && TryGetChargingPoolByEVSEId   (chargingSession.EVSEId.           Value, out var chargingPool)) ||
                        (chargingSession.ChargingStationId.HasValue && TryGetChargingPoolByStationId(chargingSession.ChargingStationId.Value, out     chargingPool)) ||
                        (chargingSession.ChargingPoolId.   HasValue && TryGetChargingPoolById       (chargingSession.ChargingPoolId.   Value, out     chargingPool))) &&
                        chargingPool is not null)
                    {

                        result = await chargingPool.RemoteStop(
                                           SessionId,
                                           ReservationHandling,
                                           ProviderId,
                                           RemoteAuthentication,
                                           AuthenticationPath,

                                           RequestTimestamp,
                                           EventTrackingId,
                                           RequestTimeout,
                                           CancellationToken
                                       );

                    }

                    result ??= RemoteStopResult.InvalidSessionId(
                                   SessionId,
                                   System_Id.Local,
                                   Runtime: Timestamp.Now - startTime
                               );

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
                    result = AdminStatus.Value switch {

                        _ => RemoteStopResult.OutOfService(
                                 SessionId,
                                 System_Id.Local,
                                 Runtime: Timestamp.Now - startTime
                             )

                    };

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(
                             SessionId,
                             System_Id.Local,
                             e.Message,
                             Runtime: Timestamp.Now - startTime
                         );
            }


            #region Send OnRemoteStopResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStopResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
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
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region SendNewChargeDetailRecord

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            if (ChargeDetailRecord is not null)
                OnNewChargeDetailRecord?.Invoke(Timestamp, Sender, ChargeDetailRecord);

        }

        #endregion

        #endregion


        #region ToJSON(Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given charging station operator.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        public JObject ToJSON(Boolean                                                     Embedded                                  = false,
                              InfoStatus                                                  ExpandRoamingNetworkId                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                                  ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                              InfoStatus                                                  ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                              InfoStatus                                                  ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                              InfoStatus                                                  ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                              InfoStatus                                                  ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<IChargingStationOperator>?  CustomChargingStationOperatorSerializer   = null,
                              CustomJObjectSerializerDelegate<IChargingPool>?             CustomChargingPoolSerializer              = null,
                              CustomJObjectSerializerDelegate<IChargingStation>?          CustomChargingStationSerializer           = null,
                              CustomJObjectSerializerDelegate<IEVSE>?                     CustomEVSESerializer                      = null,
                              CustomJObjectSerializerDelegate<ChargingConnector>?         CustomChargingConnectorSerializer         = null)
        {

            try
            {

                var json = JSONObject.Create(

                                     new JProperty("@id",                 Id.ToString()),

                               !Embedded
                                   ? new JProperty("@context",            JSONLDContext)
                                   : null,

                                     new JProperty("name",                Name.       ToJSON()),

                               Description.IsNotNullOrEmpty()
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
                                                                                  new JProperty("uri",          Logo.            ToString())
                                                                                  //new JProperty("description",  I18NString.Empty.ToJSON())
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
                                                                                                ToJSON (Embedded:                           true,
                                                                                                        ExpandRoamingNetworkId:             InfoStatus.Hidden,
                                                                                                        ExpandChargingStationOperatorId:    InfoStatus.Hidden,
                                                                                                        ExpandChargingStationIds:           InfoStatus.Expanded,
                                                                                                        ExpandEVSEIds:                      InfoStatus.Expanded,
                                                                                                        ExpandBrandIds:                     InfoStatus.ShowIdOnly,
                                                                                                        ExpandDataLicenses:                 InfoStatus.Hidden,
                                                                                                        CustomChargingPoolSerializer:       CustomChargingPoolSerializer,
                                                                                                        CustomChargingStationSerializer:    CustomChargingStationSerializer,
                                                                                                        CustomEVSESerializer:               CustomEVSESerializer,
                                                                                                        CustomChargingConnectorSerializer:  CustomChargingConnectorSerializer))))
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

                return CustomChargingStationOperatorSerializer is not null
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


        #region (private) LogEvent(Logger, LogHandler, ...)

        private Task LogEvent<TDelegate>(TDelegate?                                         Logger,
                                         Func<TDelegate, Task>                              LogHandler,
                                         [CallerArgumentExpression(nameof(Logger))] String  EventName   = "",
                                         [CallerMemberName()]                       String  Command     = "")

            where TDelegate : Delegate

                => LogEvent(
                       nameof(ChargingStationOperator),
                       Logger,
                       LogHandler,
                       EventName,
                       Command
                   );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperator1, ChargingStationOperator2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperator1">A charging station operator.</param>
        /// <param name="ChargingStationOperator2">Another charging station operator.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationOperator ChargingStationOperator1,
                                           ChargingStationOperator ChargingStationOperator2)
        {

            if (Object.ReferenceEquals(ChargingStationOperator1, ChargingStationOperator2))
                return true;

            if (ChargingStationOperator1 is null || ChargingStationOperator2 is null)
                return false;

            return ChargingStationOperator1.Equals(ChargingStationOperator2);

        }

        #endregion

        #region Operator != (ChargingStationOperator1, ChargingStationOperator2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperator1">A charging station operator.</param>
        /// <param name="ChargingStationOperator2">Another charging station operator.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationOperator ChargingStationOperator1,
                                           ChargingStationOperator ChargingStationOperator2)

            => !(ChargingStationOperator1 == ChargingStationOperator2);

        #endregion

        #region Operator <  (ChargingStationOperator1, ChargingStationOperator2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperator1">A charging station operator.</param>
        /// <param name="ChargingStationOperator2">Another charging station operator.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationOperator ChargingStationOperator1,
                                          ChargingStationOperator ChargingStationOperator2)

            => ChargingStationOperator1 is null
                   ? throw new ArgumentNullException(nameof(ChargingStationOperator1), "The given charging station operator must not be null!")
                   : ChargingStationOperator1.CompareTo(ChargingStationOperator2) < 0;

        #endregion

        #region Operator <= (ChargingStationOperator1, ChargingStationOperator2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperator1">A charging station operator.</param>
        /// <param name="ChargingStationOperator2">Another charging station operator.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationOperator ChargingStationOperator1,
                                           ChargingStationOperator ChargingStationOperator2)

            => !(ChargingStationOperator1 > ChargingStationOperator2);

        #endregion

        #region Operator >  (ChargingStationOperator1, ChargingStationOperator2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperator1">A charging station operator.</param>
        /// <param name="ChargingStationOperator2">Another charging station operator.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationOperator ChargingStationOperator1,
                                          ChargingStationOperator ChargingStationOperator2)

            => ChargingStationOperator1 is null
                   ? throw new ArgumentNullException(nameof(ChargingStationOperator1), "The given charging station operator must not be null!")
                   : ChargingStationOperator1.CompareTo(ChargingStationOperator2) > 0;

        #endregion

        #region Operator >= (ChargingStationOperator1, ChargingStationOperator2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperator1">A charging station operator.</param>
        /// <param name="ChargingStationOperator2">Another charging station operator.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationOperator ChargingStationOperator1,
                                           ChargingStationOperator ChargingStationOperator2)

            => !(ChargingStationOperator1 < ChargingStationOperator2);

        #endregion

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
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

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
        /// <returns>True if both match; False otherwise.</returns>
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

            => String.Concat(
                   "'",
                   Name.FirstText(),
                   "' (",
                   Id.ToString(),
                   ") in ",
                   RoamingNetwork.Id.ToString()
               );

        #endregion

    }

}
