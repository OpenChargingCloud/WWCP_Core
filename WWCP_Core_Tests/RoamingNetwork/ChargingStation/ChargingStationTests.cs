/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);
            ClassicAssert.IsNotNull(DE_GEF_P0001);
            ClassicAssert.IsNotNull(DE_GEF_S0001_AAAA);

            if (roamingNetwork    is not null &&
                DE_GEF            is not null &&
                DE_GEF_P0001      is not null &&
                DE_GEF_S0001_AAAA is not null)
            {

                ClassicAssert.AreEqual ("DE*GEF*S0001*AAAA",                               DE_GEF_S0001_AAAA.Id.         ToString());
                ClassicAssert.AreEqual ("GraphDefined Charging Station #AAAA",             DE_GEF_S0001_AAAA.Name.       FirstText());
                ClassicAssert.AreEqual ("powered by GraphDefined Charging Stations GmbH",  DE_GEF_S0001_AAAA.Description.FirstText());

                ClassicAssert.AreEqual (ChargingStationAdminStatusTypes.OutOfService,      DE_GEF_S0001_AAAA.AdminStatus);
                ClassicAssert.AreEqual (1,                                                 DE_GEF_S0001_AAAA.AdminStatusSchedule().Count());

                ClassicAssert.AreEqual (ChargingStationStatusTypes.Offline,                DE_GEF_S0001_AAAA.Status);
                ClassicAssert.AreEqual (1,                                                 DE_GEF_S0001_AAAA.StatusSchedule().     Count());


                ClassicAssert.AreEqual (1,                                                 roamingNetwork.ChargingStations.    Count());
                ClassicAssert.AreEqual (1,                                                 roamingNetwork.ChargingStationIds().Count());

                ClassicAssert.AreEqual (1,                                                 DE_GEF.        ChargingStations.    Count());
                ClassicAssert.AreEqual (1,                                                 DE_GEF.        ChargingStationIds().Count());

                ClassicAssert.AreEqual (1,                                                 DE_GEF_P0001.  ChargingStations.    Count());
                ClassicAssert.AreEqual (1,                                                 DE_GEF_P0001.  ChargingStationIds().Count());


                ClassicAssert.IsTrue   (roamingNetwork.ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));
                ClassicAssert.IsNotNull(roamingNetwork.GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));

                ClassicAssert.IsTrue   (DE_GEF.        ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));
                ClassicAssert.IsNotNull(DE_GEF.        GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));

                ClassicAssert.IsTrue   (DE_GEF_P0001.  ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));
                ClassicAssert.IsNotNull(DE_GEF_P0001.  GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S0001*AAAA")));

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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);
            ClassicAssert.IsNotNull(DE_GEF_P0001);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                var DE_GEF_S1234 = DE_GEF_P0001.AddChargingStation(
                                                    Id:           ChargingStation_Id.Parse("DE*GEF*S1234"),
                                                    Name:         I18NString.Create(Languages.de, "DE*GEF Station 1234"),
                                                    Description:  I18NString.Create(Languages.de, "powered by GraphDefined Charging Stations GmbH")
                                                ).Result.ChargingStation;

                ClassicAssert.IsNotNull(DE_GEF_S1234);

                if (DE_GEF_S1234 is not null)
                {

                    ClassicAssert.AreEqual ("DE*GEF*S1234",                                    DE_GEF_S1234.Id.         ToString());
                    ClassicAssert.AreEqual ("DE*GEF Station 1234",                             DE_GEF_S1234.Name.       FirstText());
                    ClassicAssert.AreEqual ("powered by GraphDefined Charging Stations GmbH",  DE_GEF_S1234.Description.FirstText());

                    ClassicAssert.AreEqual (ChargingStationAdminStatusTypes.Operational,       DE_GEF_S1234.AdminStatus);
                    ClassicAssert.AreEqual (ChargingStationStatusTypes.Available,              DE_GEF_S1234.Status);

                    ClassicAssert.IsTrue   (roamingNetwork.ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    ClassicAssert.IsNotNull(roamingNetwork.GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));

                    ClassicAssert.IsTrue   (DE_GEF.        ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    ClassicAssert.IsNotNull(DE_GEF.        GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));

                    ClassicAssert.IsTrue   (DE_GEF_P0001.  ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    ClassicAssert.IsNotNull(DE_GEF_P0001.  GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));

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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);
            ClassicAssert.IsNotNull(DE_GEF_P0001);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                var success = false;

                var DE_GEF_S1234 = DE_GEF_P0001.AddChargingStation(
                                                    Id:                  ChargingStation_Id.Parse("DE*GEF*S1234"),
                                                    Name:                I18NString.Create(Languages.de, "DE*GEF Station 1234"),
                                                    Description:         I18NString.Create(Languages.de, "powered by GraphDefined Charging Stations GmbH"),
                                                    InitialAdminStatus:  ChargingStationAdminStatusTypes.OutOfService,
                                                    InitialStatus:       ChargingStationStatusTypes.Offline,
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

                ClassicAssert.IsNotNull(DE_GEF_S1234);
                ClassicAssert.IsTrue   (success);

                if (DE_GEF_S1234 is not null)
                {

                    ClassicAssert.AreEqual ("DE*GEF*S1234",                                    DE_GEF_S1234.Id.         ToString());
                    ClassicAssert.AreEqual ("DE*GEF Station 1234",                             DE_GEF_S1234.Name.       FirstText());
                    ClassicAssert.AreEqual ("powered by GraphDefined Charging Stations GmbH",  DE_GEF_S1234.Description.FirstText());

                    ClassicAssert.AreEqual (ChargingStationAdminStatusTypes.OutOfService,      DE_GEF_S1234.AdminStatus);
                    ClassicAssert.AreEqual (ChargingStationStatusTypes.Offline,                DE_GEF_S1234.Status);

                    ClassicAssert.IsTrue   (roamingNetwork.ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    ClassicAssert.IsNotNull(roamingNetwork.GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));

                    ClassicAssert.IsTrue   (DE_GEF.        ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    ClassicAssert.IsNotNull(DE_GEF.        GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));

                    ClassicAssert.IsTrue   (DE_GEF_P0001.  ContainsChargingStation(ChargingStation_Id.Parse("DE*GEF*S1234")));
                    ClassicAssert.IsNotNull(DE_GEF_P0001.  GetChargingStationById (ChargingStation_Id.Parse("DE*GEF*S1234")));


                    ClassicAssert.AreEqual(1, DE_GEF_S1234.Brands.Count());



                    DE_GEF_S1234.Brands.Add(new Brand(
                                                Id:            Brand_Id.Parse("openChargingCloud3223"),
                                                Name:          I18NString.Create(Languages.de, "Open Charging Cloud 3223"),
                                                Logo:          URL.Parse("https://open.charging.cloud/logos.json"),
                                                Homepage:      URL.Parse("https://open.charging.cloud"),
                                                DataLicenses:  new DataLicense[] {
                                                                   DataLicense.CreativeCommons_BY_SA_4
                                                               }
                                            ));


                    ClassicAssert.AreEqual(2, DE_GEF_S1234.Brands.Count());


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

                    ClassicAssert.AreEqual(2, chargingStationDataChanges.                       Count);
                    ClassicAssert.AreEqual(2, chargingPoolChargingStationDataChanges.           Count);
                    ClassicAssert.AreEqual(2, chargingStationOperatorChargingStationDataChanges.Count);
                    ClassicAssert.AreEqual(2, roamingNetworkChargingStationDataChanges.         Count);


                    DE_GEF_S1234.MaxPower           = 123.45m;
                    DE_GEF_S1234.MaxPower           = 234.56m;

                    DE_GEF_S1234.MaxPowerRealTime   = 345.67m;
                    DE_GEF_S1234.MaxPowerRealTime   = 456.78m;

                    DE_GEF_S1234.MaxPowerPrognoses.Replace(new[] {
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(1), 567.89m),
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(2), 678.91m),
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(3), 789.12m)
                                                           });

                    ClassicAssert.AreEqual(7, chargingStationDataChanges.                       Count);
                    ClassicAssert.AreEqual(7, chargingPoolChargingStationDataChanges.           Count);
                    ClassicAssert.AreEqual(7, chargingStationOperatorChargingStationDataChanges.Count);
                    ClassicAssert.AreEqual(7, roamingNetworkChargingStationDataChanges.         Count);

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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);
            ClassicAssert.IsNotNull(DE_GEF_P0001);
            ClassicAssert.IsNotNull(DE_GEF_S0001_AAAA);

            if (roamingNetwork    is not null &&
                DE_GEF            is not null &&
                DE_GEF_P0001      is not null &&
                DE_GEF_S0001_AAAA is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.AdminStatus = ChargingStationAdminStatusTypes.InternalUse;
                ClassicAssert.AreEqual(ChargingStationAdminStatusTypes.InternalUse,  DE_GEF_S0001_AAAA.AdminStatus);
                ClassicAssert.AreEqual("internalUse, outOfService",                  DE_GEF_S0001_AAAA.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(2,                                            DE_GEF_S0001_AAAA.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.AdminStatus = ChargingStationAdminStatusTypes.Operational;
                ClassicAssert.AreEqual(ChargingStationAdminStatusTypes.Operational,  DE_GEF_S0001_AAAA.AdminStatus);
                ClassicAssert.AreEqual("operational, internalUse, outOfService",     DE_GEF_S0001_AAAA.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(3,                                            DE_GEF_S0001_AAAA.AdminStatusSchedule().Count());


                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", DE_GEF_S0001_AAAA.                                     GenerateAdminStatusReport().               ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", new IChargingStation[]         { DE_GEF_S0001_AAAA }.  GenerateAdminStatusReport().               ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", DE_GEF_P0001.                                          GenerateChargingStationAdminStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", new IChargingPool[]            { DE_GEF_P0001 }.       GenerateChargingStationAdminStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", DE_GEF.                                                GenerateChargingStationAdminStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", new IChargingStationOperator[] { DE_GEF }.             GenerateChargingStationAdminStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", roamingNetwork.                                        GenerateChargingStationAdminStatusReport().ToString());


                var jsonStatusReport = DE_GEF_S0001_AAAA.GenerateAdminStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                ClassicAssert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingStationAdminStatusReport\",\"count\":1,\"report\":{\"operational\":{\"count\":1,\"percentage\":100.0}}}",
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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);
            ClassicAssert.IsNotNull(DE_GEF_P0001);
            ClassicAssert.IsNotNull(DE_GEF_S0001_AAAA);

            if (roamingNetwork    is not null &&
                DE_GEF            is not null &&
                DE_GEF_P0001      is not null &&
                DE_GEF_S0001_AAAA is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.Status = ChargingStationStatusTypes.InDeployment;
                ClassicAssert.AreEqual(ChargingStationStatusTypes.InDeployment, DE_GEF_S0001_AAAA.Status);
                ClassicAssert.AreEqual("inDeployment, offline",           DE_GEF_S0001_AAAA.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(2,                                 DE_GEF_S0001_AAAA.StatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF_S0001_AAAA.Status = ChargingStationStatusTypes.Error;
                ClassicAssert.AreEqual(ChargingStationStatusTypes.Error,  DE_GEF_S0001_AAAA.Status);
                ClassicAssert.AreEqual("error, inDeployment, offline",    DE_GEF_S0001_AAAA.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(3,                                 DE_GEF_S0001_AAAA.StatusSchedule().Count());


                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", DE_GEF_S0001_AAAA.                                     GenerateStatusReport().               ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", new IChargingStation[]         { DE_GEF_S0001_AAAA }.  GenerateStatusReport().               ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", DE_GEF_P0001.                                          GenerateChargingStationStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", new IChargingPool[]            { DE_GEF_P0001 }.       GenerateChargingStationStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", DE_GEF.                                                GenerateChargingStationStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", new IChargingStationOperator[] { DE_GEF }.             GenerateChargingStationStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", roamingNetwork.                                        GenerateChargingStationStatusReport().ToString());


                var jsonStatusReport = DE_GEF_S0001_AAAA.GenerateStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                ClassicAssert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingStationStatusReport\",\"count\":1,\"report\":{\"error\":{\"count\":1,\"percentage\":100.0}}}",
                                jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None));

            }

        }

        #endregion


    }

}
