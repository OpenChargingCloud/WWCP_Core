/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The Roaming Provider provided eMobility services interface.
    /// </summary>
    public interface ICSORoamingProvider : IPullEVSEData,
                                           IPullEVSEStatus,
                                           IRemoteStartStop,
                                           IChargingReservations,
                                           IChargingSessions,
                                           IChargeDetailRecords
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming provider.
        /// </summary>
        CSORoamingProvider_Id  Id                { get; }

        /// <summary>
        /// The offical (multi-language) name of the roaming provider.
        /// </summary>
        I18NString             Name              { get; }

        /// <summary>
        /// The attached roaming network.
        /// </summary>
        IRoamingNetwork?       RoamingNetwork    { get; }

        #endregion

        #region Events

        // Client methods (logging)

        #region OnGetChargeDetailRecordsRequest/-Response

        /// <summary>
        /// An event sent whenever a 'get charge detail records' request will be send.
        /// </summary>
        event OnGetCDRsRequestDelegate    OnGetChargeDetailRecordsRequest;

        /// <summary>
        /// An event sent whenever a response to a 'get charge detail records' request was received.
        /// </summary>
        event OnGetCDRsResponseDelegate   OnGetChargeDetailRecordsResponse;

        #endregion


        // Server methods

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize EVSE start' request was received.
        /// </summary>
        event OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize EVSE start' request was sent.
        /// </summary>
        event OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize EVSE stop' request was received.
        /// </summary>
        event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize EVSE stop' request was sent.
        /// </summary>
        event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion

        #region OnChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event sent whenever a 'charge detail record' was received.
        /// </summary>
        event OnSendCDRsRequestDelegate   OnChargeDetailRecordRequest;

        /// <summary>
        /// An event sent whenever a response to a 'charge detail record' was sent.
        /// </summary>
        event OnSendCDRsResponseDelegate  OnChargeDetailRecordResponse;

        #endregion

        #endregion


        #region GetChargeDetailRecords(From, To = null, ProviderId = null, ...)

        /// <summary>
        /// Download all charge detail records from the OICP server.
        /// </summary>
        /// <param name="From">The starting time.</param>
        /// <param name="To">An optional end time. [default: current time].</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<IEnumerable<ChargeDetailRecord>>

            GetChargeDetailRecords(DateTime               From,
                                   DateTime?              To                  = null,
                                   EMobilityProvider_Id?  ProviderId          = null,

                                   DateTime?              Timestamp           = null,
                                   EventTracking_Id?      EventTrackingId     = null,
                                   TimeSpan?              RequestTimeout      = null,
                                   CancellationToken      CancellationToken   = default);

        #endregion

    }

}
