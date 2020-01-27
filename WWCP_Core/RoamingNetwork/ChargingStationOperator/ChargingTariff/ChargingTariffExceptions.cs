/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace org.GraphDefined.WWCP
{

    #region ChargingTariffAlreadyExists

    /// <summary>
    /// An exception thrown whenever a charging tariff already exists within the given charging station operator.
    /// </summary>
    public class ChargingTariffAlreadyExists : ChargingStationOperatorException
    {

        public ChargingTariffAlreadyExists(ChargingStationOperator  ChargingStationOperator,
                                           ChargingTariff_Id        ChargingTariffId)

            : base(ChargingStationOperator.RoamingNetwork,
                   "The given charging tariff identification '" + ChargingTariffId + "' already exists within the given '" + ChargingStationOperator.Id + "' charging station operator!")

        { }

    }

    #endregion


    /// <summary>
    /// A charging tariff exception.
    /// </summary>
    public class ChargingTariffException : ChargingStationOperatorException
    {

        public ChargingTariffException(ChargingStationOperator  ChargingStationOperator,
                                       String                   Message)

            : base(ChargingStationOperator.RoamingNetwork,
                   Message)

        { }

        public ChargingTariffException(ChargingStationOperator  ChargingStationOperator,
                                       String                   Message,
                                       Exception                InnerException)

            : base(ChargingStationOperator.RoamingNetwork,
                   Message,
                   InnerException)

        { }

    }

}
