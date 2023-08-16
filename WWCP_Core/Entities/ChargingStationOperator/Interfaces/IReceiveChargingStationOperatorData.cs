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
    public interface IReceiveChargingStationOperatorData
    {

        #region AddChargingStationOperator           (ChargingStationOperator,  ...)

        /// <summary>
        /// Add the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationOperatorResult>

            AddChargingStationOperator(IChargingStationOperator  ChargingStationOperator,

                                       DateTime?                 Timestamp           = null,
                                       EventTracking_Id?         EventTrackingId     = null,
                                       TimeSpan?                 RequestTimeout      = null,
                                       CancellationToken         CancellationToken   = default);

        #endregion

        #region AddChargingStationOperatorIfNotExists(ChargingStationOperator,  ...)

        /// <summary>
        /// Add the given charging station operator, if it does not already exist.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator to add, if it does not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationOperatorResult>

            AddChargingStationOperatorIfNotExists(IChargingStationOperator  ChargingStationOperator,

                                                  DateTime?                 Timestamp           = null,
                                                  EventTracking_Id?         EventTrackingId     = null,
                                                  TimeSpan?                 RequestTimeout      = null,
                                                  CancellationToken         CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingStationOperator   (ChargingStationOperator,  ...)

        /// <summary>
        /// Add or update the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingStationOperatorResult>

            AddOrUpdateChargingStationOperator(IChargingStationOperator  ChargingStationOperator,

                                               DateTime?                 Timestamp           = null,
                                               EventTracking_Id?         EventTrackingId     = null,
                                               TimeSpan?                 RequestTimeout      = null,
                                               CancellationToken         CancellationToken   = default);

        #endregion

        #region UpdateChargingStationOperator        (ChargingStationOperator,  PropertyName, NewValue, OldValue = null, DataSource = null, ...)

        /// <summary>
        /// Update the given charging station operator.
        /// The charging station operator can be uploaded as a whole, or just a single property of the charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator to update.</param>
        /// <param name="PropertyName">The name of the charging station operator property to update.</param>
        /// <param name="NewValue">The new value of the charging station operator property to update.</param>
        /// <param name="OldValue">The optional old value of the charging station operator property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingStationOperatorResult>

            UpdateChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                          String                    PropertyName,
                                          Object?                   NewValue,
                                          Object?                   OldValue            = null,
                                          Context?                  DataSource          = null,

                                          DateTime?                 Timestamp           = null,
                                          EventTracking_Id?         EventTrackingId     = null,
                                          TimeSpan?                 RequestTimeout      = null,
                                          CancellationToken         CancellationToken   = default);

        #endregion

        #region DeleteChargingStationOperator        (ChargingStationOperator,  ...)

        /// <summary>
        /// Delete the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingStationOperatorResult>

            DeleteChargingStationOperator(IChargingStationOperator  ChargingStationOperator,

                                          DateTime?                 Timestamp           = null,
                                          EventTracking_Id?         EventTrackingId     = null,
                                          TimeSpan?                 RequestTimeout      = null,
                                          CancellationToken         CancellationToken   = default);

        #endregion


        #region AddChargingStationOperators          (ChargingStationOperators, ...)

        /// <summary>
        /// Add the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationOperatorsResult>

            AddChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,

                                        DateTime?                              Timestamp           = null,
                                        EventTracking_Id?                      EventTrackingId     = null,
                                        TimeSpan?                              RequestTimeout      = null,
                                        CancellationToken                      CancellationToken   = default);

        #endregion

        #region AddChargingStationOperatorsIfNotExist(ChargingStationOperators, ...)

        /// <summary>
        /// Add the given enumeration of charging station operators, if they do not already exist.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators to add, if they do not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationOperatorsResult>

            AddChargingStationOperatorsIfNotExist(IEnumerable<IChargingStationOperator>  ChargingStationOperators,

                                                  DateTime?                              Timestamp           = null,
                                                  EventTracking_Id?                      EventTrackingId     = null,
                                                  TimeSpan?                              RequestTimeout      = null,
                                                  CancellationToken                      CancellationToken   = default);

        #endregion

        #region AddOrUpdateChargingStationOperators  (ChargingStationOperators, ...)

        /// <summary>
        /// Add or update the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingStationOperatorsResult>

            AddOrUpdateChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,

                                                DateTime?                              Timestamp           = null,
                                                EventTracking_Id?                      EventTrackingId     = null,
                                                TimeSpan?                              RequestTimeout      = null,
                                                CancellationToken                      CancellationToken   = default);

        #endregion

        #region UpdateChargingStationOperators       (ChargingStationOperators, ...)

        /// <summary>
        /// Update the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators to update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingStationOperatorsResult>

            UpdateChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,

                                           DateTime?                              Timestamp           = null,
                                           EventTracking_Id?                      EventTrackingId     = null,
                                           TimeSpan?                              RequestTimeout      = null,
                                           CancellationToken                      CancellationToken   = default);

        #endregion

        #region DeleteChargingStationOperators       (ChargingStationOperators, ...)

        /// <summary>
        /// Delete the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingStationOperatorsResult>

            DeleteChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,

                                           DateTime?                              Timestamp           = null,
                                           EventTracking_Id?                      EventTrackingId     = null,
                                           TimeSpan?                              RequestTimeout      = null,
                                           CancellationToken                      CancellationToken   = default);

        #endregion


    }

}
