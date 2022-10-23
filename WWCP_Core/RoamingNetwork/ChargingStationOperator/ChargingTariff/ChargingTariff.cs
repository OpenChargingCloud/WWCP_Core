/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for charging tariffs.
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
        public static JArray ToJSON(this IEnumerable<ChargingTariff>  ChargingTariffs,
                                    UInt64?                           Skip                              = null,
                                    UInt64?                           Take                              = null,
                                    Boolean                           Embedded                          = false,
                                    InfoStatus                        ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                        ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                        ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                        ExpandEVSEIds                     = InfoStatus.Expanded,
                                    InfoStatus                        ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                        ExpandDataLicenses                = InfoStatus.ShowIdOnly)


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
                                     (group.AutoIncludeStations != null && group.AutoIncludeStations(station.Operator.GetChargingStationById(station.Id))))
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
                                 foreach (var group in evse.Operator.EVSEGroups.Where(group => group.Tariff is not null))
                                     if (group.AllowedMemberIds.Contains(evse.Id) ||
                                         (group.AutoIncludeEVSEs != null && group.AutoIncludeEVSEs(evse.Operator.GetEVSEById(evse.Id))))
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


    /// <summary>
    /// A charging tariff to charge an electric vehicle.
    /// </summary>
    public class ChargingTariff : AEMobilityEntity<ChargingTariff_Id,
                                                   ChargingTariffAdminStatusTypes,
                                                   ChargingTariffStatusTypes>,
                                  IEquatable<ChargingTariff>,
                                  IComparable<ChargingTariff>,
                                  IComparable
    {

        #region Properties

        /// <summary>
        /// An optional (multi-language) name of this charging tariff.
        /// </summary>
        [Optional]
        public I18NString                          Name              { get; }

        /// <summary>
        /// An optional (multi-language) description of this charging tariff.
        /// </summary>
        [Optional]
        public I18NString                          Description       { get; }

        /// <summary>
        /// An optional brand for this charging tariff.
        /// </summary>
        [Optional]
        public Brand                               Brand             { get; }

        /// <summary>
        /// An URI for more information about this tariff.
        /// </summary>
        [Optional]
        public Uri                                 TariffURI         { get; }

        /// <summary>
        /// ISO 4217 code of the currency used for this tariff.
        /// </summary>
        [Mandatory]
        public Currency                            Currency          { get; }

        /// <summary>
        /// The energy mix.
        /// </summary>
        [Optional]
        public EnergyMix                           EnergyMix         { get;  }

                /// <summary>
        /// An enumeration of tariff elements.
        /// </summary>
        [Mandatory]
        public IEnumerable<ChargingTariffElement>  TariffElements    { get; }


        /// <summary>
        /// The charging tariff operator of this charging tariff.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator             Operator          { get; }


        public ChargingTariffGroup?                TariffGroup       { get; }

        #endregion

        #region Constructor(s)

        #region ChargingTariff(Id, Operator, ...)

        /// <summary>
        /// Create a new charging tariff having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="Operator">The charging station operator of this charging tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        public ChargingTariff(ChargingTariff_Id                   Id,
                              ChargingStationOperator             Operator,
                              I18NString                          Name,
                              I18NString                          Description,
                              Brand                               Brand,

                              Uri                                 TariffUrl,
                              Currency                            Currency,
                              EnergyMix                           EnergyMix,
                              IEnumerable<ChargingTariffElement>  TariffElements)

            : base(Id)

        {

            #region Initial checks

            if (Operator is null)
                throw new ArgumentNullException(nameof(Operator),        "The given charging station operator must not be null!");

            if (TariffElements is null || !TariffElements.Any())
                throw new ArgumentNullException(nameof(TariffElements),  "The given enumeration of tariff elements must not be null or empty!");

            #endregion

            #region Init data and properties

            this.Operator        = Operator;
            this.Name            = Name;
            this.Description     = Description ?? new I18NString();
            this.Brand           = Brand;

            this.TariffURI       = TariffUrl;
            this.Currency        = Currency;
            this.EnergyMix       = EnergyMix;
            this.TariffElements  = TariffElements;

            #endregion

        }

        #endregion

        #region ChargingTariff(Id, TariffGroup, ...)

        /// <summary>
        /// Create a new charging tariff having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="TariffGroup">The charging tariff group of this charging tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        public ChargingTariff(ChargingTariff_Id                   Id,
                              ChargingTariffGroup                 TariffGroup,
                              I18NString                          Name,
                              I18NString                          Description,
                              Brand                               Brand,

                              Uri                                 TariffUrl,
                              Currency                            Currency,
                              EnergyMix                           EnergyMix,
                              IEnumerable<ChargingTariffElement>  TariffElements)

            : this(Id,
                   TariffGroup.Operator,
                   Name,
                   Description,
                   Brand,

                   TariffUrl,
                   Currency,
                   EnergyMix,
                   TariffElements)

        {

            this.TariffGroup  = TariffGroup;

        }

        #endregion

        #endregion


        #region ToJSON(Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging tariff.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public JObject ToJSON(Boolean              Embedded                          = false,
                              InfoStatus           ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus           ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus           ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                              InfoStatus           ExpandEVSEIds                     = InfoStatus.Expanded,
                              InfoStatus           ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus           ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => JSONObject.Create(

                         new JProperty("@id", Id.ToString()),

                         Embedded
                             ? null
                             : new JProperty("@context", "https://open.charging.cloud/contexts/wwcp+json/ChargingTariff"),

                         Name.       IsNeitherNullNorEmpty()
                             ? new JProperty("name",        Name.ToJSON())
                             : null,

                         Description.IsNeitherNullNorEmpty()
                             ? new JProperty("description", Description.ToJSON())
                             : null,

                         Brand != null
                             ? ExpandBrandIds.Switch(
                                   () => new JProperty("brandId",  Brand.Id.ToString()),
                                   () => new JProperty("brand",    Brand.   ToJSON()))
                             : null,

                         (DataSource is not null && (!Embedded || DataSource != Operator.DataSource))
                             ? new JProperty("dataSource", DataSource)
                             : null,

                         //(!Embedded || DataLicenses != Operator.DataLicenses)
                         //    ? ExpandDataLicenses.Switch(
                         //        () => new JProperty("dataLicenseIds",  new JArray(DataLicenses.SafeSelect(license => license.Id.ToString()))),
                         //        () => new JProperty("dataLicenses",    DataLicenses.ToJSON()))
                         //    : null,


                         new JProperty("currency", Currency.ISOCode),

                         TariffURI is not null
                             ? new JProperty("URI", TariffURI.ToString())
                             : null,

                         TariffElements.Any()
                             ? new JProperty("elements", new JArray(TariffElements.Select(TariffElement => TariffElement.ToJSON())))
                             : null,

                         EnergyMix is not null
                             ? new JProperty("energy_mix", EnergyMix.ToJSON())
                             : null,



                         Operator.ChargingStationGroups.Any(group => group.Tariff == this)
                             ? new JProperty("chargingStations", new JArray(Operator.ChargingStationGroups.
                                                                                Where (group => group.Tariff == this).
                                                                                Select(group => group.AllowedMemberIds.
                                                                                                          Select(id => id.ToString()))))
                             : null,

                         Operator.EVSEGroups.           Any(group => group.Tariff == this)
                             ? new JProperty("EVSEs",            new JArray(Operator.EVSEGroups.
                                                                                Where (group => group.Tariff == this).
                                                                                Select(group => group.AllowedMemberIds.
                                                                                                          Select(id => id.ToString()))))
                             : null






        //                 #region Embedded means it is served as a substructure of e.g. a charging station operator

        //                 Embedded
        //                     ? null
        //                     : ExpandRoamingNetworkId.Switch(
        //                           () => new JProperty("roamingNetworkId",           RoamingNetwork.Id. ToString()),
        //                           () => new JProperty("roamingNetwork",             RoamingNetwork.    ToJSON(Embedded:                          true,
        //                                                                                                                            ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
        //                                                                                                                            ExpandChargingPoolIds:             InfoStatus.Hidden,
        //                                                                                                                            ExpandChargingStationIds:          InfoStatus.Hidden,
        //                                                                                                                            ExpandEVSEIds:                     InfoStatus.Hidden,
        //                                                                                                                            ExpandBrandIds:                    InfoStatus.Hidden,
        //                                                                                                                            ExpandDataLicenses:                InfoStatus.Hidden))),

        //                 Embedded
        //                     ? null
        //                     : ExpandChargingStationOperatorId.Switch(
        //                           () => new JProperty("chargingStationOperatorId",  Operator.Id.       ToString()),
        //                           () => new JProperty("chargingStationOperator",    Operator.          ToJSON(Embedded:                          true,
        //                                                                                                                            ExpandRoamingNetworkId:            InfoStatus.Hidden,
        //                                                                                                                            ExpandChargingPoolIds:             InfoStatus.Hidden,
        //                                                                                                                            ExpandChargingStationIds:          InfoStatus.Hidden,
        //                                                                                                                            ExpandEVSEIds:                     InfoStatus.Hidden,
        //                                                                                                                            ExpandBrandIds:                    InfoStatus.Hidden,
        //                                                                                                                            ExpandDataLicenses:                InfoStatus.Hidden))),

        //                 #endregion

        //                 ExpandEVSEIds.Switch(
        //                     () => new JProperty("EVSEIds",
        //                                         EVSEIds.SafeAny()
        //                                             ? new JArray(EVSEIds.
        //                                                                               OrderBy(evseid => evseid).
        //                                                                               Select (evseid => evseid.ToString()))
        //                                             : null),

        //                     () => new JProperty("EVSEs",
        //                                         EVSEs.SafeAny()
        //                                             ? new JArray(EVSEs.
        //                                                                               OrderBy(evse   => evse.Id).
        //                                                                               Select (evse   => evse.  ToJSON(Embedded: true)))
        //                                             : null))

                        );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingTariff1, ChargingTariff2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingTariff1 == null) || ((Object) ChargingTariff2 == null))
                return false;

            return ChargingTariff1.Equals(ChargingTariff2);

        }

        #endregion

        #region Operator != (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
            => !(ChargingTariff1 == ChargingTariff2);

        #endregion

        #region Operator <  (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
        {

            if ((Object) ChargingTariff1 == null)
                throw new ArgumentNullException(nameof(ChargingTariff1), "The given ChargingTariff1 must not be null!");

            return ChargingTariff1.CompareTo(ChargingTariff2) < 0;

        }

        #endregion

        #region Operator <= (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
            => !(ChargingTariff1 > ChargingTariff2);

        #endregion

        #region Operator >  (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
        {

            if ((Object) ChargingTariff1 == null)
                throw new ArgumentNullException(nameof(ChargingTariff1), "The given ChargingTariff1 must not be null!");

            return ChargingTariff1.CompareTo(ChargingTariff2) > 0;

        }

        #endregion

        #region Operator >= (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
            => !(ChargingTariff1 < ChargingTariff2);

        #endregion

        #endregion

        #region IComparable<ChargingTariff> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object? Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var ChargingTariff = Object as ChargingTariff;
            if ((Object) ChargingTariff == null)
                throw new ArgumentException("The given object is not a charging tariff!", nameof(Object));

            return CompareTo(ChargingTariff);

        }

        #endregion

        #region CompareTo(ChargingTariff)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff">A charging tariff object to compare with.</param>
        public Int32 CompareTo(ChargingTariff ChargingTariff)
        {

            if ((Object) ChargingTariff == null)
                throw new ArgumentNullException(nameof(ChargingTariff), "The given charging tariff must not be null!");

            return Id.CompareTo(ChargingTariff.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingTariff> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            var ChargingTariff = Object as ChargingTariff;
            if ((Object) ChargingTariff == null)
                return false;

            return Equals(ChargingTariff);

        }

        #endregion

        #region Equals(ChargingTariff)

        /// <summary>
        /// Compares two charging tariffs for equality.
        /// </summary>
        /// <param name="ChargingTariff">A charging tariff to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingTariff ChargingTariff)
        {

            if ((Object) ChargingTariff == null)
                return false;

            return Id.Equals(ChargingTariff.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion

    }

}
