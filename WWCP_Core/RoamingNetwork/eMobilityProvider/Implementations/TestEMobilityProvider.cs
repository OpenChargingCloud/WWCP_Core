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

using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An E-Mobility Provider for (internal) tests.
    /// </summary>
    public class TestEMobilityProvider : //ToDo: IEMobilityProvider,
                                         IRemoteEMobilityProvider
    {


        #region Data

        private readonly ConcurrentDictionary<AuthenticationToken,   AuthStartResult>     authCache             = new();
        private readonly ConcurrentDictionary<ChargeDetailRecord_Id, ChargeDetailRecord>  chargeDetailRecords   = new();

        #endregion

        #region Properties

        public IId AuthId { get; }

        public IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> AllTokens           => throw new NotImplementedException();

        public IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> AuthorizedTokens    => throw new NotImplementedException();

        public IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> NotAuthorizedTokens => throw new NotImplementedException();

        public IEnumerable<KeyValuePair<LocalAuthentication, TokenAuthorizationResultType>> BlockedTokens       => throw new NotImplementedException();

        #endregion

        #region Events

        public event OnAuthorizeStartRequestDelegate?   OnAuthorizeStartRequest;
        public event OnAuthorizeStartResponseDelegate?  OnAuthorizeStartResponse;

        public event OnAuthorizeStopRequestDelegate?    OnAuthorizeStopRequest;
        public event OnAuthorizeStopResponseDelegate?   OnAuthorizeStopResponse;

        #endregion

        #region Constructor(s)


        public TestEMobilityProvider(EMobilityProvider_Id Id)
        {

            this.AuthId = Id;

        }

        #endregion


        // From the roaming network

        #region AddStaticData

        public Task<PushEVSEDataResult> AddStaticData(EVSE EVSE, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> AddStaticData(IEnumerable<EVSE> EVSEs, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> AddStaticData(ChargingStation ChargingStation, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> AddStaticData(IEnumerable<ChargingStation> ChargingStations, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> AddStaticData(ChargingPool ChargingPool, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> AddStaticData(IEnumerable<ChargingPool> ChargingPools, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> AddStaticData(ChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> AddStaticData(IEnumerable<ChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> AddStaticData(RoamingNetwork RoamingNetwork, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region SetStaticData

        public Task<PushEVSEDataResult> SetStaticData(EVSE EVSE, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> SetStaticData(IEnumerable<EVSE> EVSEs, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> SetStaticData(ChargingStation ChargingStation, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> SetStaticData(IEnumerable<ChargingStation> ChargingStations, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> SetStaticData(ChargingPool ChargingPool, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> SetStaticData(IEnumerable<ChargingPool> ChargingPools, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> SetStaticData(ChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> SetStaticData(IEnumerable<ChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> SetStaticData(RoamingNetwork RoamingNetwork, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UpdateStaticData

        public Task<PushEVSEDataResult> UpdateStaticData(EVSE EVSE, String? PropertyName = null, Object? OldValue = null, Object? NewValue = null, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> UpdateStaticData(IEnumerable<EVSE> EVSEs, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> UpdateStaticData(ChargingStation ChargingStation, String? PropertyName = null, Object? OldValue = null, Object? NewValue = null, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> UpdateStaticData(IEnumerable<ChargingStation> ChargingStations, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> UpdateStaticData(ChargingPool ChargingPool, String? PropertyName = null, Object? OldValue = null, Object? NewValue = null, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> UpdateStaticData(IEnumerable<ChargingPool> ChargingPools, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> UpdateStaticData(ChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> UpdateStaticData(IEnumerable<ChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> UpdateStaticData(RoamingNetwork RoamingNetwork, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DeleteStaticData

        public Task<PushEVSEDataResult> DeleteStaticData(EVSE EVSE, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> DeleteStaticData(IEnumerable<EVSE> EVSEs, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> DeleteStaticData(ChargingStation ChargingStation, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> DeleteStaticData(IEnumerable<ChargingStation> ChargingStations, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> DeleteStaticData(ChargingPool ChargingPool, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> DeleteStaticData(IEnumerable<ChargingPool> ChargingPools, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> DeleteStaticData(ChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> DeleteStaticData(IEnumerable<ChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> DeleteStaticData(RoamingNetwork RoamingNetwork, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region UpdateStatus

        public Task<PushEVSEStatusResult> UpdateStatus(IEnumerable<EVSEStatusUpdate> StatusUpdates, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationStatusResult> UpdateStatus(IEnumerable<ChargingStationStatusUpdate> StatusUpdates, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingPoolStatusResult> UpdateStatus(IEnumerable<ChargingPoolStatusUpdate> StatusUpdates, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationOperatorStatusResult> UpdateStatus(IEnumerable<ChargingStationOperatorStatusUpdate> StatusUpdates, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushRoamingNetworkStatusResult> UpdateStatus(IEnumerable<RoamingNetworkStatusUpdate> StatusUpdates, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UpdateAdminStatus

        public Task<PushEVSEAdminStatusResult> UpdateAdminStatus(IEnumerable<EVSEAdminStatusUpdate> AdminStatusUpdates, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationAdminStatusResult> UpdateAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate> AdminStatusUpdates, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingPoolAdminStatusResult> UpdateAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate> AdminStatusUpdates, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationOperatorAdminStatusResult> UpdateAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate> AdminStatusUpdates, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushRoamingNetworkAdminStatusResult> UpdateAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate> AdminStatusUpdates, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region AuthorizeStart(LocalAuthentication, ...)

        public async Task<AuthStartResult> AuthorizeStart(LocalAuthentication          LocalAuthentication,
                                                          ChargingLocation?            ChargingLocation      = null,
                                                          ChargingProduct?             ChargingProduct       = null,
                                                          ChargingSession_Id?          SessionId             = null,
                                                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                                                          ChargingStationOperator_Id?  OperatorId            = null,

                                                          DateTime?                    Timestamp             = null,
                                                          CancellationToken            CancellationToken     = default,
                                                          EventTracking_Id?            EventTrackingId       = null,
                                                          TimeSpan?                    RequestTimeout        = null)
        {

            if (LocalAuthentication.AuthToken.HasValue &&
                authCache.TryGetValue(LocalAuthentication.AuthToken.Value, out var authStartResult))
            {
                return authStartResult;
            }

            return AuthStartResult.NotAuthorized(AuthorizatorId:            this.AuthId,
                                                 ISendAuthorizeStartStop:   null,
                                                 SessionId:                 null,

                                                 ProviderId:                null,
                                                 Description:               null,
                                                 AdditionalInfo:            null,
                                                 NumberOfRetries:           0,
                                                 Runtime:                   null);

        }

        #endregion

        #region AuthorizeStop (SessionId, LocalAuthentication, ...)

        public async Task<AuthStopResult> AuthorizeStop(ChargingSession_Id           SessionId,
                                                        LocalAuthentication          LocalAuthentication,
                                                        ChargingLocation?            ChargingLocation      = null,
                                                        ChargingSession_Id?          CPOPartnerSessionId   = null,
                                                        ChargingStationOperator_Id?  OperatorId            = null,

                                                        DateTime?                    Timestamp             = null,
                                                        CancellationToken            CancellationToken     = default,
                                                        EventTracking_Id?            EventTrackingId       = null,
                                                        TimeSpan?                    RequestTimeout        = null)
        {

            if (LocalAuthentication.AuthToken.HasValue &&
                authCache.TryGetValue(LocalAuthentication.AuthToken.Value, out var authStartResult))
            {

                if (authStartResult.Result == AuthStartResultTypes.Authorized)
                    return AuthStopResult.Authorized(authStartResult.AuthorizatorId,
                                                     authStartResult.ISendAuthorizeStartStop,
                                                     authStartResult.SessionId,
                                                     authStartResult.ProviderId,
                                                     authStartResult.Description,
                                                     authStartResult.AdditionalInfo,
                                                     authStartResult.NumberOfRetries,
                                                     authStartResult.Runtime);

                if (authStartResult.Result == AuthStartResultTypes.NotAuthorized)
                    return AuthStopResult.NotAuthorized(authStartResult.AuthorizatorId,
                                                        authStartResult.ISendAuthorizeStartStop,
                                                        authStartResult.SessionId,
                                                        authStartResult.ProviderId,
                                                        authStartResult.Description,
                                                        authStartResult.AdditionalInfo,
                                                        authStartResult.NumberOfRetries,
                                                        authStartResult.Runtime);

            }

            return AuthStopResult.NotAuthorized(AuthorizatorId:            this.AuthId,
                                                ISendAuthorizeStartStop:   null,
                                                SessionId:                 null,

                                                ProviderId:                null,
                                                Description:               null,
                                                AdditionalInfo:            null,
                                                NumberOfRetries:           0,
                                                Runtime:                   null);

        }

        #endregion

        #region ReceiveChargeDetailRecords(ChargeDetailRecords)

        public async Task<SendCDRsResult> ReceiveChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,

                                                                     DateTime?                        Timestamp           = null,
                                                                     CancellationToken                CancellationToken   = default,
                                                                     EventTracking_Id?                EventTrackingId     = null,
                                                                     TimeSpan?                        RequestTimeout      = null)
        {

            foreach (var chargeDetailRecord in ChargeDetailRecords)
                chargeDetailRecords.TryAdd(chargeDetailRecord.Id,
                                           chargeDetailRecord);

            return SendCDRsResult.Success(
                       org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                       AuthId,
                       this,
                       ChargeDetailRecords,
                       I18NString.Create(Languages.en, "All fine. Thank you!"),
                       Array.Empty<Warning>(),
                       TimeSpan.Zero
                   );

        }

        #endregion



        // Management methods

        #region AddAuth(Token, Result)

        public void AddAuth(AuthenticationToken  Token,
                            AuthStartResult      Result)
        {

            this.authCache.TryAdd(Token, Result);

        }

        #endregion

        #region RemoveAuth(Token)

        public void RemoveAuth(AuthenticationToken Token)
        {

            this.authCache.Remove(Token, out _);

        }

        #endregion


    }

}
