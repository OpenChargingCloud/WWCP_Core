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

using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// Which networking node the WebSocket server takes a connection for.
    /// </summary>
    [TestFixture]
    public class IdentityTests
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

        #region Data

        private static readonly  NetworkingNode_Id     CS001  = NetworkingNode_Id.Parse("CS001");

        private                  WWCPWebSocketServer?  server;
        private                  IPPort                port;

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


        #region ANameInTheQueryStringIsNoIdentity()

        /// <summary>
        /// A connection to "/?u=CS001" says neither in its path nor with any
        /// credentials which networking node it is, and is no connection to
        /// CS001.
        /// </summary>
        [Test]
        public async Task ANameInTheQueryStringIsNoIdentity()
        {

            await Start(RequireAuthentication: false);

            using var client = await RawWebSocketClient.Upgrade(port, $"/?u={CS001}");

            Assert.That(server!.ConnectedNetworkingNodeIds, Does.Not.Contain(CS001),
                        "The name in the query string was taken for the networking node of the connection.");

        }

        #endregion


        #region (private) Start(RequireAuthentication)

        private async Task Start(Boolean RequireAuthentication)
        {

            port    = TestPorts.Free();

            server  = new WWCPWebSocketServer(
                          new Node(),
                          HTTPPort:               port,
                          RequireAuthentication:  RequireAuthentication,
                          SecWebSocketProtocols:  [ "ocpp2.1" ],
                          DisableWebSocketPings:  true
                      );

            await server.Start();

        }

        #endregion

    }

}
