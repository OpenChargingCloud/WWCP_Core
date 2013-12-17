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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

#endregion

namespace org.emi3group
{

    /// <summary>
    /// A Electric Vehicle Roaming Provider (EVRP).
    /// </summary>
    public class RoamingProvider : AEntity<RoamingProvider_Id>,
                                   IEquatable<RoamingProvider>, IComparable<RoamingProvider>, IComparable,
                                   IEnumerable<EVSPool>
    {

        #region Data

        private readonly ConcurrentDictionary<EVSPool_Id, EVSPool>  _RegisteredEVSPools;

        #endregion

        #region Properties

        #region Name

        private I8NString _Name;

        /// <summary>
        /// The offical (multi-language) name of an EVSE operator.
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


        #region EVSPools

        public IEnumerable<EVSPool> EVSPools
        {
            get
            {
                return _RegisteredEVSPools.Values;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (internal) RoamingProvider()

        /// <summary>
        /// Create a new Electric Vehicle Roaming Provider (EVRP).
        /// </summary>
        internal RoamingProvider(RoamingNetwork  RoamingNetwork)
            : this(RoamingProvider_Id.New, RoamingNetwork)
        { }

        #endregion

        #region (internal) RoamingProvider(Id)

        /// <summary>
        /// Create a new Electric Vehicle Roaming Provider (EVRP)
        /// having the given RoamingProvider_Id.
        /// </summary>
        /// <param name="Id">The EVSPool Id.</param>
        internal RoamingProvider(RoamingProvider_Id  Id,
                                 RoamingNetwork      RoamingNetwork)
            : base(Id)
        {

            this.Name                   = new I8NString();

            this._RegisteredEVSPools    = new ConcurrentDictionary<EVSPool_Id, EVSPool>();

        }

        #endregion

        #endregion


        #region IEnumerable<EVSPool> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _RegisteredEVSPools.Values.GetEnumerator();
        }

        public IEnumerator<EVSPool> GetEnumerator()
        {
            return _RegisteredEVSPools.Values.GetEnumerator();
        }

        #endregion

        #region IComparable<EVSE_Operator> Members

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
            var EVSE_Operator = Object as RoamingProvider;
            if ((Object) EVSE_Operator == null)
                throw new ArgumentException("The given object is not an EVSE_Operator!");

            return CompareTo(EVSE_Operator);

        }

        #endregion

        #region CompareTo(EVSE_Operator)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Operator">An EVSE_Operator object to compare with.</param>
        public Int32 CompareTo(RoamingProvider EVSE_Operator)
        {

            if ((Object) EVSE_Operator == null)
                throw new ArgumentNullException("The given EVSE_Operator must not be null!");

            return Id.CompareTo(EVSE_Operator.Id);

        }

        #endregion

        #endregion

        #region IEquatable<EVSE_Operator> Members

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

            // Check if the given object is an EVSE_Operator.
            var EVSE_Operator = Object as RoamingProvider;
            if ((Object) EVSE_Operator == null)
                return false;

            return this.Equals(EVSE_Operator);

        }

        #endregion

        #region Equals(EVSE_Operator)

        /// <summary>
        /// Compares two EVSE_Operator for equality.
        /// </summary>
        /// <param name="EVSE_Operator">An EVSE_Operator to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingProvider EVSE_Operator)
        {

            if ((Object) EVSE_Operator == null)
                return false;

            return Id.Equals(EVSE_Operator.Id);

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
