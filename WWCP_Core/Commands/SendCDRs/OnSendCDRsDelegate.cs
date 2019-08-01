/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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
using System.Collections.Generic;

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
    /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<SendCDRsResult> OnChargeDetailRecordDelegate(DateTime                         Timestamp,
                                                                      CancellationToken                CancellationToken,
                                                                      EventTracking_Id                 EventTrackingId,
                                                                      IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                                                      TimeSpan?                        RequestTimeout  = null);


    /// <summary>
    /// An event fired whenever a charge detail record will be send.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the logging.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The sender identification of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="DroppedChargeDetailRecords">An enumeration of dropped charge detail records.</param>
    /// <param name="ForwardedChargeDetailRecords">An enumeration of forwarded charge detail records.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task OnSendCDRRequestDelegate(DateTime                         LogTimestamp,
                                                  DateTime                         RequestTimestamp,
                                                  Object                           Sender,
                                                  String                           SenderId,
                                                  EventTracking_Id                 EventTrackingId,
                                                  RoamingNetwork_Id                RoamingNetworkId,
                                                  IEnumerable<ChargeDetailRecord>  DroppedChargeDetailRecords,
                                                  IEnumerable<ChargeDetailRecord>  ForwardedChargeDetailRecords,
                                                  TimeSpan?                        RequestTimeout);


    /// <summary>
    /// An event fired whenever a charge detail record had been sent.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the logging.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The sender identification of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="DroppedChargeDetailRecords">An enumeration of dropped charge detail records.</param>
    /// <param name="ForwardedChargeDetailRecords">An enumeration of forwarded charge detail records.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The authorize stop result.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnSendCDRResponseDelegate(DateTime                         LogTimestamp,
                                                   DateTime                         RequestTimestamp,
                                                   Object                           Sender,
                                                   String                           SenderId,
                                                   EventTracking_Id                 EventTrackingId,
                                                   RoamingNetwork_Id                RoamingNetworkId,
                                                   IEnumerable<ChargeDetailRecord>  DroppedChargeDetailRecords,
                                                   IEnumerable<ChargeDetailRecord>  ForwardedChargeDetailRecords,
                                                   TimeSpan?                        RequestTimeout,
                                                   SendCDRsResult                   Result,
                                                   TimeSpan                         Runtime);

}
