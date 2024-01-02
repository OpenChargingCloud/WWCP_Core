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

                ClassicAssert.AreEqual ("PROD",                                       roamingNetwork.Id.         ToString());
                ClassicAssert.AreEqual ("PRODUCTION",                                 roamingNetwork.Name.       FirstText());
                ClassicAssert.AreEqual ("The main production roaming network",        roamingNetwork.Description.FirstText());

                ClassicAssert.AreEqual (RoamingNetworkAdminStatusTypes.OutOfService,  roamingNetwork.AdminStatus);
                ClassicAssert.AreEqual (1,                                            roamingNetwork.AdminStatusSchedule().Count());

                ClassicAssert.AreEqual (RoamingNetworkStatusTypes.Offline,            roamingNetwork.Status);
                ClassicAssert.AreEqual (1,                                            roamingNetwork.StatusSchedule().     Count());


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
                                             Name:                I18NString.Create(Languages.en, "TESTNET"),
                                             Description:         I18NString.Create(Languages.en, "A roaming network for testing"),
                                             DisableNetworkSync:  true
                                         );

                ClassicAssert.IsNotNull(roamingNetwork);

                if (roamingNetwork is not null)
                {

                    ClassicAssert.AreEqual ("TEST",                                      roamingNetwork.Id.         ToString());
                    ClassicAssert.AreEqual ("TESTNET",                                   roamingNetwork.Name.       FirstText());
                    ClassicAssert.AreEqual ("A roaming network for testing",             roamingNetwork.Description.FirstText());

                    ClassicAssert.AreEqual (RoamingNetworkAdminStatusTypes.Operational,  roamingNetwork.AdminStatus);
                    ClassicAssert.AreEqual (RoamingNetworkStatusTypes.Available,         roamingNetwork.Status);

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

                roamingNetwork.AdminStatus = RoamingNetworkAdminStatusTypes.InternalUse;
                ClassicAssert.AreEqual(RoamingNetworkAdminStatusTypes.InternalUse,  roamingNetwork.AdminStatus);
                ClassicAssert.AreEqual("internalUse, outOfService",                 roamingNetwork.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(2,                                           roamingNetwork.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                roamingNetwork.AdminStatus = RoamingNetworkAdminStatusTypes.Operational;
                ClassicAssert.AreEqual(RoamingNetworkAdminStatusTypes.Operational,  roamingNetwork.AdminStatus);
                ClassicAssert.AreEqual("operational, internalUse, outOfService",    roamingNetwork.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(3,                                           roamingNetwork.AdminStatusSchedule().Count());


                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", roamingNetwork.GenerateAdminStatusReport().ToString());


                var jsonStatusReport = roamingNetwork.GenerateAdminStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                ClassicAssert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/roamingNetworkAdminStatusReport\",\"count\":1,\"report\":{\"operational\":{\"count\":1,\"percentage\":100.0}}}",
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

            ClassicAssert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                // Status entries are compared by their ISO 8601 timestamp!
                Thread.Sleep(1000);

                roamingNetwork.Status = RoamingNetworkStatusTypes.Error;
                ClassicAssert.AreEqual(RoamingNetworkStatusTypes.Error,      roamingNetwork.Status);
                ClassicAssert.AreEqual("error, offline",                     roamingNetwork.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(2,                                    roamingNetwork.StatusSchedule().Count());

                Thread.Sleep(1000);

                roamingNetwork.Status = RoamingNetworkStatusTypes.Available;
                ClassicAssert.AreEqual(RoamingNetworkStatusTypes.Available,  roamingNetwork.Status);
                ClassicAssert.AreEqual("available, error, offline",          roamingNetwork.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(3,                                    roamingNetwork.StatusSchedule().Count());


                ClassicAssert.AreEqual("1 entities; available: 1 (100,00)", roamingNetwork.GenerateStatusReport().ToString());


                var jsonStatusReport = roamingNetwork.GenerateStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                ClassicAssert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/roamingNetworkStatusReport\",\"count\":1,\"report\":{\"available\":{\"count\":1,\"percentage\":100.0}}}",
                                jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None));

            }

        }

        #endregion


    }

}
