///*
// * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
// *
// * Licensed under the Affero GPL license, Version 3.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.gnu.org/licenses/agpl.html
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using org.GraphDefined.Vanaheimr.Illias;

//#endregion

//namespace cloud.charging.open.protocols.WWCP
//{

//    public interface IReceivePOIData
//    {

//        // Events

//        #region OnEVSEDataPush/-Pushed

//        ///// <summary>
//        ///// An event fired whenever new EVSE data will be send upstream.
//        ///// </summary>
//        //event OnPushEVSEDataRequestDelegate OnPushEVSEDataRequest;

//        ///// <summary>
//        ///// An event fired whenever new EVSE data had been sent upstream.
//        ///// </summary>
//        //event OnPushEVSEDataResponseDelegate OnPushEVSEDataResponse;

//        #endregion


//        // Push data directly...

//        #region (Set/Add/Update/Delete) Roaming network...

//        #region SetStaticData   (RoamingNetwork, ...)

//        /// <summary>
//        /// Upload the EVSE data of the given roaming network.
//        /// </summary>
//        /// <param name="RoamingNetwork">A roaming network.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        Task<PushEVSEDataResult>

//            SetStaticData(RoamingNetwork      RoamingNetwork,

//                          DateTime?           Timestamp           = null,
//                          EventTracking_Id?   EventTrackingId     = null,
//                          TimeSpan?           RequestTimeout      = null,
//                          CancellationToken   CancellationToken   = default);

//        #endregion

//        #region AddStaticData   (RoamingNetwork, ...)

//        /// <summary>
//        /// Upload the EVSE data of the given roaming network.
//        /// </summary>
//        /// <param name="RoamingNetwork">A roaming network.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        Task<PushEVSEDataResult>

//            AddStaticData(RoamingNetwork      RoamingNetwork,

//                          DateTime?           Timestamp           = null,
//                          EventTracking_Id?   EventTrackingId     = null,
//                          TimeSpan?           RequestTimeout      = null,
//                          CancellationToken   CancellationToken   = default);

//        #endregion

//        #region UpdateStaticData(RoamingNetwork, ...)

//        /// <summary>
//        /// Upload the EVSE data of the given roaming network.
//        /// </summary>
//        /// <param name="RoamingNetwork">A roaming network.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        Task<PushEVSEDataResult>

//            UpdateStaticData(RoamingNetwork      RoamingNetwork,

//                             DateTime?           Timestamp           = null,
//                             EventTracking_Id?   EventTrackingId     = null,
//                             TimeSpan?           RequestTimeout      = null,
//                             CancellationToken   CancellationToken   = default);

//        #endregion

//        #region DeleteStaticData(RoamingNetwork, ...)

//        /// <summary>
//        /// Upload the EVSE data of the given roaming network.
//        /// </summary>
//        /// <param name="RoamingNetwork">A roaming network.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        Task<PushEVSEDataResult>

//            DeleteStaticData(RoamingNetwork      RoamingNetwork,

//                             DateTime?           Timestamp           = null,
//                             EventTracking_Id?   EventTrackingId     = null,
//                             TimeSpan?           RequestTimeout      = null,
//                             CancellationToken   CancellationToken   = default);

//        #endregion

//        #endregion

//        #region (Set/Add/Update/Delete) Charging station operator(s)...

//        #region SetStaticData   (ChargingStationOperator, ...)

//        /// <summary>
//        /// Set the EVSE data of the given charging station operator as new static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStationOperator">A charging station operator.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            SetStaticData(ChargingStationOperator  ChargingStationOperator,

//                          DateTime?                Timestamp           = null,
//                          CancellationToken        CancellationToken   = default,
//                          EventTracking_Id?        EventTrackingId     = null,
//                          TimeSpan?                RequestTimeout      = null);

//        #endregion

//        #region AddStaticData   (ChargingStationOperator, ...)

//        /// <summary>
//        /// Add the EVSE data of the given charging station operator to the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStationOperator">A charging station operator.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            AddStaticData(ChargingStationOperator  ChargingStationOperator,

//                          DateTime?                Timestamp           = null,
//                          CancellationToken        CancellationToken   = default,
//                          EventTracking_Id?        EventTrackingId     = null,
//                          TimeSpan?                RequestTimeout      = null);

//        #endregion

//        #region UpdateStaticData(ChargingStationOperator, ...)

//        /// <summary>
//        /// Update the EVSE data of the given charging station operator.
//        /// </summary>
//        /// <param name="ChargingStationOperator">A charging station operator.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            UpdateStaticData(ChargingStationOperator  ChargingStationOperator,

