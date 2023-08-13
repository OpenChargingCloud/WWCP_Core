u/*
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

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public abstract class AWWCP__EMPAdapter : ACryptoEMobilityEntity<CSORoamingProvider_Id,
                                                                   CSORoamingProviderAdminStatusTypes,
                                                                   CSORoamingProviderStatusTypes>,

                                            IReceiveRoamingNetworkData,
                                            IReceiveChargingStationOperatorData,
                                            IReceiveChargingPoolData,
                                            IReceiveChargingStationData,
                                            IReceiveEVSEData,

                                            IReceiveAdminStatus,
                                            IReceiveStatus,
                                            IReceiveEnergyStatus
    {
        protected AWWCP__EMPAdapter(CSORoamingProvider_Id                             Id,
                                  IRoamingNetwork                                   RoamingNetwork,

                                  I18NString?                                       Name                         = null,
                                  I18NString?                                       Description                  = null,

                                  String?                                           EllipticCurve                = "P-256",
                                  ECPrivateKeyParameters?                           PrivateKey                   = null,
                                  PublicKeyCertificates?                            PublicKeyCertificates        = null,

                                  Timestamped<CSORoamingProviderAdminStatusTypes>?  InitialAdminStatus           = null,
                                  Timestamped<CSORoamingProviderStatusTypes>?       InitialStatus                = null,
                                  UInt16                                            MaxAdminStatusScheduleSize   = DefaultMaxAdminStatusScheduleSize,
                                  UInt16                                            MaxStatusScheduleSize        = DefaultMaxStatusScheduleSize,

                                  String?                                           DataSource                   = null,
                                  DateTime?                                         LastChange                   = null,

                                  JObject?                                          CustomData                   = null,
                                  UserDefinedDictionary?                            InternalData                 = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,
                   InitialAdminStatus,
                   InitialStatus,
                   MaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

        }


        #region IReceiveRoamingNetworkData

        public Task<AddOrUpdateRoamingNetworkResult> AddOrUpdateRoamingNetwork(IRoamingNetwork RoamingNetwork, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateRoamingNetworksResult> AddOrUpdateRoamingNetworks(IEnumerable<IRoamingNetwork> RoamingNetworks, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddRoamingNetworkResult> AddRoamingNetwork(IRoamingNetwork RoamingNetwork, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddRoamingNetworkResult> AddRoamingNetworkIfNotExists(IRoamingNetwork RoamingNetwork, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddRoamingNetworksResult> AddRoamingNetworks(IEnumerable<IRoamingNetwork> RoamingNetworks, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddRoamingNetworksResult> AddRoamingNetworksIfNotExist(IEnumerable<IRoamingNetwork> RoamingNetworks, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public Task<DeleteRoamingNetworkResult> DeleteRoamingNetwork(IRoamingNetwork RoamingNetwork, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteRoamingNetworksResult> DeleteRoamingNetworks(IEnumerable<IRoamingNetwork> RoamingNetworks, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateRoamingNetworkResult> UpdateRoamingNetwork(IRoamingNetwork RoamingNetwork, String PropertyName, Object? NewValue, Object? OldValue = null, Context? DataSource = null, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateRoamingNetworksResult> UpdateRoamingNetworks(IEnumerable<IRoamingNetwork> RoamingNetworks, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        // 

        #endregion

        #region IReceiveChargingStationOperatorData

        public Task<AddChargingStationOperatorResult> AddChargingStationOperator(IChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationOperatorResult> AddChargingStationOperatorIfNotExists(IChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationOperatorsResult> AddChargingStationOperators(IEnumerable<IChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationOperatorsResult> AddChargingStationOperatorsIfNotExist(IEnumerable<IChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public Task<AddOrUpdateChargingStationOperatorResult> AddOrUpdateChargingStationOperator(IChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingStationOperatorsResult> AddOrUpdateChargingStationOperators(IEnumerable<IChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationOperatorResult> UpdateChargingStationOperator(IChargingStationOperator ChargingStationOperator, String PropertyName, Object? NewValue, Object? OldValue = null, Context? DataSource = null, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationOperatorsResult> UpdateChargingStationOperators(IEnumerable<IChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingStationOperatorResult> DeleteChargingStationOperator(IChargingStationOperator ChargingStationOperator, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingStationOperatorsResult> DeleteChargingStationOperators(IEnumerable<IChargingStationOperator> ChargingStationOperators, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IReceiveChargingPoolData,

        public Task<AddChargingPoolResult> AddChargingPool(IChargingPool ChargingPool, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingPoolResult> AddChargingPoolIfNotExists(IChargingPool ChargingPool, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingPoolResult> AddOrUpdateChargingPool(IChargingPool ChargingPool, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingPoolResult> UpdateChargingPool(IChargingPool ChargingPool, String PropertyName, Object? NewValue, Object? OldValue = null, Context? DataSource = null, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingPoolResult> DeleteChargingPool(IChargingPool ChargingPool, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingPoolsResult> AddChargingPools(IEnumerable<IChargingPool> ChargingPools, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingPoolsResult> AddChargingPoolsIfNotExist(IEnumerable<IChargingPool> ChargingPools, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingPoolsResult> AddOrUpdateChargingPools(IEnumerable<IChargingPool> ChargingPools, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingPoolsResult> UpdateChargingPools(IEnumerable<IChargingPool> ChargingPools, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingPoolsResult> DeleteChargingPools(IEnumerable<IChargingPool> ChargingPools, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IReceiveChargingStationData,

        public Task<AddChargingStationResult> AddChargingStation(IChargingStation ChargingStation, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationResult> AddChargingStationIfNotExists(IChargingStation ChargingStation, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingStationResult> AddOrUpdateChargingStation(IChargingStation ChargingStation, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationResult> UpdateChargingStation(IChargingStation ChargingStation, String PropertyName, Object? NewValue, Object? OldValue = null, Context? DataSource = null, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingStationResult> DeleteChargingStation(IChargingStation ChargingStation, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationsResult> AddChargingStations(IEnumerable<IChargingStation> ChargingStations, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddChargingStationsResult> AddChargingStationsIfNotExist(IEnumerable<IChargingStation> ChargingStations, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateChargingStationsResult> AddOrUpdateChargingStations(IEnumerable<IChargingStation> ChargingStations, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateChargingStationsResult> UpdateChargingStations(IEnumerable<IChargingStation> ChargingStations, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteChargingStationsResult> DeleteChargingStations(IEnumerable<IChargingStation> ChargingStations, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IReceiveEVSEData

        public Task<AddEVSEResult> AddEVSE(IEVSE EVSE, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEResult> AddEVSEIfNotExists(IEVSE EVSE, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateEVSEResult> AddOrUpdateEVSE(IEVSE EVSE, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateEVSEResult> UpdateEVSE(IEVSE EVSE, String PropertyName, Object? NewValue, Object? OldValue = null, Context? DataSource = null, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteEVSEResult> DeleteEVSE(IEVSE EVSE, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEsResult> AddEVSEs(IEnumerable<IEVSE> EVSEs, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEsResult> AddEVSEsIfNotExist(IEnumerable<IEVSE> EVSEs, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateEVSEsResult> AddOrUpdateEVSEs(IEnumerable<IEVSE> EVSEs, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateEVSEsResult> UpdateEVSEs(IEnumerable<IEVSE> EVSEs, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteEVSEsResult> DeleteEVSEs(IEnumerable<IEVSE> EVSEs, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region IReceiveAdminStatus

        public Task<PushRoamingNetworkAdminStatusResult> UpdateRoamingNetworkAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate> RoamingNetworkAdminStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationOperatorAdminStatusResult> UpdateChargingStationOperatorAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate> ChargingStationOperatorAdminStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingPoolAdminStatusResult> UpdateChargingPoolAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate> ChargingPoolAdminStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationAdminStatusResult> UpdateChargingStationAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate> ChargingStationAdminStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEAdminStatusResult> UpdateEVSEAdminStatus(IEnumerable<EVSEAdminStatusUpdate> EVSEAdminStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IReceiveStatus

        public Task<PushRoamingNetworkStatusResult> UpdateRoamingNetworkStatus(IEnumerable<RoamingNetworkStatusUpdate> RoamingNetworkStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationOperatorStatusResult> UpdateChargingStationOperatorStatus(IEnumerable<ChargingStationOperatorStatusUpdate> ChargingStationOperatorStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingPoolStatusResult> UpdateChargingPoolStatus(IEnumerable<ChargingPoolStatusUpdate> ChargingPoolStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationStatusResult> UpdateChargingStationStatus(IEnumerable<ChargingStationStatusUpdate> ChargingStationStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEStatusResult> UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate> EVSEStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken CancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IReceiveStatus

        public Task<PushChargingPoolEnergyStatusResult> UpdateChargingPoolEnergyStatus(IEnumerable<ChargingPoolEnergyStatusUpdate> ChargingPoolEnergyStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken? CancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushChargingStationEnergyStatusResult> UpdateChargingStationEnergyStatus(IEnumerable<ChargingStationEnergyStatusUpdate> ChargingStationEnergyStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken? CancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEEnergyStatusResult> UpdateEVSEEnergyStatus(IEnumerable<EVSEEnergyStatusUpdate> EVSEEnergyStatusUpdates, DateTime? Timestamp = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null, CancellationToken? CancellationToken = null)
        {
            throw new NotImplementedException();
        }

        #endregion



    }

}
