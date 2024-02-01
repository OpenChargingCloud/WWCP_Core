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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An energy mix.
    /// </summary>
    public class EnergyMix : IEquatable<EnergyMix>
    {

        #region Properties

        /// <summary>
        /// The energy sources.
        /// </summary>
        public IEnumerable<PercentageOf<EnergySourceCategories>>  EnergySources           { get; }

        /// <summary>
        /// The environmental impacts.
        /// </summary>
        public IEnumerable<PercentageOf<EnvironmentalImpacts>>    EnvironmentalImpacts    { get; }

        /// <summary>
        /// The name or brand of the energy supplier.
        /// </summary>
        public I18NString                                         SupplierName            { get; }

        /// <summary>
        /// The name or brand of the energy product.
        /// </summary>
        public I18NString                                         ProductName             { get; }

        /// <summary>
        /// Optional additional remarks.
        /// </summary>
        public I18NString?                                        AdditionalRemarks       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy mix.
        /// </summary>
        /// <param name="EnergySources">The energy sources.</param>
        /// <param name="EnvironmentalImpacts">The environmental impacts.</param>
        /// <param name="SupplierName">The name or brand of the energy supplier.</param>
        /// <param name="ProductName">The name or brand of the energy product.</param>
        /// <param name="AdditionalRemarks">Optional additional remarks.</param>
        public EnergyMix(IEnumerable<PercentageOf<EnergySourceCategories>>  EnergySources,
                         IEnumerable<PercentageOf<EnvironmentalImpacts>>    EnvironmentalImpacts,
                         I18NString                                         SupplierName,
                         I18NString                                         ProductName,
                         I18NString?                                        AdditionalRemarks   = null)
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
            this.AdditionalRemarks     = AdditionalRemarks;

        }

        #endregion


        #region ToJSON(this EnergyMix)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        public JObject ToJSON()

            => JSONObject.Create(

                         //new JProperty("energy_sources",  new JArray(
                         //    EnergyMix.EnergySources.SafeSelect(energysource => energysource.ToJSON())
                         //)),

                         //new JProperty("environ_impact",  new JArray(
                         //    EnergyMix.EnvironmentalImpacts.Select(environmentalimpact => environmentalimpact.ToJSON())
                         //)),

                         new JProperty("supplierName",  SupplierName.ToJSON()),
                         new JProperty("productName",   ProductName. ToJSON()),

                   AdditionalRemarks is not null
                       ? new JProperty("additionalRemarks",  AdditionalRemarks.ToJSON())
                       : null

               );

        #endregion


        #region IEquatable<EnergyMix> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy mixes for equality.
        /// </summary>
        /// <param name="Object">An energy mix to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMix energyMix &&
                   Equals(energyMix);

        #endregion

        #region Equals(EnergyMix)

        /// <summary>
        /// Compares two energy mixes for equality.
        /// </summary>
        /// <param name="EnergyMix">An energy mix to compare with.</param>
        public Boolean Equals(EnergyMix? EnergyMix)

            => EnergyMix is not null &&

               SupplierName.       Equals(EnergyMix.SupplierName) &&
               ProductName.        Equals(EnergyMix.ProductName)  &&

               ((AdditionalRemarks is null     && EnergyMix.AdditionalRemarks is null) ||
                (AdditionalRemarks is not null && EnergyMix.AdditionalRemarks is not null && AdditionalRemarks.Equals(EnergyMix.AdditionalRemarks)));

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

                return SupplierName.       GetHashCode() * 7 ^
                       ProductName.        GetHashCode() * 5 ^
                       (AdditionalRemarks?.GetHashCode() ?? 0);

            }
        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("'", ProductName.FirstText(), "' from '", SupplierName.FirstText(), "'");

        #endregion

    }

}
