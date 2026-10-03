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
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.UnitTests.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking.tests
{

    /// <summary>
    /// Which connection the overlay WebSocket server has for a networking node:
    /// the newest one, for as long as it is there, and none once it is gone -
    /// however the connection before it went, and however it went itself.
    /// </summary>
    /// <remarks>
    /// The overlay server registers a networking node once the 101 has gone out,
    /// so each test waits for the registration before it goes on. And it does
    /// not say when a connection has ended: a test waits until the connection is
    /// off the list of its WebSocket server, which takes it off just before it
    /// says so to its handlers.
    /// </remarks>
    [TestFixture]
    public class ConnectionBookkeepingTests
    {

        #region (class) Server

        /// <summary>
        /// An overlay WebSocket server that lets anybody in by the name in its
        /// path, which says which connection it has for a networking node, and
        /// whose close frames can be made to arrive whenever a test wants them.
        /// </summary>
        private sealed class Server(IPPort Port) : OverlayWebSocketServer(NetworkingNode_Id.Parse("onn"),
                                                                          [ "ocpp2.1" ],
                                                                          TCPPort:                Port,
                                                                          RequireAuthentication:  false,
                                                                          DisableWebSocketPings:  true)
        {

            /// <summary>
            /// The remote port of the connection the server has for the given
            /// networking node, if it has one.
            /// </summary>
            public UInt16? PortOfTheConnectionTo(NetworkingNode_Id NetworkingNodeId)

                => connectedNetworkingNodes.TryGetValue(NetworkingNodeId, out var connection)
                       ? connection.Item1.RemoteSocket.Port.ToUInt16()
                       : null;

            /// <summary>
            /// What the server does when a close frame arrives on the given
            /// connection.
            /// </summary>
            public Task CloseFrameArrivesOn(WebSocketServerConnection Connection)

                => ProcessCloseMessage(
                       Timestamp.Now,
                       Connection.WebSocketServer,
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

        private static readonly  NetworkingNode_Id  station  = NetworkingNode_Id.Parse("CS001");

        private                  Server?            server;
        private                  IPPort             port;

        #endregion

        #region SetUp() / TearDown()

        [SetUp]
        public void SetUp()
        {

            port    = TestPorts.Free();
            server  = new Server(port);

            server.Start();

        }

        [TearDown]
        public async Task TearDown()
        {

            if (server is not null)
                await server.Shutdown();

            server = null;

        }

        #endregion


        #region AStationThatGoesAwayWithoutACloseFrameIsForgotten(Reset)

        /// <summary>
        /// A station whose TCP connection ends without a close frame - with a
        /// FIN, or with a reset - is gone, and the server has no connection to
        /// it any more.
        /// </summary>
        [TestCase(false, TestName = "AStationThatHangsUpWithoutACloseFrameIsForgotten")]
        [TestCase(true,  TestName = "AStationWhoseConnectionIsResetIsForgotten")]
        public async Task AStationThatGoesAwayWithoutACloseFrameIsForgotten(Boolean Reset)
        {

            using var client = await Connected($"/{station}");

            if (Reset)
                client.Abort();
            else
                client.HangUp();

            await NoticedTheEndOf(client);

            Assert.That(await Until(() => server!.PortOfTheConnectionTo(station) is null), Is.True,
                        "The station's connection has ended, and the server still has it as the connection to the station.");

        }

        #endregion

        #region AStationThatSaysGoodbyeIsForgotten()

        [Test]
        public async Task AStationThatSaysGoodbyeIsForgotten()
        {

            using var client = await Connected($"/{station}");

            await client.SendClose();
            await NoticedTheEndOf(client);

            Assert.That(await Until(() => server!.PortOfTheConnectionTo(station) is null), Is.True,
                        "The station has closed its connection, and the server still has it as the connection to the station.");

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

            using var older = await Connected($"/{station}");
            using var newer = await Upgrade  ($"/{station}");

            await NoticedTheEndOf(older);

            Assert.That(await Until(() => server!.PortOfTheConnectionTo(station) == newer.LocalPort), Is.True,
                        "The newer connection was never registered.");

            // Whatever the server does about the end of the older connection, it
            // does it in handlers that run once the connection is off the list.
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            Assert.That(server!.PortOfTheConnectionTo(station), Is.EqualTo(newer.LocalPort),
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
        /// Which comes first is up to the two connections' loops. Here the frame
        /// is late every time: the server is given it once the newer connection
        /// is in place.
        /// </remarks>
        [Test]
        public async Task ALateCloseFrameOfTheOlderConnectionLeavesTheNewerOne()
        {

            using var older            = await Connected($"/{station}");
            var       olderConnection  = server!.WebSocketConnections.Single(connection => connection.RemoteSocket.Port.ToUInt16() == older.LocalPort);

            using var newer            = await Upgrade($"/{station}");

            await NoticedTheEndOf(older);

            Assert.That(await Until(() => server.PortOfTheConnectionTo(station) == newer.LocalPort), Is.True,
                        "The newer connection was never registered.");

            await server.CloseFrameArrivesOn(olderConnection);

            Assert.That(server.PortOfTheConnectionTo(station), Is.EqualTo(newer.LocalPort),
                        "The close frame of the older connection took the newer one with it.");

        }

        #endregion


        #region (private) Upgrade  (PathAndQuery)

        private async Task<RawWebSocketClient> Upgrade(String PathAndQuery)
        {

            var client = await RawWebSocketClient.Upgrade(port, PathAndQuery);

            Assert.That(client.StatusCode, Is.EqualTo(101));

            return client;

        }

        #endregion

        #region (private) Connected(PathAndQuery)

        /// <summary>
        /// A client upgraded and registered as the connection to the station.
        /// </summary>
        private async Task<RawWebSocketClient> Connected(String PathAndQuery)
        {

            var client = await Upgrade(PathAndQuery);

            Assert.That(await Until(() => server!.PortOfTheConnectionTo(station) == client.LocalPort), Is.True,
                        "The station's connection was never registered.");

            return client;

        }

        #endregion

        #region (private) NoticedTheEndOf(Client)

        /// <summary>
        /// Wait until the server has seen the end of the given client's TCP
        /// connection: until its WebSocket server has taken it off its list.
        /// </summary>
        private async Task NoticedTheEndOf(RawWebSocketClient Client)
        {

            Assert.That(await Until(() => !server!.WebSocketConnections.Any(connection => connection.RemoteSocket.Port.ToUInt16() == Client.LocalPort),
                                    TimeSpan.FromSeconds(10)),
                        Is.True,
                        "The server never noticed that the connection had ended.");

        }

        #endregion

        #region (private static) Until(Condition, Within = 2 s)

        /// <summary>
        /// Whether the given condition holds within the given time.
        /// </summary>
        private static async Task<Boolean> Until(Func<Boolean>  Condition,
                                                 TimeSpan?      Within   = null)
        {

            var deadline = DateTimeOffset.UtcNow + (Within ?? TimeSpan.FromSeconds(2));

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
