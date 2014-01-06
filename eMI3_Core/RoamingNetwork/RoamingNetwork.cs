/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 Core <http://www.github.com/eMI3/Core>
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
using System.Collections.Generic;
using System.Collections.Concurrent;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Illias.Commons.Votes;
using eu.Vanaheimr.Styx.Arrows;

#endregion

namespace org.emi3group
{

    /// <summary>
    /// A roaming network is the collection of all entities to provide electric vehicle charging.
    /// </summary>
    public class RoamingNetwork : AEntity<RoamingNetwork_Id>,
                                  IEquatable<RoamingNetwork>, IComparable<RoamingNetwork>, IComparable,
                                  IEnumerable<IEntity>
    {

        #region Data

        private  readonly ConcurrentDictionary<EVSEOperator_Id,      EVSEOperator>      _EVSEOperators;
        private  readonly ConcurrentDictionary<EVServiceProvider_Id, EVServiceProvider> _EVServiceProviders;
        private  readonly ConcurrentDictionary<RoamingProvider_Id,   RoamingProvider>   _RoamingProviders;
        private  readonly ConcurrentDictionary<SearchProvider_Id,    SearchProvider>    _SearchProviders;

        #endregion

        #region Properties

        #region Name

        private I8NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the RoamingNetwork.
        /// </summary>
        [Mandatory]
        public I8NString Name
        {

            get
            {
                return _Name;
            }

            set
            {
                SetProperty<I8NString>(ref _Name, value);
            }

        }

        #endregion

        #region Description

        private I8NString _Description;

        /// <summary>
        /// An optional additional (multi-language) description of the RoamingNetwork.
        /// </summary>
        [Optional]
        public I8NString Description
        {

            get
            {
                return _Description;
            }

            set
            {
                SetProperty<I8NString>(ref _Description, value);
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
        public IEnumerable<SearchProvider> SearchProviders
        {
            get
            {
                return _SearchProviders.Values;
            }
        }

        #endregion

        #endregion

        #region Events

        // Roaming network events

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

        #region SearchProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, SearchProvider, Boolean> SearchProviderAddition;

        /// <summary>
        /// Called whenever an SearchProvider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, SearchProvider, Boolean> OnSearchProviderAddition
        {
            get
            {
                return SearchProviderAddition;
            }
        }

        #endregion


        // EVSE operator events

        #region EVSPoolAddition

        internal readonly IVotingNotificator<EVSEOperator, EVSPool, Boolean> EVSPoolAddition;

        /// <summary>
        /// Called whenever an EVS pool will be or was added.
        /// </summary>
        public IVotingSender<EVSEOperator, EVSPool, Boolean> OnEVSPoolAddition
        {
            get
            {
                return EVSPoolAddition;
            }
        }

        #endregion


        // EVS pool events

        #region ChargingStationAddition

        internal readonly IVotingNotificator<EVSPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<EVSPool, ChargingStation, Boolean> OnChargingStationAddition
        {
            get
            {
                return ChargingStationAddition;
            }
        }

        #endregion


        // Charging station events

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

        #region RoamingNetwork(Id)

        /// <summary>
        /// Create a new roaming network having the given
        /// unique roaming network identification.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        public RoamingNetwork(RoamingNetwork_Id Id)
            : base(Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The given unique roaming network identification must not be null!");

            #endregion

            #region Init data and properties

            this._EVSEOperators             = new ConcurrentDictionary<EVSEOperator_Id,      EVSEOperator>();
            this._EVServiceProviders        = new ConcurrentDictionary<EVServiceProvider_Id, EVServiceProvider>();
            this._RoamingProviders          = new ConcurrentDictionary<RoamingProvider_Id,   RoamingProvider>();
            this._SearchProviders           = new ConcurrentDictionary<SearchProvider_Id,    SearchProvider>();

            this.Name                       = new I8NString(Languages.en, Id.ToString());
            this.Description                = new I8NString();

            #endregion

            #region Init and link events

            this.EVSEOperatorAddition       = new VotingNotificator<RoamingNetwork,  EVSEOperator,      Boolean>(() => new VetoVote(), true);
            this.EVServiceProviderAddition  = new VotingNotificator<RoamingNetwork,  EVServiceProvider, Boolean>(() => new VetoVote(), true);
            this.RoamingProviderAddition    = new VotingNotificator<RoamingNetwork,  RoamingProvider,   Boolean>(() => new VetoVote(), true);
            this.SearchProviderAddition     = new VotingNotificator<RoamingNetwork,  SearchProvider,    Boolean>(() => new VetoVote(), true);

            // EVSE operator events
            this.EVSPoolAddition            = new VotingNotificator<EVSEOperator,    EVSPool,           Boolean>(() => new VetoVote(), true);

            // EVS pool events
            this.ChargingStationAddition    = new VotingNotificator<EVSPool,         ChargingStation,   Boolean>(() => new VetoVote(), true);

            // Charging station events
            this.EVSEAddition               = new VotingNotificator<ChargingStation, EVSE,              Boolean>(() => new VetoVote(), true);

            // EVSE events
            this.SocketOutletAddition       = new VotingNotificator<EVSE,            SocketOutlet,      Boolean>(() => new VetoVote(), true);

            #endregion

        }

        #endregion

        #endregion


        #region CreateNewEVSEOperator(EVSEOperator_Id, Action = null)

        /// <summary>
        /// Create and register a new EVSE operator having the given
        /// unique EVSE operator identification.
        /// </summary>
        /// <param name="EVSEOperator_Id">The unique identification of the new EVSE operator.</param>
        /// <param name="Action">An optional delegate to configure the new EVSE operator after its creation.</param>
        public EVSEOperator CreateNewEVSEOperator(EVSEOperator_Id       EVSEOperator_Id,
                                                  Action<EVSEOperator>  Action = null)
        {

            #region Initial checks

            if (EVSEOperator_Id == null)
                throw new ArgumentNullException("EVSEOperator_Id", "The given EVSE operator identification must not be null!");

            if (_EVSEOperators.ContainsKey(EVSEOperator_Id))
                throw new EVSEOperatorAlreadyExists(EVSEOperator_Id, this.Id);

            #endregion

            var _EVSEOperator = new EVSEOperator(EVSEOperator_Id, this);

            Action.FailSafeInvoke(_EVSEOperator);

            if (EVSEOperatorAddition.SendVoting(this, _EVSEOperator))
            {
                if (_EVSEOperators.TryAdd(EVSEOperator_Id, _EVSEOperator))
                {
                    EVSEOperatorAddition.SendNotification(this, _EVSEOperator);
                    return _EVSEOperator;
                }
            }

            throw new Exception();

        }

        #endregion

        #region CreateNewEVServiceProvider(EVServiceProvider_Id, Action = null)

        /// <summary>
        /// Create and register a new electric vehicle service provider having the given
        /// unique electric vehicle service provider identification.
        /// </summary>
        /// <param name="EVServiceProvider_Id">The unique identification of the new roaming provider.</param>
        /// <param name="Action">An optional delegate to configure the new roaming provider after its creation.</param>
        public EVServiceProvider CreateNewEVServiceProvider(EVServiceProvider_Id       EVServiceProvider_Id,
                                                            Action<EVServiceProvider>  Action = null)
        {

            #region Initial checks

            if (EVServiceProvider_Id == null)
                throw new ArgumentNullException("EVServiceProvider_Id", "The given electric vehicle service provider identification must not be null!");

            if (_EVServiceProviders.ContainsKey(EVServiceProvider_Id))
                throw new EVServiceProviderAlreadyExists(EVServiceProvider_Id, this.Id);

            #endregion

            var _EVServiceProvider = new EVServiceProvider(EVServiceProvider_Id, this);

            Action.FailSafeInvoke(_EVServiceProvider);

            if (EVServiceProviderAddition.SendVoting(this, _EVServiceProvider))
            {
                if (_EVServiceProviders.TryAdd(EVServiceProvider_Id, _EVServiceProvider))
                {
                    EVServiceProviderAddition.SendNotification(this, _EVServiceProvider);
                    return _EVServiceProvider;
                }
            }

            throw new Exception();

        }

        #endregion

        #region CreateNewRoamingProvider(RoamingProvider_Id, Action = null)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider having the given
        /// unique electric vehicle roaming provider identification.
        /// </summary>
        /// <param name="RoamingProvider_Id">The unique identification of the new roaming provider.</param>
        /// <param name="Action">An optional delegate to configure the new roaming provider after its creation.</param>
        public RoamingProvider CreateNewRoamingProvider(RoamingProvider_Id       RoamingProvider_Id,
                                                        Action<RoamingProvider>  Action = null)
        {

            #region Initial checks

            if (RoamingProvider_Id == null)
                throw new ArgumentNullException("RoamingProvider_Id", "The given roaming provider identification must not be null!");

            if (_RoamingProviders.ContainsKey(RoamingProvider_Id))
                throw new RoamingProviderAlreadyExists(RoamingProvider_Id, this.Id);

            #endregion

            var _RoamingProvider = new RoamingProvider(RoamingProvider_Id, this);

            Action.FailSafeInvoke(_RoamingProvider);

            if (RoamingProviderAddition.SendVoting(this, _RoamingProvider))
            {
                if (_RoamingProviders.TryAdd(RoamingProvider_Id, _RoamingProvider))
                {
                    RoamingProviderAddition.SendNotification(this, _RoamingProvider);
                    return _RoamingProvider;
                }
            }

            throw new Exception();

        }

        #endregion

        #region CreateNewSearchProvider(SearchProvider_Id, Action = null)

        /// <summary>
        /// Create and register a new charging station search provider having the given
        /// unique charging station search provider identification.
        /// </summary>
        /// <param name="SearchProvider_Id">The unique identification of the new search provider.</param>
        /// <param name="Action">An optional delegate to configure the new search provider after its creation.</param>
        public SearchProvider CreateNewSearchProvider(SearchProvider_Id       SearchProvider_Id,
                                                      Action<SearchProvider>  Action = null)
        {

            #region Initial checks

            if (SearchProvider_Id == null)
                throw new ArgumentNullException("SearchProvider_Id", "The given search provider identification must not be null!");

            if (_SearchProviders.ContainsKey(SearchProvider_Id))
                throw new SearchProviderAlreadyExists(SearchProvider_Id, this.Id);

            #endregion

            var _SearchProvider = new SearchProvider(SearchProvider_Id, this);

            Action.FailSafeInvoke(_SearchProvider);

            if (SearchProviderAddition.SendVoting(this, _SearchProvider))
            {
                if (_SearchProviders.TryAdd(SearchProvider_Id, _SearchProvider))
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
