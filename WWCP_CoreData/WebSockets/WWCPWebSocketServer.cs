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
        public WWCPWebSocketServer(INetworkingNode                                                 NetworkingNode,

                                   String?                                                         HTTPServiceName              = DefaultHTTPServiceName,
                                   IIPAddress?                                                     IPAddress                    = null,
                                   IPPort?                                                         TCPPort                      = null,
                                   I18NString?                                                     Description                  = null,

                                   Boolean?                                                        RequireAuthentication        = true,
                                   IEnumerable<String>?                                            SecWebSocketProtocols        = null,
                                   SubprotocolSelectorDelegate?                                    SubprotocolSelector          = null,
                                   Boolean                                                         DisableWebSocketPings        = false,
                                   TimeSpan?                                                       WebSocketPingEvery           = null,
                                   TimeSpan?                                                       SlowNetworkSimulationDelay   = null,

                                   Func<X509Certificate2>?                                         ServerCertificateSelector    = null,
                                   RemoteTLSClientCertificateValidationHandler<IWebSocketServer>?  ClientCertificateValidator   = null,
                                   LocalCertificateSelectionHandler?                               LocalCertificateSelector     = null,
                                   SslProtocols?                                                   AllowedTLSProtocols          = null,
                                   Boolean?                                                        ClientCertificateRequired    = null,
                                   Boolean?                                                        CheckCertificateRevocation   = null,

                                   ServerThreadNameCreatorDelegate?                                ServerThreadNameCreator      = null,
                                   ServerThreadPriorityDelegate?                                   ServerThreadPrioritySetter   = null,
                                   Boolean?                                                        ServerThreadIsBackground     = null,
                                   ConnectionIdBuilder?                                            ConnectionIdBuilder          = null,
                                   TimeSpan?                                                       ConnectionTimeout            = null,
                                   UInt32?                                                         MaxClientConnections         = null,

                                   DNSClient?                                                      DNSClient                    = null,
                                   Boolean                                                         AutoStart                    = true)

            : base(IPAddress,
                   TCPPort         ?? IPPort.Auto,
                   HTTPServiceName ?? DefaultHTTPServiceName,
                   Description,

                   RequireAuthentication,
                   SecWebSocketProtocols,
                   SubprotocolSelector,
                   DisableWebSocketPings,
                   WebSocketPingEvery,
                   SlowNetworkSimulationDelay,

                   ServerCertificateSelector,
                   ClientCertificateValidator,
                   LocalCertificateSelector,
                   AllowedTLSProtocols,
                   ClientCertificateRequired,
                   CheckCertificateRevocation,

                   ServerThreadNameCreator,
                   ServerThreadPrioritySetter,
                   ServerThreadIsBackground,
                   ConnectionIdBuilder,
                   ConnectionTimeout,
                   MaxClientConnections,

                   DNSClient,
                   false)

        {

            this.NetworkingNode                  = NetworkingNode;

            //this.Logger                          = new ChargePointwebsocketClient.CPClientLogger(this,
            //                                                                                LoggingPath,
            //                                                                                LoggingContext,
            //                                                                                LogfileCreator);

            base.OnValidateTCPConnection        += ValidateTCPConnection;
            base.OnValidateWebSocketConnection  += ValidateWebSocketConnection;
            base.OnNewWebSocketConnection       += ProcessNewWebSocketConnection;
            base.OnCloseMessageReceived         += ProcessCloseMessage;

            base.OnTextMessageReceived          += ProcessTextMessage;
            base.OnBinaryMessageReceived        += ProcessBinaryMessage;

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

            if (AutoStart)
                Start();

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


        // Connection management...

        #region (protected) ValidateTCPConnection         (LogTimestamp, Server, Connection, EventTrackingId, CancellationToken)

        private Task<ConnectionFilterResponse> ValidateTCPConnection(DateTimeOffset                LogTimestamp,
                                                                     IWebSocketServer              Server,
                                                                     System.Net.Sockets.TcpClient  Connection,
                                                                     EventTracking_Id              EventTrackingId,
                                                                     CancellationToken             CancellationToken)
        {

            return Task.FromResult(ConnectionFilterResponse.Accepted());

        }

        #endregion

        #region (protected) ValidateWebSocketConnection   (LogTimestamp, Server, Connection, EventTrackingId, CancellationToken)

        private Task<HTTPResponse?> ValidateWebSocketConnection(DateTimeOffset             LogTimestamp,
                                                                IWebSocketServer           Server,
                                                                WebSocketServerConnection  Connection,
                                                                EventTracking_Id           EventTrackingId,
                                                                CancellationToken          CancellationToken)
        {

            #region Verify 'Sec-WebSocket-Protocol'...

            if (Connection.HTTPRequest?.SecWebSocketProtocol is null ||
                Connection.HTTPRequest?.SecWebSocketProtocol.Any() == false)
            {

                DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket}: Missing 'Sec-WebSocket-Protocol' HTTP header!");

                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder() {
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
                           new HTTPResponse.Builder() {
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

            #region Verify HTTP Authentication

            if (RequireAuthentication)
            {

                #region HTTP Basic Authentication

                if (Connection.HTTPRequest?.Authorization is HTTPBasicAuthentication basicAuthentication)
                {

                    if (ClientLogins.TryGetValue(basicAuthentication.Username, out var securePassword) &&
                        securePassword.Equals(basicAuthentication.Password))
                    {
                        DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} using authorization: '{basicAuthentication.Username}' / '{basicAuthentication.Password}'");
                        return Task.FromResult<HTTPResponse?>(null);
                    }
                    else
                        DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} invalid authorization: '{basicAuthentication.Username}' / '{basicAuthentication.Password}'!");

                }

                #endregion

                #region HTTP TOTP  Authentication

                else if (Connection.HTTPRequest?.Authorization is HTTPTOTPAuthentication totpAuthentication)
                {

                    if (ClientTOTPConfig.TryGetValue(totpAuthentication.Username, out var totpConfig))
                    {

                        var (previousTOTP,
                             currentTOTP,
                             nextTOTP,
                             remainingTime,
                             endTime) = TOTPGenerator.GenerateTOTPs(
                                            Timestamp.Now,
                                            totpConfig.SharedSecret,
                                            totpConfig.ValidityTime,
                                            totpConfig.Length,
                                            totpConfig.Alphabet
                                        );

                        if (totpAuthentication.TOTP == previousTOTP ||
                            totpAuthentication.TOTP == currentTOTP  ||
                            totpAuthentication.TOTP == nextTOTP)
                        {
                            DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} using TOTP authorization: '{totpAuthentication.Username}' / '{totpAuthentication.TOTP}'");
                            return Task.FromResult<HTTPResponse?>(null);
                        }
                        else
                            DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} invalid or outdated TOTP authorization: '{totpAuthentication.Username}' / '{totpAuthentication.TOTP}'!");

                    }
                    else
                        DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} invalid TOTP authorization: '{totpAuthentication.Username}' / '{totpAuthentication.TOTP}'!");

                }

                #endregion

                #region QueryString u/p    Authentication

                else if (Connection.HTTPRequest?.QueryString is not null)
                {

                    var queryString  = Connection.HTTPRequest?.QueryString;
                    var username     = queryString?.GetString("u") ?? "";
                    var password     = queryString?.GetString("p") ?? "";

                    if (ClientLogins.TryGetValue(username, out var securePassword) &&
                        securePassword.Equals(password))
                    {
                        DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} using authorization: '{username}' / '{password}'");
                        return Task.FromResult<HTTPResponse?>(null);
                    }
                    else
                        DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} invalid authorization: '{username}' / '{password}'!");

                }

                #endregion

                #region QueryString u/totp Authentication

                else if (Connection.HTTPRequest?.QueryString is not null)
                {

                    var queryString  = Connection.HTTPRequest?.QueryString;
                    var username     = queryString?.GetString("u")    ?? "";
                    var totp         = queryString?.GetString("totp") ?? "";

                    if (ClientTOTPConfig.TryGetValue(username, out var totpConfig))
                    {

                        var (previousTOTP,
                             currentTOTP,
                             nextTOTP,
                             remainingTime,
                             endTime) = TOTPGenerator.GenerateTOTPs(
                                            Timestamp.Now,
                                            totpConfig.SharedSecret,
                                            totpConfig.ValidityTime,
                                            totpConfig.Length,
                                            totpConfig.Alphabet
                                        );

                        if (totp == previousTOTP ||
                            totp == currentTOTP  ||
                            totp == nextTOTP)
                        {
                            DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} using authorization: '{username}' / '{totp}'");
                            return Task.FromResult<HTTPResponse?>(null);
                        }
                        else
                            DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} invalid or outdated TOTP authorization: '{username}' / '{totp}'!");

                    }
                    else
                        DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} invalid authorization: '{username}' / '{totp}'!");

                }

                #endregion

                else
                    DebugX.Log($"{nameof(WWCPWebSocketServer)} connection from {Connection.RemoteSocket} missing or invalid authorization!");


                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder() {
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

        #region (protected) ProcessNewWebSocketConnection (LogTimestamp, Server, Connection, SharedSubprotocols, EventTrackingId, CancellationToken)

        protected async Task ProcessNewWebSocketConnection(DateTimeOffset             LogTimestamp,
                                                           IWebSocketServer           Server,
                                                           WebSocketServerConnection  Connection,
                                                           IEnumerable<String>        SharedSubprotocols,
                                                           String?                    SelectedSubprotocol,
                                                           EventTracking_Id           EventTrackingId,
                                                           CancellationToken          CancellationToken)
        {

            if (Connection.HTTPRequest is null)
                return;

            NetworkingNode_Id? networkingNodeId = null;

            #region Parse TLS Client Certificate CommonName, or...

            // We already validated and therefore trust this certificate!
            if (Connection.HTTPRequest.ClientCertificate is not null)
            {

                var x509CommonName = Connection.HTTPRequest.ClientCertificate.GetNameInfo(X509NameType.SimpleName, forIssuer: false);

                if (NetworkingNode_Id.TryParse(x509CommonName, out var networkingNodeId1))
                {
                    networkingNodeId = networkingNodeId1;
                }

            }

            #endregion

            #region ...check HTTP Basic Authentication, or...

            else if (Connection.HTTPRequest.Authorization is HTTPBasicAuthentication httpBasicAuthentication &&
                     NetworkingNode_Id.TryParse(httpBasicAuthentication.Username, out var networkingNodeId2))
            {
                networkingNodeId = networkingNodeId2;
            }

            #endregion


            //ToDo: This might be a DOS attack vector!

            #region ...try to get the NetworkingNodeId from the HTTP request path suffix...

            else
            {

                var path = Connection.HTTPRequest.Path.ToString();

                if (NetworkingNode_Id.TryParse(path[(path.LastIndexOf('/') + 1)..],
                    out var networkingNodeId3))
                {
                    networkingNodeId = networkingNodeId3;
                }

            }

            #endregion

            #region ...try to get the NetworkingNodeId from the HTTP query string (?u=CS001&p=xxx)...

            if (!networkingNodeId.HasValue && Connection.HTTPRequest.QueryString.Count() > 0 &&
                Connection.HTTPRequest.QueryString.TryGetString("u", out var u) &&
                u.IsNotNullOrEmpty() &&
                NetworkingNode_Id.TryParse(u, out var _networkingNodeId))
            {
                networkingNodeId = _networkingNodeId;
            }

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

                if (!connectedNetworkingNodes.TryAdd(
                        networkingNodeId.Value,
                        new NetworkingNodeConnections(
                            networkingNodeId.Value,
                            [ Connection ]
                        )
                   ))
                {

                    DebugX.Log($"{nameof(WWCPWebSocketServer)} Duplicate networking node '{networkingNodeId.Value}' detected: Trying to close old one(s)!");

                    if (connectedNetworkingNodes.TryRemove(networkingNodeId.Value, out var oldConnection))
                    {
                        foreach (var webSocketServerConnection in oldConnection.WebSocketServerConnections)
                        {
                            try
                            {
                                await webSocketServerConnection.Close(
                                          WebSocketFrame.ClosingStatusCode.NormalClosure,
                                          "Newer connection detected!",
                                          CancellationToken
                                      );
                            }
                            catch (Exception e)
                            {
                                DebugX.Log($"{nameof(WWCPWebSocketServer)} Closing old HTTP WebSocket connection from {webSocketServerConnection.RemoteSocket} failed: {e.Message}");
                            }
                        }
                    }

                    connectedNetworkingNodes.TryAdd(
                        networkingNodeId.Value,
                        new NetworkingNodeConnections(
                            networkingNodeId.Value,
                            [ Connection ]
                        )
                    );

                }

                #endregion

                #region Send OnNewNetworkingNodeWSConnection event

                await LogEvent(
                          OnNetworkingNodeNewWebSocketConnection,
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
                                                 IWebSocketServer                  Server,
                                                 WebSocketServerConnection         Connection,
                                                 WebSocketFrame                    Frame,
                                                 EventTracking_Id                  EventTrackingId,
                                                 WebSocketFrame.ClosingStatusCode  StatusCode,
                                                 String?                           Reason,
                                                 CancellationToken                 CancellationToken)
        {

            if (Connection.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.NetworkingNodeId, out var networkingNodeId))
            {

                connectedNetworkingNodes.TryRemove(networkingNodeId, out _);

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
        /// <param name="Frame">The HTTP WebSocket frame.</param>
        /// <param name="EventTrackingId">An optional event tracking identification.</param>
        /// <param name="TextMessage">The received text message.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        public async Task ProcessTextMessage(DateTimeOffset             RequestTimestamp,
                                             IWebSocketServer           Server,
                                             WebSocketServerConnection  WebSocketConnection,
                                             WebSocketFrame             Frame,
                                             EventTracking_Id           EventTrackingId,
                                             String                     TextMessage,
                                             CancellationToken          CancellationToken)
        {

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
        /// <param name="Frame">The HTTP WebSocket frame.</param>
        /// <param name="EventTrackingId">An optional event tracking identification.</param>
        /// <param name="BinaryMessage">The received binary message.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        public async Task ProcessBinaryMessage(DateTimeOffset             RequestTimestamp,
                                               IWebSocketServer           Server,
                                               WebSocketServerConnection  WebSocketConnection,
                                               WebSocketFrame             Frame,
                                               EventTracking_Id           EventTrackingId,
                                               Byte[]                     BinaryMessage,
                                               CancellationToken          CancellationToken)
        {

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
