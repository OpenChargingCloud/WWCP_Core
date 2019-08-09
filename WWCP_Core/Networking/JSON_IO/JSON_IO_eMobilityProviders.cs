﻿/*
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
        public static void Attach_JSON_IO_eMobilityProviders(this WWCP_HTTPAPI  WWCPAPI,
                                                             HTTPHostname?      Hostname   = null,
                                                             HTTPPath?           URIPrefix  = null)
        {

            var _Hostname   = Hostname  ?? HTTPHostname.Any;
            var _URIPrefix  = URIPrefix ?? HTTPPath.Parse("/");

            #region ~/RNs/{RoamingNetworkId}/eMobilityProviders

            #region GET         ~/RNs/{RoamingNetworkId}/eMobilityProviders

            // -----------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/eMobilityProviders
            // -----------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/eMobilityProviders",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  _RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out _RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip                    = Request.QueryString.GetUInt64("skip");
                                                     var take                    = Request.QueryString.GetUInt64("take");
                                                     var expand                  = Request.QueryString.GetStrings("expand", true);
                                                     //var expandChargingPools     = !expand.Contains("-chargingpools");
                                                     //var expandChargingStations  = !expand.Contains("-chargingstations");
                                                     //var expandBrands            = expand.Contains("brands");

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount = _RoamingNetwork.eMobilityProviders.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = _RoamingNetwork.eMobilityProviders.
                                                                                                ToJSON(skip,
                                                                                                       take,
                                                                                                       false).
                                                                                                       //expandChargingPools,
                                                                                                       //expandChargingStations,
                                                                                                       //expandBrands).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region COUNT       ~/RNs/{RoamingNetworkId}/eMobilityProviders

            // ----------------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:3004/RNs/{RoamingNetworkId}/eMobilityProviders
            // ----------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.COUNT,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/eMobilityProviders",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  _RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out _RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = JSONObject.Create(
                                                                                                new JProperty("count",  _RoamingNetwork.ChargingStationOperators.ULongCount())
                                                                                            ).ToUTF8Bytes()
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/eMobilityProviders->AdminStatus

            // ------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/eMobilityProviders->AdminStatus
            // ------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/eMobilityProviders->AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  _RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out _RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip         = Request.QueryString.GetUInt64("skip");
                                                     var take         = Request.QueryString.GetUInt64("take");
                                                     var historysize  = Request.QueryString.GetUInt64("historysize", 1);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount = _RoamingNetwork.ChargingStationOperatorAdminStatus.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = _RoamingNetwork.ChargingStationOperatorAdminStatus.
                                                                                                OrderBy(kvp => kvp.Key).
                                                                                                ToJSON (skip,
                                                                                                        take,
                                                                                                        historysize).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/eMobilityProviders->Status

            // -------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/eMobilityProviders->Status
            // -------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/eMobilityProviders->Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  _RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out _RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip         = Request.QueryString.GetUInt64("skip");
                                                     var take         = Request.QueryString.GetUInt64("take");
                                                     var historysize  = Request.QueryString.GetUInt64("historysize", 1);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount = _RoamingNetwork.ChargingStationOperatorStatus.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = _RoamingNetwork.ChargingStationOperatorStatus.
                                                                                                OrderBy(kvp => kvp.Key).
                                                                                                ToJSON (skip,
                                                                                                        take,
                                                                                                        historysize).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/eMobilityProviders/{eMobilityProviderId}

            #region GET         ~/RNs/{RoamingNetworkId}/eMobilityProviders/{eMobilityProviderId}

            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/eMobilityProviders/{eMobilityProviderId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndEMobilityProvider(WWCPAPI,
                                                                                                          out RoamingNetwork     _RoamingNetwork,
                                                                                                          out eMobilityProvider  _eMobilityProvider,
                                                                                                          out HTTPResponse       _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                                             Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                       = DateTime.UtcNow,
                                                             AccessControlAllowOrigin   = "*",
                                                             AccessControlAllowMethods  = "GET, CREATE, DELETE",
                                                             AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                             ETag                       = "1",
                                                             ContentType                = HTTPContentType.JSON_UTF8,
                                                             Content                    = _eMobilityProvider.ToJSON().ToUTF8Bytes()
                                                         }.AsImmutable);

                                           });

            #endregion

            #endregion

        }

    }

}
