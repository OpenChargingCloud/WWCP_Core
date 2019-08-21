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

using NUnit.Framework;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.Net.UnitTests
{

    /// <summary>
    /// Roaming Network WWCP API HTTP Unit Tests.
    /// </summary>
    [TestFixture]
    public class RoamingNetworkAPITests : ATests
    {

        #region GET_and_COUNT()

        [Test]
        public void GET_and_COUNT()
        {

            #region Verify GET   /RNs

            var URI = HTTPPath.Parse("/RNs");

            using (var HTTPTask  = _HTTPClient.Execute(client => client.GET(URI,
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

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed! " + HTTPResult.HTTPBody.ToUTF8String());
                    Assert.AreEqual(new JArray().ToString(),
                                    JArray.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'GET " + URI + "'!");

                }

            }

            #endregion

            #region Verify COUNT /RNs

            using (var HTTPTask  = _HTTPClient.Execute(client => client.COUNT(URI,
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

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed! " + HTTPResult.HTTPBody.ToUTF8String());
                    Assert.AreEqual(new JObject(new JProperty("count", 0)).ToString(),
                                    JObject.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'COUNT " + URI + "'!");

                }

            }

            #endregion


            var RN_1  = WWCPAPI.CreateNewRoamingNetwork(Id:  RoamingNetwork_Id.Parse("TEST_RN1"),
                                                        Name:              I18NString.Create(Languages.deu,  "Test Roaming Netz 1").
                                                                                         Add(Languages.eng,  "Test roaming network 1"));

            var RN_2  = WWCPAPI.CreateNewRoamingNetwork(Id:  RoamingNetwork_Id.Parse("TEST_RN2"),
                                                        Name:              I18NString.Create(Languages.deu,  "Test Roaming Netz 2").
                                                                                         Add(Languages.eng,  "Test roaming network 2"));

            var RN_3  = WWCPAPI.CreateNewRoamingNetwork(Id:  RoamingNetwork_Id.Parse("TEST_RN3"),
                                                        Name:              I18NString.Create(Languages.deu,  "Test Roaming Netz 3").
                                                                                         Add(Languages.eng,  "Test roaming network 3"));

            var RN_4  = WWCPAPI.CreateNewRoamingNetwork(Id:  RoamingNetwork_Id.Parse("TEST_RN4"),
                                                        Name:              I18NString.Create(Languages.deu,  "Test Roaming Netz 4").
                                                                                         Add(Languages.eng,  "Test roaming network 4"));


            #region Verify GET   /RNs

            URI = HTTPPath.Parse("/RNs");

            using (var HTTPTask  = _HTTPClient.Execute(client => client.GET(URI,
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

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed! " + HTTPResult.HTTPBody.ToUTF8String());
                    Assert.AreEqual(new JArray(
                                        new JObject(new JProperty("RoamingNetworkId",  "TEST_RN1"),
                                                    new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 1"),
                                                                                                   new JProperty("en", "Test roaming network 1")))),
                                        new JObject(new JProperty("RoamingNetworkId",  "TEST_RN2"),
                                                    new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 2"),
                                                                                                   new JProperty("en", "Test roaming network 2")))),
                                        new JObject(new JProperty("RoamingNetworkId",  "TEST_RN3"),
                                                    new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 3"),
                                                                                                   new JProperty("en", "Test roaming network 3")))),
                                        new JObject(new JProperty("RoamingNetworkId",  "TEST_RN4"),
                                                    new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 4"),
                                                                                                   new JProperty("en", "Test roaming network 4"))))
                                    ).ToString(),
                                    JArray.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'GET " + URI + "'!");

                }

            }

            #endregion

            #region Verify COUNT /RNs

            using (var HTTPTask  = _HTTPClient.Execute(client => client.COUNT(URI,
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

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed! " + HTTPResult.HTTPBody.ToUTF8String());
                    Assert.AreEqual(new JObject(new JProperty("count", 4)).ToString(),
                                    JObject.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'COUNT " + URI + "'!");

                }

            }

            #endregion

            #region Verify GET   /RNs?skip=2

            URI = HTTPPath.Parse("/RNs?skip=2");

            using (var HTTPTask  = _HTTPClient.Execute(client => client.GET(URI,
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

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed! " + HTTPResult.HTTPBody.ToUTF8String());
                    Assert.AreEqual(new JArray(
                                        new JObject(new JProperty("RoamingNetworkId",  "TEST_RN3"),
                                                    new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 3"),
                                                                                                   new JProperty("en", "Test roaming network 3")))),
                                        new JObject(new JProperty("RoamingNetworkId",  "TEST_RN4"),
                                                    new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 4"),
                                                                                                   new JProperty("en", "Test roaming network 4"))))
                                    ).ToString(),
                                    JArray.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'GET " + URI + "'!");

                }

            }

            #endregion

            #region Verify GET   /RNs?take=2

            URI = HTTPPath.Parse("/RNs?take=2");

            using (var HTTPTask  = _HTTPClient.Execute(client => client.GET(URI,
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

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed! " + HTTPResult.HTTPBody.ToUTF8String());
                    Assert.AreEqual(new JArray(
                                        new JObject(new JProperty("RoamingNetworkId",  "TEST_RN1"),
                                                    new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 1"),
                                                                                                   new JProperty("en", "Test roaming network 1")))),
                                        new JObject(new JProperty("RoamingNetworkId",  "TEST_RN2"),
                                                    new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 2"),
                                                                                                   new JProperty("en", "Test roaming network 2"))))
                                    ).ToString(),
                                    JArray.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'GET " + URI + "'!");

                }

            }

            #endregion

            #region Verify GET   /RNs?skip=1&take=2

            URI = HTTPPath.Parse("/RNs?skip=1&take=2");

            using (var HTTPTask  = _HTTPClient.Execute(client => client.GET(URI,
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

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed! " + HTTPResult.HTTPBody.ToUTF8String());
                    Assert.AreEqual(new JArray(
                                        new JObject(new JProperty("RoamingNetworkId",  "TEST_RN2"),
                                                    new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 2"),
                                                                                                   new JProperty("en", "Test roaming network 2")))),
                                        new JObject(new JProperty("RoamingNetworkId",  "TEST_RN3"),
                                                    new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 3"),
                                                                                                   new JProperty("en", "Test roaming network 3"))))
                                    ).ToString(),
                                    JArray.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'GET " + URI + "'!");

                }

            }

            #endregion

            #region Verify GET   /RNs?skip=8

            URI = HTTPPath.Parse("/RNs?skip=8");

            using (var HTTPTask  = _HTTPClient.Execute(client => client.GET(URI,
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

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed! " + HTTPResult.HTTPBody.ToUTF8String());
                    Assert.AreEqual(new JArray().ToString(),
                                    JArray.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'GET " + URI + "'!");

                }

            }

            #endregion


            #region Verify GET   /RNs/TEST_RN1

            URI = HTTPPath.Parse("/RNs/TEST_RN1");

            using (var HTTPTask  = _HTTPClient.Execute(client => client.GET(URI,
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

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode, "'GET " + URI + "' failed! " + HTTPResult.HTTPBody.ToUTF8String());
                    Assert.AreEqual(new JObject(new JProperty("RoamingNetworkId",  "TEST_RN1"),
                                                new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 1"),
                                                                                               new JProperty("en", "Test roaming network 1")))
                                    ).ToString(),
                                    JObject.Parse(HTTPResult.HTTPBody.ToUTF8String()).ToString(),
                                    "Invalid response for 'GET " + URI + "'!");

                }

            }

            #endregion

        }

        #endregion


        #region Test_Multitenancy_and_RoamingNetworks()

        [Test]
        public void Test_Multitenancy_and_RoamingNetworks()
        {

            #region Verify GET /RNs on localhost

            var task0001  = _HTTPClient.Execute(client => client.GET(HTTPPath.Parse("/RNs"),
                                                                    requestbuilder => {
                                                                        requestbuilder.Host         = HTTPHostname.Localhost;
                                                                        requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                        requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                    }),
                                                                    RequestTimeout:    Timeout,
                                                                    CancellationToken: new CancellationTokenSource().Token);

            task0001.Wait(Timeout);
            var result0001 = task0001.Result;

            Assert.AreEqual(HTTPStatusCode.OK, result0001.HTTPStatusCode);
            Assert.AreEqual(new JArray(new JObject(new JProperty("RoamingNetworkId",  "TEST_RN1"),
                                                   new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 1"),
                                                                                                  new JProperty("en", "Test roaming network 1"))))).ToString(),
                            JArray.Parse(result0001.HTTPBody.ToUTF8String()).ToString());

            #endregion

            #region Verify GET /RNs on virtualhost

            var task0002  = _HTTPClient.Execute(client => client.GET(HTTPPath.Parse("/RNs"),
                                                                    requestbuilder => {
                                                                        requestbuilder.Host         = HTTPHostname.Localhost;
                                                                        requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                        requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                    }),
                                                                    RequestTimeout:    Timeout,
                                                                    CancellationToken: new CancellationTokenSource().Token);

            task0002.Wait(Timeout);
            var result0002 = task0002.Result;

            Assert.AreEqual(HTTPStatusCode.OK, result0002.HTTPStatusCode);
            Assert.AreEqual(new JArray(new JObject(new JProperty("RoamingNetworkId",  "TEST_RN1"),
                                                   new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 1"),
                                                                                                  new JProperty("en", "Test roaming network 1")))),
                                       new JObject(new JProperty("RoamingNetworkId",  "TEST_RN2"),
                                                   new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 2"),
                                                                                                  new JProperty("en", "Test roaming network 2"))))).ToString(),
                            JArray.Parse(result0002.HTTPBody.ToUTF8String()).ToString());

            #endregion

            #region Verify GET /RNs/TEST_RN1 on localhost

            var task0003  = _HTTPClient.Execute(client => client.GET(HTTPPath.Parse("/RNs/TEST_RN1"),
                                                                    requestbuilder => {
                                                                        requestbuilder.Host         = HTTPHostname.Localhost;
                                                                        requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                        requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                    }),
                                                                    RequestTimeout:    Timeout,
                                                                    CancellationToken: new CancellationTokenSource().Token);

            task0003.Wait(Timeout);
            var result0003 = task0003.Result;

            Assert.AreEqual(HTTPStatusCode.OK, result0003.HTTPStatusCode);
            Assert.AreEqual(new JObject(new JProperty("RoamingNetworkId",  "TEST_RN1"),
                                        new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 1"),
                                                                                       new JProperty("en", "Test roaming network 1")))).ToString(),
                            JObject.Parse(result0003.HTTPBody.ToUTF8String()).ToString());

            #endregion

            #region Verify GET /RNs/TEST_RN1 on virtualhost

            var task0004  = _HTTPClient.Execute(client => client.GET(HTTPPath.Parse("/RNs/TEST_RN1"),
                                                                    requestbuilder => {
                                                                        requestbuilder.Host         = HTTPHostname.Localhost;
                                                                        requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                        requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                    }),
                                                                    RequestTimeout:    Timeout,
                                                                    CancellationToken: new CancellationTokenSource().Token);

            task0004.Wait(Timeout);
            var result0004 = task0004.Result;

            Assert.AreEqual(HTTPStatusCode.OK, result0004.HTTPStatusCode);
            Assert.AreEqual(new JObject(new JProperty("RoamingNetworkId",  "TEST_RN1"),
                                        new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 1"),
                                                                                       new JProperty("en", "Test roaming network 1")))).ToString(),
                            JObject.Parse(result0004.HTTPBody.ToUTF8String()).ToString());

            #endregion

            #region Verify GET /RNs/TEST_RN2 on virtualhost

            var task0005  = _HTTPClient.Execute(client => client.GET(HTTPPath.Parse("/RNs/TEST_RN2"),
                                                                    requestbuilder => {
                                                                        requestbuilder.Host         = HTTPHostname.Localhost;
                                                                        requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                        requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                    }),
                                                                    RequestTimeout:    Timeout,
                                                                    CancellationToken: new CancellationTokenSource().Token);

            task0005.Wait(Timeout);
            var result0005 = task0005.Result;

            Assert.AreEqual(HTTPStatusCode.OK, result0005.HTTPStatusCode);
            Assert.AreEqual(new JObject(new JProperty("RoamingNetworkId",  "TEST_RN2"),
                                        new JProperty("description",       new JObject(new JProperty("de", "Test Roaming Netz 2"),
                                                                                       new JProperty("en", "Test roaming network 2")))).ToString(),
                            JObject.Parse(result0005.HTTPBody.ToUTF8String()).ToString());

            #endregion


        }

        #endregion


    }

}
