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
using System.Globalization;
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

        #region ToJSON(this EVSE, Embedded = false, ExpandOperatorId = false, ExpandBrandId = false)

        public static JObject ToJSON(this EVSE  EVSE,
                                     Boolean    Embedded          = false,
                                     Boolean    ExpandOperatorId  = false,
                                     Boolean    ExpandBrandId     = false)
        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            // Embedded means it is served as a substructure, e.g. of a charging station
            if (Embedded)
                return JSONObject.Create(

                           EVSE.Id.                                  ToJSON("Id"),

                           EVSE.Description.IsNotNullOrEmpty()
                               ? EVSE.Description.                   ToJSON("Description")
                               : null,

                           EVSE.ChargingModes != ChargingModes.Unspecified
                               ? new JProperty("ChargingModes",  new JArray(EVSE.ChargingModes.Value.ToText()))
                               : null,

                           EVSE.CurrentTypes != CurrentTypes.Unspecified
                               ? new JProperty("CurrentTypes",  new JArray(EVSE.CurrentTypes.Value.ToText()))
                               : null,

                           EVSE.AverageVoltage.HasValue && EVSE.AverageVoltage > 0     ? new JProperty("AverageVoltage",  String.Format("{0:0.00}", EVSE.AverageVoltage)) : null,
                           EVSE.MaxCurrent.    HasValue && EVSE.MaxCurrent     > 0     ? new JProperty("MaxCurrent",      String.Format("{0:0.00}", EVSE.MaxCurrent))     : null,
                           EVSE.MaxPower.      HasValue && EVSE.MaxPower.     HasValue ? new JProperty("MaxPower",        String.Format("{0:0.00}", EVSE.MaxPower))       : null,
                           EVSE.MaxCapacity.   HasValue && EVSE.MaxCapacity.  HasValue ? new JProperty("MaxCapacity",     String.Format("{0:0.00}", EVSE.MaxCapacity))    : null,

                           EVSE.SocketOutlets.Count > 0
                              ? new JProperty("SocketOutlets",  new JArray(EVSE.SocketOutlets.ToJSON()))
                              : null,

                           EVSE.EnergyMeterId.IsNotNullOrEmpty() ? new JProperty("EnergyMeterId", EVSE.EnergyMeterId) : null

                       );

            else
                return JSONObject.Create(

                           EVSE.                             Id.ToJSON("Id"),
                           EVSE.ChargingStation.             Id.ToJSON("ChargingStationId"),
                           EVSE.ChargingStation.ChargingPool.Id.ToJSON("ChargingPoolId"),

                           ExpandOperatorId
                               ? new JProperty("Operator",    EVSE.Operator.ToJSON())
                               : new JProperty("OperatorId",  EVSE.Operator.Id.ToString()),

                           EVSE.ChargingStation.Brand != null
                               ? ExpandBrandId
                                     ? EVSE.ChargingStation.Brand.   ToJSON("Brand")
                                     : new JProperty("BrandId",      EVSE.ChargingStation.Brand.Id.ToString())
                               : null,

                           EVSE.Description.IsNotNullOrEmpty()
                               ? EVSE.Description.                   ToJSON("Description")
                               : null,

                           EVSE.ChargingModes != ChargingModes.Unspecified
                               ? new JProperty("ChargingModes",  new JArray(EVSE.ChargingModes.Value.ToText()))
                               : null,

                           EVSE.CurrentTypes != CurrentTypes.Unspecified
                               ? new JProperty("CurrentTypes",  new JArray(EVSE.CurrentTypes.Value.ToText()))
                               : null,

                           EVSE.AverageVoltage.HasValue && EVSE.AverageVoltage > 0     ? new JProperty("AverageVoltage",  String.Format("{0:0.00}", EVSE.AverageVoltage)) : null,
                           EVSE.MaxCurrent.    HasValue && EVSE.MaxCurrent     > 0     ? new JProperty("MaxCurrent",      String.Format("{0:0.00}", EVSE.MaxCurrent))     : null,
                           EVSE.MaxPower.      HasValue && EVSE.MaxPower.     HasValue ? new JProperty("MaxPower",        String.Format("{0:0.00}", EVSE.MaxPower))       : null,
                           EVSE.MaxCapacity.   HasValue && EVSE.MaxCapacity.  HasValue ? new JProperty("MaxCapacity",     String.Format("{0:0.00}", EVSE.MaxCapacity))    : null,

                           EVSE.SocketOutlets.Count > 0
                              ? new JProperty("SocketOutlets",  new JArray(EVSE.SocketOutlets.ToJSON()))
                              : null,

                           EVSE.EnergyMeterId.IsNotNullOrEmpty() ? new JProperty("EnergyMeterId", EVSE.EnergyMeterId) : null

                       );


        }

        #endregion

        #region ToJSON(this EVSE, JPropertyKey)

        public static JProperty ToJSON(this EVSE EVSE, String JPropertyKey)
        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE),          "The given EVSE must not be null!");

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey),  "The given json property key must not be null or empty!");

            #endregion

            return new JProperty(JPropertyKey,
                                 EVSE.ToJSON());

        }

        #endregion

        #region ToJSON(this EVSEs, Skip = 0, Take = 0, Embedded = false, ExpandOperatorIds = false, ExpandBrandIds = false)

        public static JArray ToJSON(this IEnumerable<EVSE>  EVSEs,
                                    UInt64                  Skip               = 0,
                                    UInt64                  Take               = 0,
                                    Boolean                 Embedded           = false,
                                    Boolean                 ExpandOperatorIds  = false,
                                    Boolean                 ExpandBrandIds     = false)
        {

            #region Initial checks

            if (EVSEs == null)
                return new JArray();

            #endregion

            return new JArray(EVSEs.
                                  Where     (evse => evse != null).
                                  OrderBy   (evse => evse.Id).
                                  SkipTakeFilter(Skip, Take).
                                  SafeSelect(evse => evse.ToJSON(Embedded,
                                                                 ExpandOperatorIds,
                                                                 ExpandBrandIds)));

        }

        #endregion

        #region ToJSON(this EVSEs, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<EVSE> EVSEs, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return EVSEs != null
                       ? new JProperty(JPropertyKey, EVSEs.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this EVSEAdminStatus, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>>  EVSEAdminStatus,
                                     UInt64                                                                                  Skip         = 0,
                                     UInt64                                                                                  Take         = 0,
                                     UInt64                                                                                  HistorySize  = 1)

        {

            if (EVSEAdminStatus == null)
                return new JObject();

            var _EVSEAdminStatus = Take == 0
                                      ? EVSEAdminStatus.Skip(Skip).           ToArray()
                                      : EVSEAdminStatus.Skip(Skip).Take(Take).ToArray();

            if (_EVSEAdminStatus.Length == 0)
                return new JObject();

            return new JObject(_EVSEAdminStatus.
                                   SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                new JObject(statuslist.Value.

                                                                            // Will filter multiple evse status having the exact same ISO 8601 timestamp!
                                                                            GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                            Select           (group => group.First()).

                                                                            OrderByDescending(tsv   => tsv.Timestamp).
                                                                            Take             (HistorySize).
                                                                            Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                     tsv.Value.    ToString())))

                                                            )));

        }

        #endregion

        #region ToJSON(this EVSEStatus, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>>  EVSEStatus,
                                     UInt64                                                                             Skip         = 0,
                                     UInt64                                                                             Take         = 0,
                                     UInt64                                                                             HistorySize  = 1)

        {

            if (EVSEStatus == null)
                return new JObject();

            var _EVSEStatus = Take == 0
                                  ? EVSEStatus.Skip(Skip).           ToArray()
                                  : EVSEStatus.Skip(Skip).Take(Take).ToArray();

            if (_EVSEStatus.Length == 0)
                return new JObject();

            return new JObject(_EVSEStatus.
                                   SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                new JObject(statuslist.Value.

                                                                            // Will filter multiple evse status having the exact same ISO 8601 timestamp!
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
