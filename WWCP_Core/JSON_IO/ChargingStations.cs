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
                             ? null
                             : new JProperty("@context", "https://open.charging.cloud/contexts/wwcp+json/ChargingStation"),

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
                         (!Embedded || ChargingStation.HotlinePhoneNumber  != ChargingStation.ChargingPool.HotlinePhoneNumber)  ? ChargingStation.HotlinePhoneNumber. ToJSON("hotlinePhoneNumber")  : null,
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
                                    UInt64?                            Skip                              = 0,
                                    UInt64?                            Take                              = 0,
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

            return ChargingStations?.Any() == true
                       ? new JProperty(JPropertyKey, ChargingStations.ToJSON())
                       : null;

        }

        #endregion


        #region ToJSON(this ChargingStationAdminStatus,          Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<ChargingStationAdminStatus>  ChargingStationAdminStatus,
                                     UInt64?                                       Skip  = 0,
                                     UInt64?                                       Take  = 0)
        {

            #region Initial checks

            if (ChargingStationAdminStatus == null || !ChargingStationAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingStation_Id, ChargingStationAdminStatus>();

            foreach (var status in ChargingStationAdminStatus)
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

        #region ToJSON(this ChargingStationAdminStatusSchedules, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<ChargingStationAdminStatusSchedule>  ChargingStationAdminStatusSchedules,
                                     UInt64                                                Skip         = 0,
                                     UInt64                                                Take         = 0,
                                     UInt64                                                HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingStationAdminStatusSchedules == null || !ChargingStationAdminStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingStation_Id, ChargingStationAdminStatusSchedule>();

            foreach (var status in ChargingStationAdminStatusSchedules)
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


        #region ToJSON(this ChargingStationStatus,               Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<ChargingStationStatus>  ChargingStationStatus,
                                     UInt64?                                  Skip  = 0,
                                     UInt64?                                  Take  = 0)
        {

            #region Initial checks

            if (ChargingStationStatus == null || !ChargingStationStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingStation_Id, ChargingStationStatus>();

            foreach (var status in ChargingStationStatus)
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

        #region ToJSON(this ChargingStationAdminStatusSchedules, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<ChargingStationStatusSchedule>  ChargingStationStatusSchedules,
                                     UInt64                                           Skip         = 0,
                                     UInt64                                           Take         = 0,
                                     UInt64                                           HistorySize  = 1)
        {

            #region Initial checks

            if (ChargingStationStatusSchedules == null || !ChargingStationStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<ChargingStation_Id, ChargingStationStatusSchedule>();

            foreach (var status in ChargingStationStatusSchedules)
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
