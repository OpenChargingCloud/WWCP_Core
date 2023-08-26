/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An energy mix prognosis.
    /// </summary>
    public class EnergyMixPrognosis : IEquatable<EnergyMixPrognosis>
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext = "https://open.charging.cloud/contexts/wwcp+json/EnergyMixPrognosis";

        #endregion

        #region Properties

        /// <summary>
        /// The timestamp of this energy mix prognosis.
        /// </summary>
        public DateTime                                                        Timestamp               { get; }

        /// <summary>
        /// The timestamped energy sources.
        /// </summary>
        public IEnumerable<Timestamped<PercentageOf<EnergySourceCategories>>>  EnergySources           { get; }

        /// <summary>
        /// The timestamped environmental impacts.
        /// </summary>
        public IEnumerable<Timestamped<PercentageOf<EnvironmentalImpacts>>>    EnvironmentalImpacts    { get; }

        /// <summary>
        /// The name or brand of the energy supplier.
        /// </summary>
        public I18NString                                                      SupplierName            { get; }

        /// <summary>
        /// The name or brand of the energy product.
        /// </summary>
        public I18NString                                                      ProductName             { get; }

        /// <summary>
        /// Optional additional remarks.
        /// </summary>
        public I18NString?                                                     AdditionalRemarks       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy mix prognosis.
        /// </summary>
        /// <param name="EnergySources">The timestamped energy sources.</param>
        /// <param name="EnvironmentalImpacts">The timestamped environmental impacts.</param>
        /// <param name="SupplierName">The name or brand of the energy supplier.</param>
        /// <param name="ProductName">The name or brand of the energy product.</param>
        /// <param name="Timestamp">The timestamp of this energy mix prognosis.</param>
        /// <param name="AdditionalRemarks">Optional additional remarks.</param>
        public EnergyMixPrognosis(IEnumerable<Timestamped<PercentageOf<EnergySourceCategories>>>  EnergySources,
                                  IEnumerable<Timestamped<PercentageOf<EnvironmentalImpacts>>>    EnvironmentalImpacts,
                                  I18NString                                                      SupplierName,
                                  I18NString                                                      ProductName,
                                  DateTime?                                                       Timestamp           = null,
                                  I18NString?                                                     AdditionalRemarks   = null)
        {

            if (!EnergySources.Any())
                throw new ArgumentException("The given energy sources must not be empty!",
                                            nameof(EnergySources));

            if (!EnvironmentalImpacts.Any())
                throw new ArgumentException("The given environmental impacts must not be empty!",
                                            nameof(EnergySources));

            this.EnergySources         = EnergySources;
            this.EnvironmentalImpacts  = EnvironmentalImpacts;
            this.SupplierName          = SupplierName;
            this.ProductName           = ProductName;
            this.Timestamp             = Timestamp ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.AdditionalRemarks     = AdditionalRemarks;

        }

        #endregion


        #region ToJSON(Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        /// <param name="CustomEnergyMixPrognosisSerializer">A delegate to serialize custom energy mix prognosis JSON objects.</param>
        public JObject ToJSON(Boolean                                               Embedded                             = false,
                              CustomJObjectSerializerDelegate<EnergyMixPrognosis>?  CustomEnergyMixPrognosisSerializer   = null)
        {

            var json = JSONObject.Create(

                           !Embedded
                               ? new JProperty("@context",            JSONLDContext)
                               : null,

                           new JProperty("energySources",         new JArray(
                               EnergySources.       Select(energysource        => new JObject(
                                                                                      new JProperty("timestamp",   energysource.       Timestamp.  ToIso8601()),
                                                                                      new JProperty("category",    energysource.       Value.Value.ToString()),
                                                                                      new JProperty("percentage",  energysource.       Value.Percent)
                                                                                  ))),

                           new JProperty("environmentalImpacts",  new JArray(
                               EnvironmentalImpacts.Select(environmentalImpact => new JObject(
                                                                                      new JProperty("timestamp",   environmentalImpact.Timestamp.  ToIso8601()),
                                                                                      new JProperty("impact",      environmentalImpact.Value.Value.ToString()),
                                                                                      new JProperty("percentage",  environmentalImpact.Value.Percent)
                                                                                  )))
                           )),

                           new JProperty("supplierName",              SupplierName.ToJSON()),
                           new JProperty("productName",               ProductName. ToJSON()),

                           AdditionalRemarks is not null
                               ? new JProperty("additionalRemarks",   AdditionalRemarks.ToJSON())
                               : null

                       );

            return CustomEnergyMixPrognosisSerializer is not null
                       ? CustomEnergyMixPrognosisSerializer(this, json)
                       : json;

        }

        #endregion


        #region IEquatable<EnergyMixPrognosis> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy mix prognoses for equality.
        /// </summary>
        /// <param name="Object">An energy mix prognosis to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMixPrognosis energyMixPrognosis &&
                   Equals(energyMixPrognosis);

        #endregion

        #region Equals(energyMixPrognosis)

        /// <summary>
        /// Compares two energy mix prognoses for equality.
        /// </summary>
        /// <param name="EnergyMixPrognosis">An energy mix prognosis to compare with.</param>
        public Boolean Equals(EnergyMixPrognosis? EnergyMixPrognosis)

            => EnergyMixPrognosis is not null &&

               Timestamp.                   Equals(EnergyMixPrognosis.Timestamp)    &&
               SupplierName.                Equals(EnergyMixPrognosis.SupplierName) &&
               ProductName.                 Equals(EnergyMixPrognosis.ProductName)  &&

               EnergySources.       Count().Equals(EnergyMixPrognosis.EnergySources.       Count()) &&
               EnvironmentalImpacts.Count().Equals(EnergyMixPrognosis.EnvironmentalImpacts.Count()) &&

               EnergySources.       All(energySource        => EnergyMixPrognosis.EnergySources.       Contains(energySource))        &&
               EnvironmentalImpacts.All(environmentalImpact => EnergyMixPrognosis.EnvironmentalImpacts.Contains(environmentalImpact)) &&

               ((AdditionalRemarks is null     && EnergyMixPrognosis.AdditionalRemarks is null) ||
                (AdditionalRemarks is not null && EnergyMixPrognosis.AdditionalRemarks is not null && AdditionalRemarks.Equals(EnergyMixPrognosis.AdditionalRemarks)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                // ToDo: Add EnergySources & EnvironmentalImpacts
                return Timestamp.          GetHashCode() * 7 ^
                       SupplierName.       GetHashCode() * 5 ^
                       ProductName.        GetHashCode() * 3 ^
                       (AdditionalRemarks?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("'", ProductName.FirstText(), "' from '", SupplierName.FirstText(), "' @ ", Timestamp.ToIso8601());

        #endregion

    }

}
