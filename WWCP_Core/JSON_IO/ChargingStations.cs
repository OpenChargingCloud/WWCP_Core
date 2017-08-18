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

        #region ToJSON(this ChargingStation,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static JObject ToJSON(this ChargingStation  ChargingStation,
                                     Boolean               Embedded                          = false,
                                     InfoStatus            ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                     InfoStatus            ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                     InfoStatus            ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                     InfoStatus            ExpandEVSEIds                     = InfoStatus.Expand,
                                     InfoStatus            ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                     InfoStatus            ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingStation == null

                   ? null

                   : JSONObject.Create(

                         ChargingStation.Id.ToJSON("@id"),

                         Embedded
                             ? new JProperty("@context",  "https://open.charging.cloud/contexts/wwcp+json/ChargingStation")
                             : null,

                         ChargingStation.Name.       IsNeitherNullNorEmpty()
                             ? ChargingStation.Name.       ToJSON("name")
                             : null,

                         ChargingStation.Description.IsNeitherNullNorEmpty()
                             ? ChargingStation.Description.ToJSON("description")
                             : null,

                         ChargingStation.Brand != null
                             ? ExpandBrandIds.Switch(
                                   () => new JProperty("brandId",  ChargingStation.Brand.Id.ToString()),
                                   () => new JProperty("brand",    ChargingStation.Brand.   ToJSON()))
                             : null,

                         (!Embedded || ChargingStation.DataSource != ChargingStation.ChargingPool.DataSource)
                             ? ChargingStation.DataSource.ToJSON("dataSource")
                             : null,

                         (!Embedded || ChargingStation.DataLicenses != ChargingStation.ChargingPool.DataLicenses)
                             ? ExpandDataLicenses.Switch(
                                 () => new JProperty("dataLicenseIds",  new JArray(ChargingStation.DataLicenses.SafeSelect(license => license.Id.ToString()))),
                                 () => new JProperty("dataLicenses",    ChargingStation.DataLicenses.ToJSON()))
                             : null,

                         #region Embedded means it is served as a substructure of e.g. a charging station operator

                         Embedded
                             ? null
                             : ExpandRoamingNetworkId.Switch(
                                   () => new JProperty("roamingNetworkId",           ChargingStation.RoamingNetwork.Id. ToString()),
                                   () => new JProperty("roamingNetwork",             ChargingStation.RoamingNetwork.    ToJSON(Embedded:                          true,
                                                                                                                               ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
                                                                                                                               ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                               ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                               ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                               ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                               ExpandDataLicenses:                InfoStatus.Hidden))),

                         Embedded
                             ? null
                             : ExpandChargingStationOperatorId.Switch(
                                   () => new JProperty("chargingStationOperatorId",  ChargingStation.Operator.Id.       ToString()),
                                   () => new JProperty("chargingStationOperator",    ChargingStation.Operator.          ToJSON(Embedded:                          true,
                                                                                                                               ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                               ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                               ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                               ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                               ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                               ExpandDataLicenses:                InfoStatus.Hidden))),

                         Embedded
                             ? null
                             : ExpandChargingPoolId.Switch(
                                   () => new JProperty("chargingPoolId",             ChargingStation.ChargingPool.Id.   ToString()),
                                   () => new JProperty("chargingPool",               ChargingStation.ChargingPool.      ToJSON(Embedded:                          true,
                                                                                                                               ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                               ExpandChargingStationOperatorId:   InfoStatus.Hidden,
                                                                                                                               ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                               ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                               ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                               ExpandDataLicenses:                InfoStatus.Hidden))),

                         #endregion

                         (!Embedded || ChargingStation.GeoLocation         != ChargingStation.ChargingPool.GeoLocation)         ? ChargingStation.GeoLocation.Value.  ToJSON("geoLocation")         : null,
                         (!Embedded || ChargingStation.Address             != ChargingStation.ChargingPool.Address)             ? ChargingStation.Address.            ToJSON("address")             : null,
                         (!Embedded || ChargingStation.AuthenticationModes != ChargingStation.ChargingPool.AuthenticationModes) ? ChargingStation.AuthenticationModes.ToJSON("authenticationModes") : null,
                         (!Embedded || ChargingStation.OpeningTimes        != ChargingStation.ChargingPool.OpeningTimes)        ? ChargingStation.OpeningTimes.       ToJSON("openingTimes")        : null,

                         ExpandEVSEIds.Switch(
                             () => new JProperty("EVSEIds",
                                                 ChargingStation.EVSEIds.SafeAny()
                                                     ? new JArray(ChargingStation.EVSEIds.
                                                                                  OrderBy(evseid => evseid).
                                                                                  Select (evseid => evseid.ToString()))
                                                     : null),

                             () => new JProperty("EVSEs",
                                                 ChargingStation.EVSEs.SafeAny()
                                                     ? new JArray(ChargingStation.EVSEs.
                                                                                  OrderBy(evse   => evse.Id).
                                                                                  Select (evse   => evse.  ToJSON(Embedded: true)))
                                                     : null))

                        );

        #endregion

        #region ToJSON(this ChargingStations, Skip = 0, Take = 0, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="Skip">The optional number of charging stations to skip.</param>
        /// <param name="Take">The optional number of charging stations to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static JArray ToJSON(this IEnumerable<ChargingStation>  ChargingStations,
                                    UInt64                             Skip                              = 0,
                                    UInt64                             Take                              = 0,
                                    Boolean                            Embedded                          = false,
                                    InfoStatus                         ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandEVSEIds                     = InfoStatus.Expand,
                                    InfoStatus                         ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingStations != null && ChargingStations.Any()

                   ? new JArray(ChargingStations.
                                    Where     (station => station != null).
                                    OrderBy   (station => station.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(station => station.ToJSON(Embedded,
                                                                         ExpandRoamingNetworkId,
                                                                         ExpandChargingStationOperatorId,
                                                                         ExpandChargingPoolId,
                                                                         ExpandEVSEIds,
                                                                         ExpandBrandIds,
                                                                         ExpandDataLicenses)))

                   : null;

        #endregion

        #region ToJSON(this ChargingStations, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingStation> ChargingStations, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingStations != null && ChargingStations.Any()
                       ? new JProperty(JPropertyKey, ChargingStations.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this ChargingStationAdminStatus, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>>>  ChargingStationAdminStatus,
                                     UInt64                                                                                                         Skip         = 0,
                                     UInt64                                                                                                         Take         = 0,
                                     UInt64                                                                                                         HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingStationAdminStatus == null || !ChargingStationAdminStatus.Any())
                return new JObject();

            var _ChargingStationAdminStatus = new Dictionary<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            foreach (var stationstatus in Take == 0 ? _ChargingStationAdminStatus.Skip(Skip)
                                                    : _ChargingStationAdminStatus.Skip(Skip).Take(Take))
            {

                if (!_ChargingStationAdminStatus.ContainsKey(stationstatus.Key))
                    _ChargingStationAdminStatus.Add(stationstatus.Key, stationstatus.Value);

                else if (stationstatus.Value.FirstOrDefault().Timestamp > _ChargingStationAdminStatus[stationstatus.Key].FirstOrDefault().Timestamp)
                    _ChargingStationAdminStatus[stationstatus.Key] = stationstatus.Value;

            }

            #endregion

            return _ChargingStationAdminStatus.Count == 0

                   ? new JObject()

                   : new JObject(_ChargingStationAdminStatus.
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

        #region ToJSON(this ChargingStationStatus, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusTypes>>>>  ChargingStationStatus,
                                     UInt64                                                                                                   Skip         = 0,
                                     UInt64                                                                                                   Take         = 0,
                                     UInt64                                                                                                   HistorySize  = 1)

        {

            #region Initial checks

            if (ChargingStationStatus == null || !ChargingStationStatus.Any())
                return new JObject();

            var _ChargingStationStatus = new Dictionary<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            foreach (var stationstatus in Take == 0 ? _ChargingStationStatus.Skip(Skip)
                                                    : _ChargingStationStatus.Skip(Skip).Take(Take))
            {

                if (!_ChargingStationStatus.ContainsKey(stationstatus.Key))
                    _ChargingStationStatus.Add(stationstatus.Key, stationstatus.Value);

                else if (stationstatus.Value.FirstOrDefault().Timestamp > _ChargingStationStatus[stationstatus.Key].FirstOrDefault().Timestamp)
                    _ChargingStationStatus[stationstatus.Key] = stationstatus.Value;

            }

            #endregion

            return _ChargingStationStatus.Count == 0

                   ? new JObject()

                   : new JObject(_ChargingStationStatus.
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
