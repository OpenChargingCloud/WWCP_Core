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
    /// A PushRoamingNetworkStatus result.
    /// </summary>
    public class PushRoamingNetworkStatusResult
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
        public PushRoamingNetworkStatusResultTypes      Result                       { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String                         Description                  { get; }

        /// <summary>
        /// An enumeration of rejected RoamingNetwork status updates.
        /// </summary>
        public IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates    { get; }

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

        #region (private)  PushRoamingNetworkStatusResult(AuthId,                 Result, ...)

        /// <summary>
        /// Create a new PushRoamingNetworkStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedRoamingNetworkStatusUpdates">An enumeration of rejected RoamingNetwork status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushRoamingNetworkStatusResult(IId                            AuthId,
                                     PushRoamingNetworkStatusResultTypes      Result,
                                     String                         Description                 = null,
                                     IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates   = null,
                                     IEnumerable<Warning>           Warnings                    = null,
                                     TimeSpan?                      Runtime                     = null)
        {

            this.AuthId                     = AuthId;
            this.Result                     = Result;

            this.Description                = Description.IsNotNullOrEmpty()
                                                  ? Description.Trim()
                                                  : null;

            this.RejectedRoamingNetworkStatusUpdates  = RejectedRoamingNetworkStatusUpdates != null
                                                  ? RejectedRoamingNetworkStatusUpdates.Where(evse => evse != null)
                                                  : new RoamingNetworkStatusUpdate[0];

            this.Warnings                   = Warnings != null
                                                  ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                  : new Warning[0];

            this.Runtime                    = Runtime;

        }

        #endregion

        #region (internal) PushRoamingNetworkStatusResult(AuthId, ISendStatus,    Result, ...)

        /// <summary>
        /// Create a new PushRoamingNetworkStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendStatus">An object implementing ISendStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedRoamingNetworkStatusUpdates">An enumeration of rejected RoamingNetwork status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushRoamingNetworkStatusResult(IId                            AuthId,
                                      ISendStatus                    ISendStatus,
                                      PushRoamingNetworkStatusResultTypes      Result,
                                      String                         Description                 = null,
                                      IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates   = null,
                                      IEnumerable<Warning>           Warnings                    = null,
                                      TimeSpan?                      Runtime                     = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedRoamingNetworkStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendStatus = ISendStatus;

        }

        #endregion

        #region (internal) PushRoamingNetworkStatusResult(AuthId, IReceiveStatus, Result, ...)

        /// <summary>
        /// Create a new PushRoamingNetworkStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveStatus">An object implementing IReceiveStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedRoamingNetworkStatusUpdates">An enumeration of rejected RoamingNetwork status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushRoamingNetworkStatusResult(IId                            AuthId,
                                      IReceiveStatus                 IReceiveStatus,
                                      PushRoamingNetworkStatusResultTypes      Result,
                                      String                         Description                 = null,
                                      IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates   = null,
                                      IEnumerable<Warning>           Warnings                    = null,
                                      TimeSpan?                      Runtime                     = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedRoamingNetworkStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveStatus = IReceiveStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushRoamingNetworkStatusResult

            Success(IId                   AuthId,
                    ISendStatus           ISendStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.Success,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            Success(IId                   AuthId,
                    IReceiveStatus        IReceiveStatus,
                    String                Description    = null,
                    IEnumerable<Warning>  Warnings       = null,
                    TimeSpan?             Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushRoamingNetworkStatusResultTypes.Success,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Enqueued

        public static PushRoamingNetworkStatusResult

            Enqueued(IId                   AuthId,
                     ISendStatus           ISendStatus,
                     String                Description    = null,
                     IEnumerable<Warning>  Warnings       = null,
                     TimeSpan?             Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.Enqueued,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region NoOperation

        public static PushRoamingNetworkStatusResult

            NoOperation(IId                   AuthId,
                        ISendStatus           ISendStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.NoOperation,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            NoOperation(IId                   AuthId,
                        IReceiveStatus        IReceiveStatus,
                        String                Description    = null,
                        IEnumerable<Warning>  Warnings       = null,
                        TimeSpan?             Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushRoamingNetworkStatusResultTypes.NoOperation,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region OutOfService

        public static PushRoamingNetworkStatusResult

            OutOfService(IId                            AuthId,
                         ISendStatus                    ISendStatus,
                         IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedRoamingNetworkStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            OutOfService(IId                            AuthId,
                         IReceiveStatus                 IReceiveStatus,
                         IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                         String                         Description    = null,
                         IEnumerable<Warning>           Warnings       = null,
                         TimeSpan?                      Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushRoamingNetworkStatusResultTypes.OutOfService,
                                        Description,
                                        RejectedRoamingNetworkStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            OutOfService(IId                                       AuthId,
                         ISendStatus                               ISendStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.OutOfService,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            OutOfService(IId                                       AuthId,
                         IReceiveStatus                            IReceiveStatus,
                         IEnumerable<ChargingStationStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                         String                                    Description    = null,
                         IEnumerable<Warning>                      Warnings       = null,
                         TimeSpan?                                 Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushRoamingNetworkStatusResultTypes.OutOfService,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            OutOfService(IId                                    AuthId,
                         ISendStatus                            ISendStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.OutOfService,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            OutOfService(IId                                    AuthId,
                         IReceiveStatus                         IReceiveStatus,
                         IEnumerable<ChargingPoolStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                         String                                 Description    = null,
                         IEnumerable<Warning>                   Warnings       = null,
                         TimeSpan?                              Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushRoamingNetworkStatusResultTypes.OutOfService,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion

        #region AdminDown

        public static PushRoamingNetworkStatusResult

            AdminDown(IId                            AuthId,
                      ISendStatus                    ISendStatus,
                      IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedRoamingNetworkStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            AdminDown(IId                            AuthId,
                      IReceiveStatus                 IReceiveStatus,
                      IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                      String                         Description    = null,
                      IEnumerable<Warning>           Warnings       = null,
                      TimeSpan?                      Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushRoamingNetworkStatusResultTypes.AdminDown,
                                        Description,
                                        RejectedRoamingNetworkStatusUpdates,
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            AdminDown(IId                                       AuthId,
                      ISendStatus                               ISendStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.AdminDown,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            AdminDown(IId                                       AuthId,
                      IReceiveStatus                            IReceiveStatus,
                      IEnumerable<ChargingStationStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                      String                                    Description    = null,
                      IEnumerable<Warning>                      Warnings       = null,
                      TimeSpan?                                 Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushRoamingNetworkStatusResultTypes.AdminDown,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            AdminDown(IId                                    AuthId,
                      ISendStatus                            ISendStatus,
                      IEnumerable<ChargingPoolStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.AdminDown,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);



        public static PushRoamingNetworkStatusResult

            AdminDown(IId                                    AuthId,
                      IReceiveStatus                         IReceiveStatus,
                      IEnumerable<ChargingPoolStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                      String                                 Description    = null,
                      IEnumerable<Warning>                   Warnings       = null,
                      TimeSpan?                              Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushRoamingNetworkStatusResultTypes.AdminDown,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion


        #region Error

        public static PushRoamingNetworkStatusResult

            Error(IId                            AuthId,
                  ISendStatus                    ISendStatus,
                  IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworks  = null,
                  String                         Description    = null,
                  IEnumerable<Warning>           Warnings       = null,
                  TimeSpan?                      Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.Error,
                                        Description,
                                        RejectedRoamingNetworks,
                                        Warnings,
                                        Runtime);


        public static PushRoamingNetworkStatusResult Error(IId                            AuthId,
                                                 IReceiveStatus                 IReceiveStatus,
                                                 IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworks  = null,
                                                 String                         Description    = null,
                                                 IEnumerable<Warning>           Warnings       = null,
                                                 TimeSpan?                      Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        IReceiveStatus,
                                        PushRoamingNetworkStatusResultTypes.Error,
                                        Description,
                                        RejectedRoamingNetworks,
                                        Warnings,
                                        Runtime);

        #endregion

        #region LockTimeout

        public static PushRoamingNetworkStatusResult LockTimeout(IId                   AuthId,
                                                       ISendStatus           ISendStatus,
                                                       String                Description    = null,
                                                       IEnumerable<Warning>  Warnings       = null,
                                                       TimeSpan?             Runtime        = null)

            => new PushRoamingNetworkStatusResult(AuthId,
                                        ISendStatus,
                                        PushRoamingNetworkStatusResultTypes.LockTimeout,
                                        Description,
                                        new RoamingNetworkStatusUpdate[0],
                                        Warnings,
                                        Runtime);

        #endregion



        public static PushRoamingNetworkStatusResult Flatten(IId                                AuthId,
                                                   ISendStatus                        ISendStatus,
                                                   IEnumerable<PushRoamingNetworkStatusResult>  PushRoamingNetworkStatusResults,
                                                   TimeSpan                           Runtime)
        {

            #region Initial checks

            if (PushRoamingNetworkStatusResults == null || !PushRoamingNetworkStatusResults.Any())
                return new PushRoamingNetworkStatusResult(AuthId,
                                                ISendStatus,
                                                PushRoamingNetworkStatusResultTypes.Error,
                                                "!",
                                                new RoamingNetworkStatusUpdate[0],
                                                new Warning[0],
                                                Runtime);

            #endregion

            var All                        = PushRoamingNetworkStatusResults.ToArray();

            var ResultOverview             = All.GroupBy     (_ => _.Result).
                                                 ToDictionary(_ => _.Key,
                                                              _ => new List<PushRoamingNetworkStatusResult>(_));

            var Descriptions               = All.Where       (_ => _ != null).
                                                 SafeSelect  (_ => _.Description).
                                                 AggregateWith(Environment.NewLine);

            var RejectedRoamingNetworkStatusUpdates  = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.RejectedRoamingNetworkStatusUpdates);

            var Warnings                   = All.Where       (_ => _ != null).
                                                 SelectMany  (_ => _.Warnings);


            foreach (var result in ResultOverview)
                if (ResultOverview[result.Key].Count == All.Length)
                    return new PushRoamingNetworkStatusResult(All[0].AuthId,
                                                    ISendStatus,
                                                    result.Key,
                                                    Descriptions,
                                                    RejectedRoamingNetworkStatusUpdates,
                                                    Warnings,
                                                    Runtime);

            return new PushRoamingNetworkStatusResult(All[0].AuthId,
                                            ISendStatus,
                                            PushRoamingNetworkStatusResultTypes.Partial,
                                            Descriptions,
                                            RejectedRoamingNetworkStatusUpdates,
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



    public enum PushRoamingNetworkStatusResultTypes
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
