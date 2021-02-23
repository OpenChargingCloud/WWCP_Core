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
    /// <param name="EVSEGroup">The updated charging pool.</param>
    /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
    /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
    public delegate Task OnAdminStatus2ChangedDelegate(DateTime Timestamp,
                                                      EventTracking_Id EventTrackingId,
                                                      EVSEGroup EVSEGroup,
                                                      Timestamped<EVSEGroupAdminStatusTypes> OldStatus,
                                                      Timestamped<EVSEGroupAdminStatusTypes> NewStatus);

    /// <summary>
    /// A delegate called whenever the status changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EVSEGroup">The updated charging pool.</param>
    /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
    /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
    public delegate Task OnStatus2ChangedDelegate(DateTime Timestamp,
                                                 EventTracking_Id EventTrackingId,
                                                 EVSEGroup EVSEGroup,
                                                 Timestamped<EVSEGroupStatusTypes> OldStatus,
                                                 Timestamped<EVSEGroupStatusTypes> NewStatus);


    //public class AutoIncludeMemberIds
    //{

    //    private List<ChargingStation_Id> _AllowedMemberIds;

    //    public AutoIncludeMemberIds(IEnumerable<ChargingStation_Id> AllowedMemberIds)
    //    {

    //        this._AllowedMemberIds = new List<ChargingStation_Id>(AllowedMemberIds);

    //    }

    //    public Boolean Allowed(ChargingStation_Id StationId)
    //        => _AllowedMemberIds.Contains(StationId);


    //}


    /// <summary>
    /// A group of EVSEs.
    /// </summary>
    public class EVSEGroup : AEMobilityEntity<EVSEGroup_Id>,
                             IEquatable<EVSEGroup>, IComparable<EVSEGroup>, IComparable,
                             IEnumerable<EVSE>
    {

        #region Data

        /// <summary>
        /// The default max size of the EVSE group status list.
        /// </summary>
        public const UInt16 DefaultMaxGroupStatusListSize       = 15;

        /// <summary>
        /// The default max size of the EVSE group admin status list.
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



        private HashSet<EVSE_Id> _AllowedMemberIds;

        public IEnumerable<EVSE_Id> AllowedMemberIds
            => _AllowedMemberIds;

        public Func<EVSE,    Boolean> AutoIncludeEVSEs      { get; }

        public Func<EVSE_Id, Boolean> AutoIncludeEVSEIds    { get; }



        #region EVSEs

        private readonly ConcurrentDictionary<EVSE_Id, EVSE> _EVSEs;

        /// <summary>
        /// Return all EVSEs registered within this EVSE group.
        /// </summary>
        public IEnumerable<EVSE> EVSEs
            => _EVSEs.Values;


        /// <summary>
        /// Return all EVSE identifications registered within this EVSE group.
        /// </summary>
        public IEnumerable<EVSE_Id> EVSEIds
            => EVSEs.Select(evse => evse.Id);

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current charging pool admin status.
        /// </summary>
        [Dynamic]
        public Timestamped<EVSEGroupAdminStatusTypes> AdminStatus
            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<EVSEGroupAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The charging pool admin status schedule.
        /// </summary>
        [Dynamic]
        public IEnumerable<Timestamped<EVSEGroupAdminStatusTypes>> AdminStatusSchedule
            => _AdminStatusSchedule;

        #endregion


        #region Status

        /// <summary>
        /// The current charging pool status.
        /// </summary>
        [Dynamic]
        public Timestamped<EVSEGroupStatusTypes> Status
            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<EVSEGroupStatusTypes> _StatusSchedule;

        /// <summary>
        /// The charging pool status schedule.
        /// </summary>
        [Dynamic]
        public IEnumerable<Timestamped<EVSEGroupStatusTypes>> StatusSchedule
            => _StatusSchedule;

        #endregion

        #region StatusAggregationDelegate

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated charging stations.
        /// </summary>
        public Func<EVSEStatusReport, EVSEGroupStatusTypes>  StatusAggregationDelegate   { get; }

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

        // EVSEGroup events

        #region OnAdminStatusChanged

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnAdminStatus2ChangedDelegate OnAdminStatusChanged;

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

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE group.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Operator">The charging station operator of this EVSE group.</param>
        /// <param name="Name">The offical (multi-language) name of this EVSE group.</param>
        /// <param name="Description">An optional (multi-language) description of this EVSE group.</param>
        /// 
        /// <param name="Members">An enumeration of charging stations member building this EVSE group.</param>
        /// <param name="MemberIds">An enumeration of charging station identifications which are building this EVSE group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new charging stations automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated charging stations.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the EVSE group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the EVSE group admin status list.</param>
        internal EVSEGroup(EVSEGroup_Id                                  Id,
                           ChargingStationOperator                       Operator,
                           I18NString                                    Name,
                           I18NString                                    Description                  = null,

                           Brand                                         Brand                        = null,
                           Priority?                                     Priority                     = null,
                           ChargingTariff                                Tariff                       = null,
                           IEnumerable<DataLicense>                      DataLicenses                 = null,

                           IEnumerable<EVSE>                             Members                      = null,
                           IEnumerable<EVSE_Id>                          MemberIds                    = null,
                           Func<EVSE_Id, Boolean>                        AutoIncludeEVSEIds           = null,
                           Func<EVSE,    Boolean>                        AutoIncludeEVSEs             = null,

                           Func<EVSEStatusReport, EVSEGroupStatusTypes>  StatusAggregationDelegate    = null,
                           UInt16                                        MaxGroupStatusListSize       = DefaultMaxGroupStatusListSize,
                           UInt16                                        MaxGroupAdminStatusListSize  = DefaultMaxGroupAdminStatusListSize)

            : base(Id)

        {

            #region Initial checks

            if (Operator == null)
                throw new ArgumentNullException(nameof(Operator),  "The charging station operator must not be null!");

            if (IEnumerableExtensions.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(Name),      "The name of the EVSE group must not be null or empty!");

            #endregion

            #region Init data and properties

            this.Operator                    = Operator;
            this.Name                        = Name;
            this.Description                 = Description ?? new I18NString();

            this.Brand                       = Brand;
            this.Priority                    = Priority;
            this.Tariff                      = Tariff;
            this.DataLicenses                = DataLicenses?.Any() == true ? new ReactiveSet<DataLicense>(DataLicenses) : new ReactiveSet<DataLicense>();

            this._AllowedMemberIds           = MemberIds != null ? new HashSet<EVSE_Id>(MemberIds) : new HashSet<EVSE_Id>();
            this.AutoIncludeEVSEIds          = AutoIncludeEVSEIds ?? (MemberIds == null ? (Func<EVSE_Id, Boolean>) (evseid => true) : evseid => false);
            this.AutoIncludeEVSEs            = AutoIncludeEVSEs   ?? (MemberIds == null ? (Func<EVSE,    Boolean>) (evse   => true) : evse   => false);
            this._EVSEs                      = new ConcurrentDictionary<EVSE_Id, EVSE>();

            this.StatusAggregationDelegate   = StatusAggregationDelegate;

            this._AdminStatusSchedule        = new StatusSchedule<EVSEGroupAdminStatusTypes>(MaxGroupAdminStatusListSize);
            this._AdminStatusSchedule.Insert(EVSEGroupAdminStatusTypes.Unknown);

            this._StatusSchedule             = new StatusSchedule<EVSEGroupStatusTypes>     (MaxGroupStatusListSize);
            this._StatusSchedule.     Insert(EVSEGroupStatusTypes.Unknown);

            #endregion

            #region Init events

            // EVSEGroup events
            this.EVSEAddition             = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval              = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events

            #endregion

            #region Link events

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            // EVSEGroup events
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
                Members.ForEach(evse => Add(evse));

        }

        #endregion


        public EVSEGroup Add(EVSE EVSE)
        {

            if (_AllowedMemberIds.Contains(EVSE.Id) &&
                AutoIncludeEVSEs(EVSE))
            {
                _EVSEs.TryAdd(EVSE.Id, EVSE);
            }

            return this;

        }

        public EVSEGroup Add(EVSE_Id EVSEId)
        {

            _AllowedMemberIds.Add(EVSEId);

            return this;

        }


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is member of this EVSE group.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)
            => _EVSEs.ContainsKey(EVSE.Id);

        #endregion

        #region ContainsEVSEId(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is member of this EVSE group.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSEId(EVSE_Id EVSEId)
            => _EVSEs.ContainsKey(EVSEId);

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(EVSEGroupAdminStatusTypes  NewAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<EVSEGroupAdminStatusTypes> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(EVSEGroupAdminStatusTypes  NewAdminStatus,
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
        public void SetAdminStatus(IEnumerable<Timestamped<EVSEGroupAdminStatusTypes>>  NewStatusList,
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
        internal async Task UpdateAdminStatus(DateTime                                Timestamp,
                                              EventTracking_Id                        EventTrackingId,
                                              Timestamped<EVSEGroupAdminStatusTypes>  OldStatus,
                                              Timestamped<EVSEGroupAdminStatusTypes>  NewStatus)
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


        #region IEnumerable<EVSE> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return EVSEs.GetEnumerator();
        }

        public IEnumerator<EVSE> GetEnumerator()
        {
            return EVSEs.GetEnumerator();
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEGroup1, EVSEGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroup1">A EVSE group.</param>
        /// <param name="EVSEGroup2">Another EVSE group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEGroup EVSEGroup1, EVSEGroup EVSEGroup2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSEGroup1, EVSEGroup2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEGroup1 == null) || ((Object) EVSEGroup2 == null))
                return false;

            return EVSEGroup1.Equals(EVSEGroup2);

        }

        #endregion

        #region Operator != (EVSEGroup1, EVSEGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroup1">A EVSE group.</param>
        /// <param name="EVSEGroup2">Another EVSE group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEGroup EVSEGroup1, EVSEGroup EVSEGroup2)
            => !(EVSEGroup1 == EVSEGroup2);

        #endregion

        #region Operator <  (EVSEGroup1, EVSEGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroup1">A EVSE group.</param>
        /// <param name="EVSEGroup2">Another EVSE group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEGroup EVSEGroup1, EVSEGroup EVSEGroup2)
        {

            if ((Object) EVSEGroup1 == null)
                throw new ArgumentNullException(nameof(EVSEGroup1), "The given EVSEGroup1 must not be null!");

            return EVSEGroup1.CompareTo(EVSEGroup2) < 0;

        }

        #endregion

        #region Operator <= (EVSEGroup1, EVSEGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroup1">A EVSE group.</param>
        /// <param name="EVSEGroup2">Another EVSE group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEGroup EVSEGroup1, EVSEGroup EVSEGroup2)
            => !(EVSEGroup1 > EVSEGroup2);

        #endregion

        #region Operator >  (EVSEGroup1, EVSEGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroup1">A EVSE group.</param>
        /// <param name="EVSEGroup2">Another EVSE group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEGroup EVSEGroup1, EVSEGroup EVSEGroup2)
        {

            if ((Object) EVSEGroup1 == null)
                throw new ArgumentNullException(nameof(EVSEGroup1), "The given EVSEGroup1 must not be null!");

            return EVSEGroup1.CompareTo(EVSEGroup2) > 0;

        }

        #endregion

        #region Operator >= (EVSEGroup1, EVSEGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroup1">A EVSE group.</param>
        /// <param name="EVSEGroup2">Another EVSE group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEGroup EVSEGroup1, EVSEGroup EVSEGroup2)
            => !(EVSEGroup1 < EVSEGroup2);

        #endregion

        #endregion

        #region IComparable<EVSEGroup> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var EVSEGroup = Object as EVSEGroup;
            if ((Object) EVSEGroup == null)
                throw new ArgumentException("The given object is not a charging pool!", nameof(Object));

            return CompareTo(EVSEGroup);

        }

        #endregion

        #region CompareTo(EVSEGroup)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroup">A EVSE group object to compare with.</param>
        public Int32 CompareTo(EVSEGroup EVSEGroup)
        {

            if ((Object) EVSEGroup == null)
                throw new ArgumentNullException(nameof(EVSEGroup), "The given EVSE group must not be null!");

            return Id.CompareTo(EVSEGroup.Id);

        }

        #endregion

        #endregion

        #region IEquatable<EVSEGroup> Members

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

            var EVSEGroup = Object as EVSEGroup;
            if ((Object) EVSEGroup == null)
                return false;

            return Equals(EVSEGroup);

        }

        #endregion

        #region Equals(EVSEGroup)

        /// <summary>
        /// Compares two charging pools for equality.
        /// </summary>
        /// <param name="EVSEGroup">A EVSE group to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEGroup EVSEGroup)
        {

            if ((Object) EVSEGroup == null)
                return false;

            return Id.Equals(EVSEGroup.Id);

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
