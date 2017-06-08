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
    public class AuthIdentification : IEquatable<AuthIdentification>,
                                      IComparable<AuthIdentification>
    {

        #region Properties

        /// <summary>
        /// An authentication token, e.g. the identification of a RFID card.
        /// </summary>
        public Auth_Token            AuthToken                      { get; }

        /// <summary>
        /// A e-mobility account identification and its password/PIN.
        /// Used within OICP.
        /// </summary>
        public eMAIdWithPIN2         QRCodeIdentification           { get; }

        /// <summary>
        /// A e-mobility account identification transmitted via PnC.
        /// </summary>
        public eMobilityAccount_Id?  PlugAndChargeIdentification    { get; }

        /// <summary>
        /// A e-mobility account identification.
        /// </summary>
        public eMobilityAccount_Id?  RemoteIdentification           { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthIdentification(AuthToken)

        private AuthIdentification(Auth_Token  AuthToken)
        {
            this.AuthToken = AuthToken;
        }

        #endregion

        #region (private) AuthIdentification(QRCodeIdentification)

        private AuthIdentification(eMAIdWithPIN2 QRCodeIdentification)
        {
            this.QRCodeIdentification = QRCodeIdentification;
        }

        #endregion

        #region (private) AuthIdentification(PlugAndChargeIdentification, IsPnC)

        private AuthIdentification(eMobilityAccount_Id  PlugAndChargeIdentification,
                                   Boolean              IsPnC)
        {
            this.PlugAndChargeIdentification  = PlugAndChargeIdentification;
        }

        #endregion

        #region (private) AuthIdentification(RemoteIdentification)

        private AuthIdentification(eMobilityAccount_Id RemoteIdentification)
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
        public static AuthIdentification FromAuthToken(Auth_Token AuthToken)

            => new AuthIdentification(AuthToken);

        #endregion

        #region (static) FromQRCodeIdentification(eMAId, PIN)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="eMAId">An e-mobility account identification.</param>
        /// <param name="PIN">A password/PIN.</param>
        public static AuthIdentification FromQRCodeIdentification(eMobilityAccount_Id  eMAId,
                                                        String               PIN)

            => new AuthIdentification(new eMAIdWithPIN2(eMAId, PIN));

        #endregion

        #region (static) FromQRCodeIdentification(QRCodeIdentification)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR code identification.</param>
        public static AuthIdentification FromQRCodeIdentification(eMAIdWithPIN2 QRCodeIdentification)

            => new AuthIdentification(QRCodeIdentification);

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification transmitted via PnC.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A PnC e-mobility account identification.</param>
        public static AuthIdentification FromPlugAndChargeIdentification(eMobilityAccount_Id PlugAndChargeIdentification)

            => new AuthIdentification(PlugAndChargeIdentification);

        #endregion

        #region (static) FromRemoteIdentification(RemoteIdentification)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        public static AuthIdentification FromRemoteIdentification(eMobilityAccount_Id RemoteIdentification)

            => new AuthIdentification(RemoteIdentification);

        #endregion


        #region Operator overloading

        #region Operator == (AuthIdentification1, AuthIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthIdentification1">A AuthIdentification.</param>
        /// <param name="AuthIdentification2">Another AuthIdentification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (AuthIdentification AuthIdentification1, AuthIdentification AuthIdentification2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AuthIdentification1, AuthIdentification2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthIdentification1 == null) || ((Object) AuthIdentification2 == null))
                return false;

            return AuthIdentification1.Equals(AuthIdentification2);

        }

        #endregion

        #region Operator != (AuthIdentification1, AuthIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthIdentification1">A AuthIdentification.</param>
        /// <param name="AuthIdentification2">Another AuthIdentification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (AuthIdentification AuthIdentification1, AuthIdentification AuthIdentification2)
        {
            return !(AuthIdentification1 == AuthIdentification2);
        }

        #endregion

        #region Operator <  (AuthIdentification1, AuthIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthIdentification1">A AuthIdentification.</param>
        /// <param name="AuthIdentification2">Another AuthIdentification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (AuthIdentification AuthIdentification1, AuthIdentification AuthIdentification2)
        {

            if ((Object) AuthIdentification1 == null)
                throw new ArgumentNullException("The given AuthIdentification1 must not be null!");

            return AuthIdentification1.CompareTo(AuthIdentification2) < 0;

        }

        #endregion

        #region Operator <= (AuthIdentification1, AuthIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthIdentification1">A AuthIdentification.</param>
        /// <param name="AuthIdentification2">Another AuthIdentification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (AuthIdentification AuthIdentification1, AuthIdentification AuthIdentification2)
        {
            return !(AuthIdentification1 > AuthIdentification2);
        }

        #endregion

        #region Operator >  (AuthIdentification1, AuthIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthIdentification1">A AuthIdentification.</param>
        /// <param name="AuthIdentification2">Another AuthIdentification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (AuthIdentification AuthIdentification1, AuthIdentification AuthIdentification2)
        {

            if ((Object) AuthIdentification1 == null)
                throw new ArgumentNullException("The given AuthIdentification1 must not be null!");

            return AuthIdentification1.CompareTo(AuthIdentification2) > 0;

        }

        #endregion

        #region Operator >= (AuthIdentification1, AuthIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthIdentification1">A AuthIdentification.</param>
        /// <param name="AuthIdentification2">Another AuthIdentification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (AuthIdentification AuthIdentification1, AuthIdentification AuthIdentification2)
        {
            return !(AuthIdentification1 < AuthIdentification2);
        }

        #endregion

        #endregion

        #region IComparable<AuthIdentification> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is an AuthIdentification.
            var AuthIdentification = Object as AuthIdentification;
            if ((Object) AuthIdentification == null)
                throw new ArgumentException("The given object is not a AuthIdentification!");

            return CompareTo(AuthIdentification);

        }

        #endregion

        #region CompareTo(AuthIdentification)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthIdentification">An object to compare with.</param>
        public Int32 CompareTo(AuthIdentification AuthIdentification)
        {

            if ((Object) AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),  "The given AuthIdentification must not be null!");

            if (AuthToken != null && AuthIdentification.AuthToken != null)
                return AuthToken.CompareTo(AuthIdentification.AuthToken);

            if (QRCodeIdentification != null && AuthIdentification.QRCodeIdentification != null)
                return QRCodeIdentification.CompareTo(AuthIdentification.QRCodeIdentification);

            if (PlugAndChargeIdentification.HasValue && AuthIdentification.PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.CompareTo(AuthIdentification.PlugAndChargeIdentification.Value);

            if (RemoteIdentification.HasValue && AuthIdentification.RemoteIdentification.HasValue)
                return RemoteIdentification.Value.CompareTo(AuthIdentification.RemoteIdentification.Value);

            return String.Compare(ToString(), AuthIdentification.ToString(), StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<AuthIdentification> Members

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

            // Check if the given object is an AuthIdentification.
            var AuthIdentification = Object as AuthIdentification;
            if ((Object) AuthIdentification == null)
                return false;

            return this.Equals(AuthIdentification);

        }

        #endregion

        #region Equals(AuthIdentification)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="AuthIdentification">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(AuthIdentification AuthIdentification)
        {

            if ((Object) AuthIdentification == null)
                return false;

            if (AuthToken != null && AuthIdentification.AuthToken != null)
                return AuthToken.Equals(AuthIdentification.AuthToken);

            if (QRCodeIdentification != null && AuthIdentification.QRCodeIdentification != null)
                return QRCodeIdentification.Equals(AuthIdentification.QRCodeIdentification);

            if (PlugAndChargeIdentification.HasValue && AuthIdentification.PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.Equals(AuthIdentification.PlugAndChargeIdentification.Value);

            if (RemoteIdentification.HasValue && AuthIdentification.RemoteIdentification.HasValue)
                return RemoteIdentification.Value.Equals(AuthIdentification.RemoteIdentification.Value);

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
