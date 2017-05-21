/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using System.Globalization;

#endregion

namespace org.GraphDefined.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this Address)

        public static JObject ToJSON(this Address _Address)

            => _Address != null
                   ? JSONObject.Create(
                         _Address.FloorLevel         .ToJSON("floorLevel"),
                         _Address.HouseNumber        .ToJSON("houseNumber"),
                         _Address.Street             .ToJSON("street"),
                         _Address.PostalCode         .ToJSON("postalCode"),
                         _Address.PostalCodeSub      .ToJSON("postalCodeSub"),
                         _Address.City               .ToJSON("city"),
                         _Address.Country != null
                              ? _Address.Country.CountryName.ToJSON("country")
                              : null
                     )
                   : null;

        #endregion

        #region ToJSON(this Address, JPropertyKey)

        public static JProperty ToJSON(this Address Address, String JPropertyKey)

            => Address != null
                   ? new JProperty(JPropertyKey,
                                   Address.ToJSON())
                   : null;

        #endregion

        #region ToJSON(this Addresses, JPropertyKey)

        public static JArray ToJSON(this IEnumerable<Address> Addresses)

            => Addresses != null && Addresses.Any()
                   ? new JArray(Addresses.SafeSelect(v => v.ToJSON()))
                   : null;

        #endregion

        #region ToJSON(this Addresses, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<Address> Addresses, String JPropertyKey)

            => Addresses != null
                   ? new JProperty(JPropertyKey,
                                   Addresses.ToJSON())
                   : null;

        #endregion

    }

}
