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
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking.tests
{

    /// <summary>
    /// The overlay WebSocket server says in its debug log who asked to be let
    /// in, and never with what password, whether the password was right or not.
    /// </summary>
    /// <remarks>
    /// Whether it then lets anybody in is not asked here.
    /// </remarks>
    [TestFixture]
    public class SecretsInTheLogTests
    {

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

        private const     String                   station   = "CS001";

        private readonly  String                   password  = $"pw-{Guid.NewGuid():N}";

        private           OverlayWebSocketServer?  server;
        private           UInt16                   port;
        private           Log?                     log;

        #endregion

        #region SetUp() / TearDown()

        [SetUp]
        public void SetUp()
        {

            port    = FreePort();

            server  = new OverlayWebSocketServer(
                          NetworkingNode_Id.Parse("onn"),
                          [ "ocpp2.1" ],
                          TCPPort:                IPPort.Parse(port),
                          RequireAuthentication:  true,
                          DisableWebSocketPings:  true
                      );

            server.AddOrUpdateHTTPBasicAuth(NetworkingNode_Id.Parse(station), password);
            server.Start();

            log     = new Log();
            Trace.Listeners.Add(log);

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
                await server.Shutdown();

            log    = null;
            server = null;

        }

        #endregion


        #region TheRightPasswordIsNotLogged()

        [Test]
        public async Task TheRightPasswordIsNotLogged()
        {

            await Upgrade(HTTPBasicAuthentication.Create(station, password).HTTPText);

            Assert.That(log!.Text, Does.Not.Contain(password),
                        "The station's password is in the log.");

        }

        #endregion

        #region AWrongPasswordIsNotLogged()

        [Test]
        public async Task AWrongPasswordIsNotLogged()
        {

            var wrongPassword = $"wrong-{Guid.NewGuid():N}";

            await Upgrade(HTTPBasicAuthentication.Create(station, wrongPassword).HTTPText);

            Assert.That(log!.Text, Does.Not.Contain(wrongPassword),
                        "The wrong password is in the log.");

        }

        #endregion


        #region (private) Upgrade(Authorization)

        /// <summary>
        /// Ask the server for an upgrade with the given Authorization header,
        /// and wait for its answer, whatever it is.
        /// </summary>
        private async Task Upgrade(String Authorization)
        {

            using var tcp     = new TcpClient();
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await tcp.ConnectAsync(System.Net.IPAddress.Loopback, port, timeout.Token);

            var stream = tcp.GetStream();

            await stream.WriteAsync(
                      Encoding.ASCII.GetBytes(
                          $"GET /{station} HTTP/1.1\r\n" +
                          $"Host: 127.0.0.1:{port}\r\n" +
                           "Upgrade: websocket\r\n" +
                           "Connection: Upgrade\r\n" +
                          $"Sec-WebSocket-Key: {Convert.ToBase64String(RandomNumberGenerator.GetBytes(16))}\r\n" +
                           "Sec-WebSocket-Version: 13\r\n" +
                           "Sec-WebSocket-Protocol: ocpp2.1\r\n" +
                          $"Authorization: {Authorization}\r\n" +
                           "\r\n"
                      ),
                      timeout.Token
                  );

            // The answer's first octet is enough: by then the server has decided.
            await stream.ReadAtLeastAsync(new Byte[1], 1, throwOnEndOfStream: false, timeout.Token);

        }

        #endregion

        #region (private static) FreePort()

        private static UInt16 FreePort()
        {

            var listener = new TcpListener(System.Net.IPAddress.Loopback, 0);

            listener.Start();

            try
            {
                return (UInt16) ((IPEndPoint) listener.LocalEndpoint).Port;
            }
            finally
            {
                listener.Stop();
            }

        }

        #endregion

    }

}
