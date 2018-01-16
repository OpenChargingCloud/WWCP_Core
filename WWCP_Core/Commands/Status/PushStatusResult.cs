/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    /// <summary>
    /// A PushStatus result.
    /// </summary>
    public class PushStatusResult
    {

        #region Properties

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushStatusResultTypes  Result          { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String                 Description     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>   Warnings        { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?              Runtime         { get;  }

        #endregion

        #region Constructor(s)

        #region (private) PushStatusResult(AuthId, ISendStatus, Result, ...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushStatusResult(IId                            AuthId,
                                 ISendStatus                    ISendStatus,
                                 PushStatusResultTypes          Result,
                                 IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates,
                                 String                         Description  = null,
                                 IEnumerable<Warning>           Warnings     = null,
                                 TimeSpan?                      Runtime      = null)
        {

            this.Result       = Result;

            this.Description  = Description.IsNotNullOrEmpty()
                                    ? Description.Trim()
                                    : null;

            this.Warnings     = Warnings != null
                                    ? Warnings.Where(warning => warning.IsNotNullOrEmpty())
                                    : new Warning[0];

            this.Runtime      = Runtime;

        }

        #endregion

        #region (private) PushStatusResult(AuthId, ISendStatus, Result, ...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushStatusResult(IId                            AuthId,
                                 IReceiveStatus                 IReceiveStatus,
                                 PushStatusResultTypes          Result,
                                 IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates,
                                 String                         Description  = null,
                                 IEnumerable<Warning>           Warnings     = null,
                                 TimeSpan?                      Runtime      = null)
        {

            this.Result       = Result;

            this.Description  = Description.IsNotNullOrEmpty()
                                    ? Description.Trim()
                                    : null;

            this.Warnings     = Warnings != null
                                    ? Warnings.Where(warning => warning.IsNotNullOrEmpty())
                                    : new Warning[0];

            this.Runtime      = Runtime;

        }

        #endregion

        #endregion



        #region Success

        public static PushStatusResult

            Success(IId                   AuthId,
                    ISendStatus           ISendStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushStatusResult(AuthId,
                                    ISendStatus,
                                    PushStatusResultTypes.Success,
                                    new EVSEStatusUpdate[0],
                                    Description,
                                    Warnings,
                                    Runtime);



        public static PushStatusResult

            Success(IId                   AuthId,
                    IReceiveStatus        IReceiveStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushStatusResult(AuthId,
                                    IReceiveStatus,
                                    PushStatusResultTypes.Success,
                                    new EVSEStatusUpdate[0],
                                    Description,
                                    Warnings,
                                    Runtime);

        #endregion


        #region Enqueued

        public static PushStatusResult

            Enqueued(IId                   AuthId,
                     ISendStatus           ISendStatus,
                     String                Description    = null,
                     IEnumerable<Warning>  Warnings       = null,
                     TimeSpan?             Runtime        = null)

            => new PushStatusResult(AuthId,
                                    ISendStatus,
                                    PushStatusResultTypes.Enqueued,
                                    new EVSEStatusUpdate[0],
                                    Description,
                                    Warnings,
                                    Runtime);

        #endregion

        #region NoOperation

        public static PushStatusResult

            NoOperation(IId                   AuthId,
                        ISendStatus           ISendStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushStatusResult(AuthId,
                                    ISendStatus,
                                    PushStatusResultTypes.NoOperation,
                                    new EVSEStatusUpdate[0],
                                    Description,
                                    Warnings,
                                    Runtime);



        public static PushStatusResult

            NoOperation(IId                   AuthId,
                        IReceiveStatus        IReceiveStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushStatusResult(AuthId,
                                    IReceiveStatus,
                                    PushStatusResultTypes.NoOperation,
                                    new EVSEStatusUpdate[0],
                                    Description,
                                    Warnings,
                                    Runtime);

        #endregion

        #region OutOfService

        public static PushStatusResult

            OutOfService(IId                            AuthId,
                         ISendStatus                    ISendStatus,
                         IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushStatusResult(AuthId,
                                    ISendStatus,
                                    PushStatusResultTypes.OutOfService,
                                    RejectedEVSEStatusUpdates,
                                    Description,
                                    Warnings,
                                    Runtime);



        public static PushStatusResult

            OutOfService(IId                            AuthId,
                         IReceiveStatus                 IReceiveStatus,
                         IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushStatusResult(AuthId,
                                    IReceiveStatus,
                                    PushStatusResultTypes.OutOfService,
                                    RejectedEVSEStatusUpdates,
                                    Description,
                                    Warnings,
                                    Runtime);



        public static PushStatusResult

            OutOfService(IId                                       AuthId,
                         ISendStatus                               ISendStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushStatusResult(AuthId,
                                    ISendStatus,
                                    PushStatusResultTypes.OutOfService,
                                    new EVSEStatusUpdate[0],
                                    Description,
                                    Warnings,
                                    Runtime);



        public static PushStatusResult

            OutOfService(IId                                       AuthId,
                         IReceiveStatus                            IReceiveStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushStatusResult(AuthId,
                                    IReceiveStatus,
                                    PushStatusResultTypes.OutOfService,
                                    new EVSEStatusUpdate[0],
                                    Description,
                                    Warnings,
                                    Runtime);



        public static PushStatusResult

            OutOfService(IId                                    AuthId,
                         ISendStatus                            ISendStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushStatusResult(AuthId,
                                    ISendStatus,
                                    PushStatusResultTypes.OutOfService,
                                    new EVSEStatusUpdate[0],
                                    Description,
                                    Warnings,
                                    Runtime);



        public static PushStatusResult

            OutOfService(IId                                    AuthId,
                         IReceiveStatus                         IReceiveStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushStatusResult(AuthId,
                                    IReceiveStatus,
                                    PushStatusResultTypes.OutOfService,
                                    new EVSEStatusUpdate[0],
                                    Description,
                                    Warnings,
                                    Runtime);

        #endregion


        #region Error

        public static PushStatusResult

            Error(IId                            AuthId,
                  ISendStatus                    ISendStatus,
                  IEnumerable<EVSEStatusUpdate>  RejectedEVSEs  = null,
                  String                         Description    = null,
                  IEnumerable<Warning>           Warnings       = null,
                  TimeSpan?                      Runtime        = null)

            => new PushStatusResult(AuthId,
                                    ISendStatus,
                                    PushStatusResultTypes.Error,
                                    RejectedEVSEs,
                                    Description,
                                    Warnings,
                                    Runtime);


        public static PushStatusResult Error(IId                            AuthId,
                                             IReceiveStatus                 IReceiveStatus,
                                             IEnumerable<EVSEStatusUpdate>  RejectedEVSEs  = null,
                                             String                         Description    = null,
                                             IEnumerable<Warning>           Warnings       = null,
                                             TimeSpan?                      Runtime        = null)

            => new PushStatusResult(AuthId,
                                    IReceiveStatus,
                                    PushStatusResultTypes.Error,
                                    RejectedEVSEs,
                                    Description,
                                    Warnings,
                                    Runtime);

        #endregion

        #region Enqueued

        public static PushStatusResult LockTimeout(IId                   AuthId,
                                                   ISendStatus           ISendStatus,
                                                   String                Description    = null,
                                                   IEnumerable<Warning>  Warnings       = null,
                                                   TimeSpan?             Runtime        = null)

            => new PushStatusResult(AuthId,
                                    ISendStatus,
                                    PushStatusResultTypes.LockTimeout,
                                    new EVSEStatusUpdate[0],
                                    Description,
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



    public enum PushStatusResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        OutOfService,

        Success,

        NoOperation,

        Enqueued,

        Error,

        LockTimeout

    }

}
