/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Indicate a remote stop of the given charging session.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task OnRemoteStopRequestDelegate(DateTime               LogTimestamp,
                                                     DateTime               RequestTimestamp,
                                                     Object                 Sender,
                                                     EventTracking_Id       EventTrackingId,
                                                     RoamingNetwork_Id      RoamingNetworkId,
                                                     ChargingSession_Id     SessionId,
                                                     ReservationHandling?   ReservationHandling,
                                                     eMobilityProvider_Id?  ProviderId,
                                                     eMobilityAccount_Id?   eMAId,
                                                     TimeSpan?              RequestTimeout);

    /// <summary>
    /// Indicate a remote stop of the given charging session.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The remote stop result.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnRemoteStopResponseDelegate(DateTime               LogTimestamp,
                                                      DateTime               RequestTimestamp,
                                                      Object                 Sender,
                                                      EventTracking_Id       EventTrackingId,
                                                      RoamingNetwork_Id      RoamingNetworkId,
                                                      ChargingSession_Id     SessionId,
                                                      ReservationHandling?   ReservationHandling,
                                                      eMobilityProvider_Id?  ProviderId,
                                                      eMobilityAccount_Id?   eMAId,
                                                      TimeSpan?              RequestTimeout,
                                                      RemoteStopResult       Result,
                                                      TimeSpan               Runtime);


    /// <summary>
    /// Stop the given charging session.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<RemoteStopResult> OnRemoteStopDelegate(DateTime               Timestamp,
                                                                CancellationToken      CancellationToken,
                                                                EventTracking_Id       EventTrackingId,
                                                                ReservationHandling?   ReservationHandling,
                                                                ChargingSession_Id     SessionId,
                                                                eMobilityProvider_Id?  ProviderId,
                                                                eMobilityAccount_Id?   eMAId,
                                                                TimeSpan?              RequestTimeout);

}
