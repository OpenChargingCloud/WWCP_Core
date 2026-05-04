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

using System.Diagnostics.CodeAnalysis;

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
        /// The response timestamp.
        /// </summary>
        public DateTimeOffset               ResponseTimestamp                { get; }

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
        /// The optional name of the e-mobility provider.
        /// </summary>
        public String?                      ProviderName                     { get; }

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

        #region (private) AuthStopResult (AuthorizatorId,                             ResponseTimestamp, Result, Runtime, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
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
                               DateTimeOffset               ResponseTimestamp,
                               AuthStopResultTypes          Result,
                               TimeSpan                     Runtime,

                               ISendAuthorizeStartStop?     ISendAuthorizeStartStop      = null,
                               IReceiveAuthorizeStartStop?  IReceiveAuthorizeStartStop   = null,

                               DateTimeOffset?              CachedResultEndOfLifeTime    = null,
                               ChargingSession_Id?          SessionId                    = null,
                               EMobilityProvider_Id?        ProviderId                   = null,
                               String?                      ProviderName                 = null,
                               I18NString?                  Description                  = null,
                               I18NString?                  AdditionalInfo               = null,
                               JObject?                     AdditionalContext            = null,

                               Byte                         NumberOfRetries              = 0)
        {

            this.AuthorizatorId              = AuthorizatorId ?? throw new ArgumentNullException(nameof(AuthorizatorId), "The given identification of the authorizator must not be null!");
            this.ResponseTimestamp           = ResponseTimestamp;
            this.Result                      = Result;
            this.Runtime                     = Runtime;

            this.ISendAuthorizeStartStop     = ISendAuthorizeStartStop;
            this.IReceiveAuthorizeStartStop  = IReceiveAuthorizeStartStop;

            this.CachedResultEndOfLifeTime   = CachedResultEndOfLifeTime;
            this.SessionId                   = SessionId;
            this.ProviderId                  = ProviderId     ?? new EMobilityProvider_Id?();
            this.ProviderName                = ProviderName;
            this.Description                 = Description    ?? I18NString.Empty;
            this.AdditionalInfo              = AdditionalInfo;
            this.AdditionalContext           = AdditionalContext;

            this.NumberOfRetries             = NumberOfRetries;

        }

        #endregion

        #region (public)  AuthStopResult (AuthorizatorId, ISendAuthorizeStartStop,    ResponseTimestamp, Result, Runtime, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
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
                              ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                              DateTimeOffset           ResponseTimestamp,
                              AuthStopResultTypes      Result,
                              TimeSpan                 Runtime,

                              DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                              ChargingSession_Id?      SessionId                   = null,
                              EMobilityProvider_Id?    ProviderId                  = null,
                              String?                  ProviderName                = null,
                              I18NString?              Description                 = null,
                              I18NString?              AdditionalInfo              = null,
                              JObject?                 AdditionalContext           = null,

                              Byte                     NumberOfRetries             = 0)

            : this(AuthorizatorId,
                   ResponseTimestamp,
                   Result,
                   Runtime,

                   ISendAuthorizeStartStop,
                   null,

                   CachedResultEndOfLifeTime,
                   SessionId,
                   ProviderId,
                   ProviderName,
                   Description,
                   AdditionalInfo,
                   AdditionalContext,

                   NumberOfRetries)

        { }

        #endregion

        #region (public)  AuthStopResult (AuthorizatorId, IReceiveAuthorizeStartStop, ResponseTimestamp, Result, Runtime, ...)

        /// <summary>
        /// Create a new abstract authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
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
                              IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                              DateTimeOffset              ResponseTimestamp,
                              AuthStopResultTypes         Result,
                              TimeSpan                    Runtime,

                              DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                              ChargingSession_Id?         SessionId                   = null,
                              EMobilityProvider_Id?       ProviderId                  = null,
                              String?                     ProviderName                = null,
                              I18NString?                 Description                 = null,
                              I18NString?                 AdditionalInfo              = null,
                              JObject?                    AdditionalContext           = null,

                              Byte                        NumberOfRetries             = 0)

            : this(AuthorizatorId,
                   ResponseTimestamp,
                   Result,
                   Runtime,

                   null,
                   IReceiveAuthorizeStartStop,

                   CachedResultEndOfLifeTime,
                   SessionId,
                   ProviderId,
                   ProviderName,
                   Description,
                   AdditionalInfo,
                   AdditionalContext,

                   NumberOfRetries)

        { }

        #endregion

        #endregion


        #region (static) Parse   (JSON, CustomAuthStopResultParser = null)

        /// <summary>
        /// Parse the given JSON representation of an AuthStopResult.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomAuthStopResultParser">A delegate to parse custom AuthStopResult JSON objects.</param>
        public static AuthStopResult Parse(JObject                                       JSON,
                                           ISendAuthorizeStartStop?                      ISendAuthorizeStartStop      = null,
                                           IReceiveAuthorizeStartStop?                   IReceiveAuthorizeStartStop   = null,
                                           CustomJObjectParserDelegate<AuthStopResult>?  CustomAuthStopResultParser   = null)
        {

            if (TryParse(JSON,
                         out var authStopResult,
                         out var errorResponse,
                         ISendAuthorizeStartStop,
                         IReceiveAuthorizeStartStop,
                         CustomAuthStopResultParser))
            {
                return authStopResult;
            }

            throw new ArgumentException("The given JSON representation of an AuthStopResult is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out AuthStopResult, out ErrorResponse, CustomAuthStopResultParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an AuthStopResult.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="AuthStopResult">The parsed AuthStopResult.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                   JSON,
                                       [NotNullWhen(true)]  out AuthStopResult?  AuthStopResult,
                                       [NotNullWhen(false)] out String?          ErrorResponse,
                                       ISendAuthorizeStartStop?                  ISendAuthorizeStartStop      = null,
                                       IReceiveAuthorizeStartStop?               IReceiveAuthorizeStartStop   = null)

            => TryParse(JSON,
                        out AuthStopResult,
                        out ErrorResponse,
                        ISendAuthorizeStartStop,
                        IReceiveAuthorizeStartStop,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an AuthStopResult.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="AuthStopResult">The parsed AuthStopResult.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomAuthStopResultParser">A delegate to parse custom AuthStopResult JSON objects.</param>
        public static Boolean TryParse(JObject                                       JSON,
                                       [NotNullWhen(true)]  out AuthStopResult?      AuthStopResult,
                                       [NotNullWhen(false)] out String?              ErrorResponse,
                                       ISendAuthorizeStartStop?                      ISendAuthorizeStartStop      = null,
                                       IReceiveAuthorizeStartStop?                   IReceiveAuthorizeStartStop   = null,
                                       CustomJObjectParserDelegate<AuthStopResult>?  CustomAuthStopResultParser   = null)
        {

            try
            {

                AuthStopResult = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse AuthorizatorId               [mandatory]

                if (!JSON.ParseMandatory("authorizatorId",
                                         "authorizator identification",
                                         Authorizator_Id.TryParse,
                                         out Authorizator_Id authorizatorId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ResponseTimestamp            [mandatory]

                if (!JSON.ParseMandatory("responseTimestamp",
                                         "response timestamp",
                                         out DateTimeOffset responseTimestamp,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Result                       [mandatory]

                if (!JSON.ParseMandatoryEnum("result",
                                             "result",
                                             out AuthStopResultTypes result,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Runtime                      [mandatory]

                if (!JSON.ParseMandatory("result",
                                         "result",
                                         TimeSpanExtensions.TryParseMilliseconds,
                                         out TimeSpan runtime,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse CachedResultEndOfLifeTime    [optional]

                if (!JSON.ParseOptional("cachedResultEndOfLifeTime",
                                        "cached result end-of-life-time",
                                        out DateTimeOffset? cachedResultEndOfLifeTime,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse SessionId                    [optional]

                if (!JSON.ParseOptional("sessionId",
                                        "session identification",
                                        ChargingSession_Id.TryParse,
                                        out ChargingSession_Id? sessionId,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse EMPPartnerSessionId          [optional]

                if (!JSON.ParseOptional("empPartnerSessionId",
                                        "EMP partner session identification",
                                        ChargingSession_Id.TryParse,
                                        out ChargingSession_Id? empPartnerSessionId,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AuthorizationReference       [optional]

                if (!JSON.ParseOptional("authorizationReference",
                                        "authorization reference",
                                        WWCP.AuthorizationReference.TryParse,
                                        out AuthorizationReference? authorizationReference,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ContractId                   [optional]

                var contractId = JSON.GetString("contractId");

                #endregion

                #region Parse PrintedNumber                [optional]

                var printedNumber = JSON.GetString("printedNumber");

                #endregion

                #region Parse UILanguage                   [optional]

                if (!JSON.ParseOptionalEnum("uiLanguage",
                                            "UI language",
                                            out Languages? uiLanguage,
                                            out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ExpiryDate                   [optional]

                if (!JSON.ParseOptional("expiryDate",
                                        "expiry date",
                                        out DateTimeOffset? expiryDate,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MaxPower                     [optional]

                if (!JSON.ParseOptional("maxPower",
                                        "max power",
                                        Watt.TryParse,
                                        out Watt? maxPower,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MaxEnergy                    [optional]

                if (!JSON.ParseOptional("maxEnergy",
                                        "max energy",
                                        WattHour.TryParse,
                                        out WattHour? maxEnergy,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MaxDuration                  [optional]

                if (!JSON.ParseOptional("maxDuration",
                                        "max duration",
                                        TimeSpanExtensions.TryParseMinutes,
                                        out TimeSpan? maxDuration,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion



                #region Parse ProviderId                   [optional]

                if (!JSON.ParseOptional("providerId",
                                        "provider identification",
                                        EMobilityProvider_Id.TryParse,
                                        out EMobilityProvider_Id? providerId,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ProviderName                 [optional]

                var providerName = JSON.GetString("providerName");

                #endregion

                #region Parse Description                  [optional]

                if (!JSON.ParseOptional("description",
                                        "description",
                                        I18NString.TryParse,
                                        out I18NString? description,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AdditionalInfo               [optional]

                if (!JSON.ParseOptional("additionalInfo",
                                        "additional info",
                                        I18NString.TryParse,
                                        out I18NString? additionalInfo,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AdditionalContext            [optional]

                var additionalContext = JSON["additionalContext"] as JObject;

                #endregion


                #region Parse ProviderName                 [optional]

                var numberOfRetries = JSON["numberOfRetries"]?.Value<Byte>() ?? 0;

                #endregion


                AuthStopResult = new AuthStopResult(

                                     authorizatorId,
                                     responseTimestamp,
                                     result,
                                     runtime,
                                     ISendAuthorizeStartStop,
                                     IReceiveAuthorizeStartStop,

                                     cachedResultEndOfLifeTime,
                                     sessionId,
                                     //empPartnerSessionId,
                                     //authorizationReference,
                                     //contractId,
                                     //printedNumber,
                                     //uiLanguage,
                                     //expiryDate,

                                     //maxPower,
                                     //maxEnergy,
                                     //maxDuration,
                                     //null, //chargingTariffs,
                                     //null, //listOfAuthStopTokens,
                                     //null, //listOfAuthStopPINs,

                                     providerId,
                                     providerName,
                                     description,
                                     additionalInfo,
                                     additionalContext,

                                     numberOfRetries

                                 );

                if (CustomAuthStopResultParser is not null)
                    AuthStopResult = CustomAuthStopResultParser(JSON,
                                                                AuthStopResult);

                return true;

            }
            catch (Exception e)
            {
                AuthStopResult  = default;
                ErrorResponse    = "The given JSON representation of an AuthStopResult is invalid: " + e.Message;
                return false;
            }

        }

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


        #region Static methods

        #region (static) Unspecified          (AuthorizatorId, ResponseTimestamp, Runtime, SessionId = null, Description = null)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            Unspecified(IId                      AuthorizatorId,
                        ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                        DateTimeOffset           ResponseTimestamp,
                        TimeSpan                 Runtime,

                        ChargingSession_Id?      SessionId                   = null,
                        I18NString?              Description                 = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.Unspecified,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description);



        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            Unspecified(IId                         AuthorizatorId,
                        IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                        DateTimeOffset              ResponseTimestamp,
                        TimeSpan                    Runtime,

                        ChargingSession_Id?         SessionId                   = null,
                        I18NString?                 Description                 = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.Unspecified,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description);

        #endregion

        #region (static) AdminDown            (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            AdminDown(IId                      AuthorizatorId,
                      ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                      DateTimeOffset           ResponseTimestamp,
                      TimeSpan                 Runtime,

                      DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                      ChargingSession_Id?      SessionId                   = null,
                      I18NString?              Description                 = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.AdminDown,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("The authentication service was disabled by the administrator!"));



        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            AdminDown(IId                         AuthorizatorId,
                      IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                      DateTimeOffset              ResponseTimestamp,
                      TimeSpan                    Runtime,

                      DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                      ChargingSession_Id?         SessionId                   = null,
                      I18NString?                 Description                 = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.AdminDown,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("The authentication service was disabled by the administrator!"));

        #endregion

        #region (static) UnknownLocation      (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            UnknownLocation(IId                      AuthorizatorId,
                            ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                            DateTimeOffset           ResponseTimestamp,
                            TimeSpan                 Runtime,

                            DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                            ChargingSession_Id?      SessionId                   = null,
                            I18NString?              Description                 = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.UnknownLocation,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Unknown location!"));



        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            UnknownLocation(IId                         AuthorizatorId,
                            IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                            DateTimeOffset              ResponseTimestamp,
                            TimeSpan                    Runtime,

                            DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                            ChargingSession_Id?         SessionId                   = null,
                            I18NString?                 Description                 = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.UnknownLocation,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Unknown location!"));

        #endregion

        #region (static) InvalidToken         (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            InvalidToken(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         DateTimeOffset           ResponseTimestamp,
                         TimeSpan                 Runtime,

                         DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                         ChargingSession_Id?      SessionId                   = null,
                         I18NString?              Description                 = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.InvalidToken,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid token!"));



        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            InvalidToken(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         DateTimeOffset              ResponseTimestamp,
                         TimeSpan                    Runtime,

                         DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                         ChargingSession_Id?         SessionId                   = null,
                         I18NString?                 Description                 = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.InvalidToken,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid token!"));

        #endregion

        #region (static) InvalidSessionId     (AuthorizatorId, Runtime, ..., SessionId, ...)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            InvalidSessionId(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             DateTimeOffset           ResponseTimestamp,
                             TimeSpan                 Runtime,

                             ChargingSession_Id       SessionId,
                             DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                             I18NString?              Description                 = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.InvalidSessionId,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid session identification!"));



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            InvalidSessionId(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             DateTimeOffset              ResponseTimestamp,
                             TimeSpan                    Runtime,

                             ChargingSession_Id          SessionId,
                             DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                             I18NString?                 Description                 = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.InvalidSessionId,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid session identification!"));

        #endregion

        #region (static) AlreadyStopped       (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The given charging session identification was already stopped.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            AlreadyStopped(IId                      AuthorizatorId,
                           ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                           DateTimeOffset           ResponseTimestamp,
                           TimeSpan                 Runtime,

                           DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                           ChargingSession_Id?      SessionId                   = null,
                           I18NString?              Description                 = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.AlreadyStopped,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Already stopped!"));



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            AlreadyStopped(IId                         AuthorizatorId,
                           IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                           DateTimeOffset              ResponseTimestamp,
                           TimeSpan                    Runtime,

                           DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                           ChargingSession_Id?         SessionId                   = null,
                           I18NString?                 Description                 = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.AlreadyStopped,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Already stopped!"));

        #endregion

        #region (static) OutOfService         (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            OutOfService(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         DateTimeOffset           ResponseTimestamp,
                         TimeSpan                 Runtime,

                         DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                         ChargingSession_Id?      SessionId                   = null,
                         I18NString?              Description                 = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.OutOfService,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Out of service!"));



        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            OutOfService(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         DateTimeOffset              ResponseTimestamp,
                         TimeSpan                    Runtime,

                         DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                         ChargingSession_Id?         SessionId                   = null,
                         I18NString?                 Description                 = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.OutOfService,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Out of service!"));

        #endregion

        #region (static) Authorized           (AuthorizatorId, Runtime, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            Authorized(IId                      AuthorizatorId,
                       ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                       DateTimeOffset           ResponseTimestamp,
                       TimeSpan                 Runtime,

                       DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                       ChargingSession_Id?      SessionId                   = null,
                       EMobilityProvider_Id?    ProviderId                  = null,
                       String?                  ProviderName                = null,
                       I18NString?              Description                 = null,
                       I18NString?              AdditionalInfo              = null,
                       JObject?                 AdditionalContext           = null,

                       Byte                     NumberOfRetries             = 0)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.Authorized,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId,
                        ProviderName,
                        Description ?? I18NString.Create("Success!"),
                        AdditionalInfo,
                        AdditionalContext,

                        NumberOfRetries);



        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            Authorized(IId                         AuthorizatorId,
                       IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                       DateTimeOffset              ResponseTimestamp,
                       TimeSpan                    Runtime,

                       DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                       ChargingSession_Id?         SessionId                   = null,
                       EMobilityProvider_Id?       ProviderId                  = null,
                       String?                     ProviderName                = null,
                       I18NString?                 Description                 = null,
                       I18NString?                 AdditionalInfo              = null,
                       JObject?                    AdditionalContext           = null,

                       Byte                        NumberOfRetries             = 0)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.Authorized,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId,
                        ProviderName,
                        Description ?? I18NString.Create("Success!"),
                        AdditionalInfo,
                        AdditionalContext,

                        NumberOfRetries);

        #endregion

        #region (static) NotAuthorized        (AuthorizatorId, Runtime, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unknown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            NotAuthorized(IId                      AuthorizatorId,
                          ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                          DateTimeOffset           ResponseTimestamp,
                          TimeSpan                 Runtime,

                          DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                          ChargingSession_Id?      SessionId                   = null,
                          EMobilityProvider_Id?    ProviderId                  = null,
                          String?                  ProviderName                = null,
                          I18NString?              Description                 = null,
                          I18NString?              AdditionalInfo              = null,
                          JObject?                 AdditionalContext           = null,

                          Byte                     NumberOfRetries             = 0)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.NotAuthorized,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId,
                        ProviderName,
                        Description ?? I18NString.Create("Not Authorized"),
                        AdditionalInfo,
                        AdditionalContext,

                        NumberOfRetries);



        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unknown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            NotAuthorized(IId                         AuthorizatorId,
                          IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                          DateTimeOffset              ResponseTimestamp,
                          TimeSpan                    Runtime,

                          DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                          ChargingSession_Id?         SessionId                   = null,
                          EMobilityProvider_Id?       ProviderId                  = null,
                          String?                     ProviderName                = null,
                          I18NString?                 Description                 = null,
                          I18NString?                 AdditionalInfo              = null,
                          JObject?                    AdditionalContext           = null,

                          Byte                        NumberOfRetries             = 0)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.NotAuthorized,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId,
                        ProviderName,
                        Description ?? I18NString.Create("Not Authorized"),
                        AdditionalInfo,
                        AdditionalContext,

                        NumberOfRetries);

        #endregion

        #region (static) Blocked              (AuthorizatorId, Runtime, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            Blocked(IId                      AuthorizatorId,
                    ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                    DateTimeOffset           ResponseTimestamp,
                    TimeSpan                 Runtime,

                    DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                    ChargingSession_Id?      SessionId                   = null,
                    EMobilityProvider_Id?    ProviderId                  = null,
                    String?                  ProviderName                = null,
                    I18NString?              Description                 = null,
                    I18NString?              AdditionalInfo              = null,
                    JObject?                 AdditionalContext           = null,

                    Byte                     NumberOfRetries             = 0)


            => new  (AuthorizatorId,
                    ISendAuthorizeStartStop,
                    ResponseTimestamp,
                    AuthStopResultTypes.Blocked,
                    Runtime,

                    CachedResultEndOfLifeTime,
                    SessionId,
                    ProviderId,
                    ProviderName,
                    Description ?? I18NString.Create("Blocked!"),
                    AdditionalInfo,
                    AdditionalContext,

                    NumberOfRetries);



        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult

            Blocked(IId                         AuthorizatorId,
                    IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                    DateTimeOffset              ResponseTimestamp,
                    TimeSpan                    Runtime,

                    DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                    ChargingSession_Id?         SessionId                   = null,
                    EMobilityProvider_Id?       ProviderId                  = null,
                    String?                     ProviderName                = null,
                    I18NString?                 Description                 = null,
                    I18NString?                 AdditionalInfo              = null,
                    JObject?                    AdditionalContext           = null,

                    Byte                        NumberOfRetries             = 0)


            => new  (AuthorizatorId,
                    IReceiveAuthorizeStartStop,
                    ResponseTimestamp,
                    AuthStopResultTypes.Blocked,
                    Runtime,

                    CachedResultEndOfLifeTime,
                    SessionId,
                    ProviderId,
                    ProviderName,
                    Description ?? I18NString.Create("Blocked!"),
                    AdditionalInfo,
                    AdditionalContext,

                    NumberOfRetries);

        #endregion

        #region (static) CommunicationTimeout (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            CommunicationTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 DateTimeOffset           ResponseTimestamp,
                                 TimeSpan                 Runtime,

                                 ChargingSession_Id?      SessionId     = null,
                                 I18NString?              Description   = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.CommunicationTimeout,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Communication timeout!"));



        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            CommunicationTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 DateTimeOffset              ResponseTimestamp,
                                 TimeSpan                    Runtime,

                                 ChargingSession_Id?         SessionId     = null,
                                 I18NString?                 Description   = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.CommunicationTimeout,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Communication timeout!"));

        #endregion

        #region (static) StartChargingTimeout (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            StopChargingTimeout(IId                      AuthorizatorId,
                                ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                DateTimeOffset           ResponseTimestamp,
                                TimeSpan                 Runtime,

                                ChargingSession_Id?      SessionId     = null,
                                I18NString?              Description   = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.StopChargingTimeout,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Stop charging timeout!"));



        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            StopChargingTimeout(IId                         AuthorizatorId,
                                IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                DateTimeOffset              ResponseTimestamp,
                                TimeSpan                    Runtime,

                                ChargingSession_Id?         SessionId     = null,
                                I18NString?                 Description   = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.StopChargingTimeout,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Stop charging timeout!"));

        #endregion

        #region (static) RateLimitReached     (AuthorizatorId, Runtime, ...)

        /// <summary>
        /// The authorize stop operation reached a rate limit.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            RateLimitReached(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             DateTimeOffset           ResponseTimestamp,
                             TimeSpan                 Runtime,

                             ChargingSession_Id?      SessionId     = null,
                             I18NString?              Description   = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.RateLimitReached,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Rate limit reached!"));



        /// <summary>
        /// The authorize start operation reached a rate limit.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            RateLimitReached(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             DateTimeOffset              ResponseTimestamp,
                             TimeSpan                    Runtime,

                             ChargingSession_Id?         SessionId     = null,
                             I18NString?                 Description   = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.RateLimitReached,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("RateLimitReached!"));

        #endregion

        #region (static) Error                (AuthorizatorId, SessionId = null, Description = null)

        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            Error(IId                      AuthorizatorId,
                  ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                  DateTimeOffset           ResponseTimestamp,
                  TimeSpan                 Runtime,

                  ChargingSession_Id?      SessionId     = null,
                  I18NString?              Description   = null)


                => new  (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.Error,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Error!"));



        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStopResult

            Error(IId                         AuthorizatorId,
                  IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                  DateTimeOffset              ResponseTimestamp,
                  TimeSpan                    Runtime,

                  ChargingSession_Id?         SessionId     = null,
                  I18NString?                 Description   = null)


                => new  (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStopResultTypes.Error,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Error!"));

        #endregion

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
