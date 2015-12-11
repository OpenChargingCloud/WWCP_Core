/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

#endregion

namespace org.GraphDefined.WWCP.LocalService
{

    /// <summary>
    /// The result of a remote start operation.
    /// </summary>
    public class RemoteStartResult
    {

        #region Properties

        #region Result

        private readonly RemoteStartResultType _Result;

        /// <summary>
        /// The result of a remote start operation.
        /// </summary>
        public RemoteStartResultType Result
        {
            get
            {
                return _Result;
            }
        }

        #endregion

        #region SessionId

        private readonly ChargingSession_Id _SessionId;

        /// <summary>
        /// The charging session identification for the remote start operation.
        /// </summary>
        public ChargingSession_Id SessionId
        {
            get
            {
                return _SessionId;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new remote start result.
        /// </summary>
        /// <param name="Result">The result of the remote start operation.</param>
        /// <param name="SessionId">An optional unique charging session identification (mandatory for successful session starts).</param>
        public RemoteStartResult(RemoteStartResultType  Result,
                                 ChargingSession_Id     SessionId = null)
        {

            this._Result     = Result;
            this._SessionId  = SessionId;

        }

        #endregion


        #region (static) Unknown

        /// <summary>
        /// Create a new remote start 'Unknown' result.
        /// </summary>
        public static RemoteStartResult Unknown
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.Unknown);
            }
        }

        #endregion

        #region (static) Success(SessionId)

        /// <summary>
        /// Create a new successful remote start result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStartResult Success(ChargingSession_Id SessionId)
        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException("SessionId", "The given parameter must not be null!");

            #endregion

            return new RemoteStartResult(RemoteStartResultType.Success, SessionId);

        }

        #endregion

        #region (static) Error

        /// <summary>
        /// Create a new remote start 'Error' result.
        /// </summary>
        public static RemoteStartResult Error
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.Error);
            }
        }

        #endregion

        #region (static) SessionId_AlreadyInUse

        /// <summary>
        /// Create a new remote start 'SessionId_AlreadyInUse' result.
        /// </summary>
        public static RemoteStartResult SessionId_AlreadyInUse
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.SessionId_AlreadyInUse);
            }
        }

        #endregion

        #region (static) EVSE_AlreadyInUse

        /// <summary>
        /// Create a new remote start 'EVSE_AlreadyInUse' result.
        /// </summary>
        public static RemoteStartResult EVSE_AlreadyInUse
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.EVSE_AlreadyInUse);
            }
        }

        #endregion

        #region (static) UnknownEVSE

        /// <summary>
        /// Create a new remote start 'UnknownEVSE' result.
        /// </summary>
        public static RemoteStartResult UnknownEVSE
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.UnknownEVSE);
            }
        }

        #endregion

        #region (static) EVSEReserved

        /// <summary>
        /// Create a new remote start 'EVSEReserved' result.
        /// </summary>
        public static RemoteStartResult EVSEReserved
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.EVSEReserved);
            }
        }

        #endregion

        #region (static) EVSEOutOfService

        /// <summary>
        /// Create a new remote start 'EVSEOutOfService' result.
        /// </summary>
        public static RemoteStartResult EVSEOutOfService
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.EVSEOutOfService);
            }
        }

        #endregion

        #region (static) EVSE_NotReachable

        /// <summary>
        /// Create a new remote start 'EVSE_NotReachable' result.
        /// </summary>
        public static RemoteStartResult EVSE_NotReachable
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.EVSE_NotReachable);
            }
        }

        #endregion

        #region (static) Start_Timeout

        /// <summary>
        /// Create a new remote start 'Start_Timeout' result.
        /// </summary>
        public static RemoteStartResult Start_Timeout
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.Start_Timeout);
            }
        }

        #endregion

    }


    /// <summary>
    /// The result types of a remote start operation.
    /// </summary>
    public enum RemoteStartResultType
    {

        /// <summary>
        /// The result is unknown or should be ignored.
        /// </summary>
        Unknown,

        Success,
        Error,
        SessionId_AlreadyInUse,
        EVSE_AlreadyInUse,
        UnknownEVSE,
        EVSEReserved,
        EVSEOutOfService,
        EVSE_NotReachable,
        Start_Timeout

    }

}
