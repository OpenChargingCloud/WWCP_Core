/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of a remote stop operation at an EVSE.
    /// </summary>
    public class RemoteStopEVSEResult
    {

        #region Properties

        /// <summary>
        /// The result of a remote stop operation.
        /// </summary>
        public RemoteStopEVSEResultType  Result                 { get; }

        /// <summary>
        /// The charging session identification for an invalid remote stop operation.
        /// </summary>
        public ChargingSession_Id        SessionId              { get; }

        /// <summary>
        /// The charge detail record for a successfully stopped charging process.
        /// </summary>
        public ChargeDetailRecord        ChargeDetailRecord     { get; }

        /// <summary>
        /// The charging reservation identification.
        /// </summary>
        public ChargingReservation_Id?   ReservationId          { get; }

        /// <summary>
        /// The handling of the charging reservation after the charging session stopped.
        /// </summary>
        public ReservationHandling       ReservationHandling    { get; }

        /// <summary>
        /// An optional (error) message.
        /// </summary>
        public String                    Message                { get; }

        /// <summary>
        /// An optional additional information on this error,
        /// e.g. the HTTP error response.
        /// </summary>
        public Object                    AdditionalInfo         { get; }

        #endregion

        #region Constructor(s)

        #region RemoteStopEVSEResult(SessionId, Result, Message = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="Message">A optional error message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        private RemoteStopEVSEResult(ChargingSession_Id        SessionId,
                                     RemoteStopEVSEResultType  Result,
                                     String                    Message         = null,
                                     Object                    AdditionalInfo  = null)
        {

            this.SessionId       = SessionId;
            this.Result          = Result;
            this.Message         = Message;
            this.AdditionalInfo  = AdditionalInfo;

        }

        #endregion

        #region RemoteStopEVSEResult(SessionId, Result, ReservationId, ReservationHandling = null)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        private RemoteStopEVSEResult(ChargingSession_Id        SessionId,
                                     RemoteStopEVSEResultType  Result,
                                     ChargingReservation_Id?   ReservationId,
                                     ReservationHandling?      ReservationHandling = null)
        {

            this.SessionId            = SessionId;
            this.Result               = Result;
            this.ReservationId        = ReservationId;
            this.ReservationHandling  = ReservationHandling ?? WWCP.ReservationHandling.Close;

        }

        #endregion

        #region RemoteStopEVSEResult(ChargeDetailRecord, Result, ReservationId, ReservationHandling = null)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="ChargeDetailRecord">The charge detail record for a successfully stopped charging process.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        private RemoteStopEVSEResult(ChargeDetailRecord        ChargeDetailRecord,
                                     RemoteStopEVSEResultType  Result,
                                     ChargingReservation_Id?   ReservationId,
                                     ReservationHandling?      ReservationHandling = null)
        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");

            #endregion

            this.ChargeDetailRecord   = ChargeDetailRecord;
            this.SessionId            = ChargeDetailRecord.SessionId;
            this.Result               = Result;
            this.ReservationId        = ReservationId;
            this.ReservationHandling  = ReservationHandling ?? WWCP.ReservationHandling.Close;

        }

        #endregion

        #endregion


        #region (static) Unspecified(SessionId)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult Unspecified(ChargingSession_Id SessionId)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.Unspecified);

        #endregion

        #region (static) UnknownOperator(SessionId)

        /// <summary>
        /// The Charging Station Operator is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult UnknownOperator(ChargingSession_Id SessionId)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.UnknownOperator);

        #endregion

        #region (static) UnknownEVSE(SessionId)

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult UnknownEVSE(ChargingSession_Id SessionId)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.UnknownEVSE);

        #endregion

        #region (static) InvalidSessionId(SessionId)

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult InvalidSessionId(ChargingSession_Id SessionId)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.InvalidSessionId);

        #endregion

        #region (static) InvalidCredentials(SessionId)

        /// <summary>
        /// Unauthorized remote stop or invalid credentials.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult InvalidCredentials(ChargingSession_Id SessionId)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.InvalidCredentials);

        #endregion

        #region (static) InternalUse(SessionId)

        /// <summary>
        /// The EVSE is reserved for internal use.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult InternalUse(ChargingSession_Id SessionId)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.InternalUse);

        #endregion

        #region (static) OutOfService(SessionId)

        /// <summary>
        /// The EVSE is out of service.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult OutOfService(ChargingSession_Id SessionId)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.OutOfService);

        #endregion

        #region (static) Offline(SessionId)

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult Offline(ChargingSession_Id SessionId)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.Offline);

        #endregion

        #region (static) Success(SessionId, ReservationId = null, ReservationHandling = null)

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        public static RemoteStopEVSEResult Success(ChargingSession_Id       SessionId,
                                                   ChargingReservation_Id?  ReservationId        = null,
                                                   ReservationHandling?     ReservationHandling  = null)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.Success,
                                        ReservationId,
                                        ReservationHandling);

        #endregion

        #region (static) Success(ChargeDetailRecord, ReservationId = null, ReservationHandling = null)

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        /// <param name="ChargeDetailRecord">The charge detail record for a successfully stopped charging process.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        public static RemoteStopEVSEResult Success(ChargeDetailRecord       ChargeDetailRecord,
                                                   ChargingReservation_Id?  ReservationId        = null,
                                                   ReservationHandling?     ReservationHandling  = null)

            => new RemoteStopEVSEResult(ChargeDetailRecord,
                                        RemoteStopEVSEResultType.Success,
                                        ReservationId,
                                        ReservationHandling);

        #endregion

        #region (static) Timeout(SessionId)

        /// <summary>
        /// The remote stop request ran into a timeout.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopEVSEResult Timeout(ChargingSession_Id SessionId)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.Timeout);

        #endregion

        #region (static) CommunicationError(SessionId, Message = "", AdditionalInfo = null)

        /// <summary>
        /// A communication error occured.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        public static RemoteStopEVSEResult CommunicationError(ChargingSession_Id  SessionId,
                                                              String              Message         = null,
                                                              Object              AdditionalInfo  = null)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.CommunicationError,
                                        Message,
                                        AdditionalInfo);

        #endregion

        #region (static) Error(SessionId, Message = null, AdditionalInfo = null)

        /// <summary>
        /// The remote stop request led to an error.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        public static RemoteStopEVSEResult Error(ChargingSession_Id  SessionId,
                                                 String              Message         = null,
                                                 Object              AdditionalInfo  = null)

            => new RemoteStopEVSEResult(SessionId,
                                        RemoteStopEVSEResultType.Error,
                                        Message,
                                        AdditionalInfo);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result.ToString(),
                             ChargeDetailRecord != null
                                 ? ", with CDR"
                                 : "",
                             ", ReservationHandling: " + ReservationHandling);

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
        /// The Charging Station Operator is unknown.
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
        /// Unauthorized remote stop or invalid credentials.
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// The EVSE is reserved for internal use.
        /// </summary>
        InternalUse,

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
        /// The remote stop request ran into a timeout.
        /// </summary>
        Timeout,

        /// <summary>
        /// A communication error occured.
        /// </summary>
        CommunicationError,

        /// <summary>
        /// The remote stop request led to an error.
        /// </summary>
        Error

    }

}
