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

namespace cloud.charging.open.protocols.WWCP.CSM
{

    /// <summary>
    /// A charging station.
    /// </summary>
    public class ChargingStationDevice
    {

        #region Data

        private readonly CryptoWallet cryptoWallet = new();

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this charging station.
        /// </summary>
        public ChargingStationDevice_Id        Id                { get; }

        /// <summary>
        /// The unique identification of the charging station model.
        /// </summary>
        public ChargingStationModel_Id         ModelId           { get; }

        /// <summary>
        /// The unique identification of the charging station manufacturer.
        /// </summary>
        public ChargingStationManufacturer_Id  ManufacturerId    { get; }

        /// <summary>
        /// The multi-language name of this charging station.
        /// </summary>
        public I18NString                      Name              { get; }

        /// <summary>
        /// The multi-language description of this charging station.
        /// </summary>
        public I18NString                      Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station.
        /// </summary>
        /// <param name="Id">An unique identification of this charging station.</param>
        /// <param name="Name">A multi-language name of this charging station.</param>
        /// <param name="Description">A multi-language description of this charging station.</param>
        /// 
        /// <param name="Identities">An optional enumeration of cryptographic identities of this charging station.</param>
        public ChargingStationDevice(ChargingStationDevice_Id?    Id            = null,
                                     I18NString?                  Name          = null,
                                     I18NString?                  Description   = null,

                                     IEnumerable<CryptoKeyInfo>?  Identities    = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given unique charging station identification must not be null or empty!");

            #endregion

            this.Id               = Id          ?? ChargingStationDevice_Id.NewRandom();
            this.Name             = Name        ?? I18NString.Empty;
            this.Description      = Description ?? I18NString.Empty;

            if (Identities is not null)
                foreach (var identity in Identities)
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
        /// Clone this charging station.
        /// </summary>
        public ChargingStationDevice Clone

            => new (Id.Clone);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two charging stations for equality.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationDevice ChargingStation1,
                                           ChargingStationDevice ChargingStation2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStation1, ChargingStation2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingStation1 is null || ChargingStation2 is null)
                return false;

            return ChargingStation1.Equals(ChargingStation2);

        }

        #endregion

        #region Operator != (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two charging stations for inequality.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationDevice ChargingStation1,
                                           ChargingStationDevice ChargingStation2)

            => !(ChargingStation1 == ChargingStation2);

        #endregion

        #region Operator <  (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two charging stations.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationDevice ChargingStation1,
                                          ChargingStationDevice ChargingStation2)
        {

            if (ChargingStation1 is null)
                throw new ArgumentNullException(nameof(ChargingStation1), "The given charging station 1 must not be null!");

            return ChargingStation1.CompareTo(ChargingStation2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two charging stations.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationDevice ChargingStation1,
                                           ChargingStationDevice ChargingStation2)

            => !(ChargingStation1 > ChargingStation2);

        #endregion

        #region Operator >  (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two charging stations.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationDevice ChargingStation1,
                                          ChargingStationDevice ChargingStation2)
        {

            if (ChargingStation1 is null)
                throw new ArgumentNullException(nameof(ChargingStation1), "The given charging station 1 must not be null!");

            return ChargingStation1.CompareTo(ChargingStation2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStation1, ChargingStation2)

        /// <summary>
        /// Compares two charging stations.
        /// </summary>
        /// <param name="ChargingStation1">A charging station.</param>
        /// <param name="ChargingStation2">Another charging station.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationDevice ChargingStation1,
                                           ChargingStationDevice ChargingStation2)

            => !(ChargingStation1 < ChargingStation2);

        #endregion

        #endregion

        #region IComparable<ChargingStation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging stations.
        /// </summary>
        /// <param name="Object">A charging station to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationDevice chargingStation
                   ? CompareTo(chargingStation)
                   : throw new ArgumentException("The given object is not a charging station!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStation)

        /// <summary>
        /// Compares two charging stations.
        /// </summary>
        /// <param name="ChargingStation">A charging station to compare with.</param>
        public Int32 CompareTo(ChargingStationDevice ChargingStation)
        {

            if (ChargingStation is null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            var c = Id.         CompareTo(ChargingStation.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(ChargingStation.Name);

            //if (c == 0)
            //    c = Description.CompareTo(ChargingStation.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStation> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging stations for equality.
        /// </summary>
        /// <param name="Object">A charging station to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationDevice chargingStation &&
                   Equals(chargingStation);

        #endregion

        #region Equals(ChargingStation)

        /// <summary>
        /// Compares two charging stations for equality.
        /// </summary>
        /// <param name="ChargingStation">A charging station to compare with.</param>
        public Boolean Equals(ChargingStationDevice ChargingStation)

            => ChargingStation is not null &&

               Id.         Equals(ChargingStation.Id)   &&
               Name.       Equals(ChargingStation.Name) &&
               Description.Equals(ChargingStation.Description);

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
