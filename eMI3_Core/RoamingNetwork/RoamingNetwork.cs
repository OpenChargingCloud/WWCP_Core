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
    /// A group/pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the RoamingNetwork
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class RoamingNetwork : AEntity<RoamingNetwork_Id>,
                                  IEquatable<RoamingNetwork>, IComparable<RoamingNetwork>, IComparable,
                                  IEnumerable<IEntity>
    {

        #region Data

        private  readonly ConcurrentDictionary<EVSEOperator_Id, EVSEOperator>  _EVSEOperators;

        #endregion

        #region Properties

        #region Name

        private I8NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the RoamingNetwork.
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
        /// An optional additional (multi-language) description of the RoamingNetwork.
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


        public IEnumerable<EVSEOperator> EVSEOperators
        {
            get
            {
                return _EVSEOperators.Values;
            }
        }

        #endregion

        #region Events

        #endregion

        #region Constructor(s)

        #region RoamingNetwork()

        /// <summary>
        /// Create a new group/pool of Electric Vehicle Supply Equipments (RoamingNetwork)
        /// having a random RoamingNetwork identification.
        /// </summary>
        public RoamingNetwork()
            : this(RoamingNetwork_Id.New)
        { }

        #endregion

         #region RoamingNetwork(Id, Operator)

        /// <summary>
        /// Create a new group/pool of Electric Vehicle Supply Equipments (RoamingNetwork)
        /// having the given RoamingNetwork identification.
        /// </summary>
        /// <param name="Id">The RoamingNetwork Id.</param>
        public RoamingNetwork(RoamingNetwork_Id Id)
            : base(Id)
        {

            this._EVSEOperators     = new ConcurrentDictionary<EVSEOperator_Id, EVSEOperator>();

            this.Name               = new I8NString(Languages.en, Id.ToString());
            this.Description        = new I8NString();

        }

        #endregion

        #endregion


        #region CreateNewEVSEOperator(EVSEOperator_Id, Action = null)

        /// <summary>
        /// Register a new charging station.
        /// </summary>
        public EVSEOperator CreateNewEVSEOperator(EVSEOperator_Id EVSEOperator_Id, Action<EVSEOperator> Action = null)
        {

            if (EVSEOperator_Id == null)
                throw new ArgumentNullException("EVSEOperator_Id", "The given EVSEOperator_Id must not be null!");

            if (_EVSEOperators.ContainsKey(EVSEOperator_Id))
                throw new Exception();


            var _EVSEOperator = new EVSEOperator(EVSEOperator_Id, this);

            if (Action != null)
                Action(_EVSEOperator);

            if (_EVSEOperators.TryAdd(EVSEOperator_Id, _EVSEOperator))
                return _EVSEOperator;

            throw new Exception();

        }

        #endregion
 

        #region IEnumerable<IEntity> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _EVSEOperators.Values.GetEnumerator();
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return _EVSEOperators.Values.GetEnumerator();
        }

        #endregion

        #region IComparable<RoamingNetwork> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an RoamingNetwork.
            var RoamingNetwork = Object as RoamingNetwork;
            if ((Object) RoamingNetwork == null)
                throw new ArgumentException("The given object is not an RoamingNetwork!");

            return CompareTo(RoamingNetwork);

        }

        #endregion

        #region CompareTo(RoamingNetwork)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork">An RoamingNetwork object to compare with.</param>
        public Int32 CompareTo(RoamingNetwork RoamingNetwork)
        {

            if ((Object) RoamingNetwork == null)
                throw new ArgumentNullException("The given RoamingNetwork must not be null!");

            return Id.CompareTo(RoamingNetwork.Id);

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetwork> Members

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

            // Check if the given object is an RoamingNetwork.
            var RoamingNetwork = Object as RoamingNetwork;
            if ((Object) RoamingNetwork == null)
                return false;

            return this.Equals(RoamingNetwork);

        }

        #endregion

        #region Equals(RoamingNetwork)

        /// <summary>
        /// Compares two RoamingNetwork for equality.
        /// </summary>
        /// <param name="RoamingNetwork">An RoamingNetwork to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetwork RoamingNetwork)
        {

            if ((Object) RoamingNetwork == null)
                return false;

            return Id.Equals(RoamingNetwork.Id);

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
