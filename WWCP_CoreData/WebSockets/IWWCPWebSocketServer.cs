/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
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


    public delegate Task OnWebSocketServerCBORMessageSentDelegate       (DateTimeOffset              Timestamp,
                                                                         IWWCPWebSocketServer        Server,
                                                                         WebSocketServerConnection   Connection,
                                                                         DateTimeOffset              MessageTimestamp,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         CBORValue                   Message,
                                                                         SentStatus                  SentStatus,
                                                                         CancellationToken           CancellationToken);

    public delegate Task OnWebSocketServerCBORMessageReceivedDelegate   (DateTimeOffset              Timestamp,
                                                                         IWWCPWebSocketServer        Server,
                                                                         WebSocketServerConnection   Connection,
                                                                         DateTimeOffset              MessageTimestamp,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         NetworkingNode_Id           SourceNodeId,
                                                                         CBORValue                   Message,
                                                                         CancellationToken           CancellationToken);


    /// <summary>
    /// The common interface of all WWCP HTTP WebSocket servers.
    /// </summary>
    public interface IWWCPWebSocketServer : IWebSocketServer,
                                            IWWCPServer
    {

        #region Properties

        /// <summary>
        /// The enumeration of all connected networking nodes.
        /// </summary>
        IEnumerable<NetworkingNodeConnections>  ConnectedNetworkingNodes      { get; }

        /// <summary>
        /// The request timeout for messages sent by this HTTP WebSocket server.
        /// </summary>
        TimeSpan?                               RequestTimeout                { get; set; }

        /// <summary>
        /// The JSON formatting to use.
        /// </summary>
        Formatting                              JSONFormatting                { get; set; }

        /// <summary>
        /// Whether the binary frames of a connection that agreed on the given subprotocol are
        /// CBOR: told of as CBOR messages, not as binary messages.
        /// </summary>
        Func<String?, Boolean>                  SubprotocolIsCBOR             { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever the connection of a networking node has been
        /// accepted and registered, before it is answered with 101 Switching
        /// Protocols - so that the networking node can be routed to by the time
        /// it knows it is connected.
        /// </summary>
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



        /// <summary>
        /// An event sent whenever a JSON message was sent.
        /// </summary>
        event     OnWebSocketServerJSONMessageSentDelegate?           OnJSONMessageSent;

        /// <summary>
        /// An event sent whenever a JSON message was received.
        /// </summary>
        event     OnWebSocketServerJSONMessageReceivedDelegate?       OnJSONMessageReceived;


        /// <summary>
        /// An event sent whenever a CBOR message was sent.
        /// </summary>
        event     OnWebSocketServerCBORMessageSentDelegate?           OnCBORMessageSent;

        /// <summary>
        /// An event sent whenever a CBOR message was received.
        /// </summary>
        event     OnWebSocketServerCBORMessageReceivedDelegate?       OnCBORMessageReceived;

        #endregion


        HTTPBasicAuthentication  AddOrUpdateHTTPBasicAuth (NetworkingNode_Id DestinationId, String            Password);
        //void                     AddStaticRouting         (NetworkingNode_Id DestinationId, NetworkingNode_Id NetworkingHubId);
        Boolean                  RemoveHTTPBasicAuth      (NetworkingNode_Id DestinationId);
        //void                     RemoveStaticRouting      (NetworkingNode_Id DestinationId, NetworkingNode_Id NetworkingHubId);


    }

}
