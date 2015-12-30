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
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;
using System.Threading.Tasks;
using System.Threading;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charging station to charge an electric vehicle.
    /// </summary>
    public class ChargingStation : AEMobilityEntity<ChargingStation_Id>,
                                   IEquatable<ChargingStation>, IComparable<ChargingStation>, IComparable,
                                   IEnumerable<EVSE>,
                                   IStatus<ChargingStationStatusType>
    {

        #region Data

        /// <summary>
        /// The default max size of the charging station (aggregated EVSE) status list.
        /// </summary>
        public const UInt16 DefaultMaxStationStatusListSize = 15;

        /// <summary>
        /// The default max size of the charging station admin status list.
        /// </summary>
        public const UInt16 DefaultMaxStationAdminStatusListSize = 15;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(30);

        #endregion

        #region Properties

        #region RemoteChargingStation

        private IRemoteChargingStation _RemoteChargingStation;

        internal IRemoteChargingStation RemoteChargingStation
        {

            get
            {
                return _RemoteChargingStation;
            }

            set
            {

                if (value == null)
                    throw new ArgumentNullException("RemoteChargingStation", "The given parameter must not be null!");

                _RemoteChargingStation = value;

            }

        }

        #endregion

        #region ServiceIdentification

        private String _ServiceIdentification;

        /// <summary>
        /// The internal service identification of the charging station maintained by the EVSE operator.
        /// </summary>
        [Optional]
        public String ServiceIdentification
        {

            get
            {
                return _ServiceIdentification;
            }

            set
            {

                if (ServiceIdentification != value)
                    SetProperty<String>(ref _ServiceIdentification, value);

            }

        }

        #endregion

        #region HubjectStationId

        private String _HubjectStationId;

        [Optional]
        public String HubjectStationId
        {

            get
            {
                return _HubjectStationId;
            }

            set
            {

                if (HubjectStationId != value)
                    SetProperty<String>(ref _HubjectStationId, value);

            }

        }

        #endregion

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of this charging station.
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

                if (_Name != value)
                    SetProperty<I18NString>(ref _Name, value);

            }

        }

        #endregion

        #region Description

        internal I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of this charging station.
        /// </summary>
        [Optional]
        public I18NString Description
        {

            get
            {

                return _Description != null
                    ? _Description
                    : ChargingPool.Description;

            }

            set
            {

                if (value == ChargingPool.Description)
                    return;

                if (Description != value)
                    SetProperty<I18NString>(ref _Description, value);

            }

        }

        #endregion

        #region Address

        internal Address _Address;

        /// <summary>
        /// The address of this charging station.
        /// </summary>
        [Optional]
        public Address Address
        {

            get
            {

                return _Address != null
                    ? _Address
                    : ChargingPool.Address;

            }

            set
            {

                if (value == ChargingPool.Address)
                    return;

                if (Address != value)
                {

                    if (value == null)
                        DeleteProperty(ref _Address);

                    else
                        SetProperty(ref _Address, value);

                }

            }

        }

        #endregion

        #region GeoLocation

        internal GeoCoordinate _GeoLocation;

        /// <summary>
        /// The geographical location of this charging station.
        /// </summary>
        [Optional]
        public GeoCoordinate GeoLocation
        {

            get
            {

                return _GeoLocation.IsValid()
                    ? _GeoLocation
                    : ChargingPool.GeoLocation;

            }

            set
            {

                if (value == ChargingPool.GeoLocation)
                    return;

                if (GeoLocation != value)
                {

                    if (value == null)
                        DeleteProperty(ref _GeoLocation);

                    else
                        SetProperty(ref _GeoLocation, value);

                }

            }

        }

        #endregion

        #region EntranceAddress

        internal Address _EntranceAddress;

        /// <summary>
        /// The address of the entrance to this charging station.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address EntranceAddress
        {

            get
            {

                return _EntranceAddress != null
                    ? _EntranceAddress
                    : ChargingPool.EntranceAddress;

            }

            set
            {

                if (value == ChargingPool.EntranceAddress)
                    return;

                if (EntranceAddress != value)
                {

                    if (value == null)
                        DeleteProperty(ref _EntranceAddress);

                    else
                        SetProperty(ref _EntranceAddress, value);

                }

            }

        }

        #endregion

        #region EntranceLocation

        internal GeoCoordinate _EntranceLocation;

        /// <summary>
        /// The geographical location of the entrance to this charging station.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate EntranceLocation
        {

            get
            {

                return _EntranceLocation.IsValid()
                    ? _EntranceLocation
                    : ChargingPool.EntranceLocation;

            }

            set
            {

                if (value == ChargingPool.EntranceLocation)
                    return;

                if (EntranceLocation != value)
                {

                    if (value == null)
                        DeleteProperty(ref _EntranceLocation);

                    else
                        SetProperty(ref _EntranceLocation, value);

                }

            }

        }

        #endregion

        #region ExitAddress

        internal Address _ExitAddress;

        /// <summary>
        /// The address of the exit of this charging station.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        public Address ExitAddress
        {

            get
            {

                return _ExitAddress != null
                    ? _ExitAddress
                    : ChargingPool.ExitAddress;

            }

            set
            {

                if (value == ChargingPool.ExitAddress)
                    return;

                if (ExitAddress != value)
                {

                    if (value == null)
                        DeleteProperty(ref _ExitAddress);

                    else
                        SetProperty(ref _ExitAddress, value);

                }

            }

        }

        #endregion

        #region ExitLocation

        internal GeoCoordinate _ExitLocation;

        /// <summary>
        /// The geographical location of the exit of this charging station.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        public GeoCoordinate ExitLocation
        {

            get
            {

                return _ExitLocation.IsValid()
                    ? _ExitLocation
                    : ChargingPool.ExitLocation;

            }

            set
            {

                if (value == ChargingPool.ExitLocation)
                    return;

                if (ExitLocation != value)
                {

                    if (value == null)
                        DeleteProperty(ref _ExitLocation);

                    else
                        SetProperty(ref _ExitLocation, value);

                }

            }

        }

        #endregion

        #region OpeningTime

        internal OpeningTime _OpeningTime;

        /// <summary>
        /// The opening time of this charging station.
        /// </summary>
        public OpeningTime OpeningTime
        {

            get
            {

                return _OpeningTime != null
                    ? _OpeningTime
                    : ChargingPool.OpeningTime;

            }

            set
            {

                if (value == ChargingPool.OpeningTime)
                    return;

                if (OpeningTime != value)
                {

                    if (value == null)
                        DeleteProperty(ref _OpeningTime);

                    else
                        SetProperty(ref _OpeningTime, value);

                }

            }

        }

        #endregion

        #region AuthenticationModes

        internal ReactiveSet<AuthenticationModes> _AuthenticationModes;

        public ReactiveSet<AuthenticationModes> AuthenticationModes
        {

            get
            {

                return _AuthenticationModes != null
                    ? _AuthenticationModes
                    : ChargingPool.AuthenticationModes;

            }

            set
            {

                if (value == ChargingPool.AuthenticationModes)
                    return;

                if (AuthenticationModes != value)
                {

                    if (_AuthenticationModes == null)
                        _AuthenticationModes = new ReactiveSet<AuthenticationModes>();

                    if (value == null)
                        DeleteProperty(ref _AuthenticationModes);

                    else
                        SetProperty(ref _AuthenticationModes, value);

                }

            }

        }

        #endregion

        #region PaymentOptions

        internal ReactiveSet<PaymentOptions> _PaymentOptions;

        [Mandatory]
        public ReactiveSet<PaymentOptions> PaymentOptions
        {

            get
            {

                return _PaymentOptions != null
                    ? _PaymentOptions
                    : ChargingPool.PaymentOptions;

            }

            set
            {

                if (value == ChargingPool.PaymentOptions)
                    return;

                if (PaymentOptions != value)
                {

                    if (_PaymentOptions == null)
                        _PaymentOptions = new ReactiveSet<PaymentOptions>();

                    if (value == null)
                        DeleteProperty(ref _PaymentOptions);

                    else
                        SetProperty(ref _PaymentOptions, value);

                }

            }

        }

        #endregion

        #region Accessibility

        internal AccessibilityTypes _Accessibility;

        [Optional]
        public AccessibilityTypes Accessibility
        {

            get
            {

                return _Accessibility != AccessibilityTypes.Unspecified
                    ? _Accessibility
                    : ChargingPool.Accessibility;

            }

            set
            {

                if (value == ChargingPool.Accessibility)
                    return;

                if (Accessibility != value)
                {

                    SetProperty(ref _Accessibility, value);

                }

            }

        }

        #endregion

        #region HotlinePhoneNumber

        internal String _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the EVSE operator hotline.
        /// </summary>
        [Optional]
        public String HotlinePhoneNumber
        {

            get
            {

                return _HotlinePhoneNumber != null
                    ? _HotlinePhoneNumber
                    : ChargingPool.HotlinePhoneNumber;

            }

            set
            {

                if (value == ChargingPool.HotlinePhoneNumber)
                    return;

                if (HotlinePhoneNumber != value)
                {

                    if (value == null)
                        DeleteProperty(ref _HotlinePhoneNumber);

                    else
                        SetProperty(ref _HotlinePhoneNumber, value);

                }

            }

        }

        #endregion

        #region IsHubjectCompatible

        private Boolean _IsHubjectCompatible;

        [Optional]
        public Boolean IsHubjectCompatible
        {

            get
            {
                return _IsHubjectCompatible;
            }

            set
            {

                if (_IsHubjectCompatible != value)
                    SetProperty<Boolean>(ref _IsHubjectCompatible, value);

            }

        }

        #endregion

        #region DynamicInfoAvailable

        private Boolean _DynamicInfoAvailable;

        [Optional]
        public Boolean DynamicInfoAvailable
        {

            get
            {
                return _DynamicInfoAvailable;
            }

            set
            {

                if (_DynamicInfoAvailable != value)
                    SetProperty<Boolean>(ref _DynamicInfoAvailable, value);

            }

        }

        #endregion


        #region UserComment

        private I18NString _UserComment;

        /// <summary>
        /// A comment from the users.
        /// </summary>
        [Optional]
        public I18NString UserComment
        {

            get
            {
                return _UserComment;
            }

            set
            {
                SetProperty<I18NString>(ref _UserComment, value);
            }

        }

        #endregion

        #region ServiceProviderComment

        private I18NString _ServiceProviderComment;

        /// <summary>
        /// A comment from the service provider.
        /// </summary>
        [Optional]
        public I18NString ServiceProviderComment
        {

            get
            {
                return _ServiceProviderComment;
            }

            set
            {
                SetProperty<I18NString>(ref _ServiceProviderComment, value);
            }

        }

        #endregion

        #region GridConnection

        private GridConnection _GridConnection;

        /// <summary>
        /// The grid connection of the charging station.
        /// </summary>
        [Optional]
        public GridConnection GridConnection
        {

            get
            {
                return _GridConnection;
            }

            set
            {
                SetProperty<GridConnection>(ref _GridConnection, value);
            }

        }

        #endregion

        #region UIFeatures

        private ChargingStationUIFeatures _UIFeatures;

        /// <summary>
        /// The features of the charging station.
        /// </summary>
        [Optional]
        public ChargingStationUIFeatures UIFeatures
        {

            get
            {
                return _UIFeatures;
            }

            set
            {
                SetProperty<ChargingStationUIFeatures>(ref _UIFeatures, value);
            }

        }

        #endregion

        #region AuthorizationOptions

        private AuthorizationOptions _AuthorizationOptions;

        /// <summary>
        /// The authorization options of the charging station.
        /// </summary>
        [Optional]
        public AuthorizationOptions AuthorizationOptions
        {

            get
            {
                return _AuthorizationOptions;
            }

            set
            {
                SetProperty<AuthorizationOptions>(ref _AuthorizationOptions, value);
            }

        }

        #endregion

        #region PhotoURIs

        private ReactiveSet<String> _PhotoURIs;

        /// <summary>
        /// URIs of photos of this charging station.
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


        #region ReservationId

        /// <summary>
        /// The charging reservation identification.
        /// </summary>
        [InternalUseOnly]
        public ChargingReservation_Id ReservationId
        {
            get
            {
                return _Reservation.Id;
            }
        }

        #endregion

        #region Reservation

        private ChargingReservation _Reservation;

        /// <summary>
        /// The charging reservation.
        /// </summary>
        [InternalUseOnly]
        public ChargingReservation Reservation
        {

            get
            {
                return _Reservation;
            }

            set
            {

                if (_Reservation == value)
                    return;

                _Reservation = value;

                //if (_Reservation != null)
                //    SetStatus(EVSEStatusType.Reserved);
                //else
                //    SetStatus(EVSEStatusType.Available);

            }

        }

        #endregion


        #region Status

        /// <summary>
        /// The current charging station status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingStationStatusType> Status
        {
            get
            {
                return _StatusSchedule.CurrentStatus;
            }
        }

        #endregion

        #region StatusSchedule

        private StatusSchedule<ChargingStationStatusType> _StatusSchedule;

        /// <summary>
        /// The charging station status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<ChargingStationStatusType>> StatusSchedule
        {
            get
            {
                return _StatusSchedule;
            }
        }

        #endregion

        #region StatusAggregationDelegate

        private Func<EVSEStatusReport, ChargingStationStatusType> _StatusAggregationDelegate;

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated EVSEs.
        /// </summary>
        public Func<EVSEStatusReport, ChargingStationStatusType> StatusAggregationDelegate
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
        /// The current charging station admin status.
        /// </summary>
        [Optional]
        public Timestamped<ChargingStationAdminStatusType> AdminStatus
        {

            get
            {
                return _AdminStatusSchedule.CurrentStatus;
            }

            set
            {
                SetAdminStatus(value);
            }

        }

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<ChargingStationAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The charging station admin status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<ChargingStationAdminStatusType>> AdminStatusSchedule
        {
            get
            {
                return _AdminStatusSchedule;
            }
        }

        #endregion


        #region ChargingPool

        private readonly ChargingPool _ChargingPool;

        /// <summary>
        /// The charging pool.
        /// </summary>
        public ChargingPool ChargingPool
        {
            get
            {
                return _ChargingPool;
            }
        }

        #endregion

        #region EVSEs

        private readonly ConcurrentDictionary<EVSE_Id, EVSE> _EVSEs;

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging station.
        /// </summary>
        public IEnumerable<EVSE> EVSEs
        {
            get
            {
                return _EVSEs.Select(KVP => KVP.Value);
            }
        }

        #endregion

        #region EVSEIds

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment (EVSEs)
        /// present within this charging station.
        /// </summary>
        public IEnumerable<EVSE_Id> EVSEIds
        {
            get
            {
                return _EVSEs.Values.Select(v => v.Id);
            }
        }

        #endregion

        #endregion

        #region Events

        // ChargingStation events

        #region OnStatusChanged

        /// <summary>
        /// A delegate called whenever the dynamic status changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old timestamped status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station.</param>
        public delegate void OnStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<ChargingStationStatusType> OldStatus, Timestamped<ChargingStationStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status changed.
        /// </summary>
        public event OnStatusChangedDelegate OnStatusChanged;

        #endregion

        #region OnAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the admin status changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old timestamped admin status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging station.</param>
        public delegate void OnAdminStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<ChargingStationAdminStatusType> OldStatus, Timestamped<ChargingStationAdminStatusType> NewStatus);

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnAdminStatusChangedDelegate OnAdminStatusChanged;

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
        /// Create a new charging station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station pool.</param>
        /// <param name="ChargingPool">The parent charging pool.</param>
        /// <param name="MaxStationStatusListSize">The default size of the charging station (aggregated EVSE) status list.</param>
        /// <param name="MaxStationAdminStatusListSize">The default size of the charging station admin status list.</param>
        internal ChargingStation(ChargingStation_Id  Id,
                                 ChargingPool        ChargingPool,
                                 UInt16              MaxStationStatusListSize       = DefaultMaxStationStatusListSize,
                                 UInt16              MaxStationAdminStatusListSize  = DefaultMaxStationAdminStatusListSize)

            : base(Id)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException("ChargingPool", "The charging pool must not be null!");

            #endregion

            #region Init data and properties

            this._ChargingPool            = ChargingPool;

            this._EVSEs                   = new ConcurrentDictionary<EVSE_Id, EVSE>();

            this.Name                     = new I18NString();
            this.Description              = new I18NString();

            this._UserComment             = new I18NString();
            this._ServiceProviderComment  = new I18NString();
            //this.GeoLocation             = new GeoCoordinate();

            this._PaymentOptions          = new ReactiveSet<PaymentOptions>();

            this._StatusSchedule          = new StatusSchedule<ChargingStationStatusType>(MaxStationStatusListSize);
            this._StatusSchedule.Insert(ChargingStationStatusType.Unspecified);

            this._AdminStatusSchedule     = new StatusSchedule<ChargingStationAdminStatusType>(MaxStationAdminStatusListSize);
            this._AdminStatusSchedule.Insert(ChargingStationAdminStatusType.Operational);

            #endregion

            #region Init events

            // ChargingStation events
            this.EVSEAddition             = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval              = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events
            this.SocketOutletAddition     = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval      = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            #endregion

            #region Link events

            this._StatusSchedule.     OnStatusChanged += (Timestamp, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, OldStatus, NewStatus);

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, OldStatus, NewStatus);

            // ChargingStation events
            this.OnEVSEAddition.           OnVoting       += (timestamp, station, evse, vote)      => ChargingPool.EVSEAddition.           SendVoting      (timestamp, station, evse, vote);
            this.OnEVSEAddition.           OnNotification += (timestamp, station, evse)            => ChargingPool.EVSEAddition.           SendNotification(timestamp, station, evse);

            this.OnEVSERemoval.            OnVoting       += (timestamp, station, evse, vote)      => ChargingPool.EVSERemoval .           SendVoting      (timestamp, station, evse, vote);
            this.OnEVSERemoval.            OnNotification += (timestamp, station, evse)            => ChargingPool.EVSERemoval .           SendNotification(timestamp, station, evse);

            // EVSE events
            this.SocketOutletAddition.     OnVoting       += (timestamp, evse, outlet, vote)       => ChargingPool.SocketOutletAddition.   SendVoting      (timestamp, evse, outlet, vote);
            this.SocketOutletAddition.     OnNotification += (timestamp, evse, outlet)             => ChargingPool.SocketOutletAddition.   SendNotification(timestamp, evse, outlet);

            this.SocketOutletRemoval.      OnVoting       += (timestamp, evse, outlet, vote)       => ChargingPool.SocketOutletRemoval.    SendVoting      (timestamp, evse, outlet, vote);
            this.SocketOutletRemoval.      OnNotification += (timestamp, evse, outlet)             => ChargingPool.SocketOutletRemoval.    SendNotification(timestamp, evse, outlet);

            #endregion

        }

        #endregion


        #region CreateNewEVSE(EVSEId, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the new EVSE.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the EVSE.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        public EVSE CreateNewEVSE(EVSE_Id                           EVSEId,
                                  Action<EVSE>                      Configurator  = null,
                                  Action<EVSE>                      OnSuccess     = null,
                                  Action<ChargingStation, EVSE_Id>  OnError       = null)
        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException("EVSEId", "The given EVSE identification must not be null!");

            if (_EVSEs.ContainsKey(EVSEId))
            {
                if (OnError == null)
                    throw new EVSEAlreadyExistsInStation(EVSEId, this.Id);
                else
                    OnError.FailSafeInvoke(this, EVSEId);
            }

            #endregion

            var Now   = DateTime.Now;
            var _EVSE = new EVSE(EVSEId, this);

            Configurator.FailSafeInvoke(_EVSE);

            if (EVSEAddition.SendVoting(Now, this, _EVSE))
            {
                if (_EVSEs.TryAdd(EVSEId, _EVSE))
                {

                    _EVSE.OnPropertyChanged     += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                                                    => UpdateEVSEData       (Timestamp, Sender as EVSE, PropertyName, OldValue, NewValue);

                    _EVSE.OnStatusChanged       += (Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus)
                                                    => UpdateEVSEStatus     (Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus);

                    _EVSE.OnAdminStatusChanged  += (Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus)
                                                    => UpdateEVSEAdminStatus(Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus);

                    OnSuccess.FailSafeInvoke(_EVSE);
                    EVSEAddition.SendNotification(Now, this, _EVSE);
                    UpdateEVSEStatus(Now, _EVSE, new Timestamped<EVSEStatusType>(Now, EVSEStatusType.Unspecified), _EVSE.Status);

                    return _EVSE;

                }
            }

            Debug.WriteLine("EVSE '" + EVSEId + "' was not created!");
            return null;

        }

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the charging station.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)
        {
            return _EVSEs.ContainsKey(EVSE.Id);
        }

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the charging station.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)
        {
            return _EVSEs.ContainsKey(EVSEId);
        }

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)
        {

            EVSE _EVSE = null;

            if (_EVSEs.TryGetValue(EVSEId, out _EVSE))
                return _EVSE;

            return null;

        }

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {
            return _EVSEs.TryGetValue(EVSEId, out EVSE);
        }

        #endregion

        #region RemoveEVSE(EVSEId)

        public EVSE RemoveEVSE(EVSE_Id EVSEId)
        {

            EVSE _EVSE = null;

            if (_EVSEs.TryRemove(EVSEId, out _EVSE))
                return _EVSE;

            return null;

        }

        #endregion

        #region TryRemoveEVSE(EVSEId, out EVSE)

        public Boolean TryRemoveEVSE(EVSE_Id EVSEId, out EVSE EVSE)
        {
            return _EVSEs.TryRemove(EVSEId, out EVSE);
        }

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<ChargingStationAdminStatusType>  NewAdminStatus)
        {

            _AdminStatusSchedule.Insert(NewAdminStatus);

        }

        #endregion

        #region SetAdminStatus(Timestamp, NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="NewAdminStatus">A new admin status.</param>
        public void SetAdminStatus(DateTime                        Timestamp,
                                   ChargingStationAdminStatusType  NewAdminStatus)
        {

            _AdminStatusSchedule.Insert(Timestamp, NewAdminStatus);

        }

        #endregion

        #region SetAdminStatus(NewAdminStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped admin status.
        /// </summary>
        /// <param name="NewAdminStatusList">A list of new timestamped admin status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetAdminStatus(IEnumerable<Timestamped<ChargingStationAdminStatusType>>  NewAdminStatusList,
                                   ChangeMethods                                             ChangeMethod = ChangeMethods.Replace)
        {

            _AdminStatusSchedule.Insert(NewAdminStatusList, ChangeMethod);

        }

        #endregion



        #region Reserve(...)

        public async Task<ReservationResult> Reserve(DateTime                 Timestamp,
                                                     CancellationToken        CancellationToken,
                                                     EVSP_Id                  ProviderId,
                                                     ChargingReservation_Id   ReservationId,
                                                     DateTime?                StartTime,
                                                     TimeSpan?                Duration,
                                                     ChargingProduct_Id       ChargingProductId  = null,
                                                     IEnumerable<Auth_Token>  RFIDIds            = null,
                                                     IEnumerable<eMA_Id>      eMAIds             = null,
                                                     IEnumerable<UInt32>      PINs               = null)
        {

            #region Try to remove an existing reservation if this is an update!

            if (ReservationId != null && _Reservation.Id != ReservationId)
            {

                return ReservationResult.UnknownChargingReservationId;

                // Send DeleteReservation event!

            }

            #endregion

            switch (Status.Value)
            {

                case ChargingStationStatusType.OutOfService:
                    return ReservationResult.OutOfService;

                case ChargingStationStatusType.Charging:
                    return ReservationResult.AlreadyInUse;

                case ChargingStationStatusType.Reserved:
                    return ReservationResult.AlreadyReserved;

                case ChargingStationStatusType.Available:

                    this._Reservation = new ChargingReservation(Timestamp,
                                                                StartTime.HasValue ? StartTime.Value : DateTime.Now,
                                                                Duration. HasValue ? Duration. Value : MaxReservationDuration,
                                                                ProviderId,
                                                                ChargingReservationType.AtChargingStation,
                                                                ChargingPool.EVSEOperator.RoamingNetwork,
                                                                ChargingPool.Id,
                                                                Id,
                                                                null,
                                                                ChargingProductId,
                                                                RFIDIds,
                                                                eMAIds,
                                                                PINs);

        //            SetStatus(EVSEStatusType.Reserved);

                    return ReservationResult.Success(_Reservation);

                default:
                    return ReservationResult.Error();

            }

        }

        #endregion

        #region RemoteStart(Timestamp, CancellationToken, ChargingStationId, ChargingProductId, ReservationId, SessionId, eMAId)

        /// <summary>
        /// Initiate a remote start of the given charging session at the given charging station
        /// and for the given provider/eMAId.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <returns>A RemoteStartResult task.</returns>
        public async Task<RemoteStartChargingStationResult> RemoteStart(DateTime                Timestamp,
                                                                        CancellationToken       CancellationToken,
                                                                        ChargingStation_Id      ChargingStationId,
                                                                        ChargingProduct_Id      ChargingProductId,
                                                                        ChargingReservation_Id  ReservationId,
                                                                        ChargingSession_Id      SessionId,
                                                                        eMA_Id                  eMAId)
        {

            if (_RemoteChargingStation == null)
                return RemoteStartChargingStationResult.Offline;

            return await _RemoteChargingStation.
                             RemoteStart(Timestamp,
                                         CancellationToken,
                                         ChargingStationId,
                                         ChargingProductId,
                                         ReservationId,
                                         SessionId,
                                         eMAId);

        }

        #endregion

        #region RemoteStop(Timestamp, CancellationToken, ChargingStationId, ReservationHandling, SessionId)

        /// <summary>
        /// Initiate a remote stop of the given charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <returns>A RemoteStopResult task.</returns>
        public async Task<RemoteStopChargingStationResult> RemoteStop(DateTime             Timestamp,
                                                                      CancellationToken    CancellationToken,
                                                                      ChargingStation_Id   ChargingStationId,
                                                                      ReservationHandling  ReservationHandling,
                                                                      ChargingSession_Id   SessionId)
        {

            if (RemoteChargingStation == null)
                return RemoteStopChargingStationResult.Offline;

            return await RemoteChargingStation.
                             RemoteStop(Timestamp,
                                        CancellationToken,
                                        ChargingStationId,
                                        ReservationHandling,
                                        SessionId);

        }

        #endregion



        #region (internal) UpdateStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal void UpdateStatus(DateTime                                Timestamp,
                                   Timestamped<ChargingStationStatusType>  OldStatus,
                                   Timestamped<ChargingStationStatusType>  NewStatus)
        {

            var OnAggregatedStatusChangedLocal = OnStatusChanged;
            if (OnAggregatedStatusChangedLocal != null)
                OnAggregatedStatusChangedLocal(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal void UpdateAdminStatus(DateTime                                    Timestamp,
                                        Timestamped<ChargingStationAdminStatusType>  OldStatus,
                                        Timestamped<ChargingStationAdminStatusType>  NewStatus)
        {

            var OnAdminStatusChangedLocal = OnAdminStatusChanged;
            if (OnAdminStatusChangedLocal != null)
                OnAdminStatusChangedLocal(Timestamp, this, OldStatus, NewStatus);

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
        /// Update the current charging station status.
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

            if (StatusAggregationDelegate != null)
                _StatusSchedule.Insert(Timestamp,
                                       StatusAggregationDelegate(new EVSEStatusReport(_EVSEs.Values)));

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station status.
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


        #region IEnumerable<EVSE> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _EVSEs.Values.GetEnumerator();
        }

        public IEnumerator<EVSE> GetEnumerator()
        {
            return _EVSEs.Values.GetEnumerator();
        }

        #endregion


        #region IComparable<ChargingStation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charging station.
            var ChargingStation = Object as ChargingStation;
            if ((Object) ChargingStation == null)
                throw new ArgumentException("The given object is not a charging station!");

            return CompareTo(ChargingStation);

        }

        #endregion

        #region CompareTo(ChargingStation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation">A charging station object to compare with.</param>
        public Int32 CompareTo(ChargingStation ChargingStation)
        {

            if ((Object) ChargingStation == null)
                throw new ArgumentNullException("The given charging station must not be null!");

            return Id.CompareTo(ChargingStation.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStation> Members

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

            // Check if the given object is a charging station.
            var ChargingStation = Object as ChargingStation;
            if ((Object) ChargingStation == null)
                return false;

            return this.Equals(ChargingStation);

        }

        #endregion

        #region Equals(ChargingStation)

        /// <summary>
        /// Compares two charging stations for equality.
        /// </summary>
        /// <param name="ChargingStation">A charging station to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStation ChargingStation)
        {

            if ((Object) ChargingStation == null)
                return false;

            return Id.Equals(ChargingStation.Id);

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
