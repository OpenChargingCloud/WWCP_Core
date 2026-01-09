/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A charging tariff element.
    /// </summary>
    public struct ChargingTariffElement
    {

        #region Properties

        /// <summary>
        /// Enumeration of price components that make up the pricing of this tariff.
        /// </summary>
        public IEnumerable<ChargingPriceComponent>     ChargingPriceComponents       { get; }

        /// <summary>
        /// Enumeration of tariff restrictions.
        /// </summary>
        public IEnumerable<ChargingTariffRestriction>  ChargingTariffRestrictions    { get;  }

        #endregion

        #region Constructor(s)

        #region ChargingTariffElement(params ChargingPriceComponents)

        /// <summary>
        /// Create a new charging tariff element.
        /// </summary>
        /// <param name="ChargingPriceComponents">Enumeration of price components that make up the pricing of this tariff.</param>
        public ChargingTariffElement(params ChargingPriceComponent[] ChargingPriceComponents)

            : this(ChargingPriceComponents,
                   Array.Empty<ChargingTariffRestriction>())

        { }

        #endregion

        #region ChargingTariffElement(ChargingPriceComponents, ChargingTariffRestrictions = null)

        /// <summary>
        /// Create a new charging tariff element.
        /// </summary>
        /// <param name="ChargingPriceComponents">Enumeration of price components that make up the pricing of this tariff.</param>
        /// <param name="ChargingTariffRestrictions">Enumeration of tariff restrictions.</param>
        public ChargingTariffElement(IEnumerable<ChargingPriceComponent>      ChargingPriceComponents,
                                     IEnumerable<ChargingTariffRestriction>?  ChargingTariffRestrictions = null)
        {

            #region Initial checks

            if (!ChargingPriceComponents.Any())
                throw new ArgumentNullException(nameof(ChargingPriceComponents),  "The given enumeration must not be empty!");

            #endregion

            this.ChargingPriceComponents     = ChargingPriceComponents;
            this.ChargingTariffRestrictions  = ChargingTariffRestrictions ?? Array.Empty<ChargingTariffRestriction>();

        }

        #endregion

        #region ChargingTariffElement(ChargingPriceComponent, ChargingTariffRestriction)

        /// <summary>
        /// Create a new charging tariff element.
        /// </summary>
        /// <param name="ChargingPriceComponent">A price component that makes up the pricing of this tariff.</param>
        /// <param name="ChargingTariffRestriction">A tariff restriction.</param>
        public ChargingTariffElement(ChargingPriceComponent     ChargingPriceComponent,
                                     ChargingTariffRestriction  ChargingTariffRestriction)
        {

            this.ChargingPriceComponents     = new[] { ChargingPriceComponent };
            this.ChargingTariffRestrictions  = new[] { ChargingTariffRestriction };

        }

        #endregion

        #endregion


        #region ToJSON()

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        public JObject ToJSON()

            => JSONObject.Create(

                   new JProperty("priceComponents",
                                 new JArray(ChargingPriceComponents.
                                                Select(component => component.ToJSON()))),

                   ChargingTariffRestrictions?.Any() == true
                       ? new JProperty("restrictions",
                                       new JArray(ChargingTariffRestrictions.
                                                      Select(restriction => restriction.ToJSON())))
                       : null

               );

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this object.
        /// </summary>
        public ChargingTariffElement Clone()

            => new (ChargingPriceComponents.   Select(chargingPriceComponents    => chargingPriceComponents.   Clone()),
                    ChargingTariffRestrictions.Select(chargingTariffRestrictions => chargingTariffRestrictions.Clone()));

        #endregion

    }

}
