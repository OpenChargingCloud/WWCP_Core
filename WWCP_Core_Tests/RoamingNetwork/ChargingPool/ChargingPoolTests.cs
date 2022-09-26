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

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork
{

    /// <summary>
    /// Unit tests for charging pools.
    /// </summary>
    [TestFixture]
    public class ChargingPoolTests : AChargingPoolTests
    {

        #region ChargingPool_Init_Test()

        /// <summary>
        /// A test for creating a charging pool within a charging station operator.
        /// </summary>
        [Test]
        public void ChargingPool_Init_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF_P0001);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                Assert.AreEqual ("DE*GEF*P0001",                                 DE_GEF_P0001.Id.         ToString());
                Assert.AreEqual ("GraphDefined Charging Pool #1",                DE_GEF_P0001.Name.       FirstText());
                Assert.AreEqual ("powered by GraphDefined Charging Pools GmbH",  DE_GEF_P0001.Description.FirstText());

                Assert.AreEqual (ChargingPoolAdminStatusTypes.OutOfService,      DE_GEF_P0001.AdminStatus);
                Assert.AreEqual (1,                                              DE_GEF_P0001.AdminStatusSchedule().Count());

                Assert.AreEqual (ChargingPoolStatusTypes.Offline,                DE_GEF_P0001.Status);
                Assert.AreEqual (1,                                              DE_GEF_P0001.StatusSchedule().     Count());


                Assert.AreEqual (1,                                              DE_GEF.ChargingPools.    Count());
                Assert.AreEqual (1,                                              DE_GEF.ChargingPoolIds().Count());

                Assert.IsTrue   (roamingNetwork.ContainsChargingPool(ChargingPool_Id.Parse("DE*GEF*P0001")));
                Assert.IsNotNull(roamingNetwork.GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P0001")));

                Assert.IsTrue   (DE_GEF.        ContainsChargingPool(ChargingPool_Id.Parse("DE*GEF*P0001")));
                Assert.IsNotNull(DE_GEF.        GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P0001")));

            }

        }

        #endregion

        #region ChargingPool_Init_DefaultStatus_Test()

        /// <summary>
        /// A test for creating a charging pool within a charging station operator.
        /// </summary>
        [Test]
        public void ChargingPool_Init_DefaultStatus_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                var DE_GEF_P1234 = DE_GEF.CreateChargingPool(
                                              Id:           ChargingPool_Id.Parse("DE*GEF*P1234"),
                                              Name:         I18NString.Create(Languages.de, "DE*GEF Pool 1234"),
                                              Description:  I18NString.Create(Languages.de, "powered by GraphDefined Charging Pools GmbH")
                                          );

                Assert.IsNotNull(DE_GEF_P1234);

                if (DE_GEF_P1234 is not null)
                {

                    Assert.AreEqual ("DE*GEF*P1234",                                 DE_GEF_P1234.Id.         ToString());
                    Assert.AreEqual ("DE*GEF Pool 1234",                             DE_GEF_P1234.Name.       FirstText());
                    Assert.AreEqual ("powered by GraphDefined Charging Pools GmbH",  DE_GEF_P1234.Description.FirstText());

                    Assert.AreEqual (ChargingPoolAdminStatusTypes.Operational,       DE_GEF_P1234.AdminStatus);
                    Assert.AreEqual (ChargingPoolStatusTypes.Available,              DE_GEF_P1234.Status);

                    Assert.IsTrue   (roamingNetwork.ContainsChargingPool(ChargingPool_Id.Parse("DE*GEF*P1234")));
                    Assert.IsNotNull(roamingNetwork.GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P1234")));

                    Assert.IsTrue   (DE_GEF.        ContainsChargingPool(ChargingPool_Id.Parse("DE*GEF*P1234")));
                    Assert.IsNotNull(DE_GEF.        GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P1234")));

                }

            }

        }

        #endregion


        #region ChargingPool_AdminStatus_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void ChargingPool_AdminStatus_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF_P0001);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_P0001.AdminStatus = ChargingPoolAdminStatusTypes.InternalUse;
                Assert.AreEqual(ChargingPoolAdminStatusTypes.InternalUse,  DE_GEF_P0001.AdminStatus);
                Assert.AreEqual("InternalUse, OutOfService",               DE_GEF_P0001.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                                         DE_GEF_P0001.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF_P0001.AdminStatus = ChargingPoolAdminStatusTypes.Operational;
                Assert.AreEqual(ChargingPoolAdminStatusTypes.Operational,  DE_GEF_P0001.AdminStatus);
                Assert.AreEqual("Operational, InternalUse, OutOfService",  DE_GEF_P0001.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                                         DE_GEF_P0001.AdminStatusSchedule().Count());

            }

        }

        #endregion

        #region ChargingPool_Status_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void ChargingPool_Status_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF_P0001);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_P0001.Status = ChargingPoolStatusTypes.InDeployment;
                Assert.AreEqual(ChargingPoolStatusTypes.InDeployment,  DE_GEF_P0001.Status);
                Assert.AreEqual("InDeployment, Offline",               DE_GEF_P0001.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                                     DE_GEF_P0001.StatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF_P0001.Status = ChargingPoolStatusTypes.Faulted;
                Assert.AreEqual(ChargingPoolStatusTypes.Faulted,       DE_GEF_P0001.Status);
                Assert.AreEqual("Faulted, InDeployment, Offline",      DE_GEF_P0001.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                                     DE_GEF_P0001.StatusSchedule().Count());

            }

        }

        #endregion


    }

}
