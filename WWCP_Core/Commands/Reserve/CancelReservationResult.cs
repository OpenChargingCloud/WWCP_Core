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
    /// The result of a cancel reservation operation.
    /// </summary>
    public class CancelReservationResult
    {

        #region Properties

        #region Result

        private readonly CancelReservationResultType _Result;

        public CancelReservationResultType Result
        {
            get
            {
                return _Result;
            }
        }

        #endregion

        #region ReservationId

        private readonly ChargingReservation_Id _ReservationId;

        public ChargingReservation_Id ReservationId
        {
            get
            {
                return _ReservationId;
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

        #region Constructor(s)

        #region CancelReservationResult(Result, Message = null)

        /// <summary>
        /// Create a new cancel reservation result.
        /// </summary>
        /// <param name="Result">The result of the cancel reservation operation.</param>
        /// <param name="Message">An optional message.</param>
        private CancelReservationResult(CancelReservationResultType  Result,
                                        String                       Message = null)
        {

            this._Result         = Result;
            this._ReservationId  = null;
            this._Message        = Message;

        }

        #endregion

        #region CancelReservationResult(Result, ReservationId)

        /// <summary>
        /// Create a new cancel reservation result.
        /// </summary>
        /// <param name="Result">The result of a cancel reservation operation.</param>
        /// <param name="ReservationId">The charging reservation identification.</param>
        private CancelReservationResult(CancelReservationResultType  Result,
                                        ChargingReservation_Id       ReservationId)
        {

            this._Result         = Result;
            this._ReservationId  = ReservationId;

        }

        #endregion

        #region CancelReservationResult(Message)

        /// <summary>
        /// Create a new cancel reservation result.
        /// </summary>
        /// <param name="Message">An (error) message.</param>
        private CancelReservationResult(String Message)
        {

            this._Result   = CancelReservationResultType.Error;
            this._Message  = Message;

        }

        #endregion

        #endregion


        #region (static) UnknownEVSEOperator

        public static CancelReservationResult UnknownEVSEOperator
        {
            get
            {
                return new CancelReservationResult(CancelReservationResultType.UnknownEVSE);
            }
        }

        #endregion

        #region (static) UnknownReservationId

        public static CancelReservationResult UnknownReservationId(ChargingReservation_Id ReservationId)
        {
            return new CancelReservationResult(CancelReservationResultType.UnknownReservationId,
                                               ReservationId);
        }

        #endregion

        #region (static) UnknownChargingPool

        public static CancelReservationResult UnknownChargingPool
        {
            get
            {
                return new CancelReservationResult(CancelReservationResultType.UnknownChargingPool);
            }
        }

        #endregion

        #region (static) UnknownChargingStation

        public static CancelReservationResult UnknownChargingStation
        {
            get
            {
                return new CancelReservationResult(CancelReservationResultType.UnknownChargingStation);
            }
        }

        #endregion

        #region (static) UnknownEVSE

        public static CancelReservationResult UnknownEVSE
        {
            get
            {
                return new CancelReservationResult(CancelReservationResultType.UnknownEVSE);
            }
        }

        #endregion

        #region (static) OutOfService

        public static CancelReservationResult OutOfService
        {
            get
            {
                return new CancelReservationResult(CancelReservationResultType.OutOfService);
            }
        }

        #endregion

        #region (static) Offline

        public static CancelReservationResult Offline
        {
            get
            {
                return new CancelReservationResult(CancelReservationResultType.Offline);
            }
        }

        #endregion

        #region (static) NoEVSEsAvailable

        public static CancelReservationResult NoEVSEsAvailable
        {
            get
            {
                return new CancelReservationResult(CancelReservationResultType.NoEVSEsAvailable);
            }
        }

        #endregion

        #region (static) Success(ReservationId)

        public static CancelReservationResult Success(ChargingReservation_Id ReservationId)
        {
            return new CancelReservationResult(CancelReservationResultType.Success,
                                               ReservationId);
        }

        #endregion

        #region (static) Timeout

        public static CancelReservationResult Timeout
        {
            get
            {
                return new CancelReservationResult(CancelReservationResultType.Timeout);
            }
        }

        #endregion

        #region (static) CommunicationError(Message = "")

        /// <summary>
        /// A communication error occured.
        /// </summary>
        /// <param name="Message">An optional (error)message.</param>
        public static CancelReservationResult CommunicationError(String Message = "")
        {

            return new CancelReservationResult(CancelReservationResultType.CommunicationError,
                                               Message);

        }

        #endregion

        #region (static) Error(Message = "")

        public static CancelReservationResult Error(String Message = "")
        {
            return new CancelReservationResult(Message);
        }

        #endregion

    }


    public enum CancelReservationResultType
    {

        Unspecified,

        UnknownEVSEOperator,
        UnknownReservationId,
        UnknownChargingPool,
        UnknownChargingStation,
        UnknownEVSE,

        OutOfService,
        Offline,

        NoEVSEsAvailable,

        Success,

        Timeout,
        CommunicationError,
        Error,

    }

}
