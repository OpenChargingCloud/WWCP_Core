/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/eMI3/Core>
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

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Illias.Commons.Votes;
using eu.Vanaheimr.Styx.Arrows;

#endregion

namespace org.emi3group
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// </summary>
    public class ChargingStation : AEntity<ChargingStation_Id>,
                                   IEquatable<ChargingStation>, IComparable<ChargingStation>, IComparable,
                                   IEnumerable<EVSE>
    {

        #region Data

        public  readonly ChargingPool                              Pool;
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

            set
            {
                SetProperty<I8NString>(ref _UserComment, value);
            }

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

            set
            {
                SetProperty<I8NString>(ref _ServiceProviderComment, value);
            }

        }

        #endregion

        #region GeoLocation

        private GeoLocation _GeoLocation;

        /// <summary>
        /// The geographical location of the charging station.
        /// If it is not set return the geographical location of its EVSPool.
        /// </summary>
        [Optional]
        public GeoLocation GeoLocation
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
                SetProperty<GeoLocation>(ref _GeoLocation, value);
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

        #region PhotoURL

        private readonly List<String> _Photos;

        /// <summary>
        /// A photo of the charging station.
        /// </summary>
        [Optional, Not_eMI3defined]
        public List<String> Photos
        {
            get
            {
                return _Photos;
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

        #region (internal) ChargingStation(EVSPool)

        /// <summary>
        /// Create a new charging station having a random identification.
        /// </summary>
        /// <param name="EVSPool">The parent EVS pool.</param>
        public ChargingStation(ChargingPool EVSPool)
            : this(ChargingStation_Id.New, EVSPool)
        { }

        #endregion

        #region (internal) ChargingStation(Id, EVSPool)

        /// <summary>
        /// Create a new charging station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station pool.</param>
        /// <param name="EVSPool">The parent EVS pool.</param>
        internal ChargingStation(ChargingStation_Id  Id,
                                 ChargingPool             EVSPool)
            : base(Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the charging station must not be null!");

            if (EVSPool == null)
                throw new ArgumentNullException("EVSPool", "The EVS pool must not be null!");

            this.Pool = EVSPool;

            #endregion

            #region Init data and properties

            this._EVSEs                  = new ConcurrentDictionary<EVSE_Id, EVSE>();

            this._Photos                 = new List<String>();
            this.UserComment             = new I8NString();
            this.ServiceProviderComment  = new I8NString();
            this.GeoLocation             = new GeoLocation();

            #endregion

            #region Init and link events

            this.EVSEAddition               = new VotingNotificator<ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            this.OnEVSEAddition.OnVoting                  += (chargingstation, evse, vote) => Pool.EVSEAddition.SendVoting      (chargingstation, evse, vote);
            this.OnEVSEAddition.OnNotification            += (chargingstation, evse)       => Pool.EVSEAddition.SendNotification(chargingstation, evse);

            // EVSE events
            this.SocketOutletAddition       = new VotingNotificator<EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            this.SocketOutletAddition.OnVoting            += (evse, socketoutlet , vote) => EVSPool.SocketOutletAddition.SendVoting      (evse, socketoutlet, vote);
            this.SocketOutletAddition.OnNotification      += (evse, socketoutlet)        => EVSPool.SocketOutletAddition.SendNotification(evse, socketoutlet);

            #endregion

        }

        #endregion

        #endregion


        #region CreateNewEVSE(EVSE_Id, Action = null)

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="EVSE_Id">The unique identification of the new EVSE.</param>
        /// <param name="Action">An optional delegate to configure the new EVSE after its creation.</param>
        public EVSE CreateNewEVSE(EVSE_Id EVSE_Id, Action<EVSE> Action = null)
        {

            #region Initial checks

            if (EVSE_Id == null)
                throw new ArgumentNullException("EVSE_Id", "The given EVSE identification must not be null!");

            if (_EVSEs.ContainsKey(EVSE_Id))
                throw new EVSEAlreadyExists(EVSE_Id, this.Id);

            #endregion

            var _EVSE = new EVSE(EVSE_Id, this);

            Action.FailSafeInvoke(_EVSE);

            if (EVSEAddition.SendVoting(this, _EVSE))
            {
                if (_EVSEs.TryAdd(EVSE_Id, _EVSE))
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
