/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3>
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
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace com.graphdefined.eMI3
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

        public  readonly EVSEOperator                                               Operator;
        private readonly ConcurrentDictionary<ChargingStation_Id, ChargingStation>  _ChargingStations;

        #endregion

        #region Properties

        #region Name

        private I8NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the EVSPool.
        /// </summary>
        [Mandatory]
        public I8NString Name
        {

            get
            {
                return _Name;
            }

            set
            {
                SetProperty<I8NString>(ref _Name, value);
            }

        }

        #endregion

        #region Description

        private I8NString _Description;

        /// <summary>
        /// An optional additional (multi-language) description of the EVSPool.
        /// </summary>
        [Optional]
        public I8NString Description
        {

            get
            {
                return _Description;
            }

            set
            {
                SetProperty<I8NString>(ref _Description, value);
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

        private GeoLocation _PoolLocation;

        /// <summary>
        /// The geographical location of this EVSPool.
        /// </summary>
        [Optional]
        public GeoLocation PoolLocation
        {

            get
            {
                return _PoolLocation;
            }

            set
            {
                SetProperty<GeoLocation>(ref _PoolLocation, value);
            }

        }

        #endregion

        #region EntranceLocation

        private GeoLocation _EntranceLocation;

        /// <summary>
        /// The geographical location of the entrance of this EVSPool.
        /// (If different from 'PoolLocation').
        /// </summary>
        [Optional]
        public GeoLocation EntranceLocation
        {

            get
            {
                return _EntranceLocation;
            }

            set
            {
                SetProperty<GeoLocation>(ref _EntranceLocation, value);
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

        #region EVSEOperator

        private EVSEOperator_Id _EVSEOperator;

        /// <summary>
        /// The EVSE operator of this EVSPool.
        /// </summary>
        [Optional]
        public EVSEOperator_Id EVSEOperator
        {

            get
            {
                return _EVSEOperator;
            }

            set
            {
                SetProperty<EVSEOperator_Id>(ref _EVSEOperator, value);
            }

        }

        #endregion

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


        #region ChargingStations

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

        #endregion

        #region Constructor(s)

        #region (internal) EVSPool(Operator)

        /// <summary>
        /// Create a new group/pool of charging stations having a random identification.
        /// </summary>
        /// <param name="EVSEOperator">The parent EVSE operator.</param>
        internal ChargingPool(EVSEOperator EVSEOperator)
            : this(ChargingPool_Id.New, EVSEOperator)
        { }

        #endregion

        #region (internal) EVSPool(Id, EVSEOperator)

        /// <summary>
        /// Create a new group/pool of charging stations having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the EVS pool.</param>
        /// <param name="EVSEOperator">The parent EVSE operator.</param>
        internal ChargingPool(ChargingPool_Id    Id,
                         EVSEOperator  EVSEOperator)
            : base(Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the EVS pool must not be null!");

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The EVSE operator must not be null!");

            this.Operator = EVSEOperator;

            #endregion

            #region Init data and properties

            this._ChargingStations  = new ConcurrentDictionary<ChargingStation_Id, ChargingStation>();

            this.Name               = new I8NString(Languages.en, Id.ToString());
            this.Description        = new I8NString();
            this.LocationLanguage   = Languages.undef;

            this.PoolLocation       = new GeoLocation();
            this.Address            = new Address();
            this.EntranceLocation   = new GeoLocation();
            this.EntranceAddress    = new Address();

            #endregion

            #region Init and link events

            // EVS pool events
            this.ChargingStationAddition    = new VotingNotificator<ChargingPool, ChargingStation, Boolean>(() => new VetoVote(), true);

            this.OnChargingStationAddition.OnVoting       += (evseoperator, evspool, vote) => Operator.ChargingStationAddition.SendVoting      (evseoperator, evspool, vote);
            this.OnChargingStationAddition.OnNotification += (evseoperator, evspool)       => Operator.ChargingStationAddition.SendNotification(evseoperator, evspool);


            // Charging station events
            this.EVSEAddition               = new VotingNotificator<ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            this.OnEVSEAddition.OnVoting                  += (chargingstation, evse, vote) => EVSEOperator.EVSEAddition.SendVoting      (chargingstation, evse, vote);
            this.OnEVSEAddition.OnNotification            += (chargingstation, evse)       => EVSEOperator.EVSEAddition.SendNotification(chargingstation, evse);


            // EVSE events
            this.SocketOutletAddition       = new VotingNotificator<EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            this.SocketOutletAddition.OnVoting            += (evse, socketoutlet , vote) => EVSEOperator.SocketOutletAddition.SendVoting      (evse, socketoutlet, vote);
            this.SocketOutletAddition.OnNotification      += (evse, socketoutlet)        => EVSEOperator.SocketOutletAddition.SendNotification(evse, socketoutlet);

            #endregion

        }

        #endregion

        #endregion


        #region CreateNewStation(ChargingStation_Id, Action = null)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="ChargingStation_Id">The unique identification of the new charging station.</param>
        /// <param name="Action">An optional delegate to configure the new charging station after its creation.</param>
        public ChargingStation CreateNewStation(ChargingStation_Id ChargingStation_Id, Action<ChargingStation> Action = null)
        {

            #region Initial checks

            if (ChargingStation_Id == null)
                throw new ArgumentNullException("ChargingStation_Id", "The given charging station identification must not be null!");

            if (_ChargingStations.ContainsKey(ChargingStation_Id))
                throw new ChargingStationAlreadyExistsInPool(ChargingStation_Id, this.Id);

            #endregion

            var _ChargingStation = new ChargingStation(ChargingStation_Id, this);

            Action.FailSafeInvoke(_ChargingStation);

            if (ChargingStationAddition.SendVoting(this, _ChargingStation))
            {
                if (_ChargingStations.TryAdd(ChargingStation_Id, _ChargingStation))
                {
                    ChargingStationAddition.SendNotification(this, _ChargingStation);
                    return _ChargingStation;
                }
            }

            throw new Exception();

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
