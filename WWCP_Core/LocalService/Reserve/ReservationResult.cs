/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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

namespace org.GraphDefined.WWCP.LocalService
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

        #region (static) Error

        public static ReservationResult Error
        {
            get
            {
                return new ReservationResult(ReservationResultType.Error);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public ReservationResult(ReservationResultType  Result,
                                 ChargingReservation    Reservation = null)
        {

            this._Result       = Result;
            this._Reservation  = Reservation;

        }

        #endregion

    }


    public enum ReservationResultType
    {

        Error,
        Timeout,

        Success,

        ReservationId_AlreadyInUse,

        UnknownChargingPool,
        UnknownChargingStation,
        UnknownEVSE,

        ChargingPool_AlreadyReserved,
        ChargingStation_AlreadyReserved,
        EVSE_AlreadyReserved,

        ChargingPool_AlreadyInUse,
        ChargingStation_AlreadyInUse,
        EVSE_AlreadyInUse,

        ChargingPool_IsOutOfService,
        ChargingStation_IsOutOfService,
        EVSE_IsOutOfService

    }

}
