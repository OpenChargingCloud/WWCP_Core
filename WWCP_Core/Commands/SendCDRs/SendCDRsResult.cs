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
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.JSON;
using System.Collections;

#endregion

namespace org.GraphDefined.WWCP
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
        public const String JSONLDContext  = "https://open.charging.cloud/contexts/wwcp+json/sendChargeDetailRecordResult";

        #endregion

        #region Properties

        /// <summary>
        /// A charge detail record.
        /// </summary>
        public ChargeDetailRecord    ChargeDetailRecord    { get; }

        /// <summary>
        /// The result of the SendCDR request.
        /// </summary>
        public SendCDRResultTypes    Result                { get; }

        /// <summary>
        /// An optional multi-language description of the result.
        /// </summary>
        public I18NString            Description           { get; }

        /// <summary>
        /// Optional warnings or additional information.
        /// </summary>
        public IEnumerable<Warning> Warnings              { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?             Runtime               { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new SendCDR result.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Result">The result of the SendCDR request.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public SendCDRResult(ChargeDetailRecord    ChargeDetailRecord,
                             SendCDRResultTypes    Result,
                             IEnumerable<Warning> Warnings      = null,
                             I18NString            Description   = null,
                             TimeSpan?             Runtime       = null)
        {

            this.ChargeDetailRecord  = ChargeDetailRecord;
            this.Result              = Result;
            this.Description         = Description;
            this.Warnings            = Warnings != null
                                           ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                           : new Warning[0];
            this.Runtime             = Runtime;

        }

        #endregion


        #region (static) Enqueued(ChargeDetailRecord, ...)

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Enqueued(ChargeDetailRecord   ChargeDetailRecord,
                     I18NString           Description   = null,
                     TimeSpan?            Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Enqueued,
                                     new Warning[0],
                                     Description,
                                     Runtime);

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Enqueued(ChargeDetailRecord  ChargeDetailRecord,
                     Warning             Warning,
                     I18NString          Description   = null,
                     TimeSpan?           Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Enqueued,
                                     new Warning[] { Warning },
                                     Description,
                                     Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Enqueued(ChargeDetailRecord    ChargeDetailRecord,
                     IEnumerable<Warning> Warnings,
                     I18NString            Description   = null,
                     TimeSpan?             Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Enqueued,
                                     Warnings,
                                     Description,
                                     Runtime);

        #endregion

        #region (static) Success (ChargeDetailRecord, ...)

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Success(ChargeDetailRecord   ChargeDetailRecord,
                    I18NString           Description   = null,
                    TimeSpan?            Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Success,
                                     new Warning[0],
                                     Description,
                                     Runtime);

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Success(ChargeDetailRecord  ChargeDetailRecord,
                    Warning             Warning,
                    I18NString          Description   = null,
                    TimeSpan?           Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Success,
                                     new Warning[] { Warning },
                                     Description,
                                     Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Success(ChargeDetailRecord    ChargeDetailRecord,
                    IEnumerable<Warning> Warnings,
                    I18NString            Description   = null,
                    TimeSpan?             Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Success,
                                     Warnings,
                                     Description,
                                     Runtime);

        #endregion

        #region (static) Error   (ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Error(ChargeDetailRecord   ChargeDetailRecord,
                  I18NString           Description   = null,
                  TimeSpan?            Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Error,
                                     new Warning[0],
                                     Description,
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Error(ChargeDetailRecord    ChargeDetailRecord,
                  Warning               Warning,
                  I18NString            Description   = null,
                  TimeSpan?             Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Error,
                                     new Warning[] { Warning },
                                     Description,
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Error(ChargeDetailRecord    ChargeDetailRecord,
                  IEnumerable<Warning> Warnings,
                  I18NString            Description   = null,
                  TimeSpan?             Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Error,
                                     Warnings,
                                     Description,
                                     Runtime);

        #endregion

        #region (static) Filtered(ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Filtered(ChargeDetailRecord   ChargeDetailRecord,
                     I18NString           Description   = null,
                     TimeSpan?            Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Filtered,
                                     new Warning[0],
                                     Description,
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Filtered(ChargeDetailRecord    ChargeDetailRecord,
                     Warning               Warning,
                     I18NString            Description   = null,
                     TimeSpan?             Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Filtered,
                                     new Warning[] { Warning },
                                     Description,
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Filtered(ChargeDetailRecord    ChargeDetailRecord,
                     IEnumerable<Warning> Warnings,
                     I18NString            Description   = null,
                     TimeSpan?             Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.Filtered,
                                     Warnings,
                                     Description,
                                     Runtime);

        #endregion


        #region (static) CouldNotConvertCDRFormat   (ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            CouldNotConvertCDRFormat(ChargeDetailRecord   ChargeDetailRecord,
                                     I18NString           Description   = null,
                                     TimeSpan?            Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.CouldNotConvertCDRFormat,
                                     new Warning[0],
                                     Description,
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            CouldNotConvertCDRFormat(ChargeDetailRecord    ChargeDetailRecord,
                                     Warning               Warning,
                                     I18NString            Description   = null,
                                     TimeSpan?             Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.CouldNotConvertCDRFormat,
                                     new Warning[] { Warning },
                                     Description,
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            CouldNotConvertCDRFormat(ChargeDetailRecord    ChargeDetailRecord,
                                     IEnumerable<Warning> Warnings,
                                     I18NString            Description   = null,
                                     TimeSpan?             Runtime       = null)


                => new SendCDRResult(ChargeDetailRecord,
                                     SendCDRResultTypes.CouldNotConvertCDRFormat,
                                     Warnings,
                                     Description,
                                     Runtime);

        #endregion



        #region ToJSON(Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charge detail record.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A custom charge detail record serializer.</param>
        /// <param name="CustomSendCDRResultSerializer">A custom send charge detail record result serializer.</param>
        public JObject ToJSON(Boolean                                           Embedded                             = false,
                              CustomJSONSerializerDelegate<ChargeDetailRecord>  CustomChargeDetailRecordSerializer   = null,
                              CustomJSONSerializerDelegate<SendCDRResult>       CustomSendCDRResultSerializer        = null)
        {

            var JSON = ChargeDetailRecord?.ToJSON(Embedded:                            true,
                                                  CustomChargeDetailRecordSerializer:  CustomChargeDetailRecordSerializer) ?? new JObject();

            JSON["delivery"] = JSONObject.Create(

                                  !Embedded
                                      ? new JProperty("@context",      JSONLDContext)
                                      : null,

                                  new JProperty("result",              Result.ToString()),

                                  Description.IsNeitherNullNorEmpty()
                                      ? new JProperty("description",   Description)
                                      : null,

                                  Runtime.HasValue
                                      ? new JProperty("runtime",       Runtime.Value.TotalMilliseconds)
                                      : null,

                                  Warnings.SafeAny()
                                      ? new JProperty("warnings",      new JArray(Warnings.Select(waring => waring.ToJSON())))
                                      : null

                              );

            return CustomSendCDRResultSerializer != null
                       ? CustomSendCDRResultSerializer(this, JSON)
                       : JSON;

        }

        #endregion



        public override String ToString()

            => String.Concat("Result: ", Result.ToString(),
                             Warnings.SafeAny() ? " with " + Warnings.Count() + " warnings!" : "");

    }


    /// <summary>
    /// The result of sending charge detail records.
    /// </summary>
    public class SendCDRsResult : IEnumerable<SendCDRResult>
    {

        #region Properties

        /// <summary>
        /// The identification of the charge detail record sending or receiving entity.
        /// </summary>
        public IId                          AuthorizatorId                 { get; }

        /// <summary>
        /// The entity sending charge detail records.
        /// </summary>
        public ISendChargeDetailRecords     ISendChargeDetailRecords       { get; }

        /// <summary>
        /// The entity receiving charge detail records.
        /// </summary>
        public IReceiveChargeDetailRecords  IReceiveChargeDetailRecords    { get; }

        /// <summary>
        /// The result of the charge detail record transmission.
        /// </summary>
        public SendCDRsResultTypes          Result                         { get; }

        /// <summary>
        /// An enumeration of charge detail records.
        /// </summary>
        public IEnumerable<SendCDRResult>   ResultMap                      { get; }

        /// <summary>
        /// An optional description of the send charge detail records result.
        /// </summary>
        public String                       Description                    { get; }

        /// <summary>
        /// Optional warnings or additional information.
        /// </summary>
        public IEnumerable<Warning> Warnings                       { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                    Runtime                        { get; }

        #endregion

        #region Constructor(s)

        #region (internal) SendCDRsResult(AuthorizatorId, ISendChargeDetailRecords,    Result, ...)

        /// <summary>
        /// Create a new send charge detail records result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="Result">The result of the charge detail record transmission.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal SendCDRsResult(IId                         AuthorizatorId,
                                ISendChargeDetailRecords    ISendChargeDetailRecords,
                                SendCDRsResultTypes         Result,
                                IEnumerable<SendCDRResult> ResultMap,
                                String                      Description   = null,
                                IEnumerable<Warning> Warnings      = null,
                                TimeSpan?                   Runtime       = null)
        {

            this.AuthorizatorId            = AuthorizatorId;
            this.ISendChargeDetailRecords  = ISendChargeDetailRecords;
            this.Result                    = Result;
            this.ResultMap                 = ResultMap ?? new SendCDRResult[0];
            this.Description               = Description;

            this.Warnings                  = Warnings != null
                                                 ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                 : new Warning[0];

            this.Runtime                   = Runtime;

        }

        #endregion

        #region (internal) SendCDRsResult(AuthorizatorId, IReceiveChargeDetailRecords, Result, ...)

        /// <summary>
        /// Create a new send charge detail records result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record receiving entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="Result">The result of the charge detail record transmission.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal SendCDRsResult(IId                          AuthorizatorId,
                                IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                                SendCDRsResultTypes          Result,
                                IEnumerable<SendCDRResult> ResultMap     = null,
                                String                       Description   = null,
                                IEnumerable<Warning> Warnings      = null,
                                TimeSpan?                    Runtime       = null)
        {

            this.AuthorizatorId               = AuthorizatorId;
            this.IReceiveChargeDetailRecords  = IReceiveChargeDetailRecords;
            this.Result                       = Result;
            this.ResultMap                    = ResultMap ?? new SendCDRResult[0];
            this.Description                  = Description;

            this.Warnings                     = Warnings != null
                                                  ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                  : new Warning[0];

            this.Runtime                      = Runtime;

        }

        #endregion

        #endregion


        #region (static) NoOperation (AuthorizatorId, ...)

        /// <summary>
        /// No operation e.g. because no charge detail record passed the charge detail records filter.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            NoOperation(IId                              AuthorizatorId,
                        ISendChargeDetailRecords         ISendChargeDetailRecords,
                        IEnumerable<ChargeDetailRecord> ResultMap,
                        String                           Description   = null,
                        IEnumerable<Warning> Warnings      = null,
                        TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.NoOperation,
                                      ResultMap.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.NoOperation)),
                                      Description,
                                      Warnings,
                                      Runtime);



        /// <summary>
        /// No operation e.g. because no charge detail record passed the charge detail records filter.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            NoOperation(IId                              AuthorizatorId,
                        IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                        IEnumerable<ChargeDetailRecord> ResultMap,
                        String                           Description   = null,
                        IEnumerable<Warning> Warnings      = null,
                        TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.NoOperation,
                                      ResultMap.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.NoOperation)),
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion

        #region (static) AdminDown   (AuthorizatorId, ...)

        ///// <summary>
        ///// The service is administratively down.
        ///// </summary>
        ///// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        ///// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        ///// <param name="ResultMap">An enumeration of charge detail records.</param>
        ///// <param name="Description">An optional description of the send charge detail records result.</param>
        ///// <param name="Warnings">Optional warnings or additional information.</param>
        ///// <param name="Runtime">The runtime of the request.</param>
        //public static SendCDRsResult

        //    AdminDown(IId                         AuthorizatorId,
        //              ISendChargeDetailRecords    ISendChargeDetailRecords,
        //              IEnumerable<SendCDRResult>  ResultMap,
        //              String                      Description   = null,
        //              IEnumerable<String>         Warnings      = null,
        //              TimeSpan?                   Runtime       = null)


        //        => new SendCDRsResult(AuthorizatorId,
        //                              ISendChargeDetailRecords,
        //                              SendCDRsResultTypes.AdminDown,
        //                              ResultMap,
        //                              Description,
        //                              Warnings,
        //                              Runtime);

        /// <summary>
        /// The service is administratively down.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            AdminDown(IId                              AuthorizatorId,
                      ISendChargeDetailRecords         ISendChargeDetailRecords,
                      IEnumerable<ChargeDetailRecord> ResultMap,
                      String                           Description   = null,
                      IEnumerable<Warning> Warnings      = null,
                      TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.AdminDown,
                                      ResultMap.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.AdminDown)),
                                      Description,
                                      Warnings,
                                      Runtime);



        ///// <summary>
        ///// The service is administratively down.
        ///// </summary>
        ///// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        ///// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        ///// <param name="ResultMap">An enumeration of charge detail records.</param>
        ///// <param name="Description">An optional description of the send charge detail records result.</param>
        ///// <param name="Warnings">Optional warnings or additional information.</param>
        ///// <param name="Runtime">The runtime of the request.</param>
        //public static SendCDRsResult

        //    AdminDown(IId                          AuthorizatorId,
        //              IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
        //              IEnumerable<SendCDRResult>   ResultMap,
        //              String                       Description   = null,
        //              IEnumerable<String>          Warnings      = null,
        //              TimeSpan?                    Runtime       = null)


        //        => new SendCDRsResult(AuthorizatorId,
        //                              IReceiveChargeDetailRecords,
        //                              SendCDRsResultTypes.AdminDown,
        //                              ResultMap,
        //                              Description,
        //                              Warnings,
        //                              Runtime);

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

            AdminDown(IId                              AuthorizatorId,
                      IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                      IEnumerable<ChargeDetailRecord> ResultMap,
                      String                           Description   = null,
                      IEnumerable<Warning> Warnings      = null,
                      TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.AdminDown,
                                      ResultMap.Select(cdr => new SendCDRResult(cdr, SendCDRResultTypes.AdminDown)),
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion

        #region (static) OutOfService(AuthorizatorId, ...)

        ///// <summary>
        ///// The service is out of service.
        ///// </summary>
        ///// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        ///// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        ///// <param name="ResultMap">An enumeration of charge detail records.</param>
        ///// <param name="Description">An optional description of the send charge detail records result.</param>
        ///// <param name="Warnings">Optional warnings or additional information.</param>
        ///// <param name="Runtime">The runtime of the request.</param>
        //public static SendCDRsResult

        //    OutOfService(IId                         AuthorizatorId,
        //                 ISendChargeDetailRecords    ISendChargeDetailRecords,
        //                 IEnumerable<SendCDRResult>  ResultMap,
        //                 String                      Description   = null,
        //                 IEnumerable<String>         Warnings      = null,
        //                 TimeSpan?                   Runtime       = null)


        //        => new SendCDRsResult(AuthorizatorId,
        //                              ISendChargeDetailRecords,
        //                              SendCDRsResultTypes.OutOfService,
        //                              ResultMap,
        //                              Description,
        //                              Warnings,
        //                              Runtime);

        /// <summary>
        /// The service is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            OutOfService(IId                              AuthorizatorId,
                         ISendChargeDetailRecords         ISendChargeDetailRecords,
                         IEnumerable<ChargeDetailRecord> ResultMap,
                         String                           Description   = null,
                         IEnumerable<Warning> Warnings      = null,
                         TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.OutOfService,
                                      ResultMap.Select(cdr => new SendCDRResult(cdr, SendCDRResultTypes.OutOfService)),
                                      Description,
                                      Warnings,
                                      Runtime);



        ///// <summary>
        ///// The service is out of service.
        ///// </summary>
        ///// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        ///// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        ///// <param name="RejectedChargeDetailRecords">An enumeration of charge detail records.</param>
        ///// <param name="Description">An optional description of the send charge detail records result.</param>
        ///// <param name="Warnings">Optional warnings or additional information.</param>
        ///// <param name="Runtime">The runtime of the request.</param>
        //public static SendCDRsResult

        //    OutOfService(IId                          AuthorizatorId,
        //                 IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
        //                 IEnumerable<SendCDRResult>   RejectedChargeDetailRecords,
        //                 String                       Description   = null,
        //                 IEnumerable<String>          Warnings      = null,
        //                 TimeSpan?                    Runtime       = null)


        //        => new SendCDRsResult(AuthorizatorId,
        //                              IReceiveChargeDetailRecords,
        //                              SendCDRsResultTypes.OutOfService,
        //                              RejectedChargeDetailRecords,
        //                              Description,
        //                              Warnings,
        //                              Runtime);

        /// <summary>
        /// The service is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            OutOfService(IId                              AuthorizatorId,
                         IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                         IEnumerable<ChargeDetailRecord> RejectedChargeDetailRecords,
                         String                           Description   = null,
                         IEnumerable<Warning> Warnings      = null,
                         TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.OutOfService,
                                      RejectedChargeDetailRecords.Select(cdr => new SendCDRResult(cdr, SendCDRResultTypes.OutOfService)),
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion

        #region (static) Enqueued    (AuthorizatorId, ...)

        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Enqueued(IId                       AuthorizatorId,
                     ISendChargeDetailRecords  ISendChargeDetailRecords,
                     ChargeDetailRecord        ChargeDetailRecord,
                     String                    Description   = null,
                     IEnumerable<Warning> Warnings      = null,
                     TimeSpan?                 Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Enqueued,
                                      new SendCDRResult[] { new SendCDRResult(ChargeDetailRecord, SendCDRResultTypes.Enqueued) },
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Enqueued(IId                              AuthorizatorId,
                     ISendChargeDetailRecords         ISendChargeDetailRecords,
                     IEnumerable<ChargeDetailRecord> ChargeDetailRecords,
                     String                           Description   = null,
                     IEnumerable<Warning> Warnings      = null,
                     TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Enqueued,
                                      ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Enqueued)),
                                      Description,
                                      Warnings,
                                      Runtime);



        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Enqueued(IId                          AuthorizatorId,
                     IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                     ChargeDetailRecord           ChargeDetailRecord,
                     String                       Description   = null,
                     IEnumerable<Warning> Warnings      = null,
                     TimeSpan?                    Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Enqueued,
                                      new SendCDRResult[] { new SendCDRResult(ChargeDetailRecord, SendCDRResultTypes.Enqueued) },
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Enqueued(IId                              AuthorizatorId,
                     IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                     IEnumerable<ChargeDetailRecord> ChargeDetailRecords,
                     String                           Description   = null,
                     IEnumerable<Warning> Warnings      = null,
                     TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Enqueued,
                                      ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Enqueued)),
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion

        #region (static) Timeout     (AuthorizatorId, ...)

        /// <summary>
        /// A timeout occured.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Timeout(IId                              AuthorizatorId,
                    ISendChargeDetailRecords         ISendChargeDetailRecords,
                    IEnumerable<ChargeDetailRecord> ResultMap,
                    String                           Description   = null,
                    IEnumerable<Warning> Warnings      = null,
                    TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Timeout,
                                      ResultMap.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Timeout)),
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion

        #region (static) Success     (AuthorizatorId, ...)

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Success(IId                       AuthorizatorId,
                    ISendChargeDetailRecords  ISendChargeDetailRecords,
                    ChargeDetailRecord        ChargeDetailRecord,
                    String                    Description   = null,
                    IEnumerable<Warning> Warnings      = null,
                    TimeSpan?                 Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      new SendCDRResult[] { new SendCDRResult(ChargeDetailRecord, SendCDRResultTypes.Success) },
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Success(IId                              AuthorizatorId,
                    ISendChargeDetailRecords         ISendChargeDetailRecords,
                    IEnumerable<ChargeDetailRecord> ChargeDetailRecords,
                    String                           Description   = null,
                    IEnumerable<Warning> Warnings      = null,
                    TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Success)),
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Success(IId                          AuthorizatorId,
                    IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                    ChargeDetailRecord           ChargeDetailRecord,
                    String                       Description   = null,
                    IEnumerable<Warning> Warnings      = null,
                    TimeSpan?                    Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      new SendCDRResult[] { new SendCDRResult(ChargeDetailRecord, SendCDRResultTypes.Success) },
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Success(IId                              AuthorizatorId,
                    IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                    IEnumerable<ChargeDetailRecord> ChargeDetailRecords,
                    String                           Description   = null,
                    IEnumerable<Warning> Warnings      = null,
                    TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Success)),
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion

        #region (static) Error       (AuthorizatorId, ...)

        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecord">A charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(IId                       AuthorizatorId,
                  ISendChargeDetailRecords  ISendChargeDetailRecords,
                  ChargeDetailRecord        ChargeDetailRecord,
                  String                    Description   = null,
                  IEnumerable<Warning> Warnings      = null,
                  TimeSpan?                 Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      new SendCDRResult[] { new SendCDRResult(ChargeDetailRecord, SendCDRResultTypes.Error) },
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(IId                              AuthorizatorId,
                  ISendChargeDetailRecords         ISendChargeDetailRecords,
                  IEnumerable<ChargeDetailRecord> ChargeDetailRecords,
                  String                           Description   = null,
                  IEnumerable<Warning> Warnings      = null,
                  TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      ChargeDetailRecords.Select(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Error)),
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(IId                              AuthorizatorId,
                  ISendChargeDetailRecords         ISendChargeDetailRecords,
                  IEnumerable<ChargeDetailRecord> ChargeDetailRecords,
                  Warning                          Warning,
                  TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      ChargeDetailRecords.Select(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Error)),
                                      String.Empty,
                                      new Warning[] { Warning },
                                      Runtime);



        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecord">A charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(IId                          AuthorizatorId,
                  IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                  ChargeDetailRecord           ChargeDetailRecord,
                  String                       Description   = null,
                  IEnumerable<Warning> Warnings      = null,
                  TimeSpan?                    Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      new SendCDRResult[] { new SendCDRResult(ChargeDetailRecord, SendCDRResultTypes.Error) },
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(IId                              AuthorizatorId,
                  IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                  IEnumerable<ChargeDetailRecord> ChargeDetailRecords,
                  String                           Description   = null,
                  IEnumerable<Warning> Warnings      = null,
                  TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      ChargeDetailRecords.Select(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Error)),
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion

        #region (static) Mixed       (AuthorizatorId, ...)

        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="Result">A charge detail record result.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Mixed(IId                         AuthorizatorId,
                  ISendChargeDetailRecords    ISendChargeDetailRecords,
                  SendCDRResult               Result,
                  String                      Description   = null,
                  IEnumerable<Warning> Warnings      = null,
                  TimeSpan?                   Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Mixed,
                                      new SendCDRResult[] { Result },
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail record results.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Mixed(IId                         AuthorizatorId,
                  ISendChargeDetailRecords    ISendChargeDetailRecords,
                  IEnumerable<SendCDRResult> ResultMap,
                  String                      Description   = null,
                  IEnumerable<Warning> Warnings      = null,
                  TimeSpan?                   Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Mixed,
                                      ResultMap,
                                      Description,
                                      Warnings,
                                      Runtime);



        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="Result">A charge detail record result.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Mixed(IId                          AuthorizatorId,
                  IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                  SendCDRResult                Result,
                  String                       Description   = null,
                  IEnumerable<Warning> Warnings      = null,
                  TimeSpan?                    Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Mixed,
                                      new SendCDRResult[] { Result },
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="ResultMap">An enumeration of charge detail record results.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Mixed(IId                          AuthorizatorId,
                  IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                  IEnumerable<SendCDRResult> ResultMap,
                  String                       Description   = null,
                  IEnumerable<Warning> Warnings      = null,
                  TimeSpan?                    Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Mixed,
                                      ResultMap,
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion



        public IEnumerator<SendCDRResult> GetEnumerator()
            => ResultMap.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ResultMap.GetEnumerator();


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
        /// The EVSE identification is unknown.
        /// </summary>
        UnknownEVSE,

        /// <summary>
        /// A data format error occured.
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
        /// A timeout occured.
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

    public enum SendCDRResultTypes
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
        /// The charge detail record had been filtered.
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
        /// The EVSE identification is unknown.
        /// </summary>
        UnknownEVSE,

        /// <summary>
        /// A data format error occured.
        /// </summary>
        CouldNotConvertCDRFormat,


        /// <summary>
        /// The charge detail record had been enqueued for later transmission.
        /// </summary>
        Enqueued,

        /// <summary>
        /// Everything ok.
        /// </summary>
        Success,

        /// <summary>
        /// A timeout occured.
        /// </summary>
        Timeout,



        /// <summary>
        /// The operation led to an error.
        /// </summary>
        Error

    }


    public static class SendCDRResultTypesExtentions
    {

        public static SendCDRsResultTypes Covert(this SendCDRResultTypes result)
        {

            switch (result)
            {

                case SendCDRResultTypes.NoOperation:
                    return SendCDRsResultTypes.NoOperation;

                case SendCDRResultTypes.AdminDown:
                    return SendCDRsResultTypes.AdminDown;

                case SendCDRResultTypes.OutOfService:
                    return SendCDRsResultTypes.OutOfService;

                case SendCDRResultTypes.Filtered:
                    return SendCDRsResultTypes.Filtered;

                case SendCDRResultTypes.InvalidSessionId:
                    return SendCDRsResultTypes.InvalidSessionId;

                case SendCDRResultTypes.UnknownSessionId:
                    return SendCDRsResultTypes.UnknownSessionId;

                case SendCDRResultTypes.UnknownEVSE:
                    return SendCDRsResultTypes.UnknownEVSE;

                case SendCDRResultTypes.CouldNotConvertCDRFormat:
                    return SendCDRsResultTypes.CouldNotConvertCDRFormat;


                case SendCDRResultTypes.Enqueued:
                    return SendCDRsResultTypes.Enqueued;

                case SendCDRResultTypes.Success:
                    return SendCDRsResultTypes.Success;

                case SendCDRResultTypes.Timeout:
                    return SendCDRsResultTypes.Timeout;



                case SendCDRResultTypes.Error:
                    return SendCDRsResultTypes.Error;

                default:
                    return SendCDRsResultTypes.Unspecified;

            }

        }

    }

}
