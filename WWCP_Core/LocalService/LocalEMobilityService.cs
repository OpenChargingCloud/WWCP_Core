/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
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

namespace org.GraphDefined.WWCP.LocalService
{

    /// <summary>
    /// A local E-Mobility service implementation.
    /// </summary>
    public class LocalEMobilityService : IAuthServices
    {

        #region Data

        private readonly Dictionary<Auth_Token,         AuthorizationResult>  AuthorizationDatabase;
        private readonly Dictionary<ChargingSession_Id, SessionInfo>          SessionDatabase;

        #endregion

        #region Properties

        #region EVSPId

        private readonly EVSP_Id _EVSPId;

        public EVSP_Id EVSPId
        {
            get
            {
                return _EVSPId;
            }
        }

        #endregion

        #region AuthorizatorId

        private readonly Authorizator_Id _AuthorizatorId;

        public Authorizator_Id AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public LocalEMobilityService(EVSP_Id          EVSPId,
                                     Authorizator_Id  AuthorizatorId = null)
        {
            this._EVSPId                = EVSPId;
            this._AuthorizatorId        = (AuthorizatorId == null) ? Authorizator_Id.Parse("eMI3 Local E-Mobility Database") : AuthorizatorId;
            this.AuthorizationDatabase  = new Dictionary<Auth_Token,     AuthorizationResult>();
            this.SessionDatabase        = new Dictionary<ChargingSession_Id, SessionInfo>();
        }

        #endregion


        #region AddToken(Token, AuthenticationResult = AuthenticationResult.Allowed)

        public Boolean AddToken(Auth_Token                Token,
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

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AllTokens
        {
            get
            {
                return AuthorizationDatabase;
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AuthorizedTokens
        {
            get
            {
                return AuthorizationDatabase.Where(v => v.Value == AuthorizationResult.Authorized);
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> NotAuthorizedTokens
        {
            get
            {
                return AuthorizationDatabase.Where(v => v.Value == AuthorizationResult.NotAuthorized);
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> BlockedTokens
        {
            get
            {
                return AuthorizationDatabase.Where(v => v.Value == AuthorizationResult.Blocked);
            }
        }

        #region RemoveToken(Token)

        public Boolean RemoveToken(Auth_Token Token)
        {
            return AuthorizationDatabase.Remove(Token);
        }

        #endregion


        #region AuthorizeStart(OperatorId, EVSEId, PartnerSessionId, Token)

        public AUTHSTARTResult AuthorizeStart(EVSEOperator_Id     OperatorId,
                                              EVSE_Id             EVSEId,
                                              ChargingSession_Id  PartnerSessionId,
                                              Auth_Token               Token)

        {

            lock (AuthorizationDatabase)
            {

                AuthorizationResult AuthenticationResult;

                if (AuthorizationDatabase.TryGetValue(Token, out AuthenticationResult))
                {

                    #region Authorized

                    if (AuthenticationResult == AuthorizationResult.Authorized)
                    {

                        var _SessionId = ChargingSession_Id.New;

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
                                            ChargingSession_Id        SessionId,
                                            ChargingSession_Id        PartnerSessionId,
                                            Auth_Token            Token)

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

        #region SendCDR(EVSEId, SessionId, PartnerSessionId, PartnerProductId, ChargeStart, ChargeEnd, Token = null, eMAId = null, SessionStart = null, SessionEnd = null, MeterValueStart = null, MeterValueEnd = null)

        public SENDCDRResult SendCDR(EVSE_Id             EVSEId,
                                     ChargingSession_Id  SessionId,
                                     ChargingSession_Id  PartnerSessionId,
                                     String              PartnerProductId,
                                     DateTime            ChargeStart,
                                     DateTime            ChargeEnd,
                                     Auth_Token               Token           = null,
                                     eMA_Id              eMAId           = null,
                                     DateTime?           SessionStart    = null,
                                     DateTime?           SessionEnd      = null,
                                     Double?             MeterValueStart = null,
                                     Double?             MeterValueEnd   = null)

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
                            State             = SENDCDRState.Forwarded,
                            PartnerSessionId  = PartnerSessionId
                        };

                    }

                    #endregion

                    #region Invalid Token for SessionId!

                    else
                        return new SENDCDRResult(AuthorizatorId) {
                                       State             = SENDCDRState.False,
                                       PartnerSessionId  = PartnerSessionId,
                                       Description       = "Invalid token for given session identification!"
                                   };

                    #endregion

                }

                #region Invalid SessionId!

                else
                    return new SENDCDRResult(AuthorizatorId) {
                                   State             = SENDCDRState.False,
                                   PartnerSessionId  = PartnerSessionId,
                                   Description       = "Invalid session identification!"
                               };

                #endregion

            }

        }

        #endregion


    }

}
