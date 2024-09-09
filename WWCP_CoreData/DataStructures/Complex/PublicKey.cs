/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OCPP <https://github.com/OpenChargingCloud/WWCP_OCPP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
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

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X509;
using System.Net.Http.Headers;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An asymmetric cryptographic public key.
    /// </summary>
    public class PublicKey : ACustomData,
                             IEquatable<PublicKey>
    {

        #region Properties

        /// <summary>
        /// The cryptographic public key.
        /// </summary>
        [Mandatory]
        public   Byte[]                  Value                 { get; }

        /// <summary>
        /// The optional cryptographic algorithm of the keys. Default is 'secp256r1'.
        /// </summary>
        [Optional]
        public   CryptoAlgorithm?        Algorithm             { get; }

        /// <summary>
        /// The optional serialization of the cryptographic keys. Default is 'raw'.
        /// </summary>
        [Optional]
        public   CryptoSerialization     Serialization         { get; }

        /// <summary>
        /// The optional encoding of the cryptographic keys. Default is 'base64'.
        /// </summary>
        [Optional]
        public   CryptoEncoding?         Encoding              { get; }


        public   X9ECParameters?         ECParameters          { get; }

        public   ECDomainParameters?     ECDomainParameters    { get; }


        internal ECPublicKeyParameters?  ParsedPublicKey       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCPP CSE asymmetric cryptographic public key.
        /// </summary>
        /// <param name="Value">The public key.</param>
        /// <param name="Algorithm">The optional cryptographic algorithm of the keys. Default is 'secp256r1'.</param>
        /// <param name="Serialization">The optional serialization of the cryptographic keys. Default is 'raw'.</param>
        /// <param name="Encoding">The optional encoding of the cryptographic keys. Default is 'base64'.</param>
        /// <param name="CustomData">An optional custom data object to allow to store any kind of customer specific data.</param>
        public PublicKey(Byte[]                  Value,
                         CryptoAlgorithm?        Algorithm            = null,
                         CryptoSerialization?    Serialization        = null,
                         CryptoEncoding?         Encoding             = null,
                         CustomData?             CustomData           = null,

                         X9ECParameters?         ECParameters         = null,
                         ECDomainParameters?     ECDomainParameters   = null,
                         ECPublicKeyParameters?  ParsedPublicKey      = null)

            : base(CustomData)

        {

            this.Value               = Value;
            this.Algorithm           = Algorithm;
            this.Serialization       = Serialization ?? CryptoSerialization.RAW;
            this.Encoding            = Encoding;

            this.ECParameters        = ECParameters;
            this.ECDomainParameters  = ECDomainParameters;
            this.ParsedPublicKey     = ParsedPublicKey;

            unchecked
            {

                hashCode = this.Value.        GetHashCode()       * 11 ^
                          (this.Algorithm?.   GetHashCode() ?? 0) *  7 ^
                           this.Serialization.GetHashCode()       *  5 ^
                          (this.Encoding?.    GetHashCode() ?? 0) *  3 ^
                           base.              GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // tba.

        #endregion

        #region (static) Parse       (Text)

        /// <summary>
        /// Parse the given text representation of a cryptographic public key.
        /// </summary>
        /// <param name="Text">The text to be parsed.</param>
        public static PublicKey Parse(String                Text,
                                      CryptoAlgorithm?      Algorithm       = null,
                                      CryptoSerialization?  Serialization   = null,
                                      CryptoEncoding?       Encoding        = null,
                                      CustomData?           CustomData      = null)
        {

            if (TryParse(Text,
                         out var publicKey,
                         out var errorResponse,
                         Algorithm,
                         Serialization,
                         Encoding,
                         CustomData))
            {
                return publicKey;
            }

            throw new ArgumentException("The given text representation of a public key is invalid: " + errorResponse,
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse    (Text, out PublicKey, out ErrorResponse, Algorithm = secp256r1, Serialization = raw, Encoding = base64, ...)

        /// <summary>
        /// Parse the given text representation of a cryptographic public key.
        /// </summary>
        /// <param name="Text">The text to be parsed.</param>
        public static Boolean TryParse(String                               Text,
                                       [NotNullWhen(true)]  out PublicKey?  PublicKey,
                                       [NotNullWhen(false)] out String?     ErrorResponse,
                                       CryptoAlgorithm?                     Algorithm       = null,
                                       CryptoSerialization?                 Serialization   = null,
                                       CryptoEncoding?                      Encoding        = null,
                                       CustomData?                          CustomData      = null)
        {

            PublicKey      = null;
            ErrorResponse  = null;

            #region HEX    encoding

            if (Encoding == CryptoEncoding.HEX)
            {

                if (!Text.TryParseHEX(out var hexByteArray1, out var errorResponse1))
                {
                    ErrorResponse = $"The given HEX encoding of a public key '{Text}' is invalid: " + errorResponse1;
                    return false;
                }

                if (TryParseASN1(hexByteArray1,
                                 out var publicKey,
                                 out var errorResponse2,
                                 CustomData))
                {
                    PublicKey = publicKey;
                }

            }

            #endregion

            #region BASE32 encoding

            else if (Encoding == CryptoEncoding.BASE32)
            {
                if (!Text.TryParseBASE32(out var base32ByteArray1, out var errorResponse1))
                {
                    ErrorResponse = $"The given base32 encoding of a public key '{Text}' is invalid: " + errorResponse1;
                    return false;
                }
            }

            #endregion

            #region BASE64 encoding

            else if (Encoding == CryptoEncoding.BASE64)
            {
                if (!Text.TryParseBASE64(out var base64ByteArray1, out var errorResponse1))
                {
                    ErrorResponse = $"The given base64 encoding of a public key '{Text}' is invalid: " + errorResponse1;
                    return false;
                }
            }

            #endregion

            #region ...or an unknown encoding!

            else
            {

                var publicKeys = new List<PublicKey>();

                #region Try to parse HEX (0-9, A-F, a-f)  // same as Base16

                if (Text.TryParseHEX(out var hexByteArray, out var errorResponse))
                {
                    try
                    {

                        if (TryParseASN1(hexByteArray,
                                         out var publicKey,
                                         out var errorResponse2,
                                         CustomData))
                        {
                            publicKeys.Add(
                                publicKey
                            );
                        }

                        else
                            publicKeys.Add(
                                new PublicKey(
                                    hexByteArray,
                                    Algorithm,
                                    Serialization,
                                    CryptoEncoding.HEX,
                                    CustomData
                                )
                            );

                    }
                    catch (Exception)
                    {
                        ErrorResponse = $"The given HEX encoding of a public key '{Text}' is invalid: " + errorResponse;
                        //return false;
                    }
                }

                #endregion

                #region Try to parse Base32

                if (Text.TryParseBASE32(out var base32ByteArray, out errorResponse))
                {
                    try
                    {

                        publicKeys.Add(
                            new PublicKey(
                                base32ByteArray,
                                Algorithm,
                                Serialization,
                                CryptoEncoding.BASE32,
                                CustomData
                            )
                        );

                    }
                    catch (Exception)
                    {
                        ErrorResponse = $"The given base32 encoding of a public key '{Text}' is invalid: " + errorResponse;
                        //return false;
                    }
                }

                #endregion

                // Base36: 0-9, A-Z
                // Base58: Which is Base64 without 0, O, I and l
                // Base62: Which is Base64 without + and /

                #region Try to parse Base64

                if (Text.TryParseBASE64(out var base64ByteArray, out errorResponse))
                {
                    try
                    {

                        publicKeys.Add(
                            new PublicKey(
                                base64ByteArray,
                                Algorithm,
                                Serialization,
                                CryptoEncoding.BASE64,
                                CustomData
                            )
                        );

                    }
                    catch (Exception)
                    {
                        ErrorResponse = $"The given base64 encoding of a public key '{Text}' is invalid: " + errorResponse;
                        //return false;
                    }
                }

                #endregion

                // Base85
                // Base91
                // Base92
                // Base100

                #region ...or not matches any encoding!

                if (publicKeys.Count == 0)
                {
                    ErrorResponse = $"The given public key '{Text}' could not be parsed!";
                    return false;
                }

                #endregion

                #region ...or ambiguous encodings!

                if (publicKeys.Count > 1)
                {

                    var hashSet = publicKeys.Select(pk => pk.Encoding).ToHashSet();

                    if (hashSet.Count == 2 &&
                        hashSet.Contains(CryptoEncoding.HEX) &&
                        hashSet.Contains(CryptoEncoding.BASE64))
                    {
                        PublicKey = publicKeys.First(publicKey => publicKey.Encoding == CryptoEncoding.HEX);
                    }

                    else if (hashSet.Count == 2 &&
                        hashSet.Contains(CryptoEncoding.BASE32) &&
                        hashSet.Contains(CryptoEncoding.BASE64))
                    {
                        PublicKey = publicKeys.First(publicKey => publicKey.Encoding == CryptoEncoding.BASE64);
                    }

                    else
                    {
                        ErrorResponse = $"The given public key '{Text}' could be parsed as multiple different encodings: {publicKeys.Select(publicKey => publicKey.Encoding.ToString()).AggregateWith(", ")}!";
                        return false;
                    }

                }

                #endregion

                else
                    PublicKey = publicKeys.First();

            }

            #endregion


            #region In case: Try to decode the public key using the given/parsed algorithm

            if (PublicKey is not null &&
                PublicKey.Algorithm.IsNotNullOrEmpty() &&
                PublicKey.ParsedPublicKey is null)
            {

                try
                {

                    var ecParameters        = ECNamedCurveTable.GetByName(PublicKey.Algorithm.ToString());
                    if (ecParameters is null)
                    {
                        ErrorResponse = $"The given cryptographic algorithm '{Algorithm}' is unknown!";
                        return false;
                    }

                    var ecDomainParameters  = new ECDomainParameters(
                                                  ecParameters.Curve,
                                                  ecParameters.G,
                                                  ecParameters.N,
                                                  ecParameters.H,
                                                  ecParameters.GetSeed()
                                              );

                    PublicKey = new PublicKey(

                                        PublicKey.Value,
                                        PublicKey.Algorithm,
                                        PublicKey.Serialization,
                                        PublicKey.Encoding,
                                        CustomData,

                                        ECParameters:        ecParameters,
                                        ECDomainParameters:  ecDomainParameters,
                                        ParsedPublicKey:     new ECPublicKeyParameters(
                                                                 "ECDSA",
                                                                 ecParameters.Curve.DecodePoint(PublicKey.Value),
                                                                 ecDomainParameters
                                                             )

                                    );

                }
                catch (Exception e)
                {
                    ErrorResponse = $"The given public key '{Text}' with algorithm '{Algorithm}' is invalid: {e.Message}";
                    return false;
                }

                return true;

            }

            #endregion


            return PublicKey is not null;

        }

        #endregion

        #region (static) Parse       (JSON, CustomPublicKeyParser = null)

        /// <summary>
        /// Parse the given JSON representation of a cryptographic public key.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomPublicKeyParser">An optional delegate to parse custom cryptographic public keys.</param>
        public static PublicKey Parse(JObject                                  JSON,
                                      CustomJObjectParserDelegate<PublicKey>?  CustomPublicKeyParser   = null)
        {

            if (TryParse(JSON,
                         out var publicKey,
                         out var errorResponse,
                         CustomPublicKeyParser))
            {
                return publicKey;
            }

            throw new ArgumentException("The given JSON representation of a public key is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse    (JSON, out PublicKey, out ErrorResponse, CustomPublicKeyParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a public key.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="PublicKey">The parsed connector type.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                              JSON,
                                       [NotNullWhen(true)]  out PublicKey?  PublicKey,
                                       [NotNullWhen(false)] out String?     ErrorResponse)

            => TryParse(JSON,
                        out PublicKey,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a public key.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="PublicKey">The parsed connector type.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPublicKeyParser">An optional delegate to parse custom public keys.</param>
        public static Boolean TryParse(JObject                                  JSON,
                                       [NotNullWhen(true)]  out PublicKey?      PublicKey,
                                       [NotNullWhen(false)] out String?         ErrorResponse,
                                       CustomJObjectParserDelegate<PublicKey>?  CustomPublicKeyParser   = null)
        {

            PublicKey = default;

            try
            {

                #region Value             [mandatory]

                if (!JSON.ParseMandatoryText("value",
                                             "public key value",
                                             out String? Value,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Algorithm         [optional]

                if (JSON.ParseOptional("algorithm",
                                       "crypto algorithm",
                                       CryptoAlgorithm.TryParse,
                                       out CryptoAlgorithm Algorithm,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Serialization     [optional]

                if (JSON.ParseOptional("serialization",
                                       "crypto serialization",
                                       CryptoSerialization.TryParse,
                                       out CryptoSerialization Serialization,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Encoding          [optional]

                if (JSON.ParseOptional("encoding",
                                       "crypto encoding",
                                       CryptoEncoding.TryParse,
                                       out CryptoEncoding Encoding,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region CustomData        [optional]

                if (JSON.ParseOptionalJSON("customData",
                                           "custom data",
                                           WWCP.CustomData.TryParse,
                                           out CustomData? CustomData,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                if (TryParse(Value,
                             out PublicKey,
                             out ErrorResponse,
                             Algorithm,
                             Serialization,
                             Encoding,
                             CustomData))
                {

                    if (CustomPublicKeyParser is not null)
                        PublicKey = CustomPublicKeyParser(JSON,
                                                          PublicKey);

                    return true;

                }

            }
            catch (Exception e)
            {
                ErrorResponse = "The given JSON representation of a public key is invalid: " + e.Message;
            }

            return false;

        }

        #endregion

        #region (static) TryParse    (ASN1, out PublicKey, out ErrorResponse, CustomData = null)
        public static Boolean TryParseASN1(Byte[]                               ASN1,
                                           [NotNullWhen(true)]  out PublicKey?  PublicKey,
                                           [NotNullWhen(false)] out String?     ErrorResponse,
                                           CustomData?                          CustomData   = null)
        {

            ErrorResponse = null;

            try
            {

                // Most likely an ASN.1 DER encoding sequence of a public key
                if (ASN1[0] != 0x30)
                {
                    PublicKey      = null;
                    ErrorResponse  = "The given ASN.1 DER representation of a public key is invalid!";
                    return false;
                }

                // 3056301006072A8648CE3D020106052B8104000A034200047D393CCB3FD83472E70717D32A26B17DAB7FF5D488451E7EC25024F8BF633A3A9E5591F7EA04BCE0B5ED9182C1454509966C92B07E293D59BA8F5E837904C60C

                var asn1Stream            = new Asn1InputStream(ASN1);
                var asn1Object            = asn1Stream.ReadObject();
                var subjectPublicKeyInfo  = SubjectPublicKeyInfo.GetInstance(asn1Object);
                var algorithmIdentifier   = subjectPublicKeyInfo.Algorithm;
                var publicKeyBytes        = subjectPublicKeyInfo.PublicKey.GetBytes();

                // Get the curve parameters (assuming this is a named curve)
                var x9ECParameters        = SecNamedCurves.GetByOid((DerObjectIdentifier) algorithmIdentifier.Parameters);
                var secNamedCurve         = SecNamedCurves.GetName ((DerObjectIdentifier) algorithmIdentifier.Parameters);
                var ecDomainParameters    = new ECDomainParameters(
                                                x9ECParameters.Curve,
                                                x9ECParameters.G,
                                                x9ECParameters.N,
                                                x9ECParameters.H,
                                                x9ECParameters.GetSeed()
                                            );

                PublicKey                 = new PublicKey(

                                                ASN1,
                                                CryptoAlgorithm.Parse(secNamedCurve),
                                                CryptoSerialization.DER,
                                                CryptoEncoding.HEX,
                                                CustomData,

                                                ECParameters:         x9ECParameters,
                                                ECDomainParameters:   ecDomainParameters,
                                                ParsedPublicKey:      new ECPublicKeyParameters(
                                                                            "ECDSA",
                                                                            x9ECParameters.Curve.DecodePoint(publicKeyBytes),
                                                                            ecDomainParameters
                                                                        )

                                            );

                return true;

            } catch (Exception e)
            {
                PublicKey      = null;
                ErrorResponse  = $"The given ASN.1 DER representation of a public key '{ASN1.ToHexString()}' is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPublicKeySerializer = null, CustomCustomDataSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPublicKeySerializer">A delegate to serialize cryptographic public keys.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PublicKey>?   CustomPublicKeySerializer    = null,
                              CustomJObjectSerializerDelegate<CustomData>?  CustomCustomDataSerializer   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("value",           Encoding.HasValue
                                                                      ? Encoding.Value.Encode(Value)
                                                                      : Value.ToBase64()),

                           Algorithm     == CryptoAlgorithm.    secp256r1
                               ? null
                               : new JProperty("algorithm",       Algorithm. ToString()),

                           Serialization == CryptoSerialization.RAW
                               ? null
                               : new JProperty("serialization",   Serialization),

                           Encoding      == CryptoEncoding.     BASE64
                               ? null
                               : new JProperty("encoding",        Encoding.  ToString()),

                           CustomData is not null
                               ? new JProperty("customData",      CustomData.ToJSON(CustomCustomDataSerializer))
                               : null

                       );

            return CustomPublicKeySerializer is not null
                       ? CustomPublicKeySerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this object.
        /// </summary>
        public PublicKey Clone()

            => new (

                   (Byte[]) Value.Clone(),

                   Algorithm?.   Clone,
                   Serialization.Clone,
                   Encoding?.    Clone,

                   CustomData

               );

        #endregion


        #region Operator overloading

        #region Operator == (PublicKey1, PublicKey2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PublicKey1">A public key.</param>
        /// <param name="PublicKey2">Another public key.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PublicKey? PublicKey1,
                                           PublicKey? PublicKey2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PublicKey1, PublicKey2))
                return true;

            // If one is null, but not both, return false.
            if (PublicKey1 is null || PublicKey2 is null)
                return false;

            return PublicKey1.Equals(PublicKey2);

        }

        #endregion

        #region Operator != (PublicKey1, PublicKey2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PublicKey1">A public key.</param>
        /// <param name="PublicKey2">Another public key.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PublicKey? PublicKey1,
                                           PublicKey? PublicKey2)

            => !(PublicKey1 == PublicKey2);

        #endregion

        #endregion

        #region IEquatable<PublicKey> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two public keys for equality.
        /// </summary>
        /// <param name="Object">A public key to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is PublicKey publicKey &&
                   Equals(publicKey);

        #endregion

        #region Equals(PublicKey)

        /// <summary>
        /// Compares two public keys for equality.
        /// </summary>
        /// <param name="PublicKey">A public key to compare with.</param>
        public Boolean Equals(PublicKey? PublicKey)

            => PublicKey is not null &&

               Value.        SequenceEqual(PublicKey.Value)         &&
               Algorithm.    Equals       (PublicKey.Algorithm)     &&
               Serialization.Equals       (PublicKey.Serialization) &&
               Encoding.     Equals       (PublicKey.Encoding)      &&
               base.         Equals       (PublicKey);

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
        {

            var parameters = new List<String>();

            if (Encoding.HasValue)
                parameters.Add(Encoding.Value.ToString());

            if (Serialization != CryptoSerialization.RAW)
                parameters.Add(Serialization.ToString());

            if (Algorithm.HasValue)
                parameters.Add(Algorithm.Value.ToString());

            return String.Concat(

                       Encoding.HasValue
                           ? Encoding.Value.Encode(Value)
                           : Value.ToBase64(),

                       parameters.Count > 0
                           ? $" [{parameters.AggregateWith(", ")}]"
                           : ""

                   );

        }

        #endregion


    }

}
