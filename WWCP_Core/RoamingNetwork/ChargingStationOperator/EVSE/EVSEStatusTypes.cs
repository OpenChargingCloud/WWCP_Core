/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The current status of an EVSE.
    /// </summary>
    public enum EVSEStatusTypes
    {

        /// <summary>
        /// Unclear or unknown status of the EVSE.
        /// </summary>
        Unspecified             = 0,

        /// <summary>
        /// Currently no communication with the EVSE possible,
        /// but charging in offline mode might be available.
        /// </summary>
        Offline                 = 1,

        /// <summary>
        /// The EVSE is available for charging.
        /// </summary>
        Available               = 2,

        /// <summary>
        /// The EVSE is reserved for future charging.
        /// </summary>
        Reserved                = 3,

        /// <summary>
        /// The door of a charging locker is open, the EVSE is unlocked
        /// and is waiting for the customer to plugin.
        /// </summary>
        WaitingForPlugin        = 4,

        /// <summary>
        /// A cable is plugged into the socket or a vehicle is connected
        /// to the cable, but both without any further action.
        /// </summary>
        PluggedIn               = 5,

        /// <summary>
        /// An ongoing charging process.
        /// </summary>
        Charging                = 6,

        /// <summary>
        /// The EVSE has a mechanical door, e.g. an e-bike charging locker,
        /// which was not closed after the customer took the battery out.
        /// </summary>
        DoorNotClosed           = 7,

        /// <summary>
        /// An error has occured.
        /// </summary>
        Faulted                 = 8,

        /// <summary>
        /// The EVSE is not ready for charging because it is under maintenance
        /// or was disabled by the charging station operator.
        /// </summary>
        OutOfService            = 9,

        /// <summary>
        /// The EVSE was not found!
        /// (Only valid within batch-processing)
        /// </summary>
        UnknownEVSE             = 10

    }

}
