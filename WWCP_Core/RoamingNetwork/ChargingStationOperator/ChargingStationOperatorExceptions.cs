/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    #region ChargingStationOperatorAlreadyExists

    /// <summary>
    /// An exception thrown whenever a charging station operator already exists within the given roaming network.
    /// </summary>
    public class ChargingStationOperatorAlreadyExists : RoamingNetworkException
    {

        /// <summary>
        /// An exception thrown whenever a charging station operator already exists within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperatorId">The charging station operator identification.</param>
        /// <param name="Name">The multi-language name of the charging station operator.</param>
        public ChargingStationOperatorAlreadyExists(RoamingNetwork              RoamingNetwork,
                                                    ChargingStationOperator_Id  ChargingStationOperatorId,
                                                    I18NString                  Name)

            : base(RoamingNetwork,
                   "The given charging station operator identification '" + ChargingStationOperatorId + "' with name '" + Name?.FirstText() + "' already exists within the given '" + RoamingNetwork.Id + "' roaming network!")

        { }

    }

    #endregion


    #region ChargingStationOperatorException

    /// <summary>
    /// An charging station operator exception.
    /// </summary>
    public class ChargingStationOperatorException : RoamingNetworkException
    {

        /// <summary>
        /// An charging station operator exception within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Message">An exception message.</param>
        public ChargingStationOperatorException(RoamingNetwork  RoamingNetwork,
                                                String          Message)

            : base(RoamingNetwork,
                   Message)

        { }

        /// <summary>
        /// An charging station operator exception within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Message">An exception message.</param>
        /// <param name="InnerException">An inner exception.</param>
        public ChargingStationOperatorException(RoamingNetwork  RoamingNetwork,
                                                String          Message,
                                                Exception       InnerException)

            : base(RoamingNetwork,
                   Message,
                   InnerException)

        { }

    }

    #endregion


    #region InvalidChargingStationOperatorId

    /// <summary>
    /// An invalid charging pool operator identification was given.
    /// </summary>
    public class InvalidChargingPoolOperatorId : ChargingPoolException
    {

        /// <summary>
        /// An invalid charging pool operator identification was given.
        /// </summary>
        /// <param name="ChargingStationOperator">The charging station operator in which the exception occured.</param>
        /// <param name="InvalidChargingStationOperatorId">The invalid operator identification.</param>
        /// <param name="ValidChargingStationOperatorIds">All expected operator identifications.</param>
        public InvalidChargingPoolOperatorId(ChargingStationOperator                  ChargingStationOperator,
                                             ChargingStationOperator_Id               InvalidChargingStationOperatorId,
                                             IEnumerable<ChargingStationOperator_Id>  ValidChargingStationOperatorIds)

            : base(ChargingStationOperator,
                   "Invalid charging station operator identification '" + InvalidChargingStationOperatorId + "' where only '" + ValidChargingStationOperatorIds.AggregateWith(", ") + "' are expected!")

        { }

    }

    #endregion


    #region BrandAlreadyExists

    /// <summary>
    /// An exception thrown whenever a brand already exists within the given charging station operator.
    /// </summary>
    public class BrandAlreadyExists : ChargingStationOperatorException
    {

        public BrandAlreadyExists(ChargingStationOperator  ChargingStationOperator,
                                  Brand_Id                 BrandId)

            : base(ChargingStationOperator.RoamingNetwork,
                   "The given brand identification '" + BrandId + "' already exists within the given '" + ChargingStationOperator.Id + "' charging station operator!")

        { }

    }

    #endregion

}
