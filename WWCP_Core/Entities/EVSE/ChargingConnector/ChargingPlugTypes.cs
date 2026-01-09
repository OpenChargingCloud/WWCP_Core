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
    /// Extension methods for charging plugs.
    /// </summary>
    public static class ChargingPlugTypesExtensions
    {

        /// <summary>
        /// Whether the charging connector is DC or AC.
        /// </summary>
        /// <param name="PlugType">A charging plug.</param>
        public static Boolean IsDC(this ChargingPlugTypes PlugType)

            => PlugType switch {

                    ChargingPlugTypes.CCSCombo1Plug_CableAttached or
                    ChargingPlugTypes.CCSCombo2Plug_CableAttached or
                    ChargingPlugTypes.CHAdeMO => true,

                    _ => false,

               };

    }


    /// <summary>
    /// The type of charging plugs.
    /// </summary>
    public enum ChargingPlugTypes
    {

        Unspecified,
        SmallPaddleInductive,
        LargePaddleInductive,
        AVCONConnector,
        TeslaConnector,

        /// <summary>
        /// The connector type is Tesla Connector “Roadster”-type (round, 4 pin).
        /// </summary>
        TESLA_Roadster,

        /// <summary>
        /// The connector type is Tesla Connector “Model-S”-type (oval, 5 pin).
        /// </summary>
        TESLA_ModelS,

        NEMA5_20,
        TypeEFrenchStandard,
        TypeFSchuko,
        TypeGBritishStandard,
        TypeJSwissStandard,
        Type1Connector_CableAttached,
        Type2Outlet,
        Type2Connector_CableAttached,
        Type3Outlet,
        IEC60309SinglePhase,
        IEC60309ThreePhase,
        CCSCombo1Plug_CableAttached,
        CCSCombo2Plug_CableAttached,
        CHAdeMO,

        CEE3,
        CEE5


    }

}
