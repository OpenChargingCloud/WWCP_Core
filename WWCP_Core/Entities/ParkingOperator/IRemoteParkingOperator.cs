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

#region Usings

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A remote Charging Station Operator.
    /// </summary>
    public interface IRemoteParkingOperator : IRemoteStartStop
    {

        #region Events towards the remote charging station operator

        ///// <summary>
        ///// An event fired whenever an EVSE is being reserved.
        ///// </summary>
        //event OnReserveDelegate         OnReserveEVSE;

        ///// <summary>
        ///// An event sent whenever an EVSE should start charging.
        ///// </summary>
        //event OnRemoteStartEVSEDelegate     OnRemoteStartEVSE;

        ///// <summary>
        ///// An event sent whenever a charging session should stop.
        ///// </summary>
        //event OnRemoteStopDelegate          OnRemoteStop;

        #endregion

    }

}