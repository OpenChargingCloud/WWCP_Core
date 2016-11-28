/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    /// <summary>
    /// The EV Roaming Provider provided EVSE Operator services interface.
    /// </summary>
    public interface IPushStatus
    {

        // Events

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


        //#region PushEVSEStatus

        ///// <summary>
        ///// Upload the EVSE status of the given lookup of EVSE status types grouped by their Charging Station Operator.
        ///// </summary>
        ///// <param name="GroupedEVSEStatus">A lookup of EVSE status grouped by their Charging Station Operator.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(ILookup<ChargingStationOperator, EVSEStatus>  GroupedEVSEStatus,
        //                   ActionType                                    ActionType         = ActionType.update,

        //                   DateTime?                                     Timestamp          = null,
        //                   CancellationToken?                            CancellationToken  = null,
        //                   EventTracking_Id                              EventTrackingId    = null,
        //                   TimeSpan?                                     RequestTimeout     = null);

        ///// <summary>
        ///// Upload the given EVSE status.
        ///// </summary>
        ///// <param name="EVSEStatus">An EVSE status.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(EVSEStatus          EVSEStatus,
        //                   ActionType          ActionType         = ActionType.update,

        //                   DateTime?           Timestamp          = null,
        //                   CancellationToken?  CancellationToken  = null,
        //                   EventTracking_Id    EventTrackingId    = null,
        //                   TimeSpan?           RequestTimeout     = null);

        ///// <summary>
        ///// Upload the given enumeration of EVSE status.
        ///// </summary>
        ///// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// <param name="OperatorId">An optional unique identification of the Charging Station Operator.</param>
        ///// <param name="OperatorName">The optional name of the Charging Station Operator.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(IEnumerable<EVSEStatus>  EVSEStatus,
        //                   ActionType               ActionType         = ActionType.update,

        //                   DateTime?                Timestamp          = null,
        //                   CancellationToken?       CancellationToken  = null,
        //                   EventTracking_Id         EventTrackingId    = null,
        //                   TimeSpan?                RequestTimeout     = null);

        ///// <summary>
        ///// Upload the EVSE status of the given EVSE.
        ///// </summary>
        ///// <param name="EVSE">An EVSE.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(EVSE                 EVSE,
        //                   ActionType           ActionType         = ActionType.update,
        //                   IncludeEVSEDelegate  IncludeEVSEs       = null,

        //                   DateTime?            Timestamp          = null,
        //                   CancellationToken?   CancellationToken  = null,
        //                   EventTracking_Id     EventTrackingId    = null,
        //                   TimeSpan?            RequestTimeout     = null);

        ///// <summary>
        ///// Upload all EVSE status of the given enumeration of EVSEs.
        ///// </summary>
        ///// <param name="EVSEs">An enumeration of EVSEs.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(IEnumerable<EVSE>    EVSEs,
        //                   ActionType           ActionType         = ActionType.update,
        //                   IncludeEVSEDelegate  IncludeEVSEs       = null,

        //                   DateTime?            Timestamp          = null,
        //                   CancellationToken?   CancellationToken  = null,
        //                   EventTracking_Id     EventTrackingId    = null,
        //                   TimeSpan?            RequestTimeout     = null);

        ///// <summary>
        ///// Upload all EVSE status of the given charging pool.
        ///// </summary>
        ///// <param name="ChargingStation">A charging pool.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(ChargingStation      ChargingStation,
        //                   ActionType           ActionType         = ActionType.update,
        //                   IncludeEVSEDelegate  IncludeEVSEs       = null,

        //                   DateTime?            Timestamp          = null,
        //                   CancellationToken?   CancellationToken  = null,
        //                   EventTracking_Id     EventTrackingId    = null,
        //                   TimeSpan?            RequestTimeout     = null);

        ///// <summary>
        ///// Upload all EVSE status of the given enumeration of charging pools.
        ///// </summary>
        ///// <param name="ChargingStations">An enumeration of charging pools.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(IEnumerable<ChargingStation>  ChargingStations,
        //                   ActionType                    ActionType         = ActionType.update,
        //                   IncludeEVSEDelegate           IncludeEVSEs       = null,

        //                   DateTime?                     Timestamp          = null,
        //                   CancellationToken?            CancellationToken  = null,
        //                   EventTracking_Id              EventTrackingId    = null,
        //                   TimeSpan?                     RequestTimeout     = null);

        ///// <summary>
        ///// Upload all EVSE status of the given charging pool.
        ///// </summary>
        ///// <param name="ChargingPool">A charging pool.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(ChargingPool         ChargingPool,
        //                   ActionType           ActionType         = ActionType.update,
        //                   IncludeEVSEDelegate  IncludeEVSEs       = null,

        //                   DateTime?            Timestamp          = null,
        //                   CancellationToken?   CancellationToken  = null,
        //                   EventTracking_Id     EventTrackingId    = null,
        //                   TimeSpan?            RequestTimeout     = null);

        ///// <summary>
        ///// Upload all EVSE status of the given enumeration of charging pools.
        ///// </summary>
        ///// <param name="ChargingPools">An enumeration of charging pools.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// <param name="OperatorId">An optional unique identification of the Charging Station Operator.</param>
        ///// <param name="OperatorName">The optional name of the Charging Station Operator.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(IEnumerable<ChargingPool>  ChargingPools,
        //                   ActionType                 ActionType         = ActionType.update,
        //                   IncludeEVSEDelegate        IncludeEVSEs       = null,

        //                   DateTime?                  Timestamp          = null,
        //                   CancellationToken?         CancellationToken  = null,
        //                   EventTracking_Id           EventTrackingId    = null,
        //                   TimeSpan?                  RequestTimeout     = null);

        ///// <summary>
        ///// Upload all EVSE status of the given Charging Station Operator.
        ///// </summary>
        ///// <param name="EVSEOperator">An Charging Station Operator.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(ChargingStationOperator         EVSEOperator,
        //                   ActionType           ActionType         = ActionType.update,
        //                   IncludeEVSEDelegate  IncludeEVSEs       = null,

        //                   DateTime?            Timestamp          = null,
        //                   CancellationToken?   CancellationToken  = null,
        //                   EventTracking_Id     EventTrackingId    = null,
        //                   TimeSpan?            RequestTimeout     = null);

        ///// <summary>
        ///// Upload all EVSE status of the given enumeration of Charging Station Operators.
        ///// </summary>
        ///// <param name="EVSEOperators">An enumeration of EVSES operators.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// <param name="OperatorId">An optional unique identification of the Charging Station Operator.</param>
        ///// <param name="OperatorName">The optional name of the Charging Station Operator.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(IEnumerable<ChargingStationOperator>  EVSEOperators,
        //                   ActionType                 ActionType         = ActionType.update,
        //                   IncludeEVSEDelegate        IncludeEVSEs       = null,

        //                   DateTime?                  Timestamp          = null,
        //                   CancellationToken?         CancellationToken  = null,
        //                   EventTracking_Id           EventTrackingId    = null,
        //                   TimeSpan?                  RequestTimeout     = null);

        ///// <summary>
        ///// Upload all EVSE status of the given roaming network.
        ///// </summary>
        ///// <param name="RoamingNetwork">A roaming network.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// <param name="OperatorId">An optional unique identification of the Charging Station Operator.</param>
        ///// <param name="OperatorName">The optional name of the Charging Station Operator.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    PushEVSEStatus(RoamingNetwork       RoamingNetwork,
        //                   ActionType           ActionType         = ActionType.update,
        //                   IncludeEVSEDelegate  IncludeEVSEs       = null,

        //                   DateTime?            Timestamp          = null,
        //                   CancellationToken?   CancellationToken  = null,
        //                   EventTracking_Id     EventTrackingId    = null,
        //                   TimeSpan?            RequestTimeout     = null);

        /////// <summary>
        /////// Send EVSE status updates.
        /////// </summary>
        /////// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        /////// 
        /////// <param name="Timestamp">The optional timestamp of the request.</param>
        /////// <param name="CancellationToken">An optional token to cancel this request.</param>
        /////// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /////// <param name="RequestTimeout">An optional timeout for this request.</param>
        ////Task

        ////    PushEVSEStatus(EVSEStatusDiff      EVSEStatusDiff,

        ////                   DateTime?           Timestamp          = null,
        ////                   CancellationToken?  CancellationToken  = null,
        ////                   EventTracking_Id    EventTrackingId    = null,
        ////                   TimeSpan?           RequestTimeout     = null);


        //#endregion

    }


    public interface IRemotePushStatus
    {

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


        // Push status directly...

        #region UpdateEVSEAdminStatus                   (AdminStatusUpdates, TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateEVSEAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,
                                  TransmissionTypes                   TransmissionType    = TransmissionTypes.Enqueued,

                                  DateTime?                           Timestamp           = null,
                                  CancellationToken?                  CancellationToken   = null,
                                  EventTracking_Id                    EventTrackingId     = null,
                                  TimeSpan?                           RequestTimeout      = null);

        #endregion

        #region UpdateEVSEStatus                        (StatusUpdates,      TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                             TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueued,

                             DateTime?                      Timestamp           = null,
                             CancellationToken?             CancellationToken   = null,
                             EventTracking_Id               EventTrackingId     = null,
                             TimeSpan?                      RequestTimeout      = null);

        #endregion


        #region UpdateChargingStationAdminStatus        (AdminStatusUpdates, TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the given enumeration of charging station admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateChargingStationAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
                                             TransmissionTypes                              TransmissionType    = TransmissionTypes.Enqueued,

                                             DateTime?                                      Timestamp           = null,
                                             CancellationToken?                             CancellationToken   = null,
                                             EventTracking_Id                               EventTrackingId     = null,
                                             TimeSpan?                                      RequestTimeout      = null);

        #endregion

        #region UpdateChargingStationStatus             (StatusUpdates,      TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the given enumeration of charging station status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateChargingStationStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,
                                        TransmissionTypes                         TransmissionType    = TransmissionTypes.Enqueued,

                                        DateTime?                                 Timestamp           = null,
                                        CancellationToken?                        CancellationToken   = null,
                                        EventTracking_Id                          EventTrackingId     = null,
                                        TimeSpan?                                 RequestTimeout      = null);

        #endregion


        #region UpdateChargingPoolAdminStatus           (AdminStatusUpdates, TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the given enumeration of charging pool admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateChargingPoolAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
                                          TransmissionTypes                           TransmissionType    = TransmissionTypes.Enqueued,

                                          DateTime?                                   Timestamp           = null,
                                          CancellationToken?                          CancellationToken   = null,
                                          EventTracking_Id                            EventTrackingId     = null,
                                          TimeSpan?                                   RequestTimeout      = null);

        #endregion

        #region UpdateChargingPoolStatus                (StatusUpdates,      TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the given enumeration of charging pool status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging pool status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateChargingPoolStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueued,

                                     DateTime?                              Timestamp           = null,
                                     CancellationToken?                     CancellationToken   = null,
                                     EventTracking_Id                       EventTrackingId     = null,
                                     TimeSpan?                              RequestTimeout      = null);

        #endregion


        #region UpdateChargingStationOperatorAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station operator admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateChargingStationOperatorAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
                                                     TransmissionTypes                                      TransmissionType    = TransmissionTypes.Enqueued,

                                                     DateTime?                                              Timestamp           = null,
                                                     CancellationToken?                                     CancellationToken   = null,
                                                     EventTracking_Id                                       EventTrackingId     = null,
                                                     TimeSpan?                                              RequestTimeout      = null);

        #endregion

        #region UpdateChargingStationOperatorStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station operator status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateChargingStationOperatorStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,
                                                TransmissionTypes                                 TransmissionType    = TransmissionTypes.Enqueued,

                                                DateTime?                                         Timestamp           = null,
                                                CancellationToken?                                CancellationToken   = null,
                                                EventTracking_Id                                  EventTrackingId     = null,
                                                TimeSpan?                                         RequestTimeout      = null);

        #endregion


        #region UpdateRoamingNetworkAdminStatus           (AdminStatusUpdates, TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the given enumeration of roaming network admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the roaming network admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateRoamingNetworkAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
                                            TransmissionTypes                             TransmissionType    = TransmissionTypes.Enqueued,

                                            DateTime?                                     Timestamp           = null,
                                            CancellationToken?                            CancellationToken   = null,
                                            EventTracking_Id                              EventTrackingId     = null,
                                            TimeSpan?                                     RequestTimeout      = null);

        #endregion

        #region UpdateRoamingNetworkStatus                (StatusUpdates,      TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the given enumeration of roaming network status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of roaming network status updates.</param>
        /// <param name="TransmissionType">Whether to send the roaming network status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateRoamingNetworkStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,
                                       TransmissionTypes                        TransmissionType    = TransmissionTypes.Enqueued,

                                       DateTime?                                Timestamp           = null,
                                       CancellationToken?                       CancellationToken   = null,
                                       EventTracking_Id                         EventTrackingId     = null,
                                       TimeSpan?                                RequestTimeout      = null);

        #endregion


    }

}
