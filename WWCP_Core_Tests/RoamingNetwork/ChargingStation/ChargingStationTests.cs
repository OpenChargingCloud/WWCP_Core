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
    /// Unit tests for charging stations.
    /// </summary>
    [TestFixture]
    public class ChargingStationTests : AChargingStationTests
    {

        #region ChargingStation_Init_Test()

        /// <summary>
        /// A test for creating a charging pool within a charging station operator.
        /// </summary>
        [Test]
        public void ChargingStation_Init_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF_P0001);
            Assert.IsNotNull(DE_GEF_S0001_AAAA);

            if (roamingNetwork    is not null &&
                DE_GEF            is not null &&
                DE_GEF_P0001      is not null &&
                DE_GEF_S0001_AAAA is not null)
            {

                Assert.AreEqual ("DE*GEF*S0001*AAAA",                               DE_GEF_S0001_AAAA.Id.         ToString());
                Assert.AreEqual ("GraphDefined Charging Station #AAAA",             DE_GEF_S0001_AAAA.Name.       FirstText());
                Assert.AreEqual ("powered by GraphDefined Charging Stations GmbH",  DE_GEF_S0001_AAAA.Description.FirstText());

                Assert.AreEqual (ChargingStationAdminStatusTypes.OutOfService,      DE_GEF_S0001_AAAA.AdminStatus);
                Assert.AreEqual (1,                                              DE_GEF_S0001_AAAA.AdminStatusSchedule().Count());

                Assert.AreEqual (ChargingStationStatusTypes.Offline,                DE_GEF_S0001_AAAA.Status);
                Assert.AreEqual (1,                                              DE_GEF_S0001_AAAA.StatusSchedule().     Count());


                Assert.AreEqual (1,                                              DE_GEF.ChargingStations.    Count());
                Assert.AreEqual (1,                                              DE_GEF.ChargingStationIds().Count());

                Assert.IsTrue   (roamingNetwork.ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));
                Assert.IsNotNull(roamingNetwork.GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));

                Assert.IsTrue   (DE_GEF.        ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));
                Assert.IsNotNull(DE_GEF.        GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));

                Assert.IsTrue   (DE_GEF_P0001.  ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));
                Assert.IsNotNull(DE_GEF_P0001.  GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));

            }

        }

        #endregion

        #region ChargingStation_Init_DefaultStatus_Test()

        /// <summary>
        /// A test for creating a charging pool within a charging station operator.
        /// </summary>
        [Test]
        public void ChargingStation_Init_DefaultStatus_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF_P0001);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                var DE_GEF_S1234 = DE_GEF_P0001.CreateChargingStation(
                                                    Id:                  ChargingStation_Id.Parse("DE*GEF*S1234"),
                                                    Name:                I18NString.Create(Languages.de, "DE*GEF Station 1234"),
                                                    Description:         I18NString.Create(Languages.de, "powered by GraphDefined Charging Stations GmbH")
                                                );

                Assert.IsNotNull(DE_GEF_S1234);

                if (DE_GEF_S1234 is not null)
                {

                    Assert.AreEqual ("DE*GEF*S1234",                                    DE_GEF_S1234.Id.         ToString());
                    Assert.AreEqual ("DE*GEF Station 1234",                             DE_GEF_S1234.Name.       FirstText());
                    Assert.AreEqual ("powered by GraphDefined Charging Stations GmbH",  DE_GEF_S1234.Description.FirstText());

                    Assert.AreEqual (ChargingStationAdminStatusTypes.Operational,       DE_GEF_S1234.AdminStatus);
                    Assert.AreEqual (ChargingStationStatusTypes.Available,              DE_GEF_S1234.Status);

                    Assert.IsTrue   (roamingNetwork.ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    Assert.IsNotNull(roamingNetwork.GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));

                    Assert.IsTrue   (DE_GEF.        ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    Assert.IsNotNull(DE_GEF.        GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));

                    Assert.IsTrue   (DE_GEF_P0001.  ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    Assert.IsNotNull(DE_GEF_P0001.  GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));

                }

            }

        }

        #endregion


        #region ChargingStation_AdminStatus_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void ChargingStation_AdminStatus_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF);

            if (roamingNetwork    is not null &&
                DE_GEF            is not null &&
                DE_GEF_P0001      is not null &&
                DE_GEF_S0001_AAAA is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.AdminStatus = ChargingStationAdminStatusTypes.InternalUse;
                Assert.AreEqual(ChargingStationAdminStatusTypes.InternalUse,  DE_GEF_S0001_AAAA.AdminStatus);
                Assert.AreEqual("InternalUse, OutOfService",                  DE_GEF_S0001_AAAA.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                                            DE_GEF_S0001_AAAA.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.AdminStatus = ChargingStationAdminStatusTypes.Operational;
                Assert.AreEqual(ChargingStationAdminStatusTypes.Operational,  DE_GEF_S0001_AAAA.AdminStatus);
                Assert.AreEqual("Operational, InternalUse, OutOfService",     DE_GEF_S0001_AAAA.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                                            DE_GEF_S0001_AAAA.AdminStatusSchedule().Count());

            }

        }

        #endregion

        #region ChargingStation_Status_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void ChargingStation_Status_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF);

            if (roamingNetwork    is not null &&
                DE_GEF            is not null &&
                DE_GEF_P0001      is not null &&
                DE_GEF_S0001_AAAA is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.Status = ChargingStationStatusTypes.InDeployment;
                Assert.AreEqual(ChargingStationStatusTypes.InDeployment, DE_GEF_S0001_AAAA.Status);
                Assert.AreEqual("InDeployment, Offline",                         DE_GEF_S0001_AAAA.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                                               DE_GEF_S0001_AAAA.StatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.Status = ChargingStationStatusTypes.Faulted;
                Assert.AreEqual(ChargingStationStatusTypes.Faulted,      DE_GEF_S0001_AAAA.Status);
                Assert.AreEqual("Faulted, InDeployment, Offline",                DE_GEF_S0001_AAAA.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                                               DE_GEF_S0001_AAAA.StatusSchedule().Count());

            }

        }

        #endregion


    }

}
