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
        public DateTimeOffset?              CachedResultEndOfLifeTime        { get; set; }

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
        /// Optional additional context information.
        /// </summary>
        public JObject?                     AdditionalContext                { get; }

        /// <summary>
        /// Number of transmission retries.
        /// </summary>
        public Byte                         NumberOfRetries                  { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan                     Runtime                          { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthStopResult (AuthorizatorId,                             Result, Runtime, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="AdditionalContext">Optional additional context information.</param>
        /// 
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        private AuthStopResult(IId                          AuthorizatorId,
                               AuthStopResultTypes          Result,
                               TimeSpan                     Runtime,
                               ISendAuthorizeStartStop?     ISendAuthorizeStartStop      = null,
                               IReceiveAuthorizeStartStop?  IReceiveAuthorizeStartStop   = null,

                               DateTimeOffset?              CachedResultEndOfLifeTime    = null,
                               ChargingSession_Id?          SessionId                    = null,
                               EMobilityProvider_Id?        ProviderId                   = null,
                               I18NString?                  Description                  = null,
                               I18NString?                  AdditionalInfo               = null,
                               JObject?                     AdditionalContext            = null,

                               Byte                         NumberOfRetries              = 0)
        {

            this.AuthorizatorId              = AuthorizatorId ?? throw new ArgumentNullException(nameof(AuthorizatorId), "The given identification of the authorizator must not be null!");
            this.Result                      = Result;
            this.Runtime                     = Runtime;

            this.ISendAuthorizeStartStop     = ISendAuthorizeStartStop;
            this.IReceiveAuthorizeStartStop  = IReceiveAuthorizeStartStop;

            this.CachedResultEndOfLifeTime   = CachedResultEndOfLifeTime;
            this.SessionId                   = SessionId;
            this.ProviderId                  = ProviderId     ?? new EMobilityProvider_Id?();
            this.Description                 = Description    ?? I18NString.Empty;
            this.AdditionalInfo              = AdditionalInfo;
            this.AdditionalContext           = AdditionalContext;

            this.NumberOfRetries             = NumberOfRetries;

        }

        #endregion

        #region (public)  AuthStopResult (AuthorizatorId, ISendAuthorizeStartStop,    Result, Runtime, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="AdditionalContext">Optional additional context information.</param>
        /// 
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public AuthStopResult(IId                      AuthorizatorId,
                              AuthStopResultTypes      Result,
                              TimeSpan                 Runtime,
                              ISendAuthorizeStartStop  ISendAuthorizeStartStop,

                              DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                              ChargingSession_Id?      SessionId                   = null,
                              EMobilityProvider_Id?    ProviderId                  = null,
                              I18NString?              Description                 = null,
                              I18NString?              AdditionalInfo              = null,
                              JObject?                 AdditionalContext           = null,

                              Byte                     NumberOfRetries             = 0)

            : this(AuthorizatorId,
                   Result,
                   Runtime,

                   ISendAuthorizeStartStop,
                   null,

                   CachedResultEndOfLifeTime,
                   SessionId,
                   ProviderId,
                   Description,
                   AdditionalInfo,
                   AdditionalContext,

                   NumberOfRetries)

        { }

        #endregion

        #region (public)  AuthStopResult (AuthorizatorId, IReceiveAuthorizeStartStop, Result, Runtime, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="AdditionalContext">Optional additional context information.</param>
        /// 
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public AuthStopResult(IId                         AuthorizatorId,
                              AuthStopResultTypes         Result,
                              TimeSpan                    Runtime,
                              IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,

                              DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                              ChargingSession_Id?         SessionId                   = null,
                              EMobilityProvider_Id?       ProviderId                  = null,
                              I18NString?                 Description                 = null,
                              I18NString?                 AdditionalInfo              = null,
                              JObject?                    AdditionalContext           = null,

                              Byte                        NumberOfRetries             = 0)

            : this(AuthorizatorId,
                   Result,
                   Runtime,

                   null,
                   IReceiveAuthorizeStartStop,

                   CachedResultEndOfLifeTime,
                   SessionId,
                   ProviderId,
                   Description,
                   AdditionalInfo,
                   AdditionalContext,

                   NumberOfRetries)

        { }

        #endregion

        #endregion


        #region (static) Unspecified         (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            Unspecified(IId                      AuthorizatorId,
                        TimeSpan                 Runtime,
                        ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                        DateTimeOffset?          CachedResultEndOfLifeTime   = null,

                        ChargingSession_Id?      SessionId                   = null,
                        I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.Unspecified,
                        Runtime,
                        ISendAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description);



        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            Unspecified(IId                         AuthorizatorId,
                        TimeSpan                    Runtime,
                        IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                        DateTimeOffset?             CachedResultEndOfLifeTime   = null,

                        ChargingSession_Id?         SessionId                   = null,
                        I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.Unspecified,
                        Runtime,
                        IReceiveAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description);

        #endregion

        #region (static) AdminDown           (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            AdminDown(IId                      AuthorizatorId,
                      TimeSpan                 Runtime,
                      ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                      DateTimeOffset?          CachedResultEndOfLifeTime   = null,

                      ChargingSession_Id?      SessionId                   = null,
                      I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.AdminDown,
                        Runtime,
                        ISendAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("The authentication service was disabled by the administrator!"));



        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            AdminDown(IId                         AuthorizatorId,
                      TimeSpan                    Runtime,
                      IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                      DateTimeOffset?             CachedResultEndOfLifeTime   = null,

                      ChargingSession_Id?         SessionId                   = null,
                      I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.AdminDown,
                        Runtime,
                        IReceiveAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("The authentication service was disabled by the administrator!"));

        #endregion

        #region (static) UnknownLocation     (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            UnknownLocation(IId                      AuthorizatorId,
                            TimeSpan                 Runtime,
                            ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                            DateTimeOffset?          CachedResultEndOfLifeTime   = null,

                            ChargingSession_Id?      SessionId                   = null,
                            I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.UnknownLocation,
                        Runtime,
                        ISendAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Unknown location!"));



        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            UnknownLocation(IId                         AuthorizatorId,
                            TimeSpan                    Runtime,
                            IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                            DateTimeOffset?             CachedResultEndOfLifeTime   = null,

                            ChargingSession_Id?         SessionId                   = null,
                            I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.UnknownLocation,
                        Runtime,
                        IReceiveAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Unknown location!"));

        #endregion

        #region (static) InvalidToken        (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            InvalidToken(IId                      AuthorizatorId,
                         TimeSpan                 Runtime,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         DateTimeOffset?          CachedResultEndOfLifeTime   = null,

                         ChargingSession_Id?      SessionId                   = null,
                         I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.InvalidToken,
                        Runtime,
                        ISendAuthorizeStartStop,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid token!"));



        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            InvalidToken(IId                         AuthorizatorId,
                         TimeSpan                    Runtime,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         DateTimeOffset?             CachedResultEndOfLifeTime   = null,

                         ChargingSession_Id?         SessionId                   = null,
                         I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.InvalidToken,
                        Runtime,
                        IReceiveAuthorizeStartStop,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid token!"));

        #endregion

        #region (static) InvalidSessionId    (AuthorizatorId, Runtime, ..., SessionId, ...)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            InvalidSessionId(IId                      AuthorizatorId,
                             TimeSpan                 Runtime,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             ChargingSession_Id       SessionId,
                             DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                             I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.InvalidSessionId,
                        Runtime,
                        ISendAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid session identification!"));



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            InvalidSessionId(IId                         AuthorizatorId,
                             TimeSpan                    Runtime,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             ChargingSession_Id          SessionId,
                             DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                             I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.InvalidSessionId,
                        Runtime,
                        IReceiveAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid session identification!"));

        #endregion

        #region (static) AlreadyStopped      (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The given charging session identification was already stopped.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            AlreadyStopped(IId                      AuthorizatorId,
                           TimeSpan                 Runtime,
                           ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                           DateTimeOffset?          CachedResultEndOfLifeTime   = null,

                           ChargingSession_Id?      SessionId                   = null,
                           I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.AlreadyStopped,
                        Runtime,
                        ISendAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Already stopped!"));



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            AlreadyStopped(IId                         AuthorizatorId,
                           TimeSpan                    Runtime,
                           IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                           DateTimeOffset?             CachedResultEndOfLifeTime   = null,

                           ChargingSession_Id?         SessionId                   = null,
                           I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.AlreadyStopped,
                        Runtime,
                        IReceiveAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Already stopped!"));

        #endregion

        #region (static) OutOfService        (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            OutOfService(IId                      AuthorizatorId,
                         TimeSpan                 Runtime,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         DateTimeOffset?          CachedResultEndOfLifeTime   = null,

                         ChargingSession_Id?      SessionId                   = null,
                         I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.OutOfService,
                        Runtime,
                        ISendAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Out of service!"));



        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            OutOfService(IId                         AuthorizatorId,
                         TimeSpan                    Runtime,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         DateTimeOffset?             CachedResultEndOfLifeTime   = null,

                         ChargingSession_Id?         SessionId                   = null,
                         I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.OutOfService,
                        Runtime,
                        IReceiveAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        Description:  Description ?? I18NString.Create("Out of service!"));

        #endregion

        #region (static) Authorized          (AuthorizatorId, Runtime, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            Authorized(IId                      AuthorizatorId,
                       TimeSpan                 Runtime,
                       ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                       DateTimeOffset?          CachedResultEndOfLifeTime   = null,

                       ChargingSession_Id?      SessionId                   = null,
                       EMobilityProvider_Id?    ProviderId                  = null,
                       I18NString?              Description                 = null,
                       I18NString?              AdditionalInfo              = null,
                       JObject?                 AdditionalContext           = null,

                       Byte                     NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        AuthStopResultTypes.Authorized,
                        Runtime,
                        ISendAuthorizeStartStop,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId,
                        Description ?? I18NString.Create("Success!"),
                        AdditionalInfo,
                        AdditionalContext,

                        NumberOfRetries);



        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            Authorized(IId                         AuthorizatorId,
                       TimeSpan                    Runtime,
                       IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                       DateTimeOffset?             CachedResultEndOfLifeTime   = null,

                       ChargingSession_Id?         SessionId                   = null,
                       EMobilityProvider_Id?       ProviderId                  = null,
                       I18NString?                 Description                 = null,
                       I18NString?                 AdditionalInfo              = null,
                       JObject?                    AdditionalContext           = null,

                       Byte                        NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        AuthStopResultTypes.Authorized,
                        Runtime,
                        IReceiveAuthorizeStartStop,
                        CachedResultEndOfLifeTime,

                        SessionId,
                        ProviderId,
                        Description ?? I18NString.Create("Success!"),
                        AdditionalInfo,
                        AdditionalContext,

                        NumberOfRetries);

        #endregion

        #region (static) NotAuthorized       (AuthorizatorId, Runtime, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unknown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            NotAuthorized(IId                      AuthorizatorId,
                          TimeSpan                 Runtime,
                          ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                          DateTimeOffset?          CachedResultEndOfLifeTime   = null,

                          ChargingSession_Id?      SessionId                   = null,
                          EMobilityProvider_Id?    ProviderId                  = null,
                          I18NString?              Description                 = null,
                          I18NString?              AdditionalInfo              = null,
                          JObject?                 AdditionalContext           = null,

                          Byte                     NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        AuthStopResultTypes.NotAuthorized,
                        Runtime,
                        ISendAuthorizeStartStop,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId,
                        Description ?? I18NString.Create("Not Authorized"),
                        AdditionalInfo,
                        AdditionalContext,

                        NumberOfRetries);



        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unknown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            NotAuthorized(IId                         AuthorizatorId,
                          TimeSpan                    Runtime,
                          IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,

                          DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                          ChargingSession_Id?         SessionId                   = null,
                          EMobilityProvider_Id?       ProviderId                  = null,
                          I18NString?                 Description                 = null,
                          I18NString?                 AdditionalInfo              = null,
                          JObject?                    AdditionalContext           = null,

                          Byte                        NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        AuthStopResultTypes.NotAuthorized,
                        Runtime,
                        IReceiveAuthorizeStartStop,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId,
                        Description ?? I18NString.Create("Not Authorized"),
                        AdditionalInfo,
                        AdditionalContext,

                        NumberOfRetries);

        #endregion

        #region (static) Blocked             (AuthorizatorId, Runtime, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            Blocked(IId                      AuthorizatorId,
                    TimeSpan                 Runtime,
                    ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                    DateTimeOffset?          CachedResultEndOfLifeTime   = null,

                    ChargingSession_Id?      SessionId                   = null,
                    EMobilityProvider_Id?    ProviderId                  = null,
                    I18NString?              Description                 = null,
                    I18NString?              AdditionalInfo              = null,
                    JObject?                 AdditionalContext           = null,

                    Byte                     NumberOfRetries             = 0)


            => new (AuthorizatorId,
                    AuthStopResultTypes.Blocked,
                    Runtime,
                    ISendAuthorizeStartStop,
                    CachedResultEndOfLifeTime,

                    SessionId,
                    ProviderId,
                    Description ?? I18NString.Create("Blocked!"),
                    AdditionalInfo,
                    AdditionalContext,

                    NumberOfRetries);



        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            Blocked(IId                         AuthorizatorId,
                    TimeSpan                    Runtime,
                    IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                    DateTimeOffset?             CachedResultEndOfLifeTime   = null,

                    ChargingSession_Id?         SessionId                   = null,
                    EMobilityProvider_Id?       ProviderId                  = null,
                    I18NString?                 Description                 = null,
                    I18NString?                 AdditionalInfo              = null,
                    JObject?                    AdditionalContext           = null,

                    Byte                        NumberOfRetries             = 0)


            => new (AuthorizatorId,
                    AuthStopResultTypes.Blocked,
                    Runtime,
                    IReceiveAuthorizeStartStop,

                    CachedResultEndOfLifeTime,
                    SessionId,
                    ProviderId,
                    Description ?? I18NString.Create("Blocked!"),
                    AdditionalInfo,
                    AdditionalContext,

                    NumberOfRetries);

        #endregion

        #region (static) CommunicationTimeout(AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            CommunicationTimeout(IId                      AuthorizatorId,
                                 TimeSpan                 Runtime,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,

                                 ChargingSession_Id?      SessionId     = null,
                                 I18NString?              Description   = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.CommunicationTimeout,
                        Runtime,
                        ISendAuthorizeStartStop,
                        null,
                        null,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Communication timeout!"));



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
                                 TimeSpan                    Runtime,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,

                                 ChargingSession_Id?         SessionId     = null,
                                 I18NString?                 Description   = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.CommunicationTimeout,
                        Runtime,
                        IReceiveAuthorizeStartStop,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Communication timeout!"));

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId, Runtime, ...)

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
                                TimeSpan                 Runtime,
                                ISendAuthorizeStartStop  ISendAuthorizeStartStop,

                                ChargingSession_Id?      SessionId     = null,
                                I18NString?              Description   = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.StopChargingTimeout,
                        Runtime,
                        ISendAuthorizeStartStop,
                        null,
                        null,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Stop charging timeout!"));



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
                                TimeSpan                    Runtime,
                                IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,

                                ChargingSession_Id?         SessionId     = null,
                                I18NString?                 Description   = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.StopChargingTimeout,
                        Runtime,
                        IReceiveAuthorizeStartStop,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Stop charging timeout!"));

        #endregion

        #region (static) RateLimitReached    (AuthorizatorId, Runtime, ...)

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
                             TimeSpan                 Runtime,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,

                             ChargingSession_Id?      SessionId     = null,
                             I18NString?              Description   = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.RateLimitReached,
                        Runtime,
                        ISendAuthorizeStartStop,
                        null,
                        null,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Rate limit reached!"));



        /// <summary>
        /// The authorize start operation reached a rate limit.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            RateLimitReached(IId                         AuthorizatorId,
                             TimeSpan                    Runtime,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,

                             ChargingSession_Id?         SessionId     = null,
                             I18NString?                 Description   = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.RateLimitReached,
                        Runtime,
                        IReceiveAuthorizeStartStop,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("RateLimitReached!"));

        #endregion

        #region (static) Error               (AuthorizatorId, SessionId = null, Description = null, Runtime = null)

        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            Error(IId                      AuthorizatorId,
                  TimeSpan                 Runtime,
                  ISendAuthorizeStartStop  ISendAuthorizeStartStop,

                  ChargingSession_Id?      SessionId     = null,
                  I18NString?              Description   = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.Error,
                        Runtime,
                        ISendAuthorizeStartStop,
                        null,
                        null,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Error!"));



        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            Error(IId                         AuthorizatorId,
                  TimeSpan                    Runtime,
                  IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,

                  ChargingSession_Id?         SessionId     = null,
                  I18NString?                 Description   = null)


                => new (AuthorizatorId,
                        AuthStopResultTypes.Error,
                        Runtime,
                        IReceiveAuthorizeStartStop,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Error!"));

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
                                 new JProperty("runtime",                     Runtime.       TotalMilliseconds),

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

                           AdditionalContext is not null
                               ? new JProperty("additionalContext",           AdditionalContext)
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
