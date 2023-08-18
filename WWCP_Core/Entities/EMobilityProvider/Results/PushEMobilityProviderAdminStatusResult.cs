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
    /// A PushEMobilityProviderAdminStatus result.
    /// </summary>
    public class PushEMobilityProviderAdminStatusResult
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
        public PushEMobilityProviderAdminStatusResultTypes      Result                                                { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                                Description                                           { get; }

        /// <summary>
        /// An enumeration of rejected charging station operator admin status updates.
        /// </summary>
        public IEnumerable<EMobilityProviderAdminStatusUpdate>  RejectedEMobilityProviderAdminStatusUpdates     { get; }

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

        #region (private)  PushEMobilityProviderAdminStatusResult(SenderId,                      Result, ...)

        /// <summary>
        /// Create a new PushEMobilityProviderAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEMobilityProviderAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushEMobilityProviderAdminStatusResult(IId                                               SenderId,
                                                       PushEMobilityProviderAdminStatusResultTypes       Result,
                                                       String?                                           Description                                   = null,
                                                       IEnumerable<EMobilityProviderAdminStatusUpdate>?  RejectedEMobilityProviderAdminStatusUpdates   = null,
                                                       IEnumerable<Warning>?                             Warnings                                      = null,
                                                       TimeSpan?                                         Runtime                                       = null)
        {

            this.SenderId                                           = SenderId;
            this.Result                                             = Result;

            this.Description                                        = Description is not null && Description.IsNotNullOrEmpty()
                                                                          ? Description.Trim()
                                                                          : String.Empty;

            this.RejectedEMobilityProviderAdminStatusUpdates  = RejectedEMobilityProviderAdminStatusUpdates ?? Array.Empty<EMobilityProviderAdminStatusUpdate>();

            this.Warnings                                           = Warnings is not null
                                                                          ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                                          : Array.Empty<Warning>();

            this.Runtime                                            = Runtime;

        }

        #endregion

        #region (internal) PushEMobilityProviderAdminStatusResult(SenderId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushEMobilityProviderAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEMobilityProviderAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEMobilityProviderAdminStatusResult(IId                                               SenderId,
                                                        ISendAdminStatus                                  ISendAdminStatus,
                                                        PushEMobilityProviderAdminStatusResultTypes       Result,
                                                        String?                                           Description                                   = null,
                                                        IEnumerable<EMobilityProviderAdminStatusUpdate>?  RejectedEMobilityProviderAdminStatusUpdates   = null,
                                                        IEnumerable<Warning>?                             Warnings                                      = null,
                                                        TimeSpan?                                         Runtime                                       = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedEMobilityProviderAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushEMobilityProviderAdminStatusResult(SenderId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushEMobilityProviderAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEMobilityProviderAdminStatusUpdates">An enumeration of rejected charging station operator admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEMobilityProviderAdminStatusResult(IId                                               SenderId,
                                                        IReceiveAdminStatus                               IReceiveAdminStatus,
                                                        PushEMobilityProviderAdminStatusResultTypes       Result,
                                                        String?                                           Description                                   = null,
                                                        IEnumerable<EMobilityProviderAdminStatusUpdate>?  RejectedEMobilityProviderAdminStatusUpdates   = null,
                                                        IEnumerable<Warning>?                             Warnings                                      = null,
                                                        TimeSpan?                                         Runtime                                       = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedEMobilityProviderAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushEMobilityProviderAdminStatusResult

            Success(IId                    SenderId,
                    ISendAdminStatus       ISendAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<EMobilityProviderAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushEMobilityProviderAdminStatusResult

            Success(IId                    SenderId,
                    IReceiveAdminStatus    IReceiveAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<EMobilityProviderAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushEMobilityProviderAdminStatusResult

            Enqueued(IId                    SenderId,
                     ISendAdminStatus       ISendAdminStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<EMobilityProviderAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushEMobilityProviderAdminStatusResult

            NoOperation(IId                    SenderId,
                        ISendAdminStatus       ISendAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EMobilityProviderAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushEMobilityProviderAdminStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveAdminStatus    IReceiveAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EMobilityProviderAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushEMobilityProviderAdminStatusResult

            OutOfService(IId                                                    SenderId,
                         ISendAdminStatus                                       ISendAdminStatus,
                         IEnumerable<EMobilityProviderAdminStatusUpdate>  RejectedEMobilityProviderAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedEMobilityProviderAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushEMobilityProviderAdminStatusResult

            OutOfService(IId                                                    SenderId,
                         IReceiveAdminStatus                                    IReceiveAdminStatus,
                         IEnumerable<EMobilityProviderAdminStatusUpdate>  RejectedEMobilityProviderAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedEMobilityProviderAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushEMobilityProviderAdminStatusResult

            AdminDown(IId                                                    SenderId,
                      ISendAdminStatus                                       ISendAdminStatus,
                      IEnumerable<EMobilityProviderAdminStatusUpdate>  RejectedEMobilityProviderAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedEMobilityProviderAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushEMobilityProviderAdminStatusResult

            AdminDown(IId                                                    SenderId,
                      IReceiveAdminStatus                                    IReceiveAdminStatus,
                      IEnumerable<EMobilityProviderAdminStatusUpdate>  RejectedEMobilityProviderAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedEMobilityProviderAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushEMobilityProviderAdminStatusResult

            Error(IId                                                     SenderId,
                  ISendAdminStatus                                        ISendAdminStatus,
                  IEnumerable<EMobilityProviderAdminStatusUpdate>?  RejectedEMobilityProviderAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.Error,
                    Description,
                    RejectedEMobilityProviderAdminStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushEMobilityProviderAdminStatusResult

            Error(IId                                                     SenderId,
                  IReceiveAdminStatus                                     IReceiveAdminStatus,
                  IEnumerable<EMobilityProviderAdminStatusUpdate>?  RejectedEMobilityProviderAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.Error,
                    Description,
                    RejectedEMobilityProviderAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushEMobilityProviderAdminStatusResult

            LockTimeout(IId                                                    SenderId,
                        ISendAdminStatus                                       ISendAdminStatus,
                        IEnumerable<EMobilityProviderAdminStatusUpdate>  RejectedEMobilityProviderAdminStatusUpdates,
                        String?                                                Description   = null,
                        IEnumerable<Warning>?                                  Warnings      = null,
                        TimeSpan?                                              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushEMobilityProviderAdminStatusResultTypes.LockTimeout,
                    Description,
                    RejectedEMobilityProviderAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(SenderId, ISendAdminStatus, PushEMobilityProviderAdminStatusResults, Runtime)

        public static PushEMobilityProviderAdminStatusResult Flatten(IId                                                        SenderId,
                                                                           ISendAdminStatus                                           ISendAdminStatus,
                                                                           IEnumerable<PushEMobilityProviderAdminStatusResult>  PushEMobilityProviderAdminStatusResults,
                                                                           TimeSpan                                                   Runtime)
        {

            #region Initial checks

            if (PushEMobilityProviderAdminStatusResults is null || !PushEMobilityProviderAdminStatusResults.Any())
                return new PushEMobilityProviderAdminStatusResult(SenderId,
                                                                        ISendAdminStatus,
                                                                        PushEMobilityProviderAdminStatusResultTypes.Error,
                                                                        "!",
                                                                        Array.Empty<EMobilityProviderAdminStatusUpdate>(),
                                                                        Array.Empty<Warning>(),
                                                                        Runtime);

            #endregion

            var all                                                = PushEMobilityProviderAdminStatusResults.ToArray();

            var resultOverview                                     = all.GroupBy      (result => result.Result).
                                                                         ToDictionary (result => result.Key,
                                                                                       result => new List<PushEMobilityProviderAdminStatusResult>(result));

            var descriptions                                       = all.Where        (result => result is not null).
                                                                         SafeSelect   (result => result.Description).
                                                                         AggregateWith(Environment.NewLine);

            var rejectedEMobilityProviderAdminStatusUpdates  = all.Where        (result => result is not null).
                                                                         SelectMany   (result => result.RejectedEMobilityProviderAdminStatusUpdates);

            var warnings                                           = all.Where        (result => result is not null).
                                                                         SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushEMobilityProviderAdminStatusResult(all[0].SenderId,
                                                                            ISendAdminStatus,
                                                                            result.Key,
                                                                            descriptions,
                                                                            rejectedEMobilityProviderAdminStatusUpdates,
                                                                            warnings,
                                                                            Runtime);

            return new PushEMobilityProviderAdminStatusResult(all[0].SenderId,
                                                                    ISendAdminStatus,
                                                                    PushEMobilityProviderAdminStatusResultTypes.Partial,
                                                                    descriptions,
                                                                    rejectedEMobilityProviderAdminStatusUpdates,
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


    public enum PushEMobilityProviderAdminStatusResultTypes
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
