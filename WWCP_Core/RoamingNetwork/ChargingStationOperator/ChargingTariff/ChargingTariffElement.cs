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

using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
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
        public IEnumerable<ChargingPriceComponent>     ChargingPriceComponents     { get; }

        /// <summary>
        /// Enumeration of tariff restrictions.
        /// </summary>
        public IEnumerable<ChargingTariffRestriction>  ChargingTariffRestrictions  { get;  }

        #endregion

        #region Constructor(s)

        #region ChargingTariffElement(params ChargingPriceComponents)

        /// <summary>
        /// Create a new charging tariff element.
        /// </summary>
        /// <param name="ChargingPriceComponents">Enumeration of price components that make up the pricing of this tariff.</param>
        public ChargingTariffElement(params ChargingPriceComponent[]  ChargingPriceComponents)

            : this(ChargingPriceComponents, new ChargingTariffRestriction[0])

        { }

        #endregion

        #region ChargingTariffElement(ChargingPriceComponents, ChargingTariffRestrictions = null)

        /// <summary>
        /// Create a new charging tariff element.
        /// </summary>
        /// <param name="ChargingPriceComponents">Enumeration of price components that make up the pricing of this tariff.</param>
        /// <param name="ChargingTariffRestrictions">Enumeration of tariff restrictions.</param>
        public ChargingTariffElement(IEnumerable<ChargingPriceComponent>     ChargingPriceComponents,
                                     IEnumerable<ChargingTariffRestriction>  ChargingTariffRestrictions = null)
        {

            #region Initial checks

            if (ChargingPriceComponents == null)
                throw new ArgumentNullException(nameof(ChargingPriceComponents),  "The given parameter must not be null!");

            if (!ChargingPriceComponents.Any())
                throw new ArgumentNullException(nameof(ChargingPriceComponents),  "The given enumeration must not be empty!");

            #endregion

            this.ChargingPriceComponents     = ChargingPriceComponents;
            this.ChargingTariffRestrictions  = ChargingTariffRestrictions;

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

            #region Initial checks

            if (ChargingTariffRestriction == null)
                throw new ArgumentNullException(nameof(ChargingTariffRestriction),  "The given enumeration must not be empty!");

            #endregion

            this.ChargingPriceComponents     = new ChargingPriceComponent[]    { ChargingPriceComponent };
            this.ChargingTariffRestrictions  = new ChargingTariffRestriction[] { ChargingTariffRestriction };

        }

        #endregion

        #endregion


        #region ToJSON()

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        public JObject ToJSON()

            => JSONObject.Create(

                   new JProperty("price_components",
                                 new JArray(ChargingPriceComponents.
                                                Select(component => component.ToJSON()))),

                   ChargingTariffRestrictions?.Any() == true
                       ? new JProperty("restrictions",
                                       new JArray(ChargingTariffRestrictions.
                                                      Select(restriction => restriction.ToJSON())))
                       : null

               );

        #endregion

    }

}
