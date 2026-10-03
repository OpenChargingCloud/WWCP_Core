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
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// A WebSocket client of no more than a TCP connection, which says exactly
    /// what it is told to: the path, the query, the Authorization header, the
    /// client certificate - and which ends its connection exactly as it is told
    /// to, with a close frame, with a FIN, or with a reset.
    /// </summary>
    internal sealed class RawWebSocketClient : IDisposable
    {

        #region Data

        private readonly TcpClient  tcp;
        private readonly Stream     stream;

        #endregion

        #region Properties

        /// <summary>
        /// The status code the server answered the upgrade with.
        /// </summary>
        public Int32   StatusCode    { get; }

        /// <summary>
        /// The local TCP port of this client, which is the remote port of its
        /// connection on the server.
        /// </summary>
        public UInt16  LocalPort     { get; }

        #endregion

        #region Constructor(s)

        private RawWebSocketClient(TcpClient  TCP,
                                   Stream     Stream,
                                   Int32      StatusCode)
        {

            this.tcp         = TCP;
            this.stream      = Stream;
            this.StatusCode  = StatusCode;
            this.LocalPort   = (UInt16) ((IPEndPoint) TCP.Client.LocalEndPoint!).Port;

        }

        #endregion


        #region (static) Upgrade(Port, PathAndQuery, Authorization = null, Subprotocol = "ocpp2.1", ServerCertificate = null, ClientCertificate = null)

        /// <summary>
        /// Ask the server on the given port for an upgrade, and read its answer.
        /// </summary>
        /// <param name="Port">The TCP port of the server.</param>
        /// <param name="PathAndQuery">The request target, e.g. "/CS001" or "/CS001?u=CS001".</param>
        /// <param name="Authorization">The value of an optional Authorization header.</param>
        /// <param name="Subprotocol">The WebSocket subprotocol to offer.</param>
        /// <param name="ServerCertificate">The certificate the server has to show over TLS; without one, no TLS.</param>
        /// <param name="ClientCertificate">An optional client certificate to show the server over TLS.</param>
        public static async Task<RawWebSocketClient> Upgrade(IPPort             Port,
                                                             String             PathAndQuery,
                                                             String?            Authorization       = null,
                                                             String             Subprotocol         = "ocpp2.1",
                                                             X509Certificate2?  ServerCertificate   = null,
                                                             X509Certificate2?  ClientCertificate   = null)
        {

            var tcp = new TcpClient();

            await tcp.ConnectAsync(System.Net.IPAddress.Loopback, Port.ToUInt16());

            Stream stream = tcp.GetStream();

            if (ServerCertificate is not null)
            {

                var tls = new SslStream(stream);

                // The server is the one whose certificate the test made, and no
                // other: it is known by its thumbprint, not by a chain to a root.
                await tls.AuthenticateAsClientAsync(
                          new SslClientAuthenticationOptions {
                              TargetHost                           = "localhost",
                              ClientCertificates                   = ClientCertificate is not null ? [ ClientCertificate ] : null,
                              LocalCertificateSelectionCallback    = ClientCertificate is not null ? (_, _, _, _, _) => ClientCertificate : null,
                              RemoteCertificateValidationCallback  = (_, certificate, _, _) => certificate is not null &&
                                                                                                certificate.GetCertHashString() == ServerCertificate.GetCertHashString()
                          }
                      );

                stream = tls;

            }

            var request  = new StringBuilder();

            request.Append($"GET {PathAndQuery} HTTP/1.1\r\n");
            request.Append($"Host: 127.0.0.1:{Port}\r\n");
            request.Append( "Upgrade: websocket\r\n");
            request.Append( "Connection: Upgrade\r\n");
            request.Append($"Sec-WebSocket-Key: {Convert.ToBase64String(RandomNumberGenerator.GetBytes(16))}\r\n");
            request.Append( "Sec-WebSocket-Version: 13\r\n");
            request.Append($"Sec-WebSocket-Protocol: {Subprotocol}\r\n");

            if (Authorization is not null)
                request.Append($"Authorization: {Authorization}\r\n");

            request.Append("\r\n");

            await stream.WriteAsync(Encoding.ASCII.GetBytes(request.ToString()));

            return new RawWebSocketClient(
                       tcp,
                       stream,
                       await ReadStatusCode(stream)
                   );

        }

        #endregion

        #region SendClose(StatusCode = 1000)

        /// <summary>
        /// Send a close frame, masked as a client must, and leave the connection
        /// to the server to end.
        /// </summary>
        public async Task SendClose(UInt16 StatusCode = 1000)
        {

            var payload  = new[] { (Byte) (StatusCode >> 8), (Byte) (StatusCode & 0xff) };
            var mask     = RandomNumberGenerator.GetBytes(4);
            var frame    = new Byte[2 + 4 + payload.Length];

            frame[0] = 0x88;                          // FIN + close
            frame[1] = (Byte) (0x80 | payload.Length); // masked

            Array.Copy(mask, 0, frame, 2, 4);

            for (var i = 0; i < payload.Length; i++)
                frame[6 + i] = (Byte) (payload[i] ^ mask[i % 4]);

            await stream.WriteAsync(frame);

        }

        #endregion

        #region HangUp()

        /// <summary>
        /// End the TCP connection with a FIN and without a close frame.
        /// </summary>
        public void HangUp()
            => tcp.Close();

        #endregion

        #region Abort()

        /// <summary>
        /// End the TCP connection with a reset and without a close frame.
        /// </summary>
        public void Abort()
        {
            tcp.Client.LingerState = new LingerOption(true, 0);
            tcp.Close();
        }

        #endregion

        #region Dispose()

        public void Dispose()
            => tcp.Dispose();

        #endregion


        #region (private static) ReadStatusCode(Stream)

        /// <summary>
        /// Read the answer's head, one octet at a time, so that nothing after
        /// it is taken away from whoever reads the frames.
        /// </summary>
        private static async Task<Int32> ReadStatusCode(Stream Stream)
        {

            using var timeout  = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var       head     = new StringBuilder();
            var       octet    = new Byte[1];

            while (!head.ToString().EndsWith("\r\n\r\n"))
            {

                if (await Stream.ReadAsync(octet, timeout.Token) == 0)
                    break;

                head.Append((Char) octet[0]);

            }

            // "HTTP/1.1 101 Switching Protocols"
            var statusLine = head.ToString().Split("\r\n")[0].Split(' ');

            return statusLine.Length > 1 && Int32.TryParse(statusLine[1], out var statusCode)
                       ? statusCode
                       : 0;

        }

        #endregion

    }

}
