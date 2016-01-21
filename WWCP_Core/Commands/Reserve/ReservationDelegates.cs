/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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
using System.Collections.Generic;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{


    /// <summary>
    /// Reserve the possibility to charge anywhere within the given charging pool.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <returns>A RemoteStartResult task.</returns>
    public delegate void OnReserveEVSEDelegate(Object                   Sender,
                                               DateTime                 Timestamp,
                                               RoamingNetwork_Id        RoamingNetworkId,
                                               EVSP_Id                  ProviderId,
                                               ChargingReservation_Id   ReservationId,
                                               DateTime?                StartTime,
                                               TimeSpan?                Duration,
                                               EVSE_Id                  EVSEId,
                                               ChargingProduct_Id       ChargingProductId  = null,
                                               IEnumerable<Auth_Token>  RFIDIds            = null,
                                               IEnumerable<eMA_Id>      eMAIds             = null,
                                               IEnumerable<UInt32>      PINs               = null,
                                               EventTracking_Id         EventTrackingId    = null);

    /// <summary>
    /// Reserve the possibility to charge anywhere within the given charging pool.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <returns>A RemoteStartResult task.</returns>
    public delegate void OnEVSEReservedDelegate(Object                   Sender,
                                                DateTime                 Timestamp,
                                                RoamingNetwork_Id        RoamingNetworkId,
                                                EVSP_Id                  ProviderId,
                                                ChargingReservation_Id   ReservationId,
                                                DateTime?                StartTime,
                                                TimeSpan?                Duration,
                                                EVSE_Id                  EVSEId,
                                                ReservationResult        Result,
                                                ChargingProduct_Id       ChargingProductId  = null,
                                                IEnumerable<Auth_Token>  RFIDIds            = null,
                                                IEnumerable<eMA_Id>      eMAIds             = null,
                                                IEnumerable<UInt32>      PINs               = null,
                                                EventTracking_Id         EventTrackingId    = null);


    /// <summary>
    /// Reserve the possibility to charge anywhere within the given charging pool.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <returns>A RemoteStartResult task.</returns>
    public delegate Task<RemoteStartEVSEResult> OnDeleteReservationDelegate(DateTime                Timestamp,
                                                                            RoamingNetwork_Id       RoamingNetworkId,
                                                                            ChargingReservation_Id  ReservationId,
                                                                            CancellationToken       CancellationToken);


}


