/*
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.OverlayNetworking;
using cloud.charging.open.protocols.WWCP.OverlayNetworking.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking
{

    /// <summary>
    /// The common interface of all EEBus HTTP Web Socket clients.
    /// </summary>
    public interface IWebSocketClient : IHTTPClient
    {

        //IEEBusAdapter    EEBusAdapter       { get; }
        NetworkingMode  NetworkingMode    { get; }


        event OnWebSocketJSONMessageRequestDelegate?     OnJSONMessageRequestReceived;
        event OnWebSocketJSONMessageRequestDelegate?     OnJSONMessageRequestSent;
        event OnWebSocketJSONMessageResponseDelegate?    OnJSONMessageResponseReceived;
        event OnWebSocketJSONMessageResponseDelegate?    OnJSONMessageResponseSent;

        event OnWebSocketBinaryMessageRequestDelegate?   OnBinaryMessageRequestReceived;
        event OnWebSocketBinaryMessageRequestDelegate?   OnBinaryMessageRequestSent;
        event OnWebSocketBinaryMessageResponseDelegate?  OnBinaryMessageResponseReceived;
        event OnWebSocketBinaryMessageResponseDelegate?  OnBinaryMessageResponseSent;


        Task ProcessWebSocketTextFrame  (DateTime                   RequestTimestamp,
                                         WebSocketClientConnection  ClientConnection,
                                         EventTracking_Id           EventTrackingId,
                                         String                     TextMessage,
                                         CancellationToken          CancellationToken);
        Task ProcessWebSocketBinaryFrame(DateTime                   RequestTimestamp,
                                         WebSocketClientConnection  ClientConnection,
                                         EventTracking_Id           EventTrackingId,
                                         Byte[]                     BinaryMessage,
                                         CancellationToken          CancellationToken);


        Task<SendWebSocketMessageResult> SendJSONRequest       (JSONRequestMessage        JSONRequestMessage);
        Task<SendWebSocketMessageResult> SendJSONResponse      (JSONResponseMessage       JSONResponseMessage);
        Task<SendWebSocketMessageResult> SendJSONRequestError  (JSONRequestErrorMessage   JSONRequestErrorMessage);
        Task<SendWebSocketMessageResult> SendJSONResponseError (JSONResponseErrorMessage  JSONResponseErrorMessage);


        Task<SendWebSocketMessageResult> SendBinaryRequest     (BinaryRequestMessage      BinaryRequestMessage);
        Task<SendWebSocketMessageResult> SendBinaryResponse    (BinaryResponseMessage     BinaryResponseMessage);


    }

}
