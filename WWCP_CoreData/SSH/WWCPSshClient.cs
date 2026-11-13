/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SSH;
using org.GraphDefined.Vanaheimr.Hermod.SSH.Client;
using org.GraphDefined.Vanaheimr.Hermod.Messaging;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.WebSockets;
using cloud.charging.open.protocols.WWCP.NetworkingNode;

using WWCPNetworkingMode = cloud.charging.open.protocols.WWCP.NetworkingNode.NetworkingMode;

#endregion

namespace cloud.charging.open.protocols.WWCP.SSH
{

    /// <summary>
    /// A connection of a networking node to another one over an SSH subsystem - as
    /// <see cref="WWCPWebSocketClient"/> is over an HTTP WebSocket. It signs in, asks for the subsystem, and
    /// says HELLO; where it has a reconnect policy, it connects again after the connection ended.
    /// </summary>
    public class WWCPSshClient : IWWCPClient,
                                 IAsyncDisposable
    {

        #region Data

        private readonly CancellationTokenSource  stopping  = new ();
        private          Int32                    retrying;

        #endregion

        #region Properties

        /// <summary>
        /// The networking node connecting.
        /// </summary>
        public INetworkingNode          NetworkingNode          { get; }

        /// <summary>
        /// The message protocol's client.
        /// </summary>
        public SshMessageClient         MessageClient           { get; }

        /// <summary>
        /// An optional description.
        /// </summary>
        public I18NString?              Description             { get; }

        /// <summary>
        /// Whether messages carry their network path (an overlay network) or not. Told to the server in the
        /// HELLO, where it is an overlay network.
        /// </summary>
        public NetworkingMode           NetworkingMode          { get; }

        /// <summary>
        /// The networking node the HELLO says it connects as, where that is not the SSH login.
        /// </summary>
        public NetworkingNode_Id?       ConnectAs               { get; }

        /// <summary>
        /// The subsystem asked for.
        /// </summary>
        public String                   Subsystem
            => MessageClient.Options.Subsystem;

        /// <summary>
        /// The SSH login.
        /// </summary>
        public String                   Username
            => MessageClient.Options.Ssh?.Username ?? MessageClient.Connection?.Username ?? "";

        /// <summary>
        /// Messages go over an SSH channel.
        /// </summary>
        public MessageTransport         Transport
            => MessageTransport.SSH;

        /// <summary>
        /// Where the client connects to: "ssh://cs001@lc.example.org:22 ocpp@open.charging.cloud".
        /// </summary>
        public String                   TransportDescription
            => $"ssh://{Username}@{MessageClient.Options.Host}:{MessageClient.Options.Port} {Subsystem}";

        /// <summary>
        /// The connection made last - which may have ended since.
        /// </summary>
        public SshMessageConnection?    Connection
            => MessageClient.Connection;

        /// <summary>
        /// Whether the client has a connection that lasts.
        /// </summary>
        public Boolean                  IsConnected
            => MessageClient.IsConnected;

        /// <summary>
        /// How JSON messages are written.
        /// </summary>
        public Formatting               JSONFormatting          { get; set; }
            = Formatting.None;

        #endregion

        #region Events

        /// <summary>
        /// A connection was made - the first one, and every reconnect - before its first message is read.
        /// </summary>
        public event OnWWCPSshConnectionDelegate?          OnConnected;

        /// <summary>
        /// A connection ended.
        /// </summary>
        public event OnWWCPSshConnectionClosedDelegate?    OnConnectionClosed;

        /// <summary>
        /// A JSON message was received.
        /// </summary>
        public event OnWWCPJSONMessageReceivedDelegate?    OnJSONMessageReceived;

        /// <summary>
        /// A binary message was received.
        /// </summary>
        public event OnWWCPBinaryMessageReceivedDelegate?  OnBinaryMessageReceived;

        /// <summary>
        /// A JSON message was sent, or failed to be.
        /// </summary>
        public event OnWWCPJSONMessageSentDelegate?        OnJSONMessageSent;

        /// <summary>
        /// A binary message was sent, or failed to be.
        /// </summary>
        public event OnWWCPBinaryMessageSentDelegate?      OnBinaryMessageSent;

        /// <summary>
        /// An attempt to connect failed, and is tried again or not: every one the client makes by itself -
        /// before its first connection, and after a connection ended. The failure of <see cref="Connect"/>
        /// itself is its caller's, and thrown.
        /// </summary>
        public event OnWWCPSshConnectFailedDelegate?       OnConnectFailed;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// A client of the networking node at the other end of the given SSH subsystem.
        /// </summary>
        /// <param name="NetworkingNode">The networking node connecting - whose DNS client resolves the server's host name where the options name none.</param>
        /// <param name="Options">Where and how to connect: host, port, login, subsystem, subprotocols, reconnects.</param>
        /// <param name="NetworkingMode">Whether messages carry their network path; standard by default.</param>
        /// <param name="ConnectAs">The networking node to connect as, where that is not the SSH login.</param>
        /// <param name="Description">An optional description.</param>
        public WWCPSshClient(INetworkingNode           NetworkingNode,
                             SshMessageClientOptions   Options,
                             NetworkingMode?           NetworkingMode   = null,
                             NetworkingNode_Id?        ConnectAs        = null,
                             I18NString?               Description      = null)
        {

            this.NetworkingNode  = NetworkingNode;
            this.NetworkingMode  = NetworkingMode ?? WWCPNetworkingMode.Standard;
            this.ConnectAs       = ConnectAs;
            this.Description     = Description;

            var hello = Options.HelloProperties?.DeepClone() as JObject ?? new JObject();

            if (ConnectAs.HasValue)
                hello[WWCPSshMessaging.HelloNetworkingNodeId] = ConnectAs.Value.ToString();

            // As the WebSocket transport sends its X-OCPP-NetworkingMode header: for an overlay network only.
            if (this.NetworkingMode == WWCPNetworkingMode.OverlayNetwork)
                hello[WWCPSshMessaging.HelloNetworkingMode]   = this.NetworkingMode.ToString();

            // The node's DNS client, where the options name none: a host name is never resolved by the
            // operating system, and a node that resolves its names securely is not dialled around them.
            var dnsClient        = Options.Ssh?.DNSClient ?? (NetworkingNode as AWWCPNetworkingNode)?.DNSClient;

            this.MessageClient   = new SshMessageClient(
                                       Options with {
                                           Ssh              = Options.Ssh is SshClientOptions login
                                                                  ? login with { DNSClient = dnsClient }
                                                                  : null,
                                           HelloProperties  = hello.Count > 0 ? hello : null
                                       }
                                   );

            // After a connection ended: the message client tries again by itself, and says so here.
            MessageClient.OnConnectFailed          += (timestamp, sender, error, attempt, nextAttemptIn, cancellationToken) =>
                                                          RaiseConnectFailed(error, attempt, nextAttemptIn);

            MessageClient.OnNewConnection          += (timestamp, sender, connection, cancellationToken) => {

                // Before anyone else hears of it, and before its first message is read: whoever reads one
                // learns from the connection which client it belongs to.
                connection.TryAddCustomData(WWCPConnectionExtensions.WWCPClientKey, this);

                return WWCPSshMessaging.RaiseAll(
                           OnConnected,
                           handler => handler(
                                          timestamp,
                                          this,
                                          connection,
                                          connection.NetworkingNodeIdOf(),
                                          this.NetworkingMode,
                                          EventTracking_Id.New,
                                          cancellationToken
                                      ),
                           nameof(WWCPSshClient)
                       );

            };

            MessageClient.OnTextMessageReceived    += (timestamp, sender, connection, text, cancellationToken) =>
                                                          WWCPSshMessaging.ReceiveText(this, connection, timestamp, text, OnJSONMessageReceived, nameof(WWCPSshClient), cancellationToken);

            MessageClient.OnBinaryMessageReceived  += (timestamp, sender, connection, bytes, cancellationToken) =>
                                                          WWCPSshMessaging.ReceiveBinary(this, connection, timestamp, bytes, OnBinaryMessageReceived, nameof(WWCPSshClient), cancellationToken);

            MessageClient.OnConnectionClosed       += (timestamp, sender, connection, close, cancellationToken) =>
                                                          WWCPSshMessaging.RaiseAll(
                                                              OnConnectionClosed,
                                                              handler => handler(
                                                                             timestamp,
                                                                             this,
                                                                             connection,
                                                                             connection.NetworkingNodeIdOf(),
                                                                             close,
                                                                             cancellationToken
                                                                         ),
                                                              nameof(WWCPSshClient)
                                                          );

        }

        #endregion


        #region (static) OptionsFrom(RemoteURL, Ssh, Subsystem, Subprotocols = null, Reconnect = null, PingEvery = null, DNSClient = null)

        /// <summary>
        /// Where to connect, from an "ssh://" URL: its host, its port (22 by default), and its login - which
        /// must be the one of the given SSH options, where the URL has one.
        /// </summary>
        /// <param name="RemoteURL">An "ssh://" URL, e.g. "ssh://cs001@lc.example.org:2222".</param>
        /// <param name="Ssh">The SSH login: user name, credentials, how the host key is checked.</param>
        /// <param name="Subsystem">The subsystem to ask for.</param>
        /// <param name="Subprotocols">The subprotocols to offer, by preference.</param>
        /// <param name="Reconnect">How to connect again after a connection ended; null to stay disconnected.</param>
        /// <param name="PingEvery">Every how long to send a PING; null for none.</param>
        /// <param name="DNSClient">The DNS client the URL's host name is resolved with, where the SSH options name none.</param>
        /// <param name="Connection">The SSH connection the messages go over, where it is not the client's own: one that others share, asked for at every connect and reconnect. The client opens its subsystem as one more channel of it, and closes that channel only. Null for a connection of the client's own.</param>
        /// <exception cref="ArgumentException">The URL is no "ssh://" URL, or names another login than the SSH options.</exception>
        public static SshMessageClientOptions OptionsFrom(URL                                             RemoteURL,
                                                          SshClientOptions                                Ssh,
                                                          String                                          Subsystem,
                                                          IEnumerable<String>?                            Subprotocols   = null,
                                                          WebSocketClientReconnectPolicy?                 Reconnect      = null,
                                                          TimeSpan?                                       PingEvery      = null,
                                                          IDNSClient?                                     DNSClient      = null,
                                                          Func<CancellationToken, ValueTask<SshClient>>?  Connection     = null)
        {

            if (RemoteURL.Scheme != URIScheme.ssh)
                throw new ArgumentException($"'{RemoteURL}' is no ssh:// URL.", nameof(RemoteURL));

            if (RemoteURL.Login is String login && login != Ssh.Username)
                throw new ArgumentException($"'{RemoteURL}' signs in as '{login}', the SSH options as '{Ssh.Username}'.", nameof(RemoteURL));

            return new SshMessageClientOptions {
                       Host          = RemoteURL.Host.ToString(),
                       Port          = RemoteURL.Port?.ToUInt16() ?? 22,
                       Ssh           = Ssh with { DNSClient = Ssh.DNSClient ?? DNSClient },
                       Connection    = Connection,
                       Subsystem     = Subsystem,
                       Subprotocols  = Subprotocols?.ToArray() ?? [],
                       Reconnect     = Reconnect,
                       PingEvery     = PingEvery
                   };

        }

        #endregion

        #region (static) ConnectAsFrom(RemoteURL)

        /// <summary>
        /// The networking node an "ssh://" URL connects as: its last path segment, as with the WebSocket
        /// transport - "ssh://lc01@csms.example.org/cs001" signs in as "lc01" and connects as "cs001". None,
        /// where the URL has no path: the login is who connects.
        /// </summary>
        /// <param name="RemoteURL">An "ssh://" URL.</param>
        public static NetworkingNode_Id? ConnectAsFrom(URL RemoteURL)
        {

            var path = RemoteURL.Path.ToString().TrimEnd('/');

            return NetworkingNode_Id.TryParse(path[(path.LastIndexOf('/') + 1)..]);

        }

        #endregion


        #region Connect(CancellationToken = default)

        /// <summary>
        /// Connect: sign in, ask for the subsystem, say HELLO, and wait for the WELCOME. A failure is thrown;
        /// with a reconnect policy, the client then goes on trying in the background, as a WebSocket client
        /// does - a charging station that starts while its local controller is down must reach it later.
        /// </summary>
        public async Task<SshMessageConnection> Connect(CancellationToken CancellationToken = default)
        {
            try
            {
                return await MessageClient.ConnectAsync(CancellationToken);
            }
            catch (Exception e) when (e is not OperationCanceledException &&
                                      e is not ObjectDisposedException    &&
                                      MessageClient.Options.Reconnect is WebSocketClientReconnectPolicy policy &&
                                     !stopping.IsCancellationRequested)
            {

                if (Interlocked.CompareExchange(ref retrying, 1, 0) == 0)
                    _ = RetryAsync(policy, RetryAfterOf(e));

                throw;

            }
        }

        #endregion

        #region Close(Code = NormalClosure, Reason = null)

        /// <summary>
        /// End the connection, if there is one, and connect no more.
        /// </summary>
        public async Task Close(MessageCloseCode  Code     = MessageCloseCode.NormalClosure,
                                String?           Reason   = null)
        {
            await stopping.CancelAsync();
            await MessageClient.Close(Code, Reason);
        }

        #endregion

        #region DisposeAsync()

        /// <summary>
        /// End the connection (going away), and connect no more.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await stopping.CancelAsync();
            await MessageClient.DisposeAsync();
        }

