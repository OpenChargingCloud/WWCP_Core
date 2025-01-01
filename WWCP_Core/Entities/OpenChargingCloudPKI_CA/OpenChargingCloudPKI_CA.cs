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

namespace cloud.charging.open.protocols.WWCP.PKI
{

    /// <summary>
    /// The Open Charging Cloud Public Key Infrastructure Certification Authority.
    /// </summary>
    public class OpenChargingCloudPKI_CA
    {

        #region Data

        private readonly CryptoWallet cryptoWallet = new();

        #endregion

        #region Properties

        /// <summary>
        /// The multi-language name of this public key infrastructure certification authority.
        /// </summary>
        public I18NString  Name              { get; }

        /// <summary>
        /// The multi-language description of this public key infrastructure certification authority.
        /// </summary>
        public I18NString  Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Open Charging Cloud Public Key Infrastructure Certification Authority.
        /// </summary>
        /// <param name="Name">A multi-language name of this public key infrastructure certification authority.</param>
        /// <param name="Description">A multi-language description of this public key infrastructure certification authority.</param>
        /// 
        /// <param name="CryptoKeys">An optional enumeration of cryptographic identities of this public key infrastructure certification authority.</param>
        public OpenChargingCloudPKI_CA(I18NString?                  Name          = null,
                                       I18NString?                  Description   = null,

                                       IEnumerable<CryptoKeyInfo>?  CryptoKeys    = null)
        {

            this.Name         = Name        ?? I18NString.Empty;
            this.Description  = Description ?? I18NString.Empty;

            if (CryptoKeys is not null && CryptoKeys.Any())
                AddCryptoKeys(CryptoKeys);

            unchecked
            {

                hashCode = this.Name.       GetHashCode() * 3 ^
                           this.Description.GetHashCode();

            }

        }

        #endregion


        #region Crypto Wallet

        public Boolean AddCryptoKey(CryptoKeyInfo CryptoKeyInfo)

            => cryptoWallet.Add(CryptoKeyInfo);

        public Boolean AddCryptoKeys(IEnumerable<CryptoKeyInfo> CryptoKeyInfos)

            => cryptoWallet.Add(CryptoKeyInfos);

        public CryptoKeyInfo SignKey(CryptoKeyInfo CryptoKeyInfo)

            => cryptoWallet.SignKey(CryptoKeyInfo,
                                    cryptoWallet.Where(key => key.KeyUsages.Contains(CryptoKeyUsage.Identity)).ToArray());

        #endregion


        #region Clone

        /// <summary>
        /// Clone this public key infrastructure certification authority.
        /// </summary>
        public OpenChargingCloudPKI_CA Clone

            => new (
                   Name.        Clone(),
                   Description. Clone(),
                   cryptoWallet.Clone()
               );

        #endregion


        #region Operator overloading

        #region Operator == (OpenChargingCloudPKI_CA1, OpenChargingCloudPKI_CA2)

        /// <summary>
        /// Compares two public key infrastructure certification authoritys for equality.
        /// </summary>
        /// <param name="OpenChargingCloudPKI_CA1">A public key infrastructure certification authority.</param>
        /// <param name="OpenChargingCloudPKI_CA2">Another public key infrastructure certification authority.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA1,
                                           OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(OpenChargingCloudPKI_CA1, OpenChargingCloudPKI_CA2))
                return true;

            // If one is null, but not both, return false.
            if (OpenChargingCloudPKI_CA1 is null || OpenChargingCloudPKI_CA2 is null)
                return false;

