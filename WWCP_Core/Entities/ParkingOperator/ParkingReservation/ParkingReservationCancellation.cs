﻿/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenParkingCloud/WWCP_Core>
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
    /// A reason for the parking reservation cancellation.
    /// </summary>
    public enum ParkingReservationCancellationReason
    {

        /// <summary>
        /// The parking reservation expired.
        /// </summary>
        Expired,

        /// <summary>
        /// The parking reservation was deleted from the client side/by the ev customer.
        /// </summary>
        Deleted,

        /// <summary>
        /// The parking reservation was aborted on the Parking Station Operator side.
        /// </summary>
        Aborted,

        /// <summary>
        /// The parking reservation was removed as the ev customer finished its parking process.
        /// </summary>
        EndOfProcess

    }

}
