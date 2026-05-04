/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Logging;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Hermod.SMTP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.WWCP.Networking;

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

        public const String RoamingNetworkId           = "RoamingNetworkId";
        public const String ChargingStationOperatorId  = "ChargingStationOperatorId";
        public const String ChargingPoolId             = "ChargingPoolId";
        public const String ChargingStationId          = "ChargingStationId";
        public const String EVSEId                     = "EVSEId";
        public const String ChargingSessionId          = "ChargingSessionId";
        public const String ChargingReservationId      = "ChargingReservationId";

        public const String EMobilityProviderId        = "EMobilityProviderId";


        #region TryParseRoamingNetworkId                           (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetworkId,                              out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and roaming network identification.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetworkId">The roaming network identification.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkId(this HTTPRequest                                HTTPRequest,
                                                       WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                       [NotNullWhen(true)]  out RoamingNetwork_Id      RoamingNetworkId,
                                                       [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            HTTPResponseBuilder = null;

            if (!HTTPRequest.TryParseURLParameter(
                WWCP_HTTPAPIExtensions.RoamingNetworkId,
                RoamingNetwork_Id.TryParse,
                out RoamingNetworkId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Invalid roaming network identification '{RoamingNetworkId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region TryParseRoamingNetwork                             (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork,                                out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and roaming network.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetwork(this HTTPRequest                                HTTPRequest,
                                                     WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                     [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                     [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetworkId(
                WWCP_HTTPAPI,
                out var roamingNetworkId,
                out HTTPResponseBuilder))
            {
                RoamingNetwork = null;
                return false;
            }

            if (!WWCP_HTTPAPI.TryGetRoamingNetwork(roamingNetworkId,
                                                   out RoamingNetwork,
                                                   HTTPRequest.Host))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Unknown roaming network identification '{roamingNetworkId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion


        #region TryParseRoamingNetworkAndChargingStationOperatorId (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingStationOperatorId, out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and charging station operator identification.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperatorId">The charging station operator identification.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndChargingStationOperatorId(this HTTPRequest                                     HTTPRequest,
                                                                                 WWCP_HTTPAPI                                         WWCP_HTTPAPI,
                                                                                 [NotNullWhen(true)]  out IRoamingNetwork?            RoamingNetwork,
                                                                                 [NotNullWhen(true)]  out ChargingStationOperator_Id  ChargingStationOperatorId,
                                                                                 [NotNullWhen(false)] out HTTPResponse.Builder?       HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetwork(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out HTTPResponseBuilder))
            {
                ChargingStationOperatorId = default;
                return false;
            }

            if (!HTTPRequest.TryParseURLParameter(
                WWCP_HTTPAPIExtensions.ChargingStationOperatorId,
                ChargingStationOperator_Id.TryParse,
                out ChargingStationOperatorId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Invalid charging station operator identification '{ChargingStationOperatorId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region TryParseRoamingNetworkAndChargingStationOperator   (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingStationOperator,   out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and charging station operator.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndChargingStationOperator(this HTTPRequest                                    HTTPRequest,
                                                                               WWCP_HTTPAPI                                        WWCP_HTTPAPI,
                                                                               [NotNullWhen(true)]  out IRoamingNetwork?           RoamingNetwork,
                                                                               [NotNullWhen(true)]  out IChargingStationOperator?  ChargingStationOperator,
                                                                               [NotNullWhen(false)] out HTTPResponse.Builder?      HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetworkAndChargingStationOperatorId(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out var chargingStationOperatorId,
                out HTTPResponseBuilder))
            {
                ChargingStationOperator = null;
                return false;
            }

            if (!RoamingNetwork.TryGetChargingStationOperatorById(chargingStationOperatorId, out ChargingStationOperator))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Unknown charging station operator identification '{chargingStationOperatorId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region TryParseRoamingNetworkAndChargingPoolId            (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingPoolId,            out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and charging pool identification.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingPoolId">The charging pool identification.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndChargingPoolId(this HTTPRequest                                HTTPRequest,
                                                                      WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                      [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                      [NotNullWhen(true)]  out ChargingPool_Id        ChargingPoolId,
                                                                      [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetwork(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out HTTPResponseBuilder))
            {
                ChargingPoolId = default;
                return false;
            }

            if (!HTTPRequest.TryParseURLParameter(
                WWCP_HTTPAPIExtensions.ChargingPoolId,
                ChargingPool_Id.TryParse,
                out ChargingPoolId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Invalid charging pool identification '{ChargingPoolId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region TryParseRoamingNetworkAndChargingPool              (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingPool,              out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and charging pool.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndChargingPool(this HTTPRequest                                HTTPRequest,
                                                                    WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                    [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                    [NotNullWhen(true)]  out IChargingPool?         ChargingPool,
                                                                    [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetworkAndChargingPoolId(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out var chargingPoolId,
                out HTTPResponseBuilder))
            {
                ChargingPool = null;
                return false;
            }

            if (!RoamingNetwork.TryGetChargingPoolById(chargingPoolId, out ChargingPool))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Unknown charging pool identification '{chargingPoolId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region TryParseRoamingNetworkAndChargingStationId         (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingStationId,         out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and charging station identification.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationId">The charging station identification.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndChargingStationId(this HTTPRequest                                HTTPRequest,
                                                                         WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                         [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                         [NotNullWhen(true)]  out ChargingStation_Id     ChargingStationId,
                                                                         [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetwork(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out HTTPResponseBuilder))
            {
                ChargingStationId = default;
                return false;
            }

            if (!HTTPRequest.TryParseURLParameter(
                WWCP_HTTPAPIExtensions.ChargingStationId,
                ChargingStation_Id.TryParse,
                out ChargingStationId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Invalid charging station identification '{ChargingStationId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region TryParseRoamingNetworkAndChargingStation           (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingStation,           out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and charging station.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStation">The charging station.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndChargingStation(this HTTPRequest                                HTTPRequest,
                                                                       WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                       [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                       [NotNullWhen(true)]  out IChargingStation?      ChargingStation,
                                                                       [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetworkAndChargingStationId(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out var chargingStationId,
                out HTTPResponseBuilder))
            {
                ChargingStation = null;
                return false;
            }

            if (!RoamingNetwork.TryGetChargingStationById(chargingStationId, out ChargingStation))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Unknown charging station identification '{chargingStationId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region TryParseRoamingNetworkAndEVSEId                    (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out EVSEId,                    out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and EVSE identification.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndEVSEId(this HTTPRequest                                HTTPRequest,
                                                              WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                              [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                              [NotNullWhen(true)]  out EVSE_Id                EVSEId,
                                                              [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetwork(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out HTTPResponseBuilder))
            {
                EVSEId = default;
                return false;
            }

            if (!HTTPRequest.TryParseURLParameter(
                WWCP_HTTPAPIExtensions.EVSEId,
                EVSE_Id.TryParse,
                out EVSEId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Invalid EVSE identification '{EVSEId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region TryParseRoamingNetworkAndEVSE                      (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out EVSE,                      out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and EVSE
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="EVSE">The EVSE.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndEVSE(this HTTPRequest                                HTTPRequest,
                                                            WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                            [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                            [NotNullWhen(true)]  out IEVSE?                 EVSE,
                                                            [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetworkAndEVSEId(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out var evseId,
                out HTTPResponseBuilder))
            {
                EVSE = null;
                return false;
            }

            if (!RoamingNetwork.TryGetEVSEById(evseId, out EVSE))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Unknown EVSE identification '{evseId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion



        #region TryParseRoamingNetworkAndChargingPoolAndChargingStation(this HTTPRequest, OpenChargingCloudAPI, out RoamingNetwork, out ChargingPool, out ChargingStation, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network, charging pool
        /// and charging station for the given HTTP hostname and HTTP query
        /// parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="OpenChargingCloudAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="ChargingStation">The charging station.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndChargingPoolAndChargingStation(this HTTPRequest                                HTTPRequest,
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

            if (!HTTPRequest.TryParseRoamingNetwork(WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponse))
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

        #region TryParseRoamingNetworkAndChargingPoolAndChargingStationAndEVSE(this HTTPRequest, OpenChargingCloudAPI, out RoamingNetwork, out ChargingPool, out ChargingStation, out EVSE, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network, charging pool,
        /// charging station and EVSE for the given HTTP hostname and HTTP query
        /// parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="OpenChargingCloudAPI">The OpenChargingCloud API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="ChargingStation">The charging station.</param>
        /// <param name="EVSE">The EVSE.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndChargingPoolAndChargingStationAndEVSE(this HTTPRequest                                HTTPRequest,
                                                                                             WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                                             [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                                             [NotNullWhen(true)]  out IChargingPool?         ChargingPool,
                                                                                             [NotNullWhen(true)]  out IChargingStation?      ChargingStation,
                                                                                             [NotNullWhen(true)]  out IEVSE?                 EVSE,
                                                                                             [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest is null)
                throw new ArgumentNullException(nameof(HTTPRequest),   "The given HTTP request must not be null!");

            if (WWCP_HTTPAPI is null)
                throw new ArgumentNullException(nameof(WWCP_HTTPAPI),  "The given OpenChargingCloud API must not be null!");

            #endregion

            RoamingNetwork   = null;
            ChargingPool     = null;
            ChargingStation  = null;
            EVSE             = null;

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

            if (!HTTPRequest.TryParseRoamingNetwork(WWCP_HTTPAPI, out RoamingNetwork, out HTTPResponse))
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







        #region TryParseRoamingNetworkAndChargingSessionId         (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingSessionId,         out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and charging session identification.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndChargingSessionId(this HTTPRequest                                HTTPRequest,
                                                                         WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                         [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                         [NotNullWhen(true)]  out ChargingSession_Id     ChargingSessionId,
                                                                         [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetwork(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out HTTPResponseBuilder))
            {
                ChargingSessionId = default;
                return false;
            }

            if (!HTTPRequest.TryParseURLParameter(
                WWCP_HTTPAPIExtensions.ChargingSessionId,
                ChargingSession_Id.TryParse,
                out ChargingSessionId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Invalid charging session identification '{ChargingSessionId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region TryParseRoamingNetworkAndChargingSession           (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingSession,           out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging session
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingSession">The charging session.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndChargingSession(this HTTPRequest                                HTTPRequest,
                                                                       WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                       [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                       [NotNullWhen(true)]  out ChargingSession?       ChargingSession,
                                                                       [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetworkAndChargingSessionId(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out var chargingSessionId,
                out HTTPResponseBuilder))
            {
                ChargingSession = null;
                return false;
            }

            if (!RoamingNetwork.TryGetChargingSessionById(chargingSessionId, out ChargingSession))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Unknown charging session identification '{chargingSessionId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion


        #region TryParseRoamingNetworkAndReservationId             (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingReservationId,     out HTTPResponseBuilder)

        /// <summary>
        /// Try to parse the given HTTP request and return the roaming network and charging reservation identification.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingReservationId">The charging reservation identification.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndReservationId(this HTTPRequest                                 HTTPRequest,
                                                                     WWCP_HTTPAPI                                     WWCP_HTTPAPI,
                                                                     [NotNullWhen(true)]  out IRoamingNetwork?        RoamingNetwork,
                                                                     [NotNullWhen(true)]  out ChargingReservation_Id  ChargingReservationId,
                                                                     [NotNullWhen(false)] out HTTPResponse.Builder?   HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetwork(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out HTTPResponseBuilder))
            {
                ChargingReservationId = default;
                return false;
            }

            if (!HTTPRequest.TryParseURLParameter(
                WWCP_HTTPAPIExtensions.ChargingReservationId,
                ChargingReservation_Id.TryParse,
                out ChargingReservationId))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Invalid charging reservation identification '{ChargingReservationId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion

        #region TryParseRoamingNetworkAndReservation               (this HTTPRequest, WWCP_HTTPAPI, out RoamingNetwork, out ChargingReservation,       out HTTPResponseBuilder)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging reservation
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCP_HTTPAPI">The WWCP HTTP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Reservation">The charging reservation.</param>
        /// <param name="HTTPResponseBuilder">A HTTP error response.</param>
        public static Boolean TryParseRoamingNetworkAndReservation(this HTTPRequest                                HTTPRequest,
                                                                   WWCP_HTTPAPI                                    WWCP_HTTPAPI,
                                                                   [NotNullWhen(true)]  out IRoamingNetwork?       RoamingNetwork,
                                                                   [NotNullWhen(true)]  out ChargingReservation?   ChargingReservation,
                                                                   [NotNullWhen(false)] out HTTPResponse.Builder?  HTTPResponseBuilder)
        {

            if (!HTTPRequest.TryParseRoamingNetworkAndReservationId(
                WWCP_HTTPAPI,
                out RoamingNetwork,
                out var chargingReservationId,
                out HTTPResponseBuilder))
            {
                ChargingReservation = null;
                return false;
            }

            if (!RoamingNetwork.TryGetChargingReservationById(chargingReservationId, out ChargingReservation))
            {

                HTTPResponseBuilder = new HTTPResponse.Builder(HTTPRequest) {
                                          HTTPStatusCode  = HTTPStatusCode.NotFound,
                                          Server          = WWCP_HTTPAPI.HTTPServerName,
                                          Date            = Timestamp.Now,
                                          ContentType     = HTTPContentType.Application.JSON_UTF8,
                                          Content         = $"{{ \"description\": \"Unknown charging reservation identification '{chargingReservationId}'!\" }}".ToUTF8Bytes(),
                                          Connection      = ConnectionType.KeepAlive
                                      };

                return false;

            }

            return true;

        }

        #endregion



        // Additional HTTP methods for HTTP clients

        #region RESERVE     (this HTTPClient, Path, ...)

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

            => HTTPClient.RunRequest(
                   WWCP_HTTPAPI.RESERVE,
                   Path,
                   Authentication:  Authentication,
                   RequestBuilder:  RequestBuilder
               );

        #endregion

        #region SETEXPIRED  (this HTTPClient, Path, ...)

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

            => HTTPClient.RunRequest(
                   WWCP_HTTPAPI.SETEXPIRED,
                   Path,
                   Authentication:  Authentication,
                   RequestBuilder:  RequestBuilder
               );

        #endregion

        #region AUTHSTART   (this HTTPClient, Path, ...)

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

            => HTTPClient.RunRequest(
                   WWCP_HTTPAPI.AUTHSTART,
                   Path,
                   Authentication:  Authentication,
                   RequestBuilder:  RequestBuilder
               );

        #endregion

        #region AUTHSTOP    (this HTTPClient, Path, ...)

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

            => HTTPClient.RunRequest(
                   WWCP_HTTPAPI.AUTHSTOP,
                   Path,
                   Authentication:  Authentication,
                   RequestBuilder:  RequestBuilder
               );

        #endregion

        #region REMOTESTART (this HTTPClient, Path, ...)

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

            => HTTPClient.RunRequest(
                   WWCP_HTTPAPI.REMOTESTART,
                   Path,
                   Authentication:  Authentication,
                   RequestBuilder:  RequestBuilder
               );

        #endregion

        #region REMOTESTOP  (this HTTPClient, Path, ...)

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

            => HTTPClient.RunRequest(
                   WWCP_HTTPAPI.REMOTESTOP,
                   Path,
                   Authentication:  Authentication,
                   RequestBuilder:  RequestBuilder
               );

        #endregion

        #region SENDCDR     (this HTTPClient, Path, ...)

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

            => HTTPClient.RunRequest(
                   WWCP_HTTPAPI.SENDCDR,
                   Path,
                   Authentication:  Authentication,
                   RequestBuilder:  RequestBuilder
               );

        #endregion


    }


    /// <summary>
    /// The WWCP HTTP API.
    /// </summary>
    public class WWCP_HTTPAPI : AHTTPExtAPIExtension1<HTTPExtAPI>
    {

        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const  String           DefaultHTTPServerName     = "GraphDefined WWCP HTTP API";

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const  String           DefaultHTTPServiceName    = "GraphDefined WWCP HTTP API";


        private readonly  LogFileWriter    logFileWriter             = new (10000);

        public            WWWAuthenticate  WWWAuthenticateDefaults   = WWWAuthenticate.Basic("WWCP");

        #endregion

        #region Properties

        /// <summary>
        /// WWCP Core.
        /// </summary>
        public ConcurrentDictionary<HTTPHostname, IWWCPCore>  WWCPCores                    { get; } = [];


        /// <summary>
        /// An optional additional URL path prefix.
        /// </summary>
        public HTTPPath?                                      AdditionalURLPathPrefix      { get; }

        /// <summary>
        /// Whether this API allows anonymous read access.
        /// </summary>
        public Boolean                                        AllowAnonymousReadAccesss    { get; }

        /// <summary>
        /// Allow anonymous access to locations as Open Data.
        /// </summary>
        public Boolean                                        LocationsAsOpenData          { get; }

        /// <summary>
        /// Allow anonymous access to tariffs as Open Data.
        /// </summary>
        public Boolean                                        TariffsAsOpenData            { get; }

        /// <summary>
        /// (Dis-)allow PUTting of object having an earlier 'LastUpdated'-timestamp then already existing objects.
        /// WWCP v2.2 does not define any behaviour for this.
        /// </summary>
        public Boolean?                                       AllowDowngrades              { get; }

        ///// <summary>
        ///// The logging context.
        ///// </summary>
        //public String?                                        LoggingContext               { get; }

        /// <summary>
        /// The WWCP HTTP API logger.
        /// </summary>
        public WWCP_HTTPAPI_Logger?                           Logger                       { get; set; }

        #endregion

        #region Events

        #region (protected internal) GetRootRequest      (Request)

        /// <summary>
        /// An event sent whenever a GET / request was received.
        /// </summary>
        public HTTPRequestLogEvent OnGetRootRequest = new();

        /// <summary>
        /// An event sent whenever a GET / request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The Common API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task GetRootRequest(DateTimeOffset     Timestamp,
                                               HTTPAPI            API,
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
        public HTTPResponseLogEvent OnGetRootResponse = new();

        /// <summary>
        /// An event sent whenever a GET / response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The Common API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task GetRootResponse(DateTimeOffset     Timestamp,
                                                HTTPAPI            API,
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


        #region (protected internal) CreateRoamingNetworkRequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnCreateRoamingNetworkRequest = new();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task CreateRoamingNetworkRequest(DateTimeOffset     Timestamp,
                                                            HTTPAPI            API,
                                                            HTTPRequest        Request,
                                                            CancellationToken  CancellationToken)

            => OnCreateRoamingNetworkRequest.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   CancellationToken
               );

        #endregion

        #region (protected internal) CreateRoamingNetworkResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnCreateRoamingNetworkResponse = new();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task CreateRoamingNetworkResponse(DateTimeOffset     Timestamp,
                                                             HTTPAPI            API,
                                                             HTTPRequest        Request,
                                                             HTTPResponse       Response,
                                                             CancellationToken  CancellationToken)

            => OnCreateRoamingNetworkResponse.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   Response,
                   CancellationToken
               );

        #endregion


        #region (protected internal) DeleteRoamingNetworkRequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEvent OnDeleteRoamingNetworkRequest = new();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task DeleteRoamingNetworkRequest(DateTimeOffset     Timestamp,
                                                            HTTPAPI            API,
                                                            HTTPRequest        Request,
                                                            CancellationToken  CancellationToken)

            => OnDeleteRoamingNetworkRequest.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   CancellationToken
               );

        #endregion

        #region (protected internal) DeleteRoamingNetworkResponse(Response)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnDeleteRoamingNetworkResponse = new();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task DeleteRoamingNetworkResponse(DateTimeOffset     Timestamp,
                                                             HTTPAPI            API,
                                                             HTTPRequest        Request,
                                                             HTTPResponse       Response,
                                                             CancellationToken  CancellationToken)

            => OnDeleteRoamingNetworkResponse.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   Response,
                   CancellationToken
               );

        #endregion



        // Refactored

        #region (protected internal) SetChargingStationOperatorAdminStatusRequest  (Request)

        /// <summary>
        /// An event sent whenever a SetChargingStationOperatorAdminStatus HTTP was received.
        /// </summary>
        public HTTPRequestLogEvent OnSetChargingStationOperatorAdminStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a SetChargingStationOperatorAdminStatus HTTP was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingStationOperatorAdminStatusRequest(DateTimeOffset     Timestamp,
                                                                             HTTPAPI           HTTPAPI,
                                                                             HTTPRequest        HTTPRequest,
                                                                             CancellationToken  CancellationToken)

            => OnSetChargingStationOperatorAdminStatusHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SetChargingStationOperatorAdminStatusResponse (Response)

        /// <summary>
        /// An event sent whenever a SetChargingStationOperatorAdminStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSetChargingStationOperatorAdminStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a SetChargingStationOperatorAdminStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingStationOperatorAdminStatusResponse(DateTimeOffset     Timestamp,
                                                                              HTTPAPI           HTTPAPI,
                                                                              HTTPRequest        HTTPRequest,
                                                                              HTTPResponse       HTTPResponse,
                                                                              CancellationToken  CancellationToken)

            => OnSetChargingStationOperatorAdminStatusHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion


        #region (protected internal) SetChargingStationOperatorStatusRequest       (Request)

        /// <summary>
        /// An event sent whenever a SetChargingStationOperatorStatus HTTP was received.
        /// </summary>
        public HTTPRequestLogEvent OnSetChargingStationOperatorStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a SetChargingStationOperatorStatus HTTP was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingStationOperatorStatusRequest(DateTimeOffset     Timestamp,
                                                                        HTTPAPI           HTTPAPI,
                                                                        HTTPRequest        HTTPRequest,
                                                                        CancellationToken  CancellationToken)

            => OnSetChargingStationOperatorStatusHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SetChargingStationOperatorStatusResponse      (Response)

        /// <summary>
        /// An event sent whenever a SetChargingStationOperatorStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSetChargingStationOperatorStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a SetChargingStationOperatorStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingStationOperatorStatusResponse(DateTimeOffset     Timestamp,
                                                                         HTTPAPI           HTTPAPI,
                                                                         HTTPRequest        HTTPRequest,
                                                                         HTTPResponse       HTTPResponse,
                                                                         CancellationToken  CancellationToken)

            => OnSetChargingStationOperatorStatusHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion




        #region (protected internal) GetChargingPoolsRequest                       (Request)

        /// <summary>
        /// An event sent whenever a GetChargingPools request was received.
        /// </summary>
        public HTTPRequestLogEvent OnGetChargingPoolsRequest = new();

        /// <summary>
        /// An event sent whenever a GetChargingPools request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task GetChargingPoolsRequest(DateTimeOffset     Timestamp,
                                                        HTTPAPI            API,
                                                        HTTPRequest        Request,
                                                        CancellationToken  CancellationToken)

            => OnGetChargingPoolsRequest.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   CancellationToken
               );

        #endregion

        #region (protected internal) GetChargingPoolsResponse                      (Response)

        /// <summary>
        /// An event sent whenever a GetChargingPools response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnGetChargingPoolsResponse = new();

        /// <summary>
        /// An event sent whenever a GetChargingPools response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task GetChargingPoolsResponse(DateTimeOffset     Timestamp,
                                                         HTTPAPI            API,
                                                         HTTPRequest        Request,
                                                         HTTPResponse       Response,
                                                         CancellationToken  CancellationToken)

            => OnGetChargingPoolsResponse.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   Response,
                   CancellationToken
               );

        #endregion


        #region (protected internal) SetChargingPoolAdminStatusRequest             (Request)

        /// <summary>
        /// An event sent whenever a SetChargingPoolAdminStatus HTTP was received.
        /// </summary>
        public HTTPRequestLogEvent OnSetChargingPoolAdminStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a SetChargingPoolAdminStatus HTTP was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingPoolAdminStatusRequest(DateTimeOffset     Timestamp,
                                                                  HTTPAPI           HTTPAPI,
                                                                  HTTPRequest        HTTPRequest,
                                                                  CancellationToken  CancellationToken)

            => OnSetChargingPoolAdminStatusHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SetChargingPoolAdminStatusResponse            (Response)

        /// <summary>
        /// An event sent whenever a SetChargingPoolAdminStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSetChargingPoolAdminStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a SetChargingPoolAdminStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingPoolAdminStatusResponse(DateTimeOffset     Timestamp,
                                                                   HTTPAPI           HTTPAPI,
                                                                   HTTPRequest        HTTPRequest,
                                                                   HTTPResponse       HTTPResponse,
                                                                   CancellationToken  CancellationToken)

            => OnSetChargingPoolAdminStatusHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion


        #region (protected internal) SetChargingPoolStatusRequest                  (Request)

        /// <summary>
        /// An event sent whenever a SetChargingPoolStatus HTTP was received.
        /// </summary>
        public HTTPRequestLogEvent OnSetChargingPoolStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a SetChargingPoolStatus HTTP was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingPoolStatusRequest(DateTimeOffset     Timestamp,
                                                             HTTPAPI           HTTPAPI,
                                                             HTTPRequest        HTTPRequest,
                                                             CancellationToken  CancellationToken)

            => OnSetChargingPoolStatusHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SetChargingPoolStatusResponse                 (Response)

        /// <summary>
        /// An event sent whenever a SetChargingPoolStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSetChargingPoolStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a SetChargingPoolStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingPoolStatusResponse(DateTimeOffset     Timestamp,
                                                              HTTPAPI           HTTPAPI,
                                                              HTTPRequest        HTTPRequest,
                                                              HTTPResponse       HTTPResponse,
                                                              CancellationToken  CancellationToken)

            => OnSetChargingPoolStatusHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion




        #region (protected internal) GetChargingStationsRequest                    (Request)

        /// <summary>
        /// An event sent whenever a GetChargingStations request was received.
        /// </summary>
        public HTTPRequestLogEvent OnGetChargingStationsRequest = new();

        /// <summary>
        /// An event sent whenever a GetChargingStations request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task GetChargingStationsRequest(DateTimeOffset     Timestamp,
                                                           HTTPAPI            API,
                                                           HTTPRequest        Request,
                                                           CancellationToken  CancellationToken)

            => OnGetChargingStationsRequest.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   CancellationToken
               );

        #endregion

        #region (protected internal) GetChargingStationsResponse                   (Response)

        /// <summary>
        /// An event sent whenever a GetChargingStations response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnGetChargingStationsResponse = new();

        /// <summary>
        /// An event sent whenever a GetChargingStations response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task GetChargingStationsResponse(DateTimeOffset     Timestamp,
                                                            HTTPAPI            API,
                                                            HTTPRequest        Request,
                                                            HTTPResponse       Response,
                                                            CancellationToken  CancellationToken)

            => OnGetChargingStationsResponse.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   Response,
                   CancellationToken
               );

        #endregion


        #region (protected internal) SetChargingStationAdminStatusRequest          (Request)

        /// <summary>
        /// An event sent whenever a SetChargingStationAdminStatus HTTP was received.
        /// </summary>
        public HTTPRequestLogEvent OnSetChargingStationAdminStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a SetChargingStationAdminStatus HTTP was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingStationAdminStatusRequest(DateTimeOffset     Timestamp,
                                                                     HTTPAPI           HTTPAPI,
                                                                     HTTPRequest        HTTPRequest,
                                                                     CancellationToken  CancellationToken)

            => OnSetChargingStationAdminStatusHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SetChargingStationAdminStatusResponse         (Response)

        /// <summary>
        /// An event sent whenever a SetChargingStationAdminStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSetChargingStationAdminStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a SetChargingStationAdminStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingStationAdminStatusResponse(DateTimeOffset     Timestamp,
                                                                      HTTPAPI           HTTPAPI,
                                                                      HTTPRequest        HTTPRequest,
                                                                      HTTPResponse       HTTPResponse,
                                                                      CancellationToken  CancellationToken)

            => OnSetChargingStationAdminStatusHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion


        #region (protected internal) SetChargingStationStatusRequest               (Request)

        /// <summary>
        /// An event sent whenever a SetChargingStationStatus HTTP was received.
        /// </summary>
        public HTTPRequestLogEvent OnSetChargingStationStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a SetChargingStationStatus HTTP was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingStationStatusRequest(DateTimeOffset     Timestamp,
                                                                HTTPAPI           HTTPAPI,
                                                                HTTPRequest        HTTPRequest,
                                                                CancellationToken  CancellationToken)

            => OnSetChargingStationStatusHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SetChargingStationStatusResponse              (Response)

        /// <summary>
        /// An event sent whenever a SetChargingStationStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSetChargingStationStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a SetChargingStationStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetChargingStationStatusResponse(DateTimeOffset     Timestamp,
                                                                 HTTPAPI           HTTPAPI,
                                                                 HTTPRequest        HTTPRequest,
                                                                 HTTPResponse       HTTPResponse,
                                                                 CancellationToken  CancellationToken)

            => OnSetChargingStationStatusHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion



        #region (protected internal) SetEVSEAdminStatusRequest                     (Request)

        /// <summary>
        /// An event sent whenever a SetEVSEAdminStatus HTTP was received.
        /// </summary>
        public HTTPRequestLogEvent OnSetEVSEAdminStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a SetEVSEAdminStatus HTTP was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetEVSEAdminStatusRequest(DateTimeOffset     Timestamp,
                                                          HTTPAPI           HTTPAPI,
                                                          HTTPRequest        HTTPRequest,
                                                          CancellationToken  CancellationToken)

            => OnSetEVSEAdminStatusHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SetEVSEAdminStatusResponse                    (Response)

        /// <summary>
        /// An event sent whenever a SetEVSEAdminStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSetEVSEAdminStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a SetEVSEAdminStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetEVSEAdminStatusResponse(DateTimeOffset     Timestamp,
                                                           HTTPAPI           HTTPAPI,
                                                           HTTPRequest        HTTPRequest,
                                                           HTTPResponse       HTTPResponse,
                                                           CancellationToken  CancellationToken)

            => OnSetEVSEAdminStatusHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion


        #region (protected internal) SetEVSEStatusRequest                          (Request)

        /// <summary>
        /// An event sent whenever a SetEVSEStatus HTTP was received.
        /// </summary>
        public HTTPRequestLogEvent OnSetEVSEStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a SetEVSEStatus HTTP was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetEVSEStatusRequest(DateTimeOffset     Timestamp,
                                                     HTTPAPI           HTTPAPI,
                                                     HTTPRequest        HTTPRequest,
                                                     CancellationToken  CancellationToken)

            => OnSetEVSEStatusHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SetEVSEStatusResponse                         (Response)

        /// <summary>
        /// An event sent whenever a SetEVSEStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnSetEVSEStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a SetEVSEStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SetEVSEStatusResponse(DateTimeOffset     Timestamp,
                                                      HTTPAPI           HTTPAPI,
                                                      HTTPRequest        HTTPRequest,
                                                      HTTPResponse       HTTPResponse,
                                                      CancellationToken  CancellationToken)

            => OnSetEVSEStatusHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion



        #region (protected internal) SendAuthorizeHTTPRequest                      (Request)

        /// <summary>
        /// An event sent whenever an Authorize HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an Authorize HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendAuthorizeHTTPRequest(DateTimeOffset     Timestamp,
                                                         HTTPAPI           HTTPAPI,
                                                         HTTPRequest        HTTPRequest,
                                                         CancellationToken  CancellationToken)

            => OnAuthorizeHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SendAuthorizeHTTPResponse                     (Response)

        /// <summary>
        /// An event sent whenever an Authorize HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an Authorize HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendAuthorizeHTTPResponse(DateTimeOffset     Timestamp,
                                                          HTTPAPI           HTTPAPI,
                                                          HTTPRequest        HTTPRequest,
                                                          HTTPResponse       HTTPResponse,
                                                          CancellationToken  CancellationToken)

            => OnAuthorizeHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion


        #region (protected internal) SendAuthorizeStartEVSEHTTPRequest             (Request)

        /// <summary>
        /// An event sent whenever an AuthorizeStartEVSE HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeStartEVSEHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStartEVSE HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendAuthorizeStartEVSEHTTPRequest(DateTimeOffset     Timestamp,
                                                                  HTTPAPI           HTTPAPI,
                                                                  HTTPRequest        HTTPRequest,
                                                                  CancellationToken  CancellationToken)

            => OnAuthorizeStartEVSEHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SendAuthorizeStartEVSEHTTPResponse            (Response)

        /// <summary>
        /// An event sent whenever an AuthorizeStartEVSE HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeStartEVSEHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStartEVSE HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendAuthorizeStartEVSEHTTPResponse(DateTimeOffset     Timestamp,
                                                                   HTTPAPI           HTTPAPI,
                                                                   HTTPRequest        HTTPRequest,
                                                                   HTTPResponse       HTTPResponse,
                                                                   CancellationToken  CancellationToken)

            => OnAuthorizeStartEVSEHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion


        #region (protected internal) SendAuthorizeStopEVSEHTTPRequest              (Request)

        /// <summary>
        /// An event sent whenever an AuthorizeStopEVSE HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeStopEVSEHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStopEVSE HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendAuthorizeStopEVSEHTTPRequest(DateTimeOffset     Timestamp,
                                                                 HTTPAPI           HTTPAPI,
                                                                 HTTPRequest        HTTPRequest,
                                                                 CancellationToken  CancellationToken)

            => OnAuthorizeStopEVSEHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SendAuthorizeStopEVSEHTTPResponse             (Response)

        /// <summary>
        /// An event sent whenever an AuthorizeStopEVSE HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeStopEVSEHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStopEVSE HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendAuthorizeStopEVSEHTTPResponse(DateTimeOffset     Timestamp,
                                                                  HTTPAPI           HTTPAPI,
                                                                  HTTPRequest        HTTPRequest,
                                                                  HTTPResponse       HTTPResponse,
                                                                  CancellationToken  CancellationToken)

            => OnAuthorizeStopEVSEHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion



        #region (protected internal) SendRemoteStartEVSEHTTPRequest                (Request)

        /// <summary>
        /// An event sent whenever a RemoteStartEVSE HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnRemoteStartEVSEHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a RemoteStartEVSE HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendRemoteStartEVSERequest(DateTimeOffset     Timestamp,
                                                           HTTPAPI           HTTPAPI,
                                                           HTTPRequest        HTTPRequest,
                                                           CancellationToken  CancellationToken)

            => OnRemoteStartEVSEHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SendRemoteStartEVSEHTTPResponse               (Response)

        /// <summary>
        /// An event sent whenever a RemoteStartEVSE HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnRemoteStartEVSEHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a RemoteStartEVSE HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendRemoteStartEVSEResponse(DateTimeOffset     Timestamp,
                                                            HTTPAPI           HTTPAPI,
                                                            HTTPRequest        HTTPRequest,
                                                            HTTPResponse       HTTPResponse,
                                                            CancellationToken  CancellationToken)

            => OnRemoteStartEVSEHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion


        #region (protected internal) SendRemoteStopEVSEHTTPRequest                 (Request)

        /// <summary>
        /// An event sent whenever a RemoteStopEVSE HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnRemoteStopEVSEHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a RemoteStopEVSE HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendRemoteStopEVSEHTTPRequest(DateTimeOffset     Timestamp,
                                                              HTTPAPI           HTTPAPI,
                                                              HTTPRequest        HTTPRequest,
                                                              CancellationToken  CancellationToken)

            => OnRemoteStopEVSEHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SendRemoteStopEVSEHTTPResponse                (Response)

        /// <summary>
        /// An event sent whenever a RemoteStopEVSE HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnRemoteStopEVSEHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a RemoteStopEVSE HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendRemoteStopEVSEHTTPResponse(DateTimeOffset     Timestamp,
                                                               HTTPAPI           HTTPAPI,
                                                               HTTPRequest        HTTPRequest,
                                                               HTTPResponse       HTTPResponse,
                                                               CancellationToken  CancellationToken)

            => OnRemoteStopEVSEHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
                   CancellationToken
               );

        #endregion



        #region (protected internal) SendChargeDetailRecordsHTTPRequest            (Request)

        /// <summary>
        /// An event sent whenever a ChargeDetailRecords HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnChargeDetailRecordsHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a ChargeDetailRecords HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendChargeDetailRecordsHTTPRequest(DateTimeOffset     Timestamp,
                                                                   HTTPAPI           HTTPAPI,
                                                                   HTTPRequest        HTTPRequest,
                                                                   CancellationToken  CancellationToken)

            => OnChargeDetailRecordsHTTPRequest.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   CancellationToken
               );

        #endregion

        #region (protected internal) SendChargeDetailRecordsHTTPResponse           (Response)

        /// <summary>
        /// An event sent whenever a ChargeDetailRecords HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnChargeDetailRecordsHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a ChargeDetailRecords HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the HTTP request.</param>
        /// <param name="HTTPAPI">The HTTP API.</param>
        /// <param name="HTTPRequest">The HTTP request.</param>
        /// <param name="HTTPResponse">The HTTP response.</param>
        /// <param name="CancellationToken">A cancellation token to cancel the operation.</param>
        protected internal Task SendChargeDetailRecordsHTTPResponse(DateTimeOffset     Timestamp,
                                                                    HTTPAPI           HTTPAPI,
                                                                    HTTPRequest        HTTPRequest,
                                                                    HTTPResponse       HTTPResponse,
                                                                    CancellationToken  CancellationToken)

            => OnChargeDetailRecordsHTTPResponse.WhenAll(
                   Timestamp,
                   HTTPAPI,
                   HTTPRequest,
                   HTTPResponse,
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
        public readonly static HTTPMethod RESENDCDR    = HTTPMethod.TryParse("RESENDCDR",   IsSafe: false, IsIdempotent: true)!;

        #endregion

        #region Custom JSON serializers

        public CustomJObjectSerializerDelegate<ReceivedCDRInfo>?     CustomCDRReceivedInfoSerializer       { get; set; }
        public CustomJObjectSerializerDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordSerializer    { get; set; }
        public CustomJObjectSerializerDelegate<SendCDRResult>?       CustomSendCDRResultSerializer         { get; set; }
        public CustomJObjectSerializerDelegate<ChargingSession>?     CustomChargingSessionSerializer       { get; set; }

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
        public WWCP_HTTPAPI(HTTPExtAPI                    HTTPAPI,
                            //IWWCPCore                      WWCPCore,
                            //URL                            OurBaseURL,
                            //URL                            OurVersionsURL,

                            IEnumerable<HTTPHostname>?     Hostnames                   = null,
                            HTTPPath?                      RootPath                    = null,
                            IEnumerable<HTTPContentType>?  HTTPContentTypes            = null,
                            I18NString?                    Description                 = null,

                            HTTPPath?                      BasePath                    = null,  // For URL prefixes in HTML!

                            String?                        ExternalDNSName             = null,
                            String?                        HTTPServerName              = DefaultHTTPServerName,
                            String?                        HTTPServiceName             = DefaultHTTPServiceName,
                            String?                        APIVersionHash              = null,
                            JObject?                       APIVersionHashes            = null,

                            EMailAddress?                  APIRobotEMailAddress        = null,
                            String?                        APIRobotGPGPassphrase       = null,
                            ISMTPClient?                   SMTPClient                  = null,

                            HTTPPath?                      AdditionalURLPathPrefix     = null,
                            Boolean?                       AllowAnonymousReadAccesss   = null,
                            Boolean?                       LocationsAsOpenData         = null,
                            Boolean?                       TariffsAsOpenData           = null,
                            Boolean?                       AllowDowngrades             = null,

                            Boolean?                       IsDevelopment               = null,
                            IEnumerable<String>?           DevelopmentServers          = null,
                            //Boolean?                       SkipURLTemplates            = false,
                            //String?                        DatabaseFileName            = DefaultAssetsDBFileName,
                            Boolean?                       DisableNotifications        = false,

                            Boolean?                       DisableLogging              = null,
                            String?                        LoggingContext              = null,
                            String?                        LoggingPath                 = null,
                            String?                        LogfileName                 = null,
                            WWCPLogfileCreatorDelegate?    LogfileCreator              = null)

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

            //this.WWCPCores.TryAdd(HTTPHostname.Any, WWCPCore);

            this.AdditionalURLPathPrefix    = AdditionalURLPathPrefix;

            this.AllowAnonymousReadAccesss  = AllowAnonymousReadAccesss ?? true;
            this.LocationsAsOpenData        = LocationsAsOpenData       ?? true;
            this.TariffsAsOpenData          = TariffsAsOpenData         ?? true;
            this.AllowDowngrades            = AllowDowngrades           ?? false;

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
                request =>

                    Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET" ],
                            Allow                      = [ HTTPMethod.OPTIONS, HTTPMethod.HEAD, HTTPMethod.GET ],
                            AccessControlAllowHeaders  = [ "Authorization" ],
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable),

                AllowReplacement: URLReplacement.Allow

            );

            #endregion

            #region HEAD     ~/

            HTTPBaseAPI.AddHandler(

                HTTPMethod.HEAD,
                URLPathPrefix,
                HTTPContentType.Text.PLAIN,
                request =>

                    Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET" ],
                            AccessControlAllowHeaders  = [ "Authorization" ],
                            Connection                 = ConnectionType.KeepAlive,
                            Vary                       = "Accept"
                        }.AsImmutable),

                AllowReplacement: URLReplacement.Allow

            );

            HTTPBaseAPI.AddHandler(
                HTTPMethod.HEAD,
                URLPathPrefix,
                HTTPContentType.Application.JSON_UTF8,
                request =>

                    Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET" ],
                            AccessControlAllowHeaders  = [ "Authorization" ],
                            Connection                 = ConnectionType.KeepAlive,
                            Vary                       = "Accept"
                        }.AsImmutable),

                AllowReplacement: URLReplacement.Allow

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
                            AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET" ],
                            AccessControlAllowHeaders  = [ "Authorization" ],
                            ContentType                = HTTPContentType.Text.PLAIN,
                            Content                    = "This is a World Wide Charging Protocol HTTP service!".ToUTF8Bytes(),
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
                            AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET" ],
                            AccessControlAllowHeaders  = [ "Authorization" ],
                            ContentType                = HTTPContentType.Application.JSON_UTF8,
                            Content                    = JSONObject.Create(
                                                             new JProperty(
                                                                 "message",
                                                                 "This is a World Wide Charging Protocol HTTP service!"
                                                             )
                                                         ).ToUTF8Bytes(),
                            Connection                 = ConnectionType.KeepAlive,
                            Vary                       = "Accept"
                        }.AsImmutable),

                AllowReplacement: URLReplacement.Allow

            );

            #endregion


            // Be aware of multi-tenancy!

            #region ~/RNs

            #region OPTIONS     ~/RNs

            // ----------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:3004/RNs
            // ----------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.OPTIONS,
                URLPathPrefix + "RNs",
                request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.NoContent,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region HEAD        ~/RNs

            // ---------------------------------------------------------------------------
            // curl -v -X "HEAD" -H "Accept: application/json" http://127.0.0.1:3004/RNs
            // ---------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.HEAD,
                URLPathPrefix + "RNs",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                       = "1",
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs

            // -----------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs
            // -----------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion


                    var withMetadata                      = request.QueryString.GetBoolean("withMetadata", false);

                    var matchFilter                       = request.QueryString.CreateStringFilter<IRoamingNetwork>(
                                                                "match",
                                                                (roamingNetwork, pattern) => roamingNetwork.ToString()?.Contains(pattern) == true
                                                                                             //roamingNetwork.Name?.             Contains(pattern) == true ||
                                                                                             //roamingNetwork.Address.           Contains(pattern)         ||
                                                                                             //roamingNetwork.City.              Contains(pattern)         ||
                                                                                             //roamingNetwork.PostalCode.        Contains(pattern)         ||
                                                                                             //roamingNetwork.Country.ToString()?.Contains(pattern) == true         ||
                                                                                             //roamingNetwork.Directions.        Matches (pattern)         ||
                                                                                             //roamingNetwork.Operator?.   Name. Contains(pattern) == true ||
                                                                                             //roamingNetwork.SubOperator?.Name. Contains(pattern) == true ||
                                                                                             //roamingNetwork.Owner?.      Name. Contains(pattern) == true ||
                                                                                             //roamingNetwork.Facilities.        Matches (pattern)         ||
                                                                                             //roamingNetwork.EVSEUIds.          Matches (pattern)         ||
                                                                                             //roamingNetwork.EVSEIds.           Matches (pattern)         ||
                                                                                             //roamingNetwork.EVSEs.Any(evse => evse.Connectors.Any(connector => connector?.GetTariffId(emspId).ToString()?.Contains(pattern) == true))
                                                            );
                    var skip                              = request.QueryString.GetUInt64("skip");
                    var take                              = request.QueryString.GetUInt64("take");

                    var expand                            = request.QueryString.GetStrings("expand");
                    var expandRoamingNetworkIds           = expand.ContainsIgnoreCase("networks")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingStationOperatorIds  = expand.ContainsIgnoreCase("operators") ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingPoolIds             = expand.ContainsIgnoreCase("pools")     ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingStationIds          = expand.ContainsIgnoreCase("stations")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandEVSEIds                     = expand.ContainsIgnoreCase("-evses")    ? InfoStatus.ShowIdOnly : InfoStatus.Expanded;
                    var expandBrandIds                    = expand.ContainsIgnoreCase("brands")    ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandDataLicenseIds              = expand.ContainsIgnoreCase("licenses")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandEMobilityProviderIds        = expand.ContainsIgnoreCase("providers") ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;


                    var allResults                        = GetAllRoamingNetworks(request.Host);
                    var totalCount                        = allResults.ULongCount();

                    var filteredResults                   = allResults.Where(matchFilter).ToArray();
                    var filteredCount                     = filteredResults.ULongCount();

                    var jsonResults                       = filteredResults.
                                                                OrderBy(roamingNetwork => roamingNetwork.Id).
                                                                ToJSON (skip,
                                                                        take,
                                                                        Embedded:  false,

                                                                        expandChargingStationOperatorIds,
                                                                        expandRoamingNetworkIds,
                                                                        expandChargingPoolIds,
                                                                        expandChargingStationIds,
                                                                        expandEVSEIds,
                                                                        expandBrandIds,
                                                                        expandDataLicenseIds,
                                                                        expandEMobilityProviderIds,

                                                                        null,  // CustomRoamingNetworkSerializer,
                                                                        null,  // CustomChargingStationOperatorSerializer,
                                                                        null,  // CustomChargingPoolSerializer,
                                                                        null,  // CustomChargingStationSerializer,
                                                                        null); // CustomEVSESerializer

                    return Task.FromResult(
                               new HTTPResponse.Builder(request) {
                                   HTTPStatusCode                = HTTPStatusCode.OK,
                                   Server                        = DefaultHTTPServerName,
                                   Date                          = Timestamp.Now,
                                   AccessControlAllowMethods     = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                   AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                                   Content                       = withMetadata
                                                                       ? JSONObject.Create(
                                                                             new JProperty("totalCount",     totalCount),
                                                                             new JProperty("filteredCount",  filteredCount),
                                                                             new JProperty("data",           jsonResults)
                                                                         ).ToUTF8Bytes()
                                                                       : new JArray(jsonResults).ToUTF8Bytes(),
                                   ContentType                   = HTTPContentType.Application.JSON_UTF8,
                                   X_ExpectedTotalNumberOfItems  = filteredCount,
                                   Connection                    = ConnectionType.KeepAlive,
                                   Vary                          = "Accept"
                               }.AsImmutable
                           );

                });

            #endregion

            #region COUNT       ~/RNs

            // --------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:3004/RNs
            // --------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.COUNT,
                URLPathPrefix + "RNs",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion


                    var matchFilter      = request.QueryString.CreateStringFilter<IRoamingNetwork>(
                                               "match",
                                               (roamingNetwork, pattern) => roamingNetwork.ToString()?.Contains(pattern) == true
                                                                            //roamingNetwork.Name?.             Contains(pattern) == true ||
                                                                            //roamingNetwork.Address.           Contains(pattern)         ||
                                                                            //roamingNetwork.City.              Contains(pattern)         ||
                                                                            //roamingNetwork.PostalCode.        Contains(pattern)         ||
                                                                            //roamingNetwork.Country.ToString()?.Contains(pattern) == true         ||
                                                                            //roamingNetwork.Directions.        Matches (pattern)         ||
                                                                            //roamingNetwork.Operator?.   Name. Contains(pattern) == true ||
                                                                            //roamingNetwork.SubOperator?.Name. Contains(pattern) == true ||
                                                                            //roamingNetwork.Owner?.      Name. Contains(pattern) == true ||
                                                                            //roamingNetwork.Facilities.        Matches (pattern)         ||
                                                                            //roamingNetwork.EVSEUIds.          Matches (pattern)         ||
                                                                            //roamingNetwork.EVSEIds.           Matches (pattern)         ||
                                                                            //roamingNetwork.EVSEs.Any(evse => evse.Connectors.Any(connector => connector?.GetTariffId(emspId).ToString()?.Contains(pattern) == true))
                                           );

                    var allResults       = GetAllRoamingNetworks(request.Host);
                    var totalCount       = allResults.ULongCount();

                    var filteredResults  = allResults.Where(matchFilter).ToArray();
                    var filteredCount    = filteredResults.ULongCount();


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode               = HTTPStatusCode.OK,
                            Server                       = HTTPServiceName,
                            Date                         = Timestamp.Now,
                            AccessControlAllowOrigin     = "*",
                            AccessControlAllowMethods    = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders    = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                         = "1",
                            ContentType                  = HTTPContentType.Application.JSON_UTF8,
                            Content                      = JSONObject.Create(
                                                               new JProperty("totalCount",     totalCount),
                                                               new JProperty("filteredCount",  filteredCount)
                                                           ).ToUTF8Bytes(),
                            Connection                   = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion


            #region GET         ~/RNs->Id

            // -------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs->Id
            // -------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs->Id",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "GET", "HEAD" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    var allRoamingNetworks  = GetAllRoamingNetworks(request.Host);
                    var skip                = request.QueryString.GetUInt64("skip");
                    var take                = request.QueryString.GetUInt64("take");

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                 = HTTPStatusCode.OK,
                            Server                         = HTTPServiceName,
                            Date                           = Timestamp.Now,
                            AccessControlAllowOrigin       = "*",
                            AccessControlAllowMethods      = [ "GET" ],
                            AccessControlAllowHeaders      = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                           = "1",
                            ContentType                    = HTTPContentType.Application.JSON_UTF8,
                            Content                        = new JArray(
                                                                 allRoamingNetworks.
                                                                     Select(roamingNetwork => roamingNetwork.Id.ToString()).
                                                                     Skip  (request.QueryString.GetUInt64("skip")).
                                                                     Take  (request.QueryString.GetUInt64("take"))
                                                             ).ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems   = allRoamingNetworks.ULongCount(),
                            Connection                     = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs->AdminStatus

            // ------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs->AdminStatus
            // ------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs->AdminStatus",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "GET", "HEAD" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion


                    var allRoamingNetworks  = GetAllRoamingNetworks(request.Host);
                    var skip                = request.QueryString.GetUInt64("skip");
                    var take                = request.QueryString.GetUInt64("take");
                    var sinceFilter         = request.QueryString.CreateDateTimeFilter<RoamingNetworkAdminStatus>("since", (adminStatus, timestamp) => adminStatus.Timestamp >= timestamp);
                    var matchFilter         = request.QueryString.CreateStringFilter  <RoamingNetworkAdminStatus>("match", (adminStatus, pattern)   => adminStatus.Id.ToString()?.Contains(pattern) == true);

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = allRoamingNetworks.
                                                                Select(roamingNetwork => new RoamingNetworkAdminStatus(
                                                                                             roamingNetwork.Id,
                                                                                             roamingNetwork.AdminStatus
                                                                                         )).
                                                                Where (matchFilter).
                                                                Where (sinceFilter).
                                                                ToJSON(skip, take).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = allRoamingNetworks.ULongCount(),
                            Connection                    = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs->Status

            // -------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs->Status
            // -------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs->Status",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "GET", "HEAD" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion


                    var allRoamingNetworks  = GetAllRoamingNetworks(request.Host);
                    var skip                = request.QueryString.GetUInt64("skip");
                    var take                = request.QueryString.GetUInt64("take");
                    var sinceFilter         = request.QueryString.CreateDateTimeFilter<RoamingNetworkStatus>("since", (status, timestamp) => status.Timestamp >= timestamp);
                    var matchFilter         = request.QueryString.CreateStringFilter  <RoamingNetworkStatus>("match", (status, pattern)   => status.Id.ToString()?.Contains(pattern) == true);

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                 = HTTPStatusCode.OK,
                            Server                         = HTTPServiceName,
                            Date                           = Timestamp.Now,
                            AccessControlAllowOrigin       = "*",
                            AccessControlAllowMethods      = [ "GET" ],
                            AccessControlAllowHeaders      = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                           = "1",
                            ContentType                    = HTTPContentType.Application.JSON_UTF8,
                            Content                        = allRoamingNetworks.
                                                                 Select(roamingNetwork => new RoamingNetworkStatus(
                                                                                             roamingNetwork.Id,
                                                                                             roamingNetwork.Status
                                                                                         )).
                                                                 Where (matchFilter).
                                                                 Where (sinceFilter).
                                                                 ToJSON(skip, take).
                                                                 ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems   = allRoamingNetworks.ULongCount(),
                            Connection                     = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}

            #region OPTIONS     ~/RNs/{RoamingNetworkId}

            // -----------------------------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}
            // -----------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.OPTIONS,
                URLPathPrefix + "RNs/{RoamingNetworkId..}",
                request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.NoContent,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region HEAD        ~/RNs/{RoamingNetworkId}

            // --------------------------------------------------------------------------------
            // curl -v -X "HEAD" -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test
            // --------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.HEAD,
                URLPathPrefix + "RNs/{RoamingNetworkId}",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "GET", "CREATE", "DELETE" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                       = roamingNetwork.GetHashCode().ToString(),
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}

            // ----------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test
            // ----------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var expand                            = request.QueryString.GetStrings("expand");
                    var expandRoamingNetworkIds           = expand.ContainsIgnoreCase("networks")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingStationOperatorIds  = expand.ContainsIgnoreCase("operators") ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingPoolIds             = expand.ContainsIgnoreCase("pools")     ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingStationIds          = expand.ContainsIgnoreCase("stations")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandEVSEIds                     = expand.ContainsIgnoreCase("-evses")    ? InfoStatus.ShowIdOnly : InfoStatus.Expanded;
                    var expandBrandIds                    = expand.ContainsIgnoreCase("brands")    ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandDataLicenseIds              = expand.ContainsIgnoreCase("licenses")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandEMobilityProviderIds        = expand.ContainsIgnoreCase("providers") ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "GET", "CREATE", "DELETE" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                       = roamingNetwork.GetHashCode().ToString(),
                            ContentType                = HTTPContentType.Application.JSON_UTF8,
                            Content                    = roamingNetwork.ToJSON(

                                                             Embedded:  false,

                                                             expandRoamingNetworkIds,
                                                             expandChargingStationOperatorIds,
                                                             expandChargingPoolIds,
                                                             expandChargingStationIds,
                                                             expandEVSEIds,
                                                             expandBrandIds,
                                                             expandDataLicenseIds,
                                                             expandEMobilityProviderIds,

                                                             null,  // CustomRoamingNetworkSerializer,
                                                             null,  // CustomChargingStationOperatorSerializer,
                                                             null,  // CustomChargingPoolSerializer,
                                                             null,  // CustomChargingStationSerializer,
                                                             null   // CustomEVSESerializer

                                                         ).ToUTF8Bytes(),
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region CREATE      ~/RNs/{RoamingNetworkId}

            // ---------------------------------------------------------------------------------
            // curl -v -X CREATE -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test2
            // ---------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.CREATE,
                URLPathPrefix + "RNs/{RoamingNetworkId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   CreateRoamingNetworkRequest,
                HTTPResponseLogger:  CreateRoamingNetworkResponse,
                HTTPDelegate:        request => {

                    #region Try to get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var       httpUser,
                                                    out var       httpOrganizations,
                                                    out var       httpResponseBuilder,
                                                    AccessLevel:  Access_Levels.Admin,
                                                    Recursive:    true))
                    {
                        return Task.FromResult(httpResponseBuilder!.AsImmutable);
                    }

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkId(this,
                                                          out var roamingNetworkId,
                                                          out     httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    if (TryGetRoamingNetwork(roamingNetworkId,
                                             out var roamingNetwork,
                                             request.Host))
                    {

                        return Task.FromResult(new HTTPResponse.Builder(request) {
                                    HTTPStatusCode  = HTTPStatusCode.Conflict,
                                    Server          = HTTPServiceName,
                                    Date            = Timestamp.Now,
                                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                                    Content         = HTTPResponseExtensions.CreateError($"A roaming networkId with name '{roamingNetworkId}' already exists!"),
                               }.AsImmutable);

                    }

                    #endregion

                    #region Parse optional JSON

                    if (!request.TryParseJSONObjectRequestBody(out var json,
                                                               out httpResponseBuilder,
                                                               AllowEmptyHTTPBody: true))
                    {
                        return Task.FromResult(httpResponseBuilder!.AsImmutable);
                    }

                    if (!json.ParseMandatory("name",
                                             "roaming network name",
                                             HTTPServiceName,
                                             out I18NString roamingNetworkName,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    if (json.ParseOptionalJSON("description",
                                               "roaming network description",
                                               I18NString.TryParse,
                                               out I18NString? roamingNetworkDescription,
                                               out var         errorResponse))
                    {
                        if (errorResponse is not null)
                            return Task.FromResult(new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET", "CREATE", "DELETE" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);
                    }

                    #endregion


                    roamingNetwork = CreateNewRoamingNetwork(
                                         roamingNetworkId,
                                         roamingNetworkName,
                                         Description:  roamingNetworkDescription ?? I18NString.Empty,
                                         Hostname:     request.Host
                                     );


                    return Task.FromResult(
                               new HTTPResponse.Builder(request) {
                                   HTTPStatusCode             = HTTPStatusCode.Created,
                                   Server                     = HTTPServiceName,
                                   Date                       = Timestamp.Now,
                                   AccessControlAllowOrigin   = "*",
                                   AccessControlAllowMethods  = [ "GET", "CREATE", "DELETE" ],
                                   AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                   ETag                       = "1",
                                   ContentType                = HTTPContentType.Application.JSON_UTF8,
                                   Content                    = roamingNetwork.ToJSON().ToUTF8Bytes(),
                                   Connection                 = ConnectionType.KeepAlive
                               }.AsImmutable
                           );

                });

            #endregion

            #region DELETE      ~/RNs/{RoamingNetworkId}

            // ---------------------------------------------------------------------------------
            // curl -v -X DELETE -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test2
            // ---------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.DELETE,
                URLPathPrefix + "RNs/{RoamingNetworkId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   DeleteRoamingNetworkRequest,
                HTTPResponseLogger:  DeleteRoamingNetworkResponse,
                HTTPDelegate:        request => {

                    #region Try to get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var       httpUser,
                                                    out var       httpOrganizations,
                                                    out var       httpResponseBuilder,
                                                    AccessLevel:  Access_Levels.Admin,
                                                    Recursive:    true))
                    {
                        return Task.FromResult(httpResponseBuilder!.AsImmutable);
                    }

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out     httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var deletedRoamingNetwork = RemoveRoamingNetwork(
                                                    roamingNetwork.Id,
                                                    request.Host
                                                );


                    return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.OK,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET", "CREATE", "DELETE" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                ETag                       = "1",
                                ContentType                = HTTPContentType.Application.JSON_UTF8,
                                Content                    = deletedRoamingNetwork?.ToJSON().ToUTF8Bytes()
                            }.AsImmutable);

                });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/{PropertyKey}

            //// ----------------------------------------------------------------------
            //// curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test
            //// ----------------------------------------------------------------------
            //AddHandler(
            //                  HTTPMethod.GET,
            //                  URLPathPrefix + "RNs/{RoamingNetworkId}/{PropertyKey}",
            //                  HTTPContentType.Application.JSON_UTF8,
            //                  HTTPDelegate: Request => {

            //                      #region Check anonymous access

            //                      if (!AllowAnonymousReadAccesss)
            //                          return Task.FromResult(
            //                              new HTTPResponse.Builder(Request) {
            //                                  HTTPStatusCode             = HTTPStatusCode.Unauthorized,
            //                                  Server                     = HTTPTestServer?.HTTPServerName,
            //                                  Date                       = Timestamp.Now,
            //                                  AccessControlAllowOrigin   = "*",
            //                                  AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
            //                                  AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
            //                                  WWWAuthenticate            = WWWAuthenticateDefaults,
            //                                  Connection                 = ConnectionType.KeepAlive
            //                              }.AsImmutable);

            //                      #endregion

            //                      #region Check HTTP parameters

            //                      if (!Request.ParseRoamingNetwork(this,
            //                                                       out var roamingNetwork,
            //                                                       out var httpResponseBuilder) ||
            //                           roamingNetwork is null)
            //                      {
            //                          return Task.FromResult(httpResponseBuilder!.AsImmutable);
            //                      }

            //                      #endregion

            //                      if (Request.ParsedURLParameters.Length < 2)
            //                          return Task.FromResult(
            //                              new HTTPResponse.Builder(Request) {
            //                                  HTTPStatusCode  = HTTPStatusCode.BadRequest,
            //                                  Server          = HTTPTestServer?.HTTPServerName,
            //                                  Date            = Timestamp.Now,
            //                              }.AsImmutable);

            //                      var PropertyKey = Request.ParsedURLParameters[1];

            //                      if (PropertyKey.IsNullOrEmpty())
            //                          return Task.FromResult(
            //                              new HTTPResponse.Builder(Request) {
            //                                  HTTPStatusCode  = HTTPStatusCode.BadRequest,
            //                                  Server          = HTTPTestServer?.HTTPServerName,
            //                                  Date            = Timestamp.Now,
            //                                  ContentType     = HTTPContentType.Application.JSON_UTF8,
            //                                  Content         = @"{ ""description"": ""Invalid property key!"" }".ToUTF8Bytes()
            //                              }.AsImmutable);


            //                      if (!roamingNetwork.TryGetInternalData(PropertyKey, out var Value))
            //                          return Task.FromResult(
            //                              new HTTPResponse.Builder(Request) {
            //                                  HTTPStatusCode             = HTTPStatusCode.NotFound,
            //                                  Server                     = HTTPTestServer?.HTTPServerName,
            //                                  Date                       = Timestamp.Now,
            //                                  AccessControlAllowOrigin   = "*",
            //                                  AccessControlAllowMethods  = [ "GET", "SET" ],
            //                                  AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
            //                                  ETag                       = "1",
            //                                  Connection                 = ConnectionType.KeepAlive
            //                              }.AsImmutable);


            //                      return Task.FromResult(
            //                          new HTTPResponse.Builder(Request) {
            //                              HTTPStatusCode             = HTTPStatusCode.OK,
            //                              Server                     = HTTPTestServer?.HTTPServerName,
            //                              Date                       = Timestamp.Now,
            //                              AccessControlAllowOrigin   = "*",
            //                              AccessControlAllowMethods  = [ "GET", "SET" ],
            //                              AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
            //                              ETag                       = "1",
            //                              ContentType                = HTTPContentType.Application.JSON_UTF8,
            //                              Content                    = JSONObject.Create(
            //                                                               new JProperty(PropertyKey, Value)
            //                                                           ).ToUTF8Bytes(),
            //                              Connection                 = ConnectionType.KeepAlive
            //                          }.AsImmutable);

            //                  });

            #endregion

            #region SET         ~/RNs/{RoamingNetworkId}/{PropertyKey}

            //// -----------------------------------------------------------------------------
            //// curl -v -X SET -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test
            //// -----------------------------------------------------------------------------
            //AddHandler(
            //                  HTTPMethod.SET,
            //                  URLPathPrefix + "RNs/{RoamingNetworkId}/{PropertyKey}",
            //                  HTTPContentType.Application.JSON_UTF8,
            //                  HTTPDelegate: Request => {

            //                      #region Try to get HTTP user and its organizations

            //                      // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
            //                      if (!TryGetHTTPUser(Request,
            //                                          out var       httpUser,
            //                                          out var       httpOrganizations,
            //                                          out var       httpResponseBuilder,
            //                                          AccessLevel:  Access_Levels.Admin,
            //                                          Recursive:    true))
            //                      {
            //                          return Task.FromResult(httpResponseBuilder!.AsImmutable);
            //                      }

            //                      #endregion

            //                      #region Check HTTP parameters

            //                      if (!Request.ParseRoamingNetwork(this,
            //                                                       out var roamingNetwork,
            //                                                       out httpResponseBuilder) ||
            //                           roamingNetwork is null)
            //                      {
            //                          return Task.FromResult(httpResponseBuilder!.AsImmutable);
            //                      }

            //                      #endregion


            //                      if (Request.ParsedURLParameters.Length < 2)
            //                          return Task.FromResult(
            //                              new HTTPResponse.Builder(Request) {
            //                                  HTTPStatusCode  = HTTPStatusCode.BadRequest,
            //                                  Server          = HTTPTestServer?.HTTPServerName,
            //                                  Date            = Timestamp.Now,
            //                                  Connection      = ConnectionType.KeepAlive
            //                              }.AsImmutable);

            //                      var PropertyKey = Request.ParsedURLParameters[1];

            //                      if (PropertyKey.IsNullOrEmpty())
            //                          return Task.FromResult(
            //                              new HTTPResponse.Builder(Request) {
            //                                  HTTPStatusCode  = HTTPStatusCode.BadRequest,
            //                                  Server          = HTTPTestServer?.HTTPServerName,
            //                                  Date            = Timestamp.Now,
            //                                  ContentType     = HTTPContentType.Application.JSON_UTF8,
            //                                  Content         = @"{ ""description"": ""Invalid property key!"" }".ToUTF8Bytes(),
            //                                  Connection      = ConnectionType.KeepAlive
            //                              }.AsImmutable);


            //                      #region Parse optional JSON

            //                      if (Request.TryParseJSONObjectRequestBody(out var json,
            //                                                             out httpResponseBuilder,
            //                                                             AllowEmptyHTTPBody: false) ||
            //                          json is null)
            //                      {
            //                          return Task.FromResult(httpResponseBuilder!.AsImmutable);
            //                      }


            //                      #region Parse oldValue    [mandatory]

            //                      if (!json.ParseMandatoryText("oldValue",
            //                                                   "old value of the property",
            //                                                   HTTPTestServer?.HTTPServerName,
            //                                                   out String OldValue,
            //                                                   Request,
            //                                                   out httpResponseBuilder))
            //                      {
            //                          return Task.FromResult(httpResponseBuilder!.AsImmutable);
            //                      }

            //                      #endregion

            //                      #region Parse newValue    [mandatory]

            //                      if (!json.ParseMandatoryText("newValue",
            //                                                   "new value of the property",
            //                                                   HTTPTestServer?.HTTPServerName,
            //                                                   out String NewValue,
            //                                                   Request,
            //                                                   out httpResponseBuilder))
            //                      {
            //                          return Task.FromResult(httpResponseBuilder!.AsImmutable);
            //                      }

            //                      #endregion

            //                      #endregion


            //                      var result = roamingNetwork.SetInternalData(PropertyKey,
            //                                                                  NewValue,
            //                                                                  OldValue);

            //                      #region Choose HTTP status code

            //                      HTTPStatusCode _HTTPStatusCode;

            //                      switch (result)
            //                      {

            //                          case SetPropertyResult.Added:
            //                              _HTTPStatusCode = HTTPStatusCode.Created;
            //                              break;

            //                          case SetPropertyResult.Conflict:
            //                              _HTTPStatusCode = HTTPStatusCode.Conflict;
            //                              break;

            //                          default:
            //                              _HTTPStatusCode = HTTPStatusCode.OK;
            //                              break;

            //                      }

            //                      #endregion

            //                      return Task.FromResult(
            //                          new HTTPResponse.Builder(Request) {
            //                              HTTPStatusCode             = _HTTPStatusCode,
            //                              Server                     = HTTPTestServer?.HTTPServerName,
            //                              Date                       = Timestamp.Now,
            //                              AccessControlAllowOrigin   = "*",
            //                              AccessControlAllowMethods  = [ "GET", "SET" ],
            //                              AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
            //                              ETag                       = "1",
            //                              ContentType                = HTTPContentType.Application.JSON_UTF8,
            //                              Content                    = JSONObject.Create(
            //                                                               new JProperty("oldValue",  OldValue),
            //                                                               new JProperty("newValue",  NewValue)
            //                                                           ).ToUTF8Bytes(),
            //                              Connection                 = ConnectionType.KeepAlive
            //                          }.AsImmutable);

            //                  });

            #endregion

            #endregion







            // Charging Pools

            #region ~/RNs/{RoamingNetworkId}/ChargingPools

            #region OPTIONS     ~/RNs/{RoamingNetworkId}/ChargingPools

            // -----------------------------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools
            // -----------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.OPTIONS,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools",
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.NoContent,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region HEAD        ~/RNs/{RoamingNetworkId}/ChargingPools

            // ------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools
            // ------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.HEAD,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            Connection                    = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools

            // ------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools
            // ------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   GetChargingPoolsRequest,
                HTTPResponseLogger:  GetChargingPoolsResponse,
                HTTPDelegate:        request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder!.AsImmutable);
                    }

                    #endregion


                    var withMetadata            = request.QueryString.GetBoolean("withMetadata",      false);
                    var includeRemoved          = request.QueryString.GetBoolean("includeRemoved",    false);
                    var includeCustomData       = request.QueryString.GetBoolean("includeCustomData", false);

                    var matchFilter             = request.QueryString.CreateStringFilter<IChargingPool>(
                                                      "match",
                                                      (chargingPool, pattern) => chargingPool.ToString()?.Contains(pattern) == true
                                                                                 //chargingPool.Name?.             Contains(pattern) == true ||
                                                                                 //chargingPool.Address.           Contains(pattern)         ||
                                                                                 //chargingPool.City.              Contains(pattern)         ||
                                                                                 //chargingPool.PostalCode.        Contains(pattern)         ||
                                                                                 //chargingPool.Country.ToString()?.Contains(pattern) == true         ||
                                                                                 //chargingPool.Directions.        Matches (pattern)         ||
                                                                                 //chargingPool.Operator?.   Name. Contains(pattern) == true ||
                                                                                 //chargingPool.SubOperator?.Name. Contains(pattern) == true ||
                                                                                 //chargingPool.Owner?.      Name. Contains(pattern) == true ||
                                                                                 //chargingPool.Facilities.        Matches (pattern)         ||
                                                                                 //chargingPool.EVSEUIds.          Matches (pattern)         ||
                                                                                 //chargingPool.EVSEIds.           Matches (pattern)         ||
                                                                                 //chargingPool.EVSEs.Any(evse => evse.Connectors.Any(connector => connector?.GetTariffId(emspId).ToString()?.Contains(pattern) == true))
                                                  );
                    var skip                    = request.QueryString.GetUInt64 ("skip");
                    var take                    = request.QueryString.GetUInt64 ("take");

                    var expand                  = request.QueryString.GetStrings("expand");
                    var expandRoamingNetworks   = expand. ContainsIgnoreCase("networks")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandOperators         = expand. ContainsIgnoreCase("operators") ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingStations  = expand. ContainsIgnoreCase("stations")  ? InfoStatus.ShowIdOnly : InfoStatus.Expanded;
                    var expandEVSEs             = expand. ContainsIgnoreCase("evses")     ? InfoStatus.Expanded   : InfoStatus.Hidden;
                    var expandBrands            = expand. ContainsIgnoreCase("brands")    ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandDataLicenses      = expand. ContainsIgnoreCase("licenses")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;

                    var showIds                 = request.QueryString.GetStrings("showIds");
                    var showEVSEIds             = showIds.ContainsIgnoreCase("evses")     ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                    var showBrandIds            = showIds.ContainsIgnoreCase("brands")    ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                    var showDataLicenseIds      = showIds.ContainsIgnoreCase("licenses")  ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;

                    var allResults              = roamingNetwork.ChargingPools;
                    var totalCount              = allResults.ULongCount();

                    var filteredResults         = allResults.Where(matchFilter).ToArray();
                    var filteredCount           = filteredResults.ULongCount();

                    var jsonResults             = filteredResults.
                                                      OrderBy(chargingPool => chargingPool.Id).
                                                      ToJSON (skip,
                                                              take,
                                                              Embedded:                            false,
                                                              ExpandRoamingNetworkId:              expandRoamingNetworks,
                                                              ExpandChargingStationOperatorId:     expandOperators,
                                                              ExpandChargingStationIds:            expandChargingStations,
                                                              ExpandEVSEIds:                       InfoStatusExtensions.Max(showEVSEIds,        expandEVSEs),
                                                              ExpandBrandIds:                      InfoStatusExtensions.Max(showBrandIds,       expandBrands),
                                                              ExpandDataLicenses:                  InfoStatusExtensions.Max(showDataLicenseIds, expandDataLicenses),
                                                              IncludeCustomData:                   includeCustomData,
                                                              CustomChargingPoolSerializer:        null,
                                                              CustomChargingStationSerializer:     null,
                                                              CustomEVSESerializer:                null,
                                                              CustomChargingConnectorSerializer:   null);

                    return Task.FromResult(
                               new HTTPResponse.Builder(request) {
                                   HTTPStatusCode                = HTTPStatusCode.OK,
                                   Server                        = DefaultHTTPServerName,
                                   Date                          = Timestamp.Now,
                                   AccessControlAllowMethods     = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                   AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                                   Content                       = withMetadata
                                                                       ? JSONObject.Create(
                                                                             new JProperty("totalCount",     totalCount),
                                                                             new JProperty("filteredCount",  filteredCount),
                                                                             new JProperty("data",           jsonResults)
                                                                         ).ToUTF8Bytes()
                                                                       : new JArray(jsonResults).ToUTF8Bytes(),
                                   ContentType                   = HTTPContentType.Application.JSON_UTF8,
                                   X_ExpectedTotalNumberOfItems  = filteredCount,
                                   Connection                    = ConnectionType.KeepAlive,
                                   Vary                          = "Accept"
                               }.AsImmutable
                           );

                }
            );

            #endregion

            #region COUNT       ~/RNs/{RoamingNetworkId}/ChargingPools

            // -----------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/ChargingPools
            // -----------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.COUNT,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var matchFilter      = request.QueryString.CreateStringFilter<IChargingPool>(
                                               "match",
                                               (chargingPool, pattern) => chargingPool.ToString()?.Contains(pattern) == true
                                                                          //chargingPool.Name?.             Contains(pattern) == true ||
                                                                          //chargingPool.Address.           Contains(pattern)         ||
                                                                          //chargingPool.City.              Contains(pattern)         ||
                                                                          //chargingPool.PostalCode.        Contains(pattern)         ||
                                                                          //chargingPool.Country.ToString()?.Contains(pattern) == true         ||
                                                                          //chargingPool.Directions.        Matches (pattern)         ||
                                                                          //chargingPool.Operator?.   Name. Contains(pattern) == true ||
                                                                          //chargingPool.SubOperator?.Name. Contains(pattern) == true ||
                                                                          //chargingPool.Owner?.      Name. Contains(pattern) == true ||
                                                                          //chargingPool.Facilities.        Matches (pattern)         ||
                                                                          //chargingPool.EVSEUIds.          Matches (pattern)         ||
                                                                          //chargingPool.EVSEIds.           Matches (pattern)         ||
                                                                          //chargingPool.EVSEs.Any(evse => evse.Connectors.Any(connector => connector?.GetTariffId(emspId).ToString()?.Contains(pattern) == true))
                                           );

                    var allResults       = roamingNetwork.ChargingPools;
                    var totalCount       = allResults.ULongCount();

                    var filteredResults  = allResults.Where(matchFilter).ToArray();
                    var filteredCount    = filteredResults.ULongCount();


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode               = HTTPStatusCode.OK,
                            Server                       = HTTPServiceName,
                            Date                         = Timestamp.Now,
                            AccessControlAllowOrigin     = "*",
                            AccessControlAllowMethods    = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                            AccessControlAllowHeaders    = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                         = "1",
                            ContentType                  = HTTPContentType.Application.JSON_UTF8,
                            Content                      = JSONObject.Create(
                                                               new JProperty("totalCount",     totalCount),
                                                               new JProperty("filteredCount",  filteredCount)
                                                           ).ToUTF8Bytes(),
                            Connection                   = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools->Id

            // -------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools->Id
            // -------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools->Id",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = new JArray(
                                                                roamingNetwork.ChargingPools.
                                                                    Select(pool => pool.Id.ToString()).
                                                                    Skip  (request.QueryString.GetUInt64("skip")).
                                                                    Take  (request.QueryString.GetUInt64("take"))
                                                           ).ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = roamingNetwork.ChargingPools.ULongCount()
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools->AdminStatus

            // -------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools->AdminStatus
            // -------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools->AdminStatus",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    var skip         = request.QueryString.GetUInt64("skip");
                    var take         = request.QueryString.GetUInt64("take");
                    var sinceFilter  = request.QueryString.CreateDateTimeFilter<ChargingPoolAdminStatus>("since", (status, timestamp) => status.Timestamp >= timestamp);
                    var matchFilter  = request.QueryString.CreateStringFilter  <ChargingPoolAdminStatus>("match", (status, pattern)   => status.Id.ToString()?.Contains(pattern) == true);

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = roamingNetwork.ChargingPoolAdminStatus().
                                                                Where (matchFilter).
                                                                Where (sinceFilter).
                                                                ToJSON(skip, take).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = roamingNetwork.ChargingPoolAdminStatus().ULongCount()
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools->Status

            // --------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools->Status
            // --------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools->Status",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    var skip         = request.QueryString.GetUInt64("skip");
                    var take         = request.QueryString.GetUInt64("take");
                    var sinceFilter  = request.QueryString.CreateDateTimeFilter<ChargingPoolStatus>("since", (status, timestamp) => status.Timestamp >= timestamp);
                    var matchFilter  = request.QueryString.CreateStringFilter  <ChargingPoolStatus>("match", (status, pattern)   => status.Id.ToString()?.Contains(pattern) == true);

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = roamingNetwork.ChargingPoolStatus().
                                                                Where (matchFilter).
                                                                Where (sinceFilter).
                                                                ToJSON(skip, take).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = roamingNetwork.ChargingPoolStatus().ULongCount()
                        }.AsImmutable);

                });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/DynamicStatusReport

            // --------------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/ChargingPools/DynamicStatusReport
            // --------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/ChargingPools/DynamicStatusReport",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode               = HTTPStatusCode.OK,
                            Server                       = HTTPServiceName,
                            Date                         = Timestamp.Now,
                            AccessControlAllowOrigin     = "*",
                            AccessControlAllowMethods    = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders    = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                         = "1",
                            ContentType                  = HTTPContentType.Application.JSON_UTF8,
                            Content                      = JSONObject.Create(

                                                            new JProperty("count",  roamingNetwork.ChargingPools.Count()),

                                                            new JProperty("status", JSONObject.Create(
                                                                roamingNetwork.ChargingPools.GroupBy(pool => pool.Status.Value).Select(group =>
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
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingPool(this,
                                                                       out var roamingNetwork,
                                                                       out var chargingPool,
                                                                       out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "GET", "SET" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                       = "1",
                            ContentType                = HTTPContentType.Application.JSON_UTF8,
                            Content                    = chargingPool.ToJSON().ToUTF8Bytes()
                        }.AsImmutable);

                });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/.../ChargingStations
            // ---------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingPool(this,
                                                                       out var roamingNetwork,
                                                                       out var chargingPool,
                                                                       out var _HTTPResponse))
                    {
                        return Task.FromResult(_HTTPResponse.AsImmutable);
                    }

                    #endregion

                    var skip                  = request.QueryString.GetUInt64 ("skip");
                    var take                  = request.QueryString.GetUInt64 ("take");

                    var expand                = request.QueryString.GetStrings("expand");
                    var expandRoamingNetwork  = expand. ContainsIgnoreCase("network")   ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandOperators       = expand. ContainsIgnoreCase("operators") ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingPools   = expand. ContainsIgnoreCase("pools")     ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandEVSEs           = expand. ContainsIgnoreCase("-evses")    ? InfoStatus.ShowIdOnly : InfoStatus.Expanded;
                    var expandBrands          = expand. ContainsIgnoreCase("brands")    ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandDataLicenses    = expand. ContainsIgnoreCase("licenses")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;

                    var showIds               = request.QueryString.GetStrings("showIds");
                    var showEVSEIds           = showIds.ContainsIgnoreCase("evses")     ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                    var showBrandIds          = showIds.ContainsIgnoreCase("brands")    ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                    var showDataLicenseIds    = showIds.ContainsIgnoreCase("licenses")  ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount         = chargingPool.ChargingStations.ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingPool.ChargingStations.
                                                                OrderBy(station => station.Id).
                                                                ToJSON (Skip:                                skip,
                                                                        Take:                                take,
                                                                        Embedded:                            false,
                                                                        IncludeRemoved:                      false,
                                                                        ExpandRoamingNetworkId:              expandRoamingNetwork,
                                                                        ExpandChargingStationOperatorId:     expandOperators,
                                                                        ExpandChargingPoolId:                expandChargingPools,
                                                                        ExpandEVSEIds:                       expandEVSEs,
                                                                        ExpandBrandIds:                      expandBrands,
                                                                        ExpandDataLicenses:                  expandDataLicenses,
                                                                        IncludeRemovedEVSEs:                 null,
                                                                        IncludeCustomData:                   null,
                                                                        CustomChargingStationSerializer:     null,
                                                                        CustomEVSESerializer:                null,
                                                                        CustomChargingConnectorSerializer:   null).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations->AdminStatus

            // -----------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/{ChargingStationId}/ChargingStations->AdminStatus
            // -----------------------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/ChargingStations->AdminStatus",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingPool(this,
                                                                       out var roamingNetwork,
                                                                       out var chargingPool,
                                                                       out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    var skip           = request.QueryString.GetUInt64("skip");
                    var take           = request.QueryString.GetUInt64("take");
                    var historysize    = request.QueryString.GetUInt64("historysize", 1);

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount  = chargingPool.ChargingStationAdminStatus().ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingPool.ChargingStationAdminStatus().
                                                                ToJSON(skip, take).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations->Status

            // ------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/{ChargingStationId}/ChargingStations->Status
            // ------------------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/ChargingStations->Status",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingPool(this,
                                                                       out var roamingNetwork,
                                                                       out var chargingPool,
                                                                       out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    var skip           = request.QueryString.GetUInt64("skip");
                    var take           = request.QueryString.GetUInt64("take");
                    var historysize    = request.QueryString.GetUInt64("historysize", 1);

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount  = chargingPool.ChargingStationStatus().ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingPool.ChargingStationStatus().
                                                                ToJSON(skip, take).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount
                        }.AsImmutable);

                });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/.../ChargingStations
            // ---------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingPoolAndChargingStation(this,
                                                                                         out var roamingNetwork,
                                                                                         out var chargingPool,
                                                                                         out var chargingStation,
                                                                                         out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    var skip                   = request.QueryString.GetUInt64("skip");
                    var take                   = request.QueryString.GetUInt64("take");
                    var expand                 = request.QueryString.GetStrings("expand");
                    var expandRoamingNetworks  = expand.ContainsIgnoreCase("networks")  ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;
                    var expandOperators        = expand.ContainsIgnoreCase("operators") ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;
                    var expandBrands           = expand.ContainsIgnoreCase("brands")    ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount          = chargingStation.EVSEs.ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingStation.EVSEs.
                                                                OrderBy(evse => evse.Id).
                                                                ToJSON (skip,
                                                                        take,
                                                                        false,
                                                                        false,
                                                                        expandRoamingNetworks,
                                                                        expandOperators,
                                                                        expandBrands).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount
                        }.AsImmutable);

                }
            );

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs/{EVSEId}

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs/{EVSEId}

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/.../ChargingStations
            // ---------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/ChargingStations/{ChargingStationId}/EVSEs/{EVSEId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingPoolAndChargingStationAndEVSE(this,
                                                                                                out var roamingNetwork,
                                                                                                out var chargingPool,
                                                                                                out var chargingStation,
                                                                                                out var evse,
                                                                                                out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "GET" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                       = "1",
                            ContentType                = HTTPContentType.Application.JSON_UTF8,
                            Content                    = evse.ToJSON()?.ToUTF8Bytes()
                        }.AsImmutable);

                });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/.../EVSEs
            // ---------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingPool(this,
                                                                       out var roamingNetwork,
                                                                       out var chargingPool,
                                                                       out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    var skip                    = request.QueryString.GetUInt64("skip");
                    var take                    = request.QueryString.GetUInt64("take");
                    var expand                  = request.QueryString.GetStrings("expand");
                    var expandRoamingNetworks   = expand.ContainsIgnoreCase("networks")  ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;
                    var expandOperators         = expand.ContainsIgnoreCase("operators") ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;
                    var expandChargingPools     = expand.ContainsIgnoreCase("pools")     ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;
                    var expandChargingStations  = expand.ContainsIgnoreCase("stations")  ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;
                    var expandBrands            = expand.ContainsIgnoreCase("brands")    ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount           = chargingPool.EVSEs.ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingPool.EVSEs.
                                                                OrderBy(evse => evse.Id).
                                                                ToJSON (skip,
                                                                        take,
                                                                        false,
                                                                        false,
                                                                        expandRoamingNetworks,
                                                                        expandOperators,
                                                                        expandChargingPools,
                                                                        expandChargingStations,
                                                                        expandBrands).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount
                        }.AsImmutable);

                }
            );

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs->AdminStatus

            // ------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/{ChargingPoolId}/EVSEs->AdminStatus
            // ------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs->AdminStatus",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingPool(this,
                                                                       out var roamingNetwork,
                                                                       out var chargingPool,
                                                                       out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    var skip           = request.QueryString.GetUInt64("skip");
                    var take           = request.QueryString.GetUInt64("take");
                    var historysize    = request.QueryString.GetUInt64("historysize", 1);

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount  = chargingPool.EVSEAdminStatus().ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingPool.EVSEAdminStatus().
                                                                ToJSON(skip, take).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount
                        }.AsImmutable);

                }
            );

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs->Status

            // -------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingPools/{ChargingPoolId}/EVSEs->Status
            // -------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingPools/{ChargingPoolId}/EVSEs->Status",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingPool(this,
                                                                       out var roamingNetwork,
                                                                       out var chargingPool,
                                                                       out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    var skip           = request.QueryString.GetUInt64("skip");
                    var take           = request.QueryString.GetUInt64("take");
                    var historysize    = request.QueryString.GetUInt64("historysize", 1);

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount  = chargingPool.EVSEStatus().ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingPool.EVSEStatus().
                                                                ToJSON(skip, take).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount
                        }.AsImmutable);

                }
            );

            #endregion

            #endregion


            // Charging Stations

            #region ~/RNs/{RoamingNetworkId}/ChargingStations

            #region OPTIONS     ~/RNs/{RoamingNetworkId}/ChargingStations

            // --------------------------------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations
            // --------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.OPTIONS,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingStations",
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode               = HTTPStatusCode.NoContent,
                            Server                       = HTTPServiceName,
                            Date                         = Timestamp.Now,
                            AccessControlAllowOrigin     = "*",
                            AccessControlAllowMethods    = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                            AccessControlAllowHeaders    = [ "Content-Type", "Accept", "Authorization" ],
                            Connection                   = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region HEAD        ~/RNs/{RoamingNetworkId}/ChargingStations

            // --------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations
            // --------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.HEAD,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/ChargingStations",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                       = "1",
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations

            // --------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations
            // --------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/ChargingStations",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   GetChargingStationsRequest,
                HTTPResponseLogger:  GetChargingStationsResponse,
                HTTPDelegate:        request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var withMetadata            = request.QueryString.GetBoolean("withMetadata",      false);
                    var includeRemoved          = request.QueryString.GetBoolean("includeRemoved",    false);
                    var includeCustomData       = request.QueryString.GetBoolean("includeCustomData", false);

                    var matchFilter             = request.QueryString.CreateStringFilter<IChargingStation>(
                                                      "match",
                                                      (chargingStation, pattern) => chargingStation.ToString()?.Contains(pattern) == true
                                                                                    //chargingStation.Name?.             Contains(pattern) == true ||
                                                                                    //chargingStation.Address.           Contains(pattern)         ||
                                                                                    //chargingStation.City.              Contains(pattern)         ||
                                                                                    //chargingStation.PostalCode.        Contains(pattern)         ||
                                                                                    //chargingStation.Country.ToString()?.Contains(pattern) == true         ||
                                                                                    //chargingStation.Directions.        Matches (pattern)         ||
                                                                                    //chargingStation.Operator?.   Name. Contains(pattern) == true ||
                                                                                    //chargingStation.SubOperator?.Name. Contains(pattern) == true ||
                                                                                    //chargingStation.Owner?.      Name. Contains(pattern) == true ||
                                                                                    //chargingStation.Facilities.        Matches (pattern)         ||
                                                                                    //chargingStation.EVSEUIds.          Matches (pattern)         ||
                                                                                    //chargingStation.EVSEIds.           Matches (pattern)         ||
                                                                                    //chargingStation.EVSEs.Any(evse => evse.Connectors.Any(connector => connector?.GetTariffId(emspId).ToString()?.Contains(pattern) == true))
                                                  );
                    var skip                    = request.QueryString.GetUInt64 ("skip");
                    var take                    = request.QueryString.GetUInt64 ("take");

                    var expand                  = request.QueryString.GetStrings("expand");
                    var expandRoamingNetworks   = expand. ContainsIgnoreCase("networks")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandOperators         = expand. ContainsIgnoreCase("operators") ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingPools     = expand. ContainsIgnoreCase("pools")     ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingStations  = expand. ContainsIgnoreCase("stations")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandEVSEs             = expand. ContainsIgnoreCase("-evses")    ? InfoStatus.ShowIdOnly : InfoStatus.Expanded;
                    var expandBrands            = expand. ContainsIgnoreCase("brands")    ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandDataLicenses      = expand. ContainsIgnoreCase("licenses")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;

                    var showIds                 = request.QueryString.GetStrings("showIds");
                    var showEVSEIds             = showIds.ContainsIgnoreCase("evses")     ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                    var showBrandIds            = showIds.ContainsIgnoreCase("brands")    ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                    var showDataLicenseIds      = showIds.ContainsIgnoreCase("licenses")  ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;

                    var allResults              = roamingNetwork.ChargingStations;
                    var totalCount              = allResults.ULongCount();

                    var filteredResults         = allResults.Where(matchFilter).ToArray();
                    var filteredCount           = filteredResults.ULongCount();

                    var jsonResults             = filteredResults.
                                                      OrderBy(station => station.Id).
                                                      ToJSON (skip,
                                                              take,
                                                              Embedded:                            false,
                                                              IncludeRemoved:                      includeRemoved,
                                                              ExpandRoamingNetworkId:              expandRoamingNetworks,
                                                              ExpandChargingStationOperatorId:     expandOperators,
                                                              ExpandChargingPoolId:                expandChargingPools,
                                                              ExpandEVSEIds:                       expandEVSEs,
                                                              ExpandBrandIds:                      expandBrands,
                                                              ExpandDataLicenses:                  expandDataLicenses,
                                                              CustomChargingStationSerializer:     null,
                                                              CustomEVSESerializer:                null,
                                                              CustomChargingConnectorSerializer:   null);

                    return Task.FromResult(
                               new HTTPResponse.Builder(request) {
                                   HTTPStatusCode                = HTTPStatusCode.OK,
                                   Server                        = DefaultHTTPServerName,
                                   Date                          = Timestamp.Now,
                                   AccessControlAllowMethods     = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                   AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                                   Content                       = withMetadata
                                                                       ? JSONObject.Create(
                                                                             new JProperty("totalCount",     totalCount),
                                                                             new JProperty("filteredCount",  filteredCount),
                                                                             new JProperty("data",           jsonResults)
                                                                         ).ToUTF8Bytes()
                                                                       : new JArray(jsonResults).ToUTF8Bytes(),
                                   ContentType                   = HTTPContentType.Application.JSON_UTF8,
                                   X_ExpectedTotalNumberOfItems  = filteredCount,
                                   Connection                    = ConnectionType.KeepAlive,
                                   Vary                          = "Accept"
                               }.AsImmutable
                           );

                });

            #endregion

            #region COUNT       ~/RNs/{RoamingNetworkId}/ChargingStations

            // --------------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/ChargingStations
            // --------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.COUNT,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/ChargingStations",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var matchFilter             = request.QueryString.CreateStringFilter<IChargingStation>(
                                                      "match",
                                                      (chargingStation, pattern) => chargingStation.ToString()?.Contains(pattern) == true
                                                                                    //chargingStation.Name?.             Contains(pattern) == true ||
                                                                                    //chargingStation.Address.           Contains(pattern)         ||
                                                                                    //chargingStation.City.              Contains(pattern)         ||
                                                                                    //chargingStation.PostalCode.        Contains(pattern)         ||
                                                                                    //chargingStation.Country.ToString()?.Contains(pattern) == true         ||
                                                                                    //chargingStation.Directions.        Matches (pattern)         ||
                                                                                    //chargingStation.Operator?.   Name. Contains(pattern) == true ||
                                                                                    //chargingStation.SubOperator?.Name. Contains(pattern) == true ||
                                                                                    //chargingStation.Owner?.      Name. Contains(pattern) == true ||
                                                                                    //chargingStation.Facilities.        Matches (pattern)         ||
                                                                                    //chargingStation.EVSEUIds.          Matches (pattern)         ||
                                                                                    //chargingStation.EVSEIds.           Matches (pattern)         ||
                                                                                    //chargingStation.EVSEs.Any(evse => evse.Connectors.Any(connector => connector?.GetTariffId(emspId).ToString()?.Contains(pattern) == true))
                                                  );

                    var allResults              = roamingNetwork.ChargingStations;
                    var totalCount              = allResults.ULongCount();

                    var filteredResults         = allResults.Where(matchFilter).ToArray();
                    var filteredCount           = filteredResults.ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode               = HTTPStatusCode.OK,
                            Server                       = HTTPServiceName,
                            Date                         = Timestamp.Now,
                            AccessControlAllowOrigin     = "*",
                            AccessControlAllowMethods    = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                            AccessControlAllowHeaders    = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                         = "1",
                            ContentType                  = HTTPContentType.Application.JSON_UTF8,
                            Content                      = JSONObject.Create(
                                                               new JProperty("totalCount",     totalCount),
                                                               new JProperty("filteredCount",  filteredCount)
                                                           ).ToUTF8Bytes(),
                            Connection                   = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations->Id

            // -------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations->Id
            // -------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingStations->Id",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = new JArray(
                                                                roamingNetwork.ChargingStations.
                                                                    Select(station => station.Id.ToString()).
                                                                    Skip  (request.QueryString.GetUInt64("skip")).
                                                                    Take  (request.QueryString.GetUInt64("take"))
                                                            ).ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = roamingNetwork.ChargingStations.ULongCount()
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations->AdminStatus

            // ----------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations->AdminStatus
            // ----------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingStations->AdminStatus",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    var skip         = request.QueryString.GetUInt64("skip");
                    var take         = request.QueryString.GetUInt64("take");
                    var sinceFilter  = request.QueryString.CreateDateTimeFilter<ChargingStationAdminStatus>("since", (status, timestamp) => status.Timestamp >= timestamp);
                    var matchFilter  = request.QueryString.CreateStringFilter  <ChargingStationAdminStatus>("match", (status, pattern)   => status.Id.ToString()?.Contains(pattern) == true);

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                 = HTTPStatusCode.OK,
                            Server                         = HTTPServiceName,
                            Date                           = Timestamp.Now,
                            AccessControlAllowOrigin       = "*",
                            AccessControlAllowMethods      = [ "GET" ],
                            AccessControlAllowHeaders      = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                           = "1",
                            ContentType                    = HTTPContentType.Application.JSON_UTF8,
                            Content                        = roamingNetwork.ChargingStationAdminStatus().
                                                                 Where (matchFilter).
                                                                 Where (sinceFilter).
                                                                 ToJSON(skip, take).
                                                                 ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems   = roamingNetwork.ChargingStationAdminStatus().ULongCount()
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations->Status

            // -----------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations->Status
            // -----------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingStations->Status",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    var skip         = request.QueryString.GetUInt64                         ("skip");
                    var take         = request.QueryString.GetUInt64                         ("take");
                    var sinceFilter  = request.QueryString.CreateDateTimeFilter<ChargingStationStatus>("since", (status, timestamp) => status.Timestamp >= timestamp);
                    var matchFilter  = request.QueryString.CreateStringFilter  <ChargingStationStatus>("match", (status, pattern)   => status.Id.ToString()?.Contains(pattern) == true);

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                 = HTTPStatusCode.OK,
                            Server                         = HTTPServiceName,
                            Date                           = Timestamp.Now,
                            AccessControlAllowOrigin       = "*",
                            AccessControlAllowMethods      = [ "GET" ],
                            AccessControlAllowHeaders      = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                           = "1",
                            ContentType                    = HTTPContentType.Application.JSON_UTF8,
                            Content                        = roamingNetwork.ChargingStationStatus().
                                                                 Where (matchFilter).
                                                                 Where (sinceFilter).
                                                                 ToJSON(skip, take).
                                                                 ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems   = roamingNetwork.ChargingStationStatus().ULongCount()
                        }.AsImmutable);

                });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations/DynamicStatusReport

            // --------------------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/ChargingStations/DynamicStatusReport
            // --------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/ChargingStations/DynamicStatusReport",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode               = HTTPStatusCode.OK,
                            Server                       = HTTPServiceName,
                            Date                         = Timestamp.Now,
                            AccessControlAllowOrigin     = "*",
                            AccessControlAllowMethods    = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders    = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                         = "1",
                            ContentType                  = HTTPContentType.Application.JSON_UTF8,
                            Content                      = JSONObject.Create(

                                                               new JProperty("count",  roamingNetwork.ChargingStations.Count()),

                                                               new JProperty("status", JSONObject.Create(
                                                                   roamingNetwork.ChargingStations.GroupBy(station => station.Status.Value).Select(group =>
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
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingStation(this,
                                                                          out var roamingNetwork,
                                                                          out var chargingStation,
                                                                          out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "GET", "SET" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                       = "1",
                            ContentType                = HTTPContentType.Application.JSON_UTF8,
                            Content                    = chargingStation.ToJSON().ToUTF8Bytes()
                        }.AsImmutable);

                }
            );

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/.../EVSEs
            // ---------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingStation(this,
                                                                          out var roamingNetwork,
                                                                          out var chargingStation,
                                                                          out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    var skip                    = request.QueryString.GetUInt64("skip");
                    var take                    = request.QueryString.GetUInt64("take");
                    var expand                  = request.QueryString.GetStrings("expand");
                    var expandRoamingNetworks   = expand.ContainsIgnoreCase("networks")  ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;
                    var expandOperators         = expand.ContainsIgnoreCase("operators") ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;
                    var expandChargingPools     = expand.ContainsIgnoreCase("pools")     ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;
                    var expandChargingStations  = expand.ContainsIgnoreCase("stations")  ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;
                    var expandBrands            = expand.ContainsIgnoreCase("brands")    ? InfoStatus.Expanded : InfoStatus.ShowIdOnly;


                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount           = chargingStation.EVSEs.ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingStation.EVSEs.
                                                                OrderBy(evse => evse.Id).
                                                                ToJSON (skip,
                                                                        take,
                                                                        false,
                                                                        false, // IncludeRemovedEVSEs
                                                                        expandRoamingNetworks,
                                                                        expandOperators,
                                                                        expandChargingPools,
                                                                        expandChargingStations,
                                                                        expandBrands).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount
                        }.AsImmutable);

                }
            );

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs->AdminStatus

            // ------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/{ChargingStationId}/EVSEs->AdminStatus
            // ------------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs->AdminStatus",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingStation(this,
                                                                          out var roamingNetwork,
                                                                          out var chargingStation,
                                                                          out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    var skip           = request.QueryString.GetUInt64("skip");
                    var take           = request.QueryString.GetUInt64("take");
                    var historysize    = request.QueryString.GetUInt64("historysize", 1);

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount  = chargingStation.EVSEAdminStatus().ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingStation.EVSEAdminStatus().
                                                                ToJSON(skip, take).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount
                        }.AsImmutable);

                }
            );

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs->Status

            // -------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingStations/{ChargingStationId}/EVSEs->Status
            // -------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingStations/{ChargingStationId}/EVSEs->Status",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetworkAndChargingStation(this,
                                                                          out var roamingNetwork,
                                                                          out var chargingStation,
                                                                          out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion

                    var skip           = request.QueryString.GetUInt64("skip");
                    var take           = request.QueryString.GetUInt64("take");
                    var historysize    = request.QueryString.GetUInt64("historysize", 1);

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount  = chargingStation.EVSEStatus().ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingStation.EVSEStatus().
                                                                ToJSON(skip, take).
                                                                ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount
                        }.AsImmutable);

                }
            );

            #endregion

            #endregion


            // EVSEs

            #region ~/RNs/{RoamingNetworkId}/EVSEs

            #region OPTIONS     ~/RNs/{RoamingNetworkId}/EVSEs

            // ---------------------------------------------------------------------------------------
            // curl -v -X OPTIONS -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs
            // ---------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.OPTIONS,
                URLPathPrefix + "RNs/{RoamingNetworkId}/EVSEs",
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.NoContent,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region HEAD        ~/RNs/{RoamingNetworkId}/EVSEs

            // ----------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs
            // ----------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.HEAD,
                URLPathPrefix + "RNs/{RoamingNetworkId}/EVSEs",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode             = HTTPStatusCode.OK,
                            Server                     = HTTPServiceName,
                            Date                       = Timestamp.Now,
                            AccessControlAllowOrigin   = "*",
                            AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                            AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                       = "1",
                            Connection                 = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs

            // ----------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs
            // ----------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/EVSEs",
                HTTPContentType.Application.JSON_UTF8,
                //HTTPRequestLogger:   GetEVSEsRequest,
                //HTTPResponseLogger:  GetEVSEsResponse,
                HTTPDelegate:        request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var withMetadata            = request.QueryString.GetBoolean("withMetadata",      false);
                    var includeRemoved          = request.QueryString.GetBoolean("includeRemoved",    false);
                    var includeCustomData       = request.QueryString.GetBoolean("includeCustomData", false);

                    var matchFilter             = request.QueryString.CreateStringFilter<IEVSE>(
                                                      "match",
                                                      (evse, pattern) => evse.ToString()?.Contains(pattern) == true
                                                                         //evse.Name?.             Contains(pattern) == true ||
                                                                         //evse.Address.           Contains(pattern)         ||
                                                                         //evse.City.              Contains(pattern)         ||
                                                                         //evse.PostalCode.        Contains(pattern)         ||
                                                                         //evse.Country.ToString()?.Contains(pattern) == true         ||
                                                                         //evse.Directions.        Matches (pattern)         ||
                                                                         //evse.Operator?.   Name. Contains(pattern) == true ||
                                                                         //evse.SubOperator?.Name. Contains(pattern) == true ||
                                                                         //evse.Owner?.      Name. Contains(pattern) == true ||
                                                                         //evse.Facilities.        Matches (pattern)         ||
                                                                         //evse.EVSEUIds.          Matches (pattern)         ||
                                                                         //evse.EVSEIds.           Matches (pattern)         ||
                                                                         //evse.EVSEs.Any(evse => evse.Connectors.Any(connector => connector?.GetTariffId(emspId).ToString()?.Contains(pattern) == true))
                                                  );
                    var skip                    = request.QueryString.GetUInt64 ("skip");
                    var take                    = request.QueryString.GetUInt64 ("take");

                    var expand                  = request.QueryString.GetStrings("expand");
                    var expandRoamingNetwork    = expand. ContainsIgnoreCase("network")   ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandOperator          = expand. ContainsIgnoreCase("operator")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingPool      = expand. ContainsIgnoreCase("pool")      ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandChargingStation   = expand. ContainsIgnoreCase("station")   ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandBrands            = expand. ContainsIgnoreCase("brands")    ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;
                    var expandDataLicenses      = expand. ContainsIgnoreCase("licenses")  ? InfoStatus.Expanded   : InfoStatus.ShowIdOnly;

                    var showIds                 = request.QueryString.GetStrings("showIds");
                    var showBrandIds            = showIds.ContainsIgnoreCase("brands")    ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;
                    var showDataLicenseIds      = showIds.ContainsIgnoreCase("licenses")  ? InfoStatus.ShowIdOnly : InfoStatus.Hidden;


                    var allResults              = roamingNetwork.EVSEs;
                    var totalCount              = allResults.ULongCount();

                    var filteredResults         = allResults.Where(matchFilter).ToArray();
                    var filteredCount           = filteredResults.ULongCount();

                    var jsonResults             = filteredResults.
                                                      OrderBy(evse => evse.Id).
                                                      ToJSON (skip,
                                                              take,
                                                              Embedded:                         false,
                                                              IncludeRemoved:                   includeRemoved,
                                                              ExpandRoamingNetworkId:           expandRoamingNetwork,
                                                              ExpandChargingStationOperatorId:  expandOperator,
                                                              ExpandChargingPoolId:             expandChargingPool,
                                                              ExpandChargingStationId:          expandChargingStation,
                                                              ExpandBrandIds:                   expandBrands,
                                                              ExpandDataLicenses:               expandDataLicenses,
                                                              IncludeCustomData:                includeCustomData);

                    return Task.FromResult(
                               new HTTPResponse.Builder(request) {
                                   HTTPStatusCode                = HTTPStatusCode.OK,
                                   Server                        = DefaultHTTPServerName,
                                   Date                          = Timestamp.Now,
                                   AccessControlAllowMethods     = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                   AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                                   Content                       = withMetadata
                                                                       ? JSONObject.Create(
                                                                             new JProperty("totalCount",     totalCount),
                                                                             new JProperty("filteredCount",  filteredCount),
                                                                             new JProperty("data",           jsonResults)
                                                                         ).ToUTF8Bytes()
                                                                       : new JArray(jsonResults).ToUTF8Bytes(),
                                   ContentType                   = HTTPContentType.Application.JSON_UTF8,
                                   X_ExpectedTotalNumberOfItems  = filteredCount,
                                   Connection                    = ConnectionType.KeepAlive,
                                   Vary                          = "Accept"
                               }.AsImmutable
                           );

                });

            #endregion

            #region COUNT       ~/RNs/{RoamingNetworkId}/EVSEs

            // ---------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/EVSEs
            // ---------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.COUNT,
                URLPathPrefix + "RNs/{RoamingNetworkId}/EVSEs",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var matchFilter      = request.QueryString.CreateStringFilter<IEVSE>(
                                               "match",
                                               (evse, pattern) => evse.ToString()?.Contains(pattern) == true
                                                                  //evse.Name?.             Contains(pattern) == true ||
                                                                  //evse.Address.           Contains(pattern)         ||
                                                                  //evse.City.              Contains(pattern)         ||
                                                                  //evse.PostalCode.        Contains(pattern)         ||
                                                                  //evse.Country.ToString()?.Contains(pattern) == true         ||
                                                                  //evse.Directions.        Matches (pattern)         ||
                                                                  //evse.Operator?.   Name. Contains(pattern) == true ||
                                                                  //evse.SubOperator?.Name. Contains(pattern) == true ||
                                                                  //evse.Owner?.      Name. Contains(pattern) == true ||
                                                                  //evse.Facilities.        Matches (pattern)         ||
                                                                  //evse.EVSEUIds.          Matches (pattern)         ||
                                                                  //evse.EVSEIds.           Matches (pattern)         ||
                                                                  //evse.EVSEs.Any(evse => evse.Connectors.Any(connector => connector?.GetTariffId(emspId).ToString()?.Contains(pattern) == true))
                                           );

                    var allResults       = roamingNetwork.EVSEs;
                    var totalCount       = allResults.ULongCount();

                    var matchingResults  = allResults.Where(matchFilter).ToArray();
                    var matchingCount    = matchingResults.ULongCount();

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode               = HTTPStatusCode.OK,
                            Server                       = HTTPServiceName,
                            Date                         = Timestamp.Now,
                            AccessControlAllowOrigin     = "*",
                            AccessControlAllowMethods    = [ "OPTIONS", "HEAD", "GET", "COUNT" ],
                            AccessControlAllowHeaders    = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                         = "1",
                            ContentType                  = HTTPContentType.Application.JSON_UTF8,
                            Content                      = JSONObject.Create(
                                                               new JProperty("totalCount",     totalCount),
                                                               new JProperty("matchingCount",  matchingCount)
                                                           ).ToUTF8Bytes(),
                            Connection                   = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs->AdminStatus

            // -----------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs->AdminStatus
            // -----------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/EVSEs->AdminStatus",
                HTTPContentType.Application.JSON_UTF8,
                //HTTPRequestLogger:  SendGetEVSEsAdminStatusRequest,
                //HTTPResponseLogger: SendGetEVSEsAdminStatusResponse,
                HTTPDelegate:       request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "GET", "HEAD", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var skip                      = request.QueryString.GetUInt64                           ("skip");
                    var take                      = request.QueryString.GetUInt64                           ("take");
                    var afterFilter               = request.QueryString.CreateDateTimeFilter<DateTimeOffset>("after",  (timestamp, pattern) => timestamp >= pattern);
                    var beforeFilter              = request.QueryString.CreateDateTimeFilter<DateTimeOffset>("before", (timestamp, pattern) => timestamp <= pattern);
                    var matchFilter               = request.QueryString.CreateStringFilter  <EVSE_Id>       ("match",  (evseId,    pattern) => evseId.ToString().Contains(pattern));

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount             = roamingNetwork.EVSEAdminStatus().ULongCount();

                    var evseAdminStatusSchedules  = roamingNetwork.EVSEAdminStatusSchedule(
                                                        IncludeEVSEs:     evse      => matchFilter (evse.Id),
                                                        TimestampFilter:  timestamp => beforeFilter(timestamp) &&
                                                                                       afterFilter (timestamp),
                                                        Take:             1
                                                    );

                    #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

                    var filteredAdminStatus = new Dictionary<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusType>>>();

                    foreach (var status in evseAdminStatusSchedules)
                    {

                        if (!filteredAdminStatus.TryGetValue(status.Item1, out IEnumerable<Timestamped<EVSEAdminStatusType>>? value))
                            filteredAdminStatus.Add(status.Item1, status.Item2);

                        else if (value.Any() && value.First().Timestamp >= status.Item2.First().Timestamp)
                            filteredAdminStatus[status.Item1] = status.Item2;

                    }

                    #endregion


                    var json = new JObject(
                                   filteredAdminStatus.
                                       OrderBy       (status => status.Key).
                                       SkipTakeFilter(skip, take).
                                       Select        (kvp    => new JProperty(
                                                                    kvp.Key.ToString(), // EVSEId
                                                                    kvp.Value.          // Filter multiple status having the exact same ISO 8601 timestamp!
                                                                        GroupBy(status => status.Timestamp.ToISO8601()).
                                                                        Select (group  => group.First()).
                                                                        Select (status => new JArray(
                                                                                              status.Timestamp.ToISO8601(),
                                                                                              status.Value.    ToString())
                                                                                          ).First()
                                                                ))
                               );


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                 = HTTPStatusCode.OK,
                            Server                         = HTTPServiceName,
                            Date                           = Timestamp.Now,
                            AccessControlAllowOrigin       = "*",
                            AccessControlAllowMethods      = [ "GET" ],
                            AccessControlAllowHeaders      = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                           = "1",
                            ContentType                    = HTTPContentType.Application.JSON_UTF8,
                            Content                        = json.ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems   = expectedCount,
                            Connection                     = ConnectionType.KeepAlive
                        }.AsImmutable);

                    }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs->GroupedAdminStatus

            // ----------------------------------------------------------------------------------------------------------------
            // curl -v -X GET -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/EVSEs->GroupedAdminStatus
            // ----------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs->GroupedAdminStatus",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "GET", "HEAD", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion


                    var afterFilter            = request.QueryString.CreateDateTimeFilter<DateTimeOffset>("after",  (timestamp, pattern) => timestamp >= pattern);
                    var beforeFilter           = request.QueryString.CreateDateTimeFilter<DateTimeOffset>("before", (timestamp, pattern) => timestamp <= pattern);
                    var matchFilter            = request.QueryString.CreateStringFilter  <EVSE_Id>       ("match",  (evseId,    pattern) => evseId.ToString().Contains(pattern));

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount          = roamingNetwork.EVSEAdminStatus().ULongCount();

                    var matchingEVSEs          = (matchFilter is not null
                                                     ? roamingNetwork.EVSEs.Where(evse => matchFilter(evse.Id))
                                                     : roamingNetwork.EVSEs).ToArray();

                    var evseCount              = matchingEVSEs.Length;
                    var evseAdminStatusGroups  = matchingEVSEs.GroupBy(evse => evse.AdminStatus.Value).ToArray();


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode               = HTTPStatusCode.OK,
                            Server                       = HTTPServiceName,
                            Date                         = Timestamp.Now,
                            AccessControlAllowOrigin     = "*",
                            AccessControlAllowMethods    = [ "GET", "OPTIONS" ],
                            AccessControlAllowHeaders    = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                         = "1",
                            ContentType                  = HTTPContentType.Application.JSON_UTF8,
                            Content                      = JSONObject.Create(

                                                               new JProperty("count",   evseCount),

                                                               new JProperty("status",  JSONObject.Create(
                                                                   evseAdminStatusGroups.Select(evseAdminStatusGroup => new JProperty(
                                                                                                    evseAdminStatusGroup.Key.ToString().ToLower(),
                                                                                                    new JArray(
                                                                                                        evseAdminStatusGroup.
                                                                                                            Select(evse => evse.Id.ToString()).
                                                                                                            Order ()
                                                                                                    )
                                                                                                ))
                                                               ))

                                                           ).ToUTF8Bytes(),
                            Connection                   = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs->Status

            // -----------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs->Status
            // -----------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/EVSEs->Status",
                HTTPContentType.Application.JSON_UTF8,
                //HTTPRequestLogger:  SendGetEVSEsStatusRequest,
                //HTTPResponseLogger: SendGetEVSEsStatusResponse,
                HTTPDelegate:       request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "GET", "HEAD", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check HTTP parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var skip                 = request.QueryString.GetUInt64                           ("skip");
                    var take                 = request.QueryString.GetUInt64                           ("take");
                    var afterFilter          = request.QueryString.CreateDateTimeFilter<DateTimeOffset>("after",  (timestamp, pattern) => timestamp >= pattern);
                    var beforeFilter         = request.QueryString.CreateDateTimeFilter<DateTimeOffset>("before", (timestamp, pattern) => timestamp <= pattern);
                    var matchFilter          = request.QueryString.CreateStringFilter  <EVSE_Id>       ("match",  (evseId,    pattern) => evseId.ToString().Contains(pattern));

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount        = roamingNetwork.EVSEStatus().ULongCount();

                    var evseStatusSchedules  = roamingNetwork.EVSEStatusSchedule(
                                                   IncludeEVSEs:     evse      => matchFilter (evse.Id),
                                                   TimestampFilter:  timestamp => beforeFilter(timestamp) &&
                                                                                  afterFilter (timestamp),
                                                   Take:             1
                                               );

                    #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

                    var filteredStatus = new Dictionary<EVSE_Id, IEnumerable<Timestamped<EVSEStatusType>>>();

                    foreach (var status in evseStatusSchedules)
                    {

                        if (!filteredStatus.TryGetValue(status.Item1, out IEnumerable<Timestamped<EVSEStatusType>>? value))
                            filteredStatus.Add(status.Item1, status.Item2);

                        else if (value.Any() && value.First().Timestamp >= status.Item2.First().Timestamp)
                            filteredStatus[status.Item1] = status.Item2;

                    }

                    #endregion


                    var json = new JObject(
                                   filteredStatus.
                                       OrderBy       (status => status.Key).
                                       SkipTakeFilter(skip, take).
                                       Select        (kvp    => new JProperty(
                                                                    kvp.Key.ToString(), // EVSEId
                                                                    kvp.Value.          // Filter multiple status having the exact same ISO 8601 timestamp!
                                                                        GroupBy(status => status.Timestamp.ToISO8601()).
                                                                        Select (group  => group.First()).
                                                                        Select (status => new JArray(
                                                                                              status.Timestamp.ToISO8601(),
                                                                                              status.Value.    ToString())
                                                                                          ).First()
                                                                ))
                               );


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                 = HTTPStatusCode.OK,
                            Server                         = HTTPServiceName,
                            Date                           = Timestamp.Now,
                            AccessControlAllowOrigin       = "*",
                            AccessControlAllowMethods      = [ "GET" ],
                            AccessControlAllowHeaders      = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                           = "1",
                            ContentType                    = HTTPContentType.Application.JSON_UTF8,
                            Content                        = json.ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems   = expectedCount,
                            Connection                     = ConnectionType.KeepAlive
                        }.AsImmutable);

                    }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs->GroupedStatus

            // ----------------------------------------------------------------------------------------------------------------
            // curl -v -X GET -H "Accept: application/json" http://127.0.0.1:5500/RNs/{RoamingNetworkId}/EVSEs->GroupedStatus
            // ----------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs->GroupedStatus",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check anonymous access

                    if (!AllowAnonymousReadAccesss)
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "GET", "HEAD", "COUNT" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                WWWAuthenticate            = WWWAuthenticateDefaults,
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable);

                    #endregion

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out var httpResponse))
                    {
                        return Task.FromResult(httpResponse.AsImmutable);
                    }

                    #endregion


                    var afterFilter       = request.QueryString.CreateDateTimeFilter<DateTimeOffset>("after",  (timestamp, pattern) => timestamp >= pattern);
                    var beforeFilter      = request.QueryString.CreateDateTimeFilter<DateTimeOffset>("before", (timestamp, pattern) => timestamp <= pattern);
                    var matchFilter       = request.QueryString.CreateStringFilter  <EVSE_Id>       ("match",  (evseId,    pattern) => evseId.ToString().Contains(pattern));

                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount     = roamingNetwork.EVSEStatus().ULongCount();

                    var matchingEVSEs     = (matchFilter is not null
                                                ? roamingNetwork.EVSEs.Where(evse => matchFilter(evse.Id))
                                                : roamingNetwork.EVSEs).ToArray();

                    var evseCount         = matchingEVSEs.Length;
                    var evseStatusGroups  = matchingEVSEs.GroupBy(evse => evse.Status.Value).ToArray();


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode               = HTTPStatusCode.OK,
                            Server                       = HTTPServiceName,
                            Date                         = Timestamp.Now,
                            AccessControlAllowOrigin     = "*",
                            AccessControlAllowMethods    = [ "GET", "OPTIONS" ],
                            AccessControlAllowHeaders    = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                         = "1",
                            ContentType                  = HTTPContentType.Application.JSON_UTF8,
                            Content                      = JSONObject.Create(

                                                               new JProperty("count",   evseCount),

                                                               new JProperty("status",  JSONObject.Create(
                                                                   evseStatusGroups.Select(evseStatusGroup => new JProperty(
                                                                                               evseStatusGroup.Key.ToString().ToLower(),
                                                                                               new JArray(
                                                                                                   evseStatusGroup.
                                                                                                       Select(evse => evse.Id.ToString()).
                                                                                                       Order ()
                                                                                               )
                                                                                           ))
                                                               ))

                                                           ).ToUTF8Bytes(),
                            Connection                   = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            #region OPTIONS     ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // --------------------------------------------------------------------------
            // curl -v -X OPTIONS http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            // --------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.OPTIONS,
                URLPathPrefix + "RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                HTTPDelegate: Request => {

                    #region Check RoamingNetworkId and EVSEId URI parameters

                    if (!Request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(Request) {
                            HTTPStatusCode              = HTTPStatusCode.NoContent,
                            Server                      = HTTPServiceName,
                            Date                        = Timestamp.Now,
                            AccessControlAllowOrigin    = "*",
                            AccessControlAllowMethods   = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR", "OPTIONS" ],
                            AccessControlAllowHeaders   = [ "Content-Type", "Accept", "Authorization" ],
                        }.AsImmutable);

                },
                AllowReplacement: URLReplacement.Allow
            );

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            // ---------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Check RoamingNetworkId and EVSEId URI parameters

                    if (!request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode              = HTTPStatusCode.OK,
                            Server                      = HTTPServiceName,
                            Date                        = Timestamp.Now,
                            AccessControlAllowOrigin    = "*",
                            AccessControlAllowMethods   = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                            AccessControlAllowHeaders   = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                        = "1",
                            ContentType                 = HTTPContentType.Application.JSON_UTF8,
                            Content                     = (evse?.ToJSON() ?? []).ToUTF8Bytes(),
                            Connection                  = ConnectionType.KeepAlive
                        }.AsImmutable);

                }
            );

            #endregion


            #region AUTHSTART   ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // -----------------------------------------------------------------------
            // curl -v -X AUTHSTART -H "Content-Type: application/json" \
            //                      -H "Accept:       application/json" \
            //      -d "{ \"AuthToken\":  \"00112233\" }" \
            //      http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            HTTPBaseAPI.AddHandler(

                AUTHSTART,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   SendAuthorizeStartEVSEHTTPRequest,
                HTTPResponseLogger:  SendAuthorizeStartEVSEHTTPResponse,
                HTTPDelegate:        async request => {

                    #region Parse RoamingNetworkId and EVSEId URI parameters

                    if (!request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse JSON

                    if (!request.TryParseJSONObjectRequestBody(out var json,
                                                               out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #region Parse OperatorId             [optional]

                    if (!json.ParseOptional("OperatorId",
                                            "Charging Station Operator identification",
                                            HTTPServiceName,
                                            ChargingStationOperator_Id.TryParse,
                                            out ChargingStationOperator_Id operatorId,
                                            request,
                                            out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse AuthToken              [mandatory]

                    if (!json.ParseMandatory("AuthToken",
                                             "authentication token",
                                             HTTPServiceName,
                                             AuthenticationToken.TryParse,
                                             out AuthenticationToken authToken,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse SessionId              [optional]

                    if (!json.ParseOptionalStruct2("SessionId",
                                                   "Charging session identification",
                                                   HTTPServiceName,
                                                   ChargingSession_Id.TryParse,
                                                   out ChargingSession_Id? sessionId,
                                                   request,
                                                   out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse CPOPartnerSessionId    [optional]

                    if (!json.ParseOptionalStruct2("CPOPartnerSessionId",
                                                   "CPO partner charging session identification",
                                                   HTTPServiceName,
                                                   ChargingSession_Id.TryParse,
                                                   out ChargingSession_Id? cpoPartnerSessionId,
                                                   request,
                                                   out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse ChargingProductId      [optional]

                    if (!json.ParseOptionalStruct2("ChargingProductId",
                                                   "Charging product identification",
                                                   HTTPServiceName,
                                                   ChargingProduct_Id.TryParse,
                                                   out ChargingProduct_Id? chargingProductId,
                                                   request,
                                                   out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #endregion


                    var result  = await roamingNetwork.AuthorizeStart(
                                            LocalAuthentication.FromAuthToken(authToken),
                                            ChargingLocation.FromEVSEId(evse.Id),
                                            chargingProductId.HasValue
                                                ? new ChargingProduct(chargingProductId.Value)
                                                : null,
                                            sessionId,
                                            cpoPartnerSessionId,
                                            //operatorId,
                                            null,

                                            request.Timestamp,
                                            request.EventTrackingId,
                                            null,
                                            request.CancellationToken
                                        );


                    #region Authorized

                    if (result.Result == AuthStartResultTypes.Authorized)
                        return new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.OK,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                = HTTPContentType.Application.JSON_UTF8,
                                Content                    = result.ToJSON().ToUTF8Bytes()
                            };

                    #endregion

                    #region NotAuthorized

                    else if (result.Result == AuthStartResultTypes.Error)
                        return new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                = HTTPContentType.Application.JSON_UTF8,
                                Content                    = result.ToJSON().ToUTF8Bytes()
                            };

                    #endregion

                    #region Forbidden

                    else
                        return new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Forbidden, //ToDo: Is this smart?
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                = HTTPContentType.Application.JSON_UTF8,
                                Content                    = result.ToJSON().ToUTF8Bytes()
                            };

                    #endregion

                },
                AllowReplacement: URLReplacement.Allow
            );

            #endregion

            #region AUTHSTOP    ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // -----------------------------------------------------------------------
            // curl -v -X AUTHSTOP -H "Content-Type: application/json" \
            //                     -H "Accept:       application/json" \
            //      -d "{ \"SessionId\":  \"60ce73f6-0a88-1296-3d3d-623fdd276ddc\", \
            //            \"AuthToken\":  \"00112233\" }" \
            //      http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            HTTPBaseAPI.AddHandler(

                AUTHSTOP,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:  SendAuthorizeStopEVSEHTTPRequest,
                HTTPResponseLogger: SendAuthorizeStopEVSEHTTPResponse,
                HTTPDelegate:       async request => {

                    #region Parse RoamingNetworkId and EVSEId URI parameters

                    if (!request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse JSON

                    if (!request.TryParseJSONObjectRequestBody(out var JSON,
                                                               out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #region Parse SessionId    [mandatory]

                    if (!JSON.ParseMandatory("SessionId",
                                             "Charging session identification",
                                             HTTPServiceName,
                                             ChargingSession_Id.TryParse,
                                             out ChargingSession_Id sessionId,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse AuthToken    [mandatory]

                    if (!JSON.ParseMandatory("AuthToken",
                                             "Authentication token",
                                             HTTPServiceName,
                                             AuthenticationToken.TryParse,
                                             out AuthenticationToken AuthToken,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse CPOPartnerSessionId    [optional]

                    if (!JSON.ParseOptionalStruct2("CPOPartnerSessionId",
                                                   "CPO partner charging session identification",
                                                   HTTPServiceName,
                                                   ChargingSession_Id.TryParse,
                                                   out ChargingSession_Id? CPOPartnerSessionId,
                                                   request,
                                                   out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse OperatorId   [optional]

                    if (!JSON.ParseOptional("OperatorId",
                                            "Charging Station Operator identification",
                                            HTTPServiceName,
                                            ChargingStationOperator_Id.TryParse,
                                            out ChargingStationOperator_Id chargingStationOperatorId,
                                            request,
                                            out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #endregion


                    var result = await roamingNetwork.AuthorizeStop(
                                           sessionId,
                                           LocalAuthentication.FromAuthToken(AuthToken),
                                           ChargingLocation.   FromEVSEId    (evse.Id),
                                           CPOPartnerSessionId,
                                           //chargingStationOperatorId,
                                           null,

                                           request.Timestamp,
                                           request.EventTrackingId,
                                           null,
                                           request.CancellationToken
                                       );


                    #region Authorized

                    if (result.Result == AuthStopResultTypes.Authorized)
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode  = HTTPStatusCode.OK,
                                   Server                     = HTTPServiceName,
                                   Date                       = Timestamp.Now,
                                   AccessControlAllowOrigin   = "*",
                                   AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                   AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                   ContentType                = HTTPContentType.Application.JSON_UTF8,
                                   Content                    = result.ToJSON().ToUTF8Bytes()
                               };

                    #endregion

                    #region NotAuthorized

                    else if (result.Result == AuthStopResultTypes.NotAuthorized)
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                   Server                     = HTTPServiceName,
                                   Date                       = Timestamp.Now,
                                   AccessControlAllowOrigin   = "*",
                                   AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                   AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                   ContentType                = HTTPContentType.Application.JSON_UTF8,
                                   Content                    = result.ToJSON().ToUTF8Bytes()
                               };

                    #endregion

                    #region Forbidden

                    return new HTTPResponse.Builder(request) {
                               HTTPStatusCode             = HTTPStatusCode.Forbidden, //ToDo: Is this smart?
                               Server                     = HTTPServiceName,
                               Date                       = Timestamp.Now,
                               AccessControlAllowOrigin   = "*",
                               AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                               AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                               ContentType                = HTTPContentType.Application.JSON_UTF8,
                               Content                    = result.ToJSON().ToUTF8Bytes()
                           };

                    #endregion

                },
                AllowReplacement: URLReplacement.Allow
            );

            #endregion

            #region REMOTESTART ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // -----------------------------------------------------------------------
            // curl -v -X REMOTESTART -H "Content-Type: application/json" \
            //                        -H "Accept:       application/json"  \
            //      -d "{ \"ProviderId\":  \"DE*GDF\", \
            //            \"eMAId\":       \"DE*GDF*00112233*1\" }" \
            //      http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            HTTPBaseAPI.AddHandler(

                REMOTESTART,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   SendRemoteStartEVSERequest,
                HTTPResponseLogger:  SendRemoteStartEVSEResponse,
                HTTPDelegate:        async request => {

                    #region Get RoamingNetwork and EVSE URI parameters

                    if (!request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse JSON  [optional]

                    ChargingProduct_Id?      ChargingProductId    = null;
                    ChargingReservation_Id?  ReservationId        = null;
                    ChargingSession_Id?      SessionId            = null;
                    EMobilityProvider_Id?    ProviderId           = null;
                    EMobilityAccount_Id      eMAId                = default;
                    Auth_Path?               AuthenticationPath   = null;

                    if (request.TryParseJSONObjectRequestBody(out var json,
                                                              out httpResponseBuilder))
                    {

                        #region Check ChargingProductId  [optional]

                        if (!json.ParseOptionalStruct2("ChargingProductId",
                                                       "Charging product identification",
                                                       HTTPServiceName,
                                                       ChargingProduct_Id.TryParse,
                                                       out ChargingProductId,
                                                       request,
                                                       out httpResponseBuilder))
                        {

                            return httpResponseBuilder;

                        }

                        #endregion

                        // MaxKWh
                        // MaxPrice

                        #region Check ReservationId      [optional]

                        if (!json.ParseOptionalStruct2("ReservationId",
                                                       "Charging reservation identification",
                                                       HTTPServiceName,
                                                       ChargingReservation_Id.TryParse,
                                                       out ReservationId,
                                                       request,
                                                       out httpResponseBuilder))
                        {

                            return httpResponseBuilder;

                        }

                        #endregion

                        #region Parse SessionId          [optional]

                        if (!json.ParseOptionalStruct2("SessionId",
                                                       "Charging session identification",
                                                       HTTPServiceName,
                                                       ChargingSession_Id.TryParse,
                                                       out SessionId,
                                                       request,
                                                       out httpResponseBuilder))
                        {

                            return httpResponseBuilder;

                        }

                        #endregion

                        #region Parse ProviderId         [optional]

                        if (!json.ParseOptionalStruct2("ProviderId",
                                                       "EV service provider identification",
                                                       HTTPServiceName,
                                                       EMobilityProvider_Id.TryParse,
                                                       out ProviderId,
                                                       request,
                                                       out httpResponseBuilder))
                        {

                            return httpResponseBuilder;

                        }

                        #endregion

                        #region Parse eMAId             [mandatory]

                        if (!json.ParseMandatory("eMAId",
                                                 "e-Mobility account identification",
                                                 HTTPServiceName,
                                                 EMobilityAccount_Id.TryParse,
                                                 out eMAId,
                                                 request,
                                                 out httpResponseBuilder))

                            return httpResponseBuilder;

                        #endregion

                        #region Parse AuthenticationPath    [optional]

                        if (json.ParseOptional("authenticationPath",
                                               "authentication path",
                                               Auth_Path.TryParse,
                                               out AuthenticationPath,
                                               out var errorResponse))
                        {
                            if (errorResponse is not null)
                                return new HTTPResponse.Builder(request) {
                                        HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                        ContentType     = HTTPContentType.Application.JSON_UTF8,
                                        Content         = new JObject(new JProperty("description", "Invalid authentication path: " + errorResponse)).ToUTF8Bytes()
                                    };
                        }

                        #endregion

                    }

                    else
                        return httpResponseBuilder;

                    #endregion


                    var result = await roamingNetwork.RemoteStart(
                                           ChargingLocation.FromEVSEId(evse.Id),
                                           ChargingProductId.HasValue
                                               ? new ChargingProduct(ChargingProductId.Value)
                                               : null,
                                           ReservationId,
                                           SessionId,
                                           ProviderId,
                                           RemoteAuthentication.FromRemoteIdentification(eMAId),
                                           null,
                                           AuthenticationPath,
                                           null,

                                           request.Timestamp,
                                           request.EventTrackingId,
                                           null,
                                           request.CancellationToken
                                       );


                    #region Success

                    if (result.Result == RemoteStartResultTypes.Success)
                        return new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.Created,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                = HTTPContentType.Application.JSON_UTF8,
                                Content                    = JSONObject.Create(

                                                                 result.Session is not null
                                                                     ? new JProperty("sessionId",   result.Session.Id.ToString())
                                                                     : null

                                                             ).ToUTF8Bytes()
                            };

                    #endregion

                    #region ...or fail!

                    else
                        return 
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                = HTTPContentType.Application.JSON_UTF8,
                                Content                    = JSONObject.Create(

                                                                result.Session is not null
                                                                    ? new JProperty("sessionId",     result.Session.Id.ToString())
                                                                    : null,

                                                                      new JProperty("result",        result.Result.    ToString()),

                                                                result.Description is not null
                                                                    ? new JProperty("description",   result.Description)
                                                                    : null

                                                            ).ToUTF8Bytes()
                            };

                    #endregion

                }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region REMOTESTOP  ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}

            // -----------------------------------------------------------------------
            // curl -v -X REMOTESTOP -H "Content-Type: application/json" \
            //                       -H "Accept:       application/json"  \
            //      -d "{ \"ProviderId\":  \"DE*8BD\", \
            //            \"SessionId\":   \"60ce73f6-0a88-1296-3d3d-623fdd276ddc\", \
            //            \"eMAId\":       \"DE*GDF*00112233*1\" }" \
            //      http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1
            HTTPBaseAPI.AddHandler(

                REMOTESTOP,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   SendRemoteStopEVSEHTTPRequest,
                HTTPResponseLogger:  SendRemoteStopEVSEHTTPResponse,
                HTTPDelegate:        async request => {

                    #region Get RoamingNetwork and EVSE URI parameters

                    if (!request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse JSON

                    if (!request.TryParseJSONObjectRequestBody(out var json,
                                                               out httpResponseBuilder,
                                                               AllowEmptyHTTPBody: false))
                    {
                        return httpResponseBuilder;
                    }

                    // Bypass SessionId check for remote safety admins
                    // coming from the same ev service provider

                    #region Parse SessionId         [mandatory]

                    if (!json.ParseMandatory("SessionId",
                                             "Charging session identification",
                                             HTTPServiceName,
                                             ChargingSession_Id.TryParse,
                                             out ChargingSession_Id SessionId,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse ProviderId         [optional]

                    if (!json.ParseOptionalStruct2("ProviderId",
                                                   "EV service provider identification",
                                                   HTTPServiceName,
                                                   EMobilityProvider_Id.TryParse,
                                                   out EMobilityProvider_Id? ProviderId,
                                                   request,
                                                   out httpResponseBuilder))
                    {
                        return httpResponseBuilder!;
                    }

                    #endregion

                    #region Parse eMAId              [optional]

                    if (!json.ParseOptionalStruct2("eMAId",
                                                   "e-Mobility account identification",
                                                   HTTPServiceName,
                                                   EMobilityAccount_Id.TryParse,
                                                   out EMobilityAccount_Id? eMAId,
                                                   request,
                                                   out httpResponseBuilder))
                    {
                        return httpResponseBuilder!;
                    }

                    #endregion

                    // ReservationHandling

                    #region Parse AuthenticationPath    [optional]

                    if (json.ParseOptional("authenticationPath",
                                           "authentication path",
                                           Auth_Path.TryParse,
                                           out Auth_Path? AuthenticationPath,
                                           out var errorResponse))
                    {
                        if (errorResponse is not null)
                            return new HTTPResponse.Builder(request) {
                                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                    ContentType     = HTTPContentType.Application.JSON_UTF8,
                                    Content         = new JObject(new JProperty("description", "Invalid authentication path: " + errorResponse)).ToUTF8Bytes()
                                };
                    }

                    #endregion

                    #endregion


                    var result = await roamingNetwork.RemoteStop(
                                           SessionId,
                                           ReservationHandling.Close, // ToDo: Parse this property!
                                           ProviderId,
                                           RemoteAuthentication.FromRemoteIdentification(eMAId),
                                           null,
                                           AuthenticationPath,
                                           null,

                                           request.Timestamp,
                                           request.EventTrackingId,
                                           null,
                                           request.CancellationToken
                                       );


                    #region Success

                    if (result.Result == RemoteStopResultTypes.Success)
                    {

                        if (result.ReservationHandling.IsKeepAlive == false)
                            return new HTTPResponse.Builder(request) {
                                       HTTPStatusCode             = HTTPStatusCode.NoContent,
                                       Server                     = HTTPServiceName,
                                       Date                       = Timestamp.Now,
                                       AccessControlAllowOrigin   = "*",
                                       AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                       AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                   };

                        else
                            return new HTTPResponse.Builder(request) {
                                       HTTPStatusCode             = HTTPStatusCode.OK,
                                       Server                     = HTTPServiceName,
                                       Date                       = Timestamp.Now,
                                       AccessControlAllowOrigin   = "*",
                                       AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                       AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                       ContentType                = HTTPContentType.Application.JSON_UTF8,
                                       Content                    = new JObject(

                                                                        new JProperty("keepAlive",   (Int32) result.ReservationHandling.KeepAliveTime.TotalSeconds)

                                                                    ).ToUTF8Bytes()
                                   };

                    }

                    #endregion

                    #region ...or fail

                    else
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                   Server                     = HTTPServiceName,
                                   Date                       = Timestamp.Now,
                                   AccessControlAllowOrigin   = "*",
                                   AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                   AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                   ContentType                = HTTPContentType.Application.JSON_UTF8,
                                   Content                    = new JObject(
                                                                    new JProperty("description",   result.Result.ToString())
                                                                ).ToUTF8Bytes()
                               };

                    #endregion

                }, AllowReplacement: URLReplacement.Allow);

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
            HTTPBaseAPI.AddHandler(

                SENDCDR,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   SendChargeDetailRecordsHTTPRequest,
                HTTPResponseLogger:  SendChargeDetailRecordsHTTPResponse,
                HTTPDelegate:        async request => {

                    #region Parse RoamingNetwork and EVSE

                    if (!request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse JSON

                    if (!request.TryParseJSONObjectRequestBody(out var json,
                                                               out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #region Parse SessionId            [mandatory]

                    if (!json.ParseMandatory("SessionId",
                                             "charging session identification",
                                             HTTPServiceName,
                                             ChargingSession_Id.TryParse,
                                             out ChargingSession_Id SessionId,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse ChargingProductId    [optional]

                    if (json.ParseOptionalStruct2("ChargingProductId",
                                                  "charging product identification",
                                                  HTTPServiceName,
                                                  ChargingProduct_Id.TryParse,
                                                  out ChargingProduct_Id? ChargingProductId,
                                                  request,
                                                  out httpResponseBuilder))
                    {
                        if (httpResponseBuilder is not null)
                            return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse AuthToken or eMAId

                    if (json.ParseOptional("AuthToken",
                                           "authentication token",
                                           HTTPServiceName,
                                           AuthenticationToken.TryParse,
                                           out AuthenticationToken? AuthToken,
                                           request,
                                           out httpResponseBuilder))
                    {
                        if (httpResponseBuilder is not null)
                            return httpResponseBuilder;
                    }

                    if (json.ParseOptionalStruct2("eMAId",
                                                  "e-mobility account identification",
                                                  HTTPServiceName,
                                                  EMobilityAccount_Id.TryParse,
                                                  out EMobilityAccount_Id? eMAId,
                                                  request,
                                                  out httpResponseBuilder))
                    {
                        if (httpResponseBuilder is not null)
                            return httpResponseBuilder;
                    }


                    if (AuthToken is null && eMAId is null)
                        return new HTTPResponse.Builder(request) {
                            HTTPStatusCode  = HTTPStatusCode.BadRequest,
                            Server          = HTTPServiceName,
                            Date            = Timestamp.Now,
                            ContentType     = HTTPContentType.Application.JSON_UTF8,
                            Content         = new JObject(
                                                  new JProperty("description", "Missing authentication token or eMAId!")
                                              ).ToUTF8Bytes()
                        };

                    #endregion

                    #region Parse ChargeStart/End...

                    if (!json.ParseMandatory("ChargeStart",
                                             "Charging start time",
                                             HTTPServiceName,
                                             out DateTimeOffset ChargingStart,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    if (!json.ParseMandatory("ChargeEnd",
                                             "Charging end time",
                                             HTTPServiceName,
                                             out DateTimeOffset ChargingEnd,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse SessionStart/End...

                    if (!json.ParseMandatory("SessionStart",
                                             "Charging start time",
                                             HTTPServiceName,
                                             out DateTime SessionStart,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    if (!json.ParseMandatory("SessionEnd",
                                             "Charging end time",
                                             HTTPServiceName,
                                             out DateTime SessionEnd,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse MeterValueStart/End...

                    if (!json.ParseMandatory("MeterValueStart",
                                             "Energy meter start value",
                                             HTTPServiceName,
                                             WattHour.TryParseKWh,
                                             out WattHour MeterValueStart,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    if (!json.ParseMandatory("MeterValueEnd",
                                             "Energy meter end value",
                                             HTTPServiceName,
                                             WattHour.TryParseKWh,
                                             out WattHour MeterValueEnd,
                                             request,
                                             out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #endregion


                    var authenticationStart  = AuthToken.HasValue
                                                   ? (AAuthentication)       LocalAuthentication. FromAuthToken           (AuthToken.Value)
                                                   : eMAId.HasValue
                                                         ? (AAuthentication) RemoteAuthentication.FromRemoteIdentification(eMAId.    Value)
                                                         : null;

                    var chargeDetailRecord   = new ChargeDetailRecord(
                                                   Id:                     ChargeDetailRecord_Id.Parse(SessionId.ToString()),
                                                   SessionId:              SessionId,
                                                   EVSEId:                 evse.Id,
                                                   EVSE:                   evse,
                                                   ChargingProduct:        ChargingProductId.HasValue
                                                                               ? new ChargingProduct(ChargingProductId.Value)
                                                                               : null,
                                                   SessionTime:            new StartEndDateTime(SessionStart, SessionEnd),
                                                   AuthenticationStart:    authenticationStart,
                                                   //ChargingTime:         new StartEndDateTime(ChargingStart.Value, ChargingEnd.Value),
                                                   EnergyMeteringValues:   [
                                                                               new EnergyMeteringValue(ChargingStart, MeterValueStart, EnergyMeteringValueTypes.Start),
                                                                               new EnergyMeteringValue(ChargingEnd,   MeterValueEnd,   EnergyMeteringValueTypes.Stop)
                                                                           ]
                                               );

                    var result               = await roamingNetwork.SendChargeDetailRecords(
                                                         [ chargeDetailRecord ],
                                                         TransmissionTypes.Enqueue,

                                                         request.Timestamp,
                                                         request.EventTrackingId,
                                                         null,
                                                         request.CancellationToken
                                                     );


                    #region Forwarded

                    if (result.Result == SendCDRsResultTypes.Success)
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode             = HTTPStatusCode.OK,
                                   Server                     = HTTPServiceName,
                                   Date                       = Timestamp.Now,
                                   AccessControlAllowOrigin   = "*",
                                   AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                   AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                   ContentType                = HTTPContentType.Application.JSON_UTF8,
                                   Content                    = JSONObject.Create(
                                                                    new JProperty("status",          "forwarded"),
                                                                    new JProperty("authorizatorId",   result.AuthorizatorId.ToString())
                                                                ).ToUTF8Bytes()
                               };

                    #endregion

                    #region NotForwared

                    else if (result.Result == SendCDRsResultTypes.Error)
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode             = HTTPStatusCode.OK,
                                   Server                     = HTTPServiceName,
                                   Date                       = Timestamp.Now,
                                   AccessControlAllowOrigin   = "*",
                                   AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                   AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                   ContentType                = HTTPContentType.Application.JSON_UTF8,
                                   Content                    = JSONObject.Create(
                                                                    new JProperty("status",   "Not forwarded")
                                                                ).ToUTF8Bytes()
                               };

                    #endregion

                    #region ...or fail!

                    else
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode             = HTTPStatusCode.NotFound,
                                   Server                     = HTTPServiceName,
                                   Date                       = Timestamp.Now,
                                   AccessControlAllowOrigin   = "*",
                                   AccessControlAllowMethods  = [ "GET", "RESERVE", "AUTHSTART", "AUTHSTOP", "REMOTESTART", "REMOTESTOP", "SENDCDR" ],
                                   AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                   ContentType                = HTTPContentType.Application.JSON_UTF8,
                                   Content                    = JSONObject.Create(
                                                                    new JProperty("sessionId",        SessionId.ToString()),
                                                                    new JProperty("description",      result.Description),
                                                                    new JProperty("authorizatorId",   result.AuthorizatorId.ToString())
                                                                ).ToUTF8Bytes()
                               };

                    #endregion

                },
                AllowReplacement: URLReplacement.Allow
            );

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/AdminStatus

            #region OPTIONS     ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/AdminStatus

            // --------------------------------------------------------------------------------------
            // curl -v -X OPTIONS http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1/AdminStatus
            // --------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.OPTIONS,
                URLPathPrefix + "RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/AdminStatus",
                HTTPDelegate: Request => {

                    #region Check RoamingNetworkId and EVSEId URI parameters

                    if (!Request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(Request) {
                            HTTPStatusCode              = HTTPStatusCode.NoContent,
                            Server                      = HTTPServiceName,
                            Date                        = Timestamp.Now,
                            AccessControlAllowOrigin    = "*",
                            AccessControlAllowMethods   = [ "OPTIONS", "GET", "SET" ],
                            AccessControlAllowHeaders   = [ "Content-Type", "Accept", "Authorization" ],
                        }.AsImmutable);

                },
                AllowReplacement: URLReplacement.Allow
            );

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/AdminStatus

            // -----------------------------------------------------------------------
            // curl -v -H "Accept:       application/json" \
            //      http://127.0.0.1:5500/RNs/TEST/EVSEs/DE*GEF*E0001*1/AdminStatus
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/AdminStatus",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Parse RoamingNetworkId and EVSEId parameters

                    if (!request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode              = HTTPStatusCode.OK,
                            Server                      = HTTPServiceName,
                            Date                        = Timestamp.Now,
                            AccessControlAllowOrigin    = "*",
                            AccessControlAllowMethods   = [ "GET" ],
                            AccessControlAllowHeaders   = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                        = "1",
                            ContentType                 = HTTPContentType.Application.JSON_UTF8,
                            Content                     = evse.AdminStatus.
                                                              ToJSON().
                                                              ToUTF8Bytes(),
                            Connection                  = ConnectionType.KeepAlive
                        }.AsImmutable);

                }
            );

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
            HTTPBaseAPI.AddHandler(

                HTTPMethod.SET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/AdminStatus",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   SetEVSEAdminStatusRequest,
                HTTPResponseLogger:  SetEVSEAdminStatusResponse,
                HTTPDelegate:        async request => {

                    #region Parse RoamingNetwork and EVSE

                    if (!request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse JSON

                    if (!request.TryParseJSONObjectRequestBody(out var json,
                                                               out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #region Parse CurrentStatus  [optional]

                    if (json.ParseOptional("currentStatus",
                                           "EVSE admin status",
                                           HTTPServiceName,
                                           EVSEAdminStatusType.TryParse,
                                           out EVSEAdminStatusType? currentStatus,
                                           request,
                                           out httpResponseBuilder))
                    {
                        if (httpResponseBuilder is not null)
                            return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse StatusList     [optional]

                    var statusList = new List<Timestamped<EVSEAdminStatusType>>();

                    if (json.ParseOptional("statusList",
                                           "status list",
                                           HTTPServiceName,
                                           out JObject statusListJSON,
                                           request,
                                           out httpResponseBuilder))
                    {

                        if (httpResponseBuilder is not null)
                            return httpResponseBuilder;

                        if (statusListJSON is not null)
                        {
                            foreach (var jproperty in statusListJSON)
                            {

                                try
                                {

                                    if (jproperty.Value != null &&
                                        DateTimeOffset.     TryParse(jproperty.Key,              out var timestamp) &&
                                        EVSEAdminStatusType.TryParse(jproperty.Value.ToString(), out var evseAdminStatusType))
                                    {
                                        statusList.Add(
                                            new Timestamped<EVSEAdminStatusType>(
                                                timestamp,
                                                evseAdminStatusType
                                            )
                                        );
                                    }

                                }
                                catch
                                {
                                    // Will send the below BadRequest HTTP reply...
                                }

                            }
                        }

                        if (statusListJSON is null || statusList.Count == 0)
                            return new HTTPResponse.Builder(request) {
                                       HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                       Server          = HTTPServiceName,
                                       Date            = Timestamp.Now,
                                       ContentType     = HTTPContentType.Application.JSON_UTF8,
                                       Content         = new JObject(
                                                           new JProperty("description", "Invalid status list!")
                                                       ).ToUTF8Bytes()
                                   };

                    }

                    #endregion

                    #region Fail, if both CurrentStatus and StatusList are missing...

                    if (!currentStatus.HasValue && statusList.Count == 0)
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                   Server          = HTTPServiceName,
                                   Date            = Timestamp.Now,
                                   ContentType     = HTTPContentType.Application.JSON_UTF8,
                                   Content         = new JObject(
                                                         new JProperty("description", "Either a 'currentStatus' or a 'statusList' must be send!")
                                                     ).ToUTF8Bytes()
                               };

                    #endregion

                    #endregion


                    if (currentStatus.HasValue)
                        roamingNetwork.SetEVSEAdminStatus(
                                  evse.Id,
                                  request.Timestamp,
                                  currentStatus.Value
                              );

                    else if (statusList.Count > 0)
                        roamingNetwork.SetEVSEAdminStatus(
                                  evse.Id,
                                  statusList
                              );


                    return new HTTPResponse.Builder(request) {
                               HTTPStatusCode  = HTTPStatusCode.OK,
                               Server          = HTTPServiceName,
                               Date            = Timestamp.Now,
                               Connection      = ConnectionType.KeepAlive
                           };

                });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status

            #region OPTIONS     ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status

            // --------------------------------------------------------------------------------------
            // curl -v -X OPTIONS http://127.0.0.1:5500/RNs/Test/EVSEs/DE*GEF*E000001*1/Status
            // --------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.OPTIONS,
                URLPathPrefix + "RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status",
                HTTPDelegate: Request => {

                    #region Check RoamingNetworkId and EVSEId URI parameters

                    if (!Request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(Request) {
                            HTTPStatusCode              = HTTPStatusCode.NoContent,
                            Server                      = HTTPServiceName,
                            Date                        = Timestamp.Now,
                            AccessControlAllowOrigin    = "*",
                            AccessControlAllowMethods   = [ "OPTIONS", "GET", "SET" ],
                            AccessControlAllowHeaders   = [ "Content-Type", "Accept", "Authorization" ],
                        }.AsImmutable);

                },
                AllowReplacement: URLReplacement.Allow
            );

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status

            // -----------------------------------------------------------------------
            // curl -v -H "Accept:       application/json" \
            //      http://127.0.0.1:5500/RNs/TEST/EVSEs/DE*GEF*E0001*1/Status
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Parse RoamingNetworkId and EVSEId parameters

                    if (!request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode              = HTTPStatusCode.OK,
                            Server                      = HTTPServiceName,
                            Date                        = Timestamp.Now,
                            AccessControlAllowOrigin    = "*",
                            AccessControlAllowMethods   = [ "GET" ],
                            AccessControlAllowHeaders   = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                        = "1",
                            ContentType                 = HTTPContentType.Application.JSON_UTF8,
                            Content                     = evse.Status.
                                                              ToJSON().
                                                              ToUTF8Bytes(),
                            Connection                  = ConnectionType.KeepAlive
                        }.AsImmutable);

                }
            );

            #endregion

            #region SET         ~/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status

            // -----------------------------------------------------------------------
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"currentStatus\":  \"OutOfService\" }" \
            //      http://127.0.0.1:5500/RNs/TEST/EVSEs/DE*GEF*EVSE*ALPHA*ONE*1/Status
            // -----------------------------------------------------------------------
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"statusList\":  { \
            //              \"2014-10-13T22:14:01.862Z\": \"OutOfService\", \
            //              \"2014-10-13T21:32:15.386Z\": \"Charging\"  \
            //          }" \
            //      http://127.0.0.1:5500/RNs/TEST/EVSEs/DE*GEF*EVSE*ALPHA*ONE*1/Status
            // -----------------------------------------------------------------------
            // curl -v -X SET -H "Content-Type: application/json" \
            //                -H "Accept:       application/json" \
            //      -d "{ \"currentStatus\":  \"Charging\" }"     \
            //      http://127.0.0.1:3004/RNs/Prod/EVSEs/DE*BDO*EVSE*CI*TESTS*A*1/Status
            HTTPBaseAPI.AddHandler(

                HTTPMethod.SET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs/{EVSEId}/Status",
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   SetEVSEStatusRequest,
                HTTPResponseLogger:  SetEVSEStatusResponse,
                HTTPDelegate:        async request => {

                    #region Check RoamingNetworkId and EVSEId URI parameters

                    if (!request.TryParseRoamingNetworkAndEVSE(this,
                                                               out var roamingNetwork,
                                                               out var evse,
                                                               out var httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse JSON

                    if (!request.TryParseJSONObjectRequestBody(out var json,
                                                               out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #region Parse CurrentStatus  [optional]

                    if (json.ParseOptional("currentStatus",
                                           "EVSE admin status",
                                           HTTPServiceName,
                                           EVSEStatusType.TryParse,
                                           out EVSEStatusType? currentStatus,
                                           request,
                                           out httpResponseBuilder))
                    {
                        if (httpResponseBuilder is not null)
                            return httpResponseBuilder;
                    }

                    #endregion

                    #region Parse StatusList     [optional]

                    var statusList = new List<Timestamped<EVSEStatusType>>();

                    if (json.ParseOptional("statusList",
                                           "status list",
                                           HTTPServiceName,
                                           out JObject statusListJSON,
                                           request,
                                           out httpResponseBuilder))
                    {

                        if (httpResponseBuilder is not null)
                            return httpResponseBuilder;

                        if (statusListJSON is not null)
                        {
                            foreach (var jproperty in statusListJSON)
                            {

                                try
                                {

                                    if (jproperty.Value != null &&
                                        DateTimeOffset.TryParse(jproperty.Key,              out var timestamp) &&
                                        EVSEStatusType.TryParse(jproperty.Value.ToString(), out var evseAdminStatusType))
                                    {
                                        statusList.Add(
                                            new Timestamped<EVSEStatusType>(
                                                timestamp,
                                                evseAdminStatusType
                                            )
                                        );
                                    }

                                }
                                catch
                                {
                                    // Will send the below BadRequest HTTP reply...
                                }

                            }
                        }

                        if (statusListJSON is null || statusList.Count == 0)
                            return new HTTPResponse.Builder(request) {
                                       HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                       Server          = HTTPServiceName,
                                       Date            = Timestamp.Now,
                                       ContentType     = HTTPContentType.Application.JSON_UTF8,
                                       Content         = new JObject(
                                                           new JProperty("description", "Invalid status list!")
                                                       ).ToUTF8Bytes()
                                   };

                    }

                    #endregion

                    #region Fail, if both CurrentStatus and StatusList are missing...

                    if (!currentStatus.HasValue && statusList.Count == 0)
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                   Server          = HTTPServiceName,
                                   Date            = Timestamp.Now,
                                   ContentType     = HTTPContentType.Application.JSON_UTF8,
                                   Content         = new JObject(
                                                         new JProperty("description", "Either a 'currentStatus' or a 'statusList' must be send!")
                                                     ).ToUTF8Bytes()
                               };

                    #endregion

                    #endregion


                    if (currentStatus.HasValue)
                        await roamingNetwork.SetEVSEStatus(
                                  evse.Id,
                                  request.Timestamp,
                                  currentStatus.Value
                              );

                    else if (statusList.Count > 0)
                        await roamingNetwork.SetEVSEStatus(
                                  evse.Id,
                                  statusList
                              );


                    return new HTTPResponse.Builder(request) {
                               HTTPStatusCode  = HTTPStatusCode.OK,
                               Server          = HTTPServiceName,
                               Date            = Timestamp.Now,
                               Connection      = ConnectionType.KeepAlive
                           };

                });

            #endregion

            #endregion


            // Charging Sessions

            #region ~/RNs/{RoamingNetworkId}/ChargingSessions

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingSessions

            // ---------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingSessions
            // ---------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingSessions",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    #region Get roaming network

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    //ToDo: Filter sessions by HTTPUser organization!


                    //ToDo: Getting the expected total count might be very expensive!
                    var expectedCount           = roamingNetwork.ChargingSessions.ULongCount();


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = roamingNetwork.ChargingSessions.
                                                                OrderBy(session => session.SessionTime.StartTime).
                                                                ToJSON (Embedded:    false,
                                                                        OnlineInfos: false,
                                                                        CustomChargingSessionSerializer,
                                                                        CustomCDRReceivedInfoSerializer,
                                                                        CustomChargeDetailRecordSerializer,
                                                                        CustomSendCDRResultSerializer,
                                                                        request.QueryString.GetUInt64("skip"),
                                                                        request.QueryString.GetUInt64("take")
                                                            ).ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = expectedCount,
                            Connection                    = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region COUNT       ~/RNs/{RoamingNetworkId}/ChargingSessions

            // ------------------------------------------------------------------------------------------------
            // curl -v -X COUNT -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingSessions
            // ------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.COUNT,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingSessions",
                HTTPContentType.Text.PLAIN,
                HTTPDelegate: request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    #region Get roaming network

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Text.PLAIN,
                            Content                       = roamingNetwork.SessionsStore.NumberOfStoredSessions.ToString().ToUTF8Bytes(),
                            Connection                    = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion


            #region GET         ~/RNs/{RoamingNetworkId}/ChargingSessions/{ChargingSessionId}

            // -----------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:5500/RNs/Test/ChargingSessions/{ChargingSessionId}
            // -----------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingSessions/{ChargingSessionId}",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    #region Get roaming network and charging session

                    if (!request.TryParseRoamingNetworkAndChargingSession(this,
                                                                          out var roamingNetwork,
                                                                          out var chargingSession,
                                                                          out httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    //ToDo: Filter sessions by HTTPUser organization!


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET", "COUNT", "OPTIONS" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ETag                          = "1",
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = chargingSession.
                                                                ToJSON(Embedded:                         false,
                                                                       CustomChargingSessionSerializer:  CustomChargingSessionSerializer).
                                                                ToUTF8Bytes(),
                            Connection                    = ConnectionType.KeepAlive
                        }.AsImmutable);

                });

            #endregion

            #region RESENDCDR   ~/RNs/{RoamingNetworkId}/ChargingSessions/{ChargingSessionId}

            // ------------------------------------------------------------------------------------------
            // curl -v -X RESENDCDR http://127.0.0.1:5500/RNs/Test/ChargingSessions/{ChargingSessionId}
            // ------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                RESENDCDR,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingSessions/{ChargingSessionId}",
                HTTPContentType.Application.JSON_UTF8,
                async request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    #region Get roaming network and charging session

                    if (!request.TryParseRoamingNetworkAndChargingSession(this,
                                                                          out var roamingNetwork,
                                                                          out var chargingSession,
                                                                          out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion

                    //ToDo: Filter sessions by HTTPUser organization!


                    var lastCDR = chargingSession.ReceivedCDRInfos.LastOrDefault()?.ChargeDetailRecord;

                    if (lastCDR is not null)
                    {

                        var sendCDRResult = await roamingNetwork.SendChargeDetailRecord(lastCDR);

                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode  = HTTPStatusCode.OK,
                                   ContentType     = HTTPContentType.Text.PLAIN,
                                   Content         = sendCDRResult.ToJSON().ToUTF8Bytes(),
                                   Connection      = ConnectionType.Close
                               }.AsImmutable;

                    }


                    return new HTTPResponse.Builder(request) {
                               HTTPStatusCode                = HTTPStatusCode.NotFound,
                               Server                        = HTTPServiceName,
                               Date                          = Timestamp.Now,
                               AccessControlAllowOrigin      = "*",
                               AccessControlAllowMethods     = [ "GET", "COUNT", "OPTIONS", "RESENDCDR" ],
                               AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                               Connection                    = ConnectionType.Close
                           };

                });

            #endregion


            #region SET         ~/RNs/{RoamingNetworkId}/ChargingSessions/{ChargingSessionId}/{command}

            // -------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X SET -H "Content-Type: application/json" --data @session.json http://127.0.0.1:3004/RNs/Prod/ChargingSessions/{ChargingSessionId}/{command}
            // -------------------------------------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.SET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingSessions/{ChargingSessionId}/{command}",
                HTTPContentType.Application.JSON_UTF8,
                async request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return httpResponseBuilder.AsImmutable;
                    }

                    #endregion

                    #region Get roaming network and charging session identification

                    if (!request.TryParseRoamingNetworkAndChargingSessionId(this,
                                                                            out var roamingNetwork,
                                                                            out var chargingSessionId,
                                                                            out httpResponseBuilder))
                    {
                        return httpResponseBuilder.AsImmutable;
                    }

                    #endregion

                    #region Parse command

                    if (!request.TryGetURLParameter("command", out var command))
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                   Server          = HTTPServiceName,
                                   Date            = Timestamp.Now,
                                   ContentType     = HTTPContentType.Application.JSON_UTF8,
                                   Content         = @"{ ""description"": ""Invalid command!"" }".ToUTF8Bytes(),
                                   Connection      = ConnectionType.KeepAlive
                               };

                    #endregion

                    #region Parse Charging Session JSON

                    if (!request.TryParseJSONObjectRequestBody(out var json,
                                                               out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    if (!ChargingSession.TryParse(json, out var chargingSession, out var errorResponse))
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                   Server          = HTTPServiceName,
                                   Date            = Timestamp.Now,
                                   ContentType     = HTTPContentType.Application.JSON_UTF8,
                                   Content         = new JObject(
                                                         new JProperty("description", errorResponse)
                                                     ).ToUTF8Bytes()
                               };

                    #endregion


                    var result = await roamingNetwork.AddExternalChargingSession(
                                           Timestamp.Now,
                                           this,
                                           command,
                                           chargingSession
                                       );


                    return result

                               ? new HTTPResponse.Builder(request) {
                                     HTTPStatusCode             = HTTPStatusCode.OK,
                                     Server                     = HTTPServiceName,
                                     Date                       = Timestamp.Now,
                                     AccessControlAllowOrigin   = "*",
                                     AccessControlAllowMethods  = [ "SET" ],
                                     AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                     ContentType                = HTTPContentType.Application.JSON_UTF8,
                                     Content                    = chargingSession.
                                                                     ToJSON(Embedded:                         false,
                                                                            CustomChargingSessionSerializer:  CustomChargingSessionSerializer).
                                                                     ToUTF8Bytes(),
                                     Connection                 = ConnectionType.KeepAlive
                                 }

                               : new HTTPResponse.Builder(request) {
                                     HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                     Server                     = HTTPServiceName,
                                     Date                       = Timestamp.Now,
                                     AccessControlAllowOrigin   = "*",
                                     AccessControlAllowMethods  = [ "SET" ],
                                     AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                     //ContentType                = HTTPContentType.Application.JSON_UTF8,
                                     //Content                    = chargingSession.
                                     //                                 ToJSON(Embedded:                         false,
                                     //                                        CustomChargingSessionSerializer:  CustomChargingSessionSerializer).
                                     //                                 ToUTF8Bytes(),
                                     ContentLength              = 0,
                                     Connection                 = ConnectionType.KeepAlive
                                 };

                });

            #endregion

            #region UPDATE      ~/RNs/{RoamingNetworkId}/ChargingSessions/{ChargingSessionId}/{command}

            // ----------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X UPDATE -H "Content-Type: application/json" --data @session.json http://127.0.0.1:3004/RNs/Prod/ChargingSessions/{ChargingSessionId}/{command}
            // ----------------------------------------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.UPDATE,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingSessions/{ChargingSessionId}/{command}",
                HTTPContentType.Application.JSON_UTF8,
                async request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return httpResponseBuilder.AsImmutable;
                    }

                    #endregion

                    #region Get roaming network and charging session identification

                    if (!request.TryParseRoamingNetworkAndChargingSessionId(this,
                                                                            out var roamingNetwork,
                                                                            out var chargingSessionId,
                                                                            out httpResponseBuilder))
                    {
                        return httpResponseBuilder.AsImmutable;
                    }

                    #endregion

                    #region Parse command

                    if (!request.TryGetURLParameter("command", out var command))
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                   Server          = HTTPServiceName,
                                   Date            = Timestamp.Now,
                                   ContentType     = HTTPContentType.Application.JSON_UTF8,
                                   Content         = @"{ ""description"": ""Invalid command!"" }".ToUTF8Bytes(),
                                   Connection      = ConnectionType.KeepAlive
                               };

                    #endregion

                    #region Parse Charging Session JSON

                    if (!request.TryParseJSONObjectRequestBody(out var json,
                                                               out httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    if (!ChargingSession.TryParse(json, out var chargingSession, out var errorResponse))
                        return new HTTPResponse.Builder(request) {
                                   HTTPStatusCode  = HTTPStatusCode.BadRequest,
                                   Server          = HTTPServiceName,
                                   Date            = Timestamp.Now,
                                   ContentType     = HTTPContentType.Application.JSON_UTF8,
                                   Content         = new JObject(
                                                         new JProperty("description", errorResponse)
                                                     ).ToUTF8Bytes()
                               };

                    #endregion


                    var result = await roamingNetwork.UpdateExternalChargingSession(
                                           Timestamp.Now,
                                           this,
                                           command,
                                           chargingSession
                                       );


                    return result

                               ? new HTTPResponse.Builder(request) {
                                     HTTPStatusCode             = HTTPStatusCode.OK,
                                     Server                     = HTTPServiceName,
                                     Date                       = Timestamp.Now,
                                     AccessControlAllowOrigin   = "*",
                                     AccessControlAllowMethods  = [ "SET" ],
                                     AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                     ContentType                = HTTPContentType.Application.JSON_UTF8,
                                     Content                    = chargingSession.
                                                                     ToJSON(Embedded:                         false,
                                                                            CustomChargingSessionSerializer:  CustomChargingSessionSerializer).
                                                                     ToUTF8Bytes(),
                                     Connection                 = ConnectionType.KeepAlive
                                 }

                               : new HTTPResponse.Builder(request) {
                                     HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                     Server                     = HTTPServiceName,
                                     Date                       = Timestamp.Now,
                                     AccessControlAllowOrigin   = "*",
                                     AccessControlAllowMethods  = [ "SET" ],
                                     AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                     //ContentType                = HTTPContentType.Application.JSON_UTF8,
                                     //Content                    = chargingSession.
                                     //                                 ToJSON(Embedded:                         false,
                                     //                                        CustomChargingSessionSerializer:  CustomChargingSessionSerializer).
                                     //                                 ToUTF8Bytes(),
                                     ContentLength              = 0,
                                     Connection                 = ConnectionType.KeepAlive
                                 };

                });

            #endregion



            #region GET         ~/RNs/{RoamingNetworkId}/ChargingSessionAnalytics/MissingCDRResponses

            // ------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingSessionAnalytics/MissingCDRResponses?ExpandCDRs=false
            // ------------------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingSessionAnalytics/MissingCDRResponses",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    #region Get roaming network

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var skip                 = request.QueryString.GetUInt64("skip");
                    var take                 = request.QueryString.GetUInt64("take");
                    var from                 = request.QueryString.ParseFromTimestampFilter();
                    var to                   = request.QueryString.ParseToTimestampFilter();
                    var expandCDRs           = request.QueryString.GetBoolean("ExpandCDRs") ?? false;

                    var missingCDRResponses  = roamingNetwork.ChargingSessions.
                                                   Where  (session => session.ReceivedCDRInfos.Any() &&
                                                                     !session.SendCDRResults.  Any() &&
                                                                     (!from.HasValue ||                                                                                   session.SessionTime.StartTime     >= from.Value) &&
                                                                     (!to.  HasValue || !session.SessionTime.EndTime.HasValue || (session.SessionTime.EndTime.HasValue && session.SessionTime.EndTime.Value <= to.  Value))).
                                                   ToArray();


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = new JArray(
                                                                expandCDRs
                                                                    ? missingCDRResponses.
                                                                          OrderBy(session => session.SessionTime.StartTime).
                                                                          ToJSON (Embedded:    false,
                                                                                  OnlineInfos: false,
                                                                                  CustomChargingSessionSerializer,
                                                                                  CustomCDRReceivedInfoSerializer,
                                                                                  CustomChargeDetailRecordSerializer,
                                                                                  CustomSendCDRResultSerializer,
                                                                                  skip,
                                                                                  take)
                                                                    : missingCDRResponses.
                                                                          OrderBy       (session => session.SessionTime.StartTime).
                                                                          SkipTakeFilter(skip, take).
                                                                          Select        (session => session.Id.ToString())
                                                            ).ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = missingCDRResponses.ULongCount(),
                            Connection                    = ConnectionType.Close
                        }.AsImmutable);

                });

            #endregion

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingSessionAnalytics/FailedCDRResponses

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingSessionAnalytics/FailedCDRResponses?ExpandCDRs=false
            // -----------------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingSessionAnalytics/FailedCDRResponses",
                HTTPContentType.Application.JSON_UTF8,
                request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion

                    #region Get roaming network

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out httpResponseBuilder))
                    {
                        return Task.FromResult(httpResponseBuilder.AsImmutable);
                    }

                    #endregion


                    var skip                 = request.QueryString.GetUInt64("skip");
                    var take                 = request.QueryString.GetUInt64("take");
                    var from                 = request.QueryString.ParseFromTimestampFilter();
                    var to                   = request.QueryString.ParseToTimestampFilter();
                    var expandCDRs           = request.QueryString.GetBoolean("ExpandCDRs") ?? false;

                    var missingCDRResponses  = roamingNetwork.ChargingSessions.
                                                   Where  (session => session.ReceivedCDRInfos.Any() &&
                                                                      session.SendCDRResults.  Any() &&
                                                                      session.SendCDRResults.  All(sendCDRResult => sendCDRResult.Result != SendCDRResultTypes.Success           &&
                                                                                                                    sendCDRResult.Result != SendCDRResultTypes.InvalidSessionId) &&
                                                                     (!from.HasValue ||                                                                                   session.SessionTime.StartTime     >= from.Value) &&
                                                                     (!to.  HasValue || !session.SessionTime.EndTime.HasValue || (session.SessionTime.EndTime.HasValue && session.SessionTime.EndTime.Value <= to.  Value))).
                                                   ToArray();


                    return Task.FromResult(
                        new HTTPResponse.Builder(request) {
                            HTTPStatusCode                = HTTPStatusCode.OK,
                            Server                        = HTTPServiceName,
                            Date                          = Timestamp.Now,
                            AccessControlAllowOrigin      = "*",
                            AccessControlAllowMethods     = [ "GET" ],
                            AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                            ContentType                   = HTTPContentType.Application.JSON_UTF8,
                            Content                       = new JArray(
                                                                expandCDRs
                                                                    ? missingCDRResponses.
                                                                        OrderBy(session => session.SessionTime.StartTime).
                                                                        ToJSON (Embedded:    false,
                                                                                OnlineInfos: false,
                                                                                CustomChargingSessionSerializer,
                                                                                CustomCDRReceivedInfoSerializer,
                                                                                CustomChargeDetailRecordSerializer,
                                                                                CustomSendCDRResultSerializer,
                                                                                skip,
                                                                                take)
                                                                    : missingCDRResponses.
                                                                          OrderBy       (session => session.SessionTime.StartTime).
                                                                          SkipTakeFilter(skip, take).
                                                                          Select        (session => session.Id.ToString())
                                                            ).ToUTF8Bytes(),
                            X_ExpectedTotalNumberOfItems  = missingCDRResponses.ULongCount(),
                            Connection                    = ConnectionType.Close
                        }.AsImmutable);

                });

            #endregion

            #endregion



            // Charge Detail Records

            #region ~/RNs/{RoamingNetworkId}/ChargeDetailRecords

            #endregion

            #region ~/RNs/{RoamingNetworkId}/ChargeDetailRecords/{ChargeDetailRecordId}

            #endregion



            #region ~/RNs/{RoamingNetworkId}/AuthStartCache

            //ToDo: OPTIONS

            #region GET    ~/RNs/{RoamingNetworkId}/AuthStartCache

            // -------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/AuthStartCache
            // -------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/AuthStartCache",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: async request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return httpResponseBuilder.AsImmutable;
                    }

                    #endregion

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out     httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion


                    var withMetadata              = request.QueryString.GetBoolean("withMetadata", false);

                    var includeFilter             = request.QueryString.CreateStringFilter<AuthenticationToken>("match", (authenticationToken, include) => authenticationToken.ToString().Contains(include));

                    var allAuthStartResults       = roamingNetwork.CachedAuthStartResults.
                                                        ToArray();

                    var totalCount                = allAuthStartResults.ULongLength();

                    var filteredAuthStartResults  = allAuthStartResults.
                                                        Where            (authStartResultKeyValuePair => includeFilter(authStartResultKeyValuePair.Key)).
                                                        OrderByDescending(authStartResultKeyValuePair => authStartResultKeyValuePair.Value.CachedResultEndOfLifeTime).
                                                        Skip             (request.QueryString.GetUInt64("skip")).
                                                        Take             (request.QueryString.GetUInt32("take")).
                                                        Select           (authStartResultKeyValuePair => {
                                                                            var authStartResultInfo = authStartResultKeyValuePair.Value.ToJSON(Embedded: true);
                                                                            authStartResultInfo.Add("authenticationToken", authStartResultKeyValuePair.Key.ToString());
                                                                            return authStartResultInfo;
                                                                        }).
                                                        ToArray          ();

                    var filteredCount             = filteredAuthStartResults.ULongLength();

                    var jsonResults               = new JArray(filteredAuthStartResults);


                    return new HTTPResponse.Builder(request) {
                                HTTPStatusCode                = HTTPStatusCode.OK,
                                Server                        = HTTPServiceName,
                                Date                          = Timestamp.Now,
                                AccessControlAllowOrigin      = "*",
                                AccessControlAllowMethods     = [ "GET", "COUNT", "CLEAR" ],
                                AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                   = HTTPContentType.Application.JSON_UTF8,
                                Content                       = withMetadata
                                                                    ? JSONObject.Create(
                                                                        new JProperty("totalCount",        totalCount),
                                                                        new JProperty("filteredCount",     filteredCount),
                                                                        new JProperty("authStartResults",  jsonResults)
                                                                    ).ToUTF8Bytes()
                                                                    : jsonResults.ToUTF8Bytes(),
                                X_ExpectedTotalNumberOfItems  = filteredCount,
                                Connection                    = ConnectionType.KeepAlive,
                                Vary                          = "Accept"
                            };

                });

            #endregion

            #region COUNT  ~/RNs/{RoamingNetworkId}/AuthStartCache

            // -------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/AuthStartCache
            // -------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.COUNT,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/AuthStartCache",
                HTTPContentType.Text.PLAIN,
                HTTPDelegate:  async request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return httpResponseBuilder.AsImmutable;
                    }

                    #endregion

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out     httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion


                    return new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.OK,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET", "COUNT", "CLEAR" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                = HTTPContentType.Text.PLAIN,
                                Content                    = roamingNetwork.CachedAuthStartResults.Count().ToString().ToUTF8Bytes()
                            };

                });

            #endregion

            #region CLEAR  ~/RNs/{RoamingNetworkId}/AuthStartCache

            // -------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/AuthStartCache
            // -------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.CLEAR,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/AuthStartCache",
                HTTPDelegate: async request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return httpResponseBuilder.AsImmutable;
                    }

                    #endregion

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out     httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion


                    await roamingNetwork.ClearAuthStartResultCache();


                    return new HTTPResponse.Builder(request) {
                               HTTPStatusCode             = HTTPStatusCode.OK,
                               Server                     = HTTPServiceName,
                               Date                       = Timestamp.Now,
                               AccessControlAllowOrigin   = "*",
                               AccessControlAllowMethods  = [ "GET", "COUNT", "CLEAR" ],
                               AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                           };

                });

            #endregion

            #endregion

            #region ~/RNs/{RoamingNetworkId}/AuthStopCache

            //ToDo: OPTIONS

            #region GET    ~/RNs/{RoamingNetworkId}/AuthStopCache

            // ------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/AuthStopCache
            // ------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/AuthStopCache",
                HTTPContentType.Application.JSON_UTF8,
                HTTPDelegate: async request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return httpResponseBuilder.AsImmutable;
                    }

                    #endregion

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out     httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion


                    var withMetadata             = request.QueryString.GetBoolean("withMetadata", false);

                    var includeFilter            = request.QueryString.CreateStringFilter<AuthenticationToken>("match", (authenticationToken, include) => authenticationToken.ToString().Contains(include));

                    var allAuthStopResults       = roamingNetwork.CachedAuthStopResults.
                                                       ToArray();

                    var totalCount               = allAuthStopResults.ULongLength();

                    var filteredAuthStopResults  = allAuthStopResults.
                                                        Where            (authStopResultKeyValuePair => includeFilter(authStopResultKeyValuePair.Key)).
                                                        OrderByDescending(authStopResultKeyValuePair => authStopResultKeyValuePair.Value.CachedResultEndOfLifeTime).
                                                        Skip             (request.QueryString.GetUInt64("skip")).
                                                        Take             (request.QueryString.GetUInt32("take")).
                                                        Select           (authStopResultKeyValuePair => {
                                                                             var authStopResultInfo = authStopResultKeyValuePair.Value.ToJSON(Embedded: true);
                                                                             authStopResultInfo.Add("authenticationToken", authStopResultKeyValuePair.Key.ToString());
                                                                             return authStopResultInfo;
                                                                         }).
                                                        ToArray          ();

                    var filteredCount            = filteredAuthStopResults.ULongLength();

                    var jsonResults              = new JArray(filteredAuthStopResults);


                    return new HTTPResponse.Builder(request) {
                                HTTPStatusCode                = HTTPStatusCode.OK,
                                Server                        = HTTPServiceName,
                                Date                          = Timestamp.Now,
                                AccessControlAllowOrigin      = "*",
                                AccessControlAllowMethods     = [ "GET", "COUNT", "CLEAR" ],
                                AccessControlAllowHeaders     = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                   = HTTPContentType.Application.JSON_UTF8,
                                Content                       = withMetadata
                                                                    ? JSONObject.Create(
                                                                        new JProperty("totalCount",        totalCount),
                                                                        new JProperty("filteredCount",     filteredCount),
                                                                        new JProperty("authStopResults",  jsonResults)
                                                                    ).ToUTF8Bytes()
                                                                    : jsonResults.ToUTF8Bytes(),
                                X_ExpectedTotalNumberOfItems  = filteredCount,
                                Connection                    = ConnectionType.KeepAlive,
                                Vary                          = "Accept"
                            };

                });

            #endregion

            #region COUNT  ~/RNs/{RoamingNetworkId}/AuthStopCache

            // -------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/AuthStopCache
            // -------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.COUNT,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/AuthStopCache",
                HTTPContentType.Text.PLAIN,
                HTTPDelegate:  async request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return httpResponseBuilder.AsImmutable;
                    }

                    #endregion

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out     httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion


                    return new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.OK,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET", "COUNT", "CLEAR" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                = HTTPContentType.Text.PLAIN,
                                Content                    = roamingNetwork.CachedAuthStopResults.Count().ToString().ToUTF8Bytes()
                            };

                });

            #endregion

            #region CLEAR  ~/RNs/{RoamingNetworkId}/AuthStopCache

            // -------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Test/AuthStopCache
            // -------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.CLEAR,
                URLPathPrefix + "/RNs/{RoamingNetworkId}/AuthStopCache",
                HTTPDelegate: async request => {

                    #region Get HTTP user and its organizations

                    // Will return HTTP 401 Unauthorized, when the HTTP user is unknown!
                    if (!HTTPBaseAPI.TryGetHTTPUser(request,
                                                    out var httpUser,
                                                    out var httpOrganizations,
                                                    out var httpResponseBuilder,
                                                    Recursive: true))
                    {
                        return httpResponseBuilder.AsImmutable;
                    }

                    #endregion

                    #region Check parameters

                    if (!request.TryParseRoamingNetwork(this,
                                                        out var roamingNetwork,
                                                        out     httpResponseBuilder))
                    {
                        return httpResponseBuilder;
                    }

                    #endregion


                    await roamingNetwork.ClearAuthStopResultCache();


                    return new HTTPResponse.Builder(request) {
                               HTTPStatusCode             = HTTPStatusCode.OK,
                               Server                     = HTTPServiceName,
                               Date                       = Timestamp.Now,
                               AccessControlAllowOrigin   = "*",
                               AccessControlAllowMethods  = [ "GET", "COUNT", "CLEAR" ],
                               AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                           };

                });

            #endregion

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
                            Content                    = "https://github.com/OpenChargingCloud/WWCP_Core".ToUTF8Bytes(),
                            Connection                 = ConnectionType.KeepAlive,
                            Vary                       = "Accept"
                        }.AsImmutable
                    )

            );

            #endregion

        }

        #endregion


        #region CreateNewRoamingNetwork (Id, Name, Description = null, Configurator = null, ...)

        /// <summary>
        /// Create and register a new roaming network collection
        /// for the given HTTP hostname.
        /// </summary>
        /// <param name="Hostname">A HTTP hostname.</param>
        /// <param name="Id">The unique identification of the new roaming network.</param>
        /// <param name="Name">The multi-language name of the roaming network.</param>
        /// <param name="Description">A multilanguage description of the roaming networks object.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming network after its creation.</param>
        /// <param name="AdminStatus">The initial admin status of the roaming network.</param>
        /// <param name="Status">The initial status of the roaming network.</param>
        /// <param name="MaxAdminStatusListSize">The maximum number of entries in the admin status history.</param>
        /// <param name="MaxStatusListSize">The maximum number of entries in the status history.</param>
        /// <param name="ChargingStationSignatureGenerator">A delegate to sign a charging station.</param>
        /// <param name="ChargingPoolSignatureGenerator">A delegate to sign a charging pool.</param>
        /// <param name="ChargingStationOperatorSignatureGenerator">A delegate to sign a charging station operator.</param>
        public IRoamingNetwork CreateNewRoamingNetwork(RoamingNetwork_Id                          Id,
                                                       I18NString                                 Name,
                                                       I18NString?                                Description                                  = null,
                                                       Action<RoamingNetwork>?                    Configurator                                 = null,
                                                       RoamingNetworkAdminStatusTypes?            AdminStatus                                  = null,
                                                       RoamingNetworkStatusTypes?                 Status                                       = null,
                                                       UInt16?                                    MaxAdminStatusListSize                       = null,
                                                       UInt16?                                    MaxStatusListSize                            = null,

                                                       Boolean?                                   DisableAuthenticationCache                   = false,
                                                       TimeSpan?                                  AuthenticationCacheTimeout                   = null,
                                                       UInt32?                                    MaxAuthStartResultCacheElements              = null,
                                                       UInt32?                                    MaxAuthStopResultCacheElements               = null,

                                                       Boolean?                                   DisableAuthenticationRateLimit               = true,
                                                       TimeSpan?                                  AuthenticationRateLimitTimeSpan              = null,
                                                       UInt16?                                    AuthenticationRateLimitPerChargingLocation   = null,

                                                       TimeSpan?                                  RequestTimeout                               = null,

                                                       ChargingStationSignatureDelegate?          ChargingStationSignatureGenerator            = null,
                                                       ChargingPoolSignatureDelegate?             ChargingPoolSignatureGenerator               = null,
                                                       ChargingStationOperatorSignatureDelegate?  ChargingStationOperatorSignatureGenerator    = null,

                                                       IEnumerable<RoamingNetworkInfo>?           RoamingNetworkInfos                          = null,
                                                       Boolean                                    DisableNetworkSync                           = false,

                                                       HTTPHostname?                              Hostname                                     = null)

        {

            if (WWCPCores.TryGetValue         (Hostname ?? HTTPHostname.Any, out var wwcpCore) &&
               !wwcpCore. TryGetRoamingNetwork(Id,                           out var existingRoamingNetwork))
            {

                //if (!WWCPHTTPServer.TryGetTenants(out RoamingNetworks _RoamingNetworks))
                //{

                //    _RoamingNetworks = new RoamingNetworks();

                //    if (!WWCPHTTPServer.TryAddTenants( _RoamingNetworks))
                //        throw new Exception("Could not add new roaming networks object to the HTTP host!");

                //}

                var newRoamingNetwork = wwcpCore.CreateNewRoamingNetwork(
                                            Id,
                                            Name,
                                            Description,
                                            Configurator,
                                            AdminStatus,
                                            Status,
                                            MaxAdminStatusListSize,
                                            MaxStatusListSize,

                                            DisableAuthenticationCache,
                                            AuthenticationCacheTimeout,
                                            MaxAuthStartResultCacheElements,
                                            MaxAuthStopResultCacheElements,

                                            DisableAuthenticationRateLimit,
                                            AuthenticationRateLimitTimeSpan,
                                            AuthenticationRateLimitPerChargingLocation,

                                            RequestTimeout,

                                            ChargingStationSignatureGenerator,
                                            ChargingPoolSignatureGenerator,
                                            ChargingStationOperatorSignatureGenerator,

                                            RoamingNetworkInfos,
                                            DisableNetworkSync
                                            //OpenChargingCloudAPIPath
                                        );

                #region Link log events to HTTP-SSE...

                #region OnAuthorizeStartRequest/-Response

                //newRoamingNetwork.OnAuthorizeStartRequest += async (LogTimestamp,
                //                                                    RequestTimestamp,
                //                                                    Sender,
                //                                                    SenderId,
                //                                                    EventTrackingId,
                //                                                    RoamingNetworkId,
                //                                                    EMPRoamingProviderId,
                //                                                    CSORoamingProviderId,
                //                                                    OperatorId,
                //                                                    Authentication,
                //                                                    ChargingLocation,
                //                                                    ChargingProduct,
                //                                                    SessionId,
                //                                                    CPOPartnerSessionId,
                //                                                    ISendAuthorizeStartStop,
                //                                                    RequestTimeout)

                //    => await DebugLog.SubmitEvent("AUTHSTARTRequest",
                //                                  JSONObject.Create(
                //                                      new JProperty("timestamp",                   RequestTimestamp.    ToISO8601()),
                //                                      new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                //                                      new JProperty("roamingNetworkId",            RoamingNetworkId.    ToString()),
                //                                      EMPRoamingProviderId.HasValue
                //                                          ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                //                                          : null,
                //                                      CSORoamingProviderId.HasValue
                //                                          ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                //                                          : null,
                //                                      OperatorId.    HasValue
                //                                          ? new JProperty("operatorId",            OperatorId.          ToString())
                //                                          : null,
                //                                      Authentication is not null
                //                                          ? new JProperty("authentication",        Authentication.      ToJSON())
                //                                          : null,
                //                                      ChargingLocation.IsDefined()
                //                                          ? new JProperty("chargingLocation",      ChargingLocation.    ToJSON())
                //                                          : null,
                //                                      ChargingProduct is not null
                //                                          ? new JProperty("chargingProduct",       ChargingProduct.     ToJSON())
                //                                          : null,
                //                                      SessionId.     HasValue
                //                                          ? new JProperty("sessionId",             SessionId.           ToString())
                //                                          : null,
                //                                      CPOPartnerSessionId.HasValue
                //                                          ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId. ToString())
                //                                          : null,
                //                                      RequestTimeout.HasValue
                //                                          ? new JProperty("requestTimeout",        Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                //                                          : null
                //                               ));


                //newRoamingNetwork.OnAuthorizeStartResponse += async (LogTimestamp,
                //                                                     RequestTimestamp,
                //                                                     Sender,
                //                                                     SenderId,
                //                                                     EventTrackingId,
                //                                                     RoamingNetworkId2,
                //                                                     EMPRoamingProviderId,
                //                                                     CSORoamingProviderId,
                //                                                     OperatorId,
                //                                                     Authentication,
                //                                                     ChargingLocation,
                //                                                     ChargingProduct,
                //                                                     SessionId,
                //                                                     CPOPartnerSessionId,
                //                                                     ISendAuthorizeStartStop,
                //                                                     RequestTimeout,
                //                                                     Result,
                //                                                     Runtime)

                //    => await DebugLog.SubmitEvent("AUTHSTARTResponse",
                //                                  JSONObject.Create(
                //                                      new JProperty("timestamp",                   RequestTimestamp.    ToISO8601()),
                //                                      new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                //                                      new JProperty("roamingNetworkId",            RoamingNetworkId2.   ToString()),
                //                                      EMPRoamingProviderId.HasValue
                //                                          ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                //                                          : null,
                //                                      CSORoamingProviderId.HasValue
                //                                          ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                //                                          : null,
                //                                      OperatorId.HasValue
                //                                          ? new JProperty("operatorId",            OperatorId.          ToString())
                //                                          : null,
                //                                      new JProperty("authentication",              Authentication.      ToJSON()),
                //                                      ChargingLocation.IsDefined()
                //                                          ? new JProperty("chargingLocation",      ChargingLocation.    ToJSON())
                //                                          : null,
                //                                      ChargingProduct is not null
                //                                          ? new JProperty("chargingProduct",       ChargingProduct.     ToJSON())
                //                                          : null,
                //                                      SessionId.HasValue
                //                                          ? new JProperty("sessionId",             SessionId.           ToString())
                //                                          : null,
                //                                      CPOPartnerSessionId.HasValue
                //                                          ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId. ToString())
                //                                          : null,
                //                                      RequestTimeout.HasValue
                //                                          ? new JProperty("requestTimeout",        Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                //                                          : null,

                //                                      new JProperty("result",                      Result.              ToJSON()),
                //                                      new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds, 0))

                //                                  ));

                #endregion

                #region OnAuthorizeStopRequest/-Response

                //newRoamingNetwork.OnAuthorizeStopRequest += async (LogTimestamp,
                //                                                   RequestTimestamp,
                //                                                   Sender,
                //                                                   SenderId,
                //                                                   EventTrackingId,
                //                                                   RoamingNetworkId2,
                //                                                   EMPRoamingProviderId,
                //                                                   CSORoamingProviderId,
                //                                                   OperatorId,
                //                                                   ChargingLocation,
                //                                                   SessionId,
                //                                                   CPOPartnerSessionId,
                //                                                   Authentication,
                //                                                   RequestTimeout)

                //    => await DebugLog.SubmitEvent("AUTHSTOPRequest",
                //                                  JSONObject.Create(
                //                                      new JProperty("timestamp",                   RequestTimestamp.    ToISO8601()),
                //                                      new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                //                                      new JProperty("roamingNetworkId",            RoamingNetworkId2.   ToString()),
                //                                      EMPRoamingProviderId.HasValue
                //                                          ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                //                                          : null,
                //                                      CSORoamingProviderId.HasValue
                //                                          ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                //                                          : null,
                //                                      OperatorId is not null
                //                                          ? new JProperty("operatorId",            OperatorId.          ToString())
                //                                          : null,
                //                                      ChargingLocation.IsDefined()
                //                                          ? new JProperty("chargingLocation",      ChargingLocation.    ToJSON())
                //                                          : null,
                //                                      new JProperty("sessionId",                   SessionId.           ToString()),
                //                                      CPOPartnerSessionId.HasValue
                //                                          ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId. ToString())
                //                                          : null,
                //                                      new JProperty("authentication",              Authentication.      ToString()),
                //                                      RequestTimeout.HasValue
                //                                          ? new JProperty("requestTimeout",        Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                //                                          : null
                //                                  ));

                //newRoamingNetwork.OnAuthorizeStopResponse += async (LogTimestamp,
                //                                                    RequestTimestamp,
                //                                                    Sender,
                //                                                    SenderId,
                //                                                    EventTrackingId,
                //                                                    RoamingNetworkId2,
                //                                                    EMPRoamingProviderId,
                //                                                    CSORoamingProviderId,
                //                                                    OperatorId,
                //                                                    ChargingLocation,
                //                                                    SessionId,
                //                                                    CPOPartnerSessionId,
                //                                                    Authentication,
                //                                                    RequestTimeout,
                //                                                    Result,
                //                                                    Runtime)

                //    => await DebugLog.SubmitEvent("AUTHSTOPResponse",
                //                                  JSONObject.Create(

                //                                      new JProperty("timestamp",                   RequestTimestamp.    ToISO8601()),
                //                                      new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                //                                      new JProperty("roamingNetworkId",            RoamingNetworkId2.   ToString()),
                //                                      EMPRoamingProviderId.HasValue
                //                                          ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                //                                          : null,
                //                                      CSORoamingProviderId.HasValue
                //                                          ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                //                                          : null,
                //                                      OperatorId.HasValue
                //                                          ? new JProperty("operatorId",            OperatorId.          ToString())
                //                                          : null,
                //                                      ChargingLocation.IsDefined()
                //                                          ? new JProperty("chargingLocation",      ChargingLocation.    ToJSON())
                //                                          : null,
                //                                      SessionId.HasValue
                //                                          ? new JProperty("sessionId",             SessionId.           ToString())
                //                                          : null,
                //                                      CPOPartnerSessionId.HasValue
                //                                          ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId. ToString())
                //                                          : null,
                //                                      new JProperty("authentication",              Authentication.      ToString()),
                //                                      RequestTimeout.HasValue
                //                                          ? new JProperty("requestTimeout",        Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                //                                          : null,

                //                                      new JProperty("result",                      Result.              ToJSON()),
                //                                      new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds, 0))

                //                              ));

                #endregion


                #region OnReserveEVSERequest/-Response

                //newRoamingNetwork.OnReserveRequest += async (LogTimestamp,
                //                                             Timestamp,
                //                                             Sender,
                //                                             EventTrackingId,
                //                                             RoamingNetworkId2,
                //                                             ReservationId,
                //                                             LinkedReservationId,
                //                                             ChargingLocation,
                //                                             StartTime,
                //                                             Duration,
                //                                             ProviderId,
                //                                             eMAId,
                //                                             ChargingProduct,
                //                                             AuthTokens,
                //                                             eMAIds,
                //                                             PINs,
                //                                             RequestTimeout)

                //    => await DebugLog.SubmitEvent("OnReserveRequest",
                //                                  JSONObject.Create(
                //                                      new JProperty("Timestamp",                 Timestamp.ToISO8601()),
                //                                      EventTrackingId is not null
                //                                         ? new JProperty("EventTrackingId",      EventTrackingId.ToString())
                //                                         : null,
                //                                      new JProperty("RoamingNetwork",            Id.ToString()),
                //                                      ReservationId.HasValue
                //                                         ? new JProperty("ReservationId",        ReservationId.ToString())
                //                                         : null,
                //                                      LinkedReservationId.HasValue
                //                                         ? new JProperty("LinkedReservationId",  LinkedReservationId.ToString())
                //                                         : null,
                //                                      ChargingLocation is not null
                //                                          ? new JProperty("ChargingLocation",    ChargingLocation.ToString())
                //                                          : null,
                //                                      StartTime.HasValue
                //                                          ? new JProperty("StartTime",           StartTime.Value.ToISO8601())
                //                                          : null,
                //                                      Duration.HasValue
                //                                          ? new JProperty("Duration",            Duration.Value.TotalSeconds.ToString())
                //                                          : null,
                //                                      ProviderId.HasValue
                //                                          ? new JProperty("ProviderId",          ProviderId.ToString())
                //                                          : null,
                //                                      eMAId is not null
                //                                          ? new JProperty("eMAId",               eMAId.ToString())
                //                                          : null,
                //                                      ChargingProduct is not null
                //                                          ? new JProperty("ChargingProduct",     JSONObject.Create(
                //                                                new JProperty("Id",                              ChargingProduct.Id.ToString()),
                //                                                ChargingProduct.MinDuration.HasValue
                //                                                    ? new JProperty("MinDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                //                                                    : null,
                //                                                ChargingProduct.StopChargingAfterTime.HasValue
                //                                                    ? new JProperty("StopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                //                                                    : null,
                //                                                ChargingProduct.MinPower.HasValue
                //                                                    ? new JProperty("MinPower",                  ChargingProduct.MinPower.Value)
                //                                                    : null,
                //                                                ChargingProduct.MaxPower.HasValue
                //                                                    ? new JProperty("MaxPower",                  ChargingProduct.MaxPower.Value)
                //                                                    : null,
                //                                                ChargingProduct.MinEnergy.HasValue
                //                                                    ? new JProperty("MinEnergy",                 ChargingProduct.MinEnergy.Value)
                //                                                    : null,
                //                                                ChargingProduct.StopChargingAfterKWh.HasValue
                //                                                    ? new JProperty("StopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                //                                                    : null
                //                                               ))
                //                                          : null,
                //                                      AuthTokens is not null
                //                                          ? new JProperty("AuthTokens",          new JArray(AuthTokens.Select(_ => _.ToString())))
                //                                          : null,
                //                                      eMAIds is not null
                //                                          ? new JProperty("eMAIds",              new JArray(eMAIds.Select(_ => _.ToString())))
                //                                          : null,
                //                                      PINs is not null
                //                                          ? new JProperty("PINs",                new JArray(PINs.Select(_ => _.ToString())))
                //                                          : null
                //                                  ));

                //newRoamingNetwork.OnReserveResponse += async (LogTimestamp,
                //                                              Timestamp,
                //                                              Sender,
                //                                              EventTrackingId,
                //                                              RoamingNetworkId2,
                //                                              ReservationId,
                //                                              LinkedReservationId,
                //                                              ChargingLocation,
                //                                              StartTime,
                //                                              Duration,
                //                                              ProviderId,
                //                                              eMAId,
                //                                              ChargingProduct,
                //                                              AuthTokens,
                //                                              eMAIds,
                //                                              PINs,
                //                                              Result,
                //                                              Runtime,
                //                                              RequestTimeout)

                //    => await DebugLog.SubmitEvent("OnReserveResponse",
                //                                  JSONObject.Create(
                //                                      new JProperty("Timestamp",                 Timestamp.ToISO8601()),
                //                                        EventTrackingId is not null
                //                                           ? new JProperty("EventTrackingId",      EventTrackingId.ToString())
                //                                           : null,
                //                                        new JProperty("RoamingNetwork",            Id.ToString()),
                //                                        ReservationId.HasValue
                //                                           ? new JProperty("ReservationId",        ReservationId.ToString())
                //                                           : null,
                //                                        LinkedReservationId.HasValue
                //                                           ? new JProperty("LinkedReservationId",  LinkedReservationId.ToString())
                //                                           : null,
                //                                        ChargingLocation is not null
                //                                            ? new JProperty("ChargingLocation",    ChargingLocation.ToString())
                //                                            : null,
                //                                        StartTime.HasValue
                //                                            ? new JProperty("StartTime",           StartTime.Value.ToISO8601())
                //                                            : null,
                //                                        Duration.HasValue
                //                                            ? new JProperty("Duration",            Duration.Value.TotalSeconds.ToString())
                //                                            : null,
                //                                        ProviderId is not null
                //                                            ? new JProperty("ProviderId",          ProviderId.ToString()+"X")
                //                                            : null,
                //                                        eMAId is not null
                //                                            ? new JProperty("eMAId",               eMAId.ToString())
                //                                            : null,
                //                                        ChargingProduct is not null
                //                                          ? new JProperty("ChargingProduct",     JSONObject.Create(
                //                                                new JProperty("Id",                              ChargingProduct.Id.ToString()),
                //                                                ChargingProduct.MinDuration.HasValue
                //                                                    ? new JProperty("MinDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                //                                                    : null,
                //                                                ChargingProduct.StopChargingAfterTime.HasValue
                //                                                    ? new JProperty("StopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                //                                                    : null,
                //                                                ChargingProduct.MinPower.HasValue
                //                                                    ? new JProperty("MinPower",                  ChargingProduct.MinPower.Value)
                //                                                    : null,
                //                                                ChargingProduct.MaxPower.HasValue
                //                                                    ? new JProperty("MaxPower",                  ChargingProduct.MaxPower.Value)
                //                                                    : null,
                //                                                ChargingProduct.MinEnergy.HasValue
                //                                                    ? new JProperty("MinEnergy",                 ChargingProduct.MinEnergy.Value)
                //                                                    : null,
                //                                                ChargingProduct.StopChargingAfterKWh.HasValue
                //                                                    ? new JProperty("StopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                //                                                    : null
                //                                               ))
                //                                          : null,
                //                                        AuthTokens is not null
                //                                            ? new JProperty("AuthTokens",          new JArray(AuthTokens.Select(_ => _.ToString())))
                //                                            : null,
                //                                        eMAIds is not null
                //                                            ? new JProperty("eMAIds",              new JArray(eMAIds.Select(_ => _.ToString())))
                //                                            : null,
                //                                        PINs is not null
                //                                            ? new JProperty("PINs",                new JArray(PINs.Select(_ => _.ToString())))
                //                                            : null,
                //                                        new JProperty("Result",                    Result.Result.ToString()),
                //                                        Result.Message.IsNotNullOrEmpty()
                //                                            ? new JProperty("ErrorMessage",        Result.Message)
                //                                            : null,
                //                                        new JProperty("Runtime",                   Math.Round(Runtime.TotalMilliseconds, 0))
                //                                  ));

                #endregion

                #region OnCancelReservationResponse

                //newRoamingNetwork.OnCancelReservationResponse += async (LogTimestamp,
                //                                                   Timestamp,
                //                                                   Sender,
                //                                                   EventTrackingId,
                //                                                   RoamingNetworkId,
                //                                                   //ProviderId,
                //                                                   ReservationId,
                //                                                   Reservation,
                //                                                   Reason,
                //                                                   Result,
                //                                                   Runtime,
                //                                                   RequestTimeout)

                //    => await DebugLog.SubmitEvent("OnCancelReservation",
                //                                  JSONObject.Create(
                //                                      new JProperty("Timestamp",                Timestamp.ToISO8601()),
                //                                      EventTrackingId is not null
                //                                          ? new JProperty("EventTrackingId",    EventTrackingId.ToString())
                //                                          : null,
                //                                      new JProperty("ReservationId",            ReservationId.ToString()),

                //                                      new JProperty("RoamingNetwork",           RoamingNetworkId.ToString()),

                //                                      Reservation?.EVSEId is not null
                //                                          ? new JProperty("EVSEId",             Reservation.EVSEId.ToString())
                //                                          : null,
                //                                      Reservation?.ChargingStationId is not null
                //                                          ? new JProperty("ChargingStationId",  Reservation.ChargingStationId.ToString())
                //                                          : null,
                //                                      Reservation?.ChargingPoolId is not null
                //                                          ? new JProperty("ChargingPoolId",     Reservation.EVSEId.ToString())
                //                                          : null,

                //                                      new JProperty("Reason",                   Reason.ToString()),

                //                                      new JProperty("Result",                   Result.Result.ToString()),
                //                                      new JProperty("Message",                  Result.Message),
                //                                      new JProperty("AdditionalInfo",           Result.AdditionalInfo),
                //                                      new JProperty("Runtime",                  Result.Runtime)

                //                                  ));

                ////ToDo: OnCancelReservationResponse Result!

                #endregion


                #region OnRemoteStartRequest/-Response

                //newRoamingNetwork.OnRemoteStartRequest += async (LogTimestamp,
                //                                                 Timestamp,
                //                                                 Sender,
                //                                                 EventTrackingId,
                //                                                 RoamingNetworkId,
                //                                                 ChargingLocation,
                //                                                 remoteAuthentication,
                //                                                 SessionId,
                //                                                 ReservationId,
                //                                                 ChargingProduct,
                //                                                 EMPRoamingProviderId,
                //                                                 CSORoamingProviderId,
                //                                                 ProviderId,
                //                                                 RequestTimeout)

                //    => await DebugLog.SubmitEvent("OnRemoteStartRequest",
                //                                  JSONObject.Create(
                //                                      new JProperty("timestamp",                   Timestamp.           ToISO8601()),
                //                                      EventTrackingId is not null
                //                                          ? new JProperty("eventTrackingId",       EventTrackingId.     ToString())
                //                                          : null,
                //                                      new JProperty("roamingNetworkId",            RoamingNetworkId.    ToString()),
                //                                      ChargingLocation.IsDefined()
                //                                          ? new JProperty("chargingLocation",      ChargingLocation.    ToJSON())
                //                                          : null,
                //                                      ChargingProduct is not null
                //                                          ? new JProperty("chargingProduct",       ChargingProduct.     ToJSON())
                //                                          : null,
                //                                      ReservationId.HasValue
                //                                          ? new JProperty("reservationId",         ReservationId.       ToString())
                //                                          : null,
                //                                      SessionId.HasValue
                //                                          ? new JProperty("sessionId",             SessionId.           ToString())
                //                                          : null,
                //                                      EMPRoamingProviderId.HasValue
                //                                          ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                //                                          : null,
                //                                      CSORoamingProviderId.HasValue
                //                                          ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                //                                          : null,
                //                                      ProviderId.HasValue
                //                                          ? new JProperty("providerId",            ProviderId.          ToString())
                //                                          : null,
                //                                      remoteAuthentication is not null
                //                                          ? new JProperty("authentication",        remoteAuthentication.ToJSON())
                //                                          : null,
                //                                      RequestTimeout.HasValue
                //                                          ? new JProperty("requestTimeout",        Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                //                                          : null
                //                                  ));

                //newRoamingNetwork.OnRemoteStartResponse += async (LogTimestamp,
                //                                                  Timestamp,
                //                                                  Sender,
                //                                                  EventTrackingId,
                //                                                  RoamingNetworkId,
                //                                                  ChargingLocation,
                //                                                  remoteAuthentication,
                //                                                  SessionId,
                //                                                  ReservationId,
                //                                                  ChargingProduct,
                //                                                  EMPRoamingProviderId,
                //                                                  CSORoamingProviderId,
                //                                                  ProviderId,
                //                                                  RequestTimeout,
                //                                                  Result,
                //                                                  Runtime)

                //    => await DebugLog.SubmitEvent("OnRemoteStartResponse",
                //                                  JSONObject.Create(
                //                                      new JProperty("timestamp",                   Timestamp.           ToISO8601()),
                //                                      EventTrackingId      is not null
                //                                          ? new JProperty("eventTrackingId",       EventTrackingId.     ToString())
                //                                          : null,
                //                                      new JProperty("roamingNetworkId",            RoamingNetworkId.    ToString()),
                //                                      ChargingLocation.IsDefined()
                //                                          ? new JProperty("chargingLocation",      ChargingLocation.    ToJSON())
                //                                          : null,
                //                                      ChargingProduct      is not null
                //                                          ? new JProperty("chargingProduct",       ChargingProduct.     ToJSON())
                //                                          : null,
                //                                      ReservationId        is not null
                //                                          ? new JProperty("reservationId",         ReservationId.       ToString())
                //                                          : null,
                //                                      SessionId            is not null
                //                                          ? new JProperty("sessionId",             SessionId.           ToString())
                //                                          : null,
                //                                      EMPRoamingProviderId.HasValue
                //                                          ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                //                                          : null,
                //                                      CSORoamingProviderId.HasValue
                //                                          ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                //                                          : null,
                //                                      ProviderId           is not null
                //                                          ? new JProperty("providerId",            ProviderId.          ToString())
                //                                          : null,
                //                                      remoteAuthentication is not null
                //                                          ? new JProperty("authentication",        remoteAuthentication.ToJSON())
                //                                          : null,
                //                                      RequestTimeout.HasValue
                //                                          ? new JProperty("requestTimeout",        Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                //                                          : null,
                //                                      new JProperty("result",                      Result.              ToJSON()),
                //                                      new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds, 0))
                //                                  ));

                #endregion

                #region OnRemoteStopRequest/-Response

                //newRoamingNetwork.OnRemoteStopRequest += async (LogTimestamp,
                //                                                Timestamp,
                //                                                Sender,
                //                                                EventTrackingId,
                //                                                RoamingNetworkId,
                //                                                SessionId,
                //                                                ReservationHandling,
                //                                                EMPRoamingProviderId,
                //                                                CSORoamingProviderId,
                //                                                ProviderId,
                //                                                Authentication,
                //                                                RequestTimeout)

                //    => await DebugLog.SubmitEvent("OnRemoteStopRequest",
                //                                  JSONObject.Create(
                //                                      new JProperty("timestamp",                   Timestamp.           ToISO8601()),
                //                                      EventTrackingId is not null
                //                                          ? new JProperty("eventTrackingId",       EventTrackingId.     ToString())
                //                                          : null,
                //                                      new JProperty("roamingNetworkId",            RoamingNetworkId.    ToString()),
                //                                      new JProperty("sessionId",                   SessionId.           ToString()),
                //                                      ReservationHandling.HasValue
                //                                          ? new JProperty("reservationHandling",   ReservationHandling. ToString())
                //                                          : null,
                //                                      EMPRoamingProviderId.HasValue
                //                                          ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                //                                          : null,
                //                                      CSORoamingProviderId.HasValue
                //                                          ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                //                                          : null,
                //                                      ProviderId.HasValue
                //                                          ? new JProperty("providerId",            ProviderId.          ToString())
                //                                          : null,
                //                                      Authentication is not null
                //                                          ? new JProperty("authentication",        Authentication.      ToJSON())
                //                                          : null,
                //                                      RequestTimeout.HasValue
                //                                          ? new JProperty("requestTimeout",        Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                //                                          : null
                //                                  ));

                //newRoamingNetwork.OnRemoteStopResponse += async (LogTimestamp,
                //                                                 Timestamp,
                //                                                 Sender,
                //                                                 EventTrackingId,
                //                                                 RoamingNetworkId,
                //                                                 SessionId,
                //                                                 ReservationHandling,
                //                                                 EMPRoamingProviderId,
                //                                                 CSORoamingProviderId,
                //                                                 ProviderId,
                //                                                 Authentication,
                //                                                 RequestTimeout,
                //                                                 Result,
                //                                                 Runtime)

                //    => await DebugLog.SubmitEvent("OnRemoteStopResponse",
                //                                  JSONObject.Create(
                //                                      new JProperty("timestamp",                   Timestamp.           ToISO8601()),
                //                                      EventTrackingId is not null
                //                                          ? new JProperty("eventTrackingId",       EventTrackingId.     ToString())
                //                                          : null,
                //                                      new JProperty("roamingNetworkId",            RoamingNetworkId.    ToString()),
                //                                      new JProperty("sessionId",                   SessionId.           ToString()),
                //                                      ReservationHandling.HasValue
                //                                          ? new JProperty("reservationHandling",   ReservationHandling. ToString())
                //                                          : null,
                //                                      EMPRoamingProviderId.HasValue
                //                                          ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                //                                          : null,
                //                                      CSORoamingProviderId.HasValue
                //                                          ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                //                                          : null,
                //                                      ProviderId.HasValue
                //                                          ? new JProperty("providerId",            ProviderId.          ToString())
                //                                          : null,
                //                                      Authentication is not null
                //                                          ? new JProperty("authentication",        Authentication.      ToJSON())
                //                                          : null,
                //                                      RequestTimeout.HasValue
                //                                          ? new JProperty("requestTimeout",        Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                //                                          : null,
                //                                      new JProperty("result",                      Result.              ToJSON()),
                //                                      new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds, 0))
                //                                  ));

                #endregion


                #region OnSendCDRsRequest/-Response

                //newRoamingNetwork.OnSendCDRsRequest += async (LogTimestamp,
                //                                              RequestTimestamp,
                //                                              Sender,
                //                                              SenderId,
                //                                              EventTrackingId,
                //                                              RoamingNetworkId2,
                //                                              ChargeDetailRecords,
                //                                              RequestTimeout)


                //    => await DebugLog.SubmitEvent("OnSendCDRsRequest",
                //                                  JSONObject.Create(
                //                                      new JProperty("timestamp",                RequestTimestamp.  ToISO8601()),
                //                                      new JProperty("eventTrackingId",          EventTrackingId.   ToString()),
                //                                      new JProperty("roamingNetworkId",         RoamingNetworkId2. ToString()),
                //                                      //new JProperty("LogTimestamp",                     LogTimestamp.                                          ToISO8601()),
                //                                      //new JProperty("RequestTimestamp",                 RequestTimestamp.                                      ToISO8601()),

                //                                      new JProperty("chargeDetailRecords",              new JArray(
                //                                          ChargeDetailRecords.Select(ChargeDetailRecord => JSONObject.Create(

                //                                             new JProperty("@id",                              ChargeDetailRecord.Id.                                      ToString()),

                //                                             new JProperty("sessionId",                        ChargeDetailRecord.SessionId.                               ToString()),

                //                                             ChargeDetailRecord.SessionTime is not null
                //                                                 ? new JProperty("sessionStart",               ChargeDetailRecord.SessionTime.StartTime.                   ToISO8601())
                //                                                 : null,
                //                                             ChargeDetailRecord.SessionTime is not null && ChargeDetailRecord.SessionTime.EndTime.HasValue
                //                                                 ? new JProperty("sessionStop",                ChargeDetailRecord.SessionTime.EndTime.Value.               ToISO8601())
                //                                                 : null,

                //                                             ChargeDetailRecord.AuthenticationStart is not null
                //                                                 ? new JProperty("authenticationStart",        ChargeDetailRecord.AuthenticationStart.                     ToJSON())
                //                                                 : null,
                //                                             ChargeDetailRecord.AuthenticationStop is not null
                //                                                 ? new JProperty("authenticationStop",         ChargeDetailRecord.AuthenticationStop.                      ToJSON())
                //                                                 : null,
                //                                             ChargeDetailRecord.ProviderIdStart.HasValue
                //                                                 ? new JProperty("providerIdStart",            ChargeDetailRecord.ProviderIdStart.                         ToString())
                //                                                 : null,
                //                                             ChargeDetailRecord.ProviderIdStop.HasValue
                //                                                 ? new JProperty("providerIdStop",             ChargeDetailRecord.ProviderIdStop.                          ToString())
                //                                                 : null,

                //                                             ChargeDetailRecord.ReservationId.HasValue
                //                                                 ? new JProperty("reservationId",              ChargeDetailRecord.ReservationId.                           ToString())
                //                                                 : null,
                //                                             ChargeDetailRecord.ReservationTime is not null
                //                                                 ? new JProperty("reservationStart",           ChargeDetailRecord.ReservationTime.StartTime.               ToString())
                //                                                 : null,
                //                                             ChargeDetailRecord.ReservationTime is not null && ChargeDetailRecord.ReservationTime.EndTime.HasValue
                //                                                 ? new JProperty("reservationStop",            ChargeDetailRecord.ReservationTime.EndTime.Value.           ToISO8601())
                //                                                 : null,
                //                                             ChargeDetailRecord.Reservation is not null
                //                                                 ? new JProperty("reservationLevel",           ChargeDetailRecord.Reservation.ReservationLevel.            ToString())
                //                                                 : null,

                //                                             ChargeDetailRecord.ChargingStationOperator is not null
                //                                                 ? new JProperty("chargingStationOperator",    ChargeDetailRecord.ChargingStationOperator.                 ToString())
                //                                                 : null,

                //                                             ChargeDetailRecord.EVSE is not null
                //                                                 ? new JProperty("EVSEId",                     ChargeDetailRecord.EVSE.Id.                                 ToString())
                //                                                 : ChargeDetailRecord.EVSEId.HasValue
                //                                                       ? new JProperty("EVSEId",               ChargeDetailRecord.EVSEId.                                  ToString())
                //                                                       : null,

                //                                             ChargeDetailRecord.ChargingProduct is not null
                //                                                 ? new JProperty("chargingProduct",            ChargeDetailRecord.ChargingProduct.ToJSON())
                //                                                 : null,

                //                                             ChargeDetailRecord.EnergyMeterId.HasValue
                //                                                 ? new JProperty("energyMeterId",              ChargeDetailRecord.EnergyMeterId.                      ToString())
                //                                                 : null,
                //                                             ChargeDetailRecord.ConsumedEnergy.HasValue
                //                                                 ? new JProperty("consumedEnergy",             ChargeDetailRecord.ConsumedEnergy.Value.kWh)
                //                                                 : null,
                //                                             ChargeDetailRecord.EnergyMeteringValues.Any()
                //                                                 ? new JProperty("energyMeteringValues", JSONObject.Create(
                //                                                       ChargeDetailRecord.EnergyMeteringValues.Select(metervalue => new JProperty(metervalue.Timestamp.ToISO8601(),
                //                                                                                                                                  metervalue.WattHours.kWh)))
                //                                                   )
                //                                                 : null,
                //                                             //ChargeDetailRecord.MeteringSignature.IsNotNullOrEmpty()
                //                                             //    ? new JProperty("meteringSignature",          ChargeDetailRecord.MeteringSignature)
                //                                             //    : null,

                //                                             ChargeDetailRecord.ParkingSpaceId.HasValue
                //                                                 ? new JProperty("parkingSpaceId",             ChargeDetailRecord.ParkingSpaceId.                      ToString())
                //                                                 : null,
                //                                             ChargeDetailRecord.ParkingTime is not null
                //                                                 ? new JProperty("parkingTimeStart",           ChargeDetailRecord.ParkingTime.StartTime.               ToISO8601())
                //                                                 : null,
                //                                             ChargeDetailRecord.ParkingTime is not null && ChargeDetailRecord.ParkingTime.EndTime.HasValue
                //                                                 ? new JProperty("parkingTimeEnd",             ChargeDetailRecord.ParkingTime.EndTime.Value.           ToString())
                //                                                 : null,
                //                                             ChargeDetailRecord.ParkingFee.HasValue
                //                                                 ? new JProperty("parkingFee",                 ChargeDetailRecord.ParkingFee.                          ToString())
                //                                                 : null)

                //                                                 )
                //                                         )
                //                                     )

                //                                  ));

                #endregion


                #region OnEVSEData/(Admin)StatusChanged

                //newRoamingNetwork.OnEVSEDataChanged += async (Timestamp,
                //                                              EventTrackingId,
                //                                              EVSE,
                //                                              PropertyName,
                //                                              NewValue,
                //                                              OldValue,
                //                                              dataSource)

                //    => await DebugLog.SubmitEvent("OnEVSEDataChanged",
                //                                  JSONObject.Create(
                //                                      new JProperty("timestamp",        Timestamp.           ToISO8601()),
                //                                      new JProperty("eventTrackingId",  EventTrackingId.     ToString()),
                //                                      new JProperty("roamingNetworkId", newRoamingNetwork.Id.ToString()),
                //                                      new JProperty("EVSEId",           EVSE.Id.             ToString()),
                //                                      new JProperty("propertyName",     PropertyName),
                //                                      new JProperty("oldValue",         OldValue?.           ToString()),
                //                                      new JProperty("newValue",         NewValue?.           ToString())
                //                                  ));



                //newRoamingNetwork.OnEVSEStatusChanged += async (Timestamp,
                //                                                EventTrackingId,
                //                                                EVSE,
                //                                                NewStatus,
                //                                                OldStatus,
                //                                                dataSource)

                //    => await DebugLog.SubmitEvent("OnEVSEStatusChanged",
                //                                  JSONObject.Create(
                //                                            new JProperty("timestamp",         Timestamp.           ToISO8601()),
                //                                            new JProperty("eventTrackingId",   EventTrackingId.     ToString()),
                //                                            new JProperty("roamingNetworkId",  newRoamingNetwork.Id.ToString()),
                //                                            new JProperty("EVSEId",            EVSE.Id.             ToString()),
                //                                      OldStatus.HasValue
                //                                          ? new JProperty("oldStatus",         OldStatus?.Value.    ToString())
                //                                          : null,
                //                                            new JProperty("newStatus",         NewStatus. Value.    ToString())
                //                                  ));



                //newRoamingNetwork.OnEVSEAdminStatusChanged += async (Timestamp,
                //                                                     EventTrackingId,
                //                                                     EVSE,
                //                                                     NewStatus,
                //                                                     OldStatus,
                //                                                     dataSource)

                //    => await DebugLog.SubmitEvent("OnEVSEAdminStatusChanged",
                //                                  JSONObject.Create(
                //                                            new JProperty("timestamp",         Timestamp.           ToISO8601()),
                //                                            new JProperty("eventTrackingId",   EventTrackingId.     ToString()),
                //                                            new JProperty("roamingNetworkId",  newRoamingNetwork.Id.ToString()),
                //                                            new JProperty("EVSEId",            EVSE.Id.             ToString()),
                //                                      OldStatus.HasValue
                //                                          ? new JProperty("oldStatus",         OldStatus?.Value.    ToString())
                //                                          : null,
                //                                            new JProperty("newStatus",         NewStatus.Value.     ToString())
                //                                  ));

                #endregion

                #endregion

                return newRoamingNetwork;

            }

            throw new RoamingNetworkAlreadyExists(Id);

        }

        #endregion


        #region GetAllRoamingNetworks   (                                      Hostname = null)

        /// <summary>
        /// Return all roaming networks available for the given hostname.
        /// </summary>
        /// <param name="Hostname">An optional HTTP hostname.</param>
        public IEnumerable<IRoamingNetwork> GetAllRoamingNetworks(HTTPHostname? Hostname = null)
        {

            if (WWCPCores.TryGetValue(Hostname ?? HTTPHostname.Any, out var wwcpCore))
                return wwcpCore;

            return [];

        }

        #endregion

        #region GetRoamingNetwork       (RoamingNetworkId,                     Hostname = null)

        /// <summary>
        /// Return the roaming network for the given hostname.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="Hostname">An optional HTTP hostname.</param>
        public IRoamingNetwork? GetRoamingNetwork(RoamingNetwork_Id  RoamingNetworkId,
                                                  HTTPHostname?      Hostname   = null)
        {

            Hostname ??= HTTPHostname.Any;

            if (WWCPCores.TryGetValue         (Hostname.Value,   out var wwcpCore) &&
                wwcpCore. TryGetRoamingNetwork(RoamingNetworkId, out var roamingNetwork))
            {
                return roamingNetwork;
            }

            return null;

        }

        #endregion

        #region TryGetRoamingNetwork    (RoamingNetworkId, out RoamingNetwork, Hostname = null)

        /// <summary>
        ///Try to return all roaming networks available for the given hostname.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="Hostname">An optional HTTP hostname.</param>
        public Boolean TryGetRoamingNetwork(RoamingNetwork_Id                         RoamingNetworkId,
                                            [NotNullWhen(true)] out IRoamingNetwork?  RoamingNetwork,
                                            HTTPHostname?                             Hostname   = null)
        {

            if (!WWCPCores.TryGetValue(Hostname ?? HTTPHostname.Any, out var wwcpCore))
                 WWCPCores.TryGetValue(            HTTPHostname.Any, out     wwcpCore);

            if (wwcpCore is not null &&
                wwcpCore.TryGetRoamingNetwork(RoamingNetworkId, out RoamingNetwork))
            {
                return true;
            }

            RoamingNetwork = null;
            return false;

        }

        #endregion

        #region RoamingNetworkExists    (RoamingNetworkId,                     Hostname = null)

        /// <summary>
        /// Check if a roaming networks exists for the given hostname.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="Hostname">An optional HTTP hostname.</param>
        public Boolean RoamingNetworkExists(RoamingNetwork_Id   RoamingNetworkId,
                                            HTTPHostname?       Hostname   = null)
        {

            if (WWCPCores.TryGetValue(Hostname ?? HTTPHostname.Any, out var wwcpCore))
                return wwcpCore.Contains(RoamingNetworkId);

            return false;

        }

        #endregion

        #region RemoveRoamingNetwork    (RoamingNetworkId,                     Hostname = null)

        /// <summary>
        /// Return all roaming networks available for the given hostname.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="Hostname">An optional HTTP hostname.</param>
        public IRoamingNetwork? RemoveRoamingNetwork(RoamingNetwork_Id  RoamingNetworkId,
                                                     HTTPHostname?      Hostname   = null)
        {

            if (WWCPCores.TryGetValue            (Hostname ?? HTTPHostname.Any, out var wwcpCore) &&
                wwcpCore. TryRemoveRoamingNetwork(RoamingNetworkId,             out var roamingNetwork))
            {
                return roamingNetwork;
            }

            return null;

        }

        #endregion

        #region TryRemoveRoamingNetwork (RoamingNetworkId, out RoamingNetwork, Hostname = null)

        /// <summary>
        /// Return all roaming networks available for the given hostname.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="RoamingNetwork">The removed roaming network.</param>
        /// <param name="Hostname">An optional HTTP hostname.</param>
        public Boolean TryRemoveRoamingNetwork(RoamingNetwork_Id                         RoamingNetworkId,
                                               [NotNullWhen(true)] out IRoamingNetwork?  RoamingNetwork,
                                               HTTPHostname?                             Hostname   = null)
        {

            if (WWCPCores.TryGetValue            (Hostname ?? HTTPHostname.Any, out var wwcpCore) &&
                wwcpCore. TryRemoveRoamingNetwork(RoamingNetworkId,             out RoamingNetwork))
            {
                return true;
            }

            RoamingNetwork = null;
            return false;

        }

        #endregion


    }

}