//                             DateTime?                Timestamp           = null,
//                             CancellationToken        CancellationToken   = default,
//                             EventTracking_Id?        EventTrackingId     = null,
//                             TimeSpan?                RequestTimeout      = null);

//        #endregion

//        #region DeleteStaticData(ChargingStationOperator, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given charging station operator from the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStationOperator">A charging station operator.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            DeleteStaticData(ChargingStationOperator  ChargingStationOperator,

//                             DateTime?                Timestamp           = null,
//                             CancellationToken        CancellationToken   = default,
//                             EventTracking_Id?        EventTrackingId     = null,
//                             TimeSpan?                RequestTimeout      = null);

//        #endregion


//        #region SetStaticData   (ChargingStationOperators, ...)

//        /// <summary>
//        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            SetStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

//                          DateTime?                             Timestamp           = null,
//                          CancellationToken                     CancellationToken   = default,
//                          EventTracking_Id?                     EventTrackingId     = null,
//                          TimeSpan?                             RequestTimeout      = null);

//        #endregion

//        #region AddStaticData   (ChargingStationOperators, ...)

//        /// <summary>
//        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            AddStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

//                          DateTime?                             Timestamp           = null,
//                          CancellationToken                     CancellationToken   = default,
//                          EventTracking_Id?                     EventTrackingId     = null,
//                          TimeSpan?                             RequestTimeout      = null);

//        #endregion

//        #region UpdateStaticData(ChargingStationOperators, ...)

//        /// <summary>
//        /// Update the EVSE data of the given enumeration of charging station operators.
//        /// </summary>
//        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            UpdateStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

//                             DateTime?                             Timestamp           = null,
//                             CancellationToken                     CancellationToken   = default,
//                             EventTracking_Id?                     EventTrackingId     = null,
//                             TimeSpan?                             RequestTimeout      = null);
//        #endregion

//        #region DeleteStaticData(ChargingStationOperators, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            DeleteStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

//                             DateTime?                             Timestamp           = null,
//                             CancellationToken                     CancellationToken   = default,
//                             EventTracking_Id?                     EventTrackingId     = null,
//                             TimeSpan?                             RequestTimeout      = null);

//        #endregion

//        #endregion

//        #region (Set/Add/Update/Delete) Charging pool(s)...

//        #region SetStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Set the EVSE data of the given charging pool as new static EVSE data.
//        /// </summary>
//        /// <param name="ChargingPool">A charging pool.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            SetStaticData(ChargingPool        ChargingPool,

//                          DateTime?           Timestamp           = null,
//                          CancellationToken   CancellationToken   = default,
//                          EventTracking_Id?   EventTrackingId     = null,
//                          TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region AddStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Add the EVSE data of the given charging pool to the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingPool">A charging pool.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            AddStaticData(ChargingPool        ChargingPool,

//                          DateTime?           Timestamp           = null,
//                          CancellationToken   CancellationToken   = default,
//                          EventTracking_Id?   EventTrackingId     = null,
//                          TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region UpdateStaticData(ChargingPool, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the data of the given charging pool.
//        /// </summary>
//        /// <param name="ChargingPool">A charging pool.</param>
//        /// <param name="PropertyName">The name of the charging pool property to update.</param>
//        /// <param name="OldValue">The old value of the charging pool property to update.</param>
//        /// <param name="NewValue">The new value of the charging pool property to update.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            UpdateStaticData(ChargingPool        ChargingPool,
//                             String?             PropertyName        = null,
//                             Object?             OldValue            = null,
//                             Object?             NewValue            = null,

//                             DateTime?           Timestamp           = null,
//                             CancellationToken   CancellationToken   = default,
//                             EventTracking_Id?   EventTrackingId     = null,
//                             TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region DeleteStaticData(ChargingPool, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given charging pool from the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingPool">A charging pool.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            DeleteStaticData(ChargingPool        ChargingPool,

//                             DateTime?           Timestamp           = null,
//                             CancellationToken   CancellationToken   = default,
//                             EventTracking_Id?   EventTrackingId     = null,
//                             TimeSpan?           RequestTimeout      = null);

//        #endregion


//        #region SetStaticData   (ChargingPools, ...)

//        /// <summary>
//        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data.
//        /// </summary>
//        /// <param name="ChargingPools">An enumeration of charging pools.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            SetStaticData(IEnumerable<ChargingPool>  ChargingPools,

//                          DateTime?                  Timestamp           = null,
//                          CancellationToken          CancellationToken   = default,
//                          EventTracking_Id?          EventTrackingId     = null,
//                          TimeSpan?                  RequestTimeout      = null);

