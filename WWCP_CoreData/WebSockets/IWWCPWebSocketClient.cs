/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;
using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP.WebSockets
{

    public delegate Task OnWebSocketClientJSONMessageSentDelegate       (DateTime                    Timestamp,
                                                                         IWWCPWebSocketClient        Client,
                                                                         WebSocketClientConnection   Connection,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         DateTime                    MessageTimestamp,
                                                                         JArray                      JSONMessage,
                                                                         SentStatus                  SentStatus,
                                                                         CancellationToken           CancellationToken);

    public delegate Task OnWebSocketClientJSONMessageReceivedDelegate   (DateTime                    Timestamp,
                                                                         IWWCPWebSocketClient        Client,
                                                                         WebSocketClientConnection   Connection,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         DateTime                    MessageTimestamp,
                                                                         NetworkingNode_Id           SourceNodeId,
                                                                         JArray                      JSONMessage,
                                                                         CancellationToken           CancellationToken);


    public delegate Task OnWebSocketClientBinaryMessageSentDelegate     (DateTime                    Timestamp,
                                                                         IWWCPWebSocketClient        Client,
                                                                         WebSocketClientConnection   Connection,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         DateTime                    MessageTimestamp,
                                                                         Byte[]                      BinaryMessage,
                                                                         SentStatus                  SentStatus,
                                                                         CancellationToken           CancellationToken);

    public delegate Task OnWebSocketClientBinaryMessageReceivedDelegate (DateTime                    Timestamp,
                                                                         IWWCPWebSocketClient        Client,
                                                                         WebSocketClientConnection   Connection,
                                                                         EventTracking_Id            EventTrackingId,
                                                                         DateTime                    MessageTimestamp,
                                                                         NetworkingNode_Id           SourceNodeId,
                                                                         Byte[]                      BinaryMessage,
                                                                         CancellationToken           CancellationToken);


    /// <summary>
    /// The common interface of all WWCP HTTP WebSocket clients.
    /// </summary>
    public interface IWWCPWebSocketClient : IWebSocketClient
    {


        NetworkingMode  NetworkingMode    { get; }


        /// <summary>
        /// An event sent whenever a JSON message was sent.
        /// </summary>
        event     OnWebSocketClientJSONMessageSentDelegate?         OnJSONMessageSent;

        /// <summary>
        /// An event sent whenever a JSON message was received.
        /// </summary>
        event     OnWebSocketClientJSONMessageReceivedDelegate?     OnJSONMessageReceived;


        /// <summary>
        /// An event sent whenever a binary message was sent.
        /// </summary>
        new event OnWebSocketClientBinaryMessageSentDelegate?       OnBinaryMessageSent;

        /// <summary>
        /// An event sent whenever a binary message was received.
        /// </summary>
        new event OnWebSocketClientBinaryMessageReceivedDelegate?   OnBinaryMessageReceived;


        //Task ProcessWebSocketTextFrame  (DateTime                   RequestTimestamp,
        //                                 WebSocketClientConnection  ClientConnection,
        //                                 EventTracking_Id           EventTrackingId,
        //                                 String                     TextMessage,
        //                                 CancellationToken          CancellationToken);
        //Task ProcessWebSocketBinaryFrame(DateTime                   RequestTimestamp,
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
