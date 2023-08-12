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

//#endregion

//namespace cloud.charging.open.protocols.WWCP
//{

//    /// <summary>
//    /// A PushData result.
//    /// </summary>
//    public class SendRoamingNetworkResult
//    {

//        #region Properties

//        /// <summary>
//        /// The unqiue identification of the authenticator.
//        /// </summary>
//        public IId                                    AuthId             { get; }

//        /// <summary>
//        /// The object implementing SendPOIData.
//        /// </summary>
//        public ISendRoamingNetworkData?               SendPOIData        { get; }

//        /// <summary>
//        /// The object implementing ReceivePOIData.
//        /// </summary>
//        public IReceivePOIData?                       ReceivePOIData     { get; }

//        /// <summary>
//        /// The result of the operation.
//        /// </summary>
//        public SendRoamingNetworkResultTypes          Result             { get; }

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

//        #region SendRoamingNetworkResult(AuthId, SendPOIData,    Result, ...)

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
//        public SendRoamingNetworkResult(IId                                     AuthId,
//                                        ISendRoamingNetworkData                 SendPOIData,
//                                        SendRoamingNetworkResultTypes           Result,
//                                        IEnumerable<PushSingleEVSEDataResult>?  SuccessfulEVSEs   = null,
//                                        IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs     = null,
//                                        String?                                 Description       = null,
//                                        IEnumerable<Warning>?                   Warnings          = null,
//                                        TimeSpan?                               Runtime           = null)
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

//        #region SendRoamingNetworkResult(AuthId, ReceivePOIData, Result, ...)

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
//        public SendRoamingNetworkResult(IId                                     AuthId,
//                                        IReceivePOIData                         ReceivePOIData,
//                                        SendRoamingNetworkResultTypes           Result,
//                                        IEnumerable<PushSingleEVSEDataResult>?  SuccessfulEVSEs   = null,
//                                        IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs     = null,
//                                        String?                                 Description       = null,
//                                        IEnumerable<Warning>?                   Warnings          = null,
//                                        TimeSpan?                               Runtime           = null)
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

//        public static SendRoamingNetworkResult

//            AdminDown(IId                      AuthId,
//                      ISendRoamingNetworkData  SendPOIData,
//                      IEnumerable<IEVSE>       RejectedEVSEs,
//                      String?                  Description   = null,
//                      IEnumerable<Warning>?    Warnings      = null,
//                      TimeSpan?                Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        SendRoamingNetworkResultTypes.AdminDown,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static SendRoamingNetworkResult

//            AdminDown(IId                    AuthId,
//                      IReceivePOIData        ReceivePOIData,
//                      IEnumerable<IEVSE>     RejectedEVSEs,
//                      String?                Description   = null,
//                      IEnumerable<Warning>?  Warnings      = null,
//                      TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        ReceivePOIData,
//                        SendRoamingNetworkResultTypes.AdminDown,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) Success

//        public static SendRoamingNetworkResult

//            Success(IId                    AuthId,
//                    ISendRoamingNetworkData           SendPOIData,
//                    IEnumerable<IEVSE>     SuccessfulEVSEs,
//                    String?                Description       = null,
//                    IEnumerable<Warning>?  Warnings          = null,
//                    TimeSpan?              Runtime           = null)

//                => new (AuthId,
//                        SendPOIData,
//                        SendRoamingNetworkResultTypes.Success,
//                        SuccessfulEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static SendRoamingNetworkResult

//            Success(IId                    AuthId,
//                    IReceivePOIData        ReceivePOIData,
//                    IEnumerable<IEVSE>     SuccessfulEVSEs,
//                    String?                Description       = null,
//                    IEnumerable<Warning>?  Warnings          = null,
//                    TimeSpan?              Runtime           = null)

//                => new (AuthId,
//                        ReceivePOIData,
//                        SendRoamingNetworkResultTypes.Success,
//                        SuccessfulEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) Enqueued

//        public static SendRoamingNetworkResult

//            Enqueued(IId                    AuthId,
//                     ISendRoamingNetworkData           SendPOIData,
//                     IEnumerable<IEVSE>     EnqueuedEVSEs,
//                     String?                Description   = null,
//                     IEnumerable<Warning>?  Warnings      = null,
//                     TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        SendRoamingNetworkResultTypes.Enqueued,
//                        EnqueuedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Enqueued, Warnings)),
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static SendRoamingNetworkResult
//            Enqueued(IId                    AuthId,
//                     IReceivePOIData        ReceivePOIData,
//                     IEnumerable<IEVSE>     EnqueuedEVSEs,
//                     String?                Description   = null,
//                     IEnumerable<Warning>?  Warnings      = null,
//                     TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        ReceivePOIData,
//                        SendRoamingNetworkResultTypes.Enqueued,
//                        EnqueuedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Enqueued, Warnings)),
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) NoOperation

