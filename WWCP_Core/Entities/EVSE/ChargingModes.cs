/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for charging modes.
    /// </summary>
    public static class ChargingModesExtensions
    {

        public static ChargingModes Reduce(this IEnumerable<ChargingModes> EnumerationOfChargingModes)
        {

            var chargingModes = ChargingModes.Unspecified;

            foreach (var chargingMode in EnumerationOfChargingModes)
                chargingModes |= chargingMode;

            return chargingModes;

        }

        public static IEnumerable<ChargingModes> ToEnumeration(this ChargingModes ChargingModesEnum)

            => Enum.GetValues<ChargingModes>().
                    Where(flag => ChargingModesEnum.HasFlag(flag) && flag != ChargingModes.Unspecified);


        public static IEnumerable<String> ToText(this ChargingModes ChargingModesEnum)

            => ChargingModesEnum.ToEnumeration().Select(item => item.ToString());


    }

    /// <summary>
    /// Charging modes.
    /// </summary>
    [Flags]
    public enum ChargingModes
    {

        /// <summary>
        /// Unknown charging mode.
        /// </summary>
        Unspecified     =  0,

        /// <summary>
        /// IEC 61851-1 Mode 1: AC charging without communication, just protective earth and RCD protection.
        /// </summary>
        Mode_1          =  1,

        /// <summary>
        /// IEC 61851-1 Mode 2: AC charging with in-cable control and protection device (IC-CPD), simple communication in cable.
        /// </summary>
        Mode_2          =  2,

        /// <summary>
        /// IEC 61851-1 Mode 3: AC charging with communication between vehicle and charging point via CP/PP (PWM signaling).
        /// </summary>
        Mode_3          =  4,

        /// <summary>
        /// IEC 61851-1 Mode 4: DC charging with communication, power electronics are external (DC EVSE), not inside vehicle.
        /// </summary>
        Mode_4          =  8,

        /// <summary>
        /// CHAdeMO
        /// </summary>
        CHAdeMO         = 16,

    }

}
