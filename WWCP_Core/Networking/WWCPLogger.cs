/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.Net
{

    /// <summary>
    /// A WWCP HTTP API logger.
    /// </summary>
    public class WWCPLogger : HTTPServerLogger
    {

        #region Data

        /// <summary>
        /// The default context of this logger.
        /// </summary>
        public const String DefaultContext = "WWCP";

        #endregion

        #region Properties

        /// <summary>
        /// The linked WWCP API.
        /// </summary>
        public WWCP_HTTPAPI  WWCPAPI  { get; }

        #endregion

        #region Constructor(s)

        #region WWCPLogger(WWCPAPI, Context = DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new WWCP HTTP API logger using the default logging delegates.
        /// </summary>
        /// <param name="WWCPAPI">A WWCP API.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public WWCPLogger(WWCP_HTTPAPI            WWCPAPI,
                          String                  Context         = DefaultContext,
                          LogfileCreatorDelegate  LogfileCreator  = null)

            : this(WWCPAPI,
                   Context,
                   null,
                   null,
                   null,
                   null,
                   LogfileCreator: LogfileCreator)

        { }

        #endregion

        #region WWCPLogger(WWCPAPI, Context, ... Logging delegates ...)

        /// <summary>
        /// Create a new WWCP HTTP API logger using the given logging delegates.
        /// </summary>
        /// <param name="WWCPAPI">A WWCP API.</param>
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
        public WWCPLogger(WWCP_HTTPAPI                WWCPAPI,
                          String                      Context,

                          HTTPRequestLoggerDelegate   LogHTTPRequest_toConsole,
                          HTTPResponseLoggerDelegate  LogHTTPResponse_toConsole,
                          HTTPRequestLoggerDelegate   LogHTTPRequest_toDisc,
                          HTTPResponseLoggerDelegate  LogHTTPResponse_toDisc,

                          HTTPRequestLoggerDelegate   LogHTTPRequest_toNetwork    = null,
                          HTTPResponseLoggerDelegate  LogHTTPResponse_toNetwork   = null,
                          HTTPRequestLoggerDelegate   LogHTTPRequest_toHTTPSSE    = null,
                          HTTPResponseLoggerDelegate  LogHTTPResponse_toHTTPSSE   = null,

                          HTTPResponseLoggerDelegate  LogHTTPError_toConsole      = null,
                          HTTPResponseLoggerDelegate  LogHTTPError_toDisc         = null,
                          HTTPResponseLoggerDelegate  LogHTTPError_toNetwork      = null,
                          HTTPResponseLoggerDelegate  LogHTTPError_toHTTPSSE      = null,

                          LogfileCreatorDelegate      LogfileCreator              = null)

            : base(WWCPAPI.HTTPServer,//.InternalHTTPServer,
                   Context,

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

                   LogfileCreator)

        {

            #region Initial checks

            if (WWCPAPI == null)
                throw new ArgumentNullException(nameof(WWCPAPI), "The given WWCP HTTP API must not be null!");

            #endregion

            this.WWCPAPI = WWCPAPI;

            #region Register auth start/stop log events

            RegisterEvent("AuthEVSEStart",
                          handler => WWCPAPI.OnAuthStartEVSERequest += handler,
                          handler => WWCPAPI.OnAuthStartEVSERequest -= handler,
                          "Auth", "AuthEVSE", "AuthStart", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AuthEVSEStarted",
                          handler => WWCPAPI.OnAuthStartEVSEResponse += handler,
                          handler => WWCPAPI.OnAuthStartEVSEResponse -= handler,
                          "Auth", "AuthEVSE", "AuthStarted", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AuthEVSEStop",
                          handler => WWCPAPI.OnAuthStopEVSERequest += handler,
                          handler => WWCPAPI.OnAuthStopEVSERequest -= handler,
                          "Auth", "AuthEVSE", "AuthStop", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AuthEVSEStopped",
                          handler => WWCPAPI.OnAuthStopEVSEResponse += handler,
                          handler => WWCPAPI.OnAuthStopEVSEResponse -= handler,
                          "Auth", "AuthEVSE", "AuthStopped", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region Register remote start/stop log events

            RegisterEvent("RemoteEVSEStart",
                          handler => WWCPAPI.OnRemoteStartEVSE += handler,
                          handler => WWCPAPI.OnRemoteStartEVSE -= handler,
                          "Remote", "RemoteEVSE", "RemoteStart", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteEVSEStarted",
                          handler => WWCPAPI.OnEVSERemoteStarted += handler,
                          handler => WWCPAPI.OnEVSERemoteStarted -= handler,
                          "Remote", "RemoteEVSE", "RemoteStarted", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteEVSEStop",
                          handler => WWCPAPI.OnRemoteStopEVSE += handler,
                          handler => WWCPAPI.OnRemoteStopEVSE -= handler,
                          "Remote", "RemoteEVSE", "RemoteStop", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteEVSEStopped",
                          handler => WWCPAPI.OnEVSERemoteStopped += handler,
                          handler => WWCPAPI.OnEVSERemoteStopped -= handler,
                          "Remote", "RemoteEVSE", "RemoteStopped", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region Register CDR log events

            RegisterEvent("SendCDR",
                          handler => WWCPAPI.OnSendCDRsRequest += handler,
                          handler => WWCPAPI.OnSendCDRsRequest -= handler,
                          "CDR", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("CDRSent",
                          handler => WWCPAPI.OnSendCDRsResponse += handler,
                          handler => WWCPAPI.OnSendCDRsResponse -= handler,
                          "CDR", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

        }

        #endregion

        #endregion

    }

}
