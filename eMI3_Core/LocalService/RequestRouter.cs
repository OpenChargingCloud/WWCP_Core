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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace org.emi3group.LocalService
{

    /// <summary>
    /// A simple router to dispatch incoming requests to different service
    /// implementations. The SessionId is used as a minimal state and routing
    /// key to avoid flooding.
    /// </summary>
    public class RequestRouter : IEMobilityService
    {

        #region Data

        private readonly Dictionary<UInt32, IEMobilityService>  AuthenticationServices;
        private readonly Dictionary<String, IEMobilityService>  SessionIdAuthenticatorCache;

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

        #endregion

        #region Constructor(s)

        public RequestRouter(String AuthorizatorId = "Belectric Drive EV Gateway")
        {
            this._AuthorizatorId              = AuthorizatorId;
            this.AuthenticationServices       = new Dictionary<UInt32, IEMobilityService>();
            this.SessionIdAuthenticatorCache  = new Dictionary<String, IEMobilityService>();
        }

        #endregion


        #region RegisterService(Priority, AuthenticationService)

        public Boolean RegisterService(UInt32             Priority,
                                       IEMobilityService  AuthenticationService)
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
                                              String           PartnerSessionId,
                                              String           UID)
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
                                            String           SessionId,
                                            String           PartnerSessionId,
                                            String           UID)
        {

            lock (AuthenticationServices)
            {

                AUTHSTOPResult         AuthStopResult;
                IEMobilityService  AuthenticationService;

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

        #region SendCDR(EVSEId, SessionId, PartnerSessionId, PartnerProductId, UID, ChargeStart, ChargeEnd, SessionStart = null, SessionEnd = null, MeterValueStart = null, MeterValueEnd = null)

        public SENDCDRResult SendCDR(EVSE_Id     EVSEId,
                                     String      SessionId,
                                     String      PartnerSessionId,
                                     String      PartnerProductId,
                                     String      UID,
                                     DateTime    ChargeStart,
                                     DateTime    ChargeEnd,
                                     DateTime?   SessionStart    = null,
                                     DateTime?   SessionEnd      = null,
                                     Double?     MeterValueStart = null,
                                     Double?     MeterValueEnd   = null)
        {

            lock (AuthenticationServices)
            {

                SENDCDRResult          SENDCDRResult;
                IEMobilityService  AuthenticationService;

                #region An authenticator was found for the upstream SessionId!

                if (SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
                {

                    SENDCDRResult = AuthenticationService.SendCDR(EVSEId,
                                                                  SessionId,
                                                                  PartnerSessionId,
                                                                  PartnerProductId,
                                                                  UID,
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

    }

}
