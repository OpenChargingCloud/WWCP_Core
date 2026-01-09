/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An asymmetric elliptic curve cryptographic public key.
    /// </summary>
    public class ECCPublicKey : PublicKey
    {

        #region Data

        public const String Context = "https://open.charging.cloud/context/cryptography/ecc/publicKey";

        private readonly static List<(Func<String, (Boolean Success, Byte[]? Bytes, String? Error)> TryParse, CryptoEncoding Encoding)>  parsers1;
        private readonly static Dictionary<String, Func<String, (Boolean Success, Byte[]? Bytes, String? Error)>>                        parsers2;
        private readonly static List<String>                                                                                             algorithms;

        #endregion

        #region Properties

        public  X9ECParameters?         ECParameters          { get; }

        public  ECDomainParameters?     ECDomainParameters    { get; }


        public  ECPublicKeyParameters?  ParsedPublicKey       { get; }


        public  Byte[]                  X
            => ParsedPublicKey?.Q.XCoord.ToBigInteger().ToByteArray() ?? [];

        public  Byte[]                  Y
            => ParsedPublicKey?.Q.YCoord.ToBigInteger().ToByteArray() ?? [];

        public  Byte[]                  XY
            => ParsedPublicKey?.Q.GetEncoded()                        ?? [];


        public Byte[]                   ASN1_DER
            => SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(ParsedPublicKey).ToAsn1Object().GetEncoded();

        #endregion

        #region Constructor(s)

        #region (static)   ECCPublicKey()

        static ECCPublicKey()
        {

            parsers1    = [
                              (key => (key.TryParseBASE64(out var bytes, out var error), bytes, error), CryptoEncoding.BASE64),
                              (key => (key.TryParseHEX   (out var bytes, out var error), bytes, error), CryptoEncoding.HEX),
                              (key => (key.TryParseBASE32(out var bytes, out var error), bytes, error), CryptoEncoding.BASE32)
                          ];

            parsers2    = new Dictionary<String, Func<String, (Boolean Success, Byte[]? Bytes, String? Error)>> {
                              { "base64", key => (key.TryParseBASE64(out var bytes, out var error), bytes, error) },
                              { "base32", key => (key.TryParseBASE32(out var bytes, out var error), bytes, error) },
                              { "hex",    key => (key.TryParseHEX   (out var bytes, out var error), bytes, error) }
                          };

            algorithms  = [
                              CryptoAlgorithm.Secp192r1.ToString(),
                              CryptoAlgorithm.Secp256r1.ToString(),
                              CryptoAlgorithm.Secp256k1.ToString(),
                              CryptoAlgorithm.Secp384r1.ToString(),
                              CryptoAlgorithm.Secp521r1.ToString()
                          ];

        }

        #endregion

        #region (internal) ECCPublicKey(Value, Algorithm = secp256r1, Serialization = raw, Encoding = base64, ...)

        /// <summary>
        /// Create a new asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="Value">The public key.</param>
        /// <param name="Algorithm">An optional cryptographic algorithm of the ECC public key. Default is 'secp256r1'.</param>
        /// <param name="Serialization">An optional serialization of the ECC public key. Default is 'raw'.</param>
        /// <param name="Encoding">An optional encoding of the ECC public key. Default is 'base64'.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        internal ECCPublicKey(Byte[]                  Value,
                              CryptoAlgorithm?        Algorithm            = null,
                              CryptoSerialization?    Serialization        = null,
                              CryptoEncoding?         Encoding             = null,
                              CustomData?             CustomData           = null,

                              X9ECParameters?         ECParameters         = null,
                              ECDomainParameters?     ECDomainParameters   = null,
                              ECPublicKeyParameters?  ParsedPublicKey      = null)

            : base(Value,
                   Algorithm,
                   Serialization,
                   Encoding,
                   CustomData)

        {

            this.ECParameters        = ECParameters;
            this.ECDomainParameters  = ECDomainParameters;
            this.ParsedPublicKey     = ParsedPublicKey;

        }

        #endregion

        #endregion


        #region Documentation

        // tba.

        #endregion

        #region (static) Parse        (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomECCPublicKeyParser">An optional delegate to parse custom ECC public keys.</param>
        public static ECCPublicKey Parse(JObject                                     JSON,
                                         CustomJObjectParserDelegate<ECCPublicKey>?  CustomECCPublicKeyParser   = null)
        {

            if (TryParse(JSON,
                         out var eccPublicKey,
                         out var errorResponse,
                         CustomECCPublicKeyParser))
            {
                return eccPublicKey;
            }

            throw new ArgumentException("The given JSON representation of an ECC public key is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse     (JSON,      out ECCPublicKey, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ECCPublicKey">The parsed ECC public key.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                 JSON,
                                       [NotNullWhen(true)]  out ECCPublicKey?  ECCPublicKey,
                                       [NotNullWhen(false)] out String?        ErrorResponse)

            => TryParse(JSON,
                        out ECCPublicKey,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ECCPublicKey">The parsed ECC public key.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomECCPublicKeyParser">An optional delegate to parse custom ECC public keys.</param>
        public static Boolean TryParse(JObject                                     JSON,
                                       [NotNullWhen(true)]  out ECCPublicKey?      ECCPublicKey,
                                       [NotNullWhen(false)] out String?            ErrorResponse,
                                       CustomJObjectParserDelegate<ECCPublicKey>?  CustomECCPublicKeyParser   = null)
        {

            ECCPublicKey = default;

            try
            {

                #region Value or X/Y      [mandatory]

                var Value = JSON["value"]?.Value<String>()?.Trim();

                var X     = JSON["x"]?.    Value<String>()?.Trim();
                var Y     = JSON["y"]?.    Value<String>()?.Trim();

                if (Value.IsNullOrEmpty() &&
                   (X.    IsNullOrEmpty() ||
                    Y.    IsNullOrEmpty()))
                {
                    ErrorResponse = "The given JSON representation of a public key is invalid: Either a 'value' or 'x' and 'y' coordinates must be given!";
                    return false;
                }

                #endregion

                #region Algorithm         [optional]

                if (JSON.ParseOptional("algorithm",
                                       "crypto algorithm",
                                       CryptoAlgorithm.TryParse,
                                       out CryptoAlgorithm? Algorithm,
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
                                       out CryptoSerialization? Serialization,
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
                                       out CryptoEncoding? Encoding,
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


                // Value
                if (Value.IsNotNullOrEmpty() &&
                    TryParseASN1(Value,
                                 out ECCPublicKey,
                                 out ErrorResponse,
                                 Algorithm,
                                 Serialization,
                                 Encoding,
                                 CustomData))
                {

                    if (CustomECCPublicKeyParser is not null)
                        ECCPublicKey = CustomECCPublicKeyParser(JSON,
                                                                ECCPublicKey);

                    return true;

                }

                // X and Y
                if (X.IsNotNullOrEmpty() &&
                    Y.IsNotNullOrEmpty() &&
                    TryParseXY(X,
                               Y,
                               out ECCPublicKey,
                               out ErrorResponse,
                               Algorithm,
                               Encoding,
                               CustomData))
                {

                    if (CustomECCPublicKeyParser is not null)
                        ECCPublicKey = CustomECCPublicKeyParser(JSON,
                                                                ECCPublicKey);

                    return true;

                }

            }
            catch (Exception e)
            {
                ErrorResponse = "The given JSON representation of a public key is invalid: " + e.Message;
            }

            ErrorResponse = "The given JSON representation of a public key is invalid: Either a 'value' or 'x' and 'y' coordinates must be given!";
            return false;

        }

        #endregion


        #region (static) Parse        (PublicKey,                                      Algorithm = secp256r1, Encoding = base64, ...)

        /// <summary>
        /// Parse the given text representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="PublicKey">The text representation of the public key.</param>
        /// <param name="Algorithm">The optional cryptographic algorithm of the keys. Default is 'secp256r1'.</param>
        /// <param name="Encoding">The optional encoding of the cryptographic keys. Default is 'base64'.</param>
        /// <param name="AutoDetectAlgorithmUsed">Whether to try to automatically detect the algorithm used for the public key, if not specified.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public static ECCPublicKey Parse(String            PublicKey,
                                         CryptoAlgorithm?  Algorithm                 = null,
                                         CryptoEncoding?   Encoding                  = null,
                                         Boolean           AutoDetectAlgorithmUsed   = false,
                                         CustomData?       CustomData                = null)
        {

            if (TryParse(PublicKey,
                         out var eccPublicKey,
                         out var errorResponse,
                         Algorithm,
                         Encoding,
                         AutoDetectAlgorithmUsed,
                         CustomData))
            {
                return eccPublicKey;
            }

            throw new ArgumentException("The given text representation of an ECC public key is invalid: " + errorResponse);

        }

        #endregion

        #region (static) TryParse     (PublicKey, out ECCPublicKey, out ErrorResponse, Algorithm = secp256r1, Encoding = base64, ...)

        /// <summary>
        /// Try to parse the given text representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="PublicKey">The text representation of the public key.</param>
        /// <param name="Algorithm">The optional cryptographic algorithm of the keys. Default is 'secp256r1'.</param>
        /// <param name="Encoding">The optional encoding of the cryptographic keys. Default is 'base64'.</param>
        /// <param name="AutoDetectAlgorithmUsed">Whether to try to automatically detect the algorithm used for the public key, if not specified.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public static Boolean TryParse(String                                  PublicKey,
                                       [NotNullWhen(true)]  out ECCPublicKey?  ECCPublicKey,
                                       [NotNullWhen(false)] out String?        ErrorResponse,
                                       CryptoAlgorithm?                        Algorithm                 = null,
                                       CryptoEncoding?                         Encoding                  = null,
                                       Boolean                                 AutoDetectAlgorithmUsed   = false,
                                       CustomData?                             CustomData                = null)
        {

            #region Initial checks

            ECCPublicKey   = null;
            ErrorResponse  = null;

            if (PublicKey.IsNullOrEmpty())
            {
                ErrorResponse = "The given encoded public key must not be null or empty!";
                return false;
            }

            #endregion

            try
            {

                var publicKey = Array.Empty<Byte>();

                if (!Encoding.HasValue)
                {
                    foreach (var (tryParse, encoding) in parsers1)
                    {

                        var (success, bytes, errorResponse) = tryParse(PublicKey);

                        if (success)
                        {
                            Encoding  = encoding;
                            publicKey = bytes;
                            break;
                        }

                    }
                }

                else
                {

                    if (!parsers2.TryGetValue(Encoding.Value.ToString().ToLower(), out var parser))
                    {
                        ErrorResponse = $"The encoding '{Encoding}' is not supported.";
                        return false;
                    }

                    var (success, publicKeyBytes, errorResponse) = parser(PublicKey);
                    if (!success)
                    {
                        ErrorResponse = $"Invalid public key format for '{Encoding.Value}' encoding: {errorResponse}";
                        return false;
                    }

                    publicKey = publicKeyBytes;

                }

                if (publicKey is null)
                {
                    ErrorResponse = $"The given encoded public key '{PublicKey}' could not be parsed!";
                    return false;
                }


                if (Algorithm.HasValue)
                {

                    var ecParameters = ECNamedCurveTable.GetByName(Algorithm.Value.ToString());

                    if (ecParameters is not null)
                    {

                        var ecDomainParameters  = new ECDomainParameters(
                                                      ecParameters.Curve,
                                                      ecParameters.G,
                                                      ecParameters.N,
                                                      ecParameters.H,
                                                      ecParameters.GetSeed()
                                                  );

                        var parsedPublicKey     = new ECPublicKeyParameters(
                                                      "ECDSA",
                                                      ecParameters.Curve.DecodePoint(publicKey),
                                                      ecDomainParameters
                                                  );

                        ECCPublicKey            = new ECCPublicKey(

                                                      publicKey,
                                                      Algorithm,
                                                      null, //CryptoSerialization.ASN1_DER,
                                                      Encoding,
                                                      CustomData,

                                                      ecParameters,
                                                      ecDomainParameters,
                                                      parsedPublicKey

                                                  );

                        return true;

                    }

                }

                else if (AutoDetectAlgorithmUsed)
                {
                    foreach (var algorithm in algorithms)
                    {
                        try
                        {

                            var ecParameters = ECNamedCurveTable.GetByName(algorithm);

                            if (ecParameters is not null)
                            {

                                var ecDomainParameters  = new ECDomainParameters(
                                                              ecParameters.Curve,
                                                              ecParameters.G,
                                                              ecParameters.N,
                                                              ecParameters.H,
                                                              ecParameters.GetSeed()
                                                          );

                                var parsedPublicKey     = new ECPublicKeyParameters(
                                                              "ECDSA",
                                                              ecParameters.Curve.DecodePoint(publicKey),
                                                              ecDomainParameters
                                                          );

                                ECCPublicKey            = new ECCPublicKey(

                                                              publicKey,
                                                              Algorithm,
                                                              null, //CryptoSerialization.ASN1_DER,
                                                              Encoding,
                                                              CustomData,

                                                              ecParameters,
                                                              ecDomainParameters,
                                                              parsedPublicKey

                                                          );

                                return true;

                            }

                        }
                        catch
                        { }
                    }
                }


                // Just store the byte array...
                ECCPublicKey = new ECCPublicKey(
                                   publicKey,
                                   Algorithm,
                                   Encoding:    Encoding,
                                   CustomData:  CustomData
                               );

                return true;

            } catch (Exception e)
            {
                ErrorResponse  = $"The given text representation '{PublicKey}' of an ECC public key is invalid: " + e.Message;
                return false;
            }

        }

        #endregion


        #region (static) ParseXY      (X, Y,                                           Algorithm = secp256r1, Encoding = base64, ...)

        /// <summary>
        /// Parse the given X/Y text representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="X">The text representation of the x-coordinate of the public key.</param>
        /// <param name="Y">The text representation of the y-coordinate of the public key.</param>
        /// <param name="Algorithm">The optional cryptographic algorithm of the keys. Default is 'secp256r1'.</param>
        /// <param name="Encoding">The optional encoding of the cryptographic keys. Default is 'base64'.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public static ECCPublicKey ParseXY(String            X,
                                           String            Y,
                                           CryptoAlgorithm?  Algorithm    = null,
                                           CryptoEncoding?   Encoding     = null,
                                           CustomData?       CustomData   = null)
        {

            if (TryParseXY(X,
                           Y,
                           out var eccPublicKey,
                           out var errorResponse,
                           Algorithm,
                           Encoding,
                           CustomData))
            {
                return eccPublicKey;
            }

            throw new ArgumentException("The given X/Y-representation of an ECC public key is invalid: " + errorResponse);

        }

        #endregion

        #region (static) TryParseXY   (X, Y,      out ECCPublicKey, out ErrorResponse, Algorithm = secp256r1, Encoding = base64, ...)

        /// <summary>
        /// Try to parse the given X/Y text representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="X">The text representation of the x-coordinate of the public key.</param>
        /// <param name="Y">The text representation of the y-coordinate of the public key.</param>
        /// <param name="Algorithm">The optional cryptographic algorithm of the keys. Default is 'secp256r1'.</param>
        /// <param name="Encoding">The optional encoding of the cryptographic keys. Default is 'base64'.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public static Boolean TryParseXY(String                                  X,
                                         String                                  Y,
                                         [NotNullWhen(true)]  out ECCPublicKey?  ECCPublicKey,
                                         [NotNullWhen(false)] out String?        ErrorResponse,
                                         CryptoAlgorithm?                        Algorithm    = null,
                                         CryptoEncoding?                         Encoding     = null,
                                         CustomData?                             CustomData   = null)
        {

            #region Initial checks

            ECCPublicKey = null;

            if (X.IsNullOrEmpty())
            {
                ErrorResponse = "The given x-coordinate must not be null or empty!";
                return false;
            }

            if (Y.IsNullOrEmpty())
            {
                ErrorResponse = "The given y-coordinate must not be null or empty!";
                return false;
            }

            #endregion

            try
            {

                var x = Array.Empty<Byte>();
                var y = Array.Empty<Byte>();

                if     (!Encoding.HasValue ||
                         Encoding == CryptoEncoding.BASE64)
                {
                    x = X.FromBASE64();
                    y = Y.FromBASE64();
                }

                else if (Encoding == CryptoEncoding.BASE32)
                {
                    x = X.FromBASE32();
                    y = Y.FromBASE32();
                }

                else if (Encoding == CryptoEncoding.HEX)
                {
                    x = X.FromHEX();
                    y = Y.FromHEX();
                }

                else
                {
                    ErrorResponse = "The given encoding of the public key is unknown!";
                    return false;
                }

                return TryParseXY(
                           x,
                           y,
                           Algorithm ?? CryptoAlgorithm.Secp256r1,
                           out ECCPublicKey,
                           out ErrorResponse,
                           CustomData
                       );

            } catch (Exception e)
            {
                ErrorResponse  = $"The given X/Y-representation '{X}'/'{Y}' of an ECC public key is invalid: " + e.Message;
            }

            return false;

        }


        /// <summary>
        /// Try to parse the given X/Y representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="X">The x-coordinate of the public key.</param>
        /// <param name="Y">The y-coordinate of the public key.</param>
        /// <param name="Algorithm">The optional cryptographic algorithm of the keys. Default is 'secp256r1'.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public static Boolean TryParseXY(Byte[]                                  X,
                                         Byte[]                                  Y,
                                         CryptoAlgorithm                         Algorithm,
                                         [NotNullWhen(true)]  out ECCPublicKey?  ECCPublicKey,
                                         [NotNullWhen(false)] out String?        ErrorResponse,
                                         CustomData?                             CustomData   = null)
        {

            #region Initial checks

            ECCPublicKey   = null;
            ErrorResponse  = null;

            if (X.IsNullOrEmpty())
            {
                ErrorResponse = "The given x-coordinate must not be null or empty!";
                return false;
            }

            if (Y.IsNullOrEmpty())
            {
                ErrorResponse = "The given y-coordinate must not be null or empty!";
                return false;
            }

            #endregion

            try
            {

                var encodedPublicKey    = new Byte[1 + X.Length + Y.Length];
                encodedPublicKey[0]     = 0x04;
                Array.Copy(X, 0, encodedPublicKey, 1,            X.Length);
                Array.Copy(Y, 0, encodedPublicKey, 1 + X.Length, Y.Length);

                var ecParameters        = ECNamedCurveTable.GetByName(Algorithm.ToString());

                var ecDomainParameters  = new ECDomainParameters(
                                              ecParameters.Curve,
                                              ecParameters.G,
                                              ecParameters.N,
                                              ecParameters.H,
                                              ecParameters.GetSeed()
                                          );

                var parsedPublicKey     = new ECPublicKeyParameters(
                                              "ECDSA",
                                              ecParameters.Curve.DecodePoint(encodedPublicKey),
                                              ecDomainParameters
                                          );

                ECCPublicKey            = new ECCPublicKey(

                                              encodedPublicKey,
                                              Algorithm,
                                              CryptoSerialization.ASN1_DER,
                                              CryptoEncoding.HEX,
                                              CustomData,

                                              ecParameters,
                                              ecDomainParameters,
                                              parsedPublicKey

                                          );

                return true;

            } catch (Exception e)
            {
                ErrorResponse  = $"The given X/Y-representation '{X.ToHexString()}'/'{Y.ToHexString()}' of an ECC public key is invalid: " + e.Message;
            }

            return false;

        }

        #endregion


        #region (static) ParseASN1    (Text)

        /// <summary>
        /// Parse the given text representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="Text">The text to be parsed.</param>
        public static ECCPublicKey ParseASN1(String                Text,
                                             CryptoAlgorithm?      Algorithm       = null,
                                             CryptoSerialization?  Serialization   = null,
                                             CryptoEncoding?       Encoding        = null,
                                             CustomData?           CustomData      = null)
        {

            if (TryParseASN1(Text,
                             out var eccPublicKey,
                             out var errorResponse,
                             Algorithm,
                             Serialization,
                             Encoding,
                             CustomData))
            {
                return eccPublicKey;
            }

            throw new ArgumentException("The given ASN.1 DER representation of an ECC public key is invalid: " + errorResponse,
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParseASN1 (Text,      out ECCPublicKey, out ErrorResponse, Algorithm = secp256r1, Serialization = raw, Encoding = base64, ...)

        /// <summary>
        /// Parse the given text representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="Text">The text to be parsed.</param>
        public static Boolean TryParseASN1(String                                  Text,
                                           [NotNullWhen(true)]  out ECCPublicKey?  ECCPublicKey,
                                           [NotNullWhen(false)] out String?        ErrorResponse,
                                           CryptoAlgorithm?                        Algorithm       = null,
                                           CryptoSerialization?                    Serialization   = null,
                                           CryptoEncoding?                         Encoding        = null,
                                           CustomData?                             CustomData      = null)
        {

            ECCPublicKey   = null;
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
                                 out ErrorResponse,
                                 Encoding,
                                 CustomData))
                {
                    ECCPublicKey = publicKey;
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

                var publicKeys = new List<ECCPublicKey>();

                #region Try to parse HEX (0-9, A-F, a-f)  // same as Base16

                if (Text.TryParseHEX(out var hexByteArray, out var errorResponse))
                {
                    try
                    {

                        if (TryParseASN1(hexByteArray,
                                         out var publicKey,
                                         out var errorResponse2,
                                         CryptoEncoding.HEX,
                                         CustomData))
                        {
                            publicKeys.Add(
                                publicKey
                            );
                        }

                        else
                            publicKeys.Add(
                                new ECCPublicKey(
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
                    }
                }

                #endregion

                #region Try to parse Base32

                if (Text.TryParseBASE32(out var base32ByteArray, out errorResponse))
                {
                    try
                    {

                        if (TryParseASN1(base32ByteArray,
                                         out var publicKey,
                                         out var errorResponse2,
                                         CryptoEncoding.BASE32,
                                         CustomData))
                        {
                            publicKeys.Add(
                                publicKey
                            );
                        }

                        else
                            publicKeys.Add(
                                new ECCPublicKey(
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

                        if (TryParseASN1(base64ByteArray,
                                         out var publicKey,
                                         out var errorResponse2,
                                         CryptoEncoding.BASE64,
                                         CustomData))
                        {
                            publicKeys.Add(
                                publicKey
                            );
                        }

                        else
                            publicKeys.Add(
                                new ECCPublicKey(
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
                        ECCPublicKey = publicKeys.First(publicKey => publicKey.Encoding == CryptoEncoding.HEX);
                    }

                    else if (hashSet.Count == 2 &&
                        hashSet.Contains(CryptoEncoding.BASE32) &&
                        hashSet.Contains(CryptoEncoding.BASE64))
                    {
                        ECCPublicKey = publicKeys.First(publicKey => publicKey.Encoding == CryptoEncoding.BASE64);
                    }

                    else
                    {
                        ErrorResponse = $"The given public key '{Text}' could be parsed as multiple different encodings: {publicKeys.Select(publicKey => publicKey.Encoding.ToString()).AggregateWith(", ")}!";
                        return false;
                    }

                }

                #endregion

                else
                    ECCPublicKey = publicKeys.First();

            }

            #endregion


            #region In case: Try to decode the public key using the given/parsed algorithm

            if (ECCPublicKey                 is not null &&
                ECCPublicKey.ParsedPublicKey is     null &&
                ECCPublicKey.Algorithm.HasValue)
            {

                try
                {

                    var ecParameters        = ECNamedCurveTable.GetByName(ECCPublicKey.Algorithm.ToString());
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

                    ECCPublicKey = new ECCPublicKey(

                                        ECCPublicKey.Value,
                                        ECCPublicKey.Algorithm,
                                        ECCPublicKey.Serialization,
                                        ECCPublicKey.Encoding,
                                        CustomData,

                                        ECParameters:        ecParameters,
                                        ECDomainParameters:  ecDomainParameters,
                                        ParsedPublicKey:     new ECPublicKeyParameters(
                                                                 "ECDSA",
                                                                 ecParameters.Curve.DecodePoint(ECCPublicKey.Value),
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


            return ECCPublicKey is not null;

        }

        #endregion


        #region (static) TryParse     (ByteArray, out ECCPublicKey, out ErrorResponse, Algorithm = secp256r1, Serialization = raw, Encoding = null, ...)

        /// <summary>
        /// Try to parse the given byte representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="ByteArray">The byte array to be parsed.</param>
        /// <param name="ECCPublicKey">The parsed ECC public key.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Algorithm">The optional cryptographic algorithm of the keys. Default is 'secp256r1'.</param>
        /// <param name="Serialization">An optional serialization of the public key. Default is 'raw'.</param>
        /// <param name="Encoding">An optional encoding of the public key.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public static Boolean TryParse(Byte[]                                  ByteArray,
                                       [NotNullWhen(true)]  out ECCPublicKey?  ECCPublicKey,
                                       [NotNullWhen(false)] out String?        ErrorResponse,
                                       CryptoAlgorithm?                        Algorithm       = null,
                                       CryptoSerialization?                    Serialization   = null,
                                       CryptoEncoding?                         Encoding        = null,
                                       CustomData?                             CustomData      = null)
        {

            ECCPublicKey   = null;
            ErrorResponse  = null;

            // Check whether this is an ASN.1 DER encoding sequence of an ECC public key
            if (ByteArray[0] == 0x30 && TryParseASN1(
                                            ByteArray,
                                            out ECCPublicKey,
                                            out ErrorResponse,
                                            Encoding,
                                            CustomData
                                        ))
            {
                return true;
            }


            ECCPublicKey ??= new ECCPublicKey(
                                 ByteArray,
                                 Algorithm     ?? CryptoAlgorithm.Secp256r1,
                                 Serialization ?? CryptoSerialization.RAW,
                                 null,
                                 CustomData
                             );


            #region In case: Try to decode the public key using the given/parsed algorithm

            if (ECCPublicKey                 is not null &&
                ECCPublicKey.ParsedPublicKey is     null &&
                ECCPublicKey.Algorithm.HasValue)
            {

                try
                {

                    var ecParameters        = ECNamedCurveTable.GetByName(ECCPublicKey.Algorithm.ToString());
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

                    ECCPublicKey            = new ECCPublicKey(

                                                  ECCPublicKey.Value,
                                                  ECCPublicKey.Algorithm,
                                                  ECCPublicKey.Serialization,
                                                  ECCPublicKey.Encoding,
                                                  CustomData,

                                                  ECParameters:        ecParameters,
                                                  ECDomainParameters:  ecDomainParameters,
                                                  ParsedPublicKey:     new ECPublicKeyParameters(
                                                                           "ECDSA",
                                                                           ecParameters.Curve.DecodePoint(ECCPublicKey.Value),
                                                                           ecDomainParameters
                                                                       )

                                              );

                }
                catch (Exception e)
                {
                    ErrorResponse = $"The given public key '{ByteArray.ToHexString()}' is invalid: {e.Message}";
                    return false;
                }

                return true;

            }

            #endregion


            return ECCPublicKey is not null;

        }

        #endregion

        #region (static) TryParseASN1 (ASN1,      out ECCPublicKey, out ErrorResponse, Encoding = null, ...)

        /// <summary>
        /// Try to parse the given ASN.1 DER representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="ASN1">The ASN.1 DER to be parsed.</param>
        /// <param name="ECCPublicKey">The parsed ECC public key.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Encoding">An optional encoding of the public key.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public static Boolean TryParseASN1(Byte[]                                  ASN1,
                                           [NotNullWhen(true)]  out ECCPublicKey?  ECCPublicKey,
                                           [NotNullWhen(false)] out String?        ErrorResponse,
                                           CryptoEncoding?                         Encoding     = null,
                                           CustomData?                             CustomData   = null)
        {

            ECCPublicKey   = null;
            ErrorResponse  = null;

            var encoding   = Encoding ?? CryptoEncoding.NONE;

            try
            {

                // Most likely an ASN.1 DER encoding sequence of an ECC public key
                if (ASN1[0] != 0x30)
                {
                    ECCPublicKey   = null;
                    ErrorResponse  = $"The given ASN.1 DER representation '{ASN1.ToHexString()}' of an ECC public key is invalid!";
                    return false;
                }

                // 3056301006072A8648CE3D020106052B8104000A034200047D393CCB3FD83472E70717D32A26B17DAB7FF5D488451E7EC25024F8BF633A3A9E5591F7EA04BCE0B5ED9182C1454509966C92B07E293D59BA8F5E837904C60C

                var asn1Stream            = new Asn1InputStream(ASN1);
                var asn1Object            = asn1Stream.ReadObject();
                var subjectPublicKeyInfo  = SubjectPublicKeyInfo.GetInstance(asn1Object);
                var algorithmIdentifier   = subjectPublicKeyInfo.Algorithm;
                var publicKeyBytes        = subjectPublicKeyInfo.PublicKey.GetBytes();

                // Get the curve parameters (assuming this is a named curve) // SecNamedCurves
                var x9ECParameters        = ECNamedCurveTable.GetByOid((DerObjectIdentifier) algorithmIdentifier.Parameters);
                var secNamedCurve         = ECNamedCurveTable.GetName ((DerObjectIdentifier) algorithmIdentifier.Parameters);

                var ecDomainParameters    = new ECDomainParameters(
                                                x9ECParameters.Curve,
                                                x9ECParameters.G,
                                                x9ECParameters.N,
                                                x9ECParameters.H,
                                                x9ECParameters.GetSeed()
                                            );

                if (!CryptoAlgorithm.TryParse(secNamedCurve, out var cryptoAlgorithm))
                {
                    ErrorResponse = $"The cryptographic algorithm '{secNamedCurve}' is unknown!";
                    return false;
                }

                ECCPublicKey              = new ECCPublicKey(

                                                ASN1,
                                                cryptoAlgorithm,
                                                CryptoSerialization.ASN1_DER,
                                                encoding,
                                                CustomData,

                                                ECParameters:        x9ECParameters,
                                                ECDomainParameters:  ecDomainParameters,
                                                ParsedPublicKey:     new ECPublicKeyParameters(
                                                                         "ECDSA",
                                                                         x9ECParameters.Curve.DecodePoint(publicKeyBytes),
                                                                         ecDomainParameters
                                                                     )

                                            );

                return true;

            } catch (Exception e)
            {
                ErrorResponse = $"The given ASN.1 DER representation '{encoding.Encode(ASN1)}' of an ECC public key is invalid: " + e.Message;
            }

            return false;

        }

        #endregion


        #region ToJSON(CustomPublicKeySerializer = null, CustomCustomDataSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPublicKeySerializer">A delegate to serialize cryptographic public keys.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CryptoSerialization                           Serialization,
                              Boolean                                       Embedded                     = true,
                              CustomJObjectSerializerDelegate<PublicKey>?   CustomPublicKeySerializer    = null,
                              CustomJObjectSerializerDelegate<CustomData>?  CustomCustomDataSerializer   = null)
        {

            var json = JSONObject.Create(

                           Serialization == CryptoSerialization.ECC_x_y
                               ? new JProperty("x",               Encoding.       Encode(X))
                               : new JProperty("value",           Encoding.       Encode(Value)),

                           Serialization == CryptoSerialization.ECC_x_y
                               ? new JProperty("y",               Encoding.       Encode(Y))
                               : null,

                           Algorithm.HasValue
                               ? new JProperty("algorithm",       Algorithm.Value.ToString())
                               : null,

                           Serialization == CryptoSerialization.RAW
                               ? null
                               : new JProperty("serialization",   Serialization),

                           Encoding      == CryptoEncoding.     BASE64
                               ? null
                               : new JProperty("encoding",        Encoding.       ToString()),

                           CustomData is not null
                               ? new JProperty("customData",      CustomData.     ToJSON(CustomCustomDataSerializer))
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
        public new ECCPublicKey Clone()

            => new (

                   (Byte[]) Value.Clone(),

                   Algorithm?.   Clone(),
                   Serialization.Clone(),
                   Encoding.     Clone(),

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
        public static Boolean operator == (ECCPublicKey? PublicKey1,
                                           ECCPublicKey? PublicKey2)
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
        public static Boolean operator != (ECCPublicKey? PublicKey1,
                                           ECCPublicKey? PublicKey2)

            => !(PublicKey1 == PublicKey2);

        #endregion

        #endregion

        #region IEquatable<ECCPublicKey> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two public keys for equality.
        /// </summary>
        /// <param name="Object">A public key to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ECCPublicKey eccPublicKey &&
                   Equals(eccPublicKey);

        #endregion

        #region Equals(ECCPublicKey)

        /// <summary>
        /// Compares two public keys for equality.
        /// </summary>
        /// <param name="ECCPublicKey">A public key to compare with.</param>
        public Boolean Equals(ECCPublicKey? ECCPublicKey)

            => ECCPublicKey is not null &&

               Value.        SequenceEqual(ECCPublicKey.Value)         &&

            ((!Algorithm.HasValue && !ECCPublicKey.Algorithm.HasValue) ||
              (Algorithm.HasValue &&  ECCPublicKey.Algorithm.HasValue && Algorithm.Value.Equals(ECCPublicKey.Algorithm.Value))) &&

               Serialization.Equals       (ECCPublicKey.Serialization) &&
               Encoding.     Equals       (ECCPublicKey.Encoding)      &&
               base.         Equals       (ECCPublicKey);

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

            if (Encoding      != CryptoEncoding.     NONE)
                parameters.Add(Encoding.       ToString());

            if (Serialization != CryptoSerialization.RAW)
                parameters.Add(Serialization.  ToString());

            if (Algorithm.HasValue)
                parameters.Add(Algorithm.Value.ToString());

            return String.Concat(

                       Encoding.Encode(Value),

                       parameters.Count > 0
                           ? $" [{parameters.AggregateWith(", ")}]"
                           : ""

                   );

        }

        #endregion


    }

}
