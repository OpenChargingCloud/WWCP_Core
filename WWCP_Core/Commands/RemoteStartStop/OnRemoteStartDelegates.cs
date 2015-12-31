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
    /// Indicate a remote start of the given charging session at the given EVSE
    /// and for the given provider/eMAId.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="ReservationId">The unique identification for a charging reservation.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    public delegate void OnRemoteEVSEStartDelegate(Object                  Sender,
                                                   DateTime                Timestamp,
                                                   RoamingNetwork_Id       RoamingNetworkId,
                                                   EVSE_Id                 EVSEId,
                                                   ChargingProduct_Id      ChargingProductId,
                                                   ChargingReservation_Id  ReservationId,
                                                   ChargingSession_Id      SessionId,
                                                   EVSP_Id                 ProviderId,
                                                   eMA_Id                  eMAId);

    /// <summary>
    /// Indicate a remote start of the given charging session at the given EVSE
    /// and for the given provider/eMAId.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="Result">The remote start result.</param>
    public delegate void OnRemoteEVSEStartedDelegate(Object                 Sender,
                                                     DateTime               Timestamp,
                                                     RoamingNetwork_Id      RoamingNetworkId,
                                                     EVSE_Id                EVSEId,
                                                     RemoteStartEVSEResult  Result);



    /// <summary>
    /// Initiate a remote start of the given charging session at the given EVSE
    /// and for the given provider/eMAId.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="ReservationId">The unique identification for a charging reservation.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <returns>A RemoteStartResult task.</returns>
    public delegate Task<RemoteStartEVSEResult> OnRemoteStartEVSEDelegate(DateTime                Timestamp,
                                                                          Object                  Sender,
                                                                          CancellationToken       CancellationToken,
                                                                          EVSE_Id                 EVSEId,
                                                                          ChargingProduct_Id      ChargingProductId,
                                                                          ChargingReservation_Id  ReservationId,
                                                                          ChargingSession_Id      SessionId,
                                                                          EVSP_Id                 ProviderId,
                                                                          eMA_Id                  eMAId);


    // ----------------------------------------------------------------------------------------------------------


    /// <summary>
    /// Indicate a remote start of the given charging session at the given charging station
    /// and for the given provider/eMAId.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingStationId">The unique identification of a charging station.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="ReservationId">The unique identification for a charging reservation.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    public delegate void OnRemoteChargingStationStartDelegate(Object                  Sender,
                                                              DateTime                Timestamp,
                                                              RoamingNetwork_Id       RoamingNetworkId,
                                                              ChargingStation_Id      ChargingStationId,
                                                              ChargingProduct_Id      ChargingProductId,
                                                              ChargingReservation_Id  ReservationId,
                                                              ChargingSession_Id      SessionId,
                                                              EVSP_Id                 ProviderId,
                                                              eMA_Id                  eMAId);

    /// <summary>
    /// Indicate a remote start of the given charging session at the given charging station
    /// and for the given provider/eMAId.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingStationId">The unique identification of an charging station.</param>
    /// <param name="Result">The remote start result.</param>
    public delegate void OnRemoteChargingStationStartedDelegate(Object                            Sender,
                                                                DateTime                          Timestamp,
                                                                RoamingNetwork_Id                 RoamingNetworkId,
                                                                ChargingStation_Id                ChargingStationId,
                                                                RemoteStartChargingStationResult  Result);


    /// <summary>
    /// Initiate a remote start of the given charging session at the given charging station
    /// and for the given provider/eMAId.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="ChargingStationId">The unique identification of a charging station.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="ReservationId">The unique identification for a charging reservation.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    /// <returns>A RemoteStartResult task.</returns>
    public delegate Task<RemoteStartChargingStationResult> OnRemoteStartChargingStationDelegate(DateTime                Timestamp,
                                                                                                Object                  Sender,
                                                                                                CancellationToken       CancellationToken,
                                                                                                ChargingStation_Id      ChargingStationId,
                                                                                                ChargingProduct_Id      ChargingProductId,
                                                                                                ChargingReservation_Id  ReservationId,
                                                                                                ChargingSession_Id      SessionId,
                                                                                                EVSP_Id                 ProviderId,
                                                                                                eMA_Id                  eMAId);

}


