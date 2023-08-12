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
//    public class PushChargingPoolDataResult
//    {

//        #region Properties

//        public IId                                               Id                         { get; }

//        public ISendChargingPoolData?                            SendPOIData                { get; }

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

//        #region PushChargingPoolDataResult(Id,     SendPOIData,    Result,...)

//        /// <summary>
//        /// Create a new acknowledgement.
//        /// </summary>
//        /// <param name="Result">The result of the operation.</param>
//        /// <param name="Description">An optional description of the result code.</param>
//        /// <param name="Warnings">Warnings or additional information.</param>
//        /// <param name="Runtime">The runtime of the request.</param>
//        public PushChargingPoolDataResult(IId                                             Id,
//                                          ISendChargingPoolData                           SendPOIData,
//                                          PushDataResultTypes                             Result,
//                                          IEnumerable<PushSingleChargingPoolDataResult>?  SuccessfulChargingPools   = null,
//                                          IEnumerable<PushSingleChargingPoolDataResult>?  RejectedChargingPools     = null,
//                                          String?                                         Description               = null,
//                                          IEnumerable<Warning>?                           Warnings                  = null,
//                                          TimeSpan?                                       Runtime                   = null)
//        {

//            this.Id                       = Id;

//            this.SendPOIData              = SendPOIData;

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

//        #region PushChargingPoolDataResult(AuthId, ReceivePOIData, Result,...)

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

//            AdminDown(IId                         AuthId,
//                      ISendChargingPoolData       SendPOIData,
//                      IEnumerable<IChargingPool>  RejectedChargingPools,
//                      String?                     Description   = null,
//                      IEnumerable<Warning>?       Warnings      = null,
//                      TimeSpan?                   Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.AdminDown,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.AdminDown, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingPoolDataResult

//            AdminDown(IId                         AuthId,
//                      IReceivePOIData             ReceivePOIData,
//                      IEnumerable<IChargingPool>  RejectedChargingPools,
//                      String?                     Description   = null,
//                      IEnumerable<Warning>?       Warnings      = null,
//                      TimeSpan?                   Runtime       = null)

//                => new (AuthId,
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

//            Success(IId                         AuthId,
//                    ISendChargingPoolData                SendPOIData,
//                    IEnumerable<IChargingPool>  SuccessfulChargingPools,
//                    String?                     Description   = null,
//                    IEnumerable<Warning>?       Warnings      = null,
//                    TimeSpan?                   Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Success,
//                        SuccessfulChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.Success, Warnings)),
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingPoolDataResult

//            Success(IId                         AuthId,
//                    IReceivePOIData             ReceivePOIData,
//                    IEnumerable<IChargingPool>  SuccessfulChargingPools,
//                    String?                     Description   = null,
//                    IEnumerable<Warning>?       Warnings      = null,
//                    TimeSpan?                   Runtime       = null)

//                => new (AuthId,
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

//            Enqueued(IId                         AuthId,
//                     ISendChargingPoolData                SendPOIData,
//                     IEnumerable<IChargingPool>  SuccessfulChargingPools,
//                     String?                     Description   = null,
//                     IEnumerable<Warning>?       Warnings      = null,
//                     TimeSpan?                   Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Enqueued,
//                        SuccessfulChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.Enqueued, Warnings)),
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion

//        #region (static) NoOperation

//        public static PushChargingPoolDataResult

//            NoOperation(IId                         AuthId,
//                        ISendChargingPoolData                SendPOIData,
//                        IEnumerable<IChargingPool>  RejectedChargingPools,
//                        String?                     Description   = null,
//                        IEnumerable<Warning>?       Warnings      = null,
//                        TimeSpan?                   Runtime       = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.NoOperation,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.NoOperation, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime);


//         public static PushChargingPoolDataResult

//            NoOperation(IId                         AuthId,
//                        IReceivePOIData             ReceivePOIData,
//                        IEnumerable<IChargingPool>  RejectedChargingPools,
//                        String?                     Description   = null,
//                        IEnumerable<Warning>?       Warnings      = null,
//                        TimeSpan?                   Runtime       = null)

//                => new (AuthId,
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

//            LockTimeout(IId                         AuthId,
//                        ISendChargingPoolData                SendPOIData,
//                        IEnumerable<IChargingPool>  RejectedChargingPools,
//                        String?                     Description   = null,
//                        IEnumerable<Warning>?       Warnings      = null,
//                        TimeSpan?                   Runtime       = null)


//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Timeout,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.LockTimeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Timeout

//        public static PushChargingPoolDataResult

//            Timeout(IId                         AuthId,
//                    ISendChargingPoolData                SendPOIData,
//                    IEnumerable<IChargingPool>  RejectedChargingPools,
//                    String?                     Description   = null,
//                    IEnumerable<Warning>?       Warnings      = null,
//                    TimeSpan?                   Runtime       = null)


//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Timeout,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools.Select(chargingPool => new PushSingleChargingPoolDataResult(chargingPool, PushSingleDataResultTypes.Timeout, Warnings)),
//                        Description,
//                        Warnings,
//                        Runtime: Runtime);

//        #endregion

//        #region (static) Error

//        public static PushChargingPoolDataResult

//            Error(IId                                             AuthId,
//                  ISendChargingPoolData                                    SendPOIData,
//                  IEnumerable<PushSingleChargingPoolDataResult>?  RejectedChargingPools   = null,
//                  String?                                         Description             = null,
//                  IEnumerable<Warning>?                           Warnings                = null,
//                  TimeSpan?                                       Runtime                 = null)

//                => new (AuthId,
//                        SendPOIData,
//                        PushDataResultTypes.Error,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools,
//                        Description,
//                        Warnings,
//                        Runtime);


//        public static PushChargingPoolDataResult

//            Error(IId                                             AuthId,
//                  IReceivePOIData                                 ReceivePOIData,
//                  IEnumerable<PushSingleChargingPoolDataResult>?  RejectedChargingPools   = null,
//                  String?                                         Description             = null,
//                  IEnumerable<Warning>?                           Warnings                = null,
//                  TimeSpan?                                       Runtime                 = null)

//                => new (AuthId,
//                        ReceivePOIData,
//                        PushDataResultTypes.Error,
//                        Array.Empty<PushSingleChargingPoolDataResult>(),
//                        RejectedChargingPools,
//                        Description,
//                        Warnings,
//                        Runtime);

//        #endregion


//        //public PushEVSEDataResult ToPushEVSEDataResult()

//        //    => SendPOIData is not null

//        //           ? new PushEVSEDataResult(Id,
//        //                                    SendPOIData,
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

//}
