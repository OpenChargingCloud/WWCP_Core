/*
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
    /// The admin status of an electric vehicle.
    /// </summary>
    public enum eVehicleStatusTypes
    {

        /// <summary>
        /// Unclear or unknown status of the electric vehicle.
        /// </summary>
        Unspecified             = 0,

        /// <summary>
        /// The electric vehicle is not ready.
        /// </summary>
        OutOfService            = 1,

        /// <summary>
        /// Currently no communication with the electric vehicle possible.
        /// </summary>
        Offline                 = 2,

        /// <summary>
        /// The electric vehicle is available.
        /// </summary>
        Available               = 3,

        /// <summary>
        /// The electric vehicle is reserved.
        /// </summary>
        Reserved                = 4,

        /// <summary>
        /// An error has occured.
        /// </summary>
        Faulted                 = 5,

        /// <summary>
        /// Private or internal use.
        /// </summary>
        Other                   = 6,

        /// <summary>
        /// The electric vehicle was not found!
        /// (Only valid within batch-processing)
        /// </summary>
        UnknownElectricVehicle  = 12

    }

}
