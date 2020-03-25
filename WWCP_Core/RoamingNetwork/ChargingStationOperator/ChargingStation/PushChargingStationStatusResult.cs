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
    /// A PushChargingStationStatus result.
    /// </summary>
    public class PushChargingStationStatusResult
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
        public PushChargingStationStatusResultTypes      Result                       { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String                         Description                  { get; }

        /// <summary>
        /// An enumeration of rejected ChargingStation status updates.
        /// </summary>
        public IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates    { get; }

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

        #region (private)  PushChargingStationStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushChargingStationStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationStatusUpdates">An enumeration of rejected ChargingStation status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationStatusResult(IId                            AuthId,
                                     PushChargingStationStatusResultTypes      Result,
                                     String                         Description                 = null,
                                     IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates   = null,
                                     IEnumerable<Warning>           Warnings                    = null,
                                     TimeSpan?                      Runtime                     = null)
        {

            this.AuthId                     = AuthId;
            this.Result                     = Result;

            this.Description                = Description.IsNotNullOrEmpty()
                                                  ? Description.Trim()
                                                  : null;

            this.RejectedChargingStationStatusUpdates  = RejectedChargingStationStatusUpdates != null
                                                  ? RejectedChargingStationStatusUpdates.Where(evse => evse != null)
                                                  : new ChargingStationStatusUpdate[0];

            this.Warnings                   = Warnings != null
                                                  ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                  : new Warning[0];

            this.Runtime                    = Runtime;

        }

        #endregion

        #region (internal) PushChargingStationStatusResult(AuthId, ISendStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingStationStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendStatus">An object implementing ISendStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationStatusUpdates">An enumeration of rejected ChargingStation status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationStatusResult(IId                            AuthId,
                                      ISendStatus                    ISendStatus,
                                      PushChargingStationStatusResultTypes      Result,
                                      String                         Description                 = null,
                                      IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates   = null,
                                      IEnumerable<Warning>           Warnings                    = null,
                                      TimeSpan?                      Runtime                     = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingStationStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendStatus = ISendStatus;

        }

        #endregion

        #region (internal) PushChargingStationStatusResult(AuthId, IReceiveStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingStationStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveStatus">An object implementing IReceiveStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationStatusUpdates">An enumeration of rejected ChargingStation status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationStatusResult(IId                            AuthId,
                                      IReceiveStatus                 IReceiveStatus,
                                      PushChargingStationStatusResultTypes      Result,
                                      String                         Description                 = null,
                                      IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates   = null,
                                      IEnumerable<Warning>           Warnings                    = null,
                                      TimeSpan?                      Runtime                     = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingStationStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveStatus = IReceiveStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingStationStatusResult

            Success(IId                   AuthId,
                    ISendStatus           ISendStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationStatusResultTypes.Success,
                                        Description,
                                        new ChargingStationStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationStatusResult

            Success(IId                   AuthId,
                    IReceiveStatus        IReceiveStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationStatusResultTypes.Success,
                                        Description,
                                        new ChargingStationStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Enqueued

        public static PushChargingStationStatusResult

            Enqueued(IId                   AuthId,
                     ISendStatus           ISendStatus,
                     String                Description    = null,
                     IEnumerable<Warning>  Warnings       = null,
                     TimeSpan?             Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationStatusResultTypes.Enqueued,
                                        Description,
                                        new ChargingStationStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationStatusResult

            NoOperation(IId                   AuthId,
                        ISendStatus           ISendStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationStatusResultTypes.NoOperation,
                                        Description,
                                        new ChargingStationStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationStatusResult

            NoOperation(IId                   AuthId,
                        IReceiveStatus        IReceiveStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationStatusResultTypes.NoOperation,
                                        Description,
                                        new ChargingStationStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region OutOfService

        public static PushChargingStationStatusResult

            OutOfService(IId                            AuthId,
                         ISendStatus                    ISendStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedChargingStationStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingStationStatusResult

            OutOfService(IId                            AuthId,
                         IReceiveStatus                 IReceiveStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedChargingStationStatusUpdates,
                                        Warnings,
                                        Runtime);


        public static PushChargingStationStatusResult

            OutOfService(IId                                    AuthId,
                         ISendStatus                            ISendStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingStationStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationStatusResult

            OutOfService(IId                                    AuthId,
                         IReceiveStatus                         IReceiveStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingStationStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region AdminDown

        public static PushChargingStationStatusResult

            AdminDown(IId                            AuthId,
                      ISendStatus                    ISendStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedChargingStationStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingStationStatusResult

            AdminDown(IId                            AuthId,
                      IReceiveStatus                 IReceiveStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedChargingStationStatusUpdates,
                                        Warnings,
                                        Runtime);


        public static PushChargingStationStatusResult

            AdminDown(IId                                    AuthId,
                      IReceiveStatus                         IReceiveStatus,
                      IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingStationStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Error

        public static PushChargingStationStatusResult

            Error(IId                            AuthId,
                  ISendStatus                    ISendStatus,
                  IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStations  = null,
                  String                         Description    = null,
                  IEnumerable<Warning>           Warnings       = null,
                  TimeSpan?                      Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationStatusResultTypes.Error,
                                        Description,
                                        RejectedChargingStations,
                                        Warnings,
                                        Runtime);


        public static PushChargingStationStatusResult Error(IId                            AuthId,
                                                 IReceiveStatus                 IReceiveStatus,
                                                 IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStations  = null,
                                                 String                         Description    = null,
                                                 IEnumerable<Warning>           Warnings       = null,
                                                 TimeSpan?                      Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushChargingStationStatusResultTypes.Error,
                                        Description,
                                        RejectedChargingStations,
                                        Warnings,
                                        Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingStationStatusResult LockTimeout(IId                   AuthId,
                                                       ISendStatus           ISendStatus,
                                                       String                Description    = null,
                                                       IEnumerable<Warning>  Warnings       = null,
                                                       TimeSpan?             Runtime        = null)

            => new PushChargingStationStatusResult(AuthId,
                                        ISendStatus,
                                        PushChargingStationStatusResultTypes.LockTimeout,
                                        Description,
                                        new ChargingStationStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion



        public static PushChargingStationStatusResult Flatten(IId                                AuthId,
                                                   ISendStatus                        ISendStatus,
                                                   IEnumerable<PushChargingStationStatusResult>  PushChargingStationStatusResults,
                                                   TimeSpan                           Runtime)
        {

            #region Initial checks

            if (PushChargingStationStatusResults == null || !PushChargingStationStatusResults.Any())
                return new PushChargingStationStatusResult(AuthId,
                                                ISendStatus,
                                                PushChargingStationStatusResultTypes.Error,
                                                "!",
                                                new ChargingStationStatusUpdate[0],
                                                new Warning[0],
                                                Runtime);

            #endregion

            var All                        = PushChargingStationStatusResults.ToArray();

            var ResultOverview             = All.GroupBy     (_ => _.Result).
                                                 ToDictionary(_ => _.Key,
                                                              _ => new List<PushChargingStationStatusResult>(_));

            var Descriptions               = All.Where       (_ => _ != null).
                                                 SafeSelect  (_ => _.Description).
                                                 AggregateWith(Environment.NewLine);

            var RejectedChargingStationStatusUpdates  = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.RejectedChargingStationStatusUpdates);

            var Warnings                   = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushChargingStationStatusResult(All[0].AuthId,
                                                    ISendStatus,
                                                    result.Key,
                                                    Descriptions,
                                                    RejectedChargingStationStatusUpdates,
                                                    Warnings,
                                                    Runtime);

            return new PushChargingStationStatusResult(All[0].AuthId,
                                            ISendStatus,
                                            PushChargingStationStatusResultTypes.Partial,
                                            Descriptions,
                                            RejectedChargingStationStatusUpdates,
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



    public enum PushChargingStationStatusResultTypes
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
