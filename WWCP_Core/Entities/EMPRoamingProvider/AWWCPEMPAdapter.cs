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

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public abstract class AWWCPEMPAdapter : ACryptoEMobilityEntity<EMPRoamingProvider_Id,
                                                                   EMPRoamingProviderAdminStatusTypes,
                                                                   EMPRoamingProviderStatusTypes>,

                                            IReceiveRoamingNetworkData,
                                            IReceiveChargingStationOperatorData,
                                            IReceiveChargingPoolData,
                                            IReceiveChargingStationData,
                                            IReceiveEVSEData,

                                            IReceiveAdminStatus,
                                            IReceiveStatus,
                                            IReceiveEnergyStatus
    {

        #region Constructor(s)

        protected AWWCPEMPAdapter(EMPRoamingProvider_Id                             Id,
                                  IRoamingNetwork                                   RoamingNetwork,

                                  I18NString?                                       Name                         = null,
                                  I18NString?                                       Description                  = null,

                                  String?                                           EllipticCurve                = "P-256",
                                  ECPrivateKeyParameters?                           PrivateKey                   = null,
                                  PublicKeyCertificates?                            PublicKeyCertificates        = null,

                                  Timestamped<EMPRoamingProviderAdminStatusTypes>?  InitialAdminStatus           = null,
                                  Timestamped<EMPRoamingProviderStatusTypes>?       InitialStatus                = null,
                                  UInt16                                            MaxAdminStatusScheduleSize   = DefaultMaxAdminStatusScheduleSize,
                                  UInt16                                            MaxStatusScheduleSize        = DefaultMaxStatusScheduleSize,

                                  String?                                           DataSource                   = null,
                                  DateTime?                                         LastChange                   = null,

                                  JObject?                                          CustomData                   = null,
                                  UserDefinedDictionary?                            InternalData                 = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,
                   InitialAdminStatus,
                   InitialStatus,
                   MaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

        }

        #endregion


        #region Receive incoming Data/Status

        #region (Set/Add/Update/Delete) Roaming network(s)...

        #region AddRoamingNetwork           (RoamingNetwork,  ...)

        /// <summary>
        /// Add the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddRoamingNetworkResult>

            AddRoamingNetwork(IRoamingNetwork     RoamingNetwork,

                              DateTime?           Timestamp,
                              EventTracking_Id?   EventTrackingId,
                              TimeSpan?           RequestTimeout,
                              CancellationToken   CancellationToken)

                => Task.FromResult(
                       AddRoamingNetworkResult.NoOperation(
                           RoamingNetwork,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region AddRoamingNetworkIfNotExists(RoamingNetwork,  ...)

        /// <summary>
        /// Add the given roaming network, if it does not already exist.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to add, if it does not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddRoamingNetworkResult>

            AddRoamingNetworkIfNotExists(IRoamingNetwork     RoamingNetwork,

                                         DateTime?           Timestamp,
                                         EventTracking_Id?   EventTrackingId,
                                         TimeSpan?           RequestTimeout,
                                         CancellationToken   CancellationToken)

                => Task.FromResult(
                       AddRoamingNetworkResult.NoOperation(
                           RoamingNetwork,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region AddOrUpdateRoamingNetwork   (RoamingNetwork,  ...)

        /// <summary>
        /// Add or update the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddOrUpdateRoamingNetworkResult>

            AddOrUpdateRoamingNetwork(IRoamingNetwork     RoamingNetwork,

                                      DateTime?           Timestamp,
                                      EventTracking_Id?   EventTrackingId,
                                      TimeSpan?           RequestTimeout,
                                      CancellationToken   CancellationToken)

                => Task.FromResult(
                       AddOrUpdateRoamingNetworkResult.NoOperation(
                           RoamingNetwork,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateRoamingNetwork        (RoamingNetwork,  PropertyName, NewValue, OldValue = null, DataSource = null, ...)

        /// <summary>
        /// Update the EVSE data of the given roaming network within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<UpdateRoamingNetworkResult>

            UpdateRoamingNetwork(IRoamingNetwork     RoamingNetwork,
                                 String              PropertyName,
                                 Object?             NewValue,
                                 Object?             OldValue,
                                 Context?            DataSource,

                                 DateTime?           Timestamp,
                                 EventTracking_Id?   EventTrackingId,
                                 TimeSpan?           RequestTimeout,
                                 CancellationToken   CancellationToken)

                => Task.FromResult(
                       UpdateRoamingNetworkResult.NoOperation(
                           RoamingNetwork,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region DeleteRoamingNetwork        (RoamingNetwork,  ...)

        /// <summary>
        /// Delete the EVSE data of the given roaming network from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to upload.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<DeleteRoamingNetworkResult>

            DeleteRoamingNetwork(IRoamingNetwork     RoamingNetwork,

                                 DateTime?           Timestamp,
                                 EventTracking_Id?   EventTrackingId,
                                 TimeSpan?           RequestTimeout,
                                 CancellationToken   CancellationToken)

                => Task.FromResult(
                       DeleteRoamingNetworkResult.NoOperation(
                           RoamingNetwork,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion


        #region AddRoamingNetworks          (RoamingNetworks, ...)

        /// <summary>
        /// Add the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddRoamingNetworksResult>

            AddRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,

                               DateTime?                     Timestamp,
                               EventTracking_Id?             EventTrackingId,
                               TimeSpan?                     RequestTimeout,
                               CancellationToken             CancellationToken)

                => Task.FromResult(
                       AddRoamingNetworksResult.NoOperation(
                           RoamingNetworks,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region AddRoamingNetworkIfNotExists(RoamingNetworks, ...)

        /// <summary>
        /// Add the given enumeration of roaming networks, if they do not already exist.
        /// </summary>
        /// <param name="RoamingNetwork">An enumeration of roaming networks to add, if they do not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddRoamingNetworksResult>

            AddRoamingNetworksIfNotExist(IEnumerable<IRoamingNetwork>  RoamingNetworks,

                                         DateTime?                     Timestamp,
                                         EventTracking_Id?             EventTrackingId,
                                         TimeSpan?                     RequestTimeout,
                                         CancellationToken             CancellationToken)

                => Task.FromResult(
                       AddRoamingNetworksResult.NoOperation(
                           RoamingNetworks,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateRoamingNetwork   (RoamingNetworks, ...)

        /// <summary>
        /// Add or update the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetwork">An enumeration of roaming networks to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddOrUpdateRoamingNetworksResult>

            AddOrUpdateRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,

                                       DateTime?                     Timestamp,
                                       EventTracking_Id?             EventTrackingId,
                                       TimeSpan?                     RequestTimeout,
                                       CancellationToken             CancellationToken)

                => Task.FromResult(
                       AddOrUpdateRoamingNetworksResult.NoOperation(
                           RoamingNetworks,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region UpdateRoamingNetwork        (RoamingNetworks, ...)

        /// <summary>
        /// Update the EVSE data of the given roaming network within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<UpdateRoamingNetworksResult>

            UpdateRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,

                                  DateTime?                     Timestamp,
                                  EventTracking_Id?             EventTrackingId,
                                  TimeSpan?                     RequestTimeout,
                                  CancellationToken             CancellationToken)

                => Task.FromResult(
                       UpdateRoamingNetworksResult.NoOperation(
                           RoamingNetworks,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region DeleteRoamingNetwork        (RoamingNetworks, ...)

        /// <summary>
        /// Delete the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetwork">An enumeration of roaming networks to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<DeleteRoamingNetworksResult>

            DeleteRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,

                                  DateTime?                     Timestamp,
                                  EventTracking_Id?             EventTrackingId,
                                  TimeSpan?                     RequestTimeout,
                                  CancellationToken             CancellationToken)

                => Task.FromResult(
                       DeleteRoamingNetworksResult.NoOperation(
                           RoamingNetworks,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion


        #region UpdateRoamingNetworkAdminStatus(AdminStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of roaming network admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<PushRoamingNetworkAdminStatusResult>

            UpdateRoamingNetworkAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,

                                            DateTime?                                     Timestamp,
                                            EventTracking_Id?                             EventTrackingId,
                                            TimeSpan?                                     RequestTimeout,
                                            CancellationToken                             CancellationToken)

                => Task.FromResult(
                       PushRoamingNetworkAdminStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateRoamingNetworkStatus     (StatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of roaming network status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of roaming network status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<PushRoamingNetworkStatusResult>

            UpdateRoamingNetworkStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,

                                       DateTime?                                Timestamp,
                                       EventTracking_Id?                        EventTrackingId,
                                       TimeSpan?                                RequestTimeout,
                                       CancellationToken                        CancellationToken)

                => Task.FromResult(
                       PushRoamingNetworkStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station operator(s)...

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
        public virtual Task<AddChargingStationOperatorResult>

            AddChargingStationOperator(IChargingStationOperator  ChargingStationOperator,

                                       DateTime?                 Timestamp,
                                       EventTracking_Id?         EventTrackingId,
                                       TimeSpan?                 RequestTimeout,
                                       CancellationToken         CancellationToken)

                => Task.FromResult(
                       AddChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

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
        public virtual Task<AddChargingStationOperatorResult>

            AddChargingStationOperatorIfNotExists(IChargingStationOperator  ChargingStationOperator,

                                                  DateTime?                 Timestamp,
                                                  EventTracking_Id?         EventTrackingId,
                                                  TimeSpan?                 RequestTimeout,
                                                  CancellationToken         CancellationToken)

                => Task.FromResult(
                       AddChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

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
        public virtual Task<AddOrUpdateChargingStationOperatorResult>

            AddOrUpdateChargingStationOperator(IChargingStationOperator  ChargingStationOperator,

                                               DateTime?                 Timestamp,
                                               EventTracking_Id?         EventTrackingId,
                                               TimeSpan?                 RequestTimeout,
                                               CancellationToken         CancellationToken)

                => Task.FromResult(
                       AddOrUpdateChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

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
        public virtual Task<UpdateChargingStationOperatorResult>

            UpdateChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                          String                    PropertyName,
                                          Object?                   NewValue,
                                          Object?                   OldValue,
                                          Context?                  DataSource,

                                          DateTime?                 Timestamp,
                                          EventTracking_Id?         EventTrackingId,
                                          TimeSpan?                 RequestTimeout,
                                          CancellationToken         CancellationToken)

                => Task.FromResult(
                       UpdateChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

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
        public virtual Task<DeleteChargingStationOperatorResult>

            DeleteChargingStationOperator(IChargingStationOperator  ChargingStationOperator,

                                          DateTime?                 Timestamp,
                                          EventTracking_Id?         EventTrackingId,
                                          TimeSpan?                 RequestTimeout,
                                          CancellationToken         CancellationToken)

                => Task.FromResult(
                       DeleteChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

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
        public virtual Task<AddChargingStationOperatorsResult>

            AddChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,

                                        DateTime?                              Timestamp,
                                        EventTracking_Id?                      EventTrackingId,
                                        TimeSpan?                              RequestTimeout,
                                        CancellationToken                      CancellationToken)

                => Task.FromResult(
                       AddChargingStationOperatorsResult.NoOperation(
                           ChargingStationOperators,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

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
        public virtual Task<AddChargingStationOperatorsResult>

            AddChargingStationOperatorsIfNotExist(IEnumerable<IChargingStationOperator>  ChargingStationOperators,

                                                  DateTime?                              Timestamp,
                                                  EventTracking_Id?                      EventTrackingId,
                                                  TimeSpan?                              RequestTimeout,
                                                  CancellationToken                      CancellationToken)

                => Task.FromResult(
                       AddChargingStationOperatorsResult.NoOperation(
                           ChargingStationOperators,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

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
        public virtual Task<AddOrUpdateChargingStationOperatorsResult>

            AddOrUpdateChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,

                                                DateTime?                              Timestamp,
                                                EventTracking_Id?                      EventTrackingId,
                                                TimeSpan?                              RequestTimeout,
                                                CancellationToken                      CancellationToken)

                => Task.FromResult(
                       AddOrUpdateChargingStationOperatorsResult.NoOperation(
                           ChargingStationOperators,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

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
        public virtual Task<UpdateChargingStationOperatorsResult>

            UpdateChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,

                                           DateTime?                              Timestamp,
                                           EventTracking_Id?                      EventTrackingId,
                                           TimeSpan?                              RequestTimeout,
                                           CancellationToken                      CancellationToken)

                => Task.FromResult(
                       UpdateChargingStationOperatorsResult.NoOperation(
                           ChargingStationOperators,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

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
        public virtual Task<DeleteChargingStationOperatorsResult>

            DeleteChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,

                                           DateTime?                              Timestamp,
                                           EventTracking_Id?                      EventTrackingId,
                                           TimeSpan?                              RequestTimeout,
                                           CancellationToken                      CancellationToken)

                => Task.FromResult(
                       DeleteChargingStationOperatorsResult.NoOperation(
                           ChargingStationOperators,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion


        #region UpdateChargingStationOperatorAdminStatus(AdminStatusUpdates, ...)

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
        public virtual Task<PushChargingStationOperatorAdminStatusResult>

            UpdateChargingStationOperatorAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,

                                                     DateTime?                                              Timestamp,
                                                     EventTracking_Id?                                      EventTrackingId,
                                                     TimeSpan?                                              RequestTimeout,
                                                     CancellationToken                                      CancellationToken)

                => Task.FromResult(
                       PushChargingStationOperatorAdminStatusResult.OutOfService(
                           Id,
                           this,
                           AdminStatusUpdates
                       )
                   );

        #endregion

        #region UpdateChargingStationOperatorStatus     (StatusUpdates, ...)

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
        public virtual Task<PushChargingStationOperatorStatusResult>

            UpdateChargingStationOperatorStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,

                                                DateTime?                                         Timestamp,
                                                EventTracking_Id?                                 EventTrackingId,
                                                TimeSpan?                                         RequestTimeout,
                                                CancellationToken                                 CancellationToken)

                => Task.FromResult(
                       PushChargingStationOperatorStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        #region AddChargingPool           (ChargingPool,  ...)

        /// <summary>
        /// Add the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingPoolResult>

            AddChargingPool(IChargingPool       ChargingPool,

                            DateTime?           Timestamp,
                            EventTracking_Id?   EventTrackingId,
                            TimeSpan?           RequestTimeout,
                            CancellationToken   CancellationToken)

                => Task.FromResult(
                       AddChargingPoolResult.NoOperation(
                           ChargingPool,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region AddChargingPoolIfNotExists(ChargingPool,  ...)

        /// <summary>
        /// Add the given charging pool, if it does not already exist.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to add, if it does not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingPoolResult>

            AddChargingPoolIfNotExists(IChargingPool       ChargingPool,

                                       DateTime?           Timestamp,
                                       EventTracking_Id?   EventTrackingId,
                                       TimeSpan?           RequestTimeout,
                                       CancellationToken   CancellationToken)

                => Task.FromResult(
                       AddChargingPoolResult.NoOperation(
                           ChargingPool,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region AddOrUpdateChargingPool   (ChargingPool,  ...)

        /// <summary>
        /// Add or update the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateChargingPoolResult>

            AddOrUpdateChargingPool(IChargingPool       ChargingPool,

                                    DateTime?           Timestamp,
                                    EventTracking_Id?   EventTrackingId,
                                    TimeSpan?           RequestTimeout,
                                    CancellationToken   CancellationToken)

                => Task.FromResult(
                       AddOrUpdateChargingPoolResult.NoOperation(
                           ChargingPool,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateChargingPool        (ChargingPool,  PropertyName, NewValue, OldValue = null, DataSource = null, ...)

        /// <summary>
        /// Update the given charging pool.
        /// The charging pool can be uploaded as a whole, or just a single property of the charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to update.</param>
        /// <param name="PropertyName">The name of the charging pool property to update.</param>
        /// <param name="NewValue">The new value of the charging pool property to update.</param>
        /// <param name="OldValue">The optional old value of the charging pool property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateChargingPoolResult>

            UpdateChargingPool(IChargingPool       ChargingPool,
                               String?             PropertyName,
                               Object?             NewValue,
                               Object?             OldValue,
                               Context?            DataSource,

                               DateTime?           Timestamp,
                               EventTracking_Id?   EventTrackingId,
                               TimeSpan?           RequestTimeout,
                               CancellationToken   CancellationToken)

                => Task.FromResult(
                       UpdateChargingPoolResult.NoOperation(
                           ChargingPool,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region DeleteChargingPool        (ChargingPool,  ...)

        /// <summary>
        /// Delete the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteChargingPoolResult>

            DeleteChargingPool(IChargingPool       ChargingPool,

                               DateTime?           Timestamp,
                               EventTracking_Id?   EventTrackingId,
                               TimeSpan?           RequestTimeout,
                               CancellationToken   CancellationToken)

                => Task.FromResult(
                       DeleteChargingPoolResult.NoOperation(
                           ChargingPool,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion


        #region AddChargingPools          (ChargingPools, ...)

        /// <summary>
        /// Add the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingPoolsResult>

            AddChargingPools(IEnumerable<IChargingPool>  ChargingPools,

                             DateTime?                   Timestamp,
                             EventTracking_Id?           EventTrackingId,
                             TimeSpan?                   RequestTimeout,
                             CancellationToken           CancellationToken)

                => Task.FromResult(
                       AddChargingPoolsResult.NoOperation(
                           ChargingPools,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region AddChargingPoolsIfNotExist(ChargingPools, ...)

        /// <summary>
        /// Add the given enumeration of charging pools, if they do not already exist.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to add, if they do not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingPoolsResult>

            AddChargingPoolsIfNotExist(IEnumerable<IChargingPool>  ChargingPools,

                                       DateTime?                   Timestamp,
                                       EventTracking_Id?           EventTrackingId,
                                       TimeSpan?                   RequestTimeout,
                                       CancellationToken           CancellationToken)

                => Task.FromResult(
                       AddChargingPoolsResult.NoOperation(
                           ChargingPools,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateChargingPools  (ChargingPools, ...)

        /// <summary>
        /// Add or update the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateChargingPoolsResult>

            AddOrUpdateChargingPools(IEnumerable<IChargingPool>  ChargingPools,

                                     DateTime?                   Timestamp,
                                     EventTracking_Id?           EventTrackingId,
                                     TimeSpan?                   RequestTimeout,
                                     CancellationToken           CancellationToken)

                => Task.FromResult(
                       AddOrUpdateChargingPoolsResult.NoOperation(
                           ChargingPools,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region UpdateChargingPools       (ChargingPools, ...)

        /// <summary>
        /// Update the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateChargingPoolsResult>

            UpdateChargingPools(IEnumerable<IChargingPool>  ChargingPools,

                                DateTime?                   Timestamp,
                                EventTracking_Id?           EventTrackingId,
                                TimeSpan?                   RequestTimeout,
                                CancellationToken           CancellationToken)

                => Task.FromResult(
                       UpdateChargingPoolsResult.NoOperation(
                           ChargingPools,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region DeleteChargingPools       (ChargingPools, ...)

        /// <summary>
        /// Delete the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteChargingPoolsResult>

            DeleteChargingPools(IEnumerable<IChargingPool>  ChargingPools,

                                DateTime?                   Timestamp,
                                EventTracking_Id?           EventTrackingId,
                                TimeSpan?                   RequestTimeout,
                                CancellationToken           CancellationToken)

                => Task.FromResult(
                       DeleteChargingPoolsResult.NoOperation(
                           ChargingPools,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion


        #region UpdateChargingPoolAdminStatus (AdminStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of charging pool admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushChargingPoolAdminStatusResult>

            UpdateChargingPoolAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,

                                          DateTime?                                   Timestamp,
                                          EventTracking_Id?                           EventTrackingId,
                                          TimeSpan?                                   RequestTimeout,
                                          CancellationToken                           CancellationToken)

                => Task.FromResult(
                       PushChargingPoolAdminStatusResult.OutOfService(
                           Id,
                           this,
                           AdminStatusUpdates
                       )
                   );

        #endregion

        #region UpdateChargingPoolStatus      (StatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of charging pool status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging pool status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<PushChargingPoolStatusResult>

            UpdateChargingPoolStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,

                                     DateTime?                              Timestamp,
                                     EventTracking_Id?                      EventTrackingId,
                                     TimeSpan?                              RequestTimeout,
                                     CancellationToken                      CancellationToken)

                => Task.FromResult(
                       PushChargingPoolStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateChargingPoolEnergyStatus(ChargingPoolEnergyStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of charging pool energy status.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdates">An enumeration of charging pool energy status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<PushChargingPoolEnergyStatusResult>

            UpdateChargingPoolEnergyStatus(IEnumerable<ChargingPoolEnergyStatusUpdate>  ChargingPoolEnergyStatusUpdates,

                                           DateTime?                                    Timestamp,
                                           EventTracking_Id?                            EventTrackingId,
                                           TimeSpan?                                    RequestTimeout,
                                           CancellationToken                            CancellationToken)

                => Task.FromResult(
                       PushChargingPoolEnergyStatusResult.OutOfService(
                           Id,
                           this,
                           ChargingPoolEnergyStatusUpdates
                       )
                   );

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        #region AddChargingStation           (ChargingStation,  ...)

        /// <summary>
        /// Add the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingStationResult>

            AddChargingStation(IChargingStation   ChargingStation,

                               DateTime?          Timestamp,
                               EventTracking_Id?  EventTrackingId,
                               TimeSpan?          RequestTimeout,
                               CancellationToken  CancellationToken)

                => Task.FromResult(
                       AddChargingStationResult.NoOperation(
                           ChargingStation,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region AddChargingStationIfNotExists(ChargingStation,  ...)

        /// <summary>
        /// Add the given charging station, if it does not already exist.
        /// </summary>
        /// <param name="ChargingStation">A charging station to add, if it does not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingStationResult>

            AddChargingStationIfNotExists(IChargingStation   ChargingStation,

                                          DateTime?          Timestamp,
                                          EventTracking_Id?  EventTrackingId,
                                          TimeSpan?          RequestTimeout,
                                          CancellationToken  CancellationToken)

                => Task.FromResult(
                       AddChargingStationResult.NoOperation(
                           ChargingStation,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region AddOrUpdateChargingStation   (ChargingStation,  ...)

        /// <summary>
        /// Add or update the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateChargingStationResult>

            AddOrUpdateChargingStation(IChargingStation   ChargingStation,

                                       DateTime?          Timestamp,
                                       EventTracking_Id?  EventTrackingId,
                                       TimeSpan?          RequestTimeout,
                                       CancellationToken  CancellationToken)

                => Task.FromResult(
                       AddOrUpdateChargingStationResult.NoOperation(
                           ChargingStation,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateChargingStation        (ChargingStation,  PropertyName, NewValue, OldValue = null, DataSource = null, ...)

        /// <summary>
        /// Update the given charging station.
        /// The charging station can be uploaded as a whole, or just a single property of the charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to update.</param>
        /// <param name="PropertyName">The name of the charging station property to update.</param>
        /// <param name="NewValue">The new value of the charging station property to update.</param>
        /// <param name="OldValue">The optional old value of the charging station property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateChargingStationResult>

            UpdateChargingStation(IChargingStation   ChargingStation,
                                  String?            PropertyName,
                                  Object?            OldValue,
                                  Object?            NewValue,
                                  Context?           DataSource,

                                  DateTime?          Timestamp,
                                  EventTracking_Id?  EventTrackingId,
                                  TimeSpan?          RequestTimeout,
                                  CancellationToken  CancellationToken)

                => Task.FromResult(
                       UpdateChargingStationResult.NoOperation(
                           ChargingStation,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region DeleteChargingStation        (ChargingStation,  ...)

        /// <summary>
        /// Delete the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteChargingStationResult>

            DeleteChargingStation(IChargingStation    ChargingStation,

                                  DateTime?           Timestamp,
                                  EventTracking_Id?   EventTrackingId,
                                  TimeSpan?           RequestTimeout,
                                  CancellationToken   CancellationToken)

                => Task.FromResult(
                       DeleteChargingStationResult.NoOperation(
                           ChargingStation,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion


        #region AddChargingStations          (ChargingStations, ...)

        /// <summary>
        /// Add the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingStationsResult>

            AddChargingStations(IEnumerable<IChargingStation>  ChargingStations,

                                DateTime?                      Timestamp,
                                EventTracking_Id?              EventTrackingId,
                                TimeSpan?                      RequestTimeout,
                                CancellationToken              CancellationToken)

                => Task.FromResult(
                       AddChargingStationsResult.NoOperation(
                           ChargingStations,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region AddChargingStationsIfNotExist(ChargingStations, ...)

        /// <summary>
        /// Add the given enumeration of charging stations, if they do not already exist..
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to add, if they do not already exist..</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingStationsResult>

            AddChargingStationsIfNotExist(IEnumerable<IChargingStation>  ChargingStations,

                                          DateTime?                      Timestamp,
                                          EventTracking_Id?              EventTrackingId,
                                          TimeSpan?                      RequestTimeout,
                                          CancellationToken              CancellationToken)

                => Task.FromResult(
                       AddChargingStationsResult.NoOperation(
                           ChargingStations,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateChargingStations  (ChargingStations, ...)

        /// <summary>
        /// Add or update the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateChargingStationsResult>

            AddOrUpdateChargingStations(IEnumerable<IChargingStation>  ChargingStations,

                                        DateTime?                      Timestamp,
                                        EventTracking_Id?              EventTrackingId,
                                        TimeSpan?                      RequestTimeout,
                                        CancellationToken              CancellationToken)

                => Task.FromResult(
                       AddOrUpdateChargingStationsResult.NoOperation(
                           ChargingStations,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region UpdateChargingStations       (ChargingStations, ...)

        /// <summary>
        /// Update the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateChargingStationsResult>

            UpdateChargingStations(IEnumerable<IChargingStation>  ChargingStations,

                                   DateTime?                      Timestamp,
                                   EventTracking_Id?              EventTrackingId,
                                   TimeSpan?                      RequestTimeout,
                                   CancellationToken              CancellationToken)

                => Task.FromResult(
                       UpdateChargingStationsResult.NoOperation(
                           ChargingStations,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region DeleteChargingStations       (ChargingStations, ...)

        /// <summary>
        /// Delete the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteChargingStationsResult>

            DeleteChargingStations(IEnumerable<IChargingStation>  ChargingStations,

                                   DateTime?                      Timestamp,
                                   EventTracking_Id?              EventTrackingId,
                                   TimeSpan?                      RequestTimeout,
                                   CancellationToken              CancellationToken)

                => Task.FromResult(
                       DeleteChargingStationsResult.NoOperation(
                           ChargingStations,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion


        #region UpdateChargingStationAdminStatus (AdminStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of charging station admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station admin status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushChargingStationAdminStatusResult>

            UpdateChargingStationAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,

                                             DateTime?                                      Timestamp,
                                             EventTracking_Id?                              EventTrackingId,
                                             TimeSpan?                                      RequestTimeout,
                                             CancellationToken                              CancellationToken)

                => Task.FromResult(
                       PushChargingStationAdminStatusResult.OutOfService(
                           Id,
                           this,
                           AdminStatusUpdates
                       )
                   );

        #endregion

        #region UpdateChargingStationStatus      (StatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of charging station status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<PushChargingStationStatusResult>

            UpdateChargingStationStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,

                                        DateTime?                                 Timestamp,
                                        EventTracking_Id?                         EventTrackingId,
                                        TimeSpan?                                 RequestTimeout,
                                        CancellationToken                         CancellationToken)

                => Task.FromResult(
                       PushChargingStationStatusResult.OutOfService(
                           Id,
                           this,
                           StatusUpdates
                       )
                   );

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
        public virtual Task<PushChargingStationEnergyStatusResult>

            UpdateChargingStationEnergyStatus(IEnumerable<ChargingStationEnergyStatusUpdate>  ChargingStationEnergyStatusUpdates,

                                              DateTime?                                       Timestamp,
                                              EventTracking_Id?                               EventTrackingId,
                                              TimeSpan?                                       RequestTimeout,
                                              CancellationToken                               CancellationToken)

                => Task.FromResult(
                       PushChargingStationEnergyStatusResult.OutOfService(
                           Id,
                           this,
                           ChargingStationEnergyStatusUpdates
                       )
                   );

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) EVSE(s)...

        #region AddEVSE           (EVSE,  ...)

        /// <summary>
        /// Add the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddEVSEResult>

            AddEVSE(IEVSE              EVSE,

                    DateTime?          Timestamp,
                    EventTracking_Id?  EventTrackingId,
                    TimeSpan?          RequestTimeout,
                    CancellationToken  CancellationToken)

                => Task.FromResult(
                       AddEVSEResult.NoOperation(
                           EVSE,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region AddEVSEIfNotExists(EVSE,  ...)

        /// <summary>
        /// Add the given EVSE, if it does not already exist.
        /// </summary>
        /// <param name="EVSE">An EVSE to add, if it does not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddEVSEResult>

            AddEVSEIfNotExists(IEVSE              EVSE,

                               DateTime?          Timestamp,
                               EventTracking_Id?  EventTrackingId,
                               TimeSpan?          RequestTimeout,
                               CancellationToken  CancellationToken)

                => Task.FromResult(
                       AddEVSEResult.NoOperation(
                           EVSE,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region AddOrUpdateEVSE   (EVSE,  ...)

        /// <summary>
        /// Add or update the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateEVSEResult>

            AddOrUpdateEVSE(IEVSE              EVSE,

                            DateTime?          Timestamp,
                            EventTracking_Id?  EventTrackingId,
                            TimeSpan?          RequestTimeout,
                            CancellationToken  CancellationToken)

                => Task.FromResult(
                       AddOrUpdateEVSEResult.NoOperation(
                           EVSE,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateEVSE        (EVSE,  PropertyName, NewValue, OldValue = null, DataSource = null, ...)

        /// <summary>
        /// Update the given EVSE.
        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="OldValue">The optional old value of the EVSE property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateEVSEResult>

            UpdateEVSE(IEVSE               EVSE,
                       String?             PropertyName,
                       Object?             OldValue,
                       Object?             NewValue,
                       Context?            DataSource,

                       DateTime?           Timestamp,
                       EventTracking_Id?   EventTrackingId,
                       TimeSpan?           RequestTimeout,
                       CancellationToken   CancellationToken)

                => Task.FromResult(
                       UpdateEVSEResult.NoOperation(
                           EVSE,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion

        #region DeleteEVSE        (EVSE,  ...)

        /// <summary>
        /// Delete the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteEVSEResult>

            DeleteEVSE(IEVSE              EVSE,

                       DateTime?          Timestamp,
                       EventTracking_Id?  EventTrackingId,
                       TimeSpan?          RequestTimeout,
                       CancellationToken  CancellationToken)

                => Task.FromResult(
                       DeleteEVSEResult.NoOperation(
                           EVSE,
                           EventTrackingId,
                           Id,
                           this
                       )
                   );

        #endregion


        #region AddEVSEs          (EVSEs, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddEVSEsResult>

            AddEVSEs(IEnumerable<IEVSE>  EVSEs,

                     DateTime?           Timestamp,
                     EventTracking_Id?   EventTrackingId,
                     TimeSpan?           RequestTimeout,
                     CancellationToken   CancellationToken)

                => Task.FromResult(
                       AddEVSEsResult.NoOperation(
                           EVSEs,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region AddEVSEsIfNotExist(EVSEs, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs, if they do not already exist.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add, if they do not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddEVSEsResult>

            AddEVSEsIfNotExist(IEnumerable<IEVSE>  EVSEs,

                               DateTime?           Timestamp,
                               EventTracking_Id?   EventTrackingId,
                               TimeSpan?           RequestTimeout,
                               CancellationToken   CancellationToken)

                => Task.FromResult(
                       AddEVSEsResult.NoOperation(
                           EVSEs,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateEVSEs  (EVSEs, ...)

        /// <summary>
        /// Add or update the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateEVSEsResult>

            AddOrUpdateEVSEs(IEnumerable<IEVSE>  EVSEs,

                             DateTime?           Timestamp,
                             EventTracking_Id?   EventTrackingId,
                             TimeSpan?           RequestTimeout,
                             CancellationToken   CancellationToken)

                => Task.FromResult(
                       AddOrUpdateEVSEsResult.NoOperation(
                           EVSEs,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region UpdateEVSEs       (EVSEs, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateEVSEsResult>

            UpdateEVSEs(IEnumerable<IEVSE>  EVSEs,

                        DateTime?           Timestamp,
                        EventTracking_Id?   EventTrackingId,
                        TimeSpan?           RequestTimeout,
                        CancellationToken   CancellationToken)

                => Task.FromResult(
                       UpdateEVSEsResult.NoOperation(
                           EVSEs,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion

        #region DeleteEVSEs       (EVSEs, ...)

        /// <summary>
        /// Delete the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteEVSEsResult>

            DeleteEVSEs(IEnumerable<IEVSE>  EVSEs,

                        DateTime?           Timestamp,
                        EventTracking_Id?   EventTrackingId,
                        TimeSpan?           RequestTimeout,
                        CancellationToken   CancellationToken)

                => Task.FromResult(
                       DeleteEVSEsResult.NoOperation(
                           EVSEs,
                           Id,
                           this,
                           EventTrackingId
                       )
                   );

        #endregion


        #region UpdateEVSEAdminStatus (AdminStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushEVSEAdminStatusResult>

            UpdateEVSEAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,

                                  DateTime?                           Timestamp,
                                  EventTracking_Id?                   EventTrackingId,
                                  TimeSpan?                           RequestTimeout,
                                  CancellationToken                   CancellationToken)

                => Task.FromResult(
                       PushEVSEAdminStatusResult.OutOfService(
                           Id,
                           this,
                           AdminStatusUpdates
                       )
                   );

        #endregion

        #region UpdateEVSEStatus      (StatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushEVSEStatusResult>

            UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,

                             DateTime?                      Timestamp,
                             EventTracking_Id?              EventTrackingId,
                             TimeSpan?                      RequestTimeout,
                             CancellationToken              CancellationToken)

                => Task.FromResult(
                       PushEVSEStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );


        #endregion

        #region UpdateEVSEEnergyStatus(EVSEEnergyStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of EVSE energy status.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdates">An enumeration of EVSE energy status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<PushEVSEEnergyStatusResult>

            UpdateEVSEEnergyStatus(IEnumerable<EVSEEnergyStatusUpdate>  EVSEEnergyStatusUpdates,

                                   DateTime?                            Timestamp,
                                   EventTracking_Id?                    EventTrackingId,
                                   TimeSpan?                            RequestTimeout,
                                   CancellationToken                    CancellationToken)

                => Task.FromResult(
                       PushEVSEEnergyStatusResult.OutOfService(
                           Id,
                           this,
                           EVSEEnergyStatusUpdates
                       )
                   );

        #endregion

        #endregion

        #endregion


    }

}
