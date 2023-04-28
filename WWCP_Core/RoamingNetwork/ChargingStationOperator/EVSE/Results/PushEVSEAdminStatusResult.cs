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

using System;
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A PushEVSEAdminStatusResult result.
    /// </summary>
    public class PushEVSEAdminStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                 AuthId                            { get; }

        /// <summary>
        /// An object implementing ISendStatus.
        /// </summary>
        public ISendAdminStatus?                   ISendAdminStatus                  { get; }

        /// <summary>
        /// An object implementing IReceiveStatus.
        /// </summary>
        public IReceiveAdminStatus?                IReceiveAdminStatus               { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushEVSEAdminStatusResultTypes      Result                            { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                             Description                       { get; }

        /// <summary>
        /// An enumeration of rejected EVSE admin status updates.
        /// </summary>
        public IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates    { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                Warnings                          { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                           Runtime                           { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushEVSEAdminStatusResult(AuthId,                      Result, ...)

        /// <summary>
        /// Create a new PushEVSEAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEVSEAdminStatusUpdates">An enumeration of rejected EVSE status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushEVSEAdminStatusResult(IId                                  AuthId,
                                          PushEVSEAdminStatusResultTypes       Result,
                                          String?                              Description                      = null,
                                          IEnumerable<EVSEAdminStatusUpdate>?  RejectedEVSEAdminStatusUpdates   = null,
                                          IEnumerable<Warning>?                Warnings                         = null,
                                          TimeSpan?                            Runtime                          = null)
        {

            this.AuthId                          = AuthId;
            this.Result                          = Result;

            this.Description                     = Description.IsNotNullOrEmpty()
                                                       ? Description?.Trim()
                                                       : null;

            this.RejectedEVSEAdminStatusUpdates  = RejectedEVSEAdminStatusUpdates is not null
                                                       ? RejectedEVSEAdminStatusUpdates
                                                       : Array.Empty<EVSEAdminStatusUpdate>();

            this.Warnings                        = Warnings != null
                                                       ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                       : Array.Empty<Warning>();

            this.Runtime                         = Runtime;

        }

        #endregion

        #region (internal) PushEVSEAdminStatusResult(AuthId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushEVSEAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEVSEAdminStatusUpdates">An enumeration of rejected EVSE status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEVSEAdminStatusResult(IId                             AuthId,
                                      ISendAdminStatus                     ISendAdminStatus,
                                      PushEVSEAdminStatusResultTypes       Result,
                                      String?                              Description                      = null,
                                      IEnumerable<EVSEAdminStatusUpdate>?  RejectedEVSEAdminStatusUpdates   = null,
                                      IEnumerable<Warning>?                Warnings                         = null,
                                      TimeSpan?                            Runtime                          = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedEVSEAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushEVSEAdminStatusResult(AuthId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushEVSEAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEVSEAdminStatusUpdates">An enumeration of rejected EVSE status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEVSEAdminStatusResult(IId                                  AuthId,
                                           IReceiveAdminStatus                  IReceiveAdminStatus,
                                           PushEVSEAdminStatusResultTypes       Result,
                                           String?                              Description                      = null,
                                           IEnumerable<EVSEAdminStatusUpdate>?  RejectedEVSEAdminStatusUpdates   = null,
                                           IEnumerable<Warning>?                Warnings                         = null,
                                           TimeSpan?                            Runtime                          = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedEVSEAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushEVSEAdminStatusResult

            Success(IId                    AuthId,
                    ISendAdminStatus       ISendAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

                => new (AuthId,
                        ISendAdminStatus,
                        PushEVSEAdminStatusResultTypes.Success,
                        Description,
                        Array.Empty<EVSEAdminStatusUpdate>(),
                        Warnings,
                        Runtime);


        public static PushEVSEAdminStatusResult

            Success(IId                    AuthId,
                    IReceiveAdminStatus    IReceiveAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

                => new (AuthId,
                        IReceiveAdminStatus,
                        PushEVSEAdminStatusResultTypes.Success,
                        Description,
                        Array.Empty<EVSEAdminStatusUpdate>(),
                        Warnings,
                        Runtime);

        #endregion


        #region Enqueued

        public static PushEVSEAdminStatusResult

            Enqueued(IId                   AuthId,
                     ISendAdminStatus           ISendAdminStatus,
                     String                Description    = null,
                     IEnumerable<Warning>  Warnings       = null,
                     TimeSpan?             Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushEVSEAdminStatusResultTypes.Enqueued,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region NoOperation

        public static PushEVSEAdminStatusResult

            NoOperation(IId                   AuthId,
                        ISendAdminStatus           ISendAdminStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushEVSEAdminStatusResultTypes.NoOperation,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEAdminStatusResult

            NoOperation(IId                   AuthId,
                        IReceiveAdminStatus        IReceiveAdminStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushEVSEAdminStatusResultTypes.NoOperation,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region OutOfService

        public static PushEVSEAdminStatusResult

            OutOfService(IId                                 AuthId,
                         ISendAdminStatus                    ISendAdminStatus,
                         IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                         String                              Description    = null,
                         IEnumerable<Warning>                Warnings       = null,
                         TimeSpan?                           Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                             ISendAdminStatus,
                                             PushEVSEAdminStatusResultTypes.OutOfService,
                                             Description,
                                             RejectedEVSEAdminStatusUpdates,
                                             Warnings,
                                             Runtime);



        public static PushEVSEAdminStatusResult

            OutOfService(IId                            AuthId,
                         IReceiveAdminStatus                 IReceiveAdminStatus,
                         IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushEVSEAdminStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedEVSEAdminStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushEVSEAdminStatusResult

            OutOfService(IId                                       AuthId,
                         ISendAdminStatus                               ISendAdminStatus,
                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushEVSEAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEAdminStatusResult

            OutOfService(IId                                       AuthId,
                         IReceiveAdminStatus                            IReceiveAdminStatus,
                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushEVSEAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEAdminStatusResult

            OutOfService(IId                                    AuthId,
                         ISendAdminStatus                            ISendAdminStatus,
                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushEVSEAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEAdminStatusResult

            OutOfService(IId                                    AuthId,
                         IReceiveAdminStatus                         IReceiveAdminStatus,
                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushEVSEAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region AdminDown

        public static PushEVSEAdminStatusResult

            AdminDown(IId                            AuthId,
                      ISendAdminStatus                    ISendAdminStatus,
                      IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushEVSEAdminStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedEVSEAdminStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushEVSEAdminStatusResult

            AdminDown(IId                            AuthId,
                      IReceiveAdminStatus                 IReceiveAdminStatus,
                      IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushEVSEAdminStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedEVSEAdminStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushEVSEAdminStatusResult

            AdminDown(IId                                       AuthId,
                      ISendAdminStatus                               ISendAdminStatus,
                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushEVSEAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEAdminStatusResult

            AdminDown(IId                                       AuthId,
                      IReceiveAdminStatus                            IReceiveAdminStatus,
                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushEVSEAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEAdminStatusResult

            AdminDown(IId                                    AuthId,
                      ISendAdminStatus                            ISendAdminStatus,
                      IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushEVSEAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEAdminStatusResult

            AdminDown(IId                                    AuthId,
                      IReceiveAdminStatus                         IReceiveAdminStatus,
                      IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedEVSEAdminStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushEVSEAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Error

        public static PushEVSEAdminStatusResult

            Error(IId                                  AuthId,
                  ISendAdminStatus                     ISendAdminStatus,
                  IEnumerable<EVSEAdminStatusUpdate>?  RejectedEVSEs   = null,
                  String?                              Description     = null,
                  IEnumerable<Warning>?                Warnings        = null,
                  TimeSpan?                            Runtime         = null)

                => new (AuthId,
                        ISendAdminStatus,
                        PushEVSEAdminStatusResultTypes.Error,
                        Description,
                        RejectedEVSEs,
                        Warnings,
                        Runtime);


        public static PushEVSEAdminStatusResult

            Error(IId                                  AuthId,
                  IReceiveAdminStatus                  IReceiveAdminStatus,
                  IEnumerable<EVSEAdminStatusUpdate>?  RejectedEVSEs   = null,
                  String?                              Description     = null,
                  IEnumerable<Warning>?                Warnings        = null,
                  TimeSpan?                            Runtime         = null)

                => new (AuthId,
                        IReceiveAdminStatus,
                        PushEVSEAdminStatusResultTypes.Error,
                        Description,
                        RejectedEVSEs,
                        Warnings,
                        Runtime);

        #endregion

        #region LockTimeout

        public static PushEVSEAdminStatusResult LockTimeout(IId                   AuthId,
                                                       ISendAdminStatus           ISendAdminStatus,
                                                       String                Description    = null,
                                                       IEnumerable<Warning>  Warnings       = null,
                                                       TimeSpan?             Runtime        = null)

            => new PushEVSEAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushEVSEAdminStatusResultTypes.LockTimeout,
                                        Description,
                                        new EVSEAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion



        public static PushEVSEAdminStatusResult Flatten(IId                                     AuthId,
                                                        ISendAdminStatus                        ISendAdminStatus,
                                                        IEnumerable<PushEVSEAdminStatusResult>  PushEVSEAdminStatusResults,
                                                        TimeSpan                                Runtime)
        {

            #region Initial checks

            if (PushEVSEAdminStatusResults == null || !PushEVSEAdminStatusResults.Any())
                return new PushEVSEAdminStatusResult(AuthId,
                                                     ISendAdminStatus,
                                                     PushEVSEAdminStatusResultTypes.Error,
                                                     "!",
                                                     new EVSEAdminStatusUpdate[0],
                                                     new Warning[0],
                                                     Runtime);

            #endregion

            var All                            = PushEVSEAdminStatusResults.ToArray();

            var ResultOverview                 = All.GroupBy     (_ => _.Result).
                                                     ToDictionary(_ => _.Key,
                                                                  _ => new List<PushEVSEAdminStatusResult>(_));

            var Descriptions                   = All.Where       (_ => _ != null).
                                                     SafeSelect  (_ => _.Description).
                                                     AggregateWith(Environment.NewLine);

            var RejectedEVSEAdminStatusUpdates = All.Where       (_ => _ != null).
                                                     SelectMany  (_ => _.RejectedEVSEAdminStatusUpdates);

            var Warnings                       = All.Where       (_ => _ != null).
                                                     SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushEVSEAdminStatusResult(All[0].AuthId,
                                                         ISendAdminStatus,
                                                         result.Key,
                                                         Descriptions,
                                                         RejectedEVSEAdminStatusUpdates,
                                                         Warnings,
                                                         Runtime);

            return new PushEVSEAdminStatusResult(All[0].AuthId,
                                                 ISendAdminStatus,
                                                 PushEVSEAdminStatusResultTypes.Partial,
                                                 Descriptions,
                                                 RejectedEVSEAdminStatusUpdates,
                                                 Warnings,
                                                 Runtime);

        }


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }


    public enum PushEVSEAdminStatusResultTypes
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
