/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    ///// <summary>
    ///// Extention method for the IRemotePushStatus interface.
    ///// </summary>
    //public static class IRemotePushStatusExtentions
    //{

    //    #region UpdateEVSEAdminStatus(this IRemotePushStatus, EVSEAdminStatusUpdate,    TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given EVSE admin status update.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="EVSEAdminStatusUpdate">An EVSE admin status update to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                              EVSEAdminStatusUpdate   EVSEAdminStatusUpdate,
    //                              TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                              DateTime?               Timestamp           = null,
    //                              CancellationToken?      CancellationToken   = null,
    //                              EventTracking_Id        EventTrackingId     = null,
    //                              TimeSpan?               RequestTimeout      = null)


    //        => await IRemotePushStatus.UpdateEVSEAdminStatus(new EVSEAdminStatusUpdate[] { EVSEAdminStatusUpdate },
    //                                                         TransmissionType,

    //                                                         Timestamp,
    //                                                         CancellationToken,
    //                                                         EventTrackingId,
    //                                                         RequestTimeout).

    //                                                         ConfigureAwait(false);

    //    #endregion

    //    #region UpdateEVSEAdminStatus(this IRemotePushStatus, EVSE,                     TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the status of the given EVSE.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="EVSE">An EVSE to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                              EVSE                    EVSE,
    //                              TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                              DateTime?               Timestamp           = null,
    //                              CancellationToken?      CancellationToken   = null,
    //                              EventTracking_Id        EventTrackingId     = null,
    //                              TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (EVSE == null)
    //            throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEAdminStatus(EVSEAdminStatusUpdate.Snapshot(EVSE),
    //                                                             TransmissionType,

    //                                                             Timestamp,
    //                                                             CancellationToken,
    //                                                             EventTrackingId,
    //                                                             RequestTimeout).

    //                                                             ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEAdminStatus(this IRemotePushStatus, EVSEs,                    TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given enumeration of EVSE admin status.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="EVSEs">An enumeration of EVSEs to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                              IEnumerable<EVSE>       EVSEs,
    //                              TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                              DateTime?               Timestamp           = null,
    //                              CancellationToken?      CancellationToken   = null,
    //                              EventTracking_Id        EventTrackingId     = null,
    //                              TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (EVSEs == null)
    //            throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEAdminStatus(EVSEs.SafeSelect(evse => EVSEAdminStatusUpdate.Snapshot(evse)),
    //                                                             TransmissionType,

    //                                                             Timestamp,
    //                                                             CancellationToken,
    //                                                             EventTrackingId,
    //                                                             RequestTimeout).

    //                                                             ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEAdminStatus(this IRemotePushStatus, ChargingStation,          TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE admin status of the given charging station.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStation">A charging station to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                              ChargingStation         ChargingStation,
    //                              TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                              DateTime?               Timestamp           = null,
    //                              CancellationToken?      CancellationToken   = null,
    //                              EventTracking_Id        EventTrackingId     = null,
    //                              TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStation == null)
    //            throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEAdminStatus(ChargingStation.EVSEs.SafeSelect(evse => EVSEAdminStatusUpdate.Snapshot(evse)),
    //                                                             TransmissionType,

    //                                                             Timestamp,
    //                                                             CancellationToken,
    //                                                             EventTrackingId,
    //                                                             RequestTimeout).

    //                                                             ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEAdminStatus(this IRemotePushStatus, ChargingStations,         TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE admin status of the given enumeration of charging station.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStations">An enumeration of charging stations to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEAdminStatus(this IRemotePushStatus        IRemotePushStatus,
    //                              IEnumerable<ChargingStation>  ChargingStations,
    //                              TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueued,

    //                              DateTime?                     Timestamp           = null,
    //                              CancellationToken?            CancellationToken   = null,
    //                              EventTracking_Id              EventTrackingId     = null,
    //                              TimeSpan?                     RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStations == null)
    //            throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEAdminStatus(ChargingStations.
    //                                                                 SafeSelectMany(station => station.EVSEs).
    //                                                                 SafeSelect    (evse    => EVSEAdminStatusUpdate.Snapshot(evse)),
    //                                                             TransmissionType,

    //                                                             Timestamp,
    //                                                             CancellationToken,
    //                                                             EventTrackingId,
    //                                                             RequestTimeout).

    //                                                             ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEAdminStatus(this IRemotePushStatus, ChargingPool,             TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE admin status of the given charging pool.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPool">A charging pool to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                              ChargingPool            ChargingPool,
    //                              TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                              DateTime?               Timestamp           = null,
    //                              CancellationToken?      CancellationToken   = null,
    //                              EventTracking_Id        EventTrackingId     = null,
    //                              TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPool == null)
    //            throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEAdminStatus(ChargingPool.EVSEs.SafeSelect(evse => EVSEAdminStatusUpdate.Snapshot(evse)),
    //                                                             TransmissionType,

    //                                                             Timestamp,
    //                                                             CancellationToken,
    //                                                             EventTrackingId,
    //                                                             RequestTimeout).

    //                                                             ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEAdminStatus(this IRemotePushStatus, ChargingPools,            TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE admin status of the given enumeration of charging pools.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPools">An enumeration of charging pools to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEAdminStatus(this IRemotePushStatus     IRemotePushStatus,
    //                              IEnumerable<ChargingPool>  ChargingPools,
    //                              TransmissionTypes          TransmissionType    = TransmissionTypes.Enqueued,

    //                              DateTime?                  Timestamp           = null,
    //                              CancellationToken?         CancellationToken   = null,
    //                              EventTracking_Id           EventTrackingId     = null,
    //                              TimeSpan?                  RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPools == null)
    //            throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEAdminStatus(ChargingPools.
    //                                                                 SafeSelectMany(pool => pool.EVSEs).
    //                                                                 SafeSelect    (evse => EVSEAdminStatusUpdate.Snapshot(evse)),
    //                                                             TransmissionType,

    //                                                             Timestamp,
    //                                                             CancellationToken,
    //                                                             EventTrackingId,
    //                                                             RequestTimeout).

    //                                                             ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEAdminStatus(this IRemotePushStatus, ChargingStationOperator,  TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE admin status of the given charging station operator.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperator">A charging station operator to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEAdminStatus(this IRemotePushStatus   IRemotePushStatus,
    //                              ChargingStationOperator  ChargingStationOperator,
    //                              TransmissionTypes        TransmissionType    = TransmissionTypes.Enqueued,

    //                              DateTime?                Timestamp           = null,
    //                              CancellationToken?       CancellationToken   = null,
    //                              EventTracking_Id         EventTrackingId     = null,
    //                              TimeSpan?                RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperator == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEAdminStatus(ChargingStationOperator.EVSEs.SafeSelect(evse => EVSEAdminStatusUpdate.Snapshot(evse)),
    //                                                             TransmissionType,

    //                                                             Timestamp,
    //                                                             CancellationToken,
    //                                                             EventTrackingId,
    //                                                             RequestTimeout).

    //                                                             ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEAdminStatus(this IRemotePushStatus, ChargingStationOperators, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE admin status of the given enumeration of charging station operators.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperators">An enumeration of charging station operators to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEAdminStatus(this IRemotePushStatus                IRemotePushStatus,
    //                              IEnumerable<ChargingStationOperator>  ChargingStationOperators,
    //                              TransmissionTypes                     TransmissionType    = TransmissionTypes.Enqueued,

    //                              DateTime?                             Timestamp           = null,
    //                              CancellationToken?                    CancellationToken   = null,
    //                              EventTracking_Id                      EventTrackingId     = null,
    //                              TimeSpan?                             RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperators == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEAdminStatus(ChargingStationOperators.
    //                                                                 SafeSelectMany(cso  => cso.EVSEs).
    //                                                                 SafeSelect    (evse => EVSEAdminStatusUpdate.Snapshot(evse)),
    //                                                             TransmissionType,

    //                                                             Timestamp,
    //                                                             CancellationToken,
    //                                                             EventTrackingId,
    //                                                             RequestTimeout).

    //                                                             ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEAdminStatus(this IRemotePushStatus, RoamingNetwork,           TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE admin status of the given roaming network.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetwork">A roaming network to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                              RoamingNetwork          RoamingNetwork,
    //                              TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                              DateTime?               Timestamp           = null,
    //                              CancellationToken?      CancellationToken   = null,
    //                              EventTracking_Id        EventTrackingId     = null,
    //                              TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (RoamingNetwork == null)
    //            throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEAdminStatus(RoamingNetwork.EVSEs.SafeSelect(evse => EVSEAdminStatusUpdate.Snapshot(evse)),
    //                                                             TransmissionType,

    //                                                             Timestamp,
    //                                                             CancellationToken,
    //                                                             EventTrackingId,
    //                                                             RequestTimeout).

    //                                                             ConfigureAwait(false);

    //    }

    //    #endregion


    //    #region UpdateEVSEStatus(this IRemotePushStatus, EVSEStatusUpdate,         TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given EVSE status update.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="EVSEStatusUpdate">An EVSE status update to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEStatus(this IRemotePushStatus  IRemotePushStatus,
    //                         EVSEStatusUpdate        EVSEStatusUpdate,
    //                         TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                         DateTime?               Timestamp           = null,
    //                         CancellationToken?      CancellationToken   = null,
    //                         EventTracking_Id        EventTrackingId     = null,
    //                         TimeSpan?               RequestTimeout      = null)


    //        => await IRemotePushStatus.UpdateEVSEStatus(new EVSEStatusUpdate[] { EVSEStatusUpdate },
    //                                                    TransmissionType,

    //                                                    Timestamp,
    //                                                    CancellationToken,
    //                                                    EventTrackingId,
    //                                                    RequestTimeout).

    //                                                    ConfigureAwait(false);

    //    #endregion

    //    #region UpdateEVSEStatus(this IRemotePushStatus, EVSE,                     TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the status of the given EVSE.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="EVSE">An EVSE to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEStatus(this IRemotePushStatus  IRemotePushStatus,
    //                         EVSE                    EVSE,
    //                         TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                         DateTime?               Timestamp           = null,
    //                         CancellationToken?      CancellationToken   = null,
    //                         EventTracking_Id        EventTrackingId     = null,
    //                         TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (EVSE == null)
    //            throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEStatus(EVSEStatusUpdate.Snapshot(EVSE),
    //                                                        TransmissionType,

    //                                                        Timestamp,
    //                                                        CancellationToken,
    //                                                        EventTrackingId,
    //                                                        RequestTimeout).

    //                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEStatus(this IRemotePushStatus, EVSEs,                    TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given enumeration of EVSE status.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="EVSEs">An enumeration of EVSEs to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEStatus(this IRemotePushStatus  IRemotePushStatus,
    //                         IEnumerable<EVSE>       EVSEs,
    //                         TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                         DateTime?               Timestamp           = null,
    //                         CancellationToken?      CancellationToken   = null,
    //                         EventTracking_Id        EventTrackingId     = null,
    //                         TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (EVSEs == null)
    //            throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEStatus(EVSEs.SafeSelect(evse => EVSEStatusUpdate.Snapshot(evse)),
    //                                                        TransmissionType,

    //                                                        Timestamp,
    //                                                        CancellationToken,
    //                                                        EventTrackingId,
    //                                                        RequestTimeout).

    //                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEStatus(this IRemotePushStatus, ChargingStation,          TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE status of the given charging station.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStation">A charging station to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEStatus(this IRemotePushStatus  IRemotePushStatus,
    //                         ChargingStation         ChargingStation,
    //                         TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                         DateTime?               Timestamp           = null,
    //                         CancellationToken?      CancellationToken   = null,
    //                         EventTracking_Id        EventTrackingId     = null,
    //                         TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStation == null)
    //            throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEStatus(ChargingStation.EVSEs.SafeSelect(evse => EVSEStatusUpdate.Snapshot(evse)),
    //                                                        TransmissionType,

    //                                                        Timestamp,
    //                                                        CancellationToken,
    //                                                        EventTrackingId,
    //                                                        RequestTimeout).

    //                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEStatus(this IRemotePushStatus, ChargingStations,         TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE status of the given enumeration of charging station.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStations">An enumeration of charging stations to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEStatus(this IRemotePushStatus        IRemotePushStatus,
    //                         IEnumerable<ChargingStation>  ChargingStations,
    //                         TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueued,

    //                         DateTime?                     Timestamp           = null,
    //                         CancellationToken?            CancellationToken   = null,
    //                         EventTracking_Id              EventTrackingId     = null,
    //                         TimeSpan?                     RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStations == null)
    //            throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEStatus(ChargingStations.
    //                                                            SafeSelectMany(station => station.EVSEs).
    //                                                            SafeSelect    (evse    => EVSEStatusUpdate.Snapshot(evse)),
    //                                                        TransmissionType,

    //                                                        Timestamp,
    //                                                        CancellationToken,
    //                                                        EventTrackingId,
    //                                                        RequestTimeout).

    //                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEStatus(this IRemotePushStatus, ChargingPool,             TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE status of the given charging pool.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPool">A charging pool to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEStatus(this IRemotePushStatus  IRemotePushStatus,
    //                         ChargingPool            ChargingPool,
    //                         TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                         DateTime?               Timestamp           = null,
    //                         CancellationToken?      CancellationToken   = null,
    //                         EventTracking_Id        EventTrackingId     = null,
    //                         TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPool == null)
    //            throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEStatus(ChargingPool.EVSEs.SafeSelect(evse => EVSEStatusUpdate.Snapshot(evse)),
    //                                                        TransmissionType,

    //                                                        Timestamp,
    //                                                        CancellationToken,
    //                                                        EventTrackingId,
    //                                                        RequestTimeout).

    //                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEStatus(this IRemotePushStatus, ChargingPools,            TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE status of the given enumeration of charging pools.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPools">An enumeration of charging pools to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEStatus(this IRemotePushStatus     IRemotePushStatus,
    //                         IEnumerable<ChargingPool>  ChargingPools,
    //                         TransmissionTypes          TransmissionType    = TransmissionTypes.Enqueued,

    //                         DateTime?                  Timestamp           = null,
    //                         CancellationToken?         CancellationToken   = null,
    //                         EventTracking_Id           EventTrackingId     = null,
    //                         TimeSpan?                  RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPools == null)
    //            throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEStatus(ChargingPools.
    //                                                            SafeSelectMany(pool => pool.EVSEs).
    //                                                            SafeSelect    (evse => EVSEStatusUpdate.Snapshot(evse)),
    //                                                        TransmissionType,

    //                                                        Timestamp,
    //                                                        CancellationToken,
    //                                                        EventTrackingId,
    //                                                        RequestTimeout).

    //                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEStatus(this IRemotePushStatus, ChargingStationOperator,  TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE status of the given charging station operator.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperator">A charging station operator to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEStatus(this IRemotePushStatus   IRemotePushStatus,
    //                         ChargingStationOperator  ChargingStationOperator,
    //                         TransmissionTypes        TransmissionType    = TransmissionTypes.Enqueued,

    //                         DateTime?                Timestamp           = null,
    //                         CancellationToken?       CancellationToken   = null,
    //                         EventTracking_Id         EventTrackingId     = null,
    //                         TimeSpan?                RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperator == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEStatus(ChargingStationOperator.EVSEs.SafeSelect(evse => EVSEStatusUpdate.Snapshot(evse)),
    //                                                        TransmissionType,

    //                                                        Timestamp,
    //                                                        CancellationToken,
    //                                                        EventTrackingId,
    //                                                        RequestTimeout).

    //                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEStatus(this IRemotePushStatus, ChargingStationOperators, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE status of the given enumeration of charging station operators.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperators">An enumeration of charging station operators to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEStatus(this IRemotePushStatus                IRemotePushStatus,
    //                         IEnumerable<ChargingStationOperator>  ChargingStationOperators,
    //                         TransmissionTypes                     TransmissionType    = TransmissionTypes.Enqueued,

    //                         DateTime?                             Timestamp           = null,
    //                         CancellationToken?                    CancellationToken   = null,
    //                         EventTracking_Id                      EventTrackingId     = null,
    //                         TimeSpan?                             RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperators == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEStatus(ChargingStationOperators.
    //                                                            SafeSelectMany(cso  => cso.EVSEs).
    //                                                            SafeSelect    (evse => EVSEStatusUpdate.Snapshot(evse)),
    //                                                        TransmissionType,

    //                                                        Timestamp,
    //                                                        CancellationToken,
    //                                                        EventTrackingId,
    //                                                        RequestTimeout).

    //                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateEVSEStatus(this IRemotePushStatus, RoamingNetwork,           TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all EVSE status of the given roaming network.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetwork">A roaming network to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the EVSE status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateEVSEStatus(this IRemotePushStatus  IRemotePushStatus,
    //                         RoamingNetwork          RoamingNetwork,
    //                         TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                         DateTime?               Timestamp           = null,
    //                         CancellationToken?      CancellationToken   = null,
    //                         EventTracking_Id        EventTrackingId     = null,
    //                         TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (RoamingNetwork == null)
    //            throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateEVSEStatus(RoamingNetwork.EVSEs.SafeSelect(evse => EVSEStatusUpdate.Snapshot(evse)),
    //                                                        TransmissionType,

    //                                                        Timestamp,
    //                                                        CancellationToken,
    //                                                        EventTrackingId,
    //                                                        RequestTimeout).

    //                                                        ConfigureAwait(false);

    //    }

    //    #endregion



    //    #region UpdateChargingStationAdminStatus(this IRemotePushStatus, ChargingStationAdminStatusUpdate, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given charging station admin status update.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationAdminStatusUpdate">A charging station admin status update to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationAdminStatus(this IRemotePushStatus            IRemotePushStatus,
    //                                         ChargingStationAdminStatusUpdate  ChargingStationAdminStatusUpdate,
    //                                         TransmissionTypes                 TransmissionType    = TransmissionTypes.Enqueued,

    //                                         DateTime?                         Timestamp           = null,
    //                                         CancellationToken?                CancellationToken   = null,
    //                                         EventTracking_Id                  EventTrackingId     = null,
    //                                         TimeSpan?                         RequestTimeout      = null)


    //        => await IRemotePushStatus.UpdateChargingStationAdminStatus(new ChargingStationAdminStatusUpdate[] { ChargingStationAdminStatusUpdate },
    //                                                                    TransmissionType,

    //                                                                    Timestamp,
    //                                                                    CancellationToken,
    //                                                                    EventTrackingId,
    //                                                                    RequestTimeout).

    //                                                                    ConfigureAwait(false);

    //    #endregion

    //    #region UpdateChargingStationAdminStatus(this IRemotePushStatus, ChargingStation,                  TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the status of the given charging station.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStation">A charging station to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                         ChargingStation         ChargingStation,
    //                                         TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                         DateTime?               Timestamp           = null,
    //                                         CancellationToken?      CancellationToken   = null,
    //                                         EventTracking_Id        EventTrackingId     = null,
    //                                         TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStation == null)
    //            throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationAdminStatus(ChargingStationAdminStatusUpdate.Snapshot(ChargingStation),
    //                                                                        TransmissionType,

    //                                                                        Timestamp,
    //                                                                        CancellationToken,
    //                                                                        EventTrackingId,
    //                                                                        RequestTimeout).

    //                                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationAdminStatus(this IRemotePushStatus, ChargingStations,                 TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given enumeration of charging station admin status.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStations">An enumeration of charging stations to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationAdminStatus(this IRemotePushStatus        IRemotePushStatus,
    //                                         IEnumerable<ChargingStation>  ChargingStations,
    //                                         TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueued,

    //                                         DateTime?                     Timestamp           = null,
    //                                         CancellationToken?            CancellationToken   = null,
    //                                         EventTracking_Id              EventTrackingId     = null,
    //                                         TimeSpan?                     RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStations == null)
    //            throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationAdminStatus(ChargingStations.SafeSelect(station => ChargingStationAdminStatusUpdate.Snapshot(station)),
    //                                                                        TransmissionType,

    //                                                                        Timestamp,
    //                                                                        CancellationToken,
    //                                                                        EventTrackingId,
    //                                                                        RequestTimeout).

    //                                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationAdminStatus(this IRemotePushStatus, ChargingPool,                     TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station admin status of the given charging pool.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPool">A charging pool to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                         ChargingPool            ChargingPool,
    //                                         TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                         DateTime?               Timestamp           = null,
    //                                         CancellationToken?      CancellationToken   = null,
    //                                         EventTracking_Id        EventTrackingId     = null,
    //                                         TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPool == null)
    //            throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationAdminStatus(ChargingPool.ChargingStations.SafeSelect(station => ChargingStationAdminStatusUpdate.Snapshot(station)),
    //                                                                        TransmissionType,

    //                                                                        Timestamp,
    //                                                                        CancellationToken,
    //                                                                        EventTrackingId,
    //                                                                        RequestTimeout).

    //                                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationAdminStatus(this IRemotePushStatus, ChargingPools,                    TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station admin status of the given enumeration of charging pools.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPools">An enumeration of charging pools to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationAdminStatus(this IRemotePushStatus     IRemotePushStatus,
    //                                         IEnumerable<ChargingPool>  ChargingPools,
    //                                         TransmissionTypes          TransmissionType    = TransmissionTypes.Enqueued,

    //                                         DateTime?                  Timestamp           = null,
    //                                         CancellationToken?         CancellationToken   = null,
    //                                         EventTracking_Id           EventTrackingId     = null,
    //                                         TimeSpan?                  RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPools == null)
    //            throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationAdminStatus(ChargingPools.
    //                                                                            SafeSelectMany(pool    => pool.ChargingStations).
    //                                                                            SafeSelect    (station => ChargingStationAdminStatusUpdate.Snapshot(station)),
    //                                                                        TransmissionType,

    //                                                                        Timestamp,
    //                                                                        CancellationToken,
    //                                                                        EventTrackingId,
    //                                                                        RequestTimeout).

    //                                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationAdminStatus(this IRemotePushStatus, ChargingStationOperator,          TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station admin status of the given charging station operator.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperator">A charging station operator to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationAdminStatus(this IRemotePushStatus   IRemotePushStatus,
    //                                         ChargingStationOperator  ChargingStationOperator,
    //                                         TransmissionTypes        TransmissionType    = TransmissionTypes.Enqueued,

    //                                         DateTime?                Timestamp           = null,
    //                                         CancellationToken?       CancellationToken   = null,
    //                                         EventTracking_Id         EventTrackingId     = null,
    //                                         TimeSpan?                RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperator == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationAdminStatus(ChargingStationOperator.ChargingStations.SafeSelect(station => ChargingStationAdminStatusUpdate.Snapshot(station)),
    //                                                                        TransmissionType,

    //                                                                        Timestamp,
    //                                                                        CancellationToken,
    //                                                                        EventTrackingId,
    //                                                                        RequestTimeout).

    //                                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationAdminStatus(this IRemotePushStatus, ChargingStationOperators,         TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station admin status of the given enumeration of charging station operators.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperators">An enumeration of charging station operators to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationAdminStatus(this IRemotePushStatus                IRemotePushStatus,
    //                                         IEnumerable<ChargingStationOperator>  ChargingStationOperators,
    //                                         TransmissionTypes                     TransmissionType    = TransmissionTypes.Enqueued,

    //                                         DateTime?                             Timestamp           = null,
    //                                         CancellationToken?                    CancellationToken   = null,
    //                                         EventTracking_Id                      EventTrackingId     = null,
    //                                         TimeSpan?                             RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperators == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationAdminStatus(ChargingStationOperators.
    //                                                                            SafeSelectMany(cso     => cso.ChargingStations).
    //                                                                            SafeSelect    (station => ChargingStationAdminStatusUpdate.Snapshot(station)),
    //                                                                        TransmissionType,

    //                                                                        Timestamp,
    //                                                                        CancellationToken,
    //                                                                        EventTrackingId,
    //                                                                        RequestTimeout).

    //                                                                        ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationAdminStatus(this IRemotePushStatus, RoamingNetwork,                   TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station admin status of the given roaming network.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetwork">A roaming network to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                         RoamingNetwork          RoamingNetwork,
    //                                         TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                         DateTime?               Timestamp           = null,
    //                                         CancellationToken?      CancellationToken   = null,
    //                                         EventTracking_Id        EventTrackingId     = null,
    //                                         TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (RoamingNetwork == null)
    //            throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationAdminStatus(RoamingNetwork.ChargingStations.SafeSelect(station => ChargingStationAdminStatusUpdate.Snapshot(station)),
    //                                                                        TransmissionType,

    //                                                                        Timestamp,
    //                                                                        CancellationToken,
    //                                                                        EventTrackingId,
    //                                                                        RequestTimeout).

    //                                                                        ConfigureAwait(false);

    //    }

    //    #endregion


    //    #region UpdateChargingStationStatus(this IRemotePushStatus, ChargingStationStatusUpdate, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given charging station status update.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationStatusUpdate">A charging station status update to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationStatus(this IRemotePushStatus       IRemotePushStatus,
    //                                    ChargingStationStatusUpdate  ChargingStationStatusUpdate,
    //                                    TransmissionTypes            TransmissionType    = TransmissionTypes.Enqueued,

    //                                    DateTime?                    Timestamp           = null,
    //                                    CancellationToken?           CancellationToken   = null,
    //                                    EventTracking_Id             EventTrackingId     = null,
    //                                    TimeSpan?                    RequestTimeout      = null)


    //        => await IRemotePushStatus.UpdateChargingStationStatus(new ChargingStationStatusUpdate[] { ChargingStationStatusUpdate },
    //                                                               TransmissionType,

    //                                                               Timestamp,
    //                                                               CancellationToken,
    //                                                               EventTrackingId,
    //                                                               RequestTimeout).

    //                                                               ConfigureAwait(false);

    //    #endregion

    //    #region UpdateChargingStationStatus(this IRemotePushStatus, ChargingStation,             TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the status of the given charging station.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStation">A charging station to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                    ChargingStation         ChargingStation,
    //                                    TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                    DateTime?               Timestamp           = null,
    //                                    CancellationToken?      CancellationToken   = null,
    //                                    EventTracking_Id        EventTrackingId     = null,
    //                                    TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStation == null)
    //            throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationStatus(ChargingStationStatusUpdate.Snapshot(ChargingStation),
    //                                                                   TransmissionType,

    //                                                                   Timestamp,
    //                                                                   CancellationToken,
    //                                                                   EventTrackingId,
    //                                                                   RequestTimeout).

    //                                                                   ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationStatus(this IRemotePushStatus, ChargingStations,            TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given enumeration of charging station status.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStations">An enumeration of charging stations to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationStatus(this IRemotePushStatus        IRemotePushStatus,
    //                                    IEnumerable<ChargingStation>  ChargingStations,
    //                                    TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueued,

    //                                    DateTime?                     Timestamp           = null,
    //                                    CancellationToken?            CancellationToken   = null,
    //                                    EventTracking_Id              EventTrackingId     = null,
    //                                    TimeSpan?                     RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStations == null)
    //            throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationStatus(ChargingStations.SafeSelect(station => ChargingStationStatusUpdate.Snapshot(station)),
    //                                                                   TransmissionType,

    //                                                                   Timestamp,
    //                                                                   CancellationToken,
    //                                                                   EventTrackingId,
    //                                                                   RequestTimeout).

    //                                                                   ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationStatus(this IRemotePushStatus, ChargingPool,                TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station status of the given charging pool.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPool">A charging pool to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                    ChargingPool            ChargingPool,
    //                                    TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                    DateTime?               Timestamp           = null,
    //                                    CancellationToken?      CancellationToken   = null,
    //                                    EventTracking_Id        EventTrackingId     = null,
    //                                    TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPool == null)
    //            throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationStatus(ChargingPool.ChargingStations.SafeSelect(station => ChargingStationStatusUpdate.Snapshot(station)),
    //                                                                   TransmissionType,

    //                                                                   Timestamp,
    //                                                                   CancellationToken,
    //                                                                   EventTrackingId,
    //                                                                   RequestTimeout).

    //                                                                   ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationStatus(this IRemotePushStatus, ChargingPools,               TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station status of the given enumeration of charging pools.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPools">An enumeration of charging pools to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationStatus(this IRemotePushStatus     IRemotePushStatus,
    //                                    IEnumerable<ChargingPool>  ChargingPools,
    //                                    TransmissionTypes          TransmissionType    = TransmissionTypes.Enqueued,

    //                                    DateTime?                  Timestamp           = null,
    //                                    CancellationToken?         CancellationToken   = null,
    //                                    EventTracking_Id           EventTrackingId     = null,
    //                                    TimeSpan?                  RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPools == null)
    //            throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationStatus(ChargingPools.
    //                                                                       SafeSelectMany(pool    => pool.ChargingStations).
    //                                                                       SafeSelect    (station => ChargingStationStatusUpdate.Snapshot(station)),
    //                                                                   TransmissionType,

    //                                                                   Timestamp,
    //                                                                   CancellationToken,
    //                                                                   EventTrackingId,
    //                                                                   RequestTimeout).

    //                                                                   ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationStatus(this IRemotePushStatus, ChargingStationOperator,     TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station status of the given charging station operator.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperator">A charging station operator to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationStatus(this IRemotePushStatus   IRemotePushStatus,
    //                                    ChargingStationOperator  ChargingStationOperator,
    //                                    TransmissionTypes        TransmissionType    = TransmissionTypes.Enqueued,

    //                                    DateTime?                Timestamp           = null,
    //                                    CancellationToken?       CancellationToken   = null,
    //                                    EventTracking_Id         EventTrackingId     = null,
    //                                    TimeSpan?                RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperator == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationStatus(ChargingStationOperator.ChargingStations.SafeSelect(station => ChargingStationStatusUpdate.Snapshot(station)),
    //                                                                   TransmissionType,

    //                                                                   Timestamp,
    //                                                                   CancellationToken,
    //                                                                   EventTrackingId,
    //                                                                   RequestTimeout).

    //                                                                   ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationStatus(this IRemotePushStatus, ChargingStationOperators,    TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station status of the given enumeration of charging station operators.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperators">An enumeration of charging station operators to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationStatus(this IRemotePushStatus                IRemotePushStatus,
    //                                    IEnumerable<ChargingStationOperator>  ChargingStationOperators,
    //                                    TransmissionTypes                     TransmissionType    = TransmissionTypes.Enqueued,

    //                                    DateTime?                             Timestamp           = null,
    //                                    CancellationToken?                    CancellationToken   = null,
    //                                    EventTracking_Id                      EventTrackingId     = null,
    //                                    TimeSpan?                             RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperators == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationStatus(ChargingStationOperators.
    //                                                                       SafeSelectMany(cso     => cso.ChargingStations).
    //                                                                       SafeSelect    (station => ChargingStationStatusUpdate.Snapshot(station)),
    //                                                                   TransmissionType,

    //                                                                   Timestamp,
    //                                                                   CancellationToken,
    //                                                                   EventTrackingId,
    //                                                                   RequestTimeout).

    //                                                                   ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationStatus(this IRemotePushStatus, RoamingNetwork,              TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station status of the given roaming network.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetwork">A roaming network to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                    RoamingNetwork          RoamingNetwork,
    //                                    TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                    DateTime?               Timestamp           = null,
    //                                    CancellationToken?      CancellationToken   = null,
    //                                    EventTracking_Id        EventTrackingId     = null,
    //                                    TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (RoamingNetwork == null)
    //            throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationStatus(RoamingNetwork.ChargingStations.SafeSelect(station => ChargingStationStatusUpdate.Snapshot(station)),
    //                                                                   TransmissionType,

    //                                                                   Timestamp,
    //                                                                   CancellationToken,
    //                                                                   EventTrackingId,
    //                                                                   RequestTimeout).

    //                                                                   ConfigureAwait(false);

    //    }

    //    #endregion



    //    #region UpdateChargingPoolAdminStatus(this IRemotePushStatus, ChargingPoolAdminStatusUpdate, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given charging pool admin status update.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPoolAdminStatusUpdate">A charging pool admin status update to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging pool admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolAdminStatus(this IRemotePushStatus         IRemotePushStatus,
    //                                      ChargingPoolAdminStatusUpdate  ChargingPoolAdminStatusUpdate,
    //                                      TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueued,

    //                                      DateTime?                      Timestamp           = null,
    //                                      CancellationToken?             CancellationToken   = null,
    //                                      EventTracking_Id               EventTrackingId     = null,
    //                                      TimeSpan?                      RequestTimeout      = null)


    //        => await IRemotePushStatus.UpdateChargingPoolAdminStatus(new ChargingPoolAdminStatusUpdate[] { ChargingPoolAdminStatusUpdate },
    //                                                                 TransmissionType,

    //                                                                 Timestamp,
    //                                                                 CancellationToken,
    //                                                                 EventTrackingId,
    //                                                                 RequestTimeout).

    //                                                                 ConfigureAwait(false);

    //    #endregion

    //    #region UpdateChargingPoolAdminStatus(this IRemotePushStatus, ChargingPool,                  TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the admin status of the given charging pool.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPool">A charging pool to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging pool status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                      ChargingPool            ChargingPool,
    //                                      TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                      DateTime?               Timestamp           = null,
    //                                      CancellationToken?      CancellationToken   = null,
    //                                      EventTracking_Id        EventTrackingId     = null,
    //                                      TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPool == null)
    //            throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingPoolAdminStatus(ChargingPoolAdminStatusUpdate.Snapshot(ChargingPool),
    //                                                                     TransmissionType,

    //                                                                     Timestamp,
    //                                                                     CancellationToken,
    //                                                                     EventTrackingId,
    //                                                                     RequestTimeout).

    //                                                                     ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingPoolAdminStatus(this IRemotePushStatus, ChargingPools,                 TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given enumeration of charging pool admin status.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPools">An enumeration of charging pools to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolAdminStatus(this IRemotePushStatus     IRemotePushStatus,
    //                                      IEnumerable<ChargingPool>  ChargingPools,
    //                                      TransmissionTypes          TransmissionType    = TransmissionTypes.Enqueued,

    //                                      DateTime?                  Timestamp           = null,
    //                                      CancellationToken?         CancellationToken   = null,
    //                                      EventTracking_Id           EventTrackingId     = null,
    //                                      TimeSpan?                  RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPools == null)
    //            throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingPoolAdminStatus(ChargingPools.SafeSelect(pool => ChargingPoolAdminStatusUpdate.Snapshot(pool)),
    //                                                                     TransmissionType,

    //                                                                     Timestamp,
    //                                                                     CancellationToken,
    //                                                                     EventTrackingId,
    //                                                                     RequestTimeout).

    //                                                                     ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingPoolAdminStatus(this IRemotePushStatus, ChargingStationOperator,       TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging pool admin status of the given charging station operator.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperator">A charging station operator to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station operator admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolAdminStatus(this IRemotePushStatus   IRemotePushStatus,
    //                                      ChargingStationOperator  ChargingStationOperator,
    //                                      TransmissionTypes        TransmissionType    = TransmissionTypes.Enqueued,

    //                                      DateTime?                Timestamp           = null,
    //                                      CancellationToken?       CancellationToken   = null,
    //                                      EventTracking_Id         EventTrackingId     = null,
    //                                      TimeSpan?                RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperator == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingPoolAdminStatus(ChargingStationOperator.ChargingPools.SafeSelect(pool => ChargingPoolAdminStatusUpdate.Snapshot(pool)),
    //                                                                     TransmissionType,

    //                                                                     Timestamp,
    //                                                                     CancellationToken,
    //                                                                     EventTrackingId,
    //                                                                     RequestTimeout).

    //                                                                     ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingPoolAdminStatus(this IRemotePushStatus, ChargingStationOperators,      TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging pool admin status of the given enumeration of charging station operators.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperators">An enumeration of charging station operators to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolAdminStatus(this IRemotePushStatus                IRemotePushStatus,
    //                                      IEnumerable<ChargingStationOperator>  ChargingStationOperators,
    //                                      TransmissionTypes                     TransmissionType    = TransmissionTypes.Enqueued,

    //                                      DateTime?                             Timestamp           = null,
    //                                      CancellationToken?                    CancellationToken   = null,
    //                                      EventTracking_Id                      EventTrackingId     = null,
    //                                      TimeSpan?                             RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperators == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingPoolAdminStatus(ChargingStationOperators.
    //                                                                         SafeSelectMany(cso  => cso.ChargingPools).
    //                                                                         SafeSelect    (pool => ChargingPoolAdminStatusUpdate.Snapshot(pool)),
    //                                                                     TransmissionType,

    //                                                                     Timestamp,
    //                                                                     CancellationToken,
    //                                                                     EventTrackingId,
    //                                                                     RequestTimeout).

    //                                                                     ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingPoolAdminStatus(this IRemotePushStatus, RoamingNetwork,                TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging pool admin status of the given roaming network.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetwork">A roaming network to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging pool admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                      RoamingNetwork          RoamingNetwork,
    //                                      TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                      DateTime?               Timestamp           = null,
    //                                      CancellationToken?      CancellationToken   = null,
    //                                      EventTracking_Id        EventTrackingId     = null,
    //                                      TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (RoamingNetwork == null)
    //            throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingPoolAdminStatus(RoamingNetwork.ChargingPools.SafeSelect(pool => ChargingPoolAdminStatusUpdate.Snapshot(pool)),
    //                                                                     TransmissionType,

    //                                                                     Timestamp,
    //                                                                     CancellationToken,
    //                                                                     EventTrackingId,
    //                                                                     RequestTimeout).

    //                                                                     ConfigureAwait(false);

    //    }

    //    #endregion


    //    #region UpdateChargingPoolStatus(this IRemotePushStatus, ChargingPoolStatusUpdate, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given charging pool status update.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPoolStatusUpdate">A charging pool status update to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging pool status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolStatus(this IRemotePushStatus    IRemotePushStatus,
    //                                 ChargingPoolStatusUpdate  ChargingPoolStatusUpdate,
    //                                 TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueued,

    //                                 DateTime?                 Timestamp           = null,
    //                                 CancellationToken?        CancellationToken   = null,
    //                                 EventTracking_Id          EventTrackingId     = null,
    //                                 TimeSpan?                 RequestTimeout      = null)


    //        => await IRemotePushStatus.UpdateChargingPoolStatus(new ChargingPoolStatusUpdate[] { ChargingPoolStatusUpdate },
    //                                                            TransmissionType,

    //                                                            Timestamp,
    //                                                            CancellationToken,
    //                                                            EventTrackingId,
    //                                                            RequestTimeout).

    //                                                            ConfigureAwait(false);

    //    #endregion

    //    #region UpdateChargingPoolStatus(this IRemotePushStatus, ChargingPool,             TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the status of the given charging pool.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPool">A charging pool to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging pool status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                 ChargingPool            ChargingPool,
    //                                 TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                 DateTime?               Timestamp           = null,
    //                                 CancellationToken?      CancellationToken   = null,
    //                                 EventTracking_Id        EventTrackingId     = null,
    //                                 TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPool == null)
    //            throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingPoolStatus(ChargingPoolStatusUpdate.Snapshot(ChargingPool),
    //                                                                TransmissionType,

    //                                                                Timestamp,
    //                                                                CancellationToken,
    //                                                                EventTrackingId,
    //                                                                RequestTimeout).

    //                                                                ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingPoolStatus(this IRemotePushStatus, ChargingPools,            TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given enumeration of charging pool status.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingPools">An enumeration of charging pools to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging pool status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolStatus(this IRemotePushStatus     IRemotePushStatus,
    //                                 IEnumerable<ChargingPool>  ChargingPools,
    //                                 TransmissionTypes          TransmissionType    = TransmissionTypes.Enqueued,

    //                                 DateTime?                  Timestamp           = null,
    //                                 CancellationToken?         CancellationToken   = null,
    //                                 EventTracking_Id           EventTrackingId     = null,
    //                                 TimeSpan?                  RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingPools == null)
    //            throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingPoolStatus(ChargingPools.SafeSelect(pool => ChargingPoolStatusUpdate.Snapshot(pool)),
    //                                                                TransmissionType,

    //                                                                Timestamp,
    //                                                                CancellationToken,
    //                                                                EventTrackingId,
    //                                                                RequestTimeout).

    //                                                                ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingPoolStatus(this IRemotePushStatus, ChargingStationOperator,  TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging pool status of the given charging station operator.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperator">A charging station operator to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station operator status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolStatus(this IRemotePushStatus   IRemotePushStatus,
    //                                 ChargingStationOperator  ChargingStationOperator,
    //                                 TransmissionTypes        TransmissionType    = TransmissionTypes.Enqueued,

    //                                 DateTime?                Timestamp           = null,
    //                                 CancellationToken?       CancellationToken   = null,
    //                                 EventTracking_Id         EventTrackingId     = null,
    //                                 TimeSpan?                RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperator == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingPoolStatus(ChargingStationOperator.ChargingPools.SafeSelect(pool => ChargingPoolStatusUpdate.Snapshot(pool)),
    //                                                                TransmissionType,

    //                                                                Timestamp,
    //                                                                CancellationToken,
    //                                                                EventTrackingId,
    //                                                                RequestTimeout).

    //                                                                ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingPoolStatus(this IRemotePushStatus, ChargingStationOperators, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging pool status of the given enumeration of charging station operators.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperators">An enumeration of charging station operators to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging pool status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolStatus(this IRemotePushStatus                IRemotePushStatus,
    //                                 IEnumerable<ChargingStationOperator>  ChargingStationOperators,
    //                                 TransmissionTypes                     TransmissionType    = TransmissionTypes.Enqueued,

    //                                 DateTime?                             Timestamp           = null,
    //                                 CancellationToken?                    CancellationToken   = null,
    //                                 EventTracking_Id                      EventTrackingId     = null,
    //                                 TimeSpan?                             RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperators == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingPoolStatus(ChargingStationOperators.
    //                                                                    SafeSelectMany(cso  => cso.ChargingPools).
    //                                                                    SafeSelect    (pool => ChargingPoolStatusUpdate.Snapshot(pool)),
    //                                                                TransmissionType,

    //                                                                Timestamp,
    //                                                                CancellationToken,
    //                                                                EventTrackingId,
    //                                                                RequestTimeout).

    //                                                                ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingPoolStatus(this IRemotePushStatus, RoamingNetwork,           TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging pool status of the given roaming network.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetwork">A roaming network to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging pool status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingPoolStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                 RoamingNetwork          RoamingNetwork,
    //                                 TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                 DateTime?               Timestamp           = null,
    //                                 CancellationToken?      CancellationToken   = null,
    //                                 EventTracking_Id        EventTrackingId     = null,
    //                                 TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (RoamingNetwork == null)
    //            throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingPoolStatus(RoamingNetwork.ChargingPools.SafeSelect(pool => ChargingPoolStatusUpdate.Snapshot(pool)),
    //                                                                TransmissionType,

    //                                                                Timestamp,
    //                                                                CancellationToken,
    //                                                                EventTrackingId,
    //                                                                RequestTimeout).

    //                                                                ConfigureAwait(false);

    //    }

    //    #endregion



    //    #region UpdateChargingStationOperatorAdminStatus(this IRemotePushStatus, ChargingStationOperatorAdminStatusUpdate, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given charging station operator admin status update.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperatorAdminStatusUpdate">A charging station operator admin status update to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station operator admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationOperatorAdminStatus(this IRemotePushStatus                    IRemotePushStatus,
    //                                                 ChargingStationOperatorAdminStatusUpdate  ChargingStationOperatorAdminStatusUpdate,
    //                                                 TransmissionTypes                         TransmissionType    = TransmissionTypes.Enqueued,

    //                                                 DateTime?                                 Timestamp           = null,
    //                                                 CancellationToken?                        CancellationToken   = null,
    //                                                 EventTracking_Id                          EventTrackingId     = null,
    //                                                 TimeSpan?                                 RequestTimeout      = null)


    //        => await IRemotePushStatus.UpdateChargingStationOperatorAdminStatus(new ChargingStationOperatorAdminStatusUpdate[] { ChargingStationOperatorAdminStatusUpdate },
    //                                                                    TransmissionType,

    //                                                                    Timestamp,
    //                                                                    CancellationToken,
    //                                                                    EventTrackingId,
    //                                                                    RequestTimeout).

    //                                                                    ConfigureAwait(false);

    //    #endregion

    //    #region UpdateChargingStationOperatorAdminStatus(this IRemotePushStatus, ChargingStationOperator,                  TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the admin status of the given charging station operator.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperator">A charging station operator to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station operator status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationOperatorAdminStatus(this IRemotePushStatus   IRemotePushStatus,
    //                                                 ChargingStationOperator  ChargingStationOperator,
    //                                                 TransmissionTypes        TransmissionType    = TransmissionTypes.Enqueued,

    //                                                 DateTime?                Timestamp           = null,
    //                                                 CancellationToken?       CancellationToken   = null,
    //                                                 EventTracking_Id         EventTrackingId     = null,
    //                                                 TimeSpan?                RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperator == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationOperatorAdminStatus(ChargingStationOperatorAdminStatusUpdate.Snapshot(ChargingStationOperator),
    //                                                                                TransmissionType,

    //                                                                                Timestamp,
    //                                                                                CancellationToken,
    //                                                                                EventTrackingId,
    //                                                                                RequestTimeout).

    //                                                                                ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationOperatorAdminStatus(this IRemotePushStatus, ChargingStationOperators,                 TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given enumeration of charging station operator admin status.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperators">An enumeration of charging station operators to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station operator admin status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationOperatorAdminStatus(this IRemotePushStatus                IRemotePushStatus,
    //                                                 IEnumerable<ChargingStationOperator>  ChargingStationOperators,
    //                                                 TransmissionTypes                     TransmissionType    = TransmissionTypes.Enqueued,

    //                                                 DateTime?                             Timestamp           = null,
    //                                                 CancellationToken?                    CancellationToken   = null,
    //                                                 EventTracking_Id                      EventTrackingId     = null,
    //                                                 TimeSpan?                             RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperators == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationOperatorAdminStatus(ChargingStationOperators.SafeSelect(cso => ChargingStationOperatorAdminStatusUpdate.Snapshot(cso)),
    //                                                                                TransmissionType,

    //                                                                                Timestamp,
    //                                                                                CancellationToken,
    //                                                                                EventTrackingId,
    //                                                                                RequestTimeout).

    //                                                                                ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationOperatorAdminStatus(this IRemotePushStatus, RoamingNetwork,                           TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station operator admin status of the given roaming network.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetwork">A roaming network to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station operator admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationOperatorAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                                 RoamingNetwork          RoamingNetwork,
    //                                                 TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                                 DateTime?               Timestamp           = null,
    //                                                 CancellationToken?      CancellationToken   = null,
    //                                                 EventTracking_Id        EventTrackingId     = null,
    //                                                 TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (RoamingNetwork == null)
    //            throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationOperatorAdminStatus(RoamingNetwork.ChargingStationOperators.SafeSelect(cso => ChargingStationOperatorAdminStatusUpdate.Snapshot(cso)),
    //                                                                                TransmissionType,

    //                                                                                Timestamp,
    //                                                                                CancellationToken,
    //                                                                                EventTrackingId,
    //                                                                                RequestTimeout).

    //                                                                                ConfigureAwait(false);

    //    }

    //    #endregion


    //    #region UpdateChargingStationOperatorStatus(this IRemotePushStatus, ChargingStationOperatorStatusUpdate, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given charging station operator status update.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperatorStatusUpdate">A charging station operator status update to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station operator status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationOperatorStatus(this IRemotePushStatus               IRemotePushStatus,
    //                                            ChargingStationOperatorStatusUpdate  ChargingStationOperatorStatusUpdate,
    //                                            TransmissionTypes                    TransmissionType    = TransmissionTypes.Enqueued,

    //                                            DateTime?                            Timestamp           = null,
    //                                            CancellationToken?                   CancellationToken   = null,
    //                                            EventTracking_Id                     EventTrackingId     = null,
    //                                            TimeSpan?                            RequestTimeout      = null)


    //        => await IRemotePushStatus.UpdateChargingStationOperatorStatus(new ChargingStationOperatorStatusUpdate[] { ChargingStationOperatorStatusUpdate },
    //                                                                       TransmissionType,

    //                                                                       Timestamp,
    //                                                                       CancellationToken,
    //                                                                       EventTrackingId,
    //                                                                       RequestTimeout).

    //                                                                       ConfigureAwait(false);

    //    #endregion

    //    #region UpdateChargingStationOperatorStatus(this IRemotePushStatus, ChargingStationOperator,             TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the status of the given charging station operator.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperator">A charging station operator to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station operator status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationOperatorStatus(this IRemotePushStatus   IRemotePushStatus,
    //                                            ChargingStationOperator  ChargingStationOperator,
    //                                            TransmissionTypes        TransmissionType    = TransmissionTypes.Enqueued,

    //                                            DateTime?                Timestamp           = null,
    //                                            CancellationToken?       CancellationToken   = null,
    //                                            EventTracking_Id         EventTrackingId     = null,
    //                                            TimeSpan?                RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperator == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationOperatorStatus(ChargingStationOperatorStatusUpdate.Snapshot(ChargingStationOperator),
    //                                                                           TransmissionType,

    //                                                                           Timestamp,
    //                                                                           CancellationToken,
    //                                                                           EventTrackingId,
    //                                                                           RequestTimeout).

    //                                                                           ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationOperatorStatus(this IRemotePushStatus, ChargingStationOperators,            TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given enumeration of charging station operator status.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="ChargingStationOperators">An enumeration of charging station operators to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station operator status updates directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationOperatorStatus(this IRemotePushStatus                IRemotePushStatus,
    //                                            IEnumerable<ChargingStationOperator>  ChargingStationOperators,
    //                                            TransmissionTypes                     TransmissionType    = TransmissionTypes.Enqueued,

    //                                            DateTime?                             Timestamp           = null,
    //                                            CancellationToken?                    CancellationToken   = null,
    //                                            EventTracking_Id                      EventTrackingId     = null,
    //                                            TimeSpan?                             RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (ChargingStationOperators == null)
    //            throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationOperatorStatus(ChargingStationOperators.SafeSelect(cso => ChargingStationOperatorStatusUpdate.Snapshot(cso)),
    //                                                                           TransmissionType,

    //                                                                           Timestamp,
    //                                                                           CancellationToken,
    //                                                                           EventTrackingId,
    //                                                                           RequestTimeout).

    //                                                                           ConfigureAwait(false);

    //    }

    //    #endregion

    //    #region UpdateChargingStationOperatorStatus(this IRemotePushStatus, RoamingNetwork,                      TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update all charging station operator status of the given roaming network.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetwork">A roaming network to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the charging station operator status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateChargingStationOperatorStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                            RoamingNetwork          RoamingNetwork,
    //                                            TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                            DateTime?               Timestamp           = null,
    //                                            CancellationToken?      CancellationToken   = null,
    //                                            EventTracking_Id        EventTrackingId     = null,
    //                                            TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (RoamingNetwork == null)
    //            throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateChargingStationOperatorStatus(RoamingNetwork.ChargingStationOperators.SafeSelect(cso => ChargingStationOperatorStatusUpdate.Snapshot(cso)),
    //                                                                           TransmissionType,

    //                                                                           Timestamp,
    //                                                                           CancellationToken,
    //                                                                           EventTrackingId,
    //                                                                           RequestTimeout).

    //                                                                           ConfigureAwait(false);

    //    }

    //    #endregion



    //    #region UpdateRoamingNetworkAdminStatus(this IRemotePushStatus, RoamingNetworkAdminStatusUpdate, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given roaming network admin status update.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetworkAdminStatusUpdate">A roaming network admin status update to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the roaming network admin status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateRoamingNetworkAdminStatus(this IRemotePushStatus           IRemotePushStatus,
    //                                        RoamingNetworkAdminStatusUpdate  RoamingNetworkAdminStatusUpdate,
    //                                        TransmissionTypes                TransmissionType    = TransmissionTypes.Enqueued,

    //                                        DateTime?                        Timestamp           = null,
    //                                        CancellationToken?               CancellationToken   = null,
    //                                        EventTracking_Id                 EventTrackingId     = null,
    //                                        TimeSpan?                        RequestTimeout      = null)


    //        => await IRemotePushStatus.UpdateRoamingNetworkAdminStatus(new RoamingNetworkAdminStatusUpdate[] { RoamingNetworkAdminStatusUpdate },
    //                                                                   TransmissionType,

    //                                                                   Timestamp,
    //                                                                   CancellationToken,
    //                                                                   EventTrackingId,
    //                                                                   RequestTimeout).

    //                                                                   ConfigureAwait(false);

    //    #endregion

    //    #region UpdateRoamingNetworkAdminStatus(this IRemotePushStatus, RoamingNetwork,                  TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the admin status of the given roaming network.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetwork">A roaming network to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the roaming network status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateRoamingNetworkAdminStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                        RoamingNetwork          RoamingNetwork,
    //                                        TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                        DateTime?               Timestamp           = null,
    //                                        CancellationToken?      CancellationToken   = null,
    //                                        EventTracking_Id        EventTrackingId     = null,
    //                                        TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (RoamingNetwork == null)
    //            throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateRoamingNetworkAdminStatus(RoamingNetworkAdminStatusUpdate.Snapshot(RoamingNetwork),
    //                                                                       TransmissionType,

    //                                                                       Timestamp,
    //                                                                       CancellationToken,
    //                                                                       EventTrackingId,
    //                                                                       RequestTimeout).

    //                                                                       ConfigureAwait(false);

    //    }

    //    #endregion


    //    #region UpdateRoamingNetworkStatus(this IRemotePushStatus, RoamingNetworkStatusUpdate, TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the given roaming network status update.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetworkStatusUpdate">A roaming network status update to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the roaming network status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateRoamingNetworkStatus(this IRemotePushStatus      IRemotePushStatus,
    //                                   RoamingNetworkStatusUpdate  RoamingNetworkStatusUpdate,
    //                                   TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueued,

    //                                   DateTime?                   Timestamp           = null,
    //                                   CancellationToken?          CancellationToken   = null,
    //                                   EventTracking_Id            EventTrackingId     = null,
    //                                   TimeSpan?                   RequestTimeout      = null)


    //        => await IRemotePushStatus.UpdateRoamingNetworkStatus(new RoamingNetworkStatusUpdate[] { RoamingNetworkStatusUpdate },
    //                                                              TransmissionType,

    //                                                              Timestamp,
    //                                                              CancellationToken,
    //                                                              EventTrackingId,
    //                                                              RequestTimeout).

    //                                                              ConfigureAwait(false);

    //    #endregion

    //    #region UpdateRoamingNetworkStatus(this IRemotePushStatus, RoamingNetwork,             TransmissionType = Enqueued, ...)

    //    /// <summary>
    //    /// Update the status of the given roaming network.
    //    /// </summary>
    //    /// <param name="IRemotePushStatus">A class implementing the IRemotePushStatus interface.</param>
    //    /// <param name="RoamingNetwork">A roaming network to upload.</param>
    //    /// <param name="TransmissionType">Whether to send the roaming network status update directly or enqueue it for a while.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public static async Task<Acknowledgement>

    //        UpdateRoamingNetworkStatus(this IRemotePushStatus  IRemotePushStatus,
    //                                   RoamingNetwork          RoamingNetwork,
    //                                   TransmissionTypes       TransmissionType    = TransmissionTypes.Enqueued,

    //                                   DateTime?               Timestamp           = null,
    //                                   CancellationToken?      CancellationToken   = null,
    //                                   EventTracking_Id        EventTrackingId     = null,
    //                                   TimeSpan?               RequestTimeout      = null)

    //    {

    //        #region Initial checks

    //        if (RoamingNetwork == null)
    //            throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

    //        #endregion

    //        return await IRemotePushStatus.UpdateRoamingNetworkStatus(RoamingNetworkStatusUpdate.Snapshot(RoamingNetwork),
    //                                                                  TransmissionType,

    //                                                                  Timestamp,
    //                                                                  CancellationToken,
    //                                                                  EventTrackingId,
    //                                                                  RequestTimeout).

    //                                                                  ConfigureAwait(false);

    //    }

    //    #endregion


    //}

}
