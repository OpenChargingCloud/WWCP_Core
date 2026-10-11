/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

using Newtonsoft.Json.Linq;

using cloud.charging.open.protocols.WWCP.POI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate called whenever the admin status changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
    /// <param name="ChargingPoolGroup">The updated charging pool group.</param>
    /// <param name="NewStatus">The new timestamped admin status of the charging pool group.</param>
    /// <param name="OldStatus">The old timestamped admin status of the charging pool group.</param>
    /// <param name="DataSource">An optional data source or context for the admin status update.</param>
    public delegate Task OnChargingPoolGroupAdminStatusChangedDelegate(DateTimeOffset                                   Timestamp,
                                                                       EventTracking_Id                                 EventTrackingId,
                                                                       ChargingPoolGroup                                ChargingPoolGroup,
                                                                       Timestamped<ChargingPoolGroupAdminStatusType>    NewStatus,
                                                                       Timestamped<ChargingPoolGroupAdminStatusType>?   OldStatus    = null,
                                                                       Context?                                         DataSource   = null);

    /// <summary>
    /// A delegate called whenever the status changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
    /// <param name="ChargingPoolGroup">The updated charging pool group.</param>
    /// <param name="NewStatus">The new timestamped status of the charging pool group.</param>
    /// <param name="OldStatus">The old timestamped status of the charging pool group.</param>
    /// <param name="DataSource">An optional data source or context for the status update.</param>
    public delegate Task OnChargingPoolGroupStatusChangedDelegate(DateTimeOffset                              Timestamp,
                                                                  EventTracking_Id                            EventTrackingId,
                                                                  ChargingPoolGroup                           ChargingPoolGroup,
                                                                  Timestamped<ChargingPoolGroupStatusType>    NewStatus,
                                                                  Timestamped<ChargingPoolGroupStatusType>?   OldStatus    = null,
                                                                  Context?                                    DataSource   = null);


    public class AutoIncludeChargingPoolMemberIds
    {

        private List<ChargingPool_Id> allowedMemberIds;

        public AutoIncludeChargingPoolMemberIds(IEnumerable<ChargingPool_Id> AllowedMemberIds)
        {

            this.allowedMemberIds = [.. AllowedMemberIds];

        }

        public Boolean Allowed(ChargingPool_Id PoolId)
            => allowedMemberIds.Contains(PoolId);


    }


    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this ChargingPoolGroup,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging pool group.
        /// </summary>
        /// <param name="ChargingPoolGroup">A charging pool group.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JObject? ToJSON(this ChargingPoolGroup?  ChargingPoolGroup,
                                      Boolean                  Embedded                          = false,
                                      InfoStatus               ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                      InfoStatus               ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                      InfoStatus               ExpandChargingPoolIds             = InfoStatus.ShowIdOnly,
                                      InfoStatus               ExpandEVSEIds                     = InfoStatus.ShowIdOnly,
                                      InfoStatus               ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                      InfoStatus               ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingPoolGroup is null

                   ? null

                   : JSONObject.Create(

                         new JProperty("@id", ChargingPoolGroup.Id.ToString()),

                         Embedded
                             ? null
                             : new JProperty("@context", "https://open.charging.cloud/contexts/wwcp+json/ChargingPoolGroup"),

                         ChargingPoolGroup.Name.       IsNotNullOrEmpty()
                             ? new JProperty("name",        ChargingPoolGroup.Name.ToJSON())
                             : null,

                         ChargingPoolGroup.Description.IsNotNullOrEmpty()
                             ? new JProperty("description", ChargingPoolGroup.Description.ToJSON())
                             : null,

                         ChargingPoolGroup.Brand is not null
                             ? ExpandBrandIds.Switch(
                                   () => new JProperty("brandId",  ChargingPoolGroup.Brand.Id.ToString()),
                                   () => new JProperty("brand",    ChargingPoolGroup.Brand.   ToJSON()))
                             : null,

                         (!Embedded || ChargingPoolGroup.DataSource != ChargingPoolGroup.Operator.DataSource)
                             ? new JProperty("dataSource", ChargingPoolGroup.DataSource)
                             : null,

                         #region Embedded means it is served as a substructure of e.g. a charging station operator

                         Embedded || ChargingPoolGroup.RoamingNetwork is not IRoamingNetwork roamingNetwork
                             ? null
                             : ExpandRoamingNetworkId.Switch(
                                   () => new JProperty("roamingNetworkId",           roamingNetwork.Id. ToString()),
                                   () => new JProperty("roamingNetwork",             roamingNetwork.    ToJSON(Embedded:                          true,
                                                                                                                                    ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
                                                                                                                                    ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                                    ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                                    ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                                    ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                                    ExpandDataLicenses:                InfoStatus.Hidden))),

                         Embedded
                             ? null
                             : ExpandChargingStationOperatorId.Switch(
                                   () => new JProperty("chargingStationOperatorId",  ChargingPoolGroup.Operator.Id.       ToString()),
                                   () => new JProperty("chargingStationOperator",    ChargingPoolGroup.Operator.          ToJSON(Embedded:                          true,
                                                                                                                                    ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                                    ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                                    ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                                    ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                                    ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                                    ExpandDataLicenses:                InfoStatus.Hidden))),

                         #endregion

                         ChargingPoolGroup.ChargingPools.Any()
                             ? ExpandChargingPoolIds.Switch(
                                   () => new JProperty("chargingPoolIds",  new JArray(ChargingPoolGroup.ChargingPoolIds.
                                                                                          Order().
                                                                                          Select(poolId => poolId.ToString()))),
                                   () => new JProperty("chargingPools",    new JArray(ChargingPoolGroup.ChargingPools.
                                                                                          OrderBy(pool => pool.Id).
                                                                                          Select (pool => pool.ToJSON(Embedded: true)))))
                             : null,

                         ChargingPoolGroup.EVSEs.Any()
                             ? ExpandEVSEIds.Switch(
                                   () => new JProperty("EVSEIds",          new JArray(ChargingPoolGroup.EVSEIds.
                                                                                          Order().
                                                                                          Select(evseId => evseId.ToString()))),
                                   () => new JProperty("EVSEs",            new JArray(ChargingPoolGroup.EVSEs.
                                                                                          OrderBy(evse => evse.Id).
                                                                                          Select (evse => evse.ToJSON(Embedded: true)))))
                             : null

                        );

        #endregion

        #region ToJSON(this ChargingPoolGroups, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging pool groups.
        /// </summary>
        /// <param name="ChargingPoolGroups">An enumeration of charging pool groups.</param>
        /// <param name="Skip">The optional number of charging pool groups to skip.</param>
        /// <param name="Take">The optional number of charging pool groups to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JArray ToJSON(this IEnumerable<ChargingPoolGroup>?  ChargingPoolGroups,
                                    UInt64?                               Skip                              = null,
                                    UInt64?                               Take                              = null,
                                    Boolean                               Embedded                          = false,
                                    InfoStatus                            ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                            ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                            ExpandChargingPoolIds             = InfoStatus.ShowIdOnly,
                                    InfoStatus                            ExpandEVSEIds                     = InfoStatus.ShowIdOnly,
                                    InfoStatus                            ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                            ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingPoolGroups is not null && ChargingPoolGroups.Any()

                   ? new JArray(ChargingPoolGroups.
                                    Where     (poolGroup => poolGroup is not null).
                                    OrderBy   (poolGroup => poolGroup.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(poolGroup => poolGroup.ToJSON(Embedded,
                                                                             ExpandRoamingNetworkId,
                                                                             ExpandChargingStationOperatorId,
                                                                             ExpandChargingPoolIds,
                                                                             ExpandEVSEIds,
                                                                             ExpandBrandIds,
                                                                             ExpandDataLicenses)))

                   : [];

        #endregion

        #region ToJSON(this ChargingPoolGroups, JPropertyKey)

        public static JProperty? ToJSON(this IEnumerable<ChargingPoolGroup>? ChargingPoolGroups, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingPoolGroups?.Any() == true
                       ? new JProperty(JPropertyKey, ChargingPoolGroups.ToJSON())
                       : null;

        }

        #endregion

    }


    /// <summary>
    /// A group of charging pools of one charging station operator, e.g. all
    /// charging pools of a city or all charging pools sharing a tariff.
    /// </summary>
    public class ChargingPoolGroup : AEMobilityEntity<ChargingPoolGroup_Id,
                                                      ChargingPoolGroupAdminStatusType,
                                                      ChargingPoolGroupStatusType>,
                                     IEquatable<ChargingPoolGroup>, IComparable<ChargingPoolGroup>, IComparable,
                                     IEnumerable<IChargingPool>
    {

        #region Data

        /// <summary>
        /// The default max size of the charging pool group status list.
        /// </summary>
        public const UInt16 DefaultMaxGroupStatusListSize       = 15;

        /// <summary>
        /// The default max size of the charging pool group admin status list.
        /// </summary>
        public const UInt16 DefaultMaxGroupAdminStatusListSize  = 15;

        private readonly HashSet<ChargingPool_Id>                             allowedMemberIds;
        private readonly ConcurrentDictionary<ChargingPool_Id, IChargingPool>  chargingPools;

        #endregion

        #region Properties

        /// <summary>
        /// An optional (multi-language) brand name for this group.
        /// </summary>
        [Optional]
        public Brand?                     Brand           { get; }

        /// <summary>
        /// The priority of this group relative to all other groups.
        /// </summary>
        [Optional]
        public Priority?                  Priority        { get; }

        /// <summary>
        /// An optional charging tariff.
        /// </summary>
        [Optional]
        public ChargingTariff?            Tariff          { get; }

        /// <summary>
        /// The licenses of the group data.
        /// </summary>
        [Mandatory]
        public IEnumerable<DataLicense>   DataLicenses    { get; }

        /// <summary>
        /// The identifications of all charging pools which may be members of this group,
        /// whatever AutoIncludePools says.
        /// </summary>
        public IEnumerable<ChargingPool_Id> AllowedMemberIds
        {
            get
            {
                lock (allowedMemberIds)
                {
                    return [.. allowedMemberIds];
                }
            }
        }

        /// <summary>
        /// A delegate deciding whether to include a charging pool that is not
        /// in AllowedMemberIds into this group.
        /// </summary>
        public Func<IChargingPool, Boolean>  AutoIncludePools    { get; }

        #region ChargingPools

        /// <summary>
        /// All charging pools of this group.
        /// </summary>
        public IEnumerable<IChargingPool> ChargingPools
            => chargingPools.Values;

        /// <summary>
        /// The identifications of all charging pools of this group.
        /// </summary>
        public IEnumerable<ChargingPool_Id> ChargingPoolIds
            => chargingPools.Keys;

        /// <summary>
        /// All charging stations of the charging pools of this group.
        /// </summary>
        public IEnumerable<IChargingStation> ChargingStations
            => ChargingPools.SelectMany(pool => pool.ChargingStations);

        /// <summary>
        /// The identifications of all charging stations of the charging pools of this group.
        /// </summary>
        public IEnumerable<ChargingStation_Id> ChargingStationIds
            => ChargingStations.Select(station => station.Id);

        /// <summary>
        /// All EVSEs of the charging pools of this group.
        /// </summary>
        public IEnumerable<IEVSE> EVSEs
            => ChargingPools.SelectMany(pool => pool.EVSEs);

        /// <summary>
        /// The identifications of all EVSEs of the charging pools of this group.
        /// </summary>
        public IEnumerable<EVSE_Id> EVSEIds
            => EVSEs.Select(evse => evse.Id);

        #endregion

        /// <summary>
        /// An optional delegate to aggregate the dynamic status of all charging pools of this group.
        /// </summary>
        public Func<ChargingPoolStatusReport, ChargingPoolGroupStatusType>?   StatusAggregationDelegate    { get; }

        #endregion

        #region Links

        /// <summary>
        /// The charging station operator of this charging pool group.
        /// </summary>
        [Mandatory]
        public IChargingStationOperator  Operator    { get; }

        /// <summary>
        /// The roaming network of this charging pool group.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork?          RoamingNetwork
            => Operator.RoamingNetwork;

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever the admin status of this group changed.
        /// </summary>
        public event OnChargingPoolGroupAdminStatusChangedDelegate?  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the static data of any charging pool of this group changed.
        /// </summary>
        public event OnChargingPoolDataChangedDelegate?              OnChargingPoolDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any charging pool of this group changed.
        /// </summary>
        public event OnChargingPoolStatusChangedDelegate?            OnChargingPoolStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any charging pool of this group changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate?       OnChargingPoolAdminStatusChanged;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool group.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool group.</param>
        /// <param name="Operator">The charging station operator of this charging pool group.</param>
        /// <param name="Name">The official (multi-language) name of this charging pool group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging pool group.</param>
        ///
        /// <param name="Brand">An optional brand of this charging pool group.</param>
        /// <param name="Priority">An optional priority of this group relative to all other groups.</param>
        /// <param name="Tariff">An optional charging tariff of this charging pool group.</param>
        /// <param name="DataLicenses">The licenses of the group data.</param>
        ///
        /// <param name="Members">Charging pools which are members of this group from its start.</param>
        /// <param name="MemberIds">The identifications of all charging pools which may be members of this group.</param>
        /// <param name="AutoIncludePools">A delegate deciding whether to include a charging pool that is not in MemberIds; by default every charging pool, unless Members or MemberIds are given.</param>
        ///
        /// <param name="StatusAggregationDelegate">An optional delegate to aggregate the dynamic status of all charging pools of this group.</param>
        /// <param name="MaxGroupStatusListSize">The max size of the charging pool group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The max size of the charging pool group admin status list.</param>
        internal ChargingPoolGroup(ChargingPoolGroup_Id                                           Id,
                                   IChargingStationOperator                                       Operator,
                                   I18NString                                                     Name,
                                   I18NString?                                                    Description                   = null,

                                   Brand?                                                         Brand                         = null,
                                   Priority?                                                      Priority                      = null,
                                   ChargingTariff?                                                Tariff                        = null,
                                   IEnumerable<DataLicense>?                                      DataLicenses                  = null,

                                   IEnumerable<IChargingPool>?                                    Members                       = null,
                                   IEnumerable<ChargingPool_Id>?                                  MemberIds                     = null,
                                   Func<IChargingPool, Boolean>?                                  AutoIncludePools              = null,

                                   Func<ChargingPoolStatusReport, ChargingPoolGroupStatusType>?   StatusAggregationDelegate     = null,
                                   UInt16                                                         MaxGroupStatusListSize        = DefaultMaxGroupStatusListSize,
                                   UInt16                                                         MaxGroupAdminStatusListSize   = DefaultMaxGroupAdminStatusListSize)

            : base(Id,
                   Name,
                   Description,
                   MaxAdminStatusScheduleSize:  MaxGroupAdminStatusListSize,
                   MaxStatusScheduleSize:       MaxGroupStatusListSize)

        {

            #region Initial checks

            if (Operator is null)
                throw new ArgumentNullException(nameof(Operator),  "The charging station operator must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),      "The name of the charging pool group must not be null or empty!");

            #endregion

            this.Operator                   = Operator;

            this.Brand                      = Brand;
            this.Priority                   = Priority;
            this.Tariff                     = Tariff;
            this.DataLicenses               = DataLicenses?.Distinct().ToArray() ?? [];

            this.allowedMemberIds           = [.. MemberIds ?? []];
            this.AutoIncludePools           = AutoIncludePools ?? (Members is null && MemberIds is null
                                                                       ? pool => true
                                                                       : pool => false);
            this.chargingPools              = new ConcurrentDictionary<ChargingPool_Id, IChargingPool>();

            this.StatusAggregationDelegate  = StatusAggregationDelegate;

            this.adminStatusSchedule.OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                            => UpdateAdminStatus(timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            foreach (var member in Members ?? [])
            {

                lock (allowedMemberIds)
                {
                    allowedMemberIds.Add(member.Id);
                }

                chargingPools.TryAdd(member.Id, member);

            }

        }

        #endregion


        #region Add(ChargingPool)

        /// <summary>
        /// Add the given charging pool to this group, when its identification
        /// is an allowed member identification or AutoIncludePools says so.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <returns>Whether the charging pool is a member of this group now.</returns>
        public Boolean Add(IChargingPool ChargingPool)
        {

            Boolean allowed;

            lock (allowedMemberIds)
            {
                allowed = allowedMemberIds.Contains(ChargingPool.Id);
            }

            if (allowed || AutoIncludePools(ChargingPool))
            {
                chargingPools.TryAdd(ChargingPool.Id, ChargingPool);
                return true;
            }

            return false;

        }

        #endregion

        #region Add(ChargingPoolId)

        /// <summary>
        /// Allow the charging pool having the given identification to become a member of this group.
        /// </summary>
        /// <param name="ChargingPoolId">The identification of a charging pool.</param>
        public ChargingPoolGroup Add(ChargingPool_Id ChargingPoolId)
        {

            lock (allowedMemberIds)
            {
                allowedMemberIds.Add(ChargingPoolId);
            }

            return this;

        }

        #endregion

        #region Remove(ChargingPoolId)

        /// <summary>
        /// Remove the charging pool having the given identification from this group,
        /// and from its allowed member identifications.
        /// </summary>
        /// <param name="ChargingPoolId">The identification of a charging pool.</param>
        /// <returns>Whether the charging pool was a member of this group.</returns>
        public Boolean Remove(ChargingPool_Id ChargingPoolId)
        {

            lock (allowedMemberIds)
            {
                allowedMemberIds.Remove(ChargingPoolId);
            }

            return chargingPools.TryRemove(ChargingPoolId, out _);

        }

        #endregion

        #region Contains(ChargingPoolId)

        /// <summary>
        /// Whether the charging pool having the given identification is a member of this group.
        /// </summary>
        /// <param name="ChargingPoolId">The identification of a charging pool.</param>
        public Boolean Contains(ChargingPool_Id ChargingPoolId)
            => chargingPools.ContainsKey(ChargingPoolId);

        #endregion


        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update the current admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="NewStatus">The new charging pool group admin status.</param>
        /// <param name="OldStatus">The old charging pool group admin status.</param>
        /// <param name="DataSource">An optional data source or context for the admin status update.</param>
        internal async Task UpdateAdminStatus(DateTimeOffset                                   Timestamp,
                                              EventTracking_Id                                 EventTrackingId,
                                              Timestamped<ChargingPoolGroupAdminStatusType>    NewStatus,
                                              Timestamped<ChargingPoolGroupAdminStatusType>?   OldStatus    = null,
                                              Context?                                         DataSource   = null)
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

        #region (internal) UpdateChargingPoolData       (Timestamp, EventTrackingId, ChargingPool, PropertyName, NewValue, OldValue = null, DataSource = null)

        /// <summary>
        /// Update the data of a charging pool of this group.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        internal async Task UpdateChargingPoolData(DateTimeOffset    Timestamp,
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

        #region (internal) UpdateChargingPoolAdminStatus(Timestamp, EventTrackingId, ChargingPool, OldStatus, NewStatus = null, DataSource = null)

        /// <summary>
        /// Update the admin status of a charging pool of this group.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old charging pool admin status.</param>
        /// <param name="NewStatus">The new charging pool admin status.</param>
        /// <param name="DataSource">An optional data source or context for the admin status update.</param>
        internal async Task UpdateChargingPoolAdminStatus(DateTimeOffset                             Timestamp,
                                                          EventTracking_Id                           EventTrackingId,
                                                          IChargingPool                              ChargingPool,
                                                          Timestamped<ChargingPoolAdminStatusType>   OldStatus,
                                                          Timestamped<ChargingPoolAdminStatusType>?  NewStatus    = null,
                                                          Context?                                   DataSource   = null)
        {

            var onChargingPoolAdminStatusChanged = OnChargingPoolAdminStatusChanged;
            if (onChargingPoolAdminStatusChanged is not null)
                await onChargingPoolAdminStatusChanged(Timestamp,
                                                       EventTrackingId,
                                                       ChargingPool,
                                                       OldStatus,
                                                       NewStatus,
                                                       DataSource);

        }

        #endregion

        #region (internal) UpdateChargingPoolStatus     (Timestamp, EventTrackingId, ChargingPool, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update the dynamic status of a charging pool of this group.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="NewStatus">The new charging pool status.</param>
        /// <param name="OldStatus">The old charging pool status.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        internal async Task UpdateChargingPoolStatus(DateTimeOffset                        Timestamp,
                                                     EventTracking_Id                      EventTrackingId,
                                                     IChargingPool                         ChargingPool,
                                                     Timestamped<ChargingPoolStatusType>   NewStatus,
                                                     Timestamped<ChargingPoolStatusType>?  OldStatus    = null,
                                                     Context?                              DataSource   = null)
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


        #region IEnumerable<IChargingPool> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => ChargingPools.GetEnumerator();

        public IEnumerator<IChargingPool> GetEnumerator()
            => ChargingPools.GetEnumerator();

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging pool group.</param>
        /// <param name="ChargingPoolGroup2">Another charging pool group.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingPoolGroup? ChargingPoolGroup1,
                                           ChargingPoolGroup? ChargingPoolGroup2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingPoolGroup1, ChargingPoolGroup2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingPoolGroup1 is null || ChargingPoolGroup2 is null)
                return false;

            return ChargingPoolGroup1.Equals(ChargingPoolGroup2);

        }

        #endregion

        #region Operator != (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging pool group.</param>
        /// <param name="ChargingPoolGroup2">Another charging pool group.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingPoolGroup? ChargingPoolGroup1,
                                           ChargingPoolGroup? ChargingPoolGroup2)

            => !(ChargingPoolGroup1 == ChargingPoolGroup2);

        #endregion

        #region Operator <  (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging pool group.</param>
        /// <param name="ChargingPoolGroup2">Another charging pool group.</param>
        /// <returns>True if ChargingPoolGroup1 comes first; False otherwise.</returns>
        public static Boolean operator < (ChargingPoolGroup? ChargingPoolGroup1,
                                          ChargingPoolGroup? ChargingPoolGroup2)

            => ChargingPoolGroup1 is null
                   ? throw new ArgumentNullException(nameof(ChargingPoolGroup1), "The given charging pool group must not be null!")
                   : ChargingPoolGroup1.CompareTo(ChargingPoolGroup2) < 0;

        #endregion

        #region Operator <= (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging pool group.</param>
        /// <param name="ChargingPoolGroup2">Another charging pool group.</param>
        /// <returns>True if ChargingPoolGroup1 does not come after ChargingPoolGroup2; False otherwise.</returns>
        public static Boolean operator <= (ChargingPoolGroup? ChargingPoolGroup1,
                                           ChargingPoolGroup? ChargingPoolGroup2)

            => !(ChargingPoolGroup1 > ChargingPoolGroup2);

        #endregion

        #region Operator >  (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging pool group.</param>
        /// <param name="ChargingPoolGroup2">Another charging pool group.</param>
        /// <returns>True if ChargingPoolGroup1 comes after ChargingPoolGroup2; False otherwise.</returns>
        public static Boolean operator > (ChargingPoolGroup? ChargingPoolGroup1,
                                          ChargingPoolGroup? ChargingPoolGroup2)

            => ChargingPoolGroup1 is null
                   ? throw new ArgumentNullException(nameof(ChargingPoolGroup1), "The given charging pool group must not be null!")
                   : ChargingPoolGroup1.CompareTo(ChargingPoolGroup2) > 0;

        #endregion

        #region Operator >= (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging pool group.</param>
        /// <param name="ChargingPoolGroup2">Another charging pool group.</param>
        /// <returns>True if ChargingPoolGroup1 does not come first; False otherwise.</returns>
        public static Boolean operator >= (ChargingPoolGroup? ChargingPoolGroup1,
                                           ChargingPoolGroup? ChargingPoolGroup2)

            => !(ChargingPoolGroup1 < ChargingPoolGroup2);

        #endregion

        #endregion

        #region IComparable<ChargingPoolGroup> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">A charging pool group to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is ChargingPoolGroup chargingPoolGroup
                   ? CompareTo(chargingPoolGroup)
                   : throw new ArgumentException("The given object is not a charging pool group!", nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolGroup)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup">A charging pool group to compare with.</param>
        public Int32 CompareTo(ChargingPoolGroup? ChargingPoolGroup)

            => ChargingPoolGroup is null
                   ? throw new ArgumentNullException(nameof(ChargingPoolGroup), "The given charging pool group must not be null!")
                   : Id.CompareTo(ChargingPoolGroup.Id);

        #endregion

        #endregion

        #region IEquatable<ChargingPoolGroup> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">A charging pool group to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolGroup chargingPoolGroup &&
                   Equals(chargingPoolGroup);

        #endregion

        #region Equals(ChargingPoolGroup)

        /// <summary>
        /// Compares two charging pool groups for equality.
        /// </summary>
        /// <param name="ChargingPoolGroup">A charging pool group to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolGroup? ChargingPoolGroup)

            => ChargingPoolGroup is not null &&
                   Id.Equals(ChargingPoolGroup.Id);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(Id, ", ", Name.FirstText());

        #endregion

    }

}
