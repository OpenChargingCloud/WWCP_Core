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

namespace org.emi3group
{

    public enum EVSERealTimeStatus
    {

        /// <summary>
        /// Unspecified
        /// </summary>
        Unspecified     = 0,

        /// <summary>
        /// The pole is not fully operational yet.
        /// </summary>
        Planned         = 1,

        /// <summary>
        /// The pole is not fully operational yet.
        /// </summary>
        InDeployment    = 1,

        /// <summary>
        /// No car connected to EVSE, ready to charge.
        /// </summary>
        Available       = 2,

        /// <summary>
        /// A charging session is still open (a car is connected)
        /// </summary>
        Charging        = 3,

        /// <summary>
        /// A car is connected and an error has occured during charge (this status appeared while charging).
        /// </summary>
        Faulted         = 4,

        /// <summary>
        /// No car is connected but the pole is not ready to charge.
        /// </summary>
        Unavailable     = 5,

        /// <summary>
        /// No car is connected, the pole is not ready to charge because under maintenance.
        /// </summary>
        Maintained      = 6,

        /// <summary>
        /// The platform has lost connection with the pole (may be used by customer depending on its ability to handle offline mode).
        /// </summary>
        Offline         = 7,

        /// <summary>
        /// No car is connected but no car can connect except the one that has booked this EVSE.
        /// </summary>
        Reserved        = 8,

        /// <summary>
        /// Private internal use.
        /// </summary>
        Other           = 9,

        /// <summary>
        /// No status is sent by the pole.
        /// </summary>
        Unknown         = 10,

    }



}
