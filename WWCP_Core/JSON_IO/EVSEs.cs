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

        {

            try
            {

                return EVSE == null
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

                }

            catch (Exception e)
            {
            }

            return null;

        }

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


        #region ToJSON(this EVSEAdminStatus, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>>  EVSEAdminStatus,
                                     UInt64                                                                                  Skip         = 0,
                                     UInt64                                                                                  Take         = 0,
                                     UInt64                                                                                  HistorySize  = 1)

        {

            #region Initial checks

            if (EVSEAdminStatus == null || !EVSEAdminStatus.Any())
                return new JObject();

            var _EVSEAdminStatus = new Dictionary<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate EVSE identifications in the enumeration... take the newest one!

            foreach (var evsestatus in Take == 0 ? EVSEAdminStatus.Skip(Skip)
                                                 : EVSEAdminStatus.Skip(Skip).Take(Take))
            {

                if (!_EVSEAdminStatus.ContainsKey(evsestatus.Key))
                    _EVSEAdminStatus.Add(evsestatus.Key, evsestatus.Value);

                else if (evsestatus.Value.FirstOrDefault().Timestamp > _EVSEAdminStatus[evsestatus.Key].FirstOrDefault().Timestamp)
                    _EVSEAdminStatus[evsestatus.Key] = evsestatus.Value;

            }

            #endregion

            return _EVSEAdminStatus.Count == 0

                   ? new JObject()

                   : new JObject(_EVSEAdminStatus.
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
                                     UInt64                                                                              Skip         = 0,
                                     UInt64                                                                              Take         = 0,
                                     UInt64                                                                              HistorySize  = 1)

        {

            #region Initial checks

            if (EVSEStatus == null || !EVSEStatus.Any())
                return new JObject();

            var _EVSEStatus = new Dictionary<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate EVSE identifications in the enumeration... take the newest one!

            foreach (var evsestatus in Take == 0 ? EVSEStatus.Skip(Skip)
                                                 : EVSEStatus.Skip(Skip).Take(Take))
            {

                if (!_EVSEStatus.ContainsKey(evsestatus.Key))
                    _EVSEStatus.Add(evsestatus.Key, evsestatus.Value);

                else if (evsestatus.Value.FirstOrDefault().Timestamp > _EVSEStatus[evsestatus.Key].FirstOrDefault().Timestamp)
                    _EVSEStatus[evsestatus.Key] = evsestatus.Value;

            }

            #endregion

            return _EVSEStatus.Count == 0

                   ? new JObject()

                   : new JObject(_EVSEStatus.
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
