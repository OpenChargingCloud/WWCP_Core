/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// E-Mobility Charging Session Information.
    /// </summary>
    public class SessionInfo
    {

        #region Properties

        public DateTime                      Created                 { get; }

        /// <summary>
        /// An optional list of authorize stop tokens.
        /// </summary>
        public IEnumerable<AAuthentication>  ListOfAuthStopTokens    { get; }

        public Boolean                       Finished                { get; set; }

        #endregion

        #region Constructor(s)

        public SessionInfo(AAuthentication Token)
        {
            this.Created               = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.ListOfAuthStopTokens  = new List<AAuthentication>() { Token };
        }

        #endregion

    }

}
