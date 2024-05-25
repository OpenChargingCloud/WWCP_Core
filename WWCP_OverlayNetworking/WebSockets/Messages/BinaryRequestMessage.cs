﻿/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.WWCP.OverlayNetworking;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking.WebSockets
{

    /// <summary>
    /// An EEBus HTTP Web Socket binary request message.
    /// </summary>
    /// <param name="RequestTimestamp">The request time stamp.</param>
    /// <param name="EventTrackingId">An optional event tracking identification.</param>
    /// <param name="NetworkingMode">The networking mode to use.</param>
    /// <param name="DestinationId">The networking node identification of the message destination.</param>
    /// <param name="NetworkPath">The (recorded) path of the request through the overlay network.</param>
    /// <param name="RequestId">An unique request identification.</param>
    /// <param name="Action">An EEBus action/method name.</param>
    /// <param name="Payload">The binary request message payload.</param>
    /// <param name="RequestTimeout">The request time out.</param>
    /// <param name="ErrorMessage">An optional error message, e.g. during sending of the message.</param>
    /// <param name="CancellationToken">The cancellation token.</param>
    public class BinaryRequestMessage(DateTime           RequestTimestamp,
                                      EventTracking_Id   EventTrackingId,
                                      NetworkingMode     NetworkingMode,
                                      NetworkingNode_Id  DestinationId,
                                      NetworkPath        NetworkPath,
                                      Request_Id         RequestId,
                                      String             Action,
                                      Byte[]             Payload,
                                      DateTime?          RequestTimeout      = null,
                                      String?            ErrorMessage        = null,
                                      CancellationToken  CancellationToken   = default)
    {

        #region Data

        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        #endregion

        #region Properties

        /// <summary>
        /// The request time stamp.
        /// </summary>
        public DateTime           RequestTimestamp     { get; }      = RequestTimestamp;

        /// <summary>
        /// The event tracking identification.
        /// </summary>
        public EventTracking_Id   EventTrackingId      { get; }      = EventTrackingId;

        /// <summary>
        /// The EEBus networking mode to use.
        /// </summary>
        public NetworkingMode     NetworkingMode       { get; set; } = NetworkingMode;

        /// <summary>
        /// The networking node identification of the message destination.
        /// </summary>
        public NetworkingNode_Id  DestinationId        { get; }      = DestinationId;

        /// <summary>
        /// The (recorded) path of the request through the overlay network.
        /// </summary>
        public NetworkPath        NetworkPath          { get; }      = NetworkPath;

        /// <summary>
        /// The unique request identification.
        /// </summary>
        public Request_Id         RequestId            { get; }      = RequestId;

        /// <summary>
        /// An EEBus action/method name.
        /// </summary>
        public String             Action               { get; }      = Action;

        /// <summary>
        /// The binary request message payload.
        /// </summary>
        public Byte[]             Payload              { get; }      = Payload;

        public DateTime           RequestTimeout       { get; set; } = RequestTimeout ?? (RequestTimestamp + DefaultTimeout);

        /// <summary>
        /// The optional error message, e.g. during sending of the message.
        /// </summary>
        public String?            ErrorMessage         { get; }      = ErrorMessage;

        /// <summary>
        /// The cancellation token.
        /// </summary>
        public CancellationToken  CancellationToken    { get; }      = CancellationToken;


        /// <summary>
        /// The request message has no errors.
        /// </summary>
        public Boolean            NoErrors
            => ErrorMessage is null;

        /// <summary>
        /// The request message has errors.
        /// </summary>
        public Boolean            HasErrors
            => ErrorMessage is not null;

        #endregion


        #region (static) FromRequest(Request, SerializedRequest)

        /// <summary>
        /// Create a new EEBus binary request message transport container based on the given binary request.
        /// </summary>
        /// <param name="Request">A request message.</param>
        /// <param name="SerializedRequest">The serialized request message.</param>
        public static BinaryRequestMessage FromRequest(IRequest  Request,
                                                            Byte[]    SerializedRequest)

            => new (Timestamp.Now,
                    Request.EventTrackingId,
                    NetworkingMode.Unknown,
                    Request.DestinationNodeId,
                    Request.NetworkPath,
                    Request.RequestId,
                    Request.Action,
                    SerializedRequest,
                    Timestamp.Now + Request.RequestTimeout,
                    null,
                    Request.CancellationToken);

        #endregion


        #region TryParse(Binary, out BinaryRequestMessage, out ErrorResponse, ImplicitSourceNodeId = null)

        /// <summary>
        /// Try to parse the given binary representation of a request message.
        /// </summary>
        /// <param name="Binary">The binary to be parsed.</param>
        /// <param name="BinaryRequestMessage">The parsed EEBus WebSocket request message.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ImplicitSourceNodeId">An optional source networking node identification, e.g. from the HTTP Web Sockets connection.</param>
        public static Boolean TryParse(Byte[]                          Binary,
                                       out BinaryRequestMessage?  BinaryRequestMessage,
                                       out String?                     ErrorResponse,
                                       DateTime?                       RequestTimestamp       = null,
                                       EventTracking_Id?               EventTrackingId        = null,
                                       NetworkingNode_Id?              ImplicitSourceNodeId   = null,
                                       CancellationToken               CancellationToken      = default)
        {

            BinaryRequestMessage  = null;
            ErrorResponse         = null;

            if (Binary is null)
                return false;

            try
            {

                var stream                   = new MemoryStream(Binary);

                // MessageType: CALL (Client-to-Server)
                var messageType              = stream.ReadByte();
                if (messageType != 2)
                {
                    ErrorResponse = $"The received message type '{messageType}' is invalid!";
                    return false;
                }


                // DestinationNodeId
                var destinationNodeId        = NetworkingNode_Id.Zero;
                var destinationNodeIdLength  = stream.ReadUInt16();
                if (destinationNodeIdLength > 0)
                {

                    var destinationNodeIdText    = stream.ReadUTF8String(destinationNodeIdLength);

                    if (!NetworkingNode_Id.TryParse(destinationNodeIdText, out destinationNodeId))
                    {
                        ErrorResponse = $"The received destination node identification '{destinationNodeId}' is invalid!";
                        return false;
                    }

                }


                // NetworkPath
                var networkPathLength        = stream.ReadByte();
                var networkPath              = new List<NetworkingNode_Id>();

                for (var i=0; i<networkPathLength; i++)
                {

                    var sourceNodeIdLength   = stream.ReadUInt16();
                    var sourceNodeIdText     = stream.ReadUTF8String(sourceNodeIdLength);

                    if (!NetworkingNode_Id.TryParse(sourceNodeIdText, out var sourceNodeId))
                    {
                        ErrorResponse = $"The source node identification '{sourceNodeIdText}' at location {i+1} of the received network path is invalid!";
                        return false;
                    }

                    networkPath.Add(sourceNodeId);

                }

                if (ImplicitSourceNodeId.HasValue &&
                    ImplicitSourceNodeId.Value != NetworkingNode_Id.Zero)
                {

                    if (networkPath.Count > 0 &&
                        networkPath.Last() != ImplicitSourceNodeId)
                    {
                        networkPath.Add(ImplicitSourceNodeId.Value);
                    }

                    if (networkPath.Count == 0)
                        networkPath.Add(ImplicitSourceNodeId.Value);

                }


                // RequestId
                var requestIdLength          = stream.ReadUInt16();
                var requestIdText            = stream.ReadUTF8String(requestIdLength);

                if (!Request_Id.TryParse(requestIdText, out var requestId))
                {
                    ErrorResponse = $"The received request identification '{requestIdText}' is invalid!";
                    return false;
                }


                // Action
                var actionLength             = stream.ReadUInt16();
                var action                   = stream.ReadUTF8String(actionLength).Trim();

                if (action.IsNullOrEmpty())
                {
                    ErrorResponse = $"The received EEBus action must not be null or empty!";
                    return false;
                }


                // Payload
                var payloadLength            = stream.ReadUInt64();
                var payload                  = stream.ReadBytes(payloadLength);


                BinaryRequestMessage         = new BinaryRequestMessage(
                                                   RequestTimestamp ?? Timestamp.Now,
                                                   EventTrackingId  ?? EventTracking_Id.New,
                                                   destinationNodeIdLength == 0 && networkPathLength == 0
                                                       ? NetworkingMode.Standard
                                                       : NetworkingMode.OverlayNetwork,
                                                   destinationNodeId,
                                                   new NetworkPath(networkPath),
                                                   requestId,
                                                   action,
                                                   payload,
                                                   null,
                                                   null,
                                                   CancellationToken
                                               );

                return true;

            }
            catch (Exception e)
            {
                ErrorResponse = "The given binary representation of an EEBus WebSocket binary request message is invalid: " + e.Message;
            }

            return false;

        }

        #endregion

        #region ToByteArray()

        /// <summary>
        /// Return a binary representation of this object.
        /// </summary>
        public Byte[] ToByteArray()
        {

            var binaryStream = new MemoryStream();

            // MessageType: CALL (Client-to-Server)
            binaryStream.WriteByte(2);

            if (NetworkingMode == NetworkingMode.Standard)
            {
                // DestinationNodeId length == 0x0000
                // NetworkPath       length == 0x00
                binaryStream.Write(new Byte[3]);
            }
            else if (NetworkingMode == NetworkingMode.OverlayNetwork)
            {

                var destinationNodeIdBytes = DestinationId.ToString().ToUTF8Bytes();
                binaryStream.WriteUInt16((UInt16) destinationNodeIdBytes.Length);
                binaryStream.Write      (destinationNodeIdBytes);

                binaryStream.WriteByte  ((Byte) (NetworkPath?.Length ?? 0));

                if (NetworkPath is not null)
                {
                    foreach (var sourceNodeId in NetworkPath)
                    {
                        var sourceNodeIdBytes = sourceNodeId.ToString().ToUTF8Bytes();
                        binaryStream.WriteUInt16((UInt16) sourceNodeIdBytes.Length);
                        binaryStream.Write      (sourceNodeIdBytes);
                    }
                }

            }

            var requestIdBytes = RequestId.ToString().ToUTF8Bytes();
            binaryStream.WriteUInt16((UInt16) requestIdBytes.Length);
            binaryStream.Write      (requestIdBytes);

            var actionBytes    = Action.   ToString().ToUTF8Bytes();
            binaryStream.WriteUInt16((UInt16) actionBytes.   Length);
            binaryStream.Write      (actionBytes);

            binaryStream.WriteUInt64((UInt64) Payload.       LongLength);
            binaryStream.Write      (Payload); //ToDo: Fix me for >2 GB!

            return binaryStream.ToArray();

        }

        #endregion


        public BinaryRequestMessage ChangeDestinationNodeId(NetworkingNode_Id NewDestinationNodeId,
                                                                 NetworkPath       NewNetworkPath)

            => new (RequestTimestamp,
                    EventTrackingId,
                    NetworkingMode,
                    NewDestinationNodeId,
                    NewNetworkPath,
                    RequestId,
                    Action,
                    Payload,
                    RequestTimeout,
                    ErrorMessage,
                    CancellationToken);


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Action} ({RequestId}) => {Payload.ToBase64().SubstringMax(100)} [{Payload.Length} bytes]";

        #endregion


    }

}
