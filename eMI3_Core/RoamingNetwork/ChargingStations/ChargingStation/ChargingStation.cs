/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
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

#endregion

namespace de.eMI3
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// </summary>
    public class ChargingStation : AEntity<ChargingStation_Id>,
                                   IEquatable<ChargingStation>, IComparable<ChargingStation>, IComparable,
                                   IEnumerable<EVSE>
    {

        #region Data

        internal readonly EVSPool                              Pool;
        private  readonly ConcurrentDictionary<EVSE_Id, EVSE>  _EVSEs;

        #endregion

        #region Properties

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

        #region StationLocation

        private GeoLocation _StationLocation;

        /// <summary>
        /// The geographical location of the charging station.
        /// If it is not set return the geographical location of its EVSPool.
        /// </summary>
        [Optional]
        public GeoLocation GeoLocation
        {

            get
            {

                if (_StationLocation.IsValid)
                    return _StationLocation;

                else
                    return Pool.PoolLocation;

            }

            set
            {
                SetProperty<GeoLocation>(ref _StationLocation, value);
            }

        }

        #endregion

        #region Features

        private ChargingStationFeatures _Features;

        /// <summary>
        /// The features of the charging station.
        /// </summary>
        [Optional]
        public ChargingStationFeatures Features
        {

            get
            {
                return _Features;
            }

            set
            {
                SetProperty<ChargingStationFeatures>(ref _Features, value);
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
        [Optional]
        public List<String> Photos
        {
            get
            {
                return _Photos;
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

        #region Constructor(s)

        #region (internal) ChargingStation(Pool)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (ChargingStation)
        /// having a random ChargingStation_Id.
        /// </summary>
        public ChargingStation(EVSPool Pool)
            : this(ChargingStation_Id.New, Pool)
        { }

        #endregion

        #region (internal) ChargingStation(Id, Pool)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (ChargingStation)
        /// having the given ChargingStation_Id.
        /// </summary>
        /// <param name="Id">The ChargingStation Id.</param>
        internal ChargingStation(ChargingStation_Id  Id,
                                 EVSPool             Pool)
            : base(Id)
        {

            if (Pool == null)
                throw new ArgumentNullException();

            this.Pool                    = Pool;
            this._EVSEs                  = new ConcurrentDictionary<EVSE_Id, EVSE>();

            this._Photos                 = new List<String>();
            this.UserComment             = new I8NString();
            this.ServiceProviderComment  = new I8NString();
            this.GeoLocation             = new GeoLocation();

        }

        #endregion

        #endregion


        public EVSE CreateNewEVSE(EVSE_Id Id, Action<EVSE> Action = null)
        {

            if (Id == null)
                throw new ArgumentNullException("Id", "The given Id must not be null!");

            if (_EVSEs.ContainsKey(Id))
                throw new Exception();

            var _EVSE = new EVSE(Id, this);


            if (Action != null)
                Action(_EVSE);

            if (_EVSEs.TryAdd(Id, _EVSE))
                return _EVSE;

            throw new Exception();

        }


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
            return Id.ToString();
        }

        #endregion

    }

}
