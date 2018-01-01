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

        #region ToJSON(this OpeningTimes)

        public static JObject ToJSON(this OpeningTimes OpeningTimes)
        {

            var JO = new JObject();
            //OpeningTimes.RegularHours.ForEach(rh => JO.Add(rh.Weekday.ToString(), new JObject(new JProperty("from", rh.Begin.ToString()), new JProperty("to", rh.End.ToString()))));
            OpeningTimes.RegularOpenings.ForEach(rh => JO.Add(rh.DayOfWeek.ToString(), new JArray(rh.PeriodBegin.ToString(), rh.PeriodEnd.ToString())));
            if (OpeningTimes.FreeText.IsNotNullOrEmpty())
                JO.Add("Text", OpeningTimes.FreeText);

            return (OpeningTimes != null)
                       ? OpeningTimes.IsOpen24Hours
                             ? new JObject()
                             : JO
                       : null;

        }

        #endregion

        #region ToJSON(this OpeningTimes, JPropertyKey)

        public static JProperty ToJSON(this OpeningTimes OpeningTimes, String JPropertyKey)

            => OpeningTimes != null
                   ? OpeningTimes.IsOpen24Hours
                         ? new JProperty(JPropertyKey, "24/7")
                         : new JProperty(JPropertyKey, OpeningTimes.ToJSON())
                   : null;

        #endregion

        #region ToJSON(this GridConnection, JPropertyKey)

        public static JProperty ToJSON(this GridConnectionTypes GridConnection, String JPropertyKey)

            => GridConnection != GridConnectionTypes.Unknown
                   ? new JProperty(JPropertyKey,
                                   GridConnection.ToString())
                   : null;

        #endregion

        #region ToJSON(this ChargingStationUIFeatures, JPropertyKey)

        public static JProperty ToJSON(this UIFeatures ChargingStationUIFeatures, String JPropertyKey)

            => new JProperty(JPropertyKey,
                             ChargingStationUIFeatures.ToString());

        #endregion

        #region ToJSON(this AuthenticationModes, JPropertyKey)

        public static JProperty ToJSON(this ReactiveSet<AuthenticationModes> AuthenticationModes, String JPropertyKey)

            => AuthenticationModes != null
                   ? new JProperty(JPropertyKey,
                                   new JArray(AuthenticationModes.SafeSelect(mode => mode.ToJSON())))
                   : null;

        #endregion

        #region ToJSON(this Brand)

        public static JObject ToJSON(this Brand               Brand,
                                     Boolean                  Embedded                   = false,
                                     InfoStatus               ExpandChargingPoolIds      = InfoStatus.Hidden,
                                     InfoStatus               ExpandChargingStationIds   = InfoStatus.Hidden,
                                     InfoStatus               ExpandEVSEIds              = InfoStatus.Hidden,
                                     InfoStatus               ExpandDataLicenses         = InfoStatus.ShowIdOnly)

            => Brand != null
                   ? JSONObject.Create(

                         new JProperty("Id",    Brand.Id.  ToString()),
                         new JProperty("Name",  Brand.Name.ToJSON()),

                         Brand.LogoURI.IsNotNullOrEmpty()
                             ? new JProperty("LogoURI",   Brand.LogoURI)
                             : null,

                         Brand.Homepage.IsNotNullOrEmpty()
                             ? new JProperty("Homepage",  Brand.Homepage)
                             : null

                     )
                   : null;

        #endregion

        #region ToJSON(this Brands, Skip = 0, Take = 0, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of brands.
        /// </summary>
        /// <param name="Brands">An enumeration of brands.</param>
        /// <param name="Skip">The optional number of charging station operators to skip.</param>
        /// <param name="Take">The optional number of charging station operators to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure.</param>
        public static JArray ToJSON(this IEnumerable<Brand>   Brands,
                                    UInt64                    Skip                       = 0,
                                    UInt64                    Take                       = 0,
                                    Boolean                   Embedded                   = false,
                                    InfoStatus                ExpandChargingPoolIds      = InfoStatus.Hidden,
                                    InfoStatus                ExpandChargingStationIds   = InfoStatus.Hidden,
                                    InfoStatus                ExpandEVSEIds              = InfoStatus.Hidden,
                                    InfoStatus                ExpandDataLicenses         = InfoStatus.ShowIdOnly)


            => Brands == null || !Brands.Any()

                   ? new JArray()

                   : new JArray(Brands.
                                    Where         (brand => brand != null).
                                    OrderBy       (brand => brand.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (brand => brand.ToJSON(Embedded,
                                                                         ExpandChargingPoolIds,
                                                                         ExpandChargingStationIds,
                                                                         ExpandEVSEIds,
                                                                         ExpandDataLicenses)));

        #endregion

        #region ToJSON(this Brand, JPropertyKey)

        public static JProperty ToJSON(this Brand Brand, String JPropertyKey)

            => Brand != null
                   ? new JProperty(JPropertyKey, Brand.ToJSON())
                   : null;

        #endregion

    }

}
