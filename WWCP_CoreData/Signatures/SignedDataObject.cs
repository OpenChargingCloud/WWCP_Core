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

using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Who signed a data object: a key whose signature over the object holds.
    /// </summary>
    /// <param name="KeyId">The RFC 7638 thumbprint of the key.</param>
    /// <param name="PublicKey">The public key.</param>
    /// <param name="PublicKeyBytes">The public key as its uncompressed point, as a signature of a message names it.</param>
    /// <param name="IssuedAt">When the signature says it was made.</param>
    /// <param name="Expires">Until when the signature says it holds, if it says so.</param>
    public sealed record DataObjectSigner(String                 KeyId,
                                          ECPublicKeyParameters  PublicKey,
                                          Byte[]                 PublicKeyBytes,
                                          DateTimeOffset         IssuedAt,
                                          DateTimeOffset?        Expires);


    /// <summary>
    /// Signatures on data that is stored and verified later - a role
    /// assignment, a signature policy, a charging tariff, a set of key pins:
    /// the embedded JWS of a signed message, on the object itself (white
    /// paper "Signed OCPP Messages and Signer Roles", section 4.4).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The "signatures" member of the object is the one of a General JWS JSON
    /// Serialization with a detached payload: the RFC 8785 form of the object
    /// without it. Each element is { "protected", "header", "signature" }.
    /// </para>
    /// <para>
    /// The protected header has no field of a message. In their place it
    /// says what kind of object was signed ("typ"), when ("iat"), and until
    /// when the signature holds ("exp"), where it says so. A signature made
    /// for one kind of object is none for another, and none for a message.
    /// </para>
    /// <para>
    /// Whose signature counts is not decided here: verifying gives the keys
    /// whose signatures hold, and whoever asked holds them to its roles.
    /// </para>
    /// </remarks>
    public static class SignedDataObject
    {

        #region Types of objects

        /// <summary>
        /// The keys of a signer role (section 6.2).
        /// </summary>
        public const String SignerRoleType       = "ocpp-signer-role";

        /// <summary>
        /// The policy table of a node (sections 5.2 and 6.4).
        /// </summary>
        public const String SignaturePolicyType  = "ocpp-signature-policy";

        /// <summary>
        /// A charging tariff.
        /// </summary>
        public const String ChargingTariffType   = "ocpp-charging-tariff";

        /// <summary>
        /// A set of key pins (Key Pins and TLS 1.3 Raw Public Keys, section 5.7).
        /// </summary>
        public const String KeyPinSetType        = "evqi-key-pin-set";

        #endregion

        /// <summary>
        /// The member of an object its signatures are in.
        /// </summary>
        public const String SignaturesMember     = "signatures";


        #region ParseAsReceived(Text)

        /// <summary>
        /// Read an object as it was written: a text that looks like a point
        /// in time stays the text it is. Newtonsoft reads such a text as a
        /// date otherwise and writes it back in its own form, and the
        /// signatures of the object no longer hold.
        /// </summary>
        /// <param name="Text">The JSON of an object.</param>
        public static JObject ParseAsReceived(String Text)
        {

            using var reader = new JsonTextReader(new StringReader(Text)) {
                                   DateParseHandling = DateParseHandling.None
                               };

            var json = JObject.Load(reader);

            // Nothing but the object, and no second one behind it.
            while (reader.Read())
                if (reader.TokenType != JsonToken.Comment)
                    throw new JsonReaderException("There is something behind the object!");

            return json;

        }

        #endregion

        #region PayloadOf(Object)

        /// <summary>
        /// The detached payload of the signatures of an object: the RFC 8785
        /// form of the object without its signatures, base64url encoded.
        /// </summary>
        /// <param name="Object">The object, with or without signatures.</param>
        public static String PayloadOf(JObject Object)
        {

            var withoutSignatures = (JObject) Object.DeepClone();
            withoutSignatures.Remove(SignaturesMember);

            return JWS.Base64URL(JSONCanonicalization.CanonicalizeToUTF8(withoutSignatures));

        }

        #endregion

        #region Sign(Object, Type, KeyPair, IssuedAt = null, Expires = null)

        /// <summary>
        /// Sign the given object: a signature is added to its "signatures",
        /// beside those that are there.
        /// </summary>
        /// <param name="Object">The object to sign. It is changed.</param>
        /// <param name="Type">What kind of object it is.</param>
        /// <param name="KeyPair">The key pair to sign with.</param>
        /// <param name="IssuedAt">When the signature is made: now, by default.</param>
        /// <param name="Expires">Until when the signature holds, if it is to say so.</param>
        /// <returns>The object.</returns>
        public static JObject Sign(JObject          Object,
                                   String           Type,
                                   ECCKeyPair       KeyPair,
                                   DateTimeOffset?  IssuedAt   = null,
                                   DateTimeOffset?  Expires    = null)
        {

            if (KeyPair.PrivateKey is null)
                throw new ArgumentException("The key pair has no private key!", nameof(KeyPair));

            var algorithm      = JWS.AlgorithmFor(KeyPair.Algorithm)
                                     ?? throw new ArgumentException($"A key of '{KeyPair.Algorithm}' cannot sign a data object!", nameof(KeyPair));

            if (Object[SignaturesMember] is not null and not JArray)
                throw new ArgumentException($"The '{SignaturesMember}' of the object are not an array!", nameof(Object));

            var jwk            = JWS.PublicKeyJWK(KeyPair.PublicKey);

            var header         = new JObject(
                                     new JProperty("alg",  algorithm),
                                     new JProperty("kid",  JWS.Thumbprint(jwk)),
                                     new JProperty("typ",  Type),
                                     new JProperty("iat",  (IssuedAt ?? Timestamp.Now).ToUnixTimeSeconds())
                                 );

            if (Expires.HasValue)
                header.Add("exp", Expires.Value.ToUnixTimeSeconds());

            var protectedText  = JWS.Base64URL(header.ToString(Formatting.None));

            var signature      = JWS.Sign(
                                     algorithm,
                                     KeyPair.PrivateKey,
                                     Encoding.ASCII.GetBytes(protectedText + "." + PayloadOf(Object))
                                 );

            if (Object[SignaturesMember] is not JArray signatures)
            {
                signatures = [];
                Object[SignaturesMember] = signatures;
            }

            signatures.Add(
                new JObject(
                    new JProperty("protected",  protectedText),
                    new JProperty("header",     new JObject(new JProperty("jwk", jwk))),
                    new JProperty("signature",  JWS.Base64URL(signature))
                )
            );

            return Object;

        }

        #endregion

        #region Verify(Object, Type, out Signers, out ErrorResponse, Now = null, MaxClockSkew = null, WithoutTimeWindow = false, KeyOf = null)

        /// <summary>
        /// Verify the signatures of the given object, as it was received.
        /// </summary>
        /// <remarks>
        /// Every signature has to hold: an object with one signature that
        /// does not is refused as a whole, since nobody can tell which of its
        /// signers meant what was changed. An object without signatures is
        /// signed by nobody, and that is what is said of it.
        /// </remarks>
        /// <param name="Object">The object, as received.</param>
        /// <param name="Type">What kind of object is expected.</param>
        /// <param name="Signers">The keys whose signatures hold, each once.</param>
        /// <param name="ErrorResponse">Why the object is refused.</param>
        /// <param name="Now">The time to hold the signatures to: now, by default.</param>
        /// <param name="MaxClockSkew">How far in the future a signature may say it was made: five minutes, by default.</param>
        /// <param name="WithoutTimeWindow">Whether the times of the signatures are left out: for an object that was accepted earlier and is read again, or for a node whose clock was never set.</param>
        /// <param name="KeyOf">The public key of a key identifier, for a signature that does not carry its key.</param>
        public static Boolean Verify(JObject                               Object,
                                     String                                Type,
                                     out IReadOnlyList<DataObjectSigner>   Signers,
                                     out String?                           ErrorResponse,
                                     DateTimeOffset?                       Now                 = null,
                                     TimeSpan?                             MaxClockSkew        = null,
                                     Boolean                               WithoutTimeWindow   = false,
                                     Func<String, ECPublicKeyParameters?>? KeyOf               = null)
        {

            var signers    = new List<DataObjectSigner>();
            Signers        = signers;
            ErrorResponse  = null;

            if (Object[SignaturesMember] is null)
                return true;

            if (Object[SignaturesMember] is not JArray signatures)
            {
                ErrorResponse = $"The '{SignaturesMember}' of the object are not an array!";
                return false;
            }

            var now      = Now          ?? Timestamp.Now;
            var skew     = MaxClockSkew ?? TimeSpan.FromMinutes(5);
            var payload  = PayloadOf(Object);

            foreach (var element in signatures)
            {

                if (element is not JObject signature                                              ||
                    signature["protected"]?.Type != JTokenType.String                             ||
                    signature["signature"]?.Type != JTokenType.String                             ||
                    !JWS.TryDecodeBase64URL(signature["protected"]!.Value<String>(), out var headerBytes) ||
                    !JWS.TryDecodeBase64URL(signature["signature"]!.Value<String>(), out var value))
                {
                    ErrorResponse = "A signature of the object is not a JWS signature!";
                    return false;
                }

                JObject header;

                try
                {
                    header = JObject.Parse(Encoding.UTF8.GetString(headerBytes));
                }
                catch
                {
                    ErrorResponse = "The protected header of a signature is not JSON!";
                    return false;
                }

                // Nothing of a header is understood that is marked as critical.
                if (header["crit"] is not null)
                {
                    ErrorResponse = "A signature of the object has critical header fields, and a data object knows none!";
                    return false;
                }

                if (header["typ"]?.Type != JTokenType.String ||
                    header["typ"]!.Value<String>() != Type)
                {
                    ErrorResponse = $"A signature of the object was not made for a '{Type}'!";
                    return false;
                }

                var algorithm  = header["alg"]?.Type == JTokenType.String ? header["alg"]!.Value<String>() : null;
                var keyId      = header["kid"]?.Type == JTokenType.String ? header["kid"]!.Value<String>() : null;

                if (algorithm is null || keyId is null || JWS.CurveFor(algorithm) is null)
                {
                    ErrorResponse = "A signature of the object names no algorithm or no key that is known!";
                    return false;
                }

                ECPublicKeyParameters? publicKey;

                if (signature["header"] is JObject { } unprotected && unprotected["jwk"] is JObject jwk)
                {
                    if (!JWS.TryParsePublicKey(jwk, out publicKey) || publicKey is null || JWS.Thumbprint(publicKey) != keyId)
                    {
                        ErrorResponse = $"The key a signature of the object carries is not the key '{keyId}' it names!";
                        return false;
                    }
                }
                else
                {
                    publicKey = KeyOf?.Invoke(keyId);
                    if (publicKey is null || JWS.Thumbprint(publicKey) != keyId)
                    {
                        ErrorResponse = $"The key '{keyId}' of a signature of the object is not known!";
                        return false;
                    }
                }

                if (!JWS.Verify(algorithm,
                                publicKey,
                                Encoding.ASCII.GetBytes(signature["protected"]!.Value<String>() + "." + payload),
                                value))
                {
                    ErrorResponse = $"The signature of the key '{keyId}' does not hold for the object!";
                    return false;
                }

                if (header["iat"]?.Type != JTokenType.Integer ||
                   (header["exp"] is not null && header["exp"]!.Type != JTokenType.Integer))
                {
                    ErrorResponse = $"The signature of the key '{keyId}' does not say when it was made, or not until when it holds!";
                    return false;
                }

                DateTimeOffset   issuedAt;
                DateTimeOffset?  expires;

                try
                {
                    issuedAt  = DateTimeOffset.FromUnixTimeSeconds(header["iat"]!.Value<Int64>());
                    expires   = header["exp"] is null
                                    ? null
                                    : DateTimeOffset.FromUnixTimeSeconds(header["exp"]!.Value<Int64>());
                }
                catch
                {
                    ErrorResponse = $"The signature of the key '{keyId}' was made at no time there is!";
                    return false;
                }

                if (!WithoutTimeWindow)
                {

                    if (issuedAt > now + skew)
                    {
                        ErrorResponse = $"The signature of the key '{keyId}' was made in the future!";
                        return false;
                    }

                    if (expires.HasValue && now > expires.Value)
                    {
                        ErrorResponse = $"The signature of the key '{keyId}' has expired!";
                        return false;
                    }

                }

                if (!signers.Any(signer => signer.KeyId == keyId))
                    signers.Add(new DataObjectSigner(
                                    keyId,
                                    publicKey,
                                    publicKey.Q.GetEncoded(false),
                                    issuedAt,
                                    expires
                                ));

            }

            return true;

        }

        #endregion

    }

}
