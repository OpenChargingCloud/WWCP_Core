/*
 * Copyright (c) 2013 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 Mockup <http://www.github.com/eMI3/Mockup>
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
    /// A group/pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the EVSPool
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class EVSPool : AEntity<EVSPool_Id>,
                           IEquatable<EVSPool>, IComparable<EVSPool>, IComparable,
                           IEnumerable<ChargingStation>
    {

        #region Data

        internal readonly EVSEOperator                                               Operator;
        private  readonly ConcurrentDictionary<ChargingStation_Id, ChargingStation>  _ChargingStations;

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

        private String _EVSEOperator;

        /// <summary>
        /// The EVSE operator of this EVSPool.
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


        public IEnumerable<ChargingStation> ChargingStations
        {
            get
            {
                return _ChargingStations.Values;
            }
        }

        #endregion

        #region Events

        #endregion

        #region Constructor(s)

        #region (internal) EVSPool(Operator)

        /// <summary>
        /// Create a new group/pool of Electric Vehicle Supply Equipments (EVSPool)
        /// having a random EVSPool identification.
        /// </summary>
        internal EVSPool(EVSEOperator  Operator)
            : this(EVSPool_Id.New, Operator)
        { }

        #endregion

        #region (internal) EVSPool(Id, Operator)

        /// <summary>
        /// Create a new group/pool of Electric Vehicle Supply Equipments (EVSPool)
        /// having the given EVSPool identification.
        /// </summary>
        /// <param name="Id">The EVSPool Id.</param>
        internal EVSPool(EVSPool_Id    Id,
                         EVSEOperator  Operator)
            : base(Id)
        {

            if (Operator == null)
                throw new ArgumentNullException();

            this.Operator           = Operator;
            this._ChargingStations   = new ConcurrentDictionary<ChargingStation_Id, ChargingStation>();

            this.Name               = new I8NString(Languages.en, Id.ToString());
            this.Description        = new I8NString();
            this.LocationLanguage   = Languages.undef;

            this.PoolLocation       = new GeoLocation();
            this.Address            = new Address();
            this.EntranceLocation   = new GeoLocation();
            this.EntranceAddress    = new Address();

        }

        #endregion

        #endregion


        #region CreateNewStation(ChargingStation_Id, Action)

        /// <summary>
        /// Register a new charging station.
        /// </summary>
        public ChargingStation CreateNewStation(ChargingStation_Id ChargingStation_Id, Action<ChargingStation> Action)
        {

            if (ChargingStation_Id == null)
                throw new ArgumentNullException("ChargingStation_Id", "The given ChargingStation_Id must not be null!");

            if (_ChargingStations.ContainsKey(ChargingStation_Id))
                throw new Exception();


            var _ChargingStation = new ChargingStation(ChargingStation_Id, this);

            if (Action != null)
                Action(_ChargingStation);

            if (_ChargingStations.TryAdd(ChargingStation_Id, _ChargingStation))
                return _ChargingStation;

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
            var EVSPool = Object as EVSPool;
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
        public Int32 CompareTo(EVSPool EVSPool)
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
            var EVSPool = Object as EVSPool;
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
        public Boolean Equals(EVSPool EVSPool)
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
            return Id.ToString();
        }

        #endregion

    }

}
