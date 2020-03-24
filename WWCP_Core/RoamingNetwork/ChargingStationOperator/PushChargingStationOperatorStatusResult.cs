/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A PushChargingStationOperatorStatus result.
    /// </summary>
    public class PushChargingStationOperatorStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                            AuthId                       { get; }

        /// <summary>
        /// An object implementing ISendStatus.
        /// </summary>
        public ISendStatus                    ISendStatus                  { get; }

        /// <summary>
        /// An object implementing IReceiveStatus.
        /// </summary>
        public IReceiveStatus                 IReceiveStatus               { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushChargingStationOperatorStatusResultTypes      Result                       { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String                         Description                  { get; }

        /// <summary>
        /// An enumeration of rejected ChargingStationOperator status updates.
        /// </summary>
        public IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates    { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>           Warnings                     { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                      Runtime                      { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushChargingStationOperatorStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushChargingStationOperatorStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationOperatorStatusUpdates">An enumeration of rejected ChargingStationOperator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationOperatorStatusResult(IId                            AuthId,
                                     PushChargingStationOperatorStatusResultTypes      Result,
                                     String                         Description                 = null,
                                     IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates   = null,
                                     IEnumerable<Warning>           Warnings                    = null,
                                     TimeSpan?                      Runtime                     = null)
        {

            this.AuthId                     = AuthId;
            this.Result                     = Result;

            this.Description                = Description.IsNotNullOrEmpty()
                                                  ? Description.Trim()
                                                  : null;

            this.RejectedChargingStationOperatorStatusUpdates  = RejectedChargingStationOperatorStatusUpdates != null
                                                  ? RejectedChargingStationOperatorStatusUpdates.Where(evse => evse != null)
                                                  : new ChargingStationOperatorStatusUpdate[0];

            this.Warnings                   = Warnings != null
                                                  ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                  : new Warning[0];

            this.Runtime                    = Runtime;

        }

        #endregion

        #region (internal) PushChargingStationOperatorStatusResult(AuthId, ISendStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingStationOperatorStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendStatus">An object implementing ISendStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationOperatorStatusUpdates">An enumeration of rejected ChargingStationOperator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationOperatorStatusResult(IId                            AuthId,
                                      ISendStatus                    ISendStatus,
                                      PushChargingStationOperatorStatusResultTypes      Result,
                                      String                         Description                 = null,
                                      IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates   = null,
                                      IEnumerable<Warning>           Warnings                    = null,
                                      TimeSpan?                      Runtime                     = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingStationOperatorStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendStatus = ISendStatus;

        }

        #endregion

        #region (internal) PushChargingStationOperatorStatusResult(AuthId, IReceiveStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingStationOperatorStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveStatus">An object implementing IReceiveStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationOperatorStatusUpdates">An enumeration of rejected ChargingStationOperator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationOperatorStatusResult(IId                            AuthId,
                                      IReceiveStatus                 IReceiveStatus,
                                      PushChargingStationOperatorStatusResultTypes      Result,
                                      String                         Description                 = null,
                                      IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates   = null,
                                      IEnumerable<Warning>           Warnings                    = null,
                                      TimeSpan?                      Runtime                     = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingStationOperatorStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveStatus = IReceiveStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingStationOperatorStatusResult

            Success(IId                   AuthId,
                    ISendStatus           ISendStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.Success,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            Success(IId                   AuthId,
                    IReceiveStatus        IReceiveStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationOperatorStatusResultTypes.Success,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Enqueued

        public static PushChargingStationOperatorStatusResult

            Enqueued(IId                   AuthId,
                     ISendStatus           ISendStatus,
                     String                Description    = null,
                     IEnumerable<Warning>  Warnings       = null,
                     TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.Enqueued,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationOperatorStatusResult

            NoOperation(IId                   AuthId,
                        ISendStatus           ISendStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.NoOperation,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            NoOperation(IId                   AuthId,
                        IReceiveStatus        IReceiveStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationOperatorStatusResultTypes.NoOperation,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region OutOfService

        public static PushChargingStationOperatorStatusResult

            OutOfService(IId                            AuthId,
                         ISendStatus                    ISendStatus,
                         IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedChargingStationOperatorStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            OutOfService(IId                            AuthId,
                         IReceiveStatus                 IReceiveStatus,
                         IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationOperatorStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedChargingStationOperatorStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            OutOfService(IId                                       AuthId,
                         ISendStatus                               ISendStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            OutOfService(IId                                       AuthId,
                         IReceiveStatus                            IReceiveStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationOperatorStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            OutOfService(IId                                    AuthId,
                         ISendStatus                            ISendStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            OutOfService(IId                                    AuthId,
                         IReceiveStatus                         IReceiveStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationOperatorStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region AdminDown

        public static PushChargingStationOperatorStatusResult

            AdminDown(IId                            AuthId,
                      ISendStatus                    ISendStatus,
                      IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedChargingStationOperatorStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            AdminDown(IId                            AuthId,
                      IReceiveStatus                 IReceiveStatus,
                      IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationOperatorStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedChargingStationOperatorStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            AdminDown(IId                                       AuthId,
                      ISendStatus                               ISendStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            AdminDown(IId                                       AuthId,
                      IReceiveStatus                            IReceiveStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationOperatorStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            AdminDown(IId                                    AuthId,
                      ISendStatus                            ISendStatus,
                      IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorStatusResult

            AdminDown(IId                                    AuthId,
                      IReceiveStatus                         IReceiveStatus,
                      IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationOperatorStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Error

        public static PushChargingStationOperatorStatusResult

            Error(IId                            AuthId,
                  ISendStatus                    ISendStatus,
                  IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperators  = null,
                  String                         Description    = null,
                  IEnumerable<Warning>           Warnings       = null,
                  TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.Error,
                                        Description,
                                        RejectedChargingStationOperators,
                                        Warnings,
                                        Runtime);


        public static PushChargingStationOperatorStatusResult Error(IId                            AuthId,
                                                 IReceiveStatus                 IReceiveStatus,
                                                 IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperators  = null,
                                                 String                         Description    = null,
                                                 IEnumerable<Warning>           Warnings       = null,
                                                 TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationOperatorStatusResultTypes.Error,
                                        Description,
                                        RejectedChargingStationOperators,
                                        Warnings,
                                        Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingStationOperatorStatusResult LockTimeout(IId                   AuthId,
                                                       ISendStatus           ISendStatus,
                                                       String                Description    = null,
                                                       IEnumerable<Warning>  Warnings       = null,
                                                       TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationOperatorStatusResultTypes.LockTimeout,
                                        Description,
                                        new ChargingStationOperatorStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion



        public static PushChargingStationOperatorStatusResult Flatten(IId                                AuthId,
                                                   ISendStatus                        ISendStatus,
                                                   IEnumerable<PushChargingStationOperatorStatusResult>  PushChargingStationOperatorStatusResults,
                                                   TimeSpan                           Runtime)
        {

            #region Initial checks

            if (PushChargingStationOperatorStatusResults == null || !PushChargingStationOperatorStatusResults.Any())
                return new PushChargingStationOperatorStatusResult(AuthId,
                                                ISendStatus,
                                                PushChargingStationOperatorStatusResultTypes.Error,
                                                "!",
                                                new ChargingStationOperatorStatusUpdate[0],
                                                new Warning[0],
                                                Runtime);

            #endregion

            var All                        = PushChargingStationOperatorStatusResults.ToArray();

            var ResultOverview             = All.GroupBy     (_ => _.Result).
                                                 ToDictionary(_ => _.Key,
                                                              _ => new List<PushChargingStationOperatorStatusResult>(_));

            var Descriptions               = All.Where       (_ => _ != null).
                                                 SafeSelect  (_ => _.Description).
                                                 AggregateWith(Environment.NewLine);

            var RejectedChargingStationOperatorStatusUpdates  = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.RejectedChargingStationOperatorStatusUpdates);

            var Warnings                   = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushChargingStationOperatorStatusResult(All[0].AuthId,
                                                    ISendStatus,
                                                    result.Key,
                                                    Descriptions,
                                                    RejectedChargingStationOperatorStatusUpdates,
                                                    Warnings,
                                                    Runtime);

            return new PushChargingStationOperatorStatusResult(All[0].AuthId,
                                            ISendStatus,
                                            PushChargingStationOperatorStatusResultTypes.Partial,
                                            Descriptions,
                                            RejectedChargingStationOperatorStatusUpdates,
                                            Warnings,
                                            Runtime);

        }



        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }



    public enum PushChargingStationOperatorStatusResultTypes
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

        Partial,

        NoOperation,

        Enqueued,

        Error,

        LockTimeout

    }

}
