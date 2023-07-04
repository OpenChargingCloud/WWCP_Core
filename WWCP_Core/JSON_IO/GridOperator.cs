/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this GridOperator, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        public static JObject ToJSON(this GridOperator  GridOperator,
                                     Boolean                       Embedded                        = false,
                                     Boolean                       ExpandChargingRoamingNetworkId  = false,
                                     Boolean                       ExpandChargingPoolIds           = false,
                                     Boolean                       ExpandChargingStationIds        = false,
                                     Boolean                       ExpandEVSEIds                   = false)

            => GridOperator != null
                   ? JSONObject.Create(

                         new JProperty("id",                        GridOperator.Id.ToString()),

                         Embedded
                             ? null
                             : ExpandChargingRoamingNetworkId
                                   ? new JProperty("roamingNetwork",      GridOperator.RoamingNetwork.ToJSON())
                                   : new JProperty("roamingNetworkId",    GridOperator.RoamingNetwork.Id.ToString()),

                         new JProperty("name",                  GridOperator.Name.       ToJSON()),
                         new JProperty("description",           GridOperator.Description.ToJSON()),

                         // Address
                         // LogoURI
                         // API - RobotKeys, Endpoints, DNS SRV
                         // MainKeys

                         GridOperator.Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          GridOperator.Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         GridOperator.Homepage.IsNotNullOrEmpty()
                             ? new JProperty("homepage",            GridOperator.Homepage)
                             : null,

                         GridOperator.HotlinePhoneNumber.IsNotNullOrEmpty()
                             ? new JProperty("hotline",             GridOperator.HotlinePhoneNumber)
                             : null,

                         GridOperator.DataLicenses.Any()
                             ? new JProperty("dataLicenses",        new JArray(GridOperator.DataLicenses.Select(license => license.ToJSON())))
                             : null

                         //new JProperty("chargingPools",         ExpandChargingPoolIds
                         //                                           ? new JArray(GridOperator.ChargingPools.     ToJSON(Embedded: true))
                         //                                           : new JArray(GridOperator.ChargingPoolIds.   Select(id => id.ToString()))),

                         //new JProperty("chargingStations",      ExpandChargingStationIds
                         //                                           ? new JArray(GridOperator.ChargingStations.  ToJSON(Embedded: true))
                         //                                           : new JArray(GridOperator.ChargingStationIds.Select(id => id.ToString()))),

                         //new JProperty("evses",                 ExpandEVSEIds
                         //                                           ? new JArray(GridOperator.EVSEs.             ToJSON(Embedded: true))
                         //                                           : new JArray(GridOperator.EVSEIds.           Select(id => id.ToString())))

                     )
                   : null;

        #endregion

        #region ToJSON(this GridOperator, JPropertyKey)

        public static JProperty ToJSON(this GridOperator GridOperator, String JPropertyKey)

            => GridOperator != null
                   ? new JProperty(JPropertyKey, GridOperator.ToJSON())
                   : null;

        #endregion

        #region ToJSON(this GridOperators, Skip = null, Take = null, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        /// <summary>
        /// Return a JSON representation for the given enumeration of Charging Station Operators.
        /// </summary>
        /// <param name="GridOperators">An enumeration of Charging Station Operators.</param>
        /// <param name="Skip">The optional number of Charging Station Operators to skip.</param>
        /// <param name="Take">The optional number of Charging Station Operators to return.</param>
        public static JArray ToJSON(this IEnumerable<GridOperator>  GridOperators,
                                    UInt64?                         Skip                            = null,
                                    UInt64?                         Take                            = null,
                                    Boolean                         Embedded                        = false,
                                    Boolean                         ExpandChargingRoamingNetworkId  = false,
                                    Boolean                         ExpandChargingPoolIds           = false,
                                    Boolean                         ExpandChargingStationIds        = false,
                                    Boolean                         ExpandEVSEIds                   = false)
        {

            #region Initial checks

            if (GridOperators == null)
                return new JArray();

            #endregion

            return new JArray(GridOperators.
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

        #region ToJSON(this GridOperators, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<GridOperator> GridOperators, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return GridOperators != null
                       ? new JProperty(JPropertyKey, GridOperators.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this GridOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<GridOperatorAdminStatusType>>  GridOperatorAdminStatus,
                                     UInt64?                                                     Skip         = null,
                                     UInt64?                                                     Take         = null,
                                     UInt64?                                                     HistorySize  = 1)

        {

            if (GridOperatorAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorAdminStatus.
                                       SkipTakeFilter(Skip, Take).

                                       // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                       GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                       Select           (group => group.First()).

                                       OrderByDescending(tsv   => tsv.Timestamp).
                                       Take             (HistorySize).
                                       Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                tsv.Value.    ToString())));

            }
            catch
            {
                // e.g. when a Stack behind GridOperatorAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this GridOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorAdminStatusType>>>>  GridOperatorAdminStatus,
                                     UInt64?                                                                                                 Skip         = null,
                                     UInt64?                                                                                                 Take         = null,
                                     UInt64?                                                                                                 HistorySize  = 1)

        {

            if (GridOperatorAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorAdminStatus.
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
            catch
            {
                // e.g. when a Stack behind GridOperatorAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this GridOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<GridOperatorStatusType>>  GridOperatorStatus,
                                     UInt64?                                                Skip         = null,
                                     UInt64?                                                Take         = null,
                                     UInt64?                                                HistorySize  = 1)

        {

            if (GridOperatorStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorStatus.
                                       SkipTakeFilter(Skip, Take).

                                       // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                       GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                       Select           (group => group.First()).

                                       OrderByDescending(tsv   => tsv.Timestamp).
                                       Take             (HistorySize).
                                       Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                tsv.Value.    ToString())));

            }
            catch
            {
                // e.g. when a Stack behind GridOperatorStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this GridOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorStatusType>>>>  GridOperatorStatus,
                                     UInt64?                                                                                            Skip         = null,
                                     UInt64?                                                                                            Take         = null,
                                     UInt64?                                                                                            HistorySize  = 1)

        {

            if (GridOperatorStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorStatus.
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
            catch
            {
                // e.g. when a Stack behind GridOperatorStatus is empty!
                return new JObject();
            }

        }

        #endregion

    }

}
