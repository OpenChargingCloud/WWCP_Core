﻿/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    #region ChargingStationAlreadyExistsInPool

    /// <summary>
    /// An exception thrown whenever a charging station already exists within the given charging pool.
    /// </summary>
    public class ChargingStationAlreadyExistsInPool : ChargingPoolException
    {

        public ChargingStationAlreadyExistsInPool(ChargingPool ChargingPool,
                                                  ChargingStation_Id ChargingStationId)

            : base(ChargingPool.Operator,
                   "The given charging station identification '" + ChargingStationId + "' already exists within the given '" + ChargingPool.Id + "' charging pool!")

        { }

    }

    #endregion

    #region ChargingStationCouldNotBeCreated

    /// <summary>
    /// An exception thrown whenever a charging station could not be created within the given charging pool.
    /// </summary>
    public class ChargingStationCouldNotBeCreated : ChargingPoolException
    {

        public ChargingStationCouldNotBeCreated(ChargingPool        ChargingPool,
                                                ChargingStation_Id  ChargingStation_Id)

            : base(ChargingPool.Operator,
                   "The given charging station identification '" + ChargingStation_Id + "' already exists within the given '" + ChargingPool.Id + "' EVS pool!")

        { }

    }

    #endregion




    /// <summary>
    /// A EVS pool exception.
    /// </summary>
    public class ChargingStationException : ChargingPoolException
    {

        public ChargingStationException(ChargingPool  ChargingPool,
                                        String        Message)

            : base(ChargingPool.Operator,
                   Message)

        { }

        public ChargingStationException(ChargingPool  ChargingPool,
                                        String        Message,
                                        Exception     InnerException)

            : base(ChargingPool.Operator, Message, InnerException)

        { }

    }


    #region InvalidEVSEOperatorId

    /// <summary>
    /// An invalid EVSE operator identification was given.
    /// </summary>
    public class InvalidEVSEOperatorId : ChargingStationException
    {

        /// <summary>
        /// An invalid EVSE operator identification was given.
        /// </summary>
        /// <param name="ChargingStation">The charging station in which the exception occured.</param>
        /// <param name="InvalidChargingStationOperatorId">The invalid operator identification.</param>
        /// <param name="ValidChargingStationOperatorIds">All expected operator identifications.</param>
        public InvalidEVSEOperatorId(ChargingStation                          ChargingStation,
                                     ChargingStationOperator_Id               InvalidChargingStationOperatorId,
                                     IEnumerable<ChargingStationOperator_Id>  ValidChargingStationOperatorIds)

            : base(ChargingStation.ChargingPool,
                   "Invalid charging station operator identification '" + InvalidChargingStationOperatorId + "' where only '" + ValidChargingStationOperatorIds.AggregateWith(", ") + "' are expected!")

        { }

    }

    #endregion

}
