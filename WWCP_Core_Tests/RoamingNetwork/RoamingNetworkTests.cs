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
using NUnit.Framework.Legacy;

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

            ClassicAssert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                Assert.That(roamingNetwork.Id.ToString(), Is.EqualTo("PROD"));
                Assert.That(roamingNetwork.Name.FirstText(), Is.EqualTo("PRODUCTION"));
                Assert.That(roamingNetwork.Description.FirstText(), Is.EqualTo("The main production roaming network"));

                Assert.That(roamingNetwork.AdminStatus, Is.EqualTo(RoamingNetworkAdminStatusType.OutOfService));
                Assert.That(roamingNetwork.AdminStatusSchedule().Count(), Is.EqualTo(1));

                Assert.That(roamingNetwork.Status, Is.EqualTo(RoamingNetworkStatusType.Offline));
                Assert.That(roamingNetwork.StatusSchedule().Count(), Is.EqualTo(1));


                ClassicAssert.IsTrue   (roamingNetwork.DisableNetworkSync);

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

            ClassicAssert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                var roamingNetwork = new WWCP.RoamingNetwork(
                                             Id:                  RoamingNetwork_Id.Parse("TEST"),
                                             Name:                I18NString.Create("TESTNET"),
                                             Description:         I18NString.Create("A roaming network for testing"),
                                             DisableNetworkSync:  true
                                         );

                ClassicAssert.IsNotNull(roamingNetwork);

                if (roamingNetwork is not null)
                {

                    Assert.That(roamingNetwork.Id.ToString(), Is.EqualTo("TEST"));
                    Assert.That(roamingNetwork.Name.FirstText(), Is.EqualTo("TESTNET"));
                    Assert.That(roamingNetwork.Description.FirstText(), Is.EqualTo("A roaming network for testing"));

                    Assert.That(roamingNetwork.AdminStatus, Is.EqualTo(RoamingNetworkAdminStatusType.Operational));
                    Assert.That(roamingNetwork.Status, Is.EqualTo(RoamingNetworkStatusType.Available));

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

            ClassicAssert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                roamingNetwork.AdminStatus = RoamingNetworkAdminStatusType.InternalUse;
                Assert.That(roamingNetwork.AdminStatus, Is.EqualTo(RoamingNetworkAdminStatusType.InternalUse));
                Assert.That(roamingNetwork.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("internalUse, outOfService"));
                Assert.That(roamingNetwork.AdminStatusSchedule().Count(), Is.EqualTo(2));

                Thread.Sleep(1000);

                roamingNetwork.AdminStatus = RoamingNetworkAdminStatusType.Operational;
                Assert.That(roamingNetwork.AdminStatus, Is.EqualTo(RoamingNetworkAdminStatusType.Operational));
                Assert.That(roamingNetwork.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("operational, internalUse, outOfService"));
                Assert.That(roamingNetwork.AdminStatusSchedule().Count(), Is.EqualTo(3));


                Assert.That(roamingNetwork.GenerateAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));


                var jsonStatusReport = roamingNetwork.GenerateAdminStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.That(jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None),
                                Is.EqualTo("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/roamingNetworkAdminStatusReport\",\"count\":1,\"report\":{\"operational\":{\"count\":1,\"percentage\":100.0}}}"));

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

            ClassicAssert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                // Status entries are compared by their ISO 8601 timestamp!
                Thread.Sleep(1000);

                roamingNetwork.Status = RoamingNetworkStatusType.Error;
                Assert.That(roamingNetwork.Status, Is.EqualTo(RoamingNetworkStatusType.Error));
                Assert.That(roamingNetwork.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("error, offline"));
                Assert.That(roamingNetwork.StatusSchedule().Count(), Is.EqualTo(2));

                Thread.Sleep(1000);

                roamingNetwork.Status = RoamingNetworkStatusType.Available;
                Assert.That(roamingNetwork.Status, Is.EqualTo(RoamingNetworkStatusType.Available));
                Assert.That(roamingNetwork.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("available, error, offline"));
                Assert.That(roamingNetwork.StatusSchedule().Count(), Is.EqualTo(3));


                Assert.That(roamingNetwork.GenerateStatusReport().ToString(), Is.EqualTo("1 entities; available: 1 (100.00)"));


                var jsonStatusReport = roamingNetwork.GenerateStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.That(jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None),
                                Is.EqualTo("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/roamingNetworkStatusReport\",\"count\":1,\"report\":{\"available\":{\"count\":1,\"percentage\":100.0}}}"));

            }

        }

        #endregion


    }

}
