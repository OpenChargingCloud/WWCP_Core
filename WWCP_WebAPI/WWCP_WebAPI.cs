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

using System.Reflection;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.HTTPTest;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// WWCP WebAPI extension methods.
    /// </summary>
    public static class ExtensionMethods
    {


    }


    /// <summary>
    /// A HTTP API providing advanced WWCP data structures.
    /// </summary>
    public class WWCP_WebAPI : AHTTPExtAPIXExtension2<WWCP_HTTPAPI, HTTPExtAPIX>
    {

        #region Data

        /// <summary>
        /// The default HTTP URL prefix.
        /// </summary>
        public     static readonly  HTTPPath       DefaultURLPathPrefix      = HTTPPath.Parse("webapi");

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const            String         DefaultHTTPServerName     = "Open Charging Cloud WWCP WebAPI";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const            String         DefaultHTTPServiceName    = "Open Charging Cloud WWCP WebAPI";

        /// <summary>
        /// The HTTP root for embedded resources.
        /// </summary>
        public const                String         HTTPRoot                  = "cloud.charging.open.protocols.WWCP.WebAPI.HTTPRoot.";

        ///// <summary>
        ///// The default HTTP realm, if HTTP Basic Authentication is used.
        ///// </summary>
        //public const                String         DefaultHTTPRealm           = $"Open Charging Cloud WWCP {Version.String} WebAPI";


        //ToDo: http://www.iana.org/form/media-types

        ///// <summary>
        ///// The HTTP content type for serving WWCP+ XML data.
        ///// </summary>
        //public static readonly HTTPContentType                      WWCPPlusJSONContentType    = new ("application", "vnd.WWCPPlus+json", "utf-8", null, null);

        ///// <summary>
        ///// The HTTP content type for serving WWCP+ HTML data.
        ///// </summary>
        //public static readonly HTTPContentType                      WWCPPlusHTMLContentType    = new ("application", "vnd.WWCPPlus+html", "utf-8", null, null);


        public static readonly HTTPEventSource_Id  DebugLogId                = HTTPEventSource_Id.Parse("WWCP_debugLog");

        /// <summary>
        /// The default WebAPI logfile name.
        /// </summary>
        public  const          String              DefaultLogfileName        = "WWCP_WebAPI.log";

        #endregion

        #region Properties

        public WWCP_HTTPAPI              WWCP_HTTPAPI
            => HTTPBaseAPI;


        public HTTPPath?                 OverlayURLPathPrefix    { get; }

        /// <summary>
        /// The HTTP URI prefix.
        /// </summary>
        public HTTPPath?                 APIURLPathPrefix        { get; }

        /// <summary>
        /// Make use of HTTP Server Sent Events for debug information.
        /// </summary>
        public ServiceSettings           UseHTTPSSE              { get; }

        /// <summary>
        /// Debug information via HTTP Server Sent Events.
        /// </summary>
        public HTTPEventSource<JObject>  DebugLog                { get; }

        #endregion

        #region Events

        #region Generic HTTP server logging

        ///// <summary>
        ///// An event called whenever a HTTP request came in.
        ///// </summary>
        //public HTTPRequestLogEvent   RequestLog    = new HTTPRequestLogEvent();

        ///// <summary>
        ///// An event called whenever a HTTP request could successfully be processed.
        ///// </summary>
        //public HTTPResponseLogEvent  ResponseLog   = new HTTPResponseLogEvent();

        ///// <summary>
        ///// An event called whenever a HTTP request resulted in an error.
        ///// </summary>
        //public HTTPErrorLogEvent     ErrorLog      = new HTTPErrorLogEvent();

        #endregion

        #endregion

        #region Constructor(s)

        static WWCP_WebAPI()
        {
            // Using static variables within normal constructors seems to
            // have a problem setting them up to their expected values!
        }

        /// <summary>
        /// Attach the WWCP WebAPI to the given HTTP server.
        /// </summary>
        /// <param name="HTTPAPI">The WWCP HTTP API.</param>
        /// 
        /// <param name="OverlayURLPathPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="APIURLPathPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="WebAPIURLPathPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="BasePath">The base path of the HTTP server.</param>
        public WWCP_WebAPI(WWCP_HTTPAPI             HTTPAPI,

                           I18NString?              Description                     = null,

                           HTTPPath?                OverlayURLPathPrefix            = null,
                           HTTPPath?                APIURLPathPrefix                = null,
                           HTTPPath?                WebAPIURLPathPrefix             = null,
                           HTTPPath?                BasePath                        = null,  // For URL prefixes in HTML!

                           ServiceSettings?         UseHTTPSSE                      = null,

                           String?                  ExternalDNSName                 = null,
                           String?                  HTTPServerName                  = DefaultHTTPServerName,
                           String?                  HTTPServiceName                 = DefaultHTTPServiceName,
                           String?                  APIVersionHash                  = null,
                           JObject?                 APIVersionHashes                = null,

                           Boolean?                 IsDevelopment                   = null,
                           IEnumerable<String>?     DevelopmentServers              = null,
                           Boolean?                 DisableNotifications            = null,
                           Boolean?                 DisableLogging                  = null,
                           String?                  LoggingPath                     = null,
                           String?                  LogfileName                     = null,
                           LogfileCreatorDelegate?  LogfileCreator                  = null)

            : base(HTTPAPI,
                   OverlayURLPathPrefix + WebAPIURLPathPrefix,
                   HTTPAPI.BasePath     + BasePath,

                   Description     ?? I18NString.Create($"WWCP Web API"),

                   ExternalDNSName,
                   HTTPServerName  ?? DefaultHTTPServerName,
                   HTTPServiceName ?? DefaultHTTPServiceName,
                   APIVersionHash,
                   APIVersionHashes,

                   IsDevelopment,
                   DevelopmentServers,
                   DisableLogging,
                   LoggingPath,
                   LogfileName     ?? DefaultLogfileName,
                   LogfileCreator)

        {

            this.OverlayURLPathPrefix  = OverlayURLPathPrefix ?? HTTPPath.Root;
            this.APIURLPathPrefix      = APIURLPathPrefix;

            //this.HTTPRealm             = HTTPRealm.IsNotNullOrEmpty() ? HTTPRealm : DefaultHTTPRealm;
            //this.HTTPLogins            = HTTPLogins ?? [];

            this.UseHTTPSSE            = UseHTTPSSE           ?? ServiceSettings.Disabled;

            this.DebugLog              = HTTPBaseAPI.HTTPBaseAPI.AddJSONEventSource(
                                             EventSourceId:            DebugLogId,
                                             MaxNumberOfCachedEvents:  1000,
                                             RetryInterval :           TimeSpan.FromSeconds(5),
                                             EnableLogging:            true,
                                             LogfilePrefix:            this.LoggingPath + "HTTPSSEs" + Path.DirectorySeparatorChar
                                         );

            RegisterURLTemplates();

        }

        #endregion


        #region (private) RegisterURLTemplates()

        #region Manage HTTP Resources

        private readonly Tuple<String, Assembly>[] resourceAssemblies = [
            new Tuple<String, Assembly>(WWCP_WebAPI.HTTPRoot, typeof(WWCP_WebAPI).Assembly),
            new Tuple<String, Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly)
        ];

        #region (protected override) GetResourceStream      (ResourceName)

        protected override Stream? GetResourceStream(String ResourceName)

            => GetResourceStream(
                   ResourceName,
                   resourceAssemblies
               );

        #endregion

        #region (protected override) GetResourceMemoryStream(ResourceName)

        protected override MemoryStream? GetResourceMemoryStream(String ResourceName)

            => GetResourceMemoryStream(
                   ResourceName,
                   resourceAssemblies
               );

        #endregion

        #region (protected override) GetResourceString      (ResourceName)

        protected override String GetResourceString(String ResourceName)

            => GetResourceString(
                   ResourceName,
                   resourceAssemblies
               );

        #endregion

        #region (protected override) GetResourceBytes       (ResourceName)

        protected override Byte[] GetResourceBytes(String ResourceName)

            => GetResourceBytes(
                   ResourceName,
                   resourceAssemblies
               );

        #endregion

        #region (protected override) MixWithHTMLTemplate    (ResourceName)

        protected override String MixWithHTMLTemplate(String ResourceName)

            => MixWithHTMLTemplate(
                   ResourceName,
                   resourceAssemblies
               );

        #endregion

        #region (protected override) MixWithHTMLTemplate    (ResourceName, HTMLConverter)

        protected override String MixWithHTMLTemplate(String ResourceName, Func<String, String> HTMLConverter)

            => MixWithHTMLTemplate(
                   ResourceName,
                   HTMLConverter,
                   resourceAssemblies
               );

        #endregion

        #endregion


        /// <summary>
        /// The following will register HTTP overlays for text/html
        /// showing a html representation of the WWCP WebAPI!
        /// </summary>
        private void RegisterURLTemplates()
        {

            #region / (HTTPRoot)

            WWCP_HTTPAPI.HTTPBaseAPI.MapResourceAssemblyFolder(
                HTTPHostname.Any,
                URLPathPrefix,
                HTTPRoot,
                RequireAuthentication:  false,
                DefaultFilename:       "index.html"
            );

            #endregion


            if (OverlayURLPathPrefix.HasValue)
            {

                #region GET ~/

                #region Text

                //CommonHTTPAPI.HTTPBaseAPI.AddHandler(
                //    HTTPMethod.GET,
                //    OverlayURLPathPrefix.Value,
                //    HTTPContentType.Text.HTML_UTF8,
                //    HTTPDelegate: request =>

                //        Task.FromResult(
                //            new HTTPResponse.Builder(request) {
                //                HTTPStatusCode             = HTTPStatusCode.OK,
                //                Server                     = HTTPServiceName,
                //                Date                       = Timestamp.Now,
                //                AccessControlAllowOrigin   = "*",
                //                AccessControlAllowMethods  = [ "OPTIONS", "GET" ],
                //                AccessControlAllowHeaders  = [ "Authorization" ],
                //                ContentType                = HTTPContentType.Text.PLAIN,
                //                Content                    = ("This is a World Wide Charging Protocol HTTP service!" + Environment.NewLine + "Please check ~/ versions!").ToUTF8Bytes(),
                //                Connection                 = ConnectionType.KeepAlive,
                //                Vary                       = "Accept"
                //            }.AsImmutable),

                //    AllowReplacement: URLReplacement.Allow

                //);


                //// Just for convenience...
                //if (OverlayURLPathPrefix.Value != HTTPPath.Root)
                //    CommonHTTPAPI.HTTPBaseAPI.AddHandler(
                //        HTTPMethod.GET,
                //        OverlayURLPathPrefix.Value + "/",
                //        HTTPContentType.Text.HTML_UTF8,
                //        HTTPDelegate: request =>

                //            Task.FromResult(
                //                new HTTPResponse.Builder(request) {
                //                    HTTPStatusCode             = HTTPStatusCode.OK,
                //                    Server                     = HTTPServiceName,
                //                    Date                       = Timestamp.Now,
                //                    AccessControlAllowOrigin   = "*",
                //                    AccessControlAllowMethods  = [ "OPTIONS", "GET" ],
                //                    AccessControlAllowHeaders  = [ "Authorization" ],
                //                    ContentType                = HTTPContentType.Text.PLAIN,
                //                    Content                    = ("This is a World Wide Charging Protocol HTTP service!" + Environment.NewLine + "Please check ~/ versions!").ToUTF8Bytes(),
                //                    Connection                 = ConnectionType.KeepAlive,
                //                    Vary                       = "Accept"
                //                }.AsImmutable),

                //        AllowReplacement: URLReplacement.Allow

                //    );

                #endregion

                #region JSON

                //CommonHTTPAPI.HTTPBaseAPI.AddHandler(
                //    HTTPMethod.GET,
                //    OverlayURLPathPrefix.Value,
                //    HTTPContentType.Application.JSON_UTF8,
                //    HTTPDelegate: request =>

                //        Task.FromResult(
                //            new HTTPResponse.Builder(request) {
                //                HTTPStatusCode             = HTTPStatusCode.OK,
                //                Server                     = HTTPServiceName,
                //                Date                       = Timestamp.Now,
                //                AccessControlAllowOrigin   = "*",
                //                AccessControlAllowMethods  = [ "OPTIONS", "GET" ],
                //                AccessControlAllowHeaders  = [ "Authorization" ],
                //                ContentType                = HTTPContentType.Application.JSON_UTF8,
                //                Content                    = JSONObject.Create(
                //                                                 new JProperty(
                //                                                     "message",
                //                                                     "This is a World Wide Charging Protocol HTTP service! Please check ~/ versions!"
                //                                                 )
                //                                             ).ToUTF8Bytes(),
                //                Connection                 = ConnectionType.KeepAlive,
                //                Vary                       = "Accept"
                //            }.AsImmutable),

                //    AllowReplacement: URLReplacement.Allow

                //);


                //// Just for convenience...
                //if (OverlayURLPathPrefix.Value != HTTPPath.Root)
                //    CommonHTTPAPI.HTTPBaseAPI.AddHandler(
                //        HTTPMethod.GET,
                //        OverlayURLPathPrefix.Value + "/",
                //        HTTPContentType.Text.HTML_UTF8,
                //        HTTPDelegate: request =>

                //            Task.FromResult(
                //                new HTTPResponse.Builder(request) {
                //                    HTTPStatusCode             = HTTPStatusCode.OK,
                //                    Server                     = HTTPServiceName,
                //                    Date                       = Timestamp.Now,
                //                    AccessControlAllowOrigin   = "*",
                //                    AccessControlAllowMethods  = [ "OPTIONS", "GET" ],
                //                    AccessControlAllowHeaders  = [ "Authorization" ],
                //                    ContentType                = HTTPContentType.Application.JSON_UTF8,
                //                    Content                    = JSONObject.Create(
                //                                                     new JProperty(
                //                                                         "message",
                //                                                         "This is a World Wide Charging Protocol HTTP service! Please check ~/ versions!"
                //                                                     )
                //                                                 ).ToUTF8Bytes(),
                //                    Connection                 = ConnectionType.KeepAlive,
                //                    Vary                       = "Accept"
                //                }.AsImmutable),

                //        AllowReplacement: URLReplacement.Allow

                //    );

                #endregion

                #region HTML

                WWCP_HTTPAPI.HTTPBaseAPI.AddHandler(
                    HTTPMethod.GET,
                    OverlayURLPathPrefix.Value,
                    HTTPContentType.Text.HTML_UTF8,
                    HTTPDelegate: request =>

                        Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.OK,
                                Server                     = HTTPServiceName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "OPTIONS", "GET" ],
                                AccessControlAllowHeaders  = [ "Authorization" ],
                                ContentType                = HTTPContentType.Text.HTML_UTF8,
                                Content                    = MixWithHTMLTemplate(
                                                                 "index.shtml",
                                                                 html => html.Replace("{{versionPath}}", "")
                                                             ).ToUTF8Bytes(),
                                Connection                 = ConnectionType.KeepAlive,
                                Vary                       = "Accept"
                            }.AsImmutable),

                    AllowReplacement: URLReplacement.Allow

                );


                //// Just for convenience...
                //if (OverlayURLPathPrefix.Value != HTTPPath.Root)
                //    CommonHTTPAPI.HTTPBaseAPI.AddHandler(
                //        HTTPMethod.GET,
                //        OverlayURLPathPrefix.Value + "/",
                //        HTTPContentType.Text.HTML_UTF8,
                //        HTTPDelegate: request =>

                //            Task.FromResult(
                //                new HTTPResponse.Builder(request) {
                //                    HTTPStatusCode             = HTTPStatusCode.OK,
                //                    Server                     = HTTPServiceName,
                //                    Date                       = Timestamp.Now,
                //                    AccessControlAllowOrigin   = "*",
                //                    AccessControlAllowMethods  = [ "OPTIONS", "GET" ],
                //                    AccessControlAllowHeaders  = [ "Authorization" ],
                //                    ContentType                = HTTPContentType.Text.HTML_UTF8,
                //                    Content                    = MixWithHTMLTemplate(
                //                                                     "index.shtml",
                //                                                     html => html.Replace("{{versionPath}}", "")
                //                                                 ).ToUTF8Bytes(),
                //                    Connection                 = ConnectionType.KeepAlive,
                //                    Vary                       = "Accept"
                //                }.AsImmutable),

                //        AllowReplacement: URLReplacement.Allow

                //    );

                #endregion

                #endregion


                #region GET ~/debugLog

                if (UseHTTPSSE != ServiceSettings.Disabled)
                {

                    HTTPBaseAPI.HTTPBaseAPI.MapJSONEventSource(
                        DebugLog,
                        OverlayURLPathPrefix.Value + "debugLog",
                        RequireAuthentication:  UseHTTPSSE == ServiceSettings.RequiresAuthentication
                    );

                    WWCP_HTTPAPI.HTTPBaseAPI.AddHandler(
                        HTTPMethod.GET,
                        OverlayURLPathPrefix.Value + "debug",
                        HTTPContentType.Text.HTML_UTF8,
                        HTTPDelegate: async request => {

                            #region Check authentication

                            if (request.User == null &&
                                UseHTTPSSE == ServiceSettings.RequiresAuthentication)
                            {

                                //ToDo: Maybe redirect to a login page instead of sending a 401?
                                return new HTTPResponse.Builder(request) {
                                           HTTPStatusCode             = HTTPStatusCode.Unauthorized,
                                           Server                     = HTTPServerName,
                                           Date                       = Timestamp.Now,
                                           AccessControlAllowOrigin   = "*",
                                           AccessControlAllowMethods  = [ "GET" ],
                                           AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                           Connection                 = ConnectionType.Close,
                                           Vary                       = "Accept"
                                       }.AsImmutable;

                            }

                            #endregion


                            return new HTTPResponse.Builder(request) {
                                       HTTPStatusCode             = HTTPStatusCode.OK,
                                       Server                     = HTTPServerName,
                                       Date                       = Timestamp.Now,
                                       AccessControlAllowOrigin   = "*",
                                       AccessControlAllowMethods  = [ "GET" ],
                                       AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                       ContentType                = HTTPContentType.Text.HTML_UTF8,
                                       Content                    = MixWithHTMLTemplate("debugLog.debugLog.shtml").ToUTF8Bytes(),
                                       Connection                 = ConnectionType.KeepAlive,
                                       Vary                       = "Accept"
                                   }.AsImmutable;

                        }

                    );

                }

                #endregion


                #region GET ~/support

                WWCP_HTTPAPI.HTTPBaseAPI.AddHandler(
                    HTTPMethod.GET,
                    OverlayURLPathPrefix.Value + "/support",
                    HTTPContentType.Text.HTML_UTF8,
                    HTTPDelegate: request =>

                        Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.OK,
                                Server                     = HTTPServerName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                = HTTPContentType.Text.HTML_UTF8,
                                Content                    = MixWithHTMLTemplate("support.support.shtml").ToUTF8Bytes(),
                                Connection                 = ConnectionType.KeepAlive,
                                Vary                       = "Accept"
                            }.AsImmutable
                        )

                );

                #endregion

                #region GET ~/favicon.png

                WWCP_HTTPAPI.HTTPBaseAPI.AddHandler(
                    HTTPMethod.GET,
                    OverlayURLPathPrefix.Value + "/favicon.png",
                    //HTTPContentType.Image.PNG,
                    HTTPDelegate: request =>

                        Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode             = HTTPStatusCode.OK,
                                Server                     = HTTPServerName,
                                Date                       = Timestamp.Now,
                                AccessControlAllowOrigin   = "*",
                                AccessControlAllowMethods  = [ "GET" ],
                                AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                ContentType                = HTTPContentType.Image.PNG,
                                Content                    = GetResourceBytes("images.favicon_big.png"),
                                Connection                 = ConnectionType.KeepAlive
                            }.AsImmutable
                        )

                );

                #endregion

            }

        }

        #endregion


    }

}
