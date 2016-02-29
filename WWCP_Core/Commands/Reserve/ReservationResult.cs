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
    /// The result of a reserve operation.
    /// </summary>
    public class ReservationResult
    {

        #region Properties

        #region Result

        private readonly ReservationResultType _Result;

        /// <summary>
        /// The result of a reserve operation.
        /// </summary>
        public ReservationResultType Result
        {
            get
            {
                return _Result;
            }
        }

        #endregion

        #region Reservation

        private readonly ChargingReservation _Reservation;

        /// <summary>
        /// The reservation for the reserve operation.
        /// </summary>
        public ChargingReservation Reservation
        {
            get
            {
                return _Reservation;
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

        #region ReservationResult(Result, Message = null)

        /// <summary>
        /// Create a new reserve result.
        /// </summary>
        /// <param name="Result">The result of the reserve operation.</param>
        /// <param name="Message">An optional message.</param>
        private ReservationResult(ReservationResultType  Result,
                                  String                 Message = null)
        {

            this._Result       = Result;
            this._Reservation  = null;
            this._Message      = Message;

        }

        #endregion

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

            this._Result       = ReservationResultType.Success;
            this._Reservation  = Reservation;

        }

        #endregion

        #region ReservationResult(Message)

        /// <summary>
        /// Create a new reserve result.
        /// </summary>
        /// <param name="Message">An (error) message.</param>
        private ReservationResult(String Message)
        {

            this._Result   = ReservationResultType.Error;
            this._Message  = Message;

        }

        #endregion

        #endregion


        #region (static) UnknownEVSEOperator

        /// <summary>
        /// The EVSE operator is unknown.
        /// </summary>
        public static ReservationResult UnknownEVSEOperator
        {
            get
            {
                return new ReservationResult(ReservationResultType.UnknownEVSE);
            }
        }

        #endregion

        #region (static) UnknownChargingReservationId

        /// <summary>
        /// The given charging reservation identification is unknown or invalid.
        /// </summary>
        public static ReservationResult UnknownChargingReservationId
        {
            get
            {
                return new ReservationResult(ReservationResultType.UnknownChargingReservationId);
            }
        }

        #endregion

        #region (static) UnknownChargingPool

        /// <summary>
        /// The charging pool is unknown.
        /// </summary>
        public static ReservationResult UnknownChargingPool
        {
            get
            {
                return new ReservationResult(ReservationResultType.UnknownChargingPool);
            }
        }

        #endregion

        #region (static) UnknownChargingStation

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        public static ReservationResult UnknownChargingStation
        {
            get
            {
                return new ReservationResult(ReservationResultType.UnknownChargingStation);
            }
        }

        #endregion

        #region (static) UnknownEVSE

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        public static ReservationResult UnknownEVSE
        {
            get
            {
                return new ReservationResult(ReservationResultType.UnknownEVSE);
            }
        }

        #endregion

        #region (static) AlreadyInUse

        /// <summary>
        /// The EVSE is already in use.
        /// </summary>
        public static ReservationResult AlreadyInUse
        {
            get
            {
                return new ReservationResult(ReservationResultType.AlreadyInUse);
            }
        }

        #endregion

        #region (static) AlreadyReserved

        /// <summary>
        /// The EVSE is already reserved.
        /// </summary>
        public static ReservationResult AlreadyReserved
        {
            get
            {
                return new ReservationResult(ReservationResultType.AlreadyReserved);
            }
        }

        #endregion

        #region (static) InvalidCredentials

        /// <summary>
        /// Unauthorized remote start or invalid credentials.
        /// </summary>
        public static ReservationResult InvalidCredentials
        {
            get
            {
                return new ReservationResult(ReservationResultType.InvalidCredentials);
            }
        }

        #endregion

        #region (static) InternalUse

        /// <summary>
        /// The EVSE is reserved for internal use.
        /// </summary>
        public static ReservationResult InternalUse
        {
            get
            {
                return new ReservationResult(ReservationResultType.InternalUse);
            }
        }

        #endregion

        #region (static) OutOfService

        /// <summary>
        /// The EVSE is Out-of-Service.
        /// </summary>
        public static ReservationResult OutOfService
        {
            get
            {
                return new ReservationResult(ReservationResultType.OutOfService);
            }
        }

        #endregion

        #region (static) Offline

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        public static ReservationResult Offline
        {
            get
            {
                return new ReservationResult(ReservationResultType.Offline);
            }
        }

        #endregion

        #region (static) NoEVSEsAvailable

        /// <summary>
        /// No EVSEs are available for reservation.
        /// </summary>
        public static ReservationResult NoEVSEsAvailable
        {
            get
            {
                return new ReservationResult(ReservationResultType.NoEVSEsAvailable);
            }
        }

        #endregion

        #region (static) Success()

        /// <summary>
        /// The reservation was successful.
        /// </summary>
        public static ReservationResult Success()
        {
            return new ReservationResult(ReservationResultType.Success);
        }

        #endregion

        #region (static) Success(Reservation)

        /// <summary>
        /// The reservation was successful.
        /// </summary>
        public static ReservationResult Success(ChargingReservation Reservation)
        {
            return new ReservationResult(Reservation);
        }

        #endregion

        #region (static) Timeout

        /// <summary>
        /// The reservation ran into a timeout.
        /// </summary>
        public static ReservationResult Timeout
        {
            get
            {
                return new ReservationResult(ReservationResultType.Timeout);
            }
        }

        #endregion

        #region (static) CommunicationError(Message = "")

        /// <summary>
        /// A communication error occured.
        /// </summary>
        /// <param name="Message">An optional (error)message.</param>
        public static ReservationResult CommunicationError(String Message = "")
        {

            return new ReservationResult(ReservationResultType.CommunicationError,
                                         Message);

        }

        #endregion

        #region (static) Error(Message = "")

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        public static ReservationResult Error(String Message = "")
        {
            return new ReservationResult(Message);
        }

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
        /// The EVSE operator is unknown.
        /// </summary>
        UnknownEVSEOperator,

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
        /// The EVSE is reserved for internal use.
        /// </summary>
        InternalUse,

        /// <summary>
        /// The EVSE is Out-of-Service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The EVSE is offline.
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
        /// The reservation ran into a timeout.
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
