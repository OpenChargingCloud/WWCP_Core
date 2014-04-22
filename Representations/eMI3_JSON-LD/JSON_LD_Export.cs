/*
 * Copyright (c) 2013 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 Mockup <http://www.github.com/eMI3/Mockup>
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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using org.emi3group;
using Newtonsoft.Json.Linq;

#endregion

namespace org.emi3group.IO.JSON_LD
{

    /// <summary>
    /// JSON export utilities.
    /// </summary>
    public static class JSONExport
    {

        public static JObject Generate(JObject JSONObject = null)
        {

            var JSON = new JObject(new JProperty("@context", null),
                                   new JProperty("eMI3",     JSONObject));

            return JSON;

        }


        #region ToJSON(this Object, JPropertyKey)

        /// <summary>
        /// Create a JSON representation of the given object..
        /// </summary>
        /// <param name="Object">An object.</param>
        /// <param name="JPropertyKey">The name of the JSON property key.</param>
        public static JProperty ToJSON(this Object Object, String JPropertyKey)
        {
            return new JProperty(JPropertyKey, Object.ToString());
        }

        #endregion

        #region ToJSON(this Timestamp, JPropertyKey, Format = "yyyyMMdd HHmmss")

        /// <summary>
        /// Create a JSON representation of the given DateTime.
        /// </summary>
        /// <param name="Timestamp">A timestamp.</param>
        /// <param name="JPropertyKey">The name of the JSON property key.</param>
        /// <param name="Format">A standard or custom date and time format string.</param>
        public static JProperty ToJSON(this DateTime Timestamp, String JPropertyKey, String Format = "yyyyMMdd HHmmss")
        {
            return new JProperty(JPropertyKey, Timestamp.ToUniversalTime().ToString(Format));
        }

        #endregion

        #region ToJSON(this I8N, JPropertyKey)

        /// <summary>
        /// Create a JSON representation of the given internationalized string.
        /// </summary>
        /// <param name="I8N">An internationalized string.</param>
        /// <param name="JPropertyKey">The name of the JSON property key.</param>
        public static JProperty ToJSON(this I8NString I8N, String JPropertyKey)
        {

            return new JProperty(JPropertyKey,
                                 new JObject(I8N.Select(v =>
                                     new JProperty(v.Language.ToString(), v.Value)
                                 )));

        }

        #endregion

        #region ToJSON(this Location, JPropertyKey)

        /// <summary>
        /// Create a JSON representation of the given GeoLocation.
        /// </summary>
        /// <param name="Location">A GeoLocation.</param>
        /// <param name="JPropertyKey">The name of the JSON property key.</param>
        public static JProperty ToJSON(this GeoLocation Location, String JPropertyKey)
        {

            if (Location.Longitude == 0 && Location.Latitude == 0)
                return null;

            return new JProperty(JPropertyKey,
                                 JSON.Create(
                                     (Location.Model != GravitationalModel.EGM2008) ? new JProperty("Model",    Location.Model)    : null,
                                     new JProperty("Latitude",  Location.Latitude),
                                     new JProperty("Longitude", Location.Longitude),
                                     (Location.Altitude != 0.0)                     ? new JProperty("Altitude", Location.Altitude) : null)
                                );

        }

        #endregion


        #region ToJSON(this Pool)

        public static JObject ToJSON(this ChargingPool Pool)
        {

            return JSON.Create(Pool.Id.              ToJSON("Id"),
                               Pool.LastChange.      ToJSON("Timestamp"),
                               Pool.Name.            ToJSON("Name"),
                               (!Pool.Description.IsEmpty)
                                   ? Pool.Description.ToJSON("Description")
                                   : null,
                               Pool.LocationLanguage.ToJSON("LocationLanguage"),
                               Pool.PoolLocation.    ToJSON("PoolLocation"),
                               Pool.EntranceLocation.ToJSON("EntranceLocation")
                              );

        }

        #endregion

    }

}
