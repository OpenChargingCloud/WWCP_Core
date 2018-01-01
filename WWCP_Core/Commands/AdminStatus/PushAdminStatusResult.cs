/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A PushAdminStatus result.
    /// </summary>
    public class PushAdminStatusResult
    {

        #region Properties

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushAdminStatusResultTypes  Result          { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String                      Description     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<String>         Warnings        { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                   Runtime         { get;  }

        #endregion

        #region Constructor(s)

        #region Constructor(s)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public PushAdminStatusResult(IId                                 AuthId,
                                     ISendAdminStatus                    ISendAdminStatus,
                                     PushAdminStatusResultTypes          Result,
                                     IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                     String                              Description  = null,
                                     IEnumerable<String>                 Warnings     = null,
                                     TimeSpan?                           Runtime      = null)
        {

            this.Result       = Result;

            this.Description  = Description.IsNotNullOrEmpty()
                                    ? Description.Trim()
                                    : null;

            this.Warnings     = Warnings != null
                                    ? Warnings.Where     (warning => warning != null).
                                               SafeSelect(warning => warning.Trim()).
                                               Where     (warning => warning.IsNotNullOrEmpty())
                                    : new String[0];

            this.Runtime      = Runtime;

        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public PushAdminStatusResult(IId                                 AuthId,
                                     IReceiveAdminStatus                 IReceiveAdminStatus,
                                     PushAdminStatusResultTypes          Result,
                                     IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                     String                              Description  = null,
                                     IEnumerable<String>                 Warnings     = null,
                                     TimeSpan?                           Runtime      = null)
        {

            this.Result       = Result;

            this.Description  = Description.IsNotNullOrEmpty()
                                    ? Description.Trim()
                                    : null;

            this.Warnings     = Warnings != null
                                    ? Warnings.Where     (warning => warning != null).
                                               SafeSelect(warning => warning.Trim()).
                                               Where     (warning => warning.IsNotNullOrEmpty())
                                    : new String[0];

            this.Runtime      = Runtime;

        }

        #endregion

        #endregion


        #region OutOfService

        public static PushAdminStatusResult NoOperation(IId                                 AuthId,
                                                        ISendAdminStatus                    ISendAdminStatus,
                                                        //IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                        String                              Description    = null,
                                                        IEnumerable<String>                 Warnings       = null,
                                                        TimeSpan?                           Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         ISendAdminStatus,
                                         PushAdminStatusResultTypes.NoOperation,
                                         new EVSEAdminStatusUpdate[0],
                                         Description,
                                         Warnings,
                                         Runtime);



        public static PushAdminStatusResult NoOperation(IId                                 AuthId,
                                                        IReceiveAdminStatus                 IReceiveAdminStatus,
                                                        //IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                        String                              Description    = null,
                                                        IEnumerable<String>                 Warnings       = null,
                                                        TimeSpan?                           Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         IReceiveAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         new EVSEAdminStatusUpdate[0],
                                         Description,
                                         Warnings,
                                         Runtime);

        #endregion
        #region OutOfService

        public static PushAdminStatusResult OutOfService(IId                                 AuthId,
                                                         ISendAdminStatus                    ISendAdminStatus,
                                                         IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                         String                              Description    = null,
                                                         IEnumerable<String>                 Warnings       = null,
                                                         TimeSpan?                           Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         ISendAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         RejectedEVSEStatusChanges,
                                         Description,
                                         Warnings,
                                         Runtime);



        public static PushAdminStatusResult OutOfService(IId                                 AuthId,
                                                         IReceiveAdminStatus                 IReceiveAdminStatus,
                                                         IEnumerable<EVSEAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                         String                              Description    = null,
                                                         IEnumerable<String>                 Warnings       = null,
                                                         TimeSpan?                           Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         IReceiveAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         RejectedEVSEStatusChanges,
                                         Description,
                                         Warnings,
                                         Runtime);



        public static PushAdminStatusResult OutOfService(IId                                            AuthId,
                                                         ISendAdminStatus                               ISendAdminStatus,
                                                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                         String                                         Description    = null,
                                                         IEnumerable<String>                            Warnings       = null,
                                                         TimeSpan?                                      Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         ISendAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         new EVSEAdminStatusUpdate[0],
                                         Description,
                                         Warnings,
                                         Runtime);



        public static PushAdminStatusResult OutOfService(IId                                            AuthId,
                                                         IReceiveAdminStatus                            IReceiveAdminStatus,
                                                         IEnumerable<ChargingStationAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                         String                                         Description    = null,
                                                         IEnumerable<String>                            Warnings       = null,
                                                         TimeSpan?                                      Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         IReceiveAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         new EVSEAdminStatusUpdate[0],
                                         Description,
                                         Warnings,
                                         Runtime);



        public static PushAdminStatusResult OutOfService(IId                            AuthId,
                                                         ISendAdminStatus               ISendAdminStatus,
                                                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                         String                         Description    = null,
                                                         IEnumerable<String>            Warnings       = null,
                                                         TimeSpan?                      Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         ISendAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         new EVSEAdminStatusUpdate[0],
                                         Description,
                                         Warnings,
                                         Runtime);



        public static PushAdminStatusResult OutOfService(IId                                         AuthId,
                                                         IReceiveAdminStatus                         IReceiveAdminStatus,
                                                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                         String                                      Description    = null,
                                                         IEnumerable<String>                         Warnings       = null,
                                                         TimeSpan?                                   Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         IReceiveAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         new EVSEAdminStatusUpdate[0],
                                         Description,
                                         Warnings,
                                         Runtime);



                public static PushAdminStatusResult OutOfService(IId                                            AuthId,
                                                         ISendAdminStatus                               ISendAdminStatus,
                                                         IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                         String                                         Description    = null,
                                                         IEnumerable<String>                            Warnings       = null,
                                                         TimeSpan?                                      Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         ISendAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         new EVSEAdminStatusUpdate[0],
                                         Description,
                                         Warnings,
                                         Runtime);



        public static PushAdminStatusResult OutOfService(IId                                            AuthId,
                                                         IReceiveAdminStatus                            IReceiveAdminStatus,
                                                         IEnumerable<ChargingStationOperatorAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                         String                                         Description    = null,
                                                         IEnumerable<String>                            Warnings       = null,
                                                         TimeSpan?                                      Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         IReceiveAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         new EVSEAdminStatusUpdate[0],
                                         Description,
                                         Warnings,
                                         Runtime);



        public static PushAdminStatusResult OutOfService(IId                            AuthId,
                                                         ISendAdminStatus               ISendAdminStatus,
                                                         IEnumerable<RoamingNetworkAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                         String                         Description    = null,
                                                         IEnumerable<String>            Warnings       = null,
                                                         TimeSpan?                      Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         ISendAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         new EVSEAdminStatusUpdate[0],
                                         Description,
                                         Warnings,
                                         Runtime);



        public static PushAdminStatusResult OutOfService(IId                                         AuthId,
                                                         IReceiveAdminStatus                         IReceiveAdminStatus,
                                                         IEnumerable<RoamingNetworkAdminStatusUpdate>  RejectedEVSEStatusChanges,
                                                         String                                      Description    = null,
                                                         IEnumerable<String>                         Warnings       = null,
                                                         TimeSpan?                                   Runtime        = null)

            => new PushAdminStatusResult(AuthId,
                                         IReceiveAdminStatus,
                                         PushAdminStatusResultTypes.OutOfService,
                                         new EVSEAdminStatusUpdate[0],
                                         Description,
                                         Warnings,
                                         Runtime);
        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }


    public enum PushAdminStatusResultTypes
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

        True,
        NoOperation,
        Enqueued,
        LockTimeout,
        False

    }

}
