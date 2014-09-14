/*
 * Copyright (c) 2013 Achim Friedland <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using eu.Vanaheimr.Illias.Commons;
using com.graphdefined.eMI3;

#endregion

namespace com.graphdefined.eMI3.IO.JSON_LD
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



        #region ToJSON(this I8NString)

        public static JObject ToJSON(this I8NString I8NString)
        {
            return new JObject(I8NString.SafeSelect(v => new JProperty(v.Language.ToString(), v.Value.ToString())));
        }

        #endregion

        #region ToJSON(this GeoLocation)

        public static JObject ToJSON(this GeoLocation GeoLocation)
        {

            return new JObject(new JProperty("lat",             GeoLocation.Latitude.ToString()),
                               new JProperty("lng",             GeoLocation.Longitude.ToString()),
                               new JProperty("projection",      "WGS84"));

        }

        #endregion

        #region ToJSON(this Address)

        public static JObject ToJSON(this Address Address)
        {

            return new JObject(new JProperty("floorLevel",      Address.FloorLevel),
                               new JProperty("houseNumber",     Address.HouseNumber),
                               new JProperty("street",          Address.Street),
                               new JProperty("postalCode",      Address.PostalCode),
                               new JProperty("postalCodeSub",   Address.PostalCodeSub),
                               new JProperty("city",            Address.City),
                               new JProperty("country",         Address.Country.CountryName.ToJSON()));

        }

        #endregion

        #region ToJSON(this OpeningTime)

        public static JObject ToJSON(this OpeningTime OpeningTime)
        {

            return new JObject(new JProperty("OpeningTime", OpeningTime.ToString()));

        }

        #endregion

        #region ToJSON(this GridConnection)

        public static JObject ToJSON(this GridConnection GridConnection)
        {

            return new JObject(new JProperty("GridConnection", GridConnection.ToString()));

        }

        #endregion

        #region ToJSON(this ChargingStationUIFeatures)

        public static JObject ToJSON(this ChargingStationUIFeatures ChargingStationUIFeatures)
        {

            return new JObject(new JProperty("ChargingStationUIFeatures", ChargingStationUIFeatures.ToString()));

        }

        #endregion

        #region ToJSON(this ChargingStationUIFeatures)

        public static JObject ToJSON(this AuthorizationOptions AuthorizationOptions)
        {

            return new JObject(new JProperty("AuthorizationOptions", AuthorizationOptions.ToString()));

        }

        #endregion



        // -----

        #region ToJSON(this ChargingPool)

        public static JObject ToJSON(this ChargingPool EVChargingPool)
        {

            return JSON.Create(EVChargingPool.Id.               ToJSON("@id"),
                               new JProperty("@context",        "http://eMI3"),
                               EVChargingPool.LastChange.       ToJSON("lastChange"),
                               EVChargingPool.Name.             ToJSON("name"),
                               (EVChargingPool.Description.IsNotEmpty)
                                   ? EVChargingPool.Description.ToJSON("description")
                                   : null,
                               EVChargingPool.LocationLanguage. ToJSON("locationLanguage"),
                               EVChargingPool.PoolLocation.     ToJSON("poolLocation"),
                               EVChargingPool.EntranceLocation. ToJSON("entranceLocation"),
                               EVChargingPool.Address.          ToJSON("address"),
                               EVChargingPool.EntranceAddress.  ToJSON("entranceAddress"),
                               EVChargingPool.PoolOwner.        ToJSON("poolOwner"),
                               EVChargingPool.LocationOwner.    ToJSON("locationOwner"),
                               EVChargingPool.EVSEOperator.     ToJSON("EVSEOperator"),
                               EVChargingPool.OpeningTime.      ToJSON("openingTime"),
                               new JProperty("chargingStations",    EVChargingPool.ChargingStations.SafeSelect(v => v.ToJSON())));

        }

        #endregion

        #region ToJSON(this ChargingStation)

        public static JObject ToJSON(this ChargingStation ChargingStation)
        {

            return new JObject(new JProperty("serviceIdentification",       ChargingStation.ServiceIdentification),
                               new JProperty("userComment",                 ChargingStation.UserComment.ToJSON()),
                               new JProperty("serviceProviderComment",      ChargingStation.ServiceProviderComment.ToJSON()),
                               new JProperty("geoLocation",                 ChargingStation.GeoLocation.ToJSON()),
                               new JProperty("gridConnection",              ChargingStation.GridConnection.ToJSON()),
                               new JProperty("chargingStationUIFeatures",   ChargingStation.Features.ToJSON()),
                               new JProperty("authorizationOptions",        ChargingStation.AuthorizationOptions.ToJSON()),
                               new JProperty("photoURIs",                   new JArray(ChargingStation.PhotoURIs.Select(v => v.ToString())),
                               new JProperty("pointOfDelivery",             ChargingStation.PointOfDelivery),
                               new JProperty("EVSEs",                       ChargingStation.EVSEs.SafeSelect(v => v.ToJSON()))));

        }

        #endregion

        #region ToJSON(this EVSE)

        public static JObject ToJSON(this EVSE EVSE)
        {

            return new JObject(new JProperty("EVSE", EVSE.ToString()));

        }

        #endregion

    }

}
