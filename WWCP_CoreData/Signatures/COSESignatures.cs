/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The encodings of a signature with a protected header (white paper "Signed OCPP
    /// Messages and Signer Roles", section 4.2).
    /// </summary>
    [Flags]
    public enum SignatureFormats
    {

        /// <summary>
        /// A JWS with a detached payload: the RFC 8785 form of the JSON of the message.
        /// </summary>
        JWS   = 1,

        /// <summary>
        /// A COSE_Sign1 with a detached payload: the deterministic CBOR of the message.
        /// </summary>
        COSE  = 2,

        /// <summary>
        /// Both, next to each other.
        /// </summary>
        Both  = JWS | COSE

    }


    /// <summary>
    /// The pieces of CBOR Object Signing (RFC 9052) that signed messages need: a
    /// COSE_Sign1 with a detached payload, whose protected header carries what the
    /// protected header of a JWS signature carries (white paper "Signed OCPP Messages
    /// and Signer Roles", sections 4.1 and 4.2).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The header is the same in both encodings, and is handled here as the JSON
    /// object the JWS form has: "alg" is the COSE algorithm (label 1), "kid" the key
    /// identifier (label 4) and "crit" the critical parameters (label 2); every other
    /// field keeps its name, as a text label.
    /// </para>
    /// <para>
    /// "kid" is the COSE Key Thumbprint of the signer's public key (RFC 9679, SHA-256).
    /// The public key itself travels in the unprotected header, as a COSE_Key under
    /// the text label "OCPPSignerKey" - what "jwk" is to the JWS form.
    /// </para>
    /// <para>
    /// The mathematics are those of the JWS form: ECDSA with the signature as r‖s,
    /// deterministic (RFC 6979). What is signed is the Sig_structure of RFC 9052,
    /// section 4.4, over the protected header and the detached payload.
    /// </para>
    /// </remarks>
    public static class COSESignatures
    {

        #region Data

        /// <summary>
        /// The text label of the signer's public key, a COSE_Key, in the unprotected header.
        /// </summary>
        public const String SignerKeyLabel = "OCPPSignerKey";

        private static readonly (Int64 COSE, String JWS)[] algorithms = [
            ( -7, JWS.ES256),
            (-35, JWS.ES384),
            (-36, JWS.ES512),
            // Their fully-specified successors (RFC 9864): the same mathematics.
            ( -9, JWS.ES256),
            (-51, JWS.ES384),
            (-52, JWS.ES512)
        ];

        #endregion


        #region KeyIdentifier(PublicKey)

        /// <summary>
        /// The key identifier of the given public key: its COSE Key Thumbprint (RFC 9679, SHA-256).
        /// </summary>
        public static Byte[] KeyIdentifier(ECPublicKeyParameters PublicKey)

            => COSEKey.From(PublicKey).Thumbprint();

        #endregion

        #region (private static) ProtectedBytes(Header, KeyIdentifier)

        /// <summary>
        /// The protected header bucket of the given header, in its deterministic encoding.
        /// </summary>
        private static Byte[] ProtectedBytes(JObject  Header,
                                             Byte[]   KeyIdentifier)
        {

            var algorithm  = Header["alg"]?.Value<String>();
            var cose       = algorithms.FirstOrDefault(known => known.JWS == algorithm);

            if (cose.JWS is null)
                throw new ArgumentException($"COSE has no algorithm for '{algorithm}'!", nameof(Header));

            var parameters = new List<KeyValuePair<CBORValue, CBORValue>> {
                                 new (COSEHeaderLabel.Algorithm,      CBORValue.FromInt64(cose.COSE)),
                                 new (COSEHeaderLabel.KeyIdentifier,  CBORValue.FromBytes(KeyIdentifier))
                             };

            foreach (var property in Header.Properties())
            {
                switch (property.Name)
                {

                    case "alg":
                    case "kid":
                        break;

                    case "crit":
                        parameters.Add(new (COSEHeaderLabel.Critical,
                                            CBORValue.FromArray(property.Value.Select(field => CBORValue.FromText(field.Value<String>()!)))));
                        break;

                    default:
                        parameters.Add(new (CBORValue.FromText(property.Name),
                                            CBORJSON.ToCBOR(property.Value)));
                        break;

                }
            }

            return CBORValue.FromMap(parameters).ToByteArray(CBORWriterOptions.Canonical);

        }

        #endregion

        #region Sign(Header, PublicKey, PrivateKey, Payload)

        /// <summary>
        /// Sign the given payload, and return the COSE_Sign1 (tag 18) without it.
        /// </summary>
        /// <param name="Header">The protected header, as the JWS form has it: "alg" as ES256, ES384 or ES512. Its "kid" is not read: it is the thumbprint of the public key.</param>
        /// <param name="PublicKey">The signer's public key, which travels in the unprotected header.</param>
        /// <param name="PrivateKey">The signer's private key.</param>
        /// <param name="Payload">What is signed: the deterministic CBOR of the message without its signatures.</param>
        public static Byte[] Sign(JObject                 Header,
                                  ECPublicKeyParameters   PublicKey,
                                  ECPrivateKeyParameters  PrivateKey,
                                  Byte[]                  Payload)
        {

            var coseKey         = COSEKey.From(PublicKey);
            var protectedBytes  = ProtectedBytes(Header, coseKey.Thumbprint());

            return new COSESign1(
                       protectedBytes,
                       new COSEHeaders(
                           (CBORValue.FromText(SignerKeyLabel), coseKey.ToCBOR())
                       ),
                       null,
                       JWS.Sign(
                           Header["alg"]!.Value<String>()!,
                           PrivateKey,
                           COSESign1.ToBeSigned(protectedBytes, Payload)
                       ),
                       true
                   ).ToByteArray();

        }

        #endregion

        #region TryRead(COSE, out Sign1, out Header, out PublicKey, out ErrorResponse)

        /// <summary>
        /// Read a COSE_Sign1 signature: its protected header as the JSON object the JWS
        /// form has, and the public key it carries, when it carries one.
        /// </summary>
        /// <param name="COSE">The bytes of the COSE_Sign1.</param>
        /// <param name="Sign1">The COSE_Sign1.</param>
        /// <param name="Header">Its protected header: "alg" as ES256, ES384 or ES512, "kid" as base64url.</param>
        /// <param name="PublicKey">The public key of its unprotected header.</param>
        /// <param name="ErrorResponse">Why it cannot be read.</param>
        public static Boolean TryRead(Byte[]                                 COSE,
                                      [NotNullWhen(true)]  out COSESign1?    Sign1,
                                      [NotNullWhen(true)]  out JObject?      Header,
                                      out ECPublicKeyParameters?             PublicKey,
                                      [NotNullWhen(false)] out String?       ErrorResponse)
        {

            Header     = null;
            PublicKey  = null;

            try
            {

                if (!COSESign1.TryParse(COSE, out Sign1, out ErrorResponse))
                    return false;

                if (!Sign1.IsDetached)
                {
                    ErrorResponse = "The signature carries a payload of its own: it signs something else than the message!";
                    return false;
                }

                var header = new JObject();

                foreach (var parameter in Sign1.ProtectedHeader.Parameters)
                {

                    if (parameter.Key.Equals(COSEHeaderLabel.Algorithm))
                    {

                        var name = parameter.Value.TryGetInt64(out var number)
                                       ? algorithms.FirstOrDefault(known => known.COSE == number).JWS
                                       : null;

                        if (name is null)
                        {
                            ErrorResponse = $"The signature algorithm '{parameter.Value.ToDiagnosticString()}' is unknown!";
                            return false;
                        }

                        header["alg"] = name;

                    }

                    else if (parameter.Key.Equals(COSEHeaderLabel.KeyIdentifier))
                    {

                        if (!parameter.Value.TryGetBytes(out var kid))
                        {
                            ErrorResponse = "The key identifier of the signature is no byte string!";
                            return false;
                        }

                        header["kid"] = JWS.Base64URL(kid);

                    }

                    else if (parameter.Key.Equals(COSEHeaderLabel.Critical))
                    {

                        if (parameter.Value.Kind != CBORValueKind.Array)
                        {
                            ErrorResponse = "The critical header parameters of the signature are no array!";
                            return false;
                        }

                        // A label that is no text is one of COSE itself, and none of those is
                        // understood here beyond the three above: it is kept, and refused later.
                        header["crit"] = new JArray(parameter.Value.AsArray().Select(item => CBORJSON.ToJSON(item)));

                    }

                    else if (parameter.Key.TryGetText(out var label))
                        header[label] = CBORJSON.ToJSON(parameter.Value);

                    else
                    {
                        ErrorResponse = $"The protected header of the signature carries the parameter {parameter.Key.ToDiagnosticString()}, which is none of a signed message!";
                        return false;
                    }

                }

                if (header["alg"] is null)
                {
                    ErrorResponse = "The protected header of the signature names no algorithm!";
                    return false;
                }

                if (Sign1.UnprotectedHeader.TryGet(CBORValue.FromText(SignerKeyLabel), out var signerKey))
                {

                    if (!COSEKey.TryParse(signerKey, out var coseKey, out var keyError) ||
                        !coseKey.TryToPublicKey(out var publicKey, out keyError)        ||
                        publicKey is not ECPublicKeyParameters ecPublicKey)
                    {
                        ErrorResponse = $"The public key the signature carries cannot be read: {keyError ?? "it is no elliptic curve key"}";
                        return false;
                    }

                    PublicKey = ecPublicKey;

                }

                Header         = header;
                ErrorResponse  = null;
                return true;

            }
            catch (Exception e)
            {
                Sign1          = null;
                ErrorResponse  = $"The COSE signature cannot be read: {e.Message}";
                return false;
            }

        }

        #endregion

        #region Verify(Sign1, Header, PublicKey, Payload)

        /// <summary>
        /// Whether the given COSE_Sign1 is valid for the given payload under the given public key.
        /// </summary>
        /// <param name="Sign1">The COSE_Sign1, without its payload.</param>
        /// <param name="Header">Its protected header, as TryRead gave it.</param>
        /// <param name="PublicKey">The public key of the signer.</param>
        /// <param name="Payload">The deterministic CBOR of the message without its signatures.</param>
        public static Boolean Verify(COSESign1              Sign1,
                                     JObject                Header,
                                     ECPublicKeyParameters  PublicKey,
                                     Byte[]                 Payload)

            => JWS.Verify(
                   Header["alg"]?.Value<String>() ?? "",
                   PublicKey,
                   COSESign1.ToBeSigned(Sign1.ProtectedHeaderBytes, Payload),
                   Sign1.Signature
               );

        #endregion

    }

}
