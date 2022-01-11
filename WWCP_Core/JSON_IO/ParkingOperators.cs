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

        #region ToJSON(this ParkingOperator, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        public static JObject ToJSON(this ParkingOperator  ParkingOperator,
                                     Boolean               Embedded                        = false,
                                     Boolean               ExpandChargingRoamingNetworkId  = false,
                                     Boolean               ExpandChargingPoolIds           = false,
                                     Boolean               ExpandChargingStationIds        = false,
                                     Boolean               ExpandEVSEIds                   = false)

            => ParkingOperator != null
                   ? JSONObject.Create(

                         new JProperty("id",                        ParkingOperator.Id.ToString()),

                         Embedded
                             ? null
                             : ExpandChargingRoamingNetworkId
                                   ? new JProperty("roamingNetwork",      ParkingOperator.RoamingNetwork.ToJSON())
                                   : new JProperty("roamingNetworkId",    ParkingOperator.RoamingNetwork.Id.ToString()),

                         new JProperty("name",                  ParkingOperator.Name.       ToJSON()),
                         new JProperty("description",           ParkingOperator.Description.ToJSON()),

                         // Address
                         // LogoURI
                         // API - RobotKeys, Endpoints, DNS SRV
                         // MainKeys

                         ParkingOperator.Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          ParkingOperator.Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         ParkingOperator.Homepage.IsNotNullOrEmpty()
                             ? new JProperty("homepage",            ParkingOperator.Homepage)
                             : null,

                         ParkingOperator.HotlinePhoneNumber.IsNotNullOrEmpty()
                             ? new JProperty("hotline",             ParkingOperator.HotlinePhoneNumber)
                             : null,

                         ParkingOperator.DataLicenses.Any()
                             ? new JProperty("dataLicenses",        new JArray(ParkingOperator.DataLicenses.Select(license => license.ToJSON())))
                             : null

                         //new JProperty("chargingPools",         ExpandChargingPoolIds
                         //                                           ? new JArray(ParkingOperator.ChargingPools.     ToJSON(Embedded: true))
                         //                                           : new JArray(ParkingOperator.ChargingPoolIds.   Select(id => id.ToString()))),

                         //new JProperty("chargingStations",      ExpandChargingStationIds
                         //                                           ? new JArray(ParkingOperator.ChargingStations.  ToJSON(Embedded: true))
                         //                                           : new JArray(ParkingOperator.ChargingStationIds.Select(id => id.ToString()))),

                         //new JProperty("evses",                 ExpandEVSEIds
                         //                                           ? new JArray(ParkingOperator.EVSEs.             ToJSON(Embedded: true))
                         //                                           : new JArray(ParkingOperator.EVSEIds.           Select(id => id.ToString())))

                     )
                   : null;

        #endregion

        #region ToJSON(this ParkingOperator, JPropertyKey)

        public static JProperty ToJSON(this ParkingOperator ParkingOperator, String JPropertyKey)

            => ParkingOperator != null
                   ? new JProperty(JPropertyKey, ParkingOperator.ToJSON())
                   : null;

        #endregion

        #region ToJSON(this ParkingOperators, Skip = null, Take = null, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        /// <summary>
        /// Return a JSON representation for the given enumeration of Charging Station Operators.
        /// </summary>
        /// <param name="ParkingOperators">An enumeration of Charging Station Operators.</param>
        /// <param name="Skip">The optional number of Charging Station Operators to skip.</param>
        /// <param name="Take">The optional number of Charging Station Operators to return.</param>
        public static JArray ToJSON(this IEnumerable<ParkingOperator>  ParkingOperators,
                                    UInt64?                            Skip                            = null,
                                    UInt64?                            Take                            = null,
                                    Boolean                            Embedded                        = false,
                                    Boolean                            ExpandChargingRoamingNetworkId  = false,
                                    Boolean                            ExpandChargingPoolIds           = false,
                                    Boolean                            ExpandChargingStationIds        = false,
                                    Boolean                            ExpandEVSEIds                   = false)
        {

            #region Initial checks

            if (ParkingOperators == null)
                return new JArray();

            #endregion

            return new JArray(ParkingOperators.
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

        #region ToJSON(this ParkingOperators, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ParkingOperator> ParkingOperators, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ParkingOperators != null
                       ? new JProperty(JPropertyKey, ParkingOperators.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this ParkingOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<ParkingOperatorAdminStatusType>>  ParkingOperatorAdminStatus,
                                     UInt64?                                                        Skip         = null,
                                     UInt64?                                                        Take         = null,
                                     UInt64?                                                        HistorySize  = 1)

        {

            if (ParkingOperatorAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(ParkingOperatorAdminStatus.
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
                // e.g. when a Stack behind ParkingOperatorAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this ParkingOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorAdminStatusType>>>>  ParkingOperatorAdminStatus,
                                     UInt64?                                                                                                       Skip         = null,
                                     UInt64?                                                                                                       Take         = null,
                                     UInt64?                                                                                                       HistorySize  = 1)

        {

            if (ParkingOperatorAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(ParkingOperatorAdminStatus.
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
                // e.g. when a Stack behind ParkingOperatorAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this ParkingOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<ParkingOperatorStatusType>>  ParkingOperatorStatus,
                                     UInt64?                                                   Skip         = null,
                                     UInt64?                                                   Take         = null,
                                     UInt64?                                                   HistorySize  = 1)

        {

            if (ParkingOperatorStatus == null)
                return new JObject();

            try
            {

                return new JObject(ParkingOperatorStatus.
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
                // e.g. when a Stack behind ParkingOperatorStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this ParkingOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorStatusType>>>>  ParkingOperatorStatus,
                                     UInt64?                                                                                                  Skip         = null,
                                     UInt64?                                                                                                  Take         = null,
                                     UInt64?                                                                                                  HistorySize  = 1)

        {

            if (ParkingOperatorStatus == null)
                return new JObject();

            try
            {

                return new JObject(ParkingOperatorStatus.
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
                // e.g. when a Stack behind ParkingOperatorStatus is empty!
                return new JObject();
            }

        }

        #endregion

    }

}
