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

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Illias.Commons.Votes;
using eu.Vanaheimr.Styx.Arrows;

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

        #region Events

        #region EVSPoolAddition

        private readonly IVotingNotificator<EVSEOperator, EVSPool, Boolean> EVSPoolAddition;

        /// <summary>
        /// Called whenever an EVS pool will be or was added.
        /// </summary>
        public IVotingSender<EVSEOperator, EVSPool, Boolean> OnEVSPoolAddition
        {
            get
            {
                return OnEVSPoolAddition;
            }
        }

        #endregion

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

        #region (internal) EVSEOperator(RoamingNetwork)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment Operator (EVSOP) to manage
        /// multiple Electric Vehicle Supply Equipments (EVSEs).
        /// </summary>
        /// <param name="RoamingNetwork">The unique identification of the associated roaming network.</param>
        internal EVSEOperator(RoamingNetwork  RoamingNetwork)
            : this(EVSEOperator_Id.New, RoamingNetwork)
        { }

        #endregion

        #region (internal) EVSEOperator(Id, RoamingNetwork)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment Operator (EVSOP) to manage
        /// multiple Electric Vehicle Supply Equipments (EVSEs)
        /// and having the given EVSEOperator_Id.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE operator.</param>
        /// <param name="RoamingNetwork">The unique identification of the associated roaming network.</param>
        internal EVSEOperator(EVSEOperator_Id  Id,
                              RoamingNetwork   RoamingNetwork)
            : base(Id)
        {

            this.Name                   = new I8NString();

            this._RegisteredEVSPools    = new ConcurrentDictionary<EVSPool_Id, EVSPool>();

            this.EVSPoolAddition        = new VotingNotificator<EVSEOperator, EVSPool, Boolean>(() => new VetoVote(), true);

            this.OnEVSPoolAddition.OnVoting += (evseoperator, evspool, vote) => RoamingNetwork.EVSPoolAddition.SendVoting2(evseoperator, evspool, vote);

        }

        #endregion

        #endregion


        #region CreateNewPool(EVSPool_Id, Action)

        /// <summary>
        /// Create and register a new EVS pool having the given
        /// unique EVS pool identification.
        /// </summary>
        public EVSPool CreateNewPool(EVSPool_Id EVSPool_Id, Action<EVSPool> Action)
        {

            if (EVSPool_Id == null)
                throw new ArgumentNullException("EVSPool_Id", "The given EVSPool_Id must not be null!");

            if (_RegisteredEVSPools.ContainsKey(EVSPool_Id))
                throw new EVSPoolAlreadyExists(EVSPool_Id, this.Id);


            var _EVSPool = new EVSPool(EVSPool_Id, this);

            if (Action != null)
                Action(_EVSPool);

            if (EVSPoolAddition.SendVoting(this, _EVSPool))
            {
                if (_RegisteredEVSPools.TryAdd(EVSPool_Id, _EVSPool))
                {
                    EVSPoolAddition.SendNotification(this, _EVSPool);
                    return _EVSPool;
                }
            }

            throw new Exception();

        }

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
