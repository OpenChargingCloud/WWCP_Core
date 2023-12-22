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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Styx;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using cloud.charging.open.protocols.WWCP.Networking;

using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering roaming networks.
    /// </summary>
    /// <param name="RoamingNetwork">A roaming network to include.</param>
    public delegate Boolean IncludeRoamingNetworkDelegate(RoamingNetwork RoamingNetwork);


    /// <summary>
    /// Extension methods for the roaming networks.
    /// </summary>
    public static class RoamingNetworkExtensions
    {

        #region ToJSON(this RoamingNetworks, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given roaming networks collection.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="Skip">The optional number of roaming networks to skip.</param>
        /// <param name="Take">The optional number of roaming networks to return.</param>
        public static JArray ToJSON(this IEnumerable<IRoamingNetwork>                           RoamingNetworks,
                                    Boolean                                                     Embedded                                  = false,
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
                                    CustomJObjectSerializerDelegate<IEVSE>?                     CustomEVSESerializer                      = null,
                                    UInt64?                                                     Skip                                      = null,
                                    UInt64?                                                     Take                                      = null)
        {

            #region Initial checks

            if (RoamingNetworks is null || !RoamingNetworks.Any())
                return new JArray();

            #endregion

            return new JArray(RoamingNetworks.
                                  SkipTakeFilter(Skip, Take).
                                  Select        (roamingNetwork => roamingNetwork.ToJSON(Embedded,
                                                                                         ExpandChargingStationOperatorIds,
                                                                                         ExpandChargingPoolIds,
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

        }

        #endregion

    }


    /// <summary>
    /// A Electric Vehicle Roaming Network is a service abstraction to allow multiple
    /// independent roaming services to be delivered over the same infrastructure.
    /// This can e.g. be a differentation of service levels (premiun, basic,
    /// discount) or allow a simplified testing (production, qa, featureX, ...)
    /// </summary>
    public class RoamingNetwork : AEMobilityEntity<RoamingNetwork_Id,
                                                   RoamingNetworkAdminStatusTypes,
                                                   RoamingNetworkStatusTypes>,
                                  IRoamingNetwork
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const               String                                             JSONLDContext                          = "https://open.charging.cloud/contexts/wwcp+json/roamingNetwork";

        protected static readonly  SemaphoreSlim                                      eMobilityProvidersSemaphore            = new (1, 1);
        protected static readonly  SemaphoreSlim                                      chargingStationOperatorsSemaphore      = new (1, 1);

        protected static readonly  TimeSpan                                           SemaphoreSlimTimeout                   = TimeSpan.FromSeconds(5);

        protected static readonly  Byte                                               MinEMobilityProviderIdLength           = 5;
        protected static readonly  Byte                                               MinEMobilityProviderNameLength         = 5;

        protected static readonly  Byte                                               MinChargingStationOperatorIdLength     = 5;
        protected static readonly  Byte                                               MinChargingStationOperatorNameLength   = 5;


        private readonly           PriorityList<ISendRoamingNetworkData>              allSendRoamingNetworkData              = new ();
        private readonly           PriorityList<ISendChargingStationOperatorData>     allSendChargingStationOperatorData     = new ();
        private readonly           PriorityList<ISendChargingPoolData>                allSendChargingPoolData                = new ();
        private readonly           PriorityList<ISendChargingStationData>             allSendChargingStationData             = new ();
        private readonly           PriorityList<ISendEVSEData>                        allSendEVSEData                        = new ();

        private readonly           PriorityList<ISendAdminStatus>                     allSendAdminStatus                     = new ();
        private readonly           PriorityList<ISendStatus>                          allSendStatus                          = new ();
        private readonly           PriorityList<ISendEnergyStatus>                    allSendEnergyStatus                    = new ();

        private readonly           PriorityList<ISendAuthorizeStartStop>              allSend2RemoteAuthorizeStartStop       = new ();
        private readonly           PriorityList<ISendChargeDetailRecords>             allRemoteSendChargeDetailRecord        = new ();

        private readonly           ConcurrentDictionary<UInt32, IEMPRoamingProvider>  eMobilityRoamingServices               = new ();
        private readonly           ConcurrentDictionary<UInt32, ICSORoamingProvider>  csoRoamingServices                     = new ();

        #endregion

        #region Properties

        IId IAuthorizeStartStop.AuthId
            => Id;

        IId ISendChargeDetailRecords.SendChargeDetailRecordsId
            => Id;

        public Boolean                                    DisableSendChargeDetailRecords               { get; set; }

        public Boolean                                    DisableNetworkSync                           { get; set; }


        public ChargingReservationsStore                  ReservationsStore                            { get; }
        public ChargingSessionsStore                      SessionsStore                                { get; }


        /// <summary>
        /// A delegate for filtering charge detail records.
        /// </summary>
        public ChargeDetailRecordFilterDelegate?          ChargeDetailRecordFilter                     { get; }


        #region Authentication

        public Boolean                                    DisableAuthentication                        { get; set; }

        #region Authentication cache

        /// <summary>
        /// Whether to disable authentication cache.
        /// </summary>
        public Boolean                                    DisableAuthenticationCache                   { get; set; }

        /// <summary>
        /// The timeout of cached authentication elements.
        /// </summary>
        public TimeSpan                                   AuthenticationCacheTimeout                   { get; set; }

        /// <summary>
        /// The maximum number of AuthStartResults stored within the authentication cache.
        /// </summary>
        public UInt32                                     MaxAuthStartResultCacheElements              { get; set; }

        /// <summary>
        /// The maximum number of AuthStartResults stored within the authentication cache.
        /// </summary>
        public UInt32                                     MaxAuthStopResultCacheElements               { get; set; }

        public HashSet<AuthenticationToken>               InvalidAuthenticationTokens                  { get; }       = new HashSet<AuthenticationToken>();
        public HashSet<AuthenticationToken>               DoNotCacheAuthenticationTokens               { get; }       = new HashSet<AuthenticationToken>();

        #endregion

        #region Authentication rate limit

        /// <summary>
        /// Whether to disable authentication rate limiting.
        /// </summary>
        public Boolean                                    DisableAuthenticationRateLimit                { get; set; }

        /// <summary>
        /// The timeout of rate limited authentications.
        /// </summary>
        public TimeSpan                                   AuthenticationRateLimitTimeSpan               { get; set; }

        /// <summary>
        /// The number of authentications per charging location before rate limits apply.
        /// </summary>
        public UInt16                                     AuthenticationRateLimitPerChargingLocation    { get; set; }

        #endregion

        #endregion

        #region Crypto

        /// <summary>
        /// A delegate to sign a charging station.
        /// </summary>
        public ChargingStationSignatureDelegate?          ChargingStationSignatureGenerator            { get; }

        /// <summary>
        /// A delegate to sign a charging pool.
        /// </summary>
        public ChargingPoolSignatureDelegate?             ChargingPoolSignatureGenerator               { get; }

        /// <summary>
        /// A delegate to sign a charging station operator.
        /// </summary>
        public ChargingStationOperatorSignatureDelegate?  ChargingStationOperatorSignatureGenerator    { get; }

        #endregion

        public String                                     LoggingPath                                  { get; }

        #region Data licenses

        private ReactiveSet<OpenDataLicense> dataLicenses;

        /// <summary>
        /// The license of the roaming network data.
        /// </summary>
        [Mandatory]
        public ReactiveSet<OpenDataLicense> DataLicenses
            => dataLicenses;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network having the given unique roaming network identification.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Name">The multi-language name of the roaming network.</param>
        /// <param name="Description">An optional multi-language description of the roaming network.</param>
        /// <param name="InitialAdminStatus">The initial admin status of the roaming network.</param>
        /// <param name="InitialStatus">The initial status of the roaming network.</param>
        /// <param name="MaxAdminStatusScheduleSize">The maximum number of entries in the admin status history.</param>
        /// <param name="MaxStatusScheduleSize">The maximum number of entries in the status history.</param>
        /// 
        /// <param name="DisableAuthenticationCache">Whether to disable authentication cache.</param>
        /// <param name="AuthenticationCacheTimeout">The timeout of cached authentication elements.</param>
        /// <param name="MaxAuthStartResultCacheElements">The maximum number of AuthStartResults stored within the authentication cache.</param>
        /// <param name="MaxAuthStopResultCacheElements">The maximum number of AuthStartResults stored within the authentication cache.</param>
        /// 
        /// <param name="DisableAuthenticationRateLimit">Whether to disable authentication rate limiting.</param>
        /// <param name="AuthenticationRateLimitTimeSpan">The timeout of rate limited authentications.</param>
        /// <param name="AuthenticationRateLimitPerChargingLocation">The number of authentications per charging location before rate limits apply.</param>
        /// 
        /// <param name="ChargingStationSignatureGenerator">A delegate to sign a charging station.</param>
        /// <param name="ChargingPoolSignatureGenerator">A delegate to sign a charging pool.</param>
        /// <param name="ChargingStationOperatorSignatureGenerator">A delegate to sign a charging station operator.</param>
        public RoamingNetwork(RoamingNetwork_Id                          Id,
                              I18NString?                                Name                                         = null,
                              I18NString?                                Description                                  = null,
                              RoamingNetworkAdminStatusTypes?            InitialAdminStatus                           = null,
                              RoamingNetworkStatusTypes?                 InitialStatus                                = null,
                              UInt16?                                    MaxAdminStatusScheduleSize                   = null,
                              UInt16?                                    MaxStatusScheduleSize                        = null,

                              Boolean?                                   DisableAuthenticationCache                   = false,
                              TimeSpan?                                  AuthenticationCacheTimeout                   = null,
                              UInt32?                                    MaxAuthStartResultCacheElements              = null,
                              UInt32?                                    MaxAuthStopResultCacheElements               = null,

                              Boolean?                                   DisableAuthenticationRateLimit               = true,
                              TimeSpan?                                  AuthenticationRateLimitTimeSpan              = null,
                              UInt16?                                    AuthenticationRateLimitPerChargingLocation   = null,

                              ChargingStationSignatureDelegate?          ChargingStationSignatureGenerator            = null,
                              ChargingPoolSignatureDelegate?             ChargingPoolSignatureGenerator               = null,
                              ChargingStationOperatorSignatureDelegate?  ChargingStationOperatorSignatureGenerator    = null,

                              IEnumerable<RoamingNetworkInfo>?           RoamingNetworkInfos                          = null,
                              Boolean                                    DisableNetworkSync                           = true,
                              String?                                    LoggingPath                                  = null,

                              String?                                    DataSource                                   = null,
                              DateTime?                                  LastChange                                   = null,

                              JObject?                                   CustomData                                   = null,
                              UserDefinedDictionary?                     InternalData                                 = null)

            : base(Id,
                   Name                       ?? new I18NString(Languages.en, "RNTest1"),
                   Description                ?? new I18NString(Languages.en, "A roaming network for testing purposes"),
                   InitialAdminStatus         ?? RoamingNetworkAdminStatusTypes.Operational,
                   InitialStatus              ?? RoamingNetworkStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Init data and properties

            this.dataLicenses                                = new ReactiveSet<OpenDataLicense>();

            this.eMobilityProviders                          = new EntityHashSet       <IRoamingNetwork,       EMobilityProvider_Id,       IEMobilityProvider>      (this);

            this.chargingStationOperators                    = new EntityHashSet       <IRoamingNetwork,       ChargingStationOperator_Id, IChargingStationOperator>(this);

            this.parkingOperators                            = new EntityHashSet       <RoamingNetwork,        ParkingOperator_Id,         ParkingOperator>         (this);
            this.smartCities                                 = new EntityHashSet       <RoamingNetwork,        SmartCity_Id,               SmartCityProxy>          (this);
            this.navigationProviders                         = new EntityHashSet       <RoamingNetwork,        NavigationProvider_Id,      NavigationProvider>      (this);
            this.gridOperators                               = new EntityHashSet       <RoamingNetwork,        GridOperator_Id,            GridOperator>            (this);

            //this._PushEVSEDataToOperatorRoamingServices      = new ConcurrentDictionary<UInt32, IPushData>();
            //this._PushEVSEStatusToOperatorRoamingServices    = new ConcurrentDictionary<UInt32, IPushStatus>();

            this.LoggingPath                                 = LoggingPath ?? AppContext.BaseDirectory;
            Directory.CreateDirectory(this.LoggingPath);

            this.DisableNetworkSync                          = DisableNetworkSync;

            this.ReservationsStore                           = new ChargingReservationsStore(this.Id,
                                                                                             DisableNetworkSync:   true,
                                                                                             LoggingPath:          this.LoggingPath);

            this.SessionsStore                               = new ChargingSessionsStore    (this,
                                                                                             ReloadDataOnStart:    false,
                                                                                             RoamingNetworkInfos:  RoamingNetworkInfos,
                                                                                             DisableNetworkSync:   DisableNetworkSync,
                                                                                             LoggingPath:          this.LoggingPath);

            this.ChargingStationSignatureGenerator           = ChargingStationSignatureGenerator;
            this.ChargingPoolSignatureGenerator              = ChargingPoolSignatureGenerator;
            this.ChargingStationOperatorSignatureGenerator   = ChargingStationOperatorSignatureGenerator;

            #endregion

            #region Init authentication cache

            this.DisableAuthenticationCache                  = DisableAuthenticationCache                 ?? false;
            this.AuthenticationCacheTimeout                  = AuthenticationCacheTimeout                 ?? TimeSpan.FromHours(1);
            this.MaxAuthStartResultCacheElements             = MaxAuthStartResultCacheElements            ?? 2000;
            this.MaxAuthStopResultCacheElements              = MaxAuthStopResultCacheElements             ?? 1000;

            this.InvalidAuthenticationTokens.Add(AuthenticationToken.Parse("00000000"));
            this.InvalidAuthenticationTokens.Add(AuthenticationToken.Parse("00000000000000"));

            #endregion

            #region Init authentication rate limit

            this.DisableAuthenticationRateLimit              = DisableAuthenticationRateLimit             ?? true;
            this.AuthenticationRateLimitTimeSpan             = AuthenticationRateLimitTimeSpan            ?? TimeSpan.FromMinutes(5);
            this.AuthenticationRateLimitPerChargingLocation  = AuthenticationRateLimitPerChargingLocation ?? 10;

            #endregion

            #region Init events

            chargingStationOperators.OnAddition.OnNotification += SendChargingStationOperatorAdded;
            chargingStationOperators.OnRemoval. OnNotification += SendChargingStationOperatorRemoved;

            // RoamingNetwork events

            this.EMPRoamingProviderAddition  = new VotingNotificator<RoamingNetwork, IEMPRoamingProvider, Boolean>(() => new VetoVote(), true);
            this.EMPRoamingProviderRemoval   = new VotingNotificator<RoamingNetwork, IEMPRoamingProvider, Boolean>(() => new VetoVote(), true);

            this.CSORoamingProviderAddition  = new VotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean>(() => new VetoVote(), true);
            this.CSORoamingProviderRemoval   = new VotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean>(() => new VetoVote(), true);

            // cso events
            this.ChargingPoolAddition        = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval         = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, Boolean>(() => new VetoVote(), true);

            // ChargingPool events
            this.ChargingStationAddition     = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval      = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition                = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval                 = new VotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events

            #endregion

            this.OnPropertyChanged += UpdateData;

        }

        #endregion


        #region Data/(Admin-)Status management

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnRoamingNetworkStatusChangedDelegate?       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnRoamingNetworkAdminStatusChangedDelegate?  OnAdminStatusChanged;



        #region (internal) UpdateData(Timestamp, EventTrackingId, Sender, PropertyName, NewValue, OldValue, DataSource)

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnRoamingNetworkDataChangedDelegate? OnDataChanged;


        /// <summary>
        /// Update the static data of the roaming network.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="Sender">The changed roaming network.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The optional old value of the changed property.</param>
        /// <param name="DataSource">An optional data source or context for the roaming network data update.</param>
        internal async Task UpdateData(DateTime          Timestamp,
                                       EventTracking_Id  EventTrackingId,
                                       Object            Sender,
                                       String?           PropertyName,
                                       Object?           NewValue,
                                       Object?           OldValue     = null,
                                       Context?          DataSource   = null)
        {

            var onDataChanged = OnDataChanged;
            if (onDataChanged is not null)
                await onDataChanged(Timestamp,
                                    EventTrackingId,
                                    Sender as RoamingNetwork,
                                    PropertyName,
                                    NewValue,
                                    OldValue,
                                    DataSource);

        }

        #endregion

        #endregion


        #region EMP Roaming Providers...

        #region EMPRoamingProviders

        private readonly ConcurrentDictionary<EMPRoamingProvider_Id, IEMPRoamingProvider> empRoamingProviders = new ();

        /// <summary>
        /// Return all e-mobility provider roaming providers registered within this roaming network.
        /// </summary>
        public IEnumerable<IEMPRoamingProvider> EMPRoamingProviders
            => empRoamingProviders.Values;

        #endregion


        #region EMPRoamingProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, IEMPRoamingProvider, Boolean> EMPRoamingProviderAddition;

        /// <summary>
        /// Called whenever an e-mobility provider roaming provider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, IEMPRoamingProvider, Boolean> OnEMPRoamingProviderAddition
            => EMPRoamingProviderAddition;

        #endregion

        #region EMPRoamingProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, IEMPRoamingProvider, Boolean> EMPRoamingProviderRemoval;

        /// <summary>
        /// Called whenever an e-mobility provider roaming provider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, IEMPRoamingProvider, Boolean> OnEMPRoamingProviderRemoval
            => EMPRoamingProviderRemoval;

        #endregion


        #region CreateNewRoamingProvider(eMobilityRoamingService, Configurator = null)

        /// <summary>
        /// Create and register a new e-mobility provider roaming provider having the given
        /// unique e-mobility provider roaming provider identification.
        /// </summary>
        /// <param name="EMPRoamingProvider">An e-mobility provider roaming provider.</param>
        /// <param name="Configurator">An optional delegate to configure the new e-mobility provider roaming provider after its creation.</param>
        public IEMPRoamingProvider CreateEMPRoamingProvider(IEMPRoamingProvider           EMPRoamingProvider,
                                                            Action<IEMPRoamingProvider>?  Configurator = null)
        {

            #region Initial checks

            if (empRoamingProviders.ContainsKey(EMPRoamingProvider.Id))
                throw new EMPRoamingProviderAlreadyExists(this, EMPRoamingProvider.Id);

            if (EMPRoamingProvider.Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(EMPRoamingProvider) + ".Name", "The given e-mobility provider roaming provider name must not be null or empty!");

            if (EMPRoamingProvider.RoamingNetwork.Id != this.Id)
                throw new ArgumentException("The given e-mobility provider roaming provider is not part of this roaming network!", nameof(EMPRoamingProvider));

            #endregion

            Configurator?.Invoke(EMPRoamingProvider);

            if (EMPRoamingProviderAddition.SendVoting(this, EMPRoamingProvider))
            {
                if (empRoamingProviders.TryAdd(EMPRoamingProvider.Id, EMPRoamingProvider))
                {

                    //allSendRoamingNetworkData.         Add(EMPRoamingProvider);
                    //allSendChargingStationOperatorData.Add(EMPRoamingProvider);
                    //allSendChargingPoolData.           Add(EMPRoamingProvider);
                    //allSendChargingStationData.        Add(EMPRoamingProvider);
                    //allSendEVSEData.                   Add(EMPRoamingProvider);

                    //allSendAdminStatus.                Add(EMPRoamingProvider);
                    //allSendStatus.                     Add(EMPRoamingProvider);
                    //allSendEnergyStatus.               Add(EMPRoamingProvider);

                    //allSend2RemoteAuthorizeStartStop.  Add(EMPRoamingProvider);
                    //allRemoteSendChargeDetailRecord.   Add(EMPRoamingProvider);

                    EMPRoamingProviderAddition.SendNotification(this, EMPRoamingProvider);

                    SetRoamingProviderPriority(EMPRoamingProvider,
                                               eMobilityRoamingServices.Count > 0
                                                   ? eMobilityRoamingServices.Keys.Max() + 1
                                                   : 10);

                    return EMPRoamingProvider;

                }
            }

            throw new Exception("Could not create new roaming provider '" + EMPRoamingProvider.Id + "'!");

        }

        #endregion

        #region SetRoamingProviderPriority(eMobilityRoamingService, Priority)

        /// <summary>
        /// Change the prioity of the given e-mobility roaming service.
        /// </summary>
        /// <param name="eMobilityRoamingService">The e-mobility roaming service.</param>
        /// <param name="Priority">The priority of the service.</param>
        public Boolean SetRoamingProviderPriority(IEMPRoamingProvider  eMobilityRoamingService,
                                                  UInt32               Priority)

            => eMobilityRoamingServices.TryAdd(Priority, eMobilityRoamingService);

        #endregion


        #region RegisterPushEVSEDataService(Priority, PushEVSEDataServices)

        ///// <summary>
        ///// Register the given push-data service.
        ///// </summary>
        ///// <param name="Priority">The priority of the service.</param>
        ///// <param name="PushEVSEDataServices">The push-data service.</param>
        //public Boolean RegisterPushEVSEStatusService(UInt32              Priority,
        //                                             IPushData           PushEVSEDataServices)

        //    => _PushEVSEDataToOperatorRoamingServices.TryAdd(Priority, PushEVSEDataServices);

        #endregion

        #region RegisterPushEVSEStatusService(Priority, PushEVSEStatusServices)

        ///// <summary>
        ///// Register the given push-status service.
        ///// </summary>
        ///// <param name="Priority">The priority of the service.</param>
        ///// <param name="PushEVSEStatusServices">The push-status service.</param>
        //public Boolean RegisterPushEVSEStatusService(UInt32       Priority,
        //                                             IPushStatus  PushEVSEStatusServices)

        //    => _PushEVSEStatusToOperatorRoamingServices.TryAdd(Priority, PushEVSEStatusServices);

        #endregion


        #region ContainsEMPRoamingProvider  (EMPRoamingProvider)

        /// <summary>
        /// Check if the given charging station operator roaming provider is already present within the roaming network.
        /// </summary>
        /// <param name="EMPRoamingProvider">A charging station operator roaming provider.</param>
        public Boolean ContainsEMPRoamingProvider(IEMPRoamingProvider EMPRoamingProvider)

            => empRoamingProviders.ContainsKey(EMPRoamingProvider.Id);

        #endregion

        #region ContainsEMPRoamingProvider  (EMPRoamingProviderId)

        /// <summary>
        /// Check if the given charging station operator roaming provider identification is already present within the roaming network.
        /// </summary>
        /// <param name="EMPRoamingProviderId">The unique identification of the charging station operator roaming provider.</param>
        public Boolean ContainsEMPRoamingProvider(EMPRoamingProvider_Id EMPRoamingProviderId)

            => empRoamingProviders.ContainsKey(EMPRoamingProviderId);

        #endregion

        #region GetEMPRoamingProviderById   (EMPRoamingProviderId)

        public IEMPRoamingProvider? GetEMPRoamingProviderById(EMPRoamingProvider_Id EMPRoamingProviderId)
        {

            if (empRoamingProviders.TryGetValue(EMPRoamingProviderId, out var empRoamingProvider))
                return empRoamingProvider;

            return null;

        }

        public IEMPRoamingProvider? GetEMPRoamingProviderById(EMPRoamingProvider_Id? EMPRoamingProviderId)
        {

            if (EMPRoamingProviderId.HasValue &&
                empRoamingProviders.TryGetValue(EMPRoamingProviderId.Value, out var empRoamingProvider))
            {
                return empRoamingProvider;
            }

            return null;

        }

        #endregion

        #region TryGetEMPRoamingProviderById(EMPRoamingProviderId, out ChargingStationOperator)

        public Boolean TryGetEMPRoamingProviderById(EMPRoamingProvider_Id EMPRoamingProviderId, out IEMPRoamingProvider? EMPRoamingProvider)

            => empRoamingProviders.TryGetValue(EMPRoamingProviderId, out EMPRoamingProvider);

        public Boolean TryGetEMPRoamingProviderById(EMPRoamingProvider_Id? EMPRoamingProviderId, out IEMPRoamingProvider? EMPRoamingProvider)
        {

            if (!EMPRoamingProviderId.HasValue)
            {
                EMPRoamingProvider = null;
                return false;
            }

            return empRoamingProviders.TryGetValue(EMPRoamingProviderId.Value, out EMPRoamingProvider);

        }

        #endregion

        #region RemoveEMPRoamingProvider    (EMPRoamingProviderId)

        public IEMPRoamingProvider? RemoveEMPRoamingProvider(EMPRoamingProvider_Id EMPRoamingProviderId)
        {

            if (empRoamingProviders.TryRemove(EMPRoamingProviderId, out var empRoamingProvider))
                return empRoamingProvider;

            return null;

        }

        public IEMPRoamingProvider? RemoveEMPRoamingProvider(EMPRoamingProvider_Id? EMPRoamingProviderId)
        {

            if (EMPRoamingProviderId.HasValue &&
                empRoamingProviders.TryRemove(EMPRoamingProviderId.Value, out var empRoamingProvider))
            {
                return empRoamingProvider;
            }

            return null;

        }

        #endregion

        #region TryRemoveEMPRoamingProvider (EMPRoamingProviderId, out EMPRoamingProvider)

        public Boolean TryRemoveEMPRoamingProvider(EMPRoamingProvider_Id EMPRoamingProviderId, out IEMPRoamingProvider? EMPRoamingProvider)

            => empRoamingProviders.TryRemove(EMPRoamingProviderId, out EMPRoamingProvider);

        public Boolean TryRemoveEMPRoamingProvider(EMPRoamingProvider_Id? EMPRoamingProviderId, out IEMPRoamingProvider? EMPRoamingProvider)
        {

            if (!EMPRoamingProviderId.HasValue)
            {
                EMPRoamingProvider = null;
                return false;
            }

            return empRoamingProviders.TryRemove(EMPRoamingProviderId.Value, out EMPRoamingProvider);

        }

        #endregion

        #endregion

        #region E-Mobility Providers...

        #region EMobilityProviders

        private readonly EntityHashSet<IRoamingNetwork, EMobilityProvider_Id, IEMobilityProvider> eMobilityProviders;

        /// <summary>
        /// Return all e-mobility providers registered within this roaming network.
        /// </summary>
        public IEnumerable<IEMobilityProvider> EMobilityProviders
        {
            get
            {

                if (eMobilityProvidersSemaphore.Wait(SemaphoreSlimTimeout))
                {
                    try
                    {

                        return eMobilityProviders.ToArray();

                    }
                    finally
                    {
                        try
                        {
                            eMobilityProvidersSemaphore.Release();
                        }
                        catch
                        { }
                    }
                }

                return Array.Empty<IEMobilityProvider>();

            }
        }

        #endregion

        #region EMobilityProviderIds        (IncludeEMobilityProvider = null)

        /// <summary>
        /// Return all e-mobility providers identifications registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEMobilityProvider">An optional delegate for filtering charging station operators.</param>
        public IEnumerable<EMobilityProvider_Id> EMobilityProviderIds(IncludeEMobilityProviderDelegate? IncludeEMobilityProvider = null)
        {

            IncludeEMobilityProvider ??= (eMobilityProvider => true);

            return eMobilityProviders.
                       Where (eMobilityProvider => IncludeEMobilityProvider(eMobilityProvider)).
                       Select(eMobilityProvider => eMobilityProvider.Id);

        }

        #endregion

        #region EMobilityProviderAdminStatus(IncludeEMobilityProvider = null)

        /// <summary>
        /// Return the admin status of all e-mobility providerss registered within this roaming network.
        /// </summary>
        public IEnumerable<EMobilityProviderAdminStatus> EMobilityProviderAdminStatus(IncludeEMobilityProviderDelegate? IncludeEMobilityProvider = null)
        {

            IncludeEMobilityProvider ??= (eMobilityProvider => true);

            return eMobilityProviders.
                       Where (eMobilityProvider => IncludeEMobilityProvider(eMobilityProvider)).
                       Select(eMobilityProvider => new EMobilityProviderAdminStatus(eMobilityProvider.Id,
                                                                                    eMobilityProvider.AdminStatus));

        }

        #endregion

        #region EMobilityProviderStatus     (IncludeEMobilityProvider = null)

        /// <summary>
        /// Return the status of all e-mobility providerss registered within this roaming network.
        /// </summary>
        public IEnumerable<EMobilityProviderStatus> EMobilityProviderStatus(IncludeEMobilityProviderDelegate? IncludeEMobilityProvider = null)
        {

            IncludeEMobilityProvider ??= (eMobilityProvider => true);

            return eMobilityProviders.
                       Where (eMobilityProvider => IncludeEMobilityProvider(eMobilityProvider)).
                       Select(eMobilityProvider => new EMobilityProviderStatus(eMobilityProvider.Id,
                                                                               eMobilityProvider.Status));

        }

        #endregion


        #region EMobilityProviderAddition

        /// <summary>
        /// Called whenever a charging station operator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IRoamingNetwork, IEMobilityProvider, Boolean> OnEMobilityProviderAddition
            => eMobilityProviders.OnAddition;

        private void SendEMobilityProviderAdded(DateTime            Timestamp,
                                                IRoamingNetwork     RoamingNetwork,
                                                IEMobilityProvider  EMobilityProvider)
        {

            try
            {

                //var results = _ISendData.WhenAll(iSendData => iSendData.AddStaticData(EMobilityProvider));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.SendEMobilityProviderAdded of charging station operator '{EMobilityProvider.Id}' to roaming network '{RoamingNetwork.Id}'");
            }

        }

        #endregion

        #region (internal) UpdateEMobilityProviderData       (Timestamp, EventTrackingId, EMobilityProvider, PropertyName, NewValue, OldValue, DataSource)

        /// <summary>
        /// An event fired whenever the static data of any subordinated Charging Station Operator changed.
        /// </summary>
        public event OnEMobilityProviderDataChangedDelegate? OnEMobilityProviderDataChanged;


        /// <summary>
        /// Update the data of a charging station operator.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="EMobilityProvider">The changed charging station operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateEMobilityProviderData(DateTime            Timestamp,
                                                        EventTracking_Id    EventTrackingId,
                                                        IEMobilityProvider  EMobilityProvider,
                                                        String              PropertyName,
                                                        Object?             OldValue,
                                                        Object?             NewValue)
        {

            try
            {

                //var results = _ISendData.WhenAll(iSendData => iSendData.UpdateStaticData(EMobilityProvider,
                //                                                                         PropertyName,
                //                                                                         OldValue,
                //                                                                         NewValue));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateEMobilityProviderData of charging station operator '{EMobilityProvider.Id}' property '{PropertyName}' from '{OldValue?.ToString() ?? "-"}' to '{NewValue?.ToString() ?? "-"}'");
            }


            //var OnEMobilityProviderDataChangedLocal = OnEMobilityProviderDataChanged;
            //if (OnEMobilityProviderDataChangedLocal is not null)
            //    await OnEMobilityProviderDataChangedLocal(Timestamp,
            //                                              EventTrackingId ?? EventTracking_Id.New,
            //                                              EMobilityProvider,
            //                                              PropertyName,
            //                                              OldValue,
            //                                              NewValue);

        }

        #endregion

        #region EMobilityProviderRemoval

        /// <summary>
        /// Called whenever charging station operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IRoamingNetwork, IEMobilityProvider, Boolean> OnEMobilityProviderRemoval
            => eMobilityProviders.OnRemoval;

        private void SendEMobilityProviderRemoved(DateTime            Timestamp,
                                                  IRoamingNetwork     RoamingNetwork,
                                                  IEMobilityProvider  EMobilityProvider)
        {

            try
            {

                //var results = _ISendData.WhenAll(iSendData => iSendData.DeleteStaticData(EMobilityProvider));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.SendEMobilityProviderRemoved of charging station operator '{EMobilityProvider.Id}' from roaming network '{RoamingNetwork.Id}'");
            }

        }

        #endregion


        #region AddEMobilityProvider           (EMobilityProvider,      ..., OnAdded = null, ...)

        /// <summary>
        /// An event fired whenever a charging station operator was added.
        /// </summary>
        public event OnEMobilityProviderAddedDelegate? OnEMobilityProviderAdded;


        #region (protected internal) _AddEMobilityProvider(EMobilityProvider, ..., OnAdded = null, ...)

        /// <summary>
        /// Add the given user to the API.
        /// </summary>
        /// <param name="EMobilityProvider">A charging station operator.</param>
        /// <param name="SkipNewEMobilityProviderNotifications">Do not send notifications for this charging station operator addition.</param>
        /// <param name="OnAdded">A delegate run whenever the charging station operator has been added successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        protected internal async Task<AddEMobilityProviderResult>

            _AddEMobilityProvider(IEMobilityProvider                 EMobilityProvider,
                                  Boolean                            SkipNewEMobilityProviderNotifications   = false,
                                  OnEMobilityProviderAddedDelegate?  OnAdded                                 = null,
                                  EventTracking_Id?                  EventTrackingId                         = null,
                                  User_Id?                           CurrentUserId                           = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            //if (User.API is not null && User.API != this)
            //    return AddUserResult.ArgumentError(User,
            //                                       eventTrackingId,
            //                                       nameof(User),
            //                                       "The given user is already attached to another API!");

            if (eMobilityProviders.ContainsId(EMobilityProvider.Id))
                return AddEMobilityProviderResult.ArgumentError(
                           EMobilityProvider,
                           $"The given charging station operator identification '{EMobilityProvider.Id}' already exists!".ToI18NString(),
                           eventTrackingId,
                           Id,
                           this,
                           this
                       );

            if (EMobilityProvider.Id.Length < MinEMobilityProviderIdLength)
                return AddEMobilityProviderResult.ArgumentError(
                           EMobilityProvider,
                           $"The given charging station operator identification '{EMobilityProvider.Id}' is too short!".ToI18NString(),
                           eventTrackingId,
                           Id,
                           this,
                           this
                       );

            if (EMobilityProvider.Name.IsNullOrEmpty())
                return AddEMobilityProviderResult.ArgumentError(
                           EMobilityProvider,
                           "The given charging station operator name must not be null!".ToI18NString(),
                           eventTrackingId,
                           Id,
                           this,
                           this
                       );

            if (EMobilityProvider.Name.FirstText().Length < MinEMobilityProviderNameLength)
                return AddEMobilityProviderResult.ArgumentError(
                           EMobilityProvider,
                           $"The given charging station operator name '{EMobilityProvider.Name}' is too short!".ToI18NString(),
                           eventTrackingId,
                           Id,
                           this,
                           this
                       );

            //User.API = this;


            //await WriteToDatabaseFile(addUser_MessageType,
            //                          EMobilityProvider.ToJSON(false),
            //                          eventTrackingId,
            //                          CurrentUserId);

            var now = Timestamp.Now;

            if (eMobilityProviders.TryAdd(EMobilityProvider,
                                          EventTracking_Id.New,
                                          null).Result == CommandResult.Success)
            {

                //EMobilityProvider.OnDataChanged                              += UpdateEMobilityProviderData;
                //EMobilityProvider.OnStatusChanged                            += UpdateEMobilityProviderStatus;
                //EMobilityProvider.OnAdminStatusChanged                       += UpdateEMobilityProviderAdminStatus;

                //EMobilityProvider.OnChargingPoolAddition.   OnVoting         += (timestamp, cso, pool, vote)      => ChargingPoolAddition.   SendVoting      (timestamp, cso, pool, vote);
                //EMobilityProvider.OnChargingPoolAddition.   OnNotification   += SendChargingPoolAdded;
                //EMobilityProvider.OnChargingPoolDataChanged                  += UpdateChargingPoolData;
                //EMobilityProvider.OnChargingPoolStatusChanged                += UpdateChargingPoolStatus;
                //EMobilityProvider.OnChargingPoolAdminStatusChanged           += UpdateChargingPoolAdminStatus;
                //EMobilityProvider.OnChargingPoolRemoval.    OnVoting         += (timestamp, cso, pool, vote)      => ChargingPoolRemoval.    SendVoting      (timestamp, cso, pool, vote);
                //EMobilityProvider.OnChargingPoolRemoval.    OnNotification   += (timestamp, cso, pool)            => ChargingPoolRemoval.    SendNotification(timestamp, cso, pool);

                //EMobilityProvider.OnChargingStationAddition.OnVoting         += (timestamp, pool, station, vote)  => ChargingStationAddition.SendVoting      (timestamp, pool, station, vote);
                //EMobilityProvider.OnChargingStationAddition.OnNotification   += SendChargingStationAdded;
                //EMobilityProvider.OnChargingStationDataChanged               += UpdateChargingStationData;
                //EMobilityProvider.OnChargingStationStatusChanged             += UpdateChargingStationStatus;
                //EMobilityProvider.OnChargingStationAdminStatusChanged        += UpdateChargingStationAdminStatus;
                //EMobilityProvider.OnChargingStationRemoval. OnVoting         += (timestamp, pool, station, vote)  => ChargingStationRemoval. SendVoting      (timestamp, pool, station, vote);
                //EMobilityProvider.OnChargingStationRemoval. OnNotification   += (timestamp, pool, station)        => ChargingStationRemoval. SendNotification(timestamp, pool, station);

                //EMobilityProvider.OnEVSEAddition.           OnVoting         += (timestamp, station, evse, vote)  => EVSEAddition.           SendVoting      (timestamp, station, evse, vote);
                //EMobilityProvider.OnEVSEAddition.           OnNotification   += SendEVSEAdded;
                //EMobilityProvider.OnEVSEDataChanged                          += UpdateEVSEData;
                //EMobilityProvider.OnEVSEStatusChanged                        += UpdateEVSEStatus;
                //EMobilityProvider.OnEVSEAdminStatusChanged                   += UpdateEVSEAdminStatus;
                //EMobilityProvider.OnEVSERemoval.            OnVoting         += (timestamp, station, evse, vote)  => EVSERemoval.            SendVoting      (timestamp, station, evse, vote);
                //EMobilityProvider.OnEVSERemoval.            OnNotification   += (timestamp, station, evse)        => EVSERemoval.            SendNotification(timestamp, station, evse);

                allSendAdminStatus.Add              (EMobilityProvider);
                allSendStatus.Add                   (EMobilityProvider);
                allSend2RemoteAuthorizeStartStop.Add(EMobilityProvider);
                allRemoteSendChargeDetailRecord.Add (EMobilityProvider);

                EMobilityProvider.OnNewReservation                           += SendNewReservation;
                EMobilityProvider.OnReservationCanceled                      += SendReservationCanceled;
                EMobilityProvider.OnNewChargingSession                       += SendNewChargingSession;
                EMobilityProvider.OnNewChargeDetailRecord                    += SendNewChargeDetailRecord;

                OnAdded?.Invoke(now,
                                EMobilityProvider,
                                eventTrackingId,
                                CurrentUserId);

            }

            //var OnEMobilityProviderAddedLocal = OnEMobilityProviderAdded;
            //if (OnEMobilityProviderAddedLocal is not null)
            //    await OnEMobilityProviderAddedLocal.Invoke(Timestamp.Now,
            //                                                     EMobilityProvider,
            //                                                     eventTrackingId,
            //                                                     CurrentUserId);


            //if (!SkipNewEMobilityProviderNotifications)
            //    await SendNotifications(EMobilityProvider,
            //                            addEMobilityProvider_MessageType,
            //                            null,
            //                            eventTrackingId,
            //                            CurrentUserId);


            return AddEMobilityProviderResult.Success(
                       EMobilityProvider,
                       eventTrackingId,
                       Id,
                       this,
                       this
                   );

        }

        #endregion

        #region AddEMobilityProvider                      (EMobilityProvider, ..., OnAdded = null, ...)

        /// <summary>
        /// Add the given charging station operator.
        /// </summary>
        /// <param name="EMobilityProvider">A charging station operator.</param>
        /// <param name="SkipNewEMobilityProviderNotifications">Do not send notifications for this charging station operator addition.</param>
        /// <param name="OnAdded">A delegate run whenever the charging station operator has been added successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddEMobilityProviderResult>

            AddEMobilityProvider(IEMobilityProvider                 EMobilityProvider,
                                 Boolean                            SkipNewEMobilityProviderNotifications   = false,
                                 OnEMobilityProviderAddedDelegate?  OnAdded                                 = null,
                                 EventTracking_Id?                  EventTrackingId                         = null,
                                 User_Id?                           CurrentUserId                           = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (await eMobilityProvidersSemaphore.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    var result = await _AddEMobilityProvider(EMobilityProvider,
                                                             SkipNewEMobilityProviderNotifications,
                                                             OnAdded,
                                                             eventTrackingId,
                                                             CurrentUserId);


                    return result;

                }
                catch (Exception e)
                {

                    return AddEMobilityProviderResult.Error(
                               EMobilityProvider,
                               e,
                               eventTrackingId,
                               Id,
                               this,
                               this
                           );

                }
                finally
                {
                    try
                    {
                        eMobilityProvidersSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return AddEMobilityProviderResult.LockTimeout(
                       EMobilityProvider,
                       SemaphoreSlimTimeout,
                       eventTrackingId,
                       Id,
                       this,
                       this
                   );

        }

        #endregion

        #endregion


        #region RegistereMobilityProvider(Priority, eMobilityServiceProvider)

        ///// <summary>
        ///// Register the given e-Mobility (service) provider.
        ///// </summary>
        ///// <param name="Priority">The priority of the service provider.</param>
        ///// <param name="eMobilityServiceProvider">An e-Mobility service provider.</param>
        //public Boolean RegistereMobilityProvider(UInt32                     Priority,
        //                                         IeMobilityServiceProvider  eMobilityServiceProvider)
        //{

        //    var result = _IeMobilityServiceProviders.TryAdd(Priority, eMobilityServiceProvider);

        //    if (result)
        //    {

        //        this.OnChargingStationRemoval.OnNotification += eMobilityServiceProvider.RemoveChargingStations;

        //    }

        //    return result;

        //}

        #endregion


        #region ContainsEMobilityProvider  (EMobilityProvider)

        /// <summary>
        /// Check if the given e-mobility provider is already present within the roaming network.
        /// </summary>
        /// <param name="EMobilityProvider">An e-mobility provider.</param>
        public Boolean ContainsEMobilityProvider(IEMobilityProvider EMobilityProvider)

            => eMobilityProviders.ContainsId(EMobilityProvider.Id);

        #endregion

        #region ContainsEMobilityProvider  (EMobilityProviderId)

        /// <summary>
        /// Check if the given e-mobility provider identification is already present within the roaming network.
        /// </summary>
        /// <param name="EMobilityProviderId">The unique identification of the e-mobility provider.</param>
        public Boolean ContainsEMobilityProvider(EMobilityProvider_Id EMobilityProviderId)

            => eMobilityProviders.ContainsId(EMobilityProviderId);

        #endregion

        #region GetEMobilityProviderById   (EMobilityProviderId)

        public IEMobilityProvider? GetEMobilityProviderById(EMobilityProvider_Id  EMobilityProviderId)

            => eMobilityProviders.GetById(EMobilityProviderId);

        public IEMobilityProvider? GetEMobilityProviderById(EMobilityProvider_Id? EMobilityProviderId)

            => EMobilityProviderId.HasValue
                   ? eMobilityProviders.GetById(EMobilityProviderId.Value)
                   : null;

        #endregion

        #region TryGetEMobilityProviderById(EMobilityProviderId, out EMobilityProvider)

        public Boolean TryGetEMobilityProviderById(EMobilityProvider_Id EMobilityProviderId, out IEMobilityProvider? EMobilityProvider)

            => eMobilityProviders.TryGet(EMobilityProviderId, out EMobilityProvider);

        public Boolean TryGetEMobilityProviderById(EMobilityProvider_Id? EMobilityProviderId, out IEMobilityProvider? EMobilityProvider)
        {

            if (!EMobilityProviderId.HasValue)
            {
                EMobilityProvider = null;
                return false;
            }

            return eMobilityProviders.TryGet(EMobilityProviderId.Value, out EMobilityProvider);

        }

        #endregion

        #region RemoveEMobilityProvider    (EMobilityProviderId)

        public IEMobilityProvider? RemoveEMobilityProvider(EMobilityProvider_Id EMobilityProviderId)
        {

            if (eMobilityProviders.TryRemove(EMobilityProviderId,
                                             out var eMobilityProvider,
                                             EventTracking_Id.New,
                                             null))
            {
                return eMobilityProvider;
            }

            return null;

        }

        public IEMobilityProvider? RemoveEMobilityProvider(EMobilityProvider_Id? EMobilityProviderId)
        {

            if (EMobilityProviderId is not null &&
                eMobilityProviders.TryRemove(EMobilityProviderId.Value,
                                             out var eMobilityProvider,
                                             EventTracking_Id.New,
                                             null))
            {
                return eMobilityProvider;
            }

            return null;

        }

        #endregion

        #region TryRemoveEMobilityProvider (EMobilityProviderId, out EMobilityProvider)

        public Boolean TryRemoveEMobilityProvider(EMobilityProvider_Id EMobilityProviderId, out IEMobilityProvider? EMobilityProvider)

            => eMobilityProviders.TryRemove(EMobilityProviderId,
                                            out EMobilityProvider,
                                            EventTracking_Id.New,
                                            null);

        public Boolean TryRemoveEMobilityProvider(EMobilityProvider_Id? EMobilityProviderId, out IEMobilityProvider? EMobilityProvider)

        {

            if (!EMobilityProviderId.HasValue)
            {
                EMobilityProvider = null;
                return false;
            }

            return eMobilityProviders.TryRemove(EMobilityProviderId.Value,
                                                out EMobilityProvider,
                                                EventTracking_Id.New,
                                                null);

        }

        #endregion

        #endregion


        #region Charging Station Operator Roaming Providers...

        #region CSORoamingProviders

        private readonly ConcurrentDictionary<CSORoamingProvider_Id, ICSORoamingProvider>  csoRoamingProviders = new ();

        /// <summary>
        /// Return all charging station operator roaming providers registered within this roaming network.
        /// </summary>
        public IEnumerable<ICSORoamingProvider> CSORoamingProviders
            => csoRoamingProviders.Values;

        #endregion


        #region CSORoamingProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean> CSORoamingProviderAddition;

        /// <summary>
        /// Called whenever a charging station operator roaming provider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, ICSORoamingProvider, Boolean> OnCSORoamingProviderAddition
            => CSORoamingProviderAddition;

        #endregion

        #region CPORoamingProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean> CSORoamingProviderRemoval;

        /// <summary>
        /// Called whenever a charging station operator roaming provider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, ICSORoamingProvider, Boolean> OnCSORoamingProviderRemoval
            => CSORoamingProviderRemoval;

        #endregion


        #region CreateNewRoamingProvider(CSORoamingProvider, Configurator = null)

        /// <summary>
        /// Create and register a new charging station operator roaming provider having the given
        /// unique electric vehicle roaming provider identification.
        /// </summary>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public ICSORoamingProvider CreateCSORoamingProvider(ICSORoamingProvider           CSORoamingProvider,
                                                            Action<ICSORoamingProvider>?  Configurator   = null)
        {

            #region Initial checks

            if (csoRoamingProviders.ContainsKey(CSORoamingProvider.Id))
                throw new CSORoamingProviderAlreadyExists(this, CSORoamingProvider.Id);

            if (CSORoamingProvider.Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(CSORoamingProvider) + ".Name",  "The given roaming provider name must not be null or empty!");

            if (CSORoamingProvider.RoamingNetwork.Id != this.Id)
                throw new ArgumentException("The given operator roaming service is not part of this roaming network!", nameof(CSORoamingProvider));

            #endregion

            Configurator?.Invoke(CSORoamingProvider);

            if (CSORoamingProviderAddition.SendVoting(this, CSORoamingProvider))
            {
                if (csoRoamingProviders.TryAdd(CSORoamingProvider.Id, CSORoamingProvider))
                {

                    allSendRoamingNetworkData.         Add(CSORoamingProvider);
                    allSendChargingStationOperatorData.Add(CSORoamingProvider);
                    allSendChargingPoolData.           Add(CSORoamingProvider);
                    allSendChargingStationData.        Add(CSORoamingProvider);
                    allSendEVSEData.                   Add(CSORoamingProvider);

                    allSendAdminStatus.                Add(CSORoamingProvider);
                    allSendStatus.                     Add(CSORoamingProvider);
                    allSendEnergyStatus.               Add(CSORoamingProvider);

                    allSend2RemoteAuthorizeStartStop.  Add(CSORoamingProvider);
                    allRemoteSendChargeDetailRecord.   Add(CSORoamingProvider);

                    //SetRoamingProviderPriority(_CPORoamingProvider,
                    //                           _ChargingStationOperatorRoamingProviderPriorities.Count > 0
                    //                               ? _ChargingStationOperatorRoamingProviderPriorities.Keys.Max() + 1
                    //                               : 10);

                    CSORoamingProviderAddition.SendNotification(this, CSORoamingProvider);

                    SetRoamingProviderPriority(CSORoamingProvider,
                                               csoRoamingServices.Count > 0
                                                   ? csoRoamingServices.Keys.Max() + 1
                                                   : 10);

                    return CSORoamingProvider;

                }
            }

            throw new Exception("Could not create new charging station operator roaming provider '" + CSORoamingProvider.Id + "'!");

        }

        #endregion

        #region SetRoamingProviderPriority(CSORoamingProvider, Priority)

        /// <summary>
        /// Change the given Charging Station Operator roaming service priority.
        /// </summary>
        /// <param name="CSORoamingProvider">The Charging Station Operator roaming provider.</param>
        /// <param name="Priority">The priority of the service.</param>
        public Boolean SetRoamingProviderPriority(ICSORoamingProvider  CSORoamingProvider,
                                                  UInt32               Priority)

            => csoRoamingServices.TryAdd(Priority, CSORoamingProvider);

        #endregion


        #region ContainsCSORoamingProvider  (CSORoamingProvider)

        /// <summary>
        /// Check if the given charging station operator roaming provider is already present within the roaming network.
        /// </summary>
        /// <param name="CSORoamingProvider">A charging station operator roaming provider.</param>
        public Boolean ContainsCSORoamingProvider(ICSORoamingProvider CSORoamingProvider)

            => csoRoamingProviders.ContainsKey(CSORoamingProvider.Id);

        #endregion

        #region ContainsCSORoamingProvider  (CSORoamingProviderId)

        /// <summary>
        /// Check if the given charging station operator roaming provider identification is already present within the roaming network.
        /// </summary>
        /// <param name="CSORoamingProviderId">The unique identification of the charging station operator roaming provider.</param>
        public Boolean ContainsCSORoamingProvider(CSORoamingProvider_Id CSORoamingProviderId)

            => csoRoamingProviders.ContainsKey(CSORoamingProviderId);

        #endregion

        #region GetCSORoamingProviderById   (CSORoamingProviderId)

        public ICSORoamingProvider? GetCSORoamingProviderById(CSORoamingProvider_Id CSORoamingProviderId)
        {

            if (csoRoamingProviders.TryGetValue(CSORoamingProviderId, out var csoRoamingProvider))
                return csoRoamingProvider;

            return null;

        }

        public ICSORoamingProvider? GetCSORoamingProviderById(CSORoamingProvider_Id? CSORoamingProviderId)
        {

            if (CSORoamingProviderId.HasValue &&
                csoRoamingProviders.TryGetValue(CSORoamingProviderId.Value, out var csoRoamingProvider))
            {
                return csoRoamingProvider;
            }

            return null;

        }

        #endregion

        #region TryGetCSORoamingProviderById(CSORoamingProviderId, out ChargingStationOperator)

        public Boolean TryGetCSORoamingProviderById(CSORoamingProvider_Id CSORoamingProviderId, out ICSORoamingProvider? CSORoamingProvider)

            => csoRoamingProviders.TryGetValue(CSORoamingProviderId, out CSORoamingProvider);

        public Boolean TryGetCSORoamingProviderById(CSORoamingProvider_Id? CSORoamingProviderId, out ICSORoamingProvider? CSORoamingProvider)
        {

            if (!CSORoamingProviderId.HasValue)
            {
                CSORoamingProvider = null;
                return false;
            }

            return csoRoamingProviders.TryGetValue(CSORoamingProviderId.Value, out CSORoamingProvider);

        }

        #endregion

        #region RemoveCSORoamingProvider    (CSORoamingProviderId)

        public ICSORoamingProvider? RemoveCSORoamingProvider(CSORoamingProvider_Id CSORoamingProviderId)
        {

            if (csoRoamingProviders.TryRemove(CSORoamingProviderId, out var csoRoamingProvider))
                return csoRoamingProvider;

            return null;

        }

        public ICSORoamingProvider? RemoveCSORoamingProvider(CSORoamingProvider_Id? CSORoamingProviderId)
        {

            if (CSORoamingProviderId.HasValue &&
                csoRoamingProviders.TryRemove(CSORoamingProviderId.Value, out var csoRoamingProvider))
            {
                return csoRoamingProvider;
            }

            return null;

        }

        #endregion

        #region TryRemoveCSORoamingProvider (CSORoamingProviderId, out CSORoamingProvider)

        public Boolean TryRemoveCSORoamingProvider(CSORoamingProvider_Id CSORoamingProviderId, out ICSORoamingProvider? CSORoamingProvider)

            => csoRoamingProviders.TryRemove(CSORoamingProviderId, out CSORoamingProvider);

        public Boolean TryRemoveCSORoamingProvider(CSORoamingProvider_Id? CSORoamingProviderId, out ICSORoamingProvider? CSORoamingProvider)
        {

            if (!CSORoamingProviderId.HasValue)
            {
                CSORoamingProvider = null;
                return false;
            }

            return csoRoamingProviders.TryRemove(CSORoamingProviderId.Value, out CSORoamingProvider);

        }

        #endregion

        #endregion

        #region Charging Station Operators...

        #region ChargingStationOperators

        private readonly EntityHashSet<IRoamingNetwork, ChargingStationOperator_Id, IChargingStationOperator> chargingStationOperators;

        /// <summary>
        /// Return all charging station operators registered within this roaming network.
        /// </summary>
        public IEnumerable<IChargingStationOperator> ChargingStationOperators
        {
            get
            {

                if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
                {
                    try
                    {

                        return chargingStationOperators.ToArray();

                    }
                    finally
                    {
                        try
                        {
                            chargingStationOperatorsSemaphore.Release();
                        }
                        catch
                        { }
                    }
                }

                return Array.Empty<IChargingStationOperator>();

            }
        }

        #endregion

        #region ChargingStationOperatorIds(IncludeChargingStationOperator = null)

        /// <summary>
        /// Return all charging station operators registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingStationOperator">An optional delegate for filtering charging station operators.</param>
        public IEnumerable<ChargingStationOperator_Id> ChargingStationOperatorIds(IncludeChargingStationOperatorDelegate? IncludeChargingStationOperator = null)
        {

            if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    IncludeChargingStationOperator ??= (chargingStationOperator => true);

                    return chargingStationOperators.
                               Where (chargingStationOperator => IncludeChargingStationOperator(chargingStationOperator)).
                               Select(chargingStationOperator => chargingStationOperator.Id);

                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return Array.Empty<ChargingStationOperator_Id>();

        }

        #endregion


        #region ChargingStationOperatorAddition

        /// <summary>
        /// Called whenever a charging station operator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IRoamingNetwork, IChargingStationOperator, Boolean> OnChargingStationOperatorAddition
            => chargingStationOperators.OnAddition;

        private void SendChargingStationOperatorAdded(DateTime                  Timestamp,
                                                      EventTracking_Id          EventTrackingId,
                                                      User_Id                   CurrentUserId,
                                                      IRoamingNetwork           RoamingNetwork,
                                                      IChargingStationOperator  ChargingStationOperator)
        {

            try
            {

                var results = allSendChargingStationOperatorData.WhenAll(target => target.AddChargingStationOperator(ChargingStationOperator));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.SendChargingStationOperatorAdded of charging station operator '{ChargingStationOperator.Id}' to roaming network '{RoamingNetwork.Id}'");
            }

        }

        #endregion

        #region (internal) UpdateChargingStationOperatorData       (Timestamp, EventTrackingId, ChargingStationOperator, PropertyName, NewValue, OldValue, DataSource)

        /// <summary>
        /// An event fired whenever the static data of any subordinated Charging Station Operator changed.
        /// </summary>
        public event OnChargingStationOperatorDataChangedDelegate? OnChargingStationOperatorDataChanged;


        /// <summary>
        /// Update the data of a charging station operator.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStationOperator">The changed charging station operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateChargingStationOperatorData(DateTime                  Timestamp,
                                                              EventTracking_Id          EventTrackingId,
                                                              IChargingStationOperator  ChargingStationOperator,
                                                              String                    PropertyName,
                                                              Object?                   OldValue,
                                                              Object?                   NewValue)
        {

            try
            {

                var results = allSendChargingStationOperatorData.WhenAll(target => target.UpdateChargingStationOperator(ChargingStationOperator,
                                                                                                                        PropertyName,
                                                                                                                        OldValue,
                                                                                                                        NewValue));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateChargingStationOperatorData of charging station operator '{ChargingStationOperator.Id}' property '{PropertyName}' from '{OldValue?.ToString() ?? "-"}' to '{NewValue?.ToString() ?? "-"}'");
            }


            var OnChargingStationOperatorDataChangedLocal = OnChargingStationOperatorDataChanged;
            if (OnChargingStationOperatorDataChangedLocal is not null)
                await OnChargingStationOperatorDataChangedLocal(Timestamp,
                                                                EventTrackingId ?? EventTracking_Id.New,
                                                                ChargingStationOperator,
                                                                PropertyName,
                                                                OldValue,
                                                                NewValue);

        }

        #endregion

        #region ChargingStationOperatorRemoval

        /// <summary>
        /// Called whenever charging station operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IRoamingNetwork, IChargingStationOperator, Boolean> OnChargingStationOperatorRemoval
            => chargingStationOperators.OnRemoval;

        private void SendChargingStationOperatorRemoved(DateTime                  Timestamp,
                                                        EventTracking_Id          EventTrackingId,
                                                        User_Id                   UserId,
                                                        IRoamingNetwork           RoamingNetwork,
                                                        IChargingStationOperator  ChargingStationOperator)
        {

            try
            {

                var results = allSendChargingStationOperatorData.WhenAll(target => target.DeleteChargingStationOperator(ChargingStationOperator));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.SendChargingStationOperatorRemoved of charging station operator '{ChargingStationOperator.Id}' from roaming network '{RoamingNetwork.Id}'");
            }

        }

        #endregion


        #region AddChargingStationOperator           (ChargingStationOperator,      ..., OnAdded = null, ...)

        /// <summary>
        /// An event fired whenever a charging station operator was added.
        /// </summary>
        public event OnChargingStationOperatorAddedDelegate? OnChargingStationOperatorAdded;


        #region (protected internal) _AddChargingStationOperator(ChargingStationOperator, ..., OnAdded = null, ...)

        /// <summary>
        /// Add the given user to the API.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="SkipNewChargingStationOperatorNotifications">Do not send notifications for this charging station operator addition.</param>
        /// <param name="OnAdded">A delegate run whenever the charging station operator has been added successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        protected internal async Task<AddChargingStationOperatorResult>

            _AddChargingStationOperator(IChargingStationOperator                 ChargingStationOperator,
                                        Boolean                                  SkipNewChargingStationOperatorNotifications   = false,
                                        OnChargingStationOperatorAddedDelegate?  OnAdded                                       = null,
                                        EventTracking_Id?                        EventTrackingId                               = null,
                                        User_Id?                                 CurrentUserId                                 = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            //if (User.API is not null && User.API != this)
            //    return AddUserResult.ArgumentError(User,
            //                                       eventTrackingId,
            //                                       nameof(User),
            //                                       "The given user is already attached to another API!");

            if (chargingStationOperators.ContainsId(ChargingStationOperator.Id))
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           Description:              $"The given charging station operator identification '{ChargingStationOperator.Id}' already exists!".ToI18NString(),
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );;

            if (ChargingStationOperator.Id.Length < MinChargingStationOperatorIdLength)
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           Description:              $"The given charging station operator identification '{ChargingStationOperator.Id}' is too short!".ToI18NString(),
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            if (ChargingStationOperator.Name.IsNullOrEmpty())
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           Description:              $"The given charging station operator name must not be null or empty!".ToI18NString(),
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            if (ChargingStationOperator.Name.FirstText().Length < MinChargingStationOperatorNameLength)
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           Description:              $"The given charging station operator name '{ChargingStationOperator.Name}' is too short!".ToI18NString(),
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            //User.API = this;


            //await WriteToDatabaseFile(addUser_MessageType,
            //                          ChargingStationOperator.ToJSON(false),
            //                          eventTrackingId,
            //                          CurrentUserId);

            var now = Timestamp.Now;

            if (chargingStationOperators.TryAdd(ChargingStationOperator,
                                                EventTracking_Id.New,
                                                null).Result == CommandResult.Success)
            {

                ChargingStationOperator.OnDataChanged                              += UpdateChargingStationOperatorData;
                ChargingStationOperator.OnStatusChanged                            += UpdateChargingStationOperatorStatus;
                ChargingStationOperator.OnAdminStatusChanged                       += UpdateChargingStationOperatorAdminStatus;

                ChargingStationOperator.OnChargingPoolAddition.   OnVoting         += (timestamp, eventTrackingId, userId, cso, pool, vote)      => ChargingPoolAddition.   SendVoting      (timestamp, eventTrackingId, userId, cso, pool, vote);
                ChargingStationOperator.OnChargingPoolAddition.   OnNotification   += SendChargingPoolAdded;
                ChargingStationOperator.OnChargingPoolDataChanged                  += UpdateChargingPoolData;
                ChargingStationOperator.OnChargingPoolAdminStatusChanged           += UpdateChargingPoolAdminStatus;
                ChargingStationOperator.OnChargingPoolStatusChanged                += UpdateChargingPoolStatus;
                ChargingStationOperator.OnChargingPoolRemoval.    OnVoting         += (timestamp, eventTrackingId, userId, cso, pool, vote)      => ChargingPoolRemoval.    SendVoting      (timestamp, eventTrackingId, userId, cso, pool, vote);
                ChargingStationOperator.OnChargingPoolRemoval.    OnNotification   += (timestamp, eventTrackingId, userId, cso, pool)            => ChargingPoolRemoval.    SendNotification(timestamp, eventTrackingId, userId, cso, pool);

                ChargingStationOperator.OnChargingStationAddition.OnVoting         += (timestamp, eventTrackingId, userId, pool, station, vote)  => ChargingStationAddition.SendVoting      (timestamp, eventTrackingId, userId, pool, station, vote);
                ChargingStationOperator.OnChargingStationAddition.OnNotification   += SendChargingStationAdded;
                ChargingStationOperator.OnChargingStationDataChanged               += UpdateChargingStationData;
                ChargingStationOperator.OnChargingStationAdminStatusChanged        += UpdateChargingStationAdminStatus;
                ChargingStationOperator.OnChargingStationStatusChanged             += UpdateChargingStationStatus;
                ChargingStationOperator.OnChargingStationRemoval. OnVoting         += (timestamp, eventTrackingId, userId, pool, station, vote)  => ChargingStationRemoval. SendVoting      (timestamp, eventTrackingId, userId, pool, station, vote);
                ChargingStationOperator.OnChargingStationRemoval. OnNotification   += (timestamp, eventTrackingId, userId, pool, station)        => ChargingStationRemoval. SendNotification(timestamp, eventTrackingId, userId, pool, station);

                ChargingStationOperator.OnEVSEAddition.           OnVoting         += (timestamp, eventTrackingId, userId, station, evse, vote)  => EVSEAddition.           SendVoting      (timestamp, eventTrackingId, userId, station, evse, vote);
                ChargingStationOperator.OnEVSEAddition.           OnNotification   += SendEVSEAdded;
                ChargingStationOperator.OnEVSEDataChanged                          += UpdateEVSEData;
                ChargingStationOperator.OnEVSEAdminStatusChanged                   += UpdateEVSEAdminStatus;
                ChargingStationOperator.OnEVSEStatusChanged                        += UpdateEVSEStatus;
                ChargingStationOperator.OnEVSERemoval.            OnVoting         += (timestamp, eventTrackingId, userId, station, evse, vote)  => EVSERemoval.            SendVoting      (timestamp, eventTrackingId, userId, station, evse, vote);
                ChargingStationOperator.OnEVSERemoval.            OnNotification   += (timestamp, eventTrackingId, userId, station, evse)        => EVSERemoval.            SendNotification(timestamp, eventTrackingId, userId, station, evse);

                ChargingStationOperator.OnNewReservation                           += SendNewReservation;
                ChargingStationOperator.OnReservationCanceled                      += SendReservationCanceled;
                ChargingStationOperator.OnNewChargingSession                       += SendNewChargingSession;
                ChargingStationOperator.OnNewChargeDetailRecord                    += SendNewChargeDetailRecord;

                OnAdded?.Invoke(now,
                                ChargingStationOperator,
                                eventTrackingId,
                                CurrentUserId);

            }

            //var OnChargingStationOperatorAddedLocal = OnChargingStationOperatorAdded;
            //if (OnChargingStationOperatorAddedLocal is not null)
            //    await OnChargingStationOperatorAddedLocal.Invoke(Timestamp.Now,
            //                                                     ChargingStationOperator,
            //                                                     eventTrackingId,
            //                                                     CurrentUserId);


            //if (!SkipNewChargingStationOperatorNotifications)
            //    await SendNotifications(ChargingStationOperator,
            //                            addChargingStationOperator_MessageType,
            //                            null,
            //                            eventTrackingId,
            //                            CurrentUserId);


            return AddChargingStationOperatorResult.Success(
                       ChargingStationOperator:  ChargingStationOperator,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion

        #region AddChargingStationOperator                      (ChargingStationOperator, ..., OnAdded = null, ...)

        /// <summary>
        /// Add the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="SkipNewChargingStationOperatorNotifications">Do not send notifications for this charging station operator addition.</param>
        /// <param name="OnAdded">A delegate run whenever the charging station operator has been added successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddChargingStationOperatorResult>

            AddChargingStationOperator(IChargingStationOperator                 ChargingStationOperator,
                                       Boolean                                  SkipNewChargingStationOperatorNotifications   = false,
                                       OnChargingStationOperatorAddedDelegate?  OnAdded                                       = null,
                                       EventTracking_Id?                        EventTrackingId                               = null,
                                       User_Id?                                 CurrentUserId                                 = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (await chargingStationOperatorsSemaphore.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    var result = await _AddChargingStationOperator(ChargingStationOperator,
                                                                   SkipNewChargingStationOperatorNotifications,
                                                                   OnAdded,
                                                                   eventTrackingId,
                                                                   CurrentUserId);


                    return result;

                }
                catch (Exception e)
                {

                    return AddChargingStationOperatorResult.Error(
                               ChargingStationOperator:  ChargingStationOperator,
                               Exception:                e,
                               EventTrackingId:          eventTrackingId,
                               SenderId:                 Id,
                               Sender:                   this,
                               RoamingNetwork:           this
                           );

                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return AddChargingStationOperatorResult.LockTimeout(
                       ChargingStationOperator:  ChargingStationOperator,
                       Timeout:                  SemaphoreSlimTimeout,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion

        #endregion

        #region AddChargingStationOperatorIfNotExists(ChargingStationOperator,      ..., OnAdded = null, ...)

        #region (protected internal) _AddChargingStationOperatorIfNotExists(ChargingStationOperator, ..., OnAdded = null, ...)

        /// <summary>
        /// When it has not been created before, add the given user to the API.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="SkipNewChargingStationOperatorNotifications">Do not send notifications for this charging station operator addition.</param>
        /// <param name="OnAdded">A delegate run whenever the charging station operator has been added successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        protected internal async Task<AddChargingStationOperatorResult>

            _AddChargingStationOperatorIfNotExists(ChargingStationOperator                  ChargingStationOperator,
                                                   Boolean                                  SkipNewChargingStationOperatorNotifications   = false,
                                                   OnChargingStationOperatorAddedDelegate?  OnAdded                                       = null,
                                                   EventTracking_Id?                        EventTrackingId                               = null,
                                                   User_Id?                                 CurrentUserId                                 = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            //if (User.API is not null && User.API != this)
            //    return AddChargingStationOperatorResult.ArgumentError(User,
            //                                                  eventTrackingId,
            //                                                  nameof(User),
            //                                                  "The given user is already attached to another API!");

            if (chargingStationOperators.TryGet(ChargingStationOperator.Id, out var existingChargingStationOperator) && existingChargingStationOperator is not null)
                return AddChargingStationOperatorResult.Exists(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            if (ChargingStationOperator.Id.Length < MinChargingStationOperatorIdLength)
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           Description:              $"The given charging station operator identification '{ChargingStationOperator.Id}' is too short!".ToI18NString(),
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            if (ChargingStationOperator.Name.IsNullOrEmpty())
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           Description:              $"The given charging station operator name must not be null or empty!".ToI18NString(),
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            if (ChargingStationOperator.Name.FirstText().Length < MinChargingStationOperatorNameLength)
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           Description:              $"The given user name '{ChargingStationOperator.Name}' is too short!".ToI18NString(),
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            //ChargingStationOperator.API = this;


            //await WriteToDatabaseFile(addUserIfNotExists_MessageType,
            //                          User.ToJSON(false),
            //                          eventTrackingId,
            //                          CurrentUserId);

            var result  = chargingStationOperators.TryAdd(ChargingStationOperator,
                                                          eventTrackingId,
                                                          CurrentUserId);
            var now     = Timestamp.Now;

            OnAdded?.Invoke(now,
                            ChargingStationOperator,
                            eventTrackingId,
                            CurrentUserId);

            //var OnChargingStationOperatorAddedLocal = OnChargingStationOperatorAdded;
            //if (OnChargingStationOperatorAddedLocal is not null)
            //    await OnChargingStationOperatorAddedLocal?.Invoke(Timestamp.Now,
            //                                                      ChargingStationOperator,
            //                                                      eventTrackingId,
            //                                                      CurrentUserId);

            //if (!SkipNewChargingStationOperatorNotifications)
            //    await SendNotifications(ChargingStationOperator,
            //                            addChargingStationOperatorIfNotExists_MessageType,
            //                            null,
            //                            eventTrackingId,
            //                            CurrentUserId);


            return AddChargingStationOperatorResult.Success(
                       ChargingStationOperator:  ChargingStationOperator,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion

        #region AddChargingStationOperatorIfNotExists                      (ChargingStationOperator, ..., OnAdded = null, ...)

        /// <summary>
        /// Add the given user.
        /// </summary>
        /// <param name="User">A new user.</param>
        /// <param name="SkipDefaultNotifications">Do not apply the default notifications settings for new users.</param>
        /// <param name="OnAdded">A delegate run whenever the user has been added successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddChargingStationOperatorResult>

            AddChargingStationOperatorIfNotExists(ChargingStationOperator                  ChargingStationOperator,
                                                  Boolean                                  SkipNewUserNotifications   = false,
                                                  OnChargingStationOperatorAddedDelegate?  OnAdded                    = null,
                                                  EventTracking_Id?                        EventTrackingId            = null,
                                                  User_Id?                                 CurrentUserId              = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (await chargingStationOperatorsSemaphore.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    return await _AddChargingStationOperatorIfNotExists(ChargingStationOperator,
                                                                        SkipNewUserNotifications,
                                                                        OnAdded,
                                                                        eventTrackingId,
                                                                        CurrentUserId);

                }
                catch (Exception e)
                {

                    return AddChargingStationOperatorResult.Error(
                               ChargingStationOperator:  ChargingStationOperator,
                               Exception:                e,
                               EventTrackingId:          eventTrackingId,
                               SenderId:                 Id,
                               Sender:                   this,
                               RoamingNetwork:           this
                           );

                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return AddChargingStationOperatorResult.LockTimeout(
                       ChargingStationOperator:  ChargingStationOperator,
                       Timeout:                  SemaphoreSlimTimeout,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion

        #endregion

        #region AddOrUpdateChargingStationOperator   (ChargingStationOperator,      ..., OnAdded = null, OnUpdated = null, ...)

        #region (protected internal) _AddOrUpdateChargingStationOperator(ChargingStationOperator, ..., OnAdded = null, OnUpdated = null, ...)

        /// <summary>
        /// Add or update the given charging station operator to/within the API.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="SkipNewChargingStationOperatorNotifications">Do not send notifications for this charging station operator addition.</param>
        /// <param name="OnAdded">A delegate run whenever the charging station operator has been added successfully.</param>
        /// <param name="OnUpdated">A delegate run whenever the charging station operator has been updated successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        protected internal async Task<AddOrUpdateChargingStationOperatorResult>

            _AddOrUpdateChargingStationOperator(ChargingStationOperator                    ChargingStationOperator,
                                                Boolean                                    SkipNewChargingStationOperatorNotifications       = false,
                                                Boolean                                    SkipChargingStationOperatorUpdatedNotifications   = false,
                                                OnChargingStationOperatorAddedDelegate?    OnAdded                                           = null,
                                                OnChargingStationOperatorUpdatedDelegate?  OnUpdated                                         = null,
                                                EventTracking_Id?                          EventTrackingId                                   = null,
                                                User_Id?                                   CurrentUserId                                     = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            //if (ChargingStationOperator.API is not null && ChargingStationOperator.API != this)
            //    return AddOrUpdateChargingStationOperatorResult.ArgumentError(ChargingStationOperator,
            //                                               eventTrackingId,
            //                                               nameof(ChargingStationOperator.API),
            //                                               "The given user is already attached to another API!");

            if (ChargingStationOperator.Id.Length < MinChargingStationOperatorIdLength)
                return AddOrUpdateChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           Description:              $"The given charging station operator identification '{ChargingStationOperator.Id}' is too short!".ToI18NString(),
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            if (ChargingStationOperator.Name.IsNullOrEmpty())
                return AddOrUpdateChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          eventTrackingId,
                           Description:              $"The given charging station operator name must not be null or empty!".ToI18NString(),
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            if (ChargingStationOperator.Name.FirstText().Length < MinChargingStationOperatorNameLength)
                return AddOrUpdateChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          eventTrackingId,
                           Description:              $"The given charging station operator name '{ChargingStationOperator.Name}' is too short!".ToI18NString(),
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            //ChargingStationOperator.API = this;

            //await WriteToDatabaseFile(addOrUpdateChargingStationOperator_MessageType,
            //                          ChargingStationOperator.ToJSON(false),
            //                          eventTrackingId,
            //                          CurrentChargingStationOperatorId);

            if (chargingStationOperators.TryGet(ChargingStationOperator.Id, out var oldChargingStationOperator) &&
                oldChargingStationOperator is not null)
            {

                chargingStationOperators.TryRemove(oldChargingStationOperator.Id,
                                                   out _,
                                                   EventTracking_Id.New,
                                                   null);

                //ChargingStationOperator.CopyAllLinkedDataFrom(OldChargingStationOperator);

            }

            var result  = chargingStationOperators.TryAdd(ChargingStationOperator,
                                                          EventTracking_Id.New,
                                                          null);
            var now     = Timestamp.Now;

            if (oldChargingStationOperator is null)
            {

                OnAdded?.Invoke(now,
                                ChargingStationOperator,
                                eventTrackingId,
                                CurrentUserId);

                var OnChargingStationOperatorAddedLocal = OnChargingStationOperatorAdded;
                if (OnChargingStationOperatorAddedLocal is not null)
                    await OnChargingStationOperatorAddedLocal.Invoke(now,
                                                                     ChargingStationOperator,
                                                                     eventTrackingId,
                                                                     CurrentUserId);

                //if (!SkipNewChargingStationOperatorNotifications)
                //    await SendNotifications(ChargingStationOperator,
                //                            addChargingStationOperator_MessageType,
                //                            null,
                //                            eventTrackingId,
                //                            CurrentUserId);

                return AddOrUpdateChargingStationOperatorResult.Added(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            }
            else
            {

                OnUpdated?.Invoke(now,
                                  ChargingStationOperator,
                                  oldChargingStationOperator,
                                  eventTrackingId,
                                  CurrentUserId);

                var OnChargingStationOperatorUpdatedLocal = OnChargingStationOperatorUpdated;
                if (OnChargingStationOperatorUpdatedLocal is not null)
                    await OnChargingStationOperatorUpdatedLocal.Invoke(now,
                                                                       ChargingStationOperator,
                                                                       oldChargingStationOperator,
                                                                       eventTrackingId,
                                                                       CurrentUserId);

                //if (!SkipChargingStationOperatorUpdatedNotifications)
                //    await SendNotifications(ChargingStationOperator,
                //                            updateChargingStationOperator_MessageType,
                //                            OldChargingStationOperator,
                //                            eventTrackingId,
                //                            CurrentUserId);

                return AddOrUpdateChargingStationOperatorResult.Updated(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            }

        }

        #endregion

        #region AddOrUpdateChargingStationOperator                      (ChargingStationOperator, ..., OnAdded = null, OnUpdated = null, ...)

        /// <summary>
        /// Add or update the given user to/within the API.
        /// </summary>
        /// <param name="ChargingStationOperator">A user.</param>
        /// <param name="SkipNewChargingStationOperatorNotifications">Do not send notifications for this user addition.</param>
        /// <param name="SkipChargingStationOperatorUpdatedNotifications">Do not send the updated user information e-mail to the new user.</param>
        /// <param name="OnAdded">A delegate run whenever the user has been added successfully.</param>
        /// <param name="OnUpdated">A delegate run whenever the user has been updated successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddOrUpdateChargingStationOperatorResult>

            AddOrUpdateChargingStationOperator(ChargingStationOperator                    ChargingStationOperator,
                                               Boolean                                    SkipNewChargingStationOperatorNotifications       = false,
                                               Boolean                                    SkipChargingStationOperatorUpdatedNotifications   = false,
                                               OnChargingStationOperatorAddedDelegate?    OnAdded                                           = null,
                                               OnChargingStationOperatorUpdatedDelegate?  OnUpdated                                         = null,
                                               EventTracking_Id?                          EventTrackingId                                   = null,
                                               User_Id?                                   CurrentUserId                                     = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (await chargingStationOperatorsSemaphore.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    return await _AddOrUpdateChargingStationOperator(ChargingStationOperator,
                                                                     SkipNewChargingStationOperatorNotifications,
                                                                     SkipChargingStationOperatorUpdatedNotifications,
                                                                     OnAdded,
                                                                     OnUpdated,
                                                                     eventTrackingId,
                                                                     CurrentUserId);

                }
                catch (Exception e)
                {

                    return AddOrUpdateChargingStationOperatorResult.Error(
                               ChargingStationOperator:  ChargingStationOperator,
                               Exception:                e,
                               EventTrackingId:          eventTrackingId,
                               SenderId:                 Id,
                               Sender:                   this,
                               RoamingNetwork:           this
                           );

                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return AddOrUpdateChargingStationOperatorResult.LockTimeout(
                       ChargingStationOperator:  ChargingStationOperator,
                       Timeout:                  SemaphoreSlimTimeout,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion

        #endregion

        #region UpdateChargingStationOperator        ((New)ChargingStationOperator, ...,                 OnUpdated = null, ...)

        /// <summary>
        /// An event fired whenever a charging station operator was updated.
        /// </summary>
        public event OnChargingStationOperatorUpdatedDelegate? OnChargingStationOperatorUpdated;


        #region (protected internal) _UpdateChargingStationOperator(NewChargingStationOperator,                 ..., OnUpdated = null, ...)

        /// <summary>
        /// Update the given charging station operator to/within the API.
        /// </summary>
        /// <param name="NewChargingStationOperator">A charging station operator.</param>
        /// <param name="SkipChargingStationOperatorUpdatedNotifications">Do not send the updated charging station operator notifications.</param>
        /// <param name="OnUpdated">A delegate run whenever the charging station operator has been updated successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        protected internal async Task<UpdateChargingStationOperatorResult>

            _UpdateChargingStationOperator(ChargingStationOperator                    NewChargingStationOperator,
                                           Boolean                                    SkipChargingStationOperatorUpdatedNotifications   = false,
                                           OnChargingStationOperatorUpdatedDelegate?  OnUpdated                                         = null,
                                           EventTracking_Id?                          EventTrackingId                                   = null,
                                           User_Id?                                   CurrentUserId                                     = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (!_TryGetChargingStationOperatorById(NewChargingStationOperator.Id, out var oldChargingStationOperator) ||
                oldChargingStationOperator is null)
            {

                return UpdateChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  NewChargingStationOperator,
                           Description:              $"The given user '{NewChargingStationOperator.Id}' does not exists in this API!".ToI18NString(),
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );

            }

            //if (ChargingStationOperator.API is not null && ChargingStationOperator.API != this)
            //    return UpdateChargingStationOperatorResult.ArgumentError(ChargingStationOperator,
            //                                          eventTrackingId,
            //                                          nameof(ChargingStationOperator.API),
            //                                          "The given user is not attached to this API!");

            //ChargingStationOperator.API = this;

            //await WriteToDatabaseFile(updateChargingStationOperator_MessageType,
            //                          ChargingStationOperator.ToJSON(),
            //                          eventTrackingId,
            //                          CurrentChargingStationOperatorId);

            chargingStationOperators.TryRemove(oldChargingStationOperator.Id,
                                               out _,
                                               EventTracking_Id.New,
                                               null);

            //ChargingStationOperator.CopyAllLinkedDataFrom(oldChargingStationOperator);
            var result  = chargingStationOperators.TryAdd(NewChargingStationOperator,
                                                          EventTracking_Id.New,
                                                          null);
            var now     = Timestamp.Now;

            OnUpdated?.Invoke(now,
                              NewChargingStationOperator,
                              oldChargingStationOperator,
                              eventTrackingId,
                              CurrentUserId);

            var OnChargingStationOperatorUpdatedLocal = OnChargingStationOperatorUpdated;
            if (OnChargingStationOperatorUpdatedLocal is not null)
                await OnChargingStationOperatorUpdatedLocal.Invoke(Timestamp.Now,
                                                                   NewChargingStationOperator,
                                                                   oldChargingStationOperator,
                                                                   eventTrackingId,
                                                                   CurrentUserId);

            //if (!SkipChargingStationOperatorUpdatedNotifications)
            //    await SendNotifications(ChargingStationOperator,
            //                            updateChargingStationOperator_MessageType,
            //                            oldChargingStationOperator,
            //                            eventTrackingId,
            //                            CurrentUserId);

            return UpdateChargingStationOperatorResult.Success(
                       ChargingStationOperator:  NewChargingStationOperator,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion

        #region UpdateChargingStationOperator                      (NewChargingStationOperator,                 ..., OnUpdated = null, ...)

        /// <summary>
        /// Update the given charging station operator to/within the API.
        /// </summary>
        /// <param name="NewChargingStationOperator">A charging station operator.</param>
        /// <param name="SkipChargingStationOperatorUpdatedNotifications">Do not send the updated charging station operator notifications.</param>
        /// <param name="OnUpdated">A delegate run whenever the charging station operator has been updated successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<UpdateChargingStationOperatorResult>

            UpdateChargingStationOperator(ChargingStationOperator                    NewChargingStationOperator,
                                          Boolean                                    SkipChargingStationOperatorUpdatedNotifications   = false,
                                          OnChargingStationOperatorUpdatedDelegate?  OnUpdated                                         = null,
                                          EventTracking_Id?                          EventTrackingId                                   = null,
                                          User_Id?                                   CurrentUserId                                     = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (await chargingStationOperatorsSemaphore.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    return await _UpdateChargingStationOperator(NewChargingStationOperator,
                                                                SkipChargingStationOperatorUpdatedNotifications,
                                                                OnUpdated,
                                                                EventTrackingId,
                                                                CurrentUserId);

                }
                catch (Exception e)
                {

                    return UpdateChargingStationOperatorResult.Error(
                               ChargingStationOperator:  NewChargingStationOperator,
                               Exception:                e,
                               EventTrackingId:          eventTrackingId,
                               SenderId:                 Id,
                               Sender:                   this,
                               RoamingNetwork:           this
                           );

                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return UpdateChargingStationOperatorResult.LockTimeout(
                       ChargingStationOperator:  NewChargingStationOperator,
                       Timeout:                  SemaphoreSlimTimeout,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion


        #region (protected internal) _UpdateChargingStationOperator(ChargingStationOperator,    UpdateDelegate, ..., OnUpdated = null, ...)

        /// <summary>
        /// Update the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="UpdateDelegate">A delegate to update the given charging station operator.</param>
        /// <param name="SkipChargingStationOperatorUpdatedNotifications">Do not send the updated charging station operator notifications.</param>
        /// <param name="OnUpdated">A delegate run whenever the charging station operator has been updated successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        protected internal async Task<UpdateChargingStationOperatorResult>

            _UpdateChargingStationOperator(ChargingStationOperator                    ChargingStationOperator,
                                           Action<ChargingStationOperator.Builder>    UpdateDelegate,
                                           Boolean                                    SkipChargingStationOperatorUpdatedNotifications   = false,
                                           OnChargingStationOperatorUpdatedDelegate?  OnUpdated                                         = null,
                                           EventTracking_Id?                          EventTrackingId                                   = null,
                                           User_Id?                                   CurrentUserId                                     = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (!_ChargingStationOperatorExists(ChargingStationOperator.Id))
                return UpdateChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this,
                           Description:              I18NString.Create(
                                                         Languages.en,
                                                         $"The given user '{ChargingStationOperator.Id}' does not exists in this API!"
                                                     )
                       );

            //if (ChargingStationOperator.API != this)
            //    return UpdateChargingStationOperatorResult.ArgumentError(ChargingStationOperator,
            //                                          eventTrackingId,
            //                                          nameof(ChargingStationOperator.API),
            //                                          "The given user is not attached to this API!");

            if (UpdateDelegate is null)
                return UpdateChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this,
                           Description:              I18NString.Create(
                                                         Languages.en,
                                                         $"The given update delegate must not be null!"
                                                     )
                       );


            //var builder = ChargingStationOperator.ToBuilder();
            //UpdateDelegate(builder);
            //var updatedChargingStationOperator = builder.ToImmutable;
            var updatedChargingStationOperator = ChargingStationOperator;

            //await WriteToDatabaseFile(updateChargingStationOperator_MessageType,
            //                          updatedChargingStationOperator.ToJSON(),
            //                          eventTrackingId,
            //                          CurrentChargingStationOperatorId);

            chargingStationOperators.TryRemove(ChargingStationOperator.Id,
                                               out _,
                                               EventTracking_Id.New,
                                               null);

            //updatedChargingStationOperator.CopyAllLinkedDataFrom(ChargingStationOperator);
            var result  = chargingStationOperators.TryAdd(updatedChargingStationOperator,
                                                          EventTracking_Id.New,
                                                          null);
            var now     = Timestamp.Now;

            OnUpdated?.Invoke(now,
                              updatedChargingStationOperator,
                              ChargingStationOperator,
                              eventTrackingId,
                              CurrentUserId);

            var OnChargingStationOperatorUpdatedLocal = OnChargingStationOperatorUpdated;
            if (OnChargingStationOperatorUpdatedLocal is not null)
                await OnChargingStationOperatorUpdatedLocal.Invoke(now,
                                                                   updatedChargingStationOperator,
                                                                   ChargingStationOperator,
                                                                   eventTrackingId,
                                                                   CurrentUserId);

            //if (!SkipChargingStationOperatorUpdatedNotifications)
            //    await SendNotifications(updatedChargingStationOperator,
            //                            updateChargingStationOperator_MessageType,
            //                            ChargingStationOperator,
            //                            eventTrackingId,
            //                            CurrentUserId);

            return UpdateChargingStationOperatorResult.Success(
                       ChargingStationOperator:  ChargingStationOperator,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion

        #region UpdateChargingStationOperator                      (ChargingStationOperator,    UpdateDelegate, ..., OnUpdated = null, ...)

        /// <summary>
        /// Update the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="UpdateDelegate">A delegate to update the given charging station operator.</param>
        /// <param name="SkipChargingStationOperatorUpdatedNotifications">Do not send the updated charging station operator notifications.</param>
        /// <param name="OnUpdated">A delegate run whenever the charging station operator has been updated successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<UpdateChargingStationOperatorResult>

            UpdateChargingStationOperator(ChargingStationOperator                    ChargingStationOperator,
                                          Action<ChargingStationOperator.Builder>    UpdateDelegate,
                                          Boolean                                    SkipChargingStationOperatorUpdatedNotifications   = false,
                                          OnChargingStationOperatorUpdatedDelegate?  OnUpdated                                         = null,
                                          EventTracking_Id?                          EventTrackingId                                   = null,
                                          User_Id?                                   CurrentUserId                                     = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (await chargingStationOperatorsSemaphore.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    return await _UpdateChargingStationOperator(ChargingStationOperator,
                                                                UpdateDelegate,
                                                                SkipChargingStationOperatorUpdatedNotifications,
                                                                OnUpdated,
                                                                eventTrackingId,
                                                                CurrentUserId);

                }
                catch (Exception e)
                {

                    return UpdateChargingStationOperatorResult.Error(
                               ChargingStationOperator:  ChargingStationOperator,
                               Exception:                e,
                               EventTrackingId:          eventTrackingId,
                               SenderId:                 Id,
                               Sender:                   this,
                               RoamingNetwork:           this
                           );

                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return UpdateChargingStationOperatorResult.LockTimeout(
                       ChargingStationOperator:  ChargingStationOperator,
                       Timeout:                  SemaphoreSlimTimeout,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion

        #endregion

        #region RemoveChargingStationOperator        (ChargingStationOperator,      ...,                 OnRemoved = null, ...)

        /// <summary>
        /// An event fired whenever a user was removed.
        /// </summary>
        public event OnChargingStationOperatorRemovedDelegate? OnChargingStationOperatorRemoved;


        #region (protected internal virtual) _CanRemoveChargingStationOperator(ChargingStationOperator)

        /// <summary>
        /// Determines whether the charging station operator can safely be removed from the API.
        /// </summary>
        /// <param name="ChargingStationOperator">The charging station operator to be removed.</param>
        protected internal virtual I18NString? _CanRemoveChargingStationOperator(ChargingStationOperator ChargingStationOperator)
        {

            //if (ChargingStationOperator.ChargingStationOperator2Organization_OutEdges.Any())
            //    return new I18NString(Languages.en, "The user is still member of an organization!");

            return null;

        }

        #endregion

        #region (protected internal) _RemoveChargingStationOperator(ChargingStationOperator, SkipChargingStationOperatorRemovedNotifications = false, OnRemoved = null, ...)

        /// <summary>
        /// Remove the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">The charging station operator to be removed.</param>
        /// <param name="OnRemoved">A delegate run whenever the charging station operator has been removed successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        protected internal async Task<DeleteChargingStationOperatorResult>

            _RemoveChargingStationOperator(ChargingStationOperator                    ChargingStationOperator,
                                           Boolean                                    SkipChargingStationOperatorRemovedNotifications   = false,
                                           OnChargingStationOperatorRemovedDelegate?  OnRemoved                                         = null,
                                           EventTracking_Id?                          EventTrackingId                                   = null,
                                           User_Id?                                   CurrentUserId                                     = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            //if (ChargingStationOperator.API != this)
            //    return RemoveChargingStationOperatorResult.ArgumentError(
            //               ChargingStationOperator,
            //               eventTrackingId,
            //               nameof(ChargingStationOperator),
            //               "The given user is not attached to this API!"
            //           );

            if (!chargingStationOperators.ContainsId(ChargingStationOperator.Id))
                return DeleteChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator:  ChargingStationOperator,
                           Description:              $"The given user does not exists in this API!".ToI18NString(),
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this
                       );


            var canBeRemoved  = _CanRemoveChargingStationOperator(ChargingStationOperator);

            if (canBeRemoved is not null)
                return DeleteChargingStationOperatorResult.CanNotBeRemoved(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          eventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this,
                           RoamingNetwork:           this,
                           Description:              canBeRemoved
                       );


            //await WriteToDatabaseFile(deleteChargingStationOperator_MessageType,
            //                          ChargingStationOperator.ToJSON(false),
            //                          eventTrackingId,
            //                          CurrentChargingStationOperatorId);


            // ToDo: Remove incoming edges


            var result  = chargingStationOperators.TryRemove(ChargingStationOperator.Id,
                                                             out _,
                                                             EventTracking_Id.New,
                                                             null);
            var now     = Timestamp.Now;

            OnRemoved?.Invoke(now,
                              ChargingStationOperator,
                              eventTrackingId,
                              CurrentUserId);

            var OnChargingStationOperatorRemovedLocal = OnChargingStationOperatorRemoved;
            if (OnChargingStationOperatorRemovedLocal is not null)
                await OnChargingStationOperatorRemovedLocal.Invoke(Timestamp.Now,
                                                                   ChargingStationOperator,
                                                                   eventTrackingId,
                                                                   CurrentUserId);

            //if (!SkipChargingStationOperatorRemovedNotifications)
            //    await SendNotifications(ChargingStationOperator,
            //                            parentOrganizations,
            //                            deleteChargingStationOperator_MessageType,
            //                            eventTrackingId,
            //                            CurrentChargingStationOperatorId);


            return DeleteChargingStationOperatorResult.Success(
                       ChargingStationOperator:  ChargingStationOperator,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion

        #region RemoveChargingStationOperator                      (ChargingStationOperator, SkipChargingStationOperatorRemovedNotifications = false, OnRemoved = null, ...)

        /// <summary>
        /// Remove the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">The charging station operator to be removed.</param>
        /// <param name="OnRemoved">A delegate run whenever the charging station operator has been removed successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<DeleteChargingStationOperatorResult>

            RemoveChargingStationOperator(ChargingStationOperator                    ChargingStationOperator,
                                          Boolean                                    SkipChargingStationOperatorRemovedNotifications   = false,
                                          OnChargingStationOperatorRemovedDelegate?  OnRemoved                                         = null,
                                          EventTracking_Id?                          EventTrackingId                                   = null,
                                          User_Id?                                   CurrentUserId                                     = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            if (await chargingStationOperatorsSemaphore.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    return await _RemoveChargingStationOperator(ChargingStationOperator,
                                                                SkipChargingStationOperatorRemovedNotifications,
                                                                OnRemoved,
                                                                eventTrackingId,
                                                                CurrentUserId);

                }
                catch (Exception e)
                {

                    return DeleteChargingStationOperatorResult.Error(
                               ChargingStationOperator:  ChargingStationOperator,
                               Exception:                e,
                               EventTrackingId:          eventTrackingId,
                               SenderId:                 Id,
                               Sender:                   this,
                               RoamingNetwork:           this
                           );

                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return DeleteChargingStationOperatorResult.LockTimeout(
                       ChargingStationOperator:  ChargingStationOperator,
                       Timeout:                  SemaphoreSlimTimeout,
                       EventTrackingId:          eventTrackingId,
                       SenderId:                 Id,
                       Sender:                   this,
                       RoamingNetwork:           this
                   );

        }

        #endregion

        #endregion


        #region ChargingStationOperatorExists    (ChargingStationOperator)

        /// <summary>
        /// Check if the given charging station operator identification is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of the charging station operator.</param>
        public Boolean ChargingStationOperatorExists(IChargingStationOperator  ChargingStationOperator)
        {

            if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _ChargingStationOperatorExists(ChargingStationOperator.Id);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, $"{nameof(RoamingNetwork)}.{nameof(ChargingStationOperatorExists)}({ChargingStationOperator.Id}, ...)");
                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return false;

        }

        #endregion

        #region ChargingStationOperatorExists    (ChargingStationOperatorId)

        /// <summary>
        /// Determines whether the given user identification exists within this API.
        /// </summary>
        /// <param name="UserId">The unique identification of an user.</param>
        protected internal Boolean _ChargingStationOperatorExists(ChargingStationOperator_Id UserId)

            => UserId.IsNotNullOrEmpty &&
               chargingStationOperators.ContainsId(UserId);

        /// <summary>
        /// Determines whether the given user identification exists within this API.
        /// </summary>
        /// <param name="UserId">The unique identification of an user.</param>
        protected internal Boolean _ChargingStationOperatorExists(ChargingStationOperator_Id? UserId)

            => UserId.HasValue &&
               UserId.IsNotNullOrEmpty() &&
               chargingStationOperators.ContainsId(UserId.Value);


        /// <summary>
        /// Check if the given charging station operator identification is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of the charging station operator.</param>
        public Boolean ChargingStationOperatorExists(ChargingStationOperator_Id ChargingStationOperatorId)
        {

            if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _ChargingStationOperatorExists(ChargingStationOperatorId);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, $"{nameof(RoamingNetwork)}.{nameof(ChargingStationOperatorExists)}({ChargingStationOperatorId}, ...)");
                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return false;

        }

        /// <summary>
        /// Check if the given charging station operator identification is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of the charging station operator.</param>
        public Boolean ChargingStationOperatorExists(ChargingStationOperator_Id? ChargingStationOperatorId)
        {

            if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _ChargingStationOperatorExists(ChargingStationOperatorId);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, $"{nameof(RoamingNetwork)}.{nameof(ChargingStationOperatorExists)}({ChargingStationOperatorId}, ...)");
                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return false;

        }

        #endregion

        #region GetChargingStationOperatorById   (ChargingStationOperatorId)

        /// <summary>
        /// Get the charging station operator having the given unique identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        protected internal IChargingStationOperator? _GetChargingStationOperatorById(ChargingStationOperator_Id ChargingStationOperatorId)
        {

            if (!ChargingStationOperatorId.IsNullOrEmpty &&
                chargingStationOperators.TryGet(ChargingStationOperatorId, out var chargingStationOperator))
            {
                return chargingStationOperator;
            }

            return null;

        }

        /// <summary>
        /// Get the charging station operator having the given unique identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        protected internal IChargingStationOperator? _GetChargingStationOperatorById(ChargingStationOperator_Id? ChargingStationOperatorId)
        {

            if (ChargingStationOperatorId.HasValue &&
                ChargingStationOperatorId.IsNotNullOrEmpty() &&
                chargingStationOperators.TryGet(ChargingStationOperatorId.Value, out var chargingStationOperator))
            {
                return chargingStationOperator;
            }

            return null;

        }


        /// <summary>
        /// Get the charging station operator having the given unique identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        public IChargingStationOperator? GetChargingStationOperatorById(ChargingStationOperator_Id ChargingStationOperatorId)
        {

            if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _GetChargingStationOperatorById(ChargingStationOperatorId);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, $"{nameof(RoamingNetwork)}.{nameof(GetChargingStationOperatorById)}({ChargingStationOperatorId}, ...)");
                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return null;

        }

        /// <summary>
        /// Get the charging station operator having the given unique identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        public IChargingStationOperator? GetChargingStationOperatorById(ChargingStationOperator_Id? ChargingStationOperatorId)
        {

            if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _GetChargingStationOperatorById(ChargingStationOperatorId);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, $"{nameof(RoamingNetwork)}.{nameof(GetChargingStationOperatorById)}({ChargingStationOperatorId}, ...)");
                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return null;

        }

        #endregion

        #region TryGetChargingStationOperatorById(ChargingStationOperatorId, out ChargingStationOperator)

        /// <summary>
        /// Try to get the charging station operator having the given unique identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        protected internal Boolean _TryGetChargingStationOperatorById(ChargingStationOperator_Id ChargingStationOperatorId, out IChargingStationOperator? ChargingStationOperator)
        {

            if (!ChargingStationOperatorId.IsNullOrEmpty &&
                chargingStationOperators.TryGet(ChargingStationOperatorId, out ChargingStationOperator))
            {
                return true;
            }

            ChargingStationOperator = null;
            return false;

        }

        /// <summary>
        /// Try to get the charging station operator having the given unique identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        protected internal Boolean _TryGetChargingStationOperatorById(ChargingStationOperator_Id? ChargingStationOperatorId, out IChargingStationOperator? ChargingStationOperator)
        {

            if (ChargingStationOperatorId.HasValue &&
                ChargingStationOperatorId.IsNotNullOrEmpty() &&
                chargingStationOperators.TryGet(ChargingStationOperatorId.Value, out ChargingStationOperator))
            {
                return true;
            }

            ChargingStationOperator = null;
            return false;

        }


        /// <summary>
        /// Try to get the charging station operator having the given unique identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        public Boolean TryGetChargingStationOperatorById(ChargingStationOperator_Id ChargingStationOperatorId, out IChargingStationOperator? ChargingStationOperator)
        {

            if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _TryGetChargingStationOperatorById(ChargingStationOperatorId, out ChargingStationOperator);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, $"{nameof(RoamingNetwork)}.{nameof(TryGetChargingStationOperatorById)}({ChargingStationOperatorId}, ...)");
                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            ChargingStationOperator = null;
            return false;

        }

        /// <summary>
        /// Try to get the charging station operator having the given unique identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        /// <param name="ChargingStationOperator">The charging station operator.</param>
        public Boolean TryGetChargingStationOperatorById(ChargingStationOperator_Id? ChargingStationOperatorId, out IChargingStationOperator? ChargingStationOperator)
        {

            if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    return _TryGetChargingStationOperatorById(ChargingStationOperatorId, out ChargingStationOperator);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, $"{nameof(RoamingNetwork)}.{nameof(TryGetChargingStationOperatorById)}({ChargingStationOperatorId}, ...)");
                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            ChargingStationOperator = null;
            return false;

        }

        #endregion


        #region ChargingStationOperatorAdminStatus(IncludeChargingStationOperator = null)

        /// <summary>
        /// Return the admin status of all charging station operators registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingStationOperator">An optional delegate for filtering charging station operators.</param>
        public IEnumerable<ChargingStationOperatorAdminStatus> ChargingStationOperatorAdminStatus(IncludeChargingStationOperatorDelegate? IncludeChargingStationOperator = null)
        {

            if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    IncludeChargingStationOperator ??= (chargingStationOperator => true);

                    return chargingStationOperators.
                               Where (chargingStationOperator => IncludeChargingStationOperator(chargingStationOperator)).
                               Select(chargingStationOperator => new ChargingStationOperatorAdminStatus(chargingStationOperator.Id,
                                                                                                        chargingStationOperator.AdminStatus));

                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return Array.Empty<ChargingStationOperatorAdminStatus>();

        }

        #endregion

        #region (internal) UpdateChargingStationOperatorAdminStatus(Timestamp, EventTrackingId, ChargingStationOperator, NewStatus, OldStatus, DataSource)

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated Charging Station Operator changed.
        /// </summary>
        public event OnChargingStationOperatorAdminStatusChangedDelegate? OnChargingStationOperatorAdminStatusChanged;


        /// <summary>
        /// Update a charging station operator admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStationOperator">The updated charging station operator.</param>
        /// <param name="OldAdminStatus">The old charging station operator admin status.</param>
        /// <param name="NewAdminStatus">The new charging station operator admin status.</param>
        internal async Task UpdateChargingStationOperatorAdminStatus(DateTime                                              Timestamp,
                                                                     EventTracking_Id                                      EventTrackingId,
                                                                     IChargingStationOperator                              ChargingStationOperator,
                                                                     Timestamped<ChargingStationOperatorAdminStatusTypes>  OldAdminStatus,
                                                                     Timestamped<ChargingStationOperatorAdminStatusTypes>  NewAdminStatus)
        {

            try
            {

                var results = allSendAdminStatus.WhenAll(iSendAdminStatus => iSendAdminStatus.UpdateChargingStationOperatorAdminStatus(new[] {
                                                                                                                                          new ChargingStationOperatorAdminStatusUpdate(
                                                                                                                                              ChargingStationOperator.Id,
                                                                                                                                              OldAdminStatus,
                                                                                                                                              NewAdminStatus
                                                                                                                                          )
                                                                                                                                      },
                                                                                                                                      EventTrackingId: EventTrackingId));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateEVSEAdminStatus of charging station operator '{ChargingStationOperator.Id}' from '{OldAdminStatus}' to '{NewAdminStatus}'");
            }


            var OnChargingStationOperatorAdminStatusChangedLocal = OnChargingStationOperatorAdminStatusChanged;
            if (OnChargingStationOperatorAdminStatusChangedLocal is not null)
                await OnChargingStationOperatorAdminStatusChangedLocal(Timestamp,
                                                                       EventTrackingId ?? EventTracking_Id.New,
                                                                       ChargingStationOperator,
                                                                       OldAdminStatus,
                                                                       NewAdminStatus);

        }

        #endregion


        #region ChargingStationOperatorStatus     (IncludeChargingStationOperator = null)

        /// <summary>
        /// Return the status of all charging station operators registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingStationOperator">An optional delegate for filtering charging station operators.</param>
        public IEnumerable<ChargingStationOperatorStatus> ChargingStationOperatorStatus(IncludeChargingStationOperatorDelegate? IncludeChargingStationOperator = null)

        {

            if (chargingStationOperatorsSemaphore.Wait(SemaphoreSlimTimeout))
            {
                try
                {

                    IncludeChargingStationOperator ??= (chargingStationOperator => true);

                    return chargingStationOperators.
                               Where (chargingStationOperator => IncludeChargingStationOperator(chargingStationOperator)).
                               Select(chargingStationOperator => new ChargingStationOperatorStatus(chargingStationOperator.Id,
                                                                                                   chargingStationOperator.Status));

                }
                finally
                {
                    try
                    {
                        chargingStationOperatorsSemaphore.Release();
                    }
                    catch
                    { }
                }
            }

            return Array.Empty<ChargingStationOperatorStatus>();

        }

        #endregion

        #region (internal) UpdateChargingStationOperatorStatus     (Timestamp, EventTrackingId, ChargingStationOperator, NewStatus, OldStatus, DataSource)

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated Charging Station Operator changed.
        /// </summary>
        public event OnChargingStationOperatorStatusChangedDelegate? OnChargingStationOperatorStatusChanged;


        /// <summary>
        /// Update a charging station operator status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStationOperator">The updated charging station operator.</param>
        /// <param name="OldStatus">The old charging station operator status.</param>
        /// <param name="NewStatus">The new charging station operator status.</param>
        internal async Task UpdateChargingStationOperatorStatus(DateTime                                         Timestamp,
                                                                EventTracking_Id                                 EventTrackingId,
                                                                IChargingStationOperator                         ChargingStationOperator,
                                                                Timestamped<ChargingStationOperatorStatusTypes>  OldStatus,
                                                                Timestamped<ChargingStationOperatorStatusTypes>  NewStatus)
        {

            try
            {

                var results = allSendStatus.WhenAll(iSendStatus => iSendStatus.UpdateChargingStationOperatorStatus(new[] {
                                                                                                                      new ChargingStationOperatorStatusUpdate(
                                                                                                                          ChargingStationOperator.Id,
                                                                                                                          OldStatus,
                                                                                                                          NewStatus
                                                                                                                      )
                                                                                                                  },
                                                                                                                  EventTrackingId: EventTrackingId));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateChargingStationOperatorStatus of charging station operator '{ChargingStationOperator.Id}' from '{OldStatus}' to '{NewStatus}'");
            }


            var OnChargingStationOperatorStatusChangedLocal = OnChargingStationOperatorStatusChanged;
            if (OnChargingStationOperatorStatusChangedLocal is not null)
                await OnChargingStationOperatorStatusChangedLocal(Timestamp,
                                                                  EventTrackingId ?? EventTracking_Id.New,
                                                                  ChargingStationOperator,
                                                                  OldStatus,
                                                                  NewStatus);

        }

        #endregion

        #endregion

        #region ChargingPools...

        #region ChargingPools

        /// <summary>
        /// Return all charging pools registered within this roaming network.
        /// </summary>
        public IEnumerable<IChargingPool> ChargingPools

            => chargingStationOperators.SelectMany(cso => cso.ChargingPools);

        #endregion

        #region ChargingPoolIds(IncludeChargingPools = null)

        /// <summary>
        /// Return all charging pool identifications registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPool_Id> ChargingPoolIds(IncludeChargingPoolDelegate? IncludeChargingPools = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingPoolIds(IncludeChargingPools));

        #endregion


        #region ChargingPoolAddition

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, Boolean> ChargingPoolAddition;

        /// <summary>
        /// Called whenever an EVS pool will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, Boolean> OnChargingPoolAddition
            => ChargingPoolAddition;


        private void SendChargingPoolAdded(DateTime                  Timestamp,
                                           EventTracking_Id          EventTrackingId,
                                           User_Id                   UserId,
                                           IChargingStationOperator  ChargingStationOperator,
                                           IChargingPool             ChargingPool)
        {

            ChargingPoolAddition.SendNotification(Timestamp,
                                                  EventTrackingId,
                                                  UserId,
                                                  ChargingStationOperator,
                                                  ChargingPool);

            try
            {

                var results = allSendChargingPoolData.WhenAll(target => target.AddChargingPool(ChargingPool));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.SendChargingPoolAdded of charging pool '{ChargingPool.Id}' to charging station operator '{ChargingStationOperator.Id}'");
            }

        }

        #endregion

        #region (internal) UpdateChargingPoolData       (Timestamp, EventTrackingId, ChargingPool, PropertyName, NewValue, OldValue, DataSource)

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolDataChangedDelegate? OnChargingPoolDataChanged;


        /// <summary>
        /// Update the data of a charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The optional old value of the changed property.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool data update.</param>
        internal async Task UpdateChargingPoolData(DateTime          Timestamp,
                                                   EventTracking_Id  EventTrackingId,
                                                   IChargingPool     ChargingPool,
                                                   String            PropertyName,
                                                   Object?           NewValue,
                                                   Object?           OldValue,
                                                   Context?          DataSource)
        {

            var onChargingPoolDataChanged = OnChargingPoolDataChanged;
            if (onChargingPoolDataChanged is not null)
                await onChargingPoolDataChanged(Timestamp,
                                                EventTrackingId,
                                                ChargingPool,
                                                PropertyName,
                                                NewValue,
                                                OldValue,
                                                DataSource);

            try
            {

                var results = allSendChargingPoolData.WhenAll(target => target.UpdateChargingPool(ChargingPool,
                                                                                                  PropertyName,
                                                                                                  NewValue,
                                                                                                  OldValue,
                                                                                                  DataSource));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateChargingPoolData of charging pool '{ChargingPool.Id}' property '{PropertyName}' from '{OldValue?.ToString() ?? "-"}' to '{NewValue?.ToString() ?? "-"}'");
            }

        }

        #endregion

        #region ChargingPoolRemoval

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, Boolean> ChargingPoolRemoval;

        /// <summary>
        /// Called whenever an EVS pool will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, Boolean> OnChargingPoolRemoval
            => ChargingPoolRemoval;


        private void SendChargingPoolRemoved(DateTime                  Timestamp,
                                             EventTracking_Id          EventTrackingId,
                                             User_Id                   UserId,
                                             IChargingStationOperator  ChargingStationOperator,
                                             IChargingPool             ChargingPool)
        {

            ChargingPoolRemoval.SendNotification(Timestamp,
                                                 EventTrackingId,
                                                 UserId,
                                                 ChargingStationOperator,
                                                 ChargingPool);

            try
            {

                var results = allSendChargingPoolData.WhenAll(target => target.DeleteChargingPool(ChargingPool));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.SendChargingPoolRemoved of charging pool '{ChargingPool.Id}' from charging station operator '{ChargingStationOperator.Id}'");
            }

        }

        #endregion


        #region ContainsChargingPool(ChargingPool)

        /// <summary>
        /// Check if the given charging pool is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public Boolean ContainsChargingPool(IChargingPool ChargingPool)
        {

            if (TryGetChargingStationOperatorById(ChargingPool.Operator.Id, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.ChargingPoolExists(ChargingPool.Id);
            }

            return false;

        }

        #endregion

        #region ContainsChargingPool(ChargingPoolId)

        /// <summary>
        /// Check if the given charging pool identification is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification.</param>
        public Boolean ContainsChargingPool(ChargingPool_Id ChargingPoolId)
        {

            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.ChargingPoolExists(ChargingPoolId);
            }

            return false;

        }

        #endregion

        #region GetChargingPoolbyId(ChargingPoolId)

        public IChargingPool? GetChargingPoolById(ChargingPool_Id ChargingPoolId)
        {

            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId,   out var chargingStationOperator) &&
                chargingStationOperator is not null                                                             &&
                chargingStationOperator.TryGetChargingPoolById(ChargingPoolId, out var chargingPool))
            {
                return chargingPool;
            }

            return null;

        }

        public IChargingPool? GetChargingPoolById(ChargingPool_Id? ChargingPoolId)
        {

            if (ChargingPoolId.HasValue &&
                TryGetChargingStationOperatorById(ChargingPoolId.Value.OperatorId,   out var chargingStationOperator) &&
                chargingStationOperator is not null                                                                   &&
                chargingStationOperator.TryGetChargingPoolById(ChargingPoolId.Value, out var chargingPool))
            {
                return chargingPool;
            }

            return null;

        }

        #endregion

        #region TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool)

        public Boolean TryGetChargingPoolById(ChargingPool_Id ChargingPoolId, out IChargingPool? ChargingPool)
        {

            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.TryGetChargingPoolById(ChargingPoolId, out ChargingPool);
            }

            ChargingPool = null;
            return false;

        }

        public Boolean TryGetChargingPoolById(ChargingPool_Id? ChargingPoolId, out IChargingPool? ChargingPool)
        {

            if (ChargingPoolId.HasValue &&
                TryGetChargingStationOperatorById(ChargingPoolId.Value.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.TryGetChargingPoolById(ChargingPoolId.Value, out ChargingPool);
            }

            ChargingPool = null;
            return false;

        }

        #endregion


        #region SetChargingPoolAdminStatus(ChargingPoolId, StatusList)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                                         ChargingPoolId,
                                               IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>  StatusList)
        {

            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                chargingStationOperator.SetChargingPoolAdminStatus(ChargingPoolId, StatusList);
            }

        }

        #endregion


        #region SendChargingPoolAdminStatusDiff(StatusDiff)

        internal void SendChargingPoolAdminStatusDiff(ChargingPoolAdminStatusDiff StatusDiff)
        {
            OnChargingPoolAdminDiff?.Invoke(StatusDiff);
        }

        #endregion


        #region OnChargingPoolAdminDiff

        public delegate void OnChargingPoolAdminDiffDelegate(ChargingPoolAdminStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a charging station admin status diff was received.
        /// </summary>
        public event OnChargingPoolAdminDiffDelegate? OnChargingPoolAdminDiff;

        #endregion


        #region ChargingPoolAdminStatus        (IncludeChargingPools = null)

        /// <summary>
        /// Return the admin status of all charging pools registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPoolAdminStatus> ChargingPoolAdminStatus(IncludeChargingPoolDelegate? IncludeChargingPools = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingPoolAdminStatus(IncludeChargingPools));

        #endregion

        #region ChargingPoolAdminStatusSchedule(IncludeChargingPools = null, TimestampFilter  = null, StatusFilter = null, Skip = null, Take = null)

        /// <summary>
        /// Return the admin status of all charging pools registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="AdminStatusFilter">An optional admin status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>>>

            ChargingPoolAdminStatusSchedule(IncludeChargingPoolDelegate?                  IncludeChargingPools   = null,
                                            Func<DateTime,                     Boolean>?  TimestampFilter        = null,
                                            Func<ChargingPoolAdminStatusTypes, Boolean>?  AdminStatusFilter      = null,
                                            UInt64?                                       Skip                   = null,
                                            UInt64?                                       Take                   = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingPoolAdminStatusSchedule(IncludeChargingPools,
                                                                         TimestampFilter,
                                                                         AdminStatusFilter,
                                                                         Skip,
                                                                         Take));

        #endregion

        #region (internal) UpdateChargingPoolAdminStatus(Timestamp, EventTrackingId, ChargingPool, NewStatus, OldStatus, DataSource)

        /// <summary>
        /// An event fired whenever the admin status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate? OnChargingPoolAdminStatusChanged;


        /// <summary>
        /// Update a charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="NewAdminStatus">The new charging pool admin status.</param>
        /// <param name="OldAdminStatus">The optional old charging pool admin status.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool admin status update.</param>
        internal async Task UpdateChargingPoolAdminStatus(DateTime                                    Timestamp,
                                                          EventTracking_Id                            EventTrackingId,
                                                          IChargingPool                               ChargingPool,
                                                          Timestamped<ChargingPoolAdminStatusTypes>   NewAdminStatus,
                                                          Timestamped<ChargingPoolAdminStatusTypes>?  OldAdminStatus   = null,
                                                          Context?                                    DataSource       = null)
        {

            try
            {

                var results = allSendAdminStatus.WhenAll(iSendAdminStatus => iSendAdminStatus.UpdateChargingPoolAdminStatus(new[] {
                                                                                                                               new ChargingPoolAdminStatusUpdate(
                                                                                                                                   ChargingPool.Id,
                                                                                                                                   NewAdminStatus,
                                                                                                                                   OldAdminStatus,
                                                                                                                                   DataSource
                                                                                                                               )
                                                                                                                           },
                                                                                                                           EventTrackingId: EventTrackingId));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateEVSEAdminStatus of charging pool '{ChargingPool.Id}' from '{OldAdminStatus}' to '{NewAdminStatus}'");
            }


            var onChargingPoolAdminStatusChanged = OnChargingPoolAdminStatusChanged;
            if (onChargingPoolAdminStatusChanged is not null)
                await onChargingPoolAdminStatusChanged(Timestamp,
                                                       EventTrackingId,
                                                       ChargingPool,
                                                       NewAdminStatus,
                                                       OldAdminStatus,
                                                       DataSource);

        }

        #endregion


        #region ChargingPoolStatus             (IncludeChargingPools = null)

        /// <summary>
        /// Return the status of all charging pools registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPoolStatus> ChargingPoolStatus(IncludeChargingPoolDelegate? IncludeChargingPools = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingPoolStatus(IncludeChargingPools));

        #endregion

        #region ChargingPoolStatusSchedule     (IncludeChargingPools = null, TimestampFilter  = null, StatusFilter = null, Skip = null, Take = null)

        /// <summary>
        /// Return the admin status of all charging pools registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusTypes>>>>

            ChargingPoolStatusSchedule(IncludeChargingPoolDelegate?             IncludeChargingPools   = null,
                                       Func<DateTime,                Boolean>?  TimestampFilter        = null,
                                       Func<ChargingPoolStatusTypes, Boolean>?  StatusFilter           = null,
                                       UInt64?                                  Skip                   = null,
                                       UInt64?                                  Take                   = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingPoolStatusSchedule(IncludeChargingPools,
                                                                    TimestampFilter,
                                                                    StatusFilter,
                                                                    Skip,
                                                                    Take));

        #endregion

        #region (internal) UpdateChargingPoolStatus     (Timestamp, EventTrackingId, ChargingPool, NewStatus, OldStatus, DataSource)

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolStatusChangedDelegate? OnChargingPoolStatusChanged;


        /// <summary>
        /// Update a charging pool status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="NewStatus">The new charging pool status.</param>
        /// <param name="OldStatus">The optional old charging pool status.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool status update.</param>
        internal async Task UpdateChargingPoolStatus(DateTime                               Timestamp,
                                                     EventTracking_Id                       EventTrackingId,
                                                     IChargingPool                          ChargingPool,
                                                     Timestamped<ChargingPoolStatusTypes>   NewStatus,
                                                     Timestamped<ChargingPoolStatusTypes>?  OldStatus    = null,
                                                     Context?                               DataSource   = null)
        {

            try
            {

                var results = allSendStatus.WhenAll(iSendStatus => iSendStatus.UpdateChargingPoolStatus(new[] {
                                                                                                           new ChargingPoolStatusUpdate(
                                                                                                               ChargingPool.Id,
                                                                                                               NewStatus,
                                                                                                               OldStatus,
                                                                                                               DataSource
                                                                                                           )
                                                                                                       },
                                                                                                       EventTrackingId: EventTrackingId));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateChargingPoolStatus of charging pool '{ChargingPool.Id}' from '{OldStatus}' to '{NewStatus}'");
            }


            var onChargingPoolStatusChanged = OnChargingPoolStatusChanged;
            if (onChargingPoolStatusChanged is not null)
                await onChargingPoolStatusChanged(Timestamp,
                                                  EventTrackingId,
                                                  ChargingPool,
                                                  NewStatus,
                                                  OldStatus,
                                                  DataSource);

        }

        #endregion

        #endregion

        #region ChargingStations...

        #region ChargingStations

        /// <summary>
        /// Return all charging stations registered within this roaming network.
        /// </summary>
        public IEnumerable<IChargingStation> ChargingStations

            => chargingStationOperators.SelectMany(cso => cso.ChargingStations);

        #endregion

        #region ChargingStationIds(IncludeStations = null)

        /// <summary>
        /// Return all charging station identifications registered within this roaming network.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate? IncludeStations = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingStationIds(IncludeStations));

        #endregion


        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> OnChargingStationAddition

            => ChargingStationAddition;

        private void SendChargingStationAdded(DateTime          Timestamp,
                                              EventTracking_Id  EventTrackingId,
                                              User_Id           UserId,
                                              IChargingPool     ChargingPool,
                                              IChargingStation  ChargingStation)
        {

            ChargingStationAddition.SendNotification(Timestamp,
                                                     EventTrackingId,
                                                     UserId,
                                                     ChargingPool,
                                                     ChargingStation);

            try
            {

                var results = allSendChargingStationData.WhenAll(target => target.AddChargingStation(ChargingStation,
                                                                                                     EventTrackingId: EventTrackingId));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.SendChargingStationAdded of charging station '{ChargingStation.Id}' to charging pool '{ChargingPool.Id}'");
            }

        }

        #endregion

        #region (internal) UpdateChargingStationData       (Timestamp, EventTrackingId, ChargingStation, PropertyName, NewValue, OldValue, DataSource)

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationDataChangedDelegate? OnChargingStationDataChanged;


        /// <summary>
        /// Update the data of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        internal async Task UpdateChargingStationData(DateTime          Timestamp,
                                                      EventTracking_Id  EventTrackingId,
                                                      IChargingStation  ChargingStation,
                                                      String            PropertyName,
                                                      Object?           NewValue,
                                                      Object?           OldValue,
                                                      Context?          DataSource)
        {


            var onChargingStationDataChanged = OnChargingStationDataChanged;
            if (onChargingStationDataChanged is not null)
                await onChargingStationDataChanged(Timestamp,
                                                   EventTrackingId,
                                                   ChargingStation,
                                                   PropertyName,
                                                   NewValue,
                                                   OldValue,
                                                   DataSource);

            try
            {

                var results = allSendChargingStationData.WhenAll(iSendData => iSendData.UpdateChargingStation(ChargingStation,
                                                                                                              PropertyName,
                                                                                                              NewValue,
                                                                                                              OldValue,
                                                                                                              DataSource));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateChargingStationData of charging station '{ChargingStation.Id}' property '{PropertyName}' from '{OldValue?.ToString() ?? "-"}' to '{NewValue?.ToString() ?? "-"}'");
            }

        }

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> OnChargingStationRemoval

            => ChargingStationRemoval;


        private void SendChargingStationRemoved(DateTime          Timestamp,
                                                EventTracking_Id  EventTrackingId,
                                                User_Id           UserId,
                                                IChargingPool     ChargingPool,
                                                IChargingStation  ChargingStation)
        {

            ChargingStationRemoval.SendNotification(Timestamp,
                                                    EventTrackingId,
                                                    UserId,
                                                    ChargingPool,
                                                    ChargingStation);

            try
            {

                var results = allSendChargingStationData.WhenAll(target => target.DeleteChargingStation(ChargingStation));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.SendChargingStationRemoved of charging station '{ChargingStation.Id}' from charging pool '{ChargingPool.Id}'");
            }

        }

        #endregion


        #region ContainsChargingStation      (ChargingStation)

        /// <summary>
        /// Check if the given charging station is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStation(IChargingStation ChargingStation)
        {

            if (ChargingStation.Operator is not null                                                            &&
                TryGetChargingStationOperatorById(ChargingStation.Operator.Id, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.ContainsChargingStation(ChargingStation.Id);
            }

            return false;

        }

        #endregion

        #region ContainsChargingStation      (ChargingStationId)

        /// <summary>
        /// Check if the given charging station identification is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)
        {

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.ContainsChargingStation(ChargingStationId);
            }

            return false;

        }

        #endregion

        #region GetChargingStationbyId       (ChargingStationId)

        public IChargingStation? GetChargingStationById(ChargingStation_Id ChargingStationId)
        {

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId,      out var chargingStationOperator) &&
                chargingStationOperator is not null                                                                   &&
                chargingStationOperator.TryGetChargingStationById(ChargingStationId, out var chargingStation))
            {
                return chargingStation;
            }

            return null;

        }

        public IChargingStation? GetChargingStationById(ChargingStation_Id? ChargingStationId)
        {

            if (ChargingStationId.HasValue &&
                TryGetChargingStationOperatorById(ChargingStationId.Value.OperatorId,      out var chargingStationOperator) &&
                chargingStationOperator is not null                                                                         &&
                chargingStationOperator.TryGetChargingStationById(ChargingStationId.Value, out var chargingStation))
            {
                return chargingStation;
            }

            return null;

        }

        #endregion

        #region TryGetChargingStationbyId    (ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationById(ChargingStation_Id     ChargingStationId,
                                                 out IChargingStation?  ChargingStation)
        {

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.TryGetChargingStationById(ChargingStationId, out ChargingStation);
            }

            ChargingStation = null;
            return false;

        }

        public Boolean TryGetChargingStationById(ChargingStation_Id?    ChargingStationId,
                                                 out IChargingStation?  ChargingStation)
        {

            if (ChargingStationId.HasValue                                                                             &&
                TryGetChargingStationOperatorById(ChargingStationId.Value.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.TryGetChargingStationById(ChargingStationId.Value, out ChargingStation);
            }

            ChargingStation = null;
            return false;

        }

        #endregion


        #region SetChargingStationAdminStatus(ChargingStationId, CurrentAdminStatus)

        public void SetChargingStationAdminStatus(ChargingStation_Id                            ChargingStationId,
                                                  Timestamped<ChargingStationAdminStatusTypes>  CurrentAdminStatus)
        {

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                chargingStationOperator.SetChargingStationAdminStatus(ChargingStationId,
                                                                      CurrentAdminStatus);
            }

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, CurrentAdminStatusList)

        public void SetChargingStationAdminStatus(ChargingStation_Id                                         ChargingStationId,
                                                  IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>  CurrentAdminStatusList)
        {

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                chargingStationOperator.SetChargingStationAdminStatus(ChargingStationId,
                                                                      CurrentAdminStatusList);
            }

        }

        #endregion

        #region SetChargingStationStatus     (ChargingStationId, CurrentStatus)

        public void SetChargingStationStatus(ChargingStation_Id                       ChargingStationId,
                                             Timestamped<ChargingStationStatusTypes>  CurrentStatus)
        {

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                chargingStationOperator.SetChargingStationStatus(ChargingStationId,
                                                                 CurrentStatus);
            }

        }

        #endregion

        #region SetChargingStationStatus     (ChargingStationId, CurrentStatusList)

        public void SetChargingStationStatus(ChargingStation_Id                                    ChargingStationId,
                                             IEnumerable<Timestamped<ChargingStationStatusTypes>>  CurrentStatusList)
        {

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                chargingStationOperator.SetChargingStationStatus(ChargingStationId,
                                                                 CurrentStatusList);
            }

        }

        #endregion


        #region ChargingStationAdminStatus        (IncludeStations = null)

        /// <summary>
        /// Return the admin status of all charging stations registered within this roaming network.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStationAdminStatus> ChargingStationAdminStatus(IncludeChargingStationDelegate? IncludeStations = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingStationAdminStatus(IncludeStations));

        #endregion

        #region ChargingStationAdminStatusSchedule(IncludeChargingStations = null, TimestampFilter  = null, StatusFilter = null, Skip = null, Take = null)

        /// <summary>
        /// Return the admin status of all charging stations registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingStations">An optional delegate for filtering charging stations.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="AdminStatusFilter">An optional admin status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>>>

            ChargingStationAdminStatusSchedule(IncludeChargingStationDelegate?                  IncludeChargingStations   = null,
                                               Func<DateTime,                        Boolean>?  TimestampFilter           = null,
                                               Func<ChargingStationAdminStatusTypes, Boolean>?  AdminStatusFilter         = null,
                                               UInt64?                                          Skip                      = null,
                                               UInt64?                                          Take                      = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingStationAdminStatusSchedule(IncludeChargingStations,
                                                                            TimestampFilter,
                                                                            AdminStatusFilter,
                                                                            Skip,
                                                                            Take));

        #endregion

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, EventTrackingId, ChargingStation, NewStatus, OldStatus, DataSource)

        /// <summary>
        /// An event fired whenever the admin status of any subordinated ChargingStation changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate? OnChargingStationAdminStatusChanged;


        /// <summary>
        /// Update a charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldAdminStatus">The old charging station admin status.</param>
        /// <param name="NewAdminStatus">The new charging station admin status.</param>
        internal async Task UpdateChargingStationAdminStatus(DateTime                                       Timestamp,
                                                             EventTracking_Id                               EventTrackingId,
                                                             IChargingStation                               ChargingStation,
                                                             Timestamped<ChargingStationAdminStatusTypes>   NewAdminStatus,
                                                             Timestamped<ChargingStationAdminStatusTypes>?  OldAdminStatus   = null,
                                                             Context?                                       DataSource       = null)
        {

            try
            {

                var results = allSendAdminStatus.WhenAll(iSendAdminStatus => iSendAdminStatus.UpdateChargingStationAdminStatus(new[] {
                                                                                                                                  new ChargingStationAdminStatusUpdate(
                                                                                                                                      ChargingStation.Id,
                                                                                                                                      NewAdminStatus,
                                                                                                                                      OldAdminStatus,
                                                                                                                                      DataSource
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              EventTrackingId: EventTrackingId));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateEVSEAdminStatus of charging station '{ChargingStation.Id}' from '{OldAdminStatus}' to '{NewAdminStatus}'");
            }


            var onChargingStationAdminStatusChanged = OnChargingStationAdminStatusChanged;
            if (onChargingStationAdminStatusChanged is not null)
                await onChargingStationAdminStatusChanged(Timestamp,
                                                          EventTrackingId,
                                                          ChargingStation,
                                                          NewAdminStatus,
                                                          OldAdminStatus,
                                                          DataSource);

        }

        #endregion

        #region SendChargingStationAdminStatusDiff(StatusDiff)

        internal void SendChargingStationAdminStatusDiff(ChargingStationAdminStatusDiff StatusDiff)
        {
            OnChargingStationAdminDiff?.Invoke(StatusDiff);
        }

        #endregion

        #region OnChargingStationAdminDiff

        public delegate void OnChargingStationAdminDiffDelegate(ChargingStationAdminStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a charging station admin status diff was received.
        /// </summary>
        public event OnChargingStationAdminDiffDelegate? OnChargingStationAdminDiff;

        #endregion


        #region ChargingStationStatus             (IncludeStations = null)

        /// <summary>
        /// Return the status of all charging stations registered within this roaming network.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStationStatus> ChargingStationStatus(IncludeChargingStationDelegate? IncludeStations = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingStationStatus(IncludeStations));

        #endregion

        #region ChargingStationStatusSchedule     (IncludeChargingStations = null, TimestampFilter  = null, StatusFilter = null, Skip = null, Take = null)

        /// <summary>
        /// Return the admin status of all charging stations registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingStations">An optional delegate for filtering charging stations.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusTypes>>>>

            ChargingStationStatusSchedule(IncludeChargingStationDelegate?             IncludeChargingStations   = null,
                                          Func<DateTime,                   Boolean>?  TimestampFilter           = null,
                                          Func<ChargingStationStatusTypes, Boolean>?  StatusFilter              = null,
                                          UInt64?                                     Skip                      = null,
                                          UInt64?                                     Take                      = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingStationStatusSchedule(IncludeChargingStations,
                                                                       TimestampFilter,
                                                                       StatusFilter,
                                                                       Skip,
                                                                       Take));

        #endregion

        #region (internal) UpdateChargingStationStatus     (Timestamp, EventTrackingId, ChargingStation, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationStatusChangedDelegate? OnChargingStationStatusChanged;


        /// <summary>
        /// Update a charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old charging station status.</param>
        /// <param name="NewStatus">The new charging station status.</param>
        internal async Task UpdateChargingStationStatus(DateTime                                  Timestamp,
                                                        EventTracking_Id                          EventTrackingId,
                                                        IChargingStation                          ChargingStation,
                                                        Timestamped<ChargingStationStatusTypes>   NewStatus,
                                                        Timestamped<ChargingStationStatusTypes>?  OldStatus    = null,
                                                        Context?                                  DataSource   = null)
        {

            try
            {

                var results = allSendStatus.WhenAll(iSendStatus => iSendStatus.UpdateChargingStationStatus(new[] {
                                                                                                              new ChargingStationStatusUpdate(
                                                                                                                  ChargingStation.Id,
                                                                                                                  NewStatus,
                                                                                                                  OldStatus,
                                                                                                                  DataSource
                                                                                                              )
                                                                                                          },
                                                                                                          EventTrackingId: EventTrackingId));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateChargingStationStatus of charging station '{ChargingStation.Id}' from '{OldStatus}' to '{NewStatus}'");
            }


            var onChargingStationStatusChanged = OnChargingStationStatusChanged;
            if (onChargingStationStatusChanged is not null)
                await onChargingStationStatusChanged(Timestamp,
                                                     EventTrackingId,
                                                     ChargingStation,
                                                     NewStatus,
                                                     OldStatus,
                                                     DataSource);

        }

        #endregion

        #endregion

        #region EVSEs...

        #region EVSEs

        /// <summary>
        /// Return all EVSEs registered within this roaming network.
        /// </summary>
        public IEnumerable<IEVSE> EVSEs

            => chargingStationOperators.SelectMany(cso => cso.EVSEs);

        #endregion

        #region EVSEIds(IncludeEVSEs = null)

        /// <summary>
        /// Return all EVSE identifications registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate? IncludeEVSEs = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.EVSEIds(IncludeEVSEs));

        #endregion


        #region EVSEAddition

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSEAddition

            => EVSEAddition;

        private void SendEVSEAdded(DateTime          Timestamp,
                                   EventTracking_Id  EventTrackingId,
                                   User_Id           UserId,
                                   IChargingStation  ChargingStation,
                                   IEVSE             EVSE)
        {

            EVSEAddition.SendNotification(Timestamp,
                                          EventTrackingId,
                                          UserId,
                                          ChargingStation,
                                          EVSE);

            try
            {

                var results = allSendEVSEData.WhenAll(target => target.AddEVSE(EVSE,
                                                                               EventTrackingId: EventTrackingId));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.SendEVSEAdded of EVSE '{EVSE.Id}' to charging station '{ChargingStation.Id}'");
            }

        }

        #endregion

        #region (internal) UpdateEVSEData       (Timestamp, EventTrackingId, EVSE, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate? OnEVSEDataChanged;


        /// <summary>
        /// Update the data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateEVSEData(DateTime          Timestamp,
                                           EventTracking_Id  EventTrackingId,
                                           IEVSE             EVSE,
                                           String            PropertyName,
                                           Object?           NewValue,
                                           Object?           OldValue     = null,
                                           Context?          DataSource   = null)
        {

            try
            {

                var results = allSendEVSEData.WhenAll(target => target.UpdateEVSE(EVSE,
                                                                                  PropertyName,
                                                                                  NewValue,
                                                                                  OldValue,
                                                                                  DataSource,
                                                                                  EventTrackingId: EventTrackingId));


                var onEVSEDataChanged = OnEVSEDataChanged;
                if (onEVSEDataChanged is not null)
                    await onEVSEDataChanged(Timestamp,
                                            EventTrackingId,
                                            EVSE,
                                            PropertyName,
                                            NewValue,
                                            OldValue,
                                            DataSource);

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateEVSEData of EVSE '{EVSE.Id}' property '{PropertyName}' from '{OldValue?.ToString() ?? "-"}' to '{NewValue?.ToString() ?? "-"}'");
            }

        }

        #endregion

        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSERemoval

            => EVSERemoval;

        private void SendEVSERemoved(DateTime          Timestamp,
                                     EventTracking_Id  EventTrackingId,
                                     User_Id           UserId,
                                     IChargingStation  ChargingStation,
                                     IEVSE             EVSE)
        {

            EVSERemoval.SendNotification(Timestamp,
                                         EventTrackingId,
                                         UserId,
                                         ChargingStation,
                                         EVSE);

            try
            {

                var results = allSendEVSEData.WhenAll(target => target.DeleteEVSE(EVSE));

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.SendEVSERemoved of EVSE '{EVSE.Id}' from charging station '{ChargingStation.Id}'");
            }

        }

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the roaming network.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(IEVSE EVSE)
        {

            if (EVSE.Operator is not null                                                            &&
                TryGetChargingStationOperatorById(EVSE.Operator.Id, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.ContainsEVSE(EVSE.Id);
            }

            return false;

        }

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the roaming network.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.ContainsEVSE(EVSEId);
            }

            return false;

        }

        #endregion

        #region GetEVSEById(EVSEId)

        public IEVSE? GetEVSEById(EVSE_Id EVSEId)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null                                                   &&
                chargingStationOperator.TryGetEVSEById(EVSEId, out var evse))
            {
                return evse;
            }

            return null;

        }

        public IEVSE? GetEVSEById(EVSE_Id? EVSEId)
        {

            if (EVSEId.HasValue &&
                TryGetChargingStationOperatorById(EVSEId.Value.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null                                                         &&
                chargingStationOperator.TryGetEVSEById(EVSEId.Value, out var evse))
            {
                return evse;
            }

            return null;

        }

        #endregion

        #region TryGetEVSEById(EVSEId, out EVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE? EVSE)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.TryGetEVSEById(EVSEId, out EVSE);
            }

            EVSE = null;
            return false;

        }

        public Boolean TryGetEVSEById(EVSE_Id? EVSEId, out IEVSE? EVSE)
        {

            if (EVSEId.HasValue                                                                             &&
                TryGetChargingStationOperatorById(EVSEId.Value.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                return chargingStationOperator.TryGetEVSEById(EVSEId.Value, out EVSE);
            }

            EVSE = null;
            return false;

        }

        #endregion


        #region SetEVSEAdminStatus(EVSEAdminStatusList)

        public void SetEVSEAdminStatus(IEnumerable<EVSEAdminStatus> EVSEAdminStatusList)
        {

            if (EVSEAdminStatusList is not null)
            {
                foreach (var evseAdminStatus in EVSEAdminStatusList)
                {
                    if (TryGetChargingStationOperatorById(evseAdminStatus.Id.OperatorId, out var chargingStationOperator) &&
                        chargingStationOperator is not null)
                    {
                        chargingStationOperator.SetEVSEAdminStatus(evseAdminStatus);
                    }
                }
            }

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, NewAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id                            EVSEId,
                                       Timestamped<EVSEAdminStatusTypes>  NewAdminStatus)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {
                chargingStationOperator.SetEVSEAdminStatus(EVSEId, NewAdminStatus);
            }

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, Timestamp, NewAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id               EVSEId,
                                       DateTime              Timestamp,
                                       EVSEAdminStatusTypes  NewAdminStatus)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {

                chargingStationOperator.SetEVSEAdminStatus(EVSEId,
                                                           new Timestamped<EVSEAdminStatusTypes>(Timestamp,
                                                           NewAdminStatus));

            }

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, AdminStatusList)

        public void SetEVSEAdminStatus(EVSE_Id                                         EVSEId,
                                       IEnumerable<Timestamped<EVSEAdminStatusTypes>>  AdminStatusList,
                                       ChangeMethods                                   ChangeMethod  = ChangeMethods.Replace)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {

                chargingStationOperator.SetEVSEAdminStatus(EVSEId,
                                                           AdminStatusList,
                                                           ChangeMethod);

            }

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EventTrackingId, EVSE, NewAdminStatus, OldAdminStatus = null, DataSource = null)

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate? OnEVSEAdminStatusChanged;


        /// <summary>
        /// Update an EVSE admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="NewAdminStatus">The new EVSE admin status.</param>
        /// <param name="OldAdminStatus">The optional old EVSE admin status.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE admin status update.</param>
        internal async Task UpdateEVSEAdminStatus(DateTime                            Timestamp,
                                                  EventTracking_Id                    EventTrackingId,
                                                  IEVSE                               EVSE,
                                                  Timestamped<EVSEAdminStatusTypes>   NewAdminStatus,
                                                  Timestamped<EVSEAdminStatusTypes>?  OldAdminStatus   = null,
                                                  Context?                            DataSource       = null)
        {

            try
            {

                var results = allSendAdminStatus.WhenAll(iSendAdminStatus => iSendAdminStatus.UpdateEVSEAdminStatus(new[] {
                                                                                                                       new EVSEAdminStatusUpdate(
                                                                                                                           EVSE.Id,
                                                                                                                           NewAdminStatus,
                                                                                                                           OldAdminStatus,
                                                                                                                           DataSource
                                                                                                                       )
                                                                                                                   },
                                                                                                                   EventTrackingId: EventTrackingId));


                var onEVSEAdminStatusChanged = OnEVSEAdminStatusChanged;
                if (onEVSEAdminStatusChanged is not null)
                    await onEVSEAdminStatusChanged(Timestamp,
                                                   EventTrackingId,
                                                   EVSE,
                                                   NewAdminStatus,
                                                   OldAdminStatus,
                                                   DataSource);

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateEVSEAdminStatus of EVSE '{EVSE.Id}' from '{OldAdminStatus?.ToString() ?? "-"}' to '{NewAdminStatus}'");
            }

        }

        #endregion

        #region EVSEAdminStatus        (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate? IncludeEVSEs = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.EVSEAdminStatus(IncludeEVSEs));

        #endregion

        #region EVSEAdminStatusSchedule(IncludeEVSEs = null, TimestampFilter  = null, StatusFilter = null, Skip = null, Take = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>>

            EVSEAdminStatusSchedule(IncludeEVSEDelegate?                  IncludeEVSEs      = null,
                                    Func<DateTime,             Boolean>?  TimestampFilter   = null,
                                    Func<EVSEAdminStatusTypes, Boolean>?  StatusFilter      = null,
                                    UInt64?                               Skip              = null,
                                    UInt64?                               Take              = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.EVSEAdminStatusSchedule(IncludeEVSEs,
                                                                 TimestampFilter,
                                                                 StatusFilter,
                                                                 Skip,
                                                                 Take));

        #endregion


        #region SetEVSEStatus(EVSEStatusList)

        public void SetEVSEStatus(IEnumerable<EVSEStatus> EVSEStatusList)
        {

            if (EVSEStatusList is not null)
            {
                foreach (var evseStatus in EVSEStatusList)
                {
                    if (TryGetChargingStationOperatorById(evseStatus.Id.OperatorId, out var chargingStationOperator) &&
                        chargingStationOperator is not null)
                    {
                        chargingStationOperator.SetEVSEStatus(evseStatus);
                    }
                }
            }

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewStatus)

        public void SetEVSEStatus(EVSE_Id                       EVSEId,
                                  Timestamped<EVSEStatusTypes>  NewStatus)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {

                chargingStationOperator.SetEVSEStatus(EVSEId,
                                                      NewStatus);

            }

        }

        #endregion

        #region SetEVSEStatus(EVSEId, Timestamp, NewStatus)

        public void SetEVSEStatus(EVSE_Id          EVSEId,
                                  DateTime         Timestamp,
                                  EVSEStatusTypes  NewStatus)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {

                chargingStationOperator.SetEVSEStatus(EVSEId,
                                                      new Timestamped<EVSEStatusTypes>(Timestamp,
                                                                                       NewStatus));

            }

        }

        #endregion

        #region SetEVSEStatus(EVSEId, StatusList)

        public void SetEVSEStatus(EVSE_Id                                    EVSEId,
                                  IEnumerable<Timestamped<EVSEStatusTypes>>  StatusList,
                                  ChangeMethods                              ChangeMethod  = ChangeMethods.Replace)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {

                chargingStationOperator.SetEVSEStatus(EVSEId,
                                                      StatusList,
                                                      ChangeMethod);

            }

        }

        #endregion

        #region EVSEStatus             (IncludeEVSEs = null)

        /// <summary>
        /// Return the status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate? IncludeEVSEs = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.EVSEStatus(IncludeEVSEs));

        #endregion

        #region EVSEStatusSchedule     (IncludeEVSEs = null, TimestampFilter  = null, StatusFilter = null, Skip = null, Take = null)

        /// <summary>
        /// Return the status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        public IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>>

            EVSEStatusSchedule(IncludeEVSEDelegate?             IncludeEVSEs      = null,
                               Func<DateTime,        Boolean>?  TimestampFilter   = null,
                               Func<EVSEStatusTypes, Boolean>?  StatusFilter      = null,
                               UInt64?                          Skip              = null,
                               UInt64?                          Take              = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.EVSEStatusSchedule(IncludeEVSEs,
                                                            TimestampFilter,
                                                            StatusFilter,
                                                            Skip,
                                                            Take));

        #endregion

        #region (internal) UpdateEVSEStatus     (Timestamp, EventTrackingId, EVSE, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate? OnEVSEStatusChanged;


        /// <summary>
        /// Update an EVSE status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        /// <param name="OldStatus">The optional old EVSE status.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE status update.</param>
        internal async Task UpdateEVSEStatus(DateTime                       Timestamp,
                                             EventTracking_Id               EventTrackingId,
                                             IEVSE                          EVSE,
                                             Timestamped<EVSEStatusTypes>   NewStatus,
                                             Timestamped<EVSEStatusTypes>?  OldStatus    = null,
                                             Context?                       DataSource   = null)
        {

            try
            {

                var results = allSendStatus.WhenAll(iSendStatus => iSendStatus.UpdateEVSEStatus(new[] {
                                                                                                   new EVSEStatusUpdate(
                                                                                                       EVSE.Id,
                                                                                                       NewStatus,
                                                                                                       OldStatus,
                                                                                                       DataSource
                                                                                                   )
                                                                                               },
                                                                                               EventTrackingId: EventTrackingId));


                var onEVSEStatusChanged = OnEVSEStatusChanged;
                if (onEVSEStatusChanged is not null)
                    await onEVSEStatusChanged(Timestamp,
                                              EventTrackingId,
                                              EVSE,
                                              NewStatus,
                                              OldStatus,
                                              DataSource);

            }
            catch (Exception e)
            {
                DebugX.Log(e, $"RoamingNetwork '{Id}'.UpdateEVSEStatus of EVSE '{EVSE.Id}' from '{OldStatus}' to '{NewStatus}'");
            }

        }

        #endregion

        #region OnEVSEStatusDiff

        public delegate void OnEVSEStatusDiffDelegate(EVSEStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever an EVSE status diff was received.
        /// </summary>
        public event OnEVSEStatusDiffDelegate? OnEVSEStatusDiff;

        #endregion

        #region SendEVSEStatusDiff(StatusDiff)

        internal void SendEVSEStatusDiff(EVSEStatusDiff StatusDiff)
        {
            OnEVSEStatusDiff?.Invoke(StatusDiff);
        }

        #endregion

        #endregion


        #region Parking Operators...

        #region ParkingOperators

        private readonly EntityHashSet<RoamingNetwork, ParkingOperator_Id, ParkingOperator> parkingOperators;

        /// <summary>
        /// Return all parking operators registered within this roaming network.
        /// </summary>
        public IEnumerable<ParkingOperator> ParkingOperators

            => parkingOperators;

        #endregion


        #region ParkingOperatorAddition

        /// <summary>
        /// Called whenever a parking operator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, ParkingOperator, Boolean> OnParkingOperatorAddition
            => parkingOperators.OnAddition;

        #endregion

        #region ParkingOperatorRemoval

        /// <summary>
        /// Called whenever a parking operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, ParkingOperator, Boolean> OnParkingOperatorRemoval
            => parkingOperators.OnRemoval;

        #endregion


        #region ParkingOperatorAdminStatus

        /// <summary>
        /// Return the admin status of all parking operators registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorAdminStatusTypes>>>> ParkingOperatorAdminStatus

            => parkingOperators.
                   Select(pop => new KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorAdminStatusTypes>>>(pop.Id,
                                                                                                                                 pop.AdminStatusSchedule()));

        #endregion

        #region ParkingOperatorStatus

        /// <summary>
        /// Return the status of all parking operators registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorStatusTypes>>>> ParkingOperatorStatus

            => parkingOperators.
                   Select(pop => new KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorStatusTypes>>>(pop.Id,
                                                                                                                            pop.StatusSchedule()));

        #endregion


        #region CreateNewParkingOperator(Id, Name = null, Description = null, Configurator = null, OnSuccess = null, OnError = null)

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
        public ParkingOperator? CreateNewParkingOperator(ParkingOperator_Id                           Id,
                                                         I18NString?                                  Name                           = null,
                                                         I18NString?                                  Description                    = null,
                                                         Action<ParkingOperator>?                     Configurator                   = null,
                                                         RemoteParkingOperatorCreatorDelegate?        RemoteParkingOperatorCreator   = null,
                                                         ParkingOperatorAdminStatusTypes?             InititalAdminStatus            = ParkingOperatorAdminStatusTypes.Operational,
                                                         ParkingOperatorStatusTypes?                  InititalStatus                 = ParkingOperatorStatusTypes.Available,
                                                         Action<ParkingOperator>?                     OnSuccess                      = null,
                                                         Action<RoamingNetwork, ParkingOperator_Id>?  OnError                        = null)

        {

            var parkingOperator = new ParkingOperator(Id,
                                                      this,
                                                      Name,
                                                      Description,
                                                      Configurator,
                                                      RemoteParkingOperatorCreator,
                                                      InititalAdminStatus,
                                                      InititalStatus);


            if (parkingOperators.TryAdd(parkingOperator,
                                        EventTracking_Id.New,
                                        null).Result == CommandResult.Success)
            {

                parkingOperator.OnDataChanged                                 += UpdateParkingOperatorData;
                parkingOperator.OnStatusChanged                               += UpdateParkingOperatorStatus;
                parkingOperator.OnAdminStatusChanged                          += UpdateParkingOperatorAdminStatus;

                //_ParkingOperator.OnChargingPoolDataChanged                     += UpdateChargingPoolData;
                //_ParkingOperator.OnChargingPoolStatusChanged                   += UpdateChargingPoolStatus;
                //_ParkingOperator.OnChargingPoolAdminStatusChanged              += UpdateChargingPoolAdminStatus;

                //_ParkingOperator.OnDataChanged                  += UpdateParkingData;
                //_ParkingOperator.OnStatusChanged                += UpdateParkingStatus;
                //_ParkingOperator.OnAdminStatusChanged           += UpdateParkingAdminStatus;

                ////chargingStationOperator.EVSEAddition.OnVoting                         += SendEVSEAdding;
                //_ParkingOperator.EVSEAddition.OnNotification                   += SendEVSEAdded;
                ////chargingStationOperator.EVSERemoval.OnVoting                          += SendEVSERemoving;
                //_ParkingOperator.EVSERemoval.OnNotification                    += SendEVSERemoved;
                //_ParkingOperator.OnEVSEDataChanged                             += UpdateEVSEData;
                //_ParkingOperator.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                //_ParkingOperator.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;


                //_ParkingOperator.OnNewReservation                              += SendNewReservation;
                //_ParkingOperator.OnCancelReservationResponse                        += SendOnCancelReservationResponse;
                //_ParkingOperator.OnNewChargingSession                          += SendNewChargingSession;
                //_ParkingOperator.OnNewChargeDetailRecord                       += SendNewChargeDetailRecord;

                return parkingOperator;

            }

            throw new ParkingOperatorAlreadyExists(this, Id);

        }

        #endregion

        #region CreateParkingSpace(ParkingSpaceId, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new parking space having the given
        /// unique parking space identification.
        /// </summary>
        /// <param name="ParkingSpaceId">The unique identification of the new charging pool.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging pool before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging pool after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging pool failed.</param>
        public ParkingSpace CreateParkingSpace(ParkingSpace_Id                                    ParkingSpaceId,
                                               Action<ParkingSpace>?                              Configurator   = null,
                                               Action<ParkingSpace>?                              OnSuccess      = null,
                                               Action<ChargingStationOperator, ParkingSpace_Id>?  OnError        = null)
        {

            var parkingSpace = new ParkingSpace(ParkingSpaceId);

            Configurator?.Invoke(parkingSpace);

            return parkingSpace;

        }

        #endregion


        #region ContainsParkingOperator(ParkingOperator)

        /// <summary>
        /// Check if the given ParkingOperator is already present within the roaming network.
        /// </summary>
        /// <param name="ParkingOperator">An parking Operator.</param>
        public Boolean ContainsParkingOperator(ParkingOperator ParkingOperator)

            => parkingOperators.ContainsId(ParkingOperator.Id);

        #endregion

        #region ContainsParkingOperator(ParkingOperatorId)

        /// <summary>
        /// Check if the given ParkingOperator identification is already present within the roaming network.
        /// </summary>
        /// <param name="ParkingOperatorId">The unique identification of the parking Operator.</param>
        public Boolean ContainsParkingOperator(ParkingOperator_Id ParkingOperatorId)

            => parkingOperators.ContainsId(ParkingOperatorId);

        #endregion

        #region GetParkingOperatorById(ParkingOperatorId)

        public ParkingOperator GetParkingOperatorById(ParkingOperator_Id ParkingOperatorId)

            => parkingOperators.GetById(ParkingOperatorId);

        #endregion

        #region TryGetParkingOperatorById(ParkingOperatorId, out ParkingOperator)

        public Boolean TryGetParkingOperatorById(ParkingOperator_Id ParkingOperatorId, out ParkingOperator ParkingOperator)

            => parkingOperators.TryGet(ParkingOperatorId, out ParkingOperator);

        #endregion

        #region RemoveParkingOperator(ParkingOperatorId)

        public ParkingOperator RemoveParkingOperator(ParkingOperator_Id ParkingOperatorId)
        {

            if (parkingOperators.TryRemove(ParkingOperatorId,
                                           out var _ParkingOperator,
                                           EventTracking_Id.New,
                                           null))
            {
                return _ParkingOperator;
            }

            return null;

        }

        #endregion

        #region TryRemoveParkingOperator(RemoveParkingOperatorId, out RemoveParkingOperator)

        public Boolean TryRemoveParkingOperator(ParkingOperator_Id ParkingOperatorId, out ParkingOperator ParkingOperator)

            => parkingOperators.TryRemove(ParkingOperatorId,
                                          out ParkingOperator,
                                          EventTracking_Id.New,
                                          null);

        #endregion


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


        #region (internal) UpdateParkingOperatorAdminStatus(Timestamp, ParkingOperator, NewStatus, OldStatus, DataSource)

        /// <summary>
        /// Update the current parking operator admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ParkingOperator">The updated parking operator.</param>
        /// <param name="OldStatus">The old aggreagted parking operator admin status.</param>
        /// <param name="NewStatus">The new aggreagted parking operator admin status.</param>
        internal async Task UpdateParkingOperatorAdminStatus(DateTime                                      Timestamp,
                                                             ParkingOperator                               ParkingOperator,
                                                             Timestamped<ParkingOperatorAdminStatusTypes>  OldStatus,
                                                             Timestamped<ParkingOperatorAdminStatusTypes>  NewStatus)
        {

            // Send parking Operator admin status change upstream
            var OnParkingOperatorAdminStatusChangedLocal = OnParkingOperatorAdminStatusChanged;
            if (OnParkingOperatorAdminStatusChangedLocal is not null)
                await OnParkingOperatorAdminStatusChangedLocal(Timestamp,
                                                               ParkingOperator,
                                                               OldStatus,
                                                               NewStatus);


            // Calculate new aggregated roaming network status and send upstream
            //if (AdminStatusAggregationDelegate != null)
            //{

            //    var NewAggregatedStatus = new Timestamped<RoamingNetworkAdminStatusType>(AdminStatusAggregationDelegate(new csoAdminStatusReport(_ParkingOperators.Values)));

            //    if (NewAggregatedStatus.Value != _AdminStatusHistory.Peek().Value)
            //    {

            //        var OldAggregatedStatus = _AdminStatusHistory.Peek();

            //        _AdminStatusHistory.Push(NewAggregatedStatus);

            //        var OnAggregatedAdminStatusChangedLocal = OnAggregatedAdminStatusChanged;
            //        if (OnAggregatedAdminStatusChangedLocal != null)
            //            OnAggregatedAdminStatusChangedLocal(Timestamp, this, OldAggregatedStatus, NewAggregatedStatus);

            //    }

            //}

        }

        #endregion

        #region (internal) UpdateParkingOperatorStatus     (Timestamp, ParkingOperator, NewStatus, OldStatus, DataSource)

        /// <summary>
        /// Update the current parking operator status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ParkingOperator">The updated parking operator.</param>
        /// <param name="OldStatus">The old aggreagted parking operator status.</param>
        /// <param name="NewStatus">The new aggreagted parking operator status.</param>
        internal async Task UpdateParkingOperatorStatus(DateTime                                 Timestamp,
                                                        ParkingOperator                          ParkingOperator,
                                                        Timestamped<ParkingOperatorStatusTypes>  OldStatus,
                                                        Timestamped<ParkingOperatorStatusTypes>  NewStatus)
        {

            var OnParkingOperatorStatusChangedLocal = OnParkingOperatorStatusChanged;
            if (OnParkingOperatorStatusChangedLocal is not null)
                await OnParkingOperatorStatusChangedLocal(Timestamp,
                                                          ParkingOperator,
                                                          OldStatus,
                                                          NewStatus);

            // Calculate new aggregated roaming network status and send upstream
            //if (StatusAggregationDelegate != null)
            //{

            //    var NewAggregatedStatus = new Timestamped<RoamingNetworkStatusType>(StatusAggregationDelegate(new csoStatusReport(_ParkingOperators.Values)));

            //    if (NewAggregatedStatus.Value != _StatusHistory.Peek().Value)
            //    {

            //        var OldAggregatedStatus = _StatusHistory.Peek();

            //        _StatusHistory.Push(NewAggregatedStatus);

            //        OnStatusChanged?.Invoke(Timestamp,
            //                                this,
            //                                OldAggregatedStatus,
            //                                NewAggregatedStatus);

            //    }

            //}

        }

        #endregion

        #region (internal) UpdateParkingOperatorData       (Timestamp, ParkingOperator, OldValue,  NewValue)

        /// <summary>
        /// Update the data of an evse operator.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ParkingOperator">The changed evse operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateParkingOperatorData(DateTime         Timestamp,
                                                      ParkingOperator  ParkingOperator,
                                                      String           PropertyName,
                                                      Object           OldValue,
                                                      Object           NewValue)
        {

            var OnParkingOperatorDataChangedLocal = OnParkingOperatorDataChanged;
            if (OnParkingOperatorDataChangedLocal is not null)
                await OnParkingOperatorDataChangedLocal(Timestamp,
                                                        ParkingOperator,
                                                        PropertyName,
                                                        OldValue,
                                                        NewValue);

        }

        #endregion

        #endregion

        #region Grid Operators...

        #region GridOperators

        private readonly EntityHashSet<RoamingNetwork, GridOperator_Id, GridOperator> gridOperators;

        /// <summary>
        /// Return all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<GridOperator> GridOperators
            => gridOperators;

        #endregion


        #region OnGridOperatorAddition

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, GridOperator, Boolean> OnGridOperatorAddition
            => gridOperators.OnAddition;

        #endregion

        #region OnGridOperatorRemoval

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, GridOperator, Boolean> OnGridOperatorRemoval
            => gridOperators.OnRemoval;

        #endregion


        #region GridOperatorsAdminStatus

        /// <summary>
        /// Return the admin status of all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorAdminStatusType>>>> GridOperatorsAdminStatus

            => gridOperators.
                   Select(emp => new KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorAdminStatusType>>>(emp.Id, emp.AdminStatusSchedule()));

        #endregion

        #region GridOperatorsStatus

        /// <summary>
        /// Return the status of all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorStatusType>>>> GridOperatorsStatus

            => gridOperators.
                   Select(emp => new KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorStatusType>>>(emp.Id, emp.StatusSchedule()));

        #endregion


        #region CreateNewGridOperator(GridOperatorId, Configurator = null)

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
        public GridOperator CreateNewGridOperator(GridOperator_Id                          GridOperatorId,
                                                  I18NString                               Name                       = null,
                                                  I18NString                               Description                = null,
                                                  GridOperatorPriority                     Priority                   = null,
                                                  GridOperatorAdminStatusType              AdminStatus                = GridOperatorAdminStatusType.Available,
                                                  GridOperatorStatusType                   Status                     = GridOperatorStatusType.Available,
                                                  Action<GridOperator>                     Configurator               = null,
                                                  Action<GridOperator>                     OnSuccess                  = null,
                                                  Action<RoamingNetwork, GridOperator_Id>  OnError                    = null,
                                                  RemoteGridOperatorCreatorDelegate        RemoteGridOperatorCreator  = null)
        {

            #region Initial checks

            if (GridOperatorId == null)
                throw new ArgumentNullException(nameof(GridOperatorId),  "The given smart city identification must not be null!");

            #endregion

            var gridOperator = new GridOperator(GridOperatorId,
                                                this,
                                                Configurator,
                                                RemoteGridOperatorCreator,
                                                Name,
                                                Description,
                                                Priority,
                                                AdminStatus,
                                                Status);


            if (gridOperators.TryAdd(gridOperator,
                                     EventTracking_Id.New,
                                     null).Result == CommandResult.Success)
            {

                // Link events!

                return gridOperator;

            }

            throw new GridOperatorAlreadyExists(this, GridOperatorId);

        }

        #endregion

        #region ContainsGridOperator(GridOperator)

        /// <summary>
        /// Check if the given GridOperator is already present within the roaming network.
        /// </summary>
        /// <param name="GridOperator">An Charging Station Operator.</param>
        public Boolean ContainsGridOperator(GridOperator GridOperator)

            => gridOperators.ContainsId(GridOperator.Id);

        #endregion

        #region ContainsGridOperator(GridOperatorId)

        /// <summary>
        /// Check if the given GridOperator identification is already present within the roaming network.
        /// </summary>
        /// <param name="GridOperatorId">The unique identification of the Charging Station Operator.</param>
        public Boolean ContainsGridOperator(GridOperator_Id GridOperatorId)

            => gridOperators.ContainsId(GridOperatorId);

        #endregion

        #region GetGridOperatorById(GridOperatorId)

        public GridOperator GetGridOperatorById(GridOperator_Id GridOperatorId)

            => gridOperators.GetById(GridOperatorId);

        #endregion

        #region TryGetGridOperatorById(GridOperatorId, out GridOperator)

        public Boolean TryGetGridOperatorById(GridOperator_Id GridOperatorId, out GridOperator GridOperator)

            => gridOperators.TryGet(GridOperatorId, out GridOperator);

        #endregion

        #region RemoveGridOperator(GridOperatorId)

        public GridOperator RemoveGridOperator(GridOperator_Id GridOperatorId)
        {

            GridOperator _GridOperator = null;

            if (gridOperators.TryRemove(GridOperatorId,
                                        out _GridOperator,
                                        EventTracking_Id.New,
                                        null))
            {
                return _GridOperator;
            }

            return null;

        }

        #endregion

        #region TryRemoveGridOperator(RemoveGridOperatorId, out RemoveGridOperator)

        public Boolean TryRemoveGridOperator(GridOperator_Id GridOperatorId, out GridOperator GridOperator)

            => gridOperators.TryRemove(GridOperatorId,
                                       out GridOperator,
                                       EventTracking_Id.New,
                                       null);

        #endregion

        #endregion

        #region Smart Cities...

        #region SmartCities

        private readonly EntityHashSet<RoamingNetwork, SmartCity_Id, SmartCityProxy> smartCities;

        /// <summary>
        /// Return all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<SmartCityProxy> SmartCities

            => smartCities;

        #endregion

        #region SmartCitiesAdminStatus

        /// <summary>
        /// Return the admin status of all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityAdminStatusTypes>>>> SmartCitiesAdminStatus

            => smartCities.
                   Select(emp => new KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityAdminStatusTypes>>>(emp.Id, emp.AdminStatusSchedule()));

        #endregion

        #region SmartCitiesStatus

        /// <summary>
        /// Return the status of all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityStatusTypes>>>> SmartCitiesStatus

            => smartCities.
                   Select(emp => new KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityStatusTypes>>>(emp.Id, emp.StatusSchedule()));

        #endregion


        #region OnSmartCityAddition

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, SmartCityProxy, Boolean> OnSmartCityAddition
            => smartCities.OnAddition;

        #endregion

        #region OnSmartCityRemoval

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, SmartCityProxy, Boolean> OnSmartCityRemoval
            => smartCities.OnRemoval;

        #endregion


        #region CreateNewSmartCity(SmartCityId, Configurator = null)

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
        public SmartCityProxy? CreateNewSmartCity(SmartCity_Id                           Id,
                                                  I18NString?                            Name                     = null,
                                                  I18NString?                            Description              = null,
                                                  SmartCityPriority?                     Priority                 = null,
                                                  SmartCityAdminStatusTypes?             InitialAdminStatus       = SmartCityAdminStatusTypes.Available,
                                                  SmartCityStatusTypes?                  InitialStatus            = SmartCityStatusTypes.Available,
                                                  Action<SmartCityProxy>?                Configurator             = null,
                                                  Action<SmartCityProxy>?                OnSuccess                = null,
                                                  Action<RoamingNetwork, SmartCity_Id>?  OnError                  = null,
                                                  RemoteSmartCityCreatorDelegate?        RemoteSmartCityCreator   = null)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),  "The given smart city identification must not be null!");

            #endregion

            var smartCity = new SmartCityProxy(Id,
                                               this,
                                               Name,
                                               Description,
                                               Configurator,
                                               RemoteSmartCityCreator,
                                               Priority,
                                               InitialAdminStatus,
                                               InitialStatus);


            if (smartCities.TryAdd(smartCity,
                                   EventTracking_Id.New,
                                   null).Result == CommandResult.Success)
            {

                // Link events!

                return smartCity;

            }

            throw new SmartCityAlreadyExists(this, Id);

        }

        #endregion

        #region ContainsSmartCity(SmartCity)

        /// <summary>
        /// Check if the given SmartCity is already present within the roaming network.
        /// </summary>
        /// <param name="SmartCity">An Charging Station Operator.</param>
        public Boolean ContainsSmartCity(SmartCityProxy SmartCity)

            => smartCities.ContainsId(SmartCity.Id);

        #endregion

        #region ContainsSmartCity(SmartCityId)

        /// <summary>
        /// Check if the given SmartCity identification is already present within the roaming network.
        /// </summary>
        /// <param name="SmartCityId">The unique identification of the Charging Station Operator.</param>
        public Boolean ContainsSmartCity(SmartCity_Id SmartCityId)

            => smartCities.ContainsId(SmartCityId);

        #endregion

        #region GetSmartCityById(SmartCityId)

        public SmartCityProxy GetSmartCityById(SmartCity_Id SmartCityId)

            => smartCities.GetById(SmartCityId);

        #endregion

        #region TryGetSmartCityById(SmartCityId, out SmartCity)

        public Boolean TryGetSmartCityById(SmartCity_Id SmartCityId, out SmartCityProxy SmartCity)

            => smartCities.TryGet(SmartCityId, out SmartCity);

        #endregion

        #region RemoveSmartCity(SmartCityId)

        public SmartCityProxy RemoveSmartCity(SmartCity_Id SmartCityId)
        {

            if (smartCities.TryRemove(SmartCityId,
                                       out var _SmartCity,
                                       EventTracking_Id.New,
                                       null))
            {
                return _SmartCity;
            }

            return null;

        }

        #endregion

        #region TryRemoveSmartCity(RemoveSmartCityId, out RemoveSmartCity)

        public Boolean TryRemoveSmartCity(SmartCity_Id SmartCityId, out SmartCityProxy SmartCity)

            => smartCities.TryRemove(SmartCityId,
                                      out SmartCity,
                                      EventTracking_Id.New,
                                      null);

        #endregion


        //#region AllTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> AllTokens

        //    => _SmartCities.SelectMany(provider => provider.AllTokens);

        //#endregion

        //#region AuthorizedTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> AuthorizedTokens

        //    => _SmartCities.SelectMany(provider => provider.AuthorizedTokens);

        //#endregion

        //#region NotAuthorizedTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> NotAuthorizedTokens

        //    => _SmartCities.SelectMany(provider => provider.NotAuthorizedTokens);

        //#endregion

        //#region BlockedTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> BlockedTokens

        //    => _SmartCities.SelectMany(provider => provider.BlockedTokens);

        //#endregion

        #endregion

        #region Navigation Providers...

        #region NavigationProviders

        private readonly EntityHashSet<RoamingNetwork, NavigationProvider_Id, NavigationProvider> navigationProviders;

        /// <summary>
        /// Return all navigation providers registered within this roaming network.
        /// </summary>
        public IEnumerable<NavigationProvider> NavigationProviders

            => navigationProviders;

        #endregion

        #region NavigationProviderAdminStatus

        /// <summary>
        /// Return the admin status of all navigation providers registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderAdminStatusType>>>> NavigationProviderAdminStatus

            => navigationProviders.
                   Select(emp => new KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderAdminStatusType>>>(emp.Id, emp.AdminStatusSchedule()));

        #endregion

        #region NavigationProviderStatus

        /// <summary>
        /// Return the status of all navigation providers registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderStatusType>>>> NavigationProviderStatus

            => navigationProviders.
                   Select(emp => new KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderStatusType>>>(emp.Id, emp.StatusSchedule()));

        #endregion


        #region OnNavigationProviderAddition

        /// <summary>
        /// Called whenever an navigation provider will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, NavigationProvider, Boolean> OnNavigationProviderAddition
            => navigationProviders.OnAddition;

        #endregion

        #region OnNavigationProviderRemoval

        /// <summary>
        /// Called whenever an navigation provider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, RoamingNetwork, NavigationProvider, Boolean> OnNavigationProviderRemoval
            => navigationProviders.OnRemoval;

        #endregion


        #region CreateNewNavigationProvider(NavigationProviderId, Configurator = null)

        /// <summary>
        /// Create and register a new navigation provider having the given
        /// unique navigation provider identification.
        /// </summary>
        /// <param name="NavigationProviderId">The unique identification of the new navigation provider.</param>
        /// <param name="Name">The offical (multi-language) name of the navigation provider.</param>
        /// <param name="Description">An optional (multi-language) description of the navigation provider.</param>
        /// <param name="Configurator">An optional delegate to configure the new navigation provider before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new navigation provider after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the navigation provider failed.</param>
        public NavigationProvider CreateNewNavigationProvider(NavigationProvider_Id                          NavigationProviderId,
                                                              I18NString                                     Name                              = null,
                                                              I18NString                                     Description                       = null,
                                                              NavigationProviderPriority                     Priority                          = null,
                                                              NavigationProviderAdminStatusType              AdminStatus                       = NavigationProviderAdminStatusType.Available,
                                                              NavigationProviderStatusType                   Status                            = NavigationProviderStatusType.Available,
                                                              Action<NavigationProvider>                     Configurator                      = null,
                                                              Action<NavigationProvider>                     OnSuccess                         = null,
                                                              Action<RoamingNetwork, NavigationProvider_Id>  OnError                           = null,
                                                              RemoteNavigationProviderCreatorDelegate        RemoteNavigationProviderCreator   = null)
        {

            #region Initial checks

            if (NavigationProviderId == null)
                throw new ArgumentNullException(nameof(NavigationProviderId),  "The given navigation provider identification must not be null!");

            #endregion

            var navigationProvider = new NavigationProvider(NavigationProviderId,
                                                            this,
                                                            Configurator,
                                                            RemoteNavigationProviderCreator,
                                                            Name,
                                                            Description,
                                                            Priority,
                                                            AdminStatus,
                                                            Status);


            if (navigationProviders.TryAdd(navigationProvider,
                                           EventTracking_Id.New,
                                           null).Result == CommandResult.Success)
            {

                // Link events!

                return navigationProvider;

            }

            throw new NavigationProviderAlreadyExists(this, NavigationProviderId);

        }

        #endregion

        #region ContainsNavigationProvider(NavigationProvider)

        /// <summary>
        /// Check if the given NavigationProvider is already present within the roaming network.
        /// </summary>
        /// <param name="NavigationProvider">An Charging Station Operator.</param>
        public Boolean ContainsNavigationProvider(NavigationProvider NavigationProvider)

            => navigationProviders.ContainsId(NavigationProvider.Id);

        #endregion

        #region ContainsNavigationProvider(NavigationProviderId)

        /// <summary>
        /// Check if the given NavigationProvider identification is already present within the roaming network.
        /// </summary>
        /// <param name="NavigationProviderId">The unique identification of the Charging Station Operator.</param>
        public Boolean ContainsNavigationProvider(NavigationProvider_Id NavigationProviderId)

            => navigationProviders.ContainsId(NavigationProviderId);

        #endregion

        #region GetNavigationProviderById(NavigationProviderId)

        public NavigationProvider GetNavigationProviderById(NavigationProvider_Id NavigationProviderId)

            => navigationProviders.GetById(NavigationProviderId);

        #endregion

        #region TryGetNavigationProviderById(NavigationProviderId, out NavigationProvider)

        public Boolean TryGetNavigationProviderById(NavigationProvider_Id NavigationProviderId, out NavigationProvider NavigationProvider)

            => navigationProviders.TryGet(NavigationProviderId, out NavigationProvider);

        #endregion

        #region RemoveNavigationProvider(NavigationProviderId)

        public NavigationProvider RemoveNavigationProvider(NavigationProvider_Id NavigationProviderId)
        {

            if (navigationProviders.TryRemove(NavigationProviderId,
                                               out var _NavigationProvider,
                                               EventTracking_Id.New,
                                               null))
            {
                return _NavigationProvider;
            }

            return null;

        }

        #endregion

        #region TryRemoveNavigationProvider(RemoveNavigationProviderId, out RemoveNavigationProvider)

        public Boolean TryRemoveNavigationProvider(NavigationProvider_Id NavigationProviderId, out NavigationProvider NavigationProvider)

            => navigationProviders.TryRemove(NavigationProviderId,
                                              out NavigationProvider,
                                              EventTracking_Id.New,
                                              null);

        #endregion

        #endregion


        #region Reservations...

        #region Data

        public IEnumerable<ChargingReservation> ChargingReservations
            => ReservationsStore.
                   Select(reservation => reservation.Last());


        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(15);

        TimeSpan IChargingReservations.MaxReservationDuration { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a charging location is being reserved.
        /// </summary>
        public event OnReserveRequestDelegate?             OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate?            OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate?             OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate?   OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate?  OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate?        OnReservationCanceled;

        #endregion


        #region Reserve(ChargingLocation, ReservationLevel = EVSE, StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">A charging location.</param>
        /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<ReservationResult>

            Reserve(ChargingLocation                   ChargingLocation,
                    ChargingReservationLevel           ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                          ReservationStartTime   = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    Auth_Path?                         AuthenticationPath     = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<EMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null,
                    CancellationToken                  CancellationToken      = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            ReservationResult? result = null;

            #endregion

            #region Send OnReserveRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveRequest?.Invoke(startTime,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         Id,
                                         ReservationId,
                                         LinkedReservationId,
                                         ChargingLocation,
                                         ReservationStartTime,
                                         Duration,
                                         ProviderId,
                                         RemoteAuthentication,
                                         ChargingProduct,
                                         AuthTokens,
                                         eMAIds,
                                         PINs,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnReserveRequest));
            }

            #endregion


            try
            {

                var EVSEId = ChargingLocation.EVSEId.Value;

                if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out var chargingStationOperator) &&
                    chargingStationOperator is not null)
                {

                    result = await chargingStationOperator.
                                       Reserve(ChargingLocation,
                                               ReservationLevel,
                                               ReservationStartTime,
                                               Duration,
                                               ReservationId,
                                               LinkedReservationId,
                                               ProviderId,
                                               RemoteAuthentication,
                                               AuthenticationPath,
                                               ChargingProduct,
                                               AuthTokens,
                                               eMAIds,
                                               PINs,

                                               Timestamp,
                                               EventTrackingId,
                                               RequestTimeout,
                                               CancellationToken);

                    if (result.Result == ReservationResultType.Success)
                    {
                        if (result.Reservation is not null)
                        {
                            result.Reservation.ChargingStationOperatorId = chargingStationOperator.Id;
                            ReservationsStore.NewOrUpdate(result.Reservation);
                        }
                    }

                }

                if (result        is     null ||
                   (result        is not null &&
                   (result.Result == ReservationResultType.UnknownLocation)))
                {

                    foreach (var empRoamingService in empRoamingProviders.
                                                          OrderBy(empRoamingServiceWithPriority => empRoamingServiceWithPriority.Key).
                                                          Select (empRoamingServiceWithPriority => empRoamingServiceWithPriority.Value))
                    {

                        result = await empRoamingService.
                                           Reserve(ChargingLocation,
                                                   ReservationLevel,
                                                   ReservationStartTime,
                                                   Duration,
                                                   ReservationId,
                                                   LinkedReservationId,
                                                   ProviderId,
                                                   RemoteAuthentication,
                                                   AuthenticationPath,
                                                   ChargingProduct,
                                                   AuthTokens,
                                                   eMAIds,
                                                   PINs,

                                                   Timestamp,
                                                   EventTrackingId,
                                                   RequestTimeout,
                                                   CancellationToken);


                        if (result.Result == ReservationResultType.Success)
                        {
                            if (result.Reservation is not null)
                            {
                                result.Reservation.EMPRoamingProviderId = empRoamingService.Id;
                                ReservationsStore.NewOrUpdate(result.Reservation);
                            }
                        }

                    }

                }

                result ??= ReservationResult.UnknownChargingStationOperator;

            }
            catch (Exception e)
            {
                result = ReservationResult.Error(e.Message);
            }


            #region Send OnReserveResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveResponse?.Invoke(endTime,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          Id,
                                          ReservationId,
                                          LinkedReservationId,
                                          ChargingLocation,
                                          ReservationStartTime,
                                          Duration,
                                          ProviderId,
                                          RemoteAuthentication,
                                          ChargingProduct,
                                          AuthTokens,
                                          eMAIds,
                                          PINs,
                                          result,
                                          endTime - startTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnReserveResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region CancelReservation(ReservationId, Reason, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,

                              DateTime?                              Timestamp          = null,
                              EventTracking_Id?                      EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null,
                              CancellationToken                      CancellationToken  = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            ChargingReservation?     canceledReservation  = null;
            CancelReservationResult? result               = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   Id,
                                                   ReservationId,
                                                   Reason,
                                                   RequestTimeout);


            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (ReservationsStore.TryGetLatest(ReservationId, out var chargingReservation))
                {

                    #region Check Charging Station Operator charging reservation lookup...

                    if (chargingReservation.ChargingStationOperatorId.HasValue &&
                        TryGetChargingStationOperatorById(chargingReservation.ChargingStationOperatorId.Value, out var chargingStationOperator) &&
                        chargingStationOperator is not null)
                    {

                        result = await chargingStationOperator.
                                           CancelReservation(ReservationId,
                                                             Reason,

                                                             Timestamp,
                                                             EventTrackingId,
                                                             RequestTimeout,
                                                             CancellationToken);

                    }

                    #endregion

                    #region ...or check CSO roaming provider charging reservation lookup...

                    if (result        is null ||
                       (result        is not null &&
                       (result.Result == CancelReservationResultTypes.UnknownEVSE ||
                        result.Result == CancelReservationResultTypes.UnknownReservationId)))
                    {


                        if (chargingReservation.EMPRoamingProviderId.HasValue &&
                            TryGetEMPRoamingProviderById(chargingReservation.EMPRoamingProviderId.Value, out var empRoamingProvider) &&
                            empRoamingProvider is not null)
                        {

                            result = await empRoamingProvider.
                                               CancelReservation(ReservationId,
                                                                 Reason,

                                                                 Timestamp,
                                                                 EventTrackingId,
                                                                 RequestTimeout,
                                                                 CancellationToken);

                        }

                    }

                    #endregion

                    #region ...or try to check every CSO roaming provider...

                    if (result        is     null ||
                       (result        is not null &&
                       (result.Result == CancelReservationResultTypes.UnknownEVSE ||
                        result.Result == CancelReservationResultTypes.UnknownReservationId)))
                    {

                        foreach (var empRoamingService in empRoamingProviders.
                                                              OrderBy(empRoamingServiceWithPriority => empRoamingServiceWithPriority.Key).
                                                              Select (empRoamingServiceWithPriority => empRoamingServiceWithPriority.Value))
                        {

                            result = await empRoamingService.
                                               CancelReservation(ReservationId,
                                                                 Reason,

                                                                 Timestamp,
                                                                 EventTrackingId,
                                                                 RequestTimeout,
                                                                 CancellationToken);

                        }

                    }

                    #endregion

                    #region ...or fail!

                    if (result == null)
                    {

                        result = CancelReservationResult.UnknownReservationId(ReservationId,
                                                                              Reason);

                        //SendReservationCanceled(Timestamp.Now,
                        //                        this,
                        //                           EventTrackingId,
                        //                           Id,
                        //                           ProviderId,
                        //                           ReservationId,
                        //                           null,
                        //                           Reason,
                        //                           result,
                        //                           result.Runtime.HasValue
                        //                               ? result.Runtime.Value
                        //                               : TimeSpan.FromMilliseconds(5),
                        //                           RequestTimeout);

                    }

                    #endregion

                }

                else
                    result = CancelReservationResult.UnknownReservationId(ReservationId,
                                                                          Reason);

            }
            catch (Exception e)
            {
                result = CancelReservationResult.Error(ReservationId,
                                                       Reason,
                                                       e.Message);
            }


            #region Send OnCancelReservationResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationResponse?.Invoke(EndTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    Id,
                                                    ReservationId,
                                                    canceledReservation,
                                                    Reason,
                                                    result,
                                                    EndTime - StartTime,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region GetChargingReservationById    (ReservationId)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        public ChargingReservation? GetChargingReservationById(ChargingReservation_Id ReservationId)
        {

            if (ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                return reservationCollection?.LastOrDefault();
            }

            return null;

        }

        #endregion

        #region GetChargingReservationsById   (ReservationId)

        /// <summary>
        /// Return the charging reservations specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        public ChargingReservationCollection? GetChargingReservationsById(ChargingReservation_Id ReservationId)
        {

            if (ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                return reservationCollection;
            }

            return null;

        }

        #endregion

        #region TryGetChargingReservationById (ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation? Reservation)
        {

            if (ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                Reservation = reservationCollection?.LastOrDefault();
                return true;
            }

            Reservation = null;
            return false;

        }

        #endregion

        #region TryGetChargingReservationsById(ReservationId, out ChargingReservations)

        /// <summary>
        /// Return the charging reservation collection specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="ChargingReservations">The charging reservations.</param>
        public Boolean TryGetChargingReservationsById(ChargingReservation_Id ReservationId, out ChargingReservationCollection? ChargingReservations)
        {

            if (ReservationsStore.TryGet(ReservationId, out var reservationCollection))
            {
                ChargingReservations = reservationCollection;
                return true;
            }

            ChargingReservations = null;
            return false;

        }

        #endregion


        #region (internal) SendNewReservation     (Timestamp, Sender, Reservation)

        internal void SendNewReservation(DateTime             Timestamp,
                                         Object               Sender,
                                         ChargingReservation  Reservation)
        {

            OnNewReservation?.Invoke(Timestamp, Sender, Reservation);

        }

        #endregion

        #region (internal) SendReservationCanceled(Timestamp, Sender, Reservation, Reason)

        internal void SendReservationCanceled(DateTime                               Timestamp,
                                              Object                                 Sender,
                                              ChargingReservation                    Reservation,
                                              ChargingReservationCancellationReason  Reason)
        {

            OnReservationCanceled?.Invoke(Timestamp, Sender, Reservation, Reason);

        }

        #endregion

        #endregion

        #region Charging Sessions

        #region Data

        public IEnumerable<ChargingSession> ChargingSessions
            => SessionsStore;

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate?              OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate?             OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate OnNewChargingSession
        {
            add
            {
                SessionsStore.OnNewChargingSession += value;
            }
            remove
            {
                SessionsStore.OnNewChargingSession -= value;
            }
        }


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate?               OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate?              OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord
        {
            add
            {
                SessionsStore.OnNewChargeDetailRecord += value;
            }
            remove
            {
                SessionsStore.OnNewChargeDetailRecord -= value;
            }
        }

        /// <summary>
        /// An event fired whenever a new charge detail record was sent.
        /// </summary>
        public event OnNewChargeDetailRecordResultDelegate OnNewChargeDetailRecordResult
        {
            add
            {
                SessionsStore.OnNewChargeDetailRecordResult += value;
            }
            remove
            {
                SessionsStore.OnNewChargeDetailRecordResult -= value;
            }
        }

        #endregion


        /// <summary>
        /// Whether the given charging session identification is known within the roaming network.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        public Boolean Contains(ChargingSession_Id ChargingSessionId)

            => SessionsStore.ContainsKey(ChargingSessionId);


        /// <summary>
        /// Return the charging session specified by the given charging session identification.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id    ChargingSessionId,
                                                 out ChargingSession?  ChargingSession)

            => SessionsStore.TryGet(ChargingSessionId, out ChargingSession);


        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        public ChargingSession? GetChargingSessionById(ChargingSession_Id ChargingSessionId)

            => SessionsStore.Get(ChargingSessionId);


        #region (internal) SendNewChargingSession   (Timestamp, Sender, Session)

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  Session)
        {

            if (Session != null)
            {

                if (Session.RoamingNetwork == null)
                {
                    Session.RoamingNetwork    = this;
                    Session.RoamingNetworkId  = Id;
                }

                //OnNewChargingSession?.Invoke(Timestamp, Sender, Session);
                //SessionsStore.SendNewChargingSession(Timestamp, Sender, Session);

            }

        }

        #endregion

        #region RegisterExternalChargingSession(Timestamp, Sender, ChargingSession)

        /// <summary>
        /// Register an external charging session which was not registered
        /// via the RemoteStart or AuthStart mechanisms.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="Sender">The sender of the charging session.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public void RegisterExternalChargingSession(DateTime         Timestamp,
                                                    Object           Sender,
                                                    ChargingSession  ChargingSession)
        {
            //SessionsStore.RegisterExternalChargingSession(Timestamp, Sender, ChargingSession);
        }

        #endregion

        #region RemoveExternalChargingSession(Timestamp, Sender, ChargingSession)

        /// <summary>
        /// Register an external charging session which was not registered
        /// via the RemoteStart or AuthStart mechanisms.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="Sender">The sender of the charging session.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public void RemoveExternalChargingSession(DateTime         Timestamp,
                                                  Object           Sender,
                                                  ChargingSession  ChargingSession)
        {
            //SessionsStore.RemoveExternalChargingSession(Timestamp, Sender, ChargingSession);
        }

        #endregion

        #endregion

        #region Charge Detail Records

        #region OnSendCDRRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record will be send upstream.
        /// </summary>
        public event OnSendCDRsRequestDelegate?   OnSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        public event OnSendCDRsResponseDelegate?  OnSendCDRsResponse;

        #endregion

        #region OnFilterCDRRecords

        public delegate IEnumerable<I18NString> OnFilterCDRRecordsDelegate(IId AuthorizatorId, ChargeDetailRecord ChargeDetailRecord);

        /// <summary>
        /// An event fired whenever a CDR needs to be filtered.
        /// </summary>
        public event OnFilterCDRRecordsDelegate? OnFilterCDRRecords;

        #endregion

        #region OnCDRWasFiltered

        public delegate Task OnCDRWasFilteredDelegate(ChargeDetailRecord ChargeDetailRecord, IEnumerable<I18NString> Reason);

        /// <summary>
        /// An event fired whenever a CDR was filtered.
        /// </summary>
        public event OnCDRWasFilteredDelegate? OnCDRWasFiltered;

        #endregion

        #region IReceiveChargeDetailRecords.SendChargeDetailRecords(ChargeDetailRecords, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// 
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        Task<SendCDRsResult>

            IReceiveChargeDetailRecords.ReceiveChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,

                                                                   DateTime?                        Timestamp,
                                                                   EventTracking_Id?                EventTrackingId,
                                                                   TimeSpan?                        RequestTimeout,
                                                                   CancellationToken                CancellationToken)

                => SendChargeDetailRecords(ChargeDetailRecords,
                                           TransmissionTypes.Enqueue,

                                           Timestamp,
                                           EventTrackingId,
                                           RequestTimeout,
                                           CancellationToken);

        #endregion

        #region SendChargeDetailRecord (ChargeDetailRecord,  ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="TransmissionType">Whether to send the charge detail record directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        public Task<SendCDRsResult>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,
                                   TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                                   DateTime?           Timestamp           = null,
                                   EventTracking_Id?   EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null,
                                   CancellationToken   CancellationToken   = default)


                => SendChargeDetailRecords(new[] { ChargeDetailRecord },
                                           TransmissionType,

                                           Timestamp,
                                           EventTrackingId,
                                           RequestTimeout,
                                           CancellationToken);

        #endregion

        #region SendChargeDetailRecords(ChargeDetailRecords, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the charge detail record directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTime?                        Timestamp           = null,
                                    EventTracking_Id?                EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null,
                                    CancellationToken                CancellationToken   = default)

        {

            #region Initial checks

            var timestamp        = Timestamp       ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            var eventTrackingId  = EventTrackingId ?? EventTracking_Id.New;

            #endregion

            #region Send OnSendCDRsRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnSendCDRsRequest?.Invoke(startTime,
                                          timestamp,
                                          this,
                                          Id.ToString(),
                                          eventTrackingId,
                                          Id,
                                          ChargeDetailRecords,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            #region if SendChargeDetailRecords disabled...

            DateTime         endtime;
            TimeSpan         runtime;
            SendCDRsResult?  result   = null;

            if (DisableSendChargeDetailRecords)
            {

                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = SendCDRsResult.AdminDown(
                               org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                               Id,
                               this as ISendChargeDetailRecords,
                               ChargeDetailRecords,
                               Runtime: runtime
                           );

            }

            #endregion

            #region ..., or when there are no charge detail records...

            else if (!ChargeDetailRecords.Any())
            {

                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = SendCDRsResult.NoOperation(
                               org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                               Id,
                               this as ISendChargeDetailRecords,
                               ChargeDetailRecords,
                               Runtime: runtime
                           );

            }

            #endregion

            else
            {

                //ToDo: Fail when RFID UID == "00000000000000"

                var chargeDetailRecordsToProcess  = ChargeDetailRecords.ToList();
                var expectedChargeDetailRecords   = ChargeDetailRecords.ToList();

                //ToDo: Merge given cdr information with local information!

                #region Delete cached session information

                foreach (var chargeDetailRecord in chargeDetailRecordsToProcess)
                {
                    if (chargeDetailRecord.EVSEId.HasValue)
                    {

                        if (TryGetEVSEById(chargeDetailRecord.EVSEId.Value, out var evse))
                        {

                            if (evse?.ChargingSession is not null &&
                                evse. ChargingSession.Id == chargeDetailRecord.SessionId)
                            {

                                //_EVSE.Status = EVSEStatusType.Available;
                                //_EVSE.ChargingSession  = null;
                                //_EVSE.Reservation      = null;

                            }

                        }

                    }
                }

                #endregion

                var resultMap = new List<SendCDRResult>();

                #region Some charge detail records should perhaps be filtered...

                var OnFilterCDRRecordsLocal = OnFilterCDRRecords;
                if (OnFilterCDRRecordsLocal != null)
                {

                    foreach (var chargeDetailRecord in ChargeDetailRecords)
                    {

                        var filterResult = OnFilterCDRRecordsLocal(Id, chargeDetailRecord);
                        if (filterResult.IsNeitherNullNorEmpty())
                        {

                            resultMap.Add(SendCDRResult.Filtered(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                 chargeDetailRecord,
                                                                 filterResult.SafeSelect(filterResult => Warning.Create(filterResult))));

                            var OnCDRWasFilteredLocal = OnCDRWasFiltered;
                            if (OnCDRWasFilteredLocal is not null)
                            {
                                try
                                {
                                    await OnCDRWasFilteredLocal.Invoke(chargeDetailRecord,
                                                                       filterResult);
                                }
                                catch (Exception e)
                                {
                                    DebugX.Log("OnCDRWasFiltered event throw an exception: " +
                                               e.Message + Environment.NewLine +
                                               e.StackTrace);
                                }
                            }

                            chargeDetailRecordsToProcess.Remove(chargeDetailRecord);

                        }

                    }

                }

                #endregion


                #region Group charge detail records by their targets...

                var cdrTargets = new Dictionary<ISendChargeDetailRecords, List<ChargeDetailRecord>>();

                foreach (var isendcdr in allRemoteSendChargeDetailRecord)
                    cdrTargets.Add(isendcdr, new List<ChargeDetailRecord>());

                #endregion

                #region Try to find the target CSO or roaming network for each CDR...

                foreach (var cdr in chargeDetailRecordsToProcess.ToArray())
                {

                    #region The CDR ProviderIdStart is known, or...

                    if (cdr.ProviderIdStart.HasValue &&
                        TryGetEMobilityProviderById(cdr.ProviderIdStart.Value, out var empForCDR) &&
                        empForCDR is not null)
                    {

                        cdrTargets[empForCDR].Add(cdr);
                        chargeDetailRecordsToProcess.Remove(cdr);

                        SessionsStore.CDRReceived(cdr.SessionId,
                                                  cdr);

                        continue;

                    }

                    #endregion

                    #region ...the CDR ProviderIdStop is known, or...

                    else if (cdr.ProviderIdStop.HasValue &&
                             TryGetEMobilityProviderById(cdr.ProviderIdStop.Value, out empForCDR) &&
                             empForCDR is not null)
                    {

                        cdrTargets[empForCDR].Add(cdr);
                        chargeDetailRecordsToProcess.Remove(cdr);

                        SessionsStore.CDRReceived(cdr.SessionId,
                                                  cdr);

                        continue;

                    }

                    #endregion

                    #region ...the CDR CSORoamingProvider(Id) is known, or...

                    else if (cdr.CSORoamingProvider is not null &&
                            !cdr.CSORoamingProviderId.HasValue)
                    {
                        cdr.CSORoamingProviderId = cdr.CSORoamingProvider.Id;
                    }

                    if (cdr.CSORoamingProviderId.HasValue)
                    {

                        var empRoamingProviderId      = cdr.CSORoamingProviderId.ToString();
                        var empRoamingProviderForCDR  = allRemoteSendChargeDetailRecord.FirstOrDefault(iSendChargeDetailRecords => iSendChargeDetailRecords.SendChargeDetailRecordsId.ToString() == empRoamingProviderId) as ICSORoamingProvider;

                        if (empRoamingProviderForCDR is not null)
                        {

                            cdrTargets[empRoamingProviderForCDR].Add(cdr);
                            chargeDetailRecordsToProcess.Remove(cdr);

                            SessionsStore.CDRReceived(cdr.SessionId,
                                                      cdr);

                            continue;

                        }

                    }

                    #endregion

                    #region ...we lookup the CSO roaming provider via the authorization within the charging sessions

                    else if (SessionsStore.TryGet(cdr.SessionId, out var chargingSession) &&
                             chargingSession is not null)
                    {

                        // Might occur when the software had been restarted
                        //   while charging sessions still had been active!
                        //if (chargingSession.AuthService == null && chargingSession.AuthorizatorId != null)
                        //    chargingSession.AuthService = _IRemoteSendChargeDetailRecord.FirstOrDefault(rm => rm.Id.ToString() == chargingSession.AuthorizatorId.ToString()) as ISendAuthorizeStartStop;

                        //if (chargingSession.AuthService != null && chargingSession.AuthService is ISendChargeDetailRecords sendCDR)
                        //{
                        //    _ISendChargeDetailRecords[sendCDR].Add(cdr);
                        //    ChargeDetailRecordsToProcess.Remove(cdr);
                        //}

                        if (chargingSession.CSORoamingProviderStart   is null &&
                            chargingSession.CSORoamingProviderIdStart is not null)
                            chargingSession.CSORoamingProviderStart = allRemoteSendChargeDetailRecord.FirstOrDefault(iSendChargeDetailRecords => iSendChargeDetailRecords.SendChargeDetailRecordsId.ToString() == chargingSession.CSORoamingProviderIdStart.ToString()) as ICSORoamingProvider;

                        if (chargingSession.CSORoamingProviderStart is not null &&
                            chargingSession.CSORoamingProviderStart is ICSORoamingProvider csoRoamingProviderForCDR)
                        {

                            cdrTargets[csoRoamingProviderForCDR].Add(cdr);
                            chargeDetailRecordsToProcess.Remove(cdr);

                            SessionsStore.CDRReceived(cdr.SessionId,
                                                      cdr);

                        }

                    }

                    #endregion

                }

                #endregion

                #region Any CDRs left? => Ask all e-mobility providers!

                foreach (var chargeDetailRecord in chargeDetailRecordsToProcess.ToList())
                {

                    #region We have a valid (start) provider identification

                    if (chargeDetailRecord.ProviderIdStart.HasValue && eMobilityProviders.TryGet(chargeDetailRecord.ProviderIdStart.Value, out var eMobPro) && eMobPro is not null)
                    {
                        cdrTargets[eMobPro].Add(chargeDetailRecord);
                        chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                    }

                    #endregion

                    #region We have a valid (stop)  provider identification

                    else if (chargeDetailRecord.ProviderIdStop.HasValue && eMobilityProviders.TryGet(chargeDetailRecord.ProviderIdStop.Value, out eMobPro) && eMobPro is not null)
                    {
                        cdrTargets[eMobPro].Add(chargeDetailRecord);
                        chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                    }

                    #endregion


                    #region We have a valid (start) AuthToken/RFID identification

                    else if (chargeDetailRecord.AuthenticationStart?.AuthToken.HasValue == true)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            LocalAuthentication LL = null;

                            if (chargeDetailRecord.AuthenticationStart is LocalAuthentication)
                                LL = chargeDetailRecord.AuthenticationStart as LocalAuthentication;

                            else if (chargeDetailRecord.AuthenticationStart is RemoteAuthentication)
                                LL = (chargeDetailRecord.AuthenticationStart as RemoteAuthentication).ToLocal;

                            if (LL != null)
                            {

                                var authStartResult = await eMob.AuthorizeStart(LL);

                                if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                    authStartResult.Result == AuthStartResultTypes.Blocked)
                                {
                                    cdrTargets[eMob].Add(chargeDetailRecord);
                                    chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                                    break;
                                }

                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  AuthToken/RFID identification

                    else if (chargeDetailRecord.AuthenticationStop?.AuthToken.HasValue == true)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(chargeDetailRecord.AuthenticationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(chargeDetailRecord);
                                chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) QR-Code identification

                    else if (chargeDetailRecord.AuthenticationStart?.QRCodeIdentification.HasValue == true)
                    {

                        var providerId = chargeDetailRecord.AuthenticationStart?.QRCodeIdentification?.eMAId.ProviderId;

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(chargeDetailRecord.AuthenticationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(chargeDetailRecord);
                                chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  QR-Code identification

                    else if (chargeDetailRecord.AuthenticationStop?.QRCodeIdentification.HasValue == true)
                    {

                        var providerId = chargeDetailRecord.AuthenticationStart?.QRCodeIdentification?.eMAId.ProviderId;

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(chargeDetailRecord.AuthenticationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(chargeDetailRecord);
                                chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) Plug'n'Charge identification

                    else if (chargeDetailRecord.AuthenticationStart?.PlugAndChargeIdentification.HasValue == true)
                    {

                        var providerId = chargeDetailRecord.AuthenticationStart?.PlugAndChargeIdentification.Value.ProviderId;

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(chargeDetailRecord.AuthenticationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(chargeDetailRecord);
                                chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  Plug'n'Charge identification

                    else if (chargeDetailRecord.AuthenticationStop?.PlugAndChargeIdentification.HasValue == true)
                    {

                        var providerId = chargeDetailRecord.AuthenticationStart?.PlugAndChargeIdentification.Value.ProviderId;

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(chargeDetailRecord.AuthenticationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(chargeDetailRecord);
                                chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) remote identification

                    else if (chargeDetailRecord.AuthenticationStart?.RemoteIdentification.HasValue == true)
                    {

                        var providerId = chargeDetailRecord.AuthenticationStart?.RemoteIdentification.Value.ProviderId;


                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(chargeDetailRecord.AuthenticationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(chargeDetailRecord);
                                chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                                break;
                            }

                        }

                        foreach (var empRoamingProvider in empRoamingProviders.Values)
                        {

                            //empRoamingProvider

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  remote identification

                    else if (chargeDetailRecord.AuthenticationStop?.RemoteIdentification.HasValue == true)
                    {

                        var providerId = chargeDetailRecord.AuthenticationStart?.RemoteIdentification.Value.ProviderId;

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(chargeDetailRecord.AuthenticationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(chargeDetailRecord);
                                chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) public key

                    else if (chargeDetailRecord.AuthenticationStart?.PublicKey != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(chargeDetailRecord.AuthenticationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(chargeDetailRecord);
                                chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  public key

                    else if (chargeDetailRecord.AuthenticationStop?.PublicKey != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(chargeDetailRecord.AuthenticationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(chargeDetailRecord);
                                chargeDetailRecordsToProcess.Remove(chargeDetailRecord);
                                break;
                            }

                        }

                    }

                    #endregion

                }

                #endregion

                #region Any CDRs left? => bad!

                foreach (var unknownCDR in chargeDetailRecordsToProcess.ToArray())
                {

                    resultMap.Add(
                        SendCDRResult.UnknownSessionId(
                            org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                            unknownCDR
                        )
                    );

                    chargeDetailRecordsToProcess.Remove(unknownCDR);

                }

                #endregion


                #region Send CDRs to IRemoteSendChargeDetailRecord targets...

                SendCDRsResult? partResults = null;

                foreach (var sendCDR in cdrTargets.Where(kvp => kvp.Value.Count > 0))
                {

                    partResults = await sendCDR.Key.SendChargeDetailRecords(sendCDR.Value,
                                                                            TransmissionTypes.Enqueue,

                                                                            Timestamp,
                                                                            EventTrackingId,
                                                                            RequestTimeout,
                                                                            CancellationToken);

                    if (partResults is null)
                    {

                        foreach (var _cdr in sendCDR.Value)
                        {
                            resultMap.Add(SendCDRResult.Error(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                              _cdr,
                                                              Warning.Create(I18NString.Create(sendCDR.Key + " returned null!"))));
                        }

                    }

                    else
                    {
                        foreach (var singleSendCDRResult in partResults.ResultMap)
                            resultMap.Add(singleSendCDRResult);
                    }

                }

                #endregion


                #region Check if we really received a response for every charge detail record!

                foreach (var cdrresult in resultMap)
                    expectedChargeDetailRecords.Remove(cdrresult.ChargeDetailRecord);

                if (expectedChargeDetailRecords.Count > 0)
                {
                    foreach (var _cdr in expectedChargeDetailRecords)
                    {
                        resultMap.Add(SendCDRResult.Error(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                          _cdr,
                                                          Warning.Create(I18NString.Create("Did not receive an result for this charge detail record!"))));
                    }
                }

                #endregion

                var cdrResultOverview = new Dictionary<SendCDRResultTypes, UInt32>();
                foreach (var res in resultMap)
                {

                    if (!cdrResultOverview.ContainsKey(res.Result))
                        cdrResultOverview.Add(res.Result, 1);

                    else
                        cdrResultOverview[res.Result]++;

                }

                var globalResultNumber = cdrResultOverview.Values.Max();
                var globalResults      = cdrResultOverview.Where(kvp => kvp.Value == globalResultNumber).Select(kvp => kvp.Key).ToList();
                if (globalResults.Count > 1)
                {

                    if (globalResults.Contains(SendCDRResultTypes.Success))
                        globalResults.Remove(SendCDRResultTypes.Success);

                    if (globalResults.Contains(SendCDRResultTypes.Enqueued))
                        globalResults.Remove(SendCDRResultTypes.Enqueued);

                }

                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = new SendCDRsResult(
                               org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                               Id,
                               this as IReceiveChargeDetailRecords,
                               globalResults[0].Convert(),
                               resultMap,
                               I18NString.Empty,
                               null,
                               runtime
                           );

            }


            #region Send OnSendCDRsResponse event

            try
            {

                OnSendCDRsResponse?.Invoke(endtime,
                                           timestamp,
                                           this,
                                           Id.ToString(),
                                           eventTrackingId,
                                           Id,
                                           ChargeDetailRecords,
                                           RequestTimeout,
                                           result,
                                           runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            if (ChargeDetailRecord is not null)
                SendChargeDetailRecords(new ChargeDetailRecord[] { ChargeDetailRecord },
                                        TransmissionTypes.Enqueue,

                                        Timestamp,
                                        EventTracking_Id.New,
                                        TimeSpan.FromMinutes(1)).

                                        Wait(TimeSpan.FromMinutes(1));

        }

        #endregion

        #endregion

        #region AuthorizeStart/-Stop

        #region Data

        private readonly ConcurrentDictionary<AuthenticationToken, AuthStartResult> authStartResultCache                   = new();
        private readonly ConcurrentDictionary<AuthenticationToken, AuthStopResult>  authStopResultCache                    = new();
        private readonly ConcurrentDictionary<ChargingLocation,    List<DateTime>>  authenticationChargingLocationCounter  = new();

        public IEnumerable<KeyValuePair<AuthenticationToken, AuthStartResult>>  CachedAuthStartResults
            => authStartResultCache;

        public IEnumerable<KeyValuePair<AuthenticationToken, AuthStopResult>>   CachedAuthStopResults
            => authStopResultCache;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate?   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authorize start command completed.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate?  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize stop command was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate?   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authorize stop command completed.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate?  OnAuthorizeStopResponse;

        #endregion


        #region AuthorizeStart           (LocalAuthentication, ChargingLocation = null, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingLocation?            ChargingLocation      = null,
                           ChargingProduct?             ChargingProduct       = null,
                           ChargingSession_Id?          SessionId             = null,
                           ChargingSession_Id?          CPOPartnerSessionId   = null,
                           ChargingStationOperator_Id?  OperatorId            = null,

                           DateTime?                    Timestamp             = null,
                           EventTracking_Id?            EventTrackingId       = null,
                           TimeSpan?                    RequestTimeout        = null,
                           CancellationToken            CancellationToken     = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= TimeSpan.FromSeconds(10);

            AuthStartResult? result = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnAuthorizeStartRequest?.Invoke(startTime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                Id,
                                                null,
                                                null,
                                                OperatorId,
                                                LocalAuthentication,
                                                ChargingLocation,
                                                ChargingProduct,
                                                SessionId,
                                                CPOPartnerSessionId,
                                                allSend2RemoteAuthorizeStartStop,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            try
            {

                #region Fail when AuthToken is invalid (e.g. "00000000", "00000000000000", ...)

                if (LocalAuthentication.AuthToken.HasValue &&
                    InvalidAuthenticationTokens.Contains(LocalAuthentication.AuthToken.Value))
                {

                    result = AuthStartResult.InvalidToken(
                                 Id,
                                 this,
                                 org.GraphDefined.Vanaheimr.Illias.Timestamp.Now + AuthenticationCacheTimeout,
                                 SessionId,
                                 I18NString.Create($"Invalid authentication token '{LocalAuthentication.AuthToken.Value}'!"),
                                 TimeSpan.Zero
                             );

                }

                #endregion

                #region Check whether there is a cached result

                if (result is null &&
                   !DisableAuthenticationCache &&
                    LocalAuthentication.AuthToken.HasValue &&
                    authStartResultCache.TryGetValue(LocalAuthentication.AuthToken.Value, out var cachedAuthStartResult) &&
                    cachedAuthStartResult is not null)
                {

                    if (cachedAuthStartResult.CachedResultEndOfLifeTime > org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)
                    {

                        result = cachedAuthStartResult.Result == AuthStartResultTypes.Authorized

                                     ? new AuthStartResult(
                                           cachedAuthStartResult.AuthorizatorId,
                                           cachedAuthStartResult.ISendAuthorizeStartStop,
                                           AuthStartResultTypes.Authorized,
                                           CachedResultEndOfLifeTime:  cachedAuthStartResult.CachedResultEndOfLifeTime,
                                           SessionId:                  SessionId,
                                           EMPPartnerSessionId:        cachedAuthStartResult.ProviderId.HasValue
                                                                           ? ChargingSession_Id.NewRandom(cachedAuthStartResult.ProviderId.Value)
                                                                           : ChargingSession_Id.NewRandom(),
                                           ProviderId:                 cachedAuthStartResult.ProviderId,
                                           Runtime:                    TimeSpan.Zero
                                       )

                                     : cachedAuthStartResult;
                    }
                    else
                        authStartResultCache.Remove(LocalAuthentication.AuthToken.Value, out _);

                }

                #endregion

                #region Check rate limit per charging location

                if (ChargingLocation is not null &&
                    ChargingLocation.IsDefined() &&
                   !DisableAuthenticationRateLimit)
                {

                    if (authenticationChargingLocationCounter.TryGetValue(ChargingLocation, out var locationInfo))
                    {

                        do
                        {

                            if (locationInfo.First() < org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - AuthenticationRateLimitTimeSpan)
                                locationInfo.Remove(locationInfo.First());

                        } while (locationInfo.Any() && locationInfo.First() < org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - AuthenticationRateLimitTimeSpan);


                        if (locationInfo.Any())
                        {

                            if (locationInfo.First() > org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - AuthenticationRateLimitTimeSpan)
                                locationInfo.Add(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now);

                            if (locationInfo.Count > AuthenticationRateLimitPerChargingLocation)
                            {

                                result = AuthStartResult.RateLimitReached(
                                             Id,
                                             this,
                                             SessionId,
                                             I18NString.Create($"Rate limit of {AuthenticationRateLimitPerChargingLocation} request per charging location per {AuthenticationRateLimitTimeSpan.TotalMinutes} minutes reached!"),
                                             TimeSpan.Zero
                                         );

                            }

                        }

                    }

                    else
                        authenticationChargingLocationCounter.TryAdd(ChargingLocation,
                                                                     new List<DateTime>() { org.GraphDefined.Vanaheimr.Illias.Timestamp.Now });

                }

                #endregion


                #region Send the request to all authorization services

                result  ??= await allSend2RemoteAuthorizeStartStop.WhenFirst(
                                      Work:           sendAuthorizeStartStop => sendAuthorizeStartStop.AuthorizeStart(
                                                                                    LocalAuthentication,
                                                                                    ChargingLocation,
                                                                                    ChargingProduct,
                                                                                    SessionId,
                                                                                    CPOPartnerSessionId,
                                                                                    OperatorId,

                                                                                    Timestamp,
                                                                                    EventTrackingId,
                                                                                    RequestTimeout,
                                                                                    CancellationToken
                                                                                ),

                                      VerifyResult:   result2  => result2.Result == AuthStartResultTypes.Authorized ||
                                                                  result2.Result == AuthStartResultTypes.Blocked,

                                      Timeout:        RequestTimeout.Value,

                                      OnException:    null,

                                      DefaultResult:  runtime  => AuthStartResult.NotAuthorized(
                                                                      Id,
                                                                      this,
                                                                      SessionId:    SessionId,
                                                                      Description:  I18NString.Create("No authorization service returned a positiv result!"),
                                                                      Runtime:      runtime
                                                                  )

                                  ).
                                  ConfigureAwait(false);

                #endregion


                #region Maybe store the result within the cache

                if (!DisableAuthenticationCache &&
                     LocalAuthentication.AuthToken.HasValue &&
                     result.Result != AuthStartResultTypes.RateLimitReached &&
                    !DoNotCacheAuthenticationTokens.Contains   (LocalAuthentication.AuthToken.Value) &&
                    !authStartResultCache.          ContainsKey(LocalAuthentication.AuthToken.Value))
                {

                    authStartResultCache.TryAdd(LocalAuthentication.AuthToken.Value,
                                                new AuthStartResult(
                                                    AuthorizatorId:             result.AuthorizatorId,
                                                    ISendAuthorizeStartStop:    result.ISendAuthorizeStartStop,
                                                    Result:                     result.Result,
                                                    CachedResultEndOfLifeTime:  org.GraphDefined.Vanaheimr.Illias.Timestamp.Now + AuthenticationCacheTimeout,
                                                    ProviderId:                 result.ProviderId,
                                                    Runtime:                    TimeSpan.Zero
                                                ));

                }

                #endregion

                #region Purge the cache to avoid denial-of-service attacks!

                if (!DisableAuthenticationCache &&
                     authStartResultCache.Count > MaxAuthStartResultCacheElements)
                {
                    foreach (var oldEntries in authStartResultCache.OrderBy(kvp => kvp.Value.CachedResultEndOfLifeTime).Take(MaxAuthStartResultCacheElements/4).ToArray())
                    {
                        authStartResultCache.Remove(oldEntries.Key, out _);
                    }
                }

                #endregion

            }
            catch (Exception e)
            {

                result = AuthStartResult.Error(
                             Id,
                             this,
                             SessionId:    SessionId,
                             Description:  I18NString.Create(e.Message),
                             Runtime:      org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - startTime
                         );

            }


            #region If Authorized...

            if (result.Result == AuthStartResultTypes.Authorized)
            {

                if (!result.SessionId.HasValue)
                    result  = AuthStartResult.Authorized(
                                  result.AuthorizatorId,
                                  result.ISendAuthorizeStartStop,
                                  null,
                                  result.ProviderId.HasValue
                                      ? ChargingSession_Id.NewRandom(result.ProviderId.Value)
                                      : ChargingSession_Id.NewRandom(),
                                  result.EMPPartnerSessionId,
                                  result.ContractId,
                                  result.PrintedNumber,
                                  result.ExpiryDate,
                                  result.MaxkW,
                                  result.MaxkWh,
                                  result.MaxDuration,
                                  result.ChargingTariffs,
                                  result.ListOfAuthStopTokens,
                                  result.ListOfAuthStopPINs,
                                  result.ProviderId,
                                  result.Description,
                                  result.AdditionalInfo,
                                  result.NumberOfRetries,
                                  result.Runtime
                              );


                // Store the upstream session id in order to contact the right authenticator at later requests!
                // Will be deleted when the charge detail record was sent!

                var evse                = ChargingLocation?.EVSEId.           HasValue == true ? GetEVSEById           (ChargingLocation.EVSEId.           Value) : null;
                var chargingStation     = ChargingLocation?.ChargingStationId.HasValue == true ? GetChargingStationById(ChargingLocation.ChargingStationId.Value) : evse?.           ChargingStation;
                var chargingPool        = ChargingLocation?.ChargingPoolId.   HasValue == true ? GetChargingPoolById   (ChargingLocation.ChargingPoolId.   Value) : chargingStation?.ChargingPool;

                var newChargingSession  = new ChargingSession(result.SessionId!.Value,
                                                              EventTrackingId) {
                                              RoamingNetworkId           = Id,
                                              CSORoamingProviderStart    = result.ISendAuthorizeStartStop as ICSORoamingProvider,
                                              ProviderIdStart            = result.ProviderId,
                                              ChargingStationOperatorId  = OperatorId,
                                              EVSEId                     = ChargingLocation?.EVSEId,
                                              ChargingStationId          = ChargingLocation?.ChargingStationId,
                                              ChargingPoolId             = ChargingLocation?.ChargingPoolId,
                                              EVSE                       = evse,
                                              ChargingStation            = chargingStation,
                                              ChargingPool               = chargingPool,
                                              ChargingStationOperator    = OperatorId.HasValue ? GetChargingStationOperatorById(OperatorId.Value) : null,
                                              AuthenticationStart        = LocalAuthentication,
                                              ChargingProduct            = ChargingProduct
                                          };

                SessionsStore.AuthStart(newChargingSession);

            }

            #endregion


            #region Send OnAuthorizeStartResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnAuthorizeStartResponse?.Invoke(endTime,
                                                 Timestamp.Value,
                                                 this,
                                                 Id.ToString(),
                                                 EventTrackingId,
                                                 Id,
                                                 null,
                                                 null,
                                                 OperatorId,
                                                 LocalAuthentication,
                                                 ChargingLocation,
                                                 ChargingProduct,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 allSend2RemoteAuthorizeStartStop,
                                                 RequestTimeout,
                                                 result,
                                                 endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop (SessionId, LocalAuthentication, ChargingLocation = null,                                           OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given location.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingLocation?            ChargingLocation      = null,
                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                          ChargingStationOperator_Id?  OperatorId            = null,

                          DateTime?                    Timestamp             = null,
                          EventTracking_Id?            EventTrackingId       = null,
                          TimeSpan?                    RequestTimeout        = null,
                          CancellationToken            CancellationToken     = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= TimeSpan.FromSeconds(10);

            AuthStopResult? result = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnAuthorizeStopRequest?.Invoke(startTime,
                                               Timestamp.Value,
                                               this,
                                               Id.ToString(),
                                               EventTrackingId,
                                               Id,
                                               null,
                                               null,
                                               OperatorId,
                                               ChargingLocation,
                                               SessionId,
                                               CPOPartnerSessionId,
                                               LocalAuthentication,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            try
            {

                //ToDo: Add a cache here too?

                #region Fail when AuthToken is invalid (e.g. "00000000", "00000000000000", ...)

                if (LocalAuthentication.AuthToken.HasValue &&
                    InvalidAuthenticationTokens.Contains(LocalAuthentication.AuthToken.Value))
                {

                    result = AuthStopResult.InvalidToken(
                                 Id,
                                 this,
                                 SessionId:    SessionId,
                                 Description:  I18NString.Create($"Invalid authentication token '{LocalAuthentication.AuthToken.Value}'!"),
                                 Runtime:      TimeSpan.Zero
                             );

                }

                #endregion

                #region Check rate limit per charging location

                if (!DisableAuthenticationRateLimit &&
                     ChargingLocation is not null &&
                     ChargingLocation.IsDefined())
                {

                    if (authenticationChargingLocationCounter.TryGetValue(ChargingLocation, out var locationInfo))
                    {

                        do
                        {

                            if (locationInfo.First() < org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - AuthenticationRateLimitTimeSpan)
                                locationInfo.Remove(locationInfo.First());

                        } while (locationInfo.Any() && locationInfo.First() < org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - AuthenticationRateLimitTimeSpan);


                        if (locationInfo.Any())
                        {

                            if (locationInfo.First() > org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - AuthenticationRateLimitTimeSpan)
                                locationInfo.Add(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now);

                            if (locationInfo.Count > AuthenticationRateLimitPerChargingLocation)
                            {

                                result = AuthStopResult.RateLimitReached(
                                             Id,
                                             this,
                                             SessionId,
                                             I18NString.Create($"Rate limit of {AuthenticationRateLimitPerChargingLocation} request per charging location per {AuthenticationRateLimitTimeSpan.TotalMinutes} minutes reached!"),
                                             TimeSpan.Zero
                                         );

                            }

                        }

                    }

                    else
                        authenticationChargingLocationCounter.TryAdd(ChargingLocation,
                                                                          new List<DateTime>() { org.GraphDefined.Vanaheimr.Illias.Timestamp.Now });

                }

                #endregion


                #region A matching charging session was found...

                if (result is null &&
                    SessionsStore.TryGet(SessionId, out var chargingSession))
                {

                    //ToDo: Add a --useForce Option to overwrite!
                    if (chargingSession.SessionTime.EndTime.HasValue)
                        result = AuthStopResult.AlreadyStopped(SessionId,
                                                               this,
                                                               SessionId:  SessionId,
                                                               Runtime:    org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - startTime);

                    else
                    {

                        #region When an e-mobility provider was found

                        if (chargingSession.ProviderStart is null && chargingSession.ProviderIdStart.HasValue)
                            chargingSession.ProviderStart = GetEMobilityProviderById(chargingSession.ProviderIdStart);

                        if (chargingSession.ProviderStart is not null)
                        {

                            result = await chargingSession.ProviderStart.AuthorizeStop(SessionId,
                                                                                       LocalAuthentication,
                                                                                       ChargingLocation,
                                                                                       CPOPartnerSessionId,
                                                                                       OperatorId,

                                                                                       Timestamp,
                                                                                       EventTrackingId,
                                                                                       RequestTimeout,
                                                                                       CancellationToken);

                            if (result?.Result == AuthStopResultTypes.Authorized)
                                SessionsStore.AuthStop(SessionId,
                                                       LocalAuthentication,
                                                       result.ProviderId.Value);

                        }

                        #endregion

                        #region ...or when an CSO roaming provider was found...

                        if (result is null)
                        {

                            if (chargingSession.CSORoamingProviderStart is null && chargingSession.CSORoamingProviderIdStart.HasValue)
                                chargingSession.CSORoamingProviderStart = GetCSORoamingProviderById(chargingSession.CSORoamingProviderIdStart.Value);

                            if (chargingSession.CSORoamingProviderStart is not null)
                            {

                                result = await chargingSession.CSORoamingProviderStart.AuthorizeStop(SessionId,
                                                                                                     LocalAuthentication,
                                                                                                     ChargingLocation,
                                                                                                     CPOPartnerSessionId,
                                                                                                     OperatorId,

                                                                                                     Timestamp,
                                                                                                     EventTrackingId,
                                                                                                     RequestTimeout,
                                                                                                     CancellationToken);

                                if (result?.Result == AuthStopResultTypes.Authorized)
                                    SessionsStore.AuthStop(SessionId,
                                                           LocalAuthentication,
                                                           result.ProviderId.Value,
                                                           result.ISendAuthorizeStartStop as ICSORoamingProvider);

                            }

                        }

                        #endregion

                    }

                }

                #endregion

                #region Send the request to all authorization services

                result ??= await allSend2RemoteAuthorizeStartStop.WhenFirst(
                                     Work:           sendAuthorizeStartStop => sendAuthorizeStartStop.AuthorizeStop(
                                                                                   SessionId,
                                                                                   LocalAuthentication,
                                                                                   ChargingLocation,
                                                                                   CPOPartnerSessionId,
                                                                                   OperatorId,

                                                                                   Timestamp,
                                                                                   EventTrackingId,
                                                                                   RequestTimeout,
                                                                                   CancellationToken
                                                                               ),

                                     VerifyResult:   result2 => result2.Result == AuthStopResultTypes.Authorized ||
                                                                result2.Result == AuthStopResultTypes.Blocked,

                                     Timeout:        RequestTimeout.Value,

                                     OnException:    null,

                                     DefaultResult:  runtime => AuthStopResult.NotAuthorized(
                                                                    Id,
                                                                    this,
                                                                    SessionId:    SessionId,
                                                                    Description:  I18NString.Create("No authorization service returned a positiv result!"),
                                                                    Runtime:      runtime
                                                                )

                                 ).
                                 ConfigureAwait(false);

                #endregion

            }
            catch (Exception e)
            {

                result = AuthStopResult.Error(
                             SessionId,
                             this,
                             SessionId,
                             I18NString.Create(e.Message),
                             org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - startTime
                         );

            }


            #region Send OnAuthorizeStopResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnAuthorizeStopResponse?.Invoke(endTime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                Id,
                                                null,
                                                null,
                                                OperatorId,
                                                ChargingLocation,
                                                SessionId,
                                                CPOPartnerSessionId,
                                                LocalAuthentication,
                                                RequestTimeout,
                                                result,
                                                endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region ClearAuthStartResultCache()
        public async Task ClearAuthStartResultCache()
        {
            authStartResultCache.Clear();
        }

        #endregion

        #region ClearAuthStopResultCache()
        public async Task ClearAuthStopResultCache()
        {
            authStopResultCache.Clear();
        }

        #endregion

        #endregion

        #region RemoteStart/-Stop

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">Use the given optinal unique charging reservation identification for charging.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingLocation          ChargingLocation,
                        ChargingProduct?          ChargingProduct            = null,
                        ChargingReservation_Id?   ReservationId              = null,
                        ChargingSession_Id?       SessionId                  = null,
                        EMobilityProvider_Id?     ProviderId                 = null,
                        RemoteAuthentication?     RemoteAuthentication       = null,
                        Auth_Path?                AuthenticationPath         = null,

                        DateTime?                 Timestamp                  = null,
                        EventTracking_Id?         EventTrackingId            = null,
                        TimeSpan?                 RequestTimeout             = null,
                        CancellationToken         CancellationToken          = default)

                => RemoteStart(null,
                               ChargingLocation,
                               ChargingProduct,
                               ReservationId,
                               SessionId,
                               ProviderId,
                               RemoteAuthentication,
                               AuthenticationPath,

                               Timestamp,
                               EventTrackingId,
                               RequestTimeout,
                               CancellationToken);


        /// <summary>
        /// Start a charging session at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">Use the given optinal unique charging reservation identification for charging.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ICSORoamingProvider      CSORoamingProvider,
                        ChargingLocation         ChargingLocation,
                        ChargingProduct?         ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication?    RemoteAuthentication   = null,
                        Auth_Path?               AuthenticationPath     = null,

                        DateTime?                Timestamp              = null,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null,
                        CancellationToken        CancellationToken      = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            RemoteStartResult? result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartRequest?.Invoke(startTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             Id,
                                             ChargingLocation,
                                             ChargingProduct,
                                             ReservationId,
                                             SessionId,
                                             null,
                                             CSORoamingProvider?.Id,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.IsNull())
                    result = RemoteStartResult.UnknownLocation();

                else if (SessionsStore.SessionExists(SessionId))
                    result = RemoteStartResult.InvalidSessionId();

                else if (AdminStatus.Value == RoamingNetworkAdminStatusTypes.Operational ||
                         AdminStatus.Value == RoamingNetworkAdminStatusTypes.InternalUse)
                {

                    #region Try to lookup the charging station operator given in the EVSE identification...

                    if ((TryGetChargingStationOperatorById(ChargingLocation.ChargingStationOperatorId,     out var chargingStationOperator) ||
                         TryGetChargingStationOperatorById(ChargingLocation.EVSEId?.           OperatorId, out     chargingStationOperator) ||
                         TryGetChargingStationOperatorById(ChargingLocation.ChargingStationId?.OperatorId, out     chargingStationOperator) ||
                         TryGetChargingStationOperatorById(ChargingLocation.ChargingPoolId?.   OperatorId, out     chargingStationOperator)) &&
                        chargingStationOperator is not null)
                    {

                        result = await chargingStationOperator.
                                           RemoteStart(ChargingLocation,
                                                       ChargingProduct,
                                                       ReservationId,
                                                       SessionId,
                                                       ProviderId,
                                                       RemoteAuthentication,
                                                       AuthenticationPath,

                                                       Timestamp,
                                                       EventTrackingId,
                                                       RequestTimeout,
                                                       CancellationToken);


                        if (result.Result == RemoteStartResultTypes.Success ||
                            result.Result == RemoteStartResultTypes.AsyncOperation)
                        {

                            SessionsStore.RemoteStart(result.Session,
                                                      result,
                                                      session => {
                                                          session.CSORoamingProviderStart = CSORoamingProvider;
                                                      });

                        }

                    }

                    #endregion

                    //ToDo: Add routing!
                    #region ...or try every CSO roaming provider...

                    if (result        is     null ||
                       (result        is not null &&
                       (result.Result == RemoteStartResultTypes.UnknownLocation)))
                    {

                        foreach (var empRoamingProvider in empRoamingProviders.
                                                                OrderBy(empRoamingServiceWithPriority => empRoamingServiceWithPriority.Key).
                                                                Select (empRoamingServiceWithPriority => empRoamingServiceWithPriority.Value))
                        {

                            result = await empRoamingProvider.
                                               RemoteStart(ChargingLocation,
                                                           ChargingProduct,
                                                           ReservationId,
                                                           SessionId,
                                                           ProviderId,
                                                           RemoteAuthentication,
                                                           AuthenticationPath,

                                                           Timestamp,
                                                           EventTrackingId,
                                                           RequestTimeout,
                                                           CancellationToken);


                            if (result.Result == RemoteStartResultTypes.Success ||
                                result.Result == RemoteStartResultTypes.AsyncOperation)
                            {

                                SessionsStore.RemoteStart(result.Session,
                                                          result,
                                                          session => {
                                                              session.CSORoamingProviderStart = CSORoamingProvider;
                                                              session.EMPRoamingProviderStart = empRoamingProvider;
                                                          });

                            }

                        }

                    }

                    #endregion

                    #region ...or fail!

                    result ??= RemoteStartResult.UnknownOperator();

                    #endregion

                }
                else
                {

                    result = AdminStatus.Value switch {
                        _ => RemoteStartResult.OutOfService(),
                    };

                }

            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }


            #region Send OnRemoteStartResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartResponse?.Invoke(endTime,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              Id,
                                              ChargingLocation,
                                              ChargingProduct,
                                              ReservationId,
                                              SessionId,
                                              null,
                                              CSORoamingProvider?.Id,
                                              ProviderId,
                                              RemoteAuthentication,
                                              RequestTimeout,
                                              result,
                                              endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop (SessionId, ReservationHandling = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       EMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,
                       Auth_Path?             AuthenticationPath     = null,

                       DateTime?              Timestamp              = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null,
                       CancellationToken      CancellationToken      = default)

                => RemoteStop(null,
                              SessionId,
                              ReservationHandling,
                              ProviderId,
                              RemoteAuthentication,
                              AuthenticationPath,

                              Timestamp,
                              EventTrackingId,
                              RequestTimeout,
                              CancellationToken);


        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(ICSORoamingProvider    CSORoamingProvider,
                       ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       EMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,
                       Auth_Path?             AuthenticationPath     = null,

                       DateTime?              Timestamp              = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null,
                       CancellationToken      CancellationToken      = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            RemoteStopResult? result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopRequest?.Invoke(startTime,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            Id,
                                            SessionId,
                                            ReservationHandling,
                                            null,
                                            CSORoamingProvider?.Id,
                                            ProviderId,
                                            RemoteAuthentication,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion

            try
            {

                #region A matching charging session was found...

                if (SessionsStore.TryGet(SessionId, out var chargingSession) && chargingSession is not null)
                {

                    //ToDo: Add a --useForce Option to overwrite!
                    if (chargingSession.SessionTime.EndTime.HasValue)
                        result = RemoteStopResult.AlreadyStopped(SessionId,
                                                                 Runtime: org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - startTime);

                    else
                    {

                        #region When a charging station operator was found...

                        if (chargingSession.ChargingStationOperator is null && chargingSession.ChargingStationOperatorId.HasValue)
                            chargingSession.ChargingStationOperator = GetChargingStationOperatorById(chargingSession.ChargingStationOperatorId.Value);

                        if (chargingSession.ChargingStationOperator is not null)
                            result = await chargingSession.ChargingStationOperator.
                                                RemoteStop(chargingSession.Id,
                                                           ReservationHandling,
                                                           ProviderId,
                                                           RemoteAuthentication,
                                                           AuthenticationPath,

                                                           Timestamp,
                                                           EventTrackingId,
                                                           RequestTimeout,
                                                           CancellationToken);

                        #endregion

                        #region ...or when a charging station roaming provider was found

                        if (result is null)
                        {

                            if (chargingSession.EMPRoamingProviderStart is null && chargingSession.EMPRoamingProviderIdStart.HasValue)
                                chargingSession.EMPRoamingProviderStart = GetEMPRoamingProviderById(chargingSession.EMPRoamingProviderIdStart.Value);

                            if (chargingSession.EMPRoamingProviderStart is not null)
                                result = await chargingSession.EMPRoamingProviderStart.
                                                    RemoteStop(chargingSession.Id,
                                                               ReservationHandling,
                                                               ProviderId,
                                                               RemoteAuthentication,
                                                               AuthenticationPath,

                                                               Timestamp,
                                                               EventTrackingId,
                                                               RequestTimeout,
                                                               CancellationToken);

                            if (result?.Result == RemoteStopResultTypes.Success)
                            {
                                chargingSession.CSORoamingProviderStop = chargingSession.CSORoamingProviderStart;
                            }

                        }

                        #endregion

                    }

                }

                #endregion

                #region ...or try to check every charging station operator...

                if (result        is     null ||
                   (result        is not null &&
                   (result.Result == RemoteStopResultTypes.InvalidSessionId)))
                {

                    foreach (var chargingStationOperator in chargingStationOperators)
                    {

                        result = await chargingStationOperator.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,
                                                      AuthenticationPath,

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

                        if (result != null && result.Result != RemoteStopResultTypes.InvalidSessionId)
                            break;

                    }

                }

                #endregion

                #region ...or try to check every CSO roaming provider...

                if (result        is     null ||
                   (result        is not null &&
                   (result.Result == RemoteStopResultTypes.InvalidSessionId)))
                {

                    foreach (var empRoamingProvider in empRoamingProviders.
                                                           OrderBy(empRoamingServiceWithPriority => empRoamingServiceWithPriority.Key).
                                                           Select (empRoamingServiceWithPriority => empRoamingServiceWithPriority.Value))
                    {

                        result = await empRoamingProvider.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,
                                                      AuthenticationPath,

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

                        if (result is not null && result.Result != RemoteStopResultTypes.InvalidSessionId)
                            break;

                    }

                }

                #endregion

                #region ...or fail!

                result ??= RemoteStopResult.InvalidSessionId(SessionId);

                #endregion

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(SessionId,
                                                e.Message,
                                                Runtime: org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - startTime);
            }


            SessionsStore.RemoteStop(SessionId,
                                     RemoteAuthentication,
                                     ProviderId,
                                     CSORoamingProvider,
                                     result);


            #region Send OnRemoteStopResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopResponse?.Invoke(endTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             Id,
                                             SessionId,
                                             ReservationHandling,
                                             null,
                                             CSORoamingProvider?.Id,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout,
                                             result,
                                             endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion


            if (result.ChargeDetailRecord is not null)
                await SendChargeDetailRecord(result.ChargeDetailRecord,
                                             CancellationToken: CancellationToken);


            return result;

        }

        #endregion

        #endregion


        #region ToJSON(this RoamingNetwork, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given roaming network.
        /// </summary>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        public JObject ToJSON(Boolean                                                     Embedded                                  = false,
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
                              CustomJObjectSerializerDelegate<IEVSE>?                     CustomEVSESerializer                      = null)
        {

            var JSON = JSONObject.Create(

                               new JProperty("@id",             Id.         ToString()),

                         !Embedded
                             ? new JProperty("@context",        JSONLDContext)
                             : null,

                               new JProperty("name",            Name.       ToJSON()),

                         Description.IsNotNullOrEmpty()
                             ? new JProperty("description",     Description.ToJSON())
                             : null,

                         DataSource is not null && DataSource.IsNeitherNullNorEmpty()
                             ? new JProperty("dataSource",      DataSource)
                             : null,

                         DataLicenses.Any()
                             ? ExpandDataLicenses.Switch(
                                 () => new JProperty("dataLicenseIds",  new JArray(DataLicenses.SafeSelect(license => license.Id.ToString()))),
                                 () => new JProperty("dataLicenses",    DataLicenses.ToJSON()))
                             : null,

                         ChargingStationOperators.Any()
                             ? ExpandChargingStationOperatorIds.Switch(

                                   () => new JProperty("chargingStationOperatorIds",  new JArray(ChargingStationOperatorIds().
                                                                                                                OrderBy(id => id).
                                                                                                                Select (id => id.ToString()))),

                                   () => new JProperty("chargingStationOperators",    new JArray(ChargingStationOperators.
                                                                                                                OrderBy(cso => cso).
                                                                                                                ToJSON (Embedded: true,
                                                                                                                        ExpandRoamingNetworkId:                   InfoStatus.Hidden,
                                                                                                                        ExpandChargingPoolIds:                    InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationIds:                 InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds:                            InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                           InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:                       InfoStatus.Hidden,
                                                                                                                        CustomChargingStationOperatorSerializer:  CustomChargingStationOperatorSerializer,
                                                                                                                        CustomChargingPoolSerializer:             CustomChargingPoolSerializer,
                                                                                                                        CustomChargingStationSerializer:          CustomChargingStationSerializer,
                                                                                                                        CustomEVSESerializer:                     CustomEVSESerializer))))
                             : null,

                         !ChargingStationOperators.Any() || ExpandChargingStationOperatorIds == InfoStatus.Expanded
                             ? null
                             : ExpandChargingPoolIds.Switch(
                                   () => new JProperty("chargingPoolIds",             new JArray(ChargingPoolIds().
                                                                                                                OrderBy(id => id).
                                                                                                                Select (id => id.ToString()))),

                                   () => new JProperty("chargingPools",               new JArray(ChargingPools.
                                                                                                                OrderBy(pool => pool).
                                                                                                                ToJSON (Embedded: true,
                                                                                                                        ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationIds:         InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:               InfoStatus.Hidden,
                                                                                                                        CustomChargingPoolSerializer:     CustomChargingPoolSerializer,
                                                                                                                        CustomChargingStationSerializer:  CustomChargingStationSerializer,
                                                                                                                        CustomEVSESerializer:             CustomEVSESerializer)))),

                         !ChargingStationOperators.Any() || (ExpandChargingPoolIds == InfoStatus.Expanded || ExpandChargingStationOperatorIds == InfoStatus.Expanded)
                             ? null
                             : ExpandChargingStationIds.Switch(
                                   () => new JProperty("chargingStationIds",          new JArray(ChargingStationIds().
                                                                                                                OrderBy(id => id).
                                                                                                                Select (id => id.ToString()))),

                                   () => new JProperty("chargingStations",            new JArray(ChargingStations.
                                                                                                                OrderBy(station => station).
                                                                                                                ToJSON (Embedded: true,
                                                                                                                        ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                                        ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds:                    InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:               InfoStatus.Hidden,
                                                                                                                        CustomChargingStationSerializer:  CustomChargingStationSerializer,
                                                                                                                        CustomEVSESerializer:             CustomEVSESerializer)))),

                         !ChargingStationOperators.Any() || (ExpandChargingStationIds == InfoStatus.Expanded || ExpandChargingPoolIds == InfoStatus.Expanded || ExpandChargingStationOperatorIds == InfoStatus.Expanded)
                             ? null
                             : ExpandEVSEIds.Switch(
                                   () => new JProperty("EVSEIds",                     new JArray(EVSEIds().
                                                                                                                OrderBy(id => id).
                                                                                                                Select (id => id.ToString()))),

                                   () => new JProperty("EVSEs",                       new JArray(EVSEs.
                                                                                                                OrderBy(evse => evse).
                                                                                                                ToJSON (Embedded: true,
                                                                                                                        ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationOperatorId:  InfoStatus.Hidden,
                                                                                                                        ExpandChargingPoolId:             InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationId:          InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:               InfoStatus.Hidden)))),


                         EMobilityProviders.Any()
                             ? ExpandEMobilityProviderId.Switch(
                                   () => new JProperty("eMobilityProviderIds",        new JArray(EMobilityProviderIds().
                                                                                                                OrderBy(id => id).
                                                                                                                Select (id => id.ToString()))),

                                   () => new JProperty("eMobilityProviders",          new JArray(EMobilityProviders.
                                                                                                                OrderBy(emp => emp).
                                                                                                                ToJSON (Embedded: true,
                                                                                                                        ExpandRoamingNetworkId:           InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                   InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:               InfoStatus.Hidden))))
                             : null

                         );

            return CustomRoamingNetworkSerializer is not null
                       ? CustomRoamingNetworkSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetwork1, RoamingNetwork2)

        /// <summary>
        /// Compares two roaming networks for equality.
        /// </summary>
        /// <param name="RoamingNetwork1">A roaming network.</param>
        /// <param name="RoamingNetwork2">Another roaming network.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RoamingNetwork? RoamingNetwork1,
                                           RoamingNetwork? RoamingNetwork2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RoamingNetwork1, RoamingNetwork2))
                return true;

            // If one is null, but not both, return false.
            if (RoamingNetwork1 is null || RoamingNetwork2 is null)
                return false;

            return RoamingNetwork1.Equals(RoamingNetwork2);

        }

        #endregion

        #region Operator != (RoamingNetwork1, RoamingNetwork2)

        /// <summary>
        /// Compares two roaming networks for inequality.
        /// </summary>
        /// <param name="RoamingNetwork1">A roaming network.</param>
        /// <param name="RoamingNetwork2">Another roaming network.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RoamingNetwork? RoamingNetwork1,
                                           RoamingNetwork? RoamingNetwork2)

            => !(RoamingNetwork1 == RoamingNetwork2);

        #endregion

        #region Operator <  (RoamingNetwork1, RoamingNetwork2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork1">A roaming network.</param>
        /// <param name="RoamingNetwork2">Another roaming network.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetwork? RoamingNetwork1,
                                          RoamingNetwork? RoamingNetwork2)
        {

            if (RoamingNetwork1 is null)
                throw new ArgumentNullException(nameof(RoamingNetwork1), "The given roaming network must not be null!");

            return RoamingNetwork1.CompareTo(RoamingNetwork2) < 0;

        }

        #endregion

        #region Operator <= (RoamingNetwork1, RoamingNetwork2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork1">A roaming network.</param>
        /// <param name="RoamingNetwork2">Another roaming network.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetwork? RoamingNetwork1,
                                           RoamingNetwork? RoamingNetwork2)

            => !(RoamingNetwork1 > RoamingNetwork2);

        #endregion

        #region Operator >  (RoamingNetwork1, RoamingNetwork2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork1">A roaming network.</param>
        /// <param name="RoamingNetwork2">Another roaming network.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetwork? RoamingNetwork1,
                                          RoamingNetwork? RoamingNetwork2)
        {

            if (RoamingNetwork1 is null)
                throw new ArgumentNullException(nameof(RoamingNetwork1), "The given roaming network must not be null!");

            return RoamingNetwork1.CompareTo(RoamingNetwork2) > 0;

        }

        #endregion

        #region Operator >= (RoamingNetwork1, RoamingNetwork2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork1">A roaming network.</param>
        /// <param name="RoamingNetwork2">Another roaming network.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetwork? RoamingNetwork1,
                                           RoamingNetwork? RoamingNetwork2)

            => !(RoamingNetwork1 < RoamingNetwork2);

        #endregion

        #endregion

        #region IComparable<RoamingNetwork> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two roaming networks.
        /// </summary>
        /// <param name="Object">A roaming network to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is RoamingNetwork roamingNetwork
                   ? CompareTo(roamingNetwork)
                   : throw new ArgumentException("The given object is not a roaming network!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetwork)

        /// <summary>
        /// Compares two roaming networks.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to compare with.</param>
        public Int32 CompareTo(RoamingNetwork? RoamingNetwork)
        {

            if (RoamingNetwork is null)
                throw new ArgumentNullException(nameof(RoamingNetwork),
                                                "The given roaming network must not be null!");

            return Id.CompareTo(RoamingNetwork.Id);

            //ToDo: Compare more properties!

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetwork> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two roaming networks for equality.
        /// </summary>
        /// <param name="Object">A roaming network to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetwork roamingNetwork &&
                   Equals(roamingNetwork);

        #endregion

        #region Equals(RoamingNetwork)

        /// <summary>
        /// Compares two roaming networks for equality.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to compare with.</param>
        public Boolean Equals(RoamingNetwork? RoamingNetwork)

            => RoamingNetwork is not null &&
                   Id.Equals(RoamingNetwork.Id);

        //ToDo: Compare more properties!

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"'{Name.FirstText()}' ({Id})";

        #endregion


    }

}
