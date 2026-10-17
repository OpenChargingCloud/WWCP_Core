/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using cloud.charging.open.protocols.WWCP.POI;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests
{

    /// <summary>
    /// What an energy meter measures and how it signs its measurements, as POI tells them: its role and its
    /// signature methods, read in any case and written as predefined.
    /// </summary>
    [TestFixture]
    public static class EnergyMeterTests
    {

        [Test]
        public static void Role_and_signature_methods_survive_JSON()
        {

            var meter = new EnergyMeter(EnergyMeter_Id.Parse("meter-1"),
                                        Role:              EnergyMeterRole.EVSE,
                                        SerialNumber:      "1ESY1161229886",
                                        SignatureMethods:  [ SignatureMethod.OCMF, SignatureMethod.Parse("FooCrypt01"), SignatureMethod.EDL40, SignatureMethod.Parse("ocmf") ]);

            var json  = meter.ToJSON();

            Assert.That(EnergyMeter.TryParse(json, out var parsed, out var errorResponse), Is.True, errorResponse);

            using (Assert.EnterMultipleScope())
            {

                Assert.That(json["role"]?.Value<String>(),                      Is.EqualTo("evse"));
                Assert.That(json["signatureMethods"]!.Values<String>(),         Is.EqualTo(new[] { "EDL40", "FooCrypt01", "OCMF" }));

                Assert.That(parsed!.Role,                                       Is.EqualTo(EnergyMeterRole.EVSE));
                Assert.That(parsed.SignatureMethods,                            Is.EqualTo(new[] { SignatureMethod.EDL40, SignatureMethod.Parse("FooCrypt01"), SignatureMethod.OCMF }));
                Assert.That(parsed.ToJSON().ToString(),                         Is.EqualTo(json.ToString()));
                Assert.That(parsed,                                             Is.EqualTo(meter));
                Assert.That(meter.Clone().ToJSON().ToString(),                  Is.EqualTo(json.ToString()));

            }

        }

        [Test]
        public static void Role_and_signature_methods_are_read_in_any_case()
        {

            var json = new JObject(
                           new JProperty("id",                "meter-1"),
                           new JProperty("role",              "PV"),
                           new JProperty("signatureMethods",  new JArray("ocmf", "alfencrypt03")),
                           new JProperty("lastChange",        "2026-10-17T12:00:00.000Z")
                       );

            Assert.That(EnergyMeter.TryParse(json, out var meter, out var errorResponse), Is.True, errorResponse);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(meter!.Role,                                        Is.EqualTo(EnergyMeterRole.PV));
                Assert.That(meter.ToJSON()["role"]?.Value<String>(),            Is.EqualTo("pv"));
                Assert.That(meter.ToJSON()["signatureMethods"]!.Values<String>(), Is.EqualTo(new[] { "AlfenCrypt03", "OCMF" }));
            }

        }

    }

}
