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

using System.Security.Cryptography.X509Certificates;

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
    /// over TLS for, when the client shows a certificate: the one named by the
    /// last segment of its path - and not with the certificate of another one,
    /// unless that one may connect as it. The certificate names its networking
    /// node by its common name.
    /// </summary>
    /// <remarks>
    /// The server asks every client for a certificate and lets in those it made
    /// for the test, and requires no authentication of its own: its HTTP Basic
    /// check knows nothing of certificates. It registers a networking node once
    /// the 101 has gone out, so a test that expects a registration waits for it.
    /// </remarks>
    [TestFixture]
    public class CertificateIdentityTests
    {

        #region Data

        private static readonly  NetworkingNode_Id        CS001     = NetworkingNode_Id.Parse("CS001");
        private static readonly  NetworkingNode_Id        CS002     = NetworkingNode_Id.Parse("CS002");
        private static readonly  NetworkingNode_Id        LC001     = NetworkingNode_Id.Parse("LC001");

        private readonly         String                   password  = $"pw-{Guid.NewGuid():N}";

        private                  X509Certificate2?        serverCertificate;
        private readonly         Dictionary<NetworkingNode_Id, X509Certificate2>  clientCertificates  = [];

        private                  OverlayWebSocketServer?  server;
        private                  IPPort                   port;

        #endregion

        #region OneTimeSetUp() / OneTimeTearDown() / TearDown()

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

            serverCertificate = TestCertificates.Server();

            foreach (var networkingNodeId in new[] { CS001, CS002, LC001 })
                clientCertificates[networkingNodeId] = TestCertificates.Client(networkingNodeId.ToString());

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

            serverCertificate?.Dispose();

            foreach (var clientCertificate in clientCertificates.Values)
                clientCertificate.Dispose();

            clientCertificates.Clear();

        }

        [TearDown]
        public async Task TearDown()
        {

            if (server is not null)
                await server.Shutdown();

            server = null;

        }

        #endregion


        #region ANodeIsWhoItsPathAndItsCertificateSay()

        [Test]
        public async Task ANodeIsWhoItsPathAndItsCertificateSay()
        {

            Start();

            using var client      = await Upgrade($"/{CS001}", CS001);
            var       registered  = await RegisteredAs(CS001);
            var       connection  = ConnectionOf(client);

            Assert.Multiple(() => {
                Assert.That(client.StatusCode, Is.EqualTo(101));
                Assert.That(registered,        Is.True,
                            "The node was not registered as the one its path and its certificate name.");
                Assert.That(connection?.ClientCertificate?.GetCertHashString(), Is.EqualTo(clientCertificates[CS001].GetCertHashString()),
                            "The connection does not have the client certificate.");
            });

        }

        #endregion

        #region ANodesCertificateOpensNoConnectionAsAnotherNode()

        /// <summary>
        /// CS002, with its certificate, asks to be let in as CS001.
        /// </summary>
        [Test]
        public async Task ANodesCertificateOpensNoConnectionAsAnotherNode()
        {

            Start();

            using var client = await Upgrade($"/{CS001}", CS002);

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion

        #region ALocalControllersCertificateConnectsAsANodeItMayActFor()

        /// <summary>
        /// A local controller connects for a node behind it with the node's path
        /// and its own certificate (OCPP 2.1 Part 4, 6.5).
        /// </summary>
        [Test]
        public async Task ALocalControllersCertificateConnectsAsANodeItMayActFor()
        {

            Start();

            server!.AllowToActFor(LC001, [ CS001 ]);

            using var client      = await Upgrade($"/{CS001}", LC001);
            var       registered  = await RegisteredAs(CS001);
            var       connection  = ConnectionOf(client);

            Assert.Multiple(() => {
                Assert.That(client.StatusCode, Is.EqualTo(101));
                Assert.That(registered,        Is.True,
                            $"The connection was registered as {server.ConnectedNetworkingNodeIds.AggregateWith(", ")}, and not as the node of its path.");
                Assert.That(connection?.TryGetCustomDataAs<NetworkingNode_Id>(AOverlayWebSocketServer.actingNetworkingNodeId_WebSocketKey), Is.EqualTo(LC001),
                            "The connection does not say that the local controller opened it.");
            });

        }

        #endregion

        #region ALocalControllersCertificateConnectsAsNoNodeItMayNotActFor()

        [Test]
        public async Task ALocalControllersCertificateConnectsAsNoNodeItMayNotActFor()
        {

            Start();

            server!.AllowToActFor(LC001, [ CS001 ]);

            using var client = await Upgrade($"/{CS002}", LC001);

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion

        #region ACertificateNamesTheNodeOfAPathWithoutAName()

        /// <summary>
        /// A path without a name in its last segment names no networking node,
        /// and the certificate does - as for a local controller whose URL is the
        /// endpoint alone.
        /// </summary>
        [Test]
        public async Task ACertificateNamesTheNodeOfAPathWithoutAName()
        {

            Start();

            using var client      = await Upgrade("/", LC001);
            var       registered  = await RegisteredAs(LC001);

            Assert.Multiple(() => {
                Assert.That(client.StatusCode, Is.EqualTo(101));
                Assert.That(registered,        Is.True,
                            "The node was not registered as the one its certificate names.");
            });

        }

        #endregion

        #region TheCertificateNamesTheNodeBeforeALogin()

        /// <summary>
        /// A client with a certificate and an HTTP Basic login is the networking
        /// node of its certificate: the certificate of CS002 with the right login
        /// of CS001 opens no connection as CS001, and the certificate of CS001
        /// with the right login of CS002 does.
        /// </summary>
        [Test]
        public async Task TheCertificateNamesTheNodeBeforeALogin()
        {

            Start();

            using var asAnotherNode  = await Upgrade($"/{CS001}", CS002, Basic(CS001));
            using var asItself       = await Upgrade($"/{CS001}", CS001, Basic(CS002));
            var       registered     = await RegisteredAs(CS001);

            Assert.Multiple(() => {
                Assert.That(asAnotherNode.StatusCode, Is.EqualTo(403));
                Assert.That(asItself.     StatusCode, Is.EqualTo(101));
                Assert.That(registered,               Is.True,
                            "The node was not registered as the one its certificate names.");
            });

        }

        #endregion


        #region (private) Start()

        private void Start()
        {

            port    = TestPorts.Free();

            server  = new OverlayWebSocketServer(
                          NetworkingNode_Id.Parse("onn"),
                          [ "ocpp2.1" ],
                          TCPPort:                     port,
                          RequireAuthentication:       false,
                          DisableWebSocketPings:       true,
                          ServerCertificateSelector:   (_, _) => serverCertificate!,
                          ClientCertificateValidator:  (_, certificate, _, _, _) => certificate is not null &&
                                                                                    clientCertificates.Values.Any(clientCertificate => clientCertificate.GetCertHashString() == certificate.GetCertHashString())
                                                                                        ? TLSValidationResult.Success()
                                                                                        : TLSValidationResult.Failed("Not a client certificate of this test!"),
                          ClientCertificateRequired:   true
                      );

            server.AddOrUpdateHTTPBasicAuth(CS001, password);
            server.AddOrUpdateHTTPBasicAuth(CS002, password);

            server.Start();

        }

        #endregion

        #region (private) Upgrade(PathAndQuery, NetworkingNodeId, Authorization = null)

        /// <summary>
        /// Ask for an upgrade over TLS with the certificate of the given
        /// networking node.
        /// </summary>
        private Task<RawWebSocketClient> Upgrade(String             PathAndQuery,
                                                 NetworkingNode_Id  NetworkingNodeId,
                                                 String?            Authorization   = null)

            => RawWebSocketClient.Upgrade(
                   port,
                   PathAndQuery,
                   Authorization,
                   ServerCertificate:  serverCertificate,
                   ClientCertificate:  clientCertificates[NetworkingNodeId]
               );

        #endregion

        #region (private) Basic(NetworkingNodeId)

        private String Basic(NetworkingNode_Id NetworkingNodeId)
            => HTTPBasicAuthentication.Create(NetworkingNodeId.ToString(), password).HTTPText;

        #endregion

        #region (private) ConnectionOf(Client)

        /// <summary>
        /// The server's end of the given client's connection.
        /// </summary>
        private org.GraphDefined.Vanaheimr.Hermod.WebSocket.WebSocketServerConnection? ConnectionOf(RawWebSocketClient Client)

            => server!.WebSocketConnections.SingleOrDefault(connection => connection.RemoteSocket.Port.ToUInt16() == Client.LocalPort);

        #endregion

        #region (private) RegisteredAs(NetworkingNodeId)

        /// <summary>
        /// Whether the server's registered networking nodes become exactly the
        /// given one within two seconds.
        /// </summary>
        private async Task<Boolean> RegisteredAs(NetworkingNode_Id NetworkingNodeId)
        {

            var deadline = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(2);

            while (!server!.ConnectedNetworkingNodeIds.SequenceEqual([ NetworkingNodeId ]))
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
