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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);
            ClassicAssert.IsNotNull(DE_GEF_P0001);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                ClassicAssert.AreEqual ("DE*GEF*P0001",                                 DE_GEF_P0001.Id.         ToString());
                ClassicAssert.AreEqual ("GraphDefined Charging Pool #1",                DE_GEF_P0001.Name.       FirstText());
                ClassicAssert.AreEqual ("powered by GraphDefined Charging Pools GmbH",  DE_GEF_P0001.Description.FirstText());

                ClassicAssert.AreEqual (ChargingPoolAdminStatusTypes.OutOfService,      DE_GEF_P0001.AdminStatus);
                ClassicAssert.AreEqual (1,                                              DE_GEF_P0001.AdminStatusSchedule().Count());

                ClassicAssert.AreEqual (ChargingPoolStatusTypes.Offline,                DE_GEF_P0001.Status);
                ClassicAssert.AreEqual (1,                                              DE_GEF_P0001.StatusSchedule().     Count());


                ClassicAssert.AreEqual (1,                                              roamingNetwork.ChargingPools.    Count());
                ClassicAssert.AreEqual (1,                                              roamingNetwork.ChargingPoolIds().Count());

                ClassicAssert.AreEqual (1,                                              DE_GEF.        ChargingPools.    Count());
                ClassicAssert.AreEqual (1,                                              DE_GEF.        ChargingPoolIds().Count());


                ClassicAssert.IsTrue   (roamingNetwork.ContainsChargingPool(ChargingPool_Id.Parse("DE*GEF*P0001")));
                ClassicAssert.IsNotNull(roamingNetwork.GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P0001")));

                ClassicAssert.IsTrue   (DE_GEF.        ChargingPoolExists  (ChargingPool_Id.Parse("DE*GEF*P0001")));
                ClassicAssert.IsNotNull(DE_GEF.        GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P0001")));

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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                var DE_GEF_P1234Result = DE_GEF.AddChargingPool(
                                             Id:           ChargingPool_Id.Parse("DE*GEF*P1234"),
                                             Name:         I18NString.Create(Languages.de, "DE*GEF Pool 1234"),
                                             Description:  I18NString.Create(Languages.de, "powered by GraphDefined Charging Pools GmbH")
                                         ).Result;

                var DE_GEF_P1234 = DE_GEF_P1234Result.ChargingPool;

                ClassicAssert.IsNotNull(DE_GEF_P1234);

                if (DE_GEF_P1234 is not null)
                {

                    ClassicAssert.AreEqual ("DE*GEF*P1234",                                 DE_GEF_P1234.Id.         ToString());
                    ClassicAssert.AreEqual ("DE*GEF Pool 1234",                             DE_GEF_P1234.Name.       FirstText());
                    ClassicAssert.AreEqual ("powered by GraphDefined Charging Pools GmbH",  DE_GEF_P1234.Description.FirstText());

                    ClassicAssert.AreEqual (ChargingPoolAdminStatusTypes.Operational,       DE_GEF_P1234.AdminStatus);
                    ClassicAssert.AreEqual (ChargingPoolStatusTypes.Available,              DE_GEF_P1234.Status);

                    ClassicAssert.IsTrue   (roamingNetwork.ContainsChargingPool(ChargingPool_Id.Parse("DE*GEF*P1234")));
                    ClassicAssert.IsNotNull(roamingNetwork.GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P1234")));

                    ClassicAssert.IsTrue   (DE_GEF.        ChargingPoolExists  (ChargingPool_Id.Parse("DE*GEF*P1234")));
                    ClassicAssert.IsNotNull(DE_GEF.        GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P1234")));

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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                var success = false;

                var DE_GEF_P1234Result = DE_GEF.AddChargingPool(
                                             Id:                  ChargingPool_Id.Parse("DE*GEF*P1234"),
                                             Name:                I18NString.Create(Languages.de, "DE*GEF Pool 1234"),
                                             Description:         I18NString.Create(Languages.de, "powered by GraphDefined Charging Pools GmbH"),
                                             InitialAdminStatus:  ChargingPoolAdminStatusTypes.OutOfService,
                                             InitialStatus:       ChargingPoolStatusTypes.Offline,
                                             OnSuccess:           (chargingPool, eventTrackingId) => success = true,
                                             Configurator:        chargingPool => {

                                                                      chargingPool.Brands.Add(new Brand(
                                                                                                  Id:            Brand_Id.Parse("openChargingCloudChargingPool"),
                                                                                                  Name:          I18NString.Create(Languages.de, "Open Charging Cloud Charging Pool"),
                                                                                                  Logo:          URL.Parse("https://open.charging.cloud/logos.json"),
                                                                                                  Homepage:      URL.Parse("https://open.charging.cloud"),
                                                                                                  DataLicenses:  new OpenDataLicense[] {
                                                                                                                     OpenDataLicense.CreativeCommons_BY_SA_4
                                                                                                                 }
                                                                                              ));

                                                                  }
                                         ).Result;

                var DE_GEF_P1234 = DE_GEF_P1234Result.ChargingPool;

                ClassicAssert.IsNotNull(DE_GEF_P1234);
                ClassicAssert.IsTrue   (success);

                if (DE_GEF_P1234 is not null)
                {

                    ClassicAssert.AreEqual ("DE*GEF*P1234",                                 DE_GEF_P1234.Id.         ToString());
                    ClassicAssert.AreEqual ("DE*GEF Pool 1234",                             DE_GEF_P1234.Name.       FirstText());
                    ClassicAssert.AreEqual ("powered by GraphDefined Charging Pools GmbH",  DE_GEF_P1234.Description.FirstText());

                    ClassicAssert.AreEqual (ChargingPoolAdminStatusTypes.OutOfService,      DE_GEF_P1234.AdminStatus);
                    ClassicAssert.AreEqual (ChargingPoolStatusTypes.Offline,                DE_GEF_P1234.Status);

                    ClassicAssert.IsTrue   (roamingNetwork.ContainsChargingPool(ChargingPool_Id.Parse("DE*GEF*P1234")));
                    ClassicAssert.IsNotNull(roamingNetwork.GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P1234")));

                    ClassicAssert.IsTrue   (DE_GEF.        ChargingPoolExists  (ChargingPool_Id.Parse("DE*GEF*P1234")));
                    ClassicAssert.IsNotNull(DE_GEF.        GetChargingPoolById (ChargingPool_Id.Parse("DE*GEF*P1234")));


                    ClassicAssert.AreEqual(1, DE_GEF_P1234.Brands.Count());



                    DE_GEF_P1234.Brands.Add(new Brand(
                                                Id:            Brand_Id.Parse("openChargingCloud3223"),
                                                Name:          I18NString.Create(Languages.de, "Open Charging Cloud 3223"),
                                                Logo:          URL.Parse("https://open.charging.cloud/logos.json"),
                                                Homepage:      URL.Parse("https://open.charging.cloud"),
                                                DataLicenses:  new OpenDataLicense[] {
                                                                   OpenDataLicense.CreativeCommons_BY_SA_4
                                                               }
                                            ));


                    ClassicAssert.AreEqual(2, DE_GEF_P1234.Brands.Count());


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

                    ClassicAssert.AreEqual(2, chargingPoolDataChanges.                       Count);
                    ClassicAssert.AreEqual(2, chargingStationOperatorChargingPoolDataChanges.Count);
                    ClassicAssert.AreEqual(2, roamingNetworkChargingPoolDataChanges.         Count);


                    DE_GEF_P1234.MaxPower           = 123.45m;
                    DE_GEF_P1234.MaxPower           = 234.56m;

                    DE_GEF_P1234.MaxPowerRealTime   = 345.67m;
                    DE_GEF_P1234.MaxPowerRealTime   = 456.78m;

                    DE_GEF_P1234.MaxPowerPrognoses.Replace(new[] {
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(1), 567.89m),
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(2), 678.91m),
                                                               new Timestamped<Decimal>(Timestamp.Now + TimeSpan.FromMinutes(3), 789.12m)
                                                           });

                    ClassicAssert.AreEqual(7, chargingPoolDataChanges.                       Count);
                    ClassicAssert.AreEqual(7, chargingStationOperatorChargingPoolDataChanges.Count);
                    ClassicAssert.AreEqual(7, roamingNetworkChargingPoolDataChanges.         Count);

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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);
            ClassicAssert.IsNotNull(DE_GEF_P0001);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_P0001.AdminStatus = ChargingPoolAdminStatusTypes.InternalUse;
                ClassicAssert.AreEqual(ChargingPoolAdminStatusTypes.InternalUse,  DE_GEF_P0001.AdminStatus);
                ClassicAssert.AreEqual("internalUse, outOfService",               DE_GEF_P0001.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(2,                                         DE_GEF_P0001.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF_P0001.AdminStatus = ChargingPoolAdminStatusTypes.Operational;
                ClassicAssert.AreEqual(ChargingPoolAdminStatusTypes.Operational,  DE_GEF_P0001.AdminStatus);
                ClassicAssert.AreEqual("operational, internalUse, outOfService",  DE_GEF_P0001.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(3,                                         DE_GEF_P0001.AdminStatusSchedule().Count());


                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", DE_GEF_P0001.                                   GenerateAdminStatusReport().            ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", new IChargingPool[]            { DE_GEF_P0001 }.GenerateAdminStatusReport().            ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", DE_GEF.                                         GenerateChargingPoolAdminStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", new IChargingStationOperator[] { DE_GEF }.      GenerateChargingPoolAdminStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; operational: 1 (100,00)", roamingNetwork.                                 GenerateChargingPoolAdminStatusReport().ToString());


                var jsonStatusReport = DE_GEF_P0001.GenerateAdminStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                ClassicAssert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingPoolAdminStatusReport\",\"count\":1,\"report\":{\"operational\":{\"count\":1,\"percentage\":100.0}}}",
                                jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None));

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

            ClassicAssert.IsNotNull(roamingNetwork);
            ClassicAssert.IsNotNull(DE_GEF);
            ClassicAssert.IsNotNull(DE_GEF_P0001);

            if (roamingNetwork is not null &&
                DE_GEF         is not null &&
                DE_GEF_P0001   is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF_P0001.Status = ChargingPoolStatusTypes.InDeployment;
                ClassicAssert.AreEqual(ChargingPoolStatusTypes.InDeployment,  DE_GEF_P0001.Status);
                ClassicAssert.AreEqual("inDeployment, offline",               DE_GEF_P0001.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(2,                                     DE_GEF_P0001.StatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF_P0001.Status = ChargingPoolStatusTypes.Error;
                ClassicAssert.AreEqual(ChargingPoolStatusTypes.Error,         DE_GEF_P0001.Status);
                ClassicAssert.AreEqual("error, inDeployment, offline",        DE_GEF_P0001.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                ClassicAssert.AreEqual(3,                                     DE_GEF_P0001.StatusSchedule().Count());


                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", DE_GEF_P0001.                                   GenerateStatusReport().            ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", new IChargingPool[]            { DE_GEF_P0001 }.GenerateStatusReport().            ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", DE_GEF.                                         GenerateChargingPoolStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", new IChargingStationOperator[] { DE_GEF }.      GenerateChargingPoolStatusReport().ToString());
                ClassicAssert.AreEqual("1 entities; error: 1 (100,00)", roamingNetwork.                                 GenerateChargingPoolStatusReport().ToString());


                var jsonStatusReport = DE_GEF_P0001.GenerateStatusReport().ToJSON();
                jsonStatusReport.Remove("timestamp");

                ClassicAssert.AreEqual("{\"@context\":\"https://open.charging.cloud/contexts/wwcp+json/chargingPoolStatusReport\",\"count\":1,\"report\":{\"error\":{\"count\":1,\"percentage\":100.0}}}",
                                jsonStatusReport.ToString(Newtonsoft.Json.Formatting.None));

            }

        }

        #endregion


    }

}
