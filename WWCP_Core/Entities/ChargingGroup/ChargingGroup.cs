/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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
    /// A group/pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the ChargingGroup
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingGroup : AEMobilityEntity<ChargingGroup_Id>,
                                 IEquatable<ChargingGroup>, IComparable<ChargingGroup>, IComparable,
                                 IEnumerable<ChargingStation>
    {

        #region Data

        public  readonly EVSEOperator                                               Operator;
        private readonly ConcurrentDictionary<ChargingStation_Id, ChargingStation>  _ChargingStations;

        #endregion

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the ChargingGroup.
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
        /// An optional additional (multi-language) description of the ChargingGroup.
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
        /// The official language at this ChargingGroup.
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
        /// The geographical location of this ChargingGroup.
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
        /// The geographical location of the entrance of this ChargingGroup.
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
        /// The address of this ChargingGroup.
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
        /// The address of the entrance of this ChargingGroup.
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
        /// The owner of this ChargingGroup.
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
        /// The owner of the ChargingGroup location.
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

        private String _EVSEOperator;

        /// <summary>
        /// The EVSE operator of this ChargingGroup.
        /// </summary>
        [Optional]
        public String EVSEOperator
        {

            get
            {
                return _EVSEOperator;
            }

            set
            {
                SetProperty<String>(ref _EVSEOperator, value);
            }

        }

        #endregion

        #region OpeningTime

        private OpeningTime _OpeningTime;

        /// <summary>
        /// The opening time of this ChargingGroup.
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
                return _ChargingStations.Select(KVP => KVP.Value);
            }
        }

        #endregion

        #endregion

        #region Events

        #region ChargingStationAddition

        private readonly IVotingNotificator<DateTime, ChargingGroup, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingGroup, ChargingStation, Boolean> OnChargingStationAddition
        {
            get
            {
                return ChargingStationAddition;
            }
        }

        #endregion


        // Charging station events

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


        // EVSE events

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

        #endregion

        #region Constructor(s)

        #region (internal) ChargingGroup(Operator)

        /// <summary>
        /// Create a new group/pool of charging stations having a random identification.
        /// </summary>
        /// <param name="EVSEOperator">The parent EVSE operator.</param>
        internal ChargingGroup(EVSEOperator EVSEOperator)
            : this(ChargingGroup_Id.New, EVSEOperator)
        { }

        #endregion

        #region (internal) ChargingGroup(Id, EVSEOperator)

        /// <summary>
        /// Create a new group/pool of charging stations having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the EVS pool.</param>
        /// <param name="EVSEOperator">The parent EVSE operator.</param>
        internal ChargingGroup(ChargingGroup_Id    Id,
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

            this.Name               = new I18NString(Languages.en, Id.ToString());
            this.Description        = new I18NString();
            this.LocationLanguage   = Languages.unknown;

            //this.PoolLocation       = new GeoCoordinate();
            this.Address            = new Address();
            //this.EntranceLocation   = new GeoCoordinate();
            this.EntranceAddress    = new Address();

            #endregion

            #region Init and link events

            // EVS pool events
            this.ChargingStationAddition    = new VotingNotificator<DateTime, ChargingGroup, ChargingStation, Boolean>(() => new VetoVote(), true);

            //this.OnChargingStationAddition.OnVoting       += (evseoperator, ChargingGroup, vote) => Operator.ChargingStationAddition.SendVoting      (evseoperator, ChargingGroup, vote);
            //this.OnChargingStationAddition.OnNotification += (evseoperator, ChargingGroup)       => Operator.ChargingStationAddition.SendNotification(evseoperator, ChargingGroup);


            // Charging station events
            this.EVSEAddition               = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            this.OnEVSEAddition.OnVoting                  += (timestamp, chargingstation, evse, vote) => EVSEOperator.EVSEAddition.SendVoting      (timestamp, chargingstation, evse, vote);
            this.OnEVSEAddition.OnNotification            += (timestamp, chargingstation, evse)       => EVSEOperator.EVSEAddition.SendNotification(timestamp, chargingstation, evse);


            // EVSE events
            this.SocketOutletAddition       = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            this.SocketOutletAddition.OnVoting            += (timestamp, evse, socketoutlet , vote) => EVSEOperator.SocketOutletAddition.SendVoting      (timestamp, evse, socketoutlet, vote);
            this.SocketOutletAddition.OnNotification      += (timestamp, evse, socketoutlet)        => EVSEOperator.SocketOutletAddition.SendNotification(timestamp, evse, socketoutlet);

            #endregion

        }

        #endregion

        #endregion


        #region AddStation(ChargingStation_Id, Action = null)

        /// <summary>
        /// Add an existing charging station to the group.
        /// </summary>
        /// <param name="ChargingStation">A charging station to add.</param>
        /// <param name="Action">An optional delegate to do something with the charging station after its addition.</param>
        public ChargingStation AddStation(ChargingStation          ChargingStation,
                                          Action<ChargingStation>  Action  = null)
        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The given charging station must not be null!");

            if (_ChargingStations.ContainsKey(ChargingStation.Id))
                throw new ChargingStationAlreadyExistsInGroup(ChargingStation.Id, this.Id);

            #endregion

            //Action.FailSafeInvoke(_ChargingStation);

            //if (ChargingStationAddition.SendVoting(this, _ChargingStation))
            //{
            //    if (_ChargingStations.TryAdd(ChargingStation_Id, _ChargingStation))
            //    {
            //        ChargingStationAddition.SendNotification(this, _ChargingStation);
            //        return _ChargingStation;
            //    }
            //}

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

        #region IComparable<ChargingGroup> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingGroup.
            var ChargingGroup = Object as ChargingGroup;
            if ((Object) ChargingGroup == null)
                throw new ArgumentException("The given object is not an ChargingGroup!");

            return CompareTo(ChargingGroup);

        }

        #endregion

        #region CompareTo(ChargingGroup)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingGroup">An ChargingGroup object to compare with.</param>
        public Int32 CompareTo(ChargingGroup ChargingGroup)
        {

            if ((Object) ChargingGroup == null)
                throw new ArgumentNullException("The given ChargingGroup must not be null!");

            return Id.CompareTo(ChargingGroup.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingGroup> Members

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

            // Check if the given object is an ChargingGroup.
            var ChargingGroup = Object as ChargingGroup;
            if ((Object) ChargingGroup == null)
                return false;

            return this.Equals(ChargingGroup);

        }

        #endregion

        #region Equals(ChargingGroup)

        /// <summary>
        /// Compares two ChargingGroup for equality.
        /// </summary>
        /// <param name="ChargingGroup">An ChargingGroup to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingGroup ChargingGroup)
        {

            if ((Object) ChargingGroup == null)
                return false;

            return Id.Equals(ChargingGroup.Id);

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
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return "eMI3 ChargingGroup" + Id.ToString();
        }

        #endregion

    }

}
