///*
// * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

//    // PushEVSEData
//    // PushChargingStationData
//    // PushChargingPoolData
//    // ...

//    public class PushSingleEVSEDataResult
//    {

//        public IEVSE                      EVSE        { get; }
//        public PushSingleDataResultTypes  Result      { get; }
//        public IEnumerable<Warning>       Warnings    { get; }

//        public PushSingleEVSEDataResult(IEVSE                      EVSE,
//                                        PushSingleDataResultTypes  Result,
//                                        IEnumerable<Warning>?      Warnings   = null)
//        {

//            this.EVSE      = EVSE;
//            this.Result    = Result;
//            this.Warnings  = Warnings is not null
//                                 ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
//                                 : Array.Empty<Warning>();

//        }

//    }

//    public class PushSingleChargingStationDataResult
//    {

//        public IChargingStation           ChargingStation    { get; }
//        public PushSingleDataResultTypes  Result             { get; }
//        public IEnumerable<Warning>       Warnings           { get; }

//        public PushSingleChargingStationDataResult(IChargingStation           ChargingStation,
//                                                   PushSingleDataResultTypes  Result,
//                                                   IEnumerable<Warning>?      Warnings   = null)
//        {

//            this.ChargingStation  = ChargingStation;
//            this.Result           = Result;
//            this.Warnings         = Warnings is not null
//                                        ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
//                                        : Array.Empty<Warning>();

//        }

//    }

//    public class PushSingleChargingPoolDataResult
//    {

//        public IChargingPool              ChargingPool    { get; }
//        public PushSingleDataResultTypes  Result          { get; }
//        public IEnumerable<Warning>       Warnings        { get; }

//        public PushSingleChargingPoolDataResult(IChargingPool              ChargingPool,
//                                                PushSingleDataResultTypes  Result,
//                                                IEnumerable<Warning>?      Warnings   = null)
//        {

//            this.ChargingPool  = ChargingPool;
//            this.Result        = Result;
//            this.Warnings      = Warnings is not null
//                                     ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
//                                     : Array.Empty<Warning>();

//        }

//    }


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
//        /// The object implementing Sender.
//        /// </summary>
//        public ISender?                          Sender        { get; }

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

//        #region PushEVSEDataResult(SenderId, Sender,    Result, ...)

//        /// <summary>
//        /// Create a new PushEVSEData result.
//        /// </summary>
//        /// <param name="SenderId">The unqiue identification of the sender.</param>
//        /// <param name="Sender">An object implementing Sender.</param>
//        /// <param name="ReceivePOIData">An object implementing ReceivePOIData.</param>
//        /// <param name="Result">The result of the operation.</param>
//        /// <param name="Description">An optional description of the result code.</param>
//        /// <param name="SuccessfulEVSEs">An enumeration of successfully uploaded EVSEs.</param>
//        /// <param name="RejectedEVSEs">An enumeration of rejected EVSEs.</param>
//        /// <param name="Warnings">Warnings or additional information.</param>
//        /// <param name="Runtime">The runtime of the request.</param>
//        public PushEVSEDataResult(IId                                     SenderId,
//                                  ISender                            Sender,
//                                  PushDataResultTypes                     Result,
//                                  IEnumerable<PushSingleEVSEDataResult>?  SuccessfulEVSEs   = null,
//                                  IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs     = null,
//                                  String?                                 Description       = null,
//                                  IEnumerable<Warning>?                   Warnings          = null,
//                                  TimeSpan?                               Runtime           = null)
//        {

//            this.AuthId           = AuthId;
//            this.Sender      = Sender;
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

//        #region PushEVSEDataResult(SenderId, ReceivePOIData, Result, ...)

//        /// <summary>
//        /// Create a new PushEVSEData result.
//        /// </summary>
//        /// <param name="SenderId">The unqiue identification of the sender.</param>
//        /// <param name="Sender">An object implementing Sender.</param>
//        /// <param name="ReceivePOIData">An object implementing ReceivePOIData.</param>
//        /// <param name="Result">The result of the operation.</param>
//        /// <param name="Description">An optional description of the result code.</param>
//        /// <param name="SuccessfulEVSEs">An enumeration of successfully uploaded EVSEs.</param>
//        /// <param name="RejectedEVSEs">An enumeration of rejected EVSEs.</param>
//        /// <param name="Warnings">Warnings or additional information.</param>
//        /// <param name="Runtime">The runtime of the request.</param>
//        public PushEVSEDataResult(IId                                     SenderId,
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

