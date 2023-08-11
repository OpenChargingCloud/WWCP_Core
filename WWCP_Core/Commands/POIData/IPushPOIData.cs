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
    public interface IPushPOIData
    {

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        Boolean  DisablePushData    { get; set; }

        #region (Add/Update/Delete) Roaming network...

        #region AddRoamingNetwork        (RoamingNetwork, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            AddRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                              TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                              DateTime?          Timestamp           = null,
                              EventTracking_Id?  EventTrackingId     = null,
                              TimeSpan?          RequestTimeout      = null,
                              CancellationToken  CancellationToken   = default);

        #endregion

        #region AddOrUpdateRoamingNetwork(RoamingNetwork, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            AddOrUpdateRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                                      TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                      DateTime?          Timestamp           = null,
                                      EventTracking_Id?  EventTrackingId     = null,
                                      TimeSpan?          RequestTimeout      = null,
                                      CancellationToken  CancellationToken   = default);

        #endregion

        #region UpdateRoamingNetwork     (RoamingNetwork, PropertyName, NewValue, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="PropertyName">The name of the roaming network property to update.</param>
        /// <param name="NewValue">The new value of the roaming network property to update.</param>
        /// <param name="OldValue">The optinal old value of the roaming network property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            UpdateRoamingNetwork(IRoamingNetwork    RoamingNetwork,
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

        #region DeleteRoamingNetwork     (RoamingNetwork, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            DeleteRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                                 TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                 DateTime?          Timestamp           = null,
                                 EventTracking_Id?  EventTrackingId     = null,
                                 TimeSpan?          RequestTimeout      = null,
                                 CancellationToken  CancellationToken   = default);

        #endregion

        #endregion

        #region (Add/Update/Delete) Charging station operator(s)...

        /// <summary>
        /// Only include charging pool identifications matching the given delegate.
        /// </summary>
        IncludeChargingStationOperatorIdDelegate  IncludeChargingStationOperatorIds    { get; }

        /// <summary>
        /// Only include charging pools matching the given delegate.
        /// </summary>
        IncludeChargingStationOperatorDelegate    IncludeChargingStationOperators      { get; }


        #region AddChargingStationOperator         (ChargingStationOperator, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station operator to the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            AddChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                       TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTime?                 Timestamp           = null,
                                       EventTracking_Id?         EventTrackingId     = null,
                                       TimeSpan?                 RequestTimeout      = null,
                                       CancellationToken         CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingStationOperator (ChargingStationOperator, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station operator as new static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            AddOrUpdateChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                               TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                                               DateTime?                 Timestamp           = null,
                                               EventTracking_Id?         EventTrackingId     = null,
                                               TimeSpan?                 RequestTimeout      = null,
                                               CancellationToken         CancellationToken   = default);

        #endregion

        #region UpdateChargingStationOperator      (ChargingStationOperator, PropertyName, NewValue, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="PropertyName">The name of the charging station operator property to update.</param>
        /// <param name="NewValue">The new value of the charging station operator property to update.</param>
        /// <param name="OldValue">The optional old value of the charging station operator property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            UpdateChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                          String                    PropertyName,
                                          Object?                   NewValue,
                                          Object?                   OldValue            = null,
                                          Context?                  DataSource          = null,
                                          TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                                          DateTime?                 Timestamp           = null,
                                          EventTracking_Id?         EventTrackingId     = null,
                                          TimeSpan?                 RequestTimeout      = null,
                                          CancellationToken         CancellationToken   = default);

        #endregion

        #region DeleteChargingStationOperator      (ChargingStationOperator, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station operator from the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            DeleteChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                          TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                                          DateTime?                 Timestamp           = null,
                                          EventTracking_Id?         EventTrackingId     = null,
                                          TimeSpan?                 RequestTimeout      = null,
                                          CancellationToken         CancellationToken   = default);

        #endregion


        #region AddChargingStationOperators        (ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            AddChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                        TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                                        DateTime?                              Timestamp           = null,
                                        EventTracking_Id?                      EventTrackingId     = null,
                                        TimeSpan?                              RequestTimeout      = null,
                                        CancellationToken                      CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingStationOperators(ChargingStationOperators, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            AddOrUpdateChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                                                DateTime?                              Timestamp           = null,
                                                EventTracking_Id?                      EventTrackingId     = null,
                                                TimeSpan?                              RequestTimeout      = null,
                                                CancellationToken                      CancellationToken   = default);

        #endregion

        #region UpdateChargingStationOperators     (ChargingStationOperators, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            UpdateChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                           TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                                           DateTime?                              Timestamp           = null,
                                           EventTracking_Id?                      EventTrackingId     = null,
                                           TimeSpan?                              RequestTimeout      = null,
                                           CancellationToken                      CancellationToken   = default);

        #endregion

        #region DeleteChargingStationOperators     (ChargingStationOperators, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            DeleteChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                           TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                                           DateTime?                              Timestamp           = null,
                                           EventTracking_Id?                      EventTrackingId     = null,
                                           TimeSpan?                              RequestTimeout      = null,
                                           CancellationToken                      CancellationToken   = default);

        #endregion

        #endregion

        #region (Add/Update/Delete) Charging pool(s)...

        /// <summary>
        /// Only include charging pool identifications matching the given delegate.
        /// </summary>
        IncludeChargingPoolIdDelegate  IncludeChargingPoolIds    { get; }

        /// <summary>
        /// Only include charging pools matching the given delegate.
        /// </summary>
        IncludeChargingPoolDelegate    IncludeChargingPools      { get; }


        #region AddChargingPool         (ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging pool to the static EVSE data.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolDataResult>

            AddChargingPool(IChargingPool      ChargingPool,
                            TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                            DateTime?          Timestamp           = null,
                            EventTracking_Id?  EventTrackingId     = null,
                            TimeSpan?          RequestTimeout      = null,
                            CancellationToken  CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingPool (ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging pool as new static EVSE data.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolDataResult>

            AddOrUpdateChargingPool(IChargingPool      ChargingPool,
                                    TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTime?          Timestamp           = null,
                                    EventTracking_Id?  EventTrackingId     = null,
                                    TimeSpan?          RequestTimeout      = null,
                                    CancellationToken  CancellationToken   = default);

        #endregion

        #region UpdateChargingPool      (ChargingPool,  PropertyName, NewValue, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the data of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
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
        Task<PushChargingPoolDataResult>

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

        #region DeleteChargingPool      (ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging pool from the static EVSE data.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolDataResult>

            DeleteChargingPool(IChargingPool      ChargingPool,
                               TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?          Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               CancellationToken  CancellationToken   = default);

        #endregion


        #region AddChargingPools        (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolDataResult>

            AddChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                             TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?                   Timestamp           = null,
                             EventTracking_Id?           EventTrackingId     = null,
                             TimeSpan?                   RequestTimeout      = null,
                             CancellationToken           CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingPools(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolDataResult>

            AddOrUpdateChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                     TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                     DateTime?                   Timestamp           = null,
                                     EventTracking_Id?           EventTrackingId     = null,
                                     TimeSpan?                   RequestTimeout      = null,
                                     CancellationToken           CancellationToken   = default);

        #endregion

        #region UpdateChargingPools     (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolDataResult>

            UpdateChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                DateTime?                   Timestamp           = null,
                                EventTracking_Id?           EventTrackingId     = null,
                                TimeSpan?                   RequestTimeout      = null,
                                CancellationToken           CancellationToken   = default);

        #endregion

        #region DeleteChargingPools     (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolDataResult>

            DeleteChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                DateTime?                   Timestamp           = null,
                                EventTracking_Id?           EventTrackingId     = null,
                                TimeSpan?                   RequestTimeout      = null,
                                CancellationToken           CancellationToken   = default);

        #endregion

        #endregion

        #region (Add/Update/Delete) Charging station(s)...

        /// <summary>
        /// Only include charging station identifications matching the given delegate.
        /// </summary>
        IncludeChargingStationIdDelegate  IncludeChargingStationIds    { get; }

        /// <summary>
        /// Only include charging stations matching the given delegate.
        /// </summary>
        IncludeChargingStationDelegate    IncludeChargingStations      { get; }


        #region AddChargingStation         (ChargingStation,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station to the static EVSE data.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationDataResult>

            AddChargingStation(IChargingStation   ChargingStation,
                               TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?          Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               CancellationToken  CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingStation (ChargingStation,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station as new static EVSE data.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationDataResult>

            AddOrUpdateChargingStation(IChargingStation   ChargingStation,
                                       TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTime?          Timestamp           = null,
                                       EventTracking_Id?  EventTrackingId     = null,
                                       TimeSpan?          RequestTimeout      = null,
                                       CancellationToken  CancellationToken   = default);

        #endregion

        #region UpdateChargingStation      (ChargingStation,  PropertyName, NewValue, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="PropertyName">The name of the charging station property to update.</param>
        /// <param name="NewValue">The new value of the charging station property to update.</param>
        /// <param name="OldValue">The optional old value of the charging station property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationDataResult>

            UpdateChargingStation(IChargingStation   ChargingStation,
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

        #region DeleteChargingStation      (ChargingStation,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station from the static EVSE data.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationDataResult>

            DeleteChargingStation(IChargingStation   ChargingStation,
                                  TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                  DateTime?          Timestamp           = null,
                                  EventTracking_Id?  EventTrackingId     = null,
                                  TimeSpan?          RequestTimeout      = null,
                                  CancellationToken  CancellationToken   = default);

        #endregion


        #region AddChargingStations        (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationDataResult>

            AddChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                                DateTime?                      Timestamp           = null,
                                EventTracking_Id?              EventTrackingId     = null,
                                TimeSpan?                      RequestTimeout      = null,
                                CancellationToken              CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingStations(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationDataResult>

            AddOrUpdateChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                        TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                                        DateTime?                      Timestamp           = null,
                                        EventTracking_Id?              EventTrackingId     = null,
                                        TimeSpan?                      RequestTimeout      = null,
                                        CancellationToken              CancellationToken   = default);

        #endregion

        #region UpdateChargingStations     (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationDataResult>

            UpdateChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                   TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                                   DateTime?                      Timestamp           = null,
                                   EventTracking_Id?              EventTrackingId     = null,
                                   TimeSpan?                      RequestTimeout      = null,
                                   CancellationToken              CancellationToken   = default);

        #endregion

        #region DeleteChargingStations     (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationDataResult>

            DeleteChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                   TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                                   DateTime?                      Timestamp           = null,
                                   EventTracking_Id?              EventTrackingId     = null,
                                   TimeSpan?                      RequestTimeout      = null,
                                   CancellationToken              CancellationToken   = default);

        #endregion

        #endregion

        #region (Add/Update/Delete) EVSE(s)...

        /// <summary>
        /// Only include EVSE identifications matching the given delegate.
        /// </summary>
        IncludeEVSEIdDelegate  IncludeEVSEIds    { get; }

        /// <summary>
        /// Only include EVSEs matching the given delegate.
        /// </summary>
        IncludeEVSEDelegate    IncludeEVSEs      { get; }


        #region AddEVSE           (EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to add.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            AddEVSE(IEVSE              EVSE,
                    TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                    DateTime?          Timestamp           = null,
                    EventTracking_Id?  EventTrackingId     = null,
                    TimeSpan?          RequestTimeout      = null,
                    CancellationToken  CancellationToken   = default);

        #endregion

        #region AddEVSEIfNotExists(EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE, if it does not already exist.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            AddEVSEIfNotExists(IEVSE              EVSE,
                               TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?          Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               CancellationToken  CancellationToken   = default);

        #endregion

        #region AddOrUpdateEVSE   (EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to add or udpate.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            AddOrUpdateEVSE(IEVSE              EVSE,
                            TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                            DateTime?          Timestamp           = null,
                            EventTracking_Id?  EventTrackingId     = null,
                            TimeSpan?          RequestTimeout      = null,
                            CancellationToken  CancellationToken   = default);

        #endregion

        #region UpdateEVSE        (EVSE,  PropertyName, NewValue, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given EVSE.
        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="OldValue">The optional old value of the EVSE property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            UpdateEVSE(IEVSE              EVSE,
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

        #region DeleteEVSE        (EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE deletion directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            DeleteEVSE(IEVSE              EVSE,
                       TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                       DateTime?          Timestamp           = null,
                       EventTracking_Id?  EventTrackingId     = null,
                       TimeSpan?          RequestTimeout      = null,
                       CancellationToken  CancellationToken   = default);

        #endregion


        #region AddEVSEs          (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            AddEVSEs     (IEnumerable<IEVSE>  EVSEs,
                          TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                          DateTime?           Timestamp           = null,
                          EventTracking_Id?   EventTrackingId     = null,
                          TimeSpan?           RequestTimeout      = null,
                          CancellationToken   CancellationToken   = default);

        #endregion

        #region AddEVSEsIfNotExist(EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs, if they do not already exist.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add, if they do not already exist.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            AddEVSEsIfNotExist(IEnumerable<IEVSE>  EVSEs,
                               TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?           Timestamp           = null,
                               EventTracking_Id?   EventTrackingId     = null,
                               TimeSpan?           RequestTimeout      = null,
                               CancellationToken   CancellationToken   = default);

        #endregion

        #region AddOrUpdateEVSEs  (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add or update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            AddOrUpdateEVSEs(IEnumerable<IEVSE>  EVSEs,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null,
                             CancellationToken   CancellationToken   = default);

        #endregion

        #region UpdateEVSEs       (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            UpdateEVSEs(IEnumerable<IEVSE>  EVSEs,
                        TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                        DateTime?           Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        CancellationToken   CancellationToken   = default);

        #endregion

        #region DeleteEVSEs       (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEDataResult>

            DeleteEVSEs(IEnumerable<IEVSE>  EVSEs,
                        TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                        DateTime?           Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        CancellationToken   CancellationToken   = default);

        #endregion

        #endregion


    }

}
