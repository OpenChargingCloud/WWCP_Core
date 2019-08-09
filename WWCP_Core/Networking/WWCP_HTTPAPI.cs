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
using System.Threading;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;
using System.Threading.Tasks;
using System.Net.Security;
using System.Security.Authentication;

#endregion

namespace org.GraphDefined.WWCP.Net
{

    /// <summary>
    /// World Wide Charging Protocol - HTTP API.
    /// </summary>
    public class WWCP_HTTPAPI
    {

        #region Data

        private const          String           DefaultHTTPRoot        = "org.GraphDefined.WWCP.Net.HTTPRoot";


        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public const           String           DefaultHTTPServerName  = "GraphDefined WWCP HTTP Service v0.3";

        /// <summary>
        /// The default HTTP server TCP port.
        /// </summary>
        public static readonly IPPort           DefaultHTTPServerPort  = IPPort.Parse(3004);

        /// <summary>
        /// The default HTTP logfile.
        /// </summary>
        public const           String           DefaultLogfileName     = "WWCP_HTTPAPI.log";

        /// <summary>
        /// The default HTTP URI prefix.
        /// </summary>
        public static readonly HTTPPath          DefaultURIPrefix       = HTTPPath.Parse("/");

        #endregion

        #region Additional HTTP methods

        public readonly static HTTPMethod RESERVE      = HTTPMethod.Create("RESERVE",     IsSafe: false, IsIdempotent: true);
        public readonly static HTTPMethod SETEXPIRED   = HTTPMethod.Create("SETEXPIRED",  IsSafe: false, IsIdempotent: true);
        public readonly static HTTPMethod AUTHSTART    = HTTPMethod.Create("AUTHSTART",   IsSafe: false, IsIdempotent: true);
        public readonly static HTTPMethod AUTHSTOP     = HTTPMethod.Create("AUTHSTOP",    IsSafe: false, IsIdempotent: true);
        public readonly static HTTPMethod REMOTESTART  = HTTPMethod.Create("REMOTESTART", IsSafe: false, IsIdempotent: true);
        public readonly static HTTPMethod REMOTESTOP   = HTTPMethod.Create("REMOTESTOP",  IsSafe: false, IsIdempotent: true);
        public readonly static HTTPMethod SENDCDR      = HTTPMethod.Create("SENDCDR",     IsSafe: false, IsIdempotent: true);

        #endregion

        #region Properties

        #region HTTPServer

        /// <summary>
        /// The HTTP server of the API.
        /// </summary>
        public HTTPServer<RoamingNetworks, RoamingNetwork> HTTPServer { get; }

        #endregion

        #region Hostname

        /// <summary>
        /// The HTTP hostname for all URIs within this API.
        /// </summary>
        public HTTPHostname Hostname { get; }

        #endregion

        #region URIPrefix

        /// <summary>
        /// A common URI prefix for all URIs within this API.
        /// </summary>
        public HTTPPath URIPrefix   { get; }

        #endregion


        #region HTTP Server Sent Events

        /// <summary>
        /// Send debug information via HTTP Server Sent Events.
        /// </summary>
        public HTTPEventSource<Object>  DebugLog       { get; }

        /// <summary>
        /// Send importer information via HTTP Server Sent Events.
        /// </summary>
        public HTTPEventSource<Object>  ImporterLog    { get; }

        #endregion

        #region DNSClient

        /// <summary>
        /// The DNS resolver to use.
        /// </summary>
        public DNSClient DNSClient { get; }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog    = new HTTPRequestLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog   = new HTTPResponseLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog      = new HTTPErrorLogEvent();

        #region OnCreateRoamingNetwork

        /// <summary>
        /// An event sent whenever a create roaming network request was received.
        /// </summary>
        public event RequestLogHandler  OnCreateRoamingNetwork_RequestLog;

        /// <summary>
        /// An event sent whenever a response on an create roaming network request was sent.
        /// </summary>
        public event AccessLogHandler   OnCreateRoamingNetwork_ResponseLog;

        #endregion



        /// <summary>
        /// An event sent whenever an EVSE status request was received.
        /// </summary>
        public event RequestLogHandler  OnGetEVSEStatus;


        #region Reservations

        /// <summary>
        /// An event sent whenever a charging pool reservation request was received.
        /// </summary>
        public event RequestLogHandler  OnReserveChargingPool;

        /// <summary>
        /// An event sent whenever a charging pool reservation response was sent.
        /// </summary>
        public event AccessLogHandler   OnChargingPoolReserved;

        /// <summary>
        /// An event sent whenever a charging station reservation request was received.
        /// </summary>
        public event RequestLogHandler  OnReserveChargingStation;

        /// <summary>
        /// An event sent whenever a charging station reservation response was sent.
        /// </summary>
        public event AccessLogHandler   OnChargingStationReserved;

        /// <summary>
        /// An event sent whenever an EVSE reservation request was received.
        /// </summary>
        public event RequestLogHandler  OnReserveEVSE;

        /// <summary>
        /// An event sent whenever an EVSE reservation request response was sent.
        /// </summary>
        public event AccessLogHandler   OnEVSEReserved;

        #endregion

        #region OnAuthStart/-Stop

        /// <summary>
        /// An event sent whenever a authenticate start EVSE request was received.
        /// </summary>
        public event RequestLogHandler  OnAuthStartEVSERequest;

        /// <summary>
        /// An event sent whenever a authenticate start EVSE response was sent.
        /// </summary>
        public event AccessLogHandler   OnAuthStartEVSEResponse;

        /// <summary>
        /// An event sent whenever a authenticate start charging station request was received.
        /// </summary>
        public event RequestLogHandler  OnAuthStartChargingStationRequest;

        /// <summary>
        /// An event sent whenever a authenticate start charging station response was sent.
        /// </summary>
        public event AccessLogHandler   OnAuthStartChargingStationResponse;

        /// <summary>
        /// An event sent whenever a authenticate stop EVSE request was received.
        /// </summary>
        public event RequestLogHandler  OnAuthStopEVSERequest;

        /// <summary>
        /// An event sent whenever a authenticate stop EVSE response was sent.
        /// </summary>
        public event AccessLogHandler   OnAuthStopEVSEResponse;

        /// <summary>
        /// An event sent whenever a authenticate stop charging station request was received.
        /// </summary>
        public event RequestLogHandler  OnAuthStopChargingStation;

        /// <summary>
        /// An event sent whenever a authenticate stop charging station response was sent.
        /// </summary>
        public event AccessLogHandler   OnChargingStationAuthStopped;

        #endregion

        #region OnRemoteStart/-Stop

        /// <summary>
        /// An event sent whenever a remote start EVSE request was received.
        /// </summary>
        public event RequestLogHandler  OnRemoteStartEVSE;

        /// <summary>
        /// An event sent whenever a remote start EVSE response was sent.
        /// </summary>
        public event AccessLogHandler   OnEVSERemoteStarted;

        /// <summary>
        /// An event sent whenever a remote start charging station request was received.
        /// </summary>
        public event RequestLogHandler  OnRemoteStartChargingStation;

        /// <summary>
        /// An event sent whenever a remote start charging station response was sent.
        /// </summary>
        public event AccessLogHandler   OnChargingStationRemoteStarted;

        /// <summary>
        /// An event sent whenever a remote stop EVSE request was received.
        /// </summary>
        public event RequestLogHandler  OnRemoteStopEVSE;

        /// <summary>
        /// An event sent whenever a remote stop EVSE response was sent.
        /// </summary>
        public event AccessLogHandler   OnEVSERemoteStopped;

        /// <summary>
        /// An event sent whenever a remote stop charging station request was received.
        /// </summary>
        public event RequestLogHandler  OnRemoteStopChargingStation;

        /// <summary>
        /// An event sent whenever a remote stop charging station response was sent.
        /// </summary>
        public event AccessLogHandler   OnChargingStationRemoteStopped;

        #endregion

        #region OnSendCDR/-CDRSent

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event RequestLogHandler  OnSendCDRsRequest;

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public event AccessLogHandler   OnSendCDRsResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region WWCP_HTTPAPI(HTTPServerName = DefaultHTTPServerName, HTTPServerPort = null, Hostname = null, URIPrefix = null, ...)

        /// <summary>
        /// Create a new HTTP(S) server and attach this WWCP HTTP API to it.
        /// </summary>
        /// <param name="HTTPServerName">The default HTTP servername, used whenever no HTTP Host-header had been given.</param>
        /// <param name="HTTPServerPort">A TCP port to listen on.</param>
        /// <param name="Hostname">The HTTP hostname for all URIs within this API.</param>
        /// <param name="URIPrefix">A common prefix for all URIs.</param>
        /// 
        /// <param name="ServerCertificateSelector">An optional delegate to select a SSL/TLS server certificate.</param>
        /// <param name="ClientCertificateValidator">An optional delegate to verify the SSL/TLS client certificate used for authentication.</param>
        /// <param name="ClientCertificateSelector">An optional delegate to select the SSL/TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The SSL/TLS protocol(s) allowed for this connection.</param>
        /// 
        /// <param name="ServerThreadName">The optional name of the TCP server thread.</param>
        /// <param name="ServerThreadPriority">The optional priority of the TCP server thread.</param>
        /// <param name="ServerThreadIsBackground">Whether the TCP server thread is a background thread or not.</param>
        /// <param name="ConnectionIdBuilder">An optional delegate to build a connection identification based on IP socket information.</param>
        /// <param name="ConnectionThreadsNameBuilder">An optional delegate to set the name of the TCP connection threads.</param>
        /// <param name="ConnectionThreadsPriorityBuilder">An optional delegate to set the priority of the TCP connection threads.</param>
        /// <param name="ConnectionThreadsAreBackground">Whether the TCP connection threads are background threads or not (default: yes).</param>
        /// <param name="ConnectionTimeout">The TCP client timeout for all incoming client connections in seconds (default: 30 sec).</param>
        /// <param name="MaxClientConnections">The maximum number of concurrent TCP client connections (default: 4096).</param>
        /// 
        /// <param name="DNSClient">The DNS client to use.</param>
        /// <param name="Autostart">Start the HTTP server thread immediately (default: no).</param>
        public WWCP_HTTPAPI(String                               HTTPServerName                     = DefaultHTTPServerName,
                            IPPort?                              HTTPServerPort                     = null,
                            HTTPHostname?                        Hostname                           = null,
                            HTTPPath?                             URIPrefix                          = null,

                            ServerCertificateSelectorDelegate    ServerCertificateSelector          = null,
                            RemoteCertificateValidationCallback  ClientCertificateValidator         = null,
                            LocalCertificateSelectionCallback    ClientCertificateSelector          = null,
                            SslProtocols                         AllowedTLSProtocols                = SslProtocols.Tls12,

                            String                               ServerThreadName                   = null,
                            ThreadPriority                       ServerThreadPriority               = ThreadPriority.AboveNormal,
                            Boolean                              ServerThreadIsBackground           = true,
                            ConnectionIdBuilder                  ConnectionIdBuilder                = null,
                            ConnectionThreadsNameBuilder         ConnectionThreadsNameBuilder       = null,
                            ConnectionThreadsPriorityBuilder     ConnectionThreadsPriorityBuilder   = null,
                            Boolean                              ConnectionThreadsAreBackground     = true,
                            TimeSpan?                            ConnectionTimeout                  = null,
                            UInt32                               MaxClientConnections               = TCPServer.__DefaultMaxClientConnections,

                            DNSClient                            DNSClient                          = null,
                            Boolean                              Autostart                          = false)


