/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of a reservation operation.
    /// </summary>
    public class ReservationResult
    {

        #region Properties

        /// <summary>
        /// The result of a reserve operation.
        /// </summary>
        public ReservationResultType  Result            { get; }

        /// <summary>
        /// The reservation for the reserve operation.
        /// </summary>
        public ChargingReservation    Reservation       { get; }

        /// <summary>
        /// An optional (error) message.
        /// </summary>
        public String                 Message           { get; }

        /// <summary>
        /// An optional additional information on this error,
        /// e.g. the HTTP error response.
        /// </summary>
        public Object                 AdditionalInfo    { get; }

        #endregion

        #region Constructor(s)

        #region ReservationResult(Reservation)

        /// <summary>
        /// Create a new successful reserve result.
        /// </summary>
        /// <param name="Reservation">The charging reservation.</param>
        private ReservationResult(ChargingReservation Reservation)
        {

            #region Initial checks

            if (Reservation == null)
                throw new ArgumentNullException(nameof(Reservation), "The given charging reservation must not be null!");

            #endregion

            this.Result       = ReservationResultType.Success;
            this.Reservation  = Reservation;

        }

        #endregion

        #region ReservationResult(Result, Message = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new reserve result.
        /// </summary>
        /// <param name="Result">The result of the reserve operation.</param>
        /// <param name="Message">An optional message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        private ReservationResult(ReservationResultType  Result,
                                  String                 Message         = null,
                                  Object                 AdditionalInfo  = null)
        {

            this.Result          = Result;
            this.Reservation     = null;
            this.Message         = Message;
            this.AdditionalInfo  = AdditionalInfo;

        }

        #endregion

        #endregion


        #region (static) UnknownChargingStationOperator

        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        public static ReservationResult UnknownChargingStationOperator

            => new ReservationResult(ReservationResultType.UnknownChargingStationOperator);

        #endregion

        #region (static) UnknownChargingReservationId

        /// <summary>
        /// The given charging reservation identification is unknown or invalid.
        /// </summary>
        public static ReservationResult UnknownChargingReservationId

            => new ReservationResult(ReservationResultType.UnknownChargingReservationId);

        #endregion

        #region (static) UnknownChargingPool

        /// <summary>
        /// The charging pool is unknown.
        /// </summary>
        public static ReservationResult UnknownChargingPool

            => new ReservationResult(ReservationResultType.UnknownChargingPool);

        #endregion

        #region (static) UnknownChargingStation

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        public static ReservationResult UnknownChargingStation

            => new ReservationResult(ReservationResultType.UnknownChargingStation);

        #endregion

        #region (static) UnknownEVSE

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        public static ReservationResult UnknownEVSE

            => new ReservationResult(ReservationResultType.UnknownEVSE);

        #endregion

        #region (static) AlreadyInUse

        /// <summary>
        /// The EVSE is already in use.
        /// </summary>
        public static ReservationResult AlreadyInUse

            => new ReservationResult(ReservationResultType.AlreadyInUse);

        #endregion

        #region (static) AlreadyReserved

        /// <summary>
        /// The EVSE is already reserved.
        /// </summary>
        public static ReservationResult AlreadyReserved

            => new ReservationResult(ReservationResultType.AlreadyReserved);

        #endregion

        #region (static) InvalidCredentials

        /// <summary>
        /// Unauthorized remote start or invalid credentials.
        /// </summary>
        public static ReservationResult InvalidCredentials

            => new ReservationResult(ReservationResultType.InvalidCredentials);

        #endregion

        #region (static) InternalUse

        /// <summary>
        /// The EVSE, charging station or charging pool is reserved for internal use.
        /// </summary>
        public static ReservationResult InternalUse

            => new ReservationResult(ReservationResultType.InternalUse);

        #endregion

        #region (static) OutOfService

        /// <summary>
        /// The EVSE, charging station, charging pool or Charging Station Operator is out-of-service.
        /// </summary>
        public static ReservationResult OutOfService

            => new ReservationResult(ReservationResultType.OutOfService);

        #endregion

        #region (static) Offline

        /// <summary>
        /// The EVSE, charging station, charging pool or Charging Station Operator is offline.
        /// </summary>
        public static ReservationResult Offline

            => new ReservationResult(ReservationResultType.Offline);

        #endregion

        #region (static) NoEVSEsAvailable

        /// <summary>
        /// No EVSEs are available for reservation.
        /// </summary>
        public static ReservationResult NoEVSEsAvailable

            => new ReservationResult(ReservationResultType.NoEVSEsAvailable);

        #endregion

        #region (static) Success()

        /// <summary>
        /// The reservation was successful.
        /// </summary>
        public static ReservationResult Success()

            => new ReservationResult(ReservationResultType.Success);

        #endregion

        #region (static) Success(Reservation)

        /// <summary>
        /// The reservation was successful.
        /// </summary>
        public static ReservationResult Success(ChargingReservation Reservation)

            => new ReservationResult(Reservation);

        #endregion

        #region (static) Timeout

        /// <summary>
        /// The reservation request ran into a timeout.
        /// </summary>
        public static ReservationResult Timeout

            => new ReservationResult(ReservationResultType.Timeout);

        #endregion

        #region (static) CommunicationError(Message = "", AdditionalInfo = null)

        /// <summary>
        /// A communication error occured.
        /// </summary>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        public static ReservationResult CommunicationError(String Message         = null,
                                                           Object AdditionalInfo  = null)

            => new ReservationResult(ReservationResultType.CommunicationError,
                                     Message,
                                     AdditionalInfo);

        #endregion

        #region (static) Error(Message = null, AdditionalInfo = null)

        /// <summary>
        /// The reservation request led to an error.
        /// </summary>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        public static ReservationResult Error(String Message         = null,
                                              Object AdditionalInfo  = null)

            => new ReservationResult(ReservationResultType.Error,
                                     Message,
                                     AdditionalInfo);

        #endregion

    }


    /// <summary>
    /// The result types of a reservation operation.
    /// </summary>
    public enum ReservationResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,


        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        UnknownChargingStationOperator,

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
        /// The given charging reservation identification is unknown or invalid.
        /// </summary>
        UnknownChargingReservationId,

        /// <summary>
        /// Unauthorized reservation or invalid credentials.
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// The EVSE is already in use.
        /// </summary>
        AlreadyInUse,

        /// <summary>
        /// The EVSE is already reserved.
        /// </summary>
        AlreadyReserved,


        /// <summary>
        /// The EVSE, charging station or charging pool is reserved for internal use.
        /// </summary>
        InternalUse,

        /// <summary>
        /// The EVSE, charging station, charging pool or Charging Station Operator is out-of-service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The EVSE, charging station, charging pool or Charging Station Operator is offline.
        /// </summary>
        Offline,

        /// <summary>
        /// No EVSEs are available for reservation.
        /// </summary>
        NoEVSEsAvailable,


        /// <summary>
        /// The reservation was successful.
        /// </summary>
        Success,


        /// <summary>
        /// The reservation request ran into a timeout.
        /// </summary>
        Timeout,

        /// <summary>
        /// A communication error occured.
        /// </summary>
        CommunicationError,

        /// <summary>
        /// The reservation request led to an error.
        /// </summary>
        Error

    }

}
