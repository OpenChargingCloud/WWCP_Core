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

using System.Diagnostics.CodeAnalysis;

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
        /// The default HTTP URI prefix.
        /// </summary>
        public static readonly  HTTPPath            DefaultURLPathPrefix      = HTTPPath.Parse("webapi");

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public const            String              DefaultHTTPServerName     = $"Open Charging Cloud WWCP {Version.String} WebAPI";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public const            String              DefaultHTTPServiceName    = $"Open Charging Cloud WWCP {Version.String} WebAPI";

        /// <summary>
        /// The default HTTP realm, if HTTP Basic Authentication is used.
        /// </summary>
        public const            String              DefaultHTTPRealm          = $"Open Charging Cloud WWCP {Version.String} WebAPI";

        /// <summary>
        /// The HTTP root for embedded resources.
        /// </summary>
        public const            String              HTTPRoot                   = "cloud.charging.open.protocols.WWCPv2_1_1.WebAPI.HTTPRoot.";


        //ToDo: http://www.iana.org/form/media-types

        ///// <summary>
        ///// The HTTP content type for serving WWCP+ XML data.
        ///// </summary>
        //public static readonly HTTPContentType                      WWCPPlusJSONContentType    = new ("application", "vnd.WWCPPlus+json", "utf-8", null, null);

        ///// <summary>
        ///// The HTTP content type for serving WWCP+ HTML data.
        ///// </summary>
        //public static readonly HTTPContentType                      WWCPPlusHTMLContentType    = new ("application", "vnd.WWCPPlus+html", "utf-8", null, null);


        public static readonly HTTPEventSource_Id  DebugLogId                 = HTTPEventSource_Id.Parse($"WWCP{Version.String}_debugLog");

        /// <summary>
        /// The default WebAPI logfile name.
        /// </summary>
        public  const          String              DefaultLogfileName         = $"WWCP{Version.String}_WebAPI.log";

        #endregion

        #region Properties

        //public CommonWebAPI             CommonWebAPI    { get; }

        //public CommonAPI                CommonAPI
        //    => HTTPBaseAPI;


        public HTTPPath                                     OverlayURLPathPrefix    { get; }

        /// <summary>
        /// The HTTP URI prefix.
        /// </summary>
        public HTTPPath?                                    APIURLPathPrefix        { get; }

        /// <summary>
        /// Send debug information via HTTP Server Sent Events.
        /// </summary>
    //    public HTTPEventSource<JObject>                     DebugLog                { get; }


        //public CPOAPI?                                      CPOAPI                  { get; set; }

        //public CPOAPILogger?                                CPOAPILogger            { get; set; }


        //public EMSPAPI?                                     EMSPAPI                 { get; set; }

        //public EMSPAPILogger?                               EMSPAPILogger           { get; set; }

        /// <summary>
        /// The default request timeout for new CPO/EMSP clients.
        /// </summary>
        //public TimeSpan?                                    RequestTimeout          { get; set; }

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
        /// <param name="HTTPBaseAPI.HTTPServer">A HTTP server.</param>
        /// <param name="WebAPIURLPathPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="HTTPRealm">The HTTP realm, if HTTP Basic Authentication is used.</param>
        /// <param name="HTTPLogins">An enumeration of logins for an optional HTTP Basic Authentication.</param>
        public WWCP_WebAPI(WWCP_HTTPAPI             HTTPAPI,

                           I18NString?              Description            = null,

                           HTTPPath?                OverlayURLPathPrefix   = null,
                           HTTPPath?                APIURLPathPrefix       = null,
                           HTTPPath?                WebAPIURLPathPrefix    = null,
                           HTTPPath?                BasePath               = null,  // For URL prefixes in HTML!

                           String?                  ExternalDNSName        = null,
                           String?                  HTTPServerName         = DefaultHTTPServerName,
                           String?                  HTTPServiceName        = DefaultHTTPServiceName,
                           String?                  APIVersionHash         = null,
                           JObject?                 APIVersionHashes       = null,

                           Boolean?                 IsDevelopment          = false,
                           IEnumerable<String>?     DevelopmentServers     = null,
                           Boolean?                 DisableNotifications   = false,
                           Boolean?                 DisableLogging         = false,
                           String?                  LoggingPath            = null,
                           String?                  LogfileName            = null,
                           LogfileCreatorDelegate?  LogfileCreator         = null)

            : base(HTTPAPI,
                   OverlayURLPathPrefix + Version.String + WebAPIURLPathPrefix,
                   HTTPAPI.BasePath + Version.String + BasePath,

                   Description     ?? I18NString.Create($"WWCP{Version.String} Web API"),

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
      //      this.VersionPath           = VersionPath ?? Version.String[..4];

            //this.HTTPRealm             = HTTPRealm.IsNotNullOrEmpty() ? HTTPRealm : DefaultHTTPRealm;
            //this.HTTPLogins            = HTTPLogins ?? [];

            //this.cpoClients            = new List<CPOClient>();
            //this.emspClients           = new List<EMSPClient>();

            var LogfilePrefix          = "HTTPSSEs" + Path.DirectorySeparatorChar;

            //this.DebugLog              = this.AddJSONEventSource(EventIdentification:      DebugLogId,
            //                                                     URLTemplate:              this.URLPathPrefix + DebugLogId.ToString(),
            //                                                     MaxNumberOfCachedEvents:  10000,
            //                                                     RetryInterval :           TimeSpan.FromSeconds(5),
            //                                                     EnableLogging:            true,
            //                                                     LogfilePrefix:            LogfilePrefix);

            RegisterURITemplates();

            //this.RequestTimeout        = RequestTimeout;

        }

        #endregion


        #region (private) RegisterURLTemplates()

        #region Manage HTTP Resources

        #region (protected override) GetResourceStream      (ResourceName)

        protected override Stream? GetResourceStream(String ResourceName)

            => GetResourceStream(
                   ResourceName,
                   new Tuple<String, System.Reflection.Assembly>(WWCP_WebAPI.HTTPRoot, typeof(WWCP_WebAPI).Assembly),
                   new Tuple<String, System.Reflection.Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly)
               );

        #endregion

        #region (protected override) GetResourceMemoryStream(ResourceName)

        protected override MemoryStream? GetResourceMemoryStream(String ResourceName)

            => GetResourceMemoryStream(
                   ResourceName,
                   new Tuple<String, System.Reflection.Assembly>(WWCP_WebAPI.HTTPRoot, typeof(WWCP_WebAPI).Assembly),
                   new Tuple<String, System.Reflection.Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly)
               );

        #endregion

        #region (protected override) GetResourceString      (ResourceName)

        protected override String GetResourceString(String ResourceName)

            => GetResourceString(
                   ResourceName,
                   new Tuple<String, System.Reflection.Assembly>(WWCP_WebAPI.HTTPRoot, typeof(WWCP_WebAPI).Assembly),
                   new Tuple<String, System.Reflection.Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly)
               );

        #endregion

        #region (protected override) GetResourceBytes       (ResourceName)

        protected override Byte[] GetResourceBytes(String ResourceName)

            => GetResourceBytes(
                   ResourceName,
                   new Tuple<String, System.Reflection.Assembly>(WWCP_WebAPI.HTTPRoot, typeof(WWCP_WebAPI).Assembly),
                   new Tuple<String, System.Reflection.Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly)
               );

        #endregion

        #region (protected override) MixWithHTMLTemplate    (ResourceName)

        protected override String MixWithHTMLTemplate(String ResourceName)

            => MixWithHTMLTemplate(
                   ResourceName,
                   new Tuple<String, System.Reflection.Assembly>(WWCP_WebAPI.HTTPRoot, typeof(WWCP_WebAPI).Assembly),
                   new Tuple<String, System.Reflection.Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly)
               );

        #endregion

        #region (protected override) MixWithHTMLTemplate    (ResourceName, HTMLConverter)

        protected override String MixWithHTMLTemplate(String ResourceName, Func<String, String> HTMLConverter)

            => MixWithHTMLTemplate(
                   ResourceName,
                   HTMLConverter,
                   new Tuple<String, System.Reflection.Assembly>(WWCP_WebAPI.HTTPRoot, typeof(WWCP_WebAPI).Assembly),
                   new Tuple<String, System.Reflection.Assembly>(HTTPAPI.    HTTPRoot, typeof(HTTPAPI).    Assembly)
               );

        #endregion

        #endregion

        /// <summary>
        /// The following will register HTTP overlays for text/html
        /// showing a html representation of the WWCP WebAPI!
        /// </summary>
        private void RegisterURITemplates()
        {

     

        }

        #endregion



    }

}
