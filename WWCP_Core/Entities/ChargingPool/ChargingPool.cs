/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging pool
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingPool : AEMobilityEntity<ChargingPool_Id>,
                                IEquatable<ChargingPool>, IComparable<ChargingPool>, IComparable,
                                IEnumerable<ChargingStation>,
                                IStatus<ChargingPoolStatusType>
    {

        #region Data

        /// <summary>
        /// The default max size of the aggregated charging pool status history.
        /// </summary>
        public const UInt16 DefaultPoolStatusHistorySize = 50;

        /// <summary>
        /// The default max size of the aggregated charging pool admin status history.
        /// </summary>
        public const UInt16 DefaultPoolAdminStatusHistorySize = 50;

        #endregion

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of this charging pool.
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

                if (value == null)
                    value = new I18NString();

                if (_Name != value)
                    SetProperty<I18NString>(ref _Name, value);

            }

        }

        #endregion

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of this charging pool.
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

                if (value == null)
                    value = new I18NString();

                if (_Description != value)
                    SetProperty<I18NString>(ref _Description, value);

            }

        }

        #endregion

        #region LocationLanguage

        private Languages _LocationLanguage;

        /// <summary>
        /// The official language at this charging pool.
        /// </summary>
        [Optional]
        public Languages LocationLanguage
        {

            get
            {
                return _LocationLanguage;
            }

            set
            {

                if (value == null)
                    value = Languages.unknown;

                if (_LocationLanguage != value)
                {

                    SetProperty(ref _LocationLanguage, value);

                    // No downstream!
                    //_ChargingStations.Values.ForEach(station => station.LocationLanguage = null);

                }

            }

        }

        #endregion

        #region Address

        private Address _Address;

        /// <summary>
        /// The address of this charging pool.
        /// </summary>
        [Optional]
        public Address Address
        {

            get
            {
                return _Address;
            }

            set
            {

                if (value == null)
                    value = new Address();

                if (_Address != value)
                {

                    SetProperty<Address>(ref _Address, value);

                    _ChargingStations.Values.ForEach(station => station._Address = null);

                }

            }

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate _GeoLocation;

        /// <summary>
        /// The geographical location of this charging pool.
        /// </summary>
        [Optional]
        public GeoCoordinate GeoLocation
        {

            get
            {
                return _GeoLocation;
            }

            set
            {

                if (value == null)
                    value = new GeoCoordinate(new Latitude(0), new Longitude(0));

                if (_GeoLocation != value)
                {

                    SetProperty(ref _GeoLocation, value);

                    _ChargingStations.Values.ForEach(station => station._GeoLocation = null);

                }

            }

        }

        #endregion

        #region EntranceAddress

        private Address _EntranceAddress;

        /// <summary>
        /// The address of the entrance to this charging pool.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address EntranceAddress
        {

            get
            {
                return _EntranceAddress;
            }

            set
            {

                if (value == null)
                    value = new Address();

                if (_EntranceAddress != value)
                {

                    SetProperty<Address>(ref _EntranceAddress, value);

                    _ChargingStations.Values.ForEach(station => station._EntranceAddress = null);

                }

            }

        }

        #endregion

        #region EntranceLocation

        private GeoCoordinate _EntranceLocation;

        /// <summary>
        /// The geographical location of the entrance to this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate EntranceLocation
        {

            get
            {
                return _EntranceLocation;
            }

            set
            {

                if (value == null)
                    value = new GeoCoordinate(new Latitude(0), new Longitude(0));

                if (_EntranceLocation != value)
                {

                    SetProperty(ref _EntranceLocation, value);

                    _ChargingStations.Values.ForEach(station => station._EntranceLocation = null);

                }

            }

        }

        #endregion

        #region ExitAddress

        private Address _ExitAddress;

        /// <summary>
        /// The address of the exit of this charging pool.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address ExitAddress
        {

            get
            {
                return _ExitAddress;
            }

            set
            {

                if (value == null)
                    value = new Address();

                if (_ExitAddress != value)
                {

                    SetProperty<Address>(ref _ExitAddress, value);

                    _ChargingStations.Values.ForEach(station => station._ExitAddress = null);

                }

            }

        }

        #endregion

        #region ExitLocation

        private GeoCoordinate _ExitLocation;

        /// <summary>
        /// The geographical location of the exit of this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate ExitLocation
        {

            get
            {
                return _ExitLocation;
            }

            set
            {

                if (value == null)
                    value = new GeoCoordinate(new Latitude(0), new Longitude(0));

                if (_ExitLocation != value)
                {

                    SetProperty(ref _ExitLocation, value);

                    _ChargingStations.Values.ForEach(station => station._ExitLocation = null);

                }

            }

        }

        #endregion

        #region OpeningTime

        private OpeningTime _OpeningTime;

        /// <summary>
        /// The opening time of this charging pool.
        /// </summary>
        [Optional]
        public OpeningTime OpeningTime
        {

            get
            {
                return _OpeningTime;
            }

            set
            {

                if (value == null)
                    value = new OpeningTime();

                if (_OpeningTime != value)
                {

                    SetProperty(ref _OpeningTime, value);

                    _ChargingStations.Values.ForEach(station => station._OpeningTime = null);

                }

            }

        }

        #endregion

        #region AuthenticationModes

        private ReactiveSet<AuthenticationModes> _AuthenticationModes;

        public ReactiveSet<AuthenticationModes> AuthenticationModes
        {

            get
            {
                return _AuthenticationModes;
            }

            set
            {

                if (value == null)
                    value = new ReactiveSet<AuthenticationModes>();

                if (_AuthenticationModes != value)
                {

                    SetProperty(ref _AuthenticationModes, value);

                    _ChargingStations.Values.ForEach(station => station._AuthenticationModes = null);

                }

            }

        }

        #endregion

        #region PaymentOptions

        private ReactiveSet<PaymentOptions> _PaymentOptions;

        [Mandatory]
        public ReactiveSet<PaymentOptions> PaymentOptions
        {

            get
            {
                return _PaymentOptions;
            }

            set
            {

                if (value == null)
                    value = new ReactiveSet<PaymentOptions>();

                if (_PaymentOptions != value)
                {

                    SetProperty(ref _PaymentOptions, value);

                    _ChargingStations.Values.ForEach(station => station._PaymentOptions = null);

                }

            }

        }

        #endregion

        #region Accessibility

        private AccessibilityTypes _Accessibility;

        [Optional]
        public AccessibilityTypes Accessibility
        {

            get
            {
                return _Accessibility;
            }

            set
            {

                if (_Accessibility != value)
                {

                    SetProperty(ref _Accessibility, value);

                    _ChargingStations.Values.ForEach(station => station._Accessibility = value);

                }

            }

        }

        #endregion

        #region HotlinePhoneNumber

        private String _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the EVSE operator hotline.
        /// </summary>
        [Optional]
        public String HotlinePhoneNumber
        {

            get
            {
                return _HotlinePhoneNumber;
            }

            set
            {

                if (value == null)
                    value = "";

                if (_HotlinePhoneNumber != value)
                {

                    SetProperty(ref _HotlinePhoneNumber, value);

                    _ChargingStations.Values.ForEach(station => station._HotlinePhoneNumber = null);

                }

            }

        }

        #endregion

        #region IsHubjectCompatible

        [Optional]
        public Partly IsHubjectCompatible
        {
            get
            {
                return PartlyHelper.Generate(_ChargingStations.Select(station => station.Value.IsHubjectCompatible));
            }
        }

        #endregion

        #region DynamicInfoAvailable

        [Optional]
        public Partly DynamicInfoAvailable
        {
            get
            {
                return PartlyHelper.Generate(_ChargingStations.Select(station => station.Value.DynamicInfoAvailable));
            }
        }

        #endregion


        #region PoolOwner

        private String _PoolOwner;

        /// <summary>
        /// The owner of this charging pool.
        /// </summary>
        [Optional]
        public String PoolOwner
        {

            get
            {
                return _PoolOwner;
            }

            set
            {
                SetProperty<String>(ref _PoolOwner, value);
            }

        }

        #endregion

        #region LocationOwner

        private String _LocationOwner;

        /// <summary>
        /// The owner of the charging pool location.
        /// </summary>
        [Optional]
        public String LocationOwner
        {

            get
            {
                return _LocationOwner;
            }

            set
            {
                SetProperty<String>(ref _LocationOwner, value);
            }

        }

        #endregion

        #region PhotoURIs

        private ReactiveSet<String> _PhotoURIs;

        /// <summary>
        /// URIs of photos of this charging pool.
        /// </summary>
        [Optional]
        public ReactiveSet<String> PhotoURIs
        {

            get
            {
                return _PhotoURIs;
            }

            set
            {
                SetProperty(ref _PhotoURIs, value);
            }

        }

        #endregion


        #region Status

        /// <summary>
        /// The current charging pool status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingPoolStatusType> Status
        {
            get
            {
                return _StatusSchedule.Peek();
            }
        }

        #endregion

        #region StatusSchedule

        private Stack<Timestamped<ChargingPoolStatusType>> _StatusSchedule;

        /// <summary>
        /// The charging pool status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<ChargingPoolStatusType>> StatusSchedule
        {
            get
            {
                return _StatusSchedule.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        #region StatusAggregationDelegate

        private Func<ChargingStationStatusReport, ChargingPoolStatusType> _StatusAggregationDelegate;

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated charging stations.
        /// </summary>
        public Func<ChargingStationStatusReport, ChargingPoolStatusType> StatusAggregationDelegate
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
        /// The current charging pool admin status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingPoolAdminStatusType> AdminStatus
        {
            get
            {
                return _AdminStatusSchedule.Peek();
            }
        }

        #endregion

        #region AdminStatusSchedule

        private Stack<Timestamped<ChargingPoolAdminStatusType>> _AdminStatusSchedule;

        /// <summary>
        /// The charging pool admin status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<ChargingPoolAdminStatusType>> AdminStatusSchedule
        {
            get
            {
                return _AdminStatusSchedule.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        #region PlannedAdminStatusChanges

        private List<Timestamped<ChargingPoolAdminStatusType>> _PlannedAdminStatusChanges;

        /// <summary>
        /// A list of planned future charging pool admin status changes.
        /// </summary>
        public IEnumerable<Timestamped<ChargingPoolAdminStatusType>> PlannedAdminStatusChanges
        {
            get
            {
                return _PlannedAdminStatusChanges.OrderBy(v => v.Timestamp);
            }
        }

        #endregion

        #region AdminStatusAggregationDelegate

        private Func<ChargingStationAdminStatusReport, ChargingPoolAdminStatusType> _AdminStatusAggregationDelegate;

        /// <summary>
        /// A delegate called to aggregate the admin status of all subordinated charging stations.
        /// </summary>
        public Func<ChargingStationAdminStatusReport, ChargingPoolAdminStatusType> AdminStatusAggregationDelegate
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


        #region EVSEOperator

        private EVSEOperator _EVSEOperator;

        /// <summary>
        /// The EVSE operator of this charging pool.
        /// </summary>
        [Optional]
        public EVSEOperator EVSEOperator
        {
            get
            {
                return _EVSEOperator;
            }
        }

        #endregion

        #region ChargingStations

        private readonly ConcurrentDictionary<ChargingStation_Id, ChargingStation> _ChargingStations;

        /// <summary>
        /// Return all charging stations registered within this charing pool.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations
        {
            get
            {
                return _ChargingStations.Select(KVP => KVP.Value);
            }
        }

        #endregion

        #region ChargingStationIds

        /// <summary>
        /// Return all charging station Ids registered within this charing pool.
        /// </summary>
        public IEnumerable<ChargingStation_Id> ChargingStationIds
        {
            get
            {
                return _ChargingStations.Select(KVP => KVP.Value.Id);
            }
        }

        #endregion

        #region EVSEs

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging pool.
        /// </summary>
        public IEnumerable<EVSE> EVSEs
        {
            get
            {

                return _ChargingStations.
                           Values.
                           SelectMany(station => station.EVSEs);

            }
        }

        #endregion

        #region EVSEIds

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment
        /// (EVSEs) present within this charging pool.
        /// </summary>
        public IEnumerable<EVSE_Id> EVSEIds
        {
            get
            {

                return _ChargingStations.
                           Values.
                           SelectMany(station => station.EVSEs).
                           Select    (evse    => evse.Id);

            }
        }

        #endregion

        #endregion

        #region Events

        // ChargingPool events

        #region OnAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the admin status changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
        public delegate void OnAdminStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<ChargingPoolAdminStatusType> OldStatus, Timestamped<ChargingPoolAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnAdminStatusChangedDelegate OnAdminStatusChanged;

        #endregion

        #region OnAggregatedStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="OldStatus">The old timestamped status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
        public delegate void OnAggregatedStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<ChargingPoolStatusType> OldStatus, Timestamped<ChargingPoolStatusType> NewStatus);

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
        /// <param name="ChargingPool">The charging pool.</param>
        /// <param name="OldStatus">The old timestamped status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
        public delegate void OnAggregatedAdminStatusChangedDelegate(DateTime Timestamp, ChargingPool ChargingPool, Timestamped<ChargingPoolAdminStatusType> OldStatus, Timestamped<ChargingPoolAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status changed.
        /// </summary>
        public event OnAggregatedAdminStatusChangedDelegate OnAggregatedAdminStatusChanged;

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

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group/pool of charging stations having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing pool.</param>
        /// <param name="EVSEOperator">The parent EVSE operator.</param>
        /// <param name="PoolStatusHistorySize">The default size of the charging pool (aggregated EVSE) status history.</param>
        /// <param name="PoolAdminStatusHistorySize">The default size of the charging pool (aggregated EVSE) admin status history.</param>
        internal ChargingPool(ChargingPool_Id  Id,
                              EVSEOperator     EVSEOperator,
                              UInt16           PoolStatusHistorySize       = DefaultPoolStatusHistorySize,
                              UInt16           PoolAdminStatusHistorySize  = DefaultPoolAdminStatusHistorySize)

            : base(Id)

        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The EVSE operator must not be null!");

            #endregion

            #region Init data and properties

            this._EVSEOperator               = EVSEOperator;

            this._ChargingStations           = new ConcurrentDictionary<ChargingStation_Id, ChargingStation>();

            this.LocationLanguage            = Languages.unknown;
            this.Name                        = new I18NString();
            this.Description                 = new I18NString();
            this.Address                     = new Address();
            this.EntranceAddress             = new Address();

            this._AuthenticationModes        = new ReactiveSet<AuthenticationModes>();

            this._StatusSchedule             = new Stack<Timestamped<ChargingPoolStatusType>>((Int32) PoolStatusHistorySize);
            this._StatusSchedule.Push(new Timestamped<ChargingPoolStatusType>(ChargingPoolStatusType.Unknown));

            this._AdminStatusSchedule        = new Stack<Timestamped<ChargingPoolAdminStatusType>>((Int32) PoolAdminStatusHistorySize);
            this._AdminStatusSchedule.Push(new Timestamped<ChargingPoolAdminStatusType>(ChargingPoolAdminStatusType.Unknown));

            #endregion

            #region Init events

            // ChargingPool events
            this.ChargingStationAddition  = new VotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval   = new VotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition             = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval              = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events
            this.SocketOutletAddition     = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval      = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            #endregion

            #region Link events

            // ChargingPool events
            this.OnChargingStationAddition.OnVoting       += (timestamp, evseoperator, pool, vote) => EVSEOperator.ChargingStationAddition.SendVoting      (timestamp, evseoperator, pool, vote);
            this.OnChargingStationAddition.OnNotification += (timestamp, evseoperator, pool)       => EVSEOperator.ChargingStationAddition.SendNotification(timestamp, evseoperator, pool);

            this.OnChargingStationRemoval. OnVoting       += (timestamp, evseoperator, pool, vote) => EVSEOperator.ChargingStationRemoval. SendVoting      (timestamp, evseoperator, pool, vote);
            this.OnChargingStationRemoval. OnNotification += (timestamp, evseoperator, pool)       => EVSEOperator.ChargingStationRemoval. SendNotification(timestamp, evseoperator, pool);

            // ChargingStation events
            this.OnEVSEAddition.           OnVoting       += (timestamp, station, evse, vote)      => EVSEOperator.EVSEAddition.           SendVoting      (timestamp, station, evse, vote);
            this.OnEVSEAddition.           OnNotification += (timestamp, station, evse)            => EVSEOperator.EVSEAddition.           SendNotification(timestamp, station, evse);

            this.OnEVSERemoval.            OnVoting       += (timestamp, station, evse, vote)      => EVSEOperator.EVSERemoval .           SendVoting      (timestamp, station, evse, vote);
            this.OnEVSERemoval.            OnNotification += (timestamp, station, evse)            => EVSEOperator.EVSERemoval .           SendNotification(timestamp, station, evse);

            // EVSE events
            this.SocketOutletAddition.     OnVoting       += (timestamp, evse, outlet, vote)       => EVSEOperator.SocketOutletAddition.   SendVoting      (timestamp, evse, outlet, vote);
            this.SocketOutletAddition.     OnNotification += (timestamp, evse, outlet)             => EVSEOperator.SocketOutletAddition.   SendNotification(timestamp, evse, outlet);

            this.SocketOutletRemoval.      OnVoting       += (timestamp, evse, outlet, vote)       => EVSEOperator.SocketOutletRemoval.    SendVoting      (timestamp, evse, outlet, vote);
            this.SocketOutletRemoval.      OnNotification += (timestamp, evse, outlet)             => EVSEOperator.SocketOutletRemoval.    SendNotification(timestamp, evse, outlet);

            #endregion

        }

        #endregion


        #region CreateNewStation(ChargingStationId = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station failed.</param>
        public ChargingStation CreateNewStation(ChargingStation_Id                        ChargingStationId  = null,
                                                Action<ChargingStation>                   Configurator       = null,
                                                Action<ChargingStation>                   OnSuccess          = null,
                                                Action<ChargingPool, ChargingStation_Id>  OnError            = null)
        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException("ChargingStationId", "The given charging station identification must not be null!");

            if (_ChargingStations.ContainsKey(ChargingStationId))
            {
                if (OnError == null)
                    throw new ChargingStationAlreadyExistsInPool(ChargingStationId, this.Id);
                else
                    OnError.FailSafeInvoke(this, ChargingStationId);
            }

            #endregion

            var _ChargingStation = new ChargingStation(ChargingStationId, this);

            Configurator.FailSafeInvoke(_ChargingStation);

            if (ChargingStationAddition.SendVoting(DateTime.Now, this, _ChargingStation))
            {
                if (_ChargingStations.TryAdd(ChargingStationId, _ChargingStation))
                {

                    _ChargingStation.OnEVSEDataChanged              += (Timestamp, EVSE, PropertyName, OldValue, NewValue)
                                                                        => UpdateEVSEData       (Timestamp, EVSE, PropertyName, OldValue, NewValue);

                    _ChargingStation.OnEVSEStatusChanged            += (Timestamp, EVSE, OldStatus, NewStatus)
                                                                        => UpdateEVSEStatus     (Timestamp, EVSE, OldStatus, NewStatus);

                    _ChargingStation.OnEVSEAdminStatusChanged       += (Timestamp, EVSE, OldStatus, NewStatus)
                                                                        => UpdateEVSEAdminStatus(Timestamp, EVSE, OldStatus, NewStatus);


                    _ChargingStation.OnPropertyChanged              += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                                                                        => UpdateChargingStationData(Timestamp, Sender as ChargingStation, PropertyName, OldValue, NewValue);

                    _ChargingStation.OnAdminStatusChanged           += (Timestamp, ChargingStation, OldStatus, NewStatus)
                                                                        => UpdateChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus);

                    _ChargingStation.OnAggregatedStatusChanged      += (Timestamp, ChargingStation, OldStatus, NewStatus)
                                                                        => UpdateAggregatedChargingStationStatus(Timestamp, ChargingStation, OldStatus, NewStatus);

                    _ChargingStation.OnAggregatedAdminStatusChanged += (Timestamp, ChargingStation, OldStatus, NewStatus)
                                                                        => UpdateAggregatedChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus);


                    OnSuccess.FailSafeInvoke(_ChargingStation);
                    ChargingStationAddition.SendNotification(DateTime.Now, this, _ChargingStation);
                    return _ChargingStation;

                }
            }

            Debug.WriteLine("ChargingStation '" + ChargingStationId + "' was not created!");
            return null;

        }

        #endregion


        #region ContainsChargingStation(ChargingStation)

        /// <summary>
        /// Check if the given ChargingStation is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStationId(ChargingStation ChargingStation)
        {
            return _ChargingStations.ContainsKey(ChargingStation.Id);
        }

        #endregion

        #region ContainsChargingStation(ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId)
        {
            return _ChargingStations.ContainsKey(ChargingStationId);
        }

        #endregion

        #region GetChargingStationById(ChargingStationId)

        public ChargingStation GetChargingStationById(ChargingStation_Id ChargingStationId)
        {

            ChargingStation _ChargingStation = null;

            if (_ChargingStations.TryGetValue(ChargingStationId, out _ChargingStation))
                return _ChargingStation;

            return null;

        }

        #endregion

        #region TryGetChargingStationbyId(ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationbyId(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {
            return _ChargingStations.TryGetValue(ChargingStationId, out ChargingStation);
        }

        #endregion

        #region RemoveChargingStation(ChargingStationId)

        public ChargingStation RemoveChargingStation(ChargingStation_Id ChargingStationId)
        {

            ChargingStation _ChargingStation = null;

            if (TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
            {

                if (ChargingStationRemoval.SendVoting(DateTime.Now, this, _ChargingStation))
                {

                    if (_ChargingStations.TryRemove(ChargingStationId, out _ChargingStation))
                    {

                        ChargingStationRemoval.SendNotification(DateTime.Now, this, _ChargingStation);

                        return _ChargingStation;

                    }

                }

            }

            return null;

        }

        #endregion

        #region TryRemoveChargingStation(ChargingStationId, out ChargingStation)

        public Boolean TryRemoveChargingStation(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {

            if (TryGetChargingStationbyId(ChargingStationId, out ChargingStation))
            {

                if (ChargingStationRemoval.SendVoting(DateTime.Now, this, ChargingStation))
                {

                    if (_ChargingStations.TryRemove(ChargingStationId, out ChargingStation))
                    {

                        ChargingStationRemoval.SendNotification(DateTime.Now, this, ChargingStation);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion


        #region SetAdminStatus(Timestamp, NewStatus)

        /// <summary>
        /// Set the timestamped admin status of the charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="NewStatus">A list of new timestamped status for this charging pool.</param>
        public void SetAdminStatus(DateTime                                  Timestamp,
                                   Timestamped<ChargingPoolAdminStatusType>  NewStatus)
        {

            if (_AdminStatusSchedule.Peek().Value != NewStatus.Value)
            {

                var OldStatus = _AdminStatusSchedule.Peek();

                _AdminStatusSchedule.Push(NewStatus);

                var OnAdminStatusChangedLocal = OnAdminStatusChanged;
                if (OnAdminStatusChangedLocal != null)
                    OnAdminStatusChanged(Timestamp, this, OldStatus, NewStatus);

            }

        }

        #endregion

        #region SetAdminStatus(Timestamp, NewStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped admin status of the charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="NewStatusList">A list of new timestamped status for this charging pool.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetAdminStatus(DateTime                                               Timestamp,
                                   IEnumerable<Timestamped<ChargingPoolAdminStatusType>>  NewStatusList,
                                   ChangeMethods                                          ChangeMethod = ChangeMethods.Replace)
        {

            foreach (var NewStatus in NewStatusList)
            {

                if (NewStatus.Timestamp <= DateTime.Now)
                {
                    if (_AdminStatusSchedule.Peek().Value != NewStatus.Value)
                    {

                        var OldStatus = _AdminStatusSchedule.Peek();

                        _AdminStatusSchedule.Push(NewStatus);

                        var OnAdminStatusChangedLocal = OnAdminStatusChanged;
                        if (OnAdminStatusChangedLocal != null)
                            OnAdminStatusChanged(Timestamp, this, OldStatus, NewStatus);

                    }
                }

                else
                {

                    if (ChangeMethod == ChangeMethods.Replace)
                        _PlannedAdminStatusChanges = NewStatusList.
                                                         Where(TVP => TVP.Timestamp > DateTime.Now).
                                                         ToList();

                    else
                        _PlannedAdminStatusChanges = _PlannedAdminStatusChanges.
                                                         Concat(NewStatusList.Where(TVP => TVP.Timestamp > DateTime.Now)).
                                                         Deduplicate().
                                                         ToList();

                }

            }

        }

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the EVSE operator.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)
        {
            return _ChargingStations.Values.Any(ChargingStation => ChargingStation.EVSEIds.Contains(EVSE.Id));
        }

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the EVSE operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)
        {
            return _ChargingStations.Values.Any(ChargingStation => ChargingStation.EVSEIds.Contains(EVSEId));
        }

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)
        {

            return _ChargingStations.Values.
                       SelectMany(station => station.EVSEs).
                       Where     (EVSE    => EVSE.Id == EVSEId).
                       FirstOrDefault();

        }

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = _ChargingStations.Values.
                       SelectMany(station => station.EVSEs).
                       Where     (_EVSE   => _EVSE.Id == EVSEId).
                       FirstOrDefault();

            return EVSE != null;

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
        internal void UpdateEVSEStatus(DateTime                     Timestamp,
                                       EVSE                         EVSE,
                                       Timestamped<EVSEStatusType>  OldStatus,
                                       Timestamped<EVSEStatusType>  NewStatus)
        {

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

            // Send charging station status change upstream
            var OnAggregatedChargingStationStatusChangedLocal = OnAggregatedChargingStationStatusChanged;
            if (OnAggregatedChargingStationStatusChangedLocal != null)
                OnAggregatedChargingStationStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);


            // Calculate new aggregated charging pool status and send upstream
            if (StatusAggregationDelegate != null)
            {

                var NewAggregatedStatus = new Timestamped<ChargingPoolStatusType>(StatusAggregationDelegate(new ChargingStationStatusReport(_ChargingStations.Values)));

                if (NewAggregatedStatus.Value != _StatusSchedule.Peek().Value)
                {

                    var OldAggregatedStatus = _StatusSchedule.Peek();

                    _StatusSchedule.Push(NewAggregatedStatus);

                    var OnAggregatedStatusChangedLocal = OnAggregatedStatusChanged;
                    if (OnAggregatedStatusChangedLocal != null)
                        OnAggregatedStatusChangedLocal(Timestamp, this, OldAggregatedStatus, NewAggregatedStatus);

                }

            }

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

            // Send charging station status change upstream
            var OnAggregatedChargingStationAdminStatusChangedLocal = OnAggregatedChargingStationAdminStatusChanged;
            if (OnAggregatedChargingStationAdminStatusChangedLocal != null)
                OnAggregatedChargingStationAdminStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);


            // Calculate new aggregated charging pool status and send upstream
            if (AdminStatusAggregationDelegate != null)
            {

                var NewAggregatedStatus = new Timestamped<ChargingPoolAdminStatusType>(AdminStatusAggregationDelegate(new ChargingStationAdminStatusReport(_ChargingStations.Values)));

                if (NewAggregatedStatus.Value != _AdminStatusSchedule.Peek().Value)
                {

                    var OldAggregatedStatus = _AdminStatusSchedule.Peek();

                    _AdminStatusSchedule.Push(NewAggregatedStatus);

                    var OnAggregatedAdminStatusChangedLocal = OnAggregatedAdminStatusChanged;
                    if (OnAggregatedAdminStatusChangedLocal != null)
                        OnAggregatedAdminStatusChangedLocal(Timestamp, this, OldAggregatedStatus, NewAggregatedStatus);

                }

            }

        }

        #endregion


        #region IEnumerable<ChargingStation> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _ChargingStations.Values.GetEnumerator();
        }

        public IEnumerator<ChargingStation> GetEnumerator()
        {
            return _ChargingStations.Values.GetEnumerator();
        }

        #endregion

        #region IComparable<ChargingPool> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charging pool.
            var ChargingPool = Object as ChargingPool;
            if ((Object) ChargingPool == null)
                throw new ArgumentException("The given object is not a charging pool!");

            return CompareTo(ChargingPool);

        }

        #endregion

        #region CompareTo(ChargingPool)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPool">A charging pool object to compare with.</param>
        public Int32 CompareTo(ChargingPool ChargingPool)
        {

            if ((Object) ChargingPool == null)
                throw new ArgumentNullException("The given charging pool must not be null!");

            return Id.CompareTo(ChargingPool.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPool> Members

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

            // Check if the given object is a charging pool.
            var ChargingPool = Object as ChargingPool;
            if ((Object) ChargingPool == null)
                return false;

            return this.Equals(ChargingPool);

        }

        #endregion

        #region Equals(ChargingPool)

        /// <summary>
        /// Compares two charging pools for equality.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPool ChargingPool)
        {

            if ((Object) ChargingPool == null)
                return false;

            return Id.Equals(ChargingPool.Id);

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
