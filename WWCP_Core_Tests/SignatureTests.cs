/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

using NUnit.Framework;

using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests
{

    /// <summary>
    /// A cryptographic signature read back as it is written.
    /// </summary>
    /// <remarks>
    /// Its encoding was written as "encoding" and read as "encodingMethod", and
    /// its key identification and value were read as BASE64 whatever they had
    /// been written in: a signature in HEX or BASE32 came back as other bytes.
    /// </remarks>
    [TestFixture]
    public class SignatureTests
    {

        private static readonly Byte[] keyId = [ 0x01, 0x02, 0x03, 0xFE ];
        private static readonly Byte[] value = [ 0x30, 0x45, 0x02, 0x21, 0x00, 0xAB, 0xCD ];

        #region WrittenIsRead(Encoding)

        [TestCase("hex")]
        [TestCase("base32")]
        [TestCase("base64")]
        public void WrittenIsRead(String Encoding)
        {

            var signature = new Signature(keyId, value, Encoding: CryptoEncoding.Parse(Encoding), Name: "station");
            var written   = signature.ToJSON();

            Assert.That(Signature.TryParse(written, out var again, out var errorResponse), Is.True, errorResponse);

            Assert.That(again!.KeyId,             Is.EqualTo(keyId));
            Assert.That(again. Value,             Is.EqualTo(value));
            Assert.That(again. Encoding,          Is.EqualTo(CryptoEncoding.Parse(Encoding)));
            Assert.That(JToken.DeepEquals(again.ToJSON(), written), Is.True);

        }

        #endregion

        #region EncodingUnderItsOldKey_IsStillRead()

        [Test]
        public void EncodingUnderItsOldKey_IsStillRead()
        {

            var json = new JObject(
                           new JProperty("keyId",    "010203FE"),
                           new JProperty("value",    "30450221 00ABCD".Replace(" ", "")),
                           new JProperty("encoding", "hex")
                       );

            Assert.That(Signature.TryParse(json, out var signature, out var errorResponse), Is.True, errorResponse);
            Assert.That(signature!.KeyId, Is.EqualTo(keyId));
            Assert.That(signature. Value, Is.EqualTo(value));

        }

        #endregion

    }

}
