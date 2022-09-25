/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP.tests.roamingNetwork
{

    /// <summary>
    /// Unit tests for charging station operators.
    /// </summary>
    [TestFixture]
    public class ChargingStationOperatorTests : AChargingStationOperatorTests
    {

        #region ChargingStationOperator_Init_Test1()

        /// <summary>
        /// A test for the roaming network constructor.
        /// </summary>
        [Test]
        public void ChargingStationOperator_Init_Test1()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                Assert.AreEqual ("DE*GEF",                                             DE_GEF.Id.         ToString());
                //Assert.AreEqual ("PRODUCTION",                                         roamingNetwork.Name.       FirstText());
                //Assert.AreEqual ("The main production roaming network",                roamingNetwork.Description.FirstText());

                Assert.AreEqual (ChargingStationOperatorAdminStatusTypes.Operational,  DE_GEF.AdminStatus);
                Assert.AreEqual (1,                                                    DE_GEF.AdminStatusSchedule().Count());

                Assert.AreEqual (ChargingStationOperatorStatusTypes.Available,         DE_GEF.Status);
                Assert.AreEqual (1,                                                    DE_GEF.StatusSchedule().     Count());


                Assert.AreEqual (1,                                                    roamingNetwork.ChargingStationOperators.  Count());
                Assert.AreEqual (1,                                                    roamingNetwork.ChargingStationOperatorIds.Count());

                Assert.IsTrue   (roamingNetwork.ContainsChargingStationOperator(ChargingStationOperator_Id.Parse("DE*GEF")));
                Assert.IsNotNull(roamingNetwork.GetChargingStationOperatorById (ChargingStationOperator_Id.Parse("DE*GEF")));


            }

        }

        #endregion

        #region ChargingStationOperator_AdminStatus_Test1()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void ChargingStationOperator_AdminStatus_Test1()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                // Status entries are compared by their ISO 8601 timestamp!
                Thread.Sleep(1000);

                DE_GEF.AdminStatus = ChargingStationOperatorAdminStatusTypes.OutOfService;
                Assert.AreEqual(ChargingStationOperatorAdminStatusTypes.OutOfService, DE_GEF.AdminStatus);
                Assert.AreEqual(2,                                                    DE_GEF.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF.AdminStatus = ChargingStationOperatorAdminStatusTypes.Operational;
                Assert.AreEqual(ChargingStationOperatorAdminStatusTypes.Operational,  DE_GEF.AdminStatus);
                Assert.AreEqual(3,                                                    DE_GEF.AdminStatusSchedule().Count());

            }

        }

        #endregion

        #region ChargingStationOperator_Status_Test1()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void ChargingStationOperator_Status_Test1()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                // Status entries are compared by their ISO 8601 timestamp!
                Thread.Sleep(1000);

                DE_GEF.Status = ChargingStationOperatorStatusTypes.InDeployment;
                Assert.AreEqual(ChargingStationOperatorStatusTypes.InDeployment, DE_GEF.Status);
                Assert.AreEqual("InDeployment, Available",                       DE_GEF.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                                               DE_GEF.StatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF.Status = ChargingStationOperatorStatusTypes.Faulted;
                Assert.AreEqual(ChargingStationOperatorStatusTypes.Faulted,      DE_GEF.Status);
                Assert.AreEqual("Faulted, InDeployment, Available",              DE_GEF.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                                               DE_GEF.StatusSchedule().Count());

            }

        }

        #endregion


    }

}
