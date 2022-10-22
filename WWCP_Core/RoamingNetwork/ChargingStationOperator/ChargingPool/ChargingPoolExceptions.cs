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

namespace cloud.charging.open.protocols.WWCP
{

    #region ChargingPoolAlreadyExists

    /// <summary>
    /// An exception thrown whenever a charging pool already exists within the given charging station operator.
    /// </summary>
    public class ChargingPoolAlreadyExists : ChargingStationOperatorException
    {

        public ChargingPoolAlreadyExists(ChargingStationOperator  ChargingStationOperator,
                                         ChargingPool_Id          ChargingPoolId)

            : base(ChargingStationOperator.RoamingNetwork,
                   "The given charging pool identification '" + ChargingPoolId + "' already exists within the given '" + ChargingStationOperator.Id + "' charging station operator!")

        { }

    }

    #endregion



    /// <summary>
    /// A charging pool exception.
    /// </summary>
    public class ChargingPoolException : ChargingStationOperatorException
    {

        public ChargingPoolException(ChargingStationOperator  ChargingStationOperator,
                                     String                   Message)

            : base(ChargingStationOperator.RoamingNetwork,
                   Message)

        { }

        public ChargingPoolException(ChargingStationOperator  ChargingStationOperator,
                                     String                   Message,
                                     Exception                InnerException)

            : base(ChargingStationOperator.RoamingNetwork,
                   Message, InnerException)

        { }

    }


    #region InvalidChargingStationOperatorId

    /// <summary>
    /// An invalid charging station operator identification was given.
    /// </summary>
    public class InvalidChargingStationOperatorId : ChargingPoolException
    {

        /// <summary>
        /// An invalid charging station operator identification was given.
        /// </summary>
        /// <param name="ChargingPool">The charging pool in which the exception occured.</param>
        /// <param name="InvalidChargingStationOperatorId">The invalid operator identification.</param>
        public InvalidChargingStationOperatorId(ChargingPool                ChargingPool,
                                                ChargingStationOperator_Id  InvalidChargingStationOperatorId)

            : base(ChargingPool.Operator,
                   "Invalid charging station operator identification '" + InvalidChargingStationOperatorId + "'!")

        { }

    }

    #endregion


}
