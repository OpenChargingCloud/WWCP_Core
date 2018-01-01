/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace org.GraphDefined.WWCP
{

    #region RoamingNetworkAlreadyExists

    /// <summary>
    /// An exception thrown whenever a roaming network already exists.
    /// </summary>
    public class RoamingNetworkAlreadyExists : WWCPException
    {

        public RoamingNetworkAlreadyExists(RoamingNetwork_Id  RoamingNetworkId)

            : base("The given roaming network identification '" + RoamingNetworkId + "' already exists!")

        { }

    }

    #endregion


    #region RoamingNetworkException

    /// <summary>
    /// A roaming network exception.
    /// </summary>
    public class RoamingNetworkException : WWCPException
    {

        /// <summary>
        /// A roaming network exception within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Message">An exception message.</param>
        public RoamingNetworkException(RoamingNetwork  RoamingNetwork,
                                       String          Message)

            : base(Message)

        { }

        /// <summary>
        /// A roaming network exception within the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Message">An exception message.</param>
        /// <param name="InnerException">An inner exception.</param>
        public RoamingNetworkException(RoamingNetwork  RoamingNetwork,
                                       String          Message,
                                       Exception       InnerException)

            : base(Message, InnerException)

        { }

    }

    #endregion

}
