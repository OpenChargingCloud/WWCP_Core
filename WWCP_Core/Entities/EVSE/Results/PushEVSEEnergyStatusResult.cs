/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A PushEVSEEnergyStatus result.
    /// </summary>
    public class PushEVSEEnergyStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                  AuthId                             { get; }

        /// <summary>
        /// An object implementing ISendEnergyStatus.
        /// </summary>
        public ISendEnergyStatus?                   ISendEnergyStatus                  { get; }

        /// <summary>
        /// An object implementing IReceiveEnergyStatus.
        /// </summary>
        public IReceiveEnergyStatus?                IReceiveEnergyStatus               { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushEVSEEnergyStatusResultTypes      Result                             { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                              Description                        { get; }

        /// <summary>
        /// An enumeration of rejected EVSE status updates.
        /// </summary>
        public IEnumerable<EVSEEnergyStatusUpdate>  RejectedEVSEEnergyStatusUpdates    { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                 Warnings                           { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan                             Runtime                            { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushEVSEEnergyStatusResult(SenderId,                 Result, ...)

        /// <summary>
        /// Create a new PushEVSEEnergyStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEVSEEnergyStatusUpdates">An enumeration of rejected EVSE status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushEVSEEnergyStatusResult(IId                                   SenderId,
                                           PushEVSEEnergyStatusResultTypes       Result,
                                           String?                               Description                       = null,
                                           IEnumerable<EVSEEnergyStatusUpdate>?  RejectedEVSEEnergyStatusUpdates   = null,
                                           IEnumerable<Warning>?                 Warnings                          = null,
                                           TimeSpan?                             Runtime                           = null)
        {

            this.AuthId                           = AuthId;
            this.Result                           = Result;
            this.Description                      = Description?.Trim();
            this.RejectedEVSEEnergyStatusUpdates  = RejectedEVSEEnergyStatusUpdates?.Distinct() ?? Array.Empty<EVSEEnergyStatusUpdate>();
            this.Warnings                         = Warnings?.                       Distinct() ?? Array.Empty<Warning>();
            this.Runtime                          = Runtime                                     ?? TimeSpan.Zero;

        }

        #endregion

        #region (internal) PushEVSEEnergyStatusResult(SenderId, ISendEnergyStatus,    Result, ...)

        /// <summary>
        /// Create a new PushEVSEEnergyStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendEnergyStatus">An object implementing ISendEnergyStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEVSEEnergyStatusUpdates">An enumeration of rejected EVSE status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEVSEEnergyStatusResult(IId                                   SenderId,
                                            ISendEnergyStatus                     ISendEnergyStatus,
                                            PushEVSEEnergyStatusResultTypes       Result,
                                            String?                               Description                       = null,
                                            IEnumerable<EVSEEnergyStatusUpdate>?  RejectedEVSEEnergyStatusUpdates   = null,
                                            IEnumerable<Warning>?                 Warnings                          = null,
                                            TimeSpan?                             Runtime                           = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedEVSEEnergyStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendEnergyStatus = ISendEnergyStatus;

        }

        #endregion

        #region (internal) PushEVSEEnergyStatusResult(SenderId, IReceiveEnergyStatus, Result, ...)

        /// <summary>
        /// Create a new PushEVSEEnergyStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveEnergyStatus">An object implementing IReceiveEnergyStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEVSEEnergyStatusUpdates">An enumeration of rejected EVSE status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEVSEEnergyStatusResult(IId                                   SenderId,
                                            IReceiveEnergyStatus                  IReceiveEnergyStatus,
                                            PushEVSEEnergyStatusResultTypes       Result,
                                            String?                               Description                       = null,
                                            IEnumerable<EVSEEnergyStatusUpdate>?  RejectedEVSEEnergyStatusUpdates   = null,
                                            IEnumerable<Warning>?                 Warnings                          = null,
                                            TimeSpan?                             Runtime                           = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedEVSEEnergyStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveEnergyStatus = IReceiveEnergyStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushEVSEEnergyStatusResult

            Success(IId                    SenderId,
                    ISendEnergyStatus            ISendEnergyStatus,
                    String?                Description    = null,
                    IEnumerable<Warning>?  Warnings       = null,
                    TimeSpan?              Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.Success,
                    Description,
                    Array.Empty<EVSEEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);


        public static PushEVSEEnergyStatusResult

            Success(IId                    SenderId,
                    IReceiveEnergyStatus         IReceiveEnergyStatus,
                    String?                Description    = null,
                    IEnumerable<Warning>?  Warnings       = null,
                    TimeSpan?              Runtime        = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.Success,
                    Description,
                    Array.Empty<EVSEEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushEVSEEnergyStatusResult

            Enqueued(IId                    SenderId,
                     ISendEnergyStatus            ISendEnergyStatus,
                     String?                Description    = null,
                     IEnumerable<Warning>?  Warnings       = null,
                     TimeSpan?              Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<EVSEEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushEVSEEnergyStatusResult

            NoOperation(IId                             SenderId,
                        ISendEnergyStatus                     ISendEnergyStatus,
                        String?                         Description                 = null,
                        IEnumerable<EVSEEnergyStatusUpdate>?  RejectedEVSEEnergyStatusUpdates   = null,
                        IEnumerable<Warning>?           Warnings                    = null,
                        TimeSpan?                       Runtime                     = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.NoOperation,
                    Description,
                    RejectedEVSEEnergyStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushEVSEEnergyStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveEnergyStatus         IReceiveEnergyStatus,
                        String?                Description    = null,
                        IEnumerable<Warning>?  Warnings       = null,
                        TimeSpan?              Runtime        = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EVSEEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushEVSEEnergyStatusResult

            OutOfService(IId                            SenderId,
                         ISendEnergyStatus                    ISendEnergyStatus,
                         IEnumerable<EVSEEnergyStatusUpdate>  RejectedEVSEEnergyStatusUpdates,
                         String?                        Description    = null,
                         IEnumerable<Warning>?          Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.OutOfService,
                    Description,
                    RejectedEVSEEnergyStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushEVSEEnergyStatusResult

            OutOfService(IId                            SenderId,
                         IReceiveEnergyStatus                 IReceiveEnergyStatus,
                         IEnumerable<EVSEEnergyStatusUpdate>  RejectedEVSEEnergyStatusUpdates,
                         String?                        Description    = null,
                         IEnumerable<Warning>?          Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.OutOfService,
                    Description,
                    RejectedEVSEEnergyStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushEVSEEnergyStatusResult

            AdminDown(IId                            SenderId,
                      ISendEnergyStatus                    ISendEnergyStatus,
                      IEnumerable<EVSEEnergyStatusUpdate>  RejectedEVSEEnergyStatusUpdates,
                      String?                        Description   = null,
                      IEnumerable<Warning>?          Warnings      = null,
                      TimeSpan?                      Runtime       = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.AdminDown,
                    Description,
                    RejectedEVSEEnergyStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushEVSEEnergyStatusResult

            AdminDown(IId                            SenderId,
                      IReceiveEnergyStatus                 IReceiveEnergyStatus,
                      IEnumerable<EVSEEnergyStatusUpdate>  RejectedEVSEEnergyStatusUpdates,
                      String?                        Description   = null,
                      IEnumerable<Warning>?          Warnings      = null,
                      TimeSpan?                      Runtime       = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.AdminDown,
                    Description,
                    RejectedEVSEEnergyStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region Error

        public static PushEVSEEnergyStatusResult

            Error(IId                             SenderId,
                  ISendEnergyStatus                     ISendEnergyStatus,
                  IEnumerable<EVSEEnergyStatusUpdate>?  RejectedEVSEs   = null,
                  String?                         Description     = null,
                  IEnumerable<Warning>?           Warnings        = null,
                  TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.Error,
                    Description,
                    RejectedEVSEs,
                    Warnings,
                    Runtime);


        public static PushEVSEEnergyStatusResult

            Error(IId                             SenderId,
                  IReceiveEnergyStatus                  IReceiveEnergyStatus,
                  IEnumerable<EVSEEnergyStatusUpdate>?  RejectedEVSEs   = null,
                  String?                         Description     = null,
                  IEnumerable<Warning>?           Warnings        = null,
                  TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.Error,
                    Description,
                    RejectedEVSEs,
                    Warnings,
                    Runtime);

        #endregion

        #region Failed

        public static PushEVSEEnergyStatusResult

            Failed(IId                             SenderId,
                   ISendEnergyStatus                     ISendEnergyStatus,
                   IEnumerable<EVSEEnergyStatusUpdate>?  RejectedEVSEs   = null,
                   String?                         Description     = null,
                   IEnumerable<Warning>?           Warnings        = null,
                   TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.Error,
                    Description,
                    RejectedEVSEs,
                    Warnings,
                    Runtime);


        public static PushEVSEEnergyStatusResult

            Failed(IId                             SenderId,
                   IReceiveEnergyStatus                  IReceiveEnergyStatus,
                   IEnumerable<EVSEEnergyStatusUpdate>?  RejectedEVSEs   = null,
                   String?                         Description     = null,
                   IEnumerable<Warning>?           Warnings        = null,
                   TimeSpan?                       Runtime         = null)

            => new (SenderId,
                    IReceiveEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.Failed,
                    Description,
                    RejectedEVSEs,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushEVSEEnergyStatusResult

            LockTimeout(IId                    SenderId,
                        ISendEnergyStatus            ISendEnergyStatus,
                        String?                Description    = null,
                        IEnumerable<Warning>?  Warnings       = null,
                        TimeSpan?              Runtime        = null)

            => new (SenderId,
                    ISendEnergyStatus,
                    PushEVSEEnergyStatusResultTypes.LockTimeout,
                    Description,
                    Array.Empty<EVSEEnergyStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Flatten(...)

        public static PushEVSEEnergyStatusResult Flatten(IId                                SenderId,
                                                   ISendEnergyStatus                        ISendEnergyStatus,
                                                   IEnumerable<PushEVSEEnergyStatusResult>  PushEVSEEnergyStatusResults,
                                                   TimeSpan                           Runtime)
        {

            #region Initial checks

            if (PushEVSEEnergyStatusResults == null || !PushEVSEEnergyStatusResults.Any())
                return new PushEVSEEnergyStatusResult(SenderId,
                                                ISendEnergyStatus,
                                                PushEVSEEnergyStatusResultTypes.Error,
                                                "!",
                                                new EVSEEnergyStatusUpdate[0],
                                                new Warning[0],
                                                Runtime);

            #endregion

            var All                        = PushEVSEEnergyStatusResults.ToArray();

            var ResultOverview             = All.GroupBy     (_ => _.Result).
                                                 ToDictionary(_ => _.Key,
                                                              _ => new List<PushEVSEEnergyStatusResult>(_));

            var Descriptions               = All.Where       (_ => _ != null).
                                                 SafeSelect  (_ => _.Description).
                                                 AggregateWith(Environment.NewLine);

            var RejectedEVSEEnergyStatusUpdates  = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.RejectedEVSEEnergyStatusUpdates);

            var Warnings                   = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushEVSEEnergyStatusResult(All[0].SenderId,
                                                    ISendEnergyStatus,
                                                    result.Key,
                                                    Descriptions,
                                                    RejectedEVSEEnergyStatusUpdates,
                                                    Warnings,
                                                    Runtime);

            return new PushEVSEEnergyStatusResult(All[0].SenderId,
                                            ISendEnergyStatus,
                                            PushEVSEEnergyStatusResultTypes.Partial,
                                            Descriptions,
                                            RejectedEVSEEnergyStatusUpdates,
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


    public enum PushEVSEEnergyStatusResultTypes
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
