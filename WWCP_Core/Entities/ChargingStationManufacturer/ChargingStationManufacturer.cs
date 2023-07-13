/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP.CSM
{

    /// <summary>
    /// A charging station manufacturer.
    /// </summary>
    public class ChargingStationManufacturer
    {

        #region Data

        private readonly CryptoWallet cryptoWallet = new();

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this charging station manufacturer.
        /// </summary>
        public ChargingStationManufacturer_Id  Id                { get; }

        /// <summary>
        /// The multi-language name of this charging station manufacturer.
        /// </summary>
        public I18NString                 Name              { get; }

        /// <summary>
        /// The multi-language description of this charging station manufacturer.
        /// </summary>
        public I18NString                 Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station manufacturer.
        /// </summary>
        /// <param name="Id">An unique identification of this charging station manufacturer.</param>
        /// <param name="Name">A multi-language name of this charging station manufacturer.</param>
        /// <param name="Description">A multi-language description of this charging station manufacturer.</param>
        /// 
        /// <param name="CryptoKeys">An optional enumeration of cryptographic identities of this charging station manufacturer.</param>
        public ChargingStationManufacturer(ChargingStationManufacturer_Id?   Id               = null,
                                      I18NString?                  Name             = null,
                                      I18NString?                  Description      = null,

                                      IEnumerable<CryptoKeyInfo>?  CryptoKeys       = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given unique charging station manufacturer identification must not be null or empty!");

            #endregion

            this.Id               = Id          ?? ChargingStationManufacturer_Id.NewRandom();
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
        /// Clone this charging station manufacturer.
        /// </summary>
        public ChargingStationManufacturer Clone

            => new (Id.Clone);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationManufacturer1, ChargingStationManufacturer2)

        /// <summary>
        /// Compares two charging station manufacturers for equality.
        /// </summary>
        /// <param name="ChargingStationManufacturer1">A charging station manufacturer.</param>
        /// <param name="ChargingStationManufacturer2">Another charging station manufacturer.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationManufacturer ChargingStationManufacturer1,
                                           ChargingStationManufacturer ChargingStationManufacturer2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationManufacturer1, ChargingStationManufacturer2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingStationManufacturer1 is null || ChargingStationManufacturer2 is null)
                return false;

            return ChargingStationManufacturer1.Equals(ChargingStationManufacturer2);

        }

        #endregion

        #region Operator != (ChargingStationManufacturer1, ChargingStationManufacturer2)

        /// <summary>
        /// Compares two charging station manufacturers for inequality.
        /// </summary>
        /// <param name="ChargingStationManufacturer1">A charging station manufacturer.</param>
        /// <param name="ChargingStationManufacturer2">Another charging station manufacturer.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationManufacturer ChargingStationManufacturer1,
                                           ChargingStationManufacturer ChargingStationManufacturer2)

            => !(ChargingStationManufacturer1 == ChargingStationManufacturer2);

        #endregion

        #region Operator <  (ChargingStationManufacturer1, ChargingStationManufacturer2)

        /// <summary>
        /// Compares two charging station manufacturers.
        /// </summary>
        /// <param name="ChargingStationManufacturer1">A charging station manufacturer.</param>
        /// <param name="ChargingStationManufacturer2">Another charging station manufacturer.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationManufacturer ChargingStationManufacturer1,
                                          ChargingStationManufacturer ChargingStationManufacturer2)
        {

            if (ChargingStationManufacturer1 is null)
                throw new ArgumentNullException(nameof(ChargingStationManufacturer1), "The given charging station manufacturer 1 must not be null!");

            return ChargingStationManufacturer1.CompareTo(ChargingStationManufacturer2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationManufacturer1, ChargingStationManufacturer2)

        /// <summary>
        /// Compares two charging station manufacturers.
        /// </summary>
        /// <param name="ChargingStationManufacturer1">A charging station manufacturer.</param>
        /// <param name="ChargingStationManufacturer2">Another charging station manufacturer.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationManufacturer ChargingStationManufacturer1,
                                           ChargingStationManufacturer ChargingStationManufacturer2)

            => !(ChargingStationManufacturer1 > ChargingStationManufacturer2);

        #endregion

        #region Operator >  (ChargingStationManufacturer1, ChargingStationManufacturer2)

        /// <summary>
        /// Compares two charging station manufacturers.
        /// </summary>
        /// <param name="ChargingStationManufacturer1">A charging station manufacturer.</param>
        /// <param name="ChargingStationManufacturer2">Another charging station manufacturer.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationManufacturer ChargingStationManufacturer1,
                                          ChargingStationManufacturer ChargingStationManufacturer2)
        {

            if (ChargingStationManufacturer1 is null)
                throw new ArgumentNullException(nameof(ChargingStationManufacturer1), "The given charging station manufacturer 1 must not be null!");

            return ChargingStationManufacturer1.CompareTo(ChargingStationManufacturer2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationManufacturer1, ChargingStationManufacturer2)

        /// <summary>
        /// Compares two charging station manufacturers.
        /// </summary>
        /// <param name="ChargingStationManufacturer1">A charging station manufacturer.</param>
        /// <param name="ChargingStationManufacturer2">Another charging station manufacturer.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationManufacturer ChargingStationManufacturer1,
                                           ChargingStationManufacturer ChargingStationManufacturer2)

            => !(ChargingStationManufacturer1 < ChargingStationManufacturer2);

        #endregion

        #endregion

        #region IComparable<ChargingStationManufacturer> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station manufacturers.
        /// </summary>
        /// <param name="Object">A charging station manufacturer to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationManufacturer chargingStationManufacturer
                   ? CompareTo(chargingStationManufacturer)
                   : throw new ArgumentException("The given object is not a charging station manufacturer!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationManufacturer)

        /// <summary>
        /// Compares two charging station manufacturers.
        /// </summary>
        /// <param name="ChargingStationManufacturer">A charging station manufacturer to compare with.</param>
        public Int32 CompareTo(ChargingStationManufacturer ChargingStationManufacturer)
        {

            if (ChargingStationManufacturer is null)
                throw new ArgumentNullException(nameof(ChargingStationManufacturer), "The given charging station manufacturer must not be null!");

            var c = Id.         CompareTo(ChargingStationManufacturer.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(ChargingStationManufacturer.Name);

            //if (c == 0)
            //    c = Description.CompareTo(ChargingStationManufacturer.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationManufacturer> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station manufacturers for equality.
        /// </summary>
        /// <param name="Object">A charging station manufacturer to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationManufacturer chargingStationManufacturer &&
                   Equals(chargingStationManufacturer);

        #endregion

        #region Equals(ChargingStationManufacturer)

        /// <summary>
        /// Compares two charging station manufacturers for equality.
        /// </summary>
        /// <param name="ChargingStationManufacturer">A charging station manufacturer to compare with.</param>
        public Boolean Equals(ChargingStationManufacturer ChargingStationManufacturer)

            => ChargingStationManufacturer is not null &&

               Id.         Equals(ChargingStationManufacturer.Id)   &&
               Name.       Equals(ChargingStationManufacturer.Name) &&
               Description.Equals(ChargingStationManufacturer.Description);

        #endregion

        #endregion

        #region GetHashCode()

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
