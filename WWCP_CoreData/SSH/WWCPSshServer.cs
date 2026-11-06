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

using System.Collections.Concurrent;
using System.Collections.Immutable;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SSH;
using org.GraphDefined.Vanaheimr.Hermod.Messaging;

using cloud.charging.open.protocols.WWCP.WebSockets;
using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP.SSH
{

    /// <summary>
    /// Serves networking nodes over an SSH subsystem - "wwcp@open.charging.cloud", or a protocol's own such
    /// as "ocpp@open.charging.cloud" - as <see cref="WWCPWebSocketServer"/> does over HTTP WebSockets.
    /// </summary>
    /// <remarks>
    /// It listens on nothing itself. An SSH server does, checks who signs in, may confine a login to some
    /// subsystems, and hands this server the channels that ask for <see cref="Subsystem"/>: give
    /// <see cref="Handler"/> to its SshServerOptions.Subsystems.
    ///
    /// A networking node is who signed in: the SSH login is its identification. A login may connect as
    /// another networking node - a local controller for its charging stations - where it was allowed to
    /// (<see cref="AllowToActFor"/>), by naming it as "networkingNodeId" in its HELLO. "networkingMode"
    /// there is what the X-OCPP-NetworkingMode header is to the WebSocket transport.
    ///
    /// A newer connection of a networking node replaces an older one, as over WebSockets.
    /// </remarks>
    public class WWCPSshServer : IWWCPServer
    {

        #region Data

        /// <summary>
        /// The subsystem of WWCP itself.
        /// </summary>
        public const String WWCPSubsystem          = "wwcp@open.charging.cloud";

        /// <summary>
        /// The HELLO property naming the networking node a login connects as.
        /// </summary>
        public const String HelloNetworkingNodeId  = WWCPSshMessaging.HelloNetworkingNodeId;

        /// <summary>
        /// The HELLO property telling the networking mode.
        /// </summary>
        public const String HelloNetworkingMode    = WWCPSshMessaging.HelloNetworkingMode;

        /// <summary>
        /// The refusal of a login that may not connect as the networking node its HELLO names.
        /// </summary>
        public const String RefusalNotPermitted    = WWCPSshMessaging.RefusalNotPermitted;

        private readonly ConcurrentDictionary<NetworkingNode_Id, ImmutableArray<SshMessageConnection>>  connectedNetworkingNodes  = [];
        private readonly ConcurrentDictionary<NetworkingNode_Id, ImmutableHashSet<NetworkingNode_Id>>   mayActFor                 = [];

        #endregion

        #region Properties

        /// <summary>
        /// The networking node serving.
        /// </summary>
        public INetworkingNode                    NetworkingNode                { get; }

        /// <summary>
        /// The message protocol's server of the subsystem.
        /// </summary>
        public SshMessageServer                   MessageServer                 { get; }

        /// <summary>
        /// The subsystem served.
        /// </summary>
        public String                             Subsystem
            => MessageServer.Subsystem;

        /// <summary>
        /// What serves the subsystem on a channel: for an SSH server's SshServerOptions.Subsystems.
        /// </summary>
        public SshSubsystemHandler                Handler
            => MessageServer.Handler;

        /// <summary>
        /// Messages go over SSH channels.
        /// </summary>
        public MessageTransport                   Transport
            => MessageTransport.SSH;

        /// <summary>
        /// The subsystem served.
        /// </summary>
        public String                             TransportDescription
            => $"SSH subsystem {Subsystem}";

        /// <summary>
        /// The networking nodes connected now.
        /// </summary>
        public IEnumerable<NetworkingNode_Id>     ConnectedNetworkingNodeIds
            => connectedNetworkingNodes.Keys;

        /// <summary>
        /// The connections served now - also those not yet registered, or no longer.
        /// </summary>
        public IEnumerable<SshMessageConnection>  Connections
            => MessageServer.Connections;

        /// <summary>
        /// How JSON messages are written.
        /// </summary>
        public Formatting                         JSONFormatting                { get; set; }
            = Formatting.None;

        #endregion

        #region Events

        /// <summary>
        /// A networking node was welcomed and registered - before its first message is read, so that it can
        /// be routed to by the time it sends one.
        /// </summary>
        public event OnWWCPSshConnectionDelegate?          OnNetworkingNodeConnectionAccepted;

        /// <summary>
        /// The connection of a networking node ended.
        /// </summary>
        public event OnWWCPSshConnectionClosedDelegate?    OnNetworkingNodeConnectionClosed;

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

        #endregion

        #region Constructor(s)

        /// <summary>
        /// A server of networking nodes over the given SSH subsystem.
        /// </summary>
        /// <param name="NetworkingNode">The networking node serving.</param>
        /// <param name="Subsystem">The subsystem served.</param>
        /// <param name="Subprotocols">The subprotocols served, e.g. "ocpp2.1"; none to serve whatever a client asks for.</param>
        /// <param name="MaxMessageSize">The largest message taken and sent.</param>
        /// <param name="PingEvery">Every how long a PING is sent on every connection; null for none.</param>
        public WWCPSshServer(INetworkingNode       NetworkingNode,
                             String                Subsystem        = WWCPSubsystem,
                             IEnumerable<String>?  Subprotocols     = null,
                             UInt64?               MaxMessageSize   = null,
                             TimeSpan?             PingEvery        = null)

            : this(NetworkingNode,
                   new SshMessageServerOptions {
                       Subsystem          = Subsystem,
                       Subprotocols       = Subprotocols?.ToArray() ?? [],
                       MaxMessageSizeIn   = MaxMessageSize ?? SshMessageFraming.DefaultMaxMessageSize,
                       MaxMessageSizeOut  = MaxMessageSize ?? SshMessageFraming.DefaultMaxMessageSize,
                       PingEvery          = PingEvery
                   })

        { }

        /// <summary>
        /// A server of networking nodes over an SSH subsystem, as the given options say.
        /// </summary>
        /// <param name="NetworkingNode">The networking node serving.</param>
        /// <param name="Options">The subsystem, its subprotocols and limits.</param>
        public WWCPSshServer(INetworkingNode          NetworkingNode,
                             SshMessageServerOptions  Options)
        {

            this.NetworkingNode  = NetworkingNode;

            this.MessageServer   = new SshMessageServer(Options) {
                                       ValidateHello = (server, context, hello, subprotocol, cancellationToken) =>
                                                           ValueTask.FromResult(Validate(context.Session.Username, hello))
                                   };

            MessageServer.OnNewConnection          += (timestamp, sender, connection, cancellationToken) =>
                                                          RegisterAsync(connection, cancellationToken);

            MessageServer.OnTextMessageReceived    += (timestamp, sender, connection, text, cancellationToken) =>
                                                          WWCPSshMessaging.ReceiveText(this, connection, timestamp, text, OnJSONMessageReceived, nameof(WWCPSshServer), cancellationToken);

            MessageServer.OnBinaryMessageReceived  += (timestamp, sender, connection, bytes, cancellationToken) =>
                                                          WWCPSshMessaging.ReceiveBinary(this, connection, timestamp, bytes, OnBinaryMessageReceived, nameof(WWCPSshServer), cancellationToken);

            MessageServer.OnConnectionClosed       += (timestamp, sender, connection, close, cancellationToken) =>
                                                          ProcessConnectionClosed(connection, close, cancellationToken);

        }

        #endregion


        // Networking nodes connecting as others

        #region AllowToActFor    (NetworkingNodeId, NetworkingNodeIds)

        /// <summary>
        /// Allow the given networking node - a local controller, say - to connect as each of the given ones,
        /// with its own SSH login. A HELLO that names another networking node than the login is refused
        /// otherwise.
        /// </summary>
        /// <param name="NetworkingNodeId">The networking node whose login opens the connections.</param>
        /// <param name="NetworkingNodeIds">The networking nodes it may connect as.</param>
        public void AllowToActFor(NetworkingNode_Id               NetworkingNodeId,
                                  IEnumerable<NetworkingNode_Id>  NetworkingNodeIds)

            => mayActFor.AddOrUpdate(
                   NetworkingNodeId,
                   _        => ImmutableHashSet.CreateRange(NetworkingNodeIds),
                   (_, ids) => ids.Union(NetworkingNodeIds)
               );

        #endregion

        #region DisallowToActFor (NetworkingNodeId, NetworkingNodeIds)

        /// <summary>
        /// No longer allow the given networking node to connect as the given ones. A connection it has open
        /// as one of them stays open.
        /// </summary>
        /// <param name="NetworkingNodeId">The networking node whose login opened the connections.</param>
        /// <param name="NetworkingNodeIds">The networking nodes it may no longer connect as.</param>
        public void DisallowToActFor(NetworkingNode_Id               NetworkingNodeId,
                                     IEnumerable<NetworkingNode_Id>  NetworkingNodeIds)
        {

            var disallowed = NetworkingNodeIds.ToArray();

            while (mayActFor.TryGetValue(NetworkingNodeId, out var ids))
            {

                var rest = ids.Except(disallowed);

                if (rest.IsEmpty
                        ? mayActFor.TryRemove(KeyValuePair.Create(NetworkingNodeId, ids))
                        : mayActFor.TryUpdate(NetworkingNodeId, rest, ids))
                {
                    return;
                }

            }

        }

        #endregion

        #region MayActFor        (NetworkingNodeId, OtherNetworkingNodeId)

        /// <summary>
        /// Whether the given networking node may connect as the other one.
        /// </summary>
        /// <param name="NetworkingNodeId">The networking node whose login opens the connection.</param>
        /// <param name="OtherNetworkingNodeId">The networking node it connects as.</param>
        public Boolean MayActFor(NetworkingNode_Id  NetworkingNodeId,
                                 NetworkingNode_Id  OtherNetworkingNodeId)

            => mayActFor.TryGetValue(NetworkingNodeId, out var ids) &&
               ids.Contains(OtherNetworkingNodeId);

        #endregion


        // Connection management

        #region Validate                  (Login, Hello)

        /// <summary>
        /// Who a client is: its login, or the networking node its HELLO names, where the login may connect
        /// as that one - and how it networks. What is decided goes into the connection's custom data, under
        /// the keys the WebSocket transport uses.
        /// </summary>
        /// <param name="Login">The SSH login.</param>
        /// <param name="Hello">The client's HELLO.</param>
        public SshHelloVerdict Validate(String           Login,
                                        SshMessageHello  Hello)
        {

            if (!NetworkingNode_Id.TryParse(Login, out var fromLogin))
                return SshHelloVerdict.Refuse(RefusalNotPermitted, "The SSH login names no networking node.");

            #region The networking node the HELLO names, if any

            NetworkingNode_Id? fromHello = null;

            if (Hello.JSON[HelloNetworkingNodeId] is JToken idToken)
            {

                if (idToken.Type != JTokenType.String ||
                   !NetworkingNode_Id.TryParse(idToken.Value<String>() ?? "", out var parsed))
                {
                    return SshHelloVerdict.Refuse(SshMessageRefusal.InvalidHello, $"'{HelloNetworkingNodeId}' must be a networking node identification.");
                }

                fromHello = parsed;

            }

            #endregion

            #region The networking mode, if any

            NetworkingMode? networkingMode = null;

            if (Hello.JSON[HelloNetworkingMode] is JToken modeToken)
            {

                // By name only: Enum.TryParse would take "2" or "Standard, OverlayNetwork" as well.
                if (modeToken.Type != JTokenType.String ||
                    modeToken.Value<String>() is not String modeText ||
                   !Enum.GetNames<NetworkingMode>().Contains(modeText, StringComparer.Ordinal))
                {
                    return SshHelloVerdict.Refuse(SshMessageRefusal.InvalidHello, $"'{HelloNetworkingMode}' must be one of {String.Join(", ", Enum.GetNames<NetworkingMode>().Select(name => $"'{name}'"))}.");
                }

                networkingMode = Enum.Parse<NetworkingMode>(modeText);

            }

            #endregion

            var customData = new Dictionary<String, Object?>();

            if (fromHello.HasValue && fromHello.Value != fromLogin)
            {

                if (!MayActFor(fromLogin, fromHello.Value))
                    return SshHelloVerdict.Refuse(RefusalNotPermitted, $"'{fromLogin}' may not connect as '{fromHello.Value}'.");

                customData[WebSocketKeys.ActingNetworkingNodeId] = fromLogin;

            }

            customData[WebSocketKeys.NetworkingNodeId] = fromHello ?? fromLogin;

            if (networkingMode.HasValue)
                customData[WebSocketKeys.NetworkingMode] = networkingMode.Value;

            return SshHelloVerdict.Accept(CustomData: customData);

        }

        #endregion

        #region GetConnectionsFor         (DestinationId)

        /// <summary>
        /// The live connections to the given networking node, or to the one it is reached through.
        /// </summary>
        /// <param name="DestinationId">A networking node.</param>
        public IEnumerable<SshMessageConnection> GetConnectionsFor(NetworkingNode_Id DestinationId)
        {

            if (DestinationId == NetworkingNode_Id.Zero)
                return [];

            if (DestinationId == NetworkingNode_Id.Broadcast)
                return connectedNetworkingNodes.Values.
                           SelectMany(connections => connections).
                           Where     (connection  => connection.IsAlive);

            var nextHop = DestinationId;

            // The destination might only be reachable via a networking hop...
            if (NetworkingNode.Routing.LookupNetworkingNode(nextHop, out var reachability))
                nextHop = reachability.DestinationId;

            return connectedNetworkingNodes.TryGetValue(nextHop, out var connectionsTo)
                       ? connectionsTo.Where(connection => connection.IsAlive)
                       : [];

        }

        #endregion

        #region Stop                      (Reason = null)

        /// <summary>
        /// Tell every client this server is going away, and close its connection. The subsystem is served on
        /// as long as the SSH server serves it: a client that connects again is served again.
        /// </summary>
        /// <param name="Reason">An optional reason.</param>
        public Task Stop(String? Reason = null)

            => Task.WhenAll(
                   MessageServer.Connections.Select(connection => connection.Close(MessageCloseCode.GoingAway, Reason))
               );

        #endregion


        #region (private) RegisterAsync           (Connection, CancellationToken)

        /// <summary>
        /// Put a welcomed connection on the books under its networking node, and close those it replaces.
        /// </summary>
        private async Task RegisterAsync(SshMessageConnection  Connection,
                                         CancellationToken     CancellationToken)
        {

            // Validate always puts it there: a connection without is none of this server's.
            if (Connection.NetworkingNodeIdOf() is not NetworkingNode_Id networkingNodeId)
                return;

            foreach (var replacedConnection in Register(networkingNodeId, Connection))
            {

                DebugX.Log($"{nameof(WWCPSshServer)} Duplicate networking node '{networkingNodeId}' detected: Closing the older connection from {replacedConnection.RemoteSocket}!");

                // Not awaited: an older connection that no longer reads - a station that lost its power -
                // must not hold up the newer one until its channel gives up.
                _ = CloseReplacedAsync(replacedConnection);

            }

            await WWCPSshMessaging.RaiseAll(
                      OnNetworkingNodeConnectionAccepted,
                      handler => handler(
                                     Timestamp.Now,
                                     this,
                                     Connection,
                                     networkingNodeId,
                                     Connection.TryGetCustomDataAs<NetworkingMode>(WebSocketKeys.NetworkingMode),
                                     EventTracking_Id.New,
                                     CancellationToken
                                 ),
                      nameof(WWCPSshServer)
                  );

        }

        #endregion

        #region (private) CloseReplacedAsync      (Connection)

        private static async Task CloseReplacedAsync(SshMessageConnection Connection)
        {
            try
            {
                await Connection.Close(MessageCloseCode.NormalClosure, "Newer connection detected!");
            }
            catch (Exception e)
            {
                DebugX.Log($"{nameof(WWCPSshServer)} Closing old SSH connection from {Connection.RemoteSocket} failed: {e.Message}");
            }
        }

        #endregion

        #region (private) ProcessConnectionClosed (Connection, Close, CancellationToken)

        private Task ProcessConnectionClosed(SshMessageConnection  Connection,
                                             SshMessageCloseInfo   Close,
                                             CancellationToken     CancellationToken)
        {

            var networkingNodeId = Connection.NetworkingNodeIdOf();

            if (networkingNodeId.HasValue)
                Forget(networkingNodeId.Value, Connection);

            return WWCPSshMessaging.RaiseAll(
                       OnNetworkingNodeConnectionClosed,
                       handler => handler(
                                      Timestamp.Now,
                                      this,
                                      Connection,
                                      networkingNodeId,
                                      Close,
                                      CancellationToken
                                  ),
                       nameof(WWCPSshServer)
                   );

        }

        #endregion

        #region (private) Register                (NetworkingNodeId, Connection)

        /// <summary>
        /// Make the given connection a connection to the given networking node, and return the connections
        /// to it that the new one replaces: all of them, newest wins.
        /// </summary>
        /// <remarks>
        /// An entry is never changed, only replaced, and only if it is still the one that was read: whoever
        /// registers or forgets a connection at the same time does not undo it.
        /// </remarks>
        private SshMessageConnection[] Register(NetworkingNode_Id     NetworkingNodeId,
                                                SshMessageConnection  Connection)
        {
            while (true)
            {

                if (connectedNetworkingNodes.TryGetValue(NetworkingNodeId, out var current))
                {

                    var replaced = current.Where(connection => !ReferenceEquals(connection, Connection)).ToArray();

                    if (connectedNetworkingNodes.TryUpdate(NetworkingNodeId, [ Connection ], current))
                        return replaced;

                }

                else if (connectedNetworkingNodes.TryAdd(NetworkingNodeId, [ Connection ]))
                    return [];

            }
        }

        #endregion

        #region (private) Forget                  (NetworkingNodeId, Connection)

        /// <summary>
        /// Forget the given connection to the given networking node - and no other one. A connection that was
        /// replaced by a newer one is no longer one to the networking node, and its end, which may well come
        /// after the newer one was registered, is not the end of the newer one.
        /// </summary>
        /// <returns>Whether the connection was one to the networking node.</returns>
        private Boolean Forget(NetworkingNode_Id     NetworkingNodeId,
                               SshMessageConnection  Connection)
        {

            while (connectedNetworkingNodes.TryGetValue(NetworkingNodeId, out var current) &&
                   current.Any(connection => ReferenceEquals(connection, Connection)))
            {

                var rest = current.RemoveAll(connection => ReferenceEquals(connection, Connection));

                if (rest.IsEmpty
                        ? connectedNetworkingNodes.TryRemove(KeyValuePair.Create(NetworkingNodeId, current))
                        : connectedNetworkingNodes.TryUpdate(NetworkingNodeId, rest, current))
                {
                    return true;
                }

            }

            return false;

        }

        #endregion


        // Send data

        #region SendJSONMessage   (Connection,    JSONMessage,   MessageTimestamp, EventTrackingId, CancellationToken = default)

        /// <summary>
        /// Send the given JSON message on the given connection.
        /// </summary>
        public Task<SentMessageResult> SendJSONMessage(IMessageConnection  Connection,
                                                       JArray              JSONMessage,
                                                       DateTimeOffset      MessageTimestamp,
                                                       EventTracking_Id    EventTrackingId,
                                                       CancellationToken   CancellationToken   = default)

            => WWCPSshMessaging.SendJSON(this, Connection, JSONMessage, JSONFormatting, MessageTimestamp, EventTrackingId, OnJSONMessageSent, nameof(WWCPSshServer), CancellationToken);

        #endregion

        #region SendBinaryMessage (Connection,    BinaryMessage, MessageTimestamp, EventTrackingId, CancellationToken = default)

        /// <summary>
        /// Send the given binary message on the given connection.
        /// </summary>
        public Task<SentMessageResult> SendBinaryMessage(IMessageConnection  Connection,
                                                         Byte[]              BinaryMessage,
                                                         DateTimeOffset      MessageTimestamp,
                                                         EventTracking_Id    EventTrackingId,
                                                         CancellationToken   CancellationToken   = default)

            => WWCPSshMessaging.SendBinary(this, Connection, BinaryMessage, MessageTimestamp, EventTrackingId, OnBinaryMessageSent, nameof(WWCPSshServer), CancellationToken);

        #endregion

        #region SendJSONMessage   (DestinationId, JSONMessage,   MessageTimestamp, EventTrackingId, CancellationToken = default)

        /// <summary>
        /// Send the given JSON message to the given networking node, over the first of its connections that
        /// takes it.
        /// </summary>
        public async Task<SentMessageResult> SendJSONMessage(NetworkingNode_Id  DestinationId,
                                                             JArray             JSONMessage,
                                                             DateTimeOffset     MessageTimestamp,
                                                             EventTracking_Id   EventTrackingId,
                                                             CancellationToken  CancellationToken   = default)
        {

            foreach (var connection in GetConnectionsFor(DestinationId))
            {

                var sentMessageResult = await SendJSONMessage(connection, JSONMessage, MessageTimestamp, EventTrackingId, CancellationToken);

                if (sentMessageResult.Result == SentMessageResults.Success)
                    return sentMessageResult;

            }

            return SentMessageResult.UnknownClient();

        }

        #endregion

        #region SendBinaryMessage (DestinationId, BinaryMessage, MessageTimestamp, EventTrackingId, CancellationToken = default)

        /// <summary>
        /// Send the given binary message to the given networking node, over the first of its connections that
        /// takes it.
        /// </summary>
        public async Task<SentMessageResult> SendBinaryMessage(NetworkingNode_Id  DestinationId,
                                                               Byte[]             BinaryMessage,
                                                               DateTimeOffset     MessageTimestamp,
                                                               EventTracking_Id   EventTrackingId,
                                                               CancellationToken  CancellationToken   = default)
        {

            foreach (var connection in GetConnectionsFor(DestinationId))
            {

                var sentMessageResult = await SendBinaryMessage(connection, BinaryMessage, MessageTimestamp, EventTrackingId, CancellationToken);

                if (sentMessageResult.Result == SentMessageResults.Success)
                    return sentMessageResult;

            }

            return SentMessageResult.UnknownClient();

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{TransportDescription} of '{NetworkingNode.Id}', {connectedNetworkingNodes.Count} networking node(s) connected";

        #endregion

    }

}
