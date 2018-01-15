/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of a authorize stop operation.
    /// </summary>
    public class AuthStopResult : AAuthStopResult<AuthStopResultType>
    {

        #region Constructor(s)

        #region (private) AuthStopResult(AuthorizatorId, ISendAuthorizeStartStop,    Result, ...)

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
        private AuthStopResult(IId                      AuthorizatorId,
                               ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                               AuthStopResultType       Result,
                               ChargingSession_Id?      SessionId        = null,

                               eMobilityProvider_Id?    ProviderId       = null,
                               String                   Description      = null,
                               String                   AdditionalInfo   = null,
                               TimeSpan?                Runtime          = null)

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

        #region (private) AuthStopResult(AuthorizatorId, IReceiveAuthorizeStartStop, Result, ...)

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
        private AuthStopResult(IId                         AuthorizatorId,
                               IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                               AuthStopResultType          Result,
                               ChargingSession_Id?         SessionId        = null,

                               eMobilityProvider_Id?       ProviderId       = null,
                               String                      Description      = null,
                               String                      AdditionalInfo   = null,
                               TimeSpan?                   Runtime          = null)

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
        public static AuthStopResult

            Unspecified(IId                      AuthorizatorId,
                        ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                        ChargingSession_Id?      SessionId  = null,
                        TimeSpan?                Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultType.Unspecified,
                                      SessionId,
                                      Runtime: Runtime);



        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Unspecified(IId                         AuthorizatorId,
                        IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                        ChargingSession_Id?         SessionId  = null,
                        TimeSpan?                   Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultType.Unspecified,
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
        public static AuthStopResult

            AdminDown(IId                      AuthorizatorId,
                      ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                      ChargingSession_Id?      SessionId  = null,
                      TimeSpan?                Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultType.AdminDown,
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
        public static AuthStopResult

            AdminDown(IId                         AuthorizatorId,
                      IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                      ChargingSession_Id?         SessionId  = null,
                      TimeSpan?                   Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultType.AdminDown,
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
        public static AuthStopResult

            InvalidSessionId(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             ChargingSession_Id?      SessionId  = null,
                             TimeSpan?                Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultType.InvalidSessionId,
                                      SessionId,
                                      Runtime: Runtime);



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            InvalidSessionId(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             ChargingSession_Id?         SessionId  = null,
                             TimeSpan?                   Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultType.InvalidSessionId,
                                      SessionId,
                                      Runtime: Runtime);

        #endregion

        #region (static) OutOfService        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            OutOfService(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId  = null,
                         TimeSpan?                Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultType.OutOfService,
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
        public static AuthStopResult

            OutOfService(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId  = null,
                         TimeSpan?                   Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultType.OutOfService,
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
        public static AuthStopResult

            Authorized(IId                      AuthorizatorId,
                       ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                       ChargingSession_Id?      SessionId        = null,
                       eMobilityProvider_Id?    ProviderId       = null,
                       String                   Description      = "Success",
                       String                   AdditionalInfo   = null,
                       TimeSpan?                Runtime          = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultType.Authorized,
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
        public static AuthStopResult

            Authorized(IId                         AuthorizatorId,
                       IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                       ChargingSession_Id?         SessionId        = null,
                       eMobilityProvider_Id?       ProviderId       = null,
                       String                      Description      = "Success",
                       String                      AdditionalInfo   = null,
                       TimeSpan?                   Runtime          = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultType.Authorized,
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
        public static AuthStopResult

            NotAuthorized(IId                      AuthorizatorId,
                          ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                          ChargingSession_Id?      SessionId        = null,
                          eMobilityProvider_Id?    ProviderId       = null,
                          String                   Description      = "NotAuthorized",
                          String                   AdditionalInfo   = null,
                          TimeSpan?                Runtime          = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultType.NotAuthorized,
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
        public static AuthStopResult

            NotAuthorized(IId                         AuthorizatorId,
                          IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                          ChargingSession_Id?         SessionId        = null,
                          eMobilityProvider_Id?       ProviderId       = null,
                          String                      Description      = "NotAuthorized",
                          String                      AdditionalInfo   = null,
                          TimeSpan?                   Runtime          = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultType.NotAuthorized,
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
        public static AuthStopResult

            Blocked(IId                      AuthorizatorId,
                    ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                    ChargingSession_Id?      SessionId        = null,
                    eMobilityProvider_Id?    ProviderId       = null,
                    String                   Description      = null,
                    String                   AdditionalInfo   = null,
                    TimeSpan?                Runtime          = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultType.Blocked,
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
        public static AuthStopResult

            Blocked(IId                         AuthorizatorId,
                    IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                    ChargingSession_Id?         SessionId        = null,
                    eMobilityProvider_Id?       ProviderId       = null,
                    String                      Description      = null,
                    String                      AdditionalInfo   = null,
                    TimeSpan?                   Runtime          = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultType.Blocked,
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
        public static AuthStopResult

            CommunicationTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId  = null,
                                 TimeSpan?                Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultType.CommunicationTimeout,
                                      SessionId,
                                      Runtime: Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            CommunicationTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId  = null,
                                 TimeSpan?                   Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultType.CommunicationTimeout,
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
        public static AuthStopResult

            StartChargingTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId  = null,
                                 TimeSpan?                Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultType.StopChargingTimeout,
                                      SessionId,
                                      Runtime: Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            StartChargingTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId  = null,
                                 TimeSpan?                   Runtime    = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultType.StopChargingTimeout,
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
        public static AuthStopResult

            Error(IId                      AuthorizatorId,
                  ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                  ChargingSession_Id?      SessionId     = null,
                  String                   ErrorMessage  = null,
                  TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultType.Error,
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
        public static AuthStopResult

            Error(IId                         AuthorizatorId,
                  IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                  ChargingSession_Id?         SessionId     = null,
                  String                      ErrorMessage  = null,
                  TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultType.Error,
                                      SessionId,
                                      Description: ErrorMessage,
                                      Runtime:     Runtime);

        #endregion


    }

    /// <summary>
    /// The result of a authorize stop operation.
    /// </summary>
    public enum AuthStopResultType
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
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The EVSE or charging station is out of service.
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