            : this(new HTTPServer<RoamingNetworks, RoamingNetwork>(TCPPort:                           HTTPServerPort ?? DefaultHTTPServerPort,
                                                                   DefaultServerName:                 HTTPServerName ?? DefaultHTTPServerName,

                                                                   ServerCertificateSelector:         ServerCertificateSelector,
                                                                   ClientCertificateValidator:        ClientCertificateValidator,
                                                                   ClientCertificateSelector:         ClientCertificateSelector,
                                                                   AllowedTLSProtocols:               AllowedTLSProtocols,

                                                                   ServerThreadName:                  ServerThreadName,
                                                                   ServerThreadPriority:              ServerThreadPriority,
                                                                   ServerThreadIsBackground:          ServerThreadIsBackground,
                                                                   ConnectionIdBuilder:               ConnectionIdBuilder,
                                                                   ConnectionThreadsNameBuilder:      ConnectionThreadsNameBuilder,
                                                                   ConnectionThreadsPriorityBuilder:  ConnectionThreadsPriorityBuilder,
                                                                   ConnectionThreadsAreBackground:    ConnectionThreadsAreBackground,
                                                                   ConnectionTimeout:                 ConnectionTimeout,
                                                                   MaxClientConnections:              MaxClientConnections,

                                                                   DNSClient:                         DNSClient,
                                                                   Autostart:                         false),
                   Hostname,
                   URIPrefix)

        {

            if (Autostart)
                HTTPServer.Start();

        }

        #endregion 

        #region WWCP_HTTPAPI(HTTPServer, HTTPHostname = null, URIPrefix = null)

        /// <summary>
        /// Attach this WWCP HTTP API to the given HTTP server.
        /// </summary>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="Hostname">The HTTP hostname for all URIs within this API.</param>
        /// <param name="URIPrefix">A common prefix for all URIs.</param>
        public WWCP_HTTPAPI(HTTPServer     HTTPServer,
                            HTTPHostname?  Hostname    = null,
                            HTTPPath?       URIPrefix   = null)

            : this(new HTTPServer<RoamingNetworks, RoamingNetwork>(HTTPServer ?? throw new ArgumentNullException(nameof(HTTPServer), "The given HTTP server must not be null!")),
                   Hostname,
                   URIPrefix)

            { }


        /// <summary>
        /// Attach this WWCP HTTP API to the given HTTP server.
        /// </summary>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="Hostname">The HTTP hostname for all URIs within this API.</param>
        /// <param name="URIPrefix">A common prefix for all URIs.</param>
        public WWCP_HTTPAPI(HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                            HTTPHostname?                                Hostname    = null,
                            HTTPPath?                                     URIPrefix   = null)
        {

            #region Init data

            this.HTTPServer  = HTTPServer ?? throw new ArgumentNullException(nameof(HTTPServer), "The given HTTP server must not be null!");
            this.Hostname    = Hostname   ?? HTTPHostname.Any;
            this.URIPrefix   = URIPrefix  ?? DefaultURIPrefix;

            this.DNSClient   = HTTPServer.DNSClient;

            #endregion

            // Link HTTP events...
            HTTPServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            HTTPServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            HTTPServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

            #region Register URI templates

            var LogfilePrefix = "HTTPSSEs" + System.IO.Path.DirectorySeparatorChar;

            DebugLog     = HTTPServer.AddEventSource(EventIdentification:      Semantics.DebugLog,
                                                     URITemplate:              this.URIPrefix + "/" + Semantics.DebugLog.ToString(),
                                                     MaxNumberOfCachedEvents:  1000,
                                                     RetryIntervall:           TimeSpan.FromSeconds(5),
                                                     CreateHelper:             _ => new Object(),
                                                     EnableLogging:            true,
                                                     LogfilePrefix:            LogfilePrefix);

            ImporterLog  = HTTPServer.AddEventSource(EventIdentification:      Semantics.ImporterLog,
                                                     URITemplate:              this.URIPrefix + "/" + Semantics.ImporterLog.ToString(),
                                                     MaxNumberOfCachedEvents:  1000,
                                                     RetryIntervall:           TimeSpan.FromSeconds(5),
                                                     CreateHelper:             _ => new Object(),
                                                     EnableLogging:            true,
                                                     LogfilePrefix:            LogfilePrefix);

            RegisterURITemplates();

            #endregion

        }

        #endregion

        #endregion


        #region (static) AttachToHTTPAPI(HTTPServer, Hostname = null, URIPrefix = null)

        /// <summary>
        /// Attach this WWCP HTTP API to the given HTTP server.
        /// </summary>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="Hostname">The HTTP hostname for all URIs within this API.</param>
        /// <param name="URIPrefix">A common prefix for all URIs.</param>
        public static WWCP_HTTPAPI

            AttachToHTTPAPI(HTTPServer     HTTPServer,
                            HTTPHostname?  Hostname    = null,
                            HTTPPath?       URIPrefix   = null)

                => new WWCP_HTTPAPI(HTTPServer,
                                    Hostname,
                                    URIPrefix);


        /// <summary>
        /// Attach this WWCP HTTP API to the given HTTP server.
        /// </summary>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="Hostname">The HTTP hostname for all URIs within this API.</param>
        /// <param name="URIPrefix">A common prefix for all URIs.</param>
        public static WWCP_HTTPAPI

            AttachToHTTPAPI(HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                            HTTPHostname?                                Hostname    = null,
                            HTTPPath?                                     URIPrefix   = null)

                => new WWCP_HTTPAPI(HTTPServer,
                                    Hostname,
                                    URIPrefix);

        #endregion

        #region (private) RegisterURITemplates()

        private void RegisterURITemplates()
        {

        }

        #endregion


        #region CreateNewRoamingNetwork(Id, Name, Description = null, Configurator = null, ...)

        /// <summary>
        /// Create and register a new roaming network collection
        /// for the given HTTP hostname.
        /// </summary>
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
        public RoamingNetwork CreateNewRoamingNetwork(RoamingNetwork_Id                         Id,
                                                      I18NString                                Name,
                                                      I18NString                                Description                                 = null,
                                                      Action<RoamingNetwork>                    Configurator                                = null,
                                                      RoamingNetworkAdminStatusTypes            AdminStatus                                 = RoamingNetworkAdminStatusTypes.Operational,
                                                      RoamingNetworkStatusTypes                 Status                                      = RoamingNetworkStatusTypes.Available,
                                                      UInt16                                    MaxAdminStatusListSize                      = RoamingNetwork.DefaultMaxAdminStatusListSize,
                                                      UInt16                                    MaxStatusListSize                           = RoamingNetwork.DefaultMaxStatusListSize,
                                                      ChargingStationSignatureDelegate          ChargingStationSignatureGenerator           = null,
                                                      ChargingPoolSignatureDelegate             ChargingPoolSignatureGenerator              = null,
                                                      ChargingStationOperatorSignatureDelegate  ChargingStationOperatorSignatureGenerator   = null,
                                                      Func<RoamingNetwork_Id, String>           ReservationLogFileNameCreator               = null,
                                                      Func<RoamingNetwork_Id, String>           SessionLogFileNameCreator                   = null,
                                                      Func<RoamingNetwork_Id, String>           ChargeDetailRecordLogFileNameCreator        = null)


            => CreateNewRoamingNetwork(HTTPHostname.Any,
                                       Id,
                                       Name,
                                       Description,
                                       Configurator,
                                       AdminStatus,
                                       Status,
                                       MaxAdminStatusListSize,
                                       MaxStatusListSize,
                                       ChargingStationSignatureGenerator,
                                       ChargingPoolSignatureGenerator,
                                       ChargingStationOperatorSignatureGenerator,
                                       ReservationLogFileNameCreator,
                                       SessionLogFileNameCreator,
                                       ChargeDetailRecordLogFileNameCreator);

        #endregion

        #region CreateNewRoamingNetwork(Hostname, Id, Name, Description = null, Configurator = null, ...)

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
        public RoamingNetwork CreateNewRoamingNetwork(HTTPHostname                              Hostname,
                                                      RoamingNetwork_Id                         Id,
                                                      I18NString                                Name,
                                                      I18NString                                Description                                 = null,
                                                      Action<RoamingNetwork>                    Configurator                                = null,
                                                      RoamingNetworkAdminStatusTypes            AdminStatus                                 = RoamingNetworkAdminStatusTypes.Operational,
                                                      RoamingNetworkStatusTypes                 Status                                      = RoamingNetworkStatusTypes.Available,
                                                      UInt16                                    MaxAdminStatusListSize                      = RoamingNetwork.DefaultMaxAdminStatusListSize,
                                                      UInt16                                    MaxStatusListSize                           = RoamingNetwork.DefaultMaxStatusListSize,
                                                      ChargingStationSignatureDelegate          ChargingStationSignatureGenerator           = null,
                                                      ChargingPoolSignatureDelegate             ChargingPoolSignatureGenerator              = null,
                                                      ChargingStationOperatorSignatureDelegate  ChargingStationOperatorSignatureGenerator   = null,
                                                      Func<RoamingNetwork_Id, String>           ReservationLogFileNameCreator               = null,
                                                      Func<RoamingNetwork_Id, String>           SessionLogFileNameCreator                   = null,
                                                      Func<RoamingNetwork_Id, String>           ChargeDetailRecordLogFileNameCreator        = null)

