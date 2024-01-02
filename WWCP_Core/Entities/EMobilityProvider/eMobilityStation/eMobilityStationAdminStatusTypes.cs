﻿/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// The admin status of a charging station.
    /// </summary>
    public enum eMobilityStationAdminStatusTypes
    {

        /// <summary>
        /// Unclear or unknown admin status of the charging station.
        /// </summary>
        Unspecified         = 0,

        /// <summary>
        /// The charging station is planned for the future.
        /// </summary>
        Planned             = 1,

        /// <summary>
        /// The charging station is currently in deployment, but not fully operational yet.
        /// </summary>
        InDeployment        = 2,

        /// <summary>
        /// Private or internal use.
        /// </summary>
        InternalUse         = 3,

        /// <summary>
        /// The charging station is not ready for charging because it is under maintenance.
        /// </summary>
        OutOfService        = 4,

        /// <summary>
        /// The charging station is ready to charge.
        /// </summary>
        Operational         = 5,


        /// <summary>
        /// The charging station was not found!
        /// (Only valid within batch-processing)
        /// </summary>
        UnknownStation      = 7

    }

}
