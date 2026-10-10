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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork.Ids
{

    /// <summary>
    /// Identifications parsed from text and compared as default values.
    /// </summary>
    [TestFixture]
    public class IdParsingTests
    {

        #region Unknown_country_codes_are_not_German_operators()

        /// <summary>
        /// Only a bare operator number like "822" is German by default;
        /// an unknown country or telephone code is no operator at all.
        /// </summary>
        [Test]
        public void Unknown_country_codes_are_not_German_operators()
        {

            Assert.That(ChargingStationOperator_Id.TryParse("QQ*ABC"),          Is.Null);
            Assert.That(ChargingStationOperator_Id.TryParse("+12345*123"),      Is.Null);
            Assert.That(EVSE_Id.                   TryParse("QQ*ABC*E1"),       Is.Null);
            Assert.That(GridOperator_Id.           TryParse("QQ*ABC", out _),   Is.False);
            Assert.That(() => GridOperator_Id.Parse("QQ*ABC"),                  Throws.ArgumentException.With.Message.Contains("'QQ*ABC'"));

            Assert.That(ChargingStationOperator_Id.TryParse("822")?.    CountryCode, Is.EqualTo(Country.Germany));
            Assert.That(ChargingStationOperator_Id.TryParse("+49*822")?.CountryCode, Is.EqualTo(Country.Germany));
            Assert.That(GridOperator_Id.Parse("822").CountryCode,                    Is.EqualTo(Country.Germany));

        }

        #endregion

        #region Operator_IDs_parse_in_every_culture()

        /// <summary>
        /// In Turkish, "i" uppers to a dotted "İ", which no operator ID pattern accepts.
        /// </summary>
        [Test]
        [SetCulture("tr-TR")]
        public void Operator_IDs_parse_in_every_culture()
        {

            Assert.That(ChargingStationOperator_Id.TryParse("it*abc")?.ToString(), Is.EqualTo("IT*ABC"));

        }

        #endregion

        #region A_brand_ID_that_cannot_be_parsed_is_null()

        [Test]
        public void A_brand_ID_that_cannot_be_parsed_is_null()
        {

            Assert.That(Brand_Id.TryParse(""),     Is.Null);
            Assert.That(Brand_Id.TryParse("   "),  Is.Null);

        }

        #endregion

        #region Default_IDs_compare_without_exceptions()

        /// <summary>
        /// The == operators meant "both null" to be equal; for these structs that is default.
        /// </summary>
        [Test]
        public void Default_IDs_compare_without_exceptions()
        {

            Assert.That(default(Brand_Id)                == default(Brand_Id),                Is.True);
            Assert.That(default(EVSEGroup_Id)            == default(EVSEGroup_Id),            Is.True);
            Assert.That(default(ChargingStationGroup_Id) == default(ChargingStationGroup_Id), Is.True);
            Assert.That(default(ChargingPoolGroup_Id)    == default(ChargingPoolGroup_Id),    Is.True);
            Assert.That(default(ChargingTariffGroup_Id)  == default(ChargingTariffGroup_Id),  Is.True);
            Assert.That(default(ParkingSpace_Id)         == default(ParkingSpace_Id),         Is.True);
            Assert.That(default(ParkingProduct_Id)       == default(ParkingProduct_Id),       Is.True);

            Assert.That(default(Brand_Id)     == Brand_Id.    Parse("brand"),       Is.False);
            Assert.That(default(EVSEGroup_Id) == EVSEGroup_Id.Parse("DE*ABC*GE1"),  Is.False);
            Assert.That(() => default(Brand_Id).    GetHashCode(), Throws.Nothing);
            Assert.That(() => default(EVSEGroup_Id).GetHashCode(), Throws.Nothing);

        }

        #endregion


    }

}
