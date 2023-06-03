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

using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;
using System.Threading.Tasks;
using org.GraphDefined.Vanaheimr.Hermod;
using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate called whenever the admin status changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingStationGroup">The updated charging pool.</param>
    /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
    /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
    public delegate Task OnAdminStatusChangedDelegate(DateTime                                            Timestamp,
                                                      EventTracking_Id                                    EventTrackingId,
                                                      ChargingStationGroup                                ChargingStationGroup,
                                                      Timestamped<ChargingStationGroupAdminStatusTypes>   NewStatus,
                                                      Timestamped<ChargingStationGroupAdminStatusTypes>?  OldStatus    = null,
                                                      Context?                                            DataSource   = null);

    /// <summary>
    /// A delegate called whenever the status changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingStationGroup">The updated charging pool.</param>
    /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
    /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
    public delegate Task OnStatusChangedDelegate(DateTime                                       Timestamp,
                                                 EventTracking_Id                               EventTrackingId,
                                                 ChargingStationGroup                           ChargingStationGroup,
                                                 Timestamped<ChargingStationGroupStatusTypes>   NewStatus,
                                                 Timestamped<ChargingStationGroupStatusTypes>?  OldStatus    = null,
                                                 Context?                                       DataSource   = null);


    public class AutoIncludeMemberIds
    {

        private List<ChargingStation_Id> allowedMemberIds;

        public AutoIncludeMemberIds(IEnumerable<ChargingStation_Id> AllowedMemberIds)
        {

            this.allowedMemberIds = new List<ChargingStation_Id>(AllowedMemberIds);

        }

        public Boolean Allowed(ChargingStation_Id StationId)
            => allowedMemberIds.Contains(StationId);


    }


    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging pool
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingStationGroup : AEMobilityEntity<ChargingStationGroup_Id,
                                                         ChargingStationGroupAdminStatusTypes,
                                                         ChargingStationGroupStatusTypes>,
                                        IEquatable<ChargingStationGroup>, IComparable<ChargingStationGroup>, IComparable,
                                        IEnumerable<IChargingStation>
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
        public ChargingTariff           Tariff         { get; }

        #region DataLicense

        private ReactiveSet<OpenDataLicense> _DataLicenses;

        /// <summary>
        /// The license of the group data.
        /// </summary>
        [Mandatory]
        public ReactiveSet<OpenDataLicense> DataLicenses
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


        private HashSet<ChargingStation_Id> _AllowedMemberIds;

        public IEnumerable<ChargingStation_Id> AllowedMemberIds
            => _AllowedMemberIds;

        public Func<IChargingStation, Boolean> AutoIncludeStations { get; }


        public ChargingStationGroup     ParentGroup    { get; }

        #region ChargingStations

        private readonly ConcurrentDictionary<ChargingStation_Id, IChargingStation> _ChargingStations;

        /// <summary>
        /// Return all charging stations registered within this charing station group.
        /// </summary>
        public IEnumerable<IChargingStation> ChargingStations
            => _ChargingStations.Values;


        /// <summary>
        /// Return all charging station identifications registered within this charing station group.
        /// </summary>
        public IEnumerable<ChargingStation_Id> ChargingStationIds
            => ChargingStations.SafeSelect(station => station.Id);

        /// <summary>
        /// Return all EVSEs registered within this charing station group.
        /// </summary>
        public IEnumerable<IEVSE> EVSEs
            => ChargingStations.SafeSelectMany(station => station.EVSEs);

        /// <summary>
        /// Return all EVSE identifications registered within this charing station group.
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
        public Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate   { get; }

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
            => Operator?.RoamingNetwork;

        #endregion

        #region Events

        // ChargingStationGroup events

        #region OnAdminStatusChanged

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnAdminStatusChangedDelegate OnAdminStatusChanged;

        #endregion

        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, ChargingStationGroup, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, ChargingStationGroup, ChargingStation, Boolean> OnChargingStationAddition
            => ChargingStationAddition;

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, ChargingStationGroup, ChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, ChargingStationGroup, ChargingStation, Boolean> OnChargingStationRemoval
            => ChargingStationRemoval;

        #endregion


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

        #region EVSEAddition

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSEAddition
            => EVSEAddition;

        #endregion

        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSERemoval
            => EVSERemoval;

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
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Operator">The charging station operator of this charging station group.</param>
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
        internal ChargingStationGroup(ChargingStationGroup_Id                                             Id,
                                      IChargingStationOperator                                            Operator,
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
                                      UInt16                                                              MaxGroupStatusListSize        = DefaultMaxGroupStatusListSize,
                                      UInt16                                                              MaxGroupAdminStatusListSize   = DefaultMaxGroupAdminStatusListSize)

            : base(Id,
                   Name,
                   Description)

        {

            #region Initial checks

            if (Operator == null)
                throw new ArgumentNullException(nameof(Operator),  "The charging station operator must not be null!");

            if (IEnumerableExtensions.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(Name),      "The name of the charging station group must not be null or empty!");

            #endregion

            #region Init data and properties

            this.Operator                    = Operator;

            this.Brand                       = Brand;
            this.Priority                    = Priority;
            this.Tariff                      = Tariff;
            this.DataLicenses                = DataLicenses?.Any() == true ? new ReactiveSet<OpenDataLicense>(DataLicenses) : new ReactiveSet<OpenDataLicense>();

            this._AllowedMemberIds           = MemberIds != null ? new HashSet<ChargingStation_Id>(MemberIds) : new HashSet<ChargingStation_Id>();
            this.AutoIncludeStations         = AutoIncludeStations ?? (MemberIds == null ? (Func<IChargingStation, Boolean>) (station => true) : station => false);
            this._ChargingStations           = new ConcurrentDictionary<ChargingStation_Id, IChargingStation>();

            this.StatusAggregationDelegate   = StatusAggregationDelegate;

            #endregion

            #region Init events

            // ChargingStationGroup events
            this.ChargingStationAddition  = new VotingNotificator<DateTime, EventTracking_Id, User_Id, ChargingStationGroup, ChargingStation, Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval   = new VotingNotificator<DateTime, EventTracking_Id, User_Id, ChargingStationGroup, ChargingStation, Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition             = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval              = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events

            #endregion

            #region Link events

            this.adminStatusSchedule.OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                         => UpdateAdminStatus(timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            // ChargingStationGroup events
            //this.OnChargingStationAddition.OnVoting       += (timestamp, evseoperator, pool, vote) => EVSEOperator.ChargingStationAddition.SendVoting      (timestamp, evseoperator, pool, vote);
            //this.OnChargingStationAddition.OnNotification += (timestamp, evseoperator, pool)       => EVSEOperator.ChargingStationAddition.SendNotification(timestamp, evseoperator, pool);
            //
            //this.OnChargingStationRemoval. OnVoting       += (timestamp, evseoperator, pool, vote) => EVSEOperator.ChargingStationRemoval. SendVoting      (timestamp, evseoperator, pool, vote);
            //this.OnChargingStationRemoval. OnNotification += (timestamp, evseoperator, pool)       => EVSEOperator.ChargingStationRemoval. SendNotification(timestamp, evseoperator, pool);

            // ChargingStation events
            this.OnEVSEAddition.           OnVoting       += (timestamp, eventTrackingId, userId, station, evse, vote)      => Operator.EVSEAddition.           SendVoting      (timestamp, eventTrackingId, userId, station, evse, vote);
            this.OnEVSEAddition.           OnNotification += (timestamp, eventTrackingId, userId, station, evse)            => Operator.EVSEAddition.           SendNotification(timestamp, eventTrackingId, userId, station, evse);

            this.OnEVSERemoval.            OnVoting       += (timestamp, eventTrackingId, userId, station, evse, vote)      => Operator.EVSERemoval .           SendVoting      (timestamp, eventTrackingId, userId, station, evse, vote);
            this.OnEVSERemoval.            OnNotification += (timestamp, eventTrackingId, userId, station, evse)            => Operator.EVSERemoval .           SendNotification(timestamp, eventTrackingId, userId, station, evse);

            #endregion


            if (Members?.Any() == true)
                Members.ForEach(station => Add(station));

        }

        #endregion


        public ChargingStationGroup Add(IChargingStation Station)
        {

            if (_AllowedMemberIds.Contains(Station.Id) &&
                AutoIncludeStations(Station))
            {
                _ChargingStations.TryAdd(Station.Id, Station);
            }

            return this;

        }

        public ChargingStationGroup Add(ChargingStation_Id StationId)
        {

            _AllowedMemberIds.Add(StationId);

            return this;

        }


        #region (internal) UpdateAdminStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                                            Timestamp,
                                              EventTracking_Id                                    EventTrackingId,
                                              Timestamped<ChargingStationGroupAdminStatusTypes>   NewStatus,
                                              Timestamped<ChargingStationGroupAdminStatusTypes>?  OldStatus    = null,
                                              Context?                                            DataSource   = null)
        {

            await OnAdminStatusChanged?.Invoke(Timestamp,
                                               EventTrackingId,
                                               this,
                                               NewStatus,
                                               OldStatus,
                                               DataSource);

        }

        #endregion


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
            if (onEVSEDataChanged != null)
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
                                                  Timestamped<EVSEAdminStatusTypes>  OldStatus,
                                                  Timestamped<EVSEAdminStatusTypes>  NewStatus)
        {

            var onEVSEAdminStatusChanged = OnEVSEAdminStatusChanged;
            if (onEVSEAdminStatusChanged != null)
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
                                             Timestamped<EVSEStatusTypes>  OldStatus,
                                             Timestamped<EVSEStatusTypes>  NewStatus)
        {

            var onEVSEStatusChanged = OnEVSEStatusChanged;
            if (onEVSEStatusChanged != null)
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
                                                       Timestamped<ChargingStationAdminStatusTypes>   NewStatus,
                                                       Timestamped<ChargingStationAdminStatusTypes>?  OldStatus    = null,
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
                                                  Timestamped<ChargingStationStatusTypes>   NewStatus,
                                                  Timestamped<ChargingStationStatusTypes>?  OldStatus    = null,
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

         //   if (StatusAggregationDelegate != null)
         //       _StatusSchedule.Insert(Timestamp,
         //                              StatusAggregationDelegate(new ChargingStationStatusReport(_ChargingStations.Values)));

        }

        #endregion


        #region IEnumerable<ChargingStation> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ChargingStations.GetEnumerator();
        }

        public IEnumerator<IChargingStation> GetEnumerator()
        {
            return ChargingStations.GetEnumerator();
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationGroup1, ChargingStationGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroup1">A charging station group.</param>
        /// <param name="ChargingStationGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationGroup ChargingStationGroup1, ChargingStationGroup ChargingStationGroup2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationGroup1, ChargingStationGroup2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationGroup1 == null) || ((Object) ChargingStationGroup2 == null))
                return false;

            return ChargingStationGroup1.Equals(ChargingStationGroup2);

        }

        #endregion

        #region Operator != (ChargingStationGroup1, ChargingStationGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroup1">A charging station group.</param>
        /// <param name="ChargingStationGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationGroup ChargingStationGroup1, ChargingStationGroup ChargingStationGroup2)
            => !(ChargingStationGroup1 == ChargingStationGroup2);

        #endregion

        #region Operator <  (ChargingStationGroup1, ChargingStationGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroup1">A charging station group.</param>
        /// <param name="ChargingStationGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationGroup ChargingStationGroup1, ChargingStationGroup ChargingStationGroup2)
        {

            if ((Object) ChargingStationGroup1 == null)
                throw new ArgumentNullException(nameof(ChargingStationGroup1), "The given ChargingStationGroup1 must not be null!");

            return ChargingStationGroup1.CompareTo(ChargingStationGroup2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationGroup1, ChargingStationGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroup1">A charging station group.</param>
        /// <param name="ChargingStationGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationGroup ChargingStationGroup1, ChargingStationGroup ChargingStationGroup2)
            => !(ChargingStationGroup1 > ChargingStationGroup2);

        #endregion

        #region Operator >  (ChargingStationGroup1, ChargingStationGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroup1">A charging station group.</param>
        /// <param name="ChargingStationGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationGroup ChargingStationGroup1, ChargingStationGroup ChargingStationGroup2)
        {

            if ((Object) ChargingStationGroup1 == null)
                throw new ArgumentNullException(nameof(ChargingStationGroup1), "The given ChargingStationGroup1 must not be null!");

            return ChargingStationGroup1.CompareTo(ChargingStationGroup2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationGroup1, ChargingStationGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroup1">A charging station group.</param>
        /// <param name="ChargingStationGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationGroup ChargingStationGroup1, ChargingStationGroup ChargingStationGroup2)
            => !(ChargingStationGroup1 < ChargingStationGroup2);

        #endregion

        #endregion

        #region IComparable<ChargingStationGroup> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var ChargingStationGroup = Object as ChargingStationGroup;
            if ((Object) ChargingStationGroup == null)
                throw new ArgumentException("The given object is not a charging pool!", nameof(Object));

            return CompareTo(ChargingStationGroup);

        }

        #endregion

        #region CompareTo(ChargingStationGroup)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroup">A charging station group object to compare with.</param>
        public Int32 CompareTo(ChargingStationGroup ChargingStationGroup)
        {

            if ((Object) ChargingStationGroup == null)
                throw new ArgumentNullException(nameof(ChargingStationGroup), "The given charging station group must not be null!");

            return Id.CompareTo(ChargingStationGroup.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationGroup> Members

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

            var ChargingStationGroup = Object as ChargingStationGroup;
            if ((Object) ChargingStationGroup == null)
                return false;

            return Equals(ChargingStationGroup);

        }

        #endregion

        #region Equals(ChargingStationGroup)

        /// <summary>
        /// Compares two charging pools for equality.
        /// </summary>
        /// <param name="ChargingStationGroup">A charging station group to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationGroup ChargingStationGroup)
        {

            if ((Object) ChargingStationGroup == null)
                return false;

            return Id.Equals(ChargingStationGroup.Id);

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
            => String.Concat(Id, ", ", Name.FirstText());

        #endregion

    }

}
