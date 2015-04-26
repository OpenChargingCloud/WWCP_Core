/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3_Core>
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

#endregion

namespace org.GraphDefined.eMI3
{

    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the EVSPool
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingPool : AEntity<ChargingPool_Id>,
                                IEquatable<ChargingPool>, IComparable<ChargingPool>, IComparable,
                                IEnumerable<ChargingStation>
    {

        #region Data

        /// <summary>
        /// The default max size of the aggregated EVSE status history.
        /// </summary>
        public const UInt16 DefaultStatusHistorySize = 50;

        #endregion

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the EVSPool.
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
                SetProperty<I18NString>(ref _Name, value);
            }

        }

        #endregion

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional additional (multi-language) description of the EVSPool.
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
                SetProperty<I18NString>(ref _Description, value);
            }

        }

        #endregion

        #region LocationLanguage

        private Languages _LocationLanguage;

        /// <summary>
        /// The official language at this EVSPool.
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
                SetProperty<Languages>(ref _LocationLanguage, value);
            }

        }

        #endregion

        #region PoolLocation

        private GeoCoordinate _PoolLocation;

        /// <summary>
        /// The geographical location of this EVSPool.
        /// </summary>
        [Optional]
        public GeoCoordinate PoolLocation
        {

            get
            {
                return _PoolLocation;
            }

            set
            {
                SetProperty<GeoCoordinate>(ref _PoolLocation, value);
            }

        }

        #endregion

        #region EntranceLocation

        private GeoCoordinate _EntranceLocation;

        /// <summary>
        /// The geographical location of the entrance of this EVSPool.
        /// (If different from 'PoolLocation').
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
                SetProperty<GeoCoordinate>(ref _EntranceLocation, value);
            }

        }

        #endregion

        #region Address

        private Address _Address;

        /// <summary>
        /// The address of this EVSPool.
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
                SetProperty<Address>(ref _Address, value);
            }

        }

        #endregion

        #region EntranceAddress

        private Address _EntranceAddress;

        /// <summary>
        /// The address of the entrance of this EVSPool.
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
                SetProperty<Address>(ref _EntranceAddress, value);
            }

        }

        #endregion

        #region PoolOwner

        private String _PoolOwner;

        /// <summary>
        /// The owner of this EVSPool.
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
        /// The owner of the EVSPool location.
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

        private readonly List<String> _PhotoURIs;

        /// <summary>
        /// URIs of photos of this charging pool.
        /// </summary>
        [Optional, Not_eMI3defined]
        public List<String> PhotoURIs
        {
            get
            {
                return _PhotoURIs;
            }
        }

        #endregion


        #region AuthenticationModes

        internal IEnumerable<String> DefaultAuthenticationModes;

        public IEnumerable<String> AuthenticationModes
        {

            get
            {

                if (_ChargingStations.Count() == 0)
                    return DefaultAuthenticationModes;

                var _AuthenticationModes = _ChargingStations.First().Value.AuthenticationModes.ToArray();

                foreach (var station in _ChargingStations.Values.Skip(1))
                {

                    if (station.AuthenticationModes.Count() != _AuthenticationModes.Length)
                        return new String[0];

                    foreach (var AuthenticationMode in station.AuthenticationModes)
                        if (!_AuthenticationModes.Contains(AuthenticationMode))
                            return new String[0];

                }

                return _AuthenticationModes;

            }

            set
            {

                var Value = value;

                if (value == null)
                    Value = new String[0];

                DefaultAuthenticationModes = Value;

                _ChargingStations.Values.ForEach(station => station.AuthenticationModes = Value);

            }

        }

        #endregion

        public IEnumerable<String>  PaymentOptions          { get; set; }

        public String               Accessibility           { get; set; }

        #region OpeningTime

        private OpeningTime _OpeningTime;

        /// <summary>
        /// The opening time of this EVSPool.
        /// </summary>
        [Optional]
        public OpeningTime OpeningTime
        {

            get
            {
                return _OpeningTime;
            }

            set
            {
                SetProperty<OpeningTime>(ref _OpeningTime, value);
            }

        }

        #endregion

        public String               HotlinePhoneNum         { get; set; }

        public I18NString           AdditionalInfo          { get; set; }

        public Boolean?             DynamicInfoAvailable    { get; set; }


        #region Status

        /// <summary>
        /// The current charging pool status.
        /// </summary>
        [Optional, Not_eMI3defined]
        public Timestamped<AggregatedStatusType> Status
        {
            get
            {
                return _StatusHistory.Peek();
            }
        }

        #endregion

        #region StatusHistory

        private Stack<Timestamped<AggregatedStatusType>> _StatusHistory;

        /// <summary>
        /// The charging pool status history.
        /// </summary>
        [Optional, Not_eMI3defined]
        public IEnumerable<Timestamped<AggregatedStatusType>> StatusHistory
        {
            get
            {
                return _StatusHistory.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        #region StatusAggregationDelegate

        private Func<EVSEStatusReport, AggregatedStatusType> _StatusAggregationDelegate;

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated EVSEs.
        /// </summary>
        public Func<EVSEStatusReport, AggregatedStatusType> StatusAggregationDelegate
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


        #region EVSEOperator

        private EVSEOperator _EVSEOperator;

        /// <summary>
        /// The EVSE operator of this EVSPool.
        /// </summary>
        [Optional]
        public EVSEOperator EVSEOperator
        {

            get
            {
                return _EVSEOperator;
            }

            set
            {
                SetProperty<EVSEOperator>(ref _EVSEOperator, value);
            }

        }

        #endregion

        #region ChargingStations

        private readonly ConcurrentDictionary<ChargingStation_Id, ChargingStation> _ChargingStations;

        /// <summary>
        /// Return all charging stations registered within this EVS pool.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations
        {
            get
            {
                return _ChargingStations.Values;
            }
        }

        #endregion

        #region EVSEs

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging pool.
        /// </summary>
        public IEnumerable<EVSE> EVSEs
        {
            get
            {

                return _ChargingStations.
                           Values.
                           SelectMany(station => station.EVSEs);

            }
        }

        #endregion

        #region EVSEIds

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment
        /// (EVSEs) present within this charging pool.
        /// </summary>
        public IEnumerable<EVSE_Id> EVSEIds
        {
            get
            {

                return _ChargingStations.
                           Values.
                           SelectMany(station => station.EVSEs).
                           Select    (evse    => evse.Id);

            }
        }

        #endregion

        #endregion

        #region Events

        #region ChargingStationAddition

        private readonly IVotingNotificator<ChargingPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<ChargingPool, ChargingStation, Boolean> OnChargingStationAddition
        {
            get
            {
                return ChargingStationAddition;
            }
        }

        #endregion


        // Charging station events

        #region EVSEAddition

        internal readonly IVotingNotificator<ChargingStation, EVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<ChargingStation, EVSE, Boolean> OnEVSEAddition
        {
            get
            {
                return EVSEAddition;
            }
        }

        #endregion


        // EVSE events

        #region SocketOutletAddition

        internal readonly IVotingNotificator<EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<EVSE, SocketOutlet, Boolean> OnSocketOutletAddition
        {
            get
            {
                return SocketOutletAddition;
            }
        }

        #endregion


        #region OnAggregatedStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="OldChargingPoolStatus">The old timestamped status of the charging pool.</param>
        /// <param name="NewChargingPoolStatus">The new timestamped status of the charging pool.</param>
        public delegate void OnAggregatedStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<AggregatedStatusType> OldChargingPoolStatus, Timestamped<AggregatedStatusType> NewChargingPoolStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        public event OnAggregatedStatusChangedDelegate OnAggregatedStatusChanged;

        #endregion

        #endregion

        #region Constructor(s)

        #region (internal) ChargingPool()

        /// <summary>
        /// Create a new charging pool having a random identification.
        /// </summary>
        internal ChargingPool()
            : this(ChargingPool_Id.New)
        { }

        #endregion

        #region (internal) ChargingPool(EVSEOperator)

        /// <summary>
        /// Create a new charging pool having a random identification
        /// and the given EVSE operator.
        /// </summary>
        /// <param name="EVSEOperator">The parent EVSE operator.</param>
        internal ChargingPool(EVSEOperator EVSEOperator)
            : this(ChargingPool_Id.New, EVSEOperator)
        { }

        #endregion

        #region (internal) ChargingPool(ChargingPool_Id, StatusHistorySize = DefaultStatusHistorySize)

        /// <summary>
        /// Create a new group/pool of charging stations having a random identification.
        /// </summary>
        /// <param name="Id">The iunique identification of the charging pool.</param>
        /// <param name="StatusHistorySize">The default size of the aggregated EVSE status history.</param>
        internal ChargingPool(ChargingPool_Id  Id,
                              UInt16           StatusHistorySize = DefaultStatusHistorySize)

            : base(Id)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the EVS pool must not be null!");

            #endregion

            #region Init data and properties

            this._ChargingStations           = new ConcurrentDictionary<ChargingStation_Id, ChargingStation>();

            this.LocationLanguage            = Languages.undef;
            this.Name                        = new I18NString();
            this.Description                 = new I18NString();
            this.Address                     = new Address();
            this.EntranceAddress             = new Address();

            this.DefaultAuthenticationModes  = new String[0];

            this._StatusHistory              = new Stack<Timestamped<AggregatedStatusType>>((Int32) StatusHistorySize);
            this._StatusHistory.Push(new Timestamped<AggregatedStatusType>(AggregatedStatusType.Unknown));

            #endregion

            #region Init and link events

            // EVSE events
            this.SocketOutletAddition       = new VotingNotificator<EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            if (EVSEOperator != null)
            {
                this.SocketOutletAddition.OnVoting            += (evse, socketoutlet , vote) => EVSEOperator.SocketOutletAddition.SendVoting      (evse, socketoutlet, vote);
                this.SocketOutletAddition.OnNotification      += (evse, socketoutlet)        => EVSEOperator.SocketOutletAddition.SendNotification(evse, socketoutlet);
            }

            #endregion

        }

        #endregion

        #region (internal) ChargingPool(Id, EVSEOperator, StatusHistorySize = DefaultStatusHistorySize)  // Main Constructor

        /// <summary>
        /// Create a new group/pool of charging stations having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the EVS pool.</param>
        /// <param name="EVSEOperator">The parent EVSE operator.</param>
        /// <param name="StatusHistorySize">The default size of the aggregated EVSE status history.</param>
        internal ChargingPool(ChargingPool_Id  Id,
                              EVSEOperator     EVSEOperator,
                              UInt16           StatusHistorySize  = DefaultStatusHistorySize)

            : this(Id, StatusHistorySize)

        {


            // EVS pool events
            this.ChargingStationAddition    = new VotingNotificator<ChargingPool, ChargingStation, Boolean>(() => new VetoVote(), true);

            if (EVSEOperator != null)
            {
                this.OnChargingStationAddition.OnVoting       += (evseoperator, evspool, vote) => EVSEOperator.ChargingStationAddition.SendVoting      (evseoperator, evspool, vote);
                this.OnChargingStationAddition.OnNotification += (evseoperator, evspool)       => EVSEOperator.ChargingStationAddition.SendNotification(evseoperator, evspool);
            }


            // Charging station events
            this.EVSEAddition               = new VotingNotificator<ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            if (EVSEOperator != null)
            {
                this.OnEVSEAddition.OnVoting                  += (chargingstation, evse, vote) => EVSEOperator.EVSEAddition.SendVoting      (chargingstation, evse, vote);
                this.OnEVSEAddition.OnNotification            += (chargingstation, evse)       => EVSEOperator.EVSEAddition.SendNotification(chargingstation, evse);
            }

        }

        #endregion

        #endregion


        #region CreateNew()

        /// <summary>
        /// Create a new charging pool having a random identification.
        /// </summary>
        public static ChargingPool CreateNew()
        {
            return new ChargingPool();
        }

        #endregion

        #region CreateNew(Id)

        /// <summary>
        /// Create a new charging pool having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        public static ChargingPool CreateNew(ChargingPool_Id Id)
        {
            return new ChargingPool(Id);
        }

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
        public ChargingStation CreateNewStation(ChargingStation_Id                        ChargingStationId  = null,
                                                Action<ChargingStation>                   Configurator       = null,
                                                Action<ChargingStation>                   OnSuccess          = null,
                                                Action<ChargingPool, ChargingStation_Id>  OnError            = null)
        {

            #region Initial checks

            if (ChargingStationId == null)
                ChargingStationId = ChargingStation_Id.New;

            if (_ChargingStations.ContainsKey(ChargingStationId))
            {
                if (OnError == null)
                    throw new ChargingStationAlreadyExistsInPool(ChargingStationId, this.Id);
                else
                    OnError.FailSafeInvoke(this, ChargingStationId);
            }

            #endregion

            var _ChargingStation = new ChargingStation(ChargingStationId, this);

            Configurator.FailSafeInvoke(_ChargingStation);

            if (ChargingStationAddition.SendVoting(this, _ChargingStation))
            {
                if (_ChargingStations.TryAdd(ChargingStationId, _ChargingStation))
                {

                    // Subscribe to charging station status changes for aggregated status creation!
                    _ChargingStation.OnAggregatedStatusChanged += (Timestamp, ChargingStation, OldChargingStationStatus, NewChargingStationStatus) => UpdateStatus(Timestamp);

                    OnSuccess.FailSafeInvoke(_ChargingStation);
                    ChargingStationAddition.SendNotification(this, _ChargingStation);
                    return _ChargingStation;

                }
            }

            Debug.WriteLine("ChargingStation '" + ChargingStationId + "' was not created!");
            return null;

        }

        #endregion


        #region (internal) UpdateStatus(Timestamp)

        /// <summary>
        /// Update the current charging pool status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        internal void UpdateStatus(DateTime Timestamp)
        {

            if (StatusAggregationDelegate != null)
            {

                var NewStatus = new Timestamped<AggregatedStatusType>(StatusAggregationDelegate(new EVSEStatusReport(EVSEs)));

                if (NewStatus.Value != _StatusHistory.Peek().Value)
                {

                    var OldStatus = _StatusHistory.Peek();

                    _StatusHistory.Push(NewStatus);

                    var OnAggregatedStatusChangedLocal = OnAggregatedStatusChanged;
                    if (OnAggregatedStatusChangedLocal != null)
                        OnAggregatedStatusChangedLocal(Timestamp, this, OldStatus, NewStatus);

                }

            }

        }

        #endregion


        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)
        {
            return _ChargingStations.Values.
                       SelectMany(v => v.EVSEs).
                       Where     (EVSE => EVSE.Id == EVSEId).
                       FirstOrDefault();
        }

        #endregion

        #region TryGetEVSEbyId(EVSEId)

        public Boolean GetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = _ChargingStations.Values.
                       SelectMany(v     => v.EVSEs).
                       Where     (_EVSE => _EVSE.Id == EVSEId).
                       FirstOrDefault();

            return EVSE != null;

        }

        #endregion


        #region IEnumerable<ChargingStation> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _ChargingStations.Values.GetEnumerator();
        }

        public IEnumerator<ChargingStation> GetEnumerator()
        {
            return _ChargingStations.Values.GetEnumerator();
        }

        #endregion

        #region IComparable<EVSPool> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSPool.
            var EVSPool = Object as ChargingPool;
            if ((Object) EVSPool == null)
                throw new ArgumentException("The given object is not an EVSPool!");

            return CompareTo(EVSPool);

        }

        #endregion

        #region CompareTo(EVSPool)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPool">An EVSPool object to compare with.</param>
        public Int32 CompareTo(ChargingPool EVSPool)
        {

            if ((Object) EVSPool == null)
                throw new ArgumentNullException("The given EVSPool must not be null!");

            return Id.CompareTo(EVSPool.Id);

        }

        #endregion

        #endregion

        #region IEquatable<EVSPool> Members

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

            // Check if the given object is an EVSPool.
            var EVSPool = Object as ChargingPool;
            if ((Object) EVSPool == null)
                return false;

            return this.Equals(EVSPool);

        }

        #endregion

        #region Equals(EVSPool)

        /// <summary>
        /// Compares two EVSPool for equality.
        /// </summary>
        /// <param name="EVSPool">An EVSPool to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPool EVSPool)
        {

            if ((Object) EVSPool == null)
                return false;

            return Id.Equals(EVSPool.Id);

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
            return "eMI3 EVSPool" + Id.ToString();
        }

        #endregion

    }

}
