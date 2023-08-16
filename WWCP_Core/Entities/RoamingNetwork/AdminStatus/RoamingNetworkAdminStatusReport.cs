/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for roaming network admin status reports.
    /// </summary>
    public static class RoamingNetworkAdminStatusReportExtensions
    {

        #region GenerateAdminStatusReport(RoamingNetwork,  Timestamp = null)

        /// <summary>
        /// Generate a new roaming network admin status report for the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static RoamingNetworkAdminStatusReport GenerateAdminStatusReport(this IRoamingNetwork               RoamingNetwork,
                                                                                DateTime?                          Timestamp   = null)

            => new (new IRoamingNetwork[] { RoamingNetwork },
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(RoamingNetworks, Timestamp = null)

        /// <summary>
        /// Generate a new roaming network admin status report for the given roaming network.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        public static RoamingNetworkAdminStatusReport GenerateAdminStatusReport(this IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                                                                DateTime?                          Timestamp   = null)

            => new (RoamingNetworks,
                    Timestamp);

        #endregion

    }


    /// <summary>
    /// A roaming network admin status report.
    /// </summary>
    public class RoamingNetworkAdminStatusReport : StatusReport<IRoamingNetwork, RoamingNetworkAdminStatusTypes>
    {

        /// <summary>
        /// Create a new roaming network admin status report for the given roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="Timestamp">The optional timestamp of the status report generation.</param>
        public RoamingNetworkAdminStatusReport(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                               DateTime?                     Timestamp   = null)

            : base(RoamingNetworks,
                   chargingStationOperator => chargingStationOperator.AdminStatus.Value,
                   Timestamp,
                   "https://open.charging.cloud/contexts/wwcp+json/roamingNetworkAdminStatusReport")

        { }

    }

}
