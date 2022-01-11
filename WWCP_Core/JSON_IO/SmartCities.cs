/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        #region ToJSON(this SmartCity, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        public static JObject ToJSON(this SmartCityProxy  SmartCity,
                                     Boolean                       Embedded                        = false,
                                     Boolean                       ExpandChargingRoamingNetworkId  = false,
                                     Boolean                       ExpandChargingPoolIds           = false,
                                     Boolean                       ExpandChargingStationIds        = false,
                                     Boolean                       ExpandEVSEIds                   = false)

            => SmartCity != null
                   ? JSONObject.Create(

                         new JProperty("id",                        SmartCity.Id.ToString()),

                         Embedded
                             ? null
                             : ExpandChargingRoamingNetworkId
                                   ? new JProperty("roamingNetwork",      SmartCity.RoamingNetwork.ToJSON())
                                   : new JProperty("roamingNetworkId",    SmartCity.RoamingNetwork.Id.ToString()),

                         new JProperty("name",                  SmartCity.Name.       ToJSON()),
                         new JProperty("description",           SmartCity.Description.ToJSON()),

                         // Address
                         // LogoURI
                         // API - RobotKeys, Endpoints, DNS SRV
                         // MainKeys

                         SmartCity.Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          SmartCity.Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         SmartCity.Homepage.IsNotNullOrEmpty()
                             ? new JProperty("homepage",            SmartCity.Homepage)
                             : null,

                         SmartCity.HotlinePhoneNumber.IsNotNullOrEmpty()
                             ? new JProperty("hotline",             SmartCity.HotlinePhoneNumber)
                             : null,

                         SmartCity.DataLicenses.Any()
                             ? new JProperty("dataLicenses",        new JArray(SmartCity.DataLicenses.Select(license => license.ToJSON())))
                             : null

                         //new JProperty("chargingPools",         ExpandChargingPoolIds
                         //                                           ? new JArray(SmartCity.ChargingPools.     ToJSON(Embedded: true))
                         //                                           : new JArray(SmartCity.ChargingPoolIds.   Select(id => id.ToString()))),

                         //new JProperty("chargingStations",      ExpandChargingStationIds
                         //                                           ? new JArray(SmartCity.ChargingStations.  ToJSON(Embedded: true))
                         //                                           : new JArray(SmartCity.ChargingStationIds.Select(id => id.ToString()))),

                         //new JProperty("evses",                 ExpandEVSEIds
                         //                                           ? new JArray(SmartCity.EVSEs.             ToJSON(Embedded: true))
                         //                                           : new JArray(SmartCity.EVSEIds.           Select(id => id.ToString())))

                     )
                   : null;

        #endregion

        #region ToJSON(this SmartCity, JPropertyKey)

        public static JProperty ToJSON(this SmartCityProxy SmartCity, String JPropertyKey)

            => SmartCity != null
                   ? new JProperty(JPropertyKey, SmartCity.ToJSON())
                   : null;

        #endregion

        #region ToJSON(this SmartCities, Skip = null, Take = null, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        /// <summary>
        /// Return a JSON representation for the given enumeration of Charging Station Operators.
        /// </summary>
        /// <param name="SmartCities">An enumeration of Charging Station Operators.</param>
        /// <param name="Skip">The optional number of Charging Station Operators to skip.</param>
        /// <param name="Take">The optional number of Charging Station Operators to return.</param>
        public static JArray ToJSON(this IEnumerable<SmartCityProxy>  SmartCities,
                                    UInt64?                           Skip                            = null,
                                    UInt64?                           Take                            = null,
                                    Boolean                           Embedded                        = false,
                                    Boolean                           ExpandChargingRoamingNetworkId  = false,
                                    Boolean                           ExpandChargingPoolIds           = false,
                                    Boolean                           ExpandChargingStationIds        = false,
                                    Boolean                           ExpandEVSEIds                   = false)
        {

            #region Initial checks

            if (SmartCities == null)
                return new JArray();

            #endregion

            return new JArray(SmartCities.
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

        #region ToJSON(this SmartCities, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<SmartCityProxy> SmartCities, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return SmartCities != null
                       ? new JProperty(JPropertyKey, SmartCities.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this SmartCityAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<SmartCityAdminStatusType>>  SmartCityAdminStatus,
                                     UInt64?                                                  Skip         = null,
                                     UInt64?                                                  Take         = null,
                                     UInt64?                                                  HistorySize  = 1)

        {

            if (SmartCityAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(SmartCityAdminStatus.
                                       SkipTakeFilter(Skip, Take).

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
                // e.g. when a Stack behind SmartCityAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this SmartCityAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityAdminStatusType>>>>  SmartCityAdminStatus,
                                     UInt64?                                                                                           Skip         = null,
                                     UInt64?                                                                                           Take         = null,
                                     UInt64?                                                                                           HistorySize  = 1)

        {

            if (SmartCityAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(SmartCityAdminStatus.
                                       SkipTakeFilter(Skip, Take).
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
            catch (Exception)
            {
                // e.g. when a Stack behind SmartCityAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this SmartCityStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<SmartCityStatusType>>  SmartCityStatus,
                                     UInt64?                                             Skip         = null,
                                     UInt64?                                             Take         = null,
                                     UInt64?                                             HistorySize  = 1)

        {

            if (SmartCityStatus == null)
                return new JObject();

            try
            {

                return new JObject(SmartCityStatus.
                                       SkipTakeFilter(Skip, Take).

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
                // e.g. when a Stack behind SmartCityStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this SmartCityStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityStatusType>>>>  SmartCityStatus,
                                     UInt64?                                                                                      Skip         = null,
                                     UInt64?                                                                                      Take         = null,
                                     UInt64?                                                                                      HistorySize  = 1)

        {

            if (SmartCityStatus == null)
                return new JObject();

            try
            {

                return new JObject(SmartCityStatus.
                                       SkipTakeFilter(Skip, Take).
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
            catch (Exception)
            {
                // e.g. when a Stack behind SmartCityStatus is empty!
                return new JObject();
            }

        }

        #endregion

    }

}
