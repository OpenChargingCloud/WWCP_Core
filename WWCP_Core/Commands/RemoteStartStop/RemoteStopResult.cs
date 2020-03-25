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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;
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

        /// <summary>
        /// The result of a remote stop operation.
        /// </summary>
        public RemoteStopResultTypes     Result                   { get; }

        /// <summary>
        /// The charging session identification for an invalid remote stop operation.
        /// </summary>
        public ChargingSession_Id       SessionId                { get; }

        /// <summary>
        /// The charge detail record for a successfully stopped charging process.
        /// </summary>
        public ChargeDetailRecord       ChargeDetailRecord       { get; }

        /// <summary>
        /// The charging reservation identification.
        /// </summary>
        public ChargingReservation_Id?  ReservationId            { get; }

        /// <summary>
        /// The handling of the charging reservation after the charging session stopped.
        /// </summary>
        public ReservationHandling      ReservationHandling      { get; }

        /// <summary>
        /// A optional description of the authorize stop result.
        /// </summary>
        public String                   Description              { get; }

        /// <summary>
        /// An optional additional message.
        /// </summary>
        public String                   AdditionalInfo           { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                Runtime                  { get; }

        #endregion

        #region Constructor(s)

        #region RemoteStopResult(SessionId, Result, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private RemoteStopResult(ChargingSession_Id     SessionId,
                                 RemoteStopResultTypes  Result,
                                 String                 Description      = null,
                                 String                 AdditionalInfo   = null,
                                 TimeSpan?              Runtime          = null)
        {

            this.SessionId       = SessionId;
            this.Result          = Result;
            this.Description     = Description;
            this.AdditionalInfo  = AdditionalInfo;
            this.Runtime         = Runtime;

        }

        #endregion

        #region RemoteStopResult(SessionId, Result, ReservationId, ReservationHandling)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private RemoteStopResult(ChargingSession_Id       SessionId,
                                 RemoteStopResultTypes    Result,
                                 ChargingReservation_Id?  ReservationId,
                                 ReservationHandling?     ReservationHandling,
                                 String                   Description      = null,
                                 String                   AdditionalInfo   = null,
                                 TimeSpan?                Runtime          = null)
        {

            this.SessionId            = SessionId;
            this.Result               = Result;
            this.ReservationId        = ReservationId;
            this.ReservationHandling  = ReservationHandling ?? WWCP.ReservationHandling.Close;
            this.Description          = Description;
            this.AdditionalInfo       = AdditionalInfo;
            this.Runtime              = Runtime;

        }

        #endregion

        #region RemoteStopResult(ChargeDetailRecord, Result, ReservationId, ReservationHandling)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="ChargeDetailRecord">The charge detail record for a successfully stopped charging process.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private RemoteStopResult(ChargeDetailRecord       ChargeDetailRecord,
                                 RemoteStopResultTypes    Result,
                                 ChargingReservation_Id?  ReservationId,
                                 ReservationHandling?     ReservationHandling,
                                 String                   Description      = null,
                                 String                   AdditionalInfo   = null,
                                 TimeSpan?                Runtime          = null)
        {

            this.ChargeDetailRecord   = ChargeDetailRecord  ?? throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");
            this.SessionId            = ChargeDetailRecord.SessionId;
            this.Result               = Result;
            this.ReservationId        = ReservationId;
            this.ReservationHandling  = ReservationHandling ?? WWCP.ReservationHandling.Close;
            this.Description          = Description;
            this.AdditionalInfo       = AdditionalInfo;
            this.Runtime              = Runtime;

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
                                        RemoteStopResultTypes.Unspecified);

        }

        #endregion

        #region (static) UnknownOperator(SessionId, Runtime = null)

        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult UnknownOperator(ChargingSession_Id  SessionId,
                                                       TimeSpan?           Runtime = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.UnknownOperator,
                                    "The EVSE or charging station operator is unknown!",
                                    Runtime: Runtime);

        #endregion

        #region (static) UnknownLocation(SessionId, Runtime = null)

        /// <summary>
        /// The charging location is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult UnknownLocation(ChargingSession_Id  SessionId,
                                                        TimeSpan?          Runtime = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.UnknownLocation,
                                    "The charging location is unknown!",
                                    Runtime: Runtime);

        #endregion

        #region (static) InvalidSessionId(SessionId, Runtime = null)

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult InvalidSessionId(ChargingSession_Id  SessionId,
                                                        TimeSpan?           Runtime  = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.InvalidSessionId,
                                    "The session identification is invalid!",
                                    Runtime: Runtime);

        #endregion

        #region (static) InvalidCredentials(SessionId, Runtime = null)

        /// <summary>
        /// Unauthorized remote stop or invalid credentials.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult InvalidCredentials(ChargingSession_Id  SessionId,
                                                          TimeSpan?           Runtime  = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.InvalidCredentials,
                                    "Unauthorized remote stop or invalid credentials!",
                                    Runtime: Runtime);

        #endregion

        #region (static) InternalUse(SessionId, Runtime = null)

        /// <summary>
        /// Reserved for internal use!
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult InternalUse(ChargingSession_Id  SessionId,
                                                   TimeSpan?           Runtime  = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.InternalUse,
                                    "Reserved for internal use!",
                                    Runtime: Runtime);

        #endregion

        #region (static) OutOfService(SessionId, Runtime = null)

        /// <summary>
        /// The  is out of service.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult OutOfService(ChargingSession_Id  SessionId,
                                                    TimeSpan?           Runtime  = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.OutOfService,
                                    "The EVSE or charging station is out of service!",
                                    Runtime: Runtime);

        #endregion

        #region (static) Offline(SessionId, Runtime = null)

        /// <summary>
        /// The  is offline.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult Offline(ChargingSession_Id  SessionId,
                                               TimeSpan?           Runtime  = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.Offline,
                                    "The EVSE or charging station is offline!",
                                    Runtime: Runtime);

        #endregion

        #region (static) Success(SessionId,          ReservationId = null, ReservationHandling = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult Success(ChargingSession_Id       SessionId,
                                               ChargingReservation_Id?  ReservationId         = null,
                                               ReservationHandling?     ReservationHandling   = null,
                                               String                   Description           = null,
                                               String                   AdditionalInfo        = null,
                                               TimeSpan?                Runtime               = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.Success,
                                    ReservationId,
                                    ReservationHandling,
                                    Description,
                                    AdditionalInfo,
                                    Runtime);

        #endregion

        #region (static) Success(ChargeDetailRecord, ReservationId = null, ReservationHandling = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        /// <param name="ChargeDetailRecord">The charge detail record for a successfully stopped charging process.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult Success(ChargeDetailRecord       ChargeDetailRecord,
                                               ChargingReservation_Id?  ReservationId         = null,
                                               ReservationHandling?     ReservationHandling   = null,
                                               String                   Description           = null,
                                               String                   AdditionalInfo        = null,
                                               TimeSpan?                Runtime               = null)

            => new RemoteStopResult(ChargeDetailRecord,
                                    RemoteStopResultTypes.Success,
                                    ReservationId,
                                    ReservationHandling,
                                    Description,
                                    AdditionalInfo,
                                    Runtime);

        #endregion

        #region (static) AlreadyStopped(SessionId, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// A previous remote stop was alredy successful.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult AlreadyStopped(ChargingSession_Id  SessionId,
                                                      String              Description      = null,
                                                      String              AdditionalInfo   = null,
                                                      TimeSpan?           Runtime          = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.AlreadyStopped,
                                    Description,
                                    AdditionalInfo,
                                    Runtime);

        #endregion

        #region (static) Timeout(SessionId, Runtime = null)

        /// <summary>
        /// The remote stop ran into a timeout.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult Timeout(ChargingSession_Id  SessionId,
                                               TimeSpan?           Runtime  = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.Timeout,
                                    Runtime: Runtime);

        #endregion

        #region (static) CommunicationError(SessionId, Message = null, Runtime = null)

        /// <summary>
        /// The remote stop led to a communication error.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Message">An optional error message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult CommunicationError(ChargingSession_Id  SessionId,
                                                          String              Message  = null,
                                                          TimeSpan?           Runtime  = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.CommunicationError,
                                    Message,
                                    Runtime: Runtime);

        #endregion

        #region (static) Error(SessionId, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult Error(ChargingSession_Id  SessionId,
                                             String              Description      = null,
                                             String              AdditionalInfo   = null,
                                             TimeSpan?           Runtime          = null)

            => new RemoteStopResult(SessionId,
                                    RemoteStopResultTypes.Error,
                                    Description,
                                    AdditionalInfo,
                                    Runtime);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {
            return Result.ToString();
        }

        #endregion

    }


    /// <summary>
    /// The result types of a remote stop operation.
    /// </summary>
    public enum RemoteStopResultTypes
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
        /// The charging location is unknown.
        /// </summary>
        UnknownLocation,

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
        /// A previous remote stop was alredy successful.
        /// </summary>
        AlreadyStopped,

        /// <summary>
        /// The remote stop ran into a timeout.
        /// </summary>
        Timeout,

        /// <summary>
        /// A communication error occured.
        /// </summary>
        CommunicationError,

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        Error

    }

}
