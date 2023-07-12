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

namespace cloud.charging.open.protocols.WWCP.SMM
{

    /// <summary>
    /// A smart meter device.
    /// </summary>
    public class SmartMeterDevice
    {

        #region Data

        private readonly CryptoWallet cryptoWallet = new();

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this smart meter device.
        /// </summary>
        public SmartMeterDevice_Id        Id                { get; }

        /// <summary>
        /// The unique identification of the smart meter model.
        /// </summary>
        public SmartMeterModel_Id         ModelId           { get; }

        /// <summary>
        /// The unique identification of the smart meter manufacturer.
        /// </summary>
        public SmartMeterManufacturer_Id  ManufacturerId    { get; }

        /// <summary>
        /// The multi-language name of this smart meter device.
        /// </summary>
        public I18NString                 Name              { get; }

        /// <summary>
        /// The multi-language description of this smart meter device.
        /// </summary>
        public I18NString                 Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new smart meter device.
        /// </summary>
        /// <param name="Id">An unique identification of this smart meter device.</param>
        /// <param name="Name">A multi-language name of this smart meter device.</param>
        /// <param name="Description">A multi-language description of this smart meter device.</param>
        /// 
        /// <param name="Identities">An optional enumeration of cryptographic identities of this smart meter device.</param>
        public SmartMeterDevice(SmartMeterDevice_Id?         Id            = null,
                                I18NString?                  Name          = null,
                                I18NString?                  Description   = null,

                                IEnumerable<CryptoKeyInfo>?  Identities    = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given unique smart meter device identification must not be null or empty!");

            #endregion

            this.Id               = Id          ?? SmartMeterDevice_Id.NewRandom();
            this.Name             = Name        ?? I18NString.Empty;
            this.Description      = Description ?? I18NString.Empty;

            if (Identities is not null)
                foreach (var identity in Identities.Where(cryptoKey => cryptoKey.KeyUsages.Contains(CryptoKeyUsage.Identity)))
                    AddCryptoKey(CryptoKeyUsage.Identity,
                                 identity);

            unchecked
            {

                hashCode = this.Id.         GetHashCode() * 5 ^
                           this.Name.       GetHashCode() * 3 ^
                           this.Description.GetHashCode();

            }

        }

        #endregion


        #region Crypto Wallet

        public Boolean AddCryptoKey(CryptoKeyUsage  CryptoKeyUsageId,
                                    CryptoKeyInfo   CryptoKeyInfo)

            => cryptoWallet.Add(CryptoKeyUsageId,
                                         CryptoKeyInfo);

        #endregion


        #region Clone

        /// <summary>
        /// Clone this smart meter device.
        /// </summary>
        public SmartMeterDevice Clone

            => new (Id.Clone);

        #endregion


        #region Operator overloading

        #region Operator == (SmartMeterDevice1, SmartMeterDevice2)