        #endregion

        #region (private) RetryAsync(Policy)

        /// <summary>
        /// Try to connect, as the policy says, until a connection is made - from which on the message client
        /// itself reconnects - or the client is closed, or the policy gives up. A server that refused with a
        /// "retryAfter" - "connection-exists" - is not asked again before then.
        /// </summary>
        /// <param name="Policy">How often and how soon to try again.</param>
        /// <param name="RetryAfter">How long the server that refused the first attempt said to wait, if it said.</param>
        private async Task RetryAsync(WebSocketClientReconnectPolicy  Policy,
                                      TimeSpan?                       RetryAfter   = null)
        {
            try
            {
                for (var attempt = 1u; !stopping.IsCancellationRequested; attempt++)
                {

                    try
                    {
                        await Task.Delay(Later(Policy.DelayForAttempt(attempt), RetryAfter), stopping.Token);
                        await MessageClient.ConnectAsync(stopping.Token);
                        return;
                    }
                    catch (OperationCanceledException) when (stopping.IsCancellationRequested)
                    {
                        return;
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }
                    catch (Exception e)
                    {

                        DebugX.Log($"{nameof(WWCPSshClient)} {TransportDescription}: Connecting failed (attempt {attempt}): {e.Message}");

                        RetryAfter = RetryAfterOf(e);

                        var last = Policy.MaxAttempts is UInt32 maxAttempts && attempt >= maxAttempts;

                        // Said, as the message client says it after a connection ended: before the first
                        // connection, a login the server turned away was tried again and again, and nobody
                        // heard of it.
                        await RaiseConnectFailed(e, attempt, last ? null : Later(Policy.DelayForAttempt(attempt + 1), RetryAfter)).ConfigureAwait(false);

                        if (last)
                            return;

                    }

                }
            }
            finally
            {
                Volatile.Write(ref retrying, 0);
            }
        }

