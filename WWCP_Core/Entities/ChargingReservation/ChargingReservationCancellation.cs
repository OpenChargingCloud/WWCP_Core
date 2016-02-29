/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The charging reservation cancellation.
    /// </summary>
    public enum ChargingReservationCancellationReason
    {

        /// <summary>
        /// The charging reservation expired.
        /// </summary>
        Expired,

        /// <summary>
        /// The charging reservation was deleted from the client side/by the ev customer.
        /// </summary>
        Deleted,

        /// <summary>
        /// The charging reservation was aborted on the EVSE operator side.
        /// </summary>
        Aborted,

        /// <summary>
        /// The charging reservation was removed as the ev customer finished its charging process.
        /// </summary>
        EndOfProcess

    }

}
