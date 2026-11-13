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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// A networking node that dials with a one-time password is let in by a
    /// networking node that knows it.
    /// </summary>
    /// <remarks>
    /// The whole way through: the node's ConnectWebSocketClient, the
    /// WWCPWebSocketClient it makes, Hermod's client underneath - and on the
    /// other side WWCPWebSocketServer, which checks the login against the path
    /// and the password against its ClientTOTPConfig. The login used to stop
    /// at the first of these: there was nowhere to hand it over, so the
    /// password went out without one, which is to say not at all.
    /// </remarks>
    [TestFixture]
    public class OneTimePasswordTests
    {

        #region (class) Node

        private sealed class Node(String Id) : AWWCPNetworkingNode(NetworkingNode_Id.Parse(Id))
        { }

        #endregion

        #region Data

        private readonly  String                sharedSecret  = $"totp-{Guid.NewGuid():N}";

        private           WWCPWebSocketServer?  server;
        private           IPPort                port;

        #endregion

        #region SetUp / TearDown

        [SetUp]
        public async Task StartAServerThatKnowsCS001()
        {

            port    = TestPorts.Free();

            server  = new WWCPWebSocketServer(
                          new Node("csms"),
                          HTTPPort:               port,
                          RequireAuthentication:  true,
                          SecWebSocketProtocols:  [ "ocpp2.1" ],
                          DisableWebSocketPings:  true
                      );

            server.ClientTOTPConfig["CS001"] = new TOTPConfig(sharedSecret);

            await server.Start();

        }

        [TearDown]
        public async Task TearDown()
        {

            if (server is not null)
            {
                await server.Shutdown();
                await server.DisposeAsync();
            }

            server = null;

        }

        #endregion


        #region ANodeWithAOneTimePasswordIsLetIn()

        [Test]
        public async Task ANodeWithAOneTimePasswordIsLetIn()
        {

            var node      = new Node("CS001");

            var response  = await node.ConnectWebSocketClient(
                                      RemoteURL:              URL.Parse($"ws://127.0.0.1:{port}/CS001"),
                                      SecWebSocketProtocols:  [ "ocpp2.1" ],
                                      DisableWebSocketPings:  true,
                                      TOTPConfig:             new TOTPConfig(sharedSecret),
                                      TOTPLogin:              "CS001"
                                  );

            Assert.That(response.HTTPStatusCode.Code, Is.EqualTo(101),
                        "A node with a one-time password the server knows was not let in.");

        }

        #endregion

        #region ANodeWithAOneTimePasswordAndNoLoginIsLetInAsWhoItsPathNames()

        /// <summary>
        /// Without a login the password goes in the "TOTP" request header,
        /// which names nobody: it is verified as the one of the networking
        /// node the path names, and of nobody else.
        /// </summary>
        [Test]
        public async Task ANodeWithAOneTimePasswordAndNoLoginIsLetInAsWhoItsPathNames()
        {

            var asItself   = await new Node("CS001").ConnectWebSocketClient(
                                       RemoteURL:              URL.Parse($"ws://127.0.0.1:{port}/CS001"),
                                       SecWebSocketProtocols:  [ "ocpp2.1" ],
                                       DisableWebSocketPings:  true,
                                       TOTPConfig:             new TOTPConfig(sharedSecret)
                                   );

            var asAnother  = await new Node("CS002").ConnectWebSocketClient(
                                       RemoteURL:              URL.Parse($"ws://127.0.0.1:{port}/CS002"),
                                       SecWebSocketProtocols:  [ "ocpp2.1" ],
                                       DisableWebSocketPings:  true,
                                       TOTPConfig:             new TOTPConfig(sharedSecret)
                                   );

            var mistaken   = await new Node("CS001").ConnectWebSocketClient(
                                       RemoteURL:              URL.Parse($"ws://127.0.0.1:{port}/CS001"),
                                       SecWebSocketProtocols:  [ "ocpp2.1" ],
                                       DisableWebSocketPings:  true,
                                       TOTPConfig:             new TOTPConfig(sharedSecret + "-not")
                                   );

            Assert.Multiple(() => {
                Assert.That(asItself. HTTPStatusCode.Code,  Is.EqualTo(101),      "A one-time password without a login was not taken for the one of the path.");
                Assert.That(asAnother.HTTPStatusCode.Code,  Is.EqualTo(401),      "A one-time password of CS001 opened a connection as CS002.");
                Assert.That(mistaken. HTTPStatusCode.Code,  Is.EqualTo(401),      "A one-time password of another secret was taken.");
                Assert.That(server!.WebSocketConnections.Count(), Is.EqualTo(1),  "Not the one connection that was let in is there.");
            });

        }

        #endregion

        #region ANodeWithAnotherOnesLoginIsNotLetIn()

        /// <summary>
        /// The login is checked against the path, as for a password: a right
        /// one-time password of CS001 opens no connection as CS002.
        /// </summary>
        [Test]
        public async Task ANodeWithAnotherOnesLoginIsNotLetIn()
        {

            var node      = new Node("CS002");

            var response  = await node.ConnectWebSocketClient(
                                      RemoteURL:              URL.Parse($"ws://127.0.0.1:{port}/CS002"),
                                      SecWebSocketProtocols:  [ "ocpp2.1" ],
                                      DisableWebSocketPings:  true,
                                      TOTPConfig:             new TOTPConfig(sharedSecret),
                                      TOTPLogin:              "CS001"
                                  );

            Assert.That(response.HTTPStatusCode.Code, Is.Not.EqualTo(101),
                        "A one-time password of CS001 opened a connection as CS002.");

        }

        #endregion

    }

}
