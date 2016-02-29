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

        #region RemoteStopChargingStationResult(SessionId, Result, ErrorMessage = null)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ErrorMessage">A optional error message.</param>
        private RemoteStopChargingStationResult(ChargingSession_Id                   SessionId,
                                                RemoteStopChargingStationResultType  Result,
                                                String                               ErrorMessage  = null)
        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId), "The given charging session identification must not be null!");

            #endregion

            this._SessionId            = SessionId;
            this._Result               = Result;
            this._ErrorMessage         = ErrorMessage;

        }

        #endregion

        #region RemoteStopChargingStationResult(SessionId, Result, ReservationId, ReservationHandling)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        private RemoteStopChargingStationResult(ChargingSession_Id                   SessionId,
                                                RemoteStopChargingStationResultType  Result,
                                                ChargingReservation_Id               ReservationId,
                                                ReservationHandling                  ReservationHandling)
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

        #region RemoteStopChargingStationResult(ChargeDetailRecord, Result, ReservationId, ReservationHandling)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="ChargeDetailRecord">The charge detail record for a successfully stopped charging process.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        private RemoteStopChargingStationResult(ChargeDetailRecord                   ChargeDetailRecord,
                                                RemoteStopChargingStationResultType  Result,
                                                ChargingReservation_Id               ReservationId,
                                                ReservationHandling                  ReservationHandling)
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
        public static RemoteStopChargingStationResult Unspecified(ChargingSession_Id SessionId)
        {

            return new RemoteStopChargingStationResult(SessionId,
                                                       RemoteStopChargingStationResultType.Unspecified);

        }

        #endregion

        #region (static) UnknownOperator(SessionId)

        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopChargingStationResult UnknownOperator(ChargingSession_Id SessionId)
        {

            return new RemoteStopChargingStationResult(SessionId,
                                                       RemoteStopChargingStationResultType.UnknownOperator,
                                                       "The charging station operator is unknown!");

        }

        #endregion

        #region (static) UnknownChargingStation(SessionId)

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopChargingStationResult UnknownChargingStation(ChargingSession_Id SessionId)
        {

            return new RemoteStopChargingStationResult(SessionId,
                                                       RemoteStopChargingStationResultType.UnknownChargingStation,
                                                       "The charging station is unknown!");

        }

        #endregion

        #region (static) InvalidSessionId(SessionId)

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopChargingStationResult InvalidSessionId(ChargingSession_Id SessionId)
        {

            return new RemoteStopChargingStationResult(SessionId,
                                                       RemoteStopChargingStationResultType.InvalidSessionId,
                                                       "The session identification is invalid!");

        }

        #endregion

        #region (static) InvalidCredentials(SessionId)

        /// <summary>
        /// Unauthorized remote stop or invalid credentials.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopChargingStationResult InvalidCredentials(ChargingSession_Id SessionId)
        {
            return new RemoteStopChargingStationResult(SessionId,
                                            RemoteStopChargingStationResultType.InvalidCredentials,
                                            "Unauthorized remote stop or invalid credentials!");
        }

        #endregion

        #region (static) InternalUse(SessionId)

        /// <summary>
        /// The charging session is reserved for internal use.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopChargingStationResult InternalUse(ChargingSession_Id SessionId)
        {

            return new RemoteStopChargingStationResult(SessionId,
                                                       RemoteStopChargingStationResultType.InternalUse,
                                                       "The charging session is reserved for internal use!");

        }

        #endregion

        #region (static) OutOfService(SessionId)

        /// <summary>
        /// The charging station is out of service.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopChargingStationResult OutOfService(ChargingSession_Id SessionId)
        {

            return new RemoteStopChargingStationResult(SessionId,
                                                       RemoteStopChargingStationResultType.OutOfService,
                                                       "The charging station is out of service!");

        }

        #endregion

        #region (static) Offline(SessionId)

        /// <summary>
        /// The charging station is offline.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopChargingStationResult Offline(ChargingSession_Id SessionId)
        {

            return new RemoteStopChargingStationResult(SessionId,
                                                       RemoteStopChargingStationResultType.Offline,
                                                       "The charging station is offline!");

        }

        #endregion

        #region (static) Success(SessionId, ReservationId = null, ReservationHandling = null)

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        public static RemoteStopChargingStationResult Success(ChargingSession_Id      SessionId,
                                                              ChargingReservation_Id  ReservationId        = null,
                                                              ReservationHandling     ReservationHandling  = null)
        {

            return new RemoteStopChargingStationResult(SessionId,
                                                       RemoteStopChargingStationResultType.Success,
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
        public static RemoteStopChargingStationResult Success(ChargeDetailRecord      ChargeDetailRecord,
                                                              ChargingReservation_Id  ReservationId        = null,
                                                              ReservationHandling     ReservationHandling  = null)
        {

            return new RemoteStopChargingStationResult(ChargeDetailRecord,
                                                       RemoteStopChargingStationResultType.Success,
                                                       ReservationId,
                                                       ReservationHandling);

        }

        #endregion

        #region (static) Timeout(SessionId)

        /// <summary>
        /// The remote stop ran into a timeout.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopChargingStationResult Timeout(ChargingSession_Id SessionId)
        {

            return new RemoteStopChargingStationResult(SessionId,
                                                       RemoteStopChargingStationResultType.Timeout);

        }

        #endregion

        #region (static) Error(SessionId, Message = null)

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Message">An optional error message.</param>
        public static RemoteStopChargingStationResult Error(ChargingSession_Id  SessionId,
                                                            String              Message  = null)
        {

            return new RemoteStopChargingStationResult(SessionId,
                                                       RemoteStopChargingStationResultType.Error,
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
    /// The result types of a remote stop operation at a charging station.
    /// </summary>
    public enum RemoteStopChargingStationResultType
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
        /// The charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// Unauthorized remote stop or invalid credentials.
        /// </summary>
        InvalidCredentials,

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
