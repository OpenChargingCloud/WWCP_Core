/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.WWCP.Networking;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP
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

        public ChargingReservationsStore(IRoamingNetwork                  RoamingNetwork,
                                         IEnumerable<RoamingNetworkInfo>  RoamingNetworkInfos   = null,
                                         Boolean                          DisableLogfiles       = false,
                                         Boolean                          ReloadDataOnStart     = true,

                                         Boolean                          DisableNetworkSync    = false,
                                         DNSClient                        DNSClient             = null)

            : base(RoamingNetwork,
                   RoamingNetworkInfos,

                   DisableLogfiles,
                   "ChargingReservations" + Path.DirectorySeparatorChar,
                   roamingNetworkId => String.Concat("ChargingReservations-",
                                                     roamingNetworkId, "-",
                                                     Environment.MachineName, "_",
                                                     DateTime.UtcNow.Year, "-", DateTime.UtcNow.Month.ToString("D2"),
                                                     ".log"),
                   ReloadDataOnStart,
                   roamingNetworkId => "ChargingReservations-" + roamingNetworkId + "-" + Environment.MachineName + "_",
                   (logfilename, command, json) => null,

                   DisableNetworkSync,
                   DNSClient)

        { }

        #endregion


        #region New(NewChargingReservation)

        public void New(ChargingReservation NewChargingReservation)
        {

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(NewChargingReservation.Id))
                {

                    InternalData.Add(NewChargingReservation.Id, new ChargingReservationCollection(NewChargingReservation));

                    LogIt("new",
                          NewChargingReservation.Id,
                          "reservations",
                          new JArray(NewChargingReservation.ToJSON()));

                }

                else
                {

                    InternalData[NewChargingReservation.Id].Add(NewChargingReservation);

                    LogIt("update",
                          NewChargingReservation.Id,
                          "reservations",
                          new JArray(InternalData[NewChargingReservation.Id].Select(reservation => reservation.ToJSON())));

                }

            }

        }

        #endregion

        #region NewOrUpdate(NewChargingReservation)

        public void NewOrUpdate(ChargingReservation NewChargingReservation)
        {

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(NewChargingReservation.Id))
                {

                    InternalData.Add(NewChargingReservation.Id, new ChargingReservationCollection(NewChargingReservation));

                    LogIt("new",
                          NewChargingReservation.Id,
                          "reservations",
                          new JArray(NewChargingReservation.ToJSON()));

                }
                else if (NewChargingReservation.ToJSON() != InternalData[NewChargingReservation.Id].Last().ToJSON())
                {

                    InternalData[NewChargingReservation.Id].UpdateLast(NewChargingReservation);

                    LogIt("update",
                          NewChargingReservation.Id,
                          "reservations",
                          new JArray(InternalData[NewChargingReservation.Id].Select(reservation => reservation.ToJSON())));

                }

            }

        }

        #endregion

        #region UpdateAll(Id, UpdateFunc)

        public ChargingReservationsStore UpdateAll(ChargingReservation_Id       Id,
                                                   Action<ChargingReservation>  UpdateFunc)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingReservationCollection reservationCollection))
                {
                    foreach (var reservation in reservationCollection)
                        UpdateFunc(reservation);
                }

                LogIt("update",
                      Id,
                      "reservations",
                      new JArray(reservationCollection.Select(reservation => reservation.ToJSON())));

            }

            return this;

        }

        #endregion

        #region UpdateLatest(Id, UpdateFunc)

        public ChargingReservationsStore UpdateLatest(ChargingReservation_Id       Id,
                                                      Action<ChargingReservation>  UpdateFunc)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingReservationCollection reservationCollection))
                    UpdateFunc(reservationCollection.Last());

                LogIt("update",
                      Id,
                      "reservations",
                      new JArray(reservationCollection.Select(reservation => reservation.ToJSON())));

            }

            return this;

        }

        #endregion

        #region Stop  (Id, Timestamp = null, StopAuthentication = null)

        public void Stop(ChargingReservation_Id  Id,
                         DateTime?               Timestamp          = null,
                         AAuthentication         StopAuthentication = null)
        {

            lock (InternalData)
            {


                if (InternalData.TryGetValue(Id, out ChargingReservationCollection reservationCollection))
                {

                    var reservation = reservationCollection.LastOrDefault();
                    if (reservation != null)
                    {

                        reservation.EndTime                 = Timestamp ?? DateTime.UtcNow;

                        if (StopAuthentication != null)
                            reservation.StopAuthentication  = StopAuthentication;

                        LogIt("stop",
                              Id,
                              "reservations",
                              new JArray(reservation.ToJSON()));

                    }

                }

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
        //                //                                             elements[0] != "" ? DateTime.Parse(elements[0]) : DateTime.UtcNow)
        //                //{
        //                //    AuthorizatorId = elements[7] != "" ? CSORoamingProvider_Id.Parse(elements[7]) : null,
        //                //    //AuthService          = result.ISendAuthorizeStartStop,
        //                //    ChargingStationOperatorId = elements[2] != "" ? ChargingStationOperator_Id.Parse(elements[2]) : new ChargingStationOperator_Id?(),
        //                //    EVSE = RoamingNetwork.GetEVSEbyId(EVSE_Id.Parse(elements[3])),
        //                //    EVSEId = EVSE_Id.Parse(elements[3]),
        //                //    IdentificationStart = elements[5] != "" ? (AAuthentication)LocalAuthentication.FromAuthToken(Auth_Token.Parse(elements[5]))
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
