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
    /// Unit tests for EVSEs.
    /// </summary>
    [TestFixture]
    public class EVSETests : AEVSETests
    {

        #region EVSE_Init_Test()

        /// <summary>
        /// A test for creating an EVSE within a charging station.
        /// </summary>
        [Test]
        public void EVSE_Init_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF_P0001);
            Assert.IsNotNull(DE_GEF_S0001_AAAA);
            Assert.IsNotNull(DE_GEF_E0001_AAAA_1);

            if (roamingNetwork      is not null &&
                DE_GEF              is not null &&
                DE_GEF_P0001        is not null &&
                DE_GEF_S0001_AAAA   is not null &&
                DE_GEF_E0001_AAAA_1 is not null)
            {

                Assert.AreEqual ("DE*GEF*E0001*AAAA*1",                 DE_GEF_E0001_AAAA_1.Id.         ToString());
                Assert.AreEqual ("GraphDefined EVSE #1",                DE_GEF_E0001_AAAA_1.Name.       FirstText());
                Assert.AreEqual ("powered by GraphDefined EVSEs GmbH",  DE_GEF_E0001_AAAA_1.Description.FirstText());

                Assert.AreEqual (EVSEAdminStatusTypes.OutOfService,     DE_GEF_E0001_AAAA_1.AdminStatus);
                Assert.AreEqual (1,                                     DE_GEF_E0001_AAAA_1.AdminStatusSchedule().Count());

                Assert.AreEqual (EVSEStatusTypes.Offline,               DE_GEF_E0001_AAAA_1.Status);
                Assert.AreEqual (1,                                     DE_GEF_E0001_AAAA_1.StatusSchedule().     Count());


                Assert.AreEqual (1,                                     roamingNetwork.   EVSEs.    Count());
                Assert.AreEqual (1,                                     roamingNetwork.   EVSEIds().Count());

                Assert.AreEqual (1,                                     DE_GEF.           EVSEs.    Count());
                Assert.AreEqual (1,                                     DE_GEF.           EVSEIds().Count());

                Assert.AreEqual (1,                                     DE_GEF_P0001.     EVSEs.    Count());
                Assert.AreEqual (1,                                     DE_GEF_P0001.     EVSEIds().Count());

                Assert.AreEqual (1,                                     DE_GEF_S0001_AAAA.EVSEs.    Count());
                Assert.AreEqual (1,                                     DE_GEF_S0001_AAAA.EVSEIds().Count());


                Assert.IsTrue   (roamingNetwork.   ContainsEVSE(EVSE_Id.Parse("DE*GEF*E0001*AAAA*1")));
                Assert.IsNotNull(roamingNetwork.   GetEVSEById (EVSE_Id.Parse("DE*GEF*E0001*AAAA*1")));

                Assert.IsTrue   (DE_GEF.           ContainsEVSE(EVSE_Id.Parse("DE*GEF*E0001*AAAA*1")));
                Assert.IsNotNull(DE_GEF.           GetEVSEById (EVSE_Id.Parse("DE*GEF*E0001*AAAA*1")));

                Assert.IsTrue   (DE_GEF_P0001.     ContainsEVSE(EVSE_Id.Parse("DE*GEF*E0001*AAAA*1")));
                Assert.IsNotNull(DE_GEF_P0001.     GetEVSEById (EVSE_Id.Parse("DE*GEF*E0001*AAAA*1")));

                Assert.IsTrue   (DE_GEF_S0001_AAAA.ContainsEVSE(EVSE_Id.Parse("DE*GEF*E0001*AAAA*1")));
                Assert.IsNotNull(DE_GEF_S0001_AAAA.GetEVSEById (EVSE_Id.Parse("DE*GEF*E0001*AAAA*1")));

            }

        }

        #endregion

        #region EVSE_Init_DefaultStatus_Test()

        /// <summary>
        /// A test for creating an EVSE within a charging station.
        /// </summary>
        [Test]
        public void EVSE_Init_DefaultStatus_Test()
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

                var DE_GEF_E0001_AAAA_X = DE_GEF_S0001_AAAA.CreateEVSE(
                                                                Id:           EVSE_Id.Parse("DE*GEF*E1234"),
                                                                Name:         I18NString.Create(Languages.de, "DE*GEF EVSE 1234"),
                                                                Description:  I18NString.Create(Languages.de, "powered by GraphDefined EVSEs GmbH")
                                                            );

                Assert.IsNotNull(DE_GEF_E0001_AAAA_X);

                if (DE_GEF_E0001_AAAA_X is not null)
                {

                    Assert.AreEqual ("DE*GEF*E1234",                        DE_GEF_E0001_AAAA_X.Id.         ToString());
                    Assert.AreEqual ("DE*GEF EVSE 1234",                    DE_GEF_E0001_AAAA_X.Name.       FirstText());
                    Assert.AreEqual ("powered by GraphDefined EVSEs GmbH",  DE_GEF_E0001_AAAA_X.Description.FirstText());

                    Assert.AreEqual (EVSEAdminStatusTypes.Operational,      DE_GEF_E0001_AAAA_X.AdminStatus);
                    Assert.AreEqual (EVSEStatusTypes.Available,             DE_GEF_E0001_AAAA_X.Status);

                    Assert.IsTrue   (roamingNetwork.   ContainsEVSE(EVSE_Id.Parse("DE*GEF*E1234")));
                    Assert.IsNotNull(roamingNetwork.   GetEVSEById (EVSE_Id.Parse("DE*GEF*E1234")));

                    Assert.IsTrue   (DE_GEF.           ContainsEVSE(EVSE_Id.Parse("DE*GEF*E1234")));
                    Assert.IsNotNull(DE_GEF.           GetEVSEById (EVSE_Id.Parse("DE*GEF*E1234")));

                    Assert.IsTrue   (DE_GEF_P0001.     ContainsEVSE(EVSE_Id.Parse("DE*GEF*E1234")));
                    Assert.IsNotNull(DE_GEF_P0001.     GetEVSEById (EVSE_Id.Parse("DE*GEF*E1234")));

                    Assert.IsTrue   (DE_GEF_S0001_AAAA.ContainsEVSE(EVSE_Id.Parse("DE*GEF*E1234")));
                    Assert.IsNotNull(DE_GEF_S0001_AAAA.GetEVSEById (EVSE_Id.Parse("DE*GEF*E1234")));

                }

            }

        }

        #endregion


        #region EVSE_AdminStatus_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void EVSE_AdminStatus_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF_P0001);
            Assert.IsNotNull(DE_GEF_S0001_AAAA);
            Assert.IsNotNull(DE_GEF_E0001_AAAA_1);

            if (roamingNetwork      is not null &&
                DE_GEF              is not null &&
                DE_GEF_P0001        is not null &&
                DE_GEF_S0001_AAAA   is not null &&
                DE_GEF_E0001_AAAA_1 is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_E0001_AAAA_1.AdminStatus = EVSEAdminStatusTypes.InternalUse;
                Assert.AreEqual(EVSEAdminStatusTypes.InternalUse,          DE_GEF_E0001_AAAA_1.AdminStatus);
                Assert.AreEqual("InternalUse, OutOfService",               DE_GEF_E0001_AAAA_1.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                                         DE_GEF_E0001_AAAA_1.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF_E0001_AAAA_1.AdminStatus = EVSEAdminStatusTypes.Operational;
                Assert.AreEqual(EVSEAdminStatusTypes.Operational,          DE_GEF_E0001_AAAA_1.AdminStatus);
                Assert.AreEqual("Operational, InternalUse, OutOfService",  DE_GEF_E0001_AAAA_1.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                                         DE_GEF_E0001_AAAA_1.AdminStatusSchedule().Count());

            }

        }

        #endregion

        #region EVSE_Status_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void EVSE_Status_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF_P0001);
            Assert.IsNotNull(DE_GEF_S0001_AAAA);
            Assert.IsNotNull(DE_GEF_E0001_AAAA_1);

            if (roamingNetwork      is not null &&
                DE_GEF              is not null &&
                DE_GEF_P0001        is not null &&
                DE_GEF_S0001_AAAA   is not null &&
                DE_GEF_E0001_AAAA_1 is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_E0001_AAAA_1.Status = EVSEStatusTypes.Reserved;
                Assert.AreEqual(EVSEStatusTypes.Reserved,      DE_GEF_E0001_AAAA_1.Status);
                Assert.AreEqual("Reserved, Offline",           DE_GEF_E0001_AAAA_1.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                             DE_GEF_E0001_AAAA_1.StatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF_E0001_AAAA_1.Status = EVSEStatusTypes.Faulted;
                Assert.AreEqual(EVSEStatusTypes.Faulted,       DE_GEF_E0001_AAAA_1.Status);
                Assert.AreEqual("Faulted, Reserved, Offline",  DE_GEF_E0001_AAAA_1.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                             DE_GEF_E0001_AAAA_1.StatusSchedule().Count());

            }

        }

        #endregion


    }

}
