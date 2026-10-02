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
    /// An optional value that is there but not valid is refused, and one that
    /// is not there is no reason to refuse.
    /// </summary>
    /// <remarks>
    /// These parsers asked for their optional values with a negation, if
    /// (!JSON.ParseOptional...(...)), and looked for an error only while it
    /// returned false: a value not valid, for which most overloads of Illias
    /// returned true, was passed over, and the rest read as if it were not
    /// there.
    /// </remarks>
    [TestFixture]
    public static class OptionalValueTests
    {

        /// <summary>
        /// What an AuthStartResult and an AuthStopResult must say.
        /// </summary>
        private const String authResult = """
                                          {
                                              "authorizatorId":     "AUTH1",
                                              "responseTimestamp":  "2026-10-01T08:00:00.000Z",
                                              "result":             "Unspecified",
                                              "runtime":            12
                                          }
                                          """;


        #region AuthStartResult

        [Test]
        public static void AnAuthStartResultWithAMaxPowerNotValidIsRefused()

            => AssertRefused(JObject.Parse(authResult), "maxPower", "much",
                             (JObject json, out String? error) => AuthStartResult.TryParse(json, out _, out error, null, null, null));

        [Test]
        public static void AnAuthStartResultWithAUILanguageNotValidIsRefused()

            => AssertRefused(JObject.Parse(authResult), "uiLanguage", "no language",
                             (JObject json, out String? error) => AuthStartResult.TryParse(json, out _, out error, null, null, null));

        #endregion

        #region AuthStopResult

        [Test]
        public static void AnAuthStopResultWithACachedResultEndOfLifeTimeNotValidIsRefused()

            => AssertRefused(JObject.Parse(authResult), "cachedResultEndOfLifeTime", "not a time",
                             (JObject json, out String? error) => AuthStopResult.TryParse(json, out _, out error, null, null, null));

        #endregion

        #region ChargingCable

        [Test]
        public static void AChargingCableWithALengthNotValidIsRefused()

            => AssertRefused(new JObject(new JProperty("lossCompensationName", "Standard")), "length", "long",
                             (JObject json, out String? error) => ChargingCable.TryParse(json, out _, out error));

        [Test]
        public static void AChargingCableWithAResistanceNotValidIsRefused()

            => AssertRefused(new JObject(new JProperty("lossCompensationName", "Standard")), "resistance", "high",
                             (JObject json, out String? error) => ChargingCable.TryParse(json, out _, out error));

        #endregion


        #region (private) AssertRefused(JSON, PropertyName, NotValid, TryParse)

        private delegate Boolean TryParser(JObject JSON, out String? ErrorResponse);

        /// <summary>
        /// The given JSON, without the optional property, is read; with a value
        /// not valid for it, it is refused, and said why.
        /// </summary>
        private static void AssertRefused(JObject    JSON,
                                          String     PropertyName,
                                          JToken     NotValid,
                                          TryParser  TryParse)
        {

            JSON.Remove(PropertyName);

            Assert.That(TryParse(JSON, out var error), Is.True, $"Without '{PropertyName}' it is not read: {error}");

            JSON[PropertyName] = NotValid;

            var parsed = TryParse(JSON, out error);

            Assert.Multiple(() => {
                Assert.That(parsed, Is.False,    $"With '{PropertyName}' not valid it is read.");
                Assert.That(error,  Is.Not.Null, $"With '{PropertyName}' not valid it is refused without a reason.");
            });

        }

        #endregion

    }

}
