/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of a cancel reservation operation.
    /// </summary>
    public class CancelReservationResult
    {

        #region Properties

        /// <summary>
        /// The reservation identification.
        /// </summary>
        public ChargingReservation_Id?                ReservationId     { get; }

        /// <summary>
        /// The reservation.
        /// </summary>
        public ChargingReservation                    Reservation       { get; }

        /// <summary>
        /// A reason for the charging reservation cancellation.
        /// </summary>
        public ChargingReservationCancellationReason  Reason            { get; }

        /// <summary>
        /// The result of a cancel reservation operation.
        /// </summary>
        public CancelReservationResults               Result            { get; }


        /// <summary>
        /// An optional (error) message.
        /// </summary>
        public String                                 Message           { get; }

        /// <summary>
        /// An optional additional information on this error,
        /// e.g. the HTTP error response.
        /// </summary>
        public Object                                 AdditionalInfo    { get; }

        /// <summary>
        /// The runtime of this request.
        /// </summary>
        public TimeSpan?                              Runtime           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new cancel reservation result.
        /// </summary>
        /// <param name="Result">The result of a cancel reservation operation.</param>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The optional charging reservation.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Message">An optional message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The optional runtime of this request.</param>
        private CancelReservationResult(CancelReservationResults               Result,
                                        ChargingReservation_Id                 ReservationId,
                                        ChargingReservationCancellationReason  Reason,
                                        ChargingReservation                    Reservation      = null,
                                        String                                 Message          = null,
                                        Object                                 AdditionalInfo   = null,
                                        TimeSpan?                              Runtime          = null)
        {

            this.Result          = Result;
            this.ReservationId   = ReservationId;
            this.Reason          = Reason;
            this.Reservation     = Reservation;
            this.Message         = Message;
            this.AdditionalInfo  = AdditionalInfo;
            this.Runtime         = Runtime;

        }

        #endregion


        #region (static) UnknownOperator       (ReservationId, Reason, Runtime = null)

        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult UnknownOperator(ChargingReservation_Id                 ReservationId,
                                                              ChargingReservationCancellationReason  Reason,
                                                              TimeSpan?                              Runtime = null)

            => new CancelReservationResult(CancelReservationResults.UnknownEVSE,
                                           ReservationId,
                                           Reason,
                                           Runtime: Runtime);

        #endregion

        #region (static) UnknownReservationId  (ReservationId, Reason, Runtime = null)

        /// <summary>
        /// The given reservation identification is unknown or invalid.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult UnknownReservationId(ChargingReservation_Id                 ReservationId,
                                                                   ChargingReservationCancellationReason  Reason,
                                                                   TimeSpan?                              Runtime = null)

            => new CancelReservationResult(CancelReservationResults.UnknownReservationId,
                                           ReservationId,
                                           Reason,
                                           Runtime: Runtime);

        #endregion

        #region (static) UnknownChargingPool   (ReservationId, Reason, Runtime = null)

        /// <summary>
        /// The charging pool is unknown.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult UnknownChargingPool(ChargingReservation_Id                 ReservationId,
                                                                  ChargingReservationCancellationReason  Reason,
                                                                  TimeSpan?                              Runtime = null)

            => new CancelReservationResult(CancelReservationResults.UnknownChargingPool,
                                           ReservationId,
                                           Reason,
                                           Runtime: Runtime);

        #endregion

        #region (static) UnknownChargingStation(ReservationId, Reason, Runtime = null)

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult UnknownChargingStation(ChargingReservation_Id                 ReservationId,
                                                                     ChargingReservationCancellationReason  Reason,
                                                                     TimeSpan?                              Runtime = null)

            => new CancelReservationResult(CancelReservationResults.UnknownChargingStation,
                                           ReservationId,
                                           Reason,
                                           Runtime: Runtime);

        #endregion

        #region (static) UnknownEVSE           (ReservationId, Reason, Runtime = null)

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult UnknownEVSE(ChargingReservation_Id                 ReservationId,
                                                          ChargingReservationCancellationReason  Reason,
                                                          TimeSpan?                              Runtime = null)

            => new CancelReservationResult(CancelReservationResults.UnknownEVSE,
                                           ReservationId,
                                           Reason,
                                           Runtime: Runtime);

        #endregion

        #region (static) OutOfService          (ReservationId, Reason, Message = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The EVSE, charging station, charging pool or charging station operator is out-of-service.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult OutOfService(ChargingReservation_Id                 ReservationId,
                                                           ChargingReservationCancellationReason  Reason,
                                                           String                                 Message         = null,
                                                           String                                 AdditionalInfo  = null,
                                                           TimeSpan?                              Runtime         = null)

            => new CancelReservationResult(CancelReservationResults.OutOfService,
                                           ReservationId,
                                           Reason,
                                           Message:         Message,
                                           AdditionalInfo:  AdditionalInfo,
                                           Runtime:         Runtime);

        #endregion

        #region (static) Offline               (ReservationId, Reason, Message = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The EVSE, charging station, charging pool or charging station operator is offline.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult Offline(ChargingReservation_Id                 ReservationId,
                                                      ChargingReservationCancellationReason  Reason,
                                                      String                                 Message         = null,
                                                      String                                 AdditionalInfo  = null,
                                                      TimeSpan?                              Runtime         = null)

            => new CancelReservationResult(CancelReservationResults.Offline,
                                           ReservationId,
                                           Reason,
                                           Message:         Message,
                                           AdditionalInfo:  AdditionalInfo,
                                           Runtime:         Runtime);

        #endregion

        #region (static) Success               (ReservationId, Reason, Reservation = null, Runtime = null)

        /// <summary>
        /// The cancel reservation request was successful.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Reservation">A charging reservation.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult Success(ChargingReservation_Id                 ReservationId,
                                                      ChargingReservationCancellationReason  Reason,
                                                      ChargingReservation                    Reservation  = null,
                                                      TimeSpan?                              Runtime      = null)

            => new CancelReservationResult(CancelReservationResults.Success,
                                           ReservationId,
                                           Reason,
                                           Reservation,
                                           Runtime: Runtime);

        #endregion

        #region (static) Timeout               (ReservationId, Reason, Message = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The cancel reservation request ran into a timeout.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult Timeout(ChargingReservation_Id                 ReservationId,
                                                      ChargingReservationCancellationReason  Reason,
                                                      String                                 Message         = null,
                                                      String                                 AdditionalInfo  = null,
                                                      TimeSpan?                              Runtime         = null)

            => new CancelReservationResult(CancelReservationResults.Timeout,
                                           ReservationId,
                                           Reason,
                                           Message:         Message,
                                           AdditionalInfo:  AdditionalInfo,
                                           Runtime:         Runtime);

        #endregion

        #region (static) CommunicationError    (ReservationId, Reason, Message = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// A communication error occured.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult CommunicationError(ChargingReservation_Id                 ReservationId,
                                                                 ChargingReservationCancellationReason  Reason,
                                                                 String                                 Message         = null,
                                                                 String                                 AdditionalInfo  = null,
                                                                 TimeSpan?                              Runtime         = null)

            => new CancelReservationResult(CancelReservationResults.CommunicationError,
                                           ReservationId,
                                           Reason,
                                           Message:         Message,
                                           AdditionalInfo:  AdditionalInfo,
                                           Runtime:         Runtime);

        #endregion

        #region (static) Error                 (ReservationId, Reason, Message = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote stop request led to an error.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        /// <param name="Reason">A reason for the charging reservation cancellation.</param>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The optional runtime of the reequest.</param>
        public static CancelReservationResult Error(ChargingReservation_Id                 ReservationId,
                                                    ChargingReservationCancellationReason  Reason,
                                                    String                                 Message         = null,
                                                    String                                 AdditionalInfo  = null,
                                                    TimeSpan?                              Runtime         = null)

            => new CancelReservationResult(CancelReservationResults.Error,
                                           ReservationId,
                                           Reason,
                                           Message:         Message,
                                           AdditionalInfo:  AdditionalInfo,
                                           Runtime:         Runtime);

        #endregion

    }


    /// <summary>
    /// The result types of a cancel reservation operation.
    /// </summary>
    public enum CancelReservationResults
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
        /// The given reservation identification is unknown or invalid.
        /// </summary>
        UnknownReservationId,

        /// <summary>
        /// The charging pool is unknown.
        /// </summary>
        UnknownChargingPool,

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        UnknownChargingStation,

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        UnknownEVSE,

        /// <summary>
        /// The EVSE, charging station, charging pool or charging station operator is out-of-service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The EVSE, charging station, charging pool or charging station operator is offline.
        /// </summary>
        Offline,


        /// <summary>
        /// The cancel reservation request was successful.
        /// </summary>
        Success,

        /// <summary>
        /// The cancel reservation request ran into a timeout.
        /// </summary>
        Timeout,

        /// <summary>
        /// A communication error occured.
        /// </summary>
        CommunicationError,

        /// <summary>
        /// The remote stop request led to an error.
        /// </summary>
        Error,

    }

}
