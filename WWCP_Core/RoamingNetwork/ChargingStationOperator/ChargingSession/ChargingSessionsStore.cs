﻿/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.WWCP.Networking;

#endregion

namespace org.GraphDefined.WWCP
{

    public class ChargingSessionsStore : ADataStore<ChargingSession_Id, ChargingSession>
    {

        #region Data

        private readonly Action<ChargeDetailRecord> AddChargeDetailRecordAction;

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a new charging session was registered.
        /// </summary>
        public event OnNewChargingSessionDelegate OnNewChargingSession;

        #endregion

        #region Constructor(s)

        public ChargingSessionsStore(IRoamingNetwork                  RoamingNetwork,
                                     Func<RoamingNetwork_Id, String>  LogFileNameCreator    = null,
                                     Boolean                          DisableLogfiles       = false,
                                     IEnumerable<RoamingNetworkInfo>  RoamingNetworkInfos   = null,
                                     Boolean                          DisableNetworkSync    = false)

            : base(RoamingNetwork,
                   LogFileNameCreator ?? (roamingNetworkId => String.Concat("ChargingSessions", Path.DirectorySeparatorChar, "ChargingSessions-",
                                                                            roamingNetworkId, "-",
                                                                            Environment.MachineName, "_",
                                                                            DateTime.UtcNow.Year, "-", DateTime.UtcNow.Month.ToString("D2"),
                                                                            ".log")),
                   DisableLogfiles,
                   RoamingNetworkInfos,
                   DisableNetworkSync)

        { }

        #endregion


        #region NewOrUpdate(NewChargingSession, UpdateFunc = null)

