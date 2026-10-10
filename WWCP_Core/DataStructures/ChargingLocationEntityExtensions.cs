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

#region Usings

using cloud.charging.open.protocols.WWCP.POI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Charging locations of WWCP Core entities. The charging location lives in
    /// WWCP_POI2, which knows no entities, so these factories live here.
    /// </summary>
    public static class ChargingLocationEntityExtensions
    {

        extension(ChargingLocation)
        {

            #region FromEVSE                   (EVSE)

            /// <summary>
            /// The charging location of the given EVSE, with its station, pool and operator.
            /// </summary>
            /// <param name="EVSE">An EVSE.</param>
            public static ChargingLocation? FromEVSE(IEVSE? EVSE)

                => EVSE is not null
                       ? new ChargingLocation(
                             EVSEId:                      EVSE.                 Id,
                             ChargingStationId:           EVSE.ChargingStation?.Id,
                             ChargingPoolId:              EVSE.ChargingPool?.   Id,
                             ChargingStationOperatorId:   EVSE.Operator?.       Id
                         )
                       : null;

            #endregion

            #region FromChargingStation        (ChargingStation)

            /// <summary>
            /// The charging location of the given charging station, with its pool and operator.
            /// </summary>
            /// <param name="ChargingStation">A charging station.</param>
            public static ChargingLocation? FromChargingStation(IChargingStation? ChargingStation)

                => ChargingStation is not null
                       ? new ChargingLocation(
                             ChargingStationId:           ChargingStation.              Id,
                             ChargingPoolId:              ChargingStation.ChargingPool?.Id,
                             ChargingStationOperatorId:   ChargingStation.Operator?.    Id
                         )
                       : null;

            #endregion

        }

    }

}
