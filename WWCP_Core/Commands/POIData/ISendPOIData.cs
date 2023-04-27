/*
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

    /// <summary>
    /// The common interface of all WWCP point-of-interest data management.
    /// </summary>
    public interface ISendPOIData
    {

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        Boolean  DisablePushData    { get; set; }


        #region (Set/Add/Update/Delete) EVSE(s)...

        /// <summary>
        /// Only include EVSE identifications matching the given delegate.
        /// </summary>
        IncludeEVSEIdDelegate  IncludeEVSEIds    { get; }

        /// <summary>
        /// Only include EVSEs matching the given delegate.
        /// </summary>
        IncludeEVSEDelegate    IncludeEVSEs      { get; }


        #region SetStaticData   (EVSE, TransmissionType = Enqueue, ...)

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
        Task<PushEVSEDataResult>

            SetStaticData(IEVSE               EVSE,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region AddStaticData   (EVSE, TransmissionType = Enqueue, ...)

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
        Task<PushEVSEDataResult>

            AddStaticData(IEVSE               EVSE,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(EVSE, PropertyName, NewValue, OldValue = null, ...)

        /// <summary>
        /// Update the static data of the given EVSE.
        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="OldValue">The optional old value of the EVSE property to update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            UpdateStaticData(IEVSE               EVSE,
                             String              PropertyName,
                             Object?             NewValue,
                             Object?             OldValue            = null,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(EVSE, TransmissionType = Enqueue, ...)

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
        Task<PushEVSEDataResult>

            DeleteStaticData(IEVSE               EVSE,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion


        #region SetStaticData   (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Upload the static data of the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            SetStaticData(IEnumerable<IEVSE>  EVSEs,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region AddStaticData   (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Upload the static data of the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            AddStaticData(IEnumerable<IEVSE>  EVSEs,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Upload the static data of the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            UpdateStaticData(IEnumerable<IEVSE>  EVSEs,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Upload the static data of the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            DeleteStaticData(IEnumerable<IEVSE>  EVSEs,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        /// <summary>
        /// Only include charging station identifications matching the given delegate.
        /// </summary>
        IncludeChargingStationIdDelegate  IncludeChargingStationIds    { get; }

        /// <summary>
        /// Only include charging stations matching the given delegate.
        /// </summary>
        IncludeChargingStationDelegate    IncludeChargingStations      { get; }


        #region SetStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station as new static EVSE data.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationDataResult>

            SetStaticData(IChargingStation    ChargingStation,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station to the static EVSE data.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationDataResult>

            AddStaticData(IChargingStation    ChargingStation,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingStation, PropertyName, NewValue, OldValue = null, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="PropertyName">The name of the charging station property to update.</param>
        /// <param name="NewValue">The new value of the charging station property to update.</param>
        /// <param name="OldValue">The optional old value of the charging station property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationDataResult>

            UpdateStaticData(IChargingStation    ChargingStation,
                             String              PropertyName,
                             Object?             NewValue,
                             Object?             OldValue            = null,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station from the static EVSE data.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationDataResult>

            DeleteStaticData(IChargingStation    ChargingStation,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion


        #region SetStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationDataResult>

            SetStaticData(IEnumerable<IChargingStation>  ChargingStations,
                          TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?                      Timestamp           = null,
                          CancellationToken?             CancellationToken   = null,
                          EventTracking_Id?              EventTrackingId     = null,
                          TimeSpan?                      RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationDataResult>

            AddStaticData(IEnumerable<IChargingStation>  ChargingStations,
                          TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?                      Timestamp           = null,
                          CancellationToken?             CancellationToken   = null,
                          EventTracking_Id?              EventTrackingId     = null,
                          TimeSpan?                      RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationDataResult>

            UpdateStaticData(IEnumerable<IChargingStation>  ChargingStations,
                             TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?                      Timestamp           = null,
                             CancellationToken?             CancellationToken   = null,
                             EventTracking_Id?              EventTrackingId     = null,
                             TimeSpan?                      RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationDataResult>

            DeleteStaticData(IEnumerable<IChargingStation>  ChargingStations,
                             TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?                      Timestamp           = null,
                             CancellationToken?             CancellationToken   = null,
                             EventTracking_Id?              EventTrackingId     = null,
                             TimeSpan?                      RequestTimeout      = null);

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        /// <summary>
        /// Only include charging pool identifications matching the given delegate.
        /// </summary>
        IncludeChargingPoolIdDelegate  IncludeChargingPoolIds    { get; }

        /// <summary>
        /// Only include charging pools matching the given delegate.
        /// </summary>
        IncludeChargingPoolDelegate    IncludeChargingPools      { get; }


        #region SetStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging pool as new static EVSE data.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolDataResult>

            SetStaticData(IChargingPool       ChargingPool,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging pool to the static EVSE data.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolDataResult>

            AddStaticData(IChargingPool       ChargingPool,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingPool, PropertyName, NewValue, OldValue = null, ...)

        /// <summary>
        /// Update the data of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="PropertyName">The name of the charging pool property to update.</param>
        /// <param name="NewValue">The new value of the charging pool property to update.</param>
        /// <param name="OldValue">The optional old value of the charging pool property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolDataResult>

            UpdateStaticData(IChargingPool       ChargingPool,
                             String              PropertyName,
                             Object?             NewValue,
                             Object?             OldValue            = null,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging pool from the static EVSE data.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolDataResult>

            DeleteStaticData(IChargingPool       ChargingPool,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion


        #region SetStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolDataResult>

            SetStaticData(IEnumerable<IChargingPool>  ChargingPools,
                          TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?                   Timestamp           = null,
                          CancellationToken?          CancellationToken   = null,
                          EventTracking_Id?           EventTrackingId     = null,
                          TimeSpan?                   RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolDataResult>

            AddStaticData(IEnumerable<IChargingPool>  ChargingPools,
                          TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?                   Timestamp           = null,
                          CancellationToken?          CancellationToken   = null,
                          EventTracking_Id?           EventTrackingId     = null,
                          TimeSpan?                   RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolDataResult>

            UpdateStaticData(IEnumerable<IChargingPool>  ChargingPools,
                             TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?                   Timestamp           = null,
                             CancellationToken?          CancellationToken   = null,
                             EventTracking_Id?           EventTrackingId     = null,
                             TimeSpan?                   RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolDataResult>

            DeleteStaticData(IEnumerable<IChargingPool>  ChargingPools,
                             TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?                   Timestamp           = null,
                             CancellationToken?          CancellationToken   = null,
                             EventTracking_Id?           EventTrackingId     = null,
                             TimeSpan?                   RequestTimeout      = null);

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station operator(s)...

        /// <summary>
        /// Only include charging pool identifications matching the given delegate.
        /// </summary>
        IncludeChargingStationOperatorIdDelegate  IncludeChargingStationOperatorIds    { get; }

        /// <summary>
        /// Only include charging pools matching the given delegate.
        /// </summary>
        IncludeChargingStationOperatorDelegate    IncludeChargingStationOperators      { get; }


        #region SetStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station operator as new static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            SetStaticData(IChargingStationOperator  ChargingStationOperator,
                          TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?                 Timestamp           = null,
                          CancellationToken?        CancellationToken   = null,
                          EventTracking_Id?         EventTrackingId     = null,
                          TimeSpan?                 RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station operator to the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            AddStaticData(IChargingStationOperator  ChargingStationOperator,
                          TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?                 Timestamp           = null,
                          CancellationToken?        CancellationToken   = null,
                          EventTracking_Id?         EventTrackingId     = null,
                          TimeSpan?                 RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingStationOperator, PropertyName, NewValue, OldValue = null, TransmissionType = TransmissionTypes.Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="PropertyName">The name of the charging station operator property to update.</param>
        /// <param name="OldValue">The old value of the charging station operator property to update.</param>
        /// <param name="NewValue">The new value of the charging station operator property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            UpdateStaticData(IChargingStationOperator  ChargingStationOperator,
                             String                    PropertyName,
                             Object?                   NewValue,
                             Object?                   OldValue            = null,
                             TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?                 Timestamp           = null,
                             CancellationToken?        CancellationToken   = null,
                             EventTracking_Id?         EventTrackingId     = null,
                             TimeSpan?                 RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station operator from the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            DeleteStaticData(IChargingStationOperator  ChargingStationOperator,
                             TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?                 Timestamp           = null,
                             CancellationToken?        CancellationToken   = null,
                             EventTracking_Id?         EventTrackingId     = null,
                             TimeSpan?                 RequestTimeout      = null);

        #endregion


        #region SetStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            SetStaticData(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                          TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?                              Timestamp           = null,
                          CancellationToken?                     CancellationToken   = null,
                          EventTracking_Id?                      EventTrackingId     = null,
                          TimeSpan?                              RequestTimeout      = null);

        #endregion

        #region AddStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            AddStaticData(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                          TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?                              Timestamp           = null,
                          CancellationToken?                     CancellationToken   = null,
                          EventTracking_Id?                      EventTrackingId     = null,
                          TimeSpan?                              RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            UpdateStaticData(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                             TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?                              Timestamp           = null,
                             CancellationToken?                     CancellationToken   = null,
                             EventTracking_Id?                      EventTrackingId     = null,
                             TimeSpan?                              RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            DeleteStaticData(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                             TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?                              Timestamp           = null,
                             CancellationToken?                     CancellationToken   = null,
                             EventTracking_Id?                      EventTrackingId     = null,
                             TimeSpan?                              RequestTimeout      = null);

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Roaming network...

        #region SetStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            SetStaticData(IRoamingNetwork     RoamingNetwork,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region AddStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            AddStaticData(IRoamingNetwork     RoamingNetwork,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          CancellationToken?  CancellationToken   = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null);

        #endregion

        #region UpdateStaticData(RoamingNetwork, PropertyName, NewValue, OldValue = null, TransmissionType = TransmissionTypes.Enqueue, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="PropertyName">The name of the roaming network property to update.</param>
        /// <param name="NewValue">The new value of the roaming network property to update.</param>
        /// <param name="OldValue">The optinal old value of the roaming network property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            UpdateStaticData(IRoamingNetwork     RoamingNetwork,
                             String              PropertyName,
                             Object?             NewValue,
                             Object?             OldValue            = null,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #region DeleteStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            DeleteStaticData(IRoamingNetwork     RoamingNetwork,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null);

        #endregion

        #endregion


    }

}
