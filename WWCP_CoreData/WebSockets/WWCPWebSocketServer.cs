/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP WWCP <https://github.com/OpenChargingCloud/WWCP_WWCP>
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
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP;
using cloud.charging.open.protocols.WWCP.NetworkingNode;
using org.GraphDefined.Vanaheimr.Hermod.TCP;
using Microsoft.Extensions.Logging;

#endregion

namespace cloud.charging.open.protocols.WWCP.WebSockets
{

    public class NetworkingNodeConnections(NetworkingNode_Id                DestinationNodeId,
                                           List<WebSocketServerConnection>  ConnectionInfos)
    {

        public NetworkingNode_Id                DestinationNodeId             { get; } = DestinationNodeId;
        public List<WebSocketServerConnection>  WebSocketServerConnections    { get; } = ConnectionInfos;

    }


    /// <summary>
    /// The WWCP HTTP WebSocket server.
    /// </summary>
    public partial class WWCPWebSocketServer : WebSocketServer,
                                               IWWCPWebSocketServer
    {

        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public const       String                                                              DefaultHTTPServiceName    = "GraphDefined WWCP WebSocket Server";

        public const       String                                                              LogfileName               = "CSMSWSServer.log";

        protected readonly ConcurrentDictionary<NetworkingNode_Id, NetworkingNodeConnections>  connectedNetworkingNodes  = [];

        private   readonly ConcurrentDictionary<NetworkingNode_Id, ImmutableHashSet<NetworkingNode_Id>>  mayActFor  = [];

        #endregion

        #region Properties

        /// <summary>
        /// The parent networking node.
        /// </summary>
        public INetworkingNode                 NetworkingNode    { get; }

        /// <summary>
        /// The enumeration of all connected networking nodes.
        /// </summary>
        public IEnumerable<NetworkingNodeConnections>  ConnectedNetworkingNodes
            => connectedNetworkingNodes.Values;

        /// <summary>
        /// The enumeration of all connected networking node identifications.
        /// </summary>
        public IEnumerable<NetworkingNode_Id>  ConnectedNetworkingNodeIds
            => connectedNetworkingNodes.Keys;

        /// <summary>
        /// The request timeout for messages sent by this HTTP WebSocket server.
        /// </summary>
        public TimeSpan?                       RequestTimeout    { get; set; }

        /// <summary>
        /// The JSON formatting to use.
        /// </summary>
        public Formatting                      JSONFormatting    { get; set; }
            = Formatting.None;

        #endregion

        #region Events

        #region Common Connection Management

        /// <summary>
        /// An event sent whenever the connection of a networking node has been
        /// accepted and registered, before it is answered with 101 Switching
        /// Protocols - so that the networking node can be routed to by the time
        /// it knows it is connected.
        /// </summary>
        /// <remarks>
        /// Nothing can be sent on the connection yet: a frame sent now waits for
        /// the 101, which is only sent once every handler has returned.
        /// OnNetworkingNodeNewWebSocketConnection follows once it has been.
        /// </remarks>
        public event OnNetworkingNodeNewWebSocketConnectionDelegate?  OnNetworkingNodeWebSocketConnectionAccepted;

        /// <summary>
        /// An event sent whenever the HTTP connection switched successfully to web socket.
        /// </summary>
        public event OnNetworkingNodeNewWebSocketConnectionDelegate?  OnNetworkingNodeNewWebSocketConnection;

        /// <summary>
        /// An event sent whenever a web socket close frame was received.
        /// </summary>
        public event OnNetworkingNodeCloseMessageReceivedDelegate?    OnNetworkingNodeCloseMessageReceived;

        /// <summary>
        /// An event sent whenever a TCP connection was closed.
        /// </summary>
        public event OnNetworkingNodeTCPConnectionClosedDelegate?     OnNetworkingNodeTCPConnectionClosed;

        #endregion

        #region JSON/Binary Message Sent/Received

        /// <summary>
        /// An event sent whenever a JSON message was sent.
        /// </summary>
        public event     OnWebSocketServerJSONMessageSentDelegate?         OnJSONMessageSent;

        /// <summary>
        /// An event sent whenever a JSON message was received.
        /// </summary>
        public event     OnWebSocketServerJSONMessageReceivedDelegate?     OnJSONMessageReceived;

        /// <summary>
        /// An event sent whenever a JSON message was received.
        /// </summary>
        public event     OnWebSocketServerJSONMessageReceivedDelegate?     OnJSONMessageReceived2;


        /// <summary>
        /// An event sent whenever a binary message was sent.
        /// </summary>
        public new event OnWebSocketServerBinaryMessageSentDelegate?       OnBinaryMessageSent;

        /// <summary>
        /// An event sent whenever a binary message was received.
        /// </summary>
        public new event OnWebSocketServerBinaryMessageReceivedDelegate?   OnBinaryMessageReceived;

        /// <summary>
        /// An event sent whenever a binary message was received.
        /// </summary>
        public new event OnWebSocketServerBinaryMessageReceivedDelegate?   OnBinaryMessageReceived2;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP HTTP WebSocket server.
        /// </summary>
        /// <param name="HTTPServiceName">An optional identification string for the HTTP service.</param>
        /// <param name="IPAddress">An IP address to listen on.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="Description">An optional description of this HTTP WebSocket service.</param>
        /// 
        /// <param name="RequireAuthentication">Require a HTTP Basic Authentication of all charging boxes.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public WWCPWebSocketServer(INetworkingNode                                           NetworkingNode,

                                   IIPAddress?                                               IPAddress                    = null,
                                   IPPort?                                                   HTTPPort                     = null,
                                   String?                                                   HTTPServerName               = null,
                                   I18NString?                                               Description                  = null,

                                   Boolean?                                                  RequireAuthentication        = true,
                                   IEnumerable<String>?                                      SecWebSocketProtocols        = null,
                                   SubprotocolSelectorDelegate?                              SubprotocolSelector          = null,
                                   Boolean                                                   DisableWebSocketPings        = false,
                                   TimeSpan?                                                 WebSocketPingEvery           = null,
                                   TimeSpan?                                                 SlowNetworkSimulationDelay   = null,

                                   UInt32?                                                   BufferSize                   = null,
                                   TimeSpan?                                                 ReceiveTimeout               = null,
                                   TimeSpan?                                                 SendTimeout                  = null,
                                   TCPEchoLoggingDelegate?                                   LoggingHandler               = null,

                                   ServerCertificateSelectorDelegate?                        ServerCertificateSelector    = null,
                                   RemoteTLSClientCertificateValidationHandler<ITCPServer>?  ClientCertificateValidator   = null,
                                   LocalCertificateSelectionHandler?                         LocalCertificateSelector     = null,
                                   SslProtocols?                                             AllowedTLSProtocols          = null,
                                   Boolean?                                                  ClientCertificateRequired    = null,
                                   Boolean?                                                  CheckCertificateRevocation   = null,

                                   ConnectionIdBuilder?                                      ConnectionIdBuilder          = null,
                                   UInt32?                                                   MaxClientConnections         = null,
                                   IDNSClient?                                               DNSClient                    = null,

                                   Boolean?                                                  DisableMaintenanceTasks      = false,
                                   TimeSpan?                                                 MaintenanceInitialDelay      = null,
                                   TimeSpan?                                                 MaintenanceEvery             = null,

                                   Boolean?                                                  DisableWardenTasks           = false,
                                   TimeSpan?                                                 WardenInitialDelay           = null,
                                   TimeSpan?                                                 WardenCheckEvery             = null,

                                   ILoggerFactory?                                           LoggerFactory                = null,
                                   Boolean?                                                  AutoStart                    = false)

