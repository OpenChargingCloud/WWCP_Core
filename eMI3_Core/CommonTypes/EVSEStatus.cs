/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/eMI3/Core>
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

using System;

#endregion

namespace org.emi3group
{

    public enum EVSEStatusType
    {

        /// <summary>
        /// Unknown EVSE status.
        /// </summary>
        Unknown,

        /// <summary>
        /// Charging Spot is available for charging.
        /// </summary>
        Available,

        /// <summary>
        /// Charging Spot is reserved and not available for charging.
        /// </summary>
        Reserved,

        /// <summary>
        /// Charging Spot is busy.
        /// </summary>
        Occupied,

        /// <summary>
        /// Charging Spot is out of service and not available for charging.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The requested EVSEID and EVSE status does not exist within the database.
        /// </summary>
        EvseNotFound

    }

}
