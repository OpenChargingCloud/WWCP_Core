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

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The common interface of all roaming networks.
    /// </summary>
    public interface IRoamingNetwork : IEquatable<RoamingNetwork>, IComparable<RoamingNetwork>, IComparable,
                                       IEnumerable<IEntity>,
                                       IStatus<RoamingNetworkStatusTypes>,
                                       ISendAuthorizeStartStop,
                                       IReserveRemoteStartStop,
                                       IReceiveChargeDetailRecords,
                                       ISendChargeDetailRecords
    {

        /// <summary>
        /// The unique roaming network identification.
        /// </summary>
        new RoamingNetwork_Id      Id                          { get; }

        I18NString Name { get; set; }
        I18NString Description { get; set; }
        ReactiveSet<DataLicense> DataLicenses { get; }
        String DataSource { get; }


        Timestamped<RoamingNetworkAdminStatusTypes> AdminStatus { get; }
        IEnumerable<Timestamped<RoamingNetworkAdminStatusTypes>> AdminStatusSchedule(ulong? HistorySize = null);

        Timestamped<RoamingNetworkStatusTypes> Status { get; }


        ChargingReservationsStore  ReservationsStore           { get; }
        ChargingSessionsStore      SessionsStore               { get; }
        ChargeDetailRecordsStore   ChargeDetailRecordsStore    { get; }


        IEnumerable<ChargingStationOperator> ChargingStationOperators { get; }
        IEnumerable<ChargingStationOperator_Id> ChargingStationOperatorIds { get; }
        bool ContainsChargingStationOperator(ChargingStationOperator ChargingStationOperator);
        bool ContainsChargingStationOperator(ChargingStationOperator_Id ChargingStationOperatorId);
        IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorAdminStatusTypes>>>> ChargingStationOperatorAdminStatus { get; }
        ChargingStationOperator GetChargingStationOperatorById(ChargingStationOperator_Id ChargingStationOperatorId);
        ChargingStationOperator CreateChargingStationOperator(ChargingStationOperator_Id ChargingStationOperatorId, I18NString Name = null, I18NString Description = null, Action<ChargingStationOperator> Configurator = null, RemoteChargingStationOperatorCreatorDelegate RemoteChargingStationOperatorCreator = null, ChargingStationOperatorAdminStatusTypes AdminStatus = ChargingStationOperatorAdminStatusTypes.Operational, ChargingStationOperatorStatusTypes Status = ChargingStationOperatorStatusTypes.Available, Action<ChargingStationOperator> OnSuccess = null, Action<RoamingNetwork, ChargingStationOperator_Id> OnError = null);
        ChargingStationOperator CreateChargingStationOperator(IEnumerable<ChargingStationOperator_Id> ChargingStationOperatorIds, I18NString Name = null, I18NString Description = null, Action<ChargingStationOperator> Configurator = null, RemoteChargingStationOperatorCreatorDelegate RemoteChargingStationOperatorCreator = null, ChargingStationOperatorAdminStatusTypes AdminStatus = ChargingStationOperatorAdminStatusTypes.Operational, ChargingStationOperatorStatusTypes Status = ChargingStationOperatorStatusTypes.Available, Action<ChargingStationOperator> OnSuccess = null, Action<RoamingNetwork, ChargingStationOperator_Id> OnError = null);
        bool TryGetChargingStationOperatorById(ChargingStationOperator_Id ChargingStationOperatorId, out ChargingStationOperator ChargingStationOperator);
        bool TryGetChargingStationOperatorById(ChargingStationOperator_Id? ChargingStationOperatorId, out ChargingStationOperator ChargingStationOperator);


        IEnumerable<ChargingPool> ChargingPools { get; }
        bool ContainsChargingPool(ChargingPool ChargingPool);
        bool ContainsChargingPool(ChargingPool_Id ChargingPoolId);
        ChargingPool GetChargingPoolById(ChargingPool_Id ChargingPoolId);
        bool TryGetChargingPoolById(ChargingPool_Id ChargingPoolId, out ChargingPool ChargingPool);
        IEnumerable<ChargingPool_Id> ChargingPoolIds(IncludeChargingPoolDelegate IncludePools = null);
        IEnumerable<ChargingPoolAdminStatus> ChargingPoolAdminStatus(IncludeChargingPoolDelegate IncludePools = null);
        IEnumerable<ChargingPoolStatus> ChargingPoolStatus(IncludeChargingPoolDelegate IncludePools = null);

        IEnumerable<ChargingStation> ChargingStations { get; }
        bool ContainsChargingStation(ChargingStation ChargingStation);
        bool ContainsChargingStation(ChargingStation_Id ChargingStationId);
        ChargingStation GetChargingStationById(ChargingStation_Id ChargingStationId);
        bool TryGetChargingStationById(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation);
        IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate IncludeStations = null);
        IEnumerable<ChargingStationAdminStatus> ChargingStationAdminStatus(IncludeChargingStationDelegate IncludeStations = null);
        IEnumerable<ChargingStationStatus> ChargingStationStatus(IncludeChargingStationDelegate IncludeStations = null);

        IEnumerable<EVSE> EVSEs { get; }
        bool ContainsEVSE(EVSE EVSE);
        bool ContainsEVSE(EVSE_Id EVSEId);
        EVSE GetEVSEbyId(EVSE_Id EVSEId);
        bool TryGetEVSEById(EVSE_Id EVSEId, out EVSE EVSE);
        IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate IncludeEVSEs = null);
        IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate IncludeEVSEs = null);
        IEnumerable<EVSEAdminStatus> EVSEAdminStatusSchedule(IncludeEVSEDelegate IncludeEVSEs = null);
        IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate IncludeEVSEs = null);



        bool ContainsEMobilityProvider(eMobilityProvider EMobilityProvider);
        bool ContainsEMobilityProvider(eMobilityProvider_Id EMobilityProviderId);

        eMobilityProvider GetEMobilityProviderById(eMobilityProvider_Id EMobilityProviderId);



        IEnumerable<eMobilityProvider> eMobilityProviders { get; }
        IEnumerable<eMobilityProvider_Id> eMobilityProviderIds { get; }
        IEnumerable<KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderAdminStatusTypes>>>> eMobilityProviderAdminStatus { get; }
        IEnumerable<KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderStatusTypes>>>> eMobilityProviderStatus { get; }
        IEnumerable<IEMPRoamingProvider> EMPRoamingProviders { get; }
        eMobilityProvider CreateEMobilityProvider(eMobilityProvider_Id ProviderId, I18NString Name = null, I18NString Description = null, eMobilityProviderPriority Priority = null, Action<eMobilityProvider> Configurator = null, RemoteEMobilityProviderCreatorDelegate RemoteEMobilityProviderCreator = null, eMobilityProviderAdminStatusTypes AdminStatus = eMobilityProviderAdminStatusTypes.Operational, eMobilityProviderStatusTypes Status = eMobilityProviderStatusTypes.Available, Action<eMobilityProvider> OnSuccess = null, Action<RoamingNetwork, eMobilityProvider_Id> OnError = null);


        ICSORoamingProvider CreateNewRoamingProvider(ICSORoamingProvider _CPORoamingProvider, Action<ICSORoamingProvider> Configurator = null);
        IEMPRoamingProvider CreateNewRoamingProvider(IEMPRoamingProvider eMobilityRoamingService, Action<IEMPRoamingProvider> Configurator = null);






        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<RemoteStartResult>

            RemoteStart(ICSORoamingProvider       ICSORoamingProvider,
                        ChargingLocation          ChargingLocation,
                        ChargingProduct           ChargingProduct            = null,
                        ChargingReservation_Id?   ReservationId              = null,
                        ChargingSession_Id?       SessionId                  = null,
                        eMobilityProvider_Id?     ProviderId                 = null,
                        RemoteAuthentication      RemoteAuthentication       = null,

                        DateTime?                 Timestamp                  = null,
                        CancellationToken?        CancellationToken          = null,
                        EventTracking_Id          EventTrackingId            = null,
                        TimeSpan?                 RequestTimeout             = null);

        #endregion





    }

}
