/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Indicate a SetEVSEStatus request for the given EVSE status list.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the logging request.</param>
    /// <param name="RequestTimestamp">The timestamp of the request, as given in the request message.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="EVSEStatusList">The list of EVSE status updates.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
    public delegate Task OnSetEVSEStatusRequestDelegate(DateTimeOffset           Timestamp,
                                                        DateTimeOffset           RequestTimestamp,
                                                        Object                   Sender,
                                                        EventTracking_Id         EventTrackingId,
                                                        RoamingNetwork_Id        RoamingNetworkId,
                                                        IEnumerable<EVSEStatus>  EVSEStatusList,
                                                        TimeSpan                 RequestTimeout,
                                                        CancellationToken        CancellationToken);


    /// <summary>
    /// Indicate a SetEVSEStatus response for the given EVSE status list.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the logging request.</param>
    /// <param name="RequestTimestamp">The timestamp of the request, as given in the request message.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="EVSEStatusList">The list of EVSE status updates.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The remote start result.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
    public delegate Task OnSetEVSEStatusResponseDelegate(DateTimeOffset           Timestamp,
                                                         DateTimeOffset           RequestTimestamp,
                                                         Object                   Sender,
                                                         EventTracking_Id         EventTrackingId,
                                                         RoamingNetwork_Id        RoamingNetworkId,
                                                         IEnumerable<EVSEStatus>  EVSEStatusList,
                                                         TimeSpan                 RequestTimeout,
                                                         List<Warning>            Result,
                                                         TimeSpan                 Runtime,
                                                         CancellationToken        CancellationToken);

}
