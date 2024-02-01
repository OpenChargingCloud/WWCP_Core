/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A charging tariff for charging an electric vehicle.
    /// </summary>
    public class ChargingTariff : AEMobilityEntity<ChargingTariff_Id,
                                                   ChargingTariffAdminStatusTypes,
                                                   ChargingTariffStatusTypes>,
                                  IChargingTariff
    {

        #region Properties

        /// <summary>
        /// The roaming network of this charging tariff.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork?                    RoamingNetwork
            => Operator?.RoamingNetwork;

        /// <summary>
        /// The charging station operator of this charging tariff.
        /// </summary>
        [Optional]
        public IChargingStationOperator?           Operator          { get; }


        /// <summary>
        /// An enumeration of tariff elements.
        /// </summary>
        [Mandatory]
        public IEnumerable<ChargingTariffElement>  TariffElements    { get; }

        /// <summary>
        /// ISO 4217 code of the currency used for this tariff.
        /// </summary>
        [Mandatory]
        public Currency                            Currency          { get; }


        /// <summary>
        /// An optional brand for this charging tariff.
        /// </summary>
        [Optional]
        public Brand?                              Brand             { get; }

        /// <summary>
        /// An URI for more information about this tariff.
        /// </summary>
        [Optional]
        public URL?                                TariffURL         { get; }

        /// <summary>
        /// The energy mix.
        /// </summary>
        [Optional]
        public EnergyMix?                          EnergyMix         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging tariff having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        public ChargingTariff(ChargingTariff_Id                   Id,
                              IChargingStationOperator            Operator,
                              I18NString                          Name,
                              I18NString                          Description,

                              IEnumerable<ChargingTariffElement>  TariffElements,
                              Currency                            Currency,

                              Brand?                              Brand          = null,
                              URL?                                TariffURL      = null,
                              EnergyMix?                          EnergyMix      = null,

                              String?                             DataSource     = null,
                              DateTime?                           LastChange     = null,

                              JObject?                            CustomData     = null,
                              UserDefinedDictionary?              InternalData   = null)

            : base(Id,
                   Name,
                   Description,
                   DataSource:    DataSource,
                   LastChange:    LastChange,
                   CustomData:    CustomData,
                   InternalData:  InternalData)

        {

            #region Initial checks

            if (TariffElements is null || !TariffElements.Any())
                throw new ArgumentNullException(nameof(TariffElements), "The given enumeration of tariff elements must not be null or empty!");

            #endregion

            #region Init data and properties

            this.Operator        = Operator;
            this.TariffElements  = TariffElements;
            this.Currency        = Currency;

            this.Brand           = Brand;
            this.TariffURL       = TariffURL;
            this.EnergyMix       = EnergyMix;

            #endregion

        }

        #endregion


        #region ToJSON(Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charging tariff.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public JObject ToJSON(Boolean     Embedded                          = false,
                              InfoStatus  ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus  ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus  ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                              InfoStatus  ExpandEVSEIds                     = InfoStatus.Expanded,
                              InfoStatus  ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus  ExpandDataLicenses                = InfoStatus.ShowIdOnly)


            => JSONObject.Create(

                               new JProperty("@id",             Id.ToString()),

                         Embedded
                             ? null
                             : new JProperty("@context",        "https://open.charging.cloud/contexts/wwcp+json/ChargingTariff"),

                         Name.IsNotNullOrEmpty()
                             ? new JProperty("name",            Name.ToJSON())
                             : null,

                         Description.IsNotNullOrEmpty()
                             ? new JProperty("description",     Description.ToJSON())
                             : null,

                         Brand != null
                             ? ExpandBrandIds.Switch(
                                   () => new JProperty("brandId", Brand.Id.ToString()),
                                   () => new JProperty("brand", Brand.ToJSON()))
                             : null,

                         (DataSource is not null && !Embedded)
                             ? new JProperty("dataSource",      DataSource)
                             : null,


                         TariffElements.Any()
                             ? new JProperty("elements",        new JArray(TariffElements.Select(tariffElement => tariffElement.ToJSON())))
                             : null,

                               new JProperty("currency",        Currency.ISOCode),




                         TariffURL is not null
                             ? new JProperty("URI",             TariffURL.ToString())
                             : null,

                         EnergyMix is not null
                             ? new JProperty("energy_mix",      EnergyMix.ToJSON())
                             : null

                        );

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this object.
        /// </summary>
        public IChargingTariff Clone()

            => new ChargingTariff(Id,
                                  Operator,
                                  Name,
                                  Description,

                                  TariffElements.Select(tariffElements => tariffElements.Clone()),
                                  Currency,

                                  Brand,
                                  TariffURL,
                                  EnergyMix,

                                  DataSource,
                                  LastChangeDate,

                                  CustomData,
                                  InternalData);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingTariff ChargingTariff1,
                                           ChargingTariff ChargingTariff2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingTariff1, ChargingTariff2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingTariff1 is null || ChargingTariff2 is null)
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
        public static Boolean operator != (ChargingTariff ChargingTariff1,
                                           ChargingTariff ChargingTariff2)

            => !(ChargingTariff1 == ChargingTariff2);

        #endregion

        #region Operator <  (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingTariff ChargingTariff1,
                                          ChargingTariff ChargingTariff2)
        {

            if (ChargingTariff1 is null)
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
        public static Boolean operator <= (ChargingTariff ChargingTariff1,
                                           ChargingTariff ChargingTariff2)

            => !(ChargingTariff1 > ChargingTariff2);

        #endregion

        #region Operator >  (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingTariff ChargingTariff1,
                                          ChargingTariff ChargingTariff2)
        {

            if (ChargingTariff1 is null)
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
        public static Boolean operator >= (ChargingTariff ChargingTariff1,
                                           ChargingTariff ChargingTariff2)

            => !(ChargingTariff1 < ChargingTariff2);

        #endregion

        #endregion

        #region IComparable<ChargingTariff> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging tariffs.
        /// </summary>
        /// <param name="Object">A charging tariff to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is ChargingTariff chargingTariff
                   ? CompareTo(chargingTariff)
                   : throw new ArgumentException("The given object is not a charging tariff!", nameof(Object));

        #endregion

        #region CompareTo(ChargingTariff)

        /// <summary>
        /// Compares two charging tariffs.
        /// </summary>
        /// <param name="ChargingTariff">A charging tariff to compare with.</param>
        public Int32 CompareTo(ChargingTariff? ChargingTariff)

            => ChargingTariff is not null
                   ? Id.CompareTo(ChargingTariff.Id)
                   : throw new ArgumentException("The given object is not a ChargingTariff!", nameof(ChargingTariff));

        /// <summary>
        /// Compares two charging tariffs.
        /// </summary>
        /// <param name="IChargingTariff">A charging tariff to compare with.</param>
        public Int32 CompareTo(IChargingTariff? IChargingTariff)

            => IChargingTariff is not null
                   ? Id.CompareTo(IChargingTariff.Id)
                   : throw new ArgumentException("The given object is not a IChargingTariff!", nameof(IChargingTariff));

        #endregion

        #endregion

        #region IEquatable<ChargingTariff> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging tariffs for equality.
        /// </summary>
        /// <param name="Object">A charging tariff to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingTariff chargingTariff &&
                   Equals(chargingTariff);

        #endregion

        #region Equals(ChargingTariff)

        /// <summary>
        /// Compares two charging tariffs for equality.
        /// </summary>
        /// <param name="ChargingTariff">A charging tariff to compare with.</param>
        public Boolean Equals(ChargingTariff? ChargingTariff)

            => ChargingTariff is not null &&
               Id.Equals(ChargingTariff.Id);

        /// <summary>
        /// Compares two charging tariffs for equality.
        /// </summary>
        /// <param name="IChargingTariff">A charging tariff to compare with.</param>
        public Boolean Equals(IChargingTariff? IChargingTariff)

            => IChargingTariff is not null &&
               Id.Equals(IChargingTariff.Id);

        #endregion

        #endregion

        #region (override) GetHashCode()

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
