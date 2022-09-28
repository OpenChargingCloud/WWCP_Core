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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace cloud.charging.open.protocols.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this EVSEGroup,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given EVSE group.
        /// </summary>
        /// <param name="EVSEGroup">A EVSE group.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JObject? ToJSON(this EVSEGroup  EVSEGroup,
                                      Boolean         Embedded                          = false,
                                      InfoStatus      ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                      InfoStatus      ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                      InfoStatus      ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                      InfoStatus      ExpandEVSEIds                     = InfoStatus.Expanded,
                                      InfoStatus      ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                      InfoStatus      ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => EVSEGroup is null

                   ? null

                   : JSONObject.Create(

                         new JProperty("@id", EVSEGroup.Id.ToString()),

                         Embedded
                             ? null
                             : new JProperty("@context", "https://open.charging.cloud/contexts/wwcp+json/EVSEGroup"),

                         EVSEGroup.Name.       IsNeitherNullNorEmpty()
                             ? new JProperty("name",        EVSEGroup.Name.ToJSON())
                             : null,

                         EVSEGroup.Description.IsNeitherNullNorEmpty()
                             ? new JProperty("description", EVSEGroup.Description.ToJSON())
                             : null,

                         EVSEGroup.Brand != null
                             ? ExpandBrandIds.Switch(
                                   () => new JProperty("brandId",  EVSEGroup.Brand.Id.ToString()),
                                   () => new JProperty("brand",    EVSEGroup.Brand.   ToJSON()))
                             : null,

                         (!Embedded || EVSEGroup.DataSource != EVSEGroup.Operator.DataSource)
                             ? new JProperty("dataSource", EVSEGroup.DataSource)
                             : null,

                         (!Embedded || EVSEGroup.DataLicenses != EVSEGroup.Operator.DataLicenses)
                             ? ExpandDataLicenses.Switch(
                                 () => new JProperty("dataLicenseIds",  new JArray(EVSEGroup.DataLicenses.SafeSelect(license => license.Id.ToString()))),
                                 () => new JProperty("dataLicenses",    EVSEGroup.DataLicenses.ToJSON()))
                             : null,

                         #region Embedded means it is served as a substructure of e.g. a charging station operator

                         Embedded
                             ? null
                             : ExpandRoamingNetworkId.Switch(
                                   () => new JProperty("roamingNetworkId",           EVSEGroup.RoamingNetwork.Id. ToString()),
                                   () => new JProperty("roamingNetwork",             EVSEGroup.RoamingNetwork.    ToJSON(Embedded:                          true,
                                                                                                                                    ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
                                                                                                                                    ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                                    ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                                    ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                                    ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                                    ExpandDataLicenses:                InfoStatus.Hidden))),

                         Embedded
                             ? null
                             : ExpandChargingStationOperatorId.Switch(
                                   () => new JProperty("chargingStationOperatorId",  EVSEGroup.Operator.Id.       ToString()),
                                   () => new JProperty("chargingStationOperator",    EVSEGroup.Operator.          ToJSON(Embedded:                          true,
                                                                                                                                    ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                                    ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                                    ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                                    ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                                    ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                                    ExpandDataLicenses:                InfoStatus.Hidden))),

                         #endregion

                         //(!Embedded || ChargingStation.GeoLocation         != ChargingStation.ChargingPool.GeoLocation)         ? ChargingStation.GeoLocation.Value.  ToJSON("geoLocation")         : null,
                         //(!Embedded || ChargingStation.Address             != ChargingStation.ChargingPool.Address)             ? ChargingStation.Address.            ToJSON("address")             : null,
                         //(!Embedded || ChargingStation.AuthenticationModes != ChargingStation.ChargingPool.AuthenticationModes) ? ChargingStation.AuthenticationModes.ToJSON("authenticationModes") : null,
                         //(!Embedded || ChargingStation.HotlinePhoneNumber  != ChargingStation.ChargingPool.HotlinePhoneNumber)  ? ChargingStation.HotlinePhoneNumber. ToJSON("hotlinePhoneNumber")  : null,
                         //(!Embedded || ChargingStation.OpeningTimes        != ChargingStation.ChargingPool.OpeningTimes)        ? ChargingStation.OpeningTimes.       ToJSON("openingTimes")        : null,

                         ExpandEVSEIds.Switch(
                             () => new JProperty("EVSEIds",
                                                 EVSEGroup.EVSEIds.SafeAny()
                                                     ? new JArray(EVSEGroup.EVSEIds.
                                                                                       OrderBy(evseid => evseid).
                                                                                       Select (evseid => evseid.ToString()))
                                                     : null),

                             () => new JProperty("EVSEs",
                                                 EVSEGroup.EVSEs.SafeAny()
                                                     ? new JArray(EVSEGroup.EVSEs.
                                                                                       OrderBy(evse   => evse.Id).
                                                                                       Select (evse   => evse.  ToJSON(Embedded: true)))
                                                     : null))

                        );

        #endregion

        #region ToJSON(this EVSEGroups, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging stations.
        /// </summary>
        /// <param name="EVSEGroups">An enumeration of EVSE groups.</param>
        /// <param name="Skip">The optional number of charging stations to skip.</param>
        /// <param name="Take">The optional number of charging stations to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static JArray? ToJSON(this IEnumerable<EVSEGroup>  EVSEGroups,
                                     UInt64?                      Skip                              = null,
                                     UInt64?                      Take                              = null,
                                     Boolean                      Embedded                          = false,
                                     InfoStatus                   ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                     InfoStatus                   ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                     InfoStatus                   ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                     InfoStatus                   ExpandEVSEIds                     = InfoStatus.Expanded,
                                     InfoStatus                   ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                     InfoStatus                   ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => EVSEGroups is not null && EVSEGroups.Any()

                   ? new JArray(EVSEGroups.
                                    Where     (stationgroup => stationgroup != null).
                                    OrderBy   (stationgroup => stationgroup.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(stationgroup => stationgroup.ToJSON(Embedded,
                                                                                   ExpandRoamingNetworkId,
                                                                                   ExpandChargingStationOperatorId,
                                                                                   ExpandChargingPoolId,
                                                                                   ExpandEVSEIds,
                                                                                   ExpandBrandIds,
                                                                                   ExpandDataLicenses)))

                   : null;

        #endregion

        #region ToJSON(this EVSEGroups, JPropertyKey)

        public static JProperty? ToJSON(this IEnumerable<EVSEGroup> EVSEGroups, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return EVSEGroups?.Any() == true
                       ? new JProperty(JPropertyKey, EVSEGroups.ToJSON())
                       : null;

        }

        #endregion

    }

}
