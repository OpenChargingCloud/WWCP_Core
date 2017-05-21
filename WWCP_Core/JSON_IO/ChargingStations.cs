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

        #region ToJSON(this ChargingStation, Embedded = false, ExpandEVSEIds = true, ExpandOperatorId = false, ExpandBrandId = false)

        public static JObject ToJSON(this ChargingStation  ChargingStation,
                                     Boolean               Embedded          = false,
                                     Boolean               ExpandEVSEIds     = true,
                                     Boolean               ExpandOperatorId  = false,
                                     Boolean               ExpandBrandId     = false)
        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            // Embedded means it is served as a substructure of e.g. a ChargingPool
            if (Embedded)
                return JSONObject.Create(
                           ChargingStation.Id.         ToJSON("Id"),
                           ChargingStation.Name.       ToJSON("Name"),
                           ChargingStation.Description.ToJSON("Description"),
                           ChargingStation.Brand != null
                               ? ChargingStation.Brand.ToJSON("Brand")
                               : null,
                           ChargingStation.GeoLocation         != ChargingStation.ChargingPool.GeoLocation         ? ChargingStation.GeoLocation.Value.  ToJSON("GeoLocation")         : null,
                           ChargingStation.Address             != ChargingStation.ChargingPool.Address             ? ChargingStation.Address.            ToJSON("Address")             : null,
                           ChargingStation.AuthenticationModes != ChargingStation.ChargingPool.AuthenticationModes ? ChargingStation.AuthenticationModes.ToJSON("AuthenticationModes") : null,
                           ChargingStation.OpeningTimes        != ChargingStation.ChargingPool.OpeningTimes        ? ChargingStation.OpeningTimes.       ToJSON("OpeningTimes")        : null,

                           ExpandEVSEIds
                               ? new JProperty("EVSEs",             new JArray(ChargingStation.EVSEs.  OrderBy(evse   => evse.Id).Select(evse   => evse.  ToJSON(Embedded: true))))
                               : new JProperty("EVSEIds",           new JArray(ChargingStation.EVSEIds.OrderBy(evseid => evseid). Select(evseid => evseid.ToString())))
                          );

            else
                return JSONObject.Create(
                           ChargingStation.Id.ToJSON("Id"),
                           new JProperty("ChargingPoolId",          ChargingStation.ChargingPool.Id.ToString()),

                           ExpandOperatorId
                               ? new JProperty("Operator",          ChargingStation.ChargingPool.Operator.ToJSON())
                               : new JProperty("OperatorId",        ChargingStation.ChargingPool.Operator.Id.ToString()),

                           ChargingStation.Brand != null
                               ? ExpandBrandId
                                     ? ChargingStation.Brand.       ToJSON("Brand")
                                     : new JProperty("BrandId",     ChargingStation.Brand.Id.ToString())
                               : null,

                           ChargingStation.Name.                    ToJSON("Name"),
                           ChargingStation.Description.             ToJSON("Description"),
                           ChargingStation.GeoLocation.Value.       ToJSON("GeoLocation"),
                           ChargingStation.Address.                 ToJSON("Address"),
                           ChargingStation.AuthenticationModes.     ToJSON("AuthenticationModes"),
                           ChargingStation.OpeningTimes.            ToJSON("OpeningTimes"),

                           ExpandEVSEIds
                               ? new JProperty("EVSEs",             new JArray(ChargingStation.EVSEs.  OrderBy(evse   => evse.Id).Select(evse   => evse.  ToJSON(Embedded: true))))
                               : new JProperty("EVSEIds",           new JArray(ChargingStation.EVSEIds.OrderBy(evseid => evseid). Select(evseid => evseid.ToString())))
                          );

        }

        #endregion

        #region ToJSON(this ChargingStation, JPropertyKey)

        public static JProperty ToJSON(this ChargingStation ChargingStation, String JPropertyKey)
        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The given json property key must not be null or empty!");

            #endregion

            return new JProperty(JPropertyKey,
                                 ChargingStation.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargingStations, Skip = 0, Take = 0, Embedded = false, ExpandEVSEIds = true, ExpandOperatorIds = false, ExpandBrandIds = false)

        public static JArray ToJSON(this IEnumerable<ChargingStation>  ChargingStations,
                                    UInt64                             Skip               = 0,
                                    UInt64                             Take               = 0,
                                    Boolean                            Embedded           = false,
                                    Boolean                            ExpandEVSEIds      = true,
                                    Boolean                            ExpandOperatorIds  = false,
                                    Boolean                            ExpandBrandIds     = false)
        {

            #region Initial checks

            if (ChargingStations == null)
                return new JArray();

            #endregion

            return new JArray(ChargingStations.
                                  Where     (station => station != null).
                                  OrderBy   (station => station.Id).
                                  SkipTakeFilter(Skip, Take).
                                  SafeSelect(station => station.ToJSON(Embedded,
                                                                       ExpandEVSEIds,
                                                                       ExpandOperatorIds,
                                                                       ExpandBrandIds)));

        }

        #endregion

        #region ToJSON(this ChargingStations, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingStation> ChargingStations, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingStations != null
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

            if (ChargingStationAdminStatus == null)
                return new JObject();

            var _ChargingStationAdminStatus = Take == 0
                                                  ? ChargingStationAdminStatus.Skip(Skip)
                                                  : ChargingStationAdminStatus.Skip(Skip).Take(Take);

            return new JObject(_ChargingStationAdminStatus.
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

            if (ChargingStationStatus == null)
                return new JObject();

            var _ChargingStationStatus = Take == 0
                                             ? ChargingStationStatus.Skip(Skip)
                                             : ChargingStationStatus.Skip(Skip).Take(Take);

            return new JObject(_ChargingStationStatus.
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
