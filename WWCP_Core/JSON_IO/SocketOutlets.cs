/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using System.Globalization;

#endregion

namespace org.GraphDefined.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this SocketOutlet, IncludeParentIds = true)

        public static JObject ToJSON(this SocketOutlet  SocketOutlet,
                                     Boolean            IncludeParentIds = true)

            => JSONObject.Create(

                   new JProperty("Plug", SocketOutlet.Plug.ToString()),

                   SocketOutlet.CableAttached.HasValue
                       ? new JProperty("CableAttached", SocketOutlet.CableAttached.ToString())
                       : null,

                   SocketOutlet.CableLength > 0
                       ? new JProperty("CableLength",   SocketOutlet.CableLength.  ToString())
                       : null
               );

        #endregion

        #region ToJSON(this SocketOutlet, JPropertyKey)

        public static JProperty ToJSON(this SocketOutlet SocketOutlet, String JPropertyKey)
        {

            #region Initial checks

            if (SocketOutlet == null)
                throw new ArgumentNullException(nameof(SocketOutlet),  "The given socket outlet must not be null!");

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey),  "The given json property key must not be null or empty!");

            #endregion

            return new JProperty(JPropertyKey, SocketOutlet.ToJSON());

        }

        #endregion

        #region ToJSON(this SocketOutlets, IncludeParentIds = true)

        public static JArray ToJSON(this IEnumerable<SocketOutlet>  SocketOutlets,
                                    Boolean                         IncludeParentIds = true)
        {

            #region Initial checks

            if (SocketOutlets == null)
                return new JArray();

            #endregion

            return SocketOutlets != null && SocketOutlets.Any()
                       ? new JArray(SocketOutlets.SafeSelect(socket => socket.ToJSON(IncludeParentIds)))
                       : new JArray();

        }

        #endregion

        #region ToJSON(this SocketOutlets, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<SocketOutlet> SocketOutlets, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return (SocketOutlets != null)
                        ? new JProperty(JPropertyKey,
                                        SocketOutlets.ToJSON())
                        : null;

        }

        #endregion

    }

}
