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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for charging station operator status reports.
    /// </summary>
    public static class ChargingStationOperatorStatusReportExtensions
    {

        #region GenerateStatusReport(this ChargingStationOperator,  Timestamp = null)

        /// <summary>
        /// Generate a new charging station operator status report for the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        public static ChargingStationOperatorStatusReport GenerateStatusReport                       (this IChargingStationOperator               ChargingStationOperator,
                                                                                                      DateTime?                                   Timestamp   = null)

            => new (new IChargingStationOperator[] { ChargingStationOperator },
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this ChargingStationOperators, Timestamp = null)

        /// <summary>
        /// Generate a new charging station operator status report for the given charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        public static ChargingStationOperatorStatusReport GenerateStatusReport                       (this IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                                                                      DateTime?                                   Timestamp   = null)

            => new (ChargingStationOperators,
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this RoamingNetwork,           Timestamp = null)

        /// <summary>
        /// Generate a new charging pool status report for the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static ChargingStationOperatorStatusReport GenerateChargingStationOperatorStatusReport(this IRoamingNetwork                        RoamingNetwork,
                                                                                                      DateTime?                                   Timestamp   = null)

            => new (RoamingNetwork.ChargingStationOperators,
                    Timestamp);

        #endregion

    }


    /// <summary>
    /// A charging station operator status report.
    /// </summary>
    public class ChargingStationOperatorStatusReport : StatusReport<IChargingStationOperator, ChargingStationOperatorStatusTypes>
    {

        /// <summary>
        /// Create a new charging station operator status report for the given charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="Timestamp">The optional timestamp of the status report generation.</param>
        public ChargingStationOperatorStatusReport(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                   DateTime?                              Timestamp   = null)

            : base(ChargingStationOperators,
                   chargingStationOperator => chargingStationOperator.Status.Value,
                   Timestamp,
                   "https://open.charging.cloud/contexts/wwcp+json/chargingStationOperatorStatusReport")

        { }

    }

}
