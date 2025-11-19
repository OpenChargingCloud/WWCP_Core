/*
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
    /// The status of an EVSE group.
    /// </summary>
    public enum EVSEGroupStatusTypes
    {

        /// <summary>
        /// Unclear or unknown status of the EVSE group.
        /// </summary>
        Unknown             = 0,


        /// <summary>
        /// The EVSE group is planned for the future.
        /// </summary>
        Planned             = 1,

        /// <summary>
        /// The EVSE group is currently in deployment, but not fully operational yet.
        /// </summary>
        InDeployment        = 2,

        /// <summary>
        /// The EVSE group is not ready for charging because it is under maintenance.
        /// </summary>
        OutOfService        = 3,

        /// <summary>
        /// Currently no communication with the EVSE group possible, but charging in offline mode might be available.
        /// </summary>
        Offline             = 4,

        /// <summary>
        /// The entire EVSE group is ready to charge.
        /// </summary>
        Available           = 5,

        /// <summary>
        /// Some ongoing charging sessions or reservations, but still ready to charge.
        /// </summary>
        PartialAvailable    = 6,

        /// <summary>
        /// The entire EVSE group was reserved by an ev customer.
        /// </summary>
        Reserved            = 7,

        /// <summary>
        /// The entire EVSE group is charging. Currently no additional charging sessions are possible.
        /// </summary>
        Charging            = 8,

        /// <summary>
        /// An error has occurred in the EVSE group.
        /// </summary>
        Faulted             = 9,

        /// <summary>
        /// Private or internal use.
        /// </summary>
        Other               = 10,


        /// <summary>
        /// The EVSE group was not found!
        /// (Only valid within batch-processing)
        /// </summary>
        UnknownGroup        = 11

    }

}
