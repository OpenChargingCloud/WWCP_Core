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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking.WebSockets
{

    public class WebSocketResponse(JSONResponseMessage?      JSONResponseMessage,
                                   JSONRequestErrorMessage?  JSONErrorMessage,
                                   BinaryResponseMessage?    BinaryResponseMessage) : IEquatable<WebSocketResponse>
    {

        public JSONResponseMessage?      JSONResponseMessage      { get; } = JSONResponseMessage;
        public JSONRequestErrorMessage?  JSONErrorMessage         { get; } = JSONErrorMessage;
        public BinaryResponseMessage?    BinaryResponseMessage    { get; } = BinaryResponseMessage;



        public static WebSocketResponse FromJSONResponse  (JSONResponseMessage JSONResponseMessage)

            => new (JSONResponseMessage,
                    null,
                    null);

        public static WebSocketResponse FromJSONError     (JSONRequestErrorMessage JSONErrorMessage)

            => new (null,
                    JSONErrorMessage,
                    null);

        public static WebSocketResponse FromBinaryResponse(BinaryResponseMessage BinaryResponseMessage)

            => new (null,
                    null,
                    BinaryResponseMessage);



        public static WebSocketResponse JSONResponse(EventTracking_Id   EventTrackingId,
                                                     NetworkingNode_Id  DestinationNodeId,
                                                     NetworkPath        NetworkPath,
                                                     Request_Id         RequestId,
                                                     JObject            Payload,
                                                     CancellationToken  CancellationToken = default)

            => new (new JSONResponseMessage(
                        Timestamp.Now,
                        EventTrackingId,
                        NetworkingMode.Unknown,
                        DestinationNodeId,
                        NetworkPath,
                        RequestId,
                        Payload,
                        CancellationToken
                    ),
                    null,
                    null);



        public static WebSocketResponse JSONError(EventTracking_Id   EventTrackingId,
                                                  NetworkingNode_Id  DestinationNodeId,
                                                  NetworkPath        NetworkPath,
                                                  Request_Id         RequestId,
                                                  ResultCode         ErrorCode,
                                                  String?            ErrorDescription    = null,
                                                  JObject?           ErrorDetails        = null,
                                                  CancellationToken  CancellationToken   = default)

            => new (null,
                    new JSONRequestErrorMessage(
                        Timestamp.Now,
                        EventTrackingId,
                        NetworkingMode.Unknown,
                        DestinationNodeId,
                        NetworkPath,
                        RequestId,
                        ErrorCode,
                        ErrorDescription,
                        ErrorDetails,
                        CancellationToken
                    ),
                    null);



        public static WebSocketResponse BinaryResponse(EventTracking_Id   EventTrackingId,
                                                       NetworkingNode_Id  DestinationNodeId,
                                                       NetworkPath        NetworkPath,
                                                       Request_Id         RequestId,
                                                       Byte[]             Payload,
                                                       CancellationToken  CancellationToken = default)

            => new (null,
                    null,
                    new BinaryResponseMessage(
                        Timestamp.Now,
                        EventTrackingId,
                        NetworkingMode.Unknown,
                        DestinationNodeId,
                        NetworkPath,
                        RequestId,
                        Payload,
                        CancellationToken
                    ));



        public static WebSocketResponse CouldNotParse(EventTracking_Id  EventTrackingId,
                                                      Request_Id        RequestId,
                                                      String            Action,
                                                      JObject           JSONObjectRequest,
                                                      String?           ErrorResponse   = null)

            => new (null,
                    JSONRequestErrorMessage.CouldNotParse(
                        EventTrackingId,
                        RequestId,
                        Action,
                        JSONObjectRequest,
                        ErrorResponse
                    ),
                    null);

        public static WebSocketResponse CouldNotParse(EventTracking_Id  EventTrackingId,
                                                      Request_Id        RequestId,
                                                      String            Action,
                                                      Byte[]            BinaryRequest,
                                                      String?           ErrorResponse   = null)

            => new (null,
                    JSONRequestErrorMessage.CouldNotParse(
                        EventTrackingId,
                        RequestId,
                        Action,
                        BinaryRequest,
                        ErrorResponse
                    ),
                    null);


        public static WebSocketResponse FormationViolation(EventTracking_Id  EventTrackingId,
                                                           Request_Id        RequestId,
                                                           String            Action,
                                                           JObject           JSONObjectRequest,
                                                           Exception         Exception)

            => new (null,
                    JSONRequestErrorMessage.FormationViolation(
                        EventTrackingId,
                        RequestId,
                        Action,
                        JSONObjectRequest,
                        Exception
                    ),
                    null);


        public static WebSocketResponse FormationViolation(EventTracking_Id  EventTrackingId,
                                                           Request_Id        RequestId,
                                                           String            Action,
                                                           Byte[]            BinaryRequest,
                                                           Exception         Exception)

            => new (null,
                    JSONRequestErrorMessage.FormationViolation(
                        EventTrackingId,
                        RequestId,
                        Action,
                        BinaryRequest,
                        Exception
                    ),
                    null);




        public override Boolean Equals(Object? EEBusResponse)

            => EEBusResponse is WebSocketResponse ocppResponse &&
               Equals(ocppResponse);


        public Boolean Equals(WebSocketResponse? EEBusResponse)

            => EEBusResponse is not null &&
               Equals(JSONResponseMessage,   EEBusResponse.JSONResponseMessage) &&
               Equals(JSONErrorMessage,      EEBusResponse.JSONErrorMessage)    &&
               Equals(BinaryResponseMessage, EEBusResponse.BinaryResponseMessage);


        public override Int32 GetHashCode()

            => (JSONResponseMessage?.  GetHashCode() ?? 0) ^
               (JSONErrorMessage?.     GetHashCode() ?? 0) ^
               (BinaryResponseMessage?.GetHashCode() ?? 0);


    }

}
