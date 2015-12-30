/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading;

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



        Task<ReservationResult> Reserve(DateTime                 Timestamp,
                                        CancellationToken        CancellationToken,
                                        EVSP_Id                  ProviderId,
                                        ChargingReservation_Id   ReservationId,
                                        DateTime?                StartTime,
                                        TimeSpan?                Duration,
                                        ChargingProduct_Id       ChargingProductId  = null,
                                        IEnumerable<Auth_Token>  RFIDIds            = null,
                                        IEnumerable<eMA_Id>      eMAIds             = null,
                                        IEnumerable<UInt32>      PINs               = null);



        /// <summary>
        /// Initiate a remote start of the given charging session at the given charging station
        /// and for the given provider/eMAId.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <returns>A RemoteStartResult task.</returns>
        Task<RemoteStartChargingStationResult> RemoteStart(DateTime                Timestamp,
                                                           CancellationToken       CancellationToken,
                                                           ChargingStation_Id      ChargingStationId,
                                                           ChargingProduct_Id      ChargingProductId,
                                                           ChargingReservation_Id  ReservationId,
                                                           ChargingSession_Id      SessionId,
                                                           eMA_Id                  eMAId);

        /// <summary>
        /// Initiate a remote start of the given charging session at the given EVSE
        /// and for the given Provider/eMAId.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <returns>A RemoteStartResult task.</returns>
        Task<RemoteStartEVSEResult> RemoteStart(DateTime                Timestamp,
                                                CancellationToken       CancellationToken,
                                                EVSE_Id                 EVSEId,
                                                ChargingProduct_Id      ChargingProductId,
                                                ChargingReservation_Id  ReservationId,
                                                ChargingSession_Id      SessionId,
                                                eMA_Id                  eMAId);

        /// <summary>
        /// Initiate a remote stop of the given charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <returns>A RemoteStopResult task.</returns>
        Task<RemoteStopChargingStationResult> RemoteStop(DateTime             Timestamp,
                                                         CancellationToken    CancellationToken,
                                                         ChargingStation_Id   ChargingStationId,
                                                         ReservationHandling  ReservationHandling,
                                                         ChargingSession_Id   SessionId);

        /// <summary>
        /// Initiate a remote stop of the given charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <returns>A RemoteStopResult task.</returns>
        Task<RemoteStopEVSEResult> RemoteStop(DateTime             Timestamp,
                                              CancellationToken    CancellationToken,
                                              EVSE_Id              EVSEId,
                                              ReservationHandling  ReservationHandling,
                                              ChargingSession_Id   SessionId);


        Boolean AuthenticateToken(Auth_Token AuthToken);

    }

}