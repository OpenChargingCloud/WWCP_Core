﻿/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/eMI3/Core>
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

namespace org.emi3group
{

    /// <summary>
    /// A EV Charging Group exception.
    /// </summary>
    public class ChargingGroupException : eMI3Exception
    {

        public ChargingGroupException(String Message)
            : base(Message)
        { }

        public ChargingGroupException(String Message, Exception InnerException)
            : base(Message, InnerException)
        { }

    }


    #region ChargingStationAlreadyExistsInGroup

    /// <summary>
    /// An exception thrown whenever a charging station already exists within the given EV charging group.
    /// </summary>
    public class ChargingStationAlreadyExistsInGroup : ChargingGroupException
    {

        public ChargingStationAlreadyExistsInGroup(ChargingStation_Id  ChargingStation_Id,
                                                   ChargingGroup_Id    ChargingGroup_Id)
            : base("The given charging station identification '" + ChargingStation_Id + "' already exists within the given '" + ChargingGroup_Id + "' charging group!")
        { }

    }

    #endregion

}
