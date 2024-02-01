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

    public delegate IRemoteEMobilityStation RemoteEMobilityStationCreatorDelegate(eMobilityStation eMobilityStation);

    public delegate String eMobilityStationNameSelectorDelegate(I18NString I18NText);


    /// <summary>
    /// A delegate called whenever the static data of the charging station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="eMobilityStation">The updated charging station.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnEMobilityStationDataChangedDelegate(DateTime          Timestamp,
                                                               EventTracking_Id  EventTrackingId,
                                                               eMobilityStation  eMobilityStation,
                                                               String            PropertyName,
                                                               Object?           NewValue,
                                                               Object?           OldValue     = null,
                                                               Context?          DataSource   = null);

    /// <summary>
    /// A delegate called whenever the admin status of the charging station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="eMobilityStation">The updated charging station.</param>
    /// <param name="OldStatus">The old timestamped status of the charging station.</param>
    /// <param name="NewStatus">The new timestamped status of the charging station.</param>
    public delegate Task OnEMobilityStationAdminStatusChangedDelegate(DateTime                                        Timestamp,
                                                                      EventTracking_Id                                EventTrackingId,
                                                                      eMobilityStation                                eMobilityStation,
                                                                      Timestamped<eMobilityStationAdminStatusTypes>   NewStatus,
                                                                      Timestamped<eMobilityStationAdminStatusTypes>?  OldStatus    = null,
                                                                      Context?                                        DataSource   = null);


    /// <summary>
    /// A delegate called whenever the admin status of the charging station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="eMobilityStation">The updated charging station.</param>
    /// <param name="OldStatus">The old timestamped status of the charging station.</param>
    /// <param name="NewStatus">The new timestamped status of the charging station.</param>
    public delegate Task OnEMobilityStationStatusChangedDelegate(DateTime                                   Timestamp,
                                                                 EventTracking_Id                           EventTrackingId,
                                                                 eMobilityStation                           eMobilityStation,
                                                                 Timestamped<eMobilityStationStatusTypes>   NewStatus,
                                                                 Timestamped<eMobilityStationStatusTypes>?  OldStatus    = null,
                                                                 Context?                                   DataSource   = null);

}
