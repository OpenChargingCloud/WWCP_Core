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
//using social.OpenData.UsersAPI;
//using System.Linq;

//#endregion

//namespace cloud.charging.open.protocols.WWCP
//{

//    /// <summary>
//    /// A PushData result.
//    /// </summary>
//    public class PushChargingStationDataResult
//    {

//        #region Properties
//        public IId                                               Id                            { get; }

//        public ISendPOIData?                                     SendPOIData                   { get; }

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

//        #region PushChargingStationDataResult(Id,     SendPOIData,    Result,...)

//        /// <summary>
//        /// Create a new acknowledgement.
//        /// </summary>
//        /// <param name="Result">The result of the operation.</param>
//        /// <param name="Description">An optional description of the result code.</param>
//        /// <param name="Warnings">Warnings or additional information.</param>
//        /// <param name="Runtime">The runtime of the request.</param>
//        public PushChargingStationDataResult(IId                                                Id,
//                                             ISendPOIData                                       SendPOIData,
//                                             PushDataResultTypes                                Result,
//                                             IEnumerable<PushSingleChargingStationDataResult>?  SuccessfulChargingStations   = null,
//                                             IEnumerable<PushSingleChargingStationDataResult>?  RejectedChargingStations     = null,
//                                             String?                                            Description                  = null,
//                                             IEnumerable<Warning>?                              Warnings                     = null,
//                                             TimeSpan?                                          Runtime                      = null)
//        {

//            this.Id                          = Id;

//            this.SendPOIData                 = SendPOIData;

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

//        #region PushChargingStationDataResult(AuthId, ReceivePOIData, Result,...)

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

//            AdminDown(IId                            AuthId,
//                      ISendPOIData                   SendPOIData,
//                      IEnumerable<IChargingStation>  RejectedChargingStations,
//                      String?                        Description   = null,
//                      IEnumerable<Warning>?          Warnings      = null,
//                      TimeSpan?                      Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.AdminDown,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingStationDataResult

//            AdminDown(IId                            AuthId,
//                      IReceivePOIData                ReceivePOIData,
//                      IEnumerable<IChargingStation>  RejectedChargingStations,
//                      String?                        Description   = null,
//                      IEnumerable<Warning>?          Warnings      = null,
//                      TimeSpan?                      Runtime       = null)

//                => new (AuthId,
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

//            Success(IId                            AuthId,
//                    ISendPOIData                   SendPOIData,
//                    IEnumerable<IChargingStation>  SuccessfulChargingStations,
//                    String?                        Description   = null,
//                    IEnumerable<Warning>?          Warnings      = null,
//                    TimeSpan?                      Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Success,
//                        SuccessfulChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingStationDataResult

//            Success(IId                            AuthId,
//                    IReceivePOIData                ReceivePOIData,
//                    IEnumerable<IChargingStation>  SuccessfulChargingStations,
//                    String?                        Description   = null,
//                    IEnumerable<Warning>?          Warnings      = null,
//                    TimeSpan?                      Runtime       = null)

//                => new (AuthId,
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

//            Enqueued(IId                            AuthId,
//                     ISendPOIData                   SendPOIData,
//                     IEnumerable<IChargingStation>  SuccessfulChargingStations,
//                     String?                        Description   = null,
//                     IEnumerable<Warning>?          Warnings      = null,
//                     TimeSpan?                      Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Enqueued,
//                        SuccessfulChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.Enqueued, Warnings)),
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) NoOperation

//        public static PushChargingStationDataResult

//            NoOperation(IId                            AuthId,
//                        ISendPOIData                   SendPOIData,
//                        IEnumerable<IChargingStation>  RejectedChargingStations,
//                        String?                        Description   = null,
//                        IEnumerable<Warning>?          Warnings      = null,
//                        TimeSpan?                      Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.NoOperation,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//         public static PushChargingStationDataResult

//            NoOperation(IId                            AuthId,
//                        IReceivePOIData                ReceivePOIData,
//                        IEnumerable<IChargingStation>  RejectedChargingStations,
//                        String?                        Description   = null,
//                        IEnumerable<Warning>?          Warnings      = null,
//                        TimeSpan?                      Runtime       = null)

//                => new (AuthId,
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

//            LockTimeout(IId                            AuthId,
//                        ISendPOIData                   SendPOIData,
//                        IEnumerable<IChargingStation>  RejectedChargingStations,
//                        String?                        Description   = null,
//                        IEnumerable<Warning>?          Warnings      = null,
//                        TimeSpan?                      Runtime       = null)


//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Timeout,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.LockTimeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Timeout

//        public static PushChargingStationDataResult

//            Timeout(IId                            AuthId,
//                    ISendPOIData                   SendPOIData,
//                    IEnumerable<IChargingStation>  RejectedChargingStations,
//                    String?                        Description   = null,
//                    IEnumerable<Warning>?          Warnings      = null,
//                    TimeSpan?                      Runtime       = null)


//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Timeout,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations.Select(chargingStation => new PushSingleChargingStationDataResult(chargingStation, PushSingleDataResultTypes.Timeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Error

//        public static PushChargingStationDataResult

//            Error(IId                                               AuthId,
//                  ISendPOIData                                      SendPOIData,
//                  IEnumerable<PushSingleChargingStationDataResult>  RejectedChargingStations,
//                  String?                                           Description     = null,
//                  IEnumerable<Warning>?                             Warnings        = null,
//                  TimeSpan?                                         Runtime         = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Error,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations,
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingStationDataResult

//            Error(IId                                               AuthId,
//                  IReceivePOIData                                   ReceivePOIData,
//                  IEnumerable<PushSingleChargingStationDataResult>  RejectedChargingStations,
//                  String?                                           Description     = null,
//                  IEnumerable<Warning>?                             Warnings        = null,
//                  TimeSpan?                                         Runtime         = null)

//                => new (AuthId,
//                        ReceivePOIData,
//                        PushDataResultTypes.Error,
//                        Array.Empty<PushSingleChargingStationDataResult>(),
//                        RejectedChargingStations,
//                        Description,
//                        Warnings,
//                        Runtime);

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
