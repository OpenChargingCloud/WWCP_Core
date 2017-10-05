/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The result of a authorize stop operation at a charging pool.
    /// </summary>
    public class AuthStopChargingPoolResult : AAuthStopResult<AuthStopChargingPoolResultType>
    {

        #region Constructor(s)

        #region (private) AuthStopChargingPoolResult(AuthorizatorId, ISendAuthorizeStartStop,    Result, ...)

        /// <summary>
        /// Create a new authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="Result">The authorize stop result type.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private AuthStopChargingPoolResult(IId                             AuthorizatorId,
                                           ISendAuthorizeStartStop         ISendAuthorizeStartStop,
                                           AuthStopChargingPoolResultType  Result,
                                           ChargingSession_Id?             SessionId        = null,

                                           eMobilityProvider_Id?           ProviderId       = null,
                                           String                          Description      = null,
                                           String                          AdditionalInfo   = null,
                                           TimeSpan?                       Runtime          = null)

            : base(AuthorizatorId,
                   ISendAuthorizeStartStop,
                   Result,
                   SessionId,
                   ProviderId,
                   Description,
                   AdditionalInfo,
                   Runtime)

        { }

        #endregion

        #region (private) AuthStopChargingPoolResult(AuthorizatorId, IReceiveAuthorizeStartStop, Result, ...)

        /// <summary>
        /// Create a new authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="Result">The authorize stop result type.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private AuthStopChargingPoolResult(IId                             AuthorizatorId,
                                           IReceiveAuthorizeStartStop      IReceiveAuthorizeStartStop,
                                           AuthStopChargingPoolResultType  Result,
                                           ChargingSession_Id?             SessionId        = null,

                                           eMobilityProvider_Id?           ProviderId       = null,
                                           String                          Description      = null,
                                           String                          AdditionalInfo   = null,
                                           TimeSpan?                       Runtime          = null)

            : base(AuthorizatorId,
                   IReceiveAuthorizeStartStop,
                   Result,
                   SessionId,
                   ProviderId,
                   Description,
                   AdditionalInfo,
                   Runtime)

        { }

        #endregion

        #endregion


        #region (static) Unspecified         (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            Unspecified(IId                      AuthorizatorId,
                        ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                        ChargingSession_Id?      SessionId  = null,
                        TimeSpan?                Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.Unspecified,
                                                  SessionId,
                                                  Runtime: Runtime);



        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            Unspecified(IId                         AuthorizatorId,
                        IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                        ChargingSession_Id?         SessionId  = null,
                        TimeSpan?                   Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.Unspecified,
                                                  SessionId,
                                                  Runtime: Runtime);

        #endregion

        #region (static) AdminDown           (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            AdminDown(IId                      AuthorizatorId,
                      ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                      ChargingSession_Id?      SessionId  = null,
                      TimeSpan?                Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.AdminDown,
                                                  SessionId,
                                                  Description: "The authentication service was disabled by the administrator!",
                                                  Runtime:     Runtime);



        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            AdminDown(IId                         AuthorizatorId,
                      IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                      ChargingSession_Id?         SessionId  = null,
                      TimeSpan?                   Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.AdminDown,
                                                  SessionId,
                                                  Description: "The authentication service was disabled by the administrator!",
                                                  Runtime:     Runtime);

        #endregion

        #region (static) InvalidSessionId    (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            InvalidSessionId(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             ChargingSession_Id?      SessionId  = null,
                             TimeSpan?                Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.InvalidSessionId,
                                                  SessionId,
                                                  Runtime: Runtime);



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            InvalidSessionId(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             ChargingSession_Id?         SessionId  = null,
                             TimeSpan?                   Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.InvalidSessionId,
                                                  SessionId,
                                                  Runtime: Runtime);

        #endregion

        #region (static) NotSupported        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The charging pool does not support this operation.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            NotSupported(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId  = null,
                         TimeSpan?                Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.NotSupported,
                                                  SessionId,
                                                  Description: "Operation not supported!",
                                                  Runtime:     Runtime);



        /// <summary>
        /// The charging pool does not support this operation.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            NotSupported(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId  = null,
                         TimeSpan?                   Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.NotSupported,
                                                  SessionId,
                                                  Description: "Operation not supported!",
                                                  Runtime:     Runtime);

        #endregion

        #region (static) OutOfService        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            OutOfService(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId  = null,
                         TimeSpan?                Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.OutOfService,
                                                  SessionId,
                                                  Description: "Out-of-service!",
                                                  Runtime:     Runtime);



        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            OutOfService(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId  = null,
                         TimeSpan?                   Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.OutOfService,
                                                  SessionId,
                                                  Description: "Out-of-service!",
                                                  Runtime:     Runtime);

        #endregion

        #region (static) Authorized          (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            Authorized(IId                      AuthorizatorId,
                       ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                       ChargingSession_Id?      SessionId        = null,
                       eMobilityProvider_Id?    ProviderId       = null,
                       String                   Description      = "Success",
                       String                   AdditionalInfo   = null,
                       TimeSpan?                Runtime          = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.Authorized,
                                                  SessionId,
                                                  ProviderId,
                                                  Description,
                                                  AdditionalInfo,
                                                  Runtime: Runtime);



        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            Authorized(IId                         AuthorizatorId,
                       IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                       ChargingSession_Id?         SessionId        = null,
                       eMobilityProvider_Id?       ProviderId       = null,
                       String                      Description      = "Success",
                       String                      AdditionalInfo   = null,
                       TimeSpan?                   Runtime          = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.Authorized,
                                                  SessionId,
                                                  ProviderId,
                                                  Description,
                                                  AdditionalInfo,
                                                  Runtime: Runtime);

        #endregion

        #region (static) NotAuthorized       (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unkown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            NotAuthorized(IId                      AuthorizatorId,
                          ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                          ChargingSession_Id?      SessionId        = null,
                          eMobilityProvider_Id?    ProviderId       = null,
                          String                   Description      = "NotAuthorized",
                          String                   AdditionalInfo   = null,
                          TimeSpan?                Runtime          = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.NotAuthorized,
                                                  SessionId,
                                                  ProviderId,
                                                  Description,
                                                  AdditionalInfo,
                                                  Runtime: Runtime);



        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unkown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            NotAuthorized(IId                         AuthorizatorId,
                          IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                          ChargingSession_Id?         SessionId        = null,
                          eMobilityProvider_Id?       ProviderId       = null,
                          String                      Description      = "NotAuthorized",
                          String                      AdditionalInfo   = null,
                          TimeSpan?                   Runtime          = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.NotAuthorized,
                                                  SessionId,
                                                  ProviderId,
                                                  Description,
                                                  AdditionalInfo,
                                                  Runtime: Runtime);

        #endregion

        #region (static) Blocked             (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            Blocked(IId                      AuthorizatorId,
                    ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                    ChargingSession_Id?      SessionId        = null,
                    eMobilityProvider_Id?    ProviderId       = null,
                    String                   Description      = null,
                    String                   AdditionalInfo   = null,
                    TimeSpan?                Runtime          = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.Blocked,
                                                  SessionId,
                                                  ProviderId,
                                                  Description,
                                                  AdditionalInfo,
                                                  Runtime: Runtime);



        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            Blocked(IId                         AuthorizatorId,
                    IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                    ChargingSession_Id?         SessionId        = null,
                    eMobilityProvider_Id?       ProviderId       = null,
                    String                      Description      = null,
                    String                      AdditionalInfo   = null,
                    TimeSpan?                   Runtime          = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.Blocked,
                                                  SessionId,
                                                  ProviderId,
                                                  Description,
                                                  AdditionalInfo,
                                                  Runtime: Runtime);

        #endregion

        #region (static) CommunicationTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            CommunicationTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId  = null,
                                 TimeSpan?                Runtime    = null)

                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.CommunicationTimeout,
                                                  SessionId,
                                                  Runtime: Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            CommunicationTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId  = null,
                                 TimeSpan?                   Runtime    = null)

                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.CommunicationTimeout,
                                                  SessionId,
                                                  Runtime: Runtime);

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            StartChargingTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId  = null,
                                 TimeSpan?                Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.StopChargingTimeout,
                                                  SessionId,
                                                  Runtime: Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            StartChargingTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId  = null,
                                 TimeSpan?                   Runtime    = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.StopChargingTimeout,
                                                  SessionId,
                                                  Runtime: Runtime);

        #endregion

        #region (static) Error               (AuthorizatorId, SessionId = null, ErrorMessage = null, Runtime = null)

        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            Error(IId                      AuthorizatorId,
                  ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                  ChargingSession_Id?      SessionId     = null,
                  String                   ErrorMessage  = null,
                  TimeSpan?                Runtime       = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  ISendAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.Error,
                                                  SessionId,
                                                  Description: ErrorMessage,
                                                  Runtime:     Runtime);



        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult

            Error(IId                         AuthorizatorId,
                  IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                  ChargingSession_Id?         SessionId     = null,
                  String                      ErrorMessage  = null,
                  TimeSpan?                   Runtime       = null)


                => new AuthStopChargingPoolResult(AuthorizatorId,
                                                  IReceiveAuthorizeStartStop,
                                                  AuthStopChargingPoolResultType.Error,
                                                  SessionId,
                                                  Description: ErrorMessage,
                                                  Runtime:     Runtime);

        #endregion


    }

    /// <summary>
    /// The result of a authorize stop operation at a charging pool.
    /// </summary>
    public enum AuthStopChargingPoolResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        AdminDown,

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        UnknownChargingStation,

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The charging pool does not support this operation.
        /// </summary>
        NotSupported,

        /// <summary>
        /// The charging station is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        Authorized,

        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unkown).
        /// </summary>
        NotAuthorized,

        /// <summary>
        /// The authorize stop operation is not allowed (ev customer is blocked).
        /// </summary>
        Blocked,

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        CommunicationTimeout,

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        StopChargingTimeout,

        /// <summary>
        /// The remote stop operation led to an error.
        /// </summary>
        Error

    }

}
