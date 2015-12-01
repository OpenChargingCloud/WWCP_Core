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
    /// The status of an EVSE.
    /// </summary>
    public enum EVSEStatusType
    {

        /// <summary>
        /// Unclear or unknown status of the EVSE
        /// </summary>
        Unspecified         = 0,

        /// <summary>
        /// The EVSE is planned for the future.
        /// </summary>
        Planned             = 1,

        /// <summary>
        /// The EVSE is currently in deployment and not fully operational yet.
        /// </summary>
        InDeployment        = 2,

        /// <summary>
        /// Nothing connected to EVSE, ready to charge.
        /// </summary>
        Available           = 3,

        /// <summary>
        /// An ongoing charging session.
        /// </summary>
        Occupied            = 5,

        /// <summary>
        /// An error has occured while charging.
        /// </summary>
        Faulted             = 6,

        /// <summary>
        /// Nothing connected, but the EVSE is not ready to charge because it is under maintenance.
        /// </summary>
        OutOfService        = 7,

        /// <summary>
        /// The platform has lost connection with the EVSE (may be used by customer depending on its ability to handle offline mode).
        /// </summary>
        Offline             = 8,

        /// <summary>
        /// Nothing is connected, but no one can connect except the customer who has reserved this EVSE.
        /// </summary>
        Reserved            = 9,

        /// <summary>
        /// Private internal use.
        /// </summary>
        Other               = 10,

        /// <summary>
        /// The EVSE was not found!
        /// </summary>
        EvseNotFound        = 11


    }



}
