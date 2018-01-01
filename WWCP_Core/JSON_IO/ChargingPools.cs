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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Aegir;

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

                         Embedded
                             ? null
                             : new JProperty("@context", "https://open.charging.cloud/contexts/wwcp+json/ChargingPool"),

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
                         ChargingPool.HotlinePhoneNumber. ToJSON("hotlinePhoneNumber"),
                         ChargingPool.OpeningTimes.       ToJSON("openingTimes"),


                         ExpandChargingStationIds.Switch(
                             () => new JProperty("chargingStationIds",
                                                 ChargingPool.ChargingStationIds().SafeAny()
                                                     ? new JArray(ChargingPool.ChargingStationIds().
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

        #region ToJSON(this ChargingPools, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingPool> ChargingPools, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingPools?.Any() == true
                       ? new JProperty(JPropertyKey, ChargingPools.ToJSON())
                       : null;

        }

        #endregion


        #region ToJSON(this ChargingPoolAdminStatus,          Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<ChargingPoolAdminStatus>  ChargingPoolAdminStatus,
                                     UInt64                                     Skip  = 0,
                                     UInt64                                     Take  = 0)
        {

            #region Initial checks

            if (ChargingPoolAdminStatus == null || !ChargingPoolAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingPool_Id, ChargingPoolAdminStatus>();

            foreach (var status in ChargingPoolAdminStatus)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take == 0 ? _FilteredStatus.OrderBy(status => status.Key).Skip(Skip)
                                          : _FilteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region ToJSON(this ChargingPoolAdminStatusSchedules, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<ChargingPoolAdminStatusSchedule>  ChargingPoolAdminStatusSchedules,
                                     UInt64                                             Skip         = 0,
                                     UInt64                                             Take         = 0,
                                     UInt64                                             HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingPoolAdminStatusSchedules == null || !ChargingPoolAdminStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingPool_Id, ChargingPoolAdminStatusSchedule>();

            foreach (var status in ChargingPoolAdminStatusSchedules)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].StatusSchedule.Any() &&
                         _FilteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take == 0 ? _FilteredStatus.OrderBy(status => status.Key).Skip(Skip)
                                          : _FilteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)).

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


        #region ToJSON(this ChargingPoolStatus,               Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<ChargingPoolStatus>  ChargingPoolStatus,
                                     UInt64                                Skip  = 0,
                                     UInt64                                Take  = 0)
        {

            #region Initial checks

            if (ChargingPoolStatus == null || !ChargingPoolStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingPool_Id, ChargingPoolStatus>();

            foreach (var status in ChargingPoolStatus)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take == 0 ? _FilteredStatus.OrderBy(status => status.Key).Skip(Skip)
                                          : _FilteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region ToJSON(this ChargingPoolStatusSchedules,      Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<ChargingPoolStatusSchedule>  ChargingPoolStatusSchedules,
                                     UInt64                                        Skip         = 0,
                                     UInt64                                        Take         = 0,
                                     UInt64                                        HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingPoolStatusSchedules == null || !ChargingPoolStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingPool_Id, ChargingPoolStatusSchedule>();

            foreach (var status in ChargingPoolStatusSchedules)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].StatusSchedule.Any() &&
                         _FilteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take == 0 ? _FilteredStatus.OrderBy(status => status.Key).Skip(Skip)
                                          : _FilteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)).

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
