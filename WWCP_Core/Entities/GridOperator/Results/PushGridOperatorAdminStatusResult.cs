/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A PushGridOperatorAdminStatus result.
    /// </summary>
    public class PushGridOperatorAdminStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                                    SenderId                                                { get; }

        /// <summary>
        /// An object implementing ISendAdminStatus.
        /// </summary>
        public ISendAdminStatus?                                      ISendAdminStatus                                      { get; }

        /// <summary>
        /// An object implementing IReceiveAdminStatus.
        /// </summary>
        public IReceiveAdminStatus?                                   IReceiveAdminStatus                                   { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushGridOperatorAdminStatusResultTypes      Result                                                { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                                Description                                           { get; }

        /// <summary>
        /// An enumeration of rejected charging station operator admin status updates.
        /// </summary>
        public IEnumerable<GridOperatorAdminStatusUpdate>  RejectedGridOperatorAdminStatusUpdates     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                                   Warnings                                              { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                                              Runtime                                               { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushGridOperatorAdminStatusResult(SenderId,                      Result, ...)

        /// <summary>
        /// Create a new PushGridOperatorAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedGridOperatorAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushGridOperatorAdminStatusResult(IId                                                     SenderId,
                                                             PushGridOperatorAdminStatusResultTypes       Result,
                                                             String?                                                 Description                                         = null,
                                                             IEnumerable<GridOperatorAdminStatusUpdate>?  RejectedGridOperatorAdminStatusUpdates   = null,
                                                             IEnumerable<Warning>?                                   Warnings                                            = null,
                                                             TimeSpan?                                               Runtime                                             = null)
        {

            this.SenderId                                           = SenderId;
            this.Result                                             = Result;

            this.Description                                        = Description is not null && Description.IsNotNullOrEmpty()
                                                                          ? Description.Trim()
                                                                          : String.Empty;

            this.RejectedGridOperatorAdminStatusUpdates  = RejectedGridOperatorAdminStatusUpdates ?? Array.Empty<GridOperatorAdminStatusUpdate>();

            this.Warnings                                           = Warnings is not null
                                                                          ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                                          : Array.Empty<Warning>();

            this.Runtime                                            = Runtime;

        }

        #endregion

        #region (internal) PushGridOperatorAdminStatusResult(SenderId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushGridOperatorAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedGridOperatorAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushGridOperatorAdminStatusResult(IId                                                     SenderId,
                                                              ISendAdminStatus                                        ISendAdminStatus,
                                                              PushGridOperatorAdminStatusResultTypes       Result,
                                                              String?                                                 Description                                         = null,
                                                              IEnumerable<GridOperatorAdminStatusUpdate>?  RejectedGridOperatorAdminStatusUpdates   = null,
                                                              IEnumerable<Warning>?                                   Warnings                                            = null,
                                                              TimeSpan?                                               Runtime                                             = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedGridOperatorAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushGridOperatorAdminStatusResult(SenderId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushGridOperatorAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedGridOperatorAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushGridOperatorAdminStatusResult(IId                                                     SenderId,
                                                              IReceiveAdminStatus                                     IReceiveAdminStatus,
                                                              PushGridOperatorAdminStatusResultTypes       Result,
                                                              String?                                                 Description                                         = null,
                                                              IEnumerable<GridOperatorAdminStatusUpdate>?  RejectedGridOperatorAdminStatusUpdates   = null,
                                                              IEnumerable<Warning>?                                   Warnings                                            = null,
                                                              TimeSpan?                                               Runtime                                             = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedGridOperatorAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushGridOperatorAdminStatusResult

            Success(IId                    SenderId,
                    ISendAdminStatus       ISendAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<GridOperatorAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushGridOperatorAdminStatusResult

            Success(IId                    SenderId,
                    IReceiveAdminStatus    IReceiveAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<GridOperatorAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushGridOperatorAdminStatusResult

            Enqueued(IId                    SenderId,
                     ISendAdminStatus       ISendAdminStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<GridOperatorAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushGridOperatorAdminStatusResult

            NoOperation(IId                    SenderId,
                        ISendAdminStatus       ISendAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<GridOperatorAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushGridOperatorAdminStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveAdminStatus    IReceiveAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<GridOperatorAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushGridOperatorAdminStatusResult

            OutOfService(IId                                                    SenderId,
                         ISendAdminStatus                                       ISendAdminStatus,
                         IEnumerable<GridOperatorAdminStatusUpdate>  RejectedGridOperatorAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedGridOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushGridOperatorAdminStatusResult

            OutOfService(IId                                                    SenderId,
                         IReceiveAdminStatus                                    IReceiveAdminStatus,
                         IEnumerable<GridOperatorAdminStatusUpdate>  RejectedGridOperatorAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedGridOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushGridOperatorAdminStatusResult

            AdminDown(IId                                                    SenderId,
                      ISendAdminStatus                                       ISendAdminStatus,
                      IEnumerable<GridOperatorAdminStatusUpdate>  RejectedGridOperatorAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedGridOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushGridOperatorAdminStatusResult

            AdminDown(IId                                                    SenderId,
                      IReceiveAdminStatus                                    IReceiveAdminStatus,
                      IEnumerable<GridOperatorAdminStatusUpdate>  RejectedGridOperatorAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedGridOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushGridOperatorAdminStatusResult

            Error(IId                                                     SenderId,
                  ISendAdminStatus                                        ISendAdminStatus,
                  IEnumerable<GridOperatorAdminStatusUpdate>?  RejectedGridOperatorAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.Error,
                    Description,
                    RejectedGridOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushGridOperatorAdminStatusResult

            Error(IId                                                     SenderId,
                  IReceiveAdminStatus                                     IReceiveAdminStatus,
                  IEnumerable<GridOperatorAdminStatusUpdate>?  RejectedGridOperatorAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.Error,
                    Description,
                    RejectedGridOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushGridOperatorAdminStatusResult

            LockTimeout(IId                                                    SenderId,
                        ISendAdminStatus                                       ISendAdminStatus,
                        IEnumerable<GridOperatorAdminStatusUpdate>  RejectedGridOperatorAdminStatusUpdates,
                        String?                                                Description   = null,
                        IEnumerable<Warning>?                                  Warnings      = null,
                        TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushGridOperatorAdminStatusResultTypes.LockTimeout,
                    Description,
                    RejectedGridOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(SenderId, ISendAdminStatus, PushGridOperatorAdminStatusResults, Runtime)

        public static PushGridOperatorAdminStatusResult Flatten(IId                                                        SenderId,
                                                                           ISendAdminStatus                                           ISendAdminStatus,
                                                                           IEnumerable<PushGridOperatorAdminStatusResult>  PushGridOperatorAdminStatusResults,
                                                                           TimeSpan                                                   Runtime)
        {

            #region Initial checks

            if (PushGridOperatorAdminStatusResults is null || !PushGridOperatorAdminStatusResults.Any())
                return new PushGridOperatorAdminStatusResult(SenderId,
                                                                        ISendAdminStatus,
                                                                        PushGridOperatorAdminStatusResultTypes.Error,
                                                                        "!",
                                                                        Array.Empty<GridOperatorAdminStatusUpdate>(),
                                                                        Array.Empty<Warning>(),
                                                                        Runtime);

            #endregion

            var all                                                = PushGridOperatorAdminStatusResults.ToArray();

            var resultOverview                                     = all.GroupBy      (result => result.Result).
                                                                         ToDictionary (result => result.Key,
                                                                                       result => new List<PushGridOperatorAdminStatusResult>(result));

            var descriptions                                       = all.Where        (result => result is not null).
                                                                         SafeSelect   (result => result.Description).
                                                                         AggregateWith(Environment.NewLine);

            var rejectedGridOperatorAdminStatusUpdates  = all.Where        (result => result is not null).
                                                                         SelectMany   (result => result.RejectedGridOperatorAdminStatusUpdates);

            var warnings                                           = all.Where        (result => result is not null).
                                                                         SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushGridOperatorAdminStatusResult(all[0].SenderId,
                                                                            ISendAdminStatus,
                                                                            result.Key,
                                                                            descriptions,
                                                                            rejectedGridOperatorAdminStatusUpdates,
                                                                            warnings,
                                                                            Runtime);

            return new PushGridOperatorAdminStatusResult(all[0].SenderId,
                                                                    ISendAdminStatus,
                                                                    PushGridOperatorAdminStatusResultTypes.Partial,
                                                                    descriptions,
                                                                    rejectedGridOperatorAdminStatusUpdates,
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


    public enum PushGridOperatorAdminStatusResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        Success,
        Partial,
        OutOfService,
        Error,
        True,
        NoOperation,
        Enqueued,
        LockTimeout,
        False

    }

}
