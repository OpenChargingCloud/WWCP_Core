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

using cloud.charging.open.protocols.WWCP.UnitTests.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking.tests
{

    /// <summary>
    /// Which networking node the overlay WebSocket server takes a connection
    /// for: the one named by the last segment of its path - and not with the
    /// credentials of another one, unless that one may connect as it.
    /// </summary>
    /// <remarks>
    /// The credentials here are HTTP Basic logins. A client certificate names
    /// its networking node by its common name, and is checked the same way; no
    /// test here sets up TLS for it. The overlay server registers a networking
    /// node once the 101 has gone out, so a test that expects a registration
    /// waits for it.
    /// </remarks>
    [TestFixture]
    public class IdentityTests
    {

        #region Data

        private static readonly  NetworkingNode_Id        CS001     = NetworkingNode_Id.Parse("CS001");
        private static readonly  NetworkingNode_Id        CS002     = NetworkingNode_Id.Parse("CS002");
        private static readonly  NetworkingNode_Id        CS003     = NetworkingNode_Id.Parse("CS003");
        private static readonly  NetworkingNode_Id        LC001     = NetworkingNode_Id.Parse("LC001");

        private readonly         String                   password  = $"pw-{Guid.NewGuid():N}";

        private                  OverlayWebSocketServer?  server;
        private                  IPPort                   port;

        #endregion

        #region TearDown()

        [TearDown]
        public async Task TearDown()
        {

            if (server is not null)
                await server.Shutdown();

            server = null;

        }

        #endregion


        #region ANodeIsWhoItsPathAndItsCredentialsSay()

        [Test]
        public async Task ANodeIsWhoItsPathAndItsCredentialsSay()
        {

            Start(RequireAuthentication: true);

            using var client      = await RawWebSocketClient.Upgrade(port, $"/{CS001}", Basic(CS001));
            var       registered  = await RegisteredAs(CS001);

            Assert.Multiple(() => {
                Assert.That(client.StatusCode, Is.EqualTo(101));
                Assert.That(registered,        Is.True,
                            "The node was not registered as the one its path and its credentials name.");
            });

        }

        #endregion

        #region ANodesPasswordOpensNoConnectionAsAnotherNode()

        /// <summary>
        /// CS002, with its right password, asks to be let in as CS001.
        /// </summary>
        [Test]
        public async Task ANodesPasswordOpensNoConnectionAsAnotherNode()
        {

            Start(RequireAuthentication: true);

            using var client = await RawWebSocketClient.Upgrade(port, $"/{CS001}", Basic(CS002));

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion

        #region ALocalControllerConnectsAsANodeItMayActFor()

        /// <summary>
        /// A local controller connects for a node behind it with the node's path
        /// and its own credentials (OCPP 2.1 Part 4, 6.2 and 6.5). The connection
        /// is one to the node, opened by the local controller.
        /// </summary>
        [Test]
        public async Task ALocalControllerConnectsAsANodeItMayActFor()
        {

            Start(RequireAuthentication: true);

            server!.AllowToActFor(LC001, [ CS001 ]);

            using var client = await RawWebSocketClient.Upgrade(port, $"/{CS001}", Basic(LC001));

            var registered = await RegisteredAs(CS001);
            var connection = server.WebSocketConnections.SingleOrDefault(connection => connection.RemoteSocket.Port.ToUInt16() == client.LocalPort);

            Assert.Multiple(() => {
                Assert.That(client.StatusCode, Is.EqualTo(101));
                Assert.That(registered,        Is.True,
                            $"The connection was registered as {server.ConnectedNetworkingNodeIds.AggregateWith(", ")}, and not as the node of its path.");
                Assert.That(connection?.TryGetCustomDataAs<NetworkingNode_Id>(AOverlayWebSocketServer.actingNetworkingNodeId_WebSocketKey), Is.EqualTo(LC001),
                            "The connection does not say that the local controller opened it.");
            });

        }

        #endregion

        #region ALocalControllerConnectsAsNoNodeItMayNotActFor()

        [Test]
        public async Task ALocalControllerConnectsAsNoNodeItMayNotActFor()
        {

            Start(RequireAuthentication: true);

            server!.AllowToActFor(LC001, [ CS001 ]);

            using var client = await RawWebSocketClient.Upgrade(port, $"/{CS002}", Basic(LC001));

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion

        #region ALocalControllerThatMayNoLongerActForANodeConnectsAsItNoMore()

        [Test]
        public async Task ALocalControllerThatMayNoLongerActForANodeConnectsAsItNoMore()
        {

            Start(RequireAuthentication: true);

            server!.AllowToActFor   (LC001, [ CS001, CS002 ]);
            server. DisallowToActFor(LC001, [ CS001 ]);

            using var client = await RawWebSocketClient.Upgrade(port, $"/{CS001}", Basic(LC001));

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server.ConnectedNetworkingNodeIds, Is.Empty);
                Assert.That(server.MayActFor(LC001, CS002),    Is.True);
            });

        }

        #endregion

        #region ANodeWithoutANameInItsPathIsWhoItsCredentialsSay()

        /// <summary>
        /// A path without a name in its last segment names no networking node,
        /// and the credentials do - as for a local controller whose URL is the
        /// endpoint alone.
        /// </summary>
        [Test]
        public async Task ANodeWithoutANameInItsPathIsWhoItsCredentialsSay()
        {

            Start(RequireAuthentication: true);

            using var client      = await RawWebSocketClient.Upgrade(port, "/", Basic(LC001));
            var       registered  = await RegisteredAs(LC001);

            Assert.Multiple(() => {
                Assert.That(client.StatusCode, Is.EqualTo(101));
                Assert.That(registered,        Is.True,
                            "The node was not registered as the one its credentials name.");
            });

        }

        #endregion

        #region WrongCredentialsAreUnauthorizedWhetherOrNotTheyMayActFor()

        /// <summary>
        /// Wrong credentials are answered alike, whichever networking node they
        /// ask to connect as: told apart, the answers would say to anybody which
        /// networking node may connect as which.
        /// </summary>
        [Test]
        public async Task WrongCredentialsAreUnauthorizedWhetherOrNotTheyMayActFor()
        {

            Start(RequireAuthentication: true);

            server!.AllowToActFor(LC001, [ CS001 ]);

            var wrong = HTTPBasicAuthentication.Create(LC001.ToString(), "wrong").HTTPText;

            using var mayActFor     = await RawWebSocketClient.Upgrade(port, $"/{CS001}", wrong);
            using var mayNotActFor  = await RawWebSocketClient.Upgrade(port, $"/{CS002}", wrong);

            Assert.Multiple(() => {
                Assert.That(mayActFor.   StatusCode, Is.EqualTo(401));
                Assert.That(mayNotActFor.StatusCode, Is.EqualTo(401));
            });

        }

        #endregion

        #region AKnownPasswordOpensNoConnectionAsAnotherNodeWithoutRequiredAuthentication()

        /// <summary>
        /// A server that does not require authentication still knows a right
        /// password of its NetworkingNodeLogins when it sees one.
        /// </summary>
        [Test]
        public async Task AKnownPasswordOpensNoConnectionAsAnotherNodeWithoutRequiredAuthentication()
        {

            Start(RequireAuthentication: false);

            using var client = await RawWebSocketClient.Upgrade(port, $"/{CS001}", Basic(CS002));

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion

        #region CredentialsTheServerCannotCheckOpenNoConnectionAsAnotherNode()

        /// <summary>
        /// A login this server does not know, for another node than the one of
        /// the path, without RequireAuthentication. The server cannot refuse it
        /// as forbidden without telling apart right and wrong credentials; the
        /// connection it lets in is no connection to either, and is closed.
        /// </summary>
        [Test]
        public async Task CredentialsTheServerCannotCheckOpenNoConnectionAsAnotherNode()
        {

            Start(RequireAuthentication: false);

            using var client  = await RawWebSocketClient.Upgrade(port, $"/{CS001}", Basic(CS003));
            var       closed  = await Until(() => !server!.WebSocketConnections.Any(connection => connection.RemoteSocket.Port.ToUInt16() == client.LocalPort));

            Assert.Multiple(() => {
                Assert.That(closed,                            Is.True,
                            "The connection was not closed.");
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion


        #region (private) Start(RequireAuthentication)

        private void Start(Boolean RequireAuthentication)
        {

            port    = TestPorts.Free();

            server  = new OverlayWebSocketServer(
                          NetworkingNode_Id.Parse("onn"),
                          [ "ocpp2.1" ],
                          TCPPort:                port,
                          RequireAuthentication:  RequireAuthentication,
                          DisableWebSocketPings:  true
                      );

            server.AddOrUpdateHTTPBasicAuth(CS001, password);
            server.AddOrUpdateHTTPBasicAuth(CS002, password);
            server.AddOrUpdateHTTPBasicAuth(LC001, password);

            server.Start();

        }

        #endregion

        #region (private) Basic(NetworkingNodeId)

        /// <summary>
        /// The right HTTP Basic credentials of the given networking node.
        /// </summary>
        private String Basic(NetworkingNode_Id NetworkingNodeId)
            => HTTPBasicAuthentication.Create(NetworkingNodeId.ToString(), password).HTTPText;

        #endregion

        #region (private) RegisteredAs(NetworkingNodeId)

        /// <summary>
        /// Whether the server's registered networking nodes become exactly the
        /// given one within two seconds.
        /// </summary>
        private Task<Boolean> RegisteredAs(NetworkingNode_Id NetworkingNodeId)

            => Until(() => server!.ConnectedNetworkingNodeIds.SequenceEqual([ NetworkingNodeId ]));

        #endregion

        #region (private static) Until(Condition)

        /// <summary>
        /// Whether the given condition holds within two seconds.
        /// </summary>
        private static async Task<Boolean> Until(Func<Boolean> Condition)
        {

            var deadline = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(2);

            while (!Condition())
            {

                if (DateTimeOffset.UtcNow > deadline)
                    return false;

                await Task.Delay(TimeSpan.FromMilliseconds(20));

            }

            return true;

        }

        #endregion

    }

}
