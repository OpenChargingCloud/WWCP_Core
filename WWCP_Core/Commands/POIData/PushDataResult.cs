/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    // PushEVSEData
    // PushChargingStationData
    // PushChargingPoolData
    // ...

    public class PushSingleEVSEDataResult
    {

        public IEVSE                      EVSE        { get; }
        public PushSingleDataResultTypes  Result      { get; }
        public IEnumerable<String>        Warnings    { get; }

        public PushSingleEVSEDataResult(IEVSE                      EVSE,
                                        PushSingleDataResultTypes  Result,
                                        IEnumerable<String>        Warnings)
        {

            this.EVSE      = EVSE;
            this.Result    = Result;
            this.Warnings  = Warnings is not null
                                 ? Warnings.Where     (warning => warning != null).
                                            SafeSelect(warning => warning.Trim()).
                                            Where     (warning => warning.IsNotNullOrEmpty())
                                 : Array.Empty<String>();

        }

    }

    public class PushSingleChargingStationDataResult
    {

        public IChargingStation           ChargingStation    { get; }
        public PushSingleDataResultTypes  Result             { get; }
        public IEnumerable<Warning>       Warnings           { get; }

        public PushSingleChargingStationDataResult(IChargingStation           ChargingStation,
                                                   PushSingleDataResultTypes  Result,
                                                   IEnumerable<Warning>?      Warnings   = null)
        {

            this.ChargingStation  = ChargingStation;
            this.Result           = Result;
            this.Warnings         = Warnings is not null
                                        ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                        : Array.Empty<Warning>();

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
        public ISendPOIData?         SendPOIData        { get; }

        /// <summary>
        /// The object implementing ReceivePOIData.
        /// </summary>
        public IReceivePOIData?      ReceivePOIData     { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushDataResultTypes   Result             { get; }

        /// <summary>
        /// The optional description of the result code.
        /// </summary>
        public String?               Description        { get; }

        /// <summary>
        /// The enumeration of successfully uploaded EVSEs.
        /// </summary>
        public IEnumerable<IEVSE>    SuccessfulEVSEs    { get; }

        /// <summary>
        /// The enumeration of rejected EVSEs.
        /// </summary>
        public IEnumerable<IEVSE>    RejectedEVSEs      { get; }

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

        #region PushEVSEDataResult(AuthId, SendPOIData,    Result, ...)

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
        internal PushEVSEDataResult(IId                    AuthId,
                                    ISendPOIData           SendPOIData,
                                    PushDataResultTypes    Result,
                                    String?                Description       = null,
                                    IEnumerable<IEVSE>?    SuccessfulEVSEs   = null,
                                    IEnumerable<IEVSE>?    RejectedEVSEs     = null,
                                    IEnumerable<Warning>?  Warnings          = null,
                                    TimeSpan?              Runtime           = null)
        {

            this.AuthId           = AuthId;
            this.SendPOIData      = SendPOIData;
            this.Result           = Result;

            this.Description      = Description is not null && Description.IsNotNullOrEmpty()
                                        ? Description.Trim()
                                        : null;

            this.SuccessfulEVSEs  = SuccessfulEVSEs is not null
                                        ? SuccessfulEVSEs.Where(evse    => evse is not null)
                                        : Array.Empty<IEVSE>();

            this.RejectedEVSEs    = RejectedEVSEs   is not null
                                        ? RejectedEVSEs.  Where(evse    => evse is not null)
                                        : Array.Empty<IEVSE>();

            this.Warnings         = Warnings        is not null
                                        ? Warnings.       Where(warning => warning.IsNeitherNullNorEmpty())
                                        : Array.Empty<Warning>();

            this.Runtime          = Runtime;

        }

        #endregion

        #region PushEVSEDataResult(AuthId, ReceivePOIData, Result, ...)

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
        internal PushEVSEDataResult(IId                    AuthId,
                                    IReceivePOIData        ReceivePOIData,
                                    PushDataResultTypes    Result,
                                    String?                Description       = null,
                                    IEnumerable<IEVSE>?    SuccessfulEVSEs   = null,
                                    IEnumerable<IEVSE>?    RejectedEVSEs     = null,
                                    IEnumerable<Warning>?  Warnings          = null,
                                    TimeSpan?              Runtime           = null)
        {

            this.AuthId           = AuthId;
            this.ReceivePOIData   = ReceivePOIData;
            this.Result           = Result;

            this.Description      = Description is not null && Description.IsNotNullOrEmpty()
                                        ? Description.Trim()
                                        : null;

            this.SuccessfulEVSEs  = SuccessfulEVSEs is not null
                                        ? SuccessfulEVSEs.Where(evse    => evse is not null)
                                        : Array.Empty<IEVSE>();

            this.RejectedEVSEs    = RejectedEVSEs   is not null
                                        ? RejectedEVSEs.  Where(evse    => evse is not null)
                                        : Array.Empty<IEVSE>();

            this.Warnings         = Warnings        is not null
                                        ? Warnings.       Where(warning => warning.IsNeitherNullNorEmpty())
                                        : Array.Empty<Warning>();

            this.Runtime          = Runtime;

        }

        #endregion

        #endregion


        #region (static) AdminDown

        public static PushEVSEDataResult

            AdminDown(IId                    AuthId,
                      ISendPOIData           SendPOIData,
                      IEnumerable<IEVSE>?    RejectedEVSEs   = null,
                      String?                Description     = null,
                      IEnumerable<Warning>?  Warnings        = null,
                      TimeSpan?              Runtime         = null)

                => new (AuthId,
                        SendPOIData,
                        PushDataResultTypes.AdminDown,
                        Description,
                        Array.Empty<EVSE>(),
                        RejectedEVSEs,
                        Warnings,
                        Runtime);


        public static PushEVSEDataResult

            AdminDown(IId                    AuthId,
                      IReceivePOIData        ReceivePOIData,
                      IEnumerable<IEVSE>?    RejectedEVSEs   = null,
                      String?                Description     = null,
                      IEnumerable<Warning>?  Warnings        = null,
                      TimeSpan?              Runtime         = null)

                => new (AuthId,
                        ReceivePOIData,
                        PushDataResultTypes.AdminDown,
                        Description,
                        Array.Empty<EVSE>(),
                        RejectedEVSEs,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success

        public static PushEVSEDataResult

            Success(IId                    AuthId,
                    ISendPOIData           SendPOIData,
                    IEnumerable<IEVSE>?    SuccessfulEVSEs   = null,
                    String?                Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (AuthId,
                        SendPOIData,
                        PushDataResultTypes.Success,
                        Description,
                        SuccessfulEVSEs,
                        Array.Empty<EVSE>(),
                        Warnings,
                        Runtime);


        public static PushEVSEDataResult

            Success(IId                    AuthId,
                    IReceivePOIData        ReceivePOIData,
                    IEnumerable<IEVSE>?    SuccessfulEVSEs   = null,
                    String?                Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (AuthId,
                        ReceivePOIData,
                        PushDataResultTypes.Success,
                        Description,
                        SuccessfulEVSEs,
                        Array.Empty<EVSE>(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Enqueued

        public static PushEVSEDataResult

            Enqueued(IId                    AuthId,
                     ISendPOIData           SendPOIData,
                     IEnumerable<IEVSE>?    EnqueuedEVSEs   = null,
                     String?                Description     = null,
                     IEnumerable<Warning>?  Warnings        = null,
                     TimeSpan?              Runtime         = null)

                => new (AuthId,
                        SendPOIData,
                        PushDataResultTypes.Enqueued,
                        Description,
                        EnqueuedEVSEs,
                        Array.Empty<EVSE>(),
                        Warnings,
                        Runtime);


        public static PushEVSEDataResult
            Enqueued(IId                    AuthId,
                     IReceivePOIData        ReceivePOIData,
                     IEnumerable<IEVSE>?    EnqueuedEVSEs   = null,
                     String?                Description     = null,
                     IEnumerable<Warning>?  Warnings        = null,
                     TimeSpan?              Runtime         = null)

                => new (AuthId,
                        ReceivePOIData,
                        PushDataResultTypes.Enqueued,
                        Description,
                        EnqueuedEVSEs,
                        Array.Empty<EVSE>(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation

        public static PushEVSEDataResult

            NoOperation(IId                    AuthId,
                        ISendPOIData           SendPOIData,
                        IEnumerable<IEVSE>?    RejectedEVSEs   = null,
                        String?                Description     = null,
                        IEnumerable<Warning>?  Warnings        = null,
                        TimeSpan?              Runtime         = null)

                => new (AuthId,
                        SendPOIData,
                        PushDataResultTypes.NoOperation,
                        Description,
                        Array.Empty<EVSE>(),
                        RejectedEVSEs,
                        Warnings,
                        Runtime);


         public static PushEVSEDataResult

            NoOperation(IId                    AuthId,
                        IReceivePOIData        ReceivePOIData,
                        IEnumerable<IEVSE>?    RejectedEVSEs   = null,
                        String?                Description     = null,
                        IEnumerable<Warning>?  Warnings        = null,
                        TimeSpan?              Runtime         = null)

                => new (AuthId,
                        ReceivePOIData,
                        PushDataResultTypes.NoOperation,
                        Description,
                        Array.Empty<EVSE>(),
                        RejectedEVSEs,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Timeout

        public static PushEVSEDataResult

            Timeout(IId                    AuthId,
                    ISendPOIData           SendPOIData,
                    IEnumerable<IEVSE>?    RejectedEVSEs,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)


                => new (AuthId,
                        SendPOIData,
                        PushDataResultTypes.Timeout,
                        Description,
                        Array.Empty<EVSE>(),
                        RejectedEVSEs,
                        Warnings,
                        Runtime: Runtime);

        #endregion

        #region (static) Error

        public static PushEVSEDataResult

            Error(IId                    AuthId,
                  ISendPOIData           SendPOIData,
                  IEnumerable<IEVSE>?    RejectedEVSEs   = null,
                  String?                Description     = null,
                  IEnumerable<Warning>?  Warnings        = null,
                  TimeSpan?              Runtime         = null)

            => new (AuthId,
                    SendPOIData,
                    PushDataResultTypes.Error,
                    Description,
                    Array.Empty<EVSE>(),
                    RejectedEVSEs,
                    Warnings,
                    Runtime);


        public static PushEVSEDataResult

            Error(IId                    AuthId,
                  IReceivePOIData        ReceivePOIData,
                  IEnumerable<IEVSE>?    RejectedEVSEs   = null,
                  String?                Description     = null,
                  IEnumerable<Warning>?  Warnings        = null,
                  TimeSpan?              Runtime         = null)

            => new (AuthId,
                    ReceivePOIData,
                    PushDataResultTypes.Error,
                    Description,
                    Array.Empty<EVSE>(),
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

        public ISendPOIData?                                     SendPOIData        { get; }

        public IReceivePOIData?                                  ReceivePOIData     { get; }

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
        public String?                                           Description        { get; }

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

        #region (private) PushChargingStationDataResult(Id,     SendPOIData,    Result,...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationDataResult(IId                                                Id,
                                              ISendPOIData                                       SendPOIData,
                                              PushDataResultTypes                                Result,
                                              IEnumerable<PushSingleChargingStationDataResult>?  RejectedEVSEs   = null,
                                              String?                                            Description     = null,
                                              IEnumerable<Warning>?                              Warnings        = null,
                                              TimeSpan?                                          Runtime         = null)
        {

            this.Id             = Id;

            this.SendPOIData    = SendPOIData;

            this.Result         = Result;

            this.RejectedEVSEs  = RejectedEVSEs ?? Array.Empty<PushSingleChargingStationDataResult>();

            this.Description    = Description is not null && Description.IsNotNullOrEmpty()
                                      ? Description.Trim()
                                      : null;

            this.Warnings       = Warnings    is not null
                                      ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                      : Array.Empty<Warning>();

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
        private PushChargingStationDataResult(IId                                                Id,
                                              IReceivePOIData                                    ReceivePOIData,
                                              PushDataResultTypes                                Result,
                                              IEnumerable<PushSingleChargingStationDataResult>?  RejectedEVSEs   = null,
                                              String?                                            Description     = null,
                                              IEnumerable<Warning>?                              Warnings        = null,
                                              TimeSpan?                                          Runtime         = null)
        {

            this.Id              = Id;

            this.ReceivePOIData  = ReceivePOIData;

            this.Result          = Result;

            this.RejectedEVSEs   = RejectedEVSEs ?? Array.Empty<PushSingleChargingStationDataResult>();

            this.Description     = Description is not null && Description.IsNotNullOrEmpty()
                                       ? Description.Trim()
                                       : null;

            this.Warnings        = Warnings != null
                                       ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                       : Array.Empty<Warning>();

            this.Runtime         = Runtime;

        }

        #endregion

        #endregion


        #region (static) AdminDown

        public static PushChargingStationDataResult

            AdminDown(IId                                                AuthId,
                      ISendPOIData                                       SendPOIData,
                      IEnumerable<PushSingleChargingStationDataResult>?  RejectedEVSEs   = null,
                      String?                                            Description     = null,
                      IEnumerable<Warning>?                              Warnings        = null,
                      TimeSpan?                                          Runtime         = null)

                => new (AuthId,
                        SendPOIData,
                        PushDataResultTypes.AdminDown,
                        RejectedEVSEs,
                        Description,
                        Warnings,
                        Runtime);


        public static PushChargingStationDataResult

            AdminDown(IId                                                AuthId,
                      IReceivePOIData                                    ReceivePOIData,
                      IEnumerable<PushSingleChargingStationDataResult>?  RejectedEVSEs   = null,
                      String?                                            Description     = null,
                      IEnumerable<Warning>?                              Warnings        = null,
                      TimeSpan?                                          Runtime         = null)

                => new (AuthId,
                        ReceivePOIData,
                        PushDataResultTypes.AdminDown,
                        RejectedEVSEs,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success

        public static PushChargingStationDataResult

            Success(IId                    AuthId,
                    ISendPOIData           SendPOIData,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

                => new (AuthId,
                        SendPOIData,
                        PushDataResultTypes.Success,
                        Array.Empty<PushSingleChargingStationDataResult>(),
                        Description,
                        Warnings,
                        Runtime);


        public static PushChargingStationDataResult

            Success(IId                    AuthId,
                    IReceivePOIData        ReceivePOIData,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

                => new (AuthId,
                        ReceivePOIData,
                        PushDataResultTypes.Success,
                        Array.Empty<PushSingleChargingStationDataResult>(),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Enqueued

        public static PushChargingStationDataResult

            Enqueued(IId                    AuthId,
                     ISendPOIData           SendPOIData,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

                => new (AuthId,
                        SendPOIData,
                        PushDataResultTypes.Enqueued,
                        Array.Empty<PushSingleChargingStationDataResult>(),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation

        public static PushChargingStationDataResult

            NoOperation(IId                    AuthId,
                        ISendPOIData           SendPOIData,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

                => new (AuthId,
                        SendPOIData,
                        PushDataResultTypes.NoOperation,
                        Array.Empty<PushSingleChargingStationDataResult>(),
                        Description,
                        Warnings,
                        Runtime);


         public static PushChargingStationDataResult

            NoOperation(IId                    AuthId,
                        IReceivePOIData        ReceivePOIData,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

                => new (AuthId,
                        ReceivePOIData,
                        PushDataResultTypes.NoOperation,
                        Array.Empty<PushSingleChargingStationDataResult>(),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error

        public static PushChargingStationDataResult

            Error(IId                                                AuthId,
                  ISendPOIData                                       SendPOIData,
                  IEnumerable<PushSingleChargingStationDataResult>?  RejectedEVSEs   = null,
                  String?                                            Description     = null,
                  IEnumerable<Warning>?                              Warnings        = null,
                  TimeSpan?                                          Runtime         = null)

                => new (AuthId,
                        SendPOIData,
                        PushDataResultTypes.Error,
                        RejectedEVSEs,
                        Description,
                        Warnings,
                        Runtime);


        public static PushChargingStationDataResult

            Error(IId                                                AuthId,
                  IReceivePOIData                                    ReceivePOIData,
                  IEnumerable<PushSingleChargingStationDataResult>?  RejectedEVSEs   = null,
                  String?                                            Description     = null,
                  IEnumerable<Warning>?                              Warnings        = null,
                  TimeSpan?                                          Runtime         = null)

                => new (AuthId,
                        ReceivePOIData,
                        PushDataResultTypes.Error,
                        RejectedEVSEs,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        public PushEVSEDataResult ToPushEVSEDataResult()

            => SendPOIData is not null

                   ? new PushEVSEDataResult(Id,
                                            SendPOIData,
                                            Result,
                                            Description,
                                            Array.Empty<EVSE>(),
                                            Array.Empty<EVSE>(),
                                            Warnings,
                                            Runtime)

                   : new PushEVSEDataResult(Id,
                                            ReceivePOIData,
                                            Result,
                                            Description,
                                            Array.Empty<EVSE>(),
                                            Array.Empty<EVSE>(),
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
