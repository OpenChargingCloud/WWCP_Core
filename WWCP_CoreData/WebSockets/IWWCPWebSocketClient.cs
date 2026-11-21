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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;
using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP.WebSockets
{

    public delegate Task OnWebSocketClientJSONMessageSentDelegate       (DateTimeOffset              Timestamp,
                                                                         IWWCPWebSocketClient        Client,
                                                                         WebSocketClientConnection   Connection,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         DateTimeOffset              MessageTimestamp,
                                                                         JArray                      JSONMessage,
                                                                         SentStatus                  SentStatus,
                                                                         CancellationToken           CancellationToken);

    public delegate Task OnWebSocketClientJSONMessageReceivedDelegate   (DateTimeOffset              Timestamp,
                                                                         IWWCPWebSocketClient        Client,
                                                                         WebSocketClientConnection   Connection,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         DateTimeOffset              MessageTimestamp,
                                                                         NetworkingNode_Id           SourceNodeId,
                                                                         JArray                      JSONMessage,
                                                                         CancellationToken           CancellationToken);


    public delegate Task OnWebSocketClientCBORMessageSentDelegate       (DateTimeOffset              Timestamp,
                                                                         IWWCPWebSocketClient        Client,
                                                                         WebSocketClientConnection   Connection,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         DateTimeOffset              MessageTimestamp,
                                                                         CBORValue                   Message,
                                                                         SentStatus                  SentStatus,
                                                                         CancellationToken           CancellationToken);

    public delegate Task OnWebSocketClientCBORMessageReceivedDelegate   (DateTimeOffset              Timestamp,
                                                                         IWWCPWebSocketClient        Client,
                                                                         WebSocketClientConnection   Connection,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         DateTimeOffset              MessageTimestamp,
                                                                         NetworkingNode_Id           SourceNodeId,
                                                                         CBORValue                   Message,
                                                                         CancellationToken           CancellationToken);


    /// <summary>
    /// The common interface of all WWCP HTTP WebSocket clients.
    /// </summary>
    public interface IWWCPWebSocketClient : IWebSocketClient,
                                            IWWCPClient
    {


        /// <summary>
        /// An event sent whenever a JSON message was sent.
        /// </summary>
        event     OnWebSocketClientJSONMessageSentDelegate?         OnJSONMessageSent;

        /// <summary>
        /// An event sent whenever a JSON message was received.
        /// </summary>
        event     OnWebSocketClientJSONMessageReceivedDelegate?     OnJSONMessageReceived;


        /// <summary>
        /// Whether the binary frames of a connection that agreed on the given subprotocol are
        /// CBOR: told of as CBOR messages, not as binary messages.
        /// </summary>
        Func<String?, Boolean>                                      SubprotocolIsCBOR    { get; set; }


        /// <summary>
        /// An event sent whenever a CBOR message was sent.
        /// </summary>
        event     OnWebSocketClientCBORMessageSentDelegate?         OnCBORMessageSent;

        /// <summary>
        /// An event sent whenever a CBOR message was received.
        /// </summary>
        event     OnWebSocketClientCBORMessageReceivedDelegate?     OnCBORMessageReceived;


        //Task ProcessWebSocketTextFrame  (DateTimeOffset             RequestTimestamp,
        //                                 WebSocketClientConnection  ClientConnection,
        //                                 EventTracking_Id           EventTrackingId,
        //                                 String                     TextMessage,
        //                                 CancellationToken          CancellationToken);
        //Task ProcessWebSocketBinaryFrame(DateTimeOffset             RequestTimestamp,
        //                                 WebSocketClientConnection  ClientConnection,
        //                                 EventTracking_Id           EventTrackingId,
        //                                 Byte[]                     BinaryMessage,
        //                                 CancellationToken          CancellationToken);


        //Task<SentMessageResult> SendJSONRequest         (WWCP_JSONRequestMessage          JSONRequestMessage);
        //Task<SentMessageResult> SendJSONResponse        (WWCP_JSONResponseMessage         JSONResponseMessage);
        //Task<SentMessageResult> SendJSONRequestError    (WWCP_JSONRequestErrorMessage     JSONRequestErrorMessage);
        //Task<SentMessageResult> SendJSONResponseError   (WWCP_JSONResponseErrorMessage    JSONResponseErrorMessage);
        //Task<SentMessageResult> SendJSONSendMessage     (WWCP_JSONSendMessage             JSONSendMessage);


        //Task<SentMessageResult> SendBinaryRequest       (WWCP_BinaryRequestMessage        BinaryRequestMessage);
        //Task<SentMessageResult> SendBinaryResponse      (WWCP_BinaryResponseMessage       BinaryResponseMessage);
        //Task<SentMessageResult> SendBinaryRequestError  (WWCP_BinaryRequestErrorMessage   BinaryRequestErrorMessage);
        //Task<SentMessageResult> SendBinaryResponseError (WWCP_BinaryResponseErrorMessage  BinaryResponseErrorMessage);
        //Task<SentMessageResult> SendBinarySendMessage   (WWCP_BinarySendMessage           BinarySendMessage);


    }

}
