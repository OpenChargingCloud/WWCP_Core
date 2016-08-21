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
using System.Collections.Generic;
using System.Collections.Concurrent;

#endregion

namespace org.GraphDefined.WWCP
{

    #region eMobilityStationAlreadyExistsInPool

    /// <summary>
    /// An exception thrown whenever a charging station already exists at the given e-mobility provider.
    /// </summary>
    public class eMobilityStationAlreadyExistsAtProvider : eMobilityProviderException
    {

        public eMobilityStationAlreadyExistsAtProvider(eMobilityProvider    eMobilityProvider,
                                                       eMobilityStation_Id  eMobilityStationId)

            : base(eMobilityProvider.RoamingNetwork,
                   "The given e-mobility station identification '" + eMobilityStationId + "' already exists within the given '" + eMobilityProvider.Id + "' e-mobility provider!")

        { }

    }

    #endregion

    #region eMobilityStationCouldNotBeCreated

    /// <summary>
    /// An exception thrown whenever a e-mobility station could not be created at the given e-mobility provider.
    /// </summary>
    public class eMobilityStationCouldNotBeCreated : eMobilityProviderException
    {

        public eMobilityStationCouldNotBeCreated(eMobilityProvider    eMobilityProvider,
                                                 eMobilityStation_Id  eMobilityStation_Id)

            : base(eMobilityProvider.RoamingNetwork,
                   "The given e-mobility station identification '" + eMobilityStation_Id + "' already exists at the given '" + eMobilityProvider.Id + "' e-mobility provider!")

        { }

    }

    #endregion




    /// <summary>
    /// A e-mobility station exception.
    /// </summary>
    public class eMobilityStationException : eMobilityProviderException
    {

        public eMobilityStationException(eMobilityProvider  eMobilityProvider,
                                         String             Message)

            : base(eMobilityProvider.RoamingNetwork,
                   Message)

        { }

        public eMobilityStationException(eMobilityProvider  eMobilityProvider,
                                         String             Message,
                                         Exception          InnerException)

            : base(eMobilityProvider.RoamingNetwork,
                   Message,
                   InnerException)

        { }

    }

}
