﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The status of a roaming network.
    /// </summary>
    public enum CSORoamingProviderStatusTypes
    {

        /// <summary>
        /// Unclear or unknown admin status of the roaming network.
        /// </summary>
        Unspecified             = 0,

        /// <summary>
        /// The roaming network is under maintenance.
        /// </summary>
        OutOfService            = 1,

        /// <summary>
        /// The roaming network is operational.
        /// </summary>
        Operational             = 2,

        /// <summary>
        /// Private or internal use.
        /// </summary>
        InternalUse             = 3,


        /// <summary>
        /// The roaming network was not found!
        /// (Only valid within batch-processing)
        /// </summary>
        UnknownRoamingNetwork   = 4

    }

}