        public void NewOrUpdate(ChargingSession          NewChargingSession,
                                Action<ChargingSession>  UpdateFunc = null)
        {

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(NewChargingSession.Id))
                {

                    InternalData.Add(NewChargingSession.Id, NewChargingSession);
                    UpdateFunc?.Invoke(NewChargingSession);

                    LogIt("new",
                          NewChargingSession.Id,
                          "chargingSession",
                          NewChargingSession.ToJSON());

                }
                else
                {

                    UpdateFunc?.Invoke(NewChargingSession);

                    LogIt("update",
                          NewChargingSession.Id,
                          "chargingSession",
                          NewChargingSession.ToJSON());

                }

            }

        }

        #endregion

        #region Update     (Id, UpdateFunc)

        public ChargingSessionsStore Update(ChargingSession_Id       Id,
                                            Action<ChargingSession>  UpdateFunc)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingSession chargingSession))
                    UpdateFunc(chargingSession);

                LogIt("update",
                      Id,
                      "chargingSession",
                      chargingSession.ToJSON());

            }

            return this;

        }

        #endregion

        #region Remove(ChargingSessionId, Authentication)

        public void Remove(ChargingSession_Id  ChargingSessionId,
                           AAuthentication     Authentication)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(ChargingSessionId, out ChargingSession session))
                {

                    InternalData.Remove(session.Id);
                    session.AuthenticationStop = Authentication;

                    LogIt("remove",
                          session.Id,
                          "chargingSession",
                          session.ToJSON());

                }

            }

        }

        #endregion

        #region Remove(ChargingSession)

        public void Remove(ChargingSession ChargingSession)
        {

            lock (InternalData)
            {

                if (ChargingSession != null &&
                    InternalData.ContainsKey(ChargingSession.Id))
                {

                    InternalData.Remove(ChargingSession.Id);

                    LogIt("remove",
                          ChargingSession.Id,
                          "chargingSession",
                          ChargingSession.ToJSON());

                }

            }

        }

        #endregion



        //#region SendCDR(SendCDRResult)

        //public void SendCDR(SendCDRResult sendCDRResult)
        //{

        //    lock (_ChargingSessions)
        //    {

        //        AddChargeDetailRecordAction?.Invoke(sendCDRResult.ChargeDetailRecord);

        //        _ChargingSessions.Remove(sendCDRResult.ChargeDetailRecord.SessionId);

        //        var LogLine = String.Concat(DateTime.UtcNow.ToIso8601(), ",",
        //                                    "SendCDR,",
        //                                    sendCDRResult.ChargeDetailRecord.EVSE?.Id ?? sendCDRResult.ChargeDetailRecord.EVSEId, ",",
        //                                    sendCDRResult.ChargeDetailRecord.SessionId, ",",
        //                                    sendCDRResult.ChargeDetailRecord.IdentificationStart, ",",
        //                                    sendCDRResult.ChargeDetailRecord.IdentificationStop, ",",
        //                                    sendCDRResult.Result, ",",
        //                                    sendCDRResult.Warnings.AggregateWith("/"));

        //        File.AppendAllText(SessionLogFileName,
        //                           LogLine + Environment.NewLine);

        //    }

        //}

        //#endregion


        //#region RegisterExternalChargingSession(Timestamp, Sender, ChargingSession)

        ///// <summary>
        ///// Register an external charging session which was not registered
        ///// via the RemoteStart or AuthStart mechanisms.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp of the request.</param>
        ///// <param name="Sender">The sender of the charging session.</param>
        ///// <param name="ChargingSession">The charging session.</param>
        //public void RegisterExternalChargingSession(DateTime         Timestamp,
        //                                            Object           Sender,
        //                                            ChargingSession  ChargingSession)
        //{

        //    #region Initial checks

        //    if (ChargingSession == null)
        //        throw new ArgumentNullException(nameof(ChargingSession), "The given charging session must not be null!");

        //    #endregion


        //    //if (!_ChargingSessions_RFID.ContainsKey(ChargingSession.Id))
        //    //{

        //        DebugX.LogT("Registered external charging session '" + ChargingSession.Id + "'!");

        //        //_ChargingSessions.TryAdd(ChargingSession.Id, ChargingSession);

        //        if (ChargingSession.EVSEId.HasValue)
        //        {

        //            var _EVSE = RoamingNetwork.GetEVSEbyId(ChargingSession.EVSEId.Value);

        //            if (_EVSE != null)
        //            {

        //                ChargingSession.EVSE = _EVSE;

        //                // Will NOT set the EVSE status!
        //                //_EVSE.ChargingSession = ChargingSession;

        //            }

        //        }

        //        // else charging station

        //        // else charging pool

        //        //SendNewChargingSession(Timestamp, Sender, ChargingSession);

        //    //}

        //}

        //#endregion

        //#region RemoveExternalChargingSession(Timestamp, Sender, ChargingSession)

        ///// <summary>
        ///// Register an external charging session which was not registered
        ///// via the RemoteStart or AuthStart mechanisms.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp of the request.</param>
        ///// <param name="Sender">The sender of the charging session.</param>
        ///// <param name="ChargingSession">The charging session.</param>
        //public void RemoveExternalChargingSession(DateTime         Timestamp,
        //                                          Object           Sender,
        //                                          ChargingSession  ChargingSession)
        //{

        //    #region Initial checks

        //    if (ChargingSession == null)
        //        throw new ArgumentNullException(nameof(ChargingSession), "The given charging session must not be null!");

        //    #endregion

        //    ChargingStationOperator _cso = null;

        //    //if (_ChargingSessions_RFID.TryRemove(ChargingSession.Id, out _cso))
        //    //{

        //        DebugX.LogT("Removing external charging session '" + ChargingSession.Id + "'!");

        //    //}

        //    if (ChargingSession.EVSE != null)
        //    {

        //        if (ChargingSession.EVSE.ChargingSession != null &&
        //            ChargingSession.EVSE.ChargingSession == ChargingSession)
        //        {

        //            //ChargingSession.EVSE.ChargingSession = null;

        //        }

        //    }

        //    else if (ChargingSession.EVSEId.HasValue)
        //    {

        //        var _EVSE = RoamingNetwork.GetEVSEbyId(ChargingSession.EVSEId.Value);

        //        if (_EVSE                 != null &&
        //            _EVSE.ChargingSession != null &&
        //            _EVSE.ChargingSession == ChargingSession)
        //        {

        //            //_EVSE.ChargingSession = null;

        //        }

        //    }

        //}

        //#endregion

        //#region (internal) SendNewChargingSession(Timestamp, Sender, Session)

        //internal void SendNewChargingSession(DateTime         Timestamp,
        //                                     Object           Sender,
        //                                     ChargingSession  Session)
        //{

        //    if (Session != null)
        //    {

        //        if (Session.RoamingNetwork == null)
        //            Session.RoamingNetwork = RoamingNetwork;

        //    }

        //    OnNewChargingSession?.Invoke(Timestamp, Sender, Session);

        //}

        //#endregion



        #region ReloadData()

        public void ReloadData()
        {

            ReloadData("ChargingSessions-" + RoamingNetwork.Id,
                       (command, json) =>
                       {
                           switch (command.ToLower())
                           {

                               case "new":

                                   if (json["chargingSession"] is JObject newChargingSession)
                                   {



                                   }

                                   break;

                               case "update":

                                   if (json["chargingSession"] is JObject updatedChargingSession)
                                   {



                                   }

                                   break;

                               case "remove":

                                   if (json["chargingSession"] is JObject removedChargingSession)
                                   {



                                   }

                                   break;

                           }
                       });

        }

        #endregion

    }

}