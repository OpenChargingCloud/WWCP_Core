/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
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
using System.Linq;
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
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// </summary>
    public class ChargingStation : AEntity<ChargingStation_Id>,
                                   IEquatable<ChargingStation>, IComparable<ChargingStation>, IComparable,
                                   IEnumerable<EVSE>
    {

        #region Data

        public  readonly ChargingPool                          Pool;
        private readonly ConcurrentDictionary<EVSE_Id, EVSE>  _EVSEs;

        #endregion

        #region Properties

        #region ServiceIdentification

        private String _ServiceIdentification;

        /// <summary>
        /// The internal service identification of the charging station maintained by the EVSE operator.
        /// </summary>
        [Optional]
        public String ServiceIdentification
        {

            get
            {
                return _ServiceIdentification;
            }

            set
            {
                SetProperty<String>(ref _ServiceIdentification, value);
            }

        }

        #endregion

        #region UserComment

        private I8NString _UserComment;

        /// <summary>
        /// A comment from the users.
        /// </summary>
        [Optional]
        public I8NString UserComment
        {

            get
            {
                return _UserComment;
            }

            //set
            //{
            //    SetProperty<I8NString>(ref _UserComment, value);
            //}

        }

        #endregion

        #region ServiceProviderComment

        private I8NString _ServiceProviderComment;

        /// <summary>
        /// A comment from the service provider.
        /// </summary>
        [Optional]
        public I8NString ServiceProviderComment
        {

            get
            {
                return _ServiceProviderComment;
            }

            //set
            //{
            //    SetProperty<I8NString>(ref _ServiceProviderComment, value);
            //}

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate _GeoLocation;

        /// <summary>
        /// The geographical location of the charging station.
        /// If it is not set return the geographical location of its EVSPool.
        /// </summary>
        [Optional]
        public GeoCoordinate GeoLocation
        {

            get
            {

                if (_GeoLocation.IsValid)
                    return _GeoLocation;

                else
                    return Pool.PoolLocation;

            }

            set
            {
                SetProperty<GeoCoordinate>(ref _GeoLocation, value);
            }

        }

        #endregion

        #region GridConnection

        private GridConnection _GridConnection;

        /// <summary>
        /// The grid connection of the charging station.
        /// </summary>
        [Optional]
        public GridConnection GridConnection
        {

            get
            {
                return _GridConnection;
            }

            set
            {
                SetProperty<GridConnection>(ref _GridConnection, value);
            }

        }

        #endregion

        #region Features

        private ChargingStationUIFeatures _Features;

        /// <summary>
        /// The features of the charging station.
        /// </summary>
        [Optional]
        public ChargingStationUIFeatures Features
        {

            get
            {
                return _Features;
            }

            set
            {
                SetProperty<ChargingStationUIFeatures>(ref _Features, value);
            }

        }

        #endregion

        #region AuthorizationOptions

        private AuthorizationOptions _AuthorizationOptions;

        /// <summary>
        /// The authorization options of the charging station.
        /// </summary>
        [Optional]
        public AuthorizationOptions AuthorizationOptions
        {

            get
            {
                return _AuthorizationOptions;
            }

            set
            {
                SetProperty<AuthorizationOptions>(ref _AuthorizationOptions, value);
            }

        }

        #endregion

        #region PhotoURIs

        private readonly List<String> _PhotoURIs;

        /// <summary>
        /// URIs of photos of this charging station.
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

        #region PointOfDelivery // MeterId

        private readonly String _PointOfDelivery;

        /// <summary>
        /// Point of delivery or meter identification.
        /// </summary>
        [Optional]
        public String PointOfDelivery
        {
            get
            {
                return _PointOfDelivery;
            }
        }

        #endregion


        #region EVSEs

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging station.
        /// </summary>
        public IEnumerable<EVSE> EVSEs
        {
            get
            {
                return _EVSEs.Values;
            }
        }

        #endregion

        #region EVSEIds

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment (EVSEs)
        /// present within this charging station.
        /// </summary>
        public IEnumerable<EVSE_Id> EVSEIds
        {
            get
            {
                return _EVSEs.Values.Select(v => v.Id);
            }
        }

        #endregion

        #endregion

        #region Events

        #region EVSEAddition

        private readonly IVotingNotificator<ChargingStation, EVSE, Boolean> EVSEAddition;

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

        #region (internal) ChargingStation()

        /// <summary>
        /// Create a new charging station having a random identification.
        /// </summary>
        public ChargingStation()
            : this(ChargingStation_Id.New)
        { }

        #endregion

        #region (internal) ChargingStation(EVSPool)

        /// <summary>
        /// Create a new charging station having a random identification.
        /// </summary>
        /// <param name="EVSPool">The parent EVS pool.</param>
        public ChargingStation(ChargingPool EVSPool)
            : this(ChargingStation_Id.New, EVSPool)
        { }

        #endregion

        #region (internal) ChargingStation(Id)

        /// <summary>
        /// Create a new charging station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station pool.</param>
        /// <param name="EVSPool">The parent EVS pool.</param>
        internal ChargingStation(ChargingStation_Id  Id)
            : base(Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the charging station must not be null!");

            #endregion

            #region Init data and properties

            this._EVSEs                   = new ConcurrentDictionary<EVSE_Id, EVSE>();

            this._PhotoURIs               = new List<String>();
            this._UserComment             = new I8NString();
            this._ServiceProviderComment  = new I8NString();
            //this.GeoLocation             = new GeoCoordinate();

            #endregion

            #region Init and link events

            this.EVSEAddition               = new VotingNotificator<ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            if (Pool != null)
            {
                this.OnEVSEAddition.OnVoting                  += (chargingstation, evse, vote) => Pool.EVSEAddition.SendVoting      (chargingstation, evse, vote);
                this.OnEVSEAddition.OnNotification            += (chargingstation, evse)       => Pool.EVSEAddition.SendNotification(chargingstation, evse);
            }

            // EVSE events
            this.SocketOutletAddition       = new VotingNotificator<EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);


            #endregion

        }

        #endregion

        #region (internal) ChargingStation(Id, EVSPool)

        /// <summary>
        /// Create a new charging station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station pool.</param>
        /// <param name="EVSPool">The parent EVS pool.</param>
        internal ChargingStation(ChargingStation_Id  Id,
                                 ChargingPool        EVSPool)
            : this(Id)
        {

            if (EVSPool == null)
                throw new ArgumentNullException("EVSPool", "The EVS pool must not be null!");

            this.Pool = EVSPool;

            this.SocketOutletAddition.OnVoting            += (evse, socketoutlet , vote) => EVSPool.SocketOutletAddition.SendVoting      (evse, socketoutlet, vote);
            this.SocketOutletAddition.OnNotification      += (evse, socketoutlet)        => EVSPool.SocketOutletAddition.SendNotification(evse, socketoutlet);

        }

        #endregion

        #endregion


        #region CreateNew()

        /// <summary>
        /// Create a new charging station having a random identification.
        /// </summary>
        public static ChargingStation CreateNew()
        {
            return new ChargingStation();
        }

        #endregion

        #region CreateNew(Id)

        /// <summary>
        /// Create a new charging station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        public static ChargingStation CreateNew(ChargingStation_Id Id)
        {
            return new ChargingStation(Id);
        }

        #endregion


        #region CreateNewEVSE(EVSEId, Action = null)

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the new EVSE.</param>
        /// <param name="Action">An optional delegate to configure the new EVSE after its creation.</param>
        public EVSE CreateNewEVSE(EVSE_Id EVSEId, Action<EVSE> Action = null)
        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException("EVSE_Id", "The given EVSE identification must not be null!");

            if (_EVSEs.ContainsKey(EVSEId))
                throw new EVSEAlreadyExists(EVSEId, this.Id);

            #endregion

            var _EVSE = new EVSE(EVSEId, this);

            Action.FailSafeInvoke(_EVSE);

            if (EVSEAddition.SendVoting(this, _EVSE))
            {
                if (_EVSEs.TryAdd(EVSEId, _EVSE))
                {
                    EVSEAddition.SendNotification(this, _EVSE);
                    return _EVSE;
                }
            }

            throw new Exception();

        }

        #endregion


        #region IEnumerable<EVSE> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _EVSEs.Values.GetEnumerator();
        }

        public IEnumerator<EVSE> GetEnumerator()
        {
            return _EVSEs.Values.GetEnumerator();
        }

        #endregion

        #region IComparable<ChargingStation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingStation.
            var ChargingStation = Object as ChargingStation;
            if ((Object) ChargingStation == null)
                throw new ArgumentException("The given object is not an ChargingStation!");

            return CompareTo(ChargingStation);

        }

        #endregion

        #region CompareTo(ChargingStation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation">An ChargingStation object to compare with.</param>
        public Int32 CompareTo(ChargingStation ChargingStation)
        {

            if ((Object) ChargingStation == null)
                throw new ArgumentNullException("The given ChargingStation must not be null!");

            return Id.CompareTo(ChargingStation.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStation> Members

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

            // Check if the given object is an ChargingStation.
            var ChargingStation = Object as ChargingStation;
            if ((Object) ChargingStation == null)
                return false;

            return this.Equals(ChargingStation);

        }

        #endregion

        #region Equals(ChargingStation)

        /// <summary>
        /// Compares two ChargingStation for equality.
        /// </summary>
        /// <param name="ChargingStation">An ChargingStation to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStation ChargingStation)
        {

            if ((Object) ChargingStation == null)
                return false;

            return Id.Equals(ChargingStation.Id);

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
            return "eMI3 ChargingStation: " + Id.ToString();
        }

        #endregion

    }

}
