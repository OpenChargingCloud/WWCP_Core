/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

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


        #region AuthorizeStart(OperatorId, AuthToken, EVSEId = null, SessionId = null, PartnerProductId = null, PartnerSessionId = null, QueryTimeout = null)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<AUTHSTARTResult>> AuthorizeStart(EVSEOperator_Id     OperatorId,
                                                                        Auth_Token          AuthToken,
                                                                        EVSE_Id             EVSEId            = null,
                                                                        ChargingSession_Id  SessionId         = null,
                                                                        ChargingProduct_Id  PartnerProductId  = null,
                                                                        ChargingSession_Id  PartnerSessionId  = null,
                                                                        TimeSpan?           QueryTimeout      = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            lock (AuthorizationDatabase)
            {

                AuthorizationResult AuthenticationResult;

                if (AuthorizationDatabase.TryGetValue(AuthToken, out AuthenticationResult))
                {

                    #region Authorized

                    if (AuthenticationResult == AuthorizationResult.Authorized)
                    {

                        var _SessionId = ChargingSession_Id.New;

                        SessionDatabase.Add(_SessionId, new SessionInfo(AuthToken));

                        return new HTTPResponse<AUTHSTARTResult>(
                                   new HTTPResponse(),
                                   new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       SessionId            = _SessionId,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId
                                   });

                    }

                    #endregion

                    #region Token is blocked!

                    else if (AuthenticationResult == AuthorizationResult.Blocked)
                        return new HTTPResponse<AUTHSTARTResult>(
                                   new HTTPResponse(),
                                   new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                       Description          = "Token is blocked!"
                                   });

                    #endregion

                    #region ...fall through!

                    else
                        return new HTTPResponse<AUTHSTARTResult>(
                                   new HTTPResponse(),
                                   new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                   });

                    #endregion

                }

                #region Unkown Token!

                else
                    return new HTTPResponse<AUTHSTARTResult>(
                                   new HTTPResponse(),
                                   new AUTHSTARTResult(AuthorizatorId) {
                                   AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                   PartnerSessionId     = PartnerSessionId,
                                   ProviderId           = EVSPId,
                                   Description          = "Unkown token!"
                               });

                #endregion

            }

        }

        #endregion

        #region AuthorizeStop(OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null, QueryTimeout = null)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<AUTHSTOPResult>> AuthorizeStop(EVSEOperator_Id      OperatorId,
                                                                      ChargingSession_Id   SessionId,
                                                                      Auth_Token           AuthToken,
                                                                      EVSE_Id              EVSEId            = null,   // OICP v2.0: Optional
                                                                      ChargingSession_Id   PartnerSessionId  = null,   // OICP v2.0: Optional [50]
                                                                      TimeSpan?            QueryTimeout      = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException("SessionId",  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            lock (AuthorizationDatabase)
            {

                AuthorizationResult AuthenticationResult;

                if (AuthorizationDatabase.TryGetValue(AuthToken, out AuthenticationResult))
                {

                    if (AuthenticationResult == AuthorizationResult.Authorized)
                    {

                        SessionInfo SessionInfo = null;

                        if (SessionDatabase.TryGetValue(SessionId, out SessionInfo))
                        {

                            #region Authorized

                            if (AuthToken == SessionInfo.Token)
                                return new HTTPResponse<AUTHSTOPResult>(
                                           new HTTPResponse(),
                                           new AUTHSTOPResult(AuthorizatorId) {
                                               AuthorizationResult  = AuthenticationResult,
                                               SessionId            = SessionId,
                                               PartnerSessionId     = PartnerSessionId,
                                               ProviderId           = EVSPId
                                           });

                            #endregion

                            #region Invalid Token for SessionId!

                            else
                            {
                                return new HTTPResponse<AUTHSTOPResult>(
                                           new HTTPResponse(),
                                           new AUTHSTOPResult(AuthorizatorId) {
                                               AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                               PartnerSessionId     = PartnerSessionId,
                                               ProviderId           = EVSPId,
                                               Description          = "Invalid token for given session identification!"
                                           });
                            }

                            #endregion

                        }

                        #region Invalid SessionId!

                        else
                        {
                            return new HTTPResponse<AUTHSTOPResult>(
                                       new HTTPResponse(),
                                       new AUTHSTOPResult(AuthorizatorId) {
                                           AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                           PartnerSessionId     = PartnerSessionId,
                                           ProviderId           = EVSPId,
                                           Description          = "Invalid session identification!"
                                       });
                        }

                        #endregion

                    }

                    #region Blocked

                    else if (AuthenticationResult == AuthorizationResult.Blocked)
                        return new HTTPResponse<AUTHSTOPResult>(
                                   new HTTPResponse(),
                                   new AUTHSTOPResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                       Description          = "Token is blocked!"
                                   });

                    #endregion

                    #region ...fall through!

                    else
                        return new HTTPResponse<AUTHSTOPResult>(
                                   new HTTPResponse(),
                                   new AUTHSTOPResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                   });

                    #endregion

                }

                #region Unkown Token!

                else
                    return new HTTPResponse<AUTHSTOPResult>(
                               new HTTPResponse(),
                               new AUTHSTOPResult(AuthorizatorId) {
                                   AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                   PartnerSessionId     = PartnerSessionId,
                                   ProviderId           = EVSPId,
                                   Description          = "Unkown token!"
                               });

                #endregion

            }

        }

        #endregion

        #region SendChargeDetailRecord(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, AuthInfo, PartnerSessionId = null, ..., QueryTimeout = null)

        /// <summary>
        /// Create a SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId"></param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthInfo">An identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<SENDCDRResult>>

            SendChargeDetailRecord(EVSE_Id              EVSEId,
                                   ChargingSession_Id   SessionId,
                                   ChargingProduct_Id   PartnerProductId,
                                   DateTime             SessionStart,
                                   DateTime             SessionEnd,
                                   AuthInfo             AuthInfo,
                                   ChargingSession_Id   PartnerSessionId      = null,
                                   DateTime?            ChargingStart         = null,
                                   DateTime?            ChargingEnd           = null,
                                   Double?              MeterValueStart       = null,
                                   Double?              MeterValueEnd         = null,
                                   IEnumerable<Double>  MeterValuesInBetween  = null,
                                   Double?              ConsumedEnergy        = null,
                                   String               MeteringSignature     = null,
                                   HubOperator_Id       HubOperatorId         = null,
                                   EVSP_Id              HubProviderId         = null,
                                   TimeSpan?            QueryTimeout          = null)

        {

            #region Initial checks

            if (EVSEId           == null)
                throw new ArgumentNullException("EVSEId",            "The given parameter must not be null!");

            if (SessionId        == null)
                throw new ArgumentNullException("SessionId",         "The given parameter must not be null!");

            if (PartnerProductId == null)
                throw new ArgumentNullException("PartnerProductId",  "The given parameter must not be null!");

            if (SessionStart     == null)
                throw new ArgumentNullException("SessionStart",      "The given parameter must not be null!");

            if (SessionEnd       == null)
                throw new ArgumentNullException("SessionEnd",        "The given parameter must not be null!");

            if (AuthInfo         == null)
                throw new ArgumentNullException("AuthInfo",          "The given parameter must not be null!");

            #endregion

            lock (AuthorizationDatabase)
            {

                SessionInfo SessionInfo  = null;

                if (SessionDatabase.TryGetValue(SessionId, out SessionInfo))
                {

                    #region Success

                    if (AuthInfo.AuthToken == SessionInfo.Token)
                    {

                        SessionDatabase.Remove(SessionId);

                        return new HTTPResponse<SENDCDRResult>(new HTTPResponse(),
                                                               new SENDCDRResult(AuthorizatorId) {
                                                                   State             = SENDCDRState.Forwarded,
                                                                   PartnerSessionId  = PartnerSessionId
                                                               });

                    }

                    #endregion

                    #region Invalid Token for SessionId!

                    return new HTTPResponse<SENDCDRResult>(new HTTPResponse(),
                                                           new SENDCDRResult(AuthorizatorId) {
                                                               State             = SENDCDRState.False,
                                                               PartnerSessionId  = PartnerSessionId,
                                                               Description       = "Invalid token for given session identification!"
                                                           });

                    #endregion

                }

                #region Invalid SessionId!

                else
                    return new HTTPResponse<SENDCDRResult>(new HTTPResponse(),
                                                           new SENDCDRResult(AuthorizatorId) {
                                                               State             = SENDCDRState.False,
                                                               PartnerSessionId  = PartnerSessionId,
                                                               Description       = "Invalid session identification!"
                                                           });

                #endregion

            }

        }

        #endregion


    }

}
