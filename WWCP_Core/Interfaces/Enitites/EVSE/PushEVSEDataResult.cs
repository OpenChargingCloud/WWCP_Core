///*
// * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
// *
// * Licensed under the Affero GPL license, Version 3.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.gnu.org/licenses/agpl.html
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using org.GraphDefined.Vanaheimr.Illias;
//using System.Linq;

//#endregion

//namespace cloud.charging.open.protocols.WWCP
//{

//    /// <summary>
//    /// A PushData result.
//    /// </summary>
//    public class PushEVSEDataResult
//    {

//        #region Properties

//        /// <summary>
//        /// The unqiue identification of the authenticator.
//        /// </summary>
//        public IId                                    AuthId             { get; }

//        /// <summary>
//        /// The object implementing SendPOIData.
//        /// </summary>
//        public ISendPOIData?                          SendPOIData        { get; }

//        /// <summary>
//        /// The object implementing ReceivePOIData.
//        /// </summary>
//        public IReceivePOIData?                       ReceivePOIData     { get; }

//        /// <summary>
//        /// The result of the operation.
//        /// </summary>
//        public PushDataResultTypes                    Result             { get; }

//        /// <summary>
//        /// The optional description of the result code.
//        /// </summary>
//        public String?                                Description        { get; }

//        /// <summary>
//        /// The enumeration of successfully uploaded EVSEs.
//        /// </summary>
//        public IEnumerable<PushSingleEVSEDataResult>  SuccessfulEVSEs    { get; }

//        /// <summary>
//        /// The enumeration of rejected EVSEs.
//        /// </summary>
//        public IEnumerable<PushSingleEVSEDataResult>  RejectedEVSEs      { get; }

//        /// <summary>
//        /// Warnings or additional information.
//        /// </summary>
//        public IEnumerable<Warning>                   Warnings           { get; }

//        /// <summary>
//        /// The runtime of the request.
//        /// </summary>
//        public TimeSpan?                              Runtime            { get;  }

//        #endregion

//        #region Constructor(s)

//        #region PushEVSEDataResult(AuthId, SendPOIData,    Result, ...)

//        /// <summary>
//        /// Create a new PushEVSEData result.
//        /// </summary>
//        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
//        /// <param name="SendPOIData">An object implementing SendPOIData.</param>
//        /// <param name="ReceivePOIData">An object implementing ReceivePOIData.</param>
//        /// <param name="Result">The result of the operation.</param>
//        /// <param name="Description">An optional description of the result code.</param>
//        /// <param name="SuccessfulEVSEs">An enumeration of successfully uploaded EVSEs.</param>
//        /// <param name="RejectedEVSEs">An enumeration of rejected EVSEs.</param>
//        /// <param name="Warnings">Warnings or additional information.</param>
//        /// <param name="Runtime">The runtime of the request.</param>
//        public PushEVSEDataResult(IId                                     AuthId,
//                                  ISendPOIData                            SendPOIData,
//                                  PushDataResultTypes                     Result,
//                                  IEnumerable<PushSingleEVSEDataResult>?  SuccessfulEVSEs   = null,
//                                  IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs     = null,
//                                  String?                                 Description       = null,
//                                  IEnumerable<Warning>?                   Warnings          = null,
//                                  TimeSpan?                               Runtime           = null)
//        {

//            this.AuthId           = AuthId;
//            this.SendPOIData      = SendPOIData;
//            this.Result           = Result;

//            this.Description      = Description is not null && Description.IsNotNullOrEmpty()
//                                        ? Description.Trim()
//                                        : null;

//            this.SuccessfulEVSEs  = SuccessfulEVSEs is not null
//                                        ? SuccessfulEVSEs.Where(evse    => evse is not null)
//                                        : Array.Empty<PushSingleEVSEDataResult>();

//            this.RejectedEVSEs    = RejectedEVSEs   is not null
//                                        ? RejectedEVSEs.  Where(evse    => evse is not null)
//                                        : Array.Empty<PushSingleEVSEDataResult>();

