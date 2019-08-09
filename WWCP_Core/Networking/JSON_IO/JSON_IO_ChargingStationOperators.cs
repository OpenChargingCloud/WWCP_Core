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
        public static void Attach_JSON_IO_ChargingOperators(this WWCP_HTTPAPI  WWCPAPI,
                                                            HTTPHostname?      Hostname   = null,
                                                            HTTPPath?           URIPrefix  = null)
        {

            var _Hostname   = Hostname  ?? HTTPHostname.Any;
            var _URIPrefix  = URIPrefix ?? HTTPPath.Parse("/");

            #region ~/RNs/{RNId}/ChargingStationOperators

            #region GET         ~/RNs/{RNId}/ChargingStationOperators

            // -----------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/ChargingStationOperators
            // -----------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork _RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                      = Request.QueryString.GetUInt64("skip");
                                                     var take                      = Request.QueryString.GetUInt64("take");
                                                     var expand                    = Request.QueryString.GetStrings("expand", true);
                                                     var expandRoamingNetworkId    = expand.Contains("network")           ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingPoolIds     = expand.Contains("chargingpools")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingStationIds  = expand.Contains("chargingstations")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandEVSEIds             = expand.Contains("EVSEs")             ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandBrandIds            = expand.Contains("brands")            ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandDataLicenses        = expand.Contains("licenses")          ? InfoStatus.Expand : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount = _RoamingNetwork.ChargingStationOperators.ULongCount();

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
                                                             Content                      = _RoamingNetwork.ChargingStationOperators.
                                                                                                OrderBy(cso => cso.Id).
                                                                                                ToJSON(skip,
                                                                                                       take,
                                                                                                       false,
                                                                                                       expandRoamingNetworkId,
                                                                                                       expandChargingPoolIds,
                                                                                                       expandChargingStationIds,
                                                                                                       expandEVSEIds,
                                                                                                       expandBrandIds,
                                                                                                       expandDataLicenses).
                                                                                                ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount,
                                                             Connection                    = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region COUNT       ~/RNs/{RNId}/ChargingStationOperators

            // ----------------------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:3004/RNs/{RNId}/ChargingStationOperators
            // ----------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.COUNT,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork _RoamingNetwork,
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
                                                             AccessControlAllowMethods    = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders    = "Content-Type, Accept, Authorization",
                                                             ETag                         = "1",
                                                             ContentType                  = HTTPContentType.JSON_UTF8,
                                                             Content                      = JSONObject.Create(
                                                                                                new JProperty("count",  _RoamingNetwork.ChargingStationOperators.ULongCount())
                                                                                            ).ToUTF8Bytes(),
                                                             Connection                   = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RNId}/ChargingStationOperators->AdminStatus

            // -----------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/ChargingStationOperators->AdminStatus
            // -----------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators->AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork _RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip         = Request.QueryString.GetUInt64("skip");
                                                     var take         = Request.QueryString.GetUInt64("take");
                                                     var historysize  = Request.QueryString.GetUInt64("historysize", 1);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount = _RoamingNetwork.ChargingStationOperatorAdminStatus.ULongCount();

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
                                                             Content                       = _RoamingNetwork.ChargingStationOperatorAdminStatus.
                                                                                                 OrderBy(kvp => kvp.Key).
                                                                                                 ToJSON (skip,
                                                                                                         take,
                                                                                                         historysize).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount,
                                                             Connection                    = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RNId}/ChargingStationOperators->Status

            // -------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/ChargingStationOperators->Status
            // -------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators->Status",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork _RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip         = Request.QueryString.GetUInt64("skip");
                                                     var take         = Request.QueryString.GetUInt64("take");
                                                     var historysize  = Request.QueryString.GetUInt64("historysize", 1);

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount = _RoamingNetwork.ChargingStationOperatorStatus.ULongCount();

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
                                                             Content                       = _RoamingNetwork.ChargingStationOperatorStatus.
                                                                                                 OrderBy(kvp => kvp.Key).
                                                                                                 ToJSON (skip,
                                                                                                         take,
                                                                                                         historysize).
                                                                                                 ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems  = _ExpectedCount,
                                                             Connection                    = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RNId}/ChargingStationOperators/{CSOId}

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}

            WWCPAPI.HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check HTTP parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStationOperator(WWCPAPI,
                                                                                                                out RoamingNetwork          _RoamingNetwork,
                                                                                                                out ChargingStationOperator _ChargingStationOperator,
                                                                                                                out HTTPResponse            _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode              = HTTPStatusCode.OK,
                                                             Server                      = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                        = DateTime.UtcNow,
                                                             AccessControlAllowOrigin    = "*",
                                                             AccessControlAllowMethods   = "GET, CREATE, DELETE",
                                                             AccessControlAllowHeaders   = "Content-Type, Accept, Authorization",
                                                             ETag                        = "1",
                                                             ContentType                 = HTTPContentType.JSON_UTF8,
                                                             Content                     = _ChargingStationOperator.ToJSON().ToUTF8Bytes(),
                                                             Connection                  = "close"
                                                         }.AsImmutable);

                                           });

            #endregion

            #endregion

            #region ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingPools

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingPools

            // -----------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/ChargingStationOperators/{CSOId}/ChargingPools
            // -----------------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingPools",
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

                                                     var skip                    = Request.QueryString.GetUInt64("skip");
                                                     var take                    = Request.QueryString.GetUInt64("take");
                                                     var expand                  = Request.QueryString.GetStrings("expand", true);
                                                     var expandChargingStations  = !expand.Contains("-chargingstations") ? InfoStatus.ShowIdOnly : InfoStatus.Expand;
                                                     var expandRoamingNetworks   =  expand.Contains("networks")          ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandOperators         =  expand.Contains("operators")         ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;
                                                     var expandBrands            =  expand.Contains("brands")            ? InfoStatus.Expand     : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount = RoamingNetwork.ChargingPools.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = RoamingNetwork.ChargingPools.
                                                                                                  ToJSON(skip,
                                                                                                         take,
                                                                                                         false,
                                                                                                         expandRoamingNetworks,
                                                                                                         expandOperators,
                                                                                                         expandChargingStations,
                                                                                                         expandBrands).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = _ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region CREATE      ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingPools/{ChargingPoolId}

            // ----------------------------------------------------------------------------------------------------------------
            // curl -v -X CREATE -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test2/ChargingPools/{ChargingPoolId}
            // ----------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.CREATE,
                                                 _URIPrefix + "RNs/{RNId}/ChargingPools/{ChargingPoolId}",
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

                                                     if (!Request.ParseRoamingNetworkAndChargingPool(WWCPAPI,
                                                                                                     out RoamingNetwork _RoamingNetwork,
                                                                                                     out ChargingPool   _ChargingPool,
                                                                                                     out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     #region Parse optional JSON

                                                     I18NString DescriptionI18N = null;

                                                     if (Request.TryParseJObjectRequestBody(out JObject JSON,
                                                                                            out _HTTPResponse,
                                                                                            AllowEmptyHTTPBody: true))
                                                     {

                                                         if (!JSON.ParseOptional("description",
                                                                                 "description",
                                                                                 WWCPAPI.HTTPServer.DefaultServerName,
                                                                                 out DescriptionI18N,
                                                                                 Request,
                                                                                 out _HTTPResponse))
                                                         {
                                                             return Task.FromResult(WWCPAPI.SendOnCreateRoamingNetwork_ResponseLog(_HTTPResponse));
                                                         }

                                                     }

                                                     #endregion


                                                     _RoamingNetwork = WWCPAPI.CreateNewRoamingNetwork(Request.Host,
                                                                                                       _RoamingNetwork.Id,
                                                                                                       I18NString.Empty,
                                                                                                       Description: DescriptionI18N ?? I18NString.Empty);


                                                     return Task.FromResult(
                                                         WWCPAPI.SendOnCreateRoamingNetwork_ResponseLog(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode              = HTTPStatusCode.Created,
                                                                 Server                      = WWCPAPI.HTTPServer.DefaultServerName,
                                                                 Date                        = DateTime.UtcNow,
                                                                 AccessControlAllowOrigin    = "*",
                                                                 AccessControlAllowMethods   = "GET, CREATE, DELETE",
                                                                 AccessControlAllowHeaders   = "Content-Type, Accept, Authorization",
                                                                 ETag                        = "1",
                                                                 ContentType                 = HTTPContentType.JSON_UTF8,
                                                                 Content                     = _RoamingNetwork.ToJSON().ToUTF8Bytes(),
                                                                 Connection                  = "close"
                                                             }.AsImmutable));

                                                 });

            #endregion

            #region SET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingPools/{ChargingPoolId}/AdminStatus

            // ------------------------------------------------------------------------------------------
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"newstatus\":  \"OutOfService\" }" \
            //      http://127.0.0.1:3004/RNs/Test/ChargingStations/DE*GEF*P000001*1/AdminStatus
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.SET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingPools/{ChargingPoolId}/AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check RoamingNetworkId and EVSEId URI parameters

                                                     if (!Request.ParseRoamingNetwork(WWCPAPI,
                                                                                      out RoamingNetwork _RoamingNetwork,
                                                                                      out HTTPResponse   _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     #region Parse ChargingPoolId

                                                     if (!ChargingPool_Id.TryParse(Request.ParsedURIParameters[1],
                                                                                   out ChargingPool_Id ChargingPoolId))
                                                     {

                                                         //Log.Timestamp("Bad request: Invalid ChargingPoolId query parameter!");

                                                         return Task.FromResult(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                 ContentType     = HTTPContentType.JSON_UTF8,
                                                                 Content         = new JObject(//new JProperty("@context",    "http://wwcp.graphdefined.org/contexts/BadRequest.jsonld"),
                                                                                               new JProperty("description", "Invalid ChargingPoolId query parameter!")).ToUTF8Bytes()
                                                             }.AsImmutable);

                                                     }

                                                     if (!_RoamingNetwork.ContainsChargingPool(ChargingPoolId))
                                                     {

                                                         //Log.Timestamp("Bad request: Unknown ChargingPoolId query parameter!");

                                                         return Task.FromResult(
                                                             new HTTPResponse.Builder(Request) {
                                                                 HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                 ContentType     = HTTPContentType.JSON_UTF8,
                                                                 Content         = new JObject(//new JProperty("@context",    "http://wwcp.graphdefined.org/contexts/BadRequest.jsonld"),
                                                                                               new JProperty("description", "Unknown ChargingPoolId query parameter!")).ToUTF8Bytes()
                                                             }.AsImmutable);

                                                     }

                                                     #endregion

                                                     #region Parse JSON and new charging pool admin status

                                                     if (!Request.TryParseJObjectRequestBody(out JObject JSON, out _HTTPResponse))
                                                         return Task.FromResult(_HTTPResponse);

                                                     if (!JSON.ParseMandatoryEnum("newstatus",
                                                                                  "charging pool admin status",
                                                                                  WWCPAPI.HTTPServer.DefaultServerName,
                                                                                  out ChargingPoolAdminStatusTypes NewChargingPoolAdminStatus,
                                                                                  Request,
                                                                                  out _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     //Log.WriteLine("SetChargingPoolAdminStatus : " + RoamingNetwork.Id + " / " + ChargingPoolId + " => " + NewChargingPoolAdminStatus);

                                                     WWCPAPI.HTTPServer.Get(Semantics.DebugLog).
                                                         SubmitSubEvent("SetChargingPoolAdminStatusRequest",
                                                                        new JObject(
                                                                            new JProperty("Timestamp",       DateTime.UtcNow.ToIso8601()),
                                                                            new JProperty("RoamingNetwork",  _RoamingNetwork.ToString()),
                                                                            new JProperty("ChargingPoolId",  ChargingPoolId.ToString()),
                                                                            new JProperty("NewStatus",       NewChargingPoolAdminStatus.ToString())
                                                                        ).ToString().
                                                                          Replace(Environment.NewLine, "")).Wait();


                                                     _RoamingNetwork.ChargingStationOperators.ForEach(evseoperator => {

                                                         if (evseoperator.ContainsChargingPool(ChargingPoolId))
                                                             evseoperator.SetChargingPoolAdminStatus(ChargingPoolId, new Timestamped<ChargingPoolAdminStatusTypes>(NewChargingPoolAdminStatus), SendUpstream: true);

                                                     });

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode  = HTTPStatusCode.OK,
                                                             Date            = DateTime.UtcNow,
                                                             Connection      = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStations/{StationId}

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStations/{StationId}

            WWCPAPI.HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStations/{StationId}",
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

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode              = HTTPStatusCode.OK,
                                                             Server                      = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                        = DateTime.UtcNow,
                                                             AccessControlAllowOrigin    = "*",
                                                             AccessControlAllowMethods   = "GET, SET",
                                                             AccessControlAllowHeaders   = "Content-Type, Accept, Authorization",
                                                             ETag                        = "1",
                                                             ContentType                 = HTTPContentType.JSON_UTF8,
                                                             Content                     = _ChargingStation.ToJSON().ToUTF8Bytes(),
                                                             Connection                  = "close"
                                                         }.AsImmutable);

                                           });

            #endregion

            #region SET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStations/{StationId}/AdminStatus

            // ------------------------------------------------------------------------------------------
            //
            // ==== QA1 ===============================================================================
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"newstatus\":  \"OutOfService\" }" \
            //      http://127.0.0.1:3004/RNs/Test/ChargingStations/49*822*S013361835
            //
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"newstatus\":  \"OutOfService\" }" \
            //      http://127.0.0.1:3004/RNs/Test/ChargingStations/49*822*S013361835
            //
            // ==== PROD ==============================================================================
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"newstatus\":  \"OutOfService\" }" \
            //      http://127.0.0.1:3004/RNs/Test/ChargingStations/DE*GEF*S000001*1
            //
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"newstatus\":  \"OutOfService\" }" \
            //      http://127.0.0.1:3004/RNs/Test/ChargingStations/DE*GEF*S000001*1
            //
            // ------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.SET,
                                                 _URIPrefix + "/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStations/{StationId}/AdminStatus",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: async HTTPRequest => {

                                                     #region Parse Query/Request Parameters

                                                     if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI,
                                                                                          out RoamingNetwork  RoamingNetwork,
                                                                                          out HTTPResponse    _HTTPResponse))
                                                     {
                                                         return _HTTPResponse;
                                                     }

                                                     var EventTrackingId = EventTracking_Id.New;

                                                     ChargingStationAdminStatusTypes NewChargingStationAdminStatus;
                                                     ChargingStation_Id ChargingStationId;

                                                     try
                                                     {

                                                         #region Parse ChargingStationId

                                                         if (!ChargingStation_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ChargingStationId))
                                                             return new HTTPResponse.Builder(HTTPRequest) {
                                                                 HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                 ContentType     = HTTPContentType.JSON_UTF8,
                                                                 Content         = new JObject(new JProperty("Description", "Invalid ChargingStationId query parameter!")).ToUTF8Bytes()
                                                             };

                                                         if (!RoamingNetwork.ContainsChargingStation(ChargingStationId))
                                                             return new HTTPResponse.Builder(HTTPRequest) {
                                                                 HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                 ContentType     = HTTPContentType.JSON_UTF8,
                                                                 Content         = new JObject(new JProperty("Description", "Unknown ChargingStationId query parameter!")).ToUTF8Bytes()
                                                             };

                                                         #endregion

                                                         if (!HTTPRequest.TryParseJObjectRequestBody(out JObject JSON, out _HTTPResponse))
                                                             return _HTTPResponse;

                                                         #region Parse newstatus

                                                         if (!JSON.ParseMandatoryEnum("newstatus",
                                                                                      "charging station admin status",
                                                                                      WWCPAPI.HTTPServer.DefaultServerName,
                                                                                      out NewChargingStationAdminStatus,
                                                                                      HTTPRequest,
                                                                                      out _HTTPResponse))
                                                         {
                                                             return _HTTPResponse;
                                                         }

                                                         #endregion

                                                     }
                                                     catch (Exception e)
                                                     {

                                                         return new HTTPResponse.Builder(HTTPRequest) {
                                                                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                                                    ContentType     = HTTPContentType.JSON_UTF8,
                                                                    Content         = new JObject(new JProperty("Description", "An exception occured: " + e.Message)).ToUTF8Bytes()
                                                                };

                                                     }

                                                     #endregion

                                                     try
                                                     {

                                                         WWCPAPI.HTTPServer.Get(Semantics.DebugLog).
                                                             SubmitSubEvent("SetChargingStationAdminStatusRequest",
                                                                            new JObject(
                                                                                new JProperty("Timestamp",          DateTime.UtcNow.ToIso8601()),
                                                                                new JProperty("RoamingNetwork",     RoamingNetwork.ToString()),
                                                                                new JProperty("ChargingStationId",  ChargingStationId.ToString()),
                                                                                new JProperty("NewStatus",          NewChargingStationAdminStatus.ToString())
                                                                            ).ToString().
                                                                              Replace(Environment.NewLine, "")).Wait();


                                                         RoamingNetwork.SetChargingStationAdminStatus(ChargingStationId,
                                                                                                      new Timestamped<ChargingStationAdminStatusTypes>[1] {
                                                                                                          new Timestamped<ChargingStationAdminStatusTypes>(NewChargingStationAdminStatus)
                                                                                                      });

                                                         //GetEventSource(Semantics.DebugLog).
                                                         //        SubmitSubEvent("AUTHSTARTResponse",
                                                         //                       new JObject(
                                                         //                           new JProperty("Timestamp",         DateTime.UtcNow.ToIso8601()),
                                                         //                           new JProperty("RoamingNetwork",    RoamingNetwork.ToString()),
                                                         //                           new JProperty("SessionId",         AuthStartResult.SessionId.ToString()),
                                                         //                           new JProperty("PartnerSessionId",  PartnerSessionId.ToString()),
                                                         //                           new JProperty("ProviderId",        AuthStartResult.ProviderId.ToString()),
                                                         //                           new JProperty("AuthorizatorId",    AuthStartResult.AuthorizatorId.ToString()),
                                                         //                           new JProperty("Description",       "Authorized")
                                                         //                       ).ToString().
                                                         //                         Replace(Environment.NewLine, ""));


                                                         return new HTTPResponse.Builder(HTTPRequest) {
                                                             HTTPStatusCode  = HTTPStatusCode.OK,
                                                             Date            = DateTime.UtcNow,
                                                             ContentType     = HTTPContentType.JSON_UTF8,
                                                             Content         = new JObject(
                                                                                   new JProperty("Description",  "Ok")
                                                                               ).ToString().
                                                                                 Replace(Environment.NewLine, "").
                                                                                 ToUTF8Bytes(),
                                                             Connection      = "close"
                                                         };

                                                     }

                                                     #region Catch errors...

                                                     catch (Exception e)
                                                     {

                                                         //Log.Timestamp("Exception occured: " + e.Message);

                                                         return new HTTPResponse.Builder(HTTPRequest) {
                                                             HTTPStatusCode  = HTTPStatusCode.InternalServerError,
                                                             ContentType     = HTTPContentType.JSON_UTF8,
                                                             Content         = new JObject(new JProperty("@context",           "http://wwcp.graphdefined.org/contexts/SETSTATUS-Response.jsonld"),
                                                                                           new JProperty("RoamingNetwork",     RoamingNetwork.ToString()),
                                                                                           new JProperty("ChargingStationId",  ChargingStationId.ToString()),
                                                                                           new JProperty("Description",        "An exception occured: " + e.Message)).
                                                                                           ToString().ToUTF8Bytes()
                                                         };

                                                     }

                                                     #endregion

                                                 });

            #endregion

            #endregion



            #region ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStationGroups

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStationGroups

            // ----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingStationOperators/{CSOId}/ChargingStationGroups
            // ----------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStationGroups",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStationOperator(WWCPAPI,
                                                                                                                out RoamingNetwork           RoamingNetwork,
                                                                                                                out ChargingStationOperator  ChargingStationOperator,
                                                                                                                out HTTPResponse             _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                      = Request.QueryString.GetUInt64("skip");
                                                     var take                      = Request.QueryString.GetUInt64("take");
                                                     var expand                    = Request.QueryString.GetStrings("expand", true);
                                                     var expandChargingPoolIds     = expand.Contains("pools")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingStationIds  = expand.Contains("stations")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandEVSEIds             = expand.Contains("evses")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandDataLicenseIds      = expand.Contains("operators") ? InfoStatus.Expand : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount            = ChargingStationOperator.ChargingStationGroups.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = ChargingStationOperator.ChargingStationGroups.
                                                                                                  ToJSON(skip,
                                                                                                         take,
                                                                                                         false,
                                                                                                         expandChargingPoolIds,
                                                                                                         expandChargingStationIds,
                                                                                                         expandEVSEIds,
                                                                                                         expandDataLicenseIds).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = _ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStationGroups/{ChargingStationGroupId}

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStationGroups/{ChargingStationGroupId}

            // --------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingStationOperators/{CSOId}/ChargingStationGroups/{ChargingStationGroupId}
            // --------------------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/ChargingStationGroups/{ChargingStationGroupId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStationOperatorAndChargingStationGroup(WWCPAPI,
                                                                                                                                       out RoamingNetwork           RoamingNetwork,
                                                                                                                                       out ChargingStationOperator  ChargingStationOperator,
                                                                                                                                       out ChargingStationGroup     ChargingStationGroup,
                                                                                                                                       out HTTPResponse             _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                       = Request.QueryString.GetUInt64("skip");
                                                     var take                       = Request.QueryString.GetUInt64("take");
                                                     var include                    = Request.QueryString.GetStrings("include", true);
                                                     var expand                     = Request.QueryString.GetStrings("expand",  true);
                                                     var expandChargingPoolIds      = expand. Contains("pools")     ? InfoStatus.Expand : include.Contains("pools")     ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                                                     var expandChargingStationIds   = expand. Contains("stations")  ? InfoStatus.Expand : include.Contains("stations")  ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                                                     var expandEVSEIds              = expand. Contains("evses")     ? InfoStatus.Expand : include.Contains("evses")     ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                                                     var expandDataLicenseIds       = expand. Contains("operators") ? InfoStatus.Expand : include.Contains("operators") ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;

                                                     var ChargingStationGroupJSON   = ChargingStationGroup.ToJSON(false,
                                                                                                                  expandChargingPoolIds,
                                                                                                                  expandChargingStationIds,
                                                                                                                  expandEVSEIds,
                                                                                                                  expandDataLicenseIds);

                                                     #region Include charging pools

                                                     //if (expandChargingPoolIds == InfoStatus.ShowIdOnly)
                                                     //{

                                                     //    var pools  = ChargingStationOperator.
                                                     //                     ChargingPools.
                                                     //                     Where(pool => pool.ChargingStationGroup == ChargingStationGroup).
                                                     //                     ToArray();

                                                     //    if (pools.Length > 0)
                                                     //       ChargingStationGroupJSON["chargingPoolIds"] = new JArray(pools.Select(pool => pool.Id.ToString()));

                                                     //}

                                                     //else if (expandChargingPoolIds == InfoStatus.Expand) {

                                                     //    var pools  = ChargingStationOperator.
                                                     //                     ChargingPools.
                                                     //                     Where(pool => pool.ChargingStationGroup == ChargingStationGroup).
                                                     //                     ToArray();

                                                     //    if (pools.Length > 0)
                                                     //        ChargingStationGroupJSON["chargingPools"]  = new JArray(pools.Select(pool => pool.ToJSON(Embedded:                        true,
                                                     //                                                                                  ExpandRoamingNetworkId:          InfoStatus.Hidden,
                                                     //                                                                                  ExpandChargingStationOperatorId: InfoStatus.Hidden,
                                                     //                                                                                  ExpandChargingStationIds:        InfoStatus.Hidden,
                                                     //                                                                                  ExpandEVSEIds:                   InfoStatus.ShowIdOnly,
                                                     //                                                                                  ExpandChargingStationGroupIds:                  InfoStatus.Hidden)));

                                                     //}

                                                     //#endregion

                                                     //#region Include charging stations

                                                     //if (expandChargingStationIds == InfoStatus.ShowIdOnly)
                                                     //{

                                                     //    var stations  = ChargingStationOperator.
                                                     //                        ChargingStations.
                                                     //                        ToArray();

                                                     //    if (stations.Length > 0)
                                                     //       ChargingStationGroupJSON["chargingStationIds"] = new JArray(stations.Select(station => station.Id.ToString()));

                                                     //}

                                                     //else if (expandChargingStationIds == InfoStatus.Expand) {

                                                     //    var stations  = ChargingStationOperator.
                                                     //                        ChargingStations.
                                                     //                        ToArray();

                                                     //    if (stations.Length > 0)
                                                     //        ChargingStationGroupJSON["chargingStations"]  = new JArray(ChargingStationOperator.
                                                     //                                                         ChargingStations.
                                                     //                                                         Select(station => station.ToJSON(Embedded:                        true,
                                                     //                                                                                          ExpandRoamingNetworkId:          InfoStatus.Hidden,
                                                     //                                                                                          ExpandChargingStationOperatorId: InfoStatus.Hidden,
                                                     //                                                                                          ExpandChargingPoolId:            InfoStatus.Hidden,
                                                     //                                                                                          ExpandEVSEIds:                   InfoStatus.ShowIdOnly,
                                                     //                                                                                          ExpandChargingStationGroupIds:                  InfoStatus.Hidden)));

                                                     //}

                                                     //#endregion

                                                     //#region Include EVSEs

                                                     //else if (expandEVSEIds == InfoStatus.ShowIdOnly)
                                                     //{

                                                     //    var evses  = ChargingStationOperator.
                                                     //                     EVSEs.
                                                     //                     Where(evse => evse.ChargingStationGroup == ChargingStationGroup).
                                                     //                     ToArray();

                                                     //    if (evses.Length > 0)
                                                     //       ChargingStationGroupJSON["EVSEIds"] = new JArray(evses.Select(station => station.Id.ToString()));

                                                     //}

                                                     //else if (expandEVSEIds == InfoStatus.Expand) {

                                                     //    var evses  = ChargingStationOperator.
                                                     //                     EVSEs.
                                                     //                     Where(evse => evse.ChargingStationGroup == ChargingStationGroup).
                                                     //                     ToArray();

                                                     //    if (evses.Length > 0)
                                                     //        ChargingStationGroupJSON["EVSEs"]   = new JArray(ChargingStationOperator.
                                                     //                                              EVSEs.
                                                     //                                              Where (evse => evse.ChargingStationGroup == ChargingStationGroup).
                                                     //                                              Select(evse => evse.ToJSON(Embedded:                        true,
                                                     //                                                                         ExpandRoamingNetworkId:          InfoStatus.Hidden,
                                                     //                                                                         ExpandChargingStationOperatorId: InfoStatus.Hidden,
                                                     //                                                                         ExpandChargingPoolId:            InfoStatus.Hidden,
                                                     //                                                                         ExpandChargingStationId:         InfoStatus.Hidden,
                                                     //                                                                         ExpandChargingStationGroupIds:                  InfoStatus.Hidden)));

                                                     //}

                                                     #endregion


                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode              = HTTPStatusCode.OK,
                                                             Server                      = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                        = DateTime.UtcNow,
                                                             AccessControlAllowOrigin    = "*",
                                                             AccessControlAllowMethods   = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders   = "Content-Type, Accept, Authorization",
                                                             ETag                        = "1",
                                                             ContentType                 = HTTPContentType.JSON_UTF8,
                                                             Content                     = ChargingStationGroupJSON.ToUTF8Bytes(),
                                                             Connection                  = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion


            #region ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/EVSEGroups

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/EVSEGroups

            // ----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingStationOperators/{CSOId}/EVSEGroups
            // ----------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/EVSEGroups",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStationOperator(WWCPAPI,
                                                                                                                out RoamingNetwork           RoamingNetwork,
                                                                                                                out ChargingStationOperator  ChargingStationOperator,
                                                                                                                out HTTPResponse             _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                      = Request.QueryString.GetUInt64("skip");
                                                     var take                      = Request.QueryString.GetUInt64("take");
                                                     var expand                    = Request.QueryString.GetStrings("expand", true);
                                                     var expandChargingPoolIds     = expand.Contains("pools")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingStationIds  = expand.Contains("stations")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandEVSEIds             = expand.Contains("evses")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandDataLicenseIds      = expand.Contains("operators") ? InfoStatus.Expand : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount            = ChargingStationOperator.EVSEGroups.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = ChargingStationOperator.EVSEGroups.
                                                                                                  ToJSON(skip,
                                                                                                         take,
                                                                                                         false,
                                                                                                         expandChargingPoolIds,
                                                                                                         expandEVSEIds,
                                                                                                         expandEVSEIds,
                                                                                                         expandDataLicenseIds).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = _ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/EVSEGroups/{EVSEGroupId}

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/EVSEGroups/{EVSEGroupId}

            // --------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingStationOperators/{CSOId}/EVSEGroups/{EVSEGroupId}
            // --------------------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/EVSEGroups/{EVSEGroupId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStationOperatorAndEVSEGroup(WWCPAPI,
                                                                                                                            out RoamingNetwork           RoamingNetwork,
                                                                                                                            out ChargingStationOperator  ChargingStationOperator,
                                                                                                                            out EVSEGroup                EVSEGroup,
                                                                                                                            out HTTPResponse             _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                      = Request.QueryString.GetUInt64("skip");
                                                     var take                      = Request.QueryString.GetUInt64("take");
                                                     var include                   = Request.QueryString.GetStrings("include", true);
                                                     var expand                    = Request.QueryString.GetStrings("expand",  true);
                                                     var expandChargingPoolIds     = expand. Contains("pools")     ? InfoStatus.Expand : include.Contains("pools")     ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                                                     var expandChargingStationIds  = expand. Contains("stations")  ? InfoStatus.Expand : include.Contains("stations")  ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                                                     var expandEVSEIds             = expand. Contains("evses")     ? InfoStatus.Expand : include.Contains("evses")     ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                                                     var expandDataLicenseIds      = expand. Contains("operators") ? InfoStatus.Expand : include.Contains("operators") ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;

                                                     var EVSEGroupJSON             = EVSEGroup.ToJSON(false,
                                                                                                      expandChargingPoolIds,
                                                                                                      expandEVSEIds,
                                                                                                      expandEVSEIds,
                                                                                                      expandDataLicenseIds);

                                                     #region Include charging pools

                                                     //if (expandChargingPoolIds == InfoStatus.ShowIdOnly)
                                                     //{

                                                     //    var pools  = EVSEOperator.
                                                     //                     ChargingPools.
                                                     //                     Where(pool => pool.EVSEGroup == EVSEGroup).
                                                     //                     ToArray();

                                                     //    if (pools.Length > 0)
                                                     //       EVSEGroupJSON["chargingPoolIds"] = new JArray(pools.Select(pool => pool.Id.ToString()));

                                                     //}

                                                     //else if (expandChargingPoolIds == InfoStatus.Expand) {

                                                     //    var pools  = EVSEOperator.
                                                     //                     ChargingPools.
                                                     //                     Where(pool => pool.EVSEGroup == EVSEGroup).
                                                     //                     ToArray();

                                                     //    if (pools.Length > 0)
                                                     //        EVSEGroupJSON["chargingPools"]  = new JArray(pools.Select(pool => pool.ToJSON(Embedded:                        true,
                                                     //                                                                                  ExpandRoamingNetworkId:          InfoStatus.Hidden,
                                                     //                                                                                  ExpandEVSEOperatorId: InfoStatus.Hidden,
                                                     //                                                                                  ExpandEVSEIds:        InfoStatus.Hidden,
                                                     //                                                                                  ExpandEVSEIds:                   InfoStatus.ShowIdOnly,
                                                     //                                                                                  ExpandEVSEGroupIds:                  InfoStatus.Hidden)));

                                                     //}

                                                     //#endregion

                                                     //#region Include charging stations

                                                     //if (expandEVSEIds == InfoStatus.ShowIdOnly)
                                                     //{

                                                     //    var stations  = EVSEOperator.
                                                     //                        EVSEs.
                                                     //                        ToArray();

                                                     //    if (stations.Length > 0)
                                                     //       EVSEGroupJSON["chargingStationIds"] = new JArray(stations.Select(station => station.Id.ToString()));

                                                     //}

                                                     //else if (expandEVSEIds == InfoStatus.Expand) {

                                                     //    var stations  = EVSEOperator.
                                                     //                        EVSEs.
                                                     //                        ToArray();

                                                     //    if (stations.Length > 0)
                                                     //        EVSEGroupJSON["chargingStations"]  = new JArray(EVSEOperator.
                                                     //                                                         EVSEs.
                                                     //                                                         Select(station => station.ToJSON(Embedded:                        true,
                                                     //                                                                                          ExpandRoamingNetworkId:          InfoStatus.Hidden,
                                                     //                                                                                          ExpandEVSEOperatorId: InfoStatus.Hidden,
                                                     //                                                                                          ExpandChargingPoolId:            InfoStatus.Hidden,
                                                     //                                                                                          ExpandEVSEIds:                   InfoStatus.ShowIdOnly,
                                                     //                                                                                          ExpandEVSEGroupIds:                  InfoStatus.Hidden)));

                                                     //}

                                                     //#endregion

                                                     //#region Include EVSEs

                                                     //else if (expandEVSEIds == InfoStatus.ShowIdOnly)
                                                     //{

                                                     //    var evses  = EVSEOperator.
                                                     //                     EVSEs.
                                                     //                     Where(evse => evse.EVSEGroup == EVSEGroup).
                                                     //                     ToArray();

                                                     //    if (evses.Length > 0)
                                                     //       EVSEGroupJSON["EVSEIds"] = new JArray(evses.Select(station => station.Id.ToString()));

                                                     //}

                                                     //else if (expandEVSEIds == InfoStatus.Expand) {

                                                     //    var evses  = EVSEOperator.
                                                     //                     EVSEs.
                                                     //                     Where(evse => evse.EVSEGroup == EVSEGroup).
                                                     //                     ToArray();

                                                     //    if (evses.Length > 0)
                                                     //        EVSEGroupJSON["EVSEs"]   = new JArray(EVSEOperator.
                                                     //                                              EVSEs.
                                                     //                                              Where (evse => evse.EVSEGroup == EVSEGroup).
                                                     //                                              Select(evse => evse.ToJSON(Embedded:                        true,
                                                     //                                                                         ExpandRoamingNetworkId:          InfoStatus.Hidden,
                                                     //                                                                         ExpandEVSEOperatorId: InfoStatus.Hidden,
                                                     //                                                                         ExpandChargingPoolId:            InfoStatus.Hidden,
                                                     //                                                                         ExpandEVSEId:         InfoStatus.Hidden,
                                                     //                                                                         ExpandEVSEGroupIds:                  InfoStatus.Hidden)));

                                                     //}

                                                     #endregion


                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode              = HTTPStatusCode.OK,
                                                             Server                      = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                        = DateTime.UtcNow,
                                                             AccessControlAllowOrigin    = "*",
                                                             AccessControlAllowMethods   = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders   = "Content-Type, Accept, Authorization",
                                                             ETag                        = "1",
                                                             ContentType                 = HTTPContentType.JSON_UTF8,
                                                             Content                     = EVSEGroupJSON.ToUTF8Bytes(),
                                                             Connection                  = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion


            #region ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/Brands

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/Brands

            // ----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingStationOperators/{CSOId}/Brands
            // ----------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/Brands",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStationOperator(WWCPAPI,
                                                                                                                out RoamingNetwork           RoamingNetwork,
                                                                                                                out ChargingStationOperator  ChargingStationOperator,
                                                                                                                out HTTPResponse             _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                      = Request.QueryString.GetUInt64("skip");
                                                     var take                      = Request.QueryString.GetUInt64("take");
                                                     var expand                    = Request.QueryString.GetStrings("expand", true);
                                                     var expandChargingPoolIds     = expand.Contains("pools")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingStationIds  = expand.Contains("stations")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandEVSEIds             = expand.Contains("evses")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandDataLicenseIds      = expand.Contains("operators") ? InfoStatus.Expand : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount            = ChargingStationOperator.Brands.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = ChargingStationOperator.Brands.
                                                                                                  ToJSON(skip,
                                                                                                         take,
                                                                                                         false,
                                                                                                         expandChargingPoolIds,
                                                                                                         expandChargingStationIds,
                                                                                                         expandEVSEIds,
                                                                                                         expandDataLicenseIds).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = _ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion

            #region ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/Brands/{BrandId}

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/Brands/{BrandId}

            // --------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingStationOperators/{CSOId}/Brands/{BrandId}
            // --------------------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/Brands/{BrandId}",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStationOperatorAndBrand(WWCPAPI,
                                                                                                                        out RoamingNetwork           RoamingNetwork,
                                                                                                                        out ChargingStationOperator  ChargingStationOperator,
                                                                                                                        out Brand                    Brand,
                                                                                                                        out HTTPResponse             _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                       = Request.QueryString.GetUInt64("skip");
                                                     var take                       = Request.QueryString.GetUInt64("take");
                                                     var include                    = Request.QueryString.GetStrings("include", true);
                                                     var expand                     = Request.QueryString.GetStrings("expand",  true);
                                                     var expandChargingPoolIds      = expand. Contains("pools")     ? InfoStatus.Expand : include.Contains("pools")     ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                                                     var expandChargingStationIds   = expand. Contains("stations")  ? InfoStatus.Expand : include.Contains("stations")  ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                                                     var expandEVSEIds              = expand. Contains("evses")     ? InfoStatus.Expand : include.Contains("evses")     ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                                                     var expandDataLicenseIds       = expand. Contains("operators") ? InfoStatus.Expand : include.Contains("operators") ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;

                                                     var BrandJSON                  = Brand.ToJSON(false,
                                                                                                   expandChargingPoolIds,
                                                                                                   expandChargingStationIds,
                                                                                                   expandEVSEIds,
                                                                                                   expandDataLicenseIds);

                                                     #region Include charging pools

                                                     if (expandChargingPoolIds == InfoStatus.ShowIdOnly)
                                                     {

                                                         var pools  = ChargingStationOperator.
                                                                          ChargingPools.
                                                                          Where(pool => pool.Brand == Brand).
                                                                          ToArray();

                                                         if (pools.Length > 0)
                                                            BrandJSON["chargingPoolIds"] = new JArray(pools.Select(pool => pool.Id.ToString()));

                                                     }

                                                     else if (expandChargingPoolIds == InfoStatus.Expand) {

                                                         var pools  = ChargingStationOperator.
                                                                          ChargingPools.
                                                                          Where(pool => pool.Brand == Brand).
                                                                          ToArray();

                                                         if (pools.Length > 0)
                                                             BrandJSON["chargingPools"]  = new JArray(pools.Select(pool => pool.ToJSON(Embedded:                        true,
                                                                                                                                       ExpandRoamingNetworkId:          InfoStatus.Hidden,
                                                                                                                                       ExpandChargingStationOperatorId: InfoStatus.Hidden,
                                                                                                                                       ExpandChargingStationIds:        InfoStatus.Hidden,
                                                                                                                                       ExpandEVSEIds:                   InfoStatus.ShowIdOnly,
                                                                                                                                       ExpandBrandIds:                  InfoStatus.Hidden)));

                                                     }

                                                     #endregion

                                                     #region Include charging stations

                                                     if (expandChargingStationIds == InfoStatus.ShowIdOnly)
                                                     {

                                                         var stations  = ChargingStationOperator.
                                                                             ChargingStations.
                                                                             Where(station => station.Brand == Brand).
                                                                             ToArray();

                                                         if (stations.Length > 0)
                                                            BrandJSON["chargingStationIds"] = new JArray(stations.Select(station => station.Id.ToString()));

                                                     }

                                                     else if (expandChargingStationIds == InfoStatus.Expand) {

                                                         var stations  = ChargingStationOperator.
                                                                             ChargingStations.
                                                                             Where(station => station.Brand == Brand).
                                                                             ToArray();

                                                         if (stations.Length > 0)
                                                             BrandJSON["chargingStations"]  = new JArray(ChargingStationOperator.
                                                                                                              ChargingStations.
                                                                                                              Where (station => station.Brand == Brand).
                                                                                                              Select(station => station.ToJSON(Embedded:                        true,
                                                                                                                                               ExpandRoamingNetworkId:          InfoStatus.Hidden,
                                                                                                                                               ExpandChargingStationOperatorId: InfoStatus.Hidden,
                                                                                                                                               ExpandChargingPoolId:            InfoStatus.Hidden,
                                                                                                                                               ExpandEVSEIds:                   InfoStatus.ShowIdOnly,
                                                                                                                                               ExpandBrandIds:                  InfoStatus.Hidden)));

                                                     }

                                                     #endregion

                                                     #region Include EVSEs

                                                     else if (expandEVSEIds == InfoStatus.ShowIdOnly)
                                                     {

                                                         var evses  = ChargingStationOperator.
                                                                          EVSEs.
                                                                          Where(evse => evse.Brand == Brand).
                                                                          ToArray();

                                                         if (evses.Length > 0)
                                                            BrandJSON["EVSEIds"] = new JArray(evses.Select(station => station.Id.ToString()));

                                                     }

                                                     else if (expandEVSEIds == InfoStatus.Expand) {

                                                         var evses  = ChargingStationOperator.
                                                                          EVSEs.
                                                                          Where(evse => evse.Brand == Brand).
                                                                          ToArray();

                                                         if (evses.Length > 0)
                                                             BrandJSON["EVSEs"]   = new JArray(ChargingStationOperator.
                                                                                                   EVSEs.
                                                                                                   Where (evse => evse.Brand == Brand).
                                                                                                   Select(evse => evse.ToJSON(Embedded:                        true,
                                                                                                                              ExpandRoamingNetworkId:          InfoStatus.Hidden,
                                                                                                                              ExpandChargingStationOperatorId: InfoStatus.Hidden,
                                                                                                                              ExpandChargingPoolId:            InfoStatus.Hidden,
                                                                                                                              ExpandChargingStationId:         InfoStatus.Hidden,
                                                                                                                              ExpandBrandIds:                  InfoStatus.Hidden)));

                                                     }

                                                     #endregion


                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode              = HTTPStatusCode.OK,
                                                             Server                      = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                        = DateTime.UtcNow,
                                                             AccessControlAllowOrigin    = "*",
                                                             AccessControlAllowMethods   = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders   = "Content-Type, Accept, Authorization",
                                                             ETag                        = "1",
                                                             ContentType                 = HTTPContentType.JSON_UTF8,
                                                             Content                     = BrandJSON.ToUTF8Bytes(),
                                                             Connection                  = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #endregion



            #region ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/Tariffs

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/Tariffs

            // ----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingStationOperators/{CSOId}/Tariffs
            // ----------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/Tariffs",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStationOperator(WWCPAPI,
                                                                                                                out RoamingNetwork           RoamingNetwork,
                                                                                                                out ChargingStationOperator  ChargingStationOperator,
                                                                                                                out HTTPResponse             _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                      = Request.QueryString.GetUInt64("skip");
                                                     var take                      = Request.QueryString.GetUInt64("take");
                                                     var expand                    = Request.QueryString.GetStrings("expand", true);
                                                     var expandChargingPoolIds     = expand.Contains("pools")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingStationIds  = expand.Contains("stations")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandEVSEIds             = expand.Contains("evses")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandDataLicenseIds      = expand.Contains("operators") ? InfoStatus.Expand : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount            = ChargingStationOperator.ChargingTariffs.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.JSON_UTF8,
                                                             Content                        = ChargingStationOperator.ChargingTariffs.
                                                                                                  ToJSON(skip,
                                                                                                         take,
                                                                                                         false,
                                                                                                         expandChargingPoolIds,
                                                                                                         expandChargingStationIds,
                                                                                                         expandEVSEIds,
                                                                                                         expandDataLicenseIds).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = _ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion

            #region GET         ~/RNs/{RNId}/ChargingStationOperators/{CSOId}/TariffOverview

            // ----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingStationOperators/{CSOId}/TariffOverview
            // ----------------------------------------------------------------------------------------------------------------------------------
            WWCPAPI.HTTPServer.AddMethodCallback(_Hostname,
                                                 HTTPMethod.GET,
                                                 _URIPrefix + "RNs/{RNId}/ChargingStationOperators/{CSOId}/TariffOverview",
                                                 HTTPContentType.JSON_UTF8,
                                                 HTTPDelegate: Request => {

                                                     #region Check parameters

                                                     if (!Request.ParseRoamingNetworkAndChargingStationOperator(WWCPAPI,
                                                                                                                out RoamingNetwork           RoamingNetwork,
                                                                                                                out ChargingStationOperator  ChargingStationOperator,
                                                                                                                out HTTPResponse             _HTTPResponse))
                                                     {
                                                         return Task.FromResult(_HTTPResponse);
                                                     }

                                                     #endregion

                                                     var skip                      = Request.QueryString.GetUInt64("skip");
                                                     var take                      = Request.QueryString.GetUInt64("take");
                                                     var expand                    = Request.QueryString.GetStrings("expand", true);
                                                     var expandChargingPoolIds     = expand.Contains("pools")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandChargingStationIds  = expand.Contains("stations")  ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandEVSEIds             = expand.Contains("evses")     ? InfoStatus.Expand : InfoStatus.ShowIdOnly;
                                                     var expandDataLicenseIds      = expand.Contains("operators") ? InfoStatus.Expand : InfoStatus.ShowIdOnly;

                                                     //ToDo: Getting the expected total count might be very expensive!
                                                     var _ExpectedCount            = ChargingStationOperator.ChargingTariffs.ULongCount();

                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode                 = HTTPStatusCode.OK,
                                                             Server                         = WWCPAPI.HTTPServer.DefaultServerName,
                                                             Date                           = DateTime.UtcNow,
                                                             AccessControlAllowOrigin       = "*",
                                                             AccessControlAllowMethods      = "GET, COUNT, STATUS",
                                                             AccessControlAllowHeaders      = "Content-Type, Accept, Authorization",
                                                             ETag                           = "1",
                                                             ContentType                    = HTTPContentType.TEXT_UTF8,
                                                             Content                        = ChargingStationOperator.ChargingStations.
                                                                                                  GetTariffs(skip,
                                                                                                             take,
                                                                                                             false,
                                                                                                             expandChargingPoolIds,
                                                                                                             expandChargingStationIds,
                                                                                                             expandEVSEIds,
                                                                                                             expandDataLicenseIds).
                                                                                                  Select(line => "\"" + line.AggregateWith("\";\"") + "\"").
                                                                                                  AggregateWith(Environment.NewLine).
                                                                                                  ToUTF8Bytes(),
                                                             X_ExpectedTotalNumberOfItems   = _ExpectedCount,
                                                             Connection                     = "close"
                                                         }.AsImmutable);

                                                 });

            #endregion


            #endregion

        }

    }

}