        #endregion


        #region (private static) RetryAfterOf(Error) / Later(Delay, RetryAfter)

        /// <summary>
        /// How long a server that refused the HELLO said to wait - or null, where it was no such refusal.
        /// </summary>
        private static TimeSpan? RetryAfterOf(Exception Error)

            => (Error as SshMessageRefusedException ?? Error.GetBaseException() as SshMessageRefusedException)?.RetryAfter;

        /// <summary>
        /// The later of the policy's delay and the server's retryAfter.
        /// </summary>
        private static TimeSpan Later(TimeSpan   Delay,
                                      TimeSpan?  RetryAfter)

            => RetryAfter is TimeSpan after && after > Delay
                   ? after
                   : Delay;

        #endregion

        #region (private) RaiseConnectFailed(Error, Attempt, NextAttemptIn)

        private Task RaiseConnectFailed(Exception  Error,
                                        UInt32     Attempt,
                                        TimeSpan?  NextAttemptIn)

            => WWCPSshMessaging.RaiseAll(
                   OnConnectFailed,
                   handler => handler(
                                  Timestamp.Now,
                                  this,
                                  Error,
                                  Attempt,
                                  NextAttemptIn,
                                  CancellationToken.None
                              ),
                   nameof(WWCPSshClient)
               );

        #endregion

