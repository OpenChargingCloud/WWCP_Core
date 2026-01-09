/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The common interface of all "send" messages.
    /// </summary>
    public interface IMessage : ISignableMessage
    {

        /// <summary>
        /// The networking node identification of the message sender or destination.
        /// </summary>
        [Mandatory]
        SourceRouting      Destination          { get; }

        /// <summary>
        /// The network path of the request.
        /// </summary>
        [Mandatory]
        NetworkPath        NetworkPath          { get; }

        /// <summary>
        /// The message identification.
        /// </summary>
        [Mandatory]
        Request_Id         MessageId            { get; }

        /// <summary>
        /// The timestamp of the message creation.
        /// </summary>
        [Mandatory]
        DateTimeOffset     MessageTimestamp     { get; }

        /// <summary>
        /// The event tracking identification for correlating this message with other events.
        /// </summary>
        [Mandatory]
        EventTracking_Id   EventTrackingId      { get; }

        /// <summary>
        /// The OCPP HTTP WebSocket action.
        /// </summary>
        [Mandatory]
        String             Action               { get; }

        /// <summary>
        /// An optional custom data object allowing to store any kind of customer specific data.
        /// </summary>
        [Optional]
        CustomData?        CustomData           { get; }

        /// <summary>
        /// An optional token to cancel this message.
        /// </summary>
        CancellationToken  CancellationToken    { get; }

    }

}
