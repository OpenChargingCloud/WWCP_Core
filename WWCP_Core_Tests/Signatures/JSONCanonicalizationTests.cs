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

using System.Globalization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.Signatures
{

    /// <summary>
    /// RFC 8785: a signer and a verifier that canonicalize the same JSON value
    /// get the same bytes, however the text looked on the wire.
    /// </summary>
    [TestFixture]
    public class JSONCanonicalizationTests
    {

        #region (private) Read(Text)

        /// <summary>
        /// Read JSON as a received message is read: strings stay strings.
        /// </summary>
        private static JToken Read(String Text)
            => JSONMessages.ParseArray($"[{Text}]")[0];

        #endregion


        #region TheSampleOfRFC8785()

        /// <summary>
        /// The example of RFC 8785 section 3.2.4.
        /// </summary>
        [Test]
        public void TheSampleOfRFC8785()
        {

            var input = Read("""
                {
                  "numbers": [333333333.33333329, 1E30, 4.50, 2e-3, 0.000000000000000000000000001],
                  "string": "\u20ac$\u000F\u000aA'\u0042\u0022\u005c\\\"\/",
                  "literals": [null, true, false]
                }
                """);

            Assert.That(JSONCanonicalization.Canonicalize(input),
                        Is.EqualTo("""{"literals":[null,true,false],"numbers":[333333333.3333333,1e+30,4.5,0.002,1e-27],"string":"€$\u000f\nA'B\"\\\\\"/"}"""));

        }

        #endregion

        #region NumbersAreWrittenAsECMAScriptWritesThem(Hex, Expected)

        private static IEnumerable<TestCaseData> Numbers

            => IndependentVectors.Numbers.Select(vector => new TestCaseData(vector[0], vector[1]).SetArgDisplayNames(vector[0]));

        /// <summary>
        /// Every number as V8 writes it: the bit patterns of RFC 8785 Appendix B, and random ones.
        /// </summary>
        [TestCaseSource(nameof(Numbers))]
        public void NumbersAreWrittenAsECMAScriptWritesThem(String Hex, String Expected)
        {

            var value = BitConverter.Int64BitsToDouble(Int64.Parse(Hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture));

            Assert.That(JSONCanonicalization.Number(value), Is.EqualTo(Expected));

        }

        #endregion

        #region MembersAreSortedByTheirUTF16CodeUnits()

        /// <summary>
        /// The example of RFC 8785 section 3.2.3: a surrogate pair sorts before U+FB33.
        /// </summary>
        [Test]
        public void MembersAreSortedByTheirUTF16CodeUnits()
        {

            var input = Read("""
                {
                  "\u20ac": "Euro Sign",
                  "\r": "Carriage Return",
                  "\ufb33": "Hebrew Letter Dalet With Dagesh",
                  "1": "One",
                  "\ud83d\ude00": "Emoji: Grinning Face",
                  "\u0080": "Control",
                  "\u00f6": "Latin Small Letter O With Diaeresis"
                }
                """);

            var values = ((JObject) JToken.Parse(JSONCanonicalization.Canonicalize(input))).Properties().Select(property => property.Value.Value<String>());

            Assert.That(values, Is.EqualTo(new[] {
                                    "Carriage Return",
                                    "One",
                                    "Control",
                                    "Latin Small Letter O With Diaeresis",
                                    "Euro Sign",
                                    "Emoji: Grinning Face",
                                    "Hebrew Letter Dalet With Dagesh"
                                }));

        }

        #endregion

        #region WhitespaceOrderAndSpellingDoNotMatter()

        [Test]
        public void WhitespaceOrderAndSpellingDoNotMatter()
        {

            var sent      = Read("""{ "b": [ 1.50, 2E2 ], "a": { "y": "A", "x": null } }""");
            var received  = Read("""{"a":{"x":null,"y":"A"},"b":[1.5,200]}""");

            Assert.That(JSONCanonicalization.CanonicalizeToUTF8(sent),
                        Is.EqualTo(JSONCanonicalization.CanonicalizeToUTF8(received)));

        }

        #endregion

        #region AChangedValueChangesTheBytes()

        [Test]
        public void AChangedValueChangesTheBytes()
        {

            Assert.That(JSONCanonicalization.Canonicalize(Read("""{ "type": "Immediate" }""")),
                        Is.Not.EqualTo(JSONCanonicalization.Canonicalize(Read("""{ "type": "OnIdle" }"""))));

        }

        #endregion

        #region AValueNewtonsoftHoldsAsADateIsTheStringThePeerReceives()

        /// <summary>
        /// A signer may build its JSON from typed values. What it canonicalizes is the
        /// string Newtonsoft writes, which is what the peer reads back and canonicalizes.
        /// </summary>
        [Test]
        public void AValueNewtonsoftHoldsAsADateIsTheStringThePeerReceives()
        {

            var sent      = new JObject(
                                new JProperty("timestamp", new DateTimeOffset(2026, 11, 11, 10, 0, 0, TimeSpan.FromHours(1))),
                                new JProperty("id",        Guid.Parse("6f9619ff-8b86-d011-b42d-00c04fc964ff")),
                                new JProperty("url",       new Uri("https://open.charging.cloud/x"))
                            );

            var received  = Read(sent.ToString(Formatting.None));

            Assert.Multiple(() => {
                Assert.That(received["timestamp"]!.Type, Is.EqualTo(JTokenType.String));
                Assert.That(JSONCanonicalization.Canonicalize(sent), Is.EqualTo(JSONCanonicalization.Canonicalize(received)));
            });

        }

        #endregion

        #region ControlCharactersAreEscapedAsJSONStringifyEscapesThem()

        [Test]
        public void ControlCharactersAreEscapedAsJSONStringifyEscapesThem()
        {

            Assert.That(JSONCanonicalization.Canonicalize(new JValue("\u0001\u001f\u007f\u2028/")),
                        Is.EqualTo("\"\\u0001\\u001f\u007f\u2028/\""));

        }

        #endregion

        #region NaNAndInfinityAreRefused()

        [Test]
        public void NaNAndInfinityAreRefused()
        {

            Assert.Multiple(() => {
                Assert.Throws<ArgumentException>(() => JSONCanonicalization.Canonicalize(new JValue(Double.NaN)));
                Assert.Throws<ArgumentException>(() => JSONCanonicalization.Canonicalize(new JValue(Double.PositiveInfinity)));
            });

        }

        #endregion

    }

}
