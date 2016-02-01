/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A Electric Vehicle Roaming Network is a service abstraction to allow multiple
    /// independent roaming services to be delivered over the same infrastructure.
    /// This can e.g. be a differentation of service levels (Premiun, Normal,
    /// Discount) or allow a simplified testing (Production, QA, Testing, ...)
    /// The RoamingNetwork class also provides access to all registered electric
    /// vehicle charging entities within an EV Roaming Network.
    /// </summary>
    public class RoamingNetwork : AEMobilityEntity<RoamingNetwork_Id>,
                                  IEquatable<RoamingNetwork>, IComparable<RoamingNetwork>, IComparable,
                                  IEnumerable<IEntity>,
                                  IStatus<RoamingNetworkStatusType>
    {

        #region Data


        #endregion

        public Boolean DisableStatusUpdates = false;

        #region Properties

        #region AuthorizatorId

        private readonly Authorizator_Id _AuthorizatorId;

        public Authorizator_Id AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion


        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional additional (multi-language) description of the RoamingNetwork.
        /// </summary>
        [Mandatory]
        public I18NString Description
        {

            get
            {
                return _Description;
            }

            set
            {
                SetProperty<I18NString>(ref _Description, value);
            }

        }

        #endregion


        #region Status

        /// <summary>
        /// The current roaming network status.
        /// </summary>
        [Optional]
        public Timestamped<RoamingNetworkStatusType> Status
        {
            get
            {
                return _StatusHistory.Peek();
            }
        }

        #endregion

        #region StatusHistory

        private Stack<Timestamped<RoamingNetworkStatusType>> _StatusHistory;

        /// <summary>
        /// The roaming network status history.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<RoamingNetworkStatusType>> StatusHistory
        {
            get
            {
                return _StatusHistory.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        #region StatusAggregationDelegate

        private Func<EVSEOperatorStatusReport, RoamingNetworkStatusType> _StatusAggregationDelegate;

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated EVSE operators.
        /// </summary>
        public Func<EVSEOperatorStatusReport, RoamingNetworkStatusType> StatusAggregationDelegate
        {

            get
            {
                return _StatusAggregationDelegate;
            }

            set
            {
                _StatusAggregationDelegate = value;
            }

        }

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current roaming network admin status.
        /// </summary>
        [Optional]
        public Timestamped<RoamingNetworkAdminStatusType> AdminStatus
        {
            get
            {
                return _AdminStatusHistory.Peek();
            }
        }

        #endregion

        #region AdminStatusHistory

        private Stack<Timestamped<RoamingNetworkAdminStatusType>> _AdminStatusHistory;

        /// <summary>
        /// The roaming network admin status history.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<RoamingNetworkAdminStatusType>> AdminStatusHistory
        {
            get
            {
                return _AdminStatusHistory.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        #region AdminStatusAggregationDelegate

        private Func<EVSEOperatorAdminStatusReport, RoamingNetworkAdminStatusType> _AdminStatusAggregationDelegate;

        /// <summary>
        /// A delegate called to aggregate the admin status of all subordinated EVSE operators.
        /// </summary>
        public Func<EVSEOperatorAdminStatusReport, RoamingNetworkAdminStatusType> AdminStatusAggregationDelegate
        {

            get
            {
                return _AdminStatusAggregationDelegate;
            }

            set
            {
                _AdminStatusAggregationDelegate = value;
            }

        }

        #endregion

        #endregion

        #region Events

        #region OnAggregatedStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="RoamingNetwork">The updated roaming network.</param>
        /// <param name="OldStatus">The old timestamped status of the roaming network.</param>
        /// <param name="NewStatus">The new timestamped status of the roaming network.</param>
        public delegate void OnAggregatedStatusChangedDelegate(DateTime Timestamp, RoamingNetwork RoamingNetwork, Timestamped<RoamingNetworkStatusType> OldStatus, Timestamped<RoamingNetworkStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status changed.
        /// </summary>
        public event OnAggregatedStatusChangedDelegate OnAggregatedStatusChanged;

        #endregion

        #region OnAggregatedAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated admin status changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="RoamingNetwork">The updated roaming network.</param>
        /// <param name="OldStatus">The old timestamped status of the roaming network.</param>
        /// <param name="NewStatus">The new timestamped status of the roaming network.</param>
        public delegate void OnAggregatedAdminStatusChangedDelegate(DateTime Timestamp, RoamingNetwork RoamingNetwork, Timestamped<RoamingNetworkAdminStatusType> OldStatus, Timestamped<RoamingNetworkAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated admin status changed.
        /// </summary>
        public event OnAggregatedAdminStatusChangedDelegate OnAggregatedAdminStatusChanged;

        #endregion

        #endregion

        #region Constructor(s)

        #region RoamingNetwork()

        /// <summary>
        /// Create a new roaming network having a random
        /// roaming network identification.
        /// </summary>
        public RoamingNetwork()
            : this(RoamingNetwork_Id.New)
        { }

        #endregion

        #region RoamingNetwork(Id, AuthorizatorId = null)

        /// <summary>
        /// Create a new roaming network having the given
        /// unique roaming network identification.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="AuthorizatorId">The unique identification for the Auth service.</param>
        public RoamingNetwork(RoamingNetwork_Id  Id,
                              Authorizator_Id    AuthorizatorId = null)

            : base(Id)

        {

            #region Init data and properties

            this._AuthorizatorId                           = (AuthorizatorId == null) ? Authorizator_Id.Parse("GraphDefined E-Mobility Gateway") : AuthorizatorId;
            this._Description                              = new I18NString();

            this._EVSEOperators                            = new ConcurrentDictionary<EVSEOperator_Id,              EVSEOperator>();
            this._EVServiceProviders                       = new ConcurrentDictionary<EVSP_Id,                      EVSP>();
            this._CPORoamingProviders                      = new ConcurrentDictionary<RoamingProvider_Id,           CPORoamingProvider>();
            this._EMPRoamingProviders                      = new ConcurrentDictionary<RoamingProvider_Id,           EMPRoamingProvider>();
            this._SearchProviders                          = new ConcurrentDictionary<NavigationServiceProvider_Id, NavigationServiceProvider>();
            this._ChargingReservations                     = new ConcurrentDictionary<ChargingReservation_Id,       EVSEOperator>();
            this._IeMobilityServiceProviders               = new ConcurrentDictionary<UInt32,                       IeMobilityServiceProvider>();
            this._OperatorRoamingServices                  = new ConcurrentDictionary<UInt32,                       IOperatorRoamingService>();
            this._eMobilityRoamingServices                 = new ConcurrentDictionary<UInt32,                       IeMobilityRoamingService>();
            this._PushEVSEStatusToOperatorRoamingServices  = new ConcurrentDictionary<UInt32,                       IPushDataAndStatus>();
            this._ChargingSessions                         = new ConcurrentDictionary<ChargingSession_Id,           ChargingSession>();
            this._ChargeDetailRecords                      = new ConcurrentDictionary<ChargingSession_Id,           ChargeDetailRecord>();

            #endregion

            #region Init events

            // RoamingNetwork events
            this.EVSEOperatorAddition        = new VotingNotificator<DateTime, RoamingNetwork,  EVSEOperator,              Boolean>(() => new VetoVote(), true);
            this.EVSEOperatorRemoval         = new VotingNotificator<DateTime, RoamingNetwork,  EVSEOperator,              Boolean>(() => new VetoVote(), true);

            this.EVServiceProviderAddition   = new VotingNotificator<RoamingNetwork,  EVSP,                                Boolean>(() => new VetoVote(), true);
            this.EVServiceProviderRemoval    = new VotingNotificator<RoamingNetwork,  EVSP,                                Boolean>(() => new VetoVote(), true);

            this.CPORoamingProviderAddition  = new VotingNotificator<RoamingNetwork,  CPORoamingProvider,                  Boolean>(() => new VetoVote(), true);
            this.CPORoamingProviderRemoval   = new VotingNotificator<RoamingNetwork,  CPORoamingProvider,                  Boolean>(() => new VetoVote(), true);

            this.EMPRoamingProviderAddition  = new VotingNotificator<RoamingNetwork,  EMPRoamingProvider,                  Boolean>(() => new VetoVote(), true);
            this.EMPRoamingProviderRemoval   = new VotingNotificator<RoamingNetwork,  EMPRoamingProvider,                  Boolean>(() => new VetoVote(), true);

            this.SearchProviderAddition      = new VotingNotificator<RoamingNetwork,  NavigationServiceProvider,           Boolean>(() => new VetoVote(), true);
            this.SearchProviderRemoval       = new VotingNotificator<RoamingNetwork,  NavigationServiceProvider,           Boolean>(() => new VetoVote(), true);

            // EVSEOperator events
            this.ChargingPoolAddition        = new VotingNotificator<DateTime, EVSEOperator,    ChargingPool,              Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval         = new VotingNotificator<DateTime, EVSEOperator,    ChargingPool,              Boolean>(() => new VetoVote(), true);

            // ChargingPool events
            this.ChargingStationAddition     = new VotingNotificator<DateTime, ChargingPool,    ChargingStation,           Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval      = new VotingNotificator<DateTime, ChargingPool,    ChargingStation,           Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition                = new VotingNotificator<DateTime, ChargingStation, EVSE,                      Boolean>(() => new VetoVote(), true);
            this.EVSERemoval                 = new VotingNotificator<DateTime, ChargingStation, EVSE,                      Boolean>(() => new VetoVote(), true);

            // EVSE events
            this.SocketOutletAddition        = new VotingNotificator<DateTime, EVSE,            SocketOutlet,              Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval         = new VotingNotificator<DateTime, EVSE,            SocketOutlet,              Boolean>(() => new VetoVote(), true);

            #endregion

        }

        #endregion

        #endregion


        #region Search providers...

        #region SearchProviders

        private readonly ConcurrentDictionary<NavigationServiceProvider_Id, NavigationServiceProvider> _SearchProviders;

        /// <summary>
        /// Return all search providers registered within this roaming network.
        /// </summary>
        public IEnumerable<NavigationServiceProvider> SearchProviders
        {
            get
            {
                return _SearchProviders.Select(KVP => KVP.Value);
            }
        }

        #endregion

        #region SearchProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, NavigationServiceProvider, Boolean> SearchProviderAddition;

        /// <summary>
        /// Called whenever an SearchProvider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, NavigationServiceProvider, Boolean> OnSearchProviderAddition
        {
            get
            {
                return SearchProviderAddition;
            }
        }

        #endregion

        #region SearchProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, NavigationServiceProvider, Boolean> SearchProviderRemoval;

        /// <summary>
        /// Called whenever an SearchProvider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, NavigationServiceProvider, Boolean> OnSearchProviderRemoval
        {
            get
            {
                return SearchProviderRemoval;
            }
        }

        #endregion

        #region CreateNewSearchProvider(NavigationServiceProviderId, Configurator = null)

        /// <summary>
        /// Create and register a new navigation service provider having the given
        /// unique identification.
        /// </summary>
        /// <param name="NavigationServiceProviderId">The unique identification of the new search provider.</param>
        /// <param name="Configurator">An optional delegate to configure the new search provider after its creation.</param>
        public NavigationServiceProvider CreateNewSearchProvider(NavigationServiceProvider_Id       NavigationServiceProviderId,
                                                                 Action<NavigationServiceProvider>  Configurator = null)
        {

            #region Initial checks

            if (NavigationServiceProviderId == null)
                throw new ArgumentNullException("NavigationServiceProviderId", "The given navigation service provider identification must not be null!");

            if (_SearchProviders.ContainsKey(NavigationServiceProviderId))
                throw new SearchProviderAlreadyExists(NavigationServiceProviderId, this.Id);

            #endregion

            var _SearchProvider = new NavigationServiceProvider(NavigationServiceProviderId, this);

            Configurator.FailSafeInvoke(_SearchProvider);

            if (SearchProviderAddition.SendVoting(this, _SearchProvider))
            {
                if (_SearchProviders.TryAdd(NavigationServiceProviderId, _SearchProvider))
                {
                    SearchProviderAddition.SendNotification(this, _SearchProvider);
                    return _SearchProvider;
                }
            }

            throw new Exception("Could not create new search provider '" + NavigationServiceProviderId + "'!");

        }

        #endregion

        #endregion

        #region e-Mobility Service Providers...

        private readonly ConcurrentDictionary<UInt32, IeMobilityServiceProvider> _IeMobilityServiceProviders;

        #region EVServiceProviders

        private readonly ConcurrentDictionary<EVSP_Id, EVSP> _EVServiceProviders;

        /// <summary>
        /// Return all EV service providers registered within this roaming network.
        /// </summary>
        public IEnumerable<EVSP> EVServiceProviders
        {
            get
            {
                return _EVServiceProviders.Select(KVP => KVP.Value);
            }
        }

        #endregion

        #region EVServiceProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, EVSP, Boolean> EVServiceProviderAddition;

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, EVSP, Boolean> OnEVServiceProviderAddition
        {
            get
            {
                return EVServiceProviderAddition;
            }
        }

        #endregion

        #region EVServiceProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, EVSP, Boolean> EVServiceProviderRemoval;

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, EVSP, Boolean> OnEVServiceProviderRemoval
        {
            get
            {
                return EVServiceProviderRemoval;
            }
        }

        #endregion


        #region CreateNewEVServiceProvider(EVServiceProviderId, Configurator = null)

        /// <summary>
        /// Create and register a new electric vehicle service provider having the given
        /// unique electric vehicle service provider identification.
        /// </summary>
        /// <param name="EVServiceProviderId">The unique identification of the new roaming provider.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public EVSP CreateNewEVServiceProvider(EVSP_Id       EVServiceProviderId,
                                               Action<EVSP>  Configurator  = null)
        {

            #region Initial checks

            if (EVServiceProviderId == null)
                throw new ArgumentNullException("EVServiceProviderId", "The given electric vehicle service provider identification must not be null!");

            if (_EVServiceProviders.ContainsKey(EVServiceProviderId))
                throw new EVServiceProviderAlreadyExists(EVServiceProviderId, this.Id);

            #endregion

            var _EVServiceProvider = new EVSP(EVServiceProviderId, this);

            Configurator.FailSafeInvoke(_EVServiceProvider);

            if (EVServiceProviderAddition.SendVoting(this, _EVServiceProvider))
            {
                if (_EVServiceProviders.TryAdd(EVServiceProviderId, _EVServiceProvider))
                {
                    EVServiceProviderAddition.SendNotification(this, _EVServiceProvider);
                    return _EVServiceProvider;
                }
            }

            throw new Exception("Could not create new ev service provider '" + EVServiceProviderId + "'!");

        }

        #endregion

        #region RegistereMobilityServiceProvider(Priority, eMobilityServiceProvider)

        /// <summary>
        /// Register the given e-Mobility service provider.
        /// </summary>
        /// <param name="Priority">The priority of the service provider.</param>
        /// <param name="eMobilityServiceProvider">An e-Mobility service provider.</param>
        public Boolean RegistereMobilityServiceProvider(UInt32                     Priority,
                                                        IeMobilityServiceProvider  eMobilityServiceProvider)
        {

            return _IeMobilityServiceProviders.TryAdd(Priority, eMobilityServiceProvider);

        }

        #endregion


        #region AllTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AllTokens
        {
            get
            {
                return _IeMobilityServiceProviders.SelectMany(vv => vv.Value.AllTokens);
            }
        }

        #endregion

        #region AuthorizedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AuthorizedTokens
        {
            get
            {
                return _IeMobilityServiceProviders.SelectMany(vv => vv.Value.AuthorizedTokens);
            }
        }

        #endregion

        #region NotAuthorizedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> NotAuthorizedTokens
        {
            get
            {
                return _IeMobilityServiceProviders.SelectMany(vv => vv.Value.NotAuthorizedTokens);
            }
        }

        #endregion

        #region BlockedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> BlockedTokens
        {
            get
            {
                return _IeMobilityServiceProviders.SelectMany(vv => vv.Value.BlockedTokens);
            }
        }

        #endregion

        #endregion


        #region Operator Roaming Providers...

        private readonly ConcurrentDictionary<UInt32, IOperatorRoamingService>   _OperatorRoamingServices;
        private readonly ConcurrentDictionary<UInt32, IeMobilityRoamingService>  _eMobilityRoamingServices;
        private readonly ConcurrentDictionary<UInt32, IPushDataAndStatus>        _PushEVSEStatusToOperatorRoamingServices;

        #region CPORoamingProviders

        private readonly ConcurrentDictionary<RoamingProvider_Id, CPORoamingProvider> _CPORoamingProviders;

        /// <summary>
        /// Return all roaming providers registered within this roaming network.
        /// </summary>
        public IEnumerable<CPORoamingProvider> CPORoamingProviders
        {
            get
            {
                return _CPORoamingProviders.Values;
            }
        }

        #endregion

        #region CreateNewRoamingProvider(OperatorRoamingService, Configurator = null)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider having the given
        /// unique electric vehicle roaming provider identification.
        /// </summary>
        /// <param name="OperatorRoamingService">The attached E-Mobility service.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public CPORoamingProvider CreateNewRoamingProvider(IOperatorRoamingService     OperatorRoamingService,
                                                           Action<CPORoamingProvider>  Configurator = null)
        {

            #region Initial checks

            if (OperatorRoamingService.Id == null)
                throw new ArgumentNullException("OperatorRoamingService.Id",    "The given roaming provider identification must not be null!");

            if (OperatorRoamingService.Name.IsNullOrEmpty())
                throw new ArgumentNullException("OperatorRoamingService.Name",  "The given roaming provider name must not be null or empty!");

            if (_CPORoamingProviders.ContainsKey(OperatorRoamingService.Id))
                throw new RoamingProviderAlreadyExists(OperatorRoamingService.Id, this.Id);

            if (OperatorRoamingService.RoamingNetworkId != this.Id)
                throw new ArgumentException("The given operator roaming service is not part of this roaming network!", "OperatorRoamingService");

            #endregion

            var _CPORoamingProvider = new CPORoamingProvider(OperatorRoamingService.Id,
                                                             OperatorRoamingService.Name,
                                                             this,
                                                             OperatorRoamingService);

            Configurator.FailSafeInvoke(_CPORoamingProvider);

            if (CPORoamingProviderAddition.SendVoting(this, _CPORoamingProvider))
            {
                if (_CPORoamingProviders.TryAdd(OperatorRoamingService.Id, _CPORoamingProvider))
                {

                    CPORoamingProviderAddition.SendNotification(this, _CPORoamingProvider);

                    return _CPORoamingProvider;

                }
            }

            throw new Exception("Could not create new roaming provider '" + OperatorRoamingService.Id + "'!");

        }

        #endregion

        #region CPORoamingProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, CPORoamingProvider, Boolean> CPORoamingProviderAddition;

        /// <summary>
        /// Called whenever a RoamingProvider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, CPORoamingProvider, Boolean> OnCPORoamingProviderAddition
        {
            get
            {
                return CPORoamingProviderAddition;
            }
        }

        #endregion

        #region CPORoamingProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, CPORoamingProvider, Boolean> CPORoamingProviderRemoval;

        /// <summary>
        /// Called whenever a RoamingProvider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, CPORoamingProvider, Boolean> OnCPORoamingProviderRemoval
        {
            get
            {
                return CPORoamingProviderRemoval;
            }
        }

        #endregion


        #region EMPRoamingProviders

        private readonly ConcurrentDictionary<RoamingProvider_Id, EMPRoamingProvider> _EMPRoamingProviders;

        /// <summary>
        /// Return all roaming providers registered within this roaming network.
        /// </summary>
        public IEnumerable<EMPRoamingProvider> EMPRoamingProviders
        {
            get
            {
                return _EMPRoamingProviders.Values;
            }
        }

        #endregion

        #region CreateNewRoamingProvider(eMobilityRoamingService, Configurator = null)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider having the given
        /// unique electric vehicle roaming provider identification.
        /// </summary>
        /// <param name="eMobilityRoamingService">A e-mobility roaming service.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public EMPRoamingProvider CreateNewRoamingProvider(IeMobilityRoamingService    eMobilityRoamingService,
                                                           Action<EMPRoamingProvider>  Configurator = null)
        {

            #region Initial checks

            if (eMobilityRoamingService.Id == null)
                throw new ArgumentNullException("eMobilityRoamingService.Id",    "The given roaming provider identification must not be null!");

            if (eMobilityRoamingService.Name.IsNullOrEmpty())
                throw new ArgumentNullException("eMobilityRoamingService.Name",  "The given roaming provider name must not be null or empty!");

            if (_EMPRoamingProviders.ContainsKey(eMobilityRoamingService.Id))
                throw new RoamingProviderAlreadyExists(eMobilityRoamingService.Id, this.Id);

            if (eMobilityRoamingService.RoamingNetworkId != this.Id)
                throw new ArgumentException("The given operator roaming service is not part of this roaming network!", "eMobilityRoamingService");

            #endregion

            var _EMPRoamingProvider = new EMPRoamingProvider(eMobilityRoamingService.Id,
                                                             eMobilityRoamingService.Name,
                                                             this,
                                                             eMobilityRoamingService);

            Configurator.FailSafeInvoke(_EMPRoamingProvider);

            if (EMPRoamingProviderAddition.SendVoting(this, _EMPRoamingProvider))
            {
                if (_EMPRoamingProviders.TryAdd(eMobilityRoamingService.Id, _EMPRoamingProvider))
                {

                    EMPRoamingProviderAddition.SendNotification(this, _EMPRoamingProvider);

                    return _EMPRoamingProvider;

                }
            }

            throw new Exception("Could not create new roaming provider '" + eMobilityRoamingService.Id + "'!");

        }

        #endregion

        #region EMPRoamingProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, EMPRoamingProvider, Boolean> EMPRoamingProviderAddition;

        /// <summary>
        /// Called whenever a RoamingProvider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, EMPRoamingProvider, Boolean> OnEMPRoamingProviderAddition
        {
            get
            {
                return EMPRoamingProviderAddition;
            }
        }

        #endregion

        #region EMPRoamingProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, EMPRoamingProvider, Boolean> EMPRoamingProviderRemoval;

        /// <summary>
        /// Called whenever a RoamingProvider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, EMPRoamingProvider, Boolean> OnEMPRoamingProviderRemoval
        {
            get
            {
                return EMPRoamingProviderRemoval;
            }
        }

        #endregion


        #region RegisterOperatorRoamingService(Priority, OperatorRoamingService)

        /// <summary>
        /// Register the given EVSE operator roaming service.
        /// </summary>
        /// <param name="Priority">The priority of the service.</param>
        /// <param name="OperatorRoamingService">The EVSE operator roaming service.</param>
        public Boolean RegisterOperatorRoamingService(UInt32                   Priority,
                                                      IOperatorRoamingService  OperatorRoamingService)
        {

            return _OperatorRoamingServices.TryAdd(Priority, OperatorRoamingService);

        }

        #endregion

        #region RegistereMobilityRoamingService(Priority, eMobilityRoamingService)

        /// <summary>
        /// Register the given e-mobility roaming service.
        /// </summary>
        /// <param name="Priority">The priority of the service.</param>
        /// <param name="eMobilityRoamingService">The e-mobility roaming service.</param>
        public Boolean RegistereMobilityRoamingService(UInt32                    Priority,
                                                       IeMobilityRoamingService  eMobilityRoamingService)
        {

            return _eMobilityRoamingServices.TryAdd(Priority, eMobilityRoamingService);

        }

        #endregion

        #region RegisterPushEVSEStatusService(Priority, PushEVSEStatusServices)

        /// <summary>
        /// Register the given push-status service.
        /// </summary>
        /// <param name="Priority">The priority of the service.</param>
        /// <param name="PushEVSEStatusServices">The push-status service.</param>
        public Boolean RegisterPushEVSEStatusService(UInt32                   Priority,
                                                     IPushDataAndStatus  PushEVSEStatusServices)
        {

            return _PushEVSEStatusToOperatorRoamingServices.TryAdd(Priority, PushEVSEStatusServices);

        }

        #endregion

        #endregion


        #region EVSE operators...

        #region EVSEOperators

        private readonly ConcurrentDictionary<EVSEOperator_Id, EVSEOperator> _EVSEOperators;

        /// <summary>
        /// Return all EVSE operators registered within this roaming network.
        /// </summary>
        public IEnumerable<EVSEOperator> EVSEOperators
        {
            get
            {
                return _EVSEOperators.Select(KVP => KVP.Value);
            }
        }

        #endregion

        #region EVSEOperatorAddition

        private readonly IVotingNotificator<DateTime, RoamingNetwork, EVSEOperator, Boolean> EVSEOperatorAddition;

        /// <summary>
        /// Called whenever an EVSEOperator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, EVSEOperator, Boolean> OnEVSEOperatorAddition
        {
            get
            {
                return EVSEOperatorAddition;
            }
        }

        #endregion

        #region EVSEOperatorRemoval

        private readonly IVotingNotificator<DateTime, RoamingNetwork, EVSEOperator, Boolean> EVSEOperatorRemoval;

        /// <summary>
        /// Called whenever an EVSEOperator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, EVSEOperator, Boolean> OnEVSEOperatorRemoval
        {
            get
            {
                return EVSEOperatorRemoval;
            }
        }

        #endregion


        #region CreateNewEVSEOperator(EVSEOperatorId, Name = null, Description = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new EVSE operator having the given
        /// unique EVSE operator identification.
        /// </summary>
        /// <param name="EVSEOperatorId">The unique identification of the new EVSE operator.</param>
        /// <param name="Name">The offical (multi-language) name of the EVSE Operator.</param>
        /// <param name="Description">An optional (multi-language) description of the EVSE Operator.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE operator before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new EVSE operator after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the EVSE operator failed.</param>
        public EVSEOperator CreateNewEVSEOperator(EVSEOperator_Id                          EVSEOperatorId,
                                                  I18NString                               Name                       = null,
                                                  I18NString                               Description                = null,
                                                  Action<EVSEOperator>                     Configurator               = null,
                                                  Action<EVSEOperator>                     OnSuccess                  = null,
                                                  Action<RoamingNetwork, EVSEOperator_Id>  OnError                    = null,
                                                  Func<EVSEOperator, IRemoteEVSEOperator>  RemoteEVSEOperatorCreator  = null)
        {

            #region Initial checks

            if (EVSEOperatorId == null)
                throw new ArgumentNullException("EVSEOperatorId", "The given EVSE operator identification must not be null!");

            if (_EVSEOperators.ContainsKey(EVSEOperatorId))
                throw new EVSEOperatorAlreadyExists(EVSEOperatorId, this.Id);

            #endregion

            var _EVSEOperator = new EVSEOperator(EVSEOperatorId, Name, Description, this);

            if (Configurator != null)
                Configurator(_EVSEOperator);

            if (EVSEOperatorAddition.SendVoting(DateTime.Now, this, _EVSEOperator))
            {
                if (_EVSEOperators.TryAdd(EVSEOperatorId, _EVSEOperator))
                {

                    _EVSEOperator.OnEVSEDataChanged                             += UpdateEVSEData;
                    _EVSEOperator.OnEVSEStatusChanged                           += (Timestamp, EVSE, OldStatus, NewStatus)
                                                                                    => UpdateEVSEStatus(Timestamp, EVSE, OldStatus, NewStatus);
                    _EVSEOperator.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;


                    _EVSEOperator.OnChargingStationDataChanged                  += UpdateChargingStationData;
                    _EVSEOperator.OnChargingStationAdminStatusChanged           += UpdateChargingStationAdminStatus;
                    _EVSEOperator.OnAggregatedChargingStationStatusChanged      += UpdateAggregatedChargingStationStatus;
                    _EVSEOperator.OnAggregatedChargingStationAdminStatusChanged += UpdateChargingStationAdminStatus;

                    _EVSEOperator.OnChargingPoolDataChanged                     += UpdateChargingPoolData;
                    _EVSEOperator.OnChargingPoolAdminStatusChanged              += UpdateChargingPoolAdminStatus;
                    _EVSEOperator.OnAggregatedChargingPoolStatusChanged         += UpdateChargingPoolStatus;
                    _EVSEOperator.OnAggregatedChargingPoolAdminStatusChanged    += UpdateChargingPoolAdminStatus;

                    _EVSEOperator.OnPropertyChanged                             += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                                                                                    => UpdateEVSEOperatorData(Timestamp, Sender as EVSEOperator, PropertyName, OldValue, NewValue);
                    _EVSEOperator.OnAggregatedStatusChanged                     += UpdateStatus;
                    _EVSEOperator.OnAggregatedAdminStatusChanged                += UpdateAdminStatus;

                    _EVSEOperator.OnNewReservation                              += SendNewReservation;
                    _EVSEOperator.OnNewChargingSession                          += SendNewChargingSession;
                    _EVSEOperator.OnNewChargeDetailRecord                       += SendNewChargeDetailRecord;


                    OnSuccess.FailSafeInvoke(_EVSEOperator);
                    EVSEOperatorAddition.SendNotification(DateTime.Now, this, _EVSEOperator);

                    if (RemoteEVSEOperatorCreator != null)
                        _EVSEOperator.RemoteEVSEOperator = RemoteEVSEOperatorCreator(_EVSEOperator);

                    return _EVSEOperator;

                }
            }

            throw new Exception("Could not create new EVSE operator '" + EVSEOperatorId + "'!");

        }

        #endregion

        #region ContainsEVSEOperator(EVSEOperator)

        /// <summary>
        /// Check if the given EVSEOperator is already present within the roaming network.
        /// </summary>
        /// <param name="EVSEOperator">An EVSE operator.</param>
        public Boolean ContainsEVSEOperator(EVSEOperator EVSEOperator)
        {
            return _EVSEOperators.ContainsKey(EVSEOperator.Id);
        }

        #endregion

        #region ContainsEVSEOperator(EVSEOperatorId)

        /// <summary>
        /// Check if the given EVSEOperator identification is already present within the roaming network.
        /// </summary>
        /// <param name="EVSEOperatorId">The unique identification of the EVSE operator.</param>
        public Boolean ContainsEVSEOperator(EVSEOperator_Id EVSEOperatorId)
        {
            return _EVSEOperators.ContainsKey(EVSEOperatorId);
        }

        #endregion

        #region GetEVSEOperatorbyId(EVSEOperatorId)

        public EVSEOperator GetEVSEOperatorbyId(EVSEOperator_Id EVSEOperatorId)
        {

            EVSEOperator _EVSEOperator = null;

            if (_EVSEOperators.TryGetValue(EVSEOperatorId, out _EVSEOperator))
                return _EVSEOperator;

            return null;

        }

        #endregion

        #region TryGetEVSEOperatorbyId(EVSEOperatorId, out EVSEOperator)

        public Boolean TryGetEVSEOperatorbyId(EVSEOperator_Id EVSEOperatorId, out EVSEOperator EVSEOperator)
        {
            return _EVSEOperators.TryGetValue(EVSEOperatorId, out EVSEOperator);
        }

        #endregion

        #region RemoveEVSEOperator(EVSEOperatorId)

        public EVSEOperator RemoveEVSEOperator(EVSEOperator_Id EVSEOperatorId)
        {

            EVSEOperator _EVSEOperator = null;

            if (_EVSEOperators.TryRemove(EVSEOperatorId, out _EVSEOperator))
                return _EVSEOperator;

            return null;

        }

        #endregion

        #region TryRemoveEVSEOperator(EVSEOperatorId, out EVSEOperator)

        public Boolean TryRemoveEVSEOperator(EVSEOperator_Id EVSEOperatorId, out EVSEOperator EVSEOperator)
        {
            return _EVSEOperators.TryRemove(EVSEOperatorId, out EVSEOperator);
        }

        #endregion


        #region OnEVSEOperatorDataChanged

        /// <summary>
        /// A delegate called whenever the static data of any subordinated EVSE operator changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSEOperator">The updated evse operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        public delegate void OnEVSEOperatorDataChangedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, String PropertyName, Object OldValue, Object NewValue);

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE operator changed.
        /// </summary>
        public event OnEVSEOperatorDataChangedDelegate OnEVSEOperatorDataChanged;

        #endregion

        #region OnAggregatedEVSEOperatorStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of any subordinated EVSE operator changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSEOperator">The updated EVSE operator.</param>
        /// <param name="OldStatus">The old timestamped status of the EVSE operator.</param>
        /// <param name="NewStatus">The new timestamped status of the EVSE operator.</param>
        public delegate void OnAggregatedEVSEOperatorStatusChangedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, Timestamped<EVSEOperatorStatusType> OldStatus, Timestamped<EVSEOperatorStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated EVSE operator changed.
        /// </summary>
        public event OnAggregatedEVSEOperatorStatusChangedDelegate OnAggregatedEVSEOperatorStatusChanged;

        #endregion

        #region OnAggregatedEVSEOperatorAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated admin status of any subordinated EVSE operator changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSEOperator">The updated EVSE operator.</param>
        /// <param name="OldStatus">The old timestamped status of the EVSE operator.</param>
        /// <param name="NewStatus">The new timestamped status of the EVSE operator.</param>
        public delegate void OnAggregatedEVSEOperatorAdminStatusChangedDelegate(DateTime Timestamp, EVSEOperator EVSEOperator, Timestamped<EVSEOperatorAdminStatusType> OldStatus, Timestamped<EVSEOperatorAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated EVSE operator changed.
        /// </summary>
        public event OnAggregatedEVSEOperatorAdminStatusChangedDelegate OnAggregatedEVSEOperatorAdminStatusChanged;

        #endregion

        #region ChargingPoolAddition

        internal readonly IVotingNotificator<DateTime, EVSEOperator, ChargingPool, Boolean> ChargingPoolAddition;

        /// <summary>
        /// Called whenever an EVS pool will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EVSEOperator, ChargingPool, Boolean> OnChargingPoolAddition
        {
            get
            {
                return ChargingPoolAddition;
            }
        }

        #endregion

        #region ChargingPoolRemoval

        internal readonly IVotingNotificator<DateTime, EVSEOperator, ChargingPool, Boolean> ChargingPoolRemoval;

        /// <summary>
        /// Called whenever an EVS pool will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EVSEOperator, ChargingPool, Boolean> OnChargingPoolRemoval
        {
            get
            {
                return ChargingPoolRemoval;
            }
        }

        #endregion


        #region (internal) UpdateEVSEOperatorData(Timestamp, EVSEOperator, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an evse operator.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSEOperator">The changed evse operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateEVSEOperatorData(DateTime      Timestamp,
                                             EVSEOperator  EVSEOperator,
                                             String        PropertyName,
                                             Object        OldValue,
                                             Object        NewValue)
        {

            var OnEVSEOperatorDataChangedLocal = OnEVSEOperatorDataChanged;
            if (OnEVSEOperatorDataChangedLocal != null)
                OnEVSEOperatorDataChangedLocal(Timestamp, EVSEOperator, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateStatus(Timestamp, EVSEOperator, OldStatus, NewStatus)

        /// <summary>
        /// Update the current EVSE operator status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSEOperator">The updated EVSE operator.</param>
        /// <param name="OldStatus">The old aggreagted EVSE operator status.</param>
        /// <param name="NewStatus">The new aggreagted EVSE operator status.</param>
        internal void UpdateStatus(DateTime                             Timestamp,
                                   EVSEOperator                         EVSEOperator,
                                   Timestamped<EVSEOperatorStatusType>  OldStatus,
                                   Timestamped<EVSEOperatorStatusType>  NewStatus)
        {

            // Send EVSE operator status change upstream
            var OnAggregatedEVSEOperatorStatusChangedLocal = OnAggregatedEVSEOperatorStatusChanged;
            if (OnAggregatedEVSEOperatorStatusChangedLocal != null)
                OnAggregatedEVSEOperatorStatusChangedLocal(Timestamp, EVSEOperator, OldStatus, NewStatus);


            // Calculate new aggregated roaming network status and send upstream
            if (StatusAggregationDelegate != null)
            {

                var NewAggregatedStatus = new Timestamped<RoamingNetworkStatusType>(StatusAggregationDelegate(new EVSEOperatorStatusReport(_EVSEOperators.Values)));

                if (NewAggregatedStatus.Value != _StatusHistory.Peek().Value)
                {

                    var OldAggregatedStatus = _StatusHistory.Peek();

                    _StatusHistory.Push(NewAggregatedStatus);

                    var OnAggregatedStatusChangedLocal = OnAggregatedStatusChanged;
                    if (OnAggregatedStatusChangedLocal != null)
                        OnAggregatedStatusChangedLocal(Timestamp, this, OldAggregatedStatus, NewAggregatedStatus);

                }

            }

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, EVSEOperator, OldStatus, NewStatus)

        /// <summary>
        /// Update the current EVSE operator admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSEOperator">The updated EVSE operator.</param>
        /// <param name="OldStatus">The old aggreagted EVSE operator status.</param>
        /// <param name="NewStatus">The new aggreagted EVSE operator status.</param>
        internal void UpdateAdminStatus(DateTime                                  Timestamp,
                                        EVSEOperator                              EVSEOperator,
                                        Timestamped<EVSEOperatorAdminStatusType>  OldStatus,
                                        Timestamped<EVSEOperatorAdminStatusType>  NewStatus)
        {

            // Send EVSE operator admin status change upstream
            var OnAggregatedEVSEOperatorAdminStatusChangedLocal = OnAggregatedEVSEOperatorAdminStatusChanged;
            if (OnAggregatedEVSEOperatorAdminStatusChangedLocal != null)
                OnAggregatedEVSEOperatorAdminStatusChangedLocal(Timestamp, EVSEOperator, OldStatus, NewStatus);


            // Calculate new aggregated roaming network status and send upstream
            if (AdminStatusAggregationDelegate != null)
            {

                var NewAggregatedStatus = new Timestamped<RoamingNetworkAdminStatusType>(AdminStatusAggregationDelegate(new EVSEOperatorAdminStatusReport(_EVSEOperators.Values)));

                if (NewAggregatedStatus.Value != _AdminStatusHistory.Peek().Value)
                {

                    var OldAggregatedStatus = _AdminStatusHistory.Peek();

                    _AdminStatusHistory.Push(NewAggregatedStatus);

                    var OnAggregatedAdminStatusChangedLocal = OnAggregatedAdminStatusChanged;
                    if (OnAggregatedAdminStatusChangedLocal != null)
                        OnAggregatedAdminStatusChangedLocal(Timestamp, this, OldAggregatedStatus, NewAggregatedStatus);

                }

            }

        }

        #endregion

        #endregion

        #region ChargingPools...

        #region ChargingPools

        /// <summary>
        /// Return all charging pools registered within this roaming network.
        /// </summary>
        public IEnumerable<ChargingPool> ChargingPools
        {
            get
            {
                return _EVSEOperators.SelectMany(evseoperator => evseoperator.Value);
            }
        }

        #endregion

        #region ChargingPoolStatus

        /// <summary>
        /// Return the status of all charging pools registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusType>>>> ChargingPoolStatus
        {
            get
            {

                return _EVSEOperators.Values.
                           SelectMany(evseoperator =>
                               evseoperator.Select(pool =>

                                           new KeyValuePair<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusType>>>(
                                               pool.Id,
                                               pool.StatusSchedule)

                                       ));

            }
        }

        #endregion


        #region ContainsChargingPool(ChargingPool)

        /// <summary>
        /// Check if the given charging pool is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public Boolean ContainsChargingPool(ChargingPool ChargingPool)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingPool.Operator.Id, out _EVSEOperator))
                return _EVSEOperator.ContainsChargingPool(ChargingPool.Id);

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

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingPool_Id.Parse(ChargingPoolId.ToString()).OperatorId, out _EVSEOperator))
                return _EVSEOperator.ContainsChargingPool(ChargingPoolId);

            return false;

        }

        #endregion

        #region GetChargingPoolbyId(ChargingPoolId)

        public ChargingPool GetChargingPoolbyId(ChargingPool_Id ChargingPoolId)
        {

            ChargingPool _ChargingPool  = null;
            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingPool_Id.Parse(ChargingPoolId.ToString()).OperatorId, out _EVSEOperator))
                if (_EVSEOperator.TryGetChargingPoolbyId(ChargingPoolId, out _ChargingPool))
                    return _ChargingPool;

            return null;

        }

        #endregion

        #region TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool)

        public Boolean TryGetChargingPoolbyId(ChargingPool_Id ChargingPoolId, out ChargingPool ChargingPool)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingPool_Id.Parse(ChargingPoolId.ToString()).OperatorId, out _EVSEOperator))
                return _EVSEOperator.TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool);

            ChargingPool = null;
            return false;

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, StatusList)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                                        ChargingPoolId,
                                               IEnumerable<Timestamped<ChargingPoolAdminStatusType>>  StatusList)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingPool_Id.Parse(ChargingPoolId.ToString()).OperatorId, out _EVSEOperator))
                _EVSEOperator.SetChargingPoolAdminStatus(ChargingPoolId, StatusList);

        }

        #endregion


        #region SendChargingPoolAdminStatusDiff(StatusDiff)

        internal void SendChargingPoolAdminStatusDiff(ChargingPoolAdminStatusDiff StatusDiff)
        {

            var OnChargingPoolAdminDiffLocal = OnChargingPoolAdminDiff;
            if (OnChargingPoolAdminDiffLocal != null)
                OnChargingPoolAdminDiffLocal(StatusDiff);

        }

        #endregion


        #region OnChargingPoolDataChanged

        /// <summary>
        /// A delegate called whenever the static data of any subordinated charging pool changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        public delegate void OnChargingPoolDataChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, String PropertyName, Object OldValue, Object NewValue);

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolDataChangedDelegate OnChargingPoolDataChanged;

        #endregion

        #region OnAggregatedChargingPoolStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old timestamped status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
        public delegate void OnAggregatedChargingPoolStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<ChargingPoolStatusType> OldStatus, Timestamped<ChargingPoolStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnAggregatedChargingPoolStatusChangedDelegate OnAggregatedChargingPoolStatusChanged;

        #endregion

        #region OnAggregatedChargingPoolAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated admin status of any subordinated charging pool changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old timestamped status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
        public delegate void OnAggregatedChargingPoolAdminStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<ChargingPoolAdminStatusType> OldStatus, Timestamped<ChargingPoolAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated charging pool changed.
        /// </summary>
        public event OnAggregatedChargingPoolAdminStatusChangedDelegate OnAggregatedChargingPoolAdminStatusChanged;

        #endregion

        #region OnChargingPoolAdminDiff

        public delegate void OnChargingPoolAdminDiffDelegate(ChargingPoolAdminStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a charging station admin status diff was received.
        /// </summary>
        public event OnChargingPoolAdminDiffDelegate OnChargingPoolAdminDiff;

        #endregion

        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationAddition
        {
            get
            {
                return ChargingStationAddition;
            }
        }

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationRemoval
        {
            get
            {
                return ChargingStationRemoval;
            }
        }

        #endregion


        #region (internal) UpdateChargingPoolData(Timestamp, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateChargingPoolData(DateTime      Timestamp,
                                             ChargingPool  ChargingPool,
                                             String        PropertyName,
                                             Object        OldValue,
                                             Object        NewValue)
        {

            var OnChargingPoolDataChangedLocal = OnChargingPoolDataChanged;
            if (OnChargingPoolDataChangedLocal != null)
                OnChargingPoolDataChangedLocal(Timestamp, ChargingPool, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateChargingPoolStatus(Timestamp, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
        internal void UpdateChargingPoolStatus(DateTime                             Timestamp,
                                               ChargingPool                         ChargingPool,
                                               Timestamped<ChargingPoolStatusType>  OldStatus,
                                               Timestamped<ChargingPoolStatusType>  NewStatus)
        {

            var OnAggregatedChargingPoolStatusChangedLocal = OnAggregatedChargingPoolStatusChanged;
            if (OnAggregatedChargingPoolStatusChangedLocal != null)
                OnAggregatedChargingPoolStatusChangedLocal(Timestamp, ChargingPool, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingPoolAdminStatus(Timestamp, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
        internal void UpdateChargingPoolAdminStatus(DateTime                                  Timestamp,
                                                    ChargingPool                              ChargingPool,
                                                    Timestamped<ChargingPoolAdminStatusType>  OldStatus,
                                                    Timestamped<ChargingPoolAdminStatusType>  NewStatus)
        {

            var OnAggregatedChargingPoolAdminStatusChangedLocal = OnAggregatedChargingPoolAdminStatusChanged;
            if (OnAggregatedChargingPoolAdminStatusChangedLocal != null)
                OnAggregatedChargingPoolAdminStatusChangedLocal(Timestamp, ChargingPool, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region ChargingStations...

        #region ChargingStations

        /// <summary>
        /// Return all charging stations registered within this roaming network.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations
        {
            get
            {
                return _EVSEOperators.SelectMany(evseoperator => evseoperator.Value.SelectMany(pool => pool));
            }
        }

        #endregion

        #region ChargingStationStatus

        /// <summary>
        /// Return the status of all charging stations registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusType>>>> ChargingStationStatus
        {
            get
            {

                return _EVSEOperators.Values.
                           SelectMany(evseoperator =>
                               evseoperator.SelectMany(pool =>
                                   pool.Select(station =>

                                           new KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusType>>>(
                                               station.Id,
                                               station.StatusSchedule)

                                       )));

            }
        }

        #endregion


        #region ContainsChargingStation(ChargingStation)

        /// <summary>
        /// Check if the given charging station is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation ChargingStation)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingStation.ChargingPool.Operator.Id, out _EVSEOperator))
                return _EVSEOperator.ContainsChargingStation(ChargingStation.Id);

            return false;

        }

        #endregion

        #region ContainsChargingStation(ChargingStationId)

        /// <summary>
        /// Check if the given charging station identification is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingStation_Id.Parse(ChargingStationId.ToString()).OperatorId, out _EVSEOperator))
                return _EVSEOperator.ContainsChargingStation(ChargingStationId);

            return false;

        }

        #endregion

        #region GetChargingStationbyId(ChargingStationId)

        public ChargingStation GetChargingStationbyId(ChargingStation_Id ChargingStationId)
        {

            ChargingStation _ChargingStation  = null;
            EVSEOperator    _EVSEOperator     = null;

            if (TryGetEVSEOperatorbyId(ChargingStation_Id.Parse(ChargingStationId.ToString()).OperatorId, out _EVSEOperator))
                if (_EVSEOperator.TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
                    return _ChargingStation;

            return null;

        }

        #endregion

        #region TryGetChargingStationbyId(ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationbyId(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingStation_Id.Parse(ChargingStationId.ToString()).OperatorId, out _EVSEOperator))
                return _EVSEOperator.TryGetChargingStationbyId(ChargingStationId, out ChargingStation);

            ChargingStation = null;
            return false;

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, StatusList)

        public void SetChargingStationAdminStatus(ChargingStation_Id                                        ChargingStationId,
                                                  IEnumerable<Timestamped<ChargingStationAdminStatusType>>  StatusList)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingStation_Id.Parse(ChargingStationId.ToString()).OperatorId, out _EVSEOperator))
                _EVSEOperator.SetChargingStationAdminStatus(ChargingStationId, StatusList);

        }

        #endregion


        #region SendChargingStationAdminStatusDiff(StatusDiff)

        internal void SendChargingStationAdminStatusDiff(ChargingStationAdminStatusDiff StatusDiff)
        {

            var OnChargingStationAdminDiffLocal = OnChargingStationAdminDiff;
            if (OnChargingStationAdminDiffLocal != null)
                OnChargingStationAdminDiffLocal(StatusDiff);

        }

        #endregion


        #region OnChargingStationDataChanged

        /// <summary>
        /// A delegate called whenever the static data of any subordinated charging station changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        public delegate void OnChargingStationDataChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, String PropertyName, Object OldValue, Object NewValue);

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationDataChangedDelegate OnChargingStationDataChanged;

        #endregion

        #region OnChargingStationAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the admin status of any subordinated charging station changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old timestamped admin status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging station.</param>
        public delegate void OnChargingStationAdminStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<ChargingStationAdminStatusType> OldStatus, Timestamped<ChargingStationAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the admin status of any subordinated ChargingStation changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate OnChargingStationAdminStatusChanged;

        #endregion

        #region OnChargingStationAdminDiff

        public delegate void OnChargingStationAdminDiffDelegate(ChargingStationAdminStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a charging station admin status diff was received.
        /// </summary>
        public event OnChargingStationAdminDiffDelegate OnChargingStationAdminDiff;

        #endregion

        #region OnAggregatedChargingStationStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old timestamped status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station.</param>
        public delegate void OnAggregatedChargingStationStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<ChargingStationStatusType> OldStatus, Timestamped<ChargingStationStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        public event OnAggregatedChargingStationStatusChangedDelegate OnAggregatedChargingStationStatusChanged;

        #endregion

        #region OnAggregatedChargingStationAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated admin status of any subordinated charging station changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old timestamped status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station.</param>
        public delegate void OnAggregatedChargingStationAdminStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<ChargingStationAdminStatusType> OldStatus, Timestamped<ChargingStationAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated charging station changed.
        /// </summary>
        public event OnAggregatedChargingStationAdminStatusChangedDelegate OnAggregatedChargingStationAdminStatusChanged;

        #endregion

        #region EVSEAddition

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSEAddition
        {
            get
            {
                return EVSEAddition;
            }
        }

        #endregion

        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSERemoval
        {
            get
            {
                return EVSERemoval;
            }
        }

        #endregion


        #region (internal) UpdateChargingStationData(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateChargingStationData(DateTime         Timestamp,
                                                ChargingStation  ChargingStation,
                                                String           PropertyName,
                                                Object           OldValue,
                                                Object           NewValue)
        {

            var OnChargingStationDataChangedLocal = OnChargingStationDataChanged;
            if (OnChargingStationDataChangedLocal != null)
                OnChargingStationDataChangedLocal(Timestamp, ChargingStation, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal void UpdateChargingStationAdminStatus(DateTime                                     Timestamp,
                                                       ChargingStation                              ChargingStation,
                                                       Timestamped<ChargingStationAdminStatusType>  OldStatus,
                                                       Timestamped<ChargingStationAdminStatusType>  NewStatus)
        {

            var OnChargingStationAdminStatusChangedLocal = OnChargingStationAdminStatusChanged;
            if (OnChargingStationAdminStatusChangedLocal != null)
                OnChargingStationAdminStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateAggregatedChargingStationStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current aggregated charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal void UpdateAggregatedChargingStationStatus(DateTime                                Timestamp,
                                                            ChargingStation                         ChargingStation,
                                                            Timestamped<ChargingStationStatusType>  OldStatus,
                                                            Timestamped<ChargingStationStatusType>  NewStatus)
        {

            var OnAggregatedChargingStationStatusChangedLocal = OnAggregatedChargingStationStatusChanged;
            if (OnAggregatedChargingStationStatusChangedLocal != null)
                OnAggregatedChargingStationStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateAggregatedChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current aggregated charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station admin status.</param>
        /// <param name="NewStatus">The new aggreagted charging station admin status.</param>
        internal void UpdateAggregatedChargingStationAdminStatus(DateTime                                     Timestamp,
                                                                 ChargingStation                              ChargingStation,
                                                                 Timestamped<ChargingStationAdminStatusType>  OldStatus,
                                                                 Timestamped<ChargingStationAdminStatusType>  NewStatus)
        {

            var OnAggregatedChargingStationAdminStatusChangedLocal = OnAggregatedChargingStationAdminStatusChanged;
            if (OnAggregatedChargingStationAdminStatusChangedLocal != null)
                OnAggregatedChargingStationAdminStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region EVSE methods

        #region EVSEs

        /// <summary>
        /// Return all EVSEs registered within this roaming network.
        /// </summary>
        public IEnumerable<EVSE> EVSEs
        {
            get
            {
                return _EVSEOperators.SelectMany(evseoperator => evseoperator.Value.SelectMany(pool => pool.SelectMany(station => station)));
            }
        }

        #endregion

        #region EVSEStatus

        /// <summary>
        /// Return the status of all EVSEs registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEStatusType>>>> EVSEStatus
        {
            get
            {

                return _EVSEOperators.Values.
                           SelectMany(evseoperator =>
                               evseoperator.SelectMany(pool =>
                                   pool.SelectMany(station =>
                                       station.Select(evse =>

                                           new KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEStatusType>>>(
                                               evse.Id,
                                               evse.StatusSchedule)

                                       ))));

            }
        }

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the roaming network.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(EVSE.Operator.Id, out _EVSEOperator))
                return _EVSEOperator.ContainsEVSE(EVSE.Id);

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

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(EVSE_Id.Parse(EVSEId.ToString()).OperatorId, out _EVSEOperator))
                return _EVSEOperator.ContainsEVSE(EVSEId);

            return false;

        }

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)
        {

            EVSE         _EVSE          = null;
            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(EVSE_Id.Parse(EVSEId.ToString()).OperatorId, out _EVSEOperator))
                if (_EVSEOperator.TryGetEVSEbyId(EVSEId, out _EVSE))
                    return _EVSE;

            return null;

        }

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(EVSE_Id.Parse(EVSEId.ToString()).OperatorId, out _EVSEOperator))
                return _EVSEOperator.TryGetEVSEbyId(EVSEId, out EVSE);

            EVSE = null;
            return false;

        }

        #endregion


        #region SetEVSEStatus(EVSEId, NewStatus)

        public void SetEVSEStatus(EVSE_Id                      EVSEId,
                                  Timestamped<EVSEStatusType>  NewStatus)
        {

            EVSEOperator _EVSEOperator = null;

            if (TryGetEVSEOperatorbyId(EVSEId.OperatorId, out _EVSEOperator))
                _EVSEOperator.SetEVSEStatus(EVSEId, NewStatus);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, Timestamp, NewStatus)

        public void SetEVSEStatus(EVSE_Id         EVSEId,
                                  DateTime        Timestamp,
                                  EVSEStatusType  NewStatus)
        {

            EVSEOperator _EVSEOperator = null;

            if (TryGetEVSEOperatorbyId(EVSEId.OperatorId, out _EVSEOperator))
                _EVSEOperator.SetEVSEStatus(EVSEId, NewStatus);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, StatusList)

        public void SetEVSEStatus(EVSE_Id                                   EVSEId,
                                  IEnumerable<Timestamped<EVSEStatusType>>  StatusList,
                                  ChangeMethods                             ChangeMethod  = ChangeMethods.Replace)
        {

            EVSEOperator _EVSEOperator = null;

            if (TryGetEVSEOperatorbyId(EVSEId.OperatorId, out _EVSEOperator))
                _EVSEOperator.SetEVSEStatus(EVSEId, StatusList, ChangeMethod);

        }

        #endregion


        #region SetEVSEAdminStatus(EVSEId, NewStatus)

        public void SetEVSEAdminStatus(EVSE_Id                           EVSEId,
                                       Timestamped<EVSEAdminStatusType>  NewAdminStatus)
        {

            EVSEOperator _EVSEOperator = null;

            if (TryGetEVSEOperatorbyId(EVSEId.OperatorId, out _EVSEOperator))
                _EVSEOperator.SetEVSEAdminStatus(EVSEId, NewAdminStatus);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, Timestamp, NewAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id              EVSEId,
                                       DateTime             Timestamp,
                                       EVSEAdminStatusType  NewAdminStatus)
        {

            EVSEOperator _EVSEOperator = null;

            if (TryGetEVSEOperatorbyId(EVSEId.OperatorId, out _EVSEOperator))
                _EVSEOperator.SetEVSEAdminStatus(EVSEId, NewAdminStatus);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, AdminStatusList)

        public void SetEVSEAdminStatus(EVSE_Id                                        EVSEId,
                                       IEnumerable<Timestamped<EVSEAdminStatusType>>  AdminStatusList,
                                       ChangeMethods                                  ChangeMethod  = ChangeMethods.Replace)
        {

            EVSEOperator _EVSEOperator = null;

            if (TryGetEVSEOperatorbyId(EVSEId.OperatorId, out _EVSEOperator))
                _EVSEOperator.SetEVSEAdminStatus(EVSEId, AdminStatusList, ChangeMethod);

        }

        #endregion


        #region SendEVSEStatusDiff(StatusDiff)

        internal void SendEVSEStatusDiff(EVSEStatusDiff StatusDiff)
        {

            var OnEVSEStatusDiffLocal = OnEVSEStatusDiff;
            if (OnEVSEStatusDiffLocal != null)
                OnEVSEStatusDiffLocal(StatusDiff);

        }

        #endregion


        #region OnEVSEDataChanged

        /// <summary>
        /// A delegate called whenever the static data of any subordinated EVSE changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        public delegate void OnEVSEDataChangedDelegate(DateTime Timestamp, EVSE EVSE, String PropertyName, Object OldValue, Object NewValue);

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate OnEVSEDataChanged;

        #endregion

        #region OnEVSEStatusChanged

        /// <summary>
        /// A delegate called whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old timestamped status of the EVSE.</param>
        /// <param name="NewStatus">The new timestamped status of the EVSE.</param>
        public delegate void OnEVSEStatusChangedDelegate(DateTime Timestamp, EVSE EVSE, Timestamped<EVSEStatusType> OldStatus, Timestamped<EVSEStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate OnEVSEStatusChanged;

        #endregion

        #region OnEVSEStatusDiff

        public delegate void OnEVSEStatusDiffDelegate(EVSEStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a EVSE status diff was received.
        /// </summary>
        public event OnEVSEStatusDiffDelegate OnEVSEStatusDiff;

        #endregion

        #region OnEVSEAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old timestamped admin status of the EVSE.</param>
        /// <param name="NewStatus">The new timestamped admin status of the EVSE.</param>
        public delegate void OnEVSEAdminStatusChangedDelegate(DateTime Timestamp, EVSE EVSE, Timestamped<EVSEAdminStatusType> OldStatus, Timestamped<EVSEAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate OnEVSEAdminStatusChanged;

        #endregion

        #region SocketOutletAddition

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletAddition
        {
            get
            {
                return SocketOutletAddition;
            }
        }

        #endregion

        #region SocketOutletRemoval

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletRemoval;

        /// <summary>
        /// Called whenever a socket outlet will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletRemoval
        {
            get
            {
                return SocketOutletRemoval;
            }
        }

        #endregion


        #region (internal) UpdateEVSEData(Timestamp, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateEVSEData(DateTime  Timestamp,
                                     EVSE      EVSE,
                                     String    PropertyName,
                                     Object    OldValue,
                                     Object    NewValue)
        {

            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal != null)
                OnEVSEDataChangedLocal(Timestamp, EVSE, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateEVSEStatus(Timestamp, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update an EVSE status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                     Timestamp,
                                             EVSE                         EVSE,
                                             Timestamped<EVSEStatusType>  OldStatus,
                                             Timestamped<EVSEStatusType>  NewStatus)
        {

            #region Send log event


            #endregion

            Acknowledgement result = null;

            if (!DisableStatusUpdates)
            { 

                foreach (var AuthenticationService in _IeMobilityServiceProviders.
                                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
                {

                    result = await AuthenticationService.PushEVSEStatus(EVSE,
                                                                        ActionType.update,
                                                                        EVSE.Operator.Id);

                }

                foreach (var OperatorRoamingService in _OperatorRoamingServices.
                                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
                {

                    result = await OperatorRoamingService.PushEVSEStatus(EVSE,
                                                                         ActionType.update,
                                                                         EVSE.Operator.Id);

                }

                foreach (var PushEVSEStatusService in _PushEVSEStatusToOperatorRoamingServices.
                                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
                {

                    result = await PushEVSEStatusService.PushEVSEStatus(EVSE,
                                                                        ActionType.update,
                                                                        EVSE.Operator.Id);

                }

            }

            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal != null)
                OnEVSEStatusChangedLocal(Timestamp, EVSE, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update an EVSE admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal void UpdateEVSEAdminStatus(DateTime                          Timestamp,
                                            EVSE                              EVSE,
                                            Timestamped<EVSEAdminStatusType>  OldStatus,
                                            Timestamped<EVSEAdminStatusType>  NewStatus)
        {

            var OnEVSEAdminStatusChangedLocal = OnEVSEAdminStatusChanged;
            if (OnEVSEAdminStatusChangedLocal != null)
                OnEVSEAdminStatusChangedLocal(Timestamp, EVSE, OldStatus, NewStatus);

        }

        #endregion

        #endregion


        #region Reservations...

        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(15);

        #region ChargingReservations

        private readonly ConcurrentDictionary<ChargingReservation_Id, EVSEOperator> _ChargingReservations;

        /// <summary>
        /// Return all current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
        {
            get
            {
                return _ChargingReservations.SelectMany(kvp => kvp.Value.ChargingReservations);
            }
        }

        #endregion

        #region OnReserve... / OnReserved... / OnNewReservation

        /// <summary>
        /// An event fired whenever an EVSE is being reserved.
        /// </summary>
        public event OnReserveEVSEDelegate              OnReserveEVSE;

        /// <summary>
        /// An event fired whenever an EVSE was reserved.
        /// </summary>
        public event OnEVSEReservedDelegate             OnEVSEReserved;

        /// <summary>
        /// An event fired whenever a charging station is being reserved.
        /// </summary>
        public event OnReserveChargingStationDelegate   OnReserveChargingStation;

        /// <summary>
        /// An event fired whenever a charging station was reserved.
        /// </summary>
        public event OnChargingStationReservedDelegate  OnChargingStationReserved;

        /// <summary>
        /// An event fired whenever a charging pool is being reserved.
        /// </summary>
        public event OnReserveChargingPoolDelegate      OnReserveChargingPool;

        /// <summary>
        /// An event fired whenever a charging pool was reserved.
        /// </summary>
        public event OnChargingPoolReservedDelegate     OnChargingPoolReserved;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate           OnNewReservation;

        #endregion

        #region Reserve(...EVSEId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult> Reserve(DateTime                 Timestamp,
                                                     CancellationToken        CancellationToken,
                                                     EventTracking_Id         EventTrackingId,
                                                     EVSE_Id                  EVSEId,
                                                     DateTime?                StartTime,
                                                     TimeSpan?                Duration,
                                                     ChargingReservation_Id   ReservationId      = null,
                                                     EVSP_Id                  ProviderId         = null,
                                                     ChargingProduct_Id       ChargingProductId  = null,
                                                     IEnumerable<Auth_Token>  AuthTokens         = null,
                                                     IEnumerable<eMA_Id>      eMAIds             = null,
                                                     IEnumerable<UInt32>      PINs               = null,
                                                     TimeSpan?                QueryTimeout       = null)
        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),  "The given EVSE identification must not be null!");

            EVSEOperator      EVSEOperator  = null;
            ReservationResult result        = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveEVSE event

            try
            {

                var OnReserveEVSELocal = OnReserveEVSE;
                if (OnReserveEVSELocal != null)
                    OnReserveEVSELocal(this,
                                       Timestamp,
                                       EventTrackingId,
                                       Id,
                                       ReservationId,
                                       EVSEId,
                                       StartTime,
                                       Duration,
                                       ProviderId,
                                       ChargingProductId,
                                       AuthTokens,
                                       eMAIds,
                                       PINs);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnReserveEVSE));
            }

            #endregion


            if (TryGetEVSEOperatorbyId(EVSEId.OperatorId, out EVSEOperator))
            {

                result = await EVSEOperator.Reserve(Timestamp,
                                                    CancellationToken,
                                                    EventTrackingId,
                                                    EVSEId,
                                                    StartTime,
                                                    Duration,
                                                    ReservationId,
                                                    ProviderId,
                                                    ChargingProductId,
                                                    AuthTokens,
                                                    eMAIds,
                                                    PINs,
                                                    QueryTimeout);

                if (result.Result == ReservationResultType.Success)
                    _ChargingReservations.TryAdd(result.Reservation.Id, EVSEOperator);

            }

            else
                result = ReservationResult.UnknownEVSEOperator;


            #region Send OnEVSEReserved event

            try
            {

                var OnEVSEReservedLocal = OnEVSEReserved;
                if (OnEVSEReservedLocal != null)
                    OnEVSEReservedLocal(this,
                                        Timestamp,
                                        EventTrackingId,
                                        Id,
                                        ReservationId,
                                        EVSEId,
                                        StartTime,
                                        Duration,
                                        ProviderId,
                                        ChargingProductId,
                                        AuthTokens,
                                        eMAIds,
                                        PINs,
                                        result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnEVSEReserved));
            }

            #endregion

            return result;

        }

        #endregion

        #region Reserve(...ChargingStationId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStationId">The unique identification of the charging station to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult> Reserve(DateTime                 Timestamp,
                                                     CancellationToken        CancellationToken,
                                                     EventTracking_Id         EventTrackingId,
                                                     ChargingStation_Id       ChargingStationId,
                                                     DateTime?                StartTime,
                                                     TimeSpan?                Duration,
                                                     ChargingReservation_Id   ReservationId      = null,
                                                     EVSP_Id                  ProviderId         = null,
                                                     ChargingProduct_Id       ChargingProductId  = null,
                                                     IEnumerable<Auth_Token>  AuthTokens         = null,
                                                     IEnumerable<eMA_Id>      eMAIds             = null,
                                                     IEnumerable<UInt32>      PINs               = null,
                                                     TimeSpan?                QueryTimeout       = null)
        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            EVSEOperator      EVSEOperator  = null;
            ReservationResult result        = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveChargingStation event

            try
            {

                var OnReserveChargingStationLocal = OnReserveChargingStation;
                if (OnReserveChargingStationLocal != null)
                    OnReserveChargingStationLocal(this,
                                                  Timestamp,
                                                  EventTrackingId,
                                                  Id,
                                                  ChargingStationId,
                                                  StartTime,
                                                  Duration,
                                                  ReservationId,
                                                  ProviderId,
                                                  ChargingProductId,
                                                  AuthTokens,
                                                  eMAIds,
                                                  PINs);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnReserveChargingStation));
            }

            #endregion


            if (TryGetEVSEOperatorbyId(ChargingStationId.OperatorId, out EVSEOperator))
            {

                result = await EVSEOperator.Reserve(Timestamp,
                                                    CancellationToken,
                                                    EventTrackingId,
                                                    ChargingStationId,
                                                    StartTime,
                                                    Duration,
                                                    ReservationId,
                                                    ProviderId,
                                                    ChargingProductId,
                                                    AuthTokens,
                                                    eMAIds,
                                                    PINs,
                                                    QueryTimeout);

                if (result.Result == ReservationResultType.Success)
                    _ChargingReservations.TryAdd(result.Reservation.Id, EVSEOperator);

            }

            else
                result = ReservationResult.UnknownEVSEOperator;


            #region Send OnChargingStationReserved event

            try
            {

                var OnChargingStationReservedLocal = OnChargingStationReserved;
                if (OnChargingStationReservedLocal != null)
                    OnChargingStationReservedLocal(this,
                                                   Timestamp,
                                                   EventTrackingId,
                                                   Id,
                                                   ChargingStationId,
                                                   StartTime,
                                                   Duration,
                                                   ReservationId,
                                                   ProviderId,
                                                   ChargingProductId,
                                                   AuthTokens,
                                                   eMAIds,
                                                   PINs,
                                                   result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnChargingStationReserved));
            }

            #endregion

            return result;

        }

        #endregion

        #region Reserve(...ChargingPoolId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge within the given charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPoolId">The unique identification of the charging pool to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult> Reserve(DateTime                 Timestamp,
                                                     CancellationToken        CancellationToken,
                                                     EventTracking_Id         EventTrackingId,
                                                     ChargingPool_Id          ChargingPoolId,
                                                     DateTime?                StartTime,
                                                     TimeSpan?                Duration,
                                                     ChargingReservation_Id   ReservationId      = null,
                                                     EVSP_Id                  ProviderId         = null,
                                                     ChargingProduct_Id       ChargingProductId  = null,
                                                     IEnumerable<Auth_Token>  AuthTokens         = null,
                                                     IEnumerable<eMA_Id>      eMAIds             = null,
                                                     IEnumerable<UInt32>      PINs               = null,
                                                     TimeSpan?                QueryTimeout       = null)
        {

            #region Initial checks

            if (ChargingPoolId == null)
                throw new ArgumentNullException(nameof(ChargingPoolId),  "The given charging pool identification must not be null!");

            EVSEOperator      EVSEOperator  = null;
            ReservationResult result        = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveChargingPool event

            try
            {

                var OnReserveChargingPoolLocal = OnReserveChargingPool;
                if (OnReserveChargingPoolLocal != null)
                    OnReserveChargingPoolLocal(this,
                                               Timestamp,
                                               EventTrackingId,
                                               Id,
                                               ChargingPoolId,
                                               StartTime,
                                               Duration,
                                               ReservationId,
                                               ProviderId,
                                               ChargingProductId,
                                               AuthTokens,
                                               eMAIds,
                                               PINs);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnReserveChargingPool));
            }

            #endregion


            if (TryGetEVSEOperatorbyId(ChargingPoolId.OperatorId, out EVSEOperator))
            {

                result = await EVSEOperator.Reserve(Timestamp,
                                                    CancellationToken,
                                                    EventTrackingId,
                                                    ChargingPoolId,
                                                    StartTime,
                                                    Duration,
                                                    ReservationId,
                                                    ProviderId,
                                                    ChargingProductId,
                                                    AuthTokens,
                                                    eMAIds,
                                                    PINs,
                                                    QueryTimeout);

                if (result.Result == ReservationResultType.Success)
                    _ChargingReservations.TryAdd(result.Reservation.Id, EVSEOperator);

            }

            else
                result = ReservationResult.UnknownEVSEOperator;


            #region Send OnEVSEReserved event

            try
            {

                var OnChargingPoolReservedLocal = OnChargingPoolReserved;
                if (OnChargingPoolReservedLocal != null)
                    OnChargingPoolReservedLocal(this,
                                                Timestamp,
                                                EventTrackingId,
                                                Id,
                                                ChargingPoolId,
                                                StartTime,
                                                Duration,
                                                ReservationId,
                                                ProviderId,
                                                ChargingProductId,
                                                AuthTokens,
                                                eMAIds,
                                                PINs,
                                                result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnChargingPoolReserved));
            }

            #endregion

            return result;

        }

        #endregion

        #region (internal) SendNewReservation(Timestamp, Sender, Reservation)

        internal void SendNewReservation(DateTime             Timestamp,
                                         Object               Sender,
                                         ChargingReservation  Reservation)
        {

            var OnNewReservationLocal = OnNewReservation;
            if (OnNewReservationLocal != null)
                OnNewReservationLocal(Timestamp, Sender, Reservation);

        }

        #endregion


        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by its unique identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation identification.</param>
        /// <returns>True when successful, false otherwise.</returns>
        public Boolean TryGetReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
        {

            EVSEOperator _EVSEOperator = null;

            if (_ChargingReservations.TryGetValue(ReservationId, out _EVSEOperator))
                return _EVSEOperator.TryGetReservationById(ReservationId, out Reservation);

            Reservation = null;
            return false;

        }

        #endregion


        #region CancelReservation(ReservationId)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <returns>True when successful, false otherwise</returns>
        public async Task<Boolean> CancelReservation(ChargingReservation_Id           ReservationId,
                                                     ChargingReservationCancellation  ReservationCancellation)
        {

            EVSEOperator _EVSEOperator = null;

            if (_ChargingReservations.TryRemove(ReservationId, out _EVSEOperator))
                return await _EVSEOperator.CancelReservation(ReservationId, ReservationCancellation);

            return false;

        }

        #endregion

        #region OnReservationExpired

        /// <summary>
        /// A delegate called whenever a reservation expired.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this reservation expired.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public delegate void OnReservationExpiredDelegate(DateTime Timestamp, ChargingReservation Reservation);

        /// <summary>
        /// An event fired whenever a reservation expired.
        /// </summary>
        public event OnReservationExpiredDelegate OnReservationExpired;

        #endregion

        #region OnReservationDeleted

        /// <summary>
        /// A delegate called whenever a reservation was deleted.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this reservation was deleted.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public delegate void OnReservationDeletedDelegate(DateTime Timestamp, ChargingReservation Reservation);

        /// <summary>
        /// An event fired whenever a reservation was deleted.
        /// </summary>
        public event OnReservationDeletedDelegate OnReservationDeleted;

        #endregion

        #endregion

        #region RemoteStart/-Stop

        #region OnRemote...Start / OnRemote...Started

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteEVSEStartDelegate               OnRemoteEVSEStart;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteEVSEStartedDelegate             OnRemoteEVSEStarted;

        /// <summary>
        /// An event fired whenever a remote start charging station command was received.
        /// </summary>
        public event OnRemoteChargingStationStartDelegate    OnRemoteChargingStationStart;

        /// <summary>
        /// An event fired whenever a remote start charging station command completed.
        /// </summary>
        public event OnRemoteChargingStationStartedDelegate  OnRemoteChargingStationStarted;

        #endregion

        #region RemoteStart(...EVSEId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartEVSEResult>

            RemoteStart(DateTime                Timestamp,
                        CancellationToken       CancellationToken,
                        EventTracking_Id        EventTrackingId,
                        EVSE_Id                 EVSEId,
                        ChargingProduct_Id      ChargingProductId  = null,
                        ChargingReservation_Id  ReservationId      = null,
                        ChargingSession_Id      SessionId          = null,
                        EVSP_Id                 ProviderId         = null,
                        eMA_Id                  eMAId              = null,
                        TimeSpan?               QueryTimeout       = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),  "The given EVSE identification must not be null!");

            EVSEOperator          _EVSEOperator  = null;
            RemoteStartEVSEResult result         = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStart event

            try
            {

                var OnRemoteEVSEStartLocal = OnRemoteEVSEStart;
                if (OnRemoteEVSEStartLocal != null)
                    OnRemoteEVSEStartLocal(Timestamp,
                                           this,
                                           EventTrackingId,
                                           Id,
                                           EVSEId,
                                           ChargingProductId,
                                           ReservationId,
                                           SessionId,
                                           ProviderId,
                                           eMAId,
                                           QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnRemoteEVSEStart));
            }

            #endregion


            if (TryGetEVSEOperatorbyId(EVSEId.OperatorId, out _EVSEOperator))
            {

                result = await _EVSEOperator.RemoteStart(Timestamp,
                                                        CancellationToken,
                                                        EventTrackingId,
                                                        EVSEId,
                                                        ChargingProductId,
                                                        ReservationId,
                                                        SessionId,
                                                        ProviderId,
                                                        eMAId,
                                                        QueryTimeout);


                if (result.Result == RemoteStartEVSEResultType.Success)
                    _ChargingSessions.TryAdd(result.Session.Id, result.Session);

            }

            else
                result = RemoteStartEVSEResult.UnknownEVSEOperator;


            #region Send OnRemoteEVSEStarted event

            try
            {

                var OnRemoteEVSEStartedLocal = OnRemoteEVSEStarted;
                if (OnRemoteEVSEStartedLocal != null)
                    OnRemoteEVSEStartedLocal(Timestamp,
                                             this,
                                             EventTrackingId,
                                             Id,
                                             EVSEId,
                                             ChargingProductId,
                                             ReservationId,
                                             SessionId,
                                             ProviderId,
                                             eMAId,
                                             QueryTimeout,
                                             result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnRemoteEVSEStarted));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStart(...ChargingStationId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStationId">The unique identification of the charging station to be started.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartChargingStationResult>

            RemoteStart(DateTime                Timestamp,
                        CancellationToken       CancellationToken,
                        EventTracking_Id        EventTrackingId,
                        ChargingStation_Id      ChargingStationId,
                        ChargingProduct_Id      ChargingProductId  = null,
                        ChargingReservation_Id  ReservationId      = null,
                        ChargingSession_Id      SessionId          = null,
                        EVSP_Id                 ProviderId         = null,
                        eMA_Id                  eMAId              = null,
                        TimeSpan?               QueryTimeout       = null)

        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            EVSEOperator                     _EVSEOperator  = null;
            RemoteStartChargingStationResult result         = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteChargingStationStart event

            try
            {

                var OnRemoteChargingStationStartLocal = OnRemoteChargingStationStart;
                if (OnRemoteChargingStationStartLocal != null)
                    OnRemoteChargingStationStartLocal(Timestamp,
                                                      this,
                                                      EventTrackingId,
                                                      Id,
                                                      ChargingStationId,
                                                      ChargingProductId,
                                                      ReservationId,
                                                      SessionId,
                                                      ProviderId,
                                                      eMAId,
                                                      QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnRemoteChargingStationStart));
            }

            #endregion


            if (TryGetEVSEOperatorbyId(ChargingStationId.OperatorId, out _EVSEOperator))
            {

                result = await _EVSEOperator.RemoteStart(Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         ChargingStationId,
                                                         ChargingProductId,
                                                         ReservationId,
                                                         SessionId,
                                                         ProviderId,
                                                         eMAId,
                                                         QueryTimeout);


                if (result.Result == RemoteStartChargingStationResultType.Success)
                    _ChargingSessions.TryAdd(result.Session.Id, result.Session);

            }

            else
                result = RemoteStartChargingStationResult.UnknownChargingStationOperator;


            #region Send OnRemoteChargingStationStarted event

            try
            {

                var OnRemoteChargingStationStartedLocal = OnRemoteChargingStationStarted;
                if (OnRemoteChargingStationStartedLocal != null)
                    OnRemoteChargingStationStartedLocal(Timestamp,
                                                        this,
                                                        EventTrackingId,
                                                        Id,
                                                        ChargingStationId,
                                                        ChargingProductId,
                                                        ReservationId,
                                                        SessionId,
                                                        ProviderId,
                                                        eMAId,
                                                        QueryTimeout,
                                                        result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnRemoteChargingStationStarted));
            }

            #endregion

            return result;

        }

        #endregion


        #region OnRemote...Stop / OnRemote...Stopped

        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopDelegate                    OnRemoteStop;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStoppedDelegate                 OnRemoteStopped;

        /// <summary>
        /// An event fired whenever a remote stop EVSE command was received.
        /// </summary>
        public event OnRemoteEVSEStopDelegate                OnRemoteEVSEStop;

        /// <summary>
        /// An event fired whenever a remote stop EVSE command completed.
        /// </summary>
        public event OnRemoteEVSEStoppedDelegate             OnRemoteEVSEStopped;

        /// <summary>
        /// An event fired whenever a remote stop charging station command was received.
        /// </summary>
        public event OnRemoteChargingStationStopDelegate     OnRemoteChargingStationStop;

        /// <summary>
        /// An event fired whenever a remote stop charging station command completed.
        /// </summary>
        public event OnRemoteChargingStationStoppedDelegate  OnRemoteChargingStationStopped;

        #endregion

        #region RemoteStop(...SessionId, ReservationHandling, ProviderId = null, QueryTimeout = null)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EventTracking_Id     EventTrackingId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId    = null,
                       TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId), "The given charging session identification must not be null!");

            RemoteStopResult result           = null;
            ChargingSession _ChargingSession  = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteStop event

            try
            {

                var OnRemoteStopLocal = OnRemoteStop;
                if (OnRemoteStopLocal != null)
                    OnRemoteStopLocal(this,
                                      Timestamp,
                                      EventTrackingId,
                                      Id,
                                      SessionId,
                                      ReservationHandling,
                                      ProviderId,
                                      QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnRemoteStop));
            }

            #endregion


            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession))
            {

                if (_ChargingSession.EVSEOperator != null)
                    result = await _ChargingSession.
                                       EVSEOperator.
                                       RemoteStop(Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  QueryTimeout);

                else
                    result = RemoteStopResult.UnknownOperator(SessionId);

            }

            else
                result = RemoteStopResult.InvalidSessionId(SessionId);


            #region Send OnRemoteStopped event

            try
            {

                var OnRemoteStoppedLocal = OnRemoteStopped;
                if (OnRemoteStoppedLocal != null)
                    OnRemoteStoppedLocal(this,
                                         Timestamp,
                                         EventTrackingId,
                                         Id,
                                         SessionId,
                                         ReservationHandling,
                                         ProviderId,
                                         QueryTimeout,
                                         result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnRemoteStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop(...EVSEId, SessionId, ReservationHandling, ProviderId = null, QueryTimeout = null)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopEVSEResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EventTracking_Id     EventTrackingId,
                       EVSE_Id              EVSEId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId    = null,
                       TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),     "The given EVSE identification must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given charging session identification must not be null!");

            RemoteStopEVSEResult result           = null;
            ChargingSession     _ChargingSession  = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStop event

            try
            {

                var OnRemoteEVSEStopLocal = OnRemoteEVSEStop;
                if (OnRemoteEVSEStopLocal != null)
                    OnRemoteEVSEStopLocal(this,
                                          Timestamp,
                                          EventTrackingId,
                                          Id,
                                          EVSEId,
                                          SessionId,
                                          ReservationHandling,
                                          ProviderId,
                                          QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnRemoteEVSEStop));
            }

            #endregion


            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession))
            {

                if (_ChargingSession.EVSEOperator != null)
                {

                    result = await _ChargingSession.
                                       EVSEOperator.
                                       RemoteStop(Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  EVSEId,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  QueryTimeout);


                    ChargingSession _ChargingSessionToRemove = null;

                    if (result.Result == RemoteStopEVSEResultType.Success)
                        _ChargingSessions.TryRemove(result.SessionId, out _ChargingSessionToRemove);

                }

                else
                    result = RemoteStopEVSEResult.UnknownOperator(SessionId);

            }

            else
                result = RemoteStopEVSEResult.InvalidSessionId(SessionId);


            #region Send OnRemoteEVSEStopped event

            try
            {

                var OnRemoteEVSEStoppedLocal = OnRemoteEVSEStopped;
                if (OnRemoteEVSEStoppedLocal != null)
                    OnRemoteEVSEStoppedLocal(this,
                                             Timestamp,
                                             EventTrackingId,
                                             Id,
                                             EVSEId,
                                             SessionId,
                                             ReservationHandling,
                                             ProviderId,
                                             QueryTimeout,
                                             result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnRemoteEVSEStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop(...ChargingStationId, SessionId, ReservationHandling, ProviderId = null, QueryTimeout = null)

        /// <summary>
        /// Stop the given charging session at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStationId">The unique identification of the charging station to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopChargingStationResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EventTracking_Id     EventTrackingId,
                       ChargingStation_Id   ChargingStationId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId    = null,
                       TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),          "The given charging session identification must not be null!");

            RemoteStopChargingStationResult result           = null;
            ChargingSession                _ChargingSession  = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteChargingStationStop event

            try
            {

                var OnRemoteChargingStationStopLocal = OnRemoteChargingStationStop;
                if (OnRemoteChargingStationStopLocal != null)
                    OnRemoteChargingStationStopLocal(this,
                                                     Timestamp,
                                                     EventTrackingId,
                                                     Id,
                                                     ChargingStationId,
                                                     SessionId,
                                                     ReservationHandling,
                                                     ProviderId,
                                                     QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnRemoteChargingStationStop));
            }

            #endregion


            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession))
            {

                if (_ChargingSession.EVSEOperator != null)
                    result = await _ChargingSession.
                                       EVSEOperator.
                                       RemoteStop(Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  ChargingStationId,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  QueryTimeout);

                else
                    result = RemoteStopChargingStationResult.UnknownOperator(SessionId);

            }

            else
                result = RemoteStopChargingStationResult.InvalidSessionId(SessionId);


            #region Send OnRemoteChargingStationStopped event

            try
            {

                var OnRemoteChargingStationStoppedLocal = OnRemoteChargingStationStopped;
                if (OnRemoteChargingStationStoppedLocal != null)
                    OnRemoteChargingStationStoppedLocal(this,
                                                        Timestamp,
                                                        EventTrackingId,
                                                        Id,
                                                        ChargingStationId,
                                                        SessionId,
                                                        ReservationHandling,
                                                        ProviderId,
                                                        QueryTimeout,
                                                        result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnRemoteChargingStationStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region AuthorizeStart/-Stop

        #region AuthorizeStart(...OperatorId, AuthToken, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(DateTime            Timestamp,
                           CancellationToken   CancellationToken,
                           EventTracking_Id    EventTrackingId,
                           EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           ChargingProduct_Id  ChargingProductId  = null,
                           ChargingSession_Id  SessionId          = null,
                           TimeSpan?           QueryTimeout       = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given EVSE operator must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given authentication token must not be null!");

            #endregion

            #region Send OnAuthorizeStart event

            try
            {

                var OnAuthorizeStartLocal = OnAuthorizeStart;
                if (OnAuthorizeStartLocal != null)
                    OnAuthorizeStartLocal(Timestamp,
                                          this,
                                          EventTrackingId,
                                          Id,
                                          OperatorId,
                                          AuthToken,
                                          ChargingProductId,
                                          SessionId,
                                          QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeStart));
            }

            #endregion


            AuthStartResult result = null;

            foreach (var AuthenticationService in _IeMobilityServiceProviders.
                                                      OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                      Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            {

                result = await AuthenticationService.AuthorizeStart(Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    OperatorId,
                                                                    AuthToken,
                                                                    ChargingProductId,
                                                                    SessionId,
                                                                    QueryTimeout);


                #region Authorized

                if (result.AuthorizationResult == AuthStartResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    _ChargingSessions.TryAdd(result.SessionId,
                                             new ChargingSession(result.SessionId) {
                                                 AuthService        = AuthenticationService,
                                                 EVSEOperatorId     = OperatorId,
                                                 AuthToken          = AuthToken,
                                                 ChargingProductId  = ChargingProductId
                                             });

                    break;

                }

                #endregion

                #region Blocked

                else if (result.AuthorizationResult == AuthStartResultType.Blocked)
                    break;

                #endregion

            }

            foreach (var OperatorRoamingService in _OperatorRoamingServices.
                                                      OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                      Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            {

                result = await OperatorRoamingService.AuthorizeStart(Timestamp,
                                                                     CancellationToken,
                                                                     EventTrackingId,
                                                                     OperatorId,
                                                                     AuthToken,
                                                                     ChargingProductId,
                                                                     SessionId,
                                                                     QueryTimeout);


                #region Authorized

                if (result.AuthorizationResult == AuthStartResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    _ChargingSessions.TryAdd(result.SessionId,
                                             new ChargingSession(result.SessionId) {
                                                 OperatorRoamingService  = OperatorRoamingService,
                                                 EVSEOperatorId          = OperatorId,
                                                 AuthToken               = AuthToken,
                                                 ChargingProductId       = ChargingProductId
                                             });

                    break;

                }

                #endregion

                #region Blocked

                else if (result.AuthorizationResult == AuthStartResultType.Blocked)
                    break;

                #endregion

            }

            #region ...or fail!

            if (result == null)
                result =  AuthStartResult.Error(AuthorizatorId,
                                                "No authorization service returned a positiv result!");

            #endregion


            #region Send OnAuthorizeStarted event

            try
            {

                var OnAuthorizeStartedLocal = OnAuthorizeStarted;
                if (OnAuthorizeStartedLocal != null)
                    OnAuthorizeStartedLocal(Timestamp,
                                            this,
                                            EventTrackingId,
                                            Id,
                                            OperatorId,
                                            AuthToken,
                                            ChargingProductId,
                                            SessionId,
                                            QueryTimeout,
                                            result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeStarted));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(...OperatorId, AuthToken, EVSEId, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(DateTime            Timestamp,
                           CancellationToken   CancellationToken,
                           EventTracking_Id    EventTrackingId,
                           EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId,
                           ChargingProduct_Id  ChargingProductId  = null,
                           ChargingSession_Id  SessionId          = null,
                           TimeSpan?           QueryTimeout       = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given EVSE operator must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given authentication token must not be null!");

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),     "The given EVSE identification must not be null!");

            #endregion

            #region Send OnAuthorizeEVSEStart event

            try
            {

                var OnAuthorizeEVSEStartLocal = OnAuthorizeEVSEStart;
                if (OnAuthorizeEVSEStartLocal != null)
                    OnAuthorizeEVSEStartLocal(Timestamp,
                                              this,
                                              EventTrackingId,
                                              Id,
                                              OperatorId,
                                              AuthToken,
                                              EVSEId,
                                              ChargingProductId,
                                              SessionId,
                                              QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeEVSEStart));
            }

            #endregion


            AuthStartEVSEResult result = null;

            foreach (var AuthenticationService in _IeMobilityServiceProviders.
                                                      OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                      Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            {

                result = await AuthenticationService.AuthorizeStart(Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    OperatorId,
                                                                    AuthToken,
                                                                    EVSEId,
                                                                    ChargingProductId,
                                                                    SessionId,
                                                                    QueryTimeout);


                #region Authorized

                if (result.Result == AuthStartEVSEResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    _ChargingSessions.TryAdd(result.SessionId,
                                             new ChargingSession(result.SessionId) {
                                                 AuthService        = AuthenticationService,
                                                 EVSEOperatorId     = OperatorId,
                                                 EVSEId             = EVSEId,
                                                 AuthToken          = AuthToken,
                                                 ChargingProductId  = ChargingProductId
                                             });

                    break;

                }

                #endregion

                #region Blocked

                else if (result.Result == AuthStartEVSEResultType.Blocked)
                    break;

                #endregion

            }

            foreach (var OperatorRoamingService in _OperatorRoamingServices.
                                                       OrderBy(OperatorRoamingServiceWithPriority => OperatorRoamingServiceWithPriority.Key).
                                                       Select (OperatorRoamingServiceWithPriority => OperatorRoamingServiceWithPriority.Value))
            {

                result = await OperatorRoamingService.AuthorizeStart(Timestamp,
                                                                     CancellationToken,
                                                                     EventTrackingId,
                                                                     OperatorId,
                                                                     AuthToken,
                                                                     EVSEId,
                                                                     ChargingProductId,
                                                                     SessionId,
                                                                     QueryTimeout);


                #region Authorized

                if (result.Result == AuthStartEVSEResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    _ChargingSessions.TryAdd(result.SessionId,
                                             new ChargingSession(result.SessionId) {
                                                 OperatorRoamingService  = OperatorRoamingService,
                                                 EVSEOperatorId          = OperatorId,
                                                 EVSEId                  = EVSEId,
                                                 AuthToken               = AuthToken,
                                                 ChargingProductId       = ChargingProductId
                                             });

                    break;

                }

                #endregion

                #region Blocked

                else if (result.Result == AuthStartEVSEResultType.Blocked)
                    break;

                #endregion

            }

            #region ...or fail!

            if (result == null)
                result =  AuthStartEVSEResult.Error(AuthorizatorId,
                                                    "No authorization service returned a positiv result!");

            #endregion


            #region Send OnAuthorizeEVSEStarted event

            try
            {

                var OnAuthorizeEVSEStartedLocal = OnAuthorizeEVSEStarted;
                if (OnAuthorizeEVSEStartedLocal != null)
                    OnAuthorizeEVSEStartedLocal(Timestamp,
                                                this,
                                                EventTrackingId,
                                                Id,
                                                OperatorId,
                                                AuthToken,
                                                EVSEId,
                                                ChargingProductId,
                                                SessionId,
                                                QueryTimeout,
                                                result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeEVSEStarted));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(...OperatorId, AuthToken, ChargingStationId, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification charging station.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(DateTime            Timestamp,
                           CancellationToken   CancellationToken,
                           EventTracking_Id    EventTrackingId,
                           EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           ChargingStation_Id  ChargingStationId,
                           ChargingProduct_Id  ChargingProductId  = null,
                           ChargingSession_Id  SessionId          = null,
                           TimeSpan?           QueryTimeout       = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId",         "The given EVSE operator must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",          "The given authentication token must not be null!");

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            #endregion

            #region Send OnAuthorizeChargingStationStart event

            try
            {

                var OnAuthorizeChargingStationStartLocal = OnAuthorizeChargingStationStart;
                if (OnAuthorizeChargingStationStartLocal != null)
                    OnAuthorizeChargingStationStartLocal(Timestamp,
                                                         this,
                                                         EventTrackingId,
                                                         Id,
                                                         OperatorId,
                                                         AuthToken,
                                                         ChargingStationId,
                                                         ChargingProductId,
                                                         SessionId,
                                                         QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeChargingStationStart));
            }

            #endregion


            AuthStartChargingStationResult result = null;

            foreach (var AuthenticationService in _IeMobilityServiceProviders.
                                                      OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                      Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            {

                result = await AuthenticationService.AuthorizeStart(Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    OperatorId,
                                                                    AuthToken,
                                                                    ChargingStationId,
                                                                    ChargingProductId,
                                                                    SessionId,
                                                                    QueryTimeout);


                #region Authorized

                if (result.AuthorizationResult == AuthStartChargingStationResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    _ChargingSessions.TryAdd(result.SessionId,
                                             new ChargingSession(result.SessionId) {
                                                 AuthService        = AuthenticationService,
                                                 EVSEOperatorId     = OperatorId,
                                                 ChargingStationId  = ChargingStationId,
                                                 AuthToken          = AuthToken,
                                                 ChargingProductId  = ChargingProductId
                                             });

                    break;

                }

                #endregion

                #region Blocked

                else if (result.AuthorizationResult == AuthStartChargingStationResultType.Blocked)
                    break;

                #endregion

            }

            foreach (var OperatorRoamingService in _OperatorRoamingServices.
                                                      OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                      Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            {

                result = await OperatorRoamingService.AuthorizeStart(Timestamp,
                                                                     CancellationToken,
                                                                     EventTrackingId,
                                                                     OperatorId,
                                                                     AuthToken,
                                                                     ChargingStationId,
                                                                     ChargingProductId,
                                                                     SessionId,
                                                                     QueryTimeout);


                #region Authorized

                if (result.AuthorizationResult == AuthStartChargingStationResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    _ChargingSessions.TryAdd(result.SessionId,
                                             new ChargingSession(result.SessionId) {
                                                 OperatorRoamingService  = OperatorRoamingService,
                                                 EVSEOperatorId          = OperatorId,
                                                 ChargingStationId       = ChargingStationId,
                                                 AuthToken               = AuthToken,
                                                 ChargingProductId       = ChargingProductId
                                             });

                    break;

                }

                #endregion

                #region Blocked

                else if (result.AuthorizationResult == AuthStartChargingStationResultType.Blocked)
                    break;

                #endregion

            }

            #region ...or fail!

            if (result == null)
                result = AuthStartChargingStationResult.Error(AuthorizatorId,
                                                              "No authorization service returned a positiv result!");

            #endregion


            #region Send OnAuthorizeChargingStationStarted event

            try
            {

                var OnAuthorizeChargingStationStartedLocal = OnAuthorizeChargingStationStarted;
                if (OnAuthorizeChargingStationStartedLocal != null)
                    OnAuthorizeChargingStationStartedLocal(Timestamp,
                                                           this,
                                                           EventTrackingId,
                                                           Id,
                                                           OperatorId,
                                                           AuthToken,
                                                           ChargingStationId,
                                                           ChargingProductId,
                                                           SessionId,
                                                           QueryTimeout,
                                                           result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeChargingStationStarted));
            }

            #endregion

            return result;

        }

        #endregion


        #region OnAuthorizeStart / OnAuthorizeStarted

        /// <summary>
        /// An event fired whenever an authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartDelegate OnAuthorizeStart;

        /// <summary>
        /// An event fired whenever an authorize start command completed.
        /// </summary>
        public event OnAuthorizeStartedDelegate OnAuthorizeStarted;

        #endregion

        #region OnAuthorizeEVSEStart / OnAuthorizeEVSEStarted

        /// <summary>
        /// An event fired whenever an authorize start EVSE command was received.
        /// </summary>
        public event OnAuthorizeEVSEStartDelegate OnAuthorizeEVSEStart;

        /// <summary>
        /// An event fired whenever an authorize start EVSE command completed.
        /// </summary>
        public event OnAuthorizeEVSEStartedDelegate OnAuthorizeEVSEStarted;

        #endregion

        #region OnAuthorizeChargingStationStart / OnAuthorizeChargingStationStarted

        /// <summary>
        /// An event fired whenever an authorize start charging station command was received.
        /// </summary>
        public event OnAuthorizeChargingStationStartDelegate OnAuthorizeChargingStationStart;

        /// <summary>
        /// An event fired whenever an authorize start charging station command completed.
        /// </summary>
        public event OnAuthorizeChargingStationStartedDelegate OnAuthorizeChargingStationStarted;

        #endregion



        #region AuthorizeStop(...OperatorId, SessionId, AuthToken, ...)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          EVSEOperator_Id     OperatorId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            #region Send OnAuthorizeStop event

            try
            {

                var OnAuthorizeStopLocal = OnAuthorizeStop;
                if (OnAuthorizeStopLocal != null)
                    OnAuthorizeStopLocal(Timestamp,
                                         this,
                                         EventTrackingId,
                                         Id,
                                         OperatorId,
                                         SessionId,
                                         AuthToken,
                                         QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeStop));
            }

            #endregion


            AuthStopResult result = null;

            #region An authenticator was found for the upstream SessionId!

            ChargingSession _ChargingSession = null;

            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession))
            {

                if (_ChargingSession.AuthService != null)
                    result = await _ChargingSession.AuthService.           AuthorizeStop(Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         OperatorId,
                                                                                         SessionId,
                                                                                         AuthToken,
                                                                                         QueryTimeout);

                else if (_ChargingSession.OperatorRoamingService != null)
                    result = await _ChargingSession.OperatorRoamingService.AuthorizeStop(Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         OperatorId,
                                                                                         SessionId,
                                                                                         AuthToken,
                                                                                         QueryTimeout);

            }

            #endregion

            #region Try to find anyone who might kown anything about the given SessionId!

            if (result == null || result.AuthorizationResult != AuthStopResultType.Authorized)
                foreach (var OtherAuthenticationService in _IeMobilityServiceProviders.
                                                               OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                               Select (AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                               ToArray())
                {

                    result = await OtherAuthenticationService.AuthorizeStop(Timestamp,
                                                                            CancellationToken,
                                                                            EventTrackingId,
                                                                            OperatorId,
                                                                            SessionId,
                                                                            AuthToken,
                                                                            QueryTimeout);

                    if (result.AuthorizationResult == AuthStopResultType.Authorized)
                        break;

                }

            if (result == null || result.AuthorizationResult != AuthStopResultType.Authorized)
                foreach (var OtherOperatorRoamingServices in _OperatorRoamingServices.
                                                                 OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                                 Select (AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                                 ToArray())
                {

                    result = await OtherOperatorRoamingServices.AuthorizeStop(Timestamp,
                                                                              CancellationToken,
                                                                              EventTrackingId,
                                                                              OperatorId,
                                                                              SessionId,
                                                                              AuthToken,
                                                                              QueryTimeout);

                    if (result.AuthorizationResult == AuthStopResultType.Authorized)
                        break;

                }

            #endregion

            #region ...or fail!

            if (result == null)
                result = AuthStopResult.Error(AuthorizatorId,
                                              "No authorization service returned a positiv result!");

            #endregion


            #region Send OnAuthorizeStopped event

            try
            {

                var OnAuthorizeStoppedLocal = OnAuthorizeStopped;
                if (OnAuthorizeStoppedLocal != null)
                    OnAuthorizeStoppedLocal(Timestamp,
                                            this,
                                            EventTrackingId,
                                            Id,
                                            OperatorId,
                                            SessionId,
                                            AuthToken,
                                            QueryTimeout,
                                            result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(...OperatorId, SessionId, AuthToken, EVSEId, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          EVSEOperator_Id     OperatorId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          EVSE_Id             EVSEId,
                          TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),     "The given parameter must not be null!");

            #endregion

            #region Send OnAuthorizeEVSEStop event

            try
            {

                var OnAuthorizeEVSEStopLocal = OnAuthorizeEVSEStop;
                if (OnAuthorizeEVSEStopLocal != null)
                    OnAuthorizeEVSEStopLocal(Timestamp,
                                             this,
                                             EventTrackingId,
                                             Id,
                                             OperatorId,
                                             EVSEId,
                                             SessionId,
                                             AuthToken,
                                             QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeEVSEStop));
            }

            #endregion


            AuthStopEVSEResult result = null;

            #region An authenticator was found for the upstream SessionId!

            ChargingSession _ChargingSession = null;

            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession))
            {

                if (_ChargingSession.AuthService != null)
                    result = await _ChargingSession.AuthService.           AuthorizeStop(Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         OperatorId,
                                                                                         EVSEId,
                                                                                         SessionId,
                                                                                         AuthToken,
                                                                                         QueryTimeout);

                else if (_ChargingSession.OperatorRoamingService != null)
                    result = await _ChargingSession.OperatorRoamingService.AuthorizeStop(Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         OperatorId,
                                                                                         EVSEId,
                                                                                         SessionId,
                                                                                         AuthToken,
                                                                                         QueryTimeout);

            }

            #endregion

            #region Try to find anyone who might kown anything about the given SessionId!

            if (result == null || result.Result != AuthStopEVSEResultType.Authorized)
                foreach (var OtherAuthenticationService in _IeMobilityServiceProviders.
                                                               OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                               Select (AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                               ToArray())
                {

                    result = await OtherAuthenticationService.AuthorizeStop(Timestamp,
                                                                            CancellationToken,
                                                                            EventTrackingId,
                                                                            OperatorId,
                                                                            EVSEId,
                                                                            SessionId,
                                                                            AuthToken,
                                                                            QueryTimeout);

                    if (result.Result == AuthStopEVSEResultType.Authorized)
                        break;

                }

            if (result == null || result.Result != AuthStopEVSEResultType.Authorized)
                foreach (var OtherOperatorRoamingServices in _OperatorRoamingServices.
                                                                 OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                                 Select(AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                                 ToArray())
                {

                    result = await OtherOperatorRoamingServices.AuthorizeStop(Timestamp,
                                                                              CancellationToken,
                                                                              EventTrackingId,
                                                                              OperatorId,
                                                                              EVSEId,
                                                                              SessionId,
                                                                              AuthToken,
                                                                              QueryTimeout);

                    if (result.Result == AuthStopEVSEResultType.Authorized)
                        break;

                }

            #endregion

            #region ...or fail!

            if (result == null)
                result = AuthStopEVSEResult.Error(AuthorizatorId,
                                                  "No authorization service returned a positiv result!");

            #endregion


            #region Send OnAuthorizeEVSEStopped event

            try
            {

                var OnAuthorizeEVSEStoppedLocal = OnAuthorizeEVSEStopped;
                if (OnAuthorizeEVSEStoppedLocal != null)
                    OnAuthorizeEVSEStoppedLocal(Timestamp,
                                                this,
                                                EventTrackingId,
                                                Id,
                                                OperatorId,
                                                EVSEId,
                                                SessionId,
                                                AuthToken,
                                                QueryTimeout,
                                                result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeEVSEStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(...OperatorId, SessionId, AuthToken, EVSEId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          EVSEOperator_Id     OperatorId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          ChargingStation_Id  ChargingStationId,
                          TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId",         "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException(nameof(SessionId),          "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",          "The given parameter must not be null!");

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given parameter must not be null!");

            #endregion

            #region Send OnAuthorizeChargingStationStop event

            try
            {

                var OnAuthorizeChargingStationStopLocal = OnAuthorizeChargingStationStop;
                if (OnAuthorizeChargingStationStopLocal != null)
                    OnAuthorizeChargingStationStopLocal(Timestamp,
                                                        this,
                                                        EventTrackingId,
                                                        Id,
                                                        OperatorId,
                                                        ChargingStationId,
                                                        SessionId,
                                                        AuthToken,
                                                        QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeChargingStationStop));
            }

            #endregion


            AuthStopChargingStationResult result = null;

            #region An authenticator was found for the upstream SessionId!

            ChargingSession _ChargingSession = null;

            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession))
            {

                if (_ChargingSession.AuthService != null)
                    result = await _ChargingSession.AuthService.           AuthorizeStop(Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         OperatorId,
                                                                                         ChargingStationId,
                                                                                         SessionId,
                                                                                         AuthToken,
                                                                                         QueryTimeout);

                else if (_ChargingSession.OperatorRoamingService != null)
                    result = await _ChargingSession.OperatorRoamingService.AuthorizeStop(Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         OperatorId,
                                                                                         ChargingStationId,
                                                                                         SessionId,
                                                                                         AuthToken,
                                                                                         QueryTimeout);

            }

            #endregion

            #region Try to find anyone who might kown anything about the given SessionId!

            if (result == null || result.AuthorizationResult != AuthStopChargingStationResultType.Authorized)
                foreach (var OtherAuthenticationService in _IeMobilityServiceProviders.
                                                               OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                               Select (AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                               ToArray())
                {

                    result = await OtherAuthenticationService.AuthorizeStop(Timestamp,
                                                                            CancellationToken,
                                                                            EventTrackingId,
                                                                            OperatorId,
                                                                            ChargingStationId,
                                                                            SessionId,
                                                                            AuthToken,
                                                                            QueryTimeout);

                    if (result.AuthorizationResult == AuthStopChargingStationResultType.Authorized)
                        break;

                }

            if (result == null || result.AuthorizationResult != AuthStopChargingStationResultType.Authorized)
                foreach (var OtherOperatorRoamingServices in _OperatorRoamingServices.
                                                                 OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                                 Select(AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                                 ToArray())
                {

                    result = await OtherOperatorRoamingServices.AuthorizeStop(Timestamp,
                                                                              CancellationToken,
                                                                              EventTrackingId,
                                                                              OperatorId,
                                                                              ChargingStationId,
                                                                              SessionId,
                                                                              AuthToken,
                                                                              QueryTimeout);

                    if (result.AuthorizationResult == AuthStopChargingStationResultType.Authorized)
                        break;

                }

            #endregion

            #region ...or fail!

            if (result == null)
                result = AuthStopChargingStationResult.Error(AuthorizatorId,
                                                             "No authorization service returned a positiv result!");

            #endregion


            #region Send OnAuthorizeChargingStationStopped event

            try
            {

                var OnAuthorizeChargingStationStoppedLocal = OnAuthorizeChargingStationStopped;
                if (OnAuthorizeChargingStationStoppedLocal != null)
                    OnAuthorizeChargingStationStoppedLocal(Timestamp,
                                                           this,
                                                           EventTrackingId,
                                                           Id,
                                                           OperatorId,
                                                           ChargingStationId,
                                                           SessionId,
                                                           AuthToken,
                                                           QueryTimeout,
                                                           result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnAuthorizeChargingStationStopped));
            }

            #endregion

            return result;

        }

        #endregion


        #region OnAuthorizeStop / OnAuthorizeStoped

        /// <summary>
        /// An event fired whenever an authorize stop command was received.
        /// </summary>
        public event OnAuthorizeStopDelegate OnAuthorizeStop;

        /// <summary>
        /// An event fired whenever an authorize stop command completed.
        /// </summary>
        public event OnAuthorizeStoppedDelegate OnAuthorizeStopped;

        #endregion

        #region OnAuthorizeEVSEStop / OnAuthorizeEVSEStoped

        /// <summary>
        /// An event fired whenever an authorize stop EVSE command was received.
        /// </summary>
        public event OnAuthorizeEVSEStopDelegate OnAuthorizeEVSEStop;

        /// <summary>
        /// An event fired whenever an authorize stop EVSE command completed.
        /// </summary>
        public event OnAuthorizeEVSEStoppedDelegate OnAuthorizeEVSEStopped;

        #endregion

        #region OnAuthorizeStop / OnAuthorizeStoped

        /// <summary>
        /// An event fired whenever an authorize stop charging station command was received.
        /// </summary>
        public event OnAuthorizeChargingStationStopDelegate OnAuthorizeChargingStationStop;

        /// <summary>
        /// An event fired whenever an authorize stop charging station command completed.
        /// </summary>
        public event OnAuthorizeChargingStationStoppedDelegate OnAuthorizeChargingStationStopped;

        #endregion

        #endregion

        #region Charging Sessions / Charge Detail Records...

        #region ChargingSessions

        private readonly ConcurrentDictionary<ChargingSession_Id, ChargingSession> _ChargingSessions;

        /// <summary>
        /// Return all current charging sessions.
        /// </summary>
        public IEnumerable<ChargingSession> ChargingSessions
        {
            get
            {
                return _ChargingSessions.Select(kvp => kvp.Value);
            }
        }

        #endregion

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate OnNewChargingSession;

        #region (internal) SendNewChargingSession(Timestamp, Sender, ChargingSession)

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  ChargingSession)
        {

            var OnNewChargingSessionLocal = OnNewChargingSession;
            if (OnNewChargingSessionLocal != null)
                OnNewChargingSessionLocal(Timestamp, Sender, ChargingSession);

        }

        #endregion


        #region ChargeDetailRecords

        private readonly ConcurrentDictionary<ChargingSession_Id, ChargeDetailRecord> _ChargeDetailRecords;

        /// <summary>
        /// Return all current charge detail records.
        /// </summary>
        public IEnumerable<ChargeDetailRecord> ChargeDetailRecords
        {
            get
            {
                return _ChargeDetailRecords.Select(kvp => kvp.Value);
            }
        }

        #endregion

        #region OnSendCDR / OnCDRSent

        /// <summary>
        /// An event fired whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordSendDelegate OnSendCDR;

        /// <summary>
        /// An event fired whenever a charge detail record result was sent.
        /// </summary>
        public event OnChargeDetailRecordSentDelegate OnCDRSent;

        #endregion

        ///// <summary>
        ///// An event fired whenever a new charge detail record was created.
        ///// </summary>
        //public event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;


        #region OnFilterCDRRecords

        public delegate SendCDRResult OnFilterCDRRecordsDelegate(Authorizator_Id AuthorizatorId, AuthInfo AuthInfo);

        /// <summary>
        /// An event fired whenever a CDR needs to be filtered.
        /// </summary>
        public event OnFilterCDRRecordsDelegate OnFilterCDRRecords;

        #endregion

        #region SendChargeDetailRecord(...EVSEId, ChargingSessionId, ChargingProductId, SessionStart, SessionEnd, AuthInfo, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="ChargingSessionId">The session identification from the Authorize Start request.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthInfo">An optional ev customer or e-Mobility account identification.</param>
        /// <param name="MeterValues">An optional enumeration of meter values during the charging session.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(DateTime                          Timestamp,
                                   CancellationToken                 CancellationToken,
                                   EventTracking_Id                  EventTrackingId,
                                   EVSE_Id                           EVSEId,
                                   ChargingSession_Id                ChargingSessionId,
                                   ChargingProduct_Id                ChargingProductId,
                                   DateTime                          SessionStart,
                                   DateTime                          SessionEnd,
                                   AuthInfo                          AuthInfo,
                                   IEnumerable<Timestamped<Double>>  MeterValues        = null,
                                   String                            MeteringSignature  = null,
                                   TimeSpan?                         QueryTimeout       = null)

        {

            #region Initial checks

            if (ChargingSessionId == null)
                throw new ArgumentNullException("ChargingSessionId", "The given charging session identification must not be null!");

            #endregion

            #region Some CDR should perhaps be filtered...

            var OnFilterCDRRecordsLocal = OnFilterCDRRecords;
            if (OnFilterCDRRecordsLocal != null)
            {

                var _SENDCDRResult = OnFilterCDRRecordsLocal(AuthorizatorId, AuthInfo);

                if (_SENDCDRResult != null)
                    return _SENDCDRResult;

            }

            #endregion

            return await SendChargeDetailRecord(Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                new ChargeDetailRecord(SessionId:          ChargingSessionId,
                                                                       EVSEId:             EVSEId,
                                                                       ChargingProductId:  ChargingProductId,
                                                                       SessionTime:        new StartEndDateTime(SessionStart, SessionEnd),
                                                                       Identification:     AuthInfo,
                                                                       //ChargingTime:       new StartEndDateTime(ChargingStart.Value, ChargingEnd.Value),
                                                                       EnergyMeterValues:  MeterValues,
                                                                       //ConsumedEnergy
                                                                       MeteringSignature:  MeteringSignature),

                                                QueryTimeout);

        }

        #endregion

        #region SendChargeDetailRecord(...ChargeDetailRecord, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargeDetailRecord"></param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(DateTime            Timestamp,
                                   CancellationToken   CancellationToken,
                                   EventTracking_Id    EventTrackingId,
                                   ChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?           QueryTimeout = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException("ChargeDetailRecord", "The given charge detail record must not be null!");

            #endregion

            #region Send OnSendCDR event

            try
            {

                var OnSendCDRLocal = OnSendCDR;
                if (OnSendCDRLocal != null)
                    OnSendCDRLocal(DateTime.Now,
                                   this,
                                   EventTrackingId,
                                   this.Id,
                                   ChargeDetailRecord,
                                   QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnSendCDR));
            }

            #endregion


            SendCDRResult result = null;

            #region Some CDR should perhaps be filtered...

            var OnFilterCDRRecordsLocal = OnFilterCDRRecords;
            if (OnFilterCDRRecordsLocal != null)
                result = OnFilterCDRRecordsLocal(AuthorizatorId, ChargeDetailRecord.Identification);

            #endregion

            if (result == null)
            {

                #region An authenticator was found for the upstream SessionId!

                ChargingSession _ChargingSession = null;

                if (_ChargingSessions.TryGetValue(ChargeDetailRecord.SessionId, out _ChargingSession))
                {

                    if (_ChargingSession.AuthService != null)
                        result = await _ChargingSession.AuthService.SendChargeDetailRecord(Timestamp,
                                                                                           CancellationToken,
                                                                                           EventTrackingId,
                                                                                           ChargeDetailRecord,
                                                                                           QueryTimeout);

                    else if (_ChargingSession.OperatorRoamingService != null)
                        result = await _ChargingSession.OperatorRoamingService.SendChargeDetailRecord(Timestamp,
                                                                                                      CancellationToken,
                                                                                                      EventTrackingId,
                                                                                                      ChargeDetailRecord,
                                                                                                      QueryTimeout);

                    _ChargingSession.RemoveMe = true;

                }

                #endregion

                #region Try to find anyone who might kown anything about the given SessionId!

                if (result == null ||
                    result.Status == SendCDRResultType.InvalidSessionId)
                {

                    foreach (var OtherAuthenticationService in _IeMobilityServiceProviders.
                                                                   OrderBy(v => v.Key).
                                                                   Select(v => v.Value).
                                                                   ToArray())
                    {

                        result = await OtherAuthenticationService.SendChargeDetailRecord(Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         ChargeDetailRecord,
                                                                                         QueryTimeout);

                    }

                }

                if (result == null ||
                    result.Status == SendCDRResultType.InvalidSessionId)
                {

                    foreach (var OtherOperatorRoamingService in _OperatorRoamingServices.
                                                                    OrderBy(v => v.Key).
                                                                    Select(v => v.Value).
                                                                    ToArray())
                    {

                        result = await OtherOperatorRoamingService.SendChargeDetailRecord(Timestamp,
                                                                                          CancellationToken,
                                                                                          EventTrackingId,
                                                                                          ChargeDetailRecord,
                                                                                          QueryTimeout);

                    }

                }

                #endregion

                #region ...else fail!

                if (result == null ||
                    result.Status == SendCDRResultType.InvalidSessionId)
                {

                    return SendCDRResult.NotForwared(AuthorizatorId,
                                                     "No authorization service returned a positiv result!");

                }

                #endregion

            }


            #region Send OnCDRSent event

            try
            {

                var OnCDRSentLocal = OnCDRSent;
                if (OnCDRSentLocal != null)
                    OnCDRSentLocal(DateTime.Now,
                                   this,
                                   EventTrackingId,
                                   this.Id,
                                   ChargeDetailRecord,
                                   QueryTimeout,
                                   result);

            }
            catch (Exception e)
            {
                e.Log("RoamingNetwork." + nameof(OnCDRSent));
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

            _ChargeDetailRecords.TryAdd(ChargeDetailRecord.SessionId, ChargeDetailRecord);

            var OnNewChargeDetailRecordLocal = OnSendCDR;
            if (OnNewChargeDetailRecordLocal != null)
                OnNewChargeDetailRecordLocal(Timestamp,
                                             Sender,
                                             EventTracking_Id.New,
                                             this.Id,
                                             ChargeDetailRecord,
                                             TimeSpan.FromMinutes(1));

        }

        #endregion

        #endregion



        #region IEnumerable<IEntity> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _EVSEOperators.Values.GetEnumerator();
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return _EVSEOperators.Values.GetEnumerator();
        }

        #endregion

        #region IComparable<RoamingNetwork> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an RoamingNetwork.
            var RoamingNetwork = Object as RoamingNetwork;
            if ((Object) RoamingNetwork == null)
                throw new ArgumentException("The given object is not an RoamingNetwork!");

            return CompareTo(RoamingNetwork);

        }

        #endregion

        #region CompareTo(RoamingNetwork)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork">An RoamingNetwork object to compare with.</param>
        public Int32 CompareTo(RoamingNetwork RoamingNetwork)
        {

            if ((Object) RoamingNetwork == null)
                throw new ArgumentNullException("The given RoamingNetwork must not be null!");

            return Id.CompareTo(RoamingNetwork.Id);

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetwork> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an RoamingNetwork.
            var RoamingNetwork = Object as RoamingNetwork;
            if ((Object) RoamingNetwork == null)
                return false;

            return this.Equals(RoamingNetwork);

        }

        #endregion

        #region Equals(RoamingNetwork)

        /// <summary>
        /// Compares two RoamingNetwork for equality.
        /// </summary>
        /// <param name="RoamingNetwork">An RoamingNetwork to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetwork RoamingNetwork)
        {

            if ((Object) RoamingNetwork == null)
                return false;

            return Id.Equals(RoamingNetwork.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return Id.ToString();
        }

        #endregion

    }

}