//            AdminDown(IId                    SenderId,
//                      ISender           Sender,
//                      IEnumerable<IEVSE>     RejectedEVSEs,
//                      String?                Description   = null,
//                      IEnumerable<Warning>?  Warnings      = null,
//                      TimeSpan?              Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.AdminDown,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushEVSEDataResult

//            AdminDown(IId                    SenderId,
//                      IReceivePOIData        ReceivePOIData,
//                      IEnumerable<IEVSE>     RejectedEVSEs,
//                      String?                Description   = null,
//                      IEnumerable<Warning>?  Warnings      = null,
//                      TimeSpan?              Runtime       = null)

//                => new (SenderId,
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

//            Success(IId                    SenderId,
//                    ISender           Sender,
//                    IEnumerable<IEVSE>     SuccessfulEVSEs,
//                    String?                Description       = null,
//                    IEnumerable<Warning>?  Warnings          = null,
//                    TimeSpan?              Runtime           = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Success,
//                        SuccessfulEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushEVSEDataResult

//            Success(IId                    SenderId,
//                    IReceivePOIData        ReceivePOIData,
//                    IEnumerable<IEVSE>     SuccessfulEVSEs,
//                    String?                Description       = null,
//                    IEnumerable<Warning>?  Warnings          = null,
//                    TimeSpan?              Runtime           = null)

//                => new (SenderId,
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

//            Enqueued(IId                    SenderId,
//                     ISender           Sender,
//                     IEnumerable<IEVSE>     EnqueuedEVSEs,
//                     String?                Description   = null,
//                     IEnumerable<Warning>?  Warnings      = null,
//                     TimeSpan?              Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Enqueued,
//                        EnqueuedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Enqueued, Warnings)),
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushEVSEDataResult
//            Enqueued(IId                    SenderId,
//                     IReceivePOIData        ReceivePOIData,
//                     IEnumerable<IEVSE>     EnqueuedEVSEs,
//                     String?                Description   = null,
//                     IEnumerable<Warning>?  Warnings      = null,
//                     TimeSpan?              Runtime       = null)

//                => new (SenderId,
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

//            NoOperation(IId                    SenderId,
//                        ISender           Sender,
//                        IEnumerable<IEVSE>     RejectedEVSEs,
//                        String?                Description   = null,
//                        IEnumerable<Warning>?  Warnings      = null,
//                        TimeSpan?              Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.NoOperation,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//         public static PushEVSEDataResult

//            NoOperation(IId                    SenderId,
//                        IReceivePOIData        ReceivePOIData,
//                        IEnumerable<IEVSE>     RejectedEVSEs,
//                        String?                Description   = null,
//                        IEnumerable<Warning>?  Warnings      = null,
//                        TimeSpan?              Runtime       = null)

//                => new (SenderId,
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

//            LockTimeout(IId                    SenderId,
//                        ISender           Sender,
//                        IEnumerable<IEVSE>     RejectedEVSEs,
//                        String?                Description   = null,
//                        IEnumerable<Warning>?  Warnings      = null,
//                        TimeSpan?              Runtime       = null)


//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.LockTimeout,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Timeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Timeout

//        public static PushEVSEDataResult

//            Timeout(IId                    SenderId,
//                    ISender           Sender,
//                    IEnumerable<IEVSE>     RejectedEVSEs,
//                    String?                Description   = null,
//                    IEnumerable<Warning>?  Warnings      = null,
//                    TimeSpan?              Runtime       = null)


//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Timeout,
//                        Array.Empty<PushSingleEVSEDataResult>(),
//                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Timeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Error

//        public static PushEVSEDataResult

//            Error(IId                                     SenderId,
//                  ISender                            Sender,
//                  IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs   = null,
//                  String?                                 Description     = null,
//                  IEnumerable<Warning>?                   Warnings        = null,
//                  TimeSpan?                               Runtime         = null)

//            => new (SenderId,
//                    Sender,
//                    PushDataResultTypes.Error,
//                    Array.Empty<PushSingleEVSEDataResult>(),
//                    RejectedEVSEs,
//                    Description,
//                    Warnings,
//                    Runtime);


