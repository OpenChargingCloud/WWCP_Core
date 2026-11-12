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
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.WWCP.WebSockets;
using cloud.charging.open.protocols.WWCP.UnitTests.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.NetworkingNode
{

    /// <summary>
    /// A client the node lets go of with DisconnectClient is no longer the
    /// node's: not among its clients, not a route, and not connected again.
    /// </summary>
    /// <remarks>
    /// A client was the node's from ConnectWebSocketClient on, for as long as
    /// the node lived: closed, it stayed among WWCPWebSocketClients. A charging
    /// station that dials one back end after another, while none of them can be
    /// reached, gathered a client for every one it dialled.
    /// </remarks>
    [TestFixture]
    public class DisconnectClientTests
    {

        #region (class) Node

        private sealed class Node : AWWCPNetworkingNode
        {
            public Node()
                : base(NetworkingNode_Id.Parse("disconnecting"))
            { }
        }

        #endregion

        #region Data

        private static readonly NetworkingNode_Id  A  = NetworkingNode_Id.Parse("back-end-a");
        private static readonly NetworkingNode_Id  B  = NetworkingNode_Id.Parse("back-end-b");

        private static WebSocketClientReconnectPolicy Quickly
            => new (InitialDelay: TimeSpan.FromMilliseconds(100),
                    MaxDelay:     TimeSpan.FromMilliseconds(200));

        private readonly List<WebSocketServer>       servers  = [];
        private readonly List<WWCPWebSocketClient>   clients  = [];

        #endregion

        #region TearDown()

        [TearDown]
        public async Task TearDown()
        {

            foreach (var client in clients)
            {
                client.ReconnectPolicy = null;
                await client.Close();
            }

            foreach (var server in servers)
                await server.Shutdown();

            clients.Clear();
            servers.Clear();

        }

        #endregion


        #region (private) AServer(Upgraded = null)

        /// <summary>
        /// A server that lets anybody in, and counts who came.
        /// </summary>
        private async Task<WebSocketServer> AServer(Action? Upgraded = null)
        {

            var server = new WebSocketServer(HTTPPort:               TestPorts.Free(),
                                             RequireAuthentication:  false);

            server.OnNewWebSocketConnection += (_, _, _, _, _, _, _) => {
                Upgraded?.Invoke();
                return Task.CompletedTask;
            };

            await server.Start();

            servers.Add(server);

            return server;

        }

        #endregion

        #region (private) Connect(Node, Server, NextHop)

        private async Task<WWCPWebSocketClient> Connect(Node               Node,
                                                        WebSocketServer    Server,
                                                        NetworkingNode_Id  NextHop)
        {

            var client    = new WWCPWebSocketClient(Node,
                                                    URL.Parse($"ws://127.0.0.1:{Server.IPPort}"),
                                                    DisableWebSocketPings: true);

            clients.Add(client);

            var response  = await Node.ConnectWebSocketClient(client, NextHop);

            Assert.That(response.HTTPStatusCode.Code, Is.EqualTo(101),
                        $"The node could not connect to {Server.IPPort}.");

            return client;

        }

        #endregion


        #region ADisconnectedClientIsNoLongerTheNodes()

        /// <summary>
        /// The client let go of is gone from the node's clients and from its
        /// routes; the other one stays in both.
        /// </summary>
        [Test]
        public async Task ADisconnectedClientIsNoLongerTheNodes()
        {

            var node     = new Node();
            var clientA  = await Connect(node, await AServer(), A);
            var clientB  = await Connect(node, await AServer(), B);

            Assert.That(node.WWCPWebSocketClients, Is.EquivalentTo(new[] { clientA, clientB }));

            var wasOne   = await node.DisconnectClient(clientA, "Dialling another one.");

            Assert.Multiple(() => {

                Assert.That(wasOne,                                                 Is.True);
                Assert.That(node.WWCPWebSocketClients,                              Is.EqualTo(new[] { clientB }));
                Assert.That(node.WWCPClients,                                       Is.EqualTo(new[] { clientB }));
                Assert.That(node.Routing.AllClients,                                Does.Not.Contain(clientA));
                Assert.That(node.Routing.LookupNetworkingNode(A, out _),            Is.False,
                            "The route through the client let go of was still there.");

                Assert.That(node.Routing.LookupNetworkingNode(B, out var toB),      Is.True,
                            "The route through the other client went too.");
                Assert.That(toB?.Client,                                            Is.SameAs(clientB));

            });

        }

        #endregion

        #region ADisconnectedClientConnectsNoMore()

        /// <summary>
        /// A client that would come back after a close - it has a reconnect
        /// policy - does not come back once the node let go of it.
        /// </summary>
        [Test]
        public async Task ADisconnectedClientConnectsNoMore()
        {

            var upgrades  = 0;
            var node      = new Node { ReconnectPolicy = Quickly };
            var server    = await AServer(() => Interlocked.Increment(ref upgrades));
            var client    = await Connect(node, server, A);

            Assert.That(client.ReconnectPolicy, Is.Not.Null, "The client was given no policy: what is asked here is not asked.");

            await node.DisconnectClient(client);

            var before    = Volatile.Read(ref upgrades);

            await Task.Delay(TimeSpan.FromSeconds(1));

            Assert.Multiple(() => {
                Assert.That(Volatile.Read(ref upgrades),                            Is.EqualTo(before),
                            "The client the node let go of connected again.");
                Assert.That(node.Routing.LookupNetworkingNode(A, out _),            Is.False);
                Assert.That(node.WWCPWebSocketClients,                              Is.Empty);
            });

        }

        #endregion

        #region AClientThatNeverGotThroughTriesNoMore()

        /// <summary>
        /// A client whose first attempt failed goes on trying in the background,
        /// as its policy says; let go of, it tries no more: a server that comes
        /// up at its address afterwards is not reached by it.
        /// </summary>
        [Test]
        public async Task AClientThatNeverGotThroughTriesNoMore()
        {

            var node      = new Node { ReconnectPolicy = Quickly };
            var port      = TestPorts.Free();

            var client    = new WWCPWebSocketClient(node,
                                                    URL.Parse($"ws://127.0.0.1:{port}"),
                                                    DisableWebSocketPings: true);

            clients.Add(client);

            var response  = await node.ConnectWebSocketClient(client, A);

            Assert.That(response.HTTPStatusCode.Code, Is.Not.EqualTo(101), $"Something answered at {port} already.");

            await node.DisconnectClient(client);

            var upgrades  = 0;
            var server    = new WebSocketServer(HTTPPort:               port,
                                                RequireAuthentication:  false);

            server.OnNewWebSocketConnection += (_, _, _, _, _, _, _) => {
                Interlocked.Increment(ref upgrades);
                return Task.CompletedTask;
            };

            await server.Start();

            servers.Add(server);

            await Task.Delay(TimeSpan.FromSeconds(1));

            Assert.Multiple(() => {
                Assert.That(Volatile.Read(ref upgrades),                            Is.Zero,
                            "The client the node let go of got through after all.");
                Assert.That(node.WWCPWebSocketClients,                              Is.Empty);
            });

        }

        #endregion

        #region DialledAgainAndAgainTheNodeKeepsOneClient()

        /// <summary>
        /// Dialled over and over, each client let go of before the next is
        /// dialled, the node keeps the last one only.
        /// </summary>
        [Test]
        public async Task DialledAgainAndAgainTheNodeKeepsOneClient()
        {

            var node    = new Node();
            var server  = await AServer();

            WWCPWebSocketClient? last = null;

            for (var i = 0; i < 5; i++)
            {

                if (last is not null)
                    await node.DisconnectClient(last);

                last = await Connect(node, server, A);

            }

            Assert.That(node.WWCPWebSocketClients, Is.EqualTo(new[] { last }));

        }

        #endregion

        #region AClientNotTheNodesIsClosedAllTheSame()

        /// <summary>
        /// A client the node never connected is not one of its own: said so,
        /// and closed all the same, and nothing of the node's goes with it.
        /// </summary>
        [Test]
        public async Task AClientNotTheNodesIsClosedAllTheSame()
        {

            var node     = new Node();
            var mine     = await Connect(node, await AServer(), A);

            var stranger = new WWCPWebSocketClient(node,
                                                   URL.Parse("ws://127.0.0.1:1"),
                                                   DisableWebSocketPings: true);

            var wasOne   = await node.DisconnectClient(stranger);

            Assert.Multiple(() => {
                Assert.That(wasOne,                                                 Is.False);
                Assert.That(node.WWCPWebSocketClients,                              Is.EqualTo(new[] { mine }));
                Assert.That(node.Routing.LookupNetworkingNode(A, out _),            Is.True);
            });

        }

        #endregion

    }

}
