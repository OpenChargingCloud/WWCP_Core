﻿/*
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
    /// A PushChargingPoolStatus result.
    /// </summary>
    public class PushChargingPoolStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                    AuthId                                { get; }

        /// <summary>
        /// An object implementing ISendStatus.
        /// </summary>
        public ISendStatus?                           ISendStatus                           { get; }

        /// <summary>
        /// An object implementing IReceiveStatus.
        /// </summary>
        public IReceiveStatus?                        IReceiveStatus                        { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushChargingPoolStatusResultTypes      Result                                { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                Description                           { get; }

        /// <summary>
        /// An enumeration of rejected charging pool status updates.
        /// </summary>
        public IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingPoolStatusUpdates     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                   Warnings                              { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                              Runtime                               { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushChargingPoolStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolStatusUpdates">An enumeration of rejected charging pool status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingPoolStatusResult(IId                                     AuthId,
                                             PushChargingPoolStatusResultTypes       Result,
                                             String?                                 Description                         = null,
                                             IEnumerable<ChargingPoolStatusUpdate>?  RejectedChargingPoolStatusUpdates   = null,
                                             IEnumerable<Warning>?                   Warnings                            = null,
                                             TimeSpan?                               Runtime                             = null)
        {

            this.AuthId                             = AuthId;
            this.Result                             = Result;

            this.Description                        = Description is not null && Description.IsNotNullOrEmpty()
                                                          ? Description.Trim()
                                                          : String.Empty;

            this.RejectedChargingPoolStatusUpdates  = RejectedChargingPoolStatusUpdates ?? Array.Empty<ChargingPoolStatusUpdate>();

            this.Warnings                           = Warnings is not null
                                                          ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                          : Array.Empty<Warning>();

            this.Runtime                            = Runtime;

        }

        #endregion

        #region (internal) PushChargingPoolStatusResult(AuthId, ISendStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendStatus">An object implementing ISendStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolStatusUpdates">An enumeration of rejected charging pool status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingPoolStatusResult(IId                                     AuthId,
                                              ISendStatus                             ISendStatus,
                                              PushChargingPoolStatusResultTypes       Result,
                                              String?                                 Description                         = null,
                                              IEnumerable<ChargingPoolStatusUpdate>?  RejectedChargingPoolStatusUpdates   = null,
                                              IEnumerable<Warning>?                   Warnings                            = null,
                                              TimeSpan?                               Runtime                             = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingPoolStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendStatus = ISendStatus;

        }

        #endregion

        #region (internal) PushChargingPoolStatusResult(AuthId, IReceiveStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveStatus">An object implementing IReceiveStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolStatusUpdates">An enumeration of rejected charging pool status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingPoolStatusResult(IId                                     AuthId,
                                              IReceiveStatus                          IReceiveStatus,
                                              PushChargingPoolStatusResultTypes       Result,
                                              String?                                 Description                         = null,
                                              IEnumerable<ChargingPoolStatusUpdate>?  RejectedChargingPoolStatusUpdates   = null,
                                              IEnumerable<Warning>?                   Warnings                            = null,
                                              TimeSpan?                               Runtime                             = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingPoolStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveStatus = IReceiveStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingPoolStatusResult

            Success(IId                    AuthId,
                    ISendStatus            ISendStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingPoolStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingPoolStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingPoolStatusResult

            Success(IId                    AuthId,
                    IReceiveStatus         IReceiveStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingPoolStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingPoolStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushChargingPoolStatusResult

            Enqueued(IId                    AuthId,
                     ISendStatus            ISendStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingPoolStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<ChargingPoolStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushChargingPoolStatusResult

            NoOperation(IId                    AuthId,
                        ISendStatus            ISendStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingPoolStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingPoolStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingPoolStatusResult

            NoOperation(IId                    AuthId,
                        IReceiveStatus         IReceiveStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingPoolStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingPoolStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushChargingPoolStatusResult

            OutOfService(IId                                               AuthId,
                         ISendStatus                                       ISendStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingPoolStatusUpdates,
                         String?                                           Description   = null,
                         IEnumerable<Warning>?                             Warnings      = null,
                         TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingPoolStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingPoolStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingPoolStatusResult

            OutOfService(IId                                               AuthId,
                         IReceiveStatus                                    IReceiveStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingPoolStatusUpdates,
                         String?                                           Description   = null,
                         IEnumerable<Warning>?                             Warnings      = null,
                         TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingPoolStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingPoolStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushChargingPoolStatusResult

            AdminDown(IId                                               AuthId,
                      ISendStatus                                       ISendStatus,
                      IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingPoolStatusUpdates,
                      String?                                           Description   = null,
                      IEnumerable<Warning>?                             Warnings      = null,
                      TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingPoolStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingPoolStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingPoolStatusResult

            AdminDown(IId                                               AuthId,
                      IReceiveStatus                                    IReceiveStatus,
                      IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingPoolStatusUpdates,
                      String?                                           Description   = null,
                      IEnumerable<Warning>?                             Warnings      = null,
                      TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingPoolStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingPoolStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushChargingPoolStatusResult

            Error(IId                                                AuthId,
                  ISendStatus                                        ISendStatus,
                  IEnumerable<ChargingPoolStatusUpdate>?  RejectedChargingPoolStatusUpdates   = null,
                  String?                                            Description                                    = null,
                  IEnumerable<Warning>?                              Warnings                                       = null,
                  TimeSpan?                                          Runtime                                        = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingPoolStatusResultTypes.Error,
                    Description,
                    RejectedChargingPoolStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingPoolStatusResult

            Error(IId                                                AuthId,
                  IReceiveStatus                                     IReceiveStatus,
                  IEnumerable<ChargingPoolStatusUpdate>?  RejectedChargingPoolStatusUpdates   = null,
                  String?                                            Description                                    = null,
                  IEnumerable<Warning>?                              Warnings                                       = null,
                  TimeSpan?                                          Runtime                                        = null)

            => new (AuthId,
                    IReceiveStatus,
                    PushChargingPoolStatusResultTypes.Error,
                    Description,
                    RejectedChargingPoolStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingPoolStatusResult

            LockTimeout(IId                                               AuthId,
                        ISendStatus                                       ISendStatus,
                        IEnumerable<ChargingPoolStatusUpdate>  RejectedChargingPoolStatusUpdates,
                        String?                                           Description   = null,
                        IEnumerable<Warning>?                             Warnings      = null,
                        TimeSpan?                                         Runtime       = null)

            => new (AuthId,
                    ISendStatus,
                    PushChargingPoolStatusResultTypes.LockTimeout,
                    Description,
                    RejectedChargingPoolStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(AuthId, ISendStatus, PushChargingPoolStatusResults, Runtime)

        public static PushChargingPoolStatusResult Flatten(IId                                                   AuthId,
                                                                      ISendStatus                                           ISendStatus,
                                                                      IEnumerable<PushChargingPoolStatusResult>  PushChargingPoolStatusResults,
                                                                      TimeSpan                                              Runtime)
        {

            #region Initial checks

            if (PushChargingPoolStatusResults is null || !PushChargingPoolStatusResults.Any())
                return new PushChargingPoolStatusResult(AuthId,
                                                                   ISendStatus,
                                                                   PushChargingPoolStatusResultTypes.Error,
                                                                   "!",
                                                                   Array.Empty<ChargingPoolStatusUpdate>(),
                                                                   Array.Empty<Warning>(),
                                                                   Runtime);

            #endregion

            var all                                           = PushChargingPoolStatusResults.ToArray();

            var resultOverview                                = all.GroupBy      (result => result.Result).
                                                                    ToDictionary (result => result.Key,
                                                                                  result => new List<PushChargingPoolStatusResult>(result));

            var descriptions                                  = all.Where        (result => result is not null).
                                                                    SafeSelect   (result => result.Description).
                                                                    AggregateWith(Environment.NewLine);

            var rejectedChargingPoolStatusUpdates  = all.Where        (result => result is not null).
                                                                    SelectMany   (result => result.RejectedChargingPoolStatusUpdates);

            var warnings                                      = all.Where        (result => result is not null).
                                                                    SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushChargingPoolStatusResult(all[0].AuthId,
                                                                       ISendStatus,
                                                                       result.Key,
                                                                       descriptions,
                                                                       rejectedChargingPoolStatusUpdates,
                                                                       warnings,
                                                                       Runtime);

            return new PushChargingPoolStatusResult(all[0].AuthId,
                                                               ISendStatus,
                                                               PushChargingPoolStatusResultTypes.Partial,
                                                               descriptions,
                                                               rejectedChargingPoolStatusUpdates,
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



    public enum PushChargingPoolStatusResultTypes
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
