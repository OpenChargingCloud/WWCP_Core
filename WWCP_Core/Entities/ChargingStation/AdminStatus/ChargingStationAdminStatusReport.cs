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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for charging station admin status reports.
    /// </summary>
    public static class ChargingStationAdminStatusReportExtensions
    {

        #region GenerateAdminStatusReport(this ChargingStation,          Timestamp = null)

        /// <summary>
        /// Generate a new charging station admin status report for the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationAdminStatusReport GenerateAdminStatusReport               (this IChargingStation                       ChargingStation,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (new IChargingStation[] { ChargingStation },
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingStations,         Timestamp = null)

        /// <summary>
        /// Generate a new charging station admin status report for the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        public static ChargingStationAdminStatusReport GenerateAdminStatusReport               (this IEnumerable<IChargingStation>          ChargingStations,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (ChargingStations,
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingPool,             Timestamp = null)

        /// <summary>
        /// Generate a new charging station admin status report for the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static ChargingStationAdminStatusReport GenerateChargingStationAdminStatusReport(this IChargingPool                          ChargingPool,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (ChargingPool.ChargingStations,
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingPools,            Timestamp = null)

        /// <summary>
        /// Generate a new charging station admin status report for the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        public static ChargingStationAdminStatusReport GenerateChargingStationAdminStatusReport(this IEnumerable<IChargingPool>             ChargingPools,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (ChargingPools.SelectMany(chargingPool => chargingPool.ChargingStations),
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingStationOperator,  Timestamp = null)

        /// <summary>
        /// Generate a new charging station admin status report for the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        public static ChargingStationAdminStatusReport GenerateChargingStationAdminStatusReport(this IChargingStationOperator               ChargingStationOperator,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (ChargingStationOperator.ChargingStations,
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingStationOperators, Timestamp = null)

        /// <summary>
        /// Generate a new charging station admin status report for the given charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        public static ChargingStationAdminStatusReport GenerateChargingStationAdminStatusReport(this IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (ChargingStationOperators.SelectMany(chargingStationOperator => chargingStationOperator.ChargingStations),
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this RoamingNetwork,           Timestamp = null)

        /// <summary>
        /// Generate a new charging station admin status report for the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static ChargingStationAdminStatusReport GenerateChargingStationAdminStatusReport(this IRoamingNetwork                        RoamingNetwork,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (RoamingNetwork.ChargingStations,
                    Timestamp);

        #endregion

    }


    /// <summary>
    /// A charging station admin status report.
    /// </summary>
    public class ChargingStationAdminStatusReport : StatusReport<IChargingStation, ChargingStationAdminStatusTypes>
    {

        /// <summary>
        /// Create a new charging station admin status report for the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="Timestamp">The optional timestamp of the status report generation.</param>
        public ChargingStationAdminStatusReport(IEnumerable<IChargingStation>  ChargingStations,
                                                DateTime?                      Timestamp   = null)

            : base(ChargingStations,
                   station => station.AdminStatus.Value,
                   Timestamp,
                   "https://open.charging.cloud/contexts/wwcp+json/chargingStationAdminStatusReport")

        { }

    }

}
