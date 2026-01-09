/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An energy information.
    /// </summary>
    public readonly struct EnergyInfo : IEquatable<EnergyInfo>,
                                        IComparable<EnergyInfo>,
                                        IComparable
    {

        #region Properties

        /// <summary>
        /// The energy used.
        /// </summary>
        public Double  Used         { get; }

        /// <summary>
        /// The energy still available.
        /// </summary>
        public Double  Available    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy information.
        /// </summary>
        /// <param name="Used">The energy used.</param>
        /// <param name="Available">The energy still available.</param>
        public EnergyInfo(Double  Used,
                          Double  Available)

        {

            this.Used       = Used;
            this.Available  = Available;

        }

        #endregion


        #region Operator overloading

        #region Operator == (EnergyInfo1, EnergyInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyInfo1">An energy information.</param>
        /// <param name="EnergyInfo2">Another energy information.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EnergyInfo EnergyInfo1,
                                           EnergyInfo EnergyInfo2)

            => EnergyInfo1.Equals(EnergyInfo2);

        #endregion

        #region Operator != (EnergyInfo1, EnergyInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyInfo1">An energy information.</param>
        /// <param name="EnergyInfo2">Another energy information.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EnergyInfo EnergyInfo1,
                                           EnergyInfo EnergyInfo2)

            => !EnergyInfo1.Equals(EnergyInfo2);

        #endregion

        #region Operator <  (EnergyInfo1, EnergyInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyInfo1">An energy information.</param>
        /// <param name="EnergyInfo2">Another energy information.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EnergyInfo EnergyInfo1,
                                          EnergyInfo EnergyInfo2)

            => EnergyInfo1.CompareTo(EnergyInfo2) < 0;

        #endregion

        #region Operator <= (EnergyInfo1, EnergyInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyInfo1">An energy information.</param>
        /// <param name="EnergyInfo2">Another energy information.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EnergyInfo EnergyInfo1,
                                           EnergyInfo EnergyInfo2)

            => EnergyInfo1.CompareTo(EnergyInfo2) <= 0;

        #endregion

        #region Operator >  (EnergyInfo1, EnergyInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyInfo1">An energy information.</param>
        /// <param name="EnergyInfo2">Another energy information.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EnergyInfo EnergyInfo1,
                                          EnergyInfo EnergyInfo2)

            => EnergyInfo1.CompareTo(EnergyInfo2) > 0;

        #endregion

        #region Operator >= (EnergyInfo1, EnergyInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyInfo1">An energy information.</param>
        /// <param name="EnergyInfo2">Another energy information.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EnergyInfo EnergyInfo1,
                                           EnergyInfo EnergyInfo2)

            => EnergyInfo1.CompareTo(EnergyInfo2) >= 0;

        #endregion

        #endregion

        #region IComparable<EnergyInfo> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two energy information.
        /// </summary>
        /// <param name="Object">An energy information to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergyInfo energyInfo
                   ? CompareTo(energyInfo)
                   : throw new ArgumentException("The given object is not an energy information!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergyInfo)

        /// <summary>
        /// Compares two energy information.
        /// </summary>
        /// <param name="EnergyInfo">An energy information to compare with.</param>
        public Int32 CompareTo(EnergyInfo EnergyInfo)
        {

            var c = Used.     CompareTo(EnergyInfo.Used);

            if (c == 0)
                c = Available.CompareTo(EnergyInfo.Available);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EnergyInfo> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy information for equality.
        /// </summary>
        /// <param name="Object">An energy information to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergyInfo energyInfo &&
                   Equals(energyInfo);

        #endregion

        #region Equals(EnergyInfo)

        /// <summary>
        /// Compares two energy information for equality.
        /// </summary>
        /// <param name="EnergyInfo">An energy information to compare with.</param>
        public Boolean Equals(EnergyInfo EnergyInfo)

            => Used.     Equals(EnergyInfo.Used) &&
               Available.Equals(EnergyInfo.Available);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Used.     GetHashCode() * 3 ^
                       Available.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Used} used, {Available} available";

        #endregion

    }

}
