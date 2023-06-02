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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public delegate IRemoteEVehicle RemoteEVehicleCreatorDelegate(EVehicle eVehicle);

    public delegate String eVehicleNameSelectorDelegate(I18NString I18NText);


    /// <summary>
    /// A delegate called whenever the static data of the electric vehicle changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="eVehicle">The updated electric vehicle.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnEVehicleDataChangedDelegate(DateTime          Timestamp,
                                                       EventTracking_Id  EventTrackingId,
                                                       EVehicle          eVehicle,
                                                       String            PropertyName,
                                                       Object?           NewValue,
                                                       Object?           OldValue     = null,
                                                       Context?          DataSource   = null);

    /// <summary>
    /// A delegate called whenever the admin status of the electric vehicle changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="eVehicle">The updated electric vehicle.</param>
    /// <param name="OldStatus">The old timestamped status of the electric vehicle.</param>
    /// <param name="NewStatus">The new timestamped status of the electric vehicle.</param>
    public delegate Task OnEVehicleAdminStatusChangedDelegate(DateTime                                Timestamp,
                                                              EventTracking_Id                        EventTrackingId,
                                                              EVehicle                                eVehicle,
                                                              Timestamped<eVehicleAdminStatusTypes>   NewStatus,
                                                              Timestamped<eVehicleAdminStatusTypes>?  OldStatus    = null,
                                                              Context?                                DataSource   = null);

    /// <summary>
    /// A delegate called whenever the dynamic status of the electric vehicle changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="eVehicle">The updated electric vehicle.</param>
    /// <param name="OldStatus">The old timestamped status of the electric vehicle.</param>
    /// <param name="NewStatus">The new timestamped status of the electric vehicle.</param>
    public delegate Task OnEVehicleStatusChangedDelegate(DateTime                           Timestamp,
                                                         EventTracking_Id                   EventTrackingId,
                                                         EVehicle                           eVehicle,
                                                         Timestamped<eVehicleStatusTypes>   NewStatus,
                                                         Timestamped<eVehicleStatusTypes>?  OldStatus    = null,
                                                         Context?                           DataSource   = null);

    /// <summary>
    /// A delegate called whenever the geo coordinate of the electric vehicle changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="eVehicle">The updated electric vehicle.</param>
    /// <param name="OldGeoCoordinate">The old timestamped geo coordinate of the electric vehicle.</param>
    /// <param name="NewGeoCoordinate">The new timestamped geo coordinate of the electric vehicle.</param>
    public delegate Task OnEVehicleGeoLocationChangedDelegate(DateTime                     Timestamp,
                                                              EventTracking_Id             EventTrackingId,
                                                              EVehicle                     eVehicle,
                                                              Timestamped<GeoCoordinate>   NewGeoCoordinate,
                                                              Timestamped<GeoCoordinate>?  OldGeoCoordinate   = null,
                                                              Context?                     DataSource         = null);

}