//            this.Warnings         = Warnings        is not null
//                                        ? Warnings.       Where(warning => warning.IsNeitherNullNorEmpty())
//                                        : Array.Empty<Warning>();

//            this.Runtime          = Runtime;

//        }

//        #endregion

//        #region PushEVSEDataResult(AuthId, ReceivePOIData, Result, ...)

//        /// <summary>
//        /// Create a new PushEVSEData result.
//        /// </summary>
//        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
//        /// <param name="SendPOIData">An object implementing SendPOIData.</param>
//        /// <param name="ReceivePOIData">An object implementing ReceivePOIData.</param>
//        /// <param name="Result">The result of the operation.</param>
//        /// <param name="Description">An optional description of the result code.</param>
//        /// <param name="SuccessfulEVSEs">An enumeration of successfully uploaded EVSEs.</param>
//        /// <param name="RejectedEVSEs">An enumeration of rejected EVSEs.</param>
//        /// <param name="Warnings">Warnings or additional information.</param>
//        /// <param name="Runtime">The runtime of the request.</param>
//        public PushEVSEDataResult(IId                                     AuthId,
//                                  IReceivePOIData                         ReceivePOIData,
//                                  PushDataResultTypes                     Result,
//                                  IEnumerable<PushSingleEVSEDataResult>?  SuccessfulEVSEs   = null,
//                                  IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs     = null,
//                                  String?                                 Description       = null,
//                                  IEnumerable<Warning>?                   Warnings          = null,
//                                  TimeSpan?                               Runtime           = null)
//        {

//            this.AuthId           = AuthId;
//            this.ReceivePOIData   = ReceivePOIData;
//            this.Result           = Result;

//            this.Description      = Description is not null && Description.IsNotNullOrEmpty()
//                                        ? Description.Trim()
//                                        : null;

//            this.SuccessfulEVSEs  = SuccessfulEVSEs is not null
//                                        ? SuccessfulEVSEs.Where(evse    => evse is not null)
//                                        : Array.Empty<PushSingleEVSEDataResult>();

//            this.RejectedEVSEs    = RejectedEVSEs   is not null
//                                        ? RejectedEVSEs.  Where(evse    => evse is not null)
//                                        : Array.Empty<PushSingleEVSEDataResult>();

//            this.Warnings         = Warnings        is not null
//                                        ? Warnings.       Where(warning => warning.IsNeitherNullNorEmpty())
//                                        : Array.Empty<Warning>();

//            this.Runtime          = Runtime;

//        }

//        #endregion

//        #endregion


//        #region (static) AdminDown

//        public static PushEVSEDataResult

//            AdminDown(IId                    AuthId,
//                      ISendPOIData           SendPOIData,
//                      IEnumerable<IEVSE>     RejectedEVSEs,
//                      String?                Description   = null,
//                      IEnumerable<Warning>?  Warnings      = null,
//                      TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.AdminDown,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushEVSEDataResult

//            AdminDown(IId                    AuthId,
//                      IReceivePOIData        ReceivePOIData,
//                      IEnumerable<IEVSE>     RejectedEVSEs,
//                      String?                Description   = null,
//                      IEnumerable<Warning>?  Warnings      = null,
//                      TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        ReceivePOIData,
//                        PushDataResultTypes.AdminDown,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) Success

//        public static PushEVSEDataResult

//            Success(IId                    AuthId,
//                    ISendPOIData           SendPOIData,
//                    IEnumerable<IEVSE>     SuccessfulEVSEs,
//                    String?                Description       = null,
//                    IEnumerable<Warning>?  Warnings          = null,
//                    TimeSpan?              Runtime           = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Success,
//                        SuccessfulEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushEVSEDataResult

//            Success(IId                    AuthId,
//                    IReceivePOIData        ReceivePOIData,
//                    IEnumerable<IEVSE>     SuccessfulEVSEs,
//                    String?                Description       = null,
//                    IEnumerable<Warning>?  Warnings          = null,
//                    TimeSpan?              Runtime           = null)

