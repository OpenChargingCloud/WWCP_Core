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

using cloud.charging.open.protocols.WWCP.OverlayNetworking;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking
{

    /// <summary>
    /// The reachability of a networking node within the overlay network.
    /// </summary>
    public class Reachability
    {

        #region Properties

        public NetworkingNode_Id      NetworkingNodeId       { get; }

        public IWebSocketClient?  EEBusWebSocketClient    { get; }

        public IWebSocketServer?  EEBusWebSocketServer    { get; }

        public NetworkingNode_Id?     NetworkingHub          { get; }

        public Byte                   Priority               { get; }

        public DateTime               Timestamp              { get; }

        public DateTime?              Timeout                { get; }

        #endregion

        #region Constructor(s)

        #region Reachability(NetworkingNodeId, EEBusWebSocketClient, Priority = 0, Timestamp = null, Timeout = null)

        public Reachability(NetworkingNode_Id     NetworkingNodeId,
                            IWebSocketClient  EEBusWebSocketClient,
                            Byte?                 Priority    = 0,
                            DateTime?             Timestamp   = null,
                            DateTime?             Timeout     = null)
        {

            this.NetworkingNodeId     = NetworkingNodeId;
            this.EEBusWebSocketClient  = EEBusWebSocketClient;
            this.Priority             = Priority  ?? 0;
            this.Timestamp            = Timestamp ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.Timeout              = Timeout;

        }

        #endregion

        #region Reachability(NetworkingNodeId, EEBusWebSocketServer, Priority = 0, Timestamp = null, Timeout = null)

        public Reachability(NetworkingNode_Id     NetworkingNodeId,
                            IWebSocketServer  EEBusWebSocketServer,
                            Byte?                 Priority    = 0,
                            DateTime?             Timestamp   = null,
                            DateTime?             Timeout     = null)
        {

            this.NetworkingNodeId     = NetworkingNodeId;
            this.EEBusWebSocketServer  = EEBusWebSocketServer;
            this.Priority             = Priority  ?? 0;
            this.Timestamp            = Timestamp ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.Timeout              = Timeout;

        }

        #endregion

        #region Reachability(NetworkingNodeId, NetworkingHub,       Priority = 0, Timestamp = null, Timeout = null)

        public Reachability(NetworkingNode_Id  NetworkingNodeId,
                            NetworkingNode_Id  NetworkingHub,
                            Byte?              Priority    = 0,
                            DateTime?          Timestamp   = null,
                            DateTime?          Timeout     = null)
        {

            this.NetworkingNodeId     = NetworkingNodeId;
            this.NetworkingHub        = NetworkingHub;
            this.Priority             = Priority  ?? 0;
            this.Timestamp            = Timestamp ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.Timeout              = Timeout;

        }

        #endregion

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (EEBusWebSocketClient is not null)
                return $"'{NetworkingNodeId}' via HTTP Web Socker client '{EEBusWebSocketClient.RemoteURL}'";

            if (EEBusWebSocketServer is not null)
                return $"'{NetworkingNodeId}' via HTTP Web Socker server '{EEBusWebSocketServer.IPSocket}'";

            if (EEBusWebSocketClient is not null)
                return $"'{NetworkingNodeId}' via networking hub '{NetworkingHub}'";

            return String.Empty;

        }

        #endregion


    }

}
