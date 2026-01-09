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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A WWCP HTTP API logger.
    /// </summary>
    public class WWCP_HTTPAPI_Logger : HTTPServerLoggerX
    {

        #region Data

        /// <summary>
        /// The default context of this logger.
        /// </summary>
        public const String  DefaultContext   = $"WWCP_WWCP HTTP API";

        #endregion

        #region Properties

        /// <summary>
        /// The linked HTTP API.
        /// </summary>
        public WWCP_HTTPAPI  HTTPAPI    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP HTTP API logger using the default logging delegates.
        /// </summary>
        /// <param name="HTTPAPI">An WWCP WWCP HTTP API.</param>
        /// <param name="Context">A context of this API.</param>
        /// 
        /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
        /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
        /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
        /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
        /// 
        /// <param name="LogHTTPRequest_toNetwork">A delegate to log incoming HTTP requests to a network target.</param>
        /// <param name="LogHTTPResponse_toNetwork">A delegate to log HTTP requests/responses to a network target.</param>
        /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP server sent events source.</param>
        /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
        /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
        /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
        /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public WWCP_HTTPAPI_Logger(WWCP_HTTPAPI                 HTTPAPI,
                                   String                       LoggingPath,
                                   String?                      Context                     = DefaultContext,

                                   HTTPRequestLoggerDelegate?   LogHTTPRequest_toConsole    = null,
                                   HTTPResponseLoggerDelegate?  LogHTTPResponse_toConsole   = null,
                                   HTTPRequestLoggerDelegate?   LogHTTPRequest_toDisc       = null,
                                   HTTPResponseLoggerDelegate?  LogHTTPResponse_toDisc      = null,

                                   HTTPRequestLoggerDelegate?   LogHTTPRequest_toNetwork    = null,
                                   HTTPResponseLoggerDelegate?  LogHTTPResponse_toNetwork   = null,
                                   HTTPRequestLoggerDelegate?   LogHTTPRequest_toHTTPSSE    = null,
                                   HTTPResponseLoggerDelegate?  LogHTTPResponse_toHTTPSSE   = null,

                                   HTTPResponseLoggerDelegate?  LogHTTPError_toConsole      = null,
                                   HTTPResponseLoggerDelegate?  LogHTTPError_toDisc         = null,
                                   HTTPResponseLoggerDelegate?  LogHTTPError_toNetwork      = null,
                                   HTTPResponseLoggerDelegate?  LogHTTPError_toHTTPSSE      = null,

                                   WWCPLogfileCreatorDelegate?  LogfileCreator              = null)

            : base(HTTPAPI.HTTPBaseAPI.HTTPServer,
                   LoggingPath,
                   Context ?? DefaultContext,

                   LogHTTPRequest_toConsole,
                   LogHTTPResponse_toConsole,
                   LogHTTPRequest_toDisc,
                   LogHTTPResponse_toDisc,

                   LogHTTPRequest_toNetwork,
                   LogHTTPResponse_toNetwork,
                   LogHTTPRequest_toHTTPSSE,
                   LogHTTPResponse_toHTTPSSE,

                   LogHTTPError_toConsole,
                   LogHTTPError_toDisc,
                   LogHTTPError_toNetwork,
                   LogHTTPError_toHTTPSSE,

                   (loggingPath, context, logfileName) => LogfileCreator?.Invoke(loggingPath, context, logfileName) ?? "")

        {

            this.HTTPAPI = HTTPAPI ?? throw new ArgumentNullException(nameof(HTTPAPI), "The given WWCP HTTP API must not be null!");

            #region Root

            RegisterEvent2("GetRootRequest",
                          handler => HTTPAPI.OnGetRootRequest += handler,
                          handler => HTTPAPI.OnGetRootRequest -= handler,
                          "GetRoot", "Request",  "all").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("GetRootResponse",
                          handler => HTTPAPI.OnGetRootResponse += handler,
                          handler => HTTPAPI.OnGetRootResponse -= handler,
                          "GetRoot", "Response", "all").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion


        }

        #endregion

    }

}
