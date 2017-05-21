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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this EMobilityProvider, Embedded = false, ExpandChargingRoamingNetworkId = false)

        public static JObject ToJSON(this eMobilityProvider  EMobilityProvider,
                                     Boolean                 Embedded                        = false,
                                     Boolean                 ExpandChargingRoamingNetworkId  = false)

            => EMobilityProvider != null
                   ? JSONObject.Create(

                         new JProperty("id",                        EMobilityProvider.Id.ToString()),

                         Embedded
                             ? null
                             : ExpandChargingRoamingNetworkId
                                   ? new JProperty("roamingNetwork",      EMobilityProvider.RoamingNetwork.ToJSON())
                                   : new JProperty("roamingNetworkId",    EMobilityProvider.RoamingNetwork.Id.ToString()),

                         new JProperty("name",                  EMobilityProvider.Name.       ToJSON()),
                         new JProperty("description",           EMobilityProvider.Description.ToJSON()),

                         // Address
                         // LogoURI
                         // API - RobotKeys, Endpoints, DNS SRV
                         // MainKeys

                         EMobilityProvider.Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          EMobilityProvider.Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         EMobilityProvider.Homepage.IsNotNullOrEmpty()
                             ? new JProperty("homepage",            EMobilityProvider.Homepage)
                             : null,

                         EMobilityProvider.HotlinePhoneNumber.IsNotNullOrEmpty()
                             ? new JProperty("hotline",             EMobilityProvider.HotlinePhoneNumber)
                             : null,

                         EMobilityProvider.DataLicenses.Any()
                             ? new JProperty("dataLicenses",        new JArray(EMobilityProvider.DataLicenses.Select(license => license.ToJSON())))
                             : null

                     )
                   : null;

        #endregion

        #region ToJSON(this EMobilityProvider, JPropertyKey)

        public static JProperty ToJSON(this eMobilityProvider EMobilityProvider, String JPropertyKey)

            => EMobilityProvider != null
                   ? new JProperty(JPropertyKey, EMobilityProvider.ToJSON())
                   : null;

        #endregion

        #region ToJSON(this EMobilityProviders, Skip = 0, Take = 0, Embedded = false, ExpandChargingRoamingNetworkId = false)

        /// <summary>
        /// Return a JSON representation for the given enumeration of e-mobility providers.
        /// </summary>
        /// <param name="EMobilityProviders">An enumeration of e-mobility providers.</param>
        /// <param name="Skip">The optional number of e-mobility providers to skip.</param>
        /// <param name="Take">The optional number of e-mobility providers to return.</param>
        public static JArray ToJSON(this IEnumerable<eMobilityProvider>  EMobilityProviders,
                                    UInt64                               Skip                            = 0,
                                    UInt64                               Take                            = 0,
                                    Boolean                              Embedded                        = false,
                                    Boolean                              ExpandChargingRoamingNetworkId  = false)
        {

            #region Initial checks

            if (EMobilityProviders == null)
                return new JArray();

            #endregion

            return new JArray(EMobilityProviders.
                                  Where     (cso => cso != null).
                                  OrderBy   (cso => cso.Id).
                                  SkipTakeFilter(Skip, Take).
                                  SafeSelect(cso => cso.ToJSON(Embedded,
                                                               ExpandChargingRoamingNetworkId)));

        }

        #endregion

        #region ToJSON(this EMobilityProviders, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<eMobilityProvider> EMobilityProviders, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return EMobilityProviders != null
                       ? new JProperty(JPropertyKey, EMobilityProviders.ToJSON())
                       : null;

        }

        #endregion

    }

}