//        public static PushEVSEDataResult

//            Error(IId                                     SenderId,
//                  IReceivePOIData                         ReceivePOIData,
//                  IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs   = null,
//                  String?                                 Description     = null,
//                  IEnumerable<Warning>?                   Warnings        = null,
//                  TimeSpan?                               Runtime         = null)

//            => new (SenderId,
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


//    /// <summary>
//    /// A PushData result.
//    /// </summary>
//    public class PushChargingStationDataResult
//    {

//        #region Properties
//        public IId                                               Id                            { get; }

//        public ISender?                                     Sender                   { get; }

//        public IReceivePOIData?                                  ReceivePOIData                { get; }

//        /// <summary>
//        /// The enumeration of successfully uploaded charging stations.
//        /// </summary>
//        public IEnumerable<PushSingleChargingStationDataResult>  SuccessfulChargingStations    { get; }

//        /// <summary>
//        /// Warnings or additional information.
//        /// </summary>
//        public IEnumerable<PushSingleChargingStationDataResult>  RejectedChargingStations      { get; }

//        /// <summary>
//        /// The result of the operation.
//        /// </summary>
//        public PushDataResultTypes                               Result                        { get; }

//        /// <summary>
//        /// An optional description of the result code.
//        /// </summary>
//        public String?                                           Description                   { get; }

//        /// <summary>
//        /// Warnings or additional information.
//        /// </summary>
//        public IEnumerable<Warning>                              Warnings                      { get; }

//        /// <summary>
//        /// The runtime of the request.
//        /// </summary>
//        public TimeSpan?                                         Runtime                       { get;  }

//        #endregion

//        #region Constructor(s)

//        #region PushChargingStationDataResult(Id,     Sender,    Result,...)

//        /// <summary>
//        /// Create a new acknowledgement.
//        /// </summary>
//        /// <param name="Result">The result of the operation.</param>
//        /// <param name="Description">An optional description of the result code.</param>
//        /// <param name="Warnings">Warnings or additional information.</param>
//        /// <param name="Runtime">The runtime of the request.</param>
//        public PushChargingStationDataResult(IId                                                Id,
//                                             ISender                                       Sender,
//                                             PushDataResultTypes                                Result,
//                                             IEnumerable<PushSingleChargingStationDataResult>?  SuccessfulChargingStations   = null,
//                                             IEnumerable<PushSingleChargingStationDataResult>?  RejectedChargingStations     = null,
//                                             String?                                            Description                  = null,
//                                             IEnumerable<Warning>?                              Warnings                     = null,
//                                             TimeSpan?                                          Runtime                      = null)
//        {

//            this.Id                          = Id;

//            this.Sender                 = Sender;

//            this.Result                      = Result;

//            this.SuccessfulChargingStations  = SuccessfulChargingStations ?? Array.Empty<PushSingleChargingStationDataResult>();

//            this.RejectedChargingStations    = RejectedChargingStations   ?? Array.Empty<PushSingleChargingStationDataResult>();

//            this.Description                 = Description is not null && Description.IsNotNullOrEmpty()
//                                                   ? Description.Trim()
//                                                   : null;

//            this.Warnings                    = Warnings    is not null
//                                                   ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
//                                                   : Array.Empty<Warning>();

//            this.Runtime                     = Runtime;

//        }

//        #endregion

//        #region PushChargingStationDataResult(SenderId, ReceivePOIData, Result,...)

//        /// <summary>
//        /// Create a new acknowledgement.
//        /// </summary>
//        /// <param name="Result">The result of the operation.</param>
//        /// <param name="Description">An optional description of the result code.</param>
//        /// <param name="Warnings">Warnings or additional information.</param>
//        /// <param name="Runtime">The runtime of the request.</param>
//        public PushChargingStationDataResult(IId                                                Id,
//                                             IReceivePOIData                                    ReceivePOIData,
//                                             PushDataResultTypes                                Result,
//                                             IEnumerable<PushSingleChargingStationDataResult>?  SuccessfulChargingStations   = null,
//                                             IEnumerable<PushSingleChargingStationDataResult>?  RejectedChargingStations     = null,
//                                             String?                                            Description                  = null,
//                                             IEnumerable<Warning>?                              Warnings                     = null,
//                                             TimeSpan?                                          Runtime                      = null)
//        {

