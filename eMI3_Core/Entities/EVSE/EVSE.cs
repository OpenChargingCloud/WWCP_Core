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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace de.eMI3
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// This is meant to be one electrical circuit which can charge a electric vehicle
    /// independently. Thus there could be multiple interdependent power sockets.
    /// </summary>
    public class EVSE : AEntity<EVSE_Id>,
                        IEquatable<EVSE>, IComparable<EVSE>, IComparable,
                        IEnumerable<SocketOutlet>
    {

        #region Data

        internal readonly ChargingStation                                      ChargingStation;
        private  readonly ConcurrentDictionary<SocketOutlet_Id, SocketOutlet>  _SocketOutlets;

        #endregion

        #region Properties

        #region MaxPower

        private Double _MaxPower;

        /// <summary>
        /// Max power at connector [Watt].
        /// </summary>
        [Mandatory]
        public Double MaxPower
        {

            get
            {
                return _MaxPower;
            }

            set
            {
                SetProperty<Double>(ref _MaxPower, value);
            }

        }

        #endregion

        #region AverageVoltage

        private Double _AverageVoltage;

        /// <summary>
        /// Average voltage at connector [Volt].
        /// </summary>
        [Mandatory]
        public Double AverageVoltage
        {

            get
            {
                return _AverageVoltage;
            }

            set
            {
                SetProperty<Double>(ref _AverageVoltage, value);
            }

        }

        #endregion


        public IEnumerable<SocketOutlet> SocketOutlets
        {
            get
            {
                return _SocketOutlets.Values;
            }
        }

        #endregion

        #region Constructor(s)

        #region (internal) EVSE(Pool)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE)
        /// having a random EVSE_Id.
        /// </summary>
        public EVSE(ChargingStation ChargingStation)
            : this(EVSE_Id.New, ChargingStation)
        { }

        #endregion

        #region (internal) EVSE(Id, Pool)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE)
        /// having a random EVSE_Id.
        /// </summary>
        /// <param name="Id">The ChargingStation Id.</param>
        internal EVSE(EVSE_Id          Id,
                      ChargingStation  ChargingStation)
            : base(Id)
        {

            if (ChargingStation == null)
                throw new ArgumentNullException();

            this.ChargingStation        = ChargingStation;
            this._SocketOutlets          = new ConcurrentDictionary<SocketOutlet_Id, SocketOutlet>();

        }

        #endregion

        #endregion


        #region CreateNewSocketOutlet(SocketOutlet_Id, Action = null)

        /// <summary>
        /// Register a new socket outlet.
        /// </summary>
        public SocketOutlet CreateNewSocketOutlet(SocketOutlet_Id SocketOutlet_Id, Action<SocketOutlet> Action = null)
        {

            if (SocketOutlet_Id == null)
                throw new ArgumentNullException("SocketOutlet_Id", "The given SocketOutlet_Id must not be null!");

            if (_SocketOutlets.ContainsKey(SocketOutlet_Id))
                throw new Exception();


            var _SocketOutlet = new SocketOutlet(SocketOutlet_Id, this);

            if (Action != null)
                Action(_SocketOutlet);

            if (_SocketOutlets.TryAdd(SocketOutlet_Id, _SocketOutlet))
                return _SocketOutlet;

            throw new Exception();

        }

        #endregion


        #region IEnumerable<SocketOutlet> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _SocketOutlets.Values.GetEnumerator();
        }

        public IEnumerator<SocketOutlet> GetEnumerator()
        {
            return _SocketOutlets.Values.GetEnumerator();
        }

        #endregion

        #region IComparable<EVSE> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSE.
            var EVSE = Object as EVSE;
            if ((Object) EVSE == null)
                throw new ArgumentException("The given object is not an EVSE!");

            return CompareTo(EVSE);

        }

        #endregion

        #region CompareTo(EVSE)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE">An EVSE object to compare with.</param>
        public Int32 CompareTo(EVSE EVSE)
        {

            if ((Object) EVSE == null)
                throw new ArgumentNullException("The given EVSE must not be null!");

            return Id.CompareTo(EVSE.Id);

        }

        #endregion

        #endregion

        #region IEquatable<EVSE> Members

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

            // Check if the given object is an EVSE.
            var EVSE = Object as EVSE;
            if ((Object) EVSE == null)
                return false;

            return this.Equals(EVSE);

        }

        #endregion

        #region Equals(EVSE)

        /// <summary>
        /// Compares two EVSE for equality.
        /// </summary>
        /// <param name="EVSE">An EVSE to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE EVSE)
        {

            if ((Object) EVSE == null)
                return false;

            return Id.Equals(EVSE.Id);

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
