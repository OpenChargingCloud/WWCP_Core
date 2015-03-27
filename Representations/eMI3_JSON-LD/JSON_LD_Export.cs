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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

using org.GraphDefined.eMI3;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.eMI3.IO.JSON_LD
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


        //#region ToJSON(this Object, JPropertyKey)

        ///// <summary>
        ///// Create a JSON representation of the given object..
        ///// </summary>
        ///// <param name="Object">An object.</param>
        ///// <param name="JPropertyKey">The name of the JSON property key.</param>
        //public static JProperty ToJSON(this Object Object, String JPropertyKey)
        //{
        //    return new JProperty(JPropertyKey, Object.ToString());
        //}

        //#endregion

        #region ToJSON(this IId, JPropertyKey)

        /// <summary>
        /// Create a JSON representation of the given object..
        /// </summary>
        /// <param name="Object">An object.</param>
        /// <param name="JPropertyKey">The name of the JSON property key.</param>
        public static JProperty ToJSON(this IId Id, String JPropertyKey)
        {

            return (Id != null)
                       ? new JProperty(JPropertyKey, Id.ToString())
                       : null;

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
        public static JProperty ToJSON(this GeoCoordinate Location, String JPropertyKey)
        {

            if (Location.Longitude.Value == 0 && Location.Latitude.Value == 0)
                return null;

            return new JProperty(JPropertyKey,
                                 JSON.Create(
                                     (Location.Projection != GravitationalModel.EGM2008) ? new JProperty("projection", Location.Projection)    : null,
                                     new JProperty("latitude",  Location.Latitude),
                                     new JProperty("longitude", Location.Longitude),
                                     (Location.Altitude.Value != 0.0)                    ? new JProperty("altitude", Location.Altitude) : null)
                                );

        }

        #endregion


        #region ToJSON(this OpeningTime, JPropertyKey)

        public static JProperty ToJSON(this OpeningTime OpeningTime, String JPropertyKey)
        {

            return (OpeningTime != null)
                       ? new JProperty(JPropertyKey,
                                       OpeningTime.ToString())
                       : null;

        }

        #endregion

        #region ToJSON(this GridConnection, JPropertyKey)

        public static JProperty ToJSON(this GridConnection GridConnection, String JPropertyKey)
        {

            return (GridConnection != null)
                       ? new JProperty(JPropertyKey,
                                       GridConnection.ToString())
                       : null;

        }

        #endregion

        #region ToJSON(this ChargingStationUIFeatures, JPropertyKey)

        public static JProperty ToJSON(this ChargingStationUIFeatures ChargingStationUIFeatures, String JPropertyKey)
        {

            return new JProperty(JPropertyKey,
                                 ChargingStationUIFeatures.ToString());

        }

        #endregion

        #region ToJSON(this AuthorizationOptions, JPropertyKey)

        public static JProperty ToJSON(this AuthorizationOptions AuthorizationOptions, String JPropertyKey)
        {

            return new JProperty(JPropertyKey,
                                 AuthorizationOptions.ToString());

        }

        #endregion

        #region ToJSON(this Languages, JPropertyKey)

        public static JProperty ToJSON(this Languages Languages, String JPropertyKey)
        {

            return new JProperty(JPropertyKey,
                                 Languages.ToString());

        }

        #endregion

        #region ToJSON(this Text, JPropertyKey)

        public static JProperty ToJSON(this String Text, String JPropertyKey)
        {

            return (Text != null)
                        ? new JProperty(JPropertyKey, Text)
                        : null;

        }

        #endregion


        // -----

        #region ToJSON(this Address)

        public static JObject ToJSON(this Address _Address)
        {

            return (_Address != null)
                       ? JSON.Create(_Address.FloorLevel         .ToJSON("floorLevel"),
                                     _Address.HouseNumber        .ToJSON("houseNumber"),
                                     _Address.Street             .ToJSON("street"),
                                     _Address.PostalCode         .ToJSON("postalCode"),
                                     _Address.PostalCodeSub      .ToJSON("postalCodeSub"),
                                     _Address.City               .ToJSON("city"),
                                     (_Address.Country != null)
                                          ? _Address.Country.CountryName.ToJSON("country")
                                          : null)
                       : null;

        }

        #endregion

        #region ToJSON(this Address, JPropertyKey)

        public static JProperty ToJSON(this Address Address, String JPropertyKey)
        {

            return (Address != null)
                       ? new JProperty(JPropertyKey,
                                       Address.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this Addresses, JPropertyKey)

        public static JArray ToJSON(this IEnumerable<Address> Addresses)
        {

            return (Addresses != null && Addresses.Any())
                        ? new JArray(Addresses.SafeSelect(v => v.ToJSON()))
                        : null;

        }

        #endregion

        #region ToJSON(this Addresses, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<Address> Addresses, String JPropertyKey)
        {

            return (Addresses != null)
                        ? new JProperty(JPropertyKey,
                                        Addresses.ToJSON())
                        : null;

        }

        #endregion


        // -----

        // ToDo: Add languages filter!

        #region ToJSON(this RoamingNetwork)

        public static JObject ToJSON(this RoamingNetwork RoamingNetwork)
        {

            return JSON.Create(RoamingNetwork.Id.               ToJSON("@id"),
                               new JProperty("@context",        "http://api.graphdefined.com/eMI3"),
                               RoamingNetwork.LastChange.       ToJSON("lastChange"),
                               RoamingNetwork.Name.             ToJSON("name"),
                               (RoamingNetwork.Description.IsNotEmpty)
                                   ? RoamingNetwork.Description.ToJSON("description")
                                   : null,
                               new JProperty("uri", "~/" + RoamingNetwork.Id.ToString()),
                               //EVChargingPool.LocationLanguage. ToJSON("locationLanguage"),
                               //EVChargingPool.PoolLocation.     ToJSON("poolLocation"),
                               //EVChargingPool.EntranceLocation. ToJSON("entranceLocation"),
                               //EVChargingPool.Address.          ToJSON("address"),
                               //EVChargingPool.EntranceAddress.  ToJSON("entranceAddress"),
                               //EVChargingPool.PoolOwner.        ToJSON("poolOwner"),
                               //EVChargingPool.LocationOwner.    ToJSON("locationOwner"),
                               //EVChargingPool.EVSEOperator.     ToJSON("EVSEOperator"),
                               //EVChargingPool.OpeningTime.      ToJSON("openingTime"),
                               RoamingNetwork.EVSEOperators.SelectMany(v => v.ChargingPools).ToJSON("chargingPools")
                             );

        }

        #endregion

        #region ToJSON(this RoamingNetwork, JPropertyKey)

        public static JProperty ToJSON(this RoamingNetwork RoamingNetwork, String JPropertyKey)
        {

            return new JProperty(JPropertyKey,
                                 RoamingNetwork.ToJSON());

        }

        #endregion

        #region ToJSON(this RoamingNetworks, JPropertyKey)

        public static JArray ToJSON(this IEnumerable<RoamingNetwork> RoamingNetworks)
        {

            return (RoamingNetworks != null && RoamingNetworks.Any())
                        ? new JArray(RoamingNetworks.SafeSelect(v => v.ToJSON()))
                        : null;

        }

        #endregion

        #region ToJSON(this RoamingNetworks, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<RoamingNetwork> RoamingNetworks, String JPropertyKey)
        {

            return (RoamingNetworks != null)
                        ? new JProperty(JPropertyKey,
                                        RoamingNetworks.ToJSON())
                        : null;

        }

        #endregion


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

        #region ToJSON(this ChargingPool, JPropertyKey)

        public static JObject ToJSON(this ChargingPool EVChargingPool, String JPropertyKey)
        {

            return new JObject(JPropertyKey,
                               EVChargingPool.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargingPools, JPropertyKey)

        public static JArray ToJSON(this IEnumerable<ChargingPool> ChargingPools)
        {

            return (ChargingPools != null && ChargingPools.Any())
                        ? new JArray(ChargingPools.SafeSelect(v => v.ToJSON()))
                        : null;

        }

        #endregion

        #region ToJSON(this ChargingPools, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingPool> ChargingPools, String JPropertyKey)
        {

            return (ChargingPools != null)
                        ? new JProperty(JPropertyKey,
                                        ChargingPools.ToJSON())
                        : null;

        }

        #endregion


        #region ToJSON(this ChargingStation)

        public static JObject ToJSON(this ChargingStation ChargingStation)
        {

            return JSON.Create(ChargingStation.ServiceIdentification.   ToJSON("serviceIdentification"),
                               ChargingStation.UserComment.             ToJSON("userComment"),
                               ChargingStation.ServiceProviderComment.  ToJSON("serviceProviderComment"),
                               ChargingStation.GeoLocation.             ToJSON("geoLocation"),
                               ChargingStation.GridConnection.          ToJSON("gridConnection"),
                               ChargingStation.Features.                ToJSON("chargingStationUIFeatures"),
                               ChargingStation.AuthorizationOptions.    ToJSON("authorizationOptions"),
                               new JProperty("photoURIs",  new JArray(ChargingStation.PhotoURIs.Select(v => v.ToString())),
                               ChargingStation.PointOfDelivery.         ToJSON("pointOfDelivery")));
            //                   new JProperty("EVSEs",                 ChargingStation.EVSEs.SafeSelect(v => v.ToJSON()))));

        }

        #endregion

        #region ToJSON(this ChargingStation, JPropertyKey)

        public static JProperty ToJSON(this ChargingStation ChargingStation, String JPropertyKey)
        {

            return new JProperty(JPropertyKey,
                                 ChargingStation.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargingStations, JPropertyKey)

        public static JArray ToJSON(this IEnumerable<ChargingStation> ChargingStations)
        {

            return (ChargingStations != null && ChargingStations.Any())
                        ? new JArray(ChargingStations.SafeSelect(v => v.ToJSON()))
                        : null;

        }

        #endregion

        #region ToJSON(this ChargingStations, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingStation> ChargingStations, String JPropertyKey)
        {

            return (ChargingStations != null)
                        ? new JProperty(JPropertyKey,
                                        ChargingStations.ToJSON())
                        : null;

        }

        #endregion


        #region ToJSON(this EVSE)

        public static JObject ToJSON(this EVSE EVSE)
        {

            return JSON.Create(EVSE.Id.ToJSON("@id"),
                               new JProperty("@context", "http://api.graphdefined.com/eMI3"),
                               EVSE.LastChange.ToJSON("lastChange"),
                               new JProperty("uri", "~/" + EVSE.Id.ToString())
                               );

        }

        #endregion

        #region ToJSON(this EVSE, JPropertyKey)

        public static JProperty ToJSON(this EVSE EVSE, String JPropertyKey)
        {

            return new JProperty(JPropertyKey,
                                 EVSE.ToJSON());

        }

        #endregion

        #region ToJSON(this EVSEs, JPropertyKey)

        public static JArray ToJSON(this IEnumerable<EVSE> EVSEs)
        {

            return (EVSEs != null && EVSEs.Any())
                        ? new JArray(EVSEs.SafeSelect(v => v.ToJSON()))
                        : null;

        }

        #endregion

        #region ToJSON(this EVSEs, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<EVSE> EVSEs, String JPropertyKey)
        {

            return (EVSEs != null)
                        ? new JProperty(JPropertyKey,
                                        EVSEs.ToJSON())
                        : null;

        }

        #endregion

    }

}
