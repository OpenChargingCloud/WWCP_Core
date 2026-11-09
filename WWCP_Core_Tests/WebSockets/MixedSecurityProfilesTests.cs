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
    /// Security profiles 2 and 3 on one port: a station proves itself with an
    /// HTTP password over TLS, or with its TLS client certificate alone - and a
    /// station that is to show its certificate is not let in with a password.
    /// </summary>
    /// <remarks>
    /// A port of its own per profile, as OCPP allows, was possible before:
    /// authentication off and a client certificate required. A port that
    /// required authentication took HTTP Basic and TOTP only, and answered a
    /// station with nothing but its certificate 401 - so a CSMS moving its
    /// stations from profile 2 to profile 3 one by one needed a second endpoint
    /// for each. Here the server asks every client for a certificate and takes
    /// one that has none as well; whether a certificate is a must is said per
    /// networking node, so that a password copied off a profile 3 station opens
    /// nothing.
    /// </remarks>
    [TestFixture]
    public class MixedSecurityProfilesTests
    {

        #region (class) Node

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
        private static readonly  NetworkingNode_Id     CS003     = NetworkingNode_Id.Parse("CS003");

        private readonly         String                password  = $"pw-{Guid.NewGuid():N}";

        private                  X509Certificate2?     serverCertificate;
        private readonly         Dictionary<NetworkingNode_Id, X509Certificate2>  clientCertificates  = [];

        private                  WWCPWebSocketServer?  server;
        private                  IPPort                port;

        #endregion

        #region OneTimeSetUp() / OneTimeTearDown() / SetUp() / TearDown()

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

            serverCertificate = TestCertificates.Server();

            foreach (var networkingNodeId in new[] { CS001, CS002, CS003 })
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

        /// <summary>
        /// A port that requires authentication, asks every client for a
        /// certificate and takes one without as well - and knows a password for
        /// each of the three stations, of which CS003 must show its certificate.
        /// </summary>
        [SetUp]
        public async Task Start()
        {

            port    = TestPorts.Free();

            server  = new WWCPWebSocketServer(
                          new Node(),
                          HTTPPort:                    port,
                          RequireAuthentication:       true,
                          SecWebSocketProtocols:       [ "ocpp2.1" ],
                          DisableWebSocketPings:       true,
                          ServerCertificateSelector:   (_, _) => serverCertificate!,
                          ClientCertificateValidator:  (_, certificate, _, _, _) => certificate is null ||
                                                                                    clientCertificates.Values.Any(clientCertificate => clientCertificate.GetCertHashString() == certificate.GetCertHashString())
                                                                                        ? TLSValidationResult.Success()
                                                                                        : TLSValidationResult.Failed("Not a client certificate of this test!"),
                          ClientCertificateRequired:   true
                      );

            server.AddOrUpdateHTTPBasicAuth(CS001, password);
            server.AddOrUpdateHTTPBasicAuth(CS002, password);
            server.AddOrUpdateHTTPBasicAuth(CS003, password);

            server.RequireACertificateOf(CS003);

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


        #region AStationWithItsCertificateAloneIsLetIn()

        /// <summary>
        /// Security profile 3: the certificate, and no password.
        /// </summary>
        [Test]
        public async Task AStationWithItsCertificateAloneIsLetIn()
        {

            using var client      = await Upgrade($"/{CS001}", Certificate: CS001);

            var       connection  = server!.ConnectedNetworkingNodes.SingleOrDefault()?.WebSocketServerConnections.SingleOrDefault();

            Assert.Multiple(() => {

                Assert.That(client.StatusCode,                 Is.EqualTo(101),
                            "A station with nothing but its client certificate was not let in.");

                Assert.That(server.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { CS001 }));

                Assert.That(connection?.Login,                 Is.EqualTo(CS001.ToString()),
                            "The connection is not logged in as the networking node its certificate names.");

            });

        }

        #endregion

        #region AStationWithItsPasswordAloneIsLetIn()

        /// <summary>
        /// Security profile 2, on the same port: a password over TLS, and no
        /// certificate.
        /// </summary>
        [Test]
        public async Task AStationWithItsPasswordAloneIsLetIn()
        {

            using var client = await Upgrade($"/{CS002}", Authorization: Basic(CS002));

            Assert.Multiple(() => {

                Assert.That(client.StatusCode,                  Is.EqualTo(101),
                            "A station with its password and no certificate was not let in.");

                Assert.That(server!.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { CS002 }));

            });

        }

        #endregion

        #region AStationThatMustShowItsCertificateIsNotLetInWithAPassword()

        /// <summary>
        /// CS003 is a profile 3 station: its password, right as it is, opens
        /// nothing - which is what makes a password copied off it worthless.
        /// </summary>
        [Test]
        public async Task AStationThatMustShowItsCertificateIsNotLetInWithAPassword()
        {

            using var client = await Upgrade($"/{CS003}", Authorization: Basic(CS003));

            Assert.Multiple(() => {

                Assert.That(client.StatusCode,                  Is.EqualTo(401),
                            "A station that must show its certificate was let in with a password.");

                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);

            });

        }

        #endregion

        #region AStationThatMustShowItsCertificateIsLetInWithIt()

        [Test]
        public async Task AStationThatMustShowItsCertificateIsLetInWithIt()
        {

            using var client = await Upgrade($"/{CS003}", Certificate: CS003);

            Assert.Multiple(() => {

                Assert.That(client.StatusCode,                  Is.EqualTo(101),
                            "A station that must show its certificate was not let in with it.");

                Assert.That(server!.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { CS003 }));

            });

        }

        #endregion

        #region AStationsCertificateOpensNoConnectionAsAnotherStation()

        /// <summary>
        /// The certificate is checked against the path as before: CS001's opens
        /// nothing as CS002.
        /// </summary>
        [Test]
        public async Task AStationsCertificateOpensNoConnectionAsAnotherStation()
        {

            using var client = await Upgrade($"/{CS002}", Certificate: CS001);

            Assert.Multiple(() => {

                Assert.That(client.StatusCode,                  Is.EqualTo(403));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);

            });

        }

        #endregion

        #region NeitherACertificateNorAPasswordIsNotLetIn()

        [Test]
        public async Task NeitherACertificateNorAPasswordIsNotLetIn()
        {

            using var client = await Upgrade($"/{CS001}");

            Assert.Multiple(() => {

                Assert.That(client.StatusCode,                  Is.EqualTo(401));
                Assert.That(server!.ConnectedNetworkingNodeIds, Is.Empty);

            });

        }

        #endregion

        #region ACertificateIsAMustUntilItIsNoLonger()

        /// <summary>
        /// The rule is taken back as it is given: CS003's password works again.
        /// </summary>
        [Test]
        public async Task ACertificateIsAMustUntilItIsNoLonger()
        {

            Assert.That(server!.MustShowACertificate(CS003), Is.True);

            server.NoLongerRequireACertificateOf(CS003);

            using var client = await Upgrade($"/{CS003}", Authorization: Basic(CS003));

            Assert.Multiple(() => {

                Assert.That(server.MustShowACertificate(CS003), Is.False);
                Assert.That(client.StatusCode,                  Is.EqualTo(101));

            });

        }

        #endregion


        #region (private) Upgrade(PathAndQuery, Authorization = null, Certificate = null)

        /// <summary>
        /// Ask for an upgrade over TLS, with the given Authorization header and
        /// the client certificate of the given networking node, where given.
        /// </summary>
        private Task<RawWebSocketClient> Upgrade(String              PathAndQuery,
                                                 String?             Authorization   = null,
                                                 NetworkingNode_Id?  Certificate     = null)

            => RawWebSocketClient.Upgrade(
                   port,
                   PathAndQuery,
                   Authorization,
                   ServerCertificate:  serverCertificate,
                   ClientCertificate:  Certificate.HasValue ? clientCertificates[Certificate.Value] : null
               );

        #endregion

        #region (private) Basic(NetworkingNodeId)

        private String Basic(NetworkingNode_Id NetworkingNodeId)
            => HTTPBasicAuthentication.Create(NetworkingNodeId.ToString(), password).HTTPText;

        #endregion

    }

}
