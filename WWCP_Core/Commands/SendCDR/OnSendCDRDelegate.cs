/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Create a SendChargeDetailRecord request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
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
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<SendCDRResult> OnChargeDetailRecordDelegate(DateTime            Timestamp,
                                                                     CancellationToken   CancellationToken,
                                                                     EventTracking_Id    EventTrackingId,
                                                                     ChargeDetailRecord  ChargeDetailRecord,
                                                                     //EVSE_Id              EVSEId,
                                                                     //ChargingSession_Id   SessionId,
                                                                     //ChargingProduct_Id   PartnerProductId,
                                                                     //DateTime             SessionStart,
                                                                     //DateTime             SessionEnd,
                                                                     //AuthInfo             AuthInfo,
                                                                     //ChargingSession_Id   PartnerSessionId      = null,
                                                                     //DateTime?            ChargingStart         = null,
                                                                     //DateTime?            ChargingEnd           = null,
                                                                     //Double?              MeterValueStart       = null,
                                                                     //Double?              MeterValueEnd         = null,
                                                                     //IEnumerable<Double>  MeterValuesInBetween  = null,
                                                                     //Double?              ConsumedEnergy        = null,
                                                                     //String               MeteringSignature     = null,
                                                                     //HubOperator_Id       HubOperatorId         = null,
                                                                     //EVSP_Id              HubProviderId         = null,
                                                                     TimeSpan?           QueryTimeout  = null);


    /// <summary>
    /// An event fired whenever a charge detail record will be send.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargeDetailRecord">A charge detail record.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnChargeDetailRecordSendDelegate(DateTime            Timestamp,
                                                          Object              Sender,
                                                          EventTracking_Id    EventTrackingId,
                                                          RoamingNetwork_Id   RoamingNetworkId,
                                                          ChargeDetailRecord  ChargeDetailRecord,
                                                          TimeSpan?           QueryTimeout);


    /// <summary>
    /// An event fired whenever a charge detail record had been sent.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargeDetailRecord">A charge detail record.</param>
    /// <param name="Result">The authorize stop result.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnChargeDetailRecordSentDelegate(DateTime            Timestamp,
                                                          Object              Sender,
                                                          EventTracking_Id    EventTrackingId,
                                                          RoamingNetwork_Id   RoamingNetworkId,
                                                          ChargeDetailRecord  ChargeDetailRecord,
                                                          TimeSpan?           QueryTimeout,
                                                          SendCDRResult       Result);

}


