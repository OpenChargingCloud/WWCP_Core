/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP.MCL
{

    /// <summary>
    /// A national contact point.
    /// </summary>
    public class NationalContactPoint
    {

        #region Data

        private readonly CryptoWallet cryptoWallet = new();

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this national contact point.
        /// </summary>
        public NationalContactPoint_Id  Id                { get; }

        /// <summary>
        /// The multi-language name of this national contact point.
        /// </summary>
        public I18NString                       Name              { get; }

        /// <summary>
        /// The multi-language description of this national contact point.
        /// </summary>
        public I18NString                       Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new national contact point.
        /// </summary>
        /// <param name="Id">An unique identification of this national contact point.</param>
        /// <param name="Name">A multi-language name of this national contact point.</param>
        /// <param name="Description">A multi-language description of this national contact point.</param>
        /// 
        /// <param name="CryptoKeys">An optional enumeration of cryptographic identities of this national contact point.</param>
        public NationalContactPoint(NationalContactPoint_Id?  Id            = null,
                                            I18NString?                       Name          = null,
                                            I18NString?                       Description   = null,

                                            IEnumerable<CryptoKeyInfo>?       CryptoKeys    = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given unique national contact point identification must not be null or empty!");

            #endregion

            this.Id               = Id          ?? NationalContactPoint_Id.NewRandom();
            this.Name             = Name        ?? I18NString.Empty;
            this.Description      = Description ?? I18NString.Empty;

            if (CryptoKeys is not null)
                foreach (var identity in CryptoKeys)
                    AddCryptoKey(identity);

            unchecked
            {

                hashCode = this.Id.         GetHashCode() * 5 ^
                           this.Name.       GetHashCode() * 3 ^
                           this.Description.GetHashCode();

            }

        }

        #endregion


        #region Crypto Wallet

        public Boolean AddCryptoKey(CryptoKeyInfo CryptoKeyInfo)

            => cryptoWallet.Add(CryptoKeyInfo);

        #endregion


        #region Clone

        /// <summary>
        /// Clone this national contact point.
        /// </summary>
        public NationalContactPoint Clone

            => new (Id.Clone);

        #endregion


        #region Operator overloading

        #region Operator == (NationalContactPoint1, NationalContactPoint2)

        /// <summary>
        /// Compares two national contact points for equality.
        /// </summary>
        /// <param name="NationalContactPoint1">A national contact point.</param>
        /// <param name="NationalContactPoint2">Another national contact point.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (NationalContactPoint NationalContactPoint1,
                                           NationalContactPoint NationalContactPoint2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(NationalContactPoint1, NationalContactPoint2))
                return true;

            // If one is null, but not both, return false.
            if (NationalContactPoint1 is null || NationalContactPoint2 is null)
                return false;

            return NationalContactPoint1.Equals(NationalContactPoint2);

        }

        #endregion

        #region Operator != (NationalContactPoint1, NationalContactPoint2)

        /// <summary>
        /// Compares two national contact points for inequality.
        /// </summary>
        /// <param name="NationalContactPoint1">A national contact point.</param>
        /// <param name="NationalContactPoint2">Another national contact point.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (NationalContactPoint NationalContactPoint1,
                                           NationalContactPoint NationalContactPoint2)

            => !(NationalContactPoint1 == NationalContactPoint2);

        #endregion

        #region Operator <  (NationalContactPoint1, NationalContactPoint2)

        /// <summary>
        /// Compares two national contact points.
        /// </summary>
        /// <param name="NationalContactPoint1">A national contact point.</param>
        /// <param name="NationalContactPoint2">Another national contact point.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (NationalContactPoint NationalContactPoint1,
                                          NationalContactPoint NationalContactPoint2)
        {

            if (NationalContactPoint1 is null)
                throw new ArgumentNullException(nameof(NationalContactPoint1), "The given national contact point 1 must not be null!");

            return NationalContactPoint1.CompareTo(NationalContactPoint2) < 0;

        }

        #endregion

        #region Operator <= (NationalContactPoint1, NationalContactPoint2)

        /// <summary>
        /// Compares two national contact points.
        /// </summary>
        /// <param name="NationalContactPoint1">A national contact point.</param>
        /// <param name="NationalContactPoint2">Another national contact point.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (NationalContactPoint NationalContactPoint1,
                                           NationalContactPoint NationalContactPoint2)

            => !(NationalContactPoint1 > NationalContactPoint2);

        #endregion

        #region Operator >  (NationalContactPoint1, NationalContactPoint2)

        /// <summary>
        /// Compares two national contact points.
        /// </summary>
        /// <param name="NationalContactPoint1">A national contact point.</param>
        /// <param name="NationalContactPoint2">Another national contact point.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (NationalContactPoint NationalContactPoint1,
                                          NationalContactPoint NationalContactPoint2)
        {

            if (NationalContactPoint1 is null)
                throw new ArgumentNullException(nameof(NationalContactPoint1), "The given national contact point 1 must not be null!");

            return NationalContactPoint1.CompareTo(NationalContactPoint2) > 0;

        }

        #endregion

        #region Operator >= (NationalContactPoint1, NationalContactPoint2)

        /// <summary>
        /// Compares two national contact points.
        /// </summary>
        /// <param name="NationalContactPoint1">A national contact point.</param>
        /// <param name="NationalContactPoint2">Another national contact point.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (NationalContactPoint NationalContactPoint1,
                                           NationalContactPoint NationalContactPoint2)

            => !(NationalContactPoint1 < NationalContactPoint2);

        #endregion

        #endregion

        #region IComparable<NationalContactPoint> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two national contact points.
        /// </summary>
        /// <param name="Object">A national contact point to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is NationalContactPoint nationalContactPoint
                   ? CompareTo(nationalContactPoint)
                   : throw new ArgumentException("The given object is not a national contact point!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(NationalContactPoint)

        /// <summary>
        /// Compares two national contact points.
        /// </summary>
        /// <param name="NationalContactPoint">A national contact point to compare with.</param>
        public Int32 CompareTo(NationalContactPoint NationalContactPoint)
        {

            if (NationalContactPoint is null)
                throw new ArgumentNullException(nameof(NationalContactPoint), "The given national contact point must not be null!");

            var c = Id.         CompareTo(NationalContactPoint.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(NationalContactPoint.Name);

            //if (c == 0)
            //    c = Description.CompareTo(NationalContactPoint.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<NationalContactPoint> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two national contact points for equality.
        /// </summary>
        /// <param name="Object">A national contact point to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is NationalContactPoint nationalContactPoint &&
                   Equals(nationalContactPoint);

        #endregion

        #region Equals(NationalContactPoint)

        /// <summary>
        /// Compares two national contact points for equality.
        /// </summary>
        /// <param name="NationalContactPoint">A national contact point to compare with.</param>
        public Boolean Equals(NationalContactPoint NationalContactPoint)

            => NationalContactPoint is not null &&

               Id.         Equals(NationalContactPoint.Id)   &&
               Name.       Equals(NationalContactPoint.Name) &&
               Description.Equals(NationalContactPoint.Description);

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Id.ToString();

        #endregion

    }

}
