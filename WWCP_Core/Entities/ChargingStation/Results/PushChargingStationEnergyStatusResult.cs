/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A PushChargingStationEnergyStatus result.
    /// </summary>
    public class PushChargingStationEnergyStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                             SenderId                                        { get; }

        /// <summary>
        /// An object implementing ISendEnergyStatus.
        /// </summary>
        public ISendEnergyStatus?                              ISendEnergyStatus                             { get; }

        /// <summary>
        /// An object implementing IReceiveEnergyStatus.
        /// </summary>
        public IReceiveEnergyStatus?                           IReceiveEnergyStatus                          { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushChargingStationEnergyStatusResultTypes      Result                                        { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                         Description                                   { get; }

        /// <summary>
        /// An enumeration of rejected ChargingStation status updates.
        /// </summary>
        public IEnumerable<ChargingStationEnergyStatusUpdate>  RejectedChargingStationEnergyStatusUpdates    { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                            Warnings                                      { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan                                        Runtime                                       { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushChargingStationEnergyStatusResult(SenderId,                       Result, ...)

        /// <summary>
        /// Create a new PushChargingStationEnergyStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationEnergyStatusUpdates">An enumeration of rejected ChargingStation status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationEnergyStatusResult(IId                                              SenderId,
                                                      PushChargingStationEnergyStatusResultTypes       Result,
                                                      String?                                          Description                                  = null,
                                                      IEnumerable<ChargingStationEnergyStatusUpdate>?  RejectedChargingStationEnergyStatusUpdates   = null,
                                                      IEnumerable<Warning>?                            Warnings                                     = null,
                                                      TimeSpan?                                        Runtime                                      = null)
        {

            this.SenderId                                    = SenderId;
            this.Result                                      = Result;
            this.Description                                 = Description?.Trim();
            this.RejectedChargingStationEnergyStatusUpdates  = RejectedChargingStationEnergyStatusUpdates?.Distinct() ?? Array.Empty<ChargingStationEnergyStatusUpdate>();
            this.Warnings                                    = Warnings?.                                  Distinct() ?? Array.Empty<Warning>();
            this.Runtime                                     = Runtime                                                ?? TimeSpan.Zero;

        }

        #endregion

        #region (internal) PushChargingStationEnergyStatusResult(SenderId, ISendEnergyStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingStationEnergyStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="ISendEnergyStatus">An object implementing ISendEnergyStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationEnergyStatusUpdates">An enumeration of rejected ChargingStation status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationEnergyStatusResult(IId                                              SenderId,
                                                       ISendEnergyStatus                                ISendEnergyStatus,
                                                       PushChargingStationEnergyStatusResultTypes       Result,
                                                       String?                                          Description                                  = null,
                                                       IEnumerable<ChargingStationEnergyStatusUpdate>?  RejectedChargingStationEnergyStatusUpdates   = null,
                                                       IEnumerable<Warning>?                            Warnings                                     = null,
                                                       TimeSpan?                                        Runtime                                      = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedChargingStationEnergyStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendEnergyStatus = ISendEnergyStatus;

        }

        #endregion

        #region (internal) PushChargingStationEnergyStatusResult(SenderId, IReceiveEnergyStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingStationEnergyStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="IReceiveEnergyStatus">An object implementing IReceiveEnergyStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationEnergyStatusUpdates">An enumeration of rejected ChargingStation status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationEnergyStatusResult(IId                                              SenderId,
                                                       IReceiveEnergyStatus                             IReceiveEnergyStatus,
                                                       PushChargingStationEnergyStatusResultTypes       Result,
                                                       String?                                          Description                                  = null,
                                                       IEnumerable<ChargingStationEnergyStatusUpdate>?  RejectedChargingStationEnergyStatusUpdates   = null,
                                                       IEnumerable<Warning>?                            Warnings                                     = null,
                                                       TimeSpan?                                        Runtime                                      = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedChargingStationEnergyStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveEnergyStatus = IReceiveEnergyStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingStationEnergyStatusResult

            Success(IId                    SenderId,
                    ISendEnergyStatus      ISendEnergyStatus,
                    String?                Description    = null,
                    IEnumerable<Warning>?  Warnings       = null,
                    TimeSpan?              Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);


        public static PushChargingStationEnergyStatusResult

            Success(IId                    SenderId,
                    IReceiveEnergyStatus   IReceiveEnergyStatus,
                    String?                Description    = null,
                    IEnumerable<Warning>?  Warnings       = null,
                    TimeSpan?              Runtime        = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushChargingStationEnergyStatusResult

            Enqueued(IId                    SenderId,
                     ISendEnergyStatus            ISendEnergyStatus,
                     String?                Description    = null,
                     IEnumerable<Warning>?  Warnings       = null,
                     TimeSpan?              Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<ChargingStationEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationEnergyStatusResult

            NoOperation(IId                             SenderId,
                        ISendEnergyStatus                     ISendEnergyStatus,
                        String?                         Description                 = null,
                        IEnumerable<ChargingStationEnergyStatusUpdate>?  RejectedChargingStationEnergyStatusUpdates   = null,
                        IEnumerable<Warning>?           Warnings                    = null,
                        TimeSpan?                       Runtime                     = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.NoOperation,
                    Description,
                    RejectedChargingStationEnergyStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingStationEnergyStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveEnergyStatus         IReceiveEnergyStatus,
                        String?                Description    = null,
                        IEnumerable<Warning>?  Warnings       = null,
                        TimeSpan?              Runtime        = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushChargingStationEnergyStatusResult

            OutOfService(IId                            SenderId,
                         ISendEnergyStatus                    ISendEnergyStatus,
                         IEnumerable<ChargingStationEnergyStatusUpdate>  RejectedChargingStationEnergyStatusUpdates,
                         String?                        Description    = null,
                         IEnumerable<Warning>?          Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationEnergyStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingStationEnergyStatusResult

            OutOfService(IId                            SenderId,
                         IReceiveEnergyStatus                 IReceiveEnergyStatus,
                         IEnumerable<ChargingStationEnergyStatusUpdate>  RejectedChargingStationEnergyStatusUpdates,
                         String?                        Description    = null,
                         IEnumerable<Warning>?          Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationEnergyStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushChargingStationEnergyStatusResult

            AdminDown(IId                            SenderId,
                      ISendEnergyStatus                    ISendEnergyStatus,
                      IEnumerable<ChargingStationEnergyStatusUpdate>  RejectedChargingStationEnergyStatusUpdates,
                      String?                        Description   = null,
                      IEnumerable<Warning>?          Warnings      = null,
                      TimeSpan?                      Runtime       = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationEnergyStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingStationEnergyStatusResult

            AdminDown(IId                            SenderId,
                      IReceiveEnergyStatus                 IReceiveEnergyStatus,
                      IEnumerable<ChargingStationEnergyStatusUpdate>  RejectedChargingStationEnergyStatusUpdates,
                      String?                        Description   = null,
                      IEnumerable<Warning>?          Warnings      = null,
                      TimeSpan?                      Runtime       = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationEnergyStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region Error

        public static PushChargingStationEnergyStatusResult

            Error(IId                             SenderId,
                  ISendEnergyStatus                     ISendEnergyStatus,
                  IEnumerable<ChargingStationEnergyStatusUpdate>?  RejectedChargingStations   = null,
                  String?                         Description     = null,
                  IEnumerable<Warning>?           Warnings        = null,
                  TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.Error,
                    Description,
                    RejectedChargingStations,
                    Warnings,
                    Runtime);


        public static PushChargingStationEnergyStatusResult

            Error(IId                             SenderId,
                  IReceiveEnergyStatus                  IReceiveEnergyStatus,
                  IEnumerable<ChargingStationEnergyStatusUpdate>?  RejectedChargingStations   = null,
                  String?                         Description     = null,
                  IEnumerable<Warning>?           Warnings        = null,
                  TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.Error,
                    Description,
                    RejectedChargingStations,
                    Warnings,
                    Runtime);

        #endregion

        #region Failed

        public static PushChargingStationEnergyStatusResult

            Failed(IId                             SenderId,
                   ISendEnergyStatus                     ISendEnergyStatus,
                   IEnumerable<ChargingStationEnergyStatusUpdate>?  RejectedChargingStations   = null,
                   String?                         Description     = null,
                   IEnumerable<Warning>?           Warnings        = null,
                   TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.Error,
                    Description,
                    RejectedChargingStations,
                    Warnings,
                    Runtime);


        public static PushChargingStationEnergyStatusResult

            Failed(IId                             SenderId,
                   IReceiveEnergyStatus                  IReceiveEnergyStatus,
                   IEnumerable<ChargingStationEnergyStatusUpdate>?  RejectedChargingStations   = null,
                   String?                         Description     = null,
                   IEnumerable<Warning>?           Warnings        = null,
                   TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.Failed,
                    Description,
                    RejectedChargingStations,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingStationEnergyStatusResult

            LockTimeout(IId                    SenderId,
                        ISendEnergyStatus            ISendEnergyStatus,
                        String?                Description    = null,
                        IEnumerable<Warning>?  Warnings       = null,
                        TimeSpan?              Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushChargingStationEnergyStatusResultTypes.LockTimeout,
                    Description,
                    Array.Empty<ChargingStationEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Flatten(...)

        public static PushChargingStationEnergyStatusResult Flatten(IId                                SenderId,
                                                   ISendEnergyStatus                        ISendEnergyStatus,
                                                   IEnumerable<PushChargingStationEnergyStatusResult>  PushChargingStationEnergyStatusResults,
                                                   TimeSpan                           Runtime)
        {

            #region Initial checks

            if (PushChargingStationEnergyStatusResults == null || !PushChargingStationEnergyStatusResults.Any())
                return new PushChargingStationEnergyStatusResult(SenderId,
                                                ISendEnergyStatus,
                                                PushChargingStationEnergyStatusResultTypes.Error,
                                                "!",
                                                new ChargingStationEnergyStatusUpdate[0],
                                                new Warning[0],
                                                Runtime);

            #endregion

            var All                        = PushChargingStationEnergyStatusResults.ToArray();

            var ResultOverview             = All.GroupBy     (_ => _.Result).
                                                 ToDictionary(_ => _.Key,
                                                              _ => new List<PushChargingStationEnergyStatusResult>(_));

            var Descriptions               = All.Where       (_ => _ != null).
                                                 SafeSelect  (_ => _.Description).
                                                 AggregateWith(Environment.NewLine);

            var RejectedChargingStationEnergyStatusUpdates  = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.RejectedChargingStationEnergyStatusUpdates);

            var Warnings                   = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushChargingStationEnergyStatusResult(All[0].SenderId,
                                                    ISendEnergyStatus,
                                                    result.Key,
                                                    Descriptions,
                                                    RejectedChargingStationEnergyStatusUpdates,
                                                    Warnings,
                                                    Runtime);

            return new PushChargingStationEnergyStatusResult(All[0].SenderId,
                                            ISendEnergyStatus,
                                            PushChargingStationEnergyStatusResultTypes.Partial,
                                            Descriptions,
                                            RejectedChargingStationEnergyStatusUpdates,
                                            Warnings,
                                            Runtime);

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }


    public enum PushChargingStationEnergyStatusResultTypes
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

        Failed,

        LockTimeout

    }

}
