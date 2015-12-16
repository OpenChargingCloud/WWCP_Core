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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The result of a remote stop operation.
    /// </summary>
    public class RemoteStopResult
    {

        #region Properties

        #region Result

        private readonly RemoteStopResultType _Result;

        /// <summary>
        /// The result of a remote stop operation.
        /// </summary>
        public RemoteStopResultType Result
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
        /// The charging session identification for an invalid remote stop operation.
        /// </summary>
        public ChargingSession_Id SessionId
        {
            get
            {
                return _SessionId;
            }
        }

        #endregion

        #region ErrorMessage

        private readonly String _ErrorMessage;

        /// <summary>
        /// An optional error message.
        /// </summary>
        public String ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (private) RemoteStopResult(Result)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="Result">The result of the remote Stop operation.</param>
        private RemoteStopResult(RemoteStopResultType  Result)
        {

            this._Result        = Result;
            this._ErrorMessage  = null;

        }

        #endregion

        #region (private) RemoteStopResult(SessionId)

        /// <summary>
        /// Create a new remote stop result caused by an invalid session identification.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        private RemoteStopResult(ChargingSession_Id SessionId)
        {

            this._Result        = RemoteStopResultType.InvalidSessionId;
            this._SessionId     = SessionId;
            this._ErrorMessage  = "The session identification is invalid!";

        }

        #endregion

        #region (private) RemoteStopResult(ErrorMessage = null)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="ErrorMessage">A optional error message.</param>
        private RemoteStopResult(String ErrorMessage = null)
        {

            this._Result        = RemoteStopResultType.Error;
            this._ErrorMessage  = ErrorMessage;

        }

        #endregion

        #endregion


        #region (static) Unspecified

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static RemoteStopResult Unspecified
        {
            get
            {
                return new RemoteStopResult(RemoteStopResultType.Unspecified);
            }
        }

        #endregion

        #region (static) UnknownEVSE

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        public static RemoteStopResult UnknownEVSE
        {
            get
            {
                return new RemoteStopResult(RemoteStopResultType.UnknownEVSE);
            }
        }

        #endregion

        #region (static) InvalidSessionId

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        public static RemoteStopResult InvalidSessionId(ChargingSession_Id SessionId)
        {
            return new RemoteStopResult(SessionId);
        }

        #endregion

        #region (static) OutOfService

        /// <summary>
        /// The EVSE is out of service.
        /// </summary>
        public static RemoteStopResult OutOfService
        {
            get
            {
                return new RemoteStopResult(RemoteStopResultType.OutOfService);
            }
        }

        #endregion

        #region (static) Offline

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        public static RemoteStopResult Offline
        {
            get
            {
                return new RemoteStopResult(RemoteStopResultType.Offline);
            }
        }

        #endregion

        #region (static) Success

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        public static RemoteStopResult Success
        {
            get
            {
                return new RemoteStopResult(RemoteStopResultType.Success);
            }
        }

        #endregion

        #region (static) Timeout

        /// <summary>
        /// The remote stop ran into a timeout.
        /// </summary>
        public static RemoteStopResult Timeout
        {
            get
            {
                return new RemoteStopResult(RemoteStopResultType.Timeout);
            }
        }

        #endregion

        #region (static) Error(Message = null)

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        /// <param name="Message">An optional error message.</param>
        public static RemoteStopResult Error(String Message = null)
        {
            return new RemoteStopResult(Message);
        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _Result.ToString();
        }

        #endregion

    }


    /// <summary>
    /// The result types of a remote stop operation.
    /// </summary>
    public enum RemoteStopResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        UnknownEVSE,

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The EVSE is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        Offline,

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        Success,

        /// <summary>
        /// The remote stop ran into a timeout.
        /// </summary>
        Timeout,

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        Error

    }

}