        {

            #region Initial checks

            if (Hostname == null)
                throw new ArgumentNullException(nameof(Hostname), "The given HTTP hostname must not be null!");

            #endregion

            var ExistingRoamingNetwork = HTTPServer.
                                             GetAllTenants(Hostname).
                                             FirstOrDefault(roamingnetwork => roamingnetwork.Id == Id);

            if (ExistingRoamingNetwork == null)
            {

                if (!HTTPServer.TryGetTenants(Hostname, out RoamingNetworks _RoamingNetworks))
                {

                    _RoamingNetworks = new RoamingNetworks();

                    if (!HTTPServer.TryAddTenants(Hostname, _RoamingNetworks))
                        throw new Exception("Could not add new roaming networks object to the HTTP host!");

                }

                var NewRoamingNetwork = _RoamingNetworks.
                                            CreateNewRoamingNetwork(Id,
                                                                    Name,
                                                                    Description,
                                                                    Configurator,
                                                                    AdminStatus,
                                                                    Status,
                                                                    MaxAdminStatusListSize,
                                                                    MaxStatusListSize,
                                                                    ChargingStationSignatureGenerator,
                                                                    ChargingPoolSignatureGenerator,
                                                                    ChargingStationOperatorSignatureGenerator,
                                                                    ReservationLogFileNameCreator,
                                                                    SessionLogFileNameCreator,
                                                                    ChargeDetailRecordLogFileNameCreator);


                #region Link log events to HTTP-SSE...

                #region OnReserveEVSERequest/-Response

                NewRoamingNetwork.OnReserveRequest += async (LogTimestamp,
                                                             Timestamp,
                                                             Sender,
                                                             EventTrackingId,
                                                             RoamingNetworkId2,
                                                             ReservationId,
                                                             EVSEId,
                                                             StartTime,
                                                             Duration,
                                                             ProviderId,
                                                             eMAId,
                                                             ChargingProduct,
                                                             AuthTokens,
                                                             eMAIds,
                                                             PINs,
                                                             RequestTimeout)

                    => await DebugLog.SubmitSubEvent("OnReserveRequest",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                 Timestamp.ToIso8601()),
                                                         EventTrackingId != null
                                                            ? new JProperty("EventTrackingId",      EventTrackingId.ToString())
                                                            : null,
                                                         new JProperty("RoamingNetwork",            Id.ToString()),
                                                         ReservationId != null
                                                            ? new JProperty("ReservationId",        ReservationId.ToString())
                                                            : null,
                                                         EVSEId     != null
                                                             ? new JProperty("EVSEId",              EVSEId.ToString())
                                                             : null,
                                                         StartTime.HasValue
                                                             ? new JProperty("StartTime",           StartTime.Value.ToIso8601())
                                                             : null,
                                                         Duration.HasValue
                                                             ? new JProperty("Duration",            Duration.Value.TotalSeconds.ToString())
                                                             : null,
                                                         ProviderId != null
                                                             ? new JProperty("ProviderId",          ProviderId.ToString())
                                                             : null,
                                                         eMAId != null
                                                             ? new JProperty("eMAId",               eMAId.ToString())
                                                             : null,
                                                         ChargingProduct != null
                                                             ? new JProperty("ChargingProduct",     JSONObject.Create(
                                                                   new JProperty("Id",                              ChargingProduct.Id.ToString()),
                                                                   ChargingProduct.MinDuration.HasValue
                                                                       ? new JProperty("MinDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterTime.HasValue
                                                                       ? new JProperty("StopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.MinPower.HasValue
                                                                       ? new JProperty("MinPower",                  ChargingProduct.MinPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MaxPower.HasValue
                                                                       ? new JProperty("MaxPower",                  ChargingProduct.MaxPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MinEnergy.HasValue
                                                                       ? new JProperty("MinEnergy",                 ChargingProduct.MinEnergy.Value)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterKWh.HasValue
                                                                       ? new JProperty("StopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                                                                       : null
                                                                  ))
                                                             : null,
                                                         AuthTokens != null
                                                             ? new JProperty("AuthTokens",          new JArray(AuthTokens.Select(_ => _.ToString())))
                                                             : null,
                                                         eMAIds != null
                                                             ? new JProperty("eMAIds",              new JArray(eMAIds.Select(_ => _.ToString())))
                                                             : null,
                                                         PINs != null
                                                             ? new JProperty("PINs",                new JArray(PINs.Select(_ => _.ToString())))
                                                             : null
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                NewRoamingNetwork.OnReserveResponse += async (LogTimestamp,
                                                              Timestamp,
                                                              Sender,
                                                              EventTrackingId,
                                                              RoamingNetworkId2,
                                                              ReservationId,
                                                              EVSEId,
                                                              StartTime,
                                                              Duration,
                                                              ProviderId,
                                                              eMAId,
                                                              ChargingProduct,
                                                              AuthTokens,
                                                              eMAIds,
                                                              PINs,
                                                              Result,
                                                              Runtime,
                                                              RequestTimeout)

                    => await DebugLog.SubmitSubEvent("OnReserveResponse",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                 Timestamp.ToIso8601()),
                                                           EventTrackingId != null
                                                              ? new JProperty("EventTrackingId",      EventTrackingId.ToString())
                                                              : null,
                                                           new JProperty("RoamingNetwork",            Id.ToString()),
                                                           ReservationId != null
                                                              ? new JProperty("ReservationId",        ReservationId.ToString())
                                                              : null,
                                                           EVSEId     != null
                                                               ? new JProperty("EVSEId",              EVSEId.ToString())
                                                               : null,
                                                           StartTime.HasValue
                                                               ? new JProperty("StartTime",           StartTime.Value.ToIso8601())
                                                               : null,
                                                           Duration.HasValue
                                                               ? new JProperty("Duration",            Duration.Value.TotalSeconds.ToString())
                                                               : null,
                                                           ProviderId != null
                                                               ? new JProperty("ProviderId",          ProviderId.ToString()+"X")
                                                               : null,
                                                           eMAId != null
                                                               ? new JProperty("eMAId",               eMAId.ToString())
                                                               : null,
                                                           ChargingProduct != null
                                                             ? new JProperty("ChargingProduct",     JSONObject.Create(
                                                                   new JProperty("Id",                              ChargingProduct.Id.ToString()),
                                                                   ChargingProduct.MinDuration.HasValue
                                                                       ? new JProperty("MinDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterTime.HasValue
                                                                       ? new JProperty("StopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.MinPower.HasValue
                                                                       ? new JProperty("MinPower",                  ChargingProduct.MinPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MaxPower.HasValue
                                                                       ? new JProperty("MaxPower",                  ChargingProduct.MaxPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MinEnergy.HasValue
                                                                       ? new JProperty("MinEnergy",                 ChargingProduct.MinEnergy.Value)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterKWh.HasValue
                                                                       ? new JProperty("StopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                                                                       : null
                                                                  ))
                                                             : null,
                                                           AuthTokens != null
                                                               ? new JProperty("AuthTokens",          new JArray(AuthTokens.Select(_ => _.ToString())))
                                                               : null,
                                                           eMAIds != null
                                                               ? new JProperty("eMAIds",              new JArray(eMAIds.Select(_ => _.ToString())))
                                                               : null,
                                                           PINs != null
                                                               ? new JProperty("PINs",                new JArray(PINs.Select(_ => _.ToString())))
                                                               : null,
                                                           new JProperty("Result",                    Result.Result.ToString()),
                                                           Result.Message.IsNotNullOrEmpty()
                                                               ? new JProperty("ErrorMessage",        Result.Message)
                                                               : null,
                                                           new JProperty("Runtime",                   Math.Round(Runtime.TotalMilliseconds, 0))
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                #endregion

                #region OnCancelReservationResponse

                NewRoamingNetwork.OnCancelReservationResponse += async (LogTimestamp,
                                                                   Timestamp,
                                                                   Sender,
                                                                   EventTrackingId,
                                                                   RoamingNetworkId,
                                                                   //ProviderId,
                                                                   ReservationId,
                                                                   Reservation,
                                                                   Reason,
                                                                   Result,
                                                                   Runtime,
                                                                   RequestTimeout)

                    => await DebugLog.SubmitSubEvent("OnCancelReservation",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                Timestamp.ToIso8601()),
                                                         EventTrackingId != null
                                                             ? new JProperty("EventTrackingId",    EventTrackingId.ToString())
                                                             : null,
                                                         new JProperty("ReservationId",            ReservationId.ToString()),

                                                         new JProperty("RoamingNetwork",           RoamingNetworkId.ToString()),

                                                         Reservation?.EVSEId != null
                                                             ? new JProperty("EVSEId",             Reservation.EVSEId.ToString())
                                                             : null,
                                                         Reservation?.ChargingStationId != null
                                                             ? new JProperty("ChargingStationId",  Reservation.ChargingStationId.ToString())
                                                             : null,
                                                         Reservation?.ChargingPoolId != null
                                                             ? new JProperty("ChargingPoolId",     Reservation.EVSEId.ToString())
                                                             : null,

                                                         new JProperty("Reason",                   Reason.ToString()),

                                                         new JProperty("Result",                   Result.Result.ToString()),
                                                         new JProperty("Message",                  Result.Message),
                                                         new JProperty("AdditionalInfo",           Result.AdditionalInfo),
                                                         new JProperty("Runtime",                  Result.Runtime)

                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                //ToDo: OnCancelReservationResponse Result!

                #endregion


                #region OnRemoteEVSEStartRequest/-Response

                NewRoamingNetwork.OnRemoteStartRequest += async (LogTimestamp,
                                                                     Timestamp,
                                                                     Sender,
                                                                     EventTrackingId,
                                                                     RoamingNetworkId,
                                                                     EVSEId,
                                                                     ChargingProduct,
                                                                     ReservationId,
                                                                     SessionId,
                                                                     ProviderId,
                                                                     RemoteAuthentication,
                                                                     RequestTimeout)

                    => await DebugLog.SubmitSubEvent("OnRemoteStartRequest",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                 Timestamp.        ToIso8601()),
                                                         new JProperty("EVSEId",                    EVSEId.           ToString()),
                                                         new JProperty("RoamingNetwork",            RoamingNetworkId. ToString()),
                                                         EventTrackingId != null
                                                             ? new JProperty("EventTrackingId",     EventTrackingId.  ToString())
                                                             : null,
                                                         SessionId.HasValue
                                                             ? new JProperty("SessionId",           SessionId.        ToString())
                                                             : null,
                                                         ProviderId.HasValue
                                                             ? new JProperty("ProviderId",          ProviderId.       ToString())
                                                             : null,
                                                         ChargingProduct != null
                                                             ? new JProperty("ChargingProduct",     JSONObject.Create(
                                                                   new JProperty("Id",                              ChargingProduct.Id.ToString()),
                                                                   ChargingProduct.MinDuration.HasValue
                                                                       ? new JProperty("MinDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterTime.HasValue
                                                                       ? new JProperty("StopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.MinPower.HasValue
                                                                       ? new JProperty("MinPower",                  ChargingProduct.MinPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MaxPower.HasValue
                                                                       ? new JProperty("MaxPower",                  ChargingProduct.MaxPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MinEnergy.HasValue
                                                                       ? new JProperty("MinEnergy",                 ChargingProduct.MinEnergy.Value)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterKWh.HasValue
                                                                       ? new JProperty("StopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                                                                       : null
                                                                  ))
                                                             : null,
                                                         ReservationId.HasValue
                                                             ? new JProperty("ReservationId",       ReservationId.    ToString())
                                                             : null,
                                                         RemoteAuthentication != null
                                                             ? new JProperty("remoteAuthentication", RemoteAuthentication.ToJSON())
                                                             : null
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                NewRoamingNetwork.OnRemoteStartResponse += async (LogTimestamp,
                                                                      Timestamp,
                                                                      Sender,
                                                                      EventTrackingId,
                                                                      RoamingNetworkId,
                                                                      EVSEId,
                                                                      ChargingProduct,
                                                                      ReservationId,
                                                                      SessionId,
                                                                      ProviderId,
                                                                      eMAId,
                                                                      RequestTimeout,
                                                                      Result,
                                                                      Runtime)

                    => await DebugLog.SubmitSubEvent("OnRemoteStartResponse",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                 Timestamp.ToIso8601()),
                                                         EventTrackingId != null
                                                             ? new JProperty("EventTrackingId",     EventTrackingId.ToString())
                                                             : null,
                                                         new JProperty("RoamingNetwork",            RoamingNetworkId.ToString()),
                                                         SessionId != null
                                                             ? new JProperty("SessionId",           SessionId.ToString())
                                                             : null,
                                                         ProviderId != null
                                                             ? new JProperty("ProviderId",          ProviderId.ToString())
                                                             : null,
                                                         EVSEId != null
                                                             ? new JProperty("EVSEId",              EVSEId.ToString())
                                                             : null,
                                                         ChargingProduct != null
                                                             ? new JProperty("ChargingProductId",   ChargingProduct.Id.ToString())
                                                             : null,
                                                         ReservationId != null
                                                             ? new JProperty("ReservationId",       ReservationId.ToString())
                                                             : null,
                                                         eMAId != null
                                                             ? new JProperty("eMAId",               eMAId.ToString())
                                                             : null,
                                                         new JProperty("Result",                    Result.Result.ToString()),
                                                         new JProperty("Runtime",                   Math.Round(Runtime.TotalMilliseconds, 0)),
                                                         Result.Message != null
                                                             ? new JProperty("ErrorMessage",        Result.Message)
                                                             : null
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                #endregion

                #region OnRemoteStop/-Stopped

                NewRoamingNetwork.OnRemoteStopRequest += async (LogTimestamp,
                                                                Timestamp,
                                                                Sender,
                                                                EventTrackingId,
                                                                RoamingNetworkId,
                                                                SessionId,
                                                                ReservationHandling,
                                                                ProviderId,
                                                                RemoteAuthentication,
                                                                RequestTimeout)

                    => await DebugLog.SubmitSubEvent("OnRemoteStopRequest",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",         Timestamp.ToIso8601()),
                                                         EventTrackingId != null
                                                                ? new JProperty("EventTrackingId",  EventTrackingId.ToString())
                                                                : null,
                                                         new JProperty("RoamingNetwork",    RoamingNetworkId.ToString()),
                                                         new JProperty("SessionId",         SessionId.ToString()),
                                                         ProviderId.HasValue
                                                             ? new JProperty("ProviderId",  ProviderId.ToString())
                                                             : null,
                                                         RemoteAuthentication != null
                                                             ? new JProperty("remoteAuthentication", RemoteAuthentication.ToJSON())
                                                             : null
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                NewRoamingNetwork.OnRemoteStopResponse += async (LogTimestamp,
                                                                 Timestamp,
                                                                 Sender,
                                                                 EventTrackingId,
                                                                 RoamingNetworkId,
                                                                 SessionId,
                                                                 ReservationHandling,
                                                                 ProviderId,
                                                                 RemoteAuthentication,
                                                                 RequestTimeout,
                                                                 Result,
                                                                 Runtime)

                    => await DebugLog.SubmitSubEvent("OnRemoteStopResponse",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                 Timestamp.ToIso8601()),
                                                         EventTrackingId != null
                                                             ? new JProperty("EventTrackingId",     EventTrackingId.ToString())
                                                             : null,
                                                         new JProperty("RoamingNetwork",            RoamingNetworkId.ToString()),
                                                         new JProperty("SessionId",                 SessionId.ToString()),
                                                         ProviderId.HasValue
                                                             ? new JProperty("ProviderId",          ProviderId.ToString())
                                                             : null,
                                                         RemoteAuthentication != null
                                                             ? new JProperty("remoteAuthentication", RemoteAuthentication.ToJSON())
                                                             : null,
                                                         new JProperty("Result",                    Result.Result.ToString()),
                                                         Result.Message.IsNotNullOrEmpty()
                                                             ? new JProperty("ErrorMessage",        Result.Message)
                                                             : null,
                                                         new JProperty("Runtime",                   Math.Round(Runtime.TotalMilliseconds, 0))
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                #endregion


                #region OnAuthorizeStart/-Started

                NewRoamingNetwork.OnAuthorizeStartRequest += async (LogTimestamp,
                                                                    RequestTimestamp,
                                                                    Sender,
                                                                    SenderId,
                                                                    EventTrackingId,
                                                                    RoamingNetworkId2,
                                                                    OperatorId,
                                                                    AuthToken,
                                                                    ChargingProduct,
                                                                    SessionId,
                                                                    RequestTimeout)

                    => await DebugLog.SubmitSubEvent("AUTHSTARTRequest",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                RequestTimestamp.  ToIso8601()),
                                                         new JProperty("EventTrackingId",          EventTrackingId.   ToString()),
                                                         new JProperty("RoamingNetwork",           RoamingNetworkId2. ToString()),
                                                         OperatorId != null
                                                             ? new JProperty("OperatorId",         OperatorId.        ToString())
                                                             : null,
                                                         new JProperty("AuthToken",                AuthToken.         ToString()),
                                                         ChargingProduct != null
                                                             ? new JProperty("ChargingProduct",     JSONObject.Create(
                                                                   new JProperty("Id",                              ChargingProduct.Id.ToString()),
                                                                   ChargingProduct.MinDuration.HasValue
                                                                       ? new JProperty("MinDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterTime.HasValue
                                                                       ? new JProperty("StopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.MinPower.HasValue
                                                                       ? new JProperty("MinPower",                  ChargingProduct.MinPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MaxPower.HasValue
                                                                       ? new JProperty("MaxPower",                  ChargingProduct.MaxPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MinEnergy.HasValue
                                                                       ? new JProperty("MinEnergy",                 ChargingProduct.MinEnergy.Value)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterKWh.HasValue
                                                                       ? new JProperty("StopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                                                                       : null
                                                                  ))
                                                             : null,
                                                         SessionId != null
                                                             ? new JProperty("SessionId",          SessionId.         ToString())
                                                             : null,
                                                         RequestTimeout.HasValue
                                                             ? new JProperty("RequestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                             : null
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));


                NewRoamingNetwork.OnAuthorizeStartResponse += async (LogTimestamp,
                                                                     RequestTimestamp,
                                                                     Sender,
                                                                     SenderId,
                                                                     EventTrackingId,
                                                                     RoamingNetworkId2,
                                                                     OperatorId,
                                                                     AuthToken,
                                                                     ChargingProduct,
                                                                     SessionId,
                                                                     RequestTimeout,
                                                                     Result,
                                                                     Runtime)

                    => await DebugLog.SubmitSubEvent("AUTHSTARTResponse",
                                                     JSONObject.Create(
                                                         new JProperty("timestamp",                RequestTimestamp.ToIso8601()),
                                                         new JProperty("eventTrackingId",          EventTrackingId.ToString()),
                                                         new JProperty("roamingNetwork",           RoamingNetworkId2.ToString()),
                                                         OperatorId.       HasValue
                                                             ? new JProperty("operatorId",         OperatorId.ToString())
                                                             : null,
                                                         new JProperty("authToken",                AuthToken.ToString()),

                                                         ChargingProduct != null
                                                             ? new JProperty("chargingProduct",     JSONObject.Create(
                                                                   new JProperty("id",                              ChargingProduct.Id.ToString()),
                                                                   ChargingProduct.MinDuration.HasValue
                                                                       ? new JProperty("minDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterTime.HasValue
                                                                       ? new JProperty("stopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.MinPower.HasValue
                                                                       ? new JProperty("minPower",                  ChargingProduct.MinPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MaxPower.HasValue
                                                                       ? new JProperty("maxPower",                  ChargingProduct.MaxPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MinEnergy.HasValue
                                                                       ? new JProperty("minEnergy",                 ChargingProduct.MinEnergy.Value)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterKWh.HasValue
                                                                       ? new JProperty("stopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                                                                       : null
                                                                  ))
                                                             : null,

                                                         SessionId.        HasValue
                                                             ? new JProperty("sessionId",          SessionId.ToString())
                                                             : null,
                                                         RequestTimeout.   HasValue
                                                             ? new JProperty("requestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                             : null,

                                                         new JProperty("result", JSONObject.Create(

                                                             new JProperty("result",            Result.Result.ToString()),

                                                             Result.SessionId.HasValue
                                                                 ? new JProperty("sessionId",   Result.SessionId.ToString())
                                                                 : null,
                                                             Result.ProviderId.HasValue
                                                                 ? new JProperty("providerId",  Result.ProviderId.ToString())
                                                                 : null,
                                                             new JProperty("authorizatorId",    Result.AuthorizatorId.ToString()),
                                                             Result.Description.IsNotNullOrEmpty()
                                                                 ? new JProperty("description", Result.Description)
                                                                 : null

                                                         )),

                                                         new JProperty("runtime",  Math.Round(Runtime.TotalMilliseconds, 0))

                                                     ). ToString().
                                                        Replace(Environment.NewLine, ""));

                #endregion

                #region OnAuthorizeEVSEStartRequest/-Response

                NewRoamingNetwork.OnAuthorizeEVSEStartRequest += async (LogTimestamp,
                                                                        RequestTimestamp,
                                                                        Sender,
                                                                        SenderId,
                                                                        EventTrackingId,
                                                                        RoamingNetworkId2,
                                                                        OperatorId,
                                                                        AuthToken,
                                                                        EVSEId,
                                                                        ChargingProduct,
                                                                        SessionId,
                                                                        ISendAuthorizeStartStop,
                                                                        RequestTimeout)

                    => await DebugLog.SubmitSubEvent("AUTHSTARTRequest",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                RequestTimestamp.  ToIso8601()),
                                                         new JProperty("EventTrackingId",          EventTrackingId.   ToString()),
                                                         new JProperty("RoamingNetwork",           RoamingNetworkId2. ToString()),
                                                         OperatorId.    HasValue
                                                             ? new JProperty("OperatorId",         OperatorId.        ToString())
                                                             : null,
                                                         new JProperty("AuthToken",                AuthToken.         ToString()),
                                                         new JProperty("EVSEId",                   EVSEId.            ToString()),
                                                         ChargingProduct != null
                                                             ? new JProperty("ChargingProduct",     JSONObject.Create(
                                                                   new JProperty("Id",                              ChargingProduct.Id.ToString()),
                                                                   ChargingProduct.MinDuration.HasValue
                                                                       ? new JProperty("MinDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterTime.HasValue
                                                                       ? new JProperty("StopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.MinPower.HasValue
                                                                       ? new JProperty("MinPower",                  ChargingProduct.MinPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MaxPower.HasValue
                                                                       ? new JProperty("MaxPower",                  ChargingProduct.MaxPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MinEnergy.HasValue
                                                                       ? new JProperty("MinEnergy",                 ChargingProduct.MinEnergy.Value)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterKWh.HasValue
                                                                       ? new JProperty("StopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                                                                       : null
                                                                  ))
                                                             : null,
                                                         SessionId.     HasValue
                                                             ? new JProperty("SessionId",          SessionId.         ToString())
                                                             : null,
                                                         RequestTimeout.HasValue
                                                             ? new JProperty("RequestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                             : null
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));


                NewRoamingNetwork.OnAuthorizeEVSEStartResponse += async (LogTimestamp,
                                                                         RequestTimestamp,
                                                                         Sender,
                                                                         SenderId,
                                                                         EventTrackingId,
                                                                         RoamingNetworkId2,
                                                                         OperatorId,
                                                                         AuthToken,
                                                                         EVSEId,
                                                                         ChargingProduct,
                                                                         SessionId,
                                                                         ISendAuthorizeStartStop,
                                                                         RequestTimeout,
                                                                         Result,
                                                                         Runtime)

                    => await DebugLog.SubmitSubEvent("AUTHSTARTResponse",
                                                     JSONObject.Create(
                                                         new JProperty("timestamp",                RequestTimestamp.ToIso8601()),
                                                         new JProperty("eventTrackingId",          EventTrackingId.ToString()),
                                                         new JProperty("roamingNetwork",           RoamingNetworkId2.ToString()),
                                                         OperatorId.       HasValue
                                                             ? new JProperty("operatorId",         OperatorId.ToString())
                                                             : null,
                                                         new JProperty("authToken",                AuthToken.ToString()),
                                                         new JProperty("EVSEId",                   EVSEId.ToString()),

                                                         ChargingProduct != null
                                                             ? new JProperty("chargingProduct",     JSONObject.Create(
                                                                   new JProperty("id",                              ChargingProduct.Id.ToString()),
                                                                   ChargingProduct.MinDuration.HasValue
                                                                       ? new JProperty("minDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterTime.HasValue
                                                                       ? new JProperty("stopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.MinPower.HasValue
                                                                       ? new JProperty("minPower",                  ChargingProduct.MinPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MaxPower.HasValue
                                                                       ? new JProperty("maxPower",                  ChargingProduct.MaxPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MinEnergy.HasValue
                                                                       ? new JProperty("minEnergy",                 ChargingProduct.MinEnergy.Value)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterKWh.HasValue
                                                                       ? new JProperty("stopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                                                                       : null
                                                                  ))
                                                             : null,

                                                         SessionId.        HasValue
                                                             ? new JProperty("sessionId",          SessionId.ToString())
                                                             : null,
                                                         RequestTimeout.   HasValue
                                                             ? new JProperty("requestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                             : null,

                                                         new JProperty("result", JSONObject.Create(

                                                             new JProperty("result",            Result.Result.ToString()),

                                                             Result.SessionId.HasValue
                                                                 ? new JProperty("sessionId",   Result.SessionId.ToString())
                                                                 : null,
                                                             Result.ProviderId.HasValue
                                                                 ? new JProperty("providerId",  Result.ProviderId.ToString())
                                                                 : null,
                                                             new JProperty("authorizatorId",    Result.AuthorizatorId.ToString()),
                                                             Result.Description.IsNotNullOrEmpty()
                                                                 ? new JProperty("description", Result.Description)
                                                                 : null

                                                         )),

                                                         new JProperty("runtime",  Math.Round(Runtime.TotalMilliseconds, 0))

                                                     ). ToString().
                                                        Replace(Environment.NewLine, ""));

                #endregion

                #region OnAuthorizeChargingStationStart/-Started

                NewRoamingNetwork.OnAuthorizeChargingStationStartRequest += async (LogTimestamp,
                                                                                   RequestTimestamp,
                                                                                   Sender,
                                                                                   SenderId,
                                                                                   EventTrackingId,
                                                                                   RoamingNetworkId2,
                                                                                   OperatorId,
                                                                                   AuthToken,
                                                                                   ChargingStationId,
                                                                                   ChargingProduct,
                                                                                   SessionId,
                                                                                   RequestTimeout)

                    => await DebugLog.SubmitSubEvent("AUTHSTARTRequest",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                RequestTimestamp.  ToIso8601()),
                                                         new JProperty("EventTrackingId",          EventTrackingId.   ToString()),
                                                         new JProperty("RoamingNetwork",           RoamingNetworkId2. ToString()),
                                                         OperatorId.    HasValue
                                                             ? new JProperty("OperatorId",         OperatorId.        ToString())
                                                             : null,
                                                         new JProperty("AuthToken",                AuthToken.         ToString()),
                                                         new JProperty("ChargingStationId",        ChargingStationId. ToString()),
                                                         ChargingProduct != null
                                                             ? new JProperty("ChargingProduct",     JSONObject.Create(
                                                                   new JProperty("Id",                              ChargingProduct.Id.ToString()),
                                                                   ChargingProduct.MinDuration.HasValue
                                                                       ? new JProperty("MinDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterTime.HasValue
                                                                       ? new JProperty("StopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.MinPower.HasValue
                                                                       ? new JProperty("MinPower",                  ChargingProduct.MinPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MaxPower.HasValue
                                                                       ? new JProperty("MaxPower",                  ChargingProduct.MaxPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MinEnergy.HasValue
                                                                       ? new JProperty("MinEnergy",                 ChargingProduct.MinEnergy.Value)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterKWh.HasValue
                                                                       ? new JProperty("StopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                                                                       : null
                                                                  ))
                                                             : null,
                                                         SessionId.     HasValue
                                                             ? new JProperty("SessionId",          SessionId.         ToString())
                                                             : null,
                                                         RequestTimeout.HasValue
                                                             ? new JProperty("RequestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                             : null
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));


                NewRoamingNetwork.OnAuthorizeChargingStationStartResponse += async (LogTimestamp,
                                                                                    RequestTimestamp,
                                                                                    Sender,
                                                                                    SenderId,
                                                                                    EventTrackingId,
                                                                                    RoamingNetworkId2,
                                                                                    OperatorId,
                                                                                    AuthToken,
                                                                                    ChargingStationId,
                                                                                    ChargingProduct,
                                                                                    SessionId,
                                                                                    RequestTimeout,
                                                                                    Result,
                                                                                    Runtime)

                    => await DebugLog.SubmitSubEvent("AUTHSTARTResponse",
                                                     JSONObject.Create(
                                                         new JProperty("timestamp",                RequestTimestamp.ToIso8601()),
                                                         new JProperty("eventTrackingId",          EventTrackingId.ToString()),
                                                         new JProperty("roamingNetwork",           RoamingNetworkId2.ToString()),
                                                         OperatorId.       HasValue
                                                             ? new JProperty("operatorId",         OperatorId.ToString())
                                                             : null,
                                                         new JProperty("authToken",                AuthToken.ToString()),
                                                         new JProperty("chargingStationId",        ChargingStationId.ToString()),

                                                         ChargingProduct != null
                                                             ? new JProperty("chargingProduct",     JSONObject.Create(
                                                                   new JProperty("id",                              ChargingProduct.Id.ToString()),
                                                                   ChargingProduct.MinDuration.HasValue
                                                                       ? new JProperty("minDuration",               ChargingProduct.MinDuration.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterTime.HasValue
                                                                       ? new JProperty("stopChargingAfterTime",     ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                                                       : null,
                                                                   ChargingProduct.MinPower.HasValue
                                                                       ? new JProperty("minPower",                  ChargingProduct.MinPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MaxPower.HasValue
                                                                       ? new JProperty("maxPower",                  ChargingProduct.MaxPower.Value)
                                                                       : null,
                                                                   ChargingProduct.MinEnergy.HasValue
                                                                       ? new JProperty("minEnergy",                 ChargingProduct.MinEnergy.Value)
                                                                       : null,
                                                                   ChargingProduct.StopChargingAfterKWh.HasValue
                                                                       ? new JProperty("stopChargingAfterKWh",      ChargingProduct.StopChargingAfterKWh.Value)
                                                                       : null
                                                                  ))
                                                             : null,

                                                         SessionId.        HasValue
                                                             ? new JProperty("sessionId",          SessionId.ToString())
                                                             : null,
                                                         RequestTimeout.   HasValue
                                                             ? new JProperty("requestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                             : null,

                                                         new JProperty("result", JSONObject.Create(

                                                             new JProperty("result",            Result.Result.ToString()),

                                                             Result.SessionId.HasValue
                                                                 ? new JProperty("sessionId",   Result.SessionId.ToString())
                                                                 : null,
                                                             Result.ProviderId.HasValue
                                                                 ? new JProperty("providerId",  Result.ProviderId.ToString())
                                                                 : null,
                                                             new JProperty("authorizatorId",    Result.AuthorizatorId.ToString()),
                                                             Result.Description.IsNotNullOrEmpty()
                                                                 ? new JProperty("description", Result.Description)
                                                                 : null

                                                         )),

                                                         new JProperty("runtime",  Math.Round(Runtime.TotalMilliseconds, 0))

                                                     ). ToString().
                                                        Replace(Environment.NewLine, ""));
                #endregion


                #region OnAuthorizeStop/-Stopped

                NewRoamingNetwork.OnAuthorizeStopRequest += async (LogTimestamp,
                                                                   RequestTimestamp,
                                                                   Sender,
                                                                   SenderId,
                                                                   EventTrackingId,
                                                                   RoamingNetworkId2,
                                                                   OperatorId,
                                                                   SessionId,
                                                                   AuthToken,
                                                                   RequestTimeout)

                    => await DebugLog.SubmitSubEvent("AUTHSTOPRequest",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                RequestTimestamp.  ToIso8601()),
                                                         new JProperty("EventTrackingId",          EventTrackingId.   ToString()),
                                                         new JProperty("RoamingNetwork",           RoamingNetworkId2. ToString()),
                                                         OperatorId != null
                                                             ? new JProperty("OperatorId",         OperatorId.        ToString())
                                                             : null,
                                                         new JProperty("AuthToken",                AuthToken.         ToString()),
                                                         new JProperty("SessionId",                SessionId.ToString()),
                                                         RequestTimeout.HasValue
                                                             ? new JProperty("RequestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                             : null
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                NewRoamingNetwork.OnAuthorizeStopResponse += async (LogTimestamp,
                                                                    RequestTimestamp,
                                                                    Sender,
                                                                    SenderId,
                                                                    EventTrackingId,
                                                                    RoamingNetworkId2,
                                                                    OperatorId,
                                                                    SessionId,
                                                                    AuthToken,
                                                                    RequestTimeout,
                                                                    Result,
                                                                    Runtime)

                => await DebugLog.SubmitSubEvent("AUTHSTOPResponse",
                                                 JSONObject.Create(
                                                     new JProperty("timestamp",                RequestTimestamp. ToIso8601()),
                                                     new JProperty("eventTrackingId",          EventTrackingId.  ToString()),
                                                     new JProperty("roamingNetwork",           RoamingNetworkId2.ToString()),
                                                     OperatorId.HasValue
                                                         ? new JProperty("operatorId",         OperatorId.       ToString())
                                                         : null,

                                                     SessionId.HasValue
                                                             ? new JProperty("sessionId",          SessionId.        ToString())
                                                             : null,
                                                         new JProperty("authToken",                AuthToken.        ToString()),
                                                         RequestTimeout.HasValue
                                                             ? new JProperty("requestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                             : null,

                                                     new JProperty("Result", JSONObject.Create(

                                                         Result.SessionId.HasValue
                                                             ? new JProperty("SessionId",   Result.SessionId.ToString())
                                                             : null,
                                                         Result.ProviderId.HasValue
                                                             ? new JProperty("ProviderId",  Result.ProviderId.ToString())
                                                             : null,
                                                         new JProperty("AuthorizatorId",    Result.AuthorizatorId.ToString()),
                                                         Result.Description.IsNotNullOrEmpty()
                                                             ? new JProperty("description", Result.Description)
                                                             : null

                                                     )),

                                                     new JProperty("runtime",                  Math.Round(Runtime.TotalMilliseconds, 0))

                                                 ).ToString().
                                                   Replace(Environment.NewLine, ""));

                #endregion

                #region OnAuthorizeEVSEStop/-Stopped

                NewRoamingNetwork.OnAuthorizeEVSEStopRequest += async (LogTimestamp,
                                                                       RequestTimestamp,
                                                                       Sender,
                                                                       SenderId,
                                                                       EventTrackingId,
                                                                       RoamingNetworkId2,
                                                                       OperatorId,
                                                                       EVSEId,
                                                                       SessionId,
                                                                       AuthToken,
                                                                       RequestTimeout)

                    => await DebugLog.SubmitSubEvent("AUTHSTOPRequest",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",                RequestTimestamp.  ToIso8601()),
                                                         new JProperty("EventTrackingId",          EventTrackingId.   ToString()),
                                                         new JProperty("RoamingNetwork",           RoamingNetworkId2. ToString()),
                                                         OperatorId != null
                                                             ? new JProperty("OperatorId",         OperatorId.        ToString())
                                                             : null,
                                                         new JProperty("EVSEId",                   EVSEId.            ToString()),
                                                         new JProperty("SessionId",                SessionId.         ToString()),
                                                         new JProperty("AuthToken",                AuthToken.         ToString()),
                                                         RequestTimeout.HasValue
                                                             ? new JProperty("RequestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                             : null
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                NewRoamingNetwork.OnAuthorizeEVSEStopResponse += async (LogTimestamp,
                                                                        RequestTimestamp,
                                                                        Sender,
                                                                        SenderId,
                                                                        EventTrackingId,
                                                                        RoamingNetworkId2,
                                                                        OperatorId,
                                                                        EVSEId,
                                                                        SessionId,
                                                                        AuthToken,
                                                                        RequestTimeout,
                                                                        Result,
                                                                        Runtime)

                    => await DebugLog.SubmitSubEvent("AUTHSTOPResponse",
                                                     JSONObject.Create(
                                                         new JProperty("timestamp",                RequestTimestamp. ToIso8601()),
                                                         new JProperty("eventTrackingId",          EventTrackingId.  ToString()),
                                                         new JProperty("roamingNetwork",           RoamingNetworkId2.ToString()),
                                                         OperatorId.HasValue
                                                             ? new JProperty("operatorId",         OperatorId.       ToString())
                                                             : null,

                                                         new JProperty("EVSEId",                   EVSEId.           ToString()),

                                                         SessionId.HasValue
                                                             ? new JProperty("sessionId",          SessionId.        ToString())
                                                             : null,
                                                         new JProperty("authToken",                AuthToken.        ToString()),
                                                         RequestTimeout.HasValue
                                                             ? new JProperty("requestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                             : null,

                                                         new JProperty("result", JSONObject.Create(

                                                             new JProperty("result",            Result.Result.ToString()),

                                                             Result.SessionId.HasValue
                                                                 ? new JProperty("sessionId",   Result.SessionId.ToString())
                                                                 : null,
                                                             Result.ProviderId.HasValue
                                                                 ? new JProperty("providerId",  Result.ProviderId.ToString())
                                                                 : null,
                                                             new JProperty("authorizatorId",    Result.AuthorizatorId.ToString()),
                                                             Result.Description.IsNotNullOrEmpty()
                                                                 ? new JProperty("description", Result.Description)
                                                                 : null

                                                         ))

                                                 ).ToString().
                                                   Replace(Environment.NewLine, ""));

                #endregion

                #region OnAuthorizeChargingStationStop/-Stopped

                NewRoamingNetwork.OnAuthorizeChargingStationStopRequest += async (LogTimestamp,
                                                                                  RequestTimestamp,
                                                                                  Sender,
                                                                                  SenderId,
                                                                                  EventTrackingId,
                                                                                  RoamingNetworkId2,
                                                                                  OperatorId,
                                                                                  ChargingStationId,
                                                                                  SessionId,
                                                                                  AuthToken,
                                                                                  RequestTimeout)

                => await DebugLog.SubmitSubEvent("AUTHSTOPRequest",
                                                 JSONObject.Create(
                                                     new JProperty("Timestamp",                RequestTimestamp. ToIso8601()),
                                                     new JProperty("EventTrackingId",          EventTrackingId.  ToString()),
                                                     new JProperty("RoamingNetwork",           RoamingNetworkId2.ToString()),
                                                     OperatorId.       HasValue
                                                         ? new JProperty("OperatorId",         OperatorId.       ToString())
                                                         : null,
                                                     new JProperty("ChargingStationId",        ChargingStationId.ToString()),
                                                     new JProperty("SessionId",                SessionId.        ToString()),
                                                     new JProperty("AuthToken",                AuthToken.        ToString())
                                                 ).ToString().
                                                   Replace(Environment.NewLine, ""));

                NewRoamingNetwork.OnAuthorizeChargingStationStopResponse += async (LogTimestamp,
                                                                                   RequestTimestamp,
                                                                                   Sender,
                                                                                   SenderId,
                                                                                   EventTrackingId,
                                                                                   RoamingNetworkId2,
                                                                                   OperatorId,
                                                                                   ChargingStationId,
                                                                                   SessionId,
                                                                                   AuthToken,
                                                                                   RequestTimeout,
                                                                                   Result,
                                                                                   Runtime)

                => await DebugLog.SubmitSubEvent("AUTHSTOPResponse",
                                                 JSONObject.Create(
                                                     new JProperty("timestamp",                RequestTimestamp. ToIso8601()),
                                                     new JProperty("eventTrackingId",          EventTrackingId.  ToString()),
                                                     new JProperty("roamingNetwork",           RoamingNetworkId2.ToString()),
                                                     OperatorId.       HasValue
                                                         ? new JProperty("operatorId",         OperatorId.       ToString())
                                                         : null,

                                                     new JProperty("chargingStationId",        ChargingStationId.ToString()),

                                                     SessionId.        HasValue
                                                         ? new JProperty("sessionId",          SessionId.        ToString())
                                                         : null,
                                                     new JProperty("authToken",                AuthToken.        ToString()),
                                                     RequestTimeout.   HasValue
                                                         ? new JProperty("requestTimeout",     Math.Round(RequestTimeout.Value.TotalSeconds, 0))
                                                         : null,

                                                     new JProperty("result", JSONObject.Create(

                                                         new JProperty("result",            Result.Result.ToString()),

                                                         Result.SessionId.HasValue
                                                             ? new JProperty("sessionId",   Result.SessionId.ToString())
                                                             : null,
                                                         Result.ProviderId.HasValue
                                                             ? new JProperty("providerId",  Result.ProviderId.ToString())
                                                             : null,
                                                         new JProperty("authorizatorId",    Result.AuthorizatorId.ToString()),
                                                         Result.Description.IsNotNullOrEmpty()
                                                             ? new JProperty("description", Result.Description)
                                                             : null

                                                     )),

                                                     new JProperty("runtime",  Math.Round(Runtime.TotalMilliseconds, 0))

                                                 ).ToString().
                                                   Replace(Environment.NewLine, ""));

                #endregion


                #region OnSendCDRsRequest/-Response

                NewRoamingNetwork.OnSendCDRsRequest += async (LogTimestamp,
                                                              RequestTimestamp,
                                                              Sender,
                                                              SenderId,
                                                              EventTrackingId,
                                                              RoamingNetworkId2,
                                                              DroppedChargeDetailRecords,
                                                              ChargeDetailRecords,
                                                              RequestTimeout)


                    => await DebugLog.SubmitSubEvent("OnSendCDRsRequest",
                                                     JSONObject.Create(
                                                         new JProperty("timestamp",                RequestTimestamp.  ToIso8601()),
                                                         new JProperty("eventTrackingId",          EventTrackingId.   ToString()),
                                                         new JProperty("roamingNetwork",           RoamingNetworkId2. ToString()),
                                                         //new JProperty("LogTimestamp",                     LogTimestamp.                                          ToIso8601()),
                                                         //new JProperty("RequestTimestamp",                 RequestTimestamp.                                      ToIso8601()),

                                                         new JProperty("chargeDetailRecords",              new JArray(
                                                             ChargeDetailRecords.Select(ChargeDetailRecord => JSONObject.Create(

                                                                new JProperty("sessionId",                        ChargeDetailRecord.SessionId.ToString()),

                                                                ChargeDetailRecord.SessionTime.HasValue
                                                                    ? new JProperty("sessionStart",               ChargeDetailRecord.SessionTime.Value.StartTime.             ToIso8601())
                                                                    : null,
                                                                ChargeDetailRecord.SessionTime.HasValue && ChargeDetailRecord.SessionTime.Value.EndTime.HasValue
                                                                    ? new JProperty("sessionStop",                ChargeDetailRecord.SessionTime.Value.EndTime.Value.         ToIso8601())
                                                                    : null,

                                                                ChargeDetailRecord.ProviderIdStart.HasValue
                                                                    ? new JProperty("providerIdStart",            ChargeDetailRecord.ProviderIdStart.                         ToString())
                                                                    : null,
                                                                ChargeDetailRecord.ProviderIdStop.HasValue
                                                                    ? new JProperty("providerIdStop",             ChargeDetailRecord.ProviderIdStop.                          ToString())
                                                                    : null,
                                                                ChargeDetailRecord.IdentificationStart.AuthToken != null
                                                                    ? new JProperty("authTokenStart",             ChargeDetailRecord.IdentificationStart.AuthToken.           ToString())
                                                                    : null,
                                                                ChargeDetailRecord.IdentificationStop?.AuthToken != null
                                                                    ? new JProperty("authTokenStop",              ChargeDetailRecord.IdentificationStop. AuthToken.           ToString())
                                                                    : null,
                                                                ChargeDetailRecord.IdentificationStart.RemoteIdentification.HasValue
                                                                    ? new JProperty("remoteIdentificationStart",  ChargeDetailRecord.IdentificationStart.RemoteIdentification.ToString())
                                                                    : null,
                                                                ChargeDetailRecord.IdentificationStop != null && ChargeDetailRecord.IdentificationStop.RemoteIdentification.HasValue
                                                                    ? new JProperty("remoteIdentificationStop",   ChargeDetailRecord.IdentificationStop.RemoteIdentification. ToString())
                                                                    : null,

                                                                ChargeDetailRecord.ReservationId.HasValue
                                                                    ? new JProperty("reservationId",              ChargeDetailRecord.ReservationId.                           ToString())
                                                                    : null,
                                                                ChargeDetailRecord.ReservationTime.HasValue
                                                                    ? new JProperty("reservationStart",           ChargeDetailRecord.ReservationTime.Value.StartTime.         ToString())
                                                                    : null,
                                                                ChargeDetailRecord.ReservationTime.HasValue && ChargeDetailRecord.ReservationTime.Value.EndTime.HasValue
                                                                    ? new JProperty("reservationStop",            ChargeDetailRecord.ReservationTime.Value.EndTime.Value.     ToIso8601())
                                                                    : null,
                                                                ChargeDetailRecord.Reservation             != null
                                                                    ? new JProperty("reservationLevel",           ChargeDetailRecord.Reservation.ReservationLevel.            ToString())
                                                                    : null,

                                                                ChargeDetailRecord.ChargingStationOperator != null
                                                                    ? new JProperty("chargingStationOperator",    ChargeDetailRecord.ChargingStationOperator.                 ToString())
                                                                    : null,

                                                                ChargeDetailRecord.EVSE != null
                                                                    ? new JProperty("EVSEId",                     ChargeDetailRecord.EVSE.Id.                                 ToString())
                                                                    : ChargeDetailRecord.EVSEId.HasValue
                                                                          ? new JProperty("EVSEId",               ChargeDetailRecord.EVSEId.                                  ToString())
                                                                          : null,

                                                                ChargeDetailRecord.ChargingProduct != null
                                                                    ? new JProperty("chargingProduct",            ChargeDetailRecord.ChargingProduct.ToJSON())
                                                                    : null,

                                                                ChargeDetailRecord.EnergyMeterId.HasValue
                                                                    ? new JProperty("energyMeterId",              ChargeDetailRecord.EnergyMeterId.                      ToString())
                                                                    : null,
                                                                      new JProperty("consumedEnergy",             ChargeDetailRecord.ConsumedEnergy),
                                                                ChargeDetailRecord.EnergyMeteringValues.Any()
                                                                    ? new JProperty("energyMeteringValues", JSONObject.Create(
                                                                          ChargeDetailRecord.EnergyMeteringValues.Select(metervalue => new JProperty(metervalue.Timestamp.ToIso8601(),
                                                                                                                                                     metervalue.Value)))
                                                                      )
                                                                    : null,
                                                                //ChargeDetailRecord.MeteringSignature.IsNotNullOrEmpty()
                                                                //    ? new JProperty("meteringSignature",          ChargeDetailRecord.MeteringSignature)
                                                                //    : null,

                                                                ChargeDetailRecord.ParkingSpaceId.HasValue
                                                                    ? new JProperty("parkingSpaceId",             ChargeDetailRecord.ParkingSpaceId.                      ToString())
                                                                    : null,
                                                                ChargeDetailRecord.ParkingTime.HasValue
                                                                    ? new JProperty("parkingTimeStart",           ChargeDetailRecord.ParkingTime.Value.StartTime.         ToIso8601())
                                                                    : null,
                                                                ChargeDetailRecord.ParkingTime.HasValue && ChargeDetailRecord.ParkingTime.Value.EndTime.HasValue
                                                                    ? new JProperty("parkingTimeEnd",             ChargeDetailRecord.ParkingTime.Value.EndTime.Value.     ToString())
                                                                    : null,
                                                                ChargeDetailRecord.ParkingFee.HasValue
                                                                    ? new JProperty("parkingFee",                 ChargeDetailRecord.ParkingFee.                          ToString())
                                                                    : null)

                                                                    )
                                                            )
                                                        )

                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                #endregion


                #region OnEVSEData/(Admin)StatusChanged

                NewRoamingNetwork.OnEVSEDataChanged += async (Timestamp,
                                                              EventTrackingId,
                                                              EVSE,
                                                              PropertyName,
                                                              OldValue,
                                                              NewValue)

                    => await DebugLog.SubmitSubEvent("OnEVSEDataChanged",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",       Timestamp.ToIso8601()),
                                                         new JProperty("EventTrackingId", EventTrackingId.ToString()),
                                                         new JProperty("RoamingNetwork",  NewRoamingNetwork.Id.ToString()),
                                                         new JProperty("EVSEId",          EVSE.Id.ToString()),
                                                         new JProperty("PropertyName",    PropertyName),
                                                         new JProperty("OldValue",        OldValue?.ToString()),
                                                         new JProperty("NewValue",        NewValue?.ToString())
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));



                NewRoamingNetwork.OnEVSEStatusChanged += async (Timestamp,
                                                                EventTrackingId,
                                                                EVSE,
                                                                OldStatus,
                                                                NewStatus)

                    => await DebugLog.SubmitSubEvent("OnEVSEStatusChanged",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",        Timestamp.ToIso8601()),
                                                         new JProperty("EventTrackingId",  EventTrackingId.ToString()),
                                                         new JProperty("RoamingNetwork",   NewRoamingNetwork.Id.ToString()),
                                                         new JProperty("EVSEId",           EVSE.Id.ToString()),
                                                         new JProperty("OldStatus",        OldStatus.Value.ToString()),
                                                         new JProperty("NewStatus",        NewStatus.Value.ToString())
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));



                NewRoamingNetwork.OnEVSEAdminStatusChanged += async (Timestamp,
                                                                     EventTrackingId,
                                                                     EVSE,
                                                                     OldStatus,
                                                                     NewStatus)

                    => await DebugLog.SubmitSubEvent("OnEVSEAdminStatusChanged",
                                                     JSONObject.Create(
                                                         new JProperty("Timestamp",        Timestamp.ToIso8601()),
                                                         new JProperty("EventTrackingId",  EventTrackingId.ToString()),
                                                         new JProperty("RoamingNetwork",   NewRoamingNetwork.Id.ToString()),
                                                         new JProperty("EVSEId",           EVSE.Id.ToString()),
                                                         new JProperty("OldStatus",        OldStatus.Value.ToString()),
                                                         new JProperty("NewStatus",        NewStatus.Value.ToString())
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                #endregion

                #endregion

                return NewRoamingNetwork;

            }

            throw new RoamingNetworkAlreadyExists(Id);

        }

        #endregion

        #region GetAllRoamingNetworks(Hostname)

        /// <summary>
        /// Return all roaming networks available for the given hostname.
        /// </summary>
        /// <param name="Hostname">The HTTP hostname.</param>
        public IEnumerable<RoamingNetwork> GetAllRoamingNetworks(HTTPHostname  Hostname)

            => HTTPServer.GetAllTenants(Hostname);

        #endregion

        #region GetRoamingNetwork(Hostname, RoamingNetworkId)

        /// <summary>
        /// Return all roaming networks available for the given hostname.
        /// </summary>
        /// <param name="Hostname">The HTTP hostname.</param>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        public RoamingNetwork GetRoamingNetwork(HTTPHostname       Hostname,
                                                RoamingNetwork_Id  RoamingNetworkId)

            => HTTPServer.GetAllTenants(Hostname).
                   FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

        #endregion

        #region TryGetRoamingNetwork(Hostname, RoamingNetworkId, out RoamingNetwork)

        /// <summary>
        ///Try to return all roaming networks available for the given hostname.
        /// </summary>
        /// <param name="Hostname">The HTTP hostname.</param>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public Boolean TryGetRoamingNetwork(HTTPHostname        Hostname,
                                            RoamingNetwork_Id   RoamingNetworkId,
                                            out RoamingNetwork  RoamingNetwork)
        {

            RoamingNetwork  = HTTPServer.GetAllTenants(Hostname).
                                  FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

            return RoamingNetwork != null;

        }

        #endregion

        #region RoamingNetworkExists(Hostname, RoamingNetworkId)

        /// <summary>
        /// Check if a roaming networks exists for the given hostname.
        /// </summary>
        /// <param name="Hostname">The HTTP hostname.</param>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        public Boolean RoamingNetworkExists(HTTPHostname        Hostname,
                                            RoamingNetwork_Id   RoamingNetworkId)
        {

            var RoamingNetwork = HTTPServer.GetAllTenants(Hostname).
                                     FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

            return RoamingNetwork != null;

        }

        #endregion

        #region RemoveRoamingNetwork(Hostname, RoamingNetworkId)

        /// <summary>
        /// Return all roaming networks available for the given hostname.
        /// </summary>
        /// <param name="Hostname">The HTTP hostname.</param>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        public RoamingNetwork RemoveRoamingNetwork(HTTPHostname       Hostname,
                                                   RoamingNetwork_Id  RoamingNetworkId)
        {

            RoamingNetworks _RoamingNetworks = null;

            if (!HTTPServer.TryGetTenants(Hostname, out _RoamingNetworks))
                return null;

            return _RoamingNetworks.RemoveRoamingNetwork(RoamingNetworkId);

        }

        #endregion

        #region TryRemoveRoamingNetwork(Hostname, RoamingNetworkId, out RoamingNetwork)

        /// <summary>
        /// Return all roaming networks available for the given hostname.
        /// </summary>
        /// <param name="Hostname">The HTTP hostname.</param>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="RoamingNetwork">The removed roaming network.</param>
        public Boolean TryRemoveRoamingNetwork(HTTPHostname        Hostname,
                                               RoamingNetwork_Id   RoamingNetworkId,
                                               out RoamingNetwork  RoamingNetwork)
        {

            RoamingNetworks _RoamingNetworks = null;

            if (!HTTPServer.TryGetTenants(Hostname, out _RoamingNetworks))
            {
                RoamingNetwork = null;
                return false;
            }

            RoamingNetwork = _RoamingNetworks.RemoveRoamingNetwork(RoamingNetworkId);

            return RoamingNetwork != null;

        }

        #endregion


        #region (internal) SendOnCreateRoamingNetwork_RequestLog(Request)

        internal HTTPRequest SendOnCreateRoamingNetwork_RequestLog(HTTPRequest Request)
        {

            OnCreateRoamingNetwork_RequestLog?.Invoke(Request.Timestamp,
                                                      HTTPServer,
                                                      Request);

            return Request;

        }

        #endregion

        #region (internal) SendOnCreateRoamingNetwork_ResponseLog(Response)

        internal HTTPResponse SendOnCreateRoamingNetwork_ResponseLog(HTTPResponse Response)
        {

            OnCreateRoamingNetwork_ResponseLog?.Invoke(Response.Timestamp,
                                                       HTTPServer,
                                                       Response.HTTPRequest,
                                                       Response);

            return Response;

        }

        #endregion



        #region (protected internal) SendGetEVSEStatus(Request)

        protected internal HTTPRequest SendGetEVSEStatus(HTTPRequest Request)
        {

            OnGetEVSEStatus?.Invoke(Request.Timestamp,
                                    this.HTTPServer,
                                    Request);

            return Request;

        }

        #endregion


        #region (protected internal) Send Reservation events

        #region (protected internal) SendReserveChargingPool(Request)

        protected internal HTTPRequest SendReserveChargingPool(HTTPRequest Request)
        {

            OnReserveChargingPool?.Invoke(Request.Timestamp,
                                          this.HTTPServer,
                                          Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendChargingPoolReserved(Response)

        protected internal HTTPResponse SendChargingPoolReserved(HTTPResponse Response)
        {

            OnChargingPoolReserved?.Invoke(Response.Timestamp,
                                           this.HTTPServer,
                                           Response.HTTPRequest,
                                           Response);

            return Response;

        }

        #endregion

        #region (protected internal) SendReserveChargingStation(Request)

        protected internal HTTPRequest SendReserveChargingStation(HTTPRequest Request)
        {

            OnReserveChargingStation?.Invoke(Request.Timestamp,
                                             this.HTTPServer,
                                             Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendChargingStationReserved(Response)

        protected internal HTTPResponse SendChargingStationReserved(HTTPResponse Response)
        {

            OnChargingStationReserved?.Invoke(Response.Timestamp,
                                              this.HTTPServer,
                                              Response.HTTPRequest,
                                              Response);

            return Response;

        }

        #endregion

        #region (protected internal) SendReserveEVSE(Request)

        protected internal HTTPRequest SendReserveEVSE(HTTPRequest Request)
        {

            OnReserveEVSE?.Invoke(Request.Timestamp,
                                  this.HTTPServer,
                                  Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendEVSEReserved(Response)

        protected internal HTTPResponse SendEVSEReserved(HTTPResponse Response)
        {

            OnEVSEReserved?.Invoke(Response.Timestamp,
                                   this.HTTPServer,
                                   Response.HTTPRequest,
                                   Response);

            return Response;

        }

        #endregion

        #endregion

        #region (protected internal) Send AuthStartRequest/-Response events

        #region (protected internal) SendAuthStartEVSERequest(Request)

        protected internal HTTPRequest SendAuthStartEVSERequest(HTTPRequest Request)
        {

            OnAuthStartEVSERequest?.Invoke(Request.Timestamp,
                                           this.HTTPServer,
                                           Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendAuthStartEVSEResponse(Response)

        protected internal HTTPResponse SendAuthStartEVSEResponse(HTTPResponse Response)
        {

            OnAuthStartEVSEResponse?.Invoke(Response.Timestamp,
                                            this.HTTPServer,
                                            Response.HTTPRequest,
                                            Response);

            return Response;

        }

        #endregion


        #region (protected internal) SendAuthStartChargingStationRequest(Request)

        protected internal HTTPRequest SendAuthStartChargingStationRequest(HTTPRequest Request)
        {

            OnAuthStartChargingStationRequest?.Invoke(Request.Timestamp,
                                                      this.HTTPServer,
                                                      Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendAuthStartChargingStationResponse(Response)

        protected internal HTTPResponse SendAuthStartChargingStationResponse(HTTPResponse Response)
        {

            OnAuthStartChargingStationResponse?.Invoke(Response.Timestamp,
                                                       this.HTTPServer,
                                                       Response.HTTPRequest,
                                                       Response);

            return Response;

        }

        #endregion


        #region (protected internal) SendAuthStopEVSERequest(Request)

        protected internal HTTPRequest SendAuthStopEVSERequest(HTTPRequest Request)
        {

            OnAuthStopEVSERequest?.Invoke(Request.Timestamp,
                                          this.HTTPServer,
                                          Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendAuthStopEVSEResponse(Response)

        protected internal HTTPResponse SendAuthStopEVSEResponse(HTTPResponse Response)
        {

            OnAuthStopEVSEResponse?.Invoke(Response.Timestamp,
                                           this.HTTPServer,
                                           Response.HTTPRequest,
                                           Response);

            return Response;

        }

        #endregion


        #region (protected internal) SendAuthStopChargingStation(Request)

        protected internal HTTPRequest SendAuthStopChargingStation(HTTPRequest Request)
        {

            OnAuthStopChargingStation?.Invoke(Request.Timestamp,
                                              this.HTTPServer,
                                              Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendChargingStationAuthStopped(Response)

        protected internal HTTPResponse SendChargingStationAuthStopped(HTTPResponse Response)
        {

            OnChargingStationAuthStopped?.Invoke(Response.Timestamp,
                                                 this.HTTPServer,
                                                 Response.HTTPRequest,
                                                 Response);

            return Response;

        }

        #endregion

        #endregion

        #region (protected internal) Send RemoteStart/-Started events

        #region (protected internal) SendRemoteStartEVSE(Request)

        protected internal HTTPRequest SendRemoteStartEVSE(HTTPRequest  Request)
        {

            OnRemoteStartEVSE?.Invoke(Request.Timestamp,
                                      this.HTTPServer,
                                      Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendEVSERemoteStarted(Response)

        protected internal HTTPResponse SendEVSERemoteStarted(HTTPResponse  Response)
        {

            OnEVSERemoteStarted?.Invoke(Response.Timestamp,
                                        this.HTTPServer,
                                        Response.HTTPRequest,
                                        Response);

            return Response;

        }

        #endregion

        #region (protected internal) SendRemoteStartChargingStation(Request)

        protected internal HTTPRequest SendRemoteStartChargingStation(HTTPRequest Request)
        {

            OnRemoteStartChargingStation?.Invoke(Request.Timestamp,
                                                 this.HTTPServer,
                                                 Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendChargingStationRemoteStarted(Response)

        protected internal HTTPResponse SendChargingStationRemoteStarted(HTTPResponse Response)
        {

            OnChargingStationRemoteStarted?.Invoke(Response.Timestamp,
                                                   this.HTTPServer,
                                                   Response.HTTPRequest,
                                                   Response);

            return Response;

        }

        #endregion


        #region (protected internal) SendRemoteStopEVSE(Request)

        protected internal HTTPRequest SendRemoteStopEVSE(HTTPRequest Request)
        {

            OnRemoteStopEVSE?.Invoke(Request.Timestamp,
                                     this.HTTPServer,
                                     Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendEVSERemoteStopped(Response)

        protected internal HTTPResponse SendEVSERemoteStopped(HTTPResponse Response)
        {

            OnEVSERemoteStopped?.Invoke(Response.Timestamp,
                                        this.HTTPServer,
                                        Response.HTTPRequest,
                                        Response);

            return Response;

        }

        #endregion

        #region (protected internal) SendRemoteStopChargingStation(Request)

        protected internal HTTPRequest SendRemoteStopChargingStation(HTTPRequest Request)
        {

            OnRemoteStopChargingStation?.Invoke(Request.Timestamp,
                                                this.HTTPServer,
                                                Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendChargingStationRemoteStopped(Response)

        protected internal HTTPResponse SendChargingStationRemoteStopped(HTTPResponse Response)
        {

            OnChargingStationRemoteStopped?.Invoke(Response.Timestamp,
                                                   this.HTTPServer,
                                                   Response.HTTPRequest,
                                                   Response);

            return Response;

        }

        #endregion

        #endregion

        #region (protected internal) Send CDRs

        #region (protected internal) SendCDRsRequest(Request)

        protected internal HTTPRequest SendCDRsRequest(HTTPRequest Request)
        {

            OnSendCDRsRequest?.Invoke(Request.Timestamp,
                                      this.HTTPServer,
                                      Request);

            return Request;

        }

        #endregion

        #region (protected internal) SendCDRsResponse(Response)

        protected internal HTTPResponse SendCDRsResponse(HTTPResponse Response)
        {

            OnSendCDRsResponse?.Invoke(Response.Timestamp,
                                       this.HTTPServer,
                                       Response.HTTPRequest,
                                       Response);

            return Response;

        }

        #endregion

        #endregion


    }

}
