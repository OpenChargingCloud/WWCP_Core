/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.NetworkingNode
{

    #region Common Connection Management

    /// <summary>
    /// A delegate for logging new HTTP WebSocket connections.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="WWCPWebSocketServer">The HTTP WebSocket server.</param>
    /// <param name="NewConnection">The new HTTP WebSocket connection.</param>
    /// <param name="NetworkingNodeId">The sending OCPP networking node/charging station identification.</param>
    /// <param name="NetworkingMode">The networking mode of the new connection.</param>
    /// <param name="SharedSubprotocols">An enumeration of shared HTTP WebSockets subprotocols.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="CancellationToken">A token to cancel the processing.</param>
    public delegate Task OnNetworkingNodeNewWebSocketConnectionDelegate (DateTime                           Timestamp,
                                                                         IWWCPWebSocketServer               WWCPWebSocketServer,
                                                                         WebSocketServerConnection          NewConnection,
                                                                         NetworkingNode_Id                  NetworkingNodeId,
                                                                         NetworkingMode?                    NetworkingMode,
                                                                         IEnumerable<String>                SharedSubprotocols,
                                                                         EventTracking_Id                   EventTrackingId,
                                                                         CancellationToken                  CancellationToken);

    /// <summary>
    /// A delegate for logging a HTTP WebSocket CLOSE message.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="WWCPWebSocketServer">The HTTP WebSocket server.</param>
    /// <param name="Connection">The HTTP WebSocket connection to be closed.</param>
    /// <param name="NetworkingNodeId">The sending OCPP networking node/charging station identification.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="StatusCode">The HTTP WebSocket Closing Status Code.</param>
    /// <param name="Reason">An optional HTTP WebSocket closing reason.</param>
    /// <param name="CancellationToken">A token to cancel the processing.</param>
    public delegate Task OnNetworkingNodeCloseMessageReceivedDelegate   (DateTime                           Timestamp,
                                                                         IWWCPWebSocketServer               WWCPWebSocketServer,
                                                                         WebSocketServerConnection          Connection,
                                                                         NetworkingNode_Id                  NetworkingNodeId,
                                                                         EventTracking_Id                   EventTrackingId,
                                                                         WebSocketFrame.ClosingStatusCode   StatusCode,
                                                                         String?                            Reason,
                                                                         CancellationToken                  CancellationToken);

    /// <summary>
    /// A delegate for logging a closed TCP connection.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="WWCPWebSocketServer">The HTTP WebSocket server.</param>
    /// <param name="Connection">The HTTP WebSocket connection to be closed.</param>
    /// <param name="NetworkingNodeId">The sending OCPP networking node/charging station identification.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="Reason">An optional closing reason.</param>
    /// <param name="CancellationToken">A token to cancel the processing.</param>
    public delegate Task OnNetworkingNodeTCPConnectionClosedDelegate    (DateTime                           Timestamp,
                                                                         IWWCPWebSocketServer               WWCPWebSocketServer,
                                                                         WebSocketServerConnection          Connection,
                                                                         NetworkingNode_Id                  NetworkingNodeId,
                                                                         EventTracking_Id                   EventTrackingId,
                                                                         String?                            Reason,
                                                                         CancellationToken                  CancellationToken);

    #endregion


    public abstract class AWWCPNetworkingNode : INetworkingNode
    {

        #region Data

        protected        readonly  HashSet<IWWCPWebSocketServer>                            wwcpWebSocketServers         = [];
        protected        readonly  List<IWWCPWebSocketClient>                               wwcpWebSocketClients         = [];

        protected static readonly  TimeSpan                                                 SemaphoreSlimTimeout         = TimeSpan.FromSeconds(5);

        private          readonly  HashSet<SignaturePolicy>                                 signaturePolicies            = [];

        private                    Int64                                                    internalRequestId            = 900000;

        //private          readonly  List<EnqueuedRequest>                                    EnqueuedRequests             = [];


        /// <summary>
        /// The default time span between heartbeat requests.
        /// </summary>
        public           readonly  TimeSpan                                                 DefaultSendHeartbeatsEvery   = TimeSpan.FromSeconds(30);

        protected        readonly  Timer                                                    SendHeartbeatsTimer;


        /// <summary>
        /// The default maintenance interval.
        /// </summary>
        public           readonly  TimeSpan                                                 DefaultMaintenanceEvery      = TimeSpan.FromSeconds(1);
        private   static readonly  SemaphoreSlim                                            MaintenanceSemaphore         = new (1, 1);
        private          readonly  Timer                                                    MaintenanceTimer;

        //public ConcurrentDictionary<String, List<ComponentConfig>>                          ComponentConfigs             = [];

        public List<UserRole>                                                               UserRoles                    = [];

        private DateTime lastRoutesBroadcast = Timestamp.Now;

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
        /// Disable all heartbeats.
        /// </summary>
        public Boolean                     DisableSendHeartbeats      { get; set; }

        /// <summary>
        /// The time span between heartbeat requests.
        /// </summary>
        public TimeSpan                    SendHeartbeatsEvery        { get; set; }






        /// <summary>
        /// Disable all maintenance tasks.
        /// </summary>
        public Boolean                     DisableMaintenanceTasks    { get; set; } = true;

        /// <summary>
        /// The maintenance interval.
        /// </summary>
        public TimeSpan                    MaintenanceEvery           { get; }


        public DNSClient                   DNSClient                  { get; }

        public HTTPExtAPI?                 HTTPExtAPI                 { get; }


        public String? ClientCloseMessage
            => "Bye!";


        public IEnumerable<IWWCPWebSocketClient> WWCPWebSocketClients
            => wwcpWebSocketClients;

        public IEnumerable<IWWCPWebSocketServer> WWCPWebSocketServers
            => wwcpWebSocketServers;

        /// <summary>
        /// Routing of messages.
        /// </summary>
        public Routing                      Routing                  { get; }

        #endregion

        #region Events

        #region WebSocket Server

        /// <summary>
        /// An event sent whenever the HTTP web socket server started.
        /// </summary>
        public event OnServerStartedDelegate?                         OnWebSocketServerStarted;

        /// <summary>
        /// An event sent whenever a new TCP connection was accepted.
        /// </summary>
        public event OnValidateTCPConnectionDelegate?                 OnValidateTCPConnection;

        /// <summary>
        /// An event sent whenever a new TCP connection was accepted.
        /// </summary>
        public event OnNewTCPConnectionDelegate?                      OnNewWebSocketTCPConnection;

        /// <summary>
        /// An event sent whenever a HTTP request was received.
        /// </summary>
        public event HTTPRequestLogDelegate?                          OnHTTPRequest;

        /// <summary>
        /// An event sent whenever the HTTP headers of a new web socket connection
        /// need to be validated or filtered by an upper layer application logic.
        /// </summary>
        public event OnValidateWebSocketConnectionDelegate?           OnValidateWebSocketConnection;

        /// <summary>
        /// An event sent whenever the HTTP connection switched successfully to web socket.
        /// </summary>
        public event OnNewWebSocketConnectionDelegate?                OnNewWebSocketServerConnection;

        /// <summary>
        /// An event sent whenever a reponse to a HTTP request was sent.
        /// </summary>
        public event HTTPResponseLogDelegate?                         OnHTTPResponse;

        /// <summary>
        /// An event sent whenever a web socket close frame was received.
        /// </summary>
        public event OnWebSocketServerCloseMessageReceivedDelegate?   OnWebSocketServerCloseMessageReceived;

        /// <summary>
        /// An event sent whenever a TCP connection was closed.
        /// </summary>
        public event OnTCPConnectionClosedDelegate?                   OnWebSocketServerTCPConnectionClosed;

        /// <summary>
        /// An event sent whenever the HTTP web socket server stopped.
        /// </summary>
        public event OnServerStoppedDelegate?                         OnWebSocketServerStopped;

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
        /// An event sent whenever a JSON message request was sent.
        /// </summary>
        public event OnWebSocketJSONMessageRequestDelegate?     OnJSONMessageRequestSent;

        /// <summary>
        /// An event sent whenever the response to a JSON message request was received.
        /// </summary>
        public event OnWebSocketJSONMessageResponseDelegate?    OnJSONMessageResponseReceived;

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
        /// An event sent whenever a binary message request was sent.
        /// </summary>
        public event OnWebSocketBinaryMessageRequestDelegate?     OnBinaryMessageRequestSent;

        /// <summary>
        /// An event sent whenever the response to a binary message request was received.
        /// </summary>
        public event OnWebSocketBinaryMessageResponseDelegate?    OnBinaryMessageResponseReceived;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new networking node for testing.
        /// </summary>
        /// <param name="Id">The unique identification of this networking node.</param>
        public AWWCPNetworkingNode(NetworkingNode_Id  Id,
                                   I18NString?        Description                 = null,
                                   CustomData?        CustomData                  = null,

                                   SignaturePolicy?   SignaturePolicy             = null,
                                   SignaturePolicy?   ForwardingSignaturePolicy   = null,

                                   HTTPExtAPI?        HTTPExtAPI                  = null,

                                   Boolean            DisableSendHeartbeats       = false,
                                   TimeSpan?          SendHeartbeatsEvery         = null,
                                   TimeSpan?          DefaultRequestTimeout       = null,

                                   Boolean            DisableMaintenanceTasks     = false,
                                   TimeSpan?          MaintenanceEvery            = null,
                                   DNSClient?         DNSClient                   = null)

        {

            if (Id.IsNullOrEmpty)
                throw new ArgumentNullException(nameof(Id), "The given networking node identification must not be null or empty!");

            this.Id                       = Id;
            this.Description              = Description;
            this.CustomData               = CustomData       ?? new CustomData(Vendor_Id.GraphDefined);

            this.DNSClient                = DNSClient        ?? new DNSClient(SearchForIPv6DNSServers: false);

            this.Routing                  = new Routing(this);

            //this.OCPP                     = new OCPPAdapter(
            //                                    this,
            //                                    DisableSendHeartbeats,
            //                                    SendHeartbeatsEvery,
            //                                    DefaultRequestTimeout,
            //                                    SignaturePolicy,
            //                                    ForwardingSignaturePolicy
            //                                );

            this.HTTPExtAPI               = HTTPExtAPI;

            this.DisableSendHeartbeats    = DisableSendHeartbeats;
            this.SendHeartbeatsEvery      = SendHeartbeatsEvery   ?? DefaultSendHeartbeatsEvery;
            this.SendHeartbeatsTimer      = new Timer(
                                                DoSendHeartbeatsSync,
                                                null,
                                                this.SendHeartbeatsEvery,
                                                this.SendHeartbeatsEvery
                                            );

            this.DisableMaintenanceTasks  = DisableMaintenanceTasks;
            this.MaintenanceEvery         = MaintenanceEvery ?? DefaultMaintenanceEvery;
            this.MaintenanceTimer         = new Timer(
                                                DoMaintenanceSync,
                                                null,
                                                this.MaintenanceEvery,
                                                this.MaintenanceEvery
                                            );

        }

        #endregion



        #region ConnectWebSocketClient(...)

        public async Task<HTTPResponse> ConnectWebSocketClient(URL                                                             RemoteURL,
                                                               HTTPHostname?                                                   VirtualHostname              = null,
                                                               I18NString?                                                     Description                  = null,
                                                               Boolean?                                                        PreferIPv4                   = null,
                                                               RemoteTLSServerCertificateValidationHandler<IWebSocketClient>?  RemoteCertificateValidator   = null,
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
                                                               NetworkingNode_Id?                                              NextHopNetworkingNodeId      = null,
                                                               IEnumerable<NetworkingNode_Id>?                                 RoutingNetworkingNodeIds     = null,

                                                               Boolean                                                         DisableWebSocketPings        = false,
                                                               TimeSpan?                                                       WebSocketPingEvery           = null,
                                                               TimeSpan?                                                       SlowNetworkSimulationDelay   = null,

                                                               Boolean                                                         DisableMaintenanceTasks      = false,
                                                               TimeSpan?                                                       MaintenanceEvery             = null,

                                                               String?                                                         LoggingPath                  = null,
                                                               String                                                          LoggingContext               = null, //CPClientLogger.DefaultContext,
                                                               LogfileCreatorDelegate?                                         LogfileCreator               = null,
                                                               HTTPClientLogger?                                               HTTPLogger                   = null,
                                                               DNSClient?                                                      DNSClient                    = null,

                                                               EventTracking_Id?                                               EventTrackingId              = null,
                                                               CancellationToken                                               CancellationToken            = default)

            => await ConnectWebSocketClient(
                         new WWCPWebSocketClient(

                             this,
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

                         ),

                         NextHopNetworkingNodeId,
                         RoutingNetworkingNodeIds,
                         EventTrackingId,
                         CancellationToken

                     );



        public async Task<HTTPResponse> ConnectWebSocketClient(WWCPWebSocketClient              wwcpWebSocketClient,

                                                               NetworkingNode_Id?               NextHopNetworkingNodeId    = null,
                                                               IEnumerable<NetworkingNode_Id>?  RoutingNetworkingNodeIds   = null,

                                                               EventTracking_Id?                EventTrackingId            = null,
                                                               CancellationToken                CancellationToken          = default)
        {

            NextHopNetworkingNodeId ??= NetworkingNode_Id.CSMS;


            wwcpWebSocketClients.Add(wwcpWebSocketClient);

            var connectResponse = await wwcpWebSocketClient.Connect(
                                            EventTrackingId:      EventTrackingId ?? EventTracking_Id.New,
                                            RequestTimeout:       wwcpWebSocketClient.RequestTimeout,
                                            MaxNumberOfRetries:   wwcpWebSocketClient.MaxNumberOfRetries,
                                            HTTPRequestBuilder:   (httpRequestBuilder) => {
                                                                      if (wwcpWebSocketClient.NetworkingMode == NetworkingMode.OverlayNetwork)
                                                                          httpRequestBuilder.SetHeaderField(WebSocketKeys.X_WWCP_NetworkingMode, wwcpWebSocketClient.NetworkingMode.ToString());
                                                                  },
                                            CancellationToken:    CancellationToken
                                        );

            if (connectResponse.Item2.HTTPStatusCode == HTTPStatusCode.SwitchingProtocols &&
                connectResponse.Item1 is not null)
            {

                if (NextHopNetworkingNodeId is not null)
                {

                    connectResponse.Item1.TryAddCustomData(
                        WebSocketKeys.NetworkingNodeId,
                        NextHopNetworkingNodeId
                    );

                    Routing.AddOrUpdateStaticRouting(
                        NextHopNetworkingNodeId.Value,
                        wwcpWebSocketClient,
                        0,
                        1,
                        Timestamp.Now
                    );

                }

                if (RoutingNetworkingNodeIds is not null && RoutingNetworkingNodeIds.Any())
                    Routing.AddOrUpdateStaticRouting(
                        RoutingNetworkingNodeIds,
                        wwcpWebSocketClient,
                        0,
                        1,
                        Timestamp.Now
                    );

            }

            return connectResponse.Item2;

        }

        #endregion



        //protected virtual Task WWCPWebSocketClient_OnJSONMessageReceived(DateTime                     Timestamp,
        //                                                                 IWWCPWebSocketClient         Client,
        //                                                                 WebSocketClientConnection    Connection,
        //                                                                 EventTracking_Id             EventTrackingId,
        //                                                                 DateTime                     MessageTimestamp,
        //                                                                 NetworkingNode_Id            SourceNodeId,
        //                                                                 Newtonsoft.Json.Linq.JArray  Message,
        //                                                                 CancellationToken            CancellationToken)
        //{
        //    return Task.CompletedTask;
        //}

        //protected virtual Task WWCPWebSocketClient_OnBinaryMessageReceived(DateTime                     Timestamp,
        //                                                                   IWWCPWebSocketClient         Client,
        //                                                                   WebSocketClientConnection    Connection,
        //                                                                   EventTracking_Id             EventTrackingId,
        //                                                                   DateTime                     MessageTimestamp,
        //                                                                   NetworkingNode_Id            SourceNodeId,
        //                                                                   Byte[]                       MessageMessage,
        //                                                                   CancellationToken            CancellationToken)
        //{
        //    return Task.CompletedTask;
        //}







        #region AddOrUpdateHTTPBasicAuth(NetworkingNodeId, Password)

        /// <summary>
        /// Add the given HTTP Basic Authentication password for the given networking node.
        /// </summary>
        /// <param name="NetworkingNodeId">The unique identification of the networking node.</param>
        /// <param name="Password">The password of the networking node.</param>
        public void AddOrUpdateHTTPBasicAuth(NetworkingNode_Id  NetworkingNodeId,
                                             String             Password)
        {

            foreach (var webSocketServer in wwcpWebSocketServers)
            {
                webSocketServer.AddOrUpdateHTTPBasicAuth(NetworkingNodeId.ToString(), Password);
            }

        }

        #endregion

        #region RemoveHTTPBasicAuth(NetworkingNodeId)

        /// <summary>
        /// Remove the given HTTP Basic Authentication for the given networking node.
        /// </summary>
        /// <param name="NetworkingNodeId">The unique identification of the networking node.</param>
        public void RemoveHTTPBasicAuth(NetworkingNode_Id NetworkingNodeId)
        {

            foreach (var webSocketServer in wwcpWebSocketServers)
            {
                webSocketServer.RemoveHTTPBasicAuth(NetworkingNodeId.ToString());
            }

        }

        #endregion


        #region (protected virtual) WireWebSocketServer(WebSocketServer)

        protected virtual void WireWebSocketServer(IWWCPWebSocketServer WebSocketServer)
        {

            wwcpWebSocketServers.Add(WebSocketServer);


            #region OnWebSocketServerStarted

            WebSocketServer.OnServerStarted += async (timestamp,
                                                      server,
                                                      eventTrackingId,
                                                      cancellationToken) => {

                var onServerStarted = OnWebSocketServerStarted;
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
                                  nameof(AWWCPNetworkingNode),
                                  nameof(OnWebSocketServerStarted),
                                  e
                              );
                    }
                }

            };

            #endregion

            #region OnNewWebSocketTCPConnection

            WebSocketServer.OnNewTCPConnection += async (timestamp,
                                                         webSocketServer,
                                                         newTCPConnection,
                                                         eventTrackingId,
                                                         cancellationToken) => {

                var onNewTCPConnection = OnNewWebSocketTCPConnection;
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
                                  nameof(AWWCPNetworkingNode),
                                  nameof(OnNewWebSocketTCPConnection),
                                  e
                              );
                    }
                }

            };

            #endregion

            // Failed (Charging Station) Authentication

            #region OnNetworkingNodeNewWebSocketConnection

            WebSocketServer.OnNetworkingNodeNewWebSocketConnection += async (timestamp,
                                                                             webSocketServer,
                                                                             newConnection,
                                                                             networkingNodeId,
                                                                             networkingMode,
                                                                             sharedSubprotocols,
                                                                             eventTrackingId,
                                                                             cancellationToken) => {

                //// A new connection from the same networking node/charging station will replace the older one!
                if (webSocketServer is IWWCPWebSocketServer wwcpWebSocketServer)
                {

                    Routing.AddOrUpdateStaticRouting(
                        DestinationId:    networkingNodeId,
                        WebSocketServer:  wwcpWebSocketServer,
                        Priority:         0,
                        Timestamp:        timestamp
                    );

                    #region Send OnNewWebSocketServerConnection

                    //var logger = OnNewWebSocketServerConnection;
                    //if (logger is not null)
                    //{
                    //    try
                    //    {

                    //        await Task.WhenAll(logger.GetInvocationList().
                    //                               OfType <OnNetworkingNodeNewWebSocketConnectionDelegate>().
                    //                               Select (loggingDelegate => loggingDelegate.Invoke(
                    //                                                              timestamp,
                    //                                                              wwcpWebSocketServer,
                    //                                                              newConnection,
                    //                                                              networkingNodeId,
                    //                                                              sharedSubprotocols,
                    //                                                              eventTrackingId,
                    //                                                              cancellationToken
                    //                                                          )).
                    //                               ToArray());

                    //    }
                    //    catch (Exception e)
                    //    {
                    //        await HandleErrors(
                    //                  nameof(AWWCPNetworkingNode),
                    //                  nameof(OnNewWebSocketServerConnection),
                    //                  e
                    //              );
                    //    }
                    //}

                    #endregion

                }

            };

            #endregion

            #region OnWebSocketServerCloseMessageReceived

            //WebSocketServer.OnNetworkingNodeCloseMessageReceived += async (timestamp,
            //                                                               server,
            //                                                               connection,
            //                                                               networkingNodeId,
            //                                                               eventTrackingId,
            //                                                               statusCode,
            //                                                               reason,
            //                                                               cancellationToken) => {

            //    var logger = OnWebSocketServerCloseMessageReceived;
            //    if (logger is not null)
            //    {
            //        try
            //        {

            //            await Task.WhenAll(logger.GetInvocationList().
            //                                   OfType <OnNetworkingNodeCloseMessageReceivedDelegate>().
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
            //                      nameof(AWWCPNetworkingNode),
            //                      nameof(OnWebSocketServerCloseMessageReceived),
            //                      e
            //                  );
            //        }
            //    }

            //};

            #endregion

            #region OnWebSocketServerTCPConnectionClosed

            //WebSocketServer.OnNetworkingNodeTCPConnectionClosed += async (timestamp,
            //                                                              server,
            //                                                              connection,
            //                                                              networkingNodeId,
            //                                                              eventTrackingId,
            //                                                              reason,
            //                                                              cancellationToken) => {

            //    var logger = OnWebSocketServerTCPConnectionClosed;
            //    if (logger is not null)
            //    {
            //        try
            //        {

            //            await Task.WhenAll(logger.GetInvocationList().
            //                                   OfType <OnNetworkingNodeTCPConnectionClosedDelegate>().
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
            //                      nameof(AWWCPNetworkingNode),
            //                      nameof(OnWebSocketServerTCPConnectionClosed),
            //                      e
            //                  );
            //        }
            //    }

            //};

            #endregion

            #region OnWebSocketServerStopped

            WebSocketServer.OnServerStopped += async (timestamp,
                                                      server,
                                                      eventTrackingId,
                                                      reason,
                                                      cancellationToken) => {

                var logger = OnWebSocketServerStopped;
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
                                  nameof(AWWCPNetworkingNode),
                                  nameof(OnWebSocketServerStopped),
                                  e
                              );
                    }
                }

            };

            #endregion


            // (Generic) Error Handling

        }

        #endregion


        #region Shutdown(Message = null, Wait = true)

        /// <summary>
        /// Shutdown the HTTP web socket listener thread.
        /// </summary>
        /// <param name="Message">An optional shutdown message.</param>
        /// <param name="Wait">Wait until the server finally shutted down.</param>
        public async Task Shutdown(String?  Message   = null,
                                   Boolean  Wait      = true)
        {

            await Task.WhenAll(wwcpWebSocketServers.
                                   Select (ocppWebSocketServer => ocppWebSocketServer.Shutdown(Message, Wait)).
                                   ToArray());

        }

        #endregion


        #region NextRequestId

        public Request_Id NextRequestId
        {
            get
            {

                Interlocked.Increment(ref internalRequestId);

                return Request_Id.Parse(internalRequestId.ToString());

            }
        }

        #endregion


        public Byte[] GetEncryptionKey(NetworkingNode_Id  DestinationId,
                                       UInt16?            KeyId   = null)
        {
            return "5a733d6660df00c447ff184ae971e1d5bba5de5784768795ee6535867130aa12".HexStringToByteArray();
        }

        public Byte[] GetDecryptionKey(NetworkingNode_Id  SourceNodeId,
                                       UInt16?            KeyId   = null)
        {
            return "5a733d6660df00c447ff184ae971e1d5bba5de5784768795ee6535867130aa12".HexStringToByteArray();
        }


        public UInt64 GetEncryptionNonce(NetworkingNode_Id  DestinationId,
                                         UInt16?            KeyId   = null)
        {
            return 1;
        }

        public UInt64 GetEncryptionCounter(NetworkingNode_Id  DestinationId,
                                           UInt16?            KeyId   = null)
        {
            return 1;
        }



        #region (private) GetValidUserRolesFor(Signatures)

        /// <summary>
        /// Return the valid user roles for the given request.
        /// </summary>
        /// <param name="Request"></param>
        private IEnumerable<UserRole> GetValidUserRolesFor(IEnumerable<Signature> Signatures)
        {

            var userRoles = new List<UserRole>();

            foreach (var signature in Signatures)
            {
                if (signature.Status == VerificationStatus.ValidSignature)
                {
                    foreach (var userRole in UserRoles)
                    {
                        if (userRole.KeyPairs.Any(keyPair => signature.KeyId.SequenceEqual(keyPair.PublicKeyBytes)))
                        {
                            userRoles.Add(userRole);
                        }
                    }
                }
            }

            return userRoles;

        }

        #endregion

        #region GetValidUserRolesFor(SignedRequest)

        /// <summary>
        /// Return the valid user roles for the given request.
        /// </summary>
        /// <param name="SignedRequest">A signed request.</param>
        public IEnumerable<UserRole> GetValidUserRolesFor(IRequest SignedRequest)
            => GetValidUserRolesFor(SignedRequest.Signatures);

        #endregion

        #region GetValidUserRolesFor(SignedResponse)

        /// <summary>
        /// Return the valid user roles for the given response.
        /// </summary>
        /// <param name="SignedResponse">A signed response.</param>
        public IEnumerable<UserRole> GetValidUserRolesFor(IResponse SignedResponse)
            => GetValidUserRolesFor(SignedResponse.Signatures);

        #endregion

        #region GetValidUserRolesFor(SignedMessage)

        /// <summary>
        /// Return the valid user roles for the given message.
        /// </summary>
        /// <param name="SignedMessage">A signed message.</param>
        public IEnumerable<UserRole> GetValidUserRolesFor(IMessage SignedMessage)
            => GetValidUserRolesFor(SignedMessage.Signatures);

        #endregion


        #region (Timer) DoSendHeartbeatSync(State)

        protected virtual void DoSendHeartbeatsSync(Object? State)
        { }

        #endregion

        #region (Timer) DoMaintenance(State)

        private void DoMaintenanceSync(Object? State)
        {
            if (!DisableMaintenanceTasks)
                DoMaintenance(State).Wait();
        }

        private async Task DoMaintenance(Object? State)
        {

            //DebugX.LogT($"Node {Id}: Enter DoMaintenance(...)");

            if (await MaintenanceSemaphore.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    await DoMaintenanceAsync(State);

                }
                catch (Exception e)
                {

                    while (e.InnerException is not null)
                        e = e.InnerException;

                    DebugX.LogException(e);

                }
                finally
                {
                    MaintenanceSemaphore.Release();
                }
            }
            else
                DebugX.LogT($"Node {Id}: Could not aquire the maintenance tasks lock!");

            //DebugX.LogT($"Node {Id}: Exit DoMaintenance(...)");

        }

        protected virtual async Task DoMaintenanceAsync(Object? State)
        {

            DebugX.Log($"Node {Id}: DoMaintenanceAsync(State)");

            await Task.Delay(1);

            if (Timestamp.Now > lastRoutesBroadcast + TimeSpan.FromSeconds(10))
            {

                //lastRoutesBroadcast = Timestamp.Now;

                //var routes = OCPP.Routing.GetNetworkRoutingInformation();
                //if (routes.Any())
                //{

                //    var notifyNetworkTopologyMessage = new NotifyNetworkTopologyMessage(
                //                                           Destination:                  SourceRouting.Broadcast,
                //                                           NetworkTopologyInformation:   new NetworkTopologyInformation(
                //                                                                             RoutingNode:  Id,
                //                                                                             Routes:       routes,
                //                                                                             NotBefore:    null,
                //                                                                             NotAfter:     null,
                //                                                                             Priority:     null
                //                                                                         )
                //                                       );

                //    var rr = OCPP.OUT.NotifyNetworkTopology(notifyNetworkTopologyMessage);

                //}

            }

            //foreach (var enqueuedRequest in EnqueuedRequests.ToArray())
            //{
            //    //if (CSClient is ChargingStationWSClient wsClient)
            //    //{

            //    //    var response = await wsClient.SendRequest(
            //    //                             enqueuedRequest.DestinationId,
            //    //                             enqueuedRequest.Command,
            //    //                             enqueuedRequest.Request.RequestId,
            //    //                             enqueuedRequest.RequestJSON
            //    //                         );

            //    //    enqueuedRequest.ResponseAction(response);

            //    //    EnqueuedRequests.Remove(enqueuedRequest);

            //    //}
            //}

        }

        #endregion



        #region LogEvent(OCPPIO, Logger, LogHandler, ...)

        public async Task LogEvent<TDelegate>(String                                             OCPPIO,
                                              TDelegate?                                         Logger,
                                              Func<TDelegate, Task>                              LogHandler,
                                              [CallerArgumentExpression(nameof(Logger))] String  EventName     = "",
                                              [CallerMemberName()]                       String  OCPPCommand   = "")

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
                    await HandleErrors(OCPPIO, $"{OCPPCommand}.{EventName}", e);
                }
            }
        }

        #endregion

        #region (virtual) HandleErrors(Module, Caller, ErrorResponse)

        public virtual Task HandleErrors(String  Module,
                                         String  Caller,
                                         String  ErrorResponse)
        {

            DebugX.Log($"{Module}.{Caller}: {ErrorResponse}");

            return Task.CompletedTask;

        }

        #endregion

        #region (virtual) HandleErrors(Module, Caller, ExceptionOccured)

        public virtual Task HandleErrors(String     Module,
                                         String     Caller,
                                         Exception  ExceptionOccured)
        {

            DebugX.LogException(ExceptionOccured, Caller);

            return Task.CompletedTask;

        }

        #endregion


    }

}
