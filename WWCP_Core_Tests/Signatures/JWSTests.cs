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

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.Signatures
{

    /// <summary>
    /// ECDSA as JWS has it (r‖s, ES384 with SHA-384), JWKs and their RFC 7638 thumbprints,
    /// checked against the RFCs' own examples and against signatures made by Node.js.
    /// </summary>
    [TestFixture]
    public class JWSTests
    {

        #region Data

        // RFC 7515 Appendix A.3: ECDSA P-256 SHA-256.
        private const String A3X             = "f83OJ3D2xF1Bg8vub9tLe1gHMzV76e8Tus9uPHvRVEU";
        private const String A3Y             = "x_FEzRu9m36HLN_tue659LNpXW6pCyStikYjKIWI5a0";
        private const String A3SigningInput  = "eyJhbGciOiJFUzI1NiJ9.eyJpc3MiOiJqb2UiLA0KICJleHAiOjEzMDA4MTkzODAsDQogImh0dHA6Ly9leGFtcGxlLmNvbS9pc19yb290Ijp0cnVlfQ";
        private const String A3Signature     = "DtEhU3ljbEg8L38VWAfUAqOyKAM6-Xx-F4GawxaepmXFCgfTjDxw5djxLa8ISlSApmWQxfKTUJqPP3-Kg6NU1Q";

        private static JObject A3JWK
            => new (new JProperty("kty", "EC"), new JProperty("crv", "P-256"), new JProperty("x", A3X), new JProperty("y", A3Y));

        #endregion

        #region (private) PublicKey(JWK)

        private static ECPublicKeyParameters PublicKey(JObject JWK)
        {
            Assert.That(JWS.TryParsePublicKey(JWK, out var publicKey), Is.True, "The JWK was not read!");
            return publicKey!;
        }

        #endregion

        #region (private) NewKeyPair(Curve)

        private static AsymmetricKeys NewKeyPair(String Curve)
        {

            var curve      = SecNamedCurves.GetByName(Curve);
            var generator  = new ECKeyPairGenerator();
            generator.Init(new ECKeyGenerationParameters(new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H), new SecureRandom()));

            var pair       = generator.GenerateKeyPair();

            return new ((ECPrivateKeyParameters) pair.Private, (ECPublicKeyParameters) pair.Public);

        }

        private sealed record AsymmetricKeys(ECPrivateKeyParameters Private, ECPublicKeyParameters Public);

        #endregion


        #region TheES256ExampleOfRFC7515Verifies()

        [Test]
        public void TheES256ExampleOfRFC7515Verifies()
        {

            Assert.That(JWS.TryDecodeBase64URL(A3Signature, out var signature), Is.True);

            Assert.Multiple(() => {
                Assert.That(JWS.Verify(JWS.ES256, PublicKey(A3JWK), Encoding.ASCII.GetBytes(A3SigningInput),        signature), Is.True);
                Assert.That(JWS.Verify(JWS.ES256, PublicKey(A3JWK), Encoding.ASCII.GetBytes(A3SigningInput + "x"), signature), Is.False, "A changed input verified!");
                Assert.That(JWS.Verify(JWS.ES384, PublicKey(A3JWK), Encoding.ASCII.GetBytes(A3SigningInput),        signature), Is.False, "ES384 verified with a P-256 key!");
            });

        }

        #endregion

        #region TheThumbprintIsTheOneNodeComputes()

        [Test]
        public void TheThumbprintIsTheOneNodeComputes()
        {
            Assert.That(JWS.Thumbprint(A3JWK), Is.EqualTo(IndependentVectors.RFC7515A3Thumbprint));
        }

        #endregion

        #region AKeyPairsPointBecomesTheSameJWK()

        /// <summary>
        /// A key pair holds its public key as the raw point 04‖x‖y; its JWK has those x and y.
        /// </summary>
        [Test]
        public void AKeyPairsPointBecomesTheSameJWK()
        {

            JWS.TryDecodeBase64URL(A3X, out var x);
            JWS.TryDecodeBase64URL(A3Y, out var y);

            var curve      = SecNamedCurves.GetByName("secp256r1");
            var point      = curve.Curve.DecodePoint([ 0x04, .. x, .. y ]);
            var publicKey  = new ECPublicKeyParameters(point, new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H));

            var jwk        = JWS.PublicKeyJWK(publicKey);

            Assert.Multiple(() => {
                Assert.That(jwk["crv"]!.Value<String>(), Is.EqualTo("P-256"));
                Assert.That(jwk["x"]!.  Value<String>(), Is.EqualTo(A3X));
                Assert.That(jwk["y"]!.  Value<String>(), Is.EqualTo(A3Y));
                Assert.That(JWS.Thumbprint(publicKey),   Is.EqualTo(IndependentVectors.RFC7515A3Thumbprint));
            });

        }

        #endregion

        #region SignaturesMadeByNodeVerify(Algorithm, ...)

        private static IEnumerable<TestCaseData> NodeSignatures

            => IndependentVectors.Signatures.Select(vector => new TestCaseData(vector[0], vector[1], vector[2], vector[3], vector[4], vector[5], vector[6]).SetArgDisplayNames(vector[0]));

        [TestCaseSource(nameof(NodeSignatures))]
        public void SignaturesMadeByNodeVerify(String Algorithm, String Curve, String X, String Y, String SigningInput, String Signature, String Thumbprint)
        {

            var jwk = new JObject(new JProperty("kty", "EC"), new JProperty("crv", Curve), new JProperty("x", X), new JProperty("y", Y));

            JWS.TryDecodeBase64URL(Signature, out var signature);

            Assert.Multiple(() => {
                Assert.That(JWS.Verify(Algorithm, PublicKey(jwk), Encoding.ASCII.GetBytes(SigningInput), signature), Is.True);
                Assert.That(JWS.Thumbprint(jwk), Is.EqualTo(Thumbprint));
            });

        }

        #endregion

        #region TheDeterministicSignatureOfRFC6979()

        /// <summary>
        /// RFC 6979 A.2.5: P-256, SHA-256, message "sample". Node verifies this pair too.
        /// </summary>
        [Test]
        public void TheDeterministicSignatureOfRFC6979()
        {

            var curve       = SecNamedCurves.GetByName("secp256r1");
            var domain      = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            var privateKey  = new ECPrivateKeyParameters(new BigInteger("C9AFA9D845BA75166B5C215767B1D6934E50C3DB36E89B127B8A622B120F6721", 16), domain);

            var signature   = JWS.Sign(JWS.ES256, privateKey, Encoding.ASCII.GetBytes("sample"));

            Assert.That(Convert.ToHexString(signature),
                        Is.EqualTo("EFD48B2AACB6A8FD1140DD9CD45E81D69D2C877B56AAF991C34D0EA84EAF3716" +
                                   "F7CB1C942D657C41D436C7A1B6E29F65F3E900DBB9AFF4064DC4AB2F843ACDA8"));

        }

        #endregion

        #region EveryCurveSignsAndVerifies(Algorithm, Curve, Length)

        [TestCase(JWS.ES256, "secp256r1",  64)]
        [TestCase(JWS.ES384, "secp384r1",  96)]
        [TestCase(JWS.ES512, "secp521r1", 132)]
        public void EveryCurveSignsAndVerifies(String Algorithm, String Curve, Int32 Length)
        {

            var keys       = NewKeyPair(Curve);
            var stranger   = NewKeyPair(Curve);
            var input      = Encoding.ASCII.GetBytes("eyJhbGciOiJFUzI1NiJ9.eyJ0eXBlIjoiSW1tZWRpYXRlIn0");

            var signature  = JWS.Sign(Algorithm, keys.Private, input);
            var tampered   = signature.ToArray();
            tampered[^1]  ^= 0x01;

            Assert.Multiple(() => {
                Assert.That(signature.Length,                                              Is.EqualTo(Length));
                Assert.That(JWS.Sign(Algorithm, keys.Private, input),                     Is.EqualTo(signature), "Signing is not deterministic!");
                Assert.That(JWS.Verify(Algorithm, keys.Public,     input, signature),     Is.True);
                Assert.That(JWS.Verify(Algorithm, keys.Public,     input, tampered),      Is.False, "A changed signature verified!");
                Assert.That(JWS.Verify(Algorithm, stranger.Public, input, signature),     Is.False, "Another key verified!");
                Assert.That(JWS.Verify(Algorithm, keys.Public,     input, signature[1..]), Is.False, "A short signature verified!");
            });

        }

        #endregion

        #region AKeyOnAnotherCurveDoesNotSign()

        [Test]
        public void AKeyOnAnotherCurveDoesNotSign()
        {
            Assert.Throws<ArgumentException>(() => JWS.Sign(JWS.ES384, NewKeyPair("secp256r1").Private, [ 1, 2, 3 ]));
        }

        #endregion

        #region APointOffTheCurveIsRefused()

        [Test]
        public void APointOffTheCurveIsRefused()
        {

            var jwk = A3JWK;
            jwk["y"] = A3X;

            Assert.That(JWS.TryParsePublicKey(jwk, out _), Is.False);

        }

        #endregion

        #region OnlyBase64URLWithoutPaddingIsRead(Text)

        [TestCase("YWJj=")]
        [TestCase("a+b/")]
        [TestCase("YW Jj")]
        public void OnlyBase64URLWithoutPaddingIsRead(String Text)
        {
            Assert.That(JWS.TryDecodeBase64URL(Text, out _), Is.False);
        }

        #endregion

    }

}
