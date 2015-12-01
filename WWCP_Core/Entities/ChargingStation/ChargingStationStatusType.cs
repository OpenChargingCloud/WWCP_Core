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
        /// Unclear or unknown status of the charging station
        /// </summary>
        Unspecified         = 0,

        /// <summary>
        /// The charging station is planned for the future.
        /// </summary>
        Planned             = 1,

        /// <summary>
        /// The charging station is currently in deployment and not fully operational yet.
        /// </summary>
        InDeployment        = 2,

        /// <summary>
        /// The entire charging station is ready to charge.
        /// </summary>
        Available           = 3,

        /// <summary>
        /// Some ongoing charging sessions or reservations, but still ready to charge.
        /// </summary>
        PartialAvailable    = 4,

        /// <summary>
        /// The entire charging station is occupied. Currently no additional charging sessions are possible.
        /// </summary>
        Occupied            = 5,

        /// <summary>
        /// An error has occured in the charging station.
        /// </summary>
        Faulted             = 6,

        /// <summary>
        /// The charging station is not ready to charge because it is under maintenance.
        /// </summary>
        OutOfService        = 7,

        /// <summary>
        /// The management platform has lost connection with the charging station (may be used by customer depending on its ability to handle offline mode).
        /// </summary>
        Offline             = 8,

        /// <summary>
        /// The entire charging station is reserved.
        /// </summary>
        Reserved            = 9,

        /// <summary>
        /// The charging station (identification) was not found!
        /// </summary>
        StationNotFound     = 10

    }

}
