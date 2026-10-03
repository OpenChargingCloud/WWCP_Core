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

using System.Diagnostics;
using System.Text;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// The WebSocket server says in its debug log who asked to be let in, and
    /// never with what: no password and no one-time password, whether the
    /// client was let in or not.
    /// </summary>
    /// <remarks>
    /// The debug log is DebugX, which writes to Debug, and Debug writes to
    /// the trace listeners. Where it writes nothing at all - a release build
    /// of Styx - these tests have nothing to look at, and say so.
    /// </remarks>
    [TestFixture]
    public class SecretsInTheLogTests
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

        #region (class) Log

        /// <summary>
        /// Everything written to the trace listeners while a test runs.
        /// </summary>
        private sealed class Log : TraceListener
        {

            private readonly StringBuilder text = new();

            public override void Write    (String? Message) { lock (text) text.Append    (Message); }
            public override void WriteLine(String? Message) { lock (text) text.AppendLine(Message); }

            public String Text
            {
                get
                {
                    lock (text)
                        return text.ToString();
                }
            }

        }

        #endregion

        #region Data

        private const     String               station       = "CS001";

        private readonly  String               password      = $"pw-{Guid.NewGuid():N}";
        private readonly  String               sharedSecret  = $"totp-{Guid.NewGuid():N}";

        private           WWCPWebSocketServer? server;
        private           IPPort               port;
        private           Log?                 log;

        #endregion

        #region SetUp() / TearDown()

        [SetUp]
        public async Task SetUp()
        {

            port    = TestPorts.Free();

            server  = new WWCPWebSocketServer(
                          new Node(),
                          HTTPPort:               port,
                          RequireAuthentication:  true,
                          SecWebSocketProtocols:  [ "ocpp2.1" ],
                          DisableWebSocketPings:  true
                      );

            server.AddOrUpdateHTTPBasicAuth(NetworkingNode_Id.Parse(station), password);
            server.ClientTOTPConfig[station] = new TOTPConfig(sharedSecret);

            await server.Start();

            log     = new Log();
            Trace.Listeners.Add(log);

            // Whatever the server writes from here on is in the log - unless
            // DebugX writes nothing at all.
            var sentinel = $"sentinel-{Guid.NewGuid():N}";
            DebugX.Log(sentinel);

            Assume.That(log.Text, Does.Contain(sentinel),
                        "DebugX writes nothing to the trace listeners here, so there is no log to look at.");

        }

        [TearDown]
        public async Task TearDown()
        {

            if (log is not null)
            {
                Trace.Listeners.Remove(log);
                log.Dispose();
            }

            if (server is not null)
            {
                await server.Shutdown();
                await server.DisposeAsync();
            }

            log    = null;
            server = null;

        }

        #endregion


        #region ABasicPasswordThatLetsAStationInIsNotLogged()

        [Test]
        public async Task ABasicPasswordThatLetsAStationInIsNotLogged()
        {

            using var client = await RawWebSocketClient.Upgrade(
                                         port,
                                         $"/{station}",
                                         HTTPBasicAuthentication.Create(station, password).HTTPText
                                     );

            Assert.Multiple(() => {
                Assert.That(client.StatusCode, Is.EqualTo(101));
                Assert.That(log!.Text,         Does.Not.Contain(password),
                            "The password that let the station in is in the log.");
            });

        }

        #endregion

        #region ABasicPasswordThatKeepsAStationOutIsNotLogged()

        /// <summary>
        /// A wrong password is still somebody's password - often the right one
        /// of another account, or the right one mistyped.
        /// </summary>
        [Test]
        public async Task ABasicPasswordThatKeepsAStationOutIsNotLogged()
        {

            var wrongPassword = $"wrong-{Guid.NewGuid():N}";

            using var client = await RawWebSocketClient.Upgrade(
                                         port,
                                         $"/{station}",
                                         HTTPBasicAuthentication.Create(station, wrongPassword).HTTPText
                                     );

            Assert.Multiple(() => {
                Assert.That(client.StatusCode, Is.EqualTo(401));
                Assert.That(log!.Text,         Does.Not.Contain(wrongPassword),
                            "The password that kept the station out is in the log.");
            });

        }

        #endregion

        #region AOneTimePasswordThatLetsAStationInIsNotLogged()

        /// <summary>
        /// A one-time password is good for the rest of its time slot and the
        /// next one, which is long enough to be used again by whoever reads it.
        /// </summary>
        [Test]
        public async Task AOneTimePasswordThatLetsAStationInIsNotLogged()
        {

            var totp = TOTPGenerator.GenerateTOTP(sharedSecret).Current;

            using var client = await RawWebSocketClient.Upgrade(
                                         port,
                                         $"/{station}",
                                         HTTPTOTPAuthentication.Create(station, totp, TOTPHTTPHeaderType.RAW).HTTPText
                                     );

            Assert.Multiple(() => {
                Assert.That(client.StatusCode, Is.EqualTo(101));
                Assert.That(log!.Text,         Does.Not.Contain(totp),
                            "The one-time password that let the station in is in the log.");
            });

        }

        #endregion

        #region AOneTimePasswordThatKeepsAStationOutIsNotLogged()

        /// <summary>
        /// The station's valid one-time password, sent for a login the server
        /// does not know.
        /// </summary>
        [Test]
        public async Task AOneTimePasswordThatKeepsAStationOutIsNotLogged()
        {

            var totp = TOTPGenerator.GenerateTOTP(sharedSecret).Current;

            using var client = await RawWebSocketClient.Upgrade(
                                         port,
                                         $"/{station}",
                                         HTTPTOTPAuthentication.Create("CS002", totp, TOTPHTTPHeaderType.RAW).HTTPText
                                     );

            Assert.Multiple(() => {
                Assert.That(client.StatusCode, Is.EqualTo(401));
                Assert.That(log!.Text,         Does.Not.Contain(totp),
                            "The one-time password that kept the station out is in the log.");
            });

        }

        #endregion

        #region APasswordInTheQueryStringIsNotLogged()

        [Test]
        public async Task APasswordInTheQueryStringIsNotLogged()
        {

            using var client = await RawWebSocketClient.Upgrade(
                                         port,
                                         $"/{station}?u={station}&p={password}"
                                     );

            Assert.That(log!.Text, Does.Not.Contain(password),
                        "The password from the query string is in the log.");

        }

        #endregion

    }

}
