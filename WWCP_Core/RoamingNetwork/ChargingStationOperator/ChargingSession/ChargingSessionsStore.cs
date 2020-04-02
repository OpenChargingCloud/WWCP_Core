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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.WWCP.Networking;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charging session data store.
    /// </summary>
    public class ChargingSessionsStore : ADataStore<ChargingSession_Id, ChargingSession>
    {

        #region Data

        private readonly Action<ChargeDetailRecord> AddChargeDetailRecordAction;

        public ChargingSession_Id Id => throw new NotImplementedException();

        public ulong Length => throw new NotImplementedException();

        public bool IsNullOrEmpty => throw new NotImplementedException();

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a new charging session was registered.
        /// </summary>
        public event OnNewChargingSessionDelegate           OnNewChargingSession;

        /// <summary>
        /// An event fired whenever a new charge detail record was registered.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate        OnNewChargeDetailRecord;

        /// <summary>
        /// An event fired whenever a new charge detail record was sent.
        /// </summary>
        public event OnNewChargeDetailRecordResultDelegate  OnNewChargeDetailRecordResult;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging session data store.
        /// </summary>
        /// <param name="RoamingNetwork"></param>
        /// 
        /// <param name="DisableLogfiles"></param>
        /// <param name="ReloadDataOnStart"></param>
        /// 
        /// <param name="RoamingNetworkInfos"></param>
        /// <param name="DisableNetworkSync"></param>
        /// <param name="DNSClient">The DNS client defines which DNS servers to use.</param>
        public ChargingSessionsStore(IRoamingNetwork                  RoamingNetwork,

                                     Boolean                          DisableLogfiles       = false,
                                     Boolean                          ReloadDataOnStart     = true,

                                     IEnumerable<RoamingNetworkInfo>  RoamingNetworkInfos   = null,
                                     Boolean                          DisableNetworkSync    = false,
                                     DNSClient                        DNSClient             = null)

            : base(RoamingNetwork:         RoamingNetwork,

                   CommandProcessor:       (logfilename, remoteSocket, command, json, InternalData) => {

                       if (json["chargingSession"] is JObject chargingSession)
                       {

                           var session = ChargingSession.Parse(chargingSession, RoamingNetwork);

                           if (session.Id.ToString() == "f62520b6-9840-4601-b0b8-52e3023a6492")
                           {

                           }

                           switch (command)
                           {

                               #region "remoteStart"

                               case "remoteStart":
                                   {

                                       if (!InternalData.ContainsKey(session.Id))
                                           InternalData.Add(session.Id, session);

                                       if (session.EVSEId.HasValue && session.EVSE == null)
                                           session.EVSE = RoamingNetwork.GetEVSEById(session.EVSEId.Value);

                                       if (session.EVSE != null)
                                           session.EVSE.ChargingSession = session;

                                   }
                                   return true;

                               #endregion

                               #region "remoteStop"

                               case "remoteStop":
                                   {

                                       if (!InternalData.ContainsKey(session.Id))
                                           InternalData.Add(session.Id, session);
                                       else
                                           InternalData[session.Id] = session;

                                       if (session.EVSEId.HasValue && session.EVSE == null)
                                           session.EVSE = RoamingNetwork.GetEVSEById(session.EVSEId.Value);

                                       if (session.EVSE != null)
                                           session.EVSE.ChargingSession = null;

                                   }
                                   return true;

                               #endregion

                               #region "authStart"

                               case "authStart":
                                   {

                                       if (!InternalData.ContainsKey(session.Id))
                                           InternalData.Add(session.Id, session);

                                       if (session.EVSEId.HasValue && session.EVSE == null)
                                           session.EVSE = RoamingNetwork.GetEVSEById(session.EVSEId.Value);

                                       if (session.EVSE != null)
                                           session.EVSE.ChargingSession = session;

                                   }
                                   return true;

                               #endregion

                               #region "authStop"

                               case "authStop":
                                   {

                                       if (!InternalData.ContainsKey(session.Id))
                                           InternalData.Add(session.Id, session);
                                       else
                                           InternalData[session.Id] = session;

                                       if (session.EVSEId.HasValue && session.EVSE == null)
                                           session.EVSE = RoamingNetwork.GetEVSEById(session.EVSEId.Value);

                                       if (session.EVSE != null)
                                           session.EVSE.ChargingSession = null;

                                   }
                                   return true;

                               #endregion

                               #region "CDRReceived"

                               case "CDRReceived":
                                   {

                                       if (!InternalData.ContainsKey(session.Id))
                                           InternalData.Add(session.Id, session);
                                       else
                                           InternalData[session.Id] = session;

                                       if (session.EVSEId.HasValue && session.EVSE == null)
                                           session.EVSE = RoamingNetwork.GetEVSEById(session.EVSEId.Value);

                                       if (session.EVSE != null)
                                           session.EVSE.ChargingSession = null;

                                   }
                                   return true;

                               #endregion

                               #region "CDRForwarded"

                               case "CDRForwarded":
                                   {

                                       if (!InternalData.ContainsKey(session.Id))
                                           InternalData.Add(session.Id, session);
                                       else
                                           InternalData[session.Id] = session;

                                   }
                                   return true;

                               #endregion

                           }

                       }

                       return false;

                   },

                   DisableLogfiles:        DisableLogfiles,
                   LogFilePathCreator:     roamingNetworkId => "ChargingSessions" + Path.DirectorySeparatorChar,
                   LogFileNameCreator:     roamingNetworkId => String.Concat("ChargingSessions-",
                                                                             roamingNetworkId, "_",
                                                                             DateTime.UtcNow.Year, "-", DateTime.UtcNow.Month.ToString("D2"),
                                                                             ".log"),
                   ReloadDataOnStart:      ReloadDataOnStart,
                   LogfileSearchPattern:   roamingNetworkId => "ChargingSessions-" + roamingNetworkId + "_*.log",

                   RoamingNetworkInfos:    RoamingNetworkInfos,
                   DisableNetworkSync:     DisableNetworkSync,
                   DNSClient:              DNSClient)

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

        #region Remove(Id, Authentication)

        public void Remove(ChargingSession_Id  Id,
                           AAuthentication     Authentication)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingSession session))
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


        #region RemoteStart(NewChargingSession, Result, UpdateFunc = null)

        public Boolean RemoteStart(ChargingSession          NewChargingSession,
                                   RemoteStartResult        Result,
                                   Action<ChargingSession>  UpdateFunc = null)
        {

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(NewChargingSession.Id))
                {

                    NewChargingSession.SystemIdStart = System_Id.Parse(Environment.MachineName);
                    InternalData.Add(NewChargingSession.Id, NewChargingSession);
                    UpdateFunc?.Invoke(NewChargingSession);

                    LogIt("remoteStart",
                          NewChargingSession.Id,
                          "chargingSession",
                          NewChargingSession.ToJSON());

                    OnNewChargingSession?.Invoke(DateTime.UtcNow,
                                                 RoamingNetwork,
                                                 NewChargingSession);

                    return true;

                }

                else
                    return false;

            }

        }

        #endregion

        #region RemoteStop (Id, Authentication, ProviderId = null, CSORoamingProvider = null)

        public Boolean RemoteStop(ChargingSession_Id     Id,
                                  AAuthentication        Authentication,
                                  eMobilityProvider_Id?  ProviderId,
                                  IEMPRoamingProvider    CSORoamingProvider,
                                  RemoteStopResult       Result)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingSession session))
                {

                    session.SessionTime.EndTime     = DateTime.UtcNow;
                    session.SystemIdStop            = System_Id.Parse(Environment.MachineName);
                    session.EMPRoamingProviderStop  = CSORoamingProvider;
                    session.ProviderIdStop          = ProviderId;
                    session.AuthenticationStop      = Authentication;
                    session.CDR                     = Result.ChargeDetailRecord;
                    session.RuntimeStop             = Result.Runtime;

                    LogIt("remoteStop",
                          session.Id,
                          "chargingSession",
                          session.ToJSON());

                    return true;

                }

                else
                    return false;

            }

        }

        #endregion

        #region AuthStart(NewChargingSession, UpdateFunc = null)

        public Boolean AuthStart(ChargingSession          NewChargingSession,
                                 Action<ChargingSession>  UpdateFunc = null)
        {

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(NewChargingSession.Id))
                {

                    NewChargingSession.SystemIdStart = System_Id.Parse(Environment.MachineName);
                    InternalData.Add(NewChargingSession.Id, NewChargingSession);
                    UpdateFunc?.Invoke(NewChargingSession);

                    LogIt("authStart",
                          NewChargingSession.Id,
                          "chargingSession",
                          NewChargingSession.ToJSON());

                    OnNewChargingSession?.Invoke(DateTime.UtcNow,
                                                 RoamingNetwork,
                                                 NewChargingSession);

                    return true;

                }

                else
                    return false;

            }

        }

        #endregion

        #region AuthStop (Id, Authentication, ProviderId, CSORoamingProvider = null)

        public Boolean AuthStop(ChargingSession_Id    Id,
                                AAuthentication       Authentication,
                                eMobilityProvider_Id  ProviderId,
                                IEMPRoamingProvider   CSORoamingProvider  = null)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingSession session))
                {

                    session.SessionTime.EndTime     = DateTime.UtcNow;
                    session.SystemIdStop            = System_Id.Parse(Environment.MachineName);
                    session.EMPRoamingProviderStop  = CSORoamingProvider;
                    session.ProviderIdStop          = ProviderId;
                    session.AuthenticationStop      = Authentication;

                    LogIt("authStop",
                          session.Id,
                          "chargingSession",
                          session.ToJSON());

                    return true;

                }

                else
                    return false;

            }

        }

        #endregion

        #region CDRReceived(Id, NewChargeDetailRecord)

        public Boolean CDRReceived(ChargingSession_Id  Id,
                                   ChargeDetailRecord  NewChargeDetailRecord)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingSession session))
                {

                    // Most charging session will be stopped by just unplugging the socket!
                    if (!session.SessionTime.EndTime.HasValue)
                    {
                        session.SessionTime.EndTime  = DateTime.UtcNow;
                        session.SystemIdStop         = System_Id.Parse(Environment.MachineName);
                    }

                    session.CDRReceived  = DateTime.UtcNow;
                    session.SystemIdCDR  = System_Id.Parse(Environment.MachineName);
                    session.CDR          = NewChargeDetailRecord;

                    LogIt("CDRReceived",
                          session.Id,
                          "chargingSession",
                          session.ToJSON());

                    OnNewChargeDetailRecord?.Invoke(DateTime.UtcNow,
                                                    RoamingNetwork,
                                                    NewChargeDetailRecord);

                    return true;

                }

                else
                    return false;

            }

        }

        #endregion

        #region CDRForwarded(Id, SendCDRResult)

        public Boolean CDRForwarded(ChargingSession_Id  Id,
                                    SendCDRResult       CDRResult)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingSession session))
                {

                    session.CDRResult = CDRResult;

                    LogIt("CDRForwarded",
                          session.Id,
                          "chargingSession",
                          session.ToJSON());

                    OnNewChargeDetailRecordResult?.Invoke(DateTime.UtcNow,
                                                          RoamingNetwork,
                                                          CDRResult);

                    return true;

                }

                else
                    return false;

            }

        }

        #endregion


        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(ChargingSession_Id other)
        {
            throw new NotImplementedException();
        }


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


    }

}
