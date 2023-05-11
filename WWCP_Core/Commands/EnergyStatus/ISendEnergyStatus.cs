﻿/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    public interface ISendEnergyStatus
    {

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        Boolean  DisablePushEnergyStatus    { get; set; }


        #region OnEVSEStatusPush/-Pushed

        ///// <summary>
        ///// An event fired whenever new EVSE status will be send upstream.
        ///// </summary>
        //event OnPushEVSEStatusRequestDelegate   OnPushEVSEStatusRequest;

        ///// <summary>
        ///// An event fired whenever new EVSE status had been sent upstream.
        ///// </summary>
        //event OnPushEVSEStatusResponseDelegate  OnPushEVSEStatusResponse;

        #endregion


        #region UpdateEVSEEnergyStatus                   (EnergyStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="EnergyStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEEnergyStatusResult>

            UpdateEnergyStatus(IEnumerable<EVSEEnergyStatusUpdate>  EnergyStatusUpdates,
                               TransmissionTypes                    TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?                            Timestamp           = null,
                               CancellationToken                    CancellationToken   = default,
                               EventTracking_Id?                    EventTrackingId     = null,
                               TimeSpan?                            RequestTimeout      = null);

        #endregion

        //#region UpdateChargingStationEnergyStatus        (EnergyStatusUpdates, TransmissionType = Enqueue, ...)

        ///// <summary>
        ///// Update the given enumeration of charging station admin status updates.
        ///// </summary>
        ///// <param name="EnergyStatusUpdates">An enumeration of charging station admin status updates.</param>
        ///// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<PushChargingStationEnergyStatusResult>

        //    UpdateEnergyStatus(IEnumerable<ChargingStationEnergyStatusUpdate>  EnergyStatusUpdates,
        //                      TransmissionTypes                              TransmissionType    = TransmissionTypes.Enqueue,

        //                      DateTime?                                      Timestamp           = null,
        //                      CancellationToken                              CancellationToken   = default,
        //                      EventTracking_Id?                              EventTrackingId     = null,
        //                      TimeSpan?                                      RequestTimeout      = null);

        //#endregion

        //#region UpdateChargingPoolEnergyStatus           (EnergyStatusUpdates, TransmissionType = Enqueue, ...)

        ///// <summary>
        ///// Update the given enumeration of charging pool admin status updates.
        ///// </summary>
        ///// <param name="EnergyStatusUpdates">An enumeration of charging pool admin status updates.</param>
        ///// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<PushChargingPoolEnergyStatusResult>

        //    UpdateEnergyStatus(IEnumerable<ChargingPoolEnergyStatusUpdate>  EnergyStatusUpdates,
        //                      TransmissionTypes                           TransmissionType    = TransmissionTypes.Enqueue,

        //                      DateTime?                                   Timestamp           = null,
        //                      CancellationToken                           CancellationToken   = default,
        //                      EventTracking_Id?                           EventTrackingId     = null,
        //                      TimeSpan?                                   RequestTimeout      = null);

        //#endregion

        //#region UpdateChargingStationOperatorEnergyStatus(EnergyStatusUpdates, TransmissionType = Enqueue, ...)

        ///// <summary>
        ///// Update the given enumeration of charging station operator admin status updates.
        ///// </summary>
        ///// <param name="EnergyStatusUpdates">An enumeration of charging station operator admin status updates.</param>
        ///// <param name="TransmissionType">Whether to send the charging station operator admin status updates directly or enqueue it for a while.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<PushChargingStationOperatorEnergyStatusResult>

        //    UpdateEnergyStatus(IEnumerable<ChargingStationOperatorEnergyStatusUpdate>  EnergyStatusUpdates,
        //                      TransmissionTypes                                      TransmissionType    = TransmissionTypes.Enqueue,

        //                      DateTime?                                              Timestamp           = null,
        //                      CancellationToken                                      CancellationToken   = default,
        //                      EventTracking_Id?                                      EventTrackingId     = null,
        //                      TimeSpan?                                              RequestTimeout      = null);

        //#endregion

        //#region UpdateRoamingNetworkEnergyStatus         (EnergyStatusUpdates, TransmissionType = Enqueue, ...)

        ///// <summary>
        ///// Update the given enumeration of roaming network admin status updates.
        ///// </summary>
        ///// <param name="EnergyStatusUpdates">An enumeration of roaming network admin status updates.</param>
        ///// <param name="TransmissionType">Whether to send the roaming network admin status updates directly or enqueue it for a while.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<PushRoamingNetworkEnergyStatusResult>

        //    UpdateEnergyStatus(IEnumerable<RoamingNetworkEnergyStatusUpdate>  EnergyStatusUpdates,
        //                      TransmissionTypes                             TransmissionType    = TransmissionTypes.Enqueue,

        //                      DateTime?                                     Timestamp           = null,
        //                      CancellationToken                             CancellationToken   = default,
        //                      EventTracking_Id?                             EventTrackingId     = null,
        //                      TimeSpan?                                     RequestTimeout      = null);

        //#endregion


    }

}