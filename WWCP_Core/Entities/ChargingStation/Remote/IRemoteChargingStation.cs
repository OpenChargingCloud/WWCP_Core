/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace org.GraphDefined.WWCP
{

    public interface IRemoteChargingStation
    {

        String EVSEOperatorDNS { get; }
        TimeSpan EVSEOperatorTimeout { get; set; }
        IEnumerable<EVSE> EVSEs { get; }
        ChargingStation_Id Id { get; }
        ChargingStationStatusType Status { get; }
        bool UseIPv4 { get; }
        bool UseIPv6 { get; }

        event CSConnectedDelegate Connected;
        event CSDisconnectedDelegate Disconnected;
        event CSEVSEOperatorTimeoutReachedDelegate EVSEOperatorTimeoutReached;
        event CSStateChangedDelegate StateChanged;

        TCPConnectResult    Connect();
        TCPDisconnectResult Disconnect();


        Task<RemoteStartEVSEResult> RemoteStart(EVSE_Id EVSEId, ChargingProduct_Id ChargingProductId, ChargingReservation_Id ReservationId, ChargingSession_Id SessionId, eMA_Id eMAId);
        Task<RemoteStopEVSEResult>  RemoteStop (EVSE_Id EVSEId, ReservationHandling ReservationHandling, ChargingSession_Id SessionId);


        bool AuthRFID(string UID);

    }

}