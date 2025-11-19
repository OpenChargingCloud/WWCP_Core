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

using System.Collections;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for SendCDRsResults.
    /// </summary>
    public static class SendCDRResultTypesExtensions
    {

        #region Convert(this SendCDRResultType)

        public static SendCDRsResultTypes Convert(this SendCDRResultTypes SendCDRResultType)

            => SendCDRResultType switch {
                   SendCDRResultTypes.Unspecified               => SendCDRsResultTypes.Unspecified,
                   SendCDRResultTypes.NoOperation               => SendCDRsResultTypes.NoOperation,
                   SendCDRResultTypes.AdminDown                 => SendCDRsResultTypes.AdminDown,
                   SendCDRResultTypes.OutOfService              => SendCDRsResultTypes.OutOfService,
                   SendCDRResultTypes.Filtered                  => SendCDRsResultTypes.Filtered,
                   SendCDRResultTypes.InvalidSessionId          => SendCDRsResultTypes.InvalidSessionId,
                   SendCDRResultTypes.UnknownSessionId          => SendCDRsResultTypes.UnknownSessionId,
                   SendCDRResultTypes.UnknownProviderIdStart    => SendCDRsResultTypes.UnknownProviderIdStart,
                   SendCDRResultTypes.UnknownLocation           => SendCDRsResultTypes.UnknownLocation,
                   SendCDRResultTypes.InvalidToken              => SendCDRsResultTypes.InvalidToken,
                   SendCDRResultTypes.CouldNotConvertCDRFormat  => SendCDRsResultTypes.CouldNotConvertCDRFormat,
                   SendCDRResultTypes.Enqueued                  => SendCDRsResultTypes.Enqueued,
                   SendCDRResultTypes.Success                   => SendCDRsResultTypes.Success,
                   SendCDRResultTypes.Timeout                   => SendCDRsResultTypes.Timeout,
                   SendCDRResultTypes.Error                     => SendCDRsResultTypes.Error,
                   _                                            => SendCDRsResultTypes.Error
               };

        #endregion

        #region Combine(SendCDRResults, AuthorizatorId, ..., Description = null, Warnings = null)

        public static SendCDRsResult Combine(this IEnumerable<SendCDRResult>  SendCDRResults,
                                             IId                              AuthorizatorId,
                                             ISendChargeDetailRecords         ISendChargeDetailRecords,
                                             I18NString?                      Description   = null,
                                             IEnumerable<Warning>?            Warnings      = null)
        {

            if (!SendCDRResults.Any())
                return SendCDRsResult.Error(Timestamp.Now,
                                            AuthorizatorId,
                                            ISendChargeDetailRecords,
                                            ChargeDetailRecord:  null,
                                            Description:         Description,
                                            Warnings:            Warnings,
                                            Runtime:             TimeSpan.Zero);


            var analysis  = SendCDRResults.GroupBy(sendCDRResult => sendCDRResult.Result).    ToArray();
            var oldest    = SendCDRResults.MinBy  (sendCDRResult => sendCDRResult.Timestamp)!.Timestamp;
            var newest    = SendCDRResults.MaxBy  (sendCDRResult => sendCDRResult.Timestamp)!.Timestamp;

            if (analysis.Length == 1)
                return new SendCDRsResult(newest,
                                          AuthorizatorId,
                                          ISendChargeDetailRecords,
                                          analysis.First().Key.Convert(),
                                          SendCDRResults,
                                          Description ?? SendCDRResults.First().Description,
                                          Warnings,
                                          newest - oldest);

            return new SendCDRsResult(newest,
                                      AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Mixed,
                                      SendCDRResults,
                                      Description,
                                      Warnings,
                                      newest - oldest);

        }

        #endregion

        #region Combine(SendCDRResults, AuthorizatorId, ..., Description = null, Warnings = null)

        public static SendCDRsResult Combine(this IEnumerable<SendCDRResult>  SendCDRResults,
                                             IId                              AuthorizatorId,
                                             IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                                             I18NString?                      Description   = null,
                                             IEnumerable<Warning>?            Warnings      = null)
        {

            if (!SendCDRResults.Any())
                return SendCDRsResult.Error(Timestamp.Now,
                                            AuthorizatorId,
                                            IReceiveChargeDetailRecords,
                                            ChargeDetailRecord:  null,
                                            Description:         Description,
                                            Warnings:            Warnings,
                                            Runtime:             TimeSpan.Zero);


            var analysis  = SendCDRResults.GroupBy(sendCDRResult => sendCDRResult.Result).    ToArray();
            var oldest    = SendCDRResults.MinBy  (sendCDRResult => sendCDRResult.Timestamp)!.Timestamp;
            var newest    = SendCDRResults.MaxBy  (sendCDRResult => sendCDRResult.Timestamp)!.Timestamp;

            if (analysis.Length == 1)
                return new SendCDRsResult(newest,
                                          AuthorizatorId,
                                          IReceiveChargeDetailRecords,
                                          analysis.First().Key.Convert(),
                                          SendCDRResults,
                                          Description ?? SendCDRResults.First().Description,
                                          Warnings,
                                          newest - oldest);

            return new SendCDRsResult(newest,
                                      AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Mixed,
                                      SendCDRResults,
                                      Description,
                                      Warnings,
                                      newest - oldest);

        }

        #endregion

    }


    /// <summary>
    /// The result of sending charge detail records.
    /// </summary>
    public class SendCDRsResult : IEnumerable<SendCDRResult>
    {

        #region Properties

        /// <summary>
        /// The timestamp of the charge detail record result.
        /// </summary>
        public DateTimeOffset                Timestamp                      { get; }

        /// <summary>
        /// The identification of the charge detail record sending or receiving entity.
        /// </summary>
        public IId                           AuthorizatorId                 { get; }

        /// <summary>
        /// The entity sending charge detail records.
        /// </summary>
        public ISendChargeDetailRecords?     ISendChargeDetailRecords       { get; }

        /// <summary>
        /// The entity receiving charge detail records.
        /// </summary>
        public IReceiveChargeDetailRecords?  IReceiveChargeDetailRecords    { get; }

        /// <summary>
        /// The result of the charge detail record transmission.
        /// </summary>
        public SendCDRsResultTypes           Result                         { get; }

        /// <summary>
        /// An enumeration of charge detail records.
        /// </summary>
        public IEnumerable<SendCDRResult>    ResultMap                      { get; }

        /// <summary>
        /// An optional description of the send charge detail records result.
        /// </summary>
        public I18NString                    Description                    { get; }

        /// <summary>
        /// Optional warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>          Warnings                       { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                     Runtime                        { get; }

        #endregion

        #region Constructor(s)

        #region (internal) SendCDRsResult(AuthorizatorId, ISendChargeDetailRecords,    Result, ...)

        /// <summary>
        /// Create a new send charge detail records result.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="Result">The result of the charge detail record transmission.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal SendCDRsResult(DateTimeOffset               Timestamp,
                                IId                          AuthorizatorId,
                                ISendChargeDetailRecords     ISendChargeDetailRecords,
                                SendCDRsResultTypes          Result,
                                IEnumerable<SendCDRResult>?  ResultMap,
                                I18NString?                  Description   = null,
                                IEnumerable<Warning>?        Warnings      = null,
                                TimeSpan?                    Runtime       = null)
        {

            this.Timestamp                 = Timestamp;
            this.AuthorizatorId            = AuthorizatorId;
            this.ISendChargeDetailRecords  = ISendChargeDetailRecords;
            this.Result                    = Result;
            this.ResultMap                 = ResultMap   ?? [];
            this.Description               = Description ?? I18NString.Empty;

            this.Warnings                  = Warnings is not null
                                                 ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                 : [];

            this.Runtime                   = Runtime;

        }

        #endregion

        #region (internal) SendCDRsResult(AuthorizatorId, IReceiveChargeDetailRecords, Result, ...)

        /// <summary>
        /// Create a new send charge detail records result.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record receiving entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="Result">The result of the charge detail record transmission.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal SendCDRsResult(DateTimeOffset                Timestamp,
                                IId                           AuthorizatorId,
                                IReceiveChargeDetailRecords   IReceiveChargeDetailRecords,
                                SendCDRsResultTypes           Result,
                                IEnumerable<SendCDRResult>?   ResultMap     = null,
                                I18NString?                   Description   = null,
                                IEnumerable<Warning>?         Warnings      = null,
                                TimeSpan?                     Runtime       = null)
        {

            this.Timestamp                    = Timestamp;
            this.AuthorizatorId               = AuthorizatorId;
            this.IReceiveChargeDetailRecords  = IReceiveChargeDetailRecords;
            this.Result                       = Result;
            this.ResultMap                    = ResultMap   ?? [];
            this.Description                  = Description ?? I18NString.Empty;

            this.Warnings                     = Warnings is not null
                                                    ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                    : [];

            this.Runtime                      = Runtime;

        }

        #endregion

        #endregion


        #region (static) Unspecified              (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// An unspecified error occurred.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Unspecified(DateTimeOffset                   Timestamp,
                        IId                              AuthorizatorId,
                        ISendChargeDetailRecords         ISendChargeDetailRecords,
                        IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                        I18NString?                      Description   = null,
                        IEnumerable<Warning>?            Warnings      = null,
                        TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.NoOperation,
                        ChargeDetailRecords.SafeSelect(cdr => SendCDRResult.Unspecified(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);



        /// <summary>
        /// An unspecified error occurred.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Unspecified(DateTimeOffset                   Timestamp,
                        IId                              AuthorizatorId,
                        IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                        IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                        I18NString?                      Description   = null,
                        IEnumerable<Warning>?            Warnings      = null,
                        TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.NoOperation,
                        ChargeDetailRecords.SafeSelect(cdr => SendCDRResult.Unspecified(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation              (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// No operation e.g. because no charge detail record passed the charge detail records filter.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            NoOperation(DateTimeOffset                   Timestamp,
                        IId                              AuthorizatorId,
                        ISendChargeDetailRecords         ISendChargeDetailRecords,
                        IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                        I18NString?                      Description   = null,
                        IEnumerable<Warning>?            Warnings      = null,
                        TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.NoOperation,
                        ChargeDetailRecords.SafeSelect(cdr => SendCDRResult.NoOperation(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);



        /// <summary>
        /// No operation e.g. because no charge detail record passed the charge detail records filter.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            NoOperation(DateTimeOffset                   Timestamp,
                        IId                              AuthorizatorId,
                        IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                        IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                        I18NString?                      Description   = null,
                        IEnumerable<Warning>?            Warnings      = null,
                        TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.NoOperation,
                        ChargeDetailRecords.SafeSelect(cdr => SendCDRResult.NoOperation(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) AdminDown                (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The service is administratively down.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            AdminDown(DateTimeOffset                   Timestamp,
                      IId                              AuthorizatorId,
                      ISendChargeDetailRecords         ISendChargeDetailRecords,
                      IEnumerable<ChargeDetailRecord>  ResultMap,
                      I18NString?                      Description   = null,
                      IEnumerable<Warning>?            Warnings      = null,
                      TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.AdminDown,
                        ResultMap.SafeSelect(cdr => SendCDRResult.AdminDown(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The service is administratively down.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            AdminDown(DateTimeOffset                   Timestamp,
                      IId                              AuthorizatorId,
                      IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                      IEnumerable<ChargeDetailRecord>  ResultMap,
                      I18NString?                      Description   = null,
                      IEnumerable<Warning>?            Warnings      = null,
                      TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.AdminDown,
                        ResultMap.Select(cdr => SendCDRResult.AdminDown(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) OutOfService             (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The service is out of service.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            OutOfService(DateTimeOffset                   Timestamp,
                         IId                              AuthorizatorId,
                         ISendChargeDetailRecords         ISendChargeDetailRecords,
                         IEnumerable<ChargeDetailRecord>  ResultMap,
                         I18NString?                      Description   = null,
                         IEnumerable<Warning>?            Warnings      = null,
                         TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.OutOfService,
                        ResultMap.Select(cdr => SendCDRResult.OutOfService(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The service is out of service.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            OutOfService(DateTimeOffset                   Timestamp,
                         IId                              AuthorizatorId,
                         IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                         IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords,
                         I18NString?                      Description   = null,
                         IEnumerable<Warning>?            Warnings      = null,
                         TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.OutOfService,
                        RejectedChargeDetailRecords.Select(cdr => SendCDRResult.OutOfService(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Enqueued                 (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Enqueued(DateTimeOffset            Timestamp,
                     IId                       AuthorizatorId,
                     ISendChargeDetailRecords  ISendChargeDetailRecords,
                     ChargeDetailRecord        ChargeDetailRecord,
                     I18NString?               Description   = null,
                     IEnumerable<Warning>?     Warnings      = null,
                     TimeSpan?                 Runtime       = null)


                => new SendCDRsResult(Timestamp,
                                      AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Enqueued,
                                      [ SendCDRResult.Enqueued(Timestamp, AuthorizatorId, ChargeDetailRecord) ],
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Enqueued(DateTimeOffset                   Timestamp,
                     IId                              AuthorizatorId,
                     ISendChargeDetailRecords         ISendChargeDetailRecords,
                     IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                     I18NString?                      Description   = null,
                     IEnumerable<Warning>?            Warnings      = null,
                     TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.Enqueued,
                        ChargeDetailRecords.SafeSelect(cdr => SendCDRResult.Enqueued(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);



        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Enqueued(DateTimeOffset               Timestamp,
                     IId                          AuthorizatorId,
                     IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                     ChargeDetailRecord           ChargeDetailRecord,
                     I18NString?                  Description   = null,
                     IEnumerable<Warning>?        Warnings      = null,
                     TimeSpan?                    Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.Enqueued,
                        [ SendCDRResult.Enqueued(Timestamp, AuthorizatorId, ChargeDetailRecord) ],
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Enqueued(DateTimeOffset                   Timestamp,
                     IId                              AuthorizatorId,
                     IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                     IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                     I18NString?                      Description   = null,
                     IEnumerable<Warning>?            Warnings      = null,
                     TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.Enqueued,
                        ChargeDetailRecords.SafeSelect(cdr => SendCDRResult.Enqueued(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Timeout                  (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// A timeout occurred.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Timeout(DateTimeOffset                   Timestamp,
                    IId                              AuthorizatorId,
                    ISendChargeDetailRecords         ISendChargeDetailRecords,
                    IEnumerable<ChargeDetailRecord>  ResultMap,
                    I18NString?                      Description   = null,
                    IEnumerable<Warning>?            Warnings      = null,
                    TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.Timeout,
                        ResultMap.SafeSelect(cdr => SendCDRResult.Timeout(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success                  (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Success(DateTimeOffset            Timestamp,
                    IId                       AuthorizatorId,
                    ISendChargeDetailRecords  ISendChargeDetailRecords,
                    ChargeDetailRecord        ChargeDetailRecord,
                    I18NString?               Description   = null,
                    IEnumerable<Warning>?     Warnings      = null,
                    TimeSpan?                 Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.Success,
                        [ SendCDRResult.Success(Timestamp, AuthorizatorId, ChargeDetailRecord) ],
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Success(DateTimeOffset                   Timestamp,
                    IId                              AuthorizatorId,
                    ISendChargeDetailRecords         ISendChargeDetailRecords,
                    IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                    I18NString?                      Description   = null,
                    IEnumerable<Warning>?            Warnings      = null,
                    TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.Success,
                        ChargeDetailRecords.SafeSelect(cdr => SendCDRResult.Success(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Success(DateTimeOffset               Timestamp,
                    IId                          AuthorizatorId,
                    IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                    ChargeDetailRecord           ChargeDetailRecord,
                    I18NString?                  Description   = null,
                    IEnumerable<Warning>?        Warnings      = null,
                    TimeSpan?                    Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.Success,
                        [ SendCDRResult.Success(Timestamp, AuthorizatorId, ChargeDetailRecord) ],
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Success(DateTimeOffset                   Timestamp,
                    IId                              AuthorizatorId,
                    IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                    IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                    I18NString?                      Description   = null,
                    IEnumerable<Warning>?            Warnings      = null,
                    TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.Success,
                        ChargeDetailRecords.SafeSelect(cdr => SendCDRResult.Success(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error                    (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(DateTimeOffset            Timestamp,
                  IId                       AuthorizatorId,
                  ISendChargeDetailRecords  ISendChargeDetailRecords,
                  ChargeDetailRecord?       ChargeDetailRecord   = null,
                  I18NString?               Description          = null,
                  IEnumerable<Warning>?     Warnings             = null,
                  TimeSpan?                 Runtime              = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.Success,
                        ChargeDetailRecord is not null
                            ? [ SendCDRResult.Error(Timestamp, AuthorizatorId, ChargeDetailRecord) ]
                            : [],
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(DateTimeOffset                   Timestamp,
                  IId                              AuthorizatorId,
                  ISendChargeDetailRecords         ISendChargeDetailRecords,
                  IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                  I18NString?                      Description   = null,
                  IEnumerable<Warning>?            Warnings      = null,
                  TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.Success,
                        ChargeDetailRecords.Select(cdr => SendCDRResult.Error(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Warning">Optional warning or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(DateTimeOffset                   Timestamp,
                  IId                              AuthorizatorId,
                  ISendChargeDetailRecords         ISendChargeDetailRecords,
                  IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                  Warning                          Warning,
                  TimeSpan?                        Runtime   = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.Success,
                        ChargeDetailRecords.Select(cdr => SendCDRResult.Error(Timestamp, AuthorizatorId, cdr)),
                        I18NString.Empty,
                        [ Warning ],
                        Runtime);



        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(DateTimeOffset               Timestamp,
                  IId                          AuthorizatorId,
                  IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                  ChargeDetailRecord?          ChargeDetailRecord   = null,
                  I18NString?                  Description          = null,
                  IEnumerable<Warning>?        Warnings             = null,
                  TimeSpan?                    Runtime              = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.Success,
                        ChargeDetailRecord is not null
                            ? [ SendCDRResult.Error(Timestamp, AuthorizatorId, ChargeDetailRecord) ]
                            : [],
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(DateTimeOffset                   Timestamp,
                  IId                              AuthorizatorId,
                  IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                  IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                  I18NString?                      Description   = null,
                  IEnumerable<Warning>?            Warnings      = null,
                  TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.Success,
                        ChargeDetailRecords.Select(cdr => SendCDRResult.Error(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) InvalidSessionId         (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The given charging session identification is invalid.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            InvalidSessionId(DateTimeOffset                   Timestamp,
                             IId                              AuthorizatorId,
                             IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                             IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                             I18NString?                      Description   = null,
                             IEnumerable<Warning>?            Warnings      = null,
                             TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.InvalidSessionId,
                        ChargeDetailRecords.Select(cdr => SendCDRResult.Error(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) UnknownSessionId         (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The given charging session identification is unknown.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            UnknownSessionId(DateTimeOffset                   Timestamp,
                             IId                              AuthorizatorId,
                             IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                             IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                             I18NString?                      Description   = null,
                             IEnumerable<Warning>?            Warnings      = null,
                             TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.UnknownSessionId,
                        ChargeDetailRecords.Select(cdr => SendCDRResult.Error(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) UnknownProviderIdStart   (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The given charging session identification is unknown.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            UnknownProviderIdStart(DateTimeOffset                   Timestamp,
                                   IId                              AuthorizatorId,
                                   IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                                   IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                   I18NString?                      Description   = null,
                                   IEnumerable<Warning>?            Warnings      = null,
                                   TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.UnknownSessionId,
                        ChargeDetailRecords.Select(cdr => SendCDRResult.Error(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) InvalidToken             (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            InvalidToken(DateTimeOffset                   Timestamp,
                         IId                              AuthorizatorId,
                         IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                         IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                         I18NString?                      Description   = null,
                         IEnumerable<Warning>?            Warnings      = null,
                         TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.InvalidToken,
                        ChargeDetailRecords.Select(cdr => SendCDRResult.InvalidToken(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) CouldNotConvertCDRFormat (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The charge detail records could not be converted.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            CouldNotConvertCDRFormat(DateTimeOffset                   Timestamp,
                                     IId                              AuthorizatorId,
                                     IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                                     IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                     I18NString?                      Description   = null,
                                     IEnumerable<Warning>?            Warnings      = null,
                                     TimeSpan?                        Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.CouldNotConvertCDRFormat,
                        ChargeDetailRecords.Select(cdr => SendCDRResult.CouldNotConvertCDRFormat(Timestamp, AuthorizatorId, cdr)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Mixed                    (Timestamp, AuthorizatorId, ...)

        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="Result">A charge detail record result.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Mixed(DateTimeOffset            Timestamp,
                  IId                       AuthorizatorId,
                  ISendChargeDetailRecords  ISendChargeDetailRecords,
                  SendCDRResult             Result,
                  I18NString?               Description   = null,
                  IEnumerable<Warning>?     Warnings      = null,
                  TimeSpan?                 Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.Mixed,
                        [ Result ],
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail record results.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Mixed(DateTimeOffset              Timestamp,
                  IId                         AuthorizatorId,
                  ISendChargeDetailRecords    ISendChargeDetailRecords,
                  IEnumerable<SendCDRResult>  ResultMap,
                  I18NString?                 Description   = null,
                  IEnumerable<Warning>?       Warnings      = null,
                  TimeSpan?                   Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        ISendChargeDetailRecords,
                        SendCDRsResultTypes.Mixed,
                        ResultMap,
                        Description,
                        Warnings,
                        Runtime);



        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="Result">A charge detail record result.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Mixed(DateTimeOffset               Timestamp,
                  IId                          AuthorizatorId,
                  IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                  SendCDRResult                Result,
                  I18NString?                  Description   = null,
                  IEnumerable<Warning>?        Warnings      = null,
                  TimeSpan?                    Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.Mixed,
                        [ Result ],
                        Description,
                        Warnings,
                        Runtime);


        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail record results.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Mixed(DateTimeOffset               Timestamp,
                  IId                          AuthorizatorId,
                  IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                  IEnumerable<SendCDRResult>   ResultMap,
                  I18NString?                  Description   = null,
                  IEnumerable<Warning>?        Warnings      = null,
                  TimeSpan?                    Runtime       = null)


                => new (Timestamp,
                        AuthorizatorId,
                        IReceiveChargeDetailRecords,
                        SendCDRsResultTypes.Mixed,
                        ResultMap,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region GetEnumerator()

        public IEnumerator<SendCDRResult> GetEnumerator()
            => ResultMap.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ResultMap.GetEnumerator();

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
                => Result + " via " + AuthorizatorId;

        #endregion

    }


    public enum SendCDRsResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// No operation e.g. because no charge detail record passed the charge detail records filter.
        /// </summary>
        NoOperation,

        /// <summary>
        /// The service was disabled by an administrator.
        /// </summary>
        AdminDown,

        /// <summary>
        /// The service is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The charge detail records had been filtered.
        /// </summary>
        Filtered,

        /// <summary>
        /// The charging session identification is invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The charging session identification is unknown.
        /// </summary>
        UnknownSessionId,

        /// <summary>
        /// The ProviderIdStart of the charge detail record is unknown.
        /// </summary>
        UnknownProviderIdStart,

        /// <summary>
        /// The charging location is unknown.
        /// </summary>
        UnknownLocation,

        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        InvalidToken,

        /// <summary>
        /// A data format error occurred.
        /// </summary>
        CouldNotConvertCDRFormat,


        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        Enqueued,

        /// <summary>
        /// Everything ok.
        /// </summary>
        Success,

        /// <summary>
        /// A timeout occurred.
        /// </summary>
        Timeout,


        /// <summary>
        /// The operation led to an error.
        /// </summary>
        Error,


        /// <summary>
        /// Mixed results.
        /// </summary>
        Mixed

    }

}
