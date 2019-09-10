/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
        public static void Attach_JSON_IO_RoamingNetworks(this WWCP_HTTPAPI  WWCPAPI,
                                                          HTTPHostname?      Hostname   = null,
                                                          HTTPPath?           URIPrefix  = null)
        {

            var _Hostname   = Hostname  ?? HTTPHostname.Any;
            var _URIPrefix  = URIPrefix ?? HTTPPath.Parse("/");


            #region ~/RNs

            #region GET         ~/RNs

            // -----------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs
            // -----------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     var AllRoamingNetworks  = WWCPAPI.GetAllRoamingNetworks(Request.Host);
                                                     var skip                = Request.QueryString.GetUInt64("skip");
                                                     var take                = Request.QueryString.GetUInt64("take");

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount       = AllRoamingNetworks.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = AllRoamingNetworks.
                                                                                                 ToJSON(skip, take).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = ExpectedCount,
                                                             Connection                    = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region COUNT       ~/RNs

            // --------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:3004/RNs
            // --------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.COUNT,
                                                 _URIPrefix + "RNs",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     var AllRoamingNetworks  = WWCPAPI.GetAllRoamingNetworks(Request.Host);

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = JSONObject.Create(
                                                                                                new JProperty("count",  AllRoamingNetworks.ULongCount())
                                                                                            ).ToUTF8Bytes(),
                                                             Connection                   = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region OPTIONS     ~/RNs

            // ----------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:3004/RNs
            // ----------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.OPTIONS,
                                                 _URIPrefix + "RNs",
                                                 HTTPDelegate: Request =>

                                                     Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode             = HTTPStatusCode.NoContent,
                                                             Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                       = DateTime.UtcNow,
                                                             AccessControlAllowOrigin   = "*",
                                                             AccessControlAllowMethods  = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                             Connection                 = "close"
                                                         }.AsImmutable)

                                                 );

            #endregion


            #region GET         ~/RNs->Id

            // -------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs->Id
            // -------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs->Id",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     var AllRoamingNetworks  = WWCPAPI.GetAllRoamingNetworks(Request.Host);
                                                     var skip                = Request.QueryString.GetUInt64("skip");
                                                     var take                = Request.QueryString.GetUInt64("take");

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount       = AllRoamingNetworks.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = new JArray(AllRoamingNetworks.
                                                                                                             Select(rn => rn.Id.ToString()).
                                                                                                             Skip  (Request.QueryString.GetUInt64("skip")).
                                                                                                             Take  (Request.QueryString.GetUInt64("take"))).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs->AdminStatus

            // ------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs->AdminStatus
            // ------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs->AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     var AllRoamingNetworks  = WWCPAPI.GetAllRoamingNetworks(Request.Host);
                                                     var skip                = Request.QueryString.GetUInt64("skip");
                                                     var take                = Request.QueryString.GetUInt64("take");
                                                     var sinceFilter         = Request.QueryString.CreateDateTimeFilter<RoamingNetworkAdminStatus>("since", (status, timestamp) => status.Status.Timestamp >= timestamp);
                                                     var matchFilter         = Request.QueryString.CreateStringFilter  <RoamingNetworkAdminStatus>("match", (status, pattern)   => status.Id.ToString().Contains(pattern));

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount       = AllRoamingNetworks.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = AllRoamingNetworks.
                                                                                                  Select(rn => new RoamingNetworkAdminStatus(rn.Id, rn.AdminStatus)).
                                                                                                  Where (matchFilter).
                                                                                                  Where (sinceFilter).
                                                                                                  ToJSON(skip, take).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs->Status

            // -------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs->Status
            // -------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs->Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     var AllRoamingNetworks  = WWCPAPI.GetAllRoamingNetworks(Request.Host);
                                                     var skip                = Request.QueryString.GetUInt64("skip");
                                                     var take                = Request.QueryString.GetUInt64("take");
                                                     var sinceFilter         = Request.QueryString.CreateDateTimeFilter<RoamingNetworkStatus>("since", (status, timestamp) => status.Status.Timestamp >= timestamp);
                                                     var matchFilter         = Request.QueryString.CreateStringFilter  <RoamingNetworkStatus>("match", (status, pattern)   => status.Id.ToString().Contains(pattern));

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount      = AllRoamingNetworks.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = AllRoamingNetworks.
                                                                                                  Select(rn => new RoamingNetworkStatus(rn.Id, rn.Status)).
                                                                                                  Where (matchFilter).
                                                                                                  Where (sinceFilter).
                                                                                                  ToJSON(skip, take).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = _ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}

            #region GET         ~/RNs/{RoamingNetworkId}

            // ----------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test
            // ----------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  _RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out _RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

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
                                                             Content                    = _RoamingNetwork.ToJSON().ToUTF8Bytes()
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region CREATE      ~/RNs/{RoamingNetworkId}

            // ---------------------------------------------------------------------------------
            // curl -v -X CREATE -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test2
            // ---------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.CREATE,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     WWCPAPI.SendOnCreateRoamingNetwork_RequestLog(Request);

                                                     #region Check HTTP Basic Authentication

                                                     //if (Request.Authorization          == null      ||
                                                     //    Request.Authorization.Username != HTTPLogin ||
                                                     //    Request.Authorization.Password != HTTPPassword)
                                                     //    return SendEVSEStatusSetted(
                                                     //        new HTTPResponse.Builder(Request) {
                                                     //            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                     //            WWWAuthenticate  = @"Basic realm=""WWCP""",
                                                     //            Server           = HTTPServer.DefaultServerName,
                                                     //            Date             = DateTime.UtcNow,
                                                     //            Connection       = "close"
                                                     //        });

                                                     #endregion

                                                     #region Check HTTP parameters

                                                     if (Request.ParsedURIParameters.Length < 1)
                                                         return Task.FromResult(
                                                             WWCPAPI.SendOnCreateRoamingNetwork_ResponseLog(
                                                                 new HTTPResponse.Builder(Request) {
                                                                     HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                     Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                                     Date            = DateTime.UtcNow,
                                                                 }.AsImmutable));


                                                     if (!RoamingNetwork_Id.TryParse(Request.ParsedURIParameters[0],
                                                                                     out RoamingNetwork_Id _RoamingNetworkId))
                                                         return Task.FromResult(
                                                             WWCPAPI.SendOnCreateRoamingNetwork_ResponseLog(
                                                                 new HTTPResponse.Builder(Request) {
                                                                     HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                     Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                                     Date            = DateTime.UtcNow,
                                                                     ContentType     = HTTPContentType.JSON_UTF8,
                                                                     Content         = HTTPExtentions.CreateError("Invalid RoamingNetworkId!")
                                                                 }.AsImmutable));


                                                     if (WWCPAPI.TryGetRoamingNetwork(Request.Host,
                                                                                      _RoamingNetworkId,
                                                                                      out RoamingNetwork _RoamingNetwork))
                                                        return Task.FromResult(
                                                            WWCPAPI.SendOnCreateRoamingNetwork_ResponseLog(
                                                                new HTTPResponse.Builder(Request) {
                                                                    HTTPStatusCode  = HTTPStatusCode.Conflict,
                                                                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                                    Date            = DateTime.UtcNow,
                                                                    ContentType     = HTTPContentType.JSON_UTF8,
                                                                    Content         = HTTPExtentions.CreateError("RoamingNetworkId already exists!")
                                                                }.AsImmutable));

                                                     #endregion

                                                     #region Parse optional JSON

                                                     I18NString RoamingNetworkName         = I18NString.Empty;
                                                     I18NString RoamingNetworkDescription  = I18NString.Empty;

                                                     if (Request.TryParseJObjectRequestBody(out JObject       JSON,
                                                                                            out HTTPResponse  _HTTPResponse,
                                                                                            AllowEmptyHTTPBody: true))
                                                     {

                                                         if (!JSON.ParseMandatory("name",
                                                                                  "roaming network name",
                                                                                  WWCPAPI.HTTPServer.DefaultServerName,
                                                                                  out RoamingNetworkName,
                                                                                  Request,
                                                                                  out _HTTPResponse))
                                                         {
                                                             return Task.FromResult(WWCPAPI.SendOnCreateRoamingNetwork_ResponseLog(_HTTPResponse));
                                                         }

                                                         if (!JSON.ParseOptional("description",
                                                                                 "roaming network description",
                                                                                 WWCPAPI.HTTPServer.DefaultServerName,
                                                                                 out RoamingNetworkDescription,
                                                                                 Request,
                                                                                 out _HTTPResponse))
                                                         {
                                                             return Task.FromResult(WWCPAPI.SendOnCreateRoamingNetwork_ResponseLog(_HTTPResponse));
                                                         }

                                                     }

                                                     #endregion


                                                     _RoamingNetwork = WWCPAPI.CreateNewRoamingNetwork(Request.Host,
                                                                                                       _RoamingNetworkId,
                                                                                                       RoamingNetworkName,
                                                                                                       Description: RoamingNetworkDescription ?? I18NString.Empty);


                                                     return Task.FromResult(
                                                         WWCPAPI.SendOnCreateRoamingNetwork_ResponseLog(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.Created,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = DateTime.UtcNow,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, CREATE, DELETE",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ETag                       = "1",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = _RoamingNetwork.ToJSON().ToUTF8Bytes(),
                                                                 Connection                 = "close"
                                                             }.AsImmutable));

                                                 });

            #endregion

            #region DELETE      ~/RNs/{RoamingNetworkId}

            // ---------------------------------------------------------------------------------
            // curl -v -X DELETE -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test2
            // ---------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.DELETE,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                            //         WWCPAPI.SendOnCreateRoamingNetwork_RequestLog(Request);

                                                     #region Check HTTP Basic Authentication

                                                     //if (Request.Authorization          == null      ||
                                                     //    Request.Authorization.Username != HTTPLogin ||
                                                     //    Request.Authorization.Password != HTTPPassword)
                                                     //    return SendEVSEStatusSetted(
                                                     //        new HTTPResponse.Builder(Request) {
                                                     //            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                     //            WWWAuthenticate  = @"Basic realm=""WWCP""",
                                                     //            Server           = HTTPServer.DefaultServerName,
                                                     //            Date             = DateTime.UtcNow,
                                                     //            Connection       = "close"
                                                     //        });

                                                     #endregion

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork _RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(WWCPAPI.SendOnCreateRoamingNetwork_ResponseLog(_HTTPResponse));
                                                     }

                                                     #endregion


                                                     var RoamingNetwork = WWCPAPI.RemoveRoamingNetwork(Request.Host, _RoamingNetwork.Id);


                                                     return Task.FromResult(
                                                         WWCPAPI.SendOnCreateRoamingNetwork_ResponseLog(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.OK,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = DateTime.UtcNow,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, CREATE, DELETE",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ETag                       = "1",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = RoamingNetwork.ToJSON().ToUTF8Bytes()
                                                             }.AsImmutable));

                                                 });

            #endregion

            #region OPTIONS     ~/RNs/{RoamingNetworkId}

            // -----------------------------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}
            // -----------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.OPTIONS,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}",
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork _RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode             = HTTPStatusCode.NoContent,
                                                             Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                       = DateTime.UtcNow,
                                                             AccessControlAllowOrigin   = "*",
                                                             AccessControlAllowMethods  = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                             Connection                 = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/{PropertyKey}

            // ----------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test
            // ----------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/{PropertyKey}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork _RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     if (Request.ParsedURIParameters.Length < 2)
                                                         return Task.FromResult(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                 Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date            = DateTime.UtcNow,
                                                             }.AsImmutable);

                                                     var PropertyKey = Request.ParsedURIParameters[1];

                                                     if (PropertyKey.IsNullOrEmpty())
                                                         return Task.FromResult(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                 Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date            = DateTime.UtcNow,
                                                                 ContentType     = HTTPContentType.JSON_UTF8,
                                                                 Content         = @"{ ""description"": ""Invalid property key!"" }".ToUTF8Bytes()
                                                             }.AsImmutable);


                                                     if (!_RoamingNetwork.TryGet(PropertyKey, out Object Value))
                                                         return Task.FromResult(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.NotFound,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = DateTime.UtcNow,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, SET",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ETag                       = "1",
                                                                 Connection                 = "close"
                                                             }.AsImmutable);


                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                                             Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                       = DateTime.UtcNow,
                                                             AccessControlAllowOrigin   = "*",
                                                             AccessControlAllowMethods  = "GET, SET",
                                                             AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                             ETag                       = "1",
                                                             ContentType                = HTTPContentType.JSON_UTF8,
                                                             Content                    = JSONObject.Create(
                                                                                              new JProperty(PropertyKey, Value)
                                                                                          ).ToUTF8Bytes(),
                                                             Connection                 = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region SET         ~/RNs/{RoamingNetworkId}/{PropertyKey}

            // ----------------------------------------------------------------------
            // curl -v -X SET -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test
            // ----------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.SET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/{PropertyKey}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork _RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     if (Request.ParsedURIParameters.Length < 2)
                                                         return Task.FromResult(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                 Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date            = DateTime.UtcNow,
                                                                 Connection      = "close"
                                                             }.AsImmutable);

                                                     var PropertyKey = Request.ParsedURIParameters[1];

                                                     if (PropertyKey.IsNullOrEmpty())
                                                         return Task.FromResult(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                 Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date            = DateTime.UtcNow,
                                                                 ContentType     = HTTPContentType.JSON_UTF8,
                                                                 Content         = @"{ ""description"": ""Invalid property key!"" }".ToUTF8Bytes(),
                                                                 Connection      = "close"
                                                             }.AsImmutable);


                                                     #region Parse optional JSON

                                                     String OldValue  = null;
                                                     String NewValue  = null;

                                                     var DescriptionI18N = I18NString.Empty;

                                                     if (Request.TryParseJObjectRequestBody(out JObject      JSON,
                                                                                            out HTTPResponse HTTPResponse,
                                                                                            AllowEmptyHTTPBody: false))
                                                     {

                                                         #region Parse oldValue    [mandatory]

                                                         if (!JSON.ParseMandatory("oldValue",
                                                                                  "old value of the property",
                                                                                  WWCPAPI.HTTPServer.DefaultServerName,
                                                                                  out OldValue,
                                                                                  Request,
                                                                                  out HTTPResponse))

                                                             return Task.FromResult(HTTPResponse);

                                                         #endregion

                                                         #region Parse newValue    [mandatory]

                                                         if (!JSON.ParseMandatory("newValue",
                                                                                  "new value of the property",
                                                                                  WWCPAPI.HTTPServer.DefaultServerName,
                                                                                  out NewValue,
                                                                                  Request,
                                                                                  out HTTPResponse))

                                                             return Task.FromResult(HTTPResponse);

                                                         #endregion

                                                     }

                                                     #endregion


                                                     var result = _RoamingNetwork.Set(PropertyKey,
                                                                                      NewValue,
                                                                                      OldValue);

                                                     #region Choose HTTP status code

                                                     HTTPStatusCode _HTTPStatusCode;

                                                     switch (result)
                                                     {

                                                         case SetPropertyResult.Added:
                                                             _HTTPStatusCode = HTTPStatusCode.Created;
                                                             break;

                                                         case SetPropertyResult.Conflict:
                                                             _HTTPStatusCode = HTTPStatusCode.Conflict;
                                                             break;

                                                         default:
                                                             _HTTPStatusCode = HTTPStatusCode.OK;
                                                             break;

                                                     }

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode             = _HTTPStatusCode,
                                                             Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                       = DateTime.UtcNow,
                                                             AccessControlAllowOrigin   = "*",
                                                             AccessControlAllowMethods  = "GET, SET",
                                                             AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                             ETag                       = "1",
                                                             ContentType                = HTTPContentType.JSON_UTF8,
                                                             Content                    = JSONObject.Create(
                                                                                              new JProperty("oldValue",  OldValue),
                                                                                              new JProperty("newValue",  NewValue)
                                                                                          ).ToUTF8Bytes(),
                                                             Connection                 = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion


            #region ~/RNs/{RoamingNetworkId}/ChargingPools

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools

            // ------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools
            // ------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))

                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip                    = Request.QueryString.GetUInt64("skip");
                                                     var take                    = Request.QueryString.GetUInt64("take");
                                                     var expand                  = Request.QueryString.GetStrings("expand", true);
                                                     var expandRoamingNetworks   = expand.Contains("networks")          ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandOperators         = expand.Contains("operators")         ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandChargingStations  = expand.Contains("-chargingstations") ? InfoStatus.ShowIdOnly : InfoStatus.Expand;
                                                     var expandBrands            = expand.Contains("brands")            ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandDataLicenses      = expand.Contains("licenses")          ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount           = RoamingNetwork.ChargingPools.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = RoamingNetwork.ChargingPools.
                                                                                                ToJSON(skip,
                                                                                                       take,
                                                                                                       false,
                                                                                                       expandRoamingNetworks,
                                                                                                       expandOperators,
                                                                                                       expandChargingStations,
                                                                                                       expandBrands,
                                                                                                       expandDataLicenses).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region COUNT       ~/RNs/{RoamingNetworkId}/ChargingPools

            // -----------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/ChargingPools
            // -----------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.COUNT,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = JSONObject.Create(
                                                                                                new JProperty("count",  RoamingNetwork.ChargingPools.ULongCount())
                                                                                            ).ToUTF8Bytes()
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region OPTIONS     ~/RNs/{RoamingNetworkId}/ChargingPools

            // -----------------------------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools
            // -----------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.OPTIONS,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools",
                                                 HTTPDelegate: Request =>

                                                     Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.NoContent,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                         }.AsImmutable)

                                                 );

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools->Id

            // -------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools->Id
            // -------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools->Id",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))

                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = new JArray(RoamingNetwork.ChargingPools.
                                                                                                            Select(pool => pool.Id.ToString()).
                                                                                                            Skip  (Request.QueryString.GetUInt64("skip")).
                                                                                                            Take  (Request.QueryString.GetUInt64("take"))).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = RoamingNetwork.ChargingPools.ULongCount()
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools->AdminStatus

            // -------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools->AdminStatus
            // -------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools->AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))

                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip           = Request.QueryString.GetUInt64                           ("skip");
                                                     var take           = Request.QueryString.GetUInt64                           ("take");
                                                     var sinceFilter    = Request.QueryString.CreateDateTimeFilter<ChargingPoolAdminStatus>("since", (status, timestamp) => status.Status.Timestamp >= timestamp);
                                                     var matchFilter    = Request.QueryString.CreateStringFilter  <ChargingPoolAdminStatus>("match", (status, pattern)   => status.Id.ToString().Contains(pattern));

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount  = RoamingNetwork.ChargingPoolAdminStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = RoamingNetwork.ChargingPoolAdminStatus().
                                                                                                  Where (matchFilter).
                                                                                                  Where (sinceFilter).
                                                                                                  ToJSON(skip, take).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools->Status

            // --------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools->Status
            // --------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools->Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))

                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip           = Request.QueryString.GetUInt64                      ("skip");
                                                     var take           = Request.QueryString.GetUInt64                      ("take");
                                                     var sinceFilter    = Request.QueryString.CreateDateTimeFilter<ChargingPoolStatus>("since", (status, timestamp) => status.Status.Timestamp >= timestamp);
                                                     var matchFilter    = Request.QueryString.CreateStringFilter  <ChargingPoolStatus>("match", (status, pattern)   => status.Id.ToString().Contains(pattern));

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount  = RoamingNetwork.ChargingPoolStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = RoamingNetwork.ChargingPoolStatus().
                                                                                                  Where (matchFilter).
                                                                                                  Where (sinceFilter).
                                                                                                  ToJSON(skip, take).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/DynamicStatusReport

            // --------------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/ChargingPools/DynamicStatusReport
            // --------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/ChargingPools/DynamicStatusReport",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = JSONObject.Create(

                                                                                                new JProperty("count",  RoamingNetwork.ChargingPools.Count()),

                                                                                                new JProperty("status", JSONObject.Create(
                                                                                                    RoamingNetwork.ChargingPools.GroupBy(pool => pool.Status.Value).Select(group =>
                                                                                                        new JProperty(group.Key.ToString().ToLower(),
                                                                                                                      group.Count()))
                                                                                                ))

                                                                                            ).ToUTF8Bytes()
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/...
            // ---------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingPool(WWCPAPI,
                                                                                                     out RoamingNetwork  _RoamingNetwork,
                                                                                                     out ChargingPool    _ChargingPool,
                                                                                                     out HTTPResponse    _HTTPResponse))
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
                                                             AccessControlAllowMethods  = "GET, SET",
                                                             AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                             ETag                       = "1",
                                                             ContentType                = HTTPContentType.JSON_UTF8,
                                                             Content                    = _ChargingPool.ToJSON().ToUTF8Bytes()
                                                         }.AsImmutable);

                                           });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/.../ChargingStations
            // ---------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingPool(WWCPAPI,
                                                                                                     out RoamingNetwork  _RoamingNetwork,
                                                                                                     out ChargingPool    _ChargingPool,
                                                                                                     out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                    = Request.QueryString.GetUInt64("skip");
                                                     var take                    = Request.QueryString.GetUInt64("take");
                                                     var expand                  = Request.QueryString.GetStrings("expand", true);
                                                     var expandRoamingNetworks   = expand.Contains("networks")      ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandOperators         = expand.Contains("operators")     ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandChargingPools     = expand.Contains("chargingpools") ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandEVSEs             = expand.Contains("-evses")        ? InfoStatus.ShowIdOnly : InfoStatus.Expand;
                                                     var expandBrands            = expand.Contains("brands")        ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount   = _ChargingPool.ChargingStations.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = _ChargingPool.ChargingStations.
                                                                                                OrderBy(station => station.Id).
                                                                                                ToJSON (skip,
                                                                                                        take,
                                                                                                        false,
                                                                                                        expandEVSEs,
                                                                                                        expandOperators,
                                                                                                        expandChargingPools,
                                                                                                        expandBrands).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations->AdminStatus

            // -----------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/{ChargingStationId}/ChargingStations->AdminStatus
            // -----------------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/ChargingStations->AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingPool(WWCPAPI,
                                                                                                     out RoamingNetwork  _RoamingNetwork,
                                                                                                     out ChargingPool    _ChargingPool,
                                                                                                     out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip            = Request.QueryString.GetUInt64("skip");
                                                     var take            = Request.QueryString.GetUInt64("take");
                                                     var historysize     = Request.QueryString.GetUInt64("historysize", 1);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount  = _ChargingPool.ChargingStationAdminStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = _ChargingPool.ChargingStationAdminStatus().
                                                                                                 ToJSON(skip,
                                                                                                        take).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations->Status

            // ------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/{ChargingStationId}/ChargingStations->Status
            // ------------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/ChargingStations->Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingPool(WWCPAPI,
                                                                                                     out RoamingNetwork  _RoamingNetwork,
                                                                                                     out ChargingPool    _ChargingPool,
                                                                                                     out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip            = Request.QueryString.GetUInt64("skip");
                                                     var take            = Request.QueryString.GetUInt64("take");
                                                     var historysize     = Request.QueryString.GetUInt64("historysize", 1);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount  = _ChargingPool.ChargingStationStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = _ChargingPool.ChargingStationStatus().
                                                                                                 ToJSON(skip,
                                                                                                        take).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/.../ChargingStations
            // ---------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingPoolAndChargingStation(WWCPAPI,
                                                                                                                       out RoamingNetwork   _RoamingNetwork,
                                                                                                                       out ChargingPool     _ChargingPool,
                                                                                                                       out ChargingStation  _ChargingStation,
                                                                                                                       out HTTPResponse     _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                   = Request.QueryString.GetUInt64("skip");
                                                     var take                   = Request.QueryString.GetUInt64("take");
                                                     var expand                 = Request.QueryString.GetStrings("expand", true);
                                                     var expandRoamingNetworks  = expand.Contains("networks")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandOperators        = expand.Contains("operators") ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandBrands           = expand.Contains("brands")    ? InfoStatus.Expand : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount   = _ChargingStation.EVSEs.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = _ChargingStation.EVSEs.
                                                                                                OrderBy(evse => evse.Id).
                                                                                                ToJSON (skip,
                                                                                                        take,
                                                                                                        false,
                                                                                                        expandRoamingNetworks,
                                                                                                        expandOperators,
                                                                                                        expandBrands).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs/{EVSEId}

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs/{EVSEId}

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/.../ChargingStations
            // ---------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs/{EVSEId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingPoolAndChargingStationAndEVSE(WWCPAPI,
                                                                                                                              out RoamingNetwork   _RoamingNetwork,
                                                                                                                              out ChargingPool     _ChargingPool,
                                                                                                                              out ChargingStation  _ChargingStation,
                                                                                                                              out EVSE             _EVSE,
                                                                                                                              out HTTPResponse     _HTTPResponse))
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
                                                             AccessControlAllowMethods  = "GET",
                                                             AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                             ETag                       = "1",
                                                             ContentType                = HTTPContentType.JSON_UTF8,
                                                             Content                    = _EVSE.ToJSON().ToUTF8Bytes()
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/.../EVSEs
            // ---------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingPool(WWCPAPI,
                                                                                                     out RoamingNetwork  _RoamingNetwork,
                                                                                                     out ChargingPool    _ChargingPool,
                                                                                                     out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                    = Request.QueryString.GetUInt64("skip");
                                                     var take                    = Request.QueryString.GetUInt64("take");
                                                     var expand                  = Request.QueryString.GetStrings("expand", true);
                                                     var expandRoamingNetworks   = expand.Contains("networks")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandOperators         = expand.Contains("operators") ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingPools     = expand.Contains("pools")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingStations  = expand.Contains("stations")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandBrands            = expand.Contains("brands")    ? InfoStatus.Expand : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount   = _ChargingPool.EVSEs.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = _ChargingPool.EVSEs.
                                                                                                OrderBy(evse => evse.Id).
                                                                                                ToJSON (skip,
                                                                                                        take,
                                                                                                        false,
                                                                                                        expandRoamingNetworks,
                                                                                                        expandOperators,
                                                                                                        expandChargingPools,
                                                                                                        expandChargingStations,
                                                                                                        expandBrands).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs->AdminStatus

            // ------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/{ChargingPoolId}/EVSEs->AdminStatus
            // ------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs->AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingPool(WWCPAPI,
                                                                                                     out RoamingNetwork  _RoamingNetwork,
                                                                                                     out ChargingPool    _ChargingPool,
                                                                                                     out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip            = Request.QueryString.GetUInt64("skip");
                                                     var take            = Request.QueryString.GetUInt64("take");
                                                     var historysize     = Request.QueryString.GetUInt64("historysize", 1);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount  = _ChargingPool.EVSEAdminStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = _ChargingPool.EVSEAdminStatus().
                                                                                                 ToJSON(skip,
                                                                                                        take).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs->Status

            // -------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/{ChargingPoolId}/EVSEs->Status
            // -------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs->Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingPool(WWCPAPI,
                                                                                                     out RoamingNetwork  _RoamingNetwork,
                                                                                                     out ChargingPool    _ChargingPool,
                                                                                                     out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip            = Request.QueryString.GetUInt64("skip");
                                                     var take            = Request.QueryString.GetUInt64("take");
                                                     var historysize     = Request.QueryString.GetUInt64("historysize", 1);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount  = _ChargingPool.EVSEStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = _ChargingPool.EVSEStatus().
                                                                                                 ToJSON(skip,
                                                                                                        take).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion


            #region ~/RNs/{RoamingNetworkId}/ChargingStations

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations

            // --------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations
            // --------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/ChargingStations",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip                   = Request.QueryString.GetUInt64("skip");
                                                     var take                   = Request.QueryString.GetUInt64("take");
                                                     var expand                 = Request.QueryString.GetStrings("expand", true);
                                                     var expandRoamingNetworks  = expand.Contains("networks")      ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandOperators        = expand.Contains("operators")     ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandChargingPools    = expand.Contains("chargingpools") ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandEVSEs            = expand.Contains("-evses")        ? InfoStatus.ShowIdOnly : InfoStatus.Expand;
                                                     var expandBrands           = expand.Contains("brands")        ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandDataLicenses     = expand.Contains("licenses")      ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount   = RoamingNetwork.ChargingStations.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = RoamingNetwork.ChargingStations.
                                                                                                OrderBy(station => station.Id).
                                                                                                ToJSON (skip,
                                                                                                        take,
                                                                                                        false,
                                                                                                        expandRoamingNetworks,
                                                                                                        expandOperators,
                                                                                                        expandChargingPools,
                                                                                                        expandEVSEs,
                                                                                                        expandBrands,
                                                                                                        expandDataLicenses).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region COUNT       ~/RNs/{RoamingNetworkId}/ChargingStations

            // --------------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/ChargingStations
            // --------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.COUNT,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/ChargingStations",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = JSONObject.Create(
                                                                                                new JProperty("count",  RoamingNetwork.ChargingStations.ULongCount())
                                                                                            ).ToUTF8Bytes()
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region OPTIONS     ~/RNs/{RoamingNetworkId}/ChargingStations

            // --------------------------------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations
            // --------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.OPTIONS,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingStations",
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.NoContent,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                         }.AsImmutable);

                                                 });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations->Id

            // -------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations->Id
            // -------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingStations->Id",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = new JArray(RoamingNetwork.ChargingStations.
                                                                                                            Select(station => station.Id.ToString()).
                                                                                                            Skip  (Request.QueryString.GetUInt64("skip")).
                                                                                                            Take  (Request.QueryString.GetUInt64("take"))).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = RoamingNetwork.ChargingStations.ULongCount()
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations->AdminStatus

            // ----------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations->AdminStatus
            // ----------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingStations->AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))

                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip           = Request.QueryString.GetUInt64                              ("skip");
                                                     var take           = Request.QueryString.GetUInt64                              ("take");
                                                     var sinceFilter    = Request.QueryString.CreateDateTimeFilter<ChargingStationAdminStatus>("since", (status, timestamp) => status.Status.Timestamp >= timestamp);
                                                     var matchFilter    = Request.QueryString.CreateStringFilter  <ChargingStationAdminStatus>("match", (status, pattern)   => status.Id.ToString().Contains(pattern));

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount  = RoamingNetwork.ChargingStationAdminStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = RoamingNetwork.ChargingStationAdminStatus().
                                                                                                             Where (matchFilter).
                                                                                                             Where (sinceFilter).
                                                                                                             ToJSON(skip, take).
                                                                                                             ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations->Status

            // -----------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations->Status
            // -----------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingStations->Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))

                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip           = Request.QueryString.GetUInt64                         ("skip");
                                                     var take           = Request.QueryString.GetUInt64                         ("take");
                                                     var sinceFilter    = Request.QueryString.CreateDateTimeFilter<ChargingStationStatus>("since", (status, timestamp) => status.Status.Timestamp >= timestamp);
                                                     var matchFilter    = Request.QueryString.CreateStringFilter  <ChargingStationStatus>("match", (status, pattern)   => status.Id.ToString().Contains(pattern));

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount  = RoamingNetwork.ChargingStationStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = RoamingNetwork.ChargingStationStatus().
                                                                                                             Where (matchFilter).
                                                                                                             Where (sinceFilter).
                                                                                                             ToJSON(skip, take).
                                                                                                             ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations/DynamicStatusReport

            // --------------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/ChargingStations/DynamicStatusReport
            // --------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/ChargingStations/DynamicStatusReport",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = JSONObject.Create(

                                                                                                new JProperty("count",  RoamingNetwork.ChargingStations.Count()),

                                                                                                new JProperty("status", JSONObject.Create(
                                                                                                    RoamingNetwork.ChargingStations.GroupBy(station => station.Status.Value).Select(group =>
                                                                                                        new JProperty(group.Key.ToString().ToLower(),
                                                                                                                      group.Count()))
                                                                                                ))

                                                                                            ).ToUTF8Bytes()
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/...
            // ---------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     HTTPResponse     _HTTPResponse;
                                                     RoamingNetwork   _RoamingNetwork;
                                                     ChargingStation  _ChargingStation;

                                                     if (!Request.ParseRoamingNetworkAndChargingStation(WWCPAPI,
                                                                                                        out _RoamingNetwork,
                                                                                                        out _ChargingStation,
                                                                                                        out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                                             Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                       = DateTime.UtcNow,
                                                             AccessControlAllowOrigin   = "*",
                                                             AccessControlAllowMethods  = "GET, SET",
                                                             AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                             ETag                       = "1",
                                                             ContentType                = HTTPContentType.JSON_UTF8,
                                                             Content                    = _ChargingStation.ToJSON().ToUTF8Bytes()
                                                         }.AsImmutable);

                                           });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/.../EVSEs
            // ---------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStation(WWCPAPI,
                                                                                                        out RoamingNetwork   _RoamingNetwork,
                                                                                                        out ChargingStation  _ChargingStation,
                                                                                                        out HTTPResponse     _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                    = Request.QueryString.GetUInt64("skip");
                                                     var take                    = Request.QueryString.GetUInt64("take");
                                                     var expand                  = Request.QueryString.GetStrings("expand", true);
                                                     var expandRoamingNetworks   = expand.Contains("networks")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandOperators         = expand.Contains("operators") ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingPools     = expand.Contains("pools")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingStations  = expand.Contains("stations")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandBrands            = expand.Contains("brands")    ? InfoStatus.Expand : InfoStatus.ShowIdOnly;


                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount   = _ChargingStation.EVSEs.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = _ChargingStation.EVSEs.
                                                                                                OrderBy(evse => evse.Id).
                                                                                                ToJSON (skip,
                                                                                                        take,
                                                                                                        false,
                                                                                                        expandRoamingNetworks,
                                                                                                        expandOperators,
                                                                                                        expandChargingPools,
                                                                                                        expandChargingStations,
                                                                                                        expandBrands).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs->AdminStatus

            // ------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/{ChargingStationId}/EVSEs->AdminStatus
            // ------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs->AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStation(WWCPAPI,
                                                                                                        out RoamingNetwork   _RoamingNetwork,
                                                                                                        out ChargingStation  _ChargingStation,
                                                                                                        out HTTPResponse     _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip            = Request.QueryString.GetUInt64("skip");
                                                     var take            = Request.QueryString.GetUInt64("take");
                                                     var historysize     = Request.QueryString.GetUInt64("historysize", 1);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount  = _ChargingStation.EVSEAdminStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = _ChargingStation.EVSEAdminStatus().
                                                                                                 ToJSON(skip,
                                                                                                        take).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs->Status

            // -------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/{ChargingStationId}/EVSEs->Status
            // -------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs->Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStation(WWCPAPI,
                                                                                                        out RoamingNetwork   _RoamingNetwork,
                                                                                                        out ChargingStation  _ChargingStation,
                                                                                                        out HTTPResponse     _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip            = Request.QueryString.GetUInt64("skip");
                                                     var take            = Request.QueryString.GetUInt64("take");
                                                     var historysize     = Request.QueryString.GetUInt64("historysize", 1);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount  = _ChargingStation.EVSEStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = _ChargingStation.EVSEStatus().
                                                                                                 ToJSON(skip,
                                                                                                        take).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion


            #region ~/RNs/{RoamingNetworkId}/EVSEs

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs

            // ----------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs
            // ----------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/EVSEs",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork  RoamingNetwork,
                                                                                      out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                    = Request.QueryString.GetUInt64("skip");
                                                     var take                    = Request.QueryString.GetUInt64("take");
                                                     var expand                  = Request.QueryString.GetStrings("expand", true);
                                                     var expandRoamingNetworks   = expand.Contains("networks")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandOperators         = expand.Contains("operators") ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingPools     = expand.Contains("pools")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingStations  = expand.Contains("stations")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandBrands            = expand.Contains("brands")    ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandDataLicenses      = expand.Contains("licenses")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;


                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount          = RoamingNetwork.EVSEs.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = RoamingNetwork.EVSEs.
                                                                                                OrderBy(evse => evse.Id).
                                                                                                ToJSON (skip,
                                                                                                        take,
                                                                                                        false,
                                                                                                        expandRoamingNetworks,
                                                                                                        expandOperators,
                                                                                                        expandChargingPools,
                                                                                                        expandChargingStations,
                                                                                                        expandBrands,
                                                                                                        expandDataLicenses).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount,
                                                             Connection                    = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region COUNT       ~/RNs/{RoamingNetworkId}/EVSEs

            // ---------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/EVSEs
            // ---------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.COUNT,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/EVSEs",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = JSONObject.Create(
                                                                                                new JProperty("count",  RoamingNetwork.EVSEs.ULongCount())
                                                                                            ).ToUTF8Bytes(),
                                                             Connection                   = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region OPTIONS     ~/RNs/{RoamingNetworkId}/EVSEs

            // ---------------------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs
            // ---------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.OPTIONS,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/EVSEs",
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.NoContent,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                         }.AsImmutable);

                                                 });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs->Id

            // --------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs->Id
            // --------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/EVSEs->Id",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork  RoamingNetwork,
                                                                                      out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = new JArray(RoamingNetwork.EVSEs.
                                                                                                            Select(evse => evse.Id.ToString()).
                                                                                                            Skip  (Request.QueryString.GetUInt64("skip")).
                                                                                                            Take  (Request.QueryString.GetUInt64("take"))).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = RoamingNetwork.EVSEs.ULongCount(),
                                                             Connection                    = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs->AdminStatus

            // -----------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs->AdminStatus
            // -----------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/EVSEs->AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))

                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip           = Request.QueryString.GetUInt64("skip");
                                                     var take           = Request.QueryString.GetUInt64("take");
                                                     var sinceFilter    = Request.QueryString.CreateDateTimeFilter<EVSEAdminStatus>("since", (status, timestamp) => status.Status.Timestamp >= timestamp);
                                                     var matchFilter    = Request.QueryString.CreateStringFilter  <EVSEAdminStatus>("match", (status, pattern)   => status.Id.ToString().Contains(pattern));

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount  = RoamingNetwork.EVSEAdminStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = RoamingNetwork.EVSEAdminStatus().
                                                                                                  Where (matchFilter).
                                                                                                  Where (sinceFilter).
                                                                                                  ToJSON(skip, take).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs->AdminStatusSchedule

            // -------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs->AdminStatusSchedule
            // -------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/EVSEs->AdminStatusSchedule",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork  RoamingNetwork,
                                                                                      out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip           = Request.QueryString.GetUInt64("skip");
                                                     var take           = Request.QueryString.GetUInt64("take");
                                                     var historysize    = Request.QueryString.GetUInt64("historysize", 1);
                                                     var since          = Request.QueryString.CreateDateTimeFilter<EVSEAdminStatusSchedule>("since", (status, timestamp) => status.StatusSchedule.First().Timestamp >= timestamp);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount  = RoamingNetwork.EVSEAdminStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = RoamingNetwork.EVSEAdminStatus().
                                                                                                  ToJSON(skip,
                                                                                                         take).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs->Status

            // -----------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs->Status
            // -----------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/EVSEs->Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     WWCPAPI.SendGetEVSEStatus(Request);

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))

                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     var skip           = Request.QueryString.GetUInt64              ("skip");
                                                     var take           = Request.QueryString.GetUInt64              ("take");
                                                     var sinceFilter    = Request.QueryString.CreateDateTimeFilter<EVSEStatus>("since", (status, timestamp) => status.Status.Timestamp >= timestamp);
                                                     var matchFilter    = Request.QueryString.CreateStringFilter  <EVSEStatus>("match", (status, pattern)   => status.Id.ToString().Contains(pattern));

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var ExpectedCount  = RoamingNetwork.EVSEStatus().ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = RoamingNetwork.EVSEStatus().
                                                                                                  Where (matchFilter).
                                                                                                  Where (sinceFilter).
                                                                                                  ToJSON(skip, take).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs/DynamicStatusReport

            // --------------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/EVSEs/DynamicStatusReport
            // --------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/DynamicStatusReport",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = JSONObject.Create(

                                                                                                new JProperty("count",  RoamingNetwork.EVSEs.Count()),

                                                                                                new JProperty("status", JSONObject.Create(
                                                                                                    RoamingNetwork.EVSEs.GroupBy(evse => evse.Status.Value).Select(group =>
                                                                                                        new JProperty(group.Key.ToString().ToLower(),
                                                                                                                      group.Count()))
                                                                                                ))

                                                                                            ).ToUTF8Bytes(),
                                                             Connection                   = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            // ---------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check RoamingNetworkId and EVSEId URI parameters

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                             out RoamingNetwork _RoamingNetwork,
                                                                                             out EVSE           _EVSE,
                                                                                             out HTTPResponse   _HTTPResponse))
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
                                                             AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                             AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                             ETag                       = "1",
                                                             ContentType                = HTTPContentType.JSON_UTF8,
                                                             Content                    = _EVSE.ToJSON().ToUTF8Bytes(),
                                                             Connection                 = "close"
                                                         }.AsImmutable);

                                           });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/AdminStatus

            // -----------------------------------------------------------------------
            // curl -v -H "Accept:       application/json" \
            //      http://127.0.0.1:5500/RNs/TEST/EVSEs/DE*GEF*E0001*1/AdminStatus
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     #region Parse RoamingNetworkId and EVSEId parameters

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                             out RoamingNetwork RoamingNetwork,
                                                                                             out EVSE           EVSE,
                                                                                             out HTTPResponse   _HTTPResponse))

                                                         return _HTTPResponse;

                                                     #endregion

                                                     return new HTTPResponse.Builder(Request) {
                                                         HTTPStatusCode              = HTTPStatusCode.OK,
                                                         Server                      = WWCPAPI.HTTPServer.DefaultServerName,
                                                         Date                        = DateTime.UtcNow,
                                                         AccessControlAllowOrigin    = "*",
                                                         AccessControlAllowMethods   = "GET",
                                                         AccessControlAllowHeaders   = "Content-Type, Accept, Authorization",
                                                         ETag                        = "1",
                                                         ContentType                 = HTTPContentType.JSON_UTF8,
                                                         Content                     = EVSE.AdminStatus.
                                                                                           ToJSON().
                                                                                           ToUTF8Bytes(),
                                                         Connection                  = "close"
                                                     };

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status

            // -----------------------------------------------------------------------
            // curl -v -H "Accept:       application/json" \
            //      http://127.0.0.1:5500/RNs/TEST/EVSEs/DE*GEF*E0001*1/Status
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     #region Parse RoamingNetworkId and EVSEId parameters

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                             out RoamingNetwork  RoamingNetwork,
                                                                                             out EVSE            EVSE,
                                                                                             out HTTPResponse    _HTTPResponse))

                                                         return _HTTPResponse;

                                                     #endregion

                                                     return new HTTPResponse.Builder(Request) {
                                                         HTTPStatusCode              = HTTPStatusCode.OK,
                                                         Server                      = WWCPAPI.HTTPServer.DefaultServerName,
                                                         Date                        = DateTime.UtcNow,
                                                         AccessControlAllowOrigin    = "*",
                                                         AccessControlAllowMethods   = "GET",
                                                         AccessControlAllowHeaders   = "Content-Type, Accept, Authorization",
                                                         ETag                        = "1",
                                                         ContentType                 = HTTPContentType.JSON_UTF8,
                                                         Content                     = EVSE.Status.
                                                                                           ToJSON().
                                                                                           ToUTF8Bytes(),
                                                         Connection                  = "close"
                                                     };

                                                 });

            #endregion


            #region RESERVE     ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            #region Documentation

            // RESERVE ~/EVSEs/DE*GEF*E000001*1
            // 
            // {
            //     "ReservationId":      "5c24515b-0a88-1296-32ea-1226ce8a3cd0",               // optional
            //     "StartTime":          "2015-10-20T11:25:43.511Z",                           // optional; default: current timestamp
            //     "Duration":           3600,                                                 // optional; default: 900 [seconds]
            //     "IntendedCharging":   {                                                     // optional; (good for energy management)
            //                               "ProductId":   "AC1"                              // optional; default: "AC1"
            //                               "StartTime":   "2015-10-20T11:30:00.000Z",        // optional; default: reservation start time
            //                               "Duration":    1800,                              // optional; default: reservation duration [seconds]
            //                               "Plugs":       ["TypeFSchuko|Type2Outlet|..."],   // optional;
            //                               "MaxEnergy":   20,                                // optional; [kWh]
            //                               "MaxCosts":    [20, "Euro"],                      // optional;
            //                               "ChargePlan":  "fastest"                          // optional;
            //                           },
            //     "AuthorizedIds":      {                                                     // optional; List of authentication methods...
            //                               "AuthTokens",  ["012345ABCDEF", ...],                // optional; List of RFID Ids
            //                               "eMAIds",   ["DE*GDF*00112233*1", ...],           // optional; List of eMA Ids
            //                               "PINs",     ["123456", ...],                      // optional; List of keypad Pins
            //                               "Liste",    [...]                                 // optional; List of known (white-)lists
            //                           }
            // }

            #endregion

            // -----------------------------------------------------------------------
            // curl -v -X RESERVE -H "Content-Type: application/json" \
            //                    -H "Accept:       application/json"  \
            //      -d "{ \
            //            \"StartTime\":     \"2015-10-20T11:25:43.511Z\", \
            //            \"Duration\":        3600, \
            //            \"IntendedCharging\": { \
            //                                 \"Consumption\": 20, \
            //                                 \"Plug\":        \"TypeFSchuko\" \
            //                               }, \
            //            \"AuthorizedIds\": { \
            //                                 \"AuthTokens\": [\"1AA234BB\", \"012345ABCDEF\"], \
            //                                 \"eMAIds\":  [\"DE*GEF*00112233*1\"], \
            //                                 \"PINs\":    [\"1234\", \"6789\"] \
            //                               } \
            //          }" \
            //      http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 WWCP_HTTPAPI.RESERVE,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     WWCPAPI.SendReserveEVSE(Request);

                                                     #region Check RoamingNetworkId and EVSEId URI parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;
                                                     EVSE            EVSE;

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                             out RoamingNetwork,
                                                                                             out EVSE,
                                                                                             out _HTTPResponse))
                                                         return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                     #endregion

                                                     #region Define (optional) parameters

                                                     ChargingReservation_Id?  ReservationId       = null;
                                                     eMobilityProvider_Id?    ProviderId          = null;
                                                     eMobilityAccount_Id      eMAId               = default(eMobilityAccount_Id);
                                                     DateTime?                StartTime           = null;
                                                     TimeSpan?                Duration            = null;

                                                     // IntendedCharging
                                                     ChargingProduct_Id?      ChargingProductId   = null;
                                                     DateTime?                ChargingStartTime   = null;
                                                     TimeSpan?                CharingDuration     = null;
                                                     PlugTypes?               Plug                = null;
                                                     var                      Consumption         = 0U;

                                                     // AuthorizedIds
                                                     var                      AuthTokens          = new List<Auth_Token>();
                                                     var                      eMAIds              = new List<eMobilityAccount_Id>();
                                                     var                      PINs                = new List<UInt32>();

                                                     #endregion

                                                     #region Parse  (optional) JSON

                                                     if (Request.TryParseJObjectRequestBody(out JObject JSON,
                                                                                            out _HTTPResponse,
                                                                                            AllowEmptyHTTPBody: true))
                                                     {

                                                         #region Check ReservationId        [optional]

                                                         if (JSON.ParseOptionalStruct2("ReservationId",
                                                                                      "ReservationId",
                                                                                      WWCPAPI.HTTPServer.DefaultServerName,
                                                                                      ChargingReservation_Id.TryParse,
                                                                                      out ReservationId,
                                                                                      Request,
                                                                                      out _HTTPResponse))
                                                         {

                                                             if (_HTTPResponse != null)
                                                                 return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                         }

                                                         #endregion

                                                         #region Check ProviderId           [optional]

                                                         if (JSON.ParseOptionalStruct2("ProviderId",
                                                                                      "ProviderId",
                                                                                      WWCPAPI.HTTPServer.DefaultServerName,
                                                                                      eMobilityProvider_Id.TryParse,
                                                                                      out ProviderId,
                                                                                      Request,
                                                                                      out _HTTPResponse))
                                                         {

                                                             if (_HTTPResponse != null)
                                                                 return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                         }

                                                         #endregion

                                                         #region Check eMAId                [mandatory]

                                                         if (!JSON.ParseMandatory("eMAId",
                                                                                  "eMAId",
                                                                                  WWCPAPI.HTTPServer.DefaultServerName,
                                                                                  eMobilityAccount_Id.TryParse,
                                                                                  out eMAId,
                                                                                  Request,
                                                                                  out _HTTPResponse))
                                                         {
                                                             return WWCPAPI.SendEVSEReserved(_HTTPResponse);
                                                         }

                                                         #endregion

                                                         #region Check StartTime            [optional]

                                                         if (JSON.ParseOptional("StartTime",
                                                                                "start time!",
                                                                                WWCPAPI.HTTPServer.DefaultServerName,
                                                                                out StartTime,
                                                                                Request,
                                                                                out _HTTPResponse))
                                                         {

                                                             if (_HTTPResponse != null)
                                                                 return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                             if (StartTime <= DateTime.Now)
                                                                 return WWCPAPI.SendEVSEReserved(
                                                                     new HTTPResponse.Builder(Request) {
                                                                         HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                         ContentType     = HTTPContentType.JSON_UTF8,
                                                                         Content         = new JObject(new JProperty("description", "The starting time must be in the future!")).ToUTF8Bytes()
                                                                     });

                                                         }

                                                         #endregion

                                                         #region Check Duration             [optional]

                                                         if (JSON.ParseOptional("Duration",
                                                                                "Duration",
                                                                                WWCPAPI.HTTPServer.DefaultServerName,
                                                                                out Duration,
                                                                                Request,
                                                                                out _HTTPResponse))
                                                         {

                                                             if (_HTTPResponse != null)
                                                                 return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                         }

                                                         #endregion

                                                         #region Check IntendedCharging     [optional]

                                                         if (JSON.ParseOptional("IntendedCharging",
                                                                                "IntendedCharging",
                                                                                WWCPAPI.HTTPServer.DefaultServerName,
                                                                                out JObject IntendedChargingJSON,
                                                                                Request,
                                                                                out _HTTPResponse))
                                                         {

                                                             if (_HTTPResponse != null)
                                                                 return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                             #region Check ChargingStartTime    [optional]

                                                             if (IntendedChargingJSON.ParseOptional("StartTime",
                                                                                                    "IntendedCharging/StartTime",
                                                                                                    WWCPAPI.HTTPServer.DefaultServerName,
                                                                                                    out ChargingStartTime,
                                                                                                    Request,
                                                                                                    out _HTTPResponse))
                                                             {

                                                                 if (_HTTPResponse != null)
                                                                     return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                             }

                                                             #endregion

                                                             #region Check Duration             [optional]

                                                             if (IntendedChargingJSON.ParseOptional("Duration",
                                                                                                    "IntendedCharging/Duration",
                                                                                                    WWCPAPI.HTTPServer.DefaultServerName,
                                                                                                    out CharingDuration,
                                                                                                    Request,
                                                                                                    out _HTTPResponse))
                                                             {

                                                                 if (_HTTPResponse != null)
                                                                     return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                             }

                                                             #endregion

                                                             #region Check ChargingProductId    [optional]

                                                             if (!JSON.ParseOptional("ChargingProductId",
                                                                                     "IntendedCharging/ChargingProductId",
                                                                                     WWCPAPI.HTTPServer.DefaultServerName,
                                                                                     out ChargingProductId,
                                                                                     Request,
                                                                                     out _HTTPResponse))
                                                             {

                                                                 if (_HTTPResponse != null)
                                                                     return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                             }

                                                             #endregion

                                                             #region Check Plug                 [optional]

                                                             if (IntendedChargingJSON.ParseOptional("Plug",
                                                                                                    "IntendedCharging/ChargingProductId",
                                                                                                    WWCPAPI.HTTPServer.DefaultServerName,
                                                                                                    out Plug,
                                                                                                    Request,
                                                                                                    out _HTTPResponse))
                                                             {

                                                                 if (_HTTPResponse != null)
                                                                     return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                             }

                                                             #endregion

                                                             #region Check Consumption          [optional, kWh]

                                                             if (IntendedChargingJSON.ParseOptional("Consumption",
                                                                                                    "IntendedCharging/Consumption",
                                                                                                    WWCPAPI.HTTPServer.DefaultServerName,
                                                                                                    UInt32.Parse,
                                                                                                    out Consumption,
                                                                                                    Request,
                                                                                                    out _HTTPResponse))
                                                             {

                                                                 if (_HTTPResponse != null)
                                                                     return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                             }

                                                             #endregion

                                                         }

                                                         #endregion

                                                         #region Check AuthorizedIds        [optional]

                                                         if (JSON.ParseOptional("AuthorizedIds",
                                                                                "AuthorizedIds",
                                                                                WWCPAPI.HTTPServer.DefaultServerName,
                                                                                out JObject AuthorizedIdsJSON,
                                                                                Request,
                                                                                out _HTTPResponse))
                                                         {

                                                             if (_HTTPResponse != null)
                                                                 return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                             #region Check AuthTokens   [optional]

                                                             if (AuthorizedIdsJSON.ParseOptional("AuthTokens",
                                                                                                 "AuthorizedIds/AuthTokens",
                                                                                                 WWCPAPI.HTTPServer.DefaultServerName,
                                                                                                 out JArray AuthTokensJSON,
                                                                                                 Request,
                                                                                                 out _HTTPResponse))
                                                             {

                                                                 if (_HTTPResponse == null)
                                                                     return WWCPAPI.SendEVSEReserved(_HTTPResponse);

                                                                 foreach (var jtoken in AuthTokensJSON)
                                                                 {

                                                                     if (!Auth_Token.TryParse(jtoken.Value<String>(), out Auth_Token AuthToken))
                                                                         return WWCPAPI.SendEVSEReserved(
                                                                             new HTTPResponse.Builder(Request) {
                                                                                 HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                                 Date                       = DateTime.UtcNow,
                                                                                 AccessControlAllowOrigin   = "*",
                                                                                 AccessControlAllowMethods  = "RESERVE, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                                 Content                    = new JObject(new JProperty("description", "Invalid AuthorizedIds/RFIDId '" + jtoken.Value<String>() + "' section!")).ToUTF8Bytes()
                                                                             });

                                                                     AuthTokens.Add(AuthToken);

                                                                 }

                                                             }

                                                             #endregion

                                                             #region Check eMAIds       [optional]

                                                             if (AuthorizedIdsJSON.ParseOptional("eMAIds",
                                                                                                 "AuthorizedIds/eMAIds",
                                                                                                 WWCPAPI.HTTPServer.DefaultServerName,
                                                                                                 out JArray eMAIdsJSON,
                                                                                                 Request,
                                                                                                 out _HTTPResponse))
                                                             {

                                                                 if (_HTTPResponse == null)
                                                                     return WWCPAPI.SendEVSEReserved(_HTTPResponse);


                                                                 eMobilityAccount_Id eMAId2;

                                                                 foreach (var jtoken in eMAIdsJSON)
                                                                 {

                                                                     if (!eMobilityAccount_Id.TryParse(jtoken.Value<String>(), out eMAId2))
                                                                         return WWCPAPI.SendEVSEReserved(
                                                                             new HTTPResponse.Builder(Request) {
                                                                                 HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                                 Date                       = DateTime.UtcNow,
                                                                                 AccessControlAllowOrigin   = "*",
                                                                                 AccessControlAllowMethods  = "RESERVE, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                                 Content                    = new JObject(new JProperty("description", "Invalid AuthorizedIds/eMAIds '" + jtoken.Value<String>() + "' section!")).ToUTF8Bytes()
                                                                             });

                                                                     eMAIds.Add(eMAId2);

                                                                 }

                                                             }

                                                             #endregion

                                                             #region Check PINs         [optional]

                                                             if (AuthorizedIdsJSON.ParseOptional("PINs",
                                                                                                 "AuthorizedIds/PINs",
                                                                                                 WWCPAPI.HTTPServer.DefaultServerName,
                                                                                                 out JArray PINsJSON,
                                                                                                 Request,
                                                                                                 out _HTTPResponse))
                                                             {

                                                                 if (_HTTPResponse == null)
                                                                     return WWCPAPI.SendEVSEReserved(_HTTPResponse);


                                                                 UInt32 PIN = 0;

                                                                 foreach (var jtoken in PINsJSON)
                                                                 {

                                                                     if (!UInt32.TryParse(jtoken.Value<String>(), out PIN))
                                                                         return WWCPAPI.SendEVSEReserved(
                                                                             new HTTPResponse.Builder(Request) {
                                                                                 HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                                 Date                       = DateTime.UtcNow,
                                                                                 AccessControlAllowOrigin   = "*",
                                                                                 AccessControlAllowMethods  = "RESERVE, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                                 Content                    = new JObject(new JProperty("description", "Invalid AuthorizedIds/PINs '" + jtoken.Value<String>() + "' section!")).ToUTF8Bytes()
                                                                             });

                                                                     PINs.Add(PIN);

                                                                 }

                                                             }

                                                             #endregion

                                                         }

                                                         #endregion

                                                     }

                                                     if (_HTTPResponse                != null &&
                                                         _HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                                                     {
                                                         return WWCPAPI.SendEVSEReserved(_HTTPResponse);
                                                     }

                                                     #endregion


                                                     var result = await RoamingNetwork.
                                                                            Reserve(ChargingLocation.FromEVSEId(EVSE.Id),
                                                                                    ChargingReservationLevel.EVSE,
                                                                                    StartTime,
                                                                                    Duration,
                                                                                    ReservationId,
                                                                                    ProviderId,
                                                                                    RemoteAuthentication.FromRemoteIdentification(eMAId),
                                                                                    ChargingProductId.HasValue
                                                                                            ? new ChargingProduct(ChargingProductId.Value)
                                                                                            : null,
                                                                                    AuthTokens,
                                                                                    eMAIds,
                                                                                    PINs,

                                                                                    Request.Timestamp,
                                                                                    Request.CancellationToken,
                                                                                    Request.EventTrackingId);


                                                     var Now = DateTime.UtcNow;

                                                     #region Success

                                                     if (result.Result == ReservationResultType.Success)
                                                         return WWCPAPI.SendEVSEReserved(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.Created,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 Location                   = HTTPPath.Parse("~/RNs/" + RoamingNetwork.Id + "/Reservations/" + result.Reservation.Id),
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = new JObject(new JProperty("ReservationId",           result.Reservation.Id.       ToString()),
                                                                                                          new JProperty("StartTime",               result.Reservation.StartTime.ToIso8601()),
                                                                                                          new JProperty("Duration",       (UInt32) result.Reservation.Duration. TotalSeconds)
                                                                                                         ).ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                     #region AlreadInUse

                                                     //else if (result.Result == ReservationResultType.ReservationId_AlreadyInUse)
                                                     //    return new HTTPResponse.Builder(HTTPRequest) {
                                                     //        HTTPStatusCode             = HTTPStatusCode.Conflict,
                                                     //        Server                     = API.HTTPServer.DefaultServerName,
                                                     //        Date                       = DateTime.UtcNow,
                                                     //        AccessControlAllowOrigin   = "*",
                                                     //        AccessControlAllowMethods  = "RESERVE, REMOTESTART, REMOTESTOP, SENDCDR",
                                                     //        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                     //        ContentType                = HTTPContentType.JSON_UTF8,
                                                     //        Content                    = new JObject(new JProperty("description",  "ReservationId is already in use!")).ToUTF8Bytes(),
                                                     //        Connection                 = "close"
                                                     //    };

                                                     #endregion

                                                     #region ...or fail

                                                     else
                                                        return WWCPAPI.SendEVSEReserved(
                                                            new HTTPResponse.Builder(Request) {
                                                                HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                Date                       = Now,
                                                                AccessControlAllowOrigin   = "*",
                                                                AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                Connection                 = "close"
                                                            });

                                                     #endregion

                                                 });

            #endregion

            #region AUTHSTART   ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // -----------------------------------------------------------------------
            // curl -v -X AUTHSTART -H "Content-Type: application/json" \
            //                      -H "Accept:       application/json" \
            //      -d "{ \"AuthToken\":  \"00112233\" }" \
            //      http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 WWCP_HTTPAPI.AUTHSTART,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     WWCPAPI.SendAuthStartEVSERequest(Request);

                                                     #region Parse RoamingNetworkId and EVSEId URI parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;
                                                     EVSE            EVSE;

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                                 out RoamingNetwork,
                                                                                                 out EVSE,
                                                                                                 out _HTTPResponse))
                                                         return WWCPAPI.SendAuthStartEVSEResponse(_HTTPResponse);

                                                     #endregion

                                                     #region Parse JSON

                                                     if (!Request.TryParseJObjectRequestBody(out JObject JSON, out _HTTPResponse))
                                                         return WWCPAPI.SendAuthStartEVSEResponse(_HTTPResponse);

                                                     #region Parse OperatorId         [optional]

                                                     ChargingStationOperator_Id OperatorId;

                                                     if (!JSON.ParseOptional("OperatorId",
                                                                             "Charging Station Operator identification",
                                                                             WWCPAPI.HTTPServer.DefaultServerName,
                                                                             ChargingStationOperator_Id.TryParse,
                                                                             out OperatorId,
                                                                             Request,
                                                                             out _HTTPResponse))

                                                         return WWCPAPI.SendAuthStartEVSEResponse(_HTTPResponse);

                                                     #endregion

                                                     #region Parse AuthToken          [mandatory]

                                                     if (!JSON.ParseMandatory("AuthToken",
                                                                              "authentication token",
                                                                              WWCPAPI.HTTPServer.DefaultServerName,
                                                                              Auth_Token.TryParse,
                                                                              out Auth_Token AuthToken,
                                                                              Request,
                                                                              out _HTTPResponse))

                                                         return WWCPAPI.SendAuthStartEVSEResponse(_HTTPResponse);

                                                     #endregion

                                                     #region Parse SessionId          [optional]

                                                     if (!JSON.ParseOptionalStruct2("SessionId",
                                                                                   "Charging session identification",
                                                                                   WWCPAPI.HTTPServer.DefaultServerName,
                                                                                   ChargingSession_Id.TryParse,
                                                                                   out ChargingSession_Id? SessionId,
                                                                                   Request,
                                                                                   out _HTTPResponse))
                                                     {

                                                         return WWCPAPI.SendAuthStartEVSEResponse(_HTTPResponse);

                                                     }

                                                     #endregion

                                                     #region Parse ChargingProductId  [optional]

                                                     if (!JSON.ParseOptionalStruct2("ChargingProductId",
                                                                                   "Charging product identification",
                                                                                   WWCPAPI.HTTPServer.DefaultServerName,
                                                                                   ChargingProduct_Id.TryParse,
                                                                                   out ChargingProduct_Id? ChargingProductId,
                                                                                   Request,
                                                                                   out _HTTPResponse))
                                                     {

                                                         return WWCPAPI.SendAuthStartEVSEResponse(_HTTPResponse);

                                                     }

                                                     #endregion

                                                     #endregion


                                                     var AuthStartResult = await RoamingNetwork.
                                                                                     AuthorizeStart(LocalAuthentication.FromAuthToken(AuthToken),
                                                                                                    EVSE.Id,
                                                                                                    ChargingProductId.HasValue
                                                                                                        ? new ChargingProduct(ChargingProductId.Value)
                                                                                                        : null,
                                                                                                    SessionId,
                                                                                                    OperatorId,

                                                                                                    Request.Timestamp,
                                                                                                    Request.CancellationToken,
                                                                                                    Request.EventTrackingId);


                                                     var Now = DateTime.UtcNow;

                                                     #region Authorized

                                                     if (AuthStartResult.Result == AuthStartEVSEResultType.Authorized)

                                                         return WWCPAPI.SendAuthStartEVSEResponse(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.OK,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(
                                                                                                  new JProperty("SessionId",         AuthStartResult.SessionId.     ToString()),
                                                                                                  new JProperty("ProviderId",        AuthStartResult.ProviderId.    ToString()),
                                                                                                  new JProperty("AuthorizatorId",    AuthStartResult.AuthorizatorId.ToString()),
                                                                                                  new JProperty("Description",       "Authorized")
                                                                                              ).ToString().
                                                                                                Replace(Environment.NewLine, "").
                                                                                                ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                     #region NotAuthorized

                                                     else if (AuthStartResult.Result == AuthStartEVSEResultType.Error)

                                                         return WWCPAPI.SendAuthStartEVSEResponse(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(
                                                                                                  new JProperty("AuthorizatorId",    AuthStartResult.AuthorizatorId.ToString()),
                                                                                                  new JProperty("Description",       AuthStartResult.Description)
                                                                                              ).ToString().
                                                                                                Replace(Environment.NewLine, "").
                                                                                                ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                     #region Forbidden

                                                     else
                                                         return WWCPAPI.SendAuthStartEVSEResponse(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.Forbidden, //ToDo: Is this smart?
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(
                                                                                                  new JProperty("AuthorizatorId",    AuthStartResult.AuthorizatorId.ToString()),
                                                                                                  new JProperty("Description",       AuthStartResult.Description)
                                                                                              ).ToString().
                                                                                                Replace(Environment.NewLine, "").
                                                                                                ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                 });

            #endregion

            #region AUTHSTOP    ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // -----------------------------------------------------------------------
            // curl -v -X AUTHSTOP -H "Content-Type: application/json" \
            //                     -H "Accept:       application/json" \
            //      -d "{ \"SessionId\":  \"60ce73f6-0a88-1296-3d3d-623fdd276ddc\", \
            //            \"AuthToken\":  \"00112233\" }" \
            //      http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 WWCP_HTTPAPI.AUTHSTOP,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     WWCPAPI.SendAuthStopEVSERequest(Request);

                                                     #region Parse RoamingNetworkId and EVSEId URI parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;
                                                     EVSE            EVSE;

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                                 out RoamingNetwork,
                                                                                                 out EVSE,
                                                                                                 out _HTTPResponse))
                                                         return WWCPAPI.SendAuthStopEVSEResponse(_HTTPResponse);

                                                     #endregion

                                                     #region Parse JSON

                                                     if (!Request.TryParseJObjectRequestBody(out JObject JSON, out _HTTPResponse))
                                                         return WWCPAPI.SendAuthStopEVSEResponse(_HTTPResponse);

                                                     #region Parse SessionId    [mandatory]

                                                     ChargingSession_Id SessionId = default(ChargingSession_Id);

                                                     if (!JSON.ParseMandatory("SessionId",
                                                                              "Charging session identification",
                                                                              WWCPAPI.HTTPServer.DefaultServerName,
                                                                              ChargingSession_Id.TryParse,
                                                                              out SessionId,
                                                                              Request,
                                                                              out _HTTPResponse))

                                                         return WWCPAPI.SendAuthStopEVSEResponse(_HTTPResponse);

                                                     #endregion

                                                     #region Parse AuthToken    [mandatory]

                                                     if (!JSON.ParseMandatory("AuthToken",
                                                                              "Authentication token",
                                                                              WWCPAPI.HTTPServer.DefaultServerName,
                                                                              Auth_Token.TryParse,
                                                                              out Auth_Token AuthToken,
                                                                              Request,
                                                                              out _HTTPResponse))

                                                         return WWCPAPI.SendAuthStopEVSEResponse(_HTTPResponse);

                                                     #endregion

                                                     #region Parse OperatorId   [optional]

                                                     ChargingStationOperator_Id OperatorId;

                                                     if (!JSON.ParseOptional("OperatorId",
                                                                             "Charging Station Operator identification",
                                                                             WWCPAPI.HTTPServer.DefaultServerName,
                                                                             ChargingStationOperator_Id.TryParse,
                                                                             out OperatorId,
                                                                             Request,
                                                                             out _HTTPResponse))

                                                         return WWCPAPI.SendAuthStopEVSEResponse(_HTTPResponse);

                                                     #endregion

                                                     #endregion


                                                     var AuthStopResult = await RoamingNetwork.
                                                                                    AuthorizeStop(SessionId,
                                                                                                  LocalAuthentication.FromAuthToken(AuthToken),
                                                                                                  EVSE.Id,
                                                                                                  OperatorId,

                                                                                                  Request.Timestamp,
                                                                                                  Request.CancellationToken,
                                                                                                  Request.EventTrackingId);


                                                     var Now = DateTime.UtcNow;

                                                     #region Authorized

                                                     if (AuthStopResult.Result == AuthStopEVSEResultType.Authorized)

                                                         return WWCPAPI.SendAuthStopEVSEResponse(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode  = HTTPStatusCode.OK,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType     = HTTPContentType.JSON_UTF8,
                                                                 Content         = JSONObject.Create(
                                                                                       new JProperty("ProviderId",        AuthStopResult.ProviderId.ToString()),
                                                                                       new JProperty("AuthorizatorId",    AuthStopResult.AuthorizatorId.ToString()),
                                                                                       new JProperty("Description",       "Authorized")
                                                                                   ).ToString().
                                                                                     ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                     #region NotAuthorized

                                                     else if (AuthStopResult.Result == AuthStopEVSEResultType.NotAuthorized)

                                                         return WWCPAPI.SendAuthStopEVSEResponse(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(
                                                                                                  new JProperty("AuthorizatorId",    AuthStopResult.AuthorizatorId.ToString()),
                                                                                                  new JProperty("Description",       AuthStopResult.Description)
                                                                                              ).ToString().
                                                                                                ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                     #region Forbidden

                                                     return WWCPAPI.SendAuthStopEVSEResponse(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode             = HTTPStatusCode.Forbidden, //ToDo: Is this smart?
                                                             Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                       = Now,
                                                             AccessControlAllowOrigin   = "*",
                                                             AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                             AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                             ContentType                = HTTPContentType.JSON_UTF8,
                                                             Content                    = JSONObject.Create(
                                                                                              new JProperty("AuthorizatorId",    AuthStopResult.AuthorizatorId.ToString()),
                                                                                              new JProperty("Description",       AuthStopResult.Description)
                                                                                          ).ToString().
                                                                                            ToUTF8Bytes()
                                                         });

                                                     #endregion

                                         });

            #endregion

            #region REMOTESTART ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // -----------------------------------------------------------------------
            // curl -v -X REMOTESTART -H "Content-Type: application/json" \
            //                        -H "Accept:       application/json"  \
            //      -d "{ \"ProviderId\":  \"DE*GDF\", \
            //            \"eMAId\":       \"DE*GDF*00112233*1\" }" \
            //      http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 WWCP_HTTPAPI.REMOTESTART,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     WWCPAPI.SendRemoteStartEVSE(Request);

                                                     #region Get RoamingNetwork and EVSE URI parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;
                                                     EVSE            EVSE;

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                             out RoamingNetwork,
                                                                                             out EVSE,
                                                                                             out _HTTPResponse))
                                                         return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                     #endregion

                                                     #region Parse JSON  [optional]

                                                     ChargingProduct_Id?      ChargingProductId   = null;
                                                     ChargingReservation_Id?  ReservationId       = null;
                                                     ChargingSession_Id?      SessionId           = null;
                                                     eMobilityProvider_Id?    ProviderId          = null;
                                                     eMobilityAccount_Id      eMAId               = default;

                                                     if (Request.TryParseJObjectRequestBody(out JObject JSON, out _HTTPResponse))
                                                     {

                                                         #region Check ChargingProductId  [optional]

                                                         if (!JSON.ParseOptionalStruct2("ChargingProductId",
                                                                                       "Charging product identification",
                                                                                       WWCPAPI.HTTPServer.DefaultServerName,
                                                                                       ChargingProduct_Id.TryParse,
                                                                                       out ChargingProductId,
                                                                                       Request,
                                                                                       out _HTTPResponse))
                                                         {

                                                             return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                         }

                                                         #endregion

                                                         // MaxKWh
                                                         // MaxPrice

                                                         #region Check ReservationId      [optional]

                                                         if (!JSON.ParseOptionalStruct2("ReservationId",
                                                                                       "Charging reservation identification",
                                                                                       WWCPAPI.HTTPServer.DefaultServerName,
                                                                                       ChargingReservation_Id.TryParse,
                                                                                       out ReservationId,
                                                                                       Request,
                                                                                       out _HTTPResponse))
                                                         {

                                                             return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                         }

                                                         #endregion

                                                         #region Parse SessionId          [optional]

                                                         if (!JSON.ParseOptionalStruct2("SessionId",
                                                                                       "Charging session identification",
                                                                                       WWCPAPI.HTTPServer.DefaultServerName,
                                                                                       ChargingSession_Id.TryParse,
                                                                                       out SessionId,
                                                                                       Request,
                                                                                       out _HTTPResponse))
                                                         {

                                                             return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                         }

                                                         #endregion

                                                         #region Parse ProviderId         [optional]

                                                         if (!JSON.ParseOptionalStruct2("ProviderId",
                                                                                       "EV service provider identification",
                                                                                       WWCPAPI.HTTPServer.DefaultServerName,
                                                                                       eMobilityProvider_Id.TryParse,
                                                                                       out ProviderId,
                                                                                       Request,
                                                                                       out _HTTPResponse))
                                                         {

                                                             return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                         }

                                                         #endregion

                                                         #region Parse eMAId             [mandatory]

                                                         if (!JSON.ParseMandatory("eMAId",
                                                                                  "e-Mobility account identification",
                                                                                  WWCPAPI.HTTPServer.DefaultServerName,
                                                                                  eMobilityAccount_Id.TryParse,
                                                                                  out eMAId,
                                                                                  Request,
                                                                                  out _HTTPResponse))

                                                             return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                         #endregion

                                                     }

                                                     else
                                                         return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                     #endregion


                                                     var result = await RoamingNetwork.
                                                                            RemoteStart(ChargingLocation.FromEVSEId(EVSE.Id),
                                                                                        ChargingProductId.HasValue
                                                                                            ? new ChargingProduct(ChargingProductId.Value)
                                                                                            : null,
                                                                                        ReservationId,
                                                                                        SessionId,
                                                                                        ProviderId,
                                                                                        RemoteAuthentication.FromRemoteIdentification(eMAId),
                                                                                      //  null,

                                                                                        Request.Timestamp,
                                                                                        Request.CancellationToken,
                                                                                        Request.EventTrackingId);


                                                     var Now = DateTime.UtcNow;

                                                     #region Success

                                                     if (result.Result == RemoteStartResultType.Success)
                                                         return WWCPAPI.SendEVSERemoteStarted(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.Created,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(
                                                                                                  new JProperty("SessionId",  result.Session.Id.ToString())
                                                                                              ).ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                     #region ...or fail!

                                                     else
                                                         return WWCPAPI.SendEVSERemoteStarted(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(
                                                                                                  result.Session.Id != null
                                                                                                      ? new JProperty("SessionId",  result.Session.Id.ToString())
                                                                                                      : null,
                                                                                                  new JProperty("Result",       result.Result.ToString()),
                                                                                                  result.Message != null
                                                                                                      ? new JProperty("Description",  result.Message)
                                                                                                      : null
                                                                                              ).ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                 });

            #endregion

            #region REMOTESTOP  ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // -----------------------------------------------------------------------
            // curl -v -X REMOTESTOP -H "Content-Type: application/json" \
            //                       -H "Accept:       application/json"  \
            //      -d "{ \"ProviderId\":  \"DE*8BD\", \
            //            \"SessionId\":   \"60ce73f6-0a88-1296-3d3d-623fdd276ddc\", \
            //            \"eMAId\":       \"DE*GDF*00112233*1\" }" \
            //      http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 WWCP_HTTPAPI.REMOTESTOP,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     WWCPAPI.SendRemoteStopEVSE(Request);

                                                     #region Get RoamingNetwork and EVSE URI parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  RoamingNetwork;
                                                     EVSE            EVSE;

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                             out RoamingNetwork,
                                                                                             out EVSE,
                                                                                             out _HTTPResponse))
                                                         return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                     #endregion

                                                     #region Parse JSON

                                                     ChargingSession_Id     SessionId   = default(ChargingSession_Id);
                                                     eMobilityProvider_Id?  ProviderId  = null;
                                                     eMobilityAccount_Id?   eMAId       = null;

                                                     if (!Request.TryParseJObjectRequestBody(out JObject JSON,
                                                                                             out _HTTPResponse,
                                                                                             AllowEmptyHTTPBody: false))

                                                     {

                                                         // Bypass SessionId check for remote safety admins
                                                         // coming from the same ev service provider

                                                         #region Parse SessionId         [mandatory]

                                                         if (!JSON.ParseMandatory("SessionId",
                                                                                  "Charging session identification",
                                                                                  WWCPAPI.HTTPServer.DefaultServerName,
                                                                                  ChargingSession_Id.TryParse,
                                                                                  out SessionId,
                                                                                  Request,
                                                                                  out _HTTPResponse))

                                                             return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                         #endregion

                                                         #region Parse ProviderId         [optional]

                                                         if (!JSON.ParseOptionalStruct2("ProviderId",
                                                                                        "EV service provider identification",
                                                                                        WWCPAPI.HTTPServer.DefaultServerName,
                                                                                        eMobilityProvider_Id.TryParse,
                                                                                        out ProviderId,
                                                                                        Request,
                                                                                        out _HTTPResponse))
                                                         {

                                                             return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                         }

                                                         #endregion

                                                         #region Parse eMAId              [optional]

                                                         if (!JSON.ParseOptionalStruct2("eMAId",
                                                                                       "e-Mobility account identification",
                                                                                       WWCPAPI.HTTPServer.DefaultServerName,
                                                                                       eMobilityAccount_Id.TryParse,
                                                                                       out eMAId,
                                                                                       Request,
                                                                                       out _HTTPResponse))

                                                             return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                         #endregion

                                                         // ReservationHandling

                                                     }

                                                     else
                                                         return WWCPAPI.SendEVSERemoteStarted(_HTTPResponse);

                                                     #endregion


                                                     var result = await RoamingNetwork.RemoteStop(//EVSE.Id,
                                                                                                  SessionId,
                                                                                                  ReservationHandling.Close, // ToDo: Parse this property!
                                                                                                  ProviderId,
                                                                                                  RemoteAuthentication.FromRemoteIdentification(eMAId),

                                                                                                  Request.Timestamp,
                                                                                                  Request.CancellationToken,
                                                                                                  Request.EventTrackingId);


                                                     var Now = DateTime.UtcNow;

                                                     #region Success

                                                     if (result.Result == RemoteStopResultType.Success)
                                                     {

                                                         if (result.ReservationHandling.IsKeepAlive == false)
                                                             return WWCPAPI.SendEVSERemoteStopped(
                                                                 new HTTPResponse.Builder(Request) {
                                                                     HTTPStatusCode             = HTTPStatusCode.NoContent,
                                                                     Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                     Date                       = Now,
                                                                     AccessControlAllowOrigin   = "*",
                                                                     AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                     AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 });

                                                         else
                                                             return WWCPAPI.SendEVSERemoteStopped(
                                                                 new HTTPResponse.Builder(Request) {
                                                                     HTTPStatusCode             = HTTPStatusCode.OK,
                                                                     Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                     Date                       = Now,
                                                                     AccessControlAllowOrigin   = "*",
                                                                     AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                     AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                     ContentType                = HTTPContentType.JSON_UTF8,
                                                                     Content                    = new JObject(
                                                                                                      new JProperty("KeepAlive", (Int32) result.ReservationHandling.KeepAliveTime.Value.TotalSeconds)
                                                                                                  ).ToUTF8Bytes()
                                                                 });

                                                     }

                                                     #endregion

                                                     #region ...or fail

                                                     else
                                                         return WWCPAPI.SendEVSERemoteStopped(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = new JObject(
                                                                                                  new JProperty("description",  result.Result.ToString())
                                                                                              ).ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                 });

            #endregion

            #region SENDCDR     ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // -----------------------------------------------------------------------
            // curl -v -X SENDCDR -H "Content-Type: application/json" \
            //                    -H "Accept: application/json" \
            //      -d "{ \"SessionId\":        \"60ce73f6-0a88-1296-3d3d-623fdd276ddc\", \
            //            \"PartnerProductId\": \"Green Charging 11kWh\", \
            //            \"eMAId\":            \"DE*GDF*00112233*1\" \
            //            \"SessionStart\":     \"2014-08-18T13:12:34.641Z\", \
            //            \"ChargeStart\":      \"2014-08-18T13:12:35.853Z\", \
            //            \"MeterValueStart\":  1200.100, \
            //            \"MeterValueEnd\":    1200.110, \
            //            \"ChargeEnd\":        \"2014-08-18T14:36:11.351Z\", \
            //            \"SessionEnd\":       \"2014-08-18T14:36:12.662Z\" }" \
            //      http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 WWCP_HTTPAPI.SENDCDR,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     WWCPAPI.SendCDRsRequest(Request);

                                                     #region Check RoamingNetworkId and EVSEId URI parameters

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                             out RoamingNetwork  RoamingNetwork,
                                                                                             out EVSE            EVSE,
                                                                                             out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return WWCPAPI.SendCDRsResponse(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     #region Parse JSON

                                                     if (!Request.TryParseJObjectRequestBody(out JObject JSON, out _HTTPResponse))
                                                         return WWCPAPI.SendCDRsResponse(_HTTPResponse);

                                                     #region Parse SessionId          [mandatory]

                                                     if (!JSON.ParseMandatory("SessionId",
                                                                              "charging session identification",
                                                                              WWCPAPI.HTTPServer.DefaultServerName,
                                                                              ChargingSession_Id.TryParse,
                                                                              out ChargingSession_Id SessionId,
                                                                              Request,
                                                                              out _HTTPResponse))
                                                     {
                                                         return WWCPAPI.SendCDRsResponse(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     #region Parse ChargingProductId

                                                     if (JSON.ParseOptionalStruct2("ChargingProductId",
                                                                                  "charging product identification",
                                                                                  WWCPAPI.HTTPServer.DefaultServerName,
                                                                                  ChargingProduct_Id.TryParse,
                                                                                  out ChargingProduct_Id? ChargingProductId,
                                                                                  Request,
                                                                                  out _HTTPResponse))
                                                     {

                                                         if (_HTTPResponse != null)
                                                            return WWCPAPI.SendCDRsResponse(_HTTPResponse);

                                                     }

                                                     #endregion

                                                     #region Parse AuthToken or eMAId

                                                     if (JSON.ParseOptional("AuthToken",
                                                                            "authentication token",
                                                                            WWCPAPI.HTTPServer.DefaultServerName,
                                                                            Auth_Token.TryParse,
                                                                            out Auth_Token AuthToken,
                                                                            Request,
                                                                            out _HTTPResponse))
                                                     {

                                                         if (_HTTPResponse != null)
                                                             return WWCPAPI.SendCDRsResponse(_HTTPResponse);

                                                     }

                                                     if (JSON.ParseOptionalStruct2("eMAId",
                                                                                  "e-mobility account identification",
                                                                                  WWCPAPI.HTTPServer.DefaultServerName,
                                                                                  eMobilityAccount_Id.TryParse,
                                                                                  out eMobilityAccount_Id? eMAId,
                                                                                  Request,
                                                                                  out _HTTPResponse))
                                                     {

                                                         if (_HTTPResponse != null)
                                                             return WWCPAPI.SendCDRsResponse(_HTTPResponse);

                                                     }


                                                     if (AuthToken == null && eMAId == null)
                                                         return new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                             Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date            = DateTime.UtcNow,
                                                             ContentType     = HTTPContentType.JSON_UTF8,
                                                             Content         = new JObject(new JProperty("description", "Missing authentication token or eMAId!")).ToUTF8Bytes()
                                                         };

                                                     #endregion

                                                     #region Parse ChargeStart/End...

                                                     if (!JSON.ParseMandatory("ChargeStart",
                                                                              "Charging start time",
                                                                              WWCPAPI.HTTPServer.DefaultServerName,
                                                                              out DateTime ChargingStart,
                                                                              Request,
                                                                              out _HTTPResponse))

                                                         return WWCPAPI.SendCDRsResponse(_HTTPResponse);

                                                     if (!JSON.ParseMandatory("ChargeEnd",
                                                                              "Charging end time",
                                                                              WWCPAPI.HTTPServer.DefaultServerName,
                                                                              out DateTime ChargingEnd,
                                                                              Request,
                                                                              out _HTTPResponse))

                                                         return WWCPAPI.SendCDRsResponse(_HTTPResponse);

                                                     #endregion

                                                     #region Parse SessionStart/End...

                                                     if (!JSON.ParseMandatory("SessionStart",
                                                                              "Charging start time",
                                                                              WWCPAPI.HTTPServer.DefaultServerName,
                                                                              out DateTime SessionStart,
                                                                              Request,
                                                                              out _HTTPResponse))

                                                         return WWCPAPI.SendCDRsResponse(_HTTPResponse);

                                                     if (!JSON.ParseMandatory("SessionEnd",
                                                                              "Charging end time",
                                                                              WWCPAPI.HTTPServer.DefaultServerName,
                                                                              out DateTime SessionEnd,
                                                                              Request,
                                                                              out _HTTPResponse))

                                                         return WWCPAPI.SendCDRsResponse(_HTTPResponse);

                                                     #endregion

                                                     #region Parse MeterValueStart/End...


                                                     if (!JSON.ParseMandatory("MeterValueStart",
                                                                              "Energy meter start value",
                                                                              WWCPAPI.HTTPServer.DefaultServerName,
                                                                              out Single MeterValueStart,
                                                                              Request,
                                                                              out _HTTPResponse))

                                                         return WWCPAPI.SendCDRsResponse(_HTTPResponse);

                                                     if (!JSON.ParseMandatory("MeterValueEnd",
                                                                              "Energy meter end value",
                                                                              WWCPAPI.HTTPServer.DefaultServerName,
                                                                              out Single MeterValueEnd,
                                                                              Request,
                                                                              out _HTTPResponse))

                                                         return WWCPAPI.SendCDRsResponse(_HTTPResponse);

                                                     #endregion

                                                     #endregion


                                                     var _ChargeDetailRecord = new ChargeDetailRecord(SessionId: SessionId,
                                                                                                      EVSEId:                EVSE.Id,
                                                                                                      EVSE:                  EVSE,
                                                                                                      ChargingProduct:       ChargingProductId.HasValue
                                                                                                                                 ? new ChargingProduct(ChargingProductId.Value)
                                                                                                                                 : null,
                                                                                                      SessionTime:           new StartEndDateTime(SessionStart, SessionEnd),
                                                                                                      AuthenticationStart:   AuthToken != null
                                                                                                                                 ? (AAuthentication) LocalAuthentication. FromAuthToken(AuthToken)
                                                                                                                                 : (AAuthentication) RemoteAuthentication.FromRemoteIdentification(eMAId.Value),
                                                                                                      //ChargingTime:        new StartEndDateTime(ChargingStart.Value, ChargingEnd.Value),
                                                                                                      EnergyMeteringValues:  new List<Timestamped<Single>>() {
                                                                                                                                 new Timestamped<Single>(ChargingStart, MeterValueStart),
                                                                                                                                 new Timestamped<Single>(ChargingEnd,   MeterValueEnd)
                                                                                                                            });

                                                     var result = await RoamingNetwork.
                                                                            SendChargeDetailRecords(new ChargeDetailRecord[] { _ChargeDetailRecord },
                                                                                                    TransmissionTypes.Enqueue,

                                                                                                    Request.Timestamp,
                                                                                                    Request.CancellationToken,
                                                                                                    Request.EventTrackingId);


                                                     var Now = DateTime.UtcNow;

                                                     #region Forwarded

                                                     if (result.Result == SendCDRsResultTypes.Success)
                                                         return WWCPAPI.SendCDRsResponse(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.OK,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(
                                                                                                  new JProperty("Status",          "forwarded"),
                                                                                                  new JProperty("AuthorizatorId",  result.AuthorizatorId.ToString())
                                                                                              ).ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                     #region NotForwared

                                                     else if (result.Result == SendCDRsResultTypes.Error)
                                                         return WWCPAPI.SendCDRsResponse(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.OK,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(
                                                                                                  new JProperty("Status",    "Not forwarded")
                                                                                              ).ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                     #region ...or fail!

                                                     else
                                                         return WWCPAPI.SendCDRsResponse(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode             = HTTPStatusCode.NotFound,
                                                                 Server                     = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                       = Now,
                                                                 AccessControlAllowOrigin   = "*",
                                                                 AccessControlAllowMethods  = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR",
                                                                 AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                                 Content                    = JSONObject.Create(
                                                                                                  new JProperty("SessionId",       SessionId.ToString()),
                                                                                                  new JProperty("Description",     result.Description),
                                                                                                  new JProperty("AuthorizatorId",  result.AuthorizatorId.ToString())
                                                                                              ).ToUTF8Bytes()
                                                             });

                                                     #endregion

                                                 });

            #endregion

            #region OPTIONS     ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // --------------------------------------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            // --------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.OPTIONS,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                                                 HTTPDelegate: Request => {

                                                     #region Check RoamingNetworkId and EVSEId URI parameters

                                                     HTTPResponse    _HTTPResponse;
                                                     RoamingNetwork  _RoamingNetwork;
                                                     EVSE            _EVSE;

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                             out _RoamingNetwork,
                                                                                             out _EVSE,
                                                                                             out _HTTPResponse))

                                                         return Task.FromResult(_HTTPResponse);

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.NoContent,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, RESERVE, AUTHSTART, AUTHSTOP, REMOTESTART, REMOTESTOP, SENDCDR, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                         }.AsImmutable);

                                                 });

            #endregion


            #region SET         ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/AdminStatus

            // -----------------------------------------------------------------------
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"CurrentStatus\":  \"OutOfService\" }" \
            //      http://127.0.0.1:5500/RNs/EST/EVSEs/DE*GEF*E0001*1/AdminStatus
            // -----------------------------------------------------------------------
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"CurrentStatus\":  \"OutOfService\" }" \
            //      http://127.0.0.1:3004/RNs/Prod/EVSEs/DE*BDO*EVSE*CI*TESTS*A*1/AdminStatus
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.SET,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     #region Parse RoamingNetworkId and EVSEId parameters

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                             out RoamingNetwork  RoamingNetwork,
                                                                                             out EVSE            EVSE,
                                                                                             out HTTPResponse    _HTTPResponse))

                                                         return _HTTPResponse;

                                                     #endregion

                                                     #region Parse JSON

                                                     if (!Request.TryParseJObjectRequestBody(out JObject JSON, out _HTTPResponse))
                                                         return _HTTPResponse;

                                                     #region Parse CurrentStatus  [optional]

                                                     if (JSON.ParseOptional("CurrentStatus",
                                                                            "EVSE admin status",
                                                                            WWCPAPI.HTTPServer.DefaultServerName,
                                                                            out EVSEAdminStatusTypes? CurrentStatus,
                                                                            Request,
                                                                            out _HTTPResponse))
                                                     {

                                                         if (_HTTPResponse != null)
                                                             return _HTTPResponse;

                                                     }

                                                     #endregion

                                                     #region Parse StatusList     [optional]

                                                     Timestamped<EVSEAdminStatusTypes>[] StatusList  = null;

                                                     if (JSON.ParseOptional("StatusList",
                                                                            "status list",
                                                                            WWCPAPI.HTTPServer.DefaultServerName,
                                                                            out JObject JSONStatusList,
                                                                            Request,
                                                                            out _HTTPResponse))
                                                     {

                                                         if (_HTTPResponse != null)
                                                             return _HTTPResponse;

                                                         if (JSONStatusList != null)
                                                         {

                                                             try
                                                             {

                                                                 StatusList = JSONStatusList.
                                                                                  Values<JProperty>().
                                                                                  Select(jproperty => new Timestamped<EVSEAdminStatusTypes>(
                                                                                                          DateTime.Parse(jproperty.Name),
                                                                                                          (EVSEAdminStatusTypes) Enum.Parse(typeof(EVSEAdminStatusTypes), jproperty.Value.ToString())
                                                                                                      )).
                                                                                  OrderBy(status   => status.Timestamp).
                                                                                  ToArray();

                                                             }
                                                             catch (Exception)
                                                             {
                                                                 // Will send the below BadRequest HTTP reply...
                                                             }

                                                         }

                                                         if (JSONStatusList == null || StatusList == null || !StatusList.Any())
                                                             return new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                 Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date            = DateTime.UtcNow,
                                                                 ContentType     = HTTPContentType.JSON_UTF8,
                                                                 Content         = new JObject(
                                                                                       new JProperty("description", "Invalid status list!")
                                                                                   ).ToUTF8Bytes()
                                                             };

                                                     }

                                                     #endregion

                                                     #region Fail, if both CurrentStatus and StatusList are missing...

                                                     if (!CurrentStatus.HasValue && StatusList == null)
                                                     {

                                                         return new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                             Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date            = DateTime.UtcNow,
                                                             ContentType     = HTTPContentType.JSON_UTF8,
                                                             Content         = new JObject(
                                                                                   new JProperty("description", "Either a 'CurrentStatus' or a 'StatusList' must be send!")
                                                                               ).ToUTF8Bytes()
                                                         };

                                                     }

                                                     #endregion

                                                     #endregion


                                                     if (StatusList == null)
                                                         StatusList = new Timestamped<EVSEAdminStatusTypes>[] {
                                                                          new Timestamped<EVSEAdminStatusTypes>(Request.Timestamp, CurrentStatus.Value)
                                                                      };

                                                     RoamingNetwork.SetEVSEAdminStatus(EVSE.Id, StatusList);


                                                     return new HTTPResponse.Builder(Request) {
                                                         HTTPStatusCode  = HTTPStatusCode.OK,
                                                         Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                         Date            = DateTime.UtcNow,
                                                         Connection      = "close"
                                                     };

                                                 });

            #endregion

            #region SET         ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status

            // -----------------------------------------------------------------------
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"CurrentStatus\":  \"OutOfService\" }" \
            //      http://127.0.0.1:5500/RNs/TEST/EVSEs/DE*GEF*EVSE*ALPHA*ONE*1/Status
            // -----------------------------------------------------------------------
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"StatusList\":  { \
            //              \"2014-10-13T22:14:01.862Z\": \"OutOfService\", \
            //              \"2014-10-13T21:32:15.386Z\": \"Charging\"  \
            //          }" \
            //      http://127.0.0.1:5500/RNs/TEST/EVSEs/DE*GEF*EVSE*ALPHA*ONE*1/Status
            // -----------------------------------------------------------------------
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"CurrentStatus\":  \"Charging\" }"     \
            //      http://127.0.0.1:3004/RNs/Prod/EVSEs/DE*BDO*EVSE*CI*TESTS*A*1/Status
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.SET,
                                                 _URIPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async Request => {

                                                     #region Check RoamingNetworkId and EVSEId URI parameters

                                                     if (!Request.ParseRoamingNetworkAndEVSE(WWCPAPI,
                                                                                             out RoamingNetwork  RoamingNetwork,
                                                                                             out EVSE            EVSE,
                                                                                             out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return _HTTPResponse;
                                                     }

                                                     #endregion

                                                     #region Parse JSON

                                                     if (!Request.TryParseJObjectRequestBody(out JObject JSON, out _HTTPResponse))
                                                         return _HTTPResponse;

                                                     #region Parse CurrentStatus  [optional]

                                                     if (JSON.ParseOptional("CurrentStatus",
                                                                            "EVSE status",
                                                                            WWCPAPI.HTTPServer.DefaultServerName,
                                                                            out EVSEStatusTypes? CurrentStatus,
                                                                            Request,
                                                                            out _HTTPResponse))
                                                     {

                                                         if (_HTTPResponse != null)
                                                            return _HTTPResponse;

                                                     }

                                                     #endregion

                                                     #region Parse StatusList     [optional]

                                                     Timestamped<EVSEStatusTypes>[] StatusList = null;

                                                     if (JSON.ParseOptional("StatusList",
                                                                            "status list",
                                                                            WWCPAPI.HTTPServer.DefaultServerName,
                                                                            out JObject JSONStatusList,
                                                                            Request,
                                                                            out _HTTPResponse))
                                                     {

                                                         if (_HTTPResponse != null)
                                                             return _HTTPResponse;

                                                         if (JSONStatusList != null)
                                                         {

                                                             try
                                                             {

                                                                 StatusList = JSONStatusList.
                                                                                  Values<JProperty>().
                                                                                  Select(jproperty => new Timestamped<EVSEStatusTypes>(
                                                                                                          DateTime.Parse(jproperty.Name),
                                                                                                          (EVSEStatusTypes) Enum.Parse(typeof(EVSEStatusTypes), jproperty.Value.ToString())
                                                                                                      )).
                                                                                  OrderBy(status   => status.Timestamp).
                                                                                  ToArray();

                                                             }
                                                             catch (Exception)
                                                             {
                                                                 // Will send the below BadRequest HTTP reply...
                                                             }

                                                         }

                                                         if (JSONStatusList == null || StatusList == null || !StatusList.Any())
                                                             return new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                 Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date            = DateTime.UtcNow,
                                                                 ContentType     = HTTPContentType.JSON_UTF8,
                                                                 Content         = new JObject(
                                                                                       new JProperty("description", "Invalid status list!")
                                                                                   ).ToUTF8Bytes()
                                                             };

                                                     }

                                                     #endregion

                                                     #region Fail, if both CurrentStatus and StatusList are missing...

                                                     if (CurrentStatus == EVSEStatusTypes.Unspecified &&
                                                         StatusList    == null)
                                                     {

                                                         return new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                             Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date            = DateTime.UtcNow,
                                                             ContentType     = HTTPContentType.JSON_UTF8,
                                                             Content         = new JObject(
                                                                                   new JProperty("description", "Either a 'CurrentStatus' or a 'StatusList' must be send!")
                                                                               ).ToUTF8Bytes()
                                                         };

                                                     }

                                                     #endregion

                                                     #endregion


                                                     if (StatusList == null)
                                                         StatusList = new Timestamped<EVSEStatusTypes>[] {
                                                                          new Timestamped<EVSEStatusTypes>(Request.Timestamp, CurrentStatus.Value)
                                                                      };

                                                     RoamingNetwork.SetEVSEStatus(EVSE.Id, StatusList);


                                                     return new HTTPResponse.Builder(Request) {
                                                         HTTPStatusCode  = HTTPStatusCode.OK,
                                                         Server          = WWCPAPI.HTTPServer.DefaultServerName,
                                                         Date            = DateTime.UtcNow,
                                                         Connection      = "close"
                                                     };

                                                 });

            #endregion

            #endregion


            #region ~/RNs/{RoamingNetworkId}/ChargingSessions

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingSessions

            // ----------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingSessions
            // ----------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingSessions",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork  RoamingNetwork,
                                                                                      out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                    = Request.QueryString.GetUInt64("skip");
                                                     var take                    = Request.QueryString.GetUInt64("take");
                                                     var expand                  = Request.QueryString.GetStrings("expand", true);
                                                     var expandRoamingNetworks   = expand.Contains("networks")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandOperators         = expand.Contains("operators") ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingPools     = expand.Contains("pools")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingStations  = expand.Contains("stations")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandBrands            = expand.Contains("brands")    ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandDataLicenses      = expand.Contains("licenses")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;


                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount          = RoamingNetwork.ChargingSessions.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode               = HTTPStatusCode.OK,
                                                             Server                       = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                         = DateTime.UtcNow,
                                                             AccessControlAllowOrigin     = "*",
                                                             AccessControlAllowMethods    = "GET, COUNT, OPTIONS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = RoamingNetwork.ChargingSessions.
                                                                                                OrderBy(session => session.Id).
                                                                                                ToJSON (skip,
                                                                                                        take,
                                                                                                        false).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount,
                                                             Connection                    = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingSessions->Id

            // -------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Prod/ChargingSessions->Id
            // -------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RoamingNetworkId}/ChargingSessions->Id",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork  RoamingNetwork,
                                                                                      out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                = HTTPStatusCode.OK,
                                                             Server                        = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                          = DateTime.UtcNow,
                                                             AccessControlAllowOrigin      = "*",
                                                             AccessControlAllowMethods     = "GET",
                                                             AccessControlAllowHeaders     = "Content-Type, Accept, Authorization",
                                                             ETag                          = "1",
                                                             ContentType                   = HTTPContentType.JSON_UTF8,
                                                             Content                       = new JArray(RoamingNetwork.ChargingSessions.
                                                                                                            Select(seession => seession.Id.ToString()).
                                                                                                            Skip  (Request.QueryString.GetUInt64("skip")).
                                                                                                            Take  (Request.QueryString.GetUInt64("take"))).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = RoamingNetwork.ChargingSessions.ULongCount(),
                                                             Connection                    = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion


        }

    }

}
