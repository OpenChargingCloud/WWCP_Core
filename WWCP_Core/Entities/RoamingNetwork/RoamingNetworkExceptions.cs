/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

        public RoamingNetworkAlreadyExists(RoamingNetwork_Id  RoamingNetwork_Id)
            : base("The given roaming network identification '" + RoamingNetwork_Id + "' already exists!")
        { }

    }

    #endregion

    #region EVSEOperatorAlreadyExists

    /// <summary>
    /// An exception thrown whenever an EVSE operator already exists within the given roaming network.
    /// </summary>
    public class EVSEOperatorAlreadyExists : RoamingNetworkException
    {

        public EVSEOperatorAlreadyExists(EVSEOperator_Id    EVSEOperator_Id,
                                         RoamingNetwork_Id  RoamingNetwork_Id)
            : base("The given EVSE operator identification '" + EVSEOperator_Id + "' already exists within the given '" + RoamingNetwork_Id + "' roaming network!")
        { }

    }

    #endregion

    #region EVServiceProviderAlreadyExists

    /// <summary>
    /// An exception thrown whenever an electric vehicle service provider already exists within the given roaming network.
    /// </summary>
    public class EVServiceProviderAlreadyExists : RoamingNetworkException
    {

        public EVServiceProviderAlreadyExists(EVSP_Id  EVServiceProvider_Id,
                                              RoamingNetwork_Id     RoamingNetwork_Id)
            : base("The given EV service provider identification '" + EVServiceProvider_Id + "' already exists within the given '" + RoamingNetwork_Id + "' roaming network!")
        { }

    }

    #endregion

    #region RoamingProviderAlreadyExists

    /// <summary>
    /// An exception thrown whenever a roaming provider already exists within the given roaming network.
    /// </summary>
    public class RoamingProviderAlreadyExists : RoamingNetworkException
    {

        public RoamingProviderAlreadyExists(RoamingProvider_Id  RoamingProvider_Id,
                                            RoamingNetwork_Id   RoamingNetwork_Id)
            : base("The given roaming provider identification '" + RoamingProvider_Id + "' already exists within the given '" + RoamingNetwork_Id + "' roaming network!")
        { }

    }

    #endregion

    #region SearchProviderAlreadyExists

    /// <summary>
    /// An exception thrown whenever a charging station search provider already exists within the given roaming network.
    /// </summary>
    public class SearchProviderAlreadyExists : RoamingNetworkException
    {

        public SearchProviderAlreadyExists(NavigationServiceProvider_Id SearchProvider_Id,
                                           RoamingNetwork_Id RoamingNetwork_Id)
            : base("The given search provider identification '" + SearchProvider_Id + "' already exists within the given '" + RoamingNetwork_Id + "' roaming network!")
        { }

    }

    #endregion

}
