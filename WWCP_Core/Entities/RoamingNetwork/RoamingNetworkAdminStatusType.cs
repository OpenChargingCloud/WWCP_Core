/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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
    /// The admin status of a roaming network.
    /// </summary>
    public enum RoamingNetworkAdminStatusType
    {

        /// <summary>
        /// Unspecified
        /// </summary>
        Unspecified         = 0,

        /// <summary>
        /// The pole is not fully operational yet.
        /// </summary>
        Planned             = 1,

        /// <summary>
        /// The pole is not fully operational yet.
        /// </summary>
        InDeployment        = 2,

        /// <summary>
        /// No car connected to EVSE, ready to charge.
        /// </summary>
        Available           = 3,

                /// <summary>
        /// Some cars connected to station or pool, but still ready to charge.
        /// </summary>
        MostlyAvailable     = 4,

        /// <summary>
        /// Some cars connected to station or pool, but still ready to charge.
        /// </summary>
        PartialAvailable    = 5,

        /// <summary>
        /// A charging session is still open (a car is connected)
        /// </summary>
        Occupied            = 6,

        /// <summary>
        /// A car is connected and an error has occured during charge (this status appeared while charging).
        /// </summary>
        Faulted             = 7,

        /// <summary>
        /// No car is connected but the pole is not ready to charge.
        /// </summary>
        Unavailable         = 8,

        /// <summary>
        /// No car is connected, the pole is not ready to charge because under maintenance.
        /// </summary>
        OutOfService        = 9,

        /// <summary>
        /// The platform has lost connection with the pole (may be used by customer depending on its ability to handle offline mode).
        /// </summary>
        Offline             = 10,

        /// <summary>
        /// No car is connected but no car can connect except the one that has booked this EVSE.
        /// </summary>
        Reserved            = 11,

        /// <summary>
        /// Private or internal use.
        /// </summary>
        Other               = 12,

        /// <summary>
        /// No status is sent by the pole.
        /// </summary>
        Unknown             = 13,

        /// <summary>
        /// The EVSE was not found!
        /// </summary>
        EvseNotFound        = 14

    }

}
