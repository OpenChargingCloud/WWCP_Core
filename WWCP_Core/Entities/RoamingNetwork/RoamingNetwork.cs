/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using org.GraphDefined.WWCP.LocalService;

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
    public class RoamingNetwork : AEntity<RoamingNetwork_Id>,
                                  IEquatable<RoamingNetwork>, IComparable<RoamingNetwork>, IComparable,
                                  IEnumerable<IEntity>
    {

        #region Data

        private  readonly ConcurrentDictionary<EVSEOperator_Id,               EVSEOperator>               _EVSEOperators;
        private  readonly ConcurrentDictionary<EVSP_Id,                       EVServiceProvider>          _EVServiceProviders;
        private  readonly ConcurrentDictionary<RoamingProvider_Id,            RoamingProvider>            _RoamingProviders;
        private  readonly ConcurrentDictionary<NavigationServiceProvider_Id,  NavigationServiceProvider>  _SearchProviders;

        #endregion

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the RoamingNetwork.
        /// </summary>
        [Mandatory]
        public I18NString Name
        {

            get
            {
                return _Name;
            }

            set
            {
                SetProperty<I18NString>(ref _Name, value);
            }

        }

        #endregion

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional additional (multi-language) description of the RoamingNetwork.
        /// </summary>
        [Optional]
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


        #region EVSEOperators

        /// <summary>
        /// Return all EVSE operators registered within this roaming network.
        /// </summary>
        public IEnumerable<EVSEOperator> EVSEOperators
        {
            get
            {
                return _EVSEOperators.Values;
            }
        }

        #endregion

        #region EVServiceProviders

        /// <summary>
        /// Return all EV service providers registered within this roaming network.
        /// </summary>
        public IEnumerable<EVServiceProvider> EVServiceProviders
        {
            get
            {
                return _EVServiceProviders.Values;
            }
        }

        #endregion

        #region RoamingProviders

        /// <summary>
        /// Return all roaming providers registered within this roaming network.
        /// </summary>
        public IEnumerable<RoamingProvider> RoamingProviders
        {
            get
            {
                return _RoamingProviders.Values;
            }
        }

        #endregion

        #region SearchProviders

        /// <summary>
        /// Return all search providers registered within this roaming network.
        /// </summary>
        public IEnumerable<NavigationServiceProvider> SearchProviders
        {
            get
            {
                return _SearchProviders.Values;
            }
        }

        #endregion

        #region ChargingPools

        /// <summary>
        /// Return all search providers registered within this roaming network.
        /// </summary>
        public IEnumerable<ChargingPool> ChargingPools
        {
            get
            {
                return _EVSEOperators.SelectMany(v => v.Value);
            }
        }

        #endregion


        #region RequestRouter

        private readonly RequestRouter _RequestRouter;

        public RequestRouter RequestRouter
        {
            get
            {
                return _RequestRouter;
            }
        }

        #endregion

        #endregion

        #region Events

        // RoamingNetwork events

        #region EVSEOperatorAddition

        private readonly IVotingNotificator<RoamingNetwork, EVSEOperator, Boolean> EVSEOperatorAddition;

        /// <summary>
        /// Called whenever an EVSEOperator will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, EVSEOperator, Boolean> OnEVSEOperatorAddition
        {
            get
            {
                return EVSEOperatorAddition;
            }
        }

        #endregion

        #region EVSEOperatorRemoval

        private readonly IVotingNotificator<RoamingNetwork, EVSEOperator, Boolean> EVSEOperatorRemoval;

        /// <summary>
        /// Called whenever an EVSEOperator will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, EVSEOperator, Boolean> OnEVSEOperatorRemoval
        {
            get
            {
                return EVSEOperatorRemoval;
            }
        }

        #endregion


        #region EVServiceProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, EVServiceProvider, Boolean> EVServiceProviderAddition;

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, EVServiceProvider, Boolean> OnEVServiceProviderAddition
        {
            get
            {
                return EVServiceProviderAddition;
            }
        }

        #endregion

        #region EVServiceProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, EVServiceProvider, Boolean> EVServiceProviderRemoval;

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, EVServiceProvider, Boolean> OnEVServiceProviderRemoval
        {
            get
            {
                return EVServiceProviderRemoval;
            }
        }

        #endregion


        #region RoamingProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, RoamingProvider, Boolean> RoamingProviderAddition;

        /// <summary>
        /// Called whenever a RoamingProvider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, RoamingProvider, Boolean> OnRoamingProviderAddition
        {
            get
            {
                return RoamingProviderAddition;
            }
        }

        #endregion

        #region RoamingProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, RoamingProvider, Boolean> RoamingProviderRemoval;

        /// <summary>
        /// Called whenever a RoamingProvider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, RoamingProvider, Boolean> OnRoamingProviderRemoval
        {
            get
            {
                return RoamingProviderRemoval;
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


        // EVSEOperator events

        #region ChargingPoolAddition

        internal readonly IVotingNotificator<EVSEOperator, ChargingPool, Boolean> ChargingPoolAddition;

        /// <summary>
        /// Called whenever an EVS pool will be or was added.
        /// </summary>
        public IVotingSender<EVSEOperator, ChargingPool, Boolean> OnChargingPoolAddition
        {
            get
            {
                return ChargingPoolAddition;
            }
        }

        #endregion

        #region ChargingPoolRemoval

        internal readonly IVotingNotificator<EVSEOperator, ChargingPool, Boolean> ChargingPoolRemoval;

        /// <summary>
        /// Called whenever an EVS pool will be or was removed.
        /// </summary>
        public IVotingSender<EVSEOperator, ChargingPool, Boolean> OnChargingPoolRemoval
        {
            get
            {
                return ChargingPoolRemoval;
            }
        }

        #endregion


        // ChargingPool events

        #region ChargingStationAddition

        internal readonly IVotingNotificator<ChargingPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<ChargingPool, ChargingStation, Boolean> OnChargingStationAddition
        {
            get
            {
                return ChargingStationAddition;
            }
        }

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<ChargingPool, ChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<ChargingPool, ChargingStation, Boolean> OnChargingStationRemoval
        {
            get
            {
                return ChargingStationRemoval;
            }
        }

        #endregion


        // ChargingStation events

        #region EVSEAddition

        internal readonly IVotingNotificator<ChargingStation, EVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<ChargingStation, EVSE, Boolean> OnEVSEAddition
        {
            get
            {
                return EVSEAddition;
            }
        }

        #endregion

        #region EVSERemoval

        internal readonly IVotingNotificator<ChargingStation, EVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<ChargingStation, EVSE, Boolean> OnEVSERemoval
        {
            get
            {
                return EVSERemoval;
            }
        }

        #endregion


        // EVSE events

        #region SocketOutletAddition

        internal readonly IVotingNotificator<EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<EVSE, SocketOutlet, Boolean> OnSocketOutletAddition
        {
            get
            {
                return SocketOutletAddition;
            }
        }

        #endregion

        #region SocketOutletRemoval

        internal readonly IVotingNotificator<EVSE, SocketOutlet, Boolean> SocketOutletRemoval;

        /// <summary>
        /// Called whenever a socket outlet will be or was removed.
        /// </summary>
        public IVotingSender<EVSE, SocketOutlet, Boolean> OnSocketOutletRemoval
        {
            get
            {
                return SocketOutletRemoval;
            }
        }

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

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The given unique roaming network identification must not be null!");

            #endregion

            #region Init data and properties

            this._EVSEOperators             = new ConcurrentDictionary<EVSEOperator_Id,              EVSEOperator>();
            this._EVServiceProviders        = new ConcurrentDictionary<EVSP_Id,                      EVServiceProvider>();
            this._RoamingProviders          = new ConcurrentDictionary<RoamingProvider_Id,           RoamingProvider>();
            this._SearchProviders           = new ConcurrentDictionary<NavigationServiceProvider_Id, NavigationServiceProvider>();
            this._RequestRouter             = new RequestRouter(Id, AuthorizatorId);

            this.Name                       = new I18NString(Languages.en, Id.ToString());
            this.Description                = new I18NString();

            #endregion

            #region Init events

            // RoamingNetwork events
            this.EVSEOperatorAddition       = new VotingNotificator<RoamingNetwork,  EVSEOperator,              Boolean>(() => new VetoVote(), true);
            this.EVSEOperatorRemoval        = new VotingNotificator<RoamingNetwork,  EVSEOperator,              Boolean>(() => new VetoVote(), true);

            this.EVServiceProviderAddition  = new VotingNotificator<RoamingNetwork,  EVServiceProvider,         Boolean>(() => new VetoVote(), true);
            this.EVServiceProviderRemoval   = new VotingNotificator<RoamingNetwork,  EVServiceProvider,         Boolean>(() => new VetoVote(), true);

            this.RoamingProviderAddition    = new VotingNotificator<RoamingNetwork,  RoamingProvider,           Boolean>(() => new VetoVote(), true);
            this.RoamingProviderRemoval     = new VotingNotificator<RoamingNetwork,  RoamingProvider,           Boolean>(() => new VetoVote(), true);

            this.SearchProviderAddition     = new VotingNotificator<RoamingNetwork,  NavigationServiceProvider, Boolean>(() => new VetoVote(), true);
            this.SearchProviderRemoval      = new VotingNotificator<RoamingNetwork,  NavigationServiceProvider, Boolean>(() => new VetoVote(), true);

            // EVSEOperator events
            this.ChargingPoolAddition       = new VotingNotificator<EVSEOperator,    ChargingPool,              Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval        = new VotingNotificator<EVSEOperator,    ChargingPool,              Boolean>(() => new VetoVote(), true);

            // ChargingPool events
            this.ChargingStationAddition    = new VotingNotificator<ChargingPool,    ChargingStation,           Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval     = new VotingNotificator<ChargingPool,    ChargingStation,           Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition               = new VotingNotificator<ChargingStation, EVSE,                      Boolean>(() => new VetoVote(), true);
            this.EVSERemoval                = new VotingNotificator<ChargingStation, EVSE,                      Boolean>(() => new VetoVote(), true);

            // EVSE events
            this.SocketOutletAddition       = new VotingNotificator<EVSE,            SocketOutlet,              Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval        = new VotingNotificator<EVSE,            SocketOutlet,              Boolean>(() => new VetoVote(), true);

            #endregion

        }

        #endregion

        #endregion


        #region EVSEOperator

        #region CreateNewEVSEOperator(EVSEOperatorId, Name = null, Description = null, Action = null)

        /// <summary>
        /// Create and register a new EVSE operator having the given
        /// unique EVSE operator identification.
        /// </summary>
        /// <param name="EVSEOperatorId">The unique identification of the new EVSE operator.</param>
        /// <param name="Name">The offical (multi-language) name of the EVSE Operator.</param>
        /// <param name="Description">An optional (multi-language) description of the EVSE Operator.</param>
        /// <param name="Action">An optional delegate to configure the new EVSE operator after its creation.</param>
        public EVSEOperator CreateNewEVSEOperator(EVSEOperator_Id       EVSEOperatorId,
                                                  I18NString            Name           = null,
                                                  I18NString            Description    = null,
                                                  Action<EVSEOperator>  Action         = null)
        {

            #region Initial checks

            if (EVSEOperatorId == null)
                throw new ArgumentNullException("EVSEOperatorId", "The given EVSE operator identification must not be null!");

            if (_EVSEOperators.ContainsKey(EVSEOperatorId))
                throw new EVSEOperatorAlreadyExists(EVSEOperatorId, this.Id);

            #endregion

            var _EVSEOperator = new EVSEOperator(EVSEOperatorId, Name, Description, this);

            Action.FailSafeInvoke(_EVSEOperator);

            if (EVSEOperatorAddition.SendVoting(this, _EVSEOperator))
            {
                if (_EVSEOperators.TryAdd(EVSEOperatorId, _EVSEOperator))
                {
                    EVSEOperatorAddition.SendNotification(this, _EVSEOperator);
                    return _EVSEOperator;
                }
            }

            throw new Exception();

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

        #endregion

        #region CreateNewEVServiceProvider(EVServiceProviderId, Action = null)

        /// <summary>
        /// Create and register a new electric vehicle service provider having the given
        /// unique electric vehicle service provider identification.
        /// </summary>
        /// <param name="EVServiceProviderId">The unique identification of the new roaming provider.</param>
        /// <param name="Action">An optional delegate to configure the new roaming provider after its creation.</param>
        public EVServiceProvider CreateNewEVServiceProvider(EVSP_Id                    EVServiceProviderId,
                                                            Action<EVServiceProvider>  Action  = null)
        {

            #region Initial checks

            if (EVServiceProviderId == null)
                throw new ArgumentNullException("EVServiceProviderId", "The given electric vehicle service provider identification must not be null!");

            if (_EVServiceProviders.ContainsKey(EVServiceProviderId))
                throw new EVServiceProviderAlreadyExists(EVServiceProviderId, this.Id);

            #endregion

            var _EVServiceProvider = new EVServiceProvider(EVServiceProviderId, this);

            Action.FailSafeInvoke(_EVServiceProvider);

            if (EVServiceProviderAddition.SendVoting(this, _EVServiceProvider))
            {
                if (_EVServiceProviders.TryAdd(EVServiceProviderId, _EVServiceProvider))
                {
                    EVServiceProviderAddition.SendNotification(this, _EVServiceProvider);
                    return _EVServiceProvider;
                }
            }

            throw new Exception();

        }

        #endregion

        #region CreateNewEVServiceProvider(EVServiceProviderId, EMobilityService, Action = null)

        /// <summary>
        /// Create and register a new electric vehicle service provider having the given
        /// unique electric vehicle service provider identification.
        /// </summary>
        /// <param name="EVServiceProviderId">The unique identification of the new roaming provider.</param>
        /// <param name="EMobilityService">The attached local or remote e-mobility service.</param>
        /// <param name="Action">An optional delegate to configure the new roaming provider after its creation.</param>
        public EVServiceProvider CreateNewEVServiceProvider(EVSP_Id                    EVServiceProviderId,
                                                            IAuthServices              EMobilityService,
                                                            Action<EVServiceProvider>  Action  = null)
        {

            #region Initial checks

            if (EVServiceProviderId == null)
                throw new ArgumentNullException("EVServiceProviderId", "The given electric vehicle service provider identification must not be null!");

            if (_EVServiceProviders.ContainsKey(EVServiceProviderId))
                throw new EVServiceProviderAlreadyExists(EVServiceProviderId, this.Id);

            #endregion

            var _EVServiceProvider = new EVServiceProvider(EVServiceProviderId, this, EMobilityService);

            Action.FailSafeInvoke(_EVServiceProvider);

            if (EVServiceProviderAddition.SendVoting(this, _EVServiceProvider))
            {
                if (_EVServiceProviders.TryAdd(EVServiceProviderId, _EVServiceProvider))
                {
                    EVServiceProviderAddition.SendNotification(this, _EVServiceProvider);
                    return _EVServiceProvider;
                }
            }

            throw new Exception();

        }

        #endregion

        #region CreateNewRoamingProvider(RoamingProviderId, EMobilityService, Action = null)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider having the given
        /// unique electric vehicle roaming provider identification.
        /// </summary>
        /// <param name="RoamingProviderId">The unique identification of the new roaming provider.</param>
        /// <param name="EMobilityService">The attached E-Mobility service.</param>
        /// <param name="Action">An optional delegate to configure the new roaming provider after its creation.</param>
        public RoamingProvider CreateNewRoamingProvider(RoamingProvider_Id       RoamingProviderId,
                                                        IAuthServices            EMobilityService,
                                                        Action<RoamingProvider>  Action = null)
        {

            #region Initial checks

            if (RoamingProviderId == null)
                throw new ArgumentNullException("RoamingProviderId", "The given roaming provider identification must not be null!");

            if (_RoamingProviders.ContainsKey(RoamingProviderId))
                throw new RoamingProviderAlreadyExists(RoamingProviderId, this.Id);

            #endregion

            var _RoamingProvider = new RoamingProvider(RoamingProviderId, this, EMobilityService);

            Action.FailSafeInvoke(_RoamingProvider);

            if (RoamingProviderAddition.SendVoting(this, _RoamingProvider))
            {
                if (_RoamingProviders.TryAdd(RoamingProviderId, _RoamingProvider))
                {
                    RoamingProviderAddition.SendNotification(this, _RoamingProvider);
                    return _RoamingProvider;
                }
            }

            throw new Exception();

        }

        #endregion

        #region CreateNewSearchProvider(NavigationServiceProviderId, Action = null)

        /// <summary>
        /// Create and register a new navigation service provider having the given
        /// unique identification.
        /// </summary>
        /// <param name="NavigationServiceProviderId">The unique identification of the new search provider.</param>
        /// <param name="Action">An optional delegate to configure the new search provider after its creation.</param>
        public NavigationServiceProvider CreateNewSearchProvider(NavigationServiceProvider_Id       NavigationServiceProviderId,
                                                                 Action<NavigationServiceProvider>  Action = null)
        {

            #region Initial checks

            if (NavigationServiceProviderId == null)
                throw new ArgumentNullException("NavigationServiceProviderId", "The given navigation service provider identification must not be null!");

            if (_SearchProviders.ContainsKey(NavigationServiceProviderId))
                throw new SearchProviderAlreadyExists(NavigationServiceProviderId, this.Id);

            #endregion

            var _SearchProvider = new NavigationServiceProvider(NavigationServiceProviderId, this);

            Action.FailSafeInvoke(_SearchProvider);

            if (SearchProviderAddition.SendVoting(this, _SearchProvider))
            {
                if (_SearchProviders.TryAdd(NavigationServiceProviderId, _SearchProvider))
                {
                    SearchProviderAddition.SendNotification(this, _SearchProvider);
                    return _SearchProvider;
                }
            }

            throw new Exception();

        }

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

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return Id.ToString();
        }

        #endregion

    }

}
