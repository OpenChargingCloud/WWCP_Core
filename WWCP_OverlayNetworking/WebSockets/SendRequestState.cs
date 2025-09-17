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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using cloud.charging.open.protocols.WWCP.OverlayNetworking.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking
{

    /// <summary>
    /// Keeping track of a sent request and its response.
    /// </summary>
    /// <param name="RequestTimestamp">The time stamp of the request.</param>
    /// <param name="DestinationNodeId">The destination network node identification of the request and thus the expected source of the response.</param>
    /// <param name="Timeout">The timeout of the request.</param>
    /// 
    /// <param name="JSONRequest">The JSON request message.</param>
    /// <param name="BinaryRequest">The binary request message.</param>
    /// 
    /// <param name="ResponseTimestamp">The time stamp of the response.</param>
    /// <param name="JSONResponse">The JSON response message.</param>
    /// <param name="BinaryResponse">The binary response message.</param>
    /// 
    /// <param name="JSONRequestErrorMessage">An optional JSON request error message.</param>
    /// <param name="JSONResponseErrorMessage">An optional JSON response error message.</param>
    public class SendRequestState(DateTimeOffset             RequestTimestamp,
                                  NetworkingNode_Id          DestinationNodeId,
                                  NetworkPath                NetworkPath,
                                  DateTimeOffset             Timeout,

                                  JSONRequestMessage?        JSONRequest                = null,
                                  BinaryRequestMessage?      BinaryRequest              = null,

                                  DateTimeOffset?            ResponseTimestamp          = null,
                                  JSONResponseMessage?       JSONResponse               = null,
                                  BinaryResponseMessage?     BinaryResponse             = null,

                                  JSONRequestErrorMessage?   JSONRequestErrorMessage    = null,
                                  JSONResponseErrorMessage?  JSONResponseErrorMessage   = null)
    {

        #region Properties

        /// <summary>
        /// The time stamp of the request.
        /// </summary>
        public DateTimeOffset             RequestTimestamp            { get; }      = RequestTimestamp;

        /// <summary>
        /// The destination network node identification of the request
        /// and thus the expected source of the response.
        /// </summary>
        public NetworkingNode_Id          DestinationNodeId           { get; }      = DestinationNodeId;

        /// <summary>
        /// The network (source) path of the response.
        /// </summary>
        public NetworkPath                NetworkPath                 { get; set; } = NetworkPath;

        /// <summary>
        /// The timeout of the request.
        /// </summary>
        public DateTimeOffset             Timeout                     { get; }      = Timeout;


        /// <summary>
        /// The JSON request message.
        /// </summary>
        public JSONRequestMessage?        JSONRequest                 { get; }      = JSONRequest;

        /// <summary>
        /// The binary request message.
        /// </summary>
        public BinaryRequestMessage?      BinaryRequest               { get; }      = BinaryRequest;


        /// <summary>
        /// The time stamp of the response.
        /// </summary>
        public DateTimeOffset?            ResponseTimestamp           { get; set; } = ResponseTimestamp;

        /// <summary>
        /// The JSON response message.
        /// </summary>
        public JSONResponseMessage?       JSONResponse                { get; set; } = JSONResponse;

        /// <summary>
        /// The binary response message.
        /// </summary>
        public BinaryResponseMessage?     BinaryResponse              { get; set; } = BinaryResponse;

        /// <summary>
        /// The optional JSON request error message.
        /// </summary>
        public JSONRequestErrorMessage?   JSONRequestErrorMessage     { get; set; } = JSONRequestErrorMessage;

        /// <summary>
        /// The optional JSON response error message.
        /// </summary>
        public JSONResponseErrorMessage?  JSONResponseErrorMessage    { get; set; } = JSONResponseErrorMessage;

        /// <summary>
        /// No Errors.
        /// </summary>
        public Boolean                    NoErrors
             => JSONRequestErrorMessage  is null &&
                JSONResponseErrorMessage is null;

        /// <summary>
        /// Errors occurred.
        /// </summary>
        public Boolean                    HasErrors
             => JSONRequestErrorMessage  is not null ||
                JSONResponseErrorMessage is not null;

        #endregion


        public Boolean IsValidJSONResponse(IRequest                          Request,
                                           [NotNullWhen(true)] out JObject?  JSONMessage)
        {

            if (NoErrors &&
                JSONResponse?.Payload   is not null &&
                JSONResponse. RequestId == Request.RequestId)
            {
                JSONMessage = JSONResponse.Payload;
                return true;
            }

            JSONMessage = null;
            return false;

        }

        public Boolean IsValidBinaryResponse(IRequest                         Request,
                                             [NotNullWhen(true)] out Byte[]?  BinaryMessage)
        {

            if (NoErrors &&
                BinaryResponse?.Payload   is not null &&
                BinaryResponse. RequestId == Request.RequestId)
            {
                BinaryMessage = BinaryResponse.Payload;
                return true;
            }

            BinaryMessage = null;
            return false;

        }


        #region (static) FromJSONRequest  (...)
        public static SendRequestState FromJSONRequest(DateTimeOffset            RequestTimestamp,
                                                       NetworkingNode_Id         DestinationNodeId,
                                                       DateTimeOffset            Timeout,

                                                       JSONRequestMessage?       JSONRequest               = null,

                                                       DateTimeOffset?           ResponseTimestamp         = null,
                                                       JSONResponseMessage?      JSONResponse              = null,
                                                       BinaryResponseMessage?    BinaryResponse            = null,

                                                       JSONRequestErrorMessage?  JSONRequestErrorMessage   = null)

            => new (RequestTimestamp,
                    DestinationNodeId,
                    NetworkPath.Empty,
                    Timeout,

                    JSONRequest,
                    null,

                    ResponseTimestamp,
                    JSONResponse,
                    BinaryResponse,

                    JSONRequestErrorMessage);

        #endregion

        #region (static) FromBinaryRequest(...)

        public static SendRequestState FromBinaryRequest(DateTimeOffset            RequestTimestamp,
                                                         NetworkingNode_Id         NetworkingNodeId,
                                                         DateTimeOffset            Timeout,

                                                         BinaryRequestMessage?     BinaryRequest             = null,

                                                         DateTimeOffset?           ResponseTimestamp         = null,
                                                         JSONResponseMessage?      JSONResponse              = null,
                                                         BinaryResponseMessage?    BinaryResponse            = null,

                                                         JSONRequestErrorMessage?  JSONRequestErrorMessage   = null)

            => new (RequestTimestamp,
                    NetworkingNodeId,
                    NetworkPath.Empty,
                    Timeout,

                    null,
                    BinaryRequest,

                    ResponseTimestamp,
                    JSONResponse,
                    BinaryResponse,

                    JSONRequestErrorMessage);

        #endregion


    }

}
