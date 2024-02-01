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

namespace cloud.charging.open.protocols.WWCP.MCL
{

    /// <summary>
    /// A metering calibration law agency.
    /// </summary>
    public class MeteringCalibrationLawAgency
    {

        #region Data

        private readonly CryptoWallet cryptoWallet = new();

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this metering calibration law agency.
        /// </summary>
        public MeteringCalibrationLawAgency_Id  Id                { get; }

        /// <summary>
        /// The multi-language name of this metering calibration law agency.
        /// </summary>
        public I18NString                       Name              { get; }

        /// <summary>
        /// The multi-language description of this metering calibration law agency.
        /// </summary>
        public I18NString                       Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new metering calibration law agency.
        /// </summary>
        /// <param name="Id">An unique identification of this metering calibration law agency.</param>
        /// <param name="Name">A multi-language name of this metering calibration law agency.</param>
        /// <param name="Description">A multi-language description of this metering calibration law agency.</param>
        /// 
        /// <param name="CryptoKeys">An optional enumeration of cryptographic identities of this metering calibration law agency.</param>
        public MeteringCalibrationLawAgency(MeteringCalibrationLawAgency_Id?  Id            = null,
                                            I18NString?                       Name          = null,
                                            I18NString?                       Description   = null,

                                            IEnumerable<CryptoKeyInfo>?       CryptoKeys    = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given unique metering calibration law agency identification must not be null or empty!");

            #endregion

            this.Id               = Id          ?? MeteringCalibrationLawAgency_Id.NewRandom();
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
        /// Clone this metering calibration law agency.
        /// </summary>
        public MeteringCalibrationLawAgency Clone

            => new (Id.Clone);

        #endregion


        #region Operator overloading

        #region Operator == (MeteringCalibrationLawAgency1, MeteringCalibrationLawAgency2)

        /// <summary>
        /// Compares two metering calibration law agencys for equality.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgency1">A metering calibration law agency.</param>
        /// <param name="MeteringCalibrationLawAgency2">Another metering calibration law agency.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (MeteringCalibrationLawAgency MeteringCalibrationLawAgency1,
                                           MeteringCalibrationLawAgency MeteringCalibrationLawAgency2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(MeteringCalibrationLawAgency1, MeteringCalibrationLawAgency2))
                return true;

            // If one is null, but not both, return false.
            if (MeteringCalibrationLawAgency1 is null || MeteringCalibrationLawAgency2 is null)
                return false;

            return MeteringCalibrationLawAgency1.Equals(MeteringCalibrationLawAgency2);

        }

        #endregion

        #region Operator != (MeteringCalibrationLawAgency1, MeteringCalibrationLawAgency2)