//                => new (AuthId,
//                        ReceivePOIData,
//                        PushDataResultTypes.Success,
//                        SuccessfulEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) Enqueued

//        public static PushEVSEDataResult

//            Enqueued(IId                    AuthId,
//                     ISendPOIData           SendPOIData,
//                     IEnumerable<IEVSE>     EnqueuedEVSEs,
//                     String?                Description   = null,
//                     IEnumerable<Warning>?  Warnings      = null,
//                     TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Enqueued,
//                        EnqueuedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Enqueued, Warnings)),
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushEVSEDataResult
//            Enqueued(IId                    AuthId,
//                     IReceivePOIData        ReceivePOIData,
//                     IEnumerable<IEVSE>     EnqueuedEVSEs,
//                     String?                Description   = null,
//                     IEnumerable<Warning>?  Warnings      = null,
//                     TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        ReceivePOIData,
//                        PushDataResultTypes.Enqueued,
//                        EnqueuedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Enqueued, Warnings)),
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) NoOperation

//        public static PushEVSEDataResult

//            NoOperation(IId                    AuthId,
//                        ISendPOIData           SendPOIData,
//                        IEnumerable<IEVSE>     RejectedEVSEs,
//                        String?                Description   = null,
//                        IEnumerable<Warning>?  Warnings      = null,
//                        TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.NoOperation,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//         public static PushEVSEDataResult

//            NoOperation(IId                    AuthId,
//                        IReceivePOIData        ReceivePOIData,
//                        IEnumerable<IEVSE>     RejectedEVSEs,
//                        String?                Description   = null,
//                        IEnumerable<Warning>?  Warnings      = null,
//                        TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        ReceivePOIData,
//                        PushDataResultTypes.NoOperation,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) LockTimeout

//        public static PushEVSEDataResult

//            LockTimeout(IId                    AuthId,
//                        ISendPOIData           SendPOIData,
//                        IEnumerable<IEVSE>     RejectedEVSEs,
//                        String?                Description   = null,
//                        IEnumerable<Warning>?  Warnings      = null,
//                        TimeSpan?              Runtime       = null)


//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.LockTimeout,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Timeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Timeout

//        public static PushEVSEDataResult

//            Timeout(IId                    AuthId,
//                    ISendPOIData           SendPOIData,
//                    IEnumerable<IEVSE>     RejectedEVSEs,
//                    String?                Description   = null,
//                    IEnumerable<Warning>?  Warnings      = null,
//                    TimeSpan?              Runtime       = null)


//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Timeout,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Timeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Error

//        public static PushEVSEDataResult

//            Error(IId                                     AuthId,
//                  ISendPOIData                            SendPOIData,
//                  IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs   = null,
//                  String?                                 Description     = null,
//                  IEnumerable<Warning>?                   Warnings        = null,
//                  TimeSpan?                               Runtime         = null)

//            => new (AuthId,
//                    SendPOIData,
//                    PushDataResultTypes.Error,
//                    Array.Empty<PushSingleEVSEDataResult>(),
//                    RejectedEVSEs,
//                    Description,
//                    Warnings,
//                    Runtime);


//        public static PushEVSEDataResult

//            Error(IId                                     AuthId,
//                  IReceivePOIData                         ReceivePOIData,
//                  IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs   = null,
//                  String?                                 Description     = null,
//                  IEnumerable<Warning>?                   Warnings        = null,
//                  TimeSpan?                               Runtime         = null)

//            => new (AuthId,
//                    ReceivePOIData,
//                    PushDataResultTypes.Error,
//                    Array.Empty<PushSingleEVSEDataResult>(),
//                    RejectedEVSEs,
//                    Description,
//                    Warnings,
//                    Runtime);

//        #endregion


//        #region (override) ToString()

//        /// <summary>
//        /// Return a text representation of this object.
//        /// </summary>
//        public override String ToString()

//            => String.Concat("Result: " + Result + "; " + Description);

//        #endregion

//    }

//}
