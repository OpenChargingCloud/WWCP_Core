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

        #region ToJSON(this ChargingTariff,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging tariff.
        /// </summary>
        /// <param name="ChargingTariff">A charging tariff.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JObject ToJSON(this ChargingTariff  ChargingTariff,
                                     Boolean              Embedded                          = false,
                                     InfoStatus           ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                     InfoStatus           ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                     InfoStatus           ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                     InfoStatus           ExpandEVSEIds                     = InfoStatus.Expand,
                                     InfoStatus           ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                     InfoStatus           ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingTariff == null

                   ? null

                   : JSONObject.Create(

                         ChargingTariff.Id.ToJSON("@id"),

                         Embedded
                             ? null
                             : new JProperty("@context", "https://open.charging.cloud/contexts/wwcp+json/ChargingTariff"),

                         ChargingTariff.Name.       IsNeitherNullNorEmpty()
                             ? ChargingTariff.Name.       ToJSON("name")
                             : null,

                         ChargingTariff.Description.IsNeitherNullNorEmpty()
                             ? ChargingTariff.Description.ToJSON("description")
                             : null,

                         ChargingTariff.Brand != null
                             ? ExpandBrandIds.Switch(
                                   () => new JProperty("brandId",  ChargingTariff.Brand.Id.ToString()),
                                   () => new JProperty("brand",    ChargingTariff.Brand.   ToJSON()))
                             : null,

                         (!Embedded || ChargingTariff.DataSource != ChargingTariff.Operator.DataSource)
                             ? ChargingTariff.DataSource.ToJSON("dataSource")
                             : null,

                         //(!Embedded || ChargingTariff.DataLicenses != ChargingTariff.Operator.DataLicenses)
                         //    ? ExpandDataLicenses.Switch(
                         //        () => new JProperty("dataLicenseIds",  new JArray(ChargingTariff.DataLicenses.SafeSelect(license => license.Id.ToString()))),
                         //        () => new JProperty("dataLicenses",    ChargingTariff.DataLicenses.ToJSON()))
                         //    : null,


                         new JProperty("currency", ChargingTariff.Currency.ISOCode),

                         ChargingTariff.TariffURI != null
                             ? new JProperty("URI", ChargingTariff.TariffURI.ToString())
                             : null,

                         ChargingTariff.TariffElements.Any()
                             ? new JProperty("elements", new JArray(ChargingTariff.TariffElements.Select(TariffElement => TariffElement.ToJSON())))
                             : null,

                         ChargingTariff.EnergyMix != null
                             ? new JProperty("energy_mix", ChargingTariff.EnergyMix.ToJSON())
                             : null,



                         ChargingTariff.Operator.ChargingStationGroups.Any(group => group.Tariff == ChargingTariff)
                             ? new JProperty("chargingStations", new JArray(ChargingTariff.Operator.ChargingStationGroups.
                                                                                Where (group => group.Tariff == ChargingTariff).
                                                                                Select(group => group.AllowedMemberIds.
                                                                                                          Select(id => id.ToString()))))
                             : null,

                         ChargingTariff.Operator.EVSEGroups.           Any(group => group.Tariff == ChargingTariff)
                             ? new JProperty("EVSEs",            new JArray(ChargingTariff.Operator.EVSEGroups.
                                                                                Where (group => group.Tariff == ChargingTariff).
                                                                                Select(group => group.AllowedMemberIds.
                                                                                                          Select(id => id.ToString()))))
                             : null






        //                 #region Embedded means it is served as a substructure of e.g. a charging station operator

        //                 Embedded
        //                     ? null
        //                     : ExpandRoamingNetworkId.Switch(
        //                           () => new JProperty("roamingNetworkId",           ChargingTariff.RoamingNetwork.Id. ToString()),
        //                           () => new JProperty("roamingNetwork",             ChargingTariff.RoamingNetwork.    ToJSON(Embedded:                          true,
        //                                                                                                                            ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
        //                                                                                                                            ExpandChargingPoolIds:             InfoStatus.Hidden,
        //                                                                                                                            ExpandChargingStationIds:          InfoStatus.Hidden,
        //                                                                                                                            ExpandEVSEIds:                     InfoStatus.Hidden,
        //                                                                                                                            ExpandBrandIds:                    InfoStatus.Hidden,
        //                                                                                                                            ExpandDataLicenses:                InfoStatus.Hidden))),

        //                 Embedded
        //                     ? null
        //                     : ExpandChargingStationOperatorId.Switch(
        //                           () => new JProperty("chargingStationOperatorId",  ChargingTariff.Operator.Id.       ToString()),
        //                           () => new JProperty("chargingStationOperator",    ChargingTariff.Operator.          ToJSON(Embedded:                          true,
        //                                                                                                                            ExpandRoamingNetworkId:            InfoStatus.Hidden,
        //                                                                                                                            ExpandChargingPoolIds:             InfoStatus.Hidden,
        //                                                                                                                            ExpandChargingStationIds:          InfoStatus.Hidden,
        //                                                                                                                            ExpandEVSEIds:                     InfoStatus.Hidden,
        //                                                                                                                            ExpandBrandIds:                    InfoStatus.Hidden,
        //                                                                                                                            ExpandDataLicenses:                InfoStatus.Hidden))),

        //                 #endregion

        //                 ExpandEVSEIds.Switch(
        //                     () => new JProperty("EVSEIds",
        //                                         ChargingTariff.EVSEIds.SafeAny()
        //                                             ? new JArray(ChargingTariff.EVSEIds.
        //                                                                               OrderBy(evseid => evseid).
        //                                                                               Select (evseid => evseid.ToString()))
        //                                             : null),

        //                     () => new JProperty("EVSEs",
        //                                         ChargingTariff.EVSEs.SafeAny()
        //                                             ? new JArray(ChargingTariff.EVSEs.
        //                                                                               OrderBy(evse   => evse.Id).
        //                                                                               Select (evse   => evse.  ToJSON(Embedded: true)))
        //                                             : null))

                        );

        #endregion

        #region ToJSON(this ChargingTariffs, Skip = 0, Take = 0, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingTariffs">An enumeration of charging tariffs.</param>
        /// <param name="Skip">The optional number of charging stations to skip.</param>
        /// <param name="Take">The optional number of charging stations to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static JArray ToJSON(this IEnumerable<ChargingTariff>  ChargingTariffs,
                                    UInt64                                  Skip                              = 0,
                                    UInt64                                  Take                              = 0,
                                    Boolean                                 Embedded                          = false,
                                    InfoStatus                              ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandEVSEIds                     = InfoStatus.Expand,
                                    InfoStatus                              ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingTariffs != null && ChargingTariffs.Any()

                   ? new JArray(ChargingTariffs.
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

        #region ToJSON(this ChargingTariffs, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingTariff> ChargingTariffs, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingTariffs?.Any() == true
                       ? new JProperty(JPropertyKey, ChargingTariffs.ToJSON())
                       : null;

        }

        #endregion



        #region ToJSON(this EnergyMix)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        public static JObject ToJSON(this EnergyMix EnergyMix)

            => JSONObject.Create(

                   new JProperty("is_green_energy", EnergyMix.IsGreenEnergy),

                   //new JProperty("energy_sources",  new JArray(
                   //    EnergyMix.EnergySources.SafeSelect(energysource => energysource.ToJSON())
                   //)),

                   //new JProperty("environ_impact",  new JArray(
                   //    EnergyMix.EnvironmentalImpacts.Select(environmentalimpact => environmentalimpact.ToJSON())
                   //)),

                   new JProperty("supplier_name",        EnergyMix.Supplier.     ToJSON()),
                   new JProperty("energy_product_name",  EnergyMix.EnergyProduct.ToJSON())

               );

        #endregion

        #region ToJSON(this TariffElement)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        public static JObject ToJSON(this ChargingTariffElement TariffElement)

            => JSONObject.Create(

                   new JProperty("price_components",
                                 new JArray(TariffElement.
                                                ChargingPriceComponents.
                                                Select(component => component.ToJSON()))),

                   TariffElement.ChargingTariffRestrictions?.Any() == true
                       ? new JProperty("restrictions",
                                       new JArray(TariffElement.
                                                      ChargingTariffRestrictions.
                                                      Select(restriction => restriction.ToJSON())))
                       : null

               );

        #endregion

        #region ToJSON(this TariffRestriction)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        public static JObject ToJSON(this ChargingTariffRestriction TariffRestriction)

            => JSONObject.Create(

                   //new JProperty("type",       _Type.    ToString()),
                   //new JProperty("type", _Type.ToString()),
                   //new JProperty("price",      _Price.   ToString()),

                   TariffRestriction.Time. HasValue && TariffRestriction.Time. Value.StartTime.HasValue ? new JProperty("start_time",  TariffRestriction.Time. Value.StartTime.Value.ToString())       : null,
                   TariffRestriction.Time. HasValue && TariffRestriction.Time. Value.EndTime.  HasValue ? new JProperty("end_time",    TariffRestriction.Time. Value.EndTime.  Value.ToString())       : null,

                   TariffRestriction.kWh.  HasValue && TariffRestriction.kWh.  Value.Min.      HasValue ? new JProperty("min_kWh",     TariffRestriction.kWh.  Value.Min.      Value.ToString("0.00")) : null,
                   TariffRestriction.kWh.  HasValue && TariffRestriction.kWh.  Value.Max.      HasValue ? new JProperty("max_kWh",     TariffRestriction.kWh.  Value.Max.      Value.ToString("0.00")) : null,

                   TariffRestriction.Power.HasValue && TariffRestriction.Power.Value.Min.      HasValue ? new JProperty("min_power",   TariffRestriction.Power.Value.Min.      Value.ToString("0.00")) : null,
                   TariffRestriction.Power.HasValue && TariffRestriction.Power.Value.Max.      HasValue ? new JProperty("max_power",   TariffRestriction.Power.Value.Max.      Value.ToString("0.00")) : null,

                   TariffRestriction.DayOfWeek.Any()
                       ? new JProperty("day_of_week",
                                       new JArray(TariffRestriction.DayOfWeek.Select(day => day.ToString().ToUpper())))
                       : null

               );

        #endregion











        #region ToCSV(this ChargingTariffs, Skip = 0, Take = 0, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingTariffs">An enumeration of charging tariffs.</param>
        /// <param name="Skip">The optional number of charging stations to skip.</param>
        /// <param name="Take">The optional number of charging stations to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static IEnumerable<String[]> GetTariffs(this IEnumerable<ChargingStation> ChargingStations,
                                                       UInt64                            Skip                              = 0,
                                                       UInt64                            Take                              = 0,
                                                       Boolean                           Embedded                          = false,
                                                       InfoStatus                        ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                                       InfoStatus                        ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                                       InfoStatus                        ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                                       InfoStatus                        ExpandEVSEIds                     = InfoStatus.Expand,
                                                       InfoStatus                        ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                                       InfoStatus                        ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => ChargingStations != null && ChargingStations.Any()

                   ? ChargingStations.
                         Where     (station => station != null).
                         OrderBy   (station => station.Id).
                         SkipTakeFilter(Skip, Take).
                         SafeSelectMany(station => {

                             var results = new List<String[]>();

                             foreach (var group in station.Operator.ChargingStationGroups.Where(group => group.Tariff != null))
                                 if (group.AllowedMemberIds.Contains(station.Id) ||
                                     (group.AutoIncludeStations != null && group.AutoIncludeStations(station.Operator.GetChargingStationbyId(station.Id))))
                                     foreach (var evse in station)
                                        results.Add(new String[] {
                                                        evse.Id.                            ToString(),
                                                     //   station.Brand.Name.                 FirstText(),
                                                        station.Name.                       FirstText(),
                                                        station.Address.Street,
                                                        station.Address.HouseNumber,
                                                        station.Address.PostalCode,
                                                        station.Address.City.               FirstText(),
                                                        station.Address.Country.CountryName.FirstText(),
                                                        evse.MaxPower.                      ToString() + " kW",
                                                        evse.SocketOutlets.First().Plug.    ToString(),
                                                        group.Tariff.Name.                  FirstText()
                                                    });

                             foreach (var evse in station)
                                 foreach (var group in evse.Operator.EVSEGroups.Where(group => group.Tariff != null))
                                     if (group.AllowedMemberIds.Contains(evse.Id) ||
                                         (group.AutoIncludeStations != null && group.AutoIncludeStations(evse.Operator.GetEVSEbyId(evse.Id))))
                                         results.Add(new String[] {
                                                         evse.Id.                            ToString(),
                                                      //   station.Brand.Name.                 FirstText(),
                                                         station.Name.                       FirstText(),
                                                         station.Address.Street,
                                                         station.Address.HouseNumber,
                                                         station.Address.PostalCode,
                                                         station.Address.City.               FirstText(),
                                                         station.Address.Country.CountryName.FirstText(),
                                                         evse.MaxPower.                      ToString() + " kW",
                                                         evse.SocketOutlets.First().Plug.    ToString(),
                                                         group.Tariff.Name.                  FirstText()
                                                     });

                             if (results.Count == 0)
                                 foreach (var evse in station)
                                     results.Add(new String[] {
                                                     evse.Id.                            ToString(),
                                                   //  station.Brand.Name.                 FirstText(),
                                                     station.Name.                       FirstText(),
                                                     station.Address.Street,
                                                     station.Address.HouseNumber,
                                                     station.Address.PostalCode,
                                                     station.Address.City.               FirstText(),
                                                     station.Address.Country.CountryName.FirstText(),
                                                     evse.MaxPower.                      ToString() + " kW",
                                                     evse.SocketOutlets.First().Plug.    ToString(),
                                                     "-"
                                                 });

                             return results;

                         })

                   : new List<String[]>();

        #endregion


    }

}
