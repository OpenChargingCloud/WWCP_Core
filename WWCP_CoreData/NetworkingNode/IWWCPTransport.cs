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

using org.GraphDefined.Vanaheimr.Hermod.Messaging;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.NetworkingNode
{

    /// <summary>
    /// What a networking node exchanges messages with other networking nodes over: a client connection it
    /// keeps up, or a server others connect to - over an HTTP WebSocket, or over an SSH subsystem.
    /// </summary>
    public interface IWWCPTransport
    {

        /// <summary>
        /// What carries the messages: a WebSocket, or an SSH channel.
        /// </summary>
        MessageTransport  Transport             { get; }

        /// <summary>
        /// Where the transport goes or listens, for routing tables and logs: "wss://csms.example.org/cs001",
        /// "ssh://cs001@lc.example.org:22", "0.0.0.0:2351", "SSH subsystem ocpp@open.charging.cloud".
        /// </summary>
        String            TransportDescription  { get; }

    }


    /// <summary>
    /// A connection a networking node keeps up to another one: one far end, whatever carries it.
    /// </summary>
    public interface IWWCPClient : IWWCPTransport
    {

        /// <summary>
        /// Whether messages carry their network path (an overlay network) or not.
        /// </summary>
        NetworkingMode  NetworkingMode  { get; }

    }


    /// <summary>
    /// A server other networking nodes connect to, each with a connection of its own.
    /// </summary>
    public interface IWWCPServer : IWWCPTransport
    {

        /// <summary>
        /// The networking node serving.
        /// </summary>
        INetworkingNode                 NetworkingNode              { get; }

        /// <summary>
        /// The networking nodes connected now.
        /// </summary>
        IEnumerable<NetworkingNode_Id>  ConnectedNetworkingNodeIds  { get; }

    }


    /// <summary>
    /// Which transport a message connection belongs to.
    /// </summary>
    public static class WWCPConnectionExtensions
    {

        /// <summary>
        /// The custom data key under which a connection notes the WWCP client it belongs to.
        /// </summary>
        public const String WWCPClientKey = "WWCPClient";

        /// <summary>
        /// The WWCP client a connection this networking node opened belongs to - or null for a connection
        /// it accepted, which belongs to a server and its many connections.
        /// </summary>
        /// <param name="Connection">A message connection.</param>
        public static IWWCPClient? WWCPClientOf(this IMessageConnection Connection)

            => Connection switch {
                   WebSocketClientConnection webSocketClientConnection  => webSocketClientConnection.WebSocketClient as IWWCPClient,
                   _ when Connection.Role == ConnectionRole.Client      => Connection.TryGetCustomData(WWCPClientKey) as IWWCPClient,
                   _                                                    => null
               };

        /// <summary>
        /// The networking node at the other end of a connection, where it has been registered.
        /// </summary>
        /// <param name="Connection">A message connection.</param>
        public static NetworkingNode_Id? NetworkingNodeIdOf(this IMessageConnection Connection)

            => Connection.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.NetworkingNodeId);

        /// <summary>
        /// Whether the far end of a connection dialled it in the overlay network - X-OCPP-NetworkingMode or its
        /// SSH HELLO said so - and so is sent messages that name where they go and where they come from.
        /// </summary>
        /// <param name="Connection">A message connection.</param>
        public static Boolean InOverlayNetwork(this IMessageConnection Connection)

            => Connection.TryGetCustomDataAs<NetworkingMode>(WebSocketKeys.NetworkingMode) == NetworkingMode.OverlayNetwork;

        /// <summary>
        /// Whether a message for the given networking node may go over a connection to the given next hop:
        /// always where the next hop is that node, and through it only in the overlay network.
        /// </summary>
        /// <remarks>
        /// A message in the standard networking mode names no destination, and its receiver takes it as its
        /// own. Over a local controller's standard connection a request for a charging station behind it would
        /// be carried out by the controller itself - a Reset, a SetVariables, an InstallSSHKey - and answered
        /// as if by the station.
        /// </remarks>
        /// <param name="Connection">A connection to the next hop.</param>
        /// <param name="NextHop">The networking node the connection leads to.</param>
        /// <param name="DestinationId">The networking node the message is for.</param>
        public static Boolean Carries(this IMessageConnection  Connection,
                                      NetworkingNode_Id        NextHop,
                                      NetworkingNode_Id        DestinationId)

            => NextHop == DestinationId ||
               Connection.InOverlayNetwork();

    }

}
