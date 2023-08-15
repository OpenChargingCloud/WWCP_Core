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
    /// Extension methods for energy meter status reports.
    /// </summary>
    public static class EnergyMeterStatusReportExtensions
    {

        #region GenerateStatusReport(this EnergyMeter,          Timestamp = null)

        /// <summary>
        /// Generate a new energy meter status report for the given energy meter.
        /// </summary>
        /// <param name="EnergyMeter">A energy meter.</param>
        public static EnergyMeterStatusReport GenerateStatusReport               (this IEnergyMeter                       EnergyMeter,
                                                                                      DateTime?                                   Timestamp   = null)

            => new (new[] { EnergyMeter },
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this EnergyMeters,         Timestamp = null)

        /// <summary>
        /// Generate a new energy meter status report for the given energy meters.
        /// </summary>
        /// <param name="EnergyMeters">An enumeration of energy meters.</param>
        public static EnergyMeterStatusReport GenerateStatusReport               (this IEnumerable<IEnergyMeter>          EnergyMeters,
                                                                                      DateTime?                                   Timestamp   = null)

            => new (EnergyMeters,
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this ChargingPool,             Timestamp = null)

        /// <summary>
        /// Generate a new energy meter status report for the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static EnergyMeterStatusReport GenerateEnergyMeterStatusReport(this IChargingPool                          ChargingPool,
                                                                              DateTime?                                   Timestamp   = null)

            => new (ChargingPool.EnergyMeters,
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this ChargingPools,            Timestamp = null)

        /// <summary>
        /// Generate a new energy meter status report for the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        public static EnergyMeterStatusReport GenerateEnergyMeterStatusReport(this IEnumerable<IChargingPool>             ChargingPools,
                                                                              DateTime?                                   Timestamp   = null)

            => new (ChargingPools.SelectMany(chargingPool => chargingPool.EnergyMeters),
                    Timestamp);

        #endregion

        #region GenerateStatusReport(this EnergyMeterOperator,  Timestamp = null)

        ///// <summary>
        ///// Generate a new energy meter status report for the given energy meter operator.
        ///// </summary>
        ///// <param name="EnergyMeterOperator">A energy meter operator.</param>
        //public static EnergyMeterStatusReport GenerateEnergyMeterStatusReport(this IChargingStationOperator               EnergyMeterOperator,
        //                                                                      DateTime?                                   Timestamp   = null)

        //    => new (EnergyMeterOperator.EnergyMeters,
        //            Timestamp);

        #endregion

        #region GenerateStatusReport(this EnergyMeterOperators, Timestamp = null)

        ///// <summary>
        ///// Generate a new energy meter status report for the given energy meter operators.
        ///// </summary>
        ///// <param name="EnergyMeterOperators">An enumeration of energy meter operators.</param>
        //public static EnergyMeterStatusReport GenerateEnergyMeterStatusReport(this IEnumerable<IChargingStationOperator>  EnergyMeterOperators,
        //                                                                              DateTime?                                   Timestamp   = null)

        //    => new (EnergyMeterOperators.SelectMany(energyMeterOperator => energyMeterOperator.EnergyMeters),
        //            Timestamp);

        #endregion

        #region GenerateStatusReport(this RoamingNetwork,           Timestamp = null)

        ///// <summary>
        ///// Generate a new energy meter status report for the given roaming network.
        ///// </summary>
        ///// <param name="RoamingNetwork">A roaming network.</param>
        //public static EnergyMeterStatusReport GenerateEnergyMeterStatusReport(this IRoamingNetwork                        RoamingNetwork,
        //                                                                              DateTime?                                   Timestamp   = null)

        //    => new (RoamingNetwork.EnergyMeters,
        //            Timestamp);

        #endregion

    }


    /// <summary>
    /// A energy meter status report.
    /// </summary>
    public class EnergyMeterStatusReport : StatusReport<IEnergyMeter, EnergyMeterStatusTypes>
    {

        /// <summary>
        /// Create a new energy meter status report for the given energy meters.
        /// </summary>
        /// <param name="EnergyMeters">An enumeration of energy meters.</param>
        /// <param name="Timestamp">The optional timestamp of the status report generation.</param>
        public EnergyMeterStatusReport(IEnumerable<IEnergyMeter>  EnergyMeters,
                                       DateTime?                  Timestamp   = null)

            : base(EnergyMeters,
                   energyMeter => energyMeter.Status.Value,
                   Timestamp,
                   "https://open.charging.cloud/contexts/wwcp+json/energyMeterStatusReport")

        { }

    }

}
