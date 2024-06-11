/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using cloud.charging.open.protocols.WWCP.Networking;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class SessionStopRequest
    {

        public DateTime                Timestamp               { get; }
        public System_Id               SystemId                { get; }
        public EMPRoamingProvider_Id?  CSORoamingProviderId    { get; }
        public CSORoamingProvider_Id?  EMPRoamingProviderId    { get; }
        public EMobilityProvider_Id?   ProviderId              { get; }
        public AAuthentication         Authentication          { get; }

        public RemoteStopResult        RemoteStopResult        { get; }


        public SessionStopRequest(DateTime                Timestamp,
                                  System_Id               SystemId,
                                  EMPRoamingProvider_Id?  CSORoamingProviderId,
                                  CSORoamingProvider_Id?  EMPRoamingProviderId,
                                  EMobilityProvider_Id?   ProviderId,
                                  AAuthentication         Authentication,
                                  RemoteStopResult        RemoteStopResult)
        {

            this.Timestamp              = Timestamp;
            this.SystemId               = SystemId;
            this.CSORoamingProviderId   = CSORoamingProviderId;
            this.EMPRoamingProviderId   = EMPRoamingProviderId;
            this.ProviderId             = ProviderId;
            this.Authentication         = Authentication;
            this.RemoteStopResult       = RemoteStopResult;

        }



        public JObject ToJSON(Boolean                                               Embedded                             = false,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordSerializer   = null,
                              CustomJObjectSerializerDelegate<SessionStopRequest>?  CustomSessionStopRequestSerializer   = null)

        {

            var json = JSONObject.Create(

                                 new JProperty("timestamp",               Timestamp.                 ToIso8601()),
                                 new JProperty("systemId",                SystemId.                  ToString()),

                           CSORoamingProviderId.HasValue
                               ? new JProperty("CSORoamingProviderId",    CSORoamingProviderId.Value.ToString())
                               : null,

                           EMPRoamingProviderId.HasValue
                               ? new JProperty("EMPRoamingProviderId",    EMPRoamingProviderId.Value.ToString())
                               : null,

                           ProviderId.HasValue
                               ? new JProperty("providerId",              ProviderId.          Value.ToString())
                               : null,

                           Authentication.IsDefined()
                               ? new JProperty("authentication",          Authentication.            ToJSON())
                               : null,

                           RemoteStopResult is not null
                               ? new JProperty("remoteStopResult",        RemoteStopResult.          ToJSON(Embedded: true,
                                                                                                            CustomChargeDetailRecordSerializer))
                               : null

                );

            return CustomSessionStopRequestSerializer is not null
                       ? CustomSessionStopRequestSerializer(this, json)
                       : json;

        }

        public static SessionStopRequest Parse(JObject JSON)
        {

            return new SessionStopRequest(JSON["timestamp"].Value<DateTime>(),
                                          System_Id.Parse(JSON["systemId"]?.Value<String>()),
                                          JSON["CSORoamingProviderId"] != null                        ? EMPRoamingProvider_Id.Parse(JSON["CSORoamingProviderId"]?.Value<String>()) : new EMPRoamingProvider_Id?(),
                                          JSON["EMPRoamingProviderId"] != null                        ? CSORoamingProvider_Id.Parse(JSON["EMPRoamingProviderId"]?.Value<String>()) : new CSORoamingProvider_Id?(),
                                          JSON["providerId"]           != null                        ? EMobilityProvider_Id. Parse(JSON["providerId"]?.Value<String>())           : new EMobilityProvider_Id?(),
                                          JSON["authentication"]       is JObject authenticationStart ? RemoteAuthentication. Parse(authenticationStart)                           : null,
                                          JSON["remoteStopResult"]     is JObject remoteStopResult    ? RemoteStopResult.     Parse(remoteStopResult)                              : null);

        }


    }


    /// <summary>
    /// A charging session data store.
    /// </summary>
    public class ChargingSessionsStore : ADataStore<ChargingSession_Id, ChargingSession>
    {

        #region Events

        /// <summary>
        /// An event fired whenever a new charging session was registered.
        /// </summary>
        public event OnNewChargingSessionDelegate?           OnNewChargingSession;

        /// <summary>
        /// An event fired whenever a new charge detail record was registered.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate?        OnNewChargeDetailRecord;

        /// <summary>
        /// An event fired whenever a new charge detail record was sent.
        /// </summary>
        public event OnNewChargeDetailRecordResultDelegate?  OnNewChargeDetailRecordResult;

        #endregion

        #region Properties

        /// <summary>
        /// The time span after which a successfully completed charging session
        /// will be removed from the store.
        /// </summary>
        public TimeSpan  SuccessfulSessionRemovalAfter      { get; set; } = TimeSpan.FromDays(7);

        /// <summary>
        /// The time span after which an unsuccessfully completed charging session
        /// will be removed from the store.
        /// </summary>
        public TimeSpan  UnsuccessfulSessionRemovalAfter    { get; set; } = TimeSpan.FromDays(90);


        /// <summary>
        /// The number of stored charging sessions.
        /// </summary>
        public UInt64 NumberOfStoredSessions
            => (UInt64) InternalData.Count;

        #endregion

        #region CustomSerializers

        public CustomJObjectSerializerDelegate<ChargingSession>?     CustomChargingSessionSerializer       { get; set; }
        public CustomJObjectSerializerDelegate<ReceivedCDRInfo>?     CustomCDRReceivedInfoSerializer       { get; set; }
        public CustomJObjectSerializerDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordSerializer    { get; set; }
        public CustomJObjectSerializerDelegate<SendCDRResult>?       CustomSendCDRResultSerializer         { get; set; }
        public CustomJObjectSerializerDelegate<Warning>?             CustomWarningSerializer               { get; set; }

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
        public ChargingSessionsStore(RoamingNetwork                    RoamingNetwork,

                                     Boolean                           DisableLogfiles       = false,
                                     Boolean                           ReloadDataOnStart     = true,

                                     IEnumerable<RoamingNetworkInfo>?  RoamingNetworkInfos   = null,
                                     Boolean                           DisableNetworkSync    = false,
                                     String?                           LoggingPath           = null,
                                     DNSClient?                        DNSClient             = null)

            : base(Name:                   nameof(ChargingSessionsStore),
                   RoamingNetworkId:       RoamingNetwork.Id,
                   StringIdParser:         ChargingSession_Id.TryParse,

                   CommandProcessor:       (logfilename,
                                            lineNumber,
                                            remoteSocket,
                                            timestamp,
                                            sessionId,
                                            command,
                                            json,
                                            internalData) => {

                       if (json["chargingSession"] is JObject chargingSessionJSON)
                       {

                           if (ChargingSession.TryParse(chargingSessionJSON,
                                                        out var chargingSession,
                                                        out var errorResponse))
                           {

                               chargingSession.RoamingNetwork ??= RoamingNetwork;

                               switch (command)
                               {

                                   #region "remoteStart"

                                   case "remoteStart":
                                       {

                                           if (!internalData.ContainsKey(chargingSession.Id))
                                               internalData.TryAdd(chargingSession.Id, chargingSession);

                                           if (chargingSession.EVSEId.HasValue && chargingSession.EVSE is null)
                                               chargingSession.EVSE = RoamingNetwork.GetEVSEById(chargingSession.EVSEId.Value);

                                           if (chargingSession.EVSE is not null)
                                               chargingSession.EVSE.ChargingSession = chargingSession;

                                       }
                                       return true;

                                   #endregion

                                   #region "remoteStop"

                                   case "remoteStop":
                                       {

                                           if (!internalData.ContainsKey(chargingSession.Id))
                                               internalData.TryAdd(chargingSession.Id, chargingSession);
                                           else
                                               internalData[chargingSession.Id] = chargingSession;

                                           if (chargingSession.EVSEId.HasValue && chargingSession.EVSE is null)
                                               chargingSession.EVSE = RoamingNetwork.GetEVSEById(chargingSession.EVSEId.Value);

                                           if (chargingSession.EVSE is not null)
                                               chargingSession.EVSE.ChargingSession = null;

                                       }
                                       return true;

                                   #endregion

                                   #region "authStart"

                                   case "authStart":
                                       {

                                           if (!internalData.ContainsKey(chargingSession.Id))
                                               internalData.TryAdd(chargingSession.Id, chargingSession);

                                           if (chargingSession.EVSEId.HasValue && chargingSession.EVSE is null)
                                               chargingSession.EVSE = RoamingNetwork.GetEVSEById(chargingSession.EVSEId.Value);

                                           if (chargingSession.EVSE is not null)
                                               chargingSession.EVSE.ChargingSession = chargingSession;

                                       }
                                       return true;

                                   #endregion

                                   #region "authStop"

                                   case "authStop":
                                       {

                                           if (!internalData.ContainsKey(chargingSession.Id))
                                               internalData.TryAdd(chargingSession.Id, chargingSession);
                                           else
                                               internalData[chargingSession.Id] = chargingSession;

                                           if (chargingSession.EVSEId.HasValue && chargingSession.EVSE is null)
                                               chargingSession.EVSE = RoamingNetwork.GetEVSEById(chargingSession.EVSEId.Value);

                                           if (chargingSession.EVSE is not null)
                                               chargingSession.EVSE.ChargingSession = null;

                                       }
                                       return true;

                                   #endregion

                                   #region "CDRReceived"

                                   case "CDRReceived":
                                       {

                                           if (!internalData.ContainsKey(chargingSession.Id))
                                               internalData.TryAdd(chargingSession.Id, chargingSession);
                                           else
                                               internalData[chargingSession.Id] = chargingSession;

                                           if (chargingSession.EVSEId.HasValue && chargingSession.EVSE is null)
                                               chargingSession.EVSE = RoamingNetwork.GetEVSEById(chargingSession.EVSEId.Value);

                                           if (chargingSession.EVSE is not null)
                                               chargingSession.EVSE.ChargingSession = null;

                                       }
                                       return true;

                                   #endregion

                                   #region "CDRForwarded"

                                   case "CDRForwarded":
                                       {

                                           if (!internalData.ContainsKey(chargingSession.Id))
                                               internalData.TryAdd(chargingSession.Id, chargingSession);
                                           else
                                               internalData[chargingSession.Id] = chargingSession;

                                       }
                                       return true;

                                   #endregion

                               }

                           }
                           else
                           {
                               DebugX.Log($"Could not parse charging session in {logfilename} line {lineNumber}:" + errorResponse);
                           }

                       }

                       else
                       {
                           switch (command)
                           {

                               #region "close" | "remove"

                               case "close":
                               case "remove":
                                   internalData.TryRemove(sessionId, out _);
                                   return true;

                               #endregion

                           }
                       }

                       return false;

                   },

                   DisableLogfiles:        DisableLogfiles,
                   LogFilePathCreator:     roamingNetworkId => Path.Combine(LoggingPath ?? AppContext.BaseDirectory, "ChargingSessions"),
                   LogFileNameCreator:     roamingNetworkId => $"ChargingSessions-{roamingNetworkId}_{Timestamp.Now.Year}-{Timestamp.Now.Month:D2}.log",
                   ReloadDataOnStart:      ReloadDataOnStart,
                   LogfileSearchPattern:   roamingNetworkId => $"ChargingSessions-{roamingNetworkId}_*.log",

                   RoamingNetworkInfos:    RoamingNetworkInfos,
                   DisableNetworkSync:     DisableNetworkSync,
                   DNSClient:              DNSClient)

        { }

        #endregion


        #region (override) DoMaintenance(State)

        protected internal async override Task DoMaintenance(Object? State)
        {

            var now                    = Timestamp.Now;
            var sessionIds             = InternalData.Keys.ToArray();
            var sessionIdsToBeRemoved  = new List<Tuple<ChargingSession_Id, String>>();

            foreach (var sessionId in sessionIds)
            {
                if (InternalData.TryGetValue(sessionId, out var chargingSession))
                {

                    if (chargingSession.NoAutoDeletionBefore.HasValue &&
                       (now < chargingSession.NoAutoDeletionBefore.Value))
                    {
                        continue;
                    }

                    if (chargingSession.SendCDRResults.Any() &&
                       (chargingSession.SendCDRResults.Last().Result == SendCDRResultTypes.Success ||
                        chargingSession.SendCDRResults.Last().Result == SendCDRResultTypes.Enqueued) &&
                       (now - chargingSession.SendCDRResults.Last().  Timestamp > SuccessfulSessionRemovalAfter))
                    {
                        sessionIdsToBeRemoved.Add(new Tuple<ChargingSession_Id, String>(sessionId, "close"));
                        continue;
                    }

                    if (now - chargingSession.SessionTime.StartTime > UnsuccessfulSessionRemovalAfter)
                    {
                        sessionIdsToBeRemoved.Add(new Tuple<ChargingSession_Id, String>(sessionId, "remove"));
                        continue;
                    }

                }
            }


            if (sessionIdsToBeRemoved.Count > 0)
            {

                var deleted = 0UL;

                foreach (var sessionTuple in sessionIdsToBeRemoved)
                {
                    if (InternalData.TryRemove(sessionTuple.Item1, out _))
                    {

                        await LogIt(
                                  sessionTuple.Item2,  // "close" | "remove"
                                  sessionTuple.Item1
                              );

                        deleted++;

                    }
                }

                DebugX.Log($"{Name}: Deleted {deleted} charging sessions!");

            }

        }

        #endregion


        #region AddSession         (ChargingSession, NoAutoDeletionBefore = false)

        public async Task AddSession(ChargingSession  ChargingSession,
                                     DateTime?        NoAutoDeletionBefore   = null)
        {

            if (NoAutoDeletionBefore.HasValue)
                ChargingSession.NoAutoDeletionBefore = NoAutoDeletionBefore;

            if (InternalData.TryAdd(ChargingSession.Id, ChargingSession))
            {

                await LogIt("new",
                            ChargingSession.Id,
                            "chargingSession",
                            ChargingSession.ToJSON(Embedded:    true,
                                                   OnlineInfos: false,
                                                   CustomChargingSessionSerializer,
                                                   CustomCDRReceivedInfoSerializer,
                                                   CustomChargeDetailRecordSerializer,
                                                   CustomSendCDRResultSerializer));

            }

        }

        #endregion

        #region AddOrUpdateSession (ChargingSession, UpdateFunc)

        public async Task AddOrUpdateSession(ChargingSession                         ChargingSession,
                                             Func<ChargingSession, ChargingSession>  UpdateFunc)
        {

            var updated = false;

            ChargingSession updateFunc(ChargingSession CS) {
                updated = true;
                return UpdateFunc(CS);
            }

            var result  = InternalData.AddOrUpdate(
                              ChargingSession.Id,
                              ChargingSession,
                              (sessionId, oldSession) => updateFunc(ChargingSession)
                          );


            if (updated)
                await LogIt("update",
                            ChargingSession.Id,
                            "chargingSession",
                            ChargingSession.ToJSON(Embedded:    true,
                                                   OnlineInfos: false,
                                                   CustomChargingSessionSerializer,
                                                   CustomCDRReceivedInfoSerializer,
                                                   CustomChargeDetailRecordSerializer,
                                                   CustomSendCDRResultSerializer));

            else
                await LogIt("new",
                            ChargingSession.Id,
                            "chargingSession",
                            ChargingSession.ToJSON(Embedded:    true,
                                                   OnlineInfos: false,
                                                   CustomChargingSessionSerializer,
                                                   CustomCDRReceivedInfoSerializer,
                                                   CustomChargeDetailRecordSerializer,
                                                   CustomSendCDRResultSerializer));

        }

        #endregion

        #region UpdateSession      (Id, UpdateFunc)

        public async Task UpdateSession(ChargingSession_Id       Id,
                                        Action<ChargingSession>  UpdateFunc)
        {

            if (InternalData.TryGetValue(Id, out var chargingSession))
            {

                UpdateFunc(chargingSession);

                await LogIt("update",
                            Id,
                            "chargingSession",
                            chargingSession.ToJSON(Embedded:    true,
                                                   OnlineInfos: false,
                                                   CustomChargingSessionSerializer,
                                                   CustomCDRReceivedInfoSerializer,
                                                   CustomChargeDetailRecordSerializer,
                                                   CustomSendCDRResultSerializer));

            }

        }

        #endregion

        #region SessionExists      (Id)

        public Boolean SessionExists(ChargingSession_Id Id)

            => InternalData.ContainsKey(Id);

        public Boolean SessionExists(ChargingSession_Id? Id)

            => Id.HasValue && InternalData.ContainsKey(Id.Value);

        #endregion

        #region RemoveSession      (Id, Authentication)

        public async Task RemoveSession(ChargingSession_Id  Id,
                                        AAuthentication     Authentication)
        {

            if (InternalData.TryRemove(Id, out var chargingSession))
            {

                chargingSession.AuthenticationStop = Authentication;

                await LogIt("remove",
                            chargingSession.Id,
                            "chargingSession",
                            chargingSession.ToJSON(Embedded:    true,
                                                   OnlineInfos: false,
                                                   CustomChargingSessionSerializer,
                                                   CustomCDRReceivedInfoSerializer,
                                                   CustomChargeDetailRecordSerializer,
                                                   CustomSendCDRResultSerializer));

            }

        }

        #endregion

        #region RemoveSession      (ChargingSession)

        public async Task RemoveSession(ChargingSession ChargingSession)
        {

            if (InternalData.TryRemove(ChargingSession.Id, out var chargingSession))
            {

                await LogIt("remove",
                            ChargingSession.Id,
                            "chargingSession",
                            ChargingSession.ToJSON(Embedded:    true,
                                                   OnlineInfos: false,
                                                   CustomChargingSessionSerializer,
                                                   CustomCDRReceivedInfoSerializer,
                                                   CustomChargeDetailRecordSerializer,
                                                   CustomSendCDRResultSerializer));

            }

        }

        #endregion



        #region RemoteStart  (EventTrackingId, NewChargingSession, Result, UpdateFunc = null)

        public async Task<Boolean> RemoteStart(EventTracking_Id          EventTrackingId,
                                               ChargingSession           NewChargingSession,
                                               RemoteStartResult         Result,
                                               Action<ChargingSession>?  UpdateFunc = null)
        {

            var now       = Timestamp.Now;
            var systemId  = System_Id.Parse(Environment.MachineName);

            if (!InternalData.ContainsKey(NewChargingSession.Id))
            {

                NewChargingSession.SystemIdStart = systemId;
                InternalData.TryAdd(NewChargingSession.Id, NewChargingSession);
                UpdateFunc?.Invoke(NewChargingSession);

                await LogIt("remoteStart",
                            NewChargingSession.Id,
                            "chargingSession",
                            NewChargingSession.ToJSON(Embedded:    true,
                                                      OnlineInfos: false,
                                                      CustomChargingSessionSerializer,
                                                      CustomCDRReceivedInfoSerializer,
                                                      CustomChargeDetailRecordSerializer,
                                                      CustomSendCDRResultSerializer));

                OnNewChargingSession?.Invoke(now,
                                             RoamingNetworkId,
                                             NewChargingSession);

                return true;

            }

            return false;

        }

        #endregion

        #region RemoteStop   (EventTrackingId, Id, Authentication, ProviderId = null, CSORoamingProvider = null)

        public async Task<Boolean> RemoteStop(EventTracking_Id       EventTrackingId,
                                              ChargingSession_Id     Id,
                                              AAuthentication?       Authentication,
                                              EMobilityProvider_Id?  ProviderId,
                                              ICSORoamingProvider?   CSORoamingProvider,
                                              RemoteStopResult       Result)
        {

            if (InternalData.TryGetValue(Id, out var chargingSession))
            {

                var now       = Timestamp.Now;
                var systemId  = System_Id.Parse(Environment.MachineName);

                if (Result.Result == RemoteStopResultTypes.Success)
                {

                    chargingSession.SessionTime.EndTime     = now;
                    chargingSession.SystemIdStop            = systemId;
                    chargingSession.CSORoamingProviderStop  = CSORoamingProvider;
                    chargingSession.ProviderIdStop          = ProviderId;
                    chargingSession.AuthenticationStop      = Authentication;
                    chargingSession.RuntimeStop             = Result.Runtime;

                    if (Result.ChargeDetailRecord is not null)
                        chargingSession.AddCDRReceivedInfo(
                            new ReceivedCDRInfo(

                                now,
                                systemId,
                                EventTrackingId,
                                Result.ChargeDetailRecord,

                                SendCDRResult.Success(
                                    Timestamp:            now,
                                    AuthorizatorId:       systemId,
                                    ChargeDetailRecord:   Result.ChargeDetailRecord,
                                    Description:          null,
                                    Warnings:             null,
                                    Location:             null,
                                    Runtime:              null
                                )

                            )
                        );

                }

                chargingSession.AddStopRequest(new SessionStopRequest(
                                                   now,
                                                   systemId,
                                                   null,
                                                   CSORoamingProvider?.Id,
                                                   ProviderId,
                                                   Authentication,
                                                   Result
                                               ));

                await LogIt("remoteStop",
                            chargingSession.Id,
                            "chargingSession",
                            chargingSession.ToJSON(Embedded:    true,
                                                   OnlineInfos: false,
                                                   CustomChargingSessionSerializer,
                                                   CustomCDRReceivedInfoSerializer,
                                                   CustomChargeDetailRecordSerializer,
                                                   CustomSendCDRResultSerializer));

                return true;

            }

            return false;

        }

        #endregion


        #region AuthStart    (NewChargingSession, UpdateFunc = null)

        public async Task<Boolean> AuthStart(ChargingSession           NewChargingSession,
                                             Action<ChargingSession>?  UpdateFunc   = null)
        {

            if (!InternalData.ContainsKey(NewChargingSession.Id))
            {

                var now       = Timestamp.Now;
                var systemId  = System_Id.Parse(Environment.MachineName);

                NewChargingSession.SystemIdStart = systemId;
                InternalData.TryAdd(NewChargingSession.Id, NewChargingSession);
                UpdateFunc?.Invoke(NewChargingSession);

                await LogIt("authStart",
                            NewChargingSession.Id,
                            "chargingSession",
                            NewChargingSession.ToJSON(Embedded:    true,
                                                      OnlineInfos: false,
                                                      CustomChargingSessionSerializer,
                                                      CustomCDRReceivedInfoSerializer,
                                                      CustomChargeDetailRecordSerializer,
                                                      CustomSendCDRResultSerializer));

                OnNewChargingSession?.Invoke(now,
                                             RoamingNetworkId,
                                             NewChargingSession);

                return true;

            }

            return false;

        }

        #endregion

        #region AuthStop     (Id, Authentication, ProviderId, CSORoamingProvider = null)

        public async Task<Boolean> AuthStop(ChargingSession_Id    Id,
                                            AAuthentication       Authentication,
                                            EMobilityProvider_Id  ProviderId,
                                            ICSORoamingProvider?  CSORoamingProvider   = null)
        {

            if (InternalData.TryGetValue(Id, out var chargingSession))
            {

                var now       = Timestamp.Now;
                var systemId  = System_Id.Parse(Environment.MachineName);

                chargingSession.SessionTime.EndTime     = now;
                chargingSession.SystemIdStop            = systemId;
                chargingSession.CSORoamingProviderStop  = CSORoamingProvider;
                chargingSession.ProviderIdStop          = ProviderId;
                chargingSession.AuthenticationStop      = Authentication;

                await LogIt("authStop",
                            chargingSession.Id,
                            "chargingSession",
                            chargingSession.ToJSON(Embedded:    true,
                                                   OnlineInfos: false,
                                                   CustomChargingSessionSerializer,
                                                   CustomCDRReceivedInfoSerializer,
                                                   CustomChargeDetailRecordSerializer,
                                                   CustomSendCDRResultSerializer));

                return true;

            }

            return false;

        }

        #endregion


        #region CDRReceived  (EventTrackingId, Id, NewChargeDetailRecord)

        public async Task<Boolean> CDRReceived(EventTracking_Id    EventTrackingId,
                                               ChargingSession_Id  Id,
                                               ChargeDetailRecord  NewChargeDetailRecord)
        {

            if (InternalData.TryGetValue(Id, out var chargingSession))
            {

                var now       = Timestamp.Now;
                var systemId  = System_Id.Parse(Environment.MachineName);

                // Most charging session will be stopped by just unplugging the socket!
                if (!chargingSession.SessionTime.EndTime.HasValue)
                {
                    chargingSession.SessionTime.EndTime  = now;
                    chargingSession.SystemIdStop         = systemId;
                }

                chargingSession.AddCDRReceivedInfo(
                    new ReceivedCDRInfo(

                        now,
                        systemId,
                        EventTrackingId,
                        NewChargeDetailRecord,

                        SendCDRResult.Success(
                            Timestamp:            now,
                            AuthorizatorId:       systemId,
                            ChargeDetailRecord:   NewChargeDetailRecord,
                            Description:          null,
                            Warnings:             null,
                            Location:             null,
                            Runtime:              null
                        )

                    )
                );

                await LogIt("CDRReceived",
                            chargingSession.Id,
                            "chargingSession",
                            chargingSession.ToJSON(Embedded:    true,
                                                   OnlineInfos: false,
                                                   CustomChargingSessionSerializer,
                                                   CustomCDRReceivedInfoSerializer,
                                                   CustomChargeDetailRecordSerializer,
                                                   CustomSendCDRResultSerializer));

                OnNewChargeDetailRecord?.Invoke(now,
                                                RoamingNetworkId,
                                                NewChargeDetailRecord);

                return true;

            }

            return false;

        }

        #endregion

        #region CDRForwarded (Id, SendCDRResult)

        public async Task<Boolean> CDRForwarded(ChargingSession_Id  Id,
                                                SendCDRResult       SendCDRResult)
        {

            if (InternalData.TryGetValue(Id, out var chargingSession))
            {

                var now       = Timestamp.Now;
                var systemId  = System_Id.Parse(Environment.MachineName);

                chargingSession.AddCDRResult(SendCDRResult);

                await LogIt("CDRForwarded",
                            chargingSession.Id,
                            "chargingSession",
                            chargingSession.ToJSON(Embedded:    true,
                                                   OnlineInfos: false,
                                                   CustomChargingSessionSerializer,
                                                   CustomCDRReceivedInfoSerializer,
                                                   CustomChargeDetailRecordSerializer,
                                                   CustomSendCDRResultSerializer));

                OnNewChargeDetailRecordResult?.Invoke(now,
                                                      RoamingNetworkId,
                                                      SendCDRResult);

                return true;

            }

            return false;

        }

        #endregion


    }

}
