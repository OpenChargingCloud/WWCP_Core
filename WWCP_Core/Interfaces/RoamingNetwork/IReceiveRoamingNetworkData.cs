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
    public interface IReceiveRoamingNetworkData
    {

        #region AddRoamingNetwork           (RoamingNetwork, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddRoamingNetworkResult>

            AddRoamingNetwork(IRoamingNetwork    RoamingNetwork,

                              DateTime?          Timestamp           = null,
                              EventTracking_Id?  EventTrackingId     = null,
                              TimeSpan?          RequestTimeout      = null,
                              CancellationToken  CancellationToken   = default);

        #endregion

        #region AddRoamingNetworkIfNotExists(RoamingNetwork, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddRoamingNetworkResult>

            AddRoamingNetworkIfNotExists(IRoamingNetwork    RoamingNetwork,

                                         DateTime?          Timestamp           = null,
                                         EventTracking_Id?  EventTrackingId     = null,
                                         TimeSpan?          RequestTimeout      = null,
                                         CancellationToken  CancellationToken   = default);

        #endregion

        #region AddOrUpdateRoamingNetwork   (RoamingNetwork, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateRoamingNetworkResult>

            AddOrUpdateRoamingNetwork(IRoamingNetwork    RoamingNetwork,

                                      DateTime?          Timestamp           = null,
                                      EventTracking_Id?  EventTrackingId     = null,
                                      TimeSpan?          RequestTimeout      = null,
                                      CancellationToken  CancellationToken   = default);

        #endregion

        #region UpdateRoamingNetwork        (RoamingNetwork, PropertyName, NewValue, OldValue = null, DataSource = null, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="PropertyName">The name of the roaming network property to update.</param>
        /// <param name="NewValue">The new value of the roaming network property to update.</param>
        /// <param name="OldValue">The optinal old value of the roaming network property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
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

                                 DateTime?          Timestamp           = null,
                                 EventTracking_Id?  EventTrackingId     = null,
                                 TimeSpan?          RequestTimeout      = null,
                                 CancellationToken  CancellationToken   = default);

        #endregion

        #region DeleteRoamingNetwork        (RoamingNetwork, ...)

        /// <summary>
        /// Upload the static data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteRoamingNetworkResult>

            DeleteRoamingNetwork(IRoamingNetwork    RoamingNetwork,

                                 DateTime?          Timestamp           = null,
                                 EventTracking_Id?  EventTrackingId     = null,
                                 TimeSpan?          RequestTimeout      = null,
                                 CancellationToken  CancellationToken   = default);

        #endregion


        #region AddRoamingNetworks          (RoamingNetworks, ...)

        /// <summary>
        /// Add the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AddRoamingNetworksResult>

            AddRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,

                               DateTime?                     Timestamp           = null,
                               EventTracking_Id?             EventTrackingId     = null,
                               TimeSpan?                     RequestTimeout      = null,
                               CancellationToken             CancellationToken   = default);

        #endregion

        #region AddRoamingNetworksIfNotExist(RoamingNetworks, ...)

        /// <summary>
        /// Add the given enumeration of roaming networks, if they do not already exist.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add, if they do not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AddRoamingNetworksResult>

            AddRoamingNetworksIfNotExist(IEnumerable<IRoamingNetwork>  RoamingNetworks,

                                         DateTime?                     Timestamp           = null,
                                         EventTracking_Id?             EventTrackingId     = null,
                                         TimeSpan?                     RequestTimeout      = null,
                                         CancellationToken             CancellationToken   = default);

        #endregion

        #region AddOrUpdateRoamingNetworks  (RoamingNetworks, ...)

        /// <summary>
        /// Add or update the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateRoamingNetworksResult>

            AddOrUpdateRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,

                                       DateTime?                     Timestamp           = null,
                                       EventTracking_Id?             EventTrackingId     = null,
                                       TimeSpan?                     RequestTimeout      = null,
                                       CancellationToken             CancellationToken   = default);

        #endregion

        #region UpdateRoamingNetworks       (RoamingNetworks, ...)

        /// <summary>
        /// Update the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateRoamingNetworksResult>

            UpdateRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,

                                  DateTime?                     Timestamp           = null,
                                  EventTracking_Id?             EventTrackingId     = null,
                                  TimeSpan?                     RequestTimeout      = null,
                                  CancellationToken             CancellationToken   = default);

        #endregion

        #region DeleteRoamingNetworks       (RoamingNetworks, ...)

        /// <summary>
        /// Delete the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteRoamingNetworksResult>

            DeleteRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,

                                  DateTime?                     Timestamp           = null,
                                  EventTracking_Id?             EventTrackingId     = null,
                                  TimeSpan?                     RequestTimeout      = null,
                                  CancellationToken             CancellationToken   = default);

        #endregion


    }

}
