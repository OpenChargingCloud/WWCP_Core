/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The result of an authorize stop request.
    /// </summary>
    public class AuthStopResult
    {

        #region Properties

        /// <summary>
        /// The identification of the authorizing entity.
        /// </summary>
        public IId                          AuthorizatorId                { get; }

        /// <summary>
        /// The entity asking for an authorization.
        /// </summary>
        public ISendAuthorizeStartStop      ISendAuthorizeStartStop       { get; }

        /// <summary>
        /// The entity giving an authorization.
        /// </summary>
        public IReceiveAuthorizeStartStop   IReceiveAuthorizeStartStop    { get; }

        /// <summary>
        /// The result of the authorize stop request.
        /// </summary>
        public AuthStopResultTypes          Result                        { get; }

        /// <summary>
        /// The optional charging session identification.
        /// </summary>
        public ChargingSession_Id?          SessionId                     { get; }

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public eMobilityProvider_Id?        ProviderId                    { get; }

        /// <summary>
        /// A optional description of the authorize stop result.
        /// </summary>
        public I18NString                   Description                   { get; }

        /// <summary>
        /// An optional additional message.
        /// </summary>
        public I18NString                   AdditionalInfo                { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                    Runtime                       { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthStopResult(AuthorizatorId,                             Result, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private AuthStopResult(IId                          AuthorizatorId,
                               AuthStopResultTypes          Result,
                               ISendAuthorizeStartStop      ISendAuthorizeStartStop      = null,
                               IReceiveAuthorizeStartStop   IReceiveAuthorizeStartStop   = null,

                               ChargingSession_Id?          SessionId                    = null,
                               eMobilityProvider_Id?        ProviderId                   = null,
                               I18NString                   Description                  = null,
                               I18NString                   AdditionalInfo               = null,
                               TimeSpan?                    Runtime                      = null)
        {

            this.AuthorizatorId              = AuthorizatorId ?? throw new ArgumentNullException(nameof(AuthorizatorId), "The given identification of the authorizator must not be null!");
            this.ISendAuthorizeStartStop     = ISendAuthorizeStartStop;
            this.IReceiveAuthorizeStartStop  = IReceiveAuthorizeStartStop;
            this.Result                      = Result;
            this.SessionId                   = SessionId;
            this.ProviderId                  = ProviderId     ?? new eMobilityProvider_Id?();
            this.Description                 = Description    ?? I18NString.Empty;
            this.AdditionalInfo              = AdditionalInfo;
            this.Runtime                     = Runtime;

        }

        #endregion

        #region (private) AuthStopResult(AuthorizatorId, ISendAuthorizeStartStop,    Result, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        protected AuthStopResult(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 AuthStopResultTypes      Result,
                                 ChargingSession_Id?      SessionId        = null,
                                 eMobilityProvider_Id?    ProviderId       = null,
                                 I18NString               Description      = null,
                                 I18NString               AdditionalInfo   = null,
                                 TimeSpan?                Runtime          = null)

            : this(AuthorizatorId,
                   Result,
                   ISendAuthorizeStartStop,
                   null,
                   SessionId,
                   ProviderId,
                   Description,
                   AdditionalInfo,
                   Runtime)

        { }

        #endregion

        #region (private) AuthStopResult(AuthorizatorId, IReceiveAuthorizeStartStop, Result, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        protected AuthStopResult(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 AuthStopResultTypes         Result,
                                 ChargingSession_Id?         SessionId        = null,
                                 eMobilityProvider_Id?       ProviderId       = null,
                                 I18NString                  Description      = null,
                                 I18NString                  AdditionalInfo   = null,
                                 TimeSpan?                   Runtime          = null)

            : this(AuthorizatorId,
                   Result,
                   null,
                   IReceiveAuthorizeStartStop,
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
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Unspecified(IId                      AuthorizatorId,
                        ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                        ChargingSession_Id?      SessionId     = null,
                        I18NString               Description   = null,
                        TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.Unspecified,
                                      SessionId,
                                      Description:  Description,
                                      Runtime:      Runtime);



        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Unspecified(IId                         AuthorizatorId,
                        IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                        ChargingSession_Id?         SessionId     = null,
                        I18NString                  Description   = null,
                        TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.Unspecified,
                                      SessionId,
                                      Description:  Description,
                                      Runtime:      Runtime);

        #endregion

        #region (static) AdminDown           (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            AdminDown(IId                      AuthorizatorId,
                      ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                      ChargingSession_Id?      SessionId     = null,
                      I18NString               Description   = null,
                      TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.AdminDown,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "The authentication service was disabled by the administrator!"),
                                      Runtime:      Runtime);



        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            AdminDown(IId                         AuthorizatorId,
                      IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                      ChargingSession_Id?         SessionId     = null,
                      I18NString                  Description   = null,
                      TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.AdminDown,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "The authentication service was disabled by the administrator!"),
                                      Runtime:      Runtime);

        #endregion

        #region (static) UnknownLocation     (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            UnknownLocation(IId                      AuthorizatorId,
                            ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                            ChargingSession_Id?      SessionId     = null,
                            I18NString               Description   = null,
                            TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.UnknownLocation,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Unknown location!"),
                                      Runtime:      Runtime);



        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            UnknownLocation(IId                         AuthorizatorId,
                            IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                            ChargingSession_Id?         SessionId     = null,
                            I18NString                  Description   = null,
                            TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.UnknownLocation,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Unknown location!"),
                                      Runtime:      Runtime);

        #endregion

        #region (static) InvalidToken        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            InvalidToken(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId     = null,
                         I18NString               Description   = null,
                         TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.InvalidToken,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Invalid token!"),
                                      Runtime:      Runtime);



        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            InvalidToken(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId     = null,
                         I18NString                  Description   = null,
                         TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.InvalidToken,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Invalid token!"),
                                      Runtime:      Runtime);

        #endregion

        #region (static) InvalidSessionId    (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            InvalidSessionId(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             ChargingSession_Id?      SessionId     = null,
                             I18NString               Description   = null,
                             TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.InvalidSessionId,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Invalid session identification!"),
                                      Runtime:      Runtime);



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            InvalidSessionId(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             ChargingSession_Id?         SessionId     = null,
                             I18NString                  Description   = null,
                             TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.InvalidSessionId,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Invalid session identification!"),
                                      Runtime:      Runtime);

        #endregion

        #region (static) AlreadyStopped      (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging session identification was already stopped.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            AlreadyStopped(IId                      AuthorizatorId,
                           ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                           ChargingSession_Id?      SessionId     = null,
                           I18NString               Description   = null,
                           TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.AlreadyStopped,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Already stopped!"),
                                      Runtime:      Runtime);



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            AlreadyStopped(IId                         AuthorizatorId,
                           IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                           ChargingSession_Id?         SessionId     = null,
                           I18NString                  Description   = null,
                           TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.AlreadyStopped,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Already stopped!"),
                                      Runtime:      Runtime);

        #endregion

        #region (static) OutOfService        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            OutOfService(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId     = null,
                         I18NString               Description   = null,
                         TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.OutOfService,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Out of service!"),
                                      Runtime:      Runtime);



        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            OutOfService(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId     = null,
                         I18NString                  Description   = null,
                         TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.OutOfService,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Out of service!"),
                                      Runtime:      Runtime);

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
                       I18NString               Description      = null,
                       I18NString               AdditionalInfo   = null,
                       TimeSpan?                Runtime          = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.Authorized,
                                      SessionId,
                                      ProviderId,
                                      Description ?? I18NString.Create(Languages.eng, "Success!"),
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
                       I18NString                  Description      = null,
                       I18NString                  AdditionalInfo   = null,
                       TimeSpan?                   Runtime          = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.Authorized,
                                      SessionId,
                                      ProviderId,
                                      Description ?? I18NString.Create(Languages.eng, "Success!"),
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
                          I18NString               Description      = null,
                          I18NString               AdditionalInfo   = null,
                          TimeSpan?                Runtime          = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.NotAuthorized,
                                      SessionId,
                                      ProviderId,
                                      Description ?? I18NString.Create(Languages.eng, "Not Authorized"),
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
                          I18NString                  Description      = null,
                          I18NString                  AdditionalInfo   = null,
                          TimeSpan?                   Runtime          = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.NotAuthorized,
                                      SessionId,
                                      ProviderId,
                                      Description ?? I18NString.Create(Languages.eng, "Not Authorized"),
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
                    I18NString               Description      = null,
                    I18NString               AdditionalInfo   = null,
                    TimeSpan?                Runtime          = null)


            => new AuthStopResult(AuthorizatorId,
                                  ISendAuthorizeStartStop,
                                  AuthStopResultTypes.Blocked,
                                  SessionId,
                                  ProviderId,
                                  Description ?? I18NString.Create(Languages.eng, "Blocked!"),
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
                    I18NString                  Description      = null,
                    I18NString                  AdditionalInfo   = null,
                    TimeSpan?                   Runtime          = null)


            => new AuthStopResult(AuthorizatorId,
                                  IReceiveAuthorizeStartStop,
                                  AuthStopResultTypes.Blocked,
                                  SessionId,
                                  ProviderId,
                                  Description ?? I18NString.Create(Languages.eng, "Blocked!"),
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
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            CommunicationTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId     = null,
                                 I18NString               Description   = null,
                                 TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.CommunicationTimeout,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Communication timeout!"),
                                      Runtime:      Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            CommunicationTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId     = null,
                                 I18NString                  Description   = null,
                                 TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.CommunicationTimeout,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Communication timeout!"),
                                      Runtime:      Runtime);

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            StopChargingTimeout(IId                      AuthorizatorId,
                                ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                ChargingSession_Id?      SessionId     = null,
                                I18NString               Description   = null,
                                TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.StopChargingTimeout,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Stop charging timeout!"),
                                      Runtime:      Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            StopChargingTimeout(IId                         AuthorizatorId,
                                IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                ChargingSession_Id?         SessionId     = null,
                                I18NString                  Description   = null,
                                TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.StopChargingTimeout,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Stop charging timeout!"),
                                      Runtime:      Runtime);

        #endregion

        #region (static) Error               (AuthorizatorId, SessionId = null, Description = null, Runtime = null)

        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Error(IId                      AuthorizatorId,
                  ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                  ChargingSession_Id?      SessionId     = null,
                  I18NString               Description   = null,
                  TimeSpan?                Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      ISendAuthorizeStartStop,
                                      AuthStopResultTypes.Error,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Error!"),
                                      Runtime:      Runtime);



        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Error(IId                         AuthorizatorId,
                  IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                  ChargingSession_Id?         SessionId     = null,
                  I18NString                  Description   = null,
                  TimeSpan?                   Runtime       = null)


                => new AuthStopResult(AuthorizatorId,
                                      IReceiveAuthorizeStartStop,
                                      AuthStopResultTypes.Error,
                                      SessionId,
                                      Description:  Description ?? I18NString.Create(Languages.eng, "Error!"),
                                      Runtime:      Runtime);

        #endregion


        #region ToJSON(ResponseMapper = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="ResponseMapper">An optional response mapper delegate.</param>
        public JObject ToJSON(Func<JObject, JObject> ResponseMapper = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("result",                Result.        ToString()),

                           SessionId.HasValue
                               ? new JProperty("sessionId",       SessionId.     ToString())
                               : null,

                           ProviderId.HasValue
                               ? new JProperty("providerId",      ProviderId.    ToString())
                               : null,

                                 new JProperty("authorizatorId",  AuthorizatorId.ToString()),

                           Description.IsNeitherNullNorEmpty()
                               ? new JProperty("description",     Description.   ToJSON())
                               : null,

                           AdditionalInfo.IsNeitherNullNorEmpty()
                               ? new JProperty("additionalInfo",  AdditionalInfo.ToJSON())
                               : null,

                           Runtime.HasValue
                               ? new JProperty("runtime",         Runtime.Value.TotalMilliseconds)
                               : null

                       );

            return ResponseMapper != null
                       ? ResponseMapper(JSON)
                       : JSON;

        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (ProviderId != null)
                return String.Concat(Result.ToString(), ", ", ProviderId);

            return String.Concat(Result.ToString());

        }

        #endregion

    }

    /// <summary>
    /// The result of an authorize stop request.
    /// </summary>
    public enum AuthStopResultTypes
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
        /// The given charging location is unknown.
        /// </summary>
        UnknownLocation,

        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        InvalidToken,

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The given charging session identification was already stopped.
        /// </summary>
        AlreadyStopped,

        /// <summary>
        /// The charging location is out of service.
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
        /// The authorize stop ran into a timeout between evse operator backend and charging location.
        /// </summary>
        CommunicationTimeout,

        /// <summary>
        /// The authorize stop ran into a timeout between charging location and ev.
        /// </summary>
        StopChargingTimeout,

        /// <summary>
        /// The remote stop operation led to an error.
        /// </summary>
        Error

    }

}