        /// <summary>
        /// Compares two smart meter devices for equality.
        /// </summary>
        /// <param name="SmartMeterDevice1">A smart meter device.</param>
        /// <param name="SmartMeterDevice2">Another smart meter device.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SmartMeterDevice SmartMeterDevice1,
                                           SmartMeterDevice SmartMeterDevice2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SmartMeterDevice1, SmartMeterDevice2))
                return true;

            // If one is null, but not both, return false.
            if (SmartMeterDevice1 is null || SmartMeterDevice2 is null)
                return false;

            return SmartMeterDevice1.Equals(SmartMeterDevice2);

        }

        #endregion

        #region Operator != (SmartMeterDevice1, SmartMeterDevice2)

        /// <summary>
        /// Compares two smart meter devices for inequality.
        /// </summary>
        /// <param name="SmartMeterDevice1">A smart meter device.</param>
        /// <param name="SmartMeterDevice2">Another smart meter device.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SmartMeterDevice SmartMeterDevice1,
                                           SmartMeterDevice SmartMeterDevice2)

            => !(SmartMeterDevice1 == SmartMeterDevice2);

        #endregion

        #region Operator <  (SmartMeterDevice1, SmartMeterDevice2)

        /// <summary>
        /// Compares two smart meter devices.
        /// </summary>
        /// <param name="SmartMeterDevice1">A smart meter device.</param>
        /// <param name="SmartMeterDevice2">Another smart meter device.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SmartMeterDevice SmartMeterDevice1,
                                          SmartMeterDevice SmartMeterDevice2)
        {

            if (SmartMeterDevice1 is null)
                throw new ArgumentNullException(nameof(SmartMeterDevice1), "The given smart meter device 1 must not be null!");

            return SmartMeterDevice1.CompareTo(SmartMeterDevice2) < 0;

        }

        #endregion

        #region Operator <= (SmartMeterDevice1, SmartMeterDevice2)

        /// <summary>
        /// Compares two smart meter devices.
        /// </summary>
        /// <param name="SmartMeterDevice1">A smart meter device.</param>
        /// <param name="SmartMeterDevice2">Another smart meter device.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SmartMeterDevice SmartMeterDevice1,
                                           SmartMeterDevice SmartMeterDevice2)

            => !(SmartMeterDevice1 > SmartMeterDevice2);

        #endregion

        #region Operator >  (SmartMeterDevice1, SmartMeterDevice2)

        /// <summary>
        /// Compares two smart meter devices.
        /// </summary>
        /// <param name="SmartMeterDevice1">A smart meter device.</param>
        /// <param name="SmartMeterDevice2">Another smart meter device.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SmartMeterDevice SmartMeterDevice1,
                                          SmartMeterDevice SmartMeterDevice2)
        {

            if (SmartMeterDevice1 is null)
                throw new ArgumentNullException(nameof(SmartMeterDevice1), "The given smart meter device 1 must not be null!");

            return SmartMeterDevice1.CompareTo(SmartMeterDevice2) > 0;

        }

        #endregion

        #region Operator >= (SmartMeterDevice1, SmartMeterDevice2)

        /// <summary>
        /// Compares two smart meter devices.
        /// </summary>
        /// <param name="SmartMeterDevice1">A smart meter device.</param>
        /// <param name="SmartMeterDevice2">Another smart meter device.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SmartMeterDevice SmartMeterDevice1,
                                           SmartMeterDevice SmartMeterDevice2)

            => !(SmartMeterDevice1 < SmartMeterDevice2);

        #endregion

        #endregion

        #region IComparable<SmartMeterDevice> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two smart meter devices.
        /// </summary>
        /// <param name="Object">A smart meter device to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SmartMeterDevice smartMeter
                   ? CompareTo(smartMeter)
                   : throw new ArgumentException("The given object is not a smart meter device!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SmartMeterDevice)

        /// <summary>
        /// Compares two smart meter devices.
        /// </summary>
        /// <param name="SmartMeterDevice">A smart meter device to compare with.</param>
        public Int32 CompareTo(SmartMeterDevice SmartMeterDevice)
        {

            if (SmartMeterDevice is null)
                throw new ArgumentNullException(nameof(SmartMeterDevice), "The given smart meter device must not be null!");

            var c = Id.         CompareTo(SmartMeterDevice.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(SmartMeterDevice.Name);

            //if (c == 0)
            //    c = Description.CompareTo(SmartMeterDevice.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<SmartMeterDevice> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two smart meter devices for equality.
        /// </summary>
        /// <param name="Object">A smart meter device to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SmartMeterDevice smartMeter &&
                   Equals(smartMeter);

        #endregion

        #region Equals(SmartMeterDevice)

        /// <summary>
        /// Compares two smart meter devices for equality.
        /// </summary>
        /// <param name="SmartMeterDevice">A smart meter device to compare with.</param>
        public Boolean Equals(SmartMeterDevice SmartMeterDevice)

            => SmartMeterDevice is not null &&

               Id.         Equals(SmartMeterDevice.Id)   &&
               Name.       Equals(SmartMeterDevice.Name) &&
               Description.Equals(SmartMeterDevice.Description);

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
