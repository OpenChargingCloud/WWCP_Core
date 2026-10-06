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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate called whenever the admin status changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingPoolGroup">The updated charging pool.</param>
    /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
    /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
    public delegate Task OnChargingPoolGroupAdminStatusChangedDelegate(DateTimeOffset                                   Timestamp,
                                                                       EventTracking_Id                                 EventTrackingId,
                                                                       ChargingPoolGroup                                ChargingPoolGroup,
                                                                       Timestamped<ChargingPoolGroupAdminStatusTypes>   NewStatus,
                                                                       Timestamped<ChargingPoolGroupAdminStatusTypes>?  OldStatus    = null,
                                                                       Context?                                         DataSource   = null);

    /// <summary>
    /// A delegate called whenever the status changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingPoolGroup">The updated charging pool.</param>
    /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
    /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
    public delegate Task OnChargingPoolGroupStatusChangedDelegate(DateTimeOffset                              Timestamp,
                                                                  EventTracking_Id                            EventTrackingId,
                                                                  ChargingPoolGroup                           ChargingPoolGroup,
                                                                  Timestamped<ChargingPoolGroupStatusTypes>   NewStatus,
                                                                  Timestamped<ChargingPoolGroupStatusTypes>?  OldStatus    = null,
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
        /// Return a JSON representation of the given charging station group.
        /// </summary>
        /// <param name="ChargingPoolGroup">A charging station group.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JObject? ToJSON(this ChargingPoolGroup  ChargingPoolGroup,
                                      Boolean                    Embedded                          = false,
                                      InfoStatus                 ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                      InfoStatus                 ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                      InfoStatus                 ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                      InfoStatus                 ExpandEVSEIds                     = InfoStatus.Expanded,
                                      InfoStatus                 ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                      InfoStatus                 ExpandDataLicenses                = InfoStatus.ShowIdOnly)


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

                         //(!Embedded || ChargingPoolGroup.DataLicenses != ChargingPoolGroup.Operator.DataLicenses)
                         //    ? ExpandDataLicenses.Switch(
                         //        () => new JProperty("dataLicenseIds",  new JArray(ChargingPoolGroup.DataLicenses.SafeSelect(license => license.Id.ToString()))),
                         //        () => new JProperty("dataLicenses",    ChargingPoolGroup.DataLicenses.ToJSON()))
                         //    : null,

                         #region Embedded means it is served as a substructure of e.g. a charging station operator

                         Embedded
                             ? null
                             : ExpandRoamingNetworkId.Switch(
                                   () => new JProperty("roamingNetworkId",           ChargingPoolGroup.RoamingNetwork.Id. ToString()),
                                   () => new JProperty("roamingNetwork",             ChargingPoolGroup.RoamingNetwork.    ToJSON(Embedded:                          true,
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

                         //(!Embedded || ChargingStation.GeoLocation         != ChargingStation.ChargingPool.GeoLocation)         ? ChargingStation.GeoLocation.Value.  ToJSON("geoLocation")         : null,
                         //(!Embedded || ChargingStation.Address             != ChargingStation.ChargingPool.Address)             ? ChargingStation.Address.            ToJSON("address")             : null,
                         //(!Embedded || ChargingStation.AuthenticationModes != ChargingStation.ChargingPool.AuthenticationModes) ? ChargingStation.AuthenticationModes.ToJSON("authenticationModes") : null,
                         //(!Embedded || ChargingStation.HotlinePhoneNumber  != ChargingStation.ChargingPool.HotlinePhoneNumber)  ? ChargingStation.HotlinePhoneNumber. ToJSON("hotlinePhoneNumber")  : null,
                         //(!Embedded || ChargingStation.OpeningTimes        != ChargingStation.ChargingPool.OpeningTimes)        ? ChargingStation.OpeningTimes.       ToJSON("openingTimes")        : null,

                         ExpandEVSEIds.Switch(
                             () => new JProperty("EVSEIds",
                                                 ChargingPoolGroup.EVSEIds.SafeAny()
                                                     ? new JArray(ChargingPoolGroup.EVSEIds.
                                                                                       OrderBy(evseid => evseid).
                                                                                       Select (evseid => evseid.ToString()))
                                                     : null),

                             () => new JProperty("EVSEs",
                                                 ChargingPoolGroup.EVSEs.SafeAny()
                                                     ? new JArray(ChargingPoolGroup.EVSEs.
                                                                                       OrderBy(evse   => evse.Id).
                                                                                       Select (evse   => evse.  ToJSON(Embedded: true)))
                                                     : null))

                        );

        #endregion

        #region ToJSON(this ChargingPoolGroups, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingPoolGroups">An enumeration of charging station groups.</param>
        /// <param name="Skip">The optional number of charging stations to skip.</param>
        /// <param name="Take">The optional number of charging stations to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static JArray ToJSON(this IEnumerable<ChargingPoolGroup>  ChargingPoolGroups,
                                    UInt64?                                 Skip                              = null,
                                    UInt64?                                 Take                              = null,
                                    Boolean                                 Embedded                          = false,
                                    InfoStatus                              ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandEVSEIds                     = InfoStatus.Expanded,
                                    InfoStatus                              ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingPoolGroups is not null && ChargingPoolGroups.Any()

                   ? new JArray(ChargingPoolGroups.
                                    Where     (stationgroup => stationgroup is not null).
                                    OrderBy   (stationgroup => stationgroup.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(stationgroup => stationgroup.ToJSON(Embedded,
                                                                                   ExpandRoamingNetworkId,
                                                                                   ExpandChargingStationOperatorId,
                                                                                   ExpandChargingPoolId,
                                                                                   ExpandEVSEIds,
                                                                                   ExpandBrandIds,
                                                                                   ExpandDataLicenses)))

                   : null;

        #endregion

        #region ToJSON(this ChargingPoolGroups, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingPoolGroup> ChargingPoolGroups, String JPropertyKey)
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
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging pool
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingPoolGroup : AEMobilityEntity<ChargingPoolGroup_Id,
                                                      ChargingPoolGroupAdminStatusTypes,
                                                      ChargingPoolGroupStatusTypes>,
                                     IEquatable<ChargingPoolGroup>, IComparable<ChargingPoolGroup>, IComparable,
                                     IEnumerable<ChargingStation>
    {

        #region Data

        /// <summary>
        /// The default max size of the charging station group status list.
        /// </summary>
        public const UInt16 DefaultMaxGroupStatusListSize       = 15;

        /// <summary>
        /// The default max size of the charging station group admin status list.
        /// </summary>
        public const UInt16 DefaultMaxGroupAdminStatusListSize  = 15;

        #endregion

        #region Properties

        /// <summary>
        /// An optional (multi-language) brand name for this group.
        /// </summary>
        [Optional]
        public Brand                    Brand          { get; }

        /// <summary>
        /// The priority of this group relative to all other groups.
        /// </summary>
        public Priority?                Priority       { get; }

        /// <summary>
        /// An optional charging tariff.
        /// </summary>
        [Optional]
        public ChargingTariff?          Tariff         { get; }


        /// <summary>
        /// The license of the group data.
        /// </summary>
        [Mandatory]
        public IEnumerable<DataLicense> DataLicenses { get; }



        private HashSet<ChargingStation_Id> _AllowedMemberIds;

        public IEnumerable<ChargingStation_Id> AllowedMemberIds
            => _AllowedMemberIds;

        public Func<ChargingStation, Boolean> AutoIncludeStations { get; }


        public ChargingPoolGroup     ParentGroup    { get; }

        #region ChargingStations

        private readonly ConcurrentDictionary<ChargingStation_Id, ChargingStation> _ChargingStations;

        /// <summary>
        /// Return all charging stations registered within this charging station group.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations
            => _ChargingStations.Values;


        /// <summary>
        /// Return all charging station identifications registered within this charging station group.
        /// </summary>
        public IEnumerable<ChargingStation_Id> ChargingStationIds
            => ChargingStations.SafeSelect(station => station.Id);

        /// <summary>
        /// Return all EVSEs registered within this charging station group.
        /// </summary>
        public IEnumerable<IEVSE> EVSEs
            => ChargingStations.SafeSelectMany(station => station.EVSEs);

        /// <summary>
        /// Return all EVSE identifications registered within this charging station group.
        /// </summary>
        public IEnumerable<EVSE_Id> EVSEIds
            => ChargingStations.
                   SafeSelectMany(station => station.EVSEs).
                   SafeSelect    (evse    => evse.Id);

        #endregion

        #region StatusAggregationDelegate

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated charging stations.
        /// </summary>
        public Func<ChargingStationStatusReport, ChargingPoolGroupStatusTypes>  StatusAggregationDelegate   { get; }

        #endregion

        #endregion

        #region Links

        /// <summary>
        /// The Charging Station Operator of this charging pool.
        /// </summary>
        [Mandatory]
        public IChargingStationOperator Operator { get; }

        /// <summary>
        /// The roaming network of this charging station.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork RoamingNetwork
            => Operator.RoamingNetwork;

        #endregion

        #region Events

        // ChargingStation events

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


        // EVSE events

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

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station group.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station group.</param>
        /// <param name="Operator">The charging station operator of this charging station group.</param>
        /// <param name="Name">The official (multi-language) name of this charging station group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging station group.</param>
        /// 
        /// <param name="Members">An enumeration of charging stations member building this charging station group.</param>
        /// <param name="MemberIds">An enumeration of charging station identifications which are building this charging station group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new charging stations automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated charging stations.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the charging station group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the charging station group admin status list.</param>
        internal ChargingPoolGroup(ChargingPoolGroup_Id                                             Id,
                                      ChargingStationOperator                                            Operator,
                                      I18NString                                                          Name,
                                      I18NString                                                          Description                   = null,

                                      Brand                                                               Brand                         = null,
                                      Priority?                                                           Priority                      = null,
                                      ChargingTariff                                                      Tariff                        = null,
                                      IEnumerable<DataLicense>                                            DataLicenses                  = null,

                                      IEnumerable<ChargingStation>                                       Members                       = null,
                                      IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                      Func<ChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                      Func<ChargingStationStatusReport, ChargingPoolGroupStatusTypes>  StatusAggregationDelegate     = null,
                                      UInt16                                                              MaxGroupStatusListSize        = DefaultMaxGroupStatusListSize,
                                      UInt16                                                              MaxGroupAdminStatusListSize   = DefaultMaxGroupAdminStatusListSize)

            : base(Id,
                   Name,
                   Description)

        {

            #region Initial checks

            if (Operator is null)
                throw new ArgumentNullException(nameof(Operator),  "The charging station operator must not be null!");

            if (IEnumerableExtensions.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(Name),      "The name of the charging station group must not be null or empty!");

            #endregion

            #region Init data and properties

            this.Operator                    = Operator;

            this.Brand                       = Brand;
            this.Priority                    = Priority;
            this.Tariff                      = Tariff;
            this.DataLicenses                = DataLicenses;

            this._AllowedMemberIds           = MemberIds is not null ? new HashSet<ChargingStation_Id>(MemberIds) : new HashSet<ChargingStation_Id>();
            this.AutoIncludeStations         = AutoIncludeStations ?? (MemberIds is null ? (Func<ChargingStation, Boolean>) (station => true) : station => false);
            this._ChargingStations           = new ConcurrentDictionary<ChargingStation_Id, ChargingStation>();

            this.StatusAggregationDelegate   = StatusAggregationDelegate;

            #endregion

            if (Members?.Any() == true)
                Members.ForEach(station => Add(station));

        }

        #endregion


        public ChargingPoolGroup Add(ChargingStation Station)
        {

            if (_AllowedMemberIds.Contains(Station.Id) &&
                AutoIncludeStations(Station))
            {
                _ChargingStations.TryAdd(Station.Id, Station);
            }

            return this;

        }

        public ChargingPoolGroup Add(ChargingStation_Id StationId)
        {

            _AllowedMemberIds.Add(StationId);

            return this;

        }


        #region (internal) UpdateEVSEData       (Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateEVSEData(DateTime          Timestamp,
                                     EventTracking_Id  EventTrackingId,
                                     EVSE              EVSE,
                                     String            PropertyName,
                                     Object            OldValue,
                                     Object            NewValue)
        {

            var onEVSEDataChanged = OnEVSEDataChanged;
            if (onEVSEDataChanged is not null)
                onEVSEDataChanged(Timestamp,
                                       EventTrackingId,
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
                                                  Timestamped<EVSEAdminStatusType>  OldStatus,
                                                  Timestamped<EVSEAdminStatusType>  NewStatus)
        {

            var onEVSEAdminStatusChanged = OnEVSEAdminStatusChanged;
            if (onEVSEAdminStatusChanged is not null)
                await onEVSEAdminStatusChanged(Timestamp,
                                                    EventTrackingId,
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
        internal async Task UpdateEVSEStatus(DateTime                      Timestamp,
                                             EventTracking_Id              EventTrackingId,
                                             EVSE                          EVSE,
                                             Timestamped<EVSEStatusType>  OldStatus,
                                             Timestamped<EVSEStatusType>  NewStatus)
        {

            var onEVSEStatusChanged = OnEVSEStatusChanged;
            if (onEVSEStatusChanged is not null)
                await onEVSEStatusChanged(Timestamp,
                                               EventTrackingId,
                                               EVSE,
                                               OldStatus,
                                               NewStatus);

        }

        #endregion


        #region (internal) UpdateChargingStationData       (Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        internal void UpdateChargingStationData(DateTime          Timestamp,
                                                EventTracking_Id  EventTrackingId,
                                                ChargingStation   ChargingStation,
                                                String            PropertyName,
                                                Object?           NewValue,
                                                Object?           OldValue     = null,
                                                Context?          DataSource   = null)
        {

            var onChargingStationDataChanged = OnChargingStationDataChanged;
            if (onChargingStationDataChanged is not null)
                onChargingStationDataChanged(Timestamp,
                                             EventTrackingId,
                                             ChargingStation,
                                             PropertyName,
                                             NewValue,
                                             OldValue,
                                             DataSource);

        }

        #endregion

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal void UpdateChargingStationAdminStatus(DateTime                                       Timestamp,
                                                       EventTracking_Id                               EventTrackingId,
                                                       ChargingStation                                ChargingStation,
                                                       Timestamped<ChargingStationAdminStatusType>   NewStatus,
                                                       Timestamped<ChargingStationAdminStatusType>?  OldStatus    = null,
                                                       Context?                                       DataSource   = null)
        {

            var onChargingStationAdminStatusChanged = OnChargingStationAdminStatusChanged;
            if (onChargingStationAdminStatusChanged is not null)
                onChargingStationAdminStatusChanged(Timestamp,
                                                    EventTrackingId,
                                                    ChargingStation,
                                                    NewStatus,
                                                    OldStatus,
                                                    DataSource);

        }

        #endregion

        #region (internal) UpdateChargingStationStatus     (Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old charging station status.</param>
        /// <param name="NewStatus">The new charging station status.</param>
        internal void UpdateChargingStationStatus(DateTime                                  Timestamp,
                                                  EventTracking_Id                          EventTrackingId,
                                                  ChargingStation                           ChargingStation,
                                                  Timestamped<ChargingStationStatusType>   NewStatus,
                                                  Timestamped<ChargingStationStatusType>?  OldStatus    = null,
                                                  Context?                                  DataSource   = null)
        {

            var onChargingStationStatusChanged = OnChargingStationStatusChanged;
            if (onChargingStationStatusChanged is not null)
                onChargingStationStatusChanged(Timestamp,
                                               EventTrackingId,
                                               ChargingStation,
                                               NewStatus,
                                               OldStatus,
                                               DataSource);

         //   if (StatusAggregationDelegate is not null)
         //       _StatusSchedule.Insert(Timestamp,
         //                              StatusAggregationDelegate(new ChargingStationStatusReport(_ChargingStations.Values)));

        }

        #endregion


        #region IEnumerable<ChargingStation> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ChargingStations.GetEnumerator();
        }

        public IEnumerator<ChargingStation> GetEnumerator()
        {
            return ChargingStations.GetEnumerator();
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging station group.</param>
        /// <param name="ChargingPoolGroup2">Another charging station group.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingPoolGroup ChargingPoolGroup1, ChargingPoolGroup ChargingPoolGroup2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingPoolGroup1, ChargingPoolGroup2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingPoolGroup1 is null) || ((Object) ChargingPoolGroup2 is null))
                return false;

            return ChargingPoolGroup1.Equals(ChargingPoolGroup2);

        }

        #endregion

        #region Operator != (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging station group.</param>
        /// <param name="ChargingPoolGroup2">Another charging station group.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingPoolGroup ChargingPoolGroup1, ChargingPoolGroup ChargingPoolGroup2)
            => !(ChargingPoolGroup1 == ChargingPoolGroup2);

        #endregion

        #region Operator <  (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging station group.</param>
        /// <param name="ChargingPoolGroup2">Another charging station group.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingPoolGroup ChargingPoolGroup1, ChargingPoolGroup ChargingPoolGroup2)
        {

            if ((Object) ChargingPoolGroup1 is null)
                throw new ArgumentNullException(nameof(ChargingPoolGroup1), "The given ChargingPoolGroup1 must not be null!");

            return ChargingPoolGroup1.CompareTo(ChargingPoolGroup2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging station group.</param>
        /// <param name="ChargingPoolGroup2">Another charging station group.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingPoolGroup ChargingPoolGroup1, ChargingPoolGroup ChargingPoolGroup2)
            => !(ChargingPoolGroup1 > ChargingPoolGroup2);

        #endregion

        #region Operator >  (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging station group.</param>
        /// <param name="ChargingPoolGroup2">Another charging station group.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingPoolGroup ChargingPoolGroup1, ChargingPoolGroup ChargingPoolGroup2)
        {

            if ((Object) ChargingPoolGroup1 is null)
                throw new ArgumentNullException(nameof(ChargingPoolGroup1), "The given ChargingPoolGroup1 must not be null!");

            return ChargingPoolGroup1.CompareTo(ChargingPoolGroup2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolGroup1, ChargingPoolGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup1">A charging station group.</param>
        /// <param name="ChargingPoolGroup2">Another charging station group.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingPoolGroup ChargingPoolGroup1, ChargingPoolGroup ChargingPoolGroup2)
            => !(ChargingPoolGroup1 < ChargingPoolGroup2);

        #endregion

        #endregion

        #region IComparable<ChargingPoolGroup> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var ChargingPoolGroup = Object as ChargingPoolGroup;
            if ((Object) ChargingPoolGroup is null)
                throw new ArgumentException("The given object is not a charging pool!", nameof(Object));

            return CompareTo(ChargingPoolGroup);

        }

        #endregion

        #region CompareTo(ChargingPoolGroup)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroup">A charging station group object to compare with.</param>
        public Int32 CompareTo(ChargingPoolGroup ChargingPoolGroup)
        {

            if ((Object) ChargingPoolGroup is null)
                throw new ArgumentNullException(nameof(ChargingPoolGroup), "The given charging station group must not be null!");

            return Id.CompareTo(ChargingPoolGroup.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolGroup> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            var ChargingPoolGroup = Object as ChargingPoolGroup;
            if ((Object) ChargingPoolGroup is null)
                return false;

            return Equals(ChargingPoolGroup);

        }

        #endregion

        #region Equals(ChargingPoolGroup)

        /// <summary>
        /// Compares two charging pools for equality.
        /// </summary>
        /// <param name="ChargingPoolGroup">A charging station group to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolGroup ChargingPoolGroup)
        {

            if ((Object) ChargingPoolGroup is null)
                return false;

            return Id.Equals(ChargingPoolGroup.Id);

        }

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
