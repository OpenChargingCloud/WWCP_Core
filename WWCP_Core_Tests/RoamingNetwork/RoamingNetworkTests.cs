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
    /// Unit tests for roaming networks.
    /// </summary>
    [TestFixture]
    public class RoamingNetworkTests : ARoamingNetworkTests
    {

        #region RoamingNetwork_Init_Test()

        /// <summary>
        /// A test for the roaming network constructor.
        /// </summary>
        [Test]
        public void RoamingNetwork_Init_Test()
        {

            Assert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                Assert.AreEqual ("PROD",                                       roamingNetwork.Id.         ToString());
                Assert.AreEqual ("PRODUCTION",                                 roamingNetwork.Name.       FirstText());
                Assert.AreEqual ("The main production roaming network",        roamingNetwork.Description.FirstText());

                Assert.AreEqual (RoamingNetworkAdminStatusTypes.OutOfService,  roamingNetwork.AdminStatus);
                Assert.AreEqual (1,                                            roamingNetwork.AdminStatusSchedule().Count());

                Assert.AreEqual (RoamingNetworkStatusTypes.Offline,            roamingNetwork.Status);
                Assert.AreEqual (1,                                            roamingNetwork.StatusSchedule().     Count());


                Assert.IsTrue   (roamingNetwork.DisableNetworkSync);

            }

        }

        #endregion

        #region RoamingNetwork_Init_DefaultStatus_Test()

        /// <summary>
        /// A test for the roaming network constructor.
        /// </summary>
        [Test]
        public void RoamingNetwork_Init_DefaultStatus_Test()
        {

            Assert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                var rn = new WWCP.RoamingNetwork(
                             Id:                  RoamingNetwork_Id.Parse("TEST"),
                             Name:                I18NString.Create(Languages.en, "TESTNET"),
                             Description:         I18NString.Create(Languages.en, "A roaming network for testing"),
                             DisableNetworkSync:  true
                         );

                Assert.IsNotNull(rn);

                if (rn is not null)
                {

                    Assert.AreEqual ("TEST",                                      rn.Id.         ToString());
                    Assert.AreEqual ("TESTNET",                                   rn.Name.       FirstText());
                    Assert.AreEqual ("A roaming network for testing",             rn.Description.FirstText());

                    Assert.AreEqual (RoamingNetworkAdminStatusTypes.Operational,  rn.AdminStatus);
                    Assert.AreEqual (RoamingNetworkStatusTypes.Available,         rn.Status);

                }

            }

        }

        #endregion

        #region RoamingNetwork_AdminStatus_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void RoamingNetwork_AdminStatus_Test()
        {

            Assert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                roamingNetwork.AdminStatus = RoamingNetworkAdminStatusTypes.InternalUse;
                Assert.AreEqual(RoamingNetworkAdminStatusTypes.InternalUse,  roamingNetwork.AdminStatus);
                Assert.AreEqual("InternalUse, OutOfService",                 roamingNetwork.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                                           roamingNetwork.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                roamingNetwork.AdminStatus = RoamingNetworkAdminStatusTypes.Operational;
                Assert.AreEqual(RoamingNetworkAdminStatusTypes.Operational,  roamingNetwork.AdminStatus);
                Assert.AreEqual("Operational, InternalUse, OutOfService",    roamingNetwork.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                                           roamingNetwork.AdminStatusSchedule().Count());


                Assert.AreEqual("1 entities; Operational: 1 (100,00)", roamingNetwork.GenerateAdminStatusReport().ToString());


                var jsonStatusReport = roamingNetwork.GenerateAdminStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/roamingNetworkAdminStatusReport\",\"count\":1,\"report\":{\"Operational\":{\"count\":1,\"percentage\":100.0}}}",
                                jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None));

            }

        }

        #endregion

        #region RoamingNetwork_Status_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void RoamingNetwork_Status_Test()
        {

            Assert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                // Status entries are compared by their ISO 8601 timestamp!
                Thread.Sleep(1000);

                roamingNetwork.Status = RoamingNetworkStatusTypes.Faulted;
                Assert.AreEqual(RoamingNetworkStatusTypes.Faulted,    roamingNetwork.Status);
                Assert.AreEqual("Faulted, Offline",                   roamingNetwork.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                                    roamingNetwork.StatusSchedule().Count());

                Thread.Sleep(1000);

                roamingNetwork.Status = RoamingNetworkStatusTypes.Available;
                Assert.AreEqual(RoamingNetworkStatusTypes.Available,  roamingNetwork.Status);
                Assert.AreEqual("Available, Faulted, Offline",        roamingNetwork.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                                    roamingNetwork.StatusSchedule().Count());


                Assert.AreEqual("1 entities; Available: 1 (100,00)", roamingNetwork.GenerateStatusReport().ToString());


                var jsonStatusReport = roamingNetwork.GenerateStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/roamingNetworkStatusReport\",\"count\":1,\"report\":{\"Available\":{\"count\":1,\"percentage\":100.0}}}",
                                jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None));

            }

        }

        #endregion


    }

}
