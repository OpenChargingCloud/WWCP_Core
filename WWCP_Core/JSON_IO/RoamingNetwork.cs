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
        public static JObject ToJSON(this RoamingNetwork  RoamingNetwork,
                                     Boolean              Embedded                           = false,
                                     InfoStatus           ExpandChargingStationOperatorIds   = InfoStatus.ShowIdOnly,
                                     InfoStatus           ExpandChargingPoolIds              = InfoStatus.ShowIdOnly,
                                     InfoStatus           ExpandChargingStationIds           = InfoStatus.ShowIdOnly,
                                     InfoStatus           ExpandEVSEIds                      = InfoStatus.ShowIdOnly,
                                     InfoStatus           ExpandBrandIds                     = InfoStatus.ShowIdOnly,
                                     InfoStatus           ExpandDataLicenses                 = InfoStatus.ShowIdOnly,

                                     InfoStatus           ExpandEMobilityProviderId          = InfoStatus.ShowIdOnly)

        {

            JObject RN = null;

            try
            {

                RN =

                 RoamingNetwork == null

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

                             ExpandChargingStationOperatorIds.Switch(
                                       () => new JProperty("chargingStationOperatorIds",  new JArray(RoamingNetwork.ChargingStationOperatorIds.Select(id => id.ToString()))),
                                       () => new JProperty("chargingStationOperators",    new JArray(RoamingNetwork.ChargingStationOperators.  ToJSON(Embedded: true,
                                                                                                                                                      ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                                                      ExpandChargingPoolIds:            InfoStatus.Hidden,
                                                                                                                                                      ExpandChargingStationIds:         InfoStatus.Hidden,
                                                                                                                                                      ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                                                                      ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                                                      ExpandDataLicenses:               InfoStatus.Hidden)))),

                             ExpandChargingStationOperatorIds == InfoStatus.Expand
                                 ? null
                                 : ExpandChargingPoolIds.Switch(
                                       () => new JProperty("chargingPoolIds",             new JArray(RoamingNetwork.ChargingPoolIds.           Select(id => id.ToString()))),
                                       () => new JProperty("chargingPools",               new JArray(RoamingNetwork.ChargingPools.             ToJSON(Embedded: true,
                                                                                                                                                      ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                                                      ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                                                                      ExpandChargingStationIds:         InfoStatus.Hidden,
                                                                                                                                                      ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                                                                      ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                                                      ExpandDataLicenses:               InfoStatus.Hidden)))),

                             ExpandChargingPoolIds == InfoStatus.Expand || ExpandChargingStationOperatorIds == InfoStatus.Expand
                                 ? null
                                 : ExpandChargingStationIds.Switch(
                                       () => new JProperty("chargingStationIds",          new JArray(RoamingNetwork.ChargingStationIds.        Select(id => id.ToString()))),
                                       () => new JProperty("chargingStations",            new JArray(RoamingNetwork.ChargingStations.          ToJSON(Embedded: true,
                                                                                                                                                      ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                                                      ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                                                                      ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                                                                      ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                                                                      ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                                                      ExpandDataLicenses:               InfoStatus.Hidden)))),

                             ExpandChargingStationIds == InfoStatus.Expand || ExpandChargingPoolIds == InfoStatus.Expand || ExpandChargingStationOperatorIds == InfoStatus.Expand
                                 ? null
                                 : ExpandEVSEIds.Switch(
                                       () => new JProperty("EVSEIds",                     new JArray(RoamingNetwork.EVSEIds.                   Select(id => id.ToString()))),
                                       () => new JProperty("EVSEs",                       new JArray(RoamingNetwork.EVSEs.                     ToJSON(Embedded: true,
                                                                                                                                                      ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                                                      ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                                                                      ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                                                                      ExpandChargingStationId:          InfoStatus.Hidden,
                                                                                                                                                      ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                                                      ExpandDataLicenses:               InfoStatus.Hidden)))),


                             ExpandEMobilityProviderId.Switch(
                                       () => new JProperty("eMobilityProviderIds",        new JArray(RoamingNetwork.ChargingStationOperatorIds.Select(id => id.ToString()))),
                                       () => new JProperty("eMobilityProviders",          new JArray(RoamingNetwork.eMobilityProviders.        ToJSON(Embedded: true,
                                                                                                                                                      ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                                                      ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                                                      ExpandDataLicenses:               InfoStatus.Hidden))))

                         );


            }
            catch (Exception e)
            {


            }

            return RN;

        }

        #endregion

        #region ToJSON(this RoamingNetworks, Skip = 0, Take = 0, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="Skip">The optional number of roaming networks to skip.</param>
        /// <param name="Take">The optional number of roaming networks to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure.</param>
        public static JArray ToJSON(this IEnumerable<RoamingNetwork>  RoamingNetworks,
                                    UInt64                            Skip                               = 0,
                                    UInt64                            Take                               = 0,
                                    Boolean                           Embedded                           = false,
                                    InfoStatus                        ExpandChargingStationOperatorIds   = InfoStatus.ShowIdOnly,
                                    InfoStatus                        ExpandChargingPoolIds              = InfoStatus.ShowIdOnly,
                                    InfoStatus                        ExpandChargingStationIds           = InfoStatus.ShowIdOnly,
                                    InfoStatus                        ExpandEVSEIds                      = InfoStatus.ShowIdOnly,
                                    InfoStatus                        ExpandBrandIds                     = InfoStatus.ShowIdOnly,
                                    InfoStatus                        ExpandDataLicenses                 = InfoStatus.ShowIdOnly,

                                    InfoStatus                        ExpandEMobilityProviderId          = InfoStatus.ShowIdOnly)


        => RoamingNetworks == null || !RoamingNetworks.Any()

                   ? new JArray()

                   : new JArray(RoamingNetworks.
                                    Where     (roamingnetwork => roamingnetwork != null).
                                    OrderBy   (roamingnetwork => roamingnetwork.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(roamingnetwork => roamingnetwork.ToJSON(Embedded,
                                                                                       ExpandChargingStationOperatorIds,
                                                                                       ExpandChargingPoolIds,
                                                                                       ExpandChargingStationIds,
                                                                                       ExpandEVSEIds,
                                                                                       ExpandBrandIds,
                                                                                       ExpandDataLicenses,
                                                                                       ExpandEMobilityProviderId)));

        #endregion


        #region ToJSON(this RoamingNetworkAdminStatus, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<RoamingNetworkAdminStatusTypes>>  RoamingNetworkAdminStatus,
                                     UInt64                                                         Skip         = 0,
                                     UInt64                                                         Take         = 0,
                                     UInt64                                                         HistorySize  = 1)

        {

            if (RoamingNetworkAdminStatus == null)
                return new JObject();

            try
            {

                var _StatusHistory = Take == 0
                                         ? RoamingNetworkAdminStatus.Skip(Skip)
                                         : RoamingNetworkAdminStatus.Skip(Skip).Take(Take);

                return new JObject(_StatusHistory.
                                       // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                       GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                       Select           (group => group.First()).

                                       OrderByDescending(tsv   => tsv.Timestamp).
                                       Take             (HistorySize).
                                       Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                tsv.Value.    ToString())));

            }
            catch (Exception)
            {
                // e.g. when a Stack behind RoamingNetworkAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this RoamingNetworkAdminStatus, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<RoamingNetwork_Id, IEnumerable<Timestamped<RoamingNetworkAdminStatusTypes>>>>  RoamingNetworkAdminStatus,
                                     UInt64                                                                                                       Skip         = 0,
                                     UInt64                                                                                                       Take         = 0,
                                     UInt64                                                                                                       HistorySize  = 1)

        {

            #region Initial checks

            if (RoamingNetworkAdminStatus == null || !RoamingNetworkAdminStatus.Any())
                return new JObject();

            var _RoamingNetworkAdminStatus = new Dictionary<RoamingNetwork_Id, IEnumerable<Timestamped<RoamingNetworkAdminStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate RoamingNetwork identifications in the enumeration... take the newest one!

            foreach (var networkstatus in Take == 0 ? RoamingNetworkAdminStatus.Skip(Skip)
                                                : RoamingNetworkAdminStatus.Skip(Skip).Take(Take))
            {

                if (!_RoamingNetworkAdminStatus.ContainsKey(networkstatus.Key))
                    _RoamingNetworkAdminStatus.Add(networkstatus.Key, networkstatus.Value);

                else if (networkstatus.Value.FirstOrDefault().Timestamp > _RoamingNetworkAdminStatus[networkstatus.Key].FirstOrDefault().Timestamp)
                    _RoamingNetworkAdminStatus[networkstatus.Key] = networkstatus.Value;

            }

            #endregion

            return _RoamingNetworkAdminStatus.Count == 0

                   ? new JObject()

                   : new JObject(_RoamingNetworkAdminStatus.
                                       SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                    new JObject(statuslist.Value.

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


        #region ToJSON(this RoamingNetworkStatus,      Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<RoamingNetworkStatusTypes>>  RoamingNetworkStatus,
                                     UInt64                                                    Skip         = 0,
                                     UInt64                                                    Take         = 0,
                                     UInt64                                                    HistorySize  = 1)

        {

            if (RoamingNetworkStatus == null)
                return new JObject();

            try
            {

                var _StatusHistory = Take == 0
                                         ? RoamingNetworkStatus.Skip(Skip)
                                         : RoamingNetworkStatus.Skip(Skip).Take(Take);

                return new JObject(_StatusHistory.
                                       // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                       GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                       Select           (group => group.First()).

                                       OrderByDescending(tsv   => tsv.Timestamp).
                                       Take             (HistorySize).
                                       Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                tsv.Value.    ToString())));

            }
            catch (Exception)
            {
                // e.g. when a Stack behind RoamingNetworkStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this RoamingNetworkStatus,      Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<RoamingNetwork_Id, IEnumerable<Timestamped<RoamingNetworkStatusTypes>>>>  RoamingNetworkStatus,
                                     UInt64                                                                                                  Skip         = 0,
                                     UInt64                                                                                                  Take         = 0,
                                     UInt64                                                                                                  HistorySize  = 1)

        {

            #region Initial checks

            if (RoamingNetworkStatus == null || !RoamingNetworkStatus.Any())
                return new JObject();

            var _RoamingNetworkStatus = new Dictionary<RoamingNetwork_Id, IEnumerable<Timestamped<RoamingNetworkStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate RoamingNetwork identifications in the enumeration... take the newest one!

            foreach (var networkstatus in Take == 0 ? RoamingNetworkStatus.Skip(Skip)
                                                : RoamingNetworkStatus.Skip(Skip).Take(Take))
            {

                if (!_RoamingNetworkStatus.ContainsKey(networkstatus.Key))
                    _RoamingNetworkStatus.Add(networkstatus.Key, networkstatus.Value);

                else if (networkstatus.Value.FirstOrDefault().Timestamp > _RoamingNetworkStatus[networkstatus.Key].FirstOrDefault().Timestamp)
                    _RoamingNetworkStatus[networkstatus.Key] = networkstatus.Value;

            }

            #endregion

            return _RoamingNetworkStatus.Count == 0

                   ? new JObject()

                   : new JObject(_RoamingNetworkStatus.
                                       SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                    new JObject(statuslist.Value.

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
