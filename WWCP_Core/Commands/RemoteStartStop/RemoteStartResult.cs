/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The result of a remote start operation.
    /// </summary>
    public class RemoteStartResult
    {

        #region Properties

        /// <summary>
        /// The result of a remote start operation.
        /// </summary>
        public RemoteStartResultTypes  Result            { get; }

        /// <summary>
        /// The charging session for the remote start operation.
        /// </summary>
        public ChargingSession         Session           { get; }

        /// <summary>
        /// An optional (error) message.
        /// </summary>
        public String                  Message           { get; }

        /// <summary>
        /// An optional additional information on this error,
        /// e.g. the HTTP error response.
        /// </summary>
        public Object                  AdditionalInfo    { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?               Runtime           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new remote start result.
        /// </summary>
        /// <param name="Result">The result of the remote start operation.</param>
        /// <param name="Session">The charging session.</param>
        /// <param name="Message">An optional message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private RemoteStartResult(RemoteStartResultTypes  Result,
                                  ChargingSession         Session          = null,
                                  String                  Message          = null,
                                  Object                  AdditionalInfo   = null,
                                  TimeSpan?               Runtime          = null)
        {

            this.Result          = Result;
            this.Session         = Session;
            this.Message         = Message;
            this.AdditionalInfo  = AdditionalInfo;
            this.Runtime         = Runtime;

        }

        #endregion


        #region (static) Unspecified

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static RemoteStartResult Unspecified

            => new RemoteStartResult(RemoteStartResultTypes.Unspecified);

        #endregion

        #region (static) UnknownOperator(Runtime = null)

        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult UnknownOperator(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.UnknownOperator,
                                     Runtime: Runtime);

        #endregion

        #region (static) UnknownLocation(Runtime = null)

        /// <summary>
        /// The charging location is unknown.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult UnknownLocation(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.UnknownLocation,
                                     Runtime: Runtime);

        #endregion

        #region (static) InvalidSessionId(Runtime = null)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult InvalidSessionId(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.InvalidSessionId,
                                     Runtime: Runtime);

        #endregion

        #region (static) InvalidCredentials(Runtime = null)

        /// <summary>
        /// Unauthorized remote start or invalid credentials.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult InvalidCredentials(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.InvalidCredentials,
                                     Runtime: Runtime);

        #endregion

        #region (static) AlreadyInUse(Runtime = null)

        /// <summary>
        /// The EVSE is already in use.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult AlreadyInUse(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.AlreadyInUse,
                                     Runtime: Runtime);

        #endregion

        #region (static) Reserved(Message, Runtime = null)

        /// <summary>
        /// The EVSE is reserved.
        /// </summary>
        /// <param name="Message">An optional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Reserved(String     Message   = null,
                                                 TimeSpan?  Runtime   = null)

            => new RemoteStartResult(RemoteStartResultTypes.Reserved,
                                     null,
                                     Message,
                                     Runtime: Runtime);

        #endregion

        #region (static) InternalUse(Runtime = null)

        /// <summary>
        /// The EVSE is reserved for internal use.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult InternalUse(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.InternalUse,
                                     Runtime: Runtime);

        #endregion

        #region (static) OutOfService(Runtime = null)

        /// <summary>
        /// The EVSE is out-of-service.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult OutOfService(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.OutOfService,
                                     Runtime: Runtime);

        #endregion

        #region (static) Offline(Runtime = null)

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Offline(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.Offline,
                                     Runtime: Runtime);

        #endregion

        #region (static) Success(Session, Runtime = null)

        /// <summary>
        /// The remote start was successful and a charging session
        /// will be embedded within the response.
        /// </summary>
        /// <param name="Session">The charging session.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Success(ChargingSession  Session,
                                                TimeSpan?        Runtime  = null)

            => new RemoteStartResult(RemoteStartResultTypes.Success,
                                     Session,
                                     Runtime: Runtime);

        #endregion

        #region (static) Timeout(Runtime = null)

        /// <summary>
        /// The remote start request ran into a timeout.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Timeout(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.Timeout,
                                     Runtime: Runtime);

        #endregion

        #region (static) CommunicationError(Message = "", AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// A communication error occured.
        /// </summary>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult CommunicationError(String     Message          = null,
                                                           Object     AdditionalInfo   = null,
                                                           TimeSpan?  Runtime          = null)

            => new RemoteStartResult(RemoteStartResultTypes.CommunicationError,
                                     null,
                                     Message,
                                     AdditionalInfo,
                                     Runtime);

        #endregion

        #region (static) Error(Message = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote start request led to an error.
        /// </summary>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Error(String     Message          = null,
                                              Object     AdditionalInfo   = null,
                                              TimeSpan?  Runtime          = null)

            => new RemoteStartResult(RemoteStartResultTypes.Error,
                                     null,
                                     Message,
                                     AdditionalInfo,
                                     Runtime);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => Result.ToString();

        #endregion

    }


    /// <summary>
    /// The result types of a remote start operation at an EVSE.
    /// </summary>
    public enum RemoteStartResultTypes
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
        /// The charging location is unknown.
        /// </summary>
        UnknownLocation,


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
