﻿/*
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

using org.GraphDefined.Vanaheimr.Illias;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class TimestampedMap<TKey, TValue>
    {

        public DateTime                                 Timestamp   { get; }
        public IEnumerable<KeyValuePair<TKey, TValue>>  Map         { get; }

        public TimestampedMap(DateTime                                 Timestamp,
                              IEnumerable<KeyValuePair<TKey, TValue>>  Map)
        {

            this.Timestamp  = Timestamp;
            this.Map        = Map;

        }

    }


    /// <summary>
    /// A socket outlet to connect an electric vehicle (EV)
    /// to an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public class EnergyMix
    {

        #region Properties

        public Boolean                                         IsGreenEnergy           { get; }

        public TimestampedMap<EnergySourceCategories, Single>  EnergySources           { get; }

        public TimestampedMap<EnvironmentalImpacts,   Single>  EnvironmentalImpacts    { get; }

        public I18NString                                      Supplier                { get; }

        public I18NString                                      EnergyProduct           { get; }

        public GridConnectionTypes?                            GridConnection          { get; }

        #endregion


        #region Constructor(s)

        /// <summary>
        /// Create a new energy sources overview.
        /// </summary>
        public EnergyMix()
        {

            

        }

        #endregion


        #region ToJSON(this EnergyMix)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        public JObject ToJSON()

            => JSONObject.Create(

                   new JProperty("is_green_energy", IsGreenEnergy),

                   //new JProperty("energy_sources",  new JArray(
                   //    EnergyMix.EnergySources.SafeSelect(energysource => energysource.ToJSON())
                   //)),

                   //new JProperty("environ_impact",  new JArray(
                   //    EnergyMix.EnvironmentalImpacts.Select(environmentalimpact => environmentalimpact.ToJSON())
                   //)),

                   new JProperty("supplier_name",        Supplier.     ToJSON()),
                   new JProperty("energy_product_name",  EnergyProduct.ToJSON())

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("");

        #endregion

    }

}
