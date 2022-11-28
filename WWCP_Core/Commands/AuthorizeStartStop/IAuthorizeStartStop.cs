﻿/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The interface for sending AuthStart/-Stop requests.
    /// </summary>
    public interface IAuthorizeStartStop
    {

        IId AuthId { get; }


        #region AuthorizeStart

        /// <summary>
        /// Create an authorize start request at the given charging location.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingLocation             ChargingLocation      = null,
                           ChargingProduct              ChargingProduct       = null,
                           ChargingSession_Id?          SessionId             = null,
                           ChargingSession_Id?          CPOPartnerSessionId   = null,
                           ChargingStationOperator_Id?  OperatorId            = null,

                           DateTime?                    Timestamp             = null,
                           CancellationToken?           CancellationToken     = null,
                           EventTracking_Id             EventTrackingId       = null,
                           TimeSpan?                    RequestTimeout        = null);

        #endregion

        #region AuthorizeStop

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingLocation             ChargingLocation      = null,
                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                          ChargingStationOperator_Id?  OperatorId            = null,

                          DateTime?                    Timestamp             = null,
                          CancellationToken?           CancellationToken     = null,
                          EventTracking_Id             EventTrackingId       = null,
                          TimeSpan?                    RequestTimeout        = null);

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging.
        /// </summary>
        event OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        event OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion


        //#region AuthorizeStart

        ///// <summary>
        ///// Create an authorize start request.
        ///// </summary>
        ///// <param name="ChargingStationOperatorId">A charging station operator identification.</param>
        ///// <param name="AuthToken">A (RFID) user identification.</param>
        ///// <param name="ChargingProductId">An optional charging product identification.</param>
        ///// <param name="SessionId">An optional session identification.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<AuthStartResult>

        //    AuthorizeStart(ChargingStationOperator_Id  ChargingStationOperatorId,
        //                   AuthenticationToken                  AuthToken,
        //                   ChargingProduct_Id?         ChargingProductId  = null,
        //                   ChargingSession_Id?         SessionId          = null,

        //                   DateTime?                   Timestamp          = null,
        //                   CancellationToken?          CancellationToken  = null,
        //                   EventTracking_Id            EventTrackingId    = null,
        //                   TimeSpan?                   RequestTimeout     = null);

        ///// <summary>
        ///// Create an authorize start request at the given EVSE.
        ///// </summary>
        ///// <param name="ChargingStationOperatorId">A charging station operator identification.</param>
        ///// <param name="AuthToken">A (RFID) user identification.</param>
        ///// <param name="EVSEId">The unique identification of an EVSE.</param>
        ///// <param name="ChargingProductId">An optional charging product identification.</param>
        ///// <param name="SessionId">An optional session identification.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<AuthStartEVSEResult>

        //    AuthorizeStart(ChargingStationOperator_Id  ChargingStationOperatorId,
        //                   AuthenticationToken                  AuthToken,
        //                   EVSE_Id                     EVSEId,
        //                   ChargingProduct_Id?         ChargingProductId  = null,
        //                   ChargingSession_Id?         SessionId          = null,

        //                   DateTime?                   Timestamp          = null,
        //                   CancellationToken?          CancellationToken  = null,
        //                   EventTracking_Id            EventTrackingId    = null,
        //                   TimeSpan?                   RequestTimeout     = null);

        ///// <summary>
        ///// Create an authorize start request at the given charging station.
        ///// </summary>
        ///// <param name="ChargingStationOperatorId">A charging station operator identification.</param>
        ///// <param name="AuthToken">A (RFID) user identification.</param>
        ///// <param name="ChargingStationId">The unique identification of a charging station.</param>
        ///// <param name="ChargingProductId">An optional charging product identification.</param>
        ///// <param name="SessionId">An optional session identification.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<AuthStartChargingStationResult>

        //    AuthorizeStart(ChargingStationOperator_Id   ChargingStationOperatorId,
        //                   AuthenticationToken                   AuthToken,
        //                   ChargingStation_Id           ChargingStationId,
        //                   ChargingProduct_Id?          ChargingProductId  = null,
        //                   ChargingSession_Id?          SessionId          = null,

        //                   DateTime?                    Timestamp          = null,
        //                   CancellationToken?           CancellationToken  = null,
        //                   EventTracking_Id             EventTrackingId    = null,
        //                   TimeSpan?                    RequestTimeout     = null);

        //#endregion

        //#region AuthorizeStop

        ///// <summary>
        ///// Create an authorize stop request.
        ///// </summary>
        ///// <param name="ChargingStationOperatorId">A charging station operator identification.</param>
        ///// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        ///// <param name="AuthToken">A (RFID) user identification.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<AuthStopResult>

        //    AuthorizeStop(ChargingStationOperator_Id  ChargingStationOperatorId,
        //                  ChargingSession_Id          SessionId,
        //                  AuthenticationToken                  AuthToken,

        //                  DateTime?                   Timestamp           = null,
        //                  CancellationToken?          CancellationToken   = null,
        //                  EventTracking_Id            EventTrackingId     = null,
        //                  TimeSpan?                   RequestTimeout      = null);

        ///// <summary>
        ///// Create an authorize stop request at the given EVSE.
        ///// </summary>
        ///// <param name="ChargingStationOperatorId">A charging station operator identification.</param>
        ///// <param name="EVSEId">The unique identification of an EVSE.</param>
        ///// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        ///// <param name="AuthToken">A (RFID) user identification.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<AuthStopEVSEResult>

        //    AuthorizeStop(ChargingStationOperator_Id  ChargingStationOperatorId,
        //                  EVSE_Id                     EVSEId,
        //                  ChargingSession_Id          SessionId,
        //                  AuthenticationToken                  AuthToken,

        //                  DateTime?                   Timestamp           = null,
        //                  CancellationToken?          CancellationToken   = null,
        //                  EventTracking_Id            EventTrackingId     = null,
        //                  TimeSpan?                   RequestTimeout      = null);

        ///// <summary>
        ///// Create an authorize stop request at the given charging station.
        ///// </summary>
        ///// <param name="ChargingStationOperatorId">A charging station operator identification.</param>
        ///// <param name="ChargingStationId">A charging station identification.</param>
        ///// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        ///// <param name="AuthToken">A (RFID) user identification.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<AuthStopChargingStationResult>

        //    AuthorizeStop(ChargingStationOperator_Id  ChargingStationOperatorId,
        //                  ChargingStation_Id          ChargingStationId,
        //                  ChargingSession_Id          SessionId,
        //                  AuthenticationToken                  AuthToken,

        //                  DateTime?                   Timestamp           = null,
        //                  CancellationToken?          CancellationToken   = null,
        //                  EventTracking_Id            EventTrackingId     = null,
        //                  TimeSpan?                   RequestTimeout      = null);

        //#endregion

    }


    /// <summary>
    /// The interface for sending AuthStart/-Stop requests.
    /// </summary>
    public interface ISendAuthorizeStartStop : IAuthorizeStartStop
    {

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        Boolean  DisableAuthentication   { get; set; }

    }



    /// <summary>
    /// The interface for receiving AuthStart/-Stop requests.
    /// </summary>
    public interface IReceiveAuthorizeStartStop : IAuthorizeStartStop
    {


    }

}