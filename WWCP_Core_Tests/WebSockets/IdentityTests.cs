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
    /// Which networking node the WebSocket server takes a connection for: the
    /// one named by the last segment of its path, as OCPP-J has it - and not
    /// with the credentials of another one, unless that one may connect as it.
    /// </summary>
    /// <remarks>
    /// The credentials here are HTTP Basic and TOTP logins. A client
    /// certificate names its networking node by its common name, and is
    /// checked the same way; no test here sets up TLS for it.
    /// </remarks>
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

        private static readonly  NetworkingNode_Id     CS001         = NetworkingNode_Id.Parse("CS001");
        private static readonly  NetworkingNode_Id     CS002         = NetworkingNode_Id.Parse("CS002");
        private static readonly  NetworkingNode_Id     CS003         = NetworkingNode_Id.Parse("CS003");
        private static readonly  NetworkingNode_Id     LC001         = NetworkingNode_Id.Parse("LC001");

        private readonly         String                password      = $"pw-{Guid.NewGuid():N}";
        private readonly         String                sharedSecret  = $"totp-{Guid.NewGuid():N}";

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

            using var client = await Upgrade($"/?u={CS001}");

            Assert.That(server!.ConnectedNetworkingNodeIds, Does.Not.Contain(CS001),
                        "The name in the query string was taken for the networking node of the connection.");

        }

        #endregion


        #region AStationIsWhoItsPathAndItsCredentialsSay()

        [Test]
        public async Task AStationIsWhoItsPathAndItsCredentialsSay()
        {

            await Start(RequireAuthentication: true);

            using var client = await Upgrade($"/{CS001}", Basic(CS001));

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(101));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { CS001 }));
            });

        }

        #endregion

        #region AStationsPasswordOpensNoConnectionAsAnotherStation()

        /// <summary>
        /// CS002, with its right password, asks to be let in as CS001.
        /// </summary>
        [Test]
        public async Task AStationsPasswordOpensNoConnectionAsAnotherStation()
        {

            await Start(RequireAuthentication: true);

            using var client = await Upgrade($"/{CS001}", Basic(CS002));

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion

        #region AStationsOneTimePasswordOpensNoConnectionAsAnotherStation()

        /// <summary>
        /// CS002, with its right one-time password, asks to be let in as CS001.
        /// </summary>
        [Test]
        public async Task AStationsOneTimePasswordOpensNoConnectionAsAnotherStation()
        {

            await Start(RequireAuthentication: true);

            using var client = await Upgrade($"/{CS001}", TOTP(CS002));

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion

        #region ALocalControllerConnectsAsAStationItMayActFor()

        /// <summary>
        /// A local controller connects for a station behind it with the
        /// station's path and its own credentials (OCPP 2.1 Part 4, 6.2 and 6.5).
        /// The connection is one to the station, opened by the local controller.
        /// </summary>
        [Test]
        public async Task ALocalControllerConnectsAsAStationItMayActFor()
        {

            await Start(RequireAuthentication: true);

            server!.AllowToActFor(LC001, [ CS001 ]);

            using var client = await Upgrade($"/{CS001}", Basic(LC001));

            var connection = server.ConnectedNetworkingNodes.SingleOrDefault()?.WebSocketServerConnections.SingleOrDefault();

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(101));
                Assert.That(server.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { CS001 }));
                Assert.That(connection?.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.ActingNetworkingNodeId), Is.EqualTo(LC001),
                            "The connection does not say that the local controller opened it.");
            });

        }

        #endregion

        #region ALocalControllerConnectsAsNoStationItMayNotActFor()

        [Test]
        public async Task ALocalControllerConnectsAsNoStationItMayNotActFor()
        {

            await Start(RequireAuthentication: true);

            server!.AllowToActFor(LC001, [ CS001 ]);

            using var client = await Upgrade($"/{CS002}", Basic(LC001));

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion

        #region ALocalControllerThatMayNoLongerActForAStationConnectsAsItNoMore()

        [Test]
        public async Task ALocalControllerThatMayNoLongerActForAStationConnectsAsItNoMore()
        {

            await Start(RequireAuthentication: true);

            server!.AllowToActFor   (LC001, [ CS001, CS002 ]);
            server. DisallowToActFor(LC001, [ CS001 ]);

            using var client = await Upgrade($"/{CS001}", Basic(LC001));

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

            await Start(RequireAuthentication: true);

            using var client = await Upgrade("/", Basic(LC001));

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(101));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { LC001 }));
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

            await Start(RequireAuthentication: true);

            server!.AllowToActFor(LC001, [ CS001 ]);

            var wrong = HTTPBasicAuthentication.Create(LC001.ToString(), "wrong").HTTPText;

            using var mayActFor     = await Upgrade($"/{CS001}", wrong);
            using var mayNotActFor  = await Upgrade($"/{CS002}", wrong);

            Assert.Multiple(() => {
                Assert.That(mayActFor.   StatusCode, Is.EqualTo(401));
                Assert.That(mayNotActFor.StatusCode, Is.EqualTo(401));
            });

        }

        #endregion

        #region AKnownPasswordOpensNoConnectionAsAnotherStationWithoutRequiredAuthentication()

        /// <summary>
        /// A server that leaves the checking of credentials to a validator of
        /// its user's - as the CSMS does, with the logins in ClientLogins - still
        /// knows a right password when it sees one.
        /// </summary>
        [Test]
        public async Task AKnownPasswordOpensNoConnectionAsAnotherStationWithoutRequiredAuthentication()
        {

            await Start(RequireAuthentication: false);

            using var client = await Upgrade($"/{CS001}", Basic(CS002));

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion

        #region CredentialsTheServerCannotCheckOpenNoConnectionAsAnotherStation()

        /// <summary>
        /// A TOTP login this server does not know - the CSMS checks those against
        /// a store of its own - for another station than the one of the path. The
        /// server cannot refuse it as forbidden without telling apart right and
        /// wrong credentials; whatever lets it in, it is no connection to either.
        /// </summary>
        [Test]
        public async Task CredentialsTheServerCannotCheckOpenNoConnectionAsAnotherStation()
        {

            await Start(RequireAuthentication: false);

            using var client = await Upgrade($"/{CS001}", HTTPTOTPAuthentication.Create(CS003.ToString(), "123456789012", TOTPHTTPHeaderType.RAW).HTTPText);

            Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);

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

            server.AddOrUpdateHTTPBasicAuth(CS001, password);
            server.AddOrUpdateHTTPBasicAuth(CS002, password);
            server.AddOrUpdateHTTPBasicAuth(LC001, password);

            server.ClientTOTPConfig[CS002.ToString()] = new TOTPConfig(sharedSecret);

            await server.Start();

        }

        #endregion

        #region (private) Upgrade(PathAndQuery, Authorization = null)

        private Task<RawWebSocketClient> Upgrade(String   PathAndQuery,
                                                 String?  Authorization   = null)

            => RawWebSocketClient.Upgrade(port, PathAndQuery, Authorization);

        #endregion

        #region (private) Basic(NetworkingNodeId) / TOTP(NetworkingNodeId)

        /// <summary>
        /// The right HTTP Basic credentials of the given networking node.
        /// </summary>
        private String Basic(NetworkingNode_Id NetworkingNodeId)
            => HTTPBasicAuthentication.Create(NetworkingNodeId.ToString(), password).HTTPText;

        /// <summary>
        /// The right TOTP credentials of the given networking node.
        /// </summary>
        private String TOTP(NetworkingNode_Id NetworkingNodeId)
            => HTTPTOTPAuthentication.Create(NetworkingNodeId.ToString(), TOTPGenerator.GenerateTOTP(sharedSecret).Current, TOTPHTTPHeaderType.RAW).HTTPText;

        #endregion

    }

}
