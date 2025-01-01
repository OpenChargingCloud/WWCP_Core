/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    public class PushAuthenticationDataResult2
    {

        public ChargeDetailRecord   ChargeDetailRecord    { get; }
        public SendCDRResultTypes   Result                { get; }
        public IEnumerable<String>  Warnings              { get; }

        public PushAuthenticationDataResult2(ChargeDetailRecord   ChargeDetailRecord,
                               SendCDRResultTypes   Result,
                               IEnumerable<String>  Warnings)
        {

            this.ChargeDetailRecord  = ChargeDetailRecord;
            this.Result              = Result;
            this.Warnings            = Warnings != null
                                           ? Warnings.Where     (warning => warning != null).
                                                      SafeSelect(warning => warning.Trim()).
                                                      Where     (warning => warning.IsNotNullOrEmpty())
                                           : new String[0];

        }

    }


    /// <summary>
    /// A PushData result.
    /// </summary>
    public class PushAuthenticationDataResult
    {

        #region Properties

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushAuthenticationDataResultTypes  Result          { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String               Description     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<String>  Warnings        { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?            Runtime         { get;  }

        #endregion

        #region Constructor(s)

        #region (private) PushAuthenticationDataResult(SenderId, ISendAuthenticationData,    Result,...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushAuthenticationDataResult(IId                                SenderId,
                                             ISendAuthenticationData            ISendAuthenticationData,
                                             PushAuthenticationDataResultTypes  Result,
                                             IEnumerable<EVSE>?                 RejectedEVSEs  = null,
                                             String?                            Description    = null,
                                             IEnumerable<String>?               Warnings       = null,
                                             TimeSpan?                          Runtime        = null)
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

        #region (private) PushAuthenticationDataResult(SenderId, IReceiveAuthenticationData, Result,...)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushAuthenticationDataResult(IId                                SenderId,
                                             IReceiveAuthenticationData         IReceiveAuthenticationData,
                                             PushAuthenticationDataResultTypes  Result,
                                             IEnumerable<EVSE>?                 RejectedEVSEs  = null,
                                             String?                            Description    = null,
                                             IEnumerable<String>?               Warnings       = null,
                                             TimeSpan?                          Runtime        = null)
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


        #region AdminDown

        public static PushAuthenticationDataResult AdminDown(IId                      SenderId,
                                                             ISendAuthenticationData  ISendAuthenticationData,
                                                             IEnumerable<EVSE>        RejectedEVSEs  = null,
                                                             String                   Description    = null,
                                                             IEnumerable<String>      Warnings       = null,
                                                             TimeSpan?                Runtime        = null)

            => new (SenderId,
                    ISendAuthenticationData,
                    PushAuthenticationDataResultTypes.AdminDown,
                    RejectedEVSEs,
                    Description,
                    Warnings,
                    Runtime);


        public static PushAuthenticationDataResult AdminDown(IId                         SenderId,
                                                             IReceiveAuthenticationData  IReceiveAuthenticationData,
                                                             IEnumerable<EVSE>           RejectedEVSEs  = null,
                                                             String                      Description    = null,
                                                             IEnumerable<String>         Warnings       = null,
                                                             TimeSpan?                   Runtime        = null)

            => new (SenderId,
                    IReceiveAuthenticationData,
                    PushAuthenticationDataResultTypes.AdminDown,
                    RejectedEVSEs,
                    Description,
                    Warnings,
                    Runtime);

        #endregion

        #region Success

        public static PushAuthenticationDataResult Success(IId                      SenderId,
                                                           ISendAuthenticationData  ISendAuthenticationData,
                                                           String                   Description   = null,
                                                           IEnumerable<String>      Warnings      = null,
                                                           TimeSpan?                Runtime       = null)

            => new (SenderId,
                    ISendAuthenticationData,
                    PushAuthenticationDataResultTypes.Success,
                    Array.Empty<EVSE>(),
                    Description,
                    Warnings,
                    Runtime);


        public static PushAuthenticationDataResult Success(IId                         SenderId,
                                                           IReceiveAuthenticationData  IReceiveAuthenticationData,
                                                           String                      Description   = null,
                                                           IEnumerable<String>         Warnings      = null,
                                                           TimeSpan?                   Runtime       = null)

            => new (SenderId,
                    IReceiveAuthenticationData,
                    PushAuthenticationDataResultTypes.Success,
                    Array.Empty<EVSE>(),
                    Description,
                    Warnings,
                    Runtime);

        #endregion

        #region Enqueued

        public static PushAuthenticationDataResult Enqueued(IId                      SenderId,
                                                            ISendAuthenticationData  ISendAuthenticationData,
                                                            String?                  Description   = null,
                                                            IEnumerable<String>?     Warnings      = null,
                                                            TimeSpan?                Runtime       = null)

            => new (SenderId,
                    ISendAuthenticationData,
                    PushAuthenticationDataResultTypes.Enqueued,
                    Array.Empty<EVSE>(),
                    Description,
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushAuthenticationDataResult NoOperation(IId                      SenderId,
                                                               ISendAuthenticationData  ISendAuthenticationData,
                                                               String?                  Description    = null,
                                                               IEnumerable<String>?     Warnings       = null,
                                                               TimeSpan?                Runtime        = null)

            => new (SenderId,
                    ISendAuthenticationData,
                    PushAuthenticationDataResultTypes.NoOperation,
                    Array.Empty<EVSE>(),
                    Description,
                    Warnings,
                    Runtime);


         public static PushAuthenticationDataResult NoOperation(IId                         SenderId,
                                                                IReceiveAuthenticationData  IReceiveAuthenticationData,
                                                                String?                     Description    = null,
                                                                IEnumerable<String>?        Warnings       = null,
                                                                TimeSpan?                   Runtime        = null)

            => new (SenderId,
                    IReceiveAuthenticationData,
                    PushAuthenticationDataResultTypes.NoOperation,
                    Array.Empty<EVSE>(),
                    Description,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushAuthenticationDataResult Error(IId                      SenderId,
                                                         ISendAuthenticationData  ISendAuthenticationData,
                                                         IEnumerable<EVSE>        RejectedEVSEs  = null,
                                                         String                   Description    = null,
                                                         IEnumerable<String>      Warnings       = null,
                                                         TimeSpan?                Runtime        = null)

            => new (SenderId,
                    ISendAuthenticationData,
                    PushAuthenticationDataResultTypes.Error,
                    RejectedEVSEs,
                    Description,
                    Warnings,
                    Runtime);


        public static PushAuthenticationDataResult Error(IId                         SenderId,
                                                         IReceiveAuthenticationData  IReceiveAuthenticationData,
                                                         IEnumerable<EVSE>           RejectedEVSEs  = null,
                                                         String                      Description    = null,
                                                         IEnumerable<String>         Warnings       = null,
                                                         TimeSpan?                   Runtime        = null)

            => new (SenderId,
                    IReceiveAuthenticationData,
                    PushAuthenticationDataResultTypes.Error,
                    RejectedEVSEs,
                    Description,
                    Warnings,
                    Runtime);

        #endregion



        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }


    public enum PushAuthenticationDataResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        /// <summary>
        /// No operation e.g. because no EVSE data passed the EVSE filter.
        /// </summary>
        NoOperation,

        /// <summary>
        /// The charge detail records had been enqueued for later transmission.
        /// </summary>
        Enqueued,

        Success,

        OutOfService,

        /// <summary>
        /// A timeout occured.
        /// </summary>
        Timeout,

        /// <summary>
        /// The operation led to an error.
        /// </summary>
        Error

    }

    public enum PushSingleAuthenticationDataResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        True,
        NoOperation,
        Enqueued,
        LockTimeout,
        False

    }

}
