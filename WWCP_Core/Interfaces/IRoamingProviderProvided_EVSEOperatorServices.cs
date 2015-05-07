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
using System.Collections.Generic;
using System.Text;

#endregion

namespace org.GraphDefined.WWCP.LocalService
{

    /// <summary>
    /// The EV Roaming Provider provided EVSE Operator services interface.
    /// </summary>
    public interface IRoamingProviderProvided_EVSEOperatorServices
    {

        Authorizator_Id AuthorizatorId { get; }

        IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AllTokens            { get; }
        IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AuthorizedTokens     { get; }
        IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> NotAuthorizedTokens  { get; }
        IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> BlockedTokens        { get; }


        AUTHSTARTResult AuthorizeStart(EVSEOperator_Id      OperatorId,
                                       EVSE_Id              EVSEId,
                                       ChargingSession_Id   PartnerSessionId,
                                       Auth_Token                UID);

        AUTHSTOPResult  AuthorizeStop (EVSEOperator_Id      OperatorId,
                                       EVSE_Id              EVSEId,
                                       ChargingSession_Id   SessionId,
                                       ChargingSession_Id   PartnerSessionId,
                                       Auth_Token                UID);

        SENDCDRResult   SendCDR       (EVSE_Id              EVSEId,
                                       ChargingSession_Id   SessionId,
                                       ChargingSession_Id   PartnerSessionId,
                                       String               PartnerProductId,
                                       DateTime             ChargeStart,
                                       DateTime             ChargeEnd,
                                       Auth_Token                UID             = null,
                                       eMA_Id               eMAId           = null,
                                       DateTime?            SessionStart    = null,
                                       DateTime?            SessionEnd      = null,
                                       Double?              MeterValueStart = null,
                                       Double?              MeterValueEnd   = null);

    }

}
