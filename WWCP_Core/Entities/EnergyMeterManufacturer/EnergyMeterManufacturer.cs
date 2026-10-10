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

#region Usings

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

using cloud.charging.open.protocols.WWCP.POI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A energy meter manufacturer.
    /// </summary>
    public class EnergyMeterManufacturer
    {

        #region Data

        private readonly CryptoWallet cryptoWallet = new();

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this energy meter manufacturer.
        /// </summary>
        public EnergyMeterManufacturer_Id  Id                { get; }

        /// <summary>
        /// The multi-language name of this energy meter manufacturer.
        /// </summary>
        public I18NString                  Name              { get; }

        /// <summary>
        /// The multi-language description of this energy meter manufacturer.
        /// </summary>
        public I18NString                  Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy meter manufacturer.
        /// </summary>
        /// <param name="Id">An unique identification of this energy meter manufacturer.</param>
        /// <param name="Name">A multi-language name of this energy meter manufacturer.</param>
        /// <param name="Description">A multi-language description of this energy meter manufacturer.</param>
        /// 
        /// <param name="CryptoKeys">An optional enumeration of cryptographic identities of this energy meter manufacturer.</param>
        public EnergyMeterManufacturer(EnergyMeterManufacturer_Id?   Id            = null,
                                      I18NString?                  Name          = null,
                                      I18NString?                  Description   = null,

                                      IEnumerable<CryptoKeyInfo>?  CryptoKeys    = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given unique energy meter manufacturer identification must not be null or empty!");

            #endregion

            this.Id               = Id          ?? EnergyMeterManufacturer_Id.NewRandom();
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


        #region Clone()

        /// <summary>
        /// Clone this energy meter manufacturer.
        /// </summary>
        public EnergyMeterManufacturer Clone()

            => new (
                   Id.Clone()
               );

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeterManufacturer1, EnergyMeterManufacturer2)

        /// <summary>
        /// Compares two energy meter manufacturers for equality.
        /// </summary>
        /// <param name="EnergyMeterManufacturer1">A energy meter manufacturer.</param>
        /// <param name="EnergyMeterManufacturer2">Another energy meter manufacturer.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EnergyMeterManufacturer EnergyMeterManufacturer1,
                                           EnergyMeterManufacturer EnergyMeterManufacturer2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EnergyMeterManufacturer1, EnergyMeterManufacturer2))
                return true;

            // If one is null, but not both, return false.
            if (EnergyMeterManufacturer1 is null || EnergyMeterManufacturer2 is null)
                return false;

            return EnergyMeterManufacturer1.Equals(EnergyMeterManufacturer2);

        }

        #endregion

        #region Operator != (EnergyMeterManufacturer1, EnergyMeterManufacturer2)

        /// <summary>
        /// Compares two energy meter manufacturers for inequality.
        /// </summary>
        /// <param name="EnergyMeterManufacturer1">A energy meter manufacturer.</param>
        /// <param name="EnergyMeterManufacturer2">Another energy meter manufacturer.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EnergyMeterManufacturer EnergyMeterManufacturer1,
                                           EnergyMeterManufacturer EnergyMeterManufacturer2)

            => !(EnergyMeterManufacturer1 == EnergyMeterManufacturer2);

        #endregion

        #region Operator <  (EnergyMeterManufacturer1, EnergyMeterManufacturer2)

        /// <summary>
        /// Compares two energy meter manufacturers.
        /// </summary>
        /// <param name="EnergyMeterManufacturer1">A energy meter manufacturer.</param>
        /// <param name="EnergyMeterManufacturer2">Another energy meter manufacturer.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EnergyMeterManufacturer EnergyMeterManufacturer1,
                                          EnergyMeterManufacturer EnergyMeterManufacturer2)
        {

            if (EnergyMeterManufacturer1 is null)
                throw new ArgumentNullException(nameof(EnergyMeterManufacturer1), "The given energy meter manufacturer 1 must not be null!");

            return EnergyMeterManufacturer1.CompareTo(EnergyMeterManufacturer2) < 0;

        }

        #endregion

        #region Operator <= (EnergyMeterManufacturer1, EnergyMeterManufacturer2)

        /// <summary>
        /// Compares two energy meter manufacturers.
        /// </summary>
        /// <param name="EnergyMeterManufacturer1">A energy meter manufacturer.</param>
        /// <param name="EnergyMeterManufacturer2">Another energy meter manufacturer.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EnergyMeterManufacturer EnergyMeterManufacturer1,
                                           EnergyMeterManufacturer EnergyMeterManufacturer2)

            => !(EnergyMeterManufacturer1 > EnergyMeterManufacturer2);

        #endregion

        #region Operator >  (EnergyMeterManufacturer1, EnergyMeterManufacturer2)

        /// <summary>
        /// Compares two energy meter manufacturers.
        /// </summary>
        /// <param name="EnergyMeterManufacturer1">A energy meter manufacturer.</param>
        /// <param name="EnergyMeterManufacturer2">Another energy meter manufacturer.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EnergyMeterManufacturer EnergyMeterManufacturer1,
                                          EnergyMeterManufacturer EnergyMeterManufacturer2)
        {

            if (EnergyMeterManufacturer1 is null)
                throw new ArgumentNullException(nameof(EnergyMeterManufacturer1), "The given energy meter manufacturer 1 must not be null!");

            return EnergyMeterManufacturer1.CompareTo(EnergyMeterManufacturer2) > 0;

        }

        #endregion

        #region Operator >= (EnergyMeterManufacturer1, EnergyMeterManufacturer2)

        /// <summary>
        /// Compares two energy meter manufacturers.
        /// </summary>
        /// <param name="EnergyMeterManufacturer1">A energy meter manufacturer.</param>
        /// <param name="EnergyMeterManufacturer2">Another energy meter manufacturer.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EnergyMeterManufacturer EnergyMeterManufacturer1,
                                           EnergyMeterManufacturer EnergyMeterManufacturer2)

            => !(EnergyMeterManufacturer1 < EnergyMeterManufacturer2);

        #endregion

        #endregion

        #region IComparable<EnergyMeterManufacturer> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two energy meter manufacturers.
        /// </summary>
        /// <param name="Object">A energy meter manufacturer to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergyMeterManufacturer smartMeterManufacturer
                   ? CompareTo(smartMeterManufacturer)
                   : throw new ArgumentException("The given object is not a energy meter manufacturer!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergyMeterManufacturer)

        /// <summary>
        /// Compares two energy meter manufacturers.
        /// </summary>
        /// <param name="EnergyMeterManufacturer">A energy meter manufacturer to compare with.</param>
        public Int32 CompareTo(EnergyMeterManufacturer EnergyMeterManufacturer)
        {

            if (EnergyMeterManufacturer is null)
                throw new ArgumentNullException(nameof(EnergyMeterManufacturer), "The given energy meter manufacturer must not be null!");

            var c = Id.         CompareTo(EnergyMeterManufacturer.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(EnergyMeterManufacturer.Name);

            //if (c == 0)
            //    c = Description.CompareTo(EnergyMeterManufacturer.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EnergyMeterManufacturer> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy meter manufacturers for equality.
        /// </summary>
        /// <param name="Object">A energy meter manufacturer to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMeterManufacturer smartMeterManufacturer &&
                   Equals(smartMeterManufacturer);

        #endregion

        #region Equals(EnergyMeterManufacturer)

        /// <summary>
        /// Compares two energy meter manufacturers for equality.
        /// </summary>
        /// <param name="EnergyMeterManufacturer">A energy meter manufacturer to compare with.</param>
        public Boolean Equals(EnergyMeterManufacturer EnergyMeterManufacturer)

            => EnergyMeterManufacturer is not null &&

               Id.         Equals(EnergyMeterManufacturer.Id)   &&
               Name.       Equals(EnergyMeterManufacturer.Name) &&
               Description.Equals(EnergyMeterManufacturer.Description);

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
