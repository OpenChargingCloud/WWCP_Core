/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    #region eVehicleAlreadyExists

    /// <summary>
    /// An exception thrown whenever an electric vehicle already exists within the given e-mobility provider.
    /// </summary>
    public class eVehicleAlreadyExists : eMobilityProviderException
    {

        /// <summary>
        /// An exception thrown whenever an electric vehicle already exists within the given e-mobility provider.
        /// </summary>
        /// <param name="eMobilityProvider">The e-mobility provider.</param>
        /// <param name="eVehicleId">The electric vehicle identification.</param>
        public eVehicleAlreadyExists(EMobilityProviderProxy  eMobilityProvider,
                                     EVehicle_Id        eVehicleId)

            : base(eMobilityProvider.RoamingNetwork,
                   "The given electric vehicle identification '" + eVehicleId + "' already exists within the given '" + eMobilityProvider.Id + "' e-mobility provider!")

        { }

    }

    #endregion

    #region eVehicleAlreadyExistsInStation

    /// <summary>
    /// An exception thrown whenever an electric vehicle already exists within the given e-mobility station.
    /// </summary>
    public class eVehicleAlreadyExistsInStation : eMobilityStationException
    {

        /// <summary>
        /// An exception thrown whenever an electric vehicle already exists within the given e-mobility station.
        /// </summary>
        /// <param name="eMobilityStation">The e-mobility station.</param>
        /// <param name="eVehicleId">The electric vehicle identification.</param>
        public eVehicleAlreadyExistsInStation(eMobilityStation  eMobilityStation,
                                              EVehicle_Id       eVehicleId)

            : base(eMobilityStation.Provider,
                   "The given electric vehicle identification '" + eVehicleId + "' already exists within the given '" + eMobilityStation.Id + "' e-mobility station!")

        { }

    }

    #endregion


    #region eVehicleException

    /// <summary>
    /// An electric vehicle exception.
    /// </summary>
    public class eVehicleException : eMobilityProviderException
    {

        /// <summary>
        /// An electric vehicle exception within the given e-mobility provider.
        /// </summary>
        /// <param name="eMobilityProvider">The e-mobility provider.</param>
        /// <param name="Message">An exception message.</param>
        public eVehicleException(EMobilityProviderProxy  eMobilityProvider,
                                 String             Message)

            : base(eMobilityProvider.RoamingNetwork,
                   Message)

        { }

        /// <summary>
        /// An electric vehicle exception within the given e-mobility provider.
        /// </summary>
        /// <param name="eMobilityProvider">The e-mobility provider.</param>
        /// <param name="Message">An exception message.</param>
        /// <param name="InnerException">An inner exception.</param>
        public eVehicleException(EMobilityProviderProxy  eMobilityProvider,
                                 String             Message,
                                 Exception          InnerException)

            : base(eMobilityProvider.RoamingNetwork,
                   Message,
                   InnerException)

        { }

    }

    #endregion


}
