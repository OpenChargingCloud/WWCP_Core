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
using System.Buffers.Text;
using System.Security.Cryptography;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.Sec;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The pieces of JSON Web Signatures (RFC 7515) and JSON Web Keys (RFC 7517)
    /// that signed messages need: ECDSA as JWA defines it (RFC 7518 §3.4), the
    /// public key of an EC key pair as a JWK, and its thumbprint (RFC 7638).
    /// </summary>
    /// <remarks>
    /// <para>
    /// A JWS signature over ECDSA is the fixed-length concatenation r‖s, not DER,
    /// and ES384 hashes with SHA-384. Format 0 of the signed messages did both
    /// differently (finding F13 of the white paper "Signed OCPP Messages and
    /// Signer Roles"), which is why its signatures cannot be re-expressed as JWS.
    /// </para>
    /// <para>
    /// Signing uses deterministic ECDSA (RFC 6979): the same key and input give
    /// the same signature, which test vectors need, and a bad random number
    /// generator cannot leak the key.
    /// </para>
    /// </remarks>
    public static class JWS
    {

        #region Algorithm names

        /// <summary>ECDSA over P-256 with SHA-256.</summary>
        public const String ES256 = "ES256";

        /// <summary>ECDSA over P-384 with SHA-384.</summary>
        public const String ES384 = "ES384";

        /// <summary>ECDSA over P-521 with SHA-512.</summary>
        public const String ES512 = "ES512";

        #endregion


        #region Base64URL (Bytes)

        /// <summary>
        /// The given bytes in base64url without padding (RFC 7515 §2).
        /// </summary>
        public static String Base64URL(ReadOnlySpan<Byte> Bytes)

            => Base64Url.EncodeToString(Bytes);

        /// <summary>
        /// The given UTF-8 text in base64url without padding.
        /// </summary>
        public static String Base64URL(String Text)

            => Base64Url.EncodeToString(Encoding.UTF8.GetBytes(Text));

        #endregion

        #region TryDecodeBase64URL(Text, out Bytes)

        /// <summary>
        /// Decode base64url without padding. Padding, whitespace and the
        /// characters of plain base64 are refused.
        /// </summary>
        public static Boolean TryDecodeBase64URL(String? Text, out Byte[] Bytes)
        {

            Bytes = [];

            if (Text is null)
                return false;

            foreach (var c in Text)
                if (!(c is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '-' or '_'))
                    return false;

            try
            {
                Bytes = Base64Url.DecodeFromChars(Text);
                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion


        #region AlgorithmFor (CryptoAlgorithm)

        /// <summary>
        /// The JWS algorithm for keys of the given curve, or null when JWS has none.
        /// </summary>
        public static String? AlgorithmFor(CryptoAlgorithm Algorithm)

            => Algorithm switch {
                   var a when a == CryptoAlgorithm.Secp256r1 => ES256,
                   var a when a == CryptoAlgorithm.Secp384r1 => ES384,
                   var a when a == CryptoAlgorithm.Secp521r1 => ES512,
                   _                                         => null
               };

        #endregion

        #region CurveFor     (Algorithm)

        /// <summary>
        /// The JWK curve name ("crv") and the curve of the given JWS algorithm,
        /// or null for an algorithm this library does not verify.
        /// </summary>
        public static (String Name, String SECName, Int32 Length)? CurveFor(String? Algorithm)

            => Algorithm switch {
                   ES256 => ("P-256", "secp256r1", 32),
                   ES384 => ("P-384", "secp384r1", 48),
                   ES512 => ("P-521", "secp521r1", 66),
                   _     => null
               };

        #endregion


        #region Hash   (Algorithm, Data)

        private static Byte[] Hash(String Algorithm, Byte[] Data)

            => Algorithm switch {
                   ES256 => SHA256.HashData(Data),
                   ES384 => SHA384.HashData(Data),
                   ES512 => SHA512.HashData(Data),
                   _     => throw new ArgumentException($"Unknown JWS algorithm '{Algorithm}'!", nameof(Algorithm))
               };

        #endregion

        #region Digest (Algorithm)

        private static Org.BouncyCastle.Crypto.IDigest Digest(String Algorithm)

            => Algorithm switch {
                   ES256 => new Sha256Digest(),
                   ES384 => new Sha384Digest(),
                   ES512 => new Sha512Digest(),
                   _     => throw new ArgumentException($"Unknown JWS algorithm '{Algorithm}'!", nameof(Algorithm))
               };

        #endregion


        #region Sign   (Algorithm, PrivateKey, SigningInput)

        /// <summary>
        /// Sign the given JWS signing input (ASCII(BASE64URL(protected) || '.' || BASE64URL(payload)))
        /// and return the signature as r‖s.
        /// </summary>
        /// <param name="Algorithm">ES256, ES384 or ES512.</param>
        /// <param name="PrivateKey">The private key, on the algorithm's curve.</param>
        /// <param name="SigningInput">The signing input.</param>
        public static Byte[] Sign(String                      Algorithm,
                                  ECPrivateKeyParameters      PrivateKey,
                                  Byte[]                      SigningInput)
        {

            var curve = CurveFor(Algorithm) ?? throw new ArgumentException($"Unknown JWS algorithm '{Algorithm}'!", nameof(Algorithm));

            if (PrivateKey.Parameters.Curve.FieldSize != SECNamedCurves(curve.SECName).Curve.FieldSize)
                throw new ArgumentException($"The private key is not on the curve of {Algorithm}!", nameof(PrivateKey));

            var signer = new ECDsaSigner(new HMacDsaKCalculator(Digest(Algorithm)));
            signer.Init(true, PrivateKey);

            var rs     = signer.GenerateSignature(Hash(Algorithm, SigningInput));
            var result = new Byte[2 * curve.Length];

            rs[0].ToByteArrayUnsigned().CopyTo(result, curve.Length     - rs[0].ToByteArrayUnsigned().Length);
            rs[1].ToByteArrayUnsigned().CopyTo(result, 2 * curve.Length - rs[1].ToByteArrayUnsigned().Length);

            return result;

        }

        #endregion

        #region Verify (Algorithm, PublicKey,  SigningInput, Signature)

        /// <summary>
        /// Whether the given r‖s signature is valid for the signing input under the given public key.
        /// </summary>
        public static Boolean Verify(String                  Algorithm,
                                     ECPublicKeyParameters   PublicKey,
                                     Byte[]                  SigningInput,
                                     Byte[]                  Signature)
        {

            var curve = CurveFor(Algorithm);

            if (curve is null || Signature.Length != 2 * curve.Value.Length)
                return false;

            if (PublicKey.Parameters.Curve.FieldSize != SECNamedCurves(curve.Value.SECName).Curve.FieldSize)
                return false;

            try
            {

                var verifier = new ECDsaSigner();
                verifier.Init(false, PublicKey);

                return verifier.VerifySignature(
                           Hash(Algorithm, SigningInput),
                           new BigInteger(1, Signature, 0,                  curve.Value.Length),
                           new BigInteger(1, Signature, curve.Value.Length, curve.Value.Length)
                       );

            }
            catch
            {
                return false;
            }

        }

        #endregion


        #region PublicKeyJWK (PublicKey)

        /// <summary>
        /// The given EC public key as a JWK with only its required members
        /// (crv, kty, x, y), in the member order of RFC 7638.
        /// </summary>
        public static JObject PublicKeyJWK(ECPublicKeyParameters PublicKey)
        {

            var length  = (PublicKey.Parameters.Curve.FieldSize + 7) / 8;
            var name    = length switch {
                              32 => "P-256",
                              48 => "P-384",
                              66 => "P-521",
                              _  => throw new ArgumentException("JWS has no name for the curve of this key!", nameof(PublicKey))
                          };

            var q       = PublicKey.Q.Normalize();

            return new JObject(
                       new JProperty("crv", name),
                       new JProperty("kty", "EC"),
                       new JProperty("x",   Base64URL(q.AffineXCoord.GetEncoded())),
                       new JProperty("y",   Base64URL(q.AffineYCoord.GetEncoded()))
                   );

        }

        #endregion

        #region Thumbprint   (JWK)

        /// <summary>
        /// The RFC 7638 thumbprint (SHA-256, base64url) of the given JWK.
        /// </summary>
        /// <exception cref="ArgumentException">The JWK is not an EC key with crv, x and y.</exception>
        public static String Thumbprint(JObject JWK)
        {

            if (JWK["kty"]?.Value<String>() != "EC" ||
                JWK["crv"]?.Value<String>() is not String crv ||
                JWK["x"]?.  Value<String>() is not String x   ||
                JWK["y"]?.  Value<String>() is not String y)
            {
                throw new ArgumentException("Only EC keys with crv, x and y have a thumbprint here!", nameof(JWK));
            }

            // The required members in lexicographic order, no whitespace (RFC 7638 §3.2).
            var json = JSONCanonicalization.Canonicalize(
                           new JObject(
                               new JProperty("crv", crv),
                               new JProperty("kty", "EC"),
                               new JProperty("x",   x),
                               new JProperty("y",   y)
                           )
                       );

            return Base64URL(SHA256.HashData(Encoding.UTF8.GetBytes(json)));

        }

        /// <summary>
        /// The RFC 7638 thumbprint of the given EC public key.
        /// </summary>
        public static String Thumbprint(ECPublicKeyParameters PublicKey)

            => Thumbprint(PublicKeyJWK(PublicKey));

        #endregion

        #region TryParsePublicKey(JWK, out PublicKey)

        /// <summary>
        /// Read an EC public key from a JWK and check that the point is on its curve.
        /// </summary>
        public static Boolean TryParsePublicKey(JObject? JWK, out ECPublicKeyParameters? PublicKey)
        {

            PublicKey = null;

            if (JWK?["kty"]?.Value<String>() != "EC")
                return false;

            var curveName = JWK["crv"]?.Value<String>() switch {
                                "P-256" => "secp256r1",
                                "P-384" => "secp384r1",
                                "P-521" => "secp521r1",
                                _       => null
                            };

            if (curveName is null ||
                !TryDecodeBase64URL(JWK["x"]?.Value<String>(), out var x) ||
                !TryDecodeBase64URL(JWK["y"]?.Value<String>(), out var y))
            {
                return false;
            }

            var curve  = SECNamedCurves(curveName);
            var length = (curve.Curve.FieldSize + 7) / 8;

            if (x.Length != length || y.Length != length)
                return false;

            try
            {

                var point = curve.Curve.CreatePoint(new BigInteger(1, x), new BigInteger(1, y));

                if (!point.IsValid())
                    return false;

                PublicKey = new ECPublicKeyParameters(
                                point,
                                new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed())
                            );

                return true;

            }
            catch
            {
                return false;
            }

        }

        #endregion

        #region SECNamedCurves(Name)

        private static X9ECParameters SECNamedCurves(String Name)

            => SecNamedCurves.GetByName(Name)
                   ?? throw new ArgumentException($"Unknown curve '{Name}'!", nameof(Name));

        #endregion

    }

}
