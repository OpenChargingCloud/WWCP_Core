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
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using org.GraphDefined.WWCP.Net.IO.JSON;
using org.GraphDefined.WWCP.Net.IO.GeoJSON;

#endregion

namespace org.GraphDefined.WWCP.Net.UnitTests
{

    /// <summary>
    /// Abstract Unit Tests.
    /// </summary>
    public abstract class ATests
    {

        #region Data

        protected readonly IPv4Address                                  RemoteAddress = IPv4Address.Localhost;
        //protected readonly IPv4Address                                  RemoteAddress = IPv4Address.Parse("80.148.29.35");
        //protected readonly IPv4Address                                  RemoteAddress = IPv4Address.Parse("138.201.28.98");
        //protected readonly IPPort                                       RemotePort    = IPPort.Parse(8000);
        protected readonly IPPort                                       RemotePort    = IPPort.Parse(8001);

        protected          HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPAPI;
        protected          WWCP_HTTPAPI                                 WWCPAPI;
        protected readonly TimeSpan                                     Timeout  = TimeSpan.FromSeconds(20);

        protected          DNSClient                                    _DNSClient;
        protected          HTTPClient                                   _HTTPClient;

        #endregion

        #region ATests()

        protected ATests()
        {

            _DNSClient = new DNSClient(SearchForIPv6DNSServers: false);

            if (RemoteAddress == IPv4Address.Localhost)
            {

                HTTPAPI = new HTTPServer<RoamingNetworks, RoamingNetwork>(
                              TCPPort:            RemotePort,
                              DefaultServerName:  "GraphDefined WWCP Unit Tests",
                              DNSClient:          _DNSClient
                          );

                //HTTPAPI.AttachTCPPort(IPPort.Parse(8001));

                WWCPAPI = WWCP_HTTPAPI.AttachToHTTPAPI(HTTPAPI);
                WWCPAPI.Attach_JSON_IO();
                //WWCPAPI.Attach_GeoJSON_IO();

                HTTPAPI.Start();

            }

        }

        #endregion


        #region Init()

        [OneTimeSetUp]
        public void Init()
        {

            _HTTPClient = new HTTPClient(RemoteAddress,
                              RemotePort: RemotePort,
                              DNSClient:  _DNSClient);

        }

        #endregion


        #region Cleanup()

        [TearDown]
        public void Cleanup()
        {

            var      URI                = HTTPPath.Parse("/RNs");
            String[] RoamingNetworkIds  = null;

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

                    Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode);

                    RoamingNetworkIds = JArray.Parse(HTTPResult.HTTPBody.ToUTF8String()).
                                               AsEnumerable().
                                               Select(v => (v as JObject)["RoamingNetworkId"].Value<String>()).
                                               ToArray();

                }

            }


            foreach (var RoamingNetworkId in RoamingNetworkIds)
            {

                URI = HTTPPath.Parse("/RNs/" + RoamingNetworkId);

                using (var HTTPTask  = _HTTPClient.Execute(client => client.DELETE(URI,
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

                        Assert.AreEqual(HTTPStatusCode.OK, HTTPResult.HTTPStatusCode);

                    }

                }

            }



            if (RemoteAddress == IPv4Address.Localhost)
                HTTPAPI.Shutdown();

        }

        #endregion

    }

}
