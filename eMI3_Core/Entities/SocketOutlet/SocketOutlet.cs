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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace org.GraphDefined.eMI3
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment Connector to charge an electric vehicle (EV).
    /// </summary>
    public class SocketOutlet : AEntity<SocketOutlet_Id>,
                                IEquatable<SocketOutlet>, IComparable<SocketOutlet>, IComparable
    {

        #region Data

        public readonly EVSE EVSE;

        #endregion

        #region Properties

        /// <summary>
        /// [Watt]
        /// </summary>
        public Double       MaxPower                { get; set; }

        /// <summary>
        /// [Watt]
        /// </summary>
        public Double       RealTimePower           { get; set; }

        /// <summary>
        /// [Watt]
        /// </summary>
        public Double       GuranteedMinPower       { get; set; }



        public CableType    CableAttached           { get; set; }

        /// <summary>
        /// [mm]
        /// </summary>
        public Double       CableLength             { get; set; }


        public PlugType     Plug                    { get; set; }

        #endregion

        #region Constructor(s)

        #region (internal) SocketOutlet(EVSE)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment Connector
        /// having a random EVSE Connector Id.
        /// </summary>
        public SocketOutlet(EVSE EVSE)
            : this(SocketOutlet_Id.New, EVSE)
        { }

        #endregion

        #region (internal) SocketOutlet(Id, EVSE)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment Connector
        /// having the given EVSE Connector Id.
        /// </summary>
        /// <param name="Id">The ChargingStation Id.</param>
        internal SocketOutlet(SocketOutlet_Id  Id,
                              EVSE             EVSE)
            : base(Id)
        {

            if (EVSE == null)
                throw new ArgumentNullException();

            this.EVSE = EVSE;

        }

        #endregion

        #endregion


        #region IComparable<SocketOutlet> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an SocketOutlet.
            var SocketOutlet = Object as SocketOutlet;
            if ((Object) SocketOutlet == null)
                throw new ArgumentException("The given object is not an SocketOutlet!");

            return CompareTo(SocketOutlet);

        }

        #endregion

        #region CompareTo(SocketOutlet)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutlet">An SocketOutlet object to compare with.</param>
        public Int32 CompareTo(SocketOutlet SocketOutlet)
        {

            if ((Object) SocketOutlet == null)
                throw new ArgumentNullException("The given SocketOutlet must not be null!");

            return Id.CompareTo(SocketOutlet.Id);

        }

        #endregion

        #endregion

        #region IEquatable<SocketOutlet> Members

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

            // Check if the given object is an SocketOutlet.
            var SocketOutlet = Object as SocketOutlet;
            if ((Object) SocketOutlet == null)
                return false;

            return this.Equals(SocketOutlet);

        }

        #endregion

        #region Equals(SocketOutlet)

        /// <summary>
        /// Compares two SocketOutlet for equality.
        /// </summary>
        /// <param name="SocketOutlet">An SocketOutlet to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SocketOutlet SocketOutlet)
        {

            if ((Object) SocketOutlet == null)
                return false;

            return Id.Equals(SocketOutlet.Id);

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
            return "eMI3 SocketOutlet: " + Id.ToString();
        }

        #endregion

    }

}
