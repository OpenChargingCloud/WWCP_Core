/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// A energy meter admin status diff.
    /// </summary>
    public class EnergyMeterAdminStatusDiff : StatusDiff<ChargingStationOperator_Id,
                                                             EnergyMeter_Id,
                                                             EnergyMeterAdminStatusTypes>
    {

        /// <summary>
        /// Create a new energy meter admin status diff.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the admin status diff.</param>
        /// <param name="EnergyMeterOperatorId">The unique identification of the energy meter operator.</param>
        /// <param name="EnergyMeterOperatorName">The optional multi-language name of the energy meter operator.</param>
        /// <param name="NewStatus">An optional enumeration of all new admin status.</param>
        /// <param name="ChangedStatus">An optional enumeration of all changed admin status.</param>
        /// <param name="RemovedIds">An optional enumeration of all removed admin status.</param>
        public EnergyMeterAdminStatusDiff(DateTime                                                                         Timestamp,
                                              ChargingStationOperator_Id                                                       EnergyMeterOperatorId,
                                              I18NString?                                                                      EnergyMeterOperatorName   = null,
                                              IEnumerable<KeyValuePair<EnergyMeter_Id, EnergyMeterAdminStatusTypes>>?  NewStatus                     = null,
                                              IEnumerable<KeyValuePair<EnergyMeter_Id, EnergyMeterAdminStatusTypes>>?  ChangedStatus                 = null,
                                              IEnumerable<EnergyMeter_Id>?                                                 RemovedIds                    = null)

            : base(Timestamp,
                   EnergyMeterOperatorId,
                   EnergyMeterOperatorName,
                   NewStatus,
                   ChangedStatus,
                   RemovedIds)

        { }

    }

}
