/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;
using System.Collections;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The Charging Station Operator (CSO), also known as Charge Point Operator (CPO),
    /// is responsible for operating charging pools, charging stations and EVSEs.
    /// The Charging Station Operator is not neccessarily also the owner of all devices.
    /// For the delivered service (energy, parking, etc.) the Charging Station Operator
    /// will either be payed directly by the ev driver or by a contracted e-mobility service provider.
    /// The Charging Station Operator delivers the locations, characteristics and real-time
    /// status information of its charging pools/-stations and EVSEs as Linked
    /// Open Data (LOD) to e-mobility service providers, navigation service
    /// providers and the public. Pricing information can either be public
    /// information or part of business-to-business contracts.
    /// </summary>
    [DebuggerDisplay("{Id.ToString()} - {Name.FirstText}")]
    public class ChargingStationOperator : AEMobilityEntity<ChargingStationOperator_Id>,
                                           IEquatable<ChargingStationOperator>, IComparable<ChargingStationOperator>, IComparable,
                                           IEnumerable<ChargingPool>,
                                           IStatus<ChargingStationOperatorStatusType>
    {

        #region Data

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

                if (value == null)
                    value = new I18NString();

                if (_Name != value)
                    SetProperty(ref _Name, value);

            }

        }

        #endregion

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
                    value = new Address();

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
                    value = new GeoCoordinate(new Latitude(0), new Longitude(0));

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


        #region DataLicense

        private List<DataLicense> _DataLicenses;

        /// <summary>
        /// The license of the charging station operator data.
        /// </summary>
        [Mandatory]
        public IEnumerable<DataLicense> DataLicenses
            => _DataLicenses;

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current admin status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingStationOperatorAdminStatusType> AdminStatus

            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<ChargingStationOperatorAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The admin status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<ChargingStationOperatorAdminStatusType>> AdminStatusSchedule

            => _AdminStatusSchedule;

        #endregion


        #region Status

        /// <summary>
        /// The current status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingStationOperatorStatusType> Status

            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<ChargingStationOperatorStatusType> _StatusSchedule;

        /// <summary>
        /// The status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<ChargingStationOperatorStatusType>> StatusSchedule

            => _StatusSchedule;

        #endregion

        #endregion

        #region Links

        /// <summary>
        /// The remote charging station operator.
        /// </summary>
        [Optional]
        public IRemoteChargingStationOperator  RemoteChargingStationOperator    { get; }


        /// <summary>
        /// The parent roaming network.
        /// </summary>
        public RoamingNetwork                  RoamingNetwork                   { get; }

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
        internal ChargingStationOperator(ChargingStationOperator_Id                     Id,
                                         RoamingNetwork                                 RoamingNetwork,
                                         Action<ChargingStationOperator>                Configurator                          = null,
                                         RemoteChargingStationOperatorCreatorDelegate   RemoteChargingStationOperatorCreator  = null,
                                         I18NString                                     Name                                  = null,
                                         I18NString                                     Description                           = null,
                                         ChargingStationOperatorAdminStatusType         AdminStatus                           = ChargingStationOperatorAdminStatusType.Operational,
                                         ChargingStationOperatorStatusType              Status                                = ChargingStationOperatorStatusType.Available,
                                         UInt16                                         MaxAdminStatusListSize                = DefaultMaxAdminStatusListSize,
                                         UInt16                                         MaxStatusListSize                     = DefaultMaxStatusListSize)

            : base(Id)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The roaming network must not be null!");

            #endregion

            #region Init data and properties

            this.RoamingNetwork              = RoamingNetwork;

            this._Name                        = Name        ?? new I18NString();
            this._Description                 = Description ?? new I18NString();
            this._DataLicenses                = new List<DataLicense>();

            #region InvalidEVSEIds

            this.InvalidEVSEIds            = new ReactiveSet<EVSE_Id>();

            InvalidEVSEIds.OnItemAdded += (Timestamp, Set, EVSEId) =>
            {
                var OnInvalidEVSEIdAddedLocal = OnInvalidEVSEIdAdded;
                if (OnInvalidEVSEIdAddedLocal != null)
                    OnInvalidEVSEIdAddedLocal(Timestamp, this, EVSEId);
            };

            InvalidEVSEIds.OnItemRemoved += (Timestamp, Set, EVSEId) =>
            {
                var OnInvalidEVSEIdRemovedLocal = OnInvalidEVSEIdRemoved;
                if (OnInvalidEVSEIdRemovedLocal != null)
                    OnInvalidEVSEIdRemovedLocal(Timestamp, this, EVSEId);
            };

            #endregion

            this._ChargingPools               = new EntityHashSet<ChargingStationOperator, ChargingPool_Id,         ChargingPool>(this);
            this._ChargingStationGroups       = new EntityHashSet<ChargingStationOperator, ChargingStationGroup_Id, ChargingStationGroup>(this);

            this._AdminStatusSchedule         = new StatusSchedule<ChargingStationOperatorAdminStatusType>();
            this._AdminStatusSchedule.Insert(AdminStatus);

            this._StatusSchedule              = new StatusSchedule<ChargingStationOperatorStatusType>();
            this._StatusSchedule.Insert(Status);

            this._ChargingReservations        = new ConcurrentDictionary<ChargingReservation_Id, ChargingPool>();
            this._ChargingSessions            = new ConcurrentDictionary<ChargingSession_Id,     ChargingPool>();

            #endregion

            #region Init events

            // Charging Station Operator events
            this.ChargingPoolAddition          = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingPool,         Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval           = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingPool,         Boolean>(() => new VetoVote(), true);

            // Charging station group events
            this.ChargingStationGroupAddition  = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingStationGroup, Boolean>(() => new VetoVote(), true);
            this.ChargingStationGroupRemoval   = new VotingNotificator<DateTime, ChargingStationOperator,    ChargingStationGroup, Boolean>(() => new VetoVote(), true);

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

            // ChargingStationOperator events
            this.OnChargingPoolAddition.   OnVoting       += (timestamp, ChargingStationOperator, pool, vote) => RoamingNetwork.ChargingPoolAddition.   SendVoting      (timestamp, ChargingStationOperator, pool, vote);
            this.OnChargingPoolAddition.   OnNotification += (timestamp, ChargingStationOperator, pool)       => RoamingNetwork.ChargingPoolAddition.   SendNotification(timestamp, ChargingStationOperator, pool);

            this.OnChargingPoolRemoval.    OnVoting       += (timestamp, ChargingStationOperator, pool, vote) => RoamingNetwork.ChargingPoolRemoval.    SendVoting      (timestamp, ChargingStationOperator, pool, vote);
            this.OnChargingPoolRemoval.    OnNotification += (timestamp, ChargingStationOperator, pool)       => RoamingNetwork.ChargingPoolRemoval.    SendNotification(timestamp, ChargingStationOperator, pool);

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

            LocalEVSEIds = new ReactiveSet<EVSE_Id>();

            Configurator?.Invoke(this);

            this.RemoteChargingStationOperator = RemoteChargingStationOperatorCreator?.Invoke(this);

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
        public void SetAdminStatus(ChargingStationOperatorAdminStatusType  NewAdminStatus)
        {

            _AdminStatusSchedule.Insert(NewAdminStatus);

        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<ChargingStationOperatorAdminStatusType> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(ChargingStationOperatorAdminStatusType  NewAdminStatus,
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
        public void SetAdminStatus(IEnumerable<Timestamped<ChargingStationOperatorAdminStatusType>>  NewAdminStatusList,
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
                                         Timestamped<ChargingStationOperatorStatusType>  OldStatus,
                                         Timestamped<ChargingStationOperatorStatusType>  NewStatus)
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
                                              Timestamped<ChargingStationOperatorAdminStatusType>  OldStatus,
                                              Timestamped<ChargingStationOperatorAdminStatusType>  NewStatus)
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

            if (DataLicenses.Length > 0)
            {
                foreach (var license in DataLicenses.Where(license => license != null))
                    _DataLicenses.Add(license);
            }

            return this;

        }

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

        #region ChargingPoolRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingPool, Boolean> ChargingPoolRemoval;

        /// <summary>
        /// Called whenever an charging pool will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingPool, Boolean> OnChargingPoolRemoval

            => ChargingPoolRemoval;

        #endregion


        #region ChargingPools

        private EntityHashSet<ChargingStationOperator, ChargingPool_Id, ChargingPool> _ChargingPools;

        public IEnumerable<ChargingPool> ChargingPools

            => _ChargingPools;

        #endregion

        #region ChargingPoolIds

        public IEnumerable<ChargingPool_Id> ChargingPoolIds

            => _ChargingPools.Ids;

        #endregion

        #region ChargingPoolAdminStatus(IncludePool = null)

        public IEnumerable<KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>> ChargingPoolAdminStatus(Func<ChargingPool, Boolean> IncludePool = null)

            => _ChargingPools.
                   Where  (pool => IncludePool == null || IncludePool(pool)).
                   OrderBy(pool => pool.Id).
                   Select (pool => new KeyValuePair<ChargingPool_Id, ChargingPoolAdminStatusType>(pool.Id, pool.AdminStatus.Value));

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
        public ChargingPool CreateNewChargingPool(ChargingPool_Id                                   ChargingPoolId             = null,
                                                  Action<ChargingPool>                              Configurator               = null,
                                                  RemoteChargingPoolCreatorDelegate                 RemoteChargingPoolCreator  = null,
                                                  ChargingPoolAdminStatusType                       AdminStatus                = ChargingPoolAdminStatusType.Operational,
                                                  ChargingPoolStatusType                            Status                     = ChargingPoolStatusType.Available,
                                                  Action<ChargingPool>                              OnSuccess                  = null,
                                                  Action<ChargingStationOperator, ChargingPool_Id>  OnError                    = null)

        {

            #region Initial checks

            if (ChargingPoolId == null)
                ChargingPoolId = ChargingPool_Id.Random(this.Id);

            // Do not throw an exception when an OnError delegate was given!
            if (_ChargingPools.Any(pool => pool.Id == ChargingPoolId))
            {
                if (OnError == null)
                    throw new ChargingPoolAlreadyExists(this, ChargingPoolId);
                else
                    OnError?.Invoke(this, ChargingPoolId);
            }

            #endregion

            var _ChargingPool = new ChargingPool(ChargingPoolId,
                                                 this,
                                                 Configurator,
                                                 RemoteChargingPoolCreator,
                                                 AdminStatus,
                                                 Status);


            if (ChargingPoolAddition.SendVoting(DateTime.Now, this, _ChargingPool))
            {
                if (_ChargingPools.TryAdd(_ChargingPool))
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


                    OnSuccess?.Invoke(_ChargingPool);
                    ChargingPoolAddition.SendNotification(DateTime.Now, this, _ChargingPool);

                    return _ChargingPool;

                }
            }

            return null;

        }

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

            => _ChargingPools.Contains(ChargingPoolId);

        #endregion

        #region GetChargingPoolbyId(ChargingPoolId)

        public ChargingPool GetChargingPoolbyId(ChargingPool_Id ChargingPoolId)

            => _ChargingPools.Get(ChargingPoolId);

        #endregion

        #region TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool)

        public Boolean TryGetChargingPoolbyId(ChargingPool_Id ChargingPoolId, out ChargingPool ChargingPool)

            => _ChargingPools.TryGet(ChargingPoolId, out ChargingPool);

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

        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationAddition

            => ChargingStationAddition;

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationRemoval

            => ChargingStationRemoval;

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

            //if (StatusAggregationDelegate != null)
            //    _StatusSchedule.Insert(StatusAggregationDelegate(new ChargingPoolStatusReport(_ChargingPools.Values)),
            //                           Timestamp);

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

        IEnumerator IEnumerable.GetEnumerator()

            => _ChargingPools.GetEnumerator();

        public IEnumerator<ChargingPool> GetEnumerator()

            => _ChargingPools.GetEnumerator();

        #endregion

        #endregion

        #region Charging stations

        #region ChargingStations

        public IEnumerable<ChargingStation> ChargingStations

            => _ChargingPools.
                   SelectMany(pool => pool.ChargingStations);

        #endregion

        #region ChargingStationIds

        public IEnumerable<ChargingStation_Id> ChargingStationIds

            => _ChargingPools.
                   SelectMany(pool    => pool.   ChargingStations).
                   Select    (station => station.Id);

        #endregion

        #region ChargingStationAdminStatus(IncludeStation = null)

        public IEnumerable<KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>> ChargingStationAdminStatus(Func<ChargingStation, Boolean> IncludeStation = null)

            => _ChargingPools.
                   SelectMany(pool    => pool.ChargingStations).
                   Where     (station => IncludeStation == null || IncludeStation(station)).
                   OrderBy   (station => station.Id).
                   Select    (station => new KeyValuePair<ChargingStation_Id, ChargingStationAdminStatusType>(station.Id, station.AdminStatus.Value));

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

        #region GetChargingStationbyId(ChargingStationId)

        public ChargingStation GetChargingStationbyId(ChargingStation_Id ChargingStationId)

            => _ChargingPools.
                   SelectMany    (pool    => pool.ChargingStations).
                   FirstOrDefault(station => station.Id == ChargingStationId);

        #endregion

        #region TryGetChargingStationbyId(ChargingStationId, out ChargingStation ChargingStation)

        public Boolean TryGetChargingStationbyId(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {

            ChargingStation = _ChargingPools.
                                  SelectMany    (pool    => pool.ChargingStations).
                                  FirstOrDefault(station => station.Id == ChargingStationId);

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

        #region EVSEAddition

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSEAddition

            => EVSEAddition;

        #endregion

        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSERemoval

            => EVSERemoval;

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
        /// Called whenever an charging station group will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> OnChargingStationGroupRemoval

            => ChargingStationGroupRemoval;

        #endregion


        #region ChargingStationGroups

        private readonly EntityHashSet<ChargingStationOperator, ChargingStationGroup_Id, ChargingStationGroup> _ChargingStationGroups;

        /// <summary>
        /// All charging station groups registered within this Charging Station Operator.
        /// </summary>
        public IEnumerable<ChargingStationGroup> ChargingStationGroups

            => _ChargingStationGroups;

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
        public ChargingStationGroup CreateNewChargingStationGroup(ChargingStationGroup_Id                                   ChargingStationGroupId  = null,
                                                                  Action<ChargingStationGroup>                              Configurator            = null,
                                                                  Action<ChargingStationGroup>                              OnSuccess               = null,
                                                                  Action<ChargingStationOperator, ChargingStationGroup_Id>  OnError                 = null)
        {

            #region Initial checks

            if (ChargingStationGroupId == null)
                ChargingStationGroupId = ChargingStationGroup_Id.Random(this.Id);

            // Do not throw an exception when an OnError delegate was given!
            if (_ChargingStationGroups.Contains(ChargingStationGroupId))
            {
                if (OnError == null)
                    throw new ChargingStationGroupAlreadyExists(this, ChargingStationGroupId);
                else
                    OnError?.Invoke(this, ChargingStationGroupId);
            }

            #endregion

            var _ChargingStationGroup = new ChargingStationGroup(ChargingStationGroupId, this);

            if (Configurator != null)
                Configurator(_ChargingStationGroup);

            if (ChargingStationGroupAddition.SendVoting(DateTime.Now, this, _ChargingStationGroup))
            {
                if (_ChargingStationGroups.TryAdd(_ChargingStationGroup))
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
                                                                    Action<ChargingStationOperator, ChargingStationGroup_Id>  OnError                 = null)
        {

            ChargingStationGroup _ChargingStationGroup = null;

            if (_ChargingStationGroups.TryGet(ChargingStationGroupId, out _ChargingStationGroup))
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

            => _ChargingStationGroups.TryGet(ChargingStationGroupId, out ChargingStationGroup);

        #endregion

        #endregion

        #region EVSEs

        #region EVSEs

        public IEnumerable<EVSE> EVSEs

            => _ChargingPools.
                   SelectMany(v => v.ChargingStations).
                   SelectMany(v => v.EVSEs);

        #endregion

        #region EVSEIds

        public IEnumerable<EVSE_Id> EVSEIds

            => _ChargingPools.
                   SelectMany(v => v.ChargingStations).
                   SelectMany(v => v.EVSEs).
                   Select    (v => v.Id);

        #endregion

        #region AllEVSEStatus(IncludeEVSE = null)

        public IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>> AllEVSEStatus(Func<EVSE, Boolean>  IncludeEVSE = null)

            => _ChargingPools.
                   SelectMany(pool    => pool.ChargingStations).
                   SelectMany(station => station.EVSEs).
                   Where     (evse    => IncludeEVSE == null || IncludeEVSE(evse)).
                   OrderBy   (evse    => evse.Id).
                   Select    (evse    => new KeyValuePair<EVSE_Id, EVSEStatusType>(evse.Id, evse.Status.Value));

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

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)

            => _ChargingPools.
                   SelectMany    (pool    => pool.   ChargingStations).
                   SelectMany    (station => station.EVSEs).
                   FirstOrDefault(evse    => evse.Id == EVSEId);

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = _ChargingPools.
                       SelectMany    (pool    => pool.   ChargingStations).
                       SelectMany    (station => station.EVSEs).
                       FirstOrDefault(evse    => evse.Id == EVSEId);

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

        /// <summary>
        /// A list of invalid EVSE Ids.
        /// </summary>
        public ReactiveSet<EVSE_Id> InvalidEVSEIds { get; }

        #endregion

        #region LocalEVSEIds

        /// <summary>
        /// A list of manual EVSE Ids which will not be touched automagically.
        /// </summary>
        public ReactiveSet<EVSE_Id> LocalEVSEIds { get; }

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

        #region SocketOutletAddition

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletAddition

            => SocketOutletAddition;

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

            => _ChargingPools.
                       SelectMany(pool => pool.ChargingReservations);

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
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(EVSE_Id                  EVSEId,
                    DateTime?                StartTime          = null,
                    TimeSpan?                Duration           = null,
                    ChargingReservation_Id   ReservationId      = null,
                    eMobilityProvider_Id     ProviderId         = null,
                    eMobilityAccount_Id                   eMAId              = null,
                    ChargingProduct_Id       ChargingProductId  = null,
                    IEnumerable<Auth_Token>  AuthTokens         = null,
                    IEnumerable<eMobilityAccount_Id>      eMAIds             = null,
                    IEnumerable<UInt32>      PINs               = null,

                    DateTime?                Timestamp          = null,
                    CancellationToken?       CancellationToken  = null,
                    EventTracking_Id         EventTrackingId    = null,
                    TimeSpan?                RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId), "The given EVSE identification must not be null!");

            ReservationResult result = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveEVSE event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnReserveEVSE?.Invoke(DateTime.Now,
                                      Timestamp.Value,
                                      this,
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
                                      RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnReserveEVSE));
            }

            #endregion


            #region Try the remote Charging Station Operator...

            if (RemoteChargingStationOperator != null &&
               !LocalEVSEIds.Contains(EVSEId))
            {

                result = await RemoteChargingStationOperator.Reserve(EVSEId,
                                                                      StartTime,
                                                                      Duration,
                                                                      ReservationId,
                                                                      ProviderId,
                                                                      eMAId,
                                                                      ChargingProductId,
                                                                      AuthTokens,
                                                                      eMAIds,
                                                                      PINs,

                                                                      Timestamp,
                                                                      CancellationToken,
                                                                      EventTrackingId,
                                                                      RequestTimeout);


                if (result.Result == ReservationResultType.Success)
                {

                    //result.Reservation.ChargingStationOperator = this;

                    var OnNewReservationLocal = OnNewReservation;
                    if (OnNewReservationLocal != null)
                        OnNewReservationLocal(DateTime.Now, this, result.Reservation);

                }

            }

            #endregion

            #region ...else/or try local

            if (RemoteChargingStationOperator == null ||
                 result             == null ||
                (result             != null &&
                (result.Result      == ReservationResultType.UnknownEVSE ||
                 result.Result      == ReservationResultType.Error)))
            {

                var _ChargingPool = EVSEs.Where (evse => evse.Id == EVSEId).
                                          Select(evse => evse.ChargingStation.ChargingPool).
                                          FirstOrDefault();

                if (_ChargingPool != null)
                {

                    result = await _ChargingPool.Reserve(EVSEId,
                                                         StartTime,
                                                         Duration,
                                                         ReservationId,
                                                         ProviderId,
                                                         eMAId,
                                                         ChargingProductId,
                                                         AuthTokens,
                                                         eMAIds,
                                                         PINs,

                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout);

                    if (result.Result == ReservationResultType.Success)
                        _ChargingReservations.TryAdd(result.Reservation.Id, _ChargingPool);

                }

                else
                    result = ReservationResult.UnknownEVSE;

            }

            #endregion


            #region Send OnEVSEReserved event

            Runtime.Stop();

            try
            {

                OnEVSEReserved?.Invoke(DateTime.Now,
                                       Timestamp.Value,
                                       this,
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
                                       RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnEVSEReserved));
            }

            #endregion

            return result;

        }

        #endregion

        #region Reserve(...ChargingStationId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging station.
        /// </summary>
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
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(ChargingStation_Id       ChargingStationId,
                    DateTime?                StartTime          = null,
                    TimeSpan?                Duration           = null,
                    ChargingReservation_Id   ReservationId      = null,
                    eMobilityProvider_Id     ProviderId         = null,
                    eMobilityAccount_Id                   eMAId              = null,
                    ChargingProduct_Id       ChargingProductId  = null,
                    IEnumerable<Auth_Token>  AuthTokens         = null,
                    IEnumerable<eMobilityAccount_Id>      eMAIds             = null,
                    IEnumerable<UInt32>      PINs               = null,

                    DateTime?                Timestamp          = null,
                    CancellationToken?       CancellationToken  = null,
                    EventTracking_Id         EventTrackingId    = null,
                    TimeSpan?                RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            ReservationResult result = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveChargingStation event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnReserveChargingStation?.Invoke(DateTime.Now,
                                                 Timestamp.Value,
                                                 this,
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
                                                 RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnReserveChargingStation));
            }

            #endregion


            var _ChargingPool  = ChargingStations.
                                     Where (station => station.Id == ChargingStationId).
                                     Select(station => station.ChargingPool).
                                     FirstOrDefault();

            if (_ChargingPool != null)
            {

                result = await _ChargingPool.Reserve(ChargingStationId,
                                                     StartTime,
                                                     Duration,
                                                     ReservationId,
                                                     ProviderId,
                                                     eMAId,
                                                     ChargingProductId,
                                                     AuthTokens,
                                                     eMAIds,
                                                     PINs,

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout);

                if (result.Result == ReservationResultType.Success)
                    _ChargingReservations.TryAdd(result.Reservation.Id, _ChargingPool);

            }

            else
                result = ReservationResult.UnknownChargingStation;


            #region Send OnChargingStationReserved event

            Runtime.Stop();

            try
            {

                OnChargingStationReserved?.Invoke(DateTime.Now,
                                                  Timestamp.Value,
                                                  this,
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
                                                  RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnChargingStationReserved));
            }

            #endregion

            return result;

        }

        #endregion

        #region Reserve(...ChargingPoolId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge within the given charging pool.
        /// </summary>
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
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(ChargingPool_Id          ChargingPoolId,
                    DateTime?                StartTime          = null,
                    TimeSpan?                Duration           = null,
                    ChargingReservation_Id   ReservationId      = null,
                    eMobilityProvider_Id     ProviderId         = null,
                    eMobilityAccount_Id                   eMAId              = null,
                    ChargingProduct_Id       ChargingProductId  = null,
                    IEnumerable<Auth_Token>  AuthTokens         = null,
                    IEnumerable<eMobilityAccount_Id>      eMAIds             = null,
                    IEnumerable<UInt32>      PINs               = null,

                    DateTime?                Timestamp          = null,
                    CancellationToken?       CancellationToken  = null,
                    EventTracking_Id         EventTrackingId    = null,
                    TimeSpan?                RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingPoolId == null)
                throw new ArgumentNullException(nameof(ChargingPoolId),  "The given charging pool identification must not be null!");

            ReservationResult result = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveChargingPool event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnReserveChargingPool?.Invoke(DateTime.Now,
                                              Timestamp.Value,
                                              this,
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
                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnReserveChargingPool));
            }

            #endregion


            var _ChargingPool  = ChargingPools.
                                     FirstOrDefault(pool => pool.Id == ChargingPoolId);

            if (_ChargingPool != null)
            {

                result = await _ChargingPool.Reserve(ChargingPoolId,
                                                     StartTime,
                                                     Duration,
                                                     ReservationId,
                                                     ProviderId,
                                                     eMAId,
                                                     ChargingProductId,
                                                     AuthTokens,
                                                     eMAIds,
                                                     PINs,

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout);

                if (result.Result == ReservationResultType.Success)
                    _ChargingReservations.TryAdd(result.Reservation.Id, _ChargingPool);

            }

            else
                result = ReservationResult.UnknownChargingStation;


            #region Send OnChargingPoolReserved event

            Runtime.Stop();

            try
            {

                OnChargingPoolReserved?.Invoke(DateTime.Now,
                                               Timestamp.Value,
                                               this,
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
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnChargingPoolReserved));
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


        #region CancelReservation(...ReservationId, Reason, ProviderId = null, EVSEId = null, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="EVSEId">An optional identification of the EVSE.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              eMobilityProvider_Id                   ProviderId         = null,
                              EVSE_Id                                EVSEId             = null,

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            CancelReservationResult result         = null;
            ChargingPool            _ChargingPool  = null;

            if (_ChargingReservations.TryRemove(ReservationId, out _ChargingPool))
                result = await _ChargingPool.CancelReservation(ReservationId,
                                                               Reason,
                                                               ProviderId,
                                                               EVSEId,

                                                               Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               RequestTimeout);

            else
            {

                foreach (var __ChargingPool in _ChargingPools)
                {

                    result = await __ChargingPool.CancelReservation(ReservationId,
                                                                    Reason,
                                                                    ProviderId,
                                                                    EVSEId,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

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

        private void SendOnReservationCancelled(DateTime                               LogTimestamp,
                                                DateTime                               RequestTimestamp,
                                                Object                                 Sender,
                                                EventTracking_Id                       EventTrackingId,
                                                ChargingReservation_Id                 ReservationId,
                                                ChargingReservation                    Reservation,
                                                ChargingReservationCancellationReason  Reason)
        {

            ChargingPool _ChargingPool = null;

            _ChargingReservations.TryRemove(ReservationId, out _ChargingPool);

            OnReservationCancelled?.Invoke(LogTimestamp,
                                           RequestTimestamp,
                                           Sender,
                                           EventTrackingId,
                                           ReservationId,
                                           Reservation,
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

            => _ChargingPools.
                   SelectMany(pool => pool.ChargingSessions);

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
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartEVSEResult>

            RemoteStart(EVSE_Id                 EVSEId,
                        ChargingProduct_Id      ChargingProductId  = null,
                        ChargingReservation_Id  ReservationId      = null,
                        ChargingSession_Id      SessionId          = null,
                        eMobilityProvider_Id    ProviderId         = null,
                        eMobilityAccount_Id                  eMAId              = null,

                        DateTime?               Timestamp          = null,
                        CancellationToken?      CancellationToken  = null,
                        EventTracking_Id        EventTrackingId    = null,
                        TimeSpan?               RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId), "The given EVSE identification must not be null!");

            RemoteStartEVSEResult result = null;

            if (!Timestamp.HasValue)
                Timestamp       = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStart event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteEVSEStart?.Invoke(DateTime.Now,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          EVSEId,
                                          ChargingProductId,
                                          ReservationId,
                                          SessionId,
                                          ProviderId,
                                          eMAId,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnRemoteEVSEStart));
            }

            #endregion


            #region Try the remote Charging Station Operator...

            if (RemoteChargingStationOperator != null &&
               !LocalEVSEIds.Contains(EVSEId))
            {

                result = await RemoteChargingStationOperator.RemoteStart(EVSEId,
                                                                          ChargingProductId,
                                                                          ReservationId,
                                                                          SessionId,
                                                                          ProviderId,
                                                                          eMAId,

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout);


                if (result.Result == RemoteStartEVSEResultType.Success)
                {

                    result.Session.ChargingStationOperator = this;

                    OnNewChargingSession?.Invoke(DateTime.Now, this, result.Session);

                }

            }

            #endregion

            #region ...else/or try local

            if (RemoteChargingStationOperator == null ||
                 result             == null ||
                (result             != null &&
                (result.Result      == RemoteStartEVSEResultType.UnknownEVSE ||
                 result.Result      == RemoteStartEVSEResultType.Error)))
            {

                var _ChargingPool = _ChargingPools.SelectMany(pool => pool.EVSEs).
                                                       Where (evse => evse.Id == EVSEId).
                                                       Select(evse => evse.ChargingStation.ChargingPool).
                                                       FirstOrDefault();

                if (_ChargingPool != null)
                {

                    result = await _ChargingPool.RemoteStart(EVSEId,
                                                             ChargingProductId,
                                                             ReservationId,
                                                             SessionId,
                                                             ProviderId,
                                                             eMAId,

                                                             Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             RequestTimeout);


                    if (result.Result == RemoteStartEVSEResultType.Success)
                    {
                        result.Session.ChargingStationOperator = this;
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

                OnRemoteEVSEStarted?.Invoke(DateTime.Now,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            EVSEId,
                                            ChargingProductId,
                                            ReservationId,
                                            SessionId,
                                            ProviderId,
                                            eMAId,
                                            RequestTimeout,
                                            result,
                                            Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnRemoteEVSEStarted));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStart(...ChargingStationId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station to be started.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartChargingStationResult>

            RemoteStart(ChargingStation_Id      ChargingStationId,
                        ChargingProduct_Id      ChargingProductId  = null,
                        ChargingReservation_Id  ReservationId      = null,
                        ChargingSession_Id      SessionId          = null,
                        eMobilityProvider_Id    ProviderId         = null,
                        eMobilityAccount_Id                  eMAId              = null,

                        DateTime?               Timestamp          = null,
                        CancellationToken?      CancellationToken  = null,
                        EventTracking_Id        EventTrackingId    = null,
                        TimeSpan?               RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            RemoteStartChargingStationResult result = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteChargingStationStart event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteChargingStationStart?.Invoke(DateTime.Now,
                                                     Timestamp.Value,
                                                     this,
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     ChargingStationId,
                                                     ChargingProductId,
                                                     ReservationId,
                                                     SessionId,
                                                     ProviderId,
                                                     eMAId,
                                                     RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnRemoteChargingStationStart));
            }

            #endregion


            #region Try remote Charging Station Operator...

            if (RemoteChargingStationOperator != null)
            {

                result = await RemoteChargingStationOperator.RemoteStart(ChargingStationId,
                                                                          ChargingProductId,
                                                                          ReservationId,
                                                                          SessionId,
                                                                          ProviderId,
                                                                          eMAId,

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout);


                if (result.Result == RemoteStartChargingStationResultType.Success)

                {

                    result.Session.ChargingStationOperator = this;

                    OnNewChargingSession?.Invoke(DateTime.Now, this, result.Session);

                }

            }

            #endregion

            #region ...else/or try local

            if (RemoteChargingStationOperator == null ||
                (result             != null &&
                (result.Result      == RemoteStartChargingStationResultType.UnknownChargingStation ||
                 result.Result      == RemoteStartChargingStationResultType.Error)))
            {

                var _ChargingPool = _ChargingPools.SelectMany(pool    => pool.ChargingStations).
                                                      Where  (station => station.Id == ChargingStationId).
                                                      Select (station => station.ChargingPool).
                                                      FirstOrDefault();

                if (_ChargingPool != null)
                {

                    result = await _ChargingPool.RemoteStart(ChargingStationId,
                                                             ChargingProductId,
                                                             ReservationId,
                                                             SessionId,
                                                             ProviderId,
                                                             eMAId,

                                                             Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             RequestTimeout);

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

                OnRemoteChargingStationStarted?.Invoke(DateTime.Now,
                                                       Timestamp.Value,
                                                       this,
                                                       EventTrackingId,
                                                       RoamingNetwork.Id,
                                                       ChargingStationId,
                                                       ChargingProductId,
                                                       ReservationId,
                                                       SessionId,
                                                       ProviderId,
                                                       eMAId,
                                                       RequestTimeout,
                                                       result,
                                                       Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnRemoteChargingStationStarted));
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

            OnNewChargingSession?.Invoke(Timestamp, Sender, ChargingSession);

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
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id    SessionId,
                       ReservationHandling   ReservationHandling,
                       eMobilityProvider_Id  ProviderId         = null,
                       eMobilityAccount_Id                eMAId              = null,

                       DateTime?             Timestamp          = null,
                       CancellationToken?    CancellationToken  = null,
                       EventTracking_Id      EventTrackingId    = null,
                       TimeSpan?             RequestTimeout     = null)

        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId), "The given charging session identification must not be null!");

            RemoteStopResult result        = null;
            ChargingPool    _ChargingPool  = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteStop?.Invoke(DateTime.Now,
                                     Timestamp.Value,
                                     this,
                                     EventTrackingId,
                                     RoamingNetwork.Id,
                                     SessionId,
                                     ReservationHandling,
                                     ProviderId,
                                     eMAId,
                                     RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnRemoteStop));
            }

            #endregion


            #region Try remote Charging Station Operator...

            if (RemoteChargingStationOperator != null)
            {

                result = await RemoteChargingStationOperator.
                                   RemoteStop(SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);


                if (result.Result == RemoteStopResultType.Success)
                {

                    // The CDR could also be sent separately!
                    if (result.ChargeDetailRecord != null)
                    {

                        OnNewChargeDetailRecord?.Invoke(DateTime.Now, this, result.ChargeDetailRecord);

                    }

                }


            }

            #endregion

            #region ...else/or try local

            if (RemoteChargingStationOperator == null ||
                (result             != null &&
                (result.Result      == RemoteStopResultType.InvalidSessionId ||
                 result.Result      == RemoteStopResultType.Error)))
            {

                if (_ChargingSessions.TryGetValue(SessionId, out _ChargingPool))
                {

                    result = await _ChargingPool.
                                       RemoteStop(SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout);

                }

                else
                    result = RemoteStopResult.InvalidSessionId(SessionId);

            }

            #endregion


            #region Send OnRemoteStopped event

            Runtime.Stop();

            try
            {

                OnRemoteStopped?.Invoke(DateTime.Now,
                                        Timestamp.Value,
                                        this,
                                        EventTrackingId,
                                        RoamingNetwork.Id,
                                        SessionId,
                                        ReservationHandling,
                                        ProviderId,
                                        eMAId,
                                        RequestTimeout,
                                        result,
                                        Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnRemoteStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop(...EVSEId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopEVSEResult>

            RemoteStop(EVSE_Id               EVSEId,
                       ChargingSession_Id    SessionId,
                       ReservationHandling   ReservationHandling,
                       eMobilityProvider_Id  ProviderId         = null,
                       eMobilityAccount_Id                eMAId              = null,

                       DateTime?             Timestamp          = null,
                       CancellationToken?    CancellationToken  = null,
                       EventTracking_Id      EventTrackingId    = null,
                       TimeSpan?             RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),     "The given EVSE identification must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given charging session identification must not be null!");

            RemoteStopEVSEResult result        = null;
            ChargingPool        _ChargingPool  = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteEVSEStop?.Invoke(DateTime.Now,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         EVSEId,
                                         SessionId,
                                         ReservationHandling,
                                         ProviderId,
                                         eMAId,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnRemoteEVSEStop));
            }

            #endregion


            #region Try remote Charging Station Operator...

            if (RemoteChargingStationOperator != null &&
               !LocalEVSEIds.Contains(EVSEId))
            {

                result = await RemoteChargingStationOperator.
                                   RemoteStop(EVSEId,
                                              SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);


                if (result.Result == RemoteStopEVSEResultType.Success)
                {

                    // The CDR could also be sent separately!
                    if (result.ChargeDetailRecord != null)
                    {

                        OnNewChargeDetailRecord?.Invoke(DateTime.Now,
                                                        this,
                                                        result.ChargeDetailRecord);

                    }

                }


            }

            #endregion

            #region ...else/or try local

            if (RemoteChargingStationOperator == null ||
                 result             == null ||
                (result             != null &&
                (result.Result      == RemoteStopEVSEResultType.UnknownEVSE ||
                 result.Result      == RemoteStopEVSEResultType.InvalidSessionId ||
                 result.Result      == RemoteStopEVSEResultType.Error)))
            {

                if (_ChargingSessions.TryGetValue(SessionId, out _ChargingPool))
                {

                    result = await _ChargingPool.
                                       RemoteStop(EVSEId,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout);

                }

                else {

                    var __CP = ChargingPools.FirstOrDefault(cp => cp.ContainsEVSE(EVSEId));

                    if (__CP != null)
                      result = await __CP.RemoteStop(EVSEId,
                                                     SessionId,
                                                     ReservationHandling,
                                                     ProviderId,
                                                     eMAId,

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout);

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

                OnRemoteEVSEStopped?.Invoke(DateTime.Now,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            EVSEId,
                                            SessionId,
                                            ReservationHandling,
                                            ProviderId,
                                            eMAId,
                                            RequestTimeout,
                                            result,
                                            Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnRemoteEVSEStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop(...ChargingStationId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopChargingStationResult>

            RemoteStop(ChargingStation_Id    ChargingStationId,
                       ChargingSession_Id    SessionId,
                       ReservationHandling   ReservationHandling,
                       eMobilityProvider_Id  ProviderId         = null,
                       eMobilityAccount_Id                eMAId              = null,

                       DateTime?             Timestamp          = null,
                       CancellationToken?    CancellationToken  = null,
                       EventTracking_Id      EventTrackingId    = null,
                       TimeSpan?             RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),          "The given charging session identification must not be null!");

            RemoteStopChargingStationResult result        = null;
            ChargingPool                   _ChargingPool  = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteChargingStationStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteChargingStationStop?.Invoke(DateTime.Now,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ChargingStationId,
                                                    SessionId,
                                                    ReservationHandling,
                                                    ProviderId,
                                                    eMAId,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnRemoteChargingStationStop));
            }

            #endregion


            #region Try remote Charging Station Operator...

            if (RemoteChargingStationOperator != null)
            {

                result = await RemoteChargingStationOperator.
                                   RemoteStop(ChargingStationId,
                                              SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);


                if (result.Result == RemoteStopChargingStationResultType.Success)
                {

                    // The CDR could also be sent separately!
                    if (result.ChargeDetailRecord != null)
                    {

                        OnNewChargeDetailRecord?.Invoke(DateTime.Now,
                                                        this,
                                                        result.ChargeDetailRecord);

                    }

                }


            }

            #endregion

            #region ...else/or try local

            if (RemoteChargingStationOperator == null ||
                (result             != null &&
                (result.Result      == RemoteStopChargingStationResultType.UnknownChargingStation ||
                 result.Result      == RemoteStopChargingStationResultType.InvalidSessionId ||
                 result.Result      == RemoteStopChargingStationResultType.Error)))
            {

                if (_ChargingSessions.TryGetValue(SessionId, out _ChargingPool))
                {

                    result = await _ChargingPool.
                                       RemoteStop(ChargingStationId,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout);

                }

                else
                    result = RemoteStopChargingStationResult.InvalidSessionId(SessionId);

            }

            #endregion


            #region Send OnRemoteChargingStationStopped event

            Runtime.Stop();

            try
            {

                OnRemoteChargingStationStopped?.Invoke(DateTime.Now,
                                                       Timestamp.Value,
                                                       this,
                                                       EventTrackingId,
                                                       RoamingNetwork.Id,
                                                       ChargingStationId,
                                                       SessionId,
                                                       ReservationHandling,
                                                       ProviderId,
                                                       eMAId,
                                                       RequestTimeout,
                                                       result,
                                                       Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingStationOperator) + "." + nameof(OnRemoteChargingStationStopped));
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

            OnNewChargeDetailRecord?.Invoke(Timestamp, Sender, ChargeDetailRecord);

        }

        #endregion

        #endregion


        #region IComparable<ChargingStationOperator> Members

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion

    }

}