        #region SendJSONMessage   (JSONMessage,   MessageTimestamp, EventTrackingId, CancellationToken = default)

        /// <summary>
        /// Send the given JSON message.
        /// </summary>
        public Task<SentMessageResult> SendJSONMessage(JArray             JSONMessage,
                                                       DateTimeOffset     MessageTimestamp,
                                                       EventTracking_Id   EventTrackingId,
                                                       CancellationToken  CancellationToken   = default)

            => MessageClient.Connection is SshMessageConnection connection && connection.IsAlive
                   ? WWCPSshMessaging.SendJSON(this, connection, JSONMessage, JSONFormatting, MessageTimestamp, EventTrackingId, OnJSONMessageSent, nameof(WWCPSshClient), CancellationToken)
                   : Task.FromResult(SentMessageResult.UnknownClient());

        #endregion

        #region SendBinaryMessage (BinaryMessage, MessageTimestamp, EventTrackingId, CancellationToken = default)

        /// <summary>
        /// Send the given binary message.
        /// </summary>
        public Task<SentMessageResult> SendBinaryMessage(Byte[]             BinaryMessage,
                                                         DateTimeOffset     MessageTimestamp,
                                                         EventTracking_Id   EventTrackingId,
                                                         CancellationToken  CancellationToken   = default)

            => MessageClient.Connection is SshMessageConnection connection && connection.IsAlive
                   ? WWCPSshMessaging.SendBinary(this, connection, BinaryMessage, MessageTimestamp, EventTrackingId, OnBinaryMessageSent, nameof(WWCPSshClient), CancellationToken)
                   : Task.FromResult(SentMessageResult.UnknownClient());

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{TransportDescription}{(ConnectAs.HasValue ? $" as '{ConnectAs}'" : "")}{(IsConnected ? "" : " (not connected)")}";

        #endregion

    }

}
