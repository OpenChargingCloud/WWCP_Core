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
    /// A PushChargingStationStatus result.
    /// </summary>
    public class PushChargingStationStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                       SenderId                                   { get; }

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

        #region (private)  PushChargingStationStatusResult(SenderId,                 Result, ...)

        /// <summary>
        /// Create a new PushChargingStationStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationStatusUpdates">An enumeration of rejected charging station status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationStatusResult(IId                                        SenderId,
                                                PushChargingStationStatusResultTypes       Result,
                                                String?                                    Description                            = null,
                                                IEnumerable<ChargingStationStatusUpdate>?  RejectedChargingStationStatusUpdates   = null,
                                                IEnumerable<Warning>?                      Warnings                               = null,
                                                TimeSpan?                                  Runtime                                = null)
        {

            this.SenderId                              = SenderId;
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

        #region (internal) PushChargingStationStatusResult(SenderId, ISendStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingStationStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="ISendStatus">An object implementing ISendStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationStatusUpdates">An enumeration of rejected charging station status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationStatusResult(IId                                        SenderId,
                                                 ISendStatus                                ISendStatus,
                                                 PushChargingStationStatusResultTypes       Result,
                                                 String?                                    Description                            = null,
                                                 IEnumerable<ChargingStationStatusUpdate>?  RejectedChargingStationStatusUpdates   = null,
                                                 IEnumerable<Warning>?                      Warnings                               = null,
                                                 TimeSpan?                                  Runtime                                = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedChargingStationStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendStatus = ISendStatus;

        }

        #endregion

        #region (internal) PushChargingStationStatusResult(SenderId, IReceiveStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingStationStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="IReceiveStatus">An object implementing IReceiveStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationStatusUpdates">An enumeration of rejected charging station status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationStatusResult(IId                                        SenderId,
                                                 IReceiveStatus                             IReceiveStatus,
                                                 PushChargingStationStatusResultTypes       Result,
                                                 String?                                    Description                            = null,
                                                 IEnumerable<ChargingStationStatusUpdate>?  RejectedChargingStationStatusUpdates   = null,
                                                 IEnumerable<Warning>?                      Warnings                               = null,
                                                 TimeSpan?                                  Runtime                                = null)

            : this(SenderId,
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

            Success(IId                    SenderId,
                    ISendStatus            ISendStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingStationStatusResult

            Success(IId                    SenderId,
                    IReceiveStatus         IReceiveStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushChargingStationStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingStationStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushChargingStationStatusResult

            Enqueued(IId                    SenderId,
                     ISendStatus            ISendStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<ChargingStationStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationStatusResult

            NoOperation(IId                    SenderId,
                        ISendStatus            ISendStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingStationStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveStatus         IReceiveStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushChargingStationStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingStationStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushChargingStationStatusResult

            OutOfService(IId                                       SenderId,
                         ISendStatus                               ISendStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                         String?                                   Description   = null,
                         IEnumerable<Warning>?                     Warnings      = null,
                         TimeSpan?                                 Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingStationStatusResult

            OutOfService(IId                                       SenderId,
                         IReceiveStatus                            IReceiveStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                         String?                                   Description   = null,
                         IEnumerable<Warning>?                     Warnings      = null,
                         TimeSpan?                                 Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushChargingStationStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushChargingStationStatusResult

            AdminDown(IId                                       SenderId,
                      ISendStatus                               ISendStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                      String?                                   Description   = null,
                      IEnumerable<Warning>?                     Warnings      = null,
                      TimeSpan?                                 Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingStationStatusResult

            AdminDown(IId                                       SenderId,
                      IReceiveStatus                            IReceiveStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                      String?                                   Description   = null,
                      IEnumerable<Warning>?                     Warnings      = null,
                      TimeSpan?                                 Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushChargingStationStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushChargingStationStatusResult

            Error(IId                                        SenderId,
                  ISendStatus                                ISendStatus,
                  IEnumerable<ChargingStationStatusUpdate>?  RejectedChargingStationStatusUpdates   = null,
                  String?                                    Description                            = null,
                  IEnumerable<Warning>?                      Warnings                               = null,
                  TimeSpan?                                  Runtime                                = null)

            => new (SenderId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.Error,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingStationStatusResult

            Error(IId                                        SenderId,
                  IReceiveStatus                             IReceiveStatus,
                  IEnumerable<ChargingStationStatusUpdate>?  RejectedChargingStationStatusUpdates   = null,
                  String?                                    Description                            = null,
                  IEnumerable<Warning>?                      Warnings                               = null,
                  TimeSpan?                                  Runtime                                = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushChargingStationStatusResultTypes.Error,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingStationStatusResult

            LockTimeout(IId                                       SenderId,
                        ISendStatus                               ISendStatus,
                        IEnumerable<ChargingStationStatusUpdate>  RejectedChargingStationStatusUpdates,
                        String?                                   Description   = null,
                        IEnumerable<Warning>?                     Warnings      = null,
                        TimeSpan?                                 Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushChargingStationStatusResultTypes.LockTimeout,
                    Description,
                    RejectedChargingStationStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(SenderId, ISendStatus, PushChargingStationStatusResults, Runtime)

        public static PushChargingStationStatusResult Flatten(IId                                           SenderId,
                                                              ISendStatus                                   ISendStatus,
                                                              IEnumerable<PushChargingStationStatusResult>  PushChargingStationStatusResults,
                                                              TimeSpan                                      Runtime)
        {

            #region Initial checks

            if (PushChargingStationStatusResults is null || !PushChargingStationStatusResults.Any())
                return new PushChargingStationStatusResult(SenderId,
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
                    return new PushChargingStationStatusResult(all[0].SenderId,
                                                               ISendStatus,
                                                               result.Key,
                                                               descriptions,
                                                               rejectedChargingStationStatusUpdates,
                                                               warnings,
                                                               Runtime);

            return new PushChargingStationStatusResult(all[0].SenderId,
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
