/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Net <https://github.com/GraphDefined/WWCP_Net>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.Net
{

    /// <summary>
    /// WWCP HTTP API extention methods.
    /// </summary>
    public static class ExtentionMethods
    {

        // Used by multiple HTTP content types

        #region ParseRoamingNetwork(this HTTPRequest, WWCPAPI, out RoamingNetwork, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network
        /// for the given HTTP hostname and HTTP query parameter
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetwork(this HTTPRequest    HTTPRequest,
                                                  WWCP_HTTPAPI        WWCPAPI,
                                                  out RoamingNetwork  RoamingNetwork,
                                                  out HTTPResponse    HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork  = null;
            HTTPResponse    = null;

            if (HTTPRequest.ParsedURIParameters.Length < 1)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    Connection      = "close"
                };

                return false;

            }

            if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out RoamingNetwork_Id RoamingNetworkId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid RoamingNetworkId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            RoamingNetwork  = WWCPAPI.
                                  GetAllRoamingNetworks(HTTPRequest.Host).
                                  FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

            if (RoamingNetwork == null) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown RoamingNetworkId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingStationOperator(this HTTPRequest, WWCPAPI, out RoamingNetwork, out ChargingStationOperator, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging station operator
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndChargingStationOperator(this HTTPRequest             HTTPRequest,
                                                                            WWCP_HTTPAPI                 WWCPAPI,
                                                                            out RoamingNetwork           RoamingNetwork,
                                                                            out ChargingStationOperator  ChargingStationOperator,
                                                                            out HTTPResponse             HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork           = null;
            ChargingStationOperator  = null;
            HTTPResponse             = null;

            if (HTTPRequest.ParsedURIParameters.Length < 2) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    Connection      = "close"
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!ChargingStationOperator_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ChargingStationOperator_Id ChargingStationOperatorId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationOperatorById(ChargingStationOperatorId, out ChargingStationOperator))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingPool(this HTTPRequest, WWCPAPI, out RoamingNetwork, out ChargingPool, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging pool
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndChargingPool(this HTTPRequest    HTTPRequest,
                                                                 WWCP_HTTPAPI        WWCPAPI,
                                                                 out RoamingNetwork  RoamingNetwork,
                                                                 out ChargingPool    ChargingPool,
                                                                 out HTTPResponse    HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork  = null;
            ChargingPool    = null;
            HTTPResponse    = null;

            if (HTTPRequest.ParsedURIParameters.Length < 2) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    Connection      = "close"
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!ChargingPool_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ChargingPool_Id ChargingPoolId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingPoolById(ChargingPoolId, out ChargingPool))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingStation(this HTTPRequest, WWCPAPI, out RoamingNetwork, out ChargingStation, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging station
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStation">The charging station.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndChargingStation(this HTTPRequest     HTTPRequest,
                                                                    WWCP_HTTPAPI         WWCPAPI,
                                                                    out RoamingNetwork   RoamingNetwork,
                                                                    out ChargingStation  ChargingStation,
                                                                    out HTTPResponse     HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork   = null;
            ChargingStation  = null;
            HTTPResponse     = null;

            if (HTTPRequest.ParsedURIParameters.Length < 2) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    Connection      = "close"
                };

                return false;

            }

            if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out RoamingNetwork_Id RoamingNetworkId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid RoamingNetworkId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            RoamingNetwork  = WWCPAPI.
                                  GetAllRoamingNetworks(HTTPRequest.Host).
                                  FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

            if (RoamingNetwork == null)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown RoamingNetworkId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            if (!ChargingStation_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ChargingStation_Id ChargingStationId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationById(ChargingStationId, out ChargingStation))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndEVSE(this HTTPRequest, WWCPAPI, out RoamingNetwork, out EVSE, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and EVSE
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="EVSE">The EVSE.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndEVSE(this HTTPRequest    HTTPRequest,
                                                         WWCP_HTTPAPI        WWCPAPI,
                                                         out RoamingNetwork  RoamingNetwork,
                                                         out EVSE            EVSE,
                                                         out HTTPResponse    HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork  = null;
            EVSE            = null;
            HTTPResponse    = null;

            if (HTTPRequest.ParsedURIParameters.Length < 2)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    Connection      = "close"
                };

                return false;

            }

            if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out RoamingNetwork_Id RoamingNetworkId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid RoamingNetworkId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            RoamingNetwork = WWCPAPI.
                                 GetAllRoamingNetworks(HTTPRequest.Host).
                                 FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

            if (RoamingNetwork == null)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown RoamingNetworkId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            if (!EVSE_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out EVSE_Id EVSEId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid EVSEId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetEVSEById(EVSEId, out EVSE))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown EVSEId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndReservation(this HTTPRequest, WWCPAPI, out RoamingNetwork, out EVSE, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging reservation
        /// for the given HTTP hostname and HTTP query parameters
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="Reservation">The charging reservation.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndReservation(this HTTPRequest         HTTPRequest,
                                                                WWCP_HTTPAPI             WWCPAPI,
                                                                out RoamingNetwork       RoamingNetwork,
                                                                out ChargingReservation  Reservation,
                                                                out HTTPResponse         HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork  = null;
            Reservation     = null;
            HTTPResponse    = null;

            if (HTTPRequest.ParsedURIParameters.Length < 2)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                };

                return false;

            }

            if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out RoamingNetwork_Id RoamingNetworkId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid RoamingNetworkId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            RoamingNetwork = WWCPAPI.
                                 GetAllRoamingNetworks(HTTPRequest.Host).
                                 FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

            if (RoamingNetwork == null)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown RoamingNetworkId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            if (!ChargingReservation_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ChargingReservation_Id ReservationId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ReservationId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.ReservationsStore.TryGetLatest(ReservationId, out Reservation))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ReservationId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion


        #region ParseRoamingNetworkAndParkingOperator(this HTTPRequest, WWCPAPI, out RoamingNetwork, out ParkingOperator, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and parking operator
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ParkingOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndParkingOperator(this HTTPRequest     HTTPRequest,
                                                                    WWCP_HTTPAPI         WWCPAPI,
                                                                    out RoamingNetwork   RoamingNetwork,
                                                                    out ParkingOperator  ParkingOperator,
                                                                    out HTTPResponse     HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork   = null;
            ParkingOperator  = null;
            HTTPResponse     = null;

            if (HTTPRequest.ParsedURIParameters.Length < 2)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    Connection      = "close"
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!ParkingOperator_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ParkingOperator_Id ParkingOperatorId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ParkingOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetParkingOperatorById(ParkingOperatorId, out ParkingOperator))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ParkingOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndEMobilityProvider(this HTTPRequest, WWCPAPI, out RoamingNetwork, out EMobilityProvider, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and e-mobility provider
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="EMobilityProvider">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndEMobilityProvider(this HTTPRequest       HTTPRequest,
                                                                      WWCP_HTTPAPI           WWCPAPI,
                                                                      out RoamingNetwork     RoamingNetwork,
                                                                      out eMobilityProvider  EMobilityProvider,
                                                                      out HTTPResponse       HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork     = null;
            EMobilityProvider  = null;
            HTTPResponse       = null;

            if (HTTPRequest.ParsedURIParameters.Length < 2)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    Connection      = "close"
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!eMobilityProvider_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out eMobilityProvider_Id EMobilityProviderId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid EMobilityProviderId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetEMobilityProviderById(EMobilityProviderId, out EMobilityProvider))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown EMobilityProviderId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndSmartCity(this HTTPRequest, WWCPAPI, out RoamingNetwork, out SmartCity, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and smart city
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="SmartCity">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndSmartCity(this HTTPRequest    HTTPRequest,
                                                              WWCP_HTTPAPI        WWCPAPI,
                                                              out RoamingNetwork  RoamingNetwork,
                                                              out SmartCityProxy  SmartCity,
                                                              out HTTPResponse    HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork  = null;
            SmartCity       = null;
            HTTPResponse    = null;

            if (HTTPRequest.ParsedURIParameters.Length < 2)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    Connection      = "close"
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!SmartCity_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out SmartCity_Id SmartCityId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid SmartCityId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetSmartCityById(SmartCityId, out SmartCity))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown SmartCityId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndGridOperator(this HTTPRequest, WWCPAPI, out RoamingNetwork, out GridOperator, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and smart city
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="GridOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndGridOperator(this HTTPRequest    HTTPRequest,
                                                                 WWCP_HTTPAPI        WWCPAPI,
                                                                 out RoamingNetwork  RoamingNetwork,
                                                                 out GridOperator    GridOperator,
                                                                 out HTTPResponse    HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork  = null;
            GridOperator    = null;
            HTTPResponse    = null;

            if (HTTPRequest.ParsedURIParameters.Length < 2)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!GridOperator_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out GridOperator_Id GridOperatorId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid GridOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetGridOperatorById(GridOperatorId, out GridOperator))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown GridOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion



        #region ParseRoamingNetworkAndChargingPoolAndChargingStation(this HTTPRequest, WWCPAPI, out RoamingNetwork, out ChargingPool, out ChargingStation, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network, charging pool
        /// and charging station for the given HTTP hostname and HTTP query
        /// parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="ChargingStation">The charging station.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndChargingPoolAndChargingStation(this HTTPRequest     HTTPRequest,
                                                                                   WWCP_HTTPAPI         WWCPAPI,
                                                                                   out RoamingNetwork   RoamingNetwork,
                                                                                   out ChargingPool     ChargingPool,
                                                                                   out ChargingStation  ChargingStation,
                                                                                   out HTTPResponse     HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork   = null;
            ChargingPool     = null;
            ChargingStation  = null;
            HTTPResponse     = null;

            if (HTTPRequest.ParsedURIParameters.Length < 3)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    Connection      = "close"
                };

                return false;

            }

            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;

            #region Get charging pool...

            if (!ChargingPool_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ChargingPool_Id ChargingPoolId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingPoolById(ChargingPoolId, out ChargingPool))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            #endregion

            #region Get charging station...

            if (!ChargingStation_Id.TryParse(HTTPRequest.ParsedURIParameters[2], out ChargingStation_Id ChargingStationId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationById(ChargingStationId, out ChargingStation))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            #endregion

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingPoolAndChargingStationAndEVSE(this HTTPRequest, WWCPAPI, out RoamingNetwork, out ChargingPool, out ChargingStation, out EVSE, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network, charging pool,
        /// charging station and EVSE for the given HTTP hostname and HTTP query
        /// parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="ChargingStation">The charging station.</param>
        /// <param name="EVSE">The EVSE.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        public static Boolean ParseRoamingNetworkAndChargingPoolAndChargingStationAndEVSE(this HTTPRequest     HTTPRequest,
                                                                                          WWCP_HTTPAPI         WWCPAPI,
                                                                                          out RoamingNetwork   RoamingNetwork,
                                                                                          out ChargingPool     ChargingPool,
                                                                                          out ChargingStation  ChargingStation,
                                                                                          out EVSE             EVSE,
                                                                                          out HTTPResponse     HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork   = null;
            ChargingPool     = null;
            ChargingStation  = null;
            EVSE             = null;
            HTTPResponse     = null;

            if (HTTPRequest.ParsedURIParameters.Length < 4)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    Connection      = "close"
                };

                return false;

            }

            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;

            #region Get charging pool...

            if (!ChargingPool_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ChargingPool_Id ChargingPoolId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingPoolById(ChargingPoolId, out ChargingPool))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingPoolId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            #endregion

            #region Get charging station...

            if (!ChargingStation_Id.TryParse(HTTPRequest.ParsedURIParameters[2], out ChargingStation_Id ChargingStationId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationById(ChargingStationId, out ChargingStation))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            #endregion

            #region Get EVSE

            if (!EVSE_Id.TryParse(HTTPRequest.ParsedURIParameters[3], out EVSE_Id EVSEId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid EVSEId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetEVSEById(EVSEId, out EVSE))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown EVSEId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            #endregion

            return true;

        }

        #endregion


        #region ParseRoamingNetworkAndChargingStationOperatorAndBrand(this HTTPRequest, WWCPAPI, out RoamingNetwork, out ChargingStationOperator, out Brand, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging station operator
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndChargingStationOperatorAndBrand(this HTTPRequest             HTTPRequest,
                                                                                    WWCP_HTTPAPI                 WWCPAPI,
                                                                                    out RoamingNetwork           RoamingNetwork,
                                                                                    out ChargingStationOperator  ChargingStationOperator,
                                                                                    out Brand                    Brand,
                                                                                    out HTTPResponse             HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork           = null;
            ChargingStationOperator  = null;
            Brand                    = null;
            HTTPResponse             = null;

            if (HTTPRequest.ParsedURIParameters.Length < 3)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!ChargingStationOperator_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ChargingStationOperator_Id ChargingStationOperatorId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationOperatorById(ChargingStationOperatorId, out ChargingStationOperator)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }



            if (!Brand_Id.TryParse(HTTPRequest.ParsedURIParameters[2], out Brand_Id BrandId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid BrandId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!ChargingStationOperator.TryGetBrand(BrandId, out Brand)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown BrandId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingStationOperatorAndChargingStationGroup(this HTTPRequest, WWCPAPI, out RoamingNetwork, out ChargingStationOperator, out ChargingStationGroup, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging station operator
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndChargingStationOperatorAndChargingStationGroup(this HTTPRequest             HTTPRequest,
                                                                                                   WWCP_HTTPAPI                 WWCPAPI,
                                                                                                   out RoamingNetwork           RoamingNetwork,
                                                                                                   out ChargingStationOperator  ChargingStationOperator,
                                                                                                   out ChargingStationGroup     ChargingStationGroup,
                                                                                                   out HTTPResponse             HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork           = null;
            ChargingStationOperator  = null;
            ChargingStationGroup     = null;
            HTTPResponse             = null;

            if (HTTPRequest.ParsedURIParameters.Length < 3)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!ChargingStationOperator_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ChargingStationOperator_Id ChargingStationOperatorId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationOperatorById(ChargingStationOperatorId, out ChargingStationOperator)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }



            if (!ChargingStationGroup_Id.TryParse(HTTPRequest.ParsedURIParameters[2], out ChargingStationGroup_Id ChargingStationGroupId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationGroupId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!ChargingStationOperator.TryGetChargingStationGroup(ChargingStationGroupId, out ChargingStationGroup)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationGroupId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseRoamingNetworkAndChargingStationOperatorAndEVSEGroup(this HTTPRequest, WWCPAPI, out RoamingNetwork, out ChargingStationOperator, out ChargingStationGroup, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network and charging station operator
        /// for the given HTTP hostname and HTTP query parameters or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="WWCPAPI">The WWCP API.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetworkAndChargingStationOperatorAndEVSEGroup(this HTTPRequest             HTTPRequest,
                                                                                        WWCP_HTTPAPI                 WWCPAPI,
                                                                                        out RoamingNetwork           RoamingNetwork,
                                                                                        out ChargingStationOperator  ChargingStationOperator,
                                                                                        out EVSEGroup                EVSEGroup,
                                                                                        out HTTPResponse             HTTPResponse)
        {

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException(nameof(HTTPRequest),  "The given HTTP request must not be null!");

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI),      "The given WWCP API must not be null!");

            #endregion

            RoamingNetwork           = null;
            ChargingStationOperator  = null;
            EVSEGroup                = null;
            HTTPResponse             = null;

            if (HTTPRequest.ParsedURIParameters.Length < 3)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                };

                return false;

            }


            if (!HTTPRequest.ParseRoamingNetwork(WWCPAPI, out RoamingNetwork, out HTTPResponse))
                return false;


            if (!ChargingStationOperator_Id.TryParse(HTTPRequest.ParsedURIParameters[1], out ChargingStationOperator_Id ChargingStationOperatorId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationOperatorById(ChargingStationOperatorId, out ChargingStationOperator)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationOperatorId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }



            if (!EVSEGroup_Id.TryParse(HTTPRequest.ParsedURIParameters[2], out EVSEGroup_Id EVSEGroupId)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid EVSEGroupId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!ChargingStationOperator.TryGetEVSEGroup(EVSEGroupId, out EVSEGroup)) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = WWCPAPI.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown EVSEGroupId!"" }".ToUTF8Bytes(),
                    Connection      = "close"
                };

                return false;

            }

            return true;

        }

        #endregion


        // Additional HTTP methods for HTTP clients

        #region REMOTESTART(this HTTPClient, URI, BuilderAction = null)

        public static HTTPRequest.Builder REMOTESTART(this HTTPClient              HTTPClient,
                                                      HTTPPath                      URI,
                                                      Action<HTTPRequest.Builder>  BuilderAction  = null)
        {

            #region Initial checks

            if (HTTPClient == null)
                throw new ArgumentNullException(nameof(HTTPClient),  "The given HTTP client must not be null!");

            if (URI.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(URI),         "The given URI must not be null!");

            #endregion

            return HTTPClient.CreateRequest(WWCP_HTTPAPI.REMOTESTART, URI, BuilderAction);

        }

        #endregion

        #region REMOTESTOP (this HTTPClient, URI, BuilderAction = null)

        public static HTTPRequest.Builder REMOTESTOP(this HTTPClient              HTTPClient,
                                                     HTTPPath                      URI,
                                                     Action<HTTPRequest.Builder>  BuilderAction  = null)
        {

            #region Initial checks

            if (HTTPClient == null)
                throw new ArgumentNullException(nameof(HTTPClient),  "The given HTTP client must not be null!");

            if (URI.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(URI),         "The given URI must not be null!");

            #endregion

            return HTTPClient.CreateRequest(WWCP_HTTPAPI.REMOTESTOP, URI, BuilderAction);

        }

        #endregion


    }

}
