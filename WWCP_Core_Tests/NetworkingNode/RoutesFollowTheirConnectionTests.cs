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
    /// A route that leads through a client connection goes when that connection
    /// ends, and comes back with the next one.
    /// </summary>
    /// <remarks>
    /// A route was written down every time a client connected, and never taken
    /// away. A message for a node whose connection had ended was still handed to
    /// that connection's client, which had nowhere to send it; and with two
    /// connections to one name, the one that had ended could keep the name.
    /// Taken away when the connection ends, a message for a node that is away is
    /// told so at once, and whoever keeps it for later - an offline queue - keeps
    /// it.
    /// </remarks>
    [TestFixture]
    public class RoutesFollowTheirConnectionTests
    {

        #region (class) Node

        private sealed class Node : AWWCPNetworkingNode
        {
            public Node()
                : base(NetworkingNode_Id.Parse("routing"))
            { }
        }

        #endregion

        #region Data

        private static readonly NetworkingNode_Id  A  = NetworkingNode_Id.Parse("back-end-a");
        private static readonly NetworkingNode_Id  B  = NetworkingNode_Id.Parse("back-end-b");

        private readonly List<WebSocketServer>       servers  = [];
        private readonly List<WWCPWebSocketClient>   clients  = [];

        #endregion

        #region TearDown()

        [TearDown]
        public async Task TearDown()
        {

            foreach (var client in clients)
                await client.Close();

            foreach (var server in servers)
                await server.Shutdown();

            clients.Clear();
            servers.Clear();

        }

        #endregion


        #region (private) AServer()

        /// <summary>
        /// A server that lets anybody in: who connects is not what is asked here.
        /// </summary>
        private async Task<WebSocketServer> AServer()
        {

            var server = new WebSocketServer(HTTPPort:               TestPorts.Free(),
                                             RequireAuthentication:  false);

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

            Assert.That(Node.Routing.LookupNetworkingNode(NextHop, out _), Is.True,
                        $"A connected client was not the route to '{NextHop}'.");

            return client;

        }

        #endregion

        #region (private static) Until(Condition, Within)

        private static async Task<Boolean> Until(Func<Boolean> Condition, TimeSpan Within)
        {

            var until = DateTimeOffset.UtcNow + Within;

            while (DateTimeOffset.UtcNow < until)
            {

                if (Condition())
                    return true;

                await Task.Delay(50);

            }

            return Condition();

        }

        #endregion


        #region ARouteGoesWhenTheOtherSideEndsTheConnection()

        /// <summary>
        /// The server goes away: the route through the connection to it goes too.
        /// </summary>
        [Test]
        public async Task ARouteGoesWhenTheOtherSideEndsTheConnection()
        {

            var node    = new Node();
            var server  = await AServer();

            await Connect(node, server, A);

            await server.Shutdown();

            Assert.That(await Until(() => !node.Routing.LookupNetworkingNode(A, out _), TimeSpan.FromSeconds(10)), Is.True,
                        "The route through a connection the other side had ended was still there.");

        }

        #endregion

        #region ARouteGoesWhenTheNodeHangsUp()

        /// <summary>
        /// The node closes the connection itself: its route goes with it.
        /// </summary>
        [Test]
        public async Task ARouteGoesWhenTheNodeHangsUp()
        {

            var node    = new Node();
            var server  = await AServer();
            var client  = await Connect(node, server, A);

            await client.Close();

            Assert.That(await Until(() => !node.Routing.LookupNetworkingNode(A, out _), TimeSpan.FromSeconds(10)), Is.True,
                        "The route through a connection the node had closed itself was still there.");

        }

        #endregion

        #region TheRouteOfAnotherConnectionStays()

        /// <summary>
        /// Only the routes through the connection that ended go: another
        /// connection's route stays, and so does the node's way to it.
        /// </summary>
        [Test]
        public async Task TheRouteOfAnotherConnectionStays()
        {

            var node     = new Node();
            var serverA  = await AServer();
            var serverB  = await AServer();
            var clientB  = await Connect(node, serverB, B);

            await Connect(node, serverA, A);

            await serverA.Shutdown();

            Assert.That(await Until(() => !node.Routing.LookupNetworkingNode(A, out _), TimeSpan.FromSeconds(10)), Is.True,
                        "The route through the connection that ended was still there.");

            Assert.Multiple(() => {

                Assert.That(node.Routing.LookupNetworkingNode(B, out var reachability), Is.True,
                            "The route through the other connection went with the one that ended.");

                Assert.That(reachability?.Client,                                       Is.SameAs(clientB));

            });

        }

        #endregion

    }

}
