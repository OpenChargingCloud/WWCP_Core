/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using System.Text;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Logging;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Hermod.SMTP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.HTTPTest;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public delegate String    WWCPLogfileCreatorDelegate               (String         LoggingPath,
                                                                    //    IRemoteParty?  RemoteParty,
                                                                        String         Context,
                                                                        String         LogfileName);


/// <summary>
    /// Extension methods for the Open Charging Cloud HTTP API.
    /// </summary>
    public static class WWCP_HTTPAPIExtensions
    {

        // Used by multiple HTTP content types

        public const String RoamingNetworkId  = "RoamingNetworkId";
        public const String EVSEId            = "EVSEId";

        #region ParseRoamingNetwork                           (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork,                              out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network
        /// for the given HTTP hostname and HTTP query parameter
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean ParseRoamingNetwork(this HTTPRequest                                HTTPRequest,
                                                  WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                  [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                  [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            RoamingNetwork       = null;
            HTTPResponseBuilder  = null;

            if (!HTTPRequest.TryParseURLParameter<RoamingNetwork_Id>(
                     WWCP_HTTPAPIExtensions.RoamingNetworkId,
                     RoamingNetwork_Id.TryParse,
                     out var roamingNetworkId
               ))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = @"{ ""description"": ""Invalid roaming network identification!"" }".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            if (!WWCP_HTTPAPI.WWCPCore.TryGetRoamingNetwork(//HTTPRequest.Host,
                                                            roamingNetworkId,
                                                            out RoamingNetwork))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = @"{ ""description"": ""Unknown roaming network!"" }".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingStationOperator (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingStationOperator, out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging station operator
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndChargingStationOperator(this HTTPRequest                                    HTTPRequest,
                                                                            WWCP_HTTPAPI                                        WWCP_HTTPAPI,
                                                                            [NotNullWhen(true)]  out IRoamingNetwork?           RoamingNetwork,
                                                                            [NotNullWhen(true)]  out IChargingStationOperator?  ChargingStationOperator,
                                                                            [NotNullWhen(false)] out HTTPResponse.Builder?      HTTPResponseBuilder)
        {

            RoamingNetwork           = null;
            ChargingStationOperator  = null;
            HTTPResponseBuilder      = null;

            if (HTTPRequest.ParsedURLParameters.Length < 2)
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponseBuilder))
                return false;


            if (!ChargingStationOperator_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var chargingStationOperatorId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationOperatorById(chargingStationOperatorId, out ChargingStationOperator))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingPool            (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingPool,            out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging pool
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndChargingPool(this HTTPRequest                                HTTPRequest,
                                                                 WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                 [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                 [NotNullWhen(true)]  out IChargingPool?         ChargingPool,
                                                                 [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            RoamingNetwork       = null;
            ChargingPool         = null;
            HTTPResponseBuilder  = null;

            if (HTTPRequest.ParsedURLParameters.Length < 2) {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponseBuilder))
                return false;


            if (!ChargingPool_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var chargingPoolId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingPoolById(chargingPoolId, out ChargingPool))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingStation         (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingStation,         out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging station
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStation">The charging station.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndChargingStation(this HTTPRequest                                HTTPRequest,
                                                                    WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                    [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                    [NotNullWhen(true)]  out IChargingStation?      ChargingStation,
                                                                    [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            RoamingNetwork       = null;
            ChargingStation      = null;
            HTTPResponseBuilder  = null;

            if (HTTPRequest.ParsedURLParameters.Length < 2)
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURLParameters[0], out var roamingNetworkId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid roaming network identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            RoamingNetwork  = WWCP_HTTPAPI.WWCPCore.
                                  //GetAllRoamingNetworks(HTTPRequest.Host).
                                  FirstOrDefault(roamingNetwork => roamingNetwork.Id == roamingNetworkId);

            if (RoamingNetwork is null)
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown roaming network identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            if (!ChargingStation_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var chargingStationId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationById(chargingStationId, out ChargingStation))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndEVSE                    (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out EVSE,                    out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and EVSE
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="EVSE">The EVSE.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndEVSE(this HTTPRequest                                HTTPRequest,
                                                         WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                         [NotNullWhen(true)]  out RoamingNetwork_Id?     RoamingNetworkId,
                                                         [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                         [NotNullWhen(true)]  out EVSE_Id?               EVSEId,
                                                         [NotNullWhen(true)]  out IEVSE?                 EVSE,
                                                         [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            RoamingNetworkId     = null;
            EVSEId               = null;
            RoamingNetwork       = null;
            EVSE                 = null;
            HTTPResponseBuilder  = null;

            if (!HTTPRequest.TryParseURLParameter<RoamingNetwork_Id>(
                     WWCP_HTTPAPIExtensions.RoamingNetworkId,
                     RoamingNetwork_Id.TryParse,
                     out var roamingNetworkId
               ))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = JSONObject.Create(
                                                                new JProperty("authorizatorId",  WWCP_HTTPAPI.HTTPServiceName),
                                                                new JProperty("description",    $"Invalid roaming network identification '{HTTPRequest.ParsedURLParametersX[WWCP_HTTPAPIExtensions.RoamingNetworkId]}'!"),
                                                                new JProperty("runtime",         0)
                                                            ).ToUTF8Bytes()
                                      };

                return false;

            }

            RoamingNetworkId = roamingNetworkId;

            if (!WWCP_HTTPAPI.WWCPCore.TryGetRoamingNetwork(//HTTPHostname.Any,
                                                            roamingNetworkId, out var roamingNetwork))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = JSONObject.Create(
                                                                new JProperty("authorizatorId",  WWCP_HTTPAPI.HTTPServiceName),
                                                                new JProperty("description",    $"Unknown roaming network identification '{roamingNetworkId}'!"),
                                                                new JProperty("runtime",         0)
                                                            ).ToUTF8Bytes()
                                      };

                return false;

            }

            RoamingNetwork = roamingNetwork;

            EVSEId = EVSE_Id.TryParse(HTTPRequest.ParsedURLParametersX[WWCP_HTTPAPIExtensions.EVSEId],
                                      EVSEIdParsingMode.relaxed);

            if (!EVSEId.HasValue)
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = JSONObject.Create(
                                                                new JProperty("authorizatorId",  WWCP_HTTPAPI.HTTPServiceName),
                                                                new JProperty("description",    $"Invalid EVSE identification '{HTTPRequest.ParsedURLParametersX[WWCP_HTTPAPIExtensions.EVSEId]}'!"),
                                                                new JProperty("runtime",         0)
                                                            ).ToUTF8Bytes()
                                      };

                return false;

            }

            if (!RoamingNetwork.TryGetEVSEById(EVSEId.Value, out EVSE))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = JSONObject.Create(
                                                                new JProperty("authorizatorId",  WWCP_HTTPAPI.HTTPServiceName),
                                                                new JProperty("description",    $"Unknown EVSE identification '{EVSEId.Value}'!"),
                                                                new JProperty("runtime", 0)
                                                            ).ToUTF8Bytes()
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingSessionId       (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingSession,         out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging session
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetworkId">The roaming network identification.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndChargingSessionId(this HTTPRequest                                HTTPRequest,
                                                                      WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                      [NotNullWhen(true)]  out RoamingNetwork_Id?     RoamingNetworkId,
                                                                      [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                      [NotNullWhen(true)]  out ChargingSession_Id?    ChargingSessionId,
                                                                      [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            RoamingNetworkId     = null;
            RoamingNetwork       = null;
            ChargingSessionId    = null;
            HTTPResponseBuilder  = null;

            if (!HTTPRequest.TryParseURLParameter<RoamingNetwork_Id>("RoamingNetworkId", RoamingNetwork_Id.TryParse, out var roamingNetworkId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = @"{ ""description"": ""Invalid roaming network identification!"" }".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            RoamingNetworkId = roamingNetworkId;

            if (!WWCP_HTTPAPI.WWCPCore.TryGetRoamingNetwork(//HTTPRequest.Host,
                                                            roamingNetworkId, out RoamingNetwork))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = @"{ ""description"": ""Unknown roaming network identification!"" }".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            if (!HTTPRequest.TryParseURLParameter<ChargingSession_Id>("ChargingSessionId", ChargingSession_Id.TryParse, out var chargingSessionId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = @"{ ""description"": ""Invalid charging session identification!"" }".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            ChargingSessionId = chargingSessionId;
            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingSession         (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingSession,         out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging session
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetworkId">The roaming network identification.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndChargingSession(this HTTPRequest                                HTTPRequest,
                                                                    WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                    [NotNullWhen(true)]  out RoamingNetwork_Id?     RoamingNetworkId,
                                                                    [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                    [NotNullWhen(true)]  out ChargingSession_Id?    ChargingSessionId,
                                                                    [NotNullWhen(true)]  out ChargingSession?       ChargingSession,
                                                                    [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            RoamingNetworkId     = null;
            RoamingNetwork       = null;
            ChargingSessionId    = null;
            ChargingSession      = null;
            HTTPResponseBuilder  = null;

            if (!HTTPRequest.TryParseURLParameter<RoamingNetwork_Id>("RoamingNetworkId", RoamingNetwork_Id.TryParse, out var roamingNetworkId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = @"{ ""description"": ""Invalid roaming network identification!"" }".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            RoamingNetworkId = roamingNetworkId;

            if (!WWCP_HTTPAPI.WWCPCore.TryGetRoamingNetwork(//HTTPRequest.Host,
                                                            roamingNetworkId, out RoamingNetwork))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = @"{ ""description"": ""Unknown roaming network identification!"" }".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            if (!HTTPRequest.TryParseURLParameter<ChargingSession_Id>("ChargingSessionId", ChargingSession_Id.TryParse, out var chargingSessionId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = @"{ ""description"": ""Invalid charging session identification!"" }".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            ChargingSessionId = chargingSessionId;

            if (!RoamingNetwork.TryGetChargingSessionById(chargingSessionId, out ChargingSession))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = @"{ ""description"": ""Unknown charging session identification!"" }".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingSession         (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out EMobilityProviderId, out ChargingSession,         out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging session
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetworkId">The roaming network identification.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndChargingSession(this HTTPRequest                                HTTPRequest,
                                                                    WWCP_HTTPAPI                                            WWCP_HTTPAPI,
                                                                    [NotNullWhen(true)]  out RoamingNetwork_Id?     RoamingNetworkId,
                                                                    [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                    [NotNullWhen(true)]  out EMobilityProvider_Id?  EMobilityProviderId,
                                                                    [NotNullWhen(true)]  out ChargingSession_Id?    ChargingSessionId,
                                                                    [NotNullWhen(true)]  out ChargingSession?       ChargingSession,
                                                                    [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            RoamingNetworkId     = null;
            RoamingNetwork       = null;
            EMobilityProviderId  = null;
            ChargingSessionId    = null;
            ChargingSession      = null;
            HTTPResponseBuilder  = null;

            if (HTTPRequest.ParsedURLParameters.Length < 3) {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            RoamingNetworkId = RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURLParameters[0]);

            if (!RoamingNetworkId.HasValue)
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid roaming network identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            if (!WWCP_HTTPAPI.WWCPCore.TryGetRoamingNetwork(RoamingNetworkId.Value, out RoamingNetwork))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown roaming network identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }



            EMobilityProviderId = EMobilityProvider_Id.TryParse(HTTPRequest.ParsedURLParameters[1]);

            if (!EMobilityProviderId.HasValue)
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid e-mobility provider identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }




            ChargingSessionId = ChargingSession_Id.TryParse(HTTPRequest.ParsedURLParameters[2]);

            if (!ChargingSessionId.HasValue)
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid charging session identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingSessionById(ChargingSessionId.Value, out ChargingSession))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown charging session identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion


        #region ParseRoamingNetworkAndReservation             (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out Reservation,             out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging reservation
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Reservation">The charging reservation.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndReservation(this HTTPRequest                                HTTPRequest,
                                                                WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                [NotNullWhen(true)]  out ChargingReservation?   Reservation,
                                                                [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            RoamingNetwork       = null;
            Reservation          = null;
            HTTPResponseBuilder  = null;

            if (HTTPRequest.ParsedURLParameters.Length < 2)
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                };

                return false;

            }

            if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURLParameters[0], out var roamingNetworkId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid roaming network identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            RoamingNetwork = WWCP_HTTPAPI.WWCPCore.
                                 //GetAllRoamingNetworks(HTTPRequest.Host).
                                 FirstOrDefault(roamingNetwork => roamingNetwork.Id == roamingNetworkId);

            if (RoamingNetwork is null)
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown roaming network identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            if (!ChargingReservation_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var chargingReservationId)) {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid reservation identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.ReservationsStore.TryGetLatest(chargingReservationId, out Reservation))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown reservation identification!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndEMobilityProvider       (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out EMobilityProvider,       out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and e-mobility provider
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="EMobilityProvider">The charging station operator.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndEMobilityProvider(this HTTPRequest                                HTTPRequest,
                                                                      WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                      [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                      [NotNullWhen(true)]  out IEMobilityProvider?    EMobilityProvider,
                                                                      [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            RoamingNetwork       = null;
            EMobilityProvider    = null;
            HTTPResponseBuilder  = null;

            if (HTTPRequest.ParsedURLParameters.Length < 2)
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponseBuilder))
                return false;


            if (!EMobilityProvider_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var eMobilityProviderId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid EMobilityProviderId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetEMobilityProviderById(eMobilityProviderId, out EMobilityProvider))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown EMobilityProviderId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndGridOperator(this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out GridOperator, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and smart city
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="GridOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndGridOperator(this HTTPRequest                                HTTPRequest,
                                                                 WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                 [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                 [NotNullWhen(true)]  out IGridOperator?         GridOperator,
                                                                 [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponse)
        {

            RoamingNetwork  = null;
            GridOperator    = null;
            HTTPResponse    = null;

            if (HTTPRequest.ParsedURLParameters.Length < 2)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!GridOperator_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var gridOperatorId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid GridOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetGridOperatorById(gridOperatorId, out GridOperator))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown GridOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion




        #region ParseRoamingNetworkAndParkingOperator(this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ParkingOperator, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and parking operator
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ParkingOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndParkingOperator(this HTTPRequest           HTTPRequest,
                                                                    WWCP_HTTPAPI               WWCP_HTTPAPI,
                                                                    out IRoamingNetwork?       RoamingNetwork,
                                                                    out ParkingOperator?       ParkingOperator,
                                                                    out HTTPResponse.Builder?  HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest is null)
                throw new ArgumentNullException(nameof(HTTPRequest),           "The given HTTP request must not be null!");

            if (WWCP_HTTPAPI is null)
                throw new ArgumentNullException(nameof(WWCP_HTTPAPI),  "The given OpenChargingCloud API must not be null!");

            #endregion

            RoamingNetwork   = null;
            ParkingOperator  = null;
            HTTPResponse     = null;

            if (HTTPRequest.ParsedURLParameters.Length < 2)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI,
                                                 out RoamingNetwork,
                                                 out HTTPResponse))
            {
                return false;
            }


            if (!ParkingOperator_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var parkingOperatorId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ParkingOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetParkingOperatorById(parkingOperatorId, out ParkingOperator))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ParkingOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndSmartCity(this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out SmartCity, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and smart city
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="SmartCity">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndSmartCity(this HTTPRequest           HTTPRequest,
                                                              WWCP_HTTPAPI               WWCP_HTTPAPI,
                                                              out IRoamingNetwork?       RoamingNetwork,
                                                              out SmartCityProxy?        SmartCity,
                                                              out HTTPResponse.Builder?  HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest is null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCP_HTTPAPI is null)
                throw new ArgumentNullException(nameof(WWCP_HTTPAPI),      "The given OpenChargingCloud API must not be null!");

            #endregion

            RoamingNetwork  = null;
            SmartCity       = null;
            HTTPResponse    = null;

            if (HTTPRequest.ParsedURLParameters.Length < 2)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!SmartCity_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out SmartCity_Id SmartCityId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid SmartCityId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetSmartCityById(SmartCityId, out SmartCity))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown SmartCityId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion




        #region ParseRoamingNetworkAndChargingPoolAndChargingStation(this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingPool, out ChargingStation, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network, charging pool
        /// and charging station for the given HTTP hostname and HTTP query
        /// parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="ChargingStation">The charging station.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndChargingPoolAndChargingStation(this HTTPRequest                                HTTPRequest,
                                                                                   WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                                   [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                                   [NotNullWhen(true)]  out IChargingPool?         ChargingPool,
                                                                                   [NotNullWhen(true)]  out IChargingStation?      ChargingStation,
                                                                                   [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponse)
        {

            RoamingNetwork   = null;
            ChargingPool     = null;
            ChargingStation  = null;

            if (HTTPRequest.ParsedURLParameters.Length < 3)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponse))
                return false;

            #region Get charging pool...

            if (!ChargingPool_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var chargingPoolId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingPoolById(chargingPoolId, out ChargingPool))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            #endregion

            #region Get charging station...

            if (!ChargingStation_Id.TryParse(HTTPRequest.ParsedURLParameters[2], out var chargingStationId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationById(chargingStationId, out ChargingStation))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            #endregion

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingPoolAndChargingStationAndEVSE(this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingPool, out ChargingStation, out EVSE, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network, charging pool,
        /// charging station and EVSE for the given HTTP hostname and HTTP query
        /// parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="ChargingStation">The charging station.</param>
        /// <param name="EVSE">The EVSE.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndChargingPoolAndChargingStationAndEVSE(this HTTPRequest           HTTPRequest,
                                                                                          WWCP_HTTPAPI               WWCP_HTTPAPI,
                                                                                          out IRoamingNetwork?       RoamingNetwork,
                                                                                          out IChargingPool?         ChargingPool,
                                                                                          out IChargingStation?      ChargingStation,
                                                                                          out IEVSE?                 EVSE,
                                                                                          out HTTPResponse.Builder?  HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest is null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCP_HTTPAPI is null)
                throw new ArgumentNullException(nameof(WWCP_HTTPAPI),      "The given OpenChargingCloud API must not be null!");

            #endregion

            RoamingNetwork   = null;
            ChargingPool     = null;
            ChargingStation  = null;
            EVSE             = null;
            HTTPResponse     = null;

            if (HTTPRequest.ParsedURLParameters.Length < 4)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponse))
                return false;

            #region Get charging pool...

            if (!ChargingPool_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var chargingPoolId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingPoolById(chargingPoolId, out ChargingPool))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            #endregion

            #region Get charging station...

            if (!ChargingStation_Id.TryParse(HTTPRequest.ParsedURLParameters[2], out var chargingStationId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationById(chargingStationId, out ChargingStation))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            #endregion

            #region Get EVSE

            if (!EVSE_Id.TryParse(HTTPRequest.ParsedURLParameters[3], out var evseId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid EVSEId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetEVSEById(evseId, out EVSE))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown EVSEId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            #endregion

            return true;

        }

        #endregion


        #region ParseRoamingNetworkAndChargingStationOperatorAndBrand(this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingStationOperator, out Brand, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging station operator
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndChargingStationOperatorAndBrand(this HTTPRequest               HTTPRequest,
                                                                                    WWCP_HTTPAPI                   WWCP_HTTPAPI,
                                                                                    out IRoamingNetwork?           RoamingNetwork,
                                                                                    out IChargingStationOperator?  ChargingStationOperator,
                                                                                    out Brand?                     Brand,
                                                                                    out HTTPResponse.Builder?      HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest is null)
                throw new ArgumentNullException(nameof(HTTPRequest),           "The given HTTP request must not be null!");

            if (WWCP_HTTPAPI is null)
                throw new ArgumentNullException(nameof(WWCP_HTTPAPI),  "The given OpenChargingCloud API must not be null!");

            #endregion

            RoamingNetwork           = null;
            ChargingStationOperator  = null;
            Brand                    = null;
            HTTPResponse             = null;

            if (HTTPRequest.ParsedURLParameters.Length < 3)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!ChargingStationOperator_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var chargingStationOperatorId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationOperatorById(chargingStationOperatorId, out ChargingStationOperator)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }



            if (!Brand_Id.TryParse(HTTPRequest.ParsedURLParameters[2], out var brandId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid BrandId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            var brand = ChargingStationOperator.Brands.FirstOrDefault(brand => brand.Id == brandId);
            if (brand is null) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown BrandId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingStationOperatorAndChargingStationGroup(this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingStationOperator, out ChargingStationGroup, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging station operator
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndChargingStationOperatorAndChargingStationGroup(this HTTPRequest               HTTPRequest,
                                                                                                   WWCP_HTTPAPI                   WWCP_HTTPAPI,
                                                                                                   out IRoamingNetwork?           RoamingNetwork,
                                                                                                   out IChargingStationOperator?  ChargingStationOperator,
                                                                                                   out ChargingStationGroup?      ChargingStationGroup,
                                                                                                   out HTTPResponse.Builder?      HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest is null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCP_HTTPAPI is null)
                throw new ArgumentNullException(nameof(WWCP_HTTPAPI),  "The given OpenChargingCloud API must not be null!");

            #endregion

            RoamingNetwork           = null;
            ChargingStationOperator  = null;
            ChargingStationGroup     = null;
            HTTPResponse             = null;

            if (HTTPRequest.ParsedURLParameters.Length < 3)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!ChargingStationOperator_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var chargingStationOperatorId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationOperatorById(chargingStationOperatorId, out ChargingStationOperator)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }



            if (!ChargingStationGroup_Id.TryParse(HTTPRequest.ParsedURLParameters[2], out var chargingStationGroupId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationGroupId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!ChargingStationOperator.TryGetChargingStationGroup(chargingStationGroupId, out ChargingStationGroup)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationGroupId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingStationOperatorAndEVSEGroup(this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingStationOperator, out ChargingStationGroup, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging station operator
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndChargingStationOperatorAndEVSEGroup(this HTTPRequest               HTTPRequest,
                                                                                        WWCP_HTTPAPI                   WWCP_HTTPAPI,
                                                                                        out IRoamingNetwork?           RoamingNetwork,
                                                                                        out IChargingStationOperator?  ChargingStationOperator,
                                                                                        out EVSEGroup?                 EVSEGroup,
                                                                                        out HTTPResponse.Builder?      HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest is null)
                throw new ArgumentNullException(nameof(HTTPRequest),           "The given HTTP request must not be null!");

            if (WWCP_HTTPAPI is null)
                throw new ArgumentNullException(nameof(WWCP_HTTPAPI),  "The given OpenChargingCloud API must not be null!");

            #endregion

            RoamingNetwork           = null;
            ChargingStationOperator  = null;
            EVSEGroup                = null;
            HTTPResponse             = null;

            if (HTTPRequest.ParsedURLParameters.Length < 3)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(        WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!ChargingStationOperator_Id.TryParse(HTTPRequest.ParsedURLParameters[1], out var chargingStationOperatorId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationOperatorById(chargingStationOperatorId, out ChargingStationOperator)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }



            if (!EVSEGroup_Id.TryParse(HTTPRequest.ParsedURLParameters[2], out var evseGroupId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid EVSEGroupId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!ChargingStationOperator.TryGetEVSEGroup(evseGroupId, out EVSEGroup)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCP_HTTPAPI.HTTPServerName,
                    Date            = Timestamp.Now,
                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown EVSEGroupId!"" }".ToUTF8Bytes(),
                    Connection      = ConnectionType.KeepAlive
                };

                return false;

            }

            return true;

        }

        #endregion


        // Additional HTTP methods for HTTP clients

        #region RESERVE    (this HTTPClient, Path, ...)

        /// <summary>
        /// Create a new HTTP RESERVE request.
        /// </summary>
        /// <param name="HTTPClient">A HTTP client.</param>
        /// <param name="Path">A HTTP path.</param>
        /// <param name="RequestBuilder">A delegate to configure the new HTTP request builder.</param>
        /// <param name="Authentication">An optional HTTP authentication.</param>
        public static Task<HTTPResponse> RESERVE(this AHTTPClient              HTTPClient,
                                                 HTTPPath                      Path,
                                                 Action<HTTPRequest.Builder>?  RequestBuilder   = null,
                                                 IHTTPAuthentication?          Authentication   = null)

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.RESERVE,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
               );

        #endregion

        #region SETEXPIRED (this HTTPClient, Path, ...)

        /// <summary>
        /// Create a new HTTP SETEXPIRED request.
        /// </summary>
        /// <param name="HTTPClient">A HTTP client.</param>
        /// <param name="Path">A HTTP path.</param>
        /// <param name="RequestBuilder">A delegate to configure the new HTTP request builder.</param>
        /// <param name="Authentication">An optional HTTP authentication.</param>
        public static Task<HTTPResponse> SETEXPIRED(this AHTTPClient              HTTPClient,
                                                    HTTPPath                      Path,
                                                    Action<HTTPRequest.Builder>?  RequestBuilder   = null,
                                                    IHTTPAuthentication?          Authentication   = null)

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.SETEXPIRED,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
               );

        #endregion

        #region AUTHSTART  (this HTTPClient, Path, ...)

        /// <summary>
        /// Create a new HTTP AUTHSTART request.
        /// </summary>
        /// <param name="HTTPClient">A HTTP client.</param>
        /// <param name="Path">A HTTP path.</param>
        /// <param name="RequestBuilder">A delegate to configure the new HTTP request builder.</param>
        /// <param name="Authentication">An optional HTTP authentication.</param>
        public static Task<HTTPResponse> AUTHSTART(this AHTTPClient              HTTPClient,
                                                   HTTPPath                      Path,
                                                   Action<HTTPRequest.Builder>?  RequestBuilder   = null,
                                                   IHTTPAuthentication?          Authentication   = null)

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.AUTHSTART,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
               );

        #endregion

        #region AUTHSTOP   (this HTTPClient, Path, ...)

        /// <summary>
        /// Create a new HTTP REMOTESTOP request.
        /// </summary>
        /// <param name="HTTPClient">A HTTP client.</param>
        /// <param name="Path">A HTTP path.</param>
        /// <param name="RequestBuilder">A delegate to configure the new HTTP request builder.</param>
        /// <param name="Authentication">An optional HTTP authentication.</param>
        public static Task<HTTPResponse> AUTHSTOP(this AHTTPClient              HTTPClient,
                                                  HTTPPath                      Path,
                                                  Action<HTTPRequest.Builder>?  RequestBuilder   = null,
                                                  IHTTPAuthentication?          Authentication   = null)

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.AUTHSTOP,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
               );

        #endregion

        #region REMOTESTART(this HTTPClient, Path, ...)

        /// <summary>
        /// Create a new HTTP REMOTESTART request.
        /// </summary>
        /// <param name="HTTPClient">A HTTP client.</param>
        /// <param name="Path">A HTTP path.</param>
        /// <param name="RequestBuilder">A delegate to configure the new HTTP request builder.</param>
        /// <param name="Authentication">An optional HTTP authentication.</param>
        public static Task<HTTPResponse> REMOTESTART(this AHTTPClient              HTTPClient,
                                                     HTTPPath                      Path,
                                                     Action<HTTPRequest.Builder>?  RequestBuilder   = null,
                                                     IHTTPAuthentication?          Authentication   = null)

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.REMOTESTART,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
               );

        #endregion

        #region REMOTESTOP (this HTTPClient, Path, ...)

        /// <summary>
        /// Create a new HTTP REMOTESTOP request.
        /// </summary>
        /// <param name="HTTPClient">A HTTP client.</param>
        /// <param name="Path">A HTTP path.</param>
        /// <param name="RequestBuilder">A delegate to configure the new HTTP request builder.</param>
        /// <param name="Authentication">An optional HTTP authentication.</param>
        public static Task<HTTPResponse> REMOTESTOP(this AHTTPClient              HTTPClient,
                                                    HTTPPath                      Path,
                                                    Action<HTTPRequest.Builder>?  RequestBuilder   = null,
                                                    IHTTPAuthentication?          Authentication   = null)

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.REMOTESTOP,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
               );

        #endregion

        #region SENDCDR    (this HTTPClient, Path, ...)

        /// <summary>
        /// Create a new HTTP SENDCDR request.
        /// </summary>
        /// <param name="HTTPClient">A HTTP client.</param>
        /// <param name="Path">A HTTP path.</param>
        /// <param name="RequestBuilder">A delegate to configure the new HTTP request builder.</param>
        /// <param name="Authentication">An optional HTTP authentication.</param>
        public static Task<HTTPResponse> SENDCDR(this AHTTPClient              HTTPClient,
                                                 HTTPPath                      Path,
                                                 Action<HTTPRequest.Builder>?  RequestBuilder   = null,
                                                 IHTTPAuthentication?          Authentication   = null)

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.SENDCDR,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
               );

        #endregion


    }


    /// <summary>
    /// The WWCP HTTP API.
    /// </summary>
    public class WWCP_HTTPAPI : AHTTPExtAPIXExtension<HTTPExtAPIX>
    {

        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const           String         DefaultHTTPServerName           = "GraphDefined WWCP HTTP API";

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const           String         DefaultHTTPServiceName          = "GraphDefined WWCP HTTP API";


        private readonly           LogFileWriter  logFileWriter                   = new (10000);

        #endregion

        #region Properties

        /// <summary>
        /// WWCP Core.
        /// </summary>
        public IWWCPCore                WWCPCore                   { get; }


        /// <summary>
        /// An optional additional URL path prefix.
        /// </summary>
        public HTTPPath?                AdditionalURLPathPrefix    { get; }

        /// <summary>
        /// Allow anonymous access to locations as Open Data.
        /// </summary>
        public Boolean                  LocationsAsOpenData        { get; }

        /// <summary>
        /// Allow anonymous access to tariffs as Open Data.
        /// </summary>
        public Boolean                  TariffsAsOpenData          { get; }

        /// <summary>
        /// (Dis-)allow PUTting of object having an earlier 'LastUpdated'-timestamp then already existing objects.
        /// WWCP v2.2 does not define any behaviour for this.
        /// </summary>
        public Boolean?                 AllowDowngrades            { get; }

        ///// <summary>
        ///// The logging context.
        ///// </summary>
        //public String?                  LoggingContext             { get; }

        /// <summary>
        /// The WWCP HTTP API logger.
        /// </summary>
        public WWCP_HTTPAPI_Logger?     Logger                     { get; set; }

        #endregion

        #region Events

        #region (protected internal) GetRootRequest      (Request)

        /// <summary>
        /// An event sent whenever a GET / request was received.
        /// </summary>
        public HTTPRequestLogEventX OnGetRootRequest = new();

        /// <summary>
        /// An event sent whenever a GET / request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The Common API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task GetRootRequest(DateTimeOffset     Timestamp,
                                               HTTPAPIX           API,
                                               HTTPRequest        Request,
                                               CancellationToken  CancellationToken)

            => OnGetRootRequest.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   CancellationToken
               );

        #endregion

        #region (protected internal) GetRootResponse     (Response)

        /// <summary>
        /// An event sent whenever a GET / response was sent.
        /// </summary>
        public HTTPResponseLogEventX OnGetRootResponse = new();

        /// <summary>
        /// An event sent whenever a GET / response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The Common API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task GetRootResponse(DateTimeOffset     Timestamp,
                                                HTTPAPIX           API,
                                                HTTPRequest        Request,
                                                HTTPResponse       Response,
                                                CancellationToken  CancellationToken)

            => OnGetRootResponse.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   Response,
                   CancellationToken
               );

        #endregion

        #endregion

        #region Additional HTTP methods

        public readonly static HTTPMethod RESERVE      = HTTPMethod.TryParse("RESERVE",     IsSafe: false, IsIdempotent: true)!;
        public readonly static HTTPMethod SETEXPIRED   = HTTPMethod.TryParse("SETEXPIRED",  IsSafe: false, IsIdempotent: true)!;
        public readonly static HTTPMethod AUTHSTART    = HTTPMethod.TryParse("AUTHSTART",   IsSafe: false, IsIdempotent: true)!;
        public readonly static HTTPMethod AUTHSTOP     = HTTPMethod.TryParse("AUTHSTOP",    IsSafe: false, IsIdempotent: true)!;
        public readonly static HTTPMethod REMOTESTART  = HTTPMethod.TryParse("REMOTESTART", IsSafe: false, IsIdempotent: true)!;
        public readonly static HTTPMethod REMOTESTOP   = HTTPMethod.TryParse("REMOTESTOP",  IsSafe: false, IsIdempotent: true)!;
        public readonly static HTTPMethod SENDCDR      = HTTPMethod.TryParse("SENDCDR",     IsSafe: false, IsIdempotent: true)!;

        #endregion

        #region Custom JSON serializers

        //public CustomJObjectSerializerDelegate<VersionInformation>?           CustomVersionInformationSerializer            { get; set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP HTTP API.
        /// </summary>
        /// <param name="OurBaseURL">The URL for your API endpoint.</param>
        /// <param name="OurVersionsURL">The URL of our VERSIONS endpoint.</param>
        /// 
        /// <param name="AdditionalURLPathPrefix"></param>
        /// <param name="LocationsAsOpenData">Allow anonymous access to locations as Open Data.</param>
        /// <param name="TariffsAsOpenData">Allow anonymous access to tariffs as Open Data.</param>
        /// <param name="AllowDowngrades">(Dis-)allow PUTting of object having an earlier 'LastUpdated'-timestamp then already existing objects.</param>
        /// 
        /// <param name="ExternalDNSName">The official URL/DNS name of this service, e.g. for sending e-mails.</param>
        /// <param name="BasePath">When the API is served from an optional subdirectory path.</param>
        /// <param name="HTTPServerName">The default HTTP server name, used whenever no HTTP Host-header has been given.</param>
        /// 
        /// <param name="URLPathPrefix">A common prefix for all URLs.</param>
        /// <param name="HTTPServiceName">The name of the HTTP service.</param>
        /// <param name="APIVersionHashes">The API version hashes (git commit hash values).</param>
        /// 
        /// <param name="IsDevelopment">This HTTP API runs in development mode.</param>
        /// <param name="DevelopmentServers">An enumeration of server names which will imply to run this service in development mode.</param>
        /// <param name="DisableLogging">Disable any logging.</param>
        /// <param name="LoggingPath">The path for all logfiles.</param>
        /// <param name="LogfileName">The name of the logfile.</param>
        /// <param name="LogfileCreator">A delegate for creating the name of the logfile for this API.</param>
        public WWCP_HTTPAPI(HTTPExtAPIX                    HTTPAPI,
                            IWWCPCore                      WWCPCore,
                            URL                            OurBaseURL,
                            URL                            OurVersionsURL,

                            IEnumerable<HTTPHostname>?     Hostnames                 = null,
                            HTTPPath?                      RootPath                  = null,
                            IEnumerable<HTTPContentType>?  HTTPContentTypes          = null,
                            I18NString?                    Description               = null,

                            HTTPPath?                      BasePath                  = null,  // For URL prefixes in HTML!

                            String?                        ExternalDNSName           = null,
                            String?                        HTTPServerName            = DefaultHTTPServerName,
                            String?                        HTTPServiceName           = DefaultHTTPServiceName,
                            String?                        APIVersionHash            = null,
                            JObject?                       APIVersionHashes          = null,

                            EMailAddress?                  APIRobotEMailAddress      = null,
                            String?                        APIRobotGPGPassphrase     = null,
                            ISMTPClient?                   SMTPClient                = null,

                            HTTPPath?                      AdditionalURLPathPrefix   = null,
                            Boolean                        LocationsAsOpenData       = true,
                            Boolean                        TariffsAsOpenData         = false,
                            Boolean?                       AllowDowngrades           = null,

                            Boolean?                       IsDevelopment             = null,
                            IEnumerable<String>?           DevelopmentServers        = null,
                            //Boolean?                       SkipURLTemplates          = false,
                            //String?                        DatabaseFileName          = DefaultAssetsDBFileName,
                            Boolean?                       DisableNotifications      = false,

                            Boolean?                       DisableLogging            = null,
                            String?                        LoggingContext            = null,
                            String?                        LoggingPath               = null,
                            String?                        LogfileName               = null,
                            WWCPLogfileCreatorDelegate?    LogfileCreator            = null)

            : base(Description ?? I18NString.Create("WWCP HTTP API"),
                   HTTPAPI,
                   RootPath,
                   BasePath,

                   ExternalDNSName,
                   HTTPServerName,
                   HTTPServiceName,
                   APIVersionHash,
                   APIVersionHashes,

                   IsDevelopment,
                   DevelopmentServers,
                   DisableLogging,
                   LoggingPath,
                   LogfileName,
                   LogfileCreator is not null
                       ? (loggingPath, context, logfileName) => LogfileCreator(loggingPath, context, logfileName)
                       : null)

        {

            this.WWCPCore                 = WWCPCore;

            this.AdditionalURLPathPrefix  = AdditionalURLPathPrefix;
            this.LocationsAsOpenData      = LocationsAsOpenData;
            this.TariffsAsOpenData        = TariffsAsOpenData;
            this.AllowDowngrades          = AllowDowngrades;

            RegisterURLTemplates();

            if (!this.DisableLogging)
                Logger = new WWCP_HTTPAPI_Logger(
                             this,
                             LoggingPath ?? AppContext.BaseDirectory,
                             LoggingContext,
                             LogfileCreator: LogfileCreator
                         );

        }

        #endregion


        #region (private) RegisterURLTemplates()

        private void RegisterURLTemplates()
        {

            #region OPTIONS  ~/

            HTTPBaseAPI.AddHandler(
                HTTPMethod.OPTIONS,
                URLPathPrefix,
                HTTPDelegate: request =>

                    Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "GET" ],
                            Allow                      = [ HTTPMethod.OPTIONS, HTTPMethod.GET ],
                            AccessControlAllowHeaders  = [ "Authorization" ],
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable)

            );

            #endregion

            #region GET      ~/

            HTTPBaseAPI.AddHandler(
                HTTPMethod.GET,
                URLPathPrefix,
                HTTPContentType.Text.PLAIN,
                HTTPRequestLogger:   GetRootRequest,
                HTTPResponseLogger:  GetRootResponse,
                HTTPDelegate:        request =>

                    Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "GET" ],
                            AccessControlAllowHeaders  = [ "Authorization" ],
                            ContentType                = HTTPContentType.Text.PLAIN,
                            Content                    = "This is an Open Charge Point Interface v2.x HTTP service!\r\nPlease check ~/versions!".ToUTF8Bytes(),
                            Connection                 = ConnectionType.KeepAlive,
                            Vary                       = "Accept"
                        }.AsImmutable)

            );

            HTTPBaseAPI.AddHandler(
                HTTPMethod.GET,
                URLPathPrefix,
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   GetRootRequest,
                HTTPResponseLogger:  GetRootResponse,
                HTTPDelegate:        request =>

                    Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "GET" ],
                            AccessControlAllowHeaders  = [ "Authorization" ],
                            ContentType                = HTTPContentType.Application.JSON_UTF8,
                            Content                    = JSONObject.Create(
                                                             new JProperty(
                                                                 "message",
                                                                 "This is an Open Charge Point Interface v2.x HTTP service! Please check ~/versions!"
                                                             )
                                                         ).ToUTF8Bytes(),
                            Connection                 = ConnectionType.KeepAlive,
                            Vary                       = "Accept"
                        }.AsImmutable),

                AllowReplacement: URLReplacement.Allow

            );

            #endregion



            #region GET      ~/support

            HTTPBaseAPI.AddHandler(
                HTTPMethod.GET,
                URLPathPrefix + "/support",
                HTTPContentType.Text.PLAIN,
                HTTPDelegate: request =>

                    Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServerName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "GET" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            ContentType                = HTTPContentType.Text.PLAIN,
                            Content                    = "https://github.com/OpenChargingCloud/WWCP_WWCP".ToUTF8Bytes(),
                            Connection                 = ConnectionType.KeepAlive,
                            Vary                       = "Accept"
                        }.AsImmutable
                    )

            );

            #endregion

        }

        #endregion


    }

}
