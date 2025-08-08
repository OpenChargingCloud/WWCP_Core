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

using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.NetworkingNode
{

    /// <summary>
    /// The reachability of a networking node within the overlay network.
    /// </summary>
    public class Reachability
    {

        #region Properties

        /// <summary>
        /// The destination node identification.
        /// </summary>
        public NetworkingNode_Id              DestinationId          { get; }


        /// <summary>
        /// The OCPP WebSocket client to use to reach the destination node.
        /// </summary>
        public IWWCPWebSocketClient?          WWCPWebSocketClient    { get; }

        /// <summary>
        /// The OCPP WebSocket server to use to reach the destination node.
        /// </summary>
        public IWWCPWebSocketServer?          WWCPWebSocketServer    { get; }

        /// <summary>
        /// The networking hub to use to reach the destination node.
        /// </summary>
        public NetworkingNode_Id?             NetworkingHub          { get; }


        public VirtualNetworkLinkInformation  Uplink                 { get; }

        public VirtualNetworkLinkInformation  Downlink               { get; }


        /// <summary>
        /// The priority of this reachability, when multiple paths are available.
        /// </summary>
        public Byte                           Priority               { get; }

        /// <summary>
        /// The weight of this reachability under the same priority,
        /// when multiple paths are available.
        /// </summary>
        public Byte                           Weight                 { get; }


        public DateTimeOffset                 Timestamp              { get; }

        public DateTimeOffset?                Timeout                { get; }

        #endregion

        #region Constructor(s)
        private Reachability(NetworkingNode_Id               DestinationId,
                             IWWCPWebSocketClient?           WWCPWebSocketClient   = null,
                             IWWCPWebSocketServer?           WWCPWebSocketServer   = null,
                             NetworkingNode_Id?              NetworkingHub         = null,

                             VirtualNetworkLinkInformation?  Uplink                = null,
                             VirtualNetworkLinkInformation?  Downlink              = null,

                             Byte?                           Priority              = 0,
                             Byte?                           Weight                = 1,
                             DateTimeOffset?                 Timestamp             = null,
                             DateTimeOffset?                 Timeout               = null)
        {

            this.DestinationId        = DestinationId;
            this.WWCPWebSocketClient  = WWCPWebSocketClient;
            this.WWCPWebSocketServer  = WWCPWebSocketServer;
            this.NetworkingHub        = NetworkingHub;
            this.Uplink               = Uplink    ?? new VirtualNetworkLinkInformation(Distance: 1);
            this.Downlink             = Downlink  ?? new VirtualNetworkLinkInformation(Distance: 1);
            this.Priority             = Priority  ?? 0;
            this.Weight               = Weight    ?? 1;
            this.Timestamp            = Timestamp ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.Timeout              = Timeout;

        }

        #endregion



        #region (static) FromWebSocketClient (DestinationId, WebSocketClient, ...)

        public static Reachability FromWebSocketClient(NetworkingNode_Id               DestinationId,
                                                       IWWCPWebSocketClient            WebSocketClient,
                                                       VirtualNetworkLinkInformation?  Uplink       = null,
                                                       VirtualNetworkLinkInformation?  Downlink     = null,
                                                       Byte?                           Priority     = 0,
                                                       Byte?                           Weight       = 1,
                                                       DateTimeOffset?                 Timestamp    = null,
                                                       DateTimeOffset?                 Timeout      = null)

            => new (DestinationId,
                    WebSocketClient,
                    null,
                    null,
                    Uplink,
                    Downlink,
                    Priority,
                    Weight,
                    Timestamp,
                    Timeout);


        #endregion

        #region (static) FromWebSocketServer (DestinationId, WebSocketClient, ...)

        public static Reachability FromWebSocketServer(NetworkingNode_Id               DestinationId,
                                                       IWWCPWebSocketServer            WebSocketServer,
                                                       VirtualNetworkLinkInformation?  Uplink       = null,
                                                       VirtualNetworkLinkInformation?  Downlink     = null,
                                                       Byte?                           Priority     = 0,
                                                       Byte?                           Weight       = 1,
                                                       DateTimeOffset?                 Timestamp    = null,
                                                       DateTimeOffset?                 Timeout      = null)

            => new (DestinationId,
                    null,
                    WebSocketServer,
                    null,
                    Uplink,
                    Downlink,
                    Priority,
                    Weight,
                    Timestamp,
                    Timeout);


        #endregion

        #region(static) FromNetworkingHub   (DestinationId, NetworkingHub,   ...)

        public static Reachability FromNetworkingHub(NetworkingNode_Id               DestinationId,
                                                     NetworkingNode_Id               NetworkingHub,
                                                     VirtualNetworkLinkInformation?  Uplink       = null,
                                                     VirtualNetworkLinkInformation?  Downlink     = null,
                                                     Byte?                           Priority     = 0,
                                                     Byte?                           Weight       = 1,
                                                     DateTimeOffset?                 Timestamp    = null,
                                                     DateTimeOffset?                 Timeout      = null)

            => new (DestinationId,
                    null,
                    null,
                    NetworkingHub,
                    Uplink,
                    Downlink,
                    Priority,
                    Weight,
                    Timestamp,
                    Timeout);

        #endregion



        public JObject ToJSON(Boolean IgnoreDestinationId = false)
        {

            var json = JSONObject.Create(

                           IgnoreDestinationId
                               ? null
                               : new JProperty("destinationId",     DestinationId.                ToString()),


                           WWCPWebSocketClient is not null
                               ? new JProperty("webSocketClient",   WWCPWebSocketClient.RemoteURL.ToString())
                               : null,

                           WWCPWebSocketServer is not null
                               ? new JProperty("webSocketServer",   WWCPWebSocketServer.IPSocket. ToString())
                               : null,

                           NetworkingHub.HasValue
                               ? new JProperty("networkingHub",     NetworkingHub.                ToString())
                               : null,

                                 new JProperty("uplink",            Uplink.                       ToJSON()),
                                 new JProperty("downlink",          Downlink.                     ToJSON()),

                                 new JProperty("priority",          Priority),
                                 new JProperty("weight",            Weight),


                                 new JProperty("timestamp",         Timestamp.                    ToISO8601()),

                           Timeout.HasValue
                               ? new JProperty("timeout",           Timeout.Value.                ToISO8601())
                               : null

                       );

            return json;

        }


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (WWCPWebSocketClient is not null)
                return $"'{DestinationId}' via HTTP Web Socker client '{WWCPWebSocketClient.RemoteURL}'";

            if (WWCPWebSocketServer is not null)
                return $"'{DestinationId}' via HTTP Web Socker server '{WWCPWebSocketServer.IPSocket}'";

            if (NetworkingHub       is not null)
                return $"'{DestinationId}' via networking hub '{NetworkingHub}'";

            return String.Empty;

        }

        #endregion


    }

}
