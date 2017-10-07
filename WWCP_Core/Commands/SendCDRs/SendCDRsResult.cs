/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{


    public class SendCDRResult
    {

        public ChargeDetailRecord   ChargeDetailRecord    { get; }
        public SendCDRResultTypes   Result                { get; }
        public IEnumerable<String>  Warnings              { get; }

        public SendCDRResult(ChargeDetailRecord   ChargeDetailRecord,
                             SendCDRResultTypes   Result,
                             String               Warning)

            : this(ChargeDetailRecord,
                   Result,
                   Warning.IsNeitherNullNorEmpty()
                       ? new String[] { Warning }
                       : null)

        { }

        public SendCDRResult(ChargeDetailRecord   ChargeDetailRecord,
                             SendCDRResultTypes   Result,
                             IEnumerable<String>  Warnings = null)
        {

            this.ChargeDetailRecord  = ChargeDetailRecord;
            this.Result              = Result;
            this.Warnings            = Warnings != null
                                           ? Warnings.Where     (warning => warning != null).
                                                      SafeSelect(warning => warning.Trim()).
                                                      Where     (warning => warning.IsNotNullOrEmpty())
                                           : new String[0];

        }

    }


    /// <summary>
    /// The result of sending charge detail records.
    /// </summary>
    public class SendCDRsResult
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
        /// Optional status information about rejected charge detail records.
        /// </summary>
        public IEnumerable<SendCDRResult>   RejectedChargeDetailRecords    { get; }

        /// <summary>
        /// An optional description of the send charge detail records result.
        /// </summary>
        public String                       Description                    { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<String>          Warnings                       { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                    Runtime                        { get; }

        #endregion

        #region Constructor(s)

        #region (private) SendCDRsResult(AuthorizatorId, ISendChargeDetailRecords,    Result, ...)

        /// <summary>
        /// Create a new send charge detail records result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="Result">The result of the charge detail record transmission.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private SendCDRsResult(IId                         AuthorizatorId,
                               ISendChargeDetailRecords    ISendChargeDetailRecords,
                               SendCDRsResultTypes         Result,
                               IEnumerable<SendCDRResult>  RejectedChargeDetailRecords   = null,
                               String                      Description                   = null,
                               IEnumerable<String>         Warnings                      = null,
                               TimeSpan?                   Runtime                       = null)
        {

            this.AuthorizatorId               = AuthorizatorId;
            this.ISendChargeDetailRecords     = ISendChargeDetailRecords;
            this.Result                       = Result;
            this.RejectedChargeDetailRecords  = RejectedChargeDetailRecords ?? new SendCDRResult[0];
            this.Description                  = Description;
            this.Warnings                     = Warnings != null
                                                    ? Warnings.Where     (warning => warning != null).
                                                               SafeSelect(warning => warning.Trim()).
                                                               Where     (warning => warning.IsNotNullOrEmpty())
                                                    : new String[0];
            this.Runtime                      = Runtime;

        }

        #endregion

        #region (private) SendCDRsResult(AuthorizatorId, IReceiveChargeDetailRecords, Result, ...)

        /// <summary>
        /// Create a new send charge detail records result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record receiving entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="Result">The result of the charge detail record transmission.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private SendCDRsResult(IId                          AuthorizatorId,
                               IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                               SendCDRsResultTypes          Result,
                               IEnumerable<SendCDRResult>   RejectedChargeDetailRecords   = null,
                               String                       Description                   = null,
                               IEnumerable<String>          Warnings                      = null,
                               TimeSpan?                    Runtime                       = null)
        {

            this.AuthorizatorId               = AuthorizatorId;
            this.IReceiveChargeDetailRecords  = IReceiveChargeDetailRecords;
            this.Result                       = Result;
            this.RejectedChargeDetailRecords  = RejectedChargeDetailRecords ?? new SendCDRResult[0];
            this.Description                  = Description;
            this.Warnings                     = Warnings != null
                                                    ? Warnings.Where     (warning => warning != null).
                                                               SafeSelect(warning => warning.Trim()).
                                                               Where     (warning => warning.IsNotNullOrEmpty())
                                                    : new String[0];
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
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            NoOperation(IId                         AuthorizatorId,
                        ISendChargeDetailRecords    ISendChargeDetailRecords,
                        IEnumerable<SendCDRResult>  RejectedChargeDetailRecords   = null,
                        String                      Description                   = null,
                        IEnumerable<String>         Warnings                      = null,
                        TimeSpan?                   Runtime                       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.NoOperation,
                                      RejectedChargeDetailRecords,
                                      Description,
                                      Warnings,
                                      Runtime);



        /// <summary>
        /// No operation e.g. because no charge detail record passed the charge detail records filter.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            NoOperation(IId                          AuthorizatorId,
                        IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                        IEnumerable<SendCDRResult>   RejectedChargeDetailRecords   = null,
                        String                       Description                   = null,
                        IEnumerable<String>          Warnings                      = null,
                        TimeSpan?                    Runtime                       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.NoOperation,
                                      RejectedChargeDetailRecords,
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion

        #region (static) AdminDown   (AuthorizatorId, ...)

        /// <summary>
        /// The service is administratively down.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            AdminDown(IId                         AuthorizatorId,
                      ISendChargeDetailRecords    ISendChargeDetailRecords,
                      IEnumerable<SendCDRResult>  RejectedChargeDetailRecords,
                      String                      Description   = null,
                      IEnumerable<String>         Warnings      = null,
                      TimeSpan?                   Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.AdminDown,
                                      RejectedChargeDetailRecords,
                                      Description,
                                      Warnings,
                                      Runtime);

        /// <summary>
        /// The service is administratively down.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            AdminDown(IId                              AuthorizatorId,
                      ISendChargeDetailRecords         ISendChargeDetailRecords,
                      IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords,
                      String                           Description   = null,
                      IEnumerable<String>              Warnings      = null,
                      TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.AdminDown,
                                      RejectedChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.AdminDown)),
                                      Description,
                                      Warnings,
                                      Runtime);



        /// <summary>
        /// The service is administratively down.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            AdminDown(IId                          AuthorizatorId,
                      IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                      IEnumerable<SendCDRResult>   RejectedChargeDetailRecords,
                      String                       Description   = null,
                      IEnumerable<String>          Warnings      = null,
                      TimeSpan?                    Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.AdminDown,
                                      RejectedChargeDetailRecords,
                                      Description,
                                      Warnings,
                                      Runtime);

        /// <summary>
        /// The service is administratively down.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            AdminDown(IId                              AuthorizatorId,
                      IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                      IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords,
                      String                           Description   = null,
                      IEnumerable<String>              Warnings      = null,
                      TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.AdminDown,
                                      RejectedChargeDetailRecords.Select(cdr => new SendCDRResult(cdr, SendCDRResultTypes.AdminDown)),
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion

        #region (static) OutOfService(AuthorizatorId, ...)

        /// <summary>
        /// The service is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            OutOfService(IId                         AuthorizatorId,
                         ISendChargeDetailRecords    ISendChargeDetailRecords,
                         IEnumerable<SendCDRResult>  RejectedChargeDetailRecords,
                         String                      Description   = null,
                         IEnumerable<String>         Warnings      = null,
                         TimeSpan?                   Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.OutOfService,
                                      RejectedChargeDetailRecords,
                                      Description,
                                      Warnings,
                                      Runtime);

        /// <summary>
        /// The service is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="ISendChargeDetailRecords">The entity sending charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            OutOfService(IId                              AuthorizatorId,
                         ISendChargeDetailRecords         ISendChargeDetailRecords,
                         IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords,
                         String                           Description   = null,
                         IEnumerable<String>              Warnings      = null,
                         TimeSpan?                        Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.OutOfService,
                                      RejectedChargeDetailRecords.Select(cdr => new SendCDRResult(cdr, SendCDRResultTypes.OutOfService)),
                                      Description,
                                      Warnings,
                                      Runtime);



        /// <summary>
        /// The service is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            OutOfService(IId                          AuthorizatorId,
                         IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                         IEnumerable<SendCDRResult>   RejectedChargeDetailRecords,
                         String                       Description   = null,
                         IEnumerable<String>          Warnings      = null,
                         TimeSpan?                    Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.OutOfService,
                                      RejectedChargeDetailRecords,
                                      Description,
                                      Warnings,
                                      Runtime);

        /// <summary>
        /// The service is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            OutOfService(IId                              AuthorizatorId,
                         IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                         IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords,
                         String                           Description   = null,
                         IEnumerable<String>              Warnings      = null,
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
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Enqueued(IId                         AuthorizatorId,
                     ISendChargeDetailRecords    ISendChargeDetailRecords,
                     String                      Description                  = null,
                     IEnumerable<SendCDRResult>  RejectedChargeDetailRecords  = null,
                     IEnumerable<String>         Warnings                     = null,
                     TimeSpan?                   Runtime                      = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Enqueued,
                                      RejectedChargeDetailRecords ?? new SendCDRResult[0],
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Enqueued(IId                          AuthorizatorId,
                     IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                     String                       Description                  = null,
                     IEnumerable<SendCDRResult>   RejectedChargeDetailRecords  = null,
                     IEnumerable<String>          Warnings                     = null,
                     TimeSpan?                    Runtime                      = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Enqueued,
                                      RejectedChargeDetailRecords ?? new SendCDRResult[0],
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
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Timeout(IId                         AuthorizatorId,
                    ISendChargeDetailRecords    ISendChargeDetailRecords,
                    String                      Description                  = null,
                    IEnumerable<SendCDRResult>  RejectedChargeDetailRecords  = null,
                    IEnumerable<String>         Warnings                     = null,
                    TimeSpan?                   Runtime                      = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Timeout,
                                      RejectedChargeDetailRecords ?? new SendCDRResult[0],
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
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Success(IId                       AuthorizatorId,
                    ISendChargeDetailRecords  ISendChargeDetailRecords,
                    String                    Description   = null,
                    IEnumerable<String>       Warnings      = null,
                    TimeSpan?                 Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      new SendCDRResult[0],
                                      Description,
                                      Warnings,
                                      Runtime);


        /// <summary>
        /// The request completed successfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Success(IId                          AuthorizatorId,
                    IReceiveChargeDetailRecords  IReceiveChargeDetailRecords,
                    String                       Description   = null,
                    IEnumerable<String>          Warnings      = null,
                    TimeSpan?                    Runtime       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      new SendCDRResult[0],
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
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(IId                              AuthorizatorId,
                  ISendChargeDetailRecords         ISendChargeDetailRecords,
                  IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords   = null,
                  String                           Description                   = null,
                  IEnumerable<String>              Warnings                      = null,
                  TimeSpan?                        Runtime                       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      ISendChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      RejectedChargeDetailRecords.Select(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Error)),
                                      Description,
                                      Warnings,
                                      Runtime);

        /// <summary>
        /// The request completed unsuccessfully.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the charge detail record sending entity.</param>
        /// <param name="IReceiveChargeDetailRecords">The entity receiving charge detail records.</param>
        /// <param name="RejectedChargeDetailRecords">Optional status information about rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static SendCDRsResult

            Error(IId                              AuthorizatorId,
                  IReceiveChargeDetailRecords      IReceiveChargeDetailRecords,
                  IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords   = null,
                  String                           Description                   = null,
                  IEnumerable<String>              Warnings                      = null,
                  TimeSpan?                        Runtime                       = null)


                => new SendCDRsResult(AuthorizatorId,
                                      IReceiveChargeDetailRecords,
                                      SendCDRsResultTypes.Success,
                                      RejectedChargeDetailRecords.Select(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Error)),
                                      Description,
                                      Warnings,
                                      Runtime);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
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
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        /// <summary>
        /// The service is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        Enqueued,

        /// <summary>
        /// A timeout occured.
        /// </summary>
        Timeout,




        /// <summary>
        /// The operation led to an error.
        /// </summary>
        Error,

        Success

    }

    public enum SendCDRResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        AdminDown,

        Filtered,

        Enqueued,

        Success,

        InvalidSessionId,

        CouldNotConvertCDRFormat,

        UnknownEVSE,

        Timeout,


        /// <summary>
        /// The service is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The operation led to an error.
        /// </summary>
        Error

    }

}
