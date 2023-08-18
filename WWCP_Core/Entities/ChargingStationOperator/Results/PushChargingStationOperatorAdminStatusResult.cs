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
    /// A PushChargingStationOperatorAdminStatus result.
    /// </summary>
    public class PushChargingStationOperatorAdminStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                                    AuthId                                                { get; }

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
        public PushChargingStationOperatorAdminStatusResultTypes      Result                                                { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                                Description                                           { get; }

        /// <summary>
        /// An enumeration of rejected charging station operator admin status updates.
        /// </summary>
        public IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates     { get; }

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

        #region (private)  PushChargingStationOperatorAdminStatusResult(SenderId,                      Result, ...)

        /// <summary>
        /// Create a new PushChargingStationOperatorAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationOperatorAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationOperatorAdminStatusResult(IId                                                     SenderId,
                                                             PushChargingStationOperatorAdminStatusResultTypes       Result,
                                                             String?                                                 Description                                         = null,
                                                             IEnumerable<ChargingStationOperatorAdminStatusUpdate>?  RejectedChargingStationOperatorAdminStatusUpdates   = null,
                                                             IEnumerable<Warning>?                                   Warnings                                            = null,
                                                             TimeSpan?                                               Runtime                                             = null)
        {

            this.AuthId                                             = AuthId;
            this.Result                                             = Result;

            this.Description                                        = Description is not null && Description.IsNotNullOrEmpty()
                                                                          ? Description.Trim()
                                                                          : String.Empty;

            this.RejectedChargingStationOperatorAdminStatusUpdates  = RejectedChargingStationOperatorAdminStatusUpdates ?? Array.Empty<ChargingStationOperatorAdminStatusUpdate>();

            this.Warnings                                           = Warnings is not null
                                                                          ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                                          : Array.Empty<Warning>();

            this.Runtime                                            = Runtime;

        }

        #endregion

        #region (internal) PushChargingStationOperatorAdminStatusResult(SenderId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingStationOperatorAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationOperatorAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationOperatorAdminStatusResult(IId                                                     SenderId,
                                                              ISendAdminStatus                                        ISendAdminStatus,
                                                              PushChargingStationOperatorAdminStatusResultTypes       Result,
                                                              String?                                                 Description                                         = null,
                                                              IEnumerable<ChargingStationOperatorAdminStatusUpdate>?  RejectedChargingStationOperatorAdminStatusUpdates   = null,
                                                              IEnumerable<Warning>?                                   Warnings                                            = null,
                                                              TimeSpan?                                               Runtime                                             = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedChargingStationOperatorAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushChargingStationOperatorAdminStatusResult(SenderId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingStationOperatorAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationOperatorAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationOperatorAdminStatusResult(IId                                                     SenderId,
                                                              IReceiveAdminStatus                                     IReceiveAdminStatus,
                                                              PushChargingStationOperatorAdminStatusResultTypes       Result,
                                                              String?                                                 Description                                         = null,
                                                              IEnumerable<ChargingStationOperatorAdminStatusUpdate>?  RejectedChargingStationOperatorAdminStatusUpdates   = null,
                                                              IEnumerable<Warning>?                                   Warnings                                            = null,
                                                              TimeSpan?                                               Runtime                                             = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedChargingStationOperatorAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingStationOperatorAdminStatusResult

            Success(IId                    SenderId,
                    ISendAdminStatus       ISendAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationOperatorAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            Success(IId                    SenderId,
                    IReceiveAdminStatus    IReceiveAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationOperatorAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushChargingStationOperatorAdminStatusResult

            Enqueued(IId                    SenderId,
                     ISendAdminStatus       ISendAdminStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<ChargingStationOperatorAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationOperatorAdminStatusResult

            NoOperation(IId                    SenderId,
                        ISendAdminStatus       ISendAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationOperatorAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveAdminStatus    IReceiveAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationOperatorAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushChargingStationOperatorAdminStatusResult

            OutOfService(IId                                                    SenderId,
                         ISendAdminStatus                                       ISendAdminStatus,
                         IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            OutOfService(IId                                                    SenderId,
                         IReceiveAdminStatus                                    IReceiveAdminStatus,
                         IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushChargingStationOperatorAdminStatusResult

            AdminDown(IId                                                    SenderId,
                      ISendAdminStatus                                       ISendAdminStatus,
                      IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            AdminDown(IId                                                    SenderId,
                      IReceiveAdminStatus                                    IReceiveAdminStatus,
                      IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushChargingStationOperatorAdminStatusResult

            Error(IId                                                     SenderId,
                  ISendAdminStatus                                        ISendAdminStatus,
                  IEnumerable<ChargingStationOperatorAdminStatusUpdate>?  RejectedChargingStationOperatorAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.Error,
                    Description,
                    RejectedChargingStationOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingStationOperatorAdminStatusResult

            Error(IId                                                     SenderId,
                  IReceiveAdminStatus                                     IReceiveAdminStatus,
                  IEnumerable<ChargingStationOperatorAdminStatusUpdate>?  RejectedChargingStationOperatorAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.Error,
                    Description,
                    RejectedChargingStationOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingStationOperatorAdminStatusResult

            LockTimeout(IId                                                    SenderId,
                        ISendAdminStatus                                       ISendAdminStatus,
                        IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                        String?                                                Description   = null,
                        IEnumerable<Warning>?                                  Warnings      = null,
                        TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushChargingStationOperatorAdminStatusResultTypes.LockTimeout,
                    Description,
                    RejectedChargingStationOperatorAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(SenderId, ISendAdminStatus, PushChargingStationOperatorAdminStatusResults, Runtime)

        public static PushChargingStationOperatorAdminStatusResult Flatten(IId                                                        SenderId,
                                                                           ISendAdminStatus                                           ISendAdminStatus,
                                                                           IEnumerable<PushChargingStationOperatorAdminStatusResult>  PushChargingStationOperatorAdminStatusResults,
                                                                           TimeSpan                                                   Runtime)
        {

            #region Initial checks

            if (PushChargingStationOperatorAdminStatusResults is null || !PushChargingStationOperatorAdminStatusResults.Any())
                return new PushChargingStationOperatorAdminStatusResult(SenderId,
                                                                        ISendAdminStatus,
                                                                        PushChargingStationOperatorAdminStatusResultTypes.Error,
                                                                        "!",
                                                                        Array.Empty<ChargingStationOperatorAdminStatusUpdate>(),
                                                                        Array.Empty<Warning>(),
                                                                        Runtime);

            #endregion

            var all                                                = PushChargingStationOperatorAdminStatusResults.ToArray();

            var resultOverview                                     = all.GroupBy      (result => result.Result).
                                                                         ToDictionary (result => result.Key,
                                                                                       result => new List<PushChargingStationOperatorAdminStatusResult>(result));

            var descriptions                                       = all.Where        (result => result is not null).
                                                                         SafeSelect   (result => result.Description).
                                                                         AggregateWith(Environment.NewLine);

            var rejectedChargingStationOperatorAdminStatusUpdates  = all.Where        (result => result is not null).
                                                                         SelectMany   (result => result.RejectedChargingStationOperatorAdminStatusUpdates);

            var warnings                                           = all.Where        (result => result is not null).
                                                                         SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushChargingStationOperatorAdminStatusResult(all[0].SenderId,
                                                                            ISendAdminStatus,
                                                                            result.Key,
                                                                            descriptions,
                                                                            rejectedChargingStationOperatorAdminStatusUpdates,
                                                                            warnings,
                                                                            Runtime);

            return new PushChargingStationOperatorAdminStatusResult(all[0].SenderId,
                                                                    ISendAdminStatus,
                                                                    PushChargingStationOperatorAdminStatusResultTypes.Partial,
                                                                    descriptions,
                                                                    rejectedChargingStationOperatorAdminStatusUpdates,
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


    public enum PushChargingStationOperatorAdminStatusResultTypes
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
