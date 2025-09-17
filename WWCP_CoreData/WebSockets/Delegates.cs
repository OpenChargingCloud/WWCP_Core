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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using WWCPNN = cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP.WebSockets
{

    public delegate Task OnWebSocketJSONMessageRequestDelegate   (DateTimeOffset            Timestamp,
                                                                  WWCPNN.INetworkingNode      Server,
                                                                  IWebSocketConnection        Connection,
                                                                  WWCPNN.NetworkingNode_Id    DestinationId,
                                                                  WWCPNN.NetworkPath          NetworkPath,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTimeOffset            RequestTimestamp,
                                                                  JArray                      RequestMessage,
                                                                  CancellationToken           CancellationToken);

    public delegate Task OnWebSocketJSONMessageResponseDelegate  (DateTimeOffset            Timestamp,
                                                                  WWCPNN.INetworkingNode      Server,
                                                                  IWebSocketConnection        Connection,
                                                                  WWCPNN.NetworkingNode_Id    DestinationId,
                                                                  WWCPNN.NetworkPath          NetworkPath,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTimeOffset            RequestTimestamp,
                                                                  JArray                      JSONRequestMessage,
                                                                  Byte[]                      BinaryRequestMessage,
                                                                  DateTimeOffset            ResponseTimestamp,
                                                                  JArray                      ResponseMessage,
                                                                  CancellationToken           CancellationToken);



    public delegate Task OnWebSocketBinaryMessageRequestDelegate (DateTimeOffset            Timestamp,
                                                                  WWCPNN.INetworkingNode      Server,
                                                                  IWebSocketConnection        Connection,
                                                                  WWCPNN.NetworkingNode_Id    DestinationId,
                                                                  WWCPNN.NetworkPath          NetworkPath,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTimeOffset            RequestTimestamp,
                                                                  Byte[]                      RequestMessage,
                                                                  CancellationToken           CancellationToken);

    public delegate Task OnWebSocketBinaryMessageResponseDelegate(DateTimeOffset            Timestamp,
                                                                  WWCPNN.INetworkingNode      Server,
                                                                  IWebSocketConnection        Connection,
                                                                  WWCPNN.NetworkingNode_Id    DestinationId,
                                                                  WWCPNN.NetworkPath          NetworkPath,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTimeOffset            RequestTimestamp,
                                                                  JArray                      JSONRequestMessage,
                                                                  Byte[]                      BinaryRequestMessage,
                                                                  DateTimeOffset            ResponseTimestamp,
                                                                  Byte[]?                     ResponseMessage,
                                                                  CancellationToken           CancellationToken);

}
