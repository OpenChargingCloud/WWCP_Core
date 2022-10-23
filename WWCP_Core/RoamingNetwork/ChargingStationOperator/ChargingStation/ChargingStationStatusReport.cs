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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for charging station status reports.
    /// </summary>
    public static class ChargingStationStatusReportExtensions
    {

        #region GenerateStatusReport(this ChargingStation,          Timestamp = null)

        /// <summary>
        /// Generate a new charging station status report for the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationStatusReport GenerateStatusReport               (this ChargingStation                       ChargingStation,
                                                                                      DateTime?                                  Timestamp   = null)

            => new (new ChargingStation[] { ChargingStation },
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this ChargingStations,         Timestamp = null)

        /// <summary>
        /// Generate a new charging station status report for the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        public static ChargingStationStatusReport GenerateStatusReport               (this IEnumerable<ChargingStation>          ChargingStations,
                                                                                      DateTime?                                  Timestamp   = null)

            => new (ChargingStations,
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this ChargingPool,             Timestamp = null)

        /// <summary>
        /// Generate a new charging station status report for the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static ChargingStationStatusReport GenerateChargingStationStatusReport(this ChargingPool                          ChargingPool,
                                                                                      DateTime?                                  Timestamp   = null)

            => new (ChargingPool.ChargingStations,
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this ChargingPools,            Timestamp = null)

        /// <summary>
        /// Generate a new charging station status report for the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        public static ChargingStationStatusReport GenerateChargingStationStatusReport(this IEnumerable<ChargingPool>             ChargingPools,
                                                                                      DateTime?                                  Timestamp   = null)

            => new (ChargingPools.SelectMany(chargingPool => chargingPool.ChargingStations),
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this ChargingStationOperator,  Timestamp = null)

        /// <summary>
        /// Generate a new charging station status report for the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        public static ChargingStationStatusReport GenerateChargingStationStatusReport(this ChargingStationOperator               ChargingStationOperator,
                                                                                      DateTime?                                  Timestamp   = null)

            => new (ChargingStationOperator.ChargingStations,
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this ChargingStationOperators, Timestamp = null)

        /// <summary>
        /// Generate a new charging station status report for the given charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        public static ChargingStationStatusReport GenerateChargingStationStatusReport(this IEnumerable<ChargingStationOperator>  ChargingStationOperators,
                                                                                      DateTime?                                  Timestamp   = null)

            => new (ChargingStationOperators.SelectMany(chargingStationOperator => chargingStationOperator.ChargingStations),
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this RoamingNetwork,           Timestamp = null)

        /// <summary>
        /// Generate a new charging station status report for the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static ChargingStationStatusReport GenerateChargingStationStatusReport(this RoamingNetwork                        RoamingNetwork,
                                                                                      DateTime?                                  Timestamp   = null)

            => new (RoamingNetwork.ChargingStations,
                    Timestamp);

        #endregion

    }


    /// <summary>
    /// A charging station status report.
    /// </summary>
    public class ChargingStationStatusReport : StatusReport<ChargingStation, ChargingStationStatusTypes>
    {

        /// <summary>
        /// Create a new charging station status report for the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="Timestamp">The optional timestamp of the status report generation.</param>
        public ChargingStationStatusReport(IEnumerable<ChargingStation>  ChargingStations,
                                           DateTime?                     Timestamp   = null)

            : base(ChargingStations,
                   station => station.Status.Value,
                   Timestamp,
                   "https://open.charging.cloud/contexts/wwcp+json/chargingStationStatusReport")

        { }

    }

}
