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
    /// Extensions methods for abstract user authentications.
    /// </summary>
    public static class AAuthenticationExtensions
    {

        public static Boolean IsDefined(this AAuthentication Authentication)

            => Authentication is not null &&
              (Authentication.AuthToken.                  HasValue ||
               Authentication.QRCodeIdentification.       HasValue ||
               Authentication.PlugAndChargeIdentification.HasValue ||
               Authentication.RemoteIdentification.       HasValue ||
               Authentication.PIN.                        HasValue ||
               Authentication.PublicKey                is not null ||
               Authentication.Certificate.                HasValue);


        public static Boolean IsNull(this AAuthentication Authentication)

            => !IsDefined(Authentication);

    }


    /// <summary>
    /// An abstract user authentication.
    /// </summary>
    public abstract class AAuthentication: IEquatable<AAuthentication>,
                                           IComparable<AAuthentication>,
                                           IComparable
    {

        #region Properties

        /// <summary>
        /// The authentication type.
        /// </summary>
        public AuthenticationType    AuthenticationType             { get; }

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
        public I18NString?          Description                     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new abstract user authentication.
        /// </summary>
        /// <param name="AuthenticationType">The authentication type.</param>
        /// <param name="AuthToken">An optional authentication token.</param>
        /// <param name="QRCodeIdentification">An optional QR-code identification.</param>
        /// <param name="PlugAndChargeIdentification">An optional Plug&Charge identification.</param>
        /// <param name="RemoteIdentification">An optional remote identification.</param>
        /// <param name="PIN">An optional PIN.</param>
        /// <param name="PublicKey">An optional public key.</param>
        /// <param name="Certificate">An optional certificate.</param>
        /// <param name="AuthMethod">An optional authentication method used.</param>
        /// <param name="Description">An optional multilingual description.</param>
        protected AAuthentication(AuthenticationType    AuthenticationType,
                                  AuthenticationToken?  AuthToken                     = null,
                                  eMAIdWithPIN2?        QRCodeIdentification          = null,
                                  EMobilityAccount_Id?  PlugAndChargeIdentification   = null,
                                  EMobilityAccount_Id?  RemoteIdentification          = null,
                                  PIN?                  PIN                           = null,
                                  PublicKey?            PublicKey                     = null,
                                  Certificate?          Certificate                   = null,
                                  AuthMethod?           AuthMethod                    = null,
                                  I18NString?           Description                   = null)
        {

            this.AuthenticationType           = AuthenticationType;
            this.AuthToken                    = AuthToken;
            this.QRCodeIdentification         = QRCodeIdentification;
            this.PlugAndChargeIdentification  = PlugAndChargeIdentification;
            this.RemoteIdentification         = RemoteIdentification;
            this.PIN                          = PIN;
            this.PublicKey                    = PublicKey;
            this.Certificate                  = Certificate;
            this.AuthMethod                   = AuthMethod;
            this.Description                  = Description;

            unchecked
            {

                hashCode = this.AuthenticationType.          GetHashCode()       * 27 ^
                          (this.AuthToken?.                  GetHashCode() ?? 0) * 23 ^
                          (this.QRCodeIdentification?.       GetHashCode() ?? 0) * 19 ^
                          (this.PlugAndChargeIdentification?.GetHashCode() ?? 0) * 17 ^
                          (this.RemoteIdentification?.       GetHashCode() ?? 0) * 13 ^
                          (this.PIN?.                        GetHashCode() ?? 0) * 11 ^
                          (this.PublicKey?.                  GetHashCode() ?? 0) *  7 ^
                          (this.Certificate?.                GetHashCode() ?? 0) *  5 ^
                          (this.AuthMethod?.                 GetHashCode() ?? 0) *  3 ^
                          (this.Description?.                GetHashCode() ?? 0);

            }

        }

        #endregion


        #region (static) Parse    (JSON, CustomRemoteAuthenticationParser = null)

        /// <summary>
        /// Parse the given JSON representation of an abstract authentication.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomAAuthenticationParser">A custom abstract authentication JSON parser.</param>
        /// <param name="CustomLocalAuthenticationParser">A custom local authentication JSON parser.</param>
        /// <param name="CustomRemoteAuthenticationParser">A custom remote authentication JSON parser.</param>
        public static AAuthentication Parse(JObject                                             JSON,
                                            CustomJObjectParserDelegate<AAuthentication>?       CustomAAuthenticationParser        = null,
                                            CustomJObjectParserDelegate<LocalAuthentication>?   CustomLocalAuthenticationParser    = null,
                                            CustomJObjectParserDelegate<RemoteAuthentication>?  CustomRemoteAuthenticationParser   = null)
        {

            if (TryParse(JSON,
                         out var authentication,
                         out var errorResponse,
                         CustomAAuthenticationParser,
                         CustomLocalAuthenticationParser,
                         CustomRemoteAuthenticationParser))
            {
                return authentication;
            }

            throw new ArgumentException("The given JSON representation of an abstract authentication is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out RemoteAuthentication, out ErrorResponse, CustomRemoteAuthenticationParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an abstract authentication.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Authentication">The parsed abstract authentication.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                    JSON,
                                       [NotNullWhen(true)]  out AAuthentication?  Authentication,
                                       [NotNullWhen(false)] out String?           ErrorResponse)

            => TryParse(JSON,
                        out Authentication,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an abstract authentication.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Authentication">The parsed abstract authentication.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomAAuthenticationParser">A custom abstract authentication JSON parser.</param>
        /// <param name="CustomLocalAuthenticationParser">A custom local authentication JSON parser.</param>
        /// <param name="CustomRemoteAuthenticationParser">A custom remote authentication JSON parser.</param>
        public static Boolean TryParse(JObject                                             JSON,
                                       [NotNullWhen(true)]  out AAuthentication?           Authentication,
                                       [NotNullWhen(false)] out String?                    ErrorResponse,
                                       CustomJObjectParserDelegate<AAuthentication>?       CustomAAuthenticationParser        = null,
                                       CustomJObjectParserDelegate<LocalAuthentication>?   CustomLocalAuthenticationParser    = null,
                                       CustomJObjectParserDelegate<RemoteAuthentication>?  CustomRemoteAuthenticationParser   = null)
        {

            try
            {

                Authentication  = null;
                ErrorResponse   = null;

                if (!AuthenticationTypeExtensions.TryParse(JSON["authenticationType"]?.Value<String>() ?? "", out var authenticationType) ||
                    authenticationType != AuthenticationType.Remote)
                {
                    ErrorResponse = "The given JSON representation does not contain a valid authentication type!";
                    return false;
                }

                switch (authenticationType)
                {

                    case AuthenticationType.Local:
                        if (!LocalAuthentication.TryParse(JSON,
                                                          out var localAuthentication,
                                                          out ErrorResponse,
                                                          CustomLocalAuthenticationParser))
                        {
                            ErrorResponse = "The given JSON representation of a local authentication is invalid: " + ErrorResponse;
                            return false;
                        }
                        Authentication = localAuthentication;
                        break;

                    case AuthenticationType.Remote:
                        if (!RemoteAuthentication.TryParse(JSON,
                                                           out var remoteAuthentication,
                                                           out ErrorResponse,
                                                           CustomRemoteAuthenticationParser))
                        {
                            ErrorResponse = "The given JSON representation of a remote authentication is invalid: " + ErrorResponse;
                            return false;
                        }
                        Authentication = remoteAuthentication;
                        break;

                    default:
                        ErrorResponse = "The given JSON representation contains an unsupported authentication type: " + authenticationType;
                        return false;

                }

                if (CustomAAuthenticationParser is not null)
                    Authentication = CustomAAuthenticationParser(JSON,
                                                                 Authentication);

                return true;

            }
            catch (Exception e)
            {
                Authentication  = default;
                ErrorResponse   = "The given JSON representation of an abstract authentication is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAAuthenticationSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomAAuthenticationSerializer">An optional delegate to serialize custom abstract authentications.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AAuthentication>? CustomAAuthenticationSerializer = null)

        {

            var json = JSONObject.Create(

                                 new JProperty("authenticationType",            AuthenticationType.               AsText()),

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

                           PublicKey is not null
                               ? new JProperty("publicKey",                     PublicKey.                  Value.ToString())
                               : null,

                           Certificate.HasValue
                               ? new JProperty("certificate",                   Certificate.                Value.ToString())
                               : null,

                           AuthMethod.HasValue
                               ? new JProperty("authMethod",                    AuthMethod.                 Value.ToString())
                               : null,

                           Description is not null && Description.IsNotNullOrEmpty()
                               ? new JProperty("description",                   Description.                      ToString())
                               : null

                       );

            return CustomAAuthenticationSerializer is not null
                       ? CustomAAuthenticationSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AAuthentication AAuthentication1,
                                           AAuthentication AAuthentication2)
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
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AAuthentication AAuthentication1,
                                           AAuthentication AAuthentication2)

            => !(AAuthentication1 == AAuthentication2);

        #endregion

        #region Operator <  (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (AAuthentication AAuthentication1,
                                          AAuthentication AAuthentication2)

            => AAuthentication1 is null
                  ? throw new ArgumentNullException(nameof(AAuthentication1), "The given AAuthentication1 must not be null!")
                  : AAuthentication1.CompareTo(AAuthentication2) < 0;

        #endregion

        #region Operator <= (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (AAuthentication AAuthentication1,
                                           AAuthentication AAuthentication2)

            => !(AAuthentication1 > AAuthentication2);

        #endregion

        #region Operator >  (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (AAuthentication AAuthentication1,
                                          AAuthentication AAuthentication2)

            => AAuthentication1 is null
                  ? throw new ArgumentNullException(nameof(AAuthentication1), "The given AAuthentication1 must not be null!")
                  : AAuthentication1.CompareTo(AAuthentication2) > 0;

        #endregion

        #region Operator >= (AAuthentication1, AAuthentication2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AAuthentication1">A AAuthentication.</param>
        /// <param name="AAuthentication2">Another AAuthentication.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (AAuthentication AAuthentication1,
                                           AAuthentication AAuthentication2)

            => !(AAuthentication1 < AAuthentication2);

        #endregion

        #endregion

        #region IComparable<AAuthentication> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two abstract authentications.
        /// </summary>
        /// <param name="Object">An abstract authentication to compare with.</param>
        public Int32 CompareTo(Object? Object)

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

            if (PublicKey is not null                && AAuthentication.PublicKey is not null)
                return PublicKey.                  Value.ToHexString().CompareTo(AAuthentication.PublicKey.    Value.ToHexString());

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

             ((PublicKey   is null                  &&  AAuthentication.PublicKey is null)                    ||
              (PublicKey   is not null              &&  AAuthentication.PublicKey is not null                && PublicKey.                        Equals(AAuthentication.PublicKey                        ))) &&

            ((!Certificate.                HasValue && !AAuthentication.Certificate.                HasValue) ||
              (Certificate.                HasValue &&  AAuthentication.Certificate.                HasValue && Certificate.                Value.Equals(AAuthentication.Certificate.                Value))) &&

            ((!AuthMethod.                 HasValue && !AAuthentication.AuthMethod.                 HasValue) ||
              (AuthMethod.                 HasValue &&  AAuthentication.AuthMethod.                 HasValue && AuthMethod.                 Value.Equals(AAuthentication.AuthMethod.                 Value))) &&

             ((Description is null                  &&  AAuthentication.AuthMethod is null)                   ||
              (Description is not null              &&  AAuthentication.AuthMethod is not null               && Description.                      Equals(AAuthentication.Description)));

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

                   PublicKey is not null
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

}
