/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of a authorize stop operation at a charging station.
    /// </summary>
    public class AuthStopChargingStationResult : AAuthStopResult<AuthStopChargingStationResultType>
    {

        #region Constructor(s)

        #region (private) AuthStopChargingStationResult(AuthorizatorId, ISendAuthorizeStartStop,    Result, ...)

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
        private AuthStopChargingStationResult(IId                                AuthorizatorId,
                                              ISendAuthorizeStartStop            ISendAuthorizeStartStop,
                                              AuthStopChargingStationResultType  Result,
                                              ChargingSession_Id?                SessionId        = null,

                                              eMobilityProvider_Id?              ProviderId       = null,
                                              String                             Description      = null,
                                              String                             AdditionalInfo   = null,
                                              TimeSpan?                          Runtime          = null)

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

        #region (private) AuthStopChargingStationResult(AuthorizatorId, IReceiveAuthorizeStartStop, Result, ...)

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
        private AuthStopChargingStationResult(IId                                AuthorizatorId,
                                              IReceiveAuthorizeStartStop         IReceiveAuthorizeStartStop,
                                              AuthStopChargingStationResultType  Result,
                                              ChargingSession_Id?                SessionId        = null,

                                              eMobilityProvider_Id?              ProviderId       = null,
                                              String                             Description      = null,
                                              String                             AdditionalInfo   = null,
                                              TimeSpan?                          Runtime          = null)

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
        public static AuthStopChargingStationResult

            Unspecified(IId                      AuthorizatorId,
                        ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                        ChargingSession_Id?      SessionId  = null,
                        TimeSpan?                Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.Unspecified,
                                                     SessionId,
                                                     Runtime: Runtime);



        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingStationResult

            Unspecified(IId                         AuthorizatorId,
                        IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                        ChargingSession_Id?         SessionId  = null,
                        TimeSpan?                   Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.Unspecified,
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
        public static AuthStopChargingStationResult

            AdminDown(IId                      AuthorizatorId,
                      ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                      ChargingSession_Id?      SessionId  = null,
                      TimeSpan?                Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.AdminDown,
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
        public static AuthStopChargingStationResult

            AdminDown(IId                         AuthorizatorId,
                      IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                      ChargingSession_Id?         SessionId  = null,
                      TimeSpan?                   Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.AdminDown,
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
        public static AuthStopChargingStationResult

            InvalidSessionId(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             ChargingSession_Id?      SessionId  = null,
                             TimeSpan?                Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.InvalidSessionId,
                                                     SessionId,
                                                     Runtime: Runtime);



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingStationResult

            InvalidSessionId(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             ChargingSession_Id?         SessionId  = null,
                             TimeSpan?                   Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.InvalidSessionId,
                                                     SessionId,
                                                     Runtime: Runtime);

        #endregion

        #region (static) NotSupported        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The charging station does not support this operation.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingStationResult

            NotSupported(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId  = null,
                         TimeSpan?                Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.NotSupported,
                                                     SessionId,
                                                     Description: "Operation not supported!",
                                                     Runtime:     Runtime);



        /// <summary>
        /// The charging station does not support this operation.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingStationResult

            NotSupported(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId  = null,
                         TimeSpan?                   Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.NotSupported,
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
        public static AuthStopChargingStationResult

            OutOfService(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId  = null,
                         TimeSpan?                Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.OutOfService,
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
        public static AuthStopChargingStationResult

            OutOfService(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId  = null,
                         TimeSpan?                   Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.OutOfService,
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
        public static AuthStopChargingStationResult

            Authorized(IId                      AuthorizatorId,
                       ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                       ChargingSession_Id?      SessionId        = null,
                       eMobilityProvider_Id?    ProviderId       = null,
                       String                   Description      = "Success",
                       String                   AdditionalInfo   = null,
                       TimeSpan?                Runtime          = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.Authorized,
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
        public static AuthStopChargingStationResult

            Authorized(IId                         AuthorizatorId,
                       IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                       ChargingSession_Id?         SessionId        = null,
                       eMobilityProvider_Id?       ProviderId       = null,
                       String                      Description      = "Success",
                       String                      AdditionalInfo   = null,
                       TimeSpan?                   Runtime          = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.Authorized,
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
        public static AuthStopChargingStationResult

            NotAuthorized(IId                      AuthorizatorId,
                          ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                          ChargingSession_Id?      SessionId        = null,
                          eMobilityProvider_Id?    ProviderId       = null,
                          String                   Description      = "NotAuthorized",
                          String                   AdditionalInfo   = null,
                          TimeSpan?                Runtime          = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.NotAuthorized,
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
        public static AuthStopChargingStationResult

            NotAuthorized(IId                         AuthorizatorId,
                          IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                          ChargingSession_Id?         SessionId        = null,
                          eMobilityProvider_Id?       ProviderId       = null,
                          String                      Description      = "NotAuthorized",
                          String                      AdditionalInfo   = null,
                          TimeSpan?                   Runtime          = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.NotAuthorized,
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
        public static AuthStopChargingStationResult

            Blocked(IId                      AuthorizatorId,
                    ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                    ChargingSession_Id?      SessionId        = null,
                    eMobilityProvider_Id?    ProviderId       = null,
                    String                   Description      = null,
                    String                   AdditionalInfo   = null,
                    TimeSpan?                Runtime          = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.Blocked,
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
        public static AuthStopChargingStationResult

            Blocked(IId                         AuthorizatorId,
                    IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                    ChargingSession_Id?         SessionId        = null,
                    eMobilityProvider_Id?       ProviderId       = null,
                    String                      Description      = null,
                    String                      AdditionalInfo   = null,
                    TimeSpan?                   Runtime          = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.Blocked,
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
        public static AuthStopChargingStationResult

            CommunicationTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId  = null,
                                 TimeSpan?                Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.CommunicationTimeout,
                                                     SessionId,
                                                     Runtime: Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingStationResult

            CommunicationTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId  = null,
                                 TimeSpan?                   Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.CommunicationTimeout,
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
        public static AuthStopChargingStationResult

            StartChargingTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId  = null,
                                 TimeSpan?                Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.StopChargingTimeout,
                                                     SessionId,
                                                     Runtime: Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingStationResult

            StartChargingTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId  = null,
                                 TimeSpan?                   Runtime    = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.StopChargingTimeout,
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
        public static AuthStopChargingStationResult

            Error(IId                      AuthorizatorId,
                  ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                  ChargingSession_Id?      SessionId     = null,
                  String                   ErrorMessage  = null,
                  TimeSpan?                Runtime       = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     ISendAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.Error,
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
        public static AuthStopChargingStationResult

            Error(IId                         AuthorizatorId,
                  IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                  ChargingSession_Id?         SessionId     = null,
                  String                      ErrorMessage  = null,
                  TimeSpan?                   Runtime       = null)


                => new AuthStopChargingStationResult(AuthorizatorId,
                                                     IReceiveAuthorizeStartStop,
                                                     AuthStopChargingStationResultType.Error,
                                                     SessionId,
                                                     Description: ErrorMessage,
                                                     Runtime:     Runtime);

        #endregion


    }

    /// <summary>
    /// The result of a authorize stop operation at a charging station.
    /// </summary>
    public enum AuthStopChargingStationResultType
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
        /// The charging station does not support this operation.
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
