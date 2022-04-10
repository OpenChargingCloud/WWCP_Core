/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A PushChargingStationOperatorAdminStatusResult result.
    /// </summary>
    public class PushChargingStationOperatorAdminStatusResult
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
        public PushChargingStationOperatorAdminStatusResultTypes      Result                            { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String                              Description                       { get; }

        /// <summary>
        /// An enumeration of rejected ChargingStationOperator admin status updates.
        /// </summary>
        public IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates    { get; }

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

        #region (private)  PushChargingStationOperatorAdminStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushChargingStationOperatorAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationOperatorAdminStatusUpdates">An enumeration of rejected ChargingStationOperator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingStationOperatorAdminStatusResult(IId                                 AuthId,
                                          PushChargingStationOperatorAdminStatusResultTypes      Result,
                                          String                              Description                 = null,
                                          IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates   = null,
                                          IEnumerable<Warning>                Warnings                    = null,
                                          TimeSpan?                           Runtime                     = null)
        {

            this.AuthId                          = AuthId;
            this.Result                          = Result;

            this.Description                     = Description.IsNotNullOrEmpty()
                                                       ? Description.Trim()
                                                       : null;

            this.RejectedChargingStationOperatorAdminStatusUpdates  = RejectedChargingStationOperatorAdminStatusUpdates != null
                                                       ? RejectedChargingStationOperatorAdminStatusUpdates.Where(evse => evse != null)
                                                       : new ChargingStationOperatorAdminStatusUpdate[0];

            this.Warnings                        = Warnings != null
                                                       ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                       : new Warning[0];

            this.Runtime                         = Runtime;

        }

        #endregion

        #region (internal) PushChargingStationOperatorAdminStatusResult(AuthId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingStationOperatorAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationOperatorAdminStatusUpdates">An enumeration of rejected ChargingStationOperator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationOperatorAdminStatusResult(IId                            AuthId,
                                      ISendAdminStatus                    ISendAdminStatus,
                                      PushChargingStationOperatorAdminStatusResultTypes      Result,
                                      String                              Description                      = null,
                                      IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates   = null,
                                      IEnumerable<Warning>                Warnings                         = null,
                                      TimeSpan?                           Runtime                          = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingStationOperatorAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushChargingStationOperatorAdminStatusResult(AuthId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingStationOperatorAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingStationOperatorAdminStatusUpdates">An enumeration of rejected ChargingStationOperator status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingStationOperatorAdminStatusResult(IId                                 AuthId,
                                           IReceiveAdminStatus                 IReceiveAdminStatus,
                                           PushChargingStationOperatorAdminStatusResultTypes      Result,
                                           String                              Description                      = null,
                                           IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates   = null,
                                           IEnumerable<Warning>                Warnings                         = null,
                                           TimeSpan?                           Runtime                          = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingStationOperatorAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingStationOperatorAdminStatusResult

            Success(IId                   AuthId,
                    ISendAdminStatus           ISendAdminStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.Success,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            Success(IId                   AuthId,
                    IReceiveAdminStatus        IReceiveAdminStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                             IReceiveAdminStatus,
                                             PushChargingStationOperatorAdminStatusResultTypes.Success,
                                             Description,
                                             new ChargingStationOperatorAdminStatusUpdate[0],
                                             Warnings,
                                             Runtime);

        #endregion


        #region Enqueued

        public static PushChargingStationOperatorAdminStatusResult

            Enqueued(IId                   AuthId,
                     ISendAdminStatus           ISendAdminStatus,
                     String                Description    = null,
                     IEnumerable<Warning>  Warnings       = null,
                     TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.Enqueued,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region NoOperation

        public static PushChargingStationOperatorAdminStatusResult

            NoOperation(IId                   AuthId,
                        ISendAdminStatus           ISendAdminStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.NoOperation,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            NoOperation(IId                   AuthId,
                        IReceiveAdminStatus        IReceiveAdminStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.NoOperation,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region OutOfService

        public static PushChargingStationOperatorAdminStatusResult

            OutOfService(IId                                 AuthId,
                         ISendAdminStatus                    ISendAdminStatus,
                         IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                         String                              Description    = null,
                         IEnumerable<Warning>                Warnings       = null,
                         TimeSpan?                           Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                             ISendAdminStatus,
                                             PushChargingStationOperatorAdminStatusResultTypes.OutOfService,
                                             Description,
                                             RejectedChargingStationOperatorAdminStatusUpdates,
                                             Warnings,
                                             Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            OutOfService(IId                            AuthId,
                         IReceiveAdminStatus                 IReceiveAdminStatus,
                         IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedChargingStationOperatorAdminStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            OutOfService(IId                                       AuthId,
                         ISendAdminStatus                               ISendAdminStatus,
                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            OutOfService(IId                                       AuthId,
                         IReceiveAdminStatus                            IReceiveAdminStatus,
                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            OutOfService(IId                                    AuthId,
                         ISendAdminStatus                            ISendAdminStatus,
                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            OutOfService(IId                                    AuthId,
                         IReceiveAdminStatus                         IReceiveAdminStatus,
                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.OutOfService,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region AdminDown

        public static PushChargingStationOperatorAdminStatusResult

            AdminDown(IId                            AuthId,
                      ISendAdminStatus                    ISendAdminStatus,
                      IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedChargingStationOperatorAdminStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            AdminDown(IId                            AuthId,
                      IReceiveAdminStatus                 IReceiveAdminStatus,
                      IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedChargingStationOperatorAdminStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            AdminDown(IId                                       AuthId,
                      ISendAdminStatus                               ISendAdminStatus,
                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            AdminDown(IId                                       AuthId,
                      IReceiveAdminStatus                            IReceiveAdminStatus,
                      IEnumerable<ChargingStationAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            AdminDown(IId                                    AuthId,
                      ISendAdminStatus                            ISendAdminStatus,
                      IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushChargingStationOperatorAdminStatusResult

            AdminDown(IId                                    AuthId,
                      IReceiveAdminStatus                         IReceiveAdminStatus,
                      IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingStationOperatorAdminStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.AdminDown,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Error

        public static PushChargingStationOperatorAdminStatusResult

            Error(IId                            AuthId,
                  ISendAdminStatus                    ISendAdminStatus,
                  IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperators  = null,
                  String                         Description    = null,
                  IEnumerable<Warning>           Warnings       = null,
                  TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.Error,
                                        Description,
                                        RejectedChargingStationOperators,
                                        Warnings,
                                        Runtime);


        public static PushChargingStationOperatorAdminStatusResult Error(IId                            AuthId,
                                                 IReceiveAdminStatus                 IReceiveAdminStatus,
                                                 IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedChargingStationOperators  = null,
                                                 String                         Description    = null,
                                                 IEnumerable<Warning>           Warnings       = null,
                                                 TimeSpan?                      Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        IReceiveAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.Error,
                                        Description,
                                        RejectedChargingStationOperators,
                                        Warnings,
                                        Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingStationOperatorAdminStatusResult LockTimeout(IId                   AuthId,
                                                       ISendAdminStatus           ISendAdminStatus,
                                                       String                Description    = null,
                                                       IEnumerable<Warning>  Warnings       = null,
                                                       TimeSpan?             Runtime        = null)

            => new PushChargingStationOperatorAdminStatusResult(AuthId,
                                        ISendAdminStatus,
                                        PushChargingStationOperatorAdminStatusResultTypes.LockTimeout,
                                        Description,
                                        new ChargingStationOperatorAdminStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion



        public static PushChargingStationOperatorAdminStatusResult Flatten(IId                                     AuthId,
                                                                           ISendAdminStatus                        ISendAdminStatus,
                                                                           IEnumerable<PushChargingStationOperatorAdminStatusResult>  PushChargingStationOperatorAdminStatusResults,
                                                                           TimeSpan                                Runtime)
        {

            #region Initial checks

            if (PushChargingStationOperatorAdminStatusResults == null || !PushChargingStationOperatorAdminStatusResults.Any())
                return new PushChargingStationOperatorAdminStatusResult(AuthId,
                                                     ISendAdminStatus,
                                                     PushChargingStationOperatorAdminStatusResultTypes.Error,
                                                     "!",
                                                     new ChargingStationOperatorAdminStatusUpdate[0],
                                                     new Warning[0],
                                                     Runtime);

            #endregion

            var All                            = PushChargingStationOperatorAdminStatusResults.ToArray();

            var ResultOverview                 = All.GroupBy     (_ => _.Result).
                                                     ToDictionary(_ => _.Key,
                                                                  _ => new List<PushChargingStationOperatorAdminStatusResult>(_));

            var Descriptions                   = All.Where       (_ => _ != null).
                                                     SafeSelect  (_ => _.Description).
                                                     AggregateWith(Environment.NewLine);

            var RejectedChargingStationOperatorAdminStatusUpdates = All.Where       (_ => _ != null).
                                                     SelectMany  (_ => _.RejectedChargingStationOperatorAdminStatusUpdates);

            var Warnings                       = All.Where       (_ => _ != null).
                                                     SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushChargingStationOperatorAdminStatusResult(All[0].AuthId,
                                                         ISendAdminStatus,
                                                         result.Key,
                                                         Descriptions,
                                                         RejectedChargingStationOperatorAdminStatusUpdates,
                                                         Warnings,
                                                         Runtime);

            return new PushChargingStationOperatorAdminStatusResult(All[0].AuthId,
                                                 ISendAdminStatus,
                                                 PushChargingStationOperatorAdminStatusResultTypes.Partial,
                                                 Descriptions,
                                                 RejectedChargingStationOperatorAdminStatusUpdates,
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


    public enum PushChargingStationOperatorAdminStatusResultTypes
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
