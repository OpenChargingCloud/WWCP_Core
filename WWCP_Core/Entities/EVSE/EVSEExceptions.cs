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
using System.Collections.Generic;
using System.Collections.Concurrent;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    #region EVSEAlreadyExistsInStation

    /// <summary>
    /// An exception thrown whenever an EVSE already exists within the given charging station.
    /// </summary>
    public class EVSEAlreadyExistsInStation : ChargingStationException
    {

        public EVSEAlreadyExistsInStation(IChargingStation ChargingStation, EVSE_Id EVSE_Id)

            : base(ChargingStation.ChargingPool,
                  "The given EVSE identification '" + EVSE_Id + "' already exists within the given '" + ChargingStation.Id + "' charging station!")

        { }

    }

    #endregion



    /// <summary>
    /// An EVSE exception.
    /// </summary>
    public class EVSEException : ChargingStationException
    {

        public EVSEException(IChargingStation ChargingStation, String Message)
            : base(ChargingStation.ChargingPool, Message)
        { }

        public EVSEException(IChargingStation ChargingStation, String Message, Exception InnerException)
            : base(ChargingStation.ChargingPool, Message, InnerException)
        { }

    }

}