//            this.Id                          = Id;

//            this.ReceivePOIData              = ReceivePOIData;

//            this.Result                      = Result;

//            this.SuccessfulChargingStations  = SuccessfulChargingStations ?? Array.Empty<PushSingleChargingStationDataResult>();

//            this.RejectedChargingStations    = RejectedChargingStations   ?? Array.Empty<PushSingleChargingStationDataResult>();

//            this.Description                 = Description is not null && Description.IsNotNullOrEmpty()
//                                                   ? Description.Trim()
//                                                   : null;

//            this.Warnings                    = Warnings != null
//                                                   ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
//                                                   : Array.Empty<Warning>();

//            this.Runtime                     = Runtime;

//        }

//        #endregion

//        #endregion


//        #region (static) AdminDown

//        public static PushChargingStationDataResult

//            AdminDown(IId                            SenderId,
//                      ISender                   Sender,
//                      IEnumerable<IChargingStation>  RejectedChargingStations,
//                      String?                        Description   = null,
//                      IEnumerable<Warning>?          Warnings      = null,
//                      TimeSpan?                      Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.AdminDown,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingStationDataResult

//            AdminDown(IId                            SenderId,
//                      IReceivePOIData                ReceivePOIData,
//                      IEnumerable<IChargingStation>  RejectedChargingStations,
//                      String?                        Description   = null,
//                      IEnumerable<Warning>?          Warnings      = null,
//                      TimeSpan?                      Runtime       = null)

//                => new (SenderId,
//                        ReceivePOIData,
//                        PushDataResultTypes.AdminDown,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) Success

//        public static PushChargingStationDataResult

//            Success(IId                            SenderId,
//                    ISender                   Sender,
//                    IEnumerable<IChargingStation>  SuccessfulChargingStations,
//                    String?                        Description   = null,
//                    IEnumerable<Warning>?          Warnings      = null,
//                    TimeSpan?                      Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Success,
//                        SuccessfulChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingStationDataResult

//            Success(IId                            SenderId,
//                    IReceivePOIData                ReceivePOIData,
//                    IEnumerable<IChargingStation>  SuccessfulChargingStations,
//                    String?                        Description   = null,
//                    IEnumerable<Warning>?          Warnings      = null,
//                    TimeSpan?                      Runtime       = null)

//                => new (SenderId,
//                        ReceivePOIData,
//                        PushDataResultTypes.Success,
//                        SuccessfulChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) Enqueued

//        public static PushChargingStationDataResult

//            Enqueued(IId                            SenderId,
//                     ISender                   Sender,
//                     IEnumerable<IChargingStation>  SuccessfulChargingStations,
//                     String?                        Description   = null,
//                     IEnumerable<Warning>?          Warnings      = null,
//                     TimeSpan?                      Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Enqueued,
//                        SuccessfulChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.Enqueued, Warnings)),
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) NoOperation

//        public static PushChargingStationDataResult

//            NoOperation(IId                            SenderId,
//                        ISender                   Sender,
//                        IEnumerable<IChargingStation>  RejectedChargingStations,
//                        String?                        Description   = null,
//                        IEnumerable<Warning>?          Warnings      = null,
//                        TimeSpan?                      Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.NoOperation,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//         public static PushChargingStationDataResult

//            NoOperation(IId                            SenderId,
//                        IReceivePOIData                ReceivePOIData,
//                        IEnumerable<IChargingStation>  RejectedChargingStations,
//                        String?                        Description   = null,
//                        IEnumerable<Warning>?          Warnings      = null,
//                        TimeSpan?                      Runtime       = null)

//                => new (SenderId,
//                        ReceivePOIData,
//                        PushDataResultTypes.NoOperation,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) LockTimeout

//        public static PushChargingStationDataResult

//            LockTimeout(IId                            SenderId,
//                        ISender                   Sender,
//                        IEnumerable<IChargingStation>  RejectedChargingStations,
//                        String?                        Description   = null,
//                        IEnumerable<Warning>?          Warnings      = null,
//                        TimeSpan?                      Runtime       = null)


//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Timeout,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.LockTimeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Timeout