            : base(IPAddress,
                   HTTPPort,
                   HTTPServerName,
                   Description,

                   RequireAuthentication,
                   SecWebSocketProtocols,
                   SubprotocolSelector,
                   DisableWebSocketPings,
                   WebSocketPingEvery,
                   SlowNetworkSimulationDelay,

                   BufferSize,
                   ReceiveTimeout,
                   SendTimeout,
                   LoggingHandler,

                   ServerCertificateSelector,
                   ClientCertificateValidator,
                   LocalCertificateSelector,
                   AllowedTLSProtocols,
                   ClientCertificateRequired,
                   CheckCertificateRevocation,

                   ConnectionIdBuilder,
                   MaxClientConnections,
                   DNSClient,

                   DisableMaintenanceTasks,
                   MaintenanceInitialDelay,
                   MaintenanceEvery,

                   DisableWardenTasks,
                   WardenInitialDelay,
                   WardenCheckEvery,

                   LoggerFactory,
                   AutoStart: false)

        {

            this.NetworkingNode                  = NetworkingNode;

            //this.Logger                          = new ChargePointwebsocketClient.CPClientLogger(this,
            //                                                                                LoggingPath,
            //                                                                                LoggingContext,
            //                                                                                LogfileCreator);

            base.OnValidateTCPConnection        += ValidateTCPConnection;
            base.OnValidateWebSocketConnection  += ValidateWebSocketConnection;
            base.OnWebSocketConnectionAccepted  += RegisterNewWebSocketConnection;
            base.OnNewWebSocketConnection       += ProcessNewWebSocketConnection;
            base.OnCloseMessageReceived         += ProcessCloseMessage;
            base.OnTCPConnectionClosed          += ProcessTCPConnectionClosed;

            // Text and binary messages are received via the
            // ProcessTextMessage/ProcessBinaryMessage overrides!

            base.OnPingMessageReceived          += (timestamp, server, connection, frame, eventTrackingId, pingMessage, ct) => {
                                                       DebugX.Log($"HTTP WebSocket Server '{connection.RemoteSocket}' Ping received:   '{frame.Payload.ToUTF8String()}'");
                                                       return Task.CompletedTask;
                                                   };

            base.OnPongMessageReceived          += (timestamp, server, connection, frame, eventTrackingId, pingMessage, ct) => {
                                                       DebugX.Log($"HTTP WebSocket Server '{connection.RemoteSocket}' Pong received:   '{frame.Payload.ToUTF8String()}'");
                                                       return Task.CompletedTask;
                                                   };

            base.OnCloseMessageReceived         += (timestamp, server, connection, frame, eventTrackingId, closingStatusCode, closingReason, ct) => {
                                                       DebugX.Log($"HTTP WebSocket Server '{connection.Login}' Close received:  '{closingStatusCode}', '{closingReason ?? ""}'");
                                                       return Task.CompletedTask;
                                                   };

            if (AutoStart ?? false)
                Start().GetAwaiter().GetResult();

        }

        #endregion


        // HTTP Basic Auth

        #region AddOrUpdateHTTPBasicAuth (NetworkingNodeId, Password)

        /// <summary>
        /// Add the given HTTP Basic Authentication password for the given networking node.
        /// </summary>
        /// <param name="NetworkingNodeId">The unique identification of the networking node.</param>
        /// <param name="Password">The password of the charging station.</param>
        public HTTPBasicAuthentication AddOrUpdateHTTPBasicAuth(NetworkingNode_Id  NetworkingNodeId,
                                                                String             Password)

            => AddOrUpdateHTTPBasicAuth(
                   NetworkingNodeId.ToString(),
                   Password
               );

        #endregion

        #region RemoveHTTPBasicAuth      (NetworkingNodeId)

        /// <summary>
        /// Remove the given HTTP Basic Authentication for the given networking node.
        /// </summary>
        /// <param name="NetworkingNodeId">The unique identification of the networking node.</param>
        public Boolean RemoveHTTPBasicAuth(NetworkingNode_Id NetworkingNodeId)

            => RemoveHTTPBasicAuth(
                   NetworkingNodeId.ToString()
               );

        #endregion


        // Networking nodes connecting as others

        #region AllowToActFor    (NetworkingNodeId, NetworkingNodeIds)

        /// <summary>
        /// Allow the given networking node - a local controller, say - to connect
        /// as each of the given ones, with credentials of its own: its client
        /// certificate, or its HTTP Basic or TOTP login (OCPP 2.1 Part 4, 6.2 and
        /// 6.5). A connection with the credentials of one networking node and the
        /// name of another in its path is refused otherwise.
        /// </summary>
        /// <param name="NetworkingNodeId">The networking node whose credentials open the connections.</param>
        /// <param name="NetworkingNodeIds">The networking nodes it may connect as.</param>
        public void AllowToActFor(NetworkingNode_Id               NetworkingNodeId,
                                  IEnumerable<NetworkingNode_Id>  NetworkingNodeIds)

