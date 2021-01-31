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
    /// A PushChargingPoolAdminStatusResult result.
    /// </summary>
    public class PushChargingPoolAdminStatusResult
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
        public PushChargingPoolAdminStatusResultTypes      Result                            { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String                              Description                       { get; }

        /// <summary>
        /// An enumeration of rejected ChargingPool admin status updates.
        /// </summary>
        public IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates    { get; }

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

        #region (private)  PushChargingPoolAdminStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolAdminStatusUpdates">An enumeration of rejected ChargingPool status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingPoolAdminStatusResult(IId                                         AuthId,
                                                  PushChargingPoolAdminStatusResultTypes      Result,
                                                  String                                      Description                              = null,
                                                  IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates   = null,
                                                  IEnumerable<Warning>                        Warnings                                 = null,
                                                  TimeSpan?                                   Runtime                                  = null)
        {

            this.AuthId                          = AuthId;
            this.Result                          = Result;

            this.Description                     = Description.IsNotNullOrEmpty()
                                                       ? Description.Trim()
                                                       : null;

            this.RejectedChargingPoolAdminStatusUpdates  = RejectedChargingPoolAdminStatusUpdates != null
                                                       ? RejectedChargingPoolAdminStatusUpdates.Where(evse => evse != null)
                                                       : new ChargingPoolAdminStatusUpdate[0];

            this.Warnings                        = Warnings != null
                                                       ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                       : new Warning[0];

            this.Runtime                         = Runtime;

        }

        #endregion

        #region (internal) PushChargingPoolAdminStatusResult(AuthId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolAdminStatusUpdates">An enumeration of rejected ChargingPool status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingPoolAdminStatusResult(IId                                         AuthId,
                                                   ISendAdminStatus                            ISendAdminStatus,
                                                   PushChargingPoolAdminStatusResultTypes      Result,
                                                   String                                      Description                              = null,
                                                   IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates   = null,
                                                   IEnumerable<Warning>                        Warnings                                 = null,
                                                   TimeSpan?                                   Runtime                                  = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingPoolAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushChargingPoolAdminStatusResult(AuthId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolAdminStatusUpdates">An enumeration of rejected ChargingPool status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingPoolAdminStatusResult(IId                                         AuthId,
                                                   IReceiveAdminStatus                         IReceiveAdminStatus,
                                                   PushChargingPoolAdminStatusResultTypes      Result,
                                                   String                                      Description                              = null,
                                                   IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates   = null,
                                                   IEnumerable<Warning>                        Warnings                                 = null,
                                                   TimeSpan?                                   Runtime                                  = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingPoolAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingPoolAdminStatusResult

            Success(IId                   AuthId,
                    ISendAdminStatus           ISendAdminStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.Success,
                                        Description,
                                        new ChargingPoolAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingPoolAdminStatusResult

            Success(IId                   AuthId,
                    IReceiveAdminStatus        IReceiveAdminStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                             IReceiveAdminStatus,
                                             PushChargingPoolAdminStatusResultTypes.Success,
                                             Description,
                                             new ChargingPoolAdminStatusUpdate[0],
                                             Warnings,
                                             Runtime);

        #endregion


        #region Enqueued

        public static PushChargingPoolAdminStatusResult

            Enqueued(IId                   AuthId,
                     ISendAdminStatus           ISendAdminStatus,
                     String                Description    = null,
                     IEnumerable<Warning>  Warnings       = null,
                     TimeSpan?             Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.Enqueued,
                                        Description,
                                        new ChargingPoolAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region NoOperation

        public static PushChargingPoolAdminStatusResult

            NoOperation(IId                   AuthId,
                        ISendAdminStatus           ISendAdminStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.NoOperation,
                                        Description,
                                        new ChargingPoolAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingPoolAdminStatusResult

            NoOperation(IId                   AuthId,
                        IReceiveAdminStatus        IReceiveAdminStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.NoOperation,
                                        Description,
                                        new ChargingPoolAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region OutOfService

        public static PushChargingPoolAdminStatusResult

            OutOfService(IId                                 AuthId,
                         ISendAdminStatus                    ISendAdminStatus,
                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                         String                              Description    = null,
                         IEnumerable<Warning>                Warnings       = null,
                         TimeSpan?                           Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                             ISendAdminStatus,
                                             PushChargingPoolAdminStatusResultTypes.OutOfService,
                                             Description,
                                             RejectedChargingPoolAdminStatusUpdates,
                                             Warnings,
                                             Runtime);



        public static PushChargingPoolAdminStatusResult

            OutOfService(IId                            AuthId,
                         IReceiveAdminStatus                 IReceiveAdminStatus,
                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedChargingPoolAdminStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingPoolAdminStatusResult

            OutOfService(IId                                       AuthId,
                         ISendAdminStatus                               ISendAdminStatus,
                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingPoolAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingPoolAdminStatusResult

            OutOfService(IId                                       AuthId,
                         IReceiveAdminStatus                            IReceiveAdminStatus,
                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingPoolAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region AdminDown

        public static PushChargingPoolAdminStatusResult

            AdminDown(IId                                         AuthId,
                      ISendAdminStatus                            ISendAdminStatus,
                      IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                      String                                      Description   = null,
                      IEnumerable<Warning>                        Warnings      = null,
                      TimeSpan?                                   Runtime       = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedChargingPoolAdminStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingPoolAdminStatusResult

            AdminDown(IId                                         AuthId,
                      IReceiveAdminStatus                         IReceiveAdminStatus,
                      IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                      String                                      Description   = null,
                      IEnumerable<Warning>                        Warnings      = null,
                      TimeSpan?                                   Runtime       = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedChargingPoolAdminStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingPoolAdminStatusResult

            AdminDown(IId                                       AuthId,
                      ISendAdminStatus                               ISendAdminStatus,
                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingPoolAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingPoolAdminStatusResult

            AdminDown(IId                                       AuthId,
                      IReceiveAdminStatus                            IReceiveAdminStatus,
                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingPoolAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Error

        public static PushChargingPoolAdminStatusResult

            Error(IId                            AuthId,
                  ISendAdminStatus                    ISendAdminStatus,
                  IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPools  = null,
                  String                         Description    = null,
                  IEnumerable<Warning>           Warnings       = null,
                  TimeSpan?                      Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.Error,
                                        Description,
                                        RejectedChargingPools,
                                        Warnings,
                                        Runtime);


        public static PushChargingPoolAdminStatusResult Error(IId                            AuthId,
                                                 IReceiveAdminStatus                 IReceiveAdminStatus,
                                                 IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPools  = null,
                                                 String                         Description    = null,
                                                 IEnumerable<Warning>           Warnings       = null,
                                                 TimeSpan?                      Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.Error,
                                        Description,
                                        RejectedChargingPools,
                                        Warnings,
                                        Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingPoolAdminStatusResult LockTimeout(IId                   AuthId,
                                                       ISendAdminStatus           ISendAdminStatus,
                                                       String                Description    = null,
                                                       IEnumerable<Warning>  Warnings       = null,
                                                       TimeSpan?             Runtime        = null)

            => new PushChargingPoolAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingPoolAdminStatusResultTypes.LockTimeout,
                                        Description,
                                        new ChargingPoolAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion



        public static PushChargingPoolAdminStatusResult Flatten(IId                                     AuthId,
                                                        ISendAdminStatus                        ISendAdminStatus,
                                                        IEnumerable<PushChargingPoolAdminStatusResult>  PushChargingPoolAdminStatusResults,
                                                        TimeSpan                                Runtime)
        {

            #region Initial checks

            if (PushChargingPoolAdminStatusResults == null || !PushChargingPoolAdminStatusResults.Any())
                return new PushChargingPoolAdminStatusResult(AuthId,
                                                     ISendAdminStatus,
                                                     PushChargingPoolAdminStatusResultTypes.Error,
                                                     "!",
                                                     new ChargingPoolAdminStatusUpdate[0],
                                                     new Warning[0],
                                                     Runtime);

            #endregion

            var All                            = PushChargingPoolAdminStatusResults.ToArray();

            var ResultOverview                 = All.GroupBy     (_ => _.Result).
                                                     ToDictionary(_ => _.Key,
                                                                  _ => new List<PushChargingPoolAdminStatusResult>(_));

            var Descriptions                   = All.Where       (_ => _ != null).
                                                     SafeSelect  (_ => _.Description).
                                                     AggregateWith(Environment.NewLine);

            var RejectedChargingPoolAdminStatusUpdates = All.Where       (_ => _ != null).
                                                     SelectMany  (_ => _.RejectedChargingPoolAdminStatusUpdates);

            var Warnings                       = All.Where       (_ => _ != null).
                                                     SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushChargingPoolAdminStatusResult(All[0].AuthId,
                                                         ISendAdminStatus,
                                                         result.Key,
                                                         Descriptions,
                                                         RejectedChargingPoolAdminStatusUpdates,
                                                         Warnings,
                                                         Runtime);

            return new PushChargingPoolAdminStatusResult(All[0].AuthId,
                                                 ISendAdminStatus,
                                                 PushChargingPoolAdminStatusResultTypes.Partial,
                                                 Descriptions,
                                                 RejectedChargingPoolAdminStatusUpdates,
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


    public enum PushChargingPoolAdminStatusResultTypes
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
