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
                Assert.AreEqual (1,                                                 DE_GEF_S0001_AAAA.AdminStatusSchedule().Count());

                Assert.AreEqual (ChargingStationStatusTypes.Offline,                DE_GEF_S0001_AAAA.Status);
                Assert.AreEqual (1,                                                 DE_GEF_S0001_AAAA.StatusSchedule().     Count());


                Assert.AreEqual (1,                                                 roamingNetwork.ChargingStations.    Count());
                Assert.AreEqual (1,                                                 roamingNetwork.ChargingStationIds().Count());

                Assert.AreEqual (1,                                                 DE_GEF.        ChargingStations.    Count());
                Assert.AreEqual (1,                                                 DE_GEF.        ChargingStationIds().Count());

                Assert.AreEqual (1,                                                 DE_GEF_P0001.  ChargingStations.    Count());
                Assert.AreEqual (1,                                                 DE_GEF_P0001.  ChargingStationIds().Count());


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
        /// A test for creating a charging station within a charging pool.
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
                                                    Id:           ChargingStation_Id.Parse("DE*GEF*S1234"),
                                                    Name:         I18NString.Create(Languages.de, "DE*GEF Station 1234"),
                                                    Description:  I18NString.Create(Languages.de, "powered by GraphDefined Charging Stations GmbH")
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

        #region ChargingStation_AllProperties_Test()

        /// <summary>
        /// A test for creating a charging station within a charging pool having all properties.
        /// </summary>
        [Test]
        public void ChargingStation_AllProperties_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);
            Assert.IsNotNull(DE_GEF_P0001);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                var success = false;

                var DE_GEF_S1234 = DE_GEF_P0001.CreateChargingStation(
                                                    Id:                  ChargingStation_Id.Parse("DE*GEF*S1234"),
                                                    Name:                I18NString.Create(Languages.de, "DE*GEF Station 1234"),
                                                    Description:         I18NString.Create(Languages.de, "powered by GraphDefined Charging Stations GmbH"),
                                                    InitialAdminStatus:  ChargingStationAdminStatusTypes.OutOfService,
                                                    InitialStatus:       ChargingStationStatusTypes.Offline,
                                                    OnSuccess:           evse => success = true,
                                                    Configurator:        evse => {

                                                                             evse.Brands.TryAdd(new Brand(
                                                                                                    Id:            Brand_Id.Parse("openChargingCloudChargingStation"),
                                                                                                    Name:          I18NString.Create(Languages.de, "Open Charging Cloud Charging Station"),
                                                                                                    Logo:          URL.Parse("https://open.charging.cloud/logos.json"),
                                                                                                    Homepage:      URL.Parse("https://open.charging.cloud"),
                                                                                                    DataLicenses:  new DataLicense[] {
                                                                                                                       DataLicense.CreativeCommons_BY_SA_4
                                                                                                                   }
                                                                                                ));

                                                                         }
                                                );

                Assert.IsNotNull(DE_GEF_S1234);
                Assert.IsTrue   (success);

                if (DE_GEF_S1234 is not null)
                {

                    Assert.AreEqual ("DE*GEF*S1234",                                    DE_GEF_S1234.Id.         ToString());
                    Assert.AreEqual ("DE*GEF Station 1234",                             DE_GEF_S1234.Name.       FirstText());
                    Assert.AreEqual ("powered by GraphDefined Charging Stations GmbH",  DE_GEF_S1234.Description.FirstText());

                    Assert.AreEqual (ChargingStationAdminStatusTypes.OutOfService,      DE_GEF_S1234.AdminStatus);
                    Assert.AreEqual (ChargingStationStatusTypes.Offline,                DE_GEF_S1234.Status);

                    Assert.IsTrue   (roamingNetwork.ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    Assert.IsNotNull(roamingNetwork.GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));

                    Assert.IsTrue   (DE_GEF.        ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    Assert.IsNotNull(DE_GEF.        GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));

                    Assert.IsTrue   (DE_GEF_P0001.  ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    Assert.IsNotNull(DE_GEF_P0001.  GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));


                    Assert.AreEqual(1, DE_GEF_S1234.Brands.Count());



                    DE_GEF_S1234.Brands.TryAdd(new Brand(
                                                   Id:            Brand_Id.Parse("openChargingCloud3223"),
                                                   Name:          I18NString.Create(Languages.de, "Open Charging Cloud 3223"),
                                                   Logo:          URL.Parse("https://open.charging.cloud/logos.json"),
                                                   Homepage:      URL.Parse("https://open.charging.cloud"),
                                                   DataLicenses:  new DataLicense[] {
                                                                      DataLicense.CreativeCommons_BY_SA_4
                                                                  }
                                               ));


                    Assert.AreEqual(2, DE_GEF_S1234.Brands.Count());


                    #region Setup DataChange listeners

                    var chargingStationDataChanges = new List<String>();

                    DE_GEF_S1234.OnDataChanged += async (Timestamp,
                                                         EventTrackingId,
                                                         ChargingStation,
                                                         PropertyName,
                                                         OldValue,
                                                         NewValue) => {

                        chargingStationDataChanges.Add(String.Concat(ChargingStation.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };


                    var chargingPoolChargingStationDataChanges = new List<String>();

                    DE_GEF_P0001.OnChargingStationDataChanged += async (Timestamp,
                                                                        EventTrackingId,
                                                                        ChargingStation,
                                                                        PropertyName,
                                                                        OldValue,
                                                                        NewValue) => {

                        chargingPoolChargingStationDataChanges.Add(String.Concat(ChargingStation.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };


                    var chargingStationOperatorChargingStationDataChanges = new List<String>();

                    DE_GEF.OnChargingStationDataChanged += async (Timestamp,
                                                                  EventTrackingId,
                                                                  ChargingStation,
                                                                  PropertyName,
                                                                  OldValue,
                                                                  NewValue) => {

                        chargingStationOperatorChargingStationDataChanges.Add(String.Concat(ChargingStation.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };


                    var roamingNetworkChargingStationDataChanges = new List<String>();

                    roamingNetwork.OnChargingStationDataChanged += async (Timestamp,
                                                                          EventTrackingId,
                                                                          ChargingStation,
                                                                          PropertyName,
                                                                          OldValue,
                                                                          NewValue) => {

                        roamingNetworkChargingStationDataChanges.Add(String.Concat(ChargingStation.ToString(), ".", PropertyName, ": ", OldValue?.ToString() ?? "", " => ", NewValue?.ToString() ?? ""));

                    };

                    #endregion

                    DE_GEF_S1234.Name.       Add(Languages.it, "namelalala");
                    DE_GEF_S1234.Description.Add(Languages.it, "desclalala");

                    Assert.AreEqual(2, chargingStationDataChanges.                       Count);
                    Assert.AreEqual(2, chargingPoolChargingStationDataChanges.           Count);
                    Assert.AreEqual(2, chargingStationOperatorChargingStationDataChanges.Count);
                    Assert.AreEqual(2, roamingNetworkChargingStationDataChanges.         Count);


                    //DE_GEF_S1234.MaxPower           = 123.45m;
                    //DE_GEF_S1234.MaxPower           = 234.56m;

                    //DE_GEF_S1234.MaxPowerRealTime   = 345.67m;
                    //DE_GEF_S1234.MaxPowerRealTime   = 456.78m;

                    //DE_GEF_S1234.MaxPowerPrognoses  = new Timestamped<Decimal>[] {
                    //                                      new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(1), 567.89m),
                    //                                      new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(2), 678.91m),
                    //                                      new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(3), 789.12m)
                    //                                  };

                    //Assert.AreEqual(7, chargingStationDataChanges.                       Count);
                    //Assert.AreEqual(7, chargingPoolChargingStationDataChanges.           Count);
                    //Assert.AreEqual(7, chargingStationOperatorChargingStationDataChanges.Count);
                    //Assert.AreEqual(7, roamingNetworkChargingStationDataChanges.         Count);

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
            Assert.IsNotNull(DE_GEF_P0001);
            Assert.IsNotNull(DE_GEF_S0001_AAAA);

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


                Assert.AreEqual("1 entities; Operational: 1 (100,00)", DE_GEF_S0001_AAAA.                                    GenerateAdminStatusReport().               ToString());
                Assert.AreEqual("1 entities; Operational: 1 (100,00)", new ChargingStation[]         { DE_GEF_S0001_AAAA }.  GenerateAdminStatusReport().               ToString());
                Assert.AreEqual("1 entities; Operational: 1 (100,00)", DE_GEF_P0001.                                         GenerateChargingStationAdminStatusReport().ToString());
                Assert.AreEqual("1 entities; Operational: 1 (100,00)", new ChargingPool[]            { DE_GEF_P0001 }.       GenerateChargingStationAdminStatusReport().ToString());
                Assert.AreEqual("1 entities; Operational: 1 (100,00)", DE_GEF.                                               GenerateChargingStationAdminStatusReport().ToString());
                Assert.AreEqual("1 entities; Operational: 1 (100,00)", new ChargingStationOperator[] { DE_GEF }.             GenerateChargingStationAdminStatusReport().ToString());
                Assert.AreEqual("1 entities; Operational: 1 (100,00)", roamingNetwork.                                       GenerateChargingStationAdminStatusReport().ToString());


                var jsonStatusReport = DE_GEF_S0001_AAAA.GenerateAdminStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingStationAdminStatusReport\",\"count\":1,\"report\":{\"Operational\":{\"count\":1,\"percentage\":100.0}}}",
                                jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None));

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
            Assert.IsNotNull(DE_GEF_P0001);
            Assert.IsNotNull(DE_GEF_S0001_AAAA);

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


                Assert.AreEqual("1 entities; Faulted: 1 (100,00)", DE_GEF_S0001_AAAA.                                    GenerateStatusReport().               ToString());
                Assert.AreEqual("1 entities; Faulted: 1 (100,00)", new ChargingStation[]         { DE_GEF_S0001_AAAA }.  GenerateStatusReport().               ToString());
                Assert.AreEqual("1 entities; Faulted: 1 (100,00)", DE_GEF_P0001.                                         GenerateChargingStationStatusReport().ToString());
                Assert.AreEqual("1 entities; Faulted: 1 (100,00)", new ChargingPool[]            { DE_GEF_P0001 }.       GenerateChargingStationStatusReport().ToString());
                Assert.AreEqual("1 entities; Faulted: 1 (100,00)", DE_GEF.                                               GenerateChargingStationStatusReport().ToString());
                Assert.AreEqual("1 entities; Faulted: 1 (100,00)", new ChargingStationOperator[] { DE_GEF }.             GenerateChargingStationStatusReport().ToString());
                Assert.AreEqual("1 entities; Faulted: 1 (100,00)", roamingNetwork.                                       GenerateChargingStationStatusReport().ToString());


                var jsonStatusReport = DE_GEF_S0001_AAAA.GenerateStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                Assert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingStationStatusReport\",\"count\":1,\"report\":{\"Faulted\":{\"count\":1,\"percentage\":100.0}}}",
                                jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None));

            }

        }

        #endregion


    }

}
