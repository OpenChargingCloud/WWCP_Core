﻿/*
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for the charging station admin status schedule.
    /// </summary>
    public static class ChargingStationAdminStatusScheduleExtensions
    {

        #region ToJSON(this ChargingStationAdminStatusSchedules, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<ChargingStationAdminStatusSchedule>  ChargingStationAdminStatusSchedules,
                                     UInt64?                                               Skip          = null,
                                     UInt64?                                               Take          = null,
                                     UInt64                                                HistorySize   = 1)
        {

            #region Initial checks

            if (ChargingStationAdminStatusSchedules is null || !ChargingStationAdminStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<ChargingStation_Id, ChargingStationAdminStatusSchedule>();

            foreach (var status in ChargingStationAdminStatusSchedules)
            {

                if (!filteredStatus.ContainsKey(status.Id))
                    filteredStatus.Add(status.Id, status);

                else if (filteredStatus[status.Id].StatusSchedule.Any() &&
                         filteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         filteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take.HasValue ? filteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)
                                              : filteredStatus.OrderBy(status => status.Key).Skip(Skip)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JObject(
                                                                   kvp.Value.StatusSchedule.

                                                                             // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                             GroupBy          (tsv   => tsv.  Timestamp.ToISO8601()).
                                                                             Select           (group => group.First()).

                                                                             OrderByDescending(tsv   => tsv.Timestamp).
                                                                             Take             (HistorySize).
                                                                             Select           (tsv   => new JProperty(tsv.Timestamp.ToISO8601(),
                                                                                                                      tsv.Value.    ToString())))

                                                              )));

        }

        #endregion

    }


    /// <summary>
    /// The admin status schedule of a charging station.
    /// </summary>
    public class ChargingStationAdminStatusSchedule : AInternalData
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id                               Id                { get; }

        /// <summary>
        /// The timestamped admin status of the charging station.
        /// </summary>
        public StatusSchedule<ChargingStationAdminStatusTypes>  StatusSchedule    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="StatusSchedule">The timestamped admin status of the charging station.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationAdminStatusSchedule(ChargingStation_Id                               Id,
                                                  StatusSchedule<ChargingStationAdminStatusTypes>  StatusSchedule,
                                                  JObject?                                         CustomData     = null,
                                                  UserDefinedDictionary?                           InternalData   = null)

            : base(CustomData,
                   InternalData,
                   Timestamp.Now)

        {

            this.Id              = Id;
            this.StatusSchedule  = StatusSchedule;

        }

        #endregion

    }

}
