/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OCPP <https://github.com/OpenChargingCloud/WWCP_OCPP>
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

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using NUnit.Framework.Legacy;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking.tests
{

    /// <summary>
    /// Overlay Networking tests.
    /// </summary>
    public class OverlayNetworkingSetupTests
    {

        #region BasicConnectionSetup_Test1()

        /// <summary>
        /// A test for a basic connection setup (Test 1).
        /// </summary>
        [Test]
        public async Task BasicConnectionSetup_Test1()
        {

            var onn01         = new TestOverlayNetworkingNode(
                                    NetworkingNode_Id.Parse ("ONN-01"),
                                    I18NString.       Create("Overlay Networking Node #1")
                                );

            var onn02         = new TestOverlayNetworkingNode(
                                    NetworkingNode_Id.Parse ("ONN-02"),
                                    I18NString.       Create("Overlay Networking Node #2")
                                );

            var server1       = onn01.AttachWebSocketServer(
                                    SupportedEEBusWebSocketSubprotocols:   [ "ocpp2.0.1" ],
                                    TCPPort:                               IPPort.Parse(4599),
                                    RequireAuthentication:                 false,
                                    DisableWebSocketPings:                 true,
                                    AutoStart:                             true
                                );

            //server1.AddOrUpdateHTTPBasicAuth(onn01.Id, "1234abcd");


            //onn01.WebSocketServers.First().OnTextMessage += async (timestamp,
            //                                                       server,
            //                                                       connection,
            //                                                       eventTrackingId,
            //                                                       requestTimestamp,
            //                                                       textMessage,
            //                                                       cancellationToken) => {

            //    return new org.GraphDefined.Vanaheimr.Hermod.WebSocket.WebSocketTextMessageResponse(
            //               timestamp,
            //               textMessage,
            //               Timestamp.Now,
            //               "!!!",
            //               eventTrackingId
            //           );

            //};

            var jsonMessages = new List<JArray>();

            onn01.WebSocketServers.First().OnJSONMessageRequestReceived += (timestamp,
                                                                            server,
                                                                            connection,
                                                                            destinationNodeId,
                                                                            networkPath,
                                                                            eventTrackingId,
                                                                            requestTimestamp,
                                                                            jsonMessage,
                                                                            cancellationToken) => {

                jsonMessages.Add(jsonMessage);

                return Task.CompletedTask;

            };


            #region Connect test

            var connectionSetupResponse1 = await onn02.ConnectWebSocketClient(
                                               NetworkingNodeId:        NetworkingNode_Id.CSMS,
                                               RemoteURL:               URL.Parse("http://127.0.0.1:4599"),
                                               HTTPAuthentication:      HTTPBasicAuthentication.Create(onn01.Id.ToString(), "1234abcd"),
                                               SecWebSocketProtocols:   [ "ocpp2.0.1" ],
                                               DisableWebSocketPings:   true
                                            //   NetworkingMode:          OCPP.WebSockets.NetworkingMode.OverlayNetwork
                                           );

            // GET / HTTP/1.1
            // Host:                    127.0.0.1:4599
            // Connection:              Upgrade
            // Upgrade:                 websocket
            // Sec-WebSocket-Key:       HrjJcWt28BWmVQJx3LhmTw==
            // Sec-WebSocket-Version:   13
            // Authorization:           Basic T05OLTAxOjEyMzRhYmNk

            Assert.That(connectionSetupResponse1, Is.Not.Null);

            // HTTP/1.1 101 Switching Protocols
            // Date:                    Mon, 02 Apr 2023 15:55:18 GMT
            // Server:                  GraphDefined OCPP v2.0.1 HTTP/WebSocket/JSON CSMS API
            // Connection:              Upgrade
            // Upgrade:                 websocket
            // Sec-WebSocket-Accept:    HSmrc0sMlYUkAGmm5OPpG2HaGWk=
            // Sec-WebSocket-Protocol:  ocpp2.0.1
            // Sec-WebSocket-Version:   13

            ClassicAssert.AreEqual(HTTPStatusCode.SwitchingProtocols,                                    connectionSetupResponse1.HTTPStatusCode);
            ClassicAssert.AreEqual($"GraphDefined HTTP Web Socket Service v2.0",                         connectionSetupResponse1.Server);
            ClassicAssert.AreEqual("Upgrade",                                                            connectionSetupResponse1.Connection);
            ClassicAssert.AreEqual("websocket",                                                          connectionSetupResponse1.Upgrade);
            ClassicAssert.IsTrue  (connectionSetupResponse1.SecWebSocketProtocol.Contains("ocpp2.0.1"));
            ClassicAssert.AreEqual("13",                                                                 connectionSetupResponse1.SecWebSocketVersion);

            #endregion


            //var r1 = await onn02.WebSocketClients.First().SendTextMessage("[\"Hello world!\"]", EventTracking_Id.New);
            var r2 = await onn02.WebSocketClients.First().SendRequest(onn01.Id, "aaa", Request_Id.NewRandom(), new JObject(new JProperty("Hello", "world!")));

            await Task.Delay(500);

            Assert.That(jsonMessages.Count, Is.EqualTo(1));

        }

        #endregion



    }

}
