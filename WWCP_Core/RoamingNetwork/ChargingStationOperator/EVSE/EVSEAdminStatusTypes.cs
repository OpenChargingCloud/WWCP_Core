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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The admin status of an EVSE.
    /// </summary>
    public enum EVSEAdminStatusTypes
    {

        /// <summary>
        /// Unclear or unknown admin status of the EVSE.
        /// </summary>
        Unspecified         = 0,

        /// <summary>
        /// The EVSE is ready for charging.
        /// </summary>
        Operational         = 1,

        /// <summary>
        /// The EVSE not accessible because of a physical barrier,
        /// i.e. a car, a construction area or a city festival in front
        /// of the EVSE.
        /// </summary>
        Blocked             = 2,

        /// <summary>
        /// The EVSE is not ready for charging because it is under maintenance.
        /// </summary>
        OutOfService        = 3,

        /// <summary>
        /// Planned for the future.
        /// </summary>
        Planned             = 4,

        /// <summary>
        /// The EVSE is currently in deployment, but not fully operational yet.
        /// </summary>
        InDeployment        = 5,

        /// <summary>
        /// Private or internal use only.
        /// </summary>
        InternalUse         = 6,

        /// <summary>
        /// The EVSE does no longer exist.
        /// </summary>
        Deleted             = 7,

        /// <summary>
        /// The EVSE was not found!
        /// (Only valid within batch-processing)
        /// </summary>
        UnknownEVSE         = 8

    }

}
