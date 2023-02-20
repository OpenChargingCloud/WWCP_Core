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

using cloud.charging.open.protocols.WWCP.Net.IO.JSON;
using cloud.charging.open.protocols.WWCP.Networking;

using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

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
        public static JArray ToJSON(this IEnumerable<RoamingNetwork>                           RoamingNetworks,
                                    Boolean                                                    Embedded                                  = false,
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
                                    CustomJObjectSerializerDelegate<EVSE>?                     CustomEVSESerializer                      = null,
                                    UInt64?                                                    Skip                                      = null,
                                    UInt64?                                                    Take                                      = null)
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
        public const String JSONLDContext                   = "https://open.charging.cloud/contexts/wwcp+json/roamingNetwork";

        /// <summary>
        /// The default max size of the admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 15;

        /// <summary>
        /// The default max size of the status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 15;

        /// <summary>
        /// The request timeout.
        /// </summary>
        public readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(60);


        protected static readonly  SemaphoreSlim  chargingStationOperatorsSemaphore      = new (1, 1);

        protected static readonly  TimeSpan       SemaphoreSlimTimeout                   = TimeSpan.FromSeconds(5);

        protected static readonly  Byte           MinChargingStationOperatorIdLength     = 5;
        protected static readonly  Byte           MinChargingStationOperatorNameLength   = 5;

        #endregion

        #region Properties

        IId IAuthorizeStartStop.AuthId
            => Id;

        IId ISendChargeDetailRecords.Id
            => Id;

        public Boolean DisableAuthentication            { get; set; }

        public Boolean DisableSendChargeDetailRecords   { get; set; }

        public Boolean DisableNetworkSync               { get; set; }


        #region DataLicense

        private ReactiveSet<DataLicense> dataLicenses;

        /// <summary>
        /// The license of the roaming network data.
        /// </summary>
        [Mandatory]
        public ReactiveSet<DataLicense> DataLicenses
            => dataLicenses;

        #endregion


        public ChargingReservationsStore  ReservationsStore           { get; }
        public ChargingSessionsStore      SessionsStore               { get; }

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


        /// <summary>
        /// A delegate for filtering charge detail records.
        /// </summary>
        public ChargeDetailRecordFilterDelegate?          ChargeDetailRecordFilter                     { get; }


        public String                                     LoggingPath                                  { get; }

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
        /// <param name="MaxAdminStatusListSize">The maximum number of entries in the admin status history.</param>
        /// <param name="MaxStatusListSize">The maximum number of entries in the status history.</param>
        /// <param name="ChargingStationSignatureGenerator">A delegate to sign a charging station.</param>
        /// <param name="ChargingPoolSignatureGenerator">A delegate to sign a charging pool.</param>
        /// <param name="ChargingStationOperatorSignatureGenerator">A delegate to sign a charging station operator.</param>
        public RoamingNetwork(RoamingNetwork_Id                          Id,
                              I18NString?                                Name                                        = null,
                              I18NString?                                Description                                 = null,
                              RoamingNetworkAdminStatusTypes?            InitialAdminStatus                          = null,
                              RoamingNetworkStatusTypes?                 InitialStatus                               = null,
                              UInt16?                                    MaxAdminStatusListSize                      = null,
                              UInt16?                                    MaxStatusListSize                           = null,

                              ChargingStationSignatureDelegate?          ChargingStationSignatureGenerator           = null,
                              ChargingPoolSignatureDelegate?             ChargingPoolSignatureGenerator              = null,
                              ChargingStationOperatorSignatureDelegate?  ChargingStationOperatorSignatureGenerator   = null,

                              IEnumerable<RoamingNetworkInfo>?           RoamingNetworkInfos                         = null,
                              Boolean                                    DisableNetworkSync                          = true,
                              String?                                    LoggingPath                                 = null,

                              String?                                    DataSource                                  = null,
                              DateTime?                                  LastChange                                  = null,

                              JObject?                                   CustomData                                  = null,
                              UserDefinedDictionary?                     InternalData                                = null)

            : base(Id,
                   Name                   ?? new I18NString(Languages.en, "RNTest1"),
                   Description            ?? new I18NString(Languages.en, "A roaming network for testing purposes"),
                   InitialAdminStatus     ?? RoamingNetworkAdminStatusTypes.Operational,
                   InitialStatus          ?? RoamingNetworkStatusTypes.Available,
                   MaxAdminStatusListSize ?? DefaultMaxAdminStatusListSize,
                   MaxStatusListSize      ?? DefaultMaxStatusListSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Init data and properties

            this.dataLicenses                                       = new ReactiveSet<DataLicense>();

            this.empRoamingProviders                                = new ConcurrentDictionary<EMPRoamingProvider_Id, IEMPRoamingProvider>();
            this._eMobilityRoamingServices                          = new ConcurrentDictionary<UInt32, IEMPRoamingProvider>();
            this.eMobilityProviders                                 = new EntityHashSet<RoamingNetwork, EMobilityProvider_Id,       EMobilityProvider>      (this);

            this.csoRoamingProviders                                = new ConcurrentDictionary<CSORoamingProvider_Id, ICSORoamingProvider>();
            this.chargingStationOperators                           = new EntityHashSet<IRoamingNetwork, ChargingStationOperator_Id, IChargingStationOperator>(this);

            this.parkingOperators                                  = new EntityHashSet<RoamingNetwork, ParkingOperator_Id,         ParkingOperator>        (this);
            this._SmartCities                                       = new EntityHashSet<RoamingNetwork, SmartCity_Id,               SmartCityProxy>         (this);
            this._NavigationProviders                               = new EntityHashSet<RoamingNetwork, NavigationProvider_Id,      NavigationProvider>     (this);
            this.gridOperators                                     = new EntityHashSet<RoamingNetwork, GridOperator_Id,            GridOperator>           (this);

            //this._PushEVSEDataToOperatorRoamingServices             = new ConcurrentDictionary<UInt32, IPushData>();
            //this._PushEVSEStatusToOperatorRoamingServices           = new ConcurrentDictionary<UInt32, IPushStatus>();

            this.LoggingPath                                        = LoggingPath ?? AppContext.BaseDirectory;
            Directory.CreateDirectory(this.LoggingPath);

            this.DisableNetworkSync                                 = DisableNetworkSync;

            this.ReservationsStore                                  = new ChargingReservationsStore(this.Id,
                                                                                                    DisableNetworkSync:   true,
                                                                                                    LoggingPath:          this.LoggingPath);

            this.SessionsStore                                      = new ChargingSessionsStore    (this,
                                                                                                    ReloadDataOnStart:    false,
                                                                                                    RoamingNetworkInfos:  RoamingNetworkInfos,
                                                                                                    DisableNetworkSync:   DisableNetworkSync,
                                                                                                    LoggingPath:          this.LoggingPath);

            this.ChargingStationSignatureGenerator                  = ChargingStationSignatureGenerator;
            this.ChargingPoolSignatureGenerator                     = ChargingPoolSignatureGenerator;
            this.ChargingStationOperatorSignatureGenerator          = ChargingStationOperatorSignatureGenerator;

            #endregion

            #region Init events

            // RoamingNetwork events

            this.CPORoamingProviderAddition  = new VotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean>(() => new VetoVote(), true);
            this.CPORoamingProviderRemoval   = new VotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean>(() => new VetoVote(), true);

            this.EMPRoamingProviderAddition  = new VotingNotificator<RoamingNetwork, IEMPRoamingProvider, Boolean>(() => new VetoVote(), true);
            this.EMPRoamingProviderRemoval   = new VotingNotificator<RoamingNetwork, IEMPRoamingProvider, Boolean>(() => new VetoVote(), true);

            // cso events
            this.ChargingPoolAddition        = new VotingNotificator<DateTime, IChargingStationOperator, IChargingPool, Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval         = new VotingNotificator<DateTime, IChargingStationOperator, IChargingPool, Boolean>(() => new VetoVote(), true);

            // ChargingPool events
            this.ChargingStationAddition     = new VotingNotificator<DateTime, IChargingPool, IChargingStation, Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval      = new VotingNotificator<DateTime, IChargingPool, IChargingStation, Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition                = new VotingNotificator<DateTime, IChargingStation, IEVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval                 = new VotingNotificator<DateTime, IChargingStation, IEVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events

            #endregion

            this.OnPropertyChanged += UpdateData;

        }

        #endregion


        private readonly PriorityList<ISendPOIData>              _ISendData                        = new();
        private readonly PriorityList<ISendAdminStatus>          _ISendAdminStatus                 = new();
        private readonly PriorityList<ISendStatus>               _ISendStatus                      = new();
        private readonly PriorityList<ISendAuthorizeStartStop>   _ISend2RemoteAuthorizeStartStop   = new();
        private readonly PriorityList<ISendChargeDetailRecords>  _IRemoteSendChargeDetailRecord    = new();

        private readonly ConcurrentDictionary<UInt32, IEMPRoamingProvider> _eMobilityRoamingServices;


        #region Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnRoamingNetworkDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnRoamingNetworkStatusChangedDelegate?       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnRoamingNetworkAdminStatusChangedDelegate?  OnAggregatedAdminStatusChanged;

        #endregion


        #region (internal) UpdateData(Timestamp, EventTrackingId, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data of the roaming network.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="Sender">The changed roaming network.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateData(DateTime          Timestamp,
                                       EventTracking_Id  EventTrackingId,
                                       Object            Sender,
                                       String            PropertyName,
                                       Object            OldValue,
                                       Object            NewValue)
        {

            var OnDataChangedLocal = OnDataChanged;
            if (OnDataChangedLocal is not null)
                await OnDataChangedLocal(Timestamp,
                                         EventTrackingId,
                                         Sender as RoamingNetwork,
                                         PropertyName,
                                         OldValue,
                                         NewValue);

        }

        #endregion

        #endregion


        #region EMP Roaming Providers...

        #region EMPRoamingProviders

        private readonly ConcurrentDictionary<EMPRoamingProvider_Id, IEMPRoamingProvider> empRoamingProviders;

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
        public IEMPRoamingProvider CreateNewRoamingProvider(IEMPRoamingProvider           EMPRoamingProvider,
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

                    _ISendData.Add                     (EMPRoamingProvider);
                    _ISendAdminStatus.Add              (EMPRoamingProvider);
                    _ISendStatus.Add                   (EMPRoamingProvider);
                    _ISend2RemoteAuthorizeStartStop.Add(EMPRoamingProvider);
                    _IRemoteSendChargeDetailRecord.Add (EMPRoamingProvider);

                    EMPRoamingProviderAddition.SendNotification(this, EMPRoamingProvider);

                    SetRoamingProviderPriority(EMPRoamingProvider,
                                               _eMobilityRoamingServices.Count > 0
                                                   ? _eMobilityRoamingServices.Keys.Max() + 1
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

            => _eMobilityRoamingServices.TryAdd(Priority, eMobilityRoamingService);

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

        private readonly EntityHashSet<RoamingNetwork, EMobilityProvider_Id, EMobilityProvider> eMobilityProviders;

        /// <summary>
        /// Return all e-mobility providers registered within this roaming network.
        /// </summary>
        public IEnumerable<EMobilityProvider> EMobilityProviders

            => eMobilityProviders;

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


        #region OnEMobilityProviderAddition

        /// <summary>
        /// Called whenever an e-mobility provider will be or was added.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, EMobilityProvider, Boolean> OnEMobilityProviderAddition
            => eMobilityProviders.OnAddition;

        #endregion

        #region OnEMobilityProviderRemoval

        /// <summary>
        /// Called whenever an e-mobility provider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, EMobilityProvider, Boolean> OnEMobilityProviderRemoval
            => eMobilityProviders.OnRemoval;

        #endregion


        #region CreateNewEMobilityProvider(EMobilityProviderId, Configurator = null)

        /// <summary>
        /// Create and register a new e-mobility (service) provider having the given
        /// unique e-mobility provider identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the new e-mobility provider.</param>
        /// <param name="Name">The offical (multi-language) name of the e-mobility provider.</param>
        /// <param name="Description">An optional (multi-language) description of the e-mobility provider.</param>
        /// <param name="Configurator">An optional delegate to configure the new e-mobility provider before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new e-mobility provider after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the e-mobility provider failed.</param>
        public EMobilityProvider CreateEMobilityProvider(EMobilityProvider_Id                           ProviderId,
                                                         I18NString?                                    Name                             = null,
                                                         I18NString?                                    Description                      = null,
                                                         eMobilityProviderPriority?                     Priority                         = null,
                                                         Action<EMobilityProvider>?                     Configurator                     = null,
                                                         RemoteEMobilityProviderCreatorDelegate?        RemoteEMobilityProviderCreator   = null,
                                                         EMobilityProviderAdminStatusTypes?             InitialAdminStatus               = null,
                                                         EMobilityProviderStatusTypes?                  InitialStatus                    = null,
                                                         Action<EMobilityProvider>?                     OnSuccess                        = null,
                                                         Action<RoamingNetwork, EMobilityProvider_Id>?  OnError                          = null)
        {

            lock (chargingStationOperators)
            {

                var eMobilityProviderProxy = new EMobilityProvider(ProviderId,
                                                                   this,
                                                                   Name,
                                                                   Description,
                                                                   Configurator,
                                                                   RemoteEMobilityProviderCreator,
                                                                   Priority,
                                                                   InitialAdminStatus ?? EMobilityProviderAdminStatusTypes.Operational,
                                                                   InitialStatus      ?? EMobilityProviderStatusTypes.Available);


                if (eMobilityProviders.TryAdd(eMobilityProviderProxy, OnSuccess))
                {

                    // _eMobilityProviders.OnDataChanged         += UpdateData;
                    // _eMobilityProviders.OnStatusChanged       += UpdateStatus;
                    // _eMobilityProviders.OnAdminStatusChanged  += UpdateAdminStatus;

                    //_EMobilityProvider.OnEMobilityStationAddition

                    //AddIRemotePushData               (_EMobilityProvider);
                    _ISendAdminStatus.Add              (eMobilityProviderProxy);
                    _ISendStatus.Add                   (eMobilityProviderProxy);
                    _ISend2RemoteAuthorizeStartStop.Add(eMobilityProviderProxy);
                    _IRemoteSendChargeDetailRecord.Add (eMobilityProviderProxy);


                    // Link events!

                    return eMobilityProviderProxy;

                }

                throw new eMobilityProviderAlreadyExists(this,
                                                         ProviderId);

            }

        }

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
        public Boolean ContainsEMobilityProvider(EMobilityProvider EMobilityProvider)

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

        public EMobilityProvider? GetEMobilityProviderById(EMobilityProvider_Id  EMobilityProviderId)

            => eMobilityProviders.GetById(EMobilityProviderId);

        public EMobilityProvider? GetEMobilityProviderById(EMobilityProvider_Id? EMobilityProviderId)

            => EMobilityProviderId.HasValue
                   ? eMobilityProviders.GetById(EMobilityProviderId.Value)
                   : null;

        #endregion

        #region TryGetEMobilityProviderById(EMobilityProviderId, out EMobilityProvider)

        public Boolean TryGetEMobilityProviderById(EMobilityProvider_Id EMobilityProviderId, out EMobilityProvider? EMobilityProvider)

            => eMobilityProviders.TryGet(EMobilityProviderId, out EMobilityProvider);

        public Boolean TryGetEMobilityProviderById(EMobilityProvider_Id? EMobilityProviderId, out EMobilityProvider? EMobilityProvider)
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

        public EMobilityProvider? RemoveEMobilityProvider(EMobilityProvider_Id EMobilityProviderId)
        {

            if (eMobilityProviders.TryRemove(EMobilityProviderId, out var eMobilityProvider))
                return eMobilityProvider;

            return null;

        }

        public EMobilityProvider? RemoveEMobilityProvider(EMobilityProvider_Id? EMobilityProviderId)
        {

            if (EMobilityProviderId is not null &&
                eMobilityProviders.TryRemove(EMobilityProviderId.Value, out var eMobilityProvider))
            {
                return eMobilityProvider;
            }

            return null;

        }

        #endregion

        #region TryRemoveEMobilityProvider (EMobilityProviderId, out EMobilityProvider)

        public Boolean TryRemoveEMobilityProvider(EMobilityProvider_Id EMobilityProviderId, out EMobilityProvider? EMobilityProvider)

            => eMobilityProviders.TryRemove(EMobilityProviderId, out EMobilityProvider);

        public Boolean TryRemoveEMobilityProvider(EMobilityProvider_Id? EMobilityProviderId, out EMobilityProvider? EMobilityProvider)

        {

            if (!EMobilityProviderId.HasValue)
            {
                EMobilityProvider = null;
                return false;
            }

            return eMobilityProviders.TryRemove(EMobilityProviderId.Value, out EMobilityProvider);

        }

        #endregion

        #endregion


        #region Charging Station Operator Roaming Providers...

        #region CSORoamingProviders

        private readonly ConcurrentDictionary<CSORoamingProvider_Id, ICSORoamingProvider>  csoRoamingProviders;

        /// <summary>
        /// Return all charging station operator roaming providers registered within this roaming network.
        /// </summary>
        public IEnumerable<ICSORoamingProvider> CSORoamingProviders
            => csoRoamingProviders.Values;

        #endregion


        #region CPORoamingProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean> CPORoamingProviderAddition;

        /// <summary>
        /// Called whenever a charging station operator roaming provider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, ICSORoamingProvider, Boolean> OnCPORoamingProviderAddition
            => CPORoamingProviderAddition;

        #endregion

        #region CPORoamingProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean> CPORoamingProviderRemoval;

        /// <summary>
        /// Called whenever a charging station operator roaming provider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, ICSORoamingProvider, Boolean> OnCPORoamingProviderRemoval
            => CPORoamingProviderRemoval;

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

            if (CPORoamingProviderAddition.SendVoting(this, CSORoamingProvider))
            {
                if (csoRoamingProviders.TryAdd(CSORoamingProvider.Id, CSORoamingProvider))
                {

                    // this.OnChargingStationRemoval.OnNotification += _CPORoamingProvider.RemoveChargingStations;

                    //SetRoamingProviderPriority(_CPORoamingProvider,
                    //                           _ChargingStationOperatorRoamingProviderPriorities.Count > 0
                    //                               ? _ChargingStationOperatorRoamingProviderPriorities.Keys.Max() + 1
                    //                               : 10);

                    //_ISendData.Add                     (chargingStationOperatorRoamingProvider);
                    //_ISendAdminStatus.Add              (chargingStationOperatorRoamingProvider);
                    //_ISendStatus.Add                   (chargingStationOperatorRoamingProvider);
                    //_ISend2RemoteAuthorizeStartStop.Add(chargingStationOperatorRoamingProvider);
                    //_IRemoteSendChargeDetailRecord.Add (chargingStationOperatorRoamingProvider);

                    CPORoamingProviderAddition.SendNotification(this, CSORoamingProvider);

                    return CSORoamingProvider;

                }
            }

            throw new Exception("Could not create new charging station operator roaming provider '" + CSORoamingProvider.Id + "'!");

        }

        #endregion

        #region SetRoamingProviderPriority(csoRoamingProvider, Priority)

        ///// <summary>
        ///// Change the given Charging Station Operator roaming service priority.
        ///// </summary>
        ///// <param name="csoRoamingProvider">The Charging Station Operator roaming provider.</param>
        ///// <param name="Priority">The priority of the service.</param>
        //public Boolean SetRoamingProviderPriority(IChargingStationOperatorRoamingProvider csoRoamingProvider,
        //                                          UInt32                                  Priority)
        //{

        //    var a = _ChargingStationOperatorRoamingProviderPriorities.FirstOrDefault(_ => _.Value == csoRoamingProvider);

        //    if (a.Key > 0)
        //    {
        //        IChargingStationOperatorRoamingProvider b = null;
        //        _ChargingStationOperatorRoamingProviderPriorities.TryRemove(a.Key, out b);
        //    }

        //    return _ChargingStationOperatorRoamingProviderPriorities.TryAdd(Priority, csoRoamingProvider);

        //}

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


        #region ChargingStationOperatorAddition

        /// <summary>
        /// Called whenever a charging station operator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, IRoamingNetwork, IChargingStationOperator, Boolean> OnChargingStationOperatorAddition
            => chargingStationOperators.OnAddition;

        #endregion

        #region ChargingStationOperatorRemoval

        /// <summary>
        /// Called whenever charging station operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, IRoamingNetwork, IChargingStationOperator, Boolean> OnChargingStationOperatorRemoval
            => chargingStationOperators.OnRemoval;

        #endregion


        #region ChargingStationOperatorIds        (IncludeChargingStationOperator = null)

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


        #region (protected internal) _AddChargingStationOperator(ChargingStationOperator, SkipDefaultNotifications = false, OnAdded = null, ...)

        /// <summary>
        /// Add the given user to the API.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="SkipNewChargingStationOperatorNotifications">Do not send notifications for this charging station operator addition.</param>
        /// <param name="OnAdded">A delegate run whenever the user has been added successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        protected internal async Task<AddChargingStationOperatorResult>

            _AddChargingStationOperator(IChargingStationOperator                             ChargingStationOperator,
                                        Boolean                                              SkipNewChargingStationOperatorNotifications   = false,
                                        Action<IChargingStationOperator, EventTracking_Id>?  OnAdded                                       = null,
                                        EventTracking_Id?                                    EventTrackingId                               = null,
                                        User_Id?                                             CurrentUserId                                 = null)

        {

            var eventTrackingId = EventTrackingId ?? EventTracking_Id.New;

            //if (User.API is not null && User.API != this)
            //    return AddUserResult.ArgumentError(User,
            //                                       eventTrackingId,
            //                                       nameof(User),
            //                                       "The given user is already attached to another API!");

            if (chargingStationOperators.ContainsId(ChargingStationOperator.Id))
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator,
                           eventTrackingId,
                           nameof(ChargingStationOperator),
                           $"The given charging station operator identification '{ChargingStationOperator.Id}' already exists!"
                       );

            if (ChargingStationOperator.Id.Length < MinChargingStationOperatorIdLength)
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator,
                           eventTrackingId,
                           nameof(ChargingStationOperator),
                           $"The given charging station operator identification '{ChargingStationOperator.Id}' is too short!"
                       );

            if (ChargingStationOperator.Name.IsNullOrEmpty())
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator,
                           eventTrackingId,
                           nameof(User),
                           "The given charging station operator name must not be null!"
                       );

            if (ChargingStationOperator.Name.FirstText().Length < MinChargingStationOperatorNameLength)
                return AddChargingStationOperatorResult.ArgumentError(
                           ChargingStationOperator,
                           eventTrackingId,
                           nameof(ChargingStationOperator),
                           $"The given charging station operator name '{ChargingStationOperator.Name}' is too short!"
                       );

            //User.API = this;


            //await WriteToDatabaseFile(addUser_MessageType,
            //                          ChargingStationOperator.ToJSON(false),
            //                          eventTrackingId,
            //                          CurrentUserId);

            var result = chargingStationOperators.TryAdd(ChargingStationOperator);

            OnAdded?.Invoke(ChargingStationOperator,
                            eventTrackingId);

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


            return AddChargingStationOperatorResult.Success(ChargingStationOperator,
                                                            eventTrackingId);

        }

        #endregion

        #region AddChargingStationOperator (ChargingStationOperator, SkipDefaultNotifications = false, OnAdded = null, ...)

        /// <summary>
        /// Add the given charging station operator.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="SkipNewChargingStationOperatorNotifications">Do not send notifications for this charging station operator addition.</param>
        /// <param name="OnAdded">A delegate run whenever the user has been added successfully.</param>
        /// <param name="EventTrackingId">An optional unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public async Task<AddChargingStationOperatorResult> AddChargingStationOperator(IChargingStationOperator                             ChargingStationOperator,
                                                                                       Boolean                                              SkipNewChargingStationOperatorNotifications   = false,
                                                                                       Action<IChargingStationOperator, EventTracking_Id>?  OnAdded                                       = null,
                                                                                       EventTracking_Id?                                    EventTrackingId                               = null,
                                                                                       User_Id?                                             CurrentUserId                                 = null)
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

                    if (result.IsSuccess &&
                        result.ChargingStationOperator is not null)
                    {

                        result.ChargingStationOperator.OnDataChanged                              += UpdateCSOData;
                        result.ChargingStationOperator.OnStatusChanged                            += UpdateCSOStatus;
                        result.ChargingStationOperator.OnAdminStatusChanged                       += UpdateCSOAdminStatus;

                        result.ChargingStationOperator.OnChargingPoolAddition.   OnVoting         += (timestamp, cso, pool, vote)      => ChargingPoolAddition.   SendVoting      (timestamp, cso, pool, vote);
                        result.ChargingStationOperator.OnChargingPoolAddition.   OnNotification   += SendChargingPoolAdded;
                        result.ChargingStationOperator.OnChargingPoolDataChanged                  += UpdateChargingPoolData;
                        result.ChargingStationOperator.OnChargingPoolStatusChanged                += UpdateChargingPoolStatus;
                        result.ChargingStationOperator.OnChargingPoolAdminStatusChanged           += UpdateChargingPoolAdminStatus;
                        result.ChargingStationOperator.OnChargingPoolRemoval.    OnVoting         += (timestamp, cso, pool, vote)      => ChargingPoolRemoval.    SendVoting      (timestamp, cso, pool, vote);
                        result.ChargingStationOperator.OnChargingPoolRemoval.    OnNotification   += (timestamp, cso, pool)            => ChargingPoolRemoval.    SendNotification(timestamp, cso, pool);

                        result.ChargingStationOperator.OnChargingStationAddition.OnVoting         += (timestamp, pool, station, vote)  => ChargingStationAddition.SendVoting      (timestamp, pool, station, vote);
                        result.ChargingStationOperator.OnChargingStationAddition.OnNotification   += SendChargingStationAdded;
                        result.ChargingStationOperator.OnChargingStationDataChanged               += UpdateChargingStationData;
                        result.ChargingStationOperator.OnChargingStationStatusChanged             += UpdateChargingStationStatus;
                        result.ChargingStationOperator.OnChargingStationAdminStatusChanged        += UpdateChargingStationAdminStatus;
                        result.ChargingStationOperator.OnChargingStationRemoval. OnVoting         += (timestamp, pool, station, vote)  => ChargingStationRemoval. SendVoting      (timestamp, pool, station, vote);
                        result.ChargingStationOperator.OnChargingStationRemoval. OnNotification   += (timestamp, pool, station)        => ChargingStationRemoval. SendNotification(timestamp, pool, station);

                        result.ChargingStationOperator.OnEVSEAddition.           OnVoting         += (timestamp, station, evse, vote)  => EVSEAddition.           SendVoting      (timestamp, station, evse, vote);
                        result.ChargingStationOperator.OnEVSEAddition.           OnNotification   += SendEVSEAdded;
                        result.ChargingStationOperator.OnEVSEDataChanged                          += UpdateEVSEData;
                        result.ChargingStationOperator.OnEVSEStatusChanged                        += UpdateEVSEStatus;
                        result.ChargingStationOperator.OnEVSEAdminStatusChanged                   += UpdateEVSEAdminStatus;
                        result.ChargingStationOperator.OnEVSERemoval.            OnVoting         += (timestamp, station, evse, vote)  => EVSERemoval.            SendVoting      (timestamp, station, evse, vote);
                        result.ChargingStationOperator.OnEVSERemoval.            OnNotification   += (timestamp, station, evse)        => EVSERemoval.            SendNotification(timestamp, station, evse);

                        result.ChargingStationOperator.OnNewReservation                           += SendNewReservation;
                        result.ChargingStationOperator.OnReservationCanceled                      += SendReservationCanceled;
                        result.ChargingStationOperator.OnNewChargingSession                       += SendNewChargingSession;
                        result.ChargingStationOperator.OnNewChargeDetailRecord                    += SendNewChargeDetailRecord;

                    }

                    return result;

                }
                catch (Exception e)
                {

                    DebugX.LogException(e, $"{nameof(RoamingNetwork)}.{nameof(AddChargingStationOperator)}({ChargingStationOperator.Id}, ...)" );

                    return AddChargingStationOperatorResult.Failed(ChargingStationOperator,
                                                                   eventTrackingId,
                                                                   e);

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

            return AddChargingStationOperatorResult.Failed(ChargingStationOperator,
                                                           eventTrackingId,
                                                           "Internal locking failed!");

        }

        #endregion


        #region CreateChargingStationOperator(Id, Name = null, Description = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station operator having the given
        /// unique charging station operator identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new charging station operator.</param>
        /// <param name="Name">The offical (multi-language) name of the charging station operator.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station operator before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station operator after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station operator failed.</param>
        public async Task<AddChargingStationOperatorResult>

            CreateChargingStationOperator(ChargingStationOperator_Id                           Id,
                                          I18NString?                                          Name                                   = null,
                                          I18NString?                                          Description                            = null,
                                          Action<IChargingStationOperator>?                    Configurator                           = null,
                                          RemoteChargingStationOperatorCreatorDelegate?        RemoteChargingStationOperatorCreator   = null,
                                          ChargingStationOperatorAdminStatusTypes?             InitialAdminStatus                     = null,
                                          ChargingStationOperatorStatusTypes?                  InitialStatus                          = null,
                                          Action<IChargingStationOperator>?                    OnSuccess                              = null,
                                          Action<RoamingNetwork, ChargingStationOperator_Id>?  OnError                                = null,
                                          EventTracking_Id?                                    EventTrackingId                        = null,
                                          User_Id?                                             CurrentUserId                          = null)

        {

            return await AddChargingStationOperator(
                             new ChargingStationOperator(
                                 Id,
                                 this,
                                 Name,
                                 Description,
                                 Configurator,
                                 RemoteChargingStationOperatorCreator,
                                 InitialAdminStatus ?? ChargingStationOperatorAdminStatusTypes.Operational,
                                 InitialStatus      ?? ChargingStationOperatorStatusTypes.Available
                             )
                         );

        }

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

        #region RemoveChargingStationOperator    (ChargingStationOperatorId)

        public async Task<RemoveChargingStationOperatorResult>

            RemoveChargingStationOperator(ChargingStationOperator_Id ChargingStationOperatorId)

        {

            if (chargingStationOperators.TryRemove(ChargingStationOperatorId, out var chargingStationOperator) &&
                chargingStationOperator is not null)
            {

                return RemoveChargingStationOperatorResult.Success(
                           chargingStationOperator,
                           EventTracking_Id.New,
                           this
                       );

            }

            return RemoveChargingStationOperatorResult.Failed(
                       ChargingStationOperatorId,
                       EventTracking_Id.New,
                       ""
                   );

        }

        //public async Task<RemoveChargingStationOperatorResult> RemoveChargingStationOperator(ChargingStationOperator_Id? ChargingStationOperatorId)
        //{

        //    if (ChargingStationOperatorId.HasValue &&
        //        chargingStationOperators.TryRemove(ChargingStationOperatorId.Value, out var chargingStationOperator) &&
        //        chargingStationOperator is not null)
        //    {

        //        return RemoveChargingStationOperatorResult.Success(
        //                   chargingStationOperator,
        //                   EventTracking_Id.New,
        //                   this
        //               );

        //    }

        //    return RemoveChargingStationOperatorResult.Failed(
        //               default,
        //               EventTracking_Id.New,
        //               ""
        //           );

        //}

        #endregion

        #region TryRemoveChargingStationOperator (ChargingStationOperatorId, out ChargingStationOperator)

        public Boolean TryRemoveChargingStationOperator(ChargingStationOperator_Id ChargingStationOperatorId, out IChargingStationOperator? ChargingStationOperator)

            => chargingStationOperators.TryRemove(ChargingStationOperatorId, out ChargingStationOperator);

        //public Boolean TryRemoveChargingStationOperator(ChargingStationOperator_Id? ChargingStationOperatorId, out IChargingStationOperator? ChargingStationOperator)
        //{

        //    if (!ChargingStationOperatorId.HasValue)
        //    {
        //        ChargingStationOperator = null;
        //        return false;
        //    }

        //    return chargingStationOperators.TryRemove(ChargingStationOperatorId.Value, out ChargingStationOperator);

        //}

        #endregion


        #region OnChargingStationOperatorData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated Charging Station Operator changed.
        /// </summary>
        public event OnChargingStationOperatorDataChangedDelegate?         OnChargingStationOperatorDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated Charging Station Operator changed.
        /// </summary>
        public event OnChargingStationOperatorStatusChangedDelegate?       OnChargingStationOperatorStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated Charging Station Operator changed.
        /// </summary>
        public event OnChargingStationOperatorAdminStatusChangedDelegate?  OnChargingStationOperatorAdminStatusChanged;

        #endregion


        #region (internal) UpdateCSOAdminStatus(Timestamp, ChargingStationOperator, OldStatus, NewStatus)

        /// <summary>
        /// Update the current Charging Station Operator admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStationOperator">The updated Charging Station Operator.</param>
        /// <param name="OldStatus">The old aggreagted Charging Station Operator status.</param>
        /// <param name="NewStatus">The new aggreagted Charging Station Operator status.</param>
        internal async Task UpdateCSOAdminStatus(DateTime                                              Timestamp,
                                                 IChargingStationOperator                              ChargingStationOperator,
                                                 Timestamped<ChargingStationOperatorAdminStatusTypes>  OldStatus,
                                                 Timestamped<ChargingStationOperatorAdminStatusTypes>  NewStatus)
        {

            var OnChargingStationOperatorAdminStatusChangedLocal = OnChargingStationOperatorAdminStatusChanged;
            if (OnChargingStationOperatorAdminStatusChangedLocal is not null)
                await OnChargingStationOperatorAdminStatusChangedLocal(Timestamp,
                                                                       ChargingStationOperator,
                                                                       OldStatus,
                                                                       NewStatus);


            // Calculate new aggregated roaming network status and send upstream
            //if (AdminStatusAggregationDelegate != null)
            //{

            //    var NewAggregatedStatus = new Timestamped<RoamingNetworkAdminStatusType>(AdminStatusAggregationDelegate(new csoAdminStatusReport(_ChargingStationOperators.Values)));

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

        #region (internal) UpdateCSOStatus     (Timestamp, ChargingStationOperator, OldStatus, NewStatus)

        /// <summary>
        /// Update the current Charging Station Operator status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStationOperator">The updated Charging Station Operator.</param>
        /// <param name="OldStatus">The old aggreagted Charging Station Operator status.</param>
        /// <param name="NewStatus">The new aggreagted Charging Station Operator status.</param>
        internal async Task UpdateCSOStatus(DateTime                                         Timestamp,
                                            IChargingStationOperator                         ChargingStationOperator,
                                            Timestamped<ChargingStationOperatorStatusTypes>  OldStatus,
                                            Timestamped<ChargingStationOperatorStatusTypes>  NewStatus)
        {

            var OnChargingStationOperatorStatusChangedLocal = OnChargingStationOperatorStatusChanged;
            if (OnChargingStationOperatorStatusChangedLocal is not null)
                await OnChargingStationOperatorStatusChangedLocal(Timestamp,
                                                                  ChargingStationOperator,
                                                                  OldStatus,
                                                                  NewStatus);


            // Calculate new aggregated roaming network status and send upstream
            //if (StatusAggregationDelegate != null)
            //{

            //    var NewAggregatedStatus = new Timestamped<RoamingNetworkStatusType>(StatusAggregationDelegate(new csoStatusReport(_ChargingStationOperators.Values)));

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

        #region (internal) UpdateCSOData       (Timestamp, ChargingStationOperator, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an evse operator.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStationOperator">The changed evse operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateCSOData(DateTime                  Timestamp,
                                          IChargingStationOperator  ChargingStationOperator,
                                          String                    PropertyName,
                                          Object?                   OldValue,
                                          Object?                   NewValue)
        {

            var OnChargingStationOperatorDataChangedLocal = OnChargingStationOperatorDataChanged;
            if (OnChargingStationOperatorDataChangedLocal is not null)
                await OnChargingStationOperatorDataChangedLocal(Timestamp,
                                                                ChargingStationOperator,
                                                                PropertyName,
                                                                OldValue,
                                                                NewValue);

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


        #region ChargingPoolAddition

        internal readonly IVotingNotificator<DateTime, IChargingStationOperator, IChargingPool, Boolean> ChargingPoolAddition;

        /// <summary>
        /// Called whenever an EVS pool will be or was added.
        /// </summary>
        public IVotingSender<DateTime, IChargingStationOperator, IChargingPool, Boolean> OnChargingPoolAddition
            => ChargingPoolAddition;


        private void SendChargingPoolAdded(DateTime                  Timestamp,
                                           IChargingStationOperator  ChargingStationOperator,
                                           IChargingPool             ChargingPool)
        {

            ChargingPoolAddition.SendNotification(Timestamp,
                                                  ChargingStationOperator,
                                                  ChargingPool);

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              AddStaticData(ChargingPool));

        }

        #endregion

        #region ChargingPoolRemoval

        internal readonly IVotingNotificator<DateTime, IChargingStationOperator, IChargingPool, Boolean> ChargingPoolRemoval;

        /// <summary>
        /// Called whenever an EVS pool will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, IChargingStationOperator, IChargingPool, Boolean> OnChargingPoolRemoval
            => ChargingPoolRemoval;


        private void SendChargingPoolRemoved(DateTime                  Timestamp,
                                             IChargingStationOperator  ChargingStation,
                                             IChargingPool             ChargingPool)
        {

            ChargingPoolRemoval.SendNotification(Timestamp,
                                                 ChargingStation,
                                                 ChargingPool);

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              DeleteStaticData(ChargingPool));

        }

        #endregion


        #region ChargingPoolIds                (IncludeChargingPools = null)

        /// <summary>
        /// Return all charging pool identifications registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPool_Id> ChargingPoolIds(IncludeChargingPoolDelegate? IncludeChargingPools = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingPoolIds(IncludeChargingPools));

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


        #region OnChargingPoolData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolDataChangedDelegate?         OnChargingPoolDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate?  OnChargingPoolAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolStatusChangedDelegate?       OnChargingPoolStatusChanged;

        #endregion

        #region OnChargingPoolAdminDiff

        public delegate void OnChargingPoolAdminDiffDelegate(ChargingPoolAdminStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a charging station admin status diff was received.
        /// </summary>
        public event OnChargingPoolAdminDiffDelegate? OnChargingPoolAdminDiff;

        #endregion


        #region (internal) UpdateChargingPoolAdminStatus(Timestamp, EventTrackingId, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
        internal async Task UpdateChargingPoolAdminStatus(DateTime                                   Timestamp,
                                                          EventTracking_Id                           EventTrackingId,
                                                          ChargingPool                               ChargingPool,
                                                          Timestamped<ChargingPoolAdminStatusTypes>  OldStatus,
                                                          Timestamped<ChargingPoolAdminStatusTypes>  NewStatus)
        {

            var OnChargingPoolAdminStatusChangedLocal = OnChargingPoolAdminStatusChanged;
            if (OnChargingPoolAdminStatusChangedLocal is not null)
                await OnChargingPoolAdminStatusChangedLocal(Timestamp,
                                                            EventTrackingId ?? EventTracking_Id.New,
                                                            ChargingPool,
                                                            OldStatus,
                                                            NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingPoolStatus     (Timestamp, EventTrackingId, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
        internal async Task UpdateChargingPoolStatus(DateTime                              Timestamp,
                                                     EventTracking_Id                      EventTrackingId,
                                                     ChargingPool                          ChargingPool,
                                                     Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                                     Timestamped<ChargingPoolStatusTypes>  NewStatus)
        {

            var OnChargingPoolStatusChangedLocal = OnChargingPoolStatusChanged;
            if (OnChargingPoolStatusChangedLocal is not null)
                await OnChargingPoolStatusChangedLocal(Timestamp,
                                                       EventTrackingId ?? EventTracking_Id.New,
                                                       ChargingPool,
                                                       OldStatus,
                                                       NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingPoolData       (Timestamp, EventTrackingId, ChargingPool, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the data of an charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateChargingPoolData(DateTime          Timestamp,
                                                   EventTracking_Id  EventTrackingId,
                                                   ChargingPool      ChargingPool,
                                                   String            PropertyName,
                                                   Object?           OldValue,
                                                   Object?           NewValue)
        {

            //foreach (var AuthenticationService in _IeMobilityServiceProviders.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await AuthenticationService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              UpdateStaticData(ChargingPool,
                                                                               PropertyName,
                                                                               OldValue,
                                                                               NewValue));

            //foreach (var PushEVSEStatusService in _PushEVSEStatusToOperatorRoamingServices.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await PushEVSEStatusService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            var OnChargingPoolDataChangedLocal = OnChargingPoolDataChanged;
            if (OnChargingPoolDataChangedLocal is not null)
                await OnChargingPoolDataChangedLocal(Timestamp,
                                                     EventTrackingId ?? EventTracking_Id.New,
                                                     ChargingPool,
                                                     PropertyName,
                                                     OldValue,
                                                     NewValue);

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


        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, IChargingPool, IChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, IChargingPool, IChargingStation, Boolean> OnChargingStationAddition

            => ChargingStationAddition;

        private void SendChargingStationAdded(DateTime          Timestamp,
                                              IChargingPool     ChargingPool,
                                              IChargingStation  ChargingStation)
        {

            ChargingStationAddition.SendNotification(Timestamp,
                                                     ChargingPool,
                                                     ChargingStation);

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              AddStaticData(ChargingStation));

        }

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, IChargingPool, IChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, IChargingPool, IChargingStation, Boolean> OnChargingStationRemoval

            => ChargingStationRemoval;


        private void SendChargingStationRemoved(DateTime          Timestamp,
                                                IChargingPool     ChargingPool,
                                                IChargingStation  ChargingStation)
        {

            ChargingStationRemoval.SendNotification(Timestamp,
                                                    ChargingPool,
                                                    ChargingStation);

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              DeleteStaticData(ChargingStation));

        }

        #endregion


        #region ChargingStationIds                (IncludeStations = null)

        /// <summary>
        /// Return all charging station identifications registered within this roaming network.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate? IncludeStations = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.ChargingStationIds(IncludeStations));

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


        #region SendChargingStationAdminStatusDiff(StatusDiff)

        internal void SendChargingStationAdminStatusDiff(ChargingStationAdminStatusDiff StatusDiff)
        {
            OnChargingStationAdminDiff?.Invoke(StatusDiff);
        }

        #endregion


        #region OnChargingStationData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationDataChangedDelegate?         OnChargingStationDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated ChargingStation changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate?  OnChargingStationAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationStatusChangedDelegate?       OnChargingStationStatusChanged;

        #endregion

        #region OnChargingStationAdminDiff

        public delegate void OnChargingStationAdminDiffDelegate(ChargingStationAdminStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a charging station admin status diff was received.
        /// </summary>
        public event OnChargingStationAdminDiffDelegate? OnChargingStationAdminDiff;

        #endregion


        #region (internal) UpdateChargingStationAdminStatus(Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal async Task UpdateChargingStationAdminStatus(DateTime                                      Timestamp,
                                                             EventTracking_Id                              EventTrackingId,
                                                             IChargingStation                              ChargingStation,
                                                             Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                                             Timestamped<ChargingStationAdminStatusTypes>  NewStatus)
        {

            var OnChargingStationAdminStatusChangedLocal = OnChargingStationAdminStatusChanged;
            if (OnChargingStationAdminStatusChangedLocal is not null)
                await OnChargingStationAdminStatusChangedLocal(Timestamp,
                                                               EventTrackingId ?? EventTracking_Id.New,
                                                               ChargingStation,
                                                               OldStatus,
                                                               NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingStationStatus     (Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
        internal async Task UpdateChargingStationStatus(DateTime                                 Timestamp,
                                                        EventTracking_Id                         EventTrackingId,
                                                        IChargingStation                         ChargingStation,
                                                        Timestamped<ChargingStationStatusTypes>  OldStatus,
                                                        Timestamped<ChargingStationStatusTypes>  NewStatus)
        {

            var OnChargingStationStatusChangedLocal = OnChargingStationStatusChanged;
            if (OnChargingStationStatusChangedLocal is not null)
                await OnChargingStationStatusChangedLocal(Timestamp,
                                                          EventTrackingId ?? EventTracking_Id.New,
                                                          ChargingStation,
                                                          OldStatus,
                                                          NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingStationData       (Timestamp, EventTrackingId, ChargingStation, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the data of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateChargingStationData(DateTime          Timestamp,
                                                      EventTracking_Id  EventTrackingId,
                                                      IChargingStation  ChargingStation,
                                                      String            PropertyName,
                                                      Object?           OldValue,
                                                      Object?           NewValue)
        {

            //foreach (var AuthenticationService in _IeMobilityServiceProviders.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await AuthenticationService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              UpdateStaticData(ChargingStation,
                                                                               PropertyName,
                                                                               OldValue,
                                                                               NewValue));

            //foreach (var PushEVSEStatusService in _PushEVSEStatusToOperatorRoamingServices.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await PushEVSEStatusService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            var OnChargingStationDataChangedLocal = OnChargingStationDataChanged;
            if (OnChargingStationDataChangedLocal is not null)
                await OnChargingStationDataChangedLocal(Timestamp,
                                                        EventTrackingId ?? EventTracking_Id.New,
                                                        ChargingStation,
                                                        PropertyName,
                                                        OldValue,
                                                        NewValue);

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


        #region EVSEAddition

        internal readonly IVotingNotificator<DateTime, IChargingStation, IEVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, IChargingStation, IEVSE, Boolean> OnEVSEAddition

            => EVSEAddition;

        private void SendEVSEAdded(DateTime          Timestamp,
                                   IChargingStation  ChargingStation,
                                   IEVSE             EVSE)
        {

            EVSEAddition.SendNotification(Timestamp,
                                          ChargingStation,
                                          EVSE);

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              AddStaticData(EVSE));

        }

        #endregion

        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, IChargingStation, IEVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, IChargingStation, IEVSE, Boolean> OnEVSERemoval

            => EVSERemoval;

        private void SendEVSERemoved(DateTime          Timestamp,
                                     IChargingStation  ChargingStation,
                                     IEVSE             EVSE)
        {

            EVSERemoval.SendNotification(Timestamp,
                                         ChargingStation,
                                         EVSE);

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              DeleteStaticData(EVSE));

        }

        #endregion


        #region EVSEIds                (IncludeEVSEs = null)

        /// <summary>
        /// Return all EVSE identifications registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate? IncludeEVSEs = null)

            => chargingStationOperators.
                   SelectMany(cso => cso.EVSEIds(IncludeEVSEs));

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


        #region SetEVSEStatus(EVSEStatusList)

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


        #region SendEVSEStatusDiff(StatusDiff)

        internal void SendEVSEStatusDiff(EVSEStatusDiff StatusDiff)
        {
            OnEVSEStatusDiff?.Invoke(StatusDiff);
        }

        #endregion


        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate?         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate?  OnEVSEAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate?       OnEVSEStatusChanged;

        #endregion

        #region OnEVSEStatusDiff

        public delegate void OnEVSEStatusDiffDelegate(EVSEStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a EVSE status diff was received.
        /// </summary>
        public event OnEVSEStatusDiffDelegate? OnEVSEStatusDiff;

        #endregion


        #region (internal) UpdateEVSEAdminStatus(Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update an EVSE admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEAdminStatus(DateTime                           Timestamp,
                                                  EventTracking_Id                   EventTrackingId,
                                                  IEVSE                              EVSE,
                                                  Timestamped<EVSEAdminStatusTypes>  OldStatus,
                                                  Timestamped<EVSEAdminStatusTypes>  NewStatus)
        {

            var results = _ISendAdminStatus.WhenAll(iSendAdminStatus => iSendAdminStatus.
                                                                            UpdateAdminStatus(new EVSEAdminStatusUpdate[] {
                                                                                                  new EVSEAdminStatusUpdate(EVSE.Id,
                                                                                                                            OldStatus,
                                                                                                                            NewStatus)
                                                                                              },
                                                                                              EventTrackingId: EventTrackingId));

            var OnEVSEAdminStatusChangedLocal = OnEVSEAdminStatusChanged;
            if (OnEVSEAdminStatusChangedLocal is not null)
                await OnEVSEAdminStatusChangedLocal(Timestamp,
                                                    EventTrackingId ?? EventTracking_Id.New,
                                                    EVSE,
                                                    OldStatus,
                                                    NewStatus);

        }

        #endregion

        #region (internal) UpdateEVSEStatus     (Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update an EVSE status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                      Timestamp,
                                             EventTracking_Id              EventTrackingId,
                                             IEVSE                         EVSE,
                                             Timestamped<EVSEStatusTypes>  OldStatus,
                                             Timestamped<EVSEStatusTypes>  NewStatus)
        {

            var results = _ISendStatus.WhenAll(iSendStatus => iSendStatus.
                                                                  UpdateStatus(new EVSEStatusUpdate[] {
                                                                                   new EVSEStatusUpdate(EVSE.Id,
                                                                                                        OldStatus,
                                                                                                        NewStatus)
                                                                               },
                                                                               EventTrackingId: EventTrackingId));

            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal is not null)
                await OnEVSEStatusChangedLocal(Timestamp,
                                               EventTrackingId ?? EventTracking_Id.New,
                                               EVSE,
                                               OldStatus,
                                               NewStatus);

        }

        #endregion

        #region (internal) UpdateEVSEData       (Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

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
                                           Object?           OldValue,
                                           Object?           NewValue)
        {

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              UpdateStaticData(EVSE,
                                                                               PropertyName,
                                                                               OldValue,
                                                                               NewValue,
                                                                               EventTrackingId: EventTrackingId));

            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal is not null)
                await OnEVSEDataChangedLocal(Timestamp,
                                             EventTrackingId ?? EventTracking_Id.New,
                                             EVSE,
                                             PropertyName,
                                             OldValue,
                                             NewValue);

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
        public IVotingSender<DateTime, RoamingNetwork, ParkingOperator, Boolean> OnParkingOperatorAddition
            => parkingOperators.OnAddition;

        #endregion

        #region ParkingOperatorRemoval

        /// <summary>
        /// Called whenever a parking operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, ParkingOperator, Boolean> OnParkingOperatorRemoval
            => parkingOperators.OnAddition;

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


            if (parkingOperators.TryAdd(parkingOperator, OnSuccess))
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

            ParkingOperator _ParkingOperator = null;

            if (parkingOperators.TryRemove(ParkingOperatorId, out _ParkingOperator))
                return _ParkingOperator;

            return null;

        }

        #endregion

        #region TryRemoveParkingOperator(RemoveParkingOperatorId, out RemoveParkingOperator)

        public Boolean TryRemoveParkingOperator(ParkingOperator_Id ParkingOperatorId, out ParkingOperator ParkingOperator)

            => parkingOperators.TryRemove(ParkingOperatorId, out ParkingOperator);

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


        #region (internal) UpdateParkingOperatorAdminStatus(Timestamp, ParkingOperator, OldStatus, NewStatus)

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

        #region (internal) UpdateParkingOperatorStatus     (Timestamp, ParkingOperator, OldStatus, NewStatus)

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
        public IVotingSender<DateTime, RoamingNetwork, GridOperator, Boolean> OnGridOperatorAddition
            => gridOperators.OnAddition;

        #endregion

        #region OnGridOperatorRemoval

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, GridOperator, Boolean> OnGridOperatorRemoval
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
                                            I18NString                            Name                    = null,
                                            I18NString                            Description             = null,
                                            GridOperatorPriority                     Priority                = null,
                                            GridOperatorAdminStatusType              AdminStatus             = GridOperatorAdminStatusType.Available,
                                            GridOperatorStatusType                   Status                  = GridOperatorStatusType.Available,
                                            Action<GridOperator>                     Configurator            = null,
                                            Action<GridOperator>                     OnSuccess               = null,
                                            Action<RoamingNetwork, GridOperator_Id>  OnError                 = null,
                                            RemoteGridOperatorCreatorDelegate        RemoteGridOperatorCreator  = null)
        {

            #region Initial checks

            if (GridOperatorId == null)
                throw new ArgumentNullException(nameof(GridOperatorId),  "The given smart city identification must not be null!");

            #endregion

            var _GridOperator = new GridOperator(GridOperatorId,
                                           this,
                                           Configurator,
                                           RemoteGridOperatorCreator,
                                           Name,
                                           Description,
                                           Priority,
                                           AdminStatus,
                                           Status);


            if (gridOperators.TryAdd(_GridOperator, OnSuccess))
            {

                // Link events!

                return _GridOperator;

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

            if (gridOperators.TryRemove(GridOperatorId, out _GridOperator))
                return _GridOperator;

            return null;

        }

        #endregion

        #region TryRemoveGridOperator(RemoveGridOperatorId, out RemoveGridOperator)

        public Boolean TryRemoveGridOperator(GridOperator_Id GridOperatorId, out GridOperator GridOperator)

            => gridOperators.TryRemove(GridOperatorId, out GridOperator);

        #endregion

        #endregion

        #region Smart Cities...

        #region SmartCities

        private readonly EntityHashSet<RoamingNetwork, SmartCity_Id, SmartCityProxy> _SmartCities;

        /// <summary>
        /// Return all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<SmartCityProxy> SmartCities

            => _SmartCities;

        #endregion

        #region SmartCitiesAdminStatus

        /// <summary>
        /// Return the admin status of all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityAdminStatusTypes>>>> SmartCitiesAdminStatus

            => _SmartCities.
                   Select(emp => new KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityAdminStatusTypes>>>(emp.Id, emp.AdminStatusSchedule()));

        #endregion

        #region SmartCitiesStatus

        /// <summary>
        /// Return the status of all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityStatusTypes>>>> SmartCitiesStatus

            => _SmartCities.
                   Select(emp => new KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityStatusTypes>>>(emp.Id, emp.StatusSchedule()));

        #endregion


        #region OnSmartCityAddition

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was added.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, SmartCityProxy, Boolean> OnSmartCityAddition
            => _SmartCities.OnAddition;

        #endregion

        #region OnSmartCityRemoval

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, SmartCityProxy, Boolean> OnSmartCityRemoval
            => _SmartCities.OnRemoval;

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

            var _SmartCity = new SmartCityProxy(Id,
                                                this,
                                                Name,
                                                Description,
                                                Configurator,
                                                RemoteSmartCityCreator,
                                                Priority,
                                                InitialAdminStatus,
                                                InitialStatus);


            if (_SmartCities.TryAdd(_SmartCity, OnSuccess))
            {

                // Link events!

                return _SmartCity;

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

            => _SmartCities.ContainsId(SmartCity.Id);

        #endregion

        #region ContainsSmartCity(SmartCityId)

        /// <summary>
        /// Check if the given SmartCity identification is already present within the roaming network.
        /// </summary>
        /// <param name="SmartCityId">The unique identification of the Charging Station Operator.</param>
        public Boolean ContainsSmartCity(SmartCity_Id SmartCityId)

            => _SmartCities.ContainsId(SmartCityId);

        #endregion

        #region GetSmartCityById(SmartCityId)

        public SmartCityProxy GetSmartCityById(SmartCity_Id SmartCityId)

            => _SmartCities.GetById(SmartCityId);

        #endregion

        #region TryGetSmartCityById(SmartCityId, out SmartCity)

        public Boolean TryGetSmartCityById(SmartCity_Id SmartCityId, out SmartCityProxy SmartCity)

            => _SmartCities.TryGet(SmartCityId, out SmartCity);

        #endregion

        #region RemoveSmartCity(SmartCityId)

        public SmartCityProxy RemoveSmartCity(SmartCity_Id SmartCityId)
        {

            SmartCityProxy _SmartCity = null;

            if (_SmartCities.TryRemove(SmartCityId, out _SmartCity))
                return _SmartCity;

            return null;

        }

        #endregion

        #region TryRemoveSmartCity(RemoveSmartCityId, out RemoveSmartCity)

        public Boolean TryRemoveSmartCity(SmartCity_Id SmartCityId, out SmartCityProxy SmartCity)

            => _SmartCities.TryRemove(SmartCityId, out SmartCity);

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

        private readonly EntityHashSet<RoamingNetwork, NavigationProvider_Id, NavigationProvider> _NavigationProviders;

        /// <summary>
        /// Return all navigation providers registered within this roaming network.
        /// </summary>
        public IEnumerable<NavigationProvider> NavigationProviders

            => _NavigationProviders;

        #endregion

        #region NavigationProviderAdminStatus

        /// <summary>
        /// Return the admin status of all navigation providers registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderAdminStatusType>>>> NavigationProviderAdminStatus

            => _NavigationProviders.
                   Select(emp => new KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderAdminStatusType>>>(emp.Id, emp.AdminStatusSchedule()));

        #endregion

        #region NavigationProviderStatus

        /// <summary>
        /// Return the status of all navigation providers registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderStatusType>>>> NavigationProviderStatus

            => _NavigationProviders.
                   Select(emp => new KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderStatusType>>>(emp.Id, emp.StatusSchedule()));

        #endregion


        #region OnNavigationProviderAddition

        /// <summary>
        /// Called whenever an navigation provider will be or was added.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, NavigationProvider, Boolean> OnNavigationProviderAddition
            => _NavigationProviders.OnAddition;

        #endregion

        #region OnNavigationProviderRemoval

        /// <summary>
        /// Called whenever an navigation provider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, NavigationProvider, Boolean> OnNavigationProviderRemoval
            => _NavigationProviders.OnRemoval;

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
                                                            I18NString                                    Name                            = null,
                                                            I18NString                                    Description                     = null,
                                                            NavigationProviderPriority                     Priority                        = null,
                                                            NavigationProviderAdminStatusType              AdminStatus                     = NavigationProviderAdminStatusType.Available,
                                                            NavigationProviderStatusType                   Status                          = NavigationProviderStatusType.Available,
                                                            Action<NavigationProvider>                     Configurator                    = null,
                                                            Action<NavigationProvider>                     OnSuccess                       = null,
                                                            Action<RoamingNetwork, NavigationProvider_Id>  OnError                         = null,
                                                            RemoteNavigationProviderCreatorDelegate        RemoteNavigationProviderCreator  = null)
        {

            #region Initial checks

            if (NavigationProviderId == null)
                throw new ArgumentNullException(nameof(NavigationProviderId),  "The given navigation provider identification must not be null!");

            #endregion

            var _NavigationProvider = new NavigationProvider(NavigationProviderId,
                                                           this,
                                                           Configurator,
                                                           RemoteNavigationProviderCreator,
                                                           Name,
                                                           Description,
                                                           Priority,
                                                           AdminStatus,
                                                           Status);


            if (_NavigationProviders.TryAdd(_NavigationProvider, OnSuccess))
            {

                // Link events!

                return _NavigationProvider;

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

            => _NavigationProviders.ContainsId(NavigationProvider.Id);

        #endregion

        #region ContainsNavigationProvider(NavigationProviderId)

        /// <summary>
        /// Check if the given NavigationProvider identification is already present within the roaming network.
        /// </summary>
        /// <param name="NavigationProviderId">The unique identification of the Charging Station Operator.</param>
        public Boolean ContainsNavigationProvider(NavigationProvider_Id NavigationProviderId)

            => _NavigationProviders.ContainsId(NavigationProviderId);

        #endregion

        #region GetNavigationProviderById(NavigationProviderId)

        public NavigationProvider GetNavigationProviderById(NavigationProvider_Id NavigationProviderId)

            => _NavigationProviders.GetById(NavigationProviderId);

        #endregion

        #region TryGetNavigationProviderById(NavigationProviderId, out NavigationProvider)

        public Boolean TryGetNavigationProviderById(NavigationProvider_Id NavigationProviderId, out NavigationProvider NavigationProvider)

            => _NavigationProviders.TryGet(NavigationProviderId, out NavigationProvider);

        #endregion

        #region RemoveNavigationProvider(NavigationProviderId)

        public NavigationProvider RemoveNavigationProvider(NavigationProvider_Id NavigationProviderId)
        {

            NavigationProvider _NavigationProvider = null;

            if (_NavigationProviders.TryRemove(NavigationProviderId, out _NavigationProvider))
                return _NavigationProvider;

            return null;

        }

        #endregion

        #region TryRemoveNavigationProvider(RemoveNavigationProviderId, out RemoveNavigationProvider)

        public Boolean TryRemoveNavigationProvider(NavigationProvider_Id NavigationProviderId, out NavigationProvider NavigationProvider)

            => _NavigationProviders.TryRemove(NavigationProviderId, out NavigationProvider);

        #endregion

        #endregion


        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(15);

        #region Reservations...

        #region Data

        public IEnumerable<ChargingReservation> ChargingReservations
            => ReservationsStore.
                   Select(reservation => reservation.Last());

        public Boolean TryGetChargingReservationById(ChargingReservation_Id Id, out ChargingReservation ChargingReservation)
        {

            if (ReservationsStore.TryGet(Id, out ChargingReservationCollection ReservationCollection))
            {
                ChargingReservation = ReservationCollection.Last();
                return true;
            }

            ChargingReservation = null;
            return false;

        }

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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(ChargingLocation                   ChargingLocation,
                    ChargingReservationLevel           ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                          ReservationStartTime   = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    CancellationToken?                 CancellationToken      = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

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
                                               ChargingProduct,
                                               AuthTokens,
                                               eMAIds,
                                               PINs,

                                               Timestamp,
                                               CancellationToken,
                                               EventTrackingId,
                                               RequestTimeout);

                    if (result.Result == ReservationResultType.Success)
                    {
                        if (result.Reservation is not null)
                        {
                            result.Reservation.ChargingStationOperatorId = chargingStationOperator.Id;
                            ReservationsStore.NewOrUpdate(result.Reservation);
                        }
                    }

                }

                if (result        == null ||
                   (result        != null &&
                   (result.Result == ReservationResultType.UnknownLocation)))
                {

                    foreach (var CSORoamingService in csoRoamingProviders.
                                                          OrderBy(CSORoamingServiceWithPriority => CSORoamingServiceWithPriority.Key).
                                                          Select (CSORoamingServiceWithPriority => CSORoamingServiceWithPriority.Value))
                    {

                        result = await CSORoamingService.
                                           Reserve(ChargingLocation,
                                                   ReservationLevel,
                                                   ReservationStartTime,
                                                   Duration,
                                                   ReservationId,
                                                   LinkedReservationId,
                                                   ProviderId,
                                                   RemoteAuthentication,
                                                   ChargingProduct,
                                                   AuthTokens,
                                                   eMAIds,
                                                   PINs,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);


                        if (result.Result == ReservationResultType.Success)
                        {
                            if (result.Reservation != null)
                            {
                                result.Reservation.CSORoamingProviderId = CSORoamingService.Id;
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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ChargingReservation     canceledReservation  = null;
            CancelReservationResult result               = null;

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

                if (ReservationsStore.TryGetLatest(ReservationId, out ChargingReservation Reservation))
                {

                    #region Check Charging Station Operator charging reservation lookup...

                    if (Reservation.ChargingStationOperatorId.HasValue &&
                        TryGetChargingStationOperatorById(Reservation.ChargingStationOperatorId.Value, out var _ChargingStationOperator))
                    {

                        result = await _ChargingStationOperator.
                                           CancelReservation(ReservationId,
                                                             Reason,

                                                             Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             RequestTimeout);

                    }

                    #endregion

                    #region ...or check CSO roaming provider charging reservation lookup...

                    if (result == null ||
                       (result != null &&
                       (result.Result == CancelReservationResultTypes.UnknownEVSE ||
                        result.Result == CancelReservationResultTypes.UnknownReservationId)))
                    {


                        if (Reservation.CSORoamingProviderId.HasValue &&
                            TryGetCSORoamingProviderById(Reservation.CSORoamingProviderId.Value, out ICSORoamingProvider _ICSORoamingProvider))
                        {

                            result = await _ICSORoamingProvider.
                                               CancelReservation(ReservationId,
                                                                 Reason,

                                                                 Timestamp,
                                                                 CancellationToken,
                                                                 EventTrackingId,
                                                                 RequestTimeout);

                        }

                    }

                    #endregion

                    #region ...or try to check every CSO roaming provider...

                    if (result == null ||
                       (result != null &&
                       (result.Result == CancelReservationResultTypes.UnknownEVSE ||
                        result.Result == CancelReservationResultTypes.UnknownReservationId)))
                    {

                        foreach (var CSORoamingService in csoRoamingProviders.
                                                              OrderBy(CSORoamingServiceWithPriority => CSORoamingServiceWithPriority.Key).
                                                              Select (CSORoamingServiceWithPriority => CSORoamingServiceWithPriority.Value))
                        {

                            result = await CSORoamingService.
                                               CancelReservation(ReservationId,
                                                                 Reason,

                                                                 Timestamp,
                                                                 CancellationToken,
                                                                 EventTrackingId,
                                                                 RequestTimeout);

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

        #region RemoteStart/-Stop

        #region Data

        public IEnumerable<ChargingSession> ChargingSessions
            => SessionsStore;

        TimeSpan IReserveRemoteStartStop.MaxReservationDuration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
            => SessionsStore.TryGet(SessionId, out ChargingSession);

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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingLocation          ChargingLocation,
                        ChargingProduct?          ChargingProduct            = null,
                        ChargingReservation_Id?   ReservationId              = null,
                        ChargingSession_Id?       SessionId                  = null,
                        EMobilityProvider_Id?     ProviderId                 = null,
                        RemoteAuthentication?     RemoteAuthentication       = null,

                        DateTime?                 Timestamp                  = null,
                        CancellationToken?        CancellationToken          = null,
                        EventTracking_Id?         EventTrackingId            = null,
                        TimeSpan?                 RequestTimeout             = null)

                => RemoteStart(null,
                               ChargingLocation,
                               ChargingProduct,
                               ReservationId,
                               SessionId,
                               ProviderId,
                               RemoteAuthentication,

                               Timestamp,
                               CancellationToken,
                               EventTrackingId,
                               RequestTimeout);


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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(IEMPRoamingProvider      EMPRoamingProvider,
                        ChargingLocation         ChargingLocation,
                        ChargingProduct?         ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication?    RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStartResult result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartRequest?.Invoke(StartTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             Id,
                                             ChargingLocation,
                                             ChargingProduct,
                                             ReservationId,
                                             SessionId,
                                             null,
                                             EMPRoamingProvider?.Id,
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
                {
                    result = RemoteStartResult.UnknownLocation();
                }

                else if (SessionsStore.SessionExists(SessionId))
                {
                    result = RemoteStartResult.InvalidSessionId();
                }

                else if (AdminStatus.Value == RoamingNetworkAdminStatusTypes.Operational ||
                         AdminStatus.Value == RoamingNetworkAdminStatusTypes.InternalUse)
                {

                    #region Try to lookup the charging station operator given in the EVSE identification...

                    if (TryGetChargingStationOperatorById(ChargingLocation.ChargingStationOperatorId,     out var chargingStationOperator) ||
                        TryGetChargingStationOperatorById(ChargingLocation.EVSEId?.           OperatorId, out     chargingStationOperator) ||
                        TryGetChargingStationOperatorById(ChargingLocation.ChargingStationId?.OperatorId, out     chargingStationOperator) ||
                        TryGetChargingStationOperatorById(ChargingLocation.ChargingPoolId?.   OperatorId, out     chargingStationOperator))
                    {

                        result = await chargingStationOperator.
                                           RemoteStart(ChargingLocation,
                                                       ChargingProduct,
                                                       ReservationId,
                                                       SessionId,
                                                       ProviderId,
                                                       RemoteAuthentication,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout);


                        if (result.Result == RemoteStartResultTypes.Success ||
                            result.Result == RemoteStartResultTypes.AsyncOperation)
                        {

                            SessionsStore.RemoteStart(result.Session,
                                                      result,
                                                      session => {
                                                          session.EMPRoamingProviderStart = EMPRoamingProvider;
                                                      });

                        }

                    }

                    #endregion

                    //ToDo: Add routing!
                    #region ...or try every CSO roaming provider...

                    if (result        == null ||
                       (result        != null &&
                       (result.Result == RemoteStartResultTypes.UnknownLocation)))
                    {

                        foreach (var empRoamingProvider in csoRoamingProviders.
                                                                OrderBy(CSORoamingServiceWithPriority => CSORoamingServiceWithPriority.Key).
                                                                Select (CSORoamingServiceWithPriority => CSORoamingServiceWithPriority.Value))
                        {

                            result = await empRoamingProvider.
                                               RemoteStart(ChargingLocation,
                                                           ChargingProduct,
                                                           ReservationId,
                                                           SessionId,
                                                           ProviderId,
                                                           RemoteAuthentication,

                                                           Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           RequestTimeout);


                            if (result.Result == RemoteStartResultTypes.Success ||
                                result.Result == RemoteStartResultTypes.AsyncOperation)
                            {

                                SessionsStore.RemoteStart(result.Session,
                                                          result,
                                                          session => {
                                                              session.EMPRoamingProviderStart = EMPRoamingProvider;
                                                              session.CSORoamingProviderStart = empRoamingProvider;
                                                          });

                            }

                        }

                    }

                    #endregion

                    #region ...or fail!

                    if (result == null)
                        result = RemoteStartResult.UnknownOperator();

                    #endregion

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStartResult.OutOfService();
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }


            #region Send OnRemoteStartResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartResponse?.Invoke(EndTime,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              Id,
                                              ChargingLocation,
                                              ChargingProduct,
                                              ReservationId,
                                              SessionId,
                                              null,
                                              EMPRoamingProvider?.Id,
                                              ProviderId,
                                              RemoteAuthentication,
                                              RequestTimeout,
                                              result,
                                              EndTime - StartTime);

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

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

                => RemoteStop(null,
                              SessionId,
                              ReservationHandling,
                              ProviderId,
                              RemoteAuthentication,

                              Timestamp,
                              CancellationToken,
                              EventTrackingId,
                              RequestTimeout);


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
        public async Task<RemoteStopResult>

            RemoteStop(IEMPRoamingProvider    EMPRoamingProvider,
                       ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       EMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStopResult result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopRequest?.Invoke(StartTime,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            Id,
                                            SessionId,
                                            ReservationHandling,
                                            null,
                                            EMPRoamingProvider?.Id,
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

                if (SessionsStore.TryGet(SessionId, out ChargingSession chargingSession))
                {

                    //ToDo: Add a --useForce Option to overwrite!
                    if (chargingSession.SessionTime.EndTime.HasValue)
                        result = RemoteStopResult.AlreadyStopped(SessionId,
                                                                 Runtime: org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - StartTime);

                    else
                    {

                        #region When a charging station operator was found...

                        if (chargingSession.ChargingStationOperator == null & chargingSession.ChargingStationOperatorId.HasValue)
                            chargingSession.ChargingStationOperator = GetChargingStationOperatorById(chargingSession.ChargingStationOperatorId.Value);

                        if (chargingSession.ChargingStationOperator != null)
                            result = await chargingSession.ChargingStationOperator.
                                                RemoteStop(chargingSession.Id,
                                                           ReservationHandling,
                                                           ProviderId,
                                                           RemoteAuthentication,

                                                           Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           RequestTimeout);

                        #endregion

                        #region ...or when a charging station roaming provider was found

                        if (result == null)
                        {

                            if (chargingSession.CSORoamingProviderStart == null & chargingSession.CSORoamingProviderIdStart.HasValue)
                                chargingSession.CSORoamingProviderStart = GetCSORoamingProviderById(chargingSession.CSORoamingProviderIdStart.Value);

                            if (chargingSession.CSORoamingProviderStart != null)
                                result = await chargingSession.CSORoamingProviderStart.
                                                    RemoteStop(chargingSession.Id,
                                                               ReservationHandling,
                                                               ProviderId,
                                                               RemoteAuthentication,

                                                               Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               RequestTimeout);

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

                if (result        == null ||
                   (result        != null &&
                   (result.Result == RemoteStopResultTypes.InvalidSessionId)))
                {

                    foreach (var chargingStationOperator in chargingStationOperators)
                    {

                        result = await chargingStationOperator.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

                        if (result != null && result.Result != RemoteStopResultTypes.InvalidSessionId)
                            break;

                    }

                }

                #endregion

                #region ...or try to check every CSO roaming provider...

                if (result        == null ||
                   (result        != null &&
                   (result.Result == RemoteStopResultTypes.InvalidSessionId)))
                {

                    foreach (var empRoamingProvider in csoRoamingProviders.
                                                           OrderBy(CSORoamingServiceWithPriority => CSORoamingServiceWithPriority.Key).
                                                           Select (CSORoamingServiceWithPriority => CSORoamingServiceWithPriority.Value))
                    {

                        result = await empRoamingProvider.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

                        if (result != null && result.Result != RemoteStopResultTypes.InvalidSessionId)
                            break;

                    }

                }

                #endregion

                #region ...or fail!

                if (result == null)
                    result = RemoteStopResult.InvalidSessionId(SessionId);

                #endregion

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(SessionId,
                                                e.Message,
                                                Runtime: org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - StartTime);
            }



            SessionsStore.RemoteStop(SessionId,
                                     RemoteAuthentication,
                                     ProviderId,
                                     EMPRoamingProvider,
                                     result);


            #region Send OnRemoteStopResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopResponse?.Invoke(EndTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             Id,
                                             SessionId,
                                             ReservationHandling,
                                             null,
                                             EMPRoamingProvider?.Id,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout,
                                             result,
                                             EndTime - StartTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion


            if (result.ChargeDetailRecord != null)
                await SendChargeDetailRecord(result.ChargeDetailRecord);


            return result;

        }

        #endregion


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

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            if (ChargeDetailRecord != null)
                SendChargeDetailRecords(new ChargeDetailRecord[] { ChargeDetailRecord },
                                        TransmissionTypes.Enqueue,

                                        Timestamp,
                                        new CancellationTokenSource().Token,
                                        EventTracking_Id.New,
                                        TimeSpan.FromMinutes(1)).

                                        Wait(TimeSpan.FromMinutes(1));

        }

        #endregion

        #endregion

        #region AuthorizeStart/-Stop

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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingLocation?            ChargingLocation      = null,
                           ChargingProduct?             ChargingProduct       = null,
                           ChargingSession_Id?          SessionId             = null,
                           ChargingSession_Id?          CPOPartnerSessionId   = null,
                           ChargingStationOperator_Id?  OperatorId            = null,

                           DateTime?                    Timestamp             = null,
                           CancellationToken?           CancellationToken     = null,
                           EventTracking_Id?            EventTrackingId       = null,
                           TimeSpan?                    RequestTimeout        = null)

        {

            #region Initial checks

            if (LocalAuthentication is null)
                throw new ArgumentNullException(nameof(LocalAuthentication),   "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;

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
                                                _ISend2RemoteAuthorizeStartStop,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            //ToDo: Fail when RFID UID == "00000000000000"

            //DebugX.LogT(String.Concat(
            //    "RoamingNetwork '",
            //    this.Id,
            //    "' AuthorizeStart request: ",
            //    LocalAuthentication,
            //    " @ ",
            //    ChargingLocation?.ToString() ?? "-",
            //    " -> ",
            //    _ISend2RemoteAuthorizeStartStop.Select(_ => _.AuthId).AggregateWith(", ")));

            var result = await _ISend2RemoteAuthorizeStartStop.
                                   WhenFirst(Work:          iRemoteAuthorizeStartStop => iRemoteAuthorizeStartStop.
                                                                                             AuthorizeStart(LocalAuthentication,
                                                                                                            ChargingLocation,
                                                                                                            ChargingProduct,
                                                                                                            SessionId,
                                                                                                            CPOPartnerSessionId,
                                                                                                            OperatorId,

                                                                                                            Timestamp,
                                                                                                            CancellationToken,
                                                                                                            EventTrackingId,
                                                                                                            RequestTimeout),

                                             VerifyResult:  result2                   => result2.Result == AuthStartResultTypes.Authorized ||
                                                                                         result2.Result == AuthStartResultTypes.Blocked,

                                             Timeout:       RequestTimeout ?? this.RequestTimeout,

                                             OnException:   null,

                                             DefaultResult: runtime                   => AuthStartResult.NotAuthorized(Id,
                                                                                                                       this,
                                                                                                                       SessionId,
                                                                                                                       Description:  I18NString.Create(Languages.en, "No authorization service returned a positiv result!"),
                                                                                                                       Runtime:      runtime));


            //DebugX.LogT(String.Concat(
            //    "RN AuthStart Response: '",
            //    result?.ISendAuthorizeStartStop?.   AuthId?.ToString() ?? "-",
            //    "' / '",
            //    result?.IReceiveAuthorizeStartStop?.AuthId?.ToString() ?? "-",
            //    "': ", LocalAuthentication,
            //    " @ ",
            //    ChargingLocation?.ToString() ?? "-",
            //    " => ", result));

            #region If Authorized...

            if (result.Result == AuthStartResultTypes.Authorized)
            {

                if (!result.SessionId.HasValue)
                    result = AuthStartResult.Authorized(result.AuthorizatorId,
                                                        result.ISendAuthorizeStartStop,
                                                        ChargingSession_Id.NewRandom,
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
                                                        result.Runtime);


                // Store the upstream session id in order to contact the right authenticator at later requests!
                // Will be deleted when the charge detail record was sent!

                var EVSE            = ChargingLocation?.EVSEId.           HasValue == true ? GetEVSEById           (ChargingLocation.EVSEId.           Value) : null;
                var ChargingStation = ChargingLocation?.ChargingStationId.HasValue == true ? GetChargingStationById(ChargingLocation.ChargingStationId.Value) : EVSE?.ChargingStation;
                var ChargingPool    = ChargingLocation?.ChargingPoolId.   HasValue == true ? GetChargingPoolById   (ChargingLocation.ChargingPoolId.   Value) : ChargingStation?.ChargingPool;

                var NewChargingSession = new ChargingSession(result.SessionId.Value) {
                                             RoamingNetworkId           = Id,
                                             EMPRoamingProviderStart    = result.ISendAuthorizeStartStop as IEMPRoamingProvider,
                                             ProviderIdStart            = result.ProviderId,
                                             ChargingStationOperatorId  = OperatorId,
                                             EVSEId                     = ChargingLocation?.EVSEId,
                                             ChargingStationId          = ChargingLocation?.ChargingStationId,
                                             ChargingPoolId             = ChargingLocation?.ChargingPoolId,
                                             EVSE                       = EVSE,
                                             ChargingStation            = ChargingStation,
                                             ChargingPool               = ChargingPool,
                                             ChargingStationOperator    = OperatorId.HasValue ? GetChargingStationOperatorById(OperatorId.Value) : null,
                                             AuthenticationStart        = LocalAuthentication,
                                             ChargingProduct            = ChargingProduct
                                         };

                SessionsStore.AuthStart(NewChargingSession);

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
                                                 _ISend2RemoteAuthorizeStartStop,
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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingLocation?            ChargingLocation      = null,
                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                          ChargingStationOperator_Id?  OperatorId            = null,

                          DateTime?                    Timestamp             = null,
                          CancellationToken?           CancellationToken     = null,
                          EventTracking_Id?            EventTrackingId       = null,
                          TimeSpan?                    RequestTimeout        = null)

        {

            #region Initial checks

            if (LocalAuthentication is null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given parameter must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;

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


            AuthStopResult result = null;

            try
            {

                #region A matching charging session was found...

                if (SessionsStore.TryGet(SessionId, out ChargingSession chargingSession))
                {

                    //ToDo: Add a --useForce Option to overwrite!
                    if (chargingSession.SessionTime.EndTime.HasValue)
                        result = AuthStopResult.AlreadyStopped(SessionId,
                                                               this,
                                                               SessionId,
                                                               Runtime: org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - startTime);

                    else
                    {

                        #region When an e-mobility provider was found

                        if (chargingSession.ProviderStart == null && chargingSession.ProviderIdStart.HasValue)
                            chargingSession.ProviderStart = GetEMobilityProviderById(chargingSession.ProviderIdStart);

                        if (chargingSession.ProviderStart != null)
                        {

                            result  = await chargingSession.ProviderStart.AuthorizeStop(SessionId,
                                                                                        LocalAuthentication,
                                                                                        ChargingLocation,
                                                                                        CPOPartnerSessionId,
                                                                                        OperatorId,

                                                                                        Timestamp,
                                                                                        CancellationToken,
                                                                                        EventTrackingId,
                                                                                        RequestTimeout);

                            if (result?.Result == AuthStopResultTypes.Authorized)
                                SessionsStore.AuthStop(SessionId,
                                                       LocalAuthentication,
                                                       result.ProviderId.Value);

                        }

                        #endregion

                        #region ...or when an CSO roaming provider was found...

                        if (result == null)
                        {

                            if (chargingSession.EMPRoamingProviderStart == null && chargingSession.EMPRoamingProviderIdStart.HasValue)
                                chargingSession.EMPRoamingProviderStart = GetEMPRoamingProviderById(chargingSession.EMPRoamingProviderIdStart.Value);

                            if (chargingSession.EMPRoamingProviderStart != null)
                            {

                                result  = await chargingSession.EMPRoamingProviderStart.AuthorizeStop(SessionId,
                                                                                                      LocalAuthentication,
                                                                                                      ChargingLocation,
                                                                                                      CPOPartnerSessionId,
                                                                                                      OperatorId,

                                                                                                      Timestamp,
                                                                                                      CancellationToken,
                                                                                                      EventTrackingId,
                                                                                                      RequestTimeout);

                                if (result?.Result == AuthStopResultTypes.Authorized)
                                    SessionsStore.AuthStop(SessionId,
                                                           LocalAuthentication,
                                                           result.ProviderId.Value,
                                                           result.ISendAuthorizeStartStop as IEMPRoamingProvider);

                            }

                        }

                        #endregion

                    }

                }

                #endregion

                #region Send the request to all authorization services

                if (result == null)
                {

                    result = await _ISend2RemoteAuthorizeStartStop.
                                       WhenFirst(Work:           iRemoteAuthorizeStartStop => iRemoteAuthorizeStartStop.
                                                                                                  AuthorizeStop(SessionId,
                                                                                                                LocalAuthentication,
                                                                                                                ChargingLocation,
                                                                                                                CPOPartnerSessionId,
                                                                                                                OperatorId,

                                                                                                                Timestamp,
                                                                                                                CancellationToken,
                                                                                                                EventTrackingId,
                                                                                                                RequestTimeout),

                                                 VerifyResult:   result2 => result2.Result == AuthStopResultTypes.Authorized ||
                                                                            result2.Result == AuthStopResultTypes.Blocked,

                                                 Timeout:        RequestTimeout ?? this.RequestTimeout,

                                                 OnException:    null,

                                                 DefaultResult:  runtime => AuthStopResult.NotAuthorized(Id,
                                                                                                         this,
                                                                                                         SessionId,
                                                                                                         Description: I18NString.Create(Languages.en, "No authorization service returned a positiv result!"),
                                                                                                         Runtime:     runtime)).

                                       ConfigureAwait(false);

                }

                #endregion

            }
            catch (Exception e)
            {

                result = AuthStopResult.Error(SessionId,
                                              this,
                                              SessionId,
                                              I18NString.Create(Languages.en, e.Message),
                                              org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - startTime);

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

        #endregion

        #region Charging Sessions

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
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<SendCDRsResult>

            IReceiveChargeDetailRecords.SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,

                                                                DateTime?                        Timestamp,
                                                                CancellationToken?               CancellationToken,
                                                                EventTracking_Id                 EventTrackingId,
                                                                TimeSpan?                        RequestTimeout)

                => SendChargeDetailRecords(ChargeDetailRecords,
                                           TransmissionTypes.Enqueue,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout);

        #endregion

        #region SendChargeDetailRecord (ChargeDetailRecord,  ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="TransmissionType">Whether to send the charge detail record directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<SendCDRsResult>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,
                                   TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                                   DateTime?           Timestamp           = null,
                                   CancellationToken?  CancellationToken   = null,
                                   EventTracking_Id?   EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null)


                => SendChargeDetailRecords(new ChargeDetailRecord[] { ChargeDetailRecord },
                                           TransmissionType,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout);

        #endregion

        #region SendChargeDetailRecords(ChargeDetailRecords, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the charge detail record directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTime?                        Timestamp           = null,
                                    CancellationToken?               CancellationToken   = null,
                                    EventTracking_Id?                EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null)

        {

            #region Initial checks

            ChargeDetailRecords ??= Array.Empty<ChargeDetailRecord>();


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnSendCDRsRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnSendCDRsRequest?.Invoke(StartTime,
                                          Timestamp.Value,
                                          this,
                                          Id.ToString(),
                                          EventTrackingId,
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

            DateTime         Endtime;
            TimeSpan         Runtime;
            SendCDRsResult?  result = null;

            if (DisableSendChargeDetailRecords)
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                result   = SendCDRsResult.AdminDown(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                    Id,
                                                    this as ISendChargeDetailRecords,
                                                    ChargeDetailRecords,
                                                    Runtime: Runtime);

            }

            #endregion

            #region ..., or when there are no charge detail records...

            else if (!ChargeDetailRecords.Any())
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                result   = SendCDRsResult.NoOperation(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                      Id,
                                                      this as ISendChargeDetailRecords,
                                                      ChargeDetailRecords,
                                                      Runtime: Runtime);

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

                foreach (var isendcdr in _IRemoteSendChargeDetailRecord)
                    cdrTargets.Add(isendcdr, new List<ChargeDetailRecord>());

                #endregion

                #region Try to find the target EMP or roaming network for each CDR...

                foreach (var cdr in chargeDetailRecordsToProcess.ToArray())
                {

                    #region The CDR EMPRoamingProvider(Id) is known, or...

                    if (cdr.EMPRoamingProvider is not null &&
                       !cdr.EMPRoamingProviderId.HasValue)
                    {
                        cdr.EMPRoamingProviderId = cdr.EMPRoamingProvider.Id;
                    }

                    if (cdr.EMPRoamingProviderId.HasValue)
                    {

                        var empRoamingProviderId      = cdr.EMPRoamingProviderId.ToString();
                        var empRoamingProviderForCDR  = _IRemoteSendChargeDetailRecord.FirstOrDefault(iSendChargeDetailRecords => iSendChargeDetailRecords.Id.ToString() == empRoamingProviderId) as IEMPRoamingProvider;

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

                    #region ...we lookup the EMP roaming provider via the authorization within the charging sessions

                    else if (SessionsStore.TryGet(cdr.SessionId, out var chargingSession))
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

                        if (chargingSession.EMPRoamingProviderStart is null &&
                            chargingSession.EMPRoamingProviderIdStart is not null)
                            chargingSession.EMPRoamingProviderStart = _IRemoteSendChargeDetailRecord.FirstOrDefault(iSendChargeDetailRecords => iSendChargeDetailRecords.Id.ToString() == chargingSession.EMPRoamingProviderIdStart.ToString()) as IEMPRoamingProvider;

                        if (chargingSession.EMPRoamingProviderStart is not null &&
                            chargingSession.EMPRoamingProviderStart is IEMPRoamingProvider empRoamingProviderForCDR)
                        {

                            cdrTargets[empRoamingProviderForCDR].Add(cdr);
                            chargeDetailRecordsToProcess.Remove(cdr);

                            SessionsStore.CDRReceived(cdr.SessionId,
                                                      cdr);

                        }

                    }

                    #endregion

                }

                #endregion

                #region Any CDRs left? => Ask all e-mobility providers!

                foreach (var _cdr in chargeDetailRecordsToProcess.ToList())
                {

                    #region We have a valid (start) provider identification

                    if (_cdr.ProviderIdStart.HasValue && eMobilityProviders.TryGet(_cdr.ProviderIdStart.Value, out EMobilityProvider eMobPro))
                    {
                        cdrTargets[eMobPro].Add(_cdr);
                        chargeDetailRecordsToProcess.Remove(_cdr);
                    }

                    #endregion

                    #region We have a valid (stop)  provider identification

                    else if (_cdr.ProviderIdStop.HasValue && eMobilityProviders.TryGet(_cdr.ProviderIdStop.Value, out eMobPro))
                    {
                        cdrTargets[eMobPro].Add(_cdr);
                        chargeDetailRecordsToProcess.Remove(_cdr);
                    }

                    #endregion


                    #region We have a valid (start) AuthToken/RFID identification

                    else if (_cdr.AuthenticationStart?.AuthToken != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            LocalAuthentication LL = null;

                            if (_cdr.AuthenticationStart is LocalAuthentication)
                                LL = _cdr.AuthenticationStart as LocalAuthentication;

                            else if (_cdr.AuthenticationStart is RemoteAuthentication)
                                LL = (_cdr.AuthenticationStart as RemoteAuthentication).ToLocal;

                            if (LL != null)
                            {

                                var authStartResult = await eMob.AuthorizeStart(LL);

                                if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                    authStartResult.Result == AuthStartResultTypes.Blocked)
                                {
                                    cdrTargets[eMob].Add(_cdr);
                                    chargeDetailRecordsToProcess.Remove(_cdr);
                                    break;
                                }

                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  AuthToken/RFID identification

                    else if (_cdr.AuthenticationStop?.AuthToken != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.AuthenticationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(_cdr);
                                chargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) QR-Code identification

                    else if (_cdr.AuthenticationStart?.QRCodeIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.AuthenticationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(_cdr);
                                chargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  QR-Code identification

                    else if (_cdr.AuthenticationStop?.QRCodeIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.AuthenticationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(_cdr);
                                chargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) Plug'n'Charge identification

                    else if (_cdr.AuthenticationStart?.PlugAndChargeIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.AuthenticationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(_cdr);
                                chargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  Plug'n'Charge identification

                    else if (_cdr.AuthenticationStop?.PlugAndChargeIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.AuthenticationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(_cdr);
                                chargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) remote identification

                    else if (_cdr.AuthenticationStart?.RemoteIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.AuthenticationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(_cdr);
                                chargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  remote identification

                    else if (_cdr.AuthenticationStop?.RemoteIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.AuthenticationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(_cdr);
                                chargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) public key

                    else if (_cdr.AuthenticationStart?.PublicKey != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.AuthenticationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(_cdr);
                                chargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  public key

                    else if (_cdr.AuthenticationStop?.PublicKey != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.AuthenticationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultTypes.Authorized ||
                                authStartResult.Result == AuthStartResultTypes.Blocked)
                            {
                                cdrTargets[eMob].Add(_cdr);
                                chargeDetailRecordsToProcess.Remove(_cdr);
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

                    resultMap.Add(SendCDRResult.UnknownSessionId(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                 unknownCDR));

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
                                                                            CancellationToken,
                                                                            EventTrackingId,
                                                                            RequestTimeout);

                    if (partResults is null)
                    {

                        foreach (var _cdr in sendCDR.Value)
                        {
                            resultMap.Add(SendCDRResult.Error(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                              _cdr,
                                                              Warning.Create(I18NString.Create(Languages.en, sendCDR.Key + " returned null!"))));
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
                                                          Warning.Create(I18NString.Create(Languages.en, "Did not receive an result for this charge detail record!"))));
                    }
                }

                #endregion

                var Overview = new Dictionary<SendCDRResultTypes, UInt32>();
                foreach (var res in resultMap)
                {

                    if (!Overview.ContainsKey(res.Result))
                        Overview.Add(res.Result, 1);

                    else
                        Overview[res.Result]++;

                }

                var GlobalResultNumber = Overview.Values.Max();
                var GlobalResults      = Overview.Where(kvp => kvp.Value == GlobalResultNumber).Select(kvp => kvp.Key).ToList();
                if (GlobalResults.Count > 1)
                {

                    if (GlobalResults.Contains(SendCDRResultTypes.Success))
                        GlobalResults.Remove(SendCDRResultTypes.Success);

                    if (GlobalResults.Contains(SendCDRResultTypes.Enqueued))
                        GlobalResults.Remove(SendCDRResultTypes.Enqueued);

                }

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                result   = new SendCDRsResult(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                              Id,
                                              this as IReceiveChargeDetailRecords,
                                              GlobalResults[0].Convert(),
                                              resultMap,
                                              I18NString.Empty,
                                              null,
                                              Runtime);

            }


            #region Send OnSendCDRsResponse event

            try
            {

                OnSendCDRsResponse?.Invoke(Endtime,
                                           Timestamp.Value,
                                           this,
                                           Id.ToString(),
                                           EventTrackingId,
                                           Id,
                                           ChargeDetailRecords,
                                           RequestTimeout,
                                           result,
                                           Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(RoamingNetwork) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion


        #region ToJSON(this RoamingNetwork, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given roaming network.
        /// </summary>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        public JObject ToJSON(Boolean                                                    Embedded                                  = false,
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
                              CustomJObjectSerializerDelegate<EVSE>?                     CustomEVSESerializer                      = null)
        {

            var JSON = JSONObject.Create(

                         new JProperty("@id", Id.ToString()),

                         !Embedded
                             ? new JProperty("@context", JSONLDContext)
                             : null,

                         new JProperty("name", Name.ToJSON()),

                         Description.IsNeitherNullNorEmpty()
                             ? new JProperty("description", Description.ToJSON())
                             : null,

                         DataSource is not null && DataSource.IsNeitherNullNorEmpty()
                             ? new JProperty("dataSource", DataSource)
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

        #region GetHashCode()

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

            => String.Concat("'",
                             Name.FirstText(),
                             "' (",
                             Id.ToString(),
                             ")");

        #endregion

    }

}
