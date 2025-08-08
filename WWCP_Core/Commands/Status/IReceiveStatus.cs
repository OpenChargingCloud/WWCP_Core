/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    public interface IReceiveStatus
    {

        // StatusUpdate events?


        #region UpdateRoamingNetworkStatus         (RoamingNetworkStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of roaming network status updates.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdates">An enumeration of roaming network status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushRoamingNetworkStatusResult>

            UpdateRoamingNetworkStatus(IEnumerable<RoamingNetworkStatusUpdate>  RoamingNetworkStatusUpdates,

                                       DateTimeOffset?                          Timestamp           = null,
                                       EventTracking_Id?                        EventTrackingId     = null,
                                       TimeSpan?                                RequestTimeout      = null,
                                       CancellationToken                        CancellationToken   = default);

        #endregion

        #region UpdateChargingStationOperatorStatus(ChargingStationOperatorStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator status updates.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdates">An enumeration of charging station operator status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationOperatorStatusResult>

            UpdateChargingStationOperatorStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  ChargingStationOperatorStatusUpdates,

                                                DateTimeOffset?                                   Timestamp           = null,
                                                EventTracking_Id?                                 EventTrackingId     = null,
                                                TimeSpan?                                         RequestTimeout      = null,
                                                CancellationToken                                 CancellationToken   = default);

        #endregion

        #region UpdateChargingPoolStatus           (ChargingPoolStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of charging pool status updates.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdates">An enumeration of charging pool status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolStatusResult>

            UpdateChargingPoolStatus(IEnumerable<ChargingPoolStatusUpdate>  ChargingPoolStatusUpdates,

                                     DateTimeOffset?                        Timestamp           = null,
                                     EventTracking_Id?                      EventTrackingId     = null,
                                     TimeSpan?                              RequestTimeout      = null,
                                     CancellationToken                      CancellationToken   = default);

        #endregion

        #region UpdateChargingStationStatus        (ChargingStationStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of charging station status updates.
        /// </summary>
        /// <param name="ChargingStationStatusUpdates">An enumeration of charging station status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationStatusResult>

            UpdateChargingStationStatus(IEnumerable<ChargingStationStatusUpdate>  ChargingStationStatusUpdates,

                                        DateTimeOffset?                           Timestamp           = null,
                                        EventTracking_Id?                         EventTrackingId     = null,
                                        TimeSpan?                                 RequestTimeout      = null,
                                        CancellationToken                         CancellationToken   = default);

        #endregion

        #region UpdateEVSEStatus                   (EVSEStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="EVSEStatusUpdates">An enumeration of EVSE status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEStatusResult>

            UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate>  EVSEStatusUpdates,

                             DateTimeOffset?                Timestamp           = null,
                             EventTracking_Id?              EventTrackingId     = null,
                             TimeSpan?                      RequestTimeout      = null,
                             CancellationToken              CancellationToken   = default);

        #endregion


    }

}
