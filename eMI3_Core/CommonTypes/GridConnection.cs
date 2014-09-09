/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3>
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

using System;

#endregion

namespace com.graphdefined.eMI3
{

    public enum GridConnection
    {

        /// <summary>
        /// Unknown grid connection.
        /// </summary>
        Unknown,

        /// <summary>
        /// 3-phase grid connection.
        /// </summary>
        ThreePhase,

        /// <summary>
        /// Single phase grid connection.
        /// </summary>
        SinglePhase,

        /// <summary>
        /// Single phase grid connection on L0.
        /// </summary>
        SinglePhaseL0,

        /// <summary>
        /// Single phase grid connection on L1.
        /// </summary>
        SinglePhaseL1,

        /// <summary>
        /// Single phase grid connection on L2.
        /// </summary>
        SinglePhaseL2,

    }

}
