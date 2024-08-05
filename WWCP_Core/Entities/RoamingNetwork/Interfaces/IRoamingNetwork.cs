/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using social.OpenData.UsersAPI;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    #region E-Mobility Provider Delegates

    /// <summary>
    /// A delegate called whenever an e-mobility provider was added.
    /// </summary>
    /// <param name="Timestamp">The timestamp when the e-mobility provider was added.</param>
    /// <param name="ChargingStationOperator">The added e-mobility provider.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
    public delegate Task OnEMobilityProviderAddedDelegate(DateTime            Timestamp,
                                                          IEMobilityProvider  ChargingStationOperator,
                                                          EventTracking_Id?   EventTrackingId   = null,
                                                          User_Id?            CurrentUserId     = null);

    /// <summary>
    /// A delegate called whenever an e-mobility provider was updated.
    /// </summary>
    /// <param name="Timestamp">The timestamp when the e-mobility provider was updated.</param>
    /// <param name="NewChargingStationOperator">The new/updated e-mobility provider.</param>
    /// <param name="OldChargingStationOperator">The old e-mobility provider.</param>
    /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
    /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
    public delegate Task OnEMobilityProviderUpdatedDelegate(DateTime            Timestamp,
                                                            IEMobilityProvider  NewChargingStationOperator,
                                                            IEMobilityProvider  OldChargingStationOperator,
                                                            EventTracking_Id?   EventTrackingId   = null,
                                                            User_Id?            CurrentUserId     = null);

    /// <summary>
    /// A delegate called whenever an e-mobility provider was removed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when the e-mobility provider was removed.</param>
    /// <param name="ChargingStationOperator">The e-mobility provider to be removed.</param>
    /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
    /// <param name="CurrentChargingStationOperatorId">An optional user identification initiating this command/request.</param>
    public delegate Task OnEMobilityProviderRemovedDelegate(DateTime            Timestamp,
                                                            IEMobilityProvider  ChargingStationOperator,
                                                            EventTracking_Id?   EventTrackingId                    = null,
                                                            User_Id?            CurrentChargingStationOperatorId   = null);

    #endregion

    #region Charging Station Operator Delegates

    /// <summary>
    /// A delegate called whenever a charging station operator was added.
    /// </summary>
    /// <param name="Timestamp">The timestamp when the charging station operator was added.</param>
    /// <param name="ChargingStationOperator">The added charging station operator.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
    public delegate Task OnChargingStationOperatorAddedDelegate(DateTime                  Timestamp,
                                                                IChargingStationOperator  ChargingStationOperator,
                                                                EventTracking_Id?         EventTrackingId   = null,
                                                                User_Id?                  CurrentUserId     = null);

        /// <summary>
    /// A delegate called whenever a charging station operator was updated.
    /// </summary>
    /// <param name="Timestamp">The timestamp when the charging station operator was updated.</param>
    /// <param name="NewChargingStationOperator">The new/updated charging station operator.</param>
    /// <param name="OldChargingStationOperator">The old charging station operator.</param>
    /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
    /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
    public delegate Task OnChargingStationOperatorUpdatedDelegate(DateTime                  Timestamp,
                                                                  IChargingStationOperator  NewChargingStationOperator,
                                                                  IChargingStationOperator  OldChargingStationOperator,
                                                                  EventTracking_Id?         EventTrackingId   = null,
                                                                  User_Id?                  CurrentUserId     = null);

    /// <summary>
    /// A delegate called whenever a charging station operator was removed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when the charging station operator was removed.</param>
    /// <param name="ChargingStationOperator">The charging station operator to be removed.</param>
    /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
    /// <param name="CurrentChargingStationOperatorId">An optional user identification initiating this command/request.</param>
    public delegate Task OnChargingStationOperatorRemovedDelegate(DateTime                  Timestamp,
                                                                  IChargingStationOperator  ChargingStationOperator,
                                                                  EventTracking_Id?         EventTrackingId                    = null,
                                                                  User_Id?                  CurrentChargingStationOperatorId   = null);

    #endregion


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
        public static JArray ToJSON(this IEnumerable<IRoamingNetwork>                           RoamingNetworks,
                                    UInt64?                                                     Skip                                      = null,
                                    UInt64?                                                     Take                                      = null,
                                    Boolean                                                     Embedded                                  = false,
                                    InfoStatus                                                  ExpandChargingStationOperatorIds          = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                  ExpandRoamingNetworkIds                   = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                  ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                  ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                  ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                  ExpandDataLicenses                        = InfoStatus.ShowIdOnly,

                                    InfoStatus                                                  ExpandEMobilityProviderId                 = InfoStatus.ShowIdOnly,

                                    CustomJObjectSerializerDelegate<IRoamingNetwork>?           CustomRoamingNetworkSerializer            = null,
                                    CustomJObjectSerializerDelegate<IChargingStationOperator>?  CustomChargingStationOperatorSerializer   = null,
                                    CustomJObjectSerializerDelegate<IChargingPool>?             CustomChargingPoolSerializer              = null,
                                    CustomJObjectSerializerDelegate<IChargingStation>?          CustomChargingStationSerializer           = null,
                                    CustomJObjectSerializerDelegate<IEVSE>?                     CustomEVSESerializer                      = null)


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


        #region CreateEMobilityProvider      (Id, Name = null, Description = null, Configurator = null, OnCreated = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station operator having the given
        /// unique charging station operator identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new charging station operator.</param>
        /// <param name="Name">The offical (multi-language) name of the charging station operator.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station operator before its successful creation.</param>
        /// <param name="OnCreated">An optional delegate to configure the new charging station operator after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station operator failed.</param>
        public static async Task<AddEMobilityProviderResult>

            CreateEMobilityProvider(this IRoamingNetwork                                             RoamingNetwork,
                                    EMobilityProvider_Id                                             Id,
                                    I18NString?                                                      Name                             = null,
                                    I18NString?                                                      Description                      = null,
                                    Action<IEMobilityProvider>?                                      Configurator                     = null,
                                    RemoteEMobilityProviderCreatorDelegate?                          RemoteEMobilityProviderCreator   = null,
                                    EMobilityProviderAdminStatusTypes?                               InitialAdminStatus               = null,
                                    EMobilityProviderStatusTypes?                                    InitialStatus                    = null,
                                    OnEMobilityProviderAddedDelegate?                                OnCreated                        = null,
                                    //Action<RoamingNetwork, EMobilityProvider_Id, EventTracking_Id>?  OnError                          = null,
                                    EventTracking_Id?                                                EventTrackingId                  = null,
                                    User_Id?                                                         CurrentUserId                    = null)


            => await RoamingNetwork.AddEMobilityProvider(
                         new EMobilityProvider(
                             Id,
                             RoamingNetwork,
                             Name,
                             Description,
                             Configurator,
                             RemoteEMobilityProviderCreator,
                             null,
                             InitialAdminStatus,
                             InitialStatus
                         ),
                         false,
                         OnCreated,
                         EventTrackingId,
                         CurrentUserId
                     );

        #endregion


        #region CreateChargingStationOperator(Id, Name = null, Description = null, Configurator = null, OnCreated = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station operator having the given
        /// unique charging station operator identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new charging station operator.</param>
        /// <param name="Name">The offical (multi-language) name of the charging station operator.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station operator before its successful creation.</param>
        /// <param name="OnCreated">An optional delegate to configure the new charging station operator after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station operator failed.</param>
        public static async Task<AddChargingStationOperatorResult>

            CreateChargingStationOperator(this IRoamingNetwork                                                   RoamingNetwork,
                                          ChargingStationOperator_Id                                             Id,
                                          I18NString?                                                            Name                                   = null,
                                          I18NString?                                                            Description                            = null,
                                          Action<IChargingStationOperator>?                                      Configurator                           = null,
                                          RemoteChargingStationOperatorCreatorDelegate?                          RemoteChargingStationOperatorCreator   = null,
                                          ChargingStationOperatorAdminStatusTypes?                               InitialAdminStatus                     = null,
                                          ChargingStationOperatorStatusTypes?                                    InitialStatus                          = null,
                                          OnChargingStationOperatorAddedDelegate?                                OnCreated                              = null,
                                          //Action<RoamingNetwork, ChargingStationOperator_Id, EventTracking_Id>?  OnError                                = null,
                                          EventTracking_Id?                                                      EventTrackingId                        = null,
                                          User_Id?                                                               CurrentUserId                          = null)


            => await RoamingNetwork.AddChargingStationOperator(
                         new ChargingStationOperator(
                             Id,
                             RoamingNetwork,
                             Name,
                             Description,
                             Configurator,
                             RemoteChargingStationOperatorCreator,
                             InitialAdminStatus,
                             InitialStatus
                         ),
                         false,
                         OnCreated,
                         EventTrackingId,
                         CurrentUserId
                     );

        #endregion


    }


    /// <summary>
    /// The common interface of all roaming networks.
    /// </summary>
    public interface IRoamingNetwork : IEntity<RoamingNetwork_Id>,
                                       IAdminStatus<RoamingNetworkAdminStatusTypes>,
                                       IStatus<RoamingNetworkStatusTypes>,
                                       ISendAuthorizeStartStop,
                                       IAuthorizeStartStopCache,
                                       IRemoteStartStop,
                                       IChargingReservations,
                                       IChargingSessions,
                                       IChargeDetailRecords,

                                       IReceiveChargeDetailRecords,
                                       ISendChargeDetailRecords,

                                       IEquatable<RoamingNetwork>,
                                       IComparable<RoamingNetwork>,
                                       IComparable
    {

        /// <summary>
        /// The unique roaming network identification.
        /// </summary>
        new RoamingNetwork_Id         Id                          { get; }

        ReactiveSet<OpenDataLicense>  DataLicenses                { get; }



        ChargingReservationsStore     ReservationsStore           { get; }
        ChargingSessionsStore         SessionsStore               { get; }


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

        IEMPRoamingProvider                              CreateEMPRoamingProvider    (IEMPRoamingProvider           EMPRoamingProvider,
                                                                                      Action<IEMPRoamingProvider>?  Configurator = null);

        #endregion

        #region EMobilityProviders

        Task<AddEMobilityProviderResult>           AddEMobilityProvider           (IEMobilityProvider                   EMobilityProvider,
                                                                                   Boolean                              SkipNewEMobilityProviderNotifications       = false,
                                                                                   OnEMobilityProviderAddedDelegate?    OnAdded                                     = null,
                                                                                   EventTracking_Id?                    EventTrackingId                             = null,
                                                                                   User_Id?                             CurrentUserId                               = null);


        IEnumerable<IEMobilityProvider>            EMobilityProviders          { get; }
        Boolean                                    ContainsEMobilityProvider   (IEMobilityProvider                 EMobilityProvider);
        Boolean                                    ContainsEMobilityProvider   (EMobilityProvider_Id               EMobilityProviderId);
        IEMobilityProvider?                        GetEMobilityProviderById    (EMobilityProvider_Id               EMobilityProviderId);
        IEMobilityProvider?                        GetEMobilityProviderById    (EMobilityProvider_Id?              EMobilityProviderId);
        Boolean                                    TryGetEMobilityProviderById (EMobilityProvider_Id               EMobilityProviderId, [NotNullWhen(true)] out IEMobilityProvider? EMobilityProvider);
        Boolean                                    TryGetEMobilityProviderById (EMobilityProvider_Id?              EMobilityProviderId, [NotNullWhen(true)] out IEMobilityProvider? EMobilityProvider);
        IEMobilityProvider?                        RemoveEMobilityProvider     (EMobilityProvider_Id               EMobilityProviderId);
        IEMobilityProvider?                        RemoveEMobilityProvider     (EMobilityProvider_Id?              EMobilityProviderId);
        Boolean                                    TryRemoveEMobilityProvider  (EMobilityProvider_Id               EMobilityProviderId, [NotNullWhen(true)] out IEMobilityProvider? EMobilityProvider);
        Boolean                                    TryRemoveEMobilityProvider  (EMobilityProvider_Id?              EMobilityProviderId, [NotNullWhen(true)] out IEMobilityProvider? EMobilityProvider);

        IEnumerable<EMobilityProvider_Id>          EMobilityProviderIds        (IncludeEMobilityProviderDelegate?  IncludeEMobilityProvider   = null);
        IEnumerable<EMobilityProviderAdminStatus>  EMobilityProviderAdminStatus(IncludeEMobilityProviderDelegate?  IncludeEMobilityProvider   = null);
        IEnumerable<EMobilityProviderStatus>       EMobilityProviderStatus     (IncludeEMobilityProviderDelegate?  IncludeEMobilityProvider   = null);

        //EMobilityProvider?  CreateEMobilityProvider(EMobilityProvider_Id                           ProviderId,
        //                                            I18NString?                                    Name                             = null,
        //                                            I18NString?                                    Description                      = null,
        //                                            eMobilityProviderPriority?                     Priority                         = null,
        //                                            Action<EMobilityProvider>?                     Configurator                     = null,
        //                                            RemoteEMobilityProviderCreatorDelegate?        RemoteEMobilityProviderCreator   = null,
        //                                            EMobilityProviderAdminStatusTypes?             InitialAdminStatus               = null,
        //                                            EMobilityProviderStatusTypes?                  InitialStatus                    = null,
        //                                            Action<EMobilityProvider>?                     OnSuccess                        = null,
        //                                            Action<RoamingNetwork, EMobilityProvider_Id>?  OnError                          = null);

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

        Task<AddChargingStationOperatorResult>            AddChargingStationOperator           (IChargingStationOperator                   ChargingStationOperator,
                                                                                                Boolean                                    SkipNewChargingStationOperatorNotifications       = false,
                                                                                                OnChargingStationOperatorAddedDelegate?    OnAdded                                           = null,
                                                                                                EventTracking_Id?                          EventTrackingId                                   = null,
                                                                                                User_Id?                                   CurrentUserId                                     = null);

        Task<AddChargingStationOperatorResult>            AddChargingStationOperatorIfNotExists(ChargingStationOperator                    ChargingStationOperator,
                                                                                                Boolean                                    SkipNewUserNotifications                          = false,
                                                                                                OnChargingStationOperatorAddedDelegate?    OnAdded                                           = null,
                                                                                                EventTracking_Id?                          EventTrackingId                                   = null,
                                                                                                User_Id?                                   CurrentUserId                                     = null);

        Task<AddOrUpdateChargingStationOperatorResult>    AddOrUpdateChargingStationOperator   (ChargingStationOperator                    ChargingStationOperator,
                                                                                                Boolean                                    SkipNewChargingStationOperatorNotifications       = false,
                                                                                                Boolean                                    SkipChargingStationOperatorUpdatedNotifications   = false,
                                                                                                OnChargingStationOperatorAddedDelegate?    OnAdded                                           = null,
                                                                                                OnChargingStationOperatorUpdatedDelegate?  OnUpdated                                         = null,
                                                                                                EventTracking_Id?                          EventTrackingId                                   = null,
                                                                                                User_Id?                                   CurrentUserId                                     = null);

        Task<UpdateChargingStationOperatorResult>         UpdateChargingStationOperator        (ChargingStationOperator                    ChargingStationOperator,
                                                                                                Boolean                                    SkipChargingStationOperatorUpdatedNotifications   = false,
                                                                                                OnChargingStationOperatorUpdatedDelegate?  OnUpdated                                         = null,
                                                                                                EventTracking_Id?                          EventTrackingId                                   = null,
                                                                                                User_Id?                                   CurrentUserId                                     = null);

        Task<UpdateChargingStationOperatorResult>         UpdateChargingStationOperator        (ChargingStationOperator                    ChargingStationOperator,
                                                                                                Action<ChargingStationOperator.Builder>    UpdateDelegate,
                                                                                                Boolean                                    SkipChargingStationOperatorUpdatedNotifications   = false,
                                                                                                OnChargingStationOperatorUpdatedDelegate?  OnUpdated                                         = null,
                                                                                                EventTracking_Id?                          EventTrackingId                                   = null,
                                                                                                User_Id?                                   CurrentUserId                                     = null);

        Task<DeleteChargingStationOperatorResult>         RemoveChargingStationOperator        (ChargingStationOperator                    ChargingStationOperator,
                                                                                                Boolean                                    SkipChargingStationOperatorRemovedNotifications   = false,
                                                                                                OnChargingStationOperatorRemovedDelegate?  OnRemoved                                         = null,
                                                                                                EventTracking_Id?                          EventTrackingId                                   = null,
                                                                                                User_Id?                                   CurrentUserId                                     = null);


        IEnumerable<IChargingStationOperator>             ChargingStationOperators             { get; }
        Boolean                                           ChargingStationOperatorExists        (IChargingStationOperator                   ChargingStationOperator);
        Boolean                                           ChargingStationOperatorExists        (ChargingStationOperator_Id                 ChargingStationOperatorId);
        IChargingStationOperator?                         GetChargingStationOperatorById       (ChargingStationOperator_Id                 ChargingStationOperatorId);
        IChargingStationOperator?                         GetChargingStationOperatorById       (ChargingStationOperator_Id?                ChargingStationOperatorId);
        Boolean                                           TryGetChargingStationOperatorById    (ChargingStationOperator_Id                 ChargingStationOperatorId, [NotNullWhen(true)] out IChargingStationOperator? ChargingStationOperator);
        Boolean                                           TryGetChargingStationOperatorById    (ChargingStationOperator_Id?                ChargingStationOperatorId, [NotNullWhen(true)] out IChargingStationOperator? ChargingStationOperator);


        IEnumerable<ChargingStationOperator_Id>           ChargingStationOperatorIds           (IncludeChargingStationOperatorDelegate?    IncludeChargingStationOperator   = null);
        IEnumerable<ChargingStationOperatorAdminStatus>   ChargingStationOperatorAdminStatus   (IncludeChargingStationOperatorDelegate?    IncludeChargingStationOperator   = null);
        IEnumerable<ChargingStationOperatorStatus>        ChargingStationOperatorStatus        (IncludeChargingStationOperatorDelegate?    IncludeChargingStationOperator   = null);

        #endregion

        #region ChargingPools

        IEnumerable<IChargingPool>               ChargingPools                    { get; }
        Boolean                                  ContainsChargingPool             (IChargingPool                                 ChargingPool);
        Boolean                                  ContainsChargingPool             (ChargingPool_Id                               ChargingPoolId);
        IChargingPool?                           GetChargingPoolById              (ChargingPool_Id                               ChargingPoolId);
        IChargingPool?                           GetChargingPoolById              (ChargingPool_Id?                              ChargingPoolId);
        Boolean                                  TryGetChargingPoolById           (ChargingPool_Id                               ChargingPoolId, [NotNullWhen(true)] out IChargingPool? ChargingPool);
        Boolean                                  TryGetChargingPoolById           (ChargingPool_Id?                              ChargingPoolId, [NotNullWhen(true)] out IChargingPool? ChargingPool);

        IEnumerable<ChargingPool_Id>             ChargingPoolIds                  (IncludeChargingPoolDelegate?                  IncludeChargingPools   = null);
        IEnumerable<ChargingPoolAdminStatus>     ChargingPoolAdminStatus          (IncludeChargingPoolDelegate?                  IncludeChargingPools   = null);
        IEnumerable<ChargingPoolStatus>          ChargingPoolStatus               (IncludeChargingPoolDelegate?                  IncludeChargingPools   = null);


        IEnumerable<Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>>>
                                                 ChargingPoolAdminStatusSchedule  (IncludeChargingPoolDelegate?                  IncludeChargingPools   = null,
                                                                                   Func<DateTime,                     Boolean>?  TimestampFilter        = null,
                                                                                   Func<ChargingPoolAdminStatusTypes, Boolean>?  AdminStatusFilter      = null,
                                                                                   UInt64?                                       Skip                   = null,
                                                                                   UInt64?                                       Take                   = null);

        IEnumerable<Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusTypes>>>>
                                                 ChargingPoolStatusSchedule       (IncludeChargingPoolDelegate?                  IncludeChargingPools   = null,
                                                                                   Func<DateTime,                Boolean>?       TimestampFilter        = null,
                                                                                   Func<ChargingPoolStatusTypes, Boolean>?       AdminStatusFilter      = null,
                                                                                   UInt64?                                       Skip                   = null,
                                                                                   UInt64?                                       Take                   = null);

        #endregion

        #region ChargingStations

        IEnumerable<IChargingStation>            ChargingStations                  { get; }
        Boolean                                  ContainsChargingStation           (IChargingStation                                 ChargingStation);
        Boolean                                  ContainsChargingStation           (ChargingStation_Id                               ChargingStationId);
        IChargingStation?                        GetChargingStationById            (ChargingStation_Id                               ChargingStationId);
        IChargingStation?                        GetChargingStationById            (ChargingStation_Id?                              ChargingStationId);
        Boolean                                  TryGetChargingStationById         (ChargingStation_Id                               ChargingStationId, [NotNullWhen(true)] out IChargingStation? ChargingStation);
        Boolean                                  TryGetChargingStationById         (ChargingStation_Id?                              ChargingStationId, [NotNullWhen(true)] out IChargingStation? ChargingStation);

        IEnumerable<ChargingStation_Id>          ChargingStationIds                (IncludeChargingStationDelegate?                  IncludeChargingStations   = null);
        IEnumerable<ChargingStationAdminStatus>  ChargingStationAdminStatus        (IncludeChargingStationDelegate?                  IncludeChargingStations   = null);
        IEnumerable<ChargingStationStatus>       ChargingStationStatus             (IncludeChargingStationDelegate?                  IncludeChargingStations   = null);


        IEnumerable<Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>>>
                                                 ChargingStationAdminStatusSchedule(IncludeChargingStationDelegate?                  IncludeChargingStations   = null,
                                                                                    Func<DateTime,                        Boolean>?  TimestampFilter           = null,
                                                                                    Func<ChargingStationAdminStatusTypes, Boolean>?  AdminStatusFilter         = null,
                                                                                    UInt64?                                          Skip                      = null,
                                                                                    UInt64?                                          Take                      = null);

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

        IEnumerable<IEVSE>                       EVSEs                  { get; }

        Boolean                                  ContainsEVSE           (IEVSE                                 EVSE);

        Boolean                                  ContainsEVSE           (EVSE_Id                               EVSEId);

        IEVSE?                                   GetEVSEById            (EVSE_Id                               EVSEId);
        IEVSE?                                   GetEVSEById            (EVSE_Id?                              EVSEId);
        Boolean                                  TryGetEVSEById         (EVSE_Id                               EVSEId, [NotNullWhen(true)] out IEVSE? EVSE);
        Boolean                                  TryGetEVSEById         (EVSE_Id?                              EVSEId, [NotNullWhen(true)] out IEVSE? EVSE);

        IEnumerable<EVSE_Id>                     EVSEIds                (IncludeEVSEDelegate?                  IncludeEVSEs        = null);
        IEnumerable<EVSEAdminStatus>             EVSEAdminStatus        (IncludeEVSEDelegate?                  IncludeEVSEs        = null);
        IEnumerable<EVSEStatus>                  EVSEStatus             (IncludeEVSEDelegate?                  IncludeEVSEs        = null);


        IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>>
                                                 EVSEAdminStatusSchedule(IncludeEVSEDelegate?                  IncludeEVSEs        = null,
                                                                         Func<DateTime,             Boolean>?  TimestampFilter     = null,
                                                                         Func<EVSEAdminStatusTypes, Boolean>?  AdminStatusFilter   = null,
                                                                         UInt64?                               Skip                = null,
                                                                         UInt64?                               Take                = null);

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

        #region EnergyMeters

        IEnumerable<IEnergyMeter>                EnergyMeters          { get; }

        Boolean                                  ContainsEnergyMeter   (IEnergyMeter                 EnergyMeter);

        Boolean                                  ContainsEnergyMeter   (EnergyMeter_Id               EnergyMeterId);

        IEnergyMeter?                            GetEnergyMeterById    (EnergyMeter_Id               EnergyMeterId);
        IEnergyMeter?                            GetEnergyMeterById    (EnergyMeter_Id?              EnergyMeterId);
        Boolean                                  TryGetEnergyMeterById (EnergyMeter_Id               EnergyMeterId, [NotNullWhen(true)] out IEnergyMeter? EnergyMeter);
        Boolean                                  TryGetEnergyMeterById (EnergyMeter_Id?              EnergyMeterId, [NotNullWhen(true)] out IEnergyMeter? EnergyMeter);

        IEnumerable<EnergyMeter_Id>              EnergyMeterIds        (IncludeEnergyMeterDelegate?  IncludeEnergyMeters        = null);

        #endregion

        #region Charging Sessions

        Task ReceiveSendChargeDetailRecordResult (SendCDRResult SendCDRResult);

        Task ReceiveSendChargeDetailRecordResults(IEnumerable<SendCDRResult> SendCDRResults);


        Task<Boolean> RegisterExternalChargingSession(DateTime         Timestamp,
                                                      Object           Sender,
                                                      String           Command,
                                                      ChargingSession  ChargingSession,
                                                      DateTime?        NoAutoDeletionBefore   = null);

        #endregion


        #region Parking Operators...

        /// <summary>
        /// Return all parking operators registered within this roaming network.
        /// </summary>
        IEnumerable<ParkingOperator> ParkingOperators { get; }

        /// <summary>
        /// Called whenever a parking operator will be or was added.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, ParkingOperator, Boolean> OnParkingOperatorAddition { get; }

        /// <summary>
        /// Called whenever a parking operator will be or was removed.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, ParkingOperator, Boolean> OnParkingOperatorRemoval  { get; }


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
        IEnumerable<IGridOperator> GridOperators { get; }


        /// <summary>
        /// Called whenever an EVServiceProvider will be or was added.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, IGridOperator, Boolean> OnGridOperatorAddition { get; }

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, IGridOperator, Boolean> OnGridOperatorRemoval  { get; }

        /// <summary>
        /// Return the admin status of all smart cities registered within this roaming network.
        /// </summary>
        IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorAdminStatusTypes>>>> GridOperatorsAdminStatus { get; }

        /// <summary>
        /// Return the status of all smart cities registered within this roaming network.
        /// </summary>
        IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorStatusTypes>>>> GridOperatorsStatus { get; }



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
        GridOperator CreateNewGridOperator(GridOperator_Id                           GridOperatorId,
                                           I18NString?                               Name                        = null,
                                           I18NString?                               Description                 = null,
                                           GridOperatorPriority?                     Priority                    = null,
                                           GridOperatorAdminStatusTypes              AdminStatus                 = GridOperatorAdminStatusTypes.Available,
                                           GridOperatorStatusTypes                   Status                      = GridOperatorStatusTypes.Available,
                                           Action<GridOperator>?                     Configurator                = null,
                                           Action<GridOperator>?                     OnSuccess                   = null,
                                           Action<RoamingNetwork, GridOperator_Id>?  OnError                     = null,
                                           RemoteGridOperatorCreatorDelegate?        RemoteGridOperatorCreator   = null);

        /// <summary>
        /// Check if the given GridOperator is already present within the roaming network.
        /// </summary>
        /// <param name="GridOperator">An Charging Station Operator.</param>
        Boolean ContainsGridOperator(IGridOperator GridOperator);

        /// <summary>
        /// Check if the given GridOperator identification is already present within the roaming network.
        /// </summary>
        /// <param name="GridOperatorId">The unique identification of the Charging Station Operator.</param>
        Boolean ContainsGridOperator(GridOperator_Id GridOperatorId);

        IGridOperator? GetGridOperatorById(GridOperator_Id GridOperatorId);

        Boolean TryGetGridOperatorById(GridOperator_Id GridOperatorId, [NotNullWhen(true)] out IGridOperator? GridOperator);

        IGridOperator? RemoveGridOperator(GridOperator_Id GridOperatorId);

        Boolean TryRemoveGridOperator(GridOperator_Id GridOperatorId, [NotNullWhen(true)] out IGridOperator? GridOperator);

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
        IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, SmartCityProxy, Boolean> OnSmartCityAddition { get; }

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, SmartCityProxy, Boolean> OnSmartCityRemoval  { get; }



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


        /// <summary>
        /// Return a JSON representation of the given roaming network.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network API.</param>
        JObject ToJSON(Boolean                                                     Embedded                                  = false,
                       InfoStatus                                                  ExpandChargingStationOperatorIds          = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandEMobilityProviderId                 = InfoStatus.ShowIdOnly,
                       CustomJObjectSerializerDelegate<IRoamingNetwork>?           CustomRoamingNetworkSerializer            = null,
                       CustomJObjectSerializerDelegate<IChargingStationOperator>?  CustomChargingStationOperatorSerializer   = null,
                       CustomJObjectSerializerDelegate<IChargingPool>?             CustomChargingPoolSerializer              = null,
                       CustomJObjectSerializerDelegate<IChargingStation>?          CustomChargingStationSerializer           = null,
                       CustomJObjectSerializerDelegate<IEVSE>?                     CustomEVSESerializer                      = null);


    }

}
