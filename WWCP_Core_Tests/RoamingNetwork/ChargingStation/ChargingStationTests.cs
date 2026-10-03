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
        /// A test for creating a charging station within a charging pool.
        /// </summary>
        [Test]
        public void ChargingStation_Init_Test()
        {

            Assert.That(roamingNetwork,    Is.Not.Null);
            Assert.That(DE_GEF,            Is.Not.Null);
            Assert.That(DE_GEF_P0001,      Is.Not.Null);
            Assert.That(DE_GEF_S0001_AAAA, Is.Not.Null);

            if (roamingNetwork    is not null &&
                DE_GEF            is not null &&
                DE_GEF_P0001      is not null &&
                DE_GEF_S0001_AAAA is not null)
            {

                Assert.That(DE_GEF_S0001_AAAA.Id.ToString(), Is.EqualTo("DE*GEF*S0001*AAAA"));
                Assert.That(DE_GEF_S0001_AAAA.Name.FirstText(), Is.EqualTo("GraphDefined Charging Station #AAAA"));
                Assert.That(DE_GEF_S0001_AAAA.Description.FirstText(), Is.EqualTo("powered by GraphDefined Charging Stations GmbH"));

                Assert.That(DE_GEF_S0001_AAAA.AdminStatus, Is.EqualTo(ChargingStationAdminStatusType.OutOfService));
                Assert.That(DE_GEF_S0001_AAAA.AdminStatusSchedule().Count(), Is.EqualTo(1));

                Assert.That(DE_GEF_S0001_AAAA.Status, Is.EqualTo(ChargingStationStatusType.Offline));
                Assert.That(DE_GEF_S0001_AAAA.StatusSchedule().Count(), Is.EqualTo(1));


                Assert.That(roamingNetwork.ChargingStations.Count(), Is.EqualTo(1));
                Assert.That(roamingNetwork.ChargingStationIds().Count(), Is.EqualTo(1));

                Assert.That(DE_GEF.ChargingStations.Count(), Is.EqualTo(1));
                Assert.That(DE_GEF.ChargingStationIds().Count(), Is.EqualTo(1));

                Assert.That(DE_GEF_P0001.ChargingStations.Count(), Is.EqualTo(1));
                Assert.That(DE_GEF_P0001.ChargingStationIds().Count(), Is.EqualTo(1));


                Assert.That(roamingNetwork.ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")), Is.True);
                Assert.That(roamingNetwork.GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")), Is.Not.Null);

                Assert.That(DE_GEF.        ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")), Is.True);
                Assert.That(DE_GEF.        GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")), Is.Not.Null);

                Assert.That(DE_GEF_P0001.  ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")), Is.True);
                Assert.That(DE_GEF_P0001.  GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")), Is.Not.Null);

            }

        }

        #endregion

        #region ChargingStation_Init_DefaultStatus_Test()

        /// <summary>
        /// A test for creating a charging station within a charging pool.
        /// </summary>
        [Test]
        public void ChargingStation_Init_DefaultStatus_Test()
        {

            Assert.That(roamingNetwork, Is.Not.Null);
            Assert.That(DE_GEF,         Is.Not.Null);
            Assert.That(DE_GEF_P0001,   Is.Not.Null);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                var DE_GEF_S1234 = DE_GEF_P0001.AddChargingStation(
                                                    Id:           ChargingStation_Id.Parse("DE*GEF*S1234"),
                                                    Name:         I18NString.Create(Languages.de, "DE*GEF Station 1234"),
                                                    Description:  I18NString.Create(Languages.de, "powered by GraphDefined Charging Stations GmbH")
                                                ).Result.ChargingStation;

                Assert.That(DE_GEF_S1234, Is.Not.Null);

                if (DE_GEF_S1234 is not null)
                {

                    Assert.That(DE_GEF_S1234.Id.ToString(), Is.EqualTo("DE*GEF*S1234"));
                    Assert.That(DE_GEF_S1234.Name.FirstText(), Is.EqualTo("DE*GEF Station 1234"));
                    Assert.That(DE_GEF_S1234.Description.FirstText(), Is.EqualTo("powered by GraphDefined Charging Stations GmbH"));

                    Assert.That(DE_GEF_S1234.AdminStatus, Is.EqualTo(ChargingStationAdminStatusType.Operational));
                    Assert.That(DE_GEF_S1234.Status, Is.EqualTo(ChargingStationStatusType.Available));

                    Assert.That(roamingNetwork.ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")), Is.True);
                    Assert.That(roamingNetwork.GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")), Is.Not.Null);

                    Assert.That(DE_GEF.        ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")), Is.True);
                    Assert.That(DE_GEF.        GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")), Is.Not.Null);

                    Assert.That(DE_GEF_P0001.  ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")), Is.True);
                    Assert.That(DE_GEF_P0001.  GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")), Is.Not.Null);

                }

            }

        }

        #endregion

        #region ChargingStation_AllProperties_Test()

        /// <summary>
        /// A test for creating a charging station within a charging pool having all properties.
        /// </summary>
        [Test]
        public void ChargingStation_AllProperties_Test()
        {

            Assert.That(roamingNetwork, Is.Not.Null);
            Assert.That(DE_GEF,         Is.Not.Null);
            Assert.That(DE_GEF_P0001,   Is.Not.Null);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                var success = false;

                var DE_GEF_S1234 = DE_GEF_P0001.AddChargingStation(
                                                    Id:                  ChargingStation_Id.Parse("DE*GEF*S1234"),
                                                    Name:                I18NString.Create(Languages.de, "DE*GEF Station 1234"),
                                                    Description:         I18NString.Create(Languages.de, "powered by GraphDefined Charging Stations GmbH"),
                                                    InitialAdminStatus:  ChargingStationAdminStatusType.OutOfService,
                                                    InitialStatus:       ChargingStationStatusType.Offline,
                                                    OnSuccess:           (evse, et) => success = true,
                                                    Configurator:        evse => {

                                                                             evse.Brands.Add(new Brand(
                                                                                                 Id:            Brand_Id.Parse("openChargingCloudChargingStation"),
                                                                                                 Name:          I18NString.Create(Languages.de, "Open Charging Cloud Charging Station"),
                                                                                                 Logo:          URL.Parse("https://open.charging.cloud/logos.json"),
                                                                                                 Homepage:      URL.Parse("https://open.charging.cloud"),
                                                                                                 DataLicenses:  new DataLicense[] {
                                                                                                                    DataLicense.CreativeCommons_BY_SA_4
                                                                                                                }
                                                                                             ));

                                                                         }
                                                ).Result.ChargingStation;

                Assert.That(DE_GEF_S1234, Is.Not.Null);
                Assert.That(success,      Is.True);

                if (DE_GEF_S1234 is not null)
                {

                    Assert.That(DE_GEF_S1234.Id.ToString(), Is.EqualTo("DE*GEF*S1234"));
                    Assert.That(DE_GEF_S1234.Name.FirstText(), Is.EqualTo("DE*GEF Station 1234"));
                    Assert.That(DE_GEF_S1234.Description.FirstText(), Is.EqualTo("powered by GraphDefined Charging Stations GmbH"));

                    Assert.That(DE_GEF_S1234.AdminStatus, Is.EqualTo(ChargingStationAdminStatusType.OutOfService));
                    Assert.That(DE_GEF_S1234.Status, Is.EqualTo(ChargingStationStatusType.Offline));

                    Assert.That(roamingNetwork.ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")), Is.True);
                    Assert.That(roamingNetwork.GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")), Is.Not.Null);

                    Assert.That(DE_GEF.        ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")), Is.True);
                    Assert.That(DE_GEF.        GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")), Is.Not.Null);

                    Assert.That(DE_GEF_P0001.  ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")), Is.True);
                    Assert.That(DE_GEF_P0001.  GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")), Is.Not.Null);


                    Assert.That(DE_GEF_S1234.Brands.Count(), Is.EqualTo(1));



                    DE_GEF_S1234.Brands.Add(new Brand(
                                                Id:            Brand_Id.Parse("openChargingCloud3223"),
                                                Name:          I18NString.Create(Languages.de, "Open Charging Cloud 3223"),
                                                Logo:          URL.Parse("https://open.charging.cloud/logos.json"),
                                                Homepage:      URL.Parse("https://open.charging.cloud"),
                                                DataLicenses:  new DataLicense[] {
                                                                   DataLicense.CreativeCommons_BY_SA_4
                                                               }
                                            ));


                    Assert.That(DE_GEF_S1234.Brands.Count(), Is.EqualTo(2));


                    #region Setup DataChange listeners

                    var chargingStationDataChanges = new List<String>();

                    DE_GEF_S1234.OnDataChanged += async (Timestamp,
                                                         EventTrackingId,
                                                         ChargingStation,
                                                         PropertyName,
                                                         NewValue,
                                                         OldValue,
                                                         dataSource) => {

                        chargingStationDataChanges.Add(String.Concat(ChargingStation.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };


                    var chargingPoolChargingStationDataChanges = new List<String>();

                    DE_GEF_P0001.OnChargingStationDataChanged += async (Timestamp,
                                                                        EventTrackingId,
                                                                        ChargingStation,
                                                                        PropertyName,
                                                                        NewValue,
                                                                        OldValue,
                                                                        dataSource) => {

                        chargingPoolChargingStationDataChanges.Add(String.Concat(ChargingStation.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };


                    var chargingStationOperatorChargingStationDataChanges = new List<String>();

                    DE_GEF.OnChargingStationDataChanged += async (Timestamp,
                                                                  EventTrackingId,
                                                                  ChargingStation,
                                                                  PropertyName,
                                                                  NewValue,
                                                                  OldValue,
                                                                  dataSource) => {

                        chargingStationOperatorChargingStationDataChanges.Add(String.Concat(ChargingStation.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };


                    var roamingNetworkChargingStationDataChanges = new List<String>();

                    roamingNetwork.OnChargingStationDataChanged += async (Timestamp,
                                                                          EventTrackingId,
                                                                          ChargingStation,
                                                                          PropertyName,
                                                                          NewValue,
                                                                          OldValue,
                                                                          dataSource) => {

                        roamingNetworkChargingStationDataChanges.Add(String.Concat(ChargingStation.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };

                    #endregion

                    DE_GEF_S1234.Name.       Set(Languages.it, "namelalala");
                    DE_GEF_S1234.Description.Set(Languages.it, "desclalala");

                    Assert.That(chargingStationDataChanges.Count, Is.EqualTo(2));
                    Assert.That(chargingPoolChargingStationDataChanges.Count, Is.EqualTo(2));
                    Assert.That(chargingStationOperatorChargingStationDataChanges.Count, Is.EqualTo(2));
                    Assert.That(roamingNetworkChargingStationDataChanges.Count, Is.EqualTo(2));


                    DE_GEF_S1234.MaxPower           = 123.45m;
                    DE_GEF_S1234.MaxPower           = 234.56m;

                    DE_GEF_S1234.MaxPowerRealTime   = 345.67m;
                    DE_GEF_S1234.MaxPowerRealTime   = 456.78m;

                    DE_GEF_S1234.MaxPowerPrognoses.Replace(new[] {
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(1), 567.89m),
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(2), 678.91m),
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(3), 789.12m)
                                                           });

                    Assert.That(chargingStationDataChanges.Count, Is.EqualTo(7));
                    Assert.That(chargingPoolChargingStationDataChanges.Count, Is.EqualTo(7));
                    Assert.That(chargingStationOperatorChargingStationDataChanges.Count, Is.EqualTo(7));
                    Assert.That(roamingNetworkChargingStationDataChanges.Count, Is.EqualTo(7));

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

            Assert.That(roamingNetwork,    Is.Not.Null);
            Assert.That(DE_GEF,            Is.Not.Null);
            Assert.That(DE_GEF_P0001,      Is.Not.Null);
            Assert.That(DE_GEF_S0001_AAAA, Is.Not.Null);

            if (roamingNetwork    is not null &&
                DE_GEF            is not null &&
                DE_GEF_P0001      is not null &&
                DE_GEF_S0001_AAAA is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.AdminStatus = ChargingStationAdminStatusType.InternalUse;
                Assert.That(DE_GEF_S0001_AAAA.AdminStatus, Is.EqualTo(ChargingStationAdminStatusType.InternalUse));
                Assert.That(DE_GEF_S0001_AAAA.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("internalUse, outOfService"));
                Assert.That(DE_GEF_S0001_AAAA.AdminStatusSchedule().Count(), Is.EqualTo(2));

                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.AdminStatus = ChargingStationAdminStatusType.Operational;
                Assert.That(DE_GEF_S0001_AAAA.AdminStatus, Is.EqualTo(ChargingStationAdminStatusType.Operational));
                Assert.That(DE_GEF_S0001_AAAA.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("operational, internalUse, outOfService"));
                Assert.That(DE_GEF_S0001_AAAA.AdminStatusSchedule().Count(), Is.EqualTo(3));


                Assert.That(DE_GEF_S0001_AAAA.GenerateAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(new IChargingStation[] { DE_GEF_S0001_AAAA }.GenerateAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(DE_GEF_P0001.GenerateChargingStationAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(new IChargingPool[] { DE_GEF_P0001 }.GenerateChargingStationAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(DE_GEF.GenerateChargingStationAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(new IChargingStationOperator[] { DE_GEF }.GenerateChargingStationAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));
                Assert.That(roamingNetwork.GenerateChargingStationAdminStatusReport().ToString(), Is.EqualTo("1 entities; operational: 1 (100.00)"));


                var jsonStatusReport = DE_GEF_S0001_AAAA.GenerateAdminStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.That(jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None),
                                Is.EqualTo("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingStationAdminStatusReport\",\"count\":1,\"report\":{\"operational\":{\"count\":1,\"percentage\":100.0}}}"));

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

            Assert.That(roamingNetwork,    Is.Not.Null);
            Assert.That(DE_GEF,            Is.Not.Null);
            Assert.That(DE_GEF_P0001,      Is.Not.Null);
            Assert.That(DE_GEF_S0001_AAAA, Is.Not.Null);

            if (roamingNetwork    is not null &&
                DE_GEF            is not null &&
                DE_GEF_P0001      is not null &&
                DE_GEF_S0001_AAAA is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.Status = ChargingStationStatusType.InDeployment;
                Assert.That(DE_GEF_S0001_AAAA.Status, Is.EqualTo(ChargingStationStatusType.InDeployment));
                Assert.That(DE_GEF_S0001_AAAA.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("inDeployment, offline"));
                Assert.That(DE_GEF_S0001_AAAA.StatusSchedule().Count(), Is.EqualTo(2));

                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.Status = ChargingStationStatusType.Error;
                Assert.That(DE_GEF_S0001_AAAA.Status, Is.EqualTo(ChargingStationStatusType.Error));
                Assert.That(DE_GEF_S0001_AAAA.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "), Is.EqualTo("error, inDeployment, offline"));
                Assert.That(DE_GEF_S0001_AAAA.StatusSchedule().Count(), Is.EqualTo(3));


                Assert.That(DE_GEF_S0001_AAAA.GenerateStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(new IChargingStation[] { DE_GEF_S0001_AAAA }.GenerateStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(DE_GEF_P0001.GenerateChargingStationStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(new IChargingPool[] { DE_GEF_P0001 }.GenerateChargingStationStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(DE_GEF.GenerateChargingStationStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(new IChargingStationOperator[] { DE_GEF }.GenerateChargingStationStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));
                Assert.That(roamingNetwork.GenerateChargingStationStatusReport().ToString(), Is.EqualTo("1 entities; error: 1 (100.00)"));


                var jsonStatusReport = DE_GEF_S0001_AAAA.GenerateStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.That(jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None),
                                Is.EqualTo("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingStationStatusReport\",\"count\":1,\"report\":{\"error\":{\"count\":1,\"percentage\":100.0}}}"));

            }

        }

        #endregion


    }

}
