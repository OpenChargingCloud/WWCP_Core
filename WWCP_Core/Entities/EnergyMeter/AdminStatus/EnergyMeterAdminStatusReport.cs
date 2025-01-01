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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for energy meter admin status reports.
    /// </summary>
    public static class EnergyMeterAdminStatusReportExtensions
    {

        #region GenerateAdminStatusReport(this EnergyMeter,          Timestamp = null)

        /// <summary>
        /// Generate a new energy meter admin status report for the given energy meter.
        /// </summary>
        /// <param name="EnergyMeter">A energy meter.</param>
        public static EnergyMeterAdminStatusReport GenerateAdminStatusReport               (this IEnergyMeter                       EnergyMeter,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (new IEnergyMeter[] { EnergyMeter },
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this EnergyMeters,         Timestamp = null)

        /// <summary>
        /// Generate a new energy meter admin status report for the given energy meters.
        /// </summary>
        /// <param name="EnergyMeters">An enumeration of energy meters.</param>
        public static EnergyMeterAdminStatusReport GenerateAdminStatusReport               (this IEnumerable<IEnergyMeter>          EnergyMeters,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (EnergyMeters,
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingPool,             Timestamp = null)

        /// <summary>
        /// Generate a new energy meter admin status report for the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static EnergyMeterAdminStatusReport GenerateEnergyMeterAdminStatusReport(this IChargingPool                          ChargingPool,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (ChargingPool.EnergyMeters,
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this ChargingPools,            Timestamp = null)

        /// <summary>
        /// Generate a new energy meter admin status report for the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        public static EnergyMeterAdminStatusReport GenerateEnergyMeterAdminStatusReport(this IEnumerable<IChargingPool>             ChargingPools,
                                                                                                DateTime?                                   Timestamp   = null)

            => new (ChargingPools.SelectMany(chargingPool => chargingPool.EnergyMeters),
                    Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this EnergyMeterOperator,  Timestamp = null)

        ///// <summary>
        ///// Generate a new energy meter admin status report for the given energy meter operator.
        ///// </summary>
        ///// <param name="EnergyMeterOperator">A energy meter operator.</param>
        //public static EnergyMeterAdminStatusReport GenerateEnergyMeterAdminStatusReport(this IChargingStationOperator               EnergyMeterOperator,
        //                                                                                        DateTime?                                   Timestamp   = null)

        //    => new (EnergyMeterOperator.EnergyMeters,
        //            Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this EnergyMeterOperators, Timestamp = null)

        ///// <summary>
        ///// Generate a new energy meter admin status report for the given energy meter operators.
        ///// </summary>
        ///// <param name="EnergyMeterOperators">An enumeration of energy meter operators.</param>
        //public static EnergyMeterAdminStatusReport GenerateEnergyMeterAdminStatusReport(this IEnumerable<IChargingStationOperator>  EnergyMeterOperators,
        //                                                                                        DateTime?                                   Timestamp   = null)

        //    => new (EnergyMeterOperators.SelectMany(energyMeterOperator => energyMeterOperator.EnergyMeters),
        //            Timestamp);

        #endregion

        #region GenerateAdminStatusReport(this RoamingNetwork,           Timestamp = null)

        ///// <summary>
        ///// Generate a new energy meter admin status report for the given roaming network.
        ///// </summary>
        ///// <param name="RoamingNetwork">A roaming network.</param>
        //public static EnergyMeterAdminStatusReport GenerateEnergyMeterAdminStatusReport(this IRoamingNetwork                        RoamingNetwork,
        //                                                                                        DateTime?                                   Timestamp   = null)

        //    => new (RoamingNetwork.EnergyMeters,
        //            Timestamp);

        #endregion

    }


    /// <summary>
    /// A energy meter admin status report.
    /// </summary>
    public class EnergyMeterAdminStatusReport : StatusReport<IEnergyMeter, EnergyMeterAdminStatusTypes>
    {

        /// <summary>
        /// Create a new energy meter admin status report for the given energy meters.
        /// </summary>
        /// <param name="EnergyMeters">An enumeration of energy meters.</param>
        /// <param name="Timestamp">The optional timestamp of the status report generation.</param>
        public EnergyMeterAdminStatusReport(IEnumerable<IEnergyMeter>  EnergyMeters,
                                            DateTime?                  Timestamp   = null)

            : base(EnergyMeters,
                   station => station.AdminStatus.Value,
                   Timestamp,
                   "https://open.charging.cloud/contexts/wwcp+json/energyMeterAdminStatusReport")

        { }

    }

}
