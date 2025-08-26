/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for the charging tariff interface.
    /// </summary>
    public static class ChargingTariffExtensions
    {

        #region ToJSON(this ChargingTariffs, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingTariffs">An enumeration of charging tariffs.</param>
        /// <param name="Skip">The optional number of charging stations to skip.</param>
        /// <param name="Take">The optional number of charging stations to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static JArray ToJSON(this IEnumerable<IChargingTariff>  ChargingTariffs,
                                    UInt64?                            Skip                              = null,
                                    UInt64?                            Take                              = null,
                                    Boolean                            Embedded                          = false,
                                    InfoStatus                         ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandEVSEIds                     = InfoStatus.Expanded,
                                    InfoStatus                         ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                         ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingTariffs is not null && ChargingTariffs.Any()

                   ? new JArray(ChargingTariffs.
                                    Where     (chargingTariff => chargingTariff is not null).
                                    OrderBy   (chargingTariff => chargingTariff.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(chargingTariff => chargingTariff.ToJSON(Embedded,
                                                                                       ExpandRoamingNetworkId,
                                                                                       ExpandChargingStationOperatorId,
                                                                                       ExpandChargingPoolId,
                                                                                       ExpandEVSEIds,
                                                                                       ExpandBrandIds,
                                                                                       ExpandDataLicenses)))

                   : new JArray();

        #endregion

        #region GetTariffs(this ChargingStations, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingTariffs">An enumeration of charging tariffs.</param>
        /// <param name="Skip">The optional number of charging stations to skip.</param>
        /// <param name="Take">The optional number of charging stations to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static IEnumerable<String[]> GetTariffs(this IEnumerable<IChargingStation>  ChargingStations,
                                                       UInt64?                             Skip                              = null,
                                                       UInt64?                             Take                              = null,
                                                       Boolean                             Embedded                          = false,
                                                       InfoStatus                          ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                                       InfoStatus                          ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                                       InfoStatus                          ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                                       InfoStatus                          ExpandEVSEIds                     = InfoStatus.Expanded,
                                                       InfoStatus                          ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                                       InfoStatus                          ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingStations is not null && ChargingStations.Any()

                   ? ChargingStations.
                         Where     (station => station is not null).
                         OrderBy   (station => station.Id).
                         SkipTakeFilter(Skip, Take).
                         SafeSelectMany(station => {

                             var results = new List<String[]>();

                             foreach (var group in station.Operator.ChargingStationGroups.Where(group => group.Tariff is not null))
                                 if (group.AllowedMemberIds.Contains(station.Id) ||
                                     (group.AutoIncludeStations is not null && group.AutoIncludeStations(station.Operator.GetChargingStationById(station.Id))))
                                     foreach (var evse in station)
                                        results.Add(new String[] {
                                                        evse.Id.                             ToString(),
                                                     //   station.Brand.Name.                  FirstText(),
                                                        station.Name.                        FirstText(),
                                                        station.Address.Street,
                                                        station.Address.HouseNumber,
                                                        station.Address.PostalCode,
                                                        station.Address.City.                FirstText(),
                                                        station.Address.Country.CountryName. FirstText(),
                                                        evse.MaxPower.                       ToString() + " kW",
                                                        evse.ChargingConnectors.First().Plug.ToString(),
                                                        group.Tariff.Name.                   FirstText()
                                                    });

                             foreach (var evse in station)
                                 foreach (var group in evse.Operator.EVSEGroups.Where(group => group.Tariff is not null))
                                     if (group.AllowedMemberIds.Contains(evse.Id) ||
                                         (group.AutoIncludeEVSEs is not null && group.AutoIncludeEVSEs(evse.Operator.GetEVSEById(evse.Id))))
                                         results.Add(new String[] {
                                                         evse.Id.                             ToString(),
                                                      //   station.Brand.Name.                  FirstText(),
                                                         station.Name.                        FirstText(),
                                                         station.Address.Street,
                                                         station.Address.HouseNumber,
                                                         station.Address.PostalCode,
                                                         station.Address.City.                FirstText(),
                                                         station.Address.Country.CountryName. FirstText(),
                                                         evse.MaxPower.                       ToString() + " kW",
                                                         evse.ChargingConnectors.First().Plug.ToString(),
                                                         group.Tariff.Name.                   FirstText()
                                                     });

                             if (results.Count == 0)
                                 foreach (var evse in station)
                                     results.Add(new String[] {
                                                     evse.Id.                             ToString(),
                                                   //  station.Brand.Name.                  FirstText(),
                                                     station.Name.                        FirstText(),
                                                     station.Address.Street,
                                                     station.Address.HouseNumber,
                                                     station.Address.PostalCode,
                                                     station.Address.City.                FirstText(),
                                                     station.Address.Country.CountryName. FirstText(),
                                                     evse.MaxPower.                       ToString() + " kW",
                                                     evse.ChargingConnectors.First().Plug.ToString(),
                                                     "-"
                                                 });

                             return results;

                         })

                   : new List<String[]>();

        #endregion

    }


    /// <summary>
    /// The common interface of all charging tariffs.
    /// </summary>
    public interface IChargingTariff : IEntity<ChargingTariff_Id>,
                                       IEquatable<IChargingTariff>,
                                       IComparable<IChargingTariff>,
                                       IComparable
    {

        Brand?                              Brand             { get; }
        Currency                            Currency          { get; }
        EnergyMix?                          EnergyMix         { get; }
        IEnumerable<ChargingTariffElement>  TariffElements    { get; }
        URL?                                TariffURL         { get; }

        JObject ToJSON(Boolean    Embedded                         = false,
                       InfoStatus ExpandRoamingNetworkId           = InfoStatus.ShowIdOnly,
                       InfoStatus ExpandChargingStationOperatorId  = InfoStatus.ShowIdOnly,
                       InfoStatus ExpandChargingPoolId             = InfoStatus.ShowIdOnly,
                       InfoStatus ExpandEVSEIds                    = InfoStatus.Expanded,
                       InfoStatus ExpandBrandIds                   = InfoStatus.ShowIdOnly,
                       InfoStatus ExpandDataLicenses               = InfoStatus.ShowIdOnly);


        /// <summary>
        /// Clone this charging tariff.
        /// </summary>
        IChargingTariff Clone();


    }

}
