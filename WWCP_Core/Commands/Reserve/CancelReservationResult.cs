/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
        /// The result of a cancel reservation operation.
        /// </summary>
        public CancelReservationResultType  Result            { get; }

        /// <summary>
        /// The reservation identification.
        /// </summary>
        public ChargingReservation_Id?      ReservationId     { get; }

        /// <summary>
        /// An optional (error) message.
        /// </summary>
        public String                       Message           { get; }

        /// <summary>
        /// An optional additional information on this error,
        /// e.g. the HTTP error response.
        /// </summary>
        public Object                       AdditionalInfo    { get; }

        #endregion

        #region Constructor(s)

        #region CancelReservationResult(Result, ReservationId)

        /// <summary>
        /// Create a new cancel reservation result.
        /// </summary>
        /// <param name="Result">The result of a cancel reservation operation.</param>
        /// <param name="ReservationId">The charging reservation identification.</param>
        private CancelReservationResult(CancelReservationResultType  Result,
                                        ChargingReservation_Id       ReservationId)
        {

            this.Result         = Result;
            this.ReservationId  = ReservationId;

        }

        #endregion

        #region CancelReservationResult(Result, Message = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new cancel reservation result.
        /// </summary>
        /// <param name="Result">The result of the cancel reservation operation.</param>
        /// <param name="Message">An optional message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        private CancelReservationResult(CancelReservationResultType  Result,
                                        String                       Message         = null,
                                        Object                       AdditionalInfo  = null)
        {

            this.Result          = Result;
            this.ReservationId   = default(ChargingReservation_Id);
            this.Message         = Message;
            this.AdditionalInfo  = AdditionalInfo;

        }

        #endregion

        #endregion


        #region (static) UnknownOperator

        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        public static CancelReservationResult UnknownOperator

            => new CancelReservationResult(CancelReservationResultType.UnknownEVSE);

        #endregion

        #region (static) UnknownReservationId

        /// <summary>
        /// The given reservation identification is unknown or invalid.
        /// </summary>
        /// <param name="ReservationId">A reservation identification.</param>
        public static CancelReservationResult UnknownReservationId(ChargingReservation_Id ReservationId)

            => new CancelReservationResult(CancelReservationResultType.UnknownReservationId,
                                           ReservationId);

        #endregion

        #region (static) UnknownChargingPool

        /// <summary>
        /// The charging pool is unknown.
        /// </summary>
        public static CancelReservationResult UnknownChargingPool

            => new CancelReservationResult(CancelReservationResultType.UnknownChargingPool);

        #endregion

        #region (static) UnknownChargingStation

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        public static CancelReservationResult UnknownChargingStation

            => new CancelReservationResult(CancelReservationResultType.UnknownChargingStation);

        #endregion

        #region (static) UnknownEVSE

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        public static CancelReservationResult UnknownEVSE

            => new CancelReservationResult(CancelReservationResultType.UnknownEVSE);

        #endregion

        #region (static) OutOfService

        /// <summary>
        /// The EVSE, charging station, charging pool or charging station operator is out-of-service.
        /// </summary>
        public static CancelReservationResult OutOfService

            => new CancelReservationResult(CancelReservationResultType.OutOfService);

        #endregion

        #region (static) Offline

        /// <summary>
        /// The EVSE, charging station, charging pool or charging station operator is offline.
        /// </summary>
        public static CancelReservationResult Offline

            => new CancelReservationResult(CancelReservationResultType.Offline);

        #endregion

        #region (static) Success(ReservationId)

        /// <summary>
        /// The cancel reservation request was successful.
        /// </summary>
        /// <param name="ReservationId">The reservation identification.</param>
        public static CancelReservationResult Success(ChargingReservation_Id ReservationId)

            => new CancelReservationResult(CancelReservationResultType.Success,
                                           ReservationId);

        #endregion

        #region (static) Timeout

        /// <summary>
        /// The cancel reservation request ran into a timeout.
        /// </summary>
        public static CancelReservationResult Timeout

            => new CancelReservationResult(CancelReservationResultType.Timeout);

        #endregion

        #region (static) CommunicationError(Message = "")

        /// <summary>
        /// A communication error occured.
        /// </summary>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        public static CancelReservationResult CommunicationError(String Message         = null,
                                                                 Object AdditionalInfo  = null)

            => new CancelReservationResult(CancelReservationResultType.CommunicationError,
                                           Message,
                                           AdditionalInfo);

        #endregion

        #region (static) Error(Message = null, AdditionalInfo = null)

        /// <summary>
        /// The remote stop request led to an error.
        /// </summary>
        /// <param name="Message">An optional (error-)message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        public static CancelReservationResult Error(String Message         = null,
                                                    Object AdditionalInfo  = null)

            => new CancelReservationResult(CancelReservationResultType.Error,
                                           Message,
                                           AdditionalInfo);

        #endregion

    }


    /// <summary>
    /// The result types of a cancel reservation operation.
    /// </summary>
    public enum CancelReservationResultType
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
