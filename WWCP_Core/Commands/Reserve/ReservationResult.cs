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

    public class ReservationResult
    {

        #region Properties

        #region Result

        private readonly ReservationResultType _Result;

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

        public String Message
        {
            get
            {
                return _Message;
            }
        }

        #endregion

        #endregion


        #region (static) UnknownEVSEOperator

        public static ReservationResult UnknownEVSEOperator
        {
            get
            {
                return new ReservationResult(ReservationResultType.UnknownEVSE);
            }
        }

        #endregion

        #region (static) UnknownChargingReservationId

        public static ReservationResult UnknownChargingReservationId
        {
            get
            {
                return new ReservationResult(ReservationResultType.UnknownChargingReservationId);
            }
        }

        #endregion

        #region (static) UnknownChargingPool

        public static ReservationResult UnknownChargingPool
        {
            get
            {
                return new ReservationResult(ReservationResultType.UnknownChargingPool);
            }
        }

        #endregion

        #region (static) UnknownChargingStation

        public static ReservationResult UnknownChargingStation
        {
            get
            {
                return new ReservationResult(ReservationResultType.UnknownChargingStation);
            }
        }

        #endregion

        #region (static) UnknownEVSE

        public static ReservationResult UnknownEVSE
        {
            get
            {
                return new ReservationResult(ReservationResultType.UnknownEVSE);
            }
        }

        #endregion

        #region (static) AlreadyInUse

        public static ReservationResult AlreadyInUse
        {
            get
            {
                return new ReservationResult(ReservationResultType.AlreadyInUse);
            }
        }

        #endregion

        #region (static) AlreadyReserved

        public static ReservationResult AlreadyReserved
        {
            get
            {
                return new ReservationResult(ReservationResultType.AlreadyReserved);
            }
        }

        #endregion

        #region (static) OutOfService

        public static ReservationResult OutOfService
        {
            get
            {
                return new ReservationResult(ReservationResultType.OutOfService);
            }
        }

        #endregion

        #region (static) Offline

        public static ReservationResult Offline
        {
            get
            {
                return new ReservationResult(ReservationResultType.Offline);
            }
        }

        #endregion

        #region (static) NoEVSEsAvailable

        public static ReservationResult NoEVSEsAvailable
        {
            get
            {
                return new ReservationResult(ReservationResultType.NoEVSEsAvailable);
            }
        }

        #endregion

        #region (static) Success(Reservation)

        public static ReservationResult Success(ChargingReservation Reservation)
        {
            return new ReservationResult(Reservation);
        }

        #endregion

        #region (static) Timeout

        public static ReservationResult Timeout
        {
            get
            {
                return new ReservationResult(ReservationResultType.Timeout);
            }
        }

        #endregion

        #region (static) Error(Message = "")

        public static ReservationResult Error(String Message = "")
        {
            return new ReservationResult(Message);
        }

        #endregion


        #region Constructor(s)

        #region ReservationResult(Result)

        private ReservationResult(ReservationResultType  Result)
        {

            this._Result       = Result;
            this._Reservation  = null;

        }

        #endregion

        #region ReservationResult(Reservation)

        private ReservationResult(ChargingReservation Reservation)
        {

            this._Result       = ReservationResultType.Success;
            this._Reservation  = Reservation;

        }

        #endregion

        #region ReservationResult(Message = "")

        private ReservationResult(String Message = "")
        {

            this._Result   = ReservationResultType.Error;
            this._Message  = Message;

        }

        #endregion

        #endregion

    }


    public enum ReservationResultType
    {

        Unspecified,

        UnknownEVSEOperator,
        UnknownChargingReservationId,
        UnknownChargingPool,
        UnknownChargingStation,
        UnknownEVSE,

        AlreadyInUse,
        AlreadyReserved,
        OutOfService,
        Offline,

        NoEVSEsAvailable,

        Success,

        Timeout,
        Error,

    }

}
