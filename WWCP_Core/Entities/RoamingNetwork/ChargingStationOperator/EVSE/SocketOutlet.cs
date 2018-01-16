/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A socket outlet to connect an electric vehicle (EV)
    /// to an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public class SocketOutlet : IEquatable<SocketOutlet>
    {

        #region Properties

        /// <summary>
        /// The type of the charging plug.
        /// </summary>
        [Mandatory]
        public PlugTypes  Plug            { get; }

        /// <summary>
        /// Whether the charging plug is lockable or not.
        /// </summary>
        [Optional]
        public Boolean    Lockable        { get; }

        /// <summary>
        /// Whether the charging plug has an attached cable or not.
        /// </summary>
        [Optional]
        public Boolean?   CableAttached   { get; }

        /// <summary>
        /// The length of the charging cable [cm].
        /// </summary>
        [Optional]
        public Double     CableLength     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new socket outlet.
        /// </summary>
        /// <param name="Plug">The type of the charging plug.</param>
        /// <param name="Lockable">Whether the charging plug is lockable or not.</param>
        /// <param name="CableAttached">The type of the charging cable.</param>
        /// <param name="CableLength">The length of the charging cable [mm].</param>
        public SocketOutlet(PlugTypes  Plug,
                            Boolean    Lockable       = true,
                            Boolean?   CableAttached  = null,
                            Double     CableLength    = 0)
        {

            this.Plug           = Plug;
            this.Lockable       = Lockable;
            this.CableAttached  = CableAttached;
            this.CableLength    = CableLength;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SocketOutlet1, SocketOutlet2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutlet1">An socket outlet.</param>
        /// <param name="SocketOutlet2">Another socket outlet.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SocketOutlet SocketOutlet1, SocketOutlet SocketOutlet2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SocketOutlet1, SocketOutlet2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SocketOutlet1 == null) || ((Object) SocketOutlet2 == null))
                return false;

            return SocketOutlet1.Equals(SocketOutlet2);

        }

        #endregion

        #region Operator != (SocketOutlet1, SocketOutlet2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutlet1">An socket outlet.</param>
        /// <param name="SocketOutlet2">Another socket outlet.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SocketOutlet SocketOutlet1, SocketOutlet SocketOutlet2)
            => !(SocketOutlet1 == SocketOutlet2);

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

            var SocketOutlet = Object as SocketOutlet;
            if ((Object) SocketOutlet == null)
                return false;

            return Equals(SocketOutlet);

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

            return Plug.         Equals(SocketOutlet.Plug)          &&
                   Lockable.     Equals(SocketOutlet.Lockable)      &&
                   CableAttached.Equals(SocketOutlet.CableAttached) &&
                   CableLength.  Equals(SocketOutlet.CableLength);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Plug.         GetHashCode() * 7 ^
                       Lockable.     GetHashCode() * 5 ^
                       CableAttached.GetHashCode() * 3 ^
                       CableLength.  GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Plug,
                             Lockable               ? ", lockable "             : "",
                             CableAttached.HasValue ? ", with cable "           : "",
                             CableLength >  0       ? ", " + CableLength + "cm" : "");

        #endregion

    }

}
