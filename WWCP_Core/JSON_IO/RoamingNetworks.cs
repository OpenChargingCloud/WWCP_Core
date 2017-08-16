/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Net <https://github.com/OpenChargingCloud/WWCP_Net>
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
using System.Linq;

using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this RoamingNetworks, Skip = 0, Take = 0)

        /// <summary>
        /// Return a JSON representation for the given roaming networks collection.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="Skip">The optional number of roaming networks to skip.</param>
        /// <param name="Take">The optional number of roaming networks to return.</param>
        public static JArray ToJSON(this RoamingNetworks  RoamingNetworks,
                                    UInt64                Skip  = 0,
                                    UInt64                Take  = 0)

            => RoamingNetworks != null && RoamingNetworks.Any()
                    ? new JArray(RoamingNetworks.AsEnumerable().ToJSON(Skip, Take))
                    : new JArray();

        #endregion

        #region ToJSON(this RoamingNetworks, JPropertyKey, Skip = 0, Take = 0)

        /// <summary>
        /// Return a JSON representation for the given roaming networks collection
        /// using the given JSON property key.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="JPropertyKey">The name of the JSON property key to use.</param>
        /// <param name="Skip">The optional number of roaming networks to skip.</param>
        /// <param name="Take">The optional number of roaming networks to return.</param>
        public static JProperty ToJSON(this RoamingNetworks  RoamingNetworks,
                                       String                JPropertyKey,
                                       UInt64                Skip  = 0,
                                       UInt64                Take  = 0)

            => RoamingNetworks != null && RoamingNetworks.Any()
                    ? RoamingNetworks.AsEnumerable().ToJSON(JPropertyKey, Skip, Take)
                    : null;

        #endregion

    }

}
