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
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this RoamingNetwork,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure.</param>
        public static JObject ToJSON(this IRoamingNetwork  RoamingNetwork,
                                     Boolean               Embedded                           = false,
                                     InfoStatus            ExpandChargingStationOperatorIds   = InfoStatus.ShowIdOnly,
                                     InfoStatus            ExpandChargingPoolIds              = InfoStatus.ShowIdOnly,
                                     InfoStatus            ExpandChargingStationIds           = InfoStatus.ShowIdOnly,
                                     InfoStatus            ExpandEVSEIds                      = InfoStatus.ShowIdOnly,
                                     InfoStatus            ExpandBrandIds                     = InfoStatus.ShowIdOnly,
                                     InfoStatus            ExpandDataLicenses                 = InfoStatus.ShowIdOnly,

                                     InfoStatus            ExpandEMobilityProviderId          = InfoStatus.ShowIdOnly)


            => RoamingNetwork == null
                   ? null

                   : JSONObject.Create(

                         new JProperty("@id", RoamingNetwork.Id.ToString()),

                         Embedded
                             ? null
                             : new JProperty("@context", "https://open.charging.cloud/contexts/wwcp+json/RoamingNetwork"),

                         new JProperty("name", RoamingNetwork.Name.ToJSON()),

                         RoamingNetwork.Description.IsNeitherNullNorEmpty()
                             ? RoamingNetwork.Description.ToJSON("description")
                             : null,

                         RoamingNetwork.DataSource.IsNeitherNullNorEmpty()
                             ? RoamingNetwork.DataSource.ToJSON("dataSource")
                             : null,

                         RoamingNetwork.DataLicenses.Any()
                             ? ExpandDataLicenses.Switch(
                                 () => new JProperty("dataLicenseIds",  new JArray(RoamingNetwork.DataLicenses.SafeSelect(license => license.Id.ToString()))),
                                 () => new JProperty("dataLicenses",    RoamingNetwork.DataLicenses.ToJSON()))
                             : null,

                         RoamingNetwork.ChargingStationOperators.Any()
                             ? ExpandChargingStationOperatorIds.Switch(

                                   () => new JProperty("chargingStationOperatorIds",  new JArray(RoamingNetwork.ChargingStationOperatorIds.
                                                                                                                OrderBy(id => id).
                                                                                                                Select (id => id.ToString()))),

                                   () => new JProperty("chargingStationOperators",    new JArray(RoamingNetwork.ChargingStationOperators.
                                                                                                                OrderBy(cso => cso).
                                                                                                                ToJSON (Embedded: true,
                                                                                                                        ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                        ExpandChargingPoolIds:            InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationIds:         InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:               InfoStatus.Hidden))))
                             : null,

                         !RoamingNetwork.ChargingStationOperators.Any() || ExpandChargingStationOperatorIds == InfoStatus.Expand
                             ? null
                             : ExpandChargingPoolIds.Switch(
                                   () => new JProperty("chargingPoolIds",             new JArray(RoamingNetwork.ChargingPoolIds().
                                                                                                                OrderBy(id => id).
                                                                                                                Select (id => id.ToString()))),

                                   () => new JProperty("chargingPools",               new JArray(RoamingNetwork.ChargingPools.
                                                                                                                OrderBy(pool => pool).
                                                                                                                ToJSON (Embedded: true,
                                                                                                                        ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationIds:         InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:               InfoStatus.Hidden)))),

                         !RoamingNetwork.ChargingStationOperators.Any() || (ExpandChargingPoolIds == InfoStatus.Expand || ExpandChargingStationOperatorIds == InfoStatus.Expand)
                             ? null
                             : ExpandChargingStationIds.Switch(
                                   () => new JProperty("chargingStationIds",          new JArray(RoamingNetwork.ChargingStationIds().
                                                                                                                OrderBy(id => id).
                                                                                                                Select (id => id.ToString()))),

                                   () => new JProperty("chargingStations",            new JArray(RoamingNetwork.ChargingStations.
                                                                                                                OrderBy(station => station).
                                                                                                                ToJSON (Embedded: true,
                                                                                                                        ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                                        ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:               InfoStatus.Hidden)))),

                         !RoamingNetwork.ChargingStationOperators.Any() || (ExpandChargingStationIds == InfoStatus.Expand || ExpandChargingPoolIds == InfoStatus.Expand || ExpandChargingStationOperatorIds == InfoStatus.Expand)
                             ? null
                             : ExpandEVSEIds.Switch(
                                   () => new JProperty("EVSEIds",                     new JArray(RoamingNetwork.EVSEIds().
                                                                                                                OrderBy(id => id).
                                                                                                                Select (id => id.ToString()))),

                                   () => new JProperty("EVSEs",                       new JArray(RoamingNetwork.EVSEs.
                                                                                                                OrderBy(evse => evse).
                                                                                                                ToJSON (Embedded: true,
                                                                                                                        ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                                        ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationId:          InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:               InfoStatus.Hidden)))),


                         RoamingNetwork.eMobilityProviders.Any()
                             ? ExpandEMobilityProviderId.Switch(
                                   () => new JProperty("eMobilityProviderIds",        new JArray(RoamingNetwork.ChargingStationOperatorIds.
                                                                                                                OrderBy(id => id).
                                                                                                                Select (id => id.ToString()))),

                                   () => new JProperty("eMobilityProviders",          new JArray(RoamingNetwork.eMobilityProviders.
                                                                                                                OrderBy(emp=> emp).
                                                                                                                ToJSON (Embedded: true,
                                                                                                                        ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:               InfoStatus.Hidden))))
                             : null

                     );

        #endregion

        #region ToJSON(this RoamingNetworks, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="Skip">The optional number of roaming networks to skip.</param>
        /// <param name="Take">The optional number of roaming networks to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure.</param>
        public static JArray ToJSON(this IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                    UInt64?                            Skip                               = null,
                                    UInt64?                            Take                               = null,
                                    Boolean                            Embedded                           = false,
                                    InfoStatus                         ExpandChargingStationOperatorIds   = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandRoamingNetworkIds              = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandChargingStationIds           = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandEVSEIds                      = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandBrandIds                     = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandDataLicenses                 = InfoStatus.ShowIdOnly,

                                    InfoStatus                         ExpandEMobilityProviderId          = InfoStatus.ShowIdOnly)


        => RoamingNetworks == null || !RoamingNetworks.Any()

                   ? new JArray()

                   : new JArray(RoamingNetworks.
                                    Where     (roamingnetwork => roamingnetwork != null).
                                    OrderBy   (roamingnetwork => roamingnetwork.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(roamingnetwork => roamingnetwork.ToJSON(Embedded,
                                                                                       ExpandChargingStationOperatorIds,
                                                                                       ExpandRoamingNetworkIds,
                                                                                       ExpandChargingStationIds,
                                                                                       ExpandEVSEIds,
                                                                                       ExpandBrandIds,
                                                                                       ExpandDataLicenses,
                                                                                       ExpandEMobilityProviderId)));

        #endregion

        #region ToJSON(this RoamingNetworks, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<IRoamingNetwork> RoamingNetworks, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return RoamingNetworks?.Any() == true
                       ? new JProperty(JPropertyKey, RoamingNetworks.ToJSON())
                       : null;

        }

        #endregion


        #region ToJSON(this RoamingNetworkAdminStatus,          Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<RoamingNetworkAdminStatus>  RoamingNetworkAdminStatus,
                                     UInt64?                                      Skip  = null,
                                     UInt64?                                      Take  = null)
        {

            #region Initial checks

            if (RoamingNetworkAdminStatus == null || !RoamingNetworkAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<RoamingNetwork_Id, RoamingNetworkAdminStatus>();

            foreach (var status in RoamingNetworkAdminStatus)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region ToJSON(this RoamingNetworkAdminStatusSchedules, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<RoamingNetworkAdminStatusSchedule>  RoamingNetworkAdminStatusSchedules,
                                     UInt64?                                              Skip         = null,
                                     UInt64?                                              Take         = null,
                                     UInt64?                                              HistorySize  = 1)
        {

            #region Initial checks

            if (RoamingNetworkAdminStatusSchedules == null || !RoamingNetworkAdminStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<RoamingNetwork_Id, RoamingNetworkAdminStatusSchedule>();

            foreach (var status in RoamingNetworkAdminStatusSchedules)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].StatusSchedule.Any() &&
                         _FilteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JObject(
                                                                   kvp.Value.StatusSchedule.

                                                                             // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                             GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                             Select           (group => group.First()).

                                                                             OrderByDescending(tsv   => tsv.Timestamp).
                                                                             Take             (HistorySize).
                                                                             Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                      tsv.Value.    ToString())))

                                                              )));

        }

        #endregion


        #region ToJSON(this RoamingNetworkStatus,               Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<RoamingNetworkStatus>  RoamingNetworkStatus,
                                     UInt64?                                 Skip  = null,
                                     UInt64?                                 Take  = null)
        {

            #region Initial checks

            if (RoamingNetworkStatus == null || !RoamingNetworkStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<RoamingNetwork_Id, RoamingNetworkStatus>();

            foreach (var status in RoamingNetworkStatus)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region ToJSON(this RoamingNetworkStatusSchedules,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<RoamingNetworkStatusSchedule>  RoamingNetworkStatusSchedules,
                                     UInt64?                                         Skip         = null,
                                     UInt64?                                         Take         = null,
                                     UInt64?                                         HistorySize  = 1)
        {

            #region Initial checks

            if (RoamingNetworkStatusSchedules == null || !RoamingNetworkStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<RoamingNetwork_Id, RoamingNetworkStatusSchedule>();

            foreach (var status in RoamingNetworkStatusSchedules)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].StatusSchedule.Any() &&
                         _FilteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JObject(
                                                                   kvp.Value.StatusSchedule.

                                                                             // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
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
