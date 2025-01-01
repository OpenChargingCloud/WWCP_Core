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
    /// A charging pool status diff.
    /// </summary>
    public class ChargingPoolStatusDiff : StatusDiff<ChargingStationOperator_Id,
                                                     ChargingPool_Id,
                                                     ChargingPoolStatusTypes>
    {

        /// <summary>
        /// Create a new charging pool status diff.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the status diff.</param>
        /// <param name="ChargingStationOperatorId">The unique identification of the charging station operator.</param>
        /// <param name="ChargingStationOperatorName">The optional multi-language name of the charging station operator.</param>
        /// <param name="NewStatus">An optional enumeration of all new charging pool status.</param>
        /// <param name="ChangedStatus">An optional enumeration of all changed charging pool status.</param>
        /// <param name="RemovedIds">An optional enumeration of all removed charging pool status.</param>
        public ChargingPoolStatusDiff(DateTime                                                              Timestamp,
                                      ChargingStationOperator_Id                                            ChargingStationOperatorId,
                                      I18NString?                                                           ChargingStationOperatorName   = null,
                                      IEnumerable<KeyValuePair<ChargingPool_Id, ChargingPoolStatusTypes>>?  NewStatus                     = null,
                                      IEnumerable<KeyValuePair<ChargingPool_Id, ChargingPoolStatusTypes>>?  ChangedStatus                 = null,
                                      IEnumerable<ChargingPool_Id>?                                         RemovedIds                    = null)

            : base(Timestamp,
                   ChargingStationOperatorId,
                   ChargingStationOperatorName,
                   NewStatus,
                   ChangedStatus,
                   RemovedIds)

        { }

    }

}
