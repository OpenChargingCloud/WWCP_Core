/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of a remote stop operation at a charging station.
    /// </summary>
    public class RemoteStopChargingStationResult
    {

        #region Properties

        #region Result

        private readonly RemoteStopChargingStationResultType _Result;

        /// <summary>
        /// The result of a remote stop operation.
        /// </summary>
        public RemoteStopChargingStationResultType Result
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

        #region (private) RemoteStopChargingStationResult(Result)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="Result">The result of the remote Stop operation.</param>
        private RemoteStopChargingStationResult(RemoteStopChargingStationResultType  Result)
        {

            this._Result        = Result;
            this._ErrorMessage  = null;

        }

        #endregion

        #region (private) RemoteStopChargingStationResult(SessionId)

        /// <summary>
        /// Create a new remote stop result caused by an invalid session identification.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        private RemoteStopChargingStationResult(ChargingSession_Id SessionId)
        {

            this._Result        = RemoteStopChargingStationResultType.InvalidSessionId;
            this._SessionId     = SessionId;
            this._ErrorMessage  = "The session identification is invalid!";

        }

        #endregion

        #region (private) RemoteStopChargingStationResult(ErrorMessage = null)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="ErrorMessage">A optional error message.</param>
        private RemoteStopChargingStationResult(String ErrorMessage = null)
        {

            this._Result        = RemoteStopChargingStationResultType.Error;
            this._ErrorMessage  = ErrorMessage;

        }

        #endregion

        #endregion


        #region (static) Unspecified

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static RemoteStopChargingStationResult Unspecified
        {
            get
            {
                return new RemoteStopChargingStationResult(RemoteStopChargingStationResultType.Unspecified);
            }
        }

        #endregion

        #region (static) UnknownChargingStation

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        public static RemoteStopChargingStationResult UnknownChargingStation
        {
            get
            {
                return new RemoteStopChargingStationResult(RemoteStopChargingStationResultType.UnknownChargingStation);
            }
        }

        #endregion

        #region (static) InvalidSessionId

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        public static RemoteStopChargingStationResult InvalidSessionId(ChargingSession_Id SessionId)
        {
            return new RemoteStopChargingStationResult(SessionId);
        }

        #endregion

        #region (static) OutOfService

        /// <summary>
        /// The charging station is out of service.
        /// </summary>
        public static RemoteStopChargingStationResult OutOfService
        {
            get
            {
                return new RemoteStopChargingStationResult(RemoteStopChargingStationResultType.OutOfService);
            }
        }

        #endregion

        #region (static) Offline

        /// <summary>
        /// The charging station is offline.
        /// </summary>
        public static RemoteStopChargingStationResult Offline
        {
            get
            {
                return new RemoteStopChargingStationResult(RemoteStopChargingStationResultType.Offline);
            }
        }

        #endregion

        #region (static) Success

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        public static RemoteStopChargingStationResult Success
        {
            get
            {
                return new RemoteStopChargingStationResult(RemoteStopChargingStationResultType.Success);
            }
        }

        #endregion

        #region (static) Timeout

        /// <summary>
        /// The remote stop ran into a timeout.
        /// </summary>
        public static RemoteStopChargingStationResult Timeout
        {
            get
            {
                return new RemoteStopChargingStationResult(RemoteStopChargingStationResultType.Timeout);
            }
        }

        #endregion

        #region (static) Error(Message = null)

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        /// <param name="Message">An optional error message.</param>
        public static RemoteStopChargingStationResult Error(String Message = null)
        {
            return new RemoteStopChargingStationResult(Message);
        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _Result.ToString();
        }

        #endregion

    }


    /// <summary>
    /// The result types of a remote stop operation at a charging station.
    /// </summary>
    public enum RemoteStopChargingStationResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        UnknownChargingStation,

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The charging station is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The charging station is offline.
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
