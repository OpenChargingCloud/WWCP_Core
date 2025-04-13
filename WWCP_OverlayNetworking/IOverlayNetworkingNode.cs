/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP EEBus <https://github.com/OpenChargingCloud/WWCP_EEBus>
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
using org.GraphDefined.Vanaheimr.Hermod;

using cloud.charging.open.protocols.WWCP.OverlayNetworking;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking
{

    public interface INetworkingNodeIN //: NetworkingNode.     INetworkingNodeIncomingMessages,
                                       //  NetworkingNode.     INetworkingNodeIncomingMessageEvents,
                                       //  NetworkingNode.CS.  INetworkingNodeIncomingMessages,
                                       //  NetworkingNode.CS.  INetworkingNodeIncomingMessageEvents,
                                       //  NetworkingNode.CSMS.INetworkingNodeIncomingMessages,
                                       //  NetworkingNode.CSMS.INetworkingNodeIncomingMessageEvents
    {

        //void WireEvents(NetworkingNode.CS.  INetworkingNodeIncomingMessages IncomingMessages);
        //void WireEvents(NetworkingNode.CSMS.INetworkingNodeIncomingMessages IncomingMessages);


        #region Incoming Messages: Networking Node <- CSMS

        #region Reset

        //Task RaiseOnResetRequest (DateTime               Timestamp,
        //                          IEventSender           Sender,
        //                          IWebSocketConnection   Connection,
        //                          ResetRequest           Request);

        //Task RaiseOnResetResponse(DateTime               Timestamp,
        //                          IEventSender           Sender,
        //                          IWebSocketConnection   Connection,
        //                          ResetRequest           Request,
        //                          ResetResponse          Response,
        //                          TimeSpan               Runtime);

        #endregion

        #endregion

    }


    public interface INetworkingNodeOUT //: NetworkingNode.     INetworkingNodeOutgoingMessages,
                                        //  NetworkingNode.     INetworkingNodeOutgoingMessageEvents,
                                        //  NetworkingNode.CS.  INetworkingNodeOutgoingMessages,
                                        //  NetworkingNode.CS.  INetworkingNodeOutgoingMessageEvents,
                                        //  NetworkingNode.CSMS.INetworkingNodeOutgoingMessages,
                                        //  NetworkingNode.CSMS.INetworkingNodeOutgoingMessageEvents

    {

        #region Outgoing Message Events

        #region DataTransfer

        //Task RaiseOnDataTransferRequest (DateTime               Timestamp,
        //                                 IEventSender           Sender,
        //                                 DataTransferRequest    Request);

        //Task RaiseOnDataTransferResponse(DateTime               Timestamp,
        //                                 IEventSender           Sender,
        //                                 DataTransferRequest    Request,
        //                                 DataTransferResponse   Response,
        //                                 TimeSpan               Runtime);

        #endregion

        #region BinaryDataTransfer

        //Task RaiseOnBinaryDataTransferRequest (DateTime                     Timestamp,
        //                                       IEventSender                 Sender,
        //                                       BinaryDataTransferRequest    Request);

        //Task RaiseOnBinaryDataTransferResponse(DateTime                     Timestamp,
        //                                       IEventSender                 Sender,
        //                                       BinaryDataTransferRequest    Request,
        //                                       BinaryDataTransferResponse   Response,
        //                                       TimeSpan                     Runtime);

        #endregion

        #region NotifyNetworkTopology

        //Task RaiseOnNotifyNetworkTopologyRequest (DateTime                        Timestamp,
        //                                          IEventSender                    Sender,
        //                                          NotifyNetworkTopologyRequest    Request);

        //Task RaiseOnNotifyNetworkTopologyResponse(DateTime                        Timestamp,
        //                                          IEventSender                    Sender,
        //                                          NotifyNetworkTopologyRequest    Request,
        //                                          NotifyNetworkTopologyResponse   Response,
        //                                          TimeSpan                        Runtime);

        #endregion

        #endregion

    }


    /// <summary>
    /// The common interface of all WWCP Overlay Networking Nodes.
    /// </summary>
    public interface IOverlayNetworkingNode : IEventSender
    {

        NetworkingNode_Id           Id                       { get; }

        /// <summary>
        /// An optional multi-language networking node description.
        /// </summary>
        [Optional]
        I18NString?                 Description              { get; }

    //    IEEBusAdapter                EEBus                     { get; }

        CustomData                  CustomData               { get; }



        String? ClientCloseMessage { get; }



        Task HandleErrors(String     Module,
                          String     Caller,
                          Exception  ExceptionOccurred);

        Task HandleErrors(String     Module,
                          String     Caller,
                          String     ErrorResponse);



       // TimeSpan                   DefaultRequestTimeout    { get; }

       // Request_Id                 NextRequestId            { get; }

       //SignaturePolicy?           SignaturePolicy          { get; }


       //// IEnumerable<ICSMSChannel>  CSMSChannels             { get; }


       Byte[]  GetEncryptionKey     (NetworkingNode_Id DestinationNodeId, UInt16? KeyId = null);
       Byte[]  GetDecryptionKey     (NetworkingNode_Id SourceNodeId,      UInt16? KeyId = null);

       UInt64  GetEncryptionNonce   (NetworkingNode_Id DestinationNodeId, UInt16? KeyId = null);
       UInt64  GetEncryptionCounter (NetworkingNode_Id DestinationNodeId, UInt16? KeyId = null);


    }

}
