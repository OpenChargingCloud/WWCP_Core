/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System;

#endregion

namespace org.GraphDefined.WWCP
{

    public delegate void CPConnectedDelegate(IRemoteChargingPool ChargingPool);

    public delegate void CPEVSEOperatorTimeoutReachedDelegate(IRemoteChargingPool ChargingPool, String EVSEOperatorDNS);

    public delegate void CPDisconnectedDelegate(IRemoteChargingPool ChargingPool);

    public delegate void CPStateChangedDelegate(IRemoteChargingPool ChargingPool,
                                                ChargingPoolStatusTypes OldState,
                                                ChargingPoolStatusTypes NewState);





    /// <summary>
    /// A delegate called whenever the static data of any subordinated charging station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingPool">The updated remote charging station.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate void OnRemoteChargingPoolDataChangedDelegate(DateTime                Timestamp,
                                                                    IRemoteChargingPool  ChargingPool,
                                                                    String                  PropertyName,
                                                                    Object                  OldValue,
                                                                    Object                  NewValue);

    /// <summary>
    /// A delegate called whenever the admin status of the charging station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingPool">The charging station.</param>
    /// <param name="OldEVSEStatus">The old timestamped status of the charging station.</param>
    /// <param name="NewEVSEStatus">The new timestamped status of the charging station.</param>
    public delegate void OnRemoteChargingPoolAdminStatusChangedDelegate(DateTime                                      Timestamp,
                                                                           EventTracking_Id                              EventTrackingId,
                                                                           IRemoteChargingPool                        RemoteChargingPool,
                                                                           Timestamped<ChargingPoolAdminStatusTypes>  OldStatus,
                                                                           Timestamped<ChargingPoolAdminStatusTypes>  NewStatus);

    /// <summary>
    /// A delegate called whenever the dynamic status of the charging station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ChargingPool">The charging station.</param>
    /// <param name="OldEVSEStatus">The old timestamped status of the charging station.</param>
    /// <param name="NewEVSEStatus">The new timestamped status of the charging station.</param>
    public delegate void OnRemoteChargingPoolStatusChangedDelegate(DateTime                                 Timestamp,
                                                                      EventTracking_Id                         EventTrackingId,
                                                                      IRemoteChargingPool                   RemoteChargingPool,
                                                                      Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                                                      Timestamped<ChargingPoolStatusTypes>  NewStatus);

}