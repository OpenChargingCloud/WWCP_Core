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

#region Usings

using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for grid connection types.
    /// </summary>
    public static class GridConnectionTypesExtensions
    {

        

    }

    public enum GridConnectionTypes
    {

        /// <summary>
        /// Unknown grid connection.
        /// </summary>
        Unknown,

        /// <summary>
        /// Single phase grid connection.
        /// </summary>
        AC_SinglePhase,

        /// <summary>
        /// Single phase grid connection on L0.
        /// </summary>
        AC_SinglePhase_L0,

        /// <summary>
        /// Single phase grid connection on L1.
        /// </summary>
        AC_SinglePhase_L1,

        /// <summary>
        /// Single phase grid connection on L2.
        /// </summary>
        AC_SinglePhase_L2,

        /// <summary>
        /// 3-phases grid connection.
        /// </summary>
        AC_ThreePhases

    }

}
