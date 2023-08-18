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
    /// A PushEnergyMeterAdminStatus result.
    /// </summary>
    public class PushEnergyMeterAdminStatusResult
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
        public PushEnergyMeterAdminStatusResultTypes      Result                                                { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                                Description                                           { get; }

        /// <summary>
        /// An enumeration of rejected charging station operator admin status updates.
        /// </summary>
        public IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates     { get; }

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

        #region (private)  PushEnergyMeterAdminStatusResult(SenderId,                      Result, ...)

        /// <summary>
        /// Create a new PushEnergyMeterAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEnergyMeterAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushEnergyMeterAdminStatusResult(IId                                         SenderId,
                                                 PushEnergyMeterAdminStatusResultTypes       Result,
                                                 String?                                     Description                             = null,
                                                 IEnumerable<EnergyMeterAdminStatusUpdate>?  RejectedEnergyMeterAdminStatusUpdates   = null,
                                                 IEnumerable<Warning>?                       Warnings                                = null,
                                                 TimeSpan?                                   Runtime                                 = null)
        {

            this.SenderId                                           = SenderId;
            this.Result                                             = Result;

            this.Description                                        = Description is not null && Description.IsNotNullOrEmpty()
                                                                          ? Description.Trim()
                                                                          : String.Empty;

            this.RejectedEnergyMeterAdminStatusUpdates  = RejectedEnergyMeterAdminStatusUpdates ?? Array.Empty<EnergyMeterAdminStatusUpdate>();

            this.Warnings                                           = Warnings is not null
                                                                          ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                                          : Array.Empty<Warning>();

            this.Runtime                                            = Runtime;

        }

        #endregion

        #region (internal) PushEnergyMeterAdminStatusResult(SenderId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushEnergyMeterAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEnergyMeterAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEnergyMeterAdminStatusResult(IId                                         SenderId,
                                                  ISendAdminStatus                            ISendAdminStatus,
                                                  PushEnergyMeterAdminStatusResultTypes       Result,
                                                  String?                                     Description                             = null,
                                                  IEnumerable<EnergyMeterAdminStatusUpdate>?  RejectedEnergyMeterAdminStatusUpdates   = null,
                                                  IEnumerable<Warning>?                       Warnings                                = null,
                                                  TimeSpan?                                   Runtime                                 = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedEnergyMeterAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushEnergyMeterAdminStatusResult(SenderId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushEnergyMeterAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEnergyMeterAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEnergyMeterAdminStatusResult(IId                                         SenderId,
                                                  IReceiveAdminStatus                         IReceiveAdminStatus,
                                                  PushEnergyMeterAdminStatusResultTypes       Result,
                                                  String?                                     Description                             = null,
                                                  IEnumerable<EnergyMeterAdminStatusUpdate>?  RejectedEnergyMeterAdminStatusUpdates   = null,
                                                  IEnumerable<Warning>?                       Warnings                                = null,
                                                  TimeSpan?                                   Runtime                                 = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedEnergyMeterAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushEnergyMeterAdminStatusResult

            Success(IId                    SenderId,
                    ISendAdminStatus       ISendAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<EnergyMeterAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushEnergyMeterAdminStatusResult

            Success(IId                    SenderId,
                    IReceiveAdminStatus    IReceiveAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<EnergyMeterAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushEnergyMeterAdminStatusResult

            Enqueued(IId                    SenderId,
                     ISendAdminStatus       ISendAdminStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<EnergyMeterAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushEnergyMeterAdminStatusResult

            NoOperation(IId                    SenderId,
                        ISendAdminStatus       ISendAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EnergyMeterAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushEnergyMeterAdminStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveAdminStatus    IReceiveAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EnergyMeterAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushEnergyMeterAdminStatusResult

            OutOfService(IId                                                    SenderId,
                         ISendAdminStatus                                       ISendAdminStatus,
                         IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushEnergyMeterAdminStatusResult

            OutOfService(IId                                                    SenderId,
                         IReceiveAdminStatus                                    IReceiveAdminStatus,
                         IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushEnergyMeterAdminStatusResult

            AdminDown(IId                                                    SenderId,
                      ISendAdminStatus                                       ISendAdminStatus,
                      IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushEnergyMeterAdminStatusResult

            AdminDown(IId                                                    SenderId,
                      IReceiveAdminStatus                                    IReceiveAdminStatus,
                      IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushEnergyMeterAdminStatusResult

            Error(IId                                                     SenderId,
                  ISendAdminStatus                                        ISendAdminStatus,
                  IEnumerable<EnergyMeterAdminStatusUpdate>?  RejectedEnergyMeterAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.Error,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushEnergyMeterAdminStatusResult

            Error(IId                                                     SenderId,
                  IReceiveAdminStatus                                     IReceiveAdminStatus,
                  IEnumerable<EnergyMeterAdminStatusUpdate>?  RejectedEnergyMeterAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.Error,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushEnergyMeterAdminStatusResult

            LockTimeout(IId                                                    SenderId,
                        ISendAdminStatus                                       ISendAdminStatus,
                        IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates,
                        String?                                                Description   = null,
                        IEnumerable<Warning>?                                  Warnings      = null,
                        TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.LockTimeout,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(SenderId, ISendAdminStatus, PushEnergyMeterAdminStatusResults, Runtime)

        public static PushEnergyMeterAdminStatusResult Flatten(IId                                            SenderId,
                                                               ISendAdminStatus                               ISendAdminStatus,
                                                               IEnumerable<PushEnergyMeterAdminStatusResult>  PushEnergyMeterAdminStatusResults,
                                                               TimeSpan                                       Runtime)
        {

            #region Initial checks

            if (PushEnergyMeterAdminStatusResults is null || !PushEnergyMeterAdminStatusResults.Any())
                return new PushEnergyMeterAdminStatusResult(SenderId,
                                                                        ISendAdminStatus,
                                                                        PushEnergyMeterAdminStatusResultTypes.Error,
                                                                        "!",
                                                                        Array.Empty<EnergyMeterAdminStatusUpdate>(),
                                                                        Array.Empty<Warning>(),
                                                                        Runtime);

            #endregion

            var all                                                = PushEnergyMeterAdminStatusResults.ToArray();

            var resultOverview                                     = all.GroupBy      (result => result.Result).
                                                                         ToDictionary (result => result.Key,
                                                                                       result => new List<PushEnergyMeterAdminStatusResult>(result));

            var descriptions                                       = all.Where        (result => result is not null).
                                                                         SafeSelect   (result => result.Description).
                                                                         AggregateWith(Environment.NewLine);

            var rejectedEnergyMeterAdminStatusUpdates  = all.Where        (result => result is not null).
                                                                         SelectMany   (result => result.RejectedEnergyMeterAdminStatusUpdates);

            var warnings                                           = all.Where        (result => result is not null).
                                                                         SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushEnergyMeterAdminStatusResult(all[0].SenderId,
                                                                            ISendAdminStatus,
                                                                            result.Key,
                                                                            descriptions,
                                                                            rejectedEnergyMeterAdminStatusUpdates,
                                                                            warnings,
                                                                            Runtime);

            return new PushEnergyMeterAdminStatusResult(all[0].SenderId,
                                                                    ISendAdminStatus,
                                                                    PushEnergyMeterAdminStatusResultTypes.Partial,
                                                                    descriptions,
                                                                    rejectedEnergyMeterAdminStatusUpdates,
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


    public enum PushEnergyMeterAdminStatusResultTypes
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