            => mayActFor.AddOrUpdate(
                   NetworkingNodeId,
                   _       => ImmutableHashSet.CreateRange(NetworkingNodeIds),
                   (_, ids) => ids.Union(NetworkingNodeIds)
               );

        #endregion

        #region DisallowToActFor (NetworkingNodeId, NetworkingNodeIds)

        /// <summary>
        /// No longer allow the given networking node to connect as the given ones.
        /// A connection it has open as one of them stays open.
        /// </summary>
        /// <param name="NetworkingNodeId">The networking node whose credentials opened the connections.</param>
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
        /// <param name="NetworkingNodeId">The networking node whose credentials open the connection.</param>
        /// <param name="OtherNetworkingNodeId">The networking node it connects as.</param>
        public Boolean MayActFor(NetworkingNode_Id  NetworkingNodeId,
                                 NetworkingNode_Id  OtherNetworkingNodeId)

            => mayActFor.TryGetValue(NetworkingNodeId, out var ids) &&
               ids.Contains(OtherNetworkingNodeId);

        #endregion


        // Connection management...

        #region (protected) ValidateTCPConnection         (LogTimestamp, Server, Connection, EventTrackingId, CancellationToken)

        private Task<ConnectionFilterResponse> ValidateTCPConnection(DateTimeOffset                LogTimestamp,
                                                                     AWebSocketServer              Server,
                                                                     System.Net.Sockets.TcpClient  Connection,
                                                                     EventTracking_Id              EventTrackingId,
                                                                     CancellationToken             CancellationToken)
        {

            return Task.FromResult(ConnectionFilterResponse.Accepted());

        }

        #endregion

        #region (protected) ValidateWebSocketConnection   (LogTimestamp, Server, Connection, HTTPRequest, EventTrackingId, CancellationToken)

        private Task<HTTPResponse?> ValidateWebSocketConnection(DateTimeOffset             LogTimestamp,
                                                                AWebSocketServer           Server,
                                                                WebSocketServerConnection  Connection,
                                                                EventTracking_Id           EventTrackingId,
                                                                CancellationToken          CancellationToken)
        {

            #region Get HTTP request...

            // The HTTP request is attached to the connection before this event is fired,
            // but stay defensive: Without a HTTP request we can not authenticate anyone!
            if (Connection.HTTPRequest is not HTTPRequest httpRequest)
                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder(
                               Timestamp.Now,
                               EventTrackingId,
                               TimeSpan.Zero,
                               new HTTPSource(Connection.RemoteSocket),
                               Connection.LocalSocket,
                               Connection.RemoteSocket,
                               ConnectionType.Close,
                               HTTPStatusCode.BadRequest,
                               CancellationToken: CancellationToken
                           ).AsImmutable
                       );

            #endregion

            #region Verify 'Sec-WebSocket-Protocol'...

