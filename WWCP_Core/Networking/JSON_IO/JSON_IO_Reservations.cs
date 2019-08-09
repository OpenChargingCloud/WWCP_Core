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
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP HTTP API - JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        /// <summary>
        /// Attach JSON I/O to the given WWCP HTTP API.
        /// </summary>
        /// <param name="WWCPAPI">A WWCP HTTP API.</param>
        /// <param name="Hostname">Limit this JSON I/O handling to the given HTTP hostname.</param>
        /// <param name="URIPrefix">A common URI prefix for all URIs within this API.</param>
        public static void Attach_JSON_IO_Reservations(this WWCP_HTTPAPI  WWCPAPI,
                                                       HTTPHostname?      Hostname   = null,
                                                       HTTPPath?           URIPrefix  = null)
        {

            var _Hostname   = Hostname  ?? HTTPHostname.Any;
            var _URIPrefix  = URIPrefix ?? HTTPPath.Parse("/");

            #region ~/RNs/{RoamingNetworkId}/Reservations

            #region GET         ~/RNs/{RoamingNetworkId}/Reservations

            // ----------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/Reservations
            // ----------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/Reservations",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async HTTPRequest => {

                                                     #region Check HTTP Basic Authentication

                                                     //if (HTTPRequest.Authorization          == null        ||
                                                     //    HTTPRequest.Authorization.Username != HTTPLogin   ||
                                                     //    HTTPRequest.Authorization.Password != HTTPPassword)
                                                     //    return new HTTPResponse.Builder(HTTPRequest) {
                                                     //        HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                     //        WWWAuthenticate  = @"Basic realm=""WWCP EV Charging""",
                                                     //        Server           = _API.HTTPServer.DefaultServerName,
                                                     //        Date             = DateTime.UtcNow,
                                                     //        Connection       = "close"
                                                     //    };

                                                     #endregion

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;

                                                     if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out _HTTPResponse))
                                                         return _HTTPResponse;

                                                     #endregion

                                                     var skip                   = HTTPRequest.QueryString.GetUInt64("skip");
                                                     var take                   = HTTPRequest.QueryString.GetUInt32("take");

                                                     var _ChargingReservations  = RoamingNetwork.
                                                                                      ChargingReservations.
                                                                                      OrderBy(reservation => reservation.Id.ToString()).
                                                                                      Skip(skip).
                                                                                      Take(take).
                                                                                      ToArray();

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount         = RoamingNetwork.ChargingReservations.LongCount();

                                                     return new HTTPResponse.Builder(HTTPRequest) {
                                                         HTTPStatusCode             = _ChargingReservations.Any()
                                                                                          ? HTTPStatusCode.OK
                                                                                          : HTTPStatusCode.NoContent,
                                                         Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                         Date                       = DateTime.UtcNow,
                                                         AccessControlAllowOrigin   = "*",
                                                         AccessControlAllowMethods  = "GET",
                                                         AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                         ETag                       = "1",
                                                         ContentType                = HTTPContentType.JSON_UTF8,
                                                         Content                    = (_ChargingReservations.Any()
                                                                                          ? _ChargingReservations.ToJSON()
                                                                                          : new JArray()
                                                                                      ).ToUTF8Bytes()
                                                     }.Set(new HTTPHeaderField("X-ExpectedTotalNumberOfItems", typeof(UInt64), HeaderFieldType.Response, RequestPathSemantic.EndToEnd),
                                                                               _ExpectedCount);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/Reservations/{ReservationId}

            #region GET         ~/RNs/{RoamingNetworkId}/Reservations/{ReservationId}

            // -----------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/Reservations
            // -----------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/Reservations/{ReservationId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async HTTPRequest => {

                                                     #region Check HTTP Basic Authentication

                                                     //if (HTTPRequest.Authorization          == null        ||
                                                     //    HTTPRequest.Authorization.Username != HTTPLogin   ||
                                                     //    HTTPRequest.Authorization.Password != HTTPPassword)
                                                     //    return new HTTPResponse.Builder(HTTPRequest) {
                                                     //        HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                     //        WWWAuthenticate  = @"Basic realm=""WWCP EV Charging""",
                                                     //        Server           = _API.HTTPServer.DefaultServerName,
                                                     //        Date             = DateTime.UtcNow,
                                                     //        Connection       = "close"
                                                     //    };

                                                     #endregion

                                                     #region Check ChargingReservationId parameter

                                                     HTTPResponse         _HTTPResponse;
                                                     RoamingNetwork       RoamingNetwork;
                                                     ChargingReservation  Reservation;

                                                     if (!HTTPRequest.ParseRoamingNetworkAndReservation(WWCPAPI,
                                                                                                        out RoamingNetwork,
                                                                                                        out Reservation,
                                                                                                        out _HTTPResponse))
                                                         return _HTTPResponse;

                                                     #endregion

                                                     return new HTTPResponse.Builder(HTTPRequest) {
                                                         HTTPStatusCode             = HTTPStatusCode.OK,
                                                         Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                         Date                       = DateTime.UtcNow,
                                                         AccessControlAllowOrigin   = "*",
                                                         AccessControlAllowMethods  = "GET, SETEXPIRED, DELETE",
                                                         AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                         ETag                       = "1",
                                                         ContentType                = HTTPContentType.JSON_UTF8,
                                                         Content                    = Reservation.ToJSON().ToUTF8Bytes()
                                                     };

                                                 });

            #endregion

            #region SETEXPIRED  ~/RNs/{RoamingNetworkId}/Reservations/{ReservationId}

            // -----------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/Reservations
            // -----------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 WWCP_HTTPAPI.SETEXPIRED,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/Reservations/{ReservationId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     #region Check HTTP Basic Authentication

                                                     //if (HTTPRequest.Authorization          == null        ||
                                                     //    HTTPRequest.Authorization.Username != HTTPLogin   ||
                                                     //    HTTPRequest.Authorization.Password != HTTPPassword)
                                                     //    return new HTTPResponse.Builder(HTTPRequest) {
                                                     //        HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                     //        WWWAuthenticate  = @"Basic realm=""WWCP EV Charging""",
                                                     //        Server           = _API.HTTPServer.DefaultServerName,
                                                     //        Date             = DateTime.UtcNow,
                                                     //        Connection       = "close"
                                                     //    };

                                                     #endregion

                                                     #region Check ChargingReservationId parameter

                                                     HTTPResponse         _HTTPResponse;
                                                     RoamingNetwork       RoamingNetwork;
                                                     ChargingReservation  Reservation;

                                                     if (!Request.ParseRoamingNetworkAndReservation(WWCPAPI,
                                                                                                        out RoamingNetwork,
                                                                                                        out Reservation,
                                                                                                        out _HTTPResponse))
                                                         return _HTTPResponse;

                                                     #endregion


                                                     var response = RoamingNetwork.CancelReservation(Reservation.Id,
                                                                                                     ChargingReservationCancellationReason.Deleted,
                                                                                                  //   null, //ToDo: Refacor me to make use of the ProviderId!
                                                                                                 //    null,

                                                                                                     Request.Timestamp,
                                                                                                     Request.CancellationToken,
                                                                                                     Request.EventTrackingId,
                                                                                                     TimeSpan.FromSeconds(60)).Result;

                                                     switch (response.Result)
                                                     {

                                                         case CancelReservationResultTypes.Success:
                                                             return new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.OK,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = DateTime.UtcNow,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, SETEXPIRED, DELETE",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(new JProperty("en", "Reservation removed. Additional costs may be charged!")).ToUTF8Bytes()
                                                             };

                                                         default:
                                                             return new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.InternalServerError,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = DateTime.UtcNow,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, DELETE",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(new JProperty("description", "Could not remove reservation!")).ToUTF8Bytes()
                                                             };

                                                     }

                                                 });

            #endregion

            #region DELETE      ~/RNs/{RoamingNetworkId}/Reservations/{ReservationId}

            // -----------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/Reservations
            // -----------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.DELETE,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/Reservations/{ReservationId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     #region Check HTTP Basic Authentication

                                                     //if (HTTPRequest.Authorization          == null        ||
                                                     //    HTTPRequest.Authorization.Username != HTTPLogin   ||
                                                     //    HTTPRequest.Authorization.Password != HTTPPassword)
                                                     //    return new HTTPResponse.Builder(HTTPRequest) {
                                                     //        HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                     //        WWWAuthenticate  = @"Basic realm=""WWCP EV Charging""",
                                                     //        Server           = _API.HTTPServer.DefaultServerName,
                                                     //        Date             = DateTime.UtcNow,
                                                     //        Connection       = "close"
                                                     //    };

                                                     #endregion

                                                     #region Check ChargingReservationId parameter

                                                     HTTPResponse         _HTTPResponse;
                                                     RoamingNetwork       RoamingNetwork;
                                                     ChargingReservation  Reservation;

                                                     if (!Request.ParseRoamingNetworkAndReservation(WWCPAPI,
                                                                                                        out RoamingNetwork,
                                                                                                        out Reservation,
                                                                                                        out _HTTPResponse))
                                                         return _HTTPResponse;

                                                     #endregion


                                                     var response = RoamingNetwork.CancelReservation(Reservation.Id,
                                                                                                     ChargingReservationCancellationReason.Deleted,
                                                                                                 //    null, //ToDo: Refacor me to make use of the ProviderId!
                                                                                                 //    null,

                                                                                                     Request.Timestamp,
                                                                                                     Request.CancellationToken,
                                                                                                     Request.EventTrackingId,
                                                                                                     TimeSpan.FromSeconds(60)).Result;

                                                     switch (response.Result)
                                                     {

                                                         case CancelReservationResultTypes.Success:
                                                             return new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.OK,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = DateTime.UtcNow,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, SETEXPIRED, DELETE",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(new JProperty("en", "Reservation removed. Additional costs may be charged!")).ToUTF8Bytes()
                                                             };

                                                         default:
                                                             return new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.InternalServerError,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = DateTime.UtcNow,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, DELETE",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(new JProperty("description", "Could not remove reservation!")).ToUTF8Bytes()
                                                             };

                                                     }

                                                 });

            #endregion

            #endregion

        }

    }

}
