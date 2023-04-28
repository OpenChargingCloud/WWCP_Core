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
    /// A delegate called whenever the static data of the roaming network changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetwork">The updated roaming network.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnRoamingNetworkDataChangedDelegate(DateTime          Timestamp,
                                                             EventTracking_Id  EventTrackingId,
                                                             IRoamingNetwork   RoamingNetwork,
                                                             String?           PropertyName,
                                                             Object?           NewValue,
                                                             Object?           OldValue,
                                                             String?           DataSource);


    /// <summary>
    /// A delegate called whenever the admin status of the roaming network changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetwork">The updated roaming network.</param>
    /// <param name="NewStatus">The new timestamped admin status of the roaming network.</param>
    /// <param name="OldStatus">The optional old timestamped admin status of the roaming network.</param>
    /// <param name="DataSource">An optional data source or context for this roaming network admin status update.</param>
    public delegate Task OnRoamingNetworkAdminStatusChangedDelegate(DateTime                                      Timestamp,
                                                                    EventTracking_Id                              EventTrackingId,
                                                                    IRoamingNetwork                               RoamingNetwork,
                                                                    Timestamped<RoamingNetworkAdminStatusTypes>   NewStatus,
                                                                    Timestamped<RoamingNetworkAdminStatusTypes>?  OldStatus    = null,
                                                                    String?                                       DataSource   = null);


    /// <summary>
    /// A delegate called whenever the dynamic status of the roaming network changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetwork">The updated roaming network.</param>
    /// <param name="NewStatus">The new timestamped status of the roaming network.</param>
    /// <param name="OldStatus">The optional old timestamped status of the roaming network.</param>
    /// <param name="DataSource">An optional data source or context for this roaming network status update.</param>
    public delegate Task OnRoamingNetworkStatusChangedDelegate(DateTime                                 Timestamp,
                                                               EventTracking_Id                         EventTrackingId,
                                                               IRoamingNetwork                          RoamingNetwork,
                                                               Timestamped<RoamingNetworkStatusTypes>   NewStatus,
                                                               Timestamped<RoamingNetworkStatusTypes>?  OldStatus    = null,
                                                               String?                                  DataSource   = null);

}
