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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{


    public interface IPushData
    {

        // Events

        #region OnEVSEDataPush/-Pushed

        ///// <summary>
        ///// An event fired whenever new EVSE data will be send upstream.
        ///// </summary>
        //event OnPushEVSEDataRequestDelegate   OnPushEVSEDataRequest;

        ///// <summary>
        ///// An event fired whenever new EVSE data had been sent upstream.
        ///// </summary>
        //event OnPushEVSEDataResponseDelegate  OnPushEVSEDataResponse;

        #endregion

        //IncludeEVSEDelegate IncludeEVSEs { get; }


        //#region PushEVSEData

        //#region SetStaticData   (EVSE,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the static data of the given EVSE.
        ///// </summary>
        ///// <param name="EVSE">An EVSE.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    SetStaticData(EVSE                 EVSE,
        //                  IncludeEVSEDelegate  IncludeEVSEs        = null,

        //                  DateTime?            Timestamp           = null,
        //                  CancellationToken?   CancellationToken   = null,
        //                  EventTracking_Id     EventTrackingId     = null,
        //                  TimeSpan?            RequestTimeout      = null);

        //#endregion

        //#region AddStaticData   (EVSE,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the static data of the given EVSE.
        ///// </summary>
        ///// <param name="EVSE">An EVSE.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    AddStaticData(EVSE                 EVSE,
        //                  IncludeEVSEDelegate  IncludeEVSEs        = null,

        //                  DateTime?            Timestamp           = null,
        //                  CancellationToken?   CancellationToken   = null,
        //                  EventTracking_Id     EventTrackingId     = null,
        //                  TimeSpan?            RequestTimeout      = null);

        //#endregion

        //#region UpdateStaticData(EVSE,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the static data of the given EVSE.
        ///// </summary>
        ///// <param name="EVSE">An EVSE.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    UpdateStaticData(EVSE                 EVSE,
        //                     IncludeEVSEDelegate  IncludeEVSEs        = null,

        //                     DateTime?            Timestamp           = null,
        //                     CancellationToken?   CancellationToken   = null,
        //                     EventTracking_Id     EventTrackingId     = null,
        //                     TimeSpan?            RequestTimeout      = null);

        //#endregion

        //#region DeleteStaticData(EVSE,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the static data of the given EVSE.
        ///// </summary>
        ///// <param name="EVSE">An EVSE.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    DeleteStaticData(EVSE                 EVSE,
        //                     IncludeEVSEDelegate  IncludeEVSEs        = null,

        //                     DateTime?            Timestamp           = null,
        //                     CancellationToken?   CancellationToken   = null,
        //                     EventTracking_Id     EventTrackingId     = null,
        //                     TimeSpan?            RequestTimeout      = null);

        //#endregion


        //#region SetStaticData   (EVSEs,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the static data of the given EVSEs.
        ///// </summary>
        ///// <param name="EVSEs">An enumeration of EVSEs.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    SetStaticData(IEnumerable<EVSE>    EVSEs,
        //                  IncludeEVSEDelegate  IncludeEVSEs        = null,

        //                  DateTime?            Timestamp           = null,
        //                  CancellationToken?   CancellationToken   = null,
        //                  EventTracking_Id     EventTrackingId     = null,
        //                  TimeSpan?            RequestTimeout      = null);

        //#endregion

        //#region AddStaticData   (EVSEs,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the static data of the given EVSEs.
        ///// </summary>
        ///// <param name="EVSEs">An enumeration of EVSEs.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    AddStaticData(IEnumerable<EVSE>    EVSEs,
        //                  IncludeEVSEDelegate  IncludeEVSEs        = null,

        //                  DateTime?            Timestamp           = null,
        //                  CancellationToken?   CancellationToken   = null,
        //                  EventTracking_Id     EventTrackingId     = null,
        //                  TimeSpan?            RequestTimeout      = null);

        //#endregion

        //#region UpdateStaticData(EVSEs,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the static data of the given EVSEs.
        ///// </summary>
        ///// <param name="EVSEs">An enumeration of EVSEs.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    UpdateStaticData(IEnumerable<EVSE>    EVSEs,
        //                     IncludeEVSEDelegate  IncludeEVSEs        = null,

        //                     DateTime?            Timestamp           = null,
        //                     CancellationToken?   CancellationToken   = null,
        //                     EventTracking_Id     EventTrackingId     = null,
        //                     TimeSpan?            RequestTimeout      = null);

        //#endregion

        //#region DeleteStaticData(EVSEs,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the static data of the given EVSEs.
        ///// </summary>
        ///// <param name="EVSEs">An enumeration of EVSEs.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    DeleteStaticData(IEnumerable<EVSE>    EVSEs,
        //                     IncludeEVSEDelegate  IncludeEVSEs        = null,

        //                     DateTime?            Timestamp           = null,
        //                     CancellationToken?   CancellationToken   = null,
        //                     EventTracking_Id     EventTrackingId     = null,
        //                     TimeSpan?            RequestTimeout      = null);

        //#endregion





        //#region SetStaticData   (ChargingStationOperator,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given roaming network.
        ///// </summary>
        ///// <param name="ChargingStationOperator">A charging station operator.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    SetStaticData(ChargingStationOperator  ChargingStationOperator,
        //                  IncludeEVSEDelegate      IncludeEVSEs        = null,

        //                  DateTime?                Timestamp           = null,
        //                  CancellationToken?       CancellationToken   = null,
        //                  EventTracking_Id         EventTrackingId     = null,
        //                  TimeSpan?                RequestTimeout      = null);

        //#endregion

        //#region AddStaticData   (ChargingStationOperator,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given roaming network.
        ///// </summary>
        ///// <param name="ChargingStationOperator">A charging station operator.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    AddStaticData(ChargingStationOperator  ChargingStationOperator,
        //                  IncludeEVSEDelegate      IncludeEVSEs        = null,

        //                  DateTime?                Timestamp           = null,
        //                  CancellationToken?       CancellationToken   = null,
        //                  EventTracking_Id         EventTrackingId     = null,
        //                  TimeSpan?                RequestTimeout      = null);

        //#endregion

        //#region UpdateStaticData(ChargingStationOperator,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given roaming network.
        ///// </summary>
        ///// <param name="ChargingStationOperator">A charging station operator.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    UpdateStaticData(ChargingStationOperator  ChargingStationOperator,
        //                     IncludeEVSEDelegate      IncludeEVSEs        = null,

        //                     DateTime?                Timestamp           = null,
        //                     CancellationToken?       CancellationToken   = null,
        //                     EventTracking_Id         EventTrackingId     = null,
        //                     TimeSpan?                RequestTimeout      = null);

        //#endregion

        //#region DeleteStaticData(ChargingStationOperator,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given roaming network.
        ///// </summary>
        ///// <param name="ChargingStationOperator">A charging station operator.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    DeleteStaticData(ChargingStationOperator  ChargingStationOperator,
        //                     IncludeEVSEDelegate      IncludeEVSEs        = null,

        //                     DateTime?                Timestamp           = null,
        //                     CancellationToken?       CancellationToken   = null,
        //                     EventTracking_Id         EventTrackingId     = null,
        //                     TimeSpan?                RequestTimeout      = null);

        //#endregion






        //#region SetStaticData   (RoamingNetwork,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given roaming network.
        ///// </summary>
        ///// <param name="RoamingNetwork">A roaming network.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    SetStaticData(RoamingNetwork       RoamingNetwork,
        //                  IncludeEVSEDelegate  IncludeEVSEs       = null,

        //                  DateTime?            Timestamp          = null,
        //                  CancellationToken?   CancellationToken  = null,
        //                  EventTracking_Id     EventTrackingId    = null,
        //                  TimeSpan?            RequestTimeout     = null);

        //#endregion

        //#region AddStaticData   (RoamingNetwork,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given roaming network.
        ///// </summary>
        ///// <param name="RoamingNetwork">A roaming network.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    AddStaticData(RoamingNetwork       RoamingNetwork,
        //                  IncludeEVSEDelegate  IncludeEVSEs       = null,

        //                  DateTime?            Timestamp          = null,
        //                  CancellationToken?   CancellationToken  = null,
        //                  EventTracking_Id     EventTrackingId    = null,
        //                  TimeSpan?            RequestTimeout     = null);

        //#endregion

        //#region UpdateStaticData(RoamingNetwork,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given roaming network.
        ///// </summary>
        ///// <param name="RoamingNetwork">A roaming network.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    UpdateStaticData(RoamingNetwork       RoamingNetwork,
        //                     IncludeEVSEDelegate  IncludeEVSEs       = null,

        //                     DateTime?            Timestamp          = null,
        //                     CancellationToken?   CancellationToken  = null,
        //                     EventTracking_Id     EventTrackingId    = null,
        //                     TimeSpan?            RequestTimeout     = null);

        //#endregion

        //#region DeleteStaticData(RoamingNetwork,  IncludeEVSEs = null, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given roaming network.
        ///// </summary>
        ///// <param name="RoamingNetwork">A roaming network.</param>
        ///// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    DeleteStaticData(RoamingNetwork       RoamingNetwork,
        //                     IncludeEVSEDelegate  IncludeEVSEs       = null,

        //                     DateTime?            Timestamp          = null,
        //                     CancellationToken?   CancellationToken  = null,
        //                     EventTracking_Id     EventTrackingId    = null,
        //                     TimeSpan?            RequestTimeout     = null);

        //#endregion

        //#endregion

    }


    public interface IRemotePushData
    {

        // Events

        #region OnEVSEDataPush/-Pushed

        ///// <summary>
        ///// An event fired whenever new EVSE data will be send upstream.
        ///// </summary>
        //event OnPushEVSEDataRequestDelegate OnPushEVSEDataRequest;

        ///// <summary>
        ///// An event fired whenever new EVSE data had been sent upstream.
        ///// </summary>
        //event OnPushEVSEDataResponseDelegate OnPushEVSEDataResponse;

        #endregion


        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        Boolean  DisablePushData   { get; set; }


        // Push data directly...

        #region (Set/Add/Update/Delete) EVSE(s)...

        #region SetStaticData   (EVSE, TransmissionType = Enqueued, ...)

        /// <summary>
        /// Upload the static data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            SetStaticData(EVSE                EVSE,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueued,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region AddStaticData   (EVSE, TransmissionType = Enqueued, ...)

        /// <summary>
        /// Upload the static data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            AddStaticData(EVSE                EVSE,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueued,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(EVSE, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueued, ...)

        /// <summary>
        /// Update the static data of the given EVSE.
        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="OldValue">The old value of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateStaticData(EVSE                EVSE,
                             String              PropertyName        = null,
                             Object              OldValue            = null,
                             Object              NewValue            = null,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueued,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id    EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(EVSE, TransmissionType = Enqueued, ...)

        /// <summary>
        /// Delete the static data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE deletion directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            DeleteStaticData(EVSE                EVSE,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueued,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id    EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion


        #region SetStaticData   (EVSEs, ...)

        /// <summary>
        /// Upload the static data of the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            SetStaticData(IEnumerable<EVSE>   EVSEs,

                          DateTime?           Timestamp          = null,
                          CancellationToken?  CancellationToken  = null,
                          EventTracking_Id    EventTrackingId    = null,
                          TimeSpan?           RequestTimeout     = null);

        #endregion

        #region AddStaticData   (EVSEs, ...)

        /// <summary>
        /// Upload the static data of the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            AddStaticData(IEnumerable<EVSE>   EVSEs,

                          DateTime?           Timestamp          = null,
                          CancellationToken?  CancellationToken  = null,
                          EventTracking_Id    EventTrackingId    = null,
                          TimeSpan?           RequestTimeout     = null);

        #endregion

        #region UpdateStaticData(EVSEs, ...)

        /// <summary>
        /// Upload the static data of the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateStaticData(IEnumerable<EVSE>   EVSEs,

                             DateTime?           Timestamp          = null,
                             CancellationToken?  CancellationToken  = null,
                             EventTracking_Id    EventTrackingId    = null,
                             TimeSpan?           RequestTimeout     = null);

        #endregion

        #region DeleteStaticData(EVSEs, ...)

        /// <summary>
        /// Upload the static data of the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            DeleteStaticData(IEnumerable<EVSE>   EVSEs,

                             DateTime?           Timestamp          = null,
                             CancellationToken?  CancellationToken  = null,
                             EventTracking_Id    EventTrackingId    = null,
                             TimeSpan?           RequestTimeout     = null);

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        #region SetStaticData   (ChargingStation, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            SetStaticData(ChargingStation     ChargingStation,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingStation, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            AddStaticData(ChargingStation     ChargingStation,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingStation, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            UpdateStaticData(ChargingStation     ChargingStation,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id    EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingStation, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            DeleteStaticData(ChargingStation     ChargingStation,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id    EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion


        #region SetStaticData   (ChargingStations, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            SetStaticData(IEnumerable<ChargingStation>  ChargingStations,

                          DateTime?                     Timestamp           = null,
                          CancellationToken?            CancellationToken   = null,
                          EventTracking_Id              EventTrackingId     = null,
                          TimeSpan?                     RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingStations, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            AddStaticData(IEnumerable<ChargingStation>  ChargingStations,

                          DateTime?                     Timestamp           = null,
                          CancellationToken?            CancellationToken   = null,
                          EventTracking_Id              EventTrackingId     = null,
                          TimeSpan?                     RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingStations, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging stations within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            UpdateStaticData(IEnumerable<ChargingStation>  ChargingStations,

                             DateTime?                     Timestamp           = null,
                             CancellationToken?            CancellationToken   = null,
                             EventTracking_Id              EventTrackingId     = null,
                             TimeSpan?                     RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingStations, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            DeleteStaticData(IEnumerable<ChargingStation>  ChargingStations,

                             DateTime?                     Timestamp           = null,
                             CancellationToken?            CancellationToken   = null,
                             EventTracking_Id              EventTrackingId     = null,
                             TimeSpan?                     RequestTimeout      = null);

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        #region SetStaticData   (ChargingPool, ...)

        /// <summary>
        /// Set the EVSE data of the given charging pool as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            SetStaticData(ChargingPool        ChargingPool,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingPool, ...)

        /// <summary>
        /// Add the EVSE data of the given charging pool to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            AddStaticData(ChargingPool        ChargingPool,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingPool, ...)

        /// <summary>
        /// Update the EVSE data of the given charging pool within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            UpdateStaticData(ChargingPool        ChargingPool,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id    EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingPool, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging pool from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            DeleteStaticData(ChargingPool        ChargingPool,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id    EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion


        #region SetStaticData   (ChargingPools, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            SetStaticData(IEnumerable<ChargingPool>  ChargingPools,

                          DateTime?                  Timestamp           = null,
                          CancellationToken?         CancellationToken   = null,
                          EventTracking_Id           EventTrackingId     = null,
                          TimeSpan?                  RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingPools, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            AddStaticData(IEnumerable<ChargingPool>  ChargingPools,

                          DateTime?                  Timestamp           = null,
                          CancellationToken?         CancellationToken   = null,
                          EventTracking_Id           EventTrackingId     = null,
                          TimeSpan?                  RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingPools, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging pools within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            UpdateStaticData(IEnumerable<ChargingPool>  ChargingPools,

                             DateTime?                  Timestamp           = null,
                             CancellationToken?         CancellationToken   = null,
                             EventTracking_Id           EventTrackingId     = null,
                             TimeSpan?                  RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingPools, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<WWCP.Acknowledgement>

            DeleteStaticData(IEnumerable<ChargingPool>  ChargingPools,

                             DateTime?                  Timestamp           = null,
                             CancellationToken?         CancellationToken   = null,
                             EventTracking_Id           EventTrackingId     = null,
                             TimeSpan?                  RequestTimeout      = null);

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station operator(s)...

        #region SetStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station operator as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            SetStaticData(ChargingStationOperator  ChargingStationOperator,

                          DateTime?                Timestamp           = null,
                          CancellationToken?       CancellationToken   = null,
                          EventTracking_Id         EventTrackingId     = null,
                          TimeSpan?                RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station operator to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            AddStaticData(ChargingStationOperator  ChargingStationOperator,

                          DateTime?                Timestamp           = null,
                          CancellationToken?       CancellationToken   = null,
                          EventTracking_Id         EventTrackingId     = null,
                          TimeSpan?                RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station operator within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateStaticData(ChargingStationOperator  ChargingStationOperator,

                             DateTime?                Timestamp           = null,
                             CancellationToken?       CancellationToken   = null,
                             EventTracking_Id         EventTrackingId     = null,
                             TimeSpan?                RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station operator from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            DeleteStaticData(ChargingStationOperator  ChargingStationOperator,

                             DateTime?                Timestamp           = null,
                             CancellationToken?       CancellationToken   = null,
                             EventTracking_Id         EventTrackingId     = null,
                             TimeSpan?                RequestTimeout      = null);

        #endregion


        #region SetStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            SetStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                          DateTime?                             Timestamp           = null,
                          CancellationToken?                    CancellationToken   = null,
                          EventTracking_Id                      EventTrackingId     = null,
                          TimeSpan?                             RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            AddStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                          DateTime?                             Timestamp           = null,
                          CancellationToken?                    CancellationToken   = null,
                          EventTracking_Id                      EventTrackingId     = null,
                          TimeSpan?                             RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging station operators within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                             DateTime?                             Timestamp           = null,
                             CancellationToken?                    CancellationToken   = null,
                             EventTracking_Id                      EventTrackingId     = null,
                             TimeSpan?                             RequestTimeout      = null);
        #endregion

        #region DeleteStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            DeleteStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                             DateTime?                             Timestamp           = null,
                             CancellationToken?                    CancellationToken   = null,
                             EventTracking_Id                      EventTrackingId     = null,
                             TimeSpan?                             RequestTimeout      = null);

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Roaming network...

        #region SetStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Upload the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            SetStaticData(RoamingNetwork      RoamingNetwork,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region AddStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Upload the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            AddStaticData(RoamingNetwork      RoamingNetwork,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id    EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Upload the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            UpdateStaticData(RoamingNetwork      RoamingNetwork,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id    EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Upload the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            DeleteStaticData(RoamingNetwork      RoamingNetwork,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id    EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #endregion



        Task<Acknowledgement>

            EnqueueChargingStationDataUpdate(ChargingStation  ChargingStation,
                                             String           PropertyName,
                                             Object           OldValue,
                                             Object           NewValue);


        Task<Acknowledgement>

            EnqueueChargingPoolDataUpdate(ChargingPool ChargingPool,
                                          String PropertyName,
                                          Object OldValue,
                                          Object NewValue);

    }

}
