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
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.LocalService
{

    /// <summary>
    /// The EV Roaming Provider provided EVSE Operator services interface.
    /// </summary>
    public interface IAuthServices
    {

        Authorizator_Id AuthorizatorId { get; }

        IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AllTokens            { get; }
        IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AuthorizedTokens     { get; }
        IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> NotAuthorizedTokens  { get; }
        IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> BlockedTokens        { get; }


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
        Task<HTTPResponse<AUTHSTARTResult>> AuthorizeStart(EVSEOperator_Id      OperatorId,
                                                           Auth_Token           AuthToken,
                                                           EVSE_Id              EVSEId            = null,
                                                           ChargingSession_Id   SessionId         = null,
                                                           ChargingProduct_Id   PartnerProductId  = null,
                                                           ChargingSession_Id   PartnerSessionId  = null,
                                                           TimeSpan?            QueryTimeout      = null);


        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<HTTPResponse<AUTHSTOPResult>>  AuthorizeStop (EVSEOperator_Id      OperatorId,
                                                           ChargingSession_Id   SessionId,
                                                           Auth_Token           AuthToken,
                                                           EVSE_Id              EVSEId            = null,
                                                           ChargingSession_Id   PartnerSessionId  = null,
                                                           TimeSpan?            QueryTimeout      = null);


        /// <summary>
        /// Create a SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId"></param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthInfo">AuthInfo</param>.
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
        Task<HTTPResponse<SENDCDRResult>> SendChargeDetailRecord(EVSE_Id              EVSEId,
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
                                                                 TimeSpan?            QueryTimeout          = null);

    }

}
