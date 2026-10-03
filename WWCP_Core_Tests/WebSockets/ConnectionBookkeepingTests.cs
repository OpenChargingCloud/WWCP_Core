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

using System.Collections.Concurrent;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// Which connection the WebSocket server sends to for a networking node:
    /// the newest one, for as long as it is there, and none once it is gone -
    /// however the connections before it went, and however it went itself.
    /// </summary>
    [TestFixture]
    public class ConnectionBookkeepingTests
    {

        #region (class) Node

        /// <summary>
        /// A networking node of no particular kind.
        /// </summary>
        private sealed class Node : AWWCPNetworkingNode
        {
            public Node()
                : base(NetworkingNode_Id.Parse("csms"))
            { }
        }

        #endregion

        #region (class) Server

        /// <summary>
        /// A WebSocket server that lets anybody in by the name in its path, and
        /// whose close frames can be made to arrive whenever a test wants them.
        /// </summary>
        private sealed class Server(IPPort Port) : WWCPWebSocketServer(new Node(),
                                                                       HTTPPort:               Port,
                                                                       RequireAuthentication:  false,
                                                                       SecWebSocketProtocols:  [ "ocpp2.1" ],
                                                                       DisableWebSocketPings:  true)
        {

            /// <summary>
            /// What the server does when a close frame arrives on the given
            /// connection.
            /// </summary>
            public Task CloseFrameArrivesOn(WebSocketServerConnection Connection)

                => ProcessCloseMessage(
                       Timestamp.Now,
                       this,
                       Connection,
                       WebSocketFrame.Close(WebSocketFrame.ClosingStatusCode.NormalClosure),
                       EventTracking_Id.New,
                       WebSocketFrame.ClosingStatusCode.NormalClosure,
                       null,
                       CancellationToken.None
                   );

        }

        #endregion

        #region Data

        private static readonly  NetworkingNode_Id                                     station        = NetworkingNode_Id.Parse("CS001");

        private                  Server?                                               server;
        private                  IPPort                                                port;

        private readonly         ConcurrentQueue<WebSocketServerConnection>            accepted       = [];
        private readonly         ConcurrentDictionary<UInt16, TaskCompletionSource>    tcpClosed      = [];

        #endregion

        #region SetUp() / TearDown()

        [SetUp]
        public async Task SetUp()
        {

            accepted.Clear();
            tcpClosed.Clear();

            port    = TestPorts.Free();
            server  = new Server(port);

            server.OnNetworkingNodeWebSocketConnectionAccepted += (_, _, connection, _, _, _, _, _) => {
                accepted.Enqueue(connection);
                return Task.CompletedTask;
            };

            server.OnTCPConnectionClosed += (_, _, connection, _, _, _) => {
                TCPClosed(connection.RemoteSocket.Port.ToUInt16()).TrySetResult();
                return Task.CompletedTask;
            };

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


        #region AStationThatGoesAwayWithoutACloseFrameIsForgotten(Reset)

        /// <summary>
        /// A station whose TCP connection ends without a close frame - with a
        /// FIN, or with a reset, as a station does that loses its power or its
        /// network - is gone, and the server has no connection to it any more.
        /// </summary>
        [TestCase(false, TestName = "AStationThatHangsUpWithoutACloseFrameIsForgotten")]
        [TestCase(true,  TestName = "AStationWhoseConnectionIsResetIsForgotten")]
        public async Task AStationThatGoesAwayWithoutACloseFrameIsForgotten(Boolean Reset)
        {

            using var client = await RawWebSocketClient.Upgrade(port, $"/{station}");

            Assert.That(client.StatusCode,                Is.EqualTo(101));
            Assert.That(server!.ConnectedNetworkingNodeIds, Does.Contain(station));

            if (Reset)
                client.Abort();
            else
                client.HangUp();

            await NoticedTheEndOf(client);

            Assert.That(await Until(() => !server.ConnectedNetworkingNodeIds.Contains(station)), Is.True,
                        "The station's connection has ended, and the server still has it as the way to the station.");

        }

        #endregion

        #region AStationThatSaysGoodbyeIsForgotten()

        [Test]
        public async Task AStationThatSaysGoodbyeIsForgotten()
        {

            using var client = await RawWebSocketClient.Upgrade(port, $"/{station}");

            Assert.That(client.StatusCode, Is.EqualTo(101));

            await client.SendClose();
            await NoticedTheEndOf(client);

            Assert.That(await Until(() => !server!.ConnectedNetworkingNodeIds.Contains(station)), Is.True,
                        "The station has closed its connection, and the server still has it as the way to the station.");

        }

        #endregion

        #region ANewerConnectionOutlivesTheOlderOne()

        /// <summary>
        /// A station that connects again replaces its older connection, which
        /// the server closes. The end of the older connection is not the end of
        /// the newer one.
        /// </summary>
        [Test]
        public async Task ANewerConnectionOutlivesTheOlderOne()
        {

            using var older = await RawWebSocketClient.Upgrade(port, $"/{station}");
            using var newer = await RawWebSocketClient.Upgrade(port, $"/{station}");

            Assert.That(older.StatusCode, Is.EqualTo(101));
            Assert.That(newer.StatusCode, Is.EqualTo(101));

            await NoticedTheEndOf(older);

            // Whatever the server does about the end of the older connection,
            // it does it in handlers that run alongside the one of this test.
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            Assert.That(PortsOfTheConnectionsTo(station), Is.EqualTo(new[] { newer.LocalPort }),
                        "The end of the older connection took the newer one with it.");

        }

        #endregion

        #region ALateCloseFrameOfTheOlderConnectionLeavesTheNewerOne()

        /// <summary>
        /// A station closes its connection and connects again at once, and the
        /// close frame of the older connection is read only once the newer one
        /// has been registered. It closes the older connection, and nothing else.
        /// </summary>
        /// <remarks>
        /// Which comes first is up to the two connections' loops, and the frame
        /// is late only now and then. Here it is late every time: the server is
        /// given it once the newer connection is in place.
        /// </remarks>
        [Test]
        public async Task ALateCloseFrameOfTheOlderConnectionLeavesTheNewerOne()
        {

            using var older = await RawWebSocketClient.Upgrade(port, $"/{station}");
            using var newer = await RawWebSocketClient.Upgrade(port, $"/{station}");

            Assert.That(older.StatusCode, Is.EqualTo(101));
            Assert.That(newer.StatusCode, Is.EqualTo(101));

            var olderConnection = accepted.Single(connection => connection.RemoteSocket.Port.ToUInt16() == older.LocalPort);

            await server!.CloseFrameArrivesOn(olderConnection);

            Assert.That(PortsOfTheConnectionsTo(station), Is.EqualTo(new[] { newer.LocalPort }),
                        "The close frame of the older connection took the newer one with it.");

        }

        #endregion


        #region (private) TCPClosed(RemotePort)

        private TaskCompletionSource TCPClosed(UInt16 RemotePort)

            => tcpClosed.GetOrAdd(
                   RemotePort,
                   _ => new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)
               );

        #endregion

        #region (private) NoticedTheEndOf(Client)

        /// <summary>
        /// Wait until the server has seen the end of the given client's TCP
        /// connection.
        /// </summary>
        private async Task NoticedTheEndOf(RawWebSocketClient Client)
        {

            var closed = TCPClosed(Client.LocalPort).Task;

            Assert.That(await Task.WhenAny(closed, Task.Delay(TimeSpan.FromSeconds(10))), Is.SameAs(closed),
                        "The server never noticed that the connection had ended.");

        }

        #endregion

        #region (private) PortsOfTheConnectionsTo(NetworkingNodeId)

        /// <summary>
        /// The remote ports of the connections the server has to the given
        /// networking node.
        /// </summary>
        private IEnumerable<UInt16> PortsOfTheConnectionsTo(NetworkingNode_Id NetworkingNodeId)

            => server!.ConnectedNetworkingNodes.
                       Where     (networkingNode => networkingNode.DestinationNodeId == NetworkingNodeId).
                       SelectMany(networkingNode => networkingNode.WebSocketServerConnections).
                       Select    (connection     => connection.RemoteSocket.Port.ToUInt16()).
                       ToArray();

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
