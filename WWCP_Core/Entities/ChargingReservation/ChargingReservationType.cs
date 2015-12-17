/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
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
    /// The charging reservation type.
    /// </summary>
    public enum ChargingReservationType
    {

        /// <summary>
        /// The EVSE was reserved.
        /// </summary>
        AtEVSE,

        /// <summary>
        /// The charging station was reserved.
        /// </summary>
        AtChargingStation,

        /// <summary>
        /// The charging pool was reserved.
        /// </summary>
        AtChargingPool

    }

}
