/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for roaming networks.
    /// </summary>
    public static partial class IRoamingNetworkExtensions
    {

        #region ToJSON(this RoamingNetworks, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="Skip">The optional number of roaming networks to skip.</param>
        /// <param name="Take">The optional number of roaming networks to return.</param>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        public static JArray ToJSON(this IEnumerable<IRoamingNetwork>                          RoamingNetworks,
                                    UInt64?                                                    Skip                                      = null,
                                    UInt64?                                                    Take                                      = null,
                                    Boolean                                                    Embedded                                  = false,
                                    InfoStatus                                                 ExpandChargingStationOperatorIds          = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                 ExpandRoamingNetworkIds                   = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                 ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                 ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                 ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                 ExpandDataLicenses                        = InfoStatus.ShowIdOnly,

                                    InfoStatus                                                 ExpandEMobilityProviderId                 = InfoStatus.ShowIdOnly,

                                    CustomJObjectSerializerDelegate<RoamingNetwork>?           CustomRoamingNetworkSerializer            = null,
                                    CustomJObjectSerializerDelegate<ChargingStationOperator>?  CustomChargingStationOperatorSerializer   = null,
                                    CustomJObjectSerializerDelegate<ChargingPool>?             CustomChargingPoolSerializer              = null,
                                    CustomJObjectSerializerDelegate<ChargingStation>?          CustomChargingStationSerializer           = null,
                                    CustomJObjectSerializerDelegate<EVSE>?                     CustomEVSESerializer                      = null)


        => RoamingNetworks is null || !RoamingNetworks.Any()

               ? new JArray()

               : new JArray(RoamingNetworks.
                                Where         (roamingNetwork => roamingNetwork is not null).
                                OrderBy       (roamingNetwork => roamingNetwork.Id).
                                SkipTakeFilter(Skip, Take).
                                SafeSelect    (roamingNetwork => roamingNetwork.ToJSON(Embedded,
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

    }


    /// <summary>
    /// The common interface of all roaming networks.
    /// </summary>
    public interface IRoamingNetwork : IEntity<RoamingNetwork_Id>,
                                       IAdminStatus<RoamingNetworkAdminStatusTypes>,
                                       IStatus<RoamingNetworkStatusTypes>,
                                       ISendAuthorizeStartStop,
                                       IReserveRemoteStartStop,
                                       IReceiveChargeDetailRecords,
                                       ISendChargeDetailRecords,
                                       IEquatable<RoamingNetwork>,
                                       IComparable<RoamingNetwork>,
                                       IComparable
    {

        /// <summary>
        /// The unique roaming network identification.
        /// </summary>
        new RoamingNetwork_Id       Id                          { get; }

        I18NString                  Name                        { get; }
        I18NString                  Description                 { get; }
        ReactiveSet<DataLicense>    DataLicenses                { get; }
        String?                     DataSource                  { get; }



        ChargingReservationsStore   ReservationsStore           { get; }
        ChargingSessionsStore       SessionsStore               { get; }


        #region EMPRoamingProviders

        // EMPRoamingProviders provide access to other e-mobility operators via a roaming network

        IEnumerable<IEMPRoamingProvider>                 EMPRoamingProviders { get; }

        Boolean                                          ContainsEMPRoamingProvider  (IEMPRoamingProvider     EMPRoamingProvider);
        Boolean                                          ContainsEMPRoamingProvider  (EMPRoamingProvider_Id   EMPRoamingProviderId);
        IEMPRoamingProvider?                             GetEMPRoamingProviderById   (EMPRoamingProvider_Id   EMPRoamingProviderId);
        IEMPRoamingProvider?                             GetEMPRoamingProviderById   (EMPRoamingProvider_Id?  EMPRoamingProviderId);
        Boolean                                          TryGetEMPRoamingProviderById(EMPRoamingProvider_Id   Id, out IEMPRoamingProvider? EMPRoamingProvider);
        Boolean                                          TryGetEMPRoamingProviderById(EMPRoamingProvider_Id?  Id, out IEMPRoamingProvider? EMPRoamingProvider);
        IEMPRoamingProvider?                             RemoveEMPRoamingProvider    (EMPRoamingProvider_Id   EMPRoamingProviderId);
        IEMPRoamingProvider?                             RemoveEMPRoamingProvider    (EMPRoamingProvider_Id?  EMPRoamingProviderId);
        Boolean                                          TryRemoveEMPRoamingProvider (EMPRoamingProvider_Id   EMPRoamingProviderId, out IEMPRoamingProvider? EMPRoamingProvider);
        Boolean                                          TryRemoveEMPRoamingProvider (EMPRoamingProvider_Id?  EMPRoamingProviderId, out IEMPRoamingProvider? EMPRoamingProvider);

        IEMPRoamingProvider                              CreateNewRoamingProvider    (IEMPRoamingProvider           EMPRoamingProvider,
                                                                                      Action<IEMPRoamingProvider>?  Configurator = null);

        #endregion

        #region EMobilityProviders

        IEnumerable<EMobilityProvider>             EMobilityProviders          { get; }
        Boolean                                    ContainsEMobilityProvider   (EMobilityProvider                  EMobilityProvider);
        Boolean                                    ContainsEMobilityProvider   (EMobilityProvider_Id               EMobilityProviderId);
        EMobilityProvider?                         GetEMobilityProviderById    (EMobilityProvider_Id               EMobilityProviderId);
        EMobilityProvider?                         GetEMobilityProviderById    (EMobilityProvider_Id?              EMobilityProviderId);
        Boolean                                    TryGetEMobilityProviderById (EMobilityProvider_Id               EMobilityProviderId, out EMobilityProvider? EMobilityProvider);
        Boolean                                    TryGetEMobilityProviderById (EMobilityProvider_Id?              EMobilityProviderId, out EMobilityProvider? EMobilityProvider);
        EMobilityProvider?                         RemoveEMobilityProvider     (EMobilityProvider_Id               EMobilityProviderId);
        EMobilityProvider?                         RemoveEMobilityProvider     (EMobilityProvider_Id?              EMobilityProviderId);
        Boolean                                    TryRemoveEMobilityProvider  (EMobilityProvider_Id               EMobilityProviderId, out EMobilityProvider? EMobilityProvider);
        Boolean                                    TryRemoveEMobilityProvider  (EMobilityProvider_Id?              EMobilityProviderId, out EMobilityProvider? EMobilityProvider);

        IEnumerable<EMobilityProvider_Id>          EMobilityProviderIds        (IncludeEMobilityProviderDelegate?  IncludeEMobilityProvider   = null);
        IEnumerable<EMobilityProviderAdminStatus>  EMobilityProviderAdminStatus(IncludeEMobilityProviderDelegate?  IncludeEMobilityProvider   = null);
        IEnumerable<EMobilityProviderStatus>       EMobilityProviderStatus     (IncludeEMobilityProviderDelegate?  IncludeEMobilityProvider   = null);

        EMobilityProvider?  CreateEMobilityProvider(EMobilityProvider_Id                           ProviderId,
                                                    I18NString?                                    Name                             = null,
                                                    I18NString?                                    Description                      = null,
                                                    eMobilityProviderPriority?                     Priority                         = null,
                                                    Action<EMobilityProvider>?                     Configurator                     = null,
                                                    RemoteEMobilityProviderCreatorDelegate?        RemoteEMobilityProviderCreator   = null,
                                                    EMobilityProviderAdminStatusTypes?             InitialAdminStatus               = null,
                                                    EMobilityProviderStatusTypes?                  InitialStatus                    = null,
                                                    Action<EMobilityProvider>?                     OnSuccess                        = null,
                                                    Action<RoamingNetwork, EMobilityProvider_Id>?  OnError                          = null);

        #endregion


        #region CSORoamingProviders

        // CSORoamingProviders provide access to other charging station operators via a roaming network

        IEnumerable<ICSORoamingProvider>                 CSORoamingProviders { get; }
        Boolean                                          ContainsCSORoamingProvider  (ICSORoamingProvider     CSORoamingProvider);
        Boolean                                          ContainsCSORoamingProvider  (CSORoamingProvider_Id   CSORoamingProviderId);
        ICSORoamingProvider?                             GetCSORoamingProviderById   (CSORoamingProvider_Id   CSORoamingProviderId);
        ICSORoamingProvider?                             GetCSORoamingProviderById   (CSORoamingProvider_Id?  CSORoamingProviderId);
        Boolean                                          TryGetCSORoamingProviderById(CSORoamingProvider_Id   Id, out ICSORoamingProvider? CSORoamingProvider);
        Boolean                                          TryGetCSORoamingProviderById(CSORoamingProvider_Id?  Id, out ICSORoamingProvider? CSORoamingProvider);
        ICSORoamingProvider?                             RemoveCSORoamingProvider    (CSORoamingProvider_Id   CSORoamingProviderId);
        ICSORoamingProvider?                             RemoveCSORoamingProvider    (CSORoamingProvider_Id?  CSORoamingProviderId);
        Boolean                                          TryRemoveCSORoamingProvider (CSORoamingProvider_Id   CSORoamingProviderId, out ICSORoamingProvider? CSORoamingProvider);
        Boolean                                          TryRemoveCSORoamingProvider (CSORoamingProvider_Id?  CSORoamingProviderId, out ICSORoamingProvider? CSORoamingProvider);


        ICSORoamingProvider                              CreateCSORoamingProvider    (ICSORoamingProvider           CSORoamingProvider,
                                                                                      Action<ICSORoamingProvider>?  Configurator = null);

        #endregion

        #region ChargingStationOperators

        IEnumerable<IChargingStationOperator>            ChargingStationOperators { get; }
        Boolean                                          ContainsChargingStationOperator   (IChargingStationOperator                 ChargingStationOperator);
        Boolean                                          ContainsChargingStationOperator   (ChargingStationOperator_Id               ChargingStationOperatorId);
        IChargingStationOperator?                        GetChargingStationOperatorById    (ChargingStationOperator_Id               ChargingStationOperatorId);
        IChargingStationOperator?                        GetChargingStationOperatorById    (ChargingStationOperator_Id?              ChargingStationOperatorId);
        Boolean                                          TryGetChargingStationOperatorById (ChargingStationOperator_Id               ChargingStationOperatorId, out IChargingStationOperator? ChargingStationOperator);
        Boolean                                          TryGetChargingStationOperatorById (ChargingStationOperator_Id?              ChargingStationOperatorId, out IChargingStationOperator? ChargingStationOperator);
        IChargingStationOperator?                        RemoveChargingStationOperator     (ChargingStationOperator_Id               ChargingStationOperatorId);
        IChargingStationOperator?                        RemoveChargingStationOperator     (ChargingStationOperator_Id?              ChargingStationOperatorId);
        Boolean                                          TryRemoveChargingStationOperator  (ChargingStationOperator_Id               ChargingStationOperatorId, out IChargingStationOperator? ChargingStationOperator);
        Boolean                                          TryRemoveChargingStationOperator  (ChargingStationOperator_Id?              ChargingStationOperatorId, out IChargingStationOperator? ChargingStationOperator);

        IEnumerable<ChargingStationOperator_Id>          ChargingStationOperatorIds        (IncludeChargingStationOperatorDelegate?  IncludeChargingStationOperator   = null);

        IEnumerable<ChargingStationOperatorAdminStatus>  ChargingStationOperatorAdminStatus(IncludeChargingStationOperatorDelegate?  IncludeChargingStationOperator   = null);

        //IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorAdminStatusTypes>>>>
        //    ChargingStationOperatorAdminStatusSchedule(IncludeChargingStationOperatorDelegate?       IncludeChargingStationOperator   = null,
        //                                               Func<DateTime,                     Boolean>?  TimestampFilter                  = null,
        //                                               Func<ChargingPoolAdminStatusTypes, Boolean>?  AdminStatusFilter                = null,
        //                                               UInt64?                                       Skip                             = null,
        //                                               UInt64?                                       Take                             = null);

        IEnumerable<ChargingStationOperatorStatus>       ChargingStationOperatorStatus     (IncludeChargingStationOperatorDelegate?  IncludeChargingStationOperator   = null);

        //IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorStatusTypes>>>>
        //    ChargingStationOperatorStatusSchedule     (IncludeChargingStationOperatorDelegate?       IncludeChargingStationOperator   = null,
        //                                               Func<DateTime,                Boolean>?       TimestampFilter                  = null,
        //                                               Func<ChargingPoolStatusTypes, Boolean>?       StatusFilter                     = null,
        //                                               UInt64?                                       Skip                             = null,
        //                                               UInt64?                                       Take                             = null);


        IChargingStationOperator? CreateChargingStationOperator(ChargingStationOperator_Id                           ChargingStationOperatorId,
                                                                I18NString?                                          Name                                   = null,
                                                                I18NString?                                          Description                            = null,
                                                                Action<IChargingStationOperator>?                    Configurator                           = null,
                                                                RemoteChargingStationOperatorCreatorDelegate?        RemoteChargingStationOperatorCreator   = null,
                                                                ChargingStationOperatorAdminStatusTypes?             InitialAdminStatus                     = null,
                                                                ChargingStationOperatorStatusTypes?                  InitialStatus                          = null,
                                                                Action<IChargingStationOperator>?                    OnSuccess                              = null,
                                                                Action<RoamingNetwork, ChargingStationOperator_Id>?  OnError                                = null);

        #endregion

        #region ChargingPools

        IEnumerable<IChargingPool>               ChargingPools { get; }
        Boolean                                  ContainsChargingPool             (IChargingPool                                 ChargingPool);
        Boolean                                  ContainsChargingPool             (ChargingPool_Id                               ChargingPoolId);
        IChargingPool?                           GetChargingPoolById              (ChargingPool_Id                               ChargingPoolId);
        Boolean                                  TryGetChargingPoolById           (ChargingPool_Id                               ChargingPoolId, out IChargingPool? ChargingPool);
        IEnumerable<ChargingPool_Id>             ChargingPoolIds                  (IncludeChargingPoolDelegate?                  IncludeChargingPools   = null);
        IEnumerable<ChargingPoolAdminStatus>     ChargingPoolAdminStatus          (IncludeChargingPoolDelegate?                  IncludeChargingPools   = null);

        IEnumerable<Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>>>
                                                 ChargingPoolAdminStatusSchedule  (IncludeChargingPoolDelegate?                  IncludeChargingPools   = null,
                                                                                   Func<DateTime,                     Boolean>?  TimestampFilter        = null,
                                                                                   Func<ChargingPoolAdminStatusTypes, Boolean>?  AdminStatusFilter      = null,
                                                                                   UInt64?                                       Skip                   = null,
                                                                                   UInt64?                                       Take                   = null);
        IEnumerable<ChargingPoolStatus>          ChargingPoolStatus               (IncludeChargingPoolDelegate?                  IncludeChargingPools   = null);

        IEnumerable<Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusTypes>>>>
                                                 ChargingPoolStatusSchedule       (IncludeChargingPoolDelegate?                  IncludeChargingPools   = null,
                                                                                   Func<DateTime,                Boolean>?       TimestampFilter        = null,
                                                                                   Func<ChargingPoolStatusTypes, Boolean>?       AdminStatusFilter      = null,
                                                                                   UInt64?                                       Skip                   = null,
                                                                                   UInt64?                                       Take                   = null);

        #endregion

        #region ChargingStations

        IEnumerable<IChargingStation>            ChargingStations { get; }
        Boolean                                  ContainsChargingStation           (IChargingStation                                 ChargingStation);
        Boolean                                  ContainsChargingStation           (ChargingStation_Id                               ChargingStationId);
        IChargingStation?                        GetChargingStationById            (ChargingStation_Id                               ChargingStationId);
        Boolean                                  TryGetChargingStationById         (ChargingStation_Id                               ChargingStationId, out IChargingStation? ChargingStation);
        IEnumerable<ChargingStation_Id>          ChargingStationIds                (IncludeChargingStationDelegate?                  IncludeChargingStations   = null);
        IEnumerable<ChargingStationAdminStatus>  ChargingStationAdminStatus        (IncludeChargingStationDelegate?                  IncludeChargingStations   = null);

        IEnumerable<Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>>>
                                                 ChargingStationAdminStatusSchedule(IncludeChargingStationDelegate?                  IncludeChargingStations   = null,
                                                                                    Func<DateTime,                        Boolean>?  TimestampFilter           = null,
                                                                                    Func<ChargingStationAdminStatusTypes, Boolean>?  AdminStatusFilter         = null,
                                                                                    UInt64?                                          Skip                      = null,
                                                                                    UInt64?                                          Take                      = null);

        IEnumerable<ChargingStationStatus>       ChargingStationStatus             (IncludeChargingStationDelegate?                  IncludeChargingStations   = null);

        IEnumerable<Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusTypes>>>>
                                                 ChargingStationStatusSchedule     (IncludeChargingStationDelegate?                  IncludeChargingStations   = null,
                                                                                    Func<DateTime,                   Boolean>?       TimestampFilter           = null,
                                                                                    Func<ChargingStationStatusTypes, Boolean>?       StatusFilter              = null,
                                                                                    UInt64?                                          Skip                      = null,
                                                                                    UInt64?                                          Take                      = null);

        #region SetChargingStationAdminStatus

        void SetChargingStationAdminStatus(ChargingStation_Id                            ChargingStationId,
                                           Timestamped<ChargingStationAdminStatusTypes>  CurrentAdminStatus);

        void SetChargingStationAdminStatus(ChargingStation_Id                                         ChargingStationId,
                                           IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>  CurrentAdminStatusList);

        #endregion

        #region SetChargingStationStatus

        void SetChargingStationStatus(ChargingStation_Id                       ChargingStationId,
                                      Timestamped<ChargingStationStatusTypes>  CurrentStatus);

        void SetChargingStationStatus(ChargingStation_Id                                    ChargingStationId,
                                      IEnumerable<Timestamped<ChargingStationStatusTypes>>  CurrentStatusList);

        #endregion

        #endregion

        #region EVSEs

        IEnumerable<IEVSE>                       EVSEs { get; }

        Boolean                                  ContainsEVSE           (IEVSE                                 EVSE);

        Boolean                                  ContainsEVSE           (EVSE_Id                               EVSEId);

        IEVSE?                                   GetEVSEById            (EVSE_Id                               EVSEId);

        Boolean                                  TryGetEVSEById         (EVSE_Id                               EVSEId, out IEVSE? EVSE);

        IEnumerable<EVSE_Id>                     EVSEIds                (IncludeEVSEDelegate?                  IncludeEVSEs        = null);

        IEnumerable<EVSEAdminStatus>             EVSEAdminStatus        (IncludeEVSEDelegate?                  IncludeEVSEs        = null);

        IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>>
                                                 EVSEAdminStatusSchedule(IncludeEVSEDelegate?                  IncludeEVSEs        = null,
                                                                         Func<DateTime,             Boolean>?  TimestampFilter     = null,
                                                                         Func<EVSEAdminStatusTypes, Boolean>?  AdminStatusFilter   = null,
                                                                         UInt64?                               Skip                = null,
                                                                         UInt64?                               Take                = null);

        IEnumerable<EVSEStatus>                  EVSEStatus             (IncludeEVSEDelegate?                  IncludeEVSEs        = null);

        IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>>
                                                 EVSEStatusSchedule     (IncludeEVSEDelegate?                  IncludeEVSEs        = null,
                                                                         Func<DateTime,        Boolean>?       TimestampFilter     = null,
                                                                         Func<EVSEStatusTypes, Boolean>?       StatusFilter        = null,
                                                                         UInt64?                               Skip                = null,
                                                                         UInt64?                               Take                = null);


        #region SetEVSEAdminStatus

        void SetEVSEAdminStatus(IEnumerable<EVSEAdminStatus> EVSEAdminStatusList);

        void SetEVSEAdminStatus(EVSE_Id                            EVSEId,
                                Timestamped<EVSEAdminStatusTypes>  NewAdminStatus);

        void SetEVSEAdminStatus(EVSE_Id               EVSEId,
                                DateTime              Timestamp,
                                EVSEAdminStatusTypes  NewAdminStatus);

        void SetEVSEAdminStatus(EVSE_Id                                         EVSEId,
                                IEnumerable<Timestamped<EVSEAdminStatusTypes>>  AdminStatusList,
                                ChangeMethods                                   ChangeMethod  = ChangeMethods.Replace);

        #endregion

        #region SetEVSEStatus

        void SetEVSEStatus(IEnumerable<EVSEStatus> EVSEStatusList);

        void SetEVSEStatus(EVSE_Id                       EVSEId,
                           Timestamped<EVSEStatusTypes>  NewStatus);

        void SetEVSEStatus(EVSE_Id          EVSEId,
                           DateTime         Timestamp,
                           EVSEStatusTypes  NewStatus);

        void SetEVSEStatus(EVSE_Id                                    EVSEId,
                           IEnumerable<Timestamped<EVSEStatusTypes>>  StatusList,
                           ChangeMethods                              ChangeMethod  = ChangeMethods.Replace);

        #endregion

        #endregion


        #region Parking Operators...

        /// <summary>
        /// Return all parking operators registered within this roaming network.
        /// </summary>
        IEnumerable<ParkingOperator> ParkingOperators { get; }

        /// <summary>
        /// Called whenever a parking operator will be or was added.
        /// </summary>
        IVotingSender<DateTime, RoamingNetwork, ParkingOperator, Boolean> OnParkingOperatorAddition { get; }

        /// <summary>
        /// Called whenever a parking operator will be or was removed.
        /// </summary>
        IVotingSender<DateTime, RoamingNetwork, ParkingOperator, Boolean> OnParkingOperatorRemoval { get; }


        /// <summary>
        /// Return the admin status of all parking operators registered within this roaming network.
        /// </summary>
        IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorAdminStatusTypes>>>> ParkingOperatorAdminStatus { get; }

        /// <summary>
        /// Return the status of all parking operators registered within this roaming network.
        /// </summary>
        IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorStatusTypes>>>> ParkingOperatorStatus { get; }


        /// <summary>
        /// Create and register a new parking operator having the given
        /// unique parking operator identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new parking operator.</param>
        /// <param name="Name">The offical (multi-language) name of the parking operator.</param>
        /// <param name="Description">An optional (multi-language) description of the parking operator.</param>
        /// <param name="Configurator">An optional delegate to configure the new parking operator before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new parking operator after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the parking operator failed.</param>
        ParkingOperator? CreateNewParkingOperator(ParkingOperator_Id                           Id,
                                                  I18NString?                                  Name                           = null,
                                                  I18NString?                                  Description                    = null,
                                                  Action<ParkingOperator>?                     Configurator                   = null,
                                                  RemoteParkingOperatorCreatorDelegate?        RemoteParkingOperatorCreator   = null,
                                                  ParkingOperatorAdminStatusTypes?             InititalAdminStatus            = ParkingOperatorAdminStatusTypes.Operational,
                                                  ParkingOperatorStatusTypes?                  InititalStatus                 = ParkingOperatorStatusTypes.Available,
                                                  Action<ParkingOperator>?                     OnSuccess                      = null,
                                                  Action<RoamingNetwork, ParkingOperator_Id>?  OnError                        = null);

        /// <summary>
        /// Create and register a new parking space having the given
        /// unique parking space identification.
        /// </summary>
        /// <param name="ParkingSpaceId">The unique identification of the new charging pool.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging pool before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging pool after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging pool failed.</param>
        ParkingSpace CreateParkingSpace(ParkingSpace_Id                                    ParkingSpaceId,
                                        Action<ParkingSpace>?                              Configurator   = null,
                                        Action<ParkingSpace>?                              OnSuccess      = null,
                                        Action<ChargingStationOperator, ParkingSpace_Id>?  OnError        = null);

        /// <summary>
        /// Check if the given ParkingOperator is already present within the roaming network.
        /// </summary>
        /// <param name="ParkingOperator">An parking Operator.</param>
        Boolean ContainsParkingOperator(ParkingOperator ParkingOperator);

        /// <summary>
        /// Check if the given ParkingOperator identification is already present within the roaming network.
        /// </summary>
        /// <param name="ParkingOperatorId">The unique identification of the parking Operator.</param>
        Boolean ContainsParkingOperator(ParkingOperator_Id ParkingOperatorId);

        ParkingOperator GetParkingOperatorById(ParkingOperator_Id ParkingOperatorId);

        Boolean TryGetParkingOperatorById(ParkingOperator_Id ParkingOperatorId, out ParkingOperator ParkingOperator);

        ParkingOperator RemoveParkingOperator(ParkingOperator_Id ParkingOperatorId);

        Boolean TryRemoveParkingOperator(ParkingOperator_Id ParkingOperatorId, out ParkingOperator ParkingOperator);


        #region OnParkingOperatorData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated parking Operator changed.
        /// </summary>
        public event OnParkingOperatorDataChangedDelegate?         OnParkingOperatorDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated parking Operator changed.
        /// </summary>
        public event OnParkingOperatorStatusChangedDelegate?       OnParkingOperatorStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated parking Operator changed.
        /// </summary>
        public event OnParkingOperatorAdminStatusChangedDelegate?  OnParkingOperatorAdminStatusChanged;

        #endregion

        #endregion

        #region Grid Operators...

        /// <summary>
        /// Return all smart cities registered within this roaming network.
        /// </summary>
        IEnumerable<GridOperator> GridOperators { get; }


        /// <summary>
        /// Called whenever an EVServiceProvider will be or was added.
        /// </summary>
        IVotingSender<DateTime, RoamingNetwork, GridOperator, Boolean> OnGridOperatorAddition { get; }

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        IVotingSender<DateTime, RoamingNetwork, GridOperator, Boolean> OnGridOperatorRemoval { get; }

        /// <summary>
        /// Return the admin status of all smart cities registered within this roaming network.
        /// </summary>
        IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorAdminStatusType>>>> GridOperatorsAdminStatus { get; }

        /// <summary>
        /// Return the status of all smart cities registered within this roaming network.
        /// </summary>
        IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorStatusType>>>> GridOperatorsStatus { get; }



        /// <summary>
        /// Create and register a new e-mobility (service) provider having the given
        /// unique smart city identification.
        /// </summary>
        /// <param name="GridOperatorId">The unique identification of the new smart city.</param>
        /// <param name="Name">The offical (multi-language) name of the smart city.</param>
        /// <param name="Description">An optional (multi-language) description of the smart city.</param>
        /// <param name="Configurator">An optional delegate to configure the new smart city before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new smart city after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the smart city failed.</param>
        GridOperator CreateNewGridOperator(GridOperator_Id                          GridOperatorId,
                                           I18NString                               Name                        = null,
                                           I18NString                               Description                 = null,
                                           GridOperatorPriority                     Priority                    = null,
                                           GridOperatorAdminStatusType              AdminStatus                 = GridOperatorAdminStatusType.Available,
                                           GridOperatorStatusType                   Status                      = GridOperatorStatusType.Available,
                                           Action<GridOperator>                     Configurator                = null,
                                           Action<GridOperator>                     OnSuccess                   = null,
                                           Action<RoamingNetwork, GridOperator_Id>  OnError                     = null,
                                           RemoteGridOperatorCreatorDelegate        RemoteGridOperatorCreator   = null);

        /// <summary>
        /// Check if the given GridOperator is already present within the roaming network.
        /// </summary>
        /// <param name="GridOperator">An Charging Station Operator.</param>
        Boolean ContainsGridOperator(GridOperator GridOperator);

        /// <summary>
        /// Check if the given GridOperator identification is already present within the roaming network.
        /// </summary>
        /// <param name="GridOperatorId">The unique identification of the Charging Station Operator.</param>
        Boolean ContainsGridOperator(GridOperator_Id GridOperatorId);

        GridOperator GetGridOperatorById(GridOperator_Id GridOperatorId);

        Boolean TryGetGridOperatorById(GridOperator_Id GridOperatorId, out GridOperator GridOperator);

        GridOperator RemoveGridOperator(GridOperator_Id GridOperatorId);

        Boolean TryRemoveGridOperator(GridOperator_Id GridOperatorId, out GridOperator GridOperator);

        #endregion

        #region Smart Cities...

        /// <summary>
        /// Return all smart cities registered within this roaming network.
        /// </summary>
        IEnumerable<SmartCityProxy>  SmartCities    { get; }

        /// <summary>
        /// Return the admin status of all smart cities registered within this roaming network.
        /// </summary>
        IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityAdminStatusTypes>>>> SmartCitiesAdminStatus { get; }

        /// <summary>
        /// Return the status of all smart cities registered within this roaming network.
        /// </summary>
        IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityStatusTypes>>>> SmartCitiesStatus { get; }


        /// <summary>
        /// Called whenever an EVServiceProvider will be or was added.
        /// </summary>
        IVotingSender<DateTime, RoamingNetwork, SmartCityProxy, Boolean> OnSmartCityAddition { get; }

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        IVotingSender<DateTime, RoamingNetwork, SmartCityProxy, Boolean> OnSmartCityRemoval { get; }



        /// <summary>
        /// Create and register a new e-mobility (service) provider having the given
        /// unique smart city identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new smart city.</param>
        /// <param name="Name">The offical (multi-language) name of the smart city.</param>
        /// <param name="Description">An optional (multi-language) description of the smart city.</param>
        /// <param name="Configurator">An optional delegate to configure the new smart city before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new smart city after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the smart city failed.</param>
        SmartCityProxy? CreateNewSmartCity(SmartCity_Id                           Id,
                                           I18NString?                            Name                     = null,
                                           I18NString?                            Description              = null,
                                           SmartCityPriority?                     Priority                 = null,
                                           SmartCityAdminStatusTypes?             InitialAdminStatus       = SmartCityAdminStatusTypes.Available,
                                           SmartCityStatusTypes?                  InitialStatus            = SmartCityStatusTypes.Available,
                                           Action<SmartCityProxy>?                Configurator             = null,
                                           Action<SmartCityProxy>?                OnSuccess                = null,
                                           Action<RoamingNetwork, SmartCity_Id>?  OnError                  = null,
                                           RemoteSmartCityCreatorDelegate?        RemoteSmartCityCreator   = null);

        /// <summary>
        /// Check if the given SmartCity is already present within the roaming network.
        /// </summary>
        /// <param name="SmartCity">An Charging Station Operator.</param>
        Boolean ContainsSmartCity(SmartCityProxy SmartCity);

        /// <summary>
        /// Check if the given SmartCity identification is already present within the roaming network.
        /// </summary>
        /// <param name="SmartCityId">The unique identification of the Charging Station Operator.</param>
        Boolean ContainsSmartCity(SmartCity_Id SmartCityId);

        SmartCityProxy GetSmartCityById(SmartCity_Id SmartCityId);

        Boolean TryGetSmartCityById(SmartCity_Id SmartCityId, out SmartCityProxy SmartCity);

        SmartCityProxy RemoveSmartCity(SmartCity_Id SmartCityId);

        Boolean TryRemoveSmartCity(SmartCity_Id SmartCityId, out SmartCityProxy SmartCity);

        #endregion



        JObject ToJSON(Boolean                                                    Embedded                                  = false,
                       InfoStatus                                                 ExpandChargingStationOperatorIds          = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandEMobilityProviderId                 = InfoStatus.ShowIdOnly,
                       CustomJObjectSerializerDelegate<RoamingNetwork>?           CustomRoamingNetworkSerializer            = null,
                       CustomJObjectSerializerDelegate<ChargingStationOperator>?  CustomChargingStationOperatorSerializer   = null,
                       CustomJObjectSerializerDelegate<ChargingPool>?             CustomChargingPoolSerializer              = null,
                       CustomJObjectSerializerDelegate<ChargingStation>?          CustomChargingStationSerializer           = null,
                       CustomJObjectSerializerDelegate<EVSE>?                     CustomEVSESerializer                      = null);

    }

}
