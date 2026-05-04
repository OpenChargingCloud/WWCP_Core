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
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The result of an AuthorizeStart request.
    /// </summary>
    public class AuthStartResult
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext = "https://open.charging.cloud/contexts/wwcp+json/authStartResult";

        #endregion

        #region Properties

        /// <summary>
        /// The identification of the authorizing entity.
        /// </summary>
        public IId                               AuthorizatorId                   { get; }

        /// <summary>
        /// The entity asking for an authorization.
        /// </summary>
        public ISendAuthorizeStartStop?          ISendAuthorizeStartStop          { get; }

        /// <summary>
        /// The entity giving an authorization.
        /// </summary>
        public IReceiveAuthorizeStartStop?       IReceiveAuthorizeStartStop       { get; }

        /// <summary>
        /// The result of the AuthorizeStart request.
        /// </summary>
        public AuthStartResultTypes              Result                           { get; }

        /// <summary>
        /// The response timestamp.
        /// </summary>
        public DateTimeOffset                    ResponseTimestamp                { get; }

        /// <summary>
        /// An optional timestamp until the result may be cached.
        /// </summary>
        public DateTimeOffset?                   CachedResultEndOfLifeTime        { get; set; }

        /// <summary>
        /// The optional remaining life time of a cached result.
        /// </summary>
        public TimeSpan?                         CachedResultRemainingLifeTime

            => CachedResultEndOfLifeTime.HasValue
                   ? CachedResultEndOfLifeTime.Value - Timestamp.Now
                   : null;

        /// <summary>
        /// The optional charging session identification, when the AuthorizeStart operation was successful.
        /// </summary>
        public ChargingSession_Id?               SessionId                        { get; }

        /// <summary>
        /// An optional EMP partner charging session identification, when the AuthorizeStart operation was successful.
        /// </summary>
        public ChargingSession_Id?               EMPPartnerSessionId              { get; }

        /// <summary>
        /// An optional authorization reference.
        /// </summary>
        public AuthorizationReference?           AuthorizationReference           { get; }

        /// <summary>
        /// An optional contract identification.
        /// </summary>
        public String?                           ContractId                       { get; }

        /// <summary>
        /// An optional printed number.
        /// </summary>
        public String?                           PrintedNumber                    { get; }

        /// <summary>
        /// The optional user interface language to be used for further interactions with the user.
        /// </summary>
        public Languages?                        UILanguage                       { get; }

        /// <summary>
        /// The timestamp when this authorization expires.
        /// </summary>
        public DateTimeOffset?                   ExpiryDate                       { get; }

        /// <summary>
        /// The optional maximum allowed charging current.
        /// </summary>
        public Watt?                             MaxPower                            { get;}

        /// <summary>
        /// The optional maximum allowed charging energy.
        /// </summary>
        public WattHour?                         MaxEnergy                           { get;}

        /// <summary>
        /// The optional maximum allowed charging duration.
        /// </summary>
        public TimeSpan?                         MaxDuration                      { get; }

        /// <summary>
        /// Optional charging tariff information.
        /// </summary>
        public IEnumerable<ChargingTariff>       ChargingTariffs                  { get; }

        /// <summary>
        /// An optional list of authorize stop tokens.
        /// </summary>
        public IEnumerable<AuthenticationToken>  ListOfAuthStopTokens             { get; }

        /// <summary>
        /// An optional list of authorize stop PINs.
        /// </summary>
        public IEnumerable<UInt32>               ListOfAuthStopPINs               { get; }


        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public EMobilityProvider_Id?             ProviderId                       { get; }

        /// <summary>
        /// The optional name of the e-mobility provider.
        /// </summary>
        public String?                           ProviderName                     { get; }

        /// <summary>
        /// A optional description of the AuthorizeStart result, e.g. in case of an error.
        /// </summary>
        public I18NString                        Description                      { get; }

        /// <summary>
        /// An optional additional message, e.g. in case of an error.
        /// </summary>
        public I18NString                        AdditionalInfo                   { get; }

        /// <summary>
        /// Optional additional context information.
        /// </summary>
        public JObject?                          AdditionalContext                { get; }


        /// <summary>
        /// Number of transmission retries.
        /// </summary>
        public Byte                              NumberOfRetries                  { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan                          Runtime                          { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthStartResult (AuthorizatorId,                             ResponseTimestamp, Result, Runtime, ...)

        /// <summary>
        /// Create a new AuthorizeStart result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Result">The AuthorizeStart result type.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="AuthorizationReference">An optional authorization reference.</param>
        /// <param name="ContractId">An optional contract identification.</param>
        /// <param name="PrintedNumber">An optional printed number.</param>
        /// <param name="UILanguage">An optional user interface language to be used for further interactions with the user.</param>
        /// <param name="ExpiryDate">The timestamp when this authorization expires.</param>
        /// <param name="MaxPower">The optional maximum allowed charging current.</param>
        /// <param name="MaxEnergy">The optional maximum allowed charging energy.</param>
        /// <param name="MaxDuration">The optional maximum allowed charging duration.</param>
        /// <param name="ChargingTariffs">Optional charging tariff information.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="ProviderName">An optional e-mobility provider name.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="AdditionalContext">Optional additional context information.</param>
        /// 
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        private AuthStartResult(IId                                AuthorizatorId,
                                DateTimeOffset                     ResponseTimestamp,
                                AuthStartResultTypes               Result,
                                TimeSpan                           Runtime,

                                ISendAuthorizeStartStop?           ISendAuthorizeStartStop      = null,
                                IReceiveAuthorizeStartStop?        IReceiveAuthorizeStartStop   = null,

                                DateTimeOffset?                    CachedResultEndOfLifeTime    = null,
                                ChargingSession_Id?                SessionId                    = null,
                                ChargingSession_Id?                EMPPartnerSessionId          = null,
                                AuthorizationReference?            AuthorizationReference       = null,
                                String?                            ContractId                   = null,
                                String?                            PrintedNumber                = null,
                                Languages?                         UILanguage                   = null,
                                DateTimeOffset?                    ExpiryDate                   = null,
                                Watt?                              MaxPower                     = null,
                                WattHour?                          MaxEnergy                    = null,
                                TimeSpan?                          MaxDuration                  = null,
                                IEnumerable<ChargingTariff>?       ChargingTariffs              = null,
                                IEnumerable<AuthenticationToken>?  ListOfAuthStopTokens         = null,
                                IEnumerable<UInt32>?               ListOfAuthStopPINs           = null,

                                EMobilityProvider_Id?              ProviderId                   = null,
                                String?                            ProviderName                 = null,
                                I18NString?                        Description                  = null,
                                I18NString?                        AdditionalInfo               = null,
                                JObject?                           AdditionalContext            = null,

                                Byte                               NumberOfRetries              = 0)

        {

            this.AuthorizatorId              = AuthorizatorId       ?? throw new ArgumentNullException(nameof(AuthorizatorId), "The given identification of the authorizator must not be null!");
            this.ResponseTimestamp           = ResponseTimestamp;
            this.Result                      = Result;
            this.Runtime                     = Runtime;

            this.ISendAuthorizeStartStop     = ISendAuthorizeStartStop;
            this.IReceiveAuthorizeStartStop  = IReceiveAuthorizeStartStop;

            this.CachedResultEndOfLifeTime   = CachedResultEndOfLifeTime;
            this.SessionId                   = SessionId;
            this.EMPPartnerSessionId         = EMPPartnerSessionId;
            this.AuthorizationReference      = AuthorizationReference;
            this.ContractId                  = ContractId;
            this.PrintedNumber               = PrintedNumber;
            this.UILanguage                  = UILanguage;
            this.ExpiryDate                  = ExpiryDate;
            this.MaxPower                    = MaxPower;
            this.MaxEnergy                   = MaxEnergy;
            this.MaxDuration                 = MaxDuration;
            this.ChargingTariffs             = ChargingTariffs      ?? [];
            this.ListOfAuthStopTokens        = ListOfAuthStopTokens ?? [];
            this.ListOfAuthStopPINs          = ListOfAuthStopPINs   ?? [];

            this.ProviderId                  = ProviderId           ?? new EMobilityProvider_Id?();
            this.ProviderName                = ProviderName;
            this.Description                 = Description          ?? I18NString.Empty;
            this.AdditionalInfo              = AdditionalInfo       ?? I18NString.Empty;
            this.AdditionalContext           = AdditionalContext;

            this.NumberOfRetries             = NumberOfRetries;

        }

        #endregion

        #region (public)  AuthStartResult (AuthorizatorId, ISendAuthorizeStartStop,    ResponseTimestamp, Result, Runtime, ...)

        /// <summary>
        /// Create a new AuthorizeStart result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Result">The AuthorizeStart result type.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="AuthorizationReference">An optional authorization reference.</param>
        /// <param name="ContractId">An optional contract identification.</param>
        /// <param name="PrintedNumber">An optional printed number.</param>
        /// <param name="UILanguage">An optional user interface language to be used for further interactions with the user.</param>
        /// <param name="ExpiryDate">The timestamp when this authorization expires.</param>
        /// <param name="MaxPower">The optional maximum allowed charging current.</param>
        /// <param name="MaxEnergy">The optional maximum allowed charging energy.</param>
        /// <param name="MaxDuration">The optional maximum allowed charging duration.</param>
        /// <param name="ChargingTariffs">Optional charging tariff information.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="ProviderName">An optional e-mobility provider name.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="AdditionalContext">Optional additional context information.</param>
        /// 
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public AuthStartResult(IId                                AuthorizatorId,
                               ISendAuthorizeStartStop            ISendAuthorizeStartStop,
                               DateTimeOffset                     ResponseTimestamp,
                               AuthStartResultTypes               Result,
                               TimeSpan                           Runtime,

                               DateTimeOffset?                    CachedResultEndOfLifeTime   = null,
                               ChargingSession_Id?                SessionId                   = null,
                               ChargingSession_Id?                EMPPartnerSessionId         = null,
                               AuthorizationReference?            AuthorizationReference      = null,
                               String?                            ContractId                  = null,
                               String?                            PrintedNumber               = null,
                               Languages?                         UILanguage                  = null,
                               DateTimeOffset?                    ExpiryDate                  = null,
                               Watt?                              MaxPower                    = null,
                               WattHour?                          MaxEnergy                   = null,
                               TimeSpan?                          MaxDuration                 = null,
                               IEnumerable<ChargingTariff>?       ChargingTariffs             = null,
                               IEnumerable<AuthenticationToken>?  ListOfAuthStopTokens        = null,
                               IEnumerable<UInt32>?               ListOfAuthStopPINs          = null,

                               EMobilityProvider_Id?              ProviderId                  = null,
                               String?                            ProviderName                = null,
                               I18NString?                        Description                 = null,
                               I18NString?                        AdditionalInfo              = null,
                               JObject?                           AdditionalContext           = null,

                               Byte                               NumberOfRetries             = 0)

            : this (AuthorizatorId,
                    ResponseTimestamp,
                    Result,
                    Runtime,

                    ISendAuthorizeStartStop,
                    null,

                    CachedResultEndOfLifeTime,
                    SessionId,
                    EMPPartnerSessionId,
                    AuthorizationReference,
                    ContractId,
                    PrintedNumber,
                    UILanguage,
                    ExpiryDate,
                    MaxPower,
                    MaxEnergy,
                    MaxDuration,
                    ChargingTariffs,
                    ListOfAuthStopTokens,
                    ListOfAuthStopPINs,

                    ProviderId,
                    ProviderName,
                    Description,
                    AdditionalInfo,
                    AdditionalContext,

                    NumberOfRetries)

        { }

        #endregion

        #region (public)  AuthStartResult (AuthorizatorId, IReceiveAuthorizeStartStop, ResponseTimestamp, Result, Runtime, ...)

        /// <summary>
        /// Create a new AuthorizeStart result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Result">The AuthorizeStart result type.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="AuthorizationReference">An optional authorization reference.</param>
        /// <param name="ContractId">An optional contract identification.</param>
        /// <param name="PrintedNumber">An optional printed number.</param>
        /// <param name="UILanguage">An optional user interface language to be used for further interactions with the user.</param>
        /// <param name="ExpiryDate">The timestamp when this authorization expires.</param>
        /// <param name="MaxPower">The optional maximum allowed charging current.</param>
        /// <param name="MaxEnergy">The optional maximum allowed charging energy.</param>
        /// <param name="MaxDuration">The optional maximum allowed charging duration.</param>
        /// <param name="ChargingTariffs">Optional charging tariff information.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="ProviderName">An optional e-mobility provider name.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="AdditionalContext">Optional additional context information.</param>
        /// 
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public AuthStartResult(IId                                AuthorizatorId,
                               IReceiveAuthorizeStartStop         IReceiveAuthorizeStartStop,
                               DateTimeOffset                     ResponseTimestamp,
                               AuthStartResultTypes               Result,
                               TimeSpan                           Runtime,

                               DateTimeOffset?                    CachedResultEndOfLifeTime   = null,
                               ChargingSession_Id?                SessionId                   = null,
                               ChargingSession_Id?                EMPPartnerSessionId         = null,
                               AuthorizationReference?            AuthorizationReference      = null,
                               String?                            ContractId                  = null,
                               String?                            PrintedNumber               = null,
                               Languages?                         UILanguage                  = null,
                               DateTimeOffset?                    ExpiryDate                  = null,
                               Watt?                              MaxPower                    = null,
                               WattHour?                          MaxEnergy                   = null,
                               TimeSpan?                          MaxDuration                 = null,
                               IEnumerable<ChargingTariff>?       ChargingTariffs             = null,
                               IEnumerable<AuthenticationToken>?  ListOfAuthStopTokens        = null,
                               IEnumerable<UInt32>?               ListOfAuthStopPINs          = null,

                               EMobilityProvider_Id?              ProviderId                  = null,
                               String?                            ProviderName                = null,
                               I18NString?                        Description                 = null,
                               I18NString?                        AdditionalInfo              = null,
                               JObject?                           AdditionalContext           = null,

                               Byte                               NumberOfRetries             = 0)

            : this (AuthorizatorId,
                    ResponseTimestamp,
                    Result,
                    Runtime,
                    null,
                    IReceiveAuthorizeStartStop,
                    CachedResultEndOfLifeTime,

                    SessionId,
                    EMPPartnerSessionId,
                    AuthorizationReference,
                    ContractId,
                    PrintedNumber,
                    UILanguage,
                    ExpiryDate,
                    MaxPower,
                    MaxEnergy,
                    MaxDuration,
                    ChargingTariffs,
                    ListOfAuthStopTokens,
                    ListOfAuthStopPINs,

                    ProviderId,
                    ProviderName,
                    Description,
                    AdditionalInfo,

                    AdditionalContext,

                    NumberOfRetries)

        {

            this.IReceiveAuthorizeStartStop = IReceiveAuthorizeStartStop;

        }

        #endregion

        #endregion


        #region (static) Parse    (JSON, CustomAuthStartResultParser = null)

        /// <summary>
        /// Parse the given JSON representation of an AuthStartResult.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomAuthStartResultParser">A delegate to parse custom AuthStartResult JSON objects.</param>
        public static AuthStartResult Parse(JObject                                        JSON,
                                            ISendAuthorizeStartStop?                       ISendAuthorizeStartStop       = null,
                                            IReceiveAuthorizeStartStop?                    IReceiveAuthorizeStartStop    = null,
                                            CustomJObjectParserDelegate<AuthStartResult>?  CustomAuthStartResultParser   = null)
        {

            if (TryParse(JSON,
                         out var authStartResult,
                         out var errorResponse,
                         ISendAuthorizeStartStop,
                         IReceiveAuthorizeStartStop,
                         CustomAuthStartResultParser))
            {
                return authStartResult;
            }

            throw new ArgumentException("The given JSON representation of an AuthStartResult is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out AuthStartResult, out ErrorResponse, CustomAuthStartResultParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an AuthStartResult.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="AuthStartResult">The parsed AuthStartResult.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                    JSON,
                                       [NotNullWhen(true)]  out AuthStartResult?  AuthStartResult,
                                       [NotNullWhen(false)] out String?           ErrorResponse,
                                       ISendAuthorizeStartStop?                   ISendAuthorizeStartStop      = null,
                                       IReceiveAuthorizeStartStop?                IReceiveAuthorizeStartStop   = null)

            => TryParse(JSON,
                        out AuthStartResult,
                        out ErrorResponse,
                        ISendAuthorizeStartStop,
                        IReceiveAuthorizeStartStop,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an AuthStartResult.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="AuthStartResult">The parsed AuthStartResult.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomAuthStartResultParser">A delegate to parse custom AuthStartResult JSON objects.</param>
        public static Boolean TryParse(JObject                                        JSON,
                                       [NotNullWhen(true)]  out AuthStartResult?      AuthStartResult,
                                       [NotNullWhen(false)] out String?               ErrorResponse,
                                       ISendAuthorizeStartStop?                       ISendAuthorizeStartStop       = null,
                                       IReceiveAuthorizeStartStop?                    IReceiveAuthorizeStartStop    = null,
                                       CustomJObjectParserDelegate<AuthStartResult>?  CustomAuthStartResultParser   = null)
        {

            try
            {

                AuthStartResult = default;

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
                                             out AuthStartResultTypes result,
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


                AuthStartResult = new AuthStartResult(

                                      authorizatorId,
                                      responseTimestamp,
                                      result,
                                      runtime,
                                      ISendAuthorizeStartStop,
                                      IReceiveAuthorizeStartStop,

                                      cachedResultEndOfLifeTime,
                                      sessionId,
                                      empPartnerSessionId,
                                      authorizationReference,
                                      contractId,
                                      printedNumber,
                                      uiLanguage,
                                      expiryDate,
                                      maxPower,
                                      maxEnergy,
                                      maxDuration,
                                      null, //chargingTariffs,
                                      null, //listOfAuthStopTokens,
                                      null, //listOfAuthStopPINs,

                                      providerId,
                                      providerName,
                                      description,
                                      additionalInfo,
                                      additionalContext,

                                      numberOfRetries

                                  );

                if (CustomAuthStartResultParser is not null)
                    AuthStartResult = CustomAuthStartResultParser(JSON,
                                                                  AuthStartResult);

                return true;

            }
            catch (Exception e)
            {
                AuthStartResult  = default;
                ErrorResponse    = "The given JSON representation of an AuthStartResult is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(Embedded = false, CustomAuthStartResultSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        /// <param name="CustomAuthStartResultSerializer">A delegate to serialize custom AuthStartResult JSON objects.</param>
        public JObject ToJSON(Boolean                                            Embedded                          = false,
                              CustomJObjectSerializerDelegate<AuthStartResult>?  CustomAuthStartResultSerializer   = null,
                              InfoStatus                                         ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandEVSEIds                     = InfoStatus.Expanded,
                              InfoStatus                                         ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandDataLicenses                = InfoStatus.ShowIdOnly)
        {

            var json = JSONObject.Create(

                           !Embedded
                               ? new JProperty("@context",                    JSONLDContext)
                               : null,

                                 new JProperty("authorizatorId",              AuthorizatorId.                 ToString()),
                                 new JProperty("timestamp",                   ResponseTimestamp.              ToISO8601()),
                                 new JProperty("result",                      Result.                         ToString()),
                                 new JProperty("runtime",                     Runtime.TotalMilliseconds),


                           CachedResultEndOfLifeTime.HasValue
                               ? new JProperty("cachedResultEndOfLifeTime",   CachedResultEndOfLifeTime.Value.ToISO8601())
                               : null,

                           SessionId.                HasValue
                               ? new JProperty("sessionId",                   SessionId.                Value.ToString())
                               : null,

                           EMPPartnerSessionId.      HasValue
                               ? new JProperty("empPartnerSessionId",         EMPPartnerSessionId.      Value.ToString())
                               : null,

                           AuthorizationReference.   HasValue
                               ? new JProperty("authorizationReference",      AuthorizationReference.   Value.ToString())
                               : null,

                           ContractId.    IsNotNullOrEmpty()
                               ? new JProperty("contractId",                  ContractId.                     ToString())
                               : null,

                           PrintedNumber. IsNotNullOrEmpty()
                               ? new JProperty("printedNumber",               PrintedNumber.                  ToString())
                               : null,

                           UILanguage.               HasValue
                               ? new JProperty("uiLanguage",                  UILanguage.               Value.ToString())
                               : null,

                           ExpiryDate.               HasValue
                               ? new JProperty("expiryDate",                  ExpiryDate.               Value.ToString())
                               : null,

                           MaxPower.                 HasValue
                               ? new JProperty("maxPower",                    MaxPower.                 Value.Value)
                               : null,

                           MaxEnergy.                HasValue
                               ? new JProperty("maxEnergy",                   MaxEnergy.                Value.Value)
                               : null,

                           MaxDuration.              HasValue
                               ? new JProperty("maxDuration",                 MaxDuration.              Value.TotalMinutes)
                               : null,


                           ChargingTariffs.     Any()
                               ? new JProperty("chargingTariffs",             new JArray(ChargingTariffs.     Select(chargingTariff      => chargingTariff.ToJSON(
                                                                                                                                                true,
                                                                                                                                                ExpandRoamingNetworkId,
                                                                                                                                                ExpandChargingStationOperatorId,
                                                                                                                                                ExpandChargingPoolId,
                                                                                                                                                ExpandEVSEIds,
                                                                                                                                                ExpandBrandIds,
                                                                                                                                                ExpandDataLicenses
                                                                                                                                            ))))
                               : null,

                           ListOfAuthStopTokens.Any()
                               ? new JProperty("listOfAuthStopTokens",        new JArray(ListOfAuthStopTokens.Select(authenticationToken => authenticationToken.ToString())))
                               : null,

                           ListOfAuthStopPINs.Any()
                               ? new JProperty("listOfAuthStopPINs",          new JArray(ListOfAuthStopPINs.  Select(pin                 => pin)))
                               : null,


                           ProviderId.HasValue
                               ? new JProperty("providerId",                  ProviderId.                     ToString())
                               : null,

                           ProviderName.  IsNotNullOrEmpty()
                               ? new JProperty("providerName",                ProviderName)
                               : null,

                           Description.   IsNotNullOrEmpty()
                               ? new JProperty("description",                 Description.                    ToJSON())
                               : null,

                           AdditionalInfo.IsNotNullOrEmpty()
                               ? new JProperty("additionalInfo",              AdditionalInfo.                 ToJSON())
                               : null,

                           AdditionalContext is not null
                               ? new JProperty("additionalContext",           AdditionalContext)
                               : null,


                                 new JProperty("numberOfRetries",             NumberOfRetries)

                       );

            return CustomAuthStartResultSerializer is not null
                       ? CustomAuthStartResultSerializer(this, json)
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
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            Unspecified(IId                      AuthorizatorId,
                        ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                        DateTimeOffset           ResponseTimestamp,
                        TimeSpan                 Runtime,

                        ChargingSession_Id?      SessionId     = null,
                        I18NString?              Description   = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Unspecified,
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
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// 
        public static AuthStartResult

            Unspecified(IId                         AuthorizatorId,
                        IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                        DateTimeOffset              ResponseTimestamp,
                        TimeSpan                    Runtime,

                        ChargingSession_Id?         SessionId     = null,
                        I18NString?                 Description   = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Unspecified,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description);

        #endregion

        #region (static) AdminDown            (AuthorizatorId, ResponseTimestamp, Runtime, SessionId = null, Description = null)

        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            AdminDown(IId                      AuthorizatorId,
                      ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                      DateTimeOffset           ResponseTimestamp,
                      TimeSpan                 Runtime,

                      ChargingSession_Id?      SessionId     = null,
                      I18NString?              Description   = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.AdminDown,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("The authentication service was disabled by the administrator!"));



        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            AdminDown(IId                         AuthorizatorId,
                      IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                      DateTimeOffset              ResponseTimestamp,
                      TimeSpan                    Runtime,

                      ChargingSession_Id?         SessionId     = null,
                      I18NString?                 Description   = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.AdminDown,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("The authentication service was disabled by the administrator!"));

        #endregion

        #region (static) UnknownLocation      (AuthorizatorId, ResponseTimestamp, Runtime, ...)

        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            UnknownLocation(IId                      AuthorizatorId,
                            ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                            DateTimeOffset           ResponseTimestamp,
                            TimeSpan                 Runtime,

                            DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                            ChargingSession_Id?      SessionId                   = null,
                            I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.UnknownLocation,
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
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            UnknownLocation(IId                         AuthorizatorId,
                            IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                            DateTimeOffset              ResponseTimestamp,
                            TimeSpan                    Runtime,

                            DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                            ChargingSession_Id?         SessionId                   = null,
                            I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.UnknownLocation,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Unknown location!"));

        #endregion

        #region (static) InvalidToken         (AuthorizatorId, ResponseTimestamp, Runtime, ...)

        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            InvalidToken(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         DateTimeOffset           ResponseTimestamp,
                         TimeSpan                 Runtime,

                         DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                         ChargingSession_Id?      SessionId                   = null,
                         I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.InvalidToken,
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
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            InvalidToken(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         DateTimeOffset              ResponseTimestamp,
                         TimeSpan                    Runtime,

                         DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                         ChargingSession_Id?         SessionId                   = null,
                         I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.InvalidToken,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid token!"));

        #endregion

        #region (static) InvalidSessionId     (AuthorizatorId, ResponseTimestamp, Runtime, ...)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>        /// 
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            InvalidSessionId(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             DateTimeOffset           ResponseTimestamp,
                             TimeSpan                 Runtime,

                             DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                             ChargingSession_Id?      SessionId                   = null,
                             I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.InvalidSessionId,
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
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            InvalidSessionId(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             DateTimeOffset              ResponseTimestamp,
                             TimeSpan                    Runtime,

                             DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                             ChargingSession_Id?         SessionId                   = null,
                             I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.InvalidSessionId,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Invalid session identification!"));

        #endregion

        #region (static) Reserved             (AuthorizatorId, ResponseTimestamp, Runtime, ...)

        /// <summary>
        /// The EVSE is already reserved.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            Reserved(IId                      AuthorizatorId,
                     ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                     DateTimeOffset           ResponseTimestamp,
                     TimeSpan                 Runtime,

                     DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                     ChargingSession_Id?      SessionId                   = null,
                     I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Reserved,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Reserved!"));



        /// <summary>
        /// The EVSE is already reserved.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            Reserved(IId                         AuthorizatorId,
                     IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                     DateTimeOffset              ResponseTimestamp,
                     TimeSpan                    Runtime,

                     DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                     ChargingSession_Id?         SessionId                   = null,
                     I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Reserved,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Reserved!"));

        #endregion

        #region (static) OutOfService         (AuthorizatorId, ResponseTimestamp, Runtime, ...)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            OutOfService(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         DateTimeOffset           ResponseTimestamp,
                         TimeSpan                 Runtime,

                         DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                         ChargingSession_Id?      SessionId                   = null,
                         I18NString?              Description                 = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.OutOfService,
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
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            OutOfService(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         DateTimeOffset              ResponseTimestamp,
                         TimeSpan                    Runtime,

                         DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                         ChargingSession_Id?         SessionId                   = null,
                         I18NString?                 Description                 = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.OutOfService,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        Description:  Description ?? I18NString.Create("Out of service!"));

        #endregion

        #region (static) Authorized           (AuthorizatorId, ResponseTimestamp, Runtime, ...)

        /// <summary>
        /// The AuthorizeStart was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="AuthorizationReference">An optional authorization reference.</param>
        /// <param name="ContractId">An optional contract identification.</param>
        /// <param name="PrintedNumber">An optional printed number.</param>
        /// <param name="UILanguage">An optional user interface language to be used for further interactions with the user.</param>
        /// <param name="ExpiryDate">The timestamp when this authorization expires.</param>
        /// <param name="MaxPower">The optional maximum allowed charging current.</param>
        /// <param name="MaxEnergy">The optional maximum allowed charging energy.</param>
        /// <param name="MaxDuration">The optional maximum allowed charging duration.</param>
        /// <param name="ChargingTariffs">Optional charging tariff information.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        /// 
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="ProviderName">An optional e-mobility provider name.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="AdditionalContext">Optional additional context information.</param>
        /// 
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public static AuthStartResult

            Authorized(IId                                AuthorizatorId,
                       ISendAuthorizeStartStop            ISendAuthorizeStartStop,
                       DateTimeOffset                     ResponseTimestamp,
                       TimeSpan                           Runtime,

                       DateTimeOffset?                    CachedResultEndOfLifeTime   = null,
                       ChargingSession_Id?                SessionId                   = null,
                       ChargingSession_Id?                EMPPartnerSessionId         = null,
                       AuthorizationReference?            AuthorizationReference      = null,
                       String?                            ContractId                  = null,
                       String?                            PrintedNumber               = null,
                       Languages?                         UILanguage                  = null,
                       DateTimeOffset?                    ExpiryDate                  = null,
                       Watt?                              MaxPower                    = null,
                       WattHour?                          MaxEnergy                   = null,
                       TimeSpan?                          MaxDuration                 = null,
                       IEnumerable<ChargingTariff>?       ChargingTariffs             = null,
                       IEnumerable<AuthenticationToken>?  ListOfAuthStopTokens        = null,
                       IEnumerable<UInt32>?               ListOfAuthStopPINs          = null,

                       EMobilityProvider_Id?              ProviderId                  = null,
                       String?                            ProviderName                = null,
                       I18NString?                        Description                 = null,
                       I18NString?                        AdditionalInfo              = null,
                       JObject?                           AdditionalContext           = null,

                       Byte                               NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Authorized,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        EMPPartnerSessionId,
                        AuthorizationReference,
                        ContractId,
                        PrintedNumber,
                        UILanguage,
                        ExpiryDate,
                        MaxPower,
                        MaxEnergy,
                        MaxDuration,
                        ChargingTariffs,
                        ListOfAuthStopTokens,
                        ListOfAuthStopPINs,

                        ProviderId,
                        ProviderName,
                        Description ?? I18NString.Create("Success!"),
                        AdditionalInfo,

                        AdditionalContext,

                        NumberOfRetries);



        /// <summary>
        /// The AuthorizeStart was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification, when the AuthorizeStart operation was successful.</param>
        /// <param name="AuthorizationReference">An optional authorization reference.</param>
        /// <param name="ContractId">An optional contract identification.</param>
        /// <param name="PrintedNumber">An optional printed number.</param>
        /// <param name="UILanguage">An optional user interface language to be used for further interactions with the user.</param>
        /// <param name="ExpiryDate">The timestamp when this authorization expires.</param>
        /// <param name="MaxPower">The optional maximum allowed charging current.</param>
        /// <param name="MaxEnergy">The optional maximum allowed charging energy.</param>
        /// <param name="MaxDuration">The optional maximum allowed charging duration.</param>
        /// <param name="ChargingTariffs">Optional charging tariff information.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        /// 
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="ProviderName">An optional e-mobility provider name.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="AdditionalContext">Optional additional context information.</param>
        /// 
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public static AuthStartResult

            Authorized(IId                                AuthorizatorId,
                       IReceiveAuthorizeStartStop         IReceiveAuthorizeStartStop,
                       DateTimeOffset                     ResponseTimestamp,
                       TimeSpan                           Runtime,

                       DateTimeOffset?                    CachedResultEndOfLifeTime   = null,
                       ChargingSession_Id?                SessionId                   = null,
                       ChargingSession_Id?                EMPPartnerSessionId         = null,
                       AuthorizationReference?            AuthorizationReference      = null,
                       String?                            ContractId                  = null,
                       String?                            PrintedNumber               = null,
                       Languages?                         UILanguage                  = null,
                       DateTimeOffset?                    ExpiryDate                  = null,
                       Watt?                              MaxPower                    = null,
                       WattHour?                          MaxEnergy                   = null,
                       TimeSpan?                          MaxDuration                 = null,
                       IEnumerable<ChargingTariff>?       ChargingTariffs             = null,
                       IEnumerable<AuthenticationToken>?  ListOfAuthStopTokens        = null,
                       IEnumerable<UInt32>?               ListOfAuthStopPINs          = null,

                       EMobilityProvider_Id?              ProviderId                  = null,
                       String?                            ProviderName                = null,
                       I18NString?                        Description                 = null,
                       I18NString?                        AdditionalInfo              = null,
                       JObject?                           AdditionalContext           = null,

                       Byte                               NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Authorized,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        EMPPartnerSessionId,
                        AuthorizationReference,
                        ContractId,
                        PrintedNumber,
                        UILanguage,
                        ExpiryDate,
                        MaxPower,
                        MaxEnergy,
                        MaxDuration,
                        ChargingTariffs,
                        ListOfAuthStopTokens,
                        ListOfAuthStopPINs,

                        ProviderId,
                        ProviderName,
                        Description ?? I18NString.Create("Success!"),
                        AdditionalInfo,

                        AdditionalContext,

                        NumberOfRetries);

        #endregion

        #region (static) NotAuthorized        (AuthorizatorId, ResponseTimestamp, Runtime, ...)

        /// <summary>
        /// The AuthorizeStart was not successful (e.g. ev customer is unknown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public static AuthStartResult

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


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.NotAuthorized,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        ProviderId,
                        ProviderName,
                        Description ?? I18NString.Create("Not authorized!"),
                        AdditionalInfo,
                        AdditionalContext,
                        NumberOfRetries);



        /// <summary>
        /// The AuthorizeStart was not successful (e.g. ev customer is unknown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public static AuthStartResult

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


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.NotAuthorized,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        ProviderId,
                        ProviderName,
                        Description ?? I18NString.Create("Not authorized!"),
                        AdditionalInfo,
                        AdditionalContext,

                        NumberOfRetries);

        #endregion

        #region (static) Blocked              (AuthorizatorId, ResponseTimestamp, Runtime, ...)

        /// <summary>
        /// The AuthorizeStart operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public static AuthStartResult

            Blocked(IId                      AuthorizatorId,
                    ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                    DateTimeOffset           ResponseTimestamp,
                    TimeSpan                 Runtime,

                    DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                    ChargingSession_Id?      SessionId                   = null,
                    EMobilityProvider_Id?    ProviderId                  = null,
                    I18NString?              Description                 = null,
                    I18NString?              AdditionalInfo              = null,
                    Byte                     NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Blocked,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId:       ProviderId,
                        Description:      Description ?? I18NString.Create("Blocked!"),
                        AdditionalInfo:   AdditionalInfo,
                        NumberOfRetries:  NumberOfRetries);



        /// <summary>
        /// The AuthorizeStart operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public static AuthStartResult

            Blocked(IId                         AuthorizatorId,
                    IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                    DateTimeOffset              ResponseTimestamp,
                    TimeSpan                    Runtime,

                    DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                    ChargingSession_Id?         SessionId                   = null,
                    EMobilityProvider_Id?       ProviderId                  = null,
                    I18NString?                 Description                 = null,
                    I18NString?                 AdditionalInfo              = null,
                    Byte                        NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Blocked,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId:       ProviderId,
                        Description:      Description ?? I18NString.Create("Blocked!"),
                        AdditionalInfo:   AdditionalInfo,
                        NumberOfRetries:  NumberOfRetries);

        #endregion

        #region (static) Expired              (AuthorizatorId, ResponseTimestamp, Runtime, ...)

        /// <summary>
        /// The AuthorizeStart operation is not allowed (ev customer contract is expired).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            Expired(IId                      AuthorizatorId,
                    ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                    DateTimeOffset           ResponseTimestamp,
                    TimeSpan                 Runtime,

                    DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                    ChargingSession_Id?      SessionId                   = null,
                    EMobilityProvider_Id?    ProviderId                  = null,
                    I18NString?              Description                 = null,
                    I18NString?              AdditionalInfo              = null,
                    Byte                     NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Expired,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId:       ProviderId,
                        Description:      Description ?? I18NString.Create("Expired!"),
                        AdditionalInfo:   AdditionalInfo,
                        NumberOfRetries:  NumberOfRetries);



        /// <summary>
        /// The AuthorizeStart operation is not allowed (ev customer contract is expired).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            Expired(IId                         AuthorizatorId,
                    IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                    DateTimeOffset              ResponseTimestamp,
                    TimeSpan                    Runtime,

                    DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                    ChargingSession_Id?         SessionId                   = null,
                    EMobilityProvider_Id?       ProviderId                  = null,
                    I18NString?                 Description                 = null,
                    I18NString?                 AdditionalInfo              = null,
                    Byte                        NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Expired,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId:       ProviderId,
                        Description:      Description ?? I18NString.Create("Expired!"),
                        AdditionalInfo:   AdditionalInfo,
                        NumberOfRetries:  NumberOfRetries);

        #endregion

        #region (static) NoCredit             (AuthorizatorId, ResponseTimestamp, Runtime, ...)

        /// <summary>
        /// The AuthorizeStart operation is not allowed (ev customer has no credit).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            NoCredit(IId                      AuthorizatorId,
                     ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                     DateTimeOffset           ResponseTimestamp,
                     TimeSpan                 Runtime,

                     DateTimeOffset?          CachedResultEndOfLifeTime   = null,
                     ChargingSession_Id?      SessionId                   = null,
                     EMobilityProvider_Id?    ProviderId                  = null,
                     I18NString?              Description                 = null,
                     I18NString?              AdditionalInfo              = null,
                     Byte                     NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.NoCredit,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId:       ProviderId,
                        Description:      Description ?? I18NString.Create("No credit!"),
                        AdditionalInfo:   AdditionalInfo,
                        NumberOfRetries:  NumberOfRetries);



        /// <summary>
        /// The AuthorizeStart operation is not allowed (ev customer has no credit).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="CachedResultEndOfLifeTime">An optional timestamp until the result may be cached.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        public static AuthStartResult

            NoCredit(IId                         AuthorizatorId,
                     IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                     DateTimeOffset              ResponseTimestamp,
                     TimeSpan                    Runtime,

                     DateTimeOffset?             CachedResultEndOfLifeTime   = null,
                     ChargingSession_Id?         SessionId                   = null,
                     EMobilityProvider_Id?       ProviderId                  = null,
                     I18NString?                 Description                 = null,
                     I18NString?                 AdditionalInfo              = null,
                     Byte                        NumberOfRetries             = 0)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.NoCredit,
                        Runtime,

                        CachedResultEndOfLifeTime,
                        SessionId,
                        ProviderId:       ProviderId,
                        Description:      Description ?? I18NString.Create("No credit!"),
                        AdditionalInfo:   AdditionalInfo,
                        NumberOfRetries:  NumberOfRetries);

        #endregion

        #region (static) CommunicationTimeout (AuthorizatorId, ResponseTimestamp, Runtime, SessionId = null, Description = null)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            CommunicationTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 DateTimeOffset           ResponseTimestamp,
                                 TimeSpan                 Runtime,

                                 ChargingSession_Id?      SessionId           = null,
                                 I18NString?              Description         = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.CommunicationTimeout,
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
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            CommunicationTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 DateTimeOffset              ResponseTimestamp,
                                 TimeSpan                    Runtime,

                                 ChargingSession_Id?         SessionId           = null,
                                 I18NString?                 Description         = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.CommunicationTimeout,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Communication timeout!"));

        #endregion

        #region (static) StartChargingTimeout (AuthorizatorId, ResponseTimestamp, Runtime, SessionId = null, Description = null)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            StartChargingTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 DateTimeOffset           ResponseTimestamp,
                                 TimeSpan                 Runtime,

                                 ChargingSession_Id?      SessionId           = null,
                                 I18NString?              Description         = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.StartChargingTimeout,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Start charging timeout!"));



        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            StartChargingTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 DateTimeOffset              ResponseTimestamp,
                                 TimeSpan                    Runtime,

                                 ChargingSession_Id?         SessionId           = null,
                                 I18NString?                 Description         = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.StartChargingTimeout,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Start charging timeout!"));

        #endregion

        #region (static) RateLimitReached     (AuthorizatorId, ResponseTimestamp, Runtime, SessionId = null, Description = null)

        /// <summary>
        /// The AuthorizeStart operation reached a rate limit.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            RateLimitReached(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             DateTimeOffset           ResponseTimestamp,
                             TimeSpan                 Runtime,

                             ChargingSession_Id?      SessionId           = null,
                             I18NString?              Description         = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.RateLimitReached,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Rate limit reached!"));



        /// <summary>
        /// The AuthorizeStart operation reached a rate limit.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            RateLimitReached(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             DateTimeOffset              ResponseTimestamp,
                             TimeSpan                    Runtime,

                             ChargingSession_Id?         SessionId           = null,
                             I18NString?                 Description         = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.RateLimitReached,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("RateLimitReached!"));

        #endregion

        #region (static) Error                (AuthorizatorId, ResponseTimestamp, Runtime, SessionId = null, Description = null)

        /// <summary>
        /// The AuthorizeStart operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            Error(IId                      AuthorizatorId,
                  ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                  DateTimeOffset           ResponseTimestamp,
                  TimeSpan                 Runtime,

                  ChargingSession_Id?      SessionId           = null,
                  I18NString?              Description         = null)


                => new (AuthorizatorId,
                        ISendAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Error,
                        Runtime,

                        SessionId:    SessionId,
                        Description:  Description ?? I18NString.Create("Error!"));



        /// <summary>
        /// The AuthorizeStart operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        public static AuthStartResult

            Error(IId                         AuthorizatorId,
                  IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                  DateTimeOffset              ResponseTimestamp,
                  TimeSpan                    Runtime,

                  ChargingSession_Id?         SessionId           = null,
                  I18NString?                 Description         = null)


                => new (AuthorizatorId,
                        IReceiveAuthorizeStartStop,
                        ResponseTimestamp,
                        AuthStartResultTypes.Error,
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

            => $"{Result}{(ProviderId is not null
                               ? $" ({ProviderId})"
                               : "")} via {AuthorizatorId}";

        #endregion

    }


    /// <summary>
    /// The result of an AuthorizeStart request.
    /// </summary>
    public enum AuthStartResultTypes
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
        /// The charging location is already reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// The charging location is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The AuthorizeStart was successful.
        /// </summary>
        Authorized,

        /// <summary>
        /// The AuthorizeStart was not successful (e.g. ev customer is unknown).
        /// </summary>
        NotAuthorized,

        /// <summary>
        /// The AuthorizeStart operation is not allowed (ev customer is blocked).
        /// </summary>
        Blocked,

        /// <summary>
        /// The AuthorizeStart operation is not allowed (ev customer contract is expired).
        /// </summary>
        Expired,

        /// <summary>
        /// The AuthorizeStart operation is not allowed (ev customer has no credit).
        /// </summary>
        NoCredit,

        /// <summary>
        /// The AuthorizeStart ran into a timeout between evse operator backend and the charging location.
        /// </summary>
        CommunicationTimeout,

        /// <summary>
        /// The AuthorizeStart ran into a timeout between the charging location and the EV.
        /// </summary>
        StartChargingTimeout,

        /// <summary>
        /// Rate Limit Reached
        /// </summary>
        RateLimitReached,

        /// <summary>
        /// The remote start operation led to an error.
        /// </summary>
        Error

    }

}
