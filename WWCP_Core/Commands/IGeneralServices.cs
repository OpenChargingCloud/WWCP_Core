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

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The EV Roaming Provider provided EVSE Operator services interface.
    /// </summary>
    public interface IGeneralServices : IPushDataAndStatus
    {

        #region Events

        #endregion


        #region AuthorizeStart

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<AuthStartResult> AuthorizeStart(DateTime             Timestamp,
                                             CancellationToken    CancellationToken,
                                             EventTracking_Id     EventTrackingId,
                                             EVSEOperator_Id      OperatorId,
                                             Auth_Token           AuthToken,
                                             ChargingProduct_Id   ChargingProductId  = null,
                                             ChargingSession_Id   SessionId          = null,
                                             TimeSpan?            QueryTimeout       = null);

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<AuthStartEVSEResult> AuthorizeStart(DateTime             Timestamp,
                                                 CancellationToken    CancellationToken,
                                                 EventTracking_Id     EventTrackingId,
                                                 EVSEOperator_Id      OperatorId,
                                                 Auth_Token           AuthToken,
                                                 EVSE_Id              EVSEId,
                                                 ChargingProduct_Id   ChargingProductId  = null,
                                                 ChargingSession_Id   SessionId          = null,
                                                 TimeSpan?            QueryTimeout       = null);

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<AuthStartChargingStationResult> AuthorizeStart(DateTime             Timestamp,
                                                            CancellationToken    CancellationToken,
                                                            EventTracking_Id     EventTrackingId,
                                                            EVSEOperator_Id      OperatorId,
                                                            Auth_Token           AuthToken,
                                                            ChargingStation_Id   ChargingStationId,
                                                            ChargingProduct_Id   ChargingProductId  = null,
                                                            ChargingSession_Id   SessionId          = null,
                                                            TimeSpan?            QueryTimeout       = null);

        #endregion

        #region AuthorizeStop

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<AuthStopResult> AuthorizeStop(DateTime            Timestamp,
                                           CancellationToken   CancellationToken,
                                           EventTracking_Id    EventTrackingId,
                                           EVSEOperator_Id     OperatorId,
                                           ChargingSession_Id  SessionId,
                                           Auth_Token          AuthToken,
                                           TimeSpan?           QueryTimeout  = null);

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<AuthStopEVSEResult> AuthorizeStop(DateTime            Timestamp,
                                               CancellationToken   CancellationToken,
                                               EventTracking_Id    EventTrackingId,
                                               EVSEOperator_Id     OperatorId,
                                               EVSE_Id             EVSEId,
                                               ChargingSession_Id  SessionId,
                                               Auth_Token          AuthToken,
                                               TimeSpan?           QueryTimeout  = null);

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="ChargingStationId">A charging station identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<AuthStopChargingStationResult> AuthorizeStop(DateTime            Timestamp,
                                                          CancellationToken   CancellationToken,
                                                          EventTracking_Id    EventTrackingId,
                                                          EVSEOperator_Id     OperatorId,
                                                          ChargingStation_Id  ChargingStationId,
                                                          ChargingSession_Id  SessionId,
                                                          Auth_Token          AuthToken,
                                                          TimeSpan?           QueryTimeout  = null);

        #endregion

        #region SendChargeDetailRecord

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<SendCDRResult> SendChargeDetailRecord(DateTime            Timestamp,
                                                   CancellationToken   CancellationToken,
                                                   EventTracking_Id    EventTrackingId,
                                                   ChargeDetailRecord  ChargeDetailRecord,
                                                   TimeSpan?           QueryTimeout = null);


        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId">The ev charging product identification.</param>
        /// <param name="SessionStart">The session start timestamp.</param>
        /// <param name="SessionEnd">The session end timestamp.</param>
        /// <param name="Identification">An identification.</param>
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        Task<SendCDRResult> SendChargeDetailRecord(DateTime             Timestamp,
                                                   CancellationToken    CancellationToken,
                                                   EventTracking_Id     EventTrackingId,
                                                   EVSE_Id              EVSEId,
                                                   ChargingSession_Id   SessionId,
                                                   ChargingProduct_Id   PartnerProductId,
                                                   DateTime             SessionStart,
                                                   DateTime             SessionEnd,
                                                   AuthInfo             Identification,
                                                   DateTime?            ChargingStart         = null,
                                                   DateTime?            ChargingEnd           = null,
                                                   Double?              MeterValueStart       = null,
                                                   Double?              MeterValueEnd         = null,
                                                   IEnumerable<Double>  MeterValuesInBetween  = null,
                                                   Double?              ConsumedEnergy        = null,
                                                   String               MeteringSignature     = null,
                                                   TimeSpan?            QueryTimeout          = null);

        #endregion

    }

}
