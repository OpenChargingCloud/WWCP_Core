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
    /// The result of a remote stop operation at an EVSE.
    /// </summary>
    public class RemoteStopEVSEResult
    {

        #region Properties

        #region Result

        private readonly RemoteStopEVSEResultType _Result;

        /// <summary>
        /// The result of a remote stop operation.
        /// </summary>
        public RemoteStopEVSEResultType Result
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

        #region ReservationId

        private readonly ChargingReservation_Id _ReservationId;

        /// <summary>
        /// The charging reservation identification.
        /// </summary>
        public ChargingReservation_Id ReservationId
        {
            get
            {
                return _ReservationId;
            }
        }

        #endregion

        #region ReservationHandling

        private readonly ReservationHandling _ReservationHandling;

        /// <summary>
        /// The handling of the charging reservation after the charging session stopped.
        /// </summary>
        public ReservationHandling ReservationHandling
        {
            get
            {
                return _ReservationHandling;
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

        #region RemoteStopEVSEResult(SessionId, Result, ErrorMessage = null)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ErrorMessage">A optional error message.</param>
        private RemoteStopEVSEResult(ChargingSession_Id        SessionId,
                                     RemoteStopEVSEResultType  Result,
                                     String                    ErrorMessage  = null)
        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId), "The given charging session identification must not be null!");

            #endregion

            this._SessionId     = SessionId;
            this._Result        = Result;
            this._ErrorMessage  = ErrorMessage;

        }

        #endregion

        #region RemoteStopEVSEResult(SessionId, Result, ReservationId, ReservationHandling)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        private RemoteStopEVSEResult(ChargingSession_Id        SessionId,
                                     RemoteStopEVSEResultType  Result,
                                     ChargingReservation_Id    ReservationId,
                                     ReservationHandling       ReservationHandling)
        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId), "The given charging session identification must not be null!");

            #endregion

            this._SessionId            = SessionId;
            this._Result               = Result;
            this._ReservationId        = ReservationId;
            this._ReservationHandling  = ReservationHandling != null ? ReservationHandling : ReservationHandling.Close;

        }

        #endregion

        #endregion


        #region (static) Unspecified(SessionId)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult Unspecified(ChargingSession_Id SessionId)
        {

            return new RemoteStopEVSEResult(SessionId,
                                            RemoteStopEVSEResultType.Unspecified);

        }

        #endregion

        #region (static) UnknownOperator(SessionId)

        /// <summary>
        /// The EVSE operator is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult UnknownOperator(ChargingSession_Id SessionId)
        {
            return new RemoteStopEVSEResult(SessionId,
                                            RemoteStopEVSEResultType.UnknownOperator,
                                            "The EVSE operator is unknown!");
        }

        #endregion

        #region (static) UnknownEVSE(SessionId)

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult UnknownEVSE(ChargingSession_Id SessionId)
        {
            return new RemoteStopEVSEResult(SessionId,
                                            RemoteStopEVSEResultType.UnknownEVSE,
                                            "The EVSE is unknown!");
        }

        #endregion

        #region (static) InvalidSessionId(SessionId)

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult InvalidSessionId(ChargingSession_Id SessionId)
        {

            return new RemoteStopEVSEResult(SessionId,
                                            RemoteStopEVSEResultType.InvalidSessionId,
                                            "The session identification is invalid!");

        }

        #endregion

        #region (static) OutOfService(SessionId)

        /// <summary>
        /// The EVSE is out of service.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult OutOfService(ChargingSession_Id SessionId)
        {

            return new RemoteStopEVSEResult(SessionId,
                                            RemoteStopEVSEResultType.OutOfService,
                                            "The EVSE is out of service!");

        }

        #endregion

        #region (static) Offline(SessionId)

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult Offline(ChargingSession_Id SessionId)
        {

            return new RemoteStopEVSEResult(SessionId,
                                            RemoteStopEVSEResultType.Offline,
                                            "The EVSE is offline!");

        }

        #endregion

        #region (static) Success(SessionId, ReservationId = null, ReservationHandling = null)

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        public static RemoteStopEVSEResult Success(ChargingSession_Id      SessionId,
                                                   ChargingReservation_Id  ReservationId        = null,
                                                   ReservationHandling     ReservationHandling  = null)
        {

            return new RemoteStopEVSEResult(SessionId,
                                            RemoteStopEVSEResultType.Success,
                                            ReservationId,
                                            ReservationHandling);

        }

        #endregion

        #region (static) Timeout(SessionId)

        /// <summary>
        /// The remote stop ran into a timeout.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult Timeout(ChargingSession_Id SessionId)
        {

            return new RemoteStopEVSEResult(SessionId,
                                            RemoteStopEVSEResultType.Timeout);

        }

        #endregion

        #region (static) Error(SessionId, Message = null)

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Message">An optional error message.</param>
        public static RemoteStopEVSEResult Error(ChargingSession_Id  SessionId,
                                                 String              Message = null)
        {

            return new RemoteStopEVSEResult(SessionId,
                                            RemoteStopEVSEResultType.Error,
                                            Message);

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
    /// The result types of a remote stop operation at an EVSE.
    /// </summary>
    public enum RemoteStopEVSEResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The EVSE operator is unknown.
        /// </summary>
        UnknownOperator,

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
