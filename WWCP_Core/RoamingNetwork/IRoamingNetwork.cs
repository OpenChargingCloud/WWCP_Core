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
    public interface IRoamingNetwork : IAdminStatus<RoamingNetworkAdminStatusTypes>,
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

        IEnumerable<ChargingStationOperator>             ChargingStationOperators { get; }
        Boolean                                          ContainsChargingStationOperator   (ChargingStationOperator                  ChargingStationOperator);
        Boolean                                          ContainsChargingStationOperator   (ChargingStationOperator_Id               ChargingStationOperatorId);
        ChargingStationOperator?                         GetChargingStationOperatorById    (ChargingStationOperator_Id               ChargingStationOperatorId);
        ChargingStationOperator?                         GetChargingStationOperatorById    (ChargingStationOperator_Id?              ChargingStationOperatorId);
        Boolean                                          TryGetChargingStationOperatorById (ChargingStationOperator_Id               ChargingStationOperatorId, out ChargingStationOperator? ChargingStationOperator);
        Boolean                                          TryGetChargingStationOperatorById (ChargingStationOperator_Id?              ChargingStationOperatorId, out ChargingStationOperator? ChargingStationOperator);
        ChargingStationOperator?                         RemoveChargingStationOperator     (ChargingStationOperator_Id               ChargingStationOperatorId);
        ChargingStationOperator?                         RemoveChargingStationOperator     (ChargingStationOperator_Id?              ChargingStationOperatorId);
        Boolean                                          TryRemoveChargingStationOperator  (ChargingStationOperator_Id               ChargingStationOperatorId, out ChargingStationOperator? ChargingStationOperator);
        Boolean                                          TryRemoveChargingStationOperator  (ChargingStationOperator_Id?              ChargingStationOperatorId, out ChargingStationOperator? ChargingStationOperator);

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


        ChargingStationOperator CreateChargingStationOperator(ChargingStationOperator_Id                           ChargingStationOperatorId,
                                                              I18NString?                                          Name                                   = null,
                                                              I18NString?                                          Description                            = null,
                                                              Action<ChargingStationOperator>?                     Configurator                           = null,
                                                              RemoteChargingStationOperatorCreatorDelegate?        RemoteChargingStationOperatorCreator   = null,
                                                              ChargingStationOperatorAdminStatusTypes?             InitialAdminStatus                     = null,
                                                              ChargingStationOperatorStatusTypes?                  InitialStatus                          = null,
                                                              Action<ChargingStationOperator>?                     OnSuccess                              = null,
                                                              Action<RoamingNetwork, ChargingStationOperator_Id>?  OnError                                = null);

        #endregion

        #region ChargingPools

        IEnumerable<ChargingPool>                ChargingPools { get; }
        Boolean                                  ContainsChargingPool             (ChargingPool                                  ChargingPool);
        Boolean                                  ContainsChargingPool             (ChargingPool_Id                               ChargingPoolId);
        ChargingPool?                            GetChargingPoolById              (ChargingPool_Id                               ChargingPoolId);
        Boolean                                  TryGetChargingPoolById           (ChargingPool_Id                               ChargingPoolId, out ChargingPool? ChargingPool);
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

        IEnumerable<ChargingStation>             ChargingStations { get; }
        Boolean                                  ContainsChargingStation           (ChargingStation                                  ChargingStation);
        Boolean                                  ContainsChargingStation           (ChargingStation_Id                               ChargingStationId);
        ChargingStation?                         GetChargingStationById            (ChargingStation_Id                               ChargingStationId);
        Boolean                                  TryGetChargingStationById         (ChargingStation_Id                               ChargingStationId, out ChargingStation? ChargingStation);
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