//        #endregion

//        #region AddStaticData   (ChargingPools, ...)

//        /// <summary>
//        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingPools">An enumeration of charging pools.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            AddStaticData(IEnumerable<ChargingPool>  ChargingPools,

//                          DateTime?                  Timestamp           = null,
//                          CancellationToken          CancellationToken   = default,
//                          EventTracking_Id?          EventTrackingId     = null,
//                          TimeSpan?                  RequestTimeout      = null);

//        #endregion

//        #region UpdateStaticData(ChargingPools, ...)

//        /// <summary>
//        /// Update the EVSE data of the given enumeration of charging pools.
//        /// </summary>
//        /// <param name="ChargingPools">An enumeration of charging pools.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            UpdateStaticData(IEnumerable<ChargingPool>  ChargingPools,

//                             DateTime?                  Timestamp           = null,
//                             CancellationToken          CancellationToken   = default,
//                             EventTracking_Id?          EventTrackingId     = null,
//                             TimeSpan?                  RequestTimeout      = null);

//        #endregion

//        #region DeleteStaticData(ChargingPools, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingPools">An enumeration of charging pools.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            DeleteStaticData(IEnumerable<ChargingPool>  ChargingPools,

//                             DateTime?                  Timestamp           = null,
//                             CancellationToken          CancellationToken   = default,
//                             EventTracking_Id?          EventTrackingId     = null,
//                             TimeSpan?                  RequestTimeout      = null);

//        #endregion

//        #endregion

//        #region (Set/Add/Update/Delete) Charging station(s)...

//        #region SetStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Set the EVSE data of the given charging station as new static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStation">A charging station.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            SetStaticData(ChargingStation     ChargingStation,

//                          DateTime?           Timestamp           = null,
//                          CancellationToken   CancellationToken   = default,
//                          EventTracking_Id?   EventTrackingId     = null,
//                          TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region AddStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Add the EVSE data of the given charging station to the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStation">A charging station.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            AddStaticData(ChargingStation     ChargingStation,

//                          DateTime?           Timestamp           = null,
//                          CancellationToken   CancellationToken   = default,
//                          EventTracking_Id?   EventTrackingId     = null,
//                          TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region UpdateStaticData(ChargingStation, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the EVSE data of the given charging station.
//        /// </summary>
//        /// <param name="ChargingStation">A charging station.</param>
//        /// <param name="PropertyName">The name of the charging station property to update.</param>
//        /// <param name="OldValue">The old value of the charging station property to update.</param>
//        /// <param name="NewValue">The new value of the charging station property to update.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            UpdateStaticData(ChargingStation     ChargingStation,
//                             String?             PropertyName        = null,
//                             Object?             OldValue            = null,
//                             Object?             NewValue            = null,

//                             DateTime?           Timestamp           = null,
//                             CancellationToken   CancellationToken   = default,
//                             EventTracking_Id?   EventTrackingId     = null,
//                             TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region DeleteStaticData(ChargingStation, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given charging station from the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStation">A charging station.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            DeleteStaticData(ChargingStation     ChargingStation,

//                             DateTime?           Timestamp           = null,
//                             CancellationToken   CancellationToken   = default,
//                             EventTracking_Id?   EventTrackingId     = null,
//                             TimeSpan?           RequestTimeout      = null);

//        #endregion


//        #region SetStaticData   (ChargingStations, ...)

//        /// <summary>
//        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStations">An enumeration of charging stations.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            SetStaticData(IEnumerable<ChargingStation>  ChargingStations,

//                          DateTime?                     Timestamp           = null,
//                          CancellationToken             CancellationToken   = default,
//                          EventTracking_Id?             EventTrackingId     = null,
//                          TimeSpan?                     RequestTimeout      = null);

//        #endregion

//        #region AddStaticData   (ChargingStations, ...)

//        /// <summary>
//        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStations">An enumeration of charging stations.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            AddStaticData(IEnumerable<ChargingStation>  ChargingStations,

//                          DateTime?                     Timestamp           = null,
//                          CancellationToken             CancellationToken   = default,
//                          EventTracking_Id?             EventTrackingId     = null,
//                          TimeSpan?                     RequestTimeout      = null);

//        #endregion

//        #region UpdateStaticData(ChargingStations, ...)

//        /// <summary>
//        /// Update the EVSE data of the given enumeration of charging stations.
//        /// </summary>
//        /// <param name="ChargingStations">An enumeration of charging stations.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            UpdateStaticData(IEnumerable<ChargingStation>  ChargingStations,

