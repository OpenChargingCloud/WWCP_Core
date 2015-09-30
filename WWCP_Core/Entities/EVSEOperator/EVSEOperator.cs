/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The Electric Vehicle Supply Equipment Operator is responsible for
    /// operating EV Charging Pools and with them also EV Charging Stations
    /// and EVSEs. The EVSE operator is not neccessarily also the owner of
    /// these charging stations. For the delivered service (energy, parking,
    /// etc.) the EVSE operator will either be payed directly by the ev driver
    /// or by an contracted EVSP of the ev driver.
    /// The EVSE operator delivers the locations, characteristics and real-time
    /// status information of its EV Charging Pools as linked open data to EVSPs,
    /// NSPs or the public. Pricing information might be either public
    /// information or part of business-to-business contracts.
    /// </summary>
    public class EVSEOperator : AEMobilityEntity<EVSEOperator_Id>,
                                IEquatable<EVSEOperator>, IComparable<EVSEOperator>, IComparable,
                                IEnumerable<ChargingPool>,
                                IStatus<EVSEOperatorStatusType>
    {

        #region Data

        /// <summary>
        /// The default max size of the aggregated EVSE operator status history.
        /// </summary>
        public const UInt16 DefaultEVSEOperatorStatusHistorySize = 50;

        /// <summary>
        /// The default max size of the aggregated EVSE operator admin status history.
        /// </summary>
        public const UInt16 DefaultEVSEOperatorAdminStatusHistorySize = 50;

        #endregion

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the EVSE Operator.
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
                _Name = value;
            }

        }

        #endregion

        #region Description

        private readonly I18NString _Description;

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
        }

        #endregion

        #region DataLicense

        private readonly DataLicenses _DataLicense;

        /// <summary>
        /// The license of the EVSE Operator data.
        /// </summary>
        [Mandatory]
        public DataLicenses DataLicense
        {
            get
            {
                return _DataLicense;
            }
        }

        #endregion


        #region ValidEVSEIds

        private readonly ReactiveSet<EVSE_Id> _ValidEVSEIds;

        /// <summary>
        /// A list of valid EVSE Ids. All others will be filtered.
        /// </summary>
        public ReactiveSet<EVSE_Id> ValidEVSEIds
        {
            get
            {
                return _ValidEVSEIds;
            }
        }

        #endregion

        #region InvalidEVSEIds

        private readonly ReactiveSet<EVSE_Id> _InvalidEVSEIds;

        /// <summary>
        /// A list of invalid EVSE Ids.
        /// </summary>
        public ReactiveSet<EVSE_Id> InvalidEVSEIds
        {
            get
            {
                return _InvalidEVSEIds;
            }
        }

        #endregion

        #region ManualEVSEIds

        private readonly ReactiveSet<EVSE_Id> _ManualEVSEIds;

        /// <summary>
        /// A list of manual EVSE Ids which will not be touched automagically.
        /// </summary>
        public ReactiveSet<EVSE_Id> ManualEVSEIds
        {
            get
            {
                return _ManualEVSEIds;
            }
        }

        #endregion



        #region AllChargingPools

        public IEnumerable<ChargingPool> AllChargingPools
        {
            get
            {

                return _ChargingPools.
                           Select(kvp => kvp.Value);

            }
        }

        #endregion

        #region AllChargingPoolIds

        public IEnumerable<ChargingPool_Id> AllChargingPoolIds
        {
            get
            {

                return _ChargingPools.
                           Select(kvp => kvp.Value.Id);

            }
        }

        #endregion

        #region AllChargingPoolAdminStatus(IncludePool = null)

        public IEnumerable<KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>> AllChargingPoolAdminStatus(Func<ChargingPool, Boolean> IncludePool = null)
        {

            return _ChargingPools.
                       Select (kvp  => kvp.Value).
                       Where  (pool => IncludePool != null ? IncludePool(pool) : true).
                       OrderBy(pool => pool.Id).
                       Select (pool => new KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>(pool.Id, pool.AdminStatus.Value));

        }

        #endregion


        #region AllChargingStations

        public IEnumerable<ChargingStation> AllChargingStations
        {
            get
            {

                return _ChargingPools.Values.
                           SelectMany(pool => pool.ChargingStations);

            }
        }

        #endregion

        #region AllChargingStationIds

        public IEnumerable<ChargingStation_Id> AllChargingStationIds
        {
            get
            {

                return _ChargingPools.Values.
                           SelectMany(pool    => pool.   ChargingStations).
                           Select    (station => station.Id);

            }
        }

        #endregion

        #region AllChargingStationAdminStatus(IncludeStation = null)

        public IEnumerable<KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>> AllChargingStationAdminStatus(Func<ChargingStation, Boolean> IncludeStation = null)
        {

            return _ChargingPools.Values.
                       SelectMany(pool    => pool.ChargingStations).
                       Where     (station => IncludeStation != null ? IncludeStation(station) : true).
                       OrderBy   (station => station.Id).
                       Select    (station => new KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>(station.Id, station.AdminStatus.Value));

        }

        #endregion


        #region AllEVSEs

        public IEnumerable<EVSE> AllEVSEs
        {
            get
            {

                return _ChargingPools.Values.
                           SelectMany(v => v.ChargingStations).
                           SelectMany(v => v.EVSEs);

            }
        }

        #endregion

        #region AllEVSEIds

        public IEnumerable<EVSE_Id> AllEVSEIds
        {
            get
            {

                return _ChargingPools.Values.
                           SelectMany(v => v.ChargingStations).
                           SelectMany(v => v.EVSEs).
                           Select    (v => v.Id);

            }
        }

        #endregion

        #region AllEVSEStatus(IncludeEVSE = null)

        public IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>> AllEVSEStatus(Func<EVSE, Boolean>  IncludeEVSE = null)
        {

            return _ChargingPools.Values.
                       SelectMany(pool    => pool.ChargingStations).
                       SelectMany(station => station.EVSEs).
                       Where     (evse    => IncludeEVSE != null ? IncludeEVSE(evse) : true).
                       OrderBy   (evse    => evse.Id).
                       Select    (evse    => new KeyValuePair<EVSE_Id, EVSEStatusType>(evse.Id, evse.Status.Value));

        }

        #endregion


        #region Status

        /// <summary>
        /// The current EVSE operator status.
        /// </summary>
        [Optional]
        public Timestamped<EVSEOperatorStatusType> Status
        {
            get
            {
                return _StatusHistory.Peek();
            }
        }

        #endregion

        #region StatusHistory

        private Stack<Timestamped<EVSEOperatorStatusType>> _StatusHistory;

        /// <summary>
        /// The EVSE operator status history.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<EVSEOperatorStatusType>> StatusHistory
        {
            get
            {
                return _StatusHistory.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        #region StatusAggregationDelegate

        private Func<ChargingPoolStatusReport, EVSEOperatorStatusType> _StatusAggregationDelegate;

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated charging pools.
        /// </summary>
        public Func<ChargingPoolStatusReport, EVSEOperatorStatusType> StatusAggregationDelegate
        {

            get
            {
                return _StatusAggregationDelegate;
            }

            set
            {
                _StatusAggregationDelegate = value;
            }

        }

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current EVSE operator admin status.
        /// </summary>
        [Optional]
        public Timestamped<EVSEOperatorAdminStatusType> AdminStatus
        {
            get
            {
                return _AdminStatusHistory.Peek();
            }
        }

        #endregion

        #region AdminStatusHistory

        private Stack<Timestamped<EVSEOperatorAdminStatusType>> _AdminStatusHistory;

        /// <summary>
        /// The EVSE operator admin status history.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<EVSEOperatorAdminStatusType>> AdminStatusHistory
        {
            get
            {
                return _AdminStatusHistory.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        #region AdminStatusAggregationDelegate

        private Func<ChargingPoolAdminStatusReport, EVSEOperatorAdminStatusType> _AdminStatusAggregationDelegate;

        /// <summary>
        /// A delegate called to aggregate the admin status of all subordinated charging pools.
        /// </summary>
        public Func<ChargingPoolAdminStatusReport, EVSEOperatorAdminStatusType> AdminStatusAggregationDelegate
        {

            get
            {
                return _AdminStatusAggregationDelegate;
            }

            set
            {
                _AdminStatusAggregationDelegate = value;
            }

        }

        #endregion


        #region RoamingNetwork

        private readonly RoamingNetwork _RoamingNetwork;

        /// <summary>
        /// The associated EV Roaming Network of the Electric Vehicle Supply Equipment Operator.
        /// </summary>
        public RoamingNetwork RoamingNetwork
        {
            get
            {
                return _RoamingNetwork;
            }
        }

        #endregion

        #region ChargingPools

        private readonly ConcurrentDictionary<ChargingPool_Id, ChargingPool> _ChargingPools;

        /// <summary>
        /// Return all EV Charging Pools registered within this EVSE operator.
        /// </summary>
        public IEnumerable<ChargingPool> ChargingPools
        {
            get
            {
                return _ChargingPools.Select(KVP => KVP.Value);
            }
        }

        #endregion

        #endregion

        #region Events

        // EVSEOperator events

        #region OnAggregatedStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSEOperator">The updated EVSE operator.</param>
        /// <param name="OldStatus">The old timestamped status of the EVSE operator.</param>
        /// <param name="NewStatus">The new timestamped status of the EVSE operator.</param>
        public delegate void OnAggregatedStatusChangedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, Timestamped<EVSEOperatorStatusType> OldStatus, Timestamped<EVSEOperatorStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status changed.
        /// </summary>
        public event OnAggregatedStatusChangedDelegate OnAggregatedStatusChanged;

        #endregion

        #region OnAggregatedAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated admin status changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSEOperator">The updated EVSE operator.</param>
        /// <param name="OldStatus">The old timestamped status of the EVSE operator.</param>
        /// <param name="NewStatus">The new timestamped status of the EVSE operator.</param>
        public delegate void OnAggregatedAdminStatusChangedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, Timestamped<EVSEOperatorAdminStatusType> OldStatus, Timestamped<EVSEOperatorAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated admin status changed.
        /// </summary>
        public event OnAggregatedAdminStatusChangedDelegate OnAggregatedAdminStatusChanged;

        #endregion

        #region ChargingPoolAddition

        internal readonly IVotingNotificator<DateTime, EVSEOperator, ChargingPool, Boolean> ChargingPoolAddition;

        /// <summary>
        /// Called whenever an EVS pool will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EVSEOperator, ChargingPool, Boolean> OnChargingPoolAddition
        {
            get
            {
                return ChargingPoolAddition;
            }
        }

        #endregion

        #region ChargingPoolRemoval

        internal readonly IVotingNotificator<DateTime, EVSEOperator, ChargingPool, Boolean> ChargingPoolRemoval;

        /// <summary>
        /// Called whenever an EVS pool will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EVSEOperator, ChargingPool, Boolean> OnChargingPoolRemoval
        {
            get
            {
                return ChargingPoolRemoval;
            }
        }

        #endregion

        #region OnValidEVSEIdAdded

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public delegate void OnValidEVSEIdAddedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, EVSE_Id EVSEId);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        public event OnValidEVSEIdAddedDelegate OnValidEVSEIdAdded;

        #endregion

        #region OnValidEVSEIdRemoved

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public delegate void OnValidEVSEIdRemovedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, EVSE_Id EVSEId);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        public event OnValidEVSEIdRemovedDelegate OnValidEVSEIdRemoved;

        #endregion

        #region OnInvalidEVSEIdAdded

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public delegate void OnInvalidEVSEIdAddedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, EVSE_Id EVSEId);

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
        public delegate void OnInvalidEVSEIdRemovedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, EVSE_Id EVSEId);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        public event OnInvalidEVSEIdRemovedDelegate OnInvalidEVSEIdRemoved;

        #endregion


        // ChargingPool events

        #region OnChargingPoolDataChanged

        /// <summary>
        /// A delegate called whenever the static data of any subordinated charging pool changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        public delegate void OnChargingPoolDataChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, String PropertyName, Object OldValue, Object NewValue);

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolDataChangedDelegate OnChargingPoolDataChanged;

        #endregion

        #region OnChargingPoolAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the admin status of any subordinated charging pool changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
        public delegate void OnChargingPoolAdminStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<ChargingPoolAdminStatusType> OldStatus, Timestamped<ChargingPoolAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the admin status of any subordinated ChargingPool changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate OnChargingPoolAdminStatusChanged;

        #endregion

        #region OnAggregatedChargingPoolStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old timestamped status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
        public delegate void OnAggregatedChargingPoolStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<ChargingPoolStatusType> OldStatus, Timestamped<ChargingPoolStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnAggregatedChargingPoolStatusChangedDelegate OnAggregatedChargingPoolStatusChanged;

        #endregion

        #region OnAggregatedChargingPoolAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated admin status of any subordinated charging pool changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old timestamped status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
        public delegate void OnAggregatedChargingPoolAdminStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<ChargingPoolAdminStatusType> OldStatus, Timestamped<ChargingPoolAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnAggregatedChargingPoolAdminStatusChangedDelegate OnAggregatedChargingPoolAdminStatusChanged;

        #endregion

        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationAddition
        {
            get
            {
                return ChargingStationAddition;
            }
        }

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationRemoval
        {
            get
            {
                return ChargingStationRemoval;
            }
        }

        #endregion


        // ChargingStation events

        #region OnChargingStationDataChanged

        /// <summary>
        /// A delegate called whenever the static data of any subordinated charging station changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        public delegate void OnChargingStationDataChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, String PropertyName, Object OldValue, Object NewValue);

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationDataChangedDelegate OnChargingStationDataChanged;

        #endregion

        #region OnChargingStationAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the admin status of any subordinated charging station changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old timestamped admin status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging station.</param>
        public delegate void OnChargingStationAdminStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<ChargingStationAdminStatusType> OldStatus, Timestamped<ChargingStationAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the admin status of any subordinated ChargingStation changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate OnChargingStationAdminStatusChanged;

        #endregion

        #region OnAggregatedChargingStationStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old timestamped status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station.</param>
        public delegate void OnAggregatedChargingStationStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<ChargingStationStatusType> OldStatus, Timestamped<ChargingStationStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        public event OnAggregatedChargingStationStatusChangedDelegate OnAggregatedChargingStationStatusChanged;

        #endregion

        #region OnAggregatedChargingStationAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated admin status of any subordinated charging station changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old timestamped status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station.</param>
        public delegate void OnAggregatedChargingStationAdminStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<ChargingStationAdminStatusType> OldStatus, Timestamped<ChargingStationAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated charging station changed.
        /// </summary>
        public event OnAggregatedChargingStationAdminStatusChangedDelegate OnAggregatedChargingStationAdminStatusChanged;

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

        #region OnEVSEDataChanged

        /// <summary>
        /// A delegate called whenever the static data of any subordinated EVSE changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        public delegate void OnEVSEDataChangedDelegate(DateTime Timestamp, EVSE EVSE, String PropertyName, Object OldValue, Object NewValue);

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate OnEVSEDataChanged;

        #endregion

        #region OnEVSEStatusChanged

        /// <summary>
        /// A delegate called whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old timestamped status of the EVSE.</param>
        /// <param name="NewStatus">The new timestamped status of the EVSE.</param>
        public delegate void OnEVSEStatusChangedDelegate(DateTime Timestamp, EVSE EVSE, Timestamped<EVSEStatusType> OldStatus, Timestamped<EVSEStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate OnEVSEStatusChanged;

        #endregion

        #region OnEVSEAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old timestamped admin status of the EVSE.</param>
        /// <param name="NewStatus">The new timestamped admin status of the EVSE.</param>
        public delegate void OnEVSEAdminStatusChangedDelegate(DateTime Timestamp, EVSE EVSE, Timestamped<EVSEAdminStatusType> OldStatus, Timestamped<EVSEAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate OnEVSEAdminStatusChanged;

        #endregion

        #region SocketOutletAddition

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletAddition
        {
            get
            {
                return SocketOutletAddition;
            }
        }

        #endregion

        #region SocketOutletRemoval

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletRemoval;

        /// <summary>
        /// Called whenever a socket outlet will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletRemoval
        {
            get
            {
                return SocketOutletRemoval;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment Operator (EVSEOP) to manage
        /// multiple Electric Vehicle Supply Equipments (EVSEs)
        /// and having the given EVSE operator identification.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE operator.</param>
        /// <param name="Name">The offical (multi-language) name of the EVSE Operator.</param>
        /// <param name="Description">An optional (multi-language) description of the EVSE Operator.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal EVSEOperator(EVSEOperator_Id  Id,
                              I18NString       Name           = null,
                              I18NString       Description    = null,
                              RoamingNetwork   RoamingNetwork = null)

            : base(Id)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException("RoamingNetwork", "The roaming network must not be null!");

            #endregion

            #region Init data and properties

            this._RoamingNetwork            = RoamingNetwork;

            this._Name                      = Name        != null ? Name        : new I18NString();
            this._Description               = Description != null ? Description : new I18NString();

            #region ValidEVSEIds

            this._ValidEVSEIds              = new ReactiveSet<EVSE_Id>();

            _ValidEVSEIds.OnItemAdded += (Timestamp, Set, EVSEId) => {
                var OnValidEVSEIdAddedLocal = OnValidEVSEIdAdded;
                if (OnValidEVSEIdAddedLocal != null)
                    OnValidEVSEIdAddedLocal(Timestamp, this, EVSEId);
            };

            _ValidEVSEIds.OnItemRemoved += (Timestamp, Set, EVSEId) => {
                var OnValidEVSEIdRemovedLocal = OnValidEVSEIdRemoved;
                if (OnValidEVSEIdRemovedLocal != null)
                    OnValidEVSEIdRemovedLocal(Timestamp, this, EVSEId);
            };

            #endregion

            #region InvalidEVSEIds

            this._InvalidEVSEIds            = new ReactiveSet<EVSE_Id>();

            _InvalidEVSEIds.OnItemAdded += (Timestamp, Set, EVSEId) =>
            {
                var OnInvalidEVSEIdAddedLocal = OnInvalidEVSEIdAdded;
                if (OnInvalidEVSEIdAddedLocal != null)
                    OnInvalidEVSEIdAddedLocal(Timestamp, this, EVSEId);
            };

            _InvalidEVSEIds.OnItemRemoved += (Timestamp, Set, EVSEId) =>
            {
                var OnInvalidEVSEIdRemovedLocal = OnInvalidEVSEIdRemoved;
                if (OnInvalidEVSEIdRemovedLocal != null)
                    OnInvalidEVSEIdRemovedLocal(Timestamp, this, EVSEId);
            };

            #endregion

            this._ManualEVSEIds             = new ReactiveSet<EVSE_Id>();

            this._ChargingPools             = new ConcurrentDictionary<ChargingPool_Id, ChargingPool>();

            #endregion

            #region Init events

            // EVSEOperator events
            this.ChargingPoolAddition     = new VotingNotificator<DateTime, EVSEOperator, ChargingPool, Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval      = new VotingNotificator<DateTime, EVSEOperator, ChargingPool, Boolean>(() => new VetoVote(), true);

            // ChargingPool events
            this.ChargingStationAddition  = new VotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval   = new VotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition             = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval              = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events
            this.SocketOutletAddition     = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval      = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            #endregion

            #region Link events

            // EVSEOperator events
            this.OnChargingPoolAddition.   OnVoting       += (timestamp, evseoperator, pool, vote) => RoamingNetwork.ChargingPoolAddition.   SendVoting      (timestamp, evseoperator, pool, vote);
            this.OnChargingPoolAddition.   OnNotification += (timestamp, evseoperator, pool)       => RoamingNetwork.ChargingPoolAddition.   SendNotification(timestamp, evseoperator, pool);

            this.OnChargingPoolRemoval.    OnVoting       += (timestamp, evseoperator, pool, vote) => RoamingNetwork.ChargingPoolRemoval.    SendVoting      (timestamp, evseoperator, pool, vote);
            this.OnChargingPoolRemoval.    OnNotification += (timestamp, evseoperator, pool)       => RoamingNetwork.ChargingPoolRemoval.    SendNotification(timestamp, evseoperator, pool);

            // ChargingPool events
            this.OnChargingStationAddition.OnVoting       += (timestamp, evseoperator, pool, vote) => RoamingNetwork.ChargingStationAddition.SendVoting      (timestamp, evseoperator, pool, vote);
            this.OnChargingStationAddition.OnNotification += (timestamp, evseoperator, pool)       => RoamingNetwork.ChargingStationAddition.SendNotification(timestamp, evseoperator, pool);

            this.OnChargingStationRemoval. OnVoting       += (timestamp, evseoperator, pool, vote) => RoamingNetwork.ChargingStationRemoval. SendVoting      (timestamp, evseoperator, pool, vote);
            this.OnChargingStationRemoval. OnNotification += (timestamp, evseoperator, pool)       => RoamingNetwork.ChargingStationRemoval. SendNotification(timestamp, evseoperator, pool);

            // ChargingStation events
            this.OnEVSEAddition.           OnVoting       += (timestamp, station, evse, vote)      => RoamingNetwork.EVSEAddition.           SendVoting      (timestamp, station, evse, vote);
            this.OnEVSEAddition.           OnNotification += (timestamp, station, evse)            => RoamingNetwork.EVSEAddition.           SendNotification(timestamp, station, evse);

            this.OnEVSERemoval.            OnVoting       += (timestamp, station, evse, vote)      => RoamingNetwork.EVSERemoval.            SendVoting      (timestamp, station, evse, vote);
            this.OnEVSERemoval.            OnNotification += (timestamp, station, evse)            => RoamingNetwork.EVSERemoval.            SendNotification(timestamp, station, evse);

            // EVSE events
            this.SocketOutletAddition.     OnVoting       += (timestamp, evse, outlet, vote)       => RoamingNetwork.SocketOutletAddition.   SendVoting      (timestamp, evse, outlet, vote);
            this.SocketOutletAddition.     OnNotification += (timestamp, evse, outlet)             => RoamingNetwork.SocketOutletAddition.   SendNotification(timestamp, evse, outlet);

            this.SocketOutletRemoval.      OnVoting       += (timestamp, evse, outlet, vote)       => RoamingNetwork.SocketOutletRemoval.    SendVoting      (timestamp, evse, outlet, vote);
            this.SocketOutletRemoval.      OnNotification += (timestamp, evse, outlet)             => RoamingNetwork.SocketOutletRemoval.    SendNotification(timestamp, evse, outlet);

            #endregion

        }

        #endregion


        #region CreateNewChargingPool(ChargingPoolId = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging pool having the given
        /// unique charging pool identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the new charging pool.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging pool before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging pool after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging pool failed.</param>
        public ChargingPool CreateNewChargingPool(ChargingPool_Id                        ChargingPoolId  = null,
                                                  Action<ChargingPool>                   Configurator    = null,
                                                  Action<ChargingPool>                   OnSuccess       = null,
                                                  Action<EVSEOperator, ChargingPool_Id>  OnError         = null)
        {

            #region Initial checks

            if (ChargingPoolId == null)
                ChargingPoolId = ChargingPool_Id.Random(this.Id);

            // Do not throw an exception when an OnError delegate was given!
            if (_ChargingPools.ContainsKey(ChargingPoolId))
            {
                if (OnError == null)
                    throw new ChargingPoolAlreadyExists(ChargingPoolId, this.Id);
                else
                    OnError.FailSafeInvoke(this, ChargingPoolId);
            }

            #endregion

            var _ChargingPool = new ChargingPool(ChargingPoolId, this);

            if (Configurator != null)
                Configurator(_ChargingPool);

            if (ChargingPoolAddition.SendVoting(DateTime.Now, this, _ChargingPool))
            {
                if (_ChargingPools.TryAdd(ChargingPoolId, _ChargingPool))
                {

                    _ChargingPool.OnEVSEDataChanged                             += (Timestamp, EVSE, PropertyName, OldValue, NewValue)
                                                                                    => UpdateEVSEData(Timestamp, EVSE, PropertyName, OldValue, NewValue);

                    _ChargingPool.OnEVSEStatusChanged                           += (Timestamp, EVSE, OldStatus, NewStatus)
                                                                                    => UpdateEVSEStatus(Timestamp, EVSE, OldStatus, NewStatus);

                    _ChargingPool.OnEVSEAdminStatusChanged                      += (Timestamp, EVSE, OldStatus, NewStatus)
                                                                                    => UpdateEVSEAdminStatus(Timestamp, EVSE, OldStatus, NewStatus);


                    _ChargingPool.OnChargingStationDataChanged                  += (Timestamp, ChargingStation, PropertyName, OldValue, NewValue)
                                                                                    => UpdateChargingStationData(Timestamp, ChargingStation, PropertyName, OldValue, NewValue);

                    _ChargingPool.OnChargingStationAdminStatusChanged           += (Timestamp, ChargingStation, OldStatus, NewStatus)
                                                                                    => UpdateChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus);

                    _ChargingPool.OnAggregatedChargingStationStatusChanged      += (Timestamp, ChargingStation, OldStatus, NewStatus)
                                                                                    => UpdateAggregatedChargingStationStatus(Timestamp, ChargingStation, OldStatus, NewStatus);

                    _ChargingPool.OnAggregatedChargingStationAdminStatusChanged += (Timestamp, ChargingStation, OldStatus, NewStatus)
                                                                                    => UpdateAggregatedChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus);


                    _ChargingPool.OnPropertyChanged                             += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                                                                                    => UpdateChargingPoolData(Timestamp, Sender as ChargingPool, PropertyName, OldValue, NewValue);

                    _ChargingPool.OnAdminStatusChanged                          += (Timestamp, ChargingPool, OldStatus, NewStatus)
                                                                                    => UpdateChargingPoolAdminStatus(Timestamp, ChargingPool, OldStatus, NewStatus);

                    _ChargingPool.OnAggregatedStatusChanged                     += (Timestamp, ChargingPool, OldStatus, NewStatus)
                                                                                    => UpdateChargingPoolStatus(Timestamp, ChargingPool, OldStatus, NewStatus);

                    _ChargingPool.OnAggregatedAdminStatusChanged                += (Timestamp, ChargingPool, OldStatus, NewStatus)
                                                                                    => UpdateAggregatedChargingPoolAdminStatus(Timestamp, ChargingPool, OldStatus, NewStatus);


                    OnSuccess.FailSafeInvoke(_ChargingPool);
                    ChargingPoolAddition.SendNotification(DateTime.Now, this, _ChargingPool);
                    return _ChargingPool;

                }
            }

            return null;

        }

        #endregion


        #region ContainsChargingPool(ChargingPool)

        /// <summary>
        /// Check if the given ChargingPool is already present within the EVSE operator.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public Boolean ContainsChargingPool(ChargingPool ChargingPool)
        {
            return _ChargingPools.ContainsKey(ChargingPool.Id);
        }

        #endregion

        #region ContainsChargingPool(ChargingPoolId)

        /// <summary>
        /// Check if the given ChargingPool identification is already present within the EVSE operator.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the charging pool.</param>
        public Boolean ContainsChargingPool(ChargingPool_Id ChargingPoolId)
        {
            return _ChargingPools.ContainsKey(ChargingPoolId);
        }

        #endregion

        #region GetChargingPoolbyId(ChargingPoolId)

        public ChargingPool GetChargingPoolbyId(ChargingPool_Id ChargingPoolId)
        {

            ChargingPool _ChargingPool = null;

            if (_ChargingPools.TryGetValue(ChargingPoolId, out _ChargingPool))
                return _ChargingPool;

            return null;

        }

        #endregion

        #region TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool)

        public Boolean TryGetChargingPoolbyId(ChargingPool_Id ChargingPoolId, out ChargingPool ChargingPool)
        {
            return _ChargingPools.TryGetValue(ChargingPoolId, out ChargingPool);
        }

        #endregion

        #region RemoveChargingPool(ChargingPoolId)

        public ChargingPool RemoveChargingPool(ChargingPool_Id ChargingPoolId)
        {

            ChargingPool _ChargingPool = null;

            if (TryGetChargingPoolbyId(ChargingPoolId, out _ChargingPool))
            {

                if (ChargingPoolRemoval.SendVoting(DateTime.Now, this, _ChargingPool))
                {

                    if (_ChargingPools.TryRemove(ChargingPoolId, out _ChargingPool))
                    {

                        ChargingPoolRemoval.SendNotification(DateTime.Now, this, _ChargingPool);

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

            if (TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool))
            {

                if (ChargingPoolRemoval.SendVoting(DateTime.Now, this, ChargingPool))
                {

                    if (_ChargingPools.TryRemove(ChargingPoolId, out ChargingPool))
                    {

                        ChargingPoolRemoval.SendNotification(DateTime.Now, this, ChargingPool);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, NewStatus, SendUpstream = false)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                           ChargingPoolId,
                                               Timestamped<ChargingPoolAdminStatusType>  NewStatus,
                                               Boolean                                   SendUpstream = false)
        {

            SetChargingPoolAdminStatus(DateTime.Now, ChargingPoolId, NewStatus);

        }

        public void SetChargingPoolAdminStatus(DateTime                                  Timestamp,
                                               ChargingPool_Id                           ChargingPoolId,
                                               Timestamped<ChargingPoolAdminStatusType>  NewStatus,
                                               Boolean                                   SendUpstream = false)
        {

            //if (InvalidChargingPoolIds.Contains(ChargingPoolId))
            //    return;

            ChargingPool _ChargingPool = null;
            if (TryGetChargingPoolbyId(ChargingPoolId, out _ChargingPool))
            {

                _ChargingPool.SetAdminStatus(Timestamp, NewStatus);

                if (SendUpstream)
                {

                    RoamingNetwork.
                        RequestRouter.
                        SendChargingPoolAdminStatusDiff(new ChargingPoolAdminStatusDiff(
                                                               EVSEOperatorId:    Id,
                                                               EVSEOperatorName:  Name,
                                                               NewStatus:         new List<KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>>(),
                                                               ChangedStatus:     new List<KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>>() {
                                                                                          new KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>(ChargingPoolId, NewStatus.Value)
                                                                                      },
                                                               RemovedIds:        new List<ChargingPool_Id>()));

                }

            }

            else
                DebugX.Log("Could not set status for ChargingPool '" + ChargingPoolId.OriginId.ToString() + "'!");

        }

        #endregion



        #region ContainsChargingStation(ChargingStation)

        /// <summary>
        /// Check if the given ChargingStation is already present within the EVSE operator.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation ChargingStation)
        {
            return _ChargingPools.Values.Any(pool => pool.ContainsChargingStation(ChargingStation.Id));
        }

        #endregion

        #region ContainsChargingStation(ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the EVSE operator.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)
        {
            return _ChargingPools.Values.Any(pool => pool.ContainsChargingStation(ChargingStationId));
        }

        #endregion

        #region GetChargingStationbyId(ChargingStationId)

        public ChargingStation GetChargingStationbyId(ChargingStation_Id ChargingStationId)
        {

            return _ChargingPools.Values.
                       SelectMany(pool    => pool.ChargingStations).
                       Where     (station => station.Id == ChargingStationId).
                       FirstOrDefault();

        }

        #endregion

        #region TryGetChargingStationbyId(ChargingStationId, out ChargingStation ChargingStation)

        public Boolean TryGetChargingStationbyId(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {

            ChargingStation = _ChargingPools.Values.
                                  SelectMany(pool    => pool.ChargingStations).
                                  Where     (station => station.Id == ChargingStationId).
                                  FirstOrDefault();

            return ChargingStation != null;

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, NewStatus, SendUpstream = false)

        public void SetChargingStationAdminStatus(ChargingStation_Id                           ChargingStationId,
                                                  Timestamped<ChargingStationAdminStatusType>  NewStatus,
                                                  Boolean                                      SendUpstream = false)
        {

            SetChargingStationAdminStatus(DateTime.Now, ChargingStationId, NewStatus, SendUpstream);

        }

        public void SetChargingStationAdminStatus(DateTime                                     Timestamp,
                                                  ChargingStation_Id                           ChargingStationId,
                                                  Timestamped<ChargingStationAdminStatusType>  NewStatus,
                                                  Boolean                                      SendUpstream = false)
        {

            //if (InvalidChargingStationIds.Contains(ChargingStationId))
            //    return;

            ChargingStation _ChargingStation = null;
            if (TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
            {

                _ChargingStation.SetAdminStatus(Timestamp, NewStatus);

                if (SendUpstream)
                {

                    RoamingNetwork.
                        RequestRouter.
                        SendChargingStationAdminStatusDiff(new ChargingStationAdminStatusDiff(
                                                               EVSEOperatorId:    Id,
                                                               EVSEOperatorName:  Name,
                                                               NewStatus:         new List<KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>>(),
                                                               ChangedStatus:     new List<KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>>() {
                                                                                          new KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>(ChargingStationId, NewStatus.Value)
                                                                                      },
                                                               RemovedIds:        new List<ChargingStation_Id>()));

                }

            }

            else
                DebugX.Log("Could not set status for ChargingStation '" + ChargingStationId.OriginId.ToString() + "'!");

        }

        #endregion



        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the EVSE operator.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)
        {
            return _ChargingPools.Values.Any(pool => pool.ContainsEVSE(EVSE.Id));
        }

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the EVSE operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)
        {
            return _ChargingPools.Values.Any(pool => pool.ContainsEVSE(EVSEId));
        }

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)
        {

            return _ChargingPools.Values.
                       SelectMany(pool    => pool.   ChargingStations).
                       SelectMany(station => station.EVSEs).
                       Where     (evse    => evse.Id == EVSEId).
                       FirstOrDefault();

        }

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = _ChargingPools.Values.
                       SelectMany(pool    => pool.   ChargingStations).
                       SelectMany(station => station.EVSEs).
                       Where     (evse    => evse.Id == EVSEId).
                       FirstOrDefault();

            return EVSE != null;

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewStatus, SendUpstream = false)

        public void SetEVSEStatus(EVSE_Id                      EVSEId,
                                  Timestamped<EVSEStatusType>  NewStatus,
                                  Boolean                      SendUpstream = false)
        {

            SetEVSEStatus(DateTime.Now, EVSEId, NewStatus);

        }

        public void SetEVSEStatus(DateTime                     Timestamp,
                                  EVSE_Id                      EVSEId,
                                  Timestamped<EVSEStatusType>  NewStatus,
                                  Boolean                      SendUpstream = false)
        {

            if (InvalidEVSEIds.Contains(EVSEId))
                return;

            EVSE _EVSE = null;
            if (TryGetEVSEbyId(EVSEId, out _EVSE))
            {

                _EVSE.SetStatus(Timestamp, NewStatus);

                if (SendUpstream)
                {

                    RoamingNetwork.
                        RequestRouter.
                        SendEVSEStatusDiff(new EVSEStatusDiff(EVSEOperatorId:    Id,
                                                              EVSEOperatorName:  Name,
                                                              NewStatus:         new List<KeyValuePair<EVSE_Id, EVSEStatusType>>(),
                                                              ChangedStatus:     new List<KeyValuePair<EVSE_Id, EVSEStatusType>>() {
                                                                                         new KeyValuePair<EVSE_Id, EVSEStatusType>(EVSEId, NewStatus.Value)
                                                                                     },
                                                              RemovedIds:        new List<EVSE_Id>()));

                }

            }

            else
                DebugX.Log("Could not set status for EVSE '" + EVSEId.OriginId.ToString() + "'!");

        }

        #endregion

        #region CalcEVSEStatusDiff(EVSEStatus, IncludeEVSE = null, AutoApply = false)

        public EVSEStatusDiff CalcEVSEStatusDiff(Dictionary<EVSE_Id, EVSEStatusType>  EVSEStatus,
                                                 Func<EVSE, Boolean>                  IncludeEVSE  = null,
                                                 Boolean                              AutoApply    = false)
        {

            if (EVSEStatus == null || EVSEStatus.Count == 0)
                return new EVSEStatusDiff(Id, Name);

            #region Get data...

            var EVSEStatusDiff     = new EVSEStatusDiff(Id, Name);

            // Only ValidEVSEIds!
            // Do nothing with manual EVSE Ids!
            var CurrentEVSEStates  = AllEVSEStatus(IncludeEVSE).
                                         //Where(KVP => ValidEVSEIds. Contains(KVP.Key) &&
                                         //            !ManualEVSEIds.Contains(KVP.Key)).
                                         ToDictionary(v => v.Key, v => v.Value);

            var OldEVSEIds         = new List<EVSE_Id>(CurrentEVSEStates.Keys);

            #endregion

            try
            {

                #region Find new and changed EVSE states

                // Only for ValidEVSEIds!
                // Do nothing with manual EVSE Ids!
                foreach (var NewEVSEStatus in EVSEStatus)
                                                  //Where(KVP => ValidEVSEIds. Contains(KVP.Key) &&
                                                  //            !ManualEVSEIds.Contains(KVP.Key)))
                {

                    // Add to NewEVSEStates, if new EVSE was found!
                    if (!CurrentEVSEStates.ContainsKey(NewEVSEStatus.Key))
                        EVSEStatusDiff.AddNewStatus(NewEVSEStatus);

                    else
                    {

                        // Add to CHANGED, if state of known EVSE changed!
                        if (CurrentEVSEStates[NewEVSEStatus.Key] != NewEVSEStatus.Value)
                            EVSEStatusDiff.AddChangedStatus(NewEVSEStatus);

                        // Remove EVSEId, as it was processed...
                        OldEVSEIds.Remove(NewEVSEStatus.Key);

                    }

                }

                #endregion

                #region Delete what is left in OldEVSEIds!

                EVSEStatusDiff.AddRemovedId(OldEVSEIds);

                #endregion

                if ((EVSEStatusDiff.NewStatus.    Any() ||
                     EVSEStatusDiff.ChangedStatus.Any() ||
                     EVSEStatusDiff.RemovedIds.   Any()) &&
                     AutoApply)
                    ApplyEVSEStatusDiff(EVSEStatusDiff);

                return EVSEStatusDiff;

            }

            catch (Exception e)
            {
                DebugX.Log("GetEVSEStatusDiff led to an exception: " + e.Message);
            }

            // empty!
            return new EVSEStatusDiff(Id, Name);

        }

        #endregion

        #region ApplyEVSEStatusDiff(StatusDiff)

        public EVSEStatusDiff ApplyEVSEStatusDiff(EVSEStatusDiff StatusDiff)
        {

            foreach (var EVSEState in StatusDiff.NewStatus)
                SetEVSEStatus(EVSEState.Key, EVSEState.Value);

            foreach (var EVSEState in StatusDiff.ChangedStatus)
                SetEVSEStatus(EVSEState.Key, EVSEState.Value);

            //Bug: Will duplicate the status diffs!
            //RoamingNetwork.RequestRouter.SendEVSEStatusDiff(StatusDiff);

            return StatusDiff;

        }

        #endregion


        #region (internal) UpdateEVSEData(Timestamp, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateEVSEData(DateTime  Timestamp,
                                     EVSE      EVSE,
                                     String    PropertyName,
                                     Object    OldValue,
                                     Object    NewValue)
        {

            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal != null)
                OnEVSEDataChangedLocal(Timestamp, EVSE, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateEVSEStatus(Timestamp, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update an EVSE status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal void UpdateEVSEStatus(DateTime                     Timestamp,
                                       EVSE                         EVSE,
                                       Timestamped<EVSEStatusType>  OldStatus,
                                       Timestamped<EVSEStatusType>  NewStatus)
        {

            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal != null)
                OnEVSEStatusChangedLocal(Timestamp, EVSE, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update an EVSE admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal void UpdateEVSEAdminStatus(DateTime                          Timestamp,
                                            EVSE                              EVSE,
                                            Timestamped<EVSEAdminStatusType>  OldStatus,
                                            Timestamped<EVSEAdminStatusType>  NewStatus)
        {

            var OnEVSEAdminStatusChangedLocal = OnEVSEAdminStatusChanged;
            if (OnEVSEAdminStatusChangedLocal != null)
                OnEVSEAdminStatusChangedLocal(Timestamp, EVSE, OldStatus, NewStatus);

        }

        #endregion


        #region (internal) UpdateChargingStationData(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateChargingStationData(DateTime         Timestamp,
                                                ChargingStation  ChargingStation,
                                                String           PropertyName,
                                                Object           OldValue,
                                                Object           NewValue)
        {

            var OnChargingStationDataChangedLocal = OnChargingStationDataChanged;
            if (OnChargingStationDataChangedLocal != null)
                OnChargingStationDataChangedLocal(Timestamp, ChargingStation, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal void UpdateChargingStationAdminStatus(DateTime                                     Timestamp,
                                                       ChargingStation                              ChargingStation,
                                                       Timestamped<ChargingStationAdminStatusType>  OldStatus,
                                                       Timestamped<ChargingStationAdminStatusType>  NewStatus)
        {

            var OnChargingStationAdminStatusChangedLocal = OnChargingStationAdminStatusChanged;
            if (OnChargingStationAdminStatusChangedLocal != null)
                OnChargingStationAdminStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateAggregatedChargingStationStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current aggregated charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal void UpdateAggregatedChargingStationStatus(DateTime                                Timestamp,
                                                            ChargingStation                         ChargingStation,
                                                            Timestamped<ChargingStationStatusType>  OldStatus,
                                                            Timestamped<ChargingStationStatusType>  NewStatus)
        {

            var OnAggregatedChargingStationStatusChangedLocal = OnAggregatedChargingStationStatusChanged;
            if (OnAggregatedChargingStationStatusChangedLocal != null)
                OnAggregatedChargingStationStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateAggregatedChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current aggregated charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station admin status.</param>
        /// <param name="NewStatus">The new aggreagted charging station admin status.</param>
        internal void UpdateAggregatedChargingStationAdminStatus(DateTime                                     Timestamp,
                                                                 ChargingStation                              ChargingStation,
                                                                 Timestamped<ChargingStationAdminStatusType>  OldStatus,
                                                                 Timestamped<ChargingStationAdminStatusType>  NewStatus)
        {

            var OnAggregatedChargingStationAdminStatusChangedLocal = OnAggregatedChargingStationAdminStatusChanged;
            if (OnAggregatedChargingStationAdminStatusChangedLocal != null)
                OnAggregatedChargingStationAdminStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

        }

        #endregion


        #region (internal) UpdateChargingPoolData(Timestamp, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateChargingPoolData(DateTime      Timestamp,
                                             ChargingPool  ChargingPool,
                                             String        PropertyName,
                                             Object        OldValue,
                                             Object        NewValue)
        {

            var OnChargingPoolDataChangedLocal = OnChargingPoolDataChanged;
            if (OnChargingPoolDataChangedLocal != null)
                OnChargingPoolDataChangedLocal(Timestamp, ChargingPool, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateChargingPoolAdminStatus(Timestamp, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old charging pool admin status.</param>
        /// <param name="NewStatus">The new charging pool admin status.</param>
        internal void UpdateChargingPoolAdminStatus(DateTime                                  Timestamp,
                                                    ChargingPool                              ChargingPool,
                                                    Timestamped<ChargingPoolAdminStatusType>  OldStatus,
                                                    Timestamped<ChargingPoolAdminStatusType>  NewStatus)
        {

            var OnChargingPoolAdminStatusChangedLocal = OnChargingPoolAdminStatusChanged;
            if (OnChargingPoolAdminStatusChangedLocal != null)
                OnChargingPoolAdminStatusChangedLocal(Timestamp, ChargingPool, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingPoolStatus(Timestamp, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging pool status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal void UpdateChargingPoolStatus(DateTime                             Timestamp,
                                               ChargingPool                         ChargingPool,
                                               Timestamped<ChargingPoolStatusType>  OldStatus,
                                               Timestamped<ChargingPoolStatusType>  NewStatus)
        {

            // Send charging pool status change upstream
            var OnAggregatedChargingPoolStatusChangedLocal = OnAggregatedChargingPoolStatusChanged;
            if (OnAggregatedChargingPoolStatusChangedLocal != null)
                OnAggregatedChargingPoolStatusChangedLocal(Timestamp, ChargingPool, OldStatus, NewStatus);


            // Calculate new aggregated EVSE operator status and send upstream
            if (StatusAggregationDelegate != null)
            {

                var NewAggregatedStatus = new Timestamped<EVSEOperatorStatusType>(StatusAggregationDelegate(new ChargingPoolStatusReport(_ChargingPools.Values)));

                if (NewAggregatedStatus.Value != _StatusHistory.Peek().Value)
                {

                    var OldAggregatedStatus = _StatusHistory.Peek();

                    _StatusHistory.Push(NewAggregatedStatus);

                    var OnAggregatedStatusChangedLocal = OnAggregatedStatusChanged;
                    if (OnAggregatedStatusChangedLocal != null)
                        OnAggregatedStatusChangedLocal(Timestamp, this, OldAggregatedStatus, NewAggregatedStatus);

                }

            }

        }

        #endregion

        #region (internal) UpdateAggregatedChargingPoolAdminStatus(Timestamp, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal void UpdateAggregatedChargingPoolAdminStatus(DateTime                                  Timestamp,
                                                              ChargingPool                              ChargingPool,
                                                              Timestamped<ChargingPoolAdminStatusType>  OldStatus,
                                                              Timestamped<ChargingPoolAdminStatusType>  NewStatus)
        {

            // Send charging pool status change upstream
            var OnAggregatedChargingPoolAdminStatusChangedLocal = OnAggregatedChargingPoolAdminStatusChanged;
            if (OnAggregatedChargingPoolAdminStatusChangedLocal != null)
                OnAggregatedChargingPoolAdminStatusChangedLocal(Timestamp, ChargingPool, OldStatus, NewStatus);


            // Calculate new aggregated EVSE operator status and send upstream
            if (AdminStatusAggregationDelegate != null)
            {

                var NewAggregatedStatus = new Timestamped<EVSEOperatorAdminStatusType>(AdminStatusAggregationDelegate(new ChargingPoolAdminStatusReport(_ChargingPools.Values)));

                if (NewAggregatedStatus.Value != _AdminStatusHistory.Peek().Value)
                {

                    var OldAggregatedStatus = _AdminStatusHistory.Peek();

                    _AdminStatusHistory.Push(NewAggregatedStatus);

                    var OnAggregatedAdminStatusChangedLocal = OnAggregatedAdminStatusChanged;
                    if (OnAggregatedAdminStatusChangedLocal != null)
                        OnAggregatedAdminStatusChangedLocal(Timestamp, this, OldAggregatedStatus, NewAggregatedStatus);

                }

            }

        }

        #endregion


        #region IEnumerable<ChargingPool> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _ChargingPools.Values.GetEnumerator();
        }

        public IEnumerator<ChargingPool> GetEnumerator()
        {
            return _ChargingPools.Values.GetEnumerator();
        }

        #endregion

        #region IComparable<EVSEOperator> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSE_Operator.
            var EVSE_Operator = Object as EVSEOperator;
            if ((Object) EVSE_Operator == null)
                throw new ArgumentException("The given object is not an EVSE_Operator!");

            return CompareTo(EVSE_Operator);

        }

        #endregion

        #region CompareTo(Operator)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Operator">An EVSE operator object to compare with.</param>
        public Int32 CompareTo(EVSEOperator Operator)
        {

            if ((Object) Operator == null)
                throw new ArgumentNullException("The given EVSE operator must not be null!");

            return Id.CompareTo(Operator.Id);

        }

        #endregion

        #endregion

        #region IEquatable<EVSEOperator> Members

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

            // Check if the given object is an EVSEOperator.
            var EVSE_Operator = Object as EVSEOperator;
            if ((Object) EVSE_Operator == null)
                return false;

            return this.Equals(EVSE_Operator);

        }

        #endregion

        #region Equals(Operator)

        /// <summary>
        /// Compares two EVSE operators for equality.
        /// </summary>
        /// <param name="Operator">An EVSE operator to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEOperator Operator)
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
        {
            return Id.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return Id.ToString();
        }

        #endregion

    }

}
