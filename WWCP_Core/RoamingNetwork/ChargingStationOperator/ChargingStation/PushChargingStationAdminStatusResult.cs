/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A PushChargingStationAdminStatus result.
    /// </summary>
    public class PushChargingStationAdminStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                 AuthId                            { get; }

        /// <summary>
        /// An object implementing ISendStatus.
        /// </summary>
        public ISendAdminStatus                    ISendAdminStatus                  { get; }

        /// <summary>
        /// An object implementing IReceiveStatus.
        /// </summary>
        public IReceiveAdminStatus                 IReceiveAdminStatus               { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushChargingStationAdminStatusResultTypes      Result                            { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String                              Description                       { get; }

        /// <summary>
        /// An enumeration of rejected ChargingStation admin status updates.
        /// </summary>
        public IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates    { get; }

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

        #region (private)  PushChargingStationAdminStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushChargingStationAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationAdminStatusUpdates">An enumeration of rejected ChargingStation status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationAdminStatusResult(IId                                 AuthId,
                                          PushChargingStationAdminStatusResultTypes      Result,
                                          String                              Description                 = null,
                                          IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates   = null,
                                          IEnumerable<Warning>                Warnings                    = null,
                                          TimeSpan?                           Runtime                     = null)
        {

            this.AuthId                          = AuthId;
            this.Result                          = Result;

            this.Description                     = Description.IsNotNullOrEmpty()
                                                       ? Description.Trim()
                                                       : null;

            this.RejectedChargingStationAdminStatusUpdates  = RejectedChargingStationAdminStatusUpdates != null
                                                       ? RejectedChargingStationAdminStatusUpdates.Where(evse => evse != null)
                                                       : new ChargingStationAdminStatusUpdate[0];

            this.Warnings                        = Warnings != null
                                                       ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                       : new Warning[0];

            this.Runtime                         = Runtime;

        }

        #endregion

        #region (internal) PushChargingStationAdminStatusResult(AuthId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingStationAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationAdminStatusUpdates">An enumeration of rejected ChargingStation status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationAdminStatusResult(IId                            AuthId,
                                      ISendAdminStatus                    ISendAdminStatus,
                                      PushChargingStationAdminStatusResultTypes      Result,
                                      String                              Description                      = null,
                                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates   = null,
                                      IEnumerable<Warning>                Warnings                         = null,
                                      TimeSpan?                           Runtime                          = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingStationAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushChargingStationAdminStatusResult(AuthId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingStationAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationAdminStatusUpdates">An enumeration of rejected ChargingStation status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationAdminStatusResult(IId                                 AuthId,
                                           IReceiveAdminStatus                 IReceiveAdminStatus,
                                           PushChargingStationAdminStatusResultTypes      Result,
                                           String                              Description                      = null,
                                           IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates   = null,
                                           IEnumerable<Warning>                Warnings                         = null,
                                           TimeSpan?                           Runtime                          = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingStationAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingStationAdminStatusResult

            Success(IId                   AuthId,
                    ISendAdminStatus           ISendAdminStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.Success,
                                        Description,
                                        new ChargingStationAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationAdminStatusResult

            Success(IId                   AuthId,
                    IReceiveAdminStatus        IReceiveAdminStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                             IReceiveAdminStatus,
                                             PushChargingStationAdminStatusResultTypes.Success,
                                             Description,
                                             new ChargingStationAdminStatusUpdate[0],
                                             Warnings,
                                             Runtime);

        #endregion


        #region Enqueued

        public static PushChargingStationAdminStatusResult

            Enqueued(IId                   AuthId,
                     ISendAdminStatus           ISendAdminStatus,
                     String                Description    = null,
                     IEnumerable<Warning>  Warnings       = null,
                     TimeSpan?             Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.Enqueued,
                                        Description,
                                        new ChargingStationAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationAdminStatusResult

            NoOperation(IId                   AuthId,
                        ISendAdminStatus           ISendAdminStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.NoOperation,
                                        Description,
                                        new ChargingStationAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationAdminStatusResult

            NoOperation(IId                   AuthId,
                        IReceiveAdminStatus        IReceiveAdminStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.NoOperation,
                                        Description,
                                        new ChargingStationAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region OutOfService

        public static PushChargingStationAdminStatusResult

            OutOfService(IId                                 AuthId,
                         ISendAdminStatus                    ISendAdminStatus,
                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                         String                              Description    = null,
                         IEnumerable<Warning>                Warnings       = null,
                         TimeSpan?                           Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                             ISendAdminStatus,
                                             PushChargingStationAdminStatusResultTypes.OutOfService,
                                             Description,
                                             RejectedChargingStationAdminStatusUpdates,
                                             Warnings,
                                             Runtime);



        public static PushChargingStationAdminStatusResult

            OutOfService(IId                            AuthId,
                         IReceiveAdminStatus                 IReceiveAdminStatus,
                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedChargingStationAdminStatusUpdates,
                                        Warnings,
                                        Runtime);

        public static PushChargingStationAdminStatusResult

            OutOfService(IId                                    AuthId,
                         ISendAdminStatus                            ISendAdminStatus,
                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationAdminStatusResult

            OutOfService(IId                                    AuthId,
                         IReceiveAdminStatus                         IReceiveAdminStatus,
                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region AdminDown

        public static PushChargingStationAdminStatusResult

            AdminDown(IId                            AuthId,
                      ISendAdminStatus                    ISendAdminStatus,
                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedChargingStationAdminStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingStationAdminStatusResult

            AdminDown(IId                            AuthId,
                      IReceiveAdminStatus                 IReceiveAdminStatus,
                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedChargingStationAdminStatusUpdates,
                                        Warnings,
                                        Runtime);


        public static PushChargingStationAdminStatusResult

            AdminDown(IId                                    AuthId,
                      ISendAdminStatus                            ISendAdminStatus,
                      IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationAdminStatusResult

            AdminDown(IId                                    AuthId,
                      IReceiveAdminStatus                         IReceiveAdminStatus,
                      IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingStationAdminStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Error

        public static PushChargingStationAdminStatusResult

            Error(IId                            AuthId,
                  ISendAdminStatus                    ISendAdminStatus,
                  IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStations  = null,
                  String                         Description    = null,
                  IEnumerable<Warning>           Warnings       = null,
                  TimeSpan?                      Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.Error,
                                        Description,
                                        RejectedChargingStations,
                                        Warnings,
                                        Runtime);


        public static PushChargingStationAdminStatusResult Error(IId                            AuthId,
                                                 IReceiveAdminStatus                 IReceiveAdminStatus,
                                                 IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStations  = null,
                                                 String                         Description    = null,
                                                 IEnumerable<Warning>           Warnings       = null,
                                                 TimeSpan?                      Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.Error,
                                        Description,
                                        RejectedChargingStations,
                                        Warnings,
                                        Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingStationAdminStatusResult LockTimeout(IId                   AuthId,
                                                       ISendAdminStatus           ISendAdminStatus,
                                                       String                Description    = null,
                                                       IEnumerable<Warning>  Warnings       = null,
                                                       TimeSpan?             Runtime        = null)

            => new PushChargingStationAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationAdminStatusResultTypes.LockTimeout,
                                        Description,
                                        new ChargingStationAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion



        public static PushChargingStationAdminStatusResult Flatten(IId                                     AuthId,
                                                        ISendAdminStatus                        ISendAdminStatus,
                                                        IEnumerable<PushChargingStationAdminStatusResult>  PushChargingStationAdminStatusResults,
                                                        TimeSpan                                Runtime)
        {

            #region Initial checks

            if (PushChargingStationAdminStatusResults == null || !PushChargingStationAdminStatusResults.Any())
                return new PushChargingStationAdminStatusResult(AuthId,
                                                     ISendAdminStatus,
                                                     PushChargingStationAdminStatusResultTypes.Error,
                                                     "!",
                                                     new ChargingStationAdminStatusUpdate[0],
                                                     new Warning[0],
                                                     Runtime);

            #endregion

            var All                            = PushChargingStationAdminStatusResults.ToArray();

            var ResultOverview                 = All.GroupBy     (_ => _.Result).
                                                     ToDictionary(_ => _.Key,
                                                                  _ => new List<PushChargingStationAdminStatusResult>(_));

            var Descriptions                   = All.Where       (_ => _ != null).
                                                     SafeSelect  (_ => _.Description).
                                                     AggregateWith(Environment.NewLine);

            var RejectedChargingStationAdminStatusUpdates = All.Where       (_ => _ != null).
                                                     SelectMany  (_ => _.RejectedChargingStationAdminStatusUpdates);

            var Warnings                       = All.Where       (_ => _ != null).
                                                     SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushChargingStationAdminStatusResult(All[0].AuthId,
                                                         ISendAdminStatus,
                                                         result.Key,
                                                         Descriptions,
                                                         RejectedChargingStationAdminStatusUpdates,
                                                         Warnings,
                                                         Runtime);

            return new PushChargingStationAdminStatusResult(All[0].AuthId,
                                                 ISendAdminStatus,
                                                 PushChargingStationAdminStatusResultTypes.Partial,
                                                 Descriptions,
                                                 RejectedChargingStationAdminStatusUpdates,
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


    public enum PushChargingStationAdminStatusResultTypes
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
