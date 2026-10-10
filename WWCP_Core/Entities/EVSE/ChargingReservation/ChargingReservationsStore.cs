/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Globalization;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Hermod.DNS;

using cloud.charging.open.protocols.WWCP.POI;
using cloud.charging.open.protocols.WWCP.Networking;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class ChargingReservationsStore : ADataStore<ChargingReservation_Id, ChargingReservationCollection>
    {

        #region Events

        /// <summary>
        /// An event fired whenever a new charging reservation was registered.
        /// </summary>
        public event OnNewReservationDelegate OnNewChargingReservation;

        #endregion

        #region Constructor(s)

        public ChargingReservationsStore(RoamingNetwork_Id                 RoamingNetworkId,
                                         IEnumerable<RoamingNetworkInfo>?  RoamingNetworkInfos   = null,
                                         Boolean                           DisableLogfiles       = false,
                                         Boolean                           ReloadDataOnStart     = true,

                                         Boolean                           DisableNetworkSync    = false,
                                         String?                           LoggingPath           = null,
                                         DNSClient?                        DNSClient             = null)

            : base(Name:                  nameof(ChargingReservationsStore),
                   StringIdParser:        ChargingReservation_Id.TryParse,

                   CommandProcessor:      (logfilename,
                                           lineNumber,
                                           remoteSocket,
                                           timestamp,
                                           id,
                                           command,
                                           json,
                                           internalData) => false,

                   DisableLogfiles:       DisableLogfiles,
                   LogFilePathCreator:    roamingNetworkId => Path.Combine(LoggingPath ?? AppContext.BaseDirectory, "ChargingReservations"),
                   LogFileNameCreator:    roamingNetworkId => $"ChargingReservations-{roamingNetworkId}-{Environment.MachineName}_{org.GraphDefined.Vanaheimr.Illias.Timestamp.Now.ToString("yyyy-MM", CultureInfo.InvariantCulture)}.log",
                   ReloadDataOnStart:     ReloadDataOnStart,
                   LogfileSearchPattern:  roamingNetworkId => $"ChargingReservations-{roamingNetworkId}-{Environment.MachineName}_",

                   RoamingNetworkId:      RoamingNetworkId,
                   RoamingNetworkInfos:   RoamingNetworkInfos,
                   DisableNetworkSync:    DisableNetworkSync,
                   DNSClient:             DNSClient)

        { }

        #endregion


        #region New(NewChargingReservation)

        /// <summary>
        /// Store a new charging reservation, or one more version of one already stored.
        /// </summary>
        /// <remarks>
        /// What is logged is handed to the log inside the lock, so that the log
        /// file has the changes in the order they were made, and awaited after
        /// it: a failure to log is the caller's to see, not lost in a task
        /// nobody looks at.
        /// </remarks>
        public Task New(ChargingReservation NewChargingReservation)
        {

            Task logged;

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(NewChargingReservation.Id))
                {

                    InternalData.TryAdd(NewChargingReservation.Id, new ChargingReservationCollection(NewChargingReservation));

                    logged = LogIt("new",
                                   NewChargingReservation.Id,
                                   "reservations",
                                   new JArray(NewChargingReservation.ToJSON()));

                }

                else
                {

                    InternalData[NewChargingReservation.Id].Add(NewChargingReservation);

                    logged = LogIt("update",
                                   NewChargingReservation.Id,
                                   "reservations",
                                   new JArray(InternalData[NewChargingReservation.Id].Select(reservation => reservation.ToJSON())));

                }

            }

            return logged;

        }

        #endregion

        #region NewOrUpdate(NewChargingReservation)

        /// <summary>
        /// Store a new charging reservation, or replace the latest version of one
        /// already stored where it differs from it.
        /// </summary>
        public Task NewOrUpdate(ChargingReservation NewChargingReservation)
        {

            var logged = Task.CompletedTask;

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(NewChargingReservation.Id))
                {

                    InternalData.TryAdd(NewChargingReservation.Id, new ChargingReservationCollection(NewChargingReservation));

                    logged = LogIt("new",
                                   NewChargingReservation.Id,
                                   "reservations",
                                   new JArray(NewChargingReservation.ToJSON()));

                }
                else if (NewChargingReservation.ToJSON() != InternalData[NewChargingReservation.Id].Last().ToJSON())
                {

                    InternalData[NewChargingReservation.Id].UpdateLast(NewChargingReservation);

                    logged = LogIt("update",
                                   NewChargingReservation.Id,
                                   "reservations",
                                   new JArray(InternalData[NewChargingReservation.Id].Select(reservation => reservation.ToJSON())));

                }

            }

            return logged;

        }

        #endregion

        #region UpdateAll(Id, UpdateFunc)

        /// <summary>
        /// Update every stored version of a charging reservation.
        /// </summary>
        /// <remarks>
        /// A reservation not stored yet is left alone, and nothing is logged:
        /// a charging pool updates the reservation it made before the roaming
        /// network has stored it, which happens only once the answer has come
        /// back up to the roaming network.
        /// </remarks>
        public Task UpdateAll(ChargingReservation_Id       Id,
                              Action<ChargingReservation>  UpdateFunc)
        {

            lock (InternalData)
            {

                if (!InternalData.TryGetValue(Id, out var reservationCollection))
                    return Task.CompletedTask;

                foreach (var reservation in reservationCollection)
                    UpdateFunc(reservation);

                return LogIt("update",
                             Id,
                             "reservations",
                             new JArray(reservationCollection.Select(reservation => reservation.ToJSON())));

            }

        }

        #endregion

        #region UpdateLatest(Id, UpdateFunc)

        /// <summary>
        /// Update the latest stored version of a charging reservation.
        /// </summary>
        /// <remarks>
        /// A reservation not stored yet is left alone, and nothing is logged.
        /// </remarks>
        public Task UpdateLatest(ChargingReservation_Id       Id,
                                 Action<ChargingReservation>  UpdateFunc)
        {

            lock (InternalData)
            {

                if (!InternalData.TryGetValue(Id, out var reservationCollection))
                    return Task.CompletedTask;

                UpdateFunc(reservationCollection.Last());

                return LogIt("update",
                             Id,
                             "reservations",
                             new JArray(reservationCollection.Select(reservation => reservation.ToJSON())));

            }

        }

        #endregion

        #region Stop  (Id, Timestamp = null, StopAuthentication = null)

        /// <summary>
        /// End the latest version of a charging reservation.
        /// </summary>
        public Task Stop(ChargingReservation_Id  Id,
                         DateTime?               Timestamp          = null,
                         AAuthentication         StopAuthentication = null)
        {

            lock (InternalData)
            {

                if (!InternalData.TryGetValue(Id, out var reservationCollection))
                    return Task.CompletedTask;

                var reservation = reservationCollection.LastOrDefault();

                if (reservation is null)
                    return Task.CompletedTask;

                reservation.EndTime                 = Timestamp ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

                if (StopAuthentication is not null)
                    reservation.StopAuthentication  = StopAuthentication;

                return LogIt("stop",
                             Id,
                             "reservations",
                             new JArray(reservation.ToJSON()));

            }

        }

        #endregion


        #region ReloadData()

        //public void ReloadData()
        //{

        //    base.ReloadData("ChargingReservations-" + RoamingNetwork.Id.ToString(),
        //                    (command, json) => {

        //        switch (command.ToLower())
        //        {

        //            case "new":

        //                if (json["reservations"] is JArray reservations)
        //                {



        //                }

        //                //// 0: Timestamp
        //                //// 1: "Start"
        //                //// 2: OperatorId
        //                //// 3: EVSEId
        //                //// 4: ChargingProduct
        //                //// 5: AuthIdentification?.AuthToken
        //                //// 6: eMA Id
        //                //// 7: result.AuthorizatorId
        //                //// 8: result.ProviderId
        //                //// 9: result.ReservationId

        //                //var NewChargingReservation = new ChargingReservation(ChargingReservation_Id.Parse(elements[9]),
        //                //                                             elements[0] != "" ? DateTime.Parse(elements[0]) : Timestamp.Now)
        //                //{
        //                //    AuthorizatorId = elements[7] != "" ? CSORoamingProvider_Id.Parse(elements[7]) : null,
        //                //    //AuthService          = result.ISendAuthorizeStartStop,
        //                //    ChargingStationOperatorId = elements[2] != "" ? ChargingStationOperator_Id.Parse(elements[2]) : new ChargingStationOperator_Id?(),
        //                //    EVSE = RoamingNetwork.GetEVSEbyId(EVSE_Id.Parse(elements[3])),
        //                //    EVSEId = EVSE_Id.Parse(elements[3]),
        //                //    IdentificationStart = elements[5] != "" ? (AAuthentication)LocalAuthentication.FromAuthToken(authentication token.Parse(elements[5]))
        //                //                                                  : elements[6] != "" ? (AAuthentication)RemoteAuthentication.FromRemoteIdentification(eMobilityAccount_Id.Parse(elements[6]))
        //                //                                                  : null,
        //                //    ChargingProduct = elements[4] != "" ? ChargingProduct.Parse(elements[4]) : null
        //                //};

        //                //if (_ChargingReservations.ContainsKey(NewChargingReservation.Id))
        //                //    _ChargingReservations.Remove(NewChargingReservation.Id);

        //                //_ChargingReservations.Add(NewChargingReservation.Id, NewChargingReservation);

        //                break;


        //            case "update":

        //                // 0: Timestamp
        //                // 1: "Stop"
        //                // 2: EVSEId
        //                // 3: ReservationId
        //                // 4: RFID UID
        //                // 5: eMAId

        //                break;


        //            case "stop":

        //                // 0: Timestamp
        //                // 1: "Stop"
        //                // 2: EVSEId
        //                // 3: ReservationId
        //                // 4: RFID UID
        //                // 5: eMAId

        //                break;


        //        }
        //    });
        //}

        #endregion


        public Boolean TryGetLatest(ChargingReservation_Id   Id,
                                    out ChargingReservation  LatestReservation)
        {

            if (InternalData.TryGetValue(Id, out ChargingReservationCollection ReservationCollection))
            {
                LatestReservation = ReservationCollection.LastOrDefault();
                return true;
            }

            LatestReservation = null;
            return false;

        }


    }

}
