/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charging station operator exception.
    /// </summary>
    public class ChargingStationOperatorException : WWCPException
    {

        public ChargingStationOperatorException(String Message)
            : base(Message)
        { }

        public ChargingStationOperatorException(String Message, Exception InnerException)
            : base(Message, InnerException)
        { }

    }


    #region ChargingPoolAlreadyExists

    /// <summary>
    /// An exception thrown whenever a charging pool already exists within the given charging station operator.
    /// </summary>
    public class ChargingPoolAlreadyExists : ChargingStationOperatorException
    {

        public ChargingPoolAlreadyExists(ChargingPool_Id             ChargingPoolId,
                                         ChargingStationOperator_Id  ChargingStationOperatorId)
            : base("The given charging pool identification '" + ChargingPoolId + "' already exists within the given '" + ChargingStationOperatorId + "' charging station operator!")
        { }

    }

    #endregion


    #region ChargingStationGroupAlreadyExists

    /// <summary>
    /// An exception thrown whenever a charging station group already exists within the given charging station operator.
    /// </summary>
    public class ChargingStationGroupAlreadyExists : ChargingStationOperatorException
    {

        public ChargingStationGroupAlreadyExists(ChargingStationGroup_Id     ChargingStationGroupId,
                                                 ChargingStationOperator_Id  ChargingStationOperatorId)
            : base("The given charging station group identification '" + ChargingStationGroupId + "' already exists within the given '" + ChargingStationOperatorId + "' charging station operator!")
        { }

    }

    #endregion

}