            if (Connection.HTTPRequest?.SecWebSocketProtocol is null ||
                Connection.HTTPRequest?.SecWebSocketProtocol.Any() == false)
            {

                DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket}: Missing 'Sec-WebSocket-Protocol' HTTP header!");

                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder(httpRequest) {
                               HTTPStatusCode  = HTTPStatusCode.BadRequest,
                               Server          = HTTPServiceName,
                               Date            = Timestamp.Now,
                               ContentType     = HTTPContentType.Application.JSON_UTF8,
                               Content         = JSONObject.Create(
                                                     new JProperty("description",
                                                     JSONObject.Create(
                                                         new JProperty("en", "Missing 'Sec-WebSocket-Protocol' HTTP header!")
                                                     ))).ToUTF8Bytes(),
                               Connection      = ConnectionType.Close
                           }.AsImmutable);

            }
            else if (!new HashSet<String>(SecWebSocketProtocols).Overlaps(Connection.HTTPRequest?.SecWebSocketProtocol ?? []))
            {

                var error = $"This WebSocket service only supports {SecWebSocketProtocols.Select(id => $"'{id}'").AggregateWith(", ")}!";

                DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket}: {error}");

                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder(httpRequest) {
                               HTTPStatusCode  = HTTPStatusCode.BadRequest,
                               Server          = HTTPServiceName,
                               Date            = Timestamp.Now,
                               ContentType     = HTTPContentType.Application.JSON_UTF8,
                               Content         = JSONObject.Create(
                                                     new JProperty("description",
                                                         JSONObject.Create(
                                                             new JProperty("en", error)
                                                     ))).ToUTF8Bytes(),
                               Connection      = ConnectionType.Close
                           }.AsImmutable);

            }

            #endregion

            #region Verify the networking node against its credentials

            // The networking node of a connection is the last segment of its path
            // (OCPP 2.1 Part 4, 3.1.1), and credentials of another networking node
            // do not make it that one: a station's password, its one-time password
            // or its certificate opens no connection as another station - unless
            // the networking node of the credentials may connect as the one of the
            // path, see AllowToActFor.
            //
            // Refused here, before the 101, where this server knows the credentials
            // to be right. Where it does not, whoever checks them decides, as for
            // any credentials: wrong ones are refused as wrong, and right ones, which
            // only somebody else could check, open a connection that is not
            // registered and so closed once its 101 has gone out, see
            // RegisterNewWebSocketConnection. Refused here as well, they would say to
            // anybody who asks, with wrong credentials, which networking node may
            // connect as which.
            var (fromPath, fromCredentials) = IdentitiesOf(httpRequest);

            if (fromPath.       HasValue &&
                fromCredentials.HasValue &&
                fromPath.Value != fromCredentials.Value &&
               !MayActFor(fromCredentials.Value, fromPath.Value) &&
                KnowsTheCredentialsOf(httpRequest))
            {

                var error = $"'{fromCredentials.Value}' may not connect as '{fromPath.Value}'!";

                DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket}: {error}");

                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder(httpRequest) {
                               HTTPStatusCode  = HTTPStatusCode.Forbidden,
                               Server          = HTTPServiceName,
                               Date            = Timestamp.Now,
                               ContentType     = HTTPContentType.Application.JSON_UTF8,
                               Content         = JSONObject.Create(
                                                     new JProperty("description",
                                                         JSONObject.Create(
                                                             new JProperty("en", error)
                                                     ))).ToUTF8Bytes(),
                               Connection      = ConnectionType.Close
                           }.AsImmutable);

            }

            #endregion

            #region Verify HTTP Authentication

            if (RequireAuthentication)
            {

                // Who asked to be let in goes into the log, never with what: a password
                // or a one-time password in a log is one for whoever reads the log -
                // and a wrong one is often the right one mistyped, or another account's.

                #region HTTP Basic Authentication

                if (Connection.HTTPRequest?.Authorization is HTTPBasicAuthentication basicAuthentication)
                {

                    if (ClientLogins.TryGetValue(basicAuthentication.Username, out var securePassword) &&
                        securePassword.Equals(basicAuthentication.Password))
                    {
                        DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} using authorization: '{basicAuthentication.Username}'");
                        return Task.FromResult<HTTPResponse?>(null);
                    }
                    else
                        DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} invalid authorization: '{basicAuthentication.Username}'!");

                }

                #endregion

                #region HTTP TOTP  Authentication

                else if (Connection.HTTPRequest?.Authorization is HTTPTOTPAuthentication totpAuthentication)
                {

                    if (ClientTOTPConfig.TryGetValue(totpAuthentication.Login, out var totpConfig))
                    {

                        if (TOTPMatches(totpConfig, totpAuthentication.TOTP))
                        {
                            DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} using TOTP authorization: '{totpAuthentication.Login}'");
                            return Task.FromResult<HTTPResponse?>(null);
                        }
                        else
                            DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} invalid or outdated TOTP authorization: '{totpAuthentication.Login}'!");

                    }
                    else
                        DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} invalid TOTP authorization: '{totpAuthentication.Login}'!");

                }

                #endregion

                else
                    DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} missing or invalid authorization!");


                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder(httpRequest) {
                               HTTPStatusCode  = HTTPStatusCode.Unauthorized,
                               Server          = HTTPServiceName,
                               Date            = Timestamp.Now,
                               Connection      = ConnectionType.Close
                           }.AsImmutable
                       );

            }

            #endregion

            return Task.FromResult<HTTPResponse?>(null);

        }

        #endregion

        #region (protected) RegisterNewWebSocketConnection (LogTimestamp, Server, Connection, SharedSubprotocols, EventTrackingId, CancellationToken)

        /// <summary>
        /// Register the networking node behind a connection that is about to be
        /// answered with 101 Switching Protocols, before that answer is sent.
        /// </summary>
        /// <remarks>
        /// A networking node that has read its 101 may be sent a request at once,
        /// and one registered only after the 101 was unknown to exactly that
        /// request. Announcing the connection, and closing one that does not say
        /// which networking node it is, waits for the 101:
        /// see ProcessNewWebSocketConnection.
        /// </remarks>
        protected async Task RegisterNewWebSocketConnection(DateTimeOffset             LogTimestamp,
                                                            AWebSocketServer           Server,
                                                            WebSocketServerConnection  Connection,
                                                            IEnumerable<String>        SharedSubprotocols,
                                                            String?                    SelectedSubprotocol,
                                                            EventTracking_Id           EventTrackingId,
                                                            CancellationToken          CancellationToken)
        {

            if (Connection.HTTPRequest is null)
                return;

            #region The networking node of the path, or else of the credentials

            var (fromPath, fromCredentials) = IdentitiesOf(Connection.HTTPRequest);

            if (fromPath.       HasValue &&
                fromCredentials.HasValue &&
                fromPath.Value != fromCredentials.Value)
            {

                // Credentials of another networking node, which this server could
                // not check itself, and whoever did let pass: not registered, and so
                // closed once the 101 has gone out, see ValidateWebSocketConnection.
                if (!MayActFor(fromCredentials.Value, fromPath.Value))
                {
                    DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket}: '{fromCredentials.Value}' may not connect as '{fromPath.Value}', and is not registered!");
                    return;
                }

                Connection.TryAddCustomData(
                               WebSocketKeys.ActingNetworkingNodeId,
                               fromCredentials.Value
                           );

            }

            var networkingNodeId = fromPath ?? fromCredentials;

            #endregion


            if (networkingNodeId.HasValue)
            {

                #region Store the NetworkingNodeId within the HTTP WebSocket connection

                Connection.TryAddCustomData(
                               WebSocketKeys.NetworkingNodeId,
                               networkingNodeId.Value
                           );

                #endregion

                #region Try to get NetworkingMode from HTTP Headers and store it within the HTTP WebSocket connection

                NetworkingMode? networkingMode = null;

                if (Connection.HTTPRequest.TryGetHeaderField(WebSocketKeys.X_WWCP_NetworkingMode, out var networkingModeString) &&
                    Enum.TryParse<NetworkingMode>(networkingModeString?.ToString(), out var networkingModeFromHTTPHeader))
                {

                    networkingMode = networkingModeFromHTTPHeader;

                    Connection.TryAddCustomData(
                                   WebSocketKeys.NetworkingMode,
                                   networkingMode
                               );

                }

                #endregion


                #region Register new NetworkingNode

                // The connections it replaces are taken off the books in the same
                // step that puts it on them, and are closed only afterwards: the
                // networking node is never without a connection in between, and
                // whatever the end of a replaced connection sets off finds the
                // newer one in its place, see Forget.
                foreach (var replacedConnection in Register(networkingNodeId.Value, Connection))
                {

                    DebugX.Log($"{nameof(WWCPWebSocketServer)} Duplicate networking node '{networkingNodeId.Value}' detected: Closing the older connection from {replacedConnection.RemoteSocket}!");

                    try
                    {
                        await replacedConnection.Close(
                                  WebSocketFrame.ClosingStatusCode.NormalClosure,
                                  "Newer connection detected!",
                                  CancellationToken
                              );
                    }
                    catch (Exception e)
                    {
                        DebugX.Log($"{nameof(WWCPWebSocketServer)} Closing old HTTP WebSocket connection from {replacedConnection.RemoteSocket} failed: {e.Message}");
                    }

                }

                #endregion

                #region Send OnNetworkingNodeWebSocketConnectionAccepted event

                await LogEvent(
                          OnNetworkingNodeWebSocketConnectionAccepted,
                          loggingDelegate => loggingDelegate.Invoke(
                              LogTimestamp,
                              this,
                              Connection,
                              networkingNodeId.Value,
                              networkingMode,
                              SharedSubprotocols,
                              EventTrackingId,
                              CancellationToken
                          )
                      );

                #endregion

            }

        }

        #endregion

        #region (protected) ProcessNewWebSocketConnection  (LogTimestamp, Server, Connection, SharedSubprotocols, EventTrackingId, CancellationToken)

        /// <summary>
        /// The 101 has gone out: announce the networking node registered before
        /// it, or close a connection that could not be registered, because it did
        /// not say which networking node it is.
        /// </summary>
        protected async Task ProcessNewWebSocketConnection(DateTimeOffset             LogTimestamp,
                                                           AWebSocketServer           Server,
                                                           WebSocketServerConnection  Connection,
                                                           IEnumerable<String>        SharedSubprotocols,
                                                           String?                    SelectedSubprotocol,
                                                           EventTracking_Id           EventTrackingId,
                                                           CancellationToken          CancellationToken)
        {

            if (Connection.HTTPRequest is null)
                return;

            if (Connection.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.NetworkingNodeId, out var networkingNodeId))
            {

                #region Send OnNewNetworkingNodeWSConnection event

                await LogEvent(
                          OnNetworkingNodeNewWebSocketConnection,
                          loggingDelegate => loggingDelegate.Invoke(
                              LogTimestamp,
                              this,
                              Connection,
                              networkingNodeId,
                              Connection.TryGetCustomDataAs<NetworkingMode>(WebSocketKeys.NetworkingMode, out var networkingMode)
                                  ? networkingMode
                                  : (NetworkingMode?) null,
                              SharedSubprotocols,
                              EventTrackingId,
                              CancellationToken
                          )
                      );

                #endregion

            }

            #region else: Close connection

            else
            {

                DebugX.Log($"{nameof(WWCPWebSocketServer)} Could not get NetworkingNodeId from HTTP WebSocket connection ({Connection.RemoteSocket}): Closing connection!");

                try
                {
                    await Connection.Close(
                              WebSocketFrame.ClosingStatusCode.PolicyViolation,
                              "Could not get NetworkingNodeId from HTTP WebSocket connection!",
                              CancellationToken
                          );
                }
                catch (Exception e)
                {
                    DebugX.Log($"{nameof(WWCPWebSocketServer)} Closing HTTP WebSocket connection ({Connection.RemoteSocket}) failed: {e.Message}");
                }

            }

            #endregion

        }

        #endregion

        #region (protected) ProcessCloseMessage           (LogTimestamp, Server, Connection, Frame, EventTrackingId, StatusCode, Reason, CancellationToken)

        protected async Task ProcessCloseMessage(DateTimeOffset                    LogTimestamp,
                                                 AWebSocketServer                  Server,
                                                 WebSocketServerConnection         Connection,
                                                 WebSocketFrame                    Frame,
                                                 EventTracking_Id                  EventTrackingId,
                                                 WebSocketFrame.ClosingStatusCode  StatusCode,
                                                 String?                           Reason,
                                                 CancellationToken                 CancellationToken)
        {

            if (Connection.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.NetworkingNodeId, out var networkingNodeId))
            {

                Forget(networkingNodeId, Connection);

                await LogEvent(
                    OnNetworkingNodeCloseMessageReceived,
                    loggingDelegate => loggingDelegate.Invoke(
                        LogTimestamp,
                        this,
                        Connection,
                        networkingNodeId,
                        EventTrackingId,
                        StatusCode,
                        Reason,
                        CancellationToken
                    )
                );

            }

        }

        #endregion

        #region (protected) ProcessTCPConnectionClosed    (LogTimestamp, Server, Connection, EventTrackingId, Reason, CancellationToken)

        /// <summary>
        /// A connection has ended, however it ended: after a close frame either
        /// way, or without one - a station that lost its power or its network,
        /// a reset, a FIN, a peer that stopped answering pings.
        /// </summary>
        protected async Task ProcessTCPConnectionClosed(DateTimeOffset             LogTimestamp,
                                                        AWebSocketServer           Server,
                                                        WebSocketServerConnection  Connection,
                                                        EventTracking_Id           EventTrackingId,
                                                        String?                    Reason,
                                                        CancellationToken          CancellationToken)
        {

            if (Connection.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.NetworkingNodeId, out var networkingNodeId))
            {

                Forget(networkingNodeId, Connection);

                await LogEvent(
                    OnNetworkingNodeTCPConnectionClosed,
                    loggingDelegate => loggingDelegate.Invoke(
                        LogTimestamp,
                        this,
                        Connection,
                        networkingNodeId,
                        EventTrackingId,
                        Reason,
                        CancellationToken
                    )
                );

            }

        }

        #endregion

        #region (private) Register                        (NetworkingNodeId, Connection)

        /// <summary>
        /// Make the given connection a connection to the given networking node,
        /// and return the connections to it that the new one replaces.
        /// </summary>
        /// <remarks>
        /// The entry of a networking node is never changed, only replaced by a
        /// new one, and only if it is still the entry that was read: whoever
        /// registers or forgets a connection at the same time does not undo it.
        /// </remarks>
        private IEnumerable<WebSocketServerConnection> Register(NetworkingNode_Id          NetworkingNodeId,
                                                                WebSocketServerConnection  Connection)
        {
            while (true)
            {

                if (connectedNetworkingNodes.TryGetValue(NetworkingNodeId, out var current))
                {

                    var others    = current.WebSocketServerConnections.Where(connection => !ReferenceEquals(connection, Connection)).ToArray();
                    var replaced  = others.Where(connection =>  Supersedes(Connection, connection)).ToArray();
                    var kept      = others.Where(connection => !Supersedes(Connection, connection));

                    if (connectedNetworkingNodes.TryUpdate(
                            NetworkingNodeId,
                            new NetworkingNodeConnections(
                                NetworkingNodeId,
                                [ .. kept, Connection ]
                            ),
                            current
                        ))
                    {
                        return replaced;
                    }

                }

                else if (connectedNetworkingNodes.TryAdd(
                             NetworkingNodeId,
                             new NetworkingNodeConnections(
                                 NetworkingNodeId,
                                 [ Connection ]
                             )
                         ))
                {
                    return [];
                }

            }
        }

        #endregion

        #region (private static) Supersedes               (Newer, Older)

        /// <summary>
        /// Whether a newer connection of a networking node replaces an older one.
        /// </summary>
        /// <remarks>
        /// Newest wins, per networking node: every one. A networking node with
        /// several connections at once - one per channel, as in the EVQI
        /// transport - will need the newest to win per channel instead, and
        /// this is the one place that has to say so.
        /// </remarks>
        private static Boolean Supersedes(WebSocketServerConnection  Newer,
                                          WebSocketServerConnection  Older)

            => true;

        #endregion

        #region (private) Forget                          (NetworkingNodeId, Connection)

        /// <summary>
        /// Forget the given connection to the given networking node - and no
        /// other one. A connection that was replaced by a newer one is no longer
        /// a connection to the networking node, and its end, which may well come
        /// after the newer one was registered, is not the end of the newer one.
        /// </summary>
        /// <returns>Whether the connection was one to the networking node.</returns>
        private Boolean Forget(NetworkingNode_Id          NetworkingNodeId,
                               WebSocketServerConnection  Connection)
        {

            while (connectedNetworkingNodes.TryGetValue(NetworkingNodeId, out var current) &&
                   current.WebSocketServerConnections.Any(connection => ReferenceEquals(connection, Connection)))
            {

                var rest = current.WebSocketServerConnections.Where(connection => !ReferenceEquals(connection, Connection)).ToList();

                if (rest.Count == 0
                        ? connectedNetworkingNodes.TryRemove(KeyValuePair.Create(NetworkingNodeId, current))
                        : connectedNetworkingNodes.TryUpdate(NetworkingNodeId, new NetworkingNodeConnections(NetworkingNodeId, rest), current))
                {
                    return true;
                }

            }

            return false;

        }

        #endregion

        #region (private static) IdentitiesOf             (Request)

        /// <summary>
        /// The networking node named by the last segment of the path of the given
        /// request, and the one named by its credentials: the common name of its
        /// client certificate, else its HTTP Basic username, else its TOTP login.
        /// </summary>
        private static (NetworkingNode_Id? FromPath, NetworkingNode_Id? FromCredentials) IdentitiesOf(HTTPRequest Request)
        {

            // Percent-decoded, segment by segment, by the time it is here.
            var path             = Request.Path.ToString();
            var fromPath         = NetworkingNode_Id.TryParse(path[(path.LastIndexOf('/') + 1)..]);

            var fromCredentials  = Request.ClientCertificate is not null
                                       ? NetworkingNode_Id.TryParse(Request.ClientCertificate.GetNameInfo(X509NameType.SimpleName, forIssuer: false))
                                       : null;

            fromCredentials    ??= Request.Authorization switch {
                                       HTTPBasicAuthentication basicAuthentication  => NetworkingNode_Id.TryParse(basicAuthentication.Username),
                                       HTTPTOTPAuthentication  totpAuthentication   => NetworkingNode_Id.TryParse(totpAuthentication.Login),
                                       _                                            => null
                                   };

            return (fromPath, fromCredentials);

        }

        #endregion

        #region (private) KnowsTheCredentialsOf           (Request)

        /// <summary>
        /// Whether this server knows the credentials the networking node of the
        /// given request is named by to be right: a client certificate, which the
        /// TLS handshake has validated, or an HTTP Basic or TOTP login of its own
        /// ClientLogins or ClientTOTPConfig - whether or not it requires
        /// authentication itself.
        /// </summary>
        private Boolean KnowsTheCredentialsOf(HTTPRequest Request)
        {

            if (Request.ClientCertificate is not null &&
                NetworkingNode_Id.TryParse(Request.ClientCertificate.GetNameInfo(X509NameType.SimpleName, forIssuer: false)).HasValue)
            {
                return true;
            }

            return Request.Authorization switch {

                       HTTPBasicAuthentication basicAuthentication
                           => ClientLogins.TryGetValue(basicAuthentication.Username, out var password) &&
                              password.Verify(basicAuthentication.Password),

                       HTTPTOTPAuthentication totpAuthentication
                           => ClientTOTPConfig.TryGetValue(totpAuthentication.Login, out var totpConfig) &&
                              TOTPMatches(totpConfig, totpAuthentication.TOTP),

                       _   => false

                   };

        }

        #endregion

        #region (private static) TOTPMatches              (TOTPConfig, TOTP)

        /// <summary>
        /// Whether the given TOTP is the one of the current time slot, or of the
        /// one before or after it.
        /// </summary>
        private static Boolean TOTPMatches(TOTPConfig  TOTPConfig,
                                           String      TOTP)
        {

            var (previousTOTP,
                 currentTOTP,
                 nextTOTP,
                 _,
                 _) = TOTPGenerator.GenerateTOTPs(
                          Timestamp.Now,
                          TOTPConfig.SharedSecret,
                          TOTPConfig.ValidityTime,
                          TOTPConfig.Length,
                          TOTPConfig.Alphabet
                      );

            return TOTP == previousTOTP ||
                   TOTP == currentTOTP  ||
                   TOTP == nextTOTP;

        }

        #endregion

        #region (protected) GetConnectionsFor             (DestinationId)

        protected IEnumerable<WebSocketServerConnection> GetConnectionsFor(NetworkingNode_Id DestinationId)
        {

            if (DestinationId == NetworkingNode_Id.Zero)
                return [];

            if (DestinationId == NetworkingNode_Id.Broadcast)
                return WebSocketConnections;

            var nextHop = DestinationId;

            // The destination might only be reachable via a networking hop...
            if (NetworkingNode.Routing.LookupNetworkingNode(nextHop, out var reachability))
                nextHop = reachability.DestinationId;

            if (connectedNetworkingNodes.TryGetValue(nextHop, out var networkingNodeConnections))
                return networkingNodeConnections.WebSocketServerConnections.
                           Where(webSocketServerConnection => webSocketServerConnection.IsAlive);

            return [];

        }

        #endregion


        // Receive data...

        #region (protected) ProcessTextMessage            (RequestTimestamp, Server, WebSocketConnection, Frame, EventTrackingId, TextMessage,   CancellationToken)

        /// <summary>
        /// Process a HTTP WebSocket text message.
        /// </summary>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="Server">The HTTP WebSocket server.</param>
        /// <param name="WebSocketConnection">The HTTP WebSocket connection.</param>
        /// <param name="EventTrackingId">An optional event tracking identification.</param>
        /// <param name="Frame">The HTTP WebSocket frame.</param>
        /// <param name="TextMessage">The received text message.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        public override async Task ProcessTextMessage(DateTimeOffset             RequestTimestamp,
                                                      AWebSocketServer           Server,
                                                      WebSocketServerConnection  WebSocketConnection,
                                                      EventTracking_Id           EventTrackingId,
                                                      WebSocketFrame             Frame,
                                                      String                     TextMessage,
                                                      CancellationToken          CancellationToken)
        {

            // Fire the generic OnTextMessageReceived event of the base class!
            await base.ProcessTextMessage(
                      RequestTimestamp,
                      Server,
                      WebSocketConnection,
                      EventTrackingId,
                      Frame,
                      TextMessage,
                      CancellationToken
                  );

            try
            {

                var sourceNodeId  = WebSocketConnection.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.NetworkingNodeId);

                #region Initial checks

                TextMessage = TextMessage.Trim();

                if (TextMessage == "[]" ||
                    TextMessage.IsNullOrEmpty())
                {

                    await HandleErrors(
                              nameof(WWCPWebSocketServer),
                              nameof(ProcessTextMessage),
                              $"Received an empty text message from {(
                                   sourceNodeId.HasValue
                                       ? $"'{sourceNodeId}' ({WebSocketConnection.RemoteSocket})"
                                       : $"'{WebSocketConnection.RemoteSocket}"
                              )}'!"
                          );

                    return;

                }

                #endregion


                var jsonMessage   = JArray.Parse(TextMessage);
                var timestamp     = Timestamp.Now;

                // Just for logging!
                await LogEvent(
                          OnJSONMessageReceived,
                          loggingDelegate => loggingDelegate.Invoke(
                              timestamp,
                              this,
                              WebSocketConnection,
                              RequestTimestamp,
                              EventTrackingId,
                              sourceNodeId ?? NetworkingNode_Id.Zero,
                              jsonMessage,
                              CancellationToken
                          )
                      );

                // For further processing...
                await LogEvent(
                          OnJSONMessageReceived2,
                          loggingDelegate => loggingDelegate.Invoke(
                              timestamp,
                              this,
                              WebSocketConnection,
                              RequestTimestamp,
                              EventTrackingId,
                              sourceNodeId ?? NetworkingNode_Id.Zero,
                              jsonMessage,
                              CancellationToken
                          )
                      );

            }
            catch (Exception e)
            {
                await HandleErrors(
                          nameof(WWCPWebSocketServer),
                          nameof(ProcessTextMessage),
                          e
                      );
            }

        }

        #endregion

        #region (protected) ProcessBinaryMessage          (RequestTimestamp, Server, WebSocketConnection, Frame, EventTrackingId, BinaryMessage, CancellationToken)

        /// <summary>
        /// Process a HTTP WebSocket binary message.
        /// </summary>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="Server">The HTTP WebSocket server.</param>
        /// <param name="WebSocketConnection">The HTTP WebSocket connection.</param>
        /// <param name="EventTrackingId">An optional event tracking identification.</param>
        /// <param name="Frame">The HTTP WebSocket frame.</param>
        /// <param name="BinaryMessage">The received binary message.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        public override async Task ProcessBinaryMessage(DateTimeOffset             RequestTimestamp,
                                                        AWebSocketServer           Server,
                                                        WebSocketServerConnection  WebSocketConnection,
                                                        EventTracking_Id           EventTrackingId,
                                                        WebSocketFrame             Frame,
                                                        Byte[]                     BinaryMessage,
                                                        CancellationToken          CancellationToken)
        {

            // Fire the generic OnBinaryMessageReceived event of the base class!
            await base.ProcessBinaryMessage(
                      RequestTimestamp,
                      Server,
                      WebSocketConnection,
                      EventTrackingId,
                      Frame,
                      BinaryMessage,
                      CancellationToken
                  );

            try
            {

                var sourceNodeId  = WebSocketConnection.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.NetworkingNodeId);

                #region Initial checks

                if (BinaryMessage.Length == 0)
                {

                    await HandleErrors(
                              nameof(WWCPWebSocketServer),
                              nameof(ProcessTextMessage),
                              $"Received an empty binary message from {(
                                   sourceNodeId.HasValue
                                       ? $"'{sourceNodeId}' ({WebSocketConnection.RemoteSocket})"
                                       : $"'{WebSocketConnection.RemoteSocket}"
                              )}'!"
                          );

                    return;

                }

                #endregion

                var timestamp     = Timestamp.Now;

                // Just for logging!
                await LogEvent(
                          OnBinaryMessageReceived,
                          loggingDelegate => loggingDelegate.Invoke(
                              Timestamp.Now,
                              this,
                              WebSocketConnection,
                              RequestTimestamp,
                              EventTrackingId,
                              sourceNodeId ?? NetworkingNode_Id.Zero,
                              BinaryMessage,
                              CancellationToken
                          )
                      );

                // For further processing...
                await LogEvent(
                          OnBinaryMessageReceived2,
                          loggingDelegate => loggingDelegate.Invoke(
                              Timestamp.Now,
                              this,
                              WebSocketConnection,
                              RequestTimestamp,
                              EventTrackingId,
                              sourceNodeId ?? NetworkingNode_Id.Zero,
                              BinaryMessage,
                              CancellationToken
                          )
                      );

            }
            catch (Exception e)
            {
                await HandleErrors(
                          nameof(WWCPWebSocketServer),
                          nameof(ProcessBinaryMessage),
                          e
                      );
            }

        }

        #endregion


        // Send data...

        #region SendJSONMessage   (WebSocketConnection, JSONMessage,   RequestTimestamp, EventTrackingId, ...)

        /// <summary>
        /// Send the given JSON message.
        /// </summary>
        /// <param name="WebSocketConnection">A WebSocket connection.</param>
        /// <param name="JSONMessage">A JSON message.</param>
        /// <param name="RequestTimestamp">A request timestamp (for logging).</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CancellationToken">A cancellation token.</param>
        public async Task<SentMessageResult> SendJSONMessage(WebSocketServerConnection  WebSocketConnection,
                                                             JArray                     JSONMessage,
                                                             DateTimeOffset             RequestTimestamp,
                                                             EventTracking_Id           EventTrackingId,
                                                             CancellationToken          CancellationToken = default)
        {

            try
            {

                var sentStatus = await SendTextMessage(
                                           WebSocketConnection,
                                           JSONMessage.ToString(JSONFormatting),
                                           EventTrackingId,
                                           CancellationToken
                                       );

                await LogEvent(
                          OnJSONMessageSent,
                          loggingDelegate => loggingDelegate.Invoke(
                              Timestamp.Now,
                              this,
                              WebSocketConnection,
                              RequestTimestamp,
                              EventTrackingId,
                              JSONMessage,
                              sentStatus,
                              CancellationToken
                          )
                      );

                if (sentStatus == SentStatus.Success)
                    return SentMessageResult.Success(WebSocketConnection);

                return SentMessageResult.UnknownClient();

            }
            catch (Exception e)
            {
                return SentMessageResult.TransmissionFailed(e);
            }

        }

        #endregion

        #region SendBinaryMessage (WebSocketConnection, BinaryMessage, RequestTimestamp, EventTrackingId, ...)

        /// <summary>
        /// Send the given binary message.
        /// </summary>
        /// <param name="WebSocketConnection">A WebSocket connection.</param>
        /// <param name="BinaryMessage">A binary message.</param>
        /// <param name="RequestTimestamp">A request timestamp (for logging).</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CancellationToken">A cancellation token.</param>
        public async Task<SentMessageResult> SendBinaryMessage(WebSocketServerConnection  WebSocketConnection,
                                                               Byte[]                     BinaryMessage,
                                                               DateTimeOffset             RequestTimestamp,
                                                               EventTracking_Id           EventTrackingId,
                                                               CancellationToken          CancellationToken = default)
        {

            try
            {

                var sentStatus = await SendBinaryMessage(
                                           WebSocketConnection,
                                           BinaryMessage,
                                           EventTrackingId,
                                           CancellationToken
                                       );

                await LogEvent(
                          OnBinaryMessageSent,
                          loggingDelegate => loggingDelegate.Invoke(
                              Timestamp.Now,
                              this,
                              WebSocketConnection,
                              RequestTimestamp,
                              EventTrackingId,
                              BinaryMessage,
                              sentStatus,
                              CancellationToken
                          )
                      );

                if (sentStatus == SentStatus.Success)
                    return SentMessageResult.Success(WebSocketConnection);

                return SentMessageResult.UnknownClient();

            }
            catch (Exception e)
            {
                return SentMessageResult.TransmissionFailed(e);
            }

        }

        #endregion

        #region (protected) SendOnJSONMessageSent   (...)

        protected Task SendOnJSONMessageSent(DateTimeOffset            Timestamp,
                                             IWWCPWebSocketServer        Server,
                                             WebSocketServerConnection   WebSocketConnection,
                                             EventTracking_Id            EventTrackingId,
                                             DateTimeOffset            MessageTimestamp,
                                             JArray                      JSONMessage,
                                             SentStatus                  SentStatus,
                                             CancellationToken           CancellationToken)

            => LogEvent(
                   OnJSONMessageSent,
                   loggingDelegate => loggingDelegate.Invoke(
                       Timestamp,
                       Server,
                       WebSocketConnection,
                       MessageTimestamp,
                       EventTrackingId,
                       JSONMessage,
                       SentStatus,
                       CancellationToken
                   )
               );

        #endregion

        #region (protected) SendOnBinaryMessageSent (...)

        protected Task SendOnBinaryMessageSent(DateTimeOffset            Timestamp,
                                               IWWCPWebSocketServer        Server,
                                               WebSocketServerConnection   WebSocketConnection,
                                               EventTracking_Id            EventTrackingId,
                                               DateTimeOffset            MessageTimestamp,
                                               Byte[]                      BinaryMessage,
                                               SentStatus                  SentStatus,
                                               CancellationToken           CancellationToken)

            => LogEvent(
                   OnBinaryMessageSent,
                   loggingDelegate => loggingDelegate.Invoke(
                       Timestamp,
                       Server,
                       WebSocketConnection,
                       MessageTimestamp,
                       EventTrackingId,
                       BinaryMessage,
                       SentStatus,
                       CancellationToken
                   )
               );

        #endregion



        #region (private) LogEvent(Logger, LogHandler, ...)

        private async Task LogEvent<TDelegate>(TDelegate?                                         Logger,
                                               Func<TDelegate, Task>                              LogHandler,
                                               [CallerArgumentExpression(nameof(Logger))] String  EventName     = "",
                                               [CallerMemberName()]                       String  WWCPCommand   = "")

            where TDelegate : Delegate

        {
            if (Logger is not null)
            {
                try
                {

                    await Task.WhenAll(
                              Logger.GetInvocationList().
                                     OfType<TDelegate>().
                                     Select(LogHandler)
                          );

                }
                catch (Exception e)
                {
                    await HandleErrors(nameof(WWCPWebSocketServer), $"{WWCPCommand}.{EventName}", e);
                }
            }
        }

        #endregion

        #region (private) HandleErrors(Module, Caller, ErrorResponse)

        private Task HandleErrors(String  Module,
                                  String  Caller,
                                  String  ErrorResponse)
        {

            DebugX.Log($"{Module}.{Caller}: {ErrorResponse}");

            return Task.CompletedTask;

        }

        #endregion

        #region (private) HandleErrors(Module, Caller, ExceptionOccurred)

        private Task HandleErrors(String     Module,
                                  String     Caller,
                                  Exception  ExceptionOccurred)
        {

            DebugX.LogException(ExceptionOccurred, $"{Module}.{Caller}");

            return Task.CompletedTask;

        }

        #endregion


    }

}
