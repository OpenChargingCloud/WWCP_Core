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

    public static class AAuthenticationExtentions
    {

        public static Boolean IsDefined(this AAuthentication Authentication)

            => Authentication != null &&
               (Authentication.AuthToken.                  HasValue ||
                Authentication.QRCodeIdentification        != null  ||
                Authentication.PlugAndChargeIdentification.HasValue ||
                Authentication.RemoteIdentification.       HasValue ||
                Authentication.PIN.                        HasValue ||
                Authentication.PublicKey                   != null);


        public static Boolean IsNull(this AAuthentication Authentication)
            => !IsDefined(Authentication);

    }



    /// <summary>
    /// Some sort of token for authentication.
    /// </summary>
    public abstract class AAuthentication : IEquatable<AAuthentication>,
                                            IComparable<AAuthentication>
    {

        #region Properties

        /// <summary>
        /// An authentication token, e.g. the identification of a RFID card.
        /// </summary>
        public Auth_Token?           AuthToken                      { get; }

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
        /// A PIN.
        /// </summary>
        public UInt32?               PIN                            { get; }

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

        protected AAuthentication(Auth_Token?           AuthToken                     = null,
                                  eMAIdWithPIN2         QRCodeIdentification          = null,
                                  eMobilityAccount_Id?  PlugAndChargeIdentification   = null,
                                  eMobilityAccount_Id?  RemoteIdentification          = null,
                                  UInt32?               PIN                           = null,
                                  PgpPublicKey          PublicKey                     = null,
                                  I18NString            Description                   = null)
        {

            this.AuthToken                    = AuthToken;
            this.QRCodeIdentification         = QRCodeIdentification;
            this.PlugAndChargeIdentification  = PlugAndChargeIdentification;
            this.RemoteIdentification         = RemoteIdentification;
            this.PIN                          = PIN;
            this.PublicKey                    = PublicKey;
            this.Description                  = Description;

        }

        #endregion


        public JObject ToJSON()

            => JSONObject.Create(

                   AuthToken.HasValue
                       ? new JProperty("authToken",                     AuthToken.                      ToString())
                       : null,

                   QRCodeIdentification         != null
                       ? new JProperty("QRCodeIdentification",          QRCodeIdentification.           ToString())
                       : null,

                   PlugAndChargeIdentification.HasValue
                       ? new JProperty("plugAndChargeIdentification",   PlugAndChargeIdentification.    ToString())
                       : null,

                   RemoteIdentification.HasValue
                       ? new JProperty("remoteIdentification",          RemoteIdentification.           ToString())
                       : null,

                   PIN.HasValue
                       ? new JProperty("PIN",                           PIN.                            ToString())
                       : null,

                   PublicKey                    != null
                       ? new JProperty("publicKey",                     PublicKey.                      ToString())
                       : null

               );


        #region Operator overloading

        #region Operator == (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (AAuthentication AAuthentication1, AAuthentication AAuthentication2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AAuthentication1, AAuthentication2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AAuthentication1 == null) || ((Object) AAuthentication2 == null))
                return false;

            return AAuthentication1.Equals(AAuthentication2);

        }

        #endregion

        #region Operator != (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (AAuthentication AAuthentication1, AAuthentication AAuthentication2)
        {
            return !(AAuthentication1 == AAuthentication2);
        }

        #endregion

        #region Operator <  (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (AAuthentication AAuthentication1, AAuthentication AAuthentication2)
        {

            if ((Object) AAuthentication1 == null)
                throw new ArgumentNullException("The given AAuthentication1 must not be null!");

            return AAuthentication1.CompareTo(AAuthentication2) < 0;

        }

        #endregion

        #region Operator <= (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (AAuthentication AAuthentication1, AAuthentication AAuthentication2)
        {
            return !(AAuthentication1 > AAuthentication2);
        }

        #endregion

        #region Operator >  (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (AAuthentication AAuthentication1, AAuthentication AAuthentication2)
        {

            if ((Object) AAuthentication1 == null)
                throw new ArgumentNullException("The given AAuthentication1 must not be null!");

            return AAuthentication1.CompareTo(AAuthentication2) > 0;

        }

        #endregion

        #region Operator >= (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (AAuthentication AAuthentication1, AAuthentication AAuthentication2)
        {
            return !(AAuthentication1 < AAuthentication2);
        }

        #endregion

        #endregion

        #region IComparable<AAuthentication> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            if (!(Object is AAuthentication AAuthentication))
                throw new ArgumentException("The given object is not an abstract authentication!");

            return CompareTo(AAuthentication);

        }

        #endregion

        #region CompareTo(AAuthentication)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication">An object to compare with.</param>
        public Int32 CompareTo(AAuthentication AAuthentication)
        {

            if ((Object) AAuthentication == null)
                throw new ArgumentNullException(nameof(AAuthentication),  "The given abstract authentication must not be null!");

            if (AuthToken.HasValue && AAuthentication.AuthToken.HasValue)
                return AuthToken.Value.CompareTo(AAuthentication.AuthToken.Value);

            if (QRCodeIdentification != null && AAuthentication.QRCodeIdentification != null)
                return QRCodeIdentification.CompareTo(AAuthentication.QRCodeIdentification);

            if (PlugAndChargeIdentification.HasValue && AAuthentication.PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.CompareTo(AAuthentication.PlugAndChargeIdentification.Value);

            if (RemoteIdentification.HasValue && AAuthentication.RemoteIdentification.HasValue)
                return RemoteIdentification.Value.CompareTo(AAuthentication.RemoteIdentification.Value);

            if (PIN.HasValue && AAuthentication.PIN.HasValue)
                return PIN.Value.CompareTo(AAuthentication.PIN.Value);

            if (PublicKey != null && AAuthentication.PublicKey != null)
                return PublicKey.Fingerprint.ToHexString().CompareTo(AAuthentication.PublicKey.Fingerprint.ToHexString());

            return String.Compare(ToString(), AAuthentication.ToString(), StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<AAuthentication> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            if (!(Object is AAuthentication AAuthentication))
                return false;

            return Equals(AAuthentication);

        }

        #endregion

        #region Equals(AAuthentication)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="AAuthentication">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(AAuthentication AAuthentication)
        {

            if ((Object) AAuthentication == null)
                return false;

            if (AuthToken.HasValue && AAuthentication.AuthToken.HasValue)
                return AuthToken.Value.Equals(AAuthentication.AuthToken.Value);

            if (QRCodeIdentification != null && AAuthentication.QRCodeIdentification != null)
                return QRCodeIdentification.Equals(AAuthentication.QRCodeIdentification);

            if (PlugAndChargeIdentification.HasValue && AAuthentication.PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.Equals(AAuthentication.PlugAndChargeIdentification.Value);

            if (RemoteIdentification.HasValue && AAuthentication.RemoteIdentification.HasValue)
                return RemoteIdentification.Value.Equals(AAuthentication.RemoteIdentification.Value);

            if (PIN.HasValue && AAuthentication.PIN.HasValue)
                return PIN.Value.Equals(AAuthentication.PIN.Value);

            if (PublicKey != null && AAuthentication.PublicKey != null)
                return PublicKey.Fingerprint.ToHexString().Equals(AAuthentication.PublicKey.Fingerprint.ToHexString());

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

            if (AuthToken.HasValue)
                return AuthToken.                        ToString();

            if (QRCodeIdentification != null)
                return QRCodeIdentification.             ToString();

            if (PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.ToString();

            if (RemoteIdentification.HasValue)
                return RemoteIdentification.       Value.ToString();

            if (PIN.HasValue)
                return PIN.                        Value.ToString();

            if (PublicKey            != null)
                return PublicKey.Fingerprint.            ToHexString();

            return String.Empty;

        }

        #endregion

    }


    public class LocalAuthentication : AAuthentication
    {


        #region Constructor(s)

        protected internal LocalAuthentication(Auth_Token?           AuthToken                     = null,
                                               eMAIdWithPIN2         QRCodeIdentification          = null,
                                               eMobilityAccount_Id?  PlugAndChargeIdentification   = null,
                                               eMobilityAccount_Id?  RemoteIdentification          = null,
                                               UInt32?               PIN                           = null,
                                               PgpPublicKey          PublicKey                     = null,
                                               I18NString            Description                   = null)

            : base(AuthToken,
                   QRCodeIdentification,
                   PlugAndChargeIdentification,
                   RemoteIdentification,
                   PIN,
                   PublicKey,
                   Description)

        { }

        #endregion


        #region (static) FromAuthToken                  (AuthToken,                   Description = null)

        /// <summary>
        /// Create a new authentication info based on the given authentication token.
        /// </summary>
        /// <param name="AuthToken">An authentication token.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromAuthToken(Auth_Token  AuthToken,
                                                        I18NString  Description  = null)

            => new LocalAuthentication(AuthToken:   AuthToken,
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
        public static LocalAuthentication FromQRCodeIdentification(eMobilityAccount_Id  eMAId,
                                                                   String               PIN,
                                                                   I18NString           Description  = null)

            => new LocalAuthentication(QRCodeIdentification: new eMAIdWithPIN2(eMAId, PIN),
                                       Description:          Description);

        #endregion

        #region (static) FromQRCodeIdentification       (QRCodeIdentification,        Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR code identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromQRCodeIdentification(eMAIdWithPIN2  QRCodeIdentification,
                                                                   I18NString     Description  = null)

            => new LocalAuthentication(QRCodeIdentification: QRCodeIdentification,
                                       Description:          Description);

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification, Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification transmitted via PnC.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A PnC e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromPlugAndChargeIdentification(eMobilityAccount_Id  PlugAndChargeIdentification,
                                                                          I18NString           Description  = null)

            => new LocalAuthentication(PlugAndChargeIdentification: PlugAndChargeIdentification,
                                       Description:                 Description);

        #endregion

        #region (static) FromRemoteIdentification       (RemoteIdentification,        Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromRemoteIdentification(eMobilityAccount_Id  RemoteIdentification,
                                                                   I18NString           Description  = null)

            => new LocalAuthentication(RemoteIdentification: RemoteIdentification,
                                       Description:          Description);


        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromRemoteIdentification(eMobilityAccount_Id?  RemoteIdentification,
                                                                   I18NString            Description  = null)

            => RemoteIdentification.HasValue
                   ? new LocalAuthentication(RemoteIdentification: RemoteIdentification,
                                             Description:          Description)
                   : null;

        #endregion

        #region (static) FromPIN       (PIN,        Description = null)

        /// <summary>
        /// Create a new authentication info based on the given PIN.
        /// </summary>
        /// <param name="PIN">An PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromPIN(UInt32      PIN,
                                                  I18NString  Description  = null)

            => new LocalAuthentication(PIN:          PIN,
                                       Description:  Description);


        /// <summary>
        /// Create a new authentication info based on the given PIN.
        /// </summary>
        /// <param name="PIN">An PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromPIN(UInt32?     PIN,
                                                  I18NString  Description  = null)

            => PIN.HasValue
                   ? new LocalAuthentication(PIN:          PIN,
                                             Description:  Description)
                   : null;

        #endregion

        #region (static) FromPublicKey                  (PublicKey,                   Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// PGP/GPG public key.
        /// </summary>
        /// <param name="PublicKey">A PGP/GPG public key.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromPublicKey(PgpPublicKey  PublicKey,
                                                        I18NString    Description  = null)

            => new LocalAuthentication(PublicKey:   PublicKey,
                                       Description: Description);

        #endregion



        public RemoteAuthentication ToRemote

            => new RemoteAuthentication(AuthToken,
                                        QRCodeIdentification,
                                        PlugAndChargeIdentification,
                                        RemoteIdentification,
                                        PIN,
                                        PublicKey,
                                        Description);

    }

    public class RemoteAuthentication : AAuthentication
    {


        #region Constructor(s)

        protected internal RemoteAuthentication(Auth_Token?           AuthToken                     = null,
                                                eMAIdWithPIN2         QRCodeIdentification          = null,
                                                eMobilityAccount_Id?  PlugAndChargeIdentification   = null,
                                                eMobilityAccount_Id?  RemoteIdentification          = null,
                                                UInt32?               PIN                           = null,
                                                PgpPublicKey          PublicKey                     = null,
                                                I18NString            Description                   = null)

            : base(AuthToken,
                   QRCodeIdentification,
                   PlugAndChargeIdentification,
                   RemoteIdentification,
                   PIN,
                   PublicKey,
                   Description)

        { }

        #endregion


        #region (static) FromAuthToken                  (AuthToken,                   Description = null)

        /// <summary>
        /// Create a new authentication info based on the given authentication token.
        /// </summary>
        /// <param name="AuthToken">An authentication token.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromAuthToken(Auth_Token  AuthToken,
                                                       I18NString  Description  = null)

            => new RemoteAuthentication(AuthToken:   AuthToken,
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
        public static RemoteAuthentication FromQRCodeIdentification(eMobilityAccount_Id  eMAId,
                                                                  String               PIN,
                                                                  I18NString           Description  = null)

            => new RemoteAuthentication(QRCodeIdentification: new eMAIdWithPIN2(eMAId, PIN),
                                        Description:          Description);

        #endregion

        #region (static) FromQRCodeIdentification       (QRCodeIdentification,        Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR code identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromQRCodeIdentification(eMAIdWithPIN2  QRCodeIdentification,
                                                                  I18NString     Description  = null)

            => new RemoteAuthentication(QRCodeIdentification: QRCodeIdentification,
                                        Description:          Description);

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification, Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification transmitted via PnC.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A PnC e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromPlugAndChargeIdentification(eMobilityAccount_Id  PlugAndChargeIdentification,
                                                                         I18NString           Description  = null)

            => new RemoteAuthentication(PlugAndChargeIdentification: PlugAndChargeIdentification,
                                        Description:                 Description);

        #endregion

        #region (static) FromRemoteIdentification       (RemoteIdentification,        Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromRemoteIdentification(eMobilityAccount_Id  RemoteIdentification,
                                                                  I18NString           Description  = null)

            => new RemoteAuthentication(RemoteIdentification: RemoteIdentification,
                                        Description:          Description);


        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromRemoteIdentification(eMobilityAccount_Id?  RemoteIdentification,
                                                                  I18NString            Description  = null)

            => RemoteIdentification.HasValue
                   ? new RemoteAuthentication(RemoteIdentification: RemoteIdentification,
                                              Description:          Description)
                   : null;

        #endregion

        #region (static) FromPIN       (PIN,        Description = null)

        /// <summary>
        /// Create a new authentication info based on the given PIN.
        /// </summary>
        /// <param name="PIN">An PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromPIN(UInt32      PIN,
                                                   I18NString  Description  = null)

            => new RemoteAuthentication(PIN:          PIN,
                                        Description:  Description);


        /// <summary>
        /// Create a new authentication info based on the given PIN.
        /// </summary>
        /// <param name="PIN">An PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromPIN(UInt32?     PIN,
                                                   I18NString  Description  = null)

            => PIN.HasValue
                   ? new RemoteAuthentication(PIN:          PIN,
                                              Description:  Description)
                   : null;

        #endregion

        #region (static) FromPublicKey                  (PublicKey,                   Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// PGP/GPG public key.
        /// </summary>
        /// <param name="PublicKey">A PGP/GPG public key.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromPublicKey(PgpPublicKey  PublicKey,
                                                       I18NString    Description  = null)

            => new RemoteAuthentication(PublicKey:   PublicKey,
                                        Description: Description);

        #endregion


        public LocalAuthentication ToLocal

            => new LocalAuthentication(AuthToken,
                                       QRCodeIdentification,
                                       PlugAndChargeIdentification,
                                       RemoteIdentification,
                                       PIN,
                                       PublicKey,
                                       Description);

    }


}
