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
    /// A PushEnergyMeterStatus result.
    /// </summary>
    public class PushEnergyMeterStatusResult
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
        public PushEnergyMeterStatusResultTypes      Result                                           { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                           Description                                      { get; }

        /// <summary>
        /// An enumeration of rejected charging station operator status updates.
        /// </summary>
        public IEnumerable<EnergyMeterStatusUpdate>  RejectedEnergyMeterStatusUpdates     { get; }

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

        #region (private)  PushEnergyMeterStatusResult(SenderId,                 Result, ...)

        /// <summary>
        /// Create a new PushEnergyMeterStatus result.
        /// </summary>
        /// <param name="SenderId">The unique identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEnergyMeterStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushEnergyMeterStatusResult(IId                                    SenderId,
                                            PushEnergyMeterStatusResultTypes       Result,
                                            String?                                Description                        = null,
                                            IEnumerable<EnergyMeterStatusUpdate>?  RejectedEnergyMeterStatusUpdates   = null,
                                            IEnumerable<Warning>?                  Warnings                           = null,
                                            TimeSpan?                              Runtime                            = null)
        {

            this.SenderId                          = SenderId;
            this.Result                            = Result;

            this.Description                       = Description is not null && Description.IsNotNullOrEmpty()
                                                         ? Description.Trim()
                                                         : String.Empty;

            this.RejectedEnergyMeterStatusUpdates  = RejectedEnergyMeterStatusUpdates ?? Array.Empty<EnergyMeterStatusUpdate>();

            this.Warnings                          = Warnings is not null
                                                         ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                         : Array.Empty<Warning>();

            this.Runtime                           = Runtime;

        }

        #endregion

        #region (internal) PushEnergyMeterStatusResult(SenderId, ISendStatus,    Result, ...)

        /// <summary>
        /// Create a new PushEnergyMeterStatus result.
        /// </summary>
        /// <param name="SenderId">The unique identification of the sender.</param>
        /// <param name="ISendStatus">An object implementing ISendStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEnergyMeterStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEnergyMeterStatusResult(IId                                    SenderId,
                                             ISendStatus                            ISendStatus,
                                             PushEnergyMeterStatusResultTypes       Result,
                                             String?                                Description                        = null,
                                             IEnumerable<EnergyMeterStatusUpdate>?  RejectedEnergyMeterStatusUpdates   = null,
                                             IEnumerable<Warning>?                  Warnings                           = null,
                                             TimeSpan?                              Runtime                            = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedEnergyMeterStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendStatus = ISendStatus;

        }

        #endregion

        #region (internal) PushEnergyMeterStatusResult(SenderId, IReceiveStatus, Result, ...)

        /// <summary>
        /// Create a new PushEnergyMeterStatus result.
        /// </summary>
        /// <param name="SenderId">The unique identification of the sender.</param>
        /// <param name="IReceiveStatus">An object implementing IReceiveStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEnergyMeterStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEnergyMeterStatusResult(IId                                    SenderId,
                                             IReceiveStatus                         IReceiveStatus,
                                             PushEnergyMeterStatusResultTypes       Result,
                                             String?                                Description                        = null,
                                             IEnumerable<EnergyMeterStatusUpdate>?  RejectedEnergyMeterStatusUpdates   = null,
                                             IEnumerable<Warning>?                  Warnings                           = null,
                                             TimeSpan?                              Runtime                            = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedEnergyMeterStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveStatus = IReceiveStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushEnergyMeterStatusResult

            Success(IId                    SenderId,
                    ISendStatus            ISendStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushEnergyMeterStatusResultTypes.Success,
                    Description,
                    Array.Empty<EnergyMeterStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushEnergyMeterStatusResult

            Success(IId                    SenderId,
                    IReceiveStatus         IReceiveStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushEnergyMeterStatusResultTypes.Success,
                    Description,
                    Array.Empty<EnergyMeterStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushEnergyMeterStatusResult

            Enqueued(IId                    SenderId,
                     ISendStatus            ISendStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushEnergyMeterStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<EnergyMeterStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushEnergyMeterStatusResult

            NoOperation(IId                    SenderId,
                        ISendStatus            ISendStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushEnergyMeterStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EnergyMeterStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushEnergyMeterStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveStatus         IReceiveStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushEnergyMeterStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EnergyMeterStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushEnergyMeterStatusResult

            OutOfService(IId                                               SenderId,
                         ISendStatus                                       ISendStatus,
                         IEnumerable<EnergyMeterStatusUpdate>  RejectedEnergyMeterStatusUpdates,
                         String?                                           Description   = null,
                         IEnumerable<Warning>?                             Warnings      = null,
                         TimeSpan?                                         Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushEnergyMeterStatusResultTypes.OutOfService,
                    Description,
                    RejectedEnergyMeterStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushEnergyMeterStatusResult

            OutOfService(IId                                               SenderId,
                         IReceiveStatus                                    IReceiveStatus,
                         IEnumerable<EnergyMeterStatusUpdate>  RejectedEnergyMeterStatusUpdates,
                         String?                                           Description   = null,
                         IEnumerable<Warning>?                             Warnings      = null,
                         TimeSpan?                                         Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushEnergyMeterStatusResultTypes.OutOfService,
                    Description,
                    RejectedEnergyMeterStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushEnergyMeterStatusResult

            AdminDown(IId                                               SenderId,
                      ISendStatus                                       ISendStatus,
                      IEnumerable<EnergyMeterStatusUpdate>  RejectedEnergyMeterStatusUpdates,
                      String?                                           Description   = null,
                      IEnumerable<Warning>?                             Warnings      = null,
                      TimeSpan?                                         Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushEnergyMeterStatusResultTypes.AdminDown,
                    Description,
                    RejectedEnergyMeterStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushEnergyMeterStatusResult

            AdminDown(IId                                               SenderId,
                      IReceiveStatus                                    IReceiveStatus,
                      IEnumerable<EnergyMeterStatusUpdate>  RejectedEnergyMeterStatusUpdates,
                      String?                                           Description   = null,
                      IEnumerable<Warning>?                             Warnings      = null,
                      TimeSpan?                                         Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushEnergyMeterStatusResultTypes.AdminDown,
                    Description,
                    RejectedEnergyMeterStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushEnergyMeterStatusResult

            Error(IId                                                SenderId,
                  ISendStatus                                        ISendStatus,
                  IEnumerable<EnergyMeterStatusUpdate>?  RejectedEnergyMeterStatusUpdates   = null,
                  String?                                            Description                                    = null,
                  IEnumerable<Warning>?                              Warnings                                       = null,
                  TimeSpan?                                          Runtime                                        = null)

            => new (SenderId,
                    ISendStatus,
                    PushEnergyMeterStatusResultTypes.Error,
                    Description,
                    RejectedEnergyMeterStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushEnergyMeterStatusResult

            Error(IId                                                SenderId,
                  IReceiveStatus                                     IReceiveStatus,
                  IEnumerable<EnergyMeterStatusUpdate>?  RejectedEnergyMeterStatusUpdates   = null,
                  String?                                            Description                                    = null,
                  IEnumerable<Warning>?                              Warnings                                       = null,
                  TimeSpan?                                          Runtime                                        = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushEnergyMeterStatusResultTypes.Error,
                    Description,
                    RejectedEnergyMeterStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushEnergyMeterStatusResult

            LockTimeout(IId                                               SenderId,
                        ISendStatus                                       ISendStatus,
                        IEnumerable<EnergyMeterStatusUpdate>  RejectedEnergyMeterStatusUpdates,
                        String?                                           Description   = null,
                        IEnumerable<Warning>?                             Warnings      = null,
                        TimeSpan?                                         Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushEnergyMeterStatusResultTypes.LockTimeout,
                    Description,
                    RejectedEnergyMeterStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(SenderId, ISendStatus, PushEnergyMeterStatusResults, Runtime)

        public static PushEnergyMeterStatusResult Flatten(IId                                       SenderId,
                                                          ISendStatus                               ISendStatus,
                                                          IEnumerable<PushEnergyMeterStatusResult>  PushEnergyMeterStatusResults,
                                                          TimeSpan                                  Runtime)
        {

            #region Initial checks

            if (PushEnergyMeterStatusResults is null || !PushEnergyMeterStatusResults.Any())
                return new PushEnergyMeterStatusResult(SenderId,
                                                                   ISendStatus,
                                                                   PushEnergyMeterStatusResultTypes.Error,
                                                                   "!",
                                                                   Array.Empty<EnergyMeterStatusUpdate>(),
                                                                   Array.Empty<Warning>(),
                                                                   Runtime);

            #endregion

            var all                                           = PushEnergyMeterStatusResults.ToArray();

            var resultOverview                                = all.GroupBy      (result => result.Result).
                                                                    ToDictionary (result => result.Key,
                                                                                  result => new List<PushEnergyMeterStatusResult>(result));

            var descriptions                                  = all.Where        (result => result is not null).
                                                                    SafeSelect   (result => result.Description).
                                                                    AggregateWith(Environment.NewLine);

            var rejectedEnergyMeterStatusUpdates  = all.Where        (result => result is not null).
                                                                    SelectMany   (result => result.RejectedEnergyMeterStatusUpdates);

            var warnings                                      = all.Where        (result => result is not null).
                                                                    SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushEnergyMeterStatusResult(all[0].SenderId,
                                                                       ISendStatus,
                                                                       result.Key,
                                                                       descriptions,
                                                                       rejectedEnergyMeterStatusUpdates,
                                                                       warnings,
                                                                       Runtime);

            return new PushEnergyMeterStatusResult(all[0].SenderId,
                                                               ISendStatus,
                                                               PushEnergyMeterStatusResultTypes.Partial,
                                                               descriptions,
                                                               rejectedEnergyMeterStatusUpdates,
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


    public enum PushEnergyMeterStatusResultTypes
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
