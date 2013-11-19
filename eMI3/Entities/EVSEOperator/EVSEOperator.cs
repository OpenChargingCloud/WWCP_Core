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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

#endregion

namespace de.eMI3
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment Operator (EVSOP) to manage
    /// multiple Electric Vehicle Supply Equipments (EVSEs).
    /// </summary>
    public class EVSEOperator : AEntity<EVSEOperator_Id>,
                                IEquatable<EVSEOperator>, IComparable<EVSEOperator>, IComparable,
                                IEnumerable<EVSPool>
    {

        #region Data

        private readonly ConcurrentDictionary<EVSPool_Id, EVSPool>  _RegisteredEVSPools;

        #endregion

        #region Properties


        #endregion

        #region Constructor(s)

        #region EVSEOperator()

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment Operator (EVSOP) to manage
        /// multiple Electric Vehicle Supply Equipments (EVSEs).
        /// </summary>
        public EVSEOperator()
            : this(EVSEOperator_Id.New)
        { }

        #endregion

        #region EVSEOperator(Id)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment Operator (EVSOP) to manage
        /// multiple Electric Vehicle Supply Equipments (EVSEs)
        /// and having the given EVSEOperator_Id.
        /// </summary>
        /// <param name="Id">The EVSPool Id.</param>
        public EVSEOperator(EVSEOperator_Id Id)
            : base(Id)
        {

            this._RegisteredEVSPools  = new ConcurrentDictionary<EVSPool_Id, EVSPool>();

        }

        #endregion

        #endregion


        #region CreateNewPool(EVSPool_Id, Action)

        /// <summary>
        /// Register an EVSPool.
        /// </summary>
        public EVSPool CreateNewPool(EVSPool_Id EVSPool_Id, Action<EVSPool> Action)
        {

            if (EVSPool_Id == null)
                throw new ArgumentNullException("EVSPool_Id", "The given EVSPool_Id must not be null!");

            if (_RegisteredEVSPools.ContainsKey(EVSPool_Id))
                throw new Exception();


            var _EVSPool = new EVSPool(EVSPool_Id, this);

            if (Action != null)
                Action(_EVSPool);

            if (_RegisteredEVSPools.TryAdd(EVSPool_Id, _EVSPool))
                return _EVSPool;

            throw new Exception();

        }

        #endregion

        //#region RegisterEVSPool(EVSPool)

        ///// <summary>
        ///// Register an EVSE.
        ///// </summary>
        ///// <param name="EVSE">An EVSE.</param>
        //public RegisterEVSEResult RegisterEVSPool(EVSPool EVSPool)
        //{

        //    if (EVSPool == null)
        //        throw new ArgumentNullException("EVSPool", "The given EVSPool must not be null!");

        //    if (RegisteredEVSPools.TryAdd(EVSPool.Id, EVSPool))
        //    {
        //        EVSPool.Operator = this;
        //        return RegisterEVSEResult.success;
        //    }

        //    if (RegisteredEVSPools.ContainsKey(EVSPool.Id))
        //        return RegisterEVSEResult.duplicate;

        //    return RegisterEVSEResult.unknown;

        //}

        //#endregion

        //#region RegisterEVSPools(EVSPools)

        ///// <summary>
        ///// Register multiple EVSEs.
        ///// </summary>
        ///// <param name="EVSEs">An enumeration of EVSEs.</param>
        //public RegisterEVSEResult RegisterEVSPools(IEnumerable<EVSPool> EVSPools)
        //{

        //    if (EVSPools == null)
        //        throw new ArgumentNullException("EVSPools", "The given enumeration of EVSPools must not be null!");

        //    foreach (var EVSPool in EVSPools)
        //        if (RegisteredEVSPools.ContainsKey(EVSPool.Id))
        //            return RegisterEVSEResult.duplicate;

        //    foreach (var EVSPool in EVSPools)
        //        if (RegisteredEVSPools.TryAdd(EVSPool.Id, EVSPool))
        //            EVSPool.Operator = this;
        //        else
        //            return RegisterEVSEResult.failed;

        //    return RegisterEVSEResult.success;

        //}

        //#endregion


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
            var EVSE_Operator = Object as EVSEOperator;
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
        public Int32 CompareTo(EVSEOperator EVSE_Operator)
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
            var EVSE_Operator = Object as EVSEOperator;
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
        public Boolean Equals(EVSEOperator EVSE_Operator)
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
