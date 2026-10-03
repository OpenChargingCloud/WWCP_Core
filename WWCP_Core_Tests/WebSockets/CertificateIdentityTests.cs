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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// Which networking node the WebSocket server takes a connection over TLS
    /// for, when the client shows a certificate: the one named by the last
    /// segment of its path - and not with the certificate of another one, unless
    /// that one may connect as it. The certificate names its networking node by
    /// its common name.
    /// </summary>
    /// <remarks>
    /// The server asks every client for a certificate and lets in those it made
    /// for the test, as OCPP security profile 3 has it. It requires no
    /// authentication of its own, as the CSMS does: its HTTP Basic check knows
    /// nothing of certificates.
    /// </remarks>
    [TestFixture]
    public class CertificateIdentityTests
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

        private static readonly  NetworkingNode_Id     CS001     = NetworkingNode_Id.Parse("CS001");
        private static readonly  NetworkingNode_Id     CS002     = NetworkingNode_Id.Parse("CS002");
        private static readonly  NetworkingNode_Id     LC001     = NetworkingNode_Id.Parse("LC001");

        private readonly         String                password  = $"pw-{Guid.NewGuid():N}";

        private                  X509Certificate2?     serverCertificate;
        private readonly         Dictionary<NetworkingNode_Id, X509Certificate2>  clientCertificates  = [];

        private                  WWCPWebSocketServer?  server;
        private                  IPPort                port;

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


        #region AStationIsWhoItsPathAndItsCertificateSay()

        [Test]
        public async Task AStationIsWhoItsPathAndItsCertificateSay()
        {

            await Start();

            using var client      = await Upgrade($"/{CS001}", CS001);

            var       connection  = server!.ConnectedNetworkingNodes.SingleOrDefault()?.WebSocketServerConnections.SingleOrDefault();

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(101));
                Assert.That(server.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { CS001 }));
                Assert.That(connection?.ClientCertificate?.GetCertHashString(), Is.EqualTo(clientCertificates[CS001].GetCertHashString()),
                            "The connection does not have the client certificate.");
            });

        }

        #endregion

        #region AStationsCertificateOpensNoConnectionAsAnotherStation()

        /// <summary>
        /// CS002, with its certificate, asks to be let in as CS001.
        /// </summary>
        [Test]
        public async Task AStationsCertificateOpensNoConnectionAsAnotherStation()
        {

            await Start();

            using var client = await Upgrade($"/{CS001}", CS002);

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(403));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);
            });

        }

        #endregion

        #region ALocalControllersCertificateConnectsAsAStationItMayActFor()

        /// <summary>
        /// A local controller connects for a station behind it with the
        /// station's path and its own certificate (OCPP 2.1 Part 4, 6.5).
        /// </summary>
        [Test]
        public async Task ALocalControllersCertificateConnectsAsAStationItMayActFor()
        {

            await Start();

            server!.AllowToActFor(LC001, [ CS001 ]);

            using var client      = await Upgrade($"/{CS001}", LC001);

            var       connection  = server.ConnectedNetworkingNodes.SingleOrDefault()?.WebSocketServerConnections.SingleOrDefault();

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(101));
                Assert.That(server.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { CS001 }));
                Assert.That(connection?.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.ActingNetworkingNodeId), Is.EqualTo(LC001),
                            "The connection does not say that the local controller opened it.");
            });

        }

        #endregion

        #region ALocalControllersCertificateConnectsAsNoStationItMayNotActFor()

        [Test]
        public async Task ALocalControllersCertificateConnectsAsNoStationItMayNotActFor()
        {

            await Start();

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

            await Start();

            using var client = await Upgrade("/", LC001);

            Assert.Multiple(() => {
                Assert.That(client.StatusCode,                 Is.EqualTo(101));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { LC001 }));
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

            await Start();

            using var asAnotherStation  = await Upgrade($"/{CS001}", CS002, Basic(CS001));
            using var asItself          = await Upgrade($"/{CS001}", CS001, Basic(CS002));

            Assert.Multiple(() => {
                Assert.That(asAnotherStation.StatusCode,       Is.EqualTo(403));
                Assert.That(asItself.        StatusCode,       Is.EqualTo(101));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { CS001 }));
            });

        }

        #endregion


        #region (private) Start()

        private async Task Start()
        {

            port    = TestPorts.Free();

            server  = new WWCPWebSocketServer(
                          new Node(),
                          HTTPPort:                    port,
                          RequireAuthentication:       false,
                          SecWebSocketProtocols:       [ "ocpp2.1" ],
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

            await server.Start();

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

    }

}
