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
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;
using System.Threading.Tasks;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A delegate called whenever the admin status changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingStationGroup">The updated charging pool.</param>
    /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
    /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
    public delegate Task OnAdminStatusChangedDelegate(DateTime Timestamp,
                                                      EventTracking_Id EventTrackingId,
                                                      ChargingStationGroup ChargingStationGroup,
                                                      Timestamped<ChargingStationGroupAdminStatusTypes> OldStatus,
                                                      Timestamped<ChargingStationGroupAdminStatusTypes> NewStatus);

    /// <summary>
    /// A delegate called whenever the status changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingStationGroup">The updated charging pool.</param>
    /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
    /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
    public delegate Task OnStatusChangedDelegate(DateTime Timestamp,
                                                 EventTracking_Id EventTrackingId,
                                                 ChargingStationGroup ChargingStationGroup,
                                                 Timestamped<ChargingStationGroupStatusTypes> OldStatus,
                                                 Timestamped<ChargingStationGroupStatusTypes> NewStatus);


    public class AutoIncludeMemberIds
    {

        private List<ChargingStation_Id> _AllowedMemberIds;

        public AutoIncludeMemberIds(IEnumerable<ChargingStation_Id> AllowedMemberIds)
        {

            this._AllowedMemberIds = new List<ChargingStation_Id>(AllowedMemberIds);

        }

        public Boolean Allowed(ChargingStation_Id StationId)
            => _AllowedMemberIds.Contains(StationId);


    }


    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging pool
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingStationGroup : AEMobilityEntity<ChargingStationGroup_Id>,
                                        IEquatable<ChargingStationGroup>, IComparable<ChargingStationGroup>, IComparable,
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
        /// The offical (multi-language) name of this group.
        /// </summary>
        [Mandatory]
        public I18NString               Name           { get; }

        /// <summary>
        /// An optional (multi-language) description of this group.
        /// </summary>
        [Optional]
        public I18NString               Description    { get; }


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

        private ReactiveSet<DataLicense> _DataLicenses;

        /// <summary>
        /// The license of the group data.
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


        private HashSet<ChargingStation_Id> _AllowedMemberIds;

        public IEnumerable<ChargingStation_Id> AllowedMemberIds
            => _AllowedMemberIds;

        public Func<ChargingStation, Boolean> AutoIncludeStations { get; }


        public ChargingStationGroup     ParentGroup    { get; }

        #region ChargingStations

        private readonly ConcurrentDictionary<ChargingStation_Id, ChargingStation> _ChargingStations;

        /// <summary>
        /// Return all charging stations registered within this charing station group.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations
            => _ChargingStations.Values;


        /// <summary>
        /// Return all charging station identifications registered within this charing station group.
        /// </summary>
        public IEnumerable<ChargingStation_Id> ChargingStationIds
            => ChargingStations.SafeSelect(station => station.Id);

        /// <summary>
        /// Return all EVSEs registered within this charing station group.
        /// </summary>
        public IEnumerable<EVSE> EVSEs
            => ChargingStations.SafeSelectMany(station => station.EVSEs);

        /// <summary>
        /// Return all EVSE identifications registered within this charing station group.
        /// </summary>
        public IEnumerable<EVSE_Id> EVSEIds
            => ChargingStations.
                   SafeSelectMany(station => station.EVSEs).
                   SafeSelect    (evse    => evse.Id);

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current charging pool admin status.
        /// </summary>
        [Dynamic]
        public Timestamped<ChargingStationGroupAdminStatusTypes> AdminStatus
            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<ChargingStationGroupAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The charging pool admin status schedule.
        /// </summary>
        [Dynamic]
        public IEnumerable<Timestamped<ChargingStationGroupAdminStatusTypes>> AdminStatusSchedule
            => _AdminStatusSchedule;

        #endregion


        #region Status

        /// <summary>
        /// The current charging pool status.
        /// </summary>
        [Dynamic]
        public Timestamped<ChargingStationGroupStatusTypes> Status
            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<ChargingStationGroupStatusTypes> _StatusSchedule;

        /// <summary>
        /// The charging pool status schedule.
        /// </summary>
        [Dynamic]
        public IEnumerable<Timestamped<ChargingStationGroupStatusTypes>> StatusSchedule
            => _StatusSchedule;

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
        public ChargingStationOperator Operator { get; }

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

        internal readonly IVotingNotificator<DateTime, ChargingStationGroup, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationGroup, ChargingStation, Boolean> OnChargingStationAddition
        {
            get
            {
                return ChargingStationAddition;
            }
        }

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationGroup, ChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationGroup, ChargingStation, Boolean> OnChargingStationRemoval
        {
            get
            {
                return ChargingStationRemoval;
            }
        }

        #endregion


        // ChargingStation events

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

        #region EVSEAddition

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSEAddition
        {
            get
            {
                return EVSEAddition;
            }
        }

        #endregion

        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSERemoval
        {
            get
            {
                return EVSERemoval;
            }
        }

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
                                      ChargingStationOperator                                             Operator,
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
                                      UInt16                                                              MaxGroupStatusListSize        = DefaultMaxGroupStatusListSize,
                                      UInt16                                                              MaxGroupAdminStatusListSize   = DefaultMaxGroupAdminStatusListSize)

            : base(Id)

        {

            #region Initial checks

            if (Operator == null)
                throw new ArgumentNullException(nameof(Operator),  "The charging station operator must not be null!");

            if (IEnumerableExtensions.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(Name),      "The name of the charging station group must not be null or empty!");

            #endregion

            #region Init data and properties

            this.Operator                    = Operator;
            this.Name                        = Name;
            this.Description                 = Description ?? new I18NString();

            this.Brand                       = Brand;
            this.Priority                    = Priority;
            this.Tariff                      = Tariff;
            this.DataLicenses                = DataLicenses?.Any() == true ? new ReactiveSet<DataLicense>(DataLicenses) : new ReactiveSet<DataLicense>();

            this._AllowedMemberIds           = MemberIds != null ? new HashSet<ChargingStation_Id>(MemberIds) : new HashSet<ChargingStation_Id>();
            this.AutoIncludeStations         = AutoIncludeStations ?? (MemberIds == null ? (Func<ChargingStation, Boolean>) (station => true) : station => false);
            this._ChargingStations           = new ConcurrentDictionary<ChargingStation_Id, ChargingStation>();

            this.StatusAggregationDelegate   = StatusAggregationDelegate;

            this._AdminStatusSchedule        = new StatusSchedule<ChargingStationGroupAdminStatusTypes>(MaxGroupAdminStatusListSize);
            this._AdminStatusSchedule.Insert(ChargingStationGroupAdminStatusTypes.Unknown);

            this._StatusSchedule             = new StatusSchedule<ChargingStationGroupStatusTypes>     (MaxGroupStatusListSize);
            this._StatusSchedule.     Insert(ChargingStationGroupStatusTypes.Unknown);

            #endregion

            #region Init events

            // ChargingStationGroup events
            this.ChargingStationAddition  = new VotingNotificator<DateTime, ChargingStationGroup, ChargingStation, Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval   = new VotingNotificator<DateTime, ChargingStationGroup, ChargingStation, Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition             = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval              = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events

            #endregion

            #region Link events

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            // ChargingStationGroup events
            //this.OnChargingStationAddition.OnVoting       += (timestamp, evseoperator, pool, vote) => EVSEOperator.ChargingStationAddition.SendVoting      (timestamp, evseoperator, pool, vote);
            //this.OnChargingStationAddition.OnNotification += (timestamp, evseoperator, pool)       => EVSEOperator.ChargingStationAddition.SendNotification(timestamp, evseoperator, pool);
            //
            //this.OnChargingStationRemoval. OnVoting       += (timestamp, evseoperator, pool, vote) => EVSEOperator.ChargingStationRemoval. SendVoting      (timestamp, evseoperator, pool, vote);
            //this.OnChargingStationRemoval. OnNotification += (timestamp, evseoperator, pool)       => EVSEOperator.ChargingStationRemoval. SendNotification(timestamp, evseoperator, pool);

            // ChargingStation events
            this.OnEVSEAddition.           OnVoting       += (timestamp, station, evse, vote)      => Operator.EVSEAddition.           SendVoting      (timestamp, station, evse, vote);
            this.OnEVSEAddition.           OnNotification += (timestamp, station, evse)            => Operator.EVSEAddition.           SendNotification(timestamp, station, evse);

            this.OnEVSERemoval.            OnVoting       += (timestamp, station, evse, vote)      => Operator.EVSERemoval .           SendVoting      (timestamp, station, evse, vote);
            this.OnEVSERemoval.            OnNotification += (timestamp, station, evse)            => Operator.EVSERemoval .           SendNotification(timestamp, station, evse);

            #endregion


            if (Members?.Any() == true)
                Members.ForEach(station => Add(station));

        }

        #endregion


        public ChargingStationGroup Add(ChargingStation Station)
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


        #region Contains(ChargingStation)

        /// <summary>
        /// Check if the given charging station is member of this charging station group.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean Contains(ChargingStation ChargingStation)
            => _ChargingStations.ContainsKey(ChargingStation.Id);

        #endregion

        #region ContainsId(ChargingStationId)

        /// <summary>
        /// Check if the given charging station identification is member of this charging station group.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsId(ChargingStation_Id ChargingStationId)
            => _ChargingStations.ContainsKey(ChargingStationId);

        #endregion

        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is member of this charging station group.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)
            => ChargingStations.Any(ChargingStation => ChargingStation.EVSEs.Contains(EVSE));

        #endregion

        #region ContainsEVSEId(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is member of this charging station group.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSEId(EVSE_Id EVSEId)
            => ChargingStations.Any(ChargingStation => ChargingStation.EVSEIds().Contains(EVSEId));

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(ChargingStationGroupAdminStatusTypes  NewAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<ChargingStationGroupAdminStatusTypes> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(ChargingStationGroupAdminStatusTypes  NewAdminStatus,
                                   DateTime                             Timestamp)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus, Timestamp);
        }

        #endregion

        #region SetAdminStatus(NewStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped admin status.
        /// </summary>
        /// <param name="NewStatusList">A list of new timestamped admin status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetAdminStatus(IEnumerable<Timestamped<ChargingStationGroupAdminStatusTypes>>  NewStatusList,
                                   ChangeMethods                                                  ChangeMethod = ChangeMethods.Replace)
        {

            _AdminStatusSchedule.Set(NewStatusList, ChangeMethod);

        }

        #endregion


        #region (internal) UpdateAdminStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                                          Timestamp,
                                              EventTracking_Id                                  EventTrackingId,
                                              Timestamped<ChargingStationGroupAdminStatusTypes>  OldStatus,
                                              Timestamped<ChargingStationGroupAdminStatusTypes>  NewStatus)
        {

            await OnAdminStatusChanged?.Invoke(Timestamp, EventTrackingId, this, OldStatus, NewStatus);

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

            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal != null)
                OnEVSEDataChangedLocal(Timestamp,
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

            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal != null)
                await OnEVSEStatusChangedLocal(Timestamp,
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
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateChargingStationData(DateTime          Timestamp,
                                                EventTracking_Id  EventTrackingId,
                                                ChargingStation   ChargingStation,
                                                String            PropertyName,
                                                Object            OldValue,
                                                Object            NewValue)
        {

            var OnChargingStationDataChangedLocal = OnChargingStationDataChanged;
            if (OnChargingStationDataChangedLocal != null)
                OnChargingStationDataChangedLocal(Timestamp,
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
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal void UpdateChargingStationAdminStatus(DateTime                                      Timestamp,
                                                       EventTracking_Id                              EventTrackingId,
                                                       ChargingStation                               ChargingStation,
                                                       Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                                       Timestamped<ChargingStationAdminStatusTypes>  NewStatus)
        {

            var OnChargingStationAdminStatusChangedLocal = OnChargingStationAdminStatusChanged;
            if (OnChargingStationAdminStatusChangedLocal != null)
                OnChargingStationAdminStatusChangedLocal(Timestamp,
                                                         EventTrackingId,
                                                         ChargingStation,
                                                         OldStatus,
                                                         NewStatus);

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
        internal void UpdateChargingStationStatus(DateTime                                 Timestamp,
                                                  EventTracking_Id                         EventTrackingId,
                                                  ChargingStation                          ChargingStation,
                                                  Timestamped<ChargingStationStatusTypes>  OldStatus,
                                                  Timestamped<ChargingStationStatusTypes>  NewStatus)
        {

            var OnChargingStationStatusChangedLocal = OnChargingStationStatusChanged;
            if (OnChargingStationStatusChangedLocal != null)
                OnChargingStationStatusChangedLocal(Timestamp,
                                                    EventTrackingId,
                                                    ChargingStation,
                                                    OldStatus,
                                                    NewStatus);

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

        public IEnumerator<ChargingStation> GetEnumerator()
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
