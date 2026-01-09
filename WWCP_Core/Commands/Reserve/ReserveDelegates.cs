/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An event sent whenever a reservation at an EVSE is being made.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="ChargingLocation">A charging location.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProduct">The charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task OnReserveRequestDelegate(DateTimeOffset                     LogTimestamp,
                                                  DateTimeOffset                     RequestTimestamp,
                                                  Object                             Sender,
                                                  EventTracking_Id                   EventTrackingId,
                                                  RoamingNetwork_Id                  RoamingNetworkId,
                                                  ChargingReservation_Id?            ReservationId,
                                                  ChargingReservation_Id?            LinkedReservationId,
                                                  ChargingLocation                   ChargingLocation,
                                                  DateTimeOffset?                    StartTime,
                                                  TimeSpan?                          Duration,
                                                  EMobilityProvider_Id?              ProviderId,
                                                  RemoteAuthentication?              RemoteAuthentication,
                                                  ChargingProduct?                   ChargingProduct,
                                                  IEnumerable<AuthenticationToken>?  AuthTokens,
                                                  IEnumerable<EMobilityAccount_Id>?  eMAIds,
                                                  IEnumerable<UInt32>?               PINs,
                                                  TimeSpan?                          RequestTimeout);


    /// <summary>
    /// An event sent whenever a reservation at an EVSE is being made.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="ChargingLocation">A charging location.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProduct">The charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<ReservationResult> OnReserveDelegate(ChargingReservation_Id?            ReservationId,
                                                              ChargingReservation_Id?            LinkedReservationId,
                                                              ChargingLocation                   ChargingLocation,
                                                              DateTimeOffset?                    StartTime,
                                                              TimeSpan?                          Duration,
                                                              EMobilityProvider_Id?              ProviderId,
                                                              RemoteAuthentication               RemoteAuthentication,
                                                              ChargingProduct?                   ChargingProduct,
                                                              IEnumerable<AuthenticationToken>?  AuthTokens,
                                                              IEnumerable<EMobilityAccount_Id>?  eMAIds,
                                                              IEnumerable<UInt32>?               PINs,

                                                              DateTimeOffset?                    Timestamp           = null,
                                                              CancellationToken?                 CancellationToken   = null,
                                                              EventTracking_Id?                  EventTrackingId     = null,
                                                              TimeSpan?                          RequestTimeout      = null);


    /// <summary>
    /// A delegate called whenever a charging reservation was created.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="Sender">The sender of the event.</param>
    /// <param name="Reservation">The new charging reservation.</param>
    public delegate void OnNewReservationDelegate(DateTimeOffset       Timestamp,
                                                  Object               Sender,
                                                  ChargingReservation  Reservation);


    /// <summary>
    /// An event sent whenever a reservation at an EVSE was made.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="ChargingLocation">A charging location.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProduct">The charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="Result">The result of the reservation.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task OnReserveResponseDelegate(DateTimeOffset                     LogTimestamp,
                                                   DateTimeOffset                     RequestTimestamp,
                                                   Object                             Sender,
                                                   EventTracking_Id                   EventTrackingId,
                                                   RoamingNetwork_Id                  RoamingNetworkId,
                                                   ChargingReservation_Id?            ReservationId,
                                                   ChargingReservation_Id?            LinkedReservationId,
                                                   ChargingLocation                   ChargingLocation,
                                                   DateTimeOffset?                    StartTime,
                                                   TimeSpan?                          Duration,
                                                   EMobilityProvider_Id?              ProviderId,
                                                   RemoteAuthentication?              RemoteAuthentication,
                                                   ChargingProduct?                   ChargingProduct,
                                                   IEnumerable<AuthenticationToken>?  AuthTokens,
                                                   IEnumerable<EMobilityAccount_Id>?  eMAIds,
                                                   IEnumerable<UInt32>?               PINs,
                                                   ReservationResult                  Result,
                                                   TimeSpan                           Runtime,
                                                   TimeSpan?                          RequestTimeout);

}