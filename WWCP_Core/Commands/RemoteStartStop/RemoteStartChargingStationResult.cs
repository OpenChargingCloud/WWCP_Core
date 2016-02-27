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
    /// The result of a remote start operation at a charging station.
    /// </summary>
    public class RemoteStartChargingStationResult
    {

        #region Properties

        #region Result

        private readonly RemoteStartChargingStationResultType _Result;

        /// <summary>
        /// The result of a remote start operation.
        /// </summary>
        public RemoteStartChargingStationResultType Result
        {
            get
            {
                return _Result;
            }
        }

        #endregion

        #region Session

        private readonly ChargingSession _Session;

        /// <summary>
        /// The charging session for the remote start operation.
        /// </summary>
        public ChargingSession Session
        {
            get
            {
                return _Session;
            }
        }

        #endregion

        #region Message

        private readonly String _Message;

        /// <summary>
        /// An optional (error) message.
        /// </summary>
        public String Message
        {
            get
            {
                return _Message;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (private) RemoteStartResult(Result)

        /// <summary>
        /// Create a new remote start result.
        /// </summary>
        /// <param name="Result">The result of the remote start operation.</param>
        private RemoteStartChargingStationResult(RemoteStartChargingStationResultType Result)
        {

            this._Result   = Result;
            this._Session  = null;
            this._Message  = null;

        }

        #endregion

        #region (private) RemoteStartResult(Session)

        /// <summary>
        /// Create a new successful remote start result.
        /// </summary>
        /// <param name="Session">The charging session (mandatory for successful session starts).</param>
        private RemoteStartChargingStationResult(ChargingSession  Session)
        {

            this._Result        = RemoteStartChargingStationResultType.Success;
            this._Session       = Session;
            this._Message  = null;

        }

        #endregion

        #region (private) RemoteStartResult(Message)

        /// <summary>
        /// Create a new remote start result.
        /// </summary>
        /// <param name="Message">A optional (error) message.</param>
        private RemoteStartChargingStationResult(String Message)
        {

            this._Result   = RemoteStartChargingStationResultType.Error;
            this._Message  = Message;

        }

        #endregion

        #endregion


        #region (static) Unspecified

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static RemoteStartChargingStationResult Unspecified
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.Unspecified);
            }
        }

        #endregion

        #region (static) UnknownOperator

        /// <summary>
        /// The charging session operator is unknown.
        /// </summary>
        public static RemoteStartChargingStationResult UnknownOperator
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.UnknownOperator);
            }
        }

        #endregion

        #region (static) UnknownChargingStation

        /// <summary>
        /// The charging session is unknown.
        /// </summary>
        public static RemoteStartChargingStationResult UnknownChargingStation
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.UnknownChargingStation);
            }
        }

        #endregion

        #region (static) InvalidSessionId

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        public static RemoteStartChargingStationResult InvalidSessionId
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.InvalidSessionId);
            }
        }

        #endregion

        #region (static) InvalidCredentials

        /// <summary>
        /// Unauthorized remote start or invalid credentials.
        /// </summary>
        public static RemoteStartChargingStationResult InvalidCredentials
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.InvalidCredentials);
            }
        }

        #endregion

        #region (static) NoEVSEsAvailable

        /// <summary>
        /// The charging station does not have any available EVSEs for charging.
        /// </summary>
        public static RemoteStartChargingStationResult NoEVSEsAvailable
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.NoEVSEsAvailable);
            }
        }

        #endregion

        #region (static) Reserved

        /// <summary>
        /// The charging station is reserved.
        /// </summary>
        public static RemoteStartChargingStationResult Reserved
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.Reserved);
            }
        }

        #endregion

        #region (static) InternalUse

        /// <summary>
        /// The charging station is reserved for internal use.
        /// </summary>
        public static RemoteStartChargingStationResult InternalUse
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.InternalUse);
            }
        }

        #endregion

        #region (static) OutOfService

        /// <summary>
        /// The charging station is out of service.
        /// </summary>
        public static RemoteStartChargingStationResult OutOfService
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.OutOfService);
            }
        }

        #endregion

        #region (static) Offline

        /// <summary>
        /// The charging station is offline.
        /// </summary>
        public static RemoteStartChargingStationResult Offline
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.Offline);
            }
        }

        #endregion

        #region (static) Success()

        /// <summary>
        /// The remote start was successful.
        /// </summary>
        public static RemoteStartChargingStationResult Success()
        {

            return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.Success);

        }

        #endregion

        #region (static) Success(Session)

        /// <summary>
        /// The remote start was successful.
        /// </summary>
        /// <param name="Session">The charging session.</param>
        public static RemoteStartChargingStationResult Success(ChargingSession Session)
        {

            #region Initial checks

            if (Session == null)
                throw new ArgumentNullException(nameof(Session), "The given charging session must not be null!");

            #endregion

            return new RemoteStartChargingStationResult(Session);

        }

        #endregion

        #region (static) Timeout

        /// <summary>
        /// The remote stop ran into a timeout.
        /// </summary>
        public static RemoteStartChargingStationResult Timeout
        {
            get
            {
                return new RemoteStartChargingStationResult(RemoteStartChargingStationResultType.Timeout);
            }
        }

        #endregion

        #region (static) Error(ErrorMessage = null)

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        /// <param name="ErrorMessage">An optional (error) message.</param>
        public static RemoteStartChargingStationResult Error(String ErrorMessage = null)
        {
            return new RemoteStartChargingStationResult(ErrorMessage);
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
    /// The result types of a remote start operation at a charging station.
    /// </summary>
    public enum RemoteStartChargingStationResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        UnknownOperator,

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        UnknownChargingStation,

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// Unauthorized remote start or invalid credentials.
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// The charging station does not have any available EVSEs for charging.
        /// </summary>
        NoEVSEsAvailable,

        /// <summary>
        /// The charging station is reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// The charging station is reserved for internal use.
        /// </summary>
        InternalUse,

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
