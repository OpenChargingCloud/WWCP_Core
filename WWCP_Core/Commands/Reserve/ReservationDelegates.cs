/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    #region Reserve EVSEs

    /// <summary>
    /// An event send whenever a reservation at an EVSE is being made.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="EVSEId">The uniuqe identification of the reserved EVSE.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnEVSEReserveDelegate(Object                   Sender,
                                               DateTime                 Timestamp,
                                               EventTracking_Id         EventTrackingId,
                                               RoamingNetwork_Id        RoamingNetworkId,
                                               ChargingReservation_Id   ReservationId,
                                               EVSE_Id                  EVSEId,
                                               DateTime?                StartTime,
                                               TimeSpan?                Duration,
                                               EVSP_Id                  ProviderId,
                                               eMA_Id                   eMAId,
                                               ChargingProduct_Id       ChargingProductId,
                                               IEnumerable<Auth_Token>  AuthTokens,
                                               IEnumerable<eMA_Id>      eMAIds,
                                               IEnumerable<UInt32>      PINs,
                                               TimeSpan?                QueryTimeout);


    /// <summary>
    /// An event send whenever a reservation at an EVSE is being made.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="EVSEId">The uniuqe identification of the reserved EVSE.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<ReservationResult> OnReserveEVSEDelegate(DateTime                 Timestamp,
                                                                  CancellationToken        CancellationToken,
                                                                  EventTracking_Id         EventTrackingId,
                                                                  RoamingNetwork_Id        RoamingNetworkId,
                                                                  ChargingReservation_Id   ReservationId,
                                                                  EVSE_Id                  EVSEId,
                                                                  DateTime?                StartTime,
                                                                  TimeSpan?                Duration,
                                                                  EVSP_Id                  ProviderId,
                                                                  eMA_Id                   eMAId,
                                                                  ChargingProduct_Id       ChargingProductId,
                                                                  IEnumerable<Auth_Token>  AuthTokens,
                                                                  IEnumerable<eMA_Id>      eMAIds,
                                                                  IEnumerable<UInt32>      PINs,
                                                                  TimeSpan?                QueryTimeout);


    /// <summary>
    /// An event send whenever a reservation at an EVSE was made.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="EVSEId">The uniuqe identification of the reserved EVSE.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="Result">The result of the reservation.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnEVSEReservedDelegate(Object                   Sender,
                                                DateTime                 Timestamp,
                                                EventTracking_Id         EventTrackingId,
                                                RoamingNetwork_Id        RoamingNetworkId,
                                                ChargingReservation_Id   ReservationId,
                                                EVSE_Id                  EVSEId,
                                                DateTime?                StartTime,
                                                TimeSpan?                Duration,
                                                EVSP_Id                  ProviderId,
                                                eMA_Id                   eMAId,
                                                ChargingProduct_Id       ChargingProductId,
                                                IEnumerable<Auth_Token>  AuthTokens,
                                                IEnumerable<eMA_Id>      eMAIds,
                                                IEnumerable<UInt32>      PINs,
                                                ReservationResult        Result,
                                                TimeSpan                 Runtime,
                                                TimeSpan?                QueryTimeout);

    #endregion

    #region Reserve charging stations

    /// <summary>
    /// An event send whenever a reservation at a charging station is being made.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingStationId">The uniuqe identification of the reserved charging station.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnChargingStationReserveDelegate(Object                   Sender,
                                                          DateTime                 Timestamp,
                                                          EventTracking_Id         EventTrackingId,
                                                          RoamingNetwork_Id        RoamingNetworkId,
                                                          ChargingStation_Id       ChargingStationId,
                                                          DateTime?                StartTime,
                                                          TimeSpan?                Duration,
                                                          ChargingReservation_Id   ReservationId,
                                                          EVSP_Id                  ProviderId,
                                                          eMA_Id                   eMAId,
                                                          ChargingProduct_Id       ChargingProductId,
                                                          IEnumerable<Auth_Token>  AuthTokens,
                                                          IEnumerable<eMA_Id>      eMAIds,
                                                          IEnumerable<UInt32>      PINs,
                                                          TimeSpan?                QueryTimeout);


    /// <summary>
    /// An event send whenever a reservation at a charging station is being made.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingStationId">The uniuqe identification of the reserved charging station.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<ReservationResult> OnReserveChargingStationDelegate(DateTime                 Timestamp,
                                                                             CancellationToken        CancellationToken,
                                                                             EventTracking_Id         EventTrackingId,
                                                                             RoamingNetwork_Id        RoamingNetworkId,
                                                                             ChargingReservation_Id   ReservationId,
                                                                             ChargingStation_Id       ChargingStationId,
                                                                             DateTime?                StartTime,
                                                                             TimeSpan?                Duration,
                                                                             EVSP_Id                  ProviderId,
                                                                             eMA_Id                   eMAId,
                                                                             ChargingProduct_Id       ChargingProductId,
                                                                             IEnumerable<Auth_Token>  AuthTokens,
                                                                             IEnumerable<eMA_Id>      eMAIds,
                                                                             IEnumerable<UInt32>      PINs,
                                                                             TimeSpan?                QueryTimeout);


    /// <summary>
    /// An event send whenever a reservation at a charging station was made.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingStationId">The uniuqe identification of the reserved charging station.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="Result">The result of the reservation.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnChargingStationReservedDelegate(Object                   Sender,
                                                           DateTime                 Timestamp,
                                                           EventTracking_Id         EventTrackingId,
                                                           RoamingNetwork_Id        RoamingNetworkId,
                                                           ChargingStation_Id       ChargingStationId,
                                                           DateTime?                StartTime,
                                                           TimeSpan?                Duration,
                                                           ChargingReservation_Id   ReservationId,
                                                           EVSP_Id                  ProviderId,
                                                           eMA_Id                   eMAId,
                                                           ChargingProduct_Id       ChargingProductId,
                                                           IEnumerable<Auth_Token>  AuthTokens,
                                                           IEnumerable<eMA_Id>      eMAIds,
                                                           IEnumerable<UInt32>      PINs,
                                                           ReservationResult        Result,
                                                           TimeSpan                 Runtime,
                                                           TimeSpan?                QueryTimeout);

    #endregion

    #region Reserve charging pools

    /// <summary>
    /// An event send whenever a reservation at a charging pool is being made.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingPoolId">The uniuqe identification of the reserved charging pool.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnChargingPoolReserveDelegate(Object                   Sender,
                                                       DateTime                 Timestamp,
                                                       EventTracking_Id         EventTrackingId,
                                                       RoamingNetwork_Id        RoamingNetworkId,
                                                       ChargingPool_Id          ChargingPoolId,
                                                       DateTime?                StartTime,
                                                       TimeSpan?                Duration,
                                                       ChargingReservation_Id   ReservationId,
                                                       EVSP_Id                  ProviderId,
                                                       eMA_Id                   eMAId,
                                                       ChargingProduct_Id       ChargingProductId,
                                                       IEnumerable<Auth_Token>  AuthTokens,
                                                       IEnumerable<eMA_Id>      eMAIds,
                                                       IEnumerable<UInt32>      PINs,
                                                       TimeSpan?                QueryTimeout);


    /// <summary>
    /// An event send whenever a reservation at a charging pool is being made.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingPoolId">The uniuqe identification of the reserved charging pool.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<ReservationResult> OnReserveChargingPoolDelegate(DateTime                 Timestamp,
                                                                          CancellationToken        CancellationToken,
                                                                          EventTracking_Id         EventTrackingId,
                                                                          RoamingNetwork_Id        RoamingNetworkId,
                                                                          ChargingReservation_Id   ReservationId,
                                                                          ChargingPool_Id          ChargingPoolId,
                                                                          DateTime?                StartTime,
                                                                          TimeSpan?                Duration,
                                                                          EVSP_Id                  ProviderId,
                                                                          eMA_Id                   eMAId,
                                                                          ChargingProduct_Id       ChargingProductId,
                                                                          IEnumerable<Auth_Token>  AuthTokens,
                                                                          IEnumerable<eMA_Id>      eMAIds,
                                                                          IEnumerable<UInt32>      PINs,
                                                                          TimeSpan?                QueryTimeout);

    /// <summary>
    /// An event send whenever a reservation at a charging pool was made.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingPoolId">The uniuqe identification of the reserved charging pool.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="StartTime">The starting time of the reservation.</param>
    /// <param name="Duration">The duration of the reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    /// <param name="Result">The result of the reservation.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnChargingPoolReservedDelegate(Object                   Sender,
                                                        DateTime                 Timestamp,
                                                        EventTracking_Id         EventTrackingId,
                                                        RoamingNetwork_Id        RoamingNetworkId,
                                                        ChargingPool_Id          ChargingPoolId,
                                                        DateTime?                StartTime,
                                                        TimeSpan?                Duration,
                                                        ChargingReservation_Id   ReservationId,
                                                        EVSP_Id                  ProviderId,
                                                        eMA_Id                   eMAId,
                                                        ChargingProduct_Id       ChargingProductId,
                                                        IEnumerable<Auth_Token>  AuthTokens,
                                                        IEnumerable<eMA_Id>      eMAIds,
                                                        IEnumerable<UInt32>      PINs,
                                                        ReservationResult        Result,
                                                        TimeSpan                 Runtime,
                                                        TimeSpan?                QueryTimeout);

    #endregion


    #region Cancel a reservation

    /// <summary>
    /// An event send whenever a reservation will be deleted.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnReservationCancelDelegate(DateTime                               Timestamp,
                                                     Object                                 Sender,
                                                     EventTracking_Id                       EventTrackingId,
                                                     RoamingNetwork_Id                      RoamingNetworkId,
                                                     ChargingReservation_Id                 ReservationId,
                                                     ChargingReservationCancellationReason  Reason,
                                                     TimeSpan?                              QueryTimeout);


    /// <summary>
    /// Cancel a charging reservation.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting the deletion.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<Boolean> OnCancelReservationDelegate(DateTime                               Timestamp,
                                                              CancellationToken                      CancellationToken,
                                                              EventTracking_Id                       EventTrackingId,
                                                              ChargingReservation_Id                 ReservationId,
                                                              ChargingReservationCancellationReason  Reason,
                                                              TimeSpan?                              QueryTimeout);


    /// <summary>
    /// An event send whenever a reservation was deleted.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="Result">The result of the reservation.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnReservationCancelledInternalDelegate(DateTime                               Timestamp,
                                                                Object                                 Sender,
                                                                EventTracking_Id                       EventTrackingId,
                                                                ChargingReservation_Id                 ReservationId,
                                                                ChargingReservationCancellationReason  Reason);


    /// <summary>
    /// An event send whenever a reservation was deleted.
    /// </summary>
    /// <param name="Sender">The sender of this event.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ReservationId">The unique identification for this charging reservation.</param>
    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    /// <param name="Result">The result of the reservation.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnReservationCancelledDelegate(DateTime                               Timestamp,
                                                        Object                                 Sender,
                                                        EventTracking_Id                       EventTrackingId,
                                                        RoamingNetwork_Id                      RoamingNetworkId,
                                                        ChargingReservation_Id                 ReservationId,
                                                        ChargingReservationCancellationReason  Reason,
                                                        Boolean                                Result,
                                                        TimeSpan                               Runtime,
                                                        TimeSpan?                              QueryTimeout);

    #endregion

    #region Delete a reservation

    ///// <summary>
    ///// An event send whenever a reservation will be deleted by an EV customer.
    ///// </summary>
    ///// <param name="Sender">The sender of this event.</param>
    ///// <param name="Timestamp">The timestamp of the request.</param>
    ///// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    ///// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    ///// <param name="ReservationId">The unique identification for this charging reservation.</param>
    ///// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    ///// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    ///// <param name="QueryTimeout">An optional timeout for this request.</param>
    //public delegate void OnReservationDeleteDelegate(Object                  Sender,
    //                                                 DateTime                Timestamp,
    //                                                 EventTracking_Id        EventTrackingId,
    //                                                 RoamingNetwork_Id       RoamingNetworkId,
    //                                                 ChargingReservation_Id  ReservationId,
    //                                                 EVSP_Id                 ProviderId,
    //                                                 eMA_Id                  eMAId,
    //                                                 TimeSpan?               QueryTimeout);


    ///// <summary>
    ///// Delete a charging reservation as requested by an EV customer.
    ///// </summary>
    ///// <param name="Timestamp">The timestamp of the request.</param>
    ///// <param name="CancellationToken">A token to cancel this request.</param>
    ///// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    ///// <param name="ReservationId">The unique identification for this charging reservation.</param>
    ///// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    ///// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting the deletion.</param>
    ///// <param name="QueryTimeout">An optional timeout for this request.</param>
    //public delegate Task<Boolean> OnDeleteReservationDelegate(DateTime                Timestamp,
    //                                                          CancellationToken       CancellationToken,
    //                                                          EventTracking_Id        EventTrackingId,
    //                                                          ChargingReservation_Id  ReservationId,
    //                                                          EVSP_Id                 ProviderId,
    //                                                          eMA_Id                  eMAId,
    //                                                          TimeSpan?               QueryTimeout);


    ///// <summary>
    ///// An event send whenever a reservation was deleted by an EV customer.
    ///// </summary>
    ///// <param name="Sender">The sender of this event.</param>
    ///// <param name="Timestamp">The timestamp of the request.</param>
    ///// <param name="EventTrackingId">An unique event tracking identification for correlating this event with other events.</param>
    ///// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    ///// <param name="ReservationId">The unique identification for this charging reservation.</param>
    ///// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    ///// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    ///// <param name="Result">The result of the reservation.</param>
    ///// <param name="Runtime">The runtime of the request.</param>
    ///// <param name="QueryTimeout">An optional timeout for this request.</param>
    //public delegate void OnReservationDeletedDelegate(Object                  Sender,
    //                                                  DateTime                Timestamp,
    //                                                  EventTracking_Id        EventTrackingId,
    //                                                  RoamingNetwork_Id       RoamingNetworkId,
    //                                                  ChargingReservation_Id  ReservationId,
    //                                                  EVSP_Id                 ProviderId,
    //                                                  eMA_Id                  eMAId,
    //                                                  ReservationResult       Result,
    //                                                  TimeSpan                Runtime,
    //                                                  TimeSpan?               QueryTimeout);

    #endregion

}