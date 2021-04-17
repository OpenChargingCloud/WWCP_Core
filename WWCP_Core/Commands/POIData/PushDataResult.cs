/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    // PushEVSEData
    // PushChargingStationData
    // PushChargingPoolData
    // ...

    public class PushSingleEVSEDataResult
    {

        public EVSE                       EVSE        { get; }
        public PushSingleDataResultTypes  Result      { get; }
        public IEnumerable<String>        Warnings    { get; }

        public PushSingleEVSEDataResult(EVSE                       EVSE,
                                        PushSingleDataResultTypes  Result,
                                        IEnumerable<String>        Warnings)
        {

            this.EVSE      = EVSE;
            this.Result    = Result;
            this.Warnings  = Warnings != null
                                 ? Warnings.Where     (warning => warning != null).
                                            SafeSelect(warning => warning.Trim()).
                                            Where     (warning => warning.IsNotNullOrEmpty())
                                 : new String[0];

        }

    }

    public class PushSingleChargingStationDataResult
    {

        public ChargingStation            ChargingStation    { get; }
        public PushSingleDataResultTypes  Result             { get; }
        public IEnumerable<Warning>       Warnings           { get; }

        public PushSingleChargingStationDataResult(ChargingStation            ChargingStation,
                                                   PushSingleDataResultTypes  Result,
                                                   IEnumerable<Warning>       Warnings)
        {

            this.ChargingStation  = ChargingStation;
            this.Result           = Result;
            this.Warnings         = Warnings != null
                                        ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                        : new Warning[0];

        }

    }


    /// <summary>
    /// A PushData result.
    /// </summary>
    public class PushEVSEDataResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                   AuthId             { get; }

        /// <summary>
        /// The object implementing SendPOIData.
        /// </summary>
        public ISendPOIData          SendPOIData        { get; }

        /// <summary>
        /// The object implementing ReceivePOIData.
        /// </summary>
        public IReceivePOIData       ReceivePOIData     { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushDataResultTypes   Result             { get; }

        /// <summary>
        /// The optional description of the result code.
        /// </summary>
        public String                Description        { get; }

        /// <summary>
        /// The enumeration of successfully uploaded EVSEs.
        /// </summary>
        public IEnumerable<EVSE>     SuccessfulEVSEs    { get; }

        /// <summary>
        /// The enumeration of rejected EVSEs.
        /// </summary>
        public IEnumerable<EVSE>     RejectedEVSEs      { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>  Warnings           { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?             Runtime            { get;  }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PushEVSEData result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="SendPOIData">An object implementing SendPOIData.</param>
        /// <param name="ReceivePOIData">An object implementing ReceivePOIData.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="SuccessfulEVSEs">An enumeration of successfully uploaded EVSEs.</param>
        /// <param name="RejectedEVSEs">An enumeration of rejected EVSEs.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEVSEDataResult(IId                   AuthId,
                                    ISendPOIData          SendPOIData,
                                    IReceivePOIData       ReceivePOIData,
                                    PushDataResultTypes   Result,
                                    String                Description       = null,
                                    IEnumerable<EVSE> SuccessfulEVSEs = null,
                                    IEnumerable<EVSE>     RejectedEVSEs     = null,
                                    IEnumerable<Warning>  Warnings          = null,
                                    TimeSpan?             Runtime           = null)
        {

            this.AuthId           = AuthId;
            this.SendPOIData        = SendPOIData;
            this.ReceivePOIData     = ReceivePOIData;
            this.Result           = Result;

            this.Description      = Description.IsNotNullOrEmpty()
                                        ? Description.Trim()
                                        : null;

            this.SuccessfulEVSEs  = SuccessfulEVSEs != null
                                       ? SuccessfulEVSEs.Where(evse    => evse != null)
                                       : new EVSE[0];

            this.RejectedEVSEs    = RejectedEVSEs   != null
                                        ? RejectedEVSEs. Where(evse    => evse != null)
                                        : new EVSE[0];

            this.Warnings         = Warnings        != null
                                        ? Warnings.      Where(warning => warning.IsNeitherNullNorEmpty())
                                        : new Warning[0];

            this.Runtime          = Runtime;

        }

        #endregion


        #region (static) AdminDown

        public static PushEVSEDataResult

            AdminDown(IId                   AuthId,
                      ISendPOIData          SendPOIData,
                      IEnumerable<EVSE>     RejectedEVSEs   = null,
                      String                Description     = null,
                      IEnumerable<Warning>  Warnings        = null,
                      TimeSpan?             Runtime         = null)

                => new PushEVSEDataResult(AuthId,
                                          SendPOIData,
                                          null,
                                          PushDataResultTypes.AdminDown,
                                          Description,
                                          new EVSE[0],
                                          RejectedEVSEs,
                                          Warnings,
                                          Runtime);


        public static PushEVSEDataResult

            AdminDown(IId                   AuthId,
                      IReceivePOIData       ReceivePOIData,
                      IEnumerable<EVSE>     RejectedEVSEs   = null,
                      String                Description     = null,
                      IEnumerable<Warning>  Warnings        = null,
                      TimeSpan?             Runtime         = null)

                => new PushEVSEDataResult(AuthId,
                                          null,
                                          ReceivePOIData,
                                          PushDataResultTypes.AdminDown,
                                          Description,
                                          new EVSE[0],
                                          RejectedEVSEs,
                                          Warnings,
                                          Runtime);

        #endregion

        #region (static) Success

        public static PushEVSEDataResult

            Success(IId                   AuthId,
                    ISendPOIData          SendPOIData,
                    IEnumerable<EVSE>     SuccessfulEVSEs   = null,
                    String                Description       = null,
                    IEnumerable<Warning>  Warnings          = null,
                    TimeSpan?             Runtime           = null)

                => new PushEVSEDataResult(AuthId,
                                          SendPOIData,
                                          null,
                                          PushDataResultTypes.Success,
                                          Description,
                                          SuccessfulEVSEs,
                                          new EVSE[0],
                                          Warnings,
                                          Runtime);


        public static PushEVSEDataResult

            Success(IId                   AuthId,
                    IReceivePOIData       ReceivePOIData,
                    IEnumerable<EVSE>     SuccessfulEVSEs   = null,
                    String                Description       = null,
                    IEnumerable<Warning>  Warnings          = null,
                    TimeSpan?             Runtime           = null)

                => new PushEVSEDataResult(AuthId,
                                          null,
                                          ReceivePOIData,
                                          PushDataResultTypes.Success,
                                          Description,
                                          SuccessfulEVSEs,
                                          new EVSE[0],
                                          Warnings,
                                          Runtime);

        #endregion

        #region (static) Enqueued

        public static PushEVSEDataResult

            Enqueued(IId                   AuthId,
                     ISendPOIData          SendPOIData,
                     IEnumerable<EVSE>     EnqueuedEVSEs   = null,
                     String                Description     = null,
                     IEnumerable<Warning>  Warnings        = null,
                     TimeSpan?             Runtime         = null)

                => new PushEVSEDataResult(AuthId,
                                          SendPOIData,
                                          null,
                                          PushDataResultTypes.Enqueued,
                                          Description,
                                          EnqueuedEVSEs,
                                          new EVSE[0],
                                          Warnings,
                                          Runtime);


        public static PushEVSEDataResult
            Enqueued(IId                   AuthId,
                     IReceivePOIData       ReceivePOIData,
                     IEnumerable<EVSE>     EnqueuedEVSEs   = null,
                     String                Description     = null,
                     IEnumerable<Warning>  Warnings        = null,
                     TimeSpan?             Runtime         = null)

                => new PushEVSEDataResult(AuthId,
                                          null,
                                          ReceivePOIData,
                                          PushDataResultTypes.Enqueued,
                                          Description,
                                          EnqueuedEVSEs,
                                          new EVSE[0],
                                          Warnings,
                                          Runtime);

        #endregion

        #region (static) NoOperation

        public static PushEVSEDataResult

            NoOperation(IId                   AuthId,
                        ISendPOIData          SendPOIData,
                        IEnumerable<EVSE>     RejectedEVSEs   = null,
                        String                Description     = null,
                        IEnumerable<Warning>  Warnings        = null,
                        TimeSpan?             Runtime         = null)

                => new PushEVSEDataResult(AuthId,
                                          SendPOIData,
                                          null,
                                          PushDataResultTypes.NoOperation,
                                          Description,
                                          new EVSE[0],
                                          RejectedEVSEs,
                                          Warnings,
                                          Runtime);


         public static PushEVSEDataResult

            NoOperation(IId                   AuthId,
                        IReceivePOIData       ReceivePOIData,
                        IEnumerable<EVSE>     RejectedEVSEs   = null,
                        String                Description     = null,
                        IEnumerable<Warning>  Warnings        = null,
                        TimeSpan?             Runtime         = null)

                => new PushEVSEDataResult(AuthId,
                                          null,
                                          ReceivePOIData,
                                          PushDataResultTypes.NoOperation,
                                          Description,
                                          new EVSE[0],
                                          RejectedEVSEs,
                                          Warnings,
                                          Runtime);

        #endregion

        #region (static) Timeout

        public static PushEVSEDataResult

            Timeout(IId                   AuthId,
                    ISendPOIData          SendPOIData,
                    IEnumerable<EVSE>     RejectedEVSEs,
                    String                Description   = null,
                    IEnumerable<Warning>  Warnings      = null,
                    TimeSpan?             Runtime       = null)


                => new PushEVSEDataResult(AuthId,
                                          SendPOIData,
                                          null,
                                          PushDataResultTypes.Timeout,
                                          Description,
                                          new EVSE[0],
                                          RejectedEVSEs,
                                          Warnings,
                                          Runtime: Runtime);

        #endregion

        #region (static) Error

        public static PushEVSEDataResult

            Error(IId                   AuthId,
                  ISendPOIData          SendPOIData,
                  IEnumerable<EVSE>     RejectedEVSEs  = null,
                  String                Description    = null,
                  IEnumerable<Warning>  Warnings       = null,
                  TimeSpan?             Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                      SendPOIData,
                                      null,
                                      PushDataResultTypes.Error,
                                      Description,
                                      new EVSE[0],
                                      RejectedEVSEs,
                                      Warnings,
                                      Runtime);


        public static PushEVSEDataResult

            Error(IId                   AuthId,
                  IReceivePOIData       ReceivePOIData,
                  IEnumerable<EVSE>     RejectedEVSEs  = null,
                  String                Description    = null,
                  IEnumerable<Warning>  Warnings       = null,
                  TimeSpan?             Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                      null,
                                      ReceivePOIData,
                                      PushDataResultTypes.Error,
                                      Description,
                                      new EVSE[0],
                                      RejectedEVSEs,
                                      Warnings,
                                      Runtime);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }


    /// <summary>
    /// A PushData result.
    /// </summary>
    public class PushChargingStationDataResult
    {

        #region Properties

        public IId                                               Id                 { get; }

        public ISendPOIData                                         SendPOIData          { get; }

        public IReceivePOIData                                      ReceivePOIData       { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs      { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushDataResultTypes                               Result             { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String                                            Description        { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                              Warnings           { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                                         Runtime            { get;  }

        #endregion

        #region Constructor(s)

        #region (private) PushChargingStationDataResult(Id, SendPOIData,    Result,...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationDataResult(IId                                               Id,
                                              ISendPOIData                                         SendPOIData,
                                              PushDataResultTypes                               Result,
                                              IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                                              String                                            Description     = null,
                                              IEnumerable<Warning>                              Warnings        = null,
                                              TimeSpan?                                         Runtime         = null)
        {

            this.Id             = Id;

            this.SendPOIData      = SendPOIData;

            this.Result         = Result;

            this.RejectedEVSEs  = RejectedEVSEs ?? new PushSingleChargingStationDataResult[0];

            this.Description    = Description.IsNotNullOrEmpty()
                                      ? Description.Trim()
                                      : null;

            this.Warnings       = Warnings != null
                                      ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                      : new Warning[0];

            this.Runtime        = Runtime;

        }

        #endregion

        #region (private) PushChargingStationDataResult(AuthId, ReceivePOIData, Result,...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationDataResult(IId                                               Id,
                                              IReceivePOIData                                      ReceivePOIData,
                                              PushDataResultTypes                               Result,
                                              IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                                              String                                            Description     = null,
                                              IEnumerable<Warning>                              Warnings        = null,
                                              TimeSpan?                                         Runtime         = null)
        {

            this.Id             = Id;

            this.ReceivePOIData   = ReceivePOIData;

            this.Result         = Result;

            this.RejectedEVSEs  = RejectedEVSEs ?? new PushSingleChargingStationDataResult[0];

            this.Description    = Description.IsNotNullOrEmpty()
                                      ? Description.Trim()
                                      : null;

            this.Warnings       = Warnings != null
                                      ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                      : new Warning[0];

            this.Runtime        = Runtime;

        }

        #endregion

        #endregion


        #region (static) AdminDown

        public static PushChargingStationDataResult

            AdminDown(IId                                               AuthId,
                      ISendPOIData                                      SendPOIData,
                      IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                      String                                            Description     = null,
                      IEnumerable<Warning>                              Warnings        = null,
                      TimeSpan?                                         Runtime         = null)

                => new PushChargingStationDataResult(AuthId,
                                                     SendPOIData,
                                                     PushDataResultTypes.AdminDown,
                                                     RejectedEVSEs,
                                                     Description,
                                                     Warnings,
                                                     Runtime);


        public static PushChargingStationDataResult

            AdminDown(IId                                               AuthId,
                      IReceivePOIData                                   ReceivePOIData,
                      IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                      String                                            Description     = null,
                      IEnumerable<Warning>                              Warnings        = null,
                      TimeSpan?                                         Runtime         = null)

                => new PushChargingStationDataResult(AuthId,
                                                     ReceivePOIData,
                                                     PushDataResultTypes.AdminDown,
                                                     RejectedEVSEs,
                                                     Description,
                                                     Warnings,
                                                     Runtime);

        #endregion

        #region (static) Success

        public static PushChargingStationDataResult

            Success(IId                   AuthId,
                    ISendPOIData          SendPOIData,
                    String                Description   = null,
                    IEnumerable<Warning>  Warnings      = null,
                    TimeSpan?             Runtime       = null)

                => new PushChargingStationDataResult(AuthId,
                                                     SendPOIData,
                                                     PushDataResultTypes.Success,
                                                     new PushSingleChargingStationDataResult[0],
                                                     Description,
                                                     Warnings,
                                                     Runtime);


        public static PushChargingStationDataResult

            Success(IId                   AuthId,
                    IReceivePOIData       ReceivePOIData,
                    String                Description   = null,
                    IEnumerable<Warning>  Warnings      = null,
                    TimeSpan?             Runtime       = null)

                => new PushChargingStationDataResult(AuthId,
                                                     ReceivePOIData,
                                                     PushDataResultTypes.Success,
                                                     new PushSingleChargingStationDataResult[0],
                                                     Description,
                                                     Warnings,
                                                     Runtime);

        #endregion

        #region (static) Enqueued

        public static PushChargingStationDataResult

            Enqueued(IId                   AuthId,
                     ISendPOIData          SendPOIData,
                     String                Description   = null,
                     IEnumerable<Warning>  Warnings      = null,
                     TimeSpan?             Runtime       = null)

                => new PushChargingStationDataResult(AuthId,
                                                     SendPOIData,
                                                     PushDataResultTypes.Enqueued,
                                                     new PushSingleChargingStationDataResult[0],
                                                     Description,
                                                     Warnings,
                                                     Runtime);

        #endregion

        #region (static) NoOperation

        public static PushChargingStationDataResult

            NoOperation(IId                   AuthId,
                        ISendPOIData          SendPOIData,
                        String                Description   = null,
                        IEnumerable<Warning>  Warnings      = null,
                        TimeSpan?             Runtime       = null)

                => new PushChargingStationDataResult(AuthId,
                                                     SendPOIData,
                                                     PushDataResultTypes.NoOperation,
                                                     new PushSingleChargingStationDataResult[0],
                                                     Description,
                                                     Warnings,
                                                     Runtime);


         public static PushChargingStationDataResult

            NoOperation(IId                   AuthId,
                        IReceivePOIData       ReceivePOIData,
                        String                Description   = null,
                        IEnumerable<Warning>  Warnings      = null,
                        TimeSpan?             Runtime       = null)

                => new PushChargingStationDataResult(AuthId,
                                                     ReceivePOIData,
                                                     PushDataResultTypes.NoOperation,
                                                     new PushSingleChargingStationDataResult[0],
                                                     Description,
                                                     Warnings,
                                                     Runtime);

        #endregion

        #region (static) Error

        public static PushChargingStationDataResult

            Error(IId                                               AuthId,
                  ISendPOIData                                      SendPOIData,
                  IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                  String                                            Description     = null,
                  IEnumerable<Warning>                              Warnings        = null,
                  TimeSpan?                                         Runtime         = null)

                => new PushChargingStationDataResult(AuthId,
                                                     SendPOIData,
                                                     PushDataResultTypes.Error,
                                                     RejectedEVSEs,
                                                     Description,
                                                     Warnings,
                                                     Runtime);


        public static PushChargingStationDataResult

            Error(IId                                               AuthId,
                  IReceivePOIData                                   ReceivePOIData,
                  IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                  String                                            Description     = null,
                  IEnumerable<Warning>                              Warnings        = null,
                  TimeSpan?                                         Runtime         = null)

                => new PushChargingStationDataResult(AuthId,
                                                     ReceivePOIData,
                                                     PushDataResultTypes.Error,
                                                     RejectedEVSEs,
                                                     Description,
                                                     Warnings,
                                                     Runtime);

        #endregion


        public PushEVSEDataResult ToPushEVSEDataResult()

            => SendPOIData != null

                   ? new PushEVSEDataResult(Id,
                                            SendPOIData,
                                            null,
                                            Result,
                                            Description,
                                            new EVSE[0],
                                            new EVSE[0],
                                            Warnings,
                                            Runtime)

                   : new PushEVSEDataResult(Id,
                                            null,
                                            ReceivePOIData,
                                            Result,
                                            Description,
                                            new EVSE[0],
                                            new EVSE[0],
                                            Warnings,
                                            Runtime);


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }


    public enum PushDataResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        /// <summary>
        /// No operation e.g. because no EVSE data passed the EVSE filter.
        /// </summary>
        NoOperation,

        /// <summary>
        /// The data has been enqueued for later transmission.
        /// </summary>
        Enqueued,

        /// <summary>
        /// Success.
        /// </summary>
        Success,

        /// <summary>
        /// Out-Of-Service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// A timeout occured.
        /// </summary>
        Timeout,

        /// <summary>
        /// The operation led to an error.
        /// </summary>
        Error

    }

    public enum PushSingleDataResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        /// <summary>
        /// No operation e.g. because no EVSE data passed the EVSE filter.
        /// </summary>
        NoOperation,

        /// <summary>
        /// The data has been enqueued for later transmission.
        /// </summary>
        Enqueued,

        /// <summary>
        /// Success.
        /// </summary>
        Success,

        /// <summary>
        /// Out-Of-Service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// A timeout occured.
        /// </summary>
        Timeout,

        /// <summary>
        /// Error.
        /// </summary>
        Error

    }

}
