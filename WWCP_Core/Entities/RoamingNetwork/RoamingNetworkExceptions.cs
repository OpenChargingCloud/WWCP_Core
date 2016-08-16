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

    /// <summary>
    /// A RoamingNetwork exception.
    /// </summary>
    public class RoamingNetworkException : WWCPException
    {

        public RoamingNetworkException(String Message)
            : base(Message)
        { }

        public RoamingNetworkException(String Message, Exception InnerException)
            : base(Message, InnerException)
        { }

    }


    #region RoamingNetworkAlreadyExists

    /// <summary>
    /// An exception thrown whenever a roaming network already exists.
    /// </summary>
    public class RoamingNetworkAlreadyExists : RoamingNetworkException
    {

        public RoamingNetworkAlreadyExists(RoamingNetwork_Id  RoamingNetworkId)
            : base("The given roaming network identification '" + RoamingNetworkId + "' already exists!")
        { }

    }

    #endregion

    #region ChargingStationOperatorAlreadyExists

    /// <summary>
    /// An exception thrown whenever a charging station operator already exists within the given roaming network.
    /// </summary>
    public class ChargingStationOperatorAlreadyExists : RoamingNetworkException
    {

        public ChargingStationOperatorAlreadyExists(ChargingStationOperator_Id  ChargingStationOperatorId,
                                                    RoamingNetwork_Id           RoamingNetworkId)
            : base("The given charging station operator identification '" + ChargingStationOperatorId + "' already exists within the given '" + RoamingNetworkId + "' roaming network!")
        { }

    }

    #endregion

    #region ChargingStationOperatorAlreadyExists

    /// <summary>
    /// An exception thrown whenever a parking operator already exists within the given roaming network.
    /// </summary>
    public class ParkingOperatorAlreadyExists : RoamingNetworkException
    {

        public ParkingOperatorAlreadyExists(ParkingOperator_Id  ParkingOperatorId,
                                            RoamingNetwork_Id   RoamingNetworkId)
            : base("The given parking operator identification '" + ParkingOperatorId + "' already exists within the given '" + RoamingNetworkId + "' roaming network!")
        { }

    }

    #endregion

    #region EMobilityProviderAlreadyExists

    /// <summary>
    /// An exception thrown whenever a e-mobility provider already exists within the given roaming network.
    /// </summary>
    public class EMobilityProviderAlreadyExists : RoamingNetworkException
    {

        public EMobilityProviderAlreadyExists(EMobilityProvider_Id  EMobilityProviderId,
                                              RoamingNetwork_Id     RoamingNetworkId)
            : base("The given EV service provider identification '" + EMobilityProviderId + "' already exists within the given '" + RoamingNetworkId + "' roaming network!")
        { }

    }

    #endregion

    #region RoamingProviderAlreadyExists

    /// <summary>
    /// An exception thrown whenever a roaming provider already exists within the given roaming network.
    /// </summary>
    public class RoamingProviderAlreadyExists : RoamingNetworkException
    {

        public RoamingProviderAlreadyExists(RoamingProvider_Id  RoamingProviderId,
                                            RoamingNetwork_Id   RoamingNetworkId)
            : base("The given roaming provider identification '" + RoamingProviderId + "' already exists within the given '" + RoamingNetworkId + "' roaming network!")
        { }

    }

    #endregion

    #region SearchProviderAlreadyExists

    /// <summary>
    /// An exception thrown whenever a charging station search provider already exists within the given roaming network.
    /// </summary>
    public class SearchProviderAlreadyExists : RoamingNetworkException
    {

        public SearchProviderAlreadyExists(NavigationServiceProvider_Id SearchProviderId,
                                           RoamingNetwork_Id RoamingNetworkId)
            : base("The given search provider identification '" + SearchProviderId + "' already exists within the given '" + RoamingNetworkId + "' roaming network!")
        { }

    }

    #endregion

}
