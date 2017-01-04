/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The type of the current at an EVSE.
    /// </summary>
    public enum CurrentTypes
    {

        /// <summary>
        /// Unknown current type.
        /// </summary>
        Undefined,

        /// <summary>
        /// AC with 1 phase.
        /// </summary>
        AC_OnePhase,

        /// <summary>
        /// AC with 3 phases.
        /// </summary>
        AC_ThreePhases,

        /// <summary>
        /// Direct current.
        /// </summary>
        DC

    }

}
