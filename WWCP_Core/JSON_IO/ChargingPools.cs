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

        #region ToJSON(this ChargingPool, Embedded = false, ExpandChargingStationIds = true, ExpandOperatorId = false, ExpandBrandId = false)

        public static JObject ToJSON(this ChargingPool  ChargingPool,
                                     Boolean            Embedded                  = false,
                                     Boolean            ExpandChargingStationIds  = true,
                                     Boolean            ExpandOperatorId          = false,
                                     Boolean            ExpandBrandId             = false)

            => ChargingPool == null
                   ? null
                   : Embedded

                         // Embedded means it is served as a substructure of e.g. a charging station operator
                         ? JSONObject.Create(

                               ChargingPool.Id.         ToJSON("Id"),
                               ChargingPool.Name.       ToJSON("Name"),
                               ChargingPool.Description.ToJSON("Description"),

                               ExpandOperatorId
                                       ? new JProperty("Operator",    ChargingPool.Operator.ToJSON())
                                       : new JProperty("OperatorId",  ChargingPool.Operator.Id.ToString()),

                               ChargingPool.Brand != null
                                   ? ExpandBrandId
                                         ? ChargingPool.Brand.       ToJSON("Brand")
                                         : new JProperty("BrandId",  ChargingPool.Brand.Id.ToString())
                                   : null,

                               ChargingPool.GeoLocation.        ToJSON("GeoLocation"),
                               ChargingPool.Address.            ToJSON("Address"),
                               ChargingPool.AuthenticationModes.ToJSON("AuthenticationModes"),
                               ChargingPool.OpeningTimes.       ToJSON("OpeningTimes"),

                               ExpandChargingStationIds

                                   ? new JProperty("ChargingStations",
                                                   ChargingPool.ChargingStations.SafeAny()
                                                       ? new JArray(ChargingPool.ChargingStations.
                                                                                 OrderBy(station   => station.Id).
                                                                                 Select (station   => station.  ToJSON(Embedded:      true,
                                                                                                                       ExpandEVSEIds: true)))
                                                       : new JArray())

                                   : new JProperty("ChargingStationIds",
                                                   ChargingPool.ChargingStationIds.SafeAny()
                                                       ? new JArray(ChargingPool.ChargingStationIds.
                                                                                 OrderBy(stationId => stationId ).
                                                                                 Select (stationId => stationId.ToString()))
                                                       : new JArray())

                           )


                         : JSONObject.Create(

                               new JProperty("@context", "https://open.charging.cloud/contexts/ChargingPool"),

                               ChargingPool.Id.         ToJSON("Id"),
                               ChargingPool.Name.       ToJSON("Name"),
                               ChargingPool.Description.ToJSON("Description"),

                               ExpandOperatorId
                                       ? new JProperty("Operator",    ChargingPool.Operator.ToJSON())
                                       : new JProperty("OperatorId",  ChargingPool.Operator.Id.ToString()),

                               ChargingPool.Brand != null
                                   ? ExpandBrandId
                                         ? ChargingPool.Brand.       ToJSON("Brand")
                                         : new JProperty("BrandId",  ChargingPool.Brand.Id.ToString())
                                   : null,

                               ChargingPool.GeoLocation.        ToJSON("GeoLocation"),
                               ChargingPool.Address.            ToJSON("Address"),
                               ChargingPool.AuthenticationModes.ToJSON("AuthenticationModes"),
                               ChargingPool.OpeningTimes.       ToJSON("OpeningTimes"),

                               ExpandChargingStationIds

                                   ? new JProperty("ChargingStations",
                                                   ChargingPool.ChargingStations.SafeAny()
                                                       ? new JArray(ChargingPool.ChargingStations.
                                                                                 OrderBy(station   => station.Id).
                                                                                 Select (station   => station.  ToJSON(Embedded:      true,
                                                                                                                       ExpandEVSEIds: true)))
                                                       : new JArray())

                                   : new JProperty("ChargingStationIds",
                                                   ChargingPool.ChargingStationIds.SafeAny()
                                                       ? new JArray(ChargingPool.ChargingStationIds.
                                                                                 OrderBy(stationId => stationId ).
                                                                                 Select (stationId => stationId.ToString()))
                                                       : new JArray())

                           );

        #endregion

        #region ToJSON(this ChargingPool, JPropertyKey)

        public static JObject ToJSON(this ChargingPool ChargingPool, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey),  "The given json property key must not be null or empty!");

            #endregion

            return ChargingPool != null
                       ? new JObject(JPropertyKey,
                                     ChargingPool.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this ChargingPools, Skip = 0, Take = 0, Embedded = false, ExpandChargingStationIds = true, ExpandOperatorIds = false, ExpandBrandIds = false)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="Skip">The optional number of charging pools to skip.</param>
        /// <param name="Take">The optional number of charging pools to return.</param>
        /// <param name="Embedded"></param>
        /// <param name="ExpandChargingStationIds"></param>
        /// <param name="ExpandOperatorIds"></param>
        /// <param name="ExpandBrandIds"></param>
        public static JArray ToJSON(this IEnumerable<ChargingPool>  ChargingPools,
                                    UInt64                          Skip                      = 0,
                                    UInt64                          Take                      = 0,
                                    Boolean                         Embedded                  = false,
                                    Boolean                         ExpandChargingStationIds  = true,
                                    Boolean                         ExpandOperatorIds         = false,
                                    Boolean                         ExpandBrandIds            = false)
        {

            #region Initial checks

            if (ChargingPools == null || !ChargingPools.Any())
                return new JArray();

            #endregion

            return new JArray(ChargingPools.
                                  Where     (pool => pool != null).
                                  OrderBy   (pool => pool.Id).
                                  SkipTakeFilter(Skip, Take).
                                  SafeSelect(pool => pool.ToJSON(Embedded,
                                                                 ExpandChargingStationIds,
                                                                 ExpandOperatorIds,
                                                                 ExpandBrandIds)));

        }

        #endregion

        #region ToJSON(this ChargingPools, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingPool> ChargingPools, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingPools != null && ChargingPools.Any()
                       ? new JProperty(JPropertyKey, ChargingPools.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this ChargingPoolAdminStatus, Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>>>  ChargingPoolAdminStatus,
                                     UInt64                                                                                                   Skip         = 0,
                                     UInt64                                                                                                   Take         = 0,
                                     UInt64                                                                                                   HistorySize  = 1)
        {

            if (ChargingPoolAdminStatus == null)
                return new JObject();

            var _ChargingPoolAdminStatus = Take == 0
                                              ? ChargingPoolAdminStatus.Skip(Skip)
                                              : ChargingPoolAdminStatus.Skip(Skip).Take(Take);

            return new JObject(_ChargingPoolAdminStatus.
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

            if (ChargingPoolStatus == null)
                return new JObject();

            var _ChargingPoolStatus = Take == 0
                                          ? ChargingPoolStatus.Skip(Skip)
                                          : ChargingPoolStatus.Skip(Skip).Take(Take);

            return new JObject(_ChargingPoolStatus.
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
