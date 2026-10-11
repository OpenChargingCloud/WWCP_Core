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

using cloud.charging.open.protocols.WWCP.POI;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork
{

    /// <summary>
    /// Unit tests for charging station operators.
    /// </summary>
    [TestFixture]
    public class ChargingStationOperatorTests : AChargingStationOperatorTests
    {

        #region ChargingStationOperator_Init_Test()

        /// <summary>
        /// A test for creating a charging station operator within a roaming network.
        /// </summary>
        [Test]
        public void ChargingStationOperator_Init_Test()
        {

            Assert.That(roamingNetwork, Is.Not.Null);
            Assert.That(DE_GEF,         Is.Not.Null);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                Assert.That(DE_GEF.Id.ToString(), Is.EqualTo("DE*GEF"));
                Assert.That(DE_GEF.Name.FirstText(), Is.EqualTo("GraphDefined CSO"));
                Assert.That(DE_GEF.Description.FirstText(), Is.EqualTo("powered by GraphDefined GmbH"));

                Assert.That(DE_GEF.AdminStatus, Is.EqualTo(ChargingStationOperatorAdminStatusType.OutOfService));
                Assert.That(DE_GEF.AdminStatusSchedule().Count(), Is.EqualTo(1));

                Assert.That(DE_GEF.Status, Is.EqualTo(ChargingStationOperatorStatusType.Offline));
                Assert.That(DE_GEF.StatusSchedule().Count(), Is.EqualTo(1));


                Assert.That(roamingNetwork.ChargingStationOperators.Count(), Is.EqualTo(1));
                Assert.That(roamingNetwork.ChargingStationOperatorIds().Count(), Is.EqualTo(1));


                Assert.That(roamingNetwork.ChargingStationOperatorExists (ChargingStationOperator_Id.Parse("DE*GEF")), Is.True);
                Assert.That(roamingNetwork.GetChargingStationOperatorById(ChargingStationOperator_Id.Parse("DE*GEF")), Is.Not.Null);

            }

        }

        #endregion

        #region ChargingStationOperator_Init_DefaultStatus_Test()

        /// <summary>
        /// A test for creating a charging station operator within a roaming network.
        /// </summary>
        [Test]
        public void ChargingStationOperator_Init_DefaultStatus_Test()
        {

            Assert.That(roamingNetwork, Is.Not.Null);

            if (roamingNetwork is not null)
            {

                var DE_XXX = roamingNetwork.CreateChargingStationOperator(
                                                Id:           ChargingStationOperator_Id.Parse("DE*XXX"),
                                                Name:         I18NString.Create(Languages.de, "XXX CSO"),
                                                Description:  I18NString.Create(Languages.de, "powered by GraphDefined CSOs GmbH")
                                            ).Result.ChargingStationOperator;

                Assert.That(DE_XXX, Is.Not.Null);

                if (DE_XXX is not null)
                {

                    Assert.That(DE_XXX.Id.ToString(), Is.EqualTo("DE*XXX"));
                    Assert.That(DE_XXX.Name.FirstText(), Is.EqualTo("XXX CSO"));
                    Assert.That(DE_XXX.Description.FirstText(), Is.EqualTo("powered by GraphDefined CSOs GmbH"));

                    Assert.That(DE_XXX.AdminStatus, Is.EqualTo(ChargingStationOperatorAdminStatusType.Operational));
                    Assert.That(DE_XXX.Status, Is.EqualTo(ChargingStationOperatorStatusType.Available));

                    Assert.That(roamingNetwork.ChargingStationOperatorExists (ChargingStationOperator_Id.Parse("DE*XXX")), Is.True);
                    Assert.That(roamingNetwork.GetChargingStationOperatorById(ChargingStationOperator_Id.Parse("DE*XXX")), Is.Not.Null);

                }

            }

        }

        #endregion


        #region ChargingStationOperator_AdminStatus_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void ChargingStationOperator_AdminStatus_Test()
        {

            Assert.That(roamingNetwork, Is.Not.Null);
            Assert.That(DE_GEF,         Is.Not.Null);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF.AdminStatus = ChargingStationOperatorAdminStatusType.InternalUse;
                Assert.That(DE_GEF.AdminStatus, Is.EqualTo(ChargingStationOperatorAdminStatusType.InternalUse));
                Assert.That(DE_GEF.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("internalUse, outOfService"));
                Assert.That(DE_GEF.AdminStatusSchedule().Count(), Is.EqualTo(2));

                Thread.Sleep(1000);

                DE_GEF.AdminStatus = ChargingStationOperatorAdminStatusType.Operational;
                Assert.That(DE_GEF.AdminStatus, Is.EqualTo(ChargingStationOperatorAdminStatusType.Operational));
                Assert.That(DE_GEF.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("operational, internalUse, outOfService"));
                Assert.That(DE_GEF.AdminStatusSchedule().Count(), Is.EqualTo(3));


                Assert.That(DE_GEF.GenerateAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(new IChargingStationOperator[] { DE_GEF }.GenerateAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(roamingNetwork.GenerateChargingStationOperatorAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));


                var jsonStatusReport = DE_GEF.GenerateAdminStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.That(jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None),
                                Is.EqualTo("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingStationOperatorAdminStatusReport\",\"count\":1,\"report\":{\"operational\":{\"count\":1,\"percentage\":100.0}}}"));

            }

        }

        #endregion

        #region ChargingStationOperator_Status_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void ChargingStationOperator_Status_Test()
        {

            Assert.That(roamingNetwork, Is.Not.Null);
            Assert.That(DE_GEF,         Is.Not.Null);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF.Status = ChargingStationOperatorStatusType.InDeployment;
                Assert.That(DE_GEF.Status, Is.EqualTo(ChargingStationOperatorStatusType.InDeployment));
                Assert.That(DE_GEF.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("inDeployment, offline"));
                Assert.That(DE_GEF.StatusSchedule().Count(), Is.EqualTo(2));

                Thread.Sleep(1000);

                DE_GEF.Status = ChargingStationOperatorStatusType.Error;
                Assert.That(DE_GEF.Status, Is.EqualTo(ChargingStationOperatorStatusType.Error));
                Assert.That(DE_GEF.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("error, inDeployment, offline"));
                Assert.That(DE_GEF.StatusSchedule().Count(), Is.EqualTo(3));


                Assert.That(DE_GEF.GenerateStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(new IChargingStationOperator[] { DE_GEF }.GenerateStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(roamingNetwork.GenerateChargingStationOperatorStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));


                var jsonStatusReport = DE_GEF.GenerateStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.That(jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None),
                                Is.EqualTo("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingStationOperatorStatusReport\",\"count\":1,\"report\":{\"error\":{\"count\":1,\"percentage\":100.0}}}"));

            }

        }

        #endregion


    }

}
