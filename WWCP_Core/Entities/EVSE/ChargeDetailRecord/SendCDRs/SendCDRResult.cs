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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A SendCDR result.
    /// </summary>
    public class SendCDRResult
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext  = "https://open.charging.cloud/contexts/wwcp+json/sendCDRResult";

        #endregion

        #region Properties

        /// <summary>
        /// The timestamp of the charge detail record result.
        /// </summary>
        public DateTimeOffset        Timestamp             { get; }

        /// <summary>
        /// The identification of the charge detail record sending or receiving entity.
        /// </summary>
        public IId                   AuthorizatorId        { get; }

        /// <summary>
        /// The result of the SendCDR request.
        /// </summary>
        public SendCDRResultTypes    Result                { get; }

        /// <summary>
        /// The optional charge detail record.
        /// </summary>
        public ChargeDetailRecord?   ChargeDetailRecord    { get; }

        /// <summary>
        /// The optional multi-language description of the result.
        /// </summary>
        public I18NString            Description           { get; }

        /// <summary>
        /// Optional warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>  Warnings              { get; }

        /// <summary>
        /// The optional (absolute or relative) URL for locating the charge detail record as defined e.g. in OCPI.
        /// </summary>
        public Location?             Location              { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?             Runtime               { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new SendCDR result.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="Result">The result of the SendCDR request.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Location">An optional URL for locating the charge detail record as defined e.g. in OCPI.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private SendCDRResult(DateTimeOffset         Timestamp,
                              IId                    AuthorizatorId,
                              SendCDRResultTypes     Result,
                              ChargeDetailRecord?    ChargeDetailRecord   = null,
                              I18NString?            Description          = null,
                              IEnumerable<Warning>?  Warnings             = null,
                              Location?              Location             = null,
                              TimeSpan?              Runtime              = null)
        {

            this.Timestamp           = Timestamp;
            this.AuthorizatorId      = AuthorizatorId;
            this.Result              = Result;
            this.ChargeDetailRecord  = ChargeDetailRecord;
            this.Description         = Description ?? I18NString.Empty;
            this.Warnings            = Warnings is not null
                                           ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                           : [];
            this.Location            = Location;
            this.Runtime             = Runtime;

        }

        #endregion


        #region (static) Unspecified              (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Unspecified(DateTimeOffset         Timestamp,
                        IId                    AuthorizatorId,
                        ChargeDetailRecord?    ChargeDetailRecord   = null,
                        I18NString?            Description          = null,
                        IEnumerable<Warning>?  Warnings             = null,
                        TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.Unspecified,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) NoOperation              (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The request was not processed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            NoOperation(DateTimeOffset         Timestamp,
                        IId                    AuthorizatorId,
                        ChargeDetailRecord?    ChargeDetailRecord   = null,
                        I18NString?            Description          = null,
                        IEnumerable<Warning>?  Warnings             = null,
                        TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.NoOperation,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) AdminDown                (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The processing of charge detail records was disabled by an administrator.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            AdminDown(DateTimeOffset         Timestamp,
                      IId                    AuthorizatorId,
                      ChargeDetailRecord?    ChargeDetailRecord   = null,
                      I18NString?            Description          = null,
                      IEnumerable<Warning>?  Warnings             = null,
                      TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.AdminDown,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) OutOfService             (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The processing of charge detail records is out of service.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            OutOfService(DateTimeOffset         Timestamp,
                         IId                    AuthorizatorId,
                         ChargeDetailRecord?    ChargeDetailRecord   = null,
                         I18NString?            Description          = null,
                         IEnumerable<Warning>?  Warnings             = null,
                         TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.OutOfService,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) Filtered                 (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The charge detail record was filtered and will no longer be processed and/or forwarded.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Filtered(DateTimeOffset         Timestamp,
                     IId                    AuthorizatorId,
                     ChargeDetailRecord?    ChargeDetailRecord   = null,
                     I18NString?            Description          = null,
                     IEnumerable<Warning>?  Warnings             = null,
                     TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.Filtered,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) InvalidSessionId         (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The charging session identification of the charge detail record is invalid.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            InvalidSessionId(DateTimeOffset         Timestamp,
                             IId                    AuthorizatorId,
                             ChargeDetailRecord?    ChargeDetailRecord   = null,
                             I18NString?            Description          = null,
                             IEnumerable<Warning>?  Warnings             = null,
                             TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.InvalidSessionId,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) UnknownSessionId         (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The charging session identification of the charge detail record is unknown.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            UnknownSessionId(DateTimeOffset         Timestamp,
                             IId                    AuthorizatorId,
                             ChargeDetailRecord?    ChargeDetailRecord   = null,
                             I18NString?            Description          = null,
                             IEnumerable<Warning>?  Warnings             = null,
                             TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.UnknownSessionId,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) UnknownProviderIdStart   (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The e-mobility provider (start) of the charge detail record is unknown.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            UnknownProviderIdStart(DateTimeOffset         Timestamp,
                                   IId                    AuthorizatorId,
                                   ChargeDetailRecord?    ChargeDetailRecord   = null,
                                   I18NString?            Description          = null,
                                   IEnumerable<Warning>?  Warnings             = null,
                                   TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.UnknownProviderIdStart,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) UnknownProviderIdStop    (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The e-mobility provider (stop) of the charge detail record is unknown.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            UnknownProviderIdStop(DateTimeOffset         Timestamp,
                                  IId                    AuthorizatorId,
                                  ChargeDetailRecord?    ChargeDetailRecord   = null,
                                  I18NString?            Description          = null,
                                  IEnumerable<Warning>?  Warnings             = null,
                                  TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.UnknownProviderIdStop,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) UnknownLocation          (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The charging location is unknown.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            UnknownLocation(DateTimeOffset         Timestamp,
                            IId                    AuthorizatorId,
                            ChargeDetailRecord?    ChargeDetailRecord   = null,
                            I18NString?            Description          = null,
                            IEnumerable<Warning>?  Warnings             = null,
                            TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.UnknownLocation,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) InvalidToken             (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The authentication token of the charge detail record is invalid.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            InvalidToken(DateTimeOffset         Timestamp,
                         IId                    AuthorizatorId,
                         ChargeDetailRecord?    ChargeDetailRecord   = null,
                         I18NString?            Description          = null,
                         IEnumerable<Warning>?  Warnings             = null,
                         TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.InvalidToken,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) CouldNotConvertCDRFormat (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The charge detail record format could not be converted.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            CouldNotConvertCDRFormat(DateTimeOffset         Timestamp,
                                     IId                    AuthorizatorId,
                                     ChargeDetailRecord?    ChargeDetailRecord   = null,
                                     I18NString?            Description          = null,
                                     IEnumerable<Warning>?  Warnings             = null,
                                     TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.CouldNotConvertCDRFormat,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) Error                    (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The request led to an error.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Error(DateTimeOffset         Timestamp,
                  IId                    AuthorizatorId,
                  ChargeDetailRecord     ChargeDetailRecord,
                  I18NString?            Description   = null,
                  IEnumerable<Warning>?  Warnings      = null,
                  TimeSpan?              Runtime       = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.Error,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion

        #region (static) Timeout                  (Timestamp, AuthorizatorId, ChargeDetailRecord = null, ...)

        /// <summary>
        /// The request timed out.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Timeout(DateTimeOffset         Timestamp,
                    IId                    AuthorizatorId,
                    ChargeDetailRecord?    ChargeDetailRecord   = null,
                    I18NString?            Description          = null,
                    IEnumerable<Warning>?  Warnings             = null,
                    TimeSpan?              Runtime              = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.Timeout,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        null,
                        Runtime);

        #endregion


        #region (static) Enqueued                 (Timestamp, AuthorizatorId, ChargeDetailRecord, ...)

        /// <summary>
        /// The charge detail record had been enqueued for later processing and/or transmission.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Location">An optional URL for locating the charge detail record as defined e.g. in OCPI.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Enqueued(DateTimeOffset         Timestamp,
                     IId                    AuthorizatorId,
                     ChargeDetailRecord     ChargeDetailRecord,
                     I18NString?            Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     Location?              Location      = null,
                     TimeSpan?              Runtime       = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.Enqueued,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        Location,
                        Runtime);

        #endregion

        #region (static) Success                  (Timestamp, AuthorizatorId, ChargeDetailRecord, ...)

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Location">An optional URL for locating the charge detail record as defined e.g. in OCPI.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Success(DateTimeOffset         Timestamp,
                    IId                    AuthorizatorId,
                    ChargeDetailRecord     ChargeDetailRecord,
                    I18NString?            Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    Location?              Location      = null,
                    TimeSpan?              Runtime       = null)

                => new (Timestamp,
                        AuthorizatorId,
                        SendCDRResultTypes.Success,
                        ChargeDetailRecord,
                        Description,
                        Warnings,
                        Location,
                        Runtime);

        #endregion



        #region ToJSON(Embedded = false, IncludeCDR = true, ...)

        /// <summary>
        /// Return a JSON representation of the given charge detail record.
        /// </summary>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        /// <param name="IncludeCDR">Whether to include the embedded charge detail record.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A custom charge detail record serializer.</param>
        /// <param name="CustomSendCDRResultSerializer">A custom send charge detail record result serializer.</param>
        /// <param name="CustomWarningSerializer">A delegate to serialize custom warning JSON objects.</param>
        public JObject ToJSON(Boolean                                               Embedded                             = false,
                              Boolean                                               IncludeCDR                           = true,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordSerializer   = null,
                              CustomJObjectSerializerDelegate<SendCDRResult>?       CustomSendCDRResultSerializer        = null,
                              CustomJObjectSerializerDelegate<Warning>?             CustomWarningSerializer              = null)
        {

            var json = JSONObject.Create(

                           !Embedded
                               ? new JProperty("@context",             JSONLDContext)
                               : null,

                                 new JProperty("timestamp",            Timestamp.         ToISO8601()),
                                 new JProperty("authorizatorId",       AuthorizatorId.    ToString()),

                                 new JProperty("result",               Result.            ToString()),

                           Description.IsNotNullOrEmpty()
                               ? new JProperty("description",          Description.       ToJSON())
                               : null,

                           Warnings.Any()
                               ? new JProperty("warnings",             new JArray(Warnings.Select(warning => warning.ToJSON(CustomWarningSerializer))))
                               : null,

                           Location.HasValue
                               ? new JProperty("location",             Location.Value.    ToString())
                               : null,

                           Runtime.HasValue
                               ? new JProperty("runtime",              Runtime. Value.TotalMilliseconds)
                               : null,

                           ChargeDetailRecord is not null && IncludeCDR
                               ? new JProperty("chargeDetailRecord",   ChargeDetailRecord.ToJSON(Embedded:                            true,
                                                                                                 CustomChargeDetailRecordSerializer:  CustomChargeDetailRecordSerializer))
                               : null

                       );

            return CustomSendCDRResultSerializer is not null
                       ? CustomSendCDRResultSerializer(this, json)
                       : json;

        }

        #endregion

        #region (static) TryParse(JSONObject, ..., out SendCDRResult, out ErrorResponse, VerifyContext = false)

        //public static Boolean TryParse(JObject                                  JSONObject,
        //                               [NotNullWhen(true)]  out SendCDRResult?  SendCDRResult,
        //                               [NotNullWhen(false)] out String?         ErrorResponse)
        //    => TryParse(JSONObject,
        //                out SendCDRResult,
        //                out ErrorResponse);

        public static Boolean TryParse(JObject                                  JSONObject,
                                       [NotNullWhen(true)]  out SendCDRResult?  SendCDRResult,
                                       [NotNullWhen(false)] out String?         ErrorResponse,
                                       Boolean                                  VerifyContext = false)
        {

            try
            {

                SendCDRResult = default;

                if (JSONObject?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Context               [mandatory]

                if (VerifyContext)
                {

                    if (!JSONObject.ParseMandatoryText("@context",
                                                       "JSON-LD context",
                                                       out var Context,
                                                       out ErrorResponse))
                    {
                        ErrorResponse = @"The JSON-LD ""@context"" information is missing!";
                        return false;
                    }

                    if (Context != JSONLDContext)
                    {
                        ErrorResponse = @"The given JSON-LD ""@context"" information '" + Context + "' is not supported!";
                        return false;
                    }

                }

                #endregion

                #region Parse Timestamp             [mandatory]

                if (!JSONObject.ParseMandatory("timestamp",
                                               "timestamp",
                                               out DateTime Timestamp,
                                               out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse AuthorizatorId        [mandatory]

                if (!JSONObject.ParseMandatory("authorizatorId",
                                               "authorizator ídentification",
                                               Authorizator_Id.TryParse,
                                               out Authorizator_Id AuthorizatorId,
                                               out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Result                [mandatory]

                if (!JSONObject.ParseMandatoryEnum("result",
                                                   "result",
                                                   out SendCDRResultTypes Result,
                                                   out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargeDetailRecord    [optional]

                if (JSONObject.ParseOptionalJSON("chargeDetailRecord",
                                                 "charge detail record",
                                                 WWCP.ChargeDetailRecord.TryParse,
                                                 out ChargeDetailRecord? ChargeDetailRecord,
                                                 out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Description           [optional]

                if (JSONObject.ParseOptional("description",
                                             "description",
                                             out I18NString? Description,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Warnings              [optional]

                if (JSONObject.ParseOptionalJSON("warnings",
                                                 "description or warnings",
                                                 Warning.TryParse,
                                                 out IEnumerable<Warning> Warnings,
                                                 out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Location              [optional]

                if (JSONObject.ParseOptional("location",
                                             "location URL",
                                             org.GraphDefined.Vanaheimr.Hermod.HTTP.Location.TryParse,
                                             out Location? Location,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Runtime               [optional]

                if (JSONObject.ParseOptional("runtime",
                                             "runtime",
                                             out TimeSpan? Runtime,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                SendCDRResult = new SendCDRResult(
                                    Timestamp,
                                    AuthorizatorId,
                                    Result,
                                    ChargeDetailRecord,
                                    Description,
                                    Warnings,
                                    Location,
                                    Runtime
                                );

                ErrorResponse = null;
                return true;

            }
            catch (Exception e)
            {
                ErrorResponse  = e.Message;
                SendCDRResult  = null;
                return false;
            }

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: ", Result.ToString(),
                             Warnings.SafeAny() ? " with " + Warnings.Count() + " warnings!" : "");

        #endregion

    }


    public enum SendCDRResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The request was not processed.
        /// </summary>
        NoOperation,

        /// <summary>
        /// The processing of charge detail records was disabled by an administrator.
        /// </summary>
        AdminDown,

        /// <summary>
        /// The processing of charge detail records is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The charge detail record was filtered and will no longer be processed and/or forwarded.
        /// </summary>
        Filtered,

        /// <summary>
        /// The charging session identification of the charge detail record is invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The charging session identification of the charge detail record is unknown.
        /// </summary>
        UnknownSessionId,

        /// <summary>
        /// The e-mobility provider (start) of the charge detail record is unknown.
        /// </summary>
        UnknownProviderIdStart,

        /// <summary>
        /// The e-mobility provider (stop) of the charge detail record is unknown.
        /// </summary>
        UnknownProviderIdStop,

        /// <summary>
        /// The charging location is unknown.
        /// </summary>
        UnknownLocation,

        /// <summary>
        /// The authentication token of the charge detail record is invalid.
        /// </summary>
        InvalidToken,

        /// <summary>
        /// The charge detail record format could not be converted.
        /// </summary>
        CouldNotConvertCDRFormat,

        /// <summary>
        /// The request led to an error.
        /// </summary>
        Error,

        /// <summary>
        /// The request timed out.
        /// </summary>
        Timeout,


        /// <summary>
        /// The charge detail record had been enqueued for later processing and/or transmission.
        /// </summary>
        Enqueued,

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        Success

    }

}
