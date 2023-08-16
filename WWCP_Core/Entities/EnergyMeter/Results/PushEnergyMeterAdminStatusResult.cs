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

        #region (private)  PushEnergyMeterAdminStatusResult(AuthId,                      Result, ...)

        /// <summary>
        /// Create a new PushEnergyMeterAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEnergyMeterAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushEnergyMeterAdminStatusResult(IId                                                     AuthId,
                                                             PushEnergyMeterAdminStatusResultTypes       Result,
                                                             String?                                                 Description                                         = null,
                                                             IEnumerable<EnergyMeterAdminStatusUpdate>?  RejectedEnergyMeterAdminStatusUpdates   = null,
                                                             IEnumerable<Warning>?                                   Warnings                                            = null,
                                                             TimeSpan?                                               Runtime                                             = null)
        {

            this.AuthId                                             = AuthId;
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

        #region (internal) PushEnergyMeterAdminStatusResult(AuthId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushEnergyMeterAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEnergyMeterAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEnergyMeterAdminStatusResult(IId                                                     AuthId,
                                                              ISendAdminStatus                                        ISendAdminStatus,
                                                              PushEnergyMeterAdminStatusResultTypes       Result,
                                                              String?                                                 Description                                         = null,
                                                              IEnumerable<EnergyMeterAdminStatusUpdate>?  RejectedEnergyMeterAdminStatusUpdates   = null,
                                                              IEnumerable<Warning>?                                   Warnings                                            = null,
                                                              TimeSpan?                                               Runtime                                             = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedEnergyMeterAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushEnergyMeterAdminStatusResult(AuthId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushEnergyMeterAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEnergyMeterAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEnergyMeterAdminStatusResult(IId                                                     AuthId,
                                                              IReceiveAdminStatus                                     IReceiveAdminStatus,
                                                              PushEnergyMeterAdminStatusResultTypes       Result,
                                                              String?                                                 Description                                         = null,
                                                              IEnumerable<EnergyMeterAdminStatusUpdate>?  RejectedEnergyMeterAdminStatusUpdates   = null,
                                                              IEnumerable<Warning>?                                   Warnings                                            = null,
                                                              TimeSpan?                                               Runtime                                             = null)

            : this(AuthId,
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

            Success(IId                    AuthId,
                    ISendAdminStatus       ISendAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<EnergyMeterAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushEnergyMeterAdminStatusResult

            Success(IId                    AuthId,
                    IReceiveAdminStatus    IReceiveAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<EnergyMeterAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushEnergyMeterAdminStatusResult

            Enqueued(IId                    AuthId,
                     ISendAdminStatus       ISendAdminStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<EnergyMeterAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushEnergyMeterAdminStatusResult

            NoOperation(IId                    AuthId,
                        ISendAdminStatus       ISendAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EnergyMeterAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushEnergyMeterAdminStatusResult

            NoOperation(IId                    AuthId,
                        IReceiveAdminStatus    IReceiveAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EnergyMeterAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushEnergyMeterAdminStatusResult

            OutOfService(IId                                                    AuthId,
                         ISendAdminStatus                                       ISendAdminStatus,
                         IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushEnergyMeterAdminStatusResult

            OutOfService(IId                                                    AuthId,
                         IReceiveAdminStatus                                    IReceiveAdminStatus,
                         IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushEnergyMeterAdminStatusResult

            AdminDown(IId                                                    AuthId,
                      ISendAdminStatus                                       ISendAdminStatus,
                      IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushEnergyMeterAdminStatusResult

            AdminDown(IId                                                    AuthId,
                      IReceiveAdminStatus                                    IReceiveAdminStatus,
                      IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushEnergyMeterAdminStatusResult

            Error(IId                                                     AuthId,
                  ISendAdminStatus                                        ISendAdminStatus,
                  IEnumerable<EnergyMeterAdminStatusUpdate>?  RejectedEnergyMeterAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.Error,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushEnergyMeterAdminStatusResult

            Error(IId                                                     AuthId,
                  IReceiveAdminStatus                                     IReceiveAdminStatus,
                  IEnumerable<EnergyMeterAdminStatusUpdate>?  RejectedEnergyMeterAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.Error,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushEnergyMeterAdminStatusResult

            LockTimeout(IId                                                    AuthId,
                        ISendAdminStatus                                       ISendAdminStatus,
                        IEnumerable<EnergyMeterAdminStatusUpdate>  RejectedEnergyMeterAdminStatusUpdates,
                        String?                                                Description   = null,
                        IEnumerable<Warning>?                                  Warnings      = null,
                        TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushEnergyMeterAdminStatusResultTypes.LockTimeout,
                    Description,
                    RejectedEnergyMeterAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(AuthId, ISendAdminStatus, PushEnergyMeterAdminStatusResults, Runtime)

        public static PushEnergyMeterAdminStatusResult Flatten(IId                                                        AuthId,
                                                                           ISendAdminStatus                                           ISendAdminStatus,
                                                                           IEnumerable<PushEnergyMeterAdminStatusResult>  PushEnergyMeterAdminStatusResults,
                                                                           TimeSpan                                                   Runtime)
        {

            #region Initial checks

            if (PushEnergyMeterAdminStatusResults is null || !PushEnergyMeterAdminStatusResults.Any())
                return new PushEnergyMeterAdminStatusResult(AuthId,
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
                    return new PushEnergyMeterAdminStatusResult(all[0].AuthId,
                                                                            ISendAdminStatus,
                                                                            result.Key,
                                                                            descriptions,
                                                                            rejectedEnergyMeterAdminStatusUpdates,
                                                                            warnings,
                                                                            Runtime);

            return new PushEnergyMeterAdminStatusResult(all[0].AuthId,
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
