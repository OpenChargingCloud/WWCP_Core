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
using org.GraphDefined.Vanaheimr.Hermod.HTTPTest;

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

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.RESERVE,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
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

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.SETEXPIRED,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
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

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.AUTHSTART,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
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

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.AUTHSTOP,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
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

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.REMOTESTART,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
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

            => HTTPClient.Execute(
                   client => client.CreateRequest(
                                 WWCP_HTTPAPI.REMOTESTOP,
                                 Path,
                                 Authentication:  Authentication,
                                 RequestBuilder:  RequestBuilder
                             )
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
        public ConcurrentDictionary<HTTPHostname, IWWCPCore>  WWCPCores                     { get; } = [];


        /// <summary>
        /// An optional additional URL path prefix.
        /// </summary>
        public HTTPPath?                                      AdditionalURLPathPrefix       { get; }

        /// <summary>
        /// Whether this API allows anonymous read access.
        /// </summary>
        public Boolean                                        AllowsAnonymousReadAccesss    { get; }

        /// <summary>
        /// Allow anonymous access to locations as Open Data.
        /// </summary>
        public Boolean                                        LocationsAsOpenData           { get; }

        /// <summary>
        /// Allow anonymous access to tariffs as Open Data.
        /// </summary>
        public Boolean                                        TariffsAsOpenData             { get; }

        /// <summary>
        /// (Dis-)allow PUTting of object having an earlier 'LastUpdated'-timestamp then already existing objects.
        /// WWCP v2.2 does not define any behaviour for this.
        /// </summary>
        public Boolean?                                       AllowDowngrades               { get; }

        ///// <summary>
        ///// The logging context.
        ///// </summary>
        //public String?                                        LoggingContext                { get; }

        /// <summary>
        /// The WWCP HTTP API logger.
        /// </summary>
        public WWCP_HTTPAPI_Logger?                           Logger                        { get; set; }

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


        #region (protected internal) CreateRoamingNetworkRequest (Request)

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public HTTPRequestLogEventX OnCreateRoamingNetworkRequest = new();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task CreateRoamingNetworkRequest(DateTimeOffset     Timestamp,
                                                            HTTPAPIX           API,
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
        public HTTPResponseLogEventX OnCreateRoamingNetworkResponse = new();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task CreateRoamingNetworkResponse(DateTimeOffset     Timestamp,
                                                             HTTPAPIX           API,
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
        public HTTPRequestLogEventX OnDeleteRoamingNetworkRequest = new();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        protected internal Task DeleteRoamingNetworkRequest(DateTimeOffset     Timestamp,
                                                            HTTPAPIX           API,
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
        public HTTPResponseLogEventX OnDeleteRoamingNetworkResponse = new();

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The HTTP API.</param>
        /// <param name="Request">A HTTP request.</param>
        /// <param name="Response">A HTTP response.</param>
        protected internal Task DeleteRoamingNetworkResponse(DateTimeOffset     Timestamp,
                                                             HTTPAPIX           API,
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
        public WWCP_HTTPAPI(HTTPExtAPIX                    HTTPAPI,
                            //IWWCPCore                      WWCPCore,
                            //URL                            OurBaseURL,
                            //URL                            OurVersionsURL,

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

            //this.WWCPCores.TryAdd(HTTPHostname.Any, WWCPCore);

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
                            Content                    = "This is an World Wide Charging Protocol v2.x HTTP service!".ToUTF8Bytes(),
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
                                                                 "This is an World Wide Charging Protocol v2.x HTTP service!"
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

                    if (!AllowsAnonymousReadAccesss)
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

                    if (!AllowsAnonymousReadAccesss)
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

                    if (!AllowsAnonymousReadAccesss)
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

                    if (!AllowsAnonymousReadAccesss)
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

                    if (!AllowsAnonymousReadAccesss)
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

                    if (!AllowsAnonymousReadAccesss)
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

                    if (!AllowsAnonymousReadAccesss)
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

                    if (!AllowsAnonymousReadAccesss)
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

                    if (!AllowsAnonymousReadAccesss)
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

                    if (!AllowsAnonymousReadAccesss)
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

            //                      if (!AllowsAnonymousReadAccesss)
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




            #region GET         ~/RNs/{RoamingNetworkId}/ChargingSessions/MissingCDRResponses

            // ----------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingSessions/MissingCDRResponses?ExpandCDRs=false
            // ----------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingSessions/MissingCDRResponses",
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

            #region GET         ~/RNs/{RoamingNetworkId}/ChargingSessions/FailedCDRResponses

            // ---------------------------------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ChargingSessions/FailedCDRResponses?ExpandCDRs=false
            // ---------------------------------------------------------------------------------------------------------------------------
            HTTPBaseAPI.AddHandler(

                HTTPMethod.GET,
                URLPathPrefix + "RNs/{RoamingNetworkId}/ChargingSessions/FailedCDRResponses",
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
                                                                      session.SendCDRResults.  All(sendCDRResult => sendCDRResult.Result != SendCDRResultTypes.Success) &&
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

            if (WWCPCores.TryGetValue         (Hostname ?? HTTPHostname.Any, out var wwcpCore) &&
                wwcpCore. TryGetRoamingNetwork(RoamingNetworkId,             out RoamingNetwork))
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
