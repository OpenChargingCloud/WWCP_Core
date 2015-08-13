/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The status of a charging station.
    /// </summary>
    public enum ChargingStationStatusType
    {

        /// <summary>
        /// Unknown/Unspecified
        /// </summary>
        Unknown             = 0,

        /// <summary>
        /// This charging station is just planned for the future.
        /// </summary>
        Planned             = 1,

        /// <summary>
        /// The charging station is currently in deployment.
        /// </summary>
        InDeployment        = 2,

        /// <summary>
        /// The charging station is ready to charge.
        /// </summary>
        Available           = 3,

        /// <summary>
        /// Some cars are connected to the charging station, but still ready to charge.
        /// </summary>
        PartialAvailable    = 4,

        /// <summary>
        /// The charging station is occupied, no additional cars can be charged.
        /// </summary>
        Occupied            = 5,

        /// <summary>
        /// At least one car is connected and an error has occured during the charging process.
        /// </summary>
        Faulted             = 6,

        /// <summary>
        /// No cars are connected, but the station is out of service.
        /// </summary>
        OutOfService        = 7,

        /// <summary>
        /// The management platform has lost connection with the charging station (may be used by customer depending on its ability to handle offline mode).
        /// </summary>
        Offline             = 8,

        /// <summary>
        /// No cars are connected but no car can connect except the ones that are booked for this charging station.
        /// </summary>
        Reserved            = 9,

        /// <summary>
        /// The charging station (identification) was not found!
        /// </summary>
        StationNotFound     = 10

    }

}
