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

    #region ParkingOperatorAlreadyExists

    /// <summary>
    /// An exception thrown whenever a parking operator already exists within the given roaming network.
    /// </summary>
    public class ParkingOperatorAlreadyExists : RoamingNetworkException
    {

        /// <summary>
        /// An exception thrown whenever a parking operator already exists within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ParkingOperatorId">The parking operator identification.</param>
        public ParkingOperatorAlreadyExists(RoamingNetwork      RoamingNetwork,
                                            ParkingOperator_Id  ParkingOperatorId)

            : base(RoamingNetwork,
                   "The given parking operator identification '" + ParkingOperatorId + "' already exists within the given '" + RoamingNetwork.Id + "' roaming network!")

        { }

    }

    #endregion


    #region ParkingOperatorException

    /// <summary>
    /// An parking operator exception.
    /// </summary>
    public class ParkingOperatorException : RoamingNetworkException
    {

        /// <summary>
        /// An parking operator exception within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Message">An exception message.</param>
        public ParkingOperatorException(RoamingNetwork  RoamingNetwork,
                                        String          Message)

            : base(RoamingNetwork,
                   Message)

        { }

        /// <summary>
        /// An parking operator exception within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Message">An exception message.</param>
        /// <param name="InnerException">An inner exception.</param>
        public ParkingOperatorException(RoamingNetwork  RoamingNetwork,
                                        String          Message,
                                        Exception       InnerException)

            : base(RoamingNetwork,
                   Message,
                   InnerException)

        { }

    }

    #endregion


}
