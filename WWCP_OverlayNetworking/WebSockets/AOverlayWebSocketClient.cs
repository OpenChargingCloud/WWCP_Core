/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OverlayNetworking <https://github.com/OpenChargingCloud/WWCP_Core>
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

using System.Reflection;
using System.Collections.Concurrent;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

using cloud.charging.open.protocols.WWCP.OverlayNetworking;
using cloud.charging.open.protocols.WWCP.OverlayNetworking.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking
{

    public delegate Task  OnWebSocketClientJSONMessageResponseDelegate  (DateTime           Timestamp,
                                                                         IEventSender       Client,
                                                                         EventTracking_Id   EventTrackingId,
                                                                         DateTime           RequestTimestamp,
                                                                         JArray?            JSONRequestMessage,
                                                                         Byte[]?            BinaryRequestMessage,
                                                                         DateTime           ResponseTimestamp,
                                                                         JArray             ResponseMessage);

    public delegate Task  OnWebSocketClientBinaryMessageResponseDelegate(DateTime           Timestamp,
                                                                         IEventSender       Client,
                                                                         EventTracking_Id   EventTrackingId,
                                                                         DateTime           RequestTimestamp,
                                                                         JArray?            JSONRequestMessage,
                                                                         Byte[]?            BinaryRequestMessage,
                                                                         DateTime           ResponseTimestamp,
                                                                         Byte[]             ResponseMessage);


    public class OverlayWebSocketClient : AOverlayWebSocketClient
    {

        #region Constructor(s)

        /// <summary>
        /// Initialize a new HTTP server for the CSMS HTTP/WebSocket/JSON API.
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
        public OverlayWebSocketClient(NetworkingNode_Id                                               NetworkingNodeIdentity,

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


            : base(NetworkingNodeIdentity,

                   RemoteURL,
                   VirtualHostname,
                   Description,
                   PreferIPv4,
                   RemoteCertificateValidator,
                   LocalCertificateSelector,
                   ClientCert,
                   TLSProtocol,
                   HTTPUserAgent,
                   HTTPAuthentication,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries,
                   InternalBufferSize,

                   SecWebSocketProtocols,
                   NetworkingMode,

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

        }

        #endregion

    }


    /// <summary>
    /// An EEBus HTTP WebSocket client runs on a charging station or networking node
    /// and connects to a CSMS or another networking node to invoke EEBus commands.
    /// </summary>
    public abstract class AOverlayWebSocketClient : WebSocketClient,
                                                    IEventSender
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const        String                                              DefaultHTTPUserAgent              = $"GraphDefined EEBus {Version.String} Charging Station HTTP WebSocket Client";

        public static readonly  TimeSpan                                            DefaultRequestTimeout             = TimeSpan.FromSeconds(30);

        protected readonly      ConcurrentDictionary<Request_Id, SendRequestState>  requests                          = [];

        protected readonly      Dictionary<String, MethodInfo>                      incomingMessageProcessorsLookup   = [];

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this charging station.
        /// </summary>
        public NetworkingNode_Id                     Id                 { get; }

        /// <summary>
        /// The sender identification.
        /// </summary>
        String IEventSender.Id
            => Id.ToString();

        /// <summary>
        /// The JSON formatting to use.
        /// </summary>
        public Formatting                            JSONFormatting     { get; set; } = Formatting.None;


        public NetworkingMode                        NetworkingMode     { get; set; } = NetworkingMode.Standard;

        /// <summary>
        /// The attached EEBus CP client (HTTP/websocket client) logger.
        /// </summary>
        //public ChargePointWSClient.CPClientLogger    Logger             { get; }

        #endregion

        #region Events

        public event OnWebSocketClientJSONMessageResponseDelegate?    OnJSONMessageResponseReceived;
        public event OnWebSocketClientJSONMessageResponseDelegate?    OnJSONMessageResponseSent;

        public event OnWebSocketClientBinaryMessageResponseDelegate?  OnBinaryMessageResponseReceived;
        public event OnWebSocketClientBinaryMessageResponseDelegate?  OnBinaryMessageResponseSent;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EEBus HTTP WebSocket client running, e.g on a charging station
        /// and connecting to a CSMS to invoke methods.
        /// </summary>
        /// <param name="NetworkingNodeIdentity">The unique identification of this charging station.</param>
        /// 
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
        public AOverlayWebSocketClient(NetworkingNode_Id                                               NetworkingNodeIdentity,

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
                   HTTPUserAgent,
                   HTTPAuthentication,
                   RequestTimeout ?? DefaultRequestTimeout,
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

            this.Id              = NetworkingNodeIdentity;
            this.NetworkingMode  = NetworkingMode ?? WebSockets.NetworkingMode.Standard;

            //this.Logger          = new ChargePointwebsocketClient.CPClientLogger(this,
            //                                                                LoggingPath,
            //                                                                LoggingContext,
            //                                                                LogfileCreator);

            base.OnTextMessageReceived   += ProcessWebSocketTextFrame;
            base.OnBinaryMessageReceived += ProcessWebSocketBinaryFrame;

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
                                                    WebSocketClient            Client,
                                                    WebSocketClientConnection  Connection,
                                                    WebSocketFrame             Frame,
                                                    EventTracking_Id           EventTrackingId,
                                                    String                     TextMessage,
                                                    CancellationToken          CancellationToken)
        {

            if (TextMessage == "[]" ||
                TextMessage.IsNullOrEmpty())
            {
                DebugX.Log($"Received an empty JSON message within {nameof(AOverlayWebSocketClient)}!");
                return;
            }

            try
            {

                var jsonArray = JArray.Parse(TextMessage);
              //  var sourceNodeId = Connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey) ?? NetworkingNode_Id.Zero;

                if      (JSONRequestMessage.     TryParse(jsonArray, out var jsonRequestMessage,  out var requestParsingError, RequestTimestamp, null, EventTrackingId, null, CancellationToken))
                {

                    JSONResponseMessage?      EEBusJSONResponse     = null;
                    BinaryResponseMessage?    EEBusBinaryResponse   = null;
                    JSONRequestErrorMessage?  EEBusRequestError     = null;

                    // Try to call the matching 'incoming message processor'
                    if (incomingMessageProcessorsLookup.TryGetValue(jsonRequestMessage.Action, out var methodInfo) && methodInfo is not null)
                    {

                        #region Call 'incoming message' processor

                        var result = methodInfo.Invoke(this,
                                                       [ jsonRequestMessage.RequestTimestamp,
                                                         Connection,
                                                         jsonRequestMessage.DestinationId,
                                                         jsonRequestMessage.NetworkPath,
                                                         jsonRequestMessage.EventTrackingId,
                                                         jsonRequestMessage.RequestId,
                                                         jsonRequestMessage.Payload,
                                                         jsonRequestMessage.CancellationToken ]);

                        #endregion

                             if (result is Task<Tuple<JSONResponseMessage?,   JSONRequestErrorMessage?>> jsonProcessor)
                        {

                            (EEBusJSONResponse,   EEBusRequestError) = await jsonProcessor;

                            #region Send response...

                            if (EEBusJSONResponse is not null)
                            {

                                if (EEBusJSONResponse.NetworkingMode == NetworkingMode.Unknown)
                                    EEBusJSONResponse = EEBusJSONResponse.ChangeNetworkingMode(NetworkingMode);

                                var sendStatus = await SendTextMessage(
                                                           EEBusJSONResponse.ToJSON().ToString(JSONFormatting),
                                                           EventTrackingId,
                                                           CancellationToken
                                                       );

                            }

                            #endregion

                            #region ..., or send error response!

                            if (EEBusRequestError is not null)
                            {
                                // CALL RESULT ERROR: New in EEBus v2.1++
                            }

                            #endregion


                            #region OnJSONMessageResponseSent

                            try
                            {

                                OnJSONMessageResponseSent?.Invoke(Timestamp.Now,
                                                                  this,
                                                                  EventTrackingId,
                                                                  RequestTimestamp,
                                                                  jsonArray,
                                                                  null,
                                                                  Timestamp.Now,
                                                                  EEBusJSONResponse?.ToJSON() ?? EEBusRequestError?.ToJSON() ?? []);

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketClient) + "." + nameof(OnJSONMessageResponseSent));
                            }

                            #endregion

                        }

                        else if (result is Task<Tuple<BinaryResponseMessage?, JSONRequestErrorMessage?>> binaryProcessor)
                        {

                            (EEBusBinaryResponse, EEBusRequestError) = await binaryProcessor;

                            #region Send response...

                            if (EEBusBinaryResponse is not null)
                            {

                                var sendStatus = await SendBinaryMessage(
                                                           EEBusBinaryResponse.ToByteArray(),
                                                           EventTrackingId,
                                                           CancellationToken
                                                       );

                            }

                            #endregion

                            #region ..., or send error response!

                            if (EEBusRequestError is not null)
                            {
                                // CALL RESULT ERROR: New in EEBus v2.1++
                            }

                            #endregion


                            #region OnBinaryMessageResponseSent

                            try
                            {

                                OnBinaryMessageResponseSent?.Invoke(Timestamp.Now,
                                                                    this,
                                                                    EventTrackingId,
                                                                    RequestTimestamp,
                                                                    jsonArray,
                                                                    null,
                                                                    Timestamp.Now,
                                                                    EEBusBinaryResponse?.ToByteArray() ?? EEBusRequestError?.ToByteArray() ?? []);

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketClient) + "." + nameof(OnBinaryMessageResponseSent));
                            }

                            #endregion

                        }

                        else
                            DebugX.Log($"Received undefined '{jsonRequestMessage.Action}' JSON request message handler within {nameof(AOverlayWebSocketClient)}!");

                    }
                    else
                        DebugX.Log($"Received unknown '{jsonRequestMessage.Action}' JSON request message handler within {nameof(AOverlayWebSocketClient)}!");

                }

                else if (JSONResponseMessage.    TryParse(jsonArray, out var jsonResponseMessage, out var responseParsingError))
                {

                    if (requests.TryGetValue(jsonResponseMessage.RequestId, out var sendRequestState) &&
                        sendRequestState is not null)
                    {

                        sendRequestState.ResponseTimestamp  = Timestamp.Now;
                        sendRequestState.JSONResponse       = jsonResponseMessage;

                        #region OnJSONMessageResponseReceived

                        try
                        {

                            OnJSONMessageResponseReceived?.Invoke(Timestamp.Now,
                                                                  this,
                                                                  EventTrackingId,
                                                                  sendRequestState.RequestTimestamp,
                                                                  sendRequestState.JSONRequest?.     ToJSON()      ?? [],
                                                                  sendRequestState.BinaryRequest?.   ToByteArray() ?? [],
                                                                  sendRequestState.ResponseTimestamp.Value,
                                                                  sendRequestState.JSONResponse?.    ToJSON()      ?? []);

                        }
                        catch (Exception e)
                        {
                            DebugX.Log(e, nameof(AOverlayWebSocketClient) + "." + nameof(OnJSONMessageResponseReceived));
                        }

                        #endregion

                    }
                    else
                        DebugX.Log($"Received an EEBus JSON response message having an unknown request identification within {nameof(AOverlayWebSocketClient)}: '{jsonResponseMessage}'!");

                }

                else if (JSONRequestErrorMessage.TryParse(jsonArray, out var jsonRequestErrorMessage))
                {

                    if (requests.TryGetValue(jsonRequestErrorMessage.RequestId, out var sendRequestState) &&
                        sendRequestState is not null)
                    {

                        sendRequestState.ResponseTimestamp        = Timestamp.Now;
                        sendRequestState.JSONRequestErrorMessage  = jsonRequestErrorMessage;

                        #region OnJSONMessageResponseReceived

                        try
                        {

                            OnJSONMessageResponseReceived?.Invoke(Timestamp.Now,
                                                                  this,
                                                                  EventTrackingId,
                                                                  sendRequestState.RequestTimestamp,
                                                                  sendRequestState.JSONRequest?.            ToJSON()      ?? [],
                                                                  sendRequestState.BinaryRequest?.          ToByteArray() ?? [],
                                                                  sendRequestState.ResponseTimestamp.       Value,
                                                                  sendRequestState.JSONRequestErrorMessage?.ToJSON()      ?? []);

                        }
                        catch (Exception e)
                        {
                            DebugX.Log(e, nameof(AOverlayWebSocketClient) + "." + nameof(OnJSONMessageResponseReceived));
                        }

                        #endregion

                    }


                    DebugX.Log(nameof(AOverlayWebSocketClient), " Received unknown EEBus error message: " + TextMessage);


                }

                else if (requestParsingError  is not null)
                    DebugX.Log($"Failed to parse a JSON request message within {nameof(AOverlayWebSocketClient)}: '{requestParsingError}'{Environment.NewLine}'{TextMessage}'!");

                else if (responseParsingError is not null)
                    DebugX.Log($"Failed to parse a JSON response message within {nameof(AOverlayWebSocketClient)}: '{responseParsingError}'{Environment.NewLine}'{TextMessage}'!");

                else
                    DebugX.Log($"Received unknown text message within {nameof(AOverlayWebSocketClient)}: '{TextMessage}'!");

            }
            catch (Exception e)
            {

                DebugX.LogException(e, nameof(AOverlayWebSocketClient) + "." + nameof(ProcessWebSocketTextFrame));

                //EEBusErrorResponse = new EEBus_WebSocket_ErrorMessage(
                //                        Request_Id.Zero,
                //                        ResultCodes.InternalError,
                //                        $"The EEBus message '{EEBusTextMessage}' received in " + nameof(AChargingStationWSClient) + " led to an exception!",
                //                        new JObject(
                //                            new JProperty("request",      EEBusTextMessage),
                //                            new JProperty("exception",    e.Message),
                //                            new JProperty("stacktrace",   e.StackTrace)
                //                        )
                //                    );

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
                                                      WebSocketClient            Client,
                                                      WebSocketClientConnection  Connection,
                                                      WebSocketFrame             Frame,
                                                      EventTracking_Id           EventTrackingId,
                                                      Byte[]                     BinaryMessage,
                                                      CancellationToken          CancellationToken)
        {

            if (BinaryMessage.Length == 0)
            {
                DebugX.Log($"Received an empty binary message within {nameof(AOverlayWebSocketClient)}!");
                return;
            }

            try
            {

              //  var sourceNodeId = Connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey) ?? NetworkingNode_Id.Zero;

                     if (BinaryRequestMessage. TryParse(BinaryMessage, out var binaryRequest,  out var requestParsingError, RequestTimestamp, EventTrackingId, null, CancellationToken)  && binaryRequest  is not null)
                {

                    JSONResponseMessage?    EEBusJSONResponse     = null;
                    BinaryResponseMessage?  EEBusBinaryResponse   = null;
                    JSONRequestErrorMessage?       EEBusErrorResponse    = null;

                    // Try to call the matching 'incoming message processor'
                    if (incomingMessageProcessorsLookup.TryGetValue(binaryRequest.Action, out var methodInfo) && methodInfo is not null)
                    {

                        #region Call 'incoming message' processor

                        var result = methodInfo.Invoke(this,
                                                       [ binaryRequest.RequestTimestamp,
                                                         Connection,
                                                         binaryRequest.DestinationId,
                                                         binaryRequest.NetworkPath,
                                                         binaryRequest.EventTrackingId,
                                                         binaryRequest.RequestId,
                                                         binaryRequest.Payload,
                                                         binaryRequest.CancellationToken ]);

                        #endregion

                             if (result is Task<Tuple<JSONResponseMessage?,   JSONRequestErrorMessage?>> jsonProcessor)
                        {

                            (EEBusJSONResponse,   EEBusErrorResponse) = await jsonProcessor;

                            #region Send response...

                            if (EEBusJSONResponse is not null)
                                await SendTextMessage(
                                          EEBusJSONResponse.ToJSON().ToString(JSONFormatting),
                                          EventTrackingId,
                                          CancellationToken
                                      );

                            #endregion

                            #region ..., or send error response!

                            if (EEBusErrorResponse is not null)
                            {
                                // CALL RESULT ERROR: New in EEBus v2.1++
                            }

                            #endregion


                            #region OnJSONMessageResponseSent

                            try
                            {

                                OnJSONMessageResponseSent?.Invoke(Timestamp.Now,
                                                                  this,
                                                                  EventTrackingId,
                                                                  RequestTimestamp,
                                                                  null,
                                                                  BinaryMessage,
                                                                  Timestamp.Now,
                                                                  EEBusJSONResponse?.ToJSON() ?? EEBusErrorResponse?.ToJSON() ?? []);

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketClient) + "." + nameof(OnJSONMessageResponseSent));
                            }

                            #endregion

                        }

                        else if (result is Task<Tuple<BinaryResponseMessage?, JSONRequestErrorMessage?>> binaryProcessor)
                        {

                            (EEBusBinaryResponse, EEBusErrorResponse) = await binaryProcessor;

                            #region Send response...

                            if (EEBusBinaryResponse is not null)
                                await SendBinaryMessage(
                                          EEBusBinaryResponse.ToByteArray(),
                                          EventTrackingId,
                                          CancellationToken
                                      );

                            #endregion

                            #region ..., or send error response!

                            if (EEBusErrorResponse is not null)
                            {
                                // CALL RESULT ERROR: New in EEBus v2.1++
                            }

                            #endregion


                            #region OnBinaryMessageResponseSent

                            try
                            {

                                OnBinaryMessageResponseSent?.Invoke(Timestamp.Now,
                                                                    this,
                                                                    EventTrackingId,
                                                                    RequestTimestamp,
                                                                    null,
                                                                    BinaryMessage,
                                                                    Timestamp.Now,
                                                                    EEBusBinaryResponse?.ToByteArray() ?? EEBusErrorResponse?.ToByteArray() ?? []);

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketClient) + "." + nameof(OnBinaryMessageResponseSent));
                            }

                            #endregion

                        }
                        else
                            DebugX.Log($"Undefined '{binaryRequest.Action}' binary request message handler within {nameof(AOverlayWebSocketClient)}!");

                    }
                    else
                        DebugX.Log($"Unknown '{binaryRequest.Action}' binary request message handler within {nameof(AOverlayWebSocketClient)}!");

                }

                else if (BinaryResponseMessage.TryParse(BinaryMessage, out var binaryResponse, out var responseParsingError) && binaryResponse is not null)
                {

                    if (requests.TryGetValue(binaryResponse.RequestId, out var sendRequestState) &&
                        sendRequestState is not null)
                    {

                        sendRequestState.ResponseTimestamp  = Timestamp.Now;
                        sendRequestState.BinaryResponse     = binaryResponse;

                        #region OnBinaryMessageResponseReceived

                        try
                        {

                            OnBinaryMessageResponseReceived?.Invoke(Timestamp.Now,
                                                                    this,
                                                                    EventTrackingId,
                                                                    sendRequestState.RequestTimestamp,
                                                                    sendRequestState.JSONRequest?.  ToJSON()      ?? [],
                                                                    sendRequestState.BinaryRequest?.ToByteArray() ?? [],
                                                                    sendRequestState.ResponseTimestamp.Value,
                                                                    sendRequestState.BinaryResponse.ToByteArray());

                        }
                        catch (Exception e)
                        {
                            DebugX.Log(e, nameof(AOverlayWebSocketClient) + "." + nameof(OnBinaryMessageResponseReceived));
                        }

                        #endregion

                    }
                    else
                        DebugX.Log($"Received a binary EEBus response message having an unknown request identification within {nameof(AOverlayWebSocketClient)}: '{binaryResponse}'!");

                }

                else if (requestParsingError  is not null)
                    DebugX.Log($"Failed to parse a binary request message within {nameof(AOverlayWebSocketClient)}: '{requestParsingError}'{Environment.NewLine}'{BinaryMessage.ToBase64()}'!");

                else if (responseParsingError is not null)
                    DebugX.Log($"Failed to parse a binary response message within {nameof(AOverlayWebSocketClient)}: '{responseParsingError}'{Environment.NewLine}'{BinaryMessage.ToBase64()}'!");

                else
                    DebugX.Log($"Received unknown binary message within {nameof(AOverlayWebSocketClient)}: '{BinaryMessage.ToBase64()}'!");

            }
            catch (Exception e)
            {

                DebugX.LogException(e, nameof(AOverlayWebSocketClient) + "." + nameof(ProcessWebSocketBinaryFrame));

                //EEBusErrorResponse = new EEBus_WebSocket_ErrorMessage(
                //                        Request_Id.Zero,
                //                        ResultCodes.InternalError,
                //                        $"The EEBus message '{EEBusTextMessage}' received in " + nameof(AChargingStationWSClient) + " led to an exception!",
                //                        new JObject(
                //                            new JProperty("request",      EEBusTextMessage),
                //                            new JProperty("exception",    e.Message),
                //                            new JProperty("stacktrace",   e.StackTrace)
                //                        )
                //                    );

            }

        }

        #endregion


        #region SendRequest(DestinationNodeId, [NetworkPath], Action, RequestId, JSONMessage,   EventTrackingId = null)

        public Task<JSONRequestMessage> SendRequest(NetworkingNode_Id  DestinationNodeId,
                                                    String             Action,
                                                    Request_Id         RequestId,
                                                    JObject            JSONMessage,
                                                    EventTracking_Id?  EventTrackingId   = null)

            => SendRequest(DestinationNodeId,
                           NetworkPath.Empty,
                           Action,
                           RequestId,
                           JSONMessage,
                           EventTrackingId);

        public async Task<JSONRequestMessage> SendRequest(NetworkingNode_Id  DestinationNodeId,
                                                          NetworkPath        NetworkPath,
                                                          String             Action,
                                                          Request_Id         RequestId,
                                                          JObject            JSONMessage,
                                                          EventTracking_Id?  EventTrackingId   = null)
        {

            JSONRequestMessage? jsonRequestMessage = null;

            if (await MaintenanceSemaphore.WaitAsync(SemaphoreSlimTimeout).
                                           ConfigureAwait(false))
            {
                try
                {

                    if (HTTPStream is not null)
                    {

                        jsonRequestMessage = new JSONRequestMessage(
                                                 Timestamp.Now,
                                                 EventTrackingId ?? EventTracking_Id.New,
                                                 NetworkingMode,
                                                 DestinationNodeId,
                                                 NetworkPath.Append(Id),
                                                 RequestId,
                                                 Action,
                                                 JSONMessage
                                             );

                        var sendStatus = await SendTextMessage(jsonRequestMessage.
                                                                   ToJSON  ().
                                                                   ToString(JSONFormatting));

                        if (sendStatus == SentStatus.Success)
                            requests.TryAdd(RequestId,
                                            SendRequestState.FromJSONRequest(
                                                Timestamp.Now,
                                                Id,
                                                Timestamp.Now + RequestTimeout,
                                                jsonRequestMessage
                                            ));

                        else
                        {
                            //ToDo: Retry to send text message!
                        }

                    }
                    else
                    {

                        jsonRequestMessage = new JSONRequestMessage(
                                                 Timestamp.Now,
                                                 EventTracking_Id.New,
                                                 NetworkingMode,
                                                 DestinationNodeId,
                                                 NetworkPath ?? NetworkPath.Empty,
                                                 RequestId,
                                                 Action,
                                                 JSONMessage,
                                                 ErrorMessage: "Invalid HTTP WebSocket connection!"
                                             );

                    }

                }
                catch (Exception e)
                {

                    while (e.InnerException is not null)
                        e = e.InnerException;

                    jsonRequestMessage = new JSONRequestMessage(
                                             Timestamp.Now,
                                             EventTracking_Id.New,
                                             NetworkingMode,
                                             DestinationNodeId,
                                             NetworkPath ?? NetworkPath.Empty,
                                             RequestId,
                                             Action,
                                             JSONMessage,
                                             ErrorMessage: e.Message
                                         );

                    DebugX.LogException(e);

                }
                finally
                {
                    MaintenanceSemaphore.Release();
                }
            }

            else
                jsonRequestMessage = new JSONRequestMessage(
                                         Timestamp.Now,
                                         EventTracking_Id.New,
                                         NetworkingMode,
                                         DestinationNodeId,
                                         NetworkPath ?? NetworkPath.Empty,
                                         RequestId,
                                         Action,
                                         JSONMessage,
                                         ErrorMessage: "Could not aquire the maintenance tasks lock!"
                                     );

            return jsonRequestMessage;

        }

        #endregion

        #region SendRequest(DestinationNodeId, [NetworkPath], Action, RequestId, BinaryMessage, EventTrackingId = null)

        public Task<BinaryRequestMessage> SendRequest(NetworkingNode_Id  DestinationNodeId,
                                                      String             Action,
                                                      Request_Id         RequestId,
                                                      Byte[]             BinaryMessage,
                                                      EventTracking_Id?  EventTrackingId   = null)

            => SendRequest(DestinationNodeId,
                           NetworkPath.Empty,
                           Action,
                           RequestId,
                           BinaryMessage,
                           EventTrackingId);

        public async Task<BinaryRequestMessage> SendRequest(NetworkingNode_Id  DestinationNodeId,
                                                            NetworkPath        NetworkPath,
                                                            String             Action,
                                                            Request_Id         RequestId,
                                                            Byte[]             BinaryMessage,
                                                            EventTracking_Id?  EventTrackingId   = null)
        {

            BinaryRequestMessage? binaryRequestMessage = null;

            if (await MaintenanceSemaphore.WaitAsync(SemaphoreSlimTimeout).
                                           ConfigureAwait(false))
            {
                try
                {

                    if (HTTPStream is not null)
                    {

                        binaryRequestMessage = new BinaryRequestMessage(
                                                   Timestamp.Now,
                                                   EventTrackingId ?? EventTracking_Id.New,
                                                   NetworkingMode,
                                                   DestinationNodeId,
                                                   NetworkPath.Append(Id),
                                                   RequestId,
                                                   Action,
                                                   BinaryMessage
                                               );

                        var sendStatus = await SendBinaryMessage(binaryRequestMessage.ToByteArray());

                        if (sendStatus == SentStatus.Success)
                            requests.TryAdd(RequestId,
                                            SendRequestState.FromBinaryRequest(
                                                Timestamp.Now,
                                                Id,
                                                Timestamp.Now + RequestTimeout,
                                                binaryRequestMessage
                                            ));

                        else
                        {
                            //ToDo: Retry to send binary message!
                        }

                    }
                    else
                    {

                        binaryRequestMessage = new BinaryRequestMessage(
                                                   Timestamp.Now,
                                                   EventTracking_Id.New,
                                                   NetworkingMode,
                                                   DestinationNodeId,
                                                   NetworkPath ?? NetworkPath.Empty,
                                                   RequestId,
                                                   Action,
                                                   BinaryMessage,
                                                   ErrorMessage: "Invalid HTTP WebSocket connection!"
                                               );

                    }

                }
                catch (Exception e)
                {

                    while (e.InnerException is not null)
                        e = e.InnerException;

                    binaryRequestMessage = new BinaryRequestMessage(
                                               Timestamp.Now,
                                               EventTracking_Id.New,
                                               NetworkingMode,
                                               DestinationNodeId,
                                               NetworkPath ?? NetworkPath.Empty,
                                               RequestId,
                                               Action,
                                               BinaryMessage,
                                               ErrorMessage: e.Message
                                           );

                    DebugX.LogException(e);

                }
                finally
                {
                    MaintenanceSemaphore.Release();
                }
            }

            else
                binaryRequestMessage = new BinaryRequestMessage(
                                           Timestamp.Now,
                                           EventTracking_Id.New,
                                           NetworkingMode,
                                           DestinationNodeId,
                                           NetworkPath ?? NetworkPath.Empty,
                                           RequestId,
                                           Action,
                                           BinaryMessage,
                                           ErrorMessage: "Could not aquire the maintenance tasks lock!"
                                       );

            return binaryRequestMessage;

        }

        #endregion


        #region (protected) WaitForResponse(JSONRequestMessage)

        protected async Task<SendRequestState> WaitForResponse(JSONRequestMessage JSONRequestMessage)
        {

            var endTime = Timestamp.Now + RequestTimeout;

            #region Wait for a response... till timeout

            do
            {

                try
                {

                    await Task.Delay(25);

                    if (requests.TryGetValue(JSONRequestMessage.RequestId, out var aSendRequestState) &&
                        aSendRequestState is SendRequestState sendRequestState &&
                       (sendRequestState?.JSONResponse is not null ||
                        sendRequestState?.HasErrors == true))
                    {

                        requests.TryRemove(JSONRequestMessage.RequestId, out _);

                        return sendRequestState;

                    }

                }
                catch (Exception e)
                {
                    DebugX.Log(String.Concat(nameof(AOverlayWebSocketClient), ".", nameof(WaitForResponse), " exception occured: ", e.Message));
                }

            }
            while (Timestamp.Now < endTime);

            #endregion

            return SendRequestState.FromJSONRequest(

                       JSONRequestMessage.RequestTimestamp,
                       Id,
                       endTime,
                       JSONRequestMessage,
                       Timestamp.Now,

                       JSONRequestErrorMessage:  new JSONRequestErrorMessage(

                                                     Timestamp.Now,
                                                     JSONRequestMessage.EventTrackingId,
                                                     NetworkingMode.Unknown,
                                                     JSONRequestMessage.NetworkPath.Source,
                                                     NetworkPath.From(Id),
                                                     JSONRequestMessage.RequestId,

                                                     ErrorCode: ResultCode.Timeout

                                                 )

                   );

        }

        #endregion

        #region (protected) WaitForResponse(BinaryRequestMessage)

        protected async Task<SendRequestState> WaitForResponse(BinaryRequestMessage BinaryRequestMessage)
        {

            var endTime = Timestamp.Now + RequestTimeout;

            #region Wait for a response... till timeout

            do
            {

                try
                {

                    await Task.Delay(25);

                    if (requests.TryGetValue(BinaryRequestMessage.RequestId, out var aSendRequestState) &&
                        aSendRequestState is SendRequestState sendRequestState &&
                       (sendRequestState?.BinaryResponse is not null ||
                        sendRequestState?.HasErrors == true))
                    {

                        requests.TryRemove(BinaryRequestMessage.RequestId, out _);

                        return sendRequestState;

                    }

                }
                catch (Exception e)
                {
                    DebugX.Log(String.Concat(nameof(AOverlayWebSocketClient), ".", nameof(WaitForResponse), " exception occured: ", e.Message));
                }

            }
            while (Timestamp.Now < endTime);

            #endregion

            return SendRequestState.FromBinaryRequest(

                       Timestamp.Now,
                       Id,
                       endTime,
                       BinaryRequestMessage,

                       JSONRequestErrorMessage:  new JSONRequestErrorMessage(

                                                     Timestamp.Now,
                                                     BinaryRequestMessage.EventTrackingId,
                                                     NetworkingMode.Unknown,
                                                     BinaryRequestMessage.NetworkPath.Source,
                                                     NetworkPath.From(Id),
                                                     BinaryRequestMessage.RequestId,

                                                     ErrorCode: ResultCode.Timeout

                                                 )

                   );

        }

        #endregion


    }

}