//        public static PushChargingStationDataResult

//            Timeout(IId                            SenderId,
//                    ISender                   Sender,
//                    IEnumerable<IChargingStation>  RejectedChargingStations,
//                    String?                        Description   = null,
//                    IEnumerable<Warning>?          Warnings      = null,
//                    TimeSpan?                      Runtime       = null)


//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Timeout,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.Timeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Error

//        public static PushChargingStationDataResult

//            Error(IId                                               SenderId,
//                  ISender                                      Sender,
//                  IEnumerable<PushSingleChargingStationDataResult>  RejectedChargingStations,
//                  String?                                           Description     = null,
//                  IEnumerable<Warning>?                             Warnings        = null,
//                  TimeSpan?                                         Runtime         = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Error,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations,
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingStationDataResult

//            Error(IId                                               SenderId,
//                  IReceivePOIData                                   ReceivePOIData,
//                  IEnumerable<PushSingleChargingStationDataResult>  RejectedChargingStations,
//                  String?                                           Description     = null,
//                  IEnumerable<Warning>?                             Warnings        = null,
//                  TimeSpan?                                         Runtime         = null)

//                => new (SenderId,
//                        ReceivePOIData,
//                        PushDataResultTypes.Error,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations,
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion


//        public PushEVSEDataResult ToPushEVSEDataResult()

//            => Sender is not null

//                   ? new PushEVSEDataResult(Id,
//                                            Sender,
//                                            Result,
//                                            Array.Empty<PushSingleEVSEDataResult>(),
//                                            Array.Empty<PushSingleEVSEDataResult>(),
//                                            Description,
//                                            Warnings,
//                                            Runtime)

//                   : new PushEVSEDataResult(Id,
//                                            ReceivePOIData,
//                                            Result,
//                                            Array.Empty<PushSingleEVSEDataResult>(),
//                                            Array.Empty<PushSingleEVSEDataResult>(),
//                                            Description,
//                                            Warnings,
//                                            Runtime);


//        #region (override) ToString()

//        /// <summary>
//        /// Return a text representation of this object.
//        /// </summary>
//        public override String ToString()

//            => String.Concat("Result: " + Result + "; " + Description);

//        #endregion

//    }


//    /// <summary>
//    /// A PushData result.
//    /// </summary>
//    public class PushChargingPoolDataResult
//    {

//        #region Properties

//        public IId                                               Id                         { get; }

//        public ISender?                                     Sender                { get; }

//        public IReceivePOIData?                                  ReceivePOIData             { get; }

//        /// <summary>
//        /// The enumeration of successfully uploaded charging pools.
//        /// </summary>
//        public IEnumerable<PushSingleChargingPoolDataResult>     SuccessfulChargingPools    { get; }

//        /// <summary>
//        /// Warnings or additional information.
//        /// </summary>
//        public IEnumerable<PushSingleChargingPoolDataResult>     RejectedChargingPools      { get; }

//        /// <summary>
//        /// The result of the operation.
//        /// </summary>
//        public PushDataResultTypes                               Result                     { get; }

//        /// <summary>
//        /// An optional description of the result code.
//        /// </summary>
//        public String?                                           Description                { get; }

//        /// <summary>
//        /// Warnings or additional information.
//        /// </summary>
//        public IEnumerable<Warning>                              Warnings                   { get; }

//        /// <summary>
//        /// The runtime of the request.
//        /// </summary>
//        public TimeSpan?                                         Runtime                    { get;  }

//        #endregion

//        #region Constructor(s)

//        #region PushChargingPoolDataResult(Id,     Sender,    Result,...)

//        /// <summary>
//        /// Create a new acknowledgement.
//        /// </summary>
//        /// <param name="Result">The result of the operation.</param>
//        /// <param name="Description">An optional description of the result code.</param>
//        /// <param name="Warnings">Warnings or additional information.</param>
//        /// <param name="Runtime">The runtime of the request.</param>
//        public PushChargingPoolDataResult(IId                                             Id,
//                                          ISender                                    Sender,
//                                          PushDataResultTypes                             Result,
//                                          IEnumerable<PushSingleChargingPoolDataResult>?  SuccessfulChargingPools   = null,
//                                          IEnumerable<PushSingleChargingPoolDataResult>?  RejectedChargingPools     = null,
//                                          String?                                         Description               = null,
//                                          IEnumerable<Warning>?                           Warnings                  = null,
//                                          TimeSpan?                                       Runtime                   = null)
//        {

