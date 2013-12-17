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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.emi3group;

#endregion

namespace org.emi3group.IO.GeoJSON
{

    // {
    //   "type": "FeatureCollection",
    //   "properties": {
    //     "aaa": "abcde"
    //   },
    //   "features": [
    //     {
    //       "type": "Feature",
    //       "properties": {},
    //       "geometry": {
    //         "type": "Point",
    //         "coordinates": [
    //           10.579833984375,
    //           50.48197825997291
    //         ]
    //       }
    //     },
    //     {
    //       "type": "Feature",
    //       "properties": {
    //         "qqqqqq": 123
    //       },
    //       "geometry": {
    //         "type": "Point",
    //         "coordinates": [
    //           10.70068359375,
    //           50.466246274560476
    //         ]
    //       }
    //     }
    //   ]
    // }

    /// <summary>
    /// GeoJSON export utilities.
    /// </summary>
    public static class GeoJSONExport
    {

        public static JObject Generate(IEnumerable<JObject> JSONObjects, DateTime LastChange)
        {
            return new JObject(new JProperty("@context",   new JObject()),
                               new JProperty("LastChange", LastChange.ToString()),
                               new JProperty("type",       "FeatureCollection"),
                               new JProperty("features",   new JArray(JSONObjects)));
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
                                     new JProperty(v.Key.ToString(), v.Value)
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
                                 GeoJSON.Create(
                                        (Location.Model != GravitationalModel.EGM2008) ? new JProperty("Model",    Location.Model)    : null,
                                        new JProperty("Latitude",  Location.Latitude),
                                        new JProperty("Longitude", Location.Longitude),
                                        (Location.Altitude != 0.0)                     ? new JProperty("Altitude", Location.Altitude) : null)
                                );

        }

        #endregion


        #region ToGeoJSON(this Pool)

        public static JObject ToGeoJSONFeature(this ChargingStation ChargingStation,
                                               Action<ChargingStation, Dictionary<String, String>> AdditionalProperties = null)
        {

            var AdditionalPropertyList = new Dictionary<String, String>();

            if (AdditionalProperties != null)
                AdditionalProperties(ChargingStation, AdditionalPropertyList);

            return new JObject(new JProperty("type",        "Feature"),
                               new JProperty("properties",  new JObject(
                                   new JProperty("LastChange",              ChargingStation.LastChange),
                                   new JProperty("UserComment",             ""),
                                   new JProperty("ServiceProviderComment",  ""),
                                   new JProperty("Features",                new JArray()),
                                   new JProperty("AuthorizationOptions",    new JArray()),
                                   new JProperty("Photos",                  new JArray()),
                                   new JProperty("EVSEs",                   new JObject(
                                       ChargingStation.EVSEs.Select(evse => new JProperty(evse.Id.ToString(), "lala"))
                                   )),
                                   AdditionalPropertyList.Select(v => new JProperty(v.Key, v.Value))
                                   )
                               ),
                               new JProperty("geometry",    new JObject(
                                   new JProperty("type",        "Point"),
                                   new JProperty("coordinates", new JArray(ChargingStation.GeoLocation.Longitude,
                                                                           ChargingStation.GeoLocation.Latitude))
                               )));

        }

        #endregion

        #region ToGeoJSON(this Operator)

        public static JObject ToGeoJSON(this EVSEOperator Operator)
        {

            var ChargeStations = new List<JObject>();

            foreach (var EVSPool in Operator)
                foreach (var ChargeStation in EVSPool)
                    ChargeStations.Add(ChargeStation.ToGeoJSONFeature((st2, l) => {  }));

            return GeoJSONExport.Generate(ChargeStations, Operator.LastChange);

        }

        #endregion

        #region ToGeoJSON(this Operators)

        public static JObject ToGeoJSON(this IEnumerable<EVSEOperator> Operators)
        {

            var ChargeStations = new List<JObject>();

            foreach (var Operator in Operators)
                foreach (var EVSPool in Operator)
                    foreach (var ChargeStation in EVSPool)
                        ChargeStations.Add(ChargeStation.ToGeoJSONFeature((st2, l) => {  }));

            return GeoJSONExport.Generate(ChargeStations, DateTime.Now);

        }

        #endregion

    }

}
