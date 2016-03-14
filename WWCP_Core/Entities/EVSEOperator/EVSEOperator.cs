/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using System.Diagnostics;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The Electric Vehicle Supply Equipment Operator (EVSE operator), also
    /// known as Charge Point Operator (CPO), is responsible for operating
    /// charging pools, charging stations and EVSEs. The EVSE operator is not
    /// neccessarily also the owner of all devices. For the delivered service
    /// (energy, parking, etc.) the EVSE operator will either be payed directly
    /// by the ev driver or by a contracted e-mobility service provider.
    /// The EVSE operator delivers the locations, characteristics and real-time
    /// status information of its charging pools/-stations and EVSEs as Linked
    /// Open Data (LOD) to e-mobility service providers, navigation service
    /// providers and the public. Pricing information can either be public
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
        public const UInt16 DefaultEVSEOperatorStatusHistorySize        = 50;

        /// <summary>
        /// The default max size of the aggregated EVSE operator admin status history.
        /// </summary>
        public const UInt16 DefaultEVSEOperatorAdminStatusHistorySize   = 50;

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
                    SetProperty<String>(ref _Logo, value);
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
                    SetProperty<String>(ref _Homepage, value);
            }

        }

        #endregion

        #region HotlinePhoneNumber

        private String _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the EVSE operator hotline.
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
                    SetProperty<String>(ref _HotlinePhoneNumber, value);
            }

        }

        #endregion


        #region DataLicense

        private DataLicenses _DataLicense;

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

            set
            {
                SetProperty<DataLicenses>(ref _DataLicense, value);
            }

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
                return _StatusSchedule.CurrentStatus;
            }
        }

        #endregion

        #region StatusHistory

        private StatusSchedule<EVSEOperatorStatusType> _StatusSchedule;

        /// <summary>
        /// The EVSE operator status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<EVSEOperatorStatusType>> StatusSchedule
        {
            get
            {
                return _StatusSchedule.OrderByDescending(v => v.Timestamp);
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
                return _AdminStatusSchedule.CurrentStatus;
            }
        }

        #endregion

        #region AdminStatusHistory

        private StatusSchedule<EVSEOperatorAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The EVSE operator admin status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<EVSEOperatorAdminStatusType>> AdminStatusSchedule
        {
            get
            {
                return _AdminStatusSchedule.OrderByDescending(v => v.Timestamp);
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

        #endregion

        #region Links

        #region RemoteEVSEOperator

        private IRemoteEVSEOperator _RemoteEVSEOperator;

        /// <summary>
        /// The remote EVSE Operator.
        /// </summary>
        [Optional]
        public IRemoteEVSEOperator RemoteEVSEOperator
        {

            get
            {
                return _RemoteEVSEOperator;
            }

            set
            {
                _RemoteEVSEOperator = value;
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

        #endregion

        #region Events

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

            this._ChargingPools               = new ConcurrentDictionary<ChargingPool_Id, ChargingPool>();
            this._ChargingStationGroups       = new ConcurrentDictionary<ChargingStationGroup_Id, ChargingStationGroup>();

            this._StatusSchedule              = new StatusSchedule<EVSEOperatorStatusType>();
            this._StatusSchedule.Insert(EVSEOperatorStatusType.Unspecified);

            this._AdminStatusSchedule         = new StatusSchedule<EVSEOperatorAdminStatusType>();
            this._AdminStatusSchedule.Insert(EVSEOperatorAdminStatusType.Unspecified);

            this._ChargingReservations        = new ConcurrentDictionary<ChargingReservation_Id, ChargingPool>();
            this._ChargingSessions            = new ConcurrentDictionary<ChargingSession_Id,     ChargingPool>();

            #endregion

            #region Init events

            // EVSE operator events
            this.ChargingPoolAddition          = new VotingNotificator<DateTime, EVSEOperator,    ChargingPool,         Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval           = new VotingNotificator<DateTime, EVSEOperator,    ChargingPool,         Boolean>(() => new VetoVote(), true);

            // Charging station group events
            this.ChargingStationGroupAddition  = new VotingNotificator<DateTime, EVSEOperator,    ChargingStationGroup, Boolean>(() => new VetoVote(), true);
            this.ChargingStationGroupRemoval   = new VotingNotificator<DateTime, EVSEOperator,    ChargingStationGroup, Boolean>(() => new VetoVote(), true);

            // Charging pool events
            this.ChargingStationAddition       = new VotingNotificator<DateTime, ChargingPool,    ChargingStation,      Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval        = new VotingNotificator<DateTime, ChargingPool,    ChargingStation,      Boolean>(() => new VetoVote(), true);

            // Charging station events
            this.EVSEAddition                  = new VotingNotificator<DateTime, ChargingStation, EVSE,                 Boolean>(() => new VetoVote(), true);
            this.EVSERemoval                   = new VotingNotificator<DateTime, ChargingStation, EVSE,                 Boolean>(() => new VetoVote(), true);

            // EVSE events
            this.SocketOutletAddition          = new VotingNotificator<DateTime, EVSE,            SocketOutlet,         Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval           = new VotingNotificator<DateTime, EVSE,            SocketOutlet,         Boolean>(() => new VetoVote(), true);

            #endregion

            #region Link events

            // EVSEOperator events
            this.OnChargingPoolAddition.   OnVoting       += (timestamp, evseoperator, pool, vote) => RoamingNetwork.ChargingPoolAddition.   SendVoting      (timestamp, evseoperator, pool, vote);
            this.OnChargingPoolAddition.   OnNotification += (timestamp, evseoperator, pool)       => RoamingNetwork.ChargingPoolAddition.   SendNotification(timestamp, evseoperator, pool);

            this.OnChargingPoolRemoval.    OnVoting       += (timestamp, evseoperator, pool, vote) => RoamingNetwork.ChargingPoolRemoval.    SendVoting      (timestamp, evseoperator, pool, vote);
            this.OnChargingPoolRemoval.    OnNotification += (timestamp, evseoperator, pool)       => RoamingNetwork.ChargingPoolRemoval.    SendNotification(timestamp, evseoperator, pool);

            // ChargingPool events
            this.OnChargingStationAddition.OnVoting       += (timestamp, pool, station, vote)      => RoamingNetwork.ChargingStationAddition.SendVoting      (timestamp, pool, station, vote);
            this.OnChargingStationAddition.OnNotification += (timestamp, pool, station)            => RoamingNetwork.ChargingStationAddition.SendNotification(timestamp, pool, station);

            //this.OnChargingStationRemoval. OnVoting       += (timestamp, pool, station, vote)      => RoamingNetwork.ChargingStationRemoval. SendVoting      (timestamp, pool, station, vote);
            this.OnChargingStationRemoval. OnNotification += (timestamp, pool, station)            => RoamingNetwork.ChargingStationRemoval. SendNotification(timestamp,       station);

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

            _LocalEVSEIds = new ReactiveSet<EVSE_Id>();

        }

        #endregion


        #region  Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnEVSEOperatorDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnEVSEOperatorStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnEVSEOperatorAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new admin status.</param>
        public void SetAdminStatus(EVSEOperatorAdminStatusType  NewAdminStatus)
        {

            _AdminStatusSchedule.Insert(NewAdminStatus);

        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<EVSEOperatorAdminStatusType> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(EVSEOperatorAdminStatusType  NewAdminStatus,
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
        public void SetAdminStatus(IEnumerable<Timestamped<EVSEOperatorAdminStatusType>>  NewAdminStatusList,
                                   ChangeMethods                                          ChangeMethod = ChangeMethods.Replace)
        {

            _AdminStatusSchedule.Insert(NewAdminStatusList, ChangeMethod);

        }

        #endregion


        #region (internal) UpdateData(Timestamp, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="Sender">The changed EVSE operator.</param>
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
                await OnDataChangedLocal(Timestamp, Sender as EVSEOperator, PropertyName, OldValue, NewValue);

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
                                         Timestamped<EVSEOperatorStatusType>  OldStatus,
                                         Timestamped<EVSEOperatorStatusType>  NewStatus)
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
                                              Timestamped<EVSEOperatorAdminStatusType>  OldStatus,
                                              Timestamped<EVSEOperatorAdminStatusType>  NewStatus)
        {

            var OnAdminStatusChangedLocal = OnAdminStatusChanged;
            if (OnAdminStatusChangedLocal != null)
                await OnAdminStatusChangedLocal(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region Charging pools

        #region ChargingPoolAddition

        internal readonly IVotingNotificator<DateTime, EVSEOperator, ChargingPool, Boolean> ChargingPoolAddition;

        /// <summary>
        /// Called whenever an charging pool will be or was added.
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
        /// Called whenever an charging pool will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EVSEOperator, ChargingPool, Boolean> OnChargingPoolRemoval
        {
            get
            {
                return ChargingPoolRemoval;
            }
        }

        #endregion


        #region ChargingPools

        private ConcurrentDictionary<ChargingPool_Id, ChargingPool> _ChargingPools;

        public IEnumerable<ChargingPool> ChargingPools
        {
            get
            {

                return _ChargingPools.
                           Select(kvp => kvp.Value);

            }
        }

        #endregion

        #region ChargingPoolIds

        public IEnumerable<ChargingPool_Id> ChargingPoolIds
        {
            get
            {

                return _ChargingPools.
                           Select(kvp => kvp.Value.Id);

            }
        }

        #endregion

        #region ChargingPoolAdminStatus(IncludePool = null)

        public IEnumerable<KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>> ChargingPoolAdminStatus(Func<ChargingPool, Boolean> IncludePool = null)
        {

            return _ChargingPools.
                       Select (kvp  => kvp.Value).
                       Where  (pool => IncludePool != null ? IncludePool(pool) : true).
                       OrderBy(pool => pool.Id).
                       Select (pool => new KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>(pool.Id, pool.AdminStatus.Value));

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
        public ChargingPool CreateNewChargingPool(ChargingPool_Id                          ChargingPoolId             = null,
                                                  Action<ChargingPool>                     Configurator               = null,
                                                  Action<ChargingPool>                     OnSuccess                  = null,
                                                  Action<EVSEOperator, ChargingPool_Id>    OnError                    = null,
                                                  Func<ChargingPool, IRemoteChargingPool>  RemoteChargingPoolCreator  = null)
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

                    _ChargingPool.OnEVSEDataChanged                    += UpdateEVSEData;
                    _ChargingPool.OnEVSEStatusChanged                  += UpdateEVSEStatus;
                    _ChargingPool.OnEVSEAdminStatusChanged             += UpdateEVSEAdminStatus;

                    _ChargingPool.OnChargingStationDataChanged         += UpdateChargingStationData;
                    _ChargingPool.OnChargingStationStatusChanged       += UpdateChargingStationStatus;
                    _ChargingPool.OnChargingStationAdminStatusChanged  += UpdateChargingStationAdminStatus;

                    _ChargingPool.OnDataChanged                        += UpdateChargingPoolData;
                    _ChargingPool.OnStatusChanged                      += UpdateChargingPoolStatus;
                    _ChargingPool.OnAdminStatusChanged                 += UpdateChargingPoolAdminStatus;

                    _ChargingPool.OnNewReservation                     += SendNewReservation;
                    _ChargingPool.OnReservationCancelled               += SendOnReservationCancelled;
                    _ChargingPool.OnNewChargingSession                 += SendNewChargingSession;
                    _ChargingPool.OnNewChargeDetailRecord              += SendNewChargeDetailRecord;


                    OnSuccess.FailSafeInvoke(_ChargingPool);
                    ChargingPoolAddition.SendNotification(DateTime.Now, this, _ChargingPool);

                    if (RemoteChargingPoolCreator != null)
                        _ChargingPool.RemoteChargingPool = RemoteChargingPoolCreator(_ChargingPool);

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

        #region SetChargingPoolAdminStatus(ChargingPoolId, NewStatus)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                           ChargingPoolId,
                                               Timestamped<ChargingPoolAdminStatusType>  NewStatus,
                                               Boolean                                   SendUpstream = false)
        {

            ChargingPool _ChargingPool = null;
            if (TryGetChargingPoolbyId(ChargingPoolId, out _ChargingPool))
                _ChargingPool.SetAdminStatus(NewStatus);

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, NewStatus, Timestamp)

        public void SetChargingPoolAdminStatus(ChargingPool_Id              ChargingPoolId,
                                               ChargingPoolAdminStatusType  NewStatus,
                                               DateTime                     Timestamp)
        {

            ChargingPool _ChargingPool  = null;
            if (TryGetChargingPoolbyId(ChargingPoolId, out _ChargingPool))
                _ChargingPool.SetAdminStatus(NewStatus, Timestamp);

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                                        ChargingPoolId,
                                               IEnumerable<Timestamped<ChargingPoolAdminStatusType>>  StatusList,
                                               ChangeMethods                                          ChangeMethod  = ChangeMethods.Replace)
        {

            ChargingPool _ChargingPool  = null;
            if (TryGetChargingPoolbyId(ChargingPoolId, out _ChargingPool))
                _ChargingPool.SetAdminStatus(StatusList, ChangeMethod);

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendChargingPoolAdminStatusDiff(new ChargingPoolAdminStatusDiff(DateTime.Now,
            //                                               EVSEOperatorId:    Id,
            //                                               EVSEOperatorName:  Name,
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


        #region (internal) UpdateChargingPoolData(Timestamp, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateChargingPoolData(DateTime      Timestamp,
                                                   ChargingPool  ChargingPool,
                                                   String        PropertyName,
                                                   Object        OldValue,
                                                   Object        NewValue)
        {

            var OnChargingPoolDataChangedLocal = OnChargingPoolDataChanged;
            if (OnChargingPoolDataChangedLocal != null)
                await OnChargingPoolDataChangedLocal(Timestamp, ChargingPool, PropertyName, OldValue, NewValue);

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
        internal async Task UpdateChargingPoolStatus(DateTime                             Timestamp,
                                                     ChargingPool                         ChargingPool,
                                                     Timestamped<ChargingPoolStatusType>  OldStatus,
                                                     Timestamped<ChargingPoolStatusType>  NewStatus)
        {

            var OnChargingPoolStatusChangedLocal = OnChargingPoolStatusChanged;
            if (OnChargingPoolStatusChangedLocal != null)
                await OnChargingPoolStatusChangedLocal(Timestamp, ChargingPool, OldStatus, NewStatus);

            if (StatusAggregationDelegate != null)
                _StatusSchedule.Insert(StatusAggregationDelegate(new ChargingPoolStatusReport(_ChargingPools.Values)),
                                       Timestamp);

        }

        #endregion

        #region (internal) UpdateChargingPoolAdminStatus(Timestamp, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal async Task UpdateChargingPoolAdminStatus(DateTime                                  Timestamp,
                                                          ChargingPool                              ChargingPool,
                                                          Timestamped<ChargingPoolAdminStatusType>  OldStatus,
                                                          Timestamped<ChargingPoolAdminStatusType>  NewStatus)
        {

            var OnChargingPoolAdminStatusChangedLocal = OnChargingPoolAdminStatusChanged;
            if (OnChargingPoolAdminStatusChangedLocal != null)
                await OnChargingPoolAdminStatusChangedLocal(Timestamp, ChargingPool, OldStatus, NewStatus);

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

        #endregion

        #region Charging stations

        #region ChargingStations

        public IEnumerable<ChargingStation> ChargingStations
        {
            get
            {

                return _ChargingPools.Values.
                           SelectMany(pool => pool.ChargingStations);

            }
        }

        #endregion

        #region ChargingStationIds

        public IEnumerable<ChargingStation_Id> ChargingStationIds
        {
            get
            {

                return _ChargingPools.Values.
                           SelectMany(pool    => pool.   ChargingStations).
                           Select    (station => station.Id);

            }
        }

        #endregion

        #region ChargingStationAdminStatus(IncludeStation = null)

        public IEnumerable<KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>> ChargingStationAdminStatus(Func<ChargingStation, Boolean> IncludeStation = null)
        {

            return _ChargingPools.Values.
                       SelectMany(pool    => pool.ChargingStations).
                       Where     (station => IncludeStation != null ? IncludeStation(station) : true).
                       OrderBy   (station => station.Id).
                       Select    (station => new KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>(station.Id, station.AdminStatus.Value));

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


        #region SetChargingStationStatus(ChargingStationId, NewStatus)

        public void SetChargingStationStatus(ChargingStation_Id         ChargingStationId,
                                             ChargingStationStatusType  NewStatus)
        {

            ChargingStation _ChargingStation  = null;
            if (TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetStatus(NewStatus);

        }

        #endregion

        #region SetChargingStationStatus(ChargingStationId, NewTimestampedStatus)

        public void SetChargingStationStatus(ChargingStation_Id                      ChargingStationId,
                                             Timestamped<ChargingStationStatusType>  NewTimestampedStatus)
        {

            ChargingStation _ChargingStation = null;
            if (TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetStatus(NewTimestampedStatus);

        }

        #endregion


        #region SetChargingStationAdminStatus(ChargingStationId, NewStatus)

        public void SetChargingStationAdminStatus(ChargingStation_Id              ChargingStationId,
                                                  ChargingStationAdminStatusType  NewStatus)
        {

            ChargingStation _ChargingStation  = null;
            if (TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetAdminStatus(NewStatus);

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, NewTimestampedStatus)

        public void SetChargingStationAdminStatus(ChargingStation_Id                           ChargingStationId,
                                                  Timestamped<ChargingStationAdminStatusType>  NewTimestampedStatus)
        {

            ChargingStation _ChargingStation = null;
            if (TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetAdminStatus(NewTimestampedStatus);

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, NewStatus, Timestamp)

        public void SetChargingStationAdminStatus(ChargingStation_Id              ChargingStationId,
                                                  ChargingStationAdminStatusType  NewStatus,
                                                  DateTime                        Timestamp)
        {

            ChargingStation _ChargingStation  = null;
            if (TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetAdminStatus(NewStatus, Timestamp);

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetChargingStationAdminStatus(ChargingStation_Id                                        ChargingStationId,
                                                  IEnumerable<Timestamped<ChargingStationAdminStatusType>>  StatusList,
                                                  ChangeMethods                                             ChangeMethod  = ChangeMethods.Replace)
        {

            ChargingStation _ChargingStation  = null;
            if (TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
                _ChargingStation.SetAdminStatus(StatusList, ChangeMethod);

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendChargingStationAdminStatusDiff(new ChargingStationAdminStatusDiff(DateTime.Now,
            //                                               EVSEOperatorId:    Id,
            //                                               EVSEOperatorName:  Name,
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


        #region (internal) UpdateChargingStationData(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateChargingStationData(DateTime         Timestamp,
                                                      ChargingStation  ChargingStation,
                                                      String           PropertyName,
                                                      Object           OldValue,
                                                      Object           NewValue)
        {

            var OnChargingStationDataChangedLocal = OnChargingStationDataChanged;
            if (OnChargingStationDataChangedLocal != null)
                await OnChargingStationDataChangedLocal(Timestamp, ChargingStation, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateChargingStationStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current aggregated charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal async Task UpdateChargingStationStatus(DateTime                                Timestamp,
                                                        ChargingStation                         ChargingStation,
                                                        Timestamped<ChargingStationStatusType>  OldStatus,
                                                        Timestamped<ChargingStationStatusType>  NewStatus)
        {

            var OnChargingStationStatusChangedLocal = OnChargingStationStatusChanged;
            if (OnChargingStationStatusChangedLocal != null)
                await OnChargingStationStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current aggregated charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station admin status.</param>
        /// <param name="NewStatus">The new aggreagted charging station admin status.</param>
        internal async Task UpdateChargingStationAdminStatus(DateTime                                     Timestamp,
                                                             ChargingStation                              ChargingStation,
                                                             Timestamped<ChargingStationAdminStatusType>  OldStatus,
                                                             Timestamped<ChargingStationAdminStatusType>  NewStatus)
        {

            var OnChargingStationAdminStatusChangedLocal = OnChargingStationAdminStatusChanged;
            if (OnChargingStationAdminStatusChangedLocal != null)
                await OnChargingStationAdminStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region Charging station groups

        #region ChargingStationGroupAddition

        internal readonly IVotingNotificator<DateTime, EVSEOperator, ChargingStationGroup, Boolean> ChargingStationGroupAddition;

        /// <summary>
        /// Called whenever a charging station group will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EVSEOperator, ChargingStationGroup, Boolean> OnChargingStationGroupAddition
        {
            get
            {
                return ChargingStationGroupAddition;
            }
        }

        #endregion

        #region ChargingStationGroupRemoval

        internal readonly IVotingNotificator<DateTime, EVSEOperator, ChargingStationGroup, Boolean> ChargingStationGroupRemoval;

        /// <summary>
        /// Called whenever an charging station group will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EVSEOperator, ChargingStationGroup, Boolean> OnChargingStationGroupRemoval
        {
            get
            {
                return ChargingStationGroupRemoval;
            }
        }

        #endregion


        #region ChargingStationGroups

        private readonly ConcurrentDictionary<ChargingStationGroup_Id, ChargingStationGroup> _ChargingStationGroups;

        /// <summary>
        /// All charging station groups registered within this EVSE operator.
        /// </summary>
        public IEnumerable<ChargingStationGroup> ChargingStationGroups
        {
            get
            {
                return _ChargingStationGroups.Select(KVP => KVP.Value);
            }
        }

        #endregion

        #region CreateNewChargingStationGroup(ChargingStationGroupId = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="ChargingStationGroupId">The unique identification of the new charging group.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging group before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        public ChargingStationGroup CreateNewChargingStationGroup(ChargingStationGroup_Id                        ChargingStationGroupId  = null,
                                                                  Action<ChargingStationGroup>                   Configurator            = null,
                                                                  Action<ChargingStationGroup>                   OnSuccess               = null,
                                                                  Action<EVSEOperator, ChargingStationGroup_Id>  OnError                 = null)
        {

            #region Initial checks

            if (ChargingStationGroupId == null)
                ChargingStationGroupId = ChargingStationGroup_Id.Random(this.Id);

            // Do not throw an exception when an OnError delegate was given!
            if (_ChargingStationGroups.ContainsKey(ChargingStationGroupId))
            {
                if (OnError == null)
                    throw new ChargingStationGroupAlreadyExists(ChargingStationGroupId, this.Id);
                else
                    OnError.FailSafeInvoke(this, ChargingStationGroupId);
            }

            #endregion

            var _ChargingStationGroup = new ChargingStationGroup(ChargingStationGroupId, this);

            if (Configurator != null)
                Configurator(_ChargingStationGroup);

            if (ChargingStationGroupAddition.SendVoting(DateTime.Now, this, _ChargingStationGroup))
            {
                if (_ChargingStationGroups.TryAdd(ChargingStationGroupId, _ChargingStationGroup))
                {

                    _ChargingStationGroup.OnEVSEDataChanged                             += UpdateEVSEData;
                    _ChargingStationGroup.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                    _ChargingStationGroup.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;

                    _ChargingStationGroup.OnChargingStationDataChanged                  += UpdateChargingStationData;
                    _ChargingStationGroup.OnChargingStationStatusChanged                += UpdateChargingStationStatus;
                    _ChargingStationGroup.OnChargingStationAdminStatusChanged           += UpdateChargingStationAdminStatus;

                    //_ChargingStationGroup.OnDataChanged                                 += UpdateChargingStationGroupData;
                    //_ChargingStationGroup.OnAdminStatusChanged                          += UpdateChargingStationGroupAdminStatus;

                    OnSuccess.FailSafeInvoke(_ChargingStationGroup);
                    ChargingStationGroupAddition.SendNotification(DateTime.Now, this, _ChargingStationGroup);
                    return _ChargingStationGroup;

                }
            }

            return null;

        }

        #endregion

        #region GetOrCreateChargingStationGroup(...)

        public ChargingStationGroup GetOrCreateChargingStationGroup(ChargingStationGroup_Id                        ChargingStationGroupId,
                                                                    Action<ChargingStationGroup>                   Configurator            = null,
                                                                    Action<ChargingStationGroup>                   OnSuccess               = null,
                                                                    Action<EVSEOperator, ChargingStationGroup_Id>  OnError                 = null)
        {

            ChargingStationGroup _ChargingStationGroup = null;

            if (_ChargingStationGroups.TryGetValue(ChargingStationGroupId, out _ChargingStationGroup))
                return _ChargingStationGroup;

            return CreateNewChargingStationGroup(ChargingStationGroupId,
                                                 Configurator,
                                                 OnSuccess,
                                                 OnError);

        }

        #endregion

        #region TryGetChargingStationGroup

        public Boolean TryGetChargingStationGroup(ChargingStationGroup_Id   ChargingStationGroupId,
                                                  out ChargingStationGroup  ChargingStationGroup)
        {

            return _ChargingStationGroups.TryGetValue(ChargingStationGroupId, out ChargingStationGroup);

        }

        #endregion

        #endregion

        #region EVSEs

        #region EVSEs

        public IEnumerable<EVSE> EVSEs
        {
            get
            {

                return _ChargingPools.Values.
                           SelectMany(v => v.ChargingStations).
                           SelectMany(v => v.EVSEs);

            }
        }

        #endregion

        #region EVSEIds

        public IEnumerable<EVSE_Id> EVSEIds
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

        #region LocalEVSEIds

        private readonly ReactiveSet<EVSE_Id> _LocalEVSEIds;

        /// <summary>
        /// A list of manual EVSE Ids which will not be touched automagically.
        /// </summary>
        public ReactiveSet<EVSE_Id> LocalEVSEIds
        {
            get
            {
                return _LocalEVSEIds;
            }
        }

        #endregion


        #region SetEVSEStatus(EVSEId, NewStatus)

        public void SetEVSEStatus(EVSE_Id         EVSEId,
                                  EVSEStatusType  NewStatus)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEbyId(EVSEId, out _EVSE))
                _EVSE.SetStatus(NewStatus);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewTimestampedStatus)

        public void SetEVSEStatus(EVSE_Id                      EVSEId,
                                  Timestamped<EVSEStatusType>  NewTimestampedStatus)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEbyId(EVSEId, out _EVSE))
                _EVSE.SetStatus(NewTimestampedStatus);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewStatus, Timestamp)

        public void SetEVSEStatus(EVSE_Id         EVSEId,
                                  EVSEStatusType  NewStatus,
                                  DateTime        Timestamp)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEbyId(EVSEId, out _EVSE))
                _EVSE.SetStatus(NewStatus, Timestamp);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEVSEStatus(EVSE_Id                                   EVSEId,
                                  IEnumerable<Timestamped<EVSEStatusType>>  StatusList,
                                  ChangeMethods                             ChangeMethod  = ChangeMethods.Replace)
        {

            if (InvalidEVSEIds.Contains(EVSEId))
                return;

            EVSE _EVSE  = null;
            if (TryGetEVSEbyId(EVSEId, out _EVSE))
                _EVSE.SetStatus(StatusList, ChangeMethod);

        }

        #endregion

        #region CalcEVSEStatusDiff(EVSEStatus, IncludeEVSE = null)

        public EVSEStatusDiff CalcEVSEStatusDiff(Dictionary<EVSE_Id, EVSEStatusType>  EVSEStatus,
                                                 Func<EVSE, Boolean>                  IncludeEVSE  = null)
        {

            if (EVSEStatus == null || EVSEStatus.Count == 0)
                return new EVSEStatusDiff(DateTime.Now, Id, Name);

            #region Get data...

            var EVSEStatusDiff     = new EVSEStatusDiff(DateTime.Now, Id, Name);

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

                return EVSEStatusDiff;

            }

            catch (Exception e)
            {

                while (e.InnerException != null)
                    e = e.InnerException;

                DebugX.Log("GetEVSEStatusDiff led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

            }

            // empty!
            return new EVSEStatusDiff(DateTime.Now, Id, Name);

        }

        #endregion

        #region ApplyEVSEStatusDiff(EVSEStatusDiff)

        public EVSEStatusDiff ApplyEVSEStatusDiff(EVSEStatusDiff EVSEStatusDiff)
        {

            #region Initial checks

            if (EVSEStatusDiff == null)
                throw new ArgumentNullException("EVSEStatusDiff", "The given EVSE status diff must not be null!");

            #endregion

            foreach (var EVSEStatus in EVSEStatusDiff.NewStatus)
                SetEVSEStatus(EVSEStatus.Key, EVSEStatus.Value);

            foreach (var EVSEStatus in EVSEStatusDiff.ChangedStatus)
                SetEVSEStatus(EVSEStatus.Key, EVSEStatus.Value);

            return EVSEStatusDiff;

        }

        #endregion


        #region SetEVSEAdminStatus(EVSEId, NewAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id              EVSEId,
                                       EVSEAdminStatusType  NewAdminStatus)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEbyId(EVSEId, out _EVSE))
                _EVSE.SetAdminStatus(NewAdminStatus);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, NewTimestampedAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id                           EVSEId,
                                       Timestamped<EVSEAdminStatusType>  NewTimestampedAdminStatus)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEbyId(EVSEId, out _EVSE))
                _EVSE.SetAdminStatus(NewTimestampedAdminStatus);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, NewAdminStatus, Timestamp)

        public void SetEVSEAdminStatus(EVSE_Id              EVSEId,
                                       EVSEAdminStatusType  NewAdminStatus,
                                       DateTime             Timestamp)
        {

            EVSE _EVSE = null;
            if (TryGetEVSEbyId(EVSEId, out _EVSE))
                _EVSE.SetAdminStatus(NewAdminStatus, Timestamp);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, AdminStatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEVSEAdminStatus(EVSE_Id                                        EVSEId,
                                       IEnumerable<Timestamped<EVSEAdminStatusType>>  AdminStatusList,
                                       ChangeMethods                                  ChangeMethod  = ChangeMethods.Replace)
        {

            if (InvalidEVSEIds.Contains(EVSEId))
                return;

            EVSE _EVSE  = null;
            if (TryGetEVSEbyId(EVSEId, out _EVSE))
                _EVSE.SetAdminStatus(AdminStatusList, ChangeMethod);

        }

        #endregion

        #region ApplyEVSEAdminStatusDiff(EVSEAdminStatusDiff)

        public EVSEAdminStatusDiff ApplyEVSEAdminStatusDiff(EVSEAdminStatusDiff EVSEAdminStatusDiff)
        {

            #region Initial checks

            if (EVSEAdminStatusDiff == null)
                throw new ArgumentNullException("EVSEAdminStatusDiff", "The given EVSE admin status diff must not be null!");

            #endregion

            foreach (var EVSEAdminStatus in EVSEAdminStatusDiff.NewStatus)
                SetEVSEAdminStatus(EVSEAdminStatus.Key, EVSEAdminStatus.Value);

            foreach (var EVSEAdminStatus in EVSEAdminStatusDiff.ChangedStatus)
                SetEVSEAdminStatus(EVSEAdminStatus.Key, EVSEAdminStatus.Value);

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


        #region (internal) UpdateEVSEData(Timestamp, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateEVSEData(DateTime  Timestamp,
                                           EVSE      EVSE,
                                           String    PropertyName,
                                           Object    OldValue,
                                           Object    NewValue)
        {

            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal != null)
                await OnEVSEDataChangedLocal(Timestamp, EVSE, PropertyName, OldValue, NewValue);

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
        internal async Task UpdateEVSEStatus(DateTime                     Timestamp,
                                             EVSE                         EVSE,
                                             Timestamped<EVSEStatusType>  OldStatus,
                                             Timestamped<EVSEStatusType>  NewStatus)
        {

            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal != null)
                await OnEVSEStatusChangedLocal(Timestamp, EVSE, OldStatus, NewStatus);

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
        internal async Task UpdateEVSEAdminStatus(DateTime                          Timestamp,
                                                  EVSE                              EVSE,
                                                  Timestamped<EVSEAdminStatusType>  OldStatus,
                                                  Timestamped<EVSEAdminStatusType>  NewStatus)
        {

            var OnEVSEAdminStatusChangedLocal = OnEVSEAdminStatusChanged;
            if (OnEVSEAdminStatusChangedLocal != null)
                await OnEVSEAdminStatusChangedLocal(Timestamp, EVSE, OldStatus, NewStatus);

        }

        #endregion

        #endregion


        #region Reservations

        #region ChargingReservations

        private readonly ConcurrentDictionary<ChargingReservation_Id, ChargingPool>  _ChargingReservations;

        /// <summary>
        /// Return all current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
        {
            get
            {

                return _ChargingPools.Values.
                           SelectMany(pool => pool.ChargingReservations);

            }
        }

        #endregion

        #region OnReserve... / OnReserved... / OnNewReservation

        /// <summary>
        /// An event fired whenever an EVSE is being reserved.
        /// </summary>
        public event OnEVSEReserveDelegate              OnReserveEVSE;

        /// <summary>
        /// An event fired whenever an EVSE was reserved.
        /// </summary>
        public event OnEVSEReservedDelegate             OnEVSEReserved;

        /// <summary>
        /// An event fired whenever a charging station is being reserved.
        /// </summary>
        public event OnChargingStationReserveDelegate   OnReserveChargingStation;

        /// <summary>
        /// An event fired whenever a charging station was reserved.
        /// </summary>
        public event OnChargingStationReservedDelegate  OnChargingStationReserved;

        /// <summary>
        /// An event fired whenever a charging pool is being reserved.
        /// </summary>
        public event OnChargingPoolReserveDelegate      OnReserveChargingPool;

        /// <summary>
        /// An event fired whenever a charging pool was reserved.
        /// </summary>
        public event OnChargingPoolReservedDelegate     OnChargingPoolReserved;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate           OnNewReservation;

        #endregion

        #region Reserve(...EVSEId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(DateTime                 Timestamp,
                    CancellationToken        CancellationToken,
                    EventTracking_Id         EventTrackingId,
                    EVSE_Id                  EVSEId,
                    DateTime?                StartTime          = null,
                    TimeSpan?                Duration           = null,
                    ChargingReservation_Id   ReservationId      = null,
                    EVSP_Id                  ProviderId         = null,
                    eMA_Id                   eMAId              = null,
                    ChargingProduct_Id       ChargingProductId  = null,
                    IEnumerable<Auth_Token>  AuthTokens         = null,
                    IEnumerable<eMA_Id>      eMAIds             = null,
                    IEnumerable<UInt32>      PINs               = null,
                    TimeSpan?                QueryTimeout       = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId), "The given EVSE identification must not be null!");

            ReservationResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveEVSE event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnReserveEVSELocal = OnReserveEVSE;
                if (OnReserveEVSELocal != null)
                    OnReserveEVSELocal(this,
                                       Timestamp,
                                       EventTrackingId,
                                       RoamingNetwork.Id,
                                       ReservationId,
                                       EVSEId,
                                       StartTime,
                                       Duration,
                                       ProviderId,
                                       eMAId,
                                       ChargingProductId,
                                       AuthTokens,
                                       eMAIds,
                                       PINs,
                                       QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnReserveEVSE));
            }

            #endregion


            if (_RemoteEVSEOperator != null)
            {

                result = await _RemoteEVSEOperator.Reserve(Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           EVSEId,
                                                           StartTime,
                                                           Duration,
                                                           ReservationId,
                                                           ProviderId,
                                                           eMAId,
                                                           ChargingProductId,
                                                           AuthTokens,
                                                           eMAIds,
                                                           PINs,
                                                           QueryTimeout);


                if (result.Result == ReservationResultType.Success)
                {

                    //result.Reservation.EVSEOperator = this;

                    var OnNewReservationLocal = OnNewReservation;
                    if (OnNewReservationLocal != null)
                        OnNewReservationLocal(DateTime.Now, this, result.Reservation);

                }

            }

            if (result == null)
            {

                var _ChargingPool = EVSEs.Where (evse => evse.Id == EVSEId).
                                          Select(evse => evse.ChargingStation.ChargingPool).
                                          FirstOrDefault();

                if (_ChargingPool != null)
                {

                    result = await _ChargingPool.Reserve(Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         EVSEId,
                                                         StartTime,
                                                         Duration,
                                                         ReservationId,
                                                         ProviderId,
                                                         eMAId,
                                                         ChargingProductId,
                                                         AuthTokens,
                                                         eMAIds,
                                                         PINs,
                                                         QueryTimeout);

                    if (result.Result == ReservationResultType.Success)
                        _ChargingReservations.TryAdd(result.Reservation.Id, _ChargingPool);

                }

                else
                    result = ReservationResult.UnknownEVSE;

            }

            else
                result = ReservationResult.UnknownEVSE;


            #region Send OnEVSEReserved event

            Runtime.Stop();

            try
            {

                var OnEVSEReservedLocal = OnEVSEReserved;
                if (OnEVSEReservedLocal != null)
                    OnEVSEReservedLocal(this,
                                        Timestamp,
                                        EventTrackingId,
                                        RoamingNetwork.Id,
                                        ReservationId,
                                        EVSEId,
                                        StartTime,
                                        Duration,
                                        ProviderId,
                                        eMAId,
                                        ChargingProductId,
                                        AuthTokens,
                                        eMAIds,
                                        PINs,
                                        result,
                                        Runtime.Elapsed,
                                        QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnEVSEReserved));
            }

            #endregion

            return result;

        }

        #endregion

        #region Reserve(...ChargingStationId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStationId">The unique identification of the charging station to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(DateTime                 Timestamp,
                    CancellationToken        CancellationToken,
                    EventTracking_Id         EventTrackingId,
                    ChargingStation_Id       ChargingStationId,
                    DateTime?                StartTime          = null,
                    TimeSpan?                Duration           = null,
                    ChargingReservation_Id   ReservationId      = null,
                    EVSP_Id                  ProviderId         = null,
                    eMA_Id                   eMAId              = null,
                    ChargingProduct_Id       ChargingProductId  = null,
                    IEnumerable<Auth_Token>  AuthTokens         = null,
                    IEnumerable<eMA_Id>      eMAIds             = null,
                    IEnumerable<UInt32>      PINs               = null,
                    TimeSpan?                QueryTimeout       = null)

        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            ReservationResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveChargingStation event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnReserveChargingStationLocal = OnReserveChargingStation;
                if (OnReserveChargingStationLocal != null)
                    OnReserveChargingStationLocal(this,
                                                  Timestamp,
                                                  EventTrackingId,
                                                  RoamingNetwork.Id,
                                                  ChargingStationId,
                                                  StartTime,
                                                  Duration,
                                                  ReservationId,
                                                  ProviderId,
                                                  eMAId,
                                                  ChargingProductId,
                                                  AuthTokens,
                                                  eMAIds,
                                                  PINs,
                                                  QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnReserveChargingStation));
            }

            #endregion


            var _ChargingPool  = ChargingStations.
                                     Where (station => station.Id == ChargingStationId).
                                     Select(station => station.ChargingPool).
                                     FirstOrDefault();

            if (_ChargingPool != null)
            {

                result = await _ChargingPool.Reserve(Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     ChargingStationId,
                                                     StartTime,
                                                     Duration,
                                                     ReservationId,
                                                     ProviderId,
                                                     eMAId,
                                                     ChargingProductId,
                                                     AuthTokens,
                                                     eMAIds,
                                                     PINs,
                                                     QueryTimeout);

                if (result.Result == ReservationResultType.Success)
                    _ChargingReservations.TryAdd(result.Reservation.Id, _ChargingPool);

            }

            else
                result = ReservationResult.UnknownChargingStation;


            #region Send OnChargingStationReserved event

            Runtime.Stop();

            try
            {

                var OnChargingStationReservedLocal = OnChargingStationReserved;
                if (OnChargingStationReservedLocal != null)
                    OnChargingStationReservedLocal(this,
                                                   Timestamp,
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   ChargingStationId,
                                                   StartTime,
                                                   Duration,
                                                   ReservationId,
                                                   ProviderId,
                                                   eMAId,
                                                   ChargingProductId,
                                                   AuthTokens,
                                                   eMAIds,
                                                   PINs,
                                                   result,
                                                   Runtime.Elapsed,
                                                   QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnChargingStationReserved));
            }

            #endregion

            return result;

        }

        #endregion

        #region Reserve(...ChargingPoolId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge within the given charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPoolId">The unique identification of the charging pool to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(DateTime                 Timestamp,
                    CancellationToken        CancellationToken,
                    EventTracking_Id         EventTrackingId,
                    ChargingPool_Id          ChargingPoolId,
                    DateTime?                StartTime          = null,
                    TimeSpan?                Duration           = null,
                    ChargingReservation_Id   ReservationId      = null,
                    EVSP_Id                  ProviderId         = null,
                    eMA_Id                   eMAId              = null,
                    ChargingProduct_Id       ChargingProductId  = null,
                    IEnumerable<Auth_Token>  AuthTokens         = null,
                    IEnumerable<eMA_Id>      eMAIds             = null,
                    IEnumerable<UInt32>      PINs               = null,
                    TimeSpan?                QueryTimeout       = null)

        {

            #region Initial checks

            if (ChargingPoolId == null)
                throw new ArgumentNullException(nameof(ChargingPoolId),  "The given charging pool identification must not be null!");

            ReservationResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveChargingPool event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnReserveChargingPoolLocal = OnReserveChargingPool;
                if (OnReserveChargingPoolLocal != null)
                    OnReserveChargingPoolLocal(this,
                                               Timestamp,
                                               EventTrackingId,
                                               RoamingNetwork.Id,
                                               ChargingPoolId,
                                               StartTime,
                                               Duration,
                                               ReservationId,
                                               ProviderId,
                                               eMAId,
                                               ChargingProductId,
                                               AuthTokens,
                                               eMAIds,
                                               PINs,
                                               QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnReserveChargingPool));
            }

            #endregion


            var _ChargingPool  = ChargingPools.
                                     Where(pool => pool.Id == ChargingPoolId).
                                     FirstOrDefault();

            if (_ChargingPool != null)
            {

                result = await _ChargingPool.Reserve(Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     ChargingPoolId,
                                                     StartTime,
                                                     Duration,
                                                     ReservationId,
                                                     ProviderId,
                                                     eMAId,
                                                     ChargingProductId,
                                                     AuthTokens,
                                                     eMAIds,
                                                     PINs,
                                                     QueryTimeout);

                if (result.Result == ReservationResultType.Success)
                    _ChargingReservations.TryAdd(result.Reservation.Id, _ChargingPool);

            }

            else
                result = ReservationResult.UnknownChargingStation;


            #region Send OnChargingPoolReserved event

            Runtime.Stop();

            try
            {

                var OnChargingPoolReservedLocal = OnChargingPoolReserved;
                if (OnChargingPoolReservedLocal != null)
                    OnChargingPoolReservedLocal(this,
                                                Timestamp,
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                ChargingPoolId,
                                                StartTime,
                                                Duration,
                                                ReservationId,
                                                ProviderId,
                                                eMAId,
                                                ChargingProductId,
                                                AuthTokens,
                                                eMAIds,
                                                PINs,
                                                result,
                                                Runtime.Elapsed,
                                                QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnChargingPoolReserved));
            }

            #endregion

            return result;

        }

        #endregion

        #region (internal) SendNewReservation(Timestamp, Sender, Reservation)

        internal void SendNewReservation(DateTime             Timestamp,
                                         Object               Sender,
                                         ChargingReservation  Reservation)
        {

            var OnNewReservationLocal = OnNewReservation;
            if (OnNewReservationLocal != null)
                OnNewReservationLocal(Timestamp, Sender, Reservation);

        }

        #endregion


        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by its unique identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation identification.</param>
        /// <returns>True when successful, false otherwise.</returns>
        public Boolean TryGetReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
        {

            ChargingPool _ChargingPool = null;

            if (_ChargingReservations.TryGetValue(ReservationId, out _ChargingPool))
                return _ChargingPool.TryGetReservationById(ReservationId, out Reservation);

            Reservation = null;
            return false;

        }

        #endregion


        #region CancelReservation(...ReservationId, Reason, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult> CancelReservation(DateTime                               Timestamp,
                                                                     CancellationToken                      CancellationToken,
                                                                     EventTracking_Id                       EventTrackingId,
                                                                     ChargingReservation_Id                 ReservationId,
                                                                     ChargingReservationCancellationReason  Reason,
                                                                     TimeSpan?                              QueryTimeout  = null)
        {

            CancelReservationResult result         = null;
            ChargingPool            _ChargingPool  = null;

            if (_ChargingReservations.TryRemove(ReservationId, out _ChargingPool))
                result = await _ChargingPool.CancelReservation(Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               ReservationId,
                                                               Reason,
                                                               QueryTimeout);

            else
            {

                foreach (var __ChargingPool in _ChargingPools)
                {

                    result = await __ChargingPool.Value.CancelReservation(Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          ReservationId,
                                                                          Reason,
                                                                          QueryTimeout);

                    if (result != null && result.Result != CancelReservationResultType.UnknownReservationId)
                        break;

                }

            }

            return result;

        }

        #endregion

        #region OnReservationCancelled

        /// <summary>
        /// An event fired whenever a charging reservation was deleted.
        /// </summary>
        public event OnReservationCancelledInternalDelegate OnReservationCancelled;

        #endregion

        #region SendOnReservationCancelled(...)

        private void SendOnReservationCancelled(DateTime                               Timestamp,
                                                Object                                 Sender,
                                                EventTracking_Id                       EventTrackingId,
                                                ChargingReservation_Id                 ReservationId,
                                                ChargingReservationCancellationReason  Reason)
        {

            ChargingPool _ChargingPool = null;

            _ChargingReservations.TryRemove(ReservationId, out _ChargingPool);

            var OnReservationCancelledLocal = OnReservationCancelled;
            if (OnReservationCancelledLocal != null)
                OnReservationCancelledLocal(Timestamp,
                                            Sender,
                                            EventTrackingId,
                                            ReservationId,
                                            Reason);

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions

        #region ChargingSessions

        private readonly ConcurrentDictionary<ChargingSession_Id, ChargingPool>  _ChargingSessions;

        /// <summary>
        /// Return all current charging sessions.
        /// </summary>
        public IEnumerable<ChargingSession> ChargingSessions
        {
            get
            {

                return _ChargingPools.Values.
                           SelectMany(pool => pool.ChargingSessions);

            }
        }

        #endregion

        #region OnRemote...Start / OnRemote...Started / OnNewChargingSession

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteEVSEStartDelegate               OnRemoteEVSEStart;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteEVSEStartedDelegate             OnRemoteEVSEStarted;

        /// <summary>
        /// An event fired whenever a remote start charging station command was received.
        /// </summary>
        public event OnRemoteChargingStationStartDelegate    OnRemoteChargingStationStart;

        /// <summary>
        /// An event fired whenever a remote start charging station command completed.
        /// </summary>
        public event OnRemoteChargingStationStartedDelegate  OnRemoteChargingStationStarted;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate            OnNewChargingSession;

        #endregion

        #region RemoteStart(...EVSEId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartEVSEResult>

            RemoteStart(DateTime                Timestamp,
                        CancellationToken       CancellationToken,
                        EventTracking_Id        EventTrackingId,
                        EVSE_Id                 EVSEId,
                        ChargingProduct_Id      ChargingProductId  = null,
                        ChargingReservation_Id  ReservationId      = null,
                        ChargingSession_Id      SessionId          = null,
                        EVSP_Id                 ProviderId         = null,
                        eMA_Id                  eMAId              = null,
                        TimeSpan?               QueryTimeout       = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId), "The given EVSE identification must not be null!");

            RemoteStartEVSEResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStart event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnRemoteEVSEStartLocal = OnRemoteEVSEStart;
                if (OnRemoteEVSEStartLocal != null)
                    OnRemoteEVSEStartLocal(Timestamp,
                                           this,
                                           EventTrackingId,
                                           RoamingNetwork.Id,
                                           EVSEId,
                                           ChargingProductId,
                                           ReservationId,
                                           SessionId,
                                           ProviderId,
                                           eMAId,
                                           QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnRemoteEVSEStart));
            }

            #endregion


            #region Try the remote EVSE operator...

            if (_RemoteEVSEOperator != null &&
               !_LocalEVSEIds.Contains(EVSEId))
            {

                result = await _RemoteEVSEOperator.RemoteStart(Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               EVSEId,
                                                               ChargingProductId,
                                                               ReservationId,
                                                               SessionId,
                                                               ProviderId,
                                                               eMAId,
                                                               QueryTimeout);


                if (result.Result == RemoteStartEVSEResultType.Success)
                {

                    result.Session.EVSEOperator = this;

                    var OnNewChargingSessionLocal = OnNewChargingSession;
                    if (OnNewChargingSessionLocal != null)
                        OnNewChargingSessionLocal(DateTime.Now, this, result.Session);

                }

            }

            #endregion

            #region ...else/or try local

            if (_RemoteEVSEOperator == null ||
                 result             == null ||
                (result             != null &&
                (result.Result      == RemoteStartEVSEResultType.UnknownEVSE ||
                 result.Result      == RemoteStartEVSEResultType.Error)))
            {

                var _ChargingPool = _ChargingPools.SelectMany(kvp  => kvp.Value.EVSEs).
                                                       Where (evse => evse.Id == EVSEId).
                                                       Select(evse => evse.ChargingStation.ChargingPool).
                                                       FirstOrDefault();

                if (_ChargingPool != null)
                {

                    result = await _ChargingPool.RemoteStart(Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             EVSEId,
                                                             ChargingProductId,
                                                             ReservationId,
                                                             SessionId,
                                                             ProviderId,
                                                             eMAId,
                                                             QueryTimeout);


                    if (result.Result == RemoteStartEVSEResultType.Success)
                    {
                        result.Session.EVSEOperator = this;
                        _ChargingSessions.TryAdd(result.Session.Id, _ChargingPool);
                    }

                }

                else
                    result = RemoteStartEVSEResult.UnknownEVSE;

            }

            #endregion


            #region Send OnRemoteEVSEStarted event

            Runtime.Stop();

            try
            {

                var OnRemoteEVSEStartedLocal = OnRemoteEVSEStarted;
                if (OnRemoteEVSEStartedLocal != null)
                    OnRemoteEVSEStartedLocal(Timestamp,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             EVSEId,
                                             ChargingProductId,
                                             ReservationId,
                                             SessionId,
                                             ProviderId,
                                             eMAId,
                                             QueryTimeout,
                                             result,
                                             Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnRemoteEVSEStarted));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStart(...ChargingStationId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStationId">The unique identification of the charging station to be started.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartChargingStationResult>

            RemoteStart(DateTime                Timestamp,
                        CancellationToken       CancellationToken,
                        EventTracking_Id        EventTrackingId,
                        ChargingStation_Id      ChargingStationId,
                        ChargingProduct_Id      ChargingProductId  = null,
                        ChargingReservation_Id  ReservationId      = null,
                        ChargingSession_Id      SessionId          = null,
                        EVSP_Id                 ProviderId         = null,
                        eMA_Id                  eMAId              = null,
                        TimeSpan?               QueryTimeout       = null)

        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            RemoteStartChargingStationResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteChargingStationStart event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnRemoteChargingStationStartLocal = OnRemoteChargingStationStart;
                if (OnRemoteChargingStationStartLocal != null)
                    OnRemoteChargingStationStartLocal(Timestamp,
                                                      this,
                                                      EventTrackingId,
                                                      RoamingNetwork.Id,
                                                      ChargingStationId,
                                                      ChargingProductId,
                                                      ReservationId,
                                                      SessionId,
                                                      ProviderId,
                                                      eMAId,
                                                      QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnRemoteChargingStationStart));
            }

            #endregion


            #region Try remote EVSE operator...

            if (_RemoteEVSEOperator != null)
            {

                result = await _RemoteEVSEOperator.RemoteStart(Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               ChargingStationId,
                                                               ChargingProductId,
                                                               ReservationId,
                                                               SessionId,
                                                               ProviderId,
                                                               eMAId,
                                                               QueryTimeout);


                if (result.Result == RemoteStartChargingStationResultType.Success)

                {

                    result.Session.EVSEOperator = this;

                    var OnNewChargingSessionLocal = OnNewChargingSession;
                    if (OnNewChargingSessionLocal != null)
                        OnNewChargingSessionLocal(DateTime.Now, this, result.Session);

                }

            }

            #endregion

            #region ...else/or try local

            if (_RemoteEVSEOperator == null ||
                (result             != null &&
                (result.Result      == RemoteStartChargingStationResultType.UnknownChargingStation ||
                 result.Result      == RemoteStartChargingStationResultType.Error)))
            {

                var _ChargingPool = _ChargingPools.SelectMany(kvp     => kvp.Value.ChargingStations).
                                                      Where  (station => station.Id == ChargingStationId).
                                                      Select (station => station.ChargingPool).
                                                      FirstOrDefault();

                if (_ChargingPool != null)
                {

                    result = await _ChargingPool.RemoteStart(Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             ChargingStationId,
                                                             ChargingProductId,
                                                             ReservationId,
                                                             SessionId,
                                                             ProviderId,
                                                             eMAId,
                                                             QueryTimeout);

                    if (result.Result == RemoteStartChargingStationResultType.Success)
                        _ChargingSessions.TryAdd(result.Session.Id, _ChargingPool);

                }

                else
                    result = RemoteStartChargingStationResult.UnknownChargingStation;

            }

            #endregion


            #region Send OnRemoteChargingStationStarted event

            Runtime.Stop();

            try
            {

                var OnRemoteChargingStationStartedLocal = OnRemoteChargingStationStarted;
                if (OnRemoteChargingStationStartedLocal != null)
                    OnRemoteChargingStationStartedLocal(Timestamp,
                                                        this,
                                                        EventTrackingId,
                                                        RoamingNetwork.Id,
                                                        ChargingStationId,
                                                        ChargingProductId,
                                                        ReservationId,
                                                        SessionId,
                                                        ProviderId,
                                                        eMAId,
                                                        QueryTimeout,
                                                        result,
                                                        Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnRemoteChargingStationStarted));
            }

            #endregion

            return result;

        }

        #endregion

        #region (internal) SendNewChargingSession(Timestamp, Sender, ChargingSession)

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  ChargingSession)
        {

            var OnNewChargingSessionLocal = OnNewChargingSession;
            if (OnNewChargingSessionLocal != null)
                OnNewChargingSessionLocal(Timestamp, Sender, ChargingSession);

        }

        #endregion


        #region OnRemote...Stop / OnRemote...Stopped / OnNewChargeDetailRecord

        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopDelegate                    OnRemoteStop;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStoppedDelegate                 OnRemoteStopped;

        /// <summary>
        /// An event fired whenever a remote stop EVSE command was received.
        /// </summary>
        public event OnRemoteEVSEStopDelegate                OnRemoteEVSEStop;

        /// <summary>
        /// An event fired whenever a remote stop EVSE command completed.
        /// </summary>
        public event OnRemoteEVSEStoppedDelegate             OnRemoteEVSEStopped;

        /// <summary>
        /// An event fired whenever a remote stop charging station command was received.
        /// </summary>
        public event OnRemoteChargingStationStopDelegate     OnRemoteChargingStationStop;

        /// <summary>
        /// An event fired whenever a remote stop charging station command completed.
        /// </summary>
        public event OnRemoteChargingStationStoppedDelegate  OnRemoteChargingStationStopped;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate         OnNewChargeDetailRecord;

        #endregion

        #region RemoteStop(...SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EventTracking_Id     EventTrackingId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId    = null,
                       eMA_Id               eMAId         = null,
                       TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId), "The given charging session identification must not be null!");

            RemoteStopResult result        = null;
            ChargingPool    _ChargingPool  = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnRemoteStopLocal = OnRemoteStop;
                if (OnRemoteStopLocal != null)
                    OnRemoteStopLocal(this,
                                      Timestamp,
                                      EventTrackingId,
                                      RoamingNetwork.Id,
                                      SessionId,
                                      ReservationHandling,
                                      ProviderId,
                                      eMAId,
                                      QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnRemoteStop));
            }

            #endregion


            #region Try remote EVSE operator...

            if (_RemoteEVSEOperator != null)
            {

                result = await _RemoteEVSEOperator.RemoteStop(Timestamp,
                                                              CancellationToken,
                                                              EventTrackingId,
                                                              SessionId,
                                                              ReservationHandling,
                                                              ProviderId,
                                                              eMAId,
                                                              QueryTimeout);


                if (result.Result == RemoteStopResultType.Success)
                {

                    // The CDR could also be sent separately!
                    if (result.ChargeDetailRecord != null)
                    {

                        var OnNewChargeDetailRecordLocal = OnNewChargeDetailRecord;
                        if (OnNewChargeDetailRecordLocal != null)
                            OnNewChargeDetailRecordLocal(DateTime.Now, this, result.ChargeDetailRecord);

                    }

                }


            }

            #endregion

            #region ...else/or try local

            if (_RemoteEVSEOperator == null ||
                (result             != null &&
                (result.Result      == RemoteStopResultType.InvalidSessionId ||
                 result.Result      == RemoteStopResultType.Error)))
            {

                if (_ChargingSessions.TryGetValue(SessionId, out _ChargingPool))
                {

                    result = await _ChargingPool.
                                       RemoteStop(Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,
                                                  QueryTimeout);

                }

                else
                    result = RemoteStopResult.InvalidSessionId(SessionId);

            }

            #endregion


            #region Send OnRemoteStopped event

            Runtime.Stop();

            try
            {

                var OnRemoteStoppedLocal = OnRemoteStopped;
                if (OnRemoteStoppedLocal != null)
                    OnRemoteStoppedLocal(this,
                                         Timestamp,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         SessionId,
                                         ReservationHandling,
                                         ProviderId,
                                         eMAId,
                                         QueryTimeout,
                                         result,
                                         Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnRemoteStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop(...EVSEId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopEVSEResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EventTracking_Id     EventTrackingId,
                       EVSE_Id              EVSEId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId    = null,
                       eMA_Id               eMAId         = null,
                       TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),     "The given EVSE identification must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given charging session identification must not be null!");

            RemoteStopEVSEResult result        = null;
            ChargingPool        _ChargingPool  = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnRemoteEVSEStopLocal = OnRemoteEVSEStop;
                if (OnRemoteEVSEStopLocal != null)
                    OnRemoteEVSEStopLocal(this,
                                          Timestamp,
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          EVSEId,
                                          SessionId,
                                          ReservationHandling,
                                          ProviderId,
                                          eMAId,
                                          QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnRemoteEVSEStop));
            }

            #endregion


            #region Try remote EVSE operator...

            if (_RemoteEVSEOperator != null &&
               !_LocalEVSEIds.Contains(EVSEId))
            {

                result = await _RemoteEVSEOperator.RemoteStop(Timestamp,
                                                              CancellationToken,
                                                              EventTrackingId,
                                                              EVSEId,
                                                              SessionId,
                                                              ReservationHandling,
                                                              ProviderId,
                                                              eMAId,
                                                              QueryTimeout);


                if (result.Result == RemoteStopEVSEResultType.Success)
                {

                    // The CDR could also be sent separately!
                    if (result.ChargeDetailRecord != null)
                    {

                        var OnNewChargeDetailRecordLocal = OnNewChargeDetailRecord;
                        if (OnNewChargeDetailRecordLocal != null)
                            OnNewChargeDetailRecordLocal(DateTime.Now, this, result.ChargeDetailRecord);

                    }

                }


            }

            #endregion

            #region ...else/or try local

            if (_RemoteEVSEOperator == null ||
                 result             == null ||
                (result             != null &&
                (result.Result      == RemoteStopEVSEResultType.UnknownEVSE ||
                 result.Result      == RemoteStopEVSEResultType.InvalidSessionId ||
                 result.Result      == RemoteStopEVSEResultType.Error)))
            {

                if (_ChargingSessions.TryGetValue(SessionId, out _ChargingPool))
                {

                    result = await _ChargingPool.
                                       RemoteStop(Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  EVSEId,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,
                                                  QueryTimeout);

                }

                else {

                    var __CP = ChargingPools.Where(cp => cp.ContainsEVSE(EVSEId)).FirstOrDefault();

                    if (__CP != null)
                      result = await __CP.RemoteStop(Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     EVSEId,
                                                     SessionId,
                                                     ReservationHandling,
                                                     ProviderId,
                                                     eMAId,
                                                     QueryTimeout);

                    else
                        result = RemoteStopEVSEResult.InvalidSessionId(SessionId);

                }

                //else
                  //  result = RemoteStopEVSEResult.InvalidSessionId(SessionId);

            }

            #endregion


            #region Send OnRemoteEVSEStopped event

            Runtime.Stop();

            try
            {

                var OnRemoteEVSEStoppedLocal = OnRemoteEVSEStopped;
                if (OnRemoteEVSEStoppedLocal != null)
                    OnRemoteEVSEStoppedLocal(this,
                                             Timestamp,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             EVSEId,
                                             SessionId,
                                             ReservationHandling,
                                             ProviderId,
                                             eMAId,
                                             QueryTimeout,
                                             result,
                                             Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnRemoteEVSEStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop(...ChargingStationId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStationId">The unique identification of the charging station to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopChargingStationResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EventTracking_Id     EventTrackingId,
                       ChargingStation_Id   ChargingStationId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId    = null,
                       eMA_Id               eMAId         = null,
                       TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),          "The given charging session identification must not be null!");

            RemoteStopChargingStationResult result        = null;
            ChargingPool                   _ChargingPool  = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteChargingStationStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                var OnRemoteChargingStationStopLocal = OnRemoteChargingStationStop;
                if (OnRemoteChargingStationStopLocal != null)
                    OnRemoteChargingStationStopLocal(this,
                                                     Timestamp,
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     ChargingStationId,
                                                     SessionId,
                                                     ReservationHandling,
                                                     ProviderId,
                                                     eMAId,
                                                     QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnRemoteChargingStationStop));
            }

            #endregion


            #region Try remote EVSE operator...

            if (_RemoteEVSEOperator != null)
            {

                result = await _RemoteEVSEOperator.RemoteStop(Timestamp,
                                                              CancellationToken,
                                                              EventTrackingId,
                                                              ChargingStationId,
                                                              SessionId,
                                                              ReservationHandling,
                                                              ProviderId,
                                                              eMAId,
                                                              QueryTimeout);


                if (result.Result == RemoteStopChargingStationResultType.Success)
                {

                    // The CDR could also be sent separately!
                    if (result.ChargeDetailRecord != null)
                    {

                        var OnNewChargeDetailRecordLocal = OnNewChargeDetailRecord;
                        if (OnNewChargeDetailRecordLocal != null)
                            OnNewChargeDetailRecordLocal(DateTime.Now, this, result.ChargeDetailRecord);

                    }

                }


            }

            #endregion

            #region ...else/or try local

            if (_RemoteEVSEOperator == null ||
                (result             != null &&
                (result.Result      == RemoteStopChargingStationResultType.UnknownChargingStation ||
                 result.Result      == RemoteStopChargingStationResultType.InvalidSessionId ||
                 result.Result      == RemoteStopChargingStationResultType.Error)))
            {

                if (_ChargingSessions.TryGetValue(SessionId, out _ChargingPool))
                {

                    result = await _ChargingPool.
                                       RemoteStop(Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  ChargingStationId,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,
                                                  QueryTimeout);

                }

                else
                    result = RemoteStopChargingStationResult.InvalidSessionId(SessionId);

            }

            #endregion


            #region Send OnRemoteChargingStationStopped event

            Runtime.Stop();

            try
            {

                var OnRemoteChargingStationStoppedLocal = OnRemoteChargingStationStopped;
                if (OnRemoteChargingStationStoppedLocal != null)
                    OnRemoteChargingStationStoppedLocal(this,
                                                        Timestamp,
                                                        EventTrackingId,
                                                        RoamingNetwork.Id,
                                                        ChargingStationId,
                                                        SessionId,
                                                        ReservationHandling,
                                                        ProviderId,
                                                        eMAId,
                                                        QueryTimeout,
                                                        result,
                                                        Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log("EVSEOperator." + nameof(OnRemoteChargingStationStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            var OnNewChargeDetailRecordLocal = OnNewChargeDetailRecord;
            if (OnNewChargeDetailRecordLocal != null)
                OnNewChargeDetailRecordLocal(Timestamp, Sender, ChargeDetailRecord);

        }

        #endregion

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

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return Id.ToString();
        }

        #endregion

    }

}
