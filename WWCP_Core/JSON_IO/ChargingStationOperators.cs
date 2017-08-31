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

        #region ToJSON(this ChargingStationOperator,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        public static JObject ToJSON(this ChargingStationOperator  ChargingStationOperator,
                                     Boolean                       Embedded                   = false,
                                     InfoStatus                    ExpandRoamingNetworkId     = InfoStatus.ShowIdOnly,
                                     InfoStatus                    ExpandChargingPoolIds      = InfoStatus.ShowIdOnly,
                                     InfoStatus                    ExpandChargingStationIds   = InfoStatus.ShowIdOnly,
                                     InfoStatus                    ExpandEVSEIds              = InfoStatus.ShowIdOnly,
                                     InfoStatus                    ExpandBrandIds             = InfoStatus.ShowIdOnly,
                                     InfoStatus                    ExpandDataLicenses         = InfoStatus.ShowIdOnly)


            => ChargingStationOperator == null

                   ? null

                   : JSONObject.Create(

                         new JProperty("@id",  ChargingStationOperator.Id.ToString()),

                         !Embedded
                             ? new JProperty("@context",  "https://open.charging.cloud/contexts/wwcp+json/ChargingStationOperator")
                             : null,

                         new JProperty("name",  ChargingStationOperator.Name.ToJSON()),

                         ChargingStationOperator.Description.IsNeitherNullNorEmpty()
                             ? ChargingStationOperator.Description.ToJSON("description")
                             : null,

                         ChargingStationOperator.DataSource.ToJSON("DataSource"),

                         ExpandDataLicenses.Switch(
                             () => new JProperty("dataLicenseIds",  new JArray(ChargingStationOperator.DataLicenses.SafeSelect(license => license.Id.ToString()))),
                             () => new JProperty("dataLicenses",    ChargingStationOperator.DataLicenses.ToJSON())),

                         #region Embedded means it is served as a substructure of e.g. a charging station operator

                         Embedded
                             ? null
                             : ExpandRoamingNetworkId.Switch(
                                   () => new JProperty("roamingNetworkId",   ChargingStationOperator.RoamingNetwork.Id. ToString()),
                                   () => new JProperty("roamingNetwork",     ChargingStationOperator.RoamingNetwork.    ToJSON(Embedded:                          true,
                                                                                                                               ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
                                                                                                                               ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                               ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                               ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                               ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                               ExpandDataLicenses:                InfoStatus.Hidden))),

                         #endregion

                         ChargingStationOperator.Address != null
                             ? ChargingStationOperator.Address.ToJSON("address")
                             : null,

                         // API
                         // MainKeys
                         // RobotKeys
                         // Endpoints
                         // DNS SRV

                         ChargingStationOperator.Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          ChargingStationOperator.Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         ChargingStationOperator.Homepage.IsNotNullOrEmpty()
                             ? new JProperty("homepage",            ChargingStationOperator.Homepage)
                             : null,

                         ChargingStationOperator.HotlinePhoneNumber.IsNotNullOrEmpty()
                             ? new JProperty("hotline",             ChargingStationOperator.HotlinePhoneNumber)
                             : null,


                         ExpandChargingPoolIds.Switch(
                                   () => new JProperty("chargingPoolIds",
                                                       new JArray(ChargingStationOperator.ChargingPoolIds.
                                                                                          OrderBy(poolId => poolId).
                                                                                          Select (poolId => poolId.ToString()))),

                                   () => new JProperty("chargingPools",
                                                       new JArray(ChargingStationOperator.ChargingPools.
                                                                                          OrderBy(poolId => poolId).
                                                                                          ToJSON (Embedded:                         true,
                                                                                                  ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                  ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                  ExpandChargingStationIds:         InfoStatus.Hidden,
                                                                                                  ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                  ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                  ExpandDataLicenses:               InfoStatus.Hidden)))),

                         ExpandChargingPoolIds == InfoStatus.Expand
                             ? null
                             : ExpandChargingStationIds.Switch(
                                   () => new JProperty("chargingStationIds",
                                                       new JArray(ChargingStationOperator.ChargingStationIds.
                                                                                          OrderBy(stationid => stationid).
                                                                                          Select (stationid => stationid.ToString()))),

                                   () => new JProperty("chargingStations",
                                                       new JArray(ChargingStationOperator.ChargingStations.
                                                                                          OrderBy(station   => station).
                                                                                          ToJSON (Embedded:                         true,
                                                                                                  ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                  ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                  ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                  ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                  ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                  ExpandDataLicenses:               InfoStatus.Hidden)))),

                         ExpandChargingPoolIds == InfoStatus.Expand || ExpandChargingStationIds == InfoStatus.Expand
                             ? null
                             : ExpandEVSEIds.Switch(
                                   () => new JProperty("EVSEIds",
                                                       new JArray(ChargingStationOperator.EVSEIds.
                                                                                          OrderBy(evseId => evseId).
                                                                                          Select (evseId => evseId.ToString()))),

                                   () => new JProperty("EVSEs",
                                                       new JArray(ChargingStationOperator.EVSEs.
                                                                                          OrderBy(evse   => evse).
                                                                                          ToJSON (Embedded:                         true,
                                                                                                  ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                  ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                  ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                  ExpandChargingStationId:          InfoStatus.Hidden,
                                                                                                  ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                  ExpandDataLicenses:               InfoStatus.Hidden))))

                     );

        #endregion

        #region ToJSON(this ChargingStationOperators, Skip = 0, Take = 0, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="Skip">The optional number of charging station operators to skip.</param>
        /// <param name="Take">The optional number of charging station operators to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        public static JArray ToJSON(this IEnumerable<ChargingStationOperator>  ChargingStationOperators,
                                    UInt64                                     Skip                       = 0,
                                    UInt64                                     Take                       = 0,
                                    Boolean                                    Embedded                   = false,
                                    InfoStatus                                 ExpandRoamingNetworkId     = InfoStatus.ShowIdOnly,
                                    InfoStatus                                 ExpandChargingPoolIds      = InfoStatus.ShowIdOnly,
                                    InfoStatus                                 ExpandChargingStationIds   = InfoStatus.ShowIdOnly,
                                    InfoStatus                                 ExpandEVSEIds              = InfoStatus.ShowIdOnly,
                                    InfoStatus                                 ExpandBrandIds             = InfoStatus.ShowIdOnly,
                                    InfoStatus                                 ExpandDataLicenses         = InfoStatus.ShowIdOnly)


            => ChargingStationOperators == null || !ChargingStationOperators.Any()

                   ? new JArray()

                   : new JArray(ChargingStationOperators.
                                    Where         (cso => cso != null).
                                    OrderBy       (cso => cso.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (cso => cso.ToJSON(Embedded,
                                                                     ExpandRoamingNetworkId,
                                                                     ExpandChargingPoolIds,
                                                                     ExpandChargingStationIds,
                                                                     ExpandEVSEIds,
                                                                     ExpandBrandIds,
                                                                     ExpandDataLicenses)));

        #endregion


        #region ToJSON(this ChargingStationOperatorAdminStatus, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorAdminStatusTypes>>>>  ChargingStationOperatorAdminStatus,
                                     UInt64                                                                                                                         Skip         = 0,
                                     UInt64                                                                                                                         Take         = 0,
                                     UInt64                                                                                                                         HistorySize  = 1)

        {

            #region Initial checks

            if (ChargingStationOperatorAdminStatus == null || !ChargingStationOperatorAdminStatus.Any())
                return new JObject();

            var _ChargingStationOperatorAdminStatus = new Dictionary<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorAdminStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate ChargingStationOperator identifications in the enumeration... take the newest one!

            foreach (var csostatus in Take == 0 ? ChargingStationOperatorAdminStatus.Skip(Skip)
                                                : ChargingStationOperatorAdminStatus.Skip(Skip).Take(Take))
            {

                if (!_ChargingStationOperatorAdminStatus.ContainsKey(csostatus.Key))
                    _ChargingStationOperatorAdminStatus.Add(csostatus.Key, csostatus.Value);

                else if (csostatus.Value.FirstOrDefault().Timestamp > _ChargingStationOperatorAdminStatus[csostatus.Key].FirstOrDefault().Timestamp)
                    _ChargingStationOperatorAdminStatus[csostatus.Key] = csostatus.Value;

            }

            #endregion

            return _ChargingStationOperatorAdminStatus.Count == 0

                   ? new JObject()

                   : new JObject(_ChargingStationOperatorAdminStatus.
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


        #region ToJSON(this ChargingStationOperatorStatus,      Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorStatusTypes>>>>  ChargingStationOperatorStatus,
                                     UInt64                                                                                                                    Skip         = 0,
                                     UInt64                                                                                                                    Take         = 0,
                                     UInt64                                                                                                                    HistorySize  = 1)

        {

            #region Initial checks

            if (ChargingStationOperatorStatus == null || !ChargingStationOperatorStatus.Any())
                return new JObject();

            var _ChargingStationOperatorStatus = new Dictionary<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate ChargingStationOperator identifications in the enumeration... take the newest one!

            foreach (var csostatus in Take == 0 ? ChargingStationOperatorStatus.Skip(Skip)
                                                 : ChargingStationOperatorStatus.Skip(Skip).Take(Take))
            {

                if (!_ChargingStationOperatorStatus.ContainsKey(csostatus.Key))
                    _ChargingStationOperatorStatus.Add(csostatus.Key, csostatus.Value);

                else if (csostatus.Value.FirstOrDefault().Timestamp > _ChargingStationOperatorStatus[csostatus.Key].FirstOrDefault().Timestamp)
                    _ChargingStationOperatorStatus[csostatus.Key] = csostatus.Value;

            }

            #endregion

            return _ChargingStationOperatorStatus.Count == 0

                   ? new JObject()

                   : new JObject(_ChargingStationOperatorStatus.
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
