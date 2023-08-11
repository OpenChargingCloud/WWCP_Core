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
    /// A PushChargingStationStatus result.
    /// </summary>
    public class PushChargingStationStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                       AuthId                                   { get; }

        /// <summary>
        /// An object implementing ISendStatus.
        /// </summary>
        public ISendStatus?                              ISendStatus                              { get; }

        /// <summary>
        /// An object implementing IReceiveStatus.
        /// </summary>
        public IReceiveStatus?                           IReceiveStatus                           { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushChargingStationStatusResultTypes      Result                                   { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                   Description                              { get; }

        /// <summary>
        /// An enumeration of rejected charging station status updates.
        /// </summary>
        public IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                      Warnings                                 { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                                 Runtime                                  { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushChargingStationStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushChargingStationStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationStatusUpdates">An enumeration of rejected charging station status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationStatusResult(IId                                        AuthId,
                                                PushChargingStationStatusResultTypes       Result,
                                                String?                                    Description                            = null,
                                                IEnumerable<ChargingStationStatusUpdate>?  RejectedChargingStationStatusUpdates   = null,
                                                IEnumerable<Warning>?                      Warnings                               = null,
                                                TimeSpan?                                  Runtime                                = null)
        {

            this.AuthId                                = AuthId;
            this.Result                                = Result;

            this.Description                           = Description is not null && Description.IsNotNullOrEmpty()
                                                             ? Description.Trim()
                                                             : String.Empty;

            this.RejectedChargingStationStatusUpdates  = RejectedChargingStationStatusUpdates ?? Array.Empty<ChargingStationStatusUpdate>();

            this.Warnings                              = Warnings is not null
                                                             ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                             : Array.Empty<Warning>();

            this.Runtime                               = Runtime;

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
        /// <param name="RejectedChargingStationStatusUpdates">An enumeration of rejected charging station status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationStatusResult(IId                                        AuthId,
                                                 ISendStatus                                ISendStatus,
                                                 PushChargingStationStatusResultTypes       Result,
                                                 String?                                    Description                            = null,
                                                 IEnumerable<ChargingStationStatusUpdate>?  RejectedChargingStationStatusUpdates   = null,
                                                 IEnumerable<Warning>?                      Warnings                               = null,
                                                 TimeSpan?                                  Runtime                                = null)

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
        /// <param name="RejectedChargingStationStatusUpdates">An enumeration of rejected charging station status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationStatusResult(IId                                        AuthId,
                                                 IReceiveStatus                             IReceiveStatus,
                                                 PushChargingStationStatusResultTypes       Result,
                                                 String?                                    Description                            = null,
                                                 IEnumerable<ChargingStationStatusUpdate>?  RejectedChargingStationStatusUpdates   = null,
                                                 IEnumerable<Warning>?                      Warnings                               = null,
                                                 TimeSpan?                                  Runtime                                = null)

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

            Success(IId                    AuthId,
                    ISendStatus            ISendStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingStationStatusResult

            Success(IId                    AuthId,
                    IReceiveStatus         IReceiveStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingStationStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushChargingStationStatusResult

            Enqueued(IId                    AuthId,
                     ISendStatus            ISendStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<ChargingStationStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationStatusResult

            NoOperation(IId                    AuthId,
                        ISendStatus            ISendStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingStationStatusResult

            NoOperation(IId                    AuthId,
                        IReceiveStatus         IReceiveStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingStationStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushChargingStationStatusResult

            OutOfService(IId                                       AuthId,
                         ISendStatus                               ISendStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                         String?                                   Description   = null,
                         IEnumerable<Warning>?                     Warnings      = null,
                         TimeSpan?                                 Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingStationStatusResult

            OutOfService(IId                                       AuthId,
                         IReceiveStatus                            IReceiveStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                         String?                                   Description   = null,
                         IEnumerable<Warning>?                     Warnings      = null,
                         TimeSpan?                                 Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingStationStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushChargingStationStatusResult

            AdminDown(IId                                       AuthId,
                      ISendStatus                               ISendStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                      String?                                   Description   = null,
                      IEnumerable<Warning>?                     Warnings      = null,
                      TimeSpan?                                 Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingStationStatusResult

            AdminDown(IId                                       AuthId,
                      IReceiveStatus                            IReceiveStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                      String?                                   Description   = null,
                      IEnumerable<Warning>?                     Warnings      = null,
                      TimeSpan?                                 Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingStationStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushChargingStationStatusResult

            Error(IId                                        AuthId,
                  ISendStatus                                ISendStatus,
                  IEnumerable<ChargingStationStatusUpdate>?  RejectedChargingStationStatusUpdates   = null,
                  String?                                    Description                            = null,
                  IEnumerable<Warning>?                      Warnings                               = null,
                  TimeSpan?                                  Runtime                                = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.Error,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingStationStatusResult

            Error(IId                                        AuthId,
                  IReceiveStatus                             IReceiveStatus,
                  IEnumerable<ChargingStationStatusUpdate>?  RejectedChargingStationStatusUpdates   = null,
                  String?                                    Description                            = null,
                  IEnumerable<Warning>?                      Warnings                               = null,
                  TimeSpan?                                  Runtime                                = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingStationStatusResultTypes.Error,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingStationStatusResult

            LockTimeout(IId                                       AuthId,
                        ISendStatus                               ISendStatus,
                        IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                        String?                                   Description   = null,
                        IEnumerable<Warning>?                     Warnings      = null,
                        TimeSpan?                                 Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.LockTimeout,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(AuthId, ISendStatus, PushChargingStationStatusResults, Runtime)

        public static PushChargingStationStatusResult Flatten(IId                                           AuthId,
                                                              ISendStatus                                   ISendStatus,
                                                              IEnumerable<PushChargingStationStatusResult>  PushChargingStationStatusResults,
                                                              TimeSpan                                      Runtime)
        {

            #region Initial checks

            if (PushChargingStationStatusResults is null || !PushChargingStationStatusResults.Any())
                return new PushChargingStationStatusResult(AuthId,
                                                           ISendStatus,
                                                           PushChargingStationStatusResultTypes.Error,
                                                           "!",
                                                           Array.Empty<ChargingStationStatusUpdate>(),
                                                           Array.Empty<Warning>(),
                                                           Runtime);

            #endregion

            var all                                   = PushChargingStationStatusResults.ToArray();

            var resultOverview                        = all.GroupBy      (result => result.Result).
                                                            ToDictionary (result => result.Key,
                                                                          result => new List<PushChargingStationStatusResult>(result));

            var descriptions                          = all.Where        (result => result is not null).
                                                            SafeSelect   (result => result.Description).
                                                            AggregateWith(Environment.NewLine);

            var rejectedChargingStationStatusUpdates  = all.Where        (result => result is not null).
                                                            SelectMany   (result => result.RejectedChargingStationStatusUpdates);

            var warnings                              = all.Where        (result => result is not null).
                                                            SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushChargingStationStatusResult(all[0].AuthId,
                                                               ISendStatus,
                                                               result.Key,
                                                               descriptions,
                                                               rejectedChargingStationStatusUpdates,
                                                               warnings,
                                                               Runtime);

            return new PushChargingStationStatusResult(all[0].AuthId,
                                                       ISendStatus,
                                                       PushChargingStationStatusResultTypes.Partial,
                                                       descriptions,
                                                       rejectedChargingStationStatusUpdates,
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
