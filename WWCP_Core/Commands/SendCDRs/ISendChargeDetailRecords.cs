/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    public enum ChargeDetailRecordFilters
    {
        drop,
        forward
    }

    /// <summary>
    /// The interface for sending charge detail records.
    /// </summary>
    public interface ISendChargeDetailRecords
    {

        IId               Id                               { get; }

        IEnumerable<IId>  Ids                              { get; }


        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        Boolean                           DisableSendChargeDetailRecords    { get; set; }


        /// <summary>
        /// A delegate for filtering charge detail records.
        /// </summary>
        ChargeDetailRecordFilterDelegate ChargeDetailRecordFilter          { get; }

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        event OnSendCDRsResponseDelegate  OnSendCDRsResponse;
        // Events?


        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the CDR directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTime?                        Timestamp           = null,
                                    CancellationToken?               CancellationToken   = null,
                                    EventTracking_Id                 EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null);

    }

}
