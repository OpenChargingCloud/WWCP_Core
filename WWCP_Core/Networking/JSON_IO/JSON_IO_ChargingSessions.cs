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
        public static void Attach_JSON_IO_ChargingSessions(this WWCP_HTTPAPI  WWCPAPI,
                                                           HTTPHostname?      Hostname   = null,
                                                           HTTPPath?           URIPrefix  = null)
        {

            var _Hostname   = Hostname  ?? HTTPHostname.Any;
            var _URIPrefix  = URIPrefix ?? HTTPPath.Parse("/");

            #region ~/RNs/{RoamingNetworkId}/ChargingSessions

            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "ChargingSessions",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async HTTPRequest => {

                                                     #region Check HTTP Basic Authentication

                                                     //if (HTTPRequest.Authorization          == null        ||
                                                     //    HTTPRequest.Authorization.Username != HTTPLogin   ||
                                                     //    HTTPRequest.Authorization.Password != HTTPPassword)
                                                     //    return new HTTPResponse.Builder() {
                                                     //        HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                     //        WWWAuthenticate  = @"Basic realm=""Bosch E-Bike""",
                                                     //        Server           = _HTTPServer.DefaultServerName,
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

                                                     var skip               = HTTPRequest.QueryString.GetUInt32("skip");
                                                     var take               = HTTPRequest.QueryString.GetUInt32("take");

                                                     var _ChargingSessions  = RoamingNetwork.
                                                                                  ChargingSessions.
                                                                                  OrderBy(session => session.Id.ToString()).
                                                                                  Skip(skip).
                                                                                  Take(take).
                                                                                  ToArray();

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount     = RoamingNetwork.ChargingSessions.LongCount();


                                                     return new HTTPResponse.Builder(HTTPRequest) {
                                                         HTTPStatusCode             = _ChargingSessions.Any()
                                                                                          ? HTTPStatusCode.OK
                                                                                          : HTTPStatusCode.NoContent,
                                                         Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                         Date                       = DateTime.UtcNow,
                                                         AccessControlAllowOrigin   = "*",
                                                         AccessControlAllowMethods  = "GET",
                                                         AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                         ETag                       = "1",
                                                         ContentType                = HTTPContentType.JSON_UTF8,
                                                         Content                    = (_ChargingSessions.Any()
                                                                                          ? _ChargingSessions.ToJSON()
                                                                                          : new JArray()
                                                                                      ).ToUTF8Bytes()

                                                     }.Set(new HTTPHeaderField("X-ExpectedTotalNumberOfItems", typeof(UInt64), HeaderFieldType.Response, RequestPathSemantic.EndToEnd),
                                                                               _ExpectedCount);

                                                 });

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargeDetailRecords

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargeDetailRecords/{ChargingSession_Id}

            #endregion

        }

    }

}
