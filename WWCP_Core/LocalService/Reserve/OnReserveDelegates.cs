/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
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

using org.GraphDefined.WWCP.LocalService;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Reserve the possibility to charge anywhere within the given charging pool.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="CurrentCallerTime">The optional current timestamp on the caller side (to sync clocks). [default: Now]</param>
    /// <param name="StartTime">The timestamp when the reservation should start. [default: Now]</param>
    /// <param name="Duration">The duration of the reservation. [default: 30 min]</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <returns>A RemoteStartResult task.</returns>
    public delegate Task<RemoteStartResultType> OnReservePoolDelegate(DateTime                Timestamp,
                                                                      RoamingNetwork_Id       RoamingNetworkId,
                                                                      ChargingReservation_Id  ReservationId,
                                                                      EVSP_Id                 ProviderId,
                                                                      eMA_Id                  eMAId,
                                                                      ChargingPool_Id         ChargingPoolId,
                                                                      ChargingProduct_Id      ChargingProductId,
                                                                      DateTime?               CurrentCallerTime,
                                                                      DateTime?               StartTime,
                                                                      TimeSpan?               Duration,
                                                                      CancellationToken       CancellationToken);


    /// <summary>
    /// Reserve the possibility to charge anywhere at the given charging station.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    /// <param name="ChargingStationId">The unique identification of a charging station.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="CurrentCallerTime">The optional current timestamp on the caller side (to sync clocks). [default: Now]</param>
    /// <param name="StartTime">The timestamp when the reservation should start. [default: Now]</param>
    /// <param name="Duration">The duration of the reservation. [default: 30 min]</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <returns>A RemoteStartResult task.</returns>
    public delegate Task<RemoteStartResultType> OnReserveStationDelegate(DateTime                Timestamp,
                                                                         RoamingNetwork_Id       RoamingNetworkId,
                                                                         ChargingReservation_Id  ReservationId,
                                                                         EVSP_Id                 ProviderId,
                                                                         eMA_Id                  eMAId,
                                                                         ChargingStation_Id      ChargingStationId,
                                                                         ChargingProduct_Id      ChargingProductId,
                                                                         DateTime?               CurrentCallerTime,
                                                                         DateTime?               StartTime,
                                                                         TimeSpan?               Duration,
                                                                         CancellationToken       CancellationToken);


    /// <summary>
    /// Reserve the given EVSE for future charging.
    /// </summary>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="StartTime">The timestamp when the reservation should start. [default: Now]</param>
    /// <param name="Duration">The duration of the reservation. [default: 30 min]</param>
    /// <returns>A RemoteStartResult task.</returns>
    public delegate Task<ReservationResult> OnReserveEVSEDelegate(CancellationToken       CancellationToken,
                                                                  DateTime                Timestamp,
                                                                  RoamingNetwork_Id       RoamingNetworkId,
                                                                  ChargingReservation_Id  ReservationId,
                                                                  EVSP_Id                 ProviderId,
                                                                  EVSE_Id                 EVSEId,
                                                                  ChargingProduct_Id      ChargingProductId,
                                                                  DateTime?               StartTime,
                                                                  TimeSpan?               Duration);


}


