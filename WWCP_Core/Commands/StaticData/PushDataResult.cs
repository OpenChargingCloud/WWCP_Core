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
            this.Warnings  = Warnings;

        }

    }

    public class PushSingleChargingStationDataResult
    {

        public ChargingStation            ChargingStation    { get; }
        public PushSingleDataResultTypes  Result             { get; }
        public IEnumerable<String>        Warnings           { get; }

        public PushSingleChargingStationDataResult(ChargingStation            ChargingStation,
                                                   PushSingleDataResultTypes  Result,
                                                   IEnumerable<String>        Warnings)
        {

            this.ChargingStation  = ChargingStation;
            this.Result           = Result;
            this.Warnings         = Warnings;

        }

    }


    /// <summary>
    /// A PushData result.
    /// </summary>
    public class PushEVSEDataResult
    {

        #region Properties

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushDataResultTypes  Result          { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String               Description     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<String>  Warnings        { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?            Runtime         { get;  }

        #endregion

        #region Constructor(s)

        #region (internal) PushEVSEDataResult(AuthId, ISendData,    Result,...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEVSEDataResult(IId                  AuthId,
                                    ISendData            ISendData,
                                    PushDataResultTypes  Result,
                                    IEnumerable<EVSE>    RejectedEVSEs  = null,
                                    String               Description    = null,
                                    IEnumerable<String>  Warnings       = null,
                                    TimeSpan?            Runtime        = null)
        {

            this.Result       = Result;

            this.Description  = Description.IsNotNullOrEmpty()
                                    ? Description.Trim()
                                    : null;

            this.Warnings     = Warnings != null
                                    ? Warnings.Where     (warning => warning != null).
                                               SafeSelect(warning => warning.Trim()).
                                               Where     (warning => warning.IsNotNullOrEmpty())
                                    : null;

            this.Runtime      = Runtime;

        }

        #endregion

        #region (internal) PushEVSEDataResult(AuthId, IReceiveData, Result,...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEVSEDataResult(IId                  AuthId,
                                    IReceiveData         IReceiveData,
                                    PushDataResultTypes  Result,
                                    IEnumerable<EVSE>    RejectedEVSEs   = null,
                                    String               Description     = null,
                                    IEnumerable<String>  Warnings        = null,
                                    TimeSpan?            Runtime         = null)
        {

            this.Result       = Result;

            this.Description  = Description.IsNotNullOrEmpty()
                                    ? Description.Trim()
                                    : null;

            this.Warnings     = Warnings != null
                                    ? Warnings.Where     (warning => warning != null).
                                               SafeSelect(warning => warning.Trim()).
                                               Where     (warning => warning.IsNotNullOrEmpty())
                                    : null;

            this.Runtime      = Runtime;

        }

        #endregion

        #endregion


        #region AdminDown

        public static PushEVSEDataResult AdminDown(IId                  AuthId,
                                               ISendData            ISendData,
                                               IEnumerable<EVSE>    RejectedEVSEs  = null,
                                               String               Description    = null,
                                               IEnumerable<String>  Warnings       = null,
                                               TimeSpan?            Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                  ISendData,
                                  PushDataResultTypes.AdminDown,
                                  RejectedEVSEs,
                                  Description,
                                  Warnings,
                                  Runtime);


        public static PushEVSEDataResult AdminDown(IId                  AuthId,
                                               IReceiveData         IReceiveData,
                                               IEnumerable<EVSE>    RejectedEVSEs  = null,
                                               String               Description    = null,
                                               IEnumerable<String>  Warnings       = null,
                                               TimeSpan?            Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                  IReceiveData,
                                  PushDataResultTypes.AdminDown,
                                  RejectedEVSEs,
                                  Description,
                                  Warnings,
                                  Runtime);

        #endregion

        #region Success

        public static PushEVSEDataResult Success(IId                  AuthId,
                                             ISendData            ISendData,
                                             String               Description    = null,
                                             IEnumerable<String>  Warnings       = null,
                                             TimeSpan?            Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                  ISendData,
                                  PushDataResultTypes.Success,
                                  new EVSE[0],
                                  Description,
                                  Warnings,
                                  Runtime);


        public static PushEVSEDataResult Success(IId                  AuthId,
                                             IReceiveData         IReceiveData,
                                             String               Description    = null,
                                             IEnumerable<String>  Warnings       = null,
                                             TimeSpan?            Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                  IReceiveData,
                                  PushDataResultTypes.Success,
                                  new EVSE[0],
                                  Description,
                                  Warnings,
                                  Runtime);

        #endregion

        #region Enqueued

        public static PushEVSEDataResult Enqueued(IId                  AuthId,
                                              ISendData            ISendData,
                                              String               Description    = null,
                                              IEnumerable<String>  Warnings       = null,
                                              TimeSpan?            Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                  ISendData,
                                  PushDataResultTypes.Enqueued,
                                  new EVSE[0],
                                  Description,
                                  Warnings,
                                  Runtime);

        #endregion

        #region NoOperation

        public static PushEVSEDataResult NoOperation(IId                  AuthId,
                                                 ISendData            ISendData,
                                                 String               Description    = null,
                                                 IEnumerable<String>  Warnings       = null,
                                                 TimeSpan?            Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                  ISendData,
                                  PushDataResultTypes.NoOperation,
                                  new EVSE[0],
                                  Description,
                                  Warnings,
                                  Runtime);


         public static PushEVSEDataResult NoOperation(IId                  AuthId,
                                                  IReceiveData         IReceiveData,
                                                  String               Description    = null,
                                                  IEnumerable<String>  Warnings       = null,
                                                  TimeSpan?            Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                  IReceiveData,
                                  PushDataResultTypes.NoOperation,
                                  new EVSE[0],
                                  Description,
                                  Warnings,
                                  Runtime);

        #endregion


        #region Error

        public static PushEVSEDataResult Error(IId                  AuthId,
                                           ISendData            ISendData,
                                           IEnumerable<EVSE>    RejectedEVSEs  = null,
                                           String               Description    = null,
                                           IEnumerable<String>  Warnings       = null,
                                           TimeSpan?            Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                  ISendData,
                                  PushDataResultTypes.Error,
                                  RejectedEVSEs,
                                  Description,
                                  Warnings,
                                  Runtime);


        public static PushEVSEDataResult Error(IId                  AuthId,
                                           IReceiveData         IReceiveData,
                                           IEnumerable<EVSE>    RejectedEVSEs  = null,
                                           String               Description    = null,
                                           IEnumerable<String>  Warnings       = null,
                                           TimeSpan?            Runtime        = null)

            => new PushEVSEDataResult(AuthId,
                                  IReceiveData,
                                  PushDataResultTypes.Error,
                                  RejectedEVSEs,
                                  Description,
                                  Warnings,
                                  Runtime);

        #endregion



        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
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

        public IId Id { get; }

        public ISendData ISendData { get; }

        public IReceiveData IReceiveData { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushDataResultTypes  Result          { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String               Description     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<String>  Warnings        { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?            Runtime         { get;  }

        #endregion

        #region Constructor(s)

        #region (private) PushChargingStationDataResult(Id, ISendData,    Result,...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationDataResult(IId                                               Id,
                                              ISendData                                         ISendData,
                                              PushDataResultTypes                               Result,
                                              IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                                              String                                            Description     = null,
                                              IEnumerable<String>                               Warnings        = null,
                                              TimeSpan?                                         Runtime         = null)
        {

            this.Id           = Id;

            this.ISendData    = ISendData;

            this.Result       = Result;

            this.Description  = Description.IsNotNullOrEmpty()
                                    ? Description.Trim()
                                    : null;

            this.Warnings     = Warnings != null
                                    ? Warnings.Where     (warning => warning != null).
                                               SafeSelect(warning => warning.Trim()).
                                               Where     (warning => warning.IsNotNullOrEmpty())
                                    : null;

            this.Runtime      = Runtime;

        }

        #endregion

        #region (private) PushChargingStationDataResult(AuthId, IReceiveData, Result,...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationDataResult(IId                                               AuthId,
                                              IReceiveData                                      IReceiveData,
                                              PushDataResultTypes                               Result,
                                              IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                                              String                                            Description     = null,
                                              IEnumerable<String>                               Warnings        = null,
                                              TimeSpan?                                         Runtime         = null)
        {

            this.Id            = Id;

            this.IReceiveData  = IReceiveData;

            this.Result        = Result;

            this.Description   = Description.IsNotNullOrEmpty()
                                     ? Description.Trim()
                                     : null;

            this.Warnings      = Warnings != null
                                     ? Warnings.Where     (warning => warning != null).
                                                SafeSelect(warning => warning.Trim()).
                                                Where     (warning => warning.IsNotNullOrEmpty())
                                     : null;

            this.Runtime       = Runtime;

        }

        #endregion

        #endregion


        #region AdminDown

        public static PushChargingStationDataResult AdminDown(IId                                               AuthId,
                                                              ISendData                                         ISendData,
                                                              IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                                                              String                                            Description     = null,
                                                              IEnumerable<String>                               Warnings        = null,
                                                              TimeSpan?                                         Runtime         = null)

            => new PushChargingStationDataResult(AuthId,
                                                 ISendData,
                                                 PushDataResultTypes.AdminDown,
                                                 RejectedEVSEs,
                                                 Description,
                                                 Warnings,
                                                 Runtime);


        public static PushChargingStationDataResult AdminDown(IId                                               AuthId,
                                                              IReceiveData                                      IReceiveData,
                                                              IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                                                              String                                            Description     = null,
                                                              IEnumerable<String>                               Warnings        = null,
                                                              TimeSpan?                                         Runtime         = null)

            => new PushChargingStationDataResult(AuthId,
                                                 IReceiveData,
                                                 PushDataResultTypes.AdminDown,
                                                 RejectedEVSEs,
                                                 Description,
                                                 Warnings,
                                                 Runtime);

        #endregion

        #region Success

        public static PushChargingStationDataResult Success(IId                  AuthId,
                                             ISendData            ISendData,
                                             String               Description    = null,
                                             IEnumerable<String>  Warnings       = null,
                                             TimeSpan?            Runtime        = null)

            => new PushChargingStationDataResult(AuthId,
                                                 ISendData,
                                                 PushDataResultTypes.Success,
                                                 new PushSingleChargingStationDataResult[0],
                                                 Description,
                                                 Warnings,
                                                 Runtime);


        public static PushChargingStationDataResult Success(IId                  AuthId,
                                                            IReceiveData         IReceiveData,
                                                            String               Description    = null,
                                                            IEnumerable<String>  Warnings       = null,
                                                            TimeSpan?            Runtime        = null)

            => new PushChargingStationDataResult(AuthId,
                                                 IReceiveData,
                                                 PushDataResultTypes.Success,
                                                 new PushSingleChargingStationDataResult[0],
                                                 Description,
                                                 Warnings,
                                                 Runtime);

        #endregion

        #region Enqueued

        public static PushChargingStationDataResult Enqueued(IId                  AuthId,
                                                             ISendData            ISendData,
                                                             String               Description    = null,
                                                             IEnumerable<String>  Warnings       = null,
                                                             TimeSpan?            Runtime        = null)

            => new PushChargingStationDataResult(AuthId,
                                                 ISendData,
                                                 PushDataResultTypes.Enqueued,
                                                 new PushSingleChargingStationDataResult[0],
                                                 Description,
                                                 Warnings,
                                                 Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationDataResult NoOperation(IId                  AuthId,
                                                                ISendData            ISendData,
                                                                String               Description    = null,
                                                                IEnumerable<String>  Warnings       = null,
                                                                TimeSpan?            Runtime        = null)

            => new PushChargingStationDataResult(AuthId,
                                                 ISendData,
                                                 PushDataResultTypes.NoOperation,
                                                 new PushSingleChargingStationDataResult[0],
                                                 Description,
                                                 Warnings,
                                                 Runtime);


         public static PushChargingStationDataResult NoOperation(IId                  AuthId,
                                                                 IReceiveData         IReceiveData,
                                                                 String               Description    = null,
                                                                 IEnumerable<String>  Warnings       = null,
                                                                 TimeSpan?            Runtime        = null)

            => new PushChargingStationDataResult(AuthId,
                                                 IReceiveData,
                                                 PushDataResultTypes.NoOperation,
                                                 new PushSingleChargingStationDataResult[0],
                                                 Description,
                                                 Warnings,
                                                 Runtime);

        #endregion

        #region Error

        public static PushChargingStationDataResult Error(IId                                               AuthId,
                                                          ISendData                                         ISendData,
                                                          IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                                                          String                                            Description     = null,
                                                          IEnumerable<String>                               Warnings        = null,
                                                          TimeSpan?                                         Runtime         = null)

            => new PushChargingStationDataResult(AuthId,
                                                 ISendData,
                                                 PushDataResultTypes.Error,
                                                 RejectedEVSEs,
                                                 Description,
                                                 Warnings,
                                                 Runtime);


        public static PushChargingStationDataResult Error(IId                                               AuthId,
                                                          IReceiveData                                      IReceiveData,
                                                          IEnumerable<PushSingleChargingStationDataResult>  RejectedEVSEs   = null,
                                                          String                                            Description     = null,
                                                          IEnumerable<String>                               Warnings        = null,
                                                          TimeSpan?                                         Runtime         = null)

            => new PushChargingStationDataResult(AuthId,
                                                 IReceiveData,
                                                 PushDataResultTypes.Error,
                                                 RejectedEVSEs,
                                                 Description,
                                                 Warnings,
                                                 Runtime);

        #endregion


        public PushEVSEDataResult ToPushEVSEDataResult()

            => ISendData != null

                   ? new PushEVSEDataResult(Id,
                                            ISendData,
                                            Result,
                                            null, 
                                            Description,
                                            Warnings,
                                            Runtime)

                   : new PushEVSEDataResult(Id,
                                            IReceiveData,
                                            Result,
                                            null,
                                            Description,
                                            Warnings,
                                            Runtime);


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
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
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        Enqueued,

        Success,

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

        NoOperation,
        Enqueued,

        Success,
        Error,

        LockTimeout

    }

}
