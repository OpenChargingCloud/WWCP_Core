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
    /// Unit tests for roaming networks.
    /// </summary>
    [TestFixture]
    public class RoamingNetworkTests : ARoamingNetworkTests
    {

        #region RoamingNetwork_Init_Test1()

        /// <summary>
        /// A test for the roaming network constructor.
        /// </summary>
        [Test]
        public void RoamingNetwork_Init_Test1()
        {

            Assert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                Assert.AreEqual ("PROD",                                      roamingNetwork.Id.         ToString());
                Assert.AreEqual ("PRODUCTION",                                roamingNetwork.Name.       FirstText());
                Assert.AreEqual ("The main production roaming network",       roamingNetwork.Description.FirstText());

                Assert.AreEqual (RoamingNetworkAdminStatusTypes.Operational,  roamingNetwork.AdminStatus);
                Assert.AreEqual (1,                                           roamingNetwork.AdminStatusSchedule().Count());

                Assert.AreEqual (RoamingNetworkStatusTypes.Available,         roamingNetwork.Status);
                Assert.AreEqual (1,                                           roamingNetwork.StatusSchedule().     Count());


                Assert.IsTrue   (roamingNetwork.DisableNetworkSync);

            }

        }

        #endregion

        #region RoamingNetwork_AdminStatus_Test1()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void RoamingNetwork_AdminStatus_Test1()
        {

            Assert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                // Status entries are compared by their ISO 8601 timestamp!
                Thread.Sleep(1000);

                roamingNetwork.AdminStatus = RoamingNetworkAdminStatusTypes.OutOfService;
                Assert.AreEqual(RoamingNetworkAdminStatusTypes.OutOfService, roamingNetwork.AdminStatus);
                Assert.AreEqual(2,                                           roamingNetwork.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                roamingNetwork.AdminStatus = RoamingNetworkAdminStatusTypes.Operational;
                Assert.AreEqual(RoamingNetworkAdminStatusTypes.Operational,  roamingNetwork.AdminStatus);
                Assert.AreEqual(3,                                           roamingNetwork.AdminStatusSchedule().Count());

            }

        }

        #endregion

        #region RoamingNetwork_Status_Test1()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void RoamingNetwork_Status_Test1()
        {

            Assert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                // Status entries are compared by their ISO 8601 timestamp!
                Thread.Sleep(1000);

                roamingNetwork.Status = RoamingNetworkStatusTypes.OutOfService;
                Assert.AreEqual(RoamingNetworkStatusTypes.OutOfService, roamingNetwork.Status);
                Assert.AreEqual(2,                                      roamingNetwork.StatusSchedule().Count());

                Thread.Sleep(1000);

                roamingNetwork.Status = RoamingNetworkStatusTypes.Faulted;
                Assert.AreEqual(RoamingNetworkStatusTypes.Faulted,      roamingNetwork.Status);
                Assert.AreEqual(3,                                      roamingNetwork.StatusSchedule().Count());

            }

        }

        #endregion


    }

}
