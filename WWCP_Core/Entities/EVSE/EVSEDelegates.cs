/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    public delegate IRemoteEVSE RemoteEVSECreatorDelegate(EVSE EVSE);


    /// <summary>
    /// A delegate called whenever the static data of the EVSE changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSE">The updated EVSE.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    /// <param name="OldValue">The optional old value of the changed property.</param>
    /// <param name="DataSource">An optional data source or context for the status update.</param>
    public delegate Task OnEVSEDataChangedDelegate(DateTimeOffset    Timestamp,
                                                   EventTracking_Id  EventTrackingId,
                                                   IEVSE             EVSE,
                                                   String            PropertyName,
                                                   Object?           NewValue,
                                                   Object?           OldValue     = null,
                                                   Context?          DataSource   = null);

    /// <summary>
    /// A delegate called whenever the admin status of the EVSE changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSE">The EVSE.</param>
    /// <param name="NewEVSEStatus">The new timestamped status of the EVSE.</param>
    /// <param name="OldEVSEStatus">The optional old timestamped status of the EVSE.</param>
    /// <param name="DataSource">An optional data source or context for the status update.</param>
    public delegate Task OnEVSEAdminStatusChangedDelegate(DateTimeOffset                     Timestamp,
                                                          EventTracking_Id                   EventTrackingId,
                                                          IEVSE                              EVSE,
                                                          Timestamped<EVSEAdminStatusType>   NewEVSEStatus,
                                                          Timestamped<EVSEAdminStatusType>?  OldEVSEStatus   = null,
                                                          Context?                           DataSource      = null);

    /// <summary>
    /// A delegate called whenever the dynamic status of the EVSE changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSE">The EVSE.</param>
    /// <param name="NewEVSEStatus">The new timestamped status of the EVSE.</param>
    /// <param name="OldEVSEStatus">The optional old timestamped status of the EVSE.</param>
    /// <param name="DataSource">An optional data source or context for the status update.</param>
    public delegate Task OnEVSEStatusChangedDelegate(DateTimeOffset                Timestamp,
                                                     EventTracking_Id              EventTrackingId,
                                                     IEVSE                         EVSE,
                                                     Timestamped<EVSEStatusType>   NewEVSEStatus,
                                                     Timestamped<EVSEStatusType>?  OldEVSEStatus   = null,
                                                     Context?                      DataSource      = null);

}
