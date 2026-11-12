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
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// A message for a networking node behind another one - a charging station behind its local
    /// controller - goes over the other node's connection only where that connection is in the overlay
    /// network. In the standard networking mode a message names no destination, and the far end takes it as
    /// its own: a Reset for the station would reset the controller.
    /// </summary>
    [TestFixture]
    public class ThroughAnotherNodeTests
    {

        #region (class) Node / Server

        /// <summary>
        /// A networking node of no particular kind.
        /// </summary>
        private sealed class Node : AWWCPNetworkingNode
        {
            public Node()
                : base(NetworkingNode_Id.Parse("csms"))
            { }
        }

        /// <summary>
        /// A WebSocket server that lets anybody in by the name in its path, and says over which of its
        /// connections a message for a networking node would go.
        /// </summary>
        private sealed class Server(Node    Node,
                                    IPPort  Port) : WWCPWebSocketServer(Node,
                                                                        HTTPPort:               Port,
                                                                        RequireAuthentication:  false,
                                                                        SecWebSocketProtocols:  [ "ocpp2.1" ],
                                                                        DisableWebSocketPings:  true)
        {

            public WebSocketServerConnection[] ConnectionsFor(NetworkingNode_Id DestinationId)
                => [.. GetConnectionsFor(DestinationId)];

        }

        #endregion

        #region Data

        private static readonly NetworkingNode_Id  localController   = NetworkingNode_Id.Parse("LC001");
        private static readonly NetworkingNode_Id  stationBehind     = NetworkingNode_Id.Parse("CS002");

        private Node?    node;
        private Server?  server;
        private IPPort   port;

        #endregion

        #region SetUp() / TearDown()

        [SetUp]
        public async Task SetUp()
        {

            port    = TestPorts.Free();
            node    = new Node();
            server  = new Server(node, port);

            await server.Start();

            // The local controller is reached over this server, and the station behind it through it.
            node.Routing.AddOrUpdateStaticRouting(localController, server);
            node.Routing.AddOrUpdateStaticRouting(stationBehind,   localController);

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


        #region AStandardConnection_CarriesNothingForTheNodeBehindIt()

        /// <summary>
        /// A local controller that dialled without X-OCPP-NetworkingMode is sent what is for itself, and
        /// nothing that is for the station behind it: there is no connection for that, as for a node nobody
        /// knows.
        /// </summary>
        [Test]
        public async Task AStandardConnection_CarriesNothingForTheNodeBehindIt()
        {

            using var client = await RawWebSocketClient.Upgrade(port, $"/{localController}");

            Assert.That(client.StatusCode, Is.EqualTo(101));

            Assert.Multiple(() => {
                Assert.That(server!.ConnectionsFor(localController),  Has.Length.EqualTo(1), "the controller itself is not reached");
                Assert.That(server. ConnectionsFor(stationBehind),    Is.Empty,              "a message for the station behind the controller would go over its standard connection, and be the controller's");
            });

        }

        #endregion

        #region AnOverlayConnection_CarriesWhatIsForTheNodeBehindIt()

        /// <summary>
        /// A local controller that dialled in the overlay network is sent both: what is for itself, and what
        /// is for the station behind it - which the frame of the overlay network names.
        /// </summary>
        [Test]
        public async Task AnOverlayConnection_CarriesWhatIsForTheNodeBehindIt()
        {

            using var client = await RawWebSocketClient.Upgrade(port,
                                                                $"/{localController}",
                                                                Headers: [ $"{WebSocketKeys.X_WWCP_NetworkingMode}: {NetworkingMode.OverlayNetwork}" ]);

            Assert.That(client.StatusCode, Is.EqualTo(101));

            var connection = server!.ConnectionsFor(localController).Single();

            Assert.Multiple(() => {
                Assert.That(connection.InOverlayNetwork(),         Is.True);
                Assert.That(server.ConnectionsFor(stationBehind),  Is.EqualTo(new[] { connection }));
            });

        }

        #endregion

        #region ABroadcast_GoesOverEveryConnection()

        /// <summary>
        /// A broadcast is for everybody, and every far end takes it as its own: it goes over a standard
        /// connection as before.
        /// </summary>
        [Test]
        public async Task ABroadcast_GoesOverEveryConnection()
        {

            using var client = await RawWebSocketClient.Upgrade(port, $"/{localController}");

            Assert.That(client.StatusCode,                                         Is.EqualTo(101));
            Assert.That(server!.ConnectionsFor(NetworkingNode_Id.Broadcast),        Has.Length.EqualTo(1));

        }

        #endregion

    }

}