//        public static SendRoamingNetworkResult

//            NoOperation(IId                    AuthId,
//                        ISendRoamingNetworkData           SendPOIData,
//                        IEnumerable<IEVSE>     RejectedEVSEs,
//                        String?                Description   = null,
//                        IEnumerable<Warning>?  Warnings      = null,
//                        TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        SendRoamingNetworkResultTypes.NoOperation,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//         public static SendRoamingNetworkResult

//            NoOperation(IId                    AuthId,
//                        IReceivePOIData        ReceivePOIData,
//                        IEnumerable<IEVSE>     RejectedEVSEs,
//                        String?                Description   = null,
//                        IEnumerable<Warning>?  Warnings      = null,
//                        TimeSpan?              Runtime       = null)

//                => new (AuthId,
//                        ReceivePOIData,
//                        SendRoamingNetworkResultTypes.NoOperation,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) LockTimeout

//        public static SendRoamingNetworkResult

//            LockTimeout(IId                    AuthId,
//                        ISendRoamingNetworkData           SendPOIData,
//                        IEnumerable<IEVSE>     RejectedEVSEs,
//                        String?                Description   = null,
//                        IEnumerable<Warning>?  Warnings      = null,
//                        TimeSpan?              Runtime       = null)


//                => new (AuthId,
//                        SendPOIData,
//                        SendRoamingNetworkResultTypes.LockTimeout,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Timeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Timeout

//        public static SendRoamingNetworkResult

//            Timeout(IId                    AuthId,
//                    ISendRoamingNetworkData           SendPOIData,
//                    IEnumerable<IEVSE>     RejectedEVSEs,
//                    String?                Description   = null,
//                    IEnumerable<Warning>?  Warnings      = null,
//                    TimeSpan?              Runtime       = null)


//                => new (AuthId,
//                        SendPOIData,
//                        SendRoamingNetworkResultTypes.Timeout,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Timeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Error

//        public static SendRoamingNetworkResult

//            Error(IId                                     AuthId,
//                  ISendRoamingNetworkData                            SendPOIData,
//                  IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs   = null,
//                  String?                                 Description     = null,
//                  IEnumerable<Warning>?                   Warnings        = null,
//                  TimeSpan?                               Runtime         = null)

//            => new (AuthId,
//                    SendPOIData,
//                    SendRoamingNetworkResultTypes.Error,
//                    Array.Empty<PushSingleEVSEDataResult>(),
//                    RejectedEVSEs,
//                    Description,
//                    Warnings,
//                    Runtime);


//        public static SendRoamingNetworkResult

//            Error(IId                                     AuthId,
//                  IReceivePOIData                         ReceivePOIData,
//                  IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs   = null,
//                  String?                                 Description     = null,
//                  IEnumerable<Warning>?                   Warnings        = null,
//                  TimeSpan?                               Runtime         = null)

//            => new (AuthId,
//                    ReceivePOIData,
//                    SendRoamingNetworkResultTypes.Error,
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



//    public enum SendRoamingNetworkResultTypes
//    {

//        /// <summary>
//        /// The result is unknown and/or should be ignored.
//        /// </summary>
//        Unspecified,

//        /// <summary>
//        /// The service was disabled by the administrator.
//        /// </summary>
//        AdminDown,

//        /// <summary>
//        /// No operation e.g. because no EVSE data passed the EVSE filter.
//        /// </summary>
//        NoOperation,

//        /// <summary>
//        /// The data has been enqueued for later transmission.
//        /// </summary>
//        Enqueued,

//        /// <summary>
//        /// Success.
//        /// </summary>
//        Success,

//        /// <summary>
//        /// Out-Of-Service.
//        /// </summary>
//        OutOfService,

//        /// <summary>
//        /// A lock timeout occured.
//        /// </summary>
//        LockTimeout,

//        /// <summary>
//        /// A timeout occured.
//        /// </summary>
//        Timeout,

//        /// <summary>
//        /// The operation led to an error.
//        /// </summary>
//        Error

//    }

//}
