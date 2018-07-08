/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        #region ToJSON(this GridConnection, JPropertyKey)

        public static JProperty ToJSON(this GridConnectionTypes GridConnection, String JPropertyKey)

            => GridConnection != GridConnectionTypes.Unknown
                   ? new JProperty(JPropertyKey,
                                   GridConnection.ToString())
                   : null;

        #endregion

        #region ToJSON(this ChargingStationUIFeatures, JPropertyKey)

        public static JProperty ToJSON(this UIFeatures ChargingStationUIFeatures, String JPropertyKey)

            => new JProperty(JPropertyKey,
                             ChargingStationUIFeatures.ToString());

        #endregion

        #region ToJSON(this AuthenticationModes, JPropertyKey)

        public static JProperty ToJSON(this ReactiveSet<AuthenticationModes> AuthenticationModes, String JPropertyKey)

            => AuthenticationModes != null
                   ? new JProperty(JPropertyKey,
                                   new JArray(AuthenticationModes.SafeSelect(mode => mode.ToJSON())))
                   : null;

        #endregion

    }

}
