/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP EEBus <https://github.com/OpenChargingCloud/WWCP_EEBus>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using cloud.charging.open.protocols.WWCP.OverlayNetworking.WebSockets;
using org.GraphDefined.Vanaheimr.Hermod.Logging;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking
{

    public abstract class AOverlayNetworkingNode : IOverlayNetworkingNode
    {

        #region Data

        protected        readonly  HashSet<OverlayWebSocketServer>  webSocketServers            = [];
        protected        readonly  List<OverlayWebSocketClient>     webSocketClients            = [];

        //private          readonly  ConcurrentDictionary<NetworkingNode_Id, Tuple<CSMS.ICSMSChannel, DateTime>>   connectedNetworkingNodes     = [];
        protected        readonly  ConcurrentDictionary<NetworkingNode_Id, NetworkingNode_Id>                    reachableViaNetworkingHubs   = [];

        /// <summary>
        /// The default time span between maintenance tasks.
        /// </summary>
        protected static readonly  TimeSpan                      DefaultMaintenanceEvery      = TimeSpan.FromMinutes(1);

        #endregion

        #region Properties

        /// <summary>
        /// The sender identification.
        /// </summary>
        String IEventSender.Id
            => Id.ToString();

        /// <summary>
        /// The unique identification of this networking node.
        /// </summary>
        public NetworkingNode_Id           Id                         { get; }

        /// <summary>
        /// An optional multi-language networking node description.
        /// </summary>
        [Optional]
        public I18NString?                 Description                { get; }



        public CustomData                  CustomData                 { get; }


        /// <summary>
        /// Disable all maintenance tasks.
        /// </summary>
        public Boolean                     DisableMaintenanceTasks    { get; set; }

        /// <summary>
        /// The maintenance interval.
        /// </summary>
        public TimeSpan                    MaintenanceEvery           { get; }      = DefaultMaintenanceEvery;


        public DNSClient                   DNSClient                  { get; }
    //    public IEEBusAdapter                EEBus                       { get; }






        public String? ClientCloseMessage
            => "Bye!";


        public IEnumerable<OverlayWebSocketClient> WebSocketClients
            => webSocketClients;

        public IEnumerable<OverlayWebSocketServer> WebSocketServers
            => webSocketServers;

        #endregion

        #region Events

        #region WebSocket connections

        /// <summary>
        /// An event sent whenever the HTTP web socket server started.
        /// </summary>
        public event OnServerStartedDelegate?                 OnServerStarted;

        /// <summary>
        /// An event sent whenever a new TCP connection was accepted.
        /// </summary>
        public event OnValidateTCPConnectionDelegate?         OnValidateTCPConnection;

        /// <summary>
        /// An event sent whenever a new TCP connection was accepted.
        /// </summary>
        public event OnNewTCPConnectionDelegate?              OnNewTCPConnection;

        /// <summary>
        /// An event sent whenever a HTTP request was received.
        /// </summary>
        public event HTTPRequestLogDelegate?                  OnHTTPRequest;

        /// <summary>
        /// An event sent whenever the HTTP headers of a new web socket connection
        /// need to be validated or filtered by an upper layer application logic.
        /// </summary>
        public event OnValidateWebSocketConnectionDelegate?   OnValidateWebSocketConnection;

        /// <summary>
        /// An event sent whenever the HTTP connection switched successfully to web socket.
        /// </summary>
        public event OnNewWebSocketConnectionDelegate?        OnNewWebSocketConnection;

        /// <summary>
        /// An event sent whenever a reponse to a HTTP request was sent.
        /// </summary>
        public event HTTPResponseLogDelegate?                 OnHTTPResponse;

        /// <summary>
        /// An event sent whenever a web socket close frame was received.
        /// </summary>
        public event OnWebSocketServerCloseMessageReceivedDelegate?          OnCloseMessageReceived;

        /// <summary>
        /// An event sent whenever a TCP connection was closed.
        /// </summary>
        public event OnTCPConnectionClosedDelegate?           OnTCPConnectionClosed;

        /// <summary>
        /// An event sent whenever the HTTP web socket server stopped.
        /// </summary>
        public event OnServerStoppedDelegate?                 OnServerStopped;

        #endregion

        #region Generic JSON Messages

        /// <summary>
        /// An event sent whenever a JSON message request was received.
        /// </summary>
        public event OnWebSocketJSONMessageRequestDelegate?     OnJSONMessageRequestReceived;

        /// <summary>
        /// An event sent whenever the response to a JSON message was sent.
        /// </summary>
        public event OnWebSocketJSONMessageResponseDelegate?    OnJSONMessageResponseSent;

        /// <summary>
        /// An event sent whenever the error response to a JSON message was sent.
        /// </summary>
        public event OnWebSocketTextErrorResponseDelegate?      OnJSONErrorResponseSent;


        /// <summary>
        /// An event sent whenever a JSON message request was sent.
        /// </summary>
        public event OnWebSocketJSONMessageRequestDelegate?     OnJSONMessageRequestSent;

        /// <summary>
        /// An event sent whenever the response to a JSON message request was received.
        /// </summary>
        public event OnWebSocketJSONMessageResponseDelegate?    OnJSONMessageResponseReceived;

        /// <summary>
        /// An event sent whenever an error response to a JSON message request was received.
        /// </summary>
        public event OnWebSocketTextErrorResponseDelegate?      OnJSONErrorResponseReceived;

        #endregion

        #region Generic Binary Messages

        /// <summary>
        /// An event sent whenever a binary message request was received.
        /// </summary>
        public event OnWebSocketBinaryMessageRequestDelegate?     OnBinaryMessageRequestReceived;

        /// <summary>
        /// An event sent whenever the response to a binary message was sent.
        /// </summary>
        public event OnWebSocketBinaryMessageResponseDelegate?    OnBinaryMessageResponseSent;

        /// <summary>
        /// An event sent whenever the error response to a binary message was sent.
        /// </summary>
        //public event OnWebSocketBinaryErrorResponseDelegate?      OnBinaryErrorResponseSent;


        /// <summary>
        /// An event sent whenever a binary message request was sent.
        /// </summary>
        public event OnWebSocketBinaryMessageRequestDelegate?     OnBinaryMessageRequestSent;

        /// <summary>
        /// An event sent whenever the response to a binary message request was received.
        /// </summary>
        public event OnWebSocketBinaryMessageResponseDelegate?    OnBinaryMessageResponseReceived;

        /// <summary>
        /// An event sent whenever the error response to a binary message request was sent.
        /// </summary>
        //public event OnWebSocketBinaryErrorResponseDelegate?      OnBinaryErrorResponseReceived;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new networking node for testing.
        /// </summary>
        /// <param name="Id">The unique identification of this networking node.</param>
        public AOverlayNetworkingNode(NetworkingNode_Id  Id,
                                      I18NString?        Description                 = null,

                                      SignaturePolicy?   SignaturePolicy             = null,
                                      SignaturePolicy?   ForwardingSignaturePolicy   = null,

                                      Boolean            DisableSendHeartbeats       = false,
                                      TimeSpan?          SendHeartbeatsEvery         = null,
                                      TimeSpan?          DefaultRequestTimeout       = null,

                                      Boolean            DisableMaintenanceTasks     = false,
                                      TimeSpan?          MaintenanceEvery            = null,
                                      DNSClient?         DNSClient                   = null)

        {

            if (Id.IsNullOrEmpty)
                throw new ArgumentNullException(nameof(Id),          "The given networking node identification must not be null or empty!");

            this.Id                       = Id;
            this.Description              = Description;
            this.CustomData               = CustomData       ?? new CustomData(Vendor_Id.GraphDefined);

            this.DisableMaintenanceTasks  = DisableMaintenanceTasks;
            this.MaintenanceEvery         = MaintenanceEvery ?? DefaultMaintenanceEvery;
            this.DNSClient                = DNSClient        ?? new DNSClient(SearchForIPv6DNSServers: false);

          //  this.EEBus                     = new EEBusAdapter(
          //                                       this,
          //                                       DisableSendHeartbeats,
          //                                       SendHeartbeatsEvery,
          //                                       DefaultRequestTimeout,
          //                                       SignaturePolicy,
          //                                       ForwardingSignaturePolicy
          //                                   );

        }

        #endregion



        public Byte[] GetEncryptionKey(NetworkingNode_Id  DestinationNodeId,
                                       UInt16?            KeyId   = null)
        {
            return "5a733d6660df00c447ff184ae971e1d5bba5de5784768795ee6535867130aa12".HexStringToByteArray();
        }

        public Byte[] GetDecryptionKey(NetworkingNode_Id  SourceNodeId,
                                       UInt16?            KeyId   = null)
        {
            return "5a733d6660df00c447ff184ae971e1d5bba5de5784768795ee6535867130aa12".HexStringToByteArray();
        }


        public UInt64 GetEncryptionNonce(NetworkingNode_Id  DestinationNodeId,
                                         UInt16?            KeyId   = null)
        {
            return 1;
        }

        public UInt64 GetEncryptionCounter(NetworkingNode_Id  DestinationNodeId,
                                           UInt16?            KeyId   = null)
        {
            return 1;
        }




        #region ConnectWebSocketClient(...)

        public async Task<HTTPResponse> ConnectWebSocketClient(NetworkingNode_Id                                               NetworkingNodeId,
                                                               URL                                                             RemoteURL,
                                                               HTTPHostname?                                                   VirtualHostname              = null,
                                                               I18NString?                                                     Description                  = null,
                                                               Boolean?                                                        PreferIPv4                   = null,
                                                               RemoteTLSServerCertificateValidationHandler<org.GraphDefined.Vanaheimr.Hermod.WebSocket.IWebSocketClient>?  RemoteCertificateValidator   = null,
                                                               LocalCertificateSelectionHandler?                               LocalCertificateSelector     = null,
                                                               X509Certificate?                                                ClientCert                   = null,
                                                               SslProtocols?                                                   TLSProtocol                  = null,
                                                               String?                                                         HTTPUserAgent                = null,
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
        {

            var ocppWebSocketClient = new OverlayWebSocketClient(

                                          Id,

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
                                          DNSClient

                                      );

            this.webSocketClients.Add(ocppWebSocketClient);

            var connectResponse = await ocppWebSocketClient.Connect();

            //connectResponse.Item1.TryAddCustomData(OCPPAdapter.NetworkingNodeId_WebSocketKey,
            //                                       NetworkingNodeId);

            //OCPP.AddStaticRouting(NetworkingNodeId,
            //                      ocppWebSocketClient,
            //                      0,
            //                      Timestamp.Now);

            return connectResponse.Item2;

        }

        #endregion


        #region AttachWebSocketServer(...)

        /// <summary>
        /// Create a new central system for testing using HTTP/WebSocket.
        /// </summary>
        /// <param name="HTTPServiceName">An optional identification string for the HTTP server.</param>
        /// <param name="IPAddress">An IP address to listen on.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="Description">An optional description of this HTTP WebSocket service.</param>
        /// 
        /// <param name="AutoStart">Start the server immediately.</param>
        public OverlayWebSocketServer AttachWebSocketServer(IEnumerable<String>                                             SupportedEEBusWebSocketSubprotocols,

                                                            String?                                                         HTTPServiceName              = null,
                                                            IIPAddress?                                                     IPAddress                    = null,
                                                            IPPort?                                                         TCPPort                      = null,
                                                            I18NString?                                                     Description                  = null,

                                                            Boolean                                                         RequireAuthentication        = true,
                                                            Boolean                                                         DisableWebSocketPings        = false,
                                                            TimeSpan?                                                       WebSocketPingEvery           = null,
                                                            TimeSpan?                                                       SlowNetworkSimulationDelay   = null,

                                                            Func<X509Certificate2>?                                         ServerCertificateSelector    = null,
                                                            RemoteTLSClientCertificateValidationHandler<org.GraphDefined.Vanaheimr.Hermod.WebSocket.IWebSocketServer>? ClientCertificateValidator   = null,
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

                                                            Boolean                                                         AutoStart                    = false)
        {

            var ocppWebSocketServer = new OverlayWebSocketServer(

                                          Id,
                                          SupportedEEBusWebSocketSubprotocols,

                                          HTTPServiceName,
                                          IPAddress,
                                          TCPPort,
                                          Description,

                                          RequireAuthentication,
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
                                          AutoStart: false

                                      );

            WireWebSocketServer(ocppWebSocketServer);

            if (AutoStart)
                ocppWebSocketServer.Start();

            return ocppWebSocketServer;

        }

        #endregion

        #region (private) WireWebSocketServer(WebSocketServer)

        private void WireWebSocketServer(OverlayWebSocketServer WebSocketServer)
        {

            webSocketServers.Add(WebSocketServer);

            #region WebSocket related

            #region OnServerStarted

            WebSocketServer.OnServerStarted += async (timestamp,
                                                      server,
                                                      eventTrackingId,
                                                      cancellationToken) => {

                var onServerStarted = OnServerStarted;
                if (onServerStarted is not null)
                {
                    try
                    {

                        await Task.WhenAll(onServerStarted.GetInvocationList().
                                               OfType <OnServerStartedDelegate>().
                                               Select (loggingDelegate => loggingDelegate.Invoke(
                                                                              timestamp,
                                                                              server,
                                                                              eventTrackingId,
                                                                              cancellationToken
                                                                          )).
                                               ToArray());

                    }
                    catch (Exception e)
                    {
                        await HandleErrors(
                                  nameof(AOverlayNetworkingNode),
                                  nameof(OnServerStarted),
                                  e
                              );
                    }
                }

            };

            #endregion

            #region OnNewTCPConnection

            WebSocketServer.OnNewTCPConnection += async (timestamp,
                                                         webSocketServer,
                                                         newTCPConnection,
                                                         eventTrackingId,
                                                         cancellationToken) => {

                var onNewTCPConnection = OnNewTCPConnection;
                if (onNewTCPConnection is not null)
                {
                    try
                    {

                        await Task.WhenAll(onNewTCPConnection.GetInvocationList().
                                               OfType <OnNewTCPConnectionDelegate>().
                                               Select (loggingDelegate => loggingDelegate.Invoke(
                                                                              timestamp,
                                                                              webSocketServer,
                                                                              newTCPConnection,
                                                                              eventTrackingId,
                                                                              cancellationToken
                                                                          )).
                                               ToArray());

                    }
                    catch (Exception e)
                    {
                        await HandleErrors(
                                  nameof(AOverlayNetworkingNode),
                                  nameof(OnNewTCPConnection),
                                  e
                              );
                    }
                }

            };

            #endregion

            // Failed (Charging Station) Authentication

            #region OnNetworkingNodeNewWebSocketConnection

            //WebSocketServer.OnNetworkingNodeNewWebSocketConnection += async (timestamp,
            //                                                                 ocppWebSocketServer,
            //                                                                 newConnection,
            //                                                                 networkingNodeId,
            //                                                                 eventTrackingId,
            //                                                                 sharedSubprotocols,
            //                                                                 cancellationToken) => {

            //    // A new connection from the same networking node/charging station will replace the older one!
            //    OCPP.AddStaticRouting(DestinationNodeId:  networkingNodeId,
            //                          WebSocketServer:    ocppWebSocketServer,
            //                          Priority:           0,
            //                          Timestamp:          timestamp);

            //    #region Send OnNewWebSocketConnection

            //    var logger = OnNewWebSocketConnection;
            //    if (logger is not null)
            //    {
            //        try
            //        {

            //            await Task.WhenAll(logger.GetInvocationList().
            //                                   OfType <NetworkingNode.OnNetworkingNodeNewWebSocketConnectionDelegate>().
            //                                   Select (loggingDelegate => loggingDelegate.Invoke(
            //                                                                  timestamp,
            //                                                                  ocppWebSocketServer,
            //                                                                  newConnection,
            //                                                                  networkingNodeId,
            //                                                                  eventTrackingId,
            //                                                                  sharedSubprotocols,
            //                                                                  cancellationToken
            //                                                              )).
            //                                   ToArray());

            //        }
            //        catch (Exception e)
            //        {
            //            await HandleErrors(
            //                      nameof(AOverlayNetworkingNode),
            //                      nameof(OnNewWebSocketConnection),
            //                      e
            //                  );
            //        }
            //    }

            //    #endregion

            //};

            #endregion

            #region OnNetworkingNodeCloseMessageReceived

            //WebSocketServer.OnNetworkingNodeCloseMessageReceived += async (timestamp,
            //                                                 server,
            //                                                 connection,
            //                                                 networkingNodeId,
            //                                                 eventTrackingId,
            //                                                 statusCode,
            //                                                 reason,
            //                                                 cancellationToken) => {

            //    var logger = OnCloseMessageReceived;
            //    if (logger is not null)
            //    {
            //        try
            //        {

            //            await Task.WhenAll(logger.GetInvocationList().
            //                                   OfType <NetworkingNode.OnNetworkingNodeCloseMessageReceivedDelegate>().
            //                                   Select (loggingDelegate => loggingDelegate.Invoke(
            //                                                                  timestamp,
            //                                                                  server,
            //                                                                  connection,
            //                                                                  networkingNodeId,
            //                                                                  eventTrackingId,
            //                                                                  statusCode,
            //                                                                  reason,
            //                                                                  cancellationToken
            //                                                              )).
            //                                   ToArray());

            //        }
            //        catch (Exception e)
            //        {
            //            await HandleErrors(
            //                      nameof(AOverlayNetworkingNode),
            //                      nameof(OnCloseMessageReceived),
            //                      e
            //                  );
            //        }
            //    }

            //};

            #endregion

            #region OnNetworkingNodeTCPConnectionClosed

            //WebSocketServer.OnNetworkingNodeTCPConnectionClosed += async (timestamp,
            //                                                server,
            //                                                connection,
            //                                                networkingNodeId,
            //                                                eventTrackingId,
            //                                                reason,
            //                                                cancellationToken) => {

            //    var logger = OnTCPConnectionClosed;
            //    if (logger is not null)
            //    {
            //        try
            //        {

            //            await Task.WhenAll(logger.GetInvocationList().
            //                                   OfType <NetworkingNode.OnNetworkingNodeTCPConnectionClosedDelegate>().
            //                                   Select (loggingDelegate => loggingDelegate.Invoke(
            //                                                                  timestamp,
            //                                                                  server,
            //                                                                  connection,
            //                                                                  networkingNodeId,
            //                                                                  eventTrackingId,
            //                                                                  reason,
            //                                                                  cancellationToken
            //                                                              )).
            //                                   ToArray());

            //        }
            //        catch (Exception e)
            //        {
            //            await HandleErrors(
            //                      nameof(AOverlayNetworkingNode),
            //                      nameof(OnTCPConnectionClosed),
            //                      e
            //                  );
            //        }
            //    }

            //};

            #endregion

            #region OnServerStopped

            WebSocketServer.OnServerStopped += async (timestamp,
                                                  server,
                                                  eventTrackingId,
                                                  reason,
                                                  cancellationToken) => {

                var logger = OnServerStopped;
                if (logger is not null)
                {
                    try
                    {

                        await Task.WhenAll(logger.GetInvocationList().
                                                 OfType <OnServerStoppedDelegate>().
                                                 Select (loggingDelegate => loggingDelegate.Invoke(
                                                                                timestamp,
                                                                                server,
                                                                                eventTrackingId,
                                                                                reason,
                                                                                cancellationToken
                                                                            )).
                                                 ToArray());

                    }
                    catch (Exception e)
                    {
                        await HandleErrors(
                                  nameof(AOverlayNetworkingNode),
                                  nameof(OnServerStopped),
                                  e
                              );
                    }
                }

            };

            #endregion

            // (Generic) Error Handling

            #endregion

        }

        #endregion










        #region (virtual) HandleErrors(Module, Caller, ErrorResponse)

        public virtual Task HandleErrors(String  Module,
                                         String  Caller,
                                         String  ErrorResponse)
        {

            return Task.CompletedTask;

        }

        #endregion

        #region (virtual) HandleErrors(Module, Caller, ExceptionOccured)

        public virtual Task HandleErrors(String     Module,
                                         String     Caller,
                                         Exception  ExceptionOccured)
        {

            return Task.CompletedTask;

        }

        #endregion


        #region Shutdown(Message = null, Wait = true)

        /// <summary>
        /// Shutdown all HTTP WebSocket listeners.
        /// </summary>
        /// <param name="Message">An optional shutdown message.</param>
        /// <param name="Wait">Wait until the server finally shutted down.</param>
        public async Task Shutdown(String?  Message   = null,
                                   Boolean  Wait      = true)
        {

            await Task.WhenAll(webSocketServers.
                                   Select (ocppWebSocketServer => ocppWebSocketServer.Shutdown(Message, Wait)).
                                   ToArray());

        }

        #endregion


    }

}