        /// <summary>
        /// Compares two metering calibration law agencys for inequality.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgency1">A metering calibration law agency.</param>
        /// <param name="MeteringCalibrationLawAgency2">Another metering calibration law agency.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (MeteringCalibrationLawAgency MeteringCalibrationLawAgency1,
                                           MeteringCalibrationLawAgency MeteringCalibrationLawAgency2)

            => !(MeteringCalibrationLawAgency1 == MeteringCalibrationLawAgency2);

        #endregion

        #region Operator <  (MeteringCalibrationLawAgency1, MeteringCalibrationLawAgency2)

        /// <summary>
        /// Compares two metering calibration law agencys.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgency1">A metering calibration law agency.</param>
        /// <param name="MeteringCalibrationLawAgency2">Another metering calibration law agency.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (MeteringCalibrationLawAgency MeteringCalibrationLawAgency1,
                                          MeteringCalibrationLawAgency MeteringCalibrationLawAgency2)
        {

            if (MeteringCalibrationLawAgency1 is null)
                throw new ArgumentNullException(nameof(MeteringCalibrationLawAgency1), "The given metering calibration law agency 1 must not be null!");

            return MeteringCalibrationLawAgency1.CompareTo(MeteringCalibrationLawAgency2) < 0;

        }

        #endregion

        #region Operator <= (MeteringCalibrationLawAgency1, MeteringCalibrationLawAgency2)

        /// <summary>
        /// Compares two metering calibration law agencys.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgency1">A metering calibration law agency.</param>
        /// <param name="MeteringCalibrationLawAgency2">Another metering calibration law agency.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (MeteringCalibrationLawAgency MeteringCalibrationLawAgency1,
                                           MeteringCalibrationLawAgency MeteringCalibrationLawAgency2)

            => !(MeteringCalibrationLawAgency1 > MeteringCalibrationLawAgency2);

        #endregion

        #region Operator >  (MeteringCalibrationLawAgency1, MeteringCalibrationLawAgency2)

        /// <summary>
        /// Compares two metering calibration law agencys.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgency1">A metering calibration law agency.</param>
        /// <param name="MeteringCalibrationLawAgency2">Another metering calibration law agency.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (MeteringCalibrationLawAgency MeteringCalibrationLawAgency1,
                                          MeteringCalibrationLawAgency MeteringCalibrationLawAgency2)
        {

            if (MeteringCalibrationLawAgency1 is null)
                throw new ArgumentNullException(nameof(MeteringCalibrationLawAgency1), "The given metering calibration law agency 1 must not be null!");

            return MeteringCalibrationLawAgency1.CompareTo(MeteringCalibrationLawAgency2) > 0;

        }

        #endregion

        #region Operator >= (MeteringCalibrationLawAgency1, MeteringCalibrationLawAgency2)

        /// <summary>
        /// Compares two metering calibration law agencys.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgency1">A metering calibration law agency.</param>
        /// <param name="MeteringCalibrationLawAgency2">Another metering calibration law agency.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (MeteringCalibrationLawAgency MeteringCalibrationLawAgency1,
                                           MeteringCalibrationLawAgency MeteringCalibrationLawAgency2)

            => !(MeteringCalibrationLawAgency1 < MeteringCalibrationLawAgency2);

        #endregion

        #endregion

        #region IComparable<MeteringCalibrationLawAgency> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two metering calibration law agencys.
        /// </summary>
        /// <param name="Object">A metering calibration law agency to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is MeteringCalibrationLawAgency meteringCalibrationLawAgency
                   ? CompareTo(meteringCalibrationLawAgency)
                   : throw new ArgumentException("The given object is not a metering calibration law agency!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(MeteringCalibrationLawAgency)

        /// <summary>
        /// Compares two metering calibration law agencys.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgency">A metering calibration law agency to compare with.</param>
        public Int32 CompareTo(MeteringCalibrationLawAgency MeteringCalibrationLawAgency)
        {

            if (MeteringCalibrationLawAgency is null)
                throw new ArgumentNullException(nameof(MeteringCalibrationLawAgency), "The given metering calibration law agency must not be null!");

            var c = Id.         CompareTo(MeteringCalibrationLawAgency.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(MeteringCalibrationLawAgency.Name);

            //if (c == 0)
            //    c = Description.CompareTo(MeteringCalibrationLawAgency.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<MeteringCalibrationLawAgency> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two metering calibration law agencys for equality.
        /// </summary>
        /// <param name="Object">A metering calibration law agency to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is MeteringCalibrationLawAgency meteringCalibrationLawAgency &&
                   Equals(meteringCalibrationLawAgency);

        #endregion

        #region Equals(MeteringCalibrationLawAgency)

        /// <summary>
        /// Compares two metering calibration law agencys for equality.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgency">A metering calibration law agency to compare with.</param>
        public Boolean Equals(MeteringCalibrationLawAgency MeteringCalibrationLawAgency)

            => MeteringCalibrationLawAgency is not null &&

               Id.         Equals(MeteringCalibrationLawAgency.Id)   &&
               Name.       Equals(MeteringCalibrationLawAgency.Name) &&
               Description.Equals(MeteringCalibrationLawAgency.Description);

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