            return OpenChargingCloudPKI_CA1.Equals(OpenChargingCloudPKI_CA2);

        }

        #endregion

        #region Operator != (OpenChargingCloudPKI_CA1, OpenChargingCloudPKI_CA2)

        /// <summary>
        /// Compares two public key infrastructure certification authoritys for inequality.
        /// </summary>
        /// <param name="OpenChargingCloudPKI_CA1">A public key infrastructure certification authority.</param>
        /// <param name="OpenChargingCloudPKI_CA2">Another public key infrastructure certification authority.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA1,
                                           OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA2)

            => !(OpenChargingCloudPKI_CA1 == OpenChargingCloudPKI_CA2);

        #endregion

        #region Operator <  (OpenChargingCloudPKI_CA1, OpenChargingCloudPKI_CA2)

        /// <summary>
        /// Compares two public key infrastructure certification authoritys.
        /// </summary>
        /// <param name="OpenChargingCloudPKI_CA1">A public key infrastructure certification authority.</param>
        /// <param name="OpenChargingCloudPKI_CA2">Another public key infrastructure certification authority.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA1,
                                          OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA2)
        {

            if (OpenChargingCloudPKI_CA1 is null)
                throw new ArgumentNullException(nameof(OpenChargingCloudPKI_CA1), "The given public key infrastructure certification authority 1 must not be null!");

            return OpenChargingCloudPKI_CA1.CompareTo(OpenChargingCloudPKI_CA2) < 0;

        }

        #endregion

        #region Operator <= (OpenChargingCloudPKI_CA1, OpenChargingCloudPKI_CA2)

        /// <summary>
        /// Compares two public key infrastructure certification authoritys.
        /// </summary>
        /// <param name="OpenChargingCloudPKI_CA1">A public key infrastructure certification authority.</param>
        /// <param name="OpenChargingCloudPKI_CA2">Another public key infrastructure certification authority.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA1,
                                           OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA2)

            => !(OpenChargingCloudPKI_CA1 > OpenChargingCloudPKI_CA2);

        #endregion

        #region Operator >  (OpenChargingCloudPKI_CA1, OpenChargingCloudPKI_CA2)

        /// <summary>
        /// Compares two public key infrastructure certification authoritys.
        /// </summary>
        /// <param name="OpenChargingCloudPKI_CA1">A public key infrastructure certification authority.</param>
        /// <param name="OpenChargingCloudPKI_CA2">Another public key infrastructure certification authority.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA1,
                                          OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA2)
        {

            if (OpenChargingCloudPKI_CA1 is null)
                throw new ArgumentNullException(nameof(OpenChargingCloudPKI_CA1), "The given public key infrastructure certification authority 1 must not be null!");

            return OpenChargingCloudPKI_CA1.CompareTo(OpenChargingCloudPKI_CA2) > 0;

        }

        #endregion

        #region Operator >= (OpenChargingCloudPKI_CA1, OpenChargingCloudPKI_CA2)

        /// <summary>
        /// Compares two public key infrastructure certification authoritys.
        /// </summary>
        /// <param name="OpenChargingCloudPKI_CA1">A public key infrastructure certification authority.</param>
        /// <param name="OpenChargingCloudPKI_CA2">Another public key infrastructure certification authority.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA1,
                                           OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA2)

            => !(OpenChargingCloudPKI_CA1 < OpenChargingCloudPKI_CA2);

        #endregion

        #endregion

        #region IComparable<OpenChargingCloudPKI_CA> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two public key infrastructure certification authoritys.
        /// </summary>
        /// <param name="Object">A public key infrastructure certification authority to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is OpenChargingCloudPKI_CA smartMeterManufacturer
                   ? CompareTo(smartMeterManufacturer)
                   : throw new ArgumentException("The given object is not a public key infrastructure certification authority!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(OpenChargingCloudPKI_CA)

        /// <summary>
        /// Compares two public key infrastructure certification authoritys.
        /// </summary>
        /// <param name="OpenChargingCloudPKI_CA">A public key infrastructure certification authority to compare with.</param>
        public Int32 CompareTo(OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA)
        {

            if (OpenChargingCloudPKI_CA is null)
                throw new ArgumentNullException(nameof(OpenChargingCloudPKI_CA), "The given public key infrastructure certification authority must not be null!");

            var c = 0;

            //if (c == 0)
            //    c = Name.       CompareTo(OpenChargingCloudPKI_CA.Name);

            //if (c == 0)
            //    c = Description.CompareTo(OpenChargingCloudPKI_CA.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<OpenChargingCloudPKI_CA> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two public key infrastructure certification authoritys for equality.
        /// </summary>
        /// <param name="Object">A public key infrastructure certification authority to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is OpenChargingCloudPKI_CA smartMeterManufacturer &&
                   Equals(smartMeterManufacturer);

        #endregion

        #region Equals(OpenChargingCloudPKI_CA)

        /// <summary>
        /// Compares two public key infrastructure certification authoritys for equality.
        /// </summary>
        /// <param name="OpenChargingCloudPKI_CA">A public key infrastructure certification authority to compare with.</param>
        public Boolean Equals(OpenChargingCloudPKI_CA OpenChargingCloudPKI_CA)

            => OpenChargingCloudPKI_CA is not null &&

               Name.       Equals(OpenChargingCloudPKI_CA.Name) &&
               Description.Equals(OpenChargingCloudPKI_CA.Description);

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

            => Name.FirstText();

        #endregion

    }

}
