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

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A EV Charging Pool exception.
    /// </summary>
    public class ChargingPoolPoolException : WWCPException
    {

        public ChargingPoolPoolException(String Message)
            : base(Message)
        { }

        public ChargingPoolPoolException(String Message, Exception InnerException)
            : base(Message, InnerException)
        { }

    }


    #region ChargingStationAlreadyExistsInPool

    /// <summary>
    /// An exception thrown whenever a charging station already exists within the given charging pool.
    /// </summary>
    public class ChargingStationAlreadyExistsInPool : ChargingPoolPoolException
    {

        public ChargingStationAlreadyExistsInPool(ChargingStation_Id  ChargingStationId,
                                                  ChargingPool_Id     ChargingPoolId)
            : base("The given charging station identification '" + ChargingStationId + "' already exists within the given '" + ChargingPoolId + "' charging pool!")
        { }

    }

    #endregion

    #region ChargingStationCouldNotBeCreated

    /// <summary>
    /// An exception thrown whenever a charging station could not be created within the given charging pool.
    /// </summary>
    public class ChargingStationCouldNotBeCreated : ChargingPoolPoolException
    {

        public ChargingStationCouldNotBeCreated(ChargingStation_Id  ChargingStation_Id,
                                                ChargingPool_Id     ChargingPool_Id)
            : base("The given charging station identification '" + ChargingStation_Id + "' already exists within the given '" + ChargingPool_Id + "' EVS pool!")
        { }

    }

    #endregion

}
