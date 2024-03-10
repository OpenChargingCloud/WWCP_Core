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
    /// Extensions methods for energy metering value types.
    /// </summary>
    public static class EnergyMeteringValueTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of an energy metering value type.
        /// </summary>
        /// <param name="Text">A text representation of an energy metering value type.</param>
        public static EnergyMeteringValueTypes Parse(String Text)
        {

            if (TryParse(Text, out var meteringStatusType))
                return meteringStatusType;

            throw new ArgumentException($"Undefined energy metering value type '{Text}'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of an energy metering value type.
        /// </summary>
        /// <param name="Text">A text representation of an energy metering value type.</param>
        public static EnergyMeteringValueTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var meteringStatusType))
                return meteringStatusType;

            return default;

        }

        #endregion

        #region TryParse(Text, out EnergyMeteringType)

        /// <summary>
        /// Parses the given text representation of an energy metering value type.
        /// </summary>
        /// <param name="Text">A text representation of an energy metering value type.</param>
        /// <param name="EnergyMeteringType">The parsed energy metering value type.</param>
        public static Boolean TryParse(String Text, out EnergyMeteringValueTypes EnergyMeteringType)
        {
            switch (Text?.Trim())
            {

                case "Start":
                    EnergyMeteringType = EnergyMeteringValueTypes.Start;
                    return true;

                case "Intermediate":
                    EnergyMeteringType = EnergyMeteringValueTypes.Intermediate;
                    return true;

                case "TariffChange":
                    EnergyMeteringType = EnergyMeteringValueTypes.TariffChange;
                    return true;

                case "Stop":
                    EnergyMeteringType = EnergyMeteringValueTypes.Stop;
                    return true;

                default:
                    EnergyMeteringType = EnergyMeteringValueTypes.Undefined;
                    return false;

            };
        }

        #endregion

        #region AsText(this EnergyMeteringType)

        /// <summary>
        /// Return a text representation of the given energy metering value type.
        /// </summary>
        /// <param name="EnergyMeteringType">An energy metering value type.</param>
        public static String AsText(this EnergyMeteringValueTypes EnergyMeteringType)

            => EnergyMeteringType switch {
                   EnergyMeteringValueTypes.Start         => "Start",
                   EnergyMeteringValueTypes.Intermediate  => "Intermediate",
                   EnergyMeteringValueTypes.TariffChange  => "TariffChange",
                   EnergyMeteringValueTypes.Stop          => "Stop",
                   _                                      => "unknown",
               };

        #endregion

    }


    /// <summary>
    /// Energy metering value types.
    /// </summary>
    public enum EnergyMeteringValueTypes
    {

        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,

        /// <summary>
        /// An energy metering value at the start of a charging session.
        /// </summary>
        Start,

        /// <summary>
        /// An intermediate energy metering value during the charging session.
        /// </summary>
        Intermediate,

        /// <summary>
        /// An intermediate energy metering value of the charging session because of a tariff change.
        /// </summary>
        TariffChange,

        /// <summary>
        /// An energy metering value at the end of the charging session.
        /// </summary>
        Stop

    }

}
