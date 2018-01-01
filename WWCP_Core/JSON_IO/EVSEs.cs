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

        #region ToJSON(this EVSE,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        public static JObject ToJSON(this EVSE  EVSE,
                                     Boolean    Embedded                         = false,
                                     InfoStatus ExpandRoamingNetworkId           = InfoStatus.ShowIdOnly,
                                     InfoStatus ExpandChargingStationOperatorId  = InfoStatus.ShowIdOnly,
                                     InfoStatus ExpandChargingPoolId             = InfoStatus.ShowIdOnly,
                                     InfoStatus ExpandChargingStationId          = InfoStatus.ShowIdOnly,
                                     InfoStatus ExpandBrandIds                   = InfoStatus.ShowIdOnly,
                                     InfoStatus ExpandDataLicenses               = InfoStatus.ShowIdOnly)

            => EVSE == null
                   ? null

                   : JSONObject.Create(

                         EVSE.Id.ToJSON("@id"),

                         Embedded
                             ? new JProperty("@context",  "https://open.charging.cloud/contexts/wwcp+json/EVSE")
                             : null,

                         EVSE.Description.IsNeitherNullNorEmpty()
                             ? EVSE.Description.ToJSON("description")
                             : null,

                         EVSE.Brand != null
                             ? ExpandBrandIds.Switch(
                                   () => new JProperty("brandId",  EVSE.Brand.Id.ToString()),
                                   () => new JProperty("brand",    EVSE.Brand.   ToJSON()))
                             : null,

                         (!Embedded || EVSE.DataSource != EVSE.ChargingStation.DataSource)
                             ? EVSE.DataSource.ToJSON("dataSource")
                             : null,

                         (!Embedded || EVSE.DataLicenses != EVSE.ChargingStation.DataLicenses)
                             ? ExpandDataLicenses.Switch(
                                   () => new JProperty("dataLicenseIds",  new JArray(EVSE.DataLicenses.SafeSelect(license => license.Id.ToString()))),
                                   () => new JProperty("dataLicenses",    EVSE.DataLicenses.ToJSON()))
                             : null,

                         #region Embedded means it is served as a substructure, e.g. of a charging station

                         Embedded
                             ? null
                             : ExpandRoamingNetworkId.Switch(
                                   () => new JProperty("roamingNetworkId",           EVSE.RoamingNetwork.Id. ToString()),
                                   () => new JProperty("roamingNetwork",             EVSE.RoamingNetwork.    ToJSON(Embedded:                          true,
                                                                                                                    ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
                                                                                                                    ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                    ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                    ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                    ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                    ExpandDataLicenses:                InfoStatus.Hidden))),

                         Embedded
                             ? null
                             : ExpandChargingStationOperatorId.Switch(
                                   () => new JProperty("chargingStationOperatorId",  EVSE.Operator.Id.       ToString()),
                                   () => new JProperty("chargingStationOperator",    EVSE.Operator.          ToJSON(Embedded:                          true,
                                                                                                                    ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                    ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                    ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                    ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                    ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                    ExpandDataLicenses:                InfoStatus.Hidden))),

                         Embedded
                             ? null
                             : ExpandChargingPoolId.Switch(
                                   () => new JProperty("chargingPoolId",             EVSE.ChargingPool.Id.   ToString()),
                                   () => new JProperty("chargingPool",               EVSE.ChargingPool.      ToJSON(Embedded:                          true,
                                                                                                                    ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                    ExpandChargingStationOperatorId:   InfoStatus.Hidden,
                                                                                                                    ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                    ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                    ExpandDataLicenses:                InfoStatus.Hidden))),

                         Embedded
                             ? null
                             : ExpandChargingStationId.Switch(
                                   () => new JProperty("chargingStationId",          EVSE.ChargingStation.Id.ToString()),
                                   () => new JProperty("chargingStation",            EVSE.ChargingStation.   ToJSON(Embedded:                          true,
                                                                                                                    ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                    ExpandChargingStationOperatorId:   InfoStatus.Hidden,
                                                                                                                    ExpandChargingPoolId:              InfoStatus.Hidden,
                                                                                                                    ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                    ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                    ExpandDataLicenses:                InfoStatus.Hidden))),

                         #endregion

                         !Embedded ? EVSE.ChargingStation.GeoLocation.Value.  ToJSON("geoLocation")         : null,
                         !Embedded ? EVSE.ChargingStation.Address.            ToJSON("address")             : null,
                         !Embedded ? EVSE.ChargingStation.AuthenticationModes.ToJSON("authenticationModes") : null,

                         EVSE.ChargingModes.HasValue && EVSE.ChargingModes.Value != ChargingModes.Unspecified
                             ? new JProperty("chargingModes",  new JArray(EVSE.ChargingModes.Value.ToText()))
                             : null,

                         EVSE.CurrentTypes.HasValue  && EVSE.CurrentTypes.Value  != CurrentTypes.Unspecified
                             ? new JProperty("currentTypes",   new JArray(EVSE.CurrentTypes. Value.ToText()))
                             : null,

                         EVSE.AverageVoltage.HasValue && EVSE.AverageVoltage > 0     ? new JProperty("averageVoltage",  String.Format("{0:0.00}", EVSE.AverageVoltage)) : null,
                         EVSE.MaxCurrent.    HasValue && EVSE.MaxCurrent     > 0     ? new JProperty("maxCurrent",      String.Format("{0:0.00}", EVSE.MaxCurrent))     : null,
                         EVSE.MaxPower.      HasValue && EVSE.MaxPower.     HasValue ? new JProperty("maxPower",        String.Format("{0:0.00}", EVSE.MaxPower))       : null,
                         EVSE.MaxCapacity.   HasValue && EVSE.MaxCapacity.  HasValue ? new JProperty("maxCapacity",     String.Format("{0:0.00}", EVSE.MaxCapacity))    : null,

                         EVSE.SocketOutlets.Count > 0
                            ? new JProperty("socketOutlets",  new JArray(EVSE.SocketOutlets.ToJSON()))
                            : null,

                         EVSE.EnergyMeterId.IsNotNullOrEmpty() ? new JProperty("energyMeterId", EVSE.EnergyMeterId) : null,

                         !Embedded ? EVSE.ChargingStation.OpeningTimes.ToJSON("openingTimes") : null

                     );

        #endregion

        #region ToJSON(this EVSEs, Skip = 0, Take = 0, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="Skip">The optional number of EVSEs to skip.</param>
        /// <param name="Take">The optional number of EVSEs to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        public static JArray ToJSON(this IEnumerable<EVSE>  EVSEs,
                                    UInt64                  Skip                              = 0,
                                    UInt64                  Take                              = 0,
                                    Boolean                 Embedded                          = false,
                                    InfoStatus              ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus              ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus              ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus              ExpandChargingStationId           = InfoStatus.ShowIdOnly,
                                    InfoStatus              ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus              ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => EVSEs == null || !EVSEs.Any()

                   ? null

                   : new JArray(EVSEs.
                                    Where     (evse => evse != null).
                                    OrderBy   (evse => evse.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(evse => evse.ToJSON(Embedded,
                                                                   ExpandRoamingNetworkId,
                                                                   ExpandChargingStationOperatorId,
                                                                   ExpandChargingPoolId,
                                                                   ExpandChargingStationId,
                                                                   ExpandBrandIds,
                                                                   ExpandDataLicenses)));

        #endregion

        #region ToJSON(this EVSEs, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<EVSE> EVSEs, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return EVSEs?.Any() == true
                       ? new JProperty(JPropertyKey, EVSEs.ToJSON())
                       : null;

        }

        #endregion


        #region ToJSON(this EVSEAdminStatus,          Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<EVSEAdminStatus>  EVSEAdminStatus,
                                     UInt64                             Skip  = 0,
                                     UInt64                             Take  = 0)
        {

            #region Initial checks

            if (EVSEAdminStatus == null || !EVSEAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<EVSE_Id, EVSEAdminStatus>();

            foreach (var status in EVSEAdminStatus)
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

        #region ToJSON(this EVSEAdminStatusSchedules, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<EVSEAdminStatusSchedule>  EVSEAdminStatusSchedules,
                                     UInt64                                     Skip         = 0,
                                     UInt64                                     Take         = 0,
                                     UInt64                                     HistorySize  = 1)
        {

            #region Initial checks

            if (EVSEAdminStatusSchedules == null || !EVSEAdminStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<EVSE_Id, EVSEAdminStatusSchedule>();

            foreach (var status in EVSEAdminStatusSchedules)
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


        #region ToJSON(this EVSEStatus,               Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<EVSEStatus>  EVSEStatus,
                                     UInt64                        Skip  = 0,
                                     UInt64                        Take  = 0)
        {

            #region Initial checks

            if (EVSEStatus == null || !EVSEStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<EVSE_Id, EVSEStatus>();

            foreach (var status in EVSEStatus)
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

        #region ToJSON(this EVSEAdminStatusSchedules, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<EVSEStatusSchedule>  EVSEStatusSchedules,
                                     UInt64                                Skip         = 0,
                                     UInt64                                Take         = 0,
                                     UInt64                                HistorySize  = 1)
        {

            #region Initial checks

            if (EVSEStatusSchedules == null || !EVSEStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<EVSE_Id, EVSEStatusSchedule>();

            foreach (var status in EVSEStatusSchedules)
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
