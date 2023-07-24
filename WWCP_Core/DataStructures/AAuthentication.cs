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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public static class AAuthenticationExtensions
    {

        public static Boolean IsDefined(this AAuthentication Authentication)

            => Authentication is not null &&
               (Authentication.AuthToken.                  HasValue ||
                Authentication.QRCodeIdentification.       HasValue ||
                Authentication.PlugAndChargeIdentification.HasValue ||
                Authentication.RemoteIdentification.       HasValue ||
                Authentication.PIN.                        HasValue ||
                Authentication.PublicKey.                  HasValue ||
                Authentication.Certificate.                HasValue);


        public static Boolean IsNull(this AAuthentication Authentication)

            => !IsDefined(Authentication);

    }



    /// <summary>
    /// An abstract user authentication.
    /// </summary>
    public abstract class AAuthentication : IEquatable<AAuthentication>,
                                            IComparable<AAuthentication>
    {

        #region Properties

        /// <summary>
        /// An authentication token, e.g. the identification of a RFID card.
        /// </summary>
        public AuthenticationToken?  AuthToken                      { get; }

        /// <summary>
        /// A e-mobility account identification and its password/PIN.
        /// Used within OICP.
        /// </summary>
        public eMAIdWithPIN2?        QRCodeIdentification           { get; }

        /// <summary>
        /// A e-mobility account identification transmitted via PnC.
        /// </summary>
        public EMobilityAccount_Id?  PlugAndChargeIdentification    { get; }

        /// <summary>
        /// A e-mobility account identification.
        /// </summary>
        public EMobilityAccount_Id?  RemoteIdentification           { get; }

        /// <summary>
        /// A PIN.
        /// </summary>
        public PIN?                  PIN                            { get; }

        /// <summary>
        /// A public key.
        /// </summary>
        public PublicKey?            PublicKey                      { get; }

        /// <summary>
        /// A certificate.
        /// </summary>
        public Certificate?          Certificate                    { get; }

        /// <summary>
        /// The (additional) authentication method used.
        /// </summary>
        public AuthMethod?           AuthMethod                     { get; }

        /// <summary>
        /// An optional multilingual description.
        /// </summary>
        public I18NString?           Description                    { get; }

        #endregion

        #region Constructor(s)

        protected AAuthentication(AuthenticationToken?  AuthToken                     = null,
                                  eMAIdWithPIN2?        QRCodeIdentification          = null,
                                  EMobilityAccount_Id?  PlugAndChargeIdentification   = null,
                                  EMobilityAccount_Id?  RemoteIdentification          = null,
                                  PIN?                  PIN                           = null,
                                  PublicKey?            PublicKey                     = null,
                                  Certificate?          Certificate                   = null,
                                  AuthMethod?           AuthMethod                    = null,
                                  I18NString?           Description                   = null)
        {

            this.AuthToken                    = AuthToken;
            this.QRCodeIdentification         = QRCodeIdentification;
            this.PlugAndChargeIdentification  = PlugAndChargeIdentification;
            this.RemoteIdentification         = RemoteIdentification;
            this.PIN                          = PIN;
            this.PublicKey                    = PublicKey;
            this.Certificate                  = Certificate;
            this.AuthMethod                   = AuthMethod;
            this.Description                  = Description;

        }

        #endregion


        #region ToJSON()

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        public JObject ToJSON()

            => JSONObject.Create(

                   AuthToken.HasValue
                       ? new JProperty("authToken",                     AuthToken.                        ToString())
                       : null,

                   QRCodeIdentification.HasValue
                       ? new JProperty("QRCodeIdentification",          QRCodeIdentification.       Value.ToJSON())
                       : null,

                   PlugAndChargeIdentification.HasValue
                       ? new JProperty("plugAndChargeIdentification",   PlugAndChargeIdentification.Value.ToString())
                       : null,

                   RemoteIdentification.HasValue
                       ? new JProperty("remoteIdentification",          RemoteIdentification.       Value.ToString())
                       : null,

                   PIN.HasValue
                       ? new JProperty("PIN",                           PIN.                        Value.ToString())
                       : null,

                   PublicKey.HasValue
                       ? new JProperty("publicKey",                     PublicKey.                  Value.ToString())
                       : null,

                   Certificate.HasValue
                       ? new JProperty("certificate",                   Certificate.                Value.ToString())
                       : null,

                   AuthMethod.HasValue
                       ? new JProperty("authMethod",                    AuthMethod.                 Value.ToString())
                       : null,

                   Description is not null && Description.IsNeitherNullNorEmpty()
                       ? new JProperty("description",                   Description.                      ToString())
                       : null

               );

        #endregion


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
            if (ReferenceEquals(AAuthentication1, AAuthentication2))
                return true;

            // If one is null, but not both, return false.
            if ((AAuthentication1 is null) || (AAuthentication2 is null))
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
            => !(AAuthentication1 == AAuthentication2);

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

            if (AAuthentication1 is null)
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
            => !(AAuthentication1 > AAuthentication2);

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

            if (AAuthentication1 is null)
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
            => !(AAuthentication1 < AAuthentication2);

        #endregion

        #endregion

        #region IComparable<AAuthentication> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two abstract authentications.
        /// </summary>
        /// <param name="Object">An abstract authentication to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is AAuthentication aAuthentication
                   ? CompareTo(aAuthentication)
                   : throw new ArgumentException("The given object is not abstract authentication!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AAuthentication)

        /// <summary>
        /// Compares two abstract authentications.
        /// </summary>
        /// <param name="AAuthentication">An abstract authentication to compare with.</param>
        public Int32 CompareTo(AAuthentication? AAuthentication)
        {

            if (AAuthentication is null)
                throw new ArgumentNullException(nameof(AAuthentication),  "The given abstract authentication must not be null!");

            if (AuthToken.                  HasValue && AAuthentication.AuthToken.                  HasValue)
                return AuthToken.                  Value.CompareTo(AAuthentication.AuthToken.                  Value);

            if (QRCodeIdentification.       HasValue && AAuthentication.QRCodeIdentification.       HasValue)
                return QRCodeIdentification.       Value.CompareTo(AAuthentication.QRCodeIdentification.       Value);

            if (PlugAndChargeIdentification.HasValue && AAuthentication.PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.CompareTo(AAuthentication.PlugAndChargeIdentification.Value);

            if (RemoteIdentification.       HasValue && AAuthentication.RemoteIdentification.       HasValue)
                return RemoteIdentification.       Value.CompareTo(AAuthentication.RemoteIdentification.       Value);

            if (PIN.                        HasValue && AAuthentication.PIN.                        HasValue)
                return PIN.                        Value.CompareTo(AAuthentication.PIN.                        Value);

            if (PublicKey.                  HasValue && AAuthentication.PublicKey.                  HasValue)
                return PublicKey.                  Value.CompareTo(AAuthentication.PublicKey.                  Value);

            if (Certificate.                HasValue && AAuthentication.Certificate.                HasValue)
                return Certificate.                Value.CompareTo(AAuthentication.Certificate.                Value);

            return String.Compare(ToString(), AAuthentication.ToString(), StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<AAuthentication> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two abstract authentications for equality.
        /// </summary>
        /// <param name="Object">An abstract authentication to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AAuthentication aAuthentication &&
                   Equals(aAuthentication);

        #endregion

        #region Equals(AAuthentication)

        /// <summary>
        /// Compares two abstract authentications for equality.
        /// </summary>
        /// <param name="AAuthentication">An abstract authentication to compare with.</param>
        public Boolean Equals(AAuthentication? AAuthentication)

            => AAuthentication is not null &&

            ((!AuthToken.                  HasValue && !AAuthentication.AuthToken.                  HasValue) ||
              (AuthToken.                  HasValue &&  AAuthentication.AuthToken.                  HasValue && AuthToken.                  Value.Equals(AAuthentication.AuthToken.                  Value))) &&

            ((!QRCodeIdentification.       HasValue && !AAuthentication.QRCodeIdentification.       HasValue) ||
              (QRCodeIdentification.       HasValue &&  AAuthentication.QRCodeIdentification.       HasValue && QRCodeIdentification.       Value.Equals(AAuthentication.QRCodeIdentification.       Value))) &&

            ((!PlugAndChargeIdentification.HasValue && !AAuthentication.PlugAndChargeIdentification.HasValue) ||
              (PlugAndChargeIdentification.HasValue &&  AAuthentication.PlugAndChargeIdentification.HasValue && PlugAndChargeIdentification.Value.Equals(AAuthentication.PlugAndChargeIdentification.Value))) &&

            ((!RemoteIdentification.       HasValue && !AAuthentication.RemoteIdentification.       HasValue) ||
              (RemoteIdentification.       HasValue &&  AAuthentication.RemoteIdentification.       HasValue && RemoteIdentification.       Value.Equals(AAuthentication.RemoteIdentification.       Value))) &&

            ((!PIN.                        HasValue && !AAuthentication.PIN.                        HasValue) ||
              (PIN.                        HasValue &&  AAuthentication.PIN.                        HasValue && PIN.                        Value.Equals(AAuthentication.PIN.                        Value))) &&

            ((!PublicKey.                  HasValue && !AAuthentication.PublicKey.                  HasValue) ||
              (PublicKey.                  HasValue &&  AAuthentication.PublicKey.                  HasValue && PublicKey.                  Value.Equals(AAuthentication.PublicKey.                  Value))) &&

            ((!Certificate.                HasValue && !AAuthentication.Certificate.                HasValue) ||
              (Certificate.                HasValue &&  AAuthentication.Certificate.                HasValue && Certificate.                Value.Equals(AAuthentication.Certificate.                Value))) &&

            ((!AuthMethod.                 HasValue && !AAuthentication.AuthMethod.                 HasValue) ||
              (AuthMethod.                 HasValue &&  AAuthentication.AuthMethod.                 HasValue && AuthMethod.                 Value.Equals(AAuthentication.AuthMethod.                 Value))) &&

             ((Description is null                  &&  AAuthentication.AuthMethod is null)                   ||
              (Description is not null              &&  AAuthentication.AuthMethod is not null               && Description.                      Equals(AAuthentication.Description)));

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

                return (AuthToken?.                  GetHashCode() ?? 0) * 23 ^
                       (QRCodeIdentification?.       GetHashCode() ?? 0) * 19 ^
                       (PlugAndChargeIdentification?.GetHashCode() ?? 0) * 17 ^
                       (RemoteIdentification?.       GetHashCode() ?? 0) * 13 ^
                       (PIN?.                        GetHashCode() ?? 0) * 11 ^
                       (PublicKey?.                  GetHashCode() ?? 0) *  7 ^
                       (Certificate?.                GetHashCode() ?? 0) *  5 ^
                       (AuthMethod?.                 GetHashCode() ?? 0) *  3 ^
                       (Description?.                GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => new String[] {

                   AuthToken.HasValue
                       ? $"authToken: {AuthToken.Value}"
                       : String.Empty,

                   QRCodeIdentification.HasValue
                       ? $"QR Code identification: {QRCodeIdentification.Value}"
                       : String.Empty,

                   PlugAndChargeIdentification.HasValue
                       ? $"Plug&Charge identification: {PlugAndChargeIdentification.Value}"
                       : String.Empty,

                   RemoteIdentification.HasValue
                       ? $"Remote identification: {RemoteIdentification.Value}"
                       : String.Empty,

                   PIN.HasValue
                       ? $"PIN: {PIN.Value}"
                       : String.Empty,

                   PublicKey.HasValue
                       ? $"Public key: {PublicKey.Value}"
                       : String.Empty,

                   Certificate.HasValue
                       ? $"Certificate: {Certificate.Value}"
                       : String.Empty,

                   AuthMethod.HasValue
                       ? $"Auth method: {AuthMethod.Value}"
                       : String.Empty

               }.Where(_ => _.IsNeitherNullNorEmpty()).
                 AggregateWith(", ");

        #endregion

    }


    public class LocalAuthentication : AAuthentication
    {

        #region Constructor(s)

        protected internal LocalAuthentication(AuthenticationToken?  AuthToken                     = null,
                                               eMAIdWithPIN2?        QRCodeIdentification          = null,
                                               EMobilityAccount_Id?  PlugAndChargeIdentification   = null,
                                               EMobilityAccount_Id?  RemoteIdentification          = null,
                                               PIN?                  PIN                           = null,
                                               PublicKey?            PublicKey                     = null,
                                               Certificate?          Certificate                   = null,
                                               AuthMethod?           AuthMethod                    = null,
                                               I18NString?           Description                   = null)

            : base(AuthToken,
                   QRCodeIdentification,
                   PlugAndChargeIdentification,
                   RemoteIdentification,
                   PIN,
                   PublicKey,
                   Certificate,
                   AuthMethod,
                   Description)

        { }

        #endregion


        #region (static) FromAuthToken                  (AuthToken,                              Description = null)

        /// <summary>
        /// Create a new authentication info based on the given authentication token.
        /// </summary>
        /// <param name="AuthToken">An authentication token.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromAuthToken(AuthenticationToken  AuthToken,
                                                        AuthMethod?          AuthMethod    = null,
                                                        I18NString?          Description   = null)

            => new (AuthToken:    AuthToken,
                    AuthMethod:   AuthMethod,
                    Description:  Description);

        #endregion

        #region (static) FromQRCodeIdentification       (eMAId, PIN,                             Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="eMAId">An e-mobility account identification.</param>
        /// <param name="PIN">A password/PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromQRCodeIdentification(EMobilityAccount_Id  eMAId,
                                                                   String               PIN,
                                                                   I18NString           Description  = null)

            => new LocalAuthentication(QRCodeIdentification: new eMAIdWithPIN2(eMAId, PIN),
                                       Description:          Description);

        #endregion

        #region (static) FromQRCodeIdentification       (EVCOId, HashedPIN, Function, Salt = "", Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its hashed password.
        /// </summary>
        /// <param name="eMAId">An QR code identification.</param>
        /// <param name="HashedPIN">A hashed pin.</param>
        /// <param name="Function">A crypto function.</param>
        /// <param name="Salt">A salt of the crypto function.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromQRCodeIdentification(EMobilityAccount_Id  eMAId,
                                                                   String               HashedPIN,
                                                                   PINCrypto            Function,
                                                                   String               Salt          = "",
                                                                   I18NString           Description   = null)

            => new LocalAuthentication(QRCodeIdentification:  new eMAIdWithPIN2(eMAId,
                                                                                HashedPIN,
                                                                                Function,
                                                                                Salt),
                                       Description:           Description);

        #endregion

        #region (static) FromQRCodeIdentification       (QRCodeIdentification,                   Description = null)

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

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification,            Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification transmitted via PnC.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A PnC e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromPlugAndChargeIdentification(EMobilityAccount_Id  PlugAndChargeIdentification,
                                                                          I18NString           Description  = null)

            => new LocalAuthentication(PlugAndChargeIdentification: PlugAndChargeIdentification,
                                       Description:                 Description);

        #endregion

        #region (static) FromRemoteIdentification       (RemoteIdentification,                   Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromRemoteIdentification(EMobilityAccount_Id  RemoteIdentification,
                                                                   I18NString           Description  = null)

            => new LocalAuthentication(RemoteIdentification: RemoteIdentification,
                                       Description:          Description);


        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromRemoteIdentification(EMobilityAccount_Id?  RemoteIdentification,
                                                                   I18NString            Description  = null)

            => RemoteIdentification.HasValue
                   ? new LocalAuthentication(RemoteIdentification: RemoteIdentification,
                                             Description:          Description)
                   : null;

        #endregion

        #region (static) FromPIN                        (PIN,                                    Description = null)

        /// <summary>
        /// Create a new authentication info based on the given PIN.
        /// </summary>
        /// <param name="PIN">An PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromPIN(PIN          PIN,
                                                  I18NString?  Description   = null)

            => new (PIN:          PIN,
                    Description:  Description);

        #endregion

        #region (static) FromPublicKey                  (PublicKey,                              Description = null)

        /// <summary>
        /// Create a new authentication info based on the given public key.
        /// </summary>
        /// <param name="PublicKey">A public key.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromPublicKey(PublicKey    PublicKey,
                                                        I18NString?  Description   = null)

            => new (PublicKey:   PublicKey,
                    Description: Description);

        #endregion

        #region (static) FromCertificate                (Certificate,                            Description = null)

        /// <summary>
        /// Create a new authentication info based on the given certificate.
        /// </summary>
        /// <param name="Certificate">A certificate.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static LocalAuthentication FromCertificate(Certificate  Certificate,
                                                          I18NString?  Description   = null)

            => new (Certificate: Certificate,
                    Description: Description);

        #endregion


        public static LocalAuthentication Parse(JObject JSON)
        {

            var authToken                    = JSON["authToken"]?.                  Value<String>();
            var qrCodeIdentification         = JSON["QRCodeIdentification"] as JObject;
            var plugAndChargeIdentification  = JSON["plugAndChargeIdentification"]?.Value<String>();
            var remoteIdentification         = JSON["remoteIdentification"]?.       Value<String>();
            var pin                          = JSON["PIN"]?.                        Value<String>();
            var publicKey                    = JSON["publicKey"]?.                  Value<String>();
            var certificate                  = JSON["certificate"]?.                Value<String>();

            var authMethod                   = JSON["authMethod"]?.                 Value<String>();
            var description                  = JSON["description"] as JObject;

            return new(
                       authToken                   is not null ? AuthenticationToken.Parse(authToken) : null,
                       null, //JSON["QRCodeIdentification"]        != null ? eMAIdWithPIN2.      Parse(JSON["QRCodeIdentification"]?.       Value<String>()) : null,
                       plugAndChargeIdentification is not null ? EMobilityAccount_Id.Parse(plugAndChargeIdentification) : null,
                       remoteIdentification        is not null ? EMobilityAccount_Id.Parse(remoteIdentification)        : null,
                       pin                         is not null ? WWCP.PIN.           Parse(pin)                         : null,
                       publicKey                   is not null ? WWCP.PublicKey.     Parse(publicKey)                   : null,
                       certificate                 is not null ? WWCP.Certificate.   Parse(certificate)                 : null,

                       authMethod                  is not null ? null : null,
                       description                 is not null ? I18NString.         Parse(description)                 : I18NString.Empty
                   );

        }


        public RemoteAuthentication ToRemote

            => new (AuthToken,
                    QRCodeIdentification,
                    PlugAndChargeIdentification,
                    RemoteIdentification,
                    PIN,
                    PublicKey,
                    Certificate,

                    AuthMethod,
                    Description);

    }

    public class RemoteAuthentication : AAuthentication
    {

        #region Constructor(s)

        protected internal RemoteAuthentication(AuthenticationToken?  AuthToken                     = null,
                                                eMAIdWithPIN2?        QRCodeIdentification          = null,
                                                EMobilityAccount_Id?  PlugAndChargeIdentification   = null,
                                                EMobilityAccount_Id?  RemoteIdentification          = null,
                                                PIN?                  PIN                           = null,
                                                PublicKey?            PublicKey                     = null,
                                                Certificate?          Certificate                   = null,
                                                AuthMethod?           AuthMethod                    = null,
                                                I18NString?           Description                   = null)

            : base(AuthToken,
                   QRCodeIdentification,
                   PlugAndChargeIdentification,
                   RemoteIdentification,
                   PIN,
                   PublicKey,
                   Certificate,
                   AuthMethod,
                   Description)

        { }

        #endregion


        #region (static) FromAuthToken                  (AuthToken,                              Description = null)

        /// <summary>
        /// Create a new authentication info based on the given authentication token.
        /// </summary>
        /// <param name="AuthToken">An authentication token.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromAuthToken(AuthenticationToken  AuthToken,
                                                         AuthMethod?          AuthMethod    = null,
                                                         I18NString?          Description   = null)

            => new (AuthToken:    AuthToken,
                    AuthMethod:   AuthMethod,
                    Description:  Description);

        #endregion

        #region (static) FromQRCodeIdentification       (eMAId, PIN,                             Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="eMAId">An e-mobility account identification.</param>
        /// <param name="PIN">A password/PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromQRCodeIdentification(EMobilityAccount_Id  eMAId,
                                                                    String               PIN,
                                                                    I18NString           Description  = null)

            => new RemoteAuthentication(QRCodeIdentification: new eMAIdWithPIN2(eMAId, PIN),
                                        Description:          Description);

        #endregion

        #region (static) FromQRCodeIdentification       (EVCOId, HashedPIN, Function, Salt = "", Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its hashed password.
        /// </summary>
        /// <param name="eMAId">An QR code identification.</param>
        /// <param name="HashedPIN">A hashed pin.</param>
        /// <param name="Function">A crypto function.</param>
        /// <param name="Salt">A salt of the crypto function.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromQRCodeIdentification(EMobilityAccount_Id  eMAId,
                                                                    String               HashedPIN,
                                                                    PINCrypto            Function,
                                                                    String               Salt          = "",
                                                                    I18NString           Description   = null)

            => new RemoteAuthentication(QRCodeIdentification:  new eMAIdWithPIN2(eMAId,
                                                                                 HashedPIN,
                                                                                 Function,
                                                                                 Salt),
                                        Description:           Description);

        #endregion

        #region (static) FromQRCodeIdentification       (QRCodeIdentification,                   Description = null)

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

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification,            Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification transmitted via PnC.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A PnC e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromPlugAndChargeIdentification(EMobilityAccount_Id  PlugAndChargeIdentification,
                                                                           I18NString           Description  = null)

            => new RemoteAuthentication(PlugAndChargeIdentification: PlugAndChargeIdentification,
                                        Description:                 Description);

        #endregion

        #region (static) FromRemoteIdentification       (RemoteIdentification,                   Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromRemoteIdentification(EMobilityAccount_Id  RemoteIdentification,
                                                                    I18NString           Description  = null)

            => new RemoteAuthentication(RemoteIdentification: RemoteIdentification,
                                        Description:          Description);


        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromRemoteIdentification(EMobilityAccount_Id?  RemoteIdentification,
                                                                    I18NString            Description  = null)

            => RemoteIdentification.HasValue
                   ? new RemoteAuthentication(RemoteIdentification: RemoteIdentification,
                                              Description:          Description)
                   : null;

        #endregion

        #region (static) FromPIN                        (PIN,                                    Description = null)

        /// <summary>
        /// Create a new authentication info based on the given PIN.
        /// </summary>
        /// <param name="PIN">An PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromPIN(PIN          PIN,
                                                   I18NString?  Description   = null)

            => new (PIN:          PIN,
                    Description:  Description);

        #endregion

        #region (static) FromPublicKey                  (PublicKey,                              Description = null)

        /// <summary>
        /// Create a new authentication info based on the given public key.
        /// </summary>
        /// <param name="PublicKey">A public key.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromPublicKey(PublicKey    PublicKey,
                                                         I18NString?  Description   = null)

            => new (PublicKey:   PublicKey,
                    Description: Description);

        #endregion

        #region (static) FromCertificate                (Certificate,                            Description = null)

        /// <summary>
        /// Create a new authentication info based on the given certificate.
        /// </summary>
        /// <param name="Certificate">A certificate.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromCertificate(Certificate  Certificate,
                                                           I18NString?  Description   = null)

            => new (Certificate: Certificate,
                    Description: Description);

        #endregion


        public static RemoteAuthentication Parse(JObject JSON)
        {

            var authToken                    = JSON["authToken"]?.                  Value<String>();
            var qrCodeIdentification         = JSON["QRCodeIdentification"] as JObject;
            var plugAndChargeIdentification  = JSON["plugAndChargeIdentification"]?.Value<String>();
            var remoteIdentification         = JSON["remoteIdentification"]?.       Value<String>();
            var pin                          = JSON["PIN"]?.                        Value<String>();
            var publicKey                    = JSON["publicKey"]?.                  Value<String>();
            var certificate                  = JSON["certificate"]?.                Value<String>();

            var authMethod                   = JSON["authMethod"]?.                 Value<String>();
            var description                  = JSON["description"] as JObject;

            return new(
                       authToken                   is not null ? AuthenticationToken.Parse(authToken) : null,
                       null, //JSON["QRCodeIdentification"]        != null ? eMAIdWithPIN2.      Parse(JSON["QRCodeIdentification"]?.       Value<String>()) : null,
                       plugAndChargeIdentification is not null ? EMobilityAccount_Id.Parse(plugAndChargeIdentification) : null,
                       remoteIdentification        is not null ? EMobilityAccount_Id.Parse(remoteIdentification)        : null,
                       pin                         is not null ? WWCP.PIN.           Parse(pin)                         : null,
                       publicKey                   is not null ? WWCP.PublicKey.     Parse(publicKey)                   : null,
                       certificate                 is not null ? WWCP.Certificate.   Parse(certificate)                 : null,

                       authMethod                  is not null ? null : null,
                       description                 is not null ? I18NString.         Parse(description)                 : I18NString.Empty
                   );

        }

        public static Boolean TryParse(JObject                    JSON,
                                       out RemoteAuthentication?  RemoteAuthentication,
                                       out String?                ErrorResponse)
        {

            ErrorResponse = null;

            var authToken                    = JSON["authToken"]?.                  Value<String>();
            var qrCodeIdentification         = JSON["QRCodeIdentification"] as JObject;
            var plugAndChargeIdentification  = JSON["plugAndChargeIdentification"]?.Value<String>();
            var remoteIdentification         = JSON["remoteIdentification"]?.       Value<String>();
            var pin                          = JSON["PIN"]?.                        Value<String>();
            var publicKey                    = JSON["publicKey"]?.                  Value<String>();
            var certificate                  = JSON["certificate"]?.                Value<String>();

            var authMethod                   = JSON["authMethod"]?.                 Value<String>();
            var description                  = JSON["description"] as JObject;

            RemoteAuthentication = new(
                       authToken                   is not null ? AuthenticationToken.Parse(authToken) : null,
                       null, //JSON["QRCodeIdentification"]        != null ? eMAIdWithPIN2.      Parse(JSON["QRCodeIdentification"]?.       Value<String>()) : null,
                       plugAndChargeIdentification is not null ? EMobilityAccount_Id.Parse(plugAndChargeIdentification) : null,
                       remoteIdentification        is not null ? EMobilityAccount_Id.Parse(remoteIdentification)        : null,
                       pin                         is not null ? WWCP.PIN.           Parse(pin)                         : null,
                       publicKey                   is not null ? WWCP.PublicKey.     Parse(publicKey)                   : null,
                       certificate                 is not null ? WWCP.Certificate.   Parse(certificate)                 : null,

                       authMethod                  is not null ? null : null,
                       description                 is not null ? I18NString.         Parse(description)                 : I18NString.Empty
                   );

            return true;

        }


        public LocalAuthentication ToLocal

            => new (AuthToken,
                    QRCodeIdentification,
                    PlugAndChargeIdentification,
                    RemoteIdentification,
                    PIN,
                    PublicKey,
                    Certificate,

                    AuthMethod,
                    Description);

    }


}
