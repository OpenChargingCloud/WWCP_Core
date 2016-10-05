/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An event send whenever a reservation will be cancelled.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="Reason">The reason for the cancellation.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate void OnCancelReservationRequestDelegate(DateTime                               LogTimestamp,
                                                            DateTime                               RequestTimestamp,
                                                            Object                                 Sender,
                                                            EventTracking_Id                       EventTrackingId,
                                                            RoamingNetwork_Id                      RoamingNetworkId,
                                                            eMobilityProvider_Id                   ProviderId,
                                                            ChargingReservation_Id                 ReservationId,
                                                            ChargingReservationCancellationReason  Reason,
                                                            TimeSpan?                              RequestTimeout);


    /// <summary>
    /// Cancel a charging reservation.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="Reason">The reason for the cancellation.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<Boolean> OnCancelReservationDelegate(DateTime                               Timestamp,
                                                              CancellationToken                      CancellationToken,
                                                              EventTracking_Id                       EventTrackingId,
                                                              ChargingReservation_Id                 ReservationId,
                                                              ChargingReservationCancellationReason  Reason,
                                                              TimeSpan?                              RequestTimeout);


    /// <summary>
    /// An event send whenever a reservation was deleted.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="Reservation">The charging reservation (if known).</param>
    /// <param name="Reason">The reason for the cancellation.</param>
    public delegate void OnReservationCancelledInternalDelegate(DateTime                               LogTimestamp,
                                                                DateTime                               RequestTimestamp,
                                                                Object                                 Sender,
                                                                EventTracking_Id                       EventTrackingId,
                                                                ChargingReservation_Id                 ReservationId,
                                                                ChargingReservation                    Reservation,
                                                                ChargingReservationCancellationReason  Reason);


    /// <summary>
    /// An event send whenever a reservation was cancelled.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="Reason">The reason for the cancellation.</param>
    /// <param name="Result">The result of the reservation.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate void OnCancelReservationResponseDelegate(DateTime                               LogTimestamp,
                                                             DateTime                               RequestTimestamp,
                                                             Object                                 Sender,
                                                             EventTracking_Id                       EventTrackingId,
                                                             RoamingNetwork_Id                      RoamingNetworkId,
                                                             eMobilityProvider_Id                   ProviderId,
                                                             ChargingReservation_Id                 ReservationId,
                                                             ChargingReservationCancellationReason  Reason,
                                                             Boolean                                Result,
                                                             TimeSpan                               Runtime,
                                                             TimeSpan?                              RequestTimeout);

}