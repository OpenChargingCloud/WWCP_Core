/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
    /// The current status of a parking spot.
    /// </summary>
    public enum ParkingSpotStatusType
    {

        /// <summary>
        /// Unclear or unknown status of the parking spot.
        /// </summary>
        Unspecified         = 0,

        /// <summary>
        /// The parking spot is planned for the future.
        /// </summary>
        Planned             = 1,

        /// <summary>
        /// The parking spot is currently in deployment.
        /// </summary>
        InDeployment        = 2,

        /// <summary>
        /// The parking spot is currently blocked.
        /// </summary>
        Blocked             = 3,

        /// <summary>
        /// The parking spot is available.
        /// </summary>
        Available           = 4,

        /// <summary>
        /// The parking spot was reserved.
        /// </summary>
        Reserved            = 5,

        /// <summary>
        /// The parking spot is currently in use.
        /// </summary>
        InUse               = 6,

        /// <summary>
        /// Private or internal use.
        /// </summary>
        Other               = 7,

        /// <summary>
        /// The parking spot was not found!
        /// (Only valid within batch-processing)
        /// </summary>
        UnknownParkingSpot  = 8

    }

}
