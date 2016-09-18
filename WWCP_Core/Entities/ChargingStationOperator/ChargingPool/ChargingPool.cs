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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging pool
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingPool : AEMobilityEntity<ChargingPool_Id>,
                                IEquatable<ChargingPool>, IComparable<ChargingPool>, IComparable,
                                IEnumerable<ChargingStation>,
                                IStatus<ChargingPoolStatusType>
    {

        #region Data

        /// <summary>
        /// The default max size of the charging pool (aggregated charging station) status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 15;

        /// <summary>
        /// The default max size of the charging pool admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 15;

        #endregion

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of this charging pool.
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
        /// An optional (multi-language) description of this charging pool.
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

        #region BrandName

        private I18NString _BrandName;

        /// <summary>
        /// A (multi-language) brand name for this charging pool
        /// is this is different from the Charging Station Operator.
        /// </summary>
        [Mandatory]
        public I18NString BrandName
        {

            get
            {
                return _BrandName;
            }

            set
            {

                if (_BrandName != value)
                    SetProperty<I18NString>(ref _BrandName, value);

            }

        }

        #endregion

        #region LocationLanguage

        private Languages _LocationLanguage;

        /// <summary>
        /// The official language at this charging pool.
        /// </summary>
        [Optional]
        public Languages LocationLanguage
        {

            get
            {
                return _LocationLanguage;
            }

            set
            {

                if (value == null)
                    value = Languages.unknown;

                if (_LocationLanguage != value)
                {

                    SetProperty(ref _LocationLanguage, value);

                    // No downstream!
                    //_ChargingStations.Values.ForEach(station => station.LocationLanguage = null);

                }

            }

        }

        #endregion

        #region Address

        private Address _Address;

        /// <summary>
        /// The address of this charging pool.
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
                {

                    SetProperty(ref _Address, value);

                    _ChargingStations.ForEach(station => station._Address = null);

                }

            }

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate _GeoLocation;

        /// <summary>
        /// The geographical location of this charging pool.
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
                {

                    SetProperty(ref _GeoLocation, value);

                    _ChargingStations.ForEach(station => station._GeoLocation = null);

                }

            }

        }

        #endregion

        #region EntranceAddress

        private Address _EntranceAddress;

        /// <summary>
        /// The address of the entrance to this charging pool.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address EntranceAddress
        {

            get
            {
                return _EntranceAddress;
            }

            set
            {

                if (value == null)
                    value = new Address();

                if (_EntranceAddress != value)
                {

                    SetProperty(ref _EntranceAddress, value);

                    _ChargingStations.ForEach(station => station._EntranceAddress = null);

                }

            }

        }

        #endregion

        #region EntranceLocation

        private GeoCoordinate _EntranceLocation;

        /// <summary>
        /// The geographical location of the entrance to this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate EntranceLocation
        {

            get
            {
                return _EntranceLocation;
            }

            set
            {

                if (value == null)
                    value = new GeoCoordinate(new Latitude(0), new Longitude(0));

                if (_EntranceLocation != value)
                {

                    SetProperty(ref _EntranceLocation, value);

                    _ChargingStations.ForEach(station => station._EntranceLocation = null);

                }

            }

        }

        #endregion

        #region ExitAddress

        private Address _ExitAddress;

        /// <summary>
        /// The address of the exit of this charging pool.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address ExitAddress
        {

            get
            {
                return _ExitAddress;
            }

            set
            {

                if (value == null)
                    value = new Address();

                if (_ExitAddress != value)
                {

                    SetProperty(ref _ExitAddress, value);

                    _ChargingStations.ForEach(station => station._ExitAddress = null);

                }

            }

        }

        #endregion

        #region ExitLocation

        private GeoCoordinate _ExitLocation;

        /// <summary>
        /// The geographical location of the exit of this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate ExitLocation
        {

            get
            {
                return _ExitLocation;
            }

            set
            {

                if (value == null)
                    value = new GeoCoordinate(new Latitude(0), new Longitude(0));

                if (_ExitLocation != value)
                {

                    SetProperty(ref _ExitLocation, value);

                    _ChargingStations.ForEach(station => station._ExitLocation = null);

                }

            }

        }

        #endregion

        #region OpeningTimes

        private OpeningTimes _OpeningTimes;

        /// <summary>
        /// The opening times of this charging pool.
        /// </summary>
        [Optional]
        public OpeningTimes OpeningTimes
        {

            get
            {
                return _OpeningTimes;
            }

            set
            {

                if (value == null)
                    value = OpeningTimes.Open24Hours;

                if (_OpeningTimes != value)
                {

                    SetProperty(ref _OpeningTimes, value);

                    _ChargingStations.ForEach(station => station._OpeningTimes = null);

                }

            }

        }

        #endregion

        #region AuthenticationModes

        private ReactiveSet<AuthenticationModes> _AuthenticationModes;

        public ReactiveSet<AuthenticationModes> AuthenticationModes
        {

            get
            {
                return _AuthenticationModes;
            }

            set
            {

                if (value == null)
                    value = new ReactiveSet<AuthenticationModes>();

                if (_AuthenticationModes != value)
                {

                    SetProperty(ref _AuthenticationModes, value);

                    _ChargingStations.ForEach(station => station._AuthenticationModes = null);

                }

            }

        }

        #endregion

        #region PaymentOptions

        private ReactiveSet<PaymentOptions> _PaymentOptions;

        [Mandatory]
        public ReactiveSet<PaymentOptions> PaymentOptions
        {

            get
            {
                return _PaymentOptions;
            }

            set
            {

                if (value == null)
                    value = new ReactiveSet<PaymentOptions>();

                if (_PaymentOptions != value)
                {

                    SetProperty(ref _PaymentOptions, value);

                    _ChargingStations.ForEach(station => station._PaymentOptions = null);

                }

            }

        }

        #endregion

        #region Accessibility

        private AccessibilityTypes _Accessibility;

        [Optional]
        public AccessibilityTypes Accessibility
        {

            get
            {
                return _Accessibility;
            }

            set
            {

                if (_Accessibility != value)
                {

                    SetProperty(ref _Accessibility, value);

                    _ChargingStations.ForEach(station => station._Accessibility = value);

                }

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

                if (value == null)
                    value = "";

                if (_HotlinePhoneNumber != value)
                {

                    SetProperty(ref _HotlinePhoneNumber, value);

                    _ChargingStations.ForEach(station => station._HotlinePhoneNumber = null);

                }

            }

        }

        #endregion

        #region IsHubjectCompatible

        [Optional]
        public Partly IsHubjectCompatible

            => PartlyHelper.Generate(_ChargingStations.Select(station => station.IsHubjectCompatible));


        #endregion

        #region DynamicInfoAvailable

        [Optional]
        public Partly DynamicInfoAvailable

            => PartlyHelper.Generate(_ChargingStations.Select(station => station.DynamicInfoAvailable));

        #endregion


        #region PoolOwner

        private String _PoolOwner;

        /// <summary>
        /// The owner of this charging pool.
        /// </summary>
        [Optional]
        public String PoolOwner
        {

            get
            {
                return _PoolOwner;
            }

            set
            {
                SetProperty<String>(ref _PoolOwner, value);
            }

        }

        #endregion

        #region LocationOwner

        private String _LocationOwner;

        /// <summary>
        /// The owner of the charging pool location.
        /// </summary>
        [Optional]
        public String LocationOwner
        {

            get
            {
                return _LocationOwner;
            }

            set
            {
                SetProperty<String>(ref _LocationOwner, value);
            }

        }

        #endregion

        #region PhotoURIs

        private ReactiveSet<String> _PhotoURIs;

        /// <summary>
        /// URIs of photos of this charging pool.
        /// </summary>
        [Optional]
        public ReactiveSet<String> PhotoURIs
        {

            get
            {
                return _PhotoURIs;
            }

            set
            {
                SetProperty(ref _PhotoURIs, value);
            }

        }

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current admin status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingPoolAdminStatusType> AdminStatus

            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<ChargingPoolAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The admin status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<ChargingPoolAdminStatusType>> AdminStatusSchedule

            => _AdminStatusSchedule;

        #endregion


        #region Status

        /// <summary>
        /// The current status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingPoolStatusType> Status

            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<ChargingPoolStatusType> _StatusSchedule;

        /// <summary>
        /// The status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<ChargingPoolStatusType>> StatusSchedule

            => _StatusSchedule;

        #endregion

        #region StatusAggregationDelegate

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated charging stations.
        /// </summary>
        public Func<ChargingStationStatusReport, ChargingPoolStatusType> StatusAggregationDelegate { get; set; }

        #endregion

        #endregion

        #region Links

        /// <summary>
        /// The remote charging pool.
        /// </summary>
        [Optional]
        public IRemoteChargingPool      RemoteChargingPool  { get; }


        /// <summary>
        /// The Charging Station Operator of this charging pool.
        /// </summary>
        [Optional]
        public ChargingStationOperator  Operator            { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group/pool of charging stations having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing pool.</param>
        /// <param name="ChargingStationOperator">The parent charging station operator.</param>
        /// <param name="MaxPoolStatusListSize">The default size of the charging pool (aggregated charging station) status list.</param>
        /// <param name="MaxPoolAdminStatusListSize">The default size of the charging pool admin status list.</param>
        internal ChargingPool(ChargingPool_Id                    Id,
                              ChargingStationOperator            ChargingStationOperator,
                              Action<ChargingPool>               Configurator                = null,
                              RemoteChargingPoolCreatorDelegate  RemoteChargingPoolCreator   = null,
                              ChargingPoolAdminStatusType        AdminStatus                 = ChargingPoolAdminStatusType.Operational,
                              ChargingPoolStatusType             Status                      = ChargingPoolStatusType.Available,
                              UInt16                             MaxPoolAdminStatusListSize  = DefaultMaxAdminStatusListSize,
                              UInt16                             MaxPoolStatusListSize       = DefaultMaxStatusListSize)

            : base(Id)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator),  "The charging station operator must not be null!");

            #endregion

            #region Init data and properties

            this.Operator                    = ChargingStationOperator;

            this._ChargingStations           = new EntityHashSet<ChargingPool, ChargingStation_Id, ChargingStation>(this);

            this._LocationLanguage           = Languages.unknown;
            this._Name                       = new I18NString();
            this._BrandName                  = new I18NString();
            this._Description                = new I18NString();
            this._Address                    = new Address();
            this._EntranceAddress            = new Address();

            this._AuthenticationModes        = new ReactiveSet<AuthenticationModes>();

            this._AdminStatusSchedule        = new StatusSchedule<ChargingPoolAdminStatusType>(MaxPoolAdminStatusListSize);
            this._AdminStatusSchedule.Insert(AdminStatus);

            this._StatusSchedule             = new StatusSchedule<ChargingPoolStatusType>(MaxPoolStatusListSize);
            this._StatusSchedule.Insert(Status);

            this._ChargingReservations       = new ConcurrentDictionary<ChargingReservation_Id, ChargingStation>();
            this._ChargingSessions           = new ConcurrentDictionary<ChargingSession_Id,     ChargingStation>();

            #endregion

            #region Init events

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

            this._StatusSchedule.     OnStatusChanged += (Timestamp, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, OldStatus, NewStatus);

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, OldStatus, NewStatus);

            // ChargingPool events
            this.OnChargingStationAddition.OnVoting       += (timestamp, evseoperator, pool, vote) => ChargingStationOperator.ChargingStationAddition.SendVoting      (timestamp, evseoperator, pool, vote);
            this.OnChargingStationAddition.OnNotification += (timestamp, evseoperator, pool)       => ChargingStationOperator.ChargingStationAddition.SendNotification(timestamp, evseoperator, pool);

            this.OnChargingStationRemoval. OnVoting       += (timestamp, evseoperator, pool, vote) => ChargingStationOperator.ChargingStationRemoval. SendVoting      (timestamp, evseoperator, pool, vote);
            this.OnChargingStationRemoval. OnNotification += (timestamp, evseoperator, pool)       => ChargingStationOperator.ChargingStationRemoval. SendNotification(timestamp, evseoperator, pool);

            // ChargingStation events
            this.OnEVSEAddition.           OnVoting       += (timestamp, station, evse, vote)      => ChargingStationOperator.EVSEAddition.           SendVoting      (timestamp, station, evse, vote);
            this.OnEVSEAddition.           OnNotification += (timestamp, station, evse)            => ChargingStationOperator.EVSEAddition.           SendNotification(timestamp, station, evse);

            this.OnEVSERemoval.            OnVoting       += (timestamp, station, evse, vote)      => ChargingStationOperator.EVSERemoval .           SendVoting      (timestamp, station, evse, vote);
            this.OnEVSERemoval.            OnNotification += (timestamp, station, evse)            => ChargingStationOperator.EVSERemoval .           SendNotification(timestamp, station, evse);

            // EVSE events
            this.SocketOutletAddition.     OnVoting       += (timestamp, evse, outlet, vote)       => ChargingStationOperator.SocketOutletAddition.   SendVoting      (timestamp, evse, outlet, vote);
            this.SocketOutletAddition.     OnNotification += (timestamp, evse, outlet)             => ChargingStationOperator.SocketOutletAddition.   SendNotification(timestamp, evse, outlet);

            this.SocketOutletRemoval.      OnVoting       += (timestamp, evse, outlet, vote)       => ChargingStationOperator.SocketOutletRemoval.    SendVoting      (timestamp, evse, outlet, vote);
            this.SocketOutletRemoval.      OnNotification += (timestamp, evse, outlet)             => ChargingStationOperator.SocketOutletRemoval.    SendNotification(timestamp, evse, outlet);

            #endregion

            this.OnPropertyChanged += UpdateData;

            Configurator?.Invoke(this);

            this.RemoteChargingPool = RemoteChargingPoolCreator?.Invoke(this);

        }

        #endregion


        #region Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnChargingPoolDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status changed.
        /// </summary>
        public event OnChargingPoolStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(ChargingPoolAdminStatusType  NewAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<ChargingPoolAdminStatusType> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(ChargingPoolAdminStatusType  NewAdminStatus,
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
        public void SetAdminStatus(IEnumerable<Timestamped<ChargingPoolAdminStatusType>>  NewAdminStatusList,
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
        /// <param name="Sender">The changed charging pool.</param>
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
                await OnDataChangedLocal(Timestamp, Sender as ChargingPool, PropertyName, OldValue, NewValue);

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
                                         Timestamped<ChargingPoolStatusType>  OldStatus,
                                         Timestamped<ChargingPoolStatusType>  NewStatus)
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
                                              Timestamped<ChargingPoolAdminStatusType>  OldStatus,
                                              Timestamped<ChargingPoolAdminStatusType>  NewStatus)
        {

            var OnAdminStatusChangedLocal = OnAdminStatusChanged;
            if (OnAdminStatusChangedLocal != null)
                await OnAdminStatusChangedLocal(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region Charging stations

        #region ChargingStations

        private readonly EntityHashSet<ChargingPool, ChargingStation_Id, ChargingStation> _ChargingStations;

        /// <summary>
        /// Return all charging stations registered within this charing pool.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations

            => _ChargingStations;

        #endregion

        #region ChargingStationIds

        /// <summary>
        /// Return all charging station Ids registered within this charing pool.
        /// </summary>
        public IEnumerable<ChargingStation_Id> ChargingStationIds

            =>_ChargingStations.Ids;

        #endregion

        #region ChargingStationAdminStatus

        /// <summary>
        /// Return the admin status of all charging stations registered within this charging pool.
        /// </summary>

        public IEnumerable<KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusType>>>> ChargingStationAdminStatus

            => _ChargingStations.Select(station =>

                                     new KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusType>>>(
                                         station.Id,
                                         station.AdminStatusSchedule)

                                 );

        #endregion

        #region ChargingStationStatus

        /// <summary>
        /// Return the status of all charging stations registered within this charging pool.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusType>>>> ChargingStationStatus

            => _ChargingStations.Select(station =>

                                     new KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusType>>>(
                                         station.Id,
                                         station.StatusSchedule)

                                 );

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


        #region CreateNewStation(ChargingStationId = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station failed.</param>
        public ChargingStation CreateNewStation(ChargingStation_Id                        ChargingStationId             = null,
                                                Action<ChargingStation>                   Configurator                  = null,
                                                RemoteChargingStationCreatorDelegate      RemoteChargingStationCreator  = null,
                                                ChargingStationAdminStatusType            AdminStatus                   = ChargingStationAdminStatusType.Operational,
                                                ChargingStationStatusType                 Status                        = ChargingStationStatusType.Available,
                                                Action<ChargingStation>                   OnSuccess                     = null,
                                                Action<ChargingPool, ChargingStation_Id>  OnError                       = null)
        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId), "The given charging station identification must not be null!");

            if (_ChargingStations.Contains(ChargingStationId))
            {
                if (OnError == null)
                    throw new ChargingStationAlreadyExistsInPool(this, ChargingStationId);
                else
                    OnError?.Invoke(this, ChargingStationId);
            }

            #endregion

            var _ChargingStation = new ChargingStation(ChargingStationId,
                                                       this,
                                                       Configurator,
                                                       RemoteChargingStationCreator,
                                                       AdminStatus,
                                                       Status);


            if (ChargingStationAddition.SendVoting(DateTime.Now, this, _ChargingStation))
            {
                if (_ChargingStations.TryAdd(_ChargingStation))
                {

                    _ChargingStation.OnEVSEDataChanged         += UpdateEVSEData;
                    _ChargingStation.OnEVSEStatusChanged       += UpdateEVSEStatus;
                    _ChargingStation.OnEVSEAdminStatusChanged  += UpdateEVSEAdminStatus;

                    _ChargingStation.OnDataChanged             += UpdateChargingStationData;
                    _ChargingStation.OnStatusChanged           += UpdateChargingStationStatus;
                    _ChargingStation.OnAdminStatusChanged      += UpdateChargingStationAdminStatus;

                    _ChargingStation.OnNewReservation          += SendNewReservation;
                    _ChargingStation.OnReservationCancelled    += SendOnReservationCancelled;
                    _ChargingStation.OnNewChargingSession      += SendNewChargingSession;
                    _ChargingStation.OnNewChargeDetailRecord   += SendNewChargeDetailRecord;

                    OnSuccess?.Invoke(_ChargingStation);
                    ChargingStationAddition.SendNotification(DateTime.Now, this, _ChargingStation);


                    if (RemoteChargingStationCreator != null)
                    {

//                        _ChargingStation.RemoteChargingStation.OnNewReservation += SendNewReservation;

                        _ChargingStation.RemoteChargingStation.OnNewReservation += (a, b, reservation) => {

                            var __EVSE = GetEVSEbyId(reservation.EVSEId);

                            __EVSE.Reservation = reservation;

                        };

                        _ChargingStation.RemoteChargingStation.OnNewChargingSession += (a, b, session) => {

                            var __EVSE = GetEVSEbyId(session.EVSEId);

                            __EVSE.ChargingSession = session;

                        };

                        _ChargingStation.RemoteChargingStation.OnNewChargeDetailRecord += (a, b, cdr) => {

                            var __EVSE = GetEVSEbyId(cdr.EVSEId);

                            __EVSE.SendNewChargeDetailRecord(DateTime.Now, this, cdr);

                        };


                        _ChargingStation.RemoteChargingStation.OnReservationCancelled += _ChargingStation.SendOnReservationCancelled;
                        _ChargingStation.RemoteChargingStation.OnEVSEStatusChanged += (Timestamp, EVSE, OldStatus, NewStatus) => _ChargingStation.UpdateEVSEStatus(Timestamp, GetEVSEbyId(EVSE.Id), OldStatus, NewStatus);

                    }

                    return _ChargingStation;

                }
            }

            Debug.WriteLine("ChargingStation '" + ChargingStationId + "' could not be created!");

            if (OnError == null)
                throw new ChargingStationCouldNotBeCreated(this, ChargingStationId);

            OnError?.Invoke(this, ChargingStationId);
            return null;

        }

        #endregion


        #region ContainsChargingStation(ChargingStation)

        /// <summary>
        /// Check if the given ChargingStation is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStationId(ChargingStation ChargingStation)

            => _ChargingStations.Contains(ChargingStation.Id);

        #endregion

        #region ContainsChargingStation(ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)

            => _ChargingStations.Contains(ChargingStationId);

        #endregion

        #region GetChargingStationById(ChargingStationId)

        public ChargingStation GetChargingStationById(ChargingStation_Id ChargingStationId)

            => _ChargingStations.Get(ChargingStationId);

        #endregion

        #region TryGetChargingStationbyId(ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationbyId(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)

            => _ChargingStations.TryGet(ChargingStationId, out ChargingStation);

        #endregion

        #region RemoveChargingStation(ChargingStationId)

        public ChargingStation RemoveChargingStation(ChargingStation_Id ChargingStationId)
        {

            ChargingStation _ChargingStation = null;

            if (TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
            {

                if (ChargingStationRemoval.SendVoting(DateTime.Now, this, _ChargingStation))
                {

                    if (_ChargingStations.TryRemove(ChargingStationId, out _ChargingStation))
                    {

                        ChargingStationRemoval.SendNotification(DateTime.Now, this, _ChargingStation);

                        return _ChargingStation;

                    }

                }

            }

            return null;

        }

        #endregion

        #region TryRemoveChargingStation(ChargingStationId, out ChargingStation)

        public Boolean TryRemoveChargingStation(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {

            if (TryGetChargingStationbyId(ChargingStationId, out ChargingStation))
            {

                if (ChargingStationRemoval.SendVoting(DateTime.Now, this, ChargingStation))
                {

                    if (_ChargingStations.TryRemove(ChargingStationId, out ChargingStation))
                    {

                        ChargingStationRemoval.SendNotification(DateTime.Now, this, ChargingStation);

                        return true;

                    }

                }

                return false;

            }

            return true;

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


        #region (internal) UpdateChargingStationData(Timestamp, ChargingStation, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data of a charging station.
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
        /// Update the curent status of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old charging station status.</param>
        /// <param name="NewStatus">The new charging station status.</param>
        internal async Task UpdateChargingStationStatus(DateTime                                Timestamp,
                                                        ChargingStation                         ChargingStation,
                                                        Timestamped<ChargingStationStatusType>  OldStatus,
                                                        Timestamped<ChargingStationStatusType>  NewStatus)
        {

            var OnChargingStationStatusChangedLocal = OnChargingStationStatusChanged;
            if (OnChargingStationStatusChangedLocal != null)
                await OnChargingStationStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

            if (StatusAggregationDelegate != null)
            {
                _StatusSchedule.Insert(StatusAggregationDelegate(new ChargingStationStatusReport(_ChargingStations)),
                                      Timestamp);
            }

        }

        #endregion

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the curent admin status of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
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


        #region IEnumerable<ChargingStation> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => _ChargingStations.GetEnumerator();

        public IEnumerator<ChargingStation> GetEnumerator()
            => _ChargingStations.GetEnumerator();

        #endregion

        #endregion

        #region Charging station groups

        #endregion

        #region EVSEs

        #region EVSEs

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging pool.
        /// </summary>
        public IEnumerable<EVSE> EVSEs

            => _ChargingStations.
                       SelectMany(station => station.EVSEs);

        #endregion

        #region EVSEIds

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment
        /// (EVSEs) present within this charging pool.
        /// </summary>
        public IEnumerable<EVSE_Id> EVSEIds

            => _ChargingStations.
                       SelectMany(station => station.EVSEs).
                       Select    (evse    => evse.Id);

        #endregion

        #region EVSEAdminStatus

        /// <summary>
        /// Return the admin status of all EVSEs registered within this charging pool.
        /// </summary>

        public IEnumerable<KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusType>>>> EVSEAdminStatus

            => _ChargingStations.SelectMany(station => station.Select(evse =>

                                     new KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusType>>>(
                                         evse.Id,
                                         evse.AdminStatusSchedule)

                                 ));

        #endregion

        #region EVSEStatus

        /// <summary>
        /// Return the status of all EVSEs registered within this charging pool.
        /// </summary>
        public IEnumerable<KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEStatusType>>>> EVSEStatus

            => _ChargingStations.SelectMany(station => station.Select(evse =>

                                     new KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEStatusType>>>(
                                         evse.Id,
                                         evse.StatusSchedule)

                                 ));

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)

            => _ChargingStations.Any(ChargingStation => ChargingStation.EVSEIds.Contains(EVSE.Id));

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)

            => _ChargingStations.Any(ChargingStation => ChargingStation.EVSEIds.Contains(EVSEId));

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)

            => _ChargingStations.
                   SelectMany    (station => station.EVSEs).
                   FirstOrDefault(EVSE    => EVSE.Id == EVSEId);

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = _ChargingStations.
                       SelectMany    (station => station.EVSEs).
                       FirstOrDefault(_EVSE   => _EVSE.Id == EVSEId);

            return EVSE != null;

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

            => SocketOutletRemoval;

        #endregion


        #region (internal) UpdateEVSEData(Timestamp, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the static data of an EVSE.
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
        /// Update the current status of an EVSE.
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
        /// Update the current admin status of an EVSE.
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

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(30);

        #region ChargingReservations

        private readonly ConcurrentDictionary<ChargingReservation_Id, ChargingStation> _ChargingReservations;

        /// <summary>
        /// Return all current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations

            => _ChargingStations.
                   SelectMany(station => station.ChargingReservations);

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
                throw new ArgumentNullException(nameof(EVSEId),  "The given EVSE identification must not be null!");

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
                                      Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnReserveEVSE));
            }

            #endregion


            var _ChargingStation = EVSEs.Where (evse => evse.Id == EVSEId).
                                         Select(evse => evse.ChargingStation).
                                         FirstOrDefault();

            if (_ChargingStation != null)
            {

                result = await _ChargingStation.Reserve(EVSEId,
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
                    _ChargingReservations.TryAdd(result.Reservation.Id, _ChargingStation);

            }

            else
                result = ReservationResult.UnknownEVSE;


            #region Send OnEVSEReserved event

            Runtime.Stop();

            try
            {

                OnEVSEReserved?.Invoke(DateTime.Now,
                                       Timestamp.Value,
                                       this,
                                       EventTrackingId,
                                       Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnEVSEReserved));
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
                    DateTime?                StartTime,
                    TimeSpan?                Duration,
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
                                                 Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnReserveChargingStation));
            }

            #endregion


            var _ChargingStation = ChargingStations.
                                       FirstOrDefault(station => station.Id == ChargingStationId);

            if (_ChargingStation != null)
            {

                result = await _ChargingStation.Reserve(StartTime,
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
                    _ChargingReservations.TryAdd(result.Reservation.Id, _ChargingStation);

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
                                                  Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnChargingStationReserved));
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
                    DateTime?                StartTime,
                    TimeSpan?                Duration,
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
                                              Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnReserveChargingPool));
            }

            #endregion


            result = ReservationResult.Offline;


            #region Send OnChargingPoolReserved event

            Runtime.Stop();

            try
            {

                OnChargingPoolReserved?.Invoke(DateTime.Now,
                                               Timestamp.Value,
                                               this,
                                               EventTrackingId,
                                               Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnChargingPoolReserved));
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

            ChargingStation _ChargingStation = null;

            if (_ChargingReservations.TryGetValue(ReservationId, out _ChargingStation))
                return _ChargingStation.TryGetReservationById(ReservationId, out Reservation);

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

            CancelReservationResult result            = null;
            ChargingStation         _ChargingStation  = null;

            if (_ChargingReservations.TryRemove(ReservationId, out _ChargingStation))
                result = await _ChargingStation.CancelReservation(ReservationId,
                                                                  Reason,
                                                                  ProviderId,
                                                                  EVSEId,

                                                                  Timestamp,
                                                                  CancellationToken,
                                                                  EventTrackingId,
                                                                  RequestTimeout);

            else
            {

                foreach (var __ChargingStation in _ChargingStations)
                {

                    result = await __ChargingStation.CancelReservation(ReservationId,
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

        #region (internal) SendReservationCancelled(...)

        internal void SendOnReservationCancelled(DateTime                               LogTimestamp,
                                                 DateTime                               RequestTimestamp,
                                                 Object                                 Sender,
                                                 EventTracking_Id                       EventTrackingId,
                                                 ChargingReservation_Id                 ReservationId,
                                                 ChargingReservation                    Reservation,
                                                 ChargingReservationCancellationReason  Reason)
        {

            ChargingStation _ChargingStation = null;

            _ChargingReservations.TryRemove(ReservationId, out _ChargingStation);

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

        private readonly ConcurrentDictionary<ChargingSession_Id, ChargingStation> _ChargingSessions;

        /// <summary>
        /// Return all current charging sessions.
        /// </summary>
        public IEnumerable<ChargingSession> ChargingSessions

            => _ChargingStations.
                       SelectMany(station => station.ChargingSessions);

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
                Timestamp = DateTime.Now;

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
                                          Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteEVSEStart));
            }

            #endregion


            var _ChargingStation = _ChargingStations.SelectMany(station => station.EVSEs).
                                                     Where     (evse    => evse.Id == EVSEId).
                                                     Select    (evse    => evse.ChargingStation).
                                                     FirstOrDefault();

            if (_ChargingStation != null)
            {

                result = await _ChargingStation.RemoteStart(EVSEId,
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
                    _ChargingSessions.TryAdd(result.Session.Id, _ChargingStation);

            }

            else
                result = RemoteStartEVSEResult.UnknownEVSE;


            #region Send OnRemoteEVSEStarted event

            Runtime.Stop();

            try
            {

                OnRemoteEVSEStarted?.Invoke(DateTime.Now,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteEVSEStarted));
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
                                                     Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteChargingStationStart));
            }

            #endregion


            var _ChargingStation = _ChargingStations.FirstOrDefault(station => station.Id == ChargingStationId);

            if (_ChargingStation != null)
            {

                result = await _ChargingStation.RemoteStart(ChargingProductId,
                                                            ReservationId,
                                                            SessionId,
                                                            ProviderId,
                                                            eMAId,

                                                            Timestamp,
                                                            CancellationToken,
                                                            EventTrackingId,
                                                            RequestTimeout);

                if (result.Result == RemoteStartChargingStationResultType.Success)
                    _ChargingSessions.TryAdd(result.Session.Id, _ChargingStation);

            }

            else
                result = RemoteStartChargingStationResult.UnknownChargingStation;


            #region Send OnRemoteChargingStationStarted event

            Runtime.Stop();

            try
            {

                OnRemoteChargingStationStarted?.Invoke(DateTime.Now,
                                                       Timestamp.Value,
                                                       this,
                                                       EventTrackingId,
                                                       Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteChargingStationStarted));
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

            RemoteStopResult result           = null;
            ChargingStation _ChargingStation  = null;

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
                                     Operator.RoamingNetwork.Id,
                                     SessionId,
                                     ReservationHandling,
                                     ProviderId,
                                     eMAId,
                                     RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteStop));
            }

            #endregion


            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingStation))
            {

                result = await _ChargingStation.
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


            #region Send OnRemoteStopped event

            Runtime.Stop();

            try
            {

                OnRemoteStopped?.Invoke(DateTime.Now,
                                        Timestamp.Value,
                                        this,
                                        EventTrackingId,
                                        Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteStopped));
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

            RemoteStopEVSEResult result           = null;
            ChargingStation     _ChargingStation  = null;

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
                                         Operator.RoamingNetwork.Id,
                                         EVSEId,
                                         SessionId,
                                         ReservationHandling,
                                         ProviderId,
                                         eMAId,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteEVSEStop));
            }

            #endregion


            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingStation))
            {

                result = await _ChargingStation.
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

            //else
            //result = RemoteStopEVSEResult.InvalidSessionId(SessionId);

            else {

                var __ChargingStation = ChargingStations.FirstOrDefault(station => station.ContainsEVSE(EVSEId));

                if (__ChargingStation != null)
                    result = await __ChargingStation.RemoteStop(EVSEId,
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


            #region Send OnRemoteEVSEStopped event

            Runtime.Stop();

            try
            {

                OnRemoteEVSEStopped?.Invoke(DateTime.Now,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteEVSEStopped));
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

            RemoteStopChargingStationResult result           = null;
            ChargingStation                _ChargingStation  = null;

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
                                                    Operator.RoamingNetwork.Id,
                                                    ChargingStationId,
                                                    SessionId,
                                                    ReservationHandling,
                                                    ProviderId,
                                                    eMAId,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteChargingStationStop));
            }

            #endregion


            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingStation))
            {

                var result1 = await _ChargingStation.
                                        RemoteStop(SessionId,
                                                   ReservationHandling,
                                                   ProviderId,
                                                   eMAId,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);

                switch (result1.Result)
                {

                    case RemoteStopResultType.Error:
                        result = RemoteStopChargingStationResult.Error(result1.SessionId, result1.Message);
                        break;

                    case RemoteStopResultType.InvalidSessionId:
                        result = RemoteStopChargingStationResult.InvalidSessionId(result1.SessionId);
                        break;

                    case RemoteStopResultType.Offline:
                        result = RemoteStopChargingStationResult.Offline(result1.SessionId);
                        break;

                    case RemoteStopResultType.OutOfService:
                        result = RemoteStopChargingStationResult.OutOfService(result1.SessionId);
                        break;

                    case RemoteStopResultType.Success:
                        result = RemoteStopChargingStationResult.Success(result1.SessionId);
                        break;

                    case RemoteStopResultType.Timeout:
                        result = RemoteStopChargingStationResult.Timeout(result1.SessionId);
                        break;

                    case RemoteStopResultType.UnknownOperator:
                        result = RemoteStopChargingStationResult.UnknownOperator(result1.SessionId);
                        break;

                    case RemoteStopResultType.Unspecified:
                        result = RemoteStopChargingStationResult.Unspecified(result1.SessionId);
                        break;

                }

            }

            else
                result = RemoteStopChargingStationResult.InvalidSessionId(SessionId);


            #region Send OnRemoteChargingStationStopped event

            Runtime.Stop();

            try
            {

                OnRemoteChargingStationStopped?.Invoke(DateTime.Now,
                                                       Timestamp.Value,
                                                       this,
                                                       EventTrackingId,
                                                       Operator.RoamingNetwork.Id,
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
                e.Log(nameof(ChargingPool) + "." + nameof(OnRemoteChargingStationStopped));
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


        #region IComparable<ChargingPool> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charging pool.
            var ChargingPool = Object as ChargingPool;
            if ((Object) ChargingPool == null)
                throw new ArgumentException("The given object is not a charging pool!");

            return CompareTo(ChargingPool);

        }

        #endregion

        #region CompareTo(ChargingPool)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPool">A charging pool object to compare with.</param>
        public Int32 CompareTo(ChargingPool ChargingPool)
        {

            if ((Object) ChargingPool == null)
                throw new ArgumentNullException("The given charging pool must not be null!");

            return Id.CompareTo(ChargingPool.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPool> Members

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

            // Check if the given object is a charging pool.
            var ChargingPool = Object as ChargingPool;
            if ((Object) ChargingPool == null)
                return false;

            return this.Equals(ChargingPool);

        }

        #endregion

        #region Equals(ChargingPool)

        /// <summary>
        /// Compares two charging pools for equality.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPool ChargingPool)
        {

            if ((Object) ChargingPool == null)
                return false;

            return Id.Equals(ChargingPool.Id);

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
