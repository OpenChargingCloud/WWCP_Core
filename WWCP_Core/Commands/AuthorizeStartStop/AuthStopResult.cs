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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The result of an authorize stop request.
    /// </summary>
    public class AuthStopResult
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext = "https://open.charging.cloud/contexts/wwcp+json/authStopResult";

        #endregion

        #region Properties

        /// <summary>
        /// The identification of the authorizing entity.
        /// </summary>
        public IId                          AuthorizatorId                   { get; }

        /// <summary>
        /// The entity asking for an authorization.
        /// </summary>
        public ISendAuthorizeStartStop?     ISendAuthorizeStartStop          { get; }

        /// <summary>
        /// The entity giving an authorization.
        /// </summary>
        public IReceiveAuthorizeStartStop?  IReceiveAuthorizeStartStop       { get; }

        /// <summary>
        /// The result of the authorize stop request.
        /// </summary>
        public AuthStopResultTypes          Result                           { get; }

        /// <summary>
        /// An optional timestamp until the result may be cached.
        /// </summary>
        public DateTime?                    CachedResultEndOfLifeTime        { get; set; }

        /// <summary>
        /// The optional remaining life time of a cached result.
        /// </summary>
        public TimeSpan?                    CachedResultRemainingLifeTime

            => CachedResultEndOfLifeTime.HasValue
                   ? CachedResultEndOfLifeTime.Value - Timestamp.Now
                   : null;

        /// <summary>
        /// The optional charging session identification.
        /// </summary>
        public ChargingSession_Id?          SessionId                        { get; }

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public EMobilityProvider_Id?        ProviderId                       { get; }

        /// <summary>
        /// A optional description of the authorize stop result.
        /// </summary>
        public I18NString                   Description                      { get; }

        /// <summary>
        /// An optional additional message.
        /// </summary>
        public I18NString?                  AdditionalInfo                   { get; }

        /// <summary>
        /// Number of transmission retries.
        /// </summary>
        public Byte                         NumberOfRetries                  { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                    Runtime                          { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthStopResult(AuthorizatorId,                             Result, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private AuthStopResult(IId                          AuthorizatorId,
                               AuthStopResultTypes          Result,
                               DateTime?                    CachedResultEndOfLifeTime    = null,
                               ISendAuthorizeStartStop?     ISendAuthorizeStartStop      = null,
                               IReceiveAuthorizeStartStop?  IReceiveAuthorizeStartStop   = null,

                               ChargingSession_Id?          SessionId                    = null,

                               EMobilityProvider_Id?        ProviderId                   = null,
                               I18NString?                  Description                  = null,
                               I18NString?                  AdditionalInfo               = null,
                               Byte                         NumberOfRetries              = 0,
                               TimeSpan?                    Runtime                      = null)
        {

            this.AuthorizatorId                 = AuthorizatorId ?? throw new ArgumentNullException(nameof(AuthorizatorId), "The given identification of the authorizator must not be null!");
            this.ISendAuthorizeStartStop        = ISendAuthorizeStartStop;
            this.IReceiveAuthorizeStartStop     = IReceiveAuthorizeStartStop;
            this.Result                         = Result;
            this.CachedResultEndOfLifeTime  = CachedResultEndOfLifeTime;

            this.SessionId                      = SessionId;

            this.ProviderId                     = ProviderId     ?? new EMobilityProvider_Id?();
            this.Description                    = Description    ?? I18NString.Empty;
            this.AdditionalInfo                 = AdditionalInfo;
            this.NumberOfRetries                = NumberOfRetries;
            this.Runtime                        = Runtime;

        }

        #endregion

        #region (public)  AuthStopResult(AuthorizatorId, ISendAuthorizeStartStop,    Result, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public AuthStopResult(IId                      AuthorizatorId,
                              ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                              AuthStopResultTypes      Result,
                              DateTime?                CachedResultEndOfLifeTime   = null,

                              ChargingSession_Id?      SessionId                   = null,

                              EMobilityProvider_Id?    ProviderId                  = null,
                              I18NString?              Description                 = null,
                              I18NString?              AdditionalInfo              = null,
                              Byte                     NumberOfRetries             = 0,
                              TimeSpan?                Runtime                     = null)

            : this(AuthorizatorId,
                   Result,
                   CachedResultEndOfLifeTime,
                   ISendAuthorizeStartStop,
                   null,

                   SessionId,

                   ProviderId,
                   Description,
                   AdditionalInfo,
                   NumberOfRetries,
                   Runtime)

        { }

        #endregion

        #region (public)  AuthStopResult(AuthorizatorId, IReceiveAuthorizeStartStop, Result, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public AuthStopResult(IId                         AuthorizatorId,
                              IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                              AuthStopResultTypes         Result,
                              DateTime?                   CachedResultEndOfLifeTime   = null,

                              ChargingSession_Id?         SessionId                   = null,

                              EMobilityProvider_Id?       ProviderId                  = null,
                              I18NString?                 Description                 = null,
                              I18NString?                 AdditionalInfo              = null,
                              Byte                        NumberOfRetries             = 0,
                              TimeSpan?                   Runtime                     = null)

            : this(AuthorizatorId,
                   Result,
                   CachedResultEndOfLifeTime,
                   null,
                   IReceiveAuthorizeStartStop,

                   SessionId,

                   ProviderId,
                   Description,
                   AdditionalInfo,
                   NumberOfRetries,
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
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Unspecified(IId                      AuthorizatorId,
                        ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                        DateTime?                CachedResultEndOfLifeTime   = null,

                        ChargingSession_Id?      SessionId                   = null,
                        I18NString?              Description                 = null,
                        TimeSpan?                Runtime                     = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.Unspecified,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description,
                        Runtime:      Runtime);



        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Unspecified(IId                         AuthorizatorId,
                        IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                        DateTime?                   CachedResultEndOfLifeTime   = null,

                        ChargingSession_Id?         SessionId                   = null,
                        I18NString?                 Description                 = null,
                        TimeSpan?                   Runtime                     = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.Unspecified,
                        CachedResultEndOfLifeTime,

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
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            AdminDown(IId                      AuthorizatorId,
                      ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                      DateTime?                CachedResultEndOfLifeTime   = null,

                      ChargingSession_Id?      SessionId                   = null,
                      I18NString?              Description                 = null,
                      TimeSpan?                Runtime                     = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.AdminDown,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("The authentication service was disabled by the administrator!"),
                        Runtime:      Runtime);



        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            AdminDown(IId                         AuthorizatorId,
                      IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                      DateTime?                   CachedResultEndOfLifeTime   = null,

                      ChargingSession_Id?         SessionId                   = null,
                      I18NString?                 Description                 = null,
                      TimeSpan?                   Runtime                     = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.AdminDown,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("The authentication service was disabled by the administrator!"),
                        Runtime:      Runtime);

        #endregion

        #region (static) UnknownLocation     (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            UnknownLocation(IId                      AuthorizatorId,
                            ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                            DateTime?                CachedResultEndOfLifeTime   = null,

                            ChargingSession_Id?      SessionId                   = null,
                            I18NString?              Description                 = null,
                            TimeSpan?                Runtime                     = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.UnknownLocation,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Unknown location!"),
                        Runtime:      Runtime);



        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            UnknownLocation(IId                         AuthorizatorId,
                            IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                            DateTime?                   CachedResultEndOfLifeTime   = null,

                            ChargingSession_Id?         SessionId                   = null,
                            I18NString?                 Description                 = null,
                            TimeSpan?                   Runtime                     = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.UnknownLocation,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Unknown location!"),
                        Runtime:      Runtime);

        #endregion

        #region (static) InvalidToken        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            InvalidToken(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         DateTime?                CachedResultEndOfLifeTime   = null,

                         ChargingSession_Id?      SessionId                   = null,
                         I18NString?              Description                 = null,
                         TimeSpan?                Runtime                     = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.InvalidToken,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid token!"),
                        Runtime:      Runtime);



        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            InvalidToken(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         DateTime?                   CachedResultEndOfLifeTime   = null,

                         ChargingSession_Id?         SessionId                   = null,
                         I18NString?                 Description                 = null,
                         TimeSpan?                   Runtime                     = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.InvalidToken,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid token!"),
                        Runtime:      Runtime);

        #endregion

        #region (static) InvalidSessionId    (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            InvalidSessionId(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             DateTime?                CachedResultEndOfLifeTime   = null,

                             ChargingSession_Id?      SessionId                   = null,
                             I18NString?              Description                 = null,
                             TimeSpan?                Runtime                     = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.InvalidSessionId,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid session identification!"),
                        Runtime:      Runtime);



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            InvalidSessionId(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             DateTime?                   CachedResultEndOfLifeTime   = null,

                             ChargingSession_Id?         SessionId                   = null,
                             I18NString?                 Description                 = null,
                             TimeSpan?                   Runtime                     = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.InvalidSessionId,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid session identification!"),
                        Runtime:      Runtime);

        #endregion

        #region (static) AlreadyStopped      (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging session identification was already stopped.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            AlreadyStopped(IId                      AuthorizatorId,
                           ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                           DateTime?                CachedResultEndOfLifeTime   = null,

                           ChargingSession_Id?      SessionId                   = null,
                           I18NString?              Description                 = null,
                           TimeSpan?                Runtime                     = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.AlreadyStopped,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Already stopped!"),
                        Runtime:      Runtime);



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            AlreadyStopped(IId                         AuthorizatorId,
                           IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                           DateTime?                   CachedResultEndOfLifeTime   = null,

                           ChargingSession_Id?         SessionId                   = null,
                           I18NString?                 Description                 = null,
                           TimeSpan?                   Runtime                     = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.AlreadyStopped,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Already stopped!"),
                        Runtime:      Runtime);

        #endregion

        #region (static) OutOfService        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            OutOfService(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         DateTime?                CachedResultEndOfLifeTime   = null,

                         ChargingSession_Id?      SessionId                   = null,
                         I18NString?              Description                 = null,
                         TimeSpan?                Runtime                     = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.OutOfService,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Out of service!"),
                        Runtime:      Runtime);



        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            OutOfService(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         DateTime?                   CachedResultEndOfLifeTime   = null,

                         ChargingSession_Id?         SessionId                   = null,
                         I18NString?                 Description                 = null,
                         TimeSpan?                   Runtime                     = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.OutOfService,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Out of service!"),
                        Runtime:      Runtime);

        #endregion

        #region (static) Authorized          (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Authorized(IId                      AuthorizatorId,
                       ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                       DateTime?                CachedResultEndOfLifeTime   = null,

                       ChargingSession_Id?      SessionId                   = null,
                       EMobilityProvider_Id?    ProviderId                  = null,
                       I18NString?              Description                 = null,
                       I18NString?              AdditionalInfo              = null,
                       Byte                     NumberOfRetries             = 0,
                       TimeSpan?                Runtime                     = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.Authorized,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        ProviderId,
                        Description ?? I18NString.Create("Success!"),
                        AdditionalInfo,
                        NumberOfRetries,
                        Runtime);



        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Authorized(IId                         AuthorizatorId,
                       IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                       DateTime?                   CachedResultEndOfLifeTime   = null,

                       ChargingSession_Id?         SessionId                   = null,
                       EMobilityProvider_Id?       ProviderId                  = null,
                       I18NString?                 Description                 = null,
                       I18NString?                 AdditionalInfo              = null,
                       Byte                        NumberOfRetries             = 0,
                       TimeSpan?                   Runtime                     = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.Authorized,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        ProviderId,
                        Description ?? I18NString.Create("Success!"),
                        AdditionalInfo,
                        NumberOfRetries,
                        Runtime);

        #endregion

        #region (static) NotAuthorized       (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unknown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            NotAuthorized(IId                      AuthorizatorId,
                          ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                          DateTime?                CachedResultEndOfLifeTime   = null,

                          ChargingSession_Id?      SessionId                   = null,
                          EMobilityProvider_Id?    ProviderId                  = null,
                          I18NString?              Description                 = null,
                          I18NString?              AdditionalInfo              = null,
                          Byte                     NumberOfRetries             = 0,
                          TimeSpan?                Runtime                     = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.NotAuthorized,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        ProviderId,
                        Description ?? I18NString.Create("Not Authorized"),
                        AdditionalInfo,
                        NumberOfRetries,
                        Runtime);



        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unknown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            NotAuthorized(IId                         AuthorizatorId,
                          IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                          DateTime?                   CachedResultEndOfLifeTime   = null,

                          ChargingSession_Id?         SessionId                   = null,
                          EMobilityProvider_Id?       ProviderId                  = null,
                          I18NString?                 Description                 = null,
                          I18NString?                 AdditionalInfo              = null,
                          Byte                        NumberOfRetries             = 0,
                          TimeSpan?                   Runtime                     = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.NotAuthorized,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        ProviderId,
                        Description ?? I18NString.Create("Not Authorized"),
                        AdditionalInfo,
                        NumberOfRetries,
                        Runtime);

        #endregion

        #region (static) Blocked             (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Blocked(IId                      AuthorizatorId,
                    ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                    DateTime?                CachedResultEndOfLifeTime   = null,

                    ChargingSession_Id?      SessionId                   = null,
                    EMobilityProvider_Id?    ProviderId                  = null,
                    I18NString?              Description                 = null,
                    I18NString?              AdditionalInfo              = null,
                    Byte                     NumberOfRetries             = 0,
                    TimeSpan?                Runtime                     = null)


            => new (AuthorizatorId,
                    ISendAuthorizeStartStop,
                    AuthStopResultTypes.Blocked,
                    CachedResultEndOfLifeTime,

                    SessionId,
                    ProviderId,
                    Description ?? I18NString.Create("Blocked!"),
                    AdditionalInfo,
                    NumberOfRetries,
                    Runtime);



        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Blocked(IId                         AuthorizatorId,
                    IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                    DateTime?                   CachedResultEndOfLifeTime   = null,

                    ChargingSession_Id?         SessionId                   = null,
                    EMobilityProvider_Id?       ProviderId                  = null,
                    I18NString?                 Description                 = null,
                    I18NString?                 AdditionalInfo              = null,
                    Byte                        NumberOfRetries             = 0,
                    TimeSpan?                   Runtime                     = null)


            => new (AuthorizatorId,
                    IReceiveAuthorizeStartStop,
                    AuthStopResultTypes.Blocked,
                    CachedResultEndOfLifeTime,

                    SessionId,
                    ProviderId,
                    Description ?? I18NString.Create("Blocked!"),
                    AdditionalInfo,
                    NumberOfRetries,
                    Runtime);

        #endregion

        #region (static) CommunicationTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            CommunicationTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,

                                 ChargingSession_Id?      SessionId     = null,
                                 I18NString?              Description   = null,
                                 TimeSpan?                Runtime       = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.CommunicationTimeout,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Communication timeout!"),
                        Runtime:      Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            CommunicationTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,

                                 ChargingSession_Id?         SessionId     = null,
                                 I18NString?                 Description   = null,
                                 TimeSpan?                   Runtime       = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.CommunicationTimeout,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Communication timeout!"),
                        Runtime:      Runtime);

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            StopChargingTimeout(IId                      AuthorizatorId,
                                ISendAuthorizeStartStop  ISendAuthorizeStartStop,

                                ChargingSession_Id?      SessionId     = null,
                                I18NString?              Description   = null,
                                TimeSpan?                Runtime       = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.StopChargingTimeout,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Stop charging timeout!"),
                        Runtime:      Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            StopChargingTimeout(IId                         AuthorizatorId,
                                IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,

                                ChargingSession_Id?         SessionId     = null,
                                I18NString?                 Description   = null,
                                TimeSpan?                   Runtime       = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.StopChargingTimeout,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Stop charging timeout!"),
                        Runtime:      Runtime);

        #endregion

        #region (static) RateLimitReached    (AuthorizatorId, SessionId = null, Description = null, Runtime = null)

        /// <summary>
        /// The authorize stop operation reached a rate limit.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            RateLimitReached(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,

                             ChargingSession_Id?      SessionId     = null,
                             I18NString?              Description   = null,
                             TimeSpan?                Runtime       = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.RateLimitReached,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Rate limit reached!"),
                        Runtime:      Runtime);



        /// <summary>
        /// The authorize start operation reached a rate limit.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            RateLimitReached(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,

                             ChargingSession_Id?         SessionId     = null,
                             I18NString?                 Description   = null,
                             TimeSpan?                   Runtime       = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.RateLimitReached,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("RateLimitReached!"),
                        Runtime:      Runtime);

        #endregion

        #region (static) Error               (AuthorizatorId, SessionId = null, Description = null, Runtime = null)

        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Error(IId                      AuthorizatorId,
                  ISendAuthorizeStartStop  ISendAuthorizeStartStop,

                  ChargingSession_Id?      SessionId     = null,
                  I18NString?              Description   = null,
                  TimeSpan?                Runtime       = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        AuthStopResultTypes.Error,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Error!"),
                        Runtime:      Runtime);



        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopResult

            Error(IId                         AuthorizatorId,
                  IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,

                  ChargingSession_Id?         SessionId     = null,
                  I18NString?                 Description   = null,
                  TimeSpan?                   Runtime       = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        AuthStopResultTypes.Error,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Error!"),
                        Runtime:      Runtime);

        #endregion


        #region ToJSON(Embedded = false, CustomAuthStopResultSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        /// <param name="CustomAuthStopResultSerializer">A delegate to serialize custom AuthStopResult JSON objects.</param>
        public JObject ToJSON(Boolean                                           Embedded                         = false,
                              CustomJObjectSerializerDelegate<AuthStopResult>?  CustomAuthStopResultSerializer   = null)
        {

            var json = JSONObject.Create(

                           !Embedded
                               ? new JProperty("@context",                    JSONLDContext)
                               : null,

                                 new JProperty("result",                      Result.        ToString()),

                           CachedResultEndOfLifeTime.HasValue
                               ? new JProperty("cachedResultEndOfLifeTime",   CachedResultEndOfLifeTime.Value.ToISO8601())
                               : null,

                           SessionId.HasValue
                               ? new JProperty("sessionId",                   SessionId.     ToString())
                               : null,

                           ProviderId.HasValue
                               ? new JProperty("providerId",                  ProviderId.    ToString())
                               : null,

                                 new JProperty("authorizatorId",              AuthorizatorId.ToString()),

                           Description.IsNotNullOrEmpty()
                               ? new JProperty("description",                 Description.   ToJSON())
                               : null,

                           AdditionalInfo is not null && AdditionalInfo.IsNotNullOrEmpty()
                               ? new JProperty("additionalInfo",              AdditionalInfo.ToJSON())
                               : null,

                           Runtime.HasValue
                               ? new JProperty("runtime",                     Runtime.                      Value.TotalMilliseconds)
                               : null

                       );

            return CustomAuthStopResultSerializer is not null
                       ? CustomAuthStopResultSerializer(this, json)
                       : json;

        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Result}{(ProviderId is not null ? ", " + ProviderId : "")}";

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
        /// The authorize stop was not successful (e.g. ev customer is unknown).
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
        /// Rate Limit Reached
        /// </summary>
        RateLimitReached,

        /// <summary>
        /// The remote stop operation led to an error.
        /// </summary>
        Error

    }

}
