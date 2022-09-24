/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The current status of a parking space.
    /// </summary>
    public enum ParkingSpaceStatusType
    {

        /// <summary>
        /// Unclear or unknown status of the parking space.
        /// </summary>
        Unspecified         = 0,

        /// <summary>
        /// The parking space is planned for the future.
        /// </summary>
        Planned             = 1,

        /// <summary>
        /// The parking space is currently in deployment.
        /// </summary>
        InDeployment        = 2,

        /// <summary>
        /// The parking space is currently blocked.
        /// </summary>
        Blocked             = 3,

        /// <summary>
        /// The parking space is available.
        /// </summary>
        Available           = 4,

        /// <summary>
        /// The parking space was reserved.
        /// </summary>
        Reserved            = 5,

        /// <summary>
        /// The parking space is currently in use.
        /// </summary>
        InUse               = 6,

        /// <summary>
        /// Private or internal use.
        /// </summary>
        Other               = 7,

        /// <summary>
        /// The parking space was not found!
        /// (Only valid within batch-processing)
        /// </summary>
        UnknownParkingSpace  = 8

    }

}
