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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP.WebSockets
{

    public delegate Task OnWebSocketServerJSONMessageSentDelegate       (DateTimeOffset              Timestamp,
                                                                         IWWCPWebSocketServer        Server,
                                                                         WebSocketServerConnection   Connection,
                                                                         DateTimeOffset              MessageTimestamp,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         JArray                      Message,
                                                                         SentStatus                  SentStatus,
                                                                         CancellationToken           CancellationToken);

    public delegate Task OnWebSocketServerJSONMessageReceivedDelegate   (DateTimeOffset              Timestamp,
                                                                         IWWCPWebSocketServer        Server,
                                                                         WebSocketServerConnection   Connection,
                                                                         DateTimeOffset              MessageTimestamp,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         NetworkingNode_Id           SourceNodeId,
                                                                         JArray                      Message,
                                                                         CancellationToken           CancellationToken);


    public delegate Task OnWebSocketServerBinaryMessageSentDelegate     (DateTimeOffset              Timestamp,
                                                                         IWWCPWebSocketServer        Server,
                                                                         WebSocketServerConnection   Connection,
                                                                         DateTimeOffset              MessageTimestamp,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         Byte[]                      BinaryMessage,
                                                                         SentStatus                  SentStatus,
                                                                         CancellationToken           CancellationToken);

    public delegate Task OnWebSocketServerBinaryMessageReceivedDelegate (DateTimeOffset              Timestamp,
                                                                         IWWCPWebSocketServer        Server,
                                                                         WebSocketServerConnection   Connection,
                                                                         DateTimeOffset              MessageTimestamp,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         NetworkingNode_Id           SourceNodeId,
                                                                         Byte[]                      BinaryMessage,
                                                                         CancellationToken           CancellationToken);


    /// <summary>
    /// The common interface of all WWCP HTTP WebSocket servers.
    /// </summary>
    public interface IWWCPWebSocketServer : IWebSocketServer
    {

        #region Properties

        /// <summary>
        /// The parent networking node.
        /// </summary>
        INetworkingNode                         NetworkingNode                { get; }

        /// <summary>
        /// The enumeration of all connected networking nodes.
        /// </summary>
        IEnumerable<NetworkingNodeConnections>  ConnectedNetworkingNodes      { get; }

        /// <summary>
        /// The enumeration of all connected networking node identifications.
        /// </summary>
        IEnumerable<NetworkingNode_Id>          ConnectedNetworkingNodeIds    { get; }

        /// <summary>
        /// The request timeout for messages sent by this HTTP WebSocket server.
        /// </summary>
        TimeSpan?                               RequestTimeout                { get; set; }

        /// <summary>
        /// The JSON formatting to use.
        /// </summary>
        Formatting                              JSONFormatting                { get; set; }

        #endregion

        #region Events

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



        /// <summary>
        /// An event sent whenever a JSON message was sent.
        /// </summary>
        event     OnWebSocketServerJSONMessageSentDelegate?           OnJSONMessageSent;

        /// <summary>
        /// An event sent whenever a JSON message was received.
        /// </summary>
        event     OnWebSocketServerJSONMessageReceivedDelegate?       OnJSONMessageReceived;


        /// <summary>
        /// An event sent whenever a binary message was sent.
        /// </summary>
        new event OnWebSocketServerBinaryMessageSentDelegate?         OnBinaryMessageSent;

        /// <summary>
        /// An event sent whenever a binary message was received.
        /// </summary>
        new event OnWebSocketServerBinaryMessageReceivedDelegate?     OnBinaryMessageReceived;

        #endregion


        HTTPBasicAuthentication  AddOrUpdateHTTPBasicAuth (NetworkingNode_Id DestinationId, String            Password);
        //void                     AddStaticRouting         (NetworkingNode_Id DestinationId, NetworkingNode_Id NetworkingHubId);
        Boolean                  RemoveHTTPBasicAuth      (NetworkingNode_Id DestinationId);
        //void                     RemoveStaticRouting      (NetworkingNode_Id DestinationId, NetworkingNode_Id NetworkingHubId);


    }

}
