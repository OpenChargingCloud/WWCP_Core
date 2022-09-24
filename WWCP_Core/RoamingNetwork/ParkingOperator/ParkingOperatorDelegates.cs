/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public delegate IRemoteParkingOperator RemoteParkingOperatorCreatorDelegate(ParkingOperator ParkingOperator);

    public delegate String ParkingOperatorNameSelectorDelegate(I18NString I18NText);


    /// <summary>
    /// A delegate called whenever the static data of the Charging Station Operator changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ParkingOperator">The updated evse operator.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnParkingOperatorDataChangedDelegate(DateTime         Timestamp,
                                                              ParkingOperator  ParkingOperator,
                                                              String           PropertyName,
                                                              Object           OldValue,
                                                              Object           NewValue);

    /// <summary>
    /// A delegate called whenever the admin status of the Charging Station Operator changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ParkingOperator">The updated Charging Station Operator.</param>
    /// <param name="OldStatus">The old timestamped status of the Charging Station Operator.</param>
    /// <param name="NewStatus">The new timestamped status of the Charging Station Operator.</param>
    public delegate Task OnParkingOperatorAdminStatusChangedDelegate(DateTime                                     Timestamp,
                                                                     ParkingOperator                              ParkingOperator,
                                                                     Timestamped<ParkingOperatorAdminStatusTypes>  OldStatus,
                                                                     Timestamped<ParkingOperatorAdminStatusTypes>  NewStatus);


    /// <summary>
    /// A delegate called whenever the dynamic status of the Charging Station Operator changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="ParkingOperator">The updated Charging Station Operator.</param>
    /// <param name="OldStatus">The old timestamped status of the Charging Station Operator.</param>
    /// <param name="NewStatus">The new timestamped status of the Charging Station Operator.</param>
    public delegate Task OnParkingOperatorStatusChangedDelegate(DateTime                                Timestamp,
                                                                ParkingOperator                         ParkingOperator,
                                                                Timestamped<ParkingOperatorStatusTypes>  OldStatus,
                                                                Timestamped<ParkingOperatorStatusTypes>  NewStatus);

}
