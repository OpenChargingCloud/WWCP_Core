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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.Messaging;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.WebSockets;
using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP.SSH
{

    /// <summary>
    /// What an SSH server and an SSH client of networking nodes do alike: take a message apart, tell
    /// who listens, and send.
    /// </summary>
    internal static class WWCPSshMessaging
    {

        #region HELLO properties and refusals

        /// <summary>
        /// The HELLO property naming the networking node a login connects as: the last path segment of the
        /// WebSocket transport's URL.
        /// </summary>
        public const String HelloNetworkingNodeId  = "networkingNodeId";

        /// <summary>
        /// The HELLO property telling the networking mode: the X-OCPP-NetworkingMode header of the WebSocket
        /// transport.
        /// </summary>
        public const String HelloNetworkingMode    = "networkingMode";

        /// <summary>
        /// The refusal of a login that may not connect as the networking node its HELLO names.
        /// </summary>
        public const String RefusalNotPermitted    = "not-permitted";

        #endregion


        #region ReceiveText  (Transport, Connection, MessageTimestamp, Text,  Handlers, Module, CancellationToken)

        /// <summary>
        /// A text message: a JSON array, for whoever listens - or, where it is none, a line in the log.
        /// </summary>
        public static async Task ReceiveText(IWWCPTransport                      Transport,
                                             IMessageConnection                  Connection,
                                             DateTimeOffset                      MessageTimestamp,
                                             String                              Text,
                                             OnWWCPJSONMessageReceivedDelegate?  Handlers,
                                             String                              Module,
                                             CancellationToken                   CancellationToken)
        {

            var sourceNodeId = Connection.NetworkingNodeIdOf();

            try
            {

                Text = Text.Trim();

                if (Text.IsNullOrEmpty() || Text == "[]")
                {
                    DebugX.Log($"{Module}: Received an empty text message from {(sourceNodeId.HasValue ? $"'{sourceNodeId}' ({Connection.RemoteSocket})" : Connection.RemoteSocket.ToString())}!");
                    return;
                }

                var jsonMessage = JSONMessages.ParseArray(Text);

                await RaiseAll(
                          Handlers,
                          handler => handler(
                                         Timestamp.Now,
                                         Transport,
                                         Connection,
                                         MessageTimestamp,
                                         EventTracking_Id.New,
                                         sourceNodeId ?? NetworkingNode_Id.Zero,
                                         jsonMessage,
                                         CancellationToken
                                     ),
                          Module
                      );

            }
            catch (Exception e)
            {
                DebugX.LogException(e, $"{Module}.{nameof(ReceiveText)}");
            }

        }

        #endregion

        #region ReceiveBinary(Transport, Connection, MessageTimestamp, Bytes, Handlers, Module, CancellationToken)

        /// <summary>
        /// A binary message, for whoever listens - or, where it is empty, a line in the log.
        /// </summary>
        public static async Task ReceiveBinary(IWWCPTransport                        Transport,
                                               IMessageConnection                    Connection,
                                               DateTimeOffset                        MessageTimestamp,
                                               Byte[]                                Bytes,
                                               OnWWCPBinaryMessageReceivedDelegate?  Handlers,
                                               String                                Module,
                                               CancellationToken                     CancellationToken)
        {

            var sourceNodeId = Connection.NetworkingNodeIdOf();

            if (Bytes.Length == 0)
            {
                DebugX.Log($"{Module}: Received an empty binary message from {(sourceNodeId.HasValue ? $"'{sourceNodeId}' ({Connection.RemoteSocket})" : Connection.RemoteSocket.ToString())}!");
                return;
            }

            await RaiseAll(
                      Handlers,
                      handler => handler(
                                     Timestamp.Now,
                                     Transport,
                                     Connection,
                                     MessageTimestamp,
                                     EventTracking_Id.New,
                                     sourceNodeId ?? NetworkingNode_Id.Zero,
                                     Bytes,
                                     CancellationToken
                                 ),
                      Module
                  );

        }

        #endregion

        #region SendJSON     (Transport, Connection, JSONMessage,   Formatting, MessageTimestamp, EventTrackingId, Handlers, Module, CancellationToken)

        /// <summary>
        /// Send a JSON message on the given connection.
        /// </summary>
        public static async Task<SentMessageResult> SendJSON(IWWCPTransport                  Transport,
                                                             IMessageConnection              Connection,
                                                             JArray                          JSONMessage,
                                                             Formatting                      Formatting,
                                                             DateTimeOffset                  MessageTimestamp,
                                                             EventTracking_Id                EventTrackingId,
                                                             OnWWCPJSONMessageSentDelegate?  Handlers,
                                                             String                          Module,
                                                             CancellationToken               CancellationToken)
        {

            try
            {

                var sentStatus = await Connection.SendTextMessage(
                                           JSONMessage.ToString(Formatting),
                                           CancellationToken
                                       );

                await RaiseAll(
                          Handlers,
                          handler => handler(
                                         Timestamp.Now,
                                         Transport,
                                         Connection,
                                         MessageTimestamp,
                                         EventTrackingId,
                                         JSONMessage,
                                         sentStatus,
                                         CancellationToken
                                     ),
                          Module
                      );

                return sentStatus == SentStatus.Success
                           ? SentMessageResult.Success(Connection)
                           : SentMessageResult.UnknownClient();

            }
            catch (Exception e)
            {
                return SentMessageResult.TransmissionFailed(e, Connection);
            }

        }

        #endregion

        #region SendBinary   (Transport, Connection, BinaryMessage,                   MessageTimestamp, EventTrackingId, Handlers, Module, CancellationToken)

        /// <summary>
        /// Send a binary message on the given connection.
        /// </summary>
        public static async Task<SentMessageResult> SendBinary(IWWCPTransport                    Transport,
                                                               IMessageConnection                Connection,
                                                               Byte[]                            BinaryMessage,
                                                               DateTimeOffset                    MessageTimestamp,
                                                               EventTracking_Id                  EventTrackingId,
                                                               OnWWCPBinaryMessageSentDelegate?  Handlers,
                                                               String                            Module,
                                                               CancellationToken                 CancellationToken)
        {

            try
            {

                var sentStatus = await Connection.SendBinaryMessage(
                                           BinaryMessage,
                                           CancellationToken
                                       );

                await RaiseAll(
                          Handlers,
                          handler => handler(
                                         Timestamp.Now,
                                         Transport,
                                         Connection,
                                         MessageTimestamp,
                                         EventTrackingId,
                                         BinaryMessage,
                                         sentStatus,
                                         CancellationToken
                                     ),
                          Module
                      );

                return sentStatus == SentStatus.Success
                           ? SentMessageResult.Success(Connection)
                           : SentMessageResult.UnknownClient();

            }
            catch (Exception e)
            {
                return SentMessageResult.TransmissionFailed(e, Connection);
            }

        }

        #endregion

        #region RaiseAll     (Handlers, Invoke, Module)

        /// <summary>
        /// Every handler at once, as the WebSocket transport does; one that fails is logged.
        /// </summary>
        public static async Task RaiseAll<TDelegate>(TDelegate?             Handlers,
                                                     Func<TDelegate, Task>  Invoke,
                                                     String                 Module)

            where TDelegate : Delegate

        {

            if (Handlers is null)
                return;

            try
            {
                await Task.WhenAll(
                          Handlers.GetInvocationList().
                                   OfType<TDelegate>().
                                   Select(Invoke)
                      );
            }
            catch (Exception e)
            {
                DebugX.LogException(e, $"{Module}.{typeof(TDelegate).Name}");
            }

        }

        #endregion

    }

}
