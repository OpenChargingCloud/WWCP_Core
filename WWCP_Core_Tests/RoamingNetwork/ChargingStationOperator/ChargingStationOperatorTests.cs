/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Illias;

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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                ClassicAssert.AreEqual ("DE*GEF",                                              DE_GEF.Id.         ToString());
                ClassicAssert.AreEqual ("GraphDefined CSO",                                    DE_GEF.Name.       FirstText());
                ClassicAssert.AreEqual ("powered by GraphDefined GmbH",                        DE_GEF.Description.FirstText());

                ClassicAssert.AreEqual (ChargingStationOperatorAdminStatusTypes.OutOfService,  DE_GEF.AdminStatus);
                ClassicAssert.AreEqual (1,                                                     DE_GEF.AdminStatusSchedule().Count());

                ClassicAssert.AreEqual (ChargingStationOperatorStatusTypes.Offline,            DE_GEF.Status);
                ClassicAssert.AreEqual (1,                                                     DE_GEF.StatusSchedule().     Count());


                ClassicAssert.AreEqual (1,                                                     roamingNetwork.ChargingStationOperators.    Count());
                ClassicAssert.AreEqual (1,                                                     roamingNetwork.ChargingStationOperatorIds().Count());


                ClassicAssert.IsTrue   (roamingNetwork.ChargingStationOperatorExists (ChargingStationOperator_Id.Parse("DE*GEF")));
                ClassicAssert.IsNotNull(roamingNetwork.GetChargingStationOperatorById(ChargingStationOperator_Id.Parse("DE*GEF")));

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

            ClassicAssert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                var DE_XXX = roamingNetwork.CreateChargingStationOperator(
                                                Id:           ChargingStationOperator_Id.Parse("DE*XXX"),
                                                Name:         I18NString.Create(Languages.de, "XXX CSO"),
                                                Description:  I18NString.Create(Languages.de, "powered by GraphDefined CSOs GmbH")
                                            ).Result.ChargingStationOperator;

                ClassicAssert.IsNotNull(DE_XXX);

                if (DE_XXX is not null)
                {

                    ClassicAssert.AreEqual ("DE*XXX",                                             DE_XXX.Id.         ToString());
                    ClassicAssert.AreEqual ("XXX CSO",                                            DE_XXX.Name.       FirstText());
                    ClassicAssert.AreEqual ("powered by GraphDefined CSOs GmbH",                  DE_XXX.Description.FirstText());

                    ClassicAssert.AreEqual (ChargingStationOperatorAdminStatusTypes.Operational,  DE_XXX.AdminStatus);
                    ClassicAssert.AreEqual (ChargingStationOperatorStatusTypes.Available,         DE_XXX.Status);

                    ClassicAssert.IsTrue   (roamingNetwork.ChargingStationOperatorExists (ChargingStationOperator_Id.Parse("DE*XXX")));
                    ClassicAssert.IsNotNull(roamingNetwork.GetChargingStationOperatorById(ChargingStationOperator_Id.Parse("DE*XXX")));

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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF.AdminStatus = ChargingStationOperatorAdminStatusTypes.InternalUse;
                ClassicAssert.AreEqual(ChargingStationOperatorAdminStatusTypes.InternalUse,  DE_GEF.AdminStatus);
                ClassicAssert.AreEqual("internalUse, outOfService",                          DE_GEF.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(2,                                                    DE_GEF.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF.AdminStatus = ChargingStationOperatorAdminStatusTypes.Operational;
                ClassicAssert.AreEqual(ChargingStationOperatorAdminStatusTypes.Operational,  DE_GEF.AdminStatus);
                ClassicAssert.AreEqual("operational, internalUse, outOfService",             DE_GEF.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(3,                                                    DE_GEF.AdminStatusSchedule().Count());


                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", DE_GEF.                                   GenerateAdminStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", new IChargingStationOperator[] { DE_GEF }.GenerateAdminStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", roamingNetwork.                           GenerateChargingStationOperatorAdminStatusReport().ToString());


                var jsonStatusReport = DE_GEF.GenerateAdminStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                ClassicAssert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingStationOperatorAdminStatusReport\",\"count\":1,\"report\":{\"operational\":{\"count\":1,\"percentage\":100.0}}}",
                                jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None));

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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF.Status = ChargingStationOperatorStatusTypes.InDeployment;
                ClassicAssert.AreEqual(ChargingStationOperatorStatusTypes.InDeployment, DE_GEF.Status);
                ClassicAssert.AreEqual("inDeployment, offline",                         DE_GEF.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(2,                                               DE_GEF.StatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF.Status = ChargingStationOperatorStatusTypes.Error;
                ClassicAssert.AreEqual(ChargingStationOperatorStatusTypes.Error,        DE_GEF.Status);
                ClassicAssert.AreEqual("error, inDeployment, offline",                  DE_GEF.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(3,                                               DE_GEF.StatusSchedule().Count());


                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", DE_GEF.                                   GenerateStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", new IChargingStationOperator[] { DE_GEF }.GenerateStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", roamingNetwork.                           GenerateChargingStationOperatorStatusReport().ToString());


                var jsonStatusReport = DE_GEF.GenerateStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                ClassicAssert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingStationOperatorStatusReport\",\"count\":1,\"report\":{\"error\":{\"count\":1,\"percentage\":100.0}}}",
                                jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None));

            }

        }

        #endregion


    }

}
