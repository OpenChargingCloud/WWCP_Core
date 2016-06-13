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
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Illias;
using System.Threading;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The Roaming Provider provided eMobility services interface.
    /// </summary>
    public interface IEMPRoamingProvider
    {

       // Authorizator_Id AuthorizatorId { get; }

        #region Properties

        /// <summary>
        /// The unique identification of the roaming provider.
        /// </summary>
        RoamingProvider_Id Id                { get; }

        /// <summary>
        /// The offical (multi-language) name of the roaming provider.
        /// </summary>
        I18NString         Name              { get; }

        /// <summary>
        /// The attached roaming network.
        /// </summary>
        RoamingNetwork     RoamingNetwork    { get; }

        #endregion

        #region Events

        // Client methods (logging)


        // Server methods

        #region OnAuthorizeStart/-Stop

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        event OnAuthorizeStartEVSEDelegate  OnAuthorizeStartEVSE;

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        event OnAuthorizeStopEVSEDelegate   OnAuthorizeStopEVSE;

        #endregion

        #region OnChargeDetailRecord

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        event OnChargeDetailRecordDelegate OnChargeDetailRecord;

        #endregion

        #endregion


        #region Reservations

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<ReservationResult> Reserve(DateTime                 Timestamp,
                                        CancellationToken        CancellationToken,
                                        EventTracking_Id         EventTrackingId,
                                        EVSE_Id                  EVSEId,
                                        DateTime?                StartTime          = null,
                                        TimeSpan?                Duration           = null,
                                        ChargingReservation_Id   ReservationId      = null,
                                        EVSP_Id                  ProviderId         = null,
                                        eMA_Id                   eMAId              = null,
                                        ChargingProduct_Id       ChargingProductId  = null,
                                        IEnumerable<Auth_Token>  AuthTokens         = null,
                                        IEnumerable<eMA_Id>      eMAIds             = null,
                                        IEnumerable<UInt32>      PINs               = null,
                                        TimeSpan?                QueryTimeout       = null);


        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="EVSEId">An optional identification of the EVSE.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<CancelReservationResult> CancelReservation(DateTime                               Timestamp,
                                                        CancellationToken                      CancellationToken,
                                                        EventTracking_Id                       EventTrackingId,
                                                        ChargingReservation_Id                 ReservationId,
                                                        ChargingReservationCancellationReason  Reason,
                                                        EVSP_Id                                ProviderId    = null,
                                                        EVSE_Id                                EVSEId        = null,
                                                        TimeSpan?                              QueryTimeout  = null);

        #endregion

        #region RemoteStart/-Stop

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<RemoteStartEVSEResult> RemoteStart(DateTime                Timestamp,
                                                CancellationToken       CancellationToken,
                                                EventTracking_Id        EventTrackingId,
                                                EVSE_Id                 EVSEId,
                                                ChargingProduct_Id      ChargingProductId  = null,
                                                ChargingReservation_Id  ReservationId      = null,
                                                ChargingSession_Id      SessionId          = null,
                                                EVSP_Id                 ProviderId         = null,
                                                eMA_Id                  eMAId              = null,
                                                TimeSpan?               QueryTimeout       = null);


        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<RemoteStopEVSEResult> RemoteStop(DateTime             Timestamp,
                                              CancellationToken    CancellationToken,
                                              EventTracking_Id     EventTrackingId,
                                              EVSE_Id              EVSEId,
                                              ChargingSession_Id   SessionId,
                                              ReservationHandling  ReservationHandling,
                                              EVSP_Id              ProviderId    = null,
                                              eMA_Id               eMAId         = null,
                                              TimeSpan?            QueryTimeout  = null);

        #endregion

        #region GetChargeDetailRecords(From, To, ProviderId = null, QueryTimeout = null)

        /// <summary>
        /// Download all charge detail records from the OICP server.
        /// </summary>
        /// <param name="From">The starting time.</param>
        /// <param name="To">The end time.</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<IEnumerable<ChargeDetailRecord>> GetChargeDetailRecords(DateTime           Timestamp,
                                                                     CancellationToken  CancellationToken,
                                                                     EventTracking_Id   EventTrackingId,
                                                                     DateTime           From,
                                                                     DateTime           To,
                                                                     EVSP_Id            ProviderId    = null,
                                                                     TimeSpan?          QueryTimeout  = null);

        #endregion

    }

}
