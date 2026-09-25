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

using System.Net;
using System.Net.Sockets;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.NetworkingNode
{

    /// <summary>
    /// A networking node's reconnect policy reaches every WebSocket client it
    /// connects before that client's first attempt - so that a client whose
    /// first attempt finds nothing listening comes back once something is.
    /// </summary>
    /// <remarks>
    /// The node makes and connects its clients inside ConnectWebSocketClient,
    /// and a client whose first attempt failed has ended by the time anybody
    /// outside could give it a policy. A charging station started while its
    /// CSMS was down never reached it afterwards.
    /// </remarks>
    [TestFixture]
    public class ReconnectPolicyTests
    {

        #region (class) Node

        /// <summary>
        /// A networking node of no particular kind.
        /// </summary>
        private sealed class Node : AWWCPNetworkingNode
        {
            public Node()
                : base(NetworkingNode_Id.Parse("reconnecting"))
            { }
        }

        #endregion

        #region Data

        private static WebSocketClientReconnectPolicy Quickly
            => new (InitialDelay: TimeSpan.FromMilliseconds(200),
                    MaxDelay:     TimeSpan.FromMilliseconds(500));

        private WWCPWebSocketClient?  client;
        private WebSocketServer?      server;

        #endregion

        #region TearDown()

        [TearDown]
        public async Task TearDown()
        {

            if (client is not null)
                await client.Close();

            if (server is not null)
                await server.Shutdown();

            client = null;
            server = null;

        }

        #endregion


        #region AClientOfANodeWithAPolicyComesBackFromAFailedFirstAttempt()

        /// <summary>
        /// Nothing is listening at the first attempt; something is a moment
        /// later, and the client of a node with a policy connects to it.
        /// </summary>
        [Test]
        public async Task AClientOfANodeWithAPolicyComesBackFromAFailedFirstAttempt()
        {

            var node      = new Node { ReconnectPolicy = Quickly };
            var port      = FreePort();

            client        = new WWCPWebSocketClient(node, URL.Parse($"ws://127.0.0.1:{port}"));

            var response  = await node.ConnectWebSocketClient(client);

            Assert.Multiple(() => {
                Assert.That(response.HTTPStatusCode.Code, Is.Not.EqualTo(101),
                            "There was nothing to connect to, and the first attempt said it had connected.");
                Assert.That(client.ReconnectPolicy,       Is.Not.Null,
                            "The client of a node with a reconnect policy had none.");
            });

            Assert.That(await Connected(port, TimeSpan.FromSeconds(10)), Is.True,
                        "The client of a node with a reconnect policy did not connect once there was something to connect to.");

        }

        #endregion

        #region AClientKeepsAPolicyOfItsOwn()

        /// <summary>
        /// A client that brings a policy of its own keeps it.
        /// </summary>
        [Test]
        public async Task AClientKeepsAPolicyOfItsOwn()
        {

            var own   = Quickly;
            var node  = new Node { ReconnectPolicy = Quickly };

            client    = new WWCPWebSocketClient(node, URL.Parse($"ws://127.0.0.1:{FreePort()}")) {
                            ReconnectPolicy = own
                        };

            await node.ConnectWebSocketClient(client);

            Assert.That(client.ReconnectPolicy, Is.SameAs(own));

        }

        #endregion

        #region AClientOfANodeWithoutAPolicyStaysAway()

        /// <summary>
        /// And a node without a policy leaves its clients as they were: a
        /// first attempt that fails is the last.
        /// </summary>
        [Test]
        public async Task AClientOfANodeWithoutAPolicyStaysAway()
        {

            var node  = new Node();
            var port  = FreePort();

            client    = new WWCPWebSocketClient(node, URL.Parse($"ws://127.0.0.1:{port}"));

            await node.ConnectWebSocketClient(client);

            Assert.Multiple(async () => {
                Assert.That(client.ReconnectPolicy, Is.Null);
                Assert.That(await Connected(port, TimeSpan.FromSeconds(2)), Is.False,
                            "The client of a node without a reconnect policy tried again.");
            });

        }

        #endregion


        #region (private) Connected(Port, Within)

        /// <summary>
        /// Whether the client connects to a server started on the port, within
        /// the given time.
        /// </summary>
        private async Task<Boolean> Connected(IPPort Port, TimeSpan Within)
        {

            server = new WebSocketServer(HTTPPort: Port, AutoStart: true);

            var giveUp = DateTimeOffset.UtcNow + Within;

            while (DateTimeOffset.UtcNow < giveUp)
            {

                if (server.WebSocketConnections.Any())
                    return true;

                await Task.Delay(50);

            }

            return false;

        }

        #endregion

        #region (private static) FreePort()

        private static IPPort FreePort()
        {

            var listener = new TcpListener(System.Net.IPAddress.Loopback, 0);

            listener.Start();

            try
            {
                return IPPort.Parse((UInt16) ((IPEndPoint) listener.LocalEndpoint).Port);
            }
            finally
            {
                listener.Stop();
            }

        }

        #endregion

    }

}
