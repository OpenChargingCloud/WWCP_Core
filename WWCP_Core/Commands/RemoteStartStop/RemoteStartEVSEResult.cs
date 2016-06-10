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
    /// The result of a remote start operation at an EVSE.
    /// </summary>
    public class RemoteStartEVSEResult
    {

        #region Properties

        /// <summary>
        /// The result of a remote start operation.
        /// </summary>
        public RemoteStartEVSEResultType  Result            { get; }

        /// <summary>
        /// The charging session for the remote start operation.
        /// </summary>
        public ChargingSession            Session           { get; }

        /// <summary>
        /// An optional (error) message.
        /// </summary>
        public String                     Message           { get; }

        /// <summary>
        /// An optional additional information on this error,
        /// e.g. the HTTP error response.
        /// </summary>
        public Object                     AdditionalInfo    { get; }

        #endregion

        #region Constructor(s)

        #region (private) RemoteStartResult(Session)

        /// <summary>
        /// Create a new successful remote start result.
        /// </summary>
        /// <param name="Session">The charging session.</param>
        private RemoteStartEVSEResult(ChargingSession Session)
        {

            #region Initial checks

            if (Session == null)
                throw new ArgumentNullException(nameof(Session),  "The given charging session must not be null!");

            #endregion

            this.Result   = RemoteStartEVSEResultType.Success;
            this.Session  = Session;
            this.Message  = null;

        }

        #endregion

        #region (private) RemoteStartResult(Result, Message = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new remote start result.
        /// </summary>
        /// <param name="Result">The result of the remote start operation.</param>
        /// <param name="Message">An optional message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        private RemoteStartEVSEResult(RemoteStartEVSEResultType  Result,
                                      String                     Message         = null,
                                      Object                     AdditionalInfo  = null)
        {

            this.Result          = Result;
            this.Session         = null;
            this.Message         = Message;
            this.AdditionalInfo  = AdditionalInfo;

        }

        #endregion

        #endregion


        #region (static) Unspecified

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static RemoteStartEVSEResult Unspecified

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.Unspecified);

        #endregion

        #region (static) UnknownEVSEOperator

        /// <summary>
        /// The EVSE operator is unknown.
        /// </summary>
        public static RemoteStartEVSEResult UnknownEVSEOperator

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.UnknownEVSEOperator);

        #endregion

        #region (static) UnknownEVSE

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        public static RemoteStartEVSEResult UnknownEVSE

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.UnknownEVSE);

        #endregion

        #region (static) InvalidSessionId

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        public static RemoteStartEVSEResult InvalidSessionId

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.InvalidSessionId);

        #endregion

        #region (static) InvalidCredentials

        /// <summary>
        /// Unauthorized remote start or invalid credentials.
        /// </summary>
        public static RemoteStartEVSEResult InvalidCredentials

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.InvalidCredentials);

        #endregion

        #region (static) AlreadyInUse

        /// <summary>
        /// The EVSE is already in use.
        /// </summary>
        public static RemoteStartEVSEResult AlreadyInUse

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.AlreadyInUse);

        #endregion

        #region (static) Reserved(Message)

        /// <summary>
        /// The EVSE is reserved.
        /// </summary>
        /// <param name="Message">An optional message.</param>
        public static RemoteStartEVSEResult Reserved(String Message = null)

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.Reserved, Message);

        #endregion

        #region (static) InternalUse

        /// <summary>
        /// The EVSE is reserved for internal use.
        /// </summary>
        public static RemoteStartEVSEResult InternalUse

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.InternalUse);

        #endregion

        #region (static) OutOfService

        /// <summary>
        /// The EVSE is out-of-service.
        /// </summary>
        public static RemoteStartEVSEResult OutOfService

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.OutOfService);

        #endregion

        #region (static) Offline

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        public static RemoteStartEVSEResult Offline

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.Offline);

        #endregion

        #region (static) Success()

        /// <summary>
        /// The remote start was successful.
        /// The charging session will be send separately.
        /// </summary>
        public static RemoteStartEVSEResult Success()

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.Success);

        #endregion

        #region (static) Success(Session)

        /// <summary>
        /// The remote start was successful and a charging session
        /// will be embedded within the response.
        /// </summary>
        /// <param name="Session">The charging session.</param>
        public static RemoteStartEVSEResult Success(ChargingSession Session)

            => new RemoteStartEVSEResult(Session);

        #endregion

        #region (static) Timeout

        /// <summary>
        /// The remote start request ran into a timeout.
        /// </summary>
        public static RemoteStartEVSEResult Timeout

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.Timeout);

        #endregion

        #region (static) CommunicationError(Message = "", AdditionalInfo = null)

        /// <summary>
        /// A communication error occured.
        /// </summary>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        public static RemoteStartEVSEResult CommunicationError(String  Message         = null,
                                                               Object  AdditionalInfo  = null)

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.CommunicationError,
                                         Message,
                                         AdditionalInfo);

        #endregion

        #region (static) Error(Message = null, AdditionalInfo = null)

        /// <summary>
        /// The remote start request led to an error.
        /// </summary>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        public static RemoteStartEVSEResult Error(String  Message         = null,
                                                  Object  AdditionalInfo  = null)

            => new RemoteStartEVSEResult(RemoteStartEVSEResultType.Error,
                                         Message,
                                         AdditionalInfo);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => Result.ToString();

        #endregion

    }


    /// <summary>
    /// The result types of a remote start operation at an EVSE.
    /// </summary>
    public enum RemoteStartEVSEResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,


        /// <summary>
        /// The EVSE operator is unknown.
        /// </summary>
        UnknownEVSEOperator,

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        UnknownEVSE,


        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// Unauthorized remote start or invalid credentials.
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// The EVSE is already in use.
        /// </summary>
        AlreadyInUse,

        /// <summary>
        /// The EVSE is reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// The EVSE is reserved for internal use.
        /// </summary>
        InternalUse,

        /// <summary>
        /// The EVSE is out-of-service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        Offline,

        /// <summary>
        /// The remote start was successful.
        /// </summary>
        Success,


        /// <summary>
        /// The remote start request ran into a timeout.
        /// </summary>
        Timeout,

        /// <summary>
        /// A communication error occured.
        /// </summary>
        CommunicationError,

        /// <summary>
        /// The remote start request led to an error.
        /// </summary>
        Error

    }

}
