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

namespace cloud.charging.open.protocols.WWCP.SSH
{

    /// <summary>
    /// A networking node is connected over an SSH subsystem: accepted by a server, or reached by a client.
    /// </summary>
    /// <param name="Timestamp">When.</param>
    /// <param name="Transport">The server or the client.</param>
    /// <param name="Connection">The connection.</param>
    /// <param name="NetworkingNodeId">The networking node at the other end, where it is known: always on a server.</param>
    /// <param name="NetworkingMode">The networking mode it asked for, if any.</param>
    /// <param name="EventTrackingId">An event tracking identification.</param>
    /// <param name="CancellationToken">A cancellation token.</param>
    public delegate Task OnWWCPSshConnectionDelegate        (DateTimeOffset         Timestamp,
                                                             IWWCPTransport         Transport,
                                                             SshMessageConnection   Connection,
                                                             NetworkingNode_Id?     NetworkingNodeId,
                                                             NetworkingMode?        NetworkingMode,
                                                             EventTracking_Id       EventTrackingId,
                                                             CancellationToken      CancellationToken);

    /// <summary>
    /// A connection over an SSH subsystem ended.
    /// </summary>
    /// <param name="Timestamp">When.</param>
    /// <param name="Transport">The server or the client.</param>
    /// <param name="Connection">The connection.</param>
    /// <param name="NetworkingNodeId">The networking node at the other end, where it is known.</param>
    /// <param name="Close">How it ended.</param>
    /// <param name="CancellationToken">A cancellation token.</param>
    public delegate Task OnWWCPSshConnectionClosedDelegate  (DateTimeOffset         Timestamp,
                                                             IWWCPTransport         Transport,
                                                             SshMessageConnection   Connection,
                                                             NetworkingNode_Id?     NetworkingNodeId,
                                                             SshMessageCloseInfo    Close,
                                                             CancellationToken      CancellationToken);

    /// <summary>
    /// An attempt of a client to connect over an SSH subsystem failed - the first connection's, or one after
    /// the connection ended - and the client tries again, or not, as <paramref name="NextAttemptIn"/> says.
    /// </summary>
    /// <param name="Timestamp">When.</param>
    /// <param name="Client">The client.</param>
    /// <param name="Error">Why: a host key not believed, a login not taken, the subsystem refused, a server not reached.</param>
    /// <param name="Attempt">Which attempt failed, counted from 1 since the last connection - or since the first failed.</param>
    /// <param name="NextAttemptIn">When the next attempt is made; null where the client gives up.</param>
    /// <param name="CancellationToken">A cancellation token.</param>
    public delegate Task OnWWCPSshConnectFailedDelegate     (DateTimeOffset         Timestamp,
                                                             WWCPSshClient          Client,
                                                             Exception              Error,
                                                             UInt32                 Attempt,
                                                             TimeSpan?              NextAttemptIn,
                                                             CancellationToken      CancellationToken);

    /// <summary>
    /// A JSON message was received - over whatever transport.
    /// </summary>
    public delegate Task OnWWCPJSONMessageReceivedDelegate  (DateTimeOffset         Timestamp,
                                                             IWWCPTransport         Transport,
                                                             IMessageConnection     Connection,
                                                             DateTimeOffset         MessageTimestamp,
                                                             EventTracking_Id       EventTrackingId,
                                                             NetworkingNode_Id      SourceNodeId,
                                                             JArray                 JSONMessage,
                                                             CancellationToken      CancellationToken);

    /// <summary>
    /// A binary message was received - over whatever transport.
    /// </summary>
    public delegate Task OnWWCPBinaryMessageReceivedDelegate(DateTimeOffset         Timestamp,
                                                             IWWCPTransport         Transport,
                                                             IMessageConnection     Connection,
                                                             DateTimeOffset         MessageTimestamp,
                                                             EventTracking_Id       EventTrackingId,
                                                             NetworkingNode_Id      SourceNodeId,
                                                             Byte[]                 BinaryMessage,
                                                             CancellationToken      CancellationToken);

    /// <summary>
    /// A JSON message was sent, or failed to be - over whatever transport.
    /// </summary>
    public delegate Task OnWWCPJSONMessageSentDelegate      (DateTimeOffset         Timestamp,
                                                             IWWCPTransport         Transport,
                                                             IMessageConnection     Connection,
                                                             DateTimeOffset         MessageTimestamp,
                                                             EventTracking_Id       EventTrackingId,
                                                             JArray                 JSONMessage,
                                                             SentStatus             SentStatus,
                                                             CancellationToken      CancellationToken);

    /// <summary>
    /// A binary message was sent, or failed to be - over whatever transport.
    /// </summary>
    public delegate Task OnWWCPBinaryMessageSentDelegate    (DateTimeOffset         Timestamp,
                                                             IWWCPTransport         Transport,
                                                             IMessageConnection     Connection,
                                                             DateTimeOffset         MessageTimestamp,
                                                             EventTracking_Id       EventTrackingId,
                                                             Byte[]                 BinaryMessage,
                                                             SentStatus             SentStatus,
                                                             CancellationToken      CancellationToken);

}
