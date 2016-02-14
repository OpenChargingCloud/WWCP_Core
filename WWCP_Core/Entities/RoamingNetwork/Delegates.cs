/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A delegate called whenever the static data of the roaming network changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="RoamingNetwork">The updated roaming network.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnRoamingNetworkDataChangedDelegate(DateTime Timestamp, RoamingNetwork RoamingNetwork, String PropertyName, Object OldValue, Object NewValue);

    /// <summary>
    /// A delegate called whenever the dynamic status of the roaming network changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="RoamingNetwork">The updated roaming network.</param>
    /// <param name="OldStatus">The old timestamped status of the roaming network.</param>
    /// <param name="NewStatus">The new timestamped status of the roaming network.</param>
    public delegate Task OnRoamingNetworkStatusChangedDelegate(DateTime Timestamp, RoamingNetwork RoamingNetwork, Timestamped<RoamingNetworkStatusType> OldStatus, Timestamped<RoamingNetworkStatusType> NewStatus);

    /// <summary>
    /// A delegate called whenever the admin status of the roaming network changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="RoamingNetwork">The updated roaming network.</param>
    /// <param name="OldStatus">The old timestamped status of the roaming network.</param>
    /// <param name="NewStatus">The new timestamped status of the roaming network.</param>
    public delegate Task OnRoamingNetworkAdminStatusChangedDelegate(DateTime Timestamp, RoamingNetwork RoamingNetwork, Timestamped<RoamingNetworkAdminStatusType> OldStatus, Timestamped<RoamingNetworkAdminStatusType> NewStatus);




    /// <summary>
    /// A delegate called whenever the static data of the EVSE operator changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EVSEOperator">The updated evse operator.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnEVSEOperatorDataChangedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, String PropertyName, Object OldValue, Object NewValue);

    /// <summary>
    /// A delegate called whenever the dynamic status of the EVSE operator changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EVSEOperator">The updated EVSE operator.</param>
    /// <param name="OldStatus">The old timestamped status of the EVSE operator.</param>
    /// <param name="NewStatus">The new timestamped status of the EVSE operator.</param>
    public delegate Task OnEVSEOperatorStatusChangedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, Timestamped<EVSEOperatorStatusType> OldStatus, Timestamped<EVSEOperatorStatusType> NewStatus);

    /// <summary>
    /// A delegate called whenever the admin status of the EVSE operator changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EVSEOperator">The updated EVSE operator.</param>
    /// <param name="OldStatus">The old timestamped status of the EVSE operator.</param>
    /// <param name="NewStatus">The new timestamped status of the EVSE operator.</param>
    public delegate Task OnEVSEOperatorAdminStatusChangedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, Timestamped<EVSEOperatorAdminStatusType> OldStatus, Timestamped<EVSEOperatorAdminStatusType> NewStatus);




    /// <summary>
    /// A delegate called whenever the static data of the charging pool changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingPool">The updated charging pool.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnChargingPoolDataChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, String PropertyName, Object OldValue, Object NewValue);

    /// <summary>
    /// A delegate called whenever the dynamic status of the charging pool changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingPool">The updated charging pool.</param>
    /// <param name="OldStatus">The old timestamped status of the charging pool.</param>
    /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
    public delegate Task OnChargingPoolStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<ChargingPoolStatusType> OldStatus, Timestamped<ChargingPoolStatusType> NewStatus);

    /// <summary>
    /// A delegate called whenever the admin status of the charging pool changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingPool">The updated charging pool.</param>
    /// <param name="OldStatus">The old timestamped status of the charging pool.</param>
    /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
    public delegate Task OnChargingPoolAdminStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<ChargingPoolAdminStatusType> OldStatus, Timestamped<ChargingPoolAdminStatusType> NewStatus);




    /// <summary>
    /// A delegate called whenever the static data of the charging station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingStation">The updated charging station.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnChargingStationDataChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, String PropertyName, Object OldValue, Object NewValue);

    /// <summary>
    /// A delegate called whenever the dynamic status of the charging station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingStation">The updated charging station.</param>
    /// <param name="OldStatus">The old timestamped status of the charging station.</param>
    /// <param name="NewStatus">The new timestamped status of the charging station.</param>
    public delegate Task OnChargingStationStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<ChargingStationStatusType> OldStatus, Timestamped<ChargingStationStatusType> NewStatus);

    /// <summary>
    /// A delegate called whenever the admin status of the charging station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingStation">The updated charging station.</param>
    /// <param name="OldStatus">The old timestamped status of the charging station.</param>
    /// <param name="NewStatus">The new timestamped status of the charging station.</param>
    public delegate Task OnChargingStationAdminStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<ChargingStationAdminStatusType> OldStatus, Timestamped<ChargingStationAdminStatusType> NewStatus);




    /// <summary>
    /// A delegate called whenever the static data of the EVSE changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EVSE">The updated EVSE.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnEVSEDataChangedDelegate(DateTime Timestamp, EVSE EVSE, String PropertyName, Object OldValue, Object NewValue);

    /// <summary>
    /// A delegate called whenever the dynamic status of the EVSE changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EVSE">The EVSE.</param>
    /// <param name="OldEVSEStatus">The old timestamped status of the EVSE.</param>
    /// <param name="NewEVSEStatus">The new timestamped status of the EVSE.</param>
    public delegate Task OnEVSEStatusChangedDelegate(DateTime Timestamp, EVSE EVSE, Timestamped<EVSEStatusType> OldEVSEStatus, Timestamped<EVSEStatusType> NewEVSEStatus);

    /// <summary>
    /// A delegate called whenever the admin status of the EVSE changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EVSE">The EVSE.</param>
    /// <param name="OldEVSEStatus">The old timestamped status of the EVSE.</param>
    /// <param name="NewEVSEStatus">The new timestamped status of the EVSE.</param>
    public delegate Task OnEVSEAdminStatusChangedDelegate(DateTime Timestamp, EVSE EVSE, Timestamped<EVSEAdminStatusType> OldEVSEStatus, Timestamped<EVSEAdminStatusType> NewEVSEStatus);

}
