/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OCPP <https://github.com/OpenChargingCloud/WWCP_OCPP>
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

using System.Security.Authentication;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP.WebSockets
{

    /// <summary>
    /// The networking node HTTP WebSocket client runs on a networking node
    /// and connects to a CSMS to invoke methods.
    /// </summary>
    public partial class WWCPWebSocketClient : org.GraphDefined.Vanaheimr.Hermod.WebSocket.WebSocketClient,
                                               IWWCPWebSocketClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const String  DefaultHTTPUserAgent   = $"GraphDefined WWCP WebSocket Client";

        private    const String  LogfileName            = "NetworkingNodeWSClient.log";

        #endregion

        #region Properties

        /// <summary>
        /// The parent networking node.
        /// </summary>
        public INetworkingNode  NetworkingNode    { get; }

        /// <summary>
        /// The Networking Mode.
        /// </summary>
        public NetworkingMode   NetworkingMode    { get; } = NetworkingMode.Standard;

        /// <summary>
        /// The JSON formatting to use.
        /// </summary>
        public Formatting       JSONFormatting    { get; set; }
            = Formatting.None;

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a JSON message was sent.
        /// </summary>
        public     event OnWebSocketClientJSONMessageSentDelegate?         OnJSONMessageSent;

        /// <summary>
        /// An event sent whenever a JSON message was received.
        /// </summary>
        public     event OnWebSocketClientJSONMessageReceivedDelegate?     OnJSONMessageReceived;


        /// <summary>
        /// An event sent whenever a binary message was sent.
        /// </summary>
        public new event OnWebSocketClientBinaryMessageSentDelegate?       OnBinaryMessageSent;

        /// <summary>
        /// An event sent whenever a binary message was received.
        /// </summary>
        public new event OnWebSocketClientBinaryMessageReceivedDelegate?   OnBinaryMessageReceived;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new networking node websocket client running on a networking node
        /// and connecting to a CSMS to invoke methods.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the HTTP endpoint to connect to.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="Description">An optional description of this HTTP/websocket client.</param>
        /// <param name="RemoteCertificateValidator">The remote SSL/TLS certificate validator.</param>
        /// <param name="LocalCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="ClientCert">The SSL/TLS client certificate to use of HTTP authentication.</param>
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="URLPathPrefix">An optional default URL path prefix.</param>
        /// <param name="HTTPAuthentication">The WebService-Security username/password.</param>
        /// <param name="RequestTimeout">An optional Request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="HTTPLogger">A HTTP logger.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public WWCPWebSocketClient(INetworkingNode                                                 NetworkingNode,
                                   URL                                                             RemoteURL,
                                   HTTPHostname?                                                   VirtualHostname              = null,
                                   I18NString?                                                     Description                  = null,
                                   Boolean?                                                        PreferIPv4                   = null,
                                   RemoteTLSServerCertificateValidationHandler<org.GraphDefined.Vanaheimr.Hermod.WebSocket.IWebSocketClient>?  RemoteCertificateValidator   = null,
                                   LocalCertificateSelectionHandler?                               LocalCertificateSelector     = null,
                                   X509Certificate?                                                ClientCert                   = null,
                                   SslProtocols?                                                   TLSProtocol                  = null,
                                   String                                                          HTTPUserAgent                = DefaultHTTPUserAgent,
                                   IHTTPAuthentication?                                            HTTPAuthentication           = null,
                                   TimeSpan?                                                       RequestTimeout               = null,
                                   TransmissionRetryDelayDelegate?                                 TransmissionRetryDelay       = null,
                                   UInt16?                                                         MaxNumberOfRetries           = 3,
                                   UInt32?                                                         InternalBufferSize           = null,

                                   IEnumerable<String>?                                            SecWebSocketProtocols        = null,
                                   NetworkingMode?                                                 NetworkingMode               = null,

                                   Boolean                                                         DisableWebSocketPings        = false,
                                   TimeSpan?                                                       WebSocketPingEvery           = null,
                                   TimeSpan?                                                       SlowNetworkSimulationDelay   = null,

                                   Boolean                                                         DisableMaintenanceTasks      = false,
                                   TimeSpan?                                                       MaintenanceEvery             = null,

                                   String?                                                         LoggingPath                  = null,
                                   String                                                          LoggingContext               = null, //CPClientLogger.DefaultContext,
                                   LogfileCreatorDelegate?                                         LogfileCreator               = null,
                                   HTTPClientLogger?                                               HTTPLogger                   = null,
                                   DNSClient?                                                      DNSClient                    = null)

            : base(RemoteURL,
                   VirtualHostname,
                   Description,
                   PreferIPv4,
                   RemoteCertificateValidator,
                   LocalCertificateSelector,
                   ClientCert,
                   TLSProtocol,
                   HTTPUserAgent ?? DefaultHTTPUserAgent,
                   HTTPAuthentication,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries,
                   InternalBufferSize,

                   SecWebSocketProtocols,

                   DisableWebSocketPings,
                   WebSocketPingEvery,
                   SlowNetworkSimulationDelay,

                   DisableMaintenanceTasks,
                   MaintenanceEvery,

                   LoggingPath,
                   LoggingContext,
                   LogfileCreator,
                   HTTPLogger,
                   DNSClient)

        {

            this.NetworkingNode           = NetworkingNode;
            this.NetworkingMode           = NetworkingMode ?? WWCP.NetworkingNode.NetworkingMode.Standard;

            base.OnTextMessageReceived   += ProcessWebSocketTextFrame;
            base.OnBinaryMessageReceived += ProcessWebSocketBinaryFrame;

            //this.Logger                   = new ChargePointwebsocketClient.CPClientLogger(this,
            //                                                                         LoggingPath,
            //                                                                         LoggingContext,
            //                                                                         LogfileCreator);

        }

        #endregion


        #region ProcessWebSocketTextFrame   (RequestTimestamp, Client, Connection, Frame, EventTrackingId, TextMessage,   CancellationToken)

        /// <summary>
        /// Process a HTTP WebSocket text message.
        /// </summary>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="Client">The HTTP WebSocket client.</param>
        /// <param name="Connection">The HTTP WebSocket connection.</param>
        /// <param name="Frame">The HTTP WebSocket frame.</param>
        /// <param name="EventTrackingId">An optional event tracking identification.</param>
        /// <param name="TextMessage">The received text message.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        public async Task ProcessWebSocketTextFrame(DateTime                   RequestTimestamp,
                                                    org.GraphDefined.Vanaheimr.Hermod.WebSocket.WebSocketClient            Client,
                                                    org.GraphDefined.Vanaheimr.Hermod.WebSocket.WebSocketClientConnection  Connection,
                                                    org.GraphDefined.Vanaheimr.Hermod.WebSocket.WebSocketFrame             Frame,
                                                    EventTracking_Id           EventTrackingId,
                                                    String                     TextMessage,
                                                    CancellationToken          CancellationToken)
        {

            try
            {

                var sourceNodeId = Connection.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.NetworkingNodeId);

                #region Initial checks

                if (TextMessage == "[]" ||
                    TextMessage.IsNullOrEmpty())
                {

                    await HandleErrors(
                              nameof(WWCPWebSocketClient),
                              nameof(ProcessWebSocketBinaryFrame),
                              $"Received an empty text message from {(
                                   sourceNodeId.HasValue
                                       ? $"'{sourceNodeId}' ({Connection.RemoteSocket})"
                                       : $"'{Connection.RemoteSocket}"
                              )}'!"
                          );

                    return;

                }

                #endregion


                var jsonMessage = JArray.Parse(TextMessage);

                await LogEvent(
                          OnJSONMessageReceived,
                          loggingDelegate => loggingDelegate.Invoke(
                              Timestamp.Now,
                              this,
                              webSocketClientConnection,
                              EventTrackingId,
                              RequestTimestamp,
                              sourceNodeId ?? NetworkingNode_Id.Zero,
                              jsonMessage,
                              CancellationToken
                          )
                      );

            }
            catch (Exception e)
            {
                await HandleErrors(
                          nameof(WWCPWebSocketClient),
                          nameof(ProcessWebSocketTextFrame),
                          e
                      );
            }

        }

        #endregion

        #region ProcessWebSocketBinaryFrame (RequestTimestamp, Client, Connection, Frame, EventTrackingId, BinaryMessage, CancellationToken)

        /// <summary>
        /// Process a HTTP WebSocket binary message.
        /// </summary>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="Client">The HTTP WebSocket client.</param>
        /// <param name="Connection">The HTTP WebSocket connection.</param>
        /// <param name="Frame">The HTTP WebSocket frame.</param>
        /// <param name="EventTrackingId">An optional event tracking identification.</param>
        /// <param name="BinaryMessage">The received binary message.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        public async Task ProcessWebSocketBinaryFrame(DateTime                   RequestTimestamp,
                                                      org.GraphDefined.Vanaheimr.Hermod.WebSocket.WebSocketClient            Client,
                                                      org.GraphDefined.Vanaheimr.Hermod.WebSocket.WebSocketClientConnection  Connection,
                                                      org.GraphDefined.Vanaheimr.Hermod.WebSocket.WebSocketFrame             Frame,
                                                      EventTracking_Id           EventTrackingId,
                                                      Byte[]                     BinaryMessage,
                                                      CancellationToken          CancellationToken)
        {

            try
            {

                var sourceNodeId = Connection.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.NetworkingNodeId);

                #region Initial checks

                if (BinaryMessage.Length == 0)
                {

                    await HandleErrors(
                              nameof(WWCPWebSocketClient),
                              nameof(ProcessWebSocketBinaryFrame),
                              $"Received an empty binary message from {(
                                   sourceNodeId.HasValue
                                       ? $"'{sourceNodeId}' ({Connection.RemoteSocket})"
                                       : $"'{Connection.RemoteSocket}"
                              )}'!"
                          );

                    return;

                }

                #endregion


                await LogEvent(
                          OnBinaryMessageReceived,
                          loggingDelegate => loggingDelegate.Invoke(
                              Timestamp.Now,
                              this,
                              Connection,
                              EventTrackingId,
                              RequestTimestamp,
                              sourceNodeId ?? NetworkingNode_Id.Zero,
                              BinaryMessage,
                              CancellationToken
                          )
                      );

            }
            catch (Exception e)
            {
                await HandleErrors(
                          nameof(WWCPWebSocketClient),
                          nameof(ProcessWebSocketBinaryFrame),
                          e
                      );
            }

        }

        #endregion


        #region SendJSONMessage         (JSONMessage, ...)

        /// <summary>
        /// Send (and forget) the given JSON OCPP request message.
        /// </summary>
        /// <param name="JSONMessage">A JSON OCPP request message.</param>
        public async Task<SentMessageResult> SendJSONMessage(JArray             JSONMessage,
                                                             DateTime           MessageTimestamp,
                                                             EventTracking_Id   EventTrackingId,
                                                             CancellationToken  CancellationToken   = default)
        {

            try
            {

                var sentStatus = await SendTextMessage(
                                           JSONMessage.ToString(JSONFormatting),
                                           EventTrackingId,
                                           CancellationToken
                                       );

                await LogEvent(
                          OnJSONMessageSent,
                          loggingDelegate => loggingDelegate.Invoke(
                              Timestamp.Now,
                              this,
                              webSocketClientConnection,
                              EventTrackingId,
                              MessageTimestamp,
                              JSONMessage,
                              sentStatus,
                              CancellationToken
                          )
                      );

                return sentStatus switch {
                           org.GraphDefined.Vanaheimr.Hermod.WebSocket.SentStatus.Success  => SentMessageResult.Success(webSocketClientConnection),
                           org.GraphDefined.Vanaheimr.Hermod.WebSocket.SentStatus.Error    => SentMessageResult.Unknown(),
                           _                                                               => SentMessageResult.Unknown(),
                       };

            }
            catch (Exception e)
            {
                return SentMessageResult.TransmissionFailed(e, webSocketClientConnection);
            }

        }

        #endregion

        #region SendBinaryMessage       (BinaryMessage, ...)

        /// <summary>
        /// Send (and forget) the given binary OCPP request message.
        /// </summary>
        /// <param name="BinaryRequestMessage">A binary OCPP request message.</param>
        public async Task<SentMessageResult> SendBinaryMessage(Byte[]             BinaryMessage,
                                                               DateTime           MessageTimestamp,
                                                               EventTracking_Id   EventTrackingId,
                                                               CancellationToken  CancellationToken   = default)
        {

            try
            {

                //BinaryRequestMessage.NetworkingMode = NetworkingMode;
                //RequestMessage.RequestTimeout ??= RequestMessage.RequestTimestamp + (RequestTimeout ?? DefaultRequestTimeout);

                //var binaryMessage  = BinaryRequestMessage.ToByteArray();

                var sentStatus = await SendBinaryMessage(
                                           BinaryMessage,
                                           EventTrackingId,
                                           CancellationToken
                                       );

                await LogEvent(
                          OnBinaryMessageSent,
                          loggingDelegate => loggingDelegate.Invoke(
                              Timestamp.Now,
                              this,
                              webSocketClientConnection,
                              EventTrackingId,
                              MessageTimestamp,
                              BinaryMessage,
                              sentStatus,
                              CancellationToken
                          )
                      );

                return sentStatus switch {
                           org.GraphDefined.Vanaheimr.Hermod.WebSocket.SentStatus.Success  => SentMessageResult.Success(webSocketClientConnection),
                           org.GraphDefined.Vanaheimr.Hermod.WebSocket.SentStatus.Error    => SentMessageResult.Unknown(),
                           _                                                               => SentMessageResult.Unknown(),
                       };

            }
            catch (Exception e)
            {
                return SentMessageResult.TransmissionFailed(e, webSocketClientConnection);
            }

        }

        #endregion



        #region (private) LogEvent(Logger, LogHandler, ...)

        private Task LogEvent<TDelegate>(TDelegate?                                         Logger,
                                         Func<TDelegate, Task>                              LogHandler,
                                         [CallerArgumentExpression(nameof(Logger))] String  EventName     = "",
                                         [CallerMemberName()]                       String  OCPPCommand   = "")

            where TDelegate : Delegate

                => LogEvent(
                       nameof(WWCPWebSocketClient),
                       Logger,
                       LogHandler,
                       EventName,
                       OCPPCommand
                   );

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
