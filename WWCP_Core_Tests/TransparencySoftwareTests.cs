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
    /// A transparency software read back as it is written.
    /// </summary>
    /// <remarks>
    /// Its license was written as a text under "open_source_license" and read
    /// as an object under "openSourceLicense", so nothing written was read.
    /// </remarks>
    [TestFixture]
    public class TransparencySoftwareTests
    {

        private const String Sample = """
            {
                "name":                "Chargy Transparency Software",
                "version":             "1.0",
                "open_source_license": { "@id": "AGPL-3.0", "URLs": [ "https://www.gnu.org/licenses/agpl-3.0.html" ] },
                "vendor":              "GraphDefined GmbH"
            }
            """;

        #region WrittenIsRead()

        [Test]
        public void WrittenIsRead()
        {

            Assert.That(TransparencySoftware.TryParse(JObject.Parse(Sample), out var software, out var errorResponse), Is.True, errorResponse);

            var written = software!.ToJSON();

            Assert.That(written["open_source_license"]?.Type, Is.EqualTo(JTokenType.Object));

            Assert.That(TransparencySoftware.TryParse(written, out var again, out errorResponse), Is.True,
                        $"What was written was not read: {errorResponse}{Environment.NewLine}{written.ToString(Newtonsoft.Json.Formatting.None)}");

            Assert.That(JToken.DeepEquals(again!.ToJSON(), written), Is.True);

        }

        #endregion

        #region OpenSourceLicenseUnderItsOldKey_IsStillRead()

        [Test]
        public void OpenSourceLicenseUnderItsOldKey_IsStillRead()
        {

            var json = JObject.Parse(Sample);
            json["openSourceLicense"] = json["open_source_license"];
            json.Remove("open_source_license");

            Assert.That(TransparencySoftware.TryParse(json, out var software, out var errorResponse), Is.True, errorResponse);
            Assert.That(software!.OpenSourceLicense.Id.ToString(), Is.EqualTo("AGPL-3.0"));

        }

        #endregion

    }

}
