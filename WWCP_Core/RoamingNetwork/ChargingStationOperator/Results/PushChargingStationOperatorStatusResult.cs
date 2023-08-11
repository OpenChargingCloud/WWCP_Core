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

#endregion

namespace cloud.charging.open.protocols.WWCP
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
        public IId                                               AuthId                                           { get; }

        /// <summary>
        /// An object implementing ISendStatus.
        /// </summary>
        public IPushStatus?                                      ISendStatus                                      { get; }

        /// <summary>
        /// An object implementing IReceiveStatus.
        /// </summary>
        public IReceiveStatus?                                   IReceiveStatus                                   { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushChargingStationOperatorStatusResultTypes      Result                                           { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                           Description                                      { get; }

        /// <summary>
        /// An enumeration of rejected charging station operator status updates.
        /// </summary>
        public IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates     { get; }

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

        #region (private)  PushChargingStationOperatorStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushChargingStationOperatorStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationOperatorStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationOperatorStatusResult(IId                                                AuthId,
                                                        PushChargingStationOperatorStatusResultTypes       Result,
                                                        String?                                            Description                                    = null,
                                                        IEnumerable<ChargingStationOperatorStatusUpdate>?  RejectedChargingStationOperatorStatusUpdates   = null,
                                                        IEnumerable<Warning>?                              Warnings                                       = null,
                                                        TimeSpan?                                          Runtime                                        = null)
        {

            this.AuthId                                        = AuthId;
            this.Result                                        = Result;

            this.Description                                   = Description is not null && Description.IsNotNullOrEmpty()
                                                                     ? Description.Trim()
                                                                     : String.Empty;

            this.RejectedChargingStationOperatorStatusUpdates  = RejectedChargingStationOperatorStatusUpdates ?? Array.Empty<ChargingStationOperatorStatusUpdate>();

            this.Warnings                                      = Warnings is not null
                                                                     ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                                     : Array.Empty<Warning>();

            this.Runtime                                       = Runtime;

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
        /// <param name="RejectedChargingStationOperatorStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationOperatorStatusResult(IId                                                AuthId,
                                                         IPushStatus                                        ISendStatus,
                                                         PushChargingStationOperatorStatusResultTypes       Result,
                                                         String?                                            Description                                    = null,
                                                         IEnumerable<ChargingStationOperatorStatusUpdate>?  RejectedChargingStationOperatorStatusUpdates   = null,
                                                         IEnumerable<Warning>?                              Warnings                                       = null,
                                                         TimeSpan?                                          Runtime                                        = null)

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
        /// <param name="RejectedChargingStationOperatorStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationOperatorStatusResult(IId                                                AuthId,
                                                         IReceiveStatus                                     IReceiveStatus,
                                                         PushChargingStationOperatorStatusResultTypes       Result,
                                                         String?                                            Description                                    = null,
                                                         IEnumerable<ChargingStationOperatorStatusUpdate>?  RejectedChargingStationOperatorStatusUpdates   = null,
                                                         IEnumerable<Warning>?                              Warnings                                       = null,
                                                         TimeSpan?                                          Runtime                                        = null)

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

            Success(IId                    AuthId,
                    IPushStatus            ISendStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationOperatorStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationOperatorStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingStationOperatorStatusResult

            Success(IId                    AuthId,
                    IReceiveStatus         IReceiveStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingStationOperatorStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationOperatorStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushChargingStationOperatorStatusResult

            Enqueued(IId                    AuthId,
                     IPushStatus            ISendStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationOperatorStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<ChargingStationOperatorStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationOperatorStatusResult

            NoOperation(IId                    AuthId,
                        IPushStatus            ISendStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationOperatorStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationOperatorStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingStationOperatorStatusResult

            NoOperation(IId                    AuthId,
                        IReceiveStatus         IReceiveStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingStationOperatorStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationOperatorStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushChargingStationOperatorStatusResult

            OutOfService(IId                                               AuthId,
                         IPushStatus                                       ISendStatus,
                         IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                         String?                                           Description   = null,
                         IEnumerable<Warning>?                             Warnings      = null,
                         TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationOperatorStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationOperatorStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingStationOperatorStatusResult

            OutOfService(IId                                               AuthId,
                         IReceiveStatus                                    IReceiveStatus,
                         IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                         String?                                           Description   = null,
                         IEnumerable<Warning>?                             Warnings      = null,
                         TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingStationOperatorStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationOperatorStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushChargingStationOperatorStatusResult

            AdminDown(IId                                               AuthId,
                      IPushStatus                                       ISendStatus,
                      IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                      String?                                           Description   = null,
                      IEnumerable<Warning>?                             Warnings      = null,
                      TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationOperatorStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationOperatorStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingStationOperatorStatusResult

            AdminDown(IId                                               AuthId,
                      IReceiveStatus                                    IReceiveStatus,
                      IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                      String?                                           Description   = null,
                      IEnumerable<Warning>?                             Warnings      = null,
                      TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingStationOperatorStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationOperatorStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushChargingStationOperatorStatusResult

            Error(IId                                                AuthId,
                  IPushStatus                                        ISendStatus,
                  IEnumerable<ChargingStationOperatorStatusUpdate>?  RejectedChargingStationOperatorStatusUpdates   = null,
                  String?                                            Description                                    = null,
                  IEnumerable<Warning>?                              Warnings                                       = null,
                  TimeSpan?                                          Runtime                                        = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationOperatorStatusResultTypes.Error,
                    Description,
                    RejectedChargingStationOperatorStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingStationOperatorStatusResult

            Error(IId                                                AuthId,
                  IReceiveStatus                                     IReceiveStatus,
                  IEnumerable<ChargingStationOperatorStatusUpdate>?  RejectedChargingStationOperatorStatusUpdates   = null,
                  String?                                            Description                                    = null,
                  IEnumerable<Warning>?                              Warnings                                       = null,
                  TimeSpan?                                          Runtime                                        = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingStationOperatorStatusResultTypes.Error,
                    Description,
                    RejectedChargingStationOperatorStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingStationOperatorStatusResult

            LockTimeout(IId                                               AuthId,
                        IPushStatus                                       ISendStatus,
                        IEnumerable<ChargingStationOperatorStatusUpdate>  RejectedChargingStationOperatorStatusUpdates,
                        String?                                           Description   = null,
                        IEnumerable<Warning>?                             Warnings      = null,
                        TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationOperatorStatusResultTypes.LockTimeout,
                    Description,
                    RejectedChargingStationOperatorStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(AuthId, ISendStatus, PushChargingStationOperatorStatusResults, Runtime)

        public static PushChargingStationOperatorStatusResult Flatten(IId                                                   AuthId,
                                                                      IPushStatus                                           ISendStatus,
                                                                      IEnumerable<PushChargingStationOperatorStatusResult>  PushChargingStationOperatorStatusResults,
                                                                      TimeSpan                                              Runtime)
        {

            #region Initial checks

            if (PushChargingStationOperatorStatusResults is null || !PushChargingStationOperatorStatusResults.Any())
                return new PushChargingStationOperatorStatusResult(AuthId,
                                                                   ISendStatus,
                                                                   PushChargingStationOperatorStatusResultTypes.Error,
                                                                   "!",
                                                                   Array.Empty<ChargingStationOperatorStatusUpdate>(),
                                                                   Array.Empty<Warning>(),
                                                                   Runtime);

            #endregion

            var all                                           = PushChargingStationOperatorStatusResults.ToArray();

            var resultOverview                                = all.GroupBy      (result => result.Result).
                                                                    ToDictionary (result => result.Key,
                                                                                  result => new List<PushChargingStationOperatorStatusResult>(result));

            var descriptions                                  = all.Where        (result => result is not null).
                                                                    SafeSelect   (result => result.Description).
                                                                    AggregateWith(Environment.NewLine);

            var rejectedChargingStationOperatorStatusUpdates  = all.Where        (result => result is not null).
                                                                    SelectMany   (result => result.RejectedChargingStationOperatorStatusUpdates);

            var warnings                                      = all.Where        (result => result is not null).
                                                                    SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushChargingStationOperatorStatusResult(all[0].AuthId,
                                                                       ISendStatus,
                                                                       result.Key,
                                                                       descriptions,
                                                                       rejectedChargingStationOperatorStatusUpdates,
                                                                       warnings,
                                                                       Runtime);

            return new PushChargingStationOperatorStatusResult(all[0].AuthId,
                                                               ISendStatus,
                                                               PushChargingStationOperatorStatusResultTypes.Partial,
                                                               descriptions,
                                                               rejectedChargingStationOperatorStatusUpdates,
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
