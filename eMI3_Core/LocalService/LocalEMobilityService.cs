/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

#endregion

namespace org.emi3group.LocalService
{

    /// <summary>
    /// A local E-Mobility service implementation.
    /// </summary>
    public class LocalEMobilityService : IEVSEOperator2HubjectService
    {

        #region Data

        private readonly Dictionary<Token,     AuthorizationResult>  AuthorizationDatabase;
        private readonly Dictionary<ChargingSessionId, SessionInfo>          SessionDatabase;

        #endregion

        #region Properties

        #region EVSPId

        private readonly EVServiceProvider_Id _EVSPId;

        public EVServiceProvider_Id EVSPId
        {
            get
            {
                return _EVSPId;
            }
        }

        #endregion

        #region AuthorizatorId

        private readonly AuthorizatorId _AuthorizatorId;

        public AuthorizatorId AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public LocalEMobilityService(EVServiceProvider_Id  EVSPId,
                                     AuthorizatorId        AuthorizatorId = null)
        {
            this._EVSPId                = EVSPId;
            this._AuthorizatorId        = (AuthorizatorId == null) ? AuthorizatorId.Parse("Belectric Drive EV Gateway Database") : AuthorizatorId;
            this.AuthorizationDatabase  = new Dictionary<Token,     AuthorizationResult>();
            this.SessionDatabase        = new Dictionary<ChargingSessionId, SessionInfo>();
        }

        #endregion


        #region AddToken(Token, AuthenticationResult = AuthenticationResult.Allowed)

        public Boolean AddToken(Token                Token,
                                AuthorizationResult  AuthenticationResult = AuthorizationResult.Authorized)
        {

            if (!AuthorizationDatabase.ContainsKey(Token))
            {
                AuthorizationDatabase.Add(Token, AuthenticationResult);
                return true;
            }

            return false;

        }

        #endregion

        #region RemoveToken(Token)

        public Boolean RemoveToken(Token Token)
        {
            return AuthorizationDatabase.Remove(Token);
        }

        #endregion


        #region AuthorizeStart(OperatorId, EVSEId, PartnerSessionId, Token)

        public AUTHSTARTResult AuthorizeStart(EVSEOperator_Id  OperatorId,
                                              EVSE_Id          EVSEId,
                                              ChargingSessionId        PartnerSessionId,
                                              Token            Token)

        {

            lock (AuthorizationDatabase)
            {

                AuthorizationResult AuthenticationResult;

                if (AuthorizationDatabase.TryGetValue(Token, out AuthenticationResult))
                {

                    #region Authorized

                    if (AuthenticationResult == AuthorizationResult.Authorized)
                    {

                        var _SessionId = ChargingSessionId.New;

                        SessionDatabase.Add(_SessionId, new SessionInfo(Token));

                        return new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       SessionId            = _SessionId,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId
                                   };

                    }

                    #endregion

                    #region Token is blocked!

                    else if (AuthenticationResult == AuthorizationResult.Blocked)
                        return new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                       Description          = "Token is blocked!"
                                   };

                    #endregion

                    #region ...fall through!

                    else
                        return new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                   };

                    #endregion

                }

                #region Unkown Token!

                else
                    return new AUTHSTARTResult(AuthorizatorId) {
                                   AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                   PartnerSessionId     = PartnerSessionId,
                                   ProviderId           = EVSPId,
                                   Description          = "Unkown token!"
                               };

                #endregion

            }

        }

        #endregion

        #region AuthorizeStop(OperatorId, EVSEId, SessionId, PartnerSessionId, Token)

        public AUTHSTOPResult AuthorizeStop(EVSEOperator_Id  OperatorId,
                                            EVSE_Id          EVSEId,
                                            ChargingSessionId        SessionId,
                                            ChargingSessionId        PartnerSessionId,
                                            Token            Token)

        {

            lock (AuthorizationDatabase)
            {

                AuthorizationResult AuthenticationResult;

                if (AuthorizationDatabase.TryGetValue(Token, out AuthenticationResult))
                {

                    if (AuthenticationResult == AuthorizationResult.Authorized)
                    {

                        SessionInfo SessionInfo = null;

                        if (SessionDatabase.TryGetValue(SessionId, out SessionInfo))
                        {

                            #region Authorized

                            if (Token == SessionInfo.Token)
                                return new AUTHSTOPResult(AuthorizatorId) {
                                           AuthorizationResult  = AuthenticationResult,
                                           SessionId            = SessionId,
                                           PartnerSessionId     = PartnerSessionId,
                                           ProviderId           = EVSPId
                                       };

                            #endregion

                            #region Invalid Token for SessionId!

                            else
                            {
                                return new AUTHSTOPResult(AuthorizatorId) {
                                               AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                               PartnerSessionId     = PartnerSessionId,
                                               ProviderId           = EVSPId,
                                               Description          = "Invalid token for given session identification!"
                                           };
                            }

                            #endregion

                        }

                        #region Invalid SessionId!

                        else
                        {
                            return new AUTHSTOPResult(AuthorizatorId) {
                                           AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                           PartnerSessionId     = PartnerSessionId,
                                           ProviderId           = EVSPId,
                                           Description          = "Invalid session identification!"
                                       };
                        }

                        #endregion

                    }

                    #region Blocked

                    else if (AuthenticationResult == AuthorizationResult.Blocked)
                        return new AUTHSTOPResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                       Description          = "Token is blocked!"
                                   };

                    #endregion

                    #region ...fall through!

                    else
                        return new AUTHSTOPResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                   };

                    #endregion

                }

                #region Unkown Token!

                else
                    return new AUTHSTOPResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                       Description          = "Unkown token!"
                                   };

                #endregion

            }

        }

        #endregion

        #region SendCDR(EVSEId, SessionId, PartnerSessionId, PartnerProductId, Token, eMAId, ChargeStart, ChargeEnd, SessionStart = null, SessionEnd = null, MeterValueStart = null, MeterValueEnd = null)

        public SENDCDRResult SendCDR(EVSE_Id    EVSEId,
                                     ChargingSessionId  SessionId,
                                     ChargingSessionId  PartnerSessionId,
                                     String     PartnerProductId,
                                     Token      Token,
                                     eMA_Id     eMAId,
                                     DateTime   ChargeStart,
                                     DateTime   ChargeEnd,
                                     DateTime?  SessionStart    = null,
                                     DateTime?  SessionEnd      = null,
                                     Double?    MeterValueStart = null,
                                     Double?    MeterValueEnd   = null)
        {

            lock (AuthorizationDatabase)
            {

                SessionInfo SessionInfo  = null;

                if (SessionDatabase.TryGetValue(SessionId, out SessionInfo))
                {

                    #region Success

                    if (Token == SessionInfo.Token)
                    {

                        SessionDatabase.Remove(SessionId);

                        return new SENDCDRResult(AuthorizatorId) {
                            State             = true,
                            PartnerSessionId  = PartnerSessionId
                        };

                    }

                    #endregion

                    #region Invalid Token for SessionId!

                    else
                        return new SENDCDRResult(AuthorizatorId) {
                                       State             = false,
                                       PartnerSessionId  = PartnerSessionId,
                                       Description       = "Invalid token for given session identification!"
                                   };

                    #endregion

                }

                #region Invalid SessionId!

                else
                    return new SENDCDRResult(AuthorizatorId) {
                                   State             = false,
                                   PartnerSessionId  = PartnerSessionId,
                                   Description       = "Invalid session identification!"
                               };

                #endregion

            }

        }

        #endregion


    }

}
