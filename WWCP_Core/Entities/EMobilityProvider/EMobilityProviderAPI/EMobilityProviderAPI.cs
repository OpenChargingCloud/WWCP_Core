/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace cloud.charging.open.protocols.WWCP.MobilityProvider
{

    public class EMobilityProviderAPI : HTTPAPI
    {


        #region (class) APICounters

        public class APICounters
        {

            public APICounterValues  RemoteStart             { get; }
            public APICounterValues  RemoteStop              { get; }

            public APICounters(APICounterValues?  RemoteStart   = null,
                               APICounterValues?  RemoteStop    = null)
            {

                this.RemoteStart  = RemoteStart ?? new APICounterValues();
                this.RemoteStop   = RemoteStop  ?? new APICounterValues();

            }

            public virtual JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("remoteStart",  RemoteStart.ToJSON()),
                       new JProperty("remoteStop",   RemoteStop. ToJSON())
                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const           String    DefaultHTTPServiceName   = "GraphDefined E-Mobility Provider HTTP API v0.1";

        /// <summary>
        /// The default HTTP server TCP port.
        /// </summary>
        public new static readonly IPPort    DefaultHTTPServerPort    = IPPort.Parse(5837);

        /// <summary>
        /// The default HTTP URL path prefix.
        /// </summary>
        public new static readonly HTTPPath  DefaultURLPathPrefix     = HTTPPath.Root;

        /// <summary>
        /// The default e-mobility provider HTTP API logfile name.
        /// </summary>
        public  const              String    DefaultLogfileName       = "WWCP_EMobilityProviderAPI.log";



        private readonly VirtualEMobilityProvider eMobilityProvider;

        #endregion

        #region Properties

//        /// <summary>
//        /// The attached HTTP logger.
//        /// </summary>
//        public new HTTP_Logger             HTTPLogger
//#pragma warning disable CS8603 // Possible null reference return.
//            => base.HTTPLogger as HTTP_Logger;
//#pragma warning restore CS8603 // Possible null reference return.

//        /// <summary>
//        /// The attached Server API logger.
//        /// </summary>
//        public ServerAPILogger?            Logger            { get; }

        public APICounters                 Counters          { get; }

        public Newtonsoft.Json.Formatting  JSONFormatting    { get; set; }

        #endregion

        #region Custom JSON parsers

        public CustomJObjectParserDelegate<RemoteStartRequest>?  CustomRemoteStartRequestParser    { get; set; }

        public CustomJObjectParserDelegate<RemoteStopRequest>?   CustomRemoteStopRequestParser     { get; set; }

        #endregion

        #region Custom JSON serializers

        public CustomJObjectSerializerDelegate<RemoteStartResponse>?  CustomRemoteStartResponseSerializer   { get; set; }
        public CustomJObjectSerializerDelegate<ChargeDetailRecord>?   CustomChargeDetailRecordSerializer    { get; set; }
        public CustomJObjectSerializerDelegate<SendCDRResult>?        CustomSendCDRResultSerializer         { get; set; }
        public CustomJObjectSerializerDelegate<ChargingSession>?      CustomChargingSessionSerializer       { get; set; }
        public CustomJObjectSerializerDelegate<Warning>?              CustomWarningSerializer               { get; set; }

        #endregion

        #region Events

        #region (protected internal) GetLocationsRequest    (Request)

        /// <summary>
        /// An event sent whenever a GET locations request was received.
        /// </summary>
        public HTTPRequestLogEvent OnGetLocationsRequest = new ();

        /// <summary>
        /// An event sent whenever a GET locations request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMSP API.</param>
        /// <param name="Request">An HTTP request.</param>
        protected internal Task GetLocationsRequest(DateTime     Timestamp,
                                                    HTTPAPI      API,
                                                    HTTPRequest  Request)

            => OnGetLocationsRequest.WhenAll(Timestamp,
                                             API ?? this,
                                             Request);

        #endregion

        #region (protected internal) GetLocationsResponse   (Response)

        /// <summary>
        /// An event sent whenever a GET locations response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnGetLocationsResponse = new ();

        /// <summary>
        /// An event sent whenever a GET locations response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the response.</param>
        /// <param name="API">The EMSP API.</param>
        /// <param name="Request">An HTTP request.</param>
        /// <param name="Response">An HTTP response.</param>
        protected internal Task GetLocationsResponse(DateTime      Timestamp,
                                                     HTTPAPI       API,
                                                     HTTPRequest   Request,
                                                     HTTPResponse  Response)

            => OnGetLocationsResponse.WhenAll(Timestamp,
                                              API ?? this,
                                              Request,
                                              Response);

        #endregion

        #endregion

        #region Constructor(s)

        public EMobilityProviderAPI(VirtualEMobilityProvider             EMobilityProvider,

                                    HTTPHostname?                        HTTPHostname                 = null,
                                    String?                              ExternalDNSName              = null,
                                    IPPort?                              HTTPServerPort               = null,
                                    HTTPPath?                            BasePath                     = null,
                                    String?                              HTTPServerName               = DefaultHTTPServerName,

                                    HTTPPath?                            URLPathPrefix                = null,
                                    String?                              HTTPServiceName              = DefaultHTTPServiceName,
                                    String?                              HTMLTemplate                 = null,
                                    JObject?                             APIVersionHashes             = null,

                                    ServerCertificateSelectorDelegate?   ServerCertificateSelector    = null,
                                    RemoteCertificateValidationHandler?  ClientCertificateValidator   = null,
                                    LocalCertificateSelectionHandler?    ClientCertificateSelector    = null,
                                    SslProtocols?                        AllowedTLSProtocols          = null,
                                    Boolean?                             ClientCertificateRequired    = null,
                                    Boolean?                             CheckCertificateRevocation   = null,

                                    ServerThreadNameCreatorDelegate?     ServerThreadNameCreator      = null,
                                    ServerThreadPriorityDelegate?        ServerThreadPrioritySetter   = null,
                                    Boolean?                             ServerThreadIsBackground     = null,
                                    ConnectionIdBuilder?                 ConnectionIdBuilder          = null,
                                    TimeSpan?                            ConnectionTimeout            = null,
                                    UInt32?                              MaxClientConnections         = null,

                                    Boolean?                             DisableMaintenanceTasks      = null,
                                    TimeSpan?                            MaintenanceInitialDelay      = null,
                                    TimeSpan?                            MaintenanceEvery             = null,

                                    Boolean?                             DisableWardenTasks           = null,
                                    TimeSpan?                            WardenInitialDelay           = null,
                                    TimeSpan?                            WardenCheckEvery             = null,

                                    Boolean?                             IsDevelopment                = null,
                                    IEnumerable<String>?                 DevelopmentServers           = null,
                                    Boolean?                             DisableLogging               = null,
                                    String?                              LoggingPath                  = null,
                                    String?                              LogfileName                  = DefaultLogfileName,
                                    LogfileCreatorDelegate?              LogfileCreator               = null,
                                    DNSClient?                           DNSClient                    = null,
                                    Boolean                              AutoStart                    = false)

            : base(HTTPHostname,
                   ExternalDNSName,
                   HTTPServerPort,
                   BasePath,
                   HTTPServerName,

                   URLPathPrefix   ?? DefaultURLPathPrefix,
                   HTTPServiceName ?? DefaultHTTPServiceName,
                   HTMLTemplate,
                   APIVersionHashes,

                   ServerCertificateSelector,
                   ClientCertificateValidator,
                   ClientCertificateSelector,
                   AllowedTLSProtocols,
                   ClientCertificateRequired,
                   CheckCertificateRevocation,

                   ServerThreadNameCreator,
                   ServerThreadPrioritySetter,
                   ServerThreadIsBackground,
                   ConnectionIdBuilder,
                   ConnectionTimeout,
                   MaxClientConnections,

                   DisableMaintenanceTasks,
                   MaintenanceInitialDelay,
                   MaintenanceEvery,

                   DisableWardenTasks,
                   WardenInitialDelay,
                   WardenCheckEvery,

                   IsDevelopment,
                   DevelopmentServers,
                   DisableLogging,
                   LoggingPath,
                   LogfileName,
                   LogfileCreator,
                   DNSClient,
                   AutoStart)

        {

            this.eMobilityProvider  = EMobilityProvider;

            this.Counters           = new APICounters();

            this.JSONFormatting     = Newtonsoft.Json.Formatting.None;

            //base.HTTPLogger         = this.DisableLogging == false
            //                              ? new HTTP_Logger(this,
            //                                                LoggingPath,
            //                                                LoggingContext ?? DefaultLoggingContext,
            //                                                LogfileCreator)
            //                              : null;

            //this.Logger             = this.DisableLogging == false
            //                              ? new ServerAPILogger(this,
            //                                                    LoggingPath,
            //                                                    LoggingContext ?? DefaultLoggingContext,
            //                                                    LogfileCreator)
            //                              : null;

            RegisterURLTemplates();

            if (AutoStart)
                Start();

        }

        #endregion




        #region (private) RegisterURLTemplates()

        private void RegisterURLTemplates()
        {

            #region GET    [/cpo] == /

            //HTTPServer.RegisterResourcesFolder(HTTPHostname.Any,
            //                                   URLPathPrefix + "/cpo", "cloud.charging.open.protocols.HTTPv2_1_1.HTTPAPI.CPOAPI.HTTPRoot",
            //                                   Assembly.GetCallingAssembly());

            //CommonAPI.AddHTTPMethod(HTTPHostname.Any,
            //                             HTTPMethod.GET,
            //                             new HTTPPath[] {
            //                                 URLPathPrefix + "/cpo/index.html",
            //                                 URLPathPrefix + "/cpo/"
            //                             },
            //                             HTTPContentType.HTML_UTF8,
            //                             HTTPRequest: async Request => {

            //                                 var _MemoryStream = new MemoryStream();
            //                                 typeof(CPOAPI).Assembly.GetManifestResourceStream("cloud.charging.open.protocols.HTTPv2_1_1.HTTPAPI.CPOAPI.HTTPRoot._header.html").SeekAndCopyTo(_MemoryStream, 3);
            //                                 typeof(CPOAPI).Assembly.GetManifestResourceStream("cloud.charging.open.protocols.HTTPv2_1_1.HTTPAPI.CPOAPI.HTTPRoot._footer.html").SeekAndCopyTo(_MemoryStream, 3);

            //                                 return new HTTPResponse.Builder(Request.HTTPRequest) {
            //                                     HTTPStatusCode  = HTTPStatusCode.OK,
            //                                     Server          = DefaultHTTPServerName,
            //                                     Date            = Timestamp.Now,
            //                                     ContentType     = HTTPContentType.HTML_UTF8,
            //                                     Content         = _MemoryStream.ToArray(),
            //                                     Connection      = "close"
            //                                 };

            //                             });

            #endregion


            #region ~/locations

            #region OPTIONS  ~/locations

            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.OPTIONS,
                              URLPathPrefix + "locations",
                              HTTPDelegate: Request => {

                                  return Task.FromResult(
                                      new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             AccessControlAllowMethods  = new[] { "OPTIONS", "GET" },
                                             Allow                      = new[] {
                                                                              HTTPMethod.OPTIONS,
                                                                              HTTPMethod.GET
                                                                          },
                                             AccessControlAllowHeaders  = new[] { "Authorization" }
                                      }.AsImmutable);

                              });

            #endregion

            #region GET      ~/locations

            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.GET,
                              URLPathPrefix + "locations",
                              HTTPContentType.JSON_UTF8,
                              HTTPRequestLogger:   GetLocationsRequest,
                              HTTPResponseLogger:  GetLocationsResponse,
                              HTTPDelegate:        Request => {

                                  #region Check access token

                                  //if ((Request.LocalAccessInfo is not null || CommonAPI.LocationsAsOpenData == false) &&
                                  //    (Request.LocalAccessInfo?.Status != AccessStatus.ALLOWED ||
                                  //     Request.LocalAccessInfo?.Role   != Roles.EMSP))
                                  //{

                                  //    return Task.FromResult(
                                  //        new HTTPResponse.Builder(Request) {
                                  //            StatusCode           = 2000,
                                  //            StatusMessage        = "Invalid or blocked access token!",
                                  //            HTTPResponseBuilder  = new HTTPResponse.Builder(Request.HTTPRequest) {
                                  //                HTTPStatusCode             = HTTPStatusCode.Forbidden,
                                  //                AccessControlAllowMethods  = new[] { "OPTIONS", "GET", "DELETE" },
                                  //                AccessControlAllowHeaders  = new[] { "Authorization" }
                                  //            }
                                  //        });

                                  //}

                                  #endregion


                                  //var filters              = Request.GetDateAndPaginationFilters();

                                  //                                                    //ToDo: Filter to NOT show all locations to everyone!
                                  //var allLocations         = CommonAPI.GetLocations().//location => Request.AccessInfo.Value.Roles.Any(role => role.CountryCode == location.CountryCode &&
                                  //                                                    //                                                       role.PartyId     == location.PartyId)).
                                  //                                     ToArray();

                                  //var filteredLocations    = allLocations.Where(location => !filters.From.HasValue || location.LastUpdated >  filters.From.Value).
                                  //                                        Where(location => !filters.To.  HasValue || location.LastUpdated <= filters.To.  Value).
                                  //                                        ToArray();


                                  //var httpResponseBuilder  = new HTTPResponse.Builder(Request.HTTPRequest) {
                                  //                               HTTPStatusCode             = HTTPStatusCode.OK,
                                  //                               Server                     = DefaultHTTPServerName,
                                  //                               Date                       = Timestamp.Now,
                                  //                               AccessControlAllowMethods  = new[] { "OPTIONS", "GET" },
                                  //                               AccessControlAllowHeaders  = new[] { "Authorization" }
                                  //                           }.

                                  //                           // The overall number of locations
                                  //                           Set("X-Total-Count",  allLocations.Length).

                                  //                           // The maximum number of locations that the server WILL return within a single request
                                  //                           Set("X-Limit",        allLocations.Length);


                                  //#region When the limit query parameter was set & this is not the last pagination page...

                                  //if (filters.Limit.HasValue &&
                                  //    allLocations.ULongLength() > ((filters.Offset ?? 0) + (filters.Limit ?? 0)))
                                  //{

                                  //    // The new query parameters for the "next" page of pagination within the HTTP Link header
                                  //    var queryParameters    = new List<String?>() {
                                  //                                 filters.From. HasValue ? $"from={filters.From.Value}" :                             null,
                                  //                                 filters.To.   HasValue ? $"to={filters.To.Value}" :                                 null,
                                  //                                 filters.Limit.HasValue ? $"offset={(filters.Offset ?? 0) + (filters.Limit ?? 0)}" : null,
                                  //                                 filters.Limit.HasValue ? $"limit={filters.Limit ?? 0}" :                            null
                                  //                             }.Where(queryParameter => queryParameter is not null).
                                  //                               AggregateWith("&");

                                  //    if (queryParameters.Length > 0)
                                  //        queryParameters = "?" + queryParameters;

                                  //    // Link to the 'next' page should be provided when this is NOT the last page, e.g.:
                                  //    //   - Link: <https://www.server.com/ocpi/cpo/2.0/cdrs/?offset=150&limit=50>; rel="next"
                                  //    httpResponseBuilder.Set("Link", $"<{(ExternalDNSName.IsNotNullOrEmpty()
                                  //                                             ? $"https://{ExternalDNSName}"
                                  //                                             : $"http://127.0.0.1:{HTTPServer.IPPorts.First()}")}{URLPathPrefix}/locations{queryParameters}>; rel=\"next\"");

                                  //}

                                  //#endregion

                                  //return Task.FromResult(
                                  //           new HTTPResponse.Builder(Request) {
                                  //               StatusCode           = 1000,
                                  //               StatusMessage        = "Hello world!",
                                  //               HTTPResponseBuilder  = httpResponseBuilder,
                                  //               Data                 = new JArray(
                                  //                                          filteredLocations.
                                  //                                              OrderBy       (location => location.Created).
                                  //                                              SkipTakeFilter(filters.Offset,
                                  //                                                             filters.Limit).
                                  //                                              Select        (location => location.ToJSON(false,
                                  //                                                                                         Request.EMSPId,
                                  //                                                                                         CustomLocationSerializer,
                                  //                                                                                         CustomAdditionalGeoLocationSerializer,
                                  //                                                                                         CustomEVSESerializer,
                                  //                                                                                         CustomStatusScheduleSerializer,
                                  //                                                                                         CustomConnectorSerializer,
                                  //                                                                                         CustomEnergyMeterSerializer,
                                  //                                                                                         CustomTransparencySoftwareStatusSerializer,
                                  //                                                                                         CustomTransparencySoftwareSerializer,
                                  //                                                                                         CustomDisplayTextSerializer,
                                  //                                                                                         CustomBusinessDetailsSerializer,
                                  //                                                                                         CustomHoursSerializer,
                                  //                                                                                         CustomImageSerializer,
                                  //                                                                                         CustomEnergyMixSerializer,
                                  //                                                                                         CustomEnergySourceSerializer,
                                  //                                                                                         CustomEnvironmentalImpactSerializer))
                                  //                                      )
                                  //           }
                                  //       );


                                  return Task.FromResult(
                                      new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             AccessControlAllowMethods  = new[] { "OPTIONS", "GET" },
                                             Allow                      = new[] {
                                                                              HTTPMethod.OPTIONS,
                                                                              HTTPMethod.GET
                                                                          },
                                             AccessControlAllowHeaders  = new[] { "Authorization" }
                                      }.AsImmutable);


                              });

            #endregion

            #endregion


            #region POST      ~/remoteStart

            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "remoteStart",
                              HTTPContentType.JSON_UTF8,
                              //HTTPRequestLogger:   GetLocationsRequest,
                              //HTTPResponseLogger:  GetLocationsResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime = Timestamp.Now;

                                  RemoteStartResponse? remoteStartResponse = null;

                                  try
                                  {

                                      var jsonBody = Request.HTTPBodyAsJSONObject;

                                      if (jsonBody is not null &&
                                          RemoteStartRequest.TryParse(jsonBody,
                                                                      out var remoteStartRequest,
                                                                      out var errorResponse,
                                                                      Request.Timestamp,
                                                                      Request.CancellationToken,
                                                                      Request.EventTrackingId,
                                                                      Request.Timeout ?? DefaultRequestTimeout,
                                                                      CustomRemoteStartRequestParser) &&
                                          remoteStartRequest is not null)
                                      {

                                          Counters.RemoteStart.IncRequests_OK();

                                          #region Send OnRemoteStartRequest event

                                          //try
                                          //{

                                          //    if (OnRemoteStartRequest is not null)
                                          //        await Task.WhenAll(OnRemoteStartRequest.GetInvocationList().
                                          //                           Cast<OnRemoteStartRequestDelegate>().
                                          //                           Select(e => e(Timestamp.Now,
                                          //                                         this,
                                          //                                         authorizeRemoteStartRequest!))).
                                          //                           ConfigureAwait(false);

                                          //}
                                          //catch (Exception e)
                                          //{
                                          //    DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnRemoteStartRequest));
                                          //}

                                          #endregion

                                          #region Call async subscribers

                                          try
                                          {

                                              var remoteStartResult = await eMobilityProvider.RemoteStart(remoteStartRequest.ChargingLocation,
                                                                                                          remoteStartRequest.ChargingProduct,
                                                                                                          remoteStartRequest.ReservationId,
                                                                                                          remoteStartRequest.RemoteAuthentication,
                                                                                                          remoteStartRequest.AuthenticationPath,
                                                                                                          remoteStartRequest.ChargingSessionId,
                                                                                                          remoteStartRequest.Timestamp,
                                                                                                          remoteStartRequest.EventTrackingId,
                                                                                                          remoteStartRequest.RequestTimeout,
                                                                                                          remoteStartRequest.CancellationToken);

                                              remoteStartResponse = new RemoteStartResponse(remoteStartRequest,
                                                                                            remoteStartResult.Result,
                                                                                            remoteStartRequest.EventTrackingId,
                                                                                            Timestamp.Now,
                                                                                            remoteStartResult.Session,
                                                                                            remoteStartResult.Description,
                                                                                            remoteStartResult.AdditionalInfo,
                                                                                            Array.Empty<Warning>(),
                                                                                            null,
                                                                                            remoteStartResult.Runtime);

                                              //authorizeRemoteStartResponse = (await Task.WhenAll(OnRemoteStartLocal.GetInvocationList().
                                              //                                                                  Cast<OnRemoteStartDelegate>().
                                              //                                                                  Select(e => e(Timestamp.Now,
                                              //                                                                                this,
                                              //                                                                                authorizeRemoteStartRequest!))))?.FirstOrDefault();

                                              Counters.RemoteStart.IncResponses_OK();

                                          }
                                          catch (Exception e)
                                          {

                                              Counters.RemoteStart.IncResponses_Error();

                                              //authorizeRemoteStartResponse = Acknowledgement<RemoteStartRequest>.DataError(
                                              //                                   Request:                   authorizeRemoteStartRequest,
                                              //                                   StatusCodeDescription:     e.Message,
                                              //                                   StatusCodeAdditionalInfo:  e.StackTrace,
                                              //                                   SessionId:                 authorizeRemoteStartRequest?.SessionId,
                                              //                                   CPOPartnerSessionId:       authorizeRemoteStartRequest?.CPOPartnerSessionId
                                              //                               );

                                          }

                                          if (remoteStartResponse is null)
                                          {

                                              Counters.RemoteStart.IncResponses_Error();

                                              //remoteStartResponse = Acknowledgement<RemoteStartRequest>.SystemError(
                                              //                          authorizeRemoteStartRequest,
                                              //                          "Could not process the received RemoteStart request!"
                                              //                      );

                                          }

                                          #endregion

                                          #region Send OnRemoteStartResponse event

                                          //try
                                          //{

                                          //    if (OnRemoteStartResponse is not null)
                                          //        await Task.WhenAll(OnRemoteStartResponse.GetInvocationList().
                                          //                           Cast<OnRemoteStartResponseDelegate>().
                                          //                           Select(e => e(Timestamp.Now,
                                          //                                         this,
                                          //                                         authorizeRemoteStartRequest!,
                                          //                                         authorizeRemoteStartResponse,
                                          //                                         Timestamp.Now - startTime))).
                                          //                           ConfigureAwait(false);

                                          //}
                                          //catch (Exception e)
                                          //{
                                          //    DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnRemoteStartResponse));
                                          //}

                                          #endregion

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      remoteStartResponse = new RemoteStartResponse(null,
                                                                                    RemoteStartResultTypes.Error,
                                                                                    EventTracking_Id.New,
                                                                                    Timestamp.Now,
                                                                                    null,
                                                                                    I18NString.Create(Languages.en, e.Message),
                                                                                    e.StackTrace,
                                                                                    Array.Empty<Warning>(),
                                                                                    null,
                                                                                    TimeSpan.Zero);

                                  }

                                  remoteStartResponse ??= new RemoteStartResponse(null,
                                                                                  RemoteStartResultTypes.Error,
                                                                                  EventTracking_Id.New,
                                                                                  Timestamp.Now,
                                                                                  null,
                                                                                  null,
                                                                                  null,
                                                                                  Array.Empty<Warning>(),
                                                                                  null,
                                                                                  TimeSpan.Zero);


                                  if (remoteStartResponse.Result == RemoteStartResultTypes.Success ||
                                      remoteStartResponse.Result == RemoteStartResultTypes.AsyncOperation)
                                  {

                                      return new HTTPResponse.Builder(Request) {
                                                 HTTPStatusCode             = HTTPStatusCode.OK,
                                                 AccessControlAllowMethods  = new[] { "OPTIONS", "POST" },
                                                 Allow                      = new[] {
                                                                                  HTTPMethod.OPTIONS,
                                                                                  HTTPMethod.POST
                                                                              },
                                                 ContentType                = HTTPContentType.JSON_UTF8,
                                                 Content                    = remoteStartResponse.ToJSON(CustomRemoteStartResponseSerializer,
                                                                                                         CustomChargeDetailRecordSerializer,
                                                                                                         CustomSendCDRResultSerializer,
                                                                                                         CustomChargingSessionSerializer,
                                                                                                         CustomWarningSerializer).
                                                                                                  ToString(JSONFormatting).
                                                                                                  ToUTF8Bytes(),
                                                 AccessControlAllowHeaders  = new[] { "Authorization" }
                                          }.AsImmutable;

                                  }

                                  else
                                  {

                                      return new HTTPResponse.Builder(Request) {
                                                 HTTPStatusCode             = HTTPStatusCode.Forbidden,
                                                 AccessControlAllowMethods  = new[] { "OPTIONS", "POST" },
                                                 Allow                      = new[] {
                                                                                  HTTPMethod.OPTIONS,
                                                                                  HTTPMethod.POST
                                                                              },
                                                 AccessControlAllowHeaders  = new[] { "Authorization" }
                                          }.AsImmutable;

                                  }

                              });

            #endregion


        }

        #endregion


    }

}
