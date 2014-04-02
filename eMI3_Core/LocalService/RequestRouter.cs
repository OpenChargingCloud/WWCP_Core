/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 HTTP <http://www.github.com/eMI3/HTTP>
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Hermod;
using eu.Vanaheimr.Hermod.HTTP;
using eu.Vanaheimr.Hermod.Services.DNS;

#endregion

namespace org.emi3group.LocalService
{

    /// <summary>
    /// A simple router to dispatch incoming requests to different service
    /// implementations. The SessionId is used as a minimal state and routing
    /// key to avoid flooding.
    /// </summary>
    public class RequestRouter : IUpstreamEMobilityService,
                                 IDownstreamEMobilityService
    {

        #region Data

        private readonly Dictionary<UInt32,          IUpstreamEMobilityService>    AuthenticationServices;
        private readonly Dictionary<SessionId,       IUpstreamEMobilityService>    SessionIdAuthenticatorCache;
        private readonly Dictionary<EVSEOperator_Id, IDownstreamEMobilityService>  EVSEOperatorLookup;

        #endregion

        #region Properties

        #region AuthorizatorId

        private readonly String _AuthorizatorId;

        public String AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        public URLMapping FrontendHTTPServer { get; set; }

        #endregion

        #region Constructor(s)

        public RequestRouter(String AuthorizatorId = "Belectric Drive EV Gateway")
        {
            this._AuthorizatorId              = AuthorizatorId;
            this.AuthenticationServices       = new Dictionary<UInt32,          IUpstreamEMobilityService>();
            this.SessionIdAuthenticatorCache  = new Dictionary<SessionId,       IUpstreamEMobilityService>();
            this.EVSEOperatorLookup           = new Dictionary<EVSEOperator_Id, IDownstreamEMobilityService>();
        }

        #endregion


        #region RegisterService(Priority, AuthenticationService)

        public Boolean RegisterService(UInt32             Priority,
                                       IUpstreamEMobilityService  AuthenticationService)
        {

            lock (AuthenticationServices)
            {

                if (!AuthenticationServices.ContainsKey(Priority))
                {
                    AuthenticationServices.Add(Priority, AuthenticationService);
                    return true;
                }

                return false;

            }

        }

        #endregion


        #region AuthorizeStart(OperatorId, EVSEId, PartnerSessionId, UID)

        public AUTHSTARTResult AuthorizeStart(EVSEOperator_Id  OperatorId,
                                              EVSE_Id          EVSEId,
                                              SessionId        PartnerSessionId,
                                              Token            UID)
        {

            // Will store the SessionId in order to contact the right authenticator at later requests!

            lock (AuthenticationServices)
            {

                AUTHSTARTResult AuthStartResult;

                foreach (var AuthenticationService in AuthenticationServices.
                                                          OrderBy(v => v.Key).
                                                          Select(v => v.Value))
                {

                    AuthStartResult = AuthenticationService.AuthorizeStart(OperatorId, EVSEId, PartnerSessionId, UID);

                    #region Authorized

                    if (AuthStartResult.AuthorizationResult == AuthorizationResult.Authorized)
                    {

                        // Store the upstream SessionId and its AuthenticationService!
                        // Will be deleted when the CDRecord was sent!
                        SessionIdAuthenticatorCache.Add(AuthStartResult.SessionId, AuthenticationService);

                        return AuthStartResult;

                    }

                    #endregion

                    #region Blocked

                    else if (AuthStartResult.AuthorizationResult == AuthorizationResult.Blocked)
                        return AuthStartResult;

                    #endregion

                }

                #region ...else fail!

                return new AUTHSTARTResult(AuthorizatorId) {
                    AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    PartnerSessionId     = PartnerSessionId,
                    Description          = "No authorization service returned a positiv result!"
                };

                #endregion

            }

        }

        #endregion

        #region AuthorizeStop(OperatorId, EVSEId, SessionId, PartnerSessionId, UID)

        public AUTHSTOPResult AuthorizeStop(EVSEOperator_Id  OperatorId,
                                            EVSE_Id          EVSEId,
                                            SessionId        SessionId,
                                            SessionId        PartnerSessionId,
                                            Token            UID)
        {

            lock (AuthenticationServices)
            {

                AUTHSTOPResult         AuthStopResult;
                IUpstreamEMobilityService  AuthenticationService;

                #region An authenticator was found for the upstream SessionId!

                if (SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
                {

                    AuthStopResult = AuthenticationService.AuthorizeStop(OperatorId, EVSEId, SessionId, PartnerSessionId, UID);

                    if (AuthStopResult.AuthorizationResult == AuthorizationResult.Authorized ||
                        AuthStopResult.AuthorizationResult == AuthorizationResult.Blocked)
                        return AuthStopResult;

                }

                #endregion

                #region Try to find anyone who might kown anything about the given SessionId!

                foreach (var OtherAuthenticationService in AuthenticationServices.
                                                               OrderBy(v => v.Key).
                                                               Select(v => v.Value).
                                                               ToArray())
                {

                    AuthStopResult = OtherAuthenticationService.AuthorizeStop(OperatorId, EVSEId, SessionId, PartnerSessionId, UID);

                    if (AuthStopResult.AuthorizationResult == AuthorizationResult.Authorized ||
                        AuthStopResult.AuthorizationResult == AuthorizationResult.Blocked)
                        return AuthStopResult;

                }

                #endregion

                #region ...else fail!

                return new AUTHSTOPResult(AuthorizatorId) {
                    AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    PartnerSessionId     = PartnerSessionId,
                    Description          = "No authorization service returned a positiv result!"
                };

                #endregion

            }

        }

        #endregion

        #region SendCDR(EVSEId, SessionId, PartnerSessionId, PartnerProductId, UID, eMAId, ChargeStart, ChargeEnd, SessionStart = null, SessionEnd = null, MeterValueStart = null, MeterValueEnd = null)

        public SENDCDRResult SendCDR(EVSE_Id     EVSEId,
                                     SessionId   SessionId,
                                     SessionId   PartnerSessionId,
                                     String      PartnerProductId,
                                     Token       UID,
                                     eMA_Id      eMAId,
                                     DateTime    ChargeStart,
                                     DateTime    ChargeEnd,
                                     DateTime?   SessionStart    = null,
                                     DateTime?   SessionEnd      = null,
                                     Double?     MeterValueStart = null,
                                     Double?     MeterValueEnd   = null)
        {

            lock (AuthenticationServices)
            {

                SENDCDRResult              SENDCDRResult;
                IUpstreamEMobilityService  AuthenticationService;

                #region An authenticator was found for the upstream SessionId!

                if (SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
                {

                    SENDCDRResult = AuthenticationService.SendCDR(EVSEId,
                                                                  SessionId,
                                                                  PartnerSessionId,
                                                                  PartnerProductId,
                                                                  UID,
                                                                  eMAId,
                                                                  ChargeStart,
                                                                  ChargeEnd,
                                                                  SessionStart,
                                                                  SessionEnd,
                                                                  MeterValueStart,
                                                                  MeterValueEnd);

                    if (SENDCDRResult.State)
                    {

                        SessionIdAuthenticatorCache.Remove(SessionId);

                        return SENDCDRResult;

                    }

                }

                #endregion

                #region Try to find anyone who might kown anything about the given SessionId!

                foreach (var OtherAuthenticationService in AuthenticationServices.
                                                               OrderBy(v => v.Key).
                                                               Select(v => v.Value).
                                                               ToArray())
                {

                    SENDCDRResult = OtherAuthenticationService.SendCDR(EVSEId,
                                                                       SessionId,
                                                                       PartnerSessionId,
                                                                       PartnerProductId,
                                                                       UID,
                                                                       eMAId,
                                                                       ChargeStart,
                                                                       ChargeEnd,
                                                                       SessionStart,
                                                                       SessionEnd,
                                                                       MeterValueStart,
                                                                       MeterValueEnd);

                    if (SENDCDRResult.State)
                    {

                        SessionIdAuthenticatorCache.Remove(SessionId);

                        return SENDCDRResult;

                    }

                }

                #endregion

                #region ...else fail!

                return new SENDCDRResult(AuthorizatorId) {
                    State                = false,
                    PartnerSessionId     = PartnerSessionId,
                    Description          = "No authorization service returned a positiv result!"
                };

                #endregion

            }

        }

        #endregion



        #region RemoteStart(EVSEId, SessionId, ProviderId, eMAId)

        public RemoteStartResult RemoteStart(EVSE_Id               EVSEId,
                                             String                SessionId,
                                             EVServiceProvider_Id  ProviderId,
                                             eMA_Id                eMAId)
        {

            lock (AuthenticationServices)
            {

                #region (HTTP) Logging

                FrontendHTTPServer.EventSource(Semantics.DebugLog).
                    SubmitSubEvent("REMOTESTARTRequest",
                                   new JObject(
                                       new JProperty("Timestamp",   DateTime.Now.ToIso8601()),
                                       new JProperty("SessionId",   SessionId),
                                       new JProperty("ProviderId",  ProviderId.ToString()),
                                       new JProperty("EVSEId",      EVSEId.ToString()),
                                       new JProperty("eMAId",       eMAId.ToString())
                                   ).ToString().
                                     Replace(Environment.NewLine, ""));

                #endregion

                var RequestPDU   = "";
                var ResponsePDU  = "";
                var Result       = RemoteStartResult.Error;

                var _DNSClient = new DNSClient();
                var IPv4Addresses  = _DNSClient.Query<A>("portal.belectric-drive.de").Select(a => a.IPv4Address).ToArray();

                using (var HTTPClient1 = new HTTPClient(IPv4Addresses.First(), new IPPort(20080)))//, "portal.belectric-drive.de"))
                {

                    var HTTPRequestBuilder = HTTPClient1.CreateRequest(new HTTPMethod("REMOTESTART"),
                                                                       "/ps/rest/hubject/RNs/QA1/EVSEs/" + EVSEId.ToString().Replace("+", ""),
                                                                       HTTPReqBuilder => {
                                                                           HTTPReqBuilder.Host         = "portal.belectric-drive.de";
                                                                           HTTPReqBuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                           HTTPReqBuilder.Content      = new JObject(
                                                                                                             new JProperty("@context",   "http://emi3group.org/contexts/REMOTESTART-request.jsonld"),
                                                                                                             new JProperty("@id",        SessionId),
                                                                                                             new JProperty("ProviderId", ProviderId.ToString()),
                                                                                                             new JProperty("eMAId",      eMAId.ToString())
                                                                                                         ).ToString().
                                                                                                           ToUTF8Bytes();
                                                                           HTTPReqBuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                       });


                    RequestPDU = HTTPRequestBuilder.AsImmutable().EntirePDU;

                    var Task01 = HTTPClient1.Execute_Synced(HTTPRequestBuilder, Timeout: 60000);

                    ResponsePDU = Task01.EntirePDU;

                    // HTTP/1.1 200 OK
                    // Date: Fri, 28 Mar 2014 13:31:27 GMT
                    // Server: Apache/2.2.9 (Debian) mod_jk/1.2.26
                    // Content-Length: 34
                    // Content-Type: application/json
                    // 
                    // {
                    //   "code" : "EVSE_AlreadyInUse"
                    // }

                    JObject  JSONResponse = null;

                    try
                    {

                        JSONResponse = JObject.Parse(Task01.Content.ToUTF8String());
                        var ReturnCode   = JSONResponse["code"].ToString();

                        switch (ReturnCode)
                        {

                            case "EVSE_AlreadyInUse":
                                Result = RemoteStartResult.EVSE_AlreadyInUse;
                                break;

                            case "SessionId_AlreadyInUse":
                                Result = RemoteStartResult.SessionId_AlreadyInUse;
                                break;

                            case "EVSE_NotReachable":
                                Result = RemoteStartResult.EVSE_NotReachable;
                                break;

                            case "Start_Timeout":
                                Result = RemoteStartResult.Start_Timeout;
                                break;

                            case "Success":
                                Result = RemoteStartResult.Success;
                                break;

                            default:
                                Result = RemoteStartResult.Error;
                                break;

                        }

                    }
                    catch (Exception)
                    {
                        Result = RemoteStartResult.Error;
                    }

                }

                return Result;

            }

        }

        #endregion

        #region RemoteStart(EVSEId, SessionId, ProviderId)

        public RemoteStopResult RemoteStop(EVSE_Id               EVSEId,
                                           String                SessionId,
                                           EVServiceProvider_Id  ProviderId)
        {

            lock (AuthenticationServices)
            {

                #region (HTTP) Logging

                FrontendHTTPServer.EventSource(Semantics.DebugLog).
                    SubmitSubEvent("REMOTESTOPRequest",
                                   new JObject(
                                       new JProperty("Timestamp",   DateTime.Now.ToIso8601()),
                                       new JProperty("SessionId",   SessionId),
                                       new JProperty("ProviderId",  ProviderId.ToString()),
                                       new JProperty("EVSEId",      EVSEId.ToString())
                                   ).ToString().
                                     Replace(Environment.NewLine, ""));

                #endregion

                var RequestPDU   = "";
                var ResponsePDU  = "";
                var Result       = RemoteStopResult.Error;

                var _DNSClient     = new DNSClient();
                var IPv4Addresses  = _DNSClient.Query<A>("portal.belectric-drive.de").Select(a => a.IPv4Address).ToArray();

                using (var HTTPClient1 = new HTTPClient(IPv4Addresses.First(), new IPPort(20080)))//, "portal.belectric-drive.de"))
                {

                    var HTTPRequestBuilder = HTTPClient1.CreateRequest(new HTTPMethod("REMOTESTOP"),
                                                                       "/ps/rest/hubject/RNs/QA1/EVSEs/" + EVSEId.ToString().Replace("+", ""),
                                                                       HTTPReqBuilder => {
                                                                           HTTPReqBuilder.Host         = "portal.belectric-drive.de";
                                                                           HTTPReqBuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                           HTTPReqBuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                           HTTPReqBuilder.Content      = new JObject(
                                                                                                              new JProperty("@context",   "http://emi3group.org/contexts/REMOTESTOP-request.jsonld"),
                                                                                                              new JProperty("@id",        SessionId),
                                                                                                              new JProperty("ProviderId", ProviderId.ToString())
                                                                                                         ).ToString().
                                                                                                           ToUTF8Bytes();
                                                                       });


                    RequestPDU = HTTPRequestBuilder.AsImmutable().EntirePDU;

                    var Task01 = HTTPClient1.Execute_Synced(HTTPRequestBuilder, Timeout: 60000);

                    ResponsePDU = Task01.EntirePDU;

                    JObject  JSONResponse = null;

                    try
                    {

                        JSONResponse = JObject.Parse(Task01.Content.ToUTF8String());

                        switch (JSONResponse["code"].ToString())
                        {

                            //case "EVSE_AlreadyInUse":
                            //    HubjectCode         = "602";
                            //    HubjectDescription  = "EVSE is already in use!";
                            //    break;

                            //case "SessionId_AlreadyInUse":
                            //    HubjectCode         = "400";
                            //    HubjectDescription  = "Session is invalid";
                            //    break;

                            //case "EVSE_NotReachable":
                            //    HubjectCode         = "501";
                            //    HubjectDescription  = "Communication to EVSE failed!";
                            //    break;

                            case "Success":
                                Result = RemoteStopResult.Success;
                                break;

                            default:
                                Result = RemoteStopResult.Error;
                                break;

                        }


                    }
                    catch (Exception)
                    {
                        Result = RemoteStopResult.Error;
                    }

                }

                return Result;

            }

        }

        #endregion

    }


    public enum RemoteStartResult
    {
        Error,
        Success,
        EVSE_NotReachable,
        SessionId_AlreadyInUse,
        EVSE_AlreadyInUse,
        Start_Timeout
    }

    public enum RemoteStopResult
    {
        Error,
        Success
    }

}
