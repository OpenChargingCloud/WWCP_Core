/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP
{

    #region SmartCityAlreadyExists

    /// <summary>
    /// An exception thrown whenever a smart city already exists within the given roaming network.
    /// </summary>
    public class SmartCityAlreadyExists : RoamingNetworkException
    {

        /// <summary>
        /// An exception thrown whenever a smart city already exists within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="SmartCityId">The smart city identification.</param>
        public SmartCityAlreadyExists(RoamingNetwork  RoamingNetwork,
                                      SmartCity_Id    SmartCityId)

            : base(RoamingNetwork,
                   "The given smart city identification '" + SmartCityId + "' already exists within the given '" + RoamingNetwork.Id + "' roaming network!")

        { }

    }

    #endregion


    #region SmartCityException

    /// <summary>
    /// An smart city exception.
    /// </summary>
    public class SmartCityException : RoamingNetworkException
    {

        /// <summary>
        /// An smart city exception within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Message">An exception message.</param>
        public SmartCityException(RoamingNetwork  RoamingNetwork,
                                  String          Message)

            : base(RoamingNetwork,
                   Message)

        { }

        /// <summary>
        /// An smart city exception within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Message">An exception message.</param>
        /// <param name="InnerException">An inner exception.</param>
        public SmartCityException(RoamingNetwork  RoamingNetwork,
                                  String          Message,
                                  Exception       InnerException)

            : base(RoamingNetwork,
                   Message,
                   InnerException)

        { }

    }

    #endregion


}
