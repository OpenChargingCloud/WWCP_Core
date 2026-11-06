/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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
using org.GraphDefined.Vanaheimr.Hermod.SSH;
using org.GraphDefined.Vanaheimr.Hermod.Messaging;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP.WebSockets
{

    /// <summary>
    /// A message connection, for logs and event streams.
    /// </summary>
    public static class MessageConnectionJSON
    {

        /// <summary>
        /// A message connection as JSON: a WebSocket connection as it writes itself; an SSH connection with
        /// its sockets, its login, its subsystem and the networking node at its other end.
        /// </summary>
        /// <param name="Connection">A message connection.</param>
        public static JObject ToJSON(this IMessageConnection Connection)

            => Connection switch {

                   IWebSocketConnection webSocketConnection
                       => webSocketConnection.ToJSON(),

                   ISshMessageConnection sshConnection
                       => JSONObject.Create(
                              new JProperty("transport",     "SSH"),
                              new JProperty("role",          sshConnection.Role.ToString()),
                              new JProperty("localSocket",   sshConnection.LocalSocket. ToString()),
                              new JProperty("remoteSocket",  sshConnection.RemoteSocket.ToString()),
                              new JProperty("username",      sshConnection.Username),
                              new JProperty("subsystem",     sshConnection.Subsystem),

                              sshConnection.Subprotocol is not null
                                  ? new JProperty("subprotocol",       sshConnection.Subprotocol)
                                  : null,

                              sshConnection.NetworkingNodeIdOf() is NetworkingNode_Id networkingNodeId
                                  ? new JProperty("networkingNodeId",  networkingNodeId.ToString())
                                  : null
                          ),

                   _   => JSONObject.Create(
                              new JProperty("transport",     Connection.Transport.ToString()),
                              new JProperty("role",          Connection.Role.ToString()),
                              new JProperty("localSocket",   Connection.LocalSocket. ToString()),
                              new JProperty("remoteSocket",  Connection.RemoteSocket.ToString())
                          )

               };

    }

}
