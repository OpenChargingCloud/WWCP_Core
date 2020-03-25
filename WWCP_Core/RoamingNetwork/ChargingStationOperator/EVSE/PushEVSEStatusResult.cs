/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A PushEVSEStatus result.
    /// </summary>
    public class PushEVSEStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                            AuthId                       { get; }

        /// <summary>
        /// An object implementing ISendStatus.
        /// </summary>
        public ISendStatus                    ISendStatus                  { get; }

        /// <summary>
        /// An object implementing IReceiveStatus.
        /// </summary>
        public IReceiveStatus                 IReceiveStatus               { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushEVSEStatusResultTypes      Result                       { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String                         Description                  { get; }

        /// <summary>
        /// An enumeration of rejected EVSE status updates.
        /// </summary>
        public IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates    { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>           Warnings                     { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                      Runtime                      { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushEVSEStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushEVSEStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEVSEStatusUpdates">An enumeration of rejected EVSE status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushEVSEStatusResult(IId                            AuthId,
                                     PushEVSEStatusResultTypes      Result,
                                     String                         Description                 = null,
                                     IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates   = null,
                                     IEnumerable<Warning>           Warnings                    = null,
                                     TimeSpan?                      Runtime                     = null)
        {

            this.AuthId                     = AuthId;
            this.Result                     = Result;

            this.Description                = Description.IsNotNullOrEmpty()
                                                  ? Description.Trim()
                                                  : null;

            this.RejectedEVSEStatusUpdates  = RejectedEVSEStatusUpdates != null
                                                  ? RejectedEVSEStatusUpdates.Where(evse => evse != null)
                                                  : new EVSEStatusUpdate[0];

            this.Warnings                   = Warnings != null
                                                  ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                  : new Warning[0];

            this.Runtime                    = Runtime;

        }

        #endregion

        #region (internal) PushEVSEStatusResult(AuthId, ISendStatus,    Result, ...)

        /// <summary>
        /// Create a new PushEVSEStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendStatus">An object implementing ISendStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEVSEStatusUpdates">An enumeration of rejected EVSE status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEVSEStatusResult(IId                            AuthId,
                                      ISendStatus                    ISendStatus,
                                      PushEVSEStatusResultTypes      Result,
                                      String                         Description                 = null,
                                      IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates   = null,
                                      IEnumerable<Warning>           Warnings                    = null,
                                      TimeSpan?                      Runtime                     = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedEVSEStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendStatus = ISendStatus;

        }

        #endregion

        #region (internal) PushEVSEStatusResult(AuthId, IReceiveStatus, Result, ...)

        /// <summary>
        /// Create a new PushEVSEStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveStatus">An object implementing IReceiveStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedEVSEStatusUpdates">An enumeration of rejected EVSE status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushEVSEStatusResult(IId                            AuthId,
                                      IReceiveStatus                 IReceiveStatus,
                                      PushEVSEStatusResultTypes      Result,
                                      String                         Description                 = null,
                                      IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates   = null,
                                      IEnumerable<Warning>           Warnings                    = null,
                                      TimeSpan?                      Runtime                     = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedEVSEStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveStatus = IReceiveStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushEVSEStatusResult

            Success(IId                   AuthId,
                    ISendStatus           ISendStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.Success,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            Success(IId                   AuthId,
                    IReceiveStatus        IReceiveStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushEVSEStatusResultTypes.Success,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Enqueued

        public static PushEVSEStatusResult

            Enqueued(IId                   AuthId,
                     ISendStatus           ISendStatus,
                     String                Description    = null,
                     IEnumerable<Warning>  Warnings       = null,
                     TimeSpan?             Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.Enqueued,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region NoOperation

        public static PushEVSEStatusResult

            NoOperation(IId                   AuthId,
                        ISendStatus           ISendStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.NoOperation,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            NoOperation(IId                   AuthId,
                        IReceiveStatus        IReceiveStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushEVSEStatusResultTypes.NoOperation,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region OutOfService

        public static PushEVSEStatusResult

            OutOfService(IId                            AuthId,
                         ISendStatus                    ISendStatus,
                         IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedEVSEStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            OutOfService(IId                            AuthId,
                         IReceiveStatus                 IReceiveStatus,
                         IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushEVSEStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedEVSEStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            OutOfService(IId                                       AuthId,
                         ISendStatus                               ISendStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.OutOfService,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            OutOfService(IId                                       AuthId,
                         IReceiveStatus                            IReceiveStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushEVSEStatusResultTypes.OutOfService,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            OutOfService(IId                                    AuthId,
                         ISendStatus                            ISendStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.OutOfService,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            OutOfService(IId                                    AuthId,
                         IReceiveStatus                         IReceiveStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedEVSEStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushEVSEStatusResultTypes.OutOfService,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region AdminDown

        public static PushEVSEStatusResult

            AdminDown(IId                            AuthId,
                      ISendStatus                    ISendStatus,
                      IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedEVSEStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            AdminDown(IId                            AuthId,
                      IReceiveStatus                 IReceiveStatus,
                      IEnumerable<EVSEStatusUpdate>  RejectedEVSEStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushEVSEStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedEVSEStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            AdminDown(IId                                       AuthId,
                      ISendStatus                               ISendStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedEVSEStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.AdminDown,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            AdminDown(IId                                       AuthId,
                      IReceiveStatus                            IReceiveStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedEVSEStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushEVSEStatusResultTypes.AdminDown,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            AdminDown(IId                                    AuthId,
                      ISendStatus                            ISendStatus,
                      IEnumerable<ChargingPoolStatusUpdate>  RejectedEVSEStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.AdminDown,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushEVSEStatusResult

            AdminDown(IId                                    AuthId,
                      IReceiveStatus                         IReceiveStatus,
                      IEnumerable<ChargingPoolStatusUpdate>  RejectedEVSEStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushEVSEStatusResultTypes.AdminDown,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Error

        public static PushEVSEStatusResult

            Error(IId                            AuthId,
                  ISendStatus                    ISendStatus,
                  IEnumerable<EVSEStatusUpdate>  RejectedEVSEs  = null,
                  String                         Description    = null,
                  IEnumerable<Warning>           Warnings       = null,
                  TimeSpan?                      Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.Error,
                                        Description,
                                        RejectedEVSEs,
                                        Warnings,
                                        Runtime);


        public static PushEVSEStatusResult Error(IId                            AuthId,
                                                 IReceiveStatus                 IReceiveStatus,
                                                 IEnumerable<EVSEStatusUpdate>  RejectedEVSEs  = null,
                                                 String                         Description    = null,
                                                 IEnumerable<Warning>           Warnings       = null,
                                                 TimeSpan?                      Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushEVSEStatusResultTypes.Error,
                                        Description,
                                        RejectedEVSEs,
                                        Warnings,
                                        Runtime);

        #endregion

        #region LockTimeout

        public static PushEVSEStatusResult LockTimeout(IId                   AuthId,
                                                       ISendStatus           ISendStatus,
                                                       String                Description    = null,
                                                       IEnumerable<Warning>  Warnings       = null,
                                                       TimeSpan?             Runtime        = null)

            => new PushEVSEStatusResult(AuthId,
                                        ISendStatus,
                                        PushEVSEStatusResultTypes.LockTimeout,
                                        Description,
                                        new EVSEStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion



        public static PushEVSEStatusResult Flatten(IId                                AuthId,
                                                   ISendStatus                        ISendStatus,
                                                   IEnumerable<PushEVSEStatusResult>  PushEVSEStatusResults,
                                                   TimeSpan                           Runtime)
        {

            #region Initial checks

            if (PushEVSEStatusResults == null || !PushEVSEStatusResults.Any())
                return new PushEVSEStatusResult(AuthId,
                                                ISendStatus,
                                                PushEVSEStatusResultTypes.Error,
                                                "!",
                                                new EVSEStatusUpdate[0],
                                                new Warning[0],
                                                Runtime);

            #endregion

            var All                        = PushEVSEStatusResults.ToArray();

            var ResultOverview             = All.GroupBy     (_ => _.Result).
                                                 ToDictionary(_ => _.Key,
                                                              _ => new List<PushEVSEStatusResult>(_));

            var Descriptions               = All.Where       (_ => _ != null).
                                                 SafeSelect  (_ => _.Description).
                                                 AggregateWith(Environment.NewLine);

            var RejectedEVSEStatusUpdates  = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.RejectedEVSEStatusUpdates);

            var Warnings                   = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushEVSEStatusResult(All[0].AuthId,
                                                    ISendStatus,
                                                    result.Key,
                                                    Descriptions,
                                                    RejectedEVSEStatusUpdates,
                                                    Warnings,
                                                    Runtime);

            return new PushEVSEStatusResult(All[0].AuthId,
                                            ISendStatus,
                                            PushEVSEStatusResultTypes.Partial,
                                            Descriptions,
                                            RejectedEVSEStatusUpdates,
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



    public enum PushEVSEStatusResultTypes
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