//                             DateTime?                     Timestamp           = null,
//                             CancellationToken             CancellationToken   = default,
//                             EventTracking_Id?             EventTrackingId     = null,
//                             TimeSpan?                     RequestTimeout      = null);

//        #endregion

//        #region DeleteStaticData(ChargingStations, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data.
//        /// </summary>
//        /// <param name="ChargingStations">An enumeration of charging stations.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            DeleteStaticData(IEnumerable<ChargingStation>  ChargingStations,

//                             DateTime?                     Timestamp           = null,
//                             CancellationToken             CancellationToken   = default,
//                             EventTracking_Id?             EventTrackingId     = null,
//                             TimeSpan?                     RequestTimeout      = null);

//        #endregion

//        #endregion

//        #region (Set/Add/Update/Delete) EVSE(s)...

//        #region SetStaticData   (EVSE, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Upload the static data of the given EVSE.
//        /// </summary>
//        /// <param name="EVSE">An EVSE.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            SetStaticData(EVSE                EVSE,

//                          DateTime?           Timestamp           = null,
//                          CancellationToken   CancellationToken   = default,
//                          EventTracking_Id?   EventTrackingId     = null,
//                          TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region AddStaticData   (EVSE, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Upload the static data of the given EVSE.
//        /// </summary>
//        /// <param name="EVSE">An EVSE.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            AddStaticData(EVSE                EVSE,

//                          DateTime?           Timestamp           = null,
//                          CancellationToken   CancellationToken   = default,
//                          EventTracking_Id?   EventTrackingId     = null,
//                          TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region UpdateStaticData(EVSE, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the static data of the given EVSE.
//        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
//        /// </summary>
//        /// <param name="EVSE">An EVSE to update.</param>
//        /// <param name="PropertyName">The name of the EVSE property to update.</param>
//        /// <param name="OldValue">The old value of the EVSE property to update.</param>
//        /// <param name="NewValue">The new value of the EVSE property to update.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            UpdateStaticData(EVSE                EVSE,
//                             String?             PropertyName        = null,
//                             Object?             OldValue            = null,
//                             Object?             NewValue            = null,

//                             DateTime?           Timestamp           = null,
//                             CancellationToken   CancellationToken   = default,
//                             EventTracking_Id?   EventTrackingId     = null,
//                             TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region DeleteStaticData(EVSE, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Delete the static data of the given EVSE.
//        /// </summary>
//        /// <param name="EVSE">An EVSE to delete.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            DeleteStaticData(EVSE                EVSE,

//                             DateTime?           Timestamp           = null,
//                             CancellationToken   CancellationToken   = default,
//                             EventTracking_Id?   EventTrackingId     = null,
//                             TimeSpan?           RequestTimeout      = null);

//        #endregion


//        #region SetStaticData   (EVSEs, ...)

//        /// <summary>
//        /// Upload the static data of the given EVSEs.
//        /// </summary>
//        /// <param name="EVSEs">An enumeration of EVSEs.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            SetStaticData(IEnumerable<EVSE>   EVSEs,

//                          DateTime?           Timestamp           = null,
//                          CancellationToken   CancellationToken   = default,
//                          EventTracking_Id?   EventTrackingId     = null,
//                          TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region AddStaticData   (EVSEs, ...)

//        /// <summary>
//        /// Upload the static data of the given EVSEs.
//        /// </summary>
//        /// <param name="EVSEs">An enumeration of EVSEs.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            AddStaticData(IEnumerable<EVSE>   EVSEs,

//                          DateTime?           Timestamp           = null,
//                          CancellationToken   CancellationToken   = default,
//                          EventTracking_Id?   EventTrackingId     = null,
//                          TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region UpdateStaticData(EVSEs, ...)

//        /// <summary>
//        /// Upload the static data of the given EVSEs.
//        /// </summary>
//        /// <param name="EVSEs">An enumeration of EVSEs.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            UpdateStaticData(IEnumerable<EVSE>   EVSEs,

//                             DateTime?           Timestamp           = null,
//                             CancellationToken   CancellationToken   = default,
//                             EventTracking_Id?   EventTrackingId     = null,
//                             TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #region DeleteStaticData(EVSEs, ...)

//        /// <summary>
//        /// Upload the static data of the given EVSEs.
//        /// </summary>
//        /// <param name="EVSEs">An enumeration of EVSEs.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            DeleteStaticData(IEnumerable<EVSE>   EVSEs,

//                             DateTime?           Timestamp           = null,
//                             CancellationToken   CancellationToken   = default,
//                             EventTracking_Id?   EventTrackingId     = null,
//                             TimeSpan?           RequestTimeout      = null);

//        #endregion

//        #endregion


//    }

//}
