/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A remote authentication.
    /// </summary>
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

            : base(AuthenticationType.Remote,
                   AuthToken,
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


        #region (static) Parse    (JSON, CustomRemoteAuthenticationParser = null)

        /// <summary>
        /// Parse the given JSON representation of a remote authentication.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomRemoteAuthenticationParser">A custom remote authentication JSON parser.</param>
        public static RemoteAuthentication Parse(JObject                                             JSON,
                                                 CustomJObjectParserDelegate<RemoteAuthentication>?  CustomRemoteAuthenticationParser   = null)
        {

            if (TryParse(JSON,
                         out var remoteAuthentication,
                         out var errorResponse,
                         CustomRemoteAuthenticationParser))
            {
                return remoteAuthentication;
            }

            throw new ArgumentException("The given JSON representation of a remote authentication is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out RemoteAuthentication, out ErrorResponse, CustomRemoteAuthenticationParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a remote authentication.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RemoteAuthentication">The parsed remote authentication.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                         JSON,
                                       [NotNullWhen(true)]  out RemoteAuthentication?  RemoteAuthentication,
                                       [NotNullWhen(false)] out String?                ErrorResponse)

            => TryParse(JSON,
                        out RemoteAuthentication,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a remote authentication.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RemoteAuthentication">The parsed remote authentication.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomRemoteAuthenticationParser">A custom remote authentication JSON parser.</param>
        public static Boolean TryParse(JObject                                             JSON,
                                       [NotNullWhen(true)]  out RemoteAuthentication?      RemoteAuthentication,
                                       [NotNullWhen(false)] out String?                    ErrorResponse,
                                       CustomJObjectParserDelegate<RemoteAuthentication>?  CustomRemoteAuthenticationParser   = null)
        {

            try
            {

                RemoteAuthentication  = null;
                ErrorResponse         = null;

                if (!AuthenticationTypeExtensions.TryParse(JSON["authenticationType"]?.Value<String>() ?? "", out var authenticationType) ||
                    authenticationType != AuthenticationType.Remote)
                {
                    ErrorResponse = "The given JSON representation does not contain a valid remote authentication!";
                    return false;
                }

                var authToken                    = JSON["authToken"]?.                  Value<String>();
                var qrCodeIdentification         = JSON["QRCodeIdentification"] as JObject;
                var plugAndChargeIdentification  = JSON["plugAndChargeIdentification"]?.Value<String>();
                var remoteIdentification         = JSON["remoteIdentification"]?.       Value<String>();
                var pin                          = JSON["PIN"]?.                        Value<String>();
                var publicKey                    = JSON["publicKey"]?.                  Value<String>();
                var certificate                  = JSON["certificate"]?.                Value<String>();

                var authMethod                   = JSON["authMethod"]?.                 Value<String>();
                var description                  = JSON["description"] as JObject;

                RemoteAuthentication = new (
                                           authToken                   is not null ? AuthenticationToken.Parse(authToken) : null,
                                           null, //JSON["QRCodeIdentification"]        is not null ? eMAIdWithPIN2.      Parse(JSON["QRCodeIdentification"]?.       Value<String>()) : null,
                                           plugAndChargeIdentification is not null ? EMobilityAccount_Id.Parse(plugAndChargeIdentification) : null,
                                           remoteIdentification        is not null ? EMobilityAccount_Id.Parse(remoteIdentification)        : null,
                                           pin                         is not null ? WWCP.PIN.           Parse(pin)                         : null,
                                           publicKey                   is not null ? WWCP.ECCPublicKey.  ParseASN1(publicKey)               : null,
                                           certificate                 is not null ? WWCP.Certificate.   Parse(certificate)                 : null,

                                           authMethod                  is not null ? null : null,
                                           description                 is not null ? I18NString.         Parse(description)                 : I18NString.Empty
                                       );

                if (CustomRemoteAuthenticationParser is not null)
                    RemoteAuthentication = CustomRemoteAuthenticationParser(JSON,
                                                                            RemoteAuthentication);

                return true;

            }
            catch (Exception e)
            {
                RemoteAuthentication  = default;
                ErrorResponse         = "The given JSON representation of a remote authentication is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomRemoteAuthenticationSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomRemoteAuthenticationSerializer">An optional to serialize custom remote authentications.</param>
        /// <param name="CustomAAuthenticationSerializer">An optional delegate to serialize custom authentications.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<RemoteAuthentication>?  CustomRemoteAuthenticationSerializer   = null,
                              CustomJObjectSerializerDelegate<AAuthentication>?       CustomAAuthenticationSerializer        = null)

        {

            var json = base.ToJSON(CustomAAuthenticationSerializer);

            return CustomRemoteAuthenticationSerializer is not null
                       ? CustomRemoteAuthenticationSerializer(this, json)
                       : json;

        }

        #endregion


        #region (static) FromAuthToken                   (AuthToken,                              Description = null)

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

        #region (static) FromQRCodeIdentification        (eMAId, PIN,                             Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="eMAId">An e-mobility account identification.</param>
        /// <param name="PIN">A password/PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromQRCodeIdentification(EMobilityAccount_Id  eMAId,
                                                                    String               PIN,
                                                                    I18NString?          Description   = null)

            => new (
                   QRCodeIdentification:  new eMAIdWithPIN2(
                                              eMAId,
                                              PIN
                                          ),
                   Description:           Description
               );

        #endregion

        #region (static) FromQRCodeIdentification        (EVCOId, HashedPIN, Function, Salt = "", Description = null)

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
                                                                    I18NString?          Description   = null)

            => new (
                   QRCodeIdentification:  new eMAIdWithPIN2(
                                              eMAId,
                                              HashedPIN,
                                              Function,
                                              Salt
                                          ),
                    Description:          Description
               );

        #endregion

        #region (static) FromQRCodeIdentification        (QRCodeIdentification,                   Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification and its password.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR code identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromQRCodeIdentification(eMAIdWithPIN2  QRCodeIdentification,
                                                                    I18NString?    Description   = null)

            => new (
                   QRCodeIdentification:  QRCodeIdentification,
                   Description:           Description
               );

        #endregion

        #region (static) FromPlugAndChargeIdentification (PlugAndChargeIdentification,            Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification transmitted via PnC.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A PnC e-mobility account identification.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromPlugAndChargeIdentification(EMobilityAccount_Id  PlugAndChargeIdentification,
                                                                           I18NString?          Description   = null)

            => new (
                   PlugAndChargeIdentification:  PlugAndChargeIdentification,
                   Description:                  Description
               );

        #endregion

        #region (static) FromRemoteIdentification        (RemoteIdentification,                   Description = null)

        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="AuthMethod">The optional authentication method used.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromRemoteIdentification(EMobilityAccount_Id  RemoteIdentification,
                                                                    AuthMethod?          AuthMethod    = null,
                                                                    I18NString?          Description   = null)

            => new (
                   RemoteIdentification:  RemoteIdentification,
                   AuthMethod:            AuthMethod,
                   Description:           Description
               );


        /// <summary>
        /// Create a new authentication info based on the given
        /// e-mobility account identification.
        /// </summary>
        /// <param name="RemoteIdentification">An e-mobility account identification.</param>
        /// <param name="AuthMethod">The optional authentication method used.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication? FromRemoteIdentification(EMobilityAccount_Id?  RemoteIdentification,
                                                                     AuthMethod?           AuthMethod    = null,
                                                                     I18NString?           Description   = null)

            => RemoteIdentification.HasValue
                   ? new RemoteAuthentication(
                         RemoteIdentification:  RemoteIdentification,
                         AuthMethod:            AuthMethod,
                         Description:           Description
                     )
                   : null;

        #endregion

        #region (static) FromPIN                         (PIN,                                    Description = null)

        /// <summary>
        /// Create a new authentication info based on the given PIN.
        /// </summary>
        /// <param name="PIN">An PIN.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromPIN(PIN          PIN,
                                                   I18NString?  Description   = null)

            => new (
                   PIN:          PIN,
                   Description:  Description
               );

        #endregion

        #region (static) FromPublicKey                   (PublicKey,                              Description = null)

        /// <summary>
        /// Create a new authentication info based on the given public key.
        /// </summary>
        /// <param name="PublicKey">A public key.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromPublicKey(PublicKey    PublicKey,
                                                         I18NString?  Description   = null)

            => new (
                   PublicKey:   PublicKey,
                   Description: Description
               );

        #endregion

        #region (static) FromCertificate                 (Certificate,                            Description = null)

        /// <summary>
        /// Create a new authentication info based on the given certificate.
        /// </summary>
        /// <param name="Certificate">A certificate.</param>
        /// <param name="Description">An optional multilingual description.</param>
        public static RemoteAuthentication FromCertificate(Certificate  Certificate,
                                                           I18NString?  Description   = null)

            => new (
                   Certificate: Certificate,
                   Description: Description
               );

        #endregion


        #region AsLocalAuthentication()

        /// <summary>
        /// Convert this remote authentication to a local authentication.
        /// </summary>
        public LocalAuthentication AsLocalAuthentication()

            => new (AuthToken,
                    QRCodeIdentification,
                    PlugAndChargeIdentification,
                    RemoteIdentification,
                    PIN,
                    PublicKey,
                    Certificate,

                    AuthMethod,
                    Description);

        #endregion

    }

}
