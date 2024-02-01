/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    public interface IReceiveEnergyStatus
    {

        // EnergyUpdate events?


        #region UpdateChargingPoolEnergyStatus   (ChargingPoolEnergyStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of charging pool energy status.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdates">An enumeration of charging pool energy status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolEnergyStatusResult>

            UpdateChargingPoolEnergyStatus(IEnumerable<ChargingPoolEnergyStatusUpdate>  ChargingPoolEnergyStatusUpdates,

                                           DateTime?                                    Timestamp           = null,
                                           EventTracking_Id?                            EventTrackingId     = null,
                                           TimeSpan?                                    RequestTimeout      = null,
                                           CancellationToken                            CancellationToken   = default);

        #endregion

        #region UpdateChargingStationEnergyStatus(ChargingStationEnergyStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of charging station energy status.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdates">An enumeration of charging station energy status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationEnergyStatusResult>

            UpdateChargingStationEnergyStatus(IEnumerable<ChargingStationEnergyStatusUpdate>  ChargingStationEnergyStatusUpdates,

                                              DateTime?                                       Timestamp           = null,
                                              EventTracking_Id?                               EventTrackingId     = null,
                                              TimeSpan?                                       RequestTimeout      = null,
                                              CancellationToken                               CancellationToken   = default);

        #endregion

        #region UpdateEVSEEnergyStatus           (EVSEEnergyStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of EVSE energy status.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdates">An enumeration of EVSE energy status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEEnergyStatusResult>

            UpdateEVSEEnergyStatus(IEnumerable<EVSEEnergyStatusUpdate>  EVSEEnergyStatusUpdates,

                                   DateTime?                            Timestamp           = null,
                                   EventTracking_Id?                    EventTrackingId     = null,
                                   TimeSpan?                            RequestTimeout      = null,
                                   CancellationToken                    CancellationToken   = default);

        #endregion


    }

}
