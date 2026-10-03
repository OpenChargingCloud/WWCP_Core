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

using cloud.charging.open.protocols.WWCP.UnitTests.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking.tests
{

    /// <summary>
    /// Whom the overlay WebSocket server lets in: with RequireAuthentication,
    /// a networking node with the right password of its NetworkingNodeLogins,
    /// and nobody else; without it, anybody.
    /// </summary>
    [TestFixture]
    public class AuthenticationTests
    {

        #region Data

        private static readonly  NetworkingNode_Id        station   = NetworkingNode_Id.Parse("CS001");

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


        #region TheRightPasswordLetsANodeIn()

        [Test]
        public async Task TheRightPasswordLetsANodeIn()
        {

            Start(RequireAuthentication: true);

            using var client = await RawWebSocketClient.Upgrade(port, $"/{station}", Basic(station, password));

            Assert.That(client.StatusCode, Is.EqualTo(101),
                        "The station was kept out with its right password.");

        }

        #endregion

        #region AWrongPasswordKeepsANodeOut()

        [Test]
        public async Task AWrongPasswordKeepsANodeOut()
        {

            Start(RequireAuthentication: true);

            using var client = await RawWebSocketClient.Upgrade(port, $"/{station}", Basic(station, "wrong"));

            Assert.That(client.StatusCode, Is.EqualTo(401));

        }

        #endregion

        #region AnUnknownLoginKeepsANodeOut()

        [Test]
        public async Task AnUnknownLoginKeepsANodeOut()
        {

            Start(RequireAuthentication: true);

            using var client = await RawWebSocketClient.Upgrade(port, "/CS002", Basic(NetworkingNode_Id.Parse("CS002"), password));

            Assert.That(client.StatusCode, Is.EqualTo(401));

        }

        #endregion

        #region NoCredentialsKeepANodeOut()

        [Test]
        public async Task NoCredentialsKeepANodeOut()
        {

            Start(RequireAuthentication: true);

            using var client = await RawWebSocketClient.Upgrade(port, $"/{station}");

            Assert.That(client.StatusCode, Is.EqualTo(401));

        }

        #endregion

        #region WithoutRequiredAuthenticationAnybodyIsLetIn()

        [Test]
        public async Task WithoutRequiredAuthenticationAnybodyIsLetIn()
        {

            Start(RequireAuthentication: false);

            using var client = await RawWebSocketClient.Upgrade(port, $"/{station}");

            Assert.That(client.StatusCode, Is.EqualTo(101));

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

            server.AddOrUpdateHTTPBasicAuth(station, password);
            server.Start();

        }

        #endregion

        #region (private static) Basic(NetworkingNodeId, Password)

        private static String Basic(NetworkingNode_Id  NetworkingNodeId,
                                    String             Password)

            => HTTPBasicAuthentication.Create(NetworkingNodeId.ToString(), Password).HTTPText;

        #endregion

    }

}
