/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Net <https://github.com/GraphDefined/WWCP_Net>
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

using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using NUnit.Framework;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using org.GraphDefined.WWCP.Net;
using org.GraphDefined.WWCP.Net.IO.JSON;

#endregion

namespace org.GraphDefined.WWCP.Net.UnitTests
{

    /// <summary>
    /// Minimal unit tests.
    /// </summary>
    [TestFixture]
    public class WWCP_Net_JSON_IO_Tests
    {

        #region Data

        private          HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPAPI;
        private          WWCP_HTTPAPI                                 WWCPAPI;
        private readonly TimeSpan                                     Timeout  = TimeSpan.FromSeconds(20);

        #endregion


        #region Test_ChargingPools()

        [Test]
        public void Test_ChargingPools()
        {

            var HTTPClient = new HTTPClient(IPv4Address.Localhost,
                                            RemotePort: IPPort.Parse(8000),
                                            DNSClient:  HTTPAPI.DNSClient);


            var RN      = WWCPAPI.CreateNewRoamingNetwork(Id:  RoamingNetwork_Id.Parse("TEST_RN1"),
                                                          Name:              I18NString.Create(Languages.deu,  "Test Roaming Netz 1").
                                                                                           Add(Languages.eng,  "Test roaming network 1"));

            var CPO     = RN.CreateChargingStationOperator(ChargingStationOperatorId:  ChargingStationOperator_Id.Parse("DE*GEF"),
                                                   Name:            I18NString.Create(Languages.deu, "GraphDefined"),
                                                   Description:     I18NString.Create(Languages.deu, "GraphDefined EVSE Operator"),
                                                   Configurator:    evseoperator => {
                                                                        evseoperator.AddDataLicense(DataLicense.OpenDatabaseLicense);
                                                                    });

            #region Verify GET /RNs/TEST_RN1/ChargingPools

            var URI = HTTPPath.Parse("/RNs/TEST_RN1/ChargingPools");

            using (var HTTPTask  = HTTPClient.Execute(client => client.GET(URI,
                                                                           requestbuilder => {
                                                                               requestbuilder.Host         = HTTPHostname.Localhost;
                                                                               requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                               requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                           }),
                                                                            RequestTimeout: Timeout,
                                                                            CancellationToken: new CancellationTokenSource().Token))

            {

                HTTPTask.Wait(Timeout);

                using (var HTTPResult = HTTPTask.Result)
                {

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed!");
                    Assert.AreEqual(new JArray().ToString(),
                                    JArray.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'GET " + URI + "'!");

                }

            }

            #endregion

            #region Verify COUNT /RNs/TEST_RN1/ChargingPools

            using (var HTTPTask  = HTTPClient.Execute(client => client.COUNT(URI,
                                                                             requestbuilder => {
                                                                                 requestbuilder.Host         = HTTPHostname.Localhost;
                                                                                 requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                 requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                             }),
                                                                              RequestTimeout: Timeout,
                                                                              CancellationToken: new CancellationTokenSource().Token))

            {

                HTTPTask.Wait(Timeout);

                using (var HTTPResult = HTTPTask.Result)
                {

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed!");
                    Assert.AreEqual(new JObject(new JProperty("count", 0)).ToString(),
                                    JObject.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'COUNT " + URI + "'!");

                }

            }

            #endregion


            var Pool_1  = CPO.CreateChargingPool(ChargingPool_Id.Parse(CPO.Id, "1111"),
                                                 pool => {
                                                     pool.Address = Address.Create(Country.Austria,
                                                                                   "07741",
                                                                                   I18NString.Create(Languages.deu, "Wien"),
                                                                                   "Hofplatz", "17");
                                                 });

            var Pool_2  = CPO.CreateChargingPool(ChargingPool_Id.Parse(CPO.Id, "2222"),
                                                 pool => {
                                                     pool.Address = Address.Create(Country.Germany,
                                                                                   "07749",
                                                                                   I18NString.Create(Languages.deu, "Jena"),
                                                                                   "Biberweg", "18");
                                                 });

            var Pool_3  = CPO.CreateChargingPool(ChargingPool_Id.Parse(CPO.Id, "3333"),
                                                 pool => {
                                                     pool.Address = Address.Create(Country.Belgium,
                                                                                   "07758",
                                                                                   I18NString.Create(Languages.bgn, "Brussels"),
                                                                                   "Avenue", "19");
                                                 });


            #region Verify GET   /RNs/TEST_RN1/ChargingPools

            URI = HTTPPath.Parse("/RNs/TEST_RN1/ChargingPools");

            using (var HTTPTask  = HTTPClient.Execute(client => client.GET(URI,
                                                                           requestbuilder => {
                                                                               requestbuilder.Host         = HTTPHostname.Localhost;
                                                                               requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                               requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                           }),
                                                                            RequestTimeout: Timeout,
                                                                            CancellationToken: new CancellationTokenSource().Token))

            {

                HTTPTask.Wait(Timeout);

                using (var HTTPResult = HTTPTask.Result)
                {

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed!");
                    Assert.AreEqual(new JArray(

                                        new JObject(new JProperty("ChargingPoolId",   "DE*GEF*P1111"),
                                                    new JProperty("OperatorId",       "DE*GEF"),
                                                    new JProperty("Address",
                                                        new JObject(new JProperty("houseNumber",  "17"),
                                                                    new JProperty("street",       "Hofplatz"),
                                                                    new JProperty("postalCode",   "07741"),
                                                                    new JProperty("city",
                                                                        new JObject(new JProperty("de", "Wien"))
                                                                    ),
                                                                    new JProperty("country",
                                                                        new JObject(new JProperty("en", "Austria"))
                                                                    )
                                                   )),
                                                   new JProperty("ChargingStations",  new JArray())
                                                   ),

                                        new JObject(new JProperty("ChargingPoolId",   "DE*GEF*P2222"),
                                                    new JProperty("OperatorId",       "DE*GEF"),
                                                    new JProperty("Address",
                                                        new JObject(new JProperty("houseNumber",  "18"),
                                                                    new JProperty("street",       "Biberweg"),
                                                                    new JProperty("postalCode",   "07749"),
                                                                    new JProperty("city",
                                                                        new JObject(new JProperty("de", "Jena")
                                                                    )),
                                                                    new JProperty("country",
                                                                        new JObject(
                                                                            new JProperty("en",  "Germany"),
                                                                            new JProperty("de",  "Deutschland")
                                                                        )
                                                                    )
                                                   )),
                                                   new JProperty("ChargingStations",  new JArray())
                                                   ),

                                        new JObject(new JProperty("ChargingPoolId",   "DE*GEF*P3333"),
                                                    new JProperty("OperatorId",       "DE*GEF"),
                                                    new JProperty("Address",
                                                        new JObject(new JProperty("houseNumber",  "19"),
                                                                    new JProperty("street",       "Avenue"),
                                                                    new JProperty("postalCode",   "07758"),
                                                                    new JProperty("city",
                                                                        new JObject(new JProperty("be", "Brussels"))
                                                                    ),
                                                                    new JProperty("country",
                                                                        new JObject(new JProperty("en", "Belgium"))
                                                                    )
                                                   )),
                                                   new JProperty("ChargingStations",  new JArray())
                                                   )

                                    ).ToString(),

                                    JArray.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'GET " + URI + "'!");

                }

            }

            #endregion

            #region Verify COUNT /RNs/TEST_RN1/ChargingPools

            using (var HTTPTask = HTTPClient.Execute(client => client.COUNT(URI,
                                                                            requestbuilder => {
                                                                                requestbuilder.Host         = HTTPHostname.Localhost;
                                                                                requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                            }),
                                                                              RequestTimeout: Timeout,
                                                                              CancellationToken: new CancellationTokenSource().Token))

            {

                HTTPTask.Wait(Timeout);

                using (var HTTPResult = HTTPTask.Result)
                {

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed!");
                    Assert.AreEqual(new JObject(new JProperty("count", 3)).ToString(),
                                    JObject.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'COUNT " + URI + "'!");

                }

            }

            #endregion


        }

        #endregion



        #region Minimal_JSONTests()

        public WWCP_Net_JSON_IO_Tests()
        {

            HTTPAPI = new HTTPServer<RoamingNetworks, RoamingNetwork>(
                          TCPPort:            IPPort.Parse(8000),
                          DefaultServerName: "GraphDefined WWCP Unit Tests",
                          DNSClient:          new DNSClient(SearchForIPv6DNSServers: false)
                      );

            HTTPAPI.AttachTCPPort(IPPort.Parse(8001));

            WWCPAPI = WWCP_HTTPAPI.AttachToHTTPAPI(HTTPAPI);
            WWCPAPI.Attach_JSON_IO();

            HTTPAPI.Start();

            //var RN_1    = WWCPAPI.CreateNewRoamingNetwork(RoamingNetworkId:  RoamingNetwork_Id.Parse("TEST_RN1"),
            //                                              Description:       I18NString.Create(Languages.de,  "Test Roaming Netz 1").
            //                                                                               Add(Languages.en,  "Test roaming network 1"));

            //var RN_2    = WWCPAPI.CreateNewRoamingNetwork(Hostname:          HTTPHostname.Parse("virtualhost"),
            //                                              RoamingNetworkId:  RoamingNetwork_Id.Parse("TEST_RN2"),
            //                                              Description:       I18NString.Create(Languages.de,  "Test Roaming Netz 2").
            //                                                                               Add(Languages.en,  "Test roaming network 2"));

            //var CPO_1   = RN_1.CreateNewEVSEOperator(EVSEOperatorId:  EVSEOperator_Id.Parse("DE*GEF"),
            //                                         Name:            I18NString.Create(Languages.de, "GraphDefined"),
            //                                         Description:     I18NString.Create(Languages.de, "GraphDefined Test EVSE Operator"),
            //                                         Configurator:    evseoperator => {
            //                                                              evseoperator.DataLicense = DataLicenses.OpenDatabaseLicense;
            //                                                          });

            //var Pool_1  = CPO_1.CreateNewChargingPool(ChargingPoolId:  ChargingPool_Id.Parse(CPO_1.Id, "1111"),
            //                                          Configurator:    pool => {
            //                                                               pool.Address = new Address(Country.Germany,
            //                                                                                          "07749",
            //                                                                                          I18NString.Create(Languages.de, "Jena"),
            //                                                                                          "Biberweg", "18");
            //                                                           });

            //var Pool_2  = CPO_1.CreateNewChargingPool(ChargingPoolId:  ChargingPool_Id.Parse(CPO_1.Id, "2222"),
            //                                          Configurator:    pool => {
            //                                                               pool.Address = new Address(Country.Germany,
            //                                                                                          "07749",
            //                                                                                          I18NString.Create(Languages.de, "Jena"),
            //                                                                                          "Biberweg", "18");
            //                                                           });

            //var Pool_3  = CPO_1.CreateNewChargingPool(ChargingPoolId:  ChargingPool_Id.Parse(CPO_1.Id, "3333"),
            //                                          Configurator:    pool => {
            //                                                               pool.Address = new Address(Country.Germany,
            //                                                                                          "07749",
            //                                                                                          I18NString.Create(Languages.de, "Jena"),
            //                                                                                          "Biberweg", "18");
            //                                                           });

            //var Sta1_P1  = Pool_1.CreateNewStation(ChargingStationId:  ChargingStation_Id.Parse(CPO_1.Id, "11115678"),
            //                                       Configurator:       station => {
            //                                                           });

        }

        #endregion


        #region Cleanup()

        [TearDown]
        public void Cleanup()
        {
            HTTPAPI.Shutdown();
        }

        #endregion

    }

}
