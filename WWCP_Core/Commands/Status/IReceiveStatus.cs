/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    public interface IReceiveStatus
    {

        #region UpdateEVSEStatus (StatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushStatusResult>

            UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,

                         DateTime?                      Timestamp           = null,
                         CancellationToken?             CancellationToken   = null,
                         EventTracking_Id               EventTrackingId     = null,
                         TimeSpan?                      RequestTimeout      = null);


        /// <summary>
        /// Update the given enumeration of charging station status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushStatusResult>

            UpdateStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,

                         DateTime?                                 Timestamp           = null,
                         CancellationToken?                        CancellationToken   = null,
                         EventTracking_Id                          EventTrackingId     = null,
                         TimeSpan?                                 RequestTimeout      = null);


        /// <summary>
        /// Update the given enumeration of charging pool status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging pool status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushStatusResult>

            UpdateStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,

                         DateTime?                              Timestamp           = null,
                         CancellationToken?                     CancellationToken   = null,
                         EventTracking_Id                       EventTrackingId     = null,
                         TimeSpan?                              RequestTimeout      = null);


        /// <summary>
        /// Update the given enumeration of charging station operator status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station operator status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushStatusResult>

            UpdateStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,

                         DateTime?                                         Timestamp           = null,
                         CancellationToken?                                CancellationToken   = null,
                         EventTracking_Id                                  EventTrackingId     = null,
                         TimeSpan?                                         RequestTimeout      = null);


        /// <summary>
        /// Update the given enumeration of roaming network status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of roaming network status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushStatusResult>

            UpdateStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,

                         DateTime?                                Timestamp           = null,
                         CancellationToken?                       CancellationToken   = null,
                         EventTracking_Id                         EventTrackingId     = null,
                         TimeSpan?                                RequestTimeout      = null);

        #endregion

        // Update events?

    }

}
