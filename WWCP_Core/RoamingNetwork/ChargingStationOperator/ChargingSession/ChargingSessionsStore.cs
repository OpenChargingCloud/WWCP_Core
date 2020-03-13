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
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

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
        public event OnNewChargingSessionDelegate OnNewChargingSession;

        #endregion

        #region Constructor(s)

        public ChargingSessionsStore(IRoamingNetwork                  RoamingNetwork,
                                     IEnumerable<RoamingNetworkInfo>  RoamingNetworkInfos   = null,
                                     Boolean                          DisableLogfiles       = false,
                                     Boolean                          ReloadDataOnStart     = true,

                                     Boolean                          DisableNetworkSync    = false,
                                     DNSClient                        DNSClient             = null)

            : base(RoamingNetwork:          RoamingNetwork,
                   RoamingNetworkInfos:     RoamingNetworkInfos,

                   DisableLogfiles:         DisableLogfiles,
                   LogFilePath:             "ChargingSessions" + Path.DirectorySeparatorChar,
                   LogFileNameCreator:      roamingNetworkId => String.Concat("ChargingSessions-",
                                                                              roamingNetworkId, "_",
                                                                              DateTime.UtcNow.Year, "-", DateTime.UtcNow.Month.ToString("D2"),
                                                                              ".log"),
                   ReloadDataOnStart:       ReloadDataOnStart,
                   LogfileSearchPattern:    roamingNetworkId => "ChargingSessions-" + roamingNetworkId + "_*.log",
                   LogFileParser:           (logfilename, command, json) => {

                       if (json["chargingSession"] is JObject chargingSession)
                       {

                           var session = ChargingSession.Parse(chargingSession);
                           session.RoamingNetworkId           = chargingSession["roamingNetworkId"]          != null ? RoamingNetwork_Id.         Parse(chargingSession["roamingNetworkId"]?.         Value<String>()) : new RoamingNetwork_Id?();
                           session.CSORoamingProviderIdStart       = chargingSession["CSORoamingProviderId"]      != null ? CSORoamingProvider_Id.     Parse(chargingSession["CSORoamingProviderId"]?.     Value<String>()) : new CSORoamingProvider_Id?();
                           session.EMPRoamingProviderIdStart       = chargingSession["EMPRoamingProviderId"]      != null ? EMPRoamingProvider_Id.     Parse(chargingSession["EMPRoamingProviderId"]?.     Value<String>()) : new EMPRoamingProvider_Id?();
                           session.ChargingStationOperatorId  = chargingSession["chargingStationOperatorId"] != null ? ChargingStationOperator_Id.Parse(chargingSession["chargingStationOperatorId"]?.Value<String>()) : new ChargingStationOperator_Id?();
                           session.ChargingPoolId             = chargingSession["chargingPoolId"]            != null ? ChargingPool_Id.           Parse(chargingSession["chargingPoolId"]?.           Value<String>()) : new ChargingPool_Id?();
                           session.ChargingStationId          = chargingSession["chargingStationId"]         != null ? ChargingStation_Id.        Parse(chargingSession["chargingStationId"]?.        Value<String>()) : new ChargingStation_Id?();
                           session.EVSEId                     = chargingSession["EVSEId"]                    != null ? EVSE_Id.                   Parse(chargingSession["EVSEId"]?.                   Value<String>()) : new EVSE_Id?();

                           if (chargingSession["energyMeterValues"] is JObject energyMeterValueObject)
                           {

                           }

                           else if (chargingSession["energyMeterValues"] is JArray energyMeterValueArray)
                           {

                           }


                           switch (command)
                           {

                               case "remoteStart":
                                   {
                                       if (chargingSession["start"] is JObject remoteStartObject)
                                       {

                                           var startTime = remoteStartObject["timestamp"]?.Value<DateTime>();

                                           if (startTime != null)
                                           {

                                               session.SessionTime                = new StartEndDateTime(startTime.Value);

                                               session.EMPRoamingProviderIdStart  = remoteStartObject["EMPRoamingProviderId"] != null                  ? EMPRoamingProvider_Id.Parse(remoteStartObject["EMPRoamingProviderId"]?.Value<String>()) : new EMPRoamingProvider_Id?();
                                               session.CSORoamingProviderIdStart  = remoteStartObject["CSORoamingProviderId"] != null                  ? CSORoamingProvider_Id.Parse(remoteStartObject["CSORoamingProviderId"]?.Value<String>()) : new CSORoamingProvider_Id?();
                                               session.ProviderIdStart            = remoteStartObject["providerId"]           != null                  ? eMobilityProvider_Id. Parse(remoteStartObject["providerId"]?.Value<String>())           : new eMobilityProvider_Id?();
                                               session.AuthenticationStart        = remoteStartObject["authentication"] is JObject authenticationStart ? RemoteAuthentication. Parse(authenticationStart)                                        : null;

                                           }

                                       }
                                   }
                                   break;

                               case "remoteStop":
                                   {
                                       if (chargingSession["start"] is JObject remoteStartObject && chargingSession["stop"] is JObject remoteStopObject)
                                       {

                                           var startTime = remoteStartObject["timestamp"]?.Value<DateTime>();
                                           var stopTime  = remoteStopObject ["timestamp"]?.Value<DateTime>();

                                           if (startTime != null && stopTime != null)
                                           {

                                               session.SessionTime                = new StartEndDateTime(startTime.Value, stopTime.Value);

                                               session.EMPRoamingProviderIdStart  = remoteStartObject["EMPRoamingProviderId"] != null                  ? EMPRoamingProvider_Id.Parse(remoteStartObject["EMPRoamingProviderId"]?.Value<String>()) : new EMPRoamingProvider_Id?();
                                               session.CSORoamingProviderIdStart  = remoteStartObject["CSORoamingProviderId"] != null                  ? CSORoamingProvider_Id.Parse(remoteStartObject["CSORoamingProviderId"]?.Value<String>()) : new CSORoamingProvider_Id?();
                                               session.ProviderIdStart            = remoteStartObject["providerId"]           != null                  ? eMobilityProvider_Id. Parse(remoteStartObject["providerId"]?.Value<String>())           : new eMobilityProvider_Id?();
                                               session.AuthenticationStart        = remoteStartObject["authentication"] is JObject authenticationStart ? RemoteAuthentication. Parse(authenticationStart)                                        : null;

                                               session.EMPRoamingProviderIdStop   = remoteStopObject ["EMPRoamingProviderId"] != null                  ? EMPRoamingProvider_Id.Parse(remoteStopObject ["EMPRoamingProviderId"]?.Value<String>()) : new EMPRoamingProvider_Id?();
                                               session.CSORoamingProviderIdStop   = remoteStopObject ["CSORoamingProviderId"] != null                  ? CSORoamingProvider_Id.Parse(remoteStopObject ["CSORoamingProviderId"]?.Value<String>()) : new CSORoamingProvider_Id?();
                                               session.ProviderIdStop             = remoteStopObject ["providerId"]           != null                  ? eMobilityProvider_Id. Parse(remoteStopObject ["providerId"]?.Value<String>())           : new eMobilityProvider_Id?();
                                               session.AuthenticationStop         = remoteStopObject ["authentication"] is JObject authenticationStop  ? LocalAuthentication.  Parse(authenticationStop)                                         : null;

                                           }

                                       }
                                   }
                                   break;

                               case "authStart":
                                   {
                                       if (chargingSession["start"] is JObject authStartObject)
                                       {

                                           var startTime = authStartObject["timestamp"]?.Value<DateTime>();

                                           if (startTime != null)
                                           {

                                               session.SessionTime                = new StartEndDateTime(startTime.Value);

                                               session.EMPRoamingProviderIdStart  = authStartObject["EMPRoamingProviderId"] != null                  ? EMPRoamingProvider_Id.Parse(authStartObject["EMPRoamingProviderId"]?.Value<String>()) : new EMPRoamingProvider_Id?();
                                               session.CSORoamingProviderIdStart  = authStartObject["CSORoamingProviderId"] != null                  ? CSORoamingProvider_Id.Parse(authStartObject["CSORoamingProviderId"]?.Value<String>()) : new CSORoamingProvider_Id?();
                                               session.ProviderIdStart            = authStartObject["providerId"]           != null                  ? eMobilityProvider_Id. Parse(authStartObject["providerId"]?.          Value<String>()) : new eMobilityProvider_Id?();
                                               session.AuthenticationStart        = authStartObject["authentication"] is JObject authenticationStart ? LocalAuthentication.  Parse(authenticationStart)                                      : null;

                                           }

                                       }
                                   }
                                   break;

                               case "authStop":
                                   {
                                       if (chargingSession["start"] is JObject authStartObject && chargingSession["stop"] is JObject authStopObject)
                                       {

                                           var startTime = authStartObject["timestamp"]?.Value<DateTime>();
                                           var stopTime  = authStopObject ["timestamp"]?.Value<DateTime>();

                                           if (startTime != null && stopTime != null)
                                           {

                                               session.SessionTime                = new StartEndDateTime(startTime.Value, stopTime.Value);

                                               session.EMPRoamingProviderIdStart  = authStartObject["EMPRoamingProviderId"] != null                  ? EMPRoamingProvider_Id.Parse(authStartObject["EMPRoamingProviderId"]?.Value<String>()) : new EMPRoamingProvider_Id?();
                                               session.CSORoamingProviderIdStart  = authStartObject["CSORoamingProviderId"] != null                  ? CSORoamingProvider_Id.Parse(authStartObject["CSORoamingProviderId"]?.Value<String>()) : new CSORoamingProvider_Id?();
                                               session.ProviderIdStart            = authStartObject["providerId"]           != null                  ? eMobilityProvider_Id. Parse(authStartObject["providerId"]?.          Value<String>()) : new eMobilityProvider_Id?();
                                               session.AuthenticationStart        = authStartObject["authentication"] is JObject authenticationStart ? LocalAuthentication.  Parse(authenticationStart)      : null;

                                               session.EMPRoamingProviderIdStop   = authStopObject ["EMPRoamingProviderId"] != null                  ? EMPRoamingProvider_Id.Parse(authStopObject ["EMPRoamingProviderId"]?.Value<String>()) : new EMPRoamingProvider_Id?();
                                               session.CSORoamingProviderIdStop   = authStopObject ["CSORoamingProviderId"] != null                  ? CSORoamingProvider_Id.Parse(authStopObject ["CSORoamingProviderId"]?.Value<String>()) : new CSORoamingProvider_Id?();
                                               session.ProviderIdStop             = authStopObject ["providerId"]           != null                  ? eMobilityProvider_Id. Parse(authStopObject ["providerId"]?.          Value<String>()) : new eMobilityProvider_Id?();
                                               session.AuthenticationStop         = authStopObject ["authentication"] is JObject authenticationStop  ? LocalAuthentication.  Parse(authenticationStop)       : null;

                                           }

                                       }
                                   }
                                   break;

                               case "sendCDR":
                                   {



                                   }
                                   break;

                           }


                           return session;

                       }

                       return null;

                   },

                   DisableNetworkSync:      DisableNetworkSync,
                   DNSClient:               DNSClient)

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


        #region RemoteStart(NewChargingSession, UpdateFunc = null)

        public Boolean RemoteStart(ChargingSession          NewChargingSession,
                                   Action<ChargingSession>  UpdateFunc = null)
        {

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(NewChargingSession.Id))
                {

                    InternalData.Add(NewChargingSession.Id, NewChargingSession);
                    UpdateFunc?.Invoke(NewChargingSession);

                    LogIt("remoteStart",
                          NewChargingSession.Id,
                          "chargingSession",
                          NewChargingSession.ToJSON());

                    return true;

                }

                else
                    return false;

            }

        }

        public Boolean RemoteStart2(ChargingSession          NewChargingSession,
                                    Action<ChargingSession>  UpdateFunc = null)
        {

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(NewChargingSession.Id))
                {

                    InternalData.Add(NewChargingSession.Id, NewChargingSession);
                    UpdateFunc?.Invoke(NewChargingSession);

                    LogIt("remoteStart",
                          NewChargingSession.Id,
                          "chargingSession",
                          NewChargingSession.ToJSON());

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
                                  eMobilityProvider_Id?  ProviderId           = null,
                                  ICSORoamingProvider    CSORoamingProvider   = null)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingSession session))
                {

                    session.SessionTime.EndTime     = DateTime.UtcNow;
                    session.CSORoamingProviderStop  = CSORoamingProvider;
                    session.ProviderIdStop          = ProviderId;
                    session.AuthenticationStop      = Authentication;

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

                    InternalData.Add(NewChargingSession.Id, NewChargingSession);
                    UpdateFunc?.Invoke(NewChargingSession);

                    LogIt("authStart",
                          NewChargingSession.Id,
                          "chargingSession",
                          NewChargingSession.ToJSON());

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
                                ICSORoamingProvider   CSORoamingProvider  = null)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingSession session))
                {

                    session.SessionTime.EndTime     = DateTime.UtcNow;
                    session.CSORoamingProviderStop  = CSORoamingProvider;
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

        #region SendCDR (Id)

        public Boolean SendCDR(ChargingSession_Id  Id,
                               ChargeDetailRecord  CDR)
        {

            lock (InternalData)
            {

                if (InternalData.TryGetValue(Id, out ChargingSession session))
                {

                    LogIt("sendCDR",
                          session.Id,
                          "chargeDetailRecord",
                          CDR.ToJSON());

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
