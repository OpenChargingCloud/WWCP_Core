/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A PushGridOperatorStatus result.
    /// </summary>
    public class PushGridOperatorStatusResult
    {

        #region Properties

        /// <summary>
        /// The unique identification of the authenticator.
        /// </summary>
        public IId                                               SenderId                                           { get; }

        /// <summary>
        /// An object implementing ISendStatus.
        /// </summary>
        public ISendStatus?                                      ISendStatus                                      { get; }

        /// <summary>
        /// An object implementing IReceiveStatus.
        /// </summary>
        public IReceiveStatus?                                   IReceiveStatus                                   { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushGridOperatorStatusResultTypes      Result                                           { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                           Description                                      { get; }

        /// <summary>
        /// An enumeration of rejected charging station operator status updates.
        /// </summary>
        public IEnumerable<GridOperatorStatusUpdate>  RejectedGridOperatorStatusUpdates     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                              Warnings                                         { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                                         Runtime                                          { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushGridOperatorStatusResult(SenderId,                 Result, ...)

        /// <summary>
        /// Create a new PushGridOperatorStatus result.
        /// </summary>
        /// <param name="SenderId">The unique identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedGridOperatorStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushGridOperatorStatusResult(IId                                                SenderId,
                                                        PushGridOperatorStatusResultTypes       Result,
                                                        String?                                            Description                                    = null,
                                                        IEnumerable<GridOperatorStatusUpdate>?  RejectedGridOperatorStatusUpdates   = null,
                                                        IEnumerable<Warning>?                              Warnings                                       = null,
                                                        TimeSpan?                                          Runtime                                        = null)
        {

            this.SenderId                                      = SenderId;
            this.Result                                        = Result;

            this.Description                                   = Description is not null && Description.IsNotNullOrEmpty()
                                                                     ? Description.Trim()
                                                                     : String.Empty;

            this.RejectedGridOperatorStatusUpdates  = RejectedGridOperatorStatusUpdates ?? Array.Empty<GridOperatorStatusUpdate>();

            this.Warnings                                      = Warnings is not null
                                                                     ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                                     : Array.Empty<Warning>();

            this.Runtime                                       = Runtime;

        }

        #endregion

        #region (internal) PushGridOperatorStatusResult(SenderId, ISendStatus,    Result, ...)

        /// <summary>
        /// Create a new PushGridOperatorStatus result.
        /// </summary>
        /// <param name="SenderId">The unique identification of the sender.</param>
        /// <param name="ISendStatus">An object implementing ISendStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedGridOperatorStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushGridOperatorStatusResult(IId                                                SenderId,
                                                         ISendStatus                                        ISendStatus,
                                                         PushGridOperatorStatusResultTypes       Result,
                                                         String?                                            Description                                    = null,
                                                         IEnumerable<GridOperatorStatusUpdate>?  RejectedGridOperatorStatusUpdates   = null,
                                                         IEnumerable<Warning>?                              Warnings                                       = null,
                                                         TimeSpan?                                          Runtime                                        = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedGridOperatorStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendStatus = ISendStatus;

        }

        #endregion

        #region (internal) PushGridOperatorStatusResult(SenderId, IReceiveStatus, Result, ...)

        /// <summary>
        /// Create a new PushGridOperatorStatus result.
        /// </summary>
        /// <param name="SenderId">The unique identification of the sender.</param>
        /// <param name="IReceiveStatus">An object implementing IReceiveStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedGridOperatorStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushGridOperatorStatusResult(IId                                                SenderId,
                                                         IReceiveStatus                                     IReceiveStatus,
                                                         PushGridOperatorStatusResultTypes       Result,
                                                         String?                                            Description                                    = null,
                                                         IEnumerable<GridOperatorStatusUpdate>?  RejectedGridOperatorStatusUpdates   = null,
                                                         IEnumerable<Warning>?                              Warnings                                       = null,
                                                         TimeSpan?                                          Runtime                                        = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedGridOperatorStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveStatus = IReceiveStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushGridOperatorStatusResult

            Success(IId                    SenderId,
                    ISendStatus            ISendStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushGridOperatorStatusResultTypes.Success,
                    Description,
                    Array.Empty<GridOperatorStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushGridOperatorStatusResult

            Success(IId                    SenderId,
                    IReceiveStatus         IReceiveStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushGridOperatorStatusResultTypes.Success,
                    Description,
                    Array.Empty<GridOperatorStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushGridOperatorStatusResult

            Enqueued(IId                    SenderId,
                     ISendStatus            ISendStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushGridOperatorStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<GridOperatorStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushGridOperatorStatusResult

            NoOperation(IId                    SenderId,
                        ISendStatus            ISendStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushGridOperatorStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<GridOperatorStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushGridOperatorStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveStatus         IReceiveStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushGridOperatorStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<GridOperatorStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushGridOperatorStatusResult

            OutOfService(IId                                               SenderId,
                         ISendStatus                                       ISendStatus,
                         IEnumerable<GridOperatorStatusUpdate>  RejectedGridOperatorStatusUpdates,
                         String?                                           Description   = null,
                         IEnumerable<Warning>?                             Warnings      = null,
                         TimeSpan?                                         Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushGridOperatorStatusResultTypes.OutOfService,
                    Description,
                    RejectedGridOperatorStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushGridOperatorStatusResult

            OutOfService(IId                                               SenderId,
                         IReceiveStatus                                    IReceiveStatus,
                         IEnumerable<GridOperatorStatusUpdate>  RejectedGridOperatorStatusUpdates,
                         String?                                           Description   = null,
                         IEnumerable<Warning>?                             Warnings      = null,
                         TimeSpan?                                         Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushGridOperatorStatusResultTypes.OutOfService,
                    Description,
                    RejectedGridOperatorStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushGridOperatorStatusResult

            AdminDown(IId                                               SenderId,
                      ISendStatus                                       ISendStatus,
                      IEnumerable<GridOperatorStatusUpdate>  RejectedGridOperatorStatusUpdates,
                      String?                                           Description   = null,
                      IEnumerable<Warning>?                             Warnings      = null,
                      TimeSpan?                                         Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushGridOperatorStatusResultTypes.AdminDown,
                    Description,
                    RejectedGridOperatorStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushGridOperatorStatusResult

            AdminDown(IId                                               SenderId,
                      IReceiveStatus                                    IReceiveStatus,
                      IEnumerable<GridOperatorStatusUpdate>  RejectedGridOperatorStatusUpdates,
                      String?                                           Description   = null,
                      IEnumerable<Warning>?                             Warnings      = null,
                      TimeSpan?                                         Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushGridOperatorStatusResultTypes.AdminDown,
                    Description,
                    RejectedGridOperatorStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushGridOperatorStatusResult

            Error(IId                                                SenderId,
                  ISendStatus                                        ISendStatus,
                  IEnumerable<GridOperatorStatusUpdate>?  RejectedGridOperatorStatusUpdates   = null,
                  String?                                            Description                                    = null,
                  IEnumerable<Warning>?                              Warnings                                       = null,
                  TimeSpan?                                          Runtime                                        = null)

            => new (SenderId,
                    ISendStatus,
                    PushGridOperatorStatusResultTypes.Error,
                    Description,
                    RejectedGridOperatorStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushGridOperatorStatusResult

            Error(IId                                                SenderId,
                  IReceiveStatus                                     IReceiveStatus,
                  IEnumerable<GridOperatorStatusUpdate>?  RejectedGridOperatorStatusUpdates   = null,
                  String?                                            Description                                    = null,
                  IEnumerable<Warning>?                              Warnings                                       = null,
                  TimeSpan?                                          Runtime                                        = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushGridOperatorStatusResultTypes.Error,
                    Description,
                    RejectedGridOperatorStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushGridOperatorStatusResult

            LockTimeout(IId                                               SenderId,
                        ISendStatus                                       ISendStatus,
                        IEnumerable<GridOperatorStatusUpdate>  RejectedGridOperatorStatusUpdates,
                        String?                                           Description   = null,
                        IEnumerable<Warning>?                             Warnings      = null,
                        TimeSpan?                                         Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushGridOperatorStatusResultTypes.LockTimeout,
                    Description,
                    RejectedGridOperatorStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(SenderId, ISendStatus, PushGridOperatorStatusResults, Runtime)

        public static PushGridOperatorStatusResult Flatten(IId                                                   SenderId,
                                                                      ISendStatus                                           ISendStatus,
                                                                      IEnumerable<PushGridOperatorStatusResult>  PushGridOperatorStatusResults,
                                                                      TimeSpan                                              Runtime)
        {

            #region Initial checks

            if (PushGridOperatorStatusResults is null || !PushGridOperatorStatusResults.Any())
                return new PushGridOperatorStatusResult(SenderId,
                                                                   ISendStatus,
                                                                   PushGridOperatorStatusResultTypes.Error,
                                                                   "!",
                                                                   Array.Empty<GridOperatorStatusUpdate>(),
                                                                   Array.Empty<Warning>(),
                                                                   Runtime);

            #endregion

            var all                                           = PushGridOperatorStatusResults.ToArray();

            var resultOverview                                = all.GroupBy      (result => result.Result).
                                                                    ToDictionary (result => result.Key,
                                                                                  result => new List<PushGridOperatorStatusResult>(result));

            var descriptions                                  = all.Where        (result => result is not null).
                                                                    SafeSelect   (result => result.Description).
                                                                    AggregateWith(Environment.NewLine);

            var rejectedGridOperatorStatusUpdates  = all.Where        (result => result is not null).
                                                                    SelectMany   (result => result.RejectedGridOperatorStatusUpdates);

            var warnings                                      = all.Where        (result => result is not null).
                                                                    SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushGridOperatorStatusResult(all[0].SenderId,
                                                                       ISendStatus,
                                                                       result.Key,
                                                                       descriptions,
                                                                       rejectedGridOperatorStatusUpdates,
                                                                       warnings,
                                                                       Runtime);

            return new PushGridOperatorStatusResult(all[0].SenderId,
                                                               ISendStatus,
                                                               PushGridOperatorStatusResultTypes.Partial,
                                                               descriptions,
                                                               rejectedGridOperatorStatusUpdates,
                                                               warnings,
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


    public enum PushGridOperatorStatusResultTypes
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
