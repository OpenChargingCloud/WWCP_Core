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

        #region ChargeDetailRecord

        private readonly ChargeDetailRecord _ChargeDetailRecord;

        /// <summary>
        /// The charge detail record for a successfully stopped charging process.
        /// </summary>
        public ChargeDetailRecord ChargeDetailRecord
        {
            get
            {
                return _ChargeDetailRecord;
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

        #region RemoteStopResult(SessionId, Result, ErrorMessage = null)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ErrorMessage">A optional error message.</param>
        private RemoteStopResult(ChargingSession_Id    SessionId,
                                 RemoteStopResultType  Result,
                                 String                ErrorMessage = null)
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
        private RemoteStopResult(ChargingSession_Id      SessionId,
                                 RemoteStopResultType    Result,
                                 ChargingReservation_Id  ReservationId,
                                 ReservationHandling     ReservationHandling)
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

        #region RemoteStopEVSEResult(ChargeDetailRecord, Result, ReservationId, ReservationHandling)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="ChargeDetailRecord">The charge detail record for a successfully stopped charging process.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        private RemoteStopResult(ChargeDetailRecord      ChargeDetailRecord,
                                 RemoteStopResultType    Result,
                                 ChargingReservation_Id  ReservationId,
                                 ReservationHandling     ReservationHandling)
        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");

            #endregion

            this._ChargeDetailRecord   = ChargeDetailRecord;
            this._SessionId            = ChargeDetailRecord.SessionId;
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
        public static RemoteStopResult Unspecified(ChargingSession_Id SessionId)
        {

            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.Unspecified);

        }

        #endregion

        #region (static) UnknownOperator(SessionId)

        /// <summary>
        /// The  operator is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopResult UnknownOperator(ChargingSession_Id SessionId)
        {
            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.UnknownOperator,
                                        "The EVSE or charging station operator is unknown!");
        }

        #endregion

        #region (static) Unknown(SessionId)

        /// <summary>
        /// The  is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopResult Unknown(ChargingSession_Id SessionId)
        {
            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.UnknownOperator,
                                        "The EVSE or charging station is unknown!");
        }

        #endregion

        #region (static) InvalidSessionId(SessionId)

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopResult InvalidSessionId(ChargingSession_Id SessionId)
        {

            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.InvalidSessionId,
                                        "The session identification is invalid!");

        }

        #endregion

        #region (static) InvalidCredentials(SessionId)

        /// <summary>
        /// Unauthorized remote stop or invalid credentials.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopResult InvalidCredentials(ChargingSession_Id SessionId)
        {
            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.InvalidCredentials,
                                        "Unauthorized remote stop or invalid credentials!");
        }

        #endregion

        #region (static) InternalUse(SessionId)

        /// <summary>
        /// Reserved for internal use!
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopResult InternalUse(ChargingSession_Id SessionId)
        {

            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.InternalUse,
                                        "Reserved for internal use!");

        }

        #endregion

        #region (static) OutOfService(SessionId)

        /// <summary>
        /// The  is out of service.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopResult OutOfService(ChargingSession_Id SessionId)
        {

            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.OutOfService,
                                        "The EVSE or charging station is out of service!");

        }

        #endregion

        #region (static) Offline(SessionId)

        /// <summary>
        /// The  is offline.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopResult Offline(ChargingSession_Id SessionId)
        {

            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.Offline,
                                        "The EVSE or charging station is offline!");

        }

        #endregion

        #region (static) Success(SessionId, ReservationId = null, ReservationHandling = null)

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        public static RemoteStopResult Success(ChargingSession_Id      SessionId,
                                               ChargingReservation_Id  ReservationId        = null,
                                               ReservationHandling     ReservationHandling  = null)
        {

            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.Success,
                                        ReservationId,
                                        ReservationHandling);

        }

        #endregion

        #region (static) Success(ChargeDetailRecord, ReservationId = null, ReservationHandling = null)

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        /// <param name="ChargeDetailRecord">The charge detail record for a successfully stopped charging process.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        public static RemoteStopResult Success(ChargeDetailRecord      ChargeDetailRecord,
                                               ChargingReservation_Id  ReservationId        = null,
                                               ReservationHandling     ReservationHandling  = null)
        {

            return new RemoteStopResult(ChargeDetailRecord,
                                        RemoteStopResultType.Success,
                                        ReservationId,
                                        ReservationHandling);

        }

        #endregion

        #region (static) Timeout(SessionId)

        /// <summary>
        /// The remote stop ran into a timeout.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopResult Timeout(ChargingSession_Id SessionId)
        {

            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.Timeout);

        }

        #endregion

        #region (static) Error(SessionId, Message = null)

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Message">An optional error message.</param>
        public static RemoteStopResult Error(ChargingSession_Id  SessionId,
                                             String              Message = null)
        {

            return new RemoteStopResult(SessionId,
                                        RemoteStopResultType.Error,
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
    /// The result types of a remote stop operation.
    /// </summary>
    public enum RemoteStopResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The EVSE or charging station operator is unknown.
        /// </summary>
        UnknownOperator,

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// Unauthorized remote stop or invalid credentials.
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// Reserved for internal use.
        /// </summary>
        InternalUse,

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The EVSE or charging station is offline.
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
