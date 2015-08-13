/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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

        #region Plug

        private readonly PlugTypes _Plug;

        /// <summary>
        /// The type of the charging plug.
        /// </summary>
        [Mandatory]
        public PlugTypes Plug
        {
            get
            {
                return _Plug;
            }
        }

        #endregion

        #region Cable

        private readonly CableType _Cable;

        /// <summary>
        /// The type of the charging cable.
        /// </summary>
        [Mandatory]
        public CableType Cable
        {
            get
            {
                return _Cable;
            }
        }

        #endregion

        #region CableLength

        private readonly Double _CableLength;

        /// <summary>
        /// The length of the charging cable [mm].
        /// </summary>
        [Mandatory]
        public Double CableLength
        {
            get
            {
                return _CableLength;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new socket outlet.
        /// </summary>
        /// <param name="Plug">The type of the charging plug.</param>
        /// <param name="Cable">The type of the charging cable.</param>
        /// <param name="CableLength">The length of the charging cable [mm].</param>
        public SocketOutlet(PlugTypes  Plug,
                            CableType  Cable        = CableType.unspecified,
                            Double     CableLength  = 0)
        {

            this._Plug         = Plug;
            this._Cable        = Cable;
            this._CableLength  = CableLength;

        }

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

            return Plug.       Equals(SocketOutlet.Plug)  &&
                   Cable.      Equals(SocketOutlet.Cable) &&
                   CableLength.Equals(SocketOutlet.CableLength);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            return String.Concat(Plug,
                                 Cable       != CableType.unspecified ? ", " + Cable.ToString()   : "",
                                 CableLength >  0                     ? ", " + CableLength + "mm" : "");

        }

        #endregion

    }

}