//            this.Id                       = Id;

//            this.Sender              = Sender;

//            this.Result                   = Result;

//            this.SuccessfulChargingPools  = SuccessfulChargingPools ?? Array.Empty<PushSingleChargingPoolDataResult>();

//            this.RejectedChargingPools    = RejectedChargingPools   ?? Array.Empty<PushSingleChargingPoolDataResult>();

//            this.Description              = Description is not null && Description.IsNotNullOrEmpty()
//                                                ? Description.Trim()
//                                                : null;

//            this.Warnings                 = Warnings    is not null
//                                                ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
//                                                : Array.Empty<Warning>();

//            this.Runtime                  = Runtime;

//        }

//        #endregion

//        #region PushChargingPoolDataResult(SenderId, ReceivePOIData, Result,...)

//        /// <summary>
//        /// Create a new acknowledgement.
//        /// </summary>
//        /// <param name="Result">The result of the operation.</param>
//        /// <param name="Description">An optional description of the result code.</param>
//        /// <param name="Warnings">Warnings or additional information.</param>
//        /// <param name="Runtime">The runtime of the request.</param>
//        public PushChargingPoolDataResult(IId                                             Id,
//                                          IReceivePOIData                                 ReceivePOIData,
//                                          PushDataResultTypes                             Result,
//                                          IEnumerable<PushSingleChargingPoolDataResult>?  SuccessfulChargingPools   = null,
//                                          IEnumerable<PushSingleChargingPoolDataResult>?  RejectedChargingPools     = null,
//                                          String?                                         Description               = null,
//                                          IEnumerable<Warning>?                           Warnings                  = null,
//                                          TimeSpan?                                       Runtime                   = null)
//        {

//            this.Id                       = Id;

//            this.ReceivePOIData           = ReceivePOIData;

//            this.Result                   = Result;

//            this.SuccessfulChargingPools  = SuccessfulChargingPools ?? Array.Empty<PushSingleChargingPoolDataResult>();

//            this.RejectedChargingPools    = RejectedChargingPools   ?? Array.Empty<PushSingleChargingPoolDataResult>();

//            this.Description              = Description is not null && Description.IsNotNullOrEmpty()
//                                                ? Description.Trim()
//                                                : null;

//            this.Warnings                 = Warnings != null
//                                                ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
//                                                : Array.Empty<Warning>();

//            this.Runtime                  = Runtime;

//        }

//        #endregion

//        #endregion


//        #region (static) AdminDown

//        public static PushChargingPoolDataResult

//            AdminDown(IId                         SenderId,
//                      ISender                Sender,
//                      IEnumerable<IChargingPool>  RejectedChargingPools,
//                      String?                     Description   = null,
//                      IEnumerable<Warning>?       Warnings      = null,
//                      TimeSpan?                   Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.AdminDown,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingPoolDataResult

//            AdminDown(IId                         SenderId,
//                      IReceivePOIData             ReceivePOIData,
//                      IEnumerable<IChargingPool>  RejectedChargingPools,
//                      String?                     Description   = null,
//                      IEnumerable<Warning>?       Warnings      = null,
//                      TimeSpan?                   Runtime       = null)

//                => new (SenderId,
//                        ReceivePOIData,
//                        PushDataResultTypes.AdminDown,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) Success

//        public static PushChargingPoolDataResult

//            Success(IId                         SenderId,
//                    ISender                Sender,
//                    IEnumerable<IChargingPool>  SuccessfulChargingPools,
//                    String?                     Description   = null,
//                    IEnumerable<Warning>?       Warnings      = null,
//                    TimeSpan?                   Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Success,
//                        SuccessfulChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingPoolDataResult

//            Success(IId                         SenderId,
//                    IReceivePOIData             ReceivePOIData,
//                    IEnumerable<IChargingPool>  SuccessfulChargingPools,
//                    String?                     Description   = null,
//                    IEnumerable<Warning>?       Warnings      = null,
//                    TimeSpan?                   Runtime       = null)

