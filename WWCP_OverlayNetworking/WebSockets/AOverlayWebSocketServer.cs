﻿/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.OverlayNetworking.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking
{


    public delegate Task OnWebSocketJSONMessageRequestDelegate   (DateTime                    Timestamp,
                                                                  IEventSender                Server,
                                                                  WebSocketServerConnection   Connection,
                                                                  NetworkingNode_Id           DestinationNodeId,
                                                                  NetworkPath                 NetworkPath,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTime                    RequestTimestamp,
                                                                  JArray                      RequestMessage,
                                                                  CancellationToken           CancellationToken);

    public delegate Task OnWebSocketJSONMessageResponseDelegate  (DateTime                    Timestamp,
                                                                  IEventSender                Server,
                                                                  WebSocketServerConnection   Connection,
                                                                  NetworkingNode_Id           DestinationNodeId,
                                                                  NetworkPath                 NetworkPath,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTime                    RequestTimestamp,
                                                                  JArray                      JSONRequestMessage,
                                                                  Byte[]                      BinaryRequestMessage,
                                                                  DateTime                    ResponseTimestamp,
                                                                  JArray                      ResponseMessage,
                                                                  CancellationToken           CancellationToken);

    public delegate Task OnWebSocketTextErrorResponseDelegate    (DateTime                    Timestamp,
                                                                  IEventSender                Server,
                                                                  WebSocketServerConnection   Connection,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTime                    RequestTimestamp,
                                                                  String                      TextRequestMessage,
                                                                  Byte[]                      BinaryRequestMessage,
                                                                  DateTime                    ResponseTimestamp,
                                                                  String                      TextResponseMessage,
                                                                  CancellationToken           CancellationToken);



    public delegate Task OnWebSocketBinaryMessageRequestDelegate (DateTime                    Timestamp,
                                                                  IEventSender                Server,
                                                                  WebSocketServerConnection   Connection,
                                                                  NetworkingNode_Id           DestinationNodeId,
                                                                  NetworkPath                 NetworkPath,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTime                    RequestTimestamp,
                                                                  Byte[]                      RequestMessage,
                                                                  CancellationToken           CancellationToken);

    public delegate Task OnWebSocketBinaryMessageResponseDelegate(DateTime                    Timestamp,
                                                                  IEventSender                Server,
                                                                  WebSocketServerConnection   Connection,
                                                                  NetworkingNode_Id           DestinationNodeId,
                                                                  NetworkPath                 NetworkPath,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTime                    RequestTimestamp,
                                                                  JArray                      JSONRequestMessage,
                                                                  Byte[]                      BinaryRequestMessage,
                                                                  DateTime                    ResponseTimestamp,
                                                                  Byte[]                      ResponseMessage,
                                                                  CancellationToken           CancellationToken);


    public class OverlayWebSocketServer : AOverlayWebSocketServer
    {

        #region Constructor(s)

        /// <summary>
        /// Initialize a new HTTP server for the CSMS HTTP/WebSocket/JSON API.
        /// </summary>
        /// <param name="HTTPServiceName">An optional identification string for the HTTP service.</param>
        /// <param name="IPAddress">An IP address to listen on.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="Description">An optional description of this HTTP Web Socket service.</param>
        /// 
        /// <param name="RequireAuthentication">Require a HTTP Basic Authentication of all charging boxes.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public OverlayWebSocketServer(NetworkingNode_Id                                                                                           NetworkingNodeId,
                                      IEnumerable<String>                                                                                         SupportedEEBusWebSocketSubprotocols,

                                      String                                                                                                      HTTPServiceName                      = DefaultHTTPServiceName,
                                      IIPAddress?                                                                                                 IPAddress                            = null,
                                      IPPort?                                                                                                     TCPPort                              = null,
                                      I18NString?                                                                                                 Description                          = null,

                                      Boolean                                                                                                     RequireAuthentication                = true,
                                      Boolean                                                                                                     DisableWebSocketPings                = false,
                                      TimeSpan?                                                                                                   WebSocketPingEvery                   = null,
                                      TimeSpan?                                                                                                   SlowNetworkSimulationDelay           = null,

                                      Func<X509Certificate2>?                                                                                     ServerCertificateSelector            = null,
                                      RemoteTLSClientCertificateValidationHandler<org.GraphDefined.Vanaheimr.Hermod.WebSocket.IWebSocketServer>?  ClientCertificateValidator           = null,
                                      LocalCertificateSelectionHandler?                                                                           LocalCertificateSelector             = null,
                                      SslProtocols?                                                                                               AllowedTLSProtocols                  = null,
                                      Boolean?                                                                                                    ClientCertificateRequired            = null,
                                      Boolean?                                                                                                    CheckCertificateRevocation           = null,

                                      ServerThreadNameCreatorDelegate?                                                                            ServerThreadNameCreator              = null,
                                      ServerThreadPriorityDelegate?                                                                               ServerThreadPrioritySetter           = null,
                                      Boolean?                                                                                                    ServerThreadIsBackground             = null,
                                      ConnectionIdBuilder?                                                                                        ConnectionIdBuilder                  = null,
                                      TimeSpan?                                                                                                   ConnectionTimeout                    = null,
                                      UInt32?                                                                                                     MaxClientConnections                 = null,

                                      DNSClient?                                                                                                  DNSClient                            = null,
                                      Boolean                                                                                                     AutoStart                            = false)

            : base(NetworkingNodeId,
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
                   AutoStart)

        {

        }

        #endregion

    }


    /// <summary>
    /// An EEBus HTTP Web Socket server runs on a CSMS or a networking node
    /// accepting connections from charging stations or other networking nodes
    /// to invoke EEBus commands.
    /// </summary>
    public abstract class AOverlayWebSocketServer : IEventSender
    {

        #region Data

        private readonly WebSocketServer webSocketServer;

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public const            String                                                                                DefaultHTTPServiceName            = $"GraphDefined EEBus {Version.String} HTTP/WebSocket/JSON CSMS API";

        /// <summary>
        /// The default HTTP server TCP port.
        /// </summary>
        public static readonly  IPPort                                                                                DefaultHTTPServerPort             = IPPort.Parse(2010);

        /// <summary>
        /// The default HTTP server URI prefix.
        /// </summary>
        public static readonly  HTTPPath                                                                              DefaultURLPrefix                  = HTTPPath.Parse("/" + Version.String);

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public static readonly  TimeSpan                                                                              DefaultRequestTimeout             = TimeSpan.FromSeconds(30);

        protected readonly      Dictionary<String, MethodInfo>                                                        incomingMessageProcessorsLookup   = [];
        protected readonly      ConcurrentDictionary<NetworkingNode_Id, Tuple<WebSocketServerConnection, DateTime>>   connectedNetworkingNodes          = [];
        protected readonly      ConcurrentDictionary<NetworkingNode_Id, NetworkingNode_Id>                            reachableViaNetworkingHubs        = [];
        protected readonly      ConcurrentDictionary<Request_Id, SendRequestState>                                    requests                          = [];

        public    const         String                                                                                networkingNodeId_WebSocketKey     = "networkingNodeId";
        public    const         String                                                                                networkingMode_WebSocketKey       = "networkingMode";
        public    const         String                                                                                LogfileName                       = "CSMSWSServer.log";

        #endregion

        #region Properties

        public Boolean                                 DisableWebSocketPings
        {
            get { return webSocketServer.DisableWebSocketPings; }
            set { webSocketServer.DisableWebSocketPings = value; }
        }
        public DNSClient?                              DNSClient
            => webSocketServer.DNSClient;
        public String                                  HTTPServiceName
            => webSocketServer.HTTPServiceName;
        public IIPAddress                              IPAddress
            => webSocketServer.IPAddress;
        public IPPort                                  IPPort
            => webSocketServer.IPPort;
        public IPSocket                                IPSocket
            => webSocketServer.IPSocket;
        public Boolean                                 IsRunning
            => webSocketServer.IsRunning;
        public HashSet<String>                         SecWebSocketProtocols
            => webSocketServer.SecWebSocketProtocols;
        public Boolean                                 ServerThreadIsBackground
            => webSocketServer.ServerThreadIsBackground;
        public ServerThreadNameCreatorDelegate         ServerThreadNameCreator
            => webSocketServer.ServerThreadNameCreator;
        public ServerThreadPriorityDelegate            ServerThreadPrioritySetter
            => webSocketServer.ServerThreadPrioritySetter;
        public TimeSpan?                               SlowNetworkSimulationDelay
        {
            get { return webSocketServer.SlowNetworkSimulationDelay; }
            set { webSocketServer.SlowNetworkSimulationDelay = value; }
        }
        public IEnumerable<WebSocketServerConnection>  WebSocketConnections
            => webSocketServer.WebSocketConnections;
        public TimeSpan                                WebSocketPingEvery
        {
            get { return webSocketServer.WebSocketPingEvery; }
            set { webSocketServer.WebSocketPingEvery = value; }
        }









        /// <summary>
        /// The sender identification.
        /// </summary>
        String IEventSender.Id
            => webSocketServer.HTTPServiceName;

        public NetworkingNode_Id                                  NetworkingNodeId         { get; }

        /// <summary>
        /// The enumeration of all connected networking nodes.
        /// </summary>
        public IEnumerable<NetworkingNode_Id> ConnectedNetworkingNodeIds
            => connectedNetworkingNodes.Keys;

        /// <summary>
        /// Require a HTTP Basic Authentication of all networking nodes.
        /// </summary>
        public Boolean                                            RequireAuthentication    { get; }

        /// <summary>
        /// Logins and passwords for HTTP Basic Authentication.
        /// </summary>
        public ConcurrentDictionary<NetworkingNode_Id, String?>   NetworkingNodeLogins     { get; }
            = new();

        /// <summary>
        /// The JSON formatting to use.
        /// </summary>
        public Formatting                                         JSONFormatting           { get; set; }
            = Formatting.None;

        /// <summary>
        /// The request timeout for messages sent by this HTTP WebSocket server.
        /// </summary>
        public TimeSpan?                                          RequestTimeout           { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever the HTTP web socket server started.
        /// </summary>
        public event OnServerStartedDelegate?                OnServerStarted
        {
            add    { webSocketServer.OnServerStarted += value; }
            remove { webSocketServer.OnServerStarted -= value; }
        }

        /// <summary>
        /// An event sent whenever a new TCP connection was accepted.
        /// </summary>
        public event OnValidateTCPConnectionDelegate?        OnValidateTCPConnection
        {
            add    { webSocketServer.OnValidateTCPConnection += value; }
            remove { webSocketServer.OnValidateTCPConnection -= value; }
        }

        /// <summary>
        /// An event sent whenever a new TCP connection was accepted.
        /// </summary>
        public event OnNewTCPConnectionDelegate?             OnNewTCPConnection
        {
            add    { webSocketServer.OnNewTCPConnection += value; }
            remove { webSocketServer.OnNewTCPConnection -= value; }
        }

        /// <summary>
        /// An event sent whenever a TCP connection was closed.
        /// </summary>
        //public event OnCSMSTCPConnectionClosedDelegate?      OnTCPConnectionClosed;
        //{
        //    add    { webSocketServer.OnTCPConnectionClosed += value; }
        //    remove { webSocketServer.OnTCPConnectionClosed -= value; }
        //}

        /// <summary>
        /// An event sent whenever a HTTP request was received.
        /// </summary>
        public event HTTPRequestLogDelegate?                 OnHTTPRequest
        {
            add    { webSocketServer.OnHTTPRequest += value; }
            remove { webSocketServer.OnHTTPRequest -= value; }
        }

        /// <summary>
        /// An event sent whenever the HTTP headers of a new web socket connection
        /// need to be validated or filtered by an upper layer application logic.
        /// </summary>
        public event OnValidateWebSocketConnectionDelegate?  OnValidateWebSocketConnection
        {
            add    { webSocketServer.OnValidateWebSocketConnection += value; }
            remove { webSocketServer.OnValidateWebSocketConnection -= value; }
        }

        /// <summary>
        /// An event sent whenever the HTTP connection switched successfully to web socket.
        /// </summary>
        //public event OnCSMSNewWebSocketConnectionDelegate?   OnNewWebSocketConnection;
        //{
        //    add    { webSocketServer.OnNewWebSocketConnection += value; }
        //    remove { webSocketServer.OnNewWebSocketConnection -= value; }
        //}

        /// <summary>
        /// An event sent whenever a reponse to a HTTP request was sent.
        /// </summary>
        public event HTTPResponseLogDelegate?                OnHTTPResponse
        {
            add    { webSocketServer.OnHTTPResponse += value; }
            remove { webSocketServer.OnHTTPResponse -= value; }
        }

        /// <summary>
        /// An event sent whenever a web socket frame was received.
        /// </summary>
        public event OnWebSocketFrameDelegate?                          OnWebSocketFrameReceived
        {
            add    { webSocketServer.OnWebSocketFrameReceived += value; }
            remove { webSocketServer.OnWebSocketFrameReceived -= value; }
        }

        /// <summary>
        /// An event sent whenever a web socket frame was sent.
        /// </summary>
        public event OnWebSocketFrameDelegate?                          OnWebSocketFrameSent
        {
            add    { webSocketServer.OnWebSocketFrameSent += value; }
            remove { webSocketServer.OnWebSocketFrameSent -= value; }
        }


        /// <summary>
        /// An event sent whenever a text message was received.
        /// </summary>
        public event OnWebSocketServerTextMessageReceivedDelegate?      OnTextMessageReceived
        {
            add    { webSocketServer.OnTextMessageReceived += value; }
            remove { webSocketServer.OnTextMessageReceived -= value; }
        }

        /// <summary>
        /// An event sent whenever a web socket frame was sent.
        /// </summary>
        public event OnWebSocketServerTextMessageSentDelegate?          OnTextMessageSent
        {
            add    { webSocketServer.OnTextMessageSent += value; }
            remove { webSocketServer.OnTextMessageSent -= value; }
        }

        /// <summary>
        /// An event sent whenever a binary message was received.
        /// </summary>
        public event OnWebSocketServerBinaryMessageReceivedDelegate?    OnBinaryMessageReceived
        {
            add    { webSocketServer.OnBinaryMessageReceived += value; }
            remove { webSocketServer.OnBinaryMessageReceived -= value; }
        }

        /// <summary>
        /// An event sent whenever a web socket frame was sent.
        /// </summary>
        public event OnWebSocketServerBinaryMessageSentDelegate?        OnBinaryMessageSent
        {
            add    { webSocketServer.OnBinaryMessageSent += value; }
            remove { webSocketServer.OnBinaryMessageSent -= value; }
        }

        /// <summary>
        /// An event sent whenever a web socket ping frame was received.
        /// </summary>
        public event OnWebSocketServerPingMessageReceivedDelegate?      OnPingMessageReceived
        {
            add    { webSocketServer.OnPingMessageReceived += value; }
            remove { webSocketServer.OnPingMessageReceived -= value; }
        }

        /// <summary>
        /// An event sent whenever a web socket ping frame was sent.
        /// </summary>
        public event OnWebSocketServerPingMessageSentDelegate?          OnPingMessageSent
        {
            add    { webSocketServer.OnPingMessageSent += value; }
            remove { webSocketServer.OnPingMessageSent -= value; }
        }

        /// <summary>
        /// An event sent whenever a web socket pong frame was received.
        /// </summary>
        public event OnWebSocketServerPongMessageReceivedDelegate?      OnPongMessageReceived
        {
            add    { webSocketServer.OnPongMessageReceived += value; }
            remove { webSocketServer.OnPongMessageReceived -= value; }
        }

        /// <summary>
        /// An event sent whenever a web socket close frame was received.
        /// </summary>
        //public event OnCSMSCloseMessageReceivedDelegate?     OnCloseMessageReceived;
        //{
        //    add    { webSocketServer.OnCloseMessageReceived += value; }
        //    remove { webSocketServer.OnCloseMessageReceived -= value; }
        //}

        /// <summary>
        /// An event sent whenever the HTTP web socket server stopped.
        /// </summary>
        public event OnServerStoppedDelegate?                OnServerStopped
        {
            add    { webSocketServer.OnServerStopped += value; }
            remove { webSocketServer.OnServerStopped -= value; }
        }






        #region Common Connection Management

        /// <summary>
        /// An event sent whenever the HTTP connection switched successfully to web socket.
        /// </summary>
     //   public event OnCSMSNewWebSocketConnectionDelegate?    OnNewWebSocketConnection;  //ToDo: Fix me!

        /// <summary>
        /// An event sent whenever the HTTP connection switched successfully to web socket.
        /// </summary>
        //public event OnCSMSNewWebSocketConnectionDelegate?    OnCSMSNewWebSocketConnection;

        /// <summary>
        /// An event sent whenever a web socket close frame was received.
        /// </summary>
     //   public event OnCSMSCloseMessageReceivedDelegate?      OnCloseMessageReceived;  //ToDo: Fix me!

        /// <summary>
        /// An event sent whenever a web socket close frame was received.
        /// </summary>
        //public event OnCSMSCloseMessageReceivedDelegate?      OnCSMSCloseMessageReceived;

        /// <summary>
        /// An event sent whenever a TCP connection was closed.
        /// </summary>
     //   public event OnCSMSTCPConnectionClosedDelegate?       OnTCPConnectionClosed;  //ToDo: Fix me!

        /// <summary>
        /// An event sent whenever a TCP connection was closed.
        /// </summary>
        //public event OnCSMSTCPConnectionClosedDelegate?       OnCSMSTCPConnectionClosed;

        #endregion

        #region Generic JSON Messages

        /// <summary>
        /// An event sent whenever a text message request was received.
        /// </summary>
        public event OnWebSocketJSONMessageRequestDelegate?     OnJSONMessageRequestReceived;

        /// <summary>
        /// An event sent whenever the response to a text message was sent.
        /// </summary>
        public event OnWebSocketJSONMessageResponseDelegate?    OnJSONMessageResponseSent;

        /// <summary>
        /// An event sent whenever the error response to a text message was sent.
        /// </summary>
        public event OnWebSocketTextErrorResponseDelegate?      OnJSONErrorResponseSent;


        /// <summary>
        /// An event sent whenever a text message request was sent.
        /// </summary>
        public event OnWebSocketJSONMessageRequestDelegate?     OnJSONMessageRequestSent;

        /// <summary>
        /// An event sent whenever the response to a text message request was received.
        /// </summary>
        public event OnWebSocketJSONMessageResponseDelegate?    OnJSONMessageResponseReceived;

        /// <summary>
        /// An event sent whenever an error response to a text message request was received.
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

        #region Custom JSON serializer delegates

        //public CustomJObjectSerializerDelegate<StatusInfo>?                                          CustomStatusInfoSerializer                                   { get; set; }
        //public CustomJObjectSerializerDelegate<OverlayNetworking.Signature>?                                      CustomSignatureSerializer                                    { get; set; }
        public CustomJObjectSerializerDelegate<CustomData>?                                          CustomCustomDataSerializer                                   { get; set; }


        // Binary Data Streams Extensions
        //public CustomBinarySerializerDelegate<OverlayNetworking.Signature>?                                       CustomBinarySignatureSerializer                              { get; set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initialize a new HTTP server for the CSMS HTTP/WebSocket/JSON API.
        /// </summary>
        /// <param name="HTTPServiceName">An optional identification string for the HTTP service.</param>
        /// <param name="IPAddress">An IP address to listen on.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="Description">An optional description of this HTTP Web Socket service.</param>
        /// 
        /// <param name="RequireAuthentication">Require a HTTP Basic Authentication of all charging boxes.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public AOverlayWebSocketServer(NetworkingNode_Id                                               NetworkingNodeId,
                                       IEnumerable<String>                                             SupportedEEBusWebSocketSubprotocols,
                                       String                                                          HTTPServiceName              = DefaultHTTPServiceName,
                                       IIPAddress?                                                     IPAddress                    = null,
                                       IPPort?                                                         TCPPort                      = null,
                                       I18NString?                                                     Description                  = null,

                                       Boolean                                                         RequireAuthentication        = true,
                                       Boolean                                                         DisableWebSocketPings        = false,
                                       TimeSpan?                                                       WebSocketPingEvery           = null,
                                       TimeSpan?                                                       SlowNetworkSimulationDelay   = null,

                                       Func<X509Certificate2>?                                         ServerCertificateSelector    = null,
                                       RemoteTLSClientCertificateValidationHandler<org.GraphDefined.Vanaheimr.Hermod.WebSocket.IWebSocketServer>?  ClientCertificateValidator   = null,
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
                                       Boolean                                                         AutoStart                    = false)

        {

            webSocketServer                                 = new WebSocketServer(
                                                                  IPAddress,
                                                                  TCPPort ?? IPPort.Parse(8000),
                                                                  HTTPServiceName,
                                                                  Description,

                                                                  SupportedEEBusWebSocketSubprotocols,
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
                                                                  false
                                                              );

            this.NetworkingNodeId                           = NetworkingNodeId;
            this.RequireAuthentication                      = RequireAuthentication;

            webSocketServer.OnValidateTCPConnection        += ValidateTCPConnection;
            webSocketServer.OnValidateWebSocketConnection  += ValidateWebSocketConnection;
            webSocketServer.OnNewWebSocketConnection       += ProcessNewWebSocketConnection;
            webSocketServer.OnCloseMessageReceived         += ProcessCloseMessage;

            webSocketServer.OnTextMessage                  += (timestamp,
                                                               server,
                                                               connection,
                                                               eventTrackingId,
                                                               requestTimestamp,
                                                               textMessage,
                                                               cancellationToken) => ProcessTextMessage(requestTimestamp,
                                                                                                        connection,
                                                                                                        textMessage,
                                                                                                        eventTrackingId,
                                                                                                        cancellationToken);

            webSocketServer.OnBinaryMessage                += (timestamp,
                                                               server,
                                                               connection,
                                                               eventTrackingId,
                                                               requestTimestamp,
                                                               textMessage,
                                                               cancellationToken) => ProcessBinaryMessage(requestTimestamp,
                                                                                                          connection,
                                                                                                          textMessage,
                                                                                                          eventTrackingId,
                                                                                                          cancellationToken);

            if (AutoStart)
                webSocketServer.Start();

        }

        #endregion


        #region AddOrUpdateHTTPBasicAuth(NetworkingNodeId, Password)

        /// <summary>
        /// Add the given HTTP Basic Authentication password for the given networking node.
        /// </summary>
        /// <param name="NetworkingNodeId">The unique identification of the networking node.</param>
        /// <param name="Password">The password of the charging station.</param>
        public void AddOrUpdateHTTPBasicAuth(NetworkingNode_Id  NetworkingNodeId,
                                             String             Password)
        {

            NetworkingNodeLogins.AddOrUpdate(NetworkingNodeId,
                                             Password,
                                             (chargingStationId, password) => Password);

        }

        #endregion

        #region RemoveHTTPBasicAuth     (NetworkingNodeId)

        /// <summary>
        /// Remove the given HTTP Basic Authentication for the given networking node.
        /// </summary>
        /// <param name="NetworkingNodeId">The unique identification of the networking node.</param>
        public Boolean RemoveHTTPBasicAuth(NetworkingNode_Id NetworkingNodeId)
        {

            if (NetworkingNodeLogins.ContainsKey(NetworkingNodeId))
                return NetworkingNodeLogins.TryRemove(NetworkingNodeId, out _);

            return true;

        }

        #endregion


        // Connection management...

        #region (protected) ValidateTCPConnection        (LogTimestamp, Server, Connection, EventTrackingId, CancellationToken)

        private Task<ConnectionFilterResponse> ValidateTCPConnection(DateTime                      LogTimestamp,
                                                                     org.GraphDefined.Vanaheimr.Hermod.WebSocket.IWebSocketServer              Server,
                                                                     System.Net.Sockets.TcpClient  Connection,
                                                                     EventTracking_Id              EventTrackingId,
                                                                     CancellationToken             CancellationToken)
        {

            return Task.FromResult(ConnectionFilterResponse.Accepted());

        }

        #endregion

        #region (protected) ValidateWebSocketConnection  (LogTimestamp, Server, Connection, EventTrackingId, CancellationToken)

        private Task<HTTPResponse?> ValidateWebSocketConnection(DateTime                   LogTimestamp,
                                                                org.GraphDefined.Vanaheimr.Hermod.WebSocket.IWebSocketServer           Server,
                                                                WebSocketServerConnection  Connection,
                                                                EventTracking_Id           EventTrackingId,
                                                                CancellationToken          CancellationToken)
        {

            #region Verify 'Sec-WebSocket-Protocol'...

            if (Connection.HTTPRequest?.SecWebSocketProtocol is null ||
                Connection.HTTPRequest?.SecWebSocketProtocol.Any() == false)
            {

                DebugX.Log($"{nameof(AOverlayWebSocketServer)} connection from {Connection.RemoteSocket}: Missing 'Sec-WebSocket-Protocol' HTTP header!");

                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder() {
                               HTTPStatusCode  = HTTPStatusCode.BadRequest,
                               Server          = webSocketServer.HTTPServiceName,
                               Date            = Timestamp.Now,
                               ContentType     = HTTPContentType.Application.JSON_UTF8,
                               Content         = JSONObject.Create(
                                                     new JProperty("description",
                                                     JSONObject.Create(
                                                         new JProperty("en", "Missing 'Sec-WebSocket-Protocol' HTTP header!")
                                                     ))).ToUTF8Bytes(),
                               Connection      = "close"
                           }.AsImmutable);

            }
            else if (!webSocketServer.SecWebSocketProtocols.Overlaps(Connection.HTTPRequest?.SecWebSocketProtocol ?? []))
            {

                var error = $"This WebSocket service only supports {(webSocketServer.SecWebSocketProtocols.Select(id => $"'{id}'").AggregateWith(", "))}!";

                DebugX.Log($"{nameof(AOverlayWebSocketServer)} connection from {Connection.RemoteSocket}: {error}");

                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder() {
                               HTTPStatusCode  = HTTPStatusCode.BadRequest,
                               Server          = webSocketServer.HTTPServiceName,
                               Date            = Timestamp.Now,
                               ContentType     = HTTPContentType.Application.JSON_UTF8,
                               Content         = JSONObject.Create(
                                                     new JProperty("description",
                                                         JSONObject.Create(
                                                             new JProperty("en", error)
                                                     ))).ToUTF8Bytes(),
                               Connection      = "close"
                           }.AsImmutable);

            }

            #endregion

            #region Verify HTTP Authentication

            if (RequireAuthentication)
            {

                if (Connection.HTTPRequest?.Authorization is HTTPBasicAuthentication basicAuthentication)
                {

                    if (NetworkingNodeLogins.TryGetValue(NetworkingNode_Id.Parse(basicAuthentication.Username), out var password) &&
                        basicAuthentication.Password.FixedTimeEquals(password))
                    {
                        DebugX.Log($"{nameof(AOverlayWebSocketServer)} connection from {Connection.RemoteSocket} using authorization: '{basicAuthentication.Username}' / '{basicAuthentication.Password}'");
                        return Task.FromResult<HTTPResponse?>(null);
                    }
                    else
                        DebugX.Log($"{nameof(AOverlayWebSocketServer)} connection from {Connection.RemoteSocket} invalid authorization: '{basicAuthentication.Username}' / '{basicAuthentication.Password}'!");

                }
                else
                    DebugX.Log($"{nameof(AOverlayWebSocketServer)} connection from {Connection.RemoteSocket} missing authorization!");

                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder() {
                               HTTPStatusCode  = HTTPStatusCode.Unauthorized,
                               Server          = webSocketServer.HTTPServiceName,
                               Date            = Timestamp.Now,
                               Connection      = "close"
                           }.AsImmutable);

            }

            #endregion

            return Task.FromResult<HTTPResponse?>(null);

        }

        #endregion

        #region (protected) ProcessNewWebSocketConnection(LogTimestamp, Server, Connection, SharedSubprotocols, EventTrackingId, CancellationToken)

        protected async Task ProcessNewWebSocketConnection(DateTime                   LogTimestamp,
                                                           org.GraphDefined.Vanaheimr.Hermod.WebSocket.IWebSocketServer           Server,
                                                           WebSocketServerConnection  Connection,
                                                           IEnumerable<String>        SharedSubprotocols,
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

            #region ...try to get the NetworkingNodeId from the HTTP request path suffix

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


            if (networkingNodeId.HasValue)
            {

                #region Store the NetworkingNodeId within the HTTP Web Socket connection

                Connection.TryAddCustomData(
                                networkingNodeId_WebSocketKey,
                                networkingNodeId.Value
                            );

                #endregion

                #region Register new Networking Node

                if (!connectedNetworkingNodes.TryAdd(networkingNodeId.Value,
                                                     new Tuple<WebSocketServerConnection, DateTime>(
                                                         Connection,
                                                         Timestamp.Now
                                                     )))
                {

                    DebugX.Log($"{nameof(AOverlayWebSocketServer)} Duplicate networking node '{networkingNodeId.Value}' detected: Trying to close old one!");

                    if (connectedNetworkingNodes.TryRemove(networkingNodeId.Value, out var oldConnection))
                    {
                        try
                        {
                            await oldConnection.Item1.Close(
                                      WebSocketFrame.ClosingStatusCode.NormalClosure,
                                      "Newer connection detected!",
                                      CancellationToken
                                  );
                        }
                        catch (Exception e)
                        {
                            DebugX.Log($"{nameof(AOverlayWebSocketServer)} Closing old HTTP Web Socket connection from {oldConnection.Item1.RemoteSocket} failed: {e.Message}");
                        }
                    }

                    connectedNetworkingNodes.TryAdd(networkingNodeId.Value,
                                                    new Tuple<WebSocketServerConnection, DateTime>(
                                                        Connection,
                                                        Timestamp.Now
                                                    ));

                }

                #endregion


                #region Send OnCSMSNewWebSocketConnection event

                //var onCSMSNewWebSocketConnection = OnCSMSNewWebSocketConnection;
                //if (onCSMSNewWebSocketConnection is not null)
                //{
                //    try
                //    {

                //        await Task.WhenAll(onCSMSNewWebSocketConnection.GetInvocationList().
                //                               OfType<OnCSMSNewWebSocketConnectionDelegate>().
                //                               Select(loggingDelegate => loggingDelegate.Invoke(
                //                                                             LogTimestamp,
                //                                                             this,
                //                                                             Connection,
                //                                                             networkingNodeId.Value,
                //                                                             SharedSubprotocols,
                //                                                             EventTrackingId,
                //                                                             CancellationToken
                //                                                         )).
                //                               ToArray());

                //    }
                //    catch (Exception e)
                //    {
                //        await HandleErrors(
                //                  nameof(AEEBusWebSocketServer),
                //                  nameof(OnCSMSNewWebSocketConnection),
                //                  e
                //              );
                //    }

                //}

                #endregion

            }

            #region Close connection

            else
            {

                DebugX.Log($"{nameof(AOverlayWebSocketServer)} Could not get NetworkingNodeId from HTTP Web Socket connection ({Connection.RemoteSocket}): Closing connection!");

                try
                {
                    await Connection.Close(
                              WebSocketFrame.ClosingStatusCode.PolicyViolation,
                              "Could not get NetworkingNodeId from HTTP Web Socket connection!",
                              CancellationToken
                          );
                }
                catch (Exception e)
                {
                    DebugX.Log($"{nameof(AOverlayWebSocketServer)} Closing HTTP Web Socket connection ({Connection.RemoteSocket}) failed: {e.Message}");
                }

            }

            #endregion

        }

        #endregion

        #region (protected) ProcessCloseMessage          (LogTimestamp, Server, Connection, Frame, EventTrackingId, StatusCode, Reason, CancellationToken)

        protected async Task ProcessCloseMessage(DateTime                          LogTimestamp,
                                                 org.GraphDefined.Vanaheimr.Hermod.WebSocket.IWebSocketServer                  Server,
                                                 WebSocketServerConnection         Connection,
                                                 WebSocketFrame                    Frame,
                                                 EventTracking_Id                  EventTrackingId,
                                                 WebSocketFrame.ClosingStatusCode  StatusCode,
                                                 String?                           Reason,
                                                 CancellationToken                 CancellationToken)
        {

            if (Connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey, out var networkingNodeId))
            {

                connectedNetworkingNodes.TryRemove(networkingNodeId, out _);

                #region Send OnCSMSCloseMessageReceived event

                //var logger = OnCSMSCloseMessageReceived;
                //if (logger is not null)
                //{

                //    var loggerTasks = logger.GetInvocationList().
                //                             OfType <OnCSMSCloseMessageReceivedDelegate>().
                //                             Select (loggingDelegate => loggingDelegate.Invoke(LogTimestamp,
                //                                                                               this,
                //                                                                               Connection,
                //                                                                               networkingNodeId,
                //                                                                               EventTrackingId,
                //                                                                               StatusCode,
                //                                                                               Reason,
                //                                                                               CancellationToken)).
                //                             ToArray();

                //    try
                //    {
                //        await Task.WhenAll(loggerTasks);
                //    }
                //    catch (Exception e)
                //    {
                //        await HandleErrors(
                //                  nameof(AEEBusWebSocketServer),
                //                  nameof(OnCSMSCloseMessageReceived),
                //                  e
                //              );
                //    }

                //}

                #endregion

            }

        }

        #endregion


        // Receive data...

        #region (protected) ProcessTextMessage           (RequestTimestamp, Connection, TextMessage,   EventTrackingId, CancellationToken)

        /// <summary>
        /// Process all text messages of this WebSocket API.
        /// </summary>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="Connection">The WebSocket connection.</param>
        /// <param name="TextMessage">The received text message.</param>
        /// <param name="EventTrackingId">An optional event tracking identification.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        public async Task<WebSocketTextMessageResponse> ProcessTextMessage(DateTime                   RequestTimestamp,
                                                                           WebSocketServerConnection  Connection,
                                                                           String                     TextMessage,
                                                                           EventTracking_Id           EventTrackingId,
                                                                           CancellationToken          CancellationToken)
        {

            try
            {

                var jsonArray     = JArray.Parse(TextMessage);
                var sourceNodeId  = Connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey);

                if      (JSONRequestMessage.     TryParse(jsonArray, out var jsonRequestMessage,  out var requestParsingError,  RequestTimestamp, null, EventTrackingId, sourceNodeId, CancellationToken))
                {

                    JSONResponseMessage?     jsonResponseMessage       = null;
                    JSONRequestErrorMessage? jsonRequestErrorMessage   = null;

                    #region OnJSONMessageRequestReceived

                    var onJSONMessageRequestReceived = OnJSONMessageRequestReceived;
                    if (onJSONMessageRequestReceived is not null)
                    {
                        try
                        {

                            await Task.WhenAll(onJSONMessageRequestReceived.GetInvocationList().
                                                   OfType <OnWebSocketJSONMessageRequestDelegate>().
                                                   Select (loggingDelegate => loggingDelegate.Invoke(
                                                                                  Timestamp.Now,
                                                                                  this,
                                                                                  Connection,
                                                                                  jsonRequestMessage.DestinationId,
                                                                                  jsonRequestMessage.NetworkPath,
                                                                                  EventTrackingId,
                                                                                  Timestamp.Now,
                                                                                  jsonArray,
                                                                                  CancellationToken
                                                                              )).
                                                   ToArray());

                        }
                        catch (Exception e)
                        {
                            DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnJSONMessageRequestReceived));
                        }
                    }

                    #endregion

                    #region Try to call the matching 'incoming message processor'...

                    if (incomingMessageProcessorsLookup.TryGetValue(jsonRequestMessage.Action, out var methodInfo) &&
                        methodInfo is not null)
                    {

                        var resultTask = methodInfo.Invoke(
                                             this,
                                             [
                                                 jsonRequestMessage.RequestTimestamp,
                                                 Connection,
                                                 jsonRequestMessage.DestinationId,
                                                 jsonRequestMessage.NetworkPath,
                                                 jsonRequestMessage.EventTrackingId,
                                                 jsonRequestMessage.RequestId,
                                                 jsonRequestMessage.Payload,
                                                 jsonRequestMessage.CancellationToken
                                             ]
                                         );

                        if (resultTask is Task<Tuple<JSONResponseMessage?, JSONRequestErrorMessage?>> textProcessor) {
                            (jsonResponseMessage, jsonRequestErrorMessage) = await textProcessor;
                        }

                        else
                            DebugX.Log($"Received undefined '{jsonRequestMessage.Action}' JSON request message handler within {nameof(AOverlayWebSocketServer)}!");

                        if (jsonResponseMessage is not null &&
                            jsonResponseMessage.NetworkingMode == NetworkingMode.Unknown &&
                           !connectedNetworkingNodes.ContainsKey(jsonResponseMessage.DestinationId))
                        {
                            //ToDo: Fix me!
                            //EEBusResponse.NetworkingMode = NetworkingMode.OverlayNetwork;
                        }

                    }

                    #endregion

                    #region ...or error!

                    else
                    {

                        DebugX.Log($"Received unknown '{jsonRequestMessage.Action}' JSON request message handler within {nameof(AOverlayWebSocketServer)}!");

                        jsonRequestErrorMessage = new JSONRequestErrorMessage(
                                                Timestamp.Now,
                                                EventTracking_Id.New,
                                                NetworkingMode.Unknown,
                                                NetworkingNode_Id.Zero,
                                                NetworkPath.Empty,
                                                jsonRequestMessage.RequestId,
                                                ResultCode.ProtocolError,
                                                $"The EEBus message '{jsonRequestMessage.Action}' is unkown!",
                                                new JObject(
                                                    new JProperty("request", TextMessage)
                                                )
                                            );

                    }

                    #endregion


                    #region OnJSONMessageResponseSent

                    if (jsonResponseMessage is not null)
                    {

                        var now = Timestamp.Now;

                        var onJSONMessageResponseSent = OnJSONMessageResponseSent;
                        if (onJSONMessageResponseSent is not null)
                        {
                            try
                            {

                                await Task.WhenAll(onJSONMessageResponseSent.GetInvocationList().
                                                       OfType <OnWebSocketJSONMessageResponseDelegate>().
                                                       Select (loggingDelegate => loggingDelegate.Invoke(
                                                                                      now,
                                                                                      this,
                                                                                      Connection,
                                                                                      jsonRequestMessage.DestinationId,
                                                                                      jsonRequestMessage.NetworkPath,
                                                                                      EventTrackingId,
                                                                                      RequestTimestamp,
                                                                                      jsonArray,
                                                                                      [],
                                                                                      now,
                                                                                      //ToDo: For some use cases returning an error to a charging station might be useless!
                                                                                      jsonResponseMessage.ToJSON(),
                                                                                      CancellationToken
                                                                                  )).
                                                       ToArray());

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnJSONMessageResponseSent));
                            }
                        }

                    }

                    #endregion

                    #region OnJSONErrorResponseSent

                    if (jsonRequestErrorMessage is not null)
                    {

                        var now = Timestamp.Now;

                        var onJSONErrorResponseSent = OnJSONErrorResponseSent;
                        if (onJSONErrorResponseSent is not null)
                        {
                            try
                            {

                                await Task.WhenAll(onJSONErrorResponseSent.GetInvocationList().
                                                       OfType <OnWebSocketTextErrorResponseDelegate>().
                                                       Select (loggingDelegate => loggingDelegate.Invoke(
                                                                                      now,
                                                                                      this,
                                                                                      Connection,
                                                                                      EventTrackingId,
                                                                                      RequestTimestamp,
                                                                                      TextMessage,
                                                                                      [],
                                                                                      now,
                                                                                      jsonRequestErrorMessage.ToJSON().ToString(JSONFormatting),
                                                                                      CancellationToken
                                                                                  )).
                                                       ToArray());

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnJSONErrorResponseSent));
                            }
                        }

                    }

                    #endregion


                    // The response... might be empty!
                    return new WebSocketTextMessageResponse(
                               RequestTimestamp,
                               TextMessage,
                               Timestamp.Now,
                               (jsonResponseMessage?.ToJSON() ?? jsonRequestErrorMessage?.ToJSON())?.ToString(JSONFormatting) ?? String.Empty,
                               EventTrackingId,
                               CancellationToken
                           );

                }

                else if (JSONResponseMessage.    TryParse(jsonArray, out var jsonResponseMessage, out var responseParsingError, sourceNodeId))
                {

                    if (requests.TryGetValue(jsonResponseMessage.RequestId, out var sendRequestState) &&
                        sendRequestState is not null)
                    {

                        sendRequestState.ResponseTimestamp  = Timestamp.Now;
                        sendRequestState.JSONResponse       = jsonResponseMessage;

                        #region OnJSONMessageResponseReceived

                        var onJSONMessageResponseReceived = OnJSONMessageResponseReceived;
                        if (onJSONMessageResponseReceived is not null)
                        {
                            try
                            {

                                await Task.WhenAll(onJSONMessageResponseReceived.GetInvocationList().
                                                       OfType <OnWebSocketJSONMessageResponseDelegate>().
                                                       Select (loggingDelegate => loggingDelegate.Invoke(
                                                                                      Timestamp.Now,
                                                                                      this,
                                                                                      Connection,
                                                                                      jsonResponseMessage.DestinationId,
                                                                                      jsonResponseMessage.NetworkPath,
                                                                                      EventTrackingId,
                                                                                      sendRequestState.RequestTimestamp,
                                                                                      sendRequestState.JSONRequest?.  ToJSON()      ?? [],
                                                                                      sendRequestState.BinaryRequest?.ToByteArray() ?? [],
                                                                                      Timestamp.Now,
                                                                                      sendRequestState.JSONResponse.  ToJSON(),
                                                                                      CancellationToken
                                                                                  )).
                                                       ToArray());

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnJSONMessageResponseReceived));
                            }
                        }

                        #endregion

                    }

                    else
                        DebugX.Log($"Received an unknown EEBus response with identificaiton '{jsonResponseMessage.RequestId}' within {nameof(AOverlayWebSocketServer)}:{Environment.NewLine}'{TextMessage}'!");

                    // No response to the charging station unteil OCPP v2.1!

                }

                else if (JSONRequestErrorMessage.TryParse(jsonArray, out var jsonRequestErrorMessage,                           sourceNodeId))
                {

                    if (requests.TryGetValue(jsonRequestErrorMessage.RequestId, out var sendRequestState) &&
                        sendRequestState is not null)
                    {

                        var errorCode = ResultCode.GenericError;

                        if (ResultCode.TryParse(jsonArray[2]?.Value<String>() ?? "", out var errorCode2))
                            errorCode = errorCode2;

                        sendRequestState.JSONResponse             = null;
                        sendRequestState.JSONRequestErrorMessage  = new JSONRequestErrorMessage(

                                                                        Timestamp.Now,
                                                                        jsonRequestErrorMessage.EventTrackingId,
                                                                        NetworkingMode.Unknown,
                                                                        jsonRequestErrorMessage.NetworkPath.Source,
                                                                        NetworkPath.From(NetworkingNodeId),
                                                                        jsonRequestErrorMessage.RequestId,

                                                                        ErrorCode:         errorCode,
                                                                        ErrorDescription:  jsonArray[3]?.Value<String>(),
                                                                        ErrorDetails:      jsonArray[4] as JObject

                                                                    );

                        #region OnJSONErrorResponseReceived

                        var onJSONErrorResponseReceived = OnJSONErrorResponseReceived;
                        if (onJSONErrorResponseReceived is not null)
                        {
                            try
                            {

                                await Task.WhenAll(onJSONErrorResponseReceived.GetInvocationList().
                                                       OfType <OnWebSocketTextErrorResponseDelegate>().
                                                       Select (loggingDelegate => loggingDelegate.Invoke(
                                                                                      Timestamp.Now,
                                                                                      this,
                                                                                      Connection,
                                                                                      EventTrackingId,
                                                                                      sendRequestState.RequestTimestamp,
                                                                                      sendRequestState.JSONRequest?.  ToJSON().ToString(JSONFormatting) ?? "",
                                                                                      sendRequestState.BinaryRequest?.ToByteArray()                     ?? [],
                                                                                      Timestamp.Now,
                                                                                      sendRequestState.JSONRequestErrorMessage?. ToString() ?? "",
                                                                                      CancellationToken
                                                                                  )).
                                                       ToArray());

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnJSONErrorResponseReceived));
                            }
                        }

                        #endregion

                    }

                    // No response to the charging station!

                }

                else if (requestParsingError  is not null)
                    DebugX.Log($"Failed to parse a JSON request message within {nameof(AOverlayWebSocketServer)}: '{requestParsingError}'{Environment.NewLine}'{TextMessage}'!");

                else if (responseParsingError is not null)
                    DebugX.Log($"Failed to parse a JSON response message within {nameof(AOverlayWebSocketServer)}: '{responseParsingError}'{Environment.NewLine}'{TextMessage}'!");

                else
                    DebugX.Log($"Received unknown text message within {nameof(AOverlayWebSocketServer)}: '{TextMessage}'!");

            }
            catch (Exception e)
            {

                var jsonRequestErrorMessage = JSONRequestErrorMessage.InternalError(
                                                  nameof(AOverlayWebSocketServer),
                                                  EventTrackingId,
                                                  TextMessage,
                                                  e
                                              );

                #region OnJSONErrorResponseSent

                var now = Timestamp.Now;

                var onJSONErrorResponseSent = OnJSONErrorResponseSent;
                if (onJSONErrorResponseSent is not null)
                {
                    try
                    {

                        await Task.WhenAll(onJSONErrorResponseSent.GetInvocationList().
                                               OfType <OnWebSocketTextErrorResponseDelegate>().
                                               Select (loggingDelegate => loggingDelegate.Invoke(
                                                                              now,
                                                                              this,
                                                                              Connection,
                                                                              EventTrackingId,
                                                                              RequestTimestamp,
                                                                              TextMessage,
                                                                              [],
                                                                              now,
                                                                              jsonRequestErrorMessage.ToJSON().ToString(JSONFormatting),
                                                                              CancellationToken
                                                                          )).
                                               ToArray());

                    }
                    catch (Exception e2)
                    {
                        DebugX.Log(e2, nameof(AOverlayWebSocketServer) + "." + nameof(OnJSONErrorResponseSent));
                    }
                }

                #endregion

            }

            // The response... is empty!
            return new WebSocketTextMessageResponse(
                       RequestTimestamp,
                       TextMessage,
                       Timestamp.Now,
                       String.Empty,
                       EventTrackingId,
                       CancellationToken
                   );

        }

        #endregion

        #region (protected) ProcessBinaryMessage         (RequestTimestamp, Connection, BinaryMessage, EventTrackingId, CancellationToken)

        /// <summary>
        /// Process all text messages of this WebSocket API.
        /// </summary>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="Connection">The WebSocket connection.</param>
        /// <param name="BinaryMessage">The received binary message.</param>
        /// <param name="EventTrackingId">An optional event tracking identification.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        public async Task<WebSocketBinaryMessageResponse> ProcessBinaryMessage(DateTime                   RequestTimestamp,
                                                                               WebSocketServerConnection  Connection,
                                                                               Byte[]                     BinaryMessage,
                                                                               EventTracking_Id           EventTrackingId,
                                                                               CancellationToken          CancellationToken)
        {

            try
            {

                var sourceNodeId = Connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey);

                     if (BinaryRequestMessage. TryParse(BinaryMessage, out var binaryRequestMessage,  out var requestParsingError,  RequestTimestamp, EventTrackingId, sourceNodeId, CancellationToken) && binaryRequestMessage  is not null)
                {

                    BinaryResponseMessage?   binaryResponseMessage     = null;
                    JSONRequestErrorMessage? jsonRequestErrorMessage   = null;

                    #region OnBinaryMessageRequestReceived

                    var onBinaryMessageRequestReceived = OnBinaryMessageRequestReceived;
                    if (onBinaryMessageRequestReceived is not null)
                    {
                        try
                        {

                            await Task.WhenAll(onBinaryMessageRequestReceived.GetInvocationList().
                                                   OfType <OnWebSocketBinaryMessageRequestDelegate>().
                                                   Select (loggingDelegate => loggingDelegate.Invoke(
                                                                                  Timestamp.Now,
                                                                                  this,
                                                                                  Connection,
                                                                                  binaryRequestMessage.DestinationId,
                                                                                  binaryRequestMessage.NetworkPath,
                                                                                  EventTrackingId,
                                                                                  Timestamp.Now,
                                                                                  BinaryMessage,
                                                                                  CancellationToken
                                                                              )).
                                                   ToArray());

                        }
                        catch (Exception e)
                        {
                            DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnBinaryMessageRequestReceived));
                        }
                    }

                    #endregion

                    #region Try to call the matching 'incoming message processor'

                    if (incomingMessageProcessorsLookup.TryGetValue(binaryRequestMessage.Action, out var methodInfo) &&
                        methodInfo is not null)
                    {

                        var result = methodInfo.Invoke(this,
                                                       [ binaryRequestMessage.RequestTimestamp,
                                                         Connection,
                                                         binaryRequestMessage.DestinationId,
                                                         binaryRequestMessage.NetworkPath,
                                                         binaryRequestMessage.EventTrackingId,
                                                         binaryRequestMessage.RequestId,
                                                         binaryRequestMessage.Payload,
                                                         binaryRequestMessage.CancellationToken ]);

                        if (result is Task<Tuple<BinaryResponseMessage?, JSONRequestErrorMessage?>> binaryProcessor)
                        {
                            (binaryResponseMessage, jsonRequestErrorMessage) = await binaryProcessor;
                        }

                        else
                            DebugX.Log($"Received undefined '{binaryRequestMessage.Action}' binary request message handler within {nameof(AOverlayWebSocketServer)}!");

                    }

                    #endregion

                    #region ...or error!

                    else
                    {

                        DebugX.Log($"Received unknown '{binaryRequestMessage.Action}' binary request message handler within {nameof(AOverlayWebSocketServer)}!");

                        jsonRequestErrorMessage = new JSONRequestErrorMessage(
                                                      Timestamp.Now,
                                                      EventTracking_Id.New,
                                                      NetworkingMode.Unknown,
                                                      NetworkingNode_Id.Zero,
                                                      NetworkPath.Empty,
                                                      binaryRequestMessage.RequestId,
                                                      ResultCode.ProtocolError,
                                                      $"The EEBus message '{binaryRequestMessage.Action}' is unkown!",
                                                      new JObject(
                                                          new JProperty("request", BinaryMessage.ToBase64())
                                                      )
                                                  );

                    }

                    #endregion


                    #region OnBinaryMessageResponseSent

                    if (binaryResponseMessage is not null)
                    {

                        var now = Timestamp.Now;

                        var onBinaryMessageResponseSent = OnBinaryMessageResponseSent;
                        if (onBinaryMessageResponseSent is not null)
                        {
                            try
                            {

                                await Task.WhenAll(onBinaryMessageResponseSent.GetInvocationList().
                                                       OfType <OnWebSocketBinaryMessageResponseDelegate>().
                                                       Select (loggingDelegate => loggingDelegate.Invoke(
                                                                                      now,
                                                                                      this,
                                                                                      Connection,
                                                                                      binaryRequestMessage.DestinationId,
                                                                                      binaryRequestMessage.NetworkPath,
                                                                                      EventTrackingId,
                                                                                      RequestTimestamp,
                                                                                      [],
                                                                                      BinaryMessage,
                                                                                      now,
                                                                                      //ToDo: For some use cases returning an error to a charging station might be useless!
                                                                                      binaryResponseMessage.ToByteArray(),
                                                                                      CancellationToken
                                                                                  )).
                                                       ToArray());

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnBinaryMessageResponseSent));
                            }
                        }

                    }

                    #endregion

                    #region OnJSONErrorResponseSent

                    if (jsonRequestErrorMessage is not null)
                    {

                        var now = Timestamp.Now;

                        var onJSONErrorResponseSent = OnJSONErrorResponseSent;
                        if (onJSONErrorResponseSent is not null)
                        {
                            try
                            {

                                await Task.WhenAll(onJSONErrorResponseSent.GetInvocationList().
                                                       OfType <OnWebSocketTextErrorResponseDelegate>().
                                                       Select (loggingDelegate => loggingDelegate.Invoke(
                                                                                      now,
                                                                                      this,
                                                                                      Connection,
                                                                                      EventTrackingId,
                                                                                      RequestTimestamp,
                                                                                      BinaryMessage.ToBase64(),
                                                                                      [],
                                                                                      now,
                                                                                      jsonRequestErrorMessage.ToJSON().ToString(JSONFormatting),
                                                                                      CancellationToken
                                                                                  )).
                                                       ToArray());

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnJSONErrorResponseSent));
                            }
                        }

                    }

                    #endregion


                    // The response... might be empty!
                    return new WebSocketBinaryMessageResponse(
                               RequestTimestamp,
                               BinaryMessage,
                               Timestamp.Now,
                               binaryResponseMessage?.ToByteArray() ?? [],
                               EventTrackingId,
                               CancellationToken
                           );

                }

                else if (BinaryResponseMessage.TryParse(BinaryMessage, out var binaryResponse, out var responseParsingError, sourceNodeId) && binaryResponse is not null)
                {

                    if (requests.TryGetValue(binaryResponse.RequestId, out var sendRequestState) &&
                        sendRequestState is not null)
                    {

                        #region Get LastHop

                        var lastHop            = Connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey) ?? NetworkingNode_Id.Zero;

                        if (binaryResponse.NetworkingMode == NetworkingMode.OverlayNetwork &&
                            binaryResponse.NetworkPath is not null &&
                            binaryResponse.NetworkPath.Last != NetworkingNode_Id.Zero)
                        {
                            lastHop            = binaryResponse.NetworkPath.Last;
                        }

                        #endregion

                        #region Append network path

                        // Note: Keep "var networkPath2", as "var networkPath" let to runtime errors!
                        var networkPath2        = binaryResponse.NetworkPath ?? NetworkPath.Empty;

                        if (networkPath2.Last != lastHop)
                            networkPath2        = networkPath2.Append(lastHop);

                        #endregion


                        sendRequestState.ResponseTimestamp  = Timestamp.Now;
                        sendRequestState.BinaryResponse     = binaryResponse;

                        #region OnBinaryMessageResponseReceived

                        var onBinaryMessageResponseReceived = OnBinaryMessageResponseReceived;
                        if (onBinaryMessageResponseReceived is not null)
                        {
                            try
                            {

                                await Task.WhenAll(onBinaryMessageResponseReceived.GetInvocationList().
                                                       OfType <OnWebSocketBinaryMessageResponseDelegate>().
                                                       Select (loggingDelegate => loggingDelegate.Invoke(
                                                                                      Timestamp.Now,
                                                                                      this,
                                                                                      Connection,
                                                                                      binaryResponse.DestinationId,
                                                                                      networkPath2,
                                                                                      EventTrackingId,
                                                                                      sendRequestState.RequestTimestamp,
                                                                                      sendRequestState.JSONRequest?.  ToJSON()      ?? [],
                                                                                      sendRequestState.BinaryRequest?.ToByteArray() ?? [],
                                                                                      sendRequestState.ResponseTimestamp.Value,
                                                                                      sendRequestState.BinaryResponse.ToByteArray(),
                                                                                      CancellationToken
                                                                                  )).
                                                       ToArray());

                            }
                            catch (Exception e)
                            {
                                DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnBinaryMessageResponseReceived));
                            }
                        }

                        #endregion

                    }

                    else
                        DebugX.Log(nameof(AOverlayWebSocketServer), " Received unknown binary EEBus response message!");

                }

                else if (requestParsingError  is not null)
                    DebugX.Log($"Failed to parse a binary request message within {nameof(AOverlayWebSocketServer)}: '{requestParsingError}'{Environment.NewLine}'{BinaryMessage.ToBase64()}'!");

                else if (responseParsingError is not null)
                    DebugX.Log($"Failed to parse a binary response message within {nameof(AOverlayWebSocketServer)}: '{responseParsingError}'{Environment.NewLine}'{BinaryMessage.ToBase64()}'!");

                else
                    DebugX.Log($"Received unknown binary message within {nameof(AOverlayWebSocketServer)}: '{BinaryMessage.ToBase64()}'!");

            }
            catch (Exception e)
            {

                var jsonRequestErrorMessage = JSONRequestErrorMessage.InternalError(
                                                  nameof(AOverlayWebSocketServer),
                                                  EventTrackingId,
                                                  BinaryMessage,
                                                  e
                                              );

                #region OnJSONErrorResponseSent

                var now = Timestamp.Now;

                var onJSONErrorResponseSent = OnJSONErrorResponseSent;
                if (onJSONErrorResponseSent is not null)
                {
                    try
                    {

                        await Task.WhenAll(onJSONErrorResponseSent.GetInvocationList().
                                               OfType <OnWebSocketTextErrorResponseDelegate>().
                                               Select (loggingDelegate => loggingDelegate.Invoke(
                                                                              now,
                                                                              this,
                                                                              Connection,
                                                                              EventTrackingId,
                                                                              RequestTimestamp,
                                                                              BinaryMessage.ToBase64(),
                                                                              [],
                                                                              now,
                                                                              jsonRequestErrorMessage.ToJSON().ToString(JSONFormatting),
                                                                              CancellationToken
                                                                          )).
                                               ToArray());

                    }
                    catch (Exception e2)
                    {
                        DebugX.Log(e2, nameof(AOverlayWebSocketServer) + "." + nameof(OnJSONErrorResponseSent));
                    }
                }

                #endregion

            }


            // The response... is empty!
            return new WebSocketBinaryMessageResponse(
                       RequestTimestamp,
                       BinaryMessage,
                       Timestamp.Now,
                       [],
                       EventTrackingId,
                       CancellationToken
                   );

            //return new WebSocketBinaryMessageResponse(
            //           RequestTimestamp,
            //           BinaryMessage,
            //           Timestamp.Now,
            //           EEBusResponse?.ToByteArray() ?? [],
            //           EventTrackingId
            //       );

        }

        #endregion



        private IEnumerable<Tuple<WebSocketServerConnection, NetworkingMode>> LookupNetworkingNode(NetworkingNode_Id NetworkingNodeId)
        {

            if (NetworkingNodeId == NetworkingNode_Id.Zero)
                return [];

            var lookUpNetworkingNodeId = NetworkingNodeId;

            if (reachableViaNetworkingHubs.TryGetValue(lookUpNetworkingNodeId, out var networkingHubId))
            {
                lookUpNetworkingNodeId = networkingHubId;
                return webSocketServer.WebSocketConnections.Where(connection => connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey) == lookUpNetworkingNodeId).
                    Select(x => new Tuple<WebSocketServerConnection, NetworkingMode>(x, NetworkingMode.OverlayNetwork));
            }

            return webSocketServer.WebSocketConnections.Where (connection => connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey) == lookUpNetworkingNodeId).
                                                        Select(x => new Tuple<WebSocketServerConnection, NetworkingMode>(x, NetworkingMode.Standard));

        }

        public void AddStaticRouting(NetworkingNode_Id DestinationNodeId,
                                     NetworkingNode_Id NetworkingHubId)
        {

            reachableViaNetworkingHubs.TryAdd(DestinationNodeId,
                                              NetworkingHubId);

        }

        public void RemoveStaticRouting(NetworkingNode_Id DestinationNodeId,
                                        NetworkingNode_Id NetworkingHubId)
        {

            reachableViaNetworkingHubs.TryRemove(new KeyValuePair<NetworkingNode_Id, NetworkingNode_Id>(DestinationNodeId, NetworkingHubId));

        }



        // Send data...

        #region SendJSONData      (EventTrackingId, DestinationNodeId, NetworkPath, RequestId, Action, JSONData,   RequestTimeout, ...)

        /// <summary>
        /// Send (and forget) the given JSON.
        /// </summary>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="DestinationNodeId">The networking node identification of the message destination.</param>
        /// <param name="NetworkPath">The network path.</param>
        /// <param name="RequestId">A unique request identification.</param>
        /// <param name="Action">An EEBus action.</param>
        /// <param name="JSONData">The JSON payload.</param>
        /// <param name="RequestTimeout">A request timeout.</param>
        public async Task<SendWebSocketMessageResult> SendJSONData(EventTracking_Id   EventTrackingId,
                                                               NetworkingNode_Id  DestinationNodeId,
                                                               NetworkPath        NetworkPath,
                                                               Request_Id         RequestId,
                                                               String             Action,
                                                               JObject            JSONData,
                                                               DateTime           RequestTimeout,
                                                               CancellationToken  CancellationToken   = default)
        {

            try
            {

                var webSocketConnections  = LookupNetworkingNode(DestinationNodeId).ToArray();

                if (webSocketConnections.Length != 0)
                {

                    var networkingMode      = webSocketConnections.First().Item1.TryGetCustomDataAs<NetworkingMode>(networkingMode_WebSocketKey);

                    var jsonRequestMessage  = new JSONRequestMessage(
                                                  Timestamp.Now,
                                                  EventTracking_Id.New,
                                                  webSocketConnections.First().Item2,
                                                  DestinationNodeId,
                                                  NetworkPath,
                                                  RequestId,
                                                  Action,
                                                  JSONData
                                              );

                    var ocppTextMessage     = jsonRequestMessage.ToJSON().ToString(Formatting.None);


                    foreach (var webSocketConnection in webSocketConnections)
                    {

                        if (SentStatus.Success == await webSocketServer.SendTextMessage(
                                                            webSocketConnection.Item1,
                                                            ocppTextMessage,
                                                            EventTrackingId,
                                                            CancellationToken
                                                        ))
                        {

                            requests.TryAdd(RequestId,
                                            SendRequestState.FromJSONRequest(
                                                Timestamp.Now,
                                                DestinationNodeId,
                                                RequestTimeout,
                                                jsonRequestMessage
                                            ));

                            #region OnJSONMessageRequestSent

                            //var onJSONMessageRequestSent = OnJSONMessageRequestSent;
                            //if (onJSONMessageRequestSent is not null)
                            //{
                            //    try
                            //    {

                            //        await Task.WhenAll(onJSONMessageRequestSent.GetInvocationList().
                            //                               OfType<OnWebSocketJSONMessageRequestDelegate>().
                            //                               Select(loggingDelegate => loggingDelegate.Invoke(
                            //                                                              Timestamp.Now,
                            //                                                              webSocketServer,
                            //                                                              webSocketConnection.Item1,
                            //                                                              NetworkingNodeId,
                            //                                                              NetworkPath,
                            //                                                              EventTrackingId,
                            //                                                              Timestamp.Now,
                            //                                                              ocppTextMessage,
                            //                                                              CancellationToken
                            //                                                          )).
                            //                               ToArray());

                            //    }
                            //    catch (Exception e)
                            //    {
                            //        DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnJSONMessageRequestSent));
                            //    }
                            //}

                            #endregion

                            break;

                        }

                        webSocketServer.RemoveConnection(webSocketConnection.Item1);

                    }

                    return SendWebSocketMessageResult.Success;

                }
                else
                    return SendWebSocketMessageResult.UnknownClient;

            }
            catch (Exception)
            {
                return SendWebSocketMessageResult.TransmissionFailed;
            }

        }

        #endregion

        #region SendBinaryData    (EventTrackingId, DestinationNodeId, NetworkPath, RequestId, Action, BinaryData, RequestTimeout, ...)

        /// <summary>
        /// Send (and forget) the given binary data.
        /// </summary>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="DestinationNodeId">The networking node identification of the message destination.</param>
        /// <param name="NetworkPath">The network path.</param>
        /// <param name="RequestId">A unique request identification.</param>
        /// <param name="Action">An EEBus action.</param>
        /// <param name="BinaryData">The binary payload.</param>
        /// <param name="RequestTimeout">A request timeout.</param>
        public async Task<SendWebSocketMessageResult> SendBinaryData(EventTracking_Id   EventTrackingId,
                                                                 NetworkingNode_Id  DestinationNodeId,
                                                                 NetworkPath        NetworkPath,
                                                                 Request_Id         RequestId,
                                                                 String             Action,
                                                                 Byte[]             BinaryData,
                                                                 DateTime           RequestTimeout,
                                                                 CancellationToken  CancellationToken   = default)
        {

            try
            {

                var webSocketConnections = LookupNetworkingNode(DestinationNodeId).ToArray();

                if (webSocketConnections.Length != 0)
                {

                    var networkingMode        = webSocketConnections.First().Item1.TryGetCustomDataAs<NetworkingMode>(networkingMode_WebSocketKey);

                    var binaryRequestMessage  = new BinaryRequestMessage(
                                                    Timestamp.Now,
                                                    EventTracking_Id.New,
                                                    webSocketConnections.First().Item2,
                                                    DestinationNodeId,
                                                    NetworkPath,
                                                    RequestId,
                                                    Action,
                                                    BinaryData
                                                );

                    requests.TryAdd(RequestId,
                                    SendRequestState.FromBinaryRequest(
                                        Timestamp.Now,
                                        DestinationNodeId,
                                        RequestTimeout,
                                        binaryRequestMessage
                                    ));

                    var ocppBinaryMessage     = binaryRequestMessage.ToByteArray();

                    foreach (var webSocketConnection in webSocketConnections)
                    {

                        if (SentStatus.Success == await webSocketServer.SendBinaryMessage(
                                                            webSocketConnection.Item1,
                                                            ocppBinaryMessage,
                                                            EventTrackingId,
                                                            CancellationToken
                                                        ))
                        {

                            #region OnBinaryMessageRequestSent

                            //var requestLogger = OnBinaryMessageRequestSent;
                            //if (requestLogger is not null)
                            //{

                            //    var loggerTasks = requestLogger.GetInvocationList().
                            //                                    OfType <OnWebSocketBinaryMessageDelegate>().
                            //                                    Select (loggingDelegate => loggingDelegate.Invoke(Timestamp.Now,
                            //                                                                                      webSocketServer,
                            //                                                                                      webSocketConnection.Item1,
                            //                                                                                      EventTrackingId,
                            //                                                                                      ocppBinaryMessage,
                            //                                                                                      CancellationToken)).
                            //                                    ToArray();

                            //    try
                            //    {
                            //        await Task.WhenAll(loggerTasks);
                            //    }
                            //    catch (Exception e)
                            //    {
                            //        DebugX.Log(e, nameof(AOverlayWebSocketServer) + "." + nameof(OnBinaryMessageRequestSent));
                            //    }

                            //}

                            #endregion

                            break;

                        }

                        webSocketServer.RemoveConnection(webSocketConnection.Item1);

                    }

                    return SendWebSocketMessageResult.Success;

                }
                else
                    return SendWebSocketMessageResult.UnknownClient;

            }
            catch (Exception)
            {
                return SendWebSocketMessageResult.TransmissionFailed;
            }

        }

        #endregion


        #region SendJSONAndWait   (EventTrackingId, DestinationNodeId, NetworkPath, RequestId, EEBusAction, JSONPayload,   RequestTimeout = null)

        public async Task<SendRequestState> SendJSONAndWait(EventTracking_Id   EventTrackingId,
                                                            NetworkingNode_Id  DestinationNodeId,
                                                            NetworkPath        NetworkPath,
                                                            Request_Id         RequestId,
                                                            String             EEBusAction,
                                                            JObject            JSONPayload,
                                                            TimeSpan?          RequestTimeout,
                                                            CancellationToken  CancellationToken   = default)
        {

            var endTime         = Timestamp.Now + (RequestTimeout ?? this.RequestTimeout ?? DefaultRequestTimeout);

            var sendJSONResult  = await SendJSONData(
                                            EventTrackingId,
                                            DestinationNodeId,
                                            NetworkPath,
                                            RequestId,
                                            EEBusAction,
                                            JSONPayload,
                                            endTime,
                                            CancellationToken
                                        );

            if (sendJSONResult == SendWebSocketMessageResult.Success) {

                #region Wait for a response... till timeout

                do
                {

                    try
                    {

                        await Task.Delay(25, CancellationToken);

                        if (requests.TryGetValue(RequestId, out var sendRequestState) &&
                           (sendRequestState?.JSONResponse   is not null ||
                            sendRequestState?.BinaryResponse is not null ||
                            sendRequestState?.HasErrors == true))
                        {

                            requests.TryRemove(RequestId, out _);

                            return sendRequestState;

                        }

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(String.Concat(nameof(AOverlayWebSocketServer), ".", nameof(SendJSONAndWait), " exception occured: ", e.Message));
                    }

                }
                while (Timestamp.Now < endTime);

                #endregion

                #region When timeout...

                if (requests.TryGetValue(RequestId, out var sendRequestState2) &&
                    sendRequestState2 is not null)
                {

                    sendRequestState2.JSONRequestErrorMessage  = new JSONRequestErrorMessage(

                                                                     Timestamp.Now,
                                                                     EventTrackingId,
                                                                     NetworkingMode.Unknown,
                                                                     NetworkPath.Source,
                                                                     NetworkPath.From(NetworkingNodeId),
                                                                     RequestId,

                                                                     ErrorCode: ResultCode.Timeout

                                                                 );

                    requests.TryRemove(RequestId, out _);

                    return sendRequestState2;

                }

                #endregion

            }

            #region ..., or client/network error(s)

            else
            {
                if (requests.TryGetValue(RequestId, out var sendRequestState3) &&
                    sendRequestState3 is not null)
                {

                    sendRequestState3.JSONRequestErrorMessage =  new JSONRequestErrorMessage(

                                                                     Timestamp.Now,
                                                                     EventTrackingId,
                                                                     NetworkingMode.Unknown,
                                                                     NetworkPath.Source,
                                                                     NetworkPath.From(NetworkingNodeId),
                                                                     RequestId,

                                                                     ErrorCode: ResultCode.Timeout

                                                                 );

                    requests.TryRemove(RequestId, out _);

                    return sendRequestState3;

                }
            }

            #endregion


            // Just in case...
            var now = Timestamp.Now;

            return SendRequestState.FromJSONRequest(

                       now,
                       DestinationNodeId,
                       now,
                       new JSONRequestMessage(
                           Timestamp.Now,
                           EventTracking_Id.New,
                           NetworkingMode.Standard,
                           DestinationNodeId,
                           NetworkPath,
                           RequestId,
                           EEBusAction,
                           JSONPayload
                       ),
                       now,
                       null,
                       null,
                       new JSONRequestErrorMessage(

                           Timestamp.Now,
                           EventTrackingId,
                           NetworkingMode.Unknown,
                           NetworkPath.Source,
                           NetworkPath.From(NetworkingNodeId),
                           RequestId,

                           ErrorCode: ResultCode.InternalError

                       )

                   );

        }

        #endregion

        #region SendBinaryAndWait (EventTrackingId, NetworkingNodeId, NetworkPath, RequestId, EEBusAction, BinaryPayload, RequestTimeout = null)

        public async Task<SendRequestState> SendBinaryAndWait(EventTracking_Id    EventTrackingId,
                                                              NetworkingNode_Id   NetworkingNodeId,
                                                              NetworkPath         NetworkPath,
                                                              Request_Id          RequestId,
                                                              String              EEBusAction,
                                                              Byte[]              BinaryPayload,
                                                              TimeSpan?           RequestTimeout,
                                                              CancellationToken   CancellationToken   = default)
        {

            var endTime         = Timestamp.Now + (RequestTimeout ?? this.RequestTimeout ?? DefaultRequestTimeout);

            var sendJSONResult  = await SendBinaryData(
                                            EventTrackingId,
                                            NetworkingNodeId,
                                            NetworkPath,
                                            RequestId,
                                            EEBusAction,
                                            BinaryPayload,
                                            endTime,
                                            CancellationToken
                                        );

            if (sendJSONResult == SendWebSocketMessageResult.Success) {

                #region Wait for a response... till timeout

                do
                {

                    try
                    {

                        await Task.Delay(25, CancellationToken);

                        if (requests.TryGetValue(RequestId, out var sendRequestState) &&
                           (sendRequestState?.JSONResponse   is not null ||
                            sendRequestState?.BinaryResponse is not null ||
                            sendRequestState?.HasErrors == true))
                        {

                            requests.TryRemove(RequestId, out _);

                            return sendRequestState;

                        }

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(String.Concat(nameof(AOverlayWebSocketServer), ".", nameof(SendJSONAndWait), " exception occured: ", e.Message));
                    }

                }
                while (Timestamp.Now < endTime);

                #endregion

                #region When timeout...

                if (requests.TryGetValue(RequestId, out var sendRequestState2) &&
                    sendRequestState2 is not null)
                {

                    sendRequestState2.JSONRequestErrorMessage =  new JSONRequestErrorMessage(

                                                                     Timestamp.Now,
                                                                     EventTrackingId,
                                                                     NetworkingMode.Unknown,
                                                                     NetworkPath.Source,
                                                                     NetworkPath.From(NetworkingNodeId),
                                                                     RequestId,

                                                                     ErrorCode: ResultCode.Timeout

                                                                 );

                    requests.TryRemove(RequestId, out _);

                    return sendRequestState2;

                }

                #endregion

            }

            #region ..., or client/network error(s)

            else
            {
                if (requests.TryGetValue(RequestId, out var sendRequestState3) &&
                    sendRequestState3 is not null)
                {

                    sendRequestState3.JSONRequestErrorMessage =  new JSONRequestErrorMessage(

                                                                     Timestamp.Now,
                                                                     EventTrackingId,
                                                                     NetworkingMode.Unknown,
                                                                     NetworkPath.Source,
                                                                     NetworkPath.From(NetworkingNodeId),
                                                                     RequestId,

                                                                     ErrorCode: ResultCode.Timeout

                                                                 );

                    requests.TryRemove(RequestId, out _);

                    return sendRequestState3;

                }
            }

            #endregion


            // Just in case...
            var now = Timestamp.Now;

            return SendRequestState.FromBinaryRequest(

                       now,
                       NetworkingNodeId,
                       now,
                       new BinaryRequestMessage(
                           Timestamp.Now,
                           EventTracking_Id.New,
                           NetworkingMode.Standard,
                           NetworkingNodeId,
                           NetworkPath,
                           RequestId,
                           EEBusAction,
                           BinaryPayload
                       ),
                       now,
                       null,
                       null,
                       new JSONRequestErrorMessage(

                           Timestamp.Now,
                           EventTrackingId,
                           NetworkingMode.Unknown,
                           NetworkPath.Source,
                           NetworkPath.From(NetworkingNodeId),
                           RequestId,

                           ErrorCode: ResultCode.InternalError

                       )

                   );

        }

        #endregion



        #region (protected) HandleErrors(Module, Caller, Exception, Description = null)
        protected Task HandleErrors(String     Module,
                                    String     Caller,
                                    Exception  Exception,
                                    String?    Description   = null)
        {

            DebugX.LogException(Exception, $"{Module}.{Caller}{(Description is not null ? $" {Description}" : "")}");

            return Task.CompletedTask;

        }

        #endregion



        #region Start()

        /// <summary>
        /// Start the HTTP web socket listener thread.
        /// </summary>
        public void Start()
        {
            webSocketServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        /// <summary>
        /// Shutdown the HTTP web socket listener thread.
        /// </summary>
        /// <param name="Message">An optional shutdown message.</param>
        /// <param name="Wait">Wait until the server finally shutted down.</param>
        public Task Shutdown(String?  Message   = null,
                             Boolean  Wait      = true)

            => webSocketServer.Shutdown(
                                   Message,
                                   Wait
                               );

        #endregion


    }

}
