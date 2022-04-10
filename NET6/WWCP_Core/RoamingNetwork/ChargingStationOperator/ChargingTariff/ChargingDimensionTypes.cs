﻿/*
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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The authentication method used.
    /// </summary>
    public enum ChargingDimensionTypes
    {

        /// <summary>
        /// defined in kWh, default division is 1 Wh
        /// </summary>
        ENERGY,

        /// <summary>
        /// flat fee, no unit
        /// </summary>
        FLAT,

        /// <summary>
        /// defined in A (Ampere), Maximum current
        /// </summary>
        MAX_CURRENT,

        /// <summary>
        /// defined in A (Ampere), Minimum current
        /// </summary>
        MIN_CURRENT,

        /// <summary>
        /// time not charging: defined in hours, default division is 1 second
        /// </summary>
        PARKING_TIME,

        /// <summary>
        /// time charging: defined in hours, default division is 1 second
        /// </summary>
        TIME

    }

}
