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

        #region ToJSON(this ChargingPool,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JObject ToJSON(this ChargingPool  ChargingPool,
                                     Boolean            Embedded                          = false,
                                     InfoStatus         ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                     InfoStatus         ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                     InfoStatus         ExpandChargingStationIds          = InfoStatus.Expand,
                                     InfoStatus         ExpandEVSEIds                     = InfoStatus.Hidden,
                                     InfoStatus         ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                     InfoStatus         ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingPool == null

                   ? null

                   : JSONObject.Create(

                         ChargingPool.Id.ToJSON("@id"),

                         !Embedded
                             ? new JProperty("@context",  "https://open.charging.cloud/contexts/wwcp+json/ChargingPool")
                             : null,

                         ChargingPool.Name.       IsNeitherNullNorEmpty()
                             ? ChargingPool.Name.       ToJSON("name")
                             : null,

                         ChargingPool.Description.IsNeitherNullNorEmpty()
                             ? ChargingPool.Description.ToJSON("description")
                             : null,

                         ChargingPool.Brand != null
                             ? ExpandBrandIds.Switch(
                                   () => new JProperty("brandId",  ChargingPool.Brand.Id.ToString()),
                                   () => new JProperty("brand",    ChargingPool.Brand.   ToJSON()))
                             : null,

                         (!Embedded || ChargingPool.DataSource != ChargingPool.Operator.DataSource)
                             ? ChargingPool.DataSource.ToJSON("DataSource")
                             : null,

                         ExpandDataLicenses.Switch(
                             () => new JProperty("DataLicenseIds",  new JArray(ChargingPool.DataLicenses.SafeSelect(license => license.Id.ToString()))),
                             () => new JProperty("DataLicenses",    ChargingPool.DataLicenses.ToJSON())),


                         #region Embedded means it is served as a substructure of e.g. a charging station operator

                         Embedded
                             ? null
                             : ExpandRoamingNetworkId.Switch(
                                   () => new JProperty("roamingNetworkId",                  ChargingPool.RoamingNetwork.Id. ToString()),
                                   () => new JProperty("roamingNetwork",                    ChargingPool.RoamingNetwork.    ToJSON(Embedded:                          true,
                                                                                                                                   ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
                                                                                                                                   ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                                   ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                                   ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                                   ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                                   ExpandDataLicenses:                InfoStatus.Hidden))),

                         Embedded
                             ? null
                             : ExpandChargingStationOperatorId.Switch(
                                   () => new JProperty("chargingStationOperatorperatorId",  ChargingPool.Operator.Id.       ToString()),
                                   () => new JProperty("chargingStationOperatorperator",    ChargingPool.Operator.          ToJSON(Embedded:                          true,
                                                                                                                                   ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                                   ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                                   ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                                   ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                                   ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                                   ExpandDataLicenses:                InfoStatus.Hidden))),

                         #endregion

                         ChargingPool.GeoLocation.        ToJSON("geoLocation"),
                         ChargingPool.Address.            ToJSON("address"),
                         ChargingPool.AuthenticationModes.ToJSON("authenticationModes"),
                         ChargingPool.OpeningTimes.       ToJSON("openingTimes"),


                         ExpandChargingStationIds.Switch(
                             () => new JProperty("chargingStationIds",
                                                 ChargingPool.ChargingStationIds.SafeAny()
                                                     ? new JArray(ChargingPool.ChargingStationIds.
                                                                               OrderBy(stationId => stationId).
                                                                               Select (stationId => stationId.ToString()))
                                                     : new JArray()),

                             () => new JProperty("chargingStations",
                                                 ChargingPool.ChargingStations.SafeAny()
                                                     ? new JArray(ChargingPool.ChargingStations.
                                                                               OrderBy(station   => station.Id).
                                                                               ToJSON (Embedded:      true,
                                                                                       ExpandEVSEIds: InfoStatus.Expand))
                                                     : new JArray())),


                         ExpandChargingStationIds == InfoStatus.Expand
                             ? null
                             : ExpandEVSEIds.Switch(
                                   () => new JProperty("EVSEIds",
                                                       ChargingPool.EVSEIds.SafeAny()
                                                           ? new JArray(ChargingPool.EVSEIds.
                                                                                     OrderBy(evseId => evseId).
                                                                                     Select (evseId => evseId.ToString()))
                                                           : new JArray()),

                                   () => new JProperty("EVSEs",
                                                       ChargingPool.EVSEs.SafeAny()
                                                           ? new JArray(ChargingPool.EVSEs.
                                                                                     OrderBy(evse => evse.Id).
                                                                                     Select (evse => evse.ToJSON(Embedded: true)))
                                                           : new JArray()))

                     );

        #endregion

        #region ToJSON(this ChargingPools, Skip = 0, Take = 0, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="Skip">The optional number of charging pools to skip.</param>
        /// <param name="Take">The optional number of charging pools to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JArray ToJSON(this IEnumerable<ChargingPool>  ChargingPools,
                                    UInt64                          Skip                              = 0,
                                    UInt64                          Take                              = 0,
                                    Boolean                         Embedded                          = false,
                                    InfoStatus                      ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                      ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                      ExpandChargingStationIds          = InfoStatus.Expand,
                                    InfoStatus                      ExpandEVSEIds                     = InfoStatus.Hidden,
                                    InfoStatus                      ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                      ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingPools == null || !ChargingPools.Any()

                   ? new JArray()

                   : new JArray(ChargingPools.
                                    Where     (pool => pool != null).
                                    OrderBy   (pool => pool.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(pool => pool.ToJSON(Embedded,
                                                                   ExpandRoamingNetworkId,
                                                                   ExpandChargingStationOperatorId,
                                                                   ExpandChargingStationIds,
                                                                   ExpandEVSEIds,
                                                                   ExpandBrandIds,
                                                                   ExpandDataLicenses)));


        #endregion


        #region ToJSON(this ChargingPoolAdminStatus, Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>>>  ChargingPoolAdminStatus,
                                     UInt64                                                                                                   Skip         = 0,
                                     UInt64                                                                                                   Take         = 0,
                                     UInt64                                                                                                   HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingPoolAdminStatus == null || !ChargingPoolAdminStatus.Any())
                return new JObject();

            var _ChargingPoolAdminStatus = new Dictionary<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate charging pool identifications in the enumeration... take the newest one!

            foreach (var poolstatus in Take == 0 ? _ChargingPoolAdminStatus.Skip(Skip)
                                                 : _ChargingPoolAdminStatus.Skip(Skip).Take(Take))
            {

                if (!_ChargingPoolAdminStatus.ContainsKey(poolstatus.Key))
                    _ChargingPoolAdminStatus.Add(poolstatus.Key, poolstatus.Value);

                else if (poolstatus.Value.FirstOrDefault().Timestamp > _ChargingPoolAdminStatus[poolstatus.Key].FirstOrDefault().Timestamp)
                    _ChargingPoolAdminStatus[poolstatus.Key] = poolstatus.Value;

            }

            #endregion

            return _ChargingPoolAdminStatus.Count == 0

                   ? new JObject()

                   : new JObject(_ChargingPoolAdminStatus.
                                   SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                new JObject(statuslist.Value.

                                                                            // Will filter multiple charging pool status having the exact same ISO 8601 timestamp!
                                                                            GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                            Select           (group => group.First()).

                                                                            OrderByDescending(tsv   => tsv.Timestamp).
                                                                            Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                     tsv.Value.    ToString())))

                                                            )));

        }

        #endregion


        #region ToJSON(this ChargingPoolStatus, Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusTypes>>>>  ChargingPoolStatus,
                                     UInt64                                                                                              Skip         = 0,
                                     UInt64                                                                                              Take         = 0,
                                     UInt64                                                                                              HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingPoolStatus == null || !ChargingPoolStatus.Any())
                return new JObject();

            var _ChargingPoolStatus = new Dictionary<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate charging pool identifications in the enumeration... take the newest one!

            foreach (var poolstatus in Take == 0 ? _ChargingPoolStatus.Skip(Skip)
                                                 : _ChargingPoolStatus.Skip(Skip).Take(Take))
            {

                if (!_ChargingPoolStatus.ContainsKey(poolstatus.Key))
                    _ChargingPoolStatus.Add(poolstatus.Key, poolstatus.Value);

                else if (poolstatus.Value.FirstOrDefault().Timestamp > _ChargingPoolStatus[poolstatus.Key].FirstOrDefault().Timestamp)
                    _ChargingPoolStatus[poolstatus.Key] = poolstatus.Value;

            }

            #endregion

            return _ChargingPoolStatus.Count == 0

                   ? new JObject()

                   : new JObject(_ChargingPoolStatus.
                                   SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                new JObject(statuslist.Value.

                                                                            // Will filter multiple charging pool status having the exact same ISO 8601 timestamp!
                                                                            GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                            Select           (group => group.First()).

                                                                            OrderByDescending(tsv   => tsv.Timestamp).
                                                                            Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                     tsv.Value.    ToString())))

                                                            )));

        }

        #endregion

    }

}
