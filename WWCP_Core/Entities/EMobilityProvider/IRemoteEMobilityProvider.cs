/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public interface ISend2RemoteEMobilityProvider : //IRemotePushData,
                                                     ISendAdminStatus,
                                                     ISendStatus,
                                                     ISendAuthorizeStartStop,
                                                     ISendChargeDetailRecords

    {

        ///// <summary>
        ///// The unique identification of the e-mobility service provider.
        ///// </summary>
        //EMobilityProvider_Id Id { get; }

    }


    public interface IRemoteEMobilityProvider : IReceiveRoamingNetworkData,
                                                IReceiveChargingStationOperatorData,
                                                IReceiveChargingPoolData,
                                                IReceiveChargingStationData,
                                                IReceiveEVSEData,

                                                IReceiveAdminStatus,
                                                IReceiveStatus,
                                                IReceiveEnergyStatus,

                                                IReceiveAuthorizeStartStop,
                                                IReceiveChargeDetailRecords

    {

        ///// <summary>
        ///// The unique identification of the e-mobility service provider.
        ///// </summary>
        //EMobilityProvider_Id  Id            { get; }

        //  Authorizator_Id AuthorizatorId { get; }


        IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> AllTokens            { get; }
        IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> AuthorizedTokens     { get; }
        IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> NotAuthorizedTokens  { get; }
        IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> BlockedTokens        { get; }


    }


    public interface IRemoteEMobilityProviderUI

    {

        TimeSpan?  RequestTimeout    { get; set; }


        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, RemoteAuthentication = null, SessionId = null, ...)

        /// <summary>
        /// Start a charging session at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional cancellation token to cancel this request.</param>
        Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        RemoteAuthentication     RemoteAuthentication,
                        ChargingProduct?         ChargingProduct          = null,
                        ChargingReservation_Id?  ReservationId            = null,
                        ChargingSession_Id?      SessionId                = null,
                        JObject?                 AdditionalSessionInfos   = null,
                        Auth_Path?               AuthenticationPath       = null,

                        DateTime?                RequestTimestamp         = null,
                        EventTracking_Id?        EventTrackingId          = null,
                        TimeSpan?                RequestTimeout           = null,
                        CancellationToken        CancellationToken        = default);

        #endregion

        #region RemoteStop (SessionId, ReservationHandling = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,
                       Auth_Path?             AuthenticationPath     = null,

                       DateTime?              Timestamp              = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null,
                       CancellationToken      CancellationToken      = default);

        #endregion

    }

}
