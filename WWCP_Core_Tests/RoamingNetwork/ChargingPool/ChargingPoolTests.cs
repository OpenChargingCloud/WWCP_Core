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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.WWCP.POI;

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

            Assert.That(roamingNetwork, Is.Not.Null);
            Assert.That(DE_GEF,         Is.Not.Null);
            Assert.That(DE_GEF_P0001,   Is.Not.Null);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                Assert.That(DE_GEF_P0001.Id.ToString(), Is.EqualTo("DE*GEF*P0001"));
                Assert.That(DE_GEF_P0001.Name.FirstText(), Is.EqualTo("GraphDefined Charging Pool #1"));
                Assert.That(DE_GEF_P0001.Description.FirstText(), Is.EqualTo("powered by GraphDefined Charging Pools GmbH"));

                Assert.That(DE_GEF_P0001.AdminStatus, Is.EqualTo(ChargingPoolAdminStatusType.OutOfService));
                Assert.That(DE_GEF_P0001.AdminStatusSchedule().Count(), Is.EqualTo(1));

                Assert.That(DE_GEF_P0001.Status, Is.EqualTo(ChargingPoolStatusType.Offline));
                Assert.That(DE_GEF_P0001.StatusSchedule().Count(), Is.EqualTo(1));


                Assert.That(roamingNetwork.ChargingPools.Count(), Is.EqualTo(1));
                Assert.That(roamingNetwork.ChargingPoolIds().Count(), Is.EqualTo(1));

                Assert.That(DE_GEF.ChargingPools.Count(), Is.EqualTo(1));
                Assert.That(DE_GEF.ChargingPoolIds().Count(), Is.EqualTo(1));


                Assert.That(roamingNetwork.ContainsChargingPool(ChargingPool_Id.Parse("DE*GEF*P0001")), Is.True);
                Assert.That(roamingNetwork.GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P0001")), Is.Not.Null);

                Assert.That(DE_GEF.        ChargingPoolExists  (ChargingPool_Id.Parse("DE*GEF*P0001")), Is.True);
                Assert.That(DE_GEF.        GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P0001")), Is.Not.Null);

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

            Assert.That(roamingNetwork, Is.Not.Null);
            Assert.That(DE_GEF,         Is.Not.Null);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                var DE_GEF_P1234Result = DE_GEF.AddChargingPool(
                                             Id:           ChargingPool_Id.Parse("DE*GEF*P1234"),
                                             Name:         I18NString.Create(Languages.de, "DE*GEF Pool 1234"),
                                             Description:  I18NString.Create(Languages.de, "powered by GraphDefined Charging Pools GmbH")
                                         ).Result;

                var DE_GEF_P1234 = DE_GEF_P1234Result.ChargingPool;

                Assert.That(DE_GEF_P1234, Is.Not.Null);

                if (DE_GEF_P1234 is not null)
                {

                    Assert.That(DE_GEF_P1234.Id.ToString(), Is.EqualTo("DE*GEF*P1234"));
                    Assert.That(DE_GEF_P1234.Name.FirstText(), Is.EqualTo("DE*GEF Pool 1234"));
                    Assert.That(DE_GEF_P1234.Description.FirstText(), Is.EqualTo("powered by GraphDefined Charging Pools GmbH"));

                    Assert.That(DE_GEF_P1234.AdminStatus, Is.EqualTo(ChargingPoolAdminStatusType.Operational));
                    Assert.That(DE_GEF_P1234.Status, Is.EqualTo(ChargingPoolStatusType.Available));

                    Assert.That(roamingNetwork.ContainsChargingPool(ChargingPool_Id.Parse("DE*GEF*P1234")), Is.True);
                    Assert.That(roamingNetwork.GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P1234")), Is.Not.Null);

                    Assert.That(DE_GEF.        ChargingPoolExists  (ChargingPool_Id.Parse("DE*GEF*P1234")), Is.True);
                    Assert.That(DE_GEF.        GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P1234")), Is.Not.Null);

                }

            }

        }

        #endregion

        #region ChargingPool_Init_AllProperties_Test()

        /// <summary>
        /// A test for creating a charging pool within a charging station having all properties.
        /// </summary>
        [Test]
        public void ChargingPool_Init_AllProperties_Test()
        {

            Assert.That(roamingNetwork, Is.Not.Null);
            Assert.That(DE_GEF,         Is.Not.Null);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                var success = false;

                var DE_GEF_P1234Result = DE_GEF.AddChargingPool(
                                             Id:                  ChargingPool_Id.Parse("DE*GEF*P1234"),
                                             Name:                I18NString.Create(Languages.de, "DE*GEF Pool 1234"),
                                             Description:         I18NString.Create(Languages.de, "powered by GraphDefined Charging Pools GmbH"),
                                             InitialAdminStatus:  ChargingPoolAdminStatusType.OutOfService,
                                             InitialStatus:       ChargingPoolStatusType.Offline,
                                             OnSuccess:           (chargingPool, eventTrackingId) => success = true,
                                             Configurator:        chargingPool => {

                                                                      chargingPool.Brands.Add(new Brand(
                                                                                                  Id:            Brand_Id.Parse("openChargingCloudChargingPool"),
                                                                                                  Name:          I18NString.Create(Languages.de, "Open Charging Cloud Charging Pool"),
                                                                                                  Logo:          URL.Parse("https://open.charging.cloud/logos.json"),
                                                                                                  Homepage:      URL.Parse("https://open.charging.cloud"),
                                                                                                  DataLicenses:  new DataLicense[] {
                                                                                                                     DataLicense.CreativeCommons_BY_SA_4
                                                                                                                 }
                                                                                              ));

                                                                  }
                                         ).Result;

                var DE_GEF_P1234 = DE_GEF_P1234Result.ChargingPool;

                Assert.That(DE_GEF_P1234, Is.Not.Null);
                Assert.That(success,      Is.True);

                if (DE_GEF_P1234 is not null)
                {

                    Assert.That(DE_GEF_P1234.Id.ToString(), Is.EqualTo("DE*GEF*P1234"));
                    Assert.That(DE_GEF_P1234.Name.FirstText(), Is.EqualTo("DE*GEF Pool 1234"));
                    Assert.That(DE_GEF_P1234.Description.FirstText(), Is.EqualTo("powered by GraphDefined Charging Pools GmbH"));

                    Assert.That(DE_GEF_P1234.AdminStatus, Is.EqualTo(ChargingPoolAdminStatusType.OutOfService));
                    Assert.That(DE_GEF_P1234.Status, Is.EqualTo(ChargingPoolStatusType.Offline));

                    Assert.That(roamingNetwork.ContainsChargingPool(ChargingPool_Id.Parse("DE*GEF*P1234")), Is.True);
                    Assert.That(roamingNetwork.GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P1234")), Is.Not.Null);

                    Assert.That(DE_GEF.        ChargingPoolExists  (ChargingPool_Id.Parse("DE*GEF*P1234")), Is.True);
                    Assert.That(DE_GEF.        GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P1234")), Is.Not.Null);


                    Assert.That(DE_GEF_P1234.Brands.Count(), Is.EqualTo(1));



                    DE_GEF_P1234.Brands.Add(new Brand(
                                                Id:            Brand_Id.Parse("openChargingCloud3223"),
                                                Name:          I18NString.Create(Languages.de, "Open Charging Cloud 3223"),
                                                Logo:          URL.Parse("https://open.charging.cloud/logos.json"),
                                                Homepage:      URL.Parse("https://open.charging.cloud"),
                                                DataLicenses:  new DataLicense[] {
                                                                   DataLicense.CreativeCommons_BY_SA_4
                                                               }
                                            ));


                    Assert.That(DE_GEF_P1234.Brands.Count(), Is.EqualTo(2));


                    #region Setup DataChange listeners

                    var chargingPoolDataChanges = new List<String>();

                    DE_GEF_P1234.OnDataChanged += async (Timestamp,
                                                         EventTrackingId,
                                                         ChargingPool,
                                                         PropertyName,
                                                         NewValue,
                                                         OldValue,
                                                         dataSource) => {

                        chargingPoolDataChanges.Add(String.Concat(ChargingPool.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };


                    var chargingStationOperatorChargingPoolDataChanges = new List<String>();

                    DE_GEF.OnChargingPoolDataChanged += async (Timestamp,
                                                               EventTrackingId,
                                                               ChargingPool,
                                                               PropertyName,
                                                               NewValue,
                                                               OldValue,
                                                               dataSource) => {

                        chargingStationOperatorChargingPoolDataChanges.Add(String.Concat(ChargingPool.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };


                    var roamingNetworkChargingPoolDataChanges = new List<String>();

                    roamingNetwork.OnChargingPoolDataChanged += async (Timestamp,
                                                                       EventTrackingId,
                                                                       ChargingPool,
                                                                       PropertyName,
                                                                       NewValue,
                                                                       OldValue,
                                                                       dataSource) => {

                        roamingNetworkChargingPoolDataChanges.Add(String.Concat(ChargingPool.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };

                    #endregion

                    DE_GEF_P1234.Name.       Set(Languages.it, "namelalala");
                    DE_GEF_P1234.Description.Set(Languages.it, "desclalala");

                    Assert.That(chargingPoolDataChanges.Count, Is.EqualTo(2));
                    Assert.That(chargingStationOperatorChargingPoolDataChanges.Count, Is.EqualTo(2));
                    Assert.That(roamingNetworkChargingPoolDataChanges.Count, Is.EqualTo(2));


                    DE_GEF_P1234.MaxPower           = 123.45m;
                    DE_GEF_P1234.MaxPower           = 234.56m;

                    DE_GEF_P1234.MaxPowerRealTime   = 345.67m;
                    DE_GEF_P1234.MaxPowerRealTime   = 456.78m;

                    DE_GEF_P1234.MaxPowerPrognoses.Replace(new[] {
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(1), 567.89m),
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(2), 678.91m),
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(3), 789.12m)
                                                           });

                    Assert.That(chargingPoolDataChanges.Count, Is.EqualTo(7));
                    Assert.That(chargingStationOperatorChargingPoolDataChanges.Count, Is.EqualTo(7));
                    Assert.That(roamingNetworkChargingPoolDataChanges.Count, Is.EqualTo(7));

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

            Assert.That(roamingNetwork, Is.Not.Null);
            Assert.That(DE_GEF,         Is.Not.Null);
            Assert.That(DE_GEF_P0001,   Is.Not.Null);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_P0001.AdminStatus = ChargingPoolAdminStatusType.InternalUse;
                Assert.That(DE_GEF_P0001.AdminStatus, Is.EqualTo(ChargingPoolAdminStatusType.InternalUse));
                Assert.That(DE_GEF_P0001.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("internalUse, outOfService"));
                Assert.That(DE_GEF_P0001.AdminStatusSchedule().Count(), Is.EqualTo(2));

                Thread.Sleep(1000);

                DE_GEF_P0001.AdminStatus = ChargingPoolAdminStatusType.Operational;
                Assert.That(DE_GEF_P0001.AdminStatus, Is.EqualTo(ChargingPoolAdminStatusType.Operational));
                Assert.That(DE_GEF_P0001.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("operational, internalUse, outOfService"));
                Assert.That(DE_GEF_P0001.AdminStatusSchedule().Count(), Is.EqualTo(3));


                Assert.That(DE_GEF_P0001.GenerateAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(new IChargingPool[] { DE_GEF_P0001 }.GenerateAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(DE_GEF.GenerateChargingPoolAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(new IChargingStationOperator[] { DE_GEF }.GenerateChargingPoolAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(roamingNetwork.GenerateChargingPoolAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));


                var jsonStatusReport = DE_GEF_P0001.GenerateAdminStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.That(jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None),
                                Is.EqualTo("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingPoolAdminStatusReport\",\"count\":1,\"report\":{\"operational\":{\"count\":1,\"percentage\":100.0}}}"));

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

            Assert.That(roamingNetwork, Is.Not.Null);
            Assert.That(DE_GEF,         Is.Not.Null);
            Assert.That(DE_GEF_P0001,   Is.Not.Null);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_P0001.Status = ChargingPoolStatusType.InDeployment;
                Assert.That(DE_GEF_P0001.Status, Is.EqualTo(ChargingPoolStatusType.InDeployment));
                Assert.That(DE_GEF_P0001.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("inDeployment, offline"));
                Assert.That(DE_GEF_P0001.StatusSchedule().Count(), Is.EqualTo(2));

                Thread.Sleep(1000);

                DE_GEF_P0001.Status = ChargingPoolStatusType.Error;
                Assert.That(DE_GEF_P0001.Status, Is.EqualTo(ChargingPoolStatusType.Error));
                Assert.That(DE_GEF_P0001.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("error, inDeployment, offline"));
                Assert.That(DE_GEF_P0001.StatusSchedule().Count(), Is.EqualTo(3));


                Assert.That(DE_GEF_P0001.GenerateStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(new IChargingPool[] { DE_GEF_P0001 }.GenerateStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(DE_GEF.GenerateChargingPoolStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(new IChargingStationOperator[] { DE_GEF }.GenerateChargingPoolStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(roamingNetwork.GenerateChargingPoolStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));


                var jsonStatusReport = DE_GEF_P0001.GenerateStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.That(jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None),
                                Is.EqualTo("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingPoolStatusReport\",\"count\":1,\"report\":{\"error\":{\"count\":1,\"percentage\":100.0}}}"));

            }

        }

        #endregion


    }

}
