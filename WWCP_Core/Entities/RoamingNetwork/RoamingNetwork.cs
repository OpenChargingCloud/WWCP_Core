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

        public static readonly TimeSpan MaxReservationDuration              = TimeSpan.FromMinutes(15);

        private readonly ConcurrentDictionary<EVSEOperator_Id,               EVSEOperator>               _EVSEOperators;
        private readonly ConcurrentDictionary<EVSP_Id,                       EVSP>                       _EVServiceProviders;
        private readonly ConcurrentDictionary<RoamingProvider_Id,            RoamingProvider>            _RoamingProviders;
        private readonly ConcurrentDictionary<NavigationServiceProvider_Id,  NavigationServiceProvider>  _SearchProviders;

        private readonly ConcurrentDictionary<ChargingReservation_Id,        ChargingReservation>        _ChargingReservations;

        private readonly ConcurrentDictionary<UInt32,                        IAuthServices>              _AuthenticationServices;
        private readonly ConcurrentDictionary<UInt32,                        IOperatorRoamingService>    _OperatorRoamingServices;

        private readonly ConcurrentDictionary<ChargingSession_Id,            ChargingSession>            _ChargingSessionCache;
        private readonly ConcurrentDictionary<ChargingSession_Id,            IAuthServices>              _SessionIdAuthenticatorCache;
        private readonly ConcurrentDictionary<ChargingSession_Id,            IOperatorRoamingService>    _SessionIdOperatorRoamingServiceCache;

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


        #region AllTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AllTokens
        {
            get
            {
                return _AuthenticationServices.SelectMany(vv => vv.Value.AllTokens);
            }
        }

        #endregion

        #region AuthorizedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AuthorizedTokens
        {
            get
            {
                return _AuthenticationServices.SelectMany(vv => vv.Value.AuthorizedTokens);
            }
        }

        #endregion

        #region NotAuthorizedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> NotAuthorizedTokens
        {
            get
            {
                return _AuthenticationServices.SelectMany(vv => vv.Value.NotAuthorizedTokens);
            }
        }

        #endregion

        #region BlockedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> BlockedTokens
        {
            get
            {
                return _AuthenticationServices.SelectMany(vv => vv.Value.BlockedTokens);
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


        #region EVSEOperators

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

        #region EVServiceProviders

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
                return _SearchProviders.Select(KVP => KVP.Value);
            }
        }

        #endregion


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

        #region ChargingReservations

        /// <summary>
        /// Return all charging reservations registered within this roaming network.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
        {
            get
            {
                return _ChargingReservations.Select(kvp => kvp.Value);
            }
        }

        #endregion

        #endregion

        #region Events

        // RoamingNetwork events

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


        // ChargingPool events

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

        #region OnChargingPoolReserved

        /// <summary>
        /// A delegate called whenever a charging pool was reserved.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this reservation was created.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public delegate void OnChargingPoolReservedDelegate(DateTime Timestamp, ChargingReservation Reservation);

        /// <summary>
        /// An event fired whenever a charging pool was reserved.
        /// </summary>
        public event OnChargingPoolReservedDelegate OnChargingPoolReserved;

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


        // ChargingStation events

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

        #region OnChargingStationReserved

        /// <summary>
        /// A delegate called whenever a charging station was reserved.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this reservation was created.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public delegate void OnChargingStationReservedDelegate(DateTime Timestamp, ChargingReservation Reservation);

        /// <summary>
        /// An event fired whenever a charging station was reserved.
        /// </summary>
        public event OnChargingStationReservedDelegate OnChargingStationReserved;

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


        // EVSE events

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

        #region OnEVSEReserved

        /// <summary>
        /// A delegate called whenever an EVSE was reserved.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this reservation was created.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public delegate void OnEVSEReservedDelegate(DateTime Timestamp, ChargingReservation Reservation);

        /// <summary>
        /// An event fired whenever an EVSE was reserved.
        /// </summary>
        public event OnEVSEReservedDelegate OnEVSEReserved;

        #endregion


        // Reservation events

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


        // Charging log events

        #region OnRemoteEVSEStart / OnRemoteEVSEStarted

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteEVSEStartDelegate OnRemoteEVSEStart;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteEVSEStartedDelegate OnRemoteEVSEStarted;

        #endregion

        #region OnRemoteChargingStationStart / OnRemoteChargingStationStarted

        /// <summary>
        /// An event fired whenever a remote start charging station command was received.
        /// </summary>
        public event OnRemoteChargingStationStartDelegate OnRemoteChargingStationStart;

        /// <summary>
        /// An event fired whenever a remote start charging station command completed.
        /// </summary>
        public event OnRemoteChargingStationStartedDelegate OnRemoteChargingStationStarted;

        #endregion


        #region OnRemoteStop / OnRemoteStopped

        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopDelegate OnRemoteStop;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStoppedDelegate OnRemoteStopped;

        #endregion

        #region OnRemoteEVSEStop / OnRemoteEVSEStopped

        /// <summary>
        /// An event fired whenever a remote stop EVSE command was received.
        /// </summary>
        public event OnRemoteEVSEStopDelegate OnRemoteEVSEStop;

        /// <summary>
        /// An event fired whenever a remote stop EVSE command completed.
        /// </summary>
        public event OnRemoteEVSEStoppedDelegate OnRemoteEVSEStopped;

        #endregion

        #region OnRemoteChargingStationStop / OnRemoteChargingStationStopped

        /// <summary>
        /// An event fired whenever a remote stop charging station command was received.
        /// </summary>
        public event OnRemoteChargingStationStopDelegate OnRemoteChargingStationStop;

        /// <summary>
        /// An event fired whenever a remote stop charging station command completed.
        /// </summary>
        public event OnRemoteChargingStationStoppedDelegate OnRemoteChargingStationStopped;

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


        #region OnSendCDR / OnCDRSent

        /// <summary>
        /// An event fired whenever a charge detail record was received.
        /// </summary>
        public event SendCDRDelegate OnSendCDR;

        /// <summary>
        /// An event fired whenever a charge detail record result was sent.
        /// </summary>
        public event CDRSentDelegate OnCDRSent;

        #endregion


        #region OnFilterCDRRecords

        public delegate SendCDRResult OnFilterCDRRecordsDelegate(Authorizator_Id AuthorizatorId, AuthInfo AuthInfo);

        /// <summary>
        /// An event fired whenever a CDR needs to be filtered.
        /// </summary>
        public event OnFilterCDRRecordsDelegate OnFilterCDRRecords;

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

            this._AuthorizatorId                        = (AuthorizatorId == null) ? Authorizator_Id.Parse("GraphDefined E-Mobility Gateway") : AuthorizatorId;
            this._Description                           = new I18NString();

            this._EVSEOperators                         = new ConcurrentDictionary<EVSEOperator_Id,              EVSEOperator>();
            this._EVServiceProviders                    = new ConcurrentDictionary<EVSP_Id,                      EVSP>();
            this._RoamingProviders                      = new ConcurrentDictionary<RoamingProvider_Id,           RoamingProvider>();
            this._SearchProviders                       = new ConcurrentDictionary<NavigationServiceProvider_Id, NavigationServiceProvider>();
            this._ChargingReservations                  = new ConcurrentDictionary<ChargingReservation_Id,       ChargingReservation>();
            this._AuthenticationServices                = new ConcurrentDictionary<UInt32,                       IAuthServices>();
            this._OperatorRoamingServices               = new ConcurrentDictionary<UInt32,                       IOperatorRoamingService>();
            this._ChargingSessionCache                  = new ConcurrentDictionary<ChargingSession_Id,           ChargingSession>();
            this._SessionIdAuthenticatorCache           = new ConcurrentDictionary<ChargingSession_Id,           IAuthServices>();
            this._SessionIdOperatorRoamingServiceCache  = new ConcurrentDictionary<ChargingSession_Id,           IOperatorRoamingService>();

            #endregion

            #region Init events

            // RoamingNetwork events
            this.EVSEOperatorAddition       = new VotingNotificator<DateTime, RoamingNetwork,  EVSEOperator,              Boolean>(() => new VetoVote(), true);
            this.EVSEOperatorRemoval        = new VotingNotificator<DateTime, RoamingNetwork,  EVSEOperator,              Boolean>(() => new VetoVote(), true);

            this.EVServiceProviderAddition  = new VotingNotificator<RoamingNetwork,  EVSP,         Boolean>(() => new VetoVote(), true);
            this.EVServiceProviderRemoval   = new VotingNotificator<RoamingNetwork,  EVSP,         Boolean>(() => new VetoVote(), true);

            this.RoamingProviderAddition    = new VotingNotificator<RoamingNetwork,  RoamingProvider,           Boolean>(() => new VetoVote(), true);
            this.RoamingProviderRemoval     = new VotingNotificator<RoamingNetwork,  RoamingProvider,           Boolean>(() => new VetoVote(), true);

            this.SearchProviderAddition     = new VotingNotificator<RoamingNetwork,  NavigationServiceProvider, Boolean>(() => new VetoVote(), true);
            this.SearchProviderRemoval      = new VotingNotificator<RoamingNetwork,  NavigationServiceProvider, Boolean>(() => new VetoVote(), true);

            // EVSEOperator events
            this.ChargingPoolAddition       = new VotingNotificator<DateTime, EVSEOperator,    ChargingPool,              Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval        = new VotingNotificator<DateTime, EVSEOperator,    ChargingPool,              Boolean>(() => new VetoVote(), true);

            // ChargingPool events
            this.ChargingStationAddition    = new VotingNotificator<DateTime, ChargingPool,    ChargingStation,           Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval     = new VotingNotificator<DateTime, ChargingPool,    ChargingStation,           Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition               = new VotingNotificator<DateTime, ChargingStation, EVSE,                      Boolean>(() => new VetoVote(), true);
            this.EVSERemoval                = new VotingNotificator<DateTime, ChargingStation, EVSE,                      Boolean>(() => new VetoVote(), true);

            // EVSE events
            this.SocketOutletAddition       = new VotingNotificator<DateTime, EVSE,            SocketOutlet,              Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval        = new VotingNotificator<DateTime, EVSE,            SocketOutlet,              Boolean>(() => new VetoVote(), true);

            #endregion

        }

        #endregion

        #endregion


        #region EVSE operator methods

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
                                                  I18NString                               Name           = null,
                                                  I18NString                               Description    = null,
                                                  Action<EVSEOperator>                     Configurator   = null,
                                                  Action<EVSEOperator>                     OnSuccess      = null,
                                                  Action<RoamingNetwork, EVSEOperator_Id>  OnError        = null)
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

                    _EVSEOperator.OnEVSEDataChanged                             += (Timestamp, EVSE, PropertyName, OldValue, NewValue)
                                                                                    => UpdateEVSEData(Timestamp, EVSE, PropertyName, OldValue, NewValue);

                    _EVSEOperator.OnEVSEStatusChanged                           += (Timestamp, EVSE, OldStatus, NewStatus)
                                                                                    => UpdateEVSEStatus(Timestamp, EVSE, OldStatus, NewStatus);

                    _EVSEOperator.OnEVSEAdminStatusChanged                      += (Timestamp, EVSE, OldStatus, NewStatus)
                                                                                    => UpdateEVSEAdminStatus(Timestamp, EVSE, OldStatus, NewStatus);


                    _EVSEOperator.OnChargingStationDataChanged                  += (Timestamp, ChargingStation, PropertyName, OldValue, NewValue)
                                                                                    => UpdateChargingStationData(Timestamp, ChargingStation, PropertyName, OldValue, NewValue);

                    _EVSEOperator.OnChargingStationAdminStatusChanged           += (Timestamp, ChargingStation, OldStatus, NewStatus)
                                                                                    => UpdateChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus);

                    _EVSEOperator.OnAggregatedChargingStationStatusChanged      += (Timestamp, ChargingStation, OldStatus, NewStatus)
                                                                                    => UpdateAggregatedChargingStationStatus(Timestamp, ChargingStation, OldStatus, NewStatus);

                    _EVSEOperator.OnAggregatedChargingStationAdminStatusChanged += (Timestamp, ChargingStation, OldStatus, NewStatus)
                                                                                    => UpdateChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus);


                    _EVSEOperator.OnChargingPoolDataChanged                     += (Timestamp, ChargingPool, PropertyName, OldValue, NewValue)
                                                                                    => UpdateChargingPoolData(Timestamp, ChargingPool, PropertyName, OldValue, NewValue);

                    _EVSEOperator.OnChargingPoolAdminStatusChanged              += (Timestamp, ChargingPool, OldStatus, NewStatus)
                                                                                    => UpdateChargingPoolAdminStatus(Timestamp, ChargingPool, OldStatus, NewStatus);

                    _EVSEOperator.OnAggregatedChargingPoolStatusChanged         += (Timestamp, ChargingPool, OldStatus, NewStatus)
                                                                                    => UpdateChargingPoolStatus(Timestamp, ChargingPool, OldStatus, NewStatus);

                    _EVSEOperator.OnAggregatedChargingPoolAdminStatusChanged    += (Timestamp, ChargingPool, OldStatus, NewStatus)
                                                                                    => UpdateChargingPoolAdminStatus(Timestamp, ChargingPool, OldStatus, NewStatus);


                    _EVSEOperator.OnPropertyChanged                             += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                                                                                    => UpdateEVSEOperatorData(Timestamp, Sender as EVSEOperator, PropertyName, OldValue, NewValue);

                    _EVSEOperator.OnAggregatedStatusChanged                     += (Timestamp, EVSEOperator, OldStatus, NewStatus)
                                                                                    => UpdateStatus(Timestamp, EVSEOperator, OldStatus, NewStatus);

                    _EVSEOperator.OnAggregatedAdminStatusChanged                += (Timestamp, EVSEOperator, OldStatus, NewStatus)
                                                                                    => UpdateAdminStatus(Timestamp, EVSEOperator, OldStatus, NewStatus);


                    OnSuccess.FailSafeInvoke(_EVSEOperator);
                    EVSEOperatorAddition.SendNotification(DateTime.Now, this, _EVSEOperator);
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

        #region CreateNewRoamingProvider(OperatorRoamingService, Configurator = null)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider having the given
        /// unique electric vehicle roaming provider identification.
        /// </summary>
        /// <param name="OperatorRoamingService">The attached E-Mobility service.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public RoamingProvider CreateNewRoamingProvider(IOperatorRoamingService  OperatorRoamingService,
                                                        Action<RoamingProvider>  Configurator = null)
        {

            #region Initial checks

            if (OperatorRoamingService.Id == null)
                throw new ArgumentNullException("OperatorRoamingService.Id",    "The given roaming provider identification must not be null!");

            if (OperatorRoamingService.Name.IsNullOrEmpty())
                throw new ArgumentNullException("OperatorRoamingService.Name",  "The given roaming provider name must not be null or empty!");

            if (_RoamingProviders.ContainsKey(OperatorRoamingService.Id))
                throw new RoamingProviderAlreadyExists(OperatorRoamingService.Id, this.Id);

            if (OperatorRoamingService.RoamingNetworkId != this.Id)
                throw new ArgumentException("The given operator roaming service is not part of this roaming network!", "OperatorRoamingService");

            #endregion

            var _RoamingProvider = new RoamingProvider(OperatorRoamingService.Id,
                                                       OperatorRoamingService.Name,
                                                       this,
                                                       OperatorRoamingService,
                                                       null);

            Configurator.FailSafeInvoke(_RoamingProvider);

            if (RoamingProviderAddition.SendVoting(this, _RoamingProvider))
            {
                if (_RoamingProviders.TryAdd(OperatorRoamingService.Id, _RoamingProvider))
                {

                    RoamingProviderAddition.SendNotification(this, _RoamingProvider);

                    return _RoamingProvider;

                }
            }

            throw new Exception("Could not create new roaming provider '" + OperatorRoamingService.Id + "'!");

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


        #region EVSE methods

        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the roaming network.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(EVSE.ChargingStation.ChargingPool.EVSEOperator.Id, out _EVSEOperator))
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


        #region ReserveEVSE(...)

        public async Task<ReservationResult> ReserveEVSE(DateTime                 Timestamp,
                                                         CancellationToken        CancellationToken,
                                                         EVSP_Id                  ProviderId,
                                                         ChargingReservation_Id   ReservationId,
                                                         DateTime?                StartTime,
                                                         TimeSpan?                Duration,
                                                         EVSE_Id                  EVSEId,
                                                         ChargingProduct_Id       ChargingProductId  = null,
                                                         IEnumerable<Auth_Token>  RFIDIds            = null,
                                                         IEnumerable<eMA_Id>      eMAIds             = null,
                                                         IEnumerable<UInt32>      PINs               = null)
        {

            #region Try to remove an existing reservation if this is an update!

            if (ReservationId != null)
            {

                ChargingReservation _Reservation = null;

                if (!_ChargingReservations.TryRemove(ReservationId, out _Reservation))
                    return ReservationResult.UnknownChargingReservationId;

            }

            #endregion

            EVSE _EVSE;

            if (!TryGetEVSEbyId(EVSEId, out _EVSE))
                return ReservationResult.UnknownEVSE;

            var result = await _EVSE.Reserve(Timestamp,
                                             CancellationToken,
                                             ProviderId,
                                             ReservationId,
                                             StartTime,
                                             Duration,
                                             ChargingProductId,
                                             RFIDIds,
                                             eMAIds,
                                             PINs);

            if (result.Result == ReservationResultType.Success)
                _ChargingReservations.TryAdd(result.Reservation.Id, result.Reservation);

            return result;

        }

        #endregion


        #region SetEVSEStatus(EVSEId, StatusList)

        public void SetEVSEStatus(EVSE_Id                                   EVSEId,
                                  IEnumerable<Timestamped<EVSEStatusType>>  StatusList)
        {

            EVSEOperator _EVSEOperator = null;

            if (TryGetEVSEOperatorbyId(EVSE_Id.Parse(EVSEId.ToString()).OperatorId, out _EVSEOperator))
                _EVSEOperator.SetEVSEStatus(EVSEId, StatusList);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, StatusList)

        public void SetEVSEAdminStatus(EVSE_Id                                        EVSEId,
                                       IEnumerable<Timestamped<EVSEAdminStatusType>>  StatusList)
        {

            EVSEOperator _EVSEOperator = null;

            if (TryGetEVSEOperatorbyId(EVSE_Id.Parse(EVSEId.ToString()).OperatorId, out _EVSEOperator))
                _EVSEOperator.SetEVSEAdminStatus(EVSEId, StatusList);

        }

        #endregion

        #endregion

        #region ChargingStation methods

        #region ContainsChargingStation(ChargingStation)

        /// <summary>
        /// Check if the given charging station is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation ChargingStation)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingStation.ChargingPool.EVSEOperator.Id, out _EVSEOperator))
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


        #region ReserveChargingStation(...)

        public async Task<ReservationResult> ReserveChargingStation(DateTime                 Timestamp,
                                                                    CancellationToken        CancellationToken,
                                                                    EVSP_Id                  ProviderId,
                                                                    ChargingReservation_Id   ReservationId,
                                                                    DateTime?                StartTime,
                                                                    TimeSpan?                Duration,
                                                                    ChargingStation_Id       ChargingStationId,
                                                                    ChargingProduct_Id       ChargingProductId  = null,
                                                                    IEnumerable<Auth_Token>  RFIDIds            = null,
                                                                    IEnumerable<eMA_Id>      eMAIds             = null,
                                                                    IEnumerable<UInt32>      PINs               = null)
        {

            #region Try to remove an existing reservation if this is an update!

            if (ReservationId != null)
            {

                ChargingReservation _Reservation = null;

                if (!_ChargingReservations.TryRemove(ReservationId, out _Reservation))
                    return ReservationResult.UnknownChargingReservationId;

            }

            #endregion

            ChargingStation _ChargingStation;

            if (!TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
                return ReservationResult.UnknownChargingStation;

            // Find a matching EVSE within the given charging station
            var _EVSE = _ChargingStation.EVSEs.
                            Where  (evse => evse.Status.Value == EVSEStatusType.Available).
                            OrderBy(evse => evse.Id).
                            FirstOrDefault();

            if (_EVSE != null)
            {

                var _Reservation = new ChargingReservation(Timestamp,
                                                           StartTime.HasValue ? StartTime.Value : DateTime.Now,
                                                           Duration. HasValue ? Duration. Value : MaxReservationDuration,
                                                           ProviderId,
                                                           ChargingReservationType.AtChargingStation,
                                                           _EVSE.ChargingStation.ChargingPool.EVSEOperator.RoamingNetwork,
                                                           _EVSE.ChargingStation.ChargingPool.Id,
                                                           _EVSE.ChargingStation.Id,
                                                           _EVSE.Id,
                                                           ChargingProductId,
                                                           RFIDIds,
                                                           eMAIds,
                                                           PINs);

                _EVSE.Reservation = _ChargingReservations.AddAndReturnValue(_Reservation.Id, _Reservation);

                return ReservationResult.Success(_Reservation);

            }

            return ReservationResult.NoEVSEsAvailable;

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

        #endregion

        #region ChargingPool methods

        #region ContainsChargingPool(ChargingPool)

        /// <summary>
        /// Check if the given charging pool is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public Boolean ContainsChargingPool(ChargingPool ChargingPool)
        {

            EVSEOperator _EVSEOperator  = null;

            if (TryGetEVSEOperatorbyId(ChargingPool.EVSEOperator.Id, out _EVSEOperator))
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


        #region ReserveChargingPool(...)

        public async Task<ReservationResult> ReserveChargingPool(DateTime                 Timestamp,
                                                                 CancellationToken        CancellationToken,
                                                                 EVSP_Id                  ProviderId,
                                                                 ChargingReservation_Id   ReservationId,
                                                                 DateTime?                StartTime,
                                                                 TimeSpan?                Duration,
                                                                 ChargingPool_Id          ChargingPoolId,
                                                                 ChargingProduct_Id       ChargingProductId  = null,
                                                                 IEnumerable<Auth_Token>  RFIDIds            = null,
                                                                 IEnumerable<eMA_Id>      eMAIds             = null,
                                                                 IEnumerable<UInt32>      PINs               = null)
        {

            #region Try to remove an existing reservation if this is an update!

            if (ReservationId != null)
            {

                ChargingReservation _Reservation = null;

                if (!_ChargingReservations.TryRemove(ReservationId, out _Reservation))
                    return ReservationResult.UnknownChargingReservationId;

            }

            #endregion

            ChargingPool _ChargingPool;

            if (!TryGetChargingPoolbyId(ChargingPoolId, out _ChargingPool))
                return ReservationResult.UnknownChargingPool;

            // Find a matching EVSE within the given charging pool
            var _EVSE = _ChargingPool.EVSEs.
                            Where  (evse => evse.Status.Value == EVSEStatusType.Available).
                            OrderBy(evse => evse.Id).
                            FirstOrDefault();

            if (_EVSE != null)
            {

                var _Reservation = new ChargingReservation(Timestamp,
                                                           StartTime.HasValue ? StartTime.Value : DateTime.Now,
                                                           Duration. HasValue ? Duration. Value : MaxReservationDuration,
                                                           ProviderId,
                                                           ChargingReservationType.AtChargingPool,
                                                           _EVSE.ChargingStation.ChargingPool.EVSEOperator.RoamingNetwork,
                                                           _EVSE.ChargingStation.ChargingPool.Id,
                                                           _EVSE.ChargingStation.Id,
                                                           _EVSE.Id,
                                                           ChargingProductId,
                                                           RFIDIds,
                                                           eMAIds,
                                                           PINs);

                _EVSE.Reservation = _ChargingReservations.AddAndReturnValue(_Reservation.Id, _Reservation);

                return ReservationResult.Success(_Reservation);

            }

            return ReservationResult.NoEVSEsAvailable;

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

        #endregion

        #region Reservation methods

        #region TryGetChargingReservationbyId(ChargingReservationId, out ChargingReservation)

        public Boolean TryGetChargingReservationbyId(ChargingReservation_Id ChargingReservationId, out ChargingReservation ChargingReservation)
        {
            return _ChargingReservations.TryGetValue(ChargingReservationId, out ChargingReservation);
        }

        #endregion

        #region Remove(ChargingReservation)

        public Boolean Remove(ChargingReservation_Id ChargingReservationId)
        {

            ChargingReservation _ChargingReservation;

            if (_ChargingReservations.TryRemove(ChargingReservationId, out _ChargingReservation))
            {

                EVSE _EVSE = null;

                if (TryGetEVSEbyId(_ChargingReservation.EVSEId, out _EVSE))
                    _EVSE.Reservation = null;

                return true;

            }

            return false;

        }

        #endregion

        #endregion



        #region RegisterAuthService(Priority, AuthenticationService)

        /// <summary>
        /// Register the given authentication service.
        /// </summary>
        /// <param name="Priority">The priority of the service.</param>
        /// <param name="AuthenticationService">The authentication service.</param>
        public Boolean RegisterAuthService(UInt32         Priority,
                                           IAuthServices  AuthenticationService)
        {

            return _AuthenticationServices.TryAdd(Priority, AuthenticationService);

        }

        #endregion

        #region RegisterAuthService(Priority, OperatorRoamingService)

        /// <summary>
        /// Register the given EVSE operator roaming service.
        /// </summary>
        /// <param name="Priority">The priority of the service.</param>
        /// <param name="OperatorRoamingService">The EVSE operator roaming service.</param>
        public Boolean RegisterAuthService(UInt32                   Priority,
                                           IOperatorRoamingService  OperatorRoamingService)
        {

            return _OperatorRoamingServices.TryAdd(Priority, OperatorRoamingService);

        }

        #endregion


        #region AuthorizeStart(Timestamp, CancellationToken, OperatorId, AuthToken, ChargingProductId = null, SessionId = null)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(DateTime            Timestamp,
                           CancellationToken   CancellationToken,
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

            #region Send log event

            var OnAuthorizeStartLocal = OnAuthorizeStart;
            if (OnAuthorizeStartLocal != null)
                OnAuthorizeStartLocal(this,
                                      Timestamp,
                                      Id,
                                      OperatorId,
                                      AuthToken,
                                      ChargingProductId,
                                      SessionId);

            #endregion


            AuthStartResult result = null;

            foreach (var AuthenticationService in _AuthenticationServices.
                                                      OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                      Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            {

                result = await AuthenticationService.AuthorizeStart(OperatorId,
                                                                    AuthToken,
                                                                    ChargingProductId,
                                                                    SessionId,
                                                                    QueryTimeout);


                #region Authorized

                if (result.AuthorizationResult == AuthStartResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    _SessionIdAuthenticatorCache.TryAdd(result.SessionId, AuthenticationService);

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

                result = await OperatorRoamingService.AuthorizeStart(OperatorId,
                                                                     AuthToken,
                                                                     ChargingProductId,
                                                                     SessionId,
                                                                     QueryTimeout);


                #region Authorized

                if (result.AuthorizationResult == AuthStartResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    _SessionIdOperatorRoamingServiceCache.TryAdd(result.SessionId, OperatorRoamingService);

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


            #region Send log event

            var OnAuthorizeStartedLocal = OnAuthorizeStarted;
            if (OnAuthorizeStartedLocal != null)
                OnAuthorizeStartedLocal(this,
                                        DateTime.Now,
                                        Id,
                                        result);

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(Timestamp, CancellationToken, OperatorId, AuthToken, EVSEId, ChargingProductId = null, SessionId = null)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(DateTime            Timestamp,
                           CancellationToken   CancellationToken,
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
                throw new ArgumentNullException("EVSEId",     "The given EVSE identification must not be null!");

            #endregion

            #region Send log event

            var OnAuthorizeEVSEStartLocal = OnAuthorizeEVSEStart;
            if (OnAuthorizeEVSEStartLocal != null)
                OnAuthorizeEVSEStartLocal(this,
                                          Timestamp,
                                          Id,
                                          OperatorId,
                                          AuthToken,
                                          EVSEId,
                                          ChargingProductId,
                                          SessionId);

            #endregion


            AuthStartEVSEResult result = null;

            foreach (var AuthenticationService in _AuthenticationServices.
                                                      OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                      Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            {

                result = await AuthenticationService.AuthorizeStart(OperatorId,
                                                                    AuthToken,
                                                                    EVSEId,
                                                                    ChargingProductId,
                                                                    SessionId,
                                                                    QueryTimeout);


                #region Authorized

                if (result.AuthorizationResult == AuthStartEVSEResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    _SessionIdAuthenticatorCache.TryAdd(result.SessionId, AuthenticationService);

                    break;

                }

                #endregion

                #region Blocked

                else if (result.AuthorizationResult == AuthStartEVSEResultType.Blocked)
                    break;

                #endregion

            }

            foreach (var OperatorRoamingService in _OperatorRoamingServices.
                                                       OrderBy(OperatorRoamingServiceWithPriority => OperatorRoamingServiceWithPriority.Key).
                                                       Select (OperatorRoamingServiceWithPriority => OperatorRoamingServiceWithPriority.Value))
            {

                result = await OperatorRoamingService.AuthorizeStart(OperatorId,
                                                                     AuthToken,
                                                                     EVSEId,
                                                                     ChargingProductId,
                                                                     SessionId,
                                                                     QueryTimeout);


                #region Authorized

                if (result.AuthorizationResult == AuthStartEVSEResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    _SessionIdOperatorRoamingServiceCache.TryAdd(result.SessionId, OperatorRoamingService);

                    break;

                }

                #endregion

                #region Blocked

                else if (result.AuthorizationResult == AuthStartEVSEResultType.Blocked)
                    break;

                #endregion

            }

            #region ...or fail!

            if (result == null)
                result =  AuthStartEVSEResult.Error(AuthorizatorId,
                                                    "No authorization service returned a positiv result!");

            #endregion


            #region Send log event

            var OnAuthorizeEVSEStartedLocal = OnAuthorizeEVSEStarted;
            if (OnAuthorizeEVSEStartedLocal != null)
                OnAuthorizeEVSEStartedLocal(this,
                                            DateTime.Now,
                                            Id,
                                            EVSEId,
                                            result);

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(Timestamp, CancellationToken, OperatorId, AuthToken, ChargingStationId, ChargingProductId = null, SessionId = null)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification charging station.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(DateTime            Timestamp,
                           CancellationToken   CancellationToken,
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
                throw new ArgumentNullException("ChargingStationId",  "The given charging station identification must not be null!");

            #endregion

            #region Send log event

            var OnAuthorizeChargingStationStartLocal = OnAuthorizeChargingStationStart;
            if (OnAuthorizeChargingStationStartLocal != null)
                OnAuthorizeChargingStationStartLocal(this,
                                                     Timestamp,
                                                     Id,
                                                     OperatorId,
                                                     AuthToken,
                                                     ChargingStationId,
                                                     ChargingProductId,
                                                     SessionId);

            #endregion


            AuthStartChargingStationResult result = null;

            foreach (var AuthenticationService in _AuthenticationServices.
                                                      OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                      Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            {

                result = await AuthenticationService.AuthorizeStart(OperatorId,
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
                    _SessionIdAuthenticatorCache.TryAdd(result.SessionId, AuthenticationService);

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

                result = await OperatorRoamingService.AuthorizeStart(OperatorId,
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
                    _SessionIdOperatorRoamingServiceCache.TryAdd(result.SessionId, OperatorRoamingService);

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


            #region Send log event

            var OnAuthorizeChargingStationStartedLocal = OnAuthorizeChargingStationStarted;
            if (OnAuthorizeChargingStationStartedLocal != null)
                OnAuthorizeChargingStationStartedLocal(this,
                                                       DateTime.Now,
                                                       Id,
                                                       ChargingStationId,
                                                       result);

            #endregion

            return result;

        }

        #endregion


        #region AuthorizeStop(Timestamp, CancellationToken, OperatorId, SessionId, AuthToken)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
                          EVSEOperator_Id     OperatorId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException("SessionId",  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            #region Send log event

            var OnAuthorizeStopLocal = OnAuthorizeStop;
            if (OnAuthorizeStopLocal != null)
                OnAuthorizeStopLocal(this,
                                     Timestamp,
                                     Id,
                                     OperatorId,
                                     SessionId,
                                     AuthToken);

            #endregion


            AuthStopResult result = null;

            #region An authenticator was found for the upstream SessionId!

            IAuthServices AuthenticationService;

            if (_SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
            {

                result = await AuthenticationService.AuthorizeStop(OperatorId, SessionId, AuthToken);

                //ToDo: Delete the session id from the cache?

            }

            IOperatorRoamingService OperatorRoamingService;

            if (_SessionIdOperatorRoamingServiceCache.TryGetValue(SessionId, out OperatorRoamingService))
            {

                result = await OperatorRoamingService.AuthorizeStop(OperatorId, SessionId, AuthToken);

                //ToDo: Delete the session id from the cache?

            }

            #endregion

            #region Try to find anyone who might kown anything about the given SessionId!

            if (result == null || result.AuthorizationResult != AuthStopResultType.Authorized)
                foreach (var OtherAuthenticationService in _AuthenticationServices.
                                                               OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                               Select(AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                               ToArray())
                {

                    result = await OtherAuthenticationService.AuthorizeStop(OperatorId,
                                                                            SessionId,
                                                                            AuthToken,
                                                                            QueryTimeout);

                    if (result.AuthorizationResult == AuthStopResultType.Authorized)
                        break;

                }

            if (result == null || result.AuthorizationResult != AuthStopResultType.Authorized)
                foreach (var OtherOperatorRoamingServices in _OperatorRoamingServices.
                                                                 OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                                 Select(AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                                 ToArray())
                {

                    result = await OtherOperatorRoamingServices.AuthorizeStop(OperatorId,
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


            #region Send log event

            var OnAuthorizeStoppedLocal = OnAuthorizeStopped;
            if (OnAuthorizeStoppedLocal != null)
                OnAuthorizeStoppedLocal(this,
                                        DateTime.Now,
                                        Id,
                                        result);

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(Timestamp, CancellationToken, OperatorId, SessionId, AuthToken, EVSEId)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
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
                throw new ArgumentNullException("SessionId",  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            if (EVSEId == null)
                throw new ArgumentNullException("EVSEId",     "The given parameter must not be null!");

            #endregion

            #region Send log event

            var OnAuthorizeEVSEStopLocal = OnAuthorizeEVSEStop;
            if (OnAuthorizeEVSEStopLocal != null)
                OnAuthorizeEVSEStopLocal(this,
                                         Timestamp,
                                         Id,
                                         OperatorId,
                                         EVSEId,
                                         SessionId,
                                         AuthToken);

            #endregion


            AuthStopEVSEResult result = null;

            #region An authenticator was found for the upstream SessionId!

            IAuthServices AuthenticationService;

            if (_SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
            {

                result = await AuthenticationService.AuthorizeStop(OperatorId, EVSEId, SessionId, AuthToken);

                //ToDo: Delete the session id from the cache?

            }

            IOperatorRoamingService OperatorRoamingService;

            if (_SessionIdOperatorRoamingServiceCache.TryGetValue(SessionId, out OperatorRoamingService))
            {

                result = await OperatorRoamingService.AuthorizeStop(OperatorId, EVSEId, SessionId, AuthToken);

                //ToDo: Delete the session id from the cache?

            }

            #endregion

            #region Try to find anyone who might kown anything about the given SessionId!

            if (result == null || result.AuthorizationResult != AuthStopEVSEResultType.Authorized)
                foreach (var OtherAuthenticationService in _AuthenticationServices.
                                                               OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                               Select (AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                               ToArray())
                {

                    result = await OtherAuthenticationService.AuthorizeStop(OperatorId,
                                                                            EVSEId,
                                                                            SessionId,
                                                                            AuthToken,
                                                                            QueryTimeout);

                    if (result.AuthorizationResult == AuthStopEVSEResultType.Authorized)
                        break;

                }

            if (result == null || result.AuthorizationResult != AuthStopEVSEResultType.Authorized)
                foreach (var OtherOperatorRoamingServices in _OperatorRoamingServices.
                                                                 OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                                 Select(AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                                 ToArray())
                {

                    result = await OtherOperatorRoamingServices.AuthorizeStop(OperatorId,
                                                                              EVSEId,
                                                                              SessionId,
                                                                              AuthToken,
                                                                              QueryTimeout);

                    if (result.AuthorizationResult == AuthStopEVSEResultType.Authorized)
                        break;

                }

            #endregion

            #region ...or fail!

            if (result == null)
                result = AuthStopEVSEResult.Error(AuthorizatorId,
                                                  "No authorization service returned a positiv result!");

            #endregion


            #region Send log event

            var OnAuthorizeEVSEStoppedLocal = OnAuthorizeEVSEStopped;
            if (OnAuthorizeEVSEStoppedLocal != null)
                OnAuthorizeEVSEStoppedLocal(this,
                                            DateTime.Now,
                                            Id,
                                            EVSEId,
                                            result);

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(Timestamp, CancellationToken, OperatorId, SessionId, AuthToken, EVSEId = null)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
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
                throw new ArgumentNullException("SessionId",          "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",          "The given parameter must not be null!");

            if (ChargingStationId == null)
                throw new ArgumentNullException("ChargingStationId",  "The given parameter must not be null!");

            #endregion

            #region Send log event

            var OnAuthorizeChargingStationStopLocal = OnAuthorizeChargingStationStop;
            if (OnAuthorizeChargingStationStopLocal != null)
                OnAuthorizeChargingStationStopLocal(this,
                                                    Timestamp,
                                                    Id,
                                                    OperatorId,
                                                    ChargingStationId,
                                                    SessionId,
                                                    AuthToken);

            #endregion


            AuthStopChargingStationResult result = null;

            #region An authenticator was found for the upstream SessionId!

            IAuthServices AuthenticationService;

            if (_SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
            {

                result = await AuthenticationService.AuthorizeStop(OperatorId, ChargingStationId, SessionId, AuthToken);

                //ToDo: Delete the session id from the cache?

            }

            IOperatorRoamingService OperatorRoamingService;

            if (_SessionIdOperatorRoamingServiceCache.TryGetValue(SessionId, out OperatorRoamingService))
            {

                result = await OperatorRoamingService.AuthorizeStop(OperatorId, ChargingStationId, SessionId, AuthToken);

                //ToDo: Delete the session id from the cache?

            }

            #endregion

            #region Try to find anyone who might kown anything about the given SessionId!

            if (result == null || result.AuthorizationResult != AuthStopChargingStationResultType.Authorized)
                foreach (var OtherAuthenticationService in _AuthenticationServices.
                                                               OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                               Select (AuthServiceWithPriority => AuthServiceWithPriority.Value).
                                                               ToArray())
                {

                    result = await OtherAuthenticationService.AuthorizeStop(OperatorId,
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

                    result = await OtherOperatorRoamingServices.AuthorizeStop(OperatorId,
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


            #region Send log event

            var OnAuthorizeChargingStationStoppedLocal = OnAuthorizeChargingStationStopped;
            if (OnAuthorizeChargingStationStoppedLocal != null)
                OnAuthorizeChargingStationStoppedLocal(this,
                                                       DateTime.Now,
                                                       Id,
                                                       ChargingStationId,
                                                       result);

            #endregion

            return result;

        }

        #endregion


        #region RemoteStart(Timestamp, CancellationToken, EVSEId, ChargingProductId, ReservationId, SessionId, ProviderId, eMAId)

        /// <summary>
        /// Initiate a remote start of the given charging session at the given EVSE
        /// and for the given provider-/eMAId.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <returns>A remote start result object.</returns>
        public async Task<RemoteStartEVSEResult>

            RemoteStart(DateTime                Timestamp,
                        CancellationToken       CancellationToken,
                        EVSE_Id                 EVSEId,
                        ChargingProduct_Id      ChargingProductId,
                        ChargingReservation_Id  ReservationId,
                        ChargingSession_Id      SessionId,
                        EVSP_Id                 ProviderId,
                        eMA_Id                  eMAId)

        {

            var OnRemoteEVSEStartLocal = OnRemoteEVSEStart;
            if (OnRemoteEVSEStartLocal != null)
                OnRemoteEVSEStartLocal(this,
                                       Timestamp,
                                       Id,
                                       EVSEId,
                                       ChargingProductId,
                                       ReservationId,
                                       SessionId,
                                       ProviderId,
                                       eMAId);


            EVSEOperator EVSEOperator = null;

            if (!TryGetEVSEOperatorbyId(EVSEId.OperatorId, out EVSEOperator))
                return RemoteStartEVSEResult.Error("Unknown EVSE Operator!");

            var result = await EVSEOperator.RemoteStart(Timestamp,
                                                        CancellationToken,
                                                        EVSEId,
                                                        ChargingProductId,
                                                        ReservationId,
                                                        SessionId,
                                                        ProviderId,
                                                        eMAId);

            var OnRemoteEVSEStartedLocal = OnRemoteEVSEStarted;
            if (OnRemoteEVSEStartedLocal != null)
                OnRemoteEVSEStartedLocal(this,
                                         DateTime.Now,
                                         Id,
                                         EVSEId,
                                         result);

            return result;

        }

        #endregion

        #region RemoteStart(Timestamp, CancellationToken, ChargingStationId, ChargingProductId, ReservationId, SessionId, ProviderId, eMAId)

        /// <summary>
        /// Initiate a remote start of the given charging session at the given charging station
        /// and for the given provider-/eMAId.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <returns>A remote start result object.</returns>
        /// <returns>A remote start result object.</returns>
        public async Task<RemoteStartChargingStationResult>

            RemoteStart(DateTime                Timestamp,
                        CancellationToken       CancellationToken,
                        ChargingStation_Id      ChargingStationId,
                        ChargingProduct_Id      ChargingProductId,
                        ChargingReservation_Id  ReservationId,
                        ChargingSession_Id      SessionId,
                        EVSP_Id                 ProviderId,
                        eMA_Id                  eMAId)

        {

            var OnRemoteChargingStationStartLocal = OnRemoteChargingStationStart;
            if (OnRemoteChargingStationStartLocal != null)
                OnRemoteChargingStationStartLocal(this,
                                                  Timestamp,
                                                  Id,
                                                  ChargingStationId,
                                                  ChargingProductId,
                                                  ReservationId,
                                                  SessionId,
                                                  ProviderId,
                                                  eMAId);

            EVSEOperator EVSEOperator = null;

            if (!TryGetEVSEOperatorbyId(ChargingStationId.OperatorId, out EVSEOperator))
                return RemoteStartChargingStationResult.Error("Unknown EVSE Operator!");

            var result = await EVSEOperator.RemoteStart(Timestamp,
                                                        CancellationToken,
                                                        ChargingStationId,
                                                        ChargingProductId,
                                                        ReservationId,
                                                        SessionId,
                                                        ProviderId,
                                                        eMAId);

            var OnRemoteChargingStationStartedLocal = OnRemoteChargingStationStarted;
            if (OnRemoteChargingStationStartedLocal != null)
                OnRemoteChargingStationStartedLocal(this,
                                                    DateTime.Now,
                                                    Id,
                                                    ChargingStationId,
                                                    result);

            return result;

        }

        #endregion


        #region RemoteStop(Timestamp, CancellationToken, SessionId, ReservationHandling, ProviderId)

        /// <summary>
        /// Initiate a remote stop of the given charging session.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <returns>A remote stop result object.</returns>
        public async Task<RemoteStopResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId)

        {

            var OnRemoteStopLocal = OnRemoteStop;
            if (OnRemoteStopLocal != null)
                OnRemoteStopLocal(this,
                                  Timestamp,
                                  Id,
                                  SessionId,
                                  ReservationHandling,
                                  ProviderId);


            EVSEOperator EVSEOperator = null;

            //if (!TryGetEVSEOperatorbyId(EVSEId.OperatorId, out EVSEOperator))
            //    return RemoteStopEVSEResult.Error("Unknown EVSE Operator!");

            var result = await EVSEOperator.RemoteStop(Timestamp,
                                                       CancellationToken,
                                                       ReservationHandling,
                                                       SessionId,
                                                       ProviderId);

            var OnRemoteStoppedLocal = OnRemoteStopped;
            if (OnRemoteStoppedLocal != null)
                OnRemoteStoppedLocal(this,
                                     DateTime.Now,
                                     Id,
                                     result);

            return result;

        }

        #endregion

        #region RemoteStop(Timestamp, CancellationToken, EVSEId, SessionId, ReservationHandling, ProviderId)

        /// <summary>
        /// Initiate a remote stop of the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="EVSEId">An optional unique identification of an EVSE.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <returns>A remote stop result object.</returns>
        public async Task<RemoteStopEVSEResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EVSE_Id              EVSEId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId)

        {

            var OnRemoteEVSEStopLocal = OnRemoteEVSEStop;
            if (OnRemoteEVSEStopLocal != null)
                OnRemoteEVSEStopLocal(this,
                                      Timestamp,
                                      Id,
                                      EVSEId,
                                      SessionId,
                                      ReservationHandling,
                                      ProviderId);


            EVSEOperator EVSEOperator = null;

            if (!TryGetEVSEOperatorbyId(EVSEId.OperatorId, out EVSEOperator))
                return RemoteStopEVSEResult.Error("Unknown EVSE Operator!");

            var result = await EVSEOperator.RemoteStop(Timestamp,
                                                       CancellationToken,
                                                       ReservationHandling,
                                                       SessionId,
                                                       ProviderId,
                                                       EVSEId);

            var OnRemoteEVSEStoppedLocal = OnRemoteEVSEStopped;
            if (OnRemoteEVSEStoppedLocal != null)
                OnRemoteEVSEStoppedLocal(this,
                                         DateTime.Now,
                                         Id,
                                         EVSEId,
                                         result);

            return result;

        }

        #endregion

        #region RemoteStop(Timestamp, CancellationToken, ChargingStationId, SessionId, ReservationHandling, ProviderId)

        /// <summary>
        /// Initiate a remote stop of the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this task.</param>
        /// <param name="ChargingStationId">An optional unique identification of a charging station.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <returns>A remote stop result object.</returns>
        public async Task<RemoteStopChargingStationResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       ChargingStation_Id   ChargingStationId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId)

        {

            var OnRemoteChargingStationStopLocal = OnRemoteChargingStationStop;
            if (OnRemoteChargingStationStopLocal != null)
                OnRemoteChargingStationStopLocal(this,
                                                 Timestamp,
                                                 Id,
                                                 ChargingStationId,
                                                 SessionId,
                                                 ReservationHandling,
                                                 ProviderId);


            EVSEOperator EVSEOperator = null;

            if (!TryGetEVSEOperatorbyId(ChargingStationId.OperatorId, out EVSEOperator))
                return RemoteStopChargingStationResult.Error("Unknown EVSE Operator!");

            var result = await EVSEOperator.RemoteStop(Timestamp,
                                                       CancellationToken,
                                                       ReservationHandling,
                                                       SessionId,
                                                       ProviderId,
                                                       ChargingStationId);

            var OnRemoteChargingStationStoppedLocal = OnRemoteChargingStationStopped;
            if (OnRemoteChargingStationStoppedLocal != null)
                OnRemoteChargingStationStoppedLocal(this,
                                                    DateTime.Now,
                                                    Id,
                                                    ChargingStationId,
                                                    result);

            return result;

        }

        #endregion


        //ToDo: Refactor SendChargeDetailRecord to be async!

        #region SendChargeDetailRecord(EVSEId, SessionId, ChargingProductId, SessionStart, SessionEnd, AuthToken = null, eMAId = null, ..., QueryTimeout = null)

        /// <summary>
        /// Create a SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The session identification from the Authorize Start request.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthInfo">An optional ev customer or e-Mobility account identification.</param>
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(EVSE_Id              EVSEId,
                                   ChargingSession_Id   SessionId,
                                   ChargingProduct_Id   ChargingProductId,
                                   DateTime             SessionStart,
                                   DateTime             SessionEnd,
                                   AuthInfo             AuthInfo,
                                   DateTime?            ChargingStart         = null,
                                   DateTime?            ChargingEnd           = null,
                                   Double?              MeterValueStart       = null,
                                   Double?              MeterValueEnd         = null,
                                   IEnumerable<Double>  MeterValuesInBetween  = null,
                                   Double?              ConsumedEnergy        = null,
                                   String               MeteringSignature     = null,
                                   HubOperator_Id       HubOperatorId         = null,
                                   EVSP_Id              HubProviderId         = null,
                                   TimeSpan?            QueryTimeout          = null)

        {

            #region Initial checks

            if (EVSEId           == null)
                throw new ArgumentNullException("EVSEId",             "The given parameter must not be null!");

            if (SessionId        == null)
                throw new ArgumentNullException("SessionId",          "The given parameter must not be null!");

            if (ChargingProductId == null)
                throw new ArgumentNullException("ChargingProductId",  "The given parameter must not be null!");

            if (AuthInfo         == null)
                throw new ArgumentNullException("AuthInfo",           "The given parameter must not be null!");

            #endregion

            lock (_AuthenticationServices)
            {

                #region Some CDR should perhaps be filtered...

                var OnFilterCDRRecordsLocal = OnFilterCDRRecords;
                if (OnFilterCDRRecordsLocal != null)
                {

                    var _SENDCDRResult = OnFilterCDRRecordsLocal(AuthorizatorId, AuthInfo);

                    if (_SENDCDRResult != null)
                        return _SENDCDRResult;

                }

                #endregion

                IAuthServices  AuthenticationService;

                #region An authenticator was found for the upstream SessionId!

                if (_SessionIdAuthenticatorCache.TryGetValue(SessionId, out AuthenticationService))
                {

                    var _SendCDRTask = AuthenticationService.SendChargeDetailRecord(EVSEId,
                                                                                    SessionId,
                                                                                    ChargingProductId,
                                                                                    SessionStart,
                                                                                    SessionEnd,
                                                                                    AuthInfo,
                                                                                    ChargingStart,
                                                                                    ChargingEnd,
                                                                                    MeterValueStart,
                                                                                    MeterValueEnd,
                                                                                    MeterValuesInBetween,
                                                                                    ConsumedEnergy,
                                                                                    MeteringSignature,
                                                                                    HubOperatorId,
                                                                                    HubProviderId,
                                                                                    QueryTimeout);

                    _SendCDRTask.Wait();

                    if (_SendCDRTask.Result.Status == SendCDRResultType.Forwarded)
                    {
                        _SessionIdAuthenticatorCache.TryRemove(SessionId, out AuthenticationService);
                        return _SendCDRTask.Result;
                    }

                }

                IOperatorRoamingService OperatorRoamingService;

                if (_SessionIdOperatorRoamingServiceCache.TryGetValue(SessionId, out OperatorRoamingService))
                {

                    var _SendCDRTask = OperatorRoamingService.SendChargeDetailRecord(EVSEId,
                                                                                     SessionId,
                                                                                     ChargingProductId,
                                                                                     SessionStart,
                                                                                     SessionEnd,
                                                                                     AuthInfo,
                                                                                     ChargingStart,
                                                                                     ChargingEnd,
                                                                                     MeterValueStart,
                                                                                     MeterValueEnd,
                                                                                     MeterValuesInBetween,
                                                                                     ConsumedEnergy,
                                                                                     MeteringSignature,
                                                                                     HubOperatorId,
                                                                                     HubProviderId,
                                                                                     QueryTimeout);

                    _SendCDRTask.Wait();

                    if (_SendCDRTask.Result.Status == SendCDRResultType.Forwarded)
                    {
                        _SessionIdOperatorRoamingServiceCache.TryRemove(SessionId, out OperatorRoamingService);
                        return _SendCDRTask.Result;
                    }

                }

                #endregion

                #region Try to find anyone who might kown anything about the given SessionId!

                foreach (var OtherAuthenticationService in _AuthenticationServices.
                                                               OrderBy(v => v.Key).
                                                               Select(v => v.Value).
                                                               ToArray())
                {

                    var _SendCDRTask = OtherAuthenticationService.SendChargeDetailRecord(EVSEId,
                                                                                         SessionId,
                                                                                         ChargingProductId,
                                                                                         SessionStart,
                                                                                         SessionEnd,
                                                                                         AuthInfo,
                                                                                         ChargingStart,
                                                                                         ChargingEnd,
                                                                                         MeterValueStart,
                                                                                         MeterValueEnd,
                                                                                         MeterValuesInBetween,
                                                                                         ConsumedEnergy,
                                                                                         MeteringSignature,
                                                                                         HubOperatorId,
                                                                                         HubProviderId,
                                                                                         QueryTimeout);

                    _SendCDRTask.Wait();

                    if (_SendCDRTask.Result.Status == SendCDRResultType.Forwarded)
                        return _SendCDRTask.Result;

                }

                foreach (var OtherOperatorRoamingService in _OperatorRoamingServices.
                                                               OrderBy(v => v.Key).
                                                               Select(v => v.Value).
                                                               ToArray())
                {

                    var _SendCDRTask = OtherOperatorRoamingService.SendChargeDetailRecord(EVSEId,
                                                                                          SessionId,
                                                                                          ChargingProductId,
                                                                                          SessionStart,
                                                                                          SessionEnd,
                                                                                          AuthInfo,
                                                                                          ChargingStart,
                                                                                          ChargingEnd,
                                                                                          MeterValueStart,
                                                                                          MeterValueEnd,
                                                                                          MeterValuesInBetween,
                                                                                          ConsumedEnergy,
                                                                                          MeteringSignature,
                                                                                          HubOperatorId,
                                                                                          HubProviderId,
                                                                                          QueryTimeout);

                    _SendCDRTask.Wait();

                    if (_SendCDRTask.Result.Status == SendCDRResultType.Forwarded)
                        return _SendCDRTask.Result;

                }

                #endregion

                #region ...else fail!

                return SendCDRResult.False(AuthorizatorId,
                                           "No authorization service returned a positiv result!");

                #endregion

            }

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

        #region SendChargingStationAdminStatusDiff(StatusDiff)

        internal void SendChargingStationAdminStatusDiff(ChargingStationAdminStatusDiff StatusDiff)
        {

            var OnChargingStationAdminDiffLocal = OnChargingStationAdminDiff;
            if (OnChargingStationAdminDiffLocal != null)
                OnChargingStationAdminDiffLocal(StatusDiff);

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

                foreach (var AuthenticationService in _AuthenticationServices.
                                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
                {

                    result = await AuthenticationService.PushEVSEStatus(new KeyValuePair<EVSE_Id, EVSEStatusType>[] { new KeyValuePair<EVSE_Id, EVSEStatusType>(EVSE.Id, NewStatus.Value) },
                                                                        ActionType.update,
                                                                        EVSE.ChargingStation.ChargingPool.EVSEOperator.Id);

                }

                foreach (var OperatorRoamingService in _OperatorRoamingServices.
                                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
                                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
                {

                    result = await OperatorRoamingService.PushEVSEStatus(new KeyValuePair<EVSE_Id, EVSEStatusType>[] { new KeyValuePair<EVSE_Id, EVSEStatusType>(EVSE.Id, NewStatus.Value) },
                                                                         ActionType.update,
                                                                         EVSE.ChargingStation.ChargingPool.EVSEOperator.Id);

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
