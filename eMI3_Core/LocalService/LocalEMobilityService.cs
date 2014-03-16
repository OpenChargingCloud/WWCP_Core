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
    /// A local E-Mobility service implementation.
    /// </summary>
    public class LocalEMobilityService : IEMobilityService
    {

        #region Data

        private readonly Dictionary<String, AuthorizationResult>  AuthorizationDatabase;
        private readonly Dictionary<Guid,   SessionInfo>          SessionDatabase;

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

        public LocalEMobilityService(EVServiceProvider_Id  EVSPId,
                                     String                AuthorizatorId = "Belectric Drive EV Gateway Database")
        {
            this._EVSPId                = EVSPId;
            this._AuthorizatorId        = AuthorizatorId;
            this.AuthorizationDatabase  = new Dictionary<String, AuthorizationResult>();
            this.SessionDatabase        = new Dictionary<Guid,   SessionInfo>();
        }

        #endregion


        #region AddUID(UID, AuthenticationResult = AuthenticationResult.Allowed)

        public Boolean AddUID(String                UID,
                              AuthorizationResult  AuthenticationResult = AuthorizationResult.Authorized)
        {

            if (!AuthorizationDatabase.ContainsKey(UID))
            {
                AuthorizationDatabase.Add(UID, AuthenticationResult);
                return true;
            }

            return false;

        }

        #endregion

        #region RemoveUID(UID)

        public Boolean RemoveUID(String UID)
        {
            return AuthorizationDatabase.Remove(UID);
        }

        #endregion


        #region AuthorizeStart(OperatorId, EVSEId, PartnerSessionId, UID)

        public AUTHSTARTResult AuthorizeStart(EVSEOperator_Id  OperatorId,
                                              EVSE_Id          EVSEId,
                                              String           PartnerSessionId,
                                              String           UID)

        {

            lock (AuthorizationDatabase)
            {

                AuthorizationResult AuthenticationResult;

                if (AuthorizationDatabase.TryGetValue(UID, out AuthenticationResult))
                {

                    #region Authorized

                    if (AuthenticationResult == AuthorizationResult.Authorized)
                    {

                        var SessionId = Guid.NewGuid();

                        SessionDatabase.Add(SessionId, new SessionInfo(UID));

                        return new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       SessionId            = SessionId.ToString(),
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId
                                   };

                    }

                    #endregion

                    #region UID is blocked!

                    else if (AuthenticationResult == AuthorizationResult.Blocked)
                        return new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthenticationResult,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                       Description          = "UID is blocked!"
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

                #region Unkown UID!

                else
                    return new AUTHSTARTResult(AuthorizatorId) {
                                   AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                   PartnerSessionId     = PartnerSessionId,
                                   ProviderId           = EVSPId,
                                   Description          = "Unkown UID!"
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

            lock (AuthorizationDatabase)
            {

                AuthorizationResult AuthenticationResult;

                if (AuthorizationDatabase.TryGetValue(UID, out AuthenticationResult))
                {

                    if (AuthenticationResult == AuthorizationResult.Authorized)
                    {

                        SessionInfo SessionInfo = null;

                        if (SessionDatabase.TryGetValue(new Guid(SessionId), out SessionInfo))
                        {

                            #region Authorized

                            if (UID == SessionInfo.UID)
                                return new AUTHSTOPResult(AuthorizatorId) {
                                           AuthorizationResult  = AuthenticationResult,
                                           SessionId            = SessionId,
                                           PartnerSessionId     = PartnerSessionId,
                                           ProviderId           = EVSPId
                                       };

                            #endregion

                            #region Invalid UID for SessionId!

                            else
                            {
                                return new AUTHSTOPResult(AuthorizatorId) {
                                               AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                               PartnerSessionId     = PartnerSessionId,
                                               ProviderId           = EVSPId,
                                               Description          = "Invalid UID for SessionId!"
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
                                           Description          = "Invalid SessionId!"
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
                                       Description          = "UID is blocked!"
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

                #region Unkown UID!

                else
                    return new AUTHSTOPResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSPId,
                                       Description          = "Unkown UID!"
                                   };

                #endregion

            }

        }

        #endregion

        #region SendCDR(EVSEId, SessionId, PartnerSessionId, PartnerProductId, UID, ChargeStart, ChargeEnd, SessionStart = null, SessionEnd = null, MeterValueStart = null, MeterValueEnd = null)

        public SENDCDRResult SendCDR(EVSE_Id    EVSEId,
                                     String     SessionId,
                                     String     PartnerSessionId,
                                     String     PartnerProductId,
                                     String     UID,
                                     DateTime   ChargeStart,
                                     DateTime   ChargeEnd,
                                     DateTime?  SessionStart    = null,
                                     DateTime?  SessionEnd      = null,
                                     Double?    MeterValueStart = null,
                                     Double?    MeterValueEnd   = null)
        {

            lock (AuthorizationDatabase)
            {

                var         _SessionId   = new Guid(SessionId); //ToDo: Might fail!
                SessionInfo SessionInfo  = null;

                if (SessionDatabase.TryGetValue(_SessionId, out SessionInfo))
                {

                    #region Success

                    if (UID == SessionInfo.UID)
                    {

                        SessionDatabase.Remove(_SessionId);

                        return new SENDCDRResult(AuthorizatorId) {
                            State             = true,
                            PartnerSessionId  = PartnerSessionId
                        };

                    }

                    #endregion

                    #region Invalid UID for SessionId!

                    else
                        return new SENDCDRResult(AuthorizatorId) {
                                       State             = false,
                                       PartnerSessionId  = PartnerSessionId,
                                       Description       = "Invalid UID for SessionId!"
                                   };

                    #endregion

                }

                #region Invalid SessionId!

                else
                    return new SENDCDRResult(AuthorizatorId) {
                                   State             = false,
                                   PartnerSessionId  = PartnerSessionId,
                                   Description       = "Invalid SessionId!"
                               };

                #endregion

            }

        }

        #endregion


    }

}
