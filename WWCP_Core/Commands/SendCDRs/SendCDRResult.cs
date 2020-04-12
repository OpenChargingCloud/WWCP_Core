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
        public const String JSONLDContext  = "https://open.charging.cloud/contexts/wwcp+json/sendCDRResult";

        #endregion

        #region Properties

        /// <summary>
        /// The timestamp of the charge detail record result.
        /// </summary>
        public DateTime              Timestamp             { get; }

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
        public IEnumerable<Warning>  Warnings              { get; }

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
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Result">The result of the SendCDR request.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private SendCDRResult(DateTime              Timestamp,
                              ChargeDetailRecord    ChargeDetailRecord,
                              SendCDRResultTypes    Result,
                              I18NString            Description   = null,
                              IEnumerable<Warning>  Warnings      = null,
                              TimeSpan?             Runtime       = null)
        {

            this.Timestamp           = Timestamp;
            this.ChargeDetailRecord  = ChargeDetailRecord;
            this.Result              = Result;
            this.Description         = Description;
            this.Warnings            = Warnings != null
                                           ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                           : new Warning[0];
            this.Runtime             = Runtime;

        }

        #endregion


        #region (static) NoOperation             (Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            NoOperation(DateTime            Timestamp,
                        ChargeDetailRecord  ChargeDetailRecord,
                        I18NString          Description   = null,
                        TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.NoOperation,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            NoOperation(DateTime            Timestamp,
                        ChargeDetailRecord  ChargeDetailRecord,
                        Warning             Warning,
                        I18NString          Description   = null,
                        TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.NoOperation,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            NoOperation(DateTime              Timestamp,
                        ChargeDetailRecord    ChargeDetailRecord,
                        IEnumerable<Warning>  Warnings,
                        I18NString            Description   = null,
                        TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.NoOperation,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion

        #region (static) AdminDown               (Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            AdminDown(DateTime            Timestamp,
                      ChargeDetailRecord  ChargeDetailRecord,
                      I18NString          Description   = null,
                      TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.AdminDown,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            AdminDown(DateTime            Timestamp,
                      ChargeDetailRecord  ChargeDetailRecord,
                      Warning             Warning,
                      I18NString          Description   = null,
                      TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.AdminDown,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            AdminDown(DateTime              Timestamp,
                      ChargeDetailRecord    ChargeDetailRecord,
                      IEnumerable<Warning>  Warnings,
                      I18NString            Description   = null,
                      TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.AdminDown,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion

        #region (static) OutOfService            (Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            OutOfService(DateTime            Timestamp,
                         ChargeDetailRecord  ChargeDetailRecord,
                         I18NString          Description   = null,
                         TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.OutOfService,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            OutOfService(DateTime            Timestamp,
                         ChargeDetailRecord  ChargeDetailRecord,
                         Warning             Warning,
                         I18NString          Description   = null,
                         TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.OutOfService,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            OutOfService(DateTime              Timestamp,
                         ChargeDetailRecord    ChargeDetailRecord,
                         IEnumerable<Warning>  Warnings,
                         I18NString            Description   = null,
                         TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.OutOfService,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion

        #region (static) Filtered                (Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Filtered(DateTime            Timestamp,
                     ChargeDetailRecord  ChargeDetailRecord,
                     I18NString          Description   = null,
                     TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Filtered,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Filtered(DateTime            Timestamp,
                     ChargeDetailRecord  ChargeDetailRecord,
                     Warning             Warning,
                     I18NString          Description   = null,
                     TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Filtered,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Filtered(DateTime              Timestamp,
                     ChargeDetailRecord    ChargeDetailRecord,
                     IEnumerable<Warning>  Warnings,
                     I18NString            Description   = null,
                     TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Filtered,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion

        #region (static) InvalidSessionId        (Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            InvalidSessionId(DateTime            Timestamp,
                             ChargeDetailRecord  ChargeDetailRecord,
                             I18NString          Description   = null,
                             TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.InvalidSessionId,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            InvalidSessionId(DateTime            Timestamp,
                             ChargeDetailRecord  ChargeDetailRecord,
                             Warning             Warning,
                             I18NString          Description   = null,
                             TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.InvalidSessionId,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            InvalidSessionId(DateTime              Timestamp,
                             ChargeDetailRecord    ChargeDetailRecord,
                             IEnumerable<Warning>  Warnings,
                             I18NString            Description   = null,
                             TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.InvalidSessionId,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion

        #region (static) UnknownSessionId        (Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            UnknownSessionId(DateTime            Timestamp,
                             ChargeDetailRecord  ChargeDetailRecord,
                             I18NString          Description   = null,
                             TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.UnknownSessionId,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            UnknownSessionId(DateTime            Timestamp,
                             ChargeDetailRecord  ChargeDetailRecord,
                             Warning             Warning,
                             I18NString          Description   = null,
                             TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.UnknownSessionId,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            UnknownSessionId(DateTime              Timestamp,
                             ChargeDetailRecord    ChargeDetailRecord,
                             IEnumerable<Warning>  Warnings,
                             I18NString            Description   = null,
                             TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.UnknownSessionId,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion

        #region (static) CouldNotConvertCDRFormat(Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            CouldNotConvertCDRFormat(DateTime            Timestamp,
                                     ChargeDetailRecord  ChargeDetailRecord,
                                     I18NString          Description   = null,
                                     TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.CouldNotConvertCDRFormat,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            CouldNotConvertCDRFormat(DateTime            Timestamp,
                                     ChargeDetailRecord  ChargeDetailRecord,
                                     Warning             Warning,
                                     I18NString          Description   = null,
                                     TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.CouldNotConvertCDRFormat,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            CouldNotConvertCDRFormat(DateTime              Timestamp,
                                     ChargeDetailRecord    ChargeDetailRecord,
                                     IEnumerable<Warning>  Warnings,
                                     I18NString            Description   = null,
                                     TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.CouldNotConvertCDRFormat,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion

        #region (static) Enqueued                (Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Enqueued(DateTime            Timestamp,
                     ChargeDetailRecord  ChargeDetailRecord,
                     I18NString          Description   = null,
                     TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Enqueued,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Enqueued(DateTime            Timestamp,
                     ChargeDetailRecord  ChargeDetailRecord,
                     Warning             Warning,
                     I18NString          Description   = null,
                     TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Enqueued,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Enqueued(DateTime              Timestamp,
                     ChargeDetailRecord    ChargeDetailRecord,
                     IEnumerable<Warning>  Warnings,
                     I18NString            Description   = null,
                     TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Enqueued,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion

        #region (static) Timeout                 (Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Timeout(DateTime            Timestamp,
                    ChargeDetailRecord  ChargeDetailRecord,
                    I18NString          Description   = null,
                    TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Timeout,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Timeout(DateTime            Timestamp,
                    ChargeDetailRecord  ChargeDetailRecord,
                    Warning             Warning,
                    I18NString          Description   = null,
                    TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Timeout,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Timeout(DateTime              Timestamp,
                    ChargeDetailRecord    ChargeDetailRecord,
                    IEnumerable<Warning>  Warnings,
                    I18NString            Description   = null,
                    TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Timeout,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion

        #region (static) Success                 (Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Success(DateTime            Timestamp,
                    ChargeDetailRecord  ChargeDetailRecord,
                    I18NString          Description   = null,
                    TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Success,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Success(DateTime            Timestamp,
                    ChargeDetailRecord  ChargeDetailRecord,
                    Warning             Warning,
                    I18NString          Description   = null,
                    TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Success,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Success(DateTime              Timestamp,
                    ChargeDetailRecord    ChargeDetailRecord,
                    IEnumerable<Warning>  Warnings,
                    I18NString            Description   = null,
                    TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Success,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion

        #region (static) Error                   (Timestamp, ChargeDetailRecord, ...)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Error(DateTime            Timestamp,
                  ChargeDetailRecord  ChargeDetailRecord,
                  I18NString          Description   = null,
                  TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Error,
                                     Description,
                                     new Warning[0],
                                     Runtime);

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warning">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Error(DateTime            Timestamp,
                  ChargeDetailRecord  ChargeDetailRecord,
                  Warning             Warning,
                  I18NString          Description   = null,
                  TimeSpan?           Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Error,
                                     Description,
                                     new Warning[] { Warning },
                                     Runtime);


        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the charge detail record result.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="Warnings">Optional warnings or additional information.</param>
        /// <param name="Description">An optional multi-language description of the result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRResult

            Error(DateTime              Timestamp,
                  ChargeDetailRecord    ChargeDetailRecord,
                  IEnumerable<Warning>  Warnings,
                  I18NString            Description   = null,
                  TimeSpan?             Runtime       = null)


                => new SendCDRResult(Timestamp,
                                     ChargeDetailRecord,
                                     SendCDRResultTypes.Error,
                                     Description,
                                     Warnings,
                                     Runtime);

        #endregion


        #region ToJSON(Embedded = false, IncludeCDR = true, ...)

        /// <summary>
        /// Return a JSON representation of the given charge detail record.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure.</param>
        /// <param name="IncludeCDR">Whether to include the embedded charge detail record.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A custom charge detail record serializer.</param>
        /// <param name="CustomSendCDRResultSerializer">A custom send charge detail record result serializer.</param>
        public JObject ToJSON(Boolean                                           Embedded                             = false,
                              Boolean                                           IncludeCDR                           = true,
                              CustomJSONSerializerDelegate<ChargeDetailRecord>  CustomChargeDetailRecordSerializer   = null,
                              CustomJSONSerializerDelegate<SendCDRResult>       CustomSendCDRResultSerializer        = null)
        {

            var JSON = JSONObject.Create(

                           !Embedded
                               ? new JProperty("@context",            JSONLDContext)
                               : null,

                           new JProperty("timestamp",                 Timestamp.ToIso8601()),

                           new JProperty("result",                    Result.ToString()),

                           Description.IsNeitherNullNorEmpty()
                               ? new JProperty("description",         Description)
                               : null,

                           Warnings.SafeAny()
                               ? new JProperty("warnings",            new JArray(Warnings.Select(waring => waring.ToJSON())))
                               : null,

                           Runtime.HasValue
                               ? new JProperty("runtime",             Runtime.Value.TotalMilliseconds)
                               : null,

                           IncludeCDR && ChargeDetailRecord != null
                               ? new JProperty("chargeDetailRecord",  ChargeDetailRecord.ToJSON(Embedded:                           true,
                                                                                                CustomChargeDetailRecordSerializer: CustomChargeDetailRecordSerializer))
                               : null

                       );

            return CustomSendCDRResultSerializer != null
                       ? CustomSendCDRResultSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region (static) TryParse(JSONObject, ..., out SendCDRResult, out ErrorResponse, VerifyContext = false)

        public static Boolean TryParse(JObject            JSONObject,
                                       out SendCDRResult  SendCDRResult,
                                       out String         ErrorResponse,
                                       Boolean            VerifyContext = false)
        {

            try
            {

                SendCDRResult = null;

                if (JSONObject?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }


                #region Parse Context        [mandatory]

                if (VerifyContext)
                {

                    if (!JSONObject.ParseMandatory("@context",
                                                   "JSON-LD context",
                                                   out String Context,
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

                #region Parse Timestamp      [mandatory]

                if (!JSONObject.ParseMandatory("timestamp",
                                               "timestamp",
                                               out DateTime Timestamp,
                                               out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Result         [mandatory]

                if (!JSONObject.ParseMandatoryEnum("result",
                                                   "result",
                                                   out SendCDRResultTypes Result,
                                                   out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Description    [optional]

                if (JSONObject.ParseOptional("description",
                                             "description",
                                             out I18NString Description,
                                             out ErrorResponse))
                {

                    if (ErrorResponse != null)
                        return false;

                }

                #endregion


                #region Parse Runtime        [optional]

                if (JSONObject.ParseOptionalStruct("runtime",
                                                   "runtime",
                                                   TimeSpan.TryParse,
                                                   out TimeSpan? Runtime,
                                                   out ErrorResponse))
                {

                    if (ErrorResponse != null)
                        return false;

                }

                #endregion



                SendCDRResult = new SendCDRResult(Timestamp,
                                                  null,
                                                  Result,
                                                  Description,
                                                  null,
                                                  Runtime);

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
        /// The charging location is unknown.
        /// </summary>
        UnknownLocation,

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

}
