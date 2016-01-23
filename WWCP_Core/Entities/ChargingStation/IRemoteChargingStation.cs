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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    public interface IRemoteChargingStation
    {

        IEnumerable<EVSE> EVSEs { get; }
        ChargingStation_Id Id { get; }
        ChargingStationStatusType Status { get; }


        #region OnEVSEDataChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEDataChangedDelegate OnEVSEDataChanged;

        #endregion

        #region OnEVSE(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEStatusChangedDelegate OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEAdminStatusChangedDelegate OnEVSEAdminStatusChanged;

        #endregion

        #region OnReserveEVSE / OnReservedEVSE

        /// <summary>
        /// An event fired whenever a reserve EVSE command was received.
        /// </summary>
        event OnReserveEVSEDelegate OnReserveEVSE;

        /// <summary>
        /// An event fired whenever a reserve EVSE command completed.
        /// </summary>
        event OnEVSEReservedDelegate OnEVSEReserved;

        #endregion

        #region OnRemoteEVSEStart / OnRemoteEVSEStarted

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        event OnRemoteEVSEStartDelegate OnRemoteEVSEStart;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        event OnRemoteEVSEStartedDelegate OnRemoteEVSEStarted;

        #endregion

        #region OnRemoteEVSEStop / OnRemoteEVSEStopped

        /// <summary>
        /// An event fired whenever a remote stop EVSE command was received.
        /// </summary>
        event OnRemoteEVSEStopDelegate OnRemoteEVSEStop;

        /// <summary>
        /// An event fired whenever a remote stop EVSE command completed.
        /// </summary>
        event OnRemoteEVSEStoppedDelegate OnRemoteEVSEStopped;

        #endregion


        #region EVSEAddition

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        IVotingSender<DateTime, IRemoteChargingStation, IRemoteEVSE, Boolean> OnEVSEAddition { get; }

        #endregion


        IRemoteEVSE CreateNewEVSE(EVSE_Id                           EVSEId,
                                  Action<EVSE>                      Configurator  = null,
                                  Action<EVSE>                      OnSuccess     = null,
                                  Action<ChargingStation, EVSE_Id>  OnError       = null);


        Task<ReservationResult> ReserveEVSE(DateTime                 Timestamp,
                                            CancellationToken        CancellationToken,
                                            EventTracking_Id         EventTrackingId,
                                            EVSP_Id                  ProviderId,
                                            ChargingReservation_Id   ReservationId,
                                            DateTime?                StartTime,
                                            TimeSpan?                Duration,
                                            EVSE_Id                  EVSEId,
                                            ChargingProduct_Id       ChargingProductId  = null,
                                            IEnumerable<Auth_Token>  RFIDIds            = null,
                                            IEnumerable<eMA_Id>      eMAIds             = null,
                                            IEnumerable<UInt32>      PINs               = null,
                                            TimeSpan?                QueryTimeout       = null);



        /// <summary>
        /// Initiate a remote start of the given charging session at the given charging station
        /// and for the given provider/eMAId.
        /// </summary>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <returns>A RemoteStartResult task.</returns>
        Task<RemoteStartChargingStationResult> RemoteStart(DateTime                Timestamp,
                                                           CancellationToken       CancellationToken,
                                                           EventTracking_Id        EventTrackingId,
                                                           ChargingProduct_Id      ChargingProductId,
                                                           ChargingReservation_Id  ReservationId,
                                                           ChargingSession_Id      SessionId,
                                                           EVSP_Id                 ProviderId,
                                                           eMA_Id                  eMAId,
                                                           TimeSpan?               QueryTimeout  = null);

        /// <summary>
        /// Initiate a remote start of the given charging session at the given EVSE
        /// and for the given Provider/eMAId.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <returns>A RemoteStartResult task.</returns>
        Task<RemoteStartEVSEResult> RemoteStart(DateTime                Timestamp,
                                                CancellationToken       CancellationToken,
                                                EventTracking_Id        EventTrackingId,
                                                EVSE_Id                 EVSEId,
                                                ChargingProduct_Id      ChargingProductId,
                                                ChargingReservation_Id  ReservationId,
                                                ChargingSession_Id      SessionId,
                                                EVSP_Id                 ProviderId,
                                                eMA_Id                  eMAId,
                                                TimeSpan?               QueryTimeout  = null);



        /// <summary>
        /// Initiate a remote stop of the given charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <returns>A RemoteStopResult task.</returns>
        Task<RemoteStopResult> RemoteStop(DateTime             Timestamp,
                                          CancellationToken    CancellationToken,
                                          EventTracking_Id     EventTrackingId,
                                          ChargingSession_Id   SessionId,
                                          ReservationHandling  ReservationHandling,
                                          EVSP_Id              ProviderId,
                                          TimeSpan?            QueryTimeout  = null);


        /// <summary>
        /// Initiate a remote stop of the given charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <returns>A RemoteStopResult task.</returns>
        Task<RemoteStopEVSEResult> RemoteStop(DateTime             Timestamp,
                                              CancellationToken    CancellationToken,
                                              EventTracking_Id     EventTrackingId,
                                              EVSE_Id              EVSEId,
                                              ChargingSession_Id   SessionId,
                                              ReservationHandling  ReservationHandling,
                                              EVSP_Id              ProviderId,
                                              TimeSpan?            QueryTimeout  = null);


        /// <summary>
        /// Initiate a remote stop of the given charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <returns>A RemoteStopResult task.</returns>
        Task<RemoteStopChargingStationResult> RemoteStop(DateTime             Timestamp,
                                                         CancellationToken    CancellationToken,
                                                         EventTracking_Id     EventTrackingId,
                                                         ChargingStation_Id   ChargingStationId,
                                                         ChargingSession_Id   SessionId,
                                                         ReservationHandling  ReservationHandling,
                                                         EVSP_Id              ProviderId,
                                                         TimeSpan?            QueryTimeout  = null);


    }

}