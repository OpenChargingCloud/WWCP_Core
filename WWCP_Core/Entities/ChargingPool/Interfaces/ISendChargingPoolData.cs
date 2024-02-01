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

    /// <summary>
    /// The common interface of all WWCP point-of-interest data management.
    /// </summary>
    public interface ISendChargingPoolData
    {

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        Boolean                        DisableSendChargingPoolData    { get; set; }

        /// <summary>
        /// Only include charging pool identifications matching the given delegate.
        /// </summary>
        IncludeChargingPoolIdDelegate  IncludeChargingPoolIds         { get; }

        /// <summary>
        /// Only include charging pools matching the given delegate.
        /// </summary>
        IncludeChargingPoolDelegate    IncludeChargingPools           { get; }


        #region AddChargingPool           (ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to add.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingPoolResult>

            AddChargingPool(IChargingPool      ChargingPool,
                            TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                            DateTime?          Timestamp           = null,
                            EventTracking_Id?  EventTrackingId     = null,
                            TimeSpan?          RequestTimeout      = null,
                            CancellationToken  CancellationToken   = default);

        #endregion

        #region AddChargingPoolIfNotExists(ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given charging pool, if it does not already exist.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to add, if it does not already exist.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingPoolResult>

            AddChargingPoolIfNotExists(IChargingPool      ChargingPool,
                                       TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTime?          Timestamp           = null,
                                       EventTracking_Id?  EventTrackingId     = null,
                                       TimeSpan?          RequestTimeout      = null,
                                       CancellationToken  CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingPool   (ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to add or update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingPoolResult>

            AddOrUpdateChargingPool(IChargingPool      ChargingPool,
                                    TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTime?          Timestamp           = null,
                                    EventTracking_Id?  EventTrackingId     = null,
                                    TimeSpan?          RequestTimeout      = null,
                                    CancellationToken  CancellationToken   = default);

        #endregion

        #region UpdateChargingPool        (ChargingPool,  PropertyName, NewValue, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to update.</param>
        /// <param name="PropertyName">The name of the charging pool property to update.</param>
        /// <param name="NewValue">The new value of the charging pool property to update.</param>
        /// <param name="OldValue">The optional old value of the charging pool property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingPoolResult>

            UpdateChargingPool(IChargingPool      ChargingPool,
                               String             PropertyName,
                               Object?            NewValue,
                               Object?            OldValue            = null,
                               Context?           DataSource          = null,
                               TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?          Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               CancellationToken  CancellationToken   = default);

        #endregion

        #region DeleteChargingPool        (ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to delete.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingPoolResult>

            DeleteChargingPool(IChargingPool      ChargingPool,
                               TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?          Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               CancellationToken  CancellationToken   = default);

        #endregion


        #region AddChargingPools          (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to add.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingPoolsResult>

            AddChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                             TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?                   Timestamp           = null,
                             EventTracking_Id?           EventTrackingId     = null,
                             TimeSpan?                   RequestTimeout      = null,
                             CancellationToken           CancellationToken   = default);

        #endregion

        #region AddChargingPoolsIfNotExist(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of charging pools, if they do not already exist.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to add, if they do not already exist.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingPoolsResult>

            AddChargingPoolsIfNotExist(IEnumerable<IChargingPool>  ChargingPools,
                                       TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTime?                   Timestamp           = null,
                                       EventTracking_Id?           EventTrackingId     = null,
                                       TimeSpan?                   RequestTimeout      = null,
                                       CancellationToken           CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingPools  (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to add or update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingPoolsResult>

            AddOrUpdateChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                     TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                     DateTime?                   Timestamp           = null,
                                     EventTracking_Id?           EventTrackingId     = null,
                                     TimeSpan?                   RequestTimeout      = null,
                                     CancellationToken           CancellationToken   = default);

        #endregion

        #region UpdateChargingPools       (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingPoolsResult>

            UpdateChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                DateTime?                   Timestamp           = null,
                                EventTracking_Id?           EventTrackingId     = null,
                                TimeSpan?                   RequestTimeout      = null,
                                CancellationToken           CancellationToken   = default);

        #endregion

        #region ReplaceChargingPools      (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Replace the given enumeration of charging pools.
        /// Charging pools not included will be deleted.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging stations to replace.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<ReplaceChargingPoolsResult>

            ReplaceChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                 TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                 DateTime?                   Timestamp           = null,
                                 EventTracking_Id?           EventTrackingId     = null,
                                 TimeSpan?                   RequestTimeout      = null,
                                 CancellationToken           CancellationToken   = default);

        #endregion

        #region DeleteChargingPools       (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to delete.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingPoolsResult>

            DeleteChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                DateTime?                   Timestamp           = null,
                                EventTracking_Id?           EventTrackingId     = null,
                                TimeSpan?                   RequestTimeout      = null,
                                CancellationToken           CancellationToken   = default);

        #endregion


    }

}
