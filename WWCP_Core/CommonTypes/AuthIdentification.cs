/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;

using Org.BouncyCastle.Bcpg.OpenPgp;

using org.GraphDefined.Vanaheimr.Illias;
using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Some sort of token for authentication.
    /// </summary>
    public sealed class AuthIdentification : IEquatable<AuthIdentification>,
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

        /// <summary>
        /// A PGP/GPG public key.
        /// </summary>
        public PgpPublicKey          PublicKey                      { get; }

        /// <summary>
        /// An optional multilingual description.
        /// </summary>
        public I18NString            Description                    { get; }

        #endregion

        #region Constructor(s)

        private AuthIdentification(Auth_Token            AuthToken                     = null,
                                   eMAIdWithPIN2         QRCodeIdentification          = null,
                                   eMobilityAccount_Id?  PlugAndChargeIdentification   = null,
                                   eMobilityAccount_Id?  RemoteIdentification          = null,
                                   PgpPublicKey          PublicKey                     = null,
                                   I18NString            Description                   = null)
        {

            this.AuthToken                    = AuthToken;
            this.QRCodeIdentification         = QRCodeIdentification;
            this.PlugAndChargeIdentification  = PlugAndChargeIdentification;
            this.RemoteIdentification         = RemoteIdentification;
            this.PublicKey                    = PublicKey;
            this.Description                  = Description;

        }

        #endregion


        #region (static) FromAuthToken                  (AuthToken,                   Description = null)

        /// <summary>
        /// Create a new authentication info based on the given authentication token.
        /// </summary>
        /// <param name="AuthToken">An authentication token.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static AuthIdentification FromAuthToken(Auth_Token  AuthToken,
                                                       I18NString  Description  = null)

            => new AuthIdentification(AuthToken:   AuthToken,
                                      Description: Description);

        #endregion

        #region (static) FromQRCodeIdentification       (eMAId, PIN,                  Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="eMAId">An e-mobility account identification.</param>
        /// <param name="PIN">A password/PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static AuthIdentification FromQRCodeIdentification(eMobilityAccount_Id  eMAId,
                                                                  String               PIN,
                                                                  I18NString           Description  = null)

            => new AuthIdentification(QRCodeIdentification: new eMAIdWithPIN2(eMAId, PIN),
                                      Description:          Description);

        #endregion

        #region (static) FromQRCodeIdentification       (QRCodeIdentification,        Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR code identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static AuthIdentification FromQRCodeIdentification(eMAIdWithPIN2  QRCodeIdentification,
                                                                  I18NString     Description  = null)

            => new AuthIdentification(QRCodeIdentification: QRCodeIdentification,
                                      Description:          Description);

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification, Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification transmitted via PnC.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A PnC e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static AuthIdentification FromPlugAndChargeIdentification(eMobilityAccount_Id  PlugAndChargeIdentification,
                                                                         I18NString           Description  = null)

            => new AuthIdentification(PlugAndChargeIdentification: PlugAndChargeIdentification,
                                      Description:                 Description);

        #endregion

        #region (static) FromRemoteIdentification       (RemoteIdentification,        Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static AuthIdentification FromRemoteIdentification(eMobilityAccount_Id  RemoteIdentification,
                                                                  I18NString           Description  = null)

            => new AuthIdentification(RemoteIdentification: RemoteIdentification,
                                      Description:          Description);


        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static AuthIdentification FromRemoteIdentification(eMobilityAccount_Id?  RemoteIdentification,
                                                                  I18NString            Description  = null)

            => RemoteIdentification.HasValue
                   ? new AuthIdentification(RemoteIdentification: RemoteIdentification,
                                            Description:          Description)
                   : null;

        #endregion

        #region (static) FromPublicKey                  (PublicKey,                   Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// PGP/GPG public key.
        /// </summary>
        /// <param name="PublicKey">A PGP/GPG public key.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static AuthIdentification FromPublicKey(PgpPublicKey  PublicKey,
                                                       I18NString    Description  = null)

            => new AuthIdentification(PublicKey:   PublicKey,
                                      Description: Description);

        #endregion


        public JObject ToJSON()

            => JSONObject.Create(

                   AuthToken                    != null
                       ? new JProperty("authToken",                     AuthToken.                      ToString())
                       : null,

                   QRCodeIdentification         != null
                       ? new JProperty("QRCodeIdentification",          QRCodeIdentification.           ToString())
                       : null,

                   PlugAndChargeIdentification  != null
                       ? new JProperty("plugAndChargeIdentification",   PlugAndChargeIdentification.    ToString())
                       : null,

                   RemoteIdentification         != null
                       ? new JProperty("remoteIdentification",          RemoteIdentification.           ToString())
                       : null,

                   PublicKey                    != null
                       ? new JProperty("publicKey",                     PublicKey.                      ToString())
                       : null

               );



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

            if (!(Object is AuthIdentification AuthIdentification))
                throw new ArgumentException("The given object is not an AuthIdentification!");

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

            if (PublicKey != null && AuthIdentification.PublicKey != null)
                return PublicKey.Fingerprint.ToHexString().CompareTo(AuthIdentification.PublicKey.Fingerprint.ToHexString());

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

            if (!(Object is AuthIdentification AuthIdentification))
                return false;

            return Equals(AuthIdentification);

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

            if (PublicKey != null && AuthIdentification.PublicKey != null)
                return PublicKey.Fingerprint.ToHexString().Equals(AuthIdentification.PublicKey.Fingerprint.ToHexString());

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (AuthToken            != null)
                return AuthToken.                        ToString();

            if (QRCodeIdentification != null)
                return QRCodeIdentification.             ToString();

            if (PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.ToString();

            if (RemoteIdentification.HasValue)
                return RemoteIdentification.       Value.ToString();

            if (PublicKey            != null)
                return PublicKey.Fingerprint.            ToHexString();

            return String.Empty;

        }

        #endregion

    }

}
