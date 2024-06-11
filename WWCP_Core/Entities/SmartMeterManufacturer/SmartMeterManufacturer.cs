/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP.SMM
{

    /// <summary>
    /// A smart meter manufacturer.
    /// </summary>
    public class SmartMeterManufacturer
    {

        #region Data

        private readonly CryptoWallet cryptoWallet = new();

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this smart meter manufacturer.
        /// </summary>
        public SmartMeterManufacturer_Id  Id                { get; }

        /// <summary>
        /// The multi-language name of this smart meter manufacturer.
        /// </summary>
        public I18NString                 Name              { get; }

        /// <summary>
        /// The multi-language description of this smart meter manufacturer.
        /// </summary>
        public I18NString                 Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new smart meter manufacturer.
        /// </summary>
        /// <param name="Id">An unique identification of this smart meter manufacturer.</param>
        /// <param name="Name">A multi-language name of this smart meter manufacturer.</param>
        /// <param name="Description">A multi-language description of this smart meter manufacturer.</param>
        /// 
        /// <param name="CryptoKeys">An optional enumeration of cryptographic identities of this smart meter manufacturer.</param>
        public SmartMeterManufacturer(SmartMeterManufacturer_Id?   Id            = null,
                                      I18NString?                  Name          = null,
                                      I18NString?                  Description   = null,

                                      IEnumerable<CryptoKeyInfo>?  CryptoKeys    = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given unique smart meter manufacturer identification must not be null or empty!");

            #endregion

            this.Id               = Id          ?? SmartMeterManufacturer_Id.NewRandom();
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
        /// Clone this smart meter manufacturer.
        /// </summary>
        public SmartMeterManufacturer Clone

            => new (Id.Clone);

        #endregion


        #region Operator overloading

        #region Operator == (SmartMeterManufacturer1, SmartMeterManufacturer2)

        /// <summary>
        /// Compares two smart meter manufacturers for equality.
        /// </summary>
        /// <param name="SmartMeterManufacturer1">A smart meter manufacturer.</param>
        /// <param name="SmartMeterManufacturer2">Another smart meter manufacturer.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SmartMeterManufacturer SmartMeterManufacturer1,
                                           SmartMeterManufacturer SmartMeterManufacturer2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SmartMeterManufacturer1, SmartMeterManufacturer2))
                return true;

            // If one is null, but not both, return false.
            if (SmartMeterManufacturer1 is null || SmartMeterManufacturer2 is null)
                return false;

            return SmartMeterManufacturer1.Equals(SmartMeterManufacturer2);

        }

        #endregion

        #region Operator != (SmartMeterManufacturer1, SmartMeterManufacturer2)

        /// <summary>
        /// Compares two smart meter manufacturers for inequality.
        /// </summary>
        /// <param name="SmartMeterManufacturer1">A smart meter manufacturer.</param>
        /// <param name="SmartMeterManufacturer2">Another smart meter manufacturer.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SmartMeterManufacturer SmartMeterManufacturer1,
                                           SmartMeterManufacturer SmartMeterManufacturer2)

            => !(SmartMeterManufacturer1 == SmartMeterManufacturer2);

        #endregion

        #region Operator <  (SmartMeterManufacturer1, SmartMeterManufacturer2)

        /// <summary>
        /// Compares two smart meter manufacturers.
        /// </summary>
        /// <param name="SmartMeterManufacturer1">A smart meter manufacturer.</param>
        /// <param name="SmartMeterManufacturer2">Another smart meter manufacturer.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (SmartMeterManufacturer SmartMeterManufacturer1,
                                          SmartMeterManufacturer SmartMeterManufacturer2)
        {

            if (SmartMeterManufacturer1 is null)
                throw new ArgumentNullException(nameof(SmartMeterManufacturer1), "The given smart meter manufacturer 1 must not be null!");

            return SmartMeterManufacturer1.CompareTo(SmartMeterManufacturer2) < 0;

        }

        #endregion

        #region Operator <= (SmartMeterManufacturer1, SmartMeterManufacturer2)

        /// <summary>
        /// Compares two smart meter manufacturers.
        /// </summary>
        /// <param name="SmartMeterManufacturer1">A smart meter manufacturer.</param>
        /// <param name="SmartMeterManufacturer2">Another smart meter manufacturer.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (SmartMeterManufacturer SmartMeterManufacturer1,
                                           SmartMeterManufacturer SmartMeterManufacturer2)

            => !(SmartMeterManufacturer1 > SmartMeterManufacturer2);

        #endregion

        #region Operator >  (SmartMeterManufacturer1, SmartMeterManufacturer2)

        /// <summary>
        /// Compares two smart meter manufacturers.
        /// </summary>
        /// <param name="SmartMeterManufacturer1">A smart meter manufacturer.</param>
        /// <param name="SmartMeterManufacturer2">Another smart meter manufacturer.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (SmartMeterManufacturer SmartMeterManufacturer1,
                                          SmartMeterManufacturer SmartMeterManufacturer2)
        {

            if (SmartMeterManufacturer1 is null)
                throw new ArgumentNullException(nameof(SmartMeterManufacturer1), "The given smart meter manufacturer 1 must not be null!");

            return SmartMeterManufacturer1.CompareTo(SmartMeterManufacturer2) > 0;

        }

        #endregion

        #region Operator >= (SmartMeterManufacturer1, SmartMeterManufacturer2)

        /// <summary>
        /// Compares two smart meter manufacturers.
        /// </summary>
        /// <param name="SmartMeterManufacturer1">A smart meter manufacturer.</param>
        /// <param name="SmartMeterManufacturer2">Another smart meter manufacturer.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (SmartMeterManufacturer SmartMeterManufacturer1,
                                           SmartMeterManufacturer SmartMeterManufacturer2)

            => !(SmartMeterManufacturer1 < SmartMeterManufacturer2);

        #endregion

        #endregion

        #region IComparable<SmartMeterManufacturer> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two smart meter manufacturers.
        /// </summary>
        /// <param name="Object">A smart meter manufacturer to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SmartMeterManufacturer smartMeterManufacturer
                   ? CompareTo(smartMeterManufacturer)
                   : throw new ArgumentException("The given object is not a smart meter manufacturer!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SmartMeterManufacturer)

        /// <summary>
        /// Compares two smart meter manufacturers.
        /// </summary>
        /// <param name="SmartMeterManufacturer">A smart meter manufacturer to compare with.</param>
        public Int32 CompareTo(SmartMeterManufacturer SmartMeterManufacturer)
        {

            if (SmartMeterManufacturer is null)
                throw new ArgumentNullException(nameof(SmartMeterManufacturer), "The given smart meter manufacturer must not be null!");

            var c = Id.         CompareTo(SmartMeterManufacturer.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(SmartMeterManufacturer.Name);

            //if (c == 0)
            //    c = Description.CompareTo(SmartMeterManufacturer.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<SmartMeterManufacturer> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two smart meter manufacturers for equality.
        /// </summary>
        /// <param name="Object">A smart meter manufacturer to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SmartMeterManufacturer smartMeterManufacturer &&
                   Equals(smartMeterManufacturer);

        #endregion

        #region Equals(SmartMeterManufacturer)

        /// <summary>
        /// Compares two smart meter manufacturers for equality.
        /// </summary>
        /// <param name="SmartMeterManufacturer">A smart meter manufacturer to compare with.</param>
        public Boolean Equals(SmartMeterManufacturer SmartMeterManufacturer)

            => SmartMeterManufacturer is not null &&

               Id.         Equals(SmartMeterManufacturer.Id)   &&
               Name.       Equals(SmartMeterManufacturer.Name) &&
               Description.Equals(SmartMeterManufacturer.Description);

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