//                => new (SenderId,
//                        ReceivePOIData,
//                        PushDataResultTypes.Success,
//                        SuccessfulChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) Enqueued

//        public static PushChargingPoolDataResult

//            Enqueued(IId                         SenderId,
//                     ISender                Sender,
//                     IEnumerable<IChargingPool>  SuccessfulChargingPools,
//                     String?                     Description   = null,
//                     IEnumerable<Warning>?       Warnings      = null,
//                     TimeSpan?                   Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Enqueued,
//                        SuccessfulChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.Enqueued, Warnings)),
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) NoOperation

//        public static PushChargingPoolDataResult

//            NoOperation(IId                         SenderId,
//                        ISender                Sender,
//                        IEnumerable<IChargingPool>  RejectedChargingPools,
//                        String?                     Description   = null,
//                        IEnumerable<Warning>?       Warnings      = null,
//                        TimeSpan?                   Runtime       = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.NoOperation,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//         public static PushChargingPoolDataResult

//            NoOperation(IId                         SenderId,
//                        IReceivePOIData             ReceivePOIData,
//                        IEnumerable<IChargingPool>  RejectedChargingPools,
//                        String?                     Description   = null,
//                        IEnumerable<Warning>?       Warnings      = null,
//                        TimeSpan?                   Runtime       = null)

//                => new (SenderId,
//                        ReceivePOIData,
//                        PushDataResultTypes.NoOperation,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) LockTimeout

//        public static PushChargingPoolDataResult

//            LockTimeout(IId                         SenderId,
//                        ISender                Sender,
//                        IEnumerable<IChargingPool>  RejectedChargingPools,
//                        String?                     Description   = null,
//                        IEnumerable<Warning>?       Warnings      = null,
//                        TimeSpan?                   Runtime       = null)


//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Timeout,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.LockTimeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Timeout

//        public static PushChargingPoolDataResult

//            Timeout(IId                         SenderId,
//                    ISender                Sender,
//                    IEnumerable<IChargingPool>  RejectedChargingPools,
//                    String?                     Description   = null,
//                    IEnumerable<Warning>?       Warnings      = null,
//                    TimeSpan?                   Runtime       = null)


//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Timeout,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.Timeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Error

//        public static PushChargingPoolDataResult

//            Error(IId                                             SenderId,
//                  ISender                                    Sender,
//                  IEnumerable<PushSingleChargingPoolDataResult>?  RejectedChargingPools   = null,
//                  String?                                         Description             = null,
//                  IEnumerable<Warning>?                           Warnings                = null,
//                  TimeSpan?                                       Runtime                 = null)

//                => new (SenderId,
//                        Sender,
//                        PushDataResultTypes.Error,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools,
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingPoolDataResult

//            Error(IId                                             SenderId,
//                  IReceivePOIData                                 ReceivePOIData,
//                  IEnumerable<PushSingleChargingPoolDataResult>?  RejectedChargingPools   = null,
//                  String?                                         Description             = null,
//                  IEnumerable<Warning>?                           Warnings                = null,
//                  TimeSpan?                                       Runtime                 = null)

//                => new (SenderId,
//                        ReceivePOIData,
//                        PushDataResultTypes.Error,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools,
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion


//        //public PushEVSEDataResult ToPushEVSEDataResult()

//        //    => Sender is not null

//        //           ? new PushEVSEDataResult(Id,
//        //                                    Sender,
//        //                                    Result,
//        //                                    Description,
//        //                                    Array.Empty<EVSE>(),
//        //                                    Array.Empty<EVSE>(),
//        //                                    Warnings,
//        //                                    Runtime)

//        //           : new PushEVSEDataResult(Id,
//        //                                    ReceivePOIData,
//        //                                    Result,
//        //                                    Description,
//        //                                    Array.Empty<EVSE>(),
//        //                                    Array.Empty<EVSE>(),
//        //                                    Warnings,
//        //                                    Runtime);


//        #region (override) ToString()

//        /// <summary>
//        /// Return a text representation of this object.
//        /// </summary>
//        public override String ToString()

//            => String.Concat("Result: " + Result + "; " + Description);

//        #endregion

//    }


//    public enum PushDataResultTypes
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

//    public enum PushSingleDataResultTypes
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
//        /// Error.
//        /// </summary>
//        Error

//    }

//}
