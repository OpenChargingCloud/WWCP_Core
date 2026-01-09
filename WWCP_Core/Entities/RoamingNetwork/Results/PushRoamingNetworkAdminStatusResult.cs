/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A PushRoamingNetworkAdminStatus result.
    /// </summary>
    public class PushRoamingNetworkAdminStatusResult
    {

        #region Properties

        /// <summary>
        /// The unique identification of the authenticator.
        /// </summary>
        public IId                                           SenderId                                     { get; }

        /// <summary>
        /// An object implementing ISendAdminStatus.
        /// </summary>
        public ISendAdminStatus?                             ISendAdminStatus                             { get; }

        /// <summary>
        /// An object implementing IReceiveAdminStatus.
        /// </summary>
        public IReceiveAdminStatus?                          IReceiveAdminStatus                          { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushRoamingNetworkAdminStatusResultTypes      Result                                       { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                       Description                                  { get; }

        /// <summary>
        /// An enumeration of rejected roaming network status updates.
        /// </summary>
        public IEnumerable<RoamingNetworkAdminStatusUpdate>  RejectedRoamingNetworkAdminStatusUpdates     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                          Warnings                                     { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                                     Runtime                                      { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushRoamingNetworkAdminStatusResult(SenderId,                      Result, ...)

        /// <summary>
        /// Create a new PushRoamingNetworkAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unique identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedRoamingNetworkAdminStatusUpdates">An enumeration of rejected roaming network status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushRoamingNetworkAdminStatusResult(IId                                            SenderId,
                                                    PushRoamingNetworkAdminStatusResultTypes       Result,
                                                    String?                                        Description                                = null,
                                                    IEnumerable<RoamingNetworkAdminStatusUpdate>?  RejectedRoamingNetworkAdminStatusUpdates   = null,
                                                    IEnumerable<Warning>?                          Warnings                                   = null,
                                                    TimeSpan?                                      Runtime                                    = null)
        {

            this.SenderId                                  = SenderId;
            this.Result                                    = Result;

            this.Description                               = Description is not null && Description.IsNotNullOrEmpty()
                                                                 ? Description.Trim()
                                                                 : String.Empty;

            this.RejectedRoamingNetworkAdminStatusUpdates  = RejectedRoamingNetworkAdminStatusUpdates ?? Array.Empty<RoamingNetworkAdminStatusUpdate>();

            this.Warnings                                  = Warnings is not null
                                                                 ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                                 : Array.Empty<Warning>();

            this.Runtime                                   = Runtime;

        }

        #endregion

        #region (internal) PushRoamingNetworkAdminStatusResult(SenderId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushRoamingNetworkAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unique identification of the sender.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedRoamingNetworkAdminStatusUpdates">An enumeration of rejected roaming network status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushRoamingNetworkAdminStatusResult(IId                                            SenderId,
                                                     ISendAdminStatus                               ISendAdminStatus,
                                                     PushRoamingNetworkAdminStatusResultTypes       Result,
                                                     String?                                        Description                                = null,
                                                     IEnumerable<RoamingNetworkAdminStatusUpdate>?  RejectedRoamingNetworkAdminStatusUpdates   = null,
                                                     IEnumerable<Warning>?                          Warnings                                   = null,
                                                     TimeSpan?                                      Runtime                                    = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedRoamingNetworkAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushRoamingNetworkAdminStatusResult(SenderId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushRoamingNetworkAdminStatus result.
        /// </summary>
        /// <param name="SenderId">The unique identification of the sender.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedRoamingNetworkAdminStatusUpdates">An enumeration of rejected roaming network status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushRoamingNetworkAdminStatusResult(IId                                            SenderId,
                                                     IReceiveAdminStatus                            IReceiveAdminStatus,
                                                     PushRoamingNetworkAdminStatusResultTypes       Result,
                                                     String?                                        Description                                = null,
                                                     IEnumerable<RoamingNetworkAdminStatusUpdate>?  RejectedRoamingNetworkAdminStatusUpdates   = null,
                                                     IEnumerable<Warning>?                          Warnings                                   = null,
                                                     TimeSpan?                                      Runtime                                    = null)

            : this(SenderId,
                   Result,
                   Description,
                   RejectedRoamingNetworkAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushRoamingNetworkAdminStatusResult

            Success(IId                    SenderId,
                    ISendAdminStatus       ISendAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<RoamingNetworkAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushRoamingNetworkAdminStatusResult

            Success(IId                    SenderId,
                    IReceiveAdminStatus    IReceiveAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<RoamingNetworkAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushRoamingNetworkAdminStatusResult

            Enqueued(IId                    SenderId,
                     ISendAdminStatus       ISendAdminStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<RoamingNetworkAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushRoamingNetworkAdminStatusResult

            NoOperation(IId                    SenderId,
                        ISendAdminStatus       ISendAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<RoamingNetworkAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushRoamingNetworkAdminStatusResult

            NoOperation(IId                    SenderId,
                        IReceiveAdminStatus    IReceiveAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<RoamingNetworkAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushRoamingNetworkAdminStatusResult

            OutOfService(IId                                           SenderId,
                         ISendAdminStatus                              ISendAdminStatus,
                         IEnumerable<RoamingNetworkAdminStatusUpdate>  RejectedRoamingNetworkAdminStatusUpdates,
                         String?                                       Description   = null,
                         IEnumerable<Warning>?                         Warnings      = null,
                         TimeSpan?                                     Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedRoamingNetworkAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushRoamingNetworkAdminStatusResult

            OutOfService(IId                                           SenderId,
                         IReceiveAdminStatus                           IReceiveAdminStatus,
                         IEnumerable<RoamingNetworkAdminStatusUpdate>  RejectedRoamingNetworkAdminStatusUpdates,
                         String?                                       Description   = null,
                         IEnumerable<Warning>?                         Warnings      = null,
                         TimeSpan?                                     Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedRoamingNetworkAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushRoamingNetworkAdminStatusResult

            AdminDown(IId                                           SenderId,
                      ISendAdminStatus                              ISendAdminStatus,
                      IEnumerable<RoamingNetworkAdminStatusUpdate>  RejectedRoamingNetworkAdminStatusUpdates,
                      String?                                       Description   = null,
                      IEnumerable<Warning>?                         Warnings      = null,
                      TimeSpan?                                     Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedRoamingNetworkAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushRoamingNetworkAdminStatusResult

            AdminDown(IId                                           SenderId,
                      IReceiveAdminStatus                           IReceiveAdminStatus,
                      IEnumerable<RoamingNetworkAdminStatusUpdate>  RejectedRoamingNetworkAdminStatusUpdates,
                      String?                                       Description   = null,
                      IEnumerable<Warning>?                         Warnings      = null,
                      TimeSpan?                                     Runtime       = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedRoamingNetworkAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushRoamingNetworkAdminStatusResult

            Error(IId                                            SenderId,
                  ISendAdminStatus                               ISendAdminStatus,
                  IEnumerable<RoamingNetworkAdminStatusUpdate>?  RejectedRoamingNetworkAdminStatusUpdates   = null,
                  String?                                        Description                                = null,
                  IEnumerable<Warning>?                          Warnings                                   = null,
                  TimeSpan?                                      Runtime                                    = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.Error,
                    Description,
                    RejectedRoamingNetworkAdminStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushRoamingNetworkAdminStatusResult

            Error(IId                                            SenderId,
                  IReceiveAdminStatus                            IReceiveAdminStatus,
                  IEnumerable<RoamingNetworkAdminStatusUpdate>?  RejectedRoamingNetworkAdminStatusUpdates   = null,
                  String?                                        Description                                = null,
                  IEnumerable<Warning>?                          Warnings                                   = null,
                  TimeSpan?                                      Runtime                                    = null)

            => new (SenderId,
                    IReceiveAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.Error,
                    Description,
                    RejectedRoamingNetworkAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushRoamingNetworkAdminStatusResult

            LockTimeout(IId                                           SenderId,
                        ISendAdminStatus                              ISendAdminStatus,
                        IEnumerable<RoamingNetworkAdminStatusUpdate>  RejectedRoamingNetworkAdminStatusUpdates,
                        String?                                       Description   = null,
                        IEnumerable<Warning>?                         Warnings      = null,
                        TimeSpan?                                     Runtime       = null)

            => new (SenderId,
                    ISendAdminStatus,
                    PushRoamingNetworkAdminStatusResultTypes.LockTimeout,
                    Description,
                    RejectedRoamingNetworkAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(SenderId, ISendAdminStatus, PushRoamingNetworkAdminStatusResults, Runtime)

        public static PushRoamingNetworkAdminStatusResult Flatten(IId                                               SenderId,
                                                                  ISendAdminStatus                                  ISendAdminStatus,
                                                                  IEnumerable<PushRoamingNetworkAdminStatusResult>  PushRoamingNetworkAdminStatusResults,
                                                                  TimeSpan                                          Runtime)
        {

            #region Initial checks

            if (PushRoamingNetworkAdminStatusResults is null || !PushRoamingNetworkAdminStatusResults.Any())
                return new PushRoamingNetworkAdminStatusResult(SenderId,
                                                               ISendAdminStatus,
                                                               PushRoamingNetworkAdminStatusResultTypes.Error,
                                                               "!",
                                                               Array.Empty<RoamingNetworkAdminStatusUpdate>(),
                                                               Array.Empty<Warning>(),
                                                               Runtime);

            #endregion

            var all                                       = PushRoamingNetworkAdminStatusResults.ToArray();

            var resultOverview                            = all.GroupBy      (result => result.Result).
                                                                ToDictionary (result => result.Key,
                                                                              result => new List<PushRoamingNetworkAdminStatusResult>(result));

            var descriptions                              = all.Where        (result => result is not null).
                                                                SafeSelect   (result => result.Description).
                                                                AggregateWith(Environment.NewLine);

            var rejectedRoamingNetworkAdminStatusUpdates  = all.Where        (result => result is not null).
                                                                SelectMany   (result => result.RejectedRoamingNetworkAdminStatusUpdates);

            var warnings                                  = all.Where        (result => result is not null).
                                                                SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushRoamingNetworkAdminStatusResult(all[0].SenderId,
                                                                   ISendAdminStatus,
                                                                   result.Key,
                                                                   descriptions,
                                                                   rejectedRoamingNetworkAdminStatusUpdates,
                                                                   warnings,
                                                                   Runtime);

            return new PushRoamingNetworkAdminStatusResult(all[0].SenderId,
                                                           ISendAdminStatus,
                                                           PushRoamingNetworkAdminStatusResultTypes.Partial,
                                                           descriptions,
                                                           rejectedRoamingNetworkAdminStatusUpdates,
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


    public enum PushRoamingNetworkAdminStatusResultTypes
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
