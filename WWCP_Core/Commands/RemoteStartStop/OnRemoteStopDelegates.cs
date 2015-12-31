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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.WWCP;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Indicate a remote stop of the given charging session.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender..</param>
    public delegate void OnRemoteStopDelegate(Object               Sender,
                                              DateTime             Timestamp,
                                              RoamingNetwork_Id    RoamingNetworkId,
                                              ChargingSession_Id   SessionId,
                                              ReservationHandling  ReservationHandling,
                                              EVSP_Id              ProviderId);

    /// <summary>
    /// Indicate a remote stop of the given charging session.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="Result">The remote stop result.</param>
    public delegate void OnRemoteStoppedDelegate(Object             Sender,
                                                 DateTime           Timestamp,
                                                 RoamingNetwork_Id  RoamingNetworkId,
                                                 RemoteStopResult   Result);


    /// <summary>
    /// Initiate a remote stop of the given charging session.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender..</param>
    /// <returns>A RemoteStopResult task.</returns>
    public delegate Task<RemoteStopResult> OnRemoteStopDelegate2(DateTime             Timestamp,
                                                                 Object               Sender,
                                                                 CancellationToken    CancellationToken,
                                                                 ReservationHandling  ReservationHandling,
                                                                 ChargingSession_Id   SessionId,
                                                                 EVSP_Id              ProviderId);


    // ----------------------------------------------------------------------------------------------------------


    /// <summary>
    /// Indicate a remote stop of the given charging session at the given EVSE.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender..</param>
    public delegate void OnRemoteEVSEStopDelegate(Object               Sender,
                                                  DateTime             Timestamp,
                                                  RoamingNetwork_Id    RoamingNetworkId,
                                                  EVSE_Id              EVSEId,
                                                  ChargingSession_Id   SessionId,
                                                  ReservationHandling  ReservationHandling,
                                                  EVSP_Id              ProviderId);

    /// <summary>
    /// Indicate a remote stop of the given charging session at the given EVSE
    /// and for the given provider/eMAId.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="Result">The remote stop result.</param>
    public delegate void OnRemoteEVSEStoppedDelegate(Object                Sender,
                                                     DateTime              Timestamp,
                                                     RoamingNetwork_Id     RoamingNetworkId,
                                                     EVSE_Id               EVSEId,
                                                     RemoteStopEVSEResult  Result);


    /// <summary>
    /// Initiate a remote stop of the given charging session.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender..</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <returns>A RemoteStopResult task.</returns>
    public delegate Task<RemoteStopEVSEResult> OnRemoteStopEVSEDelegate(DateTime             Timestamp,
                                                                        Object               Sender,
                                                                        CancellationToken    CancellationToken,
                                                                        ReservationHandling  ReservationHandling,
                                                                        ChargingSession_Id   SessionId,
                                                                        EVSP_Id              ProviderId,
                                                                        EVSE_Id              EVSEId);


    // ----------------------------------------------------------------------------------------------------------


    /// <summary>
    /// Indicate a remote stop of the given charging session at the given charging station.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingStationId">The unique identification of a charging station.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender..</param>
    public delegate void OnRemoteChargingStationStopDelegate(Object               Sender,
                                                             DateTime             Timestamp,
                                                             RoamingNetwork_Id    RoamingNetworkId,
                                                             ChargingStation_Id   ChargingStationId,
                                                             ChargingSession_Id   SessionId,
                                                             ReservationHandling  ReservationHandling,
                                                             EVSP_Id              ProviderId);

    /// <summary>
    /// Indicate a remote stop of the given charging session at the given charging station
    /// and for the given provider/eMAId.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingStationId">The unique identification of a charging station.</param>
    /// <param name="Result">The remote stop result.</param>
    public delegate void OnRemoteChargingStationStoppedDelegate(Object                           Sender,
                                                                DateTime                         Timestamp,
                                                                RoamingNetwork_Id                RoamingNetworkId,
                                                                ChargingStation_Id               ChargingStationId,
                                                                RemoteStopChargingStationResult  Result);


    /// <summary>
    /// Initiate a remote stop of the given charging session.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender..</param>
    /// <param name="ChargingStationId">The unique identification of a charging station.</param>
    /// <returns>A RemoteStopResult task.</returns>
    public delegate Task<RemoteStopChargingStationResult> OnRemoteStopChargingStationDelegate(DateTime             Timestamp,
                                                                                              Object               Sender,
                                                                                              CancellationToken    CancellationToken,
                                                                                              ReservationHandling  ReservationHandling,
                                                                                              ChargingSession_Id   SessionId,
                                                                                              EVSP_Id              ProviderId,
                                                                                              ChargingStation_Id   ChargingStationId);


}


