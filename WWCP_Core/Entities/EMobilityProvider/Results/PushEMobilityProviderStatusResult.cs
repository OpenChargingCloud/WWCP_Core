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
    /// A PushEMobilityProviderStatus result.
    /// </summary>
    public class PushEMobilityProviderStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                               AuthId                                           { get; }

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
        public PushEMobilityProviderStatusResultTypes      Result                                           { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                           Description                                      { get; }

        /// <summary>
        /// An enumeration of rejected charging station operator status updates.
        /// </summary>
        public IEnumerable<EMobilityProviderStatusUpdate>  RejectedEMobilityProviderStatusUpdates     { get; }

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

        #region (private)  PushEMobilityProviderStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushEMobilityProviderStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEMobilityProviderStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushEMobilityProviderStatusResult(IId                                                AuthId,
                                                        PushEMobilityProviderStatusResultTypes       Result,
                                                        String?                                            Description                                    = null,
                                                        IEnumerable<EMobilityProviderStatusUpdate>?  RejectedEMobilityProviderStatusUpdates   = null,
                                                        IEnumerable<Warning>?                              Warnings                                       = null,
                                                        TimeSpan?                                          Runtime                                        = null)
        {

            this.AuthId                                        = AuthId;
            this.Result                                        = Result;

            this.Description                                   = Description is not null && Description.IsNotNullOrEmpty()
                                                                     ? Description.Trim()
                                                                     : String.Empty;

            this.RejectedEMobilityProviderStatusUpdates  = RejectedEMobilityProviderStatusUpdates ?? Array.Empty<EMobilityProviderStatusUpdate>();

            this.Warnings                                      = Warnings is not null
                                                                     ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                                     : Array.Empty<Warning>();

            this.Runtime                                       = Runtime;

        }

        #endregion

        #region (internal) PushEMobilityProviderStatusResult(AuthId, ISendStatus,    Result, ...)

        /// <summary>
        /// Create a new PushEMobilityProviderStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendStatus">An object implementing ISendStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEMobilityProviderStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEMobilityProviderStatusResult(IId                                                AuthId,
                                                         ISendStatus                                        ISendStatus,
                                                         PushEMobilityProviderStatusResultTypes       Result,
                                                         String?                                            Description                                    = null,
                                                         IEnumerable<EMobilityProviderStatusUpdate>?  RejectedEMobilityProviderStatusUpdates   = null,
                                                         IEnumerable<Warning>?                              Warnings                                       = null,
                                                         TimeSpan?                                          Runtime                                        = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedEMobilityProviderStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendStatus = ISendStatus;

        }

        #endregion

        #region (internal) PushEMobilityProviderStatusResult(AuthId, IReceiveStatus, Result, ...)

        /// <summary>
        /// Create a new PushEMobilityProviderStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveStatus">An object implementing IReceiveStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEMobilityProviderStatusUpdates">An enumeration of rejected charging station operator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEMobilityProviderStatusResult(IId                                                AuthId,
                                                         IReceiveStatus                                     IReceiveStatus,
                                                         PushEMobilityProviderStatusResultTypes       Result,
                                                         String?                                            Description                                    = null,
                                                         IEnumerable<EMobilityProviderStatusUpdate>?  RejectedEMobilityProviderStatusUpdates   = null,
                                                         IEnumerable<Warning>?                              Warnings                                       = null,
                                                         TimeSpan?                                          Runtime                                        = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedEMobilityProviderStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveStatus = IReceiveStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushEMobilityProviderStatusResult

            Success(IId                    AuthId,
                    ISendStatus            ISendStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushEMobilityProviderStatusResultTypes.Success,
                    Description,
                    Array.Empty<EMobilityProviderStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushEMobilityProviderStatusResult

            Success(IId                    AuthId,
                    IReceiveStatus         IReceiveStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushEMobilityProviderStatusResultTypes.Success,
                    Description,
                    Array.Empty<EMobilityProviderStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushEMobilityProviderStatusResult

            Enqueued(IId                    AuthId,
                     ISendStatus            ISendStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushEMobilityProviderStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<EMobilityProviderStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushEMobilityProviderStatusResult

            NoOperation(IId                    AuthId,
                        ISendStatus            ISendStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushEMobilityProviderStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EMobilityProviderStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushEMobilityProviderStatusResult

            NoOperation(IId                    AuthId,
                        IReceiveStatus         IReceiveStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushEMobilityProviderStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<EMobilityProviderStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushEMobilityProviderStatusResult

            OutOfService(IId                                               AuthId,
                         ISendStatus                                       ISendStatus,
                         IEnumerable<EMobilityProviderStatusUpdate>  RejectedEMobilityProviderStatusUpdates,
                         String?                                           Description   = null,
                         IEnumerable<Warning>?                             Warnings      = null,
                         TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushEMobilityProviderStatusResultTypes.OutOfService,
                    Description,
                    RejectedEMobilityProviderStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushEMobilityProviderStatusResult

            OutOfService(IId                                               AuthId,
                         IReceiveStatus                                    IReceiveStatus,
                         IEnumerable<EMobilityProviderStatusUpdate>  RejectedEMobilityProviderStatusUpdates,
                         String?                                           Description   = null,
                         IEnumerable<Warning>?                             Warnings      = null,
                         TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushEMobilityProviderStatusResultTypes.OutOfService,
                    Description,
                    RejectedEMobilityProviderStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushEMobilityProviderStatusResult

            AdminDown(IId                                               AuthId,
                      ISendStatus                                       ISendStatus,
                      IEnumerable<EMobilityProviderStatusUpdate>  RejectedEMobilityProviderStatusUpdates,
                      String?                                           Description   = null,
                      IEnumerable<Warning>?                             Warnings      = null,
                      TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushEMobilityProviderStatusResultTypes.AdminDown,
                    Description,
                    RejectedEMobilityProviderStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushEMobilityProviderStatusResult

            AdminDown(IId                                               AuthId,
                      IReceiveStatus                                    IReceiveStatus,
                      IEnumerable<EMobilityProviderStatusUpdate>  RejectedEMobilityProviderStatusUpdates,
                      String?                                           Description   = null,
                      IEnumerable<Warning>?                             Warnings      = null,
                      TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushEMobilityProviderStatusResultTypes.AdminDown,
                    Description,
                    RejectedEMobilityProviderStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushEMobilityProviderStatusResult

            Error(IId                                                AuthId,
                  ISendStatus                                        ISendStatus,
                  IEnumerable<EMobilityProviderStatusUpdate>?  RejectedEMobilityProviderStatusUpdates   = null,
                  String?                                            Description                                    = null,
                  IEnumerable<Warning>?                              Warnings                                       = null,
                  TimeSpan?                                          Runtime                                        = null)

            => new (AuthId,
                    ISendStatus,
                    PushEMobilityProviderStatusResultTypes.Error,
                    Description,
                    RejectedEMobilityProviderStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushEMobilityProviderStatusResult

            Error(IId                                                AuthId,
                  IReceiveStatus                                     IReceiveStatus,
                  IEnumerable<EMobilityProviderStatusUpdate>?  RejectedEMobilityProviderStatusUpdates   = null,
                  String?                                            Description                                    = null,
                  IEnumerable<Warning>?                              Warnings                                       = null,
                  TimeSpan?                                          Runtime                                        = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushEMobilityProviderStatusResultTypes.Error,
                    Description,
                    RejectedEMobilityProviderStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushEMobilityProviderStatusResult

            LockTimeout(IId                                               AuthId,
                        ISendStatus                                       ISendStatus,
                        IEnumerable<EMobilityProviderStatusUpdate>  RejectedEMobilityProviderStatusUpdates,
                        String?                                           Description   = null,
                        IEnumerable<Warning>?                             Warnings      = null,
                        TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushEMobilityProviderStatusResultTypes.LockTimeout,
                    Description,
                    RejectedEMobilityProviderStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(AuthId, ISendStatus, PushEMobilityProviderStatusResults, Runtime)

        public static PushEMobilityProviderStatusResult Flatten(IId                                                   AuthId,
                                                                      ISendStatus                                           ISendStatus,
                                                                      IEnumerable<PushEMobilityProviderStatusResult>  PushEMobilityProviderStatusResults,
                                                                      TimeSpan                                              Runtime)
        {

            #region Initial checks

            if (PushEMobilityProviderStatusResults is null || !PushEMobilityProviderStatusResults.Any())
                return new PushEMobilityProviderStatusResult(AuthId,
                                                                   ISendStatus,
                                                                   PushEMobilityProviderStatusResultTypes.Error,
                                                                   "!",
                                                                   Array.Empty<EMobilityProviderStatusUpdate>(),
                                                                   Array.Empty<Warning>(),
                                                                   Runtime);

            #endregion

            var all                                           = PushEMobilityProviderStatusResults.ToArray();

            var resultOverview                                = all.GroupBy      (result => result.Result).
                                                                    ToDictionary (result => result.Key,
                                                                                  result => new List<PushEMobilityProviderStatusResult>(result));

            var descriptions                                  = all.Where        (result => result is not null).
                                                                    SafeSelect   (result => result.Description).
                                                                    AggregateWith(Environment.NewLine);

            var rejectedEMobilityProviderStatusUpdates  = all.Where        (result => result is not null).
                                                                    SelectMany   (result => result.RejectedEMobilityProviderStatusUpdates);

            var warnings                                      = all.Where        (result => result is not null).
                                                                    SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushEMobilityProviderStatusResult(all[0].AuthId,
                                                                       ISendStatus,
                                                                       result.Key,
                                                                       descriptions,
                                                                       rejectedEMobilityProviderStatusUpdates,
                                                                       warnings,
                                                                       Runtime);

            return new PushEMobilityProviderStatusResult(all[0].AuthId,
                                                               ISendStatus,
                                                               PushEMobilityProviderStatusResultTypes.Partial,
                                                               descriptions,
                                                               rejectedEMobilityProviderStatusUpdates,
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


    public enum PushEMobilityProviderStatusResultTypes
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
