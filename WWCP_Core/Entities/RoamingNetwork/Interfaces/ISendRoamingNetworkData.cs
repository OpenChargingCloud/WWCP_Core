/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    public interface ISendRoamingNetworkData
    {

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        Boolean                          DisableSendRoamingNetworkData    { get; set; }


        /// <summary>
        /// Only include EVSE identifications matching the given delegate.
        /// </summary>
        IncludeRoamingNetworkIdDelegate  IncludeRoamingNetworkIds         { get; }

        /// <summary>
        /// Only include EVSEs matching the given delegate.
        /// </summary>
        IncludeRoamingNetworkDelegate    IncludeRoamingNetworks           { get; }


        #region AddRoamingNetwork            (RoamingNetwork, ...)

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
        Task<AddRoamingNetworkResult>

            AddRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                              TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                              DateTime?          Timestamp           = null,
                              EventTracking_Id?  EventTrackingId     = null,
                              TimeSpan?          RequestTimeout      = null,
                              User_Id?           CurrentUserId       = null,
                              CancellationToken  CancellationToken   = default);

        #endregion

        #region AddRoamingNetworkIfNotExists (RoamingNetwork, ...)

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
        Task<AddRoamingNetworkResult>

            AddRoamingNetworkIfNotExists(IRoamingNetwork    RoamingNetwork,
                                         TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                         DateTime?          Timestamp           = null,
                                         EventTracking_Id?  EventTrackingId     = null,
                                         TimeSpan?          RequestTimeout      = null,
                                         User_Id?           CurrentUserId       = null,
                                         CancellationToken  CancellationToken   = default);

        #endregion

        #region AddOrUpdateRoamingNetwork    (RoamingNetwork, ...)

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
        Task<AddOrUpdateRoamingNetworkResult>

            AddOrUpdateRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                                      TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                      DateTime?          Timestamp           = null,
                                      EventTracking_Id?  EventTrackingId     = null,
                                      TimeSpan?          RequestTimeout      = null,
                                      User_Id?           CurrentUserId       = null,
                                      CancellationToken  CancellationToken   = default);

        #endregion

        #region UpdateRoamingNetwork         (RoamingNetwork, PropertyName, NewValue, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="PropertyName">The name of the roaming network property to update.</param>
        /// <param name="NewValue">The new value of the roaming network property to update.</param>
        /// <param name="OldValue">The optional old value of the roaming network property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateRoamingNetworkResult>

            UpdateRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                                 String             PropertyName,
                                 Object?            NewValue,
                                 Object?            OldValue            = null,
                                 Context?           DataSource          = null,
                                 TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                 DateTime?          Timestamp           = null,
                                 EventTracking_Id?  EventTrackingId     = null,
                                 TimeSpan?          RequestTimeout      = null,
                                 User_Id?           CurrentUserId       = null,
                                 CancellationToken  CancellationToken   = default);

        #endregion

        #region DeleteRoamingNetwork         (RoamingNetwork, ...)

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
        Task<DeleteRoamingNetworkResult>

            DeleteRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                                 TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                 DateTime?          Timestamp           = null,
                                 EventTracking_Id?  EventTrackingId     = null,
                                 TimeSpan?          RequestTimeout      = null,
                                 User_Id?           CurrentUserId       = null,
                                 CancellationToken  CancellationToken   = default);

        #endregion


        #region AddRoamingNetworks           (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AddRoamingNetworksResult>

            AddRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                               TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?                     Timestamp           = null,
                               EventTracking_Id?             EventTrackingId     = null,
                               TimeSpan?                     RequestTimeout      = null,
                               User_Id?                      CurrentUserId       = null,
                               CancellationToken             CancellationToken   = default);

        #endregion

        #region AddRoamingNetworksIfNotExist (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of roaming networks, if they do not already exist.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add, if they do not already exist.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AddRoamingNetworksResult>

            AddRoamingNetworksIfNotExist(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                         TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                         DateTime?                     Timestamp           = null,
                                         EventTracking_Id?             EventTrackingId     = null,
                                         TimeSpan?                     RequestTimeout      = null,
                                         User_Id?                      CurrentUserId       = null,
                                         CancellationToken             CancellationToken   = default);

        #endregion

        #region AddOrUpdateRoamingNetworks   (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add or update.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateRoamingNetworksResult>

            AddOrUpdateRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                       TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTime?                     Timestamp           = null,
                                       EventTracking_Id?             EventTrackingId     = null,
                                       TimeSpan?                     RequestTimeout      = null,
                                       User_Id?                      CurrentUserId       = null,
                                       CancellationToken             CancellationToken   = default);

        #endregion

        #region UpdateRoamingNetworks        (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to update.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateRoamingNetworksResult>

            UpdateRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                  TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                  DateTime?                     Timestamp           = null,
                                  EventTracking_Id?             EventTrackingId     = null,
                                  TimeSpan?                     RequestTimeout      = null,
                                  User_Id?                      CurrentUserId       = null,
                                  CancellationToken             CancellationToken   = default);

        #endregion

        #region DeleteRoamingNetworks        (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to delete.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteRoamingNetworksResult>

            DeleteRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                  TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                  DateTime?                     Timestamp           = null,
                                  EventTracking_Id?             EventTrackingId     = null,
                                  TimeSpan?                     RequestTimeout      = null,
                                  User_Id?                      CurrentUserId       = null,
                                  CancellationToken             CancellationToken   = default);

        #endregion


    }

}
