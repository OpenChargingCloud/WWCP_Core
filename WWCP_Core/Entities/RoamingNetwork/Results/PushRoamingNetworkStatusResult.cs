/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// A PushRoamingNetworkStatus result.
    /// </summary>
    public class PushRoamingNetworkStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                      SenderId                                  { get; }

        /// <summary>
        /// An object implementing ISendStatus.
        /// </summary>
        public ISendStatus?                             ISendStatus                             { get; }

        /// <summary>
        /// An object implementing IReceiveStatus.
        /// </summary>
        public IReceiveStatus?                          IReceiveStatus                          { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushRoamingNetworkStatusResultTypes      Result                                  { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                  Description                             { get; }

        /// <summary>
        /// An enumeration of rejected roaming network admin status updates.
        /// </summary>
        public IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                     Warnings                                { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                                Runtime                                 { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushRoamingNetworkStatusResult(SenderId,                 Result, ...)

        /// <summary>
        /// Create a new PushRoamingNetworkStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedRoamingNetworkStatusUpdates">An enumeration of rejected roaming network admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushRoamingNetworkStatusResult(IId                                       SenderId,
                                               PushRoamingNetworkStatusResultTypes       Result,
                                               String?                                   Description                           = null,
                                               IEnumerable<RoamingNetworkStatusUpdate>?  RejectedRoamingNetworkStatusUpdates   = null,
                                               IEnumerable<Warning>?                     Warnings                              = null,
                                               TimeSpan?                                 Runtime                               = null)
        {

            this.SenderId                             = SenderId;
            this.Result                               = Result;

            this.Description                          = Description is not null && Description.IsNotNullOrEmpty()
                                                            ? Description.Trim()
                                                            : String.Empty;

            this.RejectedRoamingNetworkStatusUpdates  = RejectedRoamingNetworkStatusUpdates ?? Array.Empty<RoamingNetworkStatusUpdate>();

            this.Warnings                             = Warnings is not null
                                                            ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                            : Array.Empty<Warning>();

            this.Runtime                              = Runtime;

        }

        #endregion

        #region (internal) PushRoamingNetworkStatusResult(SenderId, ISendStatus,    Result, ...)

        /// <summary>
        /// Create a new PushRoamingNetworkStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="ISendStatus">An object implementing ISendStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedRoamingNetworkStatusUpdates">An enumeration of rejected roaming network admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushRoamingNetworkStatusResult(IId                                       SenderId,
                                                ISendStatus                               ISendStatus,
                                                PushRoamingNetworkStatusResultTypes       Result,
                                                String?                                   Description                           = null,
                                                IEnumerable<RoamingNetworkStatusUpdate>?  RejectedRoamingNetworkStatusUpdates   = null,
                                                IEnumerable<Warning>?                     Warnings                              = null,
                                                TimeSpan?                                 Runtime                               = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedRoamingNetworkStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendStatus = ISendStatus;

        }

        #endregion

        #region (internal) PushRoamingNetworkStatusResult(SenderId, IReceiveStatus, Result, ...)

        /// <summary>
        /// Create a new PushRoamingNetworkStatus result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="IReceiveStatus">An object implementing IReceiveStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedRoamingNetworkStatusUpdates">An enumeration of rejected roaming network admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushRoamingNetworkStatusResult(IId                                       SenderId,
                                                IReceiveStatus                            IReceiveStatus,
                                                PushRoamingNetworkStatusResultTypes       Result,
                                                String?                                   Description                           = null,
                                                IEnumerable<RoamingNetworkStatusUpdate>?  RejectedRoamingNetworkStatusUpdates   = null,
                                                IEnumerable<Warning>?                     Warnings                              = null,
                                                TimeSpan?                                 Runtime                               = null)

            : this(SenderId,
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

            Success(IId                    SenderId,
                    ISendStatus            ISendStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushRoamingNetworkStatusResultTypes.Success,
                    Description,
                    Array.Empty<RoamingNetworkStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushRoamingNetworkStatusResult

            Success(IId                    SenderId,
                    IReceiveStatus         IReceiveStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushRoamingNetworkStatusResultTypes.Success,
                    Description,
                    Array.Empty<RoamingNetworkStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushRoamingNetworkStatusResult

            Enqueued(IId                    SenderId,
                     ISendStatus            ISendStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushRoamingNetworkStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<RoamingNetworkStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushRoamingNetworkStatusResult

            NoOperation(IId                    SenderId,
                        ISendStatus            ISendStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushRoamingNetworkStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<RoamingNetworkStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushRoamingNetworkStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveStatus         IReceiveStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushRoamingNetworkStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<RoamingNetworkStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushRoamingNetworkStatusResult

            OutOfService(IId                                      SenderId,
                         ISendStatus                              ISendStatus,
                         IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                         String?                                  Description   = null,
                         IEnumerable<Warning>?                    Warnings      = null,
                         TimeSpan?                                Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushRoamingNetworkStatusResultTypes.OutOfService,
                    Description,
                    RejectedRoamingNetworkStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushRoamingNetworkStatusResult

            OutOfService(IId                                      SenderId,
                         IReceiveStatus                           IReceiveStatus,
                         IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                         String?                                  Description   = null,
                         IEnumerable<Warning>?                    Warnings      = null,
                         TimeSpan?                                Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushRoamingNetworkStatusResultTypes.OutOfService,
                    Description,
                    RejectedRoamingNetworkStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushRoamingNetworkStatusResult

            AdminDown(IId                                      SenderId,
                      ISendStatus                              ISendStatus,
                      IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                      String?                                  Description   = null,
                      IEnumerable<Warning>?                    Warnings      = null,
                      TimeSpan?                                Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushRoamingNetworkStatusResultTypes.AdminDown,
                    Description,
                    RejectedRoamingNetworkStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushRoamingNetworkStatusResult

            AdminDown(IId                                      SenderId,
                      IReceiveStatus                           IReceiveStatus,
                      IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                      String?                                  Description   = null,
                      IEnumerable<Warning>?                    Warnings      = null,
                      TimeSpan?                                Runtime       = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushRoamingNetworkStatusResultTypes.AdminDown,
                    Description,
                    RejectedRoamingNetworkStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushRoamingNetworkStatusResult

            Error(IId                                       SenderId,
                  ISendStatus                               ISendStatus,
                  IEnumerable<RoamingNetworkStatusUpdate>?  RejectedRoamingNetworkStatusUpdates   = null,
                  String?                                   Description                           = null,
                  IEnumerable<Warning>?                     Warnings                              = null,
                  TimeSpan?                                 Runtime                               = null)

            => new (SenderId,
                    ISendStatus,
                    PushRoamingNetworkStatusResultTypes.Error,
                    Description,
                    RejectedRoamingNetworkStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushRoamingNetworkStatusResult

            Error(IId                                       SenderId,
                  IReceiveStatus                            IReceiveStatus,
                  IEnumerable<RoamingNetworkStatusUpdate>?  RejectedRoamingNetworkStatusUpdates   = null,
                  String?                                   Description                           = null,
                  IEnumerable<Warning>?                     Warnings                              = null,
                  TimeSpan?                                 Runtime                               = null)

            => new (SenderId,
                    IReceiveStatus,
                    PushRoamingNetworkStatusResultTypes.Error,
                    Description,
                    RejectedRoamingNetworkStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushRoamingNetworkStatusResult

            LockTimeout(IId                                      SenderId,
                        ISendStatus                              ISendStatus,
                        IEnumerable<RoamingNetworkStatusUpdate>  RejectedRoamingNetworkStatusUpdates,
                        String?                                  Description   = null,
                        IEnumerable<Warning>?                    Warnings      = null,
                        TimeSpan?                                Runtime       = null)

            => new (SenderId,
                    ISendStatus,
                    PushRoamingNetworkStatusResultTypes.LockTimeout,
                    Description,
                    RejectedRoamingNetworkStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(SenderId, ISendStatus, PushRoamingNetworkStatusResults, Runtime)

        public static PushRoamingNetworkStatusResult Flatten(IId                                          SenderId,
                                                             ISendStatus                                  ISendStatus,
                                                             IEnumerable<PushRoamingNetworkStatusResult>  PushRoamingNetworkStatusResults,
                                                             TimeSpan                                     Runtime)
        {

            #region Initial checks

            if (PushRoamingNetworkStatusResults is null || !PushRoamingNetworkStatusResults.Any())
                return new PushRoamingNetworkStatusResult(SenderId,
                                                          ISendStatus,
                                                          PushRoamingNetworkStatusResultTypes.Error,
                                                          "!",
                                                          Array.Empty<RoamingNetworkStatusUpdate>(),
                                                          Array.Empty<Warning>(),
                                                          Runtime);

            #endregion

            var all                                  = PushRoamingNetworkStatusResults.ToArray();

            var resultOverview                       = all.GroupBy      (result => result.Result).
                                                           ToDictionary (result => result.Key,
                                                                         result => new List<PushRoamingNetworkStatusResult>(result));

            var descriptions                         = all.Where        (result => result is not null).
                                                           SafeSelect   (result => result.Description).
                                                           AggregateWith(Environment.NewLine);

            var rejectedRoamingNetworkStatusUpdates  = all.Where        (result => result is not null).
                                                           SelectMany   (result => result.RejectedRoamingNetworkStatusUpdates);

            var warnings                             = all.Where        (result => result is not null).
                                                           SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushRoamingNetworkStatusResult(all[0].SenderId,
                                                              ISendStatus,
                                                              result.Key,
                                                              descriptions,
                                                              rejectedRoamingNetworkStatusUpdates,
                                                              warnings,
                                                              Runtime);

            return new PushRoamingNetworkStatusResult(all[0].SenderId,
                                                      ISendStatus,
                                                      PushRoamingNetworkStatusResultTypes.Partial,
                                                      descriptions,
                                                      rejectedRoamingNetworkStatusUpdates,
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
