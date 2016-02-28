/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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

        #region AuthToken

        private readonly Auth_Token _AuthToken;

        /// <summary>
        /// An authentication token, e.g. the identification of a RFID card.
        /// </summary>
        public Auth_Token AuthToken
        {
            get
            {
                return _AuthToken;
            }
        }

        #endregion

        #region QRCodeIdentification

        private readonly eMAIdWithPIN2 _QRCodeIdentification;

        /// <summary>
        /// A e-mobility account identification and its password/PIN.
        /// Used within OICP.
        /// </summary>
        public eMAIdWithPIN2 QRCodeIdentification
        {
            get
            {
                return _QRCodeIdentification;
            }
        }

        #endregion

        #region PlugAndChargeIdentification

        private readonly eMA_Id _PlugAndChargeIdentification;

        /// <summary>
        /// A e-mobility account identification transmitted via PnC.
        /// </summary>
        public eMA_Id PlugAndChargeIdentification
        {
            get
            {
                return _PlugAndChargeIdentification;
            }
        }

        #endregion

        #region RemoteIdentification

        private readonly eMA_Id _RemoteIdentification;

        /// <summary>
        /// A e-mobility account identification.
        /// </summary>
        public eMA_Id RemoteIdentification
        {
            get
            {
                return _RemoteIdentification;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (private) AuthInfo(AuthToken)

        private AuthInfo(Auth_Token  AuthToken)
        {
            this._AuthToken = AuthToken;
        }

        #endregion

        #region (private) AuthInfo(QRCodeIdentification)

        private AuthInfo(eMAIdWithPIN2 QRCodeIdentification)
        {
            this._QRCodeIdentification = QRCodeIdentification;
        }

        #endregion

        #region (private) AuthInfo(PlugAndChargeIdentification, IsPnC)

        private AuthInfo(eMA_Id PlugAndChargeIdentification,
                                           Boolean IsPnC)
        {
            this._PlugAndChargeIdentification   = PlugAndChargeIdentification;
        }

        #endregion

        #region (private) AuthInfo(RemoteIdentification)

        private AuthInfo(eMA_Id RemoteIdentification)
        {
            this._RemoteIdentification = RemoteIdentification;
        }

        #endregion

        #endregion


        #region (static) FromAuthToken(AuthToken)

        /// <summary>
        /// Create a new authentication info based on the given authentication token.
        /// </summary>
        /// <param name="AuthToken">An authentication token.</param>
        public static AuthInfo FromAuthToken(Auth_Token AuthToken)
        {
            return new AuthInfo(AuthToken);
        }

        #endregion

        #region (static) FromQRCodeIdentification(eMAId, PIN)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="eMAId">An e-mobility account identification.</param>
        /// <param name="PIN">A password/PIN.</param>
        public static AuthInfo FromQRCodeIdentification(eMA_Id  eMAId,
                                                        String  PIN)
        {
            return new AuthInfo(new eMAIdWithPIN2(eMAId, PIN));
        }

        #endregion

        #region (static) FromQRCodeIdentification(QRCodeIdentification)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR code identification.</param>
        public static AuthInfo FromQRCodeIdentification(eMAIdWithPIN2 QRCodeIdentification)
        {
            return new AuthInfo(QRCodeIdentification);
        }

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification transmitted via PnC.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A PnC e-mobility account identification.</param>
        public static AuthInfo FromPlugAndChargeIdentification(eMA_Id PlugAndChargeIdentification)
        {
            return new AuthInfo(PlugAndChargeIdentification);
        }

        #endregion

        #region (static) FromRemoteIdentification(RemoteIdentification)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        public static AuthInfo FromRemoteIdentification(eMA_Id RemoteIdentification)
        {
            return new AuthInfo(RemoteIdentification);
        }

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
                throw new ArgumentNullException("The given object must not be null!");

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
                throw new ArgumentNullException("The given AuthInfo must not be null!");

            if (_AuthToken != null && AuthInfo._AuthToken != null)
                return _AuthToken.CompareTo(AuthInfo._AuthToken);

            if (_QRCodeIdentification != null && AuthInfo._QRCodeIdentification != null)
                return _QRCodeIdentification.CompareTo(AuthInfo._QRCodeIdentification);

            if (_PlugAndChargeIdentification != null && AuthInfo._PlugAndChargeIdentification != null)
                return _PlugAndChargeIdentification.CompareTo(AuthInfo._PlugAndChargeIdentification);

            if (_RemoteIdentification != null && AuthInfo._RemoteIdentification != null)
                return _RemoteIdentification.CompareTo(AuthInfo._RemoteIdentification);

            return this.ToString().CompareTo(AuthInfo.ToString());

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

            if (_AuthToken != null && AuthInfo._AuthToken != null)
                return _AuthToken.Equals(AuthInfo._AuthToken);

            if (_QRCodeIdentification != null && AuthInfo._QRCodeIdentification != null)
                return _QRCodeIdentification.Equals(AuthInfo._QRCodeIdentification);

            if (_PlugAndChargeIdentification != null && AuthInfo._PlugAndChargeIdentification != null)
                return _PlugAndChargeIdentification.Equals(AuthInfo._PlugAndChargeIdentification);

            if (_RemoteIdentification != null && AuthInfo._RemoteIdentification != null)
                return _RemoteIdentification.Equals(AuthInfo._RemoteIdentification);

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

            if (_AuthToken != null)
                return _AuthToken.ToString();

            if (_QRCodeIdentification != null)
                return _QRCodeIdentification.ToString();

            if (_PlugAndChargeIdentification != null)
                return _PlugAndChargeIdentification.ToString();

            if (_RemoteIdentification != null)
                return _RemoteIdentification.ToString();

            return String.Empty;

        }

        #endregion

    }

}
