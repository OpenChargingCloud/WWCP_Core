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

        #region ToJSON(this ChargingStationOperator, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        public static JObject ToJSON(this ChargingStationOperator  ChargingStationOperator,
                                     Boolean                       Embedded                        = false,
                                     Boolean                       ExpandChargingRoamingNetworkId  = false,
                                     Boolean                       ExpandChargingPoolIds           = false,
                                     Boolean                       ExpandChargingStationIds        = false,
                                     Boolean                       ExpandEVSEIds                   = false)

            => ChargingStationOperator != null
                   ? JSONObject.Create(

                         new JProperty("id",                        ChargingStationOperator.Id.ToString()),

                         Embedded
                             ? null
                             : ExpandChargingRoamingNetworkId
                                   ? new JProperty("roamingNetwork",      ChargingStationOperator.RoamingNetwork.ToJSON())
                                   : new JProperty("roamingNetworkId",    ChargingStationOperator.RoamingNetwork.Id.ToString()),

                         new JProperty("name",                  ChargingStationOperator.Name.       ToJSON()),
                         new JProperty("description",           ChargingStationOperator.Description.ToJSON()),

                         // Address
                         // LogoURI
                         // API - RobotKeys, Endpoints, DNS SRV
                         // MainKeys

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

                         ChargingStationOperator.DataLicenses.Any()
                             ? new JProperty("dataLicenses",        new JArray(ChargingStationOperator.DataLicenses.Select(license => license.ToJSON())))
                             : null,

                         new JProperty("chargingPools",         ExpandChargingPoolIds
                                                                    ? new JArray(ChargingStationOperator.ChargingPools.     ToJSON(Embedded: true))
                                                                    : new JArray(ChargingStationOperator.ChargingPoolIds.   Select(id => id.ToString()))),

                         new JProperty("chargingStations",      ExpandChargingStationIds
                                                                    ? new JArray(ChargingStationOperator.ChargingStations.  ToJSON(Embedded: true))
                                                                    : new JArray(ChargingStationOperator.ChargingStationIds.Select(id => id.ToString()))),

                         new JProperty("evses",                 ExpandEVSEIds
                                                                    ? new JArray(ChargingStationOperator.EVSEs.             ToJSON(Embedded: true))
                                                                    : new JArray(ChargingStationOperator.EVSEIds.           Select(id => id.ToString())))

                     )
                   : null;

        #endregion

        #region ToJSON(this ChargingStationOperator, JPropertyKey)

        public static JProperty ToJSON(this ChargingStationOperator ChargingStationOperator, String JPropertyKey)

            => ChargingStationOperator != null
                   ? new JProperty(JPropertyKey, ChargingStationOperator.ToJSON())
                   : null;

        #endregion

        #region ToJSON(this ChargingStationOperators, Skip = 0, Take = 0, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        /// <summary>
        /// Return a JSON representation for the given enumeration of Charging Station Operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of Charging Station Operators.</param>
        /// <param name="Skip">The optional number of Charging Station Operators to skip.</param>
        /// <param name="Take">The optional number of Charging Station Operators to return.</param>
        public static JArray ToJSON(this IEnumerable<ChargingStationOperator>  ChargingStationOperators,
                                    UInt64                                     Skip                            = 0,
                                    UInt64                                     Take                            = 0,
                                    Boolean                                    Embedded                        = false,
                                    Boolean                                    ExpandChargingRoamingNetworkId  = false,
                                    Boolean                                    ExpandChargingPoolIds           = false,
                                    Boolean                                    ExpandChargingStationIds        = false,
                                    Boolean                                    ExpandEVSEIds                   = false)
        {

            #region Initial checks

            if (ChargingStationOperators == null)
                return new JArray();

            #endregion

            return new JArray(ChargingStationOperators.
                                  Where     (cso => cso != null).
                                  OrderBy   (cso => cso.Id).
                                  SkipTakeFilter(Skip, Take).
                                  SafeSelect(cso => cso.ToJSON(Embedded,
                                                               ExpandChargingRoamingNetworkId,
                                                               ExpandChargingPoolIds,
                                                               ExpandChargingStationIds,
                                                               ExpandEVSEIds)));

        }

        #endregion

        #region ToJSON(this ChargingStationOperators, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingStationOperator> ChargingStationOperators, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingStationOperators != null
                       ? new JProperty(JPropertyKey, ChargingStationOperators.ToJSON())
                       : null;

        }

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
