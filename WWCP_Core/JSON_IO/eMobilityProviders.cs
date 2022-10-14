/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace cloud.charging.open.protocols.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this eMobilityProvider,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given e-mobility provider.
        /// </summary>
        /// <param name="eMobilityProvider">An e-mobility provider.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        public static JObject? ToJSON(this eMobilityProvider  eMobilityProvider,
                                      Boolean                 Embedded                 = false,
                                      InfoStatus              ExpandRoamingNetworkId   = InfoStatus.ShowIdOnly,
                                      InfoStatus              ExpandBrandIds           = InfoStatus.ShowIdOnly,
                                      InfoStatus              ExpandDataLicenses       = InfoStatus.ShowIdOnly)


            => eMobilityProvider == null

                   ? null

                   : JSONObject.Create(

                         new JProperty("@id",  eMobilityProvider.Id.ToString()),

                         Embedded
                             ? new JProperty("@context",  "https://open.charging.cloud/contexts/wwcp+json/eMobilityProvider")
                             : null,

                         new JProperty("name",  eMobilityProvider.Name.ToJSON()),

                         eMobilityProvider.Description.IsNeitherNullNorEmpty()
                             ? new JProperty("description", eMobilityProvider.Description.ToJSON())
                             : null,

                         //eMobilityProvider.DataSource.  ToJSON("DataSource"),

                         ExpandDataLicenses.Switch(
                             () => new JProperty("dataLicenseIds",  new JArray(eMobilityProvider.DataLicenses.SafeSelect(license => license.Id.ToString()))),
                             () => new JProperty("dataLicenses",    eMobilityProvider.DataLicenses.ToJSON())),

                         #region Embedded means it is served as a substructure of e.g. a charging station operator

                         Embedded
                             ? null
                             : ExpandRoamingNetworkId.Switch(
                                   () => new JProperty("roamingNetworkId",   eMobilityProvider.RoamingNetwork.Id. ToString()),
                                   () => new JProperty("roamingNetwork",     eMobilityProvider.RoamingNetwork.    ToJSON(Embedded:                   true,
                                                                                                                         ExpandEMobilityProviderId:  InfoStatus.Hidden,
                                                                                                                         ExpandChargingPoolIds:      InfoStatus.Hidden,
                                                                                                                         ExpandChargingStationIds:   InfoStatus.Hidden,
                                                                                                                         ExpandEVSEIds:              InfoStatus.Hidden,
                                                                                                                         ExpandBrandIds:             InfoStatus.Hidden,
                                                                                                                         ExpandDataLicenses:         InfoStatus.Hidden))),

                         #endregion

                         eMobilityProvider.Address is not null
                             ? new JProperty("address", eMobilityProvider.Address.ToJSON())
                             : null,

                         // LogoURI
                         // API
                         // MainKeys
                         // RobotKeys
                         // Endpoints
                         // DNS SRV

                         eMobilityProvider.Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          eMobilityProvider.Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         eMobilityProvider.Homepage.IsNotNullOrEmpty()
                             ? new JProperty("homepage",            eMobilityProvider.Homepage)
                             : null,

                         eMobilityProvider.HotlinePhoneNumber.IsNotNullOrEmpty()
                             ? new JProperty("hotline",             eMobilityProvider.HotlinePhoneNumber)
                             : null,

                         eMobilityProvider.DataLicenses.Any()
                             ? new JProperty("dataLicenses",        new JArray(eMobilityProvider.DataLicenses.Select(license => license.ToJSON())))
                             : null

                     );

        #endregion

        #region ToJSON(this eMobilityProviders, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of e-mobility providers.
        /// </summary>
        /// <param name="eMobilityProviders">An enumeration of e-mobility providers.</param>
        /// <param name="Skip">The optional number of e-mobility providers to skip.</param>
        /// <param name="Take">The optional number of e-mobility providers to return.</param>
        public static JArray ToJSON(this IEnumerable<eMobilityProvider>  eMobilityProviders,
                                    UInt64?                              Skip                     = null,
                                    UInt64?                              Take                     = null,
                                    Boolean                              Embedded                 = false,
                                    InfoStatus                           ExpandRoamingNetworkId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                           ExpandBrandIds           = InfoStatus.ShowIdOnly,
                                    InfoStatus                           ExpandDataLicenses       = InfoStatus.ShowIdOnly)


            => eMobilityProviders == null

                   ? new JArray()

                   : new JArray(eMobilityProviders.
                                    Where     (emp => emp != null).
                                    OrderBy   (emp => emp.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(emp => emp.ToJSON(Embedded,
                                                                 ExpandRoamingNetworkId,
                                                                 ExpandBrandIds,
                                                                 ExpandDataLicenses)));

        #endregion


        #region ToJSON(this eMobilityProviderAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderAdminStatusTypes>>>>  eMobilityProviderAdminStatus,
                                     UInt64?                                                                                                            Skip         = null,
                                     UInt64?                                                                                                            Take         = null,
                                     UInt64                                                                                                             HistorySize  = 1)

        {

            #region Initial checks

            if (eMobilityProviderAdminStatus == null || !eMobilityProviderAdminStatus.Any())
                return new JObject();

            var _eMobilityProviderAdminStatus = new Dictionary<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderAdminStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate eMobilityProvider identifications in the enumeration... take the newest one!

            foreach (var csostatus in Take.HasValue ? eMobilityProviderAdminStatus.Skip(Skip).Take(Take)
                                                    : eMobilityProviderAdminStatus.Skip(Skip))
            {

                if (!_eMobilityProviderAdminStatus.ContainsKey(csostatus.Key))
                    _eMobilityProviderAdminStatus.Add(csostatus.Key, csostatus.Value);

                else if (csostatus.Value.FirstOrDefault().Timestamp > _eMobilityProviderAdminStatus[csostatus.Key].FirstOrDefault().Timestamp)
                    _eMobilityProviderAdminStatus[csostatus.Key] = csostatus.Value;

            }

            #endregion

            return _eMobilityProviderAdminStatus.Count == 0

                   ? new JObject()

                   : new JObject(_eMobilityProviderAdminStatus.
                                     SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                  new JObject(statuslist.Value.

                                                                              // Will filter multiple cso status having the exact same ISO 8601 timestamp!
                                                                              GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                              Select           (group => group.First()).

                                                                              OrderByDescending(tsv   => tsv.Timestamp).
                                                                              Take             (HistorySize).
                                                                              Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                       tsv.Value.    ToString())))

                                                              )));

        }

        #endregion


        #region ToJSON(this eMobilityProviderStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderStatusTypes>>>>  eMobilityProviderStatus,
                                     UInt64?                                                                                                       Skip         = null,
                                     UInt64?                                                                                                       Take         = null,
                                     UInt64?                                                                                                       HistorySize  = 1)

        {

            #region Initial checks

            if (eMobilityProviderStatus == null || !eMobilityProviderStatus.Any())
                return new JObject();

            var _eMobilityProviderStatus = new Dictionary<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate eMobilityProvider identifications in the enumeration... take the newest one!

            foreach (var csostatus in Take.HasValue ? eMobilityProviderStatus.Skip(Skip).Take(Take)
                                                    : eMobilityProviderStatus.Skip(Skip))
            {

                if (!_eMobilityProviderStatus.ContainsKey(csostatus.Key))
                    _eMobilityProviderStatus.Add(csostatus.Key, csostatus.Value);

                else if (csostatus.Value.FirstOrDefault().Timestamp > _eMobilityProviderStatus[csostatus.Key].FirstOrDefault().Timestamp)
                    _eMobilityProviderStatus[csostatus.Key] = csostatus.Value;

            }

            #endregion

            return _eMobilityProviderStatus.Count == 0

                   ? new JObject()

                   : new JObject(_eMobilityProviderStatus.
                                     SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                  new JObject(statuslist.Value.

                                                                              // Will filter multiple cso status having the exact same ISO 8601 timestamp!
                                                                              GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                              Select           (group => group.First()).

                                                                              OrderByDescending(tsv   => tsv.Timestamp).
                                                                              Take             (HistorySize).
                                                                              Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                       tsv.Value.    ToString())))

                                                              )));

        }

        #endregion


    }

}
