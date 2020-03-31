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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.JSON;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// WWCP JSON I/O roaming network interface extentions.
    /// </summary>
    public static partial class IRoamingNetworkExtentions
    {

        #region ToJSON(this RoamingNetworks, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="Skip">The optional number of roaming networks to skip.</param>
        /// <param name="Take">The optional number of roaming networks to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure.</param>
        public static JArray ToJSON(this IEnumerable<IRoamingNetwork>                      RoamingNetworks,
                                    UInt64?                                                Skip                                      = null,
                                    UInt64?                                                Take                                      = null,
                                    Boolean                                                Embedded                                  = false,
                                    InfoStatus                                             ExpandChargingStationOperatorIds          = InfoStatus.ShowIdOnly,
                                    InfoStatus                                             ExpandRoamingNetworkIds                   = InfoStatus.ShowIdOnly,
                                    InfoStatus                                             ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                                    InfoStatus                                             ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                                    InfoStatus                                             ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                             ExpandDataLicenses                        = InfoStatus.ShowIdOnly,

                                    InfoStatus                                             ExpandEMobilityProviderId                 = InfoStatus.ShowIdOnly,

                                    CustomJSONSerializerDelegate<RoamingNetwork>           CustomRoamingNetworkSerializer            = null,
                                    CustomJSONSerializerDelegate<ChargingStationOperator>  CustomChargingStationOperatorSerializer   = null,
                                    CustomJSONSerializerDelegate<ChargingPool>             CustomChargingPoolSerializer              = null,
                                    CustomJSONSerializerDelegate<ChargingStation>          CustomChargingStationSerializer           = null,
                                    CustomJSONSerializerDelegate<EVSE>                     CustomEVSESerializer                      = null)


        => RoamingNetworks == null || !RoamingNetworks.Any()

                   ? new JArray()

                   : new JArray(RoamingNetworks.
                                    Where     (roamingnetwork => roamingnetwork != null).
                                    OrderBy   (roamingnetwork => roamingnetwork.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(roamingnetwork => roamingnetwork.ToJSON(Embedded,
                                                                                       ExpandChargingStationOperatorIds,
                                                                                       ExpandRoamingNetworkIds,
                                                                                       ExpandChargingStationIds,
                                                                                       ExpandEVSEIds,
                                                                                       ExpandBrandIds,
                                                                                       ExpandDataLicenses,
                                                                                       ExpandEMobilityProviderId,
                                                                                       CustomRoamingNetworkSerializer,
                                                                                       CustomChargingStationOperatorSerializer,
                                                                                       CustomChargingPoolSerializer,
                                                                                       CustomChargingStationSerializer,
                                                                                       CustomEVSESerializer)));

        #endregion

        #region ToJSON(this RoamingNetworks, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<IRoamingNetwork> RoamingNetworks, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return RoamingNetworks?.Any() == true
                       ? new JProperty(JPropertyKey, RoamingNetworks.ToJSON())
                       : null;

        }

        #endregion


        #region ToJSON(this RoamingNetworkAdminStatus,          Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<RoamingNetworkAdminStatus>  RoamingNetworkAdminStatus,
                                     UInt64?                                      Skip  = null,
                                     UInt64?                                      Take  = null)
        {

            #region Initial checks

            if (RoamingNetworkAdminStatus == null || !RoamingNetworkAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<RoamingNetwork_Id, RoamingNetworkAdminStatus>();

            foreach (var status in RoamingNetworkAdminStatus)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region ToJSON(this RoamingNetworkAdminStatusSchedules, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<RoamingNetworkAdminStatusSchedule>  RoamingNetworkAdminStatusSchedules,
                                     UInt64?                                              Skip         = null,
                                     UInt64?                                              Take         = null,
                                     UInt64?                                              HistorySize  = 1)
        {

            #region Initial checks

            if (RoamingNetworkAdminStatusSchedules == null || !RoamingNetworkAdminStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<RoamingNetwork_Id, RoamingNetworkAdminStatusSchedule>();

            foreach (var status in RoamingNetworkAdminStatusSchedules)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].StatusSchedule.Any() &&
                         _FilteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JObject(
                                                                   kvp.Value.StatusSchedule.

                                                                             // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                             GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                             Select           (group => group.First()).

                                                                             OrderByDescending(tsv   => tsv.Timestamp).
                                                                             Take             (HistorySize).
                                                                             Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                      tsv.Value.    ToString())))

                                                              )));

        }

        #endregion


        #region ToJSON(this RoamingNetworkStatus,               Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<RoamingNetworkStatus>  RoamingNetworkStatus,
                                     UInt64?                                 Skip  = null,
                                     UInt64?                                 Take  = null)
        {

            #region Initial checks

            if (RoamingNetworkStatus == null || !RoamingNetworkStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<RoamingNetwork_Id, RoamingNetworkStatus>();

            foreach (var status in RoamingNetworkStatus)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region ToJSON(this RoamingNetworkStatusSchedules,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<RoamingNetworkStatusSchedule>  RoamingNetworkStatusSchedules,
                                     UInt64?                                         Skip         = null,
                                     UInt64?                                         Take         = null,
                                     UInt64?                                         HistorySize  = 1)
        {

            #region Initial checks

            if (RoamingNetworkStatusSchedules == null || !RoamingNetworkStatusSchedules.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var _FilteredStatus = new Dictionary<RoamingNetwork_Id, RoamingNetworkStatusSchedule>();

            foreach (var status in RoamingNetworkStatusSchedules)
            {

                if (!_FilteredStatus.ContainsKey(status.Id))
                    _FilteredStatus.Add(status.Id, status);

                else if (_FilteredStatus[status.Id].StatusSchedule.Any() &&
                         _FilteredStatus[status.Id].StatusSchedule.First().Timestamp >= status.StatusSchedule.First().Timestamp)
                         _FilteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(_FilteredStatus.
                                   OrderBy(status => status.Key).
                                   SkipTakeFilter(Skip, Take).
                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JObject(
                                                                   kvp.Value.StatusSchedule.

                                                                             // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                             GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                             Select           (group => group.First()).

                                                                             OrderByDescending(tsv   => tsv.Timestamp).
                                                                             Take             (HistorySize).
                                                                             Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                      tsv.Value.    ToString())))

                                                              )));

        }

        #endregion

    }


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


        Boolean TryGetEMPRoamingProviderById(EMPRoamingProvider_Id Id, out IEMPRoamingProvider EMPRoamingProvider);
        IEMPRoamingProvider GetEMPRoamingProviderById(EMPRoamingProvider_Id Id);

        IEnumerable<IEMPRoamingProvider> EMPRoamingProviders { get; }


        Boolean TryGetCSORoamingProviderById(CSORoamingProvider_Id Id, out ICSORoamingProvider CSORoamingProvider);
        ICSORoamingProvider GetCSORoamingProviderById(CSORoamingProvider_Id Id);


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
        EVSE GetEVSEById(EVSE_Id EVSEId);
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

        eMobilityProvider CreateEMobilityProvider(eMobilityProvider_Id ProviderId, I18NString Name = null, I18NString Description = null, eMobilityProviderPriority Priority = null, Action<eMobilityProvider> Configurator = null, RemoteEMobilityProviderCreatorDelegate RemoteEMobilityProviderCreator = null, eMobilityProviderAdminStatusTypes AdminStatus = eMobilityProviderAdminStatusTypes.Operational, eMobilityProviderStatusTypes Status = eMobilityProviderStatusTypes.Available, Action<eMobilityProvider> OnSuccess = null, Action<RoamingNetwork, eMobilityProvider_Id> OnError = null);


        IEMPRoamingProvider CreateNewRoamingProvider(IEMPRoamingProvider _CPORoamingProvider, Action<IEMPRoamingProvider> Configurator = null);
        ICSORoamingProvider CreateNewRoamingProvider(ICSORoamingProvider eMobilityRoamingService, Action<ICSORoamingProvider> Configurator = null);






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

            RemoteStart(IEMPRoamingProvider       ICSORoamingProvider,
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


        JObject ToJSON(Boolean                                                Embedded                                  = false,
                       InfoStatus                                             ExpandChargingStationOperatorIds          = InfoStatus.ShowIdOnly,
                       InfoStatus                                             ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                       InfoStatus                                             ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                       InfoStatus                                             ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                       InfoStatus                                             ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                       InfoStatus                                             ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                       InfoStatus                                             ExpandEMobilityProviderId                 = InfoStatus.ShowIdOnly,
                       CustomJSONSerializerDelegate<RoamingNetwork>           CustomRoamingNetworkSerializer            = null,
                       CustomJSONSerializerDelegate<ChargingStationOperator>  CustomChargingStationOperatorSerializer   = null,
                       CustomJSONSerializerDelegate<ChargingPool>             CustomChargingPoolSerializer              = null,
                       CustomJSONSerializerDelegate<ChargingStation>          CustomChargingStationSerializer           = null,
                       CustomJSONSerializerDelegate<EVSE>                     CustomEVSESerializer                      = null);


    }

}
