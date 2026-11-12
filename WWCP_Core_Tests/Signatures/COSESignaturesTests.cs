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

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.Signatures
{

    /// <summary>
    /// A signature as a COSE_Sign1 (RFC 9052) with a detached payload: the header a JWS
    /// signature has, the COSE Key Thumbprint (RFC 9679) as its key identifier, and the
    /// signer's public key as a COSE_Key in the unprotected header.
    /// </summary>
    [TestFixture]
    public class COSESignaturesTests
    {

        #region (private) helpers

        private static (ECPublicKeyParameters Public, ECPrivateKeyParameters Private) NewKeyPair(String Curve)
        {

            var curve      = SecNamedCurves.GetByName(Curve);
            var generator  = new ECKeyPairGenerator();
            generator.Init(new ECKeyGenerationParameters(new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H), new SecureRandom()));

            var keyPair    = generator.GenerateKeyPair();

            return ((ECPublicKeyParameters) keyPair.Public, (ECPrivateKeyParameters) keyPair.Private);

        }

        private static JObject AHeader(String Algorithm)

            => new (
                   new JProperty("alg",                Algorithm),
                   new JProperty("kid",                "is not read"),
                   new JProperty("OCPPVersion",        "ocpp2.1"),
                   new JProperty("OCPPMessageTypeId",  2),
                   new JProperty("OCPPAction",         "Reset"),
                   new JProperty("OCPPMessageId",      "4711"),
                   new JProperty("iat",                1794484800),
                   new JProperty("OCPPSignerDescription", new JObject(new JProperty("en", "Just a test!"))),
                   new JProperty("crit",               new JArray("OCPPVersion", "OCPPMessageTypeId", "OCPPAction", "OCPPMessageId", "OCPPSignerDescription"))
               );

        private static readonly Byte[] payload = CBORValue.FromMap([
                                                     new KeyValuePair<CBORValue, CBORValue>(CBORValue.FromText("type"), CBORValue.FromText("Immediate"))
                                                 ]).ToByteArray(CBORWriterOptions.Canonical);

        #endregion


        #region ASignature_IsReadAsItWasWritten_AndIsValid(Algorithm, Curve)

        [TestCase("ES256", "secp256r1")]
        [TestCase("ES384", "secp384r1")]
        [TestCase("ES512", "secp521r1")]
        public void ASignature_IsReadAsItWasWritten_AndIsValid(String Algorithm, String Curve)
        {

            var keys    = NewKeyPair(Curve);
            var header  = AHeader(Algorithm);
            var cose    = COSESignatures.Sign(header, keys.Public, keys.Private, payload);

            Assert.That(COSESignatures.TryRead(cose, out var sign1, out var read, out var carried, out var errorResponse),
                        Is.True,
                        errorResponse);

            Assert.Multiple(() => {

                Assert.That(sign1!.IsTagged,                           Is.True, "COSE_Sign1 is tag 18");
                Assert.That(sign1.IsDetached,                          Is.True);

                // The header comes back as it went, but for the key identifier.
                Assert.That(read!["alg"]?.Value<String>(),             Is.EqualTo(Algorithm));
                Assert.That(read["kid"]?.Value<String>(),              Is.EqualTo(JWS.Base64URL(COSESignatures.KeyIdentifier(keys.Public))));
                Assert.That(read["OCPPMessageTypeId"]?.Type,           Is.EqualTo(JTokenType.Integer));
                Assert.That(read["OCPPMessageTypeId"]?.Value<Int32>(), Is.EqualTo(2));
                Assert.That(read["iat"]?.Value<Int64>(),               Is.EqualTo(1794484800));
                Assert.That(JToken.DeepEquals(read["crit"],                  header["crit"]),                  Is.True);
                Assert.That(JToken.DeepEquals(read["OCPPSignerDescription"], header["OCPPSignerDescription"]), Is.True);

                Assert.That(carried,                                   Is.Not.Null, "The key the signature carries");
                Assert.That(carried!.Q.Equals(keys.Public.Q),          Is.True);

                Assert.That(COSESignatures.Verify(sign1, read, keys.Public, payload),  Is.True);

            });

        }

        #endregion

        #region TheKeyIdentifier_IsTheCOSEKeyThumbprint()

        [Test]
        public void TheKeyIdentifier_IsTheCOSEKeyThumbprint()
        {

            var keys = NewKeyPair("secp256r1");

            Assert.Multiple(() => {
                Assert.That(COSESignatures.KeyIdentifier(keys.Public),             Has.Length.EqualTo(32), "SHA-256");
                Assert.That(COSESignatures.KeyIdentifier(keys.Public),             Is.EqualTo(COSEKey.From(keys.Public).Thumbprint()));
                Assert.That(JWS.Base64URL(COSESignatures.KeyIdentifier(keys.Public)), Is.Not.EqualTo(JWS.Thumbprint(keys.Public)),
                            "The thumbprint of a COSE_Key is not that of a JWK");
            });

        }

        #endregion

        #region AnotherPayload_AnotherKey_AnotherHeader_AreNotValid()

        [Test]
        public void AnotherPayload_AnotherKey_AnotherHeader_AreNotValid()
        {

            var keys     = NewKeyPair("secp256r1");
            var others   = NewKeyPair("secp256r1");
            var cose     = COSESignatures.Sign(AHeader("ES256"), keys.Public, keys.Private, payload);

            Assert.That(COSESignatures.TryRead(cose, out var sign1, out var header, out _, out var errorResponse), Is.True, errorResponse);

            var another  = CBORValue.FromMap([
                               new KeyValuePair<CBORValue, CBORValue>(CBORValue.FromText("type"), CBORValue.FromText("OnIdle"))
                           ]).ToByteArray(CBORWriterOptions.Canonical);

            // The same signature under a protected header that says something else.
            var forged   = new COSESign1(
                               CBORValue.FromMap([
                                   new KeyValuePair<CBORValue, CBORValue>(COSEHeaderLabel.Algorithm,        CBORValue.FromInt64(-7)),
                                   new KeyValuePair<CBORValue, CBORValue>(CBORValue.FromText("OCPPAction"),  CBORValue.FromText("UpdateFirmware"))
                               ]).ToByteArray(CBORWriterOptions.Canonical),
                               sign1!.UnprotectedHeader,
                               null,
                               sign1.Signature,
                               true
                           );

            Assert.Multiple(() => {
                Assert.That(COSESignatures.Verify(sign1,  header!, keys.  Public, payload),  Is.True);
                Assert.That(COSESignatures.Verify(sign1,  header!, keys.  Public, another),  Is.False, "Another payload");
                Assert.That(COSESignatures.Verify(sign1,  header!, others.Public, payload),  Is.False, "Another key");
                Assert.That(COSESignatures.Verify(forged, header!, keys.  Public, payload),  Is.False, "Another protected header");
            });

        }

        #endregion

        #region WhatIsNoSignatureOfAMessage_IsNotRead()

        [Test]
        public void WhatIsNoSignatureOfAMessage_IsNotRead()
        {

            var keys        = NewKeyPair("secp256r1");

            // A COSE_Sign1 that brings a payload of its own signs something else.
            var withPayload = COSESign1.Sign(payload, keys.Private, COSEAlgorithm.ES256).ToByteArray();

            Assert.Multiple(() => {

                Assert.That(COSESignatures.TryRead([ 0x01, 0x02, 0x03 ], out _, out _, out _, out var e1),  Is.False);
                Assert.That(e1,                                                                             Is.Not.Null);

                Assert.That(COSESignatures.TryRead(withPayload, out _, out _, out _, out var e2),           Is.False);
                Assert.That(e2,                                                                             Does.Contain("payload of its own"));

            });

        }

        #endregion

        #region ASignature_InJSON_IsItsBytes()

        /// <summary>
        /// In a JSON message a COSE signature is its bytes, as base64url under "cose".
        /// </summary>
        [Test]
        public void ASignature_InJSON_IsItsBytes()
        {

            var keys  = NewKeyPair("secp256r1");
            var cose  = COSESignatures.Sign(AHeader("ES256"), keys.Public, keys.Private, payload);

            Assert.That(Signature.TryParseCOSE(cose, out var signature, out var e1),  Is.True, e1);

            var json  = signature!.ToJSON();

            Assert.That(json.Properties().Select(property => property.Name),          Is.EqualTo(new[] { "cose" }));
            Assert.That(json["cose"]?.Value<String>(),                                Is.EqualTo(JWS.Base64URL(cose)));

            Assert.That(Signature.TryParse(json, out var read, out var e2),           Is.True, e2);

            Assert.Multiple(() => {
                Assert.That(read!.IsCOSE,                              Is.True);
                Assert.That(read.IsJWS,                                Is.False);
                Assert.That(read.COSE,                                 Is.EqualTo(cose));
                Assert.That(read,                                      Is.EqualTo(signature));
                Assert.That(read.Clone(),                              Is.EqualTo(signature));
                Assert.That(read.Algorithm,                            Is.EqualTo(CryptoAlgorithm.Secp256r1));
                Assert.That(read.Description?.FirstText(),             Is.EqualTo("Just a test!"));
                Assert.That(read.Timestamp?.ToUnixTimeSeconds(),       Is.EqualTo(1794484800));
                Assert.That(read.KeyId,                                Is.EqualTo(keys.Public.Q.GetEncoded(false)));
            });

        }

        #endregion

    }

}
