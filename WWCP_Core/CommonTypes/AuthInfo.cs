/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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
    /// Some sort of token for authentication.
    /// </summary>
    public class AuthInfo : IEquatable<AuthInfo>,
                            IComparable<AuthInfo>
    {

        #region Properties

        /// <summary>
        /// An authentication token, e.g. the identification of a RFID card.
        /// </summary>
        public Auth_Token            AuthToken                     { get; }

        /// <summary>
        /// A e-mobility account identification and its password/PIN.
        /// Used within OICP.
        /// </summary>
        public eMAIdWithPIN2         QRCodeIdentification          { get; }

        /// <summary>
        /// A e-mobility account identification transmitted via PnC.
        /// </summary>
        public eMobilityAccount_Id?  PlugAndChargeIdentification   { get; }

        /// <summary>
        /// A e-mobility account identification.
        /// </summary>
        public eMobilityAccount_Id?  RemoteIdentification          { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthInfo(AuthToken)

        private AuthInfo(Auth_Token  AuthToken)
        {
            this.AuthToken = AuthToken;
        }

        #endregion

        #region (private) AuthInfo(QRCodeIdentification)

        private AuthInfo(eMAIdWithPIN2 QRCodeIdentification)
        {
            this.QRCodeIdentification = QRCodeIdentification;
        }

        #endregion

        #region (private) AuthInfo(PlugAndChargeIdentification, IsPnC)

        private AuthInfo(eMobilityAccount_Id  PlugAndChargeIdentification,
                         Boolean              IsPnC)
        {
            this.PlugAndChargeIdentification  = PlugAndChargeIdentification;
        }

        #endregion

        #region (private) AuthInfo(RemoteIdentification)

        private AuthInfo(eMobilityAccount_Id RemoteIdentification)
        {
            this.RemoteIdentification = RemoteIdentification;
        }

        #endregion

        #endregion


        #region (static) FromAuthToken(AuthToken)

        /// <summary>
        /// Create a new authentication info based on the given authentication token.
        /// </summary>
        /// <param name="AuthToken">An authentication token.</param>
        public static AuthInfo FromAuthToken(Auth_Token AuthToken)

            => new AuthInfo(AuthToken);

        #endregion

        #region (static) FromQRCodeIdentification(eMAId, PIN)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="eMAId">An e-mobility account identification.</param>
        /// <param name="PIN">A password/PIN.</param>
        public static AuthInfo FromQRCodeIdentification(eMobilityAccount_Id  eMAId,
                                                        String               PIN)

            => new AuthInfo(new eMAIdWithPIN2(eMAId, PIN));

        #endregion

        #region (static) FromQRCodeIdentification(QRCodeIdentification)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR code identification.</param>
        public static AuthInfo FromQRCodeIdentification(eMAIdWithPIN2 QRCodeIdentification)

            => new AuthInfo(QRCodeIdentification);

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification transmitted via PnC.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A PnC e-mobility account identification.</param>
        public static AuthInfo FromPlugAndChargeIdentification(eMobilityAccount_Id PlugAndChargeIdentification)

            => new AuthInfo(PlugAndChargeIdentification);

        #endregion

        #region (static) FromRemoteIdentification(RemoteIdentification)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        public static AuthInfo FromRemoteIdentification(eMobilityAccount_Id RemoteIdentification)

            => new AuthInfo(RemoteIdentification);

        #endregion


        #region Operator overloading

        #region Operator == (AuthInfo1, AuthInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfo1">A AuthInfo.</param>
        /// <param name="AuthInfo2">Another AuthInfo.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (AuthInfo AuthInfo1, AuthInfo AuthInfo2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AuthInfo1, AuthInfo2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthInfo1 == null) || ((Object) AuthInfo2 == null))
                return false;

            return AuthInfo1.Equals(AuthInfo2);

        }

        #endregion

        #region Operator != (AuthInfo1, AuthInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfo1">A AuthInfo.</param>
        /// <param name="AuthInfo2">Another AuthInfo.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (AuthInfo AuthInfo1, AuthInfo AuthInfo2)
        {
            return !(AuthInfo1 == AuthInfo2);
        }

        #endregion

        #region Operator <  (AuthInfo1, AuthInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfo1">A AuthInfo.</param>
        /// <param name="AuthInfo2">Another AuthInfo.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (AuthInfo AuthInfo1, AuthInfo AuthInfo2)
        {

            if ((Object) AuthInfo1 == null)
                throw new ArgumentNullException("The given AuthInfo1 must not be null!");

            return AuthInfo1.CompareTo(AuthInfo2) < 0;

        }

        #endregion

        #region Operator <= (AuthInfo1, AuthInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfo1">A AuthInfo.</param>
        /// <param name="AuthInfo2">Another AuthInfo.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (AuthInfo AuthInfo1, AuthInfo AuthInfo2)
        {
            return !(AuthInfo1 > AuthInfo2);
        }

        #endregion

        #region Operator >  (AuthInfo1, AuthInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfo1">A AuthInfo.</param>
        /// <param name="AuthInfo2">Another AuthInfo.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (AuthInfo AuthInfo1, AuthInfo AuthInfo2)
        {

            if ((Object) AuthInfo1 == null)
                throw new ArgumentNullException("The given AuthInfo1 must not be null!");

            return AuthInfo1.CompareTo(AuthInfo2) > 0;

        }

        #endregion

        #region Operator >= (AuthInfo1, AuthInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfo1">A AuthInfo.</param>
        /// <param name="AuthInfo2">Another AuthInfo.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (AuthInfo AuthInfo1, AuthInfo AuthInfo2)
        {
            return !(AuthInfo1 < AuthInfo2);
        }

        #endregion

        #endregion

        #region IComparable<AuthInfo> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is an AuthInfo.
            var AuthInfo = Object as AuthInfo;
            if ((Object) AuthInfo == null)
                throw new ArgumentException("The given object is not a AuthInfo!");

            return CompareTo(AuthInfo);

        }

        #endregion

        #region CompareTo(AuthInfo)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfo">An object to compare with.</param>
        public Int32 CompareTo(AuthInfo AuthInfo)
        {

            if ((Object) AuthInfo == null)
                throw new ArgumentNullException(nameof(AuthInfo),  "The given AuthInfo must not be null!");

            if (AuthToken != null && AuthInfo.AuthToken != null)
                return AuthToken.CompareTo(AuthInfo.AuthToken);

            if (QRCodeIdentification != null && AuthInfo.QRCodeIdentification != null)
                return QRCodeIdentification.CompareTo(AuthInfo.QRCodeIdentification);

            if (PlugAndChargeIdentification.HasValue && AuthInfo.PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.CompareTo(AuthInfo.PlugAndChargeIdentification.Value);

            if (RemoteIdentification.HasValue && AuthInfo.RemoteIdentification.HasValue)
                return RemoteIdentification.Value.CompareTo(AuthInfo.RemoteIdentification.Value);

            return String.Compare(ToString(), AuthInfo.ToString(), StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<AuthInfo> Members

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

            // Check if the given object is an AuthInfo.
            var AuthInfo = Object as AuthInfo;
            if ((Object) AuthInfo == null)
                return false;

            return this.Equals(AuthInfo);

        }

        #endregion

        #region Equals(AuthInfo)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="AuthInfo">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(AuthInfo AuthInfo)
        {

            if ((Object) AuthInfo == null)
                return false;

            if (AuthToken != null && AuthInfo.AuthToken != null)
                return AuthToken.Equals(AuthInfo.AuthToken);

            if (QRCodeIdentification != null && AuthInfo.QRCodeIdentification != null)
                return QRCodeIdentification.Equals(AuthInfo.QRCodeIdentification);

            if (PlugAndChargeIdentification.HasValue && AuthInfo.PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.Equals(AuthInfo.PlugAndChargeIdentification.Value);

            if (RemoteIdentification.HasValue && AuthInfo.RemoteIdentification.HasValue)
                return RemoteIdentification.Value.Equals(AuthInfo.RemoteIdentification.Value);

            return false;

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return ToString().GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (AuthToken != null)
                return AuthToken.                        ToString();

            if (QRCodeIdentification != null)
                return QRCodeIdentification.             ToString();

            if (PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.ToString();

            if (RemoteIdentification.HasValue)
                return RemoteIdentification.       Value.ToString();

            return String.Empty;

        }

        #endregion

    }

}
